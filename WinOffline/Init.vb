Imports System.Threading
Imports System.Management
Imports System.Security.Principal

Partial Public Class WinOffline

    Public Class Init

        Public Shared Function Init(ByVal CallStack As String) As Integer

            CallStack += "Init|"

            ' Switch: Command line help
            If Utility.StringArrayContains(Globals.CommandLineArgs, "?", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "help", True) Then
                Globals.RunningAsSystemIdentity = WindowsIdentity.GetCurrent.IsSystem
                If Not Globals.RunningAsSystemIdentity Then
                    If Globals.AttachedtoConsole Then
                        Logger.WriteDebug(Environment.NewLine + HelpUI.rtbHelp.Text)
                    Else
                        Dim CLIHelp As New HelpUI
                        CLIHelp.ShowDialog()
                    End If
                    WindowsAPI.DetachConsole()
                    Environment.Exit(0)
                End If
            End If

            ' Switch: Removal tool execution
            If Utility.StringArrayContains(Globals.CommandLineArgs, "removeitcm", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "uninstallitcm", True) Then
                Try
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    If Utility.IsITCMInstalled Then InitRegistry(CallStack)
                    InitStartupSwitches(CallStack)
                Catch ex As Exception
                    If Not WindowsIdentity.GetCurrent.IsSystem Then
                        If Globals.AttachedtoConsole Then
                            Logger.WriteDebug(CallStack, ex.Message)
                        Else
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)
                        End If
                    End If
                End Try
                RemoveITCM(CallStack)
                DeInit(CallStack, True, False)
                Environment.Exit(0)
            End If

            ' Switch: On-demand CAF stop/start
            If Utility.StringArrayContains(Globals.CommandLineArgs, "stopcaf", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "startcaf", True) Then
                Try
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    If Utility.IsITCMInstalled Then InitRegistry(CallStack)
                    InitStartupSwitches(CallStack)
                Catch ex As Exception
                    If Not WindowsIdentity.GetCurrent.IsSystem Then
                        If Globals.AttachedtoConsole Then
                            Logger.WriteDebug(CallStack, ex.Message)
                        Else
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)
                        End If
                    End If
                    WindowsAPI.DetachConsole()
                    Environment.Exit(0)
                End Try
                If Globals.StopCAFSwitch Then
                    StopCAFOnDemand(CallStack)
                ElseIf Globals.StartCAFSwitch Then
                    StartCAFOnDemand(CallStack)
                End If
                DeInit(CallStack, True, False)
                Environment.Exit(0)
            End If

            ' Switch: Command line database utilities
            If Globals.AttachedtoConsole AndAlso
                (Utility.StringArrayContains(Globals.CommandLineArgs, "testdbconn", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "testconn", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "mdboverview", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "cleanapps", True)) Then
                Try
                    If Not Utility.IsITCMInstalled Then Throw New Exception("ITCM is not installed.")
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    InitRegistry(CallStack)
                    InitComstore(CallStack)
                    Logger.InitDebugLog(CallStack)
                    InitStartupSwitches(CallStack)
                Catch ex As Exception
                    If Not WindowsIdentity.GetCurrent.IsSystem Then
                        If Globals.AttachedtoConsole Then
                            Logger.WriteDebug(CallStack, ex.Message)
                        Else
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)
                        End If
                    End If
                    WindowsAPI.DetachConsole()
                    Environment.Exit(0)
                End Try
                DatabaseAPI.SQLFunctionDispatch(CallStack)
                DeInit(CallStack, True, True)
                Environment.Exit(0)
            End If

            ' Switch: Launch an application
            If Utility.StringArrayContains(Globals.CommandLineArgs, "launch", True) Then
                Try
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    InitStartupSwitches(CallStack)
                    InitResources(CallStack)
                Catch ex As Exception
                    If Not WindowsIdentity.GetCurrent.IsSystem Then
                        If Globals.AttachedtoConsole Then
                            Logger.WriteDebug(CallStack, ex.Message)
                        Else
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)
                        End If
                    End If
                    WindowsAPI.DetachConsole()
                End Try
                LaunchPad(CallStack, Globals.LaunchAppContext, Globals.LaunchAppFileName, FileVector.GetFilePath(Globals.LaunchAppFileName), Globals.LaunchAppArguments)
                DeInit(CallStack, True, False)
                Environment.Exit(0)
            End If

            ' Switch: On-demand software library cleanup execution
            If Globals.AttachedtoConsole AndAlso
                (Utility.StringArrayContains(Globals.CommandLineArgs, "checklibrary", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "cleanlibrary", True)) Then
                Try
                    If Not Utility.IsITCMInstalled Then Throw New Exception("ITCM is not installed.")
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    InitRegistry(CallStack)
                    InitComstore(CallStack)
                    Logger.InitDebugLog(CallStack)
                    InitStartupSwitches(CallStack)
                Catch ex As Exception
                    If Not WindowsIdentity.GetCurrent.IsSystem Then
                        If Globals.AttachedtoConsole Then
                            Logger.WriteDebug(CallStack, ex.Message)
                        Else
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)
                        End If
                    End If
                    WindowsAPI.DetachConsole()
                    Environment.Exit(0)
                End Try
                LibraryManager.RepairLibrary(CallStack)
                DeInit(CallStack, True, True)
                Environment.Exit(0)
            End If

            ' InitProcess()
            Try
                InitProcess(CallStack)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 1.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 1: " + Environment.NewLine +
                                             Globals.ProcessFriendlyName + " process initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 1
            End Try

            ' InitEnvironment()
            Try
                If Utility.IsITCMInstalled Then Globals.ITCMInstalled = True Else Globals.ITCMInstalled = False
                InitEnvironment(CallStack)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " environment initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 2.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 2: " + Environment.NewLine +
                                             Globals.ProcessShortName + " environment initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 2
            End Try

            ' InitRegistry()
            Try
                If Globals.ITCMInstalled Then
                    InitRegistry(CallStack)
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " registry initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 3.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 3: " + Environment.NewLine +
                                             Globals.ProcessShortName + " registry initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 3
            End Try

            ' InitComstore()
            Try
                If Globals.ITCMInstalled Then
                    InitComstore(CallStack)
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " comstore initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 4.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 4: " + Environment.NewLine +
                                             Globals.ProcessShortName + " comstore initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 4
            End Try

            ' IsolationCheck()
            Try
                IsolationCheck(CallStack)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " isolation check failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 5.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 5: " + Environment.NewLine +
                                             Globals.ProcessShortName + " isolation check failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 5
            End Try

            ' InitDebugLog()
            Try
                Logger.InitDebugLog(CallStack)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " debug log initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 6.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 6: " + Environment.NewLine +
                                             Globals.ProcessShortName + " debug log initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 6
            End Try

            ' InitStartupSwitches()
            Try
                InitStartupSwitches(CallStack)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " failed to process startup switches.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 7.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 7: " + Environment.NewLine +
                                             Globals.ProcessShortName + " failed to process startup switches." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 7
            End Try

            ' InitResources()
            Try
                InitResources(CallStack)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " resource extraction failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 8.")
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then
                    AlertBox.CreateUserAlert("Exit code 8: " + Environment.NewLine +
                                             Globals.ProcessShortName + " resource extraction failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)
                End If
                Return 8
            End Try

            Return 0

        End Function

        Public Shared Sub InitProcess(ByVal CallStack As String)

            Dim ProcessWMI As ManagementObject = Nothing
            Dim CurrentID As Integer = Nothing
            Dim ParentID As Integer = Nothing
            Dim ParentName As String = Nothing
            Dim LoopCounter As Integer

            CallStack += "InitProcess|"

            Globals.HostName = My.Computer.Name
            Globals.ProcessIdentity = WindowsIdentity.GetCurrent
            Globals.RunningAsSystemIdentity = WindowsIdentity.GetCurrent.IsSystem
            Globals.ProcessID = Process.GetCurrentProcess.Id

            Logger.WriteDebug(CallStack, "Process name: " + Globals.ProcessShortName)
            Logger.WriteDebug(CallStack, "Version: " + Globals.ProcessFriendlyName + " " + Globals.AppVersion)
            Logger.WriteDebug(CallStack, "Framework version: " + Globals.DotNetVersion)
            Logger.WriteDebug(CallStack, "Hostname: " + My.Computer.Name)
            Logger.WriteDebug(CallStack, "Running as: " + Globals.ProcessIdentity.Name)
            Logger.WriteDebug(CallStack, "PID: " + Globals.ProcessID.ToString)

            ' Read up the parent process tree
            Try
                CurrentID = Globals.ProcessID
                LoopCounter = 0
                While True
                    ProcessWMI = New ManagementObject("Win32_Process.Handle='" & CurrentID & "'")
                    ParentID = ProcessWMI("ParentProcessID")
                    If Not Integer.TryParse(ParentID, Nothing) OrElse ParentID <= 0 Then
                        If LoopCounter = 0 Then
                            Globals.ParentProcessName = "noparent"
                            Exit While
                        Else
                            Exit While
                        End If
                    End If
                    ParentName = Process.GetProcessById(ParentID).ProcessName.ToString
                    Logger.WriteDebug(CallStack, "Parent: " + ParentID.ToString + "/" + ParentName)
                    If LoopCounter = 0 Then Globals.ParentProcessName = ParentName.ToLower
                    Globals.ParentProcessTree.Add(ParentName.ToLower)
                    CurrentID = ParentID
                    LoopCounter += 1
                End While
            Catch ex As Exception
                If Globals.ParentProcessName Is Nothing Then Globals.ParentProcessName = "noparent"
            End Try

            Globals.WorkingDirectory = Globals.ProcessName.Substring(0, Globals.ProcessName.LastIndexOf("\") + 1)
            Logger.WriteDebug(CallStack, "Working directory: " + Globals.WorkingDirectory)
            Logger.WriteDebug(CallStack, "Attached to console: " + Globals.AttachedtoConsole.ToString)

        End Sub

        Public Shared Sub InitEnvironment(ByVal CallStack As String)

            CallStack += "InitEnvironment|"

            ' Lock on a temporary location
            Try
                Globals.WindowsBase = Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine)
                Globals.WinOfflineTemp = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine)
                If Globals.WinOfflineTemp = Nothing OrElse Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                    Globals.WinOfflineTemp = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine)
                    If Globals.WinOfflineTemp = Nothing OrElse Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                        Globals.WinOfflineTemp = Globals.WindowsBase
                        If Globals.WinOfflineTemp = Nothing OrElse Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                            Throw New Exception("Unable to create a temporary folder. " +
                                                Globals.ProcessFriendlyName +
                                                " could not read the system %temp%, %tmp% or %windir% environment variables.")
                        Else
                            Globals.WinOfflineTemp += "\temp"
                            If Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                                Throw New Exception("Unable to create a temporary folder. The %windir% system environment variable is invalid.")
                            End If
                        End If
                    End If
                End If
                Globals.WindowsTemp = Globals.WinOfflineTemp
                Globals.WinOfflineTemp = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName
                Logger.WriteDebug(CallStack, "Temporary folder: " + Globals.WindowsTemp)

            Catch ex As Exception
                Throw New Exception("Unable to create a temporary folder. " +
                                    "An exception was caught while attempting to read system environment variables: " +
                                    Environment.NewLine + Environment.NewLine + ex.Message)
            End Try

            ' Create temporary folder
            Try
                If System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                    Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " folder already exists: " + Globals.WinOfflineTemp)
                Else
                    Logger.WriteDebug(CallStack, "Create folder: " + Globals.WinOfflineTemp)
                    System.IO.Directory.CreateDirectory(Globals.WinOfflineTemp)
                End If
            Catch ex As Exception
                Throw New Exception("Unable to create a temporary folder: " + Environment.NewLine +
                                    Globals.WinOfflineTemp + Environment.NewLine + Environment.NewLine +
                                    "An exception was caught creating the folder: " +
                                    Environment.NewLine + Environment.NewLine + ex.Message)
            End Try

            ' Read component variables
            If Globals.ITCMInstalled Then
                Try
                    Globals.CAMFolder = Environment.GetEnvironmentVariable("CAI_MSQ", EnvironmentVariableTarget.Machine)
                    If Globals.CAMFolder = Nothing OrElse
                        Not System.IO.Directory.Exists(Globals.CAMFolder) Then
                        Throw New Exception("Failed to read CAM folder from environment variable: %CAI_MSQ%")
                    End If
                    Logger.WriteDebug(CallStack, "CAM folder: " + Globals.CAMFolder)
                Catch ex As Exception
                    Throw New Exception("An exception was caught reading the CAM install folder: " +
                                        Environment.NewLine + Environment.NewLine + ex.Message)
                End Try

                Try
                    Globals.SSAFolder = Environment.GetEnvironmentVariable("CSAM_SOCKADAPTER", EnvironmentVariableTarget.Machine)
                    If Globals.SSAFolder = Nothing OrElse
                        Not System.IO.Directory.Exists(Globals.SSAFolder) Then
                        Logger.WriteDebug(CallStack, "Warning: CSAM_SOCKADAPTER environment variable is invalid.")
                        Logger.WriteDebug(CallStack, "Warning: Failed to read SSA folder from environment.")
                    Else
                        Logger.WriteDebug(CallStack, "SSA folder: " + Globals.SSAFolder)
                    End If
                Catch ex As Exception
                    Throw New Exception("An exception was caught reading the SSA install folder: " +
                                        Environment.NewLine + Environment.NewLine + ex.Message)
                End Try

            End If
        End Sub

        Public Shared Sub InitRegistry(ByVal CallStack As String)

            Dim ProductInfoKey As Microsoft.Win32.RegistryKey = Nothing
            Dim CAPKIKey As Microsoft.Win32.RegistryKey = Nothing
            Dim FeatureKeys As String()
            Dim FeatureValue As String

            CallStack += "InitRegistry|"

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)
                If ProductInfoKey Is Nothing Then
                    Throw New Exception("Registry key not found: ComputerAssociates\Unicenter ITRM")
                End If
            End If

            Try
                Globals.ITCMVersion = ProductInfoKey.GetValue("InstallVersion").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallVersion")
            End Try

            Try
                Globals.CAFolder = ProductInfoKey.GetValue("InstallDir").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallDir")
            End Try

            Try
                Globals.DSMFolder = ProductInfoKey.GetValue("InstallDirProduct").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallDirProduct")
            End Try

            Try
                Globals.SharedCompFolder = ProductInfoKey.GetValue("InstallShareDir").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallShareDir")
            End Try
            ProductInfoKey.Close()

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\ComputerAssociates\Shared", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Shared", False)
                If ProductInfoKey Is Nothing Then
                    Throw New Exception("Registry key not found: ComputerAssociates\Shared")
                End If
            End If

            For Each SubKeyName As String In ProductInfoKey.GetSubKeyNames
                If SubKeyName.ToLower.StartsWith("capki") Then
                    If ProductInfoKey.Name.ToLower.Contains("wow6432node") Then
                        CAPKIKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\ComputerAssociates\Shared\" + SubKeyName, False)
                    Else
                        CAPKIKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Shared\" + SubKeyName, False)
                    End If
                End If
            Next
            ProductInfoKey.Close()

            Try
                Globals.CAPKIFolder = CAPKIKey.GetValue("InstallDir").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + CAPKIKey.Name + "\InstallDir")
            End Try
            CAPKIKey.Close()

            Logger.WriteDebug(CallStack, "ITCM version: " + Globals.ITCMVersion)
            Logger.WriteDebug(CallStack, "CA folder: " + Globals.CAFolder)
            Logger.WriteDebug(CallStack, "DSM folder: " + Globals.DSMFolder)
            Logger.WriteDebug(CallStack, "Shared Components folder: " + Globals.SharedCompFolder)
            Logger.WriteDebug(CallStack, "CAPKI Folder: " + Globals.CAPKIFolder)

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)
                If ProductInfoKey Is Nothing Then
                    Throw New Exception("Registry key not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstalledFeatures")
                End If
            End If

            FeatureKeys = ProductInfoKey.GetValueNames
            For Each feature As String In FeatureKeys
                FeatureValue = ProductInfoKey.GetValue(feature)
                If Not feature.Contains("-") Then
                    If Globals.ITCMFunction Is Nothing Then
                        Globals.ITCMFunction = feature
                    Else
                        Globals.ITCMFunction += ", " + feature
                    End If
                End If
                If feature.ToLower.Contains("asset management") And
                    Not Globals.FeatureList.Contains("Asset Management") Then
                    Globals.FeatureList.Add("Asset Management")
                ElseIf feature.ToLower.Contains("software delivery") And
                    Not Globals.FeatureList.Contains("Software Delivery") Then
                    Globals.FeatureList.Add("Software Delivery")
                ElseIf feature.ToLower.Contains("remote control") And
                    Not Globals.FeatureList.Contains("Remote Control") Then
                    Globals.FeatureList.Add("Remote Control")
                ElseIf feature.ToLower.Contains("data transport service") And
                    Not Globals.FeatureList.Contains("Data Transport") Then
                    Globals.FeatureList.Add("Data Transport")
                End If
            Next
            ProductInfoKey.Close()

            If Globals.ITCMFunction.ToLower.Contains("manager") And
                Globals.ITCMFunction.ToLower.Contains("scalability") Then
                Globals.ITCMFunction = "Domain Manager"
            ElseIf Globals.ITCMFunction.ToLower.Contains("manager") Then
                Globals.ITCMFunction = "Enterprise Manager"
            ElseIf Globals.ITCMFunction.ToLower.Contains("scalability") And
                Globals.ITCMFunction.ToLower.Contains("explorer") Then
                Globals.ITCMFunction = "Scalability Server + DSM Explorer"
            ElseIf Globals.ITCMFunction.ToLower.Contains("scalability") Then
                Globals.ITCMFunction = "Scalability Server"
            ElseIf Globals.ITCMFunction.ToLower.Contains("agent") And
                Globals.ITCMFunction.ToLower.Contains("explorer") Then
                Globals.ITCMFunction = "Agent + DSM Explorer"
            ElseIf Globals.ITCMFunction.ToLower.Contains("agent") Then
                Globals.ITCMFunction = "Agent"
            End If
            Logger.WriteDebug(CallStack, "ITCM Function: " + Globals.ITCMFunction)
            Logger.WriteDebug(CallStack, "ITCM Features: " + Utility.ArrayListtoCommaString(Globals.FeatureList, ", "))

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\hostUUID", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\hostUUID", False)
                If ProductInfoKey Is Nothing Then
                    Logger.WriteDebug(CallStack, "Warning: Registry key not found: hostUUID")
                End If
            End If

            If ProductInfoKey IsNot Nothing Then
                Try
                    Globals.HostUUID = ProductInfoKey.GetValue("HostUUID").ToString()
                Catch ex As Exception
                    Globals.HostUUID = "Not found"
                End Try
                Logger.WriteDebug(CallStack, "HostUUID: " + Globals.HostUUID)
            End If
            ProductInfoKey.Close()

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\Software Delivery", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\Software Delivery", False)
            End If
            If ProductInfoKey IsNot Nothing Then
                Try
                    Globals.SDFolder = ProductInfoKey.GetValue("SDROOT").ToString()
                Catch ex As Exception
                    Globals.SDFolder = "Not found"
                End Try
                Logger.WriteDebug(CallStack, "Software Delivery folder: " + Globals.SDFolder)
                ProductInfoKey.Close()
            End If

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)
            End If
            If ProductInfoKey IsNot Nothing Then
                Try
                    Globals.DTSFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.DTSFolder = "Not found"
                End Try
                Logger.WriteDebug(CallStack, "Data transport service folder: " + Globals.DTSFolder)
                ProductInfoKey.Close()
            End If

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\EGC3.0N", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\EGC3.0N", False)
            End If
            If ProductInfoKey IsNot Nothing Then
                Try
                    Globals.EGCFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.EGCFolder = "Not found"
                End Try
                Logger.WriteDebug(CallStack, "EGC folder: " + Globals.EGCFolder)
                ProductInfoKey.Close()
            End If

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)
            End If
            If ProductInfoKey IsNot Nothing Then
                Try
                    Globals.PMLAVersion = ProductInfoKey.GetValue("DisplayVersion").ToString()
                Catch ex As Exception
                    Globals.PMLAVersion = "Not found"
                End Try
                Try
                    Globals.PMLAFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.PMLAFolder = "Not found"
                End Try
                Logger.WriteDebug(CallStack, "Performance lite agent version: " + Globals.PMLAVersion)
                Logger.WriteDebug(CallStack, "Performance lite agent folder: " + Globals.PMLAFolder)
                ProductInfoKey.Close()
            End If

            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\DMPrimer", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\DMPrimer", False)
            End If
            If ProductInfoKey IsNot Nothing Then
                Try
                    Globals.DMPrimerFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.DMPrimerFolder = "Not found"
                End Try
                Logger.WriteDebug(CallStack, "DMPrimer folder: " + Globals.DMPrimerFolder)
                ProductInfoKey.Close()
            End If
        End Sub

        Public Shared Sub InitComstore(ByVal CallStack As String)

            Dim ComstoreString As String
            CallStack += "InitComstore|"

            If Globals.ITCMFunction.ToLower.Contains("manager") OrElse Globals.ITCMFunction.ToLower.Contains("server") Then
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/usd/shared", "ARCHIVE")
                ComstoreString = ComstoreString.Replace(vbCr, "").Replace(vbLf, "")
                If ComstoreString.Contains("\") Then
                    Globals.SDLibraryFolder = ComstoreString
                    Logger.WriteDebug(CallStack, "Software library: " + Globals.SDLibraryFolder)
                End If
            End If

            ComstoreString = ComstoreAPI.GetParameterValue("itrm/common/caf/plugins/encserver", "description")
            If ComstoreString.Contains("(") Then
                ComstoreString = ComstoreString.Substring(ComstoreString.IndexOf("(") + 1)
                If ComstoreString.ToLower.Contains("manager") And
                    ComstoreString.ToLower.Contains("server") And
                    ComstoreString.ToLower.Contains("router") Then
                    Globals.ENCFunction = "ENC Gateway (Manager, Server, Router)"
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)
                ElseIf ComstoreString.ToLower.Contains("manager") And
                    ComstoreString.ToLower.Contains("server") Then
                    Globals.ENCFunction = "ENC Gateway (Manager, Server)"
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)
                ElseIf ComstoreString.ToLower.Contains("server") And
                    ComstoreString.ToLower.Contains("router") Then
                    Globals.ENCFunction = "ENC Gateway (Server, Router)"
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)
                ElseIf ComstoreString.ToLower.Contains("router") Then
                    Globals.ENCFunction = "ENC Gateway (Router)"
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)
                End If
            Else
                Globals.ENCFunction = Nothing
                Logger.WriteDebug(CallStack, "No ENC Gateway Functionality")
            End If
            If Not Globals.ENCFunction = Nothing Then
                Globals.ITCMFunction += " + " + Globals.ENCFunction
            End If

            ComstoreString = ComstoreAPI.GetParameterValue("itrm/common/enc/client", "serveraddress")
            Globals.ENCGatewayServer = ComstoreString
            Logger.WriteDebug(CallStack, "ENC Gateway Server address: " + Globals.ENCGatewayServer)
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/common/enc/client", "servertcpport")
            Globals.ENCServerTCPPort = ComstoreString
            Logger.WriteDebug(CallStack, "ENC Gateway Server port: " + Globals.ENCServerTCPPort)

            ComstoreString = ComstoreAPI.GetParameterValue("itrm/agent/units/.", "currentmanageraddress")
            Globals.DomainManager = ComstoreString
            Logger.WriteDebug(CallStack, "Current domain manager: " + Globals.DomainManager)
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/agent/solutions/generic", "serveraddress")
            Globals.ScalabilityServer = ComstoreString
            Logger.WriteDebug(CallStack, "Current scalability server: " + Globals.ScalabilityServer)
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/agent/solutions/generic", "version")
            Globals.ITCMComstoreVersion = ComstoreString
            Logger.WriteDebug(CallStack, "Agent version: " + Globals.ITCMComstoreVersion)

            If Globals.ITCMFunction.ToLower.Contains("manager") Then
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbmsserver")
                Globals.DatabaseServer = ComstoreString
                Logger.WriteDebug(CallStack, "Database server: " + Globals.DatabaseServer)
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbmsinstance")
                If ComstoreString IsNot Nothing AndAlso ComstoreString.Contains(",") Then
                    Globals.DatabaseInstance = ComstoreString.Substring(0, ComstoreString.IndexOf(","))
                    Globals.DatabasePort = ComstoreString.Substring(ComstoreString.IndexOf(",") + 1)
                Else
                    Globals.DatabaseInstance = ComstoreString
                End If
                Logger.WriteDebug(CallStack, "Database instance: " + ComstoreString)
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbname")
                Globals.DatabaseName = ComstoreString
                Logger.WriteDebug(CallStack, "Database name: " + Globals.DatabaseName)
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbuser")
                Globals.DatabaseUser = ComstoreString
                Logger.WriteDebug(CallStack, "Database user: " + Globals.DatabaseUser)
            End If

        End Sub

        Public Shared Sub IsolationCheck(ByVal CallStack As String)

            Dim clsParentProcessName As String
            Dim LoopCounter As Integer = 0
            CallStack += "InitIsolation|"

            ' More than one of us?
            If Utility.IsProcessRunningCount(Globals.ProcessFriendlyName) > 1 Then
                Logger.WriteDebug(CallStack, "Warning: Detected existing " + Globals.ProcessFriendlyName + " execution in progress.")
                Logger.WriteDebug(CallStack, "Warning: Current execution (this process) may not be the primary process.")
                Logger.WriteDebug(CallStack, "Retrieving list of all running " + Globals.ProcessFriendlyName + " processes..")
                Dim ParallelProcessList As ArrayList = Utility.GetParallelProcesses(Globals.ProcessFriendlyName)

                For Each clsProcess As Process In ParallelProcessList
                    If clsProcess.Id = Globals.ProcessID Then
                        Continue For
                    End If
                    Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " PID: " + clsProcess.Id.ToString)
                    Try
                        Dim clsProcessWMI = New ManagementObject("Win32_Process.Handle='" & clsProcess.Id & "'")
                        Dim clsParentProcessID = clsProcessWMI("ParentProcessID")
                        Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " PPID: " + clsParentProcessID.ToString)
                        clsParentProcessName = System.Diagnostics.Process.GetProcessById(clsParentProcessID).ProcessName.ToString
                        Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " parent name: " + clsParentProcessName)
                    Catch ex As Exception
                        Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " parent process has terminated.")
                        clsParentProcessName = "NoParent"
                    End Try
                Next

                ' Dispatch duplicate process (this process)
                If Globals.ParentProcessTree.Contains("sd_jexec") Then
                    ' Scenario #1: Duplicate process, our parent is software delivery
                    ' --> Signal a re-run request and terminate
                    Logger.WriteDebug(CallStack, "Entry point: Software delivery.")
                    Logger.WriteDebug(CallStack, "Signal re-run request..")
                    Dim ExecutionString = Globals.DSMFolder + "bin\" + "sd_acmd.exe"
                    Dim ArgumentString = "signal rerun"
                    Dim SignalReRunProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                    SignalReRunProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                    SignalReRunProcessStartInfo.UseShellExecute = False
                    SignalReRunProcessStartInfo.RedirectStandardOutput = True
                    SignalReRunProcessStartInfo.CreateNoWindow = True
                    Dim RunningProcess = Process.Start(SignalReRunProcessStartInfo)
                    RunningProcess.WaitForExit()
                    RunningProcess.Close()
                    Logger.WriteDebug(CallStack, "Re-run request sent to software delivery.")
                    Throw New Exception("Scenario #1: Duplicate process, our parent is software delivery. Signal re-run request..")

                ElseIf Globals.ParentProcessName.ToLower.Equals(Globals.ProcessFriendlyName.ToLower) Then
                    ' Scenario #2: Duplicate process, our parent is WinOffline
                    ' --> Finite loop for isolation before continuing, otherwise terminate
                    Logger.WriteDebug(CallStack, "Entry point: " + Globals.ProcessFriendlyName + ".")
                    Thread.Sleep(10000)
                    LoopCounter = 0
                    While Utility.IsProcessRunningCount(Globals.ProcessFriendlyName) > 1
                        Logger.WriteDebug(CallStack, "Parent process is still active.")
                        System.Windows.Forms.Application.DoEvents()
                        Thread.Sleep(10000)
                        LoopCounter += 1
                        If LoopCounter >= 5 Then
                            Logger.WriteDebug(CallStack, "Parent process has not terminated.")
                            Logger.WriteDebug(CallStack, "Exit with isolation check failure..")
                            Throw New Exception("Scenario #2: Duplicate process, our parent is WinOffline. " +
                                                "Finite loop for isolation before continuing, otherwise terminate.")
                        End If
                    End While
                    Logger.WriteDebug(CallStack, "Parent process has terminated.")
                    Logger.WriteDebug(CallStack, "Isolation Check: Passed.")

                ElseIf Globals.ParentProcessName.ToLower.Equals("noparent") Then
                    ' Scenario #3: Duplicate process, our parent is unknown or has terminated.
                    ' --> Finite loop for isolation before continuing, otherwise terminate
                    Logger.WriteDebug(CallStack, "Entry point: No parent.")
                    Thread.Sleep(10000)
                    LoopCounter = 0
                    While Utility.IsProcessRunningCount(Globals.ProcessFriendlyName) > 1
                        Logger.WriteDebug(CallStack, "Parallel process is still active.")
                        System.Windows.Forms.Application.DoEvents()
                        Thread.Sleep(10000)
                        LoopCounter += 1
                        If LoopCounter >= 5 Then
                            Logger.WriteDebug(CallStack, "Parallel process has not terminated.")
                            Logger.WriteDebug(CallStack, "Exit with isolation check failure..")
                            Throw New Exception("Scenario #3: Duplicate process, our parent is unknown or has terminated." +
                                                "Finite loop for isolation before continuing, otherwise terminate.")
                        End If
                    End While
                    Logger.WriteDebug(CallStack, "Parallel process has terminated.")
                    Logger.WriteDebug(CallStack, "Isolation Check: Passed.")

                Else
                    ' Scenario #4: Duplicate process launched by user or script
                    ' --> Terminate
                    Logger.WriteDebug(CallStack, "Entry point: Standalone execution.")
                    Logger.WriteDebug(CallStack, "Exit with isolation check failure..")
                    Throw New Exception("Scenario #4: Duplicate process launched by user or script. Terminate.")
                End If

            Else
                Logger.WriteDebug(CallStack, "No parallel " + Globals.ProcessShortName + " processes detected.")
            End If

        End Sub

        Public Shared Sub InitStartupSwitches(ByVal CallStack As String)

            Dim RemovePatchString As String
            Dim IndividualPatch As String()
            Dim rVector As RemovalVector
            Dim RemovePatchSwitchError As Boolean = False

            CallStack += "InitSwitch|"

            ' Iterate command line args
            For i As Integer = 0 To Globals.CommandLineArgs.Length - 1
                If Globals.CommandLineArgs(i).ToString.ToLower.Equals("backout") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-backout") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/backout") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("remove") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-remove") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/remove") Then
                    If i + 1 <= Globals.CommandLineArgs.Length - 1 Then
                        RemovePatchString = Globals.CommandLineArgs(i + 1)
                        If RemovePatchString.Contains(",") Then
                            IndividualPatch = RemovePatchString.Split(",")
                            For Each strLine As String In IndividualPatch
                                rVector = New RemovalVector(strLine)
                                Manifest.UpdateManifest(CallStack, Manifest.REMOVAL_MANIFEST, rVector)
                            Next
                        Else
                            rVector = New RemovalVector(RemovePatchString)
                            Manifest.UpdateManifest(CallStack, Manifest.REMOVAL_MANIFEST, rVector)
                        End If
                        Globals.RemovePatchSwitch = True
                        Logger.WriteDebug(CallStack, "Switch: Remove patches.")
                        i += 1
                        If i = Globals.CommandLineArgs.Length - 1 Then Exit For
                    Else
                        Logger.WriteDebug(CallStack, "Switch: Remove patch switch is not followed by a patch name.")
                        RemovePatchSwitchError = True
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanlogs") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanlogs") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanlogs") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanlog") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanlog") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanlog") Then
                    Globals.CleanupLogsSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Cleanup DSM logs folder.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("resetcftrace") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-resetcftrace") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/resetcftrace") Then
                    Globals.ResetCftraceSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Reset the cftrace level.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("rmcamcfg") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-rmcamcfg") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/rmcamcfg") Then
                    Globals.RemoveCAMConfigSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Remove CAM configuration file.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanagent") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanagent") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanagent") Then
                    Globals.CleanupAgentSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Perform agent cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanserver") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanserver") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanserver") Then
                    Globals.CleanupServerSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Perform scalability server cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanlibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanlibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanlibrary") Then
                    Globals.CleanupSDLibrarySwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Perform software library cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("checklibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-checklibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/checklibrary") Then
                    Globals.CheckSDLibrarySwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Analyze software library without making changes.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleancerts") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleancerts") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleancerts") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleancert") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleancert") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleancert") Then
                    Globals.CleanupCertStoreSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Certificate store cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipcafstartup") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipcafstartup") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipcafstartup") Then
                    Globals.SkipCAFStartUpSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Don't start CAF.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipcam") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipcam") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipcam") Then
                    Globals.SkipCAM = True
                    Logger.WriteDebug(CallStack, "Switch: Don't stop CAM service.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipdmprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipdmprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipdmprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipprimer") Then
                    Globals.SkipDMPrimer = True
                    Logger.WriteDebug(CallStack, "Switch: Don't stop DMPrimer service.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skiphm") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skiphm") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skiphm") Then
                    Globals.SkiphmAgent = True
                    Logger.WriteDebug(CallStack, "Switch: Don't stop hmAgent.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("loadgui") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-loadgui") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/loadgui") Then
                    Globals.LaunchGuiSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Launch DSM Explorer.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("simulatestop") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-simulatestop") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/simulatestop") Then
                    Globals.SimulateCafStopSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Simulate recycling CAF.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("simulatepatch") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-simulatepatch") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/simulatepatch") Then
                    Globals.SimulatePatchSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Simulate patch operations.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("simulatepatcherror") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-simulatepatcherror") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/simulatepatcherror") Then
                    Globals.SimulatePatchErrorSwitch = True
                    Globals.SimulatePatchSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Simulate patching error.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("rmhistorybefore") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-rmhistorybefore") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/rmhistorybefore") Then
                    Globals.RemoveHistoryBeforeSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Remove patch history file, BEFORE patch operations.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("rmhistoryafter") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-rmhistoryafter") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/rmhistoryafter") Then
                    Globals.RemoveHistoryAfterSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Remove patch history file, AFTER patch operations.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dumpcazipxp") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dumpcazipxp") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dumpcazipxp") Then
                    Globals.DumpCazipxpSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Extract cazipxp.exe utility and exit without changes.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("regenuuid") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-regenuuid") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/regenuuid") Then
                    Globals.RegenHostUUIDSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Regenerate agent HostUUID.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("signalreboot") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-signalreboot") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/signalreboot") Then
                    Globals.SDSignalRebootSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Signal software delivery reboot request.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("reboot") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-reboot") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/reboot") Then
                    Globals.RebootSystemSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Reboot system after completion.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("uninstallitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-uninstallitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/uninstallitcm") Then
                    Globals.UninstallITCM = True
                    Logger.WriteDebug(CallStack, "Switch: Perform normal uninstall of ITCM.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("removeitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-removeitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/removeitcm") Then
                    Globals.RemoveITCM = True
                    Logger.WriteDebug(CallStack, "Switch: Perform full removal of ITCM.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("keepuuid") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-keepuuid") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/keepuuid") Then
                    Globals.KeepHostUUIDSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: If -removeitcm was specified, retain the HostUUID registry key.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("disableenc") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-disableenc") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/disableenc") Then
                    Globals.DisableENCSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Disable ENC.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("gethistory") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-gethistory") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/gethistory") Then
                    Globals.GetHistorySwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Report agent patch history only.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("stopcaf") Or
                Globals.CommandLineArgs(i).ToString.ToLower.Equals("-stopcaf") Or
                Globals.CommandLineArgs(i).ToString.ToLower.Equals("/stopcaf") Then
                    Globals.StopCAFSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Stop CAF service on-demand.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("startcaf") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-startcaf") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/startcaf") Then
                    Globals.StartCAFSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Start CAF service on-demand.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbserver") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbserver") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbserver") Then
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then
                        Globals.DatabaseServer = Globals.CommandLineArgs(i + 1).ToString
                        i += 1
                        Logger.WriteDebug(CallStack, "Switch: Database server provided.")
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Database server not provided.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbinstance") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbinstance") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbinstance") Then
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then
                        Globals.DatabaseInstance = Globals.CommandLineArgs(i + 1).ToString
                        i += 1
                        Logger.WriteDebug(CallStack, "Switch: Database instance provided.")
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Database instance not provided.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbport") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbport") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbport") Then
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then
                        Globals.DatabasePort = Globals.CommandLineArgs(i + 1).ToString
                        i += 1
                        Logger.WriteDebug(CallStack, "Switch: Database port provided.")
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Database port not provided.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbname") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbname") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbname") Then
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then
                        Globals.DatabaseName = Globals.CommandLineArgs(i + 1).ToString
                        i += 1
                        Logger.WriteDebug(CallStack, "Switch: Database name provided.")
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Database name not provided.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbuser") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbuser") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbuser") Then
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then
                        Globals.DbUser = Globals.CommandLineArgs(i + 1).ToString
                        i += 1
                        Logger.WriteDebug(CallStack, "Switch: Database username provided.")
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Database username not provided.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbpassword") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbpassword") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbpassword") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbpw") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbpw") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbpw") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbpass") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbpass") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbpass") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbpasswd") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbpasswd") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbpasswd") Then
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then
                        Globals.DbPassword = Globals.CommandLineArgs(i + 1).ToString
                        i += 1
                        Logger.WriteDebug(CallStack, "Switch: Database password provided.")
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Database password was not provided.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("testdbconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-testdbconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/testdbconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("testconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-testconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/testconn") Then
                    Globals.DbTestConnectionSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Test database connection.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("mdboverview") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-mdboverview") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/mdboverview") Then
                    Globals.MdbOverviewSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Provide database overview.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanapps") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanapps") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanapps") Then
                    Globals.MdbCleanAppsSwitch = True
                    Logger.WriteDebug(CallStack, "Switch: Execute database cleanapps script.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("launch") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-launch") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/launch") Then
                    If i + 2 < Globals.CommandLineArgs.Length AndAlso
                        Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                            Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) AndAlso
                        Not (Globals.CommandLineArgs(i + 2).ToString.StartsWith("/") OrElse
                            Globals.CommandLineArgs(i + 2).ToString.StartsWith("-")) Then
                        Globals.LaunchAppContext = Globals.CommandLineArgs(i + 1).ToString
                        Globals.LaunchAppFileName = Globals.CommandLineArgs(i + 2).ToString
                        i += 3
                        While (i < Globals.CommandLineArgs.Length)
                            Globals.LaunchAppArguments += Globals.CommandLineArgs(i).ToString + " "
                            i += 1
                        End While
                        Globals.LaunchAppSwitch = True
                        Logger.WriteDebug(CallStack, "Switch: Launch application.")
                    Else
                        Throw New ArgumentException("Insufficient parameters provided for launch.")
                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("?") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-" + "?") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("--" + "?") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/" + "?") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("help") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-" + "help") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("--" + "help") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/" + "help") Then
                    If Not Globals.RunningAsSystemIdentity Then
                        If Globals.AttachedtoConsole Then
                            Logger.WriteDebug(HelpUI.rtbHelp.Text)
                        Else
                            Dim CLIHelp As New HelpUI
                            CLIHelp.ShowDialog()
                        End If
                        DeInit(CallStack, True, False)
                        Environment.Exit(0)
                    End If
                Else
                    If Globals.AttachedtoConsole Then
                        Throw New ArgumentException("Unknown switch specified-- " + Globals.CommandLineArgs(i).ToString)
                    Else
                        Logger.WriteDebug(CallStack, "Warning: Unknown switch specified-- " + Globals.CommandLineArgs(i).ToString)
                    End If
                End If
            Next

            If RemovePatchSwitchError Then
                Throw New ArgumentException("Remove switch specified, but no patch name has been specified." +
                                            Environment.NewLine + Environment.NewLine +
                                            "Syntax: " + Environment.NewLine + Globals.ProcessShortName + " -remove T5X0001" + Environment.NewLine +
                                            "  OR" + Environment.NewLine + Globals.ProcessShortName + " -remove T5X0001,T5X0002" + Environment.NewLine)
            End If

            If Globals.ParentProcessName.ToLower.Equals("cmd") AndAlso
                Globals.WorkingDirectory.ToLower.Contains("\dmprimer\caunicenterdsm\") AndAlso
                Globals.WorkingDirectory.ToLower.Contains("\testfixes\") Then
                Globals.SkipCAM = True ' Set implied switches (don't stop CAM or DMPrimer services)
                Globals.SkipDMPrimer = True ' Note: Stopping these services will interrupt infrastructure deployment process
                Logger.WriteDebug(CallStack, "Implied Switch: Don't stop CAM service.")
                Logger.WriteDebug(CallStack, "Implied Switch: Don't stop DMPrimer service.")
            End If

            If Globals.CommandLineArgs.Length = 0 Then
                Logger.WriteDebug(CallStack, "No startup switches specified.")
            End If

        End Sub

        Public Shared Sub InitResources(ByVal CallStack As String)

            Dim Source As String
            Dim Destination As String
            CallStack += "InitResource|"

            Try
                Globals.MyAssembly = Reflection.Assembly.GetExecutingAssembly()
                Globals.MyRoot = Globals.MyAssembly.GetName().Name

                If Not System.IO.File.Exists(Globals.WorkingDirectory + "cazipxp.exe") Then
                    Logger.WriteDebug(CallStack, "Extract: " + Globals.WinOfflineTemp + "\cazipxp.exe")

                    Globals.MyResourceStream = Globals.MyAssembly.GetManifestResourceStream(Globals.MyRoot + "." + "Cazipxp.exe")
                    ReDim Globals.MyBuffer(Convert.ToInt32(Globals.MyResourceStream.Length) - 1)
                    Globals.MyResourceStream.Read(Globals.MyBuffer, 0, Globals.MyBuffer.Length)
                    Globals.MyResourceStream.Close()

                    If Globals.DumpCazipxpSwitch Then
                        Globals.MyFileStream = New System.IO.FileStream(Globals.WorkingDirectory +
                                                                        "\Cazipxp.exe",
                                                                        System.IO.FileMode.Create,
                                                                        System.IO.FileAccess.Write)
                    Else
                        Utility.DeleteFile(CallStack, Globals.WinOfflineTemp + "\Cazipxp.exe")
                        Globals.MyFileStream = New System.IO.FileStream(Globals.WinOfflineTemp +
                                                                        "\Cazipxp.exe",
                                                                        System.IO.FileMode.Create,
                                                                        System.IO.FileAccess.Write)
                    End If

                    Globals.MyFileStream.Write(Globals.MyBuffer, 0, Globals.MyBuffer.Length)
                    Globals.MyFileStream.Close()

                Else
                    Logger.WriteDebug(CallStack, "Alternative resource provided: " + Globals.WorkingDirectory + "cazipxp.exe")

                    Source = Globals.WorkingDirectory + "cazipxp.exe"
                    Destination = Globals.WinOfflineTemp + "\cazipxp.exe"

                    Logger.WriteDebug(CallStack, "Copy file: " + Source)
                    Logger.WriteDebug(CallStack, "To: " + Destination)

                    System.IO.File.Copy(Source, Destination, True)
                End If

                If Not System.IO.File.Exists(Globals.WorkingDirectory + "LaunchService.exe") Then
                    Logger.WriteDebug(CallStack, "Extract: " + Globals.WinOfflineTemp + "\LaunchService.exe")

                    Globals.MyResourceStream = Globals.MyAssembly.GetManifestResourceStream(Globals.MyRoot + "." + "LaunchService.exe")
                    ReDim Globals.MyBuffer(Convert.ToInt32(Globals.MyResourceStream.Length) - 1)
                    Globals.MyResourceStream.Read(Globals.MyBuffer, 0, Globals.MyBuffer.Length)
                    Globals.MyResourceStream.Close()

                    Utility.DeleteFile(CallStack, Globals.WinOfflineTemp + "\LaunchService.exe")

                    Globals.MyFileStream = New System.IO.FileStream(Globals.WinOfflineTemp + "\LaunchService.exe", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Globals.MyFileStream.Write(Globals.MyBuffer, 0, Globals.MyBuffer.Length)
                    Globals.MyFileStream.Close()
                Else
                    Logger.WriteDebug(CallStack, "Alternative resource provided: " + Globals.WorkingDirectory + "LaunchService.exe")

                    Source = Globals.WorkingDirectory + "LaunchService.exe"
                    Destination = Globals.WinOfflineTemp + "\LaunchService.exe"

                    Logger.WriteDebug(CallStack, "Copy file: " + Source)
                    Logger.WriteDebug(CallStack, "To: " + Destination)

                    System.IO.File.Copy(Source, Destination, True)
                End If

                If Not System.IO.File.Exists(Globals.WorkingDirectory + Globals.ProcessShortName + ".config") Then
                    Logger.WriteDebug(CallStack, "Extract: " + Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")

                    Globals.MyResourceStream = Globals.MyAssembly.GetManifestResourceStream(Globals.MyRoot + "." + "app.config")
                    ReDim Globals.MyBuffer(Convert.ToInt32(Globals.MyResourceStream.Length) - 1)
                    Globals.MyResourceStream.Read(Globals.MyBuffer, 0, Globals.MyBuffer.Length)
                    Globals.MyResourceStream.Close()

                    Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")

                    Globals.MyFileStream = New System.IO.FileStream(Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Globals.MyFileStream.Write(Globals.MyBuffer, 0, Globals.MyBuffer.Length)
                    Globals.MyFileStream.Close()
                End If

                If Globals.DumpCazipxpSwitch Then
                    DeInit(CallStack, True, False)
                    Environment.Exit(0)
                End If

            Catch ex As Exception
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Error: Exception caught extracting embedded resources, cleaning up..")
                DeInit(CallStack, True, True)
                Throw New Exception("Exception caught extracting embedded resources:" +
                                    Environment.NewLine + Environment.NewLine + ex.Message)
            End Try
        End Sub

        ' Reinitialization function 
        '   For stage I reinitialization in software delivery mode.
        '   Used for cleaning up after prior execution failures, 
        '     or incomplete executions where stage III was never run.
        '   This is detected when we process the SD job container, and the cached
        '     ID for the job doesn't match the current ID (JobContainer.vb).
        Public Shared Function ReInit(ByVal CallStack As String) As Integer

            CallStack += "ReInit|"

            ' Delete temp folder
            Try
                Logger.WriteDebug(CallStack, "Delete Folder: " + Globals.WinOfflineTemp)
                System.IO.Directory.Delete(Globals.WinOfflineTemp, True)
                Logger.WriteDebug(CallStack, "Temporary folder deleted.")
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught deleting temporary folder.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder..")
                Utility.DeleteFolderContents(CallStack, Globals.WinOfflineTemp, Nothing)
            End Try

            ' Recreate it
            Try
                Logger.WriteDebug(CallStack, "Create " + Globals.ProcessFriendlyName + " folder..")
                If System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                    Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " folder already exists: " + Globals.WinOfflineTemp)
                Else
                    Logger.WriteDebug(CallStack, "Create folder: " + Globals.WinOfflineTemp)
                    System.IO.Directory.CreateDirectory(Globals.WinOfflineTemp)
                End If
            Catch ex As Exception
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Exception caught creating temporary folder.")
                Return 1
            End Try

            ' Re-extract resources
            InitResources(CallStack)

            ' Clear manifests
            Logger.WriteDebug(CallStack, "Flush ALL manifest data..")
            Manifest.FlushManifestData()
            Logger.WriteDebug(CallStack, "Manifest data has been RESET.")

            ' Re-init debug log
            Logger.WriteDebug(CallStack, "Reinitialize debug log..")
            Logger.ReInitDebugLog()
            Logger.WriteDebug(CallStack, "Debug log reinitialized.")

            Return 0

        End Function

        Public Shared Sub DeInit(ByVal CallStack As String, ByVal BypassSummary As Boolean, ByVal KeepDebugLog As Boolean)

            Dim FileList As String()
            Dim Today As String = DateTime.Now.ToString("yyyyMMdd")

            CallStack += "DeInit|"

            ' Delete temp folder
            Try
                If Globals.WinOfflineTemp IsNot Nothing AndAlso System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                    Logger.WriteDebug(CallStack, "Delete Folder: " + Globals.WinOfflineTemp)
                    System.IO.Directory.Delete(Globals.WinOfflineTemp, True)
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught deleting temporary folder.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder..")
                FileList = System.IO.Directory.GetFiles(Globals.WinOfflineTemp)
                For n As Integer = 0 To FileList.Length - 1
                    Utility.DeleteFile(CallStack, FileList(n))
                Next
                Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder completed.")
            End Try

            ' Delete WinOffline and config file
            Try
                Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName)
                Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught deleting file: " + Globals.WindowsTemp + "\" + Globals.ProcessShortName)
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            End Try

            ' Remove prior debug logs (anything not from today)
            Try
                If System.IO.Directory.Exists(Globals.DSMFolder) Then
                    FileList = System.IO.Directory.GetFiles(Globals.DSMFolder)
                    For n As Integer = 0 To FileList.Length - 1
                        If FileList(n).ToLower.Contains(Globals.ProcessFriendlyName.ToLower) And
                                (FileList(n).ToLower.EndsWith(".debug") Or
                                FileList(n).ToLower.EndsWith(".log") Or
                                FileList(n).ToLower.EndsWith(".txt")) Then
                            If Not System.IO.File.GetCreationTime(FileList(n)).ToString("yyyyMMdd").Equals(Today) Then
                                Utility.DeleteFile(CallStack, FileList(n))
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Warning: Exception caught cleaning prior debug logs.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            End Try

            If BypassSummary = False Then
                Logger.InsertSummary(CallStack)
                Logger.WriteDebug(CallStack, "Patch summary written.")
            End If

            If Manifest.FlushManifestData() Then
                Logger.WriteDebug(CallStack, "Manifest data has been RESET.")
            End If

            Logger.TermDebugLog(CallStack, KeepDebugLog)
            Logger.SetCurrentTask("Completed.")
            Logger.WriteDebug(CallStack, "Program terminated.")

            If BypassSummary = False Then
                Logger.WriteDebug(Globals.ProgressGUISummary)
            End If

            WindowsAPI.DetachConsole()

        End Sub

    End Class

End Class
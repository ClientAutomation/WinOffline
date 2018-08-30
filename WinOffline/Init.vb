Imports System.Threading
Imports System.Management
Imports System.Security.Principal

Partial Public Class WinOffline

    Public Class Init

        Public Shared Function Init(ByVal CallStack As String) As Integer

            ' Update call stack
            CallStack += "Init|"

            ' *****************************
            ' - Switch: Command line help.
            ' *****************************

            ' Check for help switch
            If Utility.StringArrayContains(Globals.CommandLineArgs, "?", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "help", True) Then

                ' Set identity flag
                Globals.RunningAsSystemIdentity = WindowsIdentity.GetCurrent.IsSystem

                ' Verify we're not running as SYSTEM
                If Not Globals.RunningAsSystemIdentity Then

                    ' Check if attached to console
                    If Globals.AttachedtoConsole Then

                        ' Write help text
                        Logger.WriteDebug(Environment.NewLine + HelpUI.rtbHelp.Text)

                    Else

                        ' Show the about box
                        Dim CLIHelp As New HelpUI
                        CLIHelp.ShowDialog()

                    End If

                    ' Detach console
                    WindowsAPI.DetachConsole()

                    ' Hard exit
                    Environment.Exit(0)

                End If

            End If

            ' *****************************
            ' - Switch: Removal tool execution.
            ' *****************************

            ' Check for removal tool switches
            If Utility.StringArrayContains(Globals.CommandLineArgs, "removeitcm", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "uninstallitcm", True) Then

                ' Encacpsulate express initialization
                Try

                    ' Express initialization
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    If Utility.IsITCMInstalled Then InitRegistry(CallStack)
                    InitStartupSwitches(CallStack)

                Catch ex As Exception

                    ' Verify we're not running as SYSTEM
                    If Not WindowsIdentity.GetCurrent.IsSystem Then

                        ' Check if attached to console
                        If Globals.AttachedtoConsole Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, ex.Message)

                        Else

                            ' Report initialization exception
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)

                        End If

                    End If

                End Try

                ' Remove ITCM
                RemoveITCM(CallStack)

                ' Deinitialize
                DeInit(CallStack, True, False)

                ' Hard exit
                Environment.Exit(0)

            End If

            ' *****************************
            ' - Switch: On-demand CAF stop/start.
            ' *****************************

            ' Check for caf on-demand stop/start switches
            If Utility.StringArrayContains(Globals.CommandLineArgs, "stopcaf", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "startcaf", True) Then

                ' Encapsulate express initialization
                Try

                    ' Express initialization
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    If Utility.IsITCMInstalled Then InitRegistry(CallStack)
                    InitStartupSwitches(CallStack)

                Catch ex As Exception

                    ' Verify we're not running as SYSTEM
                    If Not WindowsIdentity.GetCurrent.IsSystem Then

                        ' Check if attached to console
                        If Globals.AttachedtoConsole Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, ex.Message)

                        Else

                            ' Report initialization exception
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)

                        End If

                    End If

                    ' Detach from console
                    WindowsAPI.DetachConsole()

                    ' Hard exit
                    Environment.Exit(0)

                End Try

                ' Check for caf on-demand switches
                If Globals.StopCAFSwitch Then

                    ' Stop CAF
                    StopCAFOnDemand(CallStack)

                ElseIf Globals.StartCAFSwitch Then

                    ' Start CAF
                    StartCAFOnDemand(CallStack)

                End If

                ' Deinitialize
                DeInit(CallStack, True, False)

                ' Hard exit
                Environment.Exit(0)

            End If

            ' *****************************
            ' - Switch: Command line database utilities.
            ' *****************************

            ' Check for sql switches
            If Globals.AttachedtoConsole AndAlso
                (Utility.StringArrayContains(Globals.CommandLineArgs, "testdbconn", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "testconn", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "mdboverview", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "cleanapps", True)) Then

                ' Encacpsulate express initialization
                Try

                    ' Check if ITCM is installed
                    If Not Utility.IsITCMInstalled Then Throw New Exception("ITCM is not installed.")

                    ' Express initialization
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    InitRegistry(CallStack)
                    InitComstore(CallStack)
                    Logger.InitDebugLog(CallStack)
                    InitStartupSwitches(CallStack)

                Catch ex As Exception

                    ' Verify we're not running as SYSTEM
                    If Not WindowsIdentity.GetCurrent.IsSystem Then

                        ' Check if attached to console
                        If Globals.AttachedtoConsole Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, ex.Message)

                        Else

                            ' Report initialization exception
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)

                        End If

                    End If

                    ' Detach from console
                    WindowsAPI.DetachConsole()

                    ' Hard exit
                    Environment.Exit(0)

                End Try

                ' Call SQL function dispatcher
                DatabaseAPI.SQLFunctionDispatch(CallStack)

                ' Deinitialize (keep debug log)
                DeInit(CallStack, True, True)

                ' Hard exit
                Environment.Exit(0)

            End If

            ' *****************************
            ' - Switch: Launch an application
            ' *****************************

            ' Check for sql switches
            If Utility.StringArrayContains(Globals.CommandLineArgs, "launch", True) Then

                ' Encacpsulate express initialization
                Try

                    ' Express initialization
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    InitStartupSwitches(CallStack)
                    InitResources(CallStack)

                Catch ex As Exception

                    ' Verify we're not running as SYSTEM
                    If Not WindowsIdentity.GetCurrent.IsSystem Then

                        ' Check if attached to console
                        If Globals.AttachedtoConsole Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, ex.Message)

                        Else

                            ' Report initialization exception
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)

                        End If

                    End If

                    ' Detach from console
                    WindowsAPI.DetachConsole()

                End Try

                ' Laumch app
                LaunchPad(CallStack, Globals.LaunchAppContext, Globals.LaunchAppFileName, FileVector.GetFilePath(Globals.LaunchAppFileName), Globals.LaunchAppArguments)

                ' Deinitialize
                DeInit(CallStack, True, False)

                ' Hard exit
                Environment.Exit(0)

            End If

            ' *****************************
            ' - Switch: On-demand software library cleanup execution.
            ' *****************************

            ' Check for sql switches
            If Globals.AttachedtoConsole AndAlso
                (Utility.StringArrayContains(Globals.CommandLineArgs, "checklibrary", True) OrElse
                Utility.StringArrayContains(Globals.CommandLineArgs, "cleanlibrary", True)) Then

                ' Encacpsulate express initialization
                Try

                    ' Check if ITCM is installed
                    If Not Utility.IsITCMInstalled Then Throw New Exception("ITCM is not installed.")

                    ' Express initialization
                    InitProcess(CallStack)
                    InitEnvironment(CallStack)
                    InitRegistry(CallStack)
                    InitComstore(CallStack)
                    Logger.InitDebugLog(CallStack)
                    InitStartupSwitches(CallStack)

                Catch ex As Exception

                    ' Verify we're not running as SYSTEM
                    If Not WindowsIdentity.GetCurrent.IsSystem Then

                        ' Check if attached to console
                        If Globals.AttachedtoConsole Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, ex.Message)

                        Else

                            ' Report initialization exception
                            AlertBox.CreateUserAlert(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, 20)

                        End If

                    End If

                    ' Detach from console
                    WindowsAPI.DetachConsole()

                    ' Hard exit
                    Environment.Exit(0)

                End Try

                ' Cleanup library
                LibraryManager.RepairLibrary(CallStack)

                ' Deinitialize
                DeInit(CallStack, True, True)

                ' Hard exit
                Environment.Exit(0)

            End If

            ' *****************************
            ' - Main process initialization.
            ' *****************************

            Try

                ' Process initialization
                InitProcess(CallStack)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 1.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 1: " + Environment.NewLine +
                                             Globals.ProcessFriendlyName + " process initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 1

            End Try

            Try

                ' Verify ITCM is present on the system
                If Utility.IsITCMInstalled Then

                    ' Set global flag
                    Globals.ITCMInstalled = True

                Else

                    ' Set global flag
                    Globals.ITCMInstalled = False

                End If

                ' Environment initialization
                InitEnvironment(CallStack)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " environment initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 3.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 3: " + Environment.NewLine +
                                             Globals.ProcessShortName + " environment initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 2

            End Try

            Try

                ' Initlize based on ITCM installation status
                If Globals.ITCMInstalled Then

                    ' Registry Initialization
                    InitRegistry(CallStack)

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " registry initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 4.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 4: " + Environment.NewLine +
                                             Globals.ProcessShortName + " registry initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 3

            End Try

            Try

                ' Initialize based on ITCM installation status
                If Globals.ITCMInstalled Then

                    ' Comstore Initialization
                    InitComstore(CallStack)

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " comstore initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 5.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 5: " + Environment.NewLine +
                                             Globals.ProcessShortName + " comstore initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 4

            End Try

            Try

                ' Isolation check
                IsolationCheck(CallStack)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " isolation check failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 6.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 6: " + Environment.NewLine +
                                             Globals.ProcessShortName + " isolation check failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 5

            End Try

            Try

                ' Debug log initialization
                Logger.InitDebugLog(CallStack)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " debug log initialization failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 7.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 7: " + Environment.NewLine +
                                             Globals.ProcessShortName + " debug log initialization failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 6

            End Try

            Try

                ' Startup switch initialization
                InitStartupSwitches(CallStack)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " failed to process startup switches.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 8.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 8: " + Environment.NewLine +
                                             Globals.ProcessShortName + " failed to process startup switches." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 7

            End Try

            Try

                ' Extract embedded resources
                InitResources(CallStack)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: " + Globals.ProcessShortName + " resource extraction failed.")
                Logger.WriteDebug(CallStack, "Exception: " + ex.Message)
                Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + ": Exit code 10.")

                ' Check if user prompt is appropriate
                If Not (Globals.RunningAsSystemIdentity OrElse
                    Globals.ParentProcessTree.Contains("sd_jexec") OrElse
                    Globals.AttachedtoConsole) Then

                    ' Prompt user with timeout
                    AlertBox.CreateUserAlert("Exit code 10: " + Environment.NewLine +
                                             Globals.ProcessShortName + " resource extraction failed." +
                                             Environment.NewLine + Environment.NewLine +
                                             "Exception: " + Environment.NewLine + ex.Message, 20)

                End If

                ' Return
                Return 8

            End Try

            ' Return
            Return 0

        End Function

        ' Initialize process
        Public Shared Sub InitProcess(ByVal CallStack As String)

            ' Local variables
            Dim ProcessWMI As ManagementObject = Nothing
            Dim CurrentID As Integer = Nothing
            Dim ParentID As Integer = Nothing
            Dim ParentName As String = Nothing

            ' Update call stack
            CallStack += "InitProcess|"

            ' Write debug
            Logger.WriteDebug(CallStack, "Process name: " + Globals.ProcessShortName)
            Logger.WriteDebug(CallStack, "Version: " + Globals.ProcessFriendlyName + " " + Globals.AppVersion)
            Logger.WriteDebug(CallStack, ".NET framework version: " + Globals.DotNetVersion)

            ' Set the HostName
            Globals.HostName = My.Computer.Name

            ' Write debug
            Logger.WriteDebug(CallStack, "Hostname: " + My.Computer.Name)

            ' Read process identity
            Globals.ProcessIdentity = WindowsIdentity.GetCurrent
            Globals.RunningAsSystemIdentity = WindowsIdentity.GetCurrent.IsSystem

            ' Write debug
            Logger.WriteDebug(CallStack, "Running as: " + Globals.ProcessIdentity.Name)

            ' Get the PID
            Globals.ProcessID = Process.GetCurrentProcess.Id

            ' Write debug
            Logger.WriteDebug(CallStack, "PID: " + Globals.ProcessID.ToString)

            ' Query WMI
            Try

                ' Stub first process id
                CurrentID = Globals.ProcessID

                ' Loop dangerously
                While True

                    ' Query WMI for parent process info
                    ProcessWMI = New ManagementObject("Win32_Process.Handle='" & CurrentID & "'")
                    ParentID = ProcessWMI("ParentProcessID")
                    ParentName = Process.GetProcessById(ParentID).ProcessName.ToString

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Parent: " + ParentID.ToString + "/" + ParentName)

                    ' Store immediate parent for reference
                    If Globals.ParentProcessName Is Nothing Then Globals.ParentProcessName = ParentName.ToLower

                    ' Add to list
                    Globals.ParentProcessTree.Add(ParentName.ToLower)

                    ' Reload parent id for next iteration
                    CurrentID = ParentID

                End While

            Catch ex As Exception

                ' Check if immediate parent has terminated
                If Globals.ParentProcessName Is Nothing Then Globals.ParentProcessName = "noparent"

            End Try

            ' Get the working directory
            Globals.WorkingDirectory = Globals.ProcessName.Substring(0, Globals.ProcessName.LastIndexOf("\") + 1)

            ' Write debug
            Logger.WriteDebug(CallStack, "Working directory: " + Globals.WorkingDirectory)
            Logger.WriteDebug(CallStack, "Attached to console: " + Globals.AttachedtoConsole.ToString)

        End Sub

        ' Initialize environment
        Public Shared Sub InitEnvironment(ByVal CallStack As String)

            ' Update call stack
            CallStack += "InitEnvironment|"

            ' Find a temporary folder
            Try

                ' Read the base Windows folder
                Globals.WindowsBase = Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine)

                ' Read TEMP env variable
                Globals.WinOfflineTemp = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine)

                ' Verify TEMP is valid
                If Globals.WinOfflineTemp = Nothing OrElse Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                    ' Read TMP variable
                    Globals.WinOfflineTemp = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine)

                    ' Verify TMP is valid
                    If Globals.WinOfflineTemp = Nothing OrElse Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                        ' Read WINDIR variable
                        Globals.WinOfflineTemp = Globals.WindowsBase

                        ' Verify WINDIR is valid
                        If Globals.WinOfflineTemp = Nothing OrElse Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                            ' Throw exception
                            Throw New Exception("Unable to create a temporary folder. " +
                                                Globals.ProcessFriendlyName +
                                                " could not read the system %temp%, %tmp% or %windir% environment variables.")

                        Else

                            ' Stub the windows temp directory
                            Globals.WinOfflineTemp += "\temp"

                            ' Check if the directory is valid
                            If Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                                ' Throw exception
                                Throw New Exception("Unable to create a temporary folder. The %windir% system environment variable is invalid.")

                            End If

                        End If

                    End If

                End If

                ' Set WinOffline temp folder
                Globals.WindowsTemp = Globals.WinOfflineTemp
                Globals.WinOfflineTemp = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName

                ' Write debug
                Logger.WriteDebug(CallStack, "Temporary folder: " + Globals.WindowsTemp)

            Catch ex As Exception

                ' Throw exception
                Throw New Exception("Unable to create a temporary folder. " +
                                    "An exception was caught while attempting to read system environment variables: " +
                                    Environment.NewLine + Environment.NewLine + ex.Message)

            End Try

            ' Create temp folder
            Try

                ' Check if temp folder already exists
                If System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " folder already exists: " + Globals.WinOfflineTemp)

                Else

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Create folder: " + Globals.WinOfflineTemp)

                    ' Create temp folder
                    System.IO.Directory.CreateDirectory(Globals.WinOfflineTemp)

                End If

            Catch ex As Exception

                ' Throw exception
                Throw New Exception("Unable to create a temporary folder: " + Environment.NewLine +
                                    Globals.WinOfflineTemp + Environment.NewLine + Environment.NewLine +
                                    "An exception was caught creating the folder: " +
                                    Environment.NewLine + Environment.NewLine + ex.Message)

            End Try

            ' Check if ITCM is installed
            If Globals.ITCMInstalled Then

                ' Read CAM env variable
                Try

                    ' Read CAI_MSQ env variable
                    Globals.CAMFolder = Environment.GetEnvironmentVariable("CAI_MSQ", EnvironmentVariableTarget.Machine)

                    ' Verify CAI_MSQ is valid
                    If Globals.CAMFolder = Nothing OrElse
                        Not System.IO.Directory.Exists(Globals.CAMFolder) Then

                        ' Throw exception
                        Throw New Exception("Failed to read CAM folder from environment variable: %CAI_MSQ%")

                    End If

                    ' Write debug
                    Logger.WriteDebug(CallStack, "CAM folder: " + Globals.CAMFolder)

                Catch ex As Exception

                    ' Throw exception
                    Throw New Exception("An exception was caught reading the CAM install folder: " +
                                        Environment.NewLine + Environment.NewLine + ex.Message)

                End Try

                ' Read SSA env variable [ITCM=Required, DSM=Optional]
                Try

                    ' Read CSAM_SOCKADAPTER env variable
                    Globals.SSAFolder = Environment.GetEnvironmentVariable("CSAM_SOCKADAPTER", EnvironmentVariableTarget.Machine)

                    ' Verify CSAM_SOCKADAPTER is valid
                    If Globals.SSAFolder = Nothing OrElse
                        Not System.IO.Directory.Exists(Globals.SSAFolder) Then

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: CSAM_SOCKADAPTER environment variable is invalid.")
                        Logger.WriteDebug(CallStack, "Warning: Failed to read SSA folder from environment.")

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "SSA folder: " + Globals.SSAFolder)

                    End If

                Catch ex As Exception

                    ' Throw exception
                    Throw New Exception("An exception was caught reading the SSA install folder: " +
                                        Environment.NewLine + Environment.NewLine + ex.Message)

                End Try

            End If

        End Sub

        ' Initialize registry
        Public Shared Sub InitRegistry(ByVal CallStack As String)

            ' Local variables
            Dim ProductInfoKey As Microsoft.Win32.RegistryKey = Nothing
            Dim CAPKIKey As Microsoft.Win32.RegistryKey = Nothing
            Dim FeatureKeys As String()
            Dim FeatureValue As String

            ' Update call stack
            CallStack += "InitRegistry|"

            ' *****************************
            ' - Read Client Auto registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)

                ' Check 32-bit registry
                If ProductInfoKey Is Nothing Then

                    ' Throw exception
                    Throw New Exception("Registry key not found: ComputerAssociates\Unicenter ITRM")

                End If

            End If

            ' Read install version
            Try
                Globals.ITCMVersion = ProductInfoKey.GetValue("InstallVersion").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallVersion")
            End Try

            ' Read ca base folder 
            Try
                Globals.CAFolder = ProductInfoKey.GetValue("InstallDir").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallDir")
            End Try

            ' Read itcm folder 
            Try
                Globals.DSMFolder = ProductInfoKey.GetValue("InstallDirProduct").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallDirProduct")
            End Try

            ' Read shared component folder
            Try
                Globals.SharedCompFolder = ProductInfoKey.GetValue("InstallShareDir").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstallShareDir")
            End Try

            ' Close registry key
            ProductInfoKey.Close()

            ' *****************************
            ' - Read CAPKI registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\ComputerAssociates\Shared", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Shared", False)

                ' Check 32-bit registry
                If ProductInfoKey Is Nothing Then

                    ' Throw exception
                    Throw New Exception("Registry key not found: ComputerAssociates\Shared")

                End If

            End If

            ' Iterate subkeys
            For Each SubKeyName As String In ProductInfoKey.GetSubKeyNames

                ' Check for CAPKI key
                If SubKeyName.ToLower.StartsWith("capki") Then

                    ' Check for WOW6432Node
                    If ProductInfoKey.Name.ToLower.Contains("wow6432node") Then

                        ' Read 64-bit registry
                        CAPKIKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\ComputerAssociates\Shared\" + SubKeyName, False)

                    Else

                        ' Read 32-bit registry
                        CAPKIKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Shared\" + SubKeyName, False)

                    End If

                End If

            Next

            ' Close registry key
            ProductInfoKey.Close()

            ' Read the CAPKI install folder
            Try
                Globals.CAPKIFolder = CAPKIKey.GetValue("InstallDir").ToString()
            Catch ex As Exception
                Throw New Exception("Registry value not found:" + Environment.NewLine + CAPKIKey.Name + "\InstallDir")
            End Try

            ' Close registry key
            CAPKIKey.Close()

            ' Write debug
            Logger.WriteDebug(CallStack, "ITCM version: " + Globals.ITCMVersion)
            Logger.WriteDebug(CallStack, "CA folder: " + Globals.CAFolder)
            Logger.WriteDebug(CallStack, "DSM folder: " + Globals.DSMFolder)
            Logger.WriteDebug(CallStack, "Shared Components folder: " + Globals.SharedCompFolder)
            Logger.WriteDebug(CallStack, "CAPKI Folder: " + Globals.CAPKIFolder)

            ' *****************************
            ' - Read Client Auto installed features.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)

                ' Check 32-bit registry
                If ProductInfoKey Is Nothing Then

                    ' Throw exception
                    Throw New Exception("Registry key not found:" + Environment.NewLine + "ComputerAssociates\Unicenter ITRM\InstalledFeatures")

                End If

            End If

            ' Get list of installed features
            FeatureKeys = ProductInfoKey.GetValueNames

            ' Iterare each feature
            For Each feature As String In FeatureKeys

                ' Get feature version (Registry key value)
                FeatureValue = ProductInfoKey.GetValue(feature)

                ' Write debug (suppress for now)
                'Logger.WriteDebug(CallStack, "Installed Feature: " + feature.ToString + " [" + FeatureValue.ToString + "]")

                ' Check for root feature
                If Not feature.Contains("-") Then

                    ' Check if any feature is set
                    If Globals.ITCMFunction Is Nothing Then

                        ' Assign first feature
                        Globals.ITCMFunction = feature

                    Else

                        ' Append new feature
                        Globals.ITCMFunction += ", " + feature

                    End If

                End If

                ' Update plugin list
                If feature.ToLower.Contains("asset management") And
                    Not Globals.FeatureList.Contains("Asset Management") Then

                    ' Update list
                    Globals.FeatureList.Add("Asset Management")

                ElseIf feature.ToLower.Contains("software delivery") And
                    Not Globals.FeatureList.Contains("Software Delivery") Then

                    ' Update list
                    Globals.FeatureList.Add("Software Delivery")

                ElseIf feature.ToLower.Contains("remote control") And
                    Not Globals.FeatureList.Contains("Remote Control") Then

                    ' Update list
                    Globals.FeatureList.Add("Remote Control")

                ElseIf feature.ToLower.Contains("data transport service") And
                    Not Globals.FeatureList.Contains("Data Transport") Then

                    ' Update list
                    Globals.FeatureList.Add("Data Transport")

                End If

            Next

            ' Close registry key
            ProductInfoKey.Close()

            ' Simplify feature list
            If Globals.ITCMFunction.ToLower.Contains("manager") And
                Globals.ITCMFunction.ToLower.Contains("scalability") Then

                ' Update function
                Globals.ITCMFunction = "Domain Manager"

            ElseIf Globals.ITCMFunction.ToLower.Contains("manager") Then

                ' Update function
                Globals.ITCMFunction = "Enterprise Manager"

            ElseIf Globals.ITCMFunction.ToLower.Contains("scalability") And
                Globals.ITCMFunction.ToLower.Contains("explorer") Then

                ' Update function
                Globals.ITCMFunction = "Scalability Server + DSM Explorer"

            ElseIf Globals.ITCMFunction.ToLower.Contains("scalability") Then

                ' Update function
                Globals.ITCMFunction = "Scalability Server"

            ElseIf Globals.ITCMFunction.ToLower.Contains("agent") And
                Globals.ITCMFunction.ToLower.Contains("explorer") Then

                ' Update function
                Globals.ITCMFunction = "Agent + DSM Explorer"

            ElseIf Globals.ITCMFunction.ToLower.Contains("agent") Then

                ' Update function
                Globals.ITCMFunction = "Agent"

            End If

            ' Write debug
            Logger.WriteDebug(CallStack, "ITCM Function: " + Globals.ITCMFunction)
            Logger.WriteDebug(CallStack, "ITCM Features: " + Utility.ArrayListtoCommaString(Globals.FeatureList, ", "))

            ' *****************************
            ' - Read Client Auto HostUUID.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\hostUUID", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\hostUUID", False)

                ' Check 32-bit registry
                If ProductInfoKey Is Nothing Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Warning: Registry key not found: hostUUID")

                End If

            End If

            ' Check if HostUUID value exists
            If ProductInfoKey IsNot Nothing Then

                ' Read the HostUUID value
                Try
                    Globals.HostUUID = ProductInfoKey.GetValue("HostUUID").ToString()
                Catch ex As Exception
                    Globals.HostUUID = "Not found"
                End Try

                ' Write debug
                Logger.WriteDebug(CallStack, "HostUUID: " + Globals.HostUUID)

            End If

            ' Close registry key
            ProductInfoKey.Close()

            ' *****************************
            ' - Read USD registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\Software Delivery", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\Software Delivery", False)

            End If

            ' Check for valid registry key
            If ProductInfoKey IsNot Nothing Then

                ' Read the installation directory value
                Try
                    Globals.SDFolder = ProductInfoKey.GetValue("SDROOT").ToString()
                Catch ex As Exception
                    Globals.SDFolder = "Not found"
                End Try

                ' Write debug
                Logger.WriteDebug(CallStack, "Software Delivery folder: " + Globals.SDFolder)

                ' Close registry key
                ProductInfoKey.Close()

            End If

            ' *****************************
            ' - Read DTS registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)

            End If

            ' Check for valid registry key
            If ProductInfoKey IsNot Nothing Then

                ' Read the installation directory value
                Try
                    Globals.DTSFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.DTSFolder = "Not found"
                End Try

                ' Write debug
                Logger.WriteDebug(CallStack, "Data transport service folder: " + Globals.DTSFolder)

                ' Close registry key
                ProductInfoKey.Close()

            End If

            ' *****************************
            ' - Read DSM Explorer registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\EGC3.0N", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\EGC3.0N", False)

            End If

            ' Check for valid registry key
            If ProductInfoKey IsNot Nothing Then

                ' Read the installation directory value
                Try
                    Globals.EGCFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.EGCFolder = "Not found"
                End Try

                ' Write debug
                Logger.WriteDebug(CallStack, "EGC folder: " + Globals.EGCFolder)

                ' Close registry key
                ProductInfoKey.Close()

            End If

            ' *****************************
            ' - Read Perf Lite agent registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)

            End If

            ' Check for valid registry key
            If ProductInfoKey IsNot Nothing Then

                ' Read the installation version value
                Try
                    Globals.PMLAVersion = ProductInfoKey.GetValue("DisplayVersion").ToString()
                Catch ex As Exception
                    Globals.PMLAVersion = "Not found"
                End Try

                ' Read the installation directory value
                Try
                    Globals.PMLAFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.PMLAFolder = "Not found"
                End Try

                ' Write debug
                Logger.WriteDebug(CallStack, "Performance lite agent version: " + Globals.PMLAVersion)
                Logger.WriteDebug(CallStack, "Performance lite agent folder: " + Globals.PMLAFolder)

                ' Close registry key
                ProductInfoKey.Close()

            End If

            ' *****************************
            ' - Read DMPrimer registry.
            ' *****************************

            ' Read 64-bit registry
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\DMPrimer", False)

            ' Check 64-bit registry
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\DMPrimer", False)

            End If

            ' Check for valid registry key
            If ProductInfoKey IsNot Nothing Then

                ' Read the installation directory value
                Try
                    Globals.DMPrimerFolder = ProductInfoKey.GetValue("InstallPath").ToString()
                Catch ex As Exception
                    Globals.DMPrimerFolder = "Not found"
                End Try

                ' Write debug
                Logger.WriteDebug(CallStack, "DMPrimer folder: " + Globals.DMPrimerFolder)

                ' Close registry key
                ProductInfoKey.Close()

            End If

        End Sub

        ' Initialize comstore
        Public Shared Sub InitComstore(ByVal CallStack As String)

            ' Local variables
            Dim ComstoreString As String

            ' Update call stack
            CallStack += "InitComstore|"

            ' *****************************
            ' - Get Software Delivery library location.
            ' *****************************

            ' Check ITCM function (not necessary for agents)
            If Globals.ITCMFunction.ToLower.Contains("manager") OrElse Globals.ITCMFunction.ToLower.Contains("server") Then

                ' Retrieve SD library location from comstore
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/usd/shared", "ARCHIVE")

                ' Remove carriage return (with full safety)
                ComstoreString = ComstoreString.Replace(vbCr, "").Replace(vbLf, "")

                ' Check standard output
                If ComstoreString.Contains("\") Then

                    ' Update global
                    Globals.SDLibraryFolder = ComstoreString

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Software library: " + Globals.SDLibraryFolder)

                End If

            End If

            ' *****************************
            ' - Get ENC Gateway functionality.
            ' *****************************

            ' Retrieve ENC functionality from comstore
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/common/caf/plugins/encserver", "description")

            ' Check for gateway functionality
            If ComstoreString.Contains("(") Then

                ' Filter to gateway functions
                ComstoreString = ComstoreString.Substring(ComstoreString.IndexOf("(") + 1)

                ' Check functions
                If ComstoreString.ToLower.Contains("manager") And
                    ComstoreString.ToLower.Contains("server") And
                    ComstoreString.ToLower.Contains("router") Then

                    ' Update global
                    Globals.ENCFunction = "ENC Gateway (Manager, Server, Router)"

                    ' Write debug
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)

                ElseIf ComstoreString.ToLower.Contains("manager") And
                    ComstoreString.ToLower.Contains("server") Then

                    ' Update global
                    Globals.ENCFunction = "ENC Gateway (Manager, Server)"

                    ' Write debug
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)

                ElseIf ComstoreString.ToLower.Contains("server") And
                    ComstoreString.ToLower.Contains("router") Then

                    ' Update global
                    Globals.ENCFunction = "ENC Gateway (Server, Router)"

                    ' Write debug
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)

                ElseIf ComstoreString.ToLower.Contains("router") Then

                    ' Update global
                    Globals.ENCFunction = "ENC Gateway (Router)"

                    ' Write debug
                    Logger.WriteDebug(CallStack, "ENC Gateway Functionality: " + Globals.ENCFunction)

                End If

            Else

                ' Update global
                Globals.ENCFunction = Nothing

                ' Write debug
                Logger.WriteDebug(CallStack, "No ENC Gateway Functionality")

            End If

            ' Update Client Auto functionality
            If Not Globals.ENCFunction = Nothing Then

                ' Append ENC gateway functionality
                Globals.ITCMFunction += " + " + Globals.ENCFunction

            End If

            ' *****************************
            ' - Get ENC gateway server.
            ' *****************************

            ' Retrieve ENC gateway server from comstore
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/common/enc/client", "serveraddress")

            ' Update global
            Globals.ENCGatewayServer = ComstoreString

            ' Write debug
            Logger.WriteDebug(CallStack, "ENC Gateway Server address: " + Globals.ENCGatewayServer)

            ' *****************************
            ' - Get ENC server TCP port.
            ' *****************************

            ' Retrieve ENC server tcp port from comstore
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/common/enc/client", "servertcpport")

            ' Update global
            Globals.ENCServerTCPPort = ComstoreString

            ' Write debug
            Logger.WriteDebug(CallStack, "ENC Gateway Server port: " + Globals.ENCServerTCPPort)

            ' *****************************
            ' - Get agent management info.
            ' *****************************

            ' Retrieve domain manager from comstore
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/agent/units/.", "currentmanageraddress")

            ' Update global
            Globals.DomainManager = ComstoreString

            ' Write debug
            Logger.WriteDebug(CallStack, "Current domain manager: " + Globals.DomainManager)

            ' Retrieve scalability server from comstore
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/agent/solutions/generic", "serveraddress")

            ' Update Global
            Globals.ScalabilityServer = ComstoreString

            ' Write debug
            Logger.WriteDebug(CallStack, "Current scalability server: " + Globals.ScalabilityServer)

            ' *****************************
            ' - Get agent version.
            ' *****************************

            ' Retrieve domain manager from comstore
            ComstoreString = ComstoreAPI.GetParameterValue("itrm/agent/solutions/generic", "version")

            ' Update global
            Globals.ITCMComstoreVersion = ComstoreString

            ' Write debug
            Logger.WriteDebug(CallStack, "Agent version: " + Globals.ITCMComstoreVersion)

            ' *****************************
            ' - Get database information.
            ' *****************************

            ' Check ITCM function (not necessary for agents or servers)
            If Globals.ITCMFunction.ToLower.Contains("manager") Then

                ' Retrieve database server from comstore
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbmsserver")

                ' Update global
                Globals.DatabaseServer = ComstoreString

                ' Write debug
                Logger.WriteDebug(CallStack, "Database server: " + Globals.DatabaseServer)

                ' Retrieve database instance from comstore
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbmsinstance")

                ' Check for instance,port
                If ComstoreString IsNot Nothing AndAlso ComstoreString.Contains(",") Then

                    ' Update globals
                    Globals.DatabaseInstance = ComstoreString.Substring(0, ComstoreString.IndexOf(","))
                    Globals.DatabasePort = ComstoreString.Substring(ComstoreString.IndexOf(",") + 1)

                Else

                    ' Update global
                    Globals.DatabaseInstance = ComstoreString

                End If

                ' Write debug
                Logger.WriteDebug(CallStack, "Database instance: " + ComstoreString)

                ' Retrieve database name from comstore
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbname")

                ' Update global
                Globals.DatabaseName = ComstoreString

                ' Write debug
                Logger.WriteDebug(CallStack, "Database name: " + Globals.DatabaseName)

                ' Retrieve database user from comstore
                ComstoreString = ComstoreAPI.GetParameterValue("itrm/database/default", "dbuser")

                ' Update global
                Globals.DatabaseUser = ComstoreString

                ' Write debug
                Logger.WriteDebug(CallStack, "Database user: " + Globals.DatabaseUser)

            End If

        End Sub

        ' Isolation check
        Public Shared Sub IsolationCheck(ByVal CallStack As String)

            ' Declare variables
            Dim clsParentProcessName As String
            Dim LoopCounter As Integer = 0

            ' Update call stack
            CallStack += "InitIsolation|"

            ' Check for pre-existing parallel process (exclude pipe clients)
            If Utility.IsProcessRunningCount(Globals.ProcessFriendlyName) > 1 Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Warning: Detected existing " + Globals.ProcessFriendlyName + " execution in progress.")
                Logger.WriteDebug(CallStack, "Warning: Current execution (this process) may not be the primary process.")
                Logger.WriteDebug(CallStack, "Retrieving list of all running " + Globals.ProcessFriendlyName + " processes..")

                ' Get list of all WinOffline processes
                Dim ParallelProcessList As ArrayList = Utility.GetParallelProcesses(Globals.ProcessFriendlyName)

                ' Iterate each process
                For Each clsProcess As Process In ParallelProcessList

                    ' Check if detected process matches our own
                    If clsProcess.Id = Globals.ProcessID Then

                        ' Exclude our own process
                        Continue For

                    End If

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " PID: " + clsProcess.Id.ToString)

                    ' Query WMI for PPID
                    Try

                        ' Query WMI for process info
                        Dim clsProcessWMI = New ManagementObject("Win32_Process.Handle='" & clsProcess.Id & "'")
                        Dim clsParentProcessID = clsProcessWMI("ParentProcessID")

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " PPID: " + clsParentProcessID.ToString)

                        ' Get friendly parent process name
                        clsParentProcessName = System.Diagnostics.Process.GetProcessById(clsParentProcessID).ProcessName.ToString

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " parent name: " + clsParentProcessName)

                    Catch ex As Exception

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Existing " + Globals.ProcessFriendlyName + " parent process has terminated.")

                        ' Manually set parent process
                        clsParentProcessName = "NoParent"

                    End Try

                Next

                ' *****************************
                ' - Dispatch duplicate process (this process).
                ' *****************************

                ' Check the parent process
                If Globals.ParentProcessTree.Contains("sd_jexec") Then

                    ' *****************************
                    ' Scenario #1: Duplicate process, our parent is software delivery.
                    ' --> Signal a re-run request and terminate.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Entry point: Software delivery.")
                    Logger.WriteDebug(CallStack, "Signal re-run request..")

                    ' Build an execution string
                    Dim ExecutionString = Globals.DSMFolder + "bin\" + "sd_acmd.exe"
                    Dim ArgumentString = "signal rerun"

                    ' Create detached process for sd agent rerun signal
                    Dim SignalReRunProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                    SignalReRunProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                    SignalReRunProcessStartInfo.UseShellExecute = False
                    SignalReRunProcessStartInfo.RedirectStandardOutput = True
                    SignalReRunProcessStartInfo.CreateNoWindow = True

                    ' Start detached process
                    Dim RunningProcess = Process.Start(SignalReRunProcessStartInfo)

                    ' Wait for detached process to exit
                    RunningProcess.WaitForExit()

                    ' Close detached process
                    RunningProcess.Close()

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Re-run request sent to software delivery.")

                    ' Throw Exception
                    Throw New Exception("Scenario #1: Duplicate process, our parent is software delivery. Signal re-run request..")

                ElseIf Globals.ParentProcessName.ToLower.Equals(Globals.ProcessFriendlyName.ToLower) Then

                    ' *****************************
                    ' Scenario #2: Duplicate process, our parent is WinOffline.
                    ' --> Finite loop for isolation before continuing, otherwise terminate.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Entry point: " + Globals.ProcessFriendlyName + ".")

                    ' Delay execution
                    Thread.Sleep(10000)

                    ' Reset the loop counter
                    LoopCounter = 0

                    ' Wait for parent to terminate
                    While Utility.IsProcessRunningCount(Globals.ProcessFriendlyName) > 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Parent process is still active.")

                        ' Process message queue -- Avoid deadlock for operations longer than 60 seconds
                        System.Windows.Forms.Application.DoEvents()

                        ' Delay execution
                        Thread.Sleep(10000)

                        ' Increment the loop counter
                        LoopCounter += 1

                        ' Check the loop counter
                        If LoopCounter >= 5 Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, "Parent process has not terminated.")
                            Logger.WriteDebug(CallStack, "Exit with isolation check failure..")

                            ' Throw Exception
                            Throw New Exception("Scenario #2: Duplicate process, our parent is WinOffline. " +
                                                "Finite loop for isolation before continuing, otherwise terminate.")

                        End If

                    End While

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Parent process has terminated.")
                    Logger.WriteDebug(CallStack, "Isolation Check: Passed.")

                ElseIf Globals.ParentProcessName.ToLower.Equals("noparent") Then

                    ' *****************************
                    ' Scenario #3: Duplicate process, our parent is unknown or has terminated.
                    ' --> Finite loop for isolation before continuing, otherwise terminate.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Entry point: No parent.")

                    ' Delay execution
                    Thread.Sleep(10000)

                    ' Reset the loop counter
                    LoopCounter = 0

                    ' Wait for parent to terminate
                    While Utility.IsProcessRunningCount(Globals.ProcessFriendlyName) > 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Parallel process is still active.")

                        ' Process message queue -- Avoid deadlock for operations longer than 60 seconds
                        System.Windows.Forms.Application.DoEvents()

                        ' Delay execution
                        Thread.Sleep(10000)

                        ' Increment the loop counter
                        LoopCounter += 1

                        ' Check the loop counter
                        If LoopCounter >= 5 Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, "Parallel process has not terminated.")
                            Logger.WriteDebug(CallStack, "Exit with isolation check failure..")

                            ' Throw Exception
                            Throw New Exception("Scenario #3: Duplicate process, our parent is unknown or has terminated." +
                                                "Finite loop for isolation before continuing, otherwise terminate.")

                        End If

                    End While

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Parallel process has terminated.")
                    Logger.WriteDebug(CallStack, "Isolation Check: Passed.")

                Else

                    ' *****************************
                    ' Scenario #4: Duplicate process launched by user or script.
                    ' --> Terminate.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Entry point: Standalone execution.")
                    Logger.WriteDebug(CallStack, "Exit with isolation check failure..")

                    ' Throw Exception
                    Throw New Exception("Scenario #4: Duplicate process launched by user or script. Terminate.")

                End If

            Else

                ' Write debug
                Logger.WriteDebug(CallStack, "No parallel " + Globals.ProcessShortName + " processes detected.")

            End If

        End Sub

        ' Startup switch initialization
        Public Shared Sub InitStartupSwitches(ByVal CallStack As String)

            ' Local variables
            Dim RemovePatchString As String
            Dim IndividualPatch As String()
            Dim rVector As RemovalVector
            Dim RemovePatchSwitchError As Boolean = False

            ' Update call stack
            CallStack += "InitSwitch|"

            ' Loop command line arguments
            For i As Integer = 0 To Globals.CommandLineArgs.Length - 1

                ' Match the switch
                If Globals.CommandLineArgs(i).ToString.ToLower.Equals("backout") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-backout") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/backout") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("remove") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-remove") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/remove") Then

                    ' Verify a patch list was provided
                    If i + 1 <= Globals.CommandLineArgs.Length - 1 Then

                        ' Get patch removal list
                        RemovePatchString = Globals.CommandLineArgs(i + 1)

                        ' Check if string contains a list, or a single patch
                        If RemovePatchString.Contains(",") Then

                            ' Split the string
                            IndividualPatch = RemovePatchString.Split(",")

                            ' Process the individual patches
                            For Each strLine As String In IndividualPatch

                                ' Create a removal vector
                                rVector = New RemovalVector(strLine)

                                ' Commit removal to manifest
                                Manifest.UpdateManifest(CallStack, Manifest.REMOVAL_MANIFEST, rVector)

                            Next

                        Else

                            ' Create a removal vector
                            rVector = New RemovalVector(RemovePatchString)

                            ' Commit removal to manifest
                            Manifest.UpdateManifest(CallStack, Manifest.REMOVAL_MANIFEST, rVector)

                        End If

                        ' Set the backout switch
                        Globals.RemovePatchSwitch = True

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Remove patches.")

                        ' Increment i
                        i += 1

                        ' Check if i now exceeds Globals.CommandLineArgs.Length - 1
                        If i = Globals.CommandLineArgs.Length - 1 Then Exit For

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Remove patch switch is not followed by a patch name.")

                        ' Set error switch
                        RemovePatchSwitchError = True

                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanlogs") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanlogs") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanlogs") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanlog") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanlog") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanlog") Then

                    ' Set the switch
                    Globals.CleanupLogsSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Cleanup DSM logs folder.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("resetcftrace") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-resetcftrace") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/resetcftrace") Then

                    ' Set the switch
                    Globals.ResetCftraceSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Reset the cftrace level.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("rmcamcfg") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-rmcamcfg") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/rmcamcfg") Then

                    ' Set the switch
                    Globals.RemoveCAMConfigSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Remove CAM configuration file.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanagent") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanagent") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanagent") Then

                    ' Set the switch
                    Globals.CleanupAgentSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Perform agent cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanserver") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanserver") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanserver") Then

                    ' Set the switch
                    Globals.CleanupServerSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Perform scalability server cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanlibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanlibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanlibrary") Then

                    ' Set the switch
                    Globals.CleanupSDLibrarySwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Perform software library cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("checklibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-checklibrary") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/checklibrary") Then

                    ' Set the switch
                    Globals.CheckSDLibrarySwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Analyze software library without making changes.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleancerts") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleancerts") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleancerts") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleancert") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleancert") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleancert") Then

                    ' Set the switch
                    Globals.CleanupCertStoreSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Certificate store cleanup.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipcafstartup") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipcafstartup") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipcafstartup") Then

                    ' Set the switch
                    Globals.SkipCAFStartUpSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Don't start CAF.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipcam") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipcam") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipcam") Then

                    ' Set the switch
                    Globals.SkipCAM = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Don't stop CAM service.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipdmprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipdmprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipdmprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("skipprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skipprimer") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skipprimer") Then

                    ' Set the switch
                    Globals.SkipDMPrimer = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Don't stop DMPrimer service.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("skiphm") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-skiphm") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/skiphm") Then

                    ' Set the switch
                    Globals.SkiphmAgent = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Don't stop hmAgent.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("loadgui") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-loadgui") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/loadgui") Then

                    ' Set the switch
                    Globals.LaunchGuiSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Launch DSM Explorer.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("simulatestop") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-simulatestop") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/simulatestop") Then

                    ' Set the switch
                    Globals.SimulateCafStopSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Simulate recycling CAF.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("simulatepatch") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-simulatepatch") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/simulatepatch") Then

                    ' Set the switch
                    Globals.SimulatePatchSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Simulate patch operations.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("simulatepatcherror") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-simulatepatcherror") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/simulatepatcherror") Then

                    ' Set the switch
                    Globals.SimulatePatchErrorSwitch = True
                    Globals.SimulatePatchSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Simulate patching error.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("rmhistorybefore") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-rmhistorybefore") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/rmhistorybefore") Then

                    ' Set the switch
                    Globals.RemoveHistoryBeforeSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Remove patch history file, BEFORE patch operations.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("rmhistoryafter") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-rmhistoryafter") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/rmhistoryafter") Then

                    ' Set the switch
                    Globals.RemoveHistoryAfterSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Remove patch history file, AFTER patch operations.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dumpcazipxp") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dumpcazipxp") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dumpcazipxp") Then

                    ' Set the switch
                    Globals.DumpCazipxpSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Extract cazipxp.exe utility and exit without changes.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("regenuuid") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-regenuuid") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/regenuuid") Then

                    ' Set the switch
                    Globals.RegenHostUUIDSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Regenerate agent HostUUID.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("signalreboot") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("-signalreboot") Or
                        Globals.CommandLineArgs(i).ToString.ToLower.Equals("/signalreboot") Then

                    ' Set the switch
                    Globals.SDSignalRebootSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Signal software delivery reboot request.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("reboot") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-reboot") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/reboot") Then

                    ' Set the switch
                    Globals.RebootSystemSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Reboot system after completion.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("uninstallitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-uninstallitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/uninstallitcm") Then

                    ' Set the switch
                    Globals.UninstallITCM = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Perform normal uninstall of ITCM.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("removeitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-removeitcm") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/removeitcm") Then

                    ' Set the switch
                    Globals.RemoveITCM = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Perform full removal of ITCM.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("keepuuid") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-keepuuid") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/keepuuid") Then

                    ' Set the switch
                    Globals.KeepHostUUIDSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: If -removeitcm was specified, retain the HostUUID registry key.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("disableenc") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-disableenc") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/disableenc") Then

                    ' Set the switch
                    Globals.DisableENCSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Disable ENC.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("gethistory") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-gethistory") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/gethistory") Then

                    ' Set the switch
                    Globals.GetHistorySwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Report agent patch history only.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("stopcaf") Or
                Globals.CommandLineArgs(i).ToString.ToLower.Equals("-stopcaf") Or
                Globals.CommandLineArgs(i).ToString.ToLower.Equals("/stopcaf") Then

                    ' Set the switch
                    Globals.StopCAFSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Stop CAF service on-demand.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("startcaf") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-startcaf") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/startcaf") Then

                    ' Set the switch
                    Globals.StartCAFSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Start CAF service on-demand.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbserver") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbserver") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbserver") Then

                    ' Check i's position
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then

                        ' Fetch next parameter
                        Globals.DatabaseServer = Globals.CommandLineArgs(i + 1).ToString

                        ' Increment i
                        i += 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Database server provided.")

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Database server not provided.")

                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbinstance") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbinstance") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbinstance") Then

                    ' Check i's position
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then

                        ' Fetch next parameter
                        Globals.DatabaseInstance = Globals.CommandLineArgs(i + 1).ToString

                        ' Increment i
                        i += 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Database instance provided.")

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Database instance not provided.")

                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbport") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbport") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbport") Then

                    ' Check i's position
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then

                        ' Fetch next parameter
                        Globals.DatabasePort = Globals.CommandLineArgs(i + 1).ToString

                        ' Increment i
                        i += 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Database port provided.")

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Database port not provided.")

                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbname") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbname") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbname") Then

                    ' Check i's position
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then

                        ' Fetch next parameter
                        Globals.DatabaseName = Globals.CommandLineArgs(i + 1).ToString

                        ' Increment i
                        i += 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Database name provided.")

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Database name not provided.")

                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("dbuser") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-dbuser") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/dbuser") Then

                    ' Check i's position
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then

                        ' Fetch next parameter
                        Globals.DbUser = Globals.CommandLineArgs(i + 1).ToString

                        ' Increment i
                        i += 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Database username provided.")

                    Else

                        ' Write debug
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

                    ' Check i's position
                    If i < Globals.CommandLineArgs.Length - 1 AndAlso
                    Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                    Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) Then

                        ' Fetch next parameter
                        Globals.DbPassword = Globals.CommandLineArgs(i + 1).ToString

                        ' Increment i
                        i += 1

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Database password provided.")

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Database password was not provided.")

                    End If

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("testdbconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-testdbconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/testdbconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("testconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-testconn") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/testconn") Then

                    ' Set the switch
                    Globals.DbTestConnectionSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Test database connection.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("mdboverview") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-mdboverview") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/mdboverview") Then

                    ' Set the switch
                    Globals.MdbOverviewSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Provide database overview.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("cleanapps") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-cleanapps") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/cleanapps") Then

                    ' Set the switch
                    Globals.MdbCleanAppsSwitch = True

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Switch: Execute database cleanapps script.")

                ElseIf Globals.CommandLineArgs(i).ToString.ToLower.Equals("launch") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("-launch") Or
                    Globals.CommandLineArgs(i).ToString.ToLower.Equals("/launch") Then

                    ' Check i's position
                    If i + 2 < Globals.CommandLineArgs.Length AndAlso
                        Not (Globals.CommandLineArgs(i + 1).ToString.StartsWith("/") OrElse
                            Globals.CommandLineArgs(i + 1).ToString.StartsWith("-")) AndAlso
                        Not (Globals.CommandLineArgs(i + 2).ToString.StartsWith("/") OrElse
                            Globals.CommandLineArgs(i + 2).ToString.StartsWith("-")) Then

                        ' Fetch launch context and filename
                        Globals.LaunchAppContext = Globals.CommandLineArgs(i + 1).ToString
                        Globals.LaunchAppFileName = Globals.CommandLineArgs(i + 2).ToString

                        ' Increment i for two consumed parameters
                        i += 3

                        ' Check for additional arguments and consume them
                        While (i < Globals.CommandLineArgs.Length)

                            ' Consume parameter
                            Globals.LaunchAppArguments += Globals.CommandLineArgs(i).ToString + " "

                            ' Increment i
                            i += 1

                        End While

                        ' Set the switch
                        Globals.LaunchAppSwitch = True

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Switch: Launch application.")

                    Else

                        ' Throw exception
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

                    ' Verify we're not running as SYSTEM
                    If Not Globals.RunningAsSystemIdentity Then

                        ' Check if attached to console
                        If Globals.AttachedtoConsole Then

                            ' Write help text
                            Logger.WriteDebug(HelpUI.rtbHelp.Text)

                        Else

                            ' Show the about box
                            Dim CLIHelp As New HelpUI
                            CLIHelp.ShowDialog()

                        End If

                        ' Perform cleanup
                        DeInit(CallStack, True, False)

                        ' Hard exit
                        Environment.Exit(0)

                    End If

                Else

                    ' Check if attached to console
                    If Globals.AttachedtoConsole Then

                        ' Throw exception
                        Throw New ArgumentException("Unknown switch specified-- " + Globals.CommandLineArgs(i).ToString)

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Unknown switch specified-- " + Globals.CommandLineArgs(i).ToString)

                    End If

                End If

            Next

            ' Check if user prompt is appropriate
            If RemovePatchSwitchError Then

                ' Throw exception so WinOffline initialization does not continue
                Throw New ArgumentException("Remove switch specified, but no patch name has been specified." +
                                            Environment.NewLine + Environment.NewLine +
                                            "Syntax: " + Environment.NewLine + Globals.ProcessShortName + " -remove T5X0001" + Environment.NewLine +
                                            "  OR" + Environment.NewLine + Globals.ProcessShortName + " -remove T5X0001,T5X0002" + Environment.NewLine)

            End If

            ' Check for implied switches
            If Globals.ParentProcessName.ToLower.Equals("cmd") AndAlso
                Globals.WorkingDirectory.ToLower.Contains("\dmprimer\caunicenterdsm\") AndAlso
                Globals.WorkingDirectory.ToLower.Contains("\testfixes\") Then

                ' Set implied switches (don't stop CAM or DMPrimer services)
                ' Note: Stopping these services will interrupt infrastructure deployment process
                Globals.SkipCAM = True
                Globals.SkipDMPrimer = True

                ' Write debug
                Logger.WriteDebug(CallStack, "Implied Switch: Don't stop CAM service.")
                Logger.WriteDebug(CallStack, "Implied Switch: Don't stop DMPrimer service.")

            End If

            ' No switches specified
            If Globals.CommandLineArgs.Length = 0 Then

                ' Write debug
                Logger.WriteDebug(CallStack, "No startup switches specified.")

            End If

        End Sub

        ' Embedded resource initialization
        Public Shared Sub InitResources(ByVal CallStack As String)

            ' Local variables
            Dim Source As String
            Dim Destination As String

            ' Update call stack
            CallStack += "InitResource|"

            ' Encapsulate resource extraction
            Try

                ' Get the execution assembly
                Globals.MyAssembly = Reflection.Assembly.GetExecutingAssembly()
                Globals.MyRoot = Globals.MyAssembly.GetName().Name

                ' Check if cazipxp.exe was provided
                If Not System.IO.File.Exists(Globals.WorkingDirectory + "cazipxp.exe") Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Extract: " + Globals.WinOfflineTemp + "\cazipxp.exe")

                    ' Load embedded resource
                    Globals.MyResourceStream = Globals.MyAssembly.GetManifestResourceStream(Globals.MyRoot + "." + "Cazipxp.exe")

                    ' Redimension the buffer to the size of the resource
                    ReDim Globals.MyBuffer(Convert.ToInt32(Globals.MyResourceStream.Length) - 1)

                    ' Read the resource into the buffer
                    Globals.MyResourceStream.Read(Globals.MyBuffer, 0, Globals.MyBuffer.Length)

                    ' Close the read stream
                    Globals.MyResourceStream.Close()

                    ' Check the dump switch
                    If Globals.DumpCazipxpSwitch Then

                        ' Open the write stream [Working directory]
                        Globals.MyFileStream = New System.IO.FileStream(Globals.WorkingDirectory +
                                                                        "\Cazipxp.exe",
                                                                        System.IO.FileMode.Create,
                                                                        System.IO.FileAccess.Write)

                    Else

                        ' Clear destination location
                        Utility.DeleteFile(CallStack, Globals.WinOfflineTemp + "\Cazipxp.exe")

                        ' Open the write stream [Winoffline temp folder]
                        Globals.MyFileStream = New System.IO.FileStream(Globals.WinOfflineTemp +
                                                                        "\Cazipxp.exe",
                                                                        System.IO.FileMode.Create,
                                                                        System.IO.FileAccess.Write)

                    End If

                    ' Purge the buffer to the write stream
                    Globals.MyFileStream.Write(Globals.MyBuffer, 0, Globals.MyBuffer.Length)

                    ' Close the write stream
                    Globals.MyFileStream.Close()

                Else

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Alternative resource provided: " + Globals.WorkingDirectory + "cazipxp.exe")

                    ' Populate source
                    Source = Globals.WorkingDirectory + "cazipxp.exe"

                    ' Populate destination
                    Destination = Globals.WinOfflineTemp + "\cazipxp.exe"

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Copy file: " + Source)
                    Logger.WriteDebug(CallStack, "To: " + Destination)

                    ' Copy the file
                    System.IO.File.Copy(Source, Destination, True)

                End If

                ' Check if LaunchService.exe was provided
                If Not System.IO.File.Exists(Globals.WorkingDirectory + "LaunchService.exe") Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Extract: " + Globals.WinOfflineTemp + "\LaunchService.exe")

                    ' Load embedded resource
                    Globals.MyResourceStream = Globals.MyAssembly.GetManifestResourceStream(Globals.MyRoot + "." + "LaunchService.exe")

                    ' Redimension the buffer to the size of the resource
                    ReDim Globals.MyBuffer(Convert.ToInt32(Globals.MyResourceStream.Length) - 1)

                    ' Read the resource into the buffer
                    Globals.MyResourceStream.Read(Globals.MyBuffer, 0, Globals.MyBuffer.Length)

                    ' Close the read stream
                    Globals.MyResourceStream.Close()

                    ' Clear destination location
                    Utility.DeleteFile(CallStack, Globals.WinOfflineTemp + "\LaunchService.exe")

                    ' Open the write stream
                    Globals.MyFileStream = New System.IO.FileStream(Globals.WinOfflineTemp + "\LaunchService.exe", System.IO.FileMode.Create, System.IO.FileAccess.Write)

                    ' Purge the buffer to the write stream
                    Globals.MyFileStream.Write(Globals.MyBuffer, 0, Globals.MyBuffer.Length)

                    ' Close the write stream
                    Globals.MyFileStream.Close()

                Else

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Alternative resource provided: " + Globals.WorkingDirectory + "LaunchService.exe")

                    ' Populate source
                    Source = Globals.WorkingDirectory + "LaunchService.exe"

                    ' Populate destination
                    Destination = Globals.WinOfflineTemp + "\LaunchService.exe"

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Copy file: " + Source)
                    Logger.WriteDebug(CallStack, "To: " + Destination)

                    ' Copy the file
                    System.IO.File.Copy(Source, Destination, True)

                End If

                ' Check if app.exe.config was provided
                If Not System.IO.File.Exists(Globals.WorkingDirectory + Globals.ProcessShortName + ".config") Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Extract: " + Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")

                    ' Load embedded resource
                    Globals.MyResourceStream = Globals.MyAssembly.GetManifestResourceStream(Globals.MyRoot + "." + "app.config")

                    ' Redimension the buffer to the size of the resource
                    ReDim Globals.MyBuffer(Convert.ToInt32(Globals.MyResourceStream.Length) - 1)

                    ' Read the resource into the buffer
                    Globals.MyResourceStream.Read(Globals.MyBuffer, 0, Globals.MyBuffer.Length)

                    ' Close the read stream
                    Globals.MyResourceStream.Close()

                    ' Clear destination location
                    Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")

                    ' Open the write stream
                    Globals.MyFileStream = New System.IO.FileStream(Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config", System.IO.FileMode.Create, System.IO.FileAccess.Write)

                    ' Purge the buffer to the write stream
                    Globals.MyFileStream.Write(Globals.MyBuffer, 0, Globals.MyBuffer.Length)

                    ' Close the write stream
                    Globals.MyFileStream.Close()

                End If

                ' Check for resource dump execution
                If Globals.DumpCazipxpSwitch Then

                    ' Deinitialize
                    DeInit(CallStack, True, False)

                    ' Hard exit
                    Environment.Exit(0)

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Error: Exception caught extracting embedded resources, cleaning up..")

                ' Perform cleanup
                DeInit(CallStack, True, True)

                ' Throw exception
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
        Public Shared Function SDStageIReInit(ByVal CallStack As String) As Integer

            ' Update call stack
            CallStack += "ReInit|"

            ' *****************************
            ' - Cleanup temp folder.
            ' *****************************

            Try

                ' Write debug
                Logger.WriteDebug(CallStack, "Delete Folder: " + Globals.WinOfflineTemp)

                ' Delete the folder
                System.IO.Directory.Delete(Globals.WinOfflineTemp, True)

                ' Write debug
                Logger.WriteDebug(CallStack, "Temporary folder deleted.")

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Exception caught deleting temporary folder.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

                ' Create exception
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

                ' Write debug
                Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder..")

                ' Delete temporary folder contents
                Utility.DeleteFolderContents(CallStack, Globals.WinOfflineTemp, Nothing)

            End Try

            ' *****************************
            ' - Reinitialize temp folder.
            ' *****************************

            ' Create temp folder
            Try

                ' Write debug
                Logger.WriteDebug(CallStack, "Create " + Globals.ProcessFriendlyName + " folder..")

                ' Check if temp folder already exists
                If System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " folder already exists: " + Globals.WinOfflineTemp)

                Else

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Create folder: " + Globals.WinOfflineTemp)

                    ' Create temp folder
                    System.IO.Directory.CreateDirectory(Globals.WinOfflineTemp)

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Exception caught creating temporary folder.")

                ' Return
                Return 1

            End Try

            ' Reinit embedded resources
            InitResources(CallStack)

            ' *****************************
            ' - Flush ALL Manifest data.
            ' *****************************

            ' Write debug
            Logger.WriteDebug(CallStack, "Flush ALL manifest data..")

            ' Flush all manifests
            Manifest.FlushManifestData()

            ' Write debug
            Logger.WriteDebug(CallStack, "Manifest data has been RESET.")

            ' *****************************
            ' - Reinit debug log.
            ' *****************************

            ' Write debug
            Logger.WriteDebug(CallStack, "Reinitialize debug log..")

            ' Rewrite debug log
            Logger.SDStageIReInitDebugLog()

            ' Write debug
            Logger.WriteDebug(CallStack, "Debug log reinitialized.")

            ' Return
            Return 0

        End Function

        ' Deinitialization function
        Public Shared Sub DeInit(ByVal CallStack As String,
                                 ByVal BypassSummary As Boolean,
                                 ByVal KeepDebugLog As Boolean)

            ' Local variables
            Dim FileList As String()
            Dim Today As String = DateTime.Now.ToString("yyyyMMdd")

            ' Update call stack
            CallStack += "DeInit|"

            ' *****************************
            ' - Cleanup temp folder.
            ' *****************************

            Try

                ' Check if temporary folder exists
                If Globals.WinOfflineTemp IsNot Nothing AndAlso System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Delete Folder: " + Globals.WinOfflineTemp)

                    ' Delete the folder
                    System.IO.Directory.Delete(Globals.WinOfflineTemp, True)

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Exception caught deleting temporary folder.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder..")

                ' Get the directory listing
                FileList = System.IO.Directory.GetFiles(Globals.WinOfflineTemp)

                ' Loop the file list
                For n As Integer = 0 To FileList.Length - 1

                    ' Attempt to delete each file
                    Utility.DeleteFile(CallStack, FileList(n))

                Next

                ' Write debug
                Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder completed.")

            End Try

            ' *****************************
            ' - WinOffline executable cleanup.
            ' *****************************

            ' Attempt to delete WinOffline executable and config file
            Try

                ' Delete WinOffline executable and config files
                Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName)
                Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Exception caught deleting file: " + Globals.WindowsTemp + "\" + Globals.ProcessShortName)
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

                ' Create exception
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            End Try

            ' *****************************
            ' - WinOffline log file cleanup.
            ' *****************************

            Try

                ' Verify ITCM directory is available
                If System.IO.Directory.Exists(Globals.DSMFolder) Then

                    ' Get a directory listing
                    FileList = System.IO.Directory.GetFiles(Globals.DSMFolder)

                    ' Loop the file list -- Check and delete prior debug logs
                    For n As Integer = 0 To FileList.Length - 1

                        ' Check for debug log
                        If FileList(n).ToLower.Contains(Globals.ProcessFriendlyName.ToLower) And
                                (FileList(n).ToLower.EndsWith(".debug") Or
                                FileList(n).ToLower.EndsWith(".log") Or
                                FileList(n).ToLower.EndsWith(".txt")) Then

                            ' Purge logs not from today
                            If Not System.IO.File.GetCreationTime(FileList(n)).ToString("yyyyMMdd").Equals(Today) Then

                                ' Delete the file
                                Utility.DeleteFile(CallStack, FileList(n))

                            End If

                        End If

                    Next

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Warning: Exception caught cleaning prior debug logs.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

                ' Create exception
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            End Try

            ' *****************************
            ' - Insert PatchTable summary to debug log.
            ' *****************************

            ' Check if summary is skipped
            If BypassSummary = False Then

                ' Insert the patch summary
                Logger.InsertSummary(CallStack)

                ' Write debug
                Logger.WriteDebug(CallStack, "Patch summary written.")

            End If

            ' *****************************
            ' - Flush manifest data.
            ' *****************************

            ' Flush manifest data
            If Manifest.FlushManifestData() Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Manifest data has been RESET.")

            End If

            ' *****************************
            ' - Terminate debug log.
            ' *****************************

            ' Terminate the debug log
            Logger.TermDebugLog(CallStack, KeepDebugLog)

            ' Write debug
            Logger.SetCurrentTask("Completed.")
            Logger.WriteDebug(CallStack, "Program terminated.")

            ' *****************************
            ' - Insert debug console summary.
            ' *****************************

            ' Check if summary is skipped
            If BypassSummary = False Then

                ' Insert debug console summary
                Logger.WriteDebug(Globals.ProgressGUISummary)

            End If

            ' *****************************
            ' - Detach from console (if attached).
            ' *****************************

            ' Detach from console
            WindowsAPI.DetachConsole()

        End Sub

    End Class

End Class
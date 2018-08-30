Imports System.IO
Imports System.Management
Imports System.Security.AccessControl

Partial Public Class WinOffline

    Public Class Utility

        Public Shared Sub AddToExistingArray(Of T)(ByRef AnArray As T(), NewItem As T)

            ' Check for empty array
            If AnArray Is Nothing Then

                ' Resize the array
                Array.Resize(AnArray, 1)

                ' Add the new element
                AnArray(0) = NewItem

            Else

                ' Resize the array
                Array.Resize(AnArray, AnArray.Length + 1)

                ' Add the new element
                AnArray(AnArray.Length - 1) = NewItem

            End If

        End Sub

        Public Shared Function ArrayListtoCommaString(ByVal InputArray As ArrayList, Optional ByVal Delimeter As String = ",") As String

            ' Local variables
            Dim ReturnString As String = ""

            ' Interate input arraylist elements
            For Each strLine As String In InputArray

                ' Append to return string
                ReturnString += strLine + Delimeter

            Next

            ' Return
            Return ReturnString.TrimEnd(Delimeter.ToCharArray)

        End Function

        Public Shared Function ChangeServiceMode(ByVal ServiceName As String, ByVal StartMode As String) As Boolean

            ' StartMode: Automatic, Manual, Disabled

            ' Local variabes
            Dim obj As ManagementObject
            Dim inParams As ManagementBaseObject
            Dim outParams As ManagementBaseObject

            ' Verify service exists
            If Not ServiceExists(ServiceName) Then Return False

            ' Get management object
            obj = New ManagementObject("\\" + Globals.HostName + "\root\cimv2:Win32_Service.Name='" + ServiceName + "'")

            ' Obtain management object input parameters
            inParams = obj.GetMethodParameters("ChangeStartMode")

            ' Update start mode parameter
            inParams("StartMode") = StartMode

            ' Invoke update
            outParams = obj.InvokeMethod("ChangeStartMode", inParams, Nothing)

            ' Check return
            If Convert.ToInt32(outParams("returnValue")) <> 0 Then Return False

            ' Return
            Return True

        End Function

        Public Shared Function CopyDirectory(ByVal SourceFolder As String,
                                             ByVal DestinationFolder As String) As Boolean

            ' Check if source folder exists
            If Not Directory.Exists(SourceFolder) Then

                ' Return -- Source folder does not exist
                Return False

            End If

            ' Check if destination folder exists
            If Directory.Exists(DestinationFolder) Then

                ' Return -- Destination folder already exists
                Return False

            Else

                ' Attempt to create destination folder
                Try

                    ' Create the destination folder
                    Directory.CreateDirectory(DestinationFolder)

                Catch ex As Exception

                    ' Something went wrong
                    Return False

                End Try

            End If

            ' Iterate each file
            For Each SourceFile As String In Directory.GetFiles(SourceFolder)

                ' Calculate destination file
                Dim DestinationFile As String = Path.Combine(DestinationFolder, System.IO.Path.GetFileName(SourceFile))

                ' Attempt file copy operation
                Try

                    ' Copy the file
                    File.Copy(SourceFile, DestinationFile)

                Catch ex As Exception

                    ' Something went wrong
                    Return False

                End Try

            Next

            ' Iterate each sub-folder
            For Each SubFolder As String In Directory.GetDirectories(SourceFolder)

                ' Calculate destination sub folder
                Dim DestinationSubFolder As String = Path.Combine(DestinationFolder, Path.GetFileName(SubFolder))

                ' Attempt recursive folder copy
                Try

                    ' Copy the sub folder
                    CopyDirectory(SubFolder, DestinationSubFolder)

                Catch ex As Exception

                    ' Something went wrong
                    Return False

                End Try

            Next

            ' Return
            Return True

        End Function

        Public Shared Function DeleteEnvironmentVariable(ByVal VariableName As String) As Boolean

            ' Check if variable exists
            If Environment.GetEnvironmentVariable(VariableName, EnvironmentVariableTarget.Machine) IsNot Nothing Then

                ' Delete it
                Environment.SetEnvironmentVariable(VariableName, Nothing, EnvironmentVariableTarget.Machine)

                ' Return
                Return True

            End If

            ' Return
            Return False

        End Function

        Public Shared Function DeleteFile(ByVal CallStack As String,
                                          ByVal FileName As String,
                                          Optional ByVal RaiseException As Boolean = False) As Boolean

            ' Local variables
            Dim FileDeleted As Boolean = False

            ' Check if file exists
            If System.IO.File.Exists(FileName) Then

                ' Encapsulate deletion
                Try

                    ' Unset read-only parameter (in case it's set)
                    System.IO.File.SetAttributes(FileName, IO.FileAttributes.Normal)

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Delete file: " + FileName)

                    ' Delete file
                    System.IO.File.Delete(FileName)

                    ' Set flag
                    FileDeleted = True

                Catch ex As Exception

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)

                    ' Check if exception should be cascaded
                    If RaiseException Then Throw ex

                End Try

            End If

            ' Return
            Return FileDeleted

        End Function

        Public Shared Function DeleteFilePattern(ByVal CallStack As String,
                                                 ByVal FolderName As String,
                                                 ByVal FilePattern As String,
                                                 Optional ByVal RaiseException As Boolean = False) As Boolean

            ' Local variables
            Dim FileDeleted As Boolean = False
            Dim FileList As String()
            Dim strFile As String

            ' Encapsulate file operations
            Try

                ' Check if folder exists
                If Not System.IO.Directory.Exists(FolderName) Then Return False

                ' Get the directory listing
                FileList = System.IO.Directory.GetFiles(FolderName)

                ' Check for positive files
                If FileList.Length > 0 Then

                    ' Loop the file list
                    For n As Integer = 0 To FileList.Length - 1

                        ' Get a filename
                        strFile = FileList(n).ToString.ToLower
                        strFile = strFile.Substring(strFile.LastIndexOf("\") + 1)

                        ' Check if filename starts with specified pattern
                        If strFile.ToLower.StartsWith(FilePattern.ToLower) Then

                            ' Delete the file
                            DeleteFile(CallStack, FileList(n))

                            ' Set flag
                            FileDeleted = True

                        End If

                    Next

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

                ' Check if exception should be cascaded
                If RaiseException Then Throw ex

            End Try

            ' Return
            Return FileDeleted

        End Function

        Public Shared Function DeleteFolder(ByVal CallStack As String,
                                            ByVal FolderName As String,
                                            Optional ByVal RaiseException As Boolean = False) As Boolean

            ' Local variables
            Dim FolderDeleted As Boolean = False

            ' Check if folder exists
            If System.IO.Directory.Exists(FolderName) Then

                ' Encapsulate deletion
                Try

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Delete folder: " + FolderName)

                    ' Delete folder contents
                    DeleteFolderContents(CallStack, FolderName, Nothing)

                    ' Attempt to delete folder
                    System.IO.Directory.Delete(FolderName, True)

                    ' Set flag
                    FolderDeleted = True

                Catch ex As Exception

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Warning: Exception caught deleting folder.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)

                    ' Check if exception should be cascaded
                    If RaiseException Then Throw ex

                End Try

            End If

            ' Return
            Return FolderDeleted

        End Function

        Public Shared Sub DeleteFolderContents(ByVal CallStack As String,
                                               ByVal TargetFolder As String,
                                               ByVal ReservedItems As String(),
                                               Optional ByVal VerboseOutput As Boolean = True)

            ' Local variables
            Dim FileList As String()
            Dim FolderList As String()
            Dim TargetFolderInfo As DirectoryInfo
            Dim TargetFolderACL As DirectorySecurity
            Dim FolderInfo As DirectoryInfo
            Dim SkipItem As Boolean = False

            ' Verify directory exists
            If Not System.IO.Directory.Exists(TargetFolder) Then

                ' Return
                Return

            End If

            ' Get the directory listing
            FileList = Directory.GetFiles(TargetFolder)
            FolderList = Directory.GetDirectories(TargetFolder)

            ' Adjust TargetFolder ACL, add permissions for BUILTIN\Administrators group
            Try

                ' Get TargetFolder access control list
                TargetFolderInfo = New DirectoryInfo(TargetFolder)
                TargetFolderACL = New DirectorySecurity(TargetFolder, AccessControlSections.Access)

                ' Authorize BUILTIN\Administrators for FULL CONTROL with inheritance
                TargetFolderACL.AddAccessRule(New FileSystemAccessRule(
                                              New Security.Principal.SecurityIdentifier(
                                                  Security.Principal.WellKnownSidType.BuiltinAdministratorsSid, Nothing),
                                              FileSystemRights.FullControl,
                                              AccessControlType.Allow,
                                              PropagationFlags.InheritOnly,
                                              AccessControlType.Allow))

                ' Update the access control list
                TargetFolderInfo.SetAccessControl(TargetFolderACL)

                ' Write debug -- Suppress
                'Logger.WriteDebug(CallStack, "Target folder ACL updated: " + TargetFolder)

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Failed to set target folder access control list.")
                Logger.WriteDebug(ex.Message)

                ' Create exception
                Manifest.UpdateManifest(CallStack,
                                        Manifest.EXCEPTION_MANIFEST,
                                        {"Error: Failed to set target folder access control list.",
                                        "Reason: Please analyze the debug log for exception message."})

            End Try

            ' Check for positive folders
            If FolderList.Length > 0 Then

                ' Loop the folder list
                For n As Integer = 0 To FolderList.Length - 1

                    ' Attempt to delete each folder
                    Try

                        ' Check if there are any reserved items
                        If ReservedItems IsNot Nothing Then

                            ' Check for reserved folder
                            For Each str As String In ReservedItems

                                ' Check for match
                                If FolderList(n).ToString.ToLower.EndsWith(str.ToLower) Then

                                    ' Write debug
                                    Logger.WriteDebug(CallStack, "Reserved folder: " + FolderList(n).ToString)

                                    ' Set marker
                                    SkipItem = True

                                End If

                            Next

                        End If

                        ' Delete the folder
                        If Not SkipItem Then

                            ' Get folder attributes
                            FolderInfo = New DirectoryInfo(FolderList(n).ToString)

                            ' Check for an ntfs junction (instead of a normal folder)
                            If FolderInfo.Attributes And FileAttributes.ReparsePoint Then

                                ' Call unmanaged code in an attempt to remove the ntfs junction point
                                Try

                                    ' Write debug
                                    Logger.WriteDebug(CallStack, "Remove junction: " + FolderList(n).ToString)

                                    ' Remove the junction point
                                    WindowsAPI.RemoveJunction(FolderList(n).ToString)

                                Catch ex As Exception

                                    ' Write debug
                                    Logger.WriteDebug(CallStack, "Warning: Exception caught removing NTFS junction: " + FolderList(n).ToString)
                                    Logger.WriteDebug(ex.Message)

                                End Try

                            Else

                                ' Recurse into folder structure
                                DeleteFolderContents(CallStack, FolderList(n).ToString, ReservedItems, False)

                            End If

                            ' Check for verbose output
                            If VerboseOutput Then

                                ' Write debug
                                Logger.WriteDebug(CallStack, "Delete folder: " + FolderList(n).ToString)

                            End If

                            ' After recurse is finished, delete the folder
                            Directory.Delete(FolderList(n), True)

                        End If

                        ' Reset marker
                        SkipItem = False

                    Catch ex2 As Exception

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Exception caught deleting folder: " + FolderList(n).ToString)
                        Logger.WriteDebug(ex2.Message)

                    End Try

                Next

            End If

            ' Check for positive files
            If FileList.Length > 0 Then

                ' Loop the file list
                For n As Integer = 0 To FileList.Length - 1

                    ' Attempt to delete each file
                    Try

                        ' Check if there are reserved items
                        If ReservedItems IsNot Nothing Then

                            ' Process reserved items
                            For Each str As String In ReservedItems

                                ' Check for match
                                If FileList(n).ToString.ToLower.EndsWith(str.ToLower) Then

                                    ' Write debug
                                    Logger.WriteDebug(CallStack, "Reserved file: " + FileList(n).ToString)

                                    ' Set marker
                                    SkipItem = True

                                End If

                            Next

                        End If

                        ' Delete the file
                        If Not SkipItem Then

                            ' Check for verbose output
                            If VerboseOutput Then

                                ' Write debug
                                Logger.WriteDebug(CallStack, "Delete file: " + FileList(n).ToString)

                            End If

                            ' Unset read-only parameter (in case it's set)
                            File.SetAttributes(FileList(n), IO.FileAttributes.Normal)

                            ' Delete the file
                            File.Delete(FileList(n))

                        End If

                        ' Reset marker
                        SkipItem = False

                    Catch ex2 As Exception

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file: " + FileList(n).ToString)

                    End Try

                Next

            End If

        End Sub

        Public Shared Function DeleteRegistrySubKey(ByVal regTree As String, ByVal regKey As String) As Boolean

            ' Local variables
            Dim regTest As Microsoft.Win32.RegistryKey

            ' Check tree specification
            If regTree.ToUpper.Equals("HKLM") Then

                ' Encapsulate deletion
                Try

                    ' Open registry key
                    regTest = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey, False)

                    ' Check if registry key exists
                    If regTest Is Nothing Then

                        ' Return
                        Return False

                    Else

                        ' Attempt registry deletion
                        Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(regKey, True)

                        ' Write debug
                        Logger.WriteDebug(Logger.LastCallStack, "Delete registry: HKLM\" + regKey)

                        ' Close registry key
                        regTest.Close()

                    End If

                Catch ex As Exception

                    ' Failed to delete -- return false
                    Return False

                End Try

                ' Return
                Return True

            ElseIf regTree.ToUpper.Equals("HKCR") Then

                ' Encapsulate deletion
                Try

                    ' Open registry key
                    regTest = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(regKey, False)

                    ' Check if registry key exists
                    If regTest Is Nothing Then

                        ' Return
                        Return False

                    Else

                        ' Attempt registry deletion
                        Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(regKey, True)

                        ' Write debug
                        Logger.WriteDebug(Logger.LastCallStack, "Delete registry: HKCR\" + regKey)

                        ' Close registry key
                        regTest.Close()

                    End If

                Catch ex As Exception

                    ' Failed to delete -- return false
                    Return False

                End Try

                ' Return
                Return True

            Else

                ' Throw exception
                Throw New Exception("Unknown or unsupported registry tree specified: " + regTree)

            End If

        End Function

        Public Shared Function DeleteRegistrySubKeyTree(ByVal regTree As String, ByVal regKey As String) As Boolean

            ' Local variables
            Dim regTest As Microsoft.Win32.RegistryKey

            ' Check tree specification
            If regTree.ToUpper.Equals("HKLM") Then

                ' Encapsulate deletion
                Try

                    ' Open registry key
                    regTest = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey, False)

                    ' Check if registry key exists
                    If regTest Is Nothing Then

                        ' Return
                        Return False

                    Else

                        ' Attempt registry deletion
                        Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(regKey, True)

                        ' Close registry key
                        regTest.Close()

                    End If

                Catch ex As Exception

                    ' Failed to delete -- return false
                    Return False

                End Try

                ' Return
                Return True

            ElseIf regTree.ToUpper.Equals("HKCR") Then

                ' Encapsulate deletion
                Try

                    ' Open registry key
                    regTest = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(regKey, False)

                    ' Check if registry key exists
                    If regTest Is Nothing Then

                        ' Return
                        Return False

                    Else

                        ' Attempt registry deletion
                        Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(regKey, True)

                        ' Close registry key
                        regTest.Close()

                    End If

                Catch ex As Exception

                    ' Failed to delete -- return false
                    Return False

                End Try

                ' Return
                Return True

            Else

                ' Throw exception
                Throw New Exception("Unknown or unsupported registry tree specified: " + regTree)

            End If

        End Function

        Public Shared Function DeleteRegistrySubKeysWithValue(ByVal regTree As String, ByVal RootKey As String, ByVal MatchName As String, ByVal MatchValue As String) As Boolean

            ' Local variables
            Dim TestKey As Microsoft.Win32.RegistryKey
            Dim regSubKey As Microsoft.Win32.RegistryKey
            Dim regSubKeyValue As String = Nothing

            ' Check tree specification
            If regTree.ToUpper.Equals("HKLM") Then

                ' Encapsulate registry crawl
                Try

                    ' Open registry key
                    TestKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RootKey, False)

                    ' Verify it exists, otherwise exit this block
                    If TestKey Is Nothing Then Exit Try

                    ' Iterate subkeys
                    For Each subKey As String In TestKey.GetSubKeyNames

                        ' Open registry key
                        regSubKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RootKey + "\" + subKey, False)

                        ' Read registry value
                        regSubKeyValue = regSubKey.GetValue(MatchName)

                        ' Verify registry value
                        If regSubKeyValue IsNot Nothing AndAlso regSubKeyValue.ToLower.Equals(MatchValue.ToLower) Then

                            ' Close subkey
                            regSubKey.Close()

                            ' Delete subkey and return
                            Return DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)

                        ElseIf regSubKeyValue Is Nothing AndAlso regSubKey.SubKeyCount > 0 Then

                            ' Recurse
                            If DeleteRegistrySubKeysWithValue(regTree, RootKey + "\" + subKey, MatchName, MatchValue) Then

                                ' Delete subkey
                                DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)

                            End If

                        Else

                            ' Close subkey
                            regSubKey.Close()

                            ' Continue loop
                            Continue For

                        End If

                    Next

                    ' Close registry key
                    TestKey.Close()

                Catch ex As Exception

                    ' Return
                    Return False

                End Try

                ' Return
                Return False

            ElseIf regTree.ToUpper.Equals("HKCR") Then

                ' Encapsulate registry crawl
                Try

                    ' Open registry key
                    TestKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(RootKey, False)

                    ' Verify it exists, otherwise exit this block
                    If TestKey Is Nothing Then Exit Try

                    ' Iterate subkeys
                    For Each subKey As String In TestKey.GetSubKeyNames

                        ' Open registry key
                        regSubKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(RootKey + "\" + subKey, False)

                        ' Read registry value
                        regSubKeyValue = regSubKey.GetValue(MatchName)

                        ' Verify registry value
                        If regSubKeyValue IsNot Nothing AndAlso regSubKeyValue.ToLower.Equals(MatchValue.ToLower) Then

                            ' Close subkey
                            regSubKey.Close()

                            ' Delete subkey and return
                            Return DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)

                        ElseIf regSubKeyValue Is Nothing AndAlso regSubKey.SubKeyCount > 0 Then

                            ' Recurse
                            If DeleteRegistrySubKeysWithValue(regTree, RootKey + "\" + subKey, MatchName, MatchValue) Then

                                ' Delete subkey
                                DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)

                            End If

                        Else

                            ' Close subkey
                            regSubKey.Close()

                            ' Continue loop
                            Continue For

                        End If

                    Next

                    ' Close registry key
                    TestKey.Close()

                Catch ex As Exception

                    ' Return
                    Return False

                End Try

                ' Return
                Return False

            Else

                ' Throw exception
                Throw New Exception("Unknown or unsupported registry tree specified: " + regTree)

            End If

        End Function

        Public Shared Function GetParallelProcesses(ByVal ProcessFriendlyName As String) As ArrayList

            ' Local variables
            Dim ParallelProcessList As New ArrayList

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses

                ' Match the process name
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then

                    ' Match found
                    ParallelProcessList.Add(RunningProcess)

                End If

            Next

            ' Return
            Return ParallelProcessList

        End Function

        Public Shared Function GetProcessChildren(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String, Optional ByVal FilterList As ArrayList = Nothing) As ArrayList

            ' Local variables
            Dim ParentPID As Integer
            Dim ChildArrayList As New ArrayList

            ' Each ArrayList within the ChildArrayList will contain:
            ' (0) = ProcessID, Ex: "1234"
            ' (1) = Name, Ex: "cmEngine.exe"
            ' (2) = ExecutablePath, Ex: "C:\Program Files (x86)\CA\DSM\Bin\cmEngine.exe"
            ' (3) = CommandLine, Ex: "C:\Program Files (x86)\CA\DSM\Bin\cmEngine.exe -name SystemEngine"
            ' (4) = WorkingSetSize, in bytes, Ex: 1234567

            ' Encapsulate the operation
            Try

                ' Obtain the parent process id
                ParentPID = GetProcessID(ProcessShortName, ContainsCommandLine)

                ' Query WMI for list of processes matching the parent id
                Dim WMIQuery As New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ParentProcessId='" + ParentPID.ToString + "'")

                ' Iterate the process list
                For Each WMIProcess As ManagementObject In WMIQuery.Get()

                    ' Check for valid data
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso
                        WMIProcess("Name") IsNot Nothing AndAlso
                        WMIProcess("ExecutablePath") IsNot Nothing AndAlso
                        WMIProcess("CommandLine") IsNot Nothing AndAlso
                        WMIProcess("WorkingSetSize") IsNot Nothing Then

                        ' Check the optional filter list
                        If FilterList IsNot Nothing AndAlso FilterList.Contains(WMIProcess("Name").ToString.ToLower) Then

                            ' Skip this result
                            Continue For

                        Else

                            ' Add process to the array
                            ChildArrayList.Add(New ArrayList({WMIProcess("ProcessID"),
                                                             WMIProcess("Name"),
                                                             WMIProcess("ExecutablePath"),
                                                             WMIProcess("CommandLine"),
                                                             WMIProcess("WorkingSetSize")}))

                        End If

                    Else

                        ' Missing required attributes -- skip
                        Continue For

                    End If

                Next

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught querying process children.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Return
            Return ChildArrayList

        End Function

        Public Shared Function GetProcessFileName(ByVal ProcessFriendlyName As String) As String

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses()

                ' Match the process name
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then

                    ' Return process path
                    Return RunningProcess.MainModule.FileName

                End If

            Next

            ' Process not found
            Return Nothing

        End Function

        Public Shared Function GetProcessID(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String) As Integer

            ' Local variables
            Dim ProcessSearcher As ManagementObjectSearcher
            Dim ProcessId As String = Nothing
            Dim CommandLine As String = Nothing

            ' Encapsulate the operation
            Try

                ' Query WMI for list of processes mathing the name
                ProcessSearcher = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")

                ' Iterate the process list
                For Each WMIProcess As ManagementObject In ProcessSearcher.Get()

                    ' Check for valid data
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso WMIProcess("CommandLine") IsNot Nothing Then

                        ' Retrieve management object properties
                        ProcessId = WMIProcess("ProcessId").ToString
                        CommandLine = WMIProcess("CommandLine").ToString

                    Else

                        ' Missing required attributes -- skip
                        Continue For

                    End If

                    ' Check for match by command line
                    If CommandLine IsNot Nothing AndAlso CommandLine.ToLower.Contains(ContainsCommandLine.ToLower) Then

                        ' Return
                        Return Integer.Parse(ProcessId)

                    End If

                Next

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught querying process id.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Return
            Return 0

        End Function

        Public Shared Function GetProcessPath(ByVal ProcessFriendlyName As String) As String

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses()

                ' Match the process name
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then

                    ' Return process path
                    Return RunningProcess.MainModule.FileName.Substring(0, RunningProcess.MainModule.FileName.LastIndexOf("\"))

                End If

            Next

            ' Process not found
            Return Nothing

        End Function

        Public Shared Function GetProcessPathWithSlash(ByVal ProcessFriendlyName As String) As String

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses()

                ' Match the process name
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then

                    ' Return process path
                    Return RunningProcess.MainModule.FileName.Substring(0, RunningProcess.MainModule.FileName.LastIndexOf("\") + 1)

                End If

            Next

            ' Process not found
            Return Nothing

        End Function

        Public Shared Function GetProcessWorkingSetMemorySize(ByVal ProcessId As Integer) As Double

            ' Local variables
            Dim WorkingMemory As Double

            ' Encapsulate the operation
            Try

                ' Query WMI for list of processes matching the process id 
                Dim WMIQuery As New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId='" + ProcessId.ToString + "'")

                ' Iterate the process list (should only be one process -- or none)
                For Each WMIProcess As ManagementObject In WMIQuery.Get()

                    ' Check for valid data
                    If WMIProcess("WorkingSetSize") IsNot Nothing Then

                        ' Obtain working set memory size
                        WorkingMemory = WMIProcess("WorkingSetSize")

                        ' Return
                        Return WorkingMemory

                    Else

                        ' Missing required attributes -- skip
                        Continue For

                    End If

                Next

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught querying process working set memory size.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Return
            Return 0

        End Function

        Public Shared Function InlineAssignHelper(Of T)(ByRef Target As T, ByVal Value As T) As T

            ' Set target to value
            Target = Value

            ' Return the value
            Return Value

        End Function

        Public Shared Function Is64BitOperatingSystem() As Boolean

            ' Local variables
            Dim OSClass As System.Management.ManagementClass = Nothing
            Dim result As String = "32-bit"

            ' Check .NET framework major version
            If Globals.DotNetVersion.StartsWith("2") OrElse Globals.DotNetVersion.StartsWith("3") Then

                ' Encapsulate WMI interaction
                Try

                    ' Initialize CIM
                    OSClass = New System.Management.ManagementClass("Win32_OperatingSystem")

                    ' Iterate CIM instances
                    For Each mObject As System.Management.ManagementObject In OSClass.GetInstances

                        ' Iterate CIM properties
                        For Each mProp As System.Management.PropertyData In mObject.Properties

                            ' Check of OS Architecture property
                            If mProp.Name = "OSArchitecture" Then

                                ' Assign result
                                result = mProp.Value.ToString

                                ' Exit condition
                                Exit For

                            End If

                        Next

                    Next

                Catch ex As Exception

                    ' Assign empty string
                    result = String.Empty

                Finally

                    ' Cleanup
                    If OSClass IsNot Nothing Then OSClass.Dispose()

                End Try

            Else

                ' Return
                Return Environment.Is64BitOperatingSystem

            End If

            ' Check result
            If result.Equals(String.Empty) OrElse result.Equals("32-bit") Then

                ' Return
                Return False

            Else

                ' Return
                Return True

            End If

        End Function

        Public Shared Function IsArrayEqual(ByVal ArrayListA As Array,
                                            ByVal ArrayListB As Array) As Boolean

            ' Local variables
            Dim MatchFound As Boolean = False

            ' Quick check element counts
            If ArrayListA.Length <> ArrayListB.Length Then Return False

            ' Iterate ArrayListA
            For i As Integer = 0 To ArrayListA.Length - 1

                ' Iterate ArrayListB
                For j As Integer = 0 To ArrayListB.Length - 1

                    ' Reset flag
                    MatchFound = False

                    ' Check for a match
                    If ArrayListA(i).ToString.Equals(ArrayListB(j).ToString) Then

                        ' Set flag
                        MatchFound = True

                        ' Break inner loop
                        Exit For

                    End If

                Next

                ' Ensure a match was made
                If MatchFound = False Then Return False

            Next

            ' Return
            Return True

        End Function

        Public Shared Function IsArrayListEqual(ByVal ArrayListA As ArrayList,
                                                ByVal ArrayListB As ArrayList) As Boolean

            ' Local variables
            Dim MatchFound As Boolean = False

            ' Quick check element counts
            If ArrayListA.Count <> ArrayListB.Count Then Return False

            ' Iterate ArrayListA
            For i As Integer = 0 To ArrayListA.Count - 1

                ' Iterate ArrayListB
                For j As Integer = 0 To ArrayListB.Count - 1

                    ' Reset flag
                    MatchFound = False

                    ' Check for a match
                    If ArrayListA.Item(i).ToString.Equals(ArrayListB.Item(j).ToString) Then

                        ' Set flag
                        MatchFound = True

                        ' Break inner loop
                        Exit For

                    End If

                Next

                ' Ensure a match was made
                If MatchFound = False Then Return False

            Next

            ' Return
            Return True

        End Function

        Public Shared Function IsFileEqual(file1 As String, file2 As String) As Boolean

            ' Local variables
            Dim file1byte As Integer
            Dim file2byte As Integer
            Dim fs1 As FileStream
            Dim fs2 As FileStream

            ' Check if same files were passed
            If file1 = file2 Then Return True

            ' Check if both files exist
            If Not System.IO.File.Exists(file1) Then Return False
            If Not System.IO.File.Exists(file2) Then Return False

            ' Encapsulate file ops
            Try

                ' Open both files for comparison
                fs1 = New FileStream(file1, FileMode.Open, FileAccess.Read)
                fs2 = New FileStream(file2, FileMode.Open, FileAccess.Read)

                ' Compare file length
                If fs1.Length <> fs2.Length Then

                    ' Close files
                    fs1.Close()
                    fs2.Close()

                    ' Return
                    Return False

                End If

                ' Compare bytes
                Do

                    ' Read a byte
                    file1byte = fs1.ReadByte()
                    file2byte = fs2.ReadByte()

                Loop While (file1byte = file2byte) AndAlso (file1byte <> -1)

                ' Close files
                fs1.Close()
                fs2.Close()

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(Logger.LastCallStack, "Error: Failed to compare files.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)

                ' Return
                Return False

            End Try

            ' Return
            Return ((file1byte - file2byte) = 0)

        End Function

        Public Shared Function IsFileOpen(ByVal FileName As String) As Boolean

            ' Local variables
            Dim FileInfo As New FileInfo(FileName)
            Dim FileStream As FileStream = Nothing

            ' Open the file
            Try

                ' Open the file, specifying no file sharing
                FileStream = FileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)

                ' Close the stream
                FileStream.Close()

                ' Return
                Return False

            Catch ex As Exception

                ' Return
                Return True

            End Try

        End Function

        Public Shared Function IsHexChar(ByVal c As Char) As Boolean

            ' Local variables
            Dim result As Integer

            ' Check numbers
            If Integer.TryParse(c.ToString, result) AndAlso
                result >= 0 AndAlso
                result <= 9 Then Return True

            ' Check letters
            If c.ToString.ToLower.Equals("a") Then Return True
            If c.ToString.ToLower.Equals("b") Then Return True
            If c.ToString.ToLower.Equals("c") Then Return True
            If c.ToString.ToLower.Equals("d") Then Return True
            If c.ToString.ToLower.Equals("e") Then Return True
            If c.ToString.ToLower.Equals("f") Then Return True

            ' Not a hex character
            Return False

        End Function

        Public Shared Function IsHexString(ByVal s As String) As Boolean

            ' Iterate each character
            For Each c As Char In s

                ' Verify a HEX character, otherwise fail
                If Not IsHexChar(c) Then Return False

            Next

            ' String is HEX
            Return True

        End Function

        Public Shared Function IsITCMInstalled() As Boolean

            ' Simple check -- Is caf running?
            If IsProcessRunning("caf") Then Return True

            ' Local variables
            Dim ProductInfoKey As Microsoft.Win32.RegistryKey = Nothing

            ' Read 64-bit registry -- Unicenter ITRM
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)

            ' Check 64-bit registry -- Unicenter ITRM
            If ProductInfoKey Is Nothing Then

                ' Read 32-bit registry -- Unicenter ITRM
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)

                ' Check 32-bit registry -- Unicenter ITRM
                If ProductInfoKey Is Nothing Then

                    ' Return
                    Return False

                End If

            End If

            ' Check for key/value existence
            If ProductInfoKey.GetValue("InstallDirProduct") IsNot Nothing Then

                ' Read the installation directory value
                Globals.DSMFolder = ProductInfoKey.GetValue("InstallDirProduct").ToString()

            Else

                ' Return
                Return False

            End If

            ' Close registry key
            ProductInfoKey.Close()

            ' Check if directory exists
            If System.IO.Directory.Exists(Globals.DSMFolder) AndAlso
                System.IO.Directory.Exists(Globals.DSMFolder + "bin") AndAlso
                System.IO.File.Exists(Globals.DSMFolder + "bin\caf.exe") Then

                ' Return
                Return True

            Else

                ' Return
                Return False

            End If

        End Function

        Public Shared Function IsProcessRunning(ByVal ProcessId As Integer) As Boolean

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses()

                ' Match the process name
                If RunningProcess.Id = ProcessId Then

                    ' Match found
                    Return True

                End If

            Next

            ' Process not found
            Return False

        End Function

        Public Shared Function IsProcessRunning(ByVal ProcessFriendlyName As String) As Boolean

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses()

                ' Match the process name
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then

                    ' Match found
                    Return True

                End If

            Next

            ' Process not found
            Return False

        End Function

        Public Shared Function IsProcessRunning(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String) As Boolean

            ' Local variables
            Dim WMIQuery As ManagementObjectSearcher
            Dim CommandLine As String = Nothing

            ' Query WMI filtering by process name
            WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")

            ' Iterate each process
            For Each WMIProcess As ManagementObject In WMIQuery.Get()

                ' Check for valid data
                If WMIProcess("CommandLine") IsNot Nothing Then

                    ' Retrive management object property
                    CommandLine = WMIProcess("CommandLine").ToString

                Else

                    ' Missing required attribute for comparison -- skip
                    Continue For

                End If

                ' Check for match by command line contents
                If CommandLine IsNot Nothing AndAlso CommandLine.ToLower.Contains(ContainsCommandLine.ToLower) Then

                    ' Match found
                    Return True

                End If

            Next

            ' Process not found
            Return False

        End Function

        Public Shared Function IsProcessRunningEx(ByVal ProcessName As String) As Boolean

            ' Local variables
            Dim WMIQuery As ManagementObjectSearcher
            Dim ExecutablePath As String = Nothing

            ' Query WMI filtering by process name
            WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + FileVector.GetShortName(ProcessName) + "'")

            ' Iterate each process
            For Each WMIProcess As ManagementObject In WMIQuery.Get()

                ' Check for valid data
                If WMIProcess("ExecutablePath") IsNot Nothing Then

                    ' Retrive management object property
                    ExecutablePath = WMIProcess("ExecutablePath").ToString

                Else

                    ' Missing required attribute for comparison -- skip
                    Continue For

                End If

                ' Check for match by command line contents
                If ExecutablePath IsNot Nothing AndAlso ExecutablePath.ToLower.Equals(ProcessName.ToLower) Then

                    ' Match found
                    Return True

                End If

            Next

            ' Process not found
            Return False

        End Function

        Public Shared Function IsProcessRunningCount(ByVal ProcessFriendlyName As String) As Integer

            ' Local variables
            Dim processCount As Integer = 0

            ' Loop running processes
            For Each RunningProcess As Process In Process.GetProcesses

                ' Match the process name
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then

                    ' Match found
                    processCount += 1

                End If

            Next

            ' Return process count
            Return processCount

        End Function

        Public Shared Function IsServiceEnabled(ByVal ServiceName As String) As Boolean

            ' Local variabes
            Dim obj As ManagementObject

            ' Get management object
            obj = New ManagementObject("\\" + Globals.HostName + "\root\cimv2:Win32_Service.Name='" + ServiceName + "'")

            ' Check the start mode
            If obj("StartMode").ToString.ToLower.Equals("disabled") Then

                ' Return
                Return False

            Else

                ' Return
                Return True

            End If

        End Function

        Public Shared Function KillProcess(ByVal FileName As String) As Boolean

            ' Local variables
            Dim MatchFound As Boolean = False

            ' Encapsulate the operation
            Try

                ' Loop running processes
                For Each RunningProcess As Process In Process.GetProcesses()

                    ' Match the process name
                    If RunningProcess.ProcessName.ToLower.Equals(FileName.ToLower) Then

                        ' Match found
                        MatchFound = True

                        ' Kill process
                        RunningProcess.Kill()

                    End If

                Next

            Catch ex As Exception

                ' Write debug -- Suppress
                'Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught terminating process.")
                'Logger.WriteDebug(Logger.LastCallStack, "Requested process name: " + FileName)
                'Logger.WriteDebug(ex.Message)
                'Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Return
            Return MatchFound

        End Function

        Public Shared Function KillProcess(ByVal ProcessID As Integer) As Boolean

            ' Encapsulate the operation
            Try

                ' Loop running processes
                For Each RunningProcess As Process In Process.GetProcesses()

                    ' Match the PID
                    If RunningProcess.Id = ProcessID Then

                        ' Kill process
                        RunningProcess.Kill()

                        ' Return
                        Return True

                    End If

                Next

            Catch ex As Exception

                ' Write debug -- Suppress
                'Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught terminating process.")
                'Logger.WriteDebug(Logger.LastCallStack, "Requested process ID: " + ProcessID.ToString)
                'Logger.WriteDebug(ex.Message)
                'Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Return
            Return False

        End Function

        Public Shared Function KillProcessByCommandLine(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String) As Boolean

            ' Local variables
            Dim WMIQuery As ManagementObjectSearcher
            Dim ProcessId As String = Nothing
            Dim CommandLine As String = Nothing
            Dim ProcessFound As Boolean = False

            ' Encapsualte the operation
            Try

                ' Query WMI filtering by process name
                WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")

                ' Iterate each process
                For Each WMIProcess As ManagementObject In WMIQuery.Get()

                    ' Check for valid data
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso WMIProcess("CommandLine") IsNot Nothing Then

                        ' Retrieve management object properties
                        ProcessId = WMIProcess("ProcessID").ToString
                        CommandLine = WMIProcess("CommandLine").ToString

                    Else

                        ' Missing required attributes -- skip
                        Continue For

                    End If

                    ' Check for a match based on command line
                    If CommandLine IsNot Nothing AndAlso CommandLine.ToLower.Contains(ContainsCommandLine.ToLower) Then

                        ' Match found
                        ProcessFound = True

                        ' Kill process
                        KillProcess(Integer.Parse(ProcessId))

                    End If

                Next

            Catch ex As Exception

                ' Write debug -- Suppress
                'Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught terminating process.")
                'Logger.WriteDebug(Logger.LastCallStack, "Requested process name: " + ProcessShortName)
                'Logger.WriteDebug(Logger.LastCallStack, "Requested command line: " + ContainsCommandLine)
                'Logger.WriteDebug(ex.Message)
                'Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Process not found
            Return ProcessFound

        End Function

        Public Shared Function KillProcessByPath(ByVal ProcessShortName As String, ByVal ProcessPathContains As String) As Boolean

            ' Local variables
            Dim WMIQuery As ManagementObjectSearcher
            Dim ProcessId As String = Nothing
            Dim ExecutablePath As String = Nothing
            Dim ProcessFound As Boolean = False

            ' Encapsulate the operation
            Try

                ' Query WMI filtering by process name
                WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")

                ' Iterate each process
                For Each WMIProcess As ManagementObject In WMIQuery.Get()

                    ' Check for valid data
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso WMIProcess("ExecutablePath") IsNot Nothing Then

                        ' Retrieve management object properties
                        ProcessId = WMIProcess("ProcessID").ToString
                        ExecutablePath = WMIProcess("ExecutablePath").ToString

                    Else

                        ' Missing required attributes -- skip
                        Continue For

                    End If

                    ' Check for a match based on executable path
                    If ExecutablePath IsNot Nothing AndAlso ExecutablePath.ToLower.Contains(ProcessPathContains.ToLower) Then

                        ' Match found
                        ProcessFound = True

                        ' Kill process
                        KillProcess(Integer.Parse(ProcessId))

                    End If

                Next

            Catch ex As Exception

                ' Write debug -- Suppress
                'Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught terminating process.")
                'Logger.WriteDebug(Logger.LastCallStack, "Requested process name: " + ProcessShortName)
                'Logger.WriteDebug(Logger.LastCallStack, "Requested process path: " + ProcessPathContains)
                'Logger.WriteDebug(ex.Message)
                'Logger.WriteDebug(ex.StackTrace)

            End Try

            ' Process not found
            Return ProcessFound

        End Function

        Public Shared Function RegistryKeyExists(ByVal regKey As String) As Boolean

            ' Local variables
            Dim regTest As Microsoft.Win32.RegistryKey

            ' Encapsulate deletion
            Try

                ' Open registry key
                regTest = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey, False)

                ' Check if registry key exists
                If regTest Is Nothing Then

                    ' Return
                    Return False

                Else

                    ' Return
                    Return True

                End If

            Catch ex As Exception

                ' Failed to open -- return false
                Return False

            End Try

        End Function

        Public Shared Sub RemoveFirstFromExistingArray(Of T)(ByRef AnArray As T(), RemoveItem As T)

            ' Local variables
            Dim TempList As New ArrayList

            ' Copy array elements to an ArrayList
            For Each Element As T In AnArray
                TempList.Add(Element)
            Next

            ' Remove first occurence of requested item
            TempList.Remove(RemoveItem)

            ' Reassign values to array
            For x As Integer = 0 To TempList.Count - 1
                AnArray(x) = TempList.Item(x)
            Next

            ' Resize the array
            Array.Resize(AnArray, AnArray.Length - 1)

        End Sub

        Public Shared Function ReplaceWord(ByVal InputString As String,
                                           ByVal FindWord As String,
                                           ByVal Replacement As String) As String

            ' Local variables
            Dim OutputString As String = ""
            Dim CharBuffer As String = ""
            Dim FindWordIndex As Integer = 0

            ' Input verifications
            If InputString Is Nothing OrElse InputString.Equals("") Then Return ""
            If FindWord Is Nothing OrElse FindWord.Equals("") Then Return ""
            If Replacement Is Nothing Then Return ""

            ' Iterate each character of the input string
            For Each k As Char In InputString

                ' Check if the character matches
                If k.ToString.ToLower.Equals(FindWord.Chars(FindWordIndex).ToString.ToLower) Then

                    ' Add character to buffer
                    CharBuffer += k

                    ' Check if buffer length matches the FindWord
                    If CharBuffer.Length = FindWord.Length Then

                        ' Substitute the replacement word to the OutputString
                        OutputString += Replacement

                        ' Reset the buffer and the index
                        CharBuffer = ""
                        FindWordIndex = 0

                    Else

                        ' Increment the index
                        FindWordIndex += 1

                    End If

                Else

                    ' Purge buffer and current character onto the OutputString
                    OutputString += CharBuffer + k

                    ' Reset the buffer and index
                    CharBuffer = ""
                    FindWordIndex = 0

                End If

            Next

            ' Return
            Return OutputString

        End Function

        Public Shared Function RunCommand(ByVal ExecutionString As String,
                                          Optional ByVal ArgumentString As String = "",
                                          Optional ByVal UseShellExecute As Boolean = False,
                                          Optional ByRef StandardOut As String = Nothing,
                                          Optional ByVal RaiseException As Boolean = False,
                                          Optional ByVal Verb As String = "") As Integer

            ' Local variables
            Dim RunningProcess As Process
            Dim ExitCode As Integer = -1
            Dim ProcessStartInfo As New ProcessStartInfo(ExecutionString, ArgumentString)

            ' Encapsulate
            Try

                ' Check working directory
                If ExecutionString.Contains("\") Then

                    ' Set working directory
                    ProcessStartInfo.WorkingDirectory = ExecutionString.Substring(0, ExecutionString.LastIndexOf("\"))

                Else

                    ' Set local working directory
                    ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory

                End If

                ' Create detached process
                ProcessStartInfo.UseShellExecute = UseShellExecute
                ProcessStartInfo.RedirectStandardOutput = True
                ProcessStartInfo.CreateNoWindow = True
                ProcessStartInfo.Verb = Verb

                ' Start detached process
                RunningProcess = Process.Start(ProcessStartInfo)

                ' Read live output
                While RunningProcess.HasExited = False

                    ' Update standard output
                    If StandardOut IsNot Nothing Then StandardOut += RunningProcess.StandardOutput.ReadLine + Environment.NewLine

                    ' Pump message queue
                    System.Windows.Forms.Application.DoEvents()

                    ' Rest thread
                    Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)

                End While

                ' Wait for detached process to exit
                RunningProcess.WaitForExit()

                ' Append remaining standard output
                If StandardOut IsNot Nothing Then StandardOut += RunningProcess.StandardOutput.ReadToEnd.ToString

                ' Save exit code
                ExitCode = RunningProcess.ExitCode

                ' Close detached process
                RunningProcess.Close()

            Catch ex As Exception

                ' Check if exception should be cascaded
                If RaiseException Then Throw ex

            End Try

            ' Return exit code
            Return ExitCode

        End Function

        Public Shared Function ServiceExists(ByVal ServiceName As String) As Boolean

            ' Local variabes
            Dim InstalledServices As ServiceProcess.ServiceController()

            ' Get list of installed services
            InstalledServices = ServiceProcess.ServiceController.GetServices()

            ' Iterate each service
            For Each sc As ServiceProcess.ServiceController In InstalledServices

                ' Check for a match
                If ServiceName.ToLower.Equals(sc.DisplayName.ToLower) Or
                    ServiceName.ToLower.Equals(sc.ServiceName.ToLower) Then

                    ' Return
                    Return True

                End If

            Next

            ' Match not found
            Return False

        End Function

        Public Shared Function StringArrayContains(ByVal StringArray As String(),
                                                   ByVal SearchString As String,
                                                   Optional ByVal RemoveSwitch As Boolean = False) As Boolean

            ' Iterate string array
            For Each strLine As String In StringArray

                ' Check flag
                If RemoveSwitch AndAlso (strLine.StartsWith("/") OrElse strLine.StartsWith("-") OrElse strLine.StartsWith("--")) Then

                    ' Remove switch character
                    strLine = strLine.TrimStart("/")
                    strLine = strLine.TrimStart("-")
                    strLine = strLine.TrimStart("--")

                End If

                ' Check for a match
                If SearchString.ToLower.Equals(strLine.ToLower) Then Return True

            Next

            ' Return
            Return False

        End Function

        Public Shared Sub SystemReboot(ByVal Delay As Integer)

            ' Initate system reboot with specified delay
            Process.Start("shutdown", "/r /f /t " + Delay.ToString)

        End Sub

    End Class

End Class
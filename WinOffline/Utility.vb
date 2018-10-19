Imports System.IO
Imports System.Management
Imports System.Security.AccessControl

Partial Public Class WinOffline

    Public Class Utility

        Public Shared Sub AddToExistingArray(Of T)(ByRef AnArray As T(), NewItem As T)
            If AnArray Is Nothing Then
                Array.Resize(AnArray, 1)
                AnArray(0) = NewItem
            Else
                Array.Resize(AnArray, AnArray.Length + 1)
                AnArray(AnArray.Length - 1) = NewItem
            End If
        End Sub

        Public Shared Function ArrayListtoCommaString(ByVal InputArray As ArrayList, Optional ByVal Delimeter As String = ",") As String
            Dim ReturnString As String = ""
            For Each strLine As String In InputArray
                ReturnString += strLine + Delimeter
            Next
            Return ReturnString.TrimEnd(Delimeter.ToCharArray)
        End Function

        Public Shared Function ChangeServiceMode(ByVal ServiceName As String, ByVal StartMode As String) As Boolean

            ' StartMode: Automatic, Manual, Disabled

            Dim obj As ManagementObject
            Dim inParams As ManagementBaseObject
            Dim outParams As ManagementBaseObject

            If Not ServiceExists(ServiceName) Then Return False
            obj = New ManagementObject("\\" + Globals.HostName + "\root\cimv2:Win32_Service.Name='" + ServiceName + "'")
            inParams = obj.GetMethodParameters("ChangeStartMode")
            inParams("StartMode") = StartMode
            outParams = obj.InvokeMethod("ChangeStartMode", inParams, Nothing)
            If Convert.ToInt32(outParams("returnValue")) <> 0 Then Return False
            Return True

        End Function

        Public Shared Function CopyDirectory(ByVal SourceFolder As String, ByVal DestinationFolder As String) As Boolean
            If Not Directory.Exists(SourceFolder) Then
                Return False
            End If
            If Directory.Exists(DestinationFolder) Then
                Return False
            Else
                Try
                    Directory.CreateDirectory(DestinationFolder)
                Catch ex As Exception
                    Return False
                End Try
            End If
            For Each SourceFile As String In Directory.GetFiles(SourceFolder)
                Dim DestinationFile As String = Path.Combine(DestinationFolder, System.IO.Path.GetFileName(SourceFile))
                Try
                    File.Copy(SourceFile, DestinationFile)
                Catch ex As Exception
                    Return False
                End Try
            Next
            For Each SubFolder As String In Directory.GetDirectories(SourceFolder)
                Dim DestinationSubFolder As String = Path.Combine(DestinationFolder, Path.GetFileName(SubFolder))
                Try
                    CopyDirectory(SubFolder, DestinationSubFolder)
                Catch ex As Exception
                    ' Something went wrong
                    Return False
                End Try
            Next
            Return True
        End Function

        Public Shared Function DeleteEnvironmentVariable(ByVal VariableName As String) As Boolean
            If Environment.GetEnvironmentVariable(VariableName, EnvironmentVariableTarget.Machine) IsNot Nothing Then
                Environment.SetEnvironmentVariable(VariableName, Nothing, EnvironmentVariableTarget.Machine)
                Return True
            End If
            Return False
        End Function

        Public Shared Function DeleteFile(ByVal CallStack As String, ByVal FileName As String, Optional ByVal RaiseException As Boolean = False) As Boolean
            Dim FileDeleted As Boolean = False
            If System.IO.File.Exists(FileName) Then
                Try
                    System.IO.File.SetAttributes(FileName, IO.FileAttributes.Normal)
                    Logger.WriteDebug(CallStack, "Delete file: " + FileName)
                    System.IO.File.Delete(FileName)
                    FileDeleted = True
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    If RaiseException Then Throw ex
                End Try
            End If
            Return FileDeleted
        End Function

        Public Shared Function DeleteFilePattern(ByVal CallStack As String, ByVal FolderName As String, ByVal FilePattern As String, Optional ByVal RaiseException As Boolean = False) As Boolean

            Dim FileDeleted As Boolean = False
            Dim FileList As String()
            Dim strFile As String

            Try
                If Not System.IO.Directory.Exists(FolderName) Then Return False
                FileList = System.IO.Directory.GetFiles(FolderName)
                If FileList.Length > 0 Then
                    For n As Integer = 0 To FileList.Length - 1
                        strFile = FileList(n).ToString.ToLower
                        strFile = strFile.Substring(strFile.LastIndexOf("\") + 1)
                        If strFile.ToLower.StartsWith(FilePattern.ToLower) Then
                            DeleteFile(CallStack, FileList(n))
                            FileDeleted = True
                        End If
                    Next
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                If RaiseException Then Throw ex
            End Try
            Return FileDeleted

        End Function

        Public Shared Function DeleteFolder(ByVal CallStack As String, ByVal FolderName As String, Optional ByVal RaiseException As Boolean = False) As Boolean
            Dim FolderDeleted As Boolean = False
            If System.IO.Directory.Exists(FolderName) Then
                Try
                    Logger.WriteDebug(CallStack, "Delete folder: " + FolderName)
                    DeleteFolderContents(CallStack, FolderName, Nothing)
                    System.IO.Directory.Delete(FolderName, True)
                    FolderDeleted = True
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Warning: Exception caught deleting folder.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    If RaiseException Then Throw ex
                End Try
            End If
            Return FolderDeleted
        End Function

        Public Shared Sub DeleteFolderContents(ByVal CallStack As String, ByVal TargetFolder As String, ByVal ReservedItems As String(), Optional ByVal VerboseOutput As Boolean = True)

            Dim FileList As String()
            Dim FolderList As String()
            Dim TargetFolderInfo As DirectoryInfo
            Dim TargetFolderACL As DirectorySecurity
            Dim FolderInfo As DirectoryInfo
            Dim SkipItem As Boolean = False

            If Not System.IO.Directory.Exists(TargetFolder) Then
                Return
            End If

            FileList = Directory.GetFiles(TargetFolder)
            FolderList = Directory.GetDirectories(TargetFolder)

            ' Adjust TargetFolder ACL, add permissions for BUILTIN\Administrators group
            Try
                TargetFolderInfo = New DirectoryInfo(TargetFolder)
                TargetFolderACL = New DirectorySecurity(TargetFolder, AccessControlSections.Access)
                TargetFolderACL.AddAccessRule(New FileSystemAccessRule(
                                              New Security.Principal.SecurityIdentifier(
                                                  Security.Principal.WellKnownSidType.BuiltinAdministratorsSid, Nothing),
                                              FileSystemRights.FullControl,
                                              AccessControlType.Allow,
                                              PropagationFlags.InheritOnly,
                                              AccessControlType.Allow))
                TargetFolderInfo.SetAccessControl(TargetFolderACL)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Failed to set target folder access control list.")
                Logger.WriteDebug(ex.Message)
                Manifest.UpdateManifest(CallStack,
                                        Manifest.EXCEPTION_MANIFEST,
                                        {"Error: Failed to set target folder access control list.",
                                        "Reason: Please analyze the debug log for exception message."})
            End Try

            If FolderList.Length > 0 Then
                For n As Integer = 0 To FolderList.Length - 1
                    Try
                        If ReservedItems IsNot Nothing Then
                            For Each str As String In ReservedItems
                                If FolderList(n).ToString.ToLower.EndsWith(str.ToLower) Then
                                    Logger.WriteDebug(CallStack, "Reserved folder: " + FolderList(n).ToString)
                                    SkipItem = True
                                End If
                            Next
                        End If
                        If Not SkipItem Then
                            FolderInfo = New DirectoryInfo(FolderList(n).ToString)
                            If FolderInfo.Attributes And FileAttributes.ReparsePoint Then ' Check for an ntfs junction (instead of a normal folder)
                                Try
                                    Logger.WriteDebug(CallStack, "Remove junction: " + FolderList(n).ToString)
                                    WindowsAPI.RemoveJunction(FolderList(n).ToString) ' Remove the junction point
                                Catch ex As Exception
                                    Logger.WriteDebug(CallStack, "Warning: Exception caught removing NTFS junction: " + FolderList(n).ToString)
                                    Logger.WriteDebug(ex.Message)
                                End Try
                            Else
                                DeleteFolderContents(CallStack, FolderList(n).ToString, ReservedItems, False) ' Recurse
                            End If
                            If VerboseOutput Then
                                Logger.WriteDebug(CallStack, "Delete folder: " + FolderList(n).ToString)
                            End If
                            Directory.Delete(FolderList(n), True) ' After recurse is finished, delete the folder
                        End If
                        SkipItem = False
                    Catch ex2 As Exception
                        Logger.WriteDebug(CallStack, "Warning: Exception caught deleting folder: " + FolderList(n).ToString)
                        Logger.WriteDebug(ex2.Message)
                    End Try
                Next
            End If

            If FileList.Length > 0 Then
                For n As Integer = 0 To FileList.Length - 1
                    Try
                        If ReservedItems IsNot Nothing Then
                            For Each str As String In ReservedItems
                                If FileList(n).ToString.ToLower.EndsWith(str.ToLower) Then
                                    Logger.WriteDebug(CallStack, "Reserved file: " + FileList(n).ToString)
                                    SkipItem = True
                                End If
                            Next
                        End If
                        If Not SkipItem Then
                            If VerboseOutput Then
                                Logger.WriteDebug(CallStack, "Delete file: " + FileList(n).ToString)
                            End If
                            File.SetAttributes(FileList(n), IO.FileAttributes.Normal) ' Unset read-only parameter (in case it's set)
                            File.Delete(FileList(n))
                        End If
                        SkipItem = False
                    Catch ex2 As Exception
                        Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file: " + FileList(n).ToString)
                    End Try
                Next
            End If

        End Sub

        Public Shared Function DeleteRegistrySubKey(ByVal regTree As String, ByVal regKey As String) As Boolean
            Dim regTest As Microsoft.Win32.RegistryKey
            If regTree.ToUpper.Equals("HKLM") Then
                Try
                    regTest = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey, False)
                    If regTest Is Nothing Then
                        Return False
                    Else
                        Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(regKey, True)
                        Logger.WriteDebug(Logger.LastCallStack, "Delete registry: HKLM\" + regKey)
                        regTest.Close()
                    End If
                Catch ex As Exception
                    Return False
                End Try
                Return True
            ElseIf regTree.ToUpper.Equals("HKCR") Then
                Try
                    regTest = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(regKey, False)
                    If regTest Is Nothing Then
                        Return False
                    Else
                        Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(regKey, True)
                        Logger.WriteDebug(Logger.LastCallStack, "Delete registry: HKCR\" + regKey)
                        regTest.Close()
                    End If
                Catch ex As Exception
                    Return False
                End Try
                Return True
            Else
                Throw New Exception("Unknown or unsupported registry tree specified: " + regTree)
            End If
        End Function

        Public Shared Function DeleteRegistrySubKeyTree(ByVal regTree As String, ByVal regKey As String) As Boolean
            Dim regTest As Microsoft.Win32.RegistryKey
            If regTree.ToUpper.Equals("HKLM") Then
                Try
                    regTest = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey, False)
                    If regTest Is Nothing Then
                        Return False
                    Else
                        Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(regKey, True)
                        regTest.Close()
                    End If
                Catch ex As Exception
                    Return False
                End Try
                Return True
            ElseIf regTree.ToUpper.Equals("HKCR") Then
                Try
                    regTest = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(regKey, False)
                    If regTest Is Nothing Then
                        Return False
                    Else
                        Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(regKey, True)
                        regTest.Close()
                    End If
                Catch ex As Exception
                    Return False
                End Try
                Return True
            Else
                Throw New Exception("Unknown or unsupported registry tree specified: " + regTree)
            End If
        End Function

        Public Shared Function DeleteRegistrySubKeysWithValue(ByVal regTree As String, ByVal RootKey As String, ByVal MatchName As String, ByVal MatchValue As String) As Boolean
            Dim TestKey As Microsoft.Win32.RegistryKey
            Dim regSubKey As Microsoft.Win32.RegistryKey
            Dim regSubKeyValue As String = Nothing
            If regTree.ToUpper.Equals("HKLM") Then
                Try
                    TestKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RootKey, False)
                    If TestKey Is Nothing Then Exit Try
                    For Each subKey As String In TestKey.GetSubKeyNames
                        regSubKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RootKey + "\" + subKey, False)
                        regSubKeyValue = regSubKey.GetValue(MatchName)

                        If regSubKeyValue IsNot Nothing AndAlso regSubKeyValue.ToLower.Equals(MatchValue.ToLower) Then
                            regSubKey.Close()
                            Return DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)
                        ElseIf regSubKeyValue Is Nothing AndAlso regSubKey.SubKeyCount > 0 Then
                            If DeleteRegistrySubKeysWithValue(regTree, RootKey + "\" + subKey, MatchName, MatchValue) Then
                                DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)
                            End If
                        Else
                            regSubKey.Close()
                            Continue For
                        End If
                    Next
                    TestKey.Close()
                Catch ex As Exception
                    Return False
                End Try
                Return False
            ElseIf regTree.ToUpper.Equals("HKCR") Then
                Try
                    TestKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(RootKey, False)
                    If TestKey Is Nothing Then Exit Try
                    For Each subKey As String In TestKey.GetSubKeyNames
                        regSubKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(RootKey + "\" + subKey, False)
                        regSubKeyValue = regSubKey.GetValue(MatchName)
                        If regSubKeyValue IsNot Nothing AndAlso regSubKeyValue.ToLower.Equals(MatchValue.ToLower) Then
                            regSubKey.Close()
                            Return DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)
                        ElseIf regSubKeyValue Is Nothing AndAlso regSubKey.SubKeyCount > 0 Then
                            If DeleteRegistrySubKeysWithValue(regTree, RootKey + "\" + subKey, MatchName, MatchValue) Then ' Recurse
                                DeleteRegistrySubKeyTree(regTree, RootKey + "\" + subKey)
                            End If
                        Else
                            regSubKey.Close()
                            Continue For
                        End If
                    Next
                    TestKey.Close()
                Catch ex As Exception
                    Return False
                End Try
                Return False
            Else
                Throw New Exception("Unknown or unsupported registry tree specified: " + regTree)
            End If
        End Function

        Public Shared Function GetParallelProcesses(ByVal ProcessFriendlyName As String) As ArrayList
            Dim ParallelProcessList As New ArrayList
            For Each RunningProcess As Process In Process.GetProcesses
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then
                    ParallelProcessList.Add(RunningProcess)
                End If
            Next
            Return ParallelProcessList
        End Function

        Public Shared Function GetProcessChildren(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String, Optional ByVal FilterList As ArrayList = Nothing) As ArrayList
            Dim ParentPID As Integer
            Dim ChildArrayList As New ArrayList

            ' Each ArrayList within the ChildArrayList will contain:
            ' (0) = ProcessID, Ex: "1234"
            ' (1) = Name, Ex: "cmEngine.exe"
            ' (2) = ExecutablePath, Ex: "C:\Program Files (x86)\CA\DSM\Bin\cmEngine.exe"
            ' (3) = CommandLine, Ex: "C:\Program Files (x86)\CA\DSM\Bin\cmEngine.exe -name SystemEngine"
            ' (4) = WorkingSetSize, in bytes, Ex: 1234567

            Try
                ParentPID = GetProcessID(ProcessShortName, ContainsCommandLine)
                Dim WMIQuery As New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ParentProcessId='" + ParentPID.ToString + "'")
                For Each WMIProcess As ManagementObject In WMIQuery.Get()
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso
                        WMIProcess("Name") IsNot Nothing AndAlso
                        WMIProcess("ExecutablePath") IsNot Nothing AndAlso
                        WMIProcess("CommandLine") IsNot Nothing AndAlso
                        WMIProcess("WorkingSetSize") IsNot Nothing Then
                        If FilterList IsNot Nothing AndAlso FilterList.Contains(WMIProcess("Name").ToString.ToLower) Then
                            Continue For
                        Else
                            ChildArrayList.Add(New ArrayList({WMIProcess("ProcessID"),
                                                             WMIProcess("Name"),
                                                             WMIProcess("ExecutablePath"),
                                                             WMIProcess("CommandLine"),
                                                             WMIProcess("WorkingSetSize")}))
                        End If
                    Else
                        Continue For
                    End If
                Next
            Catch ex As Exception
                Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught querying process children.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
            End Try
            Return ChildArrayList
        End Function

        Public Shared Function GetProcessFileName(ByVal ProcessFriendlyName As String) As String
            For Each RunningProcess As Process In Process.GetProcesses()
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then
                    Return RunningProcess.MainModule.FileName
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function GetProcessID(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String) As Integer
            Dim ProcessSearcher As ManagementObjectSearcher
            Dim ProcessId As String = Nothing
            Dim CommandLine As String = Nothing
            Try
                ProcessSearcher = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")
                For Each WMIProcess As ManagementObject In ProcessSearcher.Get()
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso WMIProcess("CommandLine") IsNot Nothing Then
                        ProcessId = WMIProcess("ProcessId").ToString
                        CommandLine = WMIProcess("CommandLine").ToString
                    Else
                        Continue For ' Missing required attributes -- skip
                    End If
                    If CommandLine IsNot Nothing AndAlso CommandLine.ToLower.Contains(ContainsCommandLine.ToLower) Then
                        Return Integer.Parse(ProcessId)
                    End If
                Next
            Catch ex As Exception
                Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught querying process id.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
            End Try
            Return 0
        End Function

        Public Shared Function GetProcessPath(ByVal ProcessFriendlyName As String) As String
            For Each RunningProcess As Process In Process.GetProcesses()
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then
                    Return RunningProcess.MainModule.FileName.Substring(0, RunningProcess.MainModule.FileName.LastIndexOf("\"))
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function GetProcessPathWithSlash(ByVal ProcessFriendlyName As String) As String
            For Each RunningProcess As Process In Process.GetProcesses()
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then
                    Return RunningProcess.MainModule.FileName.Substring(0, RunningProcess.MainModule.FileName.LastIndexOf("\") + 1)
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function GetProcessWorkingSetMemorySize(ByVal ProcessId As Integer) As Double
            Dim WorkingMemory As Double
            Try
                Dim WMIQuery As New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId='" + ProcessId.ToString + "'")
                For Each WMIProcess As ManagementObject In WMIQuery.Get()
                    If WMIProcess("WorkingSetSize") IsNot Nothing Then
                        WorkingMemory = WMIProcess("WorkingSetSize")
                        Return WorkingMemory
                    Else
                        Continue For ' Missing required attributes -- skip
                    End If
                Next
            Catch ex As Exception
                Logger.WriteDebug(Logger.LastCallStack, "Warning: Exception caught querying process working set memory size.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
            End Try
            Return 0
        End Function

        Public Shared Function InlineAssignHelper(Of T)(ByRef Target As T, ByVal Value As T) As T
            Target = Value
            Return Value
        End Function

        Public Shared Function IsArrayEqual(ByVal ArrayListA As Array, ByVal ArrayListB As Array) As Boolean
            Dim MatchFound As Boolean = False
            If ArrayListA.Length <> ArrayListB.Length Then Return False
            For i As Integer = 0 To ArrayListA.Length - 1
                For j As Integer = 0 To ArrayListB.Length - 1
                    MatchFound = False
                    If ArrayListA(i).ToString.Equals(ArrayListB(j).ToString) Then
                        MatchFound = True
                        Exit For
                    End If
                Next
                If MatchFound = False Then Return False
            Next
            Return True
        End Function

        Public Shared Function IsArrayListEqual(ByVal ArrayListA As ArrayList, ByVal ArrayListB As ArrayList) As Boolean
            Dim MatchFound As Boolean = False
            If ArrayListA.Count <> ArrayListB.Count Then Return False
            For i As Integer = 0 To ArrayListA.Count - 1
                For j As Integer = 0 To ArrayListB.Count - 1
                    MatchFound = False
                    If ArrayListA.Item(i).ToString.Equals(ArrayListB.Item(j).ToString) Then
                        MatchFound = True
                        Exit For
                    End If
                Next
                If MatchFound = False Then Return False
            Next
            Return True
        End Function

        Public Shared Function IsFileEqual(file1 As String, file2 As String) As Boolean
            Dim file1byte As Integer
            Dim file2byte As Integer
            Dim fs1 As FileStream
            Dim fs2 As FileStream
            If file1 = file2 Then Return True
            If Not System.IO.File.Exists(file1) Then Return False
            If Not System.IO.File.Exists(file2) Then Return False
            Try
                fs1 = New FileStream(file1, FileMode.Open, FileAccess.Read)
                fs2 = New FileStream(file2, FileMode.Open, FileAccess.Read)
                If fs1.Length <> fs2.Length Then
                    fs1.Close()
                    fs2.Close()
                    Return False
                End If
                Do
                    file1byte = fs1.ReadByte()
                    file2byte = fs2.ReadByte()
                Loop While (file1byte = file2byte) AndAlso (file1byte <> -1)
                fs1.Close()
                fs2.Close()
            Catch ex As Exception
                Logger.WriteDebug(Logger.LastCallStack, "Error: Failed to compare files.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Return False
            End Try
            Return ((file1byte - file2byte) = 0)
        End Function

        Public Shared Function IsFileOpen(ByVal FileName As String) As Boolean
            Dim FileInfo As New FileInfo(FileName)
            Dim FileStream As FileStream = Nothing
            Try
                FileStream = FileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                FileStream.Close()
                Return False
            Catch ex As Exception
                Return True
            End Try
        End Function

        Public Shared Function IsFolderEmpty(ByVal FolderName As String) As Boolean
            If System.IO.Directory.Exists(FolderName) Then
                If System.IO.Directory.GetDirectories(FolderName).Length > 0 Then Return False
                If System.IO.Directory.GetFiles(FolderName).Length > 0 Then Return False
            End If
            Return True
        End Function

        Public Shared Function IsHexChar(ByVal c As Char) As Boolean
            Dim result As Integer
            If Integer.TryParse(c.ToString, result) AndAlso result >= 0 AndAlso result <= 9 Then Return True
            If c.ToString.ToLower.Equals("a") Then Return True
            If c.ToString.ToLower.Equals("b") Then Return True
            If c.ToString.ToLower.Equals("c") Then Return True
            If c.ToString.ToLower.Equals("d") Then Return True
            If c.ToString.ToLower.Equals("e") Then Return True
            If c.ToString.ToLower.Equals("f") Then Return True
            Return False
        End Function

        Public Shared Function IsHexString(ByVal s As String) As Boolean
            For Each c As Char In s
                If Not IsHexChar(c) Then Return False
            Next
            Return True
        End Function

        Public Shared Function IsITCMInstalled() As Boolean
            If IsProcessRunning("caf") Then Return True
            Dim ProductInfoKey As Microsoft.Win32.RegistryKey = Nothing
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)
            If ProductInfoKey Is Nothing Then
                ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)
                If ProductInfoKey Is Nothing Then
                    Return False
                End If
            End If
            If ProductInfoKey.GetValue("InstallDirProduct") IsNot Nothing Then
                Globals.DSMFolder = ProductInfoKey.GetValue("InstallDirProduct").ToString()
            Else
                Return False
            End If
            ProductInfoKey.Close()
            If System.IO.Directory.Exists(Globals.DSMFolder) AndAlso
                System.IO.Directory.Exists(Globals.DSMFolder + "bin") AndAlso
                System.IO.File.Exists(Globals.DSMFolder + "bin\caf.exe") Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsProcessRunning(ByVal ProcessId As Integer) As Boolean
            For Each RunningProcess As Process In Process.GetProcesses()
                If RunningProcess.Id = ProcessId Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Shared Function IsProcessRunning(ByVal ProcessFriendlyName As String, Optional ByVal MoreInfo As Boolean = False) As Boolean
            For Each RunningProcess As Process In Process.GetProcesses()
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then
                    If MoreInfo Then
                        Dim ProcessWMI As ManagementObject = Nothing
                        Dim CurrentID As Integer = Nothing
                        Dim ParentID As Integer = Nothing
                        Dim ParentName As String = Nothing
                        Dim WMIQuery As ManagementObjectSearcher
                        Dim CommandLine As String = Nothing
                        Try
                            WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId='" + RunningProcess.Id.ToString + "'")
                            For Each WMIProcess As ManagementObject In WMIQuery.Get()
                                CommandLine = WMIProcess("CommandLine").ToString
                            Next
                        Catch ex As Exception
                            CommandLine = "unavailable"
                        End Try
                        Logger.WriteDebug(Logger.LastCallStack, "IsProcessRunning() found: " + RunningProcess.Id.ToString + "/" + RunningProcess.ProcessName + " [" + CommandLine + "]")
                        Try
                            CurrentID = RunningProcess.Id
                            While True
                                ProcessWMI = New ManagementObject("Win32_Process.Handle='" & CurrentID & "'")
                                ParentID = ProcessWMI("ParentProcessID")
                                If Not Integer.TryParse(ParentID, Nothing) OrElse ParentID <= 0 Then Exit While
                                ParentName = Process.GetProcessById(ParentID).ProcessName.ToString
                                Logger.WriteDebug(Logger.LastCallStack, "IsProcessRunning() parent: " + ParentID.ToString + "/" + ParentName)
                                CurrentID = ParentID
                            End While
                        Catch ex As Exception
                            ' Do nothing
                        End Try
                    End If
                    Return True
                End If
            Next
            Return False
        End Function

        Public Shared Function IsProcessRunning(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String) As Boolean
            Dim WMIQuery As ManagementObjectSearcher
            Dim CommandLine As String = Nothing
            WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")
            For Each WMIProcess As ManagementObject In WMIQuery.Get()
                If WMIProcess("CommandLine") IsNot Nothing Then
                    CommandLine = WMIProcess("CommandLine").ToString
                Else
                    Continue For
                End If
                If CommandLine IsNot Nothing AndAlso CommandLine.ToLower.Contains(ContainsCommandLine.ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Shared Function IsProcessRunningEx(ByVal ProcessName As String) As Boolean
            Dim WMIQuery As ManagementObjectSearcher
            Dim ExecutablePath As String = Nothing
            WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + FileVector.GetShortName(ProcessName) + "'")
            For Each WMIProcess As ManagementObject In WMIQuery.Get()
                If WMIProcess("ExecutablePath") IsNot Nothing Then
                    ExecutablePath = WMIProcess("ExecutablePath").ToString
                Else
                    Continue For ' Missing required attribute for comparison -- skip
                End If
                If ExecutablePath IsNot Nothing AndAlso ExecutablePath.ToLower.Equals(ProcessName.ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Shared Function IsProcessRunningCount(ByVal ProcessFriendlyName As String) As Integer
            Dim processCount As Integer = 0
            For Each RunningProcess As Process In Process.GetProcesses
                If RunningProcess.ProcessName.ToLower.Equals(ProcessFriendlyName.ToLower) Then
                    processCount += 1
                End If
            Next
            Return processCount
        End Function

        Public Shared Function IsServiceEnabled(ByVal ServiceName As String) As Boolean
            Dim obj As ManagementObject
            obj = New ManagementObject("\\" + Globals.HostName + "\root\cimv2:Win32_Service.Name='" + ServiceName + "'")
            If obj("StartMode").ToString.ToLower.Equals("disabled") Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Function KillProcess(ByVal FriendlyName As String, Optional ByVal MoreInfo As Boolean = False) As Boolean
            Dim MatchFound As Boolean = False
            Dim CommandLine As String = Nothing
            Try
                For Each RunningProcess As Process In Process.GetProcesses()
                    If RunningProcess.ProcessName.ToLower.Equals(FriendlyName.ToLower) Then
                        MatchFound = True
                        If MoreInfo Then ' Before killing process
                            Try
                                Dim WMIQuery As ManagementObjectSearcher
                                WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId='" + RunningProcess.Id.ToString + "'")
                                For Each WMIProcess As ManagementObject In WMIQuery.Get()
                                    CommandLine = WMIProcess("CommandLine").ToString
                                Next
                            Catch ex As Exception
                                CommandLine = "unavailable"
                            End Try
                        End If
                        RunningProcess.Kill()
                        If MoreInfo Then ' After killing process
                            Logger.WriteDebug(Logger.LastCallStack, "Killed: " + RunningProcess.Id.ToString + "/" + RunningProcess.ProcessName + " [" + CommandLine + "]")
                        End If
                    End If
                Next
            Catch ex As Exception
            End Try
            Return MatchFound
        End Function

        Public Shared Function KillProcess(ByVal ProcessID As Integer, Optional ByVal MoreInfo As Boolean = False) As Boolean
            Dim CommandLine As String = Nothing
            Try
                For Each RunningProcess As Process In Process.GetProcesses()
                    If RunningProcess.Id = ProcessID Then
                        If MoreInfo Then ' Before killing process
                            Try
                                Dim WMIQuery As ManagementObjectSearcher
                                WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId='" + RunningProcess.Id.ToString + "'")
                                For Each WMIProcess As ManagementObject In WMIQuery.Get()
                                    CommandLine = WMIProcess("CommandLine").ToString
                                Next
                            Catch ex As Exception
                                CommandLine = "unavailable"
                            End Try
                        End If
                        RunningProcess.Kill()
                        If MoreInfo Then ' After killing process
                            Logger.WriteDebug(Logger.LastCallStack, "Killed: " + RunningProcess.Id.ToString + "/" + RunningProcess.ProcessName + " [" + CommandLine + "]")
                        End If
                        Return True
                    End If
                Next
            Catch ex As Exception
            End Try
            Return False
        End Function

        Public Shared Function KillProcessByCommandLine(ByVal ProcessShortName As String, ByVal ContainsCommandLine As String, Optional ByVal MoreInfo As Boolean = False) As Boolean
            Dim WMIQuery As ManagementObjectSearcher
            Dim ProcessId As String = Nothing
            Dim CommandLine As String = Nothing
            Dim ProcessFound As Boolean = False
            Try
                WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")
                For Each WMIProcess As ManagementObject In WMIQuery.Get()
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso WMIProcess("CommandLine") IsNot Nothing Then
                        ProcessId = WMIProcess("ProcessID").ToString
                        CommandLine = WMIProcess("CommandLine").ToString
                    Else
                        Continue For ' Missing required attributes -- skip
                    End If
                    If CommandLine IsNot Nothing AndAlso CommandLine.ToLower.Contains(ContainsCommandLine.ToLower) Then
                        ProcessFound = True
                        KillProcess(Integer.Parse(ProcessId), MoreInfo)
                    End If
                Next
            Catch ex As Exception
            End Try
            Return ProcessFound
        End Function

        Public Shared Function KillProcessByPath(ByVal ProcessShortName As String, ByVal ProcessPathContains As String) As Boolean
            Dim WMIQuery As ManagementObjectSearcher
            Dim ProcessId As String = Nothing
            Dim ExecutablePath As String = Nothing
            Dim ProcessFound As Boolean = False
            Try
                WMIQuery = New ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='" + ProcessShortName + "'")
                For Each WMIProcess As ManagementObject In WMIQuery.Get()
                    If WMIProcess("ProcessID") IsNot Nothing AndAlso WMIProcess("ExecutablePath") IsNot Nothing Then
                        ProcessId = WMIProcess("ProcessID").ToString
                        ExecutablePath = WMIProcess("ExecutablePath").ToString
                    Else
                        Continue For ' Missing required attributes -- skip
                    End If
                    If ExecutablePath IsNot Nothing AndAlso ExecutablePath.ToLower.Contains(ProcessPathContains.ToLower) Then
                        ProcessFound = True
                        KillProcess(Integer.Parse(ProcessId))
                    End If
                Next
            Catch ex As Exception
            End Try
            Return ProcessFound
        End Function

        Public Shared Function RegistryKeyExists(ByVal regKey As String) As Boolean
            Dim regTest As Microsoft.Win32.RegistryKey
            Try
                regTest = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey, False)
                If regTest Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Sub RemoveFirstFromExistingArray(Of T)(ByRef AnArray As T(), RemoveItem As T)
            Dim TempList As New ArrayList
            For Each Element As T In AnArray
                TempList.Add(Element)
            Next
            TempList.Remove(RemoveItem)
            For x As Integer = 0 To TempList.Count - 1
                AnArray(x) = TempList.Item(x)
            Next
            Array.Resize(AnArray, AnArray.Length - 1)
        End Sub

        Public Shared Function ReplaceWord(ByVal InputString As String, ByVal FindWord As String, ByVal Replacement As String) As String
            Dim OutputString As String = ""
            Dim CharBuffer As String = ""
            Dim FindWordIndex As Integer = 0
            If InputString Is Nothing OrElse InputString.Equals("") Then Return ""
            If FindWord Is Nothing OrElse FindWord.Equals("") Then Return ""
            If Replacement Is Nothing Then Return ""
            For Each k As Char In InputString
                If k.ToString.ToLower.Equals(FindWord.Chars(FindWordIndex).ToString.ToLower) Then
                    CharBuffer += k
                    If CharBuffer.Length = FindWord.Length Then
                        OutputString += Replacement
                        CharBuffer = ""
                        FindWordIndex = 0
                    Else
                        FindWordIndex += 1
                    End If
                Else
                    OutputString += CharBuffer + k
                    CharBuffer = ""
                    FindWordIndex = 0
                End If
            Next
            Return OutputString
        End Function

        Public Shared Function RunCommand(ByVal ExecutionString As String,
                                          Optional ByVal ArgumentString As String = "",
                                          Optional ByVal UseShellExecute As Boolean = False,
                                          Optional ByRef StandardOut As String = Nothing,
                                          Optional ByVal RaiseException As Boolean = False,
                                          Optional ByVal Verb As String = "") As Integer
            Dim RunningProcess As Process
            Dim ExitCode As Integer = -1
            Dim ProcessStartInfo As New ProcessStartInfo(ExecutionString, ArgumentString)
            Try
                If ExecutionString.Contains("\") Then
                    ProcessStartInfo.WorkingDirectory = ExecutionString.Substring(0, ExecutionString.LastIndexOf("\"))
                Else
                    ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
                End If
                ProcessStartInfo.UseShellExecute = UseShellExecute
                ProcessStartInfo.RedirectStandardOutput = True
                ProcessStartInfo.CreateNoWindow = True
                ProcessStartInfo.Verb = Verb
                RunningProcess = Process.Start(ProcessStartInfo)
                While RunningProcess.HasExited = False
                    If StandardOut IsNot Nothing Then StandardOut += RunningProcess.StandardOutput.ReadLine + Environment.NewLine
                    System.Windows.Forms.Application.DoEvents()
                End While
                RunningProcess.WaitForExit()
                If StandardOut IsNot Nothing Then StandardOut += RunningProcess.StandardOutput.ReadToEnd.ToString
                ExitCode = RunningProcess.ExitCode
                RunningProcess.Close()
            Catch ex As Exception
                If RaiseException Then Throw ex
            End Try
            Return ExitCode
        End Function

        Public Shared Function ServiceExists(ByVal ServiceName As String) As Boolean
            Dim InstalledServices As ServiceProcess.ServiceController()
            InstalledServices = ServiceProcess.ServiceController.GetServices()
            For Each sc As ServiceProcess.ServiceController In InstalledServices
                If ServiceName.ToLower.Equals(sc.DisplayName.ToLower) Or ServiceName.ToLower.Equals(sc.ServiceName.ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Shared Function StringArrayContains(ByVal StringArray As String(),
                                                   ByVal SearchString As String,
                                                   Optional ByVal RemoveSwitch As Boolean = False) As Boolean
            For Each strLine As String In StringArray
                If RemoveSwitch AndAlso (strLine.StartsWith("/") OrElse strLine.StartsWith("-") OrElse strLine.StartsWith("--")) Then
                    strLine = strLine.TrimStart("/")
                    strLine = strLine.TrimStart("-")
                    strLine = strLine.TrimStart("--")
                End If
                If SearchString.ToLower.Equals(strLine.ToLower) Then Return True
            Next
            Return False
        End Function

        Public Shared Sub SystemReboot(ByVal Delay As Integer)
            Process.Start("shutdown", "/r /f /t " + Delay.ToString)
        End Sub

    End Class

End Class
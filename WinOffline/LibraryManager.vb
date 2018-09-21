Imports System.Data.SqlClient
Imports System.Threading

Partial Public Class WinOffline

    Public Class LibraryManager

        Public Shared LibraryEvents As New List(Of String)

        Public Shared Sub RepairLibrary(ByVal CallStack As String)

            Dim ITCMManager As Boolean = False
            Dim ITCMServer As Boolean = False
            Dim ConnectionString As String = Nothing
            Dim DbConnection As SqlConnection = Nothing
            Dim DomainUUID As String = Nothing
            Dim DomainId As String = Nothing
            Dim DomainType As String = Nothing
            Dim ManagerName As String = Nothing
            Dim AgentUUID As String = Nothing
            Dim ServerUUID As String = Nothing
            Dim LibraryFile As String
            Dim LibraryReader As System.IO.StreamReader
            Dim LibraryData As New List(Of String)
            Dim rswFileReader As System.IO.StreamReader
            Dim rswFileLine As String = Nothing
            Dim DbRegSwUUID As New List(Of String)
            Dim DbRegSwItemName As New List(Of String)
            Dim DbSwProcs As New List(Of String)
            Dim DctRegSw As New List(Of String)
            Dim DctRegSwFolder As New List(Of String)
            Dim CurrentLine As String
            Dim PreviousLine As String
            Dim DctMissingArcCount As Integer = 0
            Dim DctMissingVolCount As Integer = 0
            Dim DctMissingRegInfoCount As Integer = 0
            Dim DctDuplicateCount As Integer = 0
            Dim DctNotInDbCount As Integer = 0
            Dim DbNotInDct As New List(Of String)
            Dim ItemUUID As String = Nothing
            Dim ItemFolder As String = Nothing
            Dim ItemName As String = Nothing
            Dim ItemVersion As String = Nothing
            Dim ItemType As String = Nothing
            Dim ItemSubFolders As String() = Nothing
            Dim FolderExists As Boolean = False
            Dim VolumeExists As Boolean = False
            Dim RegInfoExists As Boolean = False
            Dim ArchivedPackage As Boolean = False
            Dim FolderList As String()
            Dim MatchFound As Boolean = False
            Dim ArcsForDeletion As New List(Of String)
            Dim RcpsForDeletion As New List(Of String)
            Dim LibraryFileBackup1 As String
            Dim LibraryFileBackup2 As String
            Dim LibraryFileWriter As System.IO.StreamWriter
            Dim SqlData As SqlDataReader
            Dim SqlCmd As SqlCommand
            Dim ReturnCode As Integer = 0

            CallStack += "LibraryMgr|"

            ' Perform verifications
            If Not Utility.IsITCMInstalled Then
                Logger.WriteDebug(CallStack, "Error: ITCM is not installed.")
                Return
            End If

            If Globals.ITCMFunction.ToLower.Contains("manager") Then
                ITCMManager = True
            ElseIf Globals.ITCMFunction.ToLower.Contains("server") Then
                ITCMServer = True
            Else
                Logger.WriteDebug(CallStack, "Error: Library cleanup only applicable to ITCM managers or servers.")
                Return
            End If

            If Globals.SDLibraryFolder IsNot Nothing AndAlso System.IO.Directory.Exists(Globals.SDLibraryFolder) Then
                If Not Globals.SDLibraryFolder.EndsWith("\") Then Globals.SDLibraryFolder += "\"
            Else
                Logger.WriteDebug(CallStack, "Error: Library folder does not exist: " + Globals.SDLibraryFolder)
                Return
            End If

            LibraryFile = Globals.SDLibraryFolder + "library.dct"
            LibraryFileBackup1 = Globals.SDLibraryFolder + "library.dct.backup1"
            LibraryFileBackup2 = Globals.SDLibraryFolder + "library.dct.backup2"

            If Not System.IO.File.Exists(LibraryFile) Then
                Logger.WriteDebug(CallStack, "Error: Library file does not exist: " + LibraryFile)
                Return
            End If

            ' Perform database verifications
            If Globals.DatabaseServer Is Nothing Then
                Logger.WriteDebug(CallStack, "Error: Database server name was not detected or provided.")
                Logger.WriteDebug(CallStack, "Info: Provide ""-dbserver serverName"" switch.")
                Return
            End If
            If Globals.DatabaseInstance Is Nothing Then Globals.DatabaseInstance = "MSSQLSERVER"
            If Globals.DatabasePort Is Nothing Then Globals.DatabasePort = "1433"
            If Globals.DatabaseName Is Nothing Then Globals.DatabaseName = "mdb"
            If Globals.DbUser Is Nothing Then
                Globals.DbUser = Globals.ProcessIdentity.Name
                Logger.WriteDebug(CallStack, "Logon as: " + Globals.DbUser)
            ElseIf Globals.DbPassword Is Nothing Then
                Console.WriteLine()
                Console.WriteLine(CallStack.Substring(CallStack.IndexOf("LibraryMgr")) + "Logon as: " + Globals.DbUser)
                Console.Write(CallStack.Substring(CallStack.IndexOf("LibraryMgr")) + "Enter password: ")
                KeyboardHook.SetHook() ' Hook the keyboard for low-level input (we are not a console app)
                While (KeyboardHook.KeyboardHooked) ' Wait for keyboard release
                    Windows.Forms.Application.DoEvents()
                    Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                End While
                Globals.DbPassword = KeyboardHook.CapturedString.ToString ' Retrieve password
            End If

            ' Build connection string
            If Globals.DbUser.Contains("\") Then
                ConnectionString = "Server=" + Globals.DatabaseServer
                ConnectionString += "\" + Globals.DatabaseInstance
                ConnectionString += "," + Globals.DatabasePort
                ConnectionString += ";Database=" + Globals.DatabaseName
                ConnectionString += ";Trusted_Connection=True"
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            Else
                ConnectionString = "Server=" + Globals.DatabaseServer
                ConnectionString += "\" + Globals.DatabaseInstance
                ConnectionString += "," + Globals.DatabasePort
                ConnectionString += ";Database=" + Globals.DatabaseName
                ConnectionString += ";User Id=" + Globals.DbUser
                ConnectionString += ";Password=" + Globals.DbPassword
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            End If

            ' Verify connection to SQL
            Try
                DbConnection = New SqlConnection(ConnectionString)
                DbConnection.Open()
                DomainUUID = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select set_val_uuid from ca_settings with (nolock) where set_id=1")
                Logger.WriteDebug(CallStack, "Domain UUID: " + DomainUUID)
                DomainId = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select domain_id from ca_n_tier with (nolock) where domain_uuid=" + DomainUUID)
                Logger.WriteDebug(CallStack, "Domain ID: " + DomainId)
                DomainType = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select domain_type from ca_n_tier with (nolock) where domain_uuid=" + DomainUUID)
                Logger.WriteDebug(CallStack, "Domain type: " + DomainType)
                ManagerName = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select label from ca_manager with (nolock) where domain_uuid=" + DomainUUID)
                Logger.WriteDebug(CallStack, "Manager name: " + ManagerName)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Database exception: " + ex.Message)
                DbConnection.Close()
                Logger.WriteDebug(CallStack, "Database connection closed.")
                Return
            End Try

            ' Verify ITCM server is registered with ITCM manager (ITCM server only)
            If ITCMServer Then
                Try
                    AgentUUID = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select dis_hw_uuid from ca_discovered_hardware with (nolock) where host_uuid='" + Globals.HostUUID.ToUpper + "'")
                    If AgentUUID Is Nothing OrElse AgentUUID.Length <> 34 Then ' Check UUID (Ex: 0xD5BEF3369CAC5540994D1EF0ECC38FEF)
                        Logger.WriteDebug(CallStack, "Error: Scalability server not found in manager database.")
                        DbConnection.Close()
                        Return
                    Else
                        Logger.WriteDebug(CallStack, "Agent UUID: " + AgentUUID)
                    End If
                    ServerUUID = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select server_uuid from ca_server with (nolock) where dis_hw_uuid=" + AgentUUID + "")
                    If ServerUUID Is Nothing OrElse ServerUUID.Length <> 34 Then ' Check UUID
                        Logger.WriteDebug(CallStack, "Error: Scalability server is unregistered with manager database.")
                        DbConnection.Close()
                        Return
                    Else
                        Logger.WriteDebug(CallStack, "Server UUID: " + ServerUUID)
                    End If
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Database exception: " + ex.Message)
                    DbConnection.Close()
                    Logger.WriteDebug(CallStack, "Database connection closed.")
                    Return
                End Try
            End If

            ' Read database packages
            Try
                DbRegSwUUID = DatabaseAPI.SqlSelectScalarList(CallStack, DbConnection, "select cast(objectid as uniqueidentifier) from usd_rsw where itemtype<>5")
                DbRegSwItemName = DatabaseAPI.SqlSelectScalarList(CallStack, DbConnection, "select itemname from usd_rsw where itemtype<>5")
                Logger.WriteDebug(CallStack, "Database Packages: " + DbRegSwUUID.Count.ToString)
                DbSwProcs = DatabaseAPI.SqlSelectScalarList(CallStack, DbConnection, "select cast(objectid as uniqueidentifier) from usd_actproc")
                Logger.WriteDebug(CallStack, "Database Procedures: " + DbSwProcs.Count.ToString)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Database exception: " + ex.Message)
                DbConnection.Close()
                Logger.WriteDebug(CallStack, "Database connection closed.")
                Return
            End Try

            ' Read library file
            Logger.WriteDebug(CallStack, "Read library file: " + LibraryFile)
            LibraryReader = New System.IO.StreamReader(LibraryFile)
            Do While LibraryReader.Peek() >= 0
                LibraryData.Add(LibraryReader.ReadLine())
            Loop
            LibraryReader.Close()

            ' Iterate the library file (Yes, not count -1)
            For i = 0 To LibraryData.Count

                ' Are we at the bottom?
                If i >= LibraryData.Count - 1 Then
                    While String.IsNullOrWhiteSpace(LibraryData.Item(LibraryData.Count - 1))
                        LibraryData.RemoveAt(LibraryData.Count - 1) ' Remove trailing whitespace
                    End While
                    LibraryData.Add("") ' Add whitespace
                    Exit For ' Stop processing
                ElseIf i = 0 And String.IsNullOrWhiteSpace(LibraryData.Item(0)) Then
                    LibraryData.RemoveAt(0) ' Remove leading white space
                    i -= 1 ' Decrement i
                    Continue For ' Continue
                End If

                ' Read current line
                CurrentLine = LibraryData.Item(i)

                ' Read previous line
                If i > 0 Then
                    PreviousLine = LibraryData.Item(i - 1)
                Else
                    PreviousLine = "-1"
                End If

                ' Process current line
                If CurrentLine.ToLower.StartsWith("[") Then

                    ' Recognizable tag?
                    If CurrentLine.ToLower.Equals("[locale]") AndAlso
                        i + 1 < LibraryData.Count - 1 AndAlso
                        LibraryData.Item(i + 1).ToLower.Contains("codepage") Then
                        i += 1
                        Continue For

                    ElseIf i + 4 <= LibraryData.Count - 1 AndAlso
                            (LibraryData.Item(i + 1).ToLower.Contains("path=") OrElse LibraryData.Item(i + 1).ToLower.Contains("path =")) AndAlso
                            (LibraryData.Item(i + 2).ToLower.Contains("itemname=") OrElse LibraryData.Item(i + 2).ToLower.Contains("itemname =")) AndAlso
                            (LibraryData.Item(i + 3).ToLower.Contains("itemversion=") OrElse LibraryData.Item(i + 3).ToLower.Contains("itemversion =")) AndAlso
                            (LibraryData.Item(i + 4).ToLower.Contains("itemtype=") OrElse LibraryData.Item(i + 4).ToLower.Contains("itemtype =")) Then

                        ' Parse properties
                        ItemFolder = Globals.SDLibraryFolder + LibraryData.Item(i + 1).Substring(LibraryData.Item(i + 1).IndexOf("=") + 1).Trim
                        ItemName = LibraryData.Item(i + 2).Substring(LibraryData.Item(i + 2).IndexOf("=") + 1).Trim
                        ItemVersion = LibraryData.Item(i + 3).Substring(LibraryData.Item(i + 3).IndexOf("=") + 1).Trim
                        ItemType = LibraryData.Item(i + 4).Substring(LibraryData.Item(i + 4).IndexOf("=") + 1).Trim

                        FolderExists = False
                        VolumeExists = False
                        RegInfoExists = False
                        ArchivedPackage = False

                        ' Archived package?
                        If ItemType.Equals("6") Then ArchivedPackage = True

                        ' Valid package?
                        If System.IO.Directory.Exists(ItemFolder) Then
                            FolderExists = True
                            ItemSubFolders = System.IO.Directory.GetDirectories(ItemFolder)
                            For x As Integer = 0 To ItemSubFolders.Length - 1 ' Iterate folder list (looking for .vol folder)
                                If ItemSubFolders(x).ToLower.EndsWith(".vol") Then
                                    VolumeExists = True
                                ElseIf ItemSubFolders(x).ToLower.EndsWith("reginfo") Then
                                    If System.IO.File.Exists(ItemSubFolders(x) + "\rsw.dat") Then
                                        rswFileReader = New System.IO.StreamReader(ItemSubFolders(x) + "\rsw.dat")
                                        Do While rswFileReader.Peek() >= 0 ' Iterate rsw.dat file contents
                                            rswFileLine = rswFileReader.ReadLine()
                                            If rswFileLine.ToLower.Contains("uuid =") Then
                                                ItemUUID = rswFileLine.Substring(rswFileLine.ToLower.IndexOf("uuid = ") + 7).ToLower
                                                RegInfoExists = True
                                            End If
                                            If RegInfoExists Then Exit Do ' Stop condition
                                        Loop
                                        rswFileReader.Close()
                                    End If
                                End If
                                If (VolumeExists AndAlso RegInfoExists) OrElse (ArchivedPackage AndAlso RegInfoExists) Then Exit For ' Exit condition
                            Next
                        Else
                            Logger.WriteDebug(CallStack, "Missing arc folder: " + ItemName + " " + ItemVersion)
                            DctMissingArcCount += 1
                        End If

                        If FolderExists AndAlso Not VolumeExists AndAlso Not ArchivedPackage Then
                            Logger.WriteDebug(CallStack, "Missing volume folder: " + ItemName + " " + ItemVersion)
                            DctMissingVolCount += 1
                        ElseIf FolderExists AndAlso Not RegInfoExists Then
                            Logger.WriteDebug(CallStack, "Missing reginfo\rsw.dat file: " + ItemName + " " + ItemVersion)
                            DctMissingRegInfoCount += 1
                        End If

                        If Not (FolderExists AndAlso (VolumeExists OrElse ArchivedPackage) AndAlso RegInfoExists) Then
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            If i > 0 Then i -= 1 ' Decrement i
                            Continue For ' Skip to end

                        ElseIf Not DbRegSwUUID.Contains(ItemUUID.ToLower) Then
                            Logger.WriteDebug(CallStack, "Missing from database: " + ItemName + " " + ItemVersion)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            If i > 0 Then i -= 1 ' Decrement i
                            DctNotInDbCount += 1
                            Continue For ' Skip to end

                        ElseIf DctRegSw.Contains(ItemUUID.ToLower) Then
                            Logger.WriteDebug(CallStack, "Duplicate in library: " + ItemName + " " + ItemVersion + " (UUID=" + ItemUUID + ")")
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            LibraryData.RemoveAt(i)
                            If i > 0 Then i -= 1 ' Decrement i
                            DctDuplicateCount += 1
                            Continue For ' SKip to end

                        Else
                            DctRegSw.Add(ItemUUID.ToLower)
                            ' Save arc folder name (we'll need it later to ensure arc folder is not removed)
                            DctRegSwFolder.Add(LibraryData.Item(i + 1).Substring(LibraryData.Item(i + 1).IndexOf("=") + 1).Trim.ToLower)
                            ' Whitespace check
                            If Not PreviousLine.Equals("-1") AndAlso Not String.IsNullOrWhiteSpace(PreviousLine) Then
                                LibraryData.Insert(i, "")
                                i += 5
                            Else
                                i += 4
                            End If
                            Continue For
                        End If

                        ItemUUID = Nothing
                        ItemFolder = Nothing
                        ItemName = Nothing
                        ItemVersion = Nothing
                        ItemType = Nothing
                        ItemSubFolders = Nothing
                        FolderExists = False
                        VolumeExists = False
                        ArchivedPackage = False
                        RegInfoExists = False

                    Else
                        Logger.WriteDebug(CallStack, "Invalid entry: " + CurrentLine)
                        LibraryData.RemoveAt(i)
                        If i > 0 Then i -= 1
                        Continue For
                    End If

                ElseIf String.IsNullOrWhiteSpace(CurrentLine) AndAlso String.IsNullOrWhiteSpace(PreviousLine) Then
                    LibraryData.RemoveAt(i)
                    i -= 1

                ElseIf String.IsNullOrWhiteSpace(CurrentLine) AndAlso Not String.IsNullOrWhiteSpace(PreviousLine) Then
                    Continue For ' Do nothing -- single CR/LF is fine.

                Else
                    Logger.WriteDebug(CallStack, "Invalid entry: " + CurrentLine)
                    LibraryData.RemoveAt(i)
                    If i > 0 Then i -= 1
                End If

            Next

            ' Process database for orphans (ITCM manager only)
            If ITCMManager Then
                For i As Integer = 0 To DbRegSwUUID.Count - 1
                    If Not DctRegSw.Contains(DbRegSwUUID.Item(i).ToLower) Then ' Check if database software was captured in the library file
                        Logger.WriteDebug(CallStack, "Database orphan: " + DbRegSwItemName.Item(i) + " (UUID=" + DbRegSwUUID.Item(i) + ")")
                        DbNotInDct.Add(DbRegSwUUID.Item(i))
                    End If
                Next
            End If

            ' Sweep for orphaned folders
            FolderList = System.IO.Directory.GetDirectories(Globals.SDLibraryFolder)
            For i As Integer = 0 To FolderList.Length - 1
                If FolderList(i).ToLower.EndsWith(".arc") Then
                    For j As Integer = 0 To DctRegSwFolder.Count - 1
                        ' Check for a match (folder matches software in library and is not on the removal list)
                        If FolderList(i).Substring(FolderList(i).LastIndexOf("\") + 1).ToLower.Equals(DctRegSwFolder.Item(j).ToLower) AndAlso
                            Not DbNotInDct.Contains(FolderList(i).Substring(FolderList(i).LastIndexOf("\") + 1).ToLower.Replace(".arc", "")) Then
                            MatchFound = True
                            Exit For
                        End If
                    Next
                    If Not MatchFound Then
                        ArcsForDeletion.Add(FolderList(i)) ' Add orphan to cleanup list
                        Logger.WriteDebug(CallStack, "Library orphan: " + FolderList(i).Replace(Globals.SDLibraryFolder, ""))
                    End If
                    MatchFound = False

                ElseIf ITCMManager AndAlso DbSwProcs.Count > 0 AndAlso FolderList(i).ToLower.EndsWith(".rcp") Then
                    For j As Integer = 0 To DbSwProcs.Count - 1
                        If FolderList(i).Substring(FolderList(i).LastIndexOf("\") + 1).ToLower.Replace(".rcp", "").Equals(DbSwProcs.Item(j).ToLower) Then
                            MatchFound = True
                            Exit For
                        End If
                    Next
                    If Not MatchFound Then
                        RcpsForDeletion.Add(FolderList(i)) ' Add orphan to cleanup list
                        Logger.WriteDebug(CallStack, "Library orphan: " + FolderList(i).Replace(Globals.SDLibraryFolder, ""))
                    End If
                    MatchFound = False
                End If
            Next

            Logger.WriteDebug(CallStack, "Software read from database: " + DbRegSwUUID.Count.ToString)
            Logger.WriteDebug(CallStack, "Software read from library: " + (DctRegSw.Count + DctMissingArcCount + DctMissingVolCount + DctMissingRegInfoCount + DctDuplicateCount + DctNotInDbCount).ToString)
            Logger.WriteDebug(CallStack, "Library items to cleanup: " + (DctMissingArcCount + DctMissingVolCount + DctMissingRegInfoCount + DctDuplicateCount + DctNotInDbCount).ToString)
            Logger.WriteDebug(CallStack, "Library folders to cleanup: " + (ArcsForDeletion.Count + RcpsForDeletion.Count).ToString)
            Logger.WriteDebug(CallStack, "Database items to cleanup: " + DbNotInDct.Count.ToString)

            LibraryEvents.Add("Software read from database: " + DbRegSwUUID.Count.ToString)
            LibraryEvents.Add("Software read from library: " + (DctRegSw.Count + DctMissingArcCount + DctMissingVolCount + DctMissingRegInfoCount + DctDuplicateCount + DctNotInDbCount).ToString)
            LibraryEvents.Add("Library items to cleanup: " + (DctMissingArcCount + DctMissingVolCount + DctMissingRegInfoCount + DctDuplicateCount + DctNotInDbCount).ToString)
            LibraryEvents.Add("Library folders to cleanup: " + (ArcsForDeletion.Count + RcpsForDeletion.Count).ToString)
            LibraryEvents.Add("Database items to cleanup: " + DbNotInDct.Count.ToString)

            ' Cleanup database
            If Globals.CleanupSDLibrarySwitch AndAlso Not Globals.CheckSDLibrarySwitch Then
                If DbNotInDct.Count > 0 Then

                    Dim SoftwareAffectedCount As Integer = 0
                    Dim ProcedureAffectedCount As Integer = 0
                    Dim ApplicationAffectedCount As Integer = 0
                    Dim GroupLinksAffectedCount As Integer = 0
                    Dim RetryCount As Integer = 0
                    Dim RetryLimit As Integer = 3
                    Dim WasSuccessful As Boolean = False

                    Do
                        Try
                            For i As Integer = DbNotInDct.Count - 1 To 0 Step -1

                                ' Convert the uniqueidentifier into string (really binary(16) for the package's usd_rsw.objectid)
                                Dim rsw_objectid As String = "0x" + BitConverter.ToString(Guid.Parse(DbNotInDct.Item(i)).ToByteArray).Replace("-", "")
                                Dim rsw_itemname As String = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select itemname from usd_rsw where objectid=" + rsw_objectid)
                                Dim rsw_itemversion As String = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select itemversion from usd_rsw where objectid=" + rsw_objectid)

                                Logger.WriteDebug(CallStack, "Delete from database: " + rsw_itemname + " " + rsw_itemversion + "(objectid=" + rsw_objectid + ")")

                                ' Retrive list of procedures associated with the rsw
                                Dim actproc_list As List(Of String) = DatabaseAPI.SqlSelectScalarList(CallStack, DbConnection, "select objectid from usd_actproc where rsw=" + rsw_objectid)

                                For Each actproc_uuid As String In actproc_list
                                    ' Retrieve procedure name
                                    Dim actproc_name As String = DatabaseAPI.SqlSelectScalar(CallStack, DbConnection, "select itemname from usd_actproc where objectid=" + actproc_uuid)
                                    SqlCmd = New SqlCommand("delete from usd_applic where actproc=" + actproc_uuid, DbConnection) ' Delete any associated application
                                    SqlData = SqlCmd.ExecuteReader()
                                    ApplicationAffectedCount += SqlData.RecordsAffected
                                    SqlData.Close()
                                    SqlCmd = New SqlCommand("delete from usd_actproc where objectid=" + actproc_uuid, DbConnection) ' Delete the procedure
                                    SqlData = SqlCmd.ExecuteReader()
                                    ProcedureAffectedCount += SqlData.RecordsAffected
                                    SqlData.Close()
                                Next

                                SqlCmd = New SqlCommand("delete from usd_rsw where objectid=" + rsw_objectid, DbConnection) ' Delete the software
                                SqlData = SqlCmd.ExecuteReader()
                                SoftwareAffectedCount += SqlData.RecordsAffected
                                SqlData.Close()

                                SqlCmd = New SqlCommand("delete from usd_link_swg_sw where sw=" + rsw_objectid, DbConnection) ' Delete the software group links
                                SqlData = SqlCmd.ExecuteReader()
                                GroupLinksAffectedCount += SqlData.RecordsAffected
                                SqlData.Close()

                                Logger.WriteDebug(CallStack, "Software deleted: " + SoftwareAffectedCount.ToString)
                                Logger.WriteDebug(CallStack, "Procedures deleted: " + ProcedureAffectedCount.ToString)
                                Logger.WriteDebug(CallStack, "Applications deleted: " + ApplicationAffectedCount.ToString)
                                Logger.WriteDebug(CallStack, "Group links deleted: " + GroupLinksAffectedCount.ToString)

                                LibraryEvents.Add("Software deleted: " + SoftwareAffectedCount.ToString)
                                LibraryEvents.Add("Procedures deleted: " + ProcedureAffectedCount.ToString)
                                LibraryEvents.Add("Applications deleted: " + ApplicationAffectedCount.ToString)
                                LibraryEvents.Add("Group links deleted: " + GroupLinksAffectedCount.ToString)

                                DbNotInDct.RemoveAt(i) ' Dequeue element (to avoid unnecessary retries)
                                SoftwareAffectedCount = 0
                                ProcedureAffectedCount = 0
                                ApplicationAffectedCount = 0
                                GroupLinksAffectedCount = 0
                                RetryCount = 0
                            Next
                            WasSuccessful = True ' All items processed -- set success flag
                        Catch ex As Exception
                            Logger.WriteDebug(CallStack, "Database exception: " + ex.Message)
                            If ex.Message.ToLower.Contains("execution timeout expired") OrElse ex.Message.ToLower.Contains("timeout period") Then
                                RetryCount += 1
                                Thread.Sleep(2000)
                            Else
                                RetryCount = RetryLimit
                            End If
                        End Try
                    Loop Until WasSuccessful = True OrElse RetryCount >= RetryLimit
                End If

                ' Cleanup library file
                If (DctMissingArcCount + DctMissingVolCount + DctMissingRegInfoCount + DctDuplicateCount + DctNotInDbCount) > 0 Then
                    If System.IO.File.Exists(LibraryFileBackup2) Then
                        Utility.DeleteFile(CallStack, LibraryFileBackup2)
                    End If

                    If System.IO.File.Exists(LibraryFileBackup1) Then
                        Logger.WriteDebug(CallStack, "Rename file: " + LibraryFileBackup1)
                        Logger.WriteDebug(CallStack, "To: " + LibraryFileBackup2)
                        System.IO.File.Move(LibraryFileBackup1, LibraryFileBackup2)

                    End If

                    If System.IO.File.Exists(LibraryFile) Then
                        Logger.WriteDebug(CallStack, "Copy file: " + LibraryFile)
                        Logger.WriteDebug(CallStack, "To: " + LibraryFileBackup1)
                        System.IO.File.Copy(LibraryFile, LibraryFileBackup1)
                    End If

                    Logger.WriteDebug(CallStack, "Write library file: " + LibraryFile)
                    LibraryFileWriter = New IO.StreamWriter(LibraryFile, False)
                    For Each LibraryLine As String In LibraryData
                        LibraryFileWriter.WriteLine(LibraryLine)
                    Next
                    Logger.WriteDebug(CallStack, "Close file: " + LibraryFile)
                    LibraryFileWriter.Close()
                End If

                ' Cleanup folders
                For Each folder As String In ArcsForDeletion
                    Utility.DeleteFolder(CallStack, folder)
                Next
                For Each folder As String In RcpsForDeletion
                    Utility.DeleteFolder(CallStack, folder)
                Next

                ' Sync staging library (ITCM servers only)
                If ITCMServer Then
                    If Utility.IsProcessRunning("caf") Then
                        Logger.WriteDebug(CallStack, "Library sync: sd_sscmd.exe deldctbu")
                        ReturnCode = Utility.RunCommand("sd_sscmd.exe", "deldctbu", False)
                        Logger.WriteDebug(CallStack, "Return code: " + ReturnCode.ToString)
                        LibraryEvents.Add("Library sync: " + "[Return Code: " + ReturnCode.ToString + "]")
                    End If
                End If

            End If

            ' Close database connection
            DbConnection.Close()
            Logger.WriteDebug(CallStack, "Database connection closed.")

        End Sub

    End Class

End Class
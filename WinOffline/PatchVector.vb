Partial Public Class WinOffline

    Public Class PatchVector

        Private _FileName As FileVector                     ' The patch file (Ex: C:\SomeDirectory\SomePatch.jcl)
        Private _InstructionList As ArrayList               ' List of patch instructions.
        Private _PatchAction As Integer                     ' Action code/result from applying patch.
        Private _PreCmdReturnCodes As ArrayList             ' Stores return codes from all PRESYSCMD scripts.
        Private _SysCmdReturnCodes As ArrayList             ' Stores return codes from all SYSCMD scripts.
        Private _FileReplaceResult As ArrayList             ' Stores result of file replacement operations.
        Private _CommentString As String                    ' Stores a comment.
        Private _SourceReplaceList As New ArrayList         ' Source location filename for each file replacement.
        Private _DestReplaceList As New ArrayList           ' Destination location filename for each file replacement.
        Private _ReplaceFolder As String                    ' Replacement location folder for storing original files.
        Private _ReplaceSubFolder As New ArrayList          ' Sub folder within replacement folder, for storing each original file.
        Private _SkipIfNotFoundList As New ArrayList        ' File replacements to skip, if destination file is not originally present (i.e. source file is new).
        Public Const UNAVAILABLE As Integer = -5            ' Apply action code: Unavailable. (Files are missing)
        Public Const NOT_APPLICABLE As Integer = -4         ' Apply action code: Not applicable.
        Public Const ALREADY_APPLIED As Integer = -3        ' Apply action code: Already applied.
        Public Const NO_ACTION As Integer = -2              ' Apply action code: No action.
        Public Const SKIPPED As Integer = -1                ' Apply action code: Skipped.
        Public Const APPLY_OK As Integer = 0                ' Apply action code: Apply ok.
        Public Const APPLY_FAIL As Integer = 1              ' Apply action code: Apply fail.
        Public Const EXECUTE_OK As Integer = 2              ' Apply action code: Execute ok.
        Public Const EXECUTE_FAIL As Integer = 3            ' Apply action code: Execute fail.
        Public Const FILE_REVERSED As Integer = -2          ' File replacement code: Reversed.
        Public Const FILE_SKIPPED As Integer = -1           ' File replacement code: Skipped.
        Public Const FILE_OK As Integer = 0                 ' File replacement code: File ok.
        Public Const FILE_REBOOT_REQUIRED As Integer = 1    ' File replacement code: Reboot required.
        Public Const FILE_FAILED As Integer = 2             ' File replacement code: File fail.
        Private ClientAutoProductCodes = New ArrayList({"BITCM", "DTSVMG", "DTMGSU", "TNGAMO", "TNGSDO", "TNGRCO"})
        Private SharedComponentProductCodes As New ArrayList({"DTMINF"})
        Private CAMProductCodes As New ArrayList({"CCSCAM", "TNGCAM"})
        Private SSAProductCodes As New ArrayList({"SSA"})
        Private DataTransportProductCodes As New ArrayList({"TNGDTS", "TNGDTS-30", "DTS"})
        Private ExplorerGUIProductCodes As New ArrayList({"EGC30N"})

        Public Sub New(ByVal FileName As String, ByVal InstructionList As ArrayList)

            _FileName = New FileVector(FileName)
            _InstructionList = New ArrayList(InstructionList)
            _PatchAction = NO_ACTION
            _PreCmdReturnCodes = New ArrayList
            _SysCmdReturnCodes = New ArrayList
            _FileReplaceResult = New ArrayList
            _CommentString = ""

            ' All these lists are built in parallel with each other:
            '   _SourceReplaceList --> Replacement source file
            '   _FileReplaceResult --> Replacement result code
            '   _DestReplaceList --> Replacement destination file
            '   _ReplaceSubFolder --> Replacement subfolder
            '   _ReplaceFolder --> Replacement base folder

            ' Generate source file list and stub SKIPPED for replacement status
            For Each ReplacedFile As String In GetShortNameReplaceList()
                _SourceReplaceList.Add(_FileName.GetFilePath + "\" + ReplacedFile)
                _FileReplaceResult.Add(FILE_SKIPPED)
            Next

            ' Generate destination file list
            For Each ReplacedFile As String In GetRawReplaceList()
                Dim ReplaceDestination As String = Nothing
                If IsClientAuto() Then
                    ReplaceDestination = Globals.DSMFolder + ReplacedFile
                ElseIf IsSharedComponent() Then
                    If ReplacedFile.ToLower.StartsWith("capki\capki") Then ' Special case -- CAPKI may not be with other shared components
                        ReplaceDestination = Globals.CAPKIFolder + ReplacedFile.ToLower.Replace("capki\capki", "") ' Use CAPKI folder
                    Else
                        ReplaceDestination = Globals.SharedCompFolder + ReplacedFile
                    End If
                ElseIf IsCAM() Then
                    ReplaceDestination = Globals.CAMFolder + "\" + ReplacedFile
                ElseIf IsSSA() Then
                    ReplaceDestination = Globals.SSAFolder + ReplacedFile
                ElseIf IsDataTransport() Then
                    ReplaceDestination = Globals.DTSFolder + "\" + ReplacedFile
                ElseIf IsExplorerGUI() Then
                    ReplaceDestination = Globals.EGCFolder + ReplacedFile
                End If

                ' Resolve parent directories e.g. D:\CA\DSM\..\SC\CBB\_SomeFile.txt --> D:\CA\SC\CBB\_SomeFile.txt
                While ReplaceDestination.Contains("\..\")
                    Dim Front As String = ReplaceDestination.Substring(0, ReplaceDestination.IndexOf("\..\"))
                    Front = Front.Substring(0, Front.LastIndexOf("\"))
                    Dim Back As String = ReplaceDestination.Substring(ReplaceDestination.IndexOf("\..\") + 3)
                    ReplaceDestination = Front + Back
                End While
                _DestReplaceList.Add(ReplaceDestination)

                ' Generate a subfolder list
                If ReplacedFile.Contains("\") Then
                    _ReplaceSubFolder.Add(ReplacedFile.Substring(0, ReplacedFile.LastIndexOf("\")))
                Else
                    _ReplaceSubFolder.Add("")
                End If
            Next

            If IsClientAuto() Then _ReplaceFolder = Globals.DSMFolder + "REPLACED\" + _FileName.GetFriendlyName.ToUpper
            If IsSharedComponent() Then _ReplaceFolder = Globals.SharedCompFolder + "REPLACED\" + _FileName.GetFriendlyName.ToUpper
            If IsCAM() Then _ReplaceFolder = Globals.CAMFolder + "\REPLACED\" + _FileName.GetFriendlyName.ToUpper
            If IsSSA() Then _ReplaceFolder = Globals.SSAFolder + "REPLACED\" + _FileName.GetFriendlyName.ToUpper
            If IsDataTransport() Then _ReplaceFolder = Globals.DTSFolder + "\REPLACED\" + _FileName.GetFriendlyName.ToUpper
            If IsExplorerGUI() Then _ReplaceFolder = Globals.EGCFolder + "REPLACED\" + _FileName.GetFriendlyName.ToUpper

            ' Build "skip if not found" list (files to be replaced only if they exist on the target)
            _SkipIfNotFoundList = GetSkipFileList()

        End Sub

        Public Function GetRawInstruction(ByVal Name As String) As String
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith(Name.ToLower) Then
                    Return strLine
                End If
            Next
            Return ""
        End Function

        Public Function GetInstruction(ByVal Name As String) As String
            Dim ReturnString As String = ""
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith(Name.ToLower) And strLine.Contains(":") Then
                    strLine = strLine.Substring(strLine.IndexOf(":") + 1)
                    If strLine.Contains(":") Then
                        strLine = strLine.Substring(0, strLine.IndexOf(":"))
                    End If
                    If ReturnString.Equals("") Then
                        ReturnString += strLine
                    Else
                        ReturnString += ";" + strLine
                    End If
                    Exit For
                End If
            Next
            Return ReturnString
        End Function

        Public Function GetRawReplaceList() As ArrayList
            Dim ReturnList As New ArrayList
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith("file:") Then
                    strLine = strLine.Substring(strLine.IndexOf(":") + 1)
                    If strLine.Contains(":") Then
                        strLine = strLine.Substring(0, strLine.IndexOf(":"))
                    End If
                    ReturnList.Add(strLine)
                End If
            Next
            Return ReturnList
        End Function

        Public Function GetShortNameReplaceList() As ArrayList
            Dim ReturnList As New ArrayList
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith("file:") Then
                    strLine = strLine.Substring(strLine.IndexOf(":") + 1)
                    If strLine.Contains(":") Then
                        strLine = strLine.Substring(0, strLine.IndexOf(":"))
                    End If
                    If strLine.Contains("\") Then
                        strLine = strLine.Substring(strLine.LastIndexOf("\") + 1)
                    End If
                    ReturnList.Add(strLine)
                End If
            Next
            Return ReturnList
        End Function

        Public Function GetSkipFileList() As ArrayList
            Dim ReturnList As New ArrayList
            For Each ReplacedFile As String In InstructionList
                If ReplacedFile.ToLower.StartsWith("file:") And ReplacedFile.ToLower.Contains(":skipifnotfound:") Then
                    ReplacedFile = ReplacedFile.Substring(ReplacedFile.IndexOf(":") + 1)
                    ReplacedFile = ReplacedFile.Substring(0, ReplacedFile.IndexOf(":"))
                    Dim ReplaceDestination As String = Nothing
                    If IsClientAuto() Then
                        ReplaceDestination = Globals.DSMFolder + ReplacedFile
                    ElseIf IsSharedComponent() Then
                        If ReplacedFile.ToLower.StartsWith("capki\capki") Then ' Special case -- CAPKI may not be with other shared components
                            ReplaceDestination = Globals.CAPKIFolder + ReplacedFile.ToLower.Replace("capki\capki", "") ' Use CAPKI folder
                        Else
                            ReplaceDestination = Globals.SharedCompFolder + ReplacedFile
                        End If
                    ElseIf IsCAM() Then
                        ReplaceDestination = Globals.CAMFolder + "\" + ReplacedFile
                    ElseIf IsSSA() Then
                        ReplaceDestination = Globals.SSAFolder + ReplacedFile
                    ElseIf IsDataTransport() Then
                        ReplaceDestination = Globals.DTSFolder + "\" + ReplacedFile
                    ElseIf IsExplorerGUI() Then
                        ReplaceDestination = Globals.EGCFolder + ReplacedFile
                    End If

                    ' Resolve parent directories e.g. D:\CA\DSM\..\SC\CBB\_SomeFile.txt --> D:\CA\SC\CBB\_SomeFile.txt
                    While ReplaceDestination.Contains("\..\")
                        Dim Front As String = ReplaceDestination.Substring(0, ReplaceDestination.IndexOf("\..\"))
                        Front = Front.Substring(0, Front.LastIndexOf("\"))
                        Dim Back As String = ReplaceDestination.Substring(ReplaceDestination.IndexOf("\..\") + 3)
                        ReplaceDestination = Front + Back
                    End While

                    ReturnList.Add(ReplaceDestination.ToLower)
                End If
            Next
            Return ReturnList
        End Function

        Public Function GetPreCommandList() As ArrayList
            Dim ReturnList As New ArrayList
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith("presyscmd:") Then
                    strLine = strLine.Substring(strLine.IndexOf(":") + 1)
                    strLine = strLine.Replace(" ", "")
                    If strLine.Contains(",") Then
                        ReturnList.AddRange(strLine.Split(","))
                    Else
                        ReturnList.Add(strLine)
                    End If
                End If
            Next
            Return ReturnList
        End Function

        Public Function GetSysCommandList() As ArrayList
            Dim ReturnList As New ArrayList
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith("syscmd:") Then
                    strLine = strLine.Substring(strLine.IndexOf(":") + 1)
                    strLine = strLine.Replace(" ", "")
                    If strLine.Contains(",") Then
                        ReturnList.AddRange(strLine.Split(","))
                    Else
                        ReturnList.Add(strLine)
                    End If
                End If
            Next
            Return ReturnList
        End Function

        Public Function GetDependsList() As ArrayList
            Dim ReturnList As New ArrayList
            For Each strLine As String In InstructionList
                If strLine.ToLower.StartsWith("depends:") Then
                    strLine = strLine.Substring(strLine.IndexOf(":") + 1)
                    strLine = strLine.Replace(" ", "")
                    If strLine.Contains(",") Then
                        ReturnList.AddRange(strLine.Split(","))
                    Else
                        ReturnList.Add(strLine)
                    End If
                End If
            Next
            Return ReturnList
        End Function

        Public Function GetAllDependentFiles() As ArrayList
            Dim ReturnArray As New ArrayList
            For Each strLine As String In GetShortNameReplaceList()
                ReturnArray.Add(strLine)
            Next
            For Each strLine As String In GetPreCommandList()
                ReturnArray.Add(strLine)
            Next
            For Each strLine As String In GetSysCommandList()
                ReturnArray.Add(strLine)
            Next
            For Each strLine As String In GetDependsList()
                ReturnArray.Add(strLine)
            Next
            Return ReturnArray
        End Function

        Public Function GetApplyPTFDateFormat() As String
            Dim ReturnString As String = ""
            Dim CurrentTime As DateTime = DateTime.Now
            ReturnString += CurrentTime.ToString("D").Substring(0, 3) + " "
            ReturnString += CurrentTime.ToString("M").Substring(0, 3) + " "
            ReturnString += CurrentTime.Day.ToString + " "
            ReturnString += CurrentTime.TimeOfDay.ToString.Substring(0, CurrentTime.TimeOfDay.ToString.IndexOf(".")) + " "
            ReturnString += CurrentTime.Year.ToString
            Return ReturnString
        End Function

        Public Function GetApplyPTFSummary() As String
            Dim ReturnString As String = "["
            Dim CurrentTime As DateTime = DateTime.Now
            ReturnString += GetApplyPTFDateFormat() + "] - PTF Wizard "
            If _PatchAction = 0 Then
                ReturnString += "installed " + _FileName.GetFriendlyName + " (" + GetInstruction("PRODUCT") + ")" + Environment.NewLine
                ReturnString += GetRawInstruction("RELEASE").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("GENLEVEL").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("COMPONENT").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("PREREQS").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("MPREREQS").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("COREQS").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("MCOREQS").Replace(":", "=") + Environment.NewLine
                ReturnString += GetRawInstruction("SUPERSEDE").Replace(":", "=")
                For x As Integer = 0 To _DestReplaceList.Count - 1
                    Dim strLine As String = _DestReplaceList.Item(x)
                    If _FileReplaceResult.Item(x) = FILE_OK Or _FileReplaceResult.Item(x) = FILE_REBOOT_REQUIRED Then
                        ReturnString += Environment.NewLine + "INSTALLEDFILE=" + strLine + ",TIME="
                        ReturnString += System.IO.File.GetLastWriteTime(strLine).ToString("D").Substring(0, 3) + " "
                        ReturnString += System.IO.File.GetLastWriteTime(strLine).ToString("M").Substring(0, 3) + " "
                        ReturnString += System.IO.File.GetLastWriteTime(strLine).Day.ToString + " "
                        ReturnString += System.IO.File.GetLastWriteTime(strLine).TimeOfDay.ToString.Substring(0, CurrentTime.TimeOfDay.ToString.IndexOf(".")) + " "
                        ReturnString += System.IO.File.GetLastWriteTime(strLine).Year.ToString
                    End If
                Next
                ReturnString += Environment.NewLine
            ElseIf _PatchAction = 1 Then
                ReturnString += "backed out " + _FileName.GetFriendlyName + " (" + GetInstruction("PRODUCT") + ")" + Environment.NewLine
                Return ReturnString
            End If
            If ReturnString.EndsWith(Environment.NewLine + Environment.NewLine) Then
                ReturnString = ReturnString.Remove(ReturnString.LastIndexOf(Environment.NewLine))
            End If
            Return ReturnString
        End Function

        Public Function WereAllFileReplacementsSkipped() As Boolean
            If _FileReplaceResult.Count > 0 Then
                For Each result As Integer In _FileReplaceResult
                    If Not result = FILE_SKIPPED Then Return False
                Next
                Return True
            End If
            Return False
        End Function

        Public Function IsClientAuto() As Boolean
            For Each ProductCode As String In ClientAutoProductCodes
                If ProductCode.ToLower.Equals(GetInstruction("PRODUCT").ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsSharedComponent() As Boolean
            For Each ProductCode As String In SharedComponentProductCodes
                If ProductCode.ToLower.Equals(GetInstruction("PRODUCT").ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsCAM() As Boolean
            For Each ProductCode As String In CAMProductCodes
                If ProductCode.ToLower.Equals(GetInstruction("PRODUCT").ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsSSA() As Boolean
            For Each ProductCode As String In SSAProductCodes
                If ProductCode.ToLower.Equals(GetInstruction("PRODUCT").ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsDataTransport() As Boolean
            For Each ProductCode As String In DataTransportProductCodes
                If ProductCode.ToLower.Equals(GetInstruction("PRODUCT").ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function IsExplorerGUI() As Boolean
            For Each ProductCode As String In ExplorerGUIProductCodes
                If ProductCode.ToLower.Equals(GetInstruction("PRODUCT").ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Property PatchFile As FileVector
            Get
                Return _FileName
            End Get
            Set(value As FileVector)
                _FileName = value
            End Set
        End Property

        Public Property InstructionList As ArrayList
            Get
                Return _InstructionList
            End Get
            Set(value As ArrayList)
                _InstructionList = value
            End Set
        End Property

        Public Property PatchAction As Integer
            Get
                Return _PatchAction
            End Get
            Set(value As Integer)
                _PatchAction = value
            End Set
        End Property

        Public Property PreCmdReturnCodes As ArrayList
            Get
                Return _PreCmdReturnCodes
            End Get
            Set(value As ArrayList)
                _PreCmdReturnCodes = value.Clone
            End Set
        End Property

        Public Property SysCmdReturnCodes As ArrayList
            Get
                Return _SysCmdReturnCodes
            End Get
            Set(value As ArrayList)
                _SysCmdReturnCodes = value.Clone
            End Set
        End Property
        Public Property FileReplaceResult As ArrayList
            Get
                Return _FileReplaceResult
            End Get
            Set(value As ArrayList)
                _FileReplaceResult = value.Clone
            End Set
        End Property

        Public Property CommentString As String
            Get
                Return _CommentString
            End Get
            Set(value As String)
                _CommentString = value
            End Set
        End Property

        Public ReadOnly Property SourceReplaceList As ArrayList
            Get
                Return _SourceReplaceList
            End Get
        End Property

        Public ReadOnly Property DestReplaceList As ArrayList
            Get
                Return _DestReplaceList
            End Get
        End Property

        Public ReadOnly Property ReplaceFolder As String
            Get
                Return _ReplaceFolder
            End Get
        End Property

        Public ReadOnly Property ReplaceSubFolder As ArrayList
            Get
                Return _ReplaceSubFolder
            End Get
        End Property

        Public ReadOnly Property SkipIfNotFoundList As ArrayList
            Get
                Return _SkipIfNotFoundList
            End Get
        End Property

        Public Overrides Function toString() As String
            Dim ReturnString As String = ""
            ReturnString += _FileName.GetFileName() + Environment.NewLine()
            For Each strLine As String In InstructionList
                ReturnString += strLine + ";"
            Next
            ReturnString += Environment.NewLine() + _PatchAction.ToString
            ReturnString += Environment.NewLine() + "PRESYSCMD:"
            For Each strLine As String In _PreCmdReturnCodes
                ReturnString += strLine + ";"
            Next
            ReturnString += Environment.NewLine() + "SYSCMD:"
            For Each strLine As String In _SysCmdReturnCodes
                ReturnString += strLine + ";"
            Next
            ReturnString += Environment.NewLine() + "FILE_REPLACE:"
            For Each strLine As String In _FileReplaceResult
                ReturnString += strLine + ";"
            Next
            ReturnString += Environment.NewLine + _CommentString
            Return ReturnString
        End Function

        Public Shared Function ReadCache(ByVal CallStack As String) As ArrayList
            Dim CacheFile As String = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-patch.cache"
            Dim CacheReader As System.IO.StreamReader
            Dim strLine As String = ""
            Dim strLine2 As String = ""
            Dim strLine3 As String = ""
            Dim InstructionList As New ArrayList
            Dim CodeList As New ArrayList
            Dim pVector As PatchVector
            Dim RestoredManifest As New ArrayList
            Logger.WriteDebug(CallStack, "Open file: " + CacheFile)
            If Not System.IO.File.Exists(CacheFile) Then
                Logger.WriteDebug(CallStack, "Cache file does not exist.")
                Return Nothing
            End If
            CacheReader = New System.IO.StreamReader(CacheFile)
            Do While CacheReader.Peek() >= 0
                strLine = CacheReader.ReadLine
                strLine2 = CacheReader.ReadLine
                InstructionList = New ArrayList
                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    InstructionList.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While
                pVector = New PatchVector(strLine, InstructionList)
                strLine = CacheReader.ReadLine() ' Read patch action
                pVector.PatchAction = Integer.Parse(strLine)
                strLine2 = CacheReader.ReadLine ' Read PRESYSCMD return codes
                strLine2 = strLine2.Replace("PRESYSCMD:", "")
                CodeList = New ArrayList
                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    CodeList.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While
                pVector.PreCmdReturnCodes = CodeList
                strLine2 = CacheReader.ReadLine ' Read SYSCMD return codes
                strLine2 = strLine2.Replace("SYSCMD:", "")
                CodeList = New ArrayList
                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    CodeList.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While
                pVector.SysCmdReturnCodes = CodeList
                strLine2 = CacheReader.ReadLine ' Read file replacement codes
                strLine2 = strLine2.Replace("FILE_REPLACE:", "")
                CodeList = New ArrayList
                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    CodeList.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While
                pVector.FileReplaceResult = CodeList
                strLine = CacheReader.ReadLine ' Read comment string
                pVector.CommentString = strLine
                RestoredManifest.Add(pVector)
                Logger.WriteDebug(CallStack, "Read: " + pVector.PatchFile.GetFriendlyName)
            Loop
            Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
            CacheReader.Close()

            Return RestoredManifest

        End Function

        Public Shared Sub WriteCache(ByVal CallStack As String, ByVal PatchManifest As ArrayList)

            Dim CacheFile As String = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-patch.cache"
            Dim CacheWriter As System.IO.StreamWriter
            Dim CurPatch As PatchVector

            ' Flush prior cache file
            If System.IO.File.Exists(CacheFile) Then
                Utility.DeleteFile(CallStack, CacheFile)
            ElseIf Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                Logger.WriteDebug(CallStack, "Warning: Temporary folder already cleaned up!")
                Logger.WriteDebug(CallStack, "Warning: Cache file write aborted!")
                Return
            End If

            ' Create a new cache file
            Logger.WriteDebug(CallStack, "Write cache file: " + CacheFile)
            CacheWriter = New System.IO.StreamWriter(CacheFile, False)
            For n As Integer = 0 To PatchManifest.Count - 1
                CurPatch = DirectCast(PatchManifest.Item(n), PatchVector)
                Logger.WriteDebug(CallStack, "Write: " + CurPatch.PatchFile.GetFriendlyName)
                CacheWriter.WriteLine(CurPatch.toString)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
            CacheWriter.Close()

        End Sub

    End Class

End Class
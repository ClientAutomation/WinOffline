'****************************** Class Header *******************************\
' Project Name: WinOffline
' Class Name:   WinOffline.RemovalVector
' File Name:    PatchVector.vb
' Author:       Brian Fontana
'***************************************************************************/

Partial Public Class WinOffline

    Public Class RemovalVector

        Private _RemovalItem As String                      ' Name of patch to be removed.
        Private _RemovalAction As Integer                   ' Action code/result from removing patch.
        Private _HistoryFile As String                      ' History file related to removal operation.
        Private _RemovalFileName As ArrayList               ' Stores list of files removed.
        Private _FileRemovalResult As ArrayList             ' Stores result of removal operation.
        Private _CommentString As String                    ' Stores a comment.
        Public Const NO_ACTION As Integer = -2              ' Removal action code: No action.
        Public Const SKIPPED As Integer = -1                ' Removal action code: Skipped.
        Public Const REMOVAL_OK As Integer = 0              ' Removal action code: Removal ok.
        Public Const REMOVAL_FAIL As Integer = 1            ' Removal action code: Removal fail.
        Public Const FILE_REVERSED As Integer = -2          ' File replacement code: Reversed.
        Public Const FILE_SKIPPED As Integer = -1           ' File replacement code: Skipped.
        Public Const FILE_OK As Integer = 0                 ' File replacement code: File ok.
        Public Const FILE_REBOOT_REQUIRED As Integer = 1    ' File replacement code: Reboot required.
        Public Const FILE_FAILED As Integer = 2             ' File replacement code: File failed.

        Public Sub New(ByVal RemovalItem As String)
            _RemovalItem = RemovalItem
            _RemovalAction = NO_ACTION
            _RemovalFileName = New ArrayList
            _FileRemovalResult = New ArrayList
            _CommentString = ""
        End Sub

        Public Property RemovalItem As String
            Get
                Return _RemovalItem
            End Get
            Set(value As String)
                _RemovalItem = value
            End Set
        End Property

        Public Property RemovalAction As Integer
            Get
                Return _RemovalAction
            End Get
            Set(value As Integer)
                _RemovalAction = value
            End Set
        End Property

        Public Property HistoryFile As String
            Get
                Return _HistoryFile
            End Get
            Set(value As String)
                _HistoryFile = value
            End Set
        End Property

        Public Property RemovalFileName As ArrayList
            Get
                Return _RemovalFileName
            End Get
            Set(value As ArrayList)
                _RemovalFileName = value.Clone
            End Set
        End Property

        Public Property FileRemovalResult As ArrayList
            Get
                Return _FileRemovalResult
            End Get
            Set(value As ArrayList)
                _FileRemovalResult = value.Clone
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

        Public Overrides Function toString() As String
            Dim ReturnString As String = ""
            ReturnString += _RemovalItem + Environment.NewLine
            ReturnString += _RemovalAction.ToString + Environment.NewLine
            ReturnString += _HistoryFile
            ReturnString += Environment.NewLine() + "FILE_REPLACE:"
            For Each strLine As String In RemovalFileName
                ReturnString += strLine + ";"
            Next
            ReturnString += Environment.NewLine() + "FILE_RESULT:"
            For Each strLine As String In FileRemovalResult
                ReturnString += strLine + ";"
            Next
            ReturnString += Environment.NewLine + _CommentString
            Return ReturnString
        End Function

        Public Shared Function ReadCache(ByVal CallStack As String) As ArrayList
            Dim CacheFile As String = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-removal.cache"
            Dim CacheReader As System.IO.StreamReader
            Dim strLine As String = ""
            Dim strLine2 As String = ""
            Dim strLine3 As String = ""
            Dim CodeList As New ArrayList
            Dim rVector As RemovalVector
            Dim RestoredManifest As New ArrayList
            Logger.WriteDebug(CallStack, "Open file: " + CacheFile)
            If Not System.IO.File.Exists(CacheFile) Then
                Logger.WriteDebug(CallStack, "Cache file does not exist.")
                Return Nothing
            End If
            CacheReader = New System.IO.StreamReader(CacheFile)
            Do While CacheReader.Peek() >= 0
                strLine = CacheReader.ReadLine ' Read a removal item
                rVector = New RemovalVector(strLine)
                strLine = CacheReader.ReadLine ' Read removal action
                rVector.RemovalAction = Integer.Parse(strLine)
                strLine = CacheReader.ReadLine ' Read history file
                rVector.HistoryFile = strLine
                strLine2 = CacheReader.ReadLine ' Read removal filenames
                strLine2 = strLine2.Replace("FILE_REPLACE:", "")
                CodeList = New ArrayList
                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    CodeList.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While
                rVector.RemovalFileName = CodeList
                strLine2 = CacheReader.ReadLine ' Read removal replacement codes
                strLine2 = strLine2.Replace("FILE_RESULT:", "")
                CodeList = New ArrayList
                While strLine2.Contains(";") And strLine2.Length > 1
                    strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                    CodeList.Add(strLine3)
                    strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
                End While
                rVector.FileRemovalResult = CodeList
                strLine = CacheReader.ReadLine ' Read comment string
                rVector.CommentString = strLine
                RestoredManifest.Add(rVector)
                Logger.WriteDebug(CallStack, "Read: " + rVector.RemovalItem)
            Loop
            Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
            CacheReader.Close()

            Return RestoredManifest

        End Function

        Public Shared Sub WriteCache(ByVal CallStack As String, ByVal RemovalManifest As ArrayList)
            Dim CacheFile As String = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-removal.cache"
            Dim CacheWriter As System.IO.StreamWriter

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
            For Each RemoveItem As RemovalVector In RemovalManifest
                Logger.WriteDebug(CallStack, "Write: " + RemoveItem.RemovalItem)
                CacheWriter.WriteLine(RemoveItem)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
            CacheWriter.Close()

        End Sub

        Public Shared Function ReadString(ByVal CacheReader As System.IO.StreamReader) As RemovalVector

            Dim strLine As String = ""
            Dim strLine2 As String = ""
            Dim strLine3 As String = ""
            Dim CodeList As New ArrayList
            Dim rVector As RemovalVector

            strLine = CacheReader.ReadLine ' Read a removal item
            rVector = New RemovalVector(strLine)
            strLine = CacheReader.ReadLine ' Read removal action
            rVector.RemovalAction = Integer.Parse(strLine)
            strLine = CacheReader.ReadLine ' Read history file
            rVector.HistoryFile = strLine
            strLine2 = CacheReader.ReadLine ' Read removal filenames
            strLine2 = strLine2.Replace("FILE_REPLACE:", "")
            CodeList = New ArrayList
            While strLine2.Contains(";") And strLine2.Length > 1
                strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                CodeList.Add(strLine3)
                strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
            End While
            rVector.RemovalFileName = CodeList
            strLine2 = CacheReader.ReadLine ' Read removal replacement codes
            strLine2 = strLine2.Replace("FILE_RESULT:", "")
            CodeList = New ArrayList
            While strLine2.Contains(";") And strLine2.Length > 1
                strLine3 = strLine2.Substring(0, strLine2.IndexOf(";"))
                CodeList.Add(strLine3)
                strLine2 = strLine2.Substring(strLine2.IndexOf(";") + 1)
            End While
            rVector.FileRemovalResult = CodeList
            strLine = CacheReader.ReadLine ' Read comment string
            rVector.CommentString = strLine

            Return rVector

        End Function

    End Class

End Class
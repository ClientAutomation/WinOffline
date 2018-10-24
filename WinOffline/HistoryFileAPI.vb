Partial Public Class WinOffline

    Public Const IT_CLIENT_MANAGER As String = "IT Client Manager -- ITCM"
    Public Const SHARED_COMPONENTS As String = "Shared Components -- SC"
    Public Const CA_MESSAGE_QUEUING As String = "CA Message Queuing Service -- CAM"
    Public Const SECURE_SOCKET_ADAPATER As String = "Secure Socket Adapter -- SSA"
    Public Const DATA_TRANSPORT As String = "Data Transport Service -- DTS"
    Public Const EXPLORER_GUI As String = "DSM Explorer GUI -- EGC"

    Public Shared Function ProcessHistoryFiles(ByVal CallStack As String)

        Dim Runlevel As Integer = 0

        CallStack += "HistoryFile|"

        ' Read component history files
        Runlevel = ReadHistoryFiles(CallStack, IT_CLIENT_MANAGER, Globals.DSMFolder)
        If Runlevel <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Failed to process ITCM history file.")
            Return 1
        End If

        Runlevel = ReadHistoryFiles(CallStack, SHARED_COMPONENTS, Globals.SharedCompFolder)
        If Runlevel <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Failed to process shared components history file.")
            Return 2
        End If

        Runlevel = ReadHistoryFiles(CallStack, CA_MESSAGE_QUEUING, Globals.CAMFolder + "\")
        If Runlevel <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Failed to process CAM history file.")
            Return 3
        End If

        If Globals.SSAFolder IsNot Nothing Then
            Runlevel = ReadHistoryFiles(CallStack, SECURE_SOCKET_ADAPATER, Globals.SSAFolder)
            If Runlevel <> 0 Then
                Logger.WriteDebug(CallStack, "Error: Failed to process SSA history file.")
                Return 4
            End If
        End If

        If Globals.DTSFolder IsNot Nothing Then
            Runlevel = ReadHistoryFiles(CallStack, DATA_TRANSPORT, Globals.DTSFolder + "\")
            If Runlevel <> 0 Then
                Logger.WriteDebug(CallStack, "Error: Failed to process DTS history file.")
                Return 5
            End If
        End If

        If Globals.EGCFolder IsNot Nothing Then
            Runlevel = ReadHistoryFiles(CallStack, EXPLORER_GUI, Globals.EGCFolder)
            If Runlevel <> 0 Then
                Logger.WriteDebug(CallStack, "Error: Failed to process EGC history file.")
                Return 6
            End If
        End If

        Return 0

    End Function

    Public Shared Function ReadHistoryFiles(ByVal CallStack As String, ByVal Component As String, ByVal HistoryBaseFolder As String)

        Dim FileList As String()
        Dim HistoryFileFound As Boolean = False
        Dim HistoryFileReader As System.IO.StreamReader
        Dim HistoryArray As New ArrayList
        Dim strLine As String
        Dim HistoryEvents As New ArrayList
        Dim hVector As HistoryVector
        Dim PatchCount As Integer = 0

        Logger.WriteDebug(CallStack, "Component: " + Component)

        If System.IO.Directory.Exists(HistoryBaseFolder) Then
            Try

                ' Read all history files (Default is ComputerName.his, but system may have been renamed)
                FileList = System.IO.Directory.GetFiles(HistoryBaseFolder)
                For n As Integer = 0 To FileList.Length - 1
                    If FileList(n).ToLower.EndsWith(".his") Then
                        HistoryFileFound = True
                        Logger.WriteDebug(CallStack, "Open file: " + FileList(n))
                        Logger.WriteDebug("############################################################")
                        HistoryFileReader = New System.IO.StreamReader(FileList(n))
                        Do While HistoryFileReader.Peek() >= 0
                            strLine = HistoryFileReader.ReadLine()
                            Logger.WriteDebug(strLine)
                            HistoryArray.Add(strLine)
                        Loop
                        Logger.WriteDebug("############################################################")
                        Logger.WriteDebug(CallStack, "Close file: " + FileList(n))
                        HistoryFileReader.Close()
                    End If
                Next

                ' Process history
                For i As Integer = 0 To HistoryArray.Count - 1
                    strLine = HistoryArray.Item(i)
                    If strLine.StartsWith("[") And strLine.ToLower.Contains("installed") Then
                        HistoryEvents = New ArrayList
                        HistoryEvents.Add(strLine)
                        For j As Integer = i + 1 To HistoryArray.Count - 1
                            strLine = HistoryArray.Item(j)
                            If strLine.StartsWith("[") Then
                                Exit For
                            Else
                                HistoryEvents.Add(strLine)
                            End If
                            i = j
                        Next
                        hVector = New HistoryVector(Component, HistoryEvents)
                        Logger.WriteDebug(CallStack, "Installed patch: " + hVector.GetPatchName)
                        Manifest.UpdateManifest(CallStack, Manifest.HISTORY_MANIFEST, hVector)
                        PatchCount += 1
                    ElseIf strLine.StartsWith("[") And strLine.ToLower.Contains("backed out") Then
                        HistoryEvents = New ArrayList
                        HistoryEvents.Add(strLine)
                        hVector = New HistoryVector(Component, HistoryEvents)
                        Logger.WriteDebug(CallStack, "Removed patch: " + hVector.GetPatchName)
                        Manifest.UpdateManifest(CallStack, Manifest.HISTORY_MANIFEST, hVector)
                        PatchCount += 1
                    End If
                Next

                If PatchCount = 0 Then
                    hVector = New HistoryVector(Component, New ArrayList({"NoHistory"}))
                    Logger.WriteDebug(CallStack, "History file is empty.")
                    Manifest.UpdateManifest(CallStack, Manifest.HISTORY_MANIFEST, hVector)
                End If

            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught during history file processing.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 1
            End Try

        Else
            hVector = New HistoryVector(Component, New ArrayList({"NoHistory"}))
            Logger.WriteDebug(CallStack, "History file: Unavailable.")
            Manifest.UpdateManifest(CallStack, Manifest.HISTORY_MANIFEST, hVector)
        End If

        Return 0

    End Function

    Public Shared Sub AddtoHistory(ByVal CallStack As String, ByVal pVector As PatchVector)

        Dim HistoryFile As String = ""
        Dim HistoryFileInfo As System.IO.FileInfo
        Dim HistoryFileWriter As System.IO.StreamWriter
        Dim NewFile As Boolean = False

        CallStack += "HistoryFile|"

        ' Stub proper history file location
        If pVector.IsClientAuto Then HistoryFile = Globals.DSMFolder + Globals.HostName + ".his"
        If pVector.IsSharedComponent Then HistoryFile = Globals.SharedCompFolder + Globals.HostName + ".his"
        If pVector.IsCAM Then HistoryFile = Globals.CAMFolder + "\" + Globals.HostName + ".his"
        If pVector.IsSSA Then HistoryFile = Globals.SSAFolder + Globals.HostName + ".his"
        If pVector.IsDataTransport Then HistoryFile = Globals.DTSFolder + "\" + Globals.HostName + ".his"
        If pVector.IsExplorerGUI Then HistoryFile = Globals.EGCFolder + Globals.HostName + ".his"

        Try
            ' Append history file
            Logger.WriteDebug(CallStack, "Append history file: " + HistoryFile)
            If System.IO.File.Exists(HistoryFile) Then
                HistoryFileInfo = New System.IO.FileInfo(HistoryFile)
                If HistoryFileInfo.Attributes And IO.FileAttributes.ReadOnly Then
                    Logger.WriteDebug(CallStack, "Unset read-only file attribute: " + HistoryFile)
                    HistoryFileInfo.Attributes = HistoryFileInfo.Attributes Xor FileAttribute.ReadOnly
                End If
                NormalizeHistoryFile(CallStack, HistoryFile)
            Else
                NewFile = True
            End If

            ' Write new history file
            HistoryFileWriter = New IO.StreamWriter(HistoryFile, True)
            If NewFile Then
                Logger.WriteDebug(CallStack, "Write: " + "[" + pVector.GetApplyPTFDateFormat + "] - PTF Wizard created this history file.")
                HistoryFileWriter.WriteLine("[" + pVector.GetApplyPTFDateFormat + "] - PTF Wizard created this history file." + Environment.NewLine)
            End If
            Logger.WriteDebug(CallStack, "Write patch summary: ")
            Logger.WriteDebug("############################################################")
            Logger.WriteDebug(pVector.GetApplyPTFSummary)
            Logger.WriteDebug("############################################################")
            HistoryFileWriter.WriteLine(pVector.GetApplyPTFSummary)
            Logger.WriteDebug(CallStack, "Close file: " + HistoryFile)
            HistoryFileWriter.Close()

        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught appending history file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return
        End Try

        Return

    End Sub

    Public Shared Sub RemoveFromHistory(ByVal CallStack As String, ByVal rVector As RemovalVector)

        Dim HistoryFileReader As System.IO.StreamReader
        Dim HistoryFileWriter As System.IO.StreamWriter
        Dim strLine As String
        Dim HistoryFileArray As New ArrayList
        Dim BeginIndex As Integer = -1
        Dim EndIndex As Integer = -1
        Dim DelimCount As Integer = 0
        Dim HistoryFileInfo As System.IO.FileInfo

        CallStack += "HistoryFile|"

        ' Read history file
        If System.IO.File.Exists(rVector.HistoryFile) Then
            Try
                Logger.WriteDebug(CallStack, "Read history file: " + rVector.HistoryFile)
                HistoryFileReader = New System.IO.StreamReader(rVector.HistoryFile)
                Do While HistoryFileReader.Peek() >= 0
                    strLine = HistoryFileReader.ReadLine()
                    HistoryFileArray.Add(strLine)
                Loop
                Logger.WriteDebug(CallStack, "Close file: " + rVector.HistoryFile)
                HistoryFileReader.Close()
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught reading history file.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return
            End Try

        End If

        ' Remove last patch occurrence of specified patch
        Try
            For i As Integer = 0 To HistoryFileArray.Count - 1
                If HistoryFileArray.Item(i).ToString.ToLower.Contains("ptf wizard installed " + rVector.RemovalItem.ToLower) Then
                    BeginIndex = i
                    EndIndex = 0
                    For j As Integer = i + 1 To HistoryFileArray.Count - 1
                        If HistoryFileArray.Item(j).ToString.StartsWith("[") Or j = HistoryFileArray.Count - 1 Then
                            EndIndex = j
                            Exit For
                        End If
                    Next
                End If
            Next
            If BeginIndex <> -1 And EndIndex <> -1 And BeginIndex < EndIndex Then
                HistoryFileArray.RemoveRange(BeginIndex, EndIndex - BeginIndex)
                Logger.WriteDebug(CallStack, "Last occurrence removed: " + rVector.RemovalItem)
            End If

        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught removing last patch occurrence.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return
        End Try

        ' Write history file
        Try
            HistoryFileInfo = New System.IO.FileInfo(rVector.HistoryFile)
            If HistoryFileInfo.Attributes And IO.FileAttributes.ReadOnly Then
                Logger.WriteDebug(CallStack, "Unset read-only file attribute: " + rVector.HistoryFile)
                HistoryFileInfo.Attributes = HistoryFileInfo.Attributes Xor FileAttribute.ReadOnly
            End If
            Logger.WriteDebug(CallStack, "Write history file: " + rVector.HistoryFile)
            HistoryFileWriter = New System.IO.StreamWriter(rVector.HistoryFile, False)
            For Each HistoryLine In HistoryFileArray
                HistoryFileWriter.WriteLine(HistoryLine)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + rVector.HistoryFile)
            HistoryFileWriter.Close()

        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught rewriting history file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return
        End Try

        ' Normalize history file
        NormalizeHistoryFile(CallStack, rVector.HistoryFile)

        Return

    End Sub

    Public Shared Sub NormalizeHistoryFile(ByVal CallStack As String, ByVal HistoryFile As String)

        Dim HistoryFileReader As System.IO.StreamReader
        Dim HistoryFileWriter As System.IO.StreamWriter
        Dim HistoryFileCreateDate As Date
        Dim CurrentLine As String
        Dim PreviousLine As String
        Dim ForwardLine As String
        Dim strLine As String
        Dim HistoryFileArray As New ArrayList
        Dim BeginIndex As Integer = 0
        Dim EndIndex As Integer = 0
        Dim DelimCount As Integer = 0

        ' Read history file
        If System.IO.File.Exists(HistoryFile) Then
            Try
                HistoryFileCreateDate = System.IO.File.GetCreationTime(HistoryFile)
                Logger.WriteDebug(CallStack, "History file creation date: " + HistoryFileCreateDate.ToString("F"))
                Logger.WriteDebug(CallStack, "Read history file contents: " + HistoryFile)
                HistoryFileReader = New System.IO.StreamReader(HistoryFile)
                Do While HistoryFileReader.Peek() >= 0
                    strLine = HistoryFileReader.ReadLine()
                    HistoryFileArray.Add(strLine)
                Loop
                Logger.WriteDebug(CallStack, "Close file: " + HistoryFile)
                HistoryFileReader.Close()
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught reading history file.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return
            End Try
        End If

        ' Normalize history file
        Try

            ' Iterate history file, in reverse
            For i As Integer = HistoryFileArray.Count - 1 To 0 Step -1

                ' Read current line
                CurrentLine = HistoryFileArray.Item(i)

                ' Read previous line
                If i < HistoryFileArray.Count - 1 Then
                    PreviousLine = HistoryFileArray.Item(i + 1)
                Else
                    PreviousLine = "-1"
                End If

                ' Process current line
                If CurrentLine.Equals("") AndAlso PreviousLine.Equals("") Then
                    HistoryFileArray.RemoveAt(i)
                    Continue For

                ElseIf CurrentLine.ToLower.StartsWith("installedfile=") Then

                    ' Is the entry long enough?
                    If i - 8 < 0 Then
                        HistoryFileArray.RemoveAt(i)
                        Continue For
                    End If

                    ' Iterate up, stupid!
                    For j As Integer = i - 1 To 0 Step -1
                        ForwardLine = HistoryFileArray.Item(j)
                        If ForwardLine.Equals("") Then
                            HistoryFileArray.RemoveAt(j)
                            i -= 1
                            Continue For
                        ElseIf ForwardLine.ToLower.StartsWith("installedfile=") Then
                            Continue For
                        ElseIf ForwardLine.ToLower.StartsWith("supersede=") Then
                            If j - 8 >= 0 AndAlso
                                DirectCast(HistoryFileArray.Item(j - 1), String).ToLower.StartsWith("mcoreqs=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 2), String).ToLower.StartsWith("coreqs=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 3), String).ToLower.StartsWith("mprereqs=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 4), String).ToLower.StartsWith("prereqs=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 5), String).ToLower.StartsWith("component=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 6), String).ToLower.StartsWith("genlevel=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 7), String).ToLower.StartsWith("release=") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 8), String).StartsWith("[") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 8), String).Contains("]") AndAlso
                                DirectCast(HistoryFileArray.Item(j - 8), String).ToLower.Contains("installed") Then
                                If j - 9 >= 0 AndAlso Not DirectCast(HistoryFileArray.Item(j - 9), String).Equals("") Then
                                    HistoryFileArray.Insert(j - 9, "") ' Good stuff, keep it
                                End If
                                i = j - 9
                                Exit For
                            Else ' Sorry, you lose
                                For k As Integer = i To j Step -1 ' Walk i to j
                                    HistoryFileArray.RemoveAt(k) ' Remove each line
                                Next
                                i = j ' Move i up to j
                                Exit For ' Get out
                            End If
                        Else
                            For k As Integer = i To j Step -1 ' Walk i to j
                                HistoryFileArray.RemoveAt(k) ' Remove each line
                            Next
                            i = j ' Move i up to j
                            Exit For ' Get out
                        End If
                    Next
                    Continue For ' From the top

                ElseIf CurrentLine.ToLower.StartsWith("#") OrElse
                    CurrentLine.Equals("") OrElse
                    CurrentLine.ToLower.Contains("ptf wizard created this history file") Then
                    If CurrentLine.ToLower.Contains("ptf wizard created this history file") AndAlso PreviousLine.Equals("-1") Then
                        HistoryFileArray.Add(Environment.NewLine)
                    ElseIf CurrentLine.ToLower.Contains("ptf wizard created this history file") AndAlso i <> 0 Then
                        HistoryFileArray.RemoveAt(i)
                        Continue For
                    End If
                    Continue For

                Else
                    HistoryFileArray.RemoveAt(i)
                    Continue For
                End If

            Next

            ' Is the header present?
            If HistoryFileArray.Count = 0 OrElse Not DirectCast(HistoryFileArray.Item(0), String).ToLower.Contains("ptf wizard created this history file") Then
                HistoryFileArray.Insert(0, HistoryFileCreateDate.ToString("[ddd MMM d HH:mm:ss yyyy]") + " - PTF Wizard created this history file.")
            End If

            Logger.WriteDebug(CallStack, "History file normalized.")

        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught normalizing history file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return
        End Try

        ' Write history file
        Try
            Logger.WriteDebug(CallStack, "Rewrite history file: " + HistoryFile)
            HistoryFileWriter = New System.IO.StreamWriter(HistoryFile, False)
            For Each HistoryLine In HistoryFileArray
                HistoryFileWriter.WriteLine(HistoryLine)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + HistoryFile)
            HistoryFileWriter.Close()
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught rewriting history file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return
        End Try

    End Sub

End Class
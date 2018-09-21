Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Sub InitStartPanel()
        grpWinOfflineWelcome.Text = grpWinOfflineWelcome.Text.Replace("WinOffline", Globals.ProcessFriendlyName)
        grpHistory.Text = "Patch History for " + Globals.HostName
        BuildPatchHistoryTree()
    End Sub

    Private Sub BuildPatchHistoryTree()

        If Globals.ITCMInstalled = False Then
            Dim ComponentRootNode As New TreeNode("Client Automation")
            treHistory.Nodes.Add(ComponentRootNode)
            ComponentRootNode.Nodes.Add(New TreeNode("Not Installed."))
            ComponentRootNode.Expand()
            rbnApply.Enabled = False
            rbnBackout.Enabled = False
            rbnCLIHelp.Select()
            Exit Sub
        End If

        ReadHistoryFile("Client Automation", Globals.DSMFolder + Globals.HostName + ".his")
        ReadHistoryFile("Shared Components", Globals.SharedCompFolder + Globals.HostName + ".his")
        ReadHistoryFile("CA Message Queuing Service", Globals.CAMFolder + "\" + Globals.HostName + ".his")

        If Globals.SSAFolder IsNot Nothing Then
            ReadHistoryFile("Secure Socket Adapter", Globals.SSAFolder + Globals.HostName + ".his")
        End If
        If Globals.DTSFolder IsNot Nothing Then
            ReadHistoryFile("Data Transport Service", Globals.DTSFolder + "\" + Globals.HostName + ".his")
        End If
        If Globals.EGCFolder IsNot Nothing Then
            ReadHistoryFile("DSM Explorer GUI", Globals.EGCFolder + "\" + Globals.HostName + ".his")
        End If

        treHistory.Nodes(0).EnsureVisible()

    End Sub

    Private Sub ReadHistoryFile(ByVal Component As String, ByVal HistoryFile As String)

        Dim ComponentRootNode As New TreeNode(Component)
        Dim HistoryFileReader As System.IO.StreamReader
        Dim HistoryArray As New ArrayList
        Dim strLine As String
        Dim PatchName As String
        Dim TempFileList As New ArrayList
        Dim PatchArray As New ArrayList
        Dim InstalledFileArray As New ArrayList
        Dim PatchFileName As String
        Dim DuplicateIndex As Integer = 0
        Dim DuplicateArray As New ArrayList
        Dim DuplicateFileArray As New ArrayList
        Dim DuplicateRemoved As Boolean = False

        treHistory.Nodes.Add(ComponentRootNode)

        If System.IO.File.Exists(HistoryFile) Then
            Try
                HistoryFileReader = New System.IO.StreamReader(HistoryFile)

                Do While HistoryFileReader.Peek() >= 0
                    HistoryArray.Add(HistoryFileReader.ReadLine())
                Loop

                HistoryFileReader.Close()
            Catch ex As Exception
                Dim NewNode As New TreeNode("Failed to read history file.")
                ComponentRootNode.Nodes.Add(NewNode)
                ComponentRootNode.Expand()
            End Try

            If HistoryArray.Count > 0 Then
                For i As Integer = 0 To HistoryArray.Count - 1
                    strLine = HistoryArray.Item(i).ToString.ToLower
                    If strLine.ToLower.Contains("ptf wizard installed") Then
                        Try
                            PatchName = strLine.ToUpper.Substring(strLine.IndexOf("installed") + 10)
                            PatchName = PatchName.Substring(0, PatchName.IndexOf("(") - 1)
                            If PatchArray.Contains(PatchName) Then
                                DuplicateIndex = PatchArray.IndexOf(PatchName)
                                ' Add to duplicate array
                                ' Note: We need to track duplicates in the history file. In the
                                '       case where a patch is applied twice, but removed only
                                '       once, means it should still be a candidate for removal,
                                '       as long as the original files are available in REPLACED.
                                DuplicateArray.Add(PatchArray.Item(DuplicateIndex))
                                DuplicateFileArray.Add(DirectCast(InstalledFileArray.Item(DuplicateIndex), ArrayList).Clone)
                                PatchArray.RemoveAt(DuplicateIndex)
                                InstalledFileArray.RemoveAt(DuplicateIndex)
                            End If
                            PatchArray.Add(PatchName)
                        Catch ex As Exception
                            Continue For ' Unexpected format, skip the line
                        End Try

                        TempFileList = New ArrayList

                        ' Perform inner loop for installed files
                        For j As Integer = i + 1 To HistoryArray.Count - 1
                            strLine = HistoryArray.Item(j).ToString
                            If strLine.ToLower.Contains("installedfile=") Then
                                Try
                                    PatchFileName = strLine.Substring(strLine.IndexOf("=") + 1)
                                    PatchFileName = PatchFileName.Substring(0, PatchFileName.IndexOf(","))
                                    TempFileList.Add(PatchFileName)
                                Catch ex As Exception
                                    Continue For ' Unexpected format, skip it
                                End Try
                            End If
                            If strLine.ToLower.Contains("ptf wizard") Then
                                Exit For ' We've gone into the next patch, break inner loop
                            End If
                            i = j ' Move i up to j
                        Next
                        InstalledFileArray.Add(TempFileList)

                    ElseIf strLine.ToLower.Contains("backed out") Then
                        Try
                            PatchName = strLine.ToLower.Substring(strLine.IndexOf("backed out") + 11)
                            PatchName = PatchName.Substring(0, PatchName.IndexOf("(") - 1)
                            DuplicateRemoved = False
                            ' Iterate the duplicate array
                            ' Note: Check the duplicate array first. Example: If a patch was
                            '       applied twice, but removed once, the resulting history
                            '       should show one positive apply of the patch, which may
                            '       qualify for removal if the original files are avalable in
                            '       in REPLACED.
                            For k As Integer = 0 To DuplicateArray.Count - 1
                                If DuplicateArray.Item(k).ToString.ToLower.Equals(PatchName) Then
                                    DuplicateArray.RemoveAt(k)
                                    DuplicateFileArray.RemoveAt(k)
                                    DuplicateRemoved = True
                                    Exit For ' Exit loop -- Only one removal
                                End If
                            Next

                            If Not DuplicateRemoved Then
                                For k As Integer = 0 To PatchArray.Count - 1
                                    If PatchArray.Item(k).ToString.ToLower.Equals(PatchName) Then
                                        PatchArray.RemoveAt(k)
                                        InstalledFileArray.RemoveAt(k)
                                        k -= 1 ' Since removing elements, decrement k
                                        If k >= PatchArray.Count - 1 Then Exit For ' Check k for exit condition
                                    End If
                                Next
                            End If
                        Catch ex As Exception
                            Continue For
                        End Try
                    End If
                Next

                For i As Integer = 0 To PatchArray.Count - 1
                    Dim PatchNode As New TreeNode(PatchArray.Item(i))
                    TempFileList = InstalledFileArray.Item(i)
                    For Each InstalledFile As String In TempFileList
                        Dim FileNode As New TreeNode(InstalledFile)
                        PatchNode.Nodes.Add(FileNode)
                    Next
                    ComponentRootNode.Nodes.Add(PatchNode)
                Next
                ComponentRootNode.Expand()
            Else
                Dim NewNode As New TreeNode("No history available.")
                ComponentRootNode.Nodes.Add(NewNode)
                ComponentRootNode.Expand()
            End If
        Else
            Dim NewNode As New TreeNode("No history available.")
            ComponentRootNode.Nodes.Add(NewNode)
            ComponentRootNode.Expand()
        End If

        If ComponentRootNode.Nodes.Count = 0 Then
            Dim NewNode As New TreeNode("No history available.")
            ComponentRootNode.Nodes.Add(NewNode)
            ComponentRootNode.Expand()
        End If

    End Sub

End Class
Partial Public Class WinOffline

    Public Shared Function JobContainer(ByVal CallStack As String) As Integer

        Dim FileList As String() = Nothing
        Dim FolderList As String() = Nothing
        Dim NewestFileName As String = ""
        Dim TempFileDate As Date
        Dim NewestFileDate As Date
        Dim NewestFileReader As System.IO.StreamReader
        Dim ContainerArray As New ArrayList
        Dim strLine As String = ""
        Dim JobList As New ArrayList
        Dim OutputIDList As New ArrayList
        Dim OutputIDWriter As System.IO.StreamWriter
        Dim RunLevel As Integer = 0

        CallStack += "JobContainer|"

        ' Check cache for existing job output ID
        Try
            If System.IO.File.Exists(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid") And
                Globals.DirtyFlag = False Then
                Logger.WriteDebug(CallStack, "Open file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
                NewestFileReader = New System.IO.StreamReader(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
                Globals.JobOutputFolder = NewestFileReader.ReadLine
                Globals.CachedJobOutputID = NewestFileReader.ReadLine
                Logger.WriteDebug(CallStack, "Close file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
                NewestFileReader.Close()
                Logger.WriteDebug(CallStack, "Cached job output folder: " + Globals.JobOutputFolder)
                Logger.WriteDebug(CallStack, "Cached job output ID: " + Globals.CachedJobOutputID)
                Globals.JobOutputFile = Globals.JobOutputFolder + "\" + strLine + ".res"
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught reading cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 1
        End Try

        ' Obtain unit list based on process identity
        If Globals.RunningAsSystemIdentity Then
            Logger.WriteDebug(CallStack, "Computer account: " + Globals.DSMFolder + "Agent\units\00000001\usd\sdjexec")
            Globals.JobOutputFolder = Globals.DSMFolder + "Agent\units\00000001\usd\sdjexec"
            If Not System.IO.Directory.Exists(Globals.JobOutputFolder) Then
                Logger.WriteDebug(CallStack, "Error: Job output directory does not exist at expected path.")
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {"Error: Job output directory does not exist at expected path."})
                Return 2
            End If
            FileList = System.IO.Directory.GetFiles(Globals.JobOutputFolder)
        Else
            If Not System.IO.Directory.Exists(Globals.DSMFolder + "Agent\units\") Then
                Logger.WriteDebug(CallStack, "Error: Units directory does not exist at expected path.")
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {"Error: Units directory does not exist at expected path."})
                Return 3
            End If
            FolderList = System.IO.Directory.GetDirectories(Globals.DSMFolder + "Agent\units\")
            For Each unit As String In FolderList
                Logger.WriteDebug(CallStack, "Found unit: " + unit)
                If System.IO.Directory.Exists(unit + "\usd\sdjexec") Then
                    For Each file As String In System.IO.Directory.GetFiles(unit + "\usd\sdjexec")
                        Logger.WriteDebug(CallStack, "Add file: " + file)
                        Utility.AddToExistingArray(FileList, file)
                    Next
                End If
            Next
        End If

        ' Find the active container work file (newest .cwf)
        Try
            For n As Integer = 0 To FileList.Length - 1
                Logger.WriteDebug(CallStack, "File: " + FileList(n))
                If Not FileList(n).EndsWith(".cwf") Then
                    Continue For
                End If
                TempFileDate = System.IO.File.GetCreationTime(FileList(n))
                Logger.WriteDebug(CallStack, "Timestamp: " + TempFileDate.ToString)
                If NewestFileName = "" Then
                    NewestFileName = FileList(n)
                    NewestFileDate = TempFileDate
                    Logger.WriteDebug(CallStack, "Newest file assigned: " + NewestFileName)
                    Continue For
                End If
                If Date.Compare(TempFileDate, NewestFileDate) > 0 Then
                    NewestFileName = FileList(n)
                    NewestFileDate = TempFileDate
                    Logger.WriteDebug(CallStack, "Newest file assigned: " + NewestFileName)
                End If
            Next
            Globals.JobOutputFolder = NewestFileName.Substring(0, NewestFileName.LastIndexOf("\"))
            Logger.WriteDebug(CallStack, "Software delivery job output folder: " + Globals.JobOutputFolder)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught locating newest .cwf file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 4
        End Try

        ' Process active container work file (.cwf)
        Try
            Logger.WriteDebug(CallStack, "Open file: " + NewestFileName + Environment.NewLine)
            Logger.WriteDebug("############################################################")
            NewestFileReader = New System.IO.StreamReader(NewestFileName)
            Do While NewestFileReader.Peek() >= 0
                strLine = NewestFileReader.ReadLine()
                Logger.WriteDebug(strLine)
                ContainerArray.Add(strLine)
            Loop
            Logger.WriteDebug("############################################################" + Environment.NewLine)
            Logger.WriteDebug(CallStack, "Close file: " + NewestFileName)
            NewestFileReader.Close()

            ' Iterate container file array
            For i As Integer = 0 To ContainerArray.Count - 1
                strLine = ContainerArray.Item(i).ToString
                If strLine.ToLower.StartsWith("[job_") And strLine.ToLower.EndsWith("]") Then
                    Logger.WriteDebug(CallStack, "Found job header: " + strLine)
                    ' Perform inner loop for procedure name
                    For j As Integer = i + 1 To ContainerArray.Count - 1
                        strLine = ContainerArray.Item(j).ToString
                        If strLine.ToLower.Contains("proctorun = ") Then
                            Logger.WriteDebug(CallStack, "Found procedure property: " + strLine.Substring(strLine.IndexOf("=") + 2))
                            JobList.Add(strLine.Substring(strLine.IndexOf("=") + 2))
                            Exit For
                        End If
                    Next
                End If

                ' Check for a target header
                If strLine.ToLower.StartsWith("[target_") And strLine.ToLower.EndsWith("]") Then
                    Logger.WriteDebug(CallStack, "Found target header: " + strLine)
                    ' Perform inner loop for output ID tag
                    For j As Integer = i + 1 To ContainerArray.Count - 1
                        strLine = ContainerArray.Item(j).ToString
                        If strLine.ToLower.Contains("j_") And strLine.ToLower.Contains("=") Then
                            Logger.WriteDebug(CallStack, "Found output ID property: " + strLine.Substring(strLine.IndexOf("=") + 2))
                            OutputIDList.Add(strLine.Substring(strLine.IndexOf("=") + 2))
                        ElseIf strLine.Length = 0 Then
                            Exit For
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught processing .cwf file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 5
        End Try

        ' Process job list to find correct output file ID
        Try
            For x As Integer = 0 To JobList.Count - 1
                If JobList.Item(x).ToString.ToLower.Equals(Globals.ProcessShortName.ToLower) Then
                    If Not System.IO.File.Exists(Globals.JobOutputFolder + "\" + OutputIDList.Item(x) + ".res") And
                        Not System.IO.File.Exists(Globals.JobOutputFolder + "\" + OutputIDList.Item(x) + ".out") Then
                        Globals.CurrentJobOutputID = OutputIDList.Item(x).ToString
                        Globals.JobOutputFile = Globals.JobOutputFolder + "\" + OutputIDList.Item(x) + ".res"
                        Logger.WriteDebug(CallStack, "SD job output file: " + Globals.JobOutputFile)
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught determining correct job output ID.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 6
        End Try

        ' Parent process check
        If Not Globals.ParentProcessName.ToLower.Equals("sd_jexec") Then
            Logger.WriteDebug(CallStack, "Job output belongs to parent process.")
            Return 7
        End If

        ' Check for a dirty execution
        If Globals.CachedJobOutputID IsNot Nothing Then
            If Not Globals.CachedJobOutputID.Equals(Globals.CurrentJobOutputID) Then
                Logger.WriteDebug(CallStack, "Error: Current job output ID does not match cached ID.")
                RunLevel = Init.SDStageIReInit(CallStack)
                If RunLevel <> 0 Then
                    Manifest.UpdateManifest(CallStack,
                                            Manifest.EXCEPTION_MANIFEST,
                                            {"Warning: Software delivery job ID mismatch.",
                                            "Reason: An incomplete prior execution was detected and remediation failed."})
                Else
                    Manifest.UpdateManifest(CallStack,
                                            Manifest.EXCEPTION_MANIFEST,
                                            {"Warning: Software delivery job ID mismatch.",
                                            "Reason: An incomplete prior execution was detected and remediated."})
                End If
            Else
                Logger.WriteDebug(CallStack, "Current job output ID matches cached ID.")
            End If
        End If

        ' Write job output ID to cache file
        Try
            Utility.DeleteFile(CallStack, Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
            Logger.WriteDebug(CallStack, "Open file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
            OutputIDWriter = New System.IO.StreamWriter(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
            Logger.WriteDebug(CallStack, "Write: " + Globals.JobOutputFolder)
            Logger.WriteDebug(CallStack, "Write: " + Globals.CurrentJobOutputID)
            OutputIDWriter.WriteLine(Globals.JobOutputFolder)
            OutputIDWriter.WriteLine(Globals.CurrentJobOutputID)
            Logger.WriteDebug(CallStack, "Close file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")
            OutputIDWriter.Close()
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught writing job output ID to cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 8
        End Try

        Return RunLevel

    End Function

End Class
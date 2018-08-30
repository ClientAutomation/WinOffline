Partial Public Class WinOffline

    Public Shared Function JobContainer(ByVal CallStack As String) As Integer

        ' Local variables
        Dim FileList As String() = Nothing                      ' For reading a directory listing of files.
        Dim FolderList As String() = Nothing                    ' For reading a directory listing of folders.
        Dim NewestFileName As String = ""                       ' Newest file name in the directory.
        Dim TempFileDate As Date                                ' Timestamp of the comparison file.
        Dim NewestFileDate As Date                              ' Timestamp of the newest file.
        Dim NewestFileReader As System.IO.StreamReader          ' Input stream for reading container work file (newest file).
        Dim ContainerArray As New ArrayList                     ' Array for storing job container info.
        Dim strLine As String = ""                              ' String for parsing the container work file.
        Dim JobList As New ArrayList                            ' Array for parsing all container work file jobs.
        Dim OutputIDList As New ArrayList                       ' Array for parsing all job output IDs.
        Dim OutputIDWriter As System.IO.StreamWriter            ' Output stream for caching job output ID.
        Dim RunLevel As Integer = 0                             ' Tracks the health of the function and calls to external functions.

        ' Update call stack
        CallStack += "JobContainer|"

        ' *****************************
        '  - Check cache for existing job output ID.
        ' *****************************

        Try

            ' Check cache file for existing job output ID
            If System.IO.File.Exists(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid") And
                Globals.DirtyFlag = False Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Open file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

                ' Open cache file
                NewestFileReader = New System.IO.StreamReader(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

                ' Set job output folder
                Globals.JobOutputFolder = NewestFileReader.ReadLine

                ' Set cached ID
                Globals.CachedJobOutputID = NewestFileReader.ReadLine

                ' Write debug
                Logger.WriteDebug(CallStack, "Close file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

                ' Close the stream
                NewestFileReader.Close()

                ' Write debug
                Logger.WriteDebug(CallStack, "Cached job output folder: " + Globals.JobOutputFolder)
                Logger.WriteDebug(CallStack, "Cached job output ID: " + Globals.CachedJobOutputID)

                ' Assign our output ID
                Globals.JobOutputFile = Globals.JobOutputFolder + "\" + strLine + ".res"

            End If

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught reading cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)

            ' Create exception
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            ' Return
            Return 1

        End Try

        ' *****************************
        ' - Obtain unit list based on process identity.
        ' *****************************

        ' Check process identity
        If Globals.RunningAsSystemIdentity Then

            ' Write debug
            Logger.WriteDebug(CallStack, "Computer account: " + Globals.DSMFolder + "Agent\units\00000001\usd\sdjexec")

            ' Build automatic job output directory
            Globals.JobOutputFolder = Globals.DSMFolder + "Agent\units\00000001\usd\sdjexec"

            ' Verify folder exists
            If Not System.IO.Directory.Exists(Globals.JobOutputFolder) Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Job output directory does not exist at expected path.")

                ' Create exception
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {"Error: Job output directory does not exist at expected path."})

                ' Return
                Return 2

            End If

            ' Get the directory listing
            FileList = System.IO.Directory.GetFiles(Globals.JobOutputFolder)

        Else

            ' Verify folder exists
            If Not System.IO.Directory.Exists(Globals.DSMFolder + "Agent\units\") Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Units directory does not exist at expected path.")

                ' Create exception
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {"Error: Units directory does not exist at expected path."})

                ' Return
                Return 3

            End If

            ' Get the folder listing
            FolderList = System.IO.Directory.GetDirectories(Globals.DSMFolder + "Agent\units\")

            ' Iterate the unit list
            For Each unit As String In FolderList

                ' Write debug
                Logger.WriteDebug(CallStack, "Found unit: " + unit)

                ' Check for proper usd folder
                If System.IO.Directory.Exists(unit + "\usd\sdjexec") Then

                    ' Iterate the file list
                    For Each file As String In System.IO.Directory.GetFiles(unit + "\usd\sdjexec")

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Add file: " + file)

                        ' Add to Filelist
                        Utility.AddToExistingArray(FileList, file)

                    Next

                End If

            Next

        End If

        ' *****************************
        ' - Find the active container work file (.cwf).
        ' *****************************

        Try

            ' Loop the directory listing
            For n As Integer = 0 To FileList.Length - 1

                ' Write debug
                Logger.WriteDebug(CallStack, "File: " + FileList(n))

                ' Verify the file extension
                If Not FileList(n).EndsWith(".cwf") Then

                    ' Skip any non .cwf file
                    Continue For

                End If

                ' Get the file attributes
                TempFileDate = System.IO.File.GetCreationTime(FileList(n))

                ' Write debug
                Logger.WriteDebug(CallStack, "Timestamp: " + TempFileDate.ToString)

                ' Check if a newest File has been picked (Base case)
                If NewestFileName = "" Then

                    ' Assign temp file to new file
                    NewestFileName = FileList(n)
                    NewestFileDate = TempFileDate

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Newest file assigned: " + NewestFileName)

                    ' Skip remainder of loop
                    Continue For

                End If

                ' Check if temp file is newer
                If Date.Compare(TempFileDate, NewestFileDate) > 0 Then

                    ' Temp file is newer
                    NewestFileName = FileList(n)
                    NewestFileDate = TempFileDate

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Newest file assigned: " + NewestFileName)

                End If

            Next

            ' Parse job output directory
            Globals.JobOutputFolder = NewestFileName.Substring(0, NewestFileName.LastIndexOf("\"))

            ' Write debug
            Logger.WriteDebug(CallStack, "Software delivery job output folder: " + Globals.JobOutputFolder)

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught locating newest .cwf file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)

            ' Create exception
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            ' Return
            Return 4

        End Try

        ' *****************************
        ' - Process active container work file.
        ' *****************************

        Try

            ' Write debug
            Logger.WriteDebug(CallStack, "Open file: " + NewestFileName + Environment.NewLine)
            Logger.WriteDebug("############################################################")

            ' Open newest container file
            NewestFileReader = New System.IO.StreamReader(NewestFileName)

            ' Loop container file contents
            Do While NewestFileReader.Peek() >= 0

                ' Read a line
                strLine = NewestFileReader.ReadLine()

                ' Write debug -- write file contents only
                Logger.WriteDebug(strLine)

                ' Update the array
                ContainerArray.Add(strLine)

            Loop

            ' Write debug
            Logger.WriteDebug("############################################################" + Environment.NewLine)
            Logger.WriteDebug(CallStack, "Close file: " + NewestFileName)

            ' Close the stream
            NewestFileReader.Close()

            ' Iterate container file array
            For i As Integer = 0 To ContainerArray.Count - 1

                ' Get element
                strLine = ContainerArray.Item(i).ToString

                ' Check for a job header
                If strLine.ToLower.StartsWith("[job_") And strLine.ToLower.EndsWith("]") Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Found job header: " + strLine)

                    ' Perform inner loop for procedure name
                    For j As Integer = i + 1 To ContainerArray.Count - 1

                        ' Get next element
                        strLine = ContainerArray.Item(j).ToString

                        ' Check for procedure property
                        If strLine.ToLower.Contains("proctorun = ") Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, "Found procedure property: " + strLine.Substring(strLine.IndexOf("=") + 2))

                            ' Add to job list
                            JobList.Add(strLine.Substring(strLine.IndexOf("=") + 2))

                            ' Break inner loop to find more jobs
                            Exit For

                        End If

                    Next

                End If

                ' Check for a target header
                If strLine.ToLower.StartsWith("[target_") And strLine.ToLower.EndsWith("]") Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Found target header: " + strLine)

                    ' Perform inner loop for output ID tag
                    For j As Integer = i + 1 To ContainerArray.Count - 1

                        ' Get next element
                        strLine = ContainerArray.Item(j).ToString

                        ' Check for an output ID property
                        If strLine.ToLower.Contains("j_") And strLine.ToLower.Contains("=") Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, "Found output ID property: " + strLine.Substring(strLine.IndexOf("=") + 2))

                            ' Add output ID to list
                            OutputIDList.Add(strLine.Substring(strLine.IndexOf("=") + 2))

                        ElseIf strLine.Length = 0 Then

                            ' No more output IDs
                            Exit For

                        End If

                    Next

                End If

            Next

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught processing .cwf file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)

            ' Create exception
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            ' Return
            Return 5

        End Try

        ' *****************************
        ' - Process job list to find correct output file ID.
        ' *****************************

        Try

            ' Iterate the job list
            For x As Integer = 0 To JobList.Count - 1

                ' Check for a "WinOffline" job
                If JobList.Item(x).ToString.ToLower.Equals(Globals.ProcessShortName.ToLower) Then

                    ' Check if output already exists (Indicating this procedure already executed)
                    If Not System.IO.File.Exists(Globals.JobOutputFolder + "\" + OutputIDList.Item(x) + ".res") And
                        Not System.IO.File.Exists(Globals.JobOutputFolder + "\" + OutputIDList.Item(x) + ".out") Then

                        ' Assign our output ID
                        Globals.CurrentJobOutputID = OutputIDList.Item(x).ToString

                        ' Assign our output file
                        Globals.JobOutputFile = Globals.JobOutputFolder + "\" + OutputIDList.Item(x) + ".res"

                        ' Write debug
                        Logger.WriteDebug(CallStack, "SD job output file: " + Globals.JobOutputFile)

                        ' Exit for loop
                        Exit For

                    End If

                End If

            Next

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught determining correct job output ID.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)

            ' Create exception
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            ' Return
            Return 6

        End Try

        ' *****************************
        '  - Parent process check.
        ' *****************************

        ' Verify immediate parent is software delivery
        If Not Globals.ParentProcessName.ToLower.Equals("sd_jexec") Then

            ' Write debug
            Logger.WriteDebug(CallStack, "Job output belongs to parent process.")

            ' Return
            Return 7

        End If

        ' *****************************
        ' - Check for a dirty execution.
        ' *****************************

        ' Check if a cached ID was read
        If Globals.CachedJobOutputID IsNot Nothing Then

            ' Check for job ID mismatch
            If Not Globals.CachedJobOutputID.Equals(Globals.CurrentJobOutputID) Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Current job output ID does not match cached ID.")

                ' Reinitialize
                RunLevel = Init.SDStageIReInit(CallStack)

                ' Check the run level
                If RunLevel <> 0 Then

                    ' Create exception
                    Manifest.UpdateManifest(CallStack,
                                            Manifest.EXCEPTION_MANIFEST,
                                            {"Warning: Software delivery job ID mismatch.",
                                            "Reason: An incomplete prior execution was detected and remediation failed."})

                Else

                    ' Create exception
                    Manifest.UpdateManifest(CallStack,
                                            Manifest.EXCEPTION_MANIFEST,
                                            {"Warning: Software delivery job ID mismatch.",
                                            "Reason: An incomplete prior execution was detected and remediated."})

                End If

            Else

                ' Write debug
                Logger.WriteDebug(CallStack, "Current job output ID matches cached ID.")

            End If

        End If

        ' *****************************
        ' - Write job output ID to cache file.
        ' *****************************

        Try

            ' *****************************
            ' - Check for and remove a prior patch output ID cache.
            ' *****************************

            ' Delete existing output ID cache file
            Utility.DeleteFile(CallStack, Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

            ' *****************************
            ' - Write cache file.
            ' *****************************

            ' Write debug
            Logger.WriteDebug(CallStack, "Open file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

            ' Open cache file
            OutputIDWriter = New System.IO.StreamWriter(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

            ' Write debug
            Logger.WriteDebug(CallStack, "Write: " + Globals.JobOutputFolder)
            Logger.WriteDebug(CallStack, "Write: " + Globals.CurrentJobOutputID)

            ' Write job output ID
            OutputIDWriter.WriteLine(Globals.JobOutputFolder)
            OutputIDWriter.WriteLine(Globals.CurrentJobOutputID)

            ' Write debug
            Logger.WriteDebug(CallStack, "Close file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".outputid")

            ' Close the stream
            OutputIDWriter.Close()

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught writing job output ID to cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)

            ' Create exception
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            ' Return
            Return 8

        End Try

        ' Return
        Return RunLevel

    End Function

End Class
Partial Public Class WinOffline

    Public Shared Function PatchOperations(ByVal CallStack As String) As Integer

        Dim Runlevel As Integer = 0

        CallStack += "PatchOperations|"
        Logger.SetCurrentTask("Executing..")

        If Globals.RemovePatchSwitch Then

            ' Execute removal operations
            Try
                For i As Integer = 0 To Manifest.RemovalManifestCount - 1
                    Logger.WriteDebug(CallStack, "Read removal manifest index: [" + i.ToString() + "]")
                    Logger.WriteDebug(CallStack, "Remove patch: " + Manifest.GetRemovalFromManifest(i).RemovalItem)
                    If Globals.SimulatePatchSwitch Then
                        If Globals.SimulatePatchErrorSwitch Then
                            Logger.WriteDebug(CallStack, "Switch: Simulate patching error.")
                            Manifest.GetRemovalFromManifest(i).RemovalAction = RemovalVector.REMOVAL_FAIL
                            Manifest.GetRemovalFromManifest(i).CommentString = "Reason: This is a patch error simulation."
                        Else

                            Logger.WriteDebug(CallStack, "Switch: Simulate removing patch.")
                            Manifest.GetRemovalFromManifest(i).RemovalAction = RemovalVector.SKIPPED
                            Manifest.GetRemovalFromManifest(i).CommentString = "Reason: This is a simulation."
                        End If
                    Else
                        Runlevel = RemovePatch(CallStack, Manifest.GetRemovalFromManifest(i))
                        If Runlevel = 1 Then
                            Logger.WriteDebug(CallStack, "Result: SKIPPED.")
                            Manifest.GetRemovalFromManifest(i).RemovalAction = RemovalVector.SKIPPED
                        ElseIf Runlevel <> 0 Then
                            Logger.WriteDebug(CallStack, "Result: FAILED TO REMOVE.")
                            Manifest.GetRemovalFromManifest(i).RemovalAction = RemovalVector.REMOVAL_FAIL
                        Else
                            Logger.WriteDebug(CallStack, "Result: REMOVED.")
                            Manifest.GetRemovalFromManifest(i).RemovalAction = RemovalVector.REMOVAL_OK
                            RemoveFromHistory(CallStack, Manifest.GetRemovalFromManifest(i))
                        End If
                    End If
                Next
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught during removal operations.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 1
            End Try
        Else

            ' Execute apply operations
            Try
                For i As Integer = 0 To Manifest.PatchManifestCount - 1
                    Logger.WriteDebug(CallStack, "Read patch manifest index: [" + i.ToString() + "]")
                    Logger.WriteDebug(CallStack, "Patch file: " + Manifest.GetPatchFromManifest(i).PatchFile.GetFileName)
                    If Globals.SimulatePatchSwitch Then
                        If Globals.SimulatePatchErrorSwitch Then
                            Logger.WriteDebug(CallStack, "Switch: Simulate patching error.")
                            Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_FAIL
                            Manifest.GetPatchFromManifest(i).CommentString = "Reason: This is a patch error simulation."
                        Else
                            Logger.WriteDebug(CallStack, "Switch: Simulate applying patch.")
                            Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.SKIPPED
                            Manifest.GetPatchFromManifest(i).CommentString = "Reason: This is a simulation."
                        End If
                    ElseIf Not Manifest.GetPatchFromManifest(i).GetInstruction("VERSIONCHECK").Equals("") AndAlso
                        Not Manifest.GetPatchFromManifest(i).GetInstruction("VERSIONCHECK").Equals(Globals.ITCMComstoreVersion) Then
                        Logger.WriteDebug(CallStack, "Result: SKIPPED.")
                        Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.SKIPPED
                        Manifest.GetPatchFromManifest(i).CommentString = "Reason: Agent version does not match JCL specification.NEWLINE"
                        Manifest.GetPatchFromManifest(i).CommentString += "- Agent version (registry): " + Globals.ITCMVersion + "NEWLINE"
                        Manifest.GetPatchFromManifest(i).CommentString += "- Agent version (comstore): " + Globals.ITCMComstoreVersion + "  **USED FOR VERSION CHECK**" + "NEWLINE"
                        Manifest.GetPatchFromManifest(i).CommentString += "- JCL ""VERSIONCHECK:"" specification: " + Manifest.GetPatchFromManifest(i).GetInstruction("VERSIONCHECK").ToString
                    Else
                        Runlevel = ApplyPatch(CallStack, Manifest.GetPatchFromManifest(i))
                        If Runlevel = 1 Then
                            Logger.WriteDebug(CallStack, "Result: SKIPPED.")
                            Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.SKIPPED
                        ElseIf Runlevel = 2 Then
                            Logger.WriteDebug(CallStack, "Result: NOT APPLICABLE.")
                            Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.NOT_APPLICABLE
                        ElseIf Runlevel = 3 Then
                            Logger.WriteDebug(CallStack, "Result: ALREADY APPLIED.")
                            Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.ALREADY_APPLIED
                        ElseIf Runlevel <> 0 Then
                            If Manifest.GetPatchFromManifest(i).SourceReplaceList.Count > 0 Then
                                Logger.WriteDebug(CallStack, "Result: FAILED TO APPLY.")
                                Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_FAIL
                            Else
                                Logger.WriteDebug(CallStack, "Result: FAILED TO EXECUTE.")
                                Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.EXECUTE_FAIL
                            End If
                        Else
                            If Manifest.GetPatchFromManifest(i).SourceReplaceList.Count > 0 And
                                Manifest.GetPatchFromManifest(i).WereAllFileReplacementsSkipped Then
                                Logger.WriteDebug(CallStack, "Result: SKIPPED.")
                                Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.SKIPPED
                            ElseIf Manifest.GetPatchFromManifest(i).SourceReplaceList.Count > 0 Then
                                Logger.WriteDebug(CallStack, "Result: APPLIED.")
                                Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_OK
                                AddtoHistory(CallStack, Manifest.GetPatchFromManifest(i))
                            Else
                                Logger.WriteDebug(CallStack, "Result: EXECUTED.")
                                Manifest.GetPatchFromManifest(i).PatchAction = PatchVector.EXECUTE_OK
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught during apply operations.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 2
            End Try
        End If

        ' Write cache files
        Try
            Manifest.WriteCache(CallStack, Manifest.PATCH_MANIFEST)
            Manifest.WriteCache(CallStack, Manifest.REMOVAL_MANIFEST)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught writing cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Check for recycle CAF service only mode
        If Manifest.PatchManifestCount = 0 And Manifest.RemovalManifestCount = 0 Then
            Logger.WriteDebug(CallStack, "No patch operations to perform.")
        End If

        Return 0

    End Function

    Public Shared Sub ExecutePreCmd(ByVal CallStack As String, ByRef pVector As PatchVector)

        Dim ExecutionString As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String
        Dim RemainingOutput As String

        ' Run PRESYSCMD sripts
        For Each strLine As String In pVector.GetPreCommandList
            ExecutionString = pVector.PatchFile.GetFilePath + "\" + strLine
            Logger.WriteDebug(CallStack, "Execute pre-script: " + ExecutionString)

            ProcessStartInfo = New ProcessStartInfo(ExecutionString)
            ProcessStartInfo.WorkingDirectory = pVector.PatchFile.GetFilePath
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True
            StandardOutput = ""
            RemainingOutput = ""
            Logger.WriteDebug("------------------------------------------------------------")

            RunningProcess = Process.Start(ProcessStartInfo)

            While RunningProcess.HasExited = False
                ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                Logger.WriteDebug(ConsoleOutput)
                StandardOutput += ConsoleOutput + Environment.NewLine
                Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
            End While

            RunningProcess.WaitForExit()
            RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
            StandardOutput += RemainingOutput

            Logger.WriteDebug(RemainingOutput)
            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)

            pVector.PreCmdReturnCodes.Add(RunningProcess.ExitCode.ToString)
            RunningProcess.Close()
        Next

    End Sub

    Public Shared Sub ExecutePostCmd(ByVal CallStack As String, ByRef pVector As PatchVector)

        Dim ExecutionString As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String
        Dim RemainingOutput As String

        ' Run SYSCMD sripts
        For Each strLine As String In pVector.GetSysCommandList
            ExecutionString = pVector.PatchFile.GetFilePath + "\" + strLine
            Logger.WriteDebug(CallStack, "Execute script: " + ExecutionString)

            ProcessStartInfo = New ProcessStartInfo(ExecutionString)
            ProcessStartInfo.WorkingDirectory = pVector.PatchFile.GetFilePath
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True
            StandardOutput = ""
            RemainingOutput = ""
            Logger.WriteDebug("------------------------------------------------------------")

            RunningProcess = Process.Start(ProcessStartInfo)

            While RunningProcess.HasExited = False
                ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                Logger.WriteDebug(ConsoleOutput)
                StandardOutput += ConsoleOutput + Environment.NewLine
                Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
            End While

            RunningProcess.WaitForExit()
            RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
            StandardOutput += RemainingOutput

            Logger.WriteDebug(RemainingOutput)
            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)

            pVector.SysCmdReturnCodes.Add(RunningProcess.ExitCode.ToString)
            RunningProcess.Close()
        Next

    End Sub

    Public Shared Function ApplyPatch(ByVal CallStack As String, ByRef pVector As PatchVector) As Integer

        Dim AlreadyAppliedList As New List(Of Boolean)
        Dim ReplacedIncrement As Integer = 0
        Dim DestinationFileName As String = ""
        Dim ReplacedFolder As String = ""
        Dim NewFileList As New ArrayList
        Dim Subfolder As String = ""
        Dim RebootFileName As String = ""
        Dim RebootIncrement As Integer = 0

        CallStack += "ApplyPatch|"

        ' Validate patch component is installed
        If (pVector.IsClientAuto AndAlso Globals.DSMFolder Is Nothing) Or
            (pVector.IsSharedComponent AndAlso Globals.SharedCompFolder Is Nothing) Or
            (pVector.IsCAM AndAlso Globals.CAMFolder Is Nothing) Or
            (pVector.IsSSA AndAlso Globals.SSAFolder Is Nothing) Or
            (pVector.IsDataTransport AndAlso Globals.DTSFolder Is Nothing) Or
            (pVector.IsExplorerGUI AndAlso Globals.EGCFolder Is Nothing) Then
            Logger.WriteDebug(CallStack, "Patch skipped: " + pVector.PatchFile.GetFriendlyName)
            Logger.WriteDebug(CallStack, "Reason: Product component (" + pVector.GetInstruction("PRODUCT") + ") is not installed.")
            pVector.CommentString = "Reason: Product component (" + pVector.GetInstruction("PRODUCT") + ") is not installed."
            Return 1
        End If

        ' Check if patch if already applied (file replacement check)
        If pVector.SourceReplaceList.Count > 0 Then
            Logger.WriteDebug(CallStack, "Check if patch is already applied..")

            For x As Integer = 0 To pVector.SourceReplaceList.Count - 1
                Logger.WriteDebug(CallStack, "Compare: " + pVector.SourceReplaceList.Item(x).ToString)
                Logger.WriteDebug(CallStack, "Compare: " + pVector.DestReplaceList.Item(x).ToString)
                If Not Utility.IsFileEqual(pVector.SourceReplaceList.Item(x).ToString, pVector.DestReplaceList.Item(x).ToString) Then
                    If Not System.IO.File.Exists(pVector.DestReplaceList.Item(x).ToString) AndAlso
                        pVector.SkipIfNotFoundList.Contains(FileVector.GetShortName(pVector.DestReplaceList.Item(x).ToString.ToLower)) Then
                        Logger.WriteDebug(CallStack, "Result: Mismatch [Skip file]") ' Skipped file -- no applicability information
                    ElseIf Not System.IO.File.Exists(pVector.DestReplaceList.Item(x).ToString) AndAlso
                        Not pVector.SkipIfNotFoundList.Contains(FileVector.GetShortName(pVector.DestReplaceList.Item(x).ToString.ToLower)) Then
                        Logger.WriteDebug(CallStack, "Result: Mismatch [New file]")
                        AlreadyAppliedList.Add(False) ' New file -- not applied
                    Else
                        Logger.WriteDebug(CallStack, "Result: Mismatch")
                        AlreadyAppliedList.Add(False)
                    End If
                Else
                    Logger.WriteDebug(CallStack, "Result: Match")
                    AlreadyAppliedList.Add(True)
                End If
            Next

            If AlreadyAppliedList.Count = 0 Then
                Logger.WriteDebug(CallStack, "Patch is not applicable.")
                pVector.CommentString = "Reason: All patch files will be skipped."
                Return 2
            ElseIf Not AlreadyAppliedList.Contains(False) Then
                Logger.WriteDebug(CallStack, "Patch is already applied.")
                pVector.CommentString = "Reason: All patch files match with destination files."
                Return 3
            Else
                Logger.WriteDebug(CallStack, "Patch is not already applied.")
            End If

        Else
            Logger.WriteDebug(CallStack, "Patch is not already applied.")
        End If

        ' Run PRESYSCMD sripts
        ExecutePreCmd(CallStack, pVector)

        ' Prepare REPLACED subfolder
        If pVector.SourceReplaceList.Count > 0 Then
            If System.IO.Directory.Exists(pVector.ReplaceFolder + ".OLD") Then
                While True ' Loop dangerously for first available subfolder
                    If System.IO.Directory.Exists(pVector.ReplaceFolder + "-" + ReplacedIncrement.ToString + ".OLD") Then
                        ReplacedIncrement += 1
                    Else
                        Try
                            Logger.WriteDebug(CallStack, "Create REPLACED subfolder: " + pVector.ReplaceFolder + "-" + ReplacedIncrement.ToString + ".OLD")
                            ReplacedFolder = pVector.ReplaceFolder + "-" + ReplacedIncrement.ToString + ".OLD"
                            System.IO.Directory.CreateDirectory(ReplacedFolder)
                            Exit While
                        Catch ex As Exception
                            Logger.WriteDebug(CallStack, "Error: Failed to create REPLACED subfolder.")
                            Logger.WriteDebug(ex.Message)
                            Logger.WriteDebug(ex.StackTrace)
                            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                            Logger.WriteDebug(CallStack, "Patch skipped: " + pVector.PatchFile.GetFriendlyName)
                            Logger.WriteDebug(CallStack, "Reason: Failed to create REPLACED subfolder.")
                            pVector.CommentString = "Reason: Failed to create REPLACED subfolder."
                            Return 4
                        End Try
                    End If
                End While
            Else
                Try
                    Logger.WriteDebug(CallStack, "Create REPLACED subfolder: " + pVector.ReplaceFolder + ".OLD")
                    ReplacedFolder = pVector.ReplaceFolder + ".OLD"
                    System.IO.Directory.CreateDirectory(ReplacedFolder)
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Error: Failed to create REPLACED subfolder.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                    Logger.WriteDebug(CallStack, "Patch skipped: " + pVector.PatchFile.GetFriendlyName)
                    Logger.WriteDebug(CallStack, "Reason: Failed to create REPLACED subfolder.")
                    pVector.CommentString = "Reason: Failed to create REPLACED subfolder."
                    Return 5
                End Try
            End If
        End If

        ' Save original files to REPLACED folder
        For x As Integer = 0 To pVector.DestReplaceList.Count - 1
            If System.IO.File.Exists(pVector.DestReplaceList.Item(x)) Then
                DestinationFileName = ReplacedFolder + "\" + pVector.ReplaceSubFolder.Item(x) + "\" + FileVector.GetShortName(pVector.DestReplaceList.Item(x))
                DestinationFileName = DestinationFileName.Replace("\\", "\")
                If Not System.IO.Directory.Exists(FileVector.GetFilePath(DestinationFileName)) Then
                    Logger.WriteDebug(CallStack, "Create subfolder: " + FileVector.GetFilePath(DestinationFileName))
                    System.IO.Directory.CreateDirectory(FileVector.GetFilePath(DestinationFileName))
                End If
                Try
                    Logger.WriteDebug(CallStack, "Save original file: " + pVector.DestReplaceList.Item(x))
                    Logger.WriteDebug(CallStack, "Save to: " + DestinationFileName)
                    System.IO.File.Copy(pVector.DestReplaceList.Item(x), DestinationFileName)
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Error: Failed to save original file(s).")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                    Logger.WriteDebug(CallStack, "Delete REPLACED folder: " + pVector.ReplaceFolder)
                    System.IO.Directory.Delete(ReplacedFolder, True)
                    pVector.FileReplaceResult.Item(x) = PatchVector.FILE_FAILED
                    Logger.WriteDebug(CallStack, "Patch failed: " + pVector.PatchFile.GetFriendlyName)
                    Logger.WriteDebug(CallStack, "Reason: Failed to save original file(s).")
                    pVector.CommentString = "Reason: Failed to save original file(s)."
                    Return 6
                End Try
            Else
                ' Add to new file list (We may need to skip replacing these new files later)
                NewFileList.Add(FileVector.GetShortName(pVector.DestReplaceList.Item(x).ToString.ToLower))
            End If
        Next

        ' Remove original files before replacement
        For x As Integer = 0 To pVector.DestReplaceList.Count - 1
            If System.IO.File.Exists(pVector.DestReplaceList.Item(x)) Then
                Try
                    If Utility.IsFileOpen(pVector.DestReplaceList.Item(x)) Then
                        Logger.WriteDebug(CallStack, "Delete original file (on next reboot): " + pVector.DestReplaceList.Item(x))
                        RebootFileName = pVector.DestReplaceList.Item(x) + ".delete_on_reboot"
                        RebootIncrement = 0
                        While True ' Loop dangerously for available filename
                            If System.IO.File.Exists(RebootFileName) Then
                                RebootFileName = pVector.DestReplaceList.Item(x) + ".delete_on_reboot" + RebootIncrement.ToString
                                RebootIncrement += 1
                            Else
                                Exit While ' Stop condition
                            End If
                        End While
                        System.IO.File.Move(pVector.DestReplaceList.Item(x), RebootFileName)
                        WindowsAPI.MoveFileEx(RebootFileName, Nothing, WindowsAPI.MoveFileFlags.DelayUntilReboot)
                        pVector.FileReplaceResult.Item(x) = PatchVector.FILE_REBOOT_REQUIRED
                    Else
                        Logger.WriteDebug(CallStack, "Delete original file: " + pVector.DestReplaceList.Item(x))
                        System.IO.File.Delete(pVector.DestReplaceList.Item(x))
                    End If
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Error: Failed to delete original file(s), or schedule their removal.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                    If x > 0 Then ' Undo changes
                        For y As Integer = 0 To pVector.DestReplaceList.Count - 1
                            DestinationFileName = ReplacedFolder + "\" + pVector.ReplaceSubFolder.Item(y) + "\" + FileVector.GetShortName(pVector.DestReplaceList.Item(y))
                            DestinationFileName = DestinationFileName.Replace("\\", "\")
                            ' Verify the file exists
                            ' Note: In the case where the patch file is a new file, then nothing was
                            '       backed up to the REPLACED folder, to restore to the original path.
                            If System.IO.File.Exists(DestinationFileName) Then
                                Logger.WriteDebug(CallStack, "Restore original file: " + DestinationFileName)
                                Logger.WriteDebug(CallStack, "To: " + pVector.DestReplaceList.Item(y))
                                System.IO.File.Copy(DestinationFileName, pVector.DestReplaceList.Item(y), True)
                            End If
                        Next
                    End If
                    Logger.WriteDebug(CallStack, "Delete REPLACED folder: " + pVector.ReplaceFolder)
                    System.IO.Directory.Delete(ReplacedFolder, True)
                    pVector.FileReplaceResult.Item(x) = PatchVector.FILE_FAILED
                    Logger.WriteDebug(CallStack, "Patch failed: " + pVector.PatchFile.GetFriendlyName)
                    Logger.WriteDebug(CallStack, "Reason: Failed to delete original file(s), or schedule their removal.")
                    pVector.CommentString = "Reason: Failed to delete original file(s), or schedule their removal."
                    Return 7
                End Try
            End If
        Next

        ' Copy in replacement files
        For x As Integer = 0 To pVector.SourceReplaceList.Count - 1
            If pVector.SkipIfNotFoundList.Contains(FileVector.GetShortName(pVector.DestReplaceList.Item(x).ToString.ToLower)) And
                NewFileList.Contains(FileVector.GetShortName(pVector.DestReplaceList.Item(x).ToString.ToLower)) Then
                Logger.WriteDebug(CallStack, "Skip replacement file: " + pVector.SourceReplaceList.Item(x))
                pVector.FileReplaceResult.Item(x) = PatchVector.SKIPPED
                Continue For
            End If
            Logger.WriteDebug(CallStack, "Copy replacement file: " + pVector.SourceReplaceList.Item(x))
            Logger.WriteDebug(CallStack, "To: " + pVector.DestReplaceList.Item(x))
            Try
                System.IO.File.Copy(pVector.SourceReplaceList.Item(x), pVector.DestReplaceList.Item(x), True)
                If Not pVector.FileReplaceResult.Item(x) = PatchVector.FILE_REBOOT_REQUIRED Then
                    pVector.FileReplaceResult.Item(x) = PatchVector.FILE_OK
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Failed to copy replacement file(s) to destination.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                If x > 0 Then ' Undo changes
                    For y As Integer = 0 To pVector.DestReplaceList.Count - 1
                        Logger.WriteDebug(CallStack, "Delete replacement file: " + pVector.DestReplaceList.Item(y))
                        System.IO.File.Delete(pVector.DestReplaceList.Item(y))
                        DestinationFileName = ReplacedFolder + "\" + pVector.ReplaceSubFolder.Item(y) + "\" + FileVector.GetShortName(pVector.DestReplaceList.Item(y))
                        DestinationFileName = DestinationFileName.Replace("\\", "\")
                        ' Verify the file exists
                        ' Note: In the case where the patch file is a new file, then nothing was
                        '       backed up to the REPLACED folder, to restore to the original path.
                        If System.IO.File.Exists(DestinationFileName) Then
                            Logger.WriteDebug(CallStack, "Restore original file: " + DestinationFileName)
                            Logger.WriteDebug(CallStack, "To: " + pVector.DestReplaceList.Item(y))
                            System.IO.File.Copy(DestinationFileName, pVector.DestReplaceList.Item(y), True)
                        End If
                    Next
                End If
                Logger.WriteDebug(CallStack, "Delete REPLACED subfolder: " + pVector.ReplaceFolder)
                System.IO.Directory.Delete(ReplacedFolder, True)
                pVector.FileReplaceResult.Item(x) = PatchVector.FILE_FAILED
                Logger.WriteDebug(CallStack, "Patch failed: " + pVector.PatchFile.GetFriendlyName)
                Logger.WriteDebug(CallStack, "Reason: Failed to copy replacement file(s) to destination.")
                pVector.CommentString = "Reason: Failed to copy replacement file(s) to destination."
                Return 8
            End Try
        Next

        ' Special case: All file replacements skipped
        If pVector.SourceReplaceList.Count > 0 And pVector.WereAllFileReplacementsSkipped Then
            pVector.CommentString = "Reason: All patch file replacements skipped."
            Logger.WriteDebug(CallStack, "Delete REPLACED subfolder: " + pVector.ReplaceFolder)
            System.IO.Directory.Delete(ReplacedFolder, True)
        End If

        ' Run SYSCMD sripts
        ExecutePostCmd(CallStack, pVector)

        Return 0

    End Function

    Public Shared Function RemovePatch(ByVal CallStack As String, ByRef rVector As RemovalVector) As Integer

        Dim hVector As HistoryVector = Nothing
        Dim HistoryVector As HistoryVector = Nothing
        Dim RemovalMatchFound As Boolean = False
        Dim OriginalFilesFound As Boolean = False
        Dim ReplacedFolder As String = ""
        Dim ReplacedBaseFolder As String = ""
        Dim LatestReplacedFolder As String = ""
        Dim ReplacedIncrement As Integer = 0
        Dim SubFolder As String = ""
        Dim SubFolderList As New ArrayList
        Dim NewFileList As New ArrayList
        Dim RestoreList As New ArrayList
        Dim DestinationFolder As String
        Dim DestinationIncrement As Integer = 0
        Dim DestinationFileName As String
        Dim RebootFileName As String = ""
        Dim RebootIncrement As Integer = 0

        CallStack += "RemovePatch|"

        ' Search patch history
        For i As Integer = 0 To Manifest.HistoryManifestCount - 1
            HistoryVector = Manifest.GetHistoryFromManifest(i)
            If HistoryVector.GetPatchName.ToLower.Equals(rVector.RemovalItem.ToLower) Then
                RemovalMatchFound = True
                hVector = HistoryVector
                ' Don't exit the loop premature.
                ' Note: The idea is to remove the "latest" application of the patch. It's possible it
                '       could have been applied more than once, and it's also possible the list of
                '       installed files may not be the same (i.e. the details of the patch have changed).
                '       Since we want to remove history from the bottom, up, we should pop the latest
                '       hVector/history file record that matches the title of the patch.
                ' Exit For
            End If
        Next

        If RemovalMatchFound = False Then
            Logger.WriteDebug(CallStack, "Removal skipped: " + rVector.RemovalItem)
            Logger.WriteDebug(CallStack, "Reason: Patch is not found in history file(s).")
            rVector.CommentString = "Reason: Patch is not found in history file(s)."
            Return 1
        End If

        ' Find latest REPLACED subfolder
        If hVector.GetProductComponent.Equals(IT_CLIENT_MANAGER) Then
            ReplacedBaseFolder = Globals.DSMFolder + "REPLACED"
            ReplacedFolder = Globals.DSMFolder + "REPLACED\" + hVector.GetPatchName
            rVector.HistoryFile = Globals.DSMFolder + Globals.HostName + ".his"
        ElseIf hVector.GetProductComponent.Equals(SHARED_COMPONENTS) Then
            ReplacedBaseFolder = Globals.SharedCompFolder + "REPLACED"
            ReplacedFolder = Globals.SharedCompFolder + "REPLACED\" + hVector.GetPatchName
            rVector.HistoryFile = Globals.SharedCompFolder + Globals.HostName + ".his"
        ElseIf hVector.GetProductComponent.Equals(CA_MESSAGE_QUEUING) Then
            ReplacedBaseFolder = Globals.CAMFolder + "\REPLACED"
            ReplacedFolder = Globals.CAMFolder + "\REPLACED\" + hVector.GetPatchName
            rVector.HistoryFile = Globals.CAMFolder + "\" + Globals.HostName + ".his"
        ElseIf hVector.GetProductComponent.Equals(SECURE_SOCKET_ADAPATER) Then
            ReplacedBaseFolder = Globals.SSAFolder + "REPLACED"
            ReplacedFolder = Globals.SSAFolder + "REPLACED\" + hVector.GetPatchName
            rVector.HistoryFile = Globals.SSAFolder + Globals.HostName + ".his"
        ElseIf hVector.GetProductComponent.Equals(DATA_TRANSPORT) Then
            ReplacedBaseFolder = Globals.DTSFolder + "\REPLACED\"
            ReplacedFolder = Globals.DTSFolder + "\REPLACED\" + hVector.GetPatchName
            rVector.HistoryFile = Globals.DTSFolder + "\" + Globals.HostName + ".his"
        ElseIf hVector.GetProductComponent.Equals(EXPLORER_GUI) Then
            ReplacedBaseFolder = Globals.EGCFolder + "REPLACED"
            ReplacedFolder = Globals.EGCFolder + "REPLACED\" + hVector.GetPatchName
            rVector.HistoryFile = Globals.EGCFolder + Globals.HostName + ".his"
        End If
        LatestReplacedFolder = ReplacedFolder + ".OLD"

        If System.IO.Directory.Exists(ReplacedFolder + ".OLD") Then
            While True ' Loop dangerously for latest increment REPLACED folder
                If System.IO.Directory.Exists(ReplacedFolder + "-" + ReplacedIncrement.ToString + ".OLD") Then
                    LatestReplacedFolder = ReplacedFolder + "-" + ReplacedIncrement.ToString + ".OLD"
                    ReplacedIncrement += 1
                Else
                    Exit While
                End If
            End While
            ReplacedFolder = LatestReplacedFolder
        Else
            Logger.WriteDebug(CallStack, "Removal failed: " + rVector.RemovalItem)
            Logger.WriteDebug(CallStack, "Reason: Original files unavailable for restoration.")
            rVector.CommentString = "Reason: Original files unavailable for restoration."
            Return 2
        End If

        ' Generate NEW FILE list for removal
        ' Note: Per testing with ApplyPTF, any "INSTALLEDFILE=" specified from the history
        '       file, that does not have a matching original file within its REPLACED folder,
        '       is to be considered a NEW FILE introduced by the application of the patch.
        '       Therefore, on removal of said patch, NEW FILES will simply be deleted, but
        '       also saved in the BACKOUT subfolder.
        '
        '       Maintaining a NewFile list is not really necessary, as lower sections of
        '       code automatically save them to the BACKOUT subsolder and delete them
        '       where they stand. But I'm leaving this code here for now, in case this
        '       functionality is changed in the future.
        For Each InstalledFile As String In hVector.GetInstalledFiles
            If hVector.GetProductComponent.Equals(IT_CLIENT_MANAGER) Then
                SubFolder = InstalledFile.ToLower.Replace(Globals.DSMFolder.ToLower, "")
                SubFolder = SubFolder.Replace("\\", "\")
            ElseIf hVector.GetProductComponent.Equals(SHARED_COMPONENTS) Then
                SubFolder = InstalledFile.ToLower.Replace(Globals.SharedCompFolder.ToLower, "")
                SubFolder = SubFolder.Replace("\\", "\")
            ElseIf hVector.GetProductComponent.Equals(CA_MESSAGE_QUEUING) Then
                SubFolder = InstalledFile.ToLower.Replace(Globals.CAMFolder.ToLower, "")
                SubFolder = SubFolder.Replace("\\", "\")
            ElseIf hVector.GetProductComponent.Equals(SECURE_SOCKET_ADAPATER) Then
                SubFolder = InstalledFile.ToLower.Replace(Globals.SSAFolder.ToLower, "")
                SubFolder = SubFolder.Replace("\\", "\")
            ElseIf hVector.GetProductComponent.Equals(DATA_TRANSPORT) Then
                SubFolder = InstalledFile.ToLower.Replace(Globals.DTSFolder.ToLower, "")
                SubFolder = SubFolder.Replace("\\", "\")
            ElseIf hVector.GetProductComponent.Equals(EXPLORER_GUI) Then
                SubFolder = InstalledFile.ToLower.Replace(Globals.EGCFolder.ToLower, "")
                SubFolder = SubFolder.Replace("\\", "\")
            End If

            If System.IO.File.Exists(ReplacedFolder + "\" + SubFolder) Then
                OriginalFilesFound = True
                RestoreList.Add(ReplacedFolder + "\" + SubFolder)
                SubFolderList.Add(SubFolder) ' Add subfolder to list (so we don't have to recalculate this later)
            Else
                NewFileList.Add(InstalledFile)
                SubFolderList.Add(SubFolder) ' Add subfolder to list (so we don't have to recalculate this later)
            End If
        Next

        ' Prepare BACKOUT subfolder
        DestinationFolder = ReplacedBaseFolder + "\BACKOUT.OLD\" + hVector.GetPatchName
        If System.IO.Directory.Exists(DestinationFolder + ".OLD") Then
            While True ' Loop dangerously for latest increment destination folder
                If System.IO.Directory.Exists(DestinationFolder + "-" + DestinationIncrement.ToString + ".OLD") Then
                    DestinationIncrement += 1
                Else
                    Try
                        Logger.WriteDebug(CallStack, "Create BACKOUT subfolder: " + DestinationFolder + "-" + DestinationIncrement.ToString + ".OLD")
                        DestinationFolder = DestinationFolder + "-" + DestinationIncrement.ToString + ".OLD"
                        System.IO.Directory.CreateDirectory(DestinationFolder)
                        Exit While
                    Catch ex As Exception
                        Logger.WriteDebug(CallStack, "Removal failed: " + rVector.RemovalItem)
                        Logger.WriteDebug(CallStack, "Error: Failed to create BACKOUT subfolder.")
                        Logger.WriteDebug(ex.Message)
                        Logger.WriteDebug(ex.StackTrace)
                        Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                        rVector.CommentString = "Reason: Failed to create BACKOUT subfolder."
                        Return 3
                    End Try
                    Exit While
                End If
            End While
        Else
            Try
                Logger.WriteDebug(CallStack, "Create BACKOUT subfolder: " + DestinationFolder + ".OLD")
                DestinationFolder = DestinationFolder + ".OLD"
                System.IO.Directory.CreateDirectory(DestinationFolder)
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Removal failed: " + rVector.RemovalItem)
                Logger.WriteDebug(CallStack, "Error: Failed to create BACKOUT subfolder.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                rVector.CommentString = "Reason: Failed to create BACKOUT subfolder."
                Return 4
            End Try
        End If

        ' Save current files to BACKOUT folder
        For x As Integer = 0 To hVector.GetInstalledFiles.Count - 1
            If System.IO.File.Exists(hVector.GetInstalledFiles.Item(x)) Then
                DestinationFileName = DestinationFolder + "\" + SubFolderList.Item(x)
                DestinationFileName = DestinationFileName.Replace("\\", "\")
                If Not System.IO.Directory.Exists(FileVector.GetFilePath(DestinationFileName)) Then
                    Logger.WriteDebug(CallStack, "Create subfolder: " + FileVector.GetFilePath(DestinationFileName))
                    System.IO.Directory.CreateDirectory(FileVector.GetFilePath(DestinationFileName))
                End If
                Try
                    Logger.WriteDebug(CallStack, "Save current file: " + hVector.GetInstalledFiles.Item(x))
                    Logger.WriteDebug(CallStack, "Save to: " + DestinationFileName)
                    System.IO.File.Copy(hVector.GetInstalledFiles.Item(x), DestinationFileName, True)
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Error: Failed to save current file(s).")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                    Logger.WriteDebug(CallStack, "Delete BACKOUT folder: " + DestinationFolder)
                    System.IO.Directory.Delete(DestinationFolder, True)
                    rVector.RemovalFileName.Add(hVector.GetInstalledFiles.Item(x))
                    rVector.FileRemovalResult.Add(RemovalVector.FILE_FAILED)
                    Logger.WriteDebug(CallStack, "Removal failed: " + rVector.RemovalItem)
                    rVector.CommentString = "Reason: Failed to save current file(s)."
                    Return 5
                End Try
            End If
        Next

        ' Remove current files before replacement
        For x As Integer = 0 To hVector.GetInstalledFiles.Count - 1
            If System.IO.File.Exists(hVector.GetInstalledFiles.Item(x)) Then
                Try
                    If Utility.IsFileOpen(hVector.GetInstalledFiles.Item(x)) Then
                        Logger.WriteDebug(CallStack, "Delete original file (on next reboot): " + hVector.GetInstalledFiles.Item(x))
                        RebootFileName = hVector.GetInstalledFiles.Item(x) + ".delete_on_reboot"
                        RebootIncrement = 0
                        While True ' Loop dangerously for available filename
                            If System.IO.File.Exists(RebootFileName) Then
                                RebootFileName = hVector.GetInstalledFiles.Item(x) + ".delete_on_reboot" + RebootIncrement.ToString
                                RebootIncrement += 1
                            Else
                                Exit While
                            End If
                        End While
                        System.IO.File.Move(hVector.GetInstalledFiles.Item(x), RebootFileName)
                        WindowsAPI.MoveFileEx(RebootFileName, Nothing, WindowsAPI.MoveFileFlags.DelayUntilReboot)
                        rVector.RemovalFileName.Add(hVector.GetInstalledFiles.Item(x))
                        rVector.FileRemovalResult.Add(RemovalVector.FILE_REBOOT_REQUIRED)
                    Else
                        Logger.WriteDebug(CallStack, "Delete current file: " + hVector.GetInstalledFiles.Item(x))
                        System.IO.File.Delete(hVector.GetInstalledFiles.Item(x))
                        rVector.RemovalFileName.Add(hVector.GetInstalledFiles.Item(x))
                        rVector.FileRemovalResult.Add(RemovalVector.FILE_OK)
                    End If
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Error: Failed to delete current file(s), or schedule their removal.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                    If x > 0 Then ' Undo changes
                        For y As Integer = 0 To hVector.GetInstalledFiles.Count - 1
                            DestinationFileName = DestinationFolder + "\" + SubFolderList.Item(y)
                            DestinationFileName = DestinationFileName.Replace("\\", "\")
                            If System.IO.File.Exists(DestinationFileName) Then
                                Logger.WriteDebug(CallStack, "Restore current file: " + DestinationFileName)
                                Logger.WriteDebug(CallStack, "To: " + hVector.GetInstalledFiles.Item(y))
                                System.IO.File.Copy(DestinationFileName, hVector.GetInstalledFiles.Item(y), True)
                            End If
                        Next
                    End If
                    Logger.WriteDebug(CallStack, "Delete BACKOUT folder: " + DestinationFolder)
                    System.IO.Directory.Delete(DestinationFolder, True)
                    rVector.RemovalFileName.Add(hVector.GetInstalledFiles.Item(x))
                    rVector.FileRemovalResult.Add(RemovalVector.FILE_FAILED)
                    Logger.WriteDebug(CallStack, "Removal failed: " + rVector.RemovalItem)
                    rVector.CommentString = "Reason: Failed to delete current file(s), or schedule their removal."
                    Return 6
                End Try
            End If
        Next

        ' Restore original files
        For x As Integer = 0 To RestoreList.Count - 1
            Logger.WriteDebug(CallStack, "Restore file: " + RestoreList.Item(x))
            Logger.WriteDebug(CallStack, "To: " + hVector.GetInstalledFiles.Item(x))
            Try
                System.IO.File.Copy(RestoreList.Item(x), hVector.GetInstalledFiles.Item(x), True)
                If Not rVector.FileRemovalResult.Item(x) = RemovalVector.FILE_REBOOT_REQUIRED Then
                    rVector.FileRemovalResult.Item(x) = RemovalVector.FILE_OK
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Failed to restore original file(s).")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                If x > 0 Then ' Undo changes
                    For y As Integer = 0 To hVector.GetInstalledFiles.Count - 1
                        Logger.WriteDebug(CallStack, "Delete restored file: " + hVector.GetInstalledFiles.Item(y))
                        System.IO.File.Delete(hVector.GetInstalledFiles.Item(y))
                        DestinationFileName = DestinationFolder + "\" + SubFolderList.Item(y)
                        DestinationFileName = DestinationFileName.Replace("\\", "\")
                        If System.IO.File.Exists(DestinationFileName) Then
                            Logger.WriteDebug(CallStack, "Restore current file: " + DestinationFileName)
                            Logger.WriteDebug(CallStack, "To: " + hVector.GetInstalledFiles.Item(y))
                            System.IO.File.Copy(DestinationFileName, hVector.GetInstalledFiles.Item(y), True)
                        End If
                    Next
                End If
                Logger.WriteDebug(CallStack, "Delete BACKOUT folder: " + DestinationFolder)
                System.IO.Directory.Delete(DestinationFolder, True)
                rVector.RemovalFileName.Add(hVector.GetInstalledFiles.Item(x))
                rVector.FileRemovalResult.Add(RemovalVector.FILE_FAILED)
                Logger.WriteDebug(CallStack, "Removal failed: " + rVector.RemovalItem)
                rVector.CommentString = "Reason: Failed to restore original file(s)."
                Return 7
            End Try
        Next

        ' Remove REPLACED folder
        Logger.WriteDebug(CallStack, "Delete REPLACED subfolder: " + ReplacedFolder)
        System.IO.Directory.Delete(ReplacedFolder, True)

        Return 0

    End Function

End Class
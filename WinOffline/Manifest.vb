Partial Public Class WinOffline

    Public Class Manifest

        Private Shared PatchManifest As New ArrayList
        Private Shared RemovalManifest As New ArrayList
        Private Shared HistoryManifest As New ArrayList
        Private Shared ExceptionManifest As New ArrayList

        Public Const PATCH_MANIFEST As Integer = 0
        Public Const REMOVAL_MANIFEST As Integer = 1
        Public Const HISTORY_MANIFEST As Integer = 2
        Public Const EXCEPTION_MANIFEST As Integer = 3

        Public Shared Sub UpdateManifest(ByVal CallStack As String, ByVal ManifestType As Integer, ByVal NewItem As Object)

            CallStack += "UpdateManifest|"

            If ManifestType = PATCH_MANIFEST Then
                NewItem = DirectCast(NewItem, PatchVector)
                For Each pVector As PatchVector In PatchManifest
                    If NewItem.PatchFile.GetFriendlyName.Equals(pVector.PatchFile.GetFriendlyName) Then
                        Logger.WriteDebug(CallStack, "Warning: Manifest item is not unique-- skipping.")
                        Return
                    End If
                Next
            End If

            If ManifestType = PATCH_MANIFEST Then
                NewItem = DirectCast(NewItem, PatchVector)
                PatchManifest.Add(NewItem)
            ElseIf ManifestType = REMOVAL_MANIFEST Then
                NewItem = DirectCast(NewItem, RemovalVector)
                RemovalManifest.Add(NewItem)
            ElseIf ManifestType = HISTORY_MANIFEST Then
                NewItem = DirectCast(NewItem, HistoryVector)
                HistoryManifest.Add(NewItem)
            ElseIf ManifestType = EXCEPTION_MANIFEST Then
                NewItem = DirectCast(NewItem, Array)
                For Each strLine As String In NewItem
                    ExceptionManifest.Add(strLine)
                Next
                ExceptionManifest.Add("//BREAK//")
            End If

            Return

        End Sub

        Public Shared Sub ReadCache(ByVal CallStack As String, ByVal ManifestType As Integer)

            Dim CacheFile As String = ""
            Dim CacheReader As System.IO.StreamReader
            Dim strLine As String = ""
            Dim RestoredManifest As New ArrayList
            CallStack += "ReadCache|"

            If ManifestType = PATCH_MANIFEST Then
                PatchManifest = PatchVector.ReadCache(CallStack)
            ElseIf ManifestType = REMOVAL_MANIFEST Then
                RemovalManifest = RemovalVector.ReadCache(CallStack)
            ElseIf ManifestType = HISTORY_MANIFEST Then
                HistoryManifest = HistoryVector.ReadCache(CallStack)
            ElseIf ManifestType = EXCEPTION_MANIFEST Then
                CacheFile = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-exception.cache"
                Logger.WriteDebug(CallStack, "Open file: " + CacheFile)
                If Not System.IO.File.Exists(CacheFile) Then
                    Logger.WriteDebug(CallStack, "Cache file does not exist.")
                    Return
                End If
                CacheReader = New System.IO.StreamReader(CacheFile)
                Do While CacheReader.Peek() >= 0
                    strLine = CacheReader.ReadLine()
                    RestoredManifest.Add(strLine)
                Loop
                Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
                CacheReader.Close()
                ExceptionManifest = RestoredManifest
            Else
                Return
            End If

        End Sub

        Public Shared Sub WriteCache(ByVal CallStack As String, ByVal ManifestType As Integer)

            Dim CacheFile As String = ""
            Dim CacheWriter As System.IO.StreamWriter
            Dim strLine As String
            CallStack += "WriteCache|"

            If ManifestType = PATCH_MANIFEST Then
                PatchVector.WriteCache(CallStack, PatchManifest)
            ElseIf ManifestType = REMOVAL_MANIFEST Then
                RemovalVector.WriteCache(CallStack, RemovalManifest)
            ElseIf ManifestType = HISTORY_MANIFEST Then
                HistoryVector.WriteCache(CallStack, HistoryManifest)
            ElseIf ManifestType = EXCEPTION_MANIFEST Then
                CacheFile = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + "-exception.cache"
                If System.IO.File.Exists(CacheFile) Then
                    Utility.DeleteFile(CallStack, CacheFile) ' Delete prior cache file
                ElseIf Not System.IO.Directory.Exists(Globals.WinOfflineTemp) Then
                    Logger.WriteDebug(CallStack, "Warning: Temporary folder already cleaned up!")
                    Logger.WriteDebug(CallStack, "Warning: Cache file write aborted!")
                    Return
                End If
                Logger.WriteDebug(CallStack, "Write cache file: " + CacheFile)
                CacheWriter = New System.IO.StreamWriter(CacheFile, False)
                For n As Integer = 0 To ExceptionManifest.Count() - 1
                    strLine = ExceptionManifest.Item(n)
                    CacheWriter.WriteLine(strLine)
                Next
                Logger.WriteDebug(CallStack, "Close file: " + CacheFile)
                CacheWriter.Close()
            Else
                Return
            End If

        End Sub

        Public Shared Function PatchManifestCount() As Integer
            Return PatchManifest.Count
        End Function

        Public Shared Function GetPatchFromManifest(index As Integer) As PatchVector
            Return PatchManifest.Item(index)
        End Function

        Public Shared Function PatchManifestContains(ByVal PatchFriendlyName As String) As Boolean
            For Each pVector As PatchVector In PatchManifest
                If pVector.PatchFile.GetFriendlyName.ToLower.Equals(PatchFriendlyName.ToLower) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Shared Function RemovalManifestCount() As Integer
            Return RemovalManifest.Count
        End Function

        Public Shared Function GetRemovalFromManifest(index As Integer) As RemovalVector
            Return RemovalManifest.Item(index)
        End Function

        Public Shared Sub ResetRemovalManifest()
            RemovalManifest.Clear()
        End Sub

        Public Shared Function HistoryManifestCount() As Integer
            Return HistoryManifest.Count
        End Function

        Public Shared Function GetHistoryFromManifest(index As Integer) As HistoryVector
            Return HistoryManifest.Item(index)
        End Function

        Public Shared Function IsAlreadyApplied(ByVal pVector As PatchVector) As Boolean

            Dim IsApplied As Boolean = False

            For Each hVector As HistoryVector In HistoryManifest
                If hVector.GetPatchName.ToLower.Equals(pVector.PatchFile.GetFriendlyName.ToLower) Then
                    If hVector.IsApplyAction Then
                        IsApplied = True
                    Else
                        IsApplied = False
                    End If
                End If
            Next
            Return IsApplied

        End Function

        Public Shared Function DebugLogSummary() As String

            Dim PatchSummary As String = ""
            Dim CurrentComponent As String = ""
            Dim CommandFile As String = ""
            Dim ReturnCode As String = ""
            Dim ReplaceFile As String = ""
            Dim ExceptionCounter As Integer = 1

            ' Event summary
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 14
                PatchSummary += "-"
            Next
            PatchSummary += Environment.NewLine + Globals.ProcessFriendlyName + " Event Summary:" + Environment.NewLine
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 14
                PatchSummary += "-"
            Next
            PatchSummary += Environment.NewLine
            PatchSummary += GeneratePatchingSummary()

            ' Append library cleanup summary
            If Globals.CheckSDLibrarySwitch OrElse Globals.CleanupSDLibrarySwitch Then
                PatchSummary += "Software library cleanup summary:" + Environment.NewLine
                For Each strLine As String In LibraryManager.LibraryEvents
                    PatchSummary += "-" + strLine + Environment.NewLine
                Next
                PatchSummary += Environment.NewLine
            End If

            ' Update summary -- History manifest
            For Each hVector As HistoryVector In HistoryManifest
                If Not hVector.GetProductComponent.Equals(CurrentComponent) Then
                    CurrentComponent = hVector.GetProductComponent
                    For i As Integer = 0 To CurrentComponent.Length + 8
                        PatchSummary += "-"
                    Next
                    PatchSummary += Environment.NewLine + CurrentComponent + " History:" + Environment.NewLine
                    For i As Integer = 0 To CurrentComponent.Length + 8
                        PatchSummary += "-"
                    Next
                    PatchSummary += Environment.NewLine
                End If
                If hVector.IsEmpty Then
                    PatchSummary += "No history." + Environment.NewLine + Environment.NewLine
                Else
                    PatchSummary += hVector.GetHeader + Environment.NewLine
                    PatchSummary += hVector.GetInstallFilesRaw + Environment.NewLine
                End If
            Next
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 18
                PatchSummary += "-"
            Next

            ' Update summary -- Exception manifest
            PatchSummary += Environment.NewLine + Globals.ProcessFriendlyName + " Exception Summary:" + Environment.NewLine
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 18
                PatchSummary += "-"
            Next
            PatchSummary += Environment.NewLine
            If ExceptionManifest.Count = 0 Then
                PatchSummary += "No exceptions thrown." + Environment.NewLine
                PatchSummary += Environment.NewLine
            Else
                For i As Integer = 0 To ExceptionManifest.Count() - 1
                    PatchSummary += "Exception #" + ExceptionCounter.ToString + ": " + ExceptionManifest.Item(i).ToString + Environment.NewLine
                    For j As Integer = i + 1 To ExceptionManifest.Count() - 1
                        If ExceptionManifest.Item(j).ToString.Equals("//BREAK//") Then
                            i = j
                            Exit For
                        Else
                            PatchSummary += ExceptionManifest.Item(j).ToString + Environment.NewLine
                        End If
                    Next
                    PatchSummary += Environment.NewLine
                    ExceptionCounter += 1
                Next
            End If

            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 12
                PatchSummary += "-"
            Next

            PatchSummary += Environment.NewLine + Globals.ProcessFriendlyName + " Debug trace:" + Environment.NewLine
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 12
                PatchSummary += "-"
            Next

            Return PatchSummary

        End Function

        Public Shared Function DebugConsoleSummary() As String

            Dim PatchSummary As String = Environment.NewLine
            Dim CurrentComponent As String = ""
            Dim ExceptionCounter As Integer = 1

            ' Update summary -- History manifest
            For Each hVector As HistoryVector In HistoryManifest
                If Not hVector.GetProductComponent.Equals(CurrentComponent) Then
                    CurrentComponent = hVector.GetProductComponent
                    For i As Integer = 0 To CurrentComponent.Length + 8
                        PatchSummary += "-"
                    Next
                    PatchSummary += Environment.NewLine + CurrentComponent + " History:" + Environment.NewLine
                    For i As Integer = 0 To CurrentComponent.Length + 8
                        PatchSummary += "-"
                    Next
                    PatchSummary += Environment.NewLine
                End If
                If hVector.IsEmpty Then
                    PatchSummary += "No history." + Environment.NewLine + Environment.NewLine
                Else
                    PatchSummary += hVector.GetHeader + Environment.NewLine
                    PatchSummary += hVector.GetInstallFilesRaw + Environment.NewLine
                End If
            Next
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 18
                PatchSummary += "-"
            Next

            ' Update summary -- Exception manifest
            PatchSummary += Environment.NewLine + Globals.ProcessFriendlyName + " Exception Summary:" + Environment.NewLine
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 18
                PatchSummary += "-"
            Next
            PatchSummary += Environment.NewLine
            If ExceptionManifest.Count = 0 Then
                PatchSummary += "No exceptions thrown." + Environment.NewLine
                PatchSummary += Environment.NewLine
            Else
                For i As Integer = 0 To ExceptionManifest.Count() - 1
                    PatchSummary += "Exception #" + ExceptionCounter.ToString + ": " + ExceptionManifest.Item(i).ToString + Environment.NewLine
                    For j As Integer = i + 1 To ExceptionManifest.Count() - 1
                        If ExceptionManifest.Item(j).ToString.Equals("//BREAK//") Then
                            i = j
                            Exit For
                        Else
                            PatchSummary += ExceptionManifest.Item(j).ToString + Environment.NewLine
                        End If
                    Next
                    PatchSummary += Environment.NewLine
                    ExceptionCounter += 1
                Next
            End If
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 14
                PatchSummary += "-"
            Next

            ' Update summary -- Patch manifest
            PatchSummary += Environment.NewLine + Globals.ProcessFriendlyName + " Event Summary:" + Environment.NewLine
            For i As Integer = 0 To Globals.ProcessFriendlyName.Length + 14
                PatchSummary += "-"
            Next
            PatchSummary += Environment.NewLine
            PatchSummary += GeneratePatchingSummary()

            ' Append library cleanup summary
            If Globals.CheckSDLibrarySwitch OrElse Globals.CleanupSDLibrarySwitch Then
                PatchSummary += "Software library cleanup summary:" + Environment.NewLine
                For Each strLine As String In LibraryManager.LibraryEvents
                    PatchSummary += "-" + strLine + Environment.NewLine
                Next
                PatchSummary += Environment.NewLine
            End If

            Return PatchSummary

        End Function

        Public Shared Function GeneratePatchingSummary() As String

            Dim PatchSummary As String = ""
            Dim CommandFile As String = ""
            Dim ReturnCode As String = ""
            Dim ReplaceFile As String = ""

            If Globals.GetHistorySwitch Then
                PatchSummary += "Report agent patch history." + Environment.NewLine + Environment.NewLine
            ElseIf PatchManifest.Count = 0 AndAlso RemovalManifest.Count = 0 Then
                PatchSummary += "No patch operations performed." + Environment.NewLine + Environment.NewLine
            Else
                If Globals.RemovePatchSwitch Then
                    For i As Integer = 0 To RemovalManifestCount() - 1
                        PatchSummary += GetRemovalFromManifest(i).RemovalItem + ": "
                        If GetRemovalFromManifest(i).RemovalAction = RemovalVector.SKIPPED Then
                            PatchSummary += "SKIPPED" + Environment.NewLine
                            PatchSummary += GetRemovalFromManifest(i).CommentString + Environment.NewLine
                        ElseIf GetRemovalFromManifest(i).RemovalAction = RemovalVector.REMOVAL_OK Then
                            PatchSummary += "REMOVED" + Environment.NewLine
                            PatchSummary += "History File: " + GetRemovalFromManifest(i).HistoryFile + Environment.NewLine
                        ElseIf GetRemovalFromManifest(i).RemovalAction = RemovalVector.REMOVAL_FAIL Then
                            PatchSummary += "FAILED TO REMOVE" + Environment.NewLine
                            PatchSummary += GetRemovalFromManifest(i).CommentString + Environment.NewLine
                            Globals.PatchErrorDetected = True
                        Else
                            PatchSummary += "UNKNOWN" + Environment.NewLine
                        End If
                        For x As Integer = 0 To GetRemovalFromManifest(i).FileRemovalResult.Count - 1
                            ReplaceFile = GetRemovalFromManifest(i).RemovalFileName.Item(x)
                            PatchSummary += "- " + ReplaceFile + " ["
                            If GetRemovalFromManifest(i).FileRemovalResult.Item(x) = RemovalVector.FILE_REVERSED Then
                                PatchSummary += "REVERSED]" + Environment.NewLine
                            ElseIf GetRemovalFromManifest(i).FileRemovalResult.Item(x) = RemovalVector.FILE_SKIPPED Then
                                PatchSummary += "SKIPPED]" + Environment.NewLine
                            ElseIf GetRemovalFromManifest(i).FileRemovalResult.Item(x) = RemovalVector.FILE_OK Then
                                PatchSummary += "OK]" + Environment.NewLine
                            ElseIf GetRemovalFromManifest(i).FileRemovalResult.Item(x) = RemovalVector.FILE_REBOOT_REQUIRED Then
                                PatchSummary += "REBOOT REQUIRED]" + Environment.NewLine
                            ElseIf GetRemovalFromManifest(i).FileRemovalResult.Item(x) = RemovalVector.FILE_FAILED Then
                                PatchSummary += "FAILED]" + Environment.NewLine
                            End If
                        Next
                        PatchSummary += Environment.NewLine
                    Next
                Else
                    For i As Integer = 0 To PatchManifest.Count - 1
                        PatchSummary += GetPatchFromManifest(i).PatchFile.GetFriendlyName + ": "
                        If GetPatchFromManifest(i).PatchAction = PatchVector.UNAVAILABLE Then
                            PatchSummary += "UNAVAILABLE" + Environment.NewLine
                            PatchSummary += GetPatchFromManifest(i).CommentString.Replace("NEWLINE", Environment.NewLine)
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.NOT_APPLICABLE Then
                            PatchSummary += "NOT APPLICABLE" + Environment.NewLine
                            PatchSummary += GetPatchFromManifest(i).CommentString.Replace("NEWLINE", Environment.NewLine) + Environment.NewLine
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.ALREADY_APPLIED Then
                            PatchSummary += "ALREADY APPLIED" + Environment.NewLine
                            PatchSummary += GetPatchFromManifest(i).CommentString.Replace("NEWLINE", Environment.NewLine) + Environment.NewLine
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.SKIPPED Then
                            PatchSummary += "SKIPPED" + Environment.NewLine
                            PatchSummary += GetPatchFromManifest(i).CommentString.Replace("NEWLINE", Environment.NewLine) + Environment.NewLine
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_OK Then
                            PatchSummary += "APPLIED" + Environment.NewLine
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_FAIL Then
                            PatchSummary += "FAILED TO APPLY" + Environment.NewLine
                            PatchSummary += GetPatchFromManifest(i).CommentString.Replace("NEWLINE", Environment.NewLine) + Environment.NewLine
                            Globals.PatchErrorDetected = True
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.EXECUTE_OK Then
                            PatchSummary += "EXECUTED" + Environment.NewLine
                        ElseIf GetPatchFromManifest(i).PatchAction = PatchVector.EXECUTE_FAIL Then
                            PatchSummary += "FAILED TO EXECUTE" + Environment.NewLine
                            PatchSummary += GetPatchFromManifest(i).CommentString.Replace("NEWLINE", Environment.NewLine) + Environment.NewLine
                            Globals.PatchErrorDetected = True
                        Else
                            PatchSummary += "UNKNOWN" + Environment.NewLine
                        End If
                        If GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_OK OrElse
                            GetPatchFromManifest(i).PatchAction = PatchVector.APPLY_FAIL OrElse
                            GetPatchFromManifest(i).PatchAction = PatchVector.EXECUTE_OK OrElse
                            GetPatchFromManifest(i).PatchAction = PatchVector.EXECUTE_FAIL Then

                            For x As Integer = 0 To GetPatchFromManifest(i).GetPreCommandList.Count - 1
                                CommandFile = GetPatchFromManifest(i).GetPreCommandList.Item(x)
                                If GetPatchFromManifest(i).PreCmdReturnCodes.Count > x Then
                                    ReturnCode = GetPatchFromManifest(i).PreCmdReturnCodes.Item(x)
                                    PatchSummary += "- " + CommandFile + " [Return Code: " + ReturnCode + "]" + Environment.NewLine
                                Else
                                    ReturnCode = "NOT EXECUTED"
                                    PatchSummary += "- " + CommandFile + " [" + ReturnCode + "]" + Environment.NewLine
                                End If
                            Next

                            For x As Integer = 0 To GetPatchFromManifest(i).DestReplaceList.Count - 1
                                ReplaceFile = GetPatchFromManifest(i).DestReplaceList.Item(x)
                                PatchSummary += "- " + ReplaceFile + " ["
                                If GetPatchFromManifest(i).FileReplaceResult.Item(x) = PatchVector.FILE_REVERSED Then
                                    PatchSummary += "REVERSED]" + Environment.NewLine
                                ElseIf GetPatchFromManifest(i).FileReplaceResult.Item(x) = PatchVector.FILE_SKIPPED Then
                                    PatchSummary += "SKIPPED]" + Environment.NewLine
                                ElseIf GetPatchFromManifest(i).FileReplaceResult.Item(x) = PatchVector.FILE_OK Then
                                    PatchSummary += "OK]" + Environment.NewLine
                                ElseIf GetPatchFromManifest(i).FileReplaceResult.Item(x) = PatchVector.FILE_REBOOT_REQUIRED Then
                                    PatchSummary += "REBOOT REQUIRED]" + Environment.NewLine
                                ElseIf GetPatchFromManifest(i).FileReplaceResult.Item(x) = PatchVector.FILE_FAILED Then
                                    PatchSummary += "FAILED]" + Environment.NewLine
                                End If
                            Next

                            For x As Integer = 0 To GetPatchFromManifest(i).GetSysCommandList.Count - 1
                                CommandFile = GetPatchFromManifest(i).GetSysCommandList.Item(x)
                                If GetPatchFromManifest(i).SysCmdReturnCodes.Count > x Then
                                    ReturnCode = GetPatchFromManifest(i).SysCmdReturnCodes.Item(x)
                                    PatchSummary += "- " + CommandFile + " [Return Code: " + ReturnCode + "]" + Environment.NewLine
                                Else
                                    ReturnCode = "NOT EXECUTED"
                                    PatchSummary += "- " + CommandFile + " [" + ReturnCode + "]" + Environment.NewLine
                                End If
                            Next

                            For x As Integer = 0 To GetPatchFromManifest(i).GetPostCommandList.Count - 1
                                CommandFile = GetPatchFromManifest(i).GetPostCommandList.Item(x)
                                If GetPatchFromManifest(i).PostCmdReturnCodes.Count > x Then
                                    ReturnCode = GetPatchFromManifest(i).PostCmdReturnCodes.Item(x)
                                    PatchSummary += "- " + CommandFile + " [Return Code: " + ReturnCode + "]" + Environment.NewLine
                                Else
                                    ReturnCode = "NOT EXECUTED"
                                    PatchSummary += "- " + CommandFile + " [" + ReturnCode + "]" + Environment.NewLine
                                End If
                            Next

                        End If
                        PatchSummary += Environment.NewLine
                    Next
                End If
            End If

            Return PatchSummary

        End Function

        Public Shared Function FlushManifestData() As Boolean

            Dim DataCleared As Boolean = False

            ' Check if any manifest data will be cleared (simply to reduce a deinit message if datacleared is false)
            If PatchManifest.Count > 0 OrElse RemovalManifest.Count > 0 OrElse HistoryManifest.Count > 0 Then DataCleared = True

            ' Clear manifests
            PatchManifest = New ArrayList
            RemovalManifest = New ArrayList
            HistoryManifest = New ArrayList
            ' ExceptionStack = New ArrayList ' Don't clear the exception manifest, this should persist

            Return DataCleared

        End Function

    End Class

End Class
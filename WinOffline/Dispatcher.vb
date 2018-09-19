﻿Partial Public Class WinOffline

    Public Shared Function Dispatcher(ByVal CallStack As String) As Integer

        Dim StateFile As String
        Dim StateStreamReader As System.IO.StreamReader
        Dim StateLine As String
        Dim RunLevel As Integer = 0
        CallStack += "Dispatcher|"
        StateFile = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state"

        Try
            If System.IO.File.Exists(StateFile) Then

                ' Read state file and set execution mode
                Logger.WriteDebug(CallStack, "Found prior execution marker.")
                Logger.WriteDebug(CallStack, "Upgrade execution mode: Software Delivery")
                Logger.WriteDebug(CallStack, "Open file: " + StateFile)
                StateStreamReader = New System.IO.StreamReader(StateFile)
                StateLine = StateStreamReader.ReadLine()
                StateStreamReader.Close()
                Globals.SDBasedMode = True

                ' Dispatch based on state file contents
                If StateLine.Equals("StageI completed.") Then
                    Logger.WriteDebug(CallStack, "Execution marker: StageI completed.")

                    ' Check for wrong mode (StageII under software delivery mode)
                    If Globals.ParentProcessTree.Contains("sd_jexec") Then
                        Logger.WriteDebug(CallStack, "Error: StageII requested in software delivery online mode.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageII requested in software delivery online mode.",
                                                 "Reason: A prior " + Globals.ProcessFriendlyName + " execution was incomplete."})
                        ' Set the DIRTY flag [Cached job output ID is invalid]
                        '   This signals MAIN that we should re-collect the job output ID from software delivery.
                        '   If collected from temp cache, we have the output ID from the wrong/previous job.
                        '   Re-init will wipe the cache, along with all temps, then we will pickup the new/current job ID.
                        Globals.DirtyFlag = True
                        Init.SDStageIReInit(CallStack)
                        RunLevel = StageI(CallStack) ' StageI
                        If RunLevel <> 0 Then
                            Logger.WriteDebug(CallStack, "Set final stage marker: True")
                            Globals.FinalStage = True
                            Return 2 ' Return (Wrong mode, ReInit, StageI-FAIL)
                        End If
                        Return RunLevel
                    End If

                    ' Dispatch StageII
                    RunLevel = StageII(CallStack)
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: StageII reports failure.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})
                    End If

                ElseIf StateLine.Equals("StageII completed.") Then
                    Logger.WriteDebug(CallStack, "Execution marker: StageII Completed.")
                    RunLevel = StageIII(CallStack) ' Dispatch StageIII
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: StageIII reports failure.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageIII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Globals.FinalStage = True
                        Return 3 ' Return (Software Delivery: StageI-OK, StageII-OK, StageIII-FAIL)
                    Else
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Globals.FinalStage = True
                    End If

                Else
                    ' This should never happen
                    Logger.WriteDebug(CallStack, "Error: Execution marker is unknown.")
                    Manifest.UpdateManifest(CallStack,
                                            Manifest.EXCEPTION_MANIFEST,
                                            {"Error: Execution marker is invalid.",
                                            "Reason: The execution marker is not recognized as a valid state."})
                    Logger.WriteDebug(CallStack, "Set final stage marker: True")
                    Globals.FinalStage = True
                    Return 4 ' Return (Software Delivery: Unknown marker, StageIII-OK)
                End If

            Else
                Logger.WriteDebug(CallStack, "Execution marker not found.")

                ' No marker, dispatch based on SD/non-SD mode
                If Not Globals.SDBasedMode Then
                    Logger.WriteDebug(CallStack, "Execution mode: Non-Software Delivery")

                    ' Dispatch StageI
                    RunLevel = StageI(CallStack)
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: StageI reports failure.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageI reports an error.",
                                                "Reason: Please analyze the debug log for more information."})
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Globals.FinalStage = True
                        Return 5 ' Return (Interactive: StageI-FAIL)
                    End If

                    ' Dispatch StageII
                    RunLevel = StageII(CallStack)
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: StageII reports failure.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Globals.FinalStage = True
                        Return 6 ' Return (Interactive: StageI-OK, StageII-FAIL)
                    End If

                    ' Dispatch StageIII
                    RunLevel = StageIII(CallStack)
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: StageIII reports failure.")
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageIII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})
                        Globals.FinalStage = True
                        Return 7 ' Return (Interactive: StageI-OK, StageII-OK, StageIII-FAIL)
                    Else
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Globals.FinalStage = True
                    End If

                Else
                    Logger.WriteDebug(CallStack, "Execution mode: Software delivery")
                    RunLevel = StageI(CallStack) ' Dispatch StageI
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: StageI reports failure.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageI reports an error.",
                                                "Reason: Please analyze the debug log for more information."})
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")
                        Globals.FinalStage = True
                        Return 8 ' Return (Software Delivery: StageI-FAIL)
                    End If
                End If

            End If

        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught in the dispatcher.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Logger.WriteDebug(CallStack, "Set final stage marker: True")
            Globals.FinalStage = True
            Return 1
        End Try

        Return RunLevel

    End Function

End Class
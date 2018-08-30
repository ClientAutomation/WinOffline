Partial Public Class WinOffline

    ' Execution Dispatcher
    Public Shared Function Dispatcher(ByVal CallStack As String) As Integer

        ' Local variables
        Dim StateFile As String                             ' Execution state marker filename
        Dim StateStreamReader As System.IO.StreamReader     ' Input stream for reading the WinOffline state file.
        Dim StateLine As String                             ' String for reading execution state from file.
        Dim RunLevel As Integer = 0                         ' Tracks the health of the function and calls to external functions.

        ' Update call stack
        CallStack += "Dispatcher|"

        ' Set execution state marker filename
        StateFile = Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state"

        ' *****************************
        ' - Check for prior execution marker.
        ' *****************************

        Try

            ' Check for execution marker file
            If System.IO.File.Exists(StateFile) Then

                ' *****************************
                ' - Read prior execution marker.
                ' *****************************

                ' Write debug
                Logger.WriteDebug(CallStack, "Found prior execution marker.")

                ' Update execution mode
                Globals.SDBasedMode = True

                ' Write debug
                Logger.WriteDebug(CallStack, "Upgrade execution mode: Software Delivery")

                ' Write debug
                Logger.WriteDebug(CallStack, "Open file: " + StateFile)

                ' Open the state marker file
                StateStreamReader = New System.IO.StreamReader(StateFile)

                ' Read the state marker
                StateLine = StateStreamReader.ReadLine()

                ' Close state marker file
                StateStreamReader.Close()

                ' *****************************
                ' - Dispatch next stage.
                ' *****************************

                ' Determine execution stage
                If StateLine.Equals("StageI completed.") Then

                    ' *****************************
                    ' - Marker: StageI completed, dispatch StageII.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Execution marker: StageI completed.")

                    ' Verify offline execution mode
                    If Globals.ParentProcessTree.Contains("sd_jexec") Then

                        ' *****************************
                        ' - StageII dispatched incorrectly in ONLINE mode.
                        ' *****************************

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageII requested in software delivery online mode.")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageII requested in software delivery online mode.",
                                                 "Reason: A prior " + Globals.ProcessFriendlyName + " execution was incomplete."})

                        ' Set the DIRTY flag [Cached job output ID is invalid]
                        '   This signals MAIN that we should re-collect the job output ID from software delivery.
                        '   If collected from temp cache, we have the output ID from the wrong/previous job.
                        '   Re-init will wipe the cache, along with all temps, then we will pickup the new/current job ID.
                        Globals.DirtyFlag = True

                        ' Perform reinit of temp folder and resources
                        Init.SDStageIReInit(CallStack)

                        ' *****************************
                        ' - Re-dispatch StageI.
                        ' *****************************

                        ' Prior state marker was cleaned up -- Start over with StageI
                        RunLevel = StageI(CallStack)

                        ' Check the run level
                        If RunLevel <> 0 Then

                            ' Write debug
                            Logger.WriteDebug(CallStack, "Set final stage marker: True")

                            ' Set final stage indicator
                            Globals.FinalStage = True

                            ' Return (Wrong mode, ReInit, StageI-FAIL)
                            Return 2

                        End If

                        ' Return
                        Return RunLevel

                    End If

                    ' *****************************
                    ' - Dispatch StageII.
                    ' *****************************

                    ' Launch StageII
                    RunLevel = StageII(CallStack)

                    ' Check the run level
                    If RunLevel <> 0 Then

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageII reports failure.")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})

                    End If

                ElseIf StateLine.Equals("StageII completed.") Then

                    ' *****************************
                    ' - Marker: StageII completed, dispatch StageIII.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Execution marker: StageII Completed.")

                    ' *****************************
                    ' - Dispatch StageIII.
                    ' *****************************

                    ' Launch StageIII
                    RunLevel = StageIII(CallStack)

                    ' Check the run level
                    If RunLevel <> 0 Then

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageIII reports failure.")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageIII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Set final stage indicator
                        Globals.FinalStage = True

                        ' Return (Software Delivery: StageI-OK, StageII-OK, StageIII-FAIL)
                        Return 3

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Set final stage indicator
                        Globals.FinalStage = True

                    End If

                Else

                    ' *****************************
                    ' - Marker: Unknown/Unrecognized.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Error: Execution marker is unknown.")

                    ' Create exception
                    Manifest.UpdateManifest(CallStack,
                                            Manifest.EXCEPTION_MANIFEST,
                                            {"Error: Execution marker is invalid.",
                                            "Reason: The execution marker is not recognized as a valid state."})

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Set final stage marker: True")

                    ' Set final stage indicator
                    Globals.FinalStage = True

                    ' Return (Software Delivery: Unknown marker, StageIII-OK)
                    Return 4

                End If

            Else

                ' Write debug
                Logger.WriteDebug(CallStack, "Execution marker not found.")

                ' *****************************
                ' - No prior execution marker, dispatch based on execution mode.
                ' *****************************

                ' Check the execution mode
                If Not Globals.SDBasedMode Then

                    ' *****************************
                    ' - Execution mode: Interactive.
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Execution mode: Non-Software Delivery")

                    ' *****************************
                    ' - Dispatch StageI.
                    ' *****************************

                    ' Launch StageI
                    RunLevel = StageI(CallStack)

                    ' Check the run level
                    If RunLevel <> 0 Then

                        ' *****************************
                        ' - StageI failure.
                        ' *****************************

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageI reports failure.")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageI reports an error.",
                                                "Reason: Please analyze the debug log for more information."})

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Set final stage indicator
                        Globals.FinalStage = True

                        ' Return (Interactive: StageI-FAIL)
                        Return 5

                    End If

                    ' *****************************
                    ' - Dispatch StageII.
                    ' *****************************

                    ' Launch StageII
                    RunLevel = StageII(CallStack)

                    ' Check the run level
                    If RunLevel <> 0 Then

                        ' *****************************
                        ' - StageII failure.
                        ' *****************************

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageII reports failure.")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Set final stage indicator
                        Globals.FinalStage = True

                        ' Return (Interactive: StageI-OK, StageII-FAIL)
                        Return 6

                    End If

                    ' *****************************
                    ' - Dispatch StageIII.
                    ' *****************************

                    ' Launch StageIII
                    RunLevel = StageIII(CallStack)

                    ' Check the run level
                    If RunLevel <> 0 Then

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageIII reports failure.")
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageIII reports an error.",
                                                "Reason: Please analyze the debug log for more information."})

                        ' Set final stage indicator
                        Globals.FinalStage = True

                        ' Return (Interactive: StageI-OK, StageII-OK, StageIII-FAIL)
                        Return 7

                    Else

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Set final stage indicator
                        Globals.FinalStage = True

                    End If

                Else

                    ' *****************************
                    ' - Execution mode: Software delivery
                    ' *****************************

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Execution mode: Software delivery")

                    ' *****************************
                    ' - Dispatch StageI.
                    ' *****************************

                    ' State marker does not exist -- Launch StageI
                    RunLevel = StageI(CallStack)

                    ' Check the run level
                    If RunLevel <> 0 Then

                        ' *****************************
                        ' - StageI failure.
                        ' *****************************

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Error: StageI reports failure.")

                        ' Create exception
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: StageI reports an error.",
                                                "Reason: Please analyze the debug log for more information."})

                        ' Write debug
                        Logger.WriteDebug(CallStack, "Set final stage marker: True")

                        ' Set final stage indicator
                        Globals.FinalStage = True

                        ' Return (Software Delivery: StageI-FAIL)
                        Return 8

                    End If

                End If

            End If

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught in the dispatcher.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)

            ' Create exception
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})

            ' Write debug
            Logger.WriteDebug(CallStack, "Set final stage marker: True")

            ' Set final stage indicator
            Globals.FinalStage = True

            ' Return
            Return 1

        End Try

        ' Return
        Return RunLevel

    End Function

End Class
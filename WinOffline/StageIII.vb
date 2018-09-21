Partial Public Class WinOffline

    Public Shared Function StageIII(ByVal CallStack As String) As Integer

        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim SignalReRunProcessStartInfo As ProcessStartInfo
        Dim RunLevel As Integer = 0

        CallStack += "StageIII|"
        Logger.SetCurrentTask("StageIII..")

        ' Read cache files
        Try
            Manifest.ReadCache(CallStack, Manifest.EXCEPTION_MANIFEST)
            Manifest.ReadCache(CallStack, Manifest.HISTORY_MANIFEST)
            Manifest.ReadCache(CallStack, Manifest.PATCH_MANIFEST)
            Manifest.ReadCache(CallStack, Manifest.REMOVAL_MANIFEST)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught reading cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            RunLevel += 1
        End Try

        ' Switch: Perform software library cleanup
        Try
            If Globals.CheckSDLibrarySwitch Then
                Logger.WriteDebug(CallStack, "Switch: Analyze software library without making changes.")
                LibraryManager.RepairLibrary(CallStack)
            ElseIf Globals.CleanupSDLibrarySwitch Then
                Logger.WriteDebug(CallStack, "Switch: Perform software library cleanup.")
                LibraryManager.RepairLibrary(CallStack)
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught during software library cleanup.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Signal software delivery reboot request [SD EXECUTION ONLY]
        If Globals.SDBasedMode And Globals.SDSignalRebootSwitch Then

            ExecutionString = Globals.DSMFolder + "bin\" + "sd_acmd.exe"
            ArgumentString = "signal reboot"

            Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
            SignalReRunProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            SignalReRunProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            SignalReRunProcessStartInfo.UseShellExecute = False
            SignalReRunProcessStartInfo.RedirectStandardOutput = True
            SignalReRunProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(SignalReRunProcessStartInfo)
            RunningProcess.WaitForExit()

            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd.ToString)
            Logger.WriteDebug("------------------------------------------------------------")
            RunLevel = RunningProcess.ExitCode
            Logger.WriteDebug(CallStack, "Exit code: " + RunLevel.ToString)
            RunningProcess.Close()

            If RunLevel <> 0 Then
                Logger.WriteDebug(CallStack, "Error: Failed to signal reboot request.")
                Manifest.UpdateManifest(CallStack,
                                        Manifest.EXCEPTION_MANIFEST,
                                        {"Error: Failed to signal reboot request.",
                                        "Reason: Non-zero return code from sd_acmd."})
                RunLevel += 1
            End If
        End If

        ' Switch: Initiate a system reboot
        If Globals.RebootSystemSwitch Then
            Globals.RebootOnTermination = True
        End If

        ' Check runlevel and exit
        If RunLevel <> 0 Then
            Logger.WriteDebug(CallStack, "StageIII execution completed with an error.")
        End If
        Globals.StageIIICompleted = True
        Return RunLevel

    End Function

End Class
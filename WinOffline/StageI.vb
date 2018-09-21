Partial Public Class WinOffline

    Public Shared Function StageI(ByVal CallStack As String) As Integer

        Dim StateStreamWriter As System.IO.StreamWriter
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim SignalReRunProcessStartInfo As ProcessStartInfo
        Dim OfflineProcessStartInfo As ProcessStartInfo
        Dim RunLevel As Integer = 0

        CallStack += "StageI|"
        Logger.SetCurrentTask("StageI..")

        ' Switch: Report agent patch history only
        If Globals.SDBasedMode AndAlso Globals.GetHistorySwitch Then
            RunLevel = ProcessHistoryFiles(CallStack)
            If RunLevel <> 0 Then
                Logger.WriteDebug(CallStack, "Error: Failed to process patch history files.")
                Return 1
            End If
            Globals.FinalStage = True
            Return 0
        End If

        ' Process patches
        If Not Globals.RemovePatchSwitch Then
            Try
                RunLevel = PatchProcessor(CallStack)
                If RunLevel <> 0 Then
                    Logger.WriteDebug(CallStack, "Error: Patch processor failure.")
                    Return 2
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught during patch processing.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 3
            End Try
        End If

        ' Copy WinOffline to temp [SD EXECUTION ONLY]
        If Globals.SDBasedMode Then
            If Not Utility.IsProcessRunningEx(Globals.WindowsTemp + "\" + Globals.ProcessShortName) Then
                Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName)
            End If
            Try
                If Not System.IO.File.Exists(Globals.WindowsTemp + "\" + Globals.ProcessShortName) Then
                    Logger.WriteDebug(CallStack, "Copy File: " + Globals.ProcessName)
                    Logger.WriteDebug(CallStack, "To: " + Globals.WindowsTemp + "\" + Globals.ProcessShortName)
                    System.IO.File.Copy(Globals.ProcessName, Globals.WindowsTemp + "\" + Globals.ProcessShortName, True)
                End If
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught copying " + Globals.ProcessFriendlyName + " to temporary directory.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 4
            End Try
        End If

        ' Signal software delivery re-run request [SD EXECUTION ONLY]
        If Globals.SDBasedMode Then

            ExecutionString = Globals.DSMFolder + "bin\" + "sd_acmd.exe"
            ArgumentString = "signal rerun"

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
                Logger.WriteDebug(CallStack, "Error: Failed to signal rerun request.")
                Manifest.UpdateManifest(CallStack,
                                        Manifest.EXCEPTION_MANIFEST,
                                        {"Error: Failed to signal rerun request.",
                                        "Reason: Non-zero return code from sd_acmd."})
                Return 5
            End If
        End If

        ' Write cache files
        Manifest.WriteCache(CallStack, Manifest.PATCH_MANIFEST)
        Manifest.WriteCache(CallStack, Manifest.REMOVAL_MANIFEST)
        Manifest.WriteCache(CallStack, Manifest.EXCEPTION_MANIFEST)

        ' Write execution state file [SD EXECUTION ONLY]
        If Globals.SDBasedMode Then
            Try
                Logger.WriteDebug(CallStack, "Open file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state")
                Logger.WriteDebug(CallStack, "Write: StageI completed")
                StateStreamWriter = New System.IO.StreamWriter(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state", False)
                StateStreamWriter.WriteLine("StageI completed.")
                Logger.WriteDebug(CallStack, "Close file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state")
                StateStreamWriter.Close()
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught writing state file.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 6
            End Try
        End If

        ' Create detached offline process [SD EXECUTION ONLY]
        If Globals.SDBasedMode Then
            ExecutionString = Globals.WindowsTemp + "\" + Globals.ProcessShortName
            ArgumentString = ""
            For Each arg As String In Globals.CommandLineArgs
                ArgumentString += " " + arg
            Next
            If Globals.RunningAsSystemIdentity Then
                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                OfflineProcessStartInfo = New ProcessStartInfo(ExecutionString)
                OfflineProcessStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(ExecutionString)
                OfflineProcessStartInfo.Arguments = ArgumentString
                Process.Start(OfflineProcessStartInfo)
            Else
                RunLevel = LaunchPad(CallStack, "system", Globals.WindowsTemp + "\" + Globals.ProcessShortName, Globals.WindowsTemp + "\", ArgumentString)
                If RunLevel = 0 Then
                    Logger.WriteDebug(CallStack, "Detached process launched as system identity.")
                Else
                    Logger.WriteDebug(CallStack, "Warning: Failed to launch detached process.")
                End If
            End If
        End If

        Globals.StageICompleted = True
        Return RunLevel

    End Function

End Class
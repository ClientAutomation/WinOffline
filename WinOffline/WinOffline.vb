Public Class WinOffline

    Public Shared Function Main(ByVal CommandLineArguments As String()) As Integer

        Dim CallStack As String = "|Main|"
        Dim RunLevel As Integer = 0

        Logger.SetCurrentTask("Starting up..")

        Globals.ProcessName = Process.GetCurrentProcess.MainModule.FileName
        Globals.ProcessFilePath = FileVector.GetFilePath(Globals.ProcessName)
        Globals.ProcessShortName = FileVector.GetShortName(Globals.ProcessName)
        Globals.ProcessFriendlyName = FileVector.GetFriendlyName(Globals.ProcessName)
        Globals.DotNetVersion = Environment.Version.ToString
        Globals.CommandLineArgs = CommandLineArguments

        If WindowsAPI.AttachConsole(-1) Then Globals.AttachedtoConsole = True Else Globals.AttachedtoConsole = False

        ' Main process initialization
        RunLevel = Init.Init(CallStack)
        If RunLevel <> 0 Then
            Init.DeInit(CallStack, True, False)
            Return RunLevel
        End If

        ' Determine entry point
        If Globals.ParentProcessTree.Contains("sd_jexec") Then
            Logger.WriteDebug(CallStack, "Entry point: Software Delivery")
            Globals.SDBasedMode = True

            ' Establish software delivery output ID and location
            RunLevel = JobContainer(CallStack)
            If RunLevel <> 0 OrElse Globals.JobOutputFile Is Nothing OrElse Globals.JobOutputFile.Equals("") Then
                Globals.WriteSDJobOutput = False
            Else
                Logger.WriteDebug(CallStack, "Software delivery job output file: " + Globals.JobOutputFile)
                Globals.WriteSDJobOutput = True
            End If

            Globals.DispatcherReturnCode = Dispatcher(CallStack)
        Else
            Logger.WriteDebug(CallStack, "Entry point: Non-Software Delivery")
            Globals.SDBasedMode = False

            If Not Globals.RunningAsSystemIdentity Then
                If Globals.AttachedtoConsole Then
                    If Globals.ITCMInstalled = False Then
                        Logger.WriteDebug(CallStack, "ITCM is not installed.")
                        Init.DeInit(CallStack, True, False)
                        Return 0
                    End If
                Else
                    Logger.WriteDebug(CallStack, "UI interaction: WinOffline Explorer")
                    Globals.CleanupLogsSwitch = True
                    Globals.WinOfflineExplorer = New WinOfflineUI
                    If Globals.WinOfflineExplorer.ShowDialog() = Windows.Forms.DialogResult.Abort Or Globals.DumpCazipxpSwitch Then
                        If Globals.DumpCazipxpSwitch Then
                            System.IO.File.Copy(Globals.WinOfflineTemp + "\Cazipxp.exe", Globals.WorkingDirectory + "Cazipxp.exe", True)
                        End If
                        Init.DeInit(CallStack, True, False)
                        Return 0
                    End If
                    Globals.ProgressGUI = New ProgressUI()
                    Globals.ProgressGUI.Hide()
                    Globals.ProgressGUI.SetFormTitle(Globals.ProcessFriendlyName + " -- " + Globals.AppVersion)
                    Globals.ProgressUIThread = New System.Threading.Thread(AddressOf Globals.ProgressGUI.ShowDialog)
                    Globals.ProgressUIThread.Start()
                    Globals.ProgressGUI.TrayIcon.Visible = True
                End If
            End If
            Globals.DispatcherReturnCode = Dispatcher(CallStack)
        End If

        ' Check for DIRTY execution flag
        If Globals.DirtyFlag Then
            Logger.WriteDebug(CallStack, "Dirty execution flag was set.")

            ' Reset the job output ID
            RunLevel = JobContainer(CallStack)
            If RunLevel <> 0 OrElse Globals.JobOutputFile Is Nothing OrElse Globals.JobOutputFile.Equals("") Then
                Logger.WriteDebug(CallStack, "Error: Exception caught processing software delivery container file.")
                Globals.WriteSDJobOutput = False
            Else
                Logger.WriteDebug(CallStack, "Software delivery job output file: " + Globals.JobOutputFile)
                Globals.WriteSDJobOutput = True
            End If

            Globals.DirtyFlag = False
            Globals.PatchErrorDetected = False
        End If

        ' Update logger task based on execution conditions
        If Globals.SDBasedMode AndAlso Globals.StageIICompleted Then
            Logger.SetCurrentTask("Waiting for software delivery..")
        ElseIf Globals.SDBasedMode AndAlso Globals.StageICompleted Then
            Logger.SetCurrentTask("Waiting for next stage..")
        Else
            Logger.SetCurrentTask("Wrapping up..")
        End If

        ' Process dispatcher return code and deinitialize
        If Globals.DispatcherReturnCode <> 0 Then
            Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " completed with an error.")
        Else
            Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " completed.")
        End If
        If Globals.FinalStage Then
            Init.DeInit(CallStack, False, True)
        End If

        ' Dispatch debug gui [User execution only]
        If Not Globals.RunningAsSystemIdentity AndAlso Globals.ProgressGUI IsNot Nothing Then
            If Globals.FinalStage Then
                Globals.ProgressGUI.Delegate_ProgressUI_GoModeless(True) ' Autoclose the debug console [Autoclose = True]
            Else
                Globals.ProgressGUI.Delegate_ProgressUI_GoModeless(False) ' Hide the debug console [Autoclose = False]
            End If
        End If

        ' Switch: Initiate a system reboot
        If Globals.RebootOnTermination Then
            Utility.SystemReboot(30)
        End If

        ' Terminate program
        If Globals.DispatcherReturnCode <> 0 Then
            Return 9
        ElseIf Globals.PatchErrorDetected Then
            Return 10
        Else
            Return 0
        End If

    End Function

End Class
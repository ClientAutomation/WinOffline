Public Class WinOffline

    Public Shared Function Main(ByVal CommandLineArguments As String()) As Integer

        ' Local variables
        Dim CallStack As String = "|Main|"
        Dim RunLevel As Integer = 0

        ' Write debug
        Logger.SetCurrentTask("Starting up..")

        ' Obtain process globals
        Globals.ProcessName = Process.GetCurrentProcess.MainModule.FileName
        Globals.ProcessFilePath = FileVector.GetFilePath(Globals.ProcessName)
        Globals.ProcessShortName = FileVector.GetShortName(Globals.ProcessName)
        Globals.ProcessFriendlyName = FileVector.GetFriendlyName(Globals.ProcessName)
        Globals.DotNetVersion = Environment.Version.ToString
        Globals.CommandLineArgs = CommandLineArguments

        ' Attach to console (if available), and set flag
        If WindowsAPI.AttachConsole(-1) Then Globals.AttachedtoConsole = True Else Globals.AttachedtoConsole = False

        ' *****************************
        ' - Main process initialization.
        ' *****************************

        ' Initialize
        RunLevel = Init.Init(CallStack)

        ' Check the run level
        If RunLevel <> 0 Then

            ' Deinitialize
            Init.DeInit(CallStack, True, False)

            ' Return [1-8]
            Return RunLevel

        End If

        ' *****************************
        ' - Determine entry point.
        ' *****************************

        ' Entry point check
        If Globals.ParentProcessTree.Contains("sd_jexec") Then

            ' Write debug
            Logger.WriteDebug(CallStack, "Entry point: Software Delivery")

            ' Set execution mode flag
            Globals.SDBasedMode = True

            ' *****************************
            ' - Establish software delivery output ID and location.
            ' *****************************

            ' Process the active container file (.cwf)
            RunLevel = JobContainer(CallStack)

            ' Verify job output file availability
            If RunLevel <> 0 OrElse Globals.JobOutputFile Is Nothing OrElse Globals.JobOutputFile.Equals("") Then

                ' Set job output flag
                Globals.WriteSDJobOutput = False

            Else

                ' Write debug
                Logger.WriteDebug(CallStack, "Software delivery job output file: " + Globals.JobOutputFile)

                ' Set job output flag
                Globals.WriteSDJobOutput = True

            End If

            ' Call the execution state dispatcher
            Globals.DispatcherReturnCode = Dispatcher(CallStack)

        Else

            ' Write debug
            Logger.WriteDebug(CallStack, "Entry point: Non-Software Delivery")

            ' Set execution mode flag
            Globals.SDBasedMode = False

            ' System or user account check
            If Not Globals.RunningAsSystemIdentity Then

                ' Check if we're attached to the console
                If Globals.AttachedtoConsole Then

                    ' Check if ITCM is not installed
                    If Globals.ITCMInstalled = False Then

                        ' Write debug
                        Logger.WriteDebug(CallStack, "ITCM is not installed.")

                        ' Deinitialize
                        Init.DeInit(CallStack, True, False)

                        ' Return
                        Return 0

                    End If

                Else

                    ' Write debug
                    Logger.WriteDebug(CallStack, "UI interaction: WinOffline Explorer")

                    ' Enable log cleanup switch by default
                    Globals.CleanupLogsSwitch = True

                    ' Initialize WinOfflineUI
                    Globals.WinOfflineExplorer = New WinOfflineUI

                    ' Wait for result..
                    If Globals.WinOfflineExplorer.ShowDialog() = Windows.Forms.DialogResult.Abort Or Globals.DumpCazipxpSwitch Then

                        ' Check if user selected resource dump execution
                        If Globals.DumpCazipxpSwitch Then

                            ' Copy Cazipxp to working directory
                            System.IO.File.Copy(Globals.WinOfflineTemp + "\Cazipxp.exe", Globals.WorkingDirectory + "Cazipxp.exe", True)

                        End If

                        ' Perform cleanup (don't generate a patching summary)
                        Init.DeInit(CallStack, True, False)

                        ' Return
                        Return 0

                    End If

                    ' Create the progress gui
                    Globals.ProgressGUI = New ProgressUI()
                    Globals.ProgressGUI.Hide()
                    Globals.ProgressGUI.SetFormTitle(Globals.ProcessFriendlyName + " -- " + Globals.AppVersion)

                    ' Start the progress gui thread
                    Globals.ProgressUIThread = New System.Threading.Thread(AddressOf Globals.ProgressGUI.ShowDialog)
                    Globals.ProgressUIThread.Start()

                    ' Enable debug gui notification icon
                    Globals.ProgressGUI.TrayIcon.Visible = True

                End If

            End If

            ' Call the execution state dispatcher
            Globals.DispatcherReturnCode = Dispatcher(CallStack)

        End If

        ' *****************************
        ' - Check for DIRTY execution flag.
        ' *****************************

        ' Check the dirty flag
        If Globals.DirtyFlag Then

            ' Write debug
            Logger.WriteDebug(CallStack, "Dirty execution flag was set.")

            ' *****************************
            ' - Reset the job output ID.
            ' *****************************

            ' Process the active container work file (.cwf)
            RunLevel = JobContainer(CallStack)

            ' Check the run level
            If RunLevel <> 0 OrElse Globals.JobOutputFile Is Nothing OrElse Globals.JobOutputFile.Equals("") Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Exception caught processing software delivery container file.")

                ' Set job output flag -- job output will be unavailable
                Globals.WriteSDJobOutput = False

            Else

                ' Write debug
                Logger.WriteDebug(CallStack, "Software delivery job output file: " + Globals.JobOutputFile)

                ' Set job output flag
                Globals.WriteSDJobOutput = True

            End If

            ' *****************************
            ' - Reset global flags.
            ' *****************************

            ' Unset DIRTY flag
            Globals.DirtyFlag = False

            ' Reset patch error status
            Globals.PatchErrorDetected = False

        End If

        ' *****************************
        ' - Update logger task based on execution conditions.
        ' *****************************

        ' Set task based on execution type
        If Globals.SDBasedMode AndAlso Globals.StageIICompleted Then

            ' Set current task
            Logger.SetCurrentTask("Waiting for software delivery..")

        ElseIf Globals.SDBasedMode AndAlso Globals.StageICompleted Then

            ' Set current task
            Logger.SetCurrentTask("Waiting for next stage..")

        Else

            ' Set current task
            Logger.SetCurrentTask("Wrapping up..")

        End If

        ' *****************************
        ' - Process dispatcher return code and deinitialize.
        ' *****************************

        ' Check the dispatcher return
        If Globals.DispatcherReturnCode <> 0 Then

            ' Write debug
            Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " completed with an error.")

        Else

            ' Write debug
            Logger.WriteDebug(CallStack, Globals.ProcessFriendlyName + " completed.")

        End If

        ' Check final stage
        If Globals.FinalStage Then

            ' Deinitialize
            Init.DeInit(CallStack, False, True)

        End If

        ' Dispatch debug gui [User execution only]
        If Not Globals.RunningAsSystemIdentity AndAlso Globals.ProgressGUI IsNot Nothing Then

            ' Check execution type
            If Globals.FinalStage Then

                ' Autoclose the debug console [Autoclose = True]
                Globals.ProgressGUI.Delegate_ProgressUI_GoModeless(True)

            Else

                ' Hide the debug console [Autoclose = False]
                Globals.ProgressGUI.Delegate_ProgressUI_GoModeless(False)

            End If

        End If

        ' *****************************
        ' - Switch: Initiate a system reboot.
        ' *****************************

        ' Check for system reboot switch
        If Globals.RebootOnTermination Then

            ' Initiate a reboot before termination
            Utility.SystemReboot(30)

        End If

        ' *****************************
        ' - Terminate program.
        ' *****************************

        ' Check the dispatcher return code
        If Globals.DispatcherReturnCode <> 0 Then

            ' Return
            Return 9

        ElseIf Globals.PatchErrorDetected Then

            ' Return
            Return 10

        Else

            ' Return
            Return 0

        End If

    End Function

End Class
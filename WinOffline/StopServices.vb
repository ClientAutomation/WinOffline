Imports System.ServiceProcess
Imports System.Threading

Partial Public Class WinOffline

    Public Shared Function StopServices(ByVal CallStack As String) As Integer

        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String
        Dim RemainingOutput As String
        Dim PMLAService As ServiceController
        Dim ProcessStartInfo As ProcessStartInfo
        Dim CAMDirectory As String
        Dim ProcessExitCode As Integer
        Dim CAFStopHelperThread As Thread = Nothing
        Dim LoopCounter As Integer = 0
        Dim RunLevel As Integer = 0

        CallStack += "StopServices|"
        Logger.SetCurrentTask("Stopping services..")

        ' Stop health monitoring agent
        Try
            If Utility.IsProcessRunning("hmagent") And Not Globals.SkiphmAgent Then
                Logger.WriteDebug(CallStack, "Health monitoring agent: ACTIVE")

                ExecutionString = Globals.DSMFolder + "bin\hmagent.exe"
                ArgumentString = "stop"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = ExecutionString.Substring(0, ExecutionString.LastIndexOf("\"))
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
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                ProcessExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                Thread.Sleep(1000) ' Just give it a second

                ' Wait up to 5 more seconds after stop request, for actual process termination
                LoopCounter = 0
                While Utility.IsProcessRunning("hmagent")
                    Logger.WriteDebug(CallStack, "Health monitoring agent: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 5 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("hmagent") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("hmagent")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "Health monitoring agent: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "Health monitoring agent: STOPPED")
                End If
            ElseIf Globals.SkiphmAgent Then
                Logger.WriteDebug(CallStack, "Health monitoring agent: BYPASS")
            Else
                Logger.WriteDebug(CallStack, "Health monitoring agent: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught stopping the health monitoring agent.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 1
        End Try

        ' Stop alert collector service
        Try
            If Utility.IsProcessRunning("AlertCollector") And Not Globals.SkiphmAgent Then
                Logger.WriteDebug(CallStack, "Alert collector: ACTIVE")

                ExecutionString = Globals.DSMFolder + "bin\AlertCollector.exe"
                ArgumentString = "stop"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = ExecutionString.Substring(0, ExecutionString.LastIndexOf("\"))
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
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                ProcessExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                Thread.Sleep(1000) ' Just give it a second

                ' Wait up to 5 more seconds after stop request, for actual process termination
                LoopCounter = 0
                While Utility.IsProcessRunning("AlertCollector")
                    Logger.WriteDebug(CallStack, "Alert collector: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 5 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("AlertCollector") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("AlertCollector")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "Alert collector: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "Alert collector: STOPPED")
                End If
            ElseIf Globals.SkiphmAgent Then
                Logger.WriteDebug(CallStack, "Alert collector: BYPASS")
            Else
                Logger.WriteDebug(CallStack, "Alert collector: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught stopping the alert collector service.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 2
        End Try

        ' Soft-termiante software delivery (allow sufficient time to self-terminate)
        Try

            If Utility.IsProcessRunning("sd_jexec") Then
                ' Check every 1 second if sd_jexec has terminated, for up to 30 seconds
                LoopCounter = 0
                While Utility.IsProcessRunning("sd_jexec")
                    If LoopCounter Mod 5 = 0 Then Logger.WriteDebug(CallStack, "Software delivery: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 30 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("sd_jexec") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("sd_jexec")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "Software delivery: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "Software delivery: STOPPED")
                End If
            Else
                Logger.WriteDebug(CallStack, "Software delivery: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught terminating the software delivery plugin.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 3
        End Try

        ' Stop DSM Explorer processes
        Try
            If Utility.IsProcessRunning("egc30n") Then
                Logger.WriteDebug(CallStack, "DSM Explorer: ACTIVE")
                Dim ExplorerProcesses() As Process = Process.GetProcessesByName("EGC30N")
                For Each pProcess As Process In ExplorerProcesses
                    Logger.WriteDebug(CallStack, "Terminate process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                    pProcess.Kill()
                Next
                Logger.WriteDebug(CallStack, "DSM Explorer: TERMINATED")
            Else
                Logger.WriteDebug(CallStack, "DSM Explorer: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught terminating DSM Explorer processes.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 4
        End Try

        ' Stop the dmprimer service
        Try
            If Utility.IsProcessRunning("dm_primer") AndAlso Not Globals.SkipDMPrimer Then
                Logger.WriteDebug(CallStack, "DMPrimer service: ACTIVE")

                If Globals.DMPrimerFolder IsNot Nothing Then
                    ExecutionString = Globals.DMPrimerFolder + "dm_primer.exe"
                    ArgumentString = "stop"
                Else
                    ExecutionString = Utility.GetProcessFileName("dm_primer")
                    ArgumentString = "stop"
                End If

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = ExecutionString.Substring(0, ExecutionString.LastIndexOf("\"))
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
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                ProcessExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                Thread.Sleep(1000) ' Just give it a second

                ' Wait up to 5 more seconds after stop request, for actual process termination
                LoopCounter = 0
                While Utility.IsProcessRunning("dm_primer")
                    Logger.WriteDebug(CallStack, "DMPrimer service: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 5 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("dm_primer") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("dm_primer")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "DMPrimer service: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "DMPrimer service: STOPPED")
                End If
            ElseIf Globals.SkipDMPrimer Then
                Logger.WriteDebug(CallStack, "DMPrimer service: SKIPPED")
            Else
                Logger.WriteDebug(CallStack, "DMPrimer service: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught stopping the dmprimer service.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 5
        End Try

        ' Stop the performance lite agent service
        Try
            If Utility.IsProcessRunning("casplitegent") Then
                Logger.WriteDebug(CallStack, "Performance lite agent: ACTIVE")

                PMLAService = New ServiceController("CASPLiteAgent")
                PMLAService.Stop()

                Thread.Sleep(1000) ' Just give it a second

                ' Wait up to 5 more seconds after stop request, for actual process termination
                LoopCounter = 0
                While Utility.IsProcessRunning("casplitegent")
                    Logger.WriteDebug(CallStack, "Performance lite agent: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 5 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("casplitegent") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("casplitegent")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "Performance lite agent: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "Performance lite agent: STOPPED")
                End If
            Else
                Logger.WriteDebug(CallStack, "Performance lite agent: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught while stopping the performance lite agent.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 6
        End Try

        ' Stop the system tray processes
        Try
            If Utility.IsProcessRunning("cfsystray") Then
                Logger.WriteDebug(CallStack, "System tray: ACTIVE")
                Dim TrayProcesses() As Process = Process.GetProcessesByName("cfsystray")
                For Each pProcess As Process In TrayProcesses
                    Logger.WriteDebug(CallStack, "Terminate process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                    pProcess.Kill()
                Next
                Logger.WriteDebug(CallStack, "System tray: TERMINATED")
            Else
                Logger.WriteDebug(CallStack, "System tray: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught stopping the system tray process.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 7
        End Try

        ' Refresh the notification area
        Try
            If Not Globals.RunningAsSystemIdentity Then
                WindowsAPI.RefreshNotificationArea()
                Logger.WriteDebug(CallStack, "Notification area refreshed.")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught refreshing notification tray.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
        End Try

        ' Stop external CAF processes
        Utility.KillProcess("sd_jexec")
        Utility.KillProcess("dsmdiag")
        Utility.KillProcess("cmDirEngJob")
        Utility.KillProcess("ACServer")
        Utility.KillProcess("amSvpCmd")
        Utility.KillProcess("amswsigscan")
        Utility.KillProcess("amsoftscan")
        Utility.KillProcess("cfCafDialog")
        Utility.KillProcess("dmscript")
        Utility.KillProcess("ContentUtility")
        Utility.KillProcess("EGC30N")
        Utility.KillProcess("cfsystray")
        Utility.KillProcessByPath("javaw.exe", "\CIC\jre\")
        Utility.KillProcessByCommandLine("iexplore.exe", "Embedding")
        Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService_HM")
        Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService")
        Utility.KillProcessByCommandLine("w3wp.exe", "ITCM_Application_Pool")

        ' Stop the CAF service
        Try
            If Utility.IsProcessRunning("caf") Then
                Logger.WriteDebug(CallStack, "CAF service: ACTIVE")

                ExecutionString = Globals.DSMFolder + "bin\caf.exe"
                ArgumentString = "stop"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                ProcessStartInfo.UseShellExecute = False
                ProcessStartInfo.RedirectStandardOutput = True
                ProcessStartInfo.CreateNoWindow = True
                StandardOutput = ""
                RemainingOutput = ""
                Logger.WriteDebug("------------------------------------------------------------")

                RunningProcess = Process.Start(ProcessStartInfo)

                ' If CAF reaches the 260 second countdown, deploy the helper-thread
                While RunningProcess.HasExited = False
                    ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                    Logger.WriteDebug(ConsoleOutput)
                    StandardOutput += ConsoleOutput + Environment.NewLine
                    If ConsoleOutput IsNot Nothing AndAlso ConsoleOutput.ToLower.Contains(" 260 ") Then
                        CAFStopHelperThread = New Thread(Sub() CafStopWorker())
                        CAFStopHelperThread.Start()
                    End If
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                ProcessExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                ' Join helper-thread (wait for thread to terminate)
                Try
                    If CAFStopHelperThread IsNot Nothing AndAlso CAFStopHelperThread.IsAlive Then
                        Logger.WriteDebug(CallStack, "Waiting for helper thread to terminate.")
                        CAFStopHelperThread.Join()
                    End If
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Warning: Exception caught while joining with helper thread.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                End Try

                If ProcessExitCode = 0 Then
                    Thread.Sleep(1000) ' Just give it a second

                    ' Wait up to 12 more seconds after successful "caf stop" completion, for CAF to actually stop
                    LoopCounter = 0
                    While Utility.IsProcessRunning("caf", True)
                        Logger.WriteDebug(CallStack, "CAF service: ACTIVE")
                        LoopCounter += 1
                        Thread.Sleep(3000)
                        If LoopCounter >= 4 Then Exit While
                    End While

                    ' Otherwise kill CAF
                    If Utility.IsProcessRunning("caf", True) Then KillCAF(CallStack)

                    ' We tried diligently
                    If Utility.IsProcessRunning("caf", True) Then
                        Logger.WriteDebug(CallStack, "Error: Unable to terminate CAF service.")
                        Manifest.UpdateManifest(CallStack,
                                                Manifest.EXCEPTION_MANIFEST,
                                                {"Error: Unable to terminate CAF service.",
                                                "Reason: Access to the CAF service was denied."})
                        Return 8
                    End If
                Else
                    If StandardOutput.ToLower.Contains("access is denied") Then
                        Logger.WriteDebug(CallStack, "Administrator unable to stop CAF service.")

                        RunLevel = LaunchPad(CallStack, "System", Globals.DSMFolder + "bin\caf.exe", Globals.DSMFolder + "bin", "stop")

                        LoopCounter = 0
                        While Utility.IsProcessRunning("caf.exe", "stop") OrElse Utility.IsProcessRunning("caf", True)
                            If LoopCounter = 0 Then
                                Logger.WriteDebug(CallStack, "CAF service: STOPPING  [This may take up to 5 minutes!]")
                            Else
                                Logger.WriteDebug(CallStack, "CAF service: STOPPING")
                            End If
                            LoopCounter += 1
                            If LoopCounter >= 12 Then ' Wait 120s before killiing CAF
                                KillCAF(CallStack)
                                Exit While
                            End If
                            Thread.Sleep(10000)
                        End While

                        If Utility.IsProcessRunning("caf", True) Then
                            Logger.WriteDebug(CallStack, "Error: Unable to terminate CAF service.")
                            Manifest.UpdateManifest(CallStack,
                                                    Manifest.EXCEPTION_MANIFEST,
                                                    {"Error: Unable to terminate CAF service.",
                                                    "Reason: Access to the CAF service was denied."})
                            Return 9
                        End If
                    Else
                        Logger.WriteDebug(CallStack, "Warning: ""caf stop"" terminated abnormally.")

                        ' Escalate to caf-kill
                        If Utility.IsProcessRunning("caf", True) Then KillCAF(CallStack)

                        ' Check for success
                        If Utility.IsProcessRunning("caf", True) Then
                            Logger.WriteDebug(CallStack, "Error: Unable to terminate CAF service.")
                            Manifest.UpdateManifest(CallStack,
                                                    Manifest.EXCEPTION_MANIFEST,
                                                    {"Error: Unable to terminate CAF service.",
                                                    "Reason: Access to the CAF service was denied."})
                            Return 10
                        End If
                    End If
                End If
                Logger.WriteDebug(CallStack, "CAF service: STOPPED")
            Else
                Logger.WriteDebug(CallStack, "CAF service: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught while stopping the CAF service.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 9
        End Try

        ' Stop external CAF processes
        Utility.KillProcess("tngdta")

        ' Stop the CAM service
        Try
            If Utility.IsProcessRunning("cam") AndAlso Not Globals.SkipCAM Then
                Logger.WriteDebug(CallStack, "CAM service: ACTIVE")

                CAMDirectory = Environment.GetEnvironmentVariable("CAI_MSQ")
                ExecutionString = CAMDirectory + "\bin\cam.exe"
                ArgumentString = "change disabled"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = CAMDirectory + "\bin"
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
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                RunningProcess.Close()

                Logger.WriteDebug(CallStack, "CAM service has been disabled.")

                ExecutionString = CAMDirectory + "\bin\camclose.exe"
                ArgumentString = ""

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = CAMDirectory + "\bin"
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
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                ProcessExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                Thread.Sleep(1000) ' Just give it a second

                ' Wait up to 5 more seconds after stop request, for actual process termination
                LoopCounter = 0
                While Utility.IsProcessRunning("cam")
                    Logger.WriteDebug(CallStack, "CAM service: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 5 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("cam") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("cam")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "CAM service: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "CAM service: STOPPED")
                End If
            ElseIf Globals.SkipCAM Then
                Logger.WriteDebug(CallStack, "CAM service: SKIPPED")
            Else
                Logger.WriteDebug(CallStack, "CAM service: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught while closing the CAM service.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            ' Don't return because caf is stopped.
        End Try

        ' Stop CSAM service
        Try
            If Utility.IsProcessRunning("csampmux") AndAlso Not Globals.SkipCAM Then
                Logger.WriteDebug(CallStack, "Port multiplexer service: ACTIVE")

                ExecutionString = Globals.SSAFolder + "bin\csampmux.exe"
                ArgumentString = "stop"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = Globals.SSAFolder + "bin"
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
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                ProcessExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                Thread.Sleep(1000) ' Just give it a second

                ' Wait up to 5 more seconds after stop request, for actual process termination
                LoopCounter = 0
                While Utility.IsProcessRunning("csampmux")
                    Logger.WriteDebug(CallStack, "Port multiplexer service: ACTIVE")
                    LoopCounter += 1
                    Thread.Sleep(1000)
                    If LoopCounter >= 5 Then Exit While
                End While

                ' Terminate process, as necessary
                If Utility.IsProcessRunning("csampmux") Then
                    Dim KillProcess() As Process = Process.GetProcessesByName("csampmux")
                    For Each pProcess As Process In KillProcess
                        Logger.WriteDebug(CallStack, "Kill process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                        pProcess.Kill()
                    Next
                    Logger.WriteDebug(CallStack, "Port multiplexer service: TERMINATED")
                Else
                    Logger.WriteDebug(CallStack, "Port multiplexer service: STOPPED")
                End If
            Else
                Logger.WriteDebug(CallStack, "Port multiplexer service: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught while stopping the port multiplexer service.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            ' Don't return because caf is stopped.
        End Try

        Return RunLevel

    End Function

    Private Shared Sub KillCAF(ByVal CallStack As String)

        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String
        Dim RemainingOutput As String

        Logger.WriteDebug(CallStack, "CAF service: KILL")

        ExecutionString = Globals.DSMFolder + "bin\caf.exe"
        ArgumentString = "kill all"

        Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
        ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
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
        End While

        RunningProcess.WaitForExit()
        RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
        StandardOutput += RemainingOutput

        Logger.WriteDebug(RemainingOutput)
        Logger.WriteDebug("------------------------------------------------------------")
        Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
        RunningProcess.Close()

        Thread.Sleep(3000)

        Utility.KillProcess("caf")

    End Sub

    Public Shared Sub CafStopWorker()

        Dim CafProcessList As New ArrayList
        Dim LoopCounter As Integer = 0
        Dim ChildProcess As ArrayList
        Dim WorkingMemoryBase As Double
        Dim WorkingMemoryCurrent As Double

        ' CafProcessList is an ArrayList with each element containing an Arraylist with the following contents:
        ' (0) = ProcessID, Ex: "1234"
        ' (1) = Name, Ex: "cmEngine.exe"
        ' (2) = ExecutablePath, Ex: "C:\Program Files (x86)\CA\DSM\Bin\cmEngine.exe"
        ' (3) = CommandLine, Ex: "C:\Program Files (x86)\CA\DSM\Bin\cmEngine.exe -name SystemEngine"
        ' (4) = WorkingSetSize, in bytes, Ex: 1234567

        Try
            Logger.WriteDebug("Helper thread: Expediting ""caf stop"" operation..")

            While Utility.IsProcessRunning("caf.exe", "stop")
                CafProcessList = Utility.GetProcessChildren("caf.exe", "service", New ArrayList({"cfsmsmd.exe", "ccnfagent.exe", "encclient.exe"}))

                If CafProcessList.Count = 0 Then
                    Logger.WriteDebug("Helper thread: Finished.")
                    Return
                End If

                Logger.WriteDebug("Helper thread: Monitoring (" + CafProcessList.Count.ToString + ") processes.")
                LoopCounter = 0

                While LoopCounter < 50
                    If Not Utility.IsProcessRunning("caf.exe", "stop") Then
                        Logger.WriteDebug("Helper thread: Finished.")
                        Return
                    End If
                    LoopCounter += 1
                    Thread.Sleep(50)
                End While

                For x As Integer = CafProcessList.Count - 1 To 0 Step -1
                    ChildProcess = CafProcessList(x)
                    Logger.WriteDebug("Helper thread: Analyzing PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)

                    If Not Utility.IsProcessRunning(Integer.Parse(ChildProcess.Item(0))) Then
                        Logger.WriteDebug("Helper thread: Self-terminated PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                        CafProcessList.RemoveAt(x)
                        Continue For
                    End If

                    WorkingMemoryBase = Double.Parse(ChildProcess.Item(4))
                    WorkingMemoryCurrent = Utility.GetProcessWorkingSetMemorySize(ChildProcess.Item(0))

                    If WorkingMemoryCurrent >= WorkingMemoryBase Then
                        Utility.KillProcess(Integer.Parse(ChildProcess.Item(0)))
                        CafProcessList.RemoveAt(x)
                        Logger.WriteDebug("Helper thread: Killed PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                        Continue For
                    End If

                    Logger.WriteDebug("Helper thread: Memory decrease PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                Next
            End While
            Logger.WriteDebug("Helper thread: Finished.")
        Catch ex As Exception
            Logger.WriteDebug("Helper thread: Exception caught monitoring caf.exe child processes.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
        End Try

    End Sub

    Public Shared Function StopCAFOnDemand(ByVal CallStack As String) As Integer

        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String
        Dim RemainingOutput As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim CAFStopExitCode As Integer
        Dim CAFStopHelperThread As Thread = Nothing
        Dim LoopCounter As Integer = 0
        Dim RestCounter As Integer = 0

        CallStack += "StopCAF|"

        ' Check CAF service status
        If Not Utility.ServiceExists("caf") Then
            Logger.WriteDebug(CallStack, "CAF service: NOT INSTALLED")
            Return 0
        End If
        If Not Utility.IsServiceEnabled("caf") Then
            Logger.WriteDebug(CallStack, "CAF service: DISABLED")
            Return 0
        End If
        If Not Utility.IsProcessRunning("caf") Then
            Logger.WriteDebug(CallStack, "CAF service: ALREADY STOPPED")
            Return 0
        End If

        ' Stop DSM Explorer processes
        Try
            If Utility.IsProcessRunning("egc30n") Then
                Logger.WriteDebug(CallStack, "DSM Explorer: ACTIVE")
                Dim ExplorerProcesses() As Process = System.Diagnostics.Process.GetProcessesByName("EGC30N")
                For Each pProcess As Process In ExplorerProcesses
                    Logger.WriteDebug(CallStack, "Terminate process: " + pProcess.ProcessName.ToString + "," + pProcess.Id.ToString)
                    pProcess.Kill()
                Next
                Logger.WriteDebug(CallStack, "DSM Explorer: TERMINATED")
            Else
                Logger.WriteDebug(CallStack, "DSM Explorer: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught terminating DSM Explorer processes.")
            Logger.WriteDebug(ex.Message)
        End Try

        ' Stop external CAF processes
        Utility.KillProcess("sd_jexec")
        Utility.KillProcess("dsmdiag")
        Utility.KillProcess("cmDirEngJob")
        Utility.KillProcess("ACServer")
        Utility.KillProcess("amSvpCmd")
        Utility.KillProcess("amswsigscan")
        Utility.KillProcess("amsoftscan")
        Utility.KillProcess("cfCafDialog")
        Utility.KillProcess("dmscript")
        Utility.KillProcess("ContentUtility")
        Utility.KillProcess("EGC30N")
        Utility.KillProcessByPath("javaw.exe", "\CIC\jre\")
        Utility.KillProcessByCommandLine("iexplore.exe", "Embedding")
        Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService_HM")
        Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService")
        Utility.KillProcessByCommandLine("w3wp.exe", "ITCM_Application_Pool")

        ' Stop the CAF service
        Try
            If Utility.IsProcessRunning("caf") Then
                Logger.WriteDebug(CallStack, "CAF service: ACTIVE")

                ExecutionString = Globals.DSMFolder + "bin\caf.exe"
                ArgumentString = "stop"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                ProcessStartInfo.UseShellExecute = False
                ProcessStartInfo.RedirectStandardOutput = True
                ProcessStartInfo.CreateNoWindow = True
                StandardOutput = ""
                RemainingOutput = ""
                Logger.WriteDebug("------------------------------------------------------------")

                RunningProcess = Process.Start(ProcessStartInfo)

                ' If CAF reaches the 260 second countdown, deploy the helper-thread
                While RunningProcess.HasExited = False
                    ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                    Logger.WriteDebug(ConsoleOutput)
                    StandardOutput += ConsoleOutput + Environment.NewLine
                    If ConsoleOutput IsNot Nothing AndAlso ConsoleOutput.ToLower.Contains(" 260 ") Then
                        CAFStopHelperThread = New Thread(Sub() CafStopWorker())
                        CAFStopHelperThread.Start()
                    End If
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput

                Logger.WriteDebug(RemainingOutput)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                CAFStopExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                ' Join helper-thread (wait for thread to terminate)
                Try
                    If CAFStopHelperThread IsNot Nothing AndAlso CAFStopHelperThread.IsAlive Then
                        Logger.WriteDebug(CallStack, "Waiting for helper thread to terminate.")
                        CAFStopHelperThread.Join()
                    End If
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Warning: Exception caught while joining with helper thread.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                End Try

                If CAFStopExitCode = 0 Then
                    Thread.Sleep(1000) ' Just give it a second

                    ' Wait up to 12 more seconds after successful "caf stop" completion, for CAF to actually stop
                    LoopCounter = 0
                    While Utility.IsProcessRunning("caf", True)
                        Logger.WriteDebug(CallStack, "CAF service: ACTIVE")
                        LoopCounter += 1
                        Thread.Sleep(3000)
                        If LoopCounter >= 4 Then Exit While
                    End While

                    ' Otherwise kill CAF
                    If Utility.IsProcessRunning("caf", True) Then KillCAF(CallStack)

                    ' We tried diligently
                    If Utility.IsProcessRunning("caf") Then
                        Logger.WriteDebug(CallStack, "Error: Unable to terminate CAF service.")
                        Return 1
                    End If
                Else
                    If StandardOutput.ToLower.Contains("access is denied") Then
                        Logger.WriteDebug(CallStack, "Administrator unable to stop CAF service.")

                        LaunchPad(CallStack, "System", Globals.DSMFolder + "bin\caf.exe", Globals.DSMFolder + "bin\", "stop")

                        LoopCounter = 0
                        While Utility.IsProcessRunning("caf.exe", "stop") OrElse Utility.IsProcessRunning("caf", True)
                            If LoopCounter = 0 Then
                                Logger.WriteDebug(CallStack, "CAF service: STOPPING  [This may take up to 5 minutes!]")
                            Else
                                Logger.WriteDebug(CallStack, "CAF service: STOPPING")
                            End If
                            LoopCounter += 1
                            If LoopCounter >= 12 Then ' Wait 120s before killiing CAF
                                KillCAF(CallStack)
                                Exit While
                            End If
                            Thread.Sleep(10000)
                        End While

                        If Utility.IsProcessRunning("caf") Then
                            Logger.WriteDebug(CallStack, "Error: Unable to terminate CAF service.")
                            Return 2
                        End If
                    Else
                        Logger.WriteDebug(CallStack, "Warning: ""caf stop"" terminated abnormally.")

                        ' Escalate to caf-kill
                        If Utility.IsProcessRunning("caf", True) Then KillCAF(CallStack)

                        ' Check for success
                        If Utility.IsProcessRunning("caf", True) Then
                            Logger.WriteDebug(CallStack, "Error: Unable to terminate CAF service.")
                            Return 3
                        End If
                    End If
                End If
                Logger.WriteDebug(CallStack, "CAF service: STOPPED")
            Else
                Logger.WriteDebug(CallStack, "CAF service: INACTIVE")
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught while stopping the CAF service.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Return 4
        End Try

        ' Stop external CAF processes
        Utility.KillProcess("tngdta")

        ' Clear notification server cache data
        Utility.DeleteFile(CallStack, Globals.DSMFolder + "appdata\cfnotsrvd.dat")

        Return 0

    End Function

End Class
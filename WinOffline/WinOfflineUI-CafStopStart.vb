Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Sub StartCAF(ByVal UIOutputControl As Control)

        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String = ""
        Dim RemainingOutput As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim LoopCounter As Integer = 0

        Try
            If Not WinOffline.Utility.ServiceExists("caf") Then
                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: NOT INSTALLED")
                Return
            End If

            If Not WinOffline.Utility.IsServiceEnabled("caf") Then
                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: DISABLED")
                Return
            End If

            If WinOffline.Utility.IsProcessRunning("caf") Then
                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: ALREADY RUNNING")
                Return
            End If

            ExecutionString = Globals.DSMFolder + "bin\caf.exe"
            ArgumentString = "start"

            Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString + Environment.NewLine)
            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True
            StandardOutput = ""
            RemainingOutput = ""

            RunningProcess = Process.Start(ProcessStartInfo)

            While RunningProcess.HasExited = False
                ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                Delegate_Sub_Append_Text(UIOutputControl, ConsoleOutput)
                StandardOutput += ConsoleOutput + Environment.NewLine
                Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
            End While

            RunningProcess.WaitForExit()
            RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
            StandardOutput += RemainingOutput
            Delegate_Sub_Append_Text(UIOutputControl, RemainingOutput + Environment.NewLine)
            Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString + Environment.NewLine)

            RunningProcess.Close()

        Catch ex As Exception
            Delegate_Sub_Append_Text(UIOutputControl, "Error: Exception caught while starting CAF service." + Environment.NewLine)
            Delegate_Sub_Append_Text(UIOutputControl, ex.Message)
            Delegate_Sub_Append_Text(UIOutputControl, ex.StackTrace)
            Return
        End Try

        Delegate_Sub_Append_Text(UIOutputControl, "CAF service: STARTUP COMPLETE" + Environment.NewLine)

    End Sub

    Private Sub StopCAF(ByVal UIOutputControl As Control)

        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim ConsoleOutput As String
        Dim StandardOutput As String
        Dim RemainingOutput As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim CAFExitCode As Integer
        Dim CAFStopHelperThread As Thread = Nothing
        Dim LoopCounter As Integer = 0
        Dim RestCounter As Integer = 0

        Try
            If Not WinOffline.Utility.ServiceExists("caf") Then
                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: NOT INSTALLED")
                Return
            End If

            If Not WinOffline.Utility.IsServiceEnabled("caf") Then
                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: DISABLED")
                Return
            End If

            If Not WinOffline.Utility.IsProcessRunning("caf") Then
                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: ALREADY STOPPED")
                Return
            End If

            WinOffline.Utility.KillProcess("sd_jexec")
            WinOffline.Utility.KillProcess("dsmdiag")
            WinOffline.Utility.KillProcess("cmDirEngJob")
            WinOffline.Utility.KillProcess("ACServer")
            WinOffline.Utility.KillProcess("amSvpCmd")
            WinOffline.Utility.KillProcess("amswsigscan")
            WinOffline.Utility.KillProcess("amsoftscan")
            WinOffline.Utility.KillProcess("cfCafDialog")
            WinOffline.Utility.KillProcess("dmscript")
            WinOffline.Utility.KillProcess("ContentUtility")
            WinOffline.Utility.KillProcess("EGC30N")
            WinOffline.Utility.KillProcessByPath("javaw.exe", "\CIC\jre\")
            WinOffline.Utility.KillProcessByCommandLine("iexplore.exe", "Embedding")
            WinOffline.Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService_HM")
            WinOffline.Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService")
            WinOffline.Utility.KillProcessByCommandLine("w3wp.exe", "ITCM_Application_Pool")

            If WinOffline.Utility.IsProcessRunning("caf") Then

                ExecutionString = Globals.DSMFolder + "bin\caf.exe"
                ArgumentString = "stop"

                Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString + Environment.NewLine)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                ProcessStartInfo.UseShellExecute = False
                ProcessStartInfo.RedirectStandardOutput = True
                ProcessStartInfo.CreateNoWindow = True
                StandardOutput = ""
                RemainingOutput = ""

                RunningProcess = Process.Start(ProcessStartInfo)

                While RunningProcess.HasExited = False
                    ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                    Delegate_Sub_Append_Text(UIOutputControl, ConsoleOutput)
                    StandardOutput += ConsoleOutput + Environment.NewLine
                    If ConsoleOutput IsNot Nothing AndAlso ConsoleOutput.ToLower.Contains(" 260 ") Then
                        CAFStopHelperThread = New Thread(Sub() CafStopWorker(UIOutputControl))
                        CAFStopHelperThread.Start()
                    End If
                    Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                End While

                RunningProcess.WaitForExit()
                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                StandardOutput += RemainingOutput
                Delegate_Sub_Append_Text(UIOutputControl, RemainingOutput + Environment.NewLine)
                Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString + Environment.NewLine)
                CAFExitCode = RunningProcess.ExitCode
                RunningProcess.Close()

                Try
                    If CAFStopHelperThread IsNot Nothing AndAlso CAFStopHelperThread.IsAlive Then
                        Delegate_Sub_Append_Text(UIOutputControl, "Waiting for helper thread to terminate.")
                        CAFStopHelperThread.Join()
                    End If
                Catch ex As Exception
                    Delegate_Sub_Append_Text(UIOutputControl, "Warning: Exception caught while joining with helper thread.")
                    Delegate_Sub_Append_Text(UIOutputControl, ex.Message)
                    Delegate_Sub_Append_Text(UIOutputControl, ex.StackTrace)
                End Try

                If CAFExitCode <> 0 Then
                    If StandardOutput.ToLower.Contains("access is denied") Then
                        Delegate_Sub_Append_Text(UIOutputControl, "Administrator unable to stop CAF service.")

                        LaunchPadUI(UIOutputControl, "System", Globals.DSMFolder + "bin\caf.exe", "stop")

                        LoopCounter = 0

                        While WinOffline.Utility.IsProcessRunning("caf.exe", "stop") Or WinOffline.Utility.IsProcessRunning("caf")
                            If LoopCounter = 0 Then
                                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: STOPPING  [This may take up to 5 minutes!]")
                            Else
                                Delegate_Sub_Append_Text(UIOutputControl, "CAF service: STOPPING")
                            End If
                            LoopCounter += 1
                            If LoopCounter >= 10 Then
                                Delegate_Sub_Append_Text(UIOutputControl, "CAF service is not stopping gracefully.")
                                Delegate_Sub_Append_Text(UIOutputControl, "Send first kill request to the CAF service..")

                                ExecutionString = Globals.DSMFolder + "bin\caf.exe"
                                ArgumentString = "kill all"

                                Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString + Environment.NewLine)
                                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                                ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                                ProcessStartInfo.UseShellExecute = False
                                ProcessStartInfo.RedirectStandardOutput = True
                                ProcessStartInfo.CreateNoWindow = True
                                StandardOutput = ""
                                RemainingOutput = ""

                                RunningProcess = Process.Start(ProcessStartInfo)

                                While RunningProcess.HasExited = False
                                    ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                                    Delegate_Sub_Append_Text(UIOutputControl, ConsoleOutput)
                                    StandardOutput += ConsoleOutput + Environment.NewLine
                                    Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                                End While

                                RunningProcess.WaitForExit()
                                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                                StandardOutput += RemainingOutput
                                Delegate_Sub_Append_Text(UIOutputControl, RemainingOutput + Environment.NewLine)
                                Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString + Environment.NewLine)
                                RunningProcess.Close()

                                Delegate_Sub_Append_Text(UIOutputControl, "Send second kill request to the CAF service..")

                                ExecutionString = Globals.DSMFolder + "bin\caf.exe"
                                ArgumentString = "kill all"

                                Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString + Environment.NewLine)
                                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                                ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                                ProcessStartInfo.UseShellExecute = False
                                ProcessStartInfo.RedirectStandardOutput = True
                                ProcessStartInfo.CreateNoWindow = True
                                StandardOutput = ""
                                RemainingOutput = ""

                                RunningProcess = Process.Start(ProcessStartInfo)

                                While RunningProcess.HasExited = False
                                    ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                                    Delegate_Sub_Append_Text(UIOutputControl, ConsoleOutput)
                                    StandardOutput += ConsoleOutput + Environment.NewLine
                                    Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                                End While

                                RunningProcess.WaitForExit()
                                RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                                StandardOutput += RemainingOutput
                                Delegate_Sub_Append_Text(UIOutputControl, RemainingOutput + Environment.NewLine)
                                Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString + Environment.NewLine)
                                RunningProcess.Close()

                                Exit While
                            End If

                            RestCounter = 0

                            While RestCounter < 199
                                If TerminateSignal Then Return
                                RestCounter += 1
                                Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                            End While

                        End While

                        If WinOffline.Utility.IsProcessRunning("caf") Then
                            Delegate_Sub_Append_Text(UIOutputControl, "Error: Unable to terminate CAF service." + Environment.NewLine)
                            Return
                        End If

                    Else
                        Delegate_Sub_Append_Text(UIOutputControl, "CAF service: WARNING [Terminated with non-zero exit code]" + Environment.NewLine)
                    End If

                Else
                    Thread.Sleep(5000)

                    If WinOffline.Utility.IsProcessRunning("caf") Then
                        Delegate_Sub_Append_Text(UIOutputControl, "CAF service has not stopped gracefully.")
                        Delegate_Sub_Append_Text(UIOutputControl, "Send kill request to the CAF service..")

                        ExecutionString = Globals.DSMFolder + "bin\caf.exe"
                        ArgumentString = "kill all"

                        Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString + Environment.NewLine)
                        ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                        ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                        ProcessStartInfo.UseShellExecute = False
                        ProcessStartInfo.RedirectStandardOutput = True
                        ProcessStartInfo.CreateNoWindow = True
                        StandardOutput = ""
                        RemainingOutput = ""

                        RunningProcess = Process.Start(ProcessStartInfo)

                        While RunningProcess.HasExited = False
                            ConsoleOutput = RunningProcess.StandardOutput.ReadLine
                            Delegate_Sub_Append_Text(UIOutputControl, ConsoleOutput)
                            StandardOutput += ConsoleOutput + Environment.NewLine
                            Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                        End While

                        RunningProcess.WaitForExit()
                        RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
                        StandardOutput += RemainingOutput
                        Delegate_Sub_Append_Text(UIOutputControl, RemainingOutput + Environment.NewLine)
                        Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString + Environment.NewLine)
                        RunningProcess.Close()
                    End If

                    If WinOffline.Utility.IsProcessRunning("caf") Then
                        Delegate_Sub_Append_Text(UIOutputControl, "Error: Unable to terminate CAF service." + Environment.NewLine)
                        Return
                    End If
                End If
            End If
        Catch ex As Exception
            Delegate_Sub_Append_Text(UIOutputControl, "Error: Exception caught while stopping the CAF service." + Environment.NewLine)
            Delegate_Sub_Append_Text(UIOutputControl, ex.Message)
            Delegate_Sub_Append_Text(UIOutputControl, ex.StackTrace)
            Return
        End Try

        ' Stop external CAF processes
        WinOffline.Utility.KillProcess("tngdta")

        ' Clear notification server cache data
        Try
            If System.IO.File.Exists(Globals.DSMFolder + "appdata\cfnotsrvd.dat") Then
                Delegate_Sub_Append_Text(UIOutputControl, "Delete file: " + Globals.DSMFolder + "appdata\cfnotsrvd.dat" + Environment.NewLine)
                System.IO.File.Delete(Globals.DSMFolder + "appdata\cfnotsrvd.dat")
            End If
        Catch ex As Exception
            Delegate_Sub_Append_Text(UIOutputControl, "Exception caught cleaning up notification server data file.")
            Delegate_Sub_Append_Text(UIOutputControl, ex.Message)
            Delegate_Sub_Append_Text(UIOutputControl, ex.StackTrace)
        End Try

        Delegate_Sub_Append_Text(UIOutputControl, "CAF service: STOPPED" + Environment.NewLine)

    End Sub

    Private Sub CafStopWorker(ByVal UIOutputControl As Control)

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
            Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Expediting ""caf stop"" operation..")

            While WinOffline.Utility.IsProcessRunning("caf.exe", "stop")
                CafProcessList = WinOffline.Utility.GetProcessChildren("caf.exe", "service", New ArrayList({"cfsmsmd.exe", "ccnfagent.exe", "encclient.exe"}))

                If CafProcessList.Count = 0 Then
                    Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Finished.")
                    Return
                End If

                Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Monitoring (" + CafProcessList.Count.ToString + ") processes.")
                LoopCounter = 0

                While LoopCounter < 50
                    If Not WinOffline.Utility.IsProcessRunning("caf.exe", "stop") Then
                        Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Finished.")
                        Return
                    End If
                    LoopCounter += 1
                    Thread.Sleep(Globals.THREAD_REST_INTERVAL)
                End While

                For x As Integer = CafProcessList.Count - 1 To 0 Step -1
                    ChildProcess = CafProcessList(x)
                    Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Analyzing PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                    If Not WinOffline.Utility.IsProcessRunning(Integer.Parse(ChildProcess.Item(0))) Then
                        Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Self-terminated PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                        CafProcessList.RemoveAt(x)
                        Continue For
                    End If

                    WorkingMemoryBase = Double.Parse(ChildProcess.Item(4))
                    WorkingMemoryCurrent = WinOffline.Utility.GetProcessWorkingSetMemorySize(ChildProcess.Item(0))

                    If WorkingMemoryCurrent >= WorkingMemoryBase Then
                        WinOffline.Utility.KillProcess(Integer.Parse(ChildProcess.Item(0)))
                        CafProcessList.RemoveAt(x)
                        Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Killed PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                        Continue For
                    End If

                    Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Memory decreasing PID " + ChildProcess.Item(0).ToString + " -- " + ChildProcess.Item(1).ToString)
                Next
            End While

            Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Finished.")
        Catch ex As Exception
            Delegate_Sub_Append_Text(UIOutputControl, "Helper thread: Exception caught monitoring caf.exe child processes.")
            Delegate_Sub_Append_Text(UIOutputControl, ex.Message)
            Delegate_Sub_Append_Text(UIOutputControl, ex.StackTrace)
        End Try

    End Sub

End Class
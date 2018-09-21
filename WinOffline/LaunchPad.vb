Imports System.ServiceProcess

Partial Public Class WinOffline

    Public Shared Function LaunchPad(ByVal CallStack As String,
                                     ByVal LaunchContext As String,
                                     ByVal AppToLaunch As String,
                                     Optional ByVal WorkingDirectory As String = "",
                                     Optional ByVal AppArguments As String = "") As Integer

        Dim LaunchControl As ServiceController
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process

        CallStack += "LaunchPad|"

        ' Launch pre-checks
        If Utility.ServiceExists(Globals.ProcessFriendlyName + " Launch Service") Then
            LaunchControl = New ServiceController(Globals.ProcessFriendlyName + " Launch Service")

            ExecutionString = "sc.exe"
            ArgumentString = "delete """ + Globals.ProcessFriendlyName + " Launch Service"""

            Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd)
            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)

            If (RunningProcess.ExitCode <> 0) Then
                RunningProcess.Close()
                Return 1
            Else
                RunningProcess.Close()
            End If
        End If

        ' Verify executable availability
        If Not System.IO.File.Exists(Globals.WinOfflineTemp + "\LaunchService.exe") Then
            Logger.WriteDebug(CallStack, "Error: Launch service executable not found.")
            Return 2
        End If

        If WorkingDirectory Is Nothing OrElse WorkingDirectory.Equals("") Then WorkingDirectory = Globals.WorkingDirectory
        Logger.WriteDebug(CallStack, "Launch Context: " + LaunchContext)
        Logger.WriteDebug(CallStack, "Application: " + AppToLaunch)
        Logger.WriteDebug(CallStack, "Working Directory: " + WorkingDirectory)
        Logger.WriteDebug(CallStack, "Arguments: " + AppArguments)

        ' Install the Launch Service
        ExecutionString = "sc.exe"
        ArgumentString = "create """ + Globals.ProcessFriendlyName + " Launch Service"" binPath= """ +
                Globals.WinOfflineTemp + "\LaunchService.exe"" " +
                "start= demand"

        Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
        ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
        ProcessStartInfo.UseShellExecute = False
        ProcessStartInfo.RedirectStandardOutput = True
        ProcessStartInfo.CreateNoWindow = True

        RunningProcess = Process.Start(ProcessStartInfo)
        RunningProcess.WaitForExit()

        Logger.WriteDebug("------------------------------------------------------------")
        Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd)
        Logger.WriteDebug("------------------------------------------------------------")
        Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)

        If (RunningProcess.ExitCode <> 0) Then
            RunningProcess.Close()
            Return 3
        Else
            RunningProcess.Close()
        End If

        ' Start the Launch Service
        LaunchControl = New ServiceController(Globals.ProcessFriendlyName + " Launch Service")
        Logger.WriteDebug(CallStack, "Launching..")
        If AppArguments Is Nothing OrElse AppArguments.Equals("") Then
            LaunchControl.Start({LaunchContext, AppToLaunch, WorkingDirectory})
        Else
            LaunchControl.Start({LaunchContext, AppToLaunch, WorkingDirectory, AppArguments})
        End If
        LaunchControl.WaitForStatus(ServiceControllerStatus.Stopped)
        Logger.WriteDebug(CallStack, "Mission completed.")

        ' Delete the Launcher Service
        ExecutionString = "sc.exe"
        ArgumentString = "delete """ + Globals.ProcessFriendlyName + " Launch Service"""

        Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
        ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
        ProcessStartInfo.UseShellExecute = False
        ProcessStartInfo.RedirectStandardOutput = True
        ProcessStartInfo.CreateNoWindow = True

        RunningProcess = Process.Start(ProcessStartInfo)
        RunningProcess.WaitForExit()

        Logger.WriteDebug("------------------------------------------------------------")
        Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd)
        Logger.WriteDebug("------------------------------------------------------------")
        Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
        RunningProcess.Close()

        Return 0

    End Function

End Class
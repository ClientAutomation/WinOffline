Imports System.ServiceProcess
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Public Sub LaunchPadUI(ByVal UIOutputControl As Control, ByVal LaunchContext As String, ByVal AppToLaunch As String, Optional ByVal AppArguments As String = "")

        Dim LaunchControl As ServiceController
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim ProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process

        Delegate_Sub_Append_Text(UIOutputControl, "Launch Context: " + LaunchContext)
        Delegate_Sub_Append_Text(UIOutputControl, "Application: " + AppToLaunch)
        Delegate_Sub_Append_Text(UIOutputControl, "Arguments: " + AppArguments)

        ' Launch Service pre-check
        If WinOffline.Utility.ServiceExists(Globals.ProcessFriendlyName + " Launch Service") Then
            LaunchControl = New ServiceController(Globals.ProcessFriendlyName + " Launch Service")

            ExecutionString = "sc.exe"
            ArgumentString = "delete """ + Globals.ProcessFriendlyName + " Launch Service"""

            Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString)
            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            Delegate_Sub_Append_Text(UIOutputControl, "------------------------------------------------------------")
            Delegate_Sub_Append_Text(UIOutputControl, RunningProcess.StandardOutput.ReadToEnd)
            Delegate_Sub_Append_Text(UIOutputControl, "------------------------------------------------------------")
            Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString)

            If (RunningProcess.ExitCode <> 0) Then
                RunningProcess.Close()
                Return
            Else
                RunningProcess.Close()
            End If
        End If

        ' Install the Launch Service
        ExecutionString = "sc.exe"
        ArgumentString = "create """ + Globals.ProcessFriendlyName + " Launch Service"" binPath= """ +
                Globals.WinOfflineTemp + "\LaunchService.exe"" " +
                "start= demand"

        Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString)
        ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
        ProcessStartInfo.UseShellExecute = False
        ProcessStartInfo.RedirectStandardOutput = True
        ProcessStartInfo.CreateNoWindow = True

        RunningProcess = Process.Start(ProcessStartInfo)
        RunningProcess.WaitForExit()

        Delegate_Sub_Append_Text(UIOutputControl, "------------------------------------------------------------")
        Delegate_Sub_Append_Text(UIOutputControl, RunningProcess.StandardOutput.ReadToEnd)
        Delegate_Sub_Append_Text(UIOutputControl, "------------------------------------------------------------")
        Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString)

        If (RunningProcess.ExitCode <> 0) Then
            RunningProcess.Close()
            Return
        Else
            RunningProcess.Close()
        End If

        ' Start the Launch Service
        LaunchControl = New ServiceController(Globals.ProcessFriendlyName + " Launch Service")
        LaunchControl.Start({LaunchContext, AppToLaunch, AppArguments})
        LaunchControl.WaitForStatus(ServiceControllerStatus.Stopped)

        ' Delete the Launcher Service
        ExecutionString = "sc.exe"
        ArgumentString = "delete """ + Globals.ProcessFriendlyName + " Launch Service"""

        Delegate_Sub_Append_Text(UIOutputControl, "Detached process: " + ExecutionString + " " + ArgumentString)
        ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        ProcessStartInfo.WorkingDirectory = Globals.WorkingDirectory
        ProcessStartInfo.UseShellExecute = False
        ProcessStartInfo.RedirectStandardOutput = True
        ProcessStartInfo.CreateNoWindow = True

        RunningProcess = Process.Start(ProcessStartInfo)
        RunningProcess.WaitForExit()

        Delegate_Sub_Append_Text(UIOutputControl, "------------------------------------------------------------")
        Delegate_Sub_Append_Text(UIOutputControl, RunningProcess.StandardOutput.ReadToEnd)
        Delegate_Sub_Append_Text(UIOutputControl, "------------------------------------------------------------")
        Delegate_Sub_Append_Text(UIOutputControl, "Exit code: " + RunningProcess.ExitCode.ToString)
        RunningProcess.Close()

        Return

    End Sub

End Class
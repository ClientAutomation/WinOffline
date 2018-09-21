Partial Public Class WinOffline

    Public Class ComstoreAPI

        Public Shared Function GetParameterValue(ByVal ParameterSection As String, ByVal ParameterName As String) As String

            Dim ExecutionString As String
            Dim ArgumentString As String
            Dim RunningProcess As Process
            Dim ProcessStartInfo As ProcessStartInfo
            Dim StandardOutput As String

            ExecutionString = Globals.DSMFolder + "bin\" + "ccnfcmda.exe"
            ArgumentString = "-cmd getparametervalue -ps " + ParameterSection + " -pn " + ParameterName

            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            StandardOutput = RunningProcess.StandardOutput.ReadToEnd
            RunningProcess.Close()

            Return StandardOutput.Replace(vbCr, "").Replace(vbLf, "") ' Return -- remove carriage return (with full safety)

        End Function

        Public Shared Function SetParameterValue(ByVal ParameterSection As String, ByVal ParameterName As String, ByVal NewValue As String) As String

            Dim ExecutionString As String
            Dim ArgumentString As String
            Dim RunningProcess As Process
            Dim ProcessStartInfo As ProcessStartInfo
            Dim StandardOutput As String

            ExecutionString = Globals.DSMFolder + "bin\" + "ccnfcmda.exe"
            ArgumentString = "-cmd setparametervalue -ps " + ParameterSection + " -pn " + ParameterName + " -v """ + NewValue + """"

            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            StandardOutput = RunningProcess.StandardOutput.ReadToEnd
            RunningProcess.Close()

            Return StandardOutput.Replace(vbCr, "").Replace(vbLf, "") ' Return -- remove carriage return (with full safety)

        End Function

        Public Shared Function DeleteParameter(ByVal ParameterSection As String, ByVal ParameterName As String) As String

            Dim ExecutionString As String
            Dim ArgumentString As String
            Dim RunningProcess As Process
            Dim ProcessStartInfo As ProcessStartInfo
            Dim StandardOutput As String

            ExecutionString = Globals.DSMFolder + "bin\" + "ccnfcmda.exe"
            ArgumentString = "-cmd deleteparameter -ps " + ParameterSection + " -pn " + ParameterName

            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            StandardOutput = RunningProcess.StandardOutput.ReadToEnd
            RunningProcess.Close()

            Return StandardOutput.Replace(vbCr, "").Replace(vbLf, "") ' Return -- remove carriage return (with full safety)

        End Function

        Public Shared Function DeleteParameterSection(ByVal ParameterSection As String) As String

            Dim ExecutionString As String
            Dim ArgumentString As String
            Dim RunningProcess As Process
            Dim ProcessStartInfo As ProcessStartInfo
            Dim StandardOutput As String

            ExecutionString = Globals.DSMFolder + "bin\" + "ccnfcmda.exe"
            ArgumentString = "-cmd deleteparamsection -ps " + ParameterSection

            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            StandardOutput = RunningProcess.StandardOutput.ReadToEnd
            RunningProcess.Close()

            Return StandardOutput.Replace(vbCr, "").Replace(vbLf, "") ' Return -- remove carriage return (with full safety)

        End Function

    End Class

End Class
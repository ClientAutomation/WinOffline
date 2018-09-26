Partial Public Class WinOffline

    Public Class Logger

        Private Shared DebugLogBuffer As New ArrayList
        Private Shared DebugGUIBuffer As New ArrayList
        Private Shared ActiveLogBuffer As Boolean = False
        Private Shared ActiveGUIBuffer As Boolean = False
        Private Shared DebugGUITask As String = ""
        Private Shared PreviousTask As String = ""
        Private Shared ReInitDebugHistory As New ArrayList
        Public Shared LastCallStack As String = ""

        Private Shared Function GetNowTime() As String
            Dim RightNow As String = DateTime.Now.Year.ToString() + "." +
                                     DateTime.Now.Month.ToString() + "." +
                                     DateTime.Now.Day.ToString() + "--" +
                                     DateTime.Now.TimeOfDay.ToString
            Return RightNow.Replace(":", ".")
        End Function

        Public Shared Sub SetCurrentTask(ByVal CurrentTask As String)
            DebugGUITask = CurrentTask
        End Sub

        Public Shared Sub WriteDebug(ByVal Message As String)

            ' Valildate message
            If Message Is Nothing Then Return

            ' Write debug to console
            System.Console.WriteLine(Message)

            ' Write debug to progress ui
            If Globals.ProgressGUI IsNot Nothing AndAlso
                Globals.ProgressUIThread IsNot Nothing Then
                If Not DebugGUITask.Equals(PreviousTask) Then
                    Globals.ProgressGUI.Delegate_ProgressUI_UpdateCurrentTask(DebugGUITask)
                    PreviousTask = DebugGUITask
                End If
                If ActiveGUIBuffer Then
                    For Each LineItem As String In DebugGUIBuffer
                        Globals.ProgressGUI.Delegate_ProgressUI_WriteDebug(LineItem)
                    Next
                    DebugGUIBuffer.Clear()
                    ActiveGUIBuffer = False
                End If
                Globals.ProgressGUI.Delegate_ProgressUI_WriteDebug(Message)
            Else
                ActiveGUIBuffer = True
                DebugGUIBuffer.Add(Message)
            End If

            ' Write debug to debug log
            If Globals.DebugStreamWriter IsNot Nothing AndAlso
                Globals.DebugStreamWriter.BaseStream IsNot Nothing Then
                Try
                    If ActiveLogBuffer Then
                        For Each LineItem As String In DebugLogBuffer
                            Globals.DebugStreamWriter.WriteLine(LineItem)
                        Next
                        DebugLogBuffer.Clear()
                        ActiveLogBuffer = False
                    End If
                    Globals.DebugStreamWriter.WriteLine(Message)
                Catch ex As Exception
                    ActiveLogBuffer = True
                    DebugLogBuffer.Add(Message)
                End Try
            Else
                ActiveLogBuffer = True
                DebugLogBuffer.Add(Message)
            End If

            ' Write debug to reinit history
            ReInitDebugHistory.Add(Message)

            ' Special case: RemoveITCM UI -- write output to GUI control
            If LastCallStack.Contains("RemoveITCM|") AndAlso Globals.WinOfflineExplorer IsNot Nothing Then
                Globals.WinOfflineExplorer.Delegate_Sub_Append_Text(Globals.WinOfflineExplorer.txtRemovalTool, Message)
            End If

        End Sub

        Public Shared Sub WriteDebug(ByVal CallStack As String, ByVal Message As String)

            Dim FilterMessage As String = ""
            Dim FinalMessage As String = ""
            Dim CallStackChanged As Boolean = False

            ' Validate message
            If Message Is Nothing Then Return

            ' Check if callstack has changed
            If Not CallStack.ToLower.Equals(LastCallStack.ToLower) Then
                CallStackChanged = True
            End If

            LastCallStack = CallStack
            Dim CurrentTime = GetNowTime()

            ' Filter message
            FilterMessage = CallStack + Message
            While FilterMessage.Contains("|")
                FinalMessage = FilterMessage
                FilterMessage = FilterMessage.Substring(FilterMessage.IndexOf("|") + 1)
            End While

            ' Write debug to console
            If CallStackChanged Then
                System.Console.WriteLine()
            End If
            System.Console.WriteLine(FinalMessage)

            ' Write debug to progress ui
            If Globals.ProgressGUI IsNot Nothing AndAlso
                Globals.ProgressUIThread IsNot Nothing Then
                If Not DebugGUITask.Equals(PreviousTask) Then
                    Globals.ProgressGUI.Delegate_ProgressUI_UpdateCurrentTask(DebugGUITask)
                    PreviousTask = DebugGUITask
                End If
                If ActiveGUIBuffer Then
                    For Each LineItem As String In DebugGUIBuffer
                        Globals.ProgressGUI.Delegate_ProgressUI_WriteDebug(LineItem)
                    Next
                    DebugGUIBuffer.Clear()
                    ActiveGUIBuffer = False
                End If
                If CallStackChanged Then
                    Globals.ProgressGUI.Delegate_ProgressUI_WriteDebug("")
                End If
                Globals.ProgressGUI.Delegate_ProgressUI_WriteDebug(CurrentTime + CallStack + Message)
            Else
                ActiveGUIBuffer = True
                If CallStackChanged Then
                    DebugGUIBuffer.Add("")
                End If
                DebugGUIBuffer.Add(CurrentTime + CallStack + Message)
            End If

            ' Write debug to debug log
            If Globals.DebugStreamWriter IsNot Nothing AndAlso
                Globals.DebugStreamWriter.BaseStream IsNot Nothing Then
                Try
                    If ActiveLogBuffer Then
                        For Each LineItem As String In DebugLogBuffer
                            Globals.DebugStreamWriter.WriteLine(LineItem)
                        Next
                        DebugLogBuffer.Clear()
                        ActiveLogBuffer = False
                    End If
                    If CallStackChanged Then
                        Globals.DebugStreamWriter.WriteLine("")
                    End If
                    Globals.DebugStreamWriter.WriteLine(CurrentTime + CallStack + Message)
                Catch ex As Exception
                    ActiveLogBuffer = True
                    If CallStackChanged Then
                        DebugLogBuffer.Add("")
                    End If
                    DebugLogBuffer.Add(CurrentTime + CallStack + Message)
                End Try
            Else
                ActiveLogBuffer = True
                If CallStackChanged Then
                    DebugLogBuffer.Add("")
                End If
                DebugLogBuffer.Add(CurrentTime + CallStack + Message)
            End If

            ' Write debug to reinit history
            If CallStackChanged Then
                ReInitDebugHistory.Add("")
            End If
            ReInitDebugHistory.Add(CurrentTime + CallStack + Message)

            ' Special case: RemoveITCM UI -- write output to GUI control
            If CallStack.Contains("RemoveITCM|") AndAlso Globals.WinOfflineExplorer IsNot Nothing Then
                If CallStackChanged Then
                    Globals.WinOfflineExplorer.Delegate_Sub_Append_Text(Globals.WinOfflineExplorer.txtRemovalTool, "")
                End If
                Globals.WinOfflineExplorer.Delegate_Sub_Append_Text(Globals.WinOfflineExplorer.txtRemovalTool, FinalMessage)
            End If

        End Sub

        Public Shared Sub InsertSummary(ByVal CallStack As String)

            Dim DebugStreamReader As System.IO.StreamReader = Nothing
            Dim CloneStreamWriter As System.IO.StreamWriter = Nothing
            Dim strLine As String

            CallStack += "Logger|"

            ' Insert summary into debug log
            Try
                Globals.DebugStreamWriter.Close()
                Logger.WriteDebug(CallStack, "Debug stream closed.")

                DebugStreamReader = New System.IO.StreamReader(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")
                CloneStreamWriter = New System.IO.StreamWriter(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".clone", False)
                CloneStreamWriter.AutoFlush = True
                Logger.WriteDebug(CallStack, "Clone stream created.")

                Globals.DebugLogSummary = Manifest.DebugLogSummary
                Logger.WriteDebug(CallStack, "Debug summary created.")

                Globals.ProgressGUISummary = Manifest.DebugConsoleSummary ' Completely unrelated, but for consistency, get the console at the same time
                CloneStreamWriter.Write(Globals.DebugLogSummary)
                Logger.WriteDebug(CallStack, "Debug summary written to clone stream.")

                Do While DebugStreamReader.Peek() >= 0
                    strLine = DebugStreamReader.ReadLine
                    CloneStreamWriter.WriteLine(strLine)
                Loop
                Logger.WriteDebug(CallStack, "Debug log appended to clone stream.")

                DebugStreamReader.Close()
                CloneStreamWriter.Close()

                Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")
                Logger.WriteDebug(CallStack, "Move file: " + Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".clone")
                Logger.WriteDebug(CallStack, "To: " + Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")
                System.IO.File.Move(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".clone",
                                    Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")

                Logger.WriteDebug(CallStack, "Debug log successfully recreated with summary.")
            Catch ex As Exception
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Exception caught creating debug log summary.")

                If DebugStreamReader IsNot Nothing Then
                    DebugStreamReader.Close()
                    Logger.WriteDebug(CallStack, "Debug stream reader closed.")
                End If

                If CloneStreamWriter IsNot Nothing Then
                    CloneStreamWriter.Close()
                    Logger.WriteDebug(CallStack, "Clone stream writer closed.")
                End If

                If System.IO.File.Exists(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".clone") Then
                    Logger.WriteDebug(CallStack, "Delete file: " + Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".clone")
                    Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".clone")
                End If
            Finally
                Globals.DebugStreamWriter = New System.IO.StreamWriter(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", True)
                Globals.DebugStreamWriter.AutoFlush = True
                Logger.WriteDebug(CallStack, "Debug stream has been restored.")
            End Try

        End Sub

        Public Shared Sub InitDebugLog(ByVal CallStack As String)

            Dim DebugLog As String = Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt"
            CallStack += "InitDebug|"

            ' Try establishing the debug stream
            Try
                Globals.DebugStreamWriter = New System.IO.StreamWriter(DebugLog, True)
                Globals.DebugStreamWriter.AutoFlush = True
                Logger.WriteDebug(CallStack, "Debug stream: " + DebugLog)
            Catch ex As Exception
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Exception caught opening debug stream.")
                Throw New Exception("Exception caught creating debug log: " + DebugLog +
                                    Environment.NewLine + Environment.NewLine + ex.Message)
            End Try

        End Sub

        Public Shared Function InitAlternateDebugLog(ByVal CallStack As String) As Integer

            Dim DebugIncrement As Integer = 0
            Dim DebugLog As String = Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + DebugIncrement.ToString + ".txt"

            CallStack += "InitAltDebug|"

            While System.IO.File.Exists(DebugLog)
                Logger.WriteDebug(CallStack, "Alternate debug stream already exists: " + DebugLog)
                DebugIncrement += 1
                DebugLog = Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + DebugIncrement.ToString + ".txt"
            End While

            ' Try establishing the debug stream
            Try
                Globals.DebugStreamWriter = New System.IO.StreamWriter(DebugLog, True)
                Globals.DebugStreamWriter.AutoFlush = True
                Logger.WriteDebug(CallStack, "Alternate debug stream: " + DebugLog)
            Catch ex As Exception
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Logger.WriteDebug(CallStack, "Exception caught opening alternate debug stream.")
                Return 1
            End Try

            Return 0

        End Function

        Public Shared Function ReadAdditionalDebug(ByVal CallStack As String) As Boolean

            Dim AdditionalFound As Boolean = False
            Dim DebugIncrement As Integer = 0
            Dim DebugLog As String = Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + DebugIncrement.ToString + ".txt"
            Dim AlternateReader As System.IO.StreamReader
            Dim strLine As String

            CallStack += "Logger|"

            ' Iterate additional debug logs
            While System.IO.File.Exists(DebugLog)
                AdditionalFound = True
                Globals.AdditionalDebug.Add("Parallel Process Debug (" + Globals.ProcessFriendlyName + DebugIncrement.ToString + ".txt):")
                Try
                    Logger.WriteDebug(CallStack, "Open file: " + DebugLog)
                    AlternateReader = New System.IO.StreamReader(DebugLog)
                    Do While AlternateReader.Peek() >= 0
                        strLine = AlternateReader.ReadLine()
                        Globals.AdditionalDebug.Add(strLine)
                    Loop
                    Logger.WriteDebug(CallStack, "Finished reading in contents of log.")
                    Logger.WriteDebug(CallStack, "Close file: " + DebugLog)
                    AlternateReader.Close()
                    Logger.WriteDebug(CallStack, "Delete file: " + DebugLog)
                    Utility.DeleteFile(CallStack, DebugLog)
                Catch ex As Exception
                    Logger.WriteDebug(CallStack, "Error: Exception caught reading additional debug log.")
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                End Try
                Globals.AdditionalDebug.Add("")
                DebugIncrement += 1
                DebugLog = Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + DebugIncrement.ToString + ".txt"
            End While

            ' Check if any alternates were found
            If AdditionalFound = False Then
                Logger.WriteDebug(CallStack, "No additional debug logs found.")
                Return False
            Else
                Logger.WriteDebug(CallStack, "Additional debug logs have been processed.")
                Return True
            End If

        End Function

        Public Shared Sub TermDebugLog(ByVal CallStack As String, ByVal KeepDebugLog As Boolean)

            Dim DebugLog As String

            If Globals.DebugStreamWriter Is Nothing OrElse Not System.IO.File.Exists(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt") Then
                Return
            End If

            If ReadAdditionalDebug(CallStack) Then
                Logger.WriteDebug(CallStack, "Contents of additional debug logs:" + Environment.NewLine)
                Logger.WriteDebug("############################################################")
                For Each strLine As String In Globals.AdditionalDebug
                    Logger.WriteDebug(strLine)
                Next
                Logger.WriteDebug("############################################################" + Environment.NewLine)
            End If

            If Globals.WriteSDJobOutput Then
                Logger.WriteDebug(CallStack, "Copy File: " + Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")
                Logger.WriteDebug(CallStack, "To: " + Globals.JobOutputFile)
                System.IO.File.Copy(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", Globals.JobOutputFile, True)
            End If

            If KeepDebugLog Then
                DebugLog = Globals.ProcessFriendlyName + "-" +
                    DateTime.Now.Year.ToString() + "." +
                    DateTime.Now.Month.ToString() + "." +
                    DateTime.Now.Day.ToString() + "--" +
                    DateTime.Now.TimeOfDay.ToString()
                DebugLog = DebugLog.Substring(0, DebugLog.LastIndexOf("."))
                DebugLog = DebugLog + ".txt"
                DebugLog = DebugLog.Replace(":", ".")
                Logger.WriteDebug(CallStack, "Archive Log: " + Globals.DSMFolder + DebugLog)

                Globals.DebugStreamWriter.Close()

                Try
                    Logger.WriteDebug(CallStack, "Move File: " + Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")
                    Logger.WriteDebug(CallStack, "To: " + Globals.DSMFolder + DebugLog)
                    System.IO.File.Move(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", Globals.DSMFolder + DebugLog)
                Catch ex As Exception
                    Globals.DebugStreamWriter = New System.IO.StreamWriter(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", True)
                    Globals.DebugStreamWriter.AutoFlush = True
                    Logger.WriteDebug(CallStack, "Debug log abandoned: " + Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt")
                    Globals.DebugStreamWriter.Close()
                End Try
            Else
                Globals.DebugStreamWriter.Close()
                Try
                    Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", True)
                Catch ex As Exception
                    Logger.WriteDebug(ex.Message)
                    Logger.WriteDebug(ex.StackTrace)
                    Logger.WriteDebug(CallStack, "Error: Exception caught deleting debug log.")
                    Globals.DebugStreamWriter = New System.IO.StreamWriter(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", True)
                    Globals.DebugStreamWriter.AutoFlush = True
                    Logger.WriteDebug(CallStack, "Debug log abandoned.")
                    Globals.DebugStreamWriter.Close()
                End Try
            End If

        End Sub

        Public Shared Sub ReInitDebugLog()

            ' Referenced by Init.SDStageIReInit().
            ' Allows the prior debugging from a failed job to be purged.
            Globals.DebugStreamWriter.Close()
            Globals.DebugStreamWriter = New System.IO.StreamWriter(Globals.WindowsTemp + "\" + Globals.ProcessFriendlyName + ".txt", False)
            Globals.DebugStreamWriter.AutoFlush = True
            For Each LineItem As String In Logger.ReInitDebugHistory
                Globals.DebugStreamWriter.WriteLine(LineItem)
            Next

        End Sub

    End Class

End Class
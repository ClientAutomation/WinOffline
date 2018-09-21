Partial Public Class WinOffline

    Public Shared Function StageII(ByVal CallStack As String) As Integer

        Dim StateStreamWriter As System.IO.StreamWriter
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim RunningProcess As Process
        Dim ProcessStartInfo As ProcessStartInfo
        Dim PatchErrorReported As Boolean = False
        Dim HistoryFile As String
        Dim BackupFileStamp As String
        Dim FileList As String()
        Dim strFile As String
        Dim hexString As String
        Dim HostUUIDKey As Microsoft.Win32.RegistryKey = Nothing
        Dim RunLevel As Integer = 0

        CallStack += "StageII|"
        Logger.SetCurrentTask("StageII..")

        ' Read cache files
        Try
            Manifest.ReadCache(CallStack, Manifest.EXCEPTION_MANIFEST)
            Manifest.ReadCache(CallStack, Manifest.PATCH_MANIFEST)
            Manifest.ReadCache(CallStack, Manifest.REMOVAL_MANIFEST)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught reading cache file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 1
        End Try

        ' Switch: Disable ENC client
        If Globals.DisableENCSwitch AndAlso System.IO.File.Exists(Globals.DSMFolder + "bin\encutilcmd.exe") Then
            Logger.WriteDebug(CallStack, "Switch: Disable ENC client.")

            ExecutionString = Globals.DSMFolder + "bin\encutilcmd.exe"
            ArgumentString = "client -state disabled"

            Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd.ToString)
            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
            RunningProcess.Close()
        ElseIf Not System.IO.File.Exists(Globals.DSMFolder + "bin\encutilcmd.exe") Then
            Logger.WriteDebug(CallStack, "Switch ignored: Disable ENC client.")
            Logger.WriteDebug(CallStack, "The encutilcmd.exe binary is not available.")
        End If

        ' Stop client automation services
        If Globals.SimulateCafStopSwitch Then
            Logger.WriteDebug(CallStack, "Switch: Simulate stopping services.")
        Else
            RunLevel = StopServices(CallStack)
        End If
        Logger.SetCurrentTask("StageII..")
        If RunLevel <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Client Automation service termination failure.")
            RunLevel = StartServices(CallStack)
            Return 2 ' Don't proceed any further. Report the problem stopping services.
        End If

        ' Automatic: Clear notification server cache data
        Utility.DeleteFile(CallStack, Globals.DSMFolder + "appdata\cfnotsrvd.dat")

        ' Automatic: Cleanup basic agent temporary files
        Try
            Logger.WriteDebug(CallStack, "Automatic: Cleanup basic agent temporary files.")
            FileList = System.IO.Directory.GetFiles(Globals.WindowsTemp)
            If FileList.Length > 0 Then
                For n As Integer = 0 To FileList.Length - 1
                    strFile = FileList(n).ToString.ToLower
                    strFile = strFile.Substring(strFile.LastIndexOf("\") + 1)
                    ' Example of files:
                    ' bhiA.tmp
                    ' bhiA0.tmp
                    ' bhiA01.tmp
                    ' bhiA012.tmp
                    If strFile.StartsWith("bhi") And strFile.EndsWith(".tmp") Then
                        hexString = strFile.Substring(3, strFile.Length - 3)
                        hexString = hexString.Substring(0, hexString.IndexOf("."))
                        If Utility.IsHexString(hexString) Then
                            Try
                                System.IO.File.SetAttributes(FileList(n), IO.FileAttributes.Normal)
                                Utility.DeleteFile(CallStack, FileList(n))
                            Catch ex2 As Exception
                                Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file: " + FileList(n).ToString)
                                Logger.WriteDebug(ex2.Message)
                                Logger.WriteDebug(ex2.StackTrace)
                            End Try
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught cleaning up temporary files.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Reset cftrace level
        Try
            If Globals.ResetCftraceSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Reset cftrace level.")

                ExecutionString = Globals.DSMFolder + "bin\cftrace.exe"
                ArgumentString = "-c reset"

                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
                ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
                ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
                ProcessStartInfo.UseShellExecute = False
                ProcessStartInfo.RedirectStandardOutput = True
                ProcessStartInfo.CreateNoWindow = True

                RunningProcess = Process.Start(ProcessStartInfo)
                RunningProcess.WaitForExit()

                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd.ToString)
                Logger.WriteDebug("------------------------------------------------------------")
                Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
                RunningProcess.Close()
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught resetting cftrace level.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Cleanup DSM logs folder
        Try
            If Globals.CleanupLogsSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Cleanup various log folders.")
                If System.IO.Directory.Exists(Globals.DSMFolder + "logs") Then
                    Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "logs", Nothing)
                End If
                If System.IO.Directory.Exists(Globals.CAMFolder + "\logs") Then
                    Utility.DeleteFolderContents(CallStack, Globals.CAMFolder + "\logs", Nothing)
                End If
                If System.IO.Directory.Exists(Globals.SSAFolder + "log") Then
                    Utility.DeleteFolderContents(CallStack, Globals.SSAFolder + "log", Nothing)
                End If
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught deleting logs folder.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Remove CAM configuration file
        Try
            If Globals.RemoveCAMConfigSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Remove CAM configuration file.")
                If System.IO.File.Exists(Globals.CAMFolder + "\cam.cfg") Then
                    BackupFileStamp = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "--" + DateTime.Now.TimeOfDay.ToString()
                    BackupFileStamp = BackupFileStamp.Substring(0, BackupFileStamp.LastIndexOf("."))
                    BackupFileStamp = BackupFileStamp + ".bak"
                    BackupFileStamp = BackupFileStamp.Replace(":", ".")
                    System.IO.File.Move(Globals.CAMFolder + "\cam.cfg", Globals.CAMFolder + "\cam--" + BackupFileStamp)
                    Logger.WriteDebug(CallStack, "Backup of CAM configuration file saved: " + Globals.CAMFolder + "\cam--" + BackupFileStamp)
                End If
                If System.IO.File.Exists(Globals.CAMFolder + "\cam.cfg") Then
                    System.IO.File.SetAttributes(Globals.CAMFolder + "\cam.cfg", IO.FileAttributes.Normal)
                    Utility.DeleteFile(CallStack, Globals.CAMFolder + "\cam.cfg")
                End If
                If System.IO.File.Exists(Globals.CAMFolder + "\cam.bak") Then
                    System.IO.File.SetAttributes(Globals.CAMFolder + "\cam.bak", IO.FileAttributes.Normal)
                    Utility.DeleteFile(CallStack, Globals.CAMFolder + "\cam.bak")
                End If
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught deleting the CAM configuration file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Perform agent cleanup
        Try
            If Globals.CleanupAgentSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Perform agent cleanup.")
                If System.IO.Directory.Exists(Globals.DSMFolder + "Agent\units") Then
                    Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "Agent\units", Nothing)
                End If
                If Globals.SDFolder IsNot Nothing AndAlso System.IO.Directory.Exists(Globals.SDFolder) Then
                    If System.IO.Directory.Exists(Globals.SDFolder + "\TMP\activate") Then
                        Utility.DeleteFolderContents(CallStack, Globals.SDFolder + "\TMP\activate", Nothing)
                    End If
                End If
                If Globals.DTSFolder IsNot Nothing Then
                    Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "dts\dta\staging", Nothing)
                    Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "dts\dta\status", Nothing)
                    System.IO.File.Create(Globals.DSMFolder + "dts\dta\status\index")
                    Logger.WriteDebug(CallStack, "DTS index file recreated: " + Globals.DSMFolder + "dts\dta\status\index")
                End If
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught during agent cleanup.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Perform scalability server cleanup

        Try
            If Globals.CleanupServerSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Perform scalability server cleanup.")
                If Globals.SDFolder IsNot Nothing AndAlso System.IO.Directory.Exists(Globals.SDFolder) Then
                    If System.IO.Directory.Exists(Globals.SDFolder + "\ASM\DATABASE\ftm_journals") Then
                        System.IO.Directory.CreateDirectory(Globals.SDFolder + "\ASM\DATABASE\ftm_journals\CLEANSTART")
                    End If
                    If System.IO.Directory.Exists(Globals.SDFolder + "\ASM\D") Then
                        Utility.DeleteFolderContents(CallStack, Globals.SDFolder + "\ASM\D", Nothing)
                    End If
                    If System.IO.Directory.Exists(Globals.SDLibraryFolder + "\activate") Then
                        Utility.DeleteFolderContents(CallStack, Globals.SDLibraryFolder + "\activate", {"lgzip", "scripts", "scripts.zip"})
                    End If
                    If Globals.DTSFolder IsNot Nothing And Not Globals.CleanupAgentSwitch Then
                        Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "dts\dta\staging", Nothing)
                        Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "dts\dta\status", Nothing)
                        System.IO.File.Create(Globals.DSMFolder + "dts\dta\status\index")
                        Logger.WriteDebug(CallStack, "DTS index file recreated: " + Globals.DSMFolder + "dts\dta\status\index")
                    End If
                End If
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught during scalability server cleanup.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Certificate store cleanup
        Try
            If Globals.CleanupCertStoreSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Certificate store cleanup.")
                CertManager.RepairStore(CallStack)
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught repairing certificate store.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Remove patch history file, BEFORE patch operations
        Try
            If Globals.RemoveHistoryBeforeSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Remove patch history file, BEFORE patch operations.")
                HistoryFile = Globals.DSMFolder + Globals.HostName + ".his"
                Utility.DeleteFile(CallStack, HistoryFile)
                If System.IO.Directory.Exists(Globals.DSMFolder + "REPLACED") Then
                    Logger.WriteDebug(CallStack, "Delete folder: " + Globals.DSMFolder + "REPLACED")
                    Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "REPLACED", Nothing)
                    System.IO.Directory.Delete(Globals.DSMFolder + "REPLACED", True)
                End If
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught deleting patch history.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Analyze patch history files
        RunLevel = ProcessHistoryFiles(CallStack)
        If RunLevel <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Failed to process patch history files.")
            RunLevel = StartServices(CallStack)
            Return 3 ' Don't proceed any further. Report the problem parsing history files.
        End If

        ' Perform patch operations
        RunLevel = PatchOperations(CallStack)
        Logger.SetCurrentTask("StageII..")
        If RunLevel <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Patch operation method reports a failure..")
            PatchErrorReported = True
        End If

        ' Switch: Remove patch history file, AFTER patch operations
        Try
            If Globals.RemoveHistoryAfterSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Remove patch history file, AFTER patch operations.")
                HistoryFile = Globals.DSMFolder + Globals.HostName + ".his"
                Utility.DeleteFile(CallStack, HistoryFile)
                If System.IO.Directory.Exists(Globals.DSMFolder + "REPLACED") Then
                    Logger.WriteDebug(CallStack, "Delete folder: " + Globals.DSMFolder + "REPLACED")
                    Utility.DeleteFolderContents(CallStack, Globals.DSMFolder + "REPLACED", Nothing)
                    System.IO.Directory.Delete(Globals.DSMFolder + "REPLACED", True)
                End If
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught deleting patch history.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Switch: Regenerate agent HostUUID
        Try
            If Globals.RegenHostUUIDSwitch Then
                Logger.WriteDebug(CallStack, "Switch: Regenerate HostUUID.")
                Logger.WriteDebug(CallStack, "Delete comstore parameter: itrm/rc/host/managed/convertedhostuid")
                ComstoreAPI.DeleteParameter("itrm/rc/host/managed", "convertedhostuid")
                Logger.WriteDebug(CallStack, "Delete comstore parameter: itrm/rc/host/managed/hostuuidcopy")
                ComstoreAPI.DeleteParameter("itrm/rc/host/managed", "hostuuidcopy")
                Logger.WriteDebug(CallStack, "Delete comstore parameter: itrm/cfencrypt/LOCALID")
                ComstoreAPI.DeleteParameter("itrm/cfencrypt", "LOCALID")
                Logger.WriteDebug(CallStack, "Delete comstore section: itrm/rc/security/providers/winnt/users")
                ComstoreAPI.DeleteParameterSection("itrm/rc/security/providers/winnt/users")
                Logger.WriteDebug(CallStack, "Delete registry key: " + "HKLM\SOFTWARE\Wow6432Node\ComputerAssociates\hostUUID")
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\hostUUID", False)
                Logger.WriteDebug(CallStack, "Delete registry key: " + "HKLM\SOFTWARE\ComputerAssociates\hostUUID")
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKey("SOFTWARE\ComputerAssociates\hostUUID", False)
            End If
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Warning: Exception caught regenerating HostUUID.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
        End Try

        ' Start client automation services
        If Globals.SimulateCafStopSwitch Then
            Logger.WriteDebug(CallStack, "Switch: Simulate starting services.")
        Else
            RunLevel = StartServices(CallStack)
        End If
        If RunLevel <> 0 Then
            Logger.WriteDebug(CallStack, "Warning: Start services method reports a failure.")
        End If

        ' Switch: Launch DSM Explorer
        If Globals.LaunchGuiSwitch AndAlso Not Globals.RunningAsSystemIdentity AndAlso System.IO.File.Exists(Globals.DSMFolder + "bin\dsmgui.exe") Then
            Logger.WriteDebug(CallStack, "Switch: Launch DSM Explorer.")

            ExecutionString = Globals.DSMFolder + "bin\dsmgui.exe"
            ArgumentString = ""

            Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
            ProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
            ProcessStartInfo.WorkingDirectory = Globals.DSMFolder + "bin"
            ProcessStartInfo.UseShellExecute = False
            ProcessStartInfo.RedirectStandardOutput = True
            ProcessStartInfo.CreateNoWindow = True

            RunningProcess = Process.Start(ProcessStartInfo)
            RunningProcess.WaitForExit()

            RunningProcess.Close()
            Logger.WriteDebug(CallStack, "DSM Explorer launched.")
        End If

        ' Write cache files
        Manifest.WriteCache(CallStack, Manifest.PATCH_MANIFEST)
        Manifest.WriteCache(CallStack, Manifest.HISTORY_MANIFEST)
        Manifest.WriteCache(CallStack, Manifest.EXCEPTION_MANIFEST)

        ' Write execution state file [SD EXECUTION ONLY]
        If Globals.SDBasedMode Then
            Logger.WriteDebug(CallStack, "Open file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state")
            Try
                Logger.WriteDebug(CallStack, "Write: StageII completed")
                StateStreamWriter = New System.IO.StreamWriter(Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state", False)
                StateStreamWriter.WriteLine("StageII completed.")
                Logger.WriteDebug(CallStack, "Close file: " + Globals.WinOfflineTemp + "\" + Globals.ProcessFriendlyName + ".state")
                StateStreamWriter.Close()
            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Error: Exception caught writing the state file.")
                Logger.WriteDebug(ex.Message)
                Logger.WriteDebug(ex.StackTrace)
                Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                Return 4
            End Try
        End If

        If PatchErrorReported Then
            Logger.WriteDebug(CallStack, "StageII reports a patching error.")
            Return 7
        Else
            Globals.StageIICompleted = True
            Return 0
        End If

    End Function

End Class
Partial Public Class WinOffline

    Public Class CertManager

        Private Shared CertStoreFile As String
        Private Shared KeyStoreFile As String
        Private Shared CertDBFolder As String
        Private Shared CertificateStore As New ArrayList
        Private Shared PrivateKeyStore As New ArrayList

        Public Shared Sub RepairStore(ByVal CallStack As String)

            Dim CBBFolder As String
            Dim CBBFolderPrimaryBackup As String
            Dim CBBFolderSecondaryBackup As String
            Dim CurrentLine As String
            Dim PreviousLine As String
            Dim EntryKey As String
            Dim EntryType As String
            Dim EntryData As String
            Dim PrivateKeyList As New ArrayList
            Dim CertFileList As New ArrayList
            Dim AnonymousCertCounter As Integer = 0
            Dim StopCondition As Boolean = False
            Dim FileList As String()

            CallStack += "CertMgr|"
            CBBFolder = Globals.SharedCompFolder + "CBB"
            CBBFolderPrimaryBackup = Globals.SharedCompFolder + "CBB.backup1"
            CBBFolderSecondaryBackup = Globals.SharedCompFolder + "CBB.backup2"
            CertStoreFile = Globals.SharedCompFolder + "CBB\certstor.dat"
            KeyStoreFile = Globals.SharedCompFolder + "CBB\cbbkstor.dat"
            CertDBFolder = Globals.SharedCompFolder + "CBB\certdb"

            ' Perform verifications
            If Not System.IO.Directory.Exists(CBBFolder) Then
                Logger.WriteDebug(CallStack, "Warning: Certificate folder does not exist: " + CBBFolder)
                Logger.WriteDebug(CallStack, "Warning: Unable to repair certificate store.")
                Return
            End If

            If Not System.IO.File.Exists(CertStoreFile) Then
                Logger.WriteDebug(CallStack, "Warning: Certificate store file does not exist: " + CertStoreFile)
                Logger.WriteDebug(CallStack, "Warning: Unable to repair certificate store.")
                Return
            End If

            If Not System.IO.File.Exists(KeyStoreFile) Then
                Logger.WriteDebug(CallStack, "Warning: Private key store file does not exist: " + KeyStoreFile)
                Logger.WriteDebug(CallStack, "Warning: Unable to repair certificate store.")
                Return
            End If

            ' Backup CBB folder
            If System.IO.Directory.Exists(CBBFolderSecondaryBackup) Then
                Logger.WriteDebug(CallStack, "Delete folder: " + CBBFolderSecondaryBackup)
                System.IO.Directory.Delete(CBBFolderSecondaryBackup, True)
            End If

            If System.IO.Directory.Exists(CBBFolderPrimaryBackup) Then
                Logger.WriteDebug(CallStack, "Move folder: " + CBBFolderPrimaryBackup)
                Logger.WriteDebug(CallStack, "To: " + CBBFolderSecondaryBackup)
                System.IO.Directory.Move(CBBFolderPrimaryBackup, CBBFolderSecondaryBackup)
            End If

            If System.IO.Directory.Exists(CBBFolder) Then
                Logger.WriteDebug(CallStack, "Backup folder: " + CBBFolder)
                Logger.WriteDebug(CallStack, "To: " + CBBFolderPrimaryBackup)
                If Not Utility.CopyDirectory(CBBFolder, CBBFolderPrimaryBackup) Then
                    Logger.WriteDebug(CallStack, "Warning: Unable to backup CBB folder.")
                    Logger.WriteDebug(CallStack, "Warning: Unable to repair certificate store.")
                    Return
                End If
            End If

            ' Read certiciate and key stores
            ReadCertStore(CallStack)

            ' Process certificate store
            For i As Integer = 0 To CertificateStore.Count - 1

                EntryKey = ""
                EntryType = ""
                EntryData = ""

                ' Are we at the bottom?
                If i > CertificateStore.Count - 1 Then
                    If CertificateStore.Item(i - 1).ToString.ToLower.Equals("end") Then
                        CertificateStore.Add(" ") ' Add a blank space to bottom of array
                    ElseIf CertificateStore.Item(i - 1).ToString.ToLower.Equals("") Then
                        CertificateStore.Item(i - 1) = " " ' Convert blank line --> to blank space
                    End If
                    Exit For ' Stop processing
                End If

                ' Get the current line
                CurrentLine = CertificateStore.Item(i)

                ' Get the previous line
                If i > 0 Then
                    PreviousLine = CertificateStore.Item(i - 1)
                Else
                    PreviousLine = "-1"
                End If

                ' Process current line
                If CurrentLine.ToLower.StartsWith("id=") Then
                    EntryKey = CurrentLine.Substring(3) ' Parse tag info

                    ' Current line matches expected format?
                    If Not EntryKey.Length > 0 OrElse Not EntryKey.Contains(".") Then
                        Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                        CertificateStore.RemoveAt(i) ' Remove it
                        If i > 0 Then i -= 1 ' Decrement i
                        Continue For ' Skip to end
                    End If

                    ' Parse more attributes
                    EntryType = EntryKey.Substring(0, EntryKey.IndexOf(".")) ' Parse entry type (cert or tag)
                    EntryKey = EntryKey.Substring(EntryKey.IndexOf(".") + 1) ' Parse uuid

                    ' Current line matches expected format?
                    If Not (EntryType.ToLower.Equals("cert") Or EntryType.ToLower.Equals("tag")) OrElse
                        (EntryType.ToLower.Equals("cert") AndAlso (EntryKey.Length <> 40 Or Not Utility.IsHexString(EntryKey))) OrElse
                        (EntryType.ToLower.Equals("tag") AndAlso EntryKey.ToLower.Contains("itcm-anonymous")) OrElse
                        i + 3 > CertificateStore.Count - 1 OrElse
                        Not CertificateStore.Item(i + 1).ToString.ToLower.Equals("data=") OrElse
                        Not CertificateStore.Item(i + 3).ToString.ToLower.Equals("end") Then
                        Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                        CertificateStore.RemoveAt(i) ' Remove it
                        If i > 0 Then i -= 1 ' Decrement i
                        Continue For ' Skip to end
                    End If

                    ' Parse data
                    EntryData = CertificateStore.Item(i + 2) ' Read entry data

                    ' Process by entry type
                    If EntryType.ToLower.Equals("tag") Then
                        If Not EntryData.ToLower.StartsWith("cn=") Or
                            EntryData.ToLower.Contains("itcm-self-signed") Then
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i))
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 1))
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 2))
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 3))
                            CertificateStore.RemoveAt(i)
                            CertificateStore.RemoveAt(i)
                            CertificateStore.RemoveAt(i)
                            CertificateStore.RemoveAt(i)
                            If i > 0 Then i -= 1 ' Decrement i
                            Continue For ' Skip to end
                        Else
                            Logger.WriteDebug(CallStack, "Valid tag:         " + CertificateStore.Item(i + 2))
                            If Not PreviousLine.ToLower.Equals(" ") Then
                                CertificateStore.Insert(i, " ") ' Add a blank space
                                i += 1 ' Increment i
                            End If
                            i += 3 ' Entry is valid, increment i (to 'end' of section)
                            Continue For ' Skip to end
                        End If

                    ElseIf EntryType.ToLower.Equals("cert") Then
                        If Not EntryData.ToLower.StartsWith("subj") Or
                            EntryData.ToLower.Contains("itcm-self-signed") Or
                            Not EntryData.ToLower.Contains("file") Then
                            If EntryData.ToLower.Contains("itcm-self-signed") Then
                                AnonymousCertCounter += 1
                            End If
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i))
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 1))
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 2))
                            Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 3))
                            CertificateStore.RemoveAt(i)
                            CertificateStore.RemoveAt(i)
                            CertificateStore.RemoveAt(i)
                            CertificateStore.RemoveAt(i)
                            If i > 0 Then i -= 1 ' Decrement i
                            Continue For ' Skip to end
                        Else
                            Dim FileString As String = EntryData.Substring(EntryData.IndexOf("file """) + 6)
                            FileString = FileString.Substring(0, FileString.Length - 1)

                            ' Referenced certificate exists?
                            If Not System.IO.File.Exists(FileString) Then
                                Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i))
                                Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 1))
                                Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 2))
                                Logger.WriteDebug(CallStack, "Removing entry: " + CertificateStore.Item(i + 3))
                                CertificateStore.RemoveAt(i)
                                CertificateStore.RemoveAt(i)
                                CertificateStore.RemoveAt(i)
                                CertificateStore.RemoveAt(i)
                                If i > 0 Then i -= 1 ' Decrement i
                                Continue For ' Skip to end
                            End If

                            ' Everything checks out-- add to lists
                            CertFileList.Add(FileString.ToLower) ' Add to certificate file list
                            PrivateKeyList.Add(EntryKey.ToLower) ' Add private key to list

                            Dim CertSubject As String = CertificateStore.Item(i + 2).ToString.Substring(CertificateStore.Item(i + 2).ToString.IndexOf("CN="))
                            CertSubject = CertSubject.Substring(0, CertSubject.IndexOf(""""))
                            Logger.WriteDebug(CallStack, "Valid certificate: " + CertSubject)

                            If Not PreviousLine.ToLower.Equals(" ") Then
                                CertificateStore.Insert(i, " ") ' Add a blank space
                                i += 1 ' Increment i
                            End If

                            i += 3 ' Entry is valid, increment i (to 'end' of section)
                            Continue For ' Skip to end
                        End If
                    End If

                ElseIf (CurrentLine.Equals("") And PreviousLine.Equals("")) Or
                    (CurrentLine.ToLower.Equals(" ") And PreviousLine.ToLower.Equals("")) Or
                    (CurrentLine.ToLower.Equals(" ") And PreviousLine.ToLower.Equals(" ")) Then
                    CertificateStore.RemoveAt(i) ' Remove current line
                    If i > 0 Then i -= 1 ' Decrement i

                ElseIf (CurrentLine.ToLower.StartsWith(" ") And Not (PreviousLine.ToLower.Equals("") Or PreviousLine.ToLower.Equals(" "))) Or
                    CurrentLine.ToLower.StartsWith("version") Or
                    CurrentLine.ToLower.StartsWith("#") Then
                    ' Do nothing -- these are valid symbols.

                Else
                    Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                    CertificateStore.RemoveAt(i) ' Remove current line
                    If i > 0 Then i -= 1 ' Decrement i
                End If

            Next

            PreviousLine = "-1"
            CurrentLine = ""

            ' Process private key store.
            For i As Integer = 0 To PrivateKeyStore.Count - 1

                EntryKey = ""
                EntryType = ""
                StopCondition = False

                ' Are we at the bottom?
                If i > PrivateKeyStore.Count - 1 Then
                    If PrivateKeyStore.Item(i - 1).ToString.ToLower.Equals("end") Then
                        PrivateKeyStore.Add(" ") ' Add a blank space to bottom of array
                    ElseIf PrivateKeyStore.Item(i - 1).ToString.ToLower.Equals("") Then
                        PrivateKeyStore.Item(i - 1) = " " ' Convert blank line --> to blank space
                    End If
                    Exit For ' Stop processing
                End If

                ' Get the current line
                CurrentLine = PrivateKeyStore.Item(i)

                ' Get the previous line
                If i > 0 Then
                    PreviousLine = PrivateKeyStore.Item(i - 1)
                Else
                    PreviousLine = "-1"
                End If

                ' Process current line
                If CurrentLine.ToLower.StartsWith("id=") Then
                    EntryKey = CurrentLine.Substring(3) ' Parse tag info

                    ' Current line matches expected format?
                    If Not EntryKey.Length > 0 OrElse Not EntryKey.Contains(".") And Not EntryKey.Equals("CBB_LOCAL_SYSTEM_KEY_NAME") Then
                        Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                        PrivateKeyStore.RemoveAt(i) ' Remove it
                        If i > 0 Then i -= 1 ' Decrement i
                        Continue For ' Skip to end
                    End If

                    ' There's a VALID EntryKey called "CBB_LOCAL_SYSTEM_KEY_NAME" which does not have a "."
                    If EntryKey.Contains(".") Then
                        EntryType = EntryKey.Substring(0, EntryKey.IndexOf(".")) ' Parse entry type (ex. cbbcstor or cfencrypt_fips)
                        EntryKey = EntryKey.Substring(EntryKey.IndexOf(".") + 1) ' Parse uuid
                    Else
                        EntryType = EntryKey ' The entire EntryKey is the type
                    End If

                    ' Current line matches expected format?
                    If (EntryType.ToLower.Equals("cbbcstor") AndAlso (EntryKey.Length <> 40 Or Not Utility.IsHexString(EntryKey))) OrElse
                        (EntryType.ToLower.Equals("cbbcstor") AndAlso Not (PrivateKeyList.Contains(EntryKey.ToLower))) OrElse
                        i + 3 > PrivateKeyStore.Count - 1 OrElse
                        Not PrivateKeyStore.Item(i + 1).ToString.ToLower.Equals("data=") Then
                        Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                        PrivateKeyStore.RemoveAt(i) ' Remove it
                        If i > 0 Then i -= 1 ' Decrement i
                        Continue For ' Skip to end
                    End If

                    ' Iterate ahead to end of private key entry
                    For j As Integer = i + 2 To PrivateKeyStore.Count - 1
                        If PrivateKeyStore.Item(j).ToString.ToLower.Equals("end") Then
                            Logger.WriteDebug(CallStack, "Valid entry: " + PrivateKeyStore.Item(i))
                            If Not PreviousLine.ToLower.Equals(" ") Then
                                PrivateKeyStore.Insert(i, " ") ' Add a blank space
                                i += 1 ' Increment i
                            End If
                            StopCondition = True
                            i = j ' Entry is valid, move i up to j's position
                            Exit For ' Skip to end
                        End If
                    Next

                    If Not StopCondition Then
                        Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                        PrivateKeyStore.RemoveAt(i) ' Remove it
                        If i > 0 Then i -= 1 ' Decrement i
                    End If

                    Continue For ' Skip to end

                ElseIf (CurrentLine.Equals("") And PreviousLine.Equals("")) Or
                    (CurrentLine.ToLower.Equals(" ") And PreviousLine.ToLower.Equals("")) Or
                    (CurrentLine.ToLower.Equals(" ") And PreviousLine.ToLower.Equals(" ")) Then
                    PrivateKeyStore.RemoveAt(i) ' Remove current line
                    If i > 0 Then i -= 1 ' Decrement i

                ElseIf (CurrentLine.ToLower.StartsWith(" ") And Not (PreviousLine.ToLower.Equals("") Or PreviousLine.ToLower.Equals(" "))) Or
                    CurrentLine.ToLower.StartsWith("version") Or
                    CurrentLine.ToLower.StartsWith("#") Then
                    ' Do nothing -- these are valid symbols.

                Else
                    Logger.WriteDebug(CallStack, "Removing entry: " + CurrentLine)
                    PrivateKeyStore.RemoveAt(i) ' Remove current line
                    If i > 0 Then i -= 1 ' Decrement i
                End If

            Next

            Logger.WriteDebug(CallStack, "Found (" + AnonymousCertCounter.ToString + ") anonymous certificate(s).")
            FileList = System.IO.Directory.GetFiles(CertDBFolder)

            ' Process certdb folder
            For Each CertFile As String In FileList
                If Not CertFileList.Contains(CertFile.ToLower) Then
                    Utility.DeleteFile(CallStack, CertFile) ' Delete the file
                End If
            Next

            ' Write updated store and trigger new anonymous cert
            WriteCertStore(CallStack)
            TriggerNewCert(CallStack)

        End Sub

        Private Shared Sub ReadCertStore(ByVal CallStack As String)

            Dim CertFileReader As System.IO.StreamReader
            Dim strLine As String

            ' Read certificate store
            Logger.WriteDebug(CallStack, "Read certificate store: " + CertStoreFile)
            CertFileReader = New System.IO.StreamReader(CertStoreFile)
            Do While CertFileReader.Peek() >= 0
                strLine = CertFileReader.ReadLine()
                CertificateStore.Add(strLine)
            Loop
            Logger.WriteDebug(CallStack, "Close file: " + CertStoreFile)
            CertFileReader.Close()

            ' Read private key store
            Logger.WriteDebug(CallStack, "Read private key store: " + KeyStoreFile)
            CertFileReader = New System.IO.StreamReader(KeyStoreFile)
            Do While CertFileReader.Peek() >= 0
                strLine = CertFileReader.ReadLine()
                PrivateKeyStore.Add(strLine)
            Loop
            Logger.WriteDebug(CallStack, "Close file: " + KeyStoreFile)
            CertFileReader.Close()

        End Sub

        Private Shared Sub WriteCertStore(ByVal CallStack As String)

            Dim CertFileWriter As System.IO.StreamWriter

            ' Write new certificate store
            Logger.WriteDebug(CallStack, "Write certificate store: " + CertStoreFile)
            CertFileWriter = New IO.StreamWriter(CertStoreFile, False)
            For Each CertLine As String In CertificateStore
                CertFileWriter.WriteLine(CertLine)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + CertStoreFile)
            CertFileWriter.Close()

            ' Write new private key store
            Logger.WriteDebug(CallStack, "Write private key store: " + KeyStoreFile)
            CertFileWriter = New IO.StreamWriter(KeyStoreFile, False)
            For Each KeyLine As String In PrivateKeyStore
                CertFileWriter.WriteLine(KeyLine)
            Next
            Logger.WriteDebug(CallStack, "Close file: " + KeyStoreFile)
            CertFileWriter.Close()

        End Sub

        Private Shared Sub TriggerNewCert(ByVal CallStack As String)

            Dim ExecutionString As String
            Dim ArgumentString As String
            Dim RunningProcess As Process
            Dim ProcessStartInfo As ProcessStartInfo
            Dim StandardOutput As String
            Dim RemainingOutput As String
            Dim ConsoleOutput As String

            ' Run cacertutil list -v
            ExecutionString = Globals.DSMFolder + "bin\cacertutil.exe"
            ArgumentString = "list -v"
            Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
            ProcessStartInfo = New ProcessStartInfo(ExecutionString)
            ProcessStartInfo.Arguments = ArgumentString
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
                Threading.Thread.Sleep(Globals.THREAD_REST_INTERVAL)
            End While
            RunningProcess.WaitForExit()
            RemainingOutput = RunningProcess.StandardOutput.ReadToEnd.ToString
            StandardOutput += RemainingOutput
            Logger.WriteDebug(RemainingOutput)
            Logger.WriteDebug("------------------------------------------------------------")
            Logger.WriteDebug(CallStack, "Exit code: " + RunningProcess.ExitCode.ToString)
            RunningProcess.Close()

        End Sub

    End Class

End Class
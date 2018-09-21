Partial Public Class WinOffline

    Public Shared Function PatchProcessor(ByVal CallStack As String) As Integer

        Dim FileList As String()
        Dim PatchFileName As String         ' Name of patch file (i.e. C:\SomeDirectory\SomePatch.caz).
        Dim PatchPath As String             ' Path to patch file (i.e. C:\SomeDirectory).
        Dim PatchShortName As String        ' Name of patch file (i.e. SomePatch.caz).
        Dim PatchFriendlyName As String     ' Name of patch file (i.e. SomePatch).
        Dim RunLevel As Integer = 0

        CallStack += "PatchProcessor|"

        FileList = System.IO.Directory.GetFiles(Globals.WorkingDirectory)

        ' Process patch files
        For n As Integer = 0 To FileList.Length - 1
            If FileList(n).ToLower.EndsWith(".caz") Or FileList(n).ToLower.EndsWith(".jcl") Then

                PatchFileName = FileList(n)
                PatchPath = FileVector.GetFilePath(PatchFileName)
                PatchShortName = FileVector.GetShortName(PatchFileName)
                PatchFriendlyName = FileVector.GetFriendlyName(PatchFileName)

                Logger.WriteDebug(CallStack, "Detected patch file: " + PatchShortName)

                If Manifest.PatchManifestContains(PatchFriendlyName) Then
                    Logger.WriteDebug(CallStack, "Warning: Patch name is duplicate, skipping: " + PatchFriendlyName)
                    Continue For
                End If

                If PatchShortName.ToLower.EndsWith(".caz") Then
                    Logger.WriteDebug(CallStack, "Decompress patch: " + PatchShortName)
                    RunLevel = DecompressPatch(CallStack, PatchFileName)
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: Decompression failure.")
                        Return 1
                    End If
                ElseIf PatchShortName.ToLower.EndsWith(".jcl") Then
                    Logger.WriteDebug(CallStack, "Migrate patch: " + PatchShortName)
                    RunLevel = MigratePatch(CallStack, PatchFileName)
                    If RunLevel <> 0 Then
                        Logger.WriteDebug(CallStack, "Error: Migration failure.")
                        Return 2
                    End If
                End If

                RunLevel = ProcessPatch(CallStack, PatchFriendlyName) ' Process the patch JCL file

                If RunLevel <> 0 Then
                    Logger.WriteDebug(CallStack, "Error: Failed to process the patch.")
                    Return 3
                End If
            End If
        Next
        Logger.WriteDebug(CallStack, " (" + Manifest.PatchManifestCount.ToString + ") patches processed.")

        Return RunLevel

    End Function

    Public Shared Function MigratePatch(ByVal CallStack As String,
                                        ByVal JCLFileName As String) As Integer

        Dim TargetFileName As String        ' Destination filename (e.g. C:\Windows\Temp\WinOffline\SomePatch\SomePatch.jcl).
        Dim RunLevel As Integer = 0

        CallStack += "Migrate|"

        ' Create unique patch folder and copy JCL to the new folder
        TargetFileName = Globals.WinOfflineTemp + "\" + FileVector.GetFriendlyName(JCLFileName) + "\" + FileVector.GetShortName(JCLFileName)
        Try
            Logger.WriteDebug(CallStack, "Create folder: " + FileVector.GetFilePath(TargetFileName))
            System.IO.Directory.CreateDirectory(FileVector.GetFilePath(TargetFileName))
            Logger.WriteDebug(CallStack, "Copy File: " + JCLFileName)
            Logger.WriteDebug(CallStack, "To: " + TargetFileName)
            System.IO.File.Copy(JCLFileName, TargetFileName, True)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught migrating patch to destination.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 1
        End Try

        Return RunLevel

    End Function

    Public Shared Function DecompressPatch(ByVal CallStack As String, ByVal CAZFileName As String) As Integer

        Dim TargetFileName As String                        ' Destination filename (e.g. C:\Windows\Temp\WinOffline\SomePatch\SomePatch.caz).
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim CAZipXPProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process
        Dim ExitCode As Integer
        Dim FileList As String()
        Dim JCLShortName As String
        Dim JCLCounter As Integer = 0
        Dim RunLevel As Integer = 0

        CallStack += "Decompress|"

        ' Create unique patch folder and copy CAZ to the new folder
        TargetFileName = Globals.WinOfflineTemp + "\" + FileVector.GetFriendlyName(CAZFileName) + "\" + FileVector.GetShortName(CAZFileName)
        Try
            Logger.WriteDebug(CallStack, "Create folder: " + FileVector.GetFilePath(TargetFileName))
            System.IO.Directory.CreateDirectory(FileVector.GetFilePath(TargetFileName))
            Logger.WriteDebug(CallStack, "Copy File: " + CAZFileName)
            Logger.WriteDebug(CallStack, "To: " + TargetFileName)
            System.IO.File.Copy(CAZFileName, TargetFileName, True)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught preparing patch destination.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 1
        End Try

        ' Decompress the CAZ file
        ExecutionString = Globals.WinOfflineTemp + "\cazipxp.exe"
        ArgumentString = "-u " + """" + TargetFileName + """"

        Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)
        CAZipXPProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        CAZipXPProcessStartInfo.WorkingDirectory = FileVector.GetFilePath(TargetFileName)
        CAZipXPProcessStartInfo.UseShellExecute = False
        CAZipXPProcessStartInfo.RedirectStandardOutput = True
        CAZipXPProcessStartInfo.CreateNoWindow = True

        RunningProcess = Process.Start(CAZipXPProcessStartInfo)
        RunningProcess.WaitForExit()

        Logger.WriteDebug("------------------------------------------------------------")
        Logger.WriteDebug(RunningProcess.StandardOutput.ReadToEnd.ToString)
        Logger.WriteDebug("------------------------------------------------------------")
        ExitCode = RunningProcess.ExitCode
        Logger.WriteDebug(CallStack, "Exit code: " + ExitCode.ToString)

        RunningProcess.Close()

        If ExitCode <> 0 Then
            Logger.WriteDebug(CallStack, "Error: Cazipxp utility decompression failed.")
            Manifest.UpdateManifest(CallStack,
                                    Manifest.EXCEPTION_MANIFEST,
                                    {"Error: Cazipxp utility decompression failed.",
                                    "Reason: A non-zero exit code was returned from the utility."})
            Return 2
        End If

        ' Verify CAZ extracted a single JCL file
        FileList = System.IO.Directory.GetFiles(FileVector.GetFilePath(TargetFileName))
        For n As Integer = 0 To FileList.Length - 1
            If FileList(n).ToLower.Contains(".jcl") Then
                JCLShortName = FileVector.GetShortName(FileList(n))
                Logger.WriteDebug(CallStack, "Detected instruction file: " + JCLShortName)
                JCLCounter = JCLCounter + 1
            End If
        Next
        If JCLCounter = 0 Then
            Logger.WriteDebug(CallStack, "Error: Patch does not contain an instruction file.")
            Manifest.UpdateManifest(CallStack,
                                    Manifest.EXCEPTION_MANIFEST,
                                    {"Error: Patch is missing an instruction (.jcl) file.",
                                    "Please verify the contents of the " + FileVector.GetFriendlyName(CAZFileName) + " patch."})
            Return 3
        ElseIf JCLCounter > 1 Then
            Logger.WriteDebug(CallStack, "Error: Patch file contains multiple instruction files.")
            Manifest.UpdateManifest(CallStack,
                                    Manifest.EXCEPTION_MANIFEST,
                                    {"Error: Patch file contains multiple instruction (.jcl) files.",
                                    "Please verify the contents of the " + FileVector.GetFriendlyName(CAZFileName) + " patch."})
            Return 4
        Else
            Logger.WriteDebug(CallStack, "Verified: Patch contains only a single instruction file.")
        End If

        Return RunLevel

    End Function

    Public Shared Function ProcessPatch(ByVal CallStack As String, ByVal PatchFriendlyName As String) As Integer

        Dim PatchFileName As String = ""
        Dim PatchStreamReader As System.IO.StreamReader
        Dim InstructionList As New ArrayList
        Dim NewPatch As PatchVector
        Dim strLine As String
        Dim FileList As String()
        Dim DependentFlag As Boolean = False
        Dim RunLevel As Integer = 0

        CallStack += "Process|"

        ' Locate the patch instruction file
        FileList = System.IO.Directory.GetFiles(Globals.WinOfflineTemp + "\" + PatchFriendlyName)
        For n As Integer = 0 To FileList.Length - 1
            If FileList(n).ToLower.Contains(".jcl") Then
                PatchFileName = FileList(n)
            End If
        Next
        If PatchFileName.Equals("") Then Return 1

        ' Open the patch instruction file
        Try
            Logger.WriteDebug(CallStack, "Open file: " + PatchFileName)
            PatchStreamReader = New System.IO.StreamReader(PatchFileName)
        Catch ex As Exception
            Logger.WriteDebug(CallStack, "Error: Exception caught opening patch instruction file.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(ex.StackTrace)
            Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
            Return 2
        End Try
        Logger.WriteDebug("############################################################")
        Do While PatchStreamReader.Peek() >= 0
            strLine = PatchStreamReader.ReadLine()
            Logger.WriteDebug(strLine)
            InstructionList.Add(strLine)
        Loop
        Logger.WriteDebug("############################################################")
        Logger.WriteDebug(CallStack, "Close file: " + PatchFileName)
        PatchStreamReader.Close()

        ' Verify the patch instructions
        NewPatch = New PatchVector(PatchFileName, InstructionList)
        If NewPatch.GetInstruction("product").Equals("") Then
            Logger.WriteDebug(CallStack, "Error: Patch file is missing a product code (i.e. 'PRODUCT:' tag).")
            Manifest.UpdateManifest(CallStack,
                                    Manifest.EXCEPTION_MANIFEST,
                                    {"Error: Patch file is missing a product code (i.e. 'PRODUCT:' tag).",
                                    "Please verify the " + NewPatch.PatchFile.GetShortName + " patch."})
            Return 3
        End If
        If Not (NewPatch.IsClientAuto Or NewPatch.IsSharedComponent Or NewPatch.IsCAM Or NewPatch.IsSSA Or NewPatch.IsDataTransport Or NewPatch.IsExplorerGUI) Then
            Logger.WriteDebug(CallStack, "Error: Unsupported product code.")
            Manifest.UpdateManifest(CallStack,
                                    Manifest.EXCEPTION_MANIFEST,
                                    {"Error: Unsupported product code.",
                                    "Please verify the PRODUCT: tag in the patch file."})
            Return 4
        End If
        If NewPatch.GetShortNameReplaceList.Count + NewPatch.GetPreCommandList.Count + NewPatch.GetSysCommandList.Count = 0 Then
            Logger.WriteDebug(CallStack, "Error: Patch file is missing instructions (i.e. 'FILE:', 'PRESYSCMD:' or 'SYSCMD:' tags).")
            Manifest.UpdateManifest(CallStack,
                                    Manifest.EXCEPTION_MANIFEST,
                                    {"Error: Patch file is missing instructions (i.e. 'FILE:', 'PRESYSCMD:' or 'SYSCMD:' tags).",
                                    "Please verify the " + NewPatch.PatchFile.GetShortName + " patch."})
            Return 5
        End If

        ' Resolve dependent files
        For Each Dependent As String In NewPatch.GetAllDependentFiles
            DependentFlag = False
            FileList = System.IO.Directory.GetFiles(NewPatch.PatchFile.GetFilePath)
            For n As Integer = 0 To FileList.Length - 1
                If FileList(n).ToLower.Contains(Dependent.ToLower) Then
                    Logger.WriteDebug(CallStack, "Dependency verified: " + Dependent)
                    DependentFlag = True
                    Exit For
                End If
            Next
            If DependentFlag Then Continue For
            FileList = System.IO.Directory.GetFiles(Globals.WorkingDirectory)
            For n As Integer = 0 To FileList.Length - 1
                If FileList(n).ToLower.Contains(Dependent.ToLower) Then
                    Try
                        Logger.WriteDebug(CallStack, "Copy File: " + Globals.WorkingDirectory + Dependent)
                        Logger.WriteDebug(CallStack, "To: " + NewPatch.PatchFile.GetFilePath + "\" + Dependent)
                        System.IO.File.Copy(Globals.WorkingDirectory + Dependent, NewPatch.PatchFile.GetFilePath + "\" + Dependent, True)
                    Catch ex As Exception
                        Logger.WriteDebug(CallStack, "Error: Exception caught copying dependent file.")
                        Logger.WriteDebug(ex.Message)
                        Logger.WriteDebug(ex.StackTrace)
                        Manifest.UpdateManifest(CallStack, Manifest.EXCEPTION_MANIFEST, {ex.Message, ex.StackTrace})
                        Return 6
                    End Try
                    Logger.WriteDebug(CallStack, "Dependency verified: " + Dependent)
                    DependentFlag = True
                    Exit For
                End If
            Next
            If DependentFlag = False Then
                Logger.WriteDebug(CallStack, "Error: Missing dependency: " + Dependent)
                Manifest.UpdateManifest(CallStack,
                                        Manifest.EXCEPTION_MANIFEST,
                                        {"Error: Missing dependency: " + Dependent,
                                        "Please verify the " + NewPatch.PatchFile.GetShortName + " patch."})
                Return 7
            End If
        Next

        Logger.WriteDebug(CallStack, "Patch file: " + NewPatch.PatchFile.GetShortName)
        Logger.WriteDebug(CallStack, "Patch folder: " + NewPatch.PatchFile.GetFilePath)
        Logger.WriteDebug(CallStack, "Product Tag: " + NewPatch.GetInstruction("product"))
        Manifest.UpdateManifest(CallStack, Manifest.PATCH_MANIFEST, NewPatch)

        Return RunLevel

    End Function

End Class
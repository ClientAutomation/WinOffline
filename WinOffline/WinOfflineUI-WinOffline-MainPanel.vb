Imports System.Threading
Imports System.Windows.Forms
Imports WinOffline.WinOffline

Partial Public Class WinOfflineUI

    Private Sub InitMainPanel()

        Dim Runlevel As Integer = 0

        Dim CleanupDSMLogsItem As ListViewItem = lvwApplyOptions.Items.Add("-cleanlogs: Cleanup DSM logs folder.")
        Dim ResetCftraceItem As ListViewItem = lvwApplyOptions.Items.Add("-resetcftrace: Reset cftrace level.")
        Dim RemoveCAMConfigItem As ListViewItem = lvwApplyOptions.Items.Add("-rmcamcfg: Remove CAM configuration file.")
        Dim CleanupAgentItem As ListViewItem = lvwApplyOptions.Items.Add("-cleanagent: Perform agent cleanup.")
        Dim CleanupServerItem As ListViewItem = lvwApplyOptions.Items.Add("-cleanserver: Perform scalability server cleanup.")
        Dim CheckLibraryItem As ListViewItem = lvwApplyOptions.Items.Add("-checklibrary: Analyze software library without making changes.")
        Dim CleanLibraryItem As ListViewItem = lvwApplyOptions.Items.Add("-cleanlibrary: Perform software library cleanup.")
        Dim CleanupCertStoreItem As ListViewItem = lvwApplyOptions.Items.Add("-cleancerts: Certificate store cleanup.")
        Dim SkipCAFStartUpItem As ListViewItem = lvwApplyOptions.Items.Add("-skipcafstartup: Skip CAF startup.")
        Dim SkipCAMItem As ListViewItem = lvwApplyOptions.Items.Add("-skipcam: Don't stop CAM service.")
        Dim SkipDMPrimerItem As ListViewItem = lvwApplyOptions.Items.Add("-skipdmprimer: Don't stop DMPrimer service.")
        Dim SkiphmAgentItem As ListViewItem = lvwApplyOptions.Items.Add("-skiphm: Don't stop hmAgent.")
        Dim LaunchGuiItem As ListViewItem = lvwApplyOptions.Items.Add("-loadgui: Launch DSM Explorer.")
        Dim SimulateCafStopItem As ListViewItem = lvwApplyOptions.Items.Add("-simulatestop: Simulate recycling CAF.")
        Dim SimulatePatchItem As ListViewItem = lvwApplyOptions.Items.Add("-simulatepatch: Simulate patch operations.")
        Dim SimulatePatchErrorItem As ListViewItem = lvwApplyOptions.Items.Add("-simulatepatcherror: Simulate a patching error.")
        Dim RemoveHistoryBeforeItem As ListViewItem = lvwApplyOptions.Items.Add("-rmhistorybefore: Remove patch history file, BEFORE patch operations.")
        Dim RemoveHistoryAfterItem As ListViewItem = lvwApplyOptions.Items.Add("-rmhistoryafter: Remove patch history file, AFTER patch operations.")
        Dim DumpCazipxpItem As ListViewItem = lvwApplyOptions.Items.Add("-dumpcazipxp: Extract cazipxp.exe utility and exit without changes.")
        Dim RegenHostUUIDItem As ListViewItem = lvwApplyOptions.Items.Add("-regenuuid: Regenerate HostUUID.")
        Dim SDSignalRebootItem As ListViewItem = lvwApplyOptions.Items.Add("-signalreboot: Signal software delivery reboot request.")
        Dim RebootSystemItem As ListViewItem = lvwApplyOptions.Items.Add("-reboot: Reboot system after completion.")

        CleanupDSMLogsItem.Name = "CleanupDSMLogsItem"
        ResetCftraceItem.Name = "ResetCftraceItem"
        RemoveCAMConfigItem.Name = "RemoveCAMConfigItem"
        CleanupAgentItem.Name = "CleanupAgentItem"
        CleanupServerItem.Name = "CleanupServerItem"
        CheckLibraryItem.Name = "CheckLibraryItem"
        CleanLibraryItem.Name = "CleanLibraryItem"
        CleanupCertStoreItem.Name = "CleanupCertItem"
        SkipCAFStartUpItem.Name = "SkipCAFStartUpItem"
        SkipCAMItem.Name = "SkipCAMItem"
        SkipDMPrimerItem.Name = "SkipDMPrimerItem"
        SkiphmAgentItem.Name = "SkiphmAgentItem"
        LaunchGuiItem.Name = "LaunchGuiItem"
        SimulateCafStopItem.Name = "SimulateCafStopItem"
        SimulatePatchItem.Name = "SimulatePatchItem"
        SimulatePatchErrorItem.Name = "SimulatePatchErrorItem"
        RemoveHistoryBeforeItem.Name = "RemoveHistoryBeforeItem"
        RemoveHistoryAfterItem.Name = "RemoveHistoryAfterItem"
        DumpCazipxpItem.Name = "DumpCazipxpItem"
        RegenHostUUIDItem.Name = "RegenHostUUIDItem"
        SDSignalRebootItem.Name = "SDSignalRebootItem"
        RebootSystemItem.Name = "RebootSystemItem"

        If Globals.CleanupLogsSwitch Then GetListViewItemByName("CleanupDSMLogsItem", lvwApplyOptions).Checked = True
        If Globals.ResetCftraceSwitch Then GetListViewItemByName("ResetCftraceItem", lvwApplyOptions).Checked = True
        If Globals.RemoveCAMConfigSwitch Then GetListViewItemByName("RemoveCAMConfigItem", lvwApplyOptions).Checked = True
        If Globals.CleanupAgentSwitch Then GetListViewItemByName("CleanupAgentItem", lvwApplyOptions).Checked = True
        If Globals.CleanupServerSwitch Then GetListViewItemByName("CleanupServerItem", lvwApplyOptions).Checked = True
        If Globals.CheckSDLibrarySwitch Then GetListViewItemByName("CheckLibraryItem", lvwApplyOptions).Checked = True
        If Globals.CleanupSDLibrarySwitch Then GetListViewItemByName("CleanLibraryItem", lvwApplyOptions).Checked = True
        If Globals.CleanupCertStoreSwitch Then GetListViewItemByName("CleanupCertItem", lvwApplyOptions).Checked = True
        If Globals.SkipCAFStartUpSwitch Then GetListViewItemByName("SkipCAFStartUpItem", lvwApplyOptions).Checked = True
        If Globals.SkipCAM Then GetListViewItemByName("SkipCAMItem", lvwApplyOptions).Checked = True
        If Globals.SkipDMPrimer Then GetListViewItemByName("SkipDMPrimerItem", lvwApplyOptions).Checked = True
        If Globals.SkiphmAgent Then GetListViewItemByName("SkiphmAgentItem", lvwApplyOptions).Checked = True
        If Globals.LaunchGuiSwitch Then GetListViewItemByName("LaunchGuiItem", lvwApplyOptions).Checked = True
        If Globals.SimulateCafStopSwitch Then GetListViewItemByName("SimulateCafStopItem", lvwApplyOptions).Checked = True
        If Globals.SimulatePatchSwitch Then GetListViewItemByName("SimulatePatchItem", lvwApplyOptions).Checked = True
        If Globals.SimulatePatchErrorSwitch Then GetListViewItemByName("SimulatePatchErrorItem", lvwApplyOptions).Checked = True
        If Globals.RemoveHistoryBeforeSwitch Then GetListViewItemByName("RemoveHistoryBeforeItem", lvwApplyOptions).Checked = True
        If Globals.RemoveHistoryAfterSwitch Then GetListViewItemByName("RemoveHistoryAfterItem", lvwApplyOptions).Checked = True
        If Globals.DumpCazipxpSwitch Then GetListViewItemByName("DumpCazipxpItem", lvwApplyOptions).Checked = True
        If Globals.RegenHostUUIDSwitch Then GetListViewItemByName("RegenHostUUIDItem", lvwApplyOptions).Checked = True
        If Globals.SDSignalRebootSwitch Then GetListViewItemByName("SDSignalRebootItem", lvwApplyOptions).Checked = True
        If Globals.RebootSystemSwitch Then GetListViewItemByName("RebootSystemItem", lvwApplyOptions).Checked = True

        ColumnOptions.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        grpPatchView.Text = "Working Directory: " + Globals.ProcessFilePath

        PatchListViewThread = New Thread(AddressOf PatchScanThread)
        PatchListViewThread.Start()

    End Sub

    Private Sub PatchScanThread()

        Dim FileList As String()
        Dim PreviousFileList As String()
        Dim PatchFileName As String         ' Name of patch file (i.e. C:\SomeDirectory\SomePatch.caz).
        Dim PatchPath As String             ' Path to patch file (i.e. C:\SomeDirectory).
        Dim PatchShortName As String        ' Name of patch file (i.e. SomePatch.caz).
        Dim PatchFriendlyName As String     ' Name of patch file (i.e. SomePatch).
        Dim PatchManifest As New ArrayList
        Dim LoopCounter As Integer = 0
        Dim PatchErrorDetected As Boolean
        Dim Runlevel As Integer = 0

        PreviousFileList = {}

        Try

            While (Not TerminateSignal) ' Patch list view will refresh for duration of WinOfflineUI lifecycle
                PatchErrorDetected = False
                FileList = System.IO.Directory.GetFiles(Globals.WorkingDirectory)

                If Not WinOffline.Utility.IsArrayEqual(FileList, PreviousFileList) Then
                    Delegate_Sub_Clear_ListView(lvwPatchList)
                    PatchManifest.Clear()

                    For n As Integer = 0 To FileList.Length - 1
                        If FileList(n).ToLower.EndsWith(".caz") OrElse FileList(n).ToLower.EndsWith(".jcl") Then

                            PatchFileName = FileList(n)
                            PatchPath = FileVector.GetFilePath(PatchFileName)
                            PatchShortName = FileVector.GetShortName(PatchFileName)
                            PatchFriendlyName = FileVector.GetFriendlyName(PatchFileName)

                            If PatchManifest.Contains(PatchFriendlyName.ToLower) Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Duplicate"}))
                                Continue For
                            End If

                            If PatchShortName.ToLower.EndsWith(".caz") Then

                                Runlevel = DecompressPatch(PatchFileName)
                                If Runlevel = 1 Then
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Failed to create " + Globals.WinOfflineTemp + "\UITemp."}))
                                    PatchErrorDetected = True
                                ElseIf Runlevel = 2 Then
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Decompression failed"}))
                                    PatchErrorDetected = True
                                ElseIf Runlevel = 3 Then
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Missing JCL file"}))
                                    PatchErrorDetected = True
                                ElseIf Runlevel = 4 Then
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Multiple JCL files"}))
                                    PatchErrorDetected = True
                                ElseIf Runlevel <> 0 Then
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Unknown error"}))
                                Else
                                    PatchFileName = Globals.WinOfflineTemp + "\UITemp\" + PatchShortName
                                    PatchPath = FileVector.GetFilePath(PatchFileName)
                                    PatchShortName = FileVector.GetShortName(PatchFileName)
                                    PatchFriendlyName = FileVector.GetFriendlyName(PatchFileName)
                                End If

                                If Runlevel <> 0 Then
                                    System.IO.Directory.Delete(Globals.WinOfflineTemp + "\UITemp", True)
                                    Continue For
                                End If
                            End If

                            Runlevel = EvaluatePatch(PatchFileName)
                            If Runlevel = 0 Then
                                If Not GetPatchVector(PatchFileName).GetInstruction("VERSIONCHECK").Equals("") Then
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                        GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                        "READY [VERSIONCHECK=" + GetPatchVector(PatchFileName).GetInstruction("VERSIONCHECK") + "]"}))
                                Else
                                    Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                        GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                        "READY"}))
                                End If
                                PatchManifest.Add(PatchFriendlyName.ToLower)

                            ElseIf Runlevel = 1 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "Failed to open JCL file"}))
                                PatchErrorDetected = True
                            ElseIf Runlevel = 2 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "JCL missing PRODUCT tag"}))
                                PatchErrorDetected = True
                            ElseIf Runlevel = 3 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                "Unsupported product code"}))
                                PatchErrorDetected = True
                            ElseIf Runlevel = 4 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName, "N/A", "JCL missing instructions"}))
                                PatchErrorDetected = True
                            ElseIf Runlevel = 5 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                "JCL missing dependencies"}))
                                PatchErrorDetected = True
                            ElseIf Runlevel = 6 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                "Product component (" + GetPatchVector(PatchFileName).GetInstruction("PRODUCT") + ") is not installed."}))
                                PatchErrorDetected = True
                            ElseIf Runlevel = 7 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                "Not applicable [VERSIONCHECK=" + GetPatchVector(PatchFileName).GetInstruction("VERSIONCHECK") + "]"}))
                            ElseIf Runlevel = 8 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                "Not applicable."}))
                            ElseIf Runlevel = 9 Then
                                Delegate_Sub_Set_Patch_ListView(lvwPatchList, New ArrayList({PatchShortName,
                                                                                GetPatchVector(PatchFileName).GetInstruction("PRODUCT"),
                                                                                "Already applied."}))
                            End If

                            If FileList(n).ToLower.EndsWith(".caz") Then
                                System.IO.Directory.Delete(Globals.WinOfflineTemp + "\UITemp", True)
                            End If
                        End If
                    Next
                    PreviousFileList = FileList.Clone
                    Delegate_Sub_Enable_Start_Button(Not PatchErrorDetected)
                End If

                LoopCounter = 0
                While LoopCounter < 99 ' 50ms * 100 = ~500ms
                    If TerminateSignal Then Return
                    LoopCounter += 1
                    Thread.Sleep(50)
                End While
            End While
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, "PatchListViewThread --> " + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, ex.StackTrace)
        End Try

    End Sub

    Private Shared Function DecompressPatch(ByVal CAZFileName As String) As Integer

        Dim DestShortName As String
        Dim CAZDestPath As String
        Dim DestFileName As String
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim CAZipXPProcessStartInfo As ProcessStartInfo
        Dim RunningProcess As Process
        Dim ExitCode As Integer
        Dim FileList As String()
        Dim JCLShortName As String
        Dim JCLCounter As Integer = 0

        DestShortName = FileVector.GetShortName(CAZFileName)
        CAZDestPath = Globals.WinOfflineTemp + "\UITemp"
        DestFileName = CAZDestPath + "\" + DestShortName

        Try
            System.IO.Directory.CreateDirectory(CAZDestPath)
            System.IO.File.Copy(CAZFileName, DestFileName, True)
        Catch ex As Exception
            Return 1
        End Try

        ExecutionString = Globals.WinOfflineTemp + "\cazipxp.exe"
        ArgumentString = "-u " + """" + DestFileName + """"

        CAZipXPProcessStartInfo = New ProcessStartInfo(ExecutionString, ArgumentString)
        CAZipXPProcessStartInfo.WorkingDirectory = CAZDestPath
        CAZipXPProcessStartInfo.UseShellExecute = False
        CAZipXPProcessStartInfo.RedirectStandardOutput = True
        CAZipXPProcessStartInfo.CreateNoWindow = True

        RunningProcess = Process.Start(CAZipXPProcessStartInfo)
        RunningProcess.WaitForExit()

        ExitCode = RunningProcess.ExitCode
        RunningProcess.Close()

        If ExitCode <> 0 Then
            Return 2
        End If

        FileList = System.IO.Directory.GetFiles(CAZDestPath)
        For n As Integer = 0 To FileList.Length - 1
            If FileList(n).ToLower.Contains(".jcl") Then
                JCLShortName = FileVector.GetShortName(FileList(n))
                JCLCounter = JCLCounter + 1
            End If
        Next

        If JCLCounter = 0 Then
            Return 3
        ElseIf JCLCounter > 1 Then
            Return 4
        End If

        Return 0

    End Function

    Public Shared Function EvaluatePatch(ByVal PatchFileName As String) As Integer

        Dim FileList As String()
        Dim PatchStreamReader As System.IO.StreamReader
        Dim InstructionList As New ArrayList
        Dim NewPatch As PatchVector
        Dim strLine As String
        Dim DependentFlag As Boolean = False
        Dim AlreadyAppliedList As New List(Of Boolean)

        If PatchFileName.ToLower.EndsWith(".caz") Then
            FileList = System.IO.Directory.GetFiles(FileVector.GetFilePath(PatchFileName))
            For n As Integer = 0 To FileList.Length - 1
                If FileList(n).ToLower.Contains(".jcl") Then
                    PatchFileName = FileList(n)
                End If
            Next
        End If

        Try
            PatchStreamReader = New System.IO.StreamReader(PatchFileName)
        Catch ex As Exception
            Return 1 ' Unable to open/read JCL
        End Try

        Do While PatchStreamReader.Peek() >= 0
            strLine = PatchStreamReader.ReadLine()
            InstructionList.Add(strLine)
        Loop

        PatchStreamReader.Close()

        NewPatch = New PatchVector(PatchFileName, InstructionList)

        If NewPatch.GetInstruction("product").Equals("") Then
            Return 2 ' No product tag
        End If

        If Not (NewPatch.IsClientAuto Or NewPatch.IsSharedComponent Or NewPatch.IsCAM Or NewPatch.IsSSA Or NewPatch.IsDataTransport Or NewPatch.IsExplorerGUI) Then
            Return 3 ' Unsupported component
        End If

        If NewPatch.GetShortNameReplaceList.Count + NewPatch.GetPreCommandList.Count + NewPatch.GetSysCommandList.Count = 0 Then
            Return 4 ' Patch contains NO instructions 
        End If

        ' Resolve patch dependencies
        For Each Dependent As String In NewPatch.GetAllDependentFiles
            DependentFlag = False
            FileList = System.IO.Directory.GetFiles(NewPatch.PatchFile.GetFilePath)
            For n As Integer = 0 To FileList.Length - 1
                If FileList(n).ToLower.Contains(Dependent.ToLower) Then
                    DependentFlag = True
                    Exit For
                End If
            Next
            If DependentFlag = False Then
                Return 5 ' Missing dependent files
            End If
        Next

        If (NewPatch.IsClientAuto AndAlso Globals.DSMFolder Is Nothing) Or
            (NewPatch.IsSharedComponent AndAlso Globals.SharedCompFolder Is Nothing) Or
            (NewPatch.IsCAM AndAlso Globals.CAMFolder Is Nothing) Or
            (NewPatch.IsSSA AndAlso Globals.SSAFolder Is Nothing) Or
            (NewPatch.IsDataTransport AndAlso Globals.DTSFolder Is Nothing) Or
            (NewPatch.IsExplorerGUI AndAlso Globals.EGCFolder Is Nothing) Then
            Return 6 ' Patch component is not installed
        End If

        If Not NewPatch.GetInstruction("VERSIONCHECK").Equals("") AndAlso Not NewPatch.GetInstruction("VERSIONCHECK").Equals(Globals.ITCMComstoreVersion) Then
            Return 7 ' VERSIONCHECK mismatch
        End If

        If NewPatch.SourceReplaceList.Count > 0 Then
            For x As Integer = 0 To NewPatch.SourceReplaceList.Count - 1
                If Not Utility.IsFileEqual(NewPatch.SourceReplaceList.Item(x).ToString, NewPatch.DestReplaceList.Item(x).ToString) Then
                    If Not System.IO.File.Exists(NewPatch.DestReplaceList.Item(x).ToString) AndAlso
                        NewPatch.SkipIfNotFoundList.Contains(FileVector.GetShortName(NewPatch.DestReplaceList.Item(x).ToString.ToLower)) Then
                        ' Skipped file -- no applicability information
                    ElseIf Not System.IO.File.Exists(NewPatch.DestReplaceList.Item(x).ToString) AndAlso
                        Not NewPatch.SkipIfNotFoundList.Contains(FileVector.GetShortName(NewPatch.DestReplaceList.Item(x).ToString.ToLower)) Then
                        AlreadyAppliedList.Add(False) ' New file -- not applied
                    Else
                        AlreadyAppliedList.Add(False) ' Complete mismatch -- not applied
                    End If
                Else
                    AlreadyAppliedList.Add(True) ' File match -- already applied
                End If
            Next

            If AlreadyAppliedList.Count = 0 Then
                Return 8 ' Not applicable
            ElseIf Not AlreadyAppliedList.Contains(False) Then
                Return 9 ' Already applied
            End If
        End If

        Return 0

    End Function

    Public Shared Function GetPatchVector(ByVal PatchFileName As String) As PatchVector

        Dim FileList As String()
        Dim PatchStreamReader As System.IO.StreamReader
        Dim InstructionList As New ArrayList
        Dim strLine As String

        If PatchFileName.ToLower.EndsWith(".caz") Then
            FileList = System.IO.Directory.GetFiles(FileVector.GetFilePath(PatchFileName))
            For n As Integer = 0 To FileList.Length - 1
                If FileList(n).ToLower.Contains(".jcl") Then
                    PatchFileName = FileList(n)
                End If
            Next
        End If

        Try
            PatchStreamReader = New System.IO.StreamReader(PatchFileName)
        Catch ex As Exception
            Return Nothing ' Unable open JCL file
        End Try

        Do While PatchStreamReader.Peek() >= 0
            strLine = PatchStreamReader.ReadLine()
            InstructionList.Add(strLine)
        Loop

        PatchStreamReader.Close()

        Return New PatchVector(PatchFileName, InstructionList)

    End Function

    Private Function GetListViewItemByName(ByVal ItemName As String, ByVal oListView As ListView) As ListViewItem
        For Each item As ListViewItem In oListView.Items
            If item.Name.Equals(ItemName) Then Return item
        Next
        Return Nothing
    End Function

    Private Sub xListView_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvwApplyOptions.ItemChecked, lvwRemoveOptions.ItemChecked

        ' Remove the check handlers during mirror operation
        RemoveHandler lvwApplyOptions.ItemChecked, AddressOf xListView_ItemChecked
        RemoveHandler lvwRemoveOptions.ItemChecked, AddressOf xListView_ItemChecked

        ' Check the sender, and mirror the check configuration to the other ListView
        If sender.Equals(lvwApplyOptions) Then
            For Each ListItem As ListViewItem In lvwRemoveOptions.Items
                If ListItem.Name.Equals(e.Item.Name) Then
                    ListItem.Checked = e.Item.Checked
                End If
            Next
        ElseIf sender.Equals(lvwRemoveOptions) Then
            For Each ListItem As ListViewItem In lvwApplyOptions.Items
                If ListItem.Name.Equals(e.Item.Name) Then
                    ListItem.Checked = e.Item.Checked
                End If
            Next
        End If

        ' Resume check handlers
        AddHandler lvwApplyOptions.ItemChecked, AddressOf xListView_ItemChecked
        AddHandler lvwRemoveOptions.ItemChecked, AddressOf xListView_ItemChecked

    End Sub

End Class
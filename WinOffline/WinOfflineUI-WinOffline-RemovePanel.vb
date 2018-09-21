Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Imports WinOffline.WinOffline

Partial Public Class WinOfflineUI

    Private Sub InitRemovePanel()

        CopyListView(lvwApplyOptions, lvwRemoveOptions)

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

        RecurseHistoryTree()

    End Sub

    Private Sub CopyListView(ByVal ListView1 As ListView, ByVal ListView2 As ListView)
        For Each ListItem As ListViewItem In ListView1.Items
            Dim CloneItem As ListViewItem = ListItem.Clone()
            CloneItem.Name = ListItem.Name
            For Each SubItem As ListViewItem.ListViewSubItem In ListItem.SubItems
                CloneItem.SubItems.Add(New ListViewItem.ListViewSubItem(CloneItem, SubItem.Text, SubItem.ForeColor, SubItem.BackColor, SubItem.Font))
            Next
            ListView2.Items.Add(CloneItem)
        Next
    End Sub

    Private Sub RecurseHistoryTree()

        Dim OriginalFileFound As Boolean = False
        Dim SupplementalPath As String = ""
        Dim LatestFolder As String = ""
        Dim FolderIncrement As Integer = 0
        Dim RemovalCount As Integer = 0

        For Each Component As TreeNode In treHistory.Nodes
            For Each Patch As TreeNode In Component.Nodes
                If Patch.Text.ToLower.Equals("no history available.") Then
                    Exit For ' ' Break loop, process next component
                End If

                OriginalFileFound = False

                ' Iterate the installed files within each patch
                ' Note: While the majority of this routine verifies all replaced files reported in the
                '       history file are available in each respective REPLACED folder, this is really
                '       not necessary. It's possible to have a patch that applies new files, in which
                '       case, no original file is saved in the REPLACED folder. Some testing with
                '       ApplyPTF shows having the REPLACED subfolder is sufficient. If some original
                '       files are missing, it doesn't complain, and just deletes the replacement files
                '       outlined in the history file. I'm leaving this routine in WinOffline, as this
                '       might change in the future, but for now, I will blindly delete replacement files
                '       that are suspected to be new.
                For Each InstalledFile As TreeNode In Patch.Nodes
                    FolderIncrement = 0
                    If Component.Text.ToLower.Equals("client automation") Then
                        SupplementalPath = InstalledFile.Text.Replace(Globals.DSMFolder, "")
                        LatestFolder = Globals.DSMFolder + "REPLACED\" + Patch.Text + ".OLD"
                        If System.IO.Directory.Exists(LatestFolder) Then
                            While True ' Loop dangerously for latest REPLACED folder
                                If System.IO.Directory.Exists(Globals.DSMFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD") Then
                                    LatestFolder = Globals.DSMFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD"
                                    FolderIncrement += 1
                                Else
                                    Exit While
                                End If
                            End While
                            If File.Exists(LatestFolder + "\" + SupplementalPath) Then
                                OriginalFileFound = True
                            Else
                                OriginalFileFound = False
                            End If
                        End If

                    ElseIf Component.Text.ToLower.Equals("shared components") Then
                        SupplementalPath = InstalledFile.Text.Replace(Globals.SharedCompFolder, "")
                        LatestFolder = Globals.SharedCompFolder + "REPLACED\" + Patch.Text + ".OLD"
                        If System.IO.Directory.Exists(LatestFolder) Then
                            While True ' Loop dangerously for latest REPLACED folder
                                If System.IO.Directory.Exists(Globals.SharedCompFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD") Then
                                    LatestFolder = Globals.SharedCompFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD"
                                    FolderIncrement += 1
                                Else
                                    Exit While
                                End If
                            End While
                            If File.Exists(LatestFolder + "\" + SupplementalPath) Then
                                OriginalFileFound = True
                            End If
                        End If

                    ElseIf Component.Text.ToLower.Equals("ca message queuing service") Then
                        SupplementalPath = InstalledFile.Text.Replace(Globals.CAMFolder + "\", "")
                        LatestFolder = Globals.CAMFolder + "\REPLACED\" + Patch.Text + ".OLD"
                        If System.IO.Directory.Exists(LatestFolder) Then
                            While True ' Loop dangerously for latest REPLACED folder
                                If System.IO.Directory.Exists(Globals.CAMFolder + "\REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD") Then
                                    LatestFolder = Globals.CAMFolder + "\REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD"
                                    FolderIncrement += 1
                                Else
                                    Exit While
                                End If
                            End While
                            If File.Exists(LatestFolder + "\" + SupplementalPath) Then
                                OriginalFileFound = True
                            End If
                        End If

                    ElseIf Component.Text.ToLower.Equals("secure socket adapter") Then
                        SupplementalPath = InstalledFile.Text.Replace(Globals.SSAFolder, "")
                        LatestFolder = Globals.SSAFolder + "REPLACED\" + Patch.Text + ".OLD"
                        If System.IO.Directory.Exists(LatestFolder) Then
                            While True ' Loop dangerously for latest REPLACED folder
                                If System.IO.Directory.Exists(Globals.SSAFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD") Then
                                    LatestFolder = Globals.SSAFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD"
                                    FolderIncrement += 1
                                Else
                                    Exit While
                                End If
                            End While
                            If File.Exists(LatestFolder + "\" + SupplementalPath) Then
                                OriginalFileFound = True
                            End If
                        End If

                    ElseIf Component.Text.ToLower.Equals("data transport service") Then
                        SupplementalPath = InstalledFile.Text.Replace(Globals.DTSFolder + "\", "")
                        LatestFolder = Globals.DTSFolder + "\REPLACED\" + Patch.Text + ".OLD"
                        If System.IO.Directory.Exists(LatestFolder) Then
                            While True ' Loop dangerously for latest REPLACED folder
                                If System.IO.Directory.Exists(Globals.DTSFolder + "\REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD") Then
                                    LatestFolder = Globals.DTSFolder + "\REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD"
                                    FolderIncrement += 1
                                Else
                                    Exit While
                                End If
                            End While
                            If File.Exists(LatestFolder + "\" + SupplementalPath) Then
                                OriginalFileFound = True
                            End If
                        End If

                    ElseIf Component.Text.ToLower.Equals("dsm explorer gui") Then
                        SupplementalPath = InstalledFile.Text.Replace(Globals.EGCFolder, "")
                        LatestFolder = Globals.EGCFolder + "REPLACED\" + Patch.Text + ".OLD"
                        If System.IO.Directory.Exists(LatestFolder) Then
                            While True ' Loop dangerously for latest REPLACED folder
                                If System.IO.Directory.Exists(Globals.EGCFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD") Then
                                    LatestFolder = Globals.EGCFolder + "REPLACED\" + Patch.Text + "-" + FolderIncrement.ToString + ".OLD"
                                    FolderIncrement += 1
                                Else
                                    Exit While
                                End If
                            End While
                            If File.Exists(LatestFolder + "\" + SupplementalPath) Then
                                OriginalFileFound = True
                            End If
                        End If
                    End If
                    If OriginalFileFound = False Then Exit For
                Next

                ' All originals are availalbe -- This is good.
                If OriginalFileFound Then
                    Dim RemovalListItem As ListViewItem = lvwHistory.Items.Add(Patch.Text)
                    RemovalListItem.SubItems.Add(Component.Text)
                    RemovalListItem.SubItems.Add("OK for removal [" + LatestFolder.Substring(LatestFolder.LastIndexOf("\") + 1) + "]")
                    For i As Integer = 0 To Manifest.RemovalManifestCount - 1
                        Dim rVector As RemovalVector = Manifest.GetRemovalFromManifest(i)
                        If rVector.RemovalItem.ToLower.Equals(Patch.Text.ToLower) Then
                            RemovalListItem.Checked = True
                        End If
                    Next

                ElseIf OriginalFileFound = False And System.IO.Directory.Exists(LatestFolder) Then
                    ' Note: Refer to the note at the top of this routine. Although not all files
                    '       identified by the history file match the contents saved in the REPLACED
                    '       subfolder, we will allow removal of the patch, based purely on the
                    '       existence of the matching REPLACED subfolder. Any file not backed up
                    '       in REPLACED will assume to have been a new file, introduced when the
                    '       patch was applied, and therefore will be discarded.
                    '
                    '       Though I'm not throwing away the code that checks for all this ;)
                    '       Perhaps it will have some future use to separate these two scenarios.
                    Dim RemovalListItem As ListViewItem = lvwHistory.Items.Add(Patch.Text)
                    RemovalListItem.SubItems.Add(Component.Text)
                    RemovalListItem.SubItems.Add("OK for removal [" + LatestFolder.Substring(LatestFolder.LastIndexOf("\") + 1) + "]")

                    For i As Integer = 0 To Manifest.RemovalManifestCount - 1
                        Dim rVector As RemovalVector = Manifest.GetRemovalFromManifest(i)
                        If rVector.RemovalItem.ToLower.Equals(Patch.Text.ToLower) Then
                            RemovalListItem.Checked = True
                        End If
                    Next
                Else
                    Dim RemovalListItem As ListViewItem = lvwHistory.Items.Add(Patch.Text)
                    RemovalListItem.SubItems.Add(Component.Text)
                    RemovalListItem.SubItems.Add("Missing REPLACED subfolder for removal.")
                    RemovalListItem.ForeColor = SystemColors.GrayText
                End If
                RemovalCount += 1
            Next
        Next

        If RemovalCount > 0 Then
            ColumnHistoryPatch.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            ColumnHistoryComp.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            ColumnHistoryStatus.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        Else
            Delegate_Sub_Enable_Remove_Start_Button(False)
        End If

    End Sub

    Private Sub HistoryListView_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvwHistory.ItemCheck
        If Not lvwHistory.Items(e.Index).SubItems(2).Text.ToLower.Contains("ok for removal") Then
            e.NewValue = e.CurrentValue
        End If
    End Sub

    Private Sub HistoryListView_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvwHistory.ItemChecked
        Delegate_Sub_Enable_Remove_Start_Button(False)
        For Each ListItem As ListViewItem In lvwHistory.Items
            If ListItem.Checked Then Delegate_Sub_Enable_Remove_Start_Button(True)
        Next
    End Sub

End Class
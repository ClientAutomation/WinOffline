Imports System.Windows.Forms
Imports WinOffline.WinOffline

Partial Public Class WinOfflineUI

    Private SDHelpStepCounter As Integer = 1
    Private PatchListViewThread As Threading.Thread

    Private Sub InitWinOffline()
        InitStartPanel()
        InitMainPanel()
        InitRemovePanel()
        InitSDHelpPanel()
        InitCLISwitchPanel()
    End Sub

    Private Sub btnWinOfflineNext1_Click(sender As Object, e As EventArgs) Handles btnWinOfflineNext1.Click
        HideAllPanels()
        If rbnApply.Checked Then
            pnlWinOfflineMain.Visible = True
            lvwApplyOptions.Focus()
        ElseIf rbnBackout.Checked Then
            pnlWinOfflineRemove.Visible = True
            lvwRemoveOptions.Focus()
        ElseIf rbnLearn.Checked Then
            pnlWinOfflineSDHelp.Visible = True
        ElseIf rbnCLIHelp.Checked Then
            pnlWinOfflineCLIHelp.Visible = True
        End If
    End Sub

    Private Sub btnWinOfflineExit1_Click(sender As Object, e As EventArgs) Handles btnWinOfflineExit1.Click, btnWinOfflineExit2.Click, btnWinOfflineExit3.Click, btnWinOfflineExit4.Click
        DialogResult = Windows.Forms.DialogResult.Abort
    End Sub

    Private Sub btnWinOfflineBack1_Click(sender As Object, e As EventArgs) Handles btnWinOfflineBack1.Click
        pnlWinOfflineCLIHelp.Visible = False
        pnlWinOfflineStart.Visible = True
    End Sub

    Private Sub btnWinOfflineBack2_Click(sender As Object, e As EventArgs) Handles btnWinOfflineBack2.Click
        pnlWinOfflineSDHelp.Visible = False
        pnlWinOfflineStart.Visible = True
    End Sub

    Private Sub btnWinOfflineBack3_Click(sender As Object, e As EventArgs) Handles btnWinOfflineBack3.Click
        pnlWinOfflineMain.Visible = False
        pnlWinOfflineStart.Visible = True
    End Sub

    Private Sub btnWinOfflineBack4_Click(sender As Object, e As EventArgs) Handles btnWinOfflineBack4.Click
        pnlWinOfflineRemove.Visible = False
        pnlWinOfflineStart.Visible = True
    End Sub

    Private Sub btnWinOfflineSwicthes_Click(sender As Object, e As EventArgs) Handles btnWinOfflineSwicthes.Click
        Dim CLIHelp As New HelpUI
        CLIHelp.ShowDialog()
    End Sub

    Private Sub btnWinOfflineStart_Click(sender As Object, e As EventArgs) Handles btnWinOfflineStart1.Click, btnWinOfflineStart2.Click

        Dim CombinedRemovalList As String = ""

        ' Iterate ApplyList for any switch settings
        ' Note: As settings are mirrored, it's not necessary to check RemoveListView options.
        For Each item As ListViewItem In lvwApplyOptions.Items
            If item.Name.Equals("CleanupDSMLogsItem") And item.Checked Then
                Globals.CleanupLogsSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Cleanup DSM logs folder --> Set")
            ElseIf item.Name.Equals("ResetCftraceItem") And item.Checked Then
                Globals.ResetCftraceSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Reset cftrace level --> Set")
            ElseIf item.Name.Equals("RemoveCAMConfigItem") And item.Checked Then
                Globals.RemoveCAMConfigSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Remove CAM configuration file --> Set")
            ElseIf item.Name.Equals("CleanupAgentItem") And item.Checked Then
                Globals.CleanupAgentSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Perform agent cleanup --> Set")
            ElseIf item.Name.Equals("CleanupServerItem") And item.Checked Then
                Globals.CleanupServerSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Perform scalability server cleanup --> Set")
            ElseIf item.Name.Equals("CheckLibraryItem") And item.Checked Then
                Globals.CheckSDLibrarySwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Analyze software library without making changes --> Set")
            ElseIf item.Name.Equals("CleanLibraryItem") And item.Checked Then
                Globals.CleanupSDLibrarySwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Perform software library cleanup --> Set")
            ElseIf item.Name.Equals("CleanupCertItem") And item.Checked Then
                Globals.CleanupCertStoreSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Certificate store cleanup --> Set")
            ElseIf item.Name.Equals("SkipCAFStartUpItem") And item.Checked Then
                Globals.SkipCAFStartUpSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Skip CAF startup --> Set")
            ElseIf item.Name.Equals("SkipCAMItem") And item.Checked Then
                Globals.SkipCAM = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Don't stop CAM --> Set")
            ElseIf item.Name.Equals("SkipDMPrimerItem") And item.Checked Then
                Globals.SkipDMPrimer = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Don't stop DMPrimer --> Set")
            ElseIf item.Name.Equals("SkiphmAgentItem") And item.Checked Then
                Globals.SkiphmAgent = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Don't stop hmAgent --> Set")
            ElseIf item.Name.Equals("LaunchGuiItem") And item.Checked Then
                Globals.LaunchGuiSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Launch DSM Explorer --> Set")
            ElseIf item.Name.Equals("SimulateCafStopItem") And item.Checked Then
                Globals.SimulateCafStopSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Simulate recycling CAF --> Set")
            ElseIf item.Name.Equals("SimulatePatchItem") And item.Checked Then
                Globals.SimulatePatchSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Simulate patch operations --> Set")
            ElseIf item.Name.Equals("SimulatePatchErrorItem") And item.Checked Then
                Globals.SimulatePatchErrorSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Simulate patch operations --> Set")
            ElseIf item.Name.Equals("RemoveHistoryBeforeItem") And item.Checked Then
                Globals.RemoveHistoryBeforeSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Remove patch history file, BEFORE patch operations --> Set")
            ElseIf item.Name.Equals("RemoveHistoryAfterItem") And item.Checked Then
                Globals.RemoveHistoryAfterSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Remove patch history file, AFTER patch operations --> Set")
            ElseIf item.Name.Equals("DumpCazipxpItem") And item.Checked Then
                Globals.DumpCazipxpSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Extract cazipxp.exe utility and exit without changes --> Set")
            ElseIf item.Name.Equals("RegenHostUUIDItem") And item.Checked Then
                Globals.RegenHostUUIDSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Regenerate agent HostUUID --> Set")
            ElseIf item.Name.Equals("SDSignalRebootItem") And item.Checked Then
                Globals.SDSignalRebootSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Signal software delivery reboot request --> Set")
            ElseIf item.Name.Equals("RebootSystemItem") And item.Checked Then
                Globals.RebootSystemSwitch = True
                Logger.WriteDebug(Logger.LastCallStack, "Switch: Reboot system after completion --> Set")
            End If
        Next

        ' Check if the -remove switch needs to be "unset"
        ' Note: The user may have supplied the "-remove" switch via command line,
        '       but chose the Apply action from the GUI menu.
        If sender.Equals(btnWinOfflineStart1) And Globals.RemovePatchSwitch Then
            Globals.RemovePatchSwitch = False
            Logger.WriteDebug(Logger.LastCallStack, "Switch: Remove patches --> Unset")
            For x As Integer = 0 To Globals.CommandLineArgs.Length - 1
                If Globals.CommandLineArgs(x).ToLower.Contains("backout") Or Globals.CommandLineArgs(x).ToLower.Contains("remove") Then
                    Utility.RemoveFirstFromExistingArray(Globals.CommandLineArgs, Globals.CommandLineArgs(x))
                    Utility.RemoveFirstFromExistingArray(Globals.CommandLineArgs, Globals.CommandLineArgs(x))
                End If
                If x >= Globals.CommandLineArgs.Length - 1 Then Exit For
            Next

        ElseIf sender.Equals(btnWinOfflineStart2) And Globals.RemovePatchSwitch = False Then
            Globals.RemovePatchSwitch = True
            Utility.AddToExistingArray(Globals.CommandLineArgs, "-remove")
            For Each ListItem As ListViewItem In lvwHistory.Items
                If ListItem.Checked Then
                    CombinedRemovalList += ListItem.Text + ","
                End If
            Next
            Utility.AddToExistingArray(Globals.CommandLineArgs, CombinedRemovalList.Substring(0, CombinedRemovalList.Length - 1))
            Logger.WriteDebug(Logger.LastCallStack, "Switch: Remove patches --> Set")
        End If

        Manifest.ResetRemovalManifest()

        ' Iterate history list view -- Update removal item selection
        For Each item As ListViewItem In lvwHistory.Items
            If item.Checked Then
                Manifest.UpdateManifest(Logger.LastCallStack, Manifest.REMOVAL_MANIFEST, New RemovalVector(item.Text))
            End If
        Next

        DialogResult = Windows.Forms.DialogResult.OK

    End Sub

End Class
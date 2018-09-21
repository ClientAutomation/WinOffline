Imports System.Threading

Partial Public Class WinOfflineUI

    Private Shared RemovalToolThread As Thread

    Private Sub rbnRemoveITCM_CheckedChanged(sender As Object, e As EventArgs) Handles rbnRemoveITCM.CheckedChanged
        If rbnRemoveITCM.Checked Then
            Globals.RemoveITCM = True
            Globals.UninstallITCM = False
            Delegate_Sub_Enable_Control(chkRetainHostUUID, True)
        End If
    End Sub

    Private Sub rbnUninstallITCM_CheckedChanged(sender As Object, e As EventArgs) Handles rbnUninstallITCM.CheckedChanged
        If rbnUninstallITCM.Checked Then
            Globals.RemoveITCM = False
            Globals.UninstallITCM = True
            chkRetainHostUUID.Checked = False
            Delegate_Sub_Enable_Control(chkRetainHostUUID, False)
        End If
    End Sub

    Private Sub chkRetainHostUUID_CheckedChanged(sender As Object, e As EventArgs) Handles chkRetainHostUUID.CheckedChanged
        If chkRetainHostUUID.Checked Then
            Globals.KeepHostUUIDSwitch = True
        Else
            Globals.KeepHostUUIDSwitch = False
        End If
    End Sub

    Private Sub btnRemoveITCM_Click(sender As Object, e As EventArgs) Handles btnRemoveITCM.Click
        Delegate_Sub_Enable_Control(btnRemoveITCM, False)
        Delegate_Sub_Enable_Control(rbnRemoveITCM, False)
        Delegate_Sub_Enable_Control(chkRetainHostUUID, False)
        Delegate_Sub_Enable_Control(rbnUninstallITCM, False)
        Delegate_Sub_Enable_Control(lblRemoveITCMCaution, False)
        txtRemovalTool.Text = ""
        RemovalToolThread = New Thread(Sub() WinOffline.RemoveITCM(WinOffline.Logger.LastCallStack))
        RemovalToolThread.Start()
    End Sub

    Private Sub btnExitRemoveITCM_Click(sender As Object, e As EventArgs) Handles btnExitRemoveITCM.Click
        Close()
    End Sub

End Class
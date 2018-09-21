Imports System.Windows.Forms

Public Class SqlConnectUI

    Private Sub SqlConnectUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = Globals.ProcessFriendlyName + " -- " + Globals.AppVersion
        cmbAuthType.Items.Add(WinOfflineUI.WINDOWS_AUTHENTICATION)
        cmbAuthType.Items.Add(WinOfflineUI.SQL_AUTHENTICATION)
        If Globals.DatabaseServer IsNot Nothing AndAlso Not Globals.DatabaseServer.Equals("") Then txtSqlServer.Text = Globals.DatabaseServer Else txtSqlServer.Text = WinOfflineUI.SqlServer
        If Globals.DatabaseInstance IsNot Nothing AndAlso Not Globals.DatabaseInstance.Equals("") Then txtInstanceName.Text = Globals.DatabaseInstance Else txtInstanceName.Text = WinOfflineUI.InstanceName
        If Globals.DatabasePort IsNot Nothing AndAlso Not Globals.DatabasePort.Equals("") Then txtPortNumber.Text = Globals.DatabasePort Else txtPortNumber.Text = WinOfflineUI.PortNumber
        If Globals.DatabaseName IsNot Nothing AndAlso Not Globals.DatabaseName.Equals("") Then txtDatabaseName.Text = Globals.DatabaseName Else txtDatabaseName.Text = WinOfflineUI.DatabaseName
        cmbAuthType.SelectedItem = WinOfflineUI.AuthType
        txtSqlUser.Text = WinOfflineUI.SqlUser
        txtSqlPassword.Text = WinOfflineUI.SqlPassword
    End Sub

    Private Sub btnSqlConnect_Click(sender As Object, e As EventArgs) Handles btnSqlConnect.Click
        WinOfflineUI.SqlServer = txtSqlServer.Text
        WinOfflineUI.InstanceName = txtInstanceName.Text
        WinOfflineUI.PortNumber = txtPortNumber.Text
        WinOfflineUI.DatabaseName = txtDatabaseName.Text
        WinOfflineUI.AuthType = cmbAuthType.SelectedItem
        WinOfflineUI.SqlUser = txtSqlUser.Text
        WinOfflineUI.SqlPassword = txtSqlPassword.Text
        Close()
    End Sub

    Private Sub btnSqlCancel_Click(sender As Object, e As EventArgs) Handles btnSqlCancel.Click
        DialogResult = DialogResult.Abort
        Close()
    End Sub

    Private Sub txtSqlServer_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtSqlServer.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtSqlServer_TextChanged(sender As Object, e As EventArgs) Handles txtSqlServer.TextChanged
        If Not txtSqlServer.Text.Equals("") Then
            btnSqlConnect.Enabled = True
        Else
            btnSqlConnect.Enabled = False
        End If
        If txtSqlServer.Text.Contains("\") Then
            txtInstanceName.Enabled = False
            txtInstanceName.Text = ""
        Else
            txtInstanceName.Enabled = True
        End If
        If txtSqlServer.Text.Contains(",") Then
            txtPortNumber.Enabled = False
            txtPortNumber.Text = ""
        Else
            txtPortNumber.Enabled = True
        End If
    End Sub

    Private Sub txtSqlInstance_EnabledChanged(sender As Object, e As EventArgs) Handles txtInstanceName.EnabledChanged
        If txtInstanceName.Enabled AndAlso txtSqlServer.Text.Contains("\") Then
            txtInstanceName.Enabled = False
        End If
    End Sub

    Private Sub txtSqlPort_EnabledChanged(sender As Object, e As EventArgs) Handles txtPortNumber.EnabledChanged
        If txtPortNumber.Enabled AndAlso txtSqlServer.Text.Contains(",") Then
            txtPortNumber.Enabled = False
        End If
    End Sub

    Private Sub cmbAuthType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAuthType.SelectedIndexChanged, cmbAuthType.EnabledChanged
        If cmbAuthType.SelectedItem.Equals(WinOfflineUI.WINDOWS_AUTHENTICATION) Then
            ' Windows authentication -- disable username/password
            txtSqlUser.Enabled = False
            txtSqlPassword.Enabled = False
        Else
            ' Sql authentication -- enable username/password
            txtSqlUser.Enabled = True
            txtSqlPassword.Enabled = True
        End If
    End Sub

    Private Sub txtSqlPassword_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtSqlPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub SqlConnectUI_KeyDown(sender As Object, ByVal e As KeyEventArgs) Handles txtSqlServer.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.D1 Then
            txtSqlServer.Text = "fonbr01-e3235.gsd.local"
            txtInstanceName.Text = ""
            txtPortNumber.Text = "1433"
            txtDatabaseName.Text = "mdb"
            cmbAuthType.SelectedItem = WinOfflineUI.SQL_AUTHENTICATION
            txtSqlUser.Text = "ca_itrm"
            txtSqlPassword.Text = "Passw0rd123"
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.D2 Then
            txtSqlServer.Text = "fonbr01-e7249.gsd.local"
            txtInstanceName.Text = ""
            txtPortNumber.Text = "1433"
            txtDatabaseName.Text = "mdb"
            cmbAuthType.SelectedItem = WinOfflineUI.SQL_AUTHENTICATION
            txtSqlUser.Text = "ca_itrm"
            txtSqlPassword.Text = "Passw0rd123"
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.D3 Then
            txtSqlServer.Text = "fonbr01-e3236.gsd.local"
            txtInstanceName.Text = ""
            txtPortNumber.Text = "1433"
            txtDatabaseName.Text = "mdb"
            cmbAuthType.SelectedItem = WinOfflineUI.SQL_AUTHENTICATION
            txtSqlUser.Text = "ca_itrm"
            txtSqlPassword.Text = "Passw0rd123"
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.D4 Then
            txtSqlServer.Text = "fonbr01-e3237.gsd.local"
            txtInstanceName.Text = ""
            txtPortNumber.Text = "1433"
            txtDatabaseName.Text = "mdb"
            cmbAuthType.SelectedItem = WinOfflineUI.SQL_AUTHENTICATION
            txtSqlUser.Text = "ca_itrm"
            txtSqlPassword.Text = "Passw0rd123"
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.D5 Then
            txtSqlServer.Text = "fonbr01-e3238.gsd.local"
            txtInstanceName.Text = ""
            txtPortNumber.Text = "1433"
            txtDatabaseName.Text = "mdb"
            cmbAuthType.SelectedItem = WinOfflineUI.SQL_AUTHENTICATION
            txtSqlUser.Text = "ca_itrm"
            txtSqlPassword.Text = "Passw0rd123"
            btnSqlConnect.PerformClick()
            e.SuppressKeyPress = True
        End If

    End Sub

End Class
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Partial Public Class WinOfflineUI

    Private Shared SqlCleanAppsThread As Thread
    Private Shared CafControlThread As Thread
    Private Shared ButtonMonitorThread As Thread

    Private Sub InitSqlCleanApps()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectCleanApps, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, False)

        If WinOffline.Utility.IsProcessRunning("caf.exe", "service") Then
            Delegate_Sub_Set_Text(btnSqlCafStopCleanApps, "&Stop CAF")
        Else
            Delegate_Sub_Set_Text(btnSqlCafStopCleanApps, "&Start CAF")
        End If

    End Sub

    Private Sub btnSqlConnectCleanApps_Click(sender As Object, e As EventArgs) Handles btnSqlConnectCleanApps.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectCleanApps_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectCleanApps.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlCafStopCleanApps_Click(sender As Object, e As EventArgs) Handles btnSqlCafStopCleanApps.Click

        ' Check if manager name matches the current hostname
        If Not ManagerName.ToLower.Equals(Globals.HostName.ToLower) Then
            AlertBox.CreateUserAlert("Functionality unavailable. You are running " +
                                     Globals.ProcessFriendlyName + " remotely from " + ManagerName + ". " +
                                     "It is recommended to stop CAF on " + ManagerName + " before running the cleanup.")
            Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, False)
            Return
        End If

        If btnSqlCafStopCleanApps.Text.ToLower.Equals("&stop caf") Then
            Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, False)
            Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, False)
            CafControlThread = New Thread(Sub() StopCAF(txtSqlCleanApps))
            CafControlThread.Start()
            ButtonMonitorThread = New Thread(Sub() ButtonMonitorWorker())
            ButtonMonitorThread.Start()

        ElseIf btnSqlCafStopCleanApps.Text.ToLower.Equals("&start caf") Then
            Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, False)
            Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, False)
            CafControlThread = New Thread(Sub() StartCAF(txtSqlCleanApps))
            CafControlThread.Start()
            ButtonMonitorThread = New Thread(Sub() ButtonMonitorWorker())
            ButtonMonitorThread.Start()
        End If

    End Sub

    Private Sub ButtonMonitorWorker()

        CafControlThread.Join()

        If WinOffline.Utility.IsProcessRunning("caf.exe", "service") Then
            Delegate_Sub_Set_Text(btnSqlCafStopCleanApps, "&Stop CAF")
        Else
            Delegate_Sub_Set_Text(btnSqlCafStopCleanApps, "&Start CAF")
        End If

        Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, True)
        Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, True)

    End Sub

    Private Sub btnSqlRunCleanApps_Click(sender As Object, e As EventArgs) Handles btnSqlRunCleanApps.Click

        Dim SqlScript As String = My.Resources.SQL_CleanApps_Unified
        Dim ConnectionString As String

        If btnSqlRunCleanApps.Text.ToLower.Equals("&run cleanapps") Then
            ConnectionString = BuildSqlConnectionString()
            If ConnectionString Is Nothing Then
                Return
            End If
            Delegate_Sub_Set_Text(btnSqlRunCleanApps, "C&ancel CleanApps")
            Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, False)
            Delegate_Sub_Set_Text(txtSqlCleanApps, "")
            SqlCleanAppsThread = New Thread(Sub() SqlCleanAppsWorker(ConnectionString, SqlScript))
            SqlCleanAppsThread.Start()

        ElseIf btnSqlRunCleanApps.Text.ToLower.Equals("c&ancel cleanapps") Then
            CancelSignal = True
            Delegate_Sub_Append_Text(txtSqlCleanApps, "Cleanup cancelled by user.")
            Delegate_Sub_Set_Text(btnSqlRunCleanApps, "&Run CleanApps")
            If Not ManagerName.ToLower.Equals(Globals.HostName.ToLower) Then
                Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, False)
            End If
        End If

    End Sub

    Private Sub btnSqlExitCleanApps_Click(sender As Object, e As EventArgs) Handles btnSqlExitCleanApps.Click
        Close()
    End Sub

    Private Sub SqlCleanAppsWorker(ByVal ConnectionString As String, ByVal SqlScript As String)

        Dim DbConnection As SqlConnection
        Dim SqlBatches As String()
        Dim SqlCmd As SqlCommand
        Dim SqlData As SqlDataReader
        Dim CallStack As String = "SqlCleanAppsWorker --> "

        Try
            DbConnection = New SqlConnection(ConnectionString)
            AddHandler DbConnection.InfoMessage, New SqlInfoMessageEventHandler(AddressOf OnSqlCleanAppsInfoMessage)
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            SqlBatches = Regex.Split(SqlScript, "^\s*GO\s*\d*\s*($|\-\-.*$)", RegexOptions.Multiline Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase)

            For Each QueryLine As String In SqlBatches
                If QueryLine.Equals("") Then Continue For
                SqlCmd = New SqlCommand(QueryLine, DbConnection)
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement(s):" + Environment.NewLine + QueryLine)
                SqlData = SqlCmd.ExecuteReader()

                If TerminateSignal Or CancelSignal Then
                    SqlData.Close()
                    DbConnection.Close()
                    Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
                    Return
                End If

                While SqlData.Read
                    If TerminateSignal Or CancelSignal Then
                        SqlData.Close()
                        DbConnection.Close()
                        Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
                        Return
                    End If
                End While

                SqlData.Close()
            Next

            DbConnection.Close()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not TerminateSignal Then
                Delegate_Sub_Set_Text(btnSqlRunCleanApps, "&Run CleanApps")
                Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, True)
            End If
        End Try

    End Sub

    Private Sub OnSqlCleanAppsInfoMessage(ByVal sender As Object, ByVal e As SqlInfoMessageEventArgs)
        Delegate_Sub_Append_Text(txtSqlCleanApps, e.Message)
    End Sub

End Class
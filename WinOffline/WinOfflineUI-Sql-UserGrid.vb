Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared UserGridThread As Thread

    Private Sub InitSqlUserGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectUserGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshUserGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportUserGrid, False)

        dgvUserGrid.AllowUserToAddRows = False
        dgvUserGrid.AllowUserToDeleteRows = False
        dgvUserGrid.AllowUserToResizeRows = False
        dgvUserGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvUserGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvUserGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvUserGrid.BorderStyle = BorderStyle.Fixed3D
        dgvUserGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvUserGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvUserGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvUserGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvUserGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvUserGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvUserGrid.DefaultCellStyle.WrapMode = False
        dgvUserGrid.EnableHeadersVisualStyles = False
        dgvUserGrid.ReadOnly = True
        dgvUserGrid.RowHeadersVisible = False
        dgvUserGrid.ScrollBars = ScrollBars.Both
        dgvUserGrid.ShowCellErrors = False
        dgvUserGrid.ShowCellToolTips = False
        dgvUserGrid.ShowEditingIcon = False
        dgvUserGrid.ShowRowErrors = False

        dgvUserObsolete90.AllowUserToAddRows = False
        dgvUserObsolete90.AllowUserToDeleteRows = False
        dgvUserObsolete90.AllowUserToResizeRows = False
        dgvUserObsolete90.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvUserObsolete90.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvUserObsolete90.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvUserObsolete90.BorderStyle = BorderStyle.Fixed3D
        dgvUserObsolete90.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvUserObsolete90.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvUserObsolete90.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvUserObsolete90.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvUserObsolete90.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvUserObsolete90.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvUserObsolete90.DefaultCellStyle.WrapMode = False
        dgvUserObsolete90.EnableHeadersVisualStyles = False
        dgvUserObsolete90.ReadOnly = True
        dgvUserObsolete90.RowHeadersVisible = False
        dgvUserObsolete90.ScrollBars = ScrollBars.Both
        dgvUserObsolete90.ShowCellErrors = False
        dgvUserObsolete90.ShowCellToolTips = False
        dgvUserObsolete90.ShowEditingIcon = False
        dgvUserObsolete90.ShowRowErrors = False

        dgvUserObsolete365.AllowUserToAddRows = False
        dgvUserObsolete365.AllowUserToDeleteRows = False
        dgvUserObsolete365.AllowUserToResizeRows = False
        dgvUserObsolete365.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvUserObsolete365.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvUserObsolete365.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvUserObsolete365.BorderStyle = BorderStyle.Fixed3D
        dgvUserObsolete365.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvUserObsolete365.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvUserObsolete365.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvUserObsolete365.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvUserObsolete365.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvUserObsolete365.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvUserObsolete365.DefaultCellStyle.WrapMode = False
        dgvUserObsolete365.EnableHeadersVisualStyles = False
        dgvUserObsolete365.ReadOnly = True
        dgvUserObsolete365.RowHeadersVisible = False
        dgvUserObsolete365.ScrollBars = ScrollBars.Both
        dgvUserObsolete365.ShowCellErrors = False
        dgvUserObsolete365.ShowCellToolTips = False
        dgvUserObsolete365.ShowEditingIcon = False
        dgvUserObsolete365.ShowRowErrors = False

        ' Force all tabs to draw (otherwise data grid views won't format during population)
        Delegate_Sub_Set_Active_Tab(tabCtrlUserGrid, tabUserObsolete365)
        Delegate_Sub_Set_Active_Tab(tabCtrlUserGrid, tabUserObsolete90)
        Delegate_Sub_Set_Active_Tab(tabCtrlUserGrid, tabUserSummary)

    End Sub

    Private Sub UserGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "UserGridWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            tabCtrlUserGrid.Invoke(Sub() tabCtrlUserGrid.Height = tabCtrlUserGrid.Height - prgUserGrid.Height - 4)

            ' Query user summary
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select du.label as 'User', isnull(t0.link_count, '-') as '# Linked Computers', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, agt.last_run_date, '19700101')) as 'Last Run Date', isnull(srv.host_name, '-') as 'Scalability Server' from ca_discovered_user du with (nolock) left join ca_agent agt with (nolock) on du.user_uuid=agt.object_uuid and agt.agent_type=2 left join ca_server srv with (nolock) on agt.server_uuid=srv.server_uuid left join (select distinct user_uuid, count(*) as link_count from ca_link_dis_hw_user with (nolock) group by user_uuid) t0 on du.user_uuid=t0.user_uuid where agt.agent_type=2 order by du.label, agt.last_run_date",
                                          dgvUserGrid,
                                          "User",
                                          DataGridViewAutoSizeColumnsMode.AllCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgUserGrid,
                                          tabUserSummary)

            ' Query obsolete user 90 day summary
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select du.label as 'User', isnull(t0.link_count, '-') as '# Linked Computers', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, agt.last_run_date, '19700101')) as 'Last Run Date', isnull(srv.host_name, '-') as 'Scalability Server' from ca_discovered_user du with (nolock) left join ca_agent agt with (nolock) on du.user_uuid=agt.object_uuid and agt.agent_type=2 left join ca_server srv with (nolock) on agt.server_uuid=srv.server_uuid left join (select distinct user_uuid, count(*) as link_count from ca_link_dis_hw_user with (nolock) group by user_uuid) t0 on du.user_uuid=t0.user_uuid where agt.agent_type=2 and agt.last_run_date <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (86400 * 90)) order by du.label, agt.last_run_date",
                                          dgvUserObsolete90,
                                          "User",
                                          DataGridViewAutoSizeColumnsMode.AllCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgUserGrid,
                                          tabUserObsolete90)

            ' Query obsolete user 1 year summary
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select du.label as 'User', isnull(t0.link_count, '-') as '# Linked Computers', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, agt.last_run_date, '19700101')) as 'Last Run Date', isnull(srv.host_name, '-') as 'Scalability Server' from ca_discovered_user du with (nolock) left join ca_agent agt with (nolock) on du.user_uuid=agt.object_uuid and agt.agent_type=2 left join ca_server srv with (nolock) on agt.server_uuid=srv.server_uuid left join (select distinct user_uuid, count(*) as link_count from ca_link_dis_hw_user with (nolock) group by user_uuid) t0 on du.user_uuid=t0.user_uuid where agt.agent_type=2 and agt.last_run_date <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (86400 * 365)) order by du.label, agt.last_run_date",
                                          dgvUserObsolete365,
                                          "User",
                                          DataGridViewAutoSizeColumnsMode.AllCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgUserGrid,
                                          tabUserObsolete365)

            tabCtrlUserGrid.Invoke(Sub() tabCtrlUserGrid.Height = pnlSqlUserGrid.Height - pnlSqlUserGridButtons.Height - 3)

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshUserGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportUserGrid, True)
        End Try

    End Sub

    Private Sub btnSqlConnectUserGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectUserGrid.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectUserGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectUserGrid.Click
        If tabCtrlUserGrid.Height + prgUserGrid.Height + pnlSqlUserGridButtons.Height < pnlSqlUserGrid.Height Then
            tabCtrlUserGrid.Invoke(Sub() tabCtrlUserGrid.Height = tabCtrlUserGrid.Height + prgUserGrid.Height)
        End If
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshUserGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshUserGrid.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshUserGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportUserGrid, False)
        If tabUserSummary.Text.Contains("(") Then Delegate_Sub_Set_Text(tabUserSummary, tabUserSummary.Text.Substring(0, tabUserSummary.Text.IndexOf("(") - 1))
        If tabUserObsolete90.Text.Contains("(") Then Delegate_Sub_Set_Text(tabUserObsolete90, tabUserObsolete90.Text.Substring(0, tabUserObsolete90.Text.IndexOf("(") - 1))
        If tabUserObsolete365.Text.Contains("(") Then Delegate_Sub_Set_Text(tabUserObsolete365, tabUserObsolete365.Text.Substring(0, tabUserObsolete365.Text.IndexOf("(") - 1))
        UserGridThread = New Thread(Sub() UserGridWorker(ConnectionString))
        UserGridThread.Start()
    End Sub

    Private Sub btnSqlExportUserGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportUserGrid.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            If tabCtrlUserGrid.SelectedTab.Equals(tabUserSummary) Then
                For Each dgvColumn As DataGridViewColumn In dgvUserGrid.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvUserGrid.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlUserGrid.SelectedTab.Equals(tabUserObsolete90) Then
                For Each dgvColumn As DataGridViewColumn In dgvUserObsolete90.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvUserObsolete90.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlUserGrid.SelectedTab.Equals(tabUserObsolete365) Then
                For Each dgvColumn As DataGridViewColumn In dgvUserObsolete365.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvUserObsolete365.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next
            End If

            StateStreamWriter.Close()
        Catch ex As Exception
            AlertBox.CreateUserAlert("Export failed." + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message)
        End Try

    End Sub

    Private Sub btnExitSqlUserGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitUserGrid.Click
        Close()
    End Sub

End Class
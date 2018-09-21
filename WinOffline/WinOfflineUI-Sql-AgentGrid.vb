Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared AgentGridThread As Thread

    Private Sub InitSqlAgentGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectAgentGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshAgentGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportAgentGrid, False)

        dgvAgentGrid.AllowUserToAddRows = False
        dgvAgentGrid.AllowUserToDeleteRows = False
        dgvAgentGrid.AllowUserToResizeRows = False
        dgvAgentGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvAgentGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAgentGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAgentGrid.BorderStyle = BorderStyle.Fixed3D
        dgvAgentGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvAgentGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvAgentGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvAgentGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAgentGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvAgentGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvAgentGrid.DefaultCellStyle.WrapMode = False
        dgvAgentGrid.EnableHeadersVisualStyles = False
        dgvAgentGrid.ReadOnly = True
        dgvAgentGrid.RowHeadersVisible = False
        dgvAgentGrid.ScrollBars = ScrollBars.Both
        dgvAgentGrid.ShowCellErrors = False
        dgvAgentGrid.ShowCellToolTips = False
        dgvAgentGrid.ShowEditingIcon = False
        dgvAgentGrid.ShowRowErrors = False

        dgvAgentObsolete90.AllowUserToAddRows = False
        dgvAgentObsolete90.AllowUserToDeleteRows = False
        dgvAgentObsolete90.AllowUserToResizeRows = False
        dgvAgentObsolete90.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvAgentObsolete90.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAgentObsolete90.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAgentObsolete90.BorderStyle = BorderStyle.Fixed3D
        dgvAgentObsolete90.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvAgentObsolete90.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvAgentObsolete90.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvAgentObsolete90.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAgentObsolete90.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvAgentObsolete90.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvAgentObsolete90.DefaultCellStyle.WrapMode = False
        dgvAgentObsolete90.EnableHeadersVisualStyles = False
        dgvAgentObsolete90.ReadOnly = True
        dgvAgentObsolete90.RowHeadersVisible = False
        dgvAgentObsolete90.ScrollBars = ScrollBars.Both
        dgvAgentObsolete90.ShowCellErrors = False
        dgvAgentObsolete90.ShowCellToolTips = False
        dgvAgentObsolete90.ShowEditingIcon = False
        dgvAgentObsolete90.ShowRowErrors = False

        dgvAgentObsolete365.AllowUserToAddRows = False
        dgvAgentObsolete365.AllowUserToDeleteRows = False
        dgvAgentObsolete365.AllowUserToResizeRows = False
        dgvAgentObsolete365.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvAgentObsolete365.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAgentObsolete365.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAgentObsolete365.BorderStyle = BorderStyle.Fixed3D
        dgvAgentObsolete365.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvAgentObsolete365.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvAgentObsolete365.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvAgentObsolete365.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAgentObsolete365.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvAgentObsolete365.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvAgentObsolete365.DefaultCellStyle.WrapMode = False
        dgvAgentObsolete365.EnableHeadersVisualStyles = False
        dgvAgentObsolete365.ReadOnly = True
        dgvAgentObsolete365.RowHeadersVisible = False
        dgvAgentObsolete365.ScrollBars = ScrollBars.Both
        dgvAgentObsolete365.ShowCellErrors = False
        dgvAgentObsolete365.ShowCellToolTips = False
        dgvAgentObsolete365.ShowEditingIcon = False
        dgvAgentObsolete365.ShowRowErrors = False

        ' Force all tabs to draw (otherwise data grid views won't format during population)
        Delegate_Sub_Set_Active_Tab(tabCtrlAgentGrid, tabAgentObsolete365)
        Delegate_Sub_Set_Active_Tab(tabCtrlAgentGrid, tabAgentObsolete90)
        Delegate_Sub_Set_Active_Tab(tabCtrlAgentGrid, tabAgentSummary)

    End Sub

    Private Sub AgentGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "AgentGridWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            tabCtrlAgentGrid.Invoke(Sub() tabCtrlAgentGrid.Height = tabCtrlAgentGrid.Height - prgAgentGrid.Height - 4)

            ' Query agent summary
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select agent_name as 'Agent', ISNULL(agtc.agent_component_version,'-') as 'Version', ISNULL(agt.ip_address,'-') as 'IP Address', ISNULL(t0.inv_count,0) as 'Inventory Count', ISNULL(t1.sw_count,0) as 'Software Count', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, agt.last_run_date, '19700101')) as 'Last Run Date', ISNULL(srv.host_name,'-') as 'Scalability Server' from ca_agent agt with (nolock) left join ca_agent_component agtc with (nolock) on agt.object_uuid=agtc.object_uuid and agt.agent_type=1 and agtc.agent_comp_id=5 left join (select distinct object_uuid as agt_uuid, count(*) as inv_count from inv_generalinventory_item with (nolock) group by object_uuid) as t0 on agt.object_uuid=t0.agt_uuid left join (select distinct asset_source_uuid as agt_uuid, count(*) as sw_count from ca_discovered_software with (nolock) group by asset_source_uuid) as t1 on agt.object_uuid=t1.agt_uuid left join ca_server srv with (nolock) on agt.server_uuid=srv.server_uuid where agt.agent_type=1 order by agent_name",
                                  dgvAgentGrid,
                                  "Agent",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgAgentGrid,
                                  tabAgentSummary)

            ' Query obsolete agent 90 day summary
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select agent_name as 'Agent', ISNULL(agtc.agent_component_version,'-') as 'Version', ISNULL(agt.ip_address,'-') as 'IP Address', ISNULL(t0.inv_count,0) as 'Inventory Count', ISNULL(t1.sw_count,0) as 'Software Count', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, agt.last_run_date, '19700101')) as 'Last Run Date', srv.host_name as 'Scalability Server' from ca_agent agt with (nolock) left join ca_agent_component agtc with (nolock) on agt.object_uuid=agtc.object_uuid and agt.agent_type=1 and agtc.agent_comp_id=5 left join (select distinct object_uuid as agt_uuid, count(*) as inv_count from inv_generalinventory_item with (nolock) group by object_uuid) as t0 on agt.object_uuid=t0.agt_uuid left join (select distinct asset_source_uuid as agt_uuid, count(*) as sw_count from ca_discovered_software with (nolock) group by asset_source_uuid) as t1 on agt.object_uuid=t1.agt_uuid left join ca_server srv with (nolock) on agt.server_uuid=srv.server_uuid where agt.agent_type=1 and agt.last_run_date <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (86400 * 90)) order by agent_name",
                                  dgvAgentObsolete90,
                                  "Agent",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgAgentGrid,
                                  tabAgentObsolete90)

            ' Query obsolete agent 1 year summary
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select agent_name as 'Agent', ISNULL(agtc.agent_component_version,'-') as 'Version', ISNULL(agt.ip_address,'-') as 'IP Address', ISNULL(t0.inv_count,0) as 'Inventory Count', ISNULL(t1.sw_count,0) as 'Software Count', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, agt.last_run_date, '19700101')) as 'Last Run Date', srv.host_name as 'Scalability Server' from ca_agent agt with (nolock) left join ca_agent_component agtc with (nolock) on agt.object_uuid=agtc.object_uuid and agt.agent_type=1 and agtc.agent_comp_id=5 left join (select distinct object_uuid as agt_uuid, count(*) as inv_count from inv_generalinventory_item with (nolock) group by object_uuid) as t0 on agt.object_uuid=t0.agt_uuid left join (select distinct asset_source_uuid as agt_uuid, count(*) as sw_count from ca_discovered_software with (nolock) group by asset_source_uuid) as t1 on agt.object_uuid=t1.agt_uuid left join ca_server srv with (nolock) on agt.server_uuid=srv.server_uuid where agt.agent_type=1 and agt.last_run_date <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (86400 * 365)) order by agent_name",
                                  dgvAgentObsolete365,
                                  "Agent",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgAgentGrid,
                                  tabAgentObsolete365)

            ' Hide progress bar
            tabCtrlAgentGrid.Invoke(Sub() tabCtrlAgentGrid.Height = pnlSqlAgentGrid.Height - pnlSqlAgentGridButtons.Height - 3)

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If

            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshAgentGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportAgentGrid, True)
        End Try

    End Sub

    Private Sub btnSqlConnectAgentGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectAgentGrid.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectAgentGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectAgentGrid.Click
        If tabCtrlAgentGrid.Height + prgAgentGrid.Height + pnlSqlAgentGridButtons.Height < pnlSqlAgentGrid.Height Then
            tabCtrlAgentGrid.Invoke(Sub() tabCtrlAgentGrid.Height = tabCtrlAgentGrid.Height + prgAgentGrid.Height)
        End If
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshAgentGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshAgentGrid.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshAgentGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportAgentGrid, False)
        If tabAgentSummary.Text.Contains("(") Then Delegate_Sub_Set_Text(tabAgentSummary, tabAgentSummary.Text.Substring(0, tabAgentSummary.Text.IndexOf("(") - 1))
        If tabAgentObsolete90.Text.Contains("(") Then Delegate_Sub_Set_Text(tabAgentObsolete90, tabAgentObsolete90.Text.Substring(0, tabAgentObsolete90.Text.IndexOf("(") - 1))
        If tabAgentObsolete365.Text.Contains("(") Then Delegate_Sub_Set_Text(tabAgentObsolete365, tabAgentObsolete365.Text.Substring(0, tabAgentObsolete365.Text.IndexOf("(") - 1))
        AgentGridThread = New Thread(Sub() AgentGridWorker(ConnectionString))
        AgentGridThread.Start()
    End Sub

    Private Sub btnSqlExportAgentGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportAgentGrid.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            If tabCtrlAgentGrid.SelectedTab.Equals(tabAgentSummary) Then

                For Each dgvColumn As DataGridViewColumn In dgvAgentGrid.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvAgentGrid.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlAgentGrid.SelectedTab.Equals(tabAgentObsolete90) Then
                For Each dgvColumn As DataGridViewColumn In dgvAgentObsolete90.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvAgentObsolete90.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlAgentGrid.SelectedTab.Equals(tabAgentObsolete365) Then
                For Each dgvColumn As DataGridViewColumn In dgvAgentObsolete365.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvAgentObsolete365.Rows
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

    Private Sub btnExitSqlAgentGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitAgentGrid.Click
        Close()
    End Sub

End Class
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared ServerGridThread As Thread

    Private Sub InitSqlServerGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectServerGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshServerGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportServerGrid, False)

        dgvServerGrid.AllowUserToAddRows = False
        dgvServerGrid.AllowUserToDeleteRows = False
        dgvServerGrid.AllowUserToResizeRows = False
        dgvServerGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvServerGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvServerGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvServerGrid.BorderStyle = BorderStyle.Fixed3D
        dgvServerGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvServerGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvServerGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvServerGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvServerGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvServerGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvServerGrid.DefaultCellStyle.WrapMode = False
        dgvServerGrid.EnableHeadersVisualStyles = False
        dgvServerGrid.ReadOnly = True
        dgvServerGrid.RowHeadersVisible = False
        dgvServerGrid.ScrollBars = ScrollBars.Both
        dgvServerGrid.ShowCellErrors = False
        dgvServerGrid.ShowCellToolTips = False
        dgvServerGrid.ShowEditingIcon = False
        dgvServerGrid.ShowRowErrors = False

        dgvServerLastCollected24.AllowUserToAddRows = False
        dgvServerLastCollected24.AllowUserToDeleteRows = False
        dgvServerLastCollected24.AllowUserToResizeRows = False
        dgvServerLastCollected24.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvServerLastCollected24.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvServerLastCollected24.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvServerLastCollected24.BorderStyle = BorderStyle.Fixed3D
        dgvServerLastCollected24.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvServerLastCollected24.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvServerLastCollected24.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvServerLastCollected24.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvServerLastCollected24.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvServerLastCollected24.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvServerLastCollected24.DefaultCellStyle.WrapMode = False
        dgvServerLastCollected24.EnableHeadersVisualStyles = False
        dgvServerLastCollected24.ReadOnly = True
        dgvServerLastCollected24.RowHeadersVisible = False
        dgvServerLastCollected24.ScrollBars = ScrollBars.Both
        dgvServerLastCollected24.ShowCellErrors = False
        dgvServerLastCollected24.ShowCellToolTips = False
        dgvServerLastCollected24.ShowEditingIcon = False
        dgvServerLastCollected24.ShowRowErrors = False

        dgvServerSignature30.AllowUserToAddRows = False
        dgvServerSignature30.AllowUserToDeleteRows = False
        dgvServerSignature30.AllowUserToResizeRows = False
        dgvServerSignature30.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvServerSignature30.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvServerSignature30.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvServerSignature30.BorderStyle = BorderStyle.Fixed3D
        dgvServerSignature30.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvServerSignature30.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvServerSignature30.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvServerSignature30.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvServerSignature30.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvServerSignature30.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvServerSignature30.DefaultCellStyle.WrapMode = False
        dgvServerSignature30.EnableHeadersVisualStyles = False
        dgvServerSignature30.ReadOnly = True
        dgvServerSignature30.RowHeadersVisible = False
        dgvServerSignature30.ScrollBars = ScrollBars.Both
        dgvServerSignature30.ShowCellErrors = False
        dgvServerSignature30.ShowCellToolTips = False
        dgvServerSignature30.ShowEditingIcon = False
        dgvServerSignature30.ShowRowErrors = False

        ' Force all tabs to draw (otherwise data grid views won't format during population)
        Delegate_Sub_Set_Active_Tab(tabCtrlServerGrid, tabServerSignature30)
        Delegate_Sub_Set_Active_Tab(tabCtrlServerGrid, tabServerLastCollected24)
        Delegate_Sub_Set_Active_Tab(tabCtrlServerGrid, tabServerSummary)

    End Sub

    Private Sub ServerGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim RecordCount As Integer = 0
        Dim CallStack As String = "ServerGridWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            tabCtrlServerGrid.Invoke(Sub() tabCtrlServerGrid.Height = tabCtrlServerGrid.Height - prgServerGrid.Height - 4)

            If DomainType.Equals("0") Then

                ' Query scalability summary (domain)
                SqlPopulateGridWorker(CallStack,
                                              DbConnection,
                                              "select srv.host_name as 'Scalability Server', count(*) as 'Registered Agents', isnull(Software_Jobs.job_count, 0) as 'Software Jobs', Engine_Task.engine as 'Linked To', Engine_Task.[Task Status] as 'Status', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, srv.validation_interval_date, '19700101')) as 'Last Engine Collect', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gi.item_value_double, '19700101')) as 'Signature File Date' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on agt.server_uuid=srv.server_uuid and agt.agent_type=1 left join (select eng.label as 'engine', case when stat.status = -1 then 'ERROR' when stat.status = 0 then 'WAITING' when stat.status = 1 then 'OK' end as ""Task Status"", convert(binary(16), '0x' + jobs.jocmd,1) as 'server_uuid' from ncjobcfg jobs with (nolock) inner join statjob stat with (nolock) on stat.jobid=jobs.jobid and stat.jdomid=jobs.domainid inner join linkjob link with (nolock) on link.jobid=stat.jobid and link.jdomid=stat.jdomid and link.object_uuid=stat.object_uuid inner join ca_engine eng with (nolock) on eng.engine_uuid=link.object_uuid where jobs.job_category=2 and jobs.jotype=10) as Engine_Task on srv.server_uuid=Engine_Task.server_uuid left join (select srv.server_uuid as 'server_uuid', count(*) as 'job_count' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on srv.server_uuid=agt.server_uuid left join usd_applic app with (nolock) on agt.object_uuid=app.target and agt.agent_type=1 where app.status in (1,2,3,7,8,11,12,27) group by srv.server_uuid) as Software_Jobs on srv.server_uuid=Software_Jobs.server_uuid left join inv_generalinventory_item gi with (nolock) on srv.dis_hw_uuid=gi.object_uuid and gi.item_parent_name_id in (select tree_name_id from inv_tree_name_id with (nolock) where tree_name='$System Status$' and domain_uuid=srv.domain_uuid) and gi.item_name_id in (select item_name_id from inv_item_name_id with (nolock) where item_name='Software Signature File Delivery Date' and domain_uuid=srv.domain_uuid) and gi.domain_uuid=srv.domain_uuid group by srv.host_name, Software_Jobs.job_count, Engine_Task.Engine, Engine_Task.[Task Status], srv.validation_interval_date, gi.item_value_double order by srv.host_name",
                                              dgvServerGrid,
                                              "Scalability Server",
                                              DataGridViewAutoSizeColumnsMode.AllCells,
                                              DataGridViewAutoSizeRowsMode.AllCells,
                                              prgServerGrid,
                                              tabServerSummary)

                ' Query scalability server last collected > 24 hours (domain)
                SqlPopulateGridWorker(CallStack,
                                              DbConnection,
                                              "select srv.host_name as 'Scalability Server', count(*) as 'Registered Agents', isnull(Software_Jobs.job_count, 0) as 'Software Jobs', Engine_Task.engine as 'Linked To', Engine_Task.[Task Status] as 'Status', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, srv.validation_interval_date, '19700101')) as 'Last Engine Collect', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gi.item_value_double, '19700101')) as 'Signature File Date' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on agt.server_uuid=srv.server_uuid and agt.agent_type=1 left join (select eng.label as 'engine', case when stat.status = -1 then 'ERROR' when stat.status = 0 then 'WAITING' when stat.status = 1 then 'OK' end as ""Task Status"", convert(binary(16), '0x' + jobs.jocmd,1) as 'server_uuid' from ncjobcfg jobs with (nolock) inner join statjob stat with (nolock) on stat.jobid=jobs.jobid and stat.jdomid=jobs.domainid inner join linkjob link with (nolock) on link.jobid=stat.jobid and link.jdomid=stat.jdomid and link.object_uuid=stat.object_uuid inner join ca_engine eng with (nolock) on eng.engine_uuid=link.object_uuid where jobs.job_category=2 and jobs.jotype=10) as Engine_Task on srv.server_uuid=Engine_Task.server_uuid left join (select srv.server_uuid as 'server_uuid', count(*) as 'job_count' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on srv.server_uuid=agt.server_uuid left join usd_applic app with (nolock) on agt.object_uuid=app.target and agt.agent_type=1 where app.status in (1,2,3,7,8,11,12,27) group by srv.server_uuid) as Software_Jobs on srv.server_uuid=Software_Jobs.server_uuid left join inv_generalinventory_item gi with (nolock) on srv.dis_hw_uuid=gi.object_uuid and gi.item_parent_name_id in (select tree_name_id from inv_tree_name_id with (nolock) where tree_name='$System Status$' and domain_uuid=srv.domain_uuid) and gi.item_name_id in (select item_name_id from inv_item_name_id with (nolock) where item_name='Software Signature File Delivery Date' and domain_uuid=srv.domain_uuid) and gi.domain_uuid=srv.domain_uuid where srv.validation_interval_date <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (3600 * 24)) group by srv.host_name, Software_Jobs.job_count, Engine_Task.Engine, Engine_Task.[Task Status], srv.validation_interval_date, gi.item_value_double order by srv.host_name",
                                              dgvServerLastCollected24,
                                              "Scalability Server",
                                              DataGridViewAutoSizeColumnsMode.AllCells,
                                              DataGridViewAutoSizeRowsMode.AllCells,
                                              prgServerGrid,
                                              tabServerLastCollected24)

                ' Query scalability server signature > 30 days (domain)
                SqlPopulateGridWorker(CallStack,
                                              DbConnection,
                                              "select srv.host_name as 'Scalability Server', count(*) as 'Registered Agents', isnull(Software_Jobs.job_count, 0) as 'Software Jobs', Engine_Task.engine as 'Linked To', Engine_Task.[Task Status] as 'Status', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, srv.validation_interval_date, '19700101')) as 'Last Engine Collect', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gi.item_value_double, '19700101')) as 'Signature File Date' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on agt.server_uuid=srv.server_uuid and agt.agent_type=1 left join (select eng.label as 'engine', case when stat.status = -1 then 'ERROR' when stat.status = 0 then 'WAITING' when stat.status = 1 then 'OK' end as ""Task Status"", convert(binary(16), '0x' + jobs.jocmd,1) as 'server_uuid' from ncjobcfg jobs with (nolock) inner join statjob stat with (nolock) on stat.jobid=jobs.jobid and stat.jdomid=jobs.domainid inner join linkjob link with (nolock) on link.jobid=stat.jobid and link.jdomid=stat.jdomid and link.object_uuid=stat.object_uuid inner join ca_engine eng with (nolock) on eng.engine_uuid=link.object_uuid where jobs.job_category=2 and jobs.jotype=10) as Engine_Task on srv.server_uuid=Engine_Task.server_uuid left join (select srv.server_uuid as 'server_uuid', count(*) as 'job_count' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on srv.server_uuid=agt.server_uuid left join usd_applic app with (nolock) on agt.object_uuid=app.target and agt.agent_type=1 where app.status in (1,2,3,7,8,11,12,27) group by srv.server_uuid) as Software_Jobs on srv.server_uuid=Software_Jobs.server_uuid left join inv_generalinventory_item gi with (nolock) on srv.dis_hw_uuid=gi.object_uuid and gi.item_parent_name_id in (select tree_name_id from inv_tree_name_id with (nolock) where tree_name='$System Status$' and domain_uuid=srv.domain_uuid) and gi.item_name_id in (select item_name_id from inv_item_name_id with (nolock) where item_name='Software Signature File Delivery Date' and domain_uuid=srv.domain_uuid) and gi.domain_uuid=srv.domain_uuid where gi.item_value_double <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (86400 * 30)) or gi.item_value_double is null group by srv.host_name, Software_Jobs.job_count, Engine_Task.Engine, Engine_Task.[Task Status], srv.validation_interval_date, gi.item_value_double order by srv.host_name",
                                              dgvServerSignature30,
                                              "Scalability Server",
                                              DataGridViewAutoSizeColumnsMode.AllCells,
                                              DataGridViewAutoSizeRowsMode.AllCells,
                                              prgServerGrid,
                                              tabServerSignature30)

            Else

                ' Query scalability summary (enterprise)
                SqlPopulateGridWorker(CallStack,
                                              DbConnection,
                                              "select srv.host_name as 'Scalability Server', count(*) as 'Registered Agents', isnull(Software_Jobs.job_count, 0) as 'Software Jobs', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, srv.validation_interval_date, '19700101')) as 'Last Engine Collect', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gi.item_value_double, '19700101')) as 'Signature File Date' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on agt.server_uuid=srv.server_uuid and agt.agent_type=1 left join (select srv.server_uuid as 'server_uuid', count(*) as 'job_count' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on srv.server_uuid=agt.server_uuid left join usd_applic app with (nolock) on agt.object_uuid=app.target and agt.agent_type=1 where app.status in (1,2,3,7,8,11,12,27) group by srv.server_uuid) as Software_Jobs on srv.server_uuid=Software_Jobs.server_uuid left join inv_generalinventory_item gi with (nolock) on srv.dis_hw_uuid=gi.object_uuid and gi.item_parent_name_id in (select tree_name_id from inv_tree_name_id with (nolock) where tree_name='$System Status$' and domain_uuid=srv.domain_uuid) and gi.item_name_id in (select item_name_id from inv_item_name_id with (nolock) where item_name='Software Signature File Delivery Date' and domain_uuid=srv.domain_uuid) and gi.domain_uuid=srv.domain_uuid group by srv.host_name, Software_Jobs.job_count, srv.validation_interval_date, gi.item_value_double order by srv.host_name",
                                              dgvServerGrid,
                                              "Scalability Server",
                                              DataGridViewAutoSizeColumnsMode.AllCells,
                                              DataGridViewAutoSizeRowsMode.AllCells,
                                              prgServerGrid,
                                              tabServerSummary)

                ' Query scalability server last collected > 24 hours (enterprise)
                SqlPopulateGridWorker(CallStack,
                                              DbConnection,
                                              "select srv.host_name as 'Scalability Server', count(*) as 'Registered Agents', isnull(Software_Jobs.job_count, 0) as 'Software Jobs', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, srv.validation_interval_date, '19700101')) as 'Last Engine Collect', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gi.item_value_double, '19700101')) as 'Signature File Date' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on agt.server_uuid=srv.server_uuid and agt.agent_type=1 left join (select srv.server_uuid as 'server_uuid', count(*) as 'job_count' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on srv.server_uuid=agt.server_uuid left join usd_applic app with (nolock) on agt.object_uuid=app.target and agt.agent_type=1 where app.status in (1,2,3,7,8,11,12,27) group by srv.server_uuid) as Software_Jobs on srv.server_uuid=Software_Jobs.server_uuid left join inv_generalinventory_item gi with (nolock) on srv.dis_hw_uuid=gi.object_uuid and gi.item_parent_name_id in (select tree_name_id from inv_tree_name_id with (nolock) where tree_name='$System Status$' and domain_uuid=srv.domain_uuid) and gi.item_name_id in (select item_name_id from inv_item_name_id with (nolock) where item_name='Software Signature File Delivery Date' and domain_uuid=srv.domain_uuid) and gi.domain_uuid=srv.domain_uuid where srv.validation_interval_date <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (3600 * 24)) group by srv.host_name, Software_Jobs.job_count, srv.validation_interval_date, gi.item_value_double order by srv.host_name",
                                              dgvServerLastCollected24,
                                              "Scalability Server",
                                              DataGridViewAutoSizeColumnsMode.AllCells,
                                              DataGridViewAutoSizeRowsMode.AllCells,
                                              prgServerGrid,
                                              tabServerLastCollected24)

                ' Query scalability server signature > 30 days (enterprise)
                SqlPopulateGridWorker(CallStack,
                                              DbConnection,
                                              "select srv.host_name as 'Scalability Server', count(*) as 'Registered Agents', isnull(Software_Jobs.job_count, 0) as 'Software Jobs', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, srv.validation_interval_date, '19700101')) as 'Last Engine Collect', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gi.item_value_double, '19700101')) as 'Signature File Date' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on agt.server_uuid=srv.server_uuid and agt.agent_type=1 left join (select srv.server_uuid as 'server_uuid', count(*) as 'job_count' from ca_server srv with (nolock) left join ca_agent agt with (nolock) on srv.server_uuid=agt.server_uuid left join usd_applic app with (nolock) on agt.object_uuid=app.target and agt.agent_type=1 where app.status in (1,2,3,7,8,11,12,27) group by srv.server_uuid) as Software_Jobs on srv.server_uuid=Software_Jobs.server_uuid left join inv_generalinventory_item gi with (nolock) on srv.dis_hw_uuid=gi.object_uuid and gi.item_parent_name_id in (select tree_name_id from inv_tree_name_id with (nolock) where tree_name='$System Status$' and domain_uuid=srv.domain_uuid) and gi.item_name_id in (select item_name_id from inv_item_name_id with (nolock) where item_name='Software Signature File Delivery Date' and domain_uuid=srv.domain_uuid) and gi.domain_uuid=srv.domain_uuid where gi.item_value_double <= (DATEDIFF(s, '19700101', GETUTCDATE()) - (86400 * 30)) or gi.item_value_double is null group by srv.host_name, Software_Jobs.job_count, srv.validation_interval_date, gi.item_value_double order by srv.host_name",
                                              dgvServerSignature30,
                                              "Scalability Server",
                                              DataGridViewAutoSizeColumnsMode.AllCells,
                                              DataGridViewAutoSizeRowsMode.AllCells,
                                              prgServerGrid,
                                              tabServerSignature30)

            End If

            tabCtrlServerGrid.Invoke(Sub() tabCtrlServerGrid.Height = pnlSqlServerGrid.Height - pnlSqlServerGridButtons.Height - 3)

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshServerGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportServerGrid, True)
        End Try

    End Sub

    Private Sub btnSqlConnectServerGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectServerGrid.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectServerGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectServerGrid.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshServerGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshServerGrid.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshServerGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportServerGrid, False)
        If tabServerSummary.Text.Contains("(") Then Delegate_Sub_Set_Text(tabServerSummary, tabServerSummary.Text.Substring(0, tabServerSummary.Text.IndexOf("(") - 1))
        If tabServerLastCollected24.Text.Contains("(") Then Delegate_Sub_Set_Text(tabServerLastCollected24, tabServerLastCollected24.Text.Substring(0, tabServerLastCollected24.Text.IndexOf("(") - 1))
        If tabServerSignature30.Text.Contains("(") Then Delegate_Sub_Set_Text(tabServerSignature30, tabServerSignature30.Text.Substring(0, tabServerSignature30.Text.IndexOf("(") - 1))
        ServerGridThread = New Thread(Sub() ServerGridWorker(ConnectionString))
        ServerGridThread.Start()
    End Sub

    Private Sub btnSqlExportServerGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportServerGrid.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            If tabCtrlServerGrid.SelectedTab.Equals(tabServerSummary) Then
                For Each dgvColumn As DataGridViewColumn In dgvServerGrid.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvServerGrid.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlServerGrid.SelectedTab.Equals(tabServerLastCollected24) Then
                For Each dgvColumn As DataGridViewColumn In dgvServerLastCollected24.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvServerLastCollected24.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlServerGrid.SelectedTab.Equals(tabServerSignature30) Then
                For Each dgvColumn As DataGridViewColumn In dgvServerSignature30.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvServerSignature30.Rows
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

    Private Sub btnSqlExitServerGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitServerGrid.Click
        Close()
    End Sub

End Class
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared DuplCompGridThread As Thread

    Private Sub InitSqlDuplCompGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectDuplCompGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshDuplCompGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportDuplCompGrid, False)

        dgvDuplHostname.AllowUserToAddRows = False
        dgvDuplHostname.AllowUserToDeleteRows = False
        dgvDuplHostname.AllowUserToResizeRows = False
        dgvDuplHostname.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDuplHostname.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDuplHostname.BorderStyle = BorderStyle.Fixed3D
        dgvDuplHostname.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDuplHostname.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplHostname.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDuplHostname.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDuplHostname.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDuplHostname.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplHostname.DefaultCellStyle.WrapMode = False
        dgvDuplHostname.EnableHeadersVisualStyles = False
        dgvDuplHostname.ReadOnly = True
        dgvDuplHostname.RowHeadersVisible = False
        dgvDuplHostname.ScrollBars = ScrollBars.Both
        dgvDuplHostname.ShowCellErrors = False
        dgvDuplHostname.ShowCellToolTips = False
        dgvDuplHostname.ShowEditingIcon = False
        dgvDuplHostname.ShowRowErrors = False

        dgvDuplSerialNum.AllowUserToAddRows = False
        dgvDuplSerialNum.AllowUserToDeleteRows = False
        dgvDuplSerialNum.AllowUserToResizeRows = False
        dgvDuplSerialNum.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDuplSerialNum.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDuplSerialNum.BorderStyle = BorderStyle.Fixed3D
        dgvDuplSerialNum.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDuplSerialNum.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplSerialNum.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDuplSerialNum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDuplSerialNum.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDuplSerialNum.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplSerialNum.DefaultCellStyle.WrapMode = False
        dgvDuplSerialNum.EnableHeadersVisualStyles = False
        dgvDuplSerialNum.ReadOnly = True
        dgvDuplSerialNum.RowHeadersVisible = False
        dgvDuplSerialNum.ScrollBars = ScrollBars.Both
        dgvDuplSerialNum.ShowCellErrors = False
        dgvDuplSerialNum.ShowCellToolTips = False
        dgvDuplSerialNum.ShowEditingIcon = False
        dgvDuplSerialNum.ShowRowErrors = False

        dgvDuplBoth.AllowUserToAddRows = False
        dgvDuplBoth.AllowUserToDeleteRows = False
        dgvDuplBoth.AllowUserToResizeRows = False
        dgvDuplBoth.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDuplBoth.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDuplBoth.BorderStyle = BorderStyle.Fixed3D
        dgvDuplBoth.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDuplBoth.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplBoth.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDuplBoth.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDuplBoth.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDuplBoth.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplBoth.DefaultCellStyle.WrapMode = False
        dgvDuplBoth.EnableHeadersVisualStyles = False
        dgvDuplBoth.ReadOnly = True
        dgvDuplBoth.RowHeadersVisible = False
        dgvDuplBoth.ScrollBars = ScrollBars.Both
        dgvDuplBoth.ShowCellErrors = False
        dgvDuplBoth.ShowCellToolTips = False
        dgvDuplBoth.ShowEditingIcon = False
        dgvDuplBoth.ShowRowErrors = False

        dgvDuplBlank.AllowUserToAddRows = False
        dgvDuplBlank.AllowUserToDeleteRows = False
        dgvDuplBlank.AllowUserToResizeRows = False
        dgvDuplBlank.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDuplBlank.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDuplBlank.BorderStyle = BorderStyle.Fixed3D
        dgvDuplBlank.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDuplBlank.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplBlank.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDuplBlank.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDuplBlank.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDuplBlank.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDuplBlank.DefaultCellStyle.WrapMode = False
        dgvDuplBlank.EnableHeadersVisualStyles = False
        dgvDuplBlank.ReadOnly = True
        dgvDuplBlank.RowHeadersVisible = False
        dgvDuplBlank.ScrollBars = ScrollBars.Both
        dgvDuplBlank.ShowCellErrors = False
        dgvDuplBlank.ShowCellToolTips = False
        dgvDuplBlank.ShowEditingIcon = False
        dgvDuplBlank.ShowRowErrors = False

        ' Force all tabs to draw (otherwise data grid views won't format during population)
        Delegate_Sub_Set_Active_Tab(tabCtrlDuplComp, tabDuplBlank)
        Delegate_Sub_Set_Active_Tab(tabCtrlDuplComp, tabDuplBoth)
        Delegate_Sub_Set_Active_Tab(tabCtrlDuplComp, tabDuplSerialNum)
        Delegate_Sub_Set_Active_Tab(tabCtrlDuplComp, tabDuplHostname)

    End Sub

    Private Sub DuplCompGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim RecordCount As Integer = 0
        Dim CallStack As String = "DuplCompGridWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)
            tabCtrlAgentGrid.Invoke(Sub() tabCtrlDuplComp.Height = tabCtrlDuplComp.Height - prgDuplCompGrid.Height - 4)

            ' Query duplicate by hostname
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select dh.label as 'Hostname', dh.serial_number as 'Serial', dh.primary_mac_address as 'Primary MAC', dh.asset_tag as 'Asset Tag', dh.system_id as 'System ID', dh.host_uuid as 'HostUUID', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, ca.last_run_date, '19700101')) as 'Last Run Date', ac.agent_component_version as 'Agent Version', nt.label as 'Source Domain' from ca_discovered_hardware dh with(nolock) left outer join ca_agent ca with(nolock) on dh.dis_hw_uuid=ca.object_uuid left outer join ca_agent_component ac with(nolock) on ca.object_uuid=ac.object_uuid and ac.agent_comp_id=5 left outer join ca_n_tier nt with(nolock) on dh.domain_uuid=nt.domain_uuid where dh.host_name in (select distinct(host_name) from ca_discovered_hardware with(nolock) group by host_name having count(*)>1) order by dh.label, ca.last_run_date",
                                  dgvDuplHostname,
                                  "Hostname",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgDuplCompGrid,
                                  tabDuplHostname)

            ' Query duplicate by serial number
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select dh.label as 'Hostname', dh.serial_number as 'Serial', dh.primary_mac_address as 'Primary MAC', dh.asset_tag as 'Asset Tag', dh.system_id as 'System ID', dh.host_uuid as 'HostUUID', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, ca.last_run_date, '19700101')) as 'Last Run Date', ac.agent_component_version as 'Agent Version', nt.label as 'Source Domain' from ca_discovered_hardware dh with(nolock) left outer join ca_agent ca with(nolock) on dh.dis_hw_uuid=ca.object_uuid left outer join ca_n_tier nt with(nolock) on dh.domain_uuid=nt.domain_uuid left outer join ca_agent_component ac with(nolock) on ca.object_uuid=ac.object_uuid and agent_comp_id=5 where dh.host_name not like 'VMWare ESX - %' and dh.serial_number in (select distinct(serial_number) from ca_discovered_hardware with(nolock) group by serial_number having count(*)>1) order by dh.serial_number, ca.last_run_date",
                                  dgvDuplSerialNum,
                                  "Hostname",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgDuplCompGrid,
                                  tabDuplSerialNum)

            ' Query duplicate by both
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select dh.label as 'Hostname', dh.serial_number as 'Serial', dh.primary_mac_address as 'Primary MAC', dh.asset_tag as 'Asset Tag', dh.system_id as 'System ID', dh.host_uuid as 'HostUUID', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, ca.last_run_date, '19700101')) as 'Last Run Date', ac.agent_component_version as 'Agent Version', nt.label as 'Source Domain' from ca_discovered_hardware dh with(nolock) inner join (select host_name, serial_number from ca_discovered_hardware with(nolock) group by host_name, serial_number having count(*) > 1) as Duplicates on dh.host_name=Duplicates.host_name and dh.serial_number=Duplicates.serial_number left outer join ca_agent ca with(nolock) on dh.dis_hw_uuid=ca.object_uuid left outer join ca_agent_component ac with(nolock) on ca.object_uuid=ac.object_uuid and ac.agent_comp_id=5 left outer join ca_n_tier nt with(nolock) on dh.domain_uuid=nt.domain_uuid order by dh.serial_number, dh.host_name, ca.last_run_date",
                                  dgvDuplBoth,
                                  "Hostname",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgDuplCompGrid,
                                  tabDuplBoth)

            ' Query blank serial
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select dh.label as 'Hostname', dh.serial_number as 'Serial', dh.primary_mac_address as 'Primary MAC', dh.asset_tag as 'Asset Tag', dh.system_id as 'System ID', dh.host_uuid as 'HostUUID', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, ca.last_run_date, '19700101')) as 'Last Run Date', ac.agent_component_version as 'Agent Version', nt.label as 'Source Domain' from ca_discovered_hardware dh with(nolock) left outer join ca_agent ca with(nolock) on dh.dis_hw_uuid=ca.object_uuid left outer join ca_n_tier nt with(nolock) on dh.domain_uuid=nt.domain_uuid left outer join ca_agent_component ac with(nolock) on ca.object_uuid=ac.object_uuid and agent_comp_id=5 where dh.serial_number is null or dh.serial_number='' and dh.host_name not like 'VMWare ESX - %' order by dh.serial_number, ca.last_run_date",
                                  dgvDuplBlank,
                                  "Hostname",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgDuplCompGrid,
                                  tabDuplBlank)

            tabCtrlDuplComp.Invoke(Sub() tabCtrlDuplComp.Height = pnlSqlDuplCompGrid.Height - pnlSqlDuplCompGridButtons.Height - 3)

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If

            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshDuplCompGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportDuplCompGrid, True)

        End Try

    End Sub

    Private Sub btnSqlConnectDuplCompGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectDuplCompGrid.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectDuplCompGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectDuplCompGrid.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshDuplCompGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshDuplCompGrid.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshDuplCompGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportDuplCompGrid, False)
        If tabDuplHostname.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplHostname, tabDuplHostname.Text.Substring(0, tabDuplHostname.Text.IndexOf("(") - 1))
        If tabDuplSerialNum.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplSerialNum, tabDuplSerialNum.Text.Substring(0, tabDuplSerialNum.Text.IndexOf("(") - 1))
        If tabDuplBoth.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplBoth, tabDuplBoth.Text.Substring(0, tabDuplBoth.Text.IndexOf("(") - 1))
        If tabDuplBlank.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplBlank, tabDuplBlank.Text.Substring(0, tabDuplBlank.Text.IndexOf("(") - 1))
        DuplCompGridThread = New Thread(Sub() DuplCompGridWorker(ConnectionString))
        DuplCompGridThread.Start()
    End Sub

    Private Sub btnSqlExportDuplCompGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportDuplCompGrid.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            If tabCtrlDuplComp.SelectedTab.Equals(tabDuplHostname) Then
                For Each dgvColumn As DataGridViewColumn In dgvDuplHostname.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvDuplHostname.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlDuplComp.SelectedTab.Equals(tabDuplSerialNum) Then
                For Each dgvColumn As DataGridViewColumn In dgvDuplSerialNum.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvDuplSerialNum.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlDuplComp.SelectedTab.Equals(tabDuplBoth) Then
                For Each dgvColumn As DataGridViewColumn In dgvDuplBoth.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)
                For Each dgvRecord As DataGridViewRow In dgvDuplBoth.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next

            ElseIf tabCtrlDuplComp.SelectedTab.Equals(tabDuplBlank) Then
                For Each dgvColumn As DataGridViewColumn In dgvDuplBlank.Columns
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)

                For Each dgvRecord As DataGridViewRow In dgvDuplBlank.Rows
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")
                    Next
                    StateStreamWriter.Write(Environment.NewLine)
                Next
            End If

            StateStreamWriter.Close()
        Catch ex As Exception
            AlertBox.CreateUserAlert("Export failed." + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message)
        End Try

    End Sub

    Private Sub btnSqlExitDuplCompGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitDuplCompGrid.Click
        Close()
    End Sub

End Class
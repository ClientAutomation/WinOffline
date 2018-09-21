Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms
Imports System.Windows.Forms.ListViewItem

Partial Public Class WinOfflineUI

    Private Shared MdbOverviewThread As Thread

    Private Sub InitSqlMdbOverview()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectMdbOverview, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshMdbOverview, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportMdbOverview, False)

        dgvAgentVersion.AllowUserToAddRows = False
        dgvAgentVersion.AllowUserToDeleteRows = False
        dgvAgentVersion.AllowUserToResizeRows = False
        dgvAgentVersion.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvAgentVersion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvAgentVersion.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvAgentVersion.BorderStyle = BorderStyle.Fixed3D
        dgvAgentVersion.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvAgentVersion.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 11)
        dgvAgentVersion.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvAgentVersion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAgentVersion.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvAgentVersion.DefaultCellStyle.Font = New Drawing.Font("Calibri", 11)
        dgvAgentVersion.DefaultCellStyle.WrapMode = False
        dgvAgentVersion.EnableHeadersVisualStyles = False
        dgvAgentVersion.ReadOnly = True
        dgvAgentVersion.RowHeadersVisible = False
        dgvAgentVersion.ScrollBars = ScrollBars.Both
        dgvAgentVersion.ShowCellErrors = False
        dgvAgentVersion.ShowCellToolTips = False
        dgvAgentVersion.ShowEditingIcon = False
        dgvAgentVersion.ShowRowErrors = False

        dgvContentSummary.AllowUserToAddRows = False
        dgvContentSummary.AllowUserToDeleteRows = False
        dgvContentSummary.AllowUserToResizeRows = False
        dgvContentSummary.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvContentSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvContentSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvContentSummary.BorderStyle = BorderStyle.Fixed3D
        dgvContentSummary.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvContentSummary.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 11)
        dgvContentSummary.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvContentSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvContentSummary.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvContentSummary.DefaultCellStyle.Font = New Drawing.Font("Calibri", 11)
        dgvContentSummary.DefaultCellStyle.WrapMode = False
        dgvContentSummary.EnableHeadersVisualStyles = False
        dgvContentSummary.ReadOnly = True
        dgvContentSummary.RowHeadersVisible = False
        dgvContentSummary.ScrollBars = ScrollBars.Both
        dgvContentSummary.ShowCellErrors = False
        dgvContentSummary.ShowCellToolTips = False
        dgvContentSummary.ShowEditingIcon = False
        dgvContentSummary.ShowRowErrors = False

    End Sub

    Private Sub MdbOverviewWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "MdbOverviewWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            ' Populate mdb installation overview
            Delegate_Sub_Set_Text(txtMdbVersion, SqlSelectScalar(CallStack, DbConnection, "select set_val_text from ca_settings with (nolock) where set_id=903"))
            Delegate_Sub_Set_Text(txtMdbInstallDate, SqlSelectScalar(CallStack, DbConnection, "select installdate from mdb_version with (nolock)"))

            ' Query mdb type
            DomainType = SqlSelectScalar(CallStack, DbConnection, "select domain_type from ca_n_tier where domain_uuid=" + DomainUuid)

            Select Case DomainType
                Case 0 : Delegate_Sub_Set_Text(txtMdbType, "Domain")
                Case 1 : Delegate_Sub_Set_Text(txtMdbType, "Enterprise")
                Case 2 : Delegate_Sub_Set_Text(txtMdbType, "Bridge Target")
                Case Else : Delegate_Sub_Set_Text(txtMdbType, "Unknown")
            End Select

            If Globals.ITCMInstalled Then
                Delegate_Sub_Set_Text(txtITCMVersion, Globals.ITCMVersion)
            Else
                Delegate_Sub_Set_Text(txtITCMVersion, "Not installed locally")
            End If

            Delegate_Sub_Begin_Update_ListView(lvwITCMSummary)
            Delegate_Sub_Clear_ListView(lvwITCMSummary)

            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Domain Managers", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_n_tier with (nolock) where domain_type=0")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Scalability Servers", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_server with (nolock)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Registered Agents", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=1")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Registered Users", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=2")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Registered Profiles", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=4")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Obsolete Agents [>=90 days]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=1 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-7776000")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Obsolete Agents [>=1 year]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=1 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-31536000")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Obsolete Users [>=90 days]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=2 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-7776000")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Obsolete Users [>=1 year]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=2 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-31536000")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Obsolete Profiles [>=90 days]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=4 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-7776000")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Obsolete Profiles [>=1 year]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=4 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-31536000")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Group Definitions [Static]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_group_def with (nolock) where member_type=1 and query_uuid is null")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Group Definitions [Dynamic]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_group_def with (nolock) where member_type=1 and query_uuid is not null")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Query Definitions [Simple]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_query_def with (nolock)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Query Definitions [Complex]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_query_def_contents with (nolock)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Software Packages", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_rsw with (nolock) where itemtype<>5")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Job Containers [In-Progress]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_job_cont with (nolock) where type=3 and comment<>'CA-Software Delivery Reserved Group' and objectid in (select jcont from usd_link_jc_act with (nolock) where activity in (select objectid from usd_activity with (nolock) where state not in (2,3)))")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Job Containers [Completed]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_job_cont with (nolock) where type=3 and comment<>'CA-Software Delivery Reserved Group' and objectid in (select jcont from usd_link_jc_act with (nolock) where activity in (select objectid from usd_activity with (nolock) where state in (2,3)))")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Software Jobs [In-Progress]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_applic with (nolock) where status in (7,8,27)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Software Jobs [Completed]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_applic with (nolock) where status=9")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Staging Jobs [In-Progress]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_applic with (nolock) where status in (2,3,17,18,19,20,21)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Staging Jobs [Completed]", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_applic with (nolock) where status=4")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Software Based Policies", SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_job_cont with (nolock) where type=4 and comment<>'CA-Software Delivery Reserved Group'")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Query Based Policies", SqlSelectScalar(CallStack, DbConnection, "select count(*) from polidef with (nolock)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Engine Instances", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ca_engine with (nolock)")}))
            Delegate_Sub_Append_ListView(lvwITCMSummary, New ArrayList({"Engine Tasks", SqlSelectScalar(CallStack, DbConnection, "select count(*) from ncjobcfg with (nolock) where jotype<>2 and domainid=" + DomainId)}))

            Delegate_Sub_End_Update_ListView(lvwITCMSummary)

            If DomainType.Equals("0") Then
                SqlPopulateGridWorker(CallStack,
                                      DbConnection,
                                      "select distinct ac.agent_component_version 'Agent Version', count(*) as 'Count' from ca_discovered_hardware dh with (nolock) inner join ca_agent_component ac with (nolock) on dh.dis_hw_uuid=ac.object_uuid and ac.agent_comp_id=5 inner join ca_n_tier nt with (nolock) on nt.domain_uuid=dh.domain_uuid inner join ca_manager mgr with (nolock) on nt.domain_uuid=mgr.domain_uuid group by mgr.label, ac.agent_component_version order by ac.agent_component_version",
                                      dgvAgentVersion,
                                      "Agent Version")
            Else
                SqlPopulateGridWorker(CallStack,
                                      DbConnection,
                                      "select distinct mgr.label as 'Manager', ac.agent_component_version 'Agent Version', count(*) as 'Count' from ca_discovered_hardware dh with (nolock) inner join ca_agent_component ac with (nolock) on dh.dis_hw_uuid=ac.object_uuid and ac.agent_comp_id=5 inner join ca_n_tier nt with (nolock) on nt.domain_uuid=dh.domain_uuid inner join ca_manager mgr with (nolock) on nt.domain_uuid=mgr.domain_uuid group by mgr.label, ac.agent_component_version order by ac.agent_component_version, mgr.label",
                                      dgvAgentVersion,
                                      "Manager")
            End If

            SqlPopulateGridWorker(CallStack, DbConnection, "select t0.source_type_name as 'Source Type', t0.def_count as 'Count' from (select type.source_type_id, type.source_type_name, count(*) as def_count from ca_software_def def with (nolock) inner join ca_source_type type with (nolock) on def.source_type_id=type.source_type_id group by type.source_type_id, type.source_type_name) t0 order by t0.source_type_id", dgvContentSummary, "Source Type")
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshMdbOverview, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportMdbOverview, True)
        End Try

    End Sub

    Private Sub btnSqlConnectMdbOverview_Click(sender As Object, e As EventArgs) Handles btnSqlConnectMdbOverview.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectMdbOverview_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectMdbOverview.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshMdbOverview_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshMdbOverview.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshMdbOverview, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportMdbOverview, False)
        MdbOverviewThread = New Thread(Sub() MdbOverviewWorker(ConnectionString))
        MdbOverviewThread.Start()
    End Sub

    Private Sub btnSqlExportMdbOverview_Click(sender As Object, e As EventArgs) Handles btnSqlExportMdbOverview.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)
            StateStreamWriter.Write("MDB Version," + txtMdbVersion.Text + "," + Environment.NewLine)
            StateStreamWriter.Write("MDB Install Date," + txtMdbInstallDate.Text + "," + Environment.NewLine)
            StateStreamWriter.Write("MDB Type," + txtMdbType.Text + "," + Environment.NewLine)

            For Each lviRecord As ListViewItem In lvwITCMSummary.Items
                For Each SubRecord As ListViewSubItem In lviRecord.SubItems
                    StateStreamWriter.Write(SubRecord.Text + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)
            Next

            For Each dgvRecord As DataGridViewRow In dgvAgentVersion.Rows
                For Each CellItem As DataGridViewCell In dgvRecord.Cells
                    StateStreamWriter.Write(CellItem.Value.ToString + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)
            Next

            For Each dgvRecord As DataGridViewRow In dgvContentSummary.Rows
                For Each CellItem As DataGridViewCell In dgvRecord.Cells
                    StateStreamWriter.Write(CellItem.Value.ToString + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)
            Next

            StateStreamWriter.Close()
        Catch ex As Exception
            AlertBox.CreateUserAlert("Export failed." + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message)
        End Try

    End Sub

    Private Sub btnSqlExitMdbOverview_Click(sender As Object, e As EventArgs) Handles btnSqlExitMdbOverview.Click
        Close()
    End Sub

End Class

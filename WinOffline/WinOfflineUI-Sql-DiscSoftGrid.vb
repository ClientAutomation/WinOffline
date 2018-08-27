﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared DiscSoftGridThread As Thread

    Private Sub InitSqlDiscSoftGrid()

        ' Disable buttons
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectDiscSoftGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshDiscSoftGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportDiscSoftGrid, False)

        ' Set grid properties
        dgvDiscSignature.AllowUserToAddRows = False
        dgvDiscSignature.AllowUserToDeleteRows = False
        dgvDiscSignature.AllowUserToResizeRows = False
        dgvDiscSignature.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvDiscSignature.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDiscSignature.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDiscSignature.BorderStyle = BorderStyle.Fixed3D
        dgvDiscSignature.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDiscSignature.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscSignature.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDiscSignature.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscSignature.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDiscSignature.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscSignature.DefaultCellStyle.WrapMode = False
        dgvDiscSignature.EnableHeadersVisualStyles = False
        dgvDiscSignature.ReadOnly = True
        dgvDiscSignature.RowHeadersVisible = False
        dgvDiscSignature.ScrollBars = ScrollBars.Both
        dgvDiscSignature.ShowCellErrors = False
        dgvDiscSignature.ShowCellToolTips = False
        dgvDiscSignature.ShowEditingIcon = False
        dgvDiscSignature.ShowRowErrors = False

        ' Set grid properties
        dgvDiscCustom.AllowUserToAddRows = False
        dgvDiscCustom.AllowUserToDeleteRows = False
        dgvDiscCustom.AllowUserToResizeRows = False
        dgvDiscCustom.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvDiscCustom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDiscCustom.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDiscCustom.BorderStyle = BorderStyle.Fixed3D
        dgvDiscCustom.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDiscCustom.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscCustom.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDiscCustom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscCustom.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDiscCustom.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscCustom.DefaultCellStyle.WrapMode = False
        dgvDiscCustom.EnableHeadersVisualStyles = False
        dgvDiscCustom.ReadOnly = True
        dgvDiscCustom.RowHeadersVisible = False
        dgvDiscCustom.ScrollBars = ScrollBars.Both
        dgvDiscCustom.ShowCellErrors = False
        dgvDiscCustom.ShowCellToolTips = False
        dgvDiscCustom.ShowEditingIcon = False
        dgvDiscCustom.ShowRowErrors = False

        ' Set grid properties
        dgvDiscHeuristic.AllowUserToAddRows = False
        dgvDiscHeuristic.AllowUserToDeleteRows = False
        dgvDiscHeuristic.AllowUserToResizeRows = False
        dgvDiscHeuristic.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvDiscHeuristic.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDiscHeuristic.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDiscHeuristic.BorderStyle = BorderStyle.Fixed3D
        dgvDiscHeuristic.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDiscHeuristic.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscHeuristic.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDiscHeuristic.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscHeuristic.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDiscHeuristic.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscHeuristic.DefaultCellStyle.WrapMode = False
        dgvDiscHeuristic.EnableHeadersVisualStyles = False
        dgvDiscHeuristic.ReadOnly = True
        dgvDiscHeuristic.RowHeadersVisible = False
        dgvDiscHeuristic.ScrollBars = ScrollBars.Both
        dgvDiscHeuristic.ShowCellErrors = False
        dgvDiscHeuristic.ShowCellToolTips = False
        dgvDiscHeuristic.ShowEditingIcon = False
        dgvDiscHeuristic.ShowRowErrors = False

        ' Set grid properties
        dgvDiscIntellisig.AllowUserToAddRows = False
        dgvDiscIntellisig.AllowUserToDeleteRows = False
        dgvDiscIntellisig.AllowUserToResizeRows = False
        dgvDiscIntellisig.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvDiscIntellisig.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvDiscIntellisig.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvDiscIntellisig.BorderStyle = BorderStyle.Fixed3D
        dgvDiscIntellisig.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDiscIntellisig.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscIntellisig.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDiscIntellisig.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscIntellisig.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDiscIntellisig.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscIntellisig.DefaultCellStyle.WrapMode = False
        dgvDiscIntellisig.EnableHeadersVisualStyles = False
        dgvDiscIntellisig.ReadOnly = True
        dgvDiscIntellisig.RowHeadersVisible = False
        dgvDiscIntellisig.ScrollBars = ScrollBars.Both
        dgvDiscIntellisig.ShowCellErrors = False
        dgvDiscIntellisig.ShowCellToolTips = False
        dgvDiscIntellisig.ShowEditingIcon = False
        dgvDiscIntellisig.ShowRowErrors = False

        ' Set grid properties
        dgvDiscEverything.AllowUserToAddRows = False
        dgvDiscEverything.AllowUserToDeleteRows = False
        dgvDiscEverything.AllowUserToResizeRows = False
        dgvDiscEverything.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvDiscEverything.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        dgvDiscEverything.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvDiscEverything.BorderStyle = BorderStyle.Fixed3D
        dgvDiscEverything.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvDiscEverything.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscEverything.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvDiscEverything.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscEverything.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvDiscEverything.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvDiscEverything.DefaultCellStyle.WrapMode = False
        dgvDiscEverything.EnableHeadersVisualStyles = False
        dgvDiscEverything.ReadOnly = True
        dgvDiscEverything.RowHeadersVisible = False
        dgvDiscEverything.ScrollBars = ScrollBars.Both
        dgvDiscEverything.ShowCellErrors = False
        dgvDiscEverything.ShowCellToolTips = False
        dgvDiscEverything.ShowEditingIcon = False
        dgvDiscEverything.ShowRowErrors = False

        ' Force all tabs to draw (otherwise data grid views won't format during population)
        Delegate_Sub_Set_Active_Tab(tabCtrlDiscSoftGrid, tabDiscEverything)
        Delegate_Sub_Set_Active_Tab(tabCtrlDiscSoftGrid, tabDiscIntellisig)
        Delegate_Sub_Set_Active_Tab(tabCtrlDiscSoftGrid, tabDiscHeuristic)
        Delegate_Sub_Set_Active_Tab(tabCtrlDiscSoftGrid, tabDiscCustom)
        Delegate_Sub_Set_Active_Tab(tabCtrlDiscSoftGrid, tabDiscSignature)

    End Sub

    Private Sub DiscSoftGridWorker(ByVal ConnectionString As String)

        ' Local variables
        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim RecordCount As Integer = 0
        Dim CallStack As String = "DiscSoftGridWorker --> "

        ' Encapsulate grid worker
        Try

            ' Open sql connection
            DbConnection.Open()

            ' Write debug
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            ' Reveal progress bar
            tabCtrlDiscSoftGrid.Invoke(Sub() tabCtrlDiscSoftGrid.Height = tabCtrlDiscSoftGrid.Height - prgDiscSoftGrid.Height - 4)

            ' Query signature report
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select distinct def.name as 'Software Name', def.sw_version_label as 'Software Version', count(*) as 'Detected Count' from ca_discovered_software ds inner join ca_software_def def on ds.sw_def_uuid=def.sw_def_uuid and def.source_type_id in (1,2) group by def.name, def.sw_version_label order by def.name, def.sw_version_label",
                                          dgvDiscSignature,
                                          "Software Name",
                                          DataGridViewAutoSizeColumnsMode.DisplayedCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgDiscSoftGrid,
                                          tabDiscSignature)

            ' Query custom signature report
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select distinct def.name as 'Software Name', def.sw_version_label as 'Software Version', count(*) as 'Detected Count' from ca_discovered_software ds inner join ca_software_def def on ds.sw_def_uuid=def.sw_def_uuid and def.source_type_id=2 group by def.name, def.sw_version_label order by def.name, def.sw_version_label",
                                          dgvDiscCustom,
                                          "Software Name",
                                          DataGridViewAutoSizeColumnsMode.DisplayedCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgDiscSoftGrid, tabDiscCustom)

            ' Query heuristic report
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select distinct def.name as 'Software Name', def.sw_version_label as 'Software Version', count(*) as 'Detected Count' from ca_discovered_software ds inner join ca_software_def def on ds.sw_def_uuid=def.sw_def_uuid and def.source_type_id=3 group by def.name, def.sw_version_label order by def.name, def.sw_version_label",
                                          dgvDiscHeuristic,
                                          "Software Name",
                                          DataGridViewAutoSizeColumnsMode.DisplayedCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgDiscSoftGrid,
                                          tabDiscHeuristic)

            ' Query intellisig report
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select distinct def.name as 'Software Name', def.sw_version_label as 'Software Version', count(*) as 'Detected Count' from ca_discovered_software ds inner join ca_software_def def on ds.sw_def_uuid=def.sw_def_uuid and def.source_type_id in (5,6) group by def.name, def.sw_version_label order by def.name, def.sw_version_label",
                                          dgvDiscIntellisig,
                                          "Software Name",
                                          DataGridViewAutoSizeColumnsMode.DisplayedCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgDiscSoftGrid,
                                          tabDiscIntellisig)

            ' Query everything report
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select distinct def.name as 'Software Name', def.sw_version_label as 'Software Version', count(*) as 'Detected Count' from ca_discovered_software ds inner join ca_software_def def on ds.sw_def_uuid=def.sw_def_uuid group by def.name, def.sw_version_label order by def.name, def.sw_version_label",
                                          dgvDiscEverything,
                                          "Software Name",
                                          DataGridViewAutoSizeColumnsMode.DisplayedCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgDiscSoftGrid,
                                          tabDiscEverything)

            ' Hide progress bar
            tabCtrlDiscSoftGrid.Invoke(Sub() tabCtrlDiscSoftGrid.Height = pnlSqlDiscSoftGrid.Height - pnlSqlDiscSoftGridButtons.Height - grpDiscSoftGrid.Height - 3)

        Catch ex As Exception

            ' Write debug
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)

        Finally

            ' Check if database connection is open
            If Not DbConnection.State = ConnectionState.Closed Then

                ' Close the database connection
                DbConnection.Close()

                ' Write debug
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")

            End If

            ' Enable buttons
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshDiscSoftGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportDiscSoftGrid, True)

        End Try

    End Sub

    Private Sub btnSqlConnectDiscSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectDiscSoftGrid.Click

        ' Perform SQL connection
        SqlConnect()

    End Sub

    Private Sub btnSqlDisconnectDiscSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectDiscSoftGrid.Click

        ' Perform disconnect method
        SqlDisconnect()

    End Sub

    Private Sub btnSqlRefreshDiscSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshDiscSoftGrid.Click

        ' Disable buttons
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshDiscSoftGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportDiscSoftGrid, False)

        ' Reset tab text
        If tabDiscSignature.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscSignature, tabDiscSignature.Text.Substring(0, tabDiscSignature.Text.IndexOf("(") - 1))
        If tabDiscCustom.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscCustom, tabDiscCustom.Text.Substring(0, tabDiscCustom.Text.IndexOf("(") - 1))
        If tabDiscHeuristic.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscHeuristic, tabDiscHeuristic.Text.Substring(0, tabDiscHeuristic.Text.IndexOf("(") - 1))
        If tabDiscIntellisig.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscIntellisig, tabDiscIntellisig.Text.Substring(0, tabDiscIntellisig.Text.IndexOf("(") - 1))
        If tabDiscEverything.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscEverything, tabDiscEverything.Text.Substring(0, tabDiscEverything.Text.IndexOf("(") - 1))

        ' Restart thread
        DiscSoftGridThread = New Thread(Sub() DiscSoftGridWorker(ConnectionString))
        DiscSoftGridThread.Start()

    End Sub

    Private Sub btnSqlExportDiscSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportDiscSoftGrid.Click

        ' Local variables
        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        ' Set dialog properties
        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        ' Launch dialog and check result
        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        ' Encapsulate file operation
        Try

            ' Open output stream
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            ' Check selected tab
            If tabCtrlDiscSoftGrid.SelectedTab.Equals(tabDiscSignature) Then

                ' Iterate datagrid column headers
                For Each dgvColumn As DataGridViewColumn In dgvDiscSignature.Columns

                    ' Write values
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")

                Next

                ' Write newline
                StateStreamWriter.Write(Environment.NewLine)

                ' Iterate datagrid rows
                For Each dgvRecord As DataGridViewRow In dgvDiscSignature.Rows

                    ' Iterate cells
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells

                        ' Write values
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")

                    Next

                    ' Write newline
                    StateStreamWriter.Write(Environment.NewLine)

                Next

            ElseIf tabCtrlDiscSoftGrid.SelectedTab.Equals(tabDiscCustom) Then

                ' Iterate datagrid column headers
                For Each dgvColumn As DataGridViewColumn In dgvDiscCustom.Columns

                    ' Write values
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")

                Next

                ' Write newline
                StateStreamWriter.Write(Environment.NewLine)

                ' Iterate datagrid rows
                For Each dgvRecord As DataGridViewRow In dgvDiscCustom.Rows

                    ' Iterate cells
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells

                        ' Write values
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")

                    Next

                    ' Write newline
                    StateStreamWriter.Write(Environment.NewLine)

                Next

            ElseIf tabCtrlDiscSoftGrid.SelectedTab.Equals(tabDiscHeuristic) Then

                ' Iterate datagrid column headers
                For Each dgvColumn As DataGridViewColumn In dgvDiscHeuristic.Columns

                    ' Write values
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")

                Next

                ' Write newline
                StateStreamWriter.Write(Environment.NewLine)

                ' Iterate datagrid rows
                For Each dgvRecord As DataGridViewRow In dgvDiscHeuristic.Rows

                    ' Iterate cells
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells

                        ' Write values
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")

                    Next

                    ' Write newline
                    StateStreamWriter.Write(Environment.NewLine)

                Next

            ElseIf tabCtrlDiscSoftGrid.SelectedTab.Equals(tabDiscIntellisig) Then

                ' Iterate datagrid column headers
                For Each dgvColumn As DataGridViewColumn In dgvDiscIntellisig.Columns

                    ' Write values
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")

                Next

                ' Write newline
                StateStreamWriter.Write(Environment.NewLine)

                ' Iterate datagrid rows
                For Each dgvRecord As DataGridViewRow In dgvDiscIntellisig.Rows

                    ' Iterate cells
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells

                        ' Write values
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")

                    Next

                    ' Write newline
                    StateStreamWriter.Write(Environment.NewLine)

                Next

            ElseIf tabCtrlDiscSoftGrid.SelectedTab.Equals(tabDiscEverything) Then

                ' Iterate datagrid column headers
                For Each dgvColumn As DataGridViewColumn In dgvDiscEverything.Columns

                    ' Write values
                    StateStreamWriter.Write(dgvColumn.HeaderText + ",")

                Next

                ' Write newline
                StateStreamWriter.Write(Environment.NewLine)

                ' Iterate datagrid rows
                For Each dgvRecord As DataGridViewRow In dgvDiscEverything.Rows

                    ' Iterate cells
                    For Each CellItem As DataGridViewCell In dgvRecord.Cells

                        ' Write values
                        StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")

                    Next

                    ' Write newline
                    StateStreamWriter.Write(Environment.NewLine)

                Next

            End If

            ' Close output stream
            StateStreamWriter.Close()

        Catch ex As Exception

            ' Push user alert
            AlertBox.CreateUserAlert("Export failed." + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message)

        End Try

    End Sub

    Private Sub btnSqlExitDiscSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitDiscSoftGrid.Click

        ' Close the dialog
        Close()

    End Sub

End Class
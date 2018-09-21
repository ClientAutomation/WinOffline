Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared TableSpaceGridThread As Thread

    Private Sub InitSqlTableSpaceGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectTableSpace, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshTableSpace, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportTableSpace, False)

        dgvTableSpaceGrid.AllowUserToAddRows = False
        dgvTableSpaceGrid.AllowUserToDeleteRows = False
        dgvTableSpaceGrid.AllowUserToResizeRows = False
        dgvTableSpaceGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvTableSpaceGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvTableSpaceGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvTableSpaceGrid.BorderStyle = BorderStyle.Fixed3D
        dgvTableSpaceGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvTableSpaceGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 10)
        dgvTableSpaceGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvTableSpaceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvTableSpaceGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvTableSpaceGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 10)
        dgvTableSpaceGrid.DefaultCellStyle.WrapMode = False
        dgvTableSpaceGrid.EnableHeadersVisualStyles = False
        dgvTableSpaceGrid.ReadOnly = True
        dgvTableSpaceGrid.RowHeadersVisible = False
        dgvTableSpaceGrid.ScrollBars = ScrollBars.Both
        dgvTableSpaceGrid.ShowCellErrors = False
        dgvTableSpaceGrid.ShowCellToolTips = False
        dgvTableSpaceGrid.ShowEditingIcon = False
        dgvTableSpaceGrid.ShowRowErrors = False

    End Sub

    Private Sub TableSpaceGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "TableSpaceGridWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            grpSqlTableSpace.Invoke(Sub() grpSqlTableSpace.Height = grpSqlTableSpace.Height - prgTableSpaceGrid.Height - 4)

            ' Query table space
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "SELECT t.NAME as 'Table Name', p.rows as 'Row Count', SUM(a.total_pages) * 8 as 'Total Space (KB)', SUM(a.used_pages) * 8 as 'Used (KB)', (SUM(a.total_pages) - SUM(a.used_pages)) * 8 as 'Unused (KB)' FROM sys.tables t INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id WHERE t.NAME NOT LIKE 'dt%' AND t.is_ms_shipped = 0 AND i.OBJECT_ID > 255 GROUP BY t.Name, p.Rows ORDER BY [Total Space (KB)] desc",
                                  dgvTableSpaceGrid,
                                  "Table Name",
                                  DataGridViewAutoSizeColumnsMode.AllCells,
                                  DataGridViewAutoSizeRowsMode.AllCells,
                                  prgTableSpaceGrid,
                                  grpSqlTableSpace)

            grpSqlTableSpace.Invoke(Sub() grpSqlTableSpace.Height = pnlSqlTableSpaceGrid.Height - pnlSqlTableSpaceGridButtons.Height - 3)

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshTableSpace, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportTableSpace, True)
        End Try

    End Sub

    Private Sub btnSqlConnectTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectTableSpace.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectTableSpace.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshTableSpace.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshTableSpace, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportTableSpace, False)
        If grpSqlTableSpace.Text.Contains("(") Then Delegate_Sub_Set_Text(grpSqlTableSpace, grpSqlTableSpace.Text.Substring(0, grpSqlTableSpace.Text.IndexOf("(") - 1))
        TableSpaceGridThread = New Thread(Sub() TableSpaceGridWorker(ConnectionString))
        TableSpaceGridThread.Start()
    End Sub

    Private Sub btnSqlExportTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportTableSpace.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            For Each dgvColumn As DataGridViewColumn In dgvTableSpaceGrid.Columns
                StateStreamWriter.Write(dgvColumn.HeaderText + ",")
            Next
            StateStreamWriter.Write(Environment.NewLine)

            For Each dgvRecord As DataGridViewRow In dgvTableSpaceGrid.Rows
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

    Private Sub btnSqlExitTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitTableSpace.Click
        Close()
    End Sub

End Class
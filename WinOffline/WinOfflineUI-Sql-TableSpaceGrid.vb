﻿'****************************** Class Header *******************************\
' Project Name: WinOffline
' Class Name:   WinOfflineUI
' File Name:    WinOfflineUI-Sql-TableSpaceGrid.vb
' Author:       Brian Fontana
'***************************************************************************/

Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared TableSpaceGridThread As Thread

    Private Sub InitSqlTableSpaceGrid()

        ' Disable buttons
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectTableSpace, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshTableSpace, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportTableSpace, False)

        ' Set table space grid properties
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

        ' Local variables
        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "TableSpaceGridWorker --> "

        ' Encapsulate grid worker
        Try

            ' Open sql connection
            DbConnection.Open()

            ' Write debug
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            ' Reveal progress bar
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

            ' Hide progress bar
            grpSqlTableSpace.Invoke(Sub() grpSqlTableSpace.Height = pnlSqlTableSpaceGrid.Height - pnlSqlTableSpaceGridButtons.Height - 3)

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
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshTableSpace, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportTableSpace, True)

        End Try

    End Sub

    Private Sub btnSqlConnectTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectTableSpace.Click

        ' Perform SQL connection
        SqlConnect()

    End Sub

    Private Sub btnSqlDisconnectTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectTableSpace.Click

        ' Perform disconnect method
        SqlDisconnect()

    End Sub

    Private Sub btnSqlRefreshTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshTableSpace.Click

        ' Disable buttons
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshTableSpace, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportTableSpace, False)

        ' Reset text
        If grpSqlTableSpace.Text.Contains("(") Then Delegate_Sub_Set_Text(grpSqlTableSpace, grpSqlTableSpace.Text.Substring(0, grpSqlTableSpace.Text.IndexOf("(") - 1))

        ' Restart thread
        TableSpaceGridThread = New Thread(Sub() TableSpaceGridWorker(ConnectionString))
        TableSpaceGridThread.Start()

    End Sub

    Private Sub btnSqlExportTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportTableSpace.Click

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

            ' Iterate datagrid column headers
            For Each dgvColumn As DataGridViewColumn In dgvTableSpaceGrid.Columns

                ' Write values
                StateStreamWriter.Write(dgvColumn.HeaderText + ",")

            Next

            ' Write newline
            StateStreamWriter.Write(Environment.NewLine)

            ' Iterate datagrid rows
            For Each dgvRecord As DataGridViewRow In dgvTableSpaceGrid.Rows

                ' Iterate cells
                For Each CellItem As DataGridViewCell In dgvRecord.Cells

                    ' Write values
                    StateStreamWriter.Write(CellItem.Value.ToString + ",")

                Next

                ' Write newline
                StateStreamWriter.Write(Environment.NewLine)

            Next

            ' Close output stream
            StateStreamWriter.Close()

        Catch ex As Exception

            ' Push user alert
            AlertBox.CreateUserAlert("Export failed." + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message)

        End Try

    End Sub

    Private Sub btnSqlExitTableSpaceGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitTableSpace.Click

        ' Close the dialog
        Close()

    End Sub

End Class
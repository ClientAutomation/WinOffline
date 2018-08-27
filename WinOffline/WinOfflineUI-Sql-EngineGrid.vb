﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared EngineGridThread As Thread

    Private Sub InitSqlEngineGrid()

        ' Disable buttons
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectEngineGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshEngineGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportEngineGrid, False)

        ' Set engine grid properties
        dgvEngineGrid.AllowUserToAddRows = False
        dgvEngineGrid.AllowUserToDeleteRows = False
        dgvEngineGrid.AllowUserToResizeRows = False
        dgvEngineGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvEngineGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvEngineGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvEngineGrid.BorderStyle = BorderStyle.Fixed3D
        dgvEngineGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvEngineGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvEngineGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvEngineGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvEngineGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvEngineGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvEngineGrid.DefaultCellStyle.WrapMode = False
        dgvEngineGrid.EnableHeadersVisualStyles = False
        dgvEngineGrid.ReadOnly = True
        dgvEngineGrid.RowHeadersVisible = False
        dgvEngineGrid.ScrollBars = ScrollBars.Both
        dgvEngineGrid.ShowCellErrors = False
        dgvEngineGrid.ShowCellToolTips = False
        dgvEngineGrid.ShowEditingIcon = False
        dgvEngineGrid.ShowRowErrors = False

    End Sub

    Private Sub EngineGridWorker(ByVal ConnectionString As String)

        ' Local variables
        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "EngineGridWorker --> "

        ' Encapsulate grid worker
        Try

            ' Open sql connection
            DbConnection.Open()

            ' Write debug
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            ' Reveal progress bar
            grpSqlEngineGrid.Invoke(Sub() grpSqlEngineGrid.Height = grpSqlEngineGrid.Height - prgTableSpaceGrid.Height - 4)

            ' Query engine grid
            SqlPopulateGridWorker(CallStack,
                                          DbConnection,
                                          "select eng.label as 'Engine', jobs.joname as 'Task', case when stat.status = -1 then 'ERROR' when stat.status = 0 then 'WAITING' when stat.status = 1 then 'OK' end as ""Status"", dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, stat.stdate, '19700101')) as 'Last Executed', stat.sttext as 'Status Comment' from ncjobcfg jobs with (nolock) inner join statjob stat with (nolock) on stat.jobid=jobs.jobid and stat.jdomid=jobs.domainid inner join linkjob link with (nolock) on link.jobid=stat.jobid and link.jdomid=stat.jdomid and link.object_uuid=stat.object_uuid inner join ca_engine eng with (nolock) on eng.engine_uuid=link.object_uuid where jobs.job_category=2 order by eng.label, jobs.joname",
                                          dgvEngineGrid,
                                          "Engine",
                                          DataGridViewAutoSizeColumnsMode.AllCells,
                                          DataGridViewAutoSizeRowsMode.AllCells,
                                          prgEngineGrid,
                                          grpSqlEngineGrid)

            ' Hide progress bar
            grpSqlEngineGrid.Invoke(Sub() grpSqlEngineGrid.Height = pnlSqlEngineGrid.Height - pnlSqlEngineGridButtons.Height - 3)

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
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshEngineGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportEngineGrid, True)

        End Try

    End Sub

    Private Sub btnSqlConnectEngineGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectEngineGrid.Click

        ' Perform SQL connection
        SqlConnect()

    End Sub

    Private Sub btnSqlDisconnectEngineGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectEngineGrid.Click

        ' Perform disconnect method
        SqlDisconnect()

    End Sub

    Private Sub btnSqlRefreshEngineGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshEngineGrid.Click

        ' Disable buttons
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshEngineGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportEngineGrid, False)

        ' Restart thread
        EngineGridThread = New Thread(Sub() EngineGridWorker(ConnectionString))
        EngineGridThread.Start()

    End Sub

    Private Sub btnSqlExportEngineGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportEngineGrid.Click

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
            For Each dgvColumn As DataGridViewColumn In dgvEngineGrid.Columns

                ' Write values
                StateStreamWriter.Write(dgvColumn.HeaderText + ",")

            Next

            ' Write newline
            StateStreamWriter.Write(Environment.NewLine)

            ' Iterate datagrid rows
            For Each dgvRecord As DataGridViewRow In dgvEngineGrid.Rows

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

    Private Sub btnSqlExitEngineGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitEngineGrid.Click

        ' Close the dialog
        Close()

    End Sub

End Class
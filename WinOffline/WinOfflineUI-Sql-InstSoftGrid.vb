Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared InstSoftGridThread As Thread

    Private Sub InitSqlInstSoftGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectInstSoftGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshInstSoftGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportInstSoftGrid, False)

        dgvSoftInst.AllowUserToAddRows = False
        dgvSoftInst.AllowUserToDeleteRows = False
        dgvSoftInst.AllowUserToResizeRows = False
        dgvSoftInst.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvSoftInst.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvSoftInst.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvSoftInst.BorderStyle = BorderStyle.Fixed3D
        dgvSoftInst.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvSoftInst.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvSoftInst.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvSoftInst.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvSoftInst.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvSoftInst.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvSoftInst.DefaultCellStyle.WrapMode = False
        dgvSoftInst.EnableHeadersVisualStyles = False
        dgvSoftInst.ReadOnly = True
        dgvSoftInst.RowHeadersVisible = False
        dgvSoftInst.ScrollBars = ScrollBars.Both
        dgvSoftInst.ShowCellErrors = False
        dgvSoftInst.ShowCellToolTips = False
        dgvSoftInst.ShowEditingIcon = False
        dgvSoftInst.ShowRowErrors = False

        Dim dgvType As Type = dgvSoftInst.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgvSoftInst, True, Nothing)

    End Sub

    Private Sub InstSoftGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim RecordCount As Integer = 0
        Dim CallStack As String = "InstSoftGridWorker --> "
        Dim SoftwareGrid As New List(Of List(Of String))
        Dim SoftwarePackage As New List(Of String)
        Dim SoftwareProcs As New List(Of String)
        Dim ProcLastUsageEpoch As Integer = 0
        Dim SoftwareLastUsageEpoch As Integer = 0
        Dim SoftwareApplCount As Integer = 0
        Dim EpochBaseTime As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
        Dim PercentLoaded As Integer = 0

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            Delegate_Sub_Set_Text(grpSqlInstSoftGrid, grpSqlInstSoftGrid.Text + " (Loading)")
            prgInstSoftGrid.Invoke(Sub() prgInstSoftGrid.Value = 0)
            grpSqlInstSoftGrid.Invoke(Sub() grpSqlInstSoftGrid.Height = grpSqlInstSoftGrid.Height - prgInstSoftGrid.Height - 4)

            ' Query list of software packages
            SoftwareGrid = SqlSelectGrid(CallStack, DbConnection, "select objectid, itemname, itemversion, creationdate from usd_rsw with (nolock) order by itemname, itemversion, creationdate")

            Delegate_Sub_Add_DataGridView_Column(dgvSoftInst, "itemname", "Software Package")
            Delegate_Sub_Add_DataGridView_Column(dgvSoftInst, "itemversion", "Version")
            Delegate_Sub_Add_DataGridView_Column(dgvSoftInst, "creationdate", "Registered")
            Delegate_Sub_Add_DataGridView_Column(dgvSoftInst, "maxcompletiontime", "Last Used")
            Delegate_Sub_Add_DataGridView_Column(dgvSoftInst, "instcount", "Installs")

            ' Set column data types (for sorting purposes)
            dgvSoftInst.Columns("creationdate").ValueType = GetType(DateTime)
            dgvSoftInst.Columns("maxcompletiontime").ValueType = GetType(DateTime)

            Delegate_Sub_Set_DataGrid_Fill_Column(dgvSoftInst, "itemname")

            prgInstSoftGrid.Invoke(Sub() prgInstSoftGrid.Maximum = SoftwareGrid.Count)

            For i As Integer = 0 To SoftwareGrid.Count - 1
                SoftwarePackage = SoftwareGrid.Item(i)

                SoftwareProcs = New List(Of String)
                SoftwareApplCount = 0
                ProcLastUsageEpoch = 0
                SoftwareLastUsageEpoch = 0

                ' Query install/uninstall procedures only (excluding delivery/undelivery procs)
                SoftwareProcs = SqlSelectScalarList(CallStack, DbConnection, "select objectid from usd_actproc with (nolock) where rsw=" + SoftwarePackage.Item(0) + " and task in (0, 1) and itemname not in ('delivery proc', 'undelivery proc')")

                ' Verify results are not empty (e.g. query aborted by terminate or cancel signal)
                If SoftwareProcs IsNot Nothing AndAlso SoftwareProcs.Count > 0 Then
                    For j As Integer = 0 To SoftwareProcs.Count - 1

                        ' Query applications (installs or uninstalls)
                        SoftwareApplCount += Integer.Parse(SqlSelectScalar(CallStack, DbConnection, "select count(*) from usd_applic with (nolock) where actproc=" + SoftwareProcs.Item(j) + " and status=9"))

                        ' Query most recent proc usage
                        Integer.TryParse(SqlSelectScalar(CallStack, DbConnection, "select max(completiontime) from usd_applic with (nolock) where actproc=" + SoftwareProcs.Item(j) + " and status=9"), ProcLastUsageEpoch)

                        ' Compare proc last usage to overall software last usage
                        If ProcLastUsageEpoch > SoftwareLastUsageEpoch Then
                            SoftwareLastUsageEpoch = ProcLastUsageEpoch ' Assign newer last usage date
                        End If
                    Next
                End If

                If TerminateSignal Or CancelSignal Then Exit For

                If SoftwareLastUsageEpoch = 0 Then
                    ' Populate row without last usage date
                    Delegate_Sub_Add_DataGridView_Row(dgvSoftInst, {SoftwarePackage.Item(1), SoftwarePackage.Item(2), EpochBaseTime.AddSeconds(SoftwarePackage.Item(3)).ToLocalTime, "-", SoftwareApplCount})
                Else
                    ' Populate row with last usage date
                    Delegate_Sub_Add_DataGridView_Row(dgvSoftInst, {SoftwarePackage.Item(1), SoftwarePackage.Item(2), EpochBaseTime.AddSeconds(SoftwarePackage.Item(3)).ToLocalTime, EpochBaseTime.AddSeconds(SoftwareLastUsageEpoch).ToLocalTime, SoftwareApplCount})
                End If

                ' Auto resize on first ten records and the last record
                If i < 25 OrElse i = SoftwareGrid.Count - 1 Then
                    Delegate_Sub_Resize_DataGrid_Columns(dgvSoftInst, DataGridViewAutoSizeColumnsMode.DisplayedCells)
                    Delegate_Sub_Resize_DataGrid_Rows(dgvSoftInst, DataGridViewAutoSizeRowsMode.AllCells)
                    If i = 0 Then Delegate_Sub_Resize_DataGrid_Row_Template_Height(dgvSoftInst, dgvSoftInst.Rows(0).Height)
                End If

                PercentLoaded = (i / SoftwareGrid.Count) * 100
                Delegate_Sub_Set_Text(grpSqlInstSoftGrid, grpSqlInstSoftGrid.Text.Substring(0, grpSqlInstSoftGrid.Text.IndexOf("(") - 1) + " (" + PercentLoaded.ToString + "%)")
                prgInstSoftGrid.Invoke(Sub() prgInstSoftGrid.Increment(1))

                If TerminateSignal Or CancelSignal Then Exit For
                Application.DoEvents()
            Next
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            grpSqlInstSoftGrid.Invoke(Sub() grpSqlInstSoftGrid.Height = pnlSqlInstSoftGrid.Height - pnlSqlInstSoftGridButtons.Height - 3)

            Delegate_Sub_Set_Text(grpSqlInstSoftGrid, grpSqlInstSoftGrid.Text.Substring(0, grpSqlInstSoftGrid.Text.IndexOf("(") - 1) + " (" + SoftwareGrid.Count.ToString + ")")
            prgInstSoftGrid.Invoke(Sub() prgInstSoftGrid.Value = 0)

            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If

            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshInstSoftGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportInstSoftGrid, True)
        End Try

    End Sub

    Private Sub dataGridView1_SortCompare(ByVal sender As Object, ByVal e As DataGridViewSortCompareEventArgs) Handles dgvSoftInst.SortCompare

        Try
            If e.CellValue1.ToString.Equals("-") OrElse e.CellValue2.ToString.Equals("-") Then
                If e.CellValue1.ToString.Equals("-") Then
                    e.SortResult = -1 ' Sort inferior (other cell is the same or has real value)
                Else
                    e.SortResult = 1 ' Sort superior (other cell has "-")
                End If
            Else
                e.SortResult = (TryCast(e.CellValue1, IComparable)).CompareTo(TryCast(e.CellValue2, IComparable)) ' Use normal sorting procedures
            End If
            e.Handled = True
        Catch ex As Exception
            e.Handled = True ' Sweep under rug
        End Try

    End Sub

    Private Sub btnSqlConnectInstSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectInstSoftGrid.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectInstSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectInstSoftGrid.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshInstSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshInstSoftGrid.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshInstSoftGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportInstSoftGrid, False)
        If grpSqlInstSoftGrid.Text.Contains("(") Then Delegate_Sub_Set_Text(grpSqlInstSoftGrid, grpSqlInstSoftGrid.Text.Substring(0, grpSqlInstSoftGrid.Text.IndexOf("(") - 1))
        Delegate_Sub_Clear_DataGridView(dgvSoftInst)
        InstSoftGridThread = New Thread(Sub() InstSoftGridWorker(ConnectionString))
        InstSoftGridThread.Start()
    End Sub

    Private Sub btnSqlExportInstSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportInstSoftGrid.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            For Each dgvColumn As DataGridViewColumn In dgvSoftInst.Columns
                StateStreamWriter.Write(dgvColumn.HeaderText + ",")
            Next
            StateStreamWriter.Write(Environment.NewLine)

            For Each dgvRecord As DataGridViewRow In dgvSoftInst.Rows
                For Each CellItem As DataGridViewCell In dgvRecord.Cells
                    StateStreamWriter.Write(CellItem.Value.ToString.Replace(",", "+") + ",")
                Next
                StateStreamWriter.Write(Environment.NewLine)
            Next

            StateStreamWriter.Close()
        Catch ex As Exception
            AlertBox.CreateUserAlert("Export failed." + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message)
        End Try

    End Sub

    Private Sub btnSqlExitInstSoftGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitInstSoftGrid.Click
        Close()
    End Sub

End Class
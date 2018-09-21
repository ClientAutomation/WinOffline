Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Shared GroupEvalGridThread As Thread
    Private Shared GroupEvalUpdateThread As Thread
    Private EngineGrid As New List(Of List(Of String))
    Private OriginalValues As New List(Of String)
    Private ChangeTracker As New List(Of String)
    Private SkipSelectionChange As Boolean = False

    Private Sub InitSqlGroupEvalGrid()

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectGroupEvalGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlRefreshGroupEvalGrid, False)
        Delegate_Sub_Enable_Blue_Button(btnSqlExportGroupEvalGrid, False)
        Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
        Delegate_Sub_Enable_Peru_Button(btnGroupEvalGridPreview, False)
        Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)

        dgvGroupEvalGrid.AllowUserToAddRows = False
        dgvGroupEvalGrid.AllowUserToDeleteRows = False
        dgvGroupEvalGrid.AllowUserToResizeRows = False
        dgvGroupEvalGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        dgvGroupEvalGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvGroupEvalGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvGroupEvalGrid.BorderStyle = BorderStyle.Fixed3D
        dgvGroupEvalGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        dgvGroupEvalGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvGroupEvalGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
        dgvGroupEvalGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvGroupEvalGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
        dgvGroupEvalGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
        dgvGroupEvalGrid.DefaultCellStyle.WrapMode = False
        dgvGroupEvalGrid.EnableHeadersVisualStyles = False
        dgvGroupEvalGrid.MultiSelect = False
        dgvGroupEvalGrid.ReadOnly = False
        dgvGroupEvalGrid.RowHeadersVisible = False
        dgvGroupEvalGrid.ScrollBars = ScrollBars.Both
        dgvGroupEvalGrid.ShowCellErrors = False
        dgvGroupEvalGrid.ShowCellToolTips = False
        dgvGroupEvalGrid.ShowEditingIcon = False
        dgvGroupEvalGrid.ShowRowErrors = False

    End Sub

    Private Sub GroupEvalGridWorker(ByVal ConnectionString As String)

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "GroupEvalGridWorker --> "
        Dim dgvCmbCell As DataGridViewComboBoxCell

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            grpSqlGroupEvalGrid.Invoke(Sub() grpSqlGroupEvalGrid.Height = grpSqlGroupEvalGrid.Height - prgGroupEvalGrid.Height - 4)

            ' Query group evaluation chart
            SqlPopulateGridWorker(CallStack,
                                  DbConnection,
                                  "select eng.engine_uuid as 'Engine UUID', eng.label as 'Engine Name', gd.eval_freq as 'Eval Interval (s)', gd.group_uuid as 'Group UUID', gd.label as 'Group Name', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, gd.last_eval_date_time, '19700101')) as 'Last Evaluated', qd.label as 'Query Name', dateadd(s, datediff(s, getutcdate(), getdate()), dateadd(s, qd.creation_date, '19700101')) as 'Query Creation Date' from ca_group_def gd with (nolock) inner join ca_query_def qd with (nolock) on gd.query_uuid=qd.query_uuid left join ca_engine eng with (nolock) on gd.evaluation_uuid=eng.engine_uuid order by gd.label",
                                  dgvGroupEvalGrid,
                                  "Group Name",,,
                                  prgGroupEvalGrid,
                                  grpSqlGroupEvalGrid,
                                  New List(Of String) From {"Group UUID", "Engine UUID"})

            ' Query available engines
            EngineGrid = SqlSelectGrid(CallStack, DbConnection, "select engine_uuid as 'Engine UUID', label as 'Engine Name' from ca_engine where domain_uuid in (select set_val_uuid from ca_settings where set_id=1)")

            Delegate_Sub_Insert_DataGridView_ComboBoxColumn(dgvGroupEvalGrid, dgvGroupEvalGrid.Columns("Engine Name").Index, "EvalEngine", "Evaluation Engine")
            Delegate_Sub_Hide_DataGridView_Column(dgvGroupEvalGrid, "Engine Name")

            If EngineGrid IsNot Nothing Then
                For Each dgvRow As DataGridViewRow In dgvGroupEvalGrid.Rows
                    dgvCmbCell = New DataGridViewComboBoxCell
                    dgvCmbCell.Items.Add("All Engines")

                    For Each EngineList As List(Of String) In EngineGrid
                        dgvCmbCell.Items.Add(EngineList(1).ToString) ' Add engines to combo box (0=Engine UUID, 1=Engine Name)
                        If EngineList(0).ToString.Equals(dgvRow.Cells("Engine UUID").Value.ToString) Then
                            dgvCmbCell.Value = EngineList(1).ToString
                        End If
                    Next

                    If dgvRow.Cells("Engine UUID").Value.ToString.ToLower.Equals("null") Then ' Check for "All Engines" setting (EngineUUID=NULL, EngineName=NULL, EvalInterval=0)
                        dgvCmbCell.Value = "All Engines"
                    End If

                    dgvRow.Cells("EvalEngine") = dgvCmbCell
                    OriginalValues.Add(dgvRow.Cells("Group UUID").Value.ToString + "," +
                                       dgvRow.Cells("EvalEngine").Value.ToString + "," +
                                       dgvRow.Cells("Eval Interval (s)").Value.ToString)
                Next
            End If

            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Value = 0)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Maximum = 10)

            ' Bold editable columns
            dgvGroupEvalGrid.Columns("EvalEngine").DefaultCellStyle.Font = New System.Drawing.Font(dgvGroupEvalGrid.DefaultCellStyle.Font, Drawing.FontStyle.Bold)
            dgvGroupEvalGrid.Columns("Eval Interval (s)").DefaultCellStyle.Font = New System.Drawing.Font(dgvGroupEvalGrid.DefaultCellStyle.Font, Drawing.FontStyle.Bold)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))

            ' Color editable columns
            dgvGroupEvalGrid.Columns("EvalEngine").DefaultCellStyle.BackColor = Drawing.Color.Yellow
            dgvGroupEvalGrid.Columns("Eval Interval (s)").DefaultCellStyle.BackColor = Drawing.Color.Yellow
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))

            ' Resize rows and columns after changes (and increment progress bar)
            Delegate_Sub_UnSet_DataGrid_Fill_Column(dgvGroupEvalGrid, "Group Name")
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Rows(dgvGroupEvalGrid, DataGridViewAutoSizeRowsMode.AllCells)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Column(dgvGroupEvalGrid, "EvalEngine", DataGridViewAutoSizeColumnsMode.AllCells)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Column(dgvGroupEvalGrid, "Eval Interval (s)", DataGridViewAutoSizeColumnsMode.AllCells)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Column(dgvGroupEvalGrid, "Group Name", 250)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Column(dgvGroupEvalGrid, "Last Evaluated", DataGridViewAutoSizeColumnsMode.AllCells)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Column(dgvGroupEvalGrid, "Query Name", 300)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))
            Delegate_Sub_Resize_DataGrid_Column(dgvGroupEvalGrid, "Query Creation Date", DataGridViewAutoSizeColumnsMode.AllCells)
            prgGroupEvalGrid.Invoke(Sub() prgGroupEvalGrid.Increment(1))

            grpSqlGroupEvalGrid.Invoke(Sub() grpSqlGroupEvalGrid.Height = grpSqlGroupEvalGrid.Height + prgGroupEvalGrid.Height + 4)

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DbConnection.State = ConnectionState.Closed Then
                DbConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If
            Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshGroupEvalGrid, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlExportGroupEvalGrid, True)
        End Try

    End Sub

    Private Sub dgvGroupEvalGrid_CellEnter(ByVal sender As System.Object, ByVal e As DataGridViewCellEventArgs) Handles dgvGroupEvalGrid.CellEnter
        SendKeys.Send("{F4}") ' Send edit key (avoid need for two clicks)
    End Sub

    Private Sub dgvGroupEvalGrid_EditingControlShowing(ByVal sender As System.Object, ByVal e As DataGridViewEditingControlShowingEventArgs) Handles dgvGroupEvalGrid.EditingControlShowing

        Dim dgvCmbCell As ComboBox

        If dgvGroupEvalGrid.CurrentCell.ColumnIndex = dgvGroupEvalGrid.Columns("EvalEngine").Index Then
            dgvCmbCell = CType(e.Control, ComboBox)
            If dgvCmbCell IsNot Nothing Then
                RemoveHandler dgvCmbCell.SelectionChangeCommitted, New EventHandler(AddressOf EvalEngine_SelectionChangeCommitted)
                AddHandler dgvCmbCell.SelectionChangeCommitted, New EventHandler(AddressOf EvalEngine_SelectionChangeCommitted)
            End If
        End If

    End Sub

    Private Sub EvalEngine_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dgvCmbCell As ComboBox = CType(sender, ComboBox)
        dgvGroupEvalGrid.Rows(dgvGroupEvalGrid.CurrentCell.RowIndex).Cells("EvalEngine").Value = dgvCmbCell.SelectedItem.ToString ' Commit cell value change (which will fire cell value changed event)
    End Sub

    Private Sub dgvGroupEvalGrid_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvGroupEvalGrid.CellValueChanged

        Dim RowIndex As Integer = dgvGroupEvalGrid.CurrentCell.RowIndex
        Dim ColIndex As Integer = dgvGroupEvalGrid.CurrentCell.ColumnIndex
        Dim GroupUUID As String = dgvGroupEvalGrid.Rows(RowIndex).Cells("Group UUID").Value.ToString
        Dim EngineUUID As String = dgvGroupEvalGrid.Rows(RowIndex).Cells("Engine UUID").Value.ToString
        Dim EvalEngine As String = dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Value.ToString
        Dim EvalInterval As String = dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value.ToString
        Dim NumberCheck As Integer = -1

        ' Skip cascading calls
        If SkipSelectionChange Then Return

        ' Lock cascading calls
        SkipSelectionChange = True

        Try
            If ColIndex = dgvGroupEvalGrid.Columns("EvalEngine").Index Then
                If EvalEngine.Equals("All Engines") Then
                    dgvGroupEvalGrid.Rows(RowIndex).Cells("Engine UUID").Value = "Null"
                    If Not dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value.ToString.Equals("0") Then
                        dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value = "0"
                    End If
                Else
                    dgvGroupEvalGrid.Rows(RowIndex).Cells("Engine UUID").Value = GetEngineUUID(EvalEngine)

                    ' Check if eval interval is 0 (since this is non-"All Engines" case and valid values are 60-604800)
                    If dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value.ToString.Equals("0") Then
                        If GetOrigEvalInterval(GroupUUID) = 0 Then
                            dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value = 86400 ' Set default value (1 day = 86400 seconds)
                        Else
                            dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value = GetOrigEvalInterval(GroupUUID) ' Restore engine eval interval
                        End If
                    End If
                End If

            ElseIf ColIndex = dgvGroupEvalGrid.Columns("Eval Interval (s)").Index Then
                If Integer.TryParse(EvalInterval, NumberCheck) Then
                    If NumberCheck = 0 AndAlso Not dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Value.Equals("All Engines") Then
                        MsgBox("An evaluation interval of '0' is only valid when the evaluation engine is set to 'All Engines'.", MsgBoxStyle.Exclamation, "Nope")
                        dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value = GetOrigEvalInterval(GroupUUID)
                    ElseIf NumberCheck <> 0 AndAlso dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Value.Equals("All Engines") Then
                        MsgBox("Evaluation interval must be '0' when the evaluation engine is set to 'All Engines'.", MsgBoxStyle.Exclamation, "Nope")
                        dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value = 0
                    ElseIf Not (NumberCheck >= 60 AndAlso NumberCheck <= 604800) Then
                        MsgBox("Evaluation interval must be an integer between 60 seconds and 604800 seconds.", MsgBoxStyle.Exclamation, "Nope")
                        dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value = GetOrigEvalInterval(GroupUUID)
                    End If
                Else
                    MsgBox("Evaluation interval must be an integer between 60 seconds and 604800 seconds.", MsgBoxStyle.Exclamation, "Nope")
                    dgvGroupEvalGrid.Rows(RowIndex).Cells(ColIndex).Value = GetOrigEvalInterval(GroupUUID)
                End If
            End If

            If dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Value.Equals(GetOrigEvalEngine(GroupUUID)) Then
                ' Back to orignal value -- restore font color
                dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Style.Font = New System.Drawing.Font(dgvGroupEvalGrid.DefaultCellStyle.Font, Drawing.FontStyle.Bold)
                dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Style.ForeColor = System.Drawing.Color.Black
            Else
                ' Change font color
                dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Style.Font = New System.Drawing.Font(dgvGroupEvalGrid.DefaultCellStyle.Font, Drawing.FontStyle.Bold Or Drawing.FontStyle.Italic)
                dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Style.ForeColor = System.Drawing.Color.Red
            End If

            ' Check if evaluation interval has changed
            If Integer.Parse(dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value) = GetOrigEvalInterval(GroupUUID) Then
                ' Back to orignal value -- restore font color
                dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Style.Font = New System.Drawing.Font(dgvGroupEvalGrid.DefaultCellStyle.Font, Drawing.FontStyle.Bold)
                dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Style.ForeColor = System.Drawing.Color.Black
            Else
                ' Change font
                dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Style.Font = New System.Drawing.Font(dgvGroupEvalGrid.DefaultCellStyle.Font, Drawing.FontStyle.Bold Or Drawing.FontStyle.Italic)
                dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Style.ForeColor = System.Drawing.Color.Red
            End If

            ' Update the tracker only if there are changes from original values
            If Not dgvGroupEvalGrid.Rows(RowIndex).Cells("EvalEngine").Value.Equals(GetOrigEvalEngine(GroupUUID)) OrElse
                Not Integer.Parse(dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value) = GetOrigEvalInterval(GroupUUID) Then

                ' Refresh values before updating tracker
                EngineUUID = dgvGroupEvalGrid.Rows(RowIndex).Cells("Engine UUID").Value.ToString
                EvalInterval = dgvGroupEvalGrid.Rows(RowIndex).Cells("Eval Interval (s)").Value.ToString

                ' Update change tracker
                RemoveFromChangeTracker(GroupUUID)
                AddtoChangeTracker(GroupUUID, EngineUUID, EvalInterval)
            Else
                RemoveFromChangeTracker(GroupUUID) ' Remove from change tracker
            End If

            ' Check if there are changes
            If ChangeTracker.Count > 0 Then
                Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, True)
                Delegate_Sub_Enable_Peru_Button(btnGroupEvalGridPreview, True)
                Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, True)
            Else
                Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
                Delegate_Sub_Enable_Peru_Button(btnGroupEvalGridPreview, False)
                Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)
            End If

        Finally
            SkipSelectionChange = False ' Unlock cascading calls
        End Try

    End Sub

    Private Sub AddtoChangeTracker(ByVal GroupUUID As String, ByVal EngineUUID As String, ByVal EvalInterval As Integer)
        For i As Integer = 0 To ChangeTracker.Count - 1
            If ChangeTracker.Item(i).ToString.Equals(GroupUUID) Then
                ChangeTracker.RemoveAt(i) ' Remove the group uuid, engine uuid and eval interval
                ChangeTracker.RemoveAt(i)
                ChangeTracker.RemoveAt(i)
            End If
        Next
        ChangeTracker.Add(GroupUUID) ' Add values to change tracker
        ChangeTracker.Add(EngineUUID)
        ChangeTracker.Add(EvalInterval.ToString)
    End Sub

    Private Sub RemoveFromChangeTracker(ByVal GroupUUID As String)
        For i As Integer = 0 To ChangeTracker.Count - 1
            If ChangeTracker.Item(i).ToString.Equals(GroupUUID) Then
                ChangeTracker.RemoveAt(i)
                ChangeTracker.RemoveAt(i)
                ChangeTracker.RemoveAt(i)
                Exit For
            End If
        Next
    End Sub

    Private Function GetOrigEvalEngine(ByVal GroupUUID As String) As String
        For Each value As String In OriginalValues
            If GroupUUID.Equals(value.Substring(0, value.IndexOf(","))) Then
                Return value.Substring(value.IndexOf(",") + 1, value.LastIndexOf(",") - value.Substring(0, value.IndexOf(",")).Length - 1)
            End If
        Next
        Return Nothing
    End Function

    Private Function GetOrigEvalInterval(ByVal GroupUUID As String) As Integer
        For Each value As String In OriginalValues
            If GroupUUID.Equals(value.Substring(0, value.IndexOf(","))) Then
                Return Integer.Parse(value.Substring(value.LastIndexOf(",") + 1))
            End If
        Next
        Return -1
    End Function

    Private Function GetEngineUUID(ByVal EngineName As String) As String
        For Each EngineList As List(Of String) In EngineGrid ' Iterate engine grid (0=Engine UUID, 1=Engine Name)
            If EngineList(1).ToString.ToLower.Equals(EngineName.ToLower) Then
                Return EngineList(0)
            End If
        Next
        Return Nothing
    End Function

    Private Sub btnGroupEvalGridCommit_Click(sender As Object, e As EventArgs) Handles btnGroupEvalGridCommit.Click
        Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
        Delegate_Sub_Enable_Peru_Button(btnGroupEvalGridPreview, False)
        Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)
        GroupEvalUpdateThread = New Thread(Sub() GroupEvalUpdateWorker(ConnectionString))
        GroupEvalUpdateThread.Start()
    End Sub

    Private Sub GroupEvalUpdateWorker(ByVal ConnectionString As String)

        Dim CallStack As String = "GroupEvalUpdateWorker --> "
        Dim QueryText As String
        Dim DatabaseConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim SqlCmd As SqlCommand
        Dim GroupUUID As String
        Dim EngineUUID As String
        Dim EvalInterval As String
        Dim GroupsAffected As Integer = 0
        Dim RecordsAffected As Integer = 0

        Try
            DatabaseConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            For i As Integer = 0 To ChangeTracker.Count - 1 Step 3
                GroupUUID = ChangeTracker(i)
                EngineUUID = ChangeTracker(i + 1)
                EvalInterval = ChangeTracker(i + 2)
                QueryText = "update ca_group_def set eval_freq=" + EvalInterval + ", evaluation_uuid=" + EngineUUID + " where group_uuid=" + GroupUUID
                SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement:" + Environment.NewLine + QueryText)
                RecordsAffected += SqlCmd.ExecuteNonQuery
                GroupsAffected += 1
            Next

            MsgBox(RecordsAffected.ToString + " record(s) affected, " + GroupsAffected.ToString + " group(s) affected.", MsgBoxStyle.Information, "Changes committed.")
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not DatabaseConnection.State = ConnectionState.Closed Then
                DatabaseConnection.Close()
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
            End If
        End Try

        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshGroupEvalGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportGroupEvalGrid, False)
        Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
        Delegate_Sub_Enable_Peru_Button(btnGroupEvalGridPreview, False)
        Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)

        OriginalValues = New List(Of String)
        ChangeTracker = New List(Of String)

        GroupEvalGridThread = New Thread(Sub() GroupEvalGridWorker(ConnectionString))
        GroupEvalGridThread.Start()

    End Sub

    Private Sub btnGroupEvalGridPreview_Click(sender As Object, e As EventArgs) Handles btnGroupEvalGridPreview.Click

        Dim GroupUUID As String
        Dim EngineUUID As String
        Dim EvalInterval As String
        Dim PreviewString As String = ""

        For i As Integer = 0 To ChangeTracker.Count - 1 Step 3
            GroupUUID = ChangeTracker(i)
            EngineUUID = ChangeTracker(i + 1)
            EvalInterval = ChangeTracker(i + 2)
            PreviewString += "update ca_group_def set eval_freq=" + EvalInterval + ", evaluation_uuid=" + EngineUUID + " where group_uuid=" + GroupUUID + Environment.NewLine
        Next

        AlertBox.CreateUserAlert(PreviewString, 0, HorizontalAlignment.Left, False, 9, True)

    End Sub

    Private Sub btnGroupEvalGridDiscard_Click(sender As Object, e As EventArgs) Handles btnGroupEvalGridDiscard.Click
        ChangeTracker = New List(Of String)
        btnSqlRefreshGroupEvalGrid.PerformClick()
        Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
        Delegate_Sub_Enable_Peru_Button(btnGroupEvalGridPreview, False)
        Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)
    End Sub

    Private Sub btnSqlConnectGroupEvalGrid_Click(sender As Object, e As EventArgs) Handles btnSqlConnectGroupEvalGrid.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectGroupEvalGrid_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectGroupEvalGrid.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlRefreshGroupEvalGrid_Click(sender As Object, e As EventArgs) Handles btnSqlRefreshGroupEvalGrid.Click
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshGroupEvalGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportGroupEvalGrid, False)
        Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
        Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)
        OriginalValues = New List(Of String)
        GroupEvalGridThread = New Thread(Sub() GroupEvalGridWorker(ConnectionString))
        GroupEvalGridThread.Start()
    End Sub

    Private Sub btnSqlExportGroupEvalGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExportGroupEvalGrid.Click

        Dim saveFileDialog1 As New SaveFileDialog()
        Dim StateStreamWriter As System.IO.StreamWriter

        saveFileDialog1.Filter = "CSV (Comma delimited)|*.csv"
        saveFileDialog1.Title = "Save a CSV File"

        If saveFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

        Try
            StateStreamWriter = New System.IO.StreamWriter(saveFileDialog1.FileName, False)

            For Each dgvColumn As DataGridViewColumn In dgvGroupEvalGrid.Columns
                StateStreamWriter.Write(dgvColumn.HeaderText + ",")
            Next
            StateStreamWriter.Write(Environment.NewLine)

            For Each dgvRecord As DataGridViewRow In dgvGroupEvalGrid.Rows
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

    Private Sub btnSqlExitGroupEvalGrid_Click(sender As Object, e As EventArgs) Handles btnSqlExitGroupEvalGrid.Click
        Close()
    End Sub

End Class
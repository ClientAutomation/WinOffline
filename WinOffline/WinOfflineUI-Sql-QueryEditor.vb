Imports System.Threading
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Partial Public Class WinOfflineUI

    Private Shared SqlQueryEditorThread As Thread
    Private Shared CancelQuery As Boolean = False

    Private Sub InitSqlQueryEditor()
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectQueryEditor, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlSubmitQueryEditor, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlCancelQueryEditor, False)
    End Sub

    Private Sub btnSqlConnectQueryEditor_Click(sender As Object, e As EventArgs) Handles btnSqlConnectQueryEditor.Click
        SqlConnect()
    End Sub

    Private Sub btnSqlDisconnectQueryEditor_Click(sender As Object, e As EventArgs) Handles btnSqlDisconnectQueryEditor.Click
        SqlDisconnect()
    End Sub

    Private Sub btnSqlSubmitQueryEditor_Click(sender As Object, e As EventArgs) Handles btnSqlSubmitQueryEditor.Click

        Dim ConnectionString As String

        ConnectionString = BuildSqlConnectionString()
        If ConnectionString Is Nothing Then Return

        Delegate_Sub_Enable_Control(rtbSqlQuery, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlSubmitQueryEditor, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlCancelQueryEditor, True)
        Delegate_Select_Button(btnSqlCancelQueryEditor)

        SqlQueryEditorThread = New Thread(Sub() SqlQueryEditorWorker(ConnectionString, Callback_Function_Read_Selected_Text(rtbSqlQuery)))
        SqlQueryEditorThread.Start()

    End Sub

    Private Sub btnSqlCancelQueryEditor_Click(sender As Object, e As EventArgs) Handles btnSqlCancelQueryEditor.Click
        CancelQuery = True
        Delegate_Sub_Set_Text(txtSqlMessage, "Query cancelled by user.")
        Delegate_Sub_Set_Active_Tab(TabControlSql, TabPageMessages) ' Change the active tab to the messages tab
        Delegate_Sub_Enable_Tan_Button(btnSqlCancelQueryEditor, False)
    End Sub

    Private Sub btnSqlExitQueryEditor_Click(sender As Object, e As EventArgs) Handles btnSqlExitQueryEditor.Click
        Close()
    End Sub

    Private Sub SqlQueryEditorWorker(ByVal ConnectionString As String, ByVal QueryText As String)

        Dim DbConnection As SqlConnection
        Dim SqlBatches As String()
        Dim SqlCmd As SqlCommand
        Dim SqlData As SqlDataReader
        Dim SqlResultRow As New ArrayList
        Dim CellCounter As Integer = 0
        Dim SqlResultGrid As DataGridView
        Dim TotalRecordReturnCount As Integer = 0
        Dim BatchRecordReturnCount As Integer = 0
        Dim pnlTableLayout As New TableLayoutPanel
        Dim CallStack As String = "SqlQueryEditorWorker --> "

        pnlTableLayout.Dock = DockStyle.Fill
        pnlTableLayout.AutoScroll = True
        pnlTableLayout.AutoSize = True
        pnlTableLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink

        Delegate_Sub_Set_Text(TabPageGrid, "Results")
        Delegate_Sub_Remove_Child_Controls(TabPageGrid)
        Delegate_Sub_Add_Control(TabPageGrid, pnlTableLayout)

        Delegate_Sub_Set_Text(txtSqlMessage, "")
        Delegate_Sub_Set_Active_Tab(TabControlSql, TabPageMessages)

        Try
            DbConnection = New SqlConnection(ConnectionString)
            AddHandler DbConnection.InfoMessage, New SqlInfoMessageEventHandler(AddressOf OnSqlQueryEditorInfoMessage)
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            ' Enumerate sql batches based on "GO"
            SqlBatches = Regex.Split(QueryText, "^\s*GO\s*\d*\s*($|\-\-.*$)", RegexOptions.Multiline Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase)

            For Each QueryLine As String In SqlBatches
                If QueryLine.Equals("") Then Continue For
                SqlCmd = New SqlCommand(QueryLine, DbConnection)
                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement(s):" + Environment.NewLine + QueryLine)
                SqlData = SqlCmd.ExecuteReader()

                If TerminateSignal Or CancelSignal Or CancelQuery Then
                    SqlData.Close()
                    DbConnection.Close()
                    Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
                    Return
                End If

                Do
                    If SqlData.HasRows Then
                        SqlResultGrid = New DataGridView
                        SqlResultGrid.AllowUserToAddRows = False
                        SqlResultGrid.AllowUserToDeleteRows = False
                        SqlResultGrid.AllowUserToResizeRows = False
                        SqlResultGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
                        SqlResultGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                        SqlResultGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                        SqlResultGrid.BorderStyle = BorderStyle.Fixed3D
                        SqlResultGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
                        SqlResultGrid.ColumnHeadersDefaultCellStyle.Font = New Drawing.Font("Calibri", 9)
                        SqlResultGrid.ColumnHeadersDefaultCellStyle.WrapMode = False
                        SqlResultGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
                        SqlResultGrid.DefaultCellStyle.BackColor = Drawing.Color.Beige
                        SqlResultGrid.DefaultCellStyle.Font = New Drawing.Font("Calibri", 8)
                        SqlResultGrid.DefaultCellStyle.WrapMode = False
                        SqlResultGrid.EnableHeadersVisualStyles = False
                        SqlResultGrid.ReadOnly = True
                        SqlResultGrid.RowHeadersVisible = False
                        SqlResultGrid.ScrollBars = ScrollBars.Both
                        SqlResultGrid.ShowCellErrors = False
                        SqlResultGrid.ShowCellToolTips = False
                        SqlResultGrid.ShowEditingIcon = False
                        SqlResultGrid.ShowRowErrors = False

                        pnlTableLayout.SetCellPosition(SqlResultGrid, New TableLayoutPanelCellPosition(0, CellCounter))
                        Delegate_Sub_Add_Control(pnlTableLayout, SqlResultGrid)
                        Delegate_Sub_Set_Active_Tab(TabControlSql, TabPageGrid)

                        CellCounter += 1

                        For i As Integer = 0 To SqlData.FieldCount - 1
                            If SqlData.GetName(i).Equals("") Then
                                Delegate_Sub_Add_DataGridView_Column(SqlResultGrid, "col_" + i.ToString, "(No column name)")
                            Else
                                Delegate_Sub_Add_DataGridView_Column(SqlResultGrid, "col_" + i.ToString, SqlData.GetName(i))
                            End If
                        Next

                        While SqlData.Read
                            For i As Integer = 0 To SqlData.FieldCount - 1
                                If SqlData.IsDBNull(i) Then
                                    SqlResultRow.Add("Null")
                                ElseIf SqlData.GetFieldType(i) Is GetType(Byte()) Then
                                    SqlResultRow.Add("0x" + BitConverter.ToString(SqlData.GetSqlBinary(i).Value).Replace("-", ""))
                                Else
                                    SqlResultRow.Add(SqlData(i).ToString)
                                End If
                            Next
                            Delegate_Sub_Add_DataGridView_Row(SqlResultGrid, SqlResultRow.ToArray)
                            SqlResultRow.Clear()
                            BatchRecordReturnCount += 1

                            If TerminateSignal Or CancelSignal Or CancelQuery Then
                                SqlData.Close()
                                DbConnection.Close()
                                Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
                                Return
                            End If
                        End While
                    End If

                    If SqlData.RecordsAffected <> -1 Then
                        Delegate_Sub_Append_Text(txtSqlMessage, Environment.NewLine +
                                                         String.Format("({0} record(s) affected)" +
                                                         Environment.NewLine, SqlData.RecordsAffected.ToString))
                        Delegate_Sub_Append_Text(rtbDebug, CallStack + String.Format("({0} record(s) affected)", SqlData.RecordsAffected.ToString))
                    Else
                        Delegate_Sub_Append_Text(txtSqlMessage, Environment.NewLine +
                                                         String.Format("(0 record(s) affected, {0} row(s) returned)", BatchRecordReturnCount.ToString) +
                                                         Environment.NewLine)
                        Delegate_Sub_Append_Text(rtbDebug, CallStack + String.Format("(0 record(s) affected, {0} row(s) returned)", BatchRecordReturnCount.ToString))
                    End If

                    TotalRecordReturnCount += BatchRecordReturnCount
                    BatchRecordReturnCount = 0

                    If TerminateSignal Or CancelSignal Or CancelQuery Then
                        SqlData.Close()
                        DbConnection.Close()
                        Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
                        Return
                    End If
                Loop While SqlData.NextResult
                SqlData.Close()
            Next

            If TotalRecordReturnCount > 0 Then
                Delegate_Sub_Set_Text(TabPageGrid, "Results (" + TotalRecordReturnCount.ToString + " records)")
            Else
                Delegate_Sub_Set_Text(TabPageGrid, "Results")
            End If
        Catch ex As Exception
            Delegate_Sub_Set_Text(txtSqlMessage, ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Stack trace: " + Environment.NewLine + ex.StackTrace)
        Finally
            If Not TerminateSignal Then
                Delegate_Sub_Enable_Control(rtbSqlQuery, True)
                Delegate_Sub_Enable_Blue_Button(btnSqlSubmitQueryEditor, True)
                Delegate_Sub_Enable_Blue_Button(btnSqlCancelQueryEditor, False)
                Delegate_Select_Button(btnSqlSubmitQueryEditor)
                CancelQuery = False
            End If
        End Try

    End Sub

    Private Sub txtSqlQuery_TextChanged(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles rtbSqlQuery.KeyDown, btnSqlSubmitQueryEditor.KeyDown
        If e.KeyCode = Keys.F5 Then
            btnSqlSubmitQueryEditor.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub OnSqlQueryEditorInfoMessage(ByVal sender As Object, ByVal e As SqlInfoMessageEventArgs)
        Delegate_Sub_Append_Text(txtSqlMessage, e.Message)
    End Sub

End Class
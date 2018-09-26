Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Public Const WINDOWS_AUTHENTICATION As String = "Windows Authentication"
    Public Const SQL_AUTHENTICATION As String = "SQL Authentication"

    Public Shared SqlServer As String = ""
    Public Shared InstanceName As String = ""
    Public Shared PortNumber As String = "1433"
    Public Shared DatabaseName As String = "mdb"
    Public Shared AuthType As String = WINDOWS_AUTHENTICATION
    Public Shared SqlUser As String = "ca_itrm"
    Public Shared SqlPassword As String = ""
    Public Shared ConnectionString As String = Nothing
    Public Shared DatabaseThread As Thread = Nothing
    Public Shared CancelSignal As Boolean = False
    Public Shared DomainUuid As String = Nothing
    Public Shared DomainId As String = Nothing
    Public Shared DomainType As String = Nothing
    Public Shared ManagerName As String = Nothing

    Public Sub SqlConnect()

        Dim SqlConnectForm As SqlConnectUI = New SqlConnectUI

        If SqlConnectForm.ShowDialog() = DialogResult.Abort Then
            Return
        End If

        ConnectionString = BuildSqlConnectionString()
        If ConnectionString Is Nothing Then
            Return
        End If

        Delegate_Sub_Enable_Green_Button(btnSqlConnectMdbOverview, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectTableSpace, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectAgentGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectUserGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectServerGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectEngineGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectGroupEvalGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectInstSoftGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectDiscSoftGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectDuplCompGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectUnUsedSoftGrid, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectCleanApps, False)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectQueryEditor, False)

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectMdbOverview, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectTableSpace, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectAgentGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectUserGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectServerGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectEngineGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectGroupEvalGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectInstSoftGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectDiscSoftGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectDuplCompGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectUnUsedSoftGrid, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectCleanApps, True)
        Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, True)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectQueryEditor, True)
        Delegate_Sub_Enable_Yellow_Button(btnSqlSubmitQueryEditor, True)

        CancelSignal = False

        DatabaseThread = New Thread(Sub() SqlDatabaseWorker())
        DatabaseThread.Start()

    End Sub

    Public Sub SqlDisconnect()

        CancelSignal = True

        While True
            If (MdbOverviewThread IsNot Nothing AndAlso MdbOverviewThread.IsAlive) OrElse
               (TableSpaceGridThread IsNot Nothing AndAlso TableSpaceGridThread.IsAlive) OrElse
               (AgentGridThread IsNot Nothing AndAlso AgentGridThread.IsAlive) OrElse
               (UserGridThread IsNot Nothing AndAlso UserGridThread.IsAlive) OrElse
               (ServerGridThread IsNot Nothing AndAlso ServerGridThread.IsAlive) OrElse
               (EngineGridThread IsNot Nothing AndAlso EngineGridThread.IsAlive) OrElse
               (GroupEvalGridThread IsNot Nothing AndAlso GroupEvalGridThread.IsAlive) OrElse
               (InstSoftGridThread IsNot Nothing AndAlso InstSoftGridThread.IsAlive) OrElse
               (DiscSoftGridThread IsNot Nothing AndAlso DiscSoftGridThread.IsAlive) OrElse
               (UnUsedSoftGridThread IsNot Nothing AndAlso UnUsedSoftGridThread.IsAlive) OrElse
               (DuplCompGridThread IsNot Nothing AndAlso DuplCompGridThread.IsAlive) OrElse
               (SqlCleanAppsThread IsNot Nothing AndAlso SqlCleanAppsThread.IsAlive) OrElse
               (SqlQueryEditorThread IsNot Nothing AndAlso SqlQueryEditorThread.IsAlive) Then
                Application.DoEvents()
            Else
                Exit While
            End If
        End While

        MdbOverviewThread = Nothing
        TableSpaceGridThread = Nothing
        AgentGridThread = Nothing
        UserGridThread = Nothing
        ServerGridThread = Nothing
        EngineGridThread = Nothing
        GroupEvalGridThread = Nothing
        InstSoftGridThread = Nothing
        DiscSoftGridThread = Nothing
        UnUsedSoftGridThread = Nothing
        DuplCompGridThread = Nothing
        SqlCleanAppsThread = Nothing
        SqlQueryEditorThread = Nothing

        Delegate_Sub_Set_Text(txtMdbVersion, "")
        Delegate_Sub_Set_Text(txtITCMVersion, "")
        Delegate_Sub_Set_Text(txtMdbInstallDate, "")
        Delegate_Sub_Set_Text(txtMdbType, "")
        Delegate_Sub_Clear_ListView(lvwITCMSummary)
        Delegate_Sub_Clear_DataGridView(dgvAgentVersion)
        Delegate_Sub_Clear_DataGridView(dgvContentSummary)
        Delegate_Sub_Clear_DataGridView(dgvTableSpaceGrid)
        Delegate_Sub_Clear_DataGridView(dgvAgentGrid)
        Delegate_Sub_Clear_DataGridView(dgvAgentObsolete90)
        Delegate_Sub_Clear_DataGridView(dgvAgentObsolete365)
        Delegate_Sub_Clear_DataGridView(dgvUserGrid)
        Delegate_Sub_Clear_DataGridView(dgvUserObsolete90)
        Delegate_Sub_Clear_DataGridView(dgvUserObsolete365)
        Delegate_Sub_Clear_DataGridView(dgvServerGrid)
        Delegate_Sub_Clear_DataGridView(dgvServerLastCollected24)
        Delegate_Sub_Clear_DataGridView(dgvServerSignature30)
        Delegate_Sub_Clear_DataGridView(dgvEngineGrid)
        Delegate_Sub_Clear_DataGridView(dgvGroupEvalGrid)
        Delegate_Sub_Clear_DataGridView(dgvSoftInst)
        Delegate_Sub_Clear_DataGridView(dgvDiscSignature)
        Delegate_Sub_Clear_DataGridView(dgvDiscCustom)
        Delegate_Sub_Clear_DataGridView(dgvDiscHeuristic)
        Delegate_Sub_Clear_DataGridView(dgvDiscIntellisig)
        Delegate_Sub_Clear_DataGridView(dgvDiscEverything)
        Delegate_Sub_Clear_DataGridView(dgvDuplHostname)
        Delegate_Sub_Clear_DataGridView(dgvDuplSerialNum)
        Delegate_Sub_Clear_DataGridView(dgvDuplBoth)
        Delegate_Sub_Clear_DataGridView(dgvDuplBlank)
        Delegate_Sub_Clear_DataGridView(dgvSwNotUsed)
        Delegate_Sub_Clear_DataGridView(dgvSwNotInst)
        Delegate_Sub_Clear_DataGridView(dgvSwNotStaged)

        If tabDiscSignature.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscSignature, tabDiscSignature.Text.Substring(0, tabDiscSignature.Text.IndexOf("(") - 1))
        If tabDiscCustom.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscCustom, tabDiscCustom.Text.Substring(0, tabDiscCustom.Text.IndexOf("(") - 1))
        If tabDiscHeuristic.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscHeuristic, tabDiscHeuristic.Text.Substring(0, tabDiscHeuristic.Text.IndexOf("(") - 1))
        If tabDiscIntellisig.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscIntellisig, tabDiscIntellisig.Text.Substring(0, tabDiscIntellisig.Text.IndexOf("(") - 1))
        If tabDiscEverything.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDiscEverything, tabDiscEverything.Text.Substring(0, tabDiscEverything.Text.IndexOf("(") - 1))
        If tabDuplHostname.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplHostname, tabDuplHostname.Text.Substring(0, tabDuplHostname.Text.IndexOf("(") - 1))
        If tabDuplSerialNum.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplSerialNum, tabDuplSerialNum.Text.Substring(0, tabDuplSerialNum.Text.IndexOf("(") - 1))
        If tabDuplBoth.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplBoth, tabDuplBoth.Text.Substring(0, tabDuplBoth.Text.IndexOf("(") - 1))
        If tabDuplBlank.Text.Contains("(") Then Delegate_Sub_Set_Text(tabDuplBlank, tabDuplBlank.Text.Substring(0, tabDuplBlank.Text.IndexOf("(") - 1))
        If grpSqlInstSoftGrid.Text.Contains("(") Then Delegate_Sub_Set_Text(grpSqlInstSoftGrid, grpSqlInstSoftGrid.Text.Substring(0, grpSqlInstSoftGrid.Text.IndexOf("(") - 1))
        If tabSwNotUsed.Text.Contains("(") Then Delegate_Sub_Set_Text(tabSwNotUsed, tabSwNotUsed.Text.Substring(0, tabSwNotUsed.Text.IndexOf("(") - 1))
        If tabSwNotInst.Text.Contains("(") Then Delegate_Sub_Set_Text(tabSwNotInst, tabSwNotInst.Text.Substring(0, tabSwNotInst.Text.IndexOf("(") - 1))
        If tabSwNotStaged.Text.Contains("(") Then Delegate_Sub_Set_Text(tabSwNotStaged, tabSwNotStaged.Text.Substring(0, tabSwNotStaged.Text.IndexOf("(") - 1))
        If tabAgentSummary.Text.Contains("(") Then Delegate_Sub_Set_Text(tabAgentSummary, tabAgentSummary.Text.Substring(0, tabAgentSummary.Text.IndexOf("(") - 1))
        If tabAgentObsolete90.Text.Contains("(") Then Delegate_Sub_Set_Text(tabAgentObsolete90, tabAgentObsolete90.Text.Substring(0, tabAgentObsolete90.Text.IndexOf("(") - 1))
        If tabAgentObsolete365.Text.Contains("(") Then Delegate_Sub_Set_Text(tabAgentObsolete365, tabAgentObsolete365.Text.Substring(0, tabAgentObsolete365.Text.IndexOf("(") - 1))
        If tabUserSummary.Text.Contains("(") Then Delegate_Sub_Set_Text(tabUserSummary, tabUserSummary.Text.Substring(0, tabUserSummary.Text.IndexOf("(") - 1))
        If tabUserObsolete90.Text.Contains("(") Then Delegate_Sub_Set_Text(tabUserObsolete90, tabUserObsolete90.Text.Substring(0, tabUserObsolete90.Text.IndexOf("(") - 1))
        If tabUserObsolete365.Text.Contains("(") Then Delegate_Sub_Set_Text(tabUserObsolete365, tabUserObsolete365.Text.Substring(0, tabUserObsolete365.Text.IndexOf("(") - 1))
        If tabServerSummary.Text.Contains("(") Then Delegate_Sub_Set_Text(tabServerSummary, tabServerSummary.Text.Substring(0, tabServerSummary.Text.IndexOf("(") - 1))
        If tabServerLastCollected24.Text.Contains("(") Then Delegate_Sub_Set_Text(tabServerLastCollected24, tabServerLastCollected24.Text.Substring(0, tabServerLastCollected24.Text.IndexOf("(") - 1))
        If tabServerSignature30.Text.Contains("(") Then Delegate_Sub_Set_Text(tabServerSignature30, tabServerSignature30.Text.Substring(0, tabServerSignature30.Text.IndexOf("(") - 1))
        If grpSqlTableSpace.Text.Contains("(") Then Delegate_Sub_Set_Text(grpSqlTableSpace, grpSqlTableSpace.Text.Substring(0, grpSqlTableSpace.Text.IndexOf("(") - 1))
        If grpSqlGroupEvalGrid.Text.Contains("(") Then Delegate_Sub_Set_Text(grpSqlGroupEvalGrid, grpSqlGroupEvalGrid.Text.Substring(0, grpSqlGroupEvalGrid.Text.IndexOf("(") - 1))

        Delegate_Sub_Enable_Green_Button(btnSqlConnectMdbOverview, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectTableSpace, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectAgentGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectUserGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectServerGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectEngineGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectGroupEvalGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectInstSoftGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectDiscSoftGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectDuplCompGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectUnUsedSoftGrid, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectCleanApps, True)
        Delegate_Sub_Enable_Green_Button(btnSqlConnectQueryEditor, True)

        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectMdbOverview, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshMdbOverview, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportMdbOverview, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectTableSpace, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshTableSpace, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportTableSpace, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectAgentGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshAgentGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportAgentGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectUserGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshUserGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportUserGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectServerGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshServerGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportServerGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectEngineGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshEngineGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportEngineGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectGroupEvalGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshGroupEvalGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportGroupEvalGrid, False)
        Delegate_Sub_Enable_CadetBlue_Button(btnGroupEvalGridCommit, False)
        Delegate_Sub_Enable_LightCoral_Button(btnGroupEvalGridDiscard, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectInstSoftGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshInstSoftGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportInstSoftGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectDiscSoftGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshDiscSoftGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportDiscSoftGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectDuplCompGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshDuplCompGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportDuplCompGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectUnUsedSoftGrid, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlRefreshUnUsedSoftGrid, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlExportUnUsedSoftGrid, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectCleanApps, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, False)
        Delegate_Sub_Enable_Red_Button(btnSqlDisconnectQueryEditor, False)
        Delegate_Sub_Enable_Yellow_Button(btnSqlSubmitQueryEditor, False)
        Delegate_Sub_Enable_Tan_Button(btnSqlCancelQueryEditor, False)

    End Sub

    Public Function BuildSqlConnectionString() As String

        Dim ConnectionString As String = Nothing

        ' Check for empty username or password
        If AuthType.Equals(SQL_AUTHENTICATION) AndAlso (SqlUser.Equals("") Or SqlPassword.Equals("")) Then
            AlertBox.CreateUserAlert("Invalid sql username or password.")
            Return Nothing
        End If

        ' Check for instance or port
        If InstanceName.Equals("") And PortNumber.Equals("") And Not (SqlServer.Contains("\") Or SqlServer.Contains(",")) Then
            AlertBox.CreateUserAlert("An instance name or port number is required.")
            Return Nothing
        End If

        ' Check for instance or port
        If DatabaseName.Equals("") Then
            AlertBox.CreateUserAlert("A database name is required.")
            Return Nothing
        End If

        ' Check authentication type
        If AuthType.Equals(SQL_AUTHENTICATION) Then
            If SqlServer.Contains("\") Or SqlServer.Contains(",") Then
                ConnectionString = "Server=" + SqlServer
                ConnectionString += ";Database=" + DatabaseName
                ConnectionString += ";User Id=" + SqlUser
                ConnectionString += ";Password=" + SqlPassword
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            Else
                ConnectionString = "Server=" + SqlServer
                ConnectionString += "\" + InstanceName
                ConnectionString += "," + PortNumber
                ConnectionString += ";Database=" + DatabaseName
                ConnectionString += ";User Id=" + SqlUser
                ConnectionString += ";Password=" + SqlPassword
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            End If
        Else
            If SqlServer.Contains("\") Or SqlServer.Contains(",") Then
                ConnectionString = "Server=" + SqlServer
                ConnectionString += ";Database=" + DatabaseName
                ConnectionString += ";Trusted_Connection=True"
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            Else
                ConnectionString = "Server=" + SqlServer
                ConnectionString += "\" + InstanceName
                ConnectionString += "," + PortNumber
                ConnectionString += ";Database=" + DatabaseName
                ConnectionString += ";Trusted_Connection=True"
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            End If
        End If

        Return ConnectionString

    End Function

    Public Sub SqlDatabaseWorker()

        Dim DbConnection As SqlConnection = New SqlConnection(ConnectionString)
        Dim CallStack As String = "DatabaseWorker --> "

        Try
            DbConnection.Open()
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected successful: " + SqlServer)

            DomainUuid = SqlSelectScalar(CallStack, DbConnection, "select set_val_uuid from ca_settings with (nolock) where set_id=1")
            Delegate_Sub_Append_Text(rtbDebug, "DatabaseWorker --> Domain UUID: " + DomainUuid)

            DomainId = SqlSelectScalar(CallStack, DbConnection, "select domain_id from ca_n_tier with (nolock) where domain_uuid=" + DomainUuid)
            Delegate_Sub_Append_Text(rtbDebug, "DatabaseWorker --> Domain ID: " + DomainId)

            DomainType = SqlSelectScalar(CallStack, DbConnection, "select domain_type from ca_n_tier with (nolock) where domain_uuid=" + DomainUuid)
            Delegate_Sub_Append_Text(rtbDebug, "DatabaseWorker --> Domain Type: " + DomainType)

            ManagerName = SqlSelectScalar(CallStack, DbConnection, "select label from ca_manager with (nolock) where domain_uuid=" + DomainUuid)
            Delegate_Sub_Append_Text(rtbDebug, "DatabaseWorker --> Manager Name: " + ManagerName)

            If Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("SqlMdbOverviewNode") Then
                MdbOverviewThread = New Thread(Sub() MdbOverviewWorker(ConnectionString))
                MdbOverviewThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("SqlTableSpaceGridNode") Then
                TableSpaceGridThread = New Thread(Sub() TableSpaceGridWorker(ConnectionString))
                TableSpaceGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("AgentGridNode") Then
                AgentGridThread = New Thread(Sub() AgentGridWorker(ConnectionString))
                AgentGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("UserGridNode") Then
                UserGridThread = New Thread(Sub() UserGridWorker(ConnectionString))
                UserGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("ServerGridNode") Then
                ServerGridThread = New Thread(Sub() ServerGridWorker(ConnectionString))
                ServerGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("EngineGridNode") Then
                EngineGridThread = New Thread(Sub() EngineGridWorker(ConnectionString))
                EngineGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("GroupEvalGridNode") Then
                GroupEvalGridThread = New Thread(Sub() GroupEvalGridWorker(ConnectionString))
                GroupEvalGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("InstSoftGridNode") Then
                InstSoftGridThread = New Thread(Sub() InstSoftGridWorker(ConnectionString))
                InstSoftGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("DiscSoftGridNode") Then
                DiscSoftGridThread = New Thread(Sub() DiscSoftGridWorker(ConnectionString))
                DiscSoftGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("DuplCompGridNode") Then
                DuplCompGridThread = New Thread(Sub() DuplCompGridWorker(ConnectionString))
                DuplCompGridThread.Start()
            ElseIf Callback_Function_Read_Selected_TreeNode(ExplorerTree).Equals("UnUsedSoftGridNode") Then
                UnUsedSoftGridThread = New Thread(Sub() UnUsedSoftGridWorker(ConnectionString))
                UnUsedSoftGridThread.Start()
            End If

            Delegate_Sub_Enable_Yellow_Button(btnSqlCafStopCleanApps, True)
            Delegate_Sub_Enable_Tan_Button(btnSqlRunCleanApps, True)

        Catch ex As Exception
            AlertBox.CreateUserAlert(ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Connected failed: " + ex.Message)
            SqlDisconnect()
            Return
        End Try

        While Not TerminateSignal Or CancelSignal
            If CancelSignal OrElse DbConnection.State = ConnectionState.Broken OrElse DbConnection.State = ConnectionState.Closed Then
                CancelSignal = True
                If Not DbConnection.State = ConnectionState.Closed Then
                    DbConnection.Close()
                    Delegate_Sub_Append_Text(rtbDebug, CallStack + "Database connection closed.")
                End If
                SqlDisconnect()
                Exit While
            End If
            Application.DoEvents()
        End While

    End Sub

    Public Function SqlSelectScalar(ByVal CallStack As String, ByVal DatabaseConnection As SqlConnection, ByVal QueryText As String) As String

        Dim SqlCmd As SqlCommand
        Dim SqlData As SqlDataReader
        Dim ScalarValue As String = ""

        CallStack += "SqlSelectScalar --> "

        Try
            If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                Return Nothing
            End If

            SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement:" + Environment.NewLine + QueryText)
            SqlData = SqlCmd.ExecuteReader()

            If TerminateSignal Or CancelSignal Then
                SqlData.Close()
                Return Nothing
            End If

            If SqlData.Read() AndAlso SqlData.HasRows Then
                If SqlData.IsDBNull(0) Then
                    ScalarValue = "Null"
                ElseIf SqlData.GetFieldType(0) Is GetType(Byte()) Then
                    ScalarValue = "0x" + BitConverter.ToString(SqlData.GetSqlBinary(0).Value).Replace("-", "")
                Else
                    ScalarValue = SqlData(0).ToString
                End If
            End If

            SqlData.Close()
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Return ex.Message
        End Try

        Return ScalarValue

    End Function

    Public Function SqlSelectScalarList(ByVal CallStack As String, ByVal DatabaseConnection As SqlConnection, ByVal QueryText As String) As List(Of String)

        Dim SqlCmd As SqlCommand
        Dim SqlData As SqlDataReader
        Dim ResultArray As New List(Of String)

        CallStack += "SqlSelectScalarList --> "

        Try
            If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                Return Nothing
            End If

            SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement:" + Environment.NewLine + QueryText)
            SqlData = SqlCmd.ExecuteReader()

            If TerminateSignal Or CancelSignal Then
                SqlData.Close()
                Return Nothing
            End If

            Do
                If SqlData.HasRows Then
                    While SqlData.Read
                        If SqlData.IsDBNull(0) Then
                            ResultArray.Add("Null")
                        ElseIf SqlData.GetFieldType(0) Is GetType(Byte()) Then
                            ResultArray.Add("0x" + BitConverter.ToString(SqlData.GetSqlBinary(0).Value).Replace("-", ""))
                        Else
                            ResultArray.Add(SqlData(0).ToString)
                        End If
                    End While
                End If
            Loop While SqlData.NextResult

            SqlData.Close()
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
        End Try

        Return ResultArray

    End Function

    Public Function SqlSelectGrid(ByVal CallStack As String,
                                  ByVal DatabaseConnection As SqlConnection,
                                  ByVal QueryText As String,
                                  Optional ByVal IncludeColumnHeaders As Boolean = False) As List(Of List(Of String))

        Dim SqlCmd As SqlCommand
        Dim SqlData As SqlDataReader
        Dim RowArray As New List(Of String)
        Dim ResultArray As New List(Of List(Of String))

        CallStack += "SqlSelectGrid --> "

        Try
            If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                Return Nothing
            End If

            SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement:" + Environment.NewLine + QueryText)
            SqlData = SqlCmd.ExecuteReader()

            If TerminateSignal Or CancelSignal Then
                SqlData.Close()
                Return Nothing
            End If

            Do
                If SqlData.HasRows Then
                    If IncludeColumnHeaders Then
                        For i As Integer = 0 To SqlData.FieldCount - 1
                            If SqlData.GetName(i).Equals("") Then
                                RowArray.Add("(No column name)")
                            Else
                                RowArray.Add(SqlData.GetName(i))
                            End If
                            Application.DoEvents()
                        Next
                        ResultArray.Add(RowArray)
                    End If

                    While SqlData.Read
                        RowArray = New List(Of String)
                        For i As Integer = 0 To SqlData.FieldCount - 1
                            If SqlData.IsDBNull(i) Then
                                RowArray.Add("Null")
                            ElseIf SqlData.GetFieldType(i) Is GetType(Byte()) Then
                                RowArray.Add("0x" + BitConverter.ToString(SqlData.GetSqlBinary(i).Value).Replace("-", ""))
                            Else
                                RowArray.Add(SqlData(i))
                            End If
                            Application.DoEvents()
                        Next
                        ResultArray.Add(RowArray)
                        If TerminateSignal Or CancelSignal Then
                            SqlData.Close()
                            Return Nothing
                        End If
                    End While
                End If

                If TerminateSignal Or CancelSignal Then
                    SqlData.Close()
                    Return Nothing
                End If
            Loop While SqlData.NextResult

            SqlData.Close()
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
        End Try

        Return ResultArray

    End Function

    Public Sub SqlPopulateGridWorker(ByVal CallStack As String,
                                     ByVal DatabaseConnection As SqlConnection,
                                     ByVal QueryText As String,
                                     ByRef dgvResultGrid As DataGridView,
                                     Optional ByVal FillColumn As String = "",
                                     Optional ByVal AutoResizeColumns As DataGridViewAutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.NotSet,
                                     Optional ByVal AutoResizeRows As DataGridViewAutoSizeRowsMode = DataGridViewAutoSizeColumnMode.NotSet,
                                     Optional ByVal LoadingBar As ProgressBar = Nothing,
                                     Optional ByVal LoadingControl As Control = Nothing,
                                     Optional ByVal HideColumns As List(Of String) = Nothing)

        Dim SqlCmd As SqlCommand
        Dim SqlReader As SqlDataReader
        Dim SqlResultTable As New DataTable
        Dim SqlDataRow As DataRow
        Dim SqlResultRow As New ArrayList
        Dim PercentLoaded As Integer = 0
        Dim PreviousPercentage As Integer = 0

        CallStack += "SqlPopulateGridWorker --> "

        Delegate_Sub_Clear_DataGridView(dgvResultGrid)

        Try
            If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then Return

            If LoadingControl IsNot Nothing Then
                Delegate_Sub_Set_Text(LoadingControl, LoadingControl.Text + " (Loading)")
            End If

            SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement:" + Environment.NewLine + QueryText)
            SqlReader = SqlCmd.ExecuteReader()
            SqlResultTable.Load(SqlReader)
            SqlReader.Close()

            If TerminateSignal Or CancelSignal Then Return

            If LoadingBar IsNot Nothing AndAlso SqlResultTable.Rows.Count > 0 Then
                LoadingBar.Invoke(Sub() LoadingBar.Value = 0)
                LoadingBar.Invoke(Sub() LoadingBar.Maximum = SqlResultTable.Rows.Count)
            End If

            If LoadingControl IsNot Nothing Then
                Delegate_Sub_Set_Text(LoadingControl, LoadingControl.Text + " (" + PercentLoaded.ToString + "%)")
            End If

            For i As Integer = 0 To SqlResultTable.Columns.Count - 1
                If SqlResultTable.Columns(i).ColumnName.Equals("") Then
                    Delegate_Sub_Add_DataGridView_Column(dgvResultGrid, "col_" + i.ToString, "(No column name)")
                Else
                    Delegate_Sub_Add_DataGridView_Column(dgvResultGrid, SqlResultTable.Columns(i).ColumnName, SqlResultTable.Columns(i).ColumnName)
                End If
                Application.DoEvents()
            Next

            If HideColumns IsNot Nothing Then
                For Each HiddenColumn As String In HideColumns
                    Delegate_Sub_Hide_DataGridView_Column(dgvResultGrid, HiddenColumn)
                Next
            End If

            For i As Integer = 0 To SqlResultTable.Rows.Count - 1
                SqlDataRow = SqlResultTable.Rows(i)
                For j As Integer = 0 To SqlDataRow.ItemArray.Length - 1
                    If SqlDataRow.IsNull(j) Then
                        SqlResultRow.Add("Null")
                    ElseIf SqlDataRow.Item(j).GetType Is GetType(Byte()) Then
                        SqlResultRow.Add("0x" + BitConverter.ToString(SqlDataRow.Item(j)).Replace("-", ""))
                    Else
                        SqlResultRow.Add(SqlDataRow.Item(j))
                    End If
                    If TerminateSignal Or CancelSignal Then Return
                    Application.DoEvents()
                Next

                Delegate_Sub_Add_DataGridView_Row(dgvResultGrid, SqlResultRow.ToArray)

                If i < 10 OrElse i = SqlResultTable.Rows.Count - 1 Then ' Auto resize on first ten records and the last record
                    If AutoResizeColumns <> 0 Then
                        Delegate_Sub_Resize_DataGrid_Columns(dgvResultGrid, AutoResizeColumns) ' Perform one-time auto resize of columns
                    End If

                    If AutoResizeRows <> 0 Then
                        Delegate_Sub_Resize_DataGrid_Rows(dgvResultGrid, AutoResizeRows) ' Perform one-time auto resize of rows
                        If i = 0 Then Delegate_Sub_Resize_DataGrid_Row_Template_Height(dgvResultGrid, dgvResultGrid.Rows(0).Height) ' Adjust row template for future rows
                    End If

                    If Not FillColumn.Equals("") Then
                        Delegate_Sub_Set_DataGrid_Fill_Column(dgvResultGrid, FillColumn) ' Set the fill column
                    End If
                End If

                SqlResultRow.Clear()
                PercentLoaded = (SqlResultTable.Rows.IndexOf(SqlDataRow) / SqlResultTable.Rows.Count) * 100

                If LoadingBar IsNot Nothing Then
                    LoadingBar.Invoke(Sub() LoadingBar.Increment(1))
                End If

                ' Increment control text (if available) only if there's a change in percentage-- this is to reduce flickering of the control
                If LoadingControl IsNot Nothing AndAlso PreviousPercentage <> PercentLoaded Then
                    Delegate_Sub_Set_Text(LoadingControl, LoadingControl.Text.Substring(0, LoadingControl.Text.IndexOf("(") - 1) + " (" + PercentLoaded.ToString + "%)")
                End If

                ' Update previous percentage only if there's a full integer change
                If PreviousPercentage <> PercentLoaded Then
                    PreviousPercentage = PercentLoaded
                End If

                If TerminateSignal Or CancelSignal Then Return
                Application.DoEvents()
            Next

        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
            Delegate_Sub_Add_DataGridView_Column(dgvResultGrid, "Exception", "Exception")
            Delegate_Sub_Add_DataGridView_Row(dgvResultGrid, {ex.Message})
            Delegate_Sub_Resize_DataGrid_Columns(dgvResultGrid, AutoResizeColumns)
        Finally
            If LoadingBar IsNot Nothing Then
                LoadingBar.Invoke(Sub() LoadingBar.Value = 0)
            End If
            If LoadingControl IsNot Nothing Then
                Delegate_Sub_Set_Text(LoadingControl, LoadingControl.Text.Substring(0, LoadingControl.Text.IndexOf("(") - 1) + " (" + SqlResultTable.Rows.Count.ToString + ")")
            End If
        End Try

    End Sub

    Public Function SqlSimplePopulateGridWorker(ByVal CallStack As String,
                                                ByVal DatabaseConnection As SqlConnection,
                                                ByVal QueryText As String,
                                                ByRef dgvResultGrid As DataGridView,
                                                Optional ByVal FillColumn As String = "") As Integer

        Dim SqlCmd As SqlCommand
        Dim SqlReader As SqlDataReader
        Dim SqlResultRow As New ArrayList
        Dim TotalRecordReturnCount As Integer = 0
        Dim BatchRecordReturnCount As Integer = 0

        CallStack += "SqlSimplePopulateGridWorker --> "

        Delegate_Sub_Clear_DataGridView(dgvResultGrid)

        Try
            If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                Return -1
            End If

            SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Execute statement:" + Environment.NewLine + QueryText)
            SqlReader = SqlCmd.ExecuteReader()

            If TerminateSignal Or CancelSignal Then
                SqlReader.Close()
                Return -1
            End If

            Do
                If SqlReader.HasRows Then
                    For i As Integer = 0 To SqlReader.FieldCount - 1
                        If SqlReader.GetName(i).Equals("") Then
                            Delegate_Sub_Add_DataGridView_Column(dgvResultGrid, "col_" + i.ToString, "(No column name)")
                        Else
                            Delegate_Sub_Add_DataGridView_Column(dgvResultGrid, SqlReader.GetName(i), SqlReader.GetName(i))
                        End If
                        Application.DoEvents()
                    Next

                    If Not FillColumn.Equals("") Then
                        Delegate_Sub_Set_DataGrid_Fill_Column(dgvResultGrid, FillColumn)
                    End If

                    While SqlReader.Read
                        For i As Integer = 0 To SqlReader.FieldCount - 1
                            If SqlReader.IsDBNull(i) Then
                                SqlResultRow.Add("Null")
                            ElseIf SqlReader.GetFieldType(i) Is GetType(Byte()) Then
                                SqlResultRow.Add("0x" + BitConverter.ToString(SqlReader.GetSqlBinary(i).Value).Replace("-", ""))
                            Else
                                SqlResultRow.Add(SqlReader(i))
                            End If
                            If TerminateSignal Or CancelSignal Then
                                SqlReader.Close()
                                Return -1
                            End If
                            Application.DoEvents()
                        Next

                        Delegate_Sub_Add_DataGridView_Row(dgvResultGrid, SqlResultRow.ToArray)
                        SqlResultRow.Clear()
                        BatchRecordReturnCount += 1

                        If TerminateSignal Or CancelSignal Then
                            SqlReader.Close()
                            Return -1
                        End If
                        Application.DoEvents()
                    End While
                End If

                TotalRecordReturnCount += BatchRecordReturnCount
                BatchRecordReturnCount = 0

                If TerminateSignal Or CancelSignal Then
                    SqlReader.Close()
                    Return -1
                End If
            Loop While SqlReader.NextResult

            SqlReader.Close()
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, CallStack + "Exception:" + Environment.NewLine + ex.Message)
        End Try

        Return TotalRecordReturnCount

    End Function

End Class
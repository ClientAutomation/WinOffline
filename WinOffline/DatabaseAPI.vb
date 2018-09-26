Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Partial Public Class WinOffline

    Public Class DatabaseAPI

        Public Shared ConnectionString As String = Nothing
        Public Shared DatabaseThread As Thread = Nothing
        Public Shared CancelSignal As Boolean = False
        Public Shared DomainUuid As String = Nothing
        Public Shared DomainId As String = Nothing
        Public Shared DomainType As String = Nothing
        Public Shared ManagerName As String = Nothing

        Public Shared Function SQLFunctionDispatch(ByVal Callstack As String) As Integer

            Dim ConnectionString As String = Nothing
            Dim DbConnection As SqlConnection = Nothing

            Callstack += "DatabaseAPI|"

            ' Perform verifications
            If Not Utility.IsITCMInstalled Then
                Logger.WriteDebug(Callstack, "Error: ITCM is not installed.")
                Return 0
            End If

            If Not Globals.ITCMFunction.ToLower.Contains("manager") Then
                Logger.WriteDebug(Callstack, "Error: Database functions only available on ITCM managers.")
                Return 0
            End If

            If Globals.DatabaseServer Is Nothing Then
                Logger.WriteDebug(Callstack, "Error: Database server name from comstore is empty.")
                Return 0
            End If

            If Globals.DbUser Is Nothing Then
                Globals.DbUser = Globals.ProcessIdentity.Name
                Logger.WriteDebug(Callstack, "Logon as: " + Globals.DbUser)
            End If

            If Globals.DbPassword Is Nothing Then
                Console.WriteLine() ' Write directly to attached console (this is not logged)
                Console.WriteLine(Callstack.Substring(Callstack.IndexOf("DatabaseAPI")) + "Logon as: " + Globals.DbUser)
                Console.Write(Callstack.Substring(Callstack.IndexOf("DatabaseAPI")) + "Enter password: ")

                KeyboardHook.SetHook() ' Hook the keyboard for low-level input

                While (KeyboardHook.KeyboardHooked) ' Wait for keyboard release
                    Windows.Forms.Application.DoEvents()
                    Thread.Sleep(50)
                End While

                Globals.DbPassword = KeyboardHook.CapturedString.ToString
            End If

            ' Build connection string
            If Globals.DbUser.Contains("\") Then
                ConnectionString = "Server=" + Globals.DatabaseServer
                ConnectionString += "\" + Globals.DatabaseInstance
                ConnectionString += "," + Globals.DatabasePort
                ConnectionString += ";Database=" + Globals.DatabaseName
                ConnectionString += ";Trusted_Connection=True"
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            Else
                ConnectionString = "Server=" + Globals.DatabaseServer
                ConnectionString += "\" + Globals.DatabaseInstance
                ConnectionString += "," + Globals.DatabasePort
                ConnectionString += ";Database=" + Globals.DatabaseName
                ConnectionString += ";User Id=" + Globals.DbUser
                ConnectionString += ";Password=" + Globals.DbPassword
                ConnectionString += ";Application Name=" + Globals.ProcessFriendlyName + " " + Globals.AppVersion
            End If

            Try
                ' Connect to database
                DbConnection = New SqlConnection(ConnectionString)
                DbConnection.Open()
                DomainUuid = SqlSelectScalar(Callstack, DbConnection, "select set_val_uuid from ca_settings with (nolock) where set_id=1")
                Logger.WriteDebug(Callstack, "Domain UUID: " + DomainUuid)
                DomainId = SqlSelectScalar(Callstack, DbConnection, "select domain_id from ca_n_tier with (nolock) where domain_uuid=" + DomainUuid)
                Logger.WriteDebug(Callstack, "Domain ID: " + DomainId)
                DomainType = SqlSelectScalar(Callstack, DbConnection, "select domain_type from ca_n_tier with (nolock) where domain_uuid=" + DomainUuid)
                Logger.WriteDebug(Callstack, "Domain type: " + DomainType)
                ManagerName = SqlSelectScalar(Callstack, DbConnection, "select label from ca_manager with (nolock) where domain_uuid=" + DomainUuid)
                Logger.WriteDebug(Callstack, "Manager name: " + ManagerName)

                ' Dispatch
                If Globals.MdbOverviewSwitch Then
                    Logger.WriteDebug(Callstack, "Domain managers: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_n_tier with (nolock) where domain_type=0"))
                    Logger.WriteDebug(Callstack, "Scalability Servers: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_server with (nolock)"))
                    Logger.WriteDebug(Callstack, "Registered Agents: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=1"))
                    Logger.WriteDebug(Callstack, "Registered Users: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=2"))
                    Logger.WriteDebug(Callstack, "Registered Profiles: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=4"))
                    Logger.WriteDebug(Callstack, "Obsolete Agents [>=90 days]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=1 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-7776000"))
                    Logger.WriteDebug(Callstack, "Obsolete Agents [>=1 year]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=1 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-31536000"))
                    Logger.WriteDebug(Callstack, "Obsolete Users [>=90 days]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=2 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-7776000"))
                    Logger.WriteDebug(Callstack, "Obsolete Users [>=1 year]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=2 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-31536000"))
                    Logger.WriteDebug(Callstack, "Obsolete Profiles [>=90 days]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=4 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-7776000"))
                    Logger.WriteDebug(Callstack, "Obsolete Profiles [>=1 year]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_agent with (nolock) where agent_type=4 and last_run_date<DATEDIFF(s,'19700101',GETUTCDATE())-31536000"))
                    Logger.WriteDebug(Callstack, "Group Definitions [Static]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_group_def with (nolock) where member_type=1 and query_uuid is null"))
                    Logger.WriteDebug(Callstack, "Group Definitions [Dynamic]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_group_def with (nolock) where member_type=1 and query_uuid is not null"))
                    Logger.WriteDebug(Callstack, "Query Definitions [Simple]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_query_def with (nolock)"))
                    Logger.WriteDebug(Callstack, "Query Definitions [Complex]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_query_def_contents with (nolock)"))
                    Logger.WriteDebug(Callstack, "Software Packages: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_rsw with (nolock) where itemtype<>5"))
                    Logger.WriteDebug(Callstack, "Job Containers [In-Progress]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_job_cont with (nolock) where type=3 and comment<>'CA-Software Delivery Reserved Group' and objectid in (select jcont from usd_link_jc_act with (nolock) where activity in (select objectid from usd_activity with (nolock) where state not in (2,3)))"))
                    Logger.WriteDebug(Callstack, "Job Containers [Completed]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_job_cont with (nolock) where type=3 and comment<>'CA-Software Delivery Reserved Group' and objectid in (select jcont from usd_link_jc_act with (nolock) where activity in (select objectid from usd_activity with (nolock) where state in (2,3)))"))
                    Logger.WriteDebug(Callstack, "Software Jobs [In-Progress]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_applic with (nolock) where status in (7,8,27)"))
                    Logger.WriteDebug(Callstack, "Software Jobs [Completed]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_applic with (nolock) where status=9"))
                    Logger.WriteDebug(Callstack, "Staging Jobs [In-Progress]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_applic with (nolock) where status in (2,3,17,18,19,20,21)"))
                    Logger.WriteDebug(Callstack, "Staging Jobs [Completed]: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_applic with (nolock) where status=4"))
                    Logger.WriteDebug(Callstack, "Software Based Policies: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from usd_job_cont with (nolock) where type=4 and comment<>'CA-Software Delivery Reserved Group'"))
                    Logger.WriteDebug(Callstack, "Query Based Policies: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from polidef with (nolock)"))
                    Logger.WriteDebug(Callstack, "Engine Instances: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ca_engine with (nolock)"))
                    Logger.WriteDebug(Callstack, "Engine Tasks: " + SqlSelectScalar(Callstack, DbConnection, "select count(*) from ncjobcfg with (nolock) where jotype<>2 and domainid=" + DomainId))

                ElseIf Globals.MdbCleanAppsSwitch Then
                    Dim SqlScript As String = My.Resources.SQL_CleanApps_Unified
                    Dim SqlBatches As String()
                    Dim SqlCmd As SqlCommand
                    Dim SqlData As SqlDataReader

                    AddHandler DbConnection.InfoMessage, New SqlInfoMessageEventHandler(AddressOf OnMdbCleanAppsInfoMessage)

                    ' Enumerate sql batches based on "GO"
                    SqlBatches = Regex.Split(SqlScript, "^\s*GO\s*\d*\s*($|\-\-.*$)", RegexOptions.Multiline Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase)

                    ' Execute sql batches
                    For Each QueryLine As String In SqlBatches
                        If QueryLine.Equals("") Then Continue For
                        SqlCmd = New SqlCommand(QueryLine, DbConnection)
                        SqlData = SqlCmd.ExecuteReader()
                        SqlData.Close()
                    Next

                End If

            Catch ex As Exception
                Logger.WriteDebug(Callstack, "Exception: " + ex.Message)
            Finally
                If Not DbConnection.State = ConnectionState.Closed Then
                    DbConnection.Close()
                    Logger.WriteDebug(Callstack, "Database connection closed.")
                End If
            End Try

            Return 0

        End Function

        Private Shared Sub OnMdbCleanAppsInfoMessage(ByVal sender As Object, ByVal e As SqlInfoMessageEventArgs)
            Logger.WriteDebug(Logger.LastCallStack, e.Message)
        End Sub

        Public Shared Function SqlSelectScalar(ByVal CallStack As String, ByVal DatabaseConnection As SqlConnection, ByVal QueryText As String) As String

            Dim SqlCmd As SqlCommand
            Dim SqlData As SqlDataReader
            Dim ScalarValue As String = ""

            Try
                If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                    Return Nothing
                End If

                SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
                SqlData = SqlCmd.ExecuteReader()

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
                Logger.WriteDebug(CallStack, "Database Exception: " + ex.Message)
                Return ex.Message
            End Try

            Return ScalarValue

        End Function

        Public Shared Function SqlSelectScalarList(ByVal CallStack As String, ByVal DatabaseConnection As SqlConnection, ByVal QueryText As String) As List(Of String)

            Dim SqlCmd As SqlCommand
            Dim SqlData As SqlDataReader
            Dim ResultArray As New List(Of String)

            Try
                If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                    Return Nothing
                End If

                SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
                SqlData = SqlCmd.ExecuteReader()

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
                Logger.WriteDebug(CallStack, "Database Exception: " + ex.Message)
            End Try

            Return ResultArray

        End Function

        Public Shared Function SqlSelectGrid(ByVal CallStack As String,
                                             ByVal DatabaseConnection As SqlConnection,
                                             ByVal QueryText As String,
                                             Optional ByVal IncludeColumnHeaders As Boolean = False) As List(Of List(Of String))

            Dim SqlCmd As SqlCommand
            Dim SqlData As SqlDataReader
            Dim RowArray As New List(Of String)
            Dim ResultArray As New List(Of List(Of String))

            Try
                If DatabaseConnection.State = ConnectionState.Closed Or DatabaseConnection.State = ConnectionState.Broken Then
                    Return Nothing
                End If

                SqlCmd = New SqlCommand(QueryText, DatabaseConnection)
                SqlData = SqlCmd.ExecuteReader()

                Do
                    If SqlData.HasRows Then
                        If IncludeColumnHeaders Then
                            For i As Integer = 0 To SqlData.FieldCount - 1
                                If SqlData.GetName(i).Equals("") Then
                                    RowArray.Add("(No column name)")
                                Else
                                    RowArray.Add(SqlData.GetName(i))
                                End If
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
                            Next
                            ResultArray.Add(RowArray)
                        End While
                    End If
                Loop While SqlData.NextResult

                SqlData.Close()

            Catch ex As Exception
                Logger.WriteDebug(CallStack, "Database Exception: " + ex.Message)
            End Try

            Return ResultArray

        End Function

    End Class

End Class
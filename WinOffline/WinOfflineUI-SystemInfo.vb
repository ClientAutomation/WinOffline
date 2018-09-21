Imports System.Management
Imports System.Windows.Forms
Imports System.Threading

Partial Public Class WinOfflineUI

    Private Shared MasterIPv4list As New ArrayList
    Private Shared MasterIPv6list As New ArrayList

    Private Sub InitSystemInfo()

        txtHostname.Text = Globals.HostName
        txtPlatform.Text = My.Computer.Info.OSFullName + Environment.OSVersion.ServicePack

        If Globals.ITCMInstalled Then
            txtFunction.Text = Globals.ITCMFunction
            txtVersion.Text = Globals.ITCMVersion
            txtHostUUID.Text = Globals.HostUUID
            txtDomainMgr.Text = Globals.DomainManager
            txtScalabilityServer.Text = Globals.ScalabilityServer
            txtDSMFolder.Text = Globals.DSMFolder
            txtCAMFolder.Text = Globals.CAMFolder
            txtSSAFolder.Text = Globals.SSAFolder
        Else
            txtFunction.Text = "Not Installed"
            txtVersion.Text = "N/A"
            txtHostUUID.Text = "N/A"
            txtDomainMgr.Text = "N/A"
            txtScalabilityServer.Text = "N/A"
            txtDSMFolder.Text = "N/A"
            txtCAMFolder.Text = "N/A"
            txtSSAFolder.Text = "N/A"
        End If

        ' Query WMI for system serial number
        Try
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_BIOS")
            For Each queryObj As ManagementObject In searcher.Get()
                txtSerial.Text = queryObj("SerialNumber")
            Next
        Catch err As ManagementException
            txtSerial.Text = "Unavailable"
        End Try

        ' Query WMI for manufacturer and model
        Try
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_ComputerSystem")
            For Each queryObj As ManagementObject In searcher.Get()
                txtManufacturer.Text = queryObj("Manufacturer")
                txtModel.Text = queryObj("Model")
            Next
        Catch err As ManagementException
            txtManufacturer.Text = "Unavailable"
            txtModel.Text = "Unavailable"
        End Try

        ' Populate IP Addresses
        For Each NetAddr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(Globals.HostName).AddressList
            If NetAddr.IsIPv6LinkLocal Or NetAddr.IsIPv6Multicast Or NetAddr.IsIPv6SiteLocal Then
                MasterIPv6list.Add(NetAddr)
            Else
                MasterIPv4list.Add(NetAddr)
            End If
        Next

        ' Add IPv4 addresses first
        For Each NetAddr As System.Net.IPAddress In MasterIPv4list
            lstNetAddr.Items.Add(NetAddr.ToString)
        Next

        ' Add IPv6 addresses next
        For Each NetAddr As System.Net.IPAddress In MasterIPv6list
            lstNetAddr.Items.Add(NetAddr.ToString)
        Next

        ' Populate plugin list
        If Globals.ITCMInstalled Then
            For Each plugin As String In Globals.FeatureList
                lstPlugins.Items.Add(plugin)
            Next
        Else
            lstPlugins.Items.Add("Not Installed")
        End If

        SystemInfoUpdateThread = New Thread(AddressOf SystemInfoWorker)
        SystemInfoUpdateThread.Start()

    End Sub

    Private Sub SystemInfoWorker()

        Dim LoopCounter As Integer = 0
        Dim ProductInfoKey As Microsoft.Win32.RegistryKey = Nothing
        Dim FeatureKeys As String()
        Dim FeatureValue As String
        Dim TempString As String
        Dim ClientAutoFunction As String
        Dim ENCFunction As String
        Dim PluginList As New ArrayList
        Dim IPv4List As New ArrayList
        Dim IPv6List As New ArrayList

        Try
            While (Not TerminateSignal) ' System Info will refresh for duration of WinOfflineUI lifecycle
                LoopCounter = 0
                While LoopCounter < 799 ' 50ms * 800 = ~40s
                    If TerminateSignal Then Return
                    LoopCounter += 1
                    Thread.Sleep(50)
                End While

                LoopCounter = 0
                ProductInfoKey = Nothing
                FeatureKeys = Nothing
                FeatureValue = Nothing
                TempString = Nothing
                ClientAutoFunction = Nothing
                ENCFunction = Nothing
                PluginList = New ArrayList
                IPv4List = New ArrayList
                IPv6List = New ArrayList

                If Not My.Computer.Name.Equals(Globals.HostName) Then
                    Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Hostname updated.")
                    Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.HostName)
                    Delegate_Sub_Append_Text(rtbDebug, "New value: " + My.Computer.Name)
                    Globals.HostName = My.Computer.Name
                    Delegate_Sub_Set_Text(txtHostname, Globals.HostName)
                End If

                If Not WinOffline.Utility.IsITCMInstalled Then
                    Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto is not installed.")
                    Globals.ITCMInstalled = False
                    Delegate_Sub_Set_Text(txtFunction, "Not Installed")
                    Delegate_Sub_Set_Text(txtVersion, "N/A")
                    Delegate_Sub_Set_Text(txtHostUUID, "N/A")
                    Delegate_Sub_Set_Text(txtDomainMgr, "N/A")
                    Delegate_Sub_Set_Text(txtScalabilityServer, "N/A")
                    Delegate_Sub_Set_Text(txtDSMFolder, "N/A")
                    Delegate_Sub_Set_Text(txtCAMFolder, "N/A")
                    Delegate_Sub_Set_Text(txtSSAFolder, "N/A")
                    Delegate_Sub_Clear_ListBox(lstPlugins)
                    Delegate_Sub_Set_ListBox(lstPlugins, New ArrayList({"Not Installed"}))
                Else
                    If RemovalToolThread Is Nothing Then
                        Globals.ITCMInstalled = True
                        Delegate_Sub_Enable_Control(btnRemoveITCM, True)
                        Delegate_Sub_Enable_Control(rbnRemoveITCM, True)
                        Delegate_Sub_Enable_Control(chkRetainHostUUID, True)
                        Delegate_Sub_Enable_Control(rbnUninstallITCM, True)
                    Else
                        Globals.ITCMInstalled = False
                    End If
                End If

                If Globals.ITCMInstalled Then
                    Try
                        TempString = Environment.GetEnvironmentVariable("CAI_MSQ", EnvironmentVariableTarget.Machine)
                        If Not TempString.Equals(Globals.CAMFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> CAM folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.CAMFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.CAMFolder = TempString
                            Delegate_Sub_Set_Text(txtCAMFolder, TempString)
                        End If
                    Catch ex As Exception
                        Globals.CAMFolder = "Unavailable"
                        Globals.ITCMInstalled = False
                    End Try

                    Try
                        TempString = Environment.GetEnvironmentVariable("CSAM_SOCKADAPTER", EnvironmentVariableTarget.Machine)
                        If Not TempString.Equals(Globals.SSAFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> CSAM folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SSAFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.SSAFolder = TempString
                            Delegate_Sub_Set_Text(txtSSAFolder, TempString)
                        End If
                    Catch ex As Exception
                        Globals.SSAFolder = "Unavailable"
                    End Try

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)
                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("InstallVersion").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.ITCMVersion) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Automation version updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ITCMVersion)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.ITCMVersion = TempString
                            Delegate_Sub_Set_Text(txtVersion, TempString)
                        End If

                        Try
                            TempString = ProductInfoKey.GetValue("InstallDir").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.CAFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> CA base folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.CAFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.CAFolder = TempString
                            'Delegate_Sub_Set_Text(txtCAFolder, TempString)  <-- No systeminfo field for this value
                        End If

                        Try
                            TempString = ProductInfoKey.GetValue("InstallDirProduct").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.DSMFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DSMFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.DSMFolder = TempString
                            Delegate_Sub_Set_Text(txtDSMFolder, TempString)
                        End If

                        Try
                            TempString = ProductInfoKey.GetValue("InstallShareDir").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.SharedCompFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto shared folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SharedCompFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.SharedCompFolder = TempString
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)
                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        FeatureKeys = ProductInfoKey.GetValueNames
                        For Each feature As String In FeatureKeys
                            Try
                                FeatureValue = ProductInfoKey.GetValue(feature)
                            Catch ex As Exception
                                FeatureValue = ""
                            End Try
                            If Not feature.Contains("-") Then ' Root feature has -
                                If ClientAutoFunction Is Nothing Then
                                    ClientAutoFunction = feature
                                Else
                                    ClientAutoFunction += ", " + feature
                                End If
                            End If

                            If feature.ToLower.Contains("asset management") And Not PluginList.Contains("Asset Management") Then
                                PluginList.Add("Asset Management")
                            ElseIf feature.ToLower.Contains("software delivery") And Not PluginList.Contains("Software Delivery") Then
                                PluginList.Add("Software Delivery")
                            ElseIf feature.ToLower.Contains("remote control") And Not PluginList.Contains("Remote Control") Then
                                PluginList.Add("Remote Control")
                            ElseIf feature.ToLower.Contains("data transport service") And Not PluginList.Contains("Data Transport") Then
                                PluginList.Add("Data Transport")
                            End If
                        Next

                        If Not WinOffline.Utility.IsArrayListEqual(PluginList, Globals.FeatureList) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Plugin list updated.")
                            Globals.FeatureList = PluginList
                            Delegate_Sub_Set_ListBox(lstPlugins, PluginList)
                        End If

                        If ClientAutoFunction.ToLower.Contains("manager") And ClientAutoFunction.ToLower.Contains("scalability") Then
                            ClientAutoFunction = "Domain Manager"
                        ElseIf ClientAutoFunction.ToLower.Contains("manager") Then
                            ClientAutoFunction = "Enterprise Manager"
                        ElseIf ClientAutoFunction.ToLower.Contains("scalability") And ClientAutoFunction.ToLower.Contains("explorer") Then
                            ClientAutoFunction = "Scalability Server + DSM Explorer"
                        ElseIf ClientAutoFunction.ToLower.Contains("scalability") Then
                            ClientAutoFunction = "Scalability Server"
                        ElseIf ClientAutoFunction.ToLower.Contains("agent") And ClientAutoFunction.ToLower.Contains("explorer") Then
                            ClientAutoFunction = "Agent + DSM Explorer"
                        ElseIf ClientAutoFunction.ToLower.Contains("agent") Then
                            ClientAutoFunction = "Agent"
                        End If


                        If Globals.ENCFunction IsNot Nothing Then
                            ClientAutoFunction += " + " + Globals.ENCFunction
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\hostUUID", False)

                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\hostUUID", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("HostUUID").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.HostUUID) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> HostUUID updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.HostUUID)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.HostUUID = TempString
                            Delegate_Sub_Set_Text(txtHostUUID, TempString)
                        End If
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\Software Delivery", False)
                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\Software Delivery", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("SDROOT").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.SDFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Software Delivery folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SDFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.SDFolder = TempString
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)

                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.DTSFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> DTS folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DTSFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.DTSFolder = TempString
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\EGC3.0N", False)

                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\EGC3.0N", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.EGCFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> EGC folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.EGCFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.EGCFolder = TempString
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)

                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("DisplayVersion").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.PMLAVersion) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> PMLA version updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.PMLAVersion)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.PMLAVersion = TempString
                        End If

                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.PMLAFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> PMLA folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.PMLAFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.PMLAFolder = TempString
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\DMPrimer", False)

                    If ProductInfoKey Is Nothing Then
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\DMPrimer", False)
                    End If

                    If ProductInfoKey IsNot Nothing Then
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        If Not TempString.Equals(Globals.DMPrimerFolder) Then
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> DMPrimer folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DMPrimerFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                            Globals.DMPrimerFolder = TempString
                        End If

                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing
                    End If

                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/usd/shared", "ARCHIVE")
                    TempString = TempString.Replace(vbCr, "").Replace(vbLf, "")

                    If TempString.Contains("\") And Not TempString.Equals(Globals.SDLibraryFolder) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Software Delivery library path updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SDLibraryFolder)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                        Globals.SDLibraryFolder = TempString
                    End If

                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/common/caf/plugins/encserver", "description")

                    If TempString.Contains("(") Then
                        TempString = TempString.Substring(TempString.IndexOf("(") + 1)

                        If TempString.ToLower.Contains("manager") And TempString.ToLower.Contains("server") And TempString.ToLower.Contains("router") Then
                            ENCFunction = "ENC Gateway (Manager, Server, Router)"
                        ElseIf TempString.ToLower.Contains("manager") And
                            TempString.ToLower.Contains("server") Then
                            ENCFunction = "ENC Gateway (Manager, Server)"
                        ElseIf TempString.ToLower.Contains("server") And
                            TempString.ToLower.Contains("router") Then
                            ENCFunction = "ENC Gateway (Server, Router)"
                        ElseIf TempString.ToLower.Contains("router") Then
                            ENCFunction = "ENC Gateway (Router)"
                        End If
                    Else
                        ENCFunction = Nothing
                    End If

                    If ENCFunction IsNot Nothing AndAlso Not ENCFunction.Equals(Globals.ENCFunction) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> ENC functionality updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ENCFunction)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + ENCFunction)
                        Globals.ENCFunction = ENCFunction
                        ClientAutoFunction += " + " + ENCFunction
                    End If

                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/common/enc/client", "serveraddress")

                    If TempString IsNot Nothing AndAlso Not TempString.Equals(Globals.ENCGatewayServer) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> ENC gateway server updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ENCGatewayServer)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                        Globals.ENCGatewayServer = TempString
                    End If

                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/common/enc/client", "servertcpport")

                    If TempString IsNot Nothing AndAlso Not TempString.Equals(Globals.ENCServerTCPPort) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> ENC server TCP port updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ENCServerTCPPort)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                        Globals.ENCServerTCPPort = TempString
                    End If

                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/agent/units/.", "currentmanageraddress")

                    If Not TempString.Equals(Globals.DomainManager) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Current manager address updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DomainManager)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                        Globals.DomainManager = TempString
                        Delegate_Sub_Set_Text(txtDomainMgr, TempString)
                    End If

                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/agent/solutions/generic", "serveraddress").Replace(vbCr, "").Replace(vbLf, "")

                    If Not TempString.Equals(Globals.ScalabilityServer) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Scalability server address updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ScalabilityServer)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)
                        Globals.ScalabilityServer = TempString
                        Delegate_Sub_Set_Text(txtScalabilityServer, TempString)
                    End If

                    If Not ClientAutoFunction.Equals(Globals.ITCMFunction) Then
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto functionality updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ITCMFunction)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + ClientAutoFunction)
                        Globals.ITCMFunction = ClientAutoFunction
                        Delegate_Sub_Set_Text(txtFunction, ClientAutoFunction)
                    End If
                End If

                For Each NetAddr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(Globals.HostName).AddressList
                    If NetAddr.IsIPv6LinkLocal Or NetAddr.IsIPv6Multicast Or NetAddr.IsIPv6SiteLocal Then
                        IPv6List.Add(NetAddr.ToString)
                    Else
                        IPv4List.Add(NetAddr.ToString)
                    End If
                Next

                If Not WinOffline.Utility.IsArrayListEqual(IPv4List, MasterIPv4list) Or
                    Not WinOffline.Utility.IsArrayListEqual(IPv6List, MasterIPv6list) Then
                    Dim CombinedList As New ArrayList
                    For Each ListItem As String In IPv4List
                        CombinedList.Add(ListItem.ToString)
                    Next
                    For Each ListItem As String In IPv6List
                        CombinedList.Add(ListItem.ToString)
                    Next

                    Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Network address list updated.")

                    MasterIPv4list = IPv4List
                    MasterIPv6list = IPv6List
                    Delegate_Sub_Set_ListBox(lstNetAddr, CombinedList)
                End If
            End While
        Catch ex As Exception
            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> " + ex.Message)
            Delegate_Sub_Append_Text(rtbDebug, ex.StackTrace)
        End Try

    End Sub

    Private Sub lstNetAddr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstNetAddr.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.C Then
            Dim copy_buffer As New System.Text.StringBuilder
            For Each item As Object In lstNetAddr.SelectedItems
                copy_buffer.AppendLine(item.ToString)
            Next
            If copy_buffer.Length > 0 Then
                Clipboard.SetText(copy_buffer.ToString)
            End If
        End If
    End Sub

    Private Sub btnBrowseDSM_Click(sender As Object, e As EventArgs) Handles btnBrowseDSM.Click
        If System.IO.Directory.Exists(txtDSMFolder.Text) Then
            Shell("explorer.exe " + txtDSMFolder.Text, AppWinStyle.NormalFocus, False)
        Else
            MsgBox("Folder does not exist.", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub btnBrowseCAM_Click(sender As Object, e As EventArgs) Handles btnBrowseCAM.Click
        If System.IO.Directory.Exists(txtCAMFolder.Text) Then
            Shell("explorer.exe " + txtCAMFolder.Text, AppWinStyle.NormalFocus, False)
        Else
            MsgBox("Folder does not exist.", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub btnBrowseSSA_Click(sender As Object, e As EventArgs) Handles btnBrowseSSA.Click
        If System.IO.Directory.Exists(txtSSAFolder.Text) Then
            Shell("explorer.exe " + txtSSAFolder.Text, AppWinStyle.NormalFocus, False)
        Else
            MsgBox("Folder does not exist.", MsgBoxStyle.Exclamation)
        End If
    End Sub

End Class
Imports System.Management
Imports System.Windows.Forms
Imports System.Threading

Partial Public Class WinOfflineUI

    Private Shared MasterIPv4list As New ArrayList
    Private Shared MasterIPv6list As New ArrayList

    Private Sub InitSystemInfo()

        ' Set system info properties
        txtHostname.Text = Globals.HostName
        txtPlatform.Text = My.Computer.Info.OSFullName + Environment.OSVersion.ServicePack

        ' Set client auto properties
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

        ' Start SystemInfo thread
        SystemInfoUpdateThread = New Thread(AddressOf SystemInfoWorker)
        SystemInfoUpdateThread.Start()

    End Sub

    Private Sub SystemInfoWorker()

        ' Local variables
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

        ' Encapsualte system info worker
        Try

            ' System Info will refresh for duration of WinOfflineUI lifecycle
            While (Not TerminateSignal)

                ' Reset loop counter
                LoopCounter = 0

                ' Rest for a short, but reasonable interval
                While LoopCounter < 799

                    ' Check for termination signal
                    If TerminateSignal Then Return

                    ' Increment loop counter
                    LoopCounter += 1

                    ' Rest the thread
                    Thread.Sleep(Globals.THREAD_REST_INTERVAL)

                End While

                ' Reset local variables
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

                ' Check for hostname change
                If Not My.Computer.Name.Equals(Globals.HostName) Then

                    ' Write debug
                    Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Hostname updated.")
                    Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.HostName)
                    Delegate_Sub_Append_Text(rtbDebug, "New value: " + My.Computer.Name)

                    ' Update information
                    Globals.HostName = My.Computer.Name
                    Delegate_Sub_Set_Text(txtHostname, Globals.HostName)

                End If

                ' Verify ITCM is present on the system
                If Not WinOffline.Utility.IsITCMInstalled Then

                    ' Write debug
                    Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto is not installed.")

                    ' Update global flag
                    Globals.ITCMInstalled = False

                    ' Update info
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

                    ' Make sure removal tool is not currently running
                    If RemovalToolThread Is Nothing Then

                        ' Set global flag
                        Globals.ITCMInstalled = True

                        ' Enable removal controls
                        Delegate_Sub_Enable_Control(btnRemoveITCM, True)
                        Delegate_Sub_Enable_Control(rbnRemoveITCM, True)
                        Delegate_Sub_Enable_Control(chkRetainHostUUID, True)
                        Delegate_Sub_Enable_Control(rbnUninstallITCM, True)

                    Else

                        ' Set global flag
                        Globals.ITCMInstalled = False

                    End If

                End If

                ' Refresh ITCM properties, if installed
                If Globals.ITCMInstalled Then

                    ' Check for CAM environment change
                    Try

                        ' Read value
                        TempString = Environment.GetEnvironmentVariable("CAI_MSQ", EnvironmentVariableTarget.Machine)

                        ' Check for change in value
                        If Not TempString.Equals(Globals.CAMFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> CAM folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.CAMFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.CAMFolder = TempString
                            Delegate_Sub_Set_Text(txtCAMFolder, TempString)

                        End If

                    Catch ex As Exception

                        ' Update information
                        Globals.CAMFolder = "Unavailable"
                        Globals.ITCMInstalled = False

                    End Try

                    ' Check for SSA environment change
                    Try

                        ' Read value
                        TempString = Environment.GetEnvironmentVariable("CSAM_SOCKADAPTER", EnvironmentVariableTarget.Machine)

                        ' Check for change in value
                        If Not TempString.Equals(Globals.SSAFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> CSAM folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SSAFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.SSAFolder = TempString
                            Delegate_Sub_Set_Text(txtSSAFolder, TempString)

                        End If

                    Catch ex As Exception

                        ' Update information
                        Globals.SSAFolder = "Unavailable"

                    End Try

                    ' Read 64-bit registry -- Unicenter ITRM
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)

                    ' Check 64-bit registry -- Unicenter ITRM
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry -- Unicenter ITRM
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get installed version
                        Try
                            TempString = ProductInfoKey.GetValue("InstallVersion").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.ITCMVersion) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Automation version updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ITCMVersion)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.ITCMVersion = TempString
                            Delegate_Sub_Set_Text(txtVersion, TempString)

                        End If

                        ' Get ca base directory
                        Try
                            TempString = ProductInfoKey.GetValue("InstallDir").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.CAFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> CA base folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.CAFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.CAFolder = TempString
                            'Delegate_Sub_Set_Text(txtDSMFolder, TempString)  <-- No systeminfo field for this value

                        End If

                        ' Get itcm directory
                        Try
                            TempString = ProductInfoKey.GetValue("InstallDirProduct").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.DSMFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DSMFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.DSMFolder = TempString
                            Delegate_Sub_Set_Text(txtDSMFolder, TempString)

                        End If

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("InstallShareDir").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.SharedCompFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto shared folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SharedCompFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.SharedCompFolder = TempString

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry -- InstalledFeatures
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)

                    ' Check 64-bit registry -- InstalledFeatures
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry -- InstalledFeatures
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\InstalledFeatures", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get list of installed features
                        FeatureKeys = ProductInfoKey.GetValueNames

                        ' Iterare each feature
                        For Each feature As String In FeatureKeys

                            Try

                                ' Get feature version (Registry key value)
                                FeatureValue = ProductInfoKey.GetValue(feature)

                            Catch ex As Exception

                                ' Stub empty value
                                FeatureValue = ""

                            End Try

                            ' Check for root feature
                            If Not feature.Contains("-") Then

                                ' Check if any feature is set
                                If ClientAutoFunction Is Nothing Then

                                    ' Assign first feature
                                    ClientAutoFunction = feature

                                Else

                                    ' Append new feature
                                    ClientAutoFunction += ", " + feature

                                End If

                            End If

                            ' Update local plugin list
                            If feature.ToLower.Contains("asset management") And
                                Not PluginList.Contains("Asset Management") Then

                                ' Update list
                                PluginList.Add("Asset Management")

                            ElseIf feature.ToLower.Contains("software delivery") And
                                Not PluginList.Contains("Software Delivery") Then

                                ' Update list
                                PluginList.Add("Software Delivery")

                            ElseIf feature.ToLower.Contains("remote control") And
                                Not PluginList.Contains("Remote Control") Then

                                ' Update list
                                PluginList.Add("Remote Control")

                            ElseIf feature.ToLower.Contains("data transport service") And
                                Not PluginList.Contains("Data Transport") Then

                                ' Update list
                                PluginList.Add("Data Transport")

                            End If

                        Next

                        ' Check for a change in value
                        If Not WinOffline.Utility.IsArrayListEqual(PluginList, Globals.FeatureList) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Plugin list updated.")

                            ' Update information
                            Globals.FeatureList = PluginList
                            Delegate_Sub_Set_ListBox(lstPlugins, PluginList)

                        End If

                        ' Simplify feature list
                        If ClientAutoFunction.ToLower.Contains("manager") And
                            ClientAutoFunction.ToLower.Contains("scalability") Then

                            ' Update function
                            ClientAutoFunction = "Domain Manager"

                        ElseIf ClientAutoFunction.ToLower.Contains("manager") Then

                            ' Update function
                            ClientAutoFunction = "Enterprise Manager"

                        ElseIf ClientAutoFunction.ToLower.Contains("scalability") And
                            ClientAutoFunction.ToLower.Contains("explorer") Then

                            ' Update function
                            ClientAutoFunction = "Scalability Server + DSM Explorer"

                        ElseIf ClientAutoFunction.ToLower.Contains("scalability") Then

                            ' Update function
                            ClientAutoFunction = "Scalability Server"

                        ElseIf ClientAutoFunction.ToLower.Contains("agent") And
                            ClientAutoFunction.ToLower.Contains("explorer") Then

                            ' Update function
                            ClientAutoFunction = "Agent + DSM Explorer"

                        ElseIf ClientAutoFunction.ToLower.Contains("agent") Then

                            ' Update function
                            ClientAutoFunction = "Agent"

                        End If

                        ' Check for ENC functionality
                        If Globals.ENCFunction IsNot Nothing Then

                            ' Append current ENC functionality
                            ClientAutoFunction += " + " + Globals.ENCFunction

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\hostUUID", False)

                    ' Check 64-bit registry
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\hostUUID", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("HostUUID").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.HostUUID) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> HostUUID updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.HostUUID)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.HostUUID = TempString
                            Delegate_Sub_Set_Text(txtHostUUID, TempString)

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM\Software Delivery", False)

                    ' Check 64-bit registry
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM\Software Delivery", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("SDROOT").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.SDFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Software Delivery folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SDFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.SDFolder = TempString

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)

                    ' Check 64-bit registry
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter Data Transport Service\CURRENTVERSION", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.DTSFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> DTS folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DTSFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.DTSFolder = TempString

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\EGC3.0N", False)

                    ' Check 64-bit registry
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\EGC3.0N", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for a change in value
                        If Not TempString.Equals(Globals.EGCFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> EGC folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.EGCFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.EGCFolder = TempString

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)

                    ' Check 64-bit registry
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\SC\CA Systems Performance LiteAgent", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("DisplayVersion").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for change in value
                        If Not TempString.Equals(Globals.PMLAVersion) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> PMLA version updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.PMLAVersion)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Udpate information
                            Globals.PMLAVersion = TempString

                        End If

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for change in value
                        If Not TempString.Equals(Globals.PMLAFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> PMLA folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.PMLAFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.PMLAFolder = TempString

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Read 64-bit registry
                    ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\DMPrimer", False)

                    ' Check 64-bit registry
                    If ProductInfoKey Is Nothing Then

                        ' Read 32-bit registry
                        ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\DMPrimer", False)

                    End If

                    ' Update if available
                    If ProductInfoKey IsNot Nothing Then

                        ' Get value
                        Try
                            TempString = ProductInfoKey.GetValue("InstallPath").ToString()
                        Catch ex As Exception
                            TempString = "Not found"
                        End Try

                        ' Check for change in value
                        If Not TempString.Equals(Globals.DMPrimerFolder) Then

                            ' Write debug
                            Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> DMPrimer folder updated.")
                            Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DMPrimerFolder)
                            Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                            ' Update information
                            Globals.DMPrimerFolder = TempString

                        End If

                        ' Close registry key
                        ProductInfoKey.Close()
                        ProductInfoKey = Nothing

                    End If

                    ' Get value
                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/usd/shared", "ARCHIVE")

                    ' Remove carriage return
                    TempString = TempString.Replace(vbCr, "").Replace(vbLf, "")

                    ' Check for change in value
                    If TempString.Contains("\") And Not TempString.Equals(Globals.SDLibraryFolder) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Software Delivery library path updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.SDLibraryFolder)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                        ' Update information
                        Globals.SDLibraryFolder = TempString

                    End If

                    ' Get value
                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/common/caf/plugins/encserver", "description")

                    ' Check for gateway functionality
                    If TempString.Contains("(") Then

                        ' Filter to gateway functions
                        TempString = TempString.Substring(TempString.IndexOf("(") + 1)

                        ' Check functions
                        If TempString.ToLower.Contains("manager") And
                            TempString.ToLower.Contains("server") And
                            TempString.ToLower.Contains("router") Then

                            ' Set value
                            ENCFunction = "ENC Gateway (Manager, Server, Router)"

                        ElseIf TempString.ToLower.Contains("manager") And
                            TempString.ToLower.Contains("server") Then

                            ' Set value
                            ENCFunction = "ENC Gateway (Manager, Server)"

                        ElseIf TempString.ToLower.Contains("server") And
                            TempString.ToLower.Contains("router") Then

                            ' Set value
                            ENCFunction = "ENC Gateway (Server, Router)"

                        ElseIf TempString.ToLower.Contains("router") Then

                            ' Set value
                            ENCFunction = "ENC Gateway (Router)"

                        End If

                    Else

                        ' Set value
                        ENCFunction = Nothing

                    End If

                    ' Check for change in value
                    If ENCFunction IsNot Nothing AndAlso Not ENCFunction.Equals(Globals.ENCFunction) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> ENC functionality updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ENCFunction)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + ENCFunction)

                        ' Update information
                        Globals.ENCFunction = ENCFunction

                        ' Append to Client Auto function
                        ClientAutoFunction += " + " + ENCFunction

                    End If

                    ' Get value
                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/common/enc/client", "serveraddress")

                    ' Check for change in value
                    If TempString IsNot Nothing AndAlso Not TempString.Equals(Globals.ENCGatewayServer) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> ENC gateway server updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ENCGatewayServer)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                        ' Update information
                        Globals.ENCGatewayServer = TempString

                    End If

                    ' Get value
                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/common/enc/client", "servertcpport")

                    ' Check for change in value
                    If TempString IsNot Nothing AndAlso Not TempString.Equals(Globals.ENCServerTCPPort) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> ENC server TCP port updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ENCServerTCPPort)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                        ' Update information
                        Globals.ENCServerTCPPort = TempString

                    End If

                    ' Get value
                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/agent/units/.", "currentmanageraddress")

                    ' Check for change in value
                    If Not TempString.Equals(Globals.DomainManager) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Current manager address updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.DomainManager)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                        ' Update information
                        Globals.DomainManager = TempString
                        Delegate_Sub_Set_Text(txtDomainMgr, TempString)

                    End If

                    ' Get value
                    TempString = WinOffline.ComstoreAPI.GetParameterValue("itrm/agent/solutions/generic", "serveraddress").Replace(vbCr, "").Replace(vbLf, "")

                    ' Check for change in value
                    If Not TempString.Equals(Globals.ScalabilityServer) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Scalability server address updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ScalabilityServer)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + TempString)

                        ' Update information
                        Globals.ScalabilityServer = TempString
                        Delegate_Sub_Set_Text(txtScalabilityServer, TempString)

                    End If

                    ' Check for a change in value
                    If Not ClientAutoFunction.Equals(Globals.ITCMFunction) Then

                        ' Write debug
                        Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Client Auto functionality updated.")
                        Delegate_Sub_Append_Text(rtbDebug, "Old value: " + Globals.ITCMFunction)
                        Delegate_Sub_Append_Text(rtbDebug, "New value: " + ClientAutoFunction)

                        ' Update information
                        Globals.ITCMFunction = ClientAutoFunction
                        Delegate_Sub_Set_Text(txtFunction, ClientAutoFunction)

                    End If

                End If

                ' Get values
                For Each NetAddr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(Globals.HostName).AddressList
                    If NetAddr.IsIPv6LinkLocal Or NetAddr.IsIPv6Multicast Or NetAddr.IsIPv6SiteLocal Then
                        IPv6List.Add(NetAddr.ToString)
                    Else
                        IPv4List.Add(NetAddr.ToString)
                    End If
                Next

                ' Check for change in values
                If Not WinOffline.Utility.IsArrayListEqual(IPv4List, MasterIPv4list) Or
                    Not WinOffline.Utility.IsArrayListEqual(IPv6List, MasterIPv6list) Then

                    ' Combine lists
                    Dim CombinedList As New ArrayList
                    For Each ListItem As String In IPv4List
                        CombinedList.Add(ListItem.ToString)
                    Next
                    For Each ListItem As String In IPv6List
                        CombinedList.Add(ListItem.ToString)
                    Next

                    ' Write debug
                    Delegate_Sub_Append_Text(rtbDebug, "SystemInfoThread --> Network address list updated.")

                    ' Update information
                    MasterIPv4list = IPv4List
                    MasterIPv6list = IPv6List
                    Delegate_Sub_Set_ListBox(lstNetAddr, CombinedList)

                End If

            End While

        Catch ex As Exception

            ' Write debug
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
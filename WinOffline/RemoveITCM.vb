Partial Public Class WinOffline

    Public Shared Function RemoveITCM(ByVal CallStack As String) As Integer

        ' Local variables
        Dim ProductInfoKey As Microsoft.Win32.RegistryKey = Nothing
        Dim ExecutionString As String
        Dim ArgumentString As String
        Dim OfflineProcessStartInfo As ProcessStartInfo
        Dim ReturnBoolean As Boolean = False
        Dim ReturnValue As Integer = 0
        Dim SvcController As ServiceProcess.ServiceController = Nothing
        Dim CAFolderPersistentVar As String = Globals.CAFolder
        Dim TestKey As Microsoft.Win32.RegistryKey = Nothing
        Dim TestKeyValue As String = Nothing
        Dim CleanPathVar As String = Nothing
        Dim FileList As String()
        Dim strFile As String
        Dim hexString As String
        Dim Info As New ProcessStartInfo()

        ' Update call stack
        CallStack += "RemoveITCM|"

        ' Write debug
        Logger.SetCurrentTask("Removing ITCM..")
        Logger.WriteDebug(CallStack, "Removing ITCM..")

        ' *****************************
        ' - Reconcile CA base folder (if unavailable).
        ' *****************************

        ' Check if CA base folder is empty
        If CAFolderPersistentVar Is Nothing Then

            ' Check for UnicenterITRM (64-bit system)
            ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates\Unicenter ITRM", False)

            ' Check for UnicenterITRM (32-bit system)
            If ProductInfoKey Is Nothing Then ProductInfoKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates\Unicenter ITRM", False)

            ' If UnicenterITRM is available
            If ProductInfoKey IsNot Nothing Then

                ' Encapsulate
                Try

                    ' Try reading shared component folder
                    Globals.SharedCompFolder = ProductInfoKey.GetValue("InstallShareDir").ToString()

                    ' Check SC base folder
                    If Globals.SharedCompFolder IsNot Nothing Then

                        ' Check if ends in \
                        If Globals.SharedCompFolder.EndsWith("\") Then

                            ' Trim twice
                            CAFolderPersistentVar = Globals.SharedCompFolder.Substring(0, Globals.SharedCompFolder.LastIndexOf("\") - 1)
                            CAFolderPersistentVar = CAFolderPersistentVar.Substring(0, CAFolderPersistentVar.LastIndexOf("\"))
                            If Not CAFolderPersistentVar.EndsWith("\") Then CAFolderPersistentVar = CAFolderPersistentVar + "\"

                        Else

                            ' Trim once
                            CAFolderPersistentVar = Globals.SharedCompFolder.Substring(0, Globals.SharedCompFolder.LastIndexOf("\"))
                            If Not CAFolderPersistentVar.EndsWith("\") Then CAFolderPersistentVar = CAFolderPersistentVar + "\"

                        End If

                    Else

                        ' Assume default folder location (in case there's anything left there)
                        CAFolderPersistentVar = Environment.GetEnvironmentVariable("SystemDrive") + "\Program Files (x86)\CA\"

                    End If

                Finally

                    ' Close registry key
                    ProductInfoKey.Close()

                End Try

            End If

        End If

        ' *****************************
        ' - Check execution mode.
        ' *****************************

        ' Entry point check
        If Globals.ParentProcessTree.Contains("sd_jexec") Then

            ' *****************************
            ' - Copy WinOffline to temp.
            ' *****************************

            ' Copy WinOffline to temp
            Try

                ' Check if WinOffline already exists
                If Not System.IO.File.Exists(Globals.WindowsTemp + "\" + Globals.ProcessShortName) Then

                    ' Write debug
                    Logger.WriteDebug(CallStack, "Copy File: " + Globals.ProcessName)
                    Logger.WriteDebug(CallStack, "To: " + Globals.WindowsTemp + "\" + Globals.ProcessShortName)

                    ' Copy to temp
                    System.IO.File.Copy(Globals.ProcessName, Globals.WindowsTemp + "\" + Globals.ProcessShortName, True)

                End If

            Catch ex As Exception

                ' Write debug
                Logger.WriteDebug(CallStack, "Error: Exception caught copying " + Globals.ProcessFriendlyName + " to temporary directory.")
                Logger.WriteDebug(ex.Message)

                ' Return
                Return 1

            End Try

            ' *****************************
            ' - Create detached offline process.
            ' *****************************

            ' Build stageII execution string
            ExecutionString = Globals.WindowsTemp + "\" + Globals.ProcessShortName
            ArgumentString = ""

            ' Build stageII argument string
            For Each arg As String In Globals.CommandLineArgs

                ' Add the argument to the list
                ArgumentString += " " + arg

            Next

            ' Create process based on process identity
            If Globals.RunningAsSystemIdentity Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Detached process: " + ExecutionString + " " + ArgumentString)

                ' Create detached process For stage II execution
                OfflineProcessStartInfo = New ProcessStartInfo(ExecutionString)
                OfflineProcessStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(ExecutionString)
                OfflineProcessStartInfo.Arguments = ArgumentString

                ' Start detached process
                Process.Start(OfflineProcessStartInfo)

            Else

                ' Call the launch pad
                LaunchPad(CallStack, "system", Globals.WindowsTemp + "\" + Globals.ProcessShortName, Globals.WindowsTemp + "\", ArgumentString)

            End If

            ' Return
            Return 0

        End If

        ' *****************************
        ' - Disable ITCM services.
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "DisableService|"

        ' Check for full uninstall switch
        If Globals.RemoveITCM Then

            ' Disable CAF service
            ReturnBoolean = Utility.ChangeServiceMode("caf", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "CAF service: Disabled")

            ' Disable hmAgent service
            ReturnBoolean = Utility.ChangeServiceMode("hmAgent", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "Health Monitoring Agent service: Disabled")

            ' Disable CA message queuing service
            ReturnBoolean = Utility.ChangeServiceMode("CA-MessageQueuing", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "CA Message Queuing service: Disabled")

            ' Disable CA connection broker service
            ReturnBoolean = Utility.ChangeServiceMode("CA-SAM-Pmux", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "CA Connection Broker service: Disabled")

            ' Disable CA performance lite agent service
            ReturnBoolean = Utility.ChangeServiceMode("CASPLiteAgent", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "CA Performance Lite Agent service: Disabled")

        Else

            ' Disable CAF service
            ReturnBoolean = Utility.ChangeServiceMode("caf", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "CAF service: Disabled")

            ' Disable hmAgent service
            ReturnBoolean = Utility.ChangeServiceMode("hmAgent", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "Health Monitoring Agent service: Disabled")

            ' Disable CA performance lite agent service
            ReturnBoolean = Utility.ChangeServiceMode("CASPLiteAgent", "Disabled")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "CA Performance Lite Agent service: Disabled")

        End If

        ' *****************************
        ' - Kill ITCM processes.
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "KillProcess|"

        ' Kill processes
        ReturnBoolean = Utility.KillProcess("CAF")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "CAF.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ACServer")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ACServer.exe: Killed")
        ReturnBoolean = Utility.KillProcess("AlertCollector")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "AlertCollector.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amAdvInvNT")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amAdvInvNT.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amagentsvc")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amagentsvc.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amappw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amappw32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amBridge")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amBridge.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amDCSPlugin")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amDCSPlugin.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amdifw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amdifw32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amicimw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amicimw32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amilicw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amilicw32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amiscap")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amiscap.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amisww32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amisww32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amLrss")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amLrss.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amm2iw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amm2iw32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amms")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amms.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amObjectManager")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amObjectManager.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amRSS")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amRSS.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amservice")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amservice.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amsoftscan")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amsoftscan.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amss")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amss.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amSvpCmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amSvpCmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amswmagt")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amswmagt.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amswmspwnt")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amswmspwnt.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amswsigscan")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amswsigscan.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amtplw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amtplw32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amUpgradeUtility")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amUpgradeUtility.exe: Killed")
        ReturnBoolean = Utility.KillProcess("amUsersnt")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "amUsersnt.exe: Killed")
        ReturnBoolean = Utility.KillProcess("am_sms_ex")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "am_sms_ex.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cacertutil")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cacertutil.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cadsmcmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cadsmcmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("CAITCMAMT")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "CAITCMAMT.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccnfac")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccnfac.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccnfAgent")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccnfAgent.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccnfcmda")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccnfcmda.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccnfregdb")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccnfregdb.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccsmactd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccsmactd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccsmagtd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccsmagtd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccsmapid")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccsmapid.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ccsmsvrd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ccsmsvrd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfbasichwwnt")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfbasichwwnt.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfCafDialog")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfCafDialog.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfFTPlugin")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfFTPlugin.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfMsgBox")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfMsgBox.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfnotsrvd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfnotsrvd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfPluginHelper")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfPluginHelper.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfprocesslog")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfprocesslog.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfProcessManager")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfProcessManager.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfsmsmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfsmsmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfSysTray")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfSysTray.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cftrace")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cftrace.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cfUsrNtf")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cfUsrNtf.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ciIVPcheck")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ciIVPcheck.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cmContDiscover")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cmContDiscover.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cmDirEngJob")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cmDirEngJob.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cmdirmgr")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cmdirmgr.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cmEngine")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cmEngine.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cmObjectManager")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cmObjectManager.exe: Killed")
        ReturnBoolean = Utility.KillProcess("CmpEdit")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "CmpEdit.exe: Killed")
        ReturnBoolean = Utility.KillProcess("Connector")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "Connector.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ContentUtility")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ContentUtility.exe: Killed")
        ReturnBoolean = Utility.KillProcess("copy144")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "copy144.exe: Killed")
        ReturnBoolean = Utility.KillProcess("CreateBTimages")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "CreateBTimages.exe: Killed")
        ReturnBoolean = Utility.KillProcess("CreateOSImage")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "CreateOSImage.exe: Killed")
        ReturnBoolean = Utility.KillProcess("cserver")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "cserver.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dbinfo")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dbinfo.exe: Killed")
        ReturnBoolean = Utility.KillProcess("Deploywrapper")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "Deploywrapper.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dm_primer")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dm_primer.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dmboot")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dmboot.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dmdeploy")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dmdeploy.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dmscript")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dmscript.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dmsedit")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dmsedit.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dmsweep")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dmsweep.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dmzip")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dmzip.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dsmdiag")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dsmdiag.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dsmgui")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dsmgui.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dsmproperties")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dsmproperties.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dsmreporter")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dsmreporter.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dsmVer")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dsmVer.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtacli")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtacli.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtdbinst")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtdbinst.exe: Killed")
        ReturnBoolean = Utility.KillProcess("DtsAdmin")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "DtsAdmin.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtsbpv")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtsbpv.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtscli")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtscli.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtsorie")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtsorie.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtsping")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtsping.exe: Killed")
        ReturnBoolean = Utility.KillProcess("dtver")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "dtver.exe: Killed")
        ReturnBoolean = Utility.KillProcess("egc30n")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "egc30n.exe: Killed")
        ReturnBoolean = Utility.KillProcess("encClient")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "encClient.exe: Killed")
        ReturnBoolean = Utility.KillProcess("encUtilCmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "encUtilCmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("engineinstallerhelper")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "engineinstallerhelper.exe: Killed")
        ReturnBoolean = Utility.KillProcess("enum64process64")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "enum64process64.exe: Killed")
        ReturnBoolean = Utility.KillProcess("getinv")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "getinv.exe: Killed")
        ReturnBoolean = Utility.KillProcess("GetLink")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "GetLink.exe: Killed")
        ReturnBoolean = Utility.KillProcess("getparam")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "getparam.exe: Killed")
        ReturnBoolean = Utility.KillProcess("gui_am_scapcfg")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "gui_am_scapcfg.exe: Killed")
        ReturnBoolean = Utility.KillProcess("gui_am_tpledit")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "gui_am_tpledit.exe: Killed")
        ReturnBoolean = Utility.KillProcess("gui_am_wmicfg")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "gui_am_wmicfg.exe: Killed")
        ReturnBoolean = Utility.KillProcess("gui_rcLaunch")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "gui_rcLaunch.exe: Killed")
        ReturnBoolean = Utility.KillProcess("hmAgent")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "hmAgent.exe: Killed")
        ReturnBoolean = Utility.KillProcess("hmAgentRepair")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "hmAgentRepair.exe: Killed")
        ReturnBoolean = Utility.KillProcess("hmAlertOPFormatter")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "hmAlertOPFormatter.exe: Killed")
        ReturnBoolean = Utility.KillProcess("HMWSAfterCopy")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "HMWSAfterCopy.exe: Killed")
        ReturnBoolean = Utility.KillProcess("icon")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "icon.exe: Killed")
        ReturnBoolean = Utility.KillProcess("intellisigcmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "intellisigcmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("InvSign")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "InvSign.exe: Killed")
        ReturnBoolean = Utility.KillProcess("MDMDataIn")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "MDMDataIn.exe: Killed")
        ReturnBoolean = Utility.KillProcess("MsiDBPatch")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "MsiDBPatch.exe: Killed")
        ReturnBoolean = Utility.KillProcess("osimfipsutil")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "osimfipsutil.exe: Killed")
        ReturnBoolean = Utility.KillProcess("osimmove")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "osimmove.exe: Killed")
        ReturnBoolean = Utility.KillProcess("preR11Rem")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "preR11Rem.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ptagent")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ptagent.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ptsvcstart")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ptsvcstart.exe: Killed")
        ReturnBoolean = Utility.KillProcess("ptupdate")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "ptupdate.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcAdmin")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcAdmin.exe: Killed")
        ReturnBoolean = Utility.KillProcess("RCCheck")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "RCCheck.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcHost")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcHost.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcManagerR11Migration")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcManagerR11Migration.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcMansrv")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcMansrv.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcMigrate")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcMigrate.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcReplayExport")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcReplayExport.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcServer")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcServer.exe: Killed")
        ReturnBoolean = Utility.KillProcess("rcUtilCmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "rcUtilCmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("RegisterBTimages")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "RegisterBTimages.exe: Killed")
        ReturnBoolean = Utility.KillProcess("RegisterOSImage")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "RegisterOSImage.exe: Killed")
        ReturnBoolean = Utility.KillProcess("regprod")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "regprod.exe: Killed")
        ReturnBoolean = Utility.KillProcess("runquiet")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "runquiet.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdbsswitch")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdbsswitch.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdcat")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdcat.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdcnfmig")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdcnfmig.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SDLaunch")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SDLaunch.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdmgrmig")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdmgrmig.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdmpcfilerest")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdmpcfilerest.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdmpcworker")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdmpcworker.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sdrpmextract")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sdrpmextract.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_acmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_acmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_agtftplugin")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_agtftplugin.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_ahdcmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_ahdcmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_apisrv")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_apisrv.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_dialog_m")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_dialog_m.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_dtaflt")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_dtaflt.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_jexec")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_jexec.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_mgr_ft")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_mgr_ft.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_msiexe")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_msiexe.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_pilot")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_pilot.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_registerproduct")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_registerproduct.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_server")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_server.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_setcnf")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_setcnf.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_sscmd")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_sscmd.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_swdet")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_swdet.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SD_Taskm")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SD_Taskm.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_wince")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_wince.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sd_zip")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sd_zip.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SendMessageProcess")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SendMessageProcess.exe: Killed")
        ReturnBoolean = Utility.KillProcess("smsecure")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "smsecure.exe: Killed")
        ReturnBoolean = Utility.KillProcess("smsetcnf")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "smsetcnf.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SNMPTRAP")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SNMPTRAP.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SSHDmBoot")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SSHDmBoot.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sxpeng32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sxpeng32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sxplog32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sxplog32.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SxpPkg")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SxpPkg.exe: Killed")
        ReturnBoolean = Utility.KillProcess("SxpPkgLo")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "SxpPkgLo.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sxpstub")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sxpstub.exe: Killed")
        ReturnBoolean = Utility.KillProcess("sxpuser")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "sxpuser.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdoba")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdoba.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdta")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdta.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdtmg")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdtmg.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdtnos")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdtnos.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdts")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdts.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdtsad")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdtsad.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdtsos")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdtsos.exe: Killed")
        ReturnBoolean = Utility.KillProcess("tngdttos")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "tngdttos.exe: Killed")
        ReturnBoolean = Utility.KillProcess("umsynw32")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "umsynw32.exe: Killed")
        ReturnBoolean = Utility.KillProcessByPath("javaw.exe", "\CIC\jre\")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "javaw.exe (CIC): Killed")
        ReturnBoolean = Utility.KillProcessByCommandLine("iexplore.exe", "Embedding")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "iexplore.exe (DSM Explorer): Killed")
        ReturnBoolean = Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService_HM")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "w3wp.exe (DSM_WebService_HM): Killed")
        ReturnBoolean = Utility.KillProcessByCommandLine("w3wp.exe", "DSM_WebService")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "w3wp.exe (DSM_WebService): Killed")
        ReturnBoolean = Utility.KillProcessByCommandLine("w3wp.exe", "ITCM_Application_Pool")
        If ReturnBoolean Then Logger.WriteDebug(CallStack, "w3wp.exe (ITCM_Application_Pool): Killed")

        ' Check for full uninstall switch
        If Globals.RemoveITCM Then

            ' Kill shared services
            ReturnBoolean = Utility.KillProcess("cam")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "cam.exe: Killed")
            ReturnBoolean = Utility.KillProcess("csampmux")
            If ReturnBoolean Then Logger.WriteDebug(CallStack, "csampmux.exe: Killed")

        End If

        ' *****************************
        ' - Remove RC agent driver.
        ' *****************************

        ' Encapsulate mirror driver removal
        Try

            ' Check environment architecture
            If Utility.Is64BitOperatingSystem Then

                ' Check if RC mirror driver installer exists
                If System.IO.File.Exists(Globals.DSMFolder + "bin\AMD64\rcMirrorInstall.exe") Then

                    ' Update call stack
                    CallStack += CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveRC|"

                    ' Uninstall RC smart card reader
                    Logger.WriteDebug(CallStack, "Uninstalling RC smart card driver..")
                    ReturnValue = Utility.RunCommand(Globals.DSMFolder + "bin\AMD64\rcMirrorInstall.exe", "-scremove")
                    Logger.WriteDebug(CallStack, "Return code: " + ReturnValue.ToString)

                    ' Uninstall RC mirror driver
                    Logger.WriteDebug(CallStack, "Uninstalling RC mirror driver..")
                    ReturnValue = Utility.RunCommand(Globals.DSMFolder + "bin\AMD64\rcMirrorInstall.exe", "-remove")
                    Logger.WriteDebug(CallStack, "Return code: " + ReturnValue.ToString)

                End If

            Else

                ' Check if RC mirror driver installer exists
                If System.IO.File.Exists(Globals.DSMFolder + "bin\x86\rcMirrorInstall.exe") Then

                    ' Update call stack
                    CallStack += CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveRC|"

                    ' Uninstall RC smart card reader
                    Logger.WriteDebug(CallStack, "Uninstalling RC smart card driver..")
                    ReturnValue = Utility.RunCommand(Globals.DSMFolder + "bin\x86\rcMirrorInstall.exe", "-scremove")
                    Logger.WriteDebug(CallStack, "Return code: " + ReturnValue.ToString)

                    ' Uninstall RC mirror driver
                    Logger.WriteDebug(CallStack, "Uninstalling RC mirror driver..")
                    ReturnValue = Utility.RunCommand(Globals.DSMFolder + "bin\x86\rcMirrorInstall.exe", "-remove")
                    Logger.WriteDebug(CallStack, "Return code: " + ReturnValue.ToString)

                End If

            End If

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught uninstalling RC drivers.")
            Logger.WriteDebug(ex.Message)

        End Try

        ' *****************************
        ' - Delete cfsystray (so MSIs don't continually stop/start it).
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveTrayIcon|"

        ' Delete cfsystray
        Utility.DeleteFile(CallStack, Globals.DSMFolder + "bin\cfsystray.exe")

        ' Refresh notification area
        WindowsAPI.RefreshNotificationArea()

        ' *****************************
        ' - Run MSI uninstall procedures.
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveMSI|"

        ' Uninstall documentation
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A56A74D1-E994-4447-A2C7-678C62457FA5}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A56A74D1-E994-4447-A2C7-678C62457FA5}") Then
            Logger.WriteDebug(CallStack, "Uninstall Documentation..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {A56A74D1-E994-4447-A2C7-678C62457FA5} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall DSM manager
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{E981CCC3-7C44-4D04-BD38-C7A501469B37}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{E981CCC3-7C44-4D04-BD38-C7A501469B37}") Then
            Logger.WriteDebug(CallStack, "Uninstall DSM Manager..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {E981CCC3-7C44-4D04-BD38-C7A501469B37} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
            Utility.RunCommand("iisreset", , True)
        End If

        ' Uninstall DSM explorer
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87}") Then
            Logger.WriteDebug(CallStack, "Uninstall DSM Explorer..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall scalability server
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{9654079C-BA1E-4628-8403-C7272FF1BD3E}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{9654079C-BA1E-4628-8403-C7272FF1BD3E}") Then
            Logger.WriteDebug(CallStack, "Uninstall Scalability Server..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {9654079C-BA1E-4628-8403-C7272FF1BD3E} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall agent Language Pack DEU
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{6B511A0E-4D3C-4128-91BE-77740420FD36}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{6B511A0E-4D3C-4128-91BE-77740420FD36}") Then
            Logger.WriteDebug(CallStack, "Uninstall Agent Language Pack DEU..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {6B511A0E-4D3C-4128-91BE-77740420FD36} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall agent Language Pack FRA
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{9DA41BF7-B1B1-46FD-9525-DEDCCACFE816}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{9DA41BF7-B1B1-46FD-9525-DEDCCACFE816}") Then
            Logger.WriteDebug(CallStack, "Uninstall Agent Language Pack FRA..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {9DA41BF7-B1B1-46FD-9525-DEDCCACFE816} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall agent Language Pack JPN
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A4DA5EED-B13B-4A5E-A8A1-748DE46A2607}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A4DA5EED-B13B-4A5E-A8A1-748DE46A2607}") Then
            Logger.WriteDebug(CallStack, "Uninstall Agent Language Pack JPN..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {A4DA5EED-B13B-4A5E-A8A1-748DE46A2607} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall agent Language Pack ESN
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{94163038-B65E-45BE-A70C-DC319C43CFF2}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{94163038-B65E-45BE-A70C-DC319C43CFF2}") Then
            Logger.WriteDebug(CallStack, "Uninstall Agent Language Pack ESN..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {94163038-B65E-45BE-A70C-DC319C43CFF2} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall agent Language Pack KOR
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{2C300042-2857-4E6B-BC05-920CA9953D2C}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{2C300042-2857-4E6B-BC05-920CA9953D2C}") Then
            Logger.WriteDebug(CallStack, "Uninstall Agent Language Pack KOR..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {2C300042-2857-4E6B-BC05-920CA9953D2C} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall agent Language Pack CHS
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8}") Then
            Logger.WriteDebug(CallStack, "Uninstall Agent Language Pack CHS..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall performance lite client
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506}") Then
            Logger.WriteDebug(CallStack, "Uninstall Performance Lite Client..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall performance lite agent
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{019094B6-40C9-45AE-A799-CCA2D6AA66A6}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{019094B6-40C9-45AE-A799-CCA2D6AA66A6}") Then
            Logger.WriteDebug(CallStack, "Uninstall Performance Lite Agent..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {019094B6-40C9-45AE-A799-CCA2D6AA66A6} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall RC agent
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{84288555-A79E-4ABD-BA53-219C4D2CA20B}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{84288555-A79E-4ABD-BA53-219C4D2CA20B}") Then
            Logger.WriteDebug(CallStack, "Uninstall Remote Control Agent (ENU and multi-language)..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {84288555-A79E-4ABD-BA53-219C4D2CA20B} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall DTS agent
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{C0C44BF2-E5E0-4C02-B9D3-33C691F060EA}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{C0C44BF2-E5E0-4C02-B9D3-33C691F060EA}") Then
            Logger.WriteDebug(CallStack, "Uninstall Data Transport Service Agent (ENU and multi-language)..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {C0C44BF2-E5E0-4C02-B9D3-33C691F060EA} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall SD agent
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{62ADA55C-1B98-431F-8618-CDF3CE4CFEEC}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{62ADA55C-1B98-431F-8618-CDF3CE4CFEEC}") Then
            Logger.WriteDebug(CallStack, "Uninstall Software Delivery Agent (ENU and multi-language)..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {62ADA55C-1B98-431F-8618-CDF3CE4CFEEC} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall AM agent
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{624FA386-3A39-4EBF-9CB9-C2B484D78B29}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{624FA386-3A39-4EBF-9CB9-C2B484D78B29}") Then
            Logger.WriteDebug(CallStack, "Uninstall Asset Management Agent (ENU and multi-language)..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {624FA386-3A39-4EBF-9CB9-C2B484D78B29} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall basic agent
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{501C99B9-1644-4FC2-833B-E675572F8929}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{501C99B9-1644-4FC2-833B-E675572F8929}") Then
            Logger.WriteDebug(CallStack, "Uninstall Basic Inventory Agent (ENU and multi-language)..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {501C99B9-1644-4FC2-833B-E675572F8929} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall old DMPrimer
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{5933CC13-52AB-4713-85DB-E72034B5697A}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{5933CC13-52AB-4713-85DB-E72034B5697A}") Then
            Logger.WriteDebug(CallStack, "Uninstall old DMPrimer..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {5933CC13-52AB-4713-85DB-E72034B5697A} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall DMPrimer
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A312C331-2E7A-42E1-9F31-902920C402EE}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A312C331-2E7A-42E1-9F31-902920C402EE}") Then
            Logger.WriteDebug(CallStack, "Uninstall DMPrimer..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {A312C331-2E7A-42E1-9F31-902920C402EE} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall SSA
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2}") Then
            Logger.WriteDebug(CallStack, "Uninstall CA Secure Socket Adapter..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall old master setup
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{C163EC47-55B6-4B06-9D03-2A720548BE86}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{C163EC47-55B6-4B06-9D03-2A720548BE86}") Then
            Logger.WriteDebug(CallStack, "Uninstall old MasterSetup..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {C163EC47-55B6-4B06-9D03-2A720548BE86} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' Uninstall master setup
        If Utility.RegistryKeyExists("SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{DA485AC8-BACB-492D-9B1E-14AA5B61597E}") OrElse
            Utility.RegistryKeyExists("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{DA485AC8-BACB-492D-9B1E-14AA5B61597E}") Then
            Logger.WriteDebug(CallStack, "Uninstall MasterSetup..")
            ReturnValue = Utility.RunCommand("msiexec.exe", "/x {DA485AC8-BACB-492D-9B1E-14AA5B61597E} /qn")
            Logger.WriteDebug(CallStack, "MSI return code: " + ReturnValue.ToString)
        End If

        ' *****************************
        ' - ITCM registry cleanup.
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveRegistry|"

        ' Registry cleanup
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A56A74D1-E994-4447-A2C7-678C62457FA5}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A56A74D1-E994-4447-A2C7-678C62457FA5}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A56A74D1-E994-4447-A2C7-678C62457FA5}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A56A74D1-E994-4447-A2C7-678C62457FA5}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{E981CCC3-7C44-4D04-BD38-C7A501469B37}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{E981CCC3-7C44-4D04-BD38-C7A501469B37}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{E981CCC3-7C44-4D04-BD38-C7A501469B37}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{E981CCC3-7C44-4D04-BD38-C7A501469B37}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{42C0EC64-A6E7-4FBD-A5B6-1A6AD94A2D87}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{9654079C-BA1E-4628-8403-C7272FF1BD3E}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{9654079C-BA1E-4628-8403-C7272FF1BD3E}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{9654079C-BA1E-4628-8403-C7272FF1BD3E}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{9654079C-BA1E-4628-8403-C7272FF1BD3E}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{6B511A0E-4D3C-4128-91BE-77740420FD36}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{6B511A0E-4D3C-4128-91BE-77740420FD36}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{6B511A0E-4D3C-4128-91BE-77740420FD36}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{6B511A0E-4D3C-4128-91BE-77740420FD36}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{9DA41BF7-B1B1-46FD-9525-DEDCCACFE816}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{9DA41BF7-B1B1-46FD-9525-DEDCCACFE816}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{9DA41BF7-B1B1-46FD-9525-DEDCCACFE816}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{9DA41BF7-B1B1-46FD-9525-DEDCCACFE816}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A4DA5EED-B13B-4A5E-A8A1-748DE46A2607}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A4DA5EED-B13B-4A5E-A8A1-748DE46A2607}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A4DA5EED-B13B-4A5E-A8A1-748DE46A2607}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A4DA5EED-B13B-4A5E-A8A1-748DE46A2607}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{94163038-B65E-45BE-A70C-DC319C43CFF2}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{94163038-B65E-45BE-A70C-DC319C43CFF2}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{94163038-B65E-45BE-A70C-DC319C43CFF2}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{94163038-B65E-45BE-A70C-DC319C43CFF2}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{2C300042-2857-4E6B-BC05-920CA9953D2C}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{2C300042-2857-4E6B-BC05-920CA9953D2C}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{2C300042-2857-4E6B-BC05-920CA9953D2C}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{2C300042-2857-4E6B-BC05-920CA9953D2C}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{2D3B15F5-BBA3-4D9E-B7AB-DC2A8BD6EAD8}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{B6588B4E-CF7C-4FF7-AC15-62A8FFD2A506}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{019094B6-40C9-45AE-A799-CCA2D6AA66A6}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{019094B6-40C9-45AE-A799-CCA2D6AA66A6}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{019094B6-40C9-45AE-A799-CCA2D6AA66A6}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{019094B6-40C9-45AE-A799-CCA2D6AA66A6}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{84288555-A79E-4ABD-BA53-219C4D2CA20B}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{84288555-A79E-4ABD-BA53-219C4D2CA20B}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{84288555-A79E-4ABD-BA53-219C4D2CA20B}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{84288555-A79E-4ABD-BA53-219C4D2CA20B}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{C0C44BF2-E5E0-4C02-B9D3-33C691F060EA}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{C0C44BF2-E5E0-4C02-B9D3-33C691F060EA}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{C0C44BF2-E5E0-4C02-B9D3-33C691F060EA}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{C0C44BF2-E5E0-4C02-B9D3-33C691F060EA}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{62ADA55C-1B98-431F-8618-CDF3CE4CFEEC}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{62ADA55C-1B98-431F-8618-CDF3CE4CFEEC}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{62ADA55C-1B98-431F-8618-CDF3CE4CFEEC}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{62ADA55C-1B98-431F-8618-CDF3CE4CFEEC}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{624FA386-3A39-4EBF-9CB9-C2B484D78B29}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{624FA386-3A39-4EBF-9CB9-C2B484D78B29}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{624FA386-3A39-4EBF-9CB9-C2B484D78B29}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{624FA386-3A39-4EBF-9CB9-C2B484D78B29}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{501C99B9-1644-4FC2-833B-E675572F8929}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{501C99B9-1644-4FC2-833B-E675572F8929}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{501C99B9-1644-4FC2-833B-E675572F8929}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{501C99B9-1644-4FC2-833B-E675572F8929}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{5933cc13-52ab-4713-85db-e72034b5697A}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{5933cc13-52ab-4713-85db-e72034b5697A}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{5933cc13-52ab-4713-85db-e72034b5697A}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{5933cc13-52ab-4713-85db-e72034b5697A}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A312C331-2E7A-42E1-9F31-902920C402EE}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A312C331-2E7A-42E1-9F31-902920C402EE}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A312C331-2E7A-42E1-9F31-902920C402EE}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A312C331-2E7A-42E1-9F31-902920C402EE}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{25CCFBFE-BDE1-43F8-B078-C9AC89B21AF2}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{C163EC47-55B6-4B06-9D03-2A720548BE86}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{C163EC47-55B6-4B06-9D03-2A720548BE86}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{C163EC47-55B6-4B06-9D03-2A720548BE86}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{C163EC47-55B6-4B06-9D03-2A720548BE86}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{C163EC47-55B6-4B06-9D03-2A720548BE86}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{C163EC47-55B6-4B06-9D03-2A720548BE86}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{C163EC47-55B6-4B06-9D03-2A720548BE86}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{C163EC47-55B6-4B06-9D03-2A720548BE86}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{DA485AC8-BACB-492D-9B1E-14AA5B61597E}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{DA485AC8-BACB-492D-9B1E-14AA5B61597E}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{DA485AC8-BACB-492D-9B1E-14AA5B61597E}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{DA485AC8-BACB-492D-9B1E-14AA5B61597E}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{DA485AC8-BACB-492D-9B1E-14AA5B61597E}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{DA485AC8-BACB-492D-9B1E-14AA5B61597E}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{DA485AC8-BACB-492D-9B1E-14AA5B61597E}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{DA485AC8-BACB-492D-9B1E-14AA5B61597E}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{FA98151D-89CE-4241-AFEA-C81B545DD95D}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{FA98151D-89CE-4241-AFEA-C81B545DD95D}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{FA98151D-89CE-4241-AFEA-C81B545DD95D}") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{FA98151D-89CE-4241-AFEA-C81B545DD95D}")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1C1") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1C1")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1C1") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1C1")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1_Feature_Pack_1") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1_Feature_Pack_1")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1_Feature_Pack_1") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CA_Client_Automation_r12_5_SP1_Feature_Pack_1")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\CA IT Client Manager 12_9 Feature Pack 1") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\CA IT Client Manager 12_9 Feature Pack 1")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CA IT Client Manager 12_9 Feature Pack 1") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CA IT Client Manager 12_9 Feature Pack 1")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\caf") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\caf")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\CA-MessageQueuing") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\CA-MessageQueuing")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\CA-SAM-Pmux") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\CA-SAM-Pmux")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\hmAgent") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\hmAgent")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\rcSmCard") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\rcSmCard")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\rcVidCap") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\rcVidCap")
        If Utility.DeleteRegistrySubKeyTree("HKLM", "SYSTEM\CurrentControlSet\Services\caspliteagent") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Services\caspliteagent")
        If Utility.DeleteRegistrySubKeysWithValue("HKLM", "SYSTEM\CurrentControlSet\Control\Video", "Service", "rcSecCap") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Control\Video")
        If Utility.DeleteRegistrySubKeysWithValue("HKLM", "SYSTEM\CurrentControlSet\Control\Video", "Service", "rcVidCap") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SYSTEM\CurrentControlSet\Control\Video")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\133C213AA7E21E24F9130992024C20EE") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\133C213AA7E21E24F9130992024C20EE")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\E4B8856BC7FC7FF4CA51268AFF2D5A60") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\E4B8856BC7FC7FF4CA51268AFF2D5A60")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\6B4909109C04EA547A99CC2A6DAA666A") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\6B4909109C04EA547A99CC2A6DAA666A")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\2FB44C0C0E5E20C49B3D336C190F06AE") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\2FB44C0C0E5E20C49B3D336C190F06AE")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\3CCC189E44C740D4DB837C5A1064B973") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\3CCC189E44C740D4DB837C5A1064B973")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\46CE0C247E6ADBF45A6BA1A69DA4D278") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\46CE0C247E6ADBF45A6BA1A69DA4D278")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\55588248E97ADBA4AB3512C9D4C22AB0") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\55588248E97ADBA4AB3512C9D4C22AB0")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\683AF42693A3FBE4C99B2C4B487DB892") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\683AF42693A3FBE4C99B2C4B487DB892")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\73D30663ADBDC164EB16784BAC372D31") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\73D30663ADBDC164EB16784BAC372D31")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\8CA584ADBCABD294B9E141AAB51695E7") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\8CA584ADBCABD294B9E141AAB51695E7")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\9B99C10544612CF438B36E5775F29892") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\9B99C10544612CF438B36E5775F29892")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\C55ADA2689B1F1346881DC3FECC4EFCE") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\C55ADA2689B1F1346881DC3FECC4EFCE")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\C9704569E1AB826448307C72F21FDBE3") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\C9704569E1AB826448307C72F21FDBE3")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\D15189AFEC981424FAAE8CB145D59DD5") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\D15189AFEC981424FAAE8CB145D59DD5")
        If Utility.DeleteRegistrySubKeyTree("HKCR", "Installer\Products\EFBFCC521EDB8F340B879CCA982BA12F") Then Logger.WriteDebug(CallStack, "Delete registry: HKCR\Installer\Products\EFBFCC521EDB8F340B879CCA982BA12F")

        ' Check for full uninstall switch
        If Globals.RemoveITCM AndAlso Not Globals.KeepHostUUIDSwitch Then

            ' Delete entire CA registry key
            If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\Wow6432Node\ComputerAssociates") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\Wow6432Node\ComputerAssociates")
            If Utility.DeleteRegistrySubKeyTree("HKLM", "SOFTWARE\ComputerAssociates") Then Logger.WriteDebug(CallStack, "Delete registry: HKLM\SOFTWARE\ComputerAssociates")

        ElseIf Globals.RemoveITCM AndAlso Globals.KeepHostUUIDSwitch Then

            ' Open 64-bit registry
            TestKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\ComputerAssociates", False)

            ' Check 64-bit or try 32-bit registry
            If TestKey Is Nothing Then TestKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\ComputerAssociates", False)

            ' Check registry is valid (32-bit or 64-bit) 
            If TestKey IsNot Nothing Then

                ' Iterate subkeys
                For Each subkey As String In TestKey.GetSubKeyNames

                    ' Check for hostuuid subkey
                    If Not subkey.ToLower.Equals("hostuuid") Then

                        ' Delete subkey
                        Utility.DeleteRegistrySubKeyTree("HKLM", TestKey.Name.Substring(TestKey.Name.IndexOf("\") + 1) + "\" + subkey)

                    End If

                Next

            End If

            ' Close registry key
            TestKey.Close()

        End If

        ' *****************************
        ' - ITCM file and folder cleanup.
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveFiles|"

        ' Remove common files
        Utility.DeleteFile(CallStack, Environment.SystemDirectory.Substring(0, 3) + "calogfile.txt")
        Utility.DeleteFile(CallStack, Environment.SystemDirectory.Substring(0, 3) + "canpc.dat")
        Utility.DeleteFile(CallStack, Environment.SystemDirectory.Substring(0, 3) + "dmmsi.log")
        Utility.DeleteFile(CallStack, Environment.SystemDirectory.Substring(0, 3) + "osimconf.ini")
        Utility.DeleteFile(CallStack, Environment.SystemDirectory.Substring(0, 3) + "S.log")
        Utility.DeleteFile(CallStack, Environment.SystemDirectory.Substring(0, 3) + "setup.bat")
        Utility.DeleteFile(CallStack, Globals.WindowsBase + "\dmsetup.exe")
        Utility.DeleteFile(CallStack, Globals.WindowsBase + "\dmboot.exe")
        Utility.DeleteFile(CallStack, Globals.WindowsBase + "\dmkeydat.cer")
        Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\capki_install.log")
        Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\dmprimer.log")
        Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\SSAInstall.log")
        Utility.DeleteFilePattern(CallStack, Globals.WindowsTemp, "DSM_DW")
        Utility.DeleteFilePattern(CallStack, Globals.WindowsTemp, "TRC_")

        ' *****************************
        ' - Cleanup basic agent temporary files.
        ' *****************************

        Try

            ' Get the directory listing
            FileList = System.IO.Directory.GetFiles(Globals.WindowsTemp)

            ' Check for positive files
            If FileList.Length > 0 Then

                ' Loop the file list
                For n As Integer = 0 To FileList.Length - 1

                    ' Get a filename
                    strFile = FileList(n).ToString.ToLower
                    strFile = strFile.Substring(strFile.LastIndexOf("\") + 1)

                    ' Example of files:
                    ' bhiA.tmp
                    ' bhiA0.tmp
                    ' bhiA01.tmp
                    ' bhiA012.tmp

                    ' Check if filename starts with 'bhi' and ends with '.tmp'
                    If strFile.StartsWith("bhi") AndAlso strFile.EndsWith(".tmp") Then

                        ' Parse the HEX substring
                        hexString = strFile.Substring(3, strFile.Length - 3)
                        hexString = hexString.Substring(0, hexString.IndexOf("."))

                        ' Check if filename only contains HEX
                        If Utility.IsHexString(hexString) Then

                            ' Attempt to delete each file
                            Try

                                ' Unset read-only parameter (in case it's set)
                                System.IO.File.SetAttributes(FileList(n), IO.FileAttributes.Normal)

                                ' Delete the file
                                Utility.DeleteFile(CallStack, FileList(n))

                            Catch ex2 As Exception

                                ' Write debug
                                Logger.WriteDebug(CallStack, "Warning: Exception caught deleting file: " + FileList(n).ToString)
                                Logger.WriteDebug(ex2.Message)

                            End Try

                        End If

                    End If

                Next

            End If

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Warning: Exception caught cleaning up temporary files.")
            Logger.WriteDebug(ex.Message)

        End Try

        ' Remove common folders
        Utility.DeleteFolder(CallStack, Environment.SystemDirectory.Substring(0, 3) + "ca-osim")
        Utility.DeleteFolder(CallStack, Environment.SystemDirectory.Substring(0, 3) + "oeminst")
        Utility.DeleteFolder(CallStack, Environment.SystemDirectory.Substring(0, 3) + "sdagent")

        ' Check for full uninstall switch
        If Globals.RemoveITCM Then

            ' Recursive delete CA folder contents (Utility API will update ACLs)
            Utility.DeleteFolderContents(CallStack, CAFolderPersistentVar, Nothing)

            ' Delete CA base folder
            Utility.DeleteFolder(CallStack, CAFolderPersistentVar)

            ' Delete CA roaming profile contents
            Utility.DeleteFolderContents(CallStack, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\CA", Nothing)

            ' Delete CA roaming profile folder
            Utility.DeleteFolder(CallStack, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\CA")

        Else

            ' Recursive delete CA folder contents (Utility API will update ACLs)
            Utility.DeleteFolderContents(CallStack, Globals.DSMFolder, Nothing)

            ' Delete CA base folder
            Utility.DeleteFolder(CallStack, Globals.DSMFolder)

        End If

        ' *****************************
        ' - ITCM environment variable cleanup.
        ' *****************************

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11) + "RemoveVariables|"

        ' Encapsulate
        Try

            ' Check for full uninstall switch
            If Globals.RemoveITCM Then

                ' Environment variable cleanup
                If Utility.DeleteEnvironmentVariable("CA_DSM_ORACLE_JDBC_PATH") Then Logger.WriteDebug(CallStack, "Variable deleted: CA_DSM_ORACLE_JDBC_PATH")
                If Utility.DeleteEnvironmentVariable("CAI_CAFT") Then Logger.WriteDebug(CallStack, "Variable deleted: CAI_CAFT")
                If Utility.DeleteEnvironmentVariable("CAI_MSQ") Then Logger.WriteDebug(CallStack, "Variable deleted: CAI_MSQ")
                If Utility.DeleteEnvironmentVariable("CSAM_LOGGER_CONF") Then Logger.WriteDebug(CallStack, "Variable deleted: CSAM_LOGGER_CONF")
                If Utility.DeleteEnvironmentVariable("CSAM_SOCKADAPTER") Then Logger.WriteDebug(CallStack, "Variable deleted: CSAM_SOCKADAPTER")
                If Utility.DeleteEnvironmentVariable("CAPKIHOME") Then Logger.WriteDebug(CallStack, "Variable deleted: CAPKIHOME")
                If Utility.DeleteEnvironmentVariable("ETPKIHOME") Then Logger.WriteDebug(CallStack, "Variable deleted: ETPKIHOME")
                If Utility.DeleteEnvironmentVariable("R_SHLIB_LD_LIBRARY_PATH") Then Logger.WriteDebug(CallStack, "Variable deleted: R_SHLIB_LD_LIBRARY_PATH")
                If Utility.DeleteEnvironmentVariable("RALHOME") Then Logger.WriteDebug(CallStack, "Variable deleted: RALHOME")
                If Utility.DeleteEnvironmentVariable("SDROOT") Then Logger.WriteDebug(CallStack, "Variable deleted: SDROOT")

            Else

                ' Environment variable cleanup
                If Utility.DeleteEnvironmentVariable("CA_DSM_ORACLE_JDBC_PATH") Then Logger.WriteDebug(CallStack, "Variable deleted: CA_DSM_ORACLE_JDBC_PATH")
                If Utility.DeleteEnvironmentVariable("SDROOT") Then Logger.WriteDebug(CallStack, "Variable deleted: SDROOT")

            End If

            ' Open registry key
            TestKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Session Manager\Environment\", True)

            ' Iterate PATH variable 
            For Each strPath In TestKey.GetValue("PATH", Nothing, Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames).ToString.Split(";")

                ' Check for empty string
                If strPath.Equals("") Then Continue For

                ' Check if path string contains the CA folder
                If CAFolderPersistentVar IsNot Nothing AndAlso
                    Environment.ExpandEnvironmentVariables(strPath).ToLower.Contains(CAFolderPersistentVar.ToLower) Then

                    ' Verify expanded path strings
                    If System.IO.Directory.Exists(Environment.ExpandEnvironmentVariables(strPath)) Then

                        ' Add to cleaned path
                        CleanPathVar += strPath + ";"

                    Else

                        ' Write debug 
                        Logger.WriteDebug(CallStack, "Remove path: " + strPath)

                    End If

                Else

                    ' Add to PATH (essentially don't touch non-CA entries)
                    CleanPathVar += strPath + ";"

                End If

            Next

            ' Ensure PATH is REG_EXPAND_SZ
            Microsoft.Win32.Registry.SetValue(TestKey.Name, "Path", CleanPathVar, Microsoft.Win32.RegistryValueKind.ExpandString)

            ' Close registry key
            TestKey.Close()

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Failed to cleanup environment variables.")
            Logger.WriteDebug(ex.Message)

        End Try

        ' Update call stack
        CallStack = CallStack.Substring(0, CallStack.IndexOf("RemoveITCM|") + 11)

        ' *****************************
        ' - Cleanup temp folder.
        ' *****************************

        Try

            ' Check if temporary folder exists
            If Globals.WinOfflineTemp IsNot Nothing AndAlso System.IO.Directory.Exists(Globals.WinOfflineTemp) Then

                ' Write debug
                Logger.WriteDebug(CallStack, "Delete Folder: " + Globals.WinOfflineTemp)

                ' Delete the folder
                System.IO.Directory.Delete(Globals.WinOfflineTemp, True)

            End If

        Catch ex As Exception

            ' Write debug
            Logger.WriteDebug(CallStack, "Error: Exception caught deleting temporary folder.")
            Logger.WriteDebug(ex.Message)
            Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder..")

            ' Get the directory listing
            FileList = System.IO.Directory.GetFiles(Globals.WinOfflineTemp)

            ' Loop the file list
            For n As Integer = 0 To FileList.Length - 1

                ' Attempt to delete each file
                Utility.DeleteFile(CallStack, FileList(n))

            Next

            ' Write debug
            Logger.WriteDebug(CallStack, "Fallback: Delete contents of temporary folder completed.")

        End Try

        ' *****************************
        ' - WinOffline executable cleanup.
        ' *****************************

        ' Check for SD mode
        If Not Globals.AttachedtoConsole AndAlso Globals.WinOfflineExplorer Is Nothing Then

            ' Write debug
            Logger.WriteDebug(CallStack, "Delete file: " + Globals.ProcessName)

            ' Schedule WinOffline removal
            Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Globals.ProcessName
            Info.WindowStyle = ProcessWindowStyle.Hidden
            Info.CreateNoWindow = True
            Info.FileName = "cmd.exe"
            Process.Start(Info)

        End If

        ' Delete the config file
        Utility.DeleteFile(CallStack, Globals.WindowsTemp + "\" + Globals.ProcessShortName + ".config")

        ' Write debug
        Logger.WriteDebug(CallStack, "Removal completed.")

        ' Re-enable remove button on GUI execution
        If Globals.WinOfflineExplorer IsNot Nothing Then Globals.WinOfflineExplorer.btnRemoveITCM.Enabled = True

        ' Return
        Return 0

    End Function

End Class
Imports System.Threading

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WinOfflineUI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Patch Maintenance")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Mdb Overview")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Table Space Summary")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Agent Summary")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("User Summary")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Scalability Summary")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Engine Summary")
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Group Evaluations")
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Installed Software")
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Discovered Software")
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Unused Software")
        Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Duplicate Computers")
        Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("CleanApps Script")
        Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Query Editor")
        Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Database Tools", New System.Windows.Forms.TreeNode() {TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6, TreeNode7, TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13, TreeNode14})
        Dim TreeNode16 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("ENC Ping Utility")
        Dim TreeNode17 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("ENC Tools", New System.Windows.Forms.TreeNode() {TreeNode16})
        Dim TreeNode18 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Removal Tool")
        Dim TreeNode19 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("System Information")
        Dim TreeNode20 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Debug Window")
        Dim TreeNode21 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("WinOffline Explorer", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode15, TreeNode17, TreeNode18, TreeNode19, TreeNode20})
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WinOfflineUI))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlSystemInfo = New System.Windows.Forms.Panel()
        Me.grpInfo = New System.Windows.Forms.GroupBox()
        Me.lblPlugins = New System.Windows.Forms.Label()
        Me.lstPlugins = New System.Windows.Forms.ListBox()
        Me.btnBrowseSSA = New System.Windows.Forms.Button()
        Me.btnBrowseDSM = New System.Windows.Forms.Button()
        Me.lblSSAFolder = New System.Windows.Forms.Label()
        Me.txtSSAFolder = New System.Windows.Forms.TextBox()
        Me.btnBrowseCAM = New System.Windows.Forms.Button()
        Me.lblCAMFolder = New System.Windows.Forms.Label()
        Me.txtCAMFolder = New System.Windows.Forms.TextBox()
        Me.lblScalability = New System.Windows.Forms.Label()
        Me.lblManager = New System.Windows.Forms.Label()
        Me.txtDomainMgr = New System.Windows.Forms.TextBox()
        Me.txtScalabilityServer = New System.Windows.Forms.TextBox()
        Me.txtHostUUID = New System.Windows.Forms.TextBox()
        Me.lblHostUUID = New System.Windows.Forms.Label()
        Me.txtFunction = New System.Windows.Forms.TextBox()
        Me.txtDSMFolder = New System.Windows.Forms.TextBox()
        Me.txtVersion = New System.Windows.Forms.TextBox()
        Me.lblInstallDir = New System.Windows.Forms.Label()
        Me.lblFunction = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.txtModel = New System.Windows.Forms.TextBox()
        Me.lblModel = New System.Windows.Forms.Label()
        Me.txtManufacturer = New System.Windows.Forms.TextBox()
        Me.lblManufacturer = New System.Windows.Forms.Label()
        Me.txtPlatform = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.lblHostname = New System.Windows.Forms.Label()
        Me.lblPlatform = New System.Windows.Forms.Label()
        Me.txtHostname = New System.Windows.Forms.TextBox()
        Me.lblSerial = New System.Windows.Forms.Label()
        Me.lblNetwork = New System.Windows.Forms.Label()
        Me.lstNetAddr = New System.Windows.Forms.ListBox()
        Me.pnlWinOfflineMain = New System.Windows.Forms.Panel()
        Me.lvwApplyOptions = New System.Windows.Forms.ListView()
        Me.ColumnOptions = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.grpPatchView = New System.Windows.Forms.GroupBox()
        Me.lvwPatchList = New System.Windows.Forms.ListView()
        Me.ColumnPatch = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnCode = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.pnlWinOfflineButton1 = New System.Windows.Forms.Panel()
        Me.btnWinOfflineStart1 = New System.Windows.Forms.Button()
        Me.btnWinOfflineExit3 = New System.Windows.Forms.Button()
        Me.btnWinOfflineBack3 = New System.Windows.Forms.Button()
        Me.ExplorerTree = New System.Windows.Forms.TreeView()
        Me.SplitWinOfflineUI = New System.Windows.Forms.SplitContainer()
        Me.pnlWinOfflineStart = New System.Windows.Forms.Panel()
        Me.grpWinOfflineWelcome = New System.Windows.Forms.GroupBox()
        Me.btnWinOfflineNext1 = New System.Windows.Forms.Button()
        Me.btnWinOfflineExit1 = New System.Windows.Forms.Button()
        Me.grpWinOfflineStartOptions = New System.Windows.Forms.GroupBox()
        Me.rbnCLIHelp = New System.Windows.Forms.RadioButton()
        Me.rbnBackout = New System.Windows.Forms.RadioButton()
        Me.rbnLearn = New System.Windows.Forms.RadioButton()
        Me.rbnApply = New System.Windows.Forms.RadioButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.grpHistory = New System.Windows.Forms.GroupBox()
        Me.treHistory = New System.Windows.Forms.TreeView()
        Me.pnlWinOfflineRemove = New System.Windows.Forms.Panel()
        Me.lvwRemoveOptions = New System.Windows.Forms.ListView()
        Me.ColumnRemoveOptions = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.grpHistoryView = New System.Windows.Forms.GroupBox()
        Me.lvwHistory = New System.Windows.Forms.ListView()
        Me.ColumnHistoryPatch = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHistoryComp = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHistoryStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.pnlWinOfflineButton2 = New System.Windows.Forms.Panel()
        Me.btnWinOfflineStart2 = New System.Windows.Forms.Button()
        Me.btnWinOfflineExit4 = New System.Windows.Forms.Button()
        Me.btnWinOfflineBack4 = New System.Windows.Forms.Button()
        Me.pnlWinOfflineSDHelp = New System.Windows.Forms.Panel()
        Me.grpSDHelp = New System.Windows.Forms.GroupBox()
        Me.btnWinOfflineSwicthes = New System.Windows.Forms.Button()
        Me.btnWinOfflineBack2 = New System.Windows.Forms.Button()
        Me.btnWinOfflineSDHelpNext = New System.Windows.Forms.Button()
        Me.btnWinOfflineSDHelpPrevious = New System.Windows.Forms.Button()
        Me.picSteps = New System.Windows.Forms.PictureBox()
        Me.lblStepx = New System.Windows.Forms.Label()
        Me.pnlWinOfflineCLIHelp = New System.Windows.Forms.Panel()
        Me.grpCLIOptions = New System.Windows.Forms.GroupBox()
        Me.btnWinOfflineExit2 = New System.Windows.Forms.Button()
        Me.btnWinOfflineBack1 = New System.Windows.Forms.Button()
        Me.rtbOptions = New System.Windows.Forms.RichTextBox()
        Me.pnlSqlMdbOverview = New System.Windows.Forms.Panel()
        Me.grpSqlMdbOverview = New System.Windows.Forms.GroupBox()
        Me.txtMdbType = New System.Windows.Forms.TextBox()
        Me.lblMdbType = New System.Windows.Forms.Label()
        Me.lblMdbVersion = New System.Windows.Forms.Label()
        Me.lblMdbInstallDate = New System.Windows.Forms.Label()
        Me.txtMdbVersion = New System.Windows.Forms.TextBox()
        Me.lblITCMVersion = New System.Windows.Forms.Label()
        Me.txtMdbInstallDate = New System.Windows.Forms.TextBox()
        Me.txtITCMVersion = New System.Windows.Forms.TextBox()
        Me.grpSqlMdbItcmSum = New System.Windows.Forms.GroupBox()
        Me.lvwITCMSummary = New System.Windows.Forms.ListView()
        Me.ColumnMetric = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnValue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.grpSqlMdbAgentVersion = New System.Windows.Forms.GroupBox()
        Me.dgvAgentVersion = New System.Windows.Forms.DataGridView()
        Me.grpSqlMdbDefSummary = New System.Windows.Forms.GroupBox()
        Me.dgvContentSummary = New System.Windows.Forms.DataGridView()
        Me.pnlSqlMdbOverviewButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectMdbOverview = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectMdbOverview = New System.Windows.Forms.Button()
        Me.btnSqlRefreshMdbOverview = New System.Windows.Forms.Button()
        Me.btnSqlExportMdbOverview = New System.Windows.Forms.Button()
        Me.btnSqlExitMdbOverview = New System.Windows.Forms.Button()
        Me.pnlSqlTableSpaceGrid = New System.Windows.Forms.Panel()
        Me.grpSqlTableSpace = New System.Windows.Forms.GroupBox()
        Me.dgvTableSpaceGrid = New System.Windows.Forms.DataGridView()
        Me.prgTableSpaceGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlTableSpaceGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectTableSpace = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectTableSpace = New System.Windows.Forms.Button()
        Me.btnSqlRefreshTableSpace = New System.Windows.Forms.Button()
        Me.btnSqlExportTableSpace = New System.Windows.Forms.Button()
        Me.btnSqlExitTableSpace = New System.Windows.Forms.Button()
        Me.pnlSqlUserGrid = New System.Windows.Forms.Panel()
        Me.tabCtrlUserGrid = New System.Windows.Forms.TabControl()
        Me.tabUserSummary = New System.Windows.Forms.TabPage()
        Me.dgvUserGrid = New System.Windows.Forms.DataGridView()
        Me.tabUserObsolete90 = New System.Windows.Forms.TabPage()
        Me.dgvUserObsolete90 = New System.Windows.Forms.DataGridView()
        Me.tabUserObsolete365 = New System.Windows.Forms.TabPage()
        Me.dgvUserObsolete365 = New System.Windows.Forms.DataGridView()
        Me.prgUserGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlUserGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectUserGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectUserGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshUserGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportUserGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitUserGrid = New System.Windows.Forms.Button()
        Me.pnlSqlAgentGrid = New System.Windows.Forms.Panel()
        Me.tabCtrlAgentGrid = New System.Windows.Forms.TabControl()
        Me.tabAgentSummary = New System.Windows.Forms.TabPage()
        Me.dgvAgentGrid = New System.Windows.Forms.DataGridView()
        Me.tabAgentObsolete90 = New System.Windows.Forms.TabPage()
        Me.dgvAgentObsolete90 = New System.Windows.Forms.DataGridView()
        Me.tabAgentObsolete365 = New System.Windows.Forms.TabPage()
        Me.dgvAgentObsolete365 = New System.Windows.Forms.DataGridView()
        Me.prgAgentGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlAgentGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectAgentGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectAgentGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshAgentGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportAgentGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitAgentGrid = New System.Windows.Forms.Button()
        Me.pnlSqlServerGrid = New System.Windows.Forms.Panel()
        Me.tabCtrlServerGrid = New System.Windows.Forms.TabControl()
        Me.tabServerSummary = New System.Windows.Forms.TabPage()
        Me.dgvServerGrid = New System.Windows.Forms.DataGridView()
        Me.tabServerLastCollected24 = New System.Windows.Forms.TabPage()
        Me.dgvServerLastCollected24 = New System.Windows.Forms.DataGridView()
        Me.tabServerSignature30 = New System.Windows.Forms.TabPage()
        Me.dgvServerSignature30 = New System.Windows.Forms.DataGridView()
        Me.prgServerGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlServerGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectServerGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectServerGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshServerGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportServerGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitServerGrid = New System.Windows.Forms.Button()
        Me.pnlSqlEngineGrid = New System.Windows.Forms.Panel()
        Me.grpSqlEngineGrid = New System.Windows.Forms.GroupBox()
        Me.dgvEngineGrid = New System.Windows.Forms.DataGridView()
        Me.prgEngineGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlEngineGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectEngineGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectEngineGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshEngineGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportEngineGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitEngineGrid = New System.Windows.Forms.Button()
        Me.pnlSqlGroupEvalGrid = New System.Windows.Forms.Panel()
        Me.grpSqlGroupEvalGrid = New System.Windows.Forms.GroupBox()
        Me.dgvGroupEvalGrid = New System.Windows.Forms.DataGridView()
        Me.prgGroupEvalGrid = New System.Windows.Forms.ProgressBar()
        Me.grpSqlGroupEvalGridInst = New System.Windows.Forms.GroupBox()
        Me.btnGroupEvalGridPreview = New System.Windows.Forms.Button()
        Me.lblGroupEvalGrid = New System.Windows.Forms.Label()
        Me.btnGroupEvalGridCommit = New System.Windows.Forms.Button()
        Me.btnGroupEvalGridDiscard = New System.Windows.Forms.Button()
        Me.pnlSqlGroupEvalGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectGroupEvalGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectGroupEvalGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshGroupEvalGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportGroupEvalGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitGroupEvalGrid = New System.Windows.Forms.Button()
        Me.pnlSqlInstSoftGrid = New System.Windows.Forms.Panel()
        Me.grpSqlInstSoftGrid = New System.Windows.Forms.GroupBox()
        Me.dgvSoftInst = New System.Windows.Forms.DataGridView()
        Me.prgInstSoftGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlInstSoftGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectInstSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectInstSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshInstSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportInstSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitInstSoftGrid = New System.Windows.Forms.Button()
        Me.pnlSqlDiscSoftGrid = New System.Windows.Forms.Panel()
        Me.grpDiscSoftGrid = New System.Windows.Forms.GroupBox()
        Me.lblDiscSoftGrid = New System.Windows.Forms.Label()
        Me.tabCtrlDiscSoftGrid = New System.Windows.Forms.TabControl()
        Me.tabDiscSignature = New System.Windows.Forms.TabPage()
        Me.dgvDiscSignature = New System.Windows.Forms.DataGridView()
        Me.tabDiscCustom = New System.Windows.Forms.TabPage()
        Me.dgvDiscCustom = New System.Windows.Forms.DataGridView()
        Me.tabDiscHeuristic = New System.Windows.Forms.TabPage()
        Me.dgvDiscHeuristic = New System.Windows.Forms.DataGridView()
        Me.tabDiscIntellisig = New System.Windows.Forms.TabPage()
        Me.dgvDiscIntellisig = New System.Windows.Forms.DataGridView()
        Me.tabDiscEverything = New System.Windows.Forms.TabPage()
        Me.dgvDiscEverything = New System.Windows.Forms.DataGridView()
        Me.prgDiscSoftGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlDiscSoftGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectDiscSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectDiscSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshDiscSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportDiscSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitDiscSoftGrid = New System.Windows.Forms.Button()
        Me.pnlSqlDuplCompGrid = New System.Windows.Forms.Panel()
        Me.tabCtrlDuplComp = New System.Windows.Forms.TabControl()
        Me.tabDuplHostname = New System.Windows.Forms.TabPage()
        Me.dgvDuplHostname = New System.Windows.Forms.DataGridView()
        Me.tabDuplSerialNum = New System.Windows.Forms.TabPage()
        Me.dgvDuplSerialNum = New System.Windows.Forms.DataGridView()
        Me.tabDuplBoth = New System.Windows.Forms.TabPage()
        Me.dgvDuplBoth = New System.Windows.Forms.DataGridView()
        Me.tabDuplBlank = New System.Windows.Forms.TabPage()
        Me.dgvDuplBlank = New System.Windows.Forms.DataGridView()
        Me.prgDuplCompGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlDuplCompGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectDuplCompGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectDuplCompGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshDuplCompGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportDuplCompGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitDuplCompGrid = New System.Windows.Forms.Button()
        Me.pnlSqlUnUsedSoftGrid = New System.Windows.Forms.Panel()
        Me.tabCtrlSwNotUsed = New System.Windows.Forms.TabControl()
        Me.tabSwNotUsed = New System.Windows.Forms.TabPage()
        Me.dgvSwNotUsed = New System.Windows.Forms.DataGridView()
        Me.tabSwNotInst = New System.Windows.Forms.TabPage()
        Me.dgvSwNotInst = New System.Windows.Forms.DataGridView()
        Me.tabSwNotStaged = New System.Windows.Forms.TabPage()
        Me.dgvSwNotStaged = New System.Windows.Forms.DataGridView()
        Me.prgUnUsedSoftGrid = New System.Windows.Forms.ProgressBar()
        Me.pnlSqlUnUsedSoftGridButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectUnUsedSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectUnUsedSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlRefreshUnUsedSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlExportUnUsedSoftGrid = New System.Windows.Forms.Button()
        Me.btnSqlExitUnUsedSoftGrid = New System.Windows.Forms.Button()
        Me.pnlSqlCleanApps = New System.Windows.Forms.Panel()
        Me.grpSqlCleanAppsInfo = New System.Windows.Forms.GroupBox()
        Me.lblSqlCleanAppsIntro = New System.Windows.Forms.Label()
        Me.grpSqlCleanAppsOutput = New System.Windows.Forms.GroupBox()
        Me.txtSqlCleanApps = New System.Windows.Forms.TextBox()
        Me.pnlSqlCleanAppsButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectCleanApps = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectCleanApps = New System.Windows.Forms.Button()
        Me.btnSqlCafStopCleanApps = New System.Windows.Forms.Button()
        Me.btnSqlRunCleanApps = New System.Windows.Forms.Button()
        Me.btnSqlExitCleanApps = New System.Windows.Forms.Button()
        Me.pnlSqlQueryEditor = New System.Windows.Forms.Panel()
        Me.grpSqlQuery = New System.Windows.Forms.GroupBox()
        Me.rtbSqlQuery = New System.Windows.Forms.RichTextBox()
        Me.TabControlSql = New System.Windows.Forms.TabControl()
        Me.TabPageMessages = New System.Windows.Forms.TabPage()
        Me.txtSqlMessage = New System.Windows.Forms.TextBox()
        Me.TabPageGrid = New System.Windows.Forms.TabPage()
        Me.pnlSqlQueryEditorButtons = New System.Windows.Forms.Panel()
        Me.btnSqlConnectQueryEditor = New System.Windows.Forms.Button()
        Me.btnSqlDisconnectQueryEditor = New System.Windows.Forms.Button()
        Me.btnSqlSubmitQueryEditor = New System.Windows.Forms.Button()
        Me.btnSqlCancelQueryEditor = New System.Windows.Forms.Button()
        Me.btnSqlExitQueryEditor = New System.Windows.Forms.Button()
        Me.pnlENCOverdrive = New System.Windows.Forms.Panel()
        Me.grpEncStressStatus = New System.Windows.Forms.GroupBox()
        Me.dgvEncStressTable = New System.Windows.Forms.DataGridView()
        Me.grpEncStressTest = New System.Windows.Forms.GroupBox()
        Me.lblEncStressIntro = New System.Windows.Forms.Label()
        Me.grpEncStressPingType = New System.Windows.Forms.GroupBox()
        Me.rbnEncStressCafPing = New System.Windows.Forms.RadioButton()
        Me.rbnEncStressCamPing = New System.Windows.Forms.RadioButton()
        Me.btnEncStressStop = New System.Windows.Forms.Button()
        Me.btnEncStressStart = New System.Windows.Forms.Button()
        Me.grpEncStressPingSettings = New System.Windows.Forms.GroupBox()
        Me.btnEncStressDelayTimePlus = New System.Windows.Forms.Button()
        Me.txtEncStressDelayTime = New System.Windows.Forms.TextBox()
        Me.btnEncStressDelayTimeMinus = New System.Windows.Forms.Button()
        Me.lblEncStressDelayTime = New System.Windows.Forms.Label()
        Me.btnEncStressReplyTimeoutPlus = New System.Windows.Forms.Button()
        Me.btnEncStressReplyTimeoutMinus = New System.Windows.Forms.Button()
        Me.txtEncStressReplyTimeout = New System.Windows.Forms.TextBox()
        Me.lblEncStressReplyTimeout = New System.Windows.Forms.Label()
        Me.btnEncStressMsgSizePlus = New System.Windows.Forms.Button()
        Me.btnEncStressMsgSizeMinus = New System.Windows.Forms.Button()
        Me.btnEncStressNumPingPlus = New System.Windows.Forms.Button()
        Me.lblEncStressNumPings = New System.Windows.Forms.Label()
        Me.btnEncStressNumPingMinus = New System.Windows.Forms.Button()
        Me.btnEncStressPingThreadPlus = New System.Windows.Forms.Button()
        Me.btnEncStressPingThreadMinus = New System.Windows.Forms.Button()
        Me.btnEncStressTargetThreadPlus = New System.Windows.Forms.Button()
        Me.btnEncStressTargetThreadMinus = New System.Windows.Forms.Button()
        Me.txtEncStressMsgSize = New System.Windows.Forms.TextBox()
        Me.txtEncStressNumPings = New System.Windows.Forms.TextBox()
        Me.txtEncStressNumTargetThreads = New System.Windows.Forms.TextBox()
        Me.txtEncStressNumPingThreads = New System.Windows.Forms.TextBox()
        Me.lblEncStressMsgSize = New System.Windows.Forms.Label()
        Me.lblEncStressPingThreads = New System.Windows.Forms.Label()
        Me.lblEncStressTargetThreads = New System.Windows.Forms.Label()
        Me.pnlRemovalTool = New System.Windows.Forms.Panel()
        Me.grpRemovalTool = New System.Windows.Forms.GroupBox()
        Me.txtRemovalTool = New System.Windows.Forms.TextBox()
        Me.grpRemovalOptions = New System.Windows.Forms.GroupBox()
        Me.lblRemoveITCMCaution = New System.Windows.Forms.Label()
        Me.chkRetainHostUUID = New System.Windows.Forms.CheckBox()
        Me.rbnUninstallITCM = New System.Windows.Forms.RadioButton()
        Me.rbnRemoveITCM = New System.Windows.Forms.RadioButton()
        Me.pnlRemovalToolButtons = New System.Windows.Forms.Panel()
        Me.btnRemoveITCM = New System.Windows.Forms.Button()
        Me.btnExitRemoveITCM = New System.Windows.Forms.Button()
        Me.pnlDebug = New System.Windows.Forms.Panel()
        Me.rtbDebug = New System.Windows.Forms.RichTextBox()
        Me.pnlSystemInfo.SuspendLayout()
        Me.grpInfo.SuspendLayout()
        Me.pnlWinOfflineMain.SuspendLayout()
        Me.grpPatchView.SuspendLayout()
        Me.pnlWinOfflineButton1.SuspendLayout()
        CType(Me.SplitWinOfflineUI, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitWinOfflineUI.Panel1.SuspendLayout()
        Me.SplitWinOfflineUI.Panel2.SuspendLayout()
        Me.SplitWinOfflineUI.SuspendLayout()
        Me.pnlWinOfflineStart.SuspendLayout()
        Me.grpWinOfflineWelcome.SuspendLayout()
        Me.grpWinOfflineStartOptions.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpHistory.SuspendLayout()
        Me.pnlWinOfflineRemove.SuspendLayout()
        Me.grpHistoryView.SuspendLayout()
        Me.pnlWinOfflineButton2.SuspendLayout()
        Me.pnlWinOfflineSDHelp.SuspendLayout()
        Me.grpSDHelp.SuspendLayout()
        CType(Me.picSteps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlWinOfflineCLIHelp.SuspendLayout()
        Me.grpCLIOptions.SuspendLayout()
        Me.pnlSqlMdbOverview.SuspendLayout()
        Me.grpSqlMdbOverview.SuspendLayout()
        Me.grpSqlMdbItcmSum.SuspendLayout()
        Me.grpSqlMdbAgentVersion.SuspendLayout()
        CType(Me.dgvAgentVersion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSqlMdbDefSummary.SuspendLayout()
        CType(Me.dgvContentSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlMdbOverviewButtons.SuspendLayout()
        Me.pnlSqlTableSpaceGrid.SuspendLayout()
        Me.grpSqlTableSpace.SuspendLayout()
        CType(Me.dgvTableSpaceGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlTableSpaceGridButtons.SuspendLayout()
        Me.pnlSqlUserGrid.SuspendLayout()
        Me.tabCtrlUserGrid.SuspendLayout()
        Me.tabUserSummary.SuspendLayout()
        CType(Me.dgvUserGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabUserObsolete90.SuspendLayout()
        CType(Me.dgvUserObsolete90, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabUserObsolete365.SuspendLayout()
        CType(Me.dgvUserObsolete365, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlUserGridButtons.SuspendLayout()
        Me.pnlSqlAgentGrid.SuspendLayout()
        Me.tabCtrlAgentGrid.SuspendLayout()
        Me.tabAgentSummary.SuspendLayout()
        CType(Me.dgvAgentGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabAgentObsolete90.SuspendLayout()
        CType(Me.dgvAgentObsolete90, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabAgentObsolete365.SuspendLayout()
        CType(Me.dgvAgentObsolete365, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlAgentGridButtons.SuspendLayout()
        Me.pnlSqlServerGrid.SuspendLayout()
        Me.tabCtrlServerGrid.SuspendLayout()
        Me.tabServerSummary.SuspendLayout()
        CType(Me.dgvServerGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabServerLastCollected24.SuspendLayout()
        CType(Me.dgvServerLastCollected24, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabServerSignature30.SuspendLayout()
        CType(Me.dgvServerSignature30, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlServerGridButtons.SuspendLayout()
        Me.pnlSqlEngineGrid.SuspendLayout()
        Me.grpSqlEngineGrid.SuspendLayout()
        CType(Me.dgvEngineGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlEngineGridButtons.SuspendLayout()
        Me.pnlSqlGroupEvalGrid.SuspendLayout()
        Me.grpSqlGroupEvalGrid.SuspendLayout()
        CType(Me.dgvGroupEvalGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSqlGroupEvalGridInst.SuspendLayout()
        Me.pnlSqlGroupEvalGridButtons.SuspendLayout()
        Me.pnlSqlInstSoftGrid.SuspendLayout()
        Me.grpSqlInstSoftGrid.SuspendLayout()
        CType(Me.dgvSoftInst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlInstSoftGridButtons.SuspendLayout()
        Me.pnlSqlDiscSoftGrid.SuspendLayout()
        Me.grpDiscSoftGrid.SuspendLayout()
        Me.tabCtrlDiscSoftGrid.SuspendLayout()
        Me.tabDiscSignature.SuspendLayout()
        CType(Me.dgvDiscSignature, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDiscCustom.SuspendLayout()
        CType(Me.dgvDiscCustom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDiscHeuristic.SuspendLayout()
        CType(Me.dgvDiscHeuristic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDiscIntellisig.SuspendLayout()
        CType(Me.dgvDiscIntellisig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDiscEverything.SuspendLayout()
        CType(Me.dgvDiscEverything, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlDiscSoftGridButtons.SuspendLayout()
        Me.pnlSqlDuplCompGrid.SuspendLayout()
        Me.tabCtrlDuplComp.SuspendLayout()
        Me.tabDuplHostname.SuspendLayout()
        CType(Me.dgvDuplHostname, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDuplSerialNum.SuspendLayout()
        CType(Me.dgvDuplSerialNum, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDuplBoth.SuspendLayout()
        CType(Me.dgvDuplBoth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDuplBlank.SuspendLayout()
        CType(Me.dgvDuplBlank, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlDuplCompGridButtons.SuspendLayout()
        Me.pnlSqlUnUsedSoftGrid.SuspendLayout()
        Me.tabCtrlSwNotUsed.SuspendLayout()
        Me.tabSwNotUsed.SuspendLayout()
        CType(Me.dgvSwNotUsed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSwNotInst.SuspendLayout()
        CType(Me.dgvSwNotInst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSwNotStaged.SuspendLayout()
        CType(Me.dgvSwNotStaged, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSqlUnUsedSoftGridButtons.SuspendLayout()
        Me.pnlSqlCleanApps.SuspendLayout()
        Me.grpSqlCleanAppsInfo.SuspendLayout()
        Me.grpSqlCleanAppsOutput.SuspendLayout()
        Me.pnlSqlCleanAppsButtons.SuspendLayout()
        Me.pnlSqlQueryEditor.SuspendLayout()
        Me.grpSqlQuery.SuspendLayout()
        Me.TabControlSql.SuspendLayout()
        Me.TabPageMessages.SuspendLayout()
        Me.pnlSqlQueryEditorButtons.SuspendLayout()
        Me.pnlENCOverdrive.SuspendLayout()
        Me.grpEncStressStatus.SuspendLayout()
        CType(Me.dgvEncStressTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEncStressTest.SuspendLayout()
        Me.grpEncStressPingType.SuspendLayout()
        Me.grpEncStressPingSettings.SuspendLayout()
        Me.pnlRemovalTool.SuspendLayout()
        Me.grpRemovalTool.SuspendLayout()
        Me.grpRemovalOptions.SuspendLayout()
        Me.pnlRemovalToolButtons.SuspendLayout()
        Me.pnlDebug.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSystemInfo
        '
        Me.pnlSystemInfo.AutoScroll = True
        Me.pnlSystemInfo.Controls.Add(Me.grpInfo)
        Me.pnlSystemInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSystemInfo.Location = New System.Drawing.Point(0, 0)
        Me.pnlSystemInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSystemInfo.Name = "pnlSystemInfo"
        Me.pnlSystemInfo.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSystemInfo.TabIndex = 29
        '
        'grpInfo
        '
        Me.grpInfo.Controls.Add(Me.lblPlugins)
        Me.grpInfo.Controls.Add(Me.lstPlugins)
        Me.grpInfo.Controls.Add(Me.btnBrowseSSA)
        Me.grpInfo.Controls.Add(Me.btnBrowseDSM)
        Me.grpInfo.Controls.Add(Me.lblSSAFolder)
        Me.grpInfo.Controls.Add(Me.txtSSAFolder)
        Me.grpInfo.Controls.Add(Me.btnBrowseCAM)
        Me.grpInfo.Controls.Add(Me.lblCAMFolder)
        Me.grpInfo.Controls.Add(Me.txtCAMFolder)
        Me.grpInfo.Controls.Add(Me.lblScalability)
        Me.grpInfo.Controls.Add(Me.lblManager)
        Me.grpInfo.Controls.Add(Me.txtDomainMgr)
        Me.grpInfo.Controls.Add(Me.txtScalabilityServer)
        Me.grpInfo.Controls.Add(Me.txtHostUUID)
        Me.grpInfo.Controls.Add(Me.lblHostUUID)
        Me.grpInfo.Controls.Add(Me.txtFunction)
        Me.grpInfo.Controls.Add(Me.txtDSMFolder)
        Me.grpInfo.Controls.Add(Me.txtVersion)
        Me.grpInfo.Controls.Add(Me.lblInstallDir)
        Me.grpInfo.Controls.Add(Me.lblFunction)
        Me.grpInfo.Controls.Add(Me.lblVersion)
        Me.grpInfo.Controls.Add(Me.txtModel)
        Me.grpInfo.Controls.Add(Me.lblModel)
        Me.grpInfo.Controls.Add(Me.txtManufacturer)
        Me.grpInfo.Controls.Add(Me.lblManufacturer)
        Me.grpInfo.Controls.Add(Me.txtPlatform)
        Me.grpInfo.Controls.Add(Me.txtSerial)
        Me.grpInfo.Controls.Add(Me.lblHostname)
        Me.grpInfo.Controls.Add(Me.lblPlatform)
        Me.grpInfo.Controls.Add(Me.txtHostname)
        Me.grpInfo.Controls.Add(Me.lblSerial)
        Me.grpInfo.Controls.Add(Me.lblNetwork)
        Me.grpInfo.Controls.Add(Me.lstNetAddr)
        Me.grpInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpInfo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpInfo.Location = New System.Drawing.Point(0, 0)
        Me.grpInfo.Margin = New System.Windows.Forms.Padding(0)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Padding = New System.Windows.Forms.Padding(4)
        Me.grpInfo.Size = New System.Drawing.Size(1023, 698)
        Me.grpInfo.TabIndex = 22
        Me.grpInfo.TabStop = False
        Me.grpInfo.Text = "System Information"
        '
        'lblPlugins
        '
        Me.lblPlugins.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPlugins.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlugins.Location = New System.Drawing.Point(624, 525)
        Me.lblPlugins.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPlugins.Name = "lblPlugins"
        Me.lblPlugins.Size = New System.Drawing.Size(90, 60)
        Me.lblPlugins.TabIndex = 61
        Me.lblPlugins.Text = "Installed Plugins:"
        '
        'lstPlugins
        '
        Me.lstPlugins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstPlugins.BackColor = System.Drawing.Color.Beige
        Me.lstPlugins.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstPlugins.FormattingEnabled = True
        Me.lstPlugins.ItemHeight = 23
        Me.lstPlugins.Location = New System.Drawing.Point(715, 525)
        Me.lstPlugins.Margin = New System.Windows.Forms.Padding(4)
        Me.lstPlugins.Name = "lstPlugins"
        Me.lstPlugins.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.lstPlugins.Size = New System.Drawing.Size(303, 119)
        Me.lstPlugins.TabIndex = 17
        Me.lstPlugins.TabStop = False
        '
        'btnBrowseSSA
        '
        Me.btnBrowseSSA.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseSSA.BackColor = System.Drawing.Color.SteelBlue
        Me.btnBrowseSSA.FlatAppearance.BorderSize = 0
        Me.btnBrowseSSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseSSA.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseSSA.ForeColor = System.Drawing.Color.White
        Me.btnBrowseSSA.Location = New System.Drawing.Point(903, 486)
        Me.btnBrowseSSA.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseSSA.Name = "btnBrowseSSA"
        Me.btnBrowseSSA.Size = New System.Drawing.Size(115, 34)
        Me.btnBrowseSSA.TabIndex = 15
        Me.btnBrowseSSA.Text = "Go there..."
        Me.btnBrowseSSA.UseVisualStyleBackColor = False
        '
        'btnBrowseDSM
        '
        Me.btnBrowseDSM.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseDSM.BackColor = System.Drawing.Color.SteelBlue
        Me.btnBrowseDSM.FlatAppearance.BorderSize = 0
        Me.btnBrowseDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseDSM.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseDSM.ForeColor = System.Drawing.Color.White
        Me.btnBrowseDSM.Location = New System.Drawing.Point(903, 411)
        Me.btnBrowseDSM.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseDSM.Name = "btnBrowseDSM"
        Me.btnBrowseDSM.Size = New System.Drawing.Size(115, 34)
        Me.btnBrowseDSM.TabIndex = 11
        Me.btnBrowseDSM.Text = "Go there..."
        Me.btnBrowseDSM.UseVisualStyleBackColor = False
        '
        'lblSSAFolder
        '
        Me.lblSSAFolder.AutoSize = True
        Me.lblSSAFolder.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSSAFolder.Location = New System.Drawing.Point(130, 489)
        Me.lblSSAFolder.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSSAFolder.Name = "lblSSAFolder"
        Me.lblSSAFolder.Size = New System.Drawing.Size(97, 23)
        Me.lblSSAFolder.TabIndex = 58
        Me.lblSSAFolder.Text = "SSA Folder:"
        Me.lblSSAFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSSAFolder
        '
        Me.txtSSAFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSSAFolder.BackColor = System.Drawing.Color.Beige
        Me.txtSSAFolder.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSSAFolder.Location = New System.Drawing.Point(272, 485)
        Me.txtSSAFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSSAFolder.Name = "txtSSAFolder"
        Me.txtSSAFolder.ReadOnly = True
        Me.txtSSAFolder.Size = New System.Drawing.Size(622, 30)
        Me.txtSSAFolder.TabIndex = 14
        Me.txtSSAFolder.Text = "SSA Folder"
        '
        'btnBrowseCAM
        '
        Me.btnBrowseCAM.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseCAM.BackColor = System.Drawing.Color.SteelBlue
        Me.btnBrowseCAM.FlatAppearance.BorderSize = 0
        Me.btnBrowseCAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseCAM.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseCAM.ForeColor = System.Drawing.Color.White
        Me.btnBrowseCAM.Location = New System.Drawing.Point(903, 449)
        Me.btnBrowseCAM.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBrowseCAM.Name = "btnBrowseCAM"
        Me.btnBrowseCAM.Size = New System.Drawing.Size(115, 34)
        Me.btnBrowseCAM.TabIndex = 13
        Me.btnBrowseCAM.Text = "Go there..."
        Me.btnBrowseCAM.UseVisualStyleBackColor = False
        '
        'lblCAMFolder
        '
        Me.lblCAMFolder.AutoSize = True
        Me.lblCAMFolder.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCAMFolder.Location = New System.Drawing.Point(122, 451)
        Me.lblCAMFolder.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCAMFolder.Name = "lblCAMFolder"
        Me.lblCAMFolder.Size = New System.Drawing.Size(105, 23)
        Me.lblCAMFolder.TabIndex = 55
        Me.lblCAMFolder.Text = "CAM Folder:"
        Me.lblCAMFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCAMFolder
        '
        Me.txtCAMFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCAMFolder.BackColor = System.Drawing.Color.Beige
        Me.txtCAMFolder.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCAMFolder.Location = New System.Drawing.Point(272, 448)
        Me.txtCAMFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCAMFolder.Name = "txtCAMFolder"
        Me.txtCAMFolder.ReadOnly = True
        Me.txtCAMFolder.Size = New System.Drawing.Size(622, 30)
        Me.txtCAMFolder.TabIndex = 12
        Me.txtCAMFolder.Text = "CAM Folder"
        '
        'lblScalability
        '
        Me.lblScalability.AutoSize = True
        Me.lblScalability.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScalability.Location = New System.Drawing.Point(134, 376)
        Me.lblScalability.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblScalability.Name = "lblScalability"
        Me.lblScalability.Size = New System.Drawing.Size(91, 23)
        Me.lblScalability.TabIndex = 53
        Me.lblScalability.Text = "Scalability:"
        Me.lblScalability.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblManager
        '
        Me.lblManager.AutoSize = True
        Me.lblManager.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManager.Location = New System.Drawing.Point(145, 339)
        Me.lblManager.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblManager.Name = "lblManager"
        Me.lblManager.Size = New System.Drawing.Size(84, 23)
        Me.lblManager.TabIndex = 52
        Me.lblManager.Text = "Manager:"
        Me.lblManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDomainMgr
        '
        Me.txtDomainMgr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDomainMgr.BackColor = System.Drawing.Color.Beige
        Me.txtDomainMgr.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDomainMgr.Location = New System.Drawing.Point(272, 335)
        Me.txtDomainMgr.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDomainMgr.Name = "txtDomainMgr"
        Me.txtDomainMgr.ReadOnly = True
        Me.txtDomainMgr.Size = New System.Drawing.Size(744, 30)
        Me.txtDomainMgr.TabIndex = 8
        Me.txtDomainMgr.Text = "Domain Manager"
        '
        'txtScalabilityServer
        '
        Me.txtScalabilityServer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScalabilityServer.BackColor = System.Drawing.Color.Beige
        Me.txtScalabilityServer.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScalabilityServer.Location = New System.Drawing.Point(272, 372)
        Me.txtScalabilityServer.Margin = New System.Windows.Forms.Padding(4)
        Me.txtScalabilityServer.Name = "txtScalabilityServer"
        Me.txtScalabilityServer.ReadOnly = True
        Me.txtScalabilityServer.Size = New System.Drawing.Size(744, 30)
        Me.txtScalabilityServer.TabIndex = 9
        Me.txtScalabilityServer.Text = "Scalability Server"
        '
        'txtHostUUID
        '
        Me.txtHostUUID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHostUUID.BackColor = System.Drawing.Color.Beige
        Me.txtHostUUID.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHostUUID.Location = New System.Drawing.Point(272, 298)
        Me.txtHostUUID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtHostUUID.Name = "txtHostUUID"
        Me.txtHostUUID.ReadOnly = True
        Me.txtHostUUID.Size = New System.Drawing.Size(744, 30)
        Me.txtHostUUID.TabIndex = 7
        Me.txtHostUUID.Text = "HostUUID"
        Me.txtHostUUID.WordWrap = False
        '
        'lblHostUUID
        '
        Me.lblHostUUID.AutoSize = True
        Me.lblHostUUID.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHostUUID.Location = New System.Drawing.Point(135, 301)
        Me.lblHostUUID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHostUUID.Name = "lblHostUUID"
        Me.lblHostUUID.Size = New System.Drawing.Size(97, 23)
        Me.lblHostUUID.TabIndex = 47
        Me.lblHostUUID.Text = "Host UUID:"
        Me.lblHostUUID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFunction
        '
        Me.txtFunction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFunction.BackColor = System.Drawing.Color.Beige
        Me.txtFunction.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFunction.Location = New System.Drawing.Point(272, 222)
        Me.txtFunction.Margin = New System.Windows.Forms.Padding(4)
        Me.txtFunction.Name = "txtFunction"
        Me.txtFunction.ReadOnly = True
        Me.txtFunction.Size = New System.Drawing.Size(744, 30)
        Me.txtFunction.TabIndex = 5
        Me.txtFunction.Text = "Client Function Here"
        Me.txtFunction.WordWrap = False
        '
        'txtDSMFolder
        '
        Me.txtDSMFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDSMFolder.BackColor = System.Drawing.Color.Beige
        Me.txtDSMFolder.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDSMFolder.Location = New System.Drawing.Point(272, 410)
        Me.txtDSMFolder.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDSMFolder.Name = "txtDSMFolder"
        Me.txtDSMFolder.ReadOnly = True
        Me.txtDSMFolder.Size = New System.Drawing.Size(622, 30)
        Me.txtDSMFolder.TabIndex = 10
        Me.txtDSMFolder.Text = "DSM Folder"
        Me.txtDSMFolder.WordWrap = False
        '
        'txtVersion
        '
        Me.txtVersion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVersion.BackColor = System.Drawing.Color.Beige
        Me.txtVersion.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVersion.Location = New System.Drawing.Point(272, 260)
        Me.txtVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.txtVersion.Name = "txtVersion"
        Me.txtVersion.ReadOnly = True
        Me.txtVersion.Size = New System.Drawing.Size(744, 30)
        Me.txtVersion.TabIndex = 6
        Me.txtVersion.Text = "Agent Version Here"
        Me.txtVersion.WordWrap = False
        '
        'lblInstallDir
        '
        Me.lblInstallDir.AutoSize = True
        Me.lblInstallDir.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstallDir.Location = New System.Drawing.Point(124, 415)
        Me.lblInstallDir.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblInstallDir.Name = "lblInstallDir"
        Me.lblInstallDir.Size = New System.Drawing.Size(105, 23)
        Me.lblInstallDir.TabIndex = 43
        Me.lblInstallDir.Text = "DSM Folder:"
        Me.lblInstallDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFunction
        '
        Me.lblFunction.AutoSize = True
        Me.lblFunction.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFunction.Location = New System.Drawing.Point(121, 226)
        Me.lblFunction.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFunction.Name = "lblFunction"
        Me.lblFunction.Size = New System.Drawing.Size(108, 23)
        Me.lblFunction.TabIndex = 41
        Me.lblFunction.Text = "CA Function:"
        Me.lblFunction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(129, 264)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVersion.Size = New System.Drawing.Size(98, 23)
        Me.lblVersion.TabIndex = 42
        Me.lblVersion.Text = "CA Version:"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtModel
        '
        Me.txtModel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtModel.BackColor = System.Drawing.Color.Beige
        Me.txtModel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModel.Location = New System.Drawing.Point(272, 148)
        Me.txtModel.Margin = New System.Windows.Forms.Padding(4)
        Me.txtModel.Name = "txtModel"
        Me.txtModel.ReadOnly = True
        Me.txtModel.Size = New System.Drawing.Size(744, 30)
        Me.txtModel.TabIndex = 3
        Me.txtModel.Text = "Model"
        '
        'lblModel
        '
        Me.lblModel.AutoSize = True
        Me.lblModel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModel.Location = New System.Drawing.Point(162, 151)
        Me.lblModel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblModel.Name = "lblModel"
        Me.lblModel.Size = New System.Drawing.Size(64, 23)
        Me.lblModel.TabIndex = 39
        Me.lblModel.Text = "Model:"
        Me.lblModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtManufacturer
        '
        Me.txtManufacturer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtManufacturer.BackColor = System.Drawing.Color.Beige
        Me.txtManufacturer.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtManufacturer.Location = New System.Drawing.Point(272, 110)
        Me.txtManufacturer.Margin = New System.Windows.Forms.Padding(4)
        Me.txtManufacturer.Name = "txtManufacturer"
        Me.txtManufacturer.ReadOnly = True
        Me.txtManufacturer.Size = New System.Drawing.Size(744, 30)
        Me.txtManufacturer.TabIndex = 2
        Me.txtManufacturer.Text = "Manufacturer"
        '
        'lblManufacturer
        '
        Me.lblManufacturer.AutoSize = True
        Me.lblManufacturer.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManufacturer.Location = New System.Drawing.Point(108, 114)
        Me.lblManufacturer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblManufacturer.Name = "lblManufacturer"
        Me.lblManufacturer.Size = New System.Drawing.Size(122, 23)
        Me.lblManufacturer.TabIndex = 37
        Me.lblManufacturer.Text = "Manufacturer:"
        Me.lblManufacturer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPlatform
        '
        Me.txtPlatform.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPlatform.BackColor = System.Drawing.Color.Beige
        Me.txtPlatform.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlatform.Location = New System.Drawing.Point(272, 185)
        Me.txtPlatform.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPlatform.Name = "txtPlatform"
        Me.txtPlatform.ReadOnly = True
        Me.txtPlatform.Size = New System.Drawing.Size(744, 30)
        Me.txtPlatform.TabIndex = 4
        Me.txtPlatform.Text = "Operating System Platform Here"
        '
        'txtSerial
        '
        Me.txtSerial.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSerial.BackColor = System.Drawing.Color.Beige
        Me.txtSerial.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(272, 72)
        Me.txtSerial.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.ReadOnly = True
        Me.txtSerial.Size = New System.Drawing.Size(744, 30)
        Me.txtSerial.TabIndex = 1
        Me.txtSerial.Text = "Serial Number"
        '
        'lblHostname
        '
        Me.lblHostname.AutoSize = True
        Me.lblHostname.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHostname.Location = New System.Drawing.Point(134, 39)
        Me.lblHostname.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHostname.Name = "lblHostname"
        Me.lblHostname.Size = New System.Drawing.Size(94, 23)
        Me.lblHostname.TabIndex = 31
        Me.lblHostname.Text = "Hostname:"
        Me.lblHostname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPlatform
        '
        Me.lblPlatform.AutoSize = True
        Me.lblPlatform.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlatform.Location = New System.Drawing.Point(146, 189)
        Me.lblPlatform.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPlatform.Name = "lblPlatform"
        Me.lblPlatform.Size = New System.Drawing.Size(82, 23)
        Me.lblPlatform.TabIndex = 34
        Me.lblPlatform.Text = "Platform:"
        Me.lblPlatform.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtHostname
        '
        Me.txtHostname.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHostname.BackColor = System.Drawing.Color.Beige
        Me.txtHostname.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHostname.Location = New System.Drawing.Point(272, 35)
        Me.txtHostname.Margin = New System.Windows.Forms.Padding(4)
        Me.txtHostname.Name = "txtHostname"
        Me.txtHostname.ReadOnly = True
        Me.txtHostname.Size = New System.Drawing.Size(744, 30)
        Me.txtHostname.TabIndex = 0
        Me.txtHostname.Text = "Hostname"
        Me.txtHostname.WordWrap = False
        '
        'lblSerial
        '
        Me.lblSerial.AutoSize = True
        Me.lblSerial.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSerial.Location = New System.Drawing.Point(101, 76)
        Me.lblSerial.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSerial.Name = "lblSerial"
        Me.lblSerial.Size = New System.Drawing.Size(124, 23)
        Me.lblSerial.TabIndex = 33
        Me.lblSerial.Text = "Serial Number:"
        Me.lblSerial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNetwork
        '
        Me.lblNetwork.AutoSize = True
        Me.lblNetwork.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetwork.Location = New System.Drawing.Point(61, 525)
        Me.lblNetwork.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNetwork.Name = "lblNetwork"
        Me.lblNetwork.Size = New System.Drawing.Size(166, 23)
        Me.lblNetwork.TabIndex = 21
        Me.lblNetwork.Text = "Network Addresses:"
        Me.lblNetwork.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lstNetAddr
        '
        Me.lstNetAddr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstNetAddr.BackColor = System.Drawing.Color.Beige
        Me.lstNetAddr.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstNetAddr.FormattingEnabled = True
        Me.lstNetAddr.ItemHeight = 23
        Me.lstNetAddr.Location = New System.Drawing.Point(272, 525)
        Me.lstNetAddr.Margin = New System.Windows.Forms.Padding(4)
        Me.lstNetAddr.Name = "lstNetAddr"
        Me.lstNetAddr.Size = New System.Drawing.Size(349, 119)
        Me.lstNetAddr.TabIndex = 16
        Me.lstNetAddr.TabStop = False
        '
        'pnlWinOfflineMain
        '
        Me.pnlWinOfflineMain.AutoScroll = True
        Me.pnlWinOfflineMain.Controls.Add(Me.lvwApplyOptions)
        Me.pnlWinOfflineMain.Controls.Add(Me.grpPatchView)
        Me.pnlWinOfflineMain.Controls.Add(Me.pnlWinOfflineButton1)
        Me.pnlWinOfflineMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWinOfflineMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlWinOfflineMain.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineMain.Name = "pnlWinOfflineMain"
        Me.pnlWinOfflineMain.Size = New System.Drawing.Size(1023, 698)
        Me.pnlWinOfflineMain.TabIndex = 27
        '
        'lvwApplyOptions
        '
        Me.lvwApplyOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwApplyOptions.BackColor = System.Drawing.Color.Beige
        Me.lvwApplyOptions.CheckBoxes = True
        Me.lvwApplyOptions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnOptions})
        Me.lvwApplyOptions.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwApplyOptions.GridLines = True
        Me.lvwApplyOptions.Location = New System.Drawing.Point(0, 0)
        Me.lvwApplyOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.lvwApplyOptions.Name = "lvwApplyOptions"
        Me.lvwApplyOptions.Size = New System.Drawing.Size(969, 394)
        Me.lvwApplyOptions.TabIndex = 28
        Me.lvwApplyOptions.UseCompatibleStateImageBehavior = False
        Me.lvwApplyOptions.View = System.Windows.Forms.View.Details
        '
        'ColumnOptions
        '
        Me.ColumnOptions.Text = "Advanced Options"
        Me.ColumnOptions.Width = 616
        '
        'grpPatchView
        '
        Me.grpPatchView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpPatchView.Controls.Add(Me.lvwPatchList)
        Me.grpPatchView.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpPatchView.Location = New System.Drawing.Point(0, 394)
        Me.grpPatchView.Margin = New System.Windows.Forms.Padding(4)
        Me.grpPatchView.Name = "grpPatchView"
        Me.grpPatchView.Padding = New System.Windows.Forms.Padding(4)
        Me.grpPatchView.Size = New System.Drawing.Size(973, 195)
        Me.grpPatchView.TabIndex = 30
        Me.grpPatchView.TabStop = False
        Me.grpPatchView.Text = "Working Directory Scan:"
        '
        'lvwPatchList
        '
        Me.lvwPatchList.BackColor = System.Drawing.Color.Beige
        Me.lvwPatchList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnPatch, Me.ColumnCode, Me.ColumnStatus})
        Me.lvwPatchList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwPatchList.GridLines = True
        Me.lvwPatchList.Location = New System.Drawing.Point(4, 27)
        Me.lvwPatchList.Margin = New System.Windows.Forms.Padding(4)
        Me.lvwPatchList.Name = "lvwPatchList"
        Me.lvwPatchList.Size = New System.Drawing.Size(965, 164)
        Me.lvwPatchList.TabIndex = 32
        Me.lvwPatchList.UseCompatibleStateImageBehavior = False
        Me.lvwPatchList.View = System.Windows.Forms.View.Details
        '
        'ColumnPatch
        '
        Me.ColumnPatch.Text = "Patch"
        Me.ColumnPatch.Width = 197
        '
        'ColumnCode
        '
        Me.ColumnCode.Text = "Code"
        Me.ColumnCode.Width = 140
        '
        'ColumnStatus
        '
        Me.ColumnStatus.Text = "Status"
        Me.ColumnStatus.Width = 272
        '
        'pnlWinOfflineButton1
        '
        Me.pnlWinOfflineButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.pnlWinOfflineButton1.Controls.Add(Me.btnWinOfflineStart1)
        Me.pnlWinOfflineButton1.Controls.Add(Me.btnWinOfflineExit3)
        Me.pnlWinOfflineButton1.Controls.Add(Me.btnWinOfflineBack3)
        Me.pnlWinOfflineButton1.Location = New System.Drawing.Point(149, 605)
        Me.pnlWinOfflineButton1.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineButton1.Name = "pnlWinOfflineButton1"
        Me.pnlWinOfflineButton1.Size = New System.Drawing.Size(674, 75)
        Me.pnlWinOfflineButton1.TabIndex = 31
        '
        'btnWinOfflineStart1
        '
        Me.btnWinOfflineStart1.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnWinOfflineStart1.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineStart1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineStart1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineStart1.ForeColor = System.Drawing.SystemColors.Window
        Me.btnWinOfflineStart1.Location = New System.Drawing.Point(228, 15)
        Me.btnWinOfflineStart1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineStart1.Name = "btnWinOfflineStart1"
        Me.btnWinOfflineStart1.Size = New System.Drawing.Size(219, 48)
        Me.btnWinOfflineStart1.TabIndex = 0
        Me.btnWinOfflineStart1.Text = "Start"
        Me.btnWinOfflineStart1.UseVisualStyleBackColor = False
        '
        'btnWinOfflineExit3
        '
        Me.btnWinOfflineExit3.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineExit3.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineExit3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineExit3.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineExit3.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineExit3.Location = New System.Drawing.Point(509, 20)
        Me.btnWinOfflineExit3.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineExit3.Name = "btnWinOfflineExit3"
        Me.btnWinOfflineExit3.Size = New System.Drawing.Size(158, 35)
        Me.btnWinOfflineExit3.TabIndex = 1
        Me.btnWinOfflineExit3.Text = "Exit"
        Me.btnWinOfflineExit3.UseVisualStyleBackColor = False
        '
        'btnWinOfflineBack3
        '
        Me.btnWinOfflineBack3.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineBack3.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineBack3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineBack3.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineBack3.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineBack3.Location = New System.Drawing.Point(8, 21)
        Me.btnWinOfflineBack3.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineBack3.Name = "btnWinOfflineBack3"
        Me.btnWinOfflineBack3.Size = New System.Drawing.Size(158, 35)
        Me.btnWinOfflineBack3.TabIndex = 2
        Me.btnWinOfflineBack3.Text = "Go Back..."
        Me.btnWinOfflineBack3.UseVisualStyleBackColor = False
        '
        'ExplorerTree
        '
        Me.ExplorerTree.BackColor = System.Drawing.Color.Beige
        Me.ExplorerTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExplorerTree.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExplorerTree.HideSelection = False
        Me.ExplorerTree.Location = New System.Drawing.Point(0, 0)
        Me.ExplorerTree.Margin = New System.Windows.Forms.Padding(4)
        Me.ExplorerTree.Name = "ExplorerTree"
        TreeNode1.Name = "WinOfflineNode"
        TreeNode1.Text = "Patch Maintenance"
        TreeNode2.Name = "SqlMdbOverviewNode"
        TreeNode2.Text = "Mdb Overview"
        TreeNode3.Name = "SqlTableSpaceGridNode"
        TreeNode3.Text = "Table Space Summary"
        TreeNode4.Name = "AgentGridNode"
        TreeNode4.Text = "Agent Summary"
        TreeNode5.Name = "UserGridNode"
        TreeNode5.Text = "User Summary"
        TreeNode6.Name = "ServerGridNode"
        TreeNode6.Text = "Scalability Summary"
        TreeNode7.Name = "EngineGridNode"
        TreeNode7.Text = "Engine Summary"
        TreeNode8.Name = "GroupEvalGridNode"
        TreeNode8.Text = "Group Evaluations"
        TreeNode9.Name = "InstSoftGridNode"
        TreeNode9.Text = "Installed Software"
        TreeNode10.Name = "DiscSoftGridNode"
        TreeNode10.Text = "Discovered Software"
        TreeNode11.Name = "UnUsedSoftGridNode"
        TreeNode11.Text = "Unused Software"
        TreeNode12.Name = "DuplCompGridNode"
        TreeNode12.Text = "Duplicate Computers"
        TreeNode13.Name = "SqlCleanAppsNode"
        TreeNode13.Text = "CleanApps Script"
        TreeNode14.Name = "SqlQueryNode"
        TreeNode14.Text = "Query Editor"
        TreeNode15.Name = "SqlToolsNode"
        TreeNode15.Text = "Database Tools"
        TreeNode16.Name = "EncStressNode"
        TreeNode16.Text = "ENC Ping Utility"
        TreeNode17.Name = "EncToolsNode"
        TreeNode17.Text = "ENC Tools"
        TreeNode18.Name = "RemovalToolNode"
        TreeNode18.Text = "Removal Tool"
        TreeNode19.Name = "SysInfoNode"
        TreeNode19.Text = "System Information"
        TreeNode20.Name = "DebugNode"
        TreeNode20.Text = "Debug Window"
        TreeNode21.Name = "RootNode"
        TreeNode21.Text = "WinOffline Explorer"
        Me.ExplorerTree.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode21})
        Me.ExplorerTree.Size = New System.Drawing.Size(261, 698)
        Me.ExplorerTree.TabIndex = 26
        Me.ExplorerTree.TabStop = False
        '
        'SplitWinOfflineUI
        '
        Me.SplitWinOfflineUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitWinOfflineUI.IsSplitterFixed = True
        Me.SplitWinOfflineUI.Location = New System.Drawing.Point(0, 0)
        Me.SplitWinOfflineUI.Margin = New System.Windows.Forms.Padding(4)
        Me.SplitWinOfflineUI.Name = "SplitWinOfflineUI"
        '
        'SplitWinOfflineUI.Panel1
        '
        Me.SplitWinOfflineUI.Panel1.Controls.Add(Me.ExplorerTree)
        Me.SplitWinOfflineUI.Panel1MinSize = 209
        '
        'SplitWinOfflineUI.Panel2
        '
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlWinOfflineStart)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlWinOfflineMain)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlWinOfflineRemove)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlWinOfflineSDHelp)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlWinOfflineCLIHelp)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlMdbOverview)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlTableSpaceGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlUserGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlAgentGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlServerGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlEngineGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlGroupEvalGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlInstSoftGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlDiscSoftGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlDuplCompGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlUnUsedSoftGrid)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlCleanApps)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSqlQueryEditor)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlENCOverdrive)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlRemovalTool)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlSystemInfo)
        Me.SplitWinOfflineUI.Panel2.Controls.Add(Me.pnlDebug)
        Me.SplitWinOfflineUI.Size = New System.Drawing.Size(1288, 698)
        Me.SplitWinOfflineUI.SplitterDistance = 261
        Me.SplitWinOfflineUI.TabIndex = 22
        Me.SplitWinOfflineUI.TabStop = False
        '
        'pnlWinOfflineStart
        '
        Me.pnlWinOfflineStart.AutoScroll = True
        Me.pnlWinOfflineStart.Controls.Add(Me.grpWinOfflineWelcome)
        Me.pnlWinOfflineStart.Controls.Add(Me.grpHistory)
        Me.pnlWinOfflineStart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWinOfflineStart.Location = New System.Drawing.Point(0, 0)
        Me.pnlWinOfflineStart.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineStart.Name = "pnlWinOfflineStart"
        Me.pnlWinOfflineStart.Size = New System.Drawing.Size(1023, 698)
        Me.pnlWinOfflineStart.TabIndex = 30
        '
        'grpWinOfflineWelcome
        '
        Me.grpWinOfflineWelcome.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpWinOfflineWelcome.Controls.Add(Me.btnWinOfflineNext1)
        Me.grpWinOfflineWelcome.Controls.Add(Me.btnWinOfflineExit1)
        Me.grpWinOfflineWelcome.Controls.Add(Me.grpWinOfflineStartOptions)
        Me.grpWinOfflineWelcome.Controls.Add(Me.PictureBox1)
        Me.grpWinOfflineWelcome.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpWinOfflineWelcome.Location = New System.Drawing.Point(0, 0)
        Me.grpWinOfflineWelcome.Margin = New System.Windows.Forms.Padding(0)
        Me.grpWinOfflineWelcome.Name = "grpWinOfflineWelcome"
        Me.grpWinOfflineWelcome.Padding = New System.Windows.Forms.Padding(4)
        Me.grpWinOfflineWelcome.Size = New System.Drawing.Size(1023, 292)
        Me.grpWinOfflineWelcome.TabIndex = 27
        Me.grpWinOfflineWelcome.TabStop = False
        Me.grpWinOfflineWelcome.Text = "Patch Maintenance"
        '
        'btnWinOfflineNext1
        '
        Me.btnWinOfflineNext1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineNext1.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineNext1.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineNext1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineNext1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineNext1.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineNext1.Location = New System.Drawing.Point(853, 195)
        Me.btnWinOfflineNext1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineNext1.Name = "btnWinOfflineNext1"
        Me.btnWinOfflineNext1.Size = New System.Drawing.Size(158, 38)
        Me.btnWinOfflineNext1.TabIndex = 4
        Me.btnWinOfflineNext1.Text = "&Next"
        Me.btnWinOfflineNext1.UseVisualStyleBackColor = False
        '
        'btnWinOfflineExit1
        '
        Me.btnWinOfflineExit1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineExit1.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineExit1.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineExit1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineExit1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineExit1.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineExit1.Location = New System.Drawing.Point(853, 241)
        Me.btnWinOfflineExit1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineExit1.Name = "btnWinOfflineExit1"
        Me.btnWinOfflineExit1.Size = New System.Drawing.Size(158, 38)
        Me.btnWinOfflineExit1.TabIndex = 5
        Me.btnWinOfflineExit1.Text = "E&xit"
        Me.btnWinOfflineExit1.UseVisualStyleBackColor = False
        '
        'grpWinOfflineStartOptions
        '
        Me.grpWinOfflineStartOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpWinOfflineStartOptions.Controls.Add(Me.rbnCLIHelp)
        Me.grpWinOfflineStartOptions.Controls.Add(Me.rbnBackout)
        Me.grpWinOfflineStartOptions.Controls.Add(Me.rbnLearn)
        Me.grpWinOfflineStartOptions.Controls.Add(Me.rbnApply)
        Me.grpWinOfflineStartOptions.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpWinOfflineStartOptions.Location = New System.Drawing.Point(25, 34)
        Me.grpWinOfflineStartOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.grpWinOfflineStartOptions.Name = "grpWinOfflineStartOptions"
        Me.grpWinOfflineStartOptions.Padding = New System.Windows.Forms.Padding(4)
        Me.grpWinOfflineStartOptions.Size = New System.Drawing.Size(819, 246)
        Me.grpWinOfflineStartOptions.TabIndex = 4
        Me.grpWinOfflineStartOptions.TabStop = False
        Me.grpWinOfflineStartOptions.Text = "What do you want to do?"
        '
        'rbnCLIHelp
        '
        Me.rbnCLIHelp.AutoSize = True
        Me.rbnCLIHelp.Location = New System.Drawing.Point(21, 195)
        Me.rbnCLIHelp.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnCLIHelp.Name = "rbnCLIHelp"
        Me.rbnCLIHelp.Size = New System.Drawing.Size(337, 27)
        Me.rbnCLIHelp.TabIndex = 3
        Me.rbnCLIHelp.TabStop = True
        Me.rbnCLIHelp.Text = "Help and command line interface usage."
        Me.rbnCLIHelp.UseVisualStyleBackColor = True
        '
        'rbnBackout
        '
        Me.rbnBackout.AutoSize = True
        Me.rbnBackout.Location = New System.Drawing.Point(21, 91)
        Me.rbnBackout.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnBackout.Name = "rbnBackout"
        Me.rbnBackout.Size = New System.Drawing.Size(408, 27)
        Me.rbnBackout.TabIndex = 1
        Me.rbnBackout.Text = "Remove ITCM patches installed on this computer."
        Me.rbnBackout.UseVisualStyleBackColor = True
        '
        'rbnLearn
        '
        Me.rbnLearn.AutoSize = True
        Me.rbnLearn.Location = New System.Drawing.Point(21, 142)
        Me.rbnLearn.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnLearn.Name = "rbnLearn"
        Me.rbnLearn.Size = New System.Drawing.Size(561, 27)
        Me.rbnLearn.TabIndex = 2
        Me.rbnLearn.Text = "Learn how to apply or remove ITCM patches using Software Delivery."
        Me.rbnLearn.UseVisualStyleBackColor = True
        '
        'rbnApply
        '
        Me.rbnApply.AutoSize = True
        Me.rbnApply.Checked = True
        Me.rbnApply.Location = New System.Drawing.Point(21, 40)
        Me.rbnApply.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnApply.Name = "rbnApply"
        Me.rbnApply.Size = New System.Drawing.Size(512, 27)
        Me.rbnApply.TabIndex = 0
        Me.rbnApply.TabStop = True
        Me.rbnApply.Text = "Apply ITCM patches or recycle ITCM services on this computer."
        Me.rbnApply.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(931, 19)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(74, 80)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'grpHistory
        '
        Me.grpHistory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpHistory.Controls.Add(Me.treHistory)
        Me.grpHistory.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpHistory.Location = New System.Drawing.Point(0, 292)
        Me.grpHistory.Margin = New System.Windows.Forms.Padding(0)
        Me.grpHistory.Name = "grpHistory"
        Me.grpHistory.Padding = New System.Windows.Forms.Padding(4)
        Me.grpHistory.Size = New System.Drawing.Size(1023, 404)
        Me.grpHistory.TabIndex = 26
        Me.grpHistory.TabStop = False
        Me.grpHistory.Text = "Patch History"
        '
        'treHistory
        '
        Me.treHistory.BackColor = System.Drawing.Color.Beige
        Me.treHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treHistory.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.treHistory.Location = New System.Drawing.Point(4, 27)
        Me.treHistory.Margin = New System.Windows.Forms.Padding(4)
        Me.treHistory.Name = "treHistory"
        Me.treHistory.Size = New System.Drawing.Size(1015, 373)
        Me.treHistory.TabIndex = 1
        Me.treHistory.TabStop = False
        '
        'pnlWinOfflineRemove
        '
        Me.pnlWinOfflineRemove.Controls.Add(Me.lvwRemoveOptions)
        Me.pnlWinOfflineRemove.Controls.Add(Me.grpHistoryView)
        Me.pnlWinOfflineRemove.Controls.Add(Me.pnlWinOfflineButton2)
        Me.pnlWinOfflineRemove.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWinOfflineRemove.Location = New System.Drawing.Point(0, 0)
        Me.pnlWinOfflineRemove.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineRemove.Name = "pnlWinOfflineRemove"
        Me.pnlWinOfflineRemove.Size = New System.Drawing.Size(1023, 698)
        Me.pnlWinOfflineRemove.TabIndex = 34
        '
        'lvwRemoveOptions
        '
        Me.lvwRemoveOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwRemoveOptions.BackColor = System.Drawing.Color.Beige
        Me.lvwRemoveOptions.CheckBoxes = True
        Me.lvwRemoveOptions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnRemoveOptions})
        Me.lvwRemoveOptions.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRemoveOptions.GridLines = True
        Me.lvwRemoveOptions.Location = New System.Drawing.Point(0, 0)
        Me.lvwRemoveOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.lvwRemoveOptions.Name = "lvwRemoveOptions"
        Me.lvwRemoveOptions.Size = New System.Drawing.Size(1019, 394)
        Me.lvwRemoveOptions.TabIndex = 32
        Me.lvwRemoveOptions.UseCompatibleStateImageBehavior = False
        Me.lvwRemoveOptions.View = System.Windows.Forms.View.Details
        '
        'ColumnRemoveOptions
        '
        Me.ColumnRemoveOptions.Text = "Advanced Options"
        Me.ColumnRemoveOptions.Width = 616
        '
        'grpHistoryView
        '
        Me.grpHistoryView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpHistoryView.Controls.Add(Me.lvwHistory)
        Me.grpHistoryView.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpHistoryView.Location = New System.Drawing.Point(0, 394)
        Me.grpHistoryView.Margin = New System.Windows.Forms.Padding(4)
        Me.grpHistoryView.Name = "grpHistoryView"
        Me.grpHistoryView.Padding = New System.Windows.Forms.Padding(4)
        Me.grpHistoryView.Size = New System.Drawing.Size(1023, 195)
        Me.grpHistoryView.TabIndex = 33
        Me.grpHistoryView.TabStop = False
        Me.grpHistoryView.Text = "Choose Patches to Remove:"
        '
        'lvwHistory
        '
        Me.lvwHistory.BackColor = System.Drawing.Color.Beige
        Me.lvwHistory.CheckBoxes = True
        Me.lvwHistory.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHistoryPatch, Me.ColumnHistoryComp, Me.ColumnHistoryStatus})
        Me.lvwHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwHistory.GridLines = True
        Me.lvwHistory.Location = New System.Drawing.Point(4, 27)
        Me.lvwHistory.Margin = New System.Windows.Forms.Padding(4)
        Me.lvwHistory.Name = "lvwHistory"
        Me.lvwHistory.Size = New System.Drawing.Size(1015, 164)
        Me.lvwHistory.TabIndex = 32
        Me.lvwHistory.UseCompatibleStateImageBehavior = False
        Me.lvwHistory.View = System.Windows.Forms.View.Details
        '
        'ColumnHistoryPatch
        '
        Me.ColumnHistoryPatch.Text = "Patch"
        Me.ColumnHistoryPatch.Width = 197
        '
        'ColumnHistoryComp
        '
        Me.ColumnHistoryComp.Text = "Component"
        Me.ColumnHistoryComp.Width = 140
        '
        'ColumnHistoryStatus
        '
        Me.ColumnHistoryStatus.Text = "Status"
        Me.ColumnHistoryStatus.Width = 272
        '
        'pnlWinOfflineButton2
        '
        Me.pnlWinOfflineButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.pnlWinOfflineButton2.Controls.Add(Me.btnWinOfflineStart2)
        Me.pnlWinOfflineButton2.Controls.Add(Me.btnWinOfflineExit4)
        Me.pnlWinOfflineButton2.Controls.Add(Me.btnWinOfflineBack4)
        Me.pnlWinOfflineButton2.Location = New System.Drawing.Point(169, 621)
        Me.pnlWinOfflineButton2.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineButton2.Name = "pnlWinOfflineButton2"
        Me.pnlWinOfflineButton2.Size = New System.Drawing.Size(674, 75)
        Me.pnlWinOfflineButton2.TabIndex = 34
        '
        'btnWinOfflineStart2
        '
        Me.btnWinOfflineStart2.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnWinOfflineStart2.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineStart2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineStart2.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineStart2.ForeColor = System.Drawing.SystemColors.Window
        Me.btnWinOfflineStart2.Location = New System.Drawing.Point(228, 15)
        Me.btnWinOfflineStart2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineStart2.Name = "btnWinOfflineStart2"
        Me.btnWinOfflineStart2.Size = New System.Drawing.Size(219, 48)
        Me.btnWinOfflineStart2.TabIndex = 0
        Me.btnWinOfflineStart2.Text = "Start"
        Me.btnWinOfflineStart2.UseVisualStyleBackColor = False
        '
        'btnWinOfflineExit4
        '
        Me.btnWinOfflineExit4.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineExit4.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineExit4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineExit4.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineExit4.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineExit4.Location = New System.Drawing.Point(509, 20)
        Me.btnWinOfflineExit4.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineExit4.Name = "btnWinOfflineExit4"
        Me.btnWinOfflineExit4.Size = New System.Drawing.Size(158, 35)
        Me.btnWinOfflineExit4.TabIndex = 1
        Me.btnWinOfflineExit4.Text = "Exit"
        Me.btnWinOfflineExit4.UseVisualStyleBackColor = False
        '
        'btnWinOfflineBack4
        '
        Me.btnWinOfflineBack4.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineBack4.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineBack4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineBack4.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineBack4.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineBack4.Location = New System.Drawing.Point(8, 21)
        Me.btnWinOfflineBack4.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineBack4.Name = "btnWinOfflineBack4"
        Me.btnWinOfflineBack4.Size = New System.Drawing.Size(158, 35)
        Me.btnWinOfflineBack4.TabIndex = 2
        Me.btnWinOfflineBack4.Text = "Go Back..."
        Me.btnWinOfflineBack4.UseVisualStyleBackColor = False
        '
        'pnlWinOfflineSDHelp
        '
        Me.pnlWinOfflineSDHelp.AutoScroll = True
        Me.pnlWinOfflineSDHelp.Controls.Add(Me.grpSDHelp)
        Me.pnlWinOfflineSDHelp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWinOfflineSDHelp.Location = New System.Drawing.Point(0, 0)
        Me.pnlWinOfflineSDHelp.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineSDHelp.Name = "pnlWinOfflineSDHelp"
        Me.pnlWinOfflineSDHelp.Size = New System.Drawing.Size(1023, 698)
        Me.pnlWinOfflineSDHelp.TabIndex = 32
        '
        'grpSDHelp
        '
        Me.grpSDHelp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSDHelp.Controls.Add(Me.btnWinOfflineSwicthes)
        Me.grpSDHelp.Controls.Add(Me.btnWinOfflineBack2)
        Me.grpSDHelp.Controls.Add(Me.btnWinOfflineSDHelpNext)
        Me.grpSDHelp.Controls.Add(Me.btnWinOfflineSDHelpPrevious)
        Me.grpSDHelp.Controls.Add(Me.picSteps)
        Me.grpSDHelp.Controls.Add(Me.lblStepx)
        Me.grpSDHelp.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSDHelp.Location = New System.Drawing.Point(0, 4)
        Me.grpSDHelp.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSDHelp.Name = "grpSDHelp"
        Me.grpSDHelp.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSDHelp.Size = New System.Drawing.Size(1019, 690)
        Me.grpSDHelp.TabIndex = 14
        Me.grpSDHelp.TabStop = False
        Me.grpSDHelp.Text = "Software Delivery Help Instructions"
        '
        'btnWinOfflineSwicthes
        '
        Me.btnWinOfflineSwicthes.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnWinOfflineSwicthes.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineSwicthes.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineSwicthes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineSwicthes.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineSwicthes.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineSwicthes.Location = New System.Drawing.Point(363, 639)
        Me.btnWinOfflineSwicthes.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineSwicthes.Name = "btnWinOfflineSwicthes"
        Me.btnWinOfflineSwicthes.Size = New System.Drawing.Size(179, 35)
        Me.btnWinOfflineSwicthes.TabIndex = 3
        Me.btnWinOfflineSwicthes.Text = "Available Switches"
        Me.btnWinOfflineSwicthes.UseVisualStyleBackColor = False
        Me.btnWinOfflineSwicthes.Visible = False
        '
        'btnWinOfflineBack2
        '
        Me.btnWinOfflineBack2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineBack2.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineBack2.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineBack2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineBack2.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineBack2.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineBack2.Location = New System.Drawing.Point(9, 639)
        Me.btnWinOfflineBack2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineBack2.Name = "btnWinOfflineBack2"
        Me.btnWinOfflineBack2.Size = New System.Drawing.Size(161, 35)
        Me.btnWinOfflineBack2.TabIndex = 2
        Me.btnWinOfflineBack2.Text = "Go Back..."
        Me.btnWinOfflineBack2.UseVisualStyleBackColor = False
        '
        'btnWinOfflineSDHelpNext
        '
        Me.btnWinOfflineSDHelpNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineSDHelpNext.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineSDHelpNext.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineSDHelpNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineSDHelpNext.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineSDHelpNext.Location = New System.Drawing.Point(873, 629)
        Me.btnWinOfflineSDHelpNext.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineSDHelpNext.Name = "btnWinOfflineSDHelpNext"
        Me.btnWinOfflineSDHelpNext.Size = New System.Drawing.Size(139, 56)
        Me.btnWinOfflineSDHelpNext.TabIndex = 0
        Me.btnWinOfflineSDHelpNext.UseVisualStyleBackColor = False
        '
        'btnWinOfflineSDHelpPrevious
        '
        Me.btnWinOfflineSDHelpPrevious.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineSDHelpPrevious.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineSDHelpPrevious.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineSDHelpPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineSDHelpPrevious.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineSDHelpPrevious.Location = New System.Drawing.Point(730, 630)
        Me.btnWinOfflineSDHelpPrevious.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineSDHelpPrevious.Name = "btnWinOfflineSDHelpPrevious"
        Me.btnWinOfflineSDHelpPrevious.Size = New System.Drawing.Size(139, 54)
        Me.btnWinOfflineSDHelpPrevious.TabIndex = 1
        Me.btnWinOfflineSDHelpPrevious.UseVisualStyleBackColor = False
        '
        'picSteps
        '
        Me.picSteps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picSteps.BackColor = System.Drawing.SystemColors.Control
        Me.picSteps.ErrorImage = Nothing
        Me.picSteps.Location = New System.Drawing.Point(4, 28)
        Me.picSteps.Margin = New System.Windows.Forms.Padding(4)
        Me.picSteps.Name = "picSteps"
        Me.picSteps.Size = New System.Drawing.Size(1010, 498)
        Me.picSteps.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picSteps.TabIndex = 6
        Me.picSteps.TabStop = False
        '
        'lblStepx
        '
        Me.lblStepx.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStepx.BackColor = System.Drawing.SystemColors.Control
        Me.lblStepx.Location = New System.Drawing.Point(4, 525)
        Me.lblStepx.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStepx.Name = "lblStepx"
        Me.lblStepx.Size = New System.Drawing.Size(1009, 95)
        Me.lblStepx.TabIndex = 10
        Me.lblStepx.Text = "Line 1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Line 2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Line 3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Line 4"
        Me.lblStepx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlWinOfflineCLIHelp
        '
        Me.pnlWinOfflineCLIHelp.AutoScroll = True
        Me.pnlWinOfflineCLIHelp.Controls.Add(Me.grpCLIOptions)
        Me.pnlWinOfflineCLIHelp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWinOfflineCLIHelp.Location = New System.Drawing.Point(0, 0)
        Me.pnlWinOfflineCLIHelp.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlWinOfflineCLIHelp.Name = "pnlWinOfflineCLIHelp"
        Me.pnlWinOfflineCLIHelp.Size = New System.Drawing.Size(1023, 698)
        Me.pnlWinOfflineCLIHelp.TabIndex = 31
        '
        'grpCLIOptions
        '
        Me.grpCLIOptions.Controls.Add(Me.btnWinOfflineExit2)
        Me.grpCLIOptions.Controls.Add(Me.btnWinOfflineBack1)
        Me.grpCLIOptions.Controls.Add(Me.rtbOptions)
        Me.grpCLIOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCLIOptions.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCLIOptions.Location = New System.Drawing.Point(0, 0)
        Me.grpCLIOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.grpCLIOptions.Name = "grpCLIOptions"
        Me.grpCLIOptions.Padding = New System.Windows.Forms.Padding(4)
        Me.grpCLIOptions.Size = New System.Drawing.Size(1023, 698)
        Me.grpCLIOptions.TabIndex = 3
        Me.grpCLIOptions.TabStop = False
        Me.grpCLIOptions.Text = "Help and Command Line Switches"
        '
        'btnWinOfflineExit2
        '
        Me.btnWinOfflineExit2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineExit2.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineExit2.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineExit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineExit2.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineExit2.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineExit2.Location = New System.Drawing.Point(853, 639)
        Me.btnWinOfflineExit2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineExit2.Name = "btnWinOfflineExit2"
        Me.btnWinOfflineExit2.Size = New System.Drawing.Size(158, 35)
        Me.btnWinOfflineExit2.TabIndex = 1
        Me.btnWinOfflineExit2.Text = "E&xit"
        Me.btnWinOfflineExit2.UseVisualStyleBackColor = False
        '
        'btnWinOfflineBack1
        '
        Me.btnWinOfflineBack1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnWinOfflineBack1.BackColor = System.Drawing.Color.SteelBlue
        Me.btnWinOfflineBack1.FlatAppearance.BorderSize = 0
        Me.btnWinOfflineBack1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWinOfflineBack1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWinOfflineBack1.ForeColor = System.Drawing.Color.White
        Me.btnWinOfflineBack1.Location = New System.Drawing.Point(8, 639)
        Me.btnWinOfflineBack1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWinOfflineBack1.Name = "btnWinOfflineBack1"
        Me.btnWinOfflineBack1.Size = New System.Drawing.Size(158, 35)
        Me.btnWinOfflineBack1.TabIndex = 0
        Me.btnWinOfflineBack1.Text = "&Go Back..."
        Me.btnWinOfflineBack1.UseVisualStyleBackColor = False
        '
        'rtbOptions
        '
        Me.rtbOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbOptions.BackColor = System.Drawing.Color.Beige
        Me.rtbOptions.DetectUrls = False
        Me.rtbOptions.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbOptions.Location = New System.Drawing.Point(8, 26)
        Me.rtbOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbOptions.Name = "rtbOptions"
        Me.rtbOptions.ReadOnly = True
        Me.rtbOptions.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.rtbOptions.Size = New System.Drawing.Size(1003, 587)
        Me.rtbOptions.TabIndex = 2
        Me.rtbOptions.TabStop = False
        Me.rtbOptions.Text = resources.GetString("rtbOptions.Text")
        '
        'pnlSqlMdbOverview
        '
        Me.pnlSqlMdbOverview.Controls.Add(Me.grpSqlMdbOverview)
        Me.pnlSqlMdbOverview.Controls.Add(Me.grpSqlMdbItcmSum)
        Me.pnlSqlMdbOverview.Controls.Add(Me.grpSqlMdbAgentVersion)
        Me.pnlSqlMdbOverview.Controls.Add(Me.grpSqlMdbDefSummary)
        Me.pnlSqlMdbOverview.Controls.Add(Me.pnlSqlMdbOverviewButtons)
        Me.pnlSqlMdbOverview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlMdbOverview.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlMdbOverview.Name = "pnlSqlMdbOverview"
        Me.pnlSqlMdbOverview.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlMdbOverview.TabIndex = 1
        '
        'grpSqlMdbOverview
        '
        Me.grpSqlMdbOverview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlMdbOverview.Controls.Add(Me.txtMdbType)
        Me.grpSqlMdbOverview.Controls.Add(Me.lblMdbType)
        Me.grpSqlMdbOverview.Controls.Add(Me.lblMdbVersion)
        Me.grpSqlMdbOverview.Controls.Add(Me.lblMdbInstallDate)
        Me.grpSqlMdbOverview.Controls.Add(Me.txtMdbVersion)
        Me.grpSqlMdbOverview.Controls.Add(Me.lblITCMVersion)
        Me.grpSqlMdbOverview.Controls.Add(Me.txtMdbInstallDate)
        Me.grpSqlMdbOverview.Controls.Add(Me.txtITCMVersion)
        Me.grpSqlMdbOverview.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlMdbOverview.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbOverview.Name = "grpSqlMdbOverview"
        Me.grpSqlMdbOverview.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbOverview.Size = New System.Drawing.Size(1023, 105)
        Me.grpSqlMdbOverview.TabIndex = 27
        Me.grpSqlMdbOverview.TabStop = False
        Me.grpSqlMdbOverview.Text = "Mdb Installation Overview"
        '
        'txtMdbType
        '
        Me.txtMdbType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMdbType.BackColor = System.Drawing.Color.Beige
        Me.txtMdbType.Location = New System.Drawing.Point(791, 65)
        Me.txtMdbType.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMdbType.Name = "txtMdbType"
        Me.txtMdbType.ReadOnly = True
        Me.txtMdbType.Size = New System.Drawing.Size(189, 30)
        Me.txtMdbType.TabIndex = 61
        Me.txtMdbType.WordWrap = False
        '
        'lblMdbType
        '
        Me.lblMdbType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMdbType.Location = New System.Drawing.Point(621, 70)
        Me.lblMdbType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMdbType.Name = "lblMdbType"
        Me.lblMdbType.Size = New System.Drawing.Size(162, 22)
        Me.lblMdbType.TabIndex = 60
        Me.lblMdbType.Text = "Mdb Type:"
        Me.lblMdbType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblMdbVersion
        '
        Me.lblMdbVersion.Location = New System.Drawing.Point(41, 35)
        Me.lblMdbVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMdbVersion.Name = "lblMdbVersion"
        Me.lblMdbVersion.Size = New System.Drawing.Size(128, 22)
        Me.lblMdbVersion.TabIndex = 33
        Me.lblMdbVersion.Text = "Mdb Version:"
        Me.lblMdbVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblMdbInstallDate
        '
        Me.lblMdbInstallDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMdbInstallDate.Location = New System.Drawing.Point(621, 35)
        Me.lblMdbInstallDate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMdbInstallDate.Name = "lblMdbInstallDate"
        Me.lblMdbInstallDate.Size = New System.Drawing.Size(162, 22)
        Me.lblMdbInstallDate.TabIndex = 34
        Me.lblMdbInstallDate.Text = "Mdb Install Date:"
        Me.lblMdbInstallDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtMdbVersion
        '
        Me.txtMdbVersion.BackColor = System.Drawing.Color.Beige
        Me.txtMdbVersion.Location = New System.Drawing.Point(174, 30)
        Me.txtMdbVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMdbVersion.Name = "txtMdbVersion"
        Me.txtMdbVersion.ReadOnly = True
        Me.txtMdbVersion.Size = New System.Drawing.Size(189, 30)
        Me.txtMdbVersion.TabIndex = 59
        Me.txtMdbVersion.WordWrap = False
        '
        'lblITCMVersion
        '
        Me.lblITCMVersion.Location = New System.Drawing.Point(41, 70)
        Me.lblITCMVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblITCMVersion.Name = "lblITCMVersion"
        Me.lblITCMVersion.Size = New System.Drawing.Size(128, 22)
        Me.lblITCMVersion.TabIndex = 35
        Me.lblITCMVersion.Text = "ITCM Version:"
        Me.lblITCMVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtMdbInstallDate
        '
        Me.txtMdbInstallDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMdbInstallDate.BackColor = System.Drawing.Color.Beige
        Me.txtMdbInstallDate.Location = New System.Drawing.Point(791, 30)
        Me.txtMdbInstallDate.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMdbInstallDate.Name = "txtMdbInstallDate"
        Me.txtMdbInstallDate.ReadOnly = True
        Me.txtMdbInstallDate.Size = New System.Drawing.Size(189, 30)
        Me.txtMdbInstallDate.TabIndex = 58
        Me.txtMdbInstallDate.WordWrap = False
        '
        'txtITCMVersion
        '
        Me.txtITCMVersion.BackColor = System.Drawing.Color.Beige
        Me.txtITCMVersion.Location = New System.Drawing.Point(174, 65)
        Me.txtITCMVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.txtITCMVersion.Name = "txtITCMVersion"
        Me.txtITCMVersion.ReadOnly = True
        Me.txtITCMVersion.Size = New System.Drawing.Size(189, 30)
        Me.txtITCMVersion.TabIndex = 48
        Me.txtITCMVersion.WordWrap = False
        '
        'grpSqlMdbItcmSum
        '
        Me.grpSqlMdbItcmSum.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlMdbItcmSum.Controls.Add(Me.lvwITCMSummary)
        Me.grpSqlMdbItcmSum.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlMdbItcmSum.Location = New System.Drawing.Point(0, 111)
        Me.grpSqlMdbItcmSum.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbItcmSum.Name = "grpSqlMdbItcmSum"
        Me.grpSqlMdbItcmSum.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbItcmSum.Size = New System.Drawing.Size(507, 523)
        Me.grpSqlMdbItcmSum.TabIndex = 63
        Me.grpSqlMdbItcmSum.TabStop = False
        Me.grpSqlMdbItcmSum.Text = "ITCM Metrics"
        '
        'lvwITCMSummary
        '
        Me.lvwITCMSummary.BackColor = System.Drawing.Color.Beige
        Me.lvwITCMSummary.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnMetric, Me.ColumnValue})
        Me.lvwITCMSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwITCMSummary.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwITCMSummary.GridLines = True
        Me.lvwITCMSummary.LabelWrap = False
        Me.lvwITCMSummary.Location = New System.Drawing.Point(4, 27)
        Me.lvwITCMSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.lvwITCMSummary.Name = "lvwITCMSummary"
        Me.lvwITCMSummary.Size = New System.Drawing.Size(499, 492)
        Me.lvwITCMSummary.TabIndex = 58
        Me.lvwITCMSummary.UseCompatibleStateImageBehavior = False
        Me.lvwITCMSummary.View = System.Windows.Forms.View.Details
        '
        'ColumnMetric
        '
        Me.ColumnMetric.Text = "Metric"
        '
        'ColumnValue
        '
        Me.ColumnValue.Text = "Value"
        '
        'grpSqlMdbAgentVersion
        '
        Me.grpSqlMdbAgentVersion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlMdbAgentVersion.Controls.Add(Me.dgvAgentVersion)
        Me.grpSqlMdbAgentVersion.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlMdbAgentVersion.Location = New System.Drawing.Point(511, 111)
        Me.grpSqlMdbAgentVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbAgentVersion.Name = "grpSqlMdbAgentVersion"
        Me.grpSqlMdbAgentVersion.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbAgentVersion.Size = New System.Drawing.Size(511, 261)
        Me.grpSqlMdbAgentVersion.TabIndex = 66
        Me.grpSqlMdbAgentVersion.TabStop = False
        Me.grpSqlMdbAgentVersion.Text = "Agent Version Distribution"
        '
        'dgvAgentVersion
        '
        Me.dgvAgentVersion.AllowUserToAddRows = False
        Me.dgvAgentVersion.AllowUserToDeleteRows = False
        Me.dgvAgentVersion.AllowUserToResizeRows = False
        Me.dgvAgentVersion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvAgentVersion.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvAgentVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvAgentVersion.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAgentVersion.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvAgentVersion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAgentVersion.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvAgentVersion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAgentVersion.EnableHeadersVisualStyles = False
        Me.dgvAgentVersion.Location = New System.Drawing.Point(4, 27)
        Me.dgvAgentVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvAgentVersion.Name = "dgvAgentVersion"
        Me.dgvAgentVersion.ReadOnly = True
        Me.dgvAgentVersion.RowHeadersVisible = False
        Me.dgvAgentVersion.ShowCellErrors = False
        Me.dgvAgentVersion.ShowCellToolTips = False
        Me.dgvAgentVersion.ShowEditingIcon = False
        Me.dgvAgentVersion.ShowRowErrors = False
        Me.dgvAgentVersion.Size = New System.Drawing.Size(503, 230)
        Me.dgvAgentVersion.TabIndex = 0
        '
        'grpSqlMdbDefSummary
        '
        Me.grpSqlMdbDefSummary.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlMdbDefSummary.Controls.Add(Me.dgvContentSummary)
        Me.grpSqlMdbDefSummary.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlMdbDefSummary.Location = New System.Drawing.Point(511, 371)
        Me.grpSqlMdbDefSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbDefSummary.Name = "grpSqlMdbDefSummary"
        Me.grpSqlMdbDefSummary.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlMdbDefSummary.Size = New System.Drawing.Size(511, 262)
        Me.grpSqlMdbDefSummary.TabIndex = 65
        Me.grpSqlMdbDefSummary.TabStop = False
        Me.grpSqlMdbDefSummary.Text = "Software Definition Metrics"
        '
        'dgvContentSummary
        '
        Me.dgvContentSummary.AllowUserToAddRows = False
        Me.dgvContentSummary.AllowUserToDeleteRows = False
        Me.dgvContentSummary.AllowUserToResizeRows = False
        Me.dgvContentSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvContentSummary.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvContentSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvContentSummary.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvContentSummary.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvContentSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.Beige
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvContentSummary.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvContentSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvContentSummary.EnableHeadersVisualStyles = False
        Me.dgvContentSummary.Location = New System.Drawing.Point(4, 27)
        Me.dgvContentSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvContentSummary.Name = "dgvContentSummary"
        Me.dgvContentSummary.ReadOnly = True
        Me.dgvContentSummary.RowHeadersVisible = False
        Me.dgvContentSummary.ShowCellErrors = False
        Me.dgvContentSummary.ShowCellToolTips = False
        Me.dgvContentSummary.ShowEditingIcon = False
        Me.dgvContentSummary.ShowRowErrors = False
        Me.dgvContentSummary.Size = New System.Drawing.Size(503, 231)
        Me.dgvContentSummary.TabIndex = 0
        '
        'pnlSqlMdbOverviewButtons
        '
        Me.pnlSqlMdbOverviewButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlMdbOverviewButtons.Controls.Add(Me.btnSqlConnectMdbOverview)
        Me.pnlSqlMdbOverviewButtons.Controls.Add(Me.btnSqlDisconnectMdbOverview)
        Me.pnlSqlMdbOverviewButtons.Controls.Add(Me.btnSqlRefreshMdbOverview)
        Me.pnlSqlMdbOverviewButtons.Controls.Add(Me.btnSqlExportMdbOverview)
        Me.pnlSqlMdbOverviewButtons.Controls.Add(Me.btnSqlExitMdbOverview)
        Me.pnlSqlMdbOverviewButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlMdbOverviewButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlMdbOverviewButtons.Name = "pnlSqlMdbOverviewButtons"
        Me.pnlSqlMdbOverviewButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlMdbOverviewButtons.TabIndex = 67
        '
        'btnSqlConnectMdbOverview
        '
        Me.btnSqlConnectMdbOverview.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectMdbOverview.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectMdbOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectMdbOverview.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectMdbOverview.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectMdbOverview.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectMdbOverview.Name = "btnSqlConnectMdbOverview"
        Me.btnSqlConnectMdbOverview.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectMdbOverview.TabIndex = 77
        Me.btnSqlConnectMdbOverview.Text = "&Connect Sql"
        Me.btnSqlConnectMdbOverview.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectMdbOverview
        '
        Me.btnSqlDisconnectMdbOverview.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectMdbOverview.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectMdbOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectMdbOverview.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectMdbOverview.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectMdbOverview.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectMdbOverview.Name = "btnSqlDisconnectMdbOverview"
        Me.btnSqlDisconnectMdbOverview.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectMdbOverview.TabIndex = 78
        Me.btnSqlDisconnectMdbOverview.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectMdbOverview.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshMdbOverview
        '
        Me.btnSqlRefreshMdbOverview.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshMdbOverview.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshMdbOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshMdbOverview.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshMdbOverview.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshMdbOverview.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshMdbOverview.Name = "btnSqlRefreshMdbOverview"
        Me.btnSqlRefreshMdbOverview.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshMdbOverview.TabIndex = 79
        Me.btnSqlRefreshMdbOverview.Text = "&Refresh"
        Me.btnSqlRefreshMdbOverview.UseVisualStyleBackColor = False
        '
        'btnSqlExportMdbOverview
        '
        Me.btnSqlExportMdbOverview.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportMdbOverview.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportMdbOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportMdbOverview.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportMdbOverview.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportMdbOverview.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportMdbOverview.Name = "btnSqlExportMdbOverview"
        Me.btnSqlExportMdbOverview.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportMdbOverview.TabIndex = 80
        Me.btnSqlExportMdbOverview.Text = "&Export"
        Me.btnSqlExportMdbOverview.UseVisualStyleBackColor = False
        '
        'btnSqlExitMdbOverview
        '
        Me.btnSqlExitMdbOverview.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitMdbOverview.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitMdbOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitMdbOverview.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitMdbOverview.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitMdbOverview.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitMdbOverview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitMdbOverview.Name = "btnSqlExitMdbOverview"
        Me.btnSqlExitMdbOverview.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitMdbOverview.TabIndex = 81
        Me.btnSqlExitMdbOverview.Text = "E&xit"
        Me.btnSqlExitMdbOverview.UseVisualStyleBackColor = False
        '
        'pnlSqlTableSpaceGrid
        '
        Me.pnlSqlTableSpaceGrid.Controls.Add(Me.grpSqlTableSpace)
        Me.pnlSqlTableSpaceGrid.Controls.Add(Me.prgTableSpaceGrid)
        Me.pnlSqlTableSpaceGrid.Controls.Add(Me.pnlSqlTableSpaceGridButtons)
        Me.pnlSqlTableSpaceGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlTableSpaceGrid.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSqlTableSpaceGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlTableSpaceGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlTableSpaceGrid.Name = "pnlSqlTableSpaceGrid"
        Me.pnlSqlTableSpaceGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlTableSpaceGrid.TabIndex = 36
        '
        'grpSqlTableSpace
        '
        Me.grpSqlTableSpace.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlTableSpace.Controls.Add(Me.dgvTableSpaceGrid)
        Me.grpSqlTableSpace.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlTableSpace.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlTableSpace.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlTableSpace.Name = "grpSqlTableSpace"
        Me.grpSqlTableSpace.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlTableSpace.Size = New System.Drawing.Size(1023, 633)
        Me.grpSqlTableSpace.TabIndex = 61
        Me.grpSqlTableSpace.TabStop = False
        Me.grpSqlTableSpace.Text = "MDB Table Space"
        '
        'dgvTableSpaceGrid
        '
        Me.dgvTableSpaceGrid.AllowUserToAddRows = False
        Me.dgvTableSpaceGrid.AllowUserToDeleteRows = False
        Me.dgvTableSpaceGrid.AllowUserToResizeRows = False
        Me.dgvTableSpaceGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvTableSpaceGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvTableSpaceGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvTableSpaceGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTableSpaceGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvTableSpaceGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.Beige
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTableSpaceGrid.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgvTableSpaceGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTableSpaceGrid.EnableHeadersVisualStyles = False
        Me.dgvTableSpaceGrid.Location = New System.Drawing.Point(4, 27)
        Me.dgvTableSpaceGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvTableSpaceGrid.Name = "dgvTableSpaceGrid"
        Me.dgvTableSpaceGrid.ReadOnly = True
        Me.dgvTableSpaceGrid.RowHeadersVisible = False
        Me.dgvTableSpaceGrid.ShowCellErrors = False
        Me.dgvTableSpaceGrid.ShowCellToolTips = False
        Me.dgvTableSpaceGrid.ShowEditingIcon = False
        Me.dgvTableSpaceGrid.ShowRowErrors = False
        Me.dgvTableSpaceGrid.Size = New System.Drawing.Size(1015, 602)
        Me.dgvTableSpaceGrid.TabIndex = 62
        '
        'prgTableSpaceGrid
        '
        Me.prgTableSpaceGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgTableSpaceGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgTableSpaceGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgTableSpaceGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgTableSpaceGrid.Name = "prgTableSpaceGrid"
        Me.prgTableSpaceGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgTableSpaceGrid.Step = 1
        Me.prgTableSpaceGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgTableSpaceGrid.TabIndex = 69
        '
        'pnlSqlTableSpaceGridButtons
        '
        Me.pnlSqlTableSpaceGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlTableSpaceGridButtons.Controls.Add(Me.btnSqlConnectTableSpace)
        Me.pnlSqlTableSpaceGridButtons.Controls.Add(Me.btnSqlDisconnectTableSpace)
        Me.pnlSqlTableSpaceGridButtons.Controls.Add(Me.btnSqlRefreshTableSpace)
        Me.pnlSqlTableSpaceGridButtons.Controls.Add(Me.btnSqlExportTableSpace)
        Me.pnlSqlTableSpaceGridButtons.Controls.Add(Me.btnSqlExitTableSpace)
        Me.pnlSqlTableSpaceGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlTableSpaceGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlTableSpaceGridButtons.Name = "pnlSqlTableSpaceGridButtons"
        Me.pnlSqlTableSpaceGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlTableSpaceGridButtons.TabIndex = 68
        '
        'btnSqlConnectTableSpace
        '
        Me.btnSqlConnectTableSpace.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectTableSpace.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectTableSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectTableSpace.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectTableSpace.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectTableSpace.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectTableSpace.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectTableSpace.Name = "btnSqlConnectTableSpace"
        Me.btnSqlConnectTableSpace.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectTableSpace.TabIndex = 77
        Me.btnSqlConnectTableSpace.Text = "&Connect Sql"
        Me.btnSqlConnectTableSpace.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectTableSpace
        '
        Me.btnSqlDisconnectTableSpace.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectTableSpace.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectTableSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectTableSpace.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectTableSpace.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectTableSpace.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectTableSpace.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectTableSpace.Name = "btnSqlDisconnectTableSpace"
        Me.btnSqlDisconnectTableSpace.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectTableSpace.TabIndex = 78
        Me.btnSqlDisconnectTableSpace.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectTableSpace.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshTableSpace
        '
        Me.btnSqlRefreshTableSpace.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshTableSpace.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshTableSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshTableSpace.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshTableSpace.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshTableSpace.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshTableSpace.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshTableSpace.Name = "btnSqlRefreshTableSpace"
        Me.btnSqlRefreshTableSpace.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshTableSpace.TabIndex = 79
        Me.btnSqlRefreshTableSpace.Text = "&Refresh"
        Me.btnSqlRefreshTableSpace.UseVisualStyleBackColor = False
        '
        'btnSqlExportTableSpace
        '
        Me.btnSqlExportTableSpace.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportTableSpace.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportTableSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportTableSpace.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportTableSpace.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportTableSpace.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportTableSpace.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportTableSpace.Name = "btnSqlExportTableSpace"
        Me.btnSqlExportTableSpace.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportTableSpace.TabIndex = 80
        Me.btnSqlExportTableSpace.Text = "&Export"
        Me.btnSqlExportTableSpace.UseVisualStyleBackColor = False
        '
        'btnSqlExitTableSpace
        '
        Me.btnSqlExitTableSpace.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitTableSpace.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitTableSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitTableSpace.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitTableSpace.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitTableSpace.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitTableSpace.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitTableSpace.Name = "btnSqlExitTableSpace"
        Me.btnSqlExitTableSpace.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitTableSpace.TabIndex = 81
        Me.btnSqlExitTableSpace.Text = "E&xit"
        Me.btnSqlExitTableSpace.UseVisualStyleBackColor = False
        '
        'pnlSqlUserGrid
        '
        Me.pnlSqlUserGrid.Controls.Add(Me.tabCtrlUserGrid)
        Me.pnlSqlUserGrid.Controls.Add(Me.prgUserGrid)
        Me.pnlSqlUserGrid.Controls.Add(Me.pnlSqlUserGridButtons)
        Me.pnlSqlUserGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlUserGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlUserGrid.Name = "pnlSqlUserGrid"
        Me.pnlSqlUserGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlUserGrid.TabIndex = 44
        '
        'tabCtrlUserGrid
        '
        Me.tabCtrlUserGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabCtrlUserGrid.Controls.Add(Me.tabUserSummary)
        Me.tabCtrlUserGrid.Controls.Add(Me.tabUserObsolete90)
        Me.tabCtrlUserGrid.Controls.Add(Me.tabUserObsolete365)
        Me.tabCtrlUserGrid.Location = New System.Drawing.Point(0, 1)
        Me.tabCtrlUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.tabCtrlUserGrid.Name = "tabCtrlUserGrid"
        Me.tabCtrlUserGrid.SelectedIndex = 0
        Me.tabCtrlUserGrid.Size = New System.Drawing.Size(1023, 633)
        Me.tabCtrlUserGrid.TabIndex = 69
        '
        'tabUserSummary
        '
        Me.tabUserSummary.Controls.Add(Me.dgvUserGrid)
        Me.tabUserSummary.Location = New System.Drawing.Point(4, 28)
        Me.tabUserSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.tabUserSummary.Name = "tabUserSummary"
        Me.tabUserSummary.Padding = New System.Windows.Forms.Padding(4)
        Me.tabUserSummary.Size = New System.Drawing.Size(1015, 601)
        Me.tabUserSummary.TabIndex = 0
        Me.tabUserSummary.Text = "User Summary"
        Me.tabUserSummary.UseVisualStyleBackColor = True
        '
        'dgvUserGrid
        '
        Me.dgvUserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUserGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUserGrid.Location = New System.Drawing.Point(4, 4)
        Me.dgvUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvUserGrid.Name = "dgvUserGrid"
        Me.dgvUserGrid.Size = New System.Drawing.Size(1007, 593)
        Me.dgvUserGrid.TabIndex = 1
        '
        'tabUserObsolete90
        '
        Me.tabUserObsolete90.Controls.Add(Me.dgvUserObsolete90)
        Me.tabUserObsolete90.Location = New System.Drawing.Point(4, 28)
        Me.tabUserObsolete90.Margin = New System.Windows.Forms.Padding(4)
        Me.tabUserObsolete90.Name = "tabUserObsolete90"
        Me.tabUserObsolete90.Padding = New System.Windows.Forms.Padding(4)
        Me.tabUserObsolete90.Size = New System.Drawing.Size(1015, 601)
        Me.tabUserObsolete90.TabIndex = 1
        Me.tabUserObsolete90.Text = "Obsolete Users [90 days]"
        Me.tabUserObsolete90.UseVisualStyleBackColor = True
        '
        'dgvUserObsolete90
        '
        Me.dgvUserObsolete90.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUserObsolete90.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUserObsolete90.Location = New System.Drawing.Point(4, 4)
        Me.dgvUserObsolete90.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvUserObsolete90.Name = "dgvUserObsolete90"
        Me.dgvUserObsolete90.Size = New System.Drawing.Size(1007, 593)
        Me.dgvUserObsolete90.TabIndex = 2
        '
        'tabUserObsolete365
        '
        Me.tabUserObsolete365.Controls.Add(Me.dgvUserObsolete365)
        Me.tabUserObsolete365.Location = New System.Drawing.Point(4, 28)
        Me.tabUserObsolete365.Margin = New System.Windows.Forms.Padding(4)
        Me.tabUserObsolete365.Name = "tabUserObsolete365"
        Me.tabUserObsolete365.Padding = New System.Windows.Forms.Padding(4)
        Me.tabUserObsolete365.Size = New System.Drawing.Size(1015, 601)
        Me.tabUserObsolete365.TabIndex = 2
        Me.tabUserObsolete365.Text = "Obsolete Users [1 year]"
        Me.tabUserObsolete365.UseVisualStyleBackColor = True
        '
        'dgvUserObsolete365
        '
        Me.dgvUserObsolete365.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUserObsolete365.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUserObsolete365.Location = New System.Drawing.Point(4, 4)
        Me.dgvUserObsolete365.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvUserObsolete365.Name = "dgvUserObsolete365"
        Me.dgvUserObsolete365.Size = New System.Drawing.Size(1007, 593)
        Me.dgvUserObsolete365.TabIndex = 3
        '
        'prgUserGrid
        '
        Me.prgUserGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgUserGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgUserGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgUserGrid.Name = "prgUserGrid"
        Me.prgUserGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgUserGrid.Step = 1
        Me.prgUserGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgUserGrid.TabIndex = 1
        '
        'pnlSqlUserGridButtons
        '
        Me.pnlSqlUserGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlUserGridButtons.Controls.Add(Me.btnSqlConnectUserGrid)
        Me.pnlSqlUserGridButtons.Controls.Add(Me.btnSqlDisconnectUserGrid)
        Me.pnlSqlUserGridButtons.Controls.Add(Me.btnSqlRefreshUserGrid)
        Me.pnlSqlUserGridButtons.Controls.Add(Me.btnSqlExportUserGrid)
        Me.pnlSqlUserGridButtons.Controls.Add(Me.btnSqlExitUserGrid)
        Me.pnlSqlUserGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlUserGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlUserGridButtons.Name = "pnlSqlUserGridButtons"
        Me.pnlSqlUserGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlUserGridButtons.TabIndex = 68
        '
        'btnSqlConnectUserGrid
        '
        Me.btnSqlConnectUserGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectUserGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectUserGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectUserGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectUserGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectUserGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectUserGrid.Name = "btnSqlConnectUserGrid"
        Me.btnSqlConnectUserGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectUserGrid.TabIndex = 77
        Me.btnSqlConnectUserGrid.Text = "&Connect Sql"
        Me.btnSqlConnectUserGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectUserGrid
        '
        Me.btnSqlDisconnectUserGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectUserGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectUserGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectUserGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectUserGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectUserGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectUserGrid.Name = "btnSqlDisconnectUserGrid"
        Me.btnSqlDisconnectUserGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectUserGrid.TabIndex = 78
        Me.btnSqlDisconnectUserGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectUserGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshUserGrid
        '
        Me.btnSqlRefreshUserGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshUserGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshUserGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshUserGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshUserGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshUserGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshUserGrid.Name = "btnSqlRefreshUserGrid"
        Me.btnSqlRefreshUserGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshUserGrid.TabIndex = 79
        Me.btnSqlRefreshUserGrid.Text = "&Refresh"
        Me.btnSqlRefreshUserGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportUserGrid
        '
        Me.btnSqlExportUserGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportUserGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportUserGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportUserGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportUserGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportUserGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportUserGrid.Name = "btnSqlExportUserGrid"
        Me.btnSqlExportUserGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportUserGrid.TabIndex = 80
        Me.btnSqlExportUserGrid.Text = "&Export"
        Me.btnSqlExportUserGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitUserGrid
        '
        Me.btnSqlExitUserGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitUserGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitUserGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitUserGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitUserGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitUserGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitUserGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitUserGrid.Name = "btnSqlExitUserGrid"
        Me.btnSqlExitUserGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitUserGrid.TabIndex = 81
        Me.btnSqlExitUserGrid.Text = "E&xit"
        Me.btnSqlExitUserGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlAgentGrid
        '
        Me.pnlSqlAgentGrid.Controls.Add(Me.tabCtrlAgentGrid)
        Me.pnlSqlAgentGrid.Controls.Add(Me.prgAgentGrid)
        Me.pnlSqlAgentGrid.Controls.Add(Me.pnlSqlAgentGridButtons)
        Me.pnlSqlAgentGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlAgentGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlAgentGrid.Name = "pnlSqlAgentGrid"
        Me.pnlSqlAgentGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlAgentGrid.TabIndex = 39
        '
        'tabCtrlAgentGrid
        '
        Me.tabCtrlAgentGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabCtrlAgentGrid.Controls.Add(Me.tabAgentSummary)
        Me.tabCtrlAgentGrid.Controls.Add(Me.tabAgentObsolete90)
        Me.tabCtrlAgentGrid.Controls.Add(Me.tabAgentObsolete365)
        Me.tabCtrlAgentGrid.Location = New System.Drawing.Point(0, 1)
        Me.tabCtrlAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.tabCtrlAgentGrid.Name = "tabCtrlAgentGrid"
        Me.tabCtrlAgentGrid.SelectedIndex = 0
        Me.tabCtrlAgentGrid.Size = New System.Drawing.Size(1023, 633)
        Me.tabCtrlAgentGrid.TabIndex = 69
        '
        'tabAgentSummary
        '
        Me.tabAgentSummary.Controls.Add(Me.dgvAgentGrid)
        Me.tabAgentSummary.Location = New System.Drawing.Point(4, 28)
        Me.tabAgentSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.tabAgentSummary.Name = "tabAgentSummary"
        Me.tabAgentSummary.Padding = New System.Windows.Forms.Padding(4)
        Me.tabAgentSummary.Size = New System.Drawing.Size(1015, 601)
        Me.tabAgentSummary.TabIndex = 0
        Me.tabAgentSummary.Text = "Agent Summary"
        Me.tabAgentSummary.UseVisualStyleBackColor = True
        '
        'dgvAgentGrid
        '
        Me.dgvAgentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAgentGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAgentGrid.Location = New System.Drawing.Point(4, 4)
        Me.dgvAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvAgentGrid.Name = "dgvAgentGrid"
        Me.dgvAgentGrid.Size = New System.Drawing.Size(1007, 593)
        Me.dgvAgentGrid.TabIndex = 1
        '
        'tabAgentObsolete90
        '
        Me.tabAgentObsolete90.Controls.Add(Me.dgvAgentObsolete90)
        Me.tabAgentObsolete90.Location = New System.Drawing.Point(4, 28)
        Me.tabAgentObsolete90.Margin = New System.Windows.Forms.Padding(4)
        Me.tabAgentObsolete90.Name = "tabAgentObsolete90"
        Me.tabAgentObsolete90.Padding = New System.Windows.Forms.Padding(4)
        Me.tabAgentObsolete90.Size = New System.Drawing.Size(1015, 601)
        Me.tabAgentObsolete90.TabIndex = 1
        Me.tabAgentObsolete90.Text = "Obsolete Agents [90 days]"
        Me.tabAgentObsolete90.UseVisualStyleBackColor = True
        '
        'dgvAgentObsolete90
        '
        Me.dgvAgentObsolete90.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAgentObsolete90.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAgentObsolete90.Location = New System.Drawing.Point(4, 4)
        Me.dgvAgentObsolete90.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvAgentObsolete90.Name = "dgvAgentObsolete90"
        Me.dgvAgentObsolete90.Size = New System.Drawing.Size(1007, 593)
        Me.dgvAgentObsolete90.TabIndex = 2
        '
        'tabAgentObsolete365
        '
        Me.tabAgentObsolete365.Controls.Add(Me.dgvAgentObsolete365)
        Me.tabAgentObsolete365.Location = New System.Drawing.Point(4, 28)
        Me.tabAgentObsolete365.Margin = New System.Windows.Forms.Padding(4)
        Me.tabAgentObsolete365.Name = "tabAgentObsolete365"
        Me.tabAgentObsolete365.Padding = New System.Windows.Forms.Padding(4)
        Me.tabAgentObsolete365.Size = New System.Drawing.Size(1015, 601)
        Me.tabAgentObsolete365.TabIndex = 2
        Me.tabAgentObsolete365.Text = "Obsolete Agents [1 year]"
        Me.tabAgentObsolete365.UseVisualStyleBackColor = True
        '
        'dgvAgentObsolete365
        '
        Me.dgvAgentObsolete365.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAgentObsolete365.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAgentObsolete365.Location = New System.Drawing.Point(4, 4)
        Me.dgvAgentObsolete365.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvAgentObsolete365.Name = "dgvAgentObsolete365"
        Me.dgvAgentObsolete365.Size = New System.Drawing.Size(1007, 593)
        Me.dgvAgentObsolete365.TabIndex = 3
        '
        'prgAgentGrid
        '
        Me.prgAgentGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgAgentGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgAgentGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgAgentGrid.Name = "prgAgentGrid"
        Me.prgAgentGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgAgentGrid.Step = 1
        Me.prgAgentGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgAgentGrid.TabIndex = 1
        '
        'pnlSqlAgentGridButtons
        '
        Me.pnlSqlAgentGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlAgentGridButtons.Controls.Add(Me.btnSqlConnectAgentGrid)
        Me.pnlSqlAgentGridButtons.Controls.Add(Me.btnSqlDisconnectAgentGrid)
        Me.pnlSqlAgentGridButtons.Controls.Add(Me.btnSqlRefreshAgentGrid)
        Me.pnlSqlAgentGridButtons.Controls.Add(Me.btnSqlExportAgentGrid)
        Me.pnlSqlAgentGridButtons.Controls.Add(Me.btnSqlExitAgentGrid)
        Me.pnlSqlAgentGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlAgentGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlAgentGridButtons.Name = "pnlSqlAgentGridButtons"
        Me.pnlSqlAgentGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlAgentGridButtons.TabIndex = 68
        '
        'btnSqlConnectAgentGrid
        '
        Me.btnSqlConnectAgentGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectAgentGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectAgentGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectAgentGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectAgentGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectAgentGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectAgentGrid.Name = "btnSqlConnectAgentGrid"
        Me.btnSqlConnectAgentGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectAgentGrid.TabIndex = 77
        Me.btnSqlConnectAgentGrid.Text = "&Connect Sql"
        Me.btnSqlConnectAgentGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectAgentGrid
        '
        Me.btnSqlDisconnectAgentGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectAgentGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectAgentGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectAgentGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectAgentGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectAgentGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectAgentGrid.Name = "btnSqlDisconnectAgentGrid"
        Me.btnSqlDisconnectAgentGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectAgentGrid.TabIndex = 78
        Me.btnSqlDisconnectAgentGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectAgentGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshAgentGrid
        '
        Me.btnSqlRefreshAgentGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshAgentGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshAgentGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshAgentGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshAgentGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshAgentGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshAgentGrid.Name = "btnSqlRefreshAgentGrid"
        Me.btnSqlRefreshAgentGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshAgentGrid.TabIndex = 79
        Me.btnSqlRefreshAgentGrid.Text = "&Refresh"
        Me.btnSqlRefreshAgentGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportAgentGrid
        '
        Me.btnSqlExportAgentGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportAgentGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportAgentGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportAgentGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportAgentGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportAgentGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportAgentGrid.Name = "btnSqlExportAgentGrid"
        Me.btnSqlExportAgentGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportAgentGrid.TabIndex = 80
        Me.btnSqlExportAgentGrid.Text = "&Export"
        Me.btnSqlExportAgentGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitAgentGrid
        '
        Me.btnSqlExitAgentGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitAgentGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitAgentGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitAgentGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitAgentGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitAgentGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitAgentGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitAgentGrid.Name = "btnSqlExitAgentGrid"
        Me.btnSqlExitAgentGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitAgentGrid.TabIndex = 81
        Me.btnSqlExitAgentGrid.Text = "E&xit"
        Me.btnSqlExitAgentGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlServerGrid
        '
        Me.pnlSqlServerGrid.Controls.Add(Me.tabCtrlServerGrid)
        Me.pnlSqlServerGrid.Controls.Add(Me.prgServerGrid)
        Me.pnlSqlServerGrid.Controls.Add(Me.pnlSqlServerGridButtons)
        Me.pnlSqlServerGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlServerGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlServerGrid.Name = "pnlSqlServerGrid"
        Me.pnlSqlServerGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlServerGrid.TabIndex = 37
        '
        'tabCtrlServerGrid
        '
        Me.tabCtrlServerGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabCtrlServerGrid.Controls.Add(Me.tabServerSummary)
        Me.tabCtrlServerGrid.Controls.Add(Me.tabServerLastCollected24)
        Me.tabCtrlServerGrid.Controls.Add(Me.tabServerSignature30)
        Me.tabCtrlServerGrid.Location = New System.Drawing.Point(0, 1)
        Me.tabCtrlServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.tabCtrlServerGrid.Name = "tabCtrlServerGrid"
        Me.tabCtrlServerGrid.SelectedIndex = 0
        Me.tabCtrlServerGrid.Size = New System.Drawing.Size(1023, 633)
        Me.tabCtrlServerGrid.TabIndex = 69
        '
        'tabServerSummary
        '
        Me.tabServerSummary.Controls.Add(Me.dgvServerGrid)
        Me.tabServerSummary.Location = New System.Drawing.Point(4, 28)
        Me.tabServerSummary.Margin = New System.Windows.Forms.Padding(4)
        Me.tabServerSummary.Name = "tabServerSummary"
        Me.tabServerSummary.Padding = New System.Windows.Forms.Padding(4)
        Me.tabServerSummary.Size = New System.Drawing.Size(1015, 601)
        Me.tabServerSummary.TabIndex = 0
        Me.tabServerSummary.Text = "Scalability Summary"
        Me.tabServerSummary.UseVisualStyleBackColor = True
        '
        'dgvServerGrid
        '
        Me.dgvServerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvServerGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvServerGrid.Location = New System.Drawing.Point(4, 4)
        Me.dgvServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvServerGrid.Name = "dgvServerGrid"
        Me.dgvServerGrid.Size = New System.Drawing.Size(1007, 593)
        Me.dgvServerGrid.TabIndex = 1
        '
        'tabServerLastCollected24
        '
        Me.tabServerLastCollected24.Controls.Add(Me.dgvServerLastCollected24)
        Me.tabServerLastCollected24.Location = New System.Drawing.Point(4, 28)
        Me.tabServerLastCollected24.Margin = New System.Windows.Forms.Padding(4)
        Me.tabServerLastCollected24.Name = "tabServerLastCollected24"
        Me.tabServerLastCollected24.Padding = New System.Windows.Forms.Padding(4)
        Me.tabServerLastCollected24.Size = New System.Drawing.Size(1015, 601)
        Me.tabServerLastCollected24.TabIndex = 1
        Me.tabServerLastCollected24.Text = "Last Collected [>24 hours]"
        Me.tabServerLastCollected24.UseVisualStyleBackColor = True
        '
        'dgvServerLastCollected24
        '
        Me.dgvServerLastCollected24.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvServerLastCollected24.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvServerLastCollected24.Location = New System.Drawing.Point(4, 4)
        Me.dgvServerLastCollected24.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvServerLastCollected24.Name = "dgvServerLastCollected24"
        Me.dgvServerLastCollected24.Size = New System.Drawing.Size(1007, 593)
        Me.dgvServerLastCollected24.TabIndex = 2
        '
        'tabServerSignature30
        '
        Me.tabServerSignature30.Controls.Add(Me.dgvServerSignature30)
        Me.tabServerSignature30.Location = New System.Drawing.Point(4, 28)
        Me.tabServerSignature30.Margin = New System.Windows.Forms.Padding(4)
        Me.tabServerSignature30.Name = "tabServerSignature30"
        Me.tabServerSignature30.Padding = New System.Windows.Forms.Padding(4)
        Me.tabServerSignature30.Size = New System.Drawing.Size(1015, 601)
        Me.tabServerSignature30.TabIndex = 2
        Me.tabServerSignature30.Text = "Signature File [>30 days]"
        Me.tabServerSignature30.UseVisualStyleBackColor = True
        '
        'dgvServerSignature30
        '
        Me.dgvServerSignature30.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvServerSignature30.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvServerSignature30.Location = New System.Drawing.Point(4, 4)
        Me.dgvServerSignature30.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvServerSignature30.Name = "dgvServerSignature30"
        Me.dgvServerSignature30.Size = New System.Drawing.Size(1007, 593)
        Me.dgvServerSignature30.TabIndex = 3
        '
        'prgServerGrid
        '
        Me.prgServerGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgServerGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgServerGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgServerGrid.Name = "prgServerGrid"
        Me.prgServerGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgServerGrid.Step = 1
        Me.prgServerGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgServerGrid.TabIndex = 70
        '
        'pnlSqlServerGridButtons
        '
        Me.pnlSqlServerGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlServerGridButtons.Controls.Add(Me.btnSqlConnectServerGrid)
        Me.pnlSqlServerGridButtons.Controls.Add(Me.btnSqlDisconnectServerGrid)
        Me.pnlSqlServerGridButtons.Controls.Add(Me.btnSqlRefreshServerGrid)
        Me.pnlSqlServerGridButtons.Controls.Add(Me.btnSqlExportServerGrid)
        Me.pnlSqlServerGridButtons.Controls.Add(Me.btnSqlExitServerGrid)
        Me.pnlSqlServerGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlServerGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlServerGridButtons.Name = "pnlSqlServerGridButtons"
        Me.pnlSqlServerGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlServerGridButtons.TabIndex = 68
        '
        'btnSqlConnectServerGrid
        '
        Me.btnSqlConnectServerGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectServerGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectServerGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectServerGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectServerGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectServerGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectServerGrid.Name = "btnSqlConnectServerGrid"
        Me.btnSqlConnectServerGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectServerGrid.TabIndex = 77
        Me.btnSqlConnectServerGrid.Text = "&Connect Sql"
        Me.btnSqlConnectServerGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectServerGrid
        '
        Me.btnSqlDisconnectServerGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectServerGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectServerGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectServerGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectServerGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectServerGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectServerGrid.Name = "btnSqlDisconnectServerGrid"
        Me.btnSqlDisconnectServerGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectServerGrid.TabIndex = 78
        Me.btnSqlDisconnectServerGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectServerGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshServerGrid
        '
        Me.btnSqlRefreshServerGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshServerGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshServerGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshServerGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshServerGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshServerGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshServerGrid.Name = "btnSqlRefreshServerGrid"
        Me.btnSqlRefreshServerGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshServerGrid.TabIndex = 79
        Me.btnSqlRefreshServerGrid.Text = "&Refresh"
        Me.btnSqlRefreshServerGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportServerGrid
        '
        Me.btnSqlExportServerGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportServerGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportServerGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportServerGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportServerGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportServerGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportServerGrid.Name = "btnSqlExportServerGrid"
        Me.btnSqlExportServerGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportServerGrid.TabIndex = 80
        Me.btnSqlExportServerGrid.Text = "&Export"
        Me.btnSqlExportServerGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitServerGrid
        '
        Me.btnSqlExitServerGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitServerGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitServerGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitServerGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitServerGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitServerGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitServerGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitServerGrid.Name = "btnSqlExitServerGrid"
        Me.btnSqlExitServerGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitServerGrid.TabIndex = 81
        Me.btnSqlExitServerGrid.Text = "E&xit"
        Me.btnSqlExitServerGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlEngineGrid
        '
        Me.pnlSqlEngineGrid.Controls.Add(Me.grpSqlEngineGrid)
        Me.pnlSqlEngineGrid.Controls.Add(Me.prgEngineGrid)
        Me.pnlSqlEngineGrid.Controls.Add(Me.pnlSqlEngineGridButtons)
        Me.pnlSqlEngineGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlEngineGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlEngineGrid.Name = "pnlSqlEngineGrid"
        Me.pnlSqlEngineGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlEngineGrid.TabIndex = 38
        '
        'grpSqlEngineGrid
        '
        Me.grpSqlEngineGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlEngineGrid.Controls.Add(Me.dgvEngineGrid)
        Me.grpSqlEngineGrid.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlEngineGrid.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlEngineGrid.Name = "grpSqlEngineGrid"
        Me.grpSqlEngineGrid.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlEngineGrid.Size = New System.Drawing.Size(1023, 634)
        Me.grpSqlEngineGrid.TabIndex = 0
        Me.grpSqlEngineGrid.TabStop = False
        Me.grpSqlEngineGrid.Text = "Engine Summary"
        '
        'dgvEngineGrid
        '
        Me.dgvEngineGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEngineGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEngineGrid.Location = New System.Drawing.Point(4, 27)
        Me.dgvEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvEngineGrid.Name = "dgvEngineGrid"
        Me.dgvEngineGrid.Size = New System.Drawing.Size(1015, 603)
        Me.dgvEngineGrid.TabIndex = 0
        '
        'prgEngineGrid
        '
        Me.prgEngineGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgEngineGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgEngineGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgEngineGrid.Name = "prgEngineGrid"
        Me.prgEngineGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgEngineGrid.Step = 1
        Me.prgEngineGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgEngineGrid.TabIndex = 69
        '
        'pnlSqlEngineGridButtons
        '
        Me.pnlSqlEngineGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlEngineGridButtons.Controls.Add(Me.btnSqlConnectEngineGrid)
        Me.pnlSqlEngineGridButtons.Controls.Add(Me.btnSqlDisconnectEngineGrid)
        Me.pnlSqlEngineGridButtons.Controls.Add(Me.btnSqlRefreshEngineGrid)
        Me.pnlSqlEngineGridButtons.Controls.Add(Me.btnSqlExportEngineGrid)
        Me.pnlSqlEngineGridButtons.Controls.Add(Me.btnSqlExitEngineGrid)
        Me.pnlSqlEngineGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlEngineGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlEngineGridButtons.Name = "pnlSqlEngineGridButtons"
        Me.pnlSqlEngineGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlEngineGridButtons.TabIndex = 68
        '
        'btnSqlConnectEngineGrid
        '
        Me.btnSqlConnectEngineGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectEngineGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectEngineGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectEngineGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectEngineGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectEngineGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectEngineGrid.Name = "btnSqlConnectEngineGrid"
        Me.btnSqlConnectEngineGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectEngineGrid.TabIndex = 77
        Me.btnSqlConnectEngineGrid.Text = "&Connect Sql"
        Me.btnSqlConnectEngineGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectEngineGrid
        '
        Me.btnSqlDisconnectEngineGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectEngineGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectEngineGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectEngineGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectEngineGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectEngineGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectEngineGrid.Name = "btnSqlDisconnectEngineGrid"
        Me.btnSqlDisconnectEngineGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectEngineGrid.TabIndex = 78
        Me.btnSqlDisconnectEngineGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectEngineGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshEngineGrid
        '
        Me.btnSqlRefreshEngineGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshEngineGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshEngineGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshEngineGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshEngineGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshEngineGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshEngineGrid.Name = "btnSqlRefreshEngineGrid"
        Me.btnSqlRefreshEngineGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshEngineGrid.TabIndex = 79
        Me.btnSqlRefreshEngineGrid.Text = "&Refresh"
        Me.btnSqlRefreshEngineGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportEngineGrid
        '
        Me.btnSqlExportEngineGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportEngineGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportEngineGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportEngineGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportEngineGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportEngineGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportEngineGrid.Name = "btnSqlExportEngineGrid"
        Me.btnSqlExportEngineGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportEngineGrid.TabIndex = 80
        Me.btnSqlExportEngineGrid.Text = "&Export"
        Me.btnSqlExportEngineGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitEngineGrid
        '
        Me.btnSqlExitEngineGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitEngineGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitEngineGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitEngineGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitEngineGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitEngineGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitEngineGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitEngineGrid.Name = "btnSqlExitEngineGrid"
        Me.btnSqlExitEngineGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitEngineGrid.TabIndex = 81
        Me.btnSqlExitEngineGrid.Text = "E&xit"
        Me.btnSqlExitEngineGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlGroupEvalGrid
        '
        Me.pnlSqlGroupEvalGrid.Controls.Add(Me.grpSqlGroupEvalGrid)
        Me.pnlSqlGroupEvalGrid.Controls.Add(Me.prgGroupEvalGrid)
        Me.pnlSqlGroupEvalGrid.Controls.Add(Me.grpSqlGroupEvalGridInst)
        Me.pnlSqlGroupEvalGrid.Controls.Add(Me.pnlSqlGroupEvalGridButtons)
        Me.pnlSqlGroupEvalGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlGroupEvalGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlGroupEvalGrid.Name = "pnlSqlGroupEvalGrid"
        Me.pnlSqlGroupEvalGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlGroupEvalGrid.TabIndex = 46
        '
        'grpSqlGroupEvalGrid
        '
        Me.grpSqlGroupEvalGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlGroupEvalGrid.Controls.Add(Me.dgvGroupEvalGrid)
        Me.grpSqlGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlGroupEvalGrid.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlGroupEvalGrid.Name = "grpSqlGroupEvalGrid"
        Me.grpSqlGroupEvalGrid.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlGroupEvalGrid.Size = New System.Drawing.Size(1023, 440)
        Me.grpSqlGroupEvalGrid.TabIndex = 70
        Me.grpSqlGroupEvalGrid.TabStop = False
        Me.grpSqlGroupEvalGrid.Text = "Dynamic Group Evaluation"
        '
        'dgvGroupEvalGrid
        '
        Me.dgvGroupEvalGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGroupEvalGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvGroupEvalGrid.Location = New System.Drawing.Point(4, 27)
        Me.dgvGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvGroupEvalGrid.Name = "dgvGroupEvalGrid"
        Me.dgvGroupEvalGrid.Size = New System.Drawing.Size(1015, 409)
        Me.dgvGroupEvalGrid.TabIndex = 0
        '
        'prgGroupEvalGrid
        '
        Me.prgGroupEvalGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgGroupEvalGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgGroupEvalGrid.Location = New System.Drawing.Point(0, 410)
        Me.prgGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgGroupEvalGrid.Name = "prgGroupEvalGrid"
        Me.prgGroupEvalGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgGroupEvalGrid.Step = 1
        Me.prgGroupEvalGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgGroupEvalGrid.TabIndex = 71
        '
        'grpSqlGroupEvalGridInst
        '
        Me.grpSqlGroupEvalGridInst.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlGroupEvalGridInst.Controls.Add(Me.btnGroupEvalGridPreview)
        Me.grpSqlGroupEvalGridInst.Controls.Add(Me.lblGroupEvalGrid)
        Me.grpSqlGroupEvalGridInst.Controls.Add(Me.btnGroupEvalGridCommit)
        Me.grpSqlGroupEvalGridInst.Controls.Add(Me.btnGroupEvalGridDiscard)
        Me.grpSqlGroupEvalGridInst.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlGroupEvalGridInst.Location = New System.Drawing.Point(0, 440)
        Me.grpSqlGroupEvalGridInst.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlGroupEvalGridInst.Name = "grpSqlGroupEvalGridInst"
        Me.grpSqlGroupEvalGridInst.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlGroupEvalGridInst.Size = New System.Drawing.Size(1023, 192)
        Me.grpSqlGroupEvalGridInst.TabIndex = 72
        Me.grpSqlGroupEvalGridInst.TabStop = False
        Me.grpSqlGroupEvalGridInst.Text = "Instructions"
        '
        'btnGroupEvalGridPreview
        '
        Me.btnGroupEvalGridPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGroupEvalGridPreview.BackColor = System.Drawing.Color.Peru
        Me.btnGroupEvalGridPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGroupEvalGridPreview.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGroupEvalGridPreview.ForeColor = System.Drawing.Color.White
        Me.btnGroupEvalGridPreview.Location = New System.Drawing.Point(851, 91)
        Me.btnGroupEvalGridPreview.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGroupEvalGridPreview.Name = "btnGroupEvalGridPreview"
        Me.btnGroupEvalGridPreview.Size = New System.Drawing.Size(158, 38)
        Me.btnGroupEvalGridPreview.TabIndex = 85
        Me.btnGroupEvalGridPreview.Text = "&Preview"
        Me.btnGroupEvalGridPreview.UseVisualStyleBackColor = False
        '
        'lblGroupEvalGrid
        '
        Me.lblGroupEvalGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGroupEvalGrid.Location = New System.Drawing.Point(12, 25)
        Me.lblGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGroupEvalGrid.Name = "lblGroupEvalGrid"
        Me.lblGroupEvalGrid.Size = New System.Drawing.Size(831, 164)
        Me.lblGroupEvalGrid.TabIndex = 84
        Me.lblGroupEvalGrid.Text = resources.GetString("lblGroupEvalGrid.Text")
        '
        'btnGroupEvalGridCommit
        '
        Me.btnGroupEvalGridCommit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGroupEvalGridCommit.BackColor = System.Drawing.Color.CadetBlue
        Me.btnGroupEvalGridCommit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGroupEvalGridCommit.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGroupEvalGridCommit.ForeColor = System.Drawing.Color.White
        Me.btnGroupEvalGridCommit.Location = New System.Drawing.Point(851, 39)
        Me.btnGroupEvalGridCommit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGroupEvalGridCommit.Name = "btnGroupEvalGridCommit"
        Me.btnGroupEvalGridCommit.Size = New System.Drawing.Size(158, 38)
        Me.btnGroupEvalGridCommit.TabIndex = 83
        Me.btnGroupEvalGridCommit.Text = "Co&mmit"
        Me.btnGroupEvalGridCommit.UseVisualStyleBackColor = False
        '
        'btnGroupEvalGridDiscard
        '
        Me.btnGroupEvalGridDiscard.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGroupEvalGridDiscard.BackColor = System.Drawing.Color.LightCoral
        Me.btnGroupEvalGridDiscard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGroupEvalGridDiscard.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGroupEvalGridDiscard.ForeColor = System.Drawing.Color.White
        Me.btnGroupEvalGridDiscard.Location = New System.Drawing.Point(851, 144)
        Me.btnGroupEvalGridDiscard.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGroupEvalGridDiscard.Name = "btnGroupEvalGridDiscard"
        Me.btnGroupEvalGridDiscard.Size = New System.Drawing.Size(158, 38)
        Me.btnGroupEvalGridDiscard.TabIndex = 82
        Me.btnGroupEvalGridDiscard.Text = "Dis&card"
        Me.btnGroupEvalGridDiscard.UseVisualStyleBackColor = False
        '
        'pnlSqlGroupEvalGridButtons
        '
        Me.pnlSqlGroupEvalGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlGroupEvalGridButtons.Controls.Add(Me.btnSqlConnectGroupEvalGrid)
        Me.pnlSqlGroupEvalGridButtons.Controls.Add(Me.btnSqlDisconnectGroupEvalGrid)
        Me.pnlSqlGroupEvalGridButtons.Controls.Add(Me.btnSqlRefreshGroupEvalGrid)
        Me.pnlSqlGroupEvalGridButtons.Controls.Add(Me.btnSqlExportGroupEvalGrid)
        Me.pnlSqlGroupEvalGridButtons.Controls.Add(Me.btnSqlExitGroupEvalGrid)
        Me.pnlSqlGroupEvalGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlGroupEvalGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlGroupEvalGridButtons.Name = "pnlSqlGroupEvalGridButtons"
        Me.pnlSqlGroupEvalGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlGroupEvalGridButtons.TabIndex = 69
        '
        'btnSqlConnectGroupEvalGrid
        '
        Me.btnSqlConnectGroupEvalGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectGroupEvalGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectGroupEvalGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectGroupEvalGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectGroupEvalGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectGroupEvalGrid.Name = "btnSqlConnectGroupEvalGrid"
        Me.btnSqlConnectGroupEvalGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectGroupEvalGrid.TabIndex = 77
        Me.btnSqlConnectGroupEvalGrid.Text = "&Connect Sql"
        Me.btnSqlConnectGroupEvalGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectGroupEvalGrid
        '
        Me.btnSqlDisconnectGroupEvalGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectGroupEvalGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectGroupEvalGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectGroupEvalGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectGroupEvalGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectGroupEvalGrid.Name = "btnSqlDisconnectGroupEvalGrid"
        Me.btnSqlDisconnectGroupEvalGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectGroupEvalGrid.TabIndex = 78
        Me.btnSqlDisconnectGroupEvalGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectGroupEvalGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshGroupEvalGrid
        '
        Me.btnSqlRefreshGroupEvalGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshGroupEvalGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshGroupEvalGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshGroupEvalGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshGroupEvalGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshGroupEvalGrid.Name = "btnSqlRefreshGroupEvalGrid"
        Me.btnSqlRefreshGroupEvalGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshGroupEvalGrid.TabIndex = 79
        Me.btnSqlRefreshGroupEvalGrid.Text = "&Refresh"
        Me.btnSqlRefreshGroupEvalGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportGroupEvalGrid
        '
        Me.btnSqlExportGroupEvalGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportGroupEvalGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportGroupEvalGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportGroupEvalGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportGroupEvalGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportGroupEvalGrid.Name = "btnSqlExportGroupEvalGrid"
        Me.btnSqlExportGroupEvalGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportGroupEvalGrid.TabIndex = 80
        Me.btnSqlExportGroupEvalGrid.Text = "&Export"
        Me.btnSqlExportGroupEvalGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitGroupEvalGrid
        '
        Me.btnSqlExitGroupEvalGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitGroupEvalGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitGroupEvalGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitGroupEvalGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitGroupEvalGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitGroupEvalGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitGroupEvalGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitGroupEvalGrid.Name = "btnSqlExitGroupEvalGrid"
        Me.btnSqlExitGroupEvalGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitGroupEvalGrid.TabIndex = 81
        Me.btnSqlExitGroupEvalGrid.Text = "E&xit"
        Me.btnSqlExitGroupEvalGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlInstSoftGrid
        '
        Me.pnlSqlInstSoftGrid.Controls.Add(Me.grpSqlInstSoftGrid)
        Me.pnlSqlInstSoftGrid.Controls.Add(Me.prgInstSoftGrid)
        Me.pnlSqlInstSoftGrid.Controls.Add(Me.pnlSqlInstSoftGridButtons)
        Me.pnlSqlInstSoftGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlInstSoftGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlInstSoftGrid.Name = "pnlSqlInstSoftGrid"
        Me.pnlSqlInstSoftGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlInstSoftGrid.TabIndex = 42
        '
        'grpSqlInstSoftGrid
        '
        Me.grpSqlInstSoftGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlInstSoftGrid.Controls.Add(Me.dgvSoftInst)
        Me.grpSqlInstSoftGrid.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlInstSoftGrid.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlInstSoftGrid.Name = "grpSqlInstSoftGrid"
        Me.grpSqlInstSoftGrid.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlInstSoftGrid.Size = New System.Drawing.Size(1023, 633)
        Me.grpSqlInstSoftGrid.TabIndex = 70
        Me.grpSqlInstSoftGrid.TabStop = False
        Me.grpSqlInstSoftGrid.Text = "Installed Software"
        '
        'dgvSoftInst
        '
        Me.dgvSoftInst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSoftInst.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSoftInst.Location = New System.Drawing.Point(4, 27)
        Me.dgvSoftInst.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSoftInst.Name = "dgvSoftInst"
        Me.dgvSoftInst.Size = New System.Drawing.Size(1015, 602)
        Me.dgvSoftInst.TabIndex = 2
        '
        'prgInstSoftGrid
        '
        Me.prgInstSoftGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgInstSoftGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgInstSoftGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgInstSoftGrid.Name = "prgInstSoftGrid"
        Me.prgInstSoftGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgInstSoftGrid.Step = 1
        Me.prgInstSoftGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgInstSoftGrid.TabIndex = 72
        '
        'pnlSqlInstSoftGridButtons
        '
        Me.pnlSqlInstSoftGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlInstSoftGridButtons.Controls.Add(Me.btnSqlConnectInstSoftGrid)
        Me.pnlSqlInstSoftGridButtons.Controls.Add(Me.btnSqlDisconnectInstSoftGrid)
        Me.pnlSqlInstSoftGridButtons.Controls.Add(Me.btnSqlRefreshInstSoftGrid)
        Me.pnlSqlInstSoftGridButtons.Controls.Add(Me.btnSqlExportInstSoftGrid)
        Me.pnlSqlInstSoftGridButtons.Controls.Add(Me.btnSqlExitInstSoftGrid)
        Me.pnlSqlInstSoftGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlInstSoftGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlInstSoftGridButtons.Name = "pnlSqlInstSoftGridButtons"
        Me.pnlSqlInstSoftGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlInstSoftGridButtons.TabIndex = 69
        '
        'btnSqlConnectInstSoftGrid
        '
        Me.btnSqlConnectInstSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectInstSoftGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectInstSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectInstSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectInstSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectInstSoftGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectInstSoftGrid.Name = "btnSqlConnectInstSoftGrid"
        Me.btnSqlConnectInstSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectInstSoftGrid.TabIndex = 77
        Me.btnSqlConnectInstSoftGrid.Text = "&Connect Sql"
        Me.btnSqlConnectInstSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectInstSoftGrid
        '
        Me.btnSqlDisconnectInstSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectInstSoftGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectInstSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectInstSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectInstSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectInstSoftGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectInstSoftGrid.Name = "btnSqlDisconnectInstSoftGrid"
        Me.btnSqlDisconnectInstSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectInstSoftGrid.TabIndex = 78
        Me.btnSqlDisconnectInstSoftGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectInstSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshInstSoftGrid
        '
        Me.btnSqlRefreshInstSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshInstSoftGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshInstSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshInstSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshInstSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshInstSoftGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshInstSoftGrid.Name = "btnSqlRefreshInstSoftGrid"
        Me.btnSqlRefreshInstSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshInstSoftGrid.TabIndex = 79
        Me.btnSqlRefreshInstSoftGrid.Text = "&Refresh"
        Me.btnSqlRefreshInstSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportInstSoftGrid
        '
        Me.btnSqlExportInstSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportInstSoftGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportInstSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportInstSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportInstSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportInstSoftGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportInstSoftGrid.Name = "btnSqlExportInstSoftGrid"
        Me.btnSqlExportInstSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportInstSoftGrid.TabIndex = 80
        Me.btnSqlExportInstSoftGrid.Text = "&Export"
        Me.btnSqlExportInstSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitInstSoftGrid
        '
        Me.btnSqlExitInstSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitInstSoftGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitInstSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitInstSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitInstSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitInstSoftGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitInstSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitInstSoftGrid.Name = "btnSqlExitInstSoftGrid"
        Me.btnSqlExitInstSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitInstSoftGrid.TabIndex = 81
        Me.btnSqlExitInstSoftGrid.Text = "E&xit"
        Me.btnSqlExitInstSoftGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlDiscSoftGrid
        '
        Me.pnlSqlDiscSoftGrid.Controls.Add(Me.grpDiscSoftGrid)
        Me.pnlSqlDiscSoftGrid.Controls.Add(Me.tabCtrlDiscSoftGrid)
        Me.pnlSqlDiscSoftGrid.Controls.Add(Me.prgDiscSoftGrid)
        Me.pnlSqlDiscSoftGrid.Controls.Add(Me.pnlSqlDiscSoftGridButtons)
        Me.pnlSqlDiscSoftGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlDiscSoftGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlDiscSoftGrid.Name = "pnlSqlDiscSoftGrid"
        Me.pnlSqlDiscSoftGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlDiscSoftGrid.TabIndex = 43
        '
        'grpDiscSoftGrid
        '
        Me.grpDiscSoftGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDiscSoftGrid.Controls.Add(Me.lblDiscSoftGrid)
        Me.grpDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDiscSoftGrid.Location = New System.Drawing.Point(0, 0)
        Me.grpDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.grpDiscSoftGrid.Name = "grpDiscSoftGrid"
        Me.grpDiscSoftGrid.Padding = New System.Windows.Forms.Padding(4)
        Me.grpDiscSoftGrid.Size = New System.Drawing.Size(1023, 56)
        Me.grpDiscSoftGrid.TabIndex = 70
        Me.grpDiscSoftGrid.TabStop = False
        Me.grpDiscSoftGrid.Text = "Discovered Software"
        '
        'lblDiscSoftGrid
        '
        Me.lblDiscSoftGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscSoftGrid.Location = New System.Drawing.Point(4, 27)
        Me.lblDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDiscSoftGrid.Name = "lblDiscSoftGrid"
        Me.lblDiscSoftGrid.Size = New System.Drawing.Size(1015, 25)
        Me.lblDiscSoftGrid.TabIndex = 0
        Me.lblDiscSoftGrid.Text = "This is a report of discovered software detected by the ITCM Asset Management Age" &
    "nt.  The data is categorized based on the detection method."
        '
        'tabCtrlDiscSoftGrid
        '
        Me.tabCtrlDiscSoftGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabCtrlDiscSoftGrid.Controls.Add(Me.tabDiscSignature)
        Me.tabCtrlDiscSoftGrid.Controls.Add(Me.tabDiscCustom)
        Me.tabCtrlDiscSoftGrid.Controls.Add(Me.tabDiscHeuristic)
        Me.tabCtrlDiscSoftGrid.Controls.Add(Me.tabDiscIntellisig)
        Me.tabCtrlDiscSoftGrid.Controls.Add(Me.tabDiscEverything)
        Me.tabCtrlDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabCtrlDiscSoftGrid.Location = New System.Drawing.Point(0, 56)
        Me.tabCtrlDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.tabCtrlDiscSoftGrid.Name = "tabCtrlDiscSoftGrid"
        Me.tabCtrlDiscSoftGrid.SelectedIndex = 0
        Me.tabCtrlDiscSoftGrid.Size = New System.Drawing.Size(1023, 578)
        Me.tabCtrlDiscSoftGrid.TabIndex = 71
        '
        'tabDiscSignature
        '
        Me.tabDiscSignature.Controls.Add(Me.dgvDiscSignature)
        Me.tabDiscSignature.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDiscSignature.Location = New System.Drawing.Point(4, 28)
        Me.tabDiscSignature.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDiscSignature.Name = "tabDiscSignature"
        Me.tabDiscSignature.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDiscSignature.Size = New System.Drawing.Size(1015, 546)
        Me.tabDiscSignature.TabIndex = 0
        Me.tabDiscSignature.Text = "Signature"
        Me.tabDiscSignature.UseVisualStyleBackColor = True
        '
        'dgvDiscSignature
        '
        Me.dgvDiscSignature.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDiscSignature.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDiscSignature.Location = New System.Drawing.Point(4, 4)
        Me.dgvDiscSignature.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDiscSignature.Name = "dgvDiscSignature"
        Me.dgvDiscSignature.Size = New System.Drawing.Size(1007, 538)
        Me.dgvDiscSignature.TabIndex = 2
        '
        'tabDiscCustom
        '
        Me.tabDiscCustom.Controls.Add(Me.dgvDiscCustom)
        Me.tabDiscCustom.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDiscCustom.Location = New System.Drawing.Point(4, 28)
        Me.tabDiscCustom.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDiscCustom.Name = "tabDiscCustom"
        Me.tabDiscCustom.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDiscCustom.Size = New System.Drawing.Size(1015, 546)
        Me.tabDiscCustom.TabIndex = 4
        Me.tabDiscCustom.Text = "Custom Signature"
        Me.tabDiscCustom.UseVisualStyleBackColor = True
        '
        'dgvDiscCustom
        '
        Me.dgvDiscCustom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDiscCustom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDiscCustom.Location = New System.Drawing.Point(4, 4)
        Me.dgvDiscCustom.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDiscCustom.Name = "dgvDiscCustom"
        Me.dgvDiscCustom.Size = New System.Drawing.Size(1007, 538)
        Me.dgvDiscCustom.TabIndex = 2
        '
        'tabDiscHeuristic
        '
        Me.tabDiscHeuristic.Controls.Add(Me.dgvDiscHeuristic)
        Me.tabDiscHeuristic.Location = New System.Drawing.Point(4, 28)
        Me.tabDiscHeuristic.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDiscHeuristic.Name = "tabDiscHeuristic"
        Me.tabDiscHeuristic.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDiscHeuristic.Size = New System.Drawing.Size(1015, 546)
        Me.tabDiscHeuristic.TabIndex = 1
        Me.tabDiscHeuristic.Text = "Heuristic"
        Me.tabDiscHeuristic.UseVisualStyleBackColor = True
        '
        'dgvDiscHeuristic
        '
        Me.dgvDiscHeuristic.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDiscHeuristic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDiscHeuristic.Location = New System.Drawing.Point(4, 4)
        Me.dgvDiscHeuristic.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDiscHeuristic.Name = "dgvDiscHeuristic"
        Me.dgvDiscHeuristic.Size = New System.Drawing.Size(1007, 538)
        Me.dgvDiscHeuristic.TabIndex = 2
        '
        'tabDiscIntellisig
        '
        Me.tabDiscIntellisig.Controls.Add(Me.dgvDiscIntellisig)
        Me.tabDiscIntellisig.Location = New System.Drawing.Point(4, 28)
        Me.tabDiscIntellisig.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDiscIntellisig.Name = "tabDiscIntellisig"
        Me.tabDiscIntellisig.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDiscIntellisig.Size = New System.Drawing.Size(1015, 546)
        Me.tabDiscIntellisig.TabIndex = 2
        Me.tabDiscIntellisig.Text = "Intellisig"
        Me.tabDiscIntellisig.UseVisualStyleBackColor = True
        '
        'dgvDiscIntellisig
        '
        Me.dgvDiscIntellisig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDiscIntellisig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDiscIntellisig.Location = New System.Drawing.Point(4, 4)
        Me.dgvDiscIntellisig.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDiscIntellisig.Name = "dgvDiscIntellisig"
        Me.dgvDiscIntellisig.Size = New System.Drawing.Size(1007, 538)
        Me.dgvDiscIntellisig.TabIndex = 3
        '
        'tabDiscEverything
        '
        Me.tabDiscEverything.Controls.Add(Me.dgvDiscEverything)
        Me.tabDiscEverything.Location = New System.Drawing.Point(4, 28)
        Me.tabDiscEverything.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDiscEverything.Name = "tabDiscEverything"
        Me.tabDiscEverything.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDiscEverything.Size = New System.Drawing.Size(1015, 546)
        Me.tabDiscEverything.TabIndex = 3
        Me.tabDiscEverything.Text = "Everything"
        Me.tabDiscEverything.UseVisualStyleBackColor = True
        '
        'dgvDiscEverything
        '
        Me.dgvDiscEverything.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDiscEverything.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDiscEverything.Location = New System.Drawing.Point(4, 4)
        Me.dgvDiscEverything.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDiscEverything.Name = "dgvDiscEverything"
        Me.dgvDiscEverything.Size = New System.Drawing.Size(1007, 538)
        Me.dgvDiscEverything.TabIndex = 3
        '
        'prgDiscSoftGrid
        '
        Me.prgDiscSoftGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgDiscSoftGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgDiscSoftGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgDiscSoftGrid.Name = "prgDiscSoftGrid"
        Me.prgDiscSoftGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgDiscSoftGrid.Step = 1
        Me.prgDiscSoftGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgDiscSoftGrid.TabIndex = 72
        '
        'pnlSqlDiscSoftGridButtons
        '
        Me.pnlSqlDiscSoftGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlDiscSoftGridButtons.Controls.Add(Me.btnSqlConnectDiscSoftGrid)
        Me.pnlSqlDiscSoftGridButtons.Controls.Add(Me.btnSqlDisconnectDiscSoftGrid)
        Me.pnlSqlDiscSoftGridButtons.Controls.Add(Me.btnSqlRefreshDiscSoftGrid)
        Me.pnlSqlDiscSoftGridButtons.Controls.Add(Me.btnSqlExportDiscSoftGrid)
        Me.pnlSqlDiscSoftGridButtons.Controls.Add(Me.btnSqlExitDiscSoftGrid)
        Me.pnlSqlDiscSoftGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlDiscSoftGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlDiscSoftGridButtons.Name = "pnlSqlDiscSoftGridButtons"
        Me.pnlSqlDiscSoftGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlDiscSoftGridButtons.TabIndex = 69
        '
        'btnSqlConnectDiscSoftGrid
        '
        Me.btnSqlConnectDiscSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectDiscSoftGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectDiscSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectDiscSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectDiscSoftGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectDiscSoftGrid.Name = "btnSqlConnectDiscSoftGrid"
        Me.btnSqlConnectDiscSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectDiscSoftGrid.TabIndex = 77
        Me.btnSqlConnectDiscSoftGrid.Text = "&Connect Sql"
        Me.btnSqlConnectDiscSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectDiscSoftGrid
        '
        Me.btnSqlDisconnectDiscSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectDiscSoftGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectDiscSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectDiscSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectDiscSoftGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectDiscSoftGrid.Name = "btnSqlDisconnectDiscSoftGrid"
        Me.btnSqlDisconnectDiscSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectDiscSoftGrid.TabIndex = 78
        Me.btnSqlDisconnectDiscSoftGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectDiscSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshDiscSoftGrid
        '
        Me.btnSqlRefreshDiscSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshDiscSoftGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshDiscSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshDiscSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshDiscSoftGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshDiscSoftGrid.Name = "btnSqlRefreshDiscSoftGrid"
        Me.btnSqlRefreshDiscSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshDiscSoftGrid.TabIndex = 79
        Me.btnSqlRefreshDiscSoftGrid.Text = "&Refresh"
        Me.btnSqlRefreshDiscSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportDiscSoftGrid
        '
        Me.btnSqlExportDiscSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportDiscSoftGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportDiscSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportDiscSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportDiscSoftGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportDiscSoftGrid.Name = "btnSqlExportDiscSoftGrid"
        Me.btnSqlExportDiscSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportDiscSoftGrid.TabIndex = 80
        Me.btnSqlExportDiscSoftGrid.Text = "&Export"
        Me.btnSqlExportDiscSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitDiscSoftGrid
        '
        Me.btnSqlExitDiscSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitDiscSoftGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitDiscSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitDiscSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitDiscSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitDiscSoftGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitDiscSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitDiscSoftGrid.Name = "btnSqlExitDiscSoftGrid"
        Me.btnSqlExitDiscSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitDiscSoftGrid.TabIndex = 81
        Me.btnSqlExitDiscSoftGrid.Text = "E&xit"
        Me.btnSqlExitDiscSoftGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlDuplCompGrid
        '
        Me.pnlSqlDuplCompGrid.Controls.Add(Me.tabCtrlDuplComp)
        Me.pnlSqlDuplCompGrid.Controls.Add(Me.prgDuplCompGrid)
        Me.pnlSqlDuplCompGrid.Controls.Add(Me.pnlSqlDuplCompGridButtons)
        Me.pnlSqlDuplCompGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlDuplCompGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlDuplCompGrid.Name = "pnlSqlDuplCompGrid"
        Me.pnlSqlDuplCompGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlDuplCompGrid.TabIndex = 41
        '
        'tabCtrlDuplComp
        '
        Me.tabCtrlDuplComp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabCtrlDuplComp.Controls.Add(Me.tabDuplHostname)
        Me.tabCtrlDuplComp.Controls.Add(Me.tabDuplSerialNum)
        Me.tabCtrlDuplComp.Controls.Add(Me.tabDuplBoth)
        Me.tabCtrlDuplComp.Controls.Add(Me.tabDuplBlank)
        Me.tabCtrlDuplComp.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabCtrlDuplComp.Location = New System.Drawing.Point(0, 1)
        Me.tabCtrlDuplComp.Margin = New System.Windows.Forms.Padding(4)
        Me.tabCtrlDuplComp.Name = "tabCtrlDuplComp"
        Me.tabCtrlDuplComp.SelectedIndex = 0
        Me.tabCtrlDuplComp.Size = New System.Drawing.Size(1023, 633)
        Me.tabCtrlDuplComp.TabIndex = 70
        '
        'tabDuplHostname
        '
        Me.tabDuplHostname.Controls.Add(Me.dgvDuplHostname)
        Me.tabDuplHostname.Location = New System.Drawing.Point(4, 28)
        Me.tabDuplHostname.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDuplHostname.Name = "tabDuplHostname"
        Me.tabDuplHostname.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDuplHostname.Size = New System.Drawing.Size(1015, 601)
        Me.tabDuplHostname.TabIndex = 0
        Me.tabDuplHostname.Text = "By Hostname"
        Me.tabDuplHostname.UseVisualStyleBackColor = True
        '
        'dgvDuplHostname
        '
        Me.dgvDuplHostname.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDuplHostname.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDuplHostname.Location = New System.Drawing.Point(4, 4)
        Me.dgvDuplHostname.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDuplHostname.Name = "dgvDuplHostname"
        Me.dgvDuplHostname.Size = New System.Drawing.Size(1007, 593)
        Me.dgvDuplHostname.TabIndex = 1
        '
        'tabDuplSerialNum
        '
        Me.tabDuplSerialNum.Controls.Add(Me.dgvDuplSerialNum)
        Me.tabDuplSerialNum.Location = New System.Drawing.Point(4, 28)
        Me.tabDuplSerialNum.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDuplSerialNum.Name = "tabDuplSerialNum"
        Me.tabDuplSerialNum.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDuplSerialNum.Size = New System.Drawing.Size(1015, 601)
        Me.tabDuplSerialNum.TabIndex = 1
        Me.tabDuplSerialNum.Text = "By Serial Number"
        Me.tabDuplSerialNum.UseVisualStyleBackColor = True
        '
        'dgvDuplSerialNum
        '
        Me.dgvDuplSerialNum.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDuplSerialNum.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDuplSerialNum.Location = New System.Drawing.Point(4, 4)
        Me.dgvDuplSerialNum.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDuplSerialNum.Name = "dgvDuplSerialNum"
        Me.dgvDuplSerialNum.Size = New System.Drawing.Size(1007, 593)
        Me.dgvDuplSerialNum.TabIndex = 0
        '
        'tabDuplBoth
        '
        Me.tabDuplBoth.Controls.Add(Me.dgvDuplBoth)
        Me.tabDuplBoth.Location = New System.Drawing.Point(4, 28)
        Me.tabDuplBoth.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDuplBoth.Name = "tabDuplBoth"
        Me.tabDuplBoth.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDuplBoth.Size = New System.Drawing.Size(1015, 601)
        Me.tabDuplBoth.TabIndex = 2
        Me.tabDuplBoth.Text = "By Hostname & Serial Number"
        Me.tabDuplBoth.UseVisualStyleBackColor = True
        '
        'dgvDuplBoth
        '
        Me.dgvDuplBoth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDuplBoth.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDuplBoth.Location = New System.Drawing.Point(4, 4)
        Me.dgvDuplBoth.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDuplBoth.Name = "dgvDuplBoth"
        Me.dgvDuplBoth.Size = New System.Drawing.Size(1007, 593)
        Me.dgvDuplBoth.TabIndex = 0
        '
        'tabDuplBlank
        '
        Me.tabDuplBlank.Controls.Add(Me.dgvDuplBlank)
        Me.tabDuplBlank.Location = New System.Drawing.Point(4, 28)
        Me.tabDuplBlank.Margin = New System.Windows.Forms.Padding(4)
        Me.tabDuplBlank.Name = "tabDuplBlank"
        Me.tabDuplBlank.Padding = New System.Windows.Forms.Padding(4)
        Me.tabDuplBlank.Size = New System.Drawing.Size(1015, 601)
        Me.tabDuplBlank.TabIndex = 3
        Me.tabDuplBlank.Text = "Missing Serial Number"
        Me.tabDuplBlank.UseVisualStyleBackColor = True
        '
        'dgvDuplBlank
        '
        Me.dgvDuplBlank.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDuplBlank.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDuplBlank.Location = New System.Drawing.Point(4, 4)
        Me.dgvDuplBlank.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDuplBlank.Name = "dgvDuplBlank"
        Me.dgvDuplBlank.Size = New System.Drawing.Size(1007, 593)
        Me.dgvDuplBlank.TabIndex = 1
        '
        'prgDuplCompGrid
        '
        Me.prgDuplCompGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgDuplCompGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgDuplCompGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgDuplCompGrid.Name = "prgDuplCompGrid"
        Me.prgDuplCompGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgDuplCompGrid.Step = 1
        Me.prgDuplCompGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgDuplCompGrid.TabIndex = 71
        '
        'pnlSqlDuplCompGridButtons
        '
        Me.pnlSqlDuplCompGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlDuplCompGridButtons.Controls.Add(Me.btnSqlConnectDuplCompGrid)
        Me.pnlSqlDuplCompGridButtons.Controls.Add(Me.btnSqlDisconnectDuplCompGrid)
        Me.pnlSqlDuplCompGridButtons.Controls.Add(Me.btnSqlRefreshDuplCompGrid)
        Me.pnlSqlDuplCompGridButtons.Controls.Add(Me.btnSqlExportDuplCompGrid)
        Me.pnlSqlDuplCompGridButtons.Controls.Add(Me.btnSqlExitDuplCompGrid)
        Me.pnlSqlDuplCompGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlDuplCompGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlDuplCompGridButtons.Name = "pnlSqlDuplCompGridButtons"
        Me.pnlSqlDuplCompGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlDuplCompGridButtons.TabIndex = 69
        '
        'btnSqlConnectDuplCompGrid
        '
        Me.btnSqlConnectDuplCompGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectDuplCompGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectDuplCompGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectDuplCompGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectDuplCompGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectDuplCompGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectDuplCompGrid.Name = "btnSqlConnectDuplCompGrid"
        Me.btnSqlConnectDuplCompGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectDuplCompGrid.TabIndex = 77
        Me.btnSqlConnectDuplCompGrid.Text = "&Connect Sql"
        Me.btnSqlConnectDuplCompGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectDuplCompGrid
        '
        Me.btnSqlDisconnectDuplCompGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectDuplCompGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectDuplCompGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectDuplCompGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectDuplCompGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectDuplCompGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectDuplCompGrid.Name = "btnSqlDisconnectDuplCompGrid"
        Me.btnSqlDisconnectDuplCompGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectDuplCompGrid.TabIndex = 78
        Me.btnSqlDisconnectDuplCompGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectDuplCompGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshDuplCompGrid
        '
        Me.btnSqlRefreshDuplCompGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshDuplCompGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshDuplCompGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshDuplCompGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshDuplCompGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshDuplCompGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshDuplCompGrid.Name = "btnSqlRefreshDuplCompGrid"
        Me.btnSqlRefreshDuplCompGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshDuplCompGrid.TabIndex = 79
        Me.btnSqlRefreshDuplCompGrid.Text = "&Refresh"
        Me.btnSqlRefreshDuplCompGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportDuplCompGrid
        '
        Me.btnSqlExportDuplCompGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportDuplCompGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportDuplCompGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportDuplCompGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportDuplCompGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportDuplCompGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportDuplCompGrid.Name = "btnSqlExportDuplCompGrid"
        Me.btnSqlExportDuplCompGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportDuplCompGrid.TabIndex = 80
        Me.btnSqlExportDuplCompGrid.Text = "&Export"
        Me.btnSqlExportDuplCompGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitDuplCompGrid
        '
        Me.btnSqlExitDuplCompGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitDuplCompGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitDuplCompGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitDuplCompGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitDuplCompGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitDuplCompGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitDuplCompGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitDuplCompGrid.Name = "btnSqlExitDuplCompGrid"
        Me.btnSqlExitDuplCompGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitDuplCompGrid.TabIndex = 81
        Me.btnSqlExitDuplCompGrid.Text = "E&xit"
        Me.btnSqlExitDuplCompGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlUnUsedSoftGrid
        '
        Me.pnlSqlUnUsedSoftGrid.Controls.Add(Me.tabCtrlSwNotUsed)
        Me.pnlSqlUnUsedSoftGrid.Controls.Add(Me.prgUnUsedSoftGrid)
        Me.pnlSqlUnUsedSoftGrid.Controls.Add(Me.pnlSqlUnUsedSoftGridButtons)
        Me.pnlSqlUnUsedSoftGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlUnUsedSoftGrid.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlUnUsedSoftGrid.Name = "pnlSqlUnUsedSoftGrid"
        Me.pnlSqlUnUsedSoftGrid.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlUnUsedSoftGrid.TabIndex = 40
        '
        'tabCtrlSwNotUsed
        '
        Me.tabCtrlSwNotUsed.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabCtrlSwNotUsed.Controls.Add(Me.tabSwNotUsed)
        Me.tabCtrlSwNotUsed.Controls.Add(Me.tabSwNotInst)
        Me.tabCtrlSwNotUsed.Controls.Add(Me.tabSwNotStaged)
        Me.tabCtrlSwNotUsed.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabCtrlSwNotUsed.Location = New System.Drawing.Point(0, 1)
        Me.tabCtrlSwNotUsed.Margin = New System.Windows.Forms.Padding(4)
        Me.tabCtrlSwNotUsed.Name = "tabCtrlSwNotUsed"
        Me.tabCtrlSwNotUsed.SelectedIndex = 0
        Me.tabCtrlSwNotUsed.Size = New System.Drawing.Size(1023, 633)
        Me.tabCtrlSwNotUsed.TabIndex = 4
        '
        'tabSwNotUsed
        '
        Me.tabSwNotUsed.Controls.Add(Me.dgvSwNotUsed)
        Me.tabSwNotUsed.Location = New System.Drawing.Point(4, 28)
        Me.tabSwNotUsed.Margin = New System.Windows.Forms.Padding(4)
        Me.tabSwNotUsed.Name = "tabSwNotUsed"
        Me.tabSwNotUsed.Padding = New System.Windows.Forms.Padding(4)
        Me.tabSwNotUsed.Size = New System.Drawing.Size(1015, 601)
        Me.tabSwNotUsed.TabIndex = 0
        Me.tabSwNotUsed.Text = "Software Not Used (Installed or Staged)"
        Me.tabSwNotUsed.UseVisualStyleBackColor = True
        '
        'dgvSwNotUsed
        '
        Me.dgvSwNotUsed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSwNotUsed.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSwNotUsed.Location = New System.Drawing.Point(4, 4)
        Me.dgvSwNotUsed.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSwNotUsed.Name = "dgvSwNotUsed"
        Me.dgvSwNotUsed.Size = New System.Drawing.Size(1007, 593)
        Me.dgvSwNotUsed.TabIndex = 1
        '
        'tabSwNotInst
        '
        Me.tabSwNotInst.Controls.Add(Me.dgvSwNotInst)
        Me.tabSwNotInst.Location = New System.Drawing.Point(4, 28)
        Me.tabSwNotInst.Margin = New System.Windows.Forms.Padding(4)
        Me.tabSwNotInst.Name = "tabSwNotInst"
        Me.tabSwNotInst.Padding = New System.Windows.Forms.Padding(4)
        Me.tabSwNotInst.Size = New System.Drawing.Size(1015, 601)
        Me.tabSwNotInst.TabIndex = 1
        Me.tabSwNotInst.Text = "Software Not Installed"
        Me.tabSwNotInst.UseVisualStyleBackColor = True
        '
        'dgvSwNotInst
        '
        Me.dgvSwNotInst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSwNotInst.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSwNotInst.Location = New System.Drawing.Point(4, 4)
        Me.dgvSwNotInst.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSwNotInst.Name = "dgvSwNotInst"
        Me.dgvSwNotInst.Size = New System.Drawing.Size(1007, 593)
        Me.dgvSwNotInst.TabIndex = 0
        '
        'tabSwNotStaged
        '
        Me.tabSwNotStaged.Controls.Add(Me.dgvSwNotStaged)
        Me.tabSwNotStaged.Location = New System.Drawing.Point(4, 28)
        Me.tabSwNotStaged.Margin = New System.Windows.Forms.Padding(4)
        Me.tabSwNotStaged.Name = "tabSwNotStaged"
        Me.tabSwNotStaged.Padding = New System.Windows.Forms.Padding(4)
        Me.tabSwNotStaged.Size = New System.Drawing.Size(1015, 601)
        Me.tabSwNotStaged.TabIndex = 2
        Me.tabSwNotStaged.Text = "Software Not Staged"
        Me.tabSwNotStaged.UseVisualStyleBackColor = True
        '
        'dgvSwNotStaged
        '
        Me.dgvSwNotStaged.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSwNotStaged.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSwNotStaged.Location = New System.Drawing.Point(4, 4)
        Me.dgvSwNotStaged.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSwNotStaged.Name = "dgvSwNotStaged"
        Me.dgvSwNotStaged.Size = New System.Drawing.Size(1007, 593)
        Me.dgvSwNotStaged.TabIndex = 0
        '
        'prgUnUsedSoftGrid
        '
        Me.prgUnUsedSoftGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prgUnUsedSoftGrid.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.prgUnUsedSoftGrid.Location = New System.Drawing.Point(0, 605)
        Me.prgUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.prgUnUsedSoftGrid.Name = "prgUnUsedSoftGrid"
        Me.prgUnUsedSoftGrid.Size = New System.Drawing.Size(1023, 29)
        Me.prgUnUsedSoftGrid.Step = 1
        Me.prgUnUsedSoftGrid.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prgUnUsedSoftGrid.TabIndex = 69
        '
        'pnlSqlUnUsedSoftGridButtons
        '
        Me.pnlSqlUnUsedSoftGridButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlUnUsedSoftGridButtons.Controls.Add(Me.btnSqlConnectUnUsedSoftGrid)
        Me.pnlSqlUnUsedSoftGridButtons.Controls.Add(Me.btnSqlDisconnectUnUsedSoftGrid)
        Me.pnlSqlUnUsedSoftGridButtons.Controls.Add(Me.btnSqlRefreshUnUsedSoftGrid)
        Me.pnlSqlUnUsedSoftGridButtons.Controls.Add(Me.btnSqlExportUnUsedSoftGrid)
        Me.pnlSqlUnUsedSoftGridButtons.Controls.Add(Me.btnSqlExitUnUsedSoftGrid)
        Me.pnlSqlUnUsedSoftGridButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlUnUsedSoftGridButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlUnUsedSoftGridButtons.Name = "pnlSqlUnUsedSoftGridButtons"
        Me.pnlSqlUnUsedSoftGridButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlUnUsedSoftGridButtons.TabIndex = 68
        '
        'btnSqlConnectUnUsedSoftGrid
        '
        Me.btnSqlConnectUnUsedSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectUnUsedSoftGrid.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectUnUsedSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectUnUsedSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectUnUsedSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectUnUsedSoftGrid.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectUnUsedSoftGrid.Name = "btnSqlConnectUnUsedSoftGrid"
        Me.btnSqlConnectUnUsedSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectUnUsedSoftGrid.TabIndex = 77
        Me.btnSqlConnectUnUsedSoftGrid.Text = "&Connect Sql"
        Me.btnSqlConnectUnUsedSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectUnUsedSoftGrid
        '
        Me.btnSqlDisconnectUnUsedSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectUnUsedSoftGrid.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectUnUsedSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectUnUsedSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectUnUsedSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectUnUsedSoftGrid.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectUnUsedSoftGrid.Name = "btnSqlDisconnectUnUsedSoftGrid"
        Me.btnSqlDisconnectUnUsedSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectUnUsedSoftGrid.TabIndex = 78
        Me.btnSqlDisconnectUnUsedSoftGrid.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectUnUsedSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlRefreshUnUsedSoftGrid
        '
        Me.btnSqlRefreshUnUsedSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRefreshUnUsedSoftGrid.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlRefreshUnUsedSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRefreshUnUsedSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRefreshUnUsedSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlRefreshUnUsedSoftGrid.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlRefreshUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRefreshUnUsedSoftGrid.Name = "btnSqlRefreshUnUsedSoftGrid"
        Me.btnSqlRefreshUnUsedSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRefreshUnUsedSoftGrid.TabIndex = 79
        Me.btnSqlRefreshUnUsedSoftGrid.Text = "&Refresh"
        Me.btnSqlRefreshUnUsedSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExportUnUsedSoftGrid
        '
        Me.btnSqlExportUnUsedSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExportUnUsedSoftGrid.BackColor = System.Drawing.Color.Tan
        Me.btnSqlExportUnUsedSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExportUnUsedSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExportUnUsedSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExportUnUsedSoftGrid.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlExportUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExportUnUsedSoftGrid.Name = "btnSqlExportUnUsedSoftGrid"
        Me.btnSqlExportUnUsedSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExportUnUsedSoftGrid.TabIndex = 80
        Me.btnSqlExportUnUsedSoftGrid.Text = "&Export"
        Me.btnSqlExportUnUsedSoftGrid.UseVisualStyleBackColor = False
        '
        'btnSqlExitUnUsedSoftGrid
        '
        Me.btnSqlExitUnUsedSoftGrid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitUnUsedSoftGrid.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitUnUsedSoftGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitUnUsedSoftGrid.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitUnUsedSoftGrid.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitUnUsedSoftGrid.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitUnUsedSoftGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitUnUsedSoftGrid.Name = "btnSqlExitUnUsedSoftGrid"
        Me.btnSqlExitUnUsedSoftGrid.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitUnUsedSoftGrid.TabIndex = 81
        Me.btnSqlExitUnUsedSoftGrid.Text = "E&xit"
        Me.btnSqlExitUnUsedSoftGrid.UseVisualStyleBackColor = False
        '
        'pnlSqlCleanApps
        '
        Me.pnlSqlCleanApps.Controls.Add(Me.grpSqlCleanAppsInfo)
        Me.pnlSqlCleanApps.Controls.Add(Me.grpSqlCleanAppsOutput)
        Me.pnlSqlCleanApps.Controls.Add(Me.pnlSqlCleanAppsButtons)
        Me.pnlSqlCleanApps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlCleanApps.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlCleanApps.Name = "pnlSqlCleanApps"
        Me.pnlSqlCleanApps.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlCleanApps.TabIndex = 28
        '
        'grpSqlCleanAppsInfo
        '
        Me.grpSqlCleanAppsInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlCleanAppsInfo.Controls.Add(Me.lblSqlCleanAppsIntro)
        Me.grpSqlCleanAppsInfo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlCleanAppsInfo.Location = New System.Drawing.Point(0, 440)
        Me.grpSqlCleanAppsInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlCleanAppsInfo.Name = "grpSqlCleanAppsInfo"
        Me.grpSqlCleanAppsInfo.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlCleanAppsInfo.Size = New System.Drawing.Size(1023, 194)
        Me.grpSqlCleanAppsInfo.TabIndex = 69
        Me.grpSqlCleanAppsInfo.TabStop = False
        Me.grpSqlCleanAppsInfo.Text = "CleanApps Tool Information"
        '
        'lblSqlCleanAppsIntro
        '
        Me.lblSqlCleanAppsIntro.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSqlCleanAppsIntro.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSqlCleanAppsIntro.Location = New System.Drawing.Point(4, 27)
        Me.lblSqlCleanAppsIntro.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSqlCleanAppsIntro.Name = "lblSqlCleanAppsIntro"
        Me.lblSqlCleanAppsIntro.Size = New System.Drawing.Size(1015, 163)
        Me.lblSqlCleanAppsIntro.TabIndex = 1
        Me.lblSqlCleanAppsIntro.Text = resources.GetString("lblSqlCleanAppsIntro.Text")
        '
        'grpSqlCleanAppsOutput
        '
        Me.grpSqlCleanAppsOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlCleanAppsOutput.Controls.Add(Me.txtSqlCleanApps)
        Me.grpSqlCleanAppsOutput.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlCleanAppsOutput.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlCleanAppsOutput.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlCleanAppsOutput.Name = "grpSqlCleanAppsOutput"
        Me.grpSqlCleanAppsOutput.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlCleanAppsOutput.Size = New System.Drawing.Size(1023, 439)
        Me.grpSqlCleanAppsOutput.TabIndex = 0
        Me.grpSqlCleanAppsOutput.TabStop = False
        Me.grpSqlCleanAppsOutput.Text = "Software Delivery Database Applications Cleanup (CleanApps)"
        '
        'txtSqlCleanApps
        '
        Me.txtSqlCleanApps.BackColor = System.Drawing.Color.Beige
        Me.txtSqlCleanApps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSqlCleanApps.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSqlCleanApps.Location = New System.Drawing.Point(4, 27)
        Me.txtSqlCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSqlCleanApps.Multiline = True
        Me.txtSqlCleanApps.Name = "txtSqlCleanApps"
        Me.txtSqlCleanApps.ReadOnly = True
        Me.txtSqlCleanApps.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtSqlCleanApps.Size = New System.Drawing.Size(1015, 408)
        Me.txtSqlCleanApps.TabIndex = 2
        Me.txtSqlCleanApps.WordWrap = False
        '
        'pnlSqlCleanAppsButtons
        '
        Me.pnlSqlCleanAppsButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlCleanAppsButtons.Controls.Add(Me.btnSqlConnectCleanApps)
        Me.pnlSqlCleanAppsButtons.Controls.Add(Me.btnSqlDisconnectCleanApps)
        Me.pnlSqlCleanAppsButtons.Controls.Add(Me.btnSqlCafStopCleanApps)
        Me.pnlSqlCleanAppsButtons.Controls.Add(Me.btnSqlRunCleanApps)
        Me.pnlSqlCleanAppsButtons.Controls.Add(Me.btnSqlExitCleanApps)
        Me.pnlSqlCleanAppsButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlCleanAppsButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlCleanAppsButtons.Name = "pnlSqlCleanAppsButtons"
        Me.pnlSqlCleanAppsButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlCleanAppsButtons.TabIndex = 68
        '
        'btnSqlConnectCleanApps
        '
        Me.btnSqlConnectCleanApps.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectCleanApps.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectCleanApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectCleanApps.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectCleanApps.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectCleanApps.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectCleanApps.Name = "btnSqlConnectCleanApps"
        Me.btnSqlConnectCleanApps.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectCleanApps.TabIndex = 77
        Me.btnSqlConnectCleanApps.Text = "&Connect Sql"
        Me.btnSqlConnectCleanApps.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectCleanApps
        '
        Me.btnSqlDisconnectCleanApps.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectCleanApps.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectCleanApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectCleanApps.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectCleanApps.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectCleanApps.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectCleanApps.Name = "btnSqlDisconnectCleanApps"
        Me.btnSqlDisconnectCleanApps.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectCleanApps.TabIndex = 78
        Me.btnSqlDisconnectCleanApps.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectCleanApps.UseVisualStyleBackColor = False
        '
        'btnSqlCafStopCleanApps
        '
        Me.btnSqlCafStopCleanApps.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlCafStopCleanApps.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlCafStopCleanApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlCafStopCleanApps.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlCafStopCleanApps.ForeColor = System.Drawing.Color.White
        Me.btnSqlCafStopCleanApps.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlCafStopCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlCafStopCleanApps.Name = "btnSqlCafStopCleanApps"
        Me.btnSqlCafStopCleanApps.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlCafStopCleanApps.TabIndex = 79
        Me.btnSqlCafStopCleanApps.Text = "&Stop CAF"
        Me.btnSqlCafStopCleanApps.UseVisualStyleBackColor = False
        '
        'btnSqlRunCleanApps
        '
        Me.btnSqlRunCleanApps.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlRunCleanApps.BackColor = System.Drawing.Color.Tan
        Me.btnSqlRunCleanApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlRunCleanApps.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlRunCleanApps.ForeColor = System.Drawing.Color.White
        Me.btnSqlRunCleanApps.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlRunCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlRunCleanApps.Name = "btnSqlRunCleanApps"
        Me.btnSqlRunCleanApps.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlRunCleanApps.TabIndex = 80
        Me.btnSqlRunCleanApps.Text = "&Run CleanApps"
        Me.btnSqlRunCleanApps.UseVisualStyleBackColor = False
        '
        'btnSqlExitCleanApps
        '
        Me.btnSqlExitCleanApps.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitCleanApps.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitCleanApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitCleanApps.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitCleanApps.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitCleanApps.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitCleanApps.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitCleanApps.Name = "btnSqlExitCleanApps"
        Me.btnSqlExitCleanApps.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitCleanApps.TabIndex = 81
        Me.btnSqlExitCleanApps.Text = "E&xit"
        Me.btnSqlExitCleanApps.UseVisualStyleBackColor = False
        '
        'pnlSqlQueryEditor
        '
        Me.pnlSqlQueryEditor.AutoScroll = True
        Me.pnlSqlQueryEditor.Controls.Add(Me.grpSqlQuery)
        Me.pnlSqlQueryEditor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSqlQueryEditor.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSqlQueryEditor.Location = New System.Drawing.Point(0, 0)
        Me.pnlSqlQueryEditor.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlQueryEditor.Name = "pnlSqlQueryEditor"
        Me.pnlSqlQueryEditor.Size = New System.Drawing.Size(1023, 698)
        Me.pnlSqlQueryEditor.TabIndex = 28
        '
        'grpSqlQuery
        '
        Me.grpSqlQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSqlQuery.Controls.Add(Me.rtbSqlQuery)
        Me.grpSqlQuery.Controls.Add(Me.TabControlSql)
        Me.grpSqlQuery.Controls.Add(Me.pnlSqlQueryEditorButtons)
        Me.grpSqlQuery.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSqlQuery.Location = New System.Drawing.Point(0, 0)
        Me.grpSqlQuery.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSqlQuery.Name = "grpSqlQuery"
        Me.grpSqlQuery.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSqlQuery.Size = New System.Drawing.Size(1023, 698)
        Me.grpSqlQuery.TabIndex = 27
        Me.grpSqlQuery.TabStop = False
        Me.grpSqlQuery.Text = "Query Editor"
        '
        'rtbSqlQuery
        '
        Me.rtbSqlQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbSqlQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtbSqlQuery.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbSqlQuery.Location = New System.Drawing.Point(4, 26)
        Me.rtbSqlQuery.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbSqlQuery.Name = "rtbSqlQuery"
        Me.rtbSqlQuery.Size = New System.Drawing.Size(1012, 213)
        Me.rtbSqlQuery.TabIndex = 1
        Me.rtbSqlQuery.Text = ""
        Me.rtbSqlQuery.WordWrap = False
        '
        'TabControlSql
        '
        Me.TabControlSql.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlSql.Controls.Add(Me.TabPageMessages)
        Me.TabControlSql.Controls.Add(Me.TabPageGrid)
        Me.TabControlSql.Location = New System.Drawing.Point(4, 248)
        Me.TabControlSql.Margin = New System.Windows.Forms.Padding(4)
        Me.TabControlSql.Name = "TabControlSql"
        Me.TabControlSql.SelectedIndex = 0
        Me.TabControlSql.Size = New System.Drawing.Size(1014, 386)
        Me.TabControlSql.TabIndex = 37
        '
        'TabPageMessages
        '
        Me.TabPageMessages.Controls.Add(Me.txtSqlMessage)
        Me.TabPageMessages.Location = New System.Drawing.Point(4, 32)
        Me.TabPageMessages.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPageMessages.Name = "TabPageMessages"
        Me.TabPageMessages.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPageMessages.Size = New System.Drawing.Size(1006, 350)
        Me.TabPageMessages.TabIndex = 0
        Me.TabPageMessages.Text = "Messages"
        Me.TabPageMessages.UseVisualStyleBackColor = True
        '
        'txtSqlMessage
        '
        Me.txtSqlMessage.BackColor = System.Drawing.Color.Beige
        Me.txtSqlMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSqlMessage.Location = New System.Drawing.Point(4, 4)
        Me.txtSqlMessage.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSqlMessage.Multiline = True
        Me.txtSqlMessage.Name = "txtSqlMessage"
        Me.txtSqlMessage.ReadOnly = True
        Me.txtSqlMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtSqlMessage.Size = New System.Drawing.Size(998, 342)
        Me.txtSqlMessage.TabIndex = 0
        Me.txtSqlMessage.WordWrap = False
        '
        'TabPageGrid
        '
        Me.TabPageGrid.AutoScroll = True
        Me.TabPageGrid.Location = New System.Drawing.Point(4, 32)
        Me.TabPageGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPageGrid.Name = "TabPageGrid"
        Me.TabPageGrid.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPageGrid.Size = New System.Drawing.Size(1006, 350)
        Me.TabPageGrid.TabIndex = 1
        Me.TabPageGrid.Text = "Results"
        Me.TabPageGrid.UseVisualStyleBackColor = True
        '
        'pnlSqlQueryEditorButtons
        '
        Me.pnlSqlQueryEditorButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSqlQueryEditorButtons.Controls.Add(Me.btnSqlConnectQueryEditor)
        Me.pnlSqlQueryEditorButtons.Controls.Add(Me.btnSqlDisconnectQueryEditor)
        Me.pnlSqlQueryEditorButtons.Controls.Add(Me.btnSqlSubmitQueryEditor)
        Me.pnlSqlQueryEditorButtons.Controls.Add(Me.btnSqlCancelQueryEditor)
        Me.pnlSqlQueryEditorButtons.Controls.Add(Me.btnSqlExitQueryEditor)
        Me.pnlSqlQueryEditorButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlSqlQueryEditorButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSqlQueryEditorButtons.Name = "pnlSqlQueryEditorButtons"
        Me.pnlSqlQueryEditorButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlSqlQueryEditorButtons.TabIndex = 68
        '
        'btnSqlConnectQueryEditor
        '
        Me.btnSqlConnectQueryEditor.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlConnectQueryEditor.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.btnSqlConnectQueryEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlConnectQueryEditor.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlConnectQueryEditor.ForeColor = System.Drawing.Color.White
        Me.btnSqlConnectQueryEditor.Location = New System.Drawing.Point(11, 11)
        Me.btnSqlConnectQueryEditor.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlConnectQueryEditor.Name = "btnSqlConnectQueryEditor"
        Me.btnSqlConnectQueryEditor.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlConnectQueryEditor.TabIndex = 77
        Me.btnSqlConnectQueryEditor.Text = "&Connect Sql"
        Me.btnSqlConnectQueryEditor.UseVisualStyleBackColor = False
        '
        'btnSqlDisconnectQueryEditor
        '
        Me.btnSqlDisconnectQueryEditor.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlDisconnectQueryEditor.BackColor = System.Drawing.Color.IndianRed
        Me.btnSqlDisconnectQueryEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlDisconnectQueryEditor.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlDisconnectQueryEditor.ForeColor = System.Drawing.Color.White
        Me.btnSqlDisconnectQueryEditor.Location = New System.Drawing.Point(221, 11)
        Me.btnSqlDisconnectQueryEditor.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlDisconnectQueryEditor.Name = "btnSqlDisconnectQueryEditor"
        Me.btnSqlDisconnectQueryEditor.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlDisconnectQueryEditor.TabIndex = 78
        Me.btnSqlDisconnectQueryEditor.Text = "&Disconnect Sql"
        Me.btnSqlDisconnectQueryEditor.UseVisualStyleBackColor = False
        '
        'btnSqlSubmitQueryEditor
        '
        Me.btnSqlSubmitQueryEditor.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlSubmitQueryEditor.BackColor = System.Drawing.Color.Khaki
        Me.btnSqlSubmitQueryEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlSubmitQueryEditor.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlSubmitQueryEditor.ForeColor = System.Drawing.Color.White
        Me.btnSqlSubmitQueryEditor.Location = New System.Drawing.Point(431, 11)
        Me.btnSqlSubmitQueryEditor.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlSubmitQueryEditor.Name = "btnSqlSubmitQueryEditor"
        Me.btnSqlSubmitQueryEditor.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlSubmitQueryEditor.TabIndex = 79
        Me.btnSqlSubmitQueryEditor.Text = "&Submit (F5)"
        Me.btnSqlSubmitQueryEditor.UseVisualStyleBackColor = False
        '
        'btnSqlCancelQueryEditor
        '
        Me.btnSqlCancelQueryEditor.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlCancelQueryEditor.BackColor = System.Drawing.Color.Tan
        Me.btnSqlCancelQueryEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlCancelQueryEditor.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlCancelQueryEditor.ForeColor = System.Drawing.Color.White
        Me.btnSqlCancelQueryEditor.Location = New System.Drawing.Point(641, 11)
        Me.btnSqlCancelQueryEditor.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlCancelQueryEditor.Name = "btnSqlCancelQueryEditor"
        Me.btnSqlCancelQueryEditor.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlCancelQueryEditor.TabIndex = 80
        Me.btnSqlCancelQueryEditor.Text = "C&ancel"
        Me.btnSqlCancelQueryEditor.UseVisualStyleBackColor = False
        '
        'btnSqlExitQueryEditor
        '
        Me.btnSqlExitQueryEditor.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSqlExitQueryEditor.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSqlExitQueryEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSqlExitQueryEditor.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSqlExitQueryEditor.ForeColor = System.Drawing.Color.White
        Me.btnSqlExitQueryEditor.Location = New System.Drawing.Point(851, 11)
        Me.btnSqlExitQueryEditor.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSqlExitQueryEditor.Name = "btnSqlExitQueryEditor"
        Me.btnSqlExitQueryEditor.Size = New System.Drawing.Size(158, 38)
        Me.btnSqlExitQueryEditor.TabIndex = 81
        Me.btnSqlExitQueryEditor.Text = "E&xit"
        Me.btnSqlExitQueryEditor.UseVisualStyleBackColor = False
        '
        'pnlENCOverdrive
        '
        Me.pnlENCOverdrive.Controls.Add(Me.grpEncStressStatus)
        Me.pnlENCOverdrive.Controls.Add(Me.grpEncStressTest)
        Me.pnlENCOverdrive.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlENCOverdrive.Location = New System.Drawing.Point(0, 0)
        Me.pnlENCOverdrive.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlENCOverdrive.Name = "pnlENCOverdrive"
        Me.pnlENCOverdrive.Size = New System.Drawing.Size(1023, 698)
        Me.pnlENCOverdrive.TabIndex = 28
        '
        'grpEncStressStatus
        '
        Me.grpEncStressStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncStressStatus.Controls.Add(Me.dgvEncStressTable)
        Me.grpEncStressStatus.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpEncStressStatus.Location = New System.Drawing.Point(1, 419)
        Me.grpEncStressStatus.Margin = New System.Windows.Forms.Padding(4)
        Me.grpEncStressStatus.Name = "grpEncStressStatus"
        Me.grpEncStressStatus.Padding = New System.Windows.Forms.Padding(4)
        Me.grpEncStressStatus.Size = New System.Drawing.Size(1021, 278)
        Me.grpEncStressStatus.TabIndex = 1
        Me.grpEncStressStatus.TabStop = False
        Me.grpEncStressStatus.Text = "Status Table"
        '
        'dgvEncStressTable
        '
        Me.dgvEncStressTable.AllowUserToAddRows = False
        Me.dgvEncStressTable.AllowUserToDeleteRows = False
        Me.dgvEncStressTable.AllowUserToResizeRows = False
        Me.dgvEncStressTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvEncStressTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvEncStressTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvEncStressTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEncStressTable.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgvEncStressTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.Beige
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEncStressTable.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgvEncStressTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEncStressTable.Location = New System.Drawing.Point(4, 27)
        Me.dgvEncStressTable.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvEncStressTable.Name = "dgvEncStressTable"
        Me.dgvEncStressTable.ReadOnly = True
        Me.dgvEncStressTable.RowHeadersVisible = False
        Me.dgvEncStressTable.ShowCellErrors = False
        Me.dgvEncStressTable.ShowCellToolTips = False
        Me.dgvEncStressTable.ShowEditingIcon = False
        Me.dgvEncStressTable.ShowRowErrors = False
        Me.dgvEncStressTable.Size = New System.Drawing.Size(1013, 247)
        Me.dgvEncStressTable.TabIndex = 0
        '
        'grpEncStressTest
        '
        Me.grpEncStressTest.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncStressTest.Controls.Add(Me.lblEncStressIntro)
        Me.grpEncStressTest.Controls.Add(Me.grpEncStressPingType)
        Me.grpEncStressTest.Controls.Add(Me.btnEncStressStop)
        Me.grpEncStressTest.Controls.Add(Me.btnEncStressStart)
        Me.grpEncStressTest.Controls.Add(Me.grpEncStressPingSettings)
        Me.grpEncStressTest.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpEncStressTest.Location = New System.Drawing.Point(0, 0)
        Me.grpEncStressTest.Margin = New System.Windows.Forms.Padding(4)
        Me.grpEncStressTest.Name = "grpEncStressTest"
        Me.grpEncStressTest.Padding = New System.Windows.Forms.Padding(4)
        Me.grpEncStressTest.Size = New System.Drawing.Size(1023, 416)
        Me.grpEncStressTest.TabIndex = 0
        Me.grpEncStressTest.TabStop = False
        Me.grpEncStressTest.Text = "ENC Ping Utility"
        '
        'lblEncStressIntro
        '
        Me.lblEncStressIntro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEncStressIntro.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressIntro.Location = New System.Drawing.Point(20, 24)
        Me.lblEncStressIntro.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressIntro.Name = "lblEncStressIntro"
        Me.lblEncStressIntro.Size = New System.Drawing.Size(994, 99)
        Me.lblEncStressIntro.TabIndex = 37
        Me.lblEncStressIntro.Text = resources.GetString("lblEncStressIntro.Text")
        '
        'grpEncStressPingType
        '
        Me.grpEncStressPingType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncStressPingType.Controls.Add(Me.rbnEncStressCafPing)
        Me.grpEncStressPingType.Controls.Add(Me.rbnEncStressCamPing)
        Me.grpEncStressPingType.Location = New System.Drawing.Point(777, 124)
        Me.grpEncStressPingType.Margin = New System.Windows.Forms.Padding(4)
        Me.grpEncStressPingType.Name = "grpEncStressPingType"
        Me.grpEncStressPingType.Padding = New System.Windows.Forms.Padding(4)
        Me.grpEncStressPingType.Size = New System.Drawing.Size(240, 238)
        Me.grpEncStressPingType.TabIndex = 36
        Me.grpEncStressPingType.TabStop = False
        Me.grpEncStressPingType.Text = "Ping Type"
        '
        'rbnEncStressCafPing
        '
        Me.rbnEncStressCafPing.AutoSize = True
        Me.rbnEncStressCafPing.Location = New System.Drawing.Point(71, 138)
        Me.rbnEncStressCafPing.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnEncStressCafPing.Name = "rbnEncStressCafPing"
        Me.rbnEncStressCafPing.Size = New System.Drawing.Size(91, 27)
        Me.rbnEncStressCafPing.TabIndex = 1
        Me.rbnEncStressCafPing.Text = "caf ping"
        Me.rbnEncStressCafPing.UseVisualStyleBackColor = True
        '
        'rbnEncStressCamPing
        '
        Me.rbnEncStressCamPing.AutoSize = True
        Me.rbnEncStressCamPing.Checked = True
        Me.rbnEncStressCamPing.Location = New System.Drawing.Point(71, 74)
        Me.rbnEncStressCamPing.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnEncStressCamPing.Name = "rbnEncStressCamPing"
        Me.rbnEncStressCamPing.Size = New System.Drawing.Size(96, 27)
        Me.rbnEncStressCamPing.TabIndex = 0
        Me.rbnEncStressCamPing.TabStop = True
        Me.rbnEncStressCamPing.Text = "camping"
        Me.rbnEncStressCamPing.UseVisualStyleBackColor = True
        '
        'btnEncStressStop
        '
        Me.btnEncStressStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressStop.BackColor = System.Drawing.Color.SteelBlue
        Me.btnEncStressStop.FlatAppearance.BorderSize = 0
        Me.btnEncStressStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEncStressStop.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressStop.ForeColor = System.Drawing.Color.White
        Me.btnEncStressStop.Location = New System.Drawing.Point(675, 371)
        Me.btnEncStressStop.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressStop.Name = "btnEncStressStop"
        Me.btnEncStressStop.Size = New System.Drawing.Size(158, 35)
        Me.btnEncStressStop.TabIndex = 10
        Me.btnEncStressStop.Text = "Stop"
        Me.btnEncStressStop.UseVisualStyleBackColor = False
        '
        'btnEncStressStart
        '
        Me.btnEncStressStart.BackColor = System.Drawing.Color.SteelBlue
        Me.btnEncStressStart.FlatAppearance.BorderSize = 0
        Me.btnEncStressStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEncStressStart.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressStart.ForeColor = System.Drawing.Color.White
        Me.btnEncStressStart.Location = New System.Drawing.Point(190, 371)
        Me.btnEncStressStart.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressStart.Name = "btnEncStressStart"
        Me.btnEncStressStart.Size = New System.Drawing.Size(158, 35)
        Me.btnEncStressStart.TabIndex = 9
        Me.btnEncStressStart.Text = "Start"
        Me.btnEncStressStart.UseVisualStyleBackColor = False
        '
        'grpEncStressPingSettings
        '
        Me.grpEncStressPingSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressDelayTimePlus)
        Me.grpEncStressPingSettings.Controls.Add(Me.txtEncStressDelayTime)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressDelayTimeMinus)
        Me.grpEncStressPingSettings.Controls.Add(Me.lblEncStressDelayTime)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressReplyTimeoutPlus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressReplyTimeoutMinus)
        Me.grpEncStressPingSettings.Controls.Add(Me.txtEncStressReplyTimeout)
        Me.grpEncStressPingSettings.Controls.Add(Me.lblEncStressReplyTimeout)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressMsgSizePlus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressMsgSizeMinus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressNumPingPlus)
        Me.grpEncStressPingSettings.Controls.Add(Me.lblEncStressNumPings)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressNumPingMinus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressPingThreadPlus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressPingThreadMinus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressTargetThreadPlus)
        Me.grpEncStressPingSettings.Controls.Add(Me.btnEncStressTargetThreadMinus)
        Me.grpEncStressPingSettings.Controls.Add(Me.txtEncStressMsgSize)
        Me.grpEncStressPingSettings.Controls.Add(Me.txtEncStressNumPings)
        Me.grpEncStressPingSettings.Controls.Add(Me.txtEncStressNumTargetThreads)
        Me.grpEncStressPingSettings.Controls.Add(Me.txtEncStressNumPingThreads)
        Me.grpEncStressPingSettings.Controls.Add(Me.lblEncStressMsgSize)
        Me.grpEncStressPingSettings.Controls.Add(Me.lblEncStressPingThreads)
        Me.grpEncStressPingSettings.Controls.Add(Me.lblEncStressTargetThreads)
        Me.grpEncStressPingSettings.Location = New System.Drawing.Point(20, 124)
        Me.grpEncStressPingSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.grpEncStressPingSettings.Name = "grpEncStressPingSettings"
        Me.grpEncStressPingSettings.Padding = New System.Windows.Forms.Padding(4)
        Me.grpEncStressPingSettings.Size = New System.Drawing.Size(761, 238)
        Me.grpEncStressPingSettings.TabIndex = 8
        Me.grpEncStressPingSettings.TabStop = False
        Me.grpEncStressPingSettings.Text = "Ping Settings"
        '
        'btnEncStressDelayTimePlus
        '
        Me.btnEncStressDelayTimePlus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressDelayTimePlus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressDelayTimePlus.Location = New System.Drawing.Point(708, 199)
        Me.btnEncStressDelayTimePlus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressDelayTimePlus.Name = "btnEncStressDelayTimePlus"
        Me.btnEncStressDelayTimePlus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressDelayTimePlus.TabIndex = 35
        Me.btnEncStressDelayTimePlus.Text = "+"
        Me.btnEncStressDelayTimePlus.UseVisualStyleBackColor = True
        '
        'txtEncStressDelayTime
        '
        Me.txtEncStressDelayTime.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEncStressDelayTime.BackColor = System.Drawing.Color.White
        Me.txtEncStressDelayTime.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEncStressDelayTime.Location = New System.Drawing.Point(312, 200)
        Me.txtEncStressDelayTime.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEncStressDelayTime.Name = "txtEncStressDelayTime"
        Me.txtEncStressDelayTime.ReadOnly = True
        Me.txtEncStressDelayTime.Size = New System.Drawing.Size(342, 27)
        Me.txtEncStressDelayTime.TabIndex = 33
        Me.txtEncStressDelayTime.Text = "0"
        '
        'btnEncStressDelayTimeMinus
        '
        Me.btnEncStressDelayTimeMinus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressDelayTimeMinus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressDelayTimeMinus.Location = New System.Drawing.Point(663, 199)
        Me.btnEncStressDelayTimeMinus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressDelayTimeMinus.Name = "btnEncStressDelayTimeMinus"
        Me.btnEncStressDelayTimeMinus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressDelayTimeMinus.TabIndex = 34
        Me.btnEncStressDelayTimeMinus.Text = "-"
        Me.btnEncStressDelayTimeMinus.UseVisualStyleBackColor = True
        '
        'lblEncStressDelayTime
        '
        Me.lblEncStressDelayTime.AutoSize = True
        Me.lblEncStressDelayTime.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressDelayTime.Location = New System.Drawing.Point(32, 204)
        Me.lblEncStressDelayTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressDelayTime.Name = "lblEncStressDelayTime"
        Me.lblEncStressDelayTime.Size = New System.Drawing.Size(280, 21)
        Me.lblEncStressDelayTime.TabIndex = 32
        Me.lblEncStressDelayTime.Text = "Delay time between pings (in seconds):"
        '
        'btnEncStressReplyTimeoutPlus
        '
        Me.btnEncStressReplyTimeoutPlus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressReplyTimeoutPlus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressReplyTimeoutPlus.Location = New System.Drawing.Point(708, 164)
        Me.btnEncStressReplyTimeoutPlus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressReplyTimeoutPlus.Name = "btnEncStressReplyTimeoutPlus"
        Me.btnEncStressReplyTimeoutPlus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressReplyTimeoutPlus.TabIndex = 31
        Me.btnEncStressReplyTimeoutPlus.Text = "+"
        Me.btnEncStressReplyTimeoutPlus.UseVisualStyleBackColor = True
        '
        'btnEncStressReplyTimeoutMinus
        '
        Me.btnEncStressReplyTimeoutMinus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressReplyTimeoutMinus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressReplyTimeoutMinus.Location = New System.Drawing.Point(663, 164)
        Me.btnEncStressReplyTimeoutMinus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressReplyTimeoutMinus.Name = "btnEncStressReplyTimeoutMinus"
        Me.btnEncStressReplyTimeoutMinus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressReplyTimeoutMinus.TabIndex = 30
        Me.btnEncStressReplyTimeoutMinus.Text = "-"
        Me.btnEncStressReplyTimeoutMinus.UseVisualStyleBackColor = True
        '
        'txtEncStressReplyTimeout
        '
        Me.txtEncStressReplyTimeout.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEncStressReplyTimeout.BackColor = System.Drawing.Color.White
        Me.txtEncStressReplyTimeout.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEncStressReplyTimeout.Location = New System.Drawing.Point(312, 165)
        Me.txtEncStressReplyTimeout.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEncStressReplyTimeout.Name = "txtEncStressReplyTimeout"
        Me.txtEncStressReplyTimeout.ReadOnly = True
        Me.txtEncStressReplyTimeout.Size = New System.Drawing.Size(342, 27)
        Me.txtEncStressReplyTimeout.TabIndex = 29
        Me.txtEncStressReplyTimeout.Text = "16"
        '
        'lblEncStressReplyTimeout
        '
        Me.lblEncStressReplyTimeout.AutoSize = True
        Me.lblEncStressReplyTimeout.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressReplyTimeout.Location = New System.Drawing.Point(54, 170)
        Me.lblEncStressReplyTimeout.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressReplyTimeout.Name = "lblEncStressReplyTimeout"
        Me.lblEncStressReplyTimeout.Size = New System.Drawing.Size(256, 21)
        Me.lblEncStressReplyTimeout.TabIndex = 28
        Me.lblEncStressReplyTimeout.Text = "Wait for reply timeout (in seconds):"
        '
        'btnEncStressMsgSizePlus
        '
        Me.btnEncStressMsgSizePlus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressMsgSizePlus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressMsgSizePlus.Location = New System.Drawing.Point(708, 130)
        Me.btnEncStressMsgSizePlus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressMsgSizePlus.Name = "btnEncStressMsgSizePlus"
        Me.btnEncStressMsgSizePlus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressMsgSizePlus.TabIndex = 27
        Me.btnEncStressMsgSizePlus.Text = "+"
        Me.btnEncStressMsgSizePlus.UseVisualStyleBackColor = True
        '
        'btnEncStressMsgSizeMinus
        '
        Me.btnEncStressMsgSizeMinus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressMsgSizeMinus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressMsgSizeMinus.Location = New System.Drawing.Point(663, 130)
        Me.btnEncStressMsgSizeMinus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressMsgSizeMinus.Name = "btnEncStressMsgSizeMinus"
        Me.btnEncStressMsgSizeMinus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressMsgSizeMinus.TabIndex = 26
        Me.btnEncStressMsgSizeMinus.Text = "-"
        Me.btnEncStressMsgSizeMinus.UseVisualStyleBackColor = True
        '
        'btnEncStressNumPingPlus
        '
        Me.btnEncStressNumPingPlus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressNumPingPlus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressNumPingPlus.Location = New System.Drawing.Point(708, 95)
        Me.btnEncStressNumPingPlus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressNumPingPlus.Name = "btnEncStressNumPingPlus"
        Me.btnEncStressNumPingPlus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressNumPingPlus.TabIndex = 25
        Me.btnEncStressNumPingPlus.Text = "+"
        Me.btnEncStressNumPingPlus.UseVisualStyleBackColor = True
        '
        'lblEncStressNumPings
        '
        Me.lblEncStressNumPings.AutoSize = True
        Me.lblEncStressNumPings.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressNumPings.Location = New System.Drawing.Point(68, 99)
        Me.lblEncStressNumPings.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressNumPings.Name = "lblEncStressNumPings"
        Me.lblEncStressNumPings.Size = New System.Drawing.Size(246, 21)
        Me.lblEncStressNumPings.TabIndex = 14
        Me.lblEncStressNumPings.Text = "Number of pings, per ping thread:"
        '
        'btnEncStressNumPingMinus
        '
        Me.btnEncStressNumPingMinus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressNumPingMinus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressNumPingMinus.Location = New System.Drawing.Point(663, 95)
        Me.btnEncStressNumPingMinus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressNumPingMinus.Name = "btnEncStressNumPingMinus"
        Me.btnEncStressNumPingMinus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressNumPingMinus.TabIndex = 24
        Me.btnEncStressNumPingMinus.Text = "-"
        Me.btnEncStressNumPingMinus.UseVisualStyleBackColor = True
        '
        'btnEncStressPingThreadPlus
        '
        Me.btnEncStressPingThreadPlus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressPingThreadPlus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressPingThreadPlus.Location = New System.Drawing.Point(708, 59)
        Me.btnEncStressPingThreadPlus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressPingThreadPlus.Name = "btnEncStressPingThreadPlus"
        Me.btnEncStressPingThreadPlus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressPingThreadPlus.TabIndex = 23
        Me.btnEncStressPingThreadPlus.Text = "+"
        Me.btnEncStressPingThreadPlus.UseVisualStyleBackColor = True
        '
        'btnEncStressPingThreadMinus
        '
        Me.btnEncStressPingThreadMinus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressPingThreadMinus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressPingThreadMinus.Location = New System.Drawing.Point(663, 59)
        Me.btnEncStressPingThreadMinus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressPingThreadMinus.Name = "btnEncStressPingThreadMinus"
        Me.btnEncStressPingThreadMinus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressPingThreadMinus.TabIndex = 22
        Me.btnEncStressPingThreadMinus.Text = "-"
        Me.btnEncStressPingThreadMinus.UseVisualStyleBackColor = True
        '
        'btnEncStressTargetThreadPlus
        '
        Me.btnEncStressTargetThreadPlus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressTargetThreadPlus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressTargetThreadPlus.Location = New System.Drawing.Point(708, 22)
        Me.btnEncStressTargetThreadPlus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressTargetThreadPlus.Name = "btnEncStressTargetThreadPlus"
        Me.btnEncStressTargetThreadPlus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressTargetThreadPlus.TabIndex = 21
        Me.btnEncStressTargetThreadPlus.Text = "+"
        Me.btnEncStressTargetThreadPlus.UseVisualStyleBackColor = True
        '
        'btnEncStressTargetThreadMinus
        '
        Me.btnEncStressTargetThreadMinus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEncStressTargetThreadMinus.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEncStressTargetThreadMinus.Location = New System.Drawing.Point(663, 22)
        Me.btnEncStressTargetThreadMinus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEncStressTargetThreadMinus.Name = "btnEncStressTargetThreadMinus"
        Me.btnEncStressTargetThreadMinus.Size = New System.Drawing.Size(38, 29)
        Me.btnEncStressTargetThreadMinus.TabIndex = 20
        Me.btnEncStressTargetThreadMinus.Text = "-"
        Me.btnEncStressTargetThreadMinus.UseVisualStyleBackColor = True
        '
        'txtEncStressMsgSize
        '
        Me.txtEncStressMsgSize.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEncStressMsgSize.BackColor = System.Drawing.Color.White
        Me.txtEncStressMsgSize.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEncStressMsgSize.Location = New System.Drawing.Point(312, 131)
        Me.txtEncStressMsgSize.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEncStressMsgSize.Name = "txtEncStressMsgSize"
        Me.txtEncStressMsgSize.ReadOnly = True
        Me.txtEncStressMsgSize.Size = New System.Drawing.Size(342, 27)
        Me.txtEncStressMsgSize.TabIndex = 19
        Me.txtEncStressMsgSize.Text = "64"
        '
        'txtEncStressNumPings
        '
        Me.txtEncStressNumPings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEncStressNumPings.BackColor = System.Drawing.Color.White
        Me.txtEncStressNumPings.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEncStressNumPings.Location = New System.Drawing.Point(312, 95)
        Me.txtEncStressNumPings.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEncStressNumPings.Name = "txtEncStressNumPings"
        Me.txtEncStressNumPings.ReadOnly = True
        Me.txtEncStressNumPings.Size = New System.Drawing.Size(342, 27)
        Me.txtEncStressNumPings.TabIndex = 18
        Me.txtEncStressNumPings.Text = "4"
        '
        'txtEncStressNumTargetThreads
        '
        Me.txtEncStressNumTargetThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEncStressNumTargetThreads.BackColor = System.Drawing.Color.White
        Me.txtEncStressNumTargetThreads.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEncStressNumTargetThreads.Location = New System.Drawing.Point(312, 22)
        Me.txtEncStressNumTargetThreads.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEncStressNumTargetThreads.Name = "txtEncStressNumTargetThreads"
        Me.txtEncStressNumTargetThreads.ReadOnly = True
        Me.txtEncStressNumTargetThreads.Size = New System.Drawing.Size(342, 27)
        Me.txtEncStressNumTargetThreads.TabIndex = 17
        Me.txtEncStressNumTargetThreads.Text = "1"
        '
        'txtEncStressNumPingThreads
        '
        Me.txtEncStressNumPingThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEncStressNumPingThreads.BackColor = System.Drawing.Color.White
        Me.txtEncStressNumPingThreads.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEncStressNumPingThreads.Location = New System.Drawing.Point(312, 59)
        Me.txtEncStressNumPingThreads.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEncStressNumPingThreads.Name = "txtEncStressNumPingThreads"
        Me.txtEncStressNumPingThreads.ReadOnly = True
        Me.txtEncStressNumPingThreads.Size = New System.Drawing.Size(342, 27)
        Me.txtEncStressNumPingThreads.TabIndex = 16
        Me.txtEncStressNumPingThreads.Text = "1"
        '
        'lblEncStressMsgSize
        '
        Me.lblEncStressMsgSize.AutoSize = True
        Me.lblEncStressMsgSize.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressMsgSize.Location = New System.Drawing.Point(75, 135)
        Me.lblEncStressMsgSize.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressMsgSize.Name = "lblEncStressMsgSize"
        Me.lblEncStressMsgSize.Size = New System.Drawing.Size(236, 21)
        Me.lblEncStressMsgSize.TabIndex = 13
        Me.lblEncStressMsgSize.Text = "Message size (in bytes) per ping:"
        '
        'lblEncStressPingThreads
        '
        Me.lblEncStressPingThreads.AutoSize = True
        Me.lblEncStressPingThreads.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressPingThreads.Location = New System.Drawing.Point(9, 62)
        Me.lblEncStressPingThreads.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressPingThreads.Name = "lblEncStressPingThreads"
        Me.lblEncStressPingThreads.Size = New System.Drawing.Size(308, 21)
        Me.lblEncStressPingThreads.TabIndex = 12
        Me.lblEncStressPingThreads.Text = "Number of ping threads, per target thread:"
        '
        'lblEncStressTargetThreads
        '
        Me.lblEncStressTargetThreads.AutoSize = True
        Me.lblEncStressTargetThreads.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEncStressTargetThreads.Location = New System.Drawing.Point(24, 30)
        Me.lblEncStressTargetThreads.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEncStressTargetThreads.Name = "lblEncStressTargetThreads"
        Me.lblEncStressTargetThreads.Size = New System.Drawing.Size(291, 21)
        Me.lblEncStressTargetThreads.TabIndex = 11
        Me.lblEncStressTargetThreads.Text = "Number of simultaneous target threads:"
        '
        'pnlRemovalTool
        '
        Me.pnlRemovalTool.Controls.Add(Me.grpRemovalTool)
        Me.pnlRemovalTool.Controls.Add(Me.grpRemovalOptions)
        Me.pnlRemovalTool.Controls.Add(Me.pnlRemovalToolButtons)
        Me.pnlRemovalTool.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlRemovalTool.Location = New System.Drawing.Point(0, 0)
        Me.pnlRemovalTool.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlRemovalTool.Name = "pnlRemovalTool"
        Me.pnlRemovalTool.Size = New System.Drawing.Size(1023, 698)
        Me.pnlRemovalTool.TabIndex = 45
        '
        'grpRemovalTool
        '
        Me.grpRemovalTool.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRemovalTool.Controls.Add(Me.txtRemovalTool)
        Me.grpRemovalTool.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRemovalTool.Location = New System.Drawing.Point(0, 0)
        Me.grpRemovalTool.Margin = New System.Windows.Forms.Padding(4)
        Me.grpRemovalTool.Name = "grpRemovalTool"
        Me.grpRemovalTool.Padding = New System.Windows.Forms.Padding(4)
        Me.grpRemovalTool.Size = New System.Drawing.Size(1023, 439)
        Me.grpRemovalTool.TabIndex = 0
        Me.grpRemovalTool.TabStop = False
        Me.grpRemovalTool.Text = "Removal Tool"
        '
        'txtRemovalTool
        '
        Me.txtRemovalTool.BackColor = System.Drawing.Color.Beige
        Me.txtRemovalTool.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtRemovalTool.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemovalTool.Location = New System.Drawing.Point(4, 27)
        Me.txtRemovalTool.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRemovalTool.Multiline = True
        Me.txtRemovalTool.Name = "txtRemovalTool"
        Me.txtRemovalTool.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRemovalTool.Size = New System.Drawing.Size(1015, 408)
        Me.txtRemovalTool.TabIndex = 0
        '
        'grpRemovalOptions
        '
        Me.grpRemovalOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRemovalOptions.Controls.Add(Me.lblRemoveITCMCaution)
        Me.grpRemovalOptions.Controls.Add(Me.chkRetainHostUUID)
        Me.grpRemovalOptions.Controls.Add(Me.rbnUninstallITCM)
        Me.grpRemovalOptions.Controls.Add(Me.rbnRemoveITCM)
        Me.grpRemovalOptions.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRemovalOptions.Location = New System.Drawing.Point(0, 440)
        Me.grpRemovalOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.grpRemovalOptions.Name = "grpRemovalOptions"
        Me.grpRemovalOptions.Padding = New System.Windows.Forms.Padding(4)
        Me.grpRemovalOptions.Size = New System.Drawing.Size(1023, 191)
        Me.grpRemovalOptions.TabIndex = 1
        Me.grpRemovalOptions.TabStop = False
        Me.grpRemovalOptions.Text = "Removal Options"
        '
        'lblRemoveITCMCaution
        '
        Me.lblRemoveITCMCaution.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRemoveITCMCaution.AutoSize = True
        Me.lblRemoveITCMCaution.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemoveITCMCaution.Location = New System.Drawing.Point(253, 139)
        Me.lblRemoveITCMCaution.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRemoveITCMCaution.Name = "lblRemoveITCMCaution"
        Me.lblRemoveITCMCaution.Size = New System.Drawing.Size(524, 29)
        Me.lblRemoveITCMCaution.TabIndex = 3
        Me.lblRemoveITCMCaution.Text = "Caution: This will remove ITCM from this endpoint!"
        '
        'chkRetainHostUUID
        '
        Me.chkRetainHostUUID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkRetainHostUUID.AutoSize = True
        Me.chkRetainHostUUID.Location = New System.Drawing.Point(42, 59)
        Me.chkRetainHostUUID.Margin = New System.Windows.Forms.Padding(4)
        Me.chkRetainHostUUID.Name = "chkRetainHostUUID"
        Me.chkRetainHostUUID.Size = New System.Drawing.Size(360, 27)
        Me.chkRetainHostUUID.TabIndex = 2
        Me.chkRetainHostUUID.Text = "Retain the agent HostUUID in the registry.  "
        Me.chkRetainHostUUID.UseVisualStyleBackColor = True
        '
        'rbnUninstallITCM
        '
        Me.rbnUninstallITCM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbnUninstallITCM.AutoSize = True
        Me.rbnUninstallITCM.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbnUninstallITCM.Location = New System.Drawing.Point(19, 90)
        Me.rbnUninstallITCM.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnUninstallITCM.Name = "rbnUninstallITCM"
        Me.rbnUninstallITCM.Size = New System.Drawing.Size(644, 27)
        Me.rbnUninstallITCM.TabIndex = 1
        Me.rbnUninstallITCM.Text = "Perform normal uninstall (choose this option if other CA products are installed) " &
    " "
        Me.rbnUninstallITCM.UseVisualStyleBackColor = True
        '
        'rbnRemoveITCM
        '
        Me.rbnRemoveITCM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbnRemoveITCM.AutoSize = True
        Me.rbnRemoveITCM.Checked = True
        Me.rbnRemoveITCM.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbnRemoveITCM.Location = New System.Drawing.Point(19, 28)
        Me.rbnRemoveITCM.Margin = New System.Windows.Forms.Padding(4)
        Me.rbnRemoveITCM.Name = "rbnRemoveITCM"
        Me.rbnRemoveITCM.Size = New System.Drawing.Size(655, 27)
        Me.rbnRemoveITCM.TabIndex = 0
        Me.rbnRemoveITCM.TabStop = True
        Me.rbnRemoveITCM.Text = "Perform full removal (choose this option if ITCM is the only CA product installed" &
    ")  "
        Me.rbnRemoveITCM.UseVisualStyleBackColor = True
        '
        'pnlRemovalToolButtons
        '
        Me.pnlRemovalToolButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlRemovalToolButtons.Controls.Add(Me.btnRemoveITCM)
        Me.pnlRemovalToolButtons.Controls.Add(Me.btnExitRemoveITCM)
        Me.pnlRemovalToolButtons.Location = New System.Drawing.Point(0, 635)
        Me.pnlRemovalToolButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlRemovalToolButtons.Name = "pnlRemovalToolButtons"
        Me.pnlRemovalToolButtons.Size = New System.Drawing.Size(1023, 61)
        Me.pnlRemovalToolButtons.TabIndex = 68
        '
        'btnRemoveITCM
        '
        Me.btnRemoveITCM.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnRemoveITCM.BackColor = System.Drawing.Color.IndianRed
        Me.btnRemoveITCM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveITCM.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveITCM.ForeColor = System.Drawing.Color.White
        Me.btnRemoveITCM.Location = New System.Drawing.Point(303, 11)
        Me.btnRemoveITCM.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemoveITCM.Name = "btnRemoveITCM"
        Me.btnRemoveITCM.Size = New System.Drawing.Size(158, 38)
        Me.btnRemoveITCM.TabIndex = 78
        Me.btnRemoveITCM.Text = "&Remove ITCM"
        Me.btnRemoveITCM.UseVisualStyleBackColor = False
        '
        'btnExitRemoveITCM
        '
        Me.btnExitRemoveITCM.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnExitRemoveITCM.BackColor = System.Drawing.Color.SteelBlue
        Me.btnExitRemoveITCM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExitRemoveITCM.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExitRemoveITCM.ForeColor = System.Drawing.Color.White
        Me.btnExitRemoveITCM.Location = New System.Drawing.Point(560, 11)
        Me.btnExitRemoveITCM.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExitRemoveITCM.Name = "btnExitRemoveITCM"
        Me.btnExitRemoveITCM.Size = New System.Drawing.Size(158, 38)
        Me.btnExitRemoveITCM.TabIndex = 81
        Me.btnExitRemoveITCM.Text = "E&xit"
        Me.btnExitRemoveITCM.UseVisualStyleBackColor = False
        '
        'pnlDebug
        '
        Me.pnlDebug.Controls.Add(Me.rtbDebug)
        Me.pnlDebug.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDebug.Font = New System.Drawing.Font("Calibri Light", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDebug.Location = New System.Drawing.Point(0, 0)
        Me.pnlDebug.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlDebug.Name = "pnlDebug"
        Me.pnlDebug.Size = New System.Drawing.Size(1023, 698)
        Me.pnlDebug.TabIndex = 33
        '
        'rtbDebug
        '
        Me.rtbDebug.BackColor = System.Drawing.Color.Beige
        Me.rtbDebug.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbDebug.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbDebug.Location = New System.Drawing.Point(0, 0)
        Me.rtbDebug.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbDebug.Name = "rtbDebug"
        Me.rtbDebug.ReadOnly = True
        Me.rtbDebug.Size = New System.Drawing.Size(1023, 698)
        Me.rtbDebug.TabIndex = 0
        Me.rtbDebug.Text = ""
        '
        'WinOfflineUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1292, 702)
        Me.Controls.Add(Me.SplitWinOfflineUI)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "WinOfflineUI"
        Me.Padding = New System.Windows.Forms.Padding(0, 0, 4, 4)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WinOffline"
        Me.pnlSystemInfo.ResumeLayout(False)
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        Me.pnlWinOfflineMain.ResumeLayout(False)
        Me.grpPatchView.ResumeLayout(False)
        Me.pnlWinOfflineButton1.ResumeLayout(False)
        Me.SplitWinOfflineUI.Panel1.ResumeLayout(False)
        Me.SplitWinOfflineUI.Panel2.ResumeLayout(False)
        CType(Me.SplitWinOfflineUI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitWinOfflineUI.ResumeLayout(False)
        Me.pnlWinOfflineStart.ResumeLayout(False)
        Me.grpWinOfflineWelcome.ResumeLayout(False)
        Me.grpWinOfflineStartOptions.ResumeLayout(False)
        Me.grpWinOfflineStartOptions.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpHistory.ResumeLayout(False)
        Me.pnlWinOfflineRemove.ResumeLayout(False)
        Me.grpHistoryView.ResumeLayout(False)
        Me.pnlWinOfflineButton2.ResumeLayout(False)
        Me.pnlWinOfflineSDHelp.ResumeLayout(False)
        Me.grpSDHelp.ResumeLayout(False)
        CType(Me.picSteps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlWinOfflineCLIHelp.ResumeLayout(False)
        Me.grpCLIOptions.ResumeLayout(False)
        Me.pnlSqlMdbOverview.ResumeLayout(False)
        Me.grpSqlMdbOverview.ResumeLayout(False)
        Me.grpSqlMdbOverview.PerformLayout()
        Me.grpSqlMdbItcmSum.ResumeLayout(False)
        Me.grpSqlMdbAgentVersion.ResumeLayout(False)
        CType(Me.dgvAgentVersion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSqlMdbDefSummary.ResumeLayout(False)
        CType(Me.dgvContentSummary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlMdbOverviewButtons.ResumeLayout(False)
        Me.pnlSqlTableSpaceGrid.ResumeLayout(False)
        Me.grpSqlTableSpace.ResumeLayout(False)
        CType(Me.dgvTableSpaceGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlTableSpaceGridButtons.ResumeLayout(False)
        Me.pnlSqlUserGrid.ResumeLayout(False)
        Me.tabCtrlUserGrid.ResumeLayout(False)
        Me.tabUserSummary.ResumeLayout(False)
        CType(Me.dgvUserGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabUserObsolete90.ResumeLayout(False)
        CType(Me.dgvUserObsolete90, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabUserObsolete365.ResumeLayout(False)
        CType(Me.dgvUserObsolete365, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlUserGridButtons.ResumeLayout(False)
        Me.pnlSqlAgentGrid.ResumeLayout(False)
        Me.tabCtrlAgentGrid.ResumeLayout(False)
        Me.tabAgentSummary.ResumeLayout(False)
        CType(Me.dgvAgentGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabAgentObsolete90.ResumeLayout(False)
        CType(Me.dgvAgentObsolete90, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabAgentObsolete365.ResumeLayout(False)
        CType(Me.dgvAgentObsolete365, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlAgentGridButtons.ResumeLayout(False)
        Me.pnlSqlServerGrid.ResumeLayout(False)
        Me.tabCtrlServerGrid.ResumeLayout(False)
        Me.tabServerSummary.ResumeLayout(False)
        CType(Me.dgvServerGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabServerLastCollected24.ResumeLayout(False)
        CType(Me.dgvServerLastCollected24, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabServerSignature30.ResumeLayout(False)
        CType(Me.dgvServerSignature30, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlServerGridButtons.ResumeLayout(False)
        Me.pnlSqlEngineGrid.ResumeLayout(False)
        Me.grpSqlEngineGrid.ResumeLayout(False)
        CType(Me.dgvEngineGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlEngineGridButtons.ResumeLayout(False)
        Me.pnlSqlGroupEvalGrid.ResumeLayout(False)
        Me.grpSqlGroupEvalGrid.ResumeLayout(False)
        CType(Me.dgvGroupEvalGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSqlGroupEvalGridInst.ResumeLayout(False)
        Me.pnlSqlGroupEvalGridButtons.ResumeLayout(False)
        Me.pnlSqlInstSoftGrid.ResumeLayout(False)
        Me.grpSqlInstSoftGrid.ResumeLayout(False)
        CType(Me.dgvSoftInst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlInstSoftGridButtons.ResumeLayout(False)
        Me.pnlSqlDiscSoftGrid.ResumeLayout(False)
        Me.grpDiscSoftGrid.ResumeLayout(False)
        Me.tabCtrlDiscSoftGrid.ResumeLayout(False)
        Me.tabDiscSignature.ResumeLayout(False)
        CType(Me.dgvDiscSignature, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDiscCustom.ResumeLayout(False)
        CType(Me.dgvDiscCustom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDiscHeuristic.ResumeLayout(False)
        CType(Me.dgvDiscHeuristic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDiscIntellisig.ResumeLayout(False)
        CType(Me.dgvDiscIntellisig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDiscEverything.ResumeLayout(False)
        CType(Me.dgvDiscEverything, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlDiscSoftGridButtons.ResumeLayout(False)
        Me.pnlSqlDuplCompGrid.ResumeLayout(False)
        Me.tabCtrlDuplComp.ResumeLayout(False)
        Me.tabDuplHostname.ResumeLayout(False)
        CType(Me.dgvDuplHostname, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDuplSerialNum.ResumeLayout(False)
        CType(Me.dgvDuplSerialNum, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDuplBoth.ResumeLayout(False)
        CType(Me.dgvDuplBoth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDuplBlank.ResumeLayout(False)
        CType(Me.dgvDuplBlank, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlDuplCompGridButtons.ResumeLayout(False)
        Me.pnlSqlUnUsedSoftGrid.ResumeLayout(False)
        Me.tabCtrlSwNotUsed.ResumeLayout(False)
        Me.tabSwNotUsed.ResumeLayout(False)
        CType(Me.dgvSwNotUsed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSwNotInst.ResumeLayout(False)
        CType(Me.dgvSwNotInst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSwNotStaged.ResumeLayout(False)
        CType(Me.dgvSwNotStaged, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSqlUnUsedSoftGridButtons.ResumeLayout(False)
        Me.pnlSqlCleanApps.ResumeLayout(False)
        Me.grpSqlCleanAppsInfo.ResumeLayout(False)
        Me.grpSqlCleanAppsOutput.ResumeLayout(False)
        Me.grpSqlCleanAppsOutput.PerformLayout()
        Me.pnlSqlCleanAppsButtons.ResumeLayout(False)
        Me.pnlSqlQueryEditor.ResumeLayout(False)
        Me.grpSqlQuery.ResumeLayout(False)
        Me.TabControlSql.ResumeLayout(False)
        Me.TabPageMessages.ResumeLayout(False)
        Me.TabPageMessages.PerformLayout()
        Me.pnlSqlQueryEditorButtons.ResumeLayout(False)
        Me.pnlENCOverdrive.ResumeLayout(False)
        Me.grpEncStressStatus.ResumeLayout(False)
        CType(Me.dgvEncStressTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEncStressTest.ResumeLayout(False)
        Me.grpEncStressPingType.ResumeLayout(False)
        Me.grpEncStressPingType.PerformLayout()
        Me.grpEncStressPingSettings.ResumeLayout(False)
        Me.grpEncStressPingSettings.PerformLayout()
        Me.pnlRemovalTool.ResumeLayout(False)
        Me.grpRemovalTool.ResumeLayout(False)
        Me.grpRemovalTool.PerformLayout()
        Me.grpRemovalOptions.ResumeLayout(False)
        Me.grpRemovalOptions.PerformLayout()
        Me.pnlRemovalToolButtons.ResumeLayout(False)
        Me.pnlDebug.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlWinOfflineMain As System.Windows.Forms.Panel
    Friend WithEvents pnlSystemInfo As System.Windows.Forms.Panel
    Friend WithEvents ExplorerTree As System.Windows.Forms.TreeView
    Friend WithEvents SplitWinOfflineUI As System.Windows.Forms.SplitContainer
    Friend WithEvents pnlWinOfflineStart As System.Windows.Forms.Panel
    Friend WithEvents pnlWinOfflineCLIHelp As System.Windows.Forms.Panel
    Friend WithEvents pnlWinOfflineSDHelp As System.Windows.Forms.Panel
    Friend WithEvents grpPatchView As System.Windows.Forms.GroupBox
    Friend WithEvents pnlWinOfflineButton1 As System.Windows.Forms.Panel
    Friend WithEvents btnWinOfflineStart1 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineExit3 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineBack3 As System.Windows.Forms.Button
    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblPlugins As System.Windows.Forms.Label
    Friend WithEvents lstPlugins As System.Windows.Forms.ListBox
    Friend WithEvents btnBrowseSSA As System.Windows.Forms.Button
    Friend WithEvents lblSSAFolder As System.Windows.Forms.Label
    Friend WithEvents txtSSAFolder As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseCAM As System.Windows.Forms.Button
    Friend WithEvents lblCAMFolder As System.Windows.Forms.Label
    Friend WithEvents txtCAMFolder As System.Windows.Forms.TextBox
    Friend WithEvents lblScalability As System.Windows.Forms.Label
    Friend WithEvents lblManager As System.Windows.Forms.Label
    Friend WithEvents txtDomainMgr As System.Windows.Forms.TextBox
    Friend WithEvents txtScalabilityServer As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseDSM As System.Windows.Forms.Button
    Friend WithEvents txtHostUUID As System.Windows.Forms.TextBox
    Friend WithEvents lblHostUUID As System.Windows.Forms.Label
    Friend WithEvents txtFunction As System.Windows.Forms.TextBox
    Friend WithEvents txtDSMFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtVersion As System.Windows.Forms.TextBox
    Friend WithEvents lblInstallDir As System.Windows.Forms.Label
    Friend WithEvents lblFunction As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents txtModel As System.Windows.Forms.TextBox
    Friend WithEvents lblModel As System.Windows.Forms.Label
    Friend WithEvents txtManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents lblManufacturer As System.Windows.Forms.Label
    Friend WithEvents txtPlatform As System.Windows.Forms.TextBox
    Friend WithEvents txtSerial As System.Windows.Forms.TextBox
    Friend WithEvents lblHostname As System.Windows.Forms.Label
    Friend WithEvents lblPlatform As System.Windows.Forms.Label
    Friend WithEvents txtHostname As System.Windows.Forms.TextBox
    Friend WithEvents lblSerial As System.Windows.Forms.Label
    Friend WithEvents lblNetwork As System.Windows.Forms.Label
    Friend WithEvents lstNetAddr As System.Windows.Forms.ListBox
    Friend WithEvents grpCLIOptions As System.Windows.Forms.GroupBox
    Friend WithEvents btnWinOfflineExit2 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineBack1 As System.Windows.Forms.Button
    Friend WithEvents rtbOptions As System.Windows.Forms.RichTextBox
    Friend WithEvents grpSDHelp As System.Windows.Forms.GroupBox
    Friend WithEvents btnWinOfflineSwicthes As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineBack2 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineSDHelpNext As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineSDHelpPrevious As System.Windows.Forms.Button
    Friend WithEvents picSteps As System.Windows.Forms.PictureBox
    Friend WithEvents lblStepx As System.Windows.Forms.Label
    Friend WithEvents grpWinOfflineWelcome As System.Windows.Forms.GroupBox
    Friend WithEvents btnWinOfflineNext1 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineExit1 As System.Windows.Forms.Button
    Friend WithEvents grpWinOfflineStartOptions As System.Windows.Forms.GroupBox
    Friend WithEvents rbnCLIHelp As System.Windows.Forms.RadioButton
    Friend WithEvents rbnBackout As System.Windows.Forms.RadioButton
    Friend WithEvents rbnLearn As System.Windows.Forms.RadioButton
    Friend WithEvents rbnApply As System.Windows.Forms.RadioButton
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents grpHistory As System.Windows.Forms.GroupBox
    Friend WithEvents treHistory As System.Windows.Forms.TreeView
    Friend WithEvents pnlDebug As System.Windows.Forms.Panel
    Friend WithEvents rtbDebug As System.Windows.Forms.RichTextBox
    Friend WithEvents lvwApplyOptions As System.Windows.Forms.ListView
    Friend WithEvents ColumnOptions As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwPatchList As System.Windows.Forms.ListView
    Friend WithEvents ColumnPatch As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnCode As System.Windows.Forms.ColumnHeader
    Friend WithEvents pnlWinOfflineRemove As System.Windows.Forms.Panel
    Friend WithEvents lvwRemoveOptions As System.Windows.Forms.ListView
    Friend WithEvents ColumnRemoveOptions As System.Windows.Forms.ColumnHeader
    Friend WithEvents grpHistoryView As System.Windows.Forms.GroupBox
    Friend WithEvents lvwHistory As System.Windows.Forms.ListView
    Friend WithEvents ColumnHistoryPatch As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHistoryComp As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHistoryStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnWinOfflineStart2 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineExit4 As System.Windows.Forms.Button
    Friend WithEvents btnWinOfflineBack4 As System.Windows.Forms.Button
    Friend WithEvents pnlSqlQueryEditor As Windows.Forms.Panel
    Friend WithEvents grpSqlQuery As Windows.Forms.GroupBox
    Friend WithEvents pnlWinOfflineButton2 As Windows.Forms.Panel
    Friend WithEvents TabControlSql As Windows.Forms.TabControl
    Friend WithEvents TabPageMessages As Windows.Forms.TabPage
    Friend WithEvents TabPageGrid As Windows.Forms.TabPage
    Friend WithEvents txtSqlMessage As Windows.Forms.TextBox
    Friend WithEvents rtbSqlQuery As Windows.Forms.RichTextBox
    Friend WithEvents pnlENCOverdrive As Windows.Forms.Panel
    Friend WithEvents grpEncStressStatus As Windows.Forms.GroupBox
    Friend WithEvents grpEncStressTest As Windows.Forms.GroupBox
    Friend WithEvents dgvEncStressTable As Windows.Forms.DataGridView
    Friend WithEvents grpEncStressPingSettings As Windows.Forms.GroupBox
    Friend WithEvents btnEncStressMsgSizePlus As Windows.Forms.Button
    Friend WithEvents btnEncStressMsgSizeMinus As Windows.Forms.Button
    Friend WithEvents btnEncStressNumPingPlus As Windows.Forms.Button
    Friend WithEvents lblEncStressNumPings As Windows.Forms.Label
    Friend WithEvents btnEncStressNumPingMinus As Windows.Forms.Button
    Friend WithEvents btnEncStressPingThreadPlus As Windows.Forms.Button
    Friend WithEvents btnEncStressPingThreadMinus As Windows.Forms.Button
    Friend WithEvents btnEncStressTargetThreadPlus As Windows.Forms.Button
    Friend WithEvents btnEncStressTargetThreadMinus As Windows.Forms.Button
    Friend WithEvents txtEncStressMsgSize As Windows.Forms.TextBox
    Friend WithEvents txtEncStressNumPings As Windows.Forms.TextBox
    Friend WithEvents txtEncStressNumTargetThreads As Windows.Forms.TextBox
    Friend WithEvents txtEncStressNumPingThreads As Windows.Forms.TextBox
    Friend WithEvents lblEncStressMsgSize As Windows.Forms.Label
    Friend WithEvents lblEncStressPingThreads As Windows.Forms.Label
    Friend WithEvents lblEncStressTargetThreads As Windows.Forms.Label
    Friend WithEvents btnEncStressStop As Windows.Forms.Button
    Friend WithEvents btnEncStressStart As Windows.Forms.Button
    Friend WithEvents btnEncStressReplyTimeoutPlus As Windows.Forms.Button
    Friend WithEvents btnEncStressReplyTimeoutMinus As Windows.Forms.Button
    Friend WithEvents txtEncStressReplyTimeout As Windows.Forms.TextBox
    Friend WithEvents lblEncStressReplyTimeout As Windows.Forms.Label
    Friend WithEvents btnEncStressDelayTimePlus As Windows.Forms.Button
    Friend WithEvents btnEncStressDelayTimeMinus As Windows.Forms.Button
    Friend WithEvents txtEncStressDelayTime As Windows.Forms.TextBox
    Friend WithEvents lblEncStressDelayTime As Windows.Forms.Label
    Friend WithEvents grpEncStressPingType As Windows.Forms.GroupBox
    Friend WithEvents rbnEncStressCafPing As Windows.Forms.RadioButton
    Friend WithEvents rbnEncStressCamPing As Windows.Forms.RadioButton
    Friend WithEvents lblEncStressIntro As Windows.Forms.Label
    Friend WithEvents pnlSqlCleanApps As Windows.Forms.Panel
    Friend WithEvents grpSqlCleanAppsOutput As Windows.Forms.GroupBox
    Friend WithEvents pnlSqlMdbOverview As Windows.Forms.Panel
    Friend WithEvents grpSqlMdbOverview As Windows.Forms.GroupBox
    Friend WithEvents lblMdbVersion As Windows.Forms.Label
    Friend WithEvents lblMdbInstallDate As Windows.Forms.Label
    Friend WithEvents lblITCMVersion As Windows.Forms.Label
    Friend WithEvents txtITCMVersion As Windows.Forms.TextBox
    Friend WithEvents txtMdbInstallDate As Windows.Forms.TextBox
    Friend WithEvents txtMdbVersion As Windows.Forms.TextBox
    Friend WithEvents txtMdbType As Windows.Forms.TextBox
    Friend WithEvents lblMdbType As Windows.Forms.Label
    Friend WithEvents grpSqlMdbItcmSum As Windows.Forms.GroupBox
    Friend WithEvents grpSqlMdbDefSummary As Windows.Forms.GroupBox
    Friend WithEvents dgvContentSummary As Windows.Forms.DataGridView
    Friend WithEvents lvwITCMSummary As Windows.Forms.ListView
    Friend WithEvents ColumnMetric As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnValue As Windows.Forms.ColumnHeader
    Friend WithEvents pnlSqlTableSpaceGrid As Windows.Forms.Panel
    Friend WithEvents grpSqlTableSpace As Windows.Forms.GroupBox
    Friend WithEvents dgvTableSpaceGrid As Windows.Forms.DataGridView
    Friend WithEvents pnlSqlServerGrid As Windows.Forms.Panel
    Friend WithEvents pnlSqlEngineGrid As Windows.Forms.Panel
    Friend WithEvents grpSqlEngineGrid As Windows.Forms.GroupBox
    Friend WithEvents dgvEngineGrid As Windows.Forms.DataGridView
    Friend WithEvents grpSqlMdbAgentVersion As Windows.Forms.GroupBox
    Friend WithEvents dgvAgentVersion As Windows.Forms.DataGridView
    Friend WithEvents pnlSqlAgentGrid As Windows.Forms.Panel
    Friend WithEvents pnlSqlUnUsedSoftGrid As Windows.Forms.Panel
    Friend WithEvents dgvSwNotStaged As Windows.Forms.DataGridView
    Friend WithEvents dgvSwNotInst As Windows.Forms.DataGridView
    Friend WithEvents pnlSqlMdbOverviewButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlExitMdbOverview As Windows.Forms.Button
    Friend WithEvents btnSqlExportMdbOverview As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshMdbOverview As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectMdbOverview As Windows.Forms.Button
    Friend WithEvents btnSqlConnectMdbOverview As Windows.Forms.Button
    Friend WithEvents pnlSqlTableSpaceGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlExitTableSpace As Windows.Forms.Button
    Friend WithEvents btnSqlExportTableSpace As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshTableSpace As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectTableSpace As Windows.Forms.Button
    Friend WithEvents btnSqlConnectTableSpace As Windows.Forms.Button
    Friend WithEvents pnlSqlAgentGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectAgentGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectAgentGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshAgentGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportAgentGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitAgentGrid As Windows.Forms.Button
    Friend WithEvents pnlSqlServerGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectServerGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectServerGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshServerGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportServerGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitServerGrid As Windows.Forms.Button
    Friend WithEvents pnlSqlEngineGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectEngineGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectEngineGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshEngineGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportEngineGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitEngineGrid As Windows.Forms.Button
    Friend WithEvents tabCtrlSwNotUsed As Windows.Forms.TabControl
    Friend WithEvents tabSwNotUsed As Windows.Forms.TabPage
    Friend WithEvents dgvSwNotUsed As Windows.Forms.DataGridView
    Friend WithEvents tabSwNotInst As Windows.Forms.TabPage
    Friend WithEvents tabSwNotStaged As Windows.Forms.TabPage
    Friend WithEvents pnlSqlUnUsedSoftGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectUnUsedSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectUnUsedSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshUnUsedSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportUnUsedSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitUnUsedSoftGrid As Windows.Forms.Button
    Friend WithEvents pnlSqlCleanAppsButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectCleanApps As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectCleanApps As Windows.Forms.Button
    Friend WithEvents btnSqlCafStopCleanApps As Windows.Forms.Button
    Friend WithEvents btnSqlRunCleanApps As Windows.Forms.Button
    Friend WithEvents btnSqlExitCleanApps As Windows.Forms.Button
    Friend WithEvents txtSqlCleanApps As Windows.Forms.TextBox
    Friend WithEvents grpSqlCleanAppsInfo As Windows.Forms.GroupBox
    Friend WithEvents lblSqlCleanAppsIntro As Windows.Forms.Label
    Friend WithEvents pnlSqlQueryEditorButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectQueryEditor As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectQueryEditor As Windows.Forms.Button
    Friend WithEvents btnSqlSubmitQueryEditor As Windows.Forms.Button
    Friend WithEvents btnSqlCancelQueryEditor As Windows.Forms.Button
    Friend WithEvents btnSqlExitQueryEditor As Windows.Forms.Button
    Friend WithEvents prgAgentGrid As Windows.Forms.ProgressBar
    Friend WithEvents pnlSqlDuplCompGrid As Windows.Forms.Panel
    Friend WithEvents pnlSqlDuplCompGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectDuplCompGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectDuplCompGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshDuplCompGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportDuplCompGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitDuplCompGrid As Windows.Forms.Button
    Friend WithEvents tabCtrlDuplComp As Windows.Forms.TabControl
    Friend WithEvents tabDuplHostname As Windows.Forms.TabPage
    Friend WithEvents dgvDuplHostname As Windows.Forms.DataGridView
    Friend WithEvents tabDuplSerialNum As Windows.Forms.TabPage
    Friend WithEvents dgvDuplSerialNum As Windows.Forms.DataGridView
    Friend WithEvents tabDuplBoth As Windows.Forms.TabPage
    Friend WithEvents dgvDuplBoth As Windows.Forms.DataGridView
    Friend WithEvents pnlSqlInstSoftGrid As Windows.Forms.Panel
    Friend WithEvents pnlSqlInstSoftGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectInstSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectInstSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshInstSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportInstSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitInstSoftGrid As Windows.Forms.Button
    Friend WithEvents grpSqlInstSoftGrid As Windows.Forms.GroupBox
    Friend WithEvents dgvSoftInst As Windows.Forms.DataGridView
    Friend WithEvents pnlSqlDiscSoftGrid As Windows.Forms.Panel
    Friend WithEvents grpDiscSoftGrid As Windows.Forms.GroupBox
    Friend WithEvents lblDiscSoftGrid As Windows.Forms.Label
    Friend WithEvents tabCtrlDiscSoftGrid As Windows.Forms.TabControl
    Friend WithEvents tabDiscSignature As Windows.Forms.TabPage
    Friend WithEvents dgvDiscSignature As Windows.Forms.DataGridView
    Friend WithEvents tabDiscCustom As Windows.Forms.TabPage
    Friend WithEvents dgvDiscCustom As Windows.Forms.DataGridView
    Friend WithEvents tabDiscHeuristic As Windows.Forms.TabPage
    Friend WithEvents dgvDiscHeuristic As Windows.Forms.DataGridView
    Friend WithEvents tabDiscIntellisig As Windows.Forms.TabPage
    Friend WithEvents dgvDiscIntellisig As Windows.Forms.DataGridView
    Friend WithEvents tabDiscEverything As Windows.Forms.TabPage
    Friend WithEvents dgvDiscEverything As Windows.Forms.DataGridView
    Friend WithEvents pnlSqlDiscSoftGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectDiscSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectDiscSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshDiscSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportDiscSoftGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitDiscSoftGrid As Windows.Forms.Button
    Friend WithEvents tabCtrlAgentGrid As Windows.Forms.TabControl
    Friend WithEvents tabAgentSummary As Windows.Forms.TabPage
    Friend WithEvents dgvAgentGrid As Windows.Forms.DataGridView
    Friend WithEvents tabAgentObsolete90 As Windows.Forms.TabPage
    Friend WithEvents dgvAgentObsolete90 As Windows.Forms.DataGridView
    Friend WithEvents tabAgentObsolete365 As Windows.Forms.TabPage
    Friend WithEvents dgvAgentObsolete365 As Windows.Forms.DataGridView
    Friend WithEvents tabCtrlServerGrid As Windows.Forms.TabControl
    Friend WithEvents tabServerSummary As Windows.Forms.TabPage
    Friend WithEvents dgvServerGrid As Windows.Forms.DataGridView
    Friend WithEvents tabServerLastCollected24 As Windows.Forms.TabPage
    Friend WithEvents dgvServerLastCollected24 As Windows.Forms.DataGridView
    Friend WithEvents tabServerSignature30 As Windows.Forms.TabPage
    Friend WithEvents dgvServerSignature30 As Windows.Forms.DataGridView
    Friend WithEvents tabDuplBlank As Windows.Forms.TabPage
    Friend WithEvents dgvDuplBlank As Windows.Forms.DataGridView
    Friend WithEvents prgDiscSoftGrid As Windows.Forms.ProgressBar
    Friend WithEvents prgDuplCompGrid As Windows.Forms.ProgressBar
    Friend WithEvents prgTableSpaceGrid As Windows.Forms.ProgressBar
    Friend WithEvents prgServerGrid As Windows.Forms.ProgressBar
    Friend WithEvents prgEngineGrid As Windows.Forms.ProgressBar
    Friend WithEvents prgInstSoftGrid As Windows.Forms.ProgressBar
    Friend WithEvents prgUnUsedSoftGrid As Windows.Forms.ProgressBar
    Friend WithEvents pnlSqlUserGrid As Windows.Forms.Panel
    Friend WithEvents tabCtrlUserGrid As Windows.Forms.TabControl
    Friend WithEvents tabUserSummary As Windows.Forms.TabPage
    Friend WithEvents dgvUserGrid As Windows.Forms.DataGridView
    Friend WithEvents tabUserObsolete90 As Windows.Forms.TabPage
    Friend WithEvents dgvUserObsolete90 As Windows.Forms.DataGridView
    Friend WithEvents tabUserObsolete365 As Windows.Forms.TabPage
    Friend WithEvents dgvUserObsolete365 As Windows.Forms.DataGridView
    Friend WithEvents prgUserGrid As Windows.Forms.ProgressBar
    Friend WithEvents pnlSqlUserGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectUserGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectUserGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshUserGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportUserGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitUserGrid As Windows.Forms.Button
    Friend WithEvents pnlRemovalTool As Windows.Forms.Panel
    Friend WithEvents grpRemovalTool As Windows.Forms.GroupBox
    Friend WithEvents txtRemovalTool As Windows.Forms.TextBox
    Friend WithEvents grpRemovalOptions As Windows.Forms.GroupBox
    Friend WithEvents rbnRemoveITCM As Windows.Forms.RadioButton
    Friend WithEvents chkRetainHostUUID As Windows.Forms.CheckBox
    Friend WithEvents rbnUninstallITCM As Windows.Forms.RadioButton
    Friend WithEvents pnlRemovalToolButtons As Windows.Forms.Panel
    Friend WithEvents btnRemoveITCM As Windows.Forms.Button
    Friend WithEvents btnExitRemoveITCM As Windows.Forms.Button
    Friend WithEvents lblRemoveITCMCaution As Windows.Forms.Label
    Friend WithEvents pnlSqlGroupEvalGrid As Windows.Forms.Panel
    Friend WithEvents pnlSqlGroupEvalGridButtons As Windows.Forms.Panel
    Friend WithEvents btnSqlConnectGroupEvalGrid As Windows.Forms.Button
    Friend WithEvents btnSqlDisconnectGroupEvalGrid As Windows.Forms.Button
    Friend WithEvents btnSqlRefreshGroupEvalGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExportGroupEvalGrid As Windows.Forms.Button
    Friend WithEvents btnSqlExitGroupEvalGrid As Windows.Forms.Button
    Friend WithEvents grpSqlGroupEvalGrid As Windows.Forms.GroupBox
    Friend WithEvents dgvGroupEvalGrid As Windows.Forms.DataGridView
    Friend WithEvents prgGroupEvalGrid As Windows.Forms.ProgressBar
    Friend WithEvents grpSqlGroupEvalGridInst As Windows.Forms.GroupBox
    Friend WithEvents lblGroupEvalGrid As Windows.Forms.Label
    Friend WithEvents btnGroupEvalGridCommit As Windows.Forms.Button
    Friend WithEvents btnGroupEvalGridDiscard As Windows.Forms.Button
    Friend WithEvents btnGroupEvalGridPreview As Windows.Forms.Button
End Class

Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    Private Const MF_STRING As Integer = &H0
    Private Const MF_SEPARATOR As Integer = &H800
    Private Const WM_SYSCOMMAND As Int32 = &H112
    Private SystemInfoUpdateThread As Thread
    Private HiddenNodes As New ArrayList
    Private TerminateSignal As Boolean = False

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Private Shared Function GetSystemMenu(
        <[In]()> ByVal hWnd As IntPtr,
        <[In]()> ByVal bRevert As Boolean) _
        As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Private Shared Function AppendMenu(
        <[In]()> ByVal hMenu As IntPtr,
        <[In]()> ByVal uFlags As Int32,
        <[In]()> ByVal uIDNewItem As IntPtr,
        <[In](), [Optional]> ByVal lpNewItem As String) _
        As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Private Sub WinOfflineUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AddSysMenuItems()
        Text = Globals.ProcessFriendlyName + " -- " + Globals.AppVersion

        pnlWinOfflineStart.AutoScrollMinSize = pnlWinOfflineStart.Size
        pnlWinOfflineMain.AutoScrollMinSize = pnlWinOfflineMain.Size
        pnlWinOfflineRemove.AutoScrollMinSize = pnlWinOfflineRemove.Size
        pnlWinOfflineSDHelp.AutoScrollMinSize = pnlWinOfflineSDHelp.Size
        pnlWinOfflineCLIHelp.AutoScrollMinSize = pnlWinOfflineCLIHelp.Size
        pnlENCOverdrive.AutoScrollMinSize = pnlENCOverdrive.Size
        pnlSqlMdbOverview.AutoScrollMinSize = pnlSqlMdbOverview.Size
        pnlSqlTableSpaceGrid.AutoScrollMinSize = pnlSqlTableSpaceGrid.Size
        pnlSqlAgentGrid.AutoScrollMinSize = pnlSqlAgentGrid.Size
        pnlSqlUserGrid.AutoScrollMinSize = pnlSqlUserGrid.Size
        pnlSqlServerGrid.AutoScrollMinSize = pnlSqlServerGrid.Size
        pnlSqlEngineGrid.AutoScrollMinSize = pnlSqlEngineGrid.Size
        pnlSqlGroupEvalGrid.AutoScrollMinSize = pnlSqlGroupEvalGrid.Size
        pnlSqlInstSoftGrid.AutoScrollMinSize = pnlSqlInstSoftGrid.Size
        pnlSqlDiscSoftGrid.AutoScrollMinSize = pnlSqlDiscSoftGrid.Size
        pnlSqlDuplCompGrid.AutoScrollMinSize = pnlSqlDuplCompGrid.Size
        pnlSqlUnUsedSoftGrid.AutoScrollMinSize = pnlSqlUnUsedSoftGrid.Size
        pnlSqlCleanApps.AutoScrollMinSize = pnlSqlCleanApps.Size
        pnlSqlQueryEditor.AutoScrollMinSize = pnlSqlQueryEditor.Size
        pnlRemovalTool.AutoScrollMinSize = pnlRemovalTool.Size
        pnlSystemInfo.AutoScrollMinSize = pnlSystemInfo.Size
        pnlDebug.AutoScrollMinSize = pnlDebug.Size

        InitWinOffline()
        InitSystemInfo()
        InitEncOverdrive()
        InitSqlMdbOverview()
        InitSqlTableSpaceGrid()
        InitSqlAgentGrid()
        InitSqlUserGrid()
        InitSqlServerGrid()
        InitSqlEngineGrid()
        InitSqlGroupEvalGrid()
        InitSqlInstSoftGrid()
        InitSqlDiscSoftGrid()
        InitSqlDuplCompGrid()
        InitSqlUnUsedSoftGrid()
        InitSqlCleanApps()
        InitSqlQueryEditor()

        ExplorerTree.SelectedNode = GetTreeNodebyName("WinOfflineNode")
        ExplorerTree.ExpandAll()
        RecurseTreeName(ExplorerTree) ' Recurse tree node names for reflexsive "WinOffline" references

        HiddenNodes.Add(GetTreeNodebyName("DebugNode"))
        ExplorerTree.Nodes(0).Nodes.Remove(GetTreeNodebyName("DebugNode"))

        ' Resize form based on user DPI setting (100% = 96 DPI)
        Using myGraphics As Drawing.Graphics = CreateGraphics()
            Width = myGraphics.DpiX / 96 * Size.Width
            Height = myGraphics.DpiY / 96 * Size.Height
        End Using

    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If (m.Msg = WM_SYSCOMMAND) Then
            Select Case m.WParam.ToInt32
                Case MF_STRING
                    Dim myAboutBox As New AboutBox
                    myAboutBox.ShowDialog()
            End Select
        End If
    End Sub

    Private Sub AddSysMenuItems()
        Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
        AppendMenu(hSysMenu, MF_SEPARATOR, IntPtr.Zero, Nothing)
        AppendMenu(hSysMenu, MF_STRING, IntPtr.Zero, "About")
    End Sub

    Private Sub WinOfflineUI_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Visible = False
        CancelSignal = True
        TerminateSignal = True

        While True ' Loop while external threads are still running
            If (SystemInfoUpdateThread IsNot Nothing AndAlso SystemInfoUpdateThread.IsAlive) OrElse
                (PatchListViewThread IsNot Nothing AndAlso PatchListViewThread.IsAlive) OrElse
                (EncClientMonitorThread IsNot Nothing AndAlso EncClientMonitorThread.IsAlive) OrElse
                (EncOverdriveThread IsNot Nothing AndAlso EncOverdriveThread.IsAlive) OrElse
                (SqlCleanAppsThread IsNot Nothing AndAlso SqlCleanAppsThread.IsAlive) OrElse
                (MdbOverviewThread IsNot Nothing AndAlso MdbOverviewThread.IsAlive) OrElse
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
                (DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive) OrElse
                (RemovalToolThread IsNot Nothing AndAlso RemovalToolThread.IsAlive) Then
                Application.DoEvents()
            Else
                Exit While
            End If
        End While

        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Abort
        End If

    End Sub

    Private Sub WinOfflineUI_KeyDown(sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.Shift AndAlso e.KeyCode = Keys.D Then
            If pnlDebug.Visible Then
                HiddenNodes.Add(GetTreeNodebyName("DebugNode"))
                ExplorerTree.Nodes(0).Nodes.Remove(GetTreeNodebyName("DebugNode"))
                pnlDebug.Visible = False
            Else
                ExplorerTree.Nodes(0).Nodes.Add(GetHiddenNodebyName("DebugNode"))
                HiddenNodes.Remove(GetHiddenNodebyName("DebugNode"))
                ExplorerTree.SelectedNode = GetTreeNodebyName("DebugNode")
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            If pnlSqlMdbOverview.Visible Then
                btnSqlConnectMdbOverview.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlTableSpaceGrid.Visible Then
                btnSqlConnectTableSpace.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlAgentGrid.Visible Then
                btnSqlConnectAgentGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlUserGrid.Visible Then
                btnSqlConnectUserGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlServerGrid.Visible Then
                btnSqlConnectServerGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlEngineGrid.Visible Then
                btnSqlConnectEngineGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlGroupEvalGrid.Visible Then
                btnSqlConnectGroupEvalGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlInstSoftGrid.Visible Then
                btnSqlConnectInstSoftGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlDiscSoftGrid.Visible Then
                btnSqlConnectDiscSoftGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlDuplCompGrid.Visible Then
                btnSqlConnectDuplCompGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlUnUsedSoftGrid.Visible Then
                btnSqlConnectUnUsedSoftGrid.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlCleanApps.Visible Then
                btnSqlConnectCleanApps.PerformClick()
                e.SuppressKeyPress = True
            ElseIf pnlSqlQueryEditor.Visible Then
                btnSqlConnectQueryEditor.PerformClick()
                e.SuppressKeyPress = True
            End If
        End If

    End Sub

    Private Sub RecurseTreeName(ByRef t As TreeView)
        For Each n As TreeNode In t.Nodes
            RecurseNodeNames(n)
        Next
    End Sub

    Private Sub RecurseNodeNames(ByRef n As TreeNode)
        If n.Text.Contains("WinOffline") Then n.Text = n.Text.Replace("WinOffline", Globals.ProcessFriendlyName)
        For Each t As TreeNode In n.Nodes
            If t.GetNodeCount(True) > 0 Then RecurseNodeNames(t)
            If t.Text.Contains("WinOffline") Then t.Text = t.Text.Replace("WinOffline", Globals.ProcessFriendlyName)
        Next
    End Sub

    Private Function GetTreeNodebyName(ByVal Name As String) As TreeNode
        For Each Node As TreeNode In ExplorerTree.Nodes(0).Nodes
            If Node.Name.ToLower.Equals(Name.ToLower) Then Return Node
        Next
        Return Nothing
    End Function

    Private Function GetHiddenNodebyName(ByVal Name As String) As TreeNode
        For Each Node As TreeNode In HiddenNodes
            If Node.Name.ToLower.Equals(Name.ToLower) Then Return Node
        Next
        Return Nothing
    End Function

    Private Sub HideAllPanels()
        pnlWinOfflineStart.Visible = False
        pnlWinOfflineMain.Visible = False
        pnlWinOfflineRemove.Visible = False
        pnlWinOfflineCLIHelp.Visible = False
        pnlWinOfflineSDHelp.Visible = False
        pnlENCOverdrive.Visible = False
        pnlSqlMdbOverview.Visible = False
        pnlSqlTableSpaceGrid.Visible = False
        pnlSqlAgentGrid.Visible = False
        pnlSqlUserGrid.Visible = False
        pnlSqlServerGrid.Visible = False
        pnlSqlEngineGrid.Visible = False
        pnlSqlGroupEvalGrid.Visible = False
        pnlSqlInstSoftGrid.Visible = False
        pnlSqlDiscSoftGrid.Visible = False
        pnlSqlDuplCompGrid.Visible = False
        pnlSqlUnUsedSoftGrid.Visible = False
        pnlSqlCleanApps.Visible = False
        pnlSqlQueryEditor.Visible = False
        pnlRemovalTool.Visible = False
        pnlSystemInfo.Visible = False
        pnlDebug.Visible = False
    End Sub

    Private Sub ExplorerTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles ExplorerTree.AfterSelect
        HideAllPanels()
        If e.Node.Name.Equals("RootNode") Or e.Node.Name.Equals("WinOfflineNode") Then
            pnlWinOfflineStart.Visible = True
        ElseIf e.Node.Name.Equals("SqlMdbOverviewNode") Then
            pnlSqlMdbOverview.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso MdbOverviewThread Is Nothing Then
                MdbOverviewThread = New Thread(Sub() MdbOverviewWorker(ConnectionString))
                MdbOverviewThread.Start()
            End If
        ElseIf e.Node.Name.Equals("SqlTableSpaceGridNode") Then
            pnlSqlTableSpaceGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso TableSpaceGridThread Is Nothing Then
                TableSpaceGridThread = New Thread(Sub() TableSpaceGridWorker(ConnectionString))
                TableSpaceGridThread.Start()

            End If
        ElseIf e.Node.Name.Equals("AgentGridNode") Then
            pnlSqlAgentGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso AgentGridThread Is Nothing Then
                AgentGridThread = New Thread(Sub() AgentGridWorker(ConnectionString))
                AgentGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("UserGridNode") Then
            pnlSqlUserGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso UserGridThread Is Nothing Then
                UserGridThread = New Thread(Sub() UserGridWorker(ConnectionString))
                UserGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("ServerGridNode") Then
            pnlSqlServerGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso ServerGridThread Is Nothing Then
                ServerGridThread = New Thread(Sub() ServerGridWorker(ConnectionString))
                ServerGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("EngineGridNode") Then
            pnlSqlEngineGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso EngineGridThread Is Nothing Then
                EngineGridThread = New Thread(Sub() EngineGridWorker(ConnectionString))
                EngineGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("GroupEvalGridNode") Then
            pnlSqlGroupEvalGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso GroupEvalGridThread Is Nothing Then
                GroupEvalGridThread = New Thread(Sub() GroupEvalGridWorker(ConnectionString))
                GroupEvalGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("InstSoftGridNode") Then
            pnlSqlInstSoftGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso InstSoftGridThread Is Nothing Then
                InstSoftGridThread = New Thread(Sub() InstSoftGridWorker(ConnectionString))
                InstSoftGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("DiscSoftGridNode") Then
            pnlSqlDiscSoftGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso DiscSoftGridThread Is Nothing Then
                DiscSoftGridThread = New Thread(Sub() DiscSoftGridWorker(ConnectionString))
                DiscSoftGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("DuplCompGridNode") Then
            pnlSqlDuplCompGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso DuplCompGridThread Is Nothing Then
                DuplCompGridThread = New Thread(Sub() DuplCompGridWorker(ConnectionString))
                DuplCompGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("UnUsedSoftGridNode") Then
            pnlSqlUnUsedSoftGrid.Visible = True
            If DatabaseThread IsNot Nothing AndAlso DatabaseThread.IsAlive AndAlso UnUsedSoftGridThread Is Nothing Then
                UnUsedSoftGridThread = New Thread(Sub() UnUsedSoftGridWorker(ConnectionString))
                UnUsedSoftGridThread.Start()
            End If
        ElseIf e.Node.Name.Equals("SqlCleanAppsNode") Then
            pnlSqlCleanApps.Visible = True
        ElseIf e.Node.Name.Equals("SqlQueryNode") Then
            pnlSqlQueryEditor.Visible = True
        ElseIf e.Node.Name.Equals("EncToolsNode") Or e.Node.Name.Equals("EncStressNode") Then
            pnlENCOverdrive.Visible = True
        ElseIf e.Node.Name.Equals("RemovalToolNode") Then
            pnlRemovalTool.Visible = True
        ElseIf e.Node.Name.Equals("SysInfoNode") Then
            pnlSystemInfo.Visible = True
        ElseIf e.Node.Name.Equals("DebugNode") Then
            pnlDebug.Visible = True
            rtbDebug.ScrollToCaret()
        End If
    End Sub

End Class
Imports System.Windows.Forms

Partial Public Class WinOfflineUI

    ' A note on multithreading and the use of delegates:
    '  If you have two or more threads manipulating the same GUI control, it's possible to force
    '  the control into an inconsistent state.
    '
    '  To avoid "cross-thread exceptions" (deadlock, race conditions, etc.), you are required to
    '  synchronize updates to GUI controls, from foreign threads, so updates to those controls
    '  only occur on the thread owning those controls.
    '
    '  In WinOfflineUI, as an example, the main WinOfflineUI GUI spawns a thread of its own
    '  called, SystemInfoUpdateThread, to monitor and update the System Information panel in the
    '  background. A seperate thread is needed, otherwise we block/freeze the main GUI every time
    '  we want to run an udpate on the System Information panel.
    '
    '  As the SystemInfoUpdateThread is a completly foreign/distinct thread from the main
    '  WinOfflineGUI thread, it does not own any of the controls on the SystemInfo panel.
    '  Thus when the SystemInfoUpdateThread wants to update the text for some system info control,
    '  it must invoke these updates, through the use of delegates, so these updates are passed
    '  appropriately to the WinOfflineUI thread, and updates are performed in a thread-safe, 
    '  synchronous method.

    Private Delegate Sub Delegate_Set_Text(ByVal MyControl As Control, ByVal MyText As String)
    Private Delegate Sub Delegate_Set_Visible(ByVal MyControl As Control, ByVal IsVisible As Boolean)
    Private Delegate Sub Delegate_Set_ListBox(ByVal MyListBox As ListBox, ByVal MyArray As ArrayList)
    Private Delegate Sub Delegate_Set_Patch_ListView(ByVal MyListView As ListView, ByVal MyArray As ArrayList)
    Private Delegate Sub Delegate_Set_Active_Tab(ByVal MyTabControl As TabControl, ByVal MyTabPage As TabPage)
    Private Delegate Sub Delegate_Set_DataGrid_Fill_Column(ByVal MyDataGridView As DataGridView, ByVal FillColumn As String)
    Private Delegate Sub Delegate_Set_DataGrid_Column_ReadOnly(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal isReadOnly As Boolean)
    Private Delegate Sub Delegate_Modify_Button(ByVal MyButton As Button)
    Private Delegate Sub Delegate_Modify_ListView(ByVal MyListView As ListView)
    Private Delegate Sub Delegate_Modify_ListBox(ByVal MyListBox As ListBox)
    Private Delegate Sub Delegate_Modify_DataGridView(ByVal MyDataGridView As DataGridView)
    Private Delegate Sub Delegate_Enable(ByVal IsEnabled As Boolean)
    Private Delegate Sub Delegate_Enable_Control(ByVal MyControl As Control, ByVal IsEnabled As Boolean)
    Private Delegate Sub Delegate_Hide_DataGridView_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String)
    Private Delegate Sub Delegate_Insert_DataGridView_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnIndex As Integer, ByVal ColumnName As String, ByVal HeaderText As String)
    Private Delegate Sub Delegate_Add_DataGridView_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal HeaderText As String)
    Private Delegate Sub Delegate_Add_DataGridView_Row(ByVal MyDataGridView As DataGridView, ByVal RowArray As Object())
    Private Delegate Sub Delegate_Add_DataGridView_Row_Custom_Height(ByVal MyDataGridView As DataGridView, ByVal RowArray As Object(), ByVal Height As Integer)
    Private Delegate Sub Delegate_Remove_DataGridView_Row(ByVal MyDataGridView As DataGridView, ByVal RowIndex As Integer)
    Private Delegate Sub Delegate_Update_DataGridView_Value(ByVal MyDataGridView As DataGridView, ByVal RowIndex As Integer, ByVal ColName As String, ByVal NewValue As Object)
    Private Delegate Sub Delegate_Add_Control(ByVal ParentControl As Control, ByVal ChildControl As Control)
    Private Delegate Sub Delegate_Remove_Child_Controls(ByVal MyControl As Control)
    Private Delegate Sub Delegate_Resize_DataGrid_ColumnName(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal ColumnSize As Integer)
    Private Delegate Sub Delegate_Resize_DataGrid_ColumnIndex(ByVal MyDataGridView As DataGridView, ByVal ColumnIndex As Integer, ByVal ColumnSize As Integer)
    Private Delegate Sub Delegate_ResizeMode_DataGrid_ColumnName(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal ResizeMode As DataGridViewAutoSizeColumnsMode)
    Private Delegate Sub Delegate_ResizeMode_DataGrid_ColumnIndex(ByVal MyDataGridView As DataGridView, ByVal ColumnIndex As Integer, ByVal ResizeMode As DataGridViewAutoSizeColumnsMode)
    Private Delegate Sub Delegate_Resize_DataGrid_Columns(ByVal MyDataGridView As DataGridView, ByVal ResizeMode As DataGridViewAutoSizeColumnsMode)
    Private Delegate Sub Delegate_Resize_DataGrid_Rows(ByVal MyDataGridView As DataGridView, ByVal ResizeMode As DataGridViewAutoSizeRowsMode)
    Private Delegate Sub Delegate_Resize_DataGrid_Row_Template_Height(ByVal MyDataGridView As DataGridView, ByVal Height As Integer)
    Private Delegate Sub Delegate_Refresh_DataGrid(ByVal MyDataGridView As DataGridView)
    Private Delegate Function Callback_Read_Text(ByVal MyControl As Control) As String
    Private Delegate Function Callback_Read_Selected_Text(ByVal MyRichTextBox As RichTextBox) As String
    Private Delegate Function Callback_Read_Selected_TreeNode(ByVal MyTreeView As TreeView) As String

    Public Sub Delegate_Sub_Enable_Control(ByVal MyControl As Control, ByVal IsEnabled As Boolean)
        If MyControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Control)
            Invoke(MyDelegate, MyControl, IsEnabled)
        Else
            MyControl.Enabled = IsEnabled
        End If
    End Sub

    Public Sub Delegate_Sub_Set_Text(ByVal MyControl As Control, ByVal MyText As String)
        If MyControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_Text(AddressOf Delegate_Sub_Set_Text)
            Invoke(MyDelegate, MyControl, MyText)
        Else
            MyControl.Text = MyText
        End If
    End Sub

    Public Sub Delegate_Sub_Set_Visible(ByVal MyControl As Control, ByVal IsVisible As Boolean)
        If MyControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_Visible(AddressOf Delegate_Sub_Set_Visible)
            Invoke(MyDelegate, MyControl, IsVisible)
        Else
            MyControl.Visible = IsVisible
        End If
    End Sub

    Public Sub Delegate_Sub_Append_Text(ByVal MyControl As Control, ByVal MyText As String)
        If MyControl IsNot Nothing AndAlso MyControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_Text(AddressOf Delegate_Sub_Append_Text)
            Invoke(MyDelegate, MyControl, MyText)
        Else
            If TryCast(MyControl, RichTextBox) IsNot Nothing Then
                If MyControl.Equals(rtbDebug) And MyText.StartsWith("SystemInfoThread -->") Then
                    TryCast(MyControl, RichTextBox).Text += MyText + Environment.NewLine + Environment.NewLine
                ElseIf MyControl.Equals(rtbDebug) And MyText.Contains("-->") Then
                    TryCast(MyControl, RichTextBox).AppendText(MyText + Environment.NewLine + Environment.NewLine) ' Append text without autoscroll
                Else
                    TryCast(MyControl, RichTextBox).AppendText(MyText + Environment.NewLine) ' Use AppendText() and focus for autoscroll
                End If
            ElseIf TryCast(MyControl, TextBox) IsNot Nothing Then
                TryCast(MyControl, TextBox).AppendText(MyText + Environment.NewLine) ' Use AppendText() and focus for autoscroll
            Else
                MyControl.Text += MyText + Environment.NewLine ' Append control text property
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Append_ListView(ByVal MyListView As ListView, ByVal MyArray As ArrayList)
        If MyListView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_Patch_ListView(AddressOf Delegate_Sub_Append_ListView)
            Invoke(MyDelegate, MyListView, MyArray)
        Else
            If MyArray.Count = 0 Then Return
            Dim MyListViewItem = MyListView.Items.Add(MyArray.Item(0).ToString)
            If MyArray.Count > 1 Then
                For i As Integer = 1 To MyArray.Count - 1
                    If MyArray.Item(i) Is Nothing Then Return
                    MyListViewItem.SubItems.Add(MyArray.Item(i).ToString)
                Next
            End If
            For Each column As ColumnHeader In MyListView.Columns
                column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                column.Width += 50
            Next
        End If
    End Sub

    Public Sub Delegate_Sub_Set_ListBox(ByVal MyListBox As ListBox, ByVal MyArray As ArrayList)
        If MyListBox.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_ListBox(AddressOf Delegate_Sub_Set_ListBox)
            Invoke(MyDelegate, MyListBox, MyArray)
        Else
            MyListBox.Items.Clear()
            If Not MyArray Is Nothing Then
                For Each NewItem As String In MyArray
                    MyListBox.Items.Add(NewItem)
                Next
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Set_Patch_ListView(ByVal MyListView As ListView, ByVal MyArray As ArrayList)
        If MyListView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_Patch_ListView(AddressOf Delegate_Sub_Set_Patch_ListView)
            Invoke(MyDelegate, MyListView, MyArray)
        Else
            If MyArray.Count = 0 Then Return
            Dim MyListViewItem = MyListView.Items.Add(MyArray.Item(0).ToString)
            If MyArray.Count > 1 Then
                For i As Integer = 1 To MyArray.Count - 1
                    MyListViewItem.SubItems.Add(MyArray.Item(i).ToString)
                Next
            End If
            ColumnPatch.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            ColumnCode.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            ColumnStatus.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            ColumnPatch.Width += 50
            ColumnCode.Width += 50
            ColumnStatus.Width += 50
        End If
    End Sub

    Public Sub Delegate_Sub_Set_Active_Tab(ByVal MyTabControl As TabControl, ByVal MyTabPage As TabPage)
        If MyTabControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_Active_Tab(AddressOf Delegate_Sub_Set_Active_Tab)
            Invoke(MyDelegate, MyTabControl, MyTabPage)
        Else
            MyTabControl.SelectedTab = MyTabPage
        End If
    End Sub

    Public Sub Delegate_Sub_Set_DataGrid_Column_ReadOnly(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal isReadOnly As Boolean)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_DataGrid_Column_ReadOnly(AddressOf Delegate_Sub_Set_DataGrid_Column_ReadOnly)
            Invoke(MyDelegate, MyDataGridView, ColumnName, isReadOnly)
        Else
            If MyDataGridView.Columns.Contains(ColumnName) Then
                MyDataGridView.Columns(ColumnName).ReadOnly = isReadOnly
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Set_DataGrid_Fill_Column(ByVal MyDataGridView As DataGridView, ByVal FillColumn As String)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_DataGrid_Fill_Column(AddressOf Delegate_Sub_Set_DataGrid_Fill_Column)
            Invoke(MyDelegate, MyDataGridView, FillColumn)
        Else
            If MyDataGridView.Columns.Contains(FillColumn) Then
                MyDataGridView.Columns(FillColumn).MinimumWidth = 150
                MyDataGridView.Columns(FillColumn).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_UnSet_DataGrid_Fill_Column(ByVal MyDataGridView As DataGridView, ByVal FillColumn As String)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Set_DataGrid_Fill_Column(AddressOf Delegate_Sub_UnSet_DataGrid_Fill_Column)
            Invoke(MyDelegate, MyDataGridView, FillColumn)
        Else
            If MyDataGridView.Columns.Contains(FillColumn) Then
                MyDataGridView.Columns(FillColumn).MinimumWidth = 150
                MyDataGridView.Columns(FillColumn).AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet
            End If
        End If
    End Sub

    Public Sub Delegate_Select_Button(ByVal MyButton As Button)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_Button(AddressOf Delegate_Select_Button)
            Invoke(MyDelegate, MyButton)
        Else
            MyButton.Select()
        End If
    End Sub

    Public Sub Delegate_Sub_Clear_ListView(ByVal MyListView As ListView)
        If MyListView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_ListView(AddressOf Delegate_Sub_Clear_ListView)
            Invoke(MyDelegate, MyListView)
        Else
            MyListView.Items.Clear()
        End If
    End Sub

    Public Sub Delegate_Sub_Clear_ListBox(ByVal MyListBox As ListBox)
        If MyListBox.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_ListBox(AddressOf Delegate_Sub_Clear_ListBox)
            Invoke(MyDelegate, MyListBox)
        Else
            MyListBox.Items.Clear()
        End If
    End Sub

    Public Sub Delegate_Sub_Begin_Update_ListView(ByVal MyListView As ListView)
        If MyListView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_ListView(AddressOf Delegate_Sub_Begin_Update_ListView)
            Invoke(MyDelegate, MyListView)
        Else
            MyListView.BeginUpdate() ' BeginUpdate -- prevent control from redrawing
        End If
    End Sub

    Public Sub Delegate_Sub_End_Update_ListView(ByVal MyListView As ListView)
        If MyListView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_ListView(AddressOf Delegate_Sub_End_Update_ListView)
            Invoke(MyDelegate, MyListView)
        Else
            MyListView.EndUpdate() ' EndUpdate -- resume control redraw
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Blue_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Blue_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.SteelBlue
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Green_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Green_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.DarkSeaGreen
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Red_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Red_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.IndianRed
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Yellow_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Yellow_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.Khaki
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Tan_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Tan_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.Tan
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_CadetBlue_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_CadetBlue_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.CadetBlue
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_LightCoral_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_LightCoral_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.LightCoral
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Peru_Button(ByVal MyButton As Button, ByVal IsEnabled As Boolean)
        If MyButton.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable_Control(AddressOf Delegate_Sub_Enable_Peru_Button)
            Invoke(MyDelegate, MyButton, IsEnabled)
        Else
            If IsEnabled Then
                MyButton.Enabled = True
                MyButton.BackColor = Drawing.Color.Peru
            Else
                MyButton.Enabled = False
                MyButton.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Start_Button(ByVal IsEnabled As Boolean)
        If btnWinOfflineStart1.InvokeRequired Then
            Dim MyDelegate As New Delegate_Enable(AddressOf Delegate_Sub_Enable_Start_Button)
            Invoke(MyDelegate, IsEnabled)
        Else
            If IsEnabled Then
                btnWinOfflineStart1.Enabled = True
                btnWinOfflineStart1.Text = "Start"
                btnWinOfflineStart1.BackColor = Drawing.Color.DarkSeaGreen
            Else
                btnWinOfflineStart1.Enabled = False
                btnWinOfflineStart1.Text = "Patch Error"
                btnWinOfflineStart1.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Enable_Remove_Start_Button(ByVal IsEnabled As Boolean)
        If btnWinOfflineStart2.InvokeRequired Then
            Dim MyStartButtonDelegate As New Delegate_Enable(AddressOf Delegate_Sub_Enable_Remove_Start_Button)
            Invoke(MyStartButtonDelegate, IsEnabled)
        Else
            If IsEnabled Then
                btnWinOfflineStart2.Enabled = True
                btnWinOfflineStart2.Text = "Start"
                btnWinOfflineStart2.BackColor = Drawing.Color.SteelBlue
            Else
                btnWinOfflineStart2.Enabled = False
                btnWinOfflineStart2.Text = "No items selected"
                btnWinOfflineStart2.BackColor = Drawing.Color.Gray
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Hide_DataGridView_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Hide_DataGridView_Column(AddressOf Delegate_Sub_Hide_DataGridView_Column)
            Invoke(MyDelegate, MyDataGridView, ColumnName)
        Else
            If MyDataGridView.Columns.Contains(ColumnName) Then
                MyDataGridView.Columns(ColumnName).Visible = False
            End If
        End If
    End Sub

    Public Sub Delegate_Sub_Insert_DataGridView_ComboBoxColumn(ByVal MyDataGridView As DataGridView,
                                                               ByVal ColumnIndex As Integer,
                                                               ByVal ColumnName As String,
                                                               ByVal HeaderText As String)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Insert_DataGridView_Column(AddressOf Delegate_Sub_Insert_DataGridView_ComboBoxColumn)
            Invoke(MyDelegate, MyDataGridView, ColumnIndex, ColumnName, HeaderText)
        Else
            Dim MyNewColumn As New DataGridViewComboBoxColumn()
            MyNewColumn.Name = ColumnName
            MyNewColumn.HeaderText = HeaderText
            MyNewColumn.AutoSizeMode = MyDataGridView.AutoSizeColumnsMode
            MyDataGridView.Columns.Insert(ColumnIndex, MyNewColumn)
        End If
    End Sub

    Public Sub Delegate_Sub_Add_DataGridView_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal HeaderText As String)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Add_DataGridView_Column(AddressOf Delegate_Sub_Add_DataGridView_Column)
            Invoke(MyDelegate, MyDataGridView, ColumnName, HeaderText)
        Else
            MyDataGridView.Columns.Add(ColumnName, HeaderText)
        End If
    End Sub

    Public Sub Delegate_Sub_Add_DataGridView_Row(ByVal MyDataGridView As DataGridView, ByVal RowArray As Object())
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Add_DataGridView_Row(AddressOf Delegate_Sub_Add_DataGridView_Row)
            Invoke(MyDelegate, MyDataGridView, RowArray)
        Else
            MyDataGridView.Rows.Add(RowArray)
        End If
    End Sub

    Public Sub Delegate_Sub_Remove_DataGridView_Row(ByVal MyDataGridView As DataGridView, ByVal RowIndex As Integer)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Remove_DataGridView_Row(AddressOf Delegate_Sub_Remove_DataGridView_Row)
            Invoke(MyDelegate, MyDataGridView, RowIndex)
        Else
            MyDataGridView.Rows.RemoveAt(RowIndex)
        End If
    End Sub

    Public Sub Delegate_Sub_Update_DataGridView_Value(ByVal MyDataGridView As DataGridView, ByVal RowIndex As Integer, ByVal ColName As String, ByVal NewValue As Object)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Update_DataGridView_Value(AddressOf Delegate_Sub_Update_DataGridView_Value)
            Invoke(MyDelegate, MyDataGridView, RowIndex, ColName, NewValue)
        Else
            MyDataGridView.Rows(RowIndex).Cells(ColName).Value = NewValue
        End If
    End Sub

    Public Sub Delegate_Sub_Clear_DataGridView(ByVal MyDataGridView As DataGridView)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_DataGridView(AddressOf Delegate_Sub_Clear_DataGridView)
            Invoke(MyDelegate, MyDataGridView)
        Else
            MyDataGridView.Rows.Clear()
            MyDataGridView.Columns.Clear()
        End If
    End Sub

    Public Sub Delegate_Sub_Clear_DataGridView_Rows(ByVal MyDataGridView As DataGridView)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Modify_DataGridView(AddressOf Delegate_Sub_Clear_DataGridView)
            Invoke(MyDelegate, MyDataGridView)
        Else
            MyDataGridView.Rows.Clear()
        End If
    End Sub

    Public Sub Delegate_Sub_Add_Control(ByVal ParentControl As Control, ByVal ChildControl As Control)
        If ParentControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Add_Control(AddressOf Delegate_Sub_Add_Control)
            Invoke(MyDelegate, ParentControl, ChildControl)
        Else
            ParentControl.Controls.Add(ChildControl)
        End If
    End Sub

    Public Sub Delegate_Sub_Remove_Child_Controls(ByVal MyControl As Control)
        If MyControl.InvokeRequired Then
            Dim MyDelegate As New Delegate_Remove_Child_Controls(AddressOf Delegate_Sub_Remove_Child_Controls)
            Invoke(MyDelegate, MyControl)
        Else
            MyControl.Controls.Clear()
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal ColumnSize As Integer)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Resize_DataGrid_ColumnName(AddressOf Delegate_Sub_Resize_DataGrid_Column)
            Invoke(MyDelegate, MyDataGridView, ColumnName, ColumnSize)
        Else
            If MyDataGridView.Columns.Contains(ColumnName) Then
                MyDataGridView.Columns(ColumnName).Width = ColumnSize
            End If
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnIndex As Integer, ByVal ColumnSize As Integer)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Resize_DataGrid_ColumnIndex(AddressOf Delegate_Sub_Resize_DataGrid_Column)
            Invoke(MyDelegate, MyDataGridView, ColumnIndex, ColumnSize)
        Else
            If MyDataGridView.Columns.Contains(ColumnIndex) Then
                MyDataGridView.Columns(ColumnIndex).Width = ColumnSize
            End If
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnName As String, ByVal ResizeMode As DataGridViewAutoSizeColumnsMode)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_ResizeMode_DataGrid_ColumnName(AddressOf Delegate_Sub_Resize_DataGrid_Column)
            Invoke(MyDelegate, MyDataGridView, ColumnName, ResizeMode)
        Else
            If MyDataGridView.Columns.Contains(ColumnName) Then
                MyDataGridView.Columns(ColumnName).AutoSizeMode = ResizeMode
            End If
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Column(ByVal MyDataGridView As DataGridView, ByVal ColumnIndex As Integer, ByVal ResizeMode As DataGridViewAutoSizeColumnsMode)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_ResizeMode_DataGrid_ColumnIndex(AddressOf Delegate_Sub_Resize_DataGrid_Column)
            Invoke(MyDelegate, MyDataGridView, ColumnIndex, ResizeMode)
        Else
            If MyDataGridView.Columns.Contains(ColumnIndex) Then
                MyDataGridView.Columns(ColumnIndex).AutoSizeMode = ResizeMode
            End If
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Columns(ByVal MyDataGridView As DataGridView, ByVal ResizeMode As DataGridViewAutoSizeColumnsMode)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Resize_DataGrid_Columns(AddressOf Delegate_Sub_Resize_DataGrid_Columns)
            Invoke(MyDelegate, MyDataGridView, ResizeMode)
        Else
            If Not ResizeMode = 0 Then
                MyDataGridView.AutoResizeColumns(ResizeMode)
            End If
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Rows(ByVal MyDataGridView As DataGridView, ByVal ResizeMode As DataGridViewAutoSizeRowsMode)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Resize_DataGrid_Rows(AddressOf Delegate_Sub_Resize_DataGrid_Rows)
            Invoke(MyDelegate, MyDataGridView, ResizeMode)
        Else
            If Not ResizeMode = 0 Then
                MyDataGridView.AutoResizeRows(ResizeMode)
            End If
        End If
    End Sub

    Private Sub Delegate_Sub_Resize_DataGrid_Row_Template_Height(ByVal MyDataGridView As DataGridView, ByVal Height As Integer)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Resize_DataGrid_Row_Template_Height(AddressOf Delegate_Sub_Resize_DataGrid_Row_Template_Height)
            Invoke(MyDelegate, MyDataGridView, Height)
        Else
            MyDataGridView.RowTemplate.Height = Height
        End If
    End Sub

    Private Sub Delegate_Sub_Refresh_DataGrid(ByVal MyDataGridView As DataGridView)
        If MyDataGridView.InvokeRequired Then
            Dim MyDelegate As New Delegate_Refresh_DataGrid(AddressOf Delegate_Sub_Refresh_DataGrid)
            Invoke(MyDelegate, MyDataGridView)
        Else
            MyDataGridView.Refresh()
        End If
    End Sub

    Public Function Callback_Function_Read_Text(ByVal MyControl As Control) As String
        If MyControl.InvokeRequired Then
            Dim MyDelegate As New Callback_Read_Text(AddressOf Callback_Function_Read_Text)
            Return Invoke(MyDelegate, MyControl)
        Else
            Return MyControl.Text
        End If
    End Function

    Public Function Callback_Function_Read_Selected_Text(ByVal MyRichTextBox As RichTextBox) As String
        If MyRichTextBox.InvokeRequired Then
            Dim MyDelegate As New Callback_Read_Selected_Text(AddressOf Callback_Function_Read_Selected_Text)
            Return Invoke(MyDelegate, MyRichTextBox)
        Else
            If MyRichTextBox.SelectedText.Equals("") Then
                Return MyRichTextBox.Text
            Else
                Return MyRichTextBox.SelectedText
            End If
        End If
    End Function

    Public Function Callback_Function_Read_Selected_TreeNode(ByVal MyTreeView As TreeView) As String
        If MyTreeView.InvokeRequired Then
            Dim MyDelegate As New Callback_Read_Selected_TreeNode(AddressOf Callback_Function_Read_Selected_TreeNode)
            Return Invoke(MyDelegate, MyTreeView)
        Else
            Return MyTreeView.SelectedNode.Name
        End If
    End Function

End Class
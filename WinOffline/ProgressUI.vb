Imports System.Windows.Forms

Public Class ProgressUI

    Private FormTitle As String
    Private AutoCloseTime As Integer = 0
    Private SimpleContextMenu As New ContextMenu()
    Private CopyItem As New MenuItem("&Copy")
    Private AutoScrollBuffer As New ArrayList
    Private Delegate Sub SetCurrentTaskCallback(ByVal CurrentTask As String)
    Private Delegate Sub SetDebugTextCallback(ByVal Message As String)
    Private Delegate Sub CloseFormCallback()
    Private Delegate Sub HideFormCallback(ByVal AutoClose As Boolean)

    Private Sub ProgressUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SimpleContextMenu.MenuItems.Add(CopyItem)
        rtbProgress.ContextMenu = SimpleContextMenu
        AddHandler CopyItem.Click, AddressOf CopyItem_Click
        CopyItem.Enabled = False ' Disable copy item (until text is selected)
    End Sub

    Public Sub Delegate_ProgressUI_Close()
        If IsHandleCreated AndAlso InvokeRequired Then
            Dim MyDelegate As New CloseFormCallback(AddressOf Delegate_ProgressUI_Close)
            Invoke(MyDelegate)
        Else
            Close()
        End If
    End Sub

    Private Sub ProgressUI_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed
        TrayIcon.Dispose()
    End Sub

    Public Sub Delegate_ProgressUI_GoModeless(ByVal AutoClose As Boolean)
        If IsHandleCreated AndAlso InvokeRequired Then
            Dim MyDelegate As New HideFormCallback(AddressOf Delegate_ProgressUI_GoModeless)
            Invoke(MyDelegate, AutoClose)
        Else
            If AutoClose Then ' Check for autoclose option [TRUE = Autoclose based on Timer, FALSE = Hide immediately]
                AutoCloseTimer.Start()
            Else
                Hide()
                TrayIcon.Dispose()
            End If
        End If
    End Sub

    Private Sub AutoCloseTimer_Tick(sender As System.Object, e As System.EventArgs) Handles AutoCloseTimer.Tick
        If AutoCloseTime >= 10 Then
            AutoCloseTimer.Stop()
            Delegate_ProgressUI_Close()
        End If
        AutoCloseTime += 1
        Text = FormTitle + " -- Autoclosing in " + (11 - AutoCloseTime).ToString + " seconds. Click to hold open."
    End Sub

    Public Sub Delegate_ProgressUI_UpdateCurrentTask(ByVal CurrentTask As String)
        If lblCurTask.InvokeRequired Then
            Dim MyDelegate As New SetCurrentTaskCallback(AddressOf Delegate_ProgressUI_UpdateCurrentTask)
            lblCurTask.Invoke(MyDelegate, CurrentTask)
        Else
            lblCurTask.Text = CurrentTask
            TrayIcon.Text = "CA Maintenance:  " + CurrentTask
        End If
    End Sub

    Public Sub Delegate_ProgressUI_WriteDebug(ByVal Message As String)
        If IsHandleCreated AndAlso rtbProgress.InvokeRequired AndAlso rtbProgress.IsHandleCreated Then
            Dim MyDelegate As New SetDebugTextCallback(AddressOf Delegate_ProgressUI_WriteDebug)
            rtbProgress.Invoke(MyDelegate, Message)
        Else
            Dim FilterMessage As String = Message
            If FilterMessage = Nothing Then
                Message = ""
            Else
                While FilterMessage.Contains("|")
                    Message = FilterMessage
                    FilterMessage = FilterMessage.Substring(FilterMessage.IndexOf("|") + 1)
                End While
            End If
            If chkTailProgress.Checked Then ' Follow debug tail?
                If rtbProgress.IsHandleCreated Then
                    rtbProgress.Invoke(New Action(Of String)(AddressOf rtbProgress.AppendText), Message + Environment.NewLine)
                Else
                    rtbProgress.Text += Message + Environment.NewLine
                End If
                rtbProgress.Focus()
            Else
                AutoScrollBuffer.Add(Message + Environment.NewLine)
            End If
        End If

    End Sub

    Public Sub SetFormTitle(ByVal Title As String)
        FormTitle = Title
        Text = Title
    End Sub

    Private Sub rtbProgress_MouseClick(sender As System.Object, e As System.EventArgs) Handles rtbProgress.MouseClick
        If AutoCloseTimer.Enabled = True Then
            AutoCloseTimer.Stop()
            Text = FormTitle + " -- Autoclose cancelled -- Close window when you are finished."
        End If
    End Sub

    Private Sub rtbProgress_VScroll(sender As System.Object, e As System.EventArgs) Handles rtbProgress.VScroll
        If AutoCloseTimer.Enabled Then
            AutoCloseTimer.Stop()
            Text = FormTitle + " -- Autoclose cancelled -- Close window when you are finished."
        End If
    End Sub

    Private Sub rtbProgress_HScroll(sender As System.Object, e As System.EventArgs) Handles rtbProgress.HScroll
        If AutoCloseTimer.Enabled = True Then
            AutoCloseTimer.Stop()
            Text = FormTitle + " -- Autoclose cancelled -- Close window when you are finished."
        End If
    End Sub

    Private Sub rtbProgress_SelectionChanged(sender As Object, e As EventArgs) Handles rtbProgress.SelectionChanged
        If rtbProgress.SelectedText IsNot Nothing AndAlso Not rtbProgress.SelectedText.Equals("") Then
            CopyItem.Enabled = True
        Else
            CopyItem.Enabled = False
        End If
    End Sub

    Private Sub CopyItem_Click(sender As Object, e As EventArgs)
        rtbProgress.Copy()
    End Sub

    Private Sub chkTailProgress_CheckedChanged(sender As Object, e As EventArgs) Handles chkTailProgress.CheckedChanged
        If chkTailProgress.Checked Then
            rtbProgress.Focus()
            If rtbProgress.IsHandleCreated Then
                For Each LineItem As String In AutoScrollBuffer
                    rtbProgress.Invoke(New Action(Of String)(AddressOf rtbProgress.AppendText), LineItem)
                Next
                AutoScrollBuffer.Clear()
            Else
                For Each LineItem As String In AutoScrollBuffer
                    rtbProgress.AppendText(LineItem)
                Next
                AutoScrollBuffer.Clear()
            End If
        End If
    End Sub

    Public Sub DoEvents()
        Application.DoEvents()
    End Sub

End Class
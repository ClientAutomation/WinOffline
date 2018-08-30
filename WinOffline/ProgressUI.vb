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

        ' Add the menu items
        SimpleContextMenu.MenuItems.Add(CopyItem)

        ' Assign the context menu
        rtbProgress.ContextMenu = SimpleContextMenu

        ' Add event handler
        AddHandler CopyItem.Click, AddressOf CopyItem_Click

        ' Disable copy item (until text is selected)
        CopyItem.Enabled = False

    End Sub

    Public Sub Delegate_ProgressUI_Close()

        ' Check if calling thread owns the control, otherwise invoke the GUI update.
        If IsHandleCreated AndAlso InvokeRequired Then

            ' Create an invoke delegate on thread owning the control
            Dim MyDelegate As New CloseFormCallback(AddressOf Delegate_ProgressUI_Close)
            Invoke(MyDelegate)

        Else

            ' Close the form
            Close()

        End If

    End Sub

    Private Sub ProgressUI_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed

        ' Dispose tray icon
        TrayIcon.Dispose()

    End Sub

    Public Sub Delegate_ProgressUI_GoModeless(ByVal AutoClose As Boolean)

        ' Check if calling thread owns the control, otherwise invoke the GUI update.
        If IsHandleCreated AndAlso InvokeRequired Then

            ' Create an invoke delegate on thread owning the control
            Dim MyDelegate As New HideFormCallback(AddressOf Delegate_ProgressUI_GoModeless)
            Invoke(MyDelegate, AutoClose)

        Else

            ' Check for autoclose option [TRUE = Autoclose based on Timer, FALSE = Hide immediately]
            If AutoClose Then

                ' Start autoclose timer
                AutoCloseTimer.Start()

            Else

                ' Hide the form
                Hide()

                ' Dispose tray icon
                TrayIcon.Dispose()

            End If

        End If

    End Sub

    Private Sub AutoCloseTimer_Tick(sender As System.Object, e As System.EventArgs) Handles AutoCloseTimer.Tick

        ' Check the interval
        If AutoCloseTime >= 10 Then

            ' Stop the timer
            AutoCloseTimer.Stop()

            ' Close the form
            Delegate_ProgressUI_Close()

        End If

        ' Increment the time
        AutoCloseTime += 1

        ' Update the titlebar
        Text = FormTitle + " -- Autoclosing in " + (11 - AutoCloseTime).ToString + " seconds. Click to hold open."

    End Sub

    Public Sub Delegate_ProgressUI_UpdateCurrentTask(ByVal CurrentTask As String)

        ' Check if calling thread owns the control, otherwise invoke the GUI update.
        If lblCurTask.InvokeRequired Then

            ' Create an invoke delegate on thread owning the control
            Dim MyDelegate As New SetCurrentTaskCallback(AddressOf Delegate_ProgressUI_UpdateCurrentTask)
            lblCurTask.Invoke(MyDelegate, CurrentTask)

        Else

            ' Update current task label
            lblCurTask.Text = CurrentTask

            ' Set tray icon tool tip
            TrayIcon.Text = "CA Maintenance:  " + CurrentTask

        End If

    End Sub

    Public Sub Delegate_ProgressUI_WriteDebug(ByVal Message As String)

        ' Check if calling thread owns the control, otherwise invoke the GUI update.
        If IsHandleCreated AndAlso rtbProgress.InvokeRequired AndAlso rtbProgress.IsHandleCreated Then

            ' Create an invoke delegate on thread owning the control
            Dim MyDelegate As New SetDebugTextCallback(AddressOf Delegate_ProgressUI_WriteDebug)
            rtbProgress.Invoke(MyDelegate, Message)

        Else

            ' Create a filtered message
            Dim FilterMessage As String = Message

            ' Check for empty debug string
            If FilterMessage = Nothing Then

                ' Stub an empty message
                Message = ""

            Else

                ' Filter the CallStack from the message
                While FilterMessage.Contains("|")

                    ' Update the message
                    Message = FilterMessage

                    ' Do more filtering
                    FilterMessage = FilterMessage.Substring(FilterMessage.IndexOf("|") + 1)

                End While

            End If

            ' Follow debug tail?
            If chkTailProgress.Checked Then

                ' Check if the rtbDebug handle is created
                If rtbProgress.IsHandleCreated Then

                    ' Invoke rtbDebug.AppendText() to update the UI and autoscroll
                    rtbProgress.Invoke(New Action(Of String)(AddressOf rtbProgress.AppendText), Message + Environment.NewLine)

                Else

                    ' Use the old fashion method
                    rtbProgress.Text += Message + Environment.NewLine

                End If

                ' Focus the textbox for autoscroll when focus is lost
                rtbProgress.Focus()

            Else

                ' Append debug to buffer
                AutoScrollBuffer.Add(Message + Environment.NewLine)

            End If

        End If

    End Sub

    Public Sub SetFormTitle(ByVal Title As String)

        ' Update title variable
        FormTitle = Title

        ' Set form title text
        Text = Title

    End Sub

    Private Sub rtbProgress_MouseClick(sender As System.Object, e As System.EventArgs) Handles rtbProgress.MouseClick

        ' Check for active autoclose timer
        If AutoCloseTimer.Enabled = True Then

            ' Stop the timer
            AutoCloseTimer.Stop()

            ' Reset form title
            Text = FormTitle + " -- Autoclose cancelled -- Close window when you are finished."

        End If

    End Sub

    Private Sub rtbProgress_VScroll(sender As System.Object, e As System.EventArgs) Handles rtbProgress.VScroll

        ' Check for active autoclose timer
        If AutoCloseTimer.Enabled Then

            ' Stop the timer
            AutoCloseTimer.Stop()

            ' Reset form title
            Text = FormTitle + " -- Autoclose cancelled -- Close window when you are finished."

        End If

    End Sub

    Private Sub rtbProgress_HScroll(sender As System.Object, e As System.EventArgs) Handles rtbProgress.HScroll

        ' Check for active autoclose timer
        If AutoCloseTimer.Enabled = True Then

            ' Stop the timer
            AutoCloseTimer.Stop()

            ' Reset form title
            Text = FormTitle + " -- Autoclose cancelled -- Close window when you are finished."

        End If

    End Sub

    Private Sub rtbProgress_SelectionChanged(sender As Object, e As EventArgs) Handles rtbProgress.SelectionChanged

        ' Verify text is selected
        If rtbProgress.SelectedText IsNot Nothing AndAlso Not rtbProgress.SelectedText.Equals("") Then

            ' Enable copy
            CopyItem.Enabled = True

        Else

            ' Disable copy
            CopyItem.Enabled = False

        End If

    End Sub

    Private Sub CopyItem_Click(sender As Object, e As EventArgs)

        ' Copy selected text to clipboard
        rtbProgress.Copy()

    End Sub

    Private Sub chkTailProgress_CheckedChanged(sender As Object, e As EventArgs) Handles chkTailProgress.CheckedChanged

        ' Purge the autoscroll buffer
        If chkTailProgress.Checked Then

            ' Apply focus
            rtbProgress.Focus()

            ' Check if the rtbDebug handle is created
            If rtbProgress.IsHandleCreated Then

                ' Purge contents of buffer
                For Each LineItem As String In AutoScrollBuffer

                    ' Write buffer -- invoke to window handle
                    rtbProgress.Invoke(New Action(Of String)(AddressOf rtbProgress.AppendText), LineItem)

                Next

                ' Clear AutoScrollBuffer
                AutoScrollBuffer.Clear()

            Else

                ' Purge contents of buffer
                For Each LineItem As String In AutoScrollBuffer

                    ' Write buffer -- append text normally
                    rtbProgress.AppendText(LineItem)

                Next

                ' Clear AutoScrollBuffer
                AutoScrollBuffer.Clear()

            End If

        End If

    End Sub

    Public Sub DoEvents()

        ' Process message queue
        Application.DoEvents()

    End Sub

End Class
Public Class HelpUI

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        Me.Close()
    End Sub

    Private Sub OptionsGui_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Dim rtbText As New ArrayList

        rtbHelp.Text = WinOffline.Utility.ReplaceWord(rtbHelp.Text, "WinOffline", Globals.ProcessFriendlyName)

        For Each line As String In rtbHelp.Lines
            rtbText.Add(line)
        Next

        rtbHelp.Text = ""

        For Each line As String In rtbText
            rtbHelp.AppendText(line + Environment.NewLine)
            If line.Contains(Globals.ProcessFriendlyName + " Command Line Switches:") Or
                line.Contains(Globals.ProcessFriendlyName + " Software Delivery and Scripting Switches:") Or
                line.Contains(Globals.ProcessFriendlyName + " Database Maintenance Switches:") Then
                rtbHelp.SelectionStart = rtbHelp.Text.LastIndexOf(line)
                rtbHelp.SelectionLength = line.Length
                rtbHelp.SelectionFont = New Drawing.Font("Calibri", 12, Drawing.FontStyle.Underline Or Drawing.FontStyle.Bold)
            End If
        Next

        rtbHelp.SelectionStart = 0
        rtbHelp.SelectionLength = 0

    End Sub

End Class
Imports System.Drawing
Imports System.Windows.Forms

Public Class AlertBox

    Private FormTitle As String
    Private AutoCloseCounter As Integer = 0
    Private AutoCloseTimeout As Integer = 0

    Public Shared Sub CreateUserAlert(ByVal Message As String,
                                      Optional ByVal Timeout As Integer = 0,
                                      Optional ByVal TextJustification As HorizontalAlignment = HorizontalAlignment.Center,
                                      Optional ByVal WordWrap As Boolean = True,
                                      Optional ByVal FontSize As Integer = 11,
                                      Optional ByVal AutoResizetoText As Boolean = False)
        Dim alert As New AlertBox()
        alert.Text = Globals.ProcessFriendlyName + " " + Globals.AppVersion
        alert.FormTitle = alert.Text
        alert.AlertText.Text = Message
        alert.AlertText.TextAlign = TextJustification
        alert.AlertText.WordWrap = WordWrap
        alert.AlertText.Font = New Font(alert.AlertText.Font.Name, FontSize, FontStyle.Regular)

        If AutoResizetoText Then
            Dim Size As Size = TextRenderer.MeasureText(alert.AlertText.Text, alert.AlertText.Font)
            alert.AutoSize = True
            alert.AutoSizeMode = AutoSizeMode.GrowAndShrink
            alert.AlertText.Width = Size.Width
            alert.btnOK.Location = New Point((alert.Width - alert.btnOK.Width) \ 2, alert.btnOK.Location.Y)
        End If

        If Timeout <> 0 Then
            alert.AutoCloseTimeout = Timeout
            alert.Timer1.Start()
        End If

        alert.ShowDialog()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If AutoCloseCounter >= AutoCloseTimeout Then
            Timer1.Stop()
            Me.Close()
        End If
        AutoCloseCounter += 1
        Text = FormTitle + " -- " + ((AutoCloseTimeout + 1) - AutoCloseCounter).ToString + " seconds."
    End Sub

    Private Sub OkayButton_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        Close()
    End Sub

End Class
Public NotInheritable Class AboutBox

    Private Sub AboutBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Text = String.Format("About {0}", Globals.ProcessFriendlyName)
        LabelProductName.Text = Globals.ProcessFriendlyName
        LabelVersion.Text = String.Format("Version {0}", Globals.AppVersion)
        LabelCopyright.Text = "Copyright " + Globals.AppVersion.Substring(0, Globals.AppVersion.IndexOf("."))
        LabelCompanyName.Text = "CA Technologies, Inc."
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Close()
    End Sub

End Class
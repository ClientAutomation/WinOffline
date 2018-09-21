Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing

Partial Public Class WinOfflineUI

    Private Sub InitSDHelpPanel()

        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim myStream As Stream
        Dim image As Bitmap
        Dim PrevImage As Bitmap
        Dim NextImage As Bitmap

        ' Set first image for software delivery help
        myStream = myAssembly.GetManifestResourceStream("WinOffline.Step" + SDHelpStepCounter.ToString + ".png")
        Image = New Bitmap(myStream)
        ScaleImageToPictureBox(picSteps, Image)
        picSteps.Image = Image
        picSteps.SizeMode = PictureBoxSizeMode.CenterImage

        ' Disable the previous button
        btnWinOfflineSDHelpPrevious.Enabled = False

        ' Set previous button image
        myStream = myAssembly.GetManifestResourceStream("WinOffline.Prev.png")
        PrevImage = New Bitmap(myStream)
        ScaleImageToButton(btnWinOfflineSDHelpPrevious, PrevImage)
        btnWinOfflineSDHelpPrevious.Image = PrevImage

        ' Set next button image
        myStream = myAssembly.GetManifestResourceStream("WinOffline.Next.png")
        NextImage = New Bitmap(myStream)
        ScaleImageToButton(btnWinOfflineSDHelpPrevious, NextImage)
        btnWinOfflineSDHelpNext.Image = NextImage

        ' Load corresponding help text
        SetStepText(SDHelpStepCounter)

    End Sub

End Class
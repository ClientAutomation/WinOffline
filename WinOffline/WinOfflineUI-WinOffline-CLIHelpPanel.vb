Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing

Partial Public Class WinOfflineUI

    Private Sub InitCLISwitchPanel()

        Dim rtbText As New ArrayList
        rtbOptions.Text = WinOffline.Utility.ReplaceWord(rtbOptions.Text, "WinOffline", Globals.ProcessFriendlyName)

        For Each line As String In rtbOptions.Lines
            rtbText.Add(line)
        Next

        rtbOptions.Text = ""

        For Each line As String In rtbText
            rtbOptions.AppendText(line + Environment.NewLine)
            If line.Contains("Purpose") Or
                line.Contains("License") Or
                line.Contains("Requirements") Or
                line.Contains("Multi-Purpose Application") Or
                line.Contains("WinOffline Debug Log") Or
                line.Contains("WinOffline Exit Codes") Or
                line.Contains("Deviations from ApplyPTF functionality") Or
                line.Contains("Custom Supported JCL tags") Or
                line.Contains("JCL file requirements and formatting") Or
                line.Contains("WinOffline Command Line Switches") Or
                line.Contains("WinOffline Software Delivery and Scripting Switches") Or
                line.Contains("WinOffline Database Maintenance Switches") Then
                rtbOptions.SelectionStart = rtbOptions.Text.LastIndexOf(line)
                rtbOptions.SelectionLength = line.Length
                rtbOptions.SelectionFont = New Drawing.Font("Calibri", 12, Drawing.FontStyle.Underline Or Drawing.FontStyle.Bold)
            End If
        Next

        rtbOptions.SelectionStart = 0
        rtbOptions.SelectionLength = 0

    End Sub

    Private Sub btnWinOfflineSDHelpPrevious_Click(sender As Object, e As EventArgs) Handles btnWinOfflineSDHelpPrevious.Click

        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim myStream As Stream
        Dim image As Bitmap

        SDHelpStepCounter -= 1
        myStream = myAssembly.GetManifestResourceStream("WinOffline.Step" + SDHelpStepCounter.ToString + ".png")
        image = New Bitmap(myStream)
        ScaleImageToPictureBox(picSteps, image)
        picSteps.Image = image
        picSteps.SizeMode = PictureBoxSizeMode.CenterImage
        If SDHelpStepCounter = 1 Then btnWinOfflineSDHelpPrevious.Enabled = False
        btnWinOfflineSDHelpNext.Enabled = True
        SetStepText(SDHelpStepCounter)

        ' Enable command line switches button, if applicable to the step
        If SDHelpStepCounter = 9 Or SDHelpStepCounter = 12 Then
            btnWinOfflineSwicthes.Visible = True
        Else
            btnWinOfflineSwicthes.Visible = False
        End If

    End Sub

    Private Sub btnWinOfflineSDHelpNext_Click(sender As Object, e As EventArgs) Handles btnWinOfflineSDHelpNext.Click

        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim myStream As Stream
        Dim image As Bitmap

        SDHelpStepCounter += 1
        myStream = myAssembly.GetManifestResourceStream("WinOffline.Step" + SDHelpStepCounter.ToString + ".png")
        image = New Bitmap(myStream)
        ScaleImageToPictureBox(picSteps, image)
        picSteps.Image = image
        picSteps.SizeMode = PictureBoxSizeMode.CenterImage

        If SDHelpStepCounter = 15 Then btnWinOfflineSDHelpNext.Enabled = False
        btnWinOfflineSDHelpPrevious.Enabled = True
        SetStepText(SDHelpStepCounter)

        ' Enable command line switches button, if applicable to the step
        If SDHelpStepCounter = 9 Or SDHelpStepCounter = 12 Then
            btnWinOfflineSwicthes.Visible = True
        Else
            btnWinOfflineSwicthes.Visible = False
        End If

    End Sub

    Private Sub ScaleImageToPictureBox(ByVal p As PictureBox, ByRef i As Bitmap)
        If i.Height > p.Height Then
            Dim diff As Integer = i.Height - p.Height
            Dim Resized As Bitmap = New Bitmap(i, New Size(i.Width - diff, i.Height - diff))
            i = Resized
        End If
        If i.Width > p.Width Then
            Dim diff As Integer = i.Width - p.Width
            Dim Resized As Bitmap = New Bitmap(i, New Size(i.Width - diff, i.Height - diff))
            i = Resized
        End If
    End Sub

    Private Sub ScaleImageToButton(ByVal p As Button, ByRef i As Bitmap)
        If i.Height > p.Height Then
            Dim diff As Integer = i.Height - p.Height
            Dim Resized As Bitmap = New Bitmap(i, New Size(i.Width - diff, i.Height - diff))
            i = Resized
        End If
        If i.Width > p.Width Then
            Dim diff As Integer = i.Width - p.Width
            Dim Resized As Bitmap = New Bitmap(i, New Size(i.Width - diff, i.Height - diff))
            i = Resized
        End If
    End Sub

    Private Sub ResizeHelpImage(sender As Object, e As EventArgs) Handles picSteps.SizeChanged
        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim myStream As Stream
        Dim image As Bitmap
        If picSteps.Image IsNot Nothing Then
            myStream = myAssembly.GetManifestResourceStream("WinOffline.Step" + SDHelpStepCounter.ToString + ".png")
            image = New Bitmap(myStream)
            ScaleImageToPictureBox(picSteps, image)
            picSteps.Image = image
        End If
    End Sub

    Public Sub SetStepText(ByVal StepNumber As Integer)
        Select Case StepNumber
            Case 1
                lblStepx.Text = "Using DSM Explorer, create a new software package in the library."
            Case 2
                lblStepx.Text = "Provide a name and version for the new software package."
            Case 3
                lblStepx.Text = "Add a new volume, 'From Files..'"
            Case 4
                lblStepx.Text = "Provide a name for the new volume."
            Case 5
                lblStepx.Text = "Drag && drop all your patch files, including WinOffline.exe, into the new volume. WinOffline can process multiple .caz or .jcl files. Be sure to include any dependent files referenced by the .jcl. WinOffline is unable to process .caz files with multiple .jcl files included or .jcl files that include master image update instructions."
            Case 6
                lblStepx.Text = "Drag && drop WinOffline.exe to 'Procedures' to create an install procedure for the package."
            Case 7
                lblStepx.Text = "Open the properties of the new procedure."
            Case 8
                lblStepx.Text = "Update the name of the procedure and ensure the task is set for 'Install'. Optionally, specify the procedure as the default prcoedure for jobs."
            Case 9
                lblStepx.Text = "By default no programming switches are required. The default behavior of WinOffline is to apply the patches found in the volume. Job output will be captured automatically. Visit the 'Available Switches..' to learn more about what options are available. Optionally, add the '$#BG' job macro if you wish to hide the initial software delivery job check dialog from the end user."
            Case 10
                lblStepx.Text = "Create a new procedure based on the install procedure. We'll use the next procedure as an uninstall task."
            Case 11
                lblStepx.Text = "Update the name of the procedure and ensure the task is set for 'Uninstall'."
            Case 12
                lblStepx.Text = "As uninstalling is not the default behavior of WinOffline, add the '-backout' switch. Visit the 'Available Switches..' to learn more about what options are available. Optionally, add the '$#BG' job macro if you wish to hide the initial software delivery job check dialog from the end user."
            Case 13
                lblStepx.Text = "Seal the new software package. It's now ready to be delivered."
            Case 14
                lblStepx.Text = "During WinOffline software deployments, you'll notice this job status message. It's completely normal behavior as WinOffline informs Software Delivery it will be shutting down CAF."
            Case 15
                lblStepx.Text = "After the deployment completes, job output, including a summary of events and exceptions will be available. If any patch is not applied or removed successfully, WinOffline will return exit code 100 back to software delivery."
            Case Else
                lblStepx.Text = "This isn't right :-/"
        End Select
    End Sub

End Class
Option Strict On 'A must-have
Imports System.IO
'©2012 validati0n
'Youtube Downloader + Youtube Lib Open Source on my Github (http://github.com/validati0n)
'GUI from https://github.com/izspk/Tubenoia
'Special thanks to (Beta) Tester from Version 1.0.0.1/1.1b1/2
'Specialy Special thanks to @sunra1n_ (http://twitter.com/#/validati0n)
'Visit me on Twitter (http://twitter.com/#/validati0n)

Public Class mainfrm
    Dim video As YoutubeVideo
    Public WithEvents Webclientos As New Net.WebClient
    Dim nowhere As conmode 'To know to convert
    Dim test As Boolean
    Dim Tempfile As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\tempvideop1.flv"
    Dim Tempfile2 As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\tempsong.mp3"

    Private Sub mainfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim aaaa As System.Drawing.Color = Gpbox1.BackColor
        'TextBox1.BackColor = aaaa
    End Sub

    Private Sub GButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GButton1.Click
        Application.Exit()
    End Sub

    Private Sub GButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GButton2.Click
        Me.WindowState = FormWindowState.Minimized 'Don't know another Methode
    End Sub

    Private Sub GButton3_Click() Handles GButton3.Click 'Returning
        If GButton3.Text = "Load" Then
            TextBox1.Enabled = False
            GButton3.Text = "Unload"
            Delay(0.1)
            loadallIN()
        Else ' HERE 
            TextBox1.Enabled = True
            TextBox1.Text = ""
            GButton3.Text = "Load"
            Label8.Text = "Waiting..."
            Label2.Text = Label2.Text.Substring(0, 6)
            Label3.Text = Label3.Text.Substring(0, 10)
            Label4.Text = Label4.Text.Substring(0, 12)
            Label5.Text = Label5.Text.Substring(0, 7)
            Label6.Text = Label6.Text.Substring(0, 10)
            Label7.Text = Label7.Text.Substring(0, 7)
            GButton4.Enabled = False
        End If
    End Sub

    Public Shared Sub Delay(ByVal dblSecs As Double) 'Thanks to iH8sn0w
        Const OneSec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblWaitTil As Date
        Now.AddSeconds(OneSec)
        dblWaitTil = Now.AddSeconds(OneSec).AddSeconds(dblSecs)
        Do Until Now > dblWaitTil
            Application.DoEvents() ' Allow windows messages to be processed
        Loop
    End Sub

    Sub loadallIN() 'Load all Controls 
        Label8.Text = "Loading..."
        If TextBox1.Text.ToLower.Contains("http://") = False Then
            TextBox1.Text = "http://" & TextBox1.Text
        End If
        Label8.Text = "LookUP..."
        Delay(0.1)
        Try
            video = New YoutubeVideo(TextBox1.Text)
        Catch ex As Exception
            Label8.Text = "Error"
            MessageBox.Show("Can't lookUP..." & vbNewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GButton3_Click()
            Label8.Text = "Waiting..."
            Exit Sub
        End Try
        Label8.Text = "Setting Information..."
        Delay(0.1)
        Label2.Text = Label2.Text & video.getTitle
        Label3.Text = Label3.Text & video.getAuthor
        Label4.Text = Label4.Text & video.getUploaddate
        Label5.Text = Label5.Text & video.getLikes.ToString
        Label6.Text = Label6.Text & video.getDislikes.ToString
        Label7.Text = Label7.Text & video.getViews
        GButton4.Enabled = True
        Delay(0.1)
        Label8.Text = "Finish..."
    End Sub

    Private Sub GButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GButton4.Click
        savevideo.FileName = video.getTitle 'I'm a fcking genius
        savevideo.ShowDialog()
    End Sub

    Enum conmode
        flv
        mp3
    End Enum

    Private Sub savevideo_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles savevideo.FileOk
        Dim endf As String = getEndung(savevideo.FileName)
        If endf = ".mp3" Then 'Selecting convert mode
            nowhere = conmode.mp3
        ElseIf endf = ".webm" Then
            nowhere = conmode.flv
        End If
        Label8.Text = "Preparing Download..." 'Controls
        Delay(0.1)
        GButton3.Enabled = False
        GButton4.Enabled = False
        For Each Proc As Process In Process.GetProcesses
            If Proc.ProcessName.ToLower.Contains("ffmpeg") Then 'Saferererer....
                Proc.Kill()
            End If
        Next
        Dim i As Integer = 0

        'Maybe you need to clean your temp  Dir some times (On startup with foreach() ?)
AGAIN1:  'To go back and try again, you also are be able to use a loop()
        If IO.File.Exists(Tempfile) = True Then
            Try
                Kill(Tempfile)
            Catch ex As Exception
                Tempfile = i.ToString & Tempfile
                GoTo AGAIN1
            End Try
        End If
AGAIN2:  'Same here
        If IO.File.Exists(Tempfile2) = True Then
            Try
                Kill(Tempfile2)
            Catch ex As Exception
                Tempfile2 = i.ToString & Tempfile2
                GoTo again2
            End Try
        End If
        Webclientos.DownloadFileAsync(New Uri(video.getDownloadlink), Tempfile) 'Let's goo...
        Label8.Text = "Downloading..."
    End Sub

    Sub File_Complete() Handles Webclientos.DownloadFileCompleted
        Label8.Text = "Download complete..."
        ProgressbarWithPercentage1.Value = 0
        Delay(0.1)
        If nowhere = conmode.flv Then 'Only FLV ->Copy 
            Label8.Text = "Copy..."
            Delay(0.1)
            FileCopy(Tempfile, savevideo.FileName)
            Dim i As Integer = MessageBox.Show("Download finished. Would you like to open it ?", "Download", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If i = 6 Then
                Process.Start(savevideo.FileName)
            End If
        Else 'CONVERT !!!
            Label8.Text = "Extracting Resource..."
            Delay(0.1)
            Dim binaryw As New BinaryWriter(New FileStream("ffmpeg.exe", FileMode.Create))
            binaryw.Write(My.Resources.ffmpeg)
            binaryw.Close()
            Label8.Text = "Write Batch..."
            Delay(0.1)
            Dim fs As New IO.StreamWriter("test.bat", False) 'Maybe you think WTF ? but this methode is really good :)
            fs.WriteLine("ffmpeg.exe -i " & Chr(34) & Tempfile & Chr(34) & _
                         " -ab 160000 -acodec libmp3lame " & Chr(34) & _
                        Tempfile2 & Chr(34)) 'To convert
            fs.WriteLine("del " + Chr(34) + "ffmpeg.exe" + Chr(34)) 'Delete resource ffmpeg
            fs.WriteLine("del " + Chr(34) + "video.flv" + Chr(34)) 'Delete old video
            fs.WriteLine("del " + Chr(34) + "test.bat" + Chr(34)) 'Delete itself
            fs.Close()
            Label8.Text = "Starting Converting..."
            Delay(0.1)
            Dim pinfo As New ProcessStartInfo
            pinfo.UseShellExecute = True 'You can add a progressbar or a status with this
            pinfo.CreateNoWindow = True 'User will not see it 
            pinfo.FileName = "test.bat"
            Label8.Text = "Converting..."
            Delay(0.1)
            Dim p As Process = Process.Start(pinfo)
            test = True
            Do
                If Len(Dir("test.bat")) = 0 Then 'Same as IO.File.Exist()
                    test = False
                    Label8.Text = "Copy..."
                    Delay(0.1)
                    IO.File.Copy(Tempfile2, savevideo.FileName, True) 'Copy finished converted file
                    Dim i2 As Integer = MessageBox.Show("Download finished. Would you like to open it ?", "Download", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 'For answer
                    If i2 = 6 Then
                        Process.Start(savevideo.FileName) 'Start
                    End If
                    Exit Do
                Else
                    Application.DoEvents() 'Don't know if this helped
                End If
            Loop
        End If
        Label8.Text = "Waiting..." 'controls
        GButton3.Enabled = True
        GButton3_Click() 'Reloading Data...
    End Sub

    Sub Download_Progress(ByVal sender As System.Object, ByVal e As System.Net.DownloadProgressChangedEventArgs) Handles Webclientos.DownloadProgressChanged
        ProgressbarWithPercentage1.Value = e.ProgressPercentage 'For user to know how high 
    End Sub

    Function getEndung(ByVal file As String) As String 'Get *.mp3 or *.flv
        Dim filen As String = file
        Dim pos1 As Integer = file.LastIndexOf(".")
        Return filen.Substring(pos1)
    End Function

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://twitter.com/#/validati0n")
    End Sub
End Class

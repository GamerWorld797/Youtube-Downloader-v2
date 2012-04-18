<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainfrm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mainfrm))
        Me.savevideo = New System.Windows.Forms.SaveFileDialog()
        Me.GTheme1 = New Youtube_Downloader_v2.GTheme()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ProgressbarWithPercentage1 = New Youtube_Downloader_v2.ProgressbarWithPercentage()
        Me.GButton4 = New Youtube_Downloader_v2.GButton()
        Me.Seperator1 = New Youtube_Downloader_v2.Seperator()
        Me.GButton3 = New Youtube_Downloader_v2.GButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Gpbox1 = New Youtube_Downloader_v2.GPBOX()
        Me.GButton2 = New Youtube_Downloader_v2.GButton()
        Me.GButton1 = New Youtube_Downloader_v2.GButton()
        Me.GTheme1.SuspendLayout()
        Me.SuspendLayout()
        '
        'savevideo
        '
        Me.savevideo.Filter = "MP3 File|*.mp3|Web Movie|*.webm"
        '
        'GTheme1
        '
        Me.GTheme1.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(41, Byte), Integer))
        Me.GTheme1.Controls.Add(Me.LinkLabel1)
        Me.GTheme1.Controls.Add(Me.Label8)
        Me.GTheme1.Controls.Add(Me.Label7)
        Me.GTheme1.Controls.Add(Me.Label6)
        Me.GTheme1.Controls.Add(Me.Label5)
        Me.GTheme1.Controls.Add(Me.Label4)
        Me.GTheme1.Controls.Add(Me.Label3)
        Me.GTheme1.Controls.Add(Me.Label2)
        Me.GTheme1.Controls.Add(Me.ProgressbarWithPercentage1)
        Me.GTheme1.Controls.Add(Me.GButton4)
        Me.GTheme1.Controls.Add(Me.Seperator1)
        Me.GTheme1.Controls.Add(Me.GButton3)
        Me.GTheme1.Controls.Add(Me.Label1)
        Me.GTheme1.Controls.Add(Me.TextBox1)
        Me.GTheme1.Controls.Add(Me.Gpbox1)
        Me.GTheme1.Controls.Add(Me.GButton2)
        Me.GTheme1.Controls.Add(Me.GButton1)
        Me.GTheme1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GTheme1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.GTheme1.Image = Nothing
        Me.GTheme1.Location = New System.Drawing.Point(0, 0)
        Me.GTheme1.MoveHeight = 17
        Me.GTheme1.Name = "GTheme1"
        Me.GTheme1.Resizable = False
        Me.GTheme1.Size = New System.Drawing.Size(487, 272)
        Me.GTheme1.TabIndex = 0
        Me.GTheme1.Text = "Youtube Downloader V2"
        Me.GTheme1.TransparencyKey = System.Drawing.Color.Fuchsia
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(451, 43)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(23, 13)
        Me.LinkLabel1.TabIndex = 16
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Me"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(315, 43)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(139, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Waiting..."
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 224)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(43, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Views: "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 211)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Dislikes: "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 198)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Likes: "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 185)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Uploaddate: "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 172)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Uploader: "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 159)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Name: "
        '
        'ProgressbarWithPercentage1
        '
        Me.ProgressbarWithPercentage1.Location = New System.Drawing.Point(12, 240)
        Me.ProgressbarWithPercentage1.Name = "ProgressbarWithPercentage1"
        Me.ProgressbarWithPercentage1.OverlayTextColor = System.Drawing.Color.Black
        Me.ProgressbarWithPercentage1.Percentage = 0.0R
        Me.ProgressbarWithPercentage1.PercentageAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ProgressbarWithPercentage1.Size = New System.Drawing.Size(338, 23)
        Me.ProgressbarWithPercentage1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressbarWithPercentage1.TabIndex = 8
        Me.ProgressbarWithPercentage1.TextColor = System.Drawing.Color.DimGray
        '
        'GButton4
        '
        Me.GButton4.Enabled = False
        Me.GButton4.Image = Nothing
        Me.GButton4.Location = New System.Drawing.Point(356, 240)
        Me.GButton4.Name = "GButton4"
        Me.GButton4.NoRounding = False
        Me.GButton4.Size = New System.Drawing.Size(119, 23)
        Me.GButton4.TabIndex = 7
        Me.GButton4.Text = "Download"
        '
        'Seperator1
        '
        Me.Seperator1.Color1 = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Seperator1.Color2 = System.Drawing.Color.FromArgb(CType(CType(14, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Seperator1.Direction = System.Windows.Forms.Orientation.Horizontal
        Me.Seperator1.Image = Nothing
        Me.Seperator1.Location = New System.Drawing.Point(0, 133)
        Me.Seperator1.Name = "Seperator1"
        Me.Seperator1.NoRounding = False
        Me.Seperator1.Size = New System.Drawing.Size(533, 23)
        Me.Seperator1.TabIndex = 6
        Me.Seperator1.Text = "Seperator1"
        '
        'GButton3
        '
        Me.GButton3.Image = Nothing
        Me.GButton3.Location = New System.Drawing.Point(12, 104)
        Me.GButton3.Name = "GButton3"
        Me.GButton3.NoRounding = False
        Me.GButton3.Size = New System.Drawing.Size(463, 23)
        Me.GButton3.TabIndex = 5
        Me.GButton3.Text = "Load"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Youtube URL"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(41, Byte), Integer))
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Segoe UI Light", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.TextBox1.Location = New System.Drawing.Point(15, 62)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(459, 32)
        Me.TextBox1.TabIndex = 3
        '
        'Gpbox1
        '
        Me.Gpbox1.Border1 = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Gpbox1.Border2 = System.Drawing.Color.FromArgb(CType(CType(14, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Gpbox1.FillColor = System.Drawing.Color.Transparent
        Me.Gpbox1.Image = Nothing
        Me.Gpbox1.Location = New System.Drawing.Point(12, 59)
        Me.Gpbox1.Name = "Gpbox1"
        Me.Gpbox1.NoRounding = False
        Me.Gpbox1.Size = New System.Drawing.Size(465, 38)
        Me.Gpbox1.TabIndex = 2
        Me.Gpbox1.Text = "Gpbox1"
        '
        'GButton2
        '
        Me.GButton2.Image = Nothing
        Me.GButton2.Location = New System.Drawing.Point(408, 3)
        Me.GButton2.Name = "GButton2"
        Me.GButton2.NoRounding = False
        Me.GButton2.Size = New System.Drawing.Size(35, 23)
        Me.GButton2.TabIndex = 1
        Me.GButton2.Text = "_"
        '
        'GButton1
        '
        Me.GButton1.Image = Nothing
        Me.GButton1.Location = New System.Drawing.Point(449, 3)
        Me.GButton1.Name = "GButton1"
        Me.GButton1.NoRounding = False
        Me.GButton1.Size = New System.Drawing.Size(35, 23)
        Me.GButton1.TabIndex = 0
        Me.GButton1.Text = "X"
        '
        'mainfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(487, 272)
        Me.Controls.Add(Me.GTheme1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "mainfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Youtube Downloader"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.GTheme1.ResumeLayout(False)
        Me.GTheme1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GTheme1 As Youtube_Downloader_v2.GTheme
    Friend WithEvents GButton2 As Youtube_Downloader_v2.GButton
    Friend WithEvents GButton1 As Youtube_Downloader_v2.GButton
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Gpbox1 As Youtube_Downloader_v2.GPBOX
    Friend WithEvents GButton3 As Youtube_Downloader_v2.GButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GButton4 As Youtube_Downloader_v2.GButton
    Friend WithEvents Seperator1 As Youtube_Downloader_v2.Seperator
    Friend WithEvents ProgressbarWithPercentage1 As Youtube_Downloader_v2.ProgressbarWithPercentage
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents savevideo As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel

End Class

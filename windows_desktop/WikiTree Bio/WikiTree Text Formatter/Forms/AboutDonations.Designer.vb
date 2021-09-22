<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutDonations
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.llblMoi = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(496, 117)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "I have enjoyed bringing you this tool for creating biographies on WikiTree. I do " & _
    "it because I love programming and genealogy and this combines both worlds."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(0, 117)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(496, 92)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "I have no wish to make money out of the venture so I am afraid you cannot send me" & _
    " any money to recompense my efforts."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Location = New System.Drawing.Point(0, 209)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(496, 92)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "If, however, you still feel constrained to part with some cash then please make a" & _
    " donation to a charity such as cancer research or animal welfare both of which I" & _
    " favour."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label4.Location = New System.Drawing.Point(0, 369)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(496, 142)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Please feel free to let me know that you have made a donation. (Use the link abov" & _
    "e if you like)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Not how much, I am not interested in that, just that you have " & _
    "done so."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'llblMoi
        '
        Me.llblMoi.AutoSize = True
        Me.llblMoi.BackColor = System.Drawing.Color.Transparent
        Me.llblMoi.Location = New System.Drawing.Point(120, 324)
        Me.llblMoi.Name = "llblMoi"
        Me.llblMoi.Size = New System.Drawing.Size(252, 24)
        Me.llblMoi.TabIndex = 4
        Me.llblMoi.TabStop = True
        Me.llblMoi.Text = "David Loring (Meredith-1182)"
        '
        'AboutDonations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.LavenderBlush
        Me.BackgroundImage = Global.WikiTree_Text_Formatter_Desktop.My.Resources.Resources.MagBlueFade600x300
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(496, 511)
        Me.Controls.Add(Me.llblMoi)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Gold
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "AboutDonations"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About Donations"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents llblMoi As System.Windows.Forms.LinkLabel
End Class

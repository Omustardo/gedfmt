<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PasteBox
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnOK = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnCancel = New System.Windows.Forms.ToolStripButton()
        Me.txtPaste = New System.Windows.Forms.TextBox()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnOK, Me.tsbtnCancel})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(706, 27)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnOK
        '
        Me.tsbtnOK.Image = Global.WikiTree_Text_Formatter_Desktop.My.Resources.Resources.confirm_16
        Me.tsbtnOK.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnOK.Name = "tsbtnOK"
        Me.tsbtnOK.Size = New System.Drawing.Size(49, 24)
        Me.tsbtnOK.Text = "OK"
        '
        'tsbtnCancel
        '
        Me.tsbtnCancel.Image = Global.WikiTree_Text_Formatter_Desktop.My.Resources.Resources.cancel_16
        Me.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnCancel.Name = "tsbtnCancel"
        Me.tsbtnCancel.Size = New System.Drawing.Size(73, 24)
        Me.tsbtnCancel.Text = "Cancel"
        '
        'txtPaste
        '
        Me.txtPaste.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPaste.Location = New System.Drawing.Point(0, 27)
        Me.txtPaste.Multiline = True
        Me.txtPaste.Name = "txtPaste"
        Me.txtPaste.Size = New System.Drawing.Size(706, 211)
        Me.txtPaste.TabIndex = 1
        '
        'PasteBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(706, 238)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtPaste)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "PasteBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Paste Box"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Private WithEvents tsbtnOK As System.Windows.Forms.ToolStripButton
    Private WithEvents tsbtnCancel As System.Windows.Forms.ToolStripButton
    Private WithEvents txtPaste As System.Windows.Forms.TextBox
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CopySelector
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
        Me.optReplace = New System.Windows.Forms.RadioButton()
        Me.optAddAbove = New System.Windows.Forms.RadioButton()
        Me.optAddBelow = New System.Windows.Forms.RadioButton()
        Me.optReplaceInSection = New System.Windows.Forms.RadioButton()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdContinue = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'optReplace
        '
        Me.optReplace.Checked = True
        Me.optReplace.Location = New System.Drawing.Point(19, 12)
        Me.optReplace.Name = "optReplace"
        Me.optReplace.Size = New System.Drawing.Size(346, 24)
        Me.optReplace.TabIndex = 0
        Me.optReplace.TabStop = True
        Me.optReplace.Text = "&Replace any current text in the Assembly Area"
        Me.optReplace.UseVisualStyleBackColor = True
        '
        'optAddAbove
        '
        Me.optAddAbove.Location = New System.Drawing.Point(19, 48)
        Me.optAddAbove.Name = "optAddAbove"
        Me.optAddAbove.Size = New System.Drawing.Size(346, 24)
        Me.optAddAbove.TabIndex = 1
        Me.optAddAbove.Text = "Add &above any current text in the Assembly Area"
        Me.optAddAbove.UseVisualStyleBackColor = True
        '
        'optAddBelow
        '
        Me.optAddBelow.Location = New System.Drawing.Point(19, 84)
        Me.optAddBelow.Name = "optAddBelow"
        Me.optAddBelow.Size = New System.Drawing.Size(346, 24)
        Me.optAddBelow.TabIndex = 2
        Me.optAddBelow.Text = "Add &below any current text in the Assembly Area"
        Me.optAddBelow.UseVisualStyleBackColor = True
        '
        'optReplaceInSection
        '
        Me.optReplaceInSection.Enabled = False
        Me.optReplaceInSection.Location = New System.Drawing.Point(19, 120)
        Me.optReplaceInSection.Name = "optReplaceInSection"
        Me.optReplaceInSection.Size = New System.Drawing.Size(346, 51)
        Me.optReplaceInSection.TabIndex = 3
        Me.optReplaceInSection.Text = "Attempt to replace a &Section of the same name in the assembly area (Coming soon)" & _
    ""
        Me.optReplaceInSection.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(19, 177)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 52)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel Copy"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdContinue
        '
        Me.cmdContinue.Location = New System.Drawing.Point(193, 177)
        Me.cmdContinue.Name = "cmdContinue"
        Me.cmdContinue.Size = New System.Drawing.Size(172, 52)
        Me.cmdContinue.TabIndex = 5
        Me.cmdContinue.Text = "&Proceed"
        Me.cmdContinue.UseVisualStyleBackColor = True
        '
        'CopySelector
        '
        Me.AcceptButton = Me.cmdContinue
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(384, 242)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdContinue)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.optReplaceInSection)
        Me.Controls.Add(Me.optAddBelow)
        Me.Controls.Add(Me.optAddAbove)
        Me.Controls.Add(Me.optReplace)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "CopySelector"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Copy Selector"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents optReplace As System.Windows.Forms.RadioButton
    Friend WithEvents optAddAbove As System.Windows.Forms.RadioButton
    Friend WithEvents optAddBelow As System.Windows.Forms.RadioButton
    Friend WithEvents optReplaceInSection As System.Windows.Forms.RadioButton
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdContinue As System.Windows.Forms.Button
End Class

Public Class PasteBox
    Public PastedText As String = ""

    Private Sub tsbtnOK_Click(sender As Object, e As EventArgs) Handles tsbtnOK.Click
        PastedText = txtPaste.Text
        Me.Hide()
    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click
        PastedText = ""
        Me.Hide()
    End Sub
End Class
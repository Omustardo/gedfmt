Public Class CopySelector
    Private m_intCopySelection As Integer = -1

    Public Function CopySelection() As Integer
        Return m_intCopySelection
    End Function

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Me.Hide()
    End Sub

    Private Sub cmdContinue_Click(sender As Object, e As EventArgs) Handles cmdContinue.Click
        If optReplace.Checked = True Then
            m_intCopySelection = 0
        ElseIf optAddAbove.Checked = True Then
            m_intCopySelection = 1
        ElseIf optAddBelow.Checked = True Then
            m_intCopySelection = 2
        ElseIf optReplaceInSection.Checked = True Then
            m_intCopySelection = 3
        End If
        Me.Hide()
    End Sub
End Class
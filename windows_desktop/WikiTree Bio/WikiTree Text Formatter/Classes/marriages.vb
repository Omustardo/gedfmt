Public Class marriages
    Implements System.Collections.IEnumerable
    Private colRecords As New Collection
    Private m_strFoundSectionYear As String = ""

    Public Function Add(ByVal MarriageYear As String) As String
        Dim cNewRec As New outSection
        Dim strRTN As String = MarriageYear
        If SectionExists(MarriageYear) = False Then
            cNewRec.SectionYear = MarriageYear
            colRecords.Add(cNewRec, cNewRec.SectionYear)
        Else
            strRTN = m_strFoundSectionYear
        End If
        Return strRTN
    End Function

    Public Function Count() As Long
        Count = colRecords.Count
    End Function

    Public Sub Delete(ByVal Index As Object)
        colRecords.Remove(Index)
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Object) As outSection
        Get
            Item = colRecords.Item(Index)
        End Get
    End Property

    Public Sub Clear()
        Dim intLoopVar As Short = 0

        On Error Resume Next
        If colRecords.Count > 0 Then
            For intLoopVar = colRecords.Count To 1 Step -1
                colRecords.Remove(intLoopVar)
            Next
        End If
    End Sub
    Private Function SectionExists(ByVal SectionData As String) As Boolean
        Dim bFound As Boolean = False
        Dim sectionEnum As IEnumerator = GetEnumerator()
        Dim thisSection As outSection
        Dim intCount As Integer = 1
        sectionEnum.Reset()
        m_strFoundSectionYear = ""
        While sectionEnum.MoveNext()
            thisSection = sectionEnum.Current()
            If thisSection.SectionYear = SectionData Then
                bFound = True
                m_strFoundSectionYear = thisSection.SectionYear
                Exit While
            End If
            intCount += 1
        End While
        Return bFound
    End Function
    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        GetEnumerator = colRecords.GetEnumerator
    End Function
End Class

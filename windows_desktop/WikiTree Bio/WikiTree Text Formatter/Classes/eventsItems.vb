Imports System.Text
Public Class eventsItems
    Implements System.Collections.IEnumerable
    Private colRecords As New Collection
    Private m_ndecFoundSectionYear As Decimal = 0
    Private m_bFound As Boolean = False

    Public Function Add(EventYear As String) As String
        Dim cNewRec As New eventsItem
        SectionExists(EventYear)
        cNewRec.EventYear = m_ndecFoundSectionYear
        colRecords.Add(cNewRec, cNewRec.EventYear.ToString)
        Return m_ndecFoundSectionYear.ToString
    End Function

    Public Function Count() As Long
        Count = colRecords.Count
    End Function

    Public Sub Delete(ByVal Index As Object)
        colRecords.Remove(Index)
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Object) As eventsItem
        Get
            Item = colRecords.Item(Index)
        End Get
    End Property
    Private Sub SectionExists(ByRef YearData As String)
        Dim eventsEnum As IEnumerator = GetEnumerator()
        Dim thisSection As eventsItem
        Dim intCount As Integer = 1
        Dim bFound As Boolean = False
        Dim ndecTargetYear As Decimal = 0
        Dim sbld As StringBuilder = New StringBuilder(YearData.ToLower)
        sbld.Replace("jan", "")
        sbld.Replace("january", "")
        sbld.Replace("feb", "")
        sbld.Replace("february", "")
        sbld.Replace("mar", "")
        sbld.Replace("march", "")
        sbld.Replace("apr", "")
        sbld.Replace("april", "")
        sbld.Replace("may", "")
        sbld.Replace("jun", "")
        sbld.Replace("june", "")
        sbld.Replace("jul", "")
        sbld.Replace("july", "")
        sbld.Replace("aug", "")
        sbld.Replace("august", "")
        sbld.Replace("sep", "")
        sbld.Replace("september", "")
        sbld.Replace("oct", "")
        sbld.Replace("october", "")
        sbld.Replace("nov", "")
        sbld.Replace("novemebr", "")
        sbld.Replace("dec", "")
        sbld.Replace("december", "")
        YearData = sbld.ToString.Trim
        If Val(YearData) = 0 Then
            ndecTargetYear = Year(Now)
        Else
            ndecTargetYear = CDec(Val(YearData))
        End If
        eventsEnum.Reset()
        m_ndecFoundSectionYear = 0
        While eventsEnum.MoveNext()
            thisSection = eventsEnum.Current()
            If thisSection.EventYear = ndecTargetYear Then
                m_ndecFoundSectionYear = ndecTargetYear + (CDec(thisSection.IndexNo + 1) / 10)
                bFound = True
                Exit While
            End If
            intCount += 1
        End While
        If bFound = False Then
            m_ndecFoundSectionYear = ndecTargetYear + 0.1
        End If
    End Sub

    Public Sub Clear()
        Dim intLoopVar As Short = 0

        On Error Resume Next
        If colRecords.Count > 0 Then
            For intLoopVar = colRecords.Count To 1 Step -1
                colRecords.Remove(intLoopVar)
            Next
        End If
    End Sub
    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        GetEnumerator = colRecords.GetEnumerator
    End Function
End Class

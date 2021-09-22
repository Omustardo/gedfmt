Public Class sources
    Implements System.Collections.IEnumerable
    Private colRecords As New Collection

    Public Function Add(ByVal SourceCode As String) As Integer
        Dim intPointer As Integer = 0
        intPointer = ParseCollection(SourceCode)
        If intPointer = -1 Then
            Dim cNewRec As New source
            ' Generate a unique ID for the new field.
            intPointer = colRecords.Count + 1
            cNewRec.ID = SourceCode
            colRecords.Add(cNewRec, cNewRec.ID)
        End If
        Return intPointer
    End Function

    Public Function Count() As Long
        Count = colRecords.Count
    End Function

    Public Sub Delete(ByVal Index As Object)
        colRecords.Remove(Index)
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Object) As source
        Get
            Item = colRecords.Item(Index)
        End Get
    End Property

    Private Function ParseCollection(ByVal CodeToFind As String) As Integer
        Dim intLoopVar As Integer = 0
        Dim element As source
        Dim bFound As Boolean = False
        On Error Resume Next
        If colRecords.Count > 0 Then
            For intLoopVar = 1 To colRecords.Count
                element = colRecords(intLoopVar)
                If element.ID = CodeToFind Then
                    bFound = True
                    Exit For
                End If
            Next
        End If
        If bFound = False Then
            intLoopVar = -1
        End If
        Return intLoopVar
    End Function

    Public Sub Clear()
        Dim intLoopVar As Integer = 0

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

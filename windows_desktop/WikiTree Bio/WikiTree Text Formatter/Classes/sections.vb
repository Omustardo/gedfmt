Public Class sections
    Implements System.Collections.IEnumerable
    Private colRecords As New Collection
    Private m_intFoundSectionPointer As Integer = 0
    Private m_strMainName As String = ""
    Private m_strMainWTID As String = ""
    Private m_strFatherName As String = ""
    Private m_strFatherWTID As String = ""
    Private m_strMotherName As String = ""
    Private m_strMotherWTID As String = ""

    Public Property MainName As String
        Get
            Return m_strMainName
        End Get
        Set(value As String)
            m_strMainName = value
        End Set
    End Property
    Public Property MainWikiTreeID As String
        Get
            Return m_strMainWTID
        End Get
        Set(value As String)
            m_strMainWTID = value
        End Set
    End Property
    Public Property FatherName As String
        Get
            Return m_strFatherName
        End Get
        Set(value As String)
            m_strFatherName = value
        End Set
    End Property
    Public Property FatherWikiTreeID As String
        Get
            Return m_strFatherWTID
        End Get
        Set(value As String)
            m_strFatherWTID = value
        End Set
    End Property
    Public Property MotherName As String
        Get
            Return m_strMotherName
        End Get
        Set(value As String)
            m_strMotherName = value
        End Set
    End Property
    Public Property MotherWikiTreeID As String
        Get
            Return m_strMotherWTID
        End Get
        Set(value As String)
            m_strMotherWTID = value
        End Set
    End Property
    Public Function Add(ByVal SectionName As String) As Integer
        Dim cNewRec As New section
        Dim intPointer As Integer = 0
        If SectionExists(SectionName) = False Then
            ' Generate a unique ID for the new field.
            intPointer = colRecords.Count + 1
            cNewRec.ID = SectionName
            colRecords.Add(cNewRec, cNewRec.ID)
        Else
            intPointer = m_intFoundSectionPointer
        End If
        Return intPointer
    End Function

    Public Function Count() As Long
        Count = colRecords.Count
    End Function

    Public Sub Delete(ByVal Index As Object)
        colRecords.Remove(Index)
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Object) As section
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
    Private Function SectionExists(ByVal SectionName As String) As Boolean
        Dim bFound As Boolean = False
        Dim sectionEnum As IEnumerator = GetEnumerator()
        Dim thisSection As section
        Dim intCount As Integer = 1
        sectionEnum.Reset()
        While sectionEnum.MoveNext()
            thisSection = sectionEnum.Current()
            If thisSection.ID = SectionName Then
                bFound = True
                m_intFoundSectionPointer = intCount
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

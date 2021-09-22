Imports System.Collections
Imports System.Collections.Generic
Public Class FMPCensusLines
    Implements System.Collections.IEnumerable
    Private colRecords As New Collection
    Private header As New FMPCensusLine
    Private m_fieldData As New List(Of ListsClasses.TI2)
    Private m_intCensusYear As Integer = 0
    Private m_strCensusCountry As String = ""
    Private m_strCensusFormat As String = ""

    Public Function Add() As Integer
        Dim cNewRec As New FMPCensusLine

        On Error Resume Next
        With cNewRec
            ' Generate a unique ID for the new field.
            .ID = colRecords.Count + 1
            colRecords.Add(cNewRec, .ID)
        End With
        Return cNewRec.ID
    End Function
    Public Property CensusCountry As String
        Get
            Return m_strCensusCountry
        End Get
        Set(value As String)
            m_strCensusCountry = value
        End Set
    End Property
    Public Property CensusYear As Integer
        Get
            Return m_intCensusYear
        End Get
        Set(value As Integer)
            m_intCensusYear = value
        End Set
    End Property
    Public Property DataFormat As String
        Get
            Return m_strCensusFormat
        End Get
        Set(value As String)
            m_strCensusFormat = value
        End Set
    End Property
    Public Function FieldHeaderData() As List(Of ListsClasses.TI2)
        If m_fieldData.Count = 0 Then
            Select Case m_strCensusFormat
                Case "FMP"
                    Select Case m_strCensusCountry
                        Case "CA"
                            Select Case m_intCensusYear
                                Case 1861, 1901, 1911
                                    m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Gender", 2, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Year", 3, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 4, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Role", 5, 0))
                                Case 1871, 1881
                                    m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Gender", 2, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Year", 3, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 4, 0))
                            End Select
                        Case "EI"
                            Select Case m_intCensusYear
                                Case 1901, 1911
                                    m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Role", 2, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Status", 3, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Age", 4, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Year", 5, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 6, 0))
                            End Select
                        Case "UK"
                            Select Case m_intCensusYear
                                Case 1841
                                    m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Gender", 2, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Age", 3, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Year", 4, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 5, 0))
                                Case 1851 To 1901
                                    m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Role", 2, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Status", 3, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Gender", 4, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Age", 5, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Year", 6, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Occupation", 7, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 8, 0))
                                Case 1911
                                    m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Role", 2, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Status", 3, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Gender", 4, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Occupation", 5, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Age", 6, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Year", 7, 0))
                                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 8, 0))
                            End Select
                        Case "US"
                            m_fieldData.Add(New ListsClasses.TI2("First Names", 0, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Last Name", 1, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Role", 2, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Status", 3, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Gender", 4, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Occupation", 5, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Age", 6, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Birth Year", 7, 0))
                            m_fieldData.Add(New ListsClasses.TI2("Birth Place", 8, 0))
                    End Select
                Case "FS"
                    'This is a family search block
                    m_fieldData.Add(New ListsClasses.TI2("Household", 0, 0))
                    m_fieldData.Add(New ListsClasses.TI2("Role", 1, 0))
                    m_fieldData.Add(New ListsClasses.TI2("Sex", 2, 0))
                    m_fieldData.Add(New ListsClasses.TI2("Age", 3, 0))
                    m_fieldData.Add(New ListsClasses.TI2("Birth Place", 4, 0))
            End Select
        End If
        Return m_fieldData
    End Function
    Public Function Count() As Integer
        Count = colRecords.Count
    End Function
    Public Sub Delete(ByVal Index As Integer)
        colRecords.Remove(Index)
    End Sub
    Default Public ReadOnly Property Item(ByVal Index As Integer) As FMPCensusLine
        Get
            Item = colRecords.Item(Index)
        End Get
    End Property
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

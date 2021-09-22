Imports System.Collections
Imports System.Collections.Generic

Public Class eventsItem
    Private m_strEventType As String = ""
    Private m_intNumber As Integer = 0
    Private m_dteDate As Date
    Private m_ndecEventYear As Decimal = 0
    Private m_strPlace As String = ""
    Private m_strDateString As String
    Private m_intDateStatusIndicator As Integer = DATE_UNK
    Private m_bHasData As Boolean = False
    Private m_listOtherLines As New List(Of String)
    Private m_intIndexNo As Integer = 1

    Public ReadOnly Property HasData As Boolean
        Get
            Return m_bHasData
        End Get
    End Property
    Public Property EventType As String
        Get
            Return m_strEventType
        End Get
        Set(value As String)
            m_strEventType = value
            m_bHasData = True
        End Set
    End Property
    Public Property EventDate As Date
        Get
            Return m_dteDate
        End Get
        Set(value As Date)
            m_dteDate = value
            m_bHasData = True
        End Set
    End Property
    Public Property EventDateString As String
        Get
            Return m_strDateString
        End Get
        Set(value As String)
            m_strDateString = value
            m_bHasData = True
        End Set
    End Property
    Public Property DateStatus As Integer
        Get
            Return m_intDateStatusIndicator
        End Get
        Set(value As Integer)
            m_intDateStatusIndicator = value
            m_bHasData = True
        End Set
    End Property
    Public Property EventYear As Decimal
        Get
            Return m_ndecEventYear
        End Get
        Set(value As Decimal)
            m_ndecEventYear = value
            m_bHasData = True
        End Set
    End Property
    Public Property IndexNo As Integer
        Get
            Return m_intIndexNo
        End Get
        Set(value As Integer)
            m_intIndexNo = value
            m_bHasData = True
        End Set
    End Property
    ''' <summary>
    ''' The place will probably also included the references
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EventPlace As String
        Get
            Return m_strPlace
        End Get
        Set(value As String)
            m_strPlace = value
            m_bHasData = True
        End Set
    End Property
    Public Function OtherLines() As List(Of String)
        Return m_listOtherLines
    End Function
End Class

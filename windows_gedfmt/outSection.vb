Imports System.Collections.Generic

Public Class outSection
    Private m_strName As String = ""
    Private m_intNumber As Integer = 0
    Private m_dteDate As Date
    Private m_intSectionYear As Integer = 0
    Private m_strPlace As String = ""
    Private m_strDateString As String
    Private m_intDateStatusIndicator As Integer = DATE_UNK
    Private m_bHasData As Boolean = False
    Private m_listOtherLines As New List(Of String)
    Private m_strHusband As String = ""
    Private m_strWife As String = ""
    Private listChildren As New List(Of String)

    ''' <summary>
    ''' Only filled for marriages
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Children() As List(Of String)
        Return listChildren
    End Function
    Public Property Husband As String
        Get
            Return m_strHusband
        End Get
        Set(value As String)
            m_strHusband = value
        End Set
    End Property
    Public Property Wife As String
        Get
            Return m_strWife
        End Get
        Set(value As String)
            m_strWife = value
        End Set
    End Property
    Public ReadOnly Property HasData As Boolean
        Get
            Return m_bHasData
        End Get
    End Property
    Public Property SectionName As String
        Get
            Return m_strName
        End Get
        Set(value As String)
            m_strName = value
            Select Case value.ToLower
                Case "0"
                    m_intNumber = L2HEADR_FREESPACE '100
                Case "acknowledgements"
                    m_intNumber = SECTION_ACKNOL '8
                Case "baptism", "christening"
                    m_intNumber = SECTION_BAPT '2
                Case "biography"
                    m_intNumber = L2HEADR_BIOG '200
                Case "birth"
                    m_intNumber = SECTION_BIRTH '1
                Case "burial"
                    m_intNumber = SECTION_BURIAL '11
                Case "census", "residence"
                    m_intNumber = SECTION_CENSUS '6
                Case "children"
                    m_intNumber = SECTION_CHILDR '5
                Case "death"
                    m_intNumber = SECTION_DEATH '10
                Case "divorce"
                    m_intNumber = SECTION_DIVOR '4
                Case "event"
                    m_intNumber = SECTION_EVENTS '99
                Case "general"
                    m_intNumber = SECTION_GENERAL '9
                Case "marriage"
                    m_intNumber = SECTION_MARR '3
                Case "notes"
                    m_intNumber = SECTION_NOTES '7
                Case "occupation"
                    m_intNumber = L2HEADR_BIOG + 1 '201
                Case "sources"
                    m_intNumber = L2HEADR_SRCES '400
                Case "timeline"
                    m_intNumber = L2HEADR_TIMEL '300
                Case "z"
                    m_intNumber = SECTION_FREESPACE '0
                Case Else
                    m_intNumber = L2HEADR_UNKNOWN '500
            End Select
            m_bHasData = True
        End Set
    End Property
    Public Property SectionDate As Date
        Get
            Return m_dteDate
        End Get
        Set(value As Date)
            m_dteDate = value
            If m_intSectionYear = 0 Then
                m_intSectionYear = Year(value)
            End If
            m_bHasData = True
        End Set
    End Property
    Public Property SectionDateString As String
        Get
            Return m_strDateString
        End Get
        Set(value As String)
            m_strDateString = value
            m_intSectionYear = CInt(Val(value))
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
    Public Property SectionYear As Integer
        Get
            Return m_intSectionYear
        End Get
        Set(value As Integer)
            m_intSectionYear = value
            m_bHasData = True
        End Set
    End Property
    ''' <summary>
    ''' The place will probably also included the references
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SectionPlace As String
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

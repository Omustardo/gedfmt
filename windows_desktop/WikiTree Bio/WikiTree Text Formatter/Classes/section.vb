Public Class section
    Private m_strSectionYear As String = ""
    Private m_bYearFilled As Boolean = False
    Private m_listMainLines As New List(Of String)
    Private m_intLevel As Integer = 0
    Private m_strParentSection As String = ""
    ' Private data for the write-once ID property.
    Private m_strID As String = ""

    Public Property SectionLevel As Integer
        Get
            Return m_intLevel
        End Get
        Set(value As Integer)
            m_intLevel = value
        End Set
    End Property
    Public Function Lines() As List(Of String)
        Return m_listMainLines
    End Function

    Public Property SectionYear() As String
        Get
            Return m_strSectionYear
        End Get
        Set(ByVal value As String)
            If m_bYearFilled = False Then
                m_strSectionYear = value
                m_bYearFilled = True
            End If
        End Set
    End Property

    Public Property ParentSection() As String
        Get
            Return m_strParentSection
        End Get
        Set(ByVal value As String)
            m_strParentSection = value
        End Set
    End Property

    Public Property ID() As String
        Get
            ID = m_strID
        End Get

        Set(ByVal strNew As String)
            ' The first time the ID property is set, the static
            ' Boolean is also set.  Subsequent calls do nothing.
            ' (It would be better to raise an error, instead.)
            Static blnAlreadySet As Boolean

            If Not blnAlreadySet Then
                blnAlreadySet = True
                m_strID = strNew
            End If
        End Set
    End Property
End Class
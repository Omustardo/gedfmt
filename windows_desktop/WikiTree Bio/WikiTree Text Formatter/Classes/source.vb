Public Class source
    Private m_strSourceInLineCode As String = ""
    Private m_strSourceText As String = ""
    Private m_bFullTextUsed As Boolean = False
    ' Private data for the write-once ID property.
    Private m_strID As String = ""

    Public Property SourceInLineCode() As String
        Get
            Return m_strSourceInLineCode
        End Get
        Set(ByVal value As String)
            m_strSourceInLineCode = value
        End Set
    End Property

    Public Property SourceText() As String
        Get
            Return m_strSourceText
        End Get
        Set(ByVal value As String)
            m_strSourceText = value
        End Set
    End Property

    Public Property FullTextUsed() As Boolean
        Get
            Return m_bFullTextUsed
        End Get
        Set(value As Boolean)
            m_bFullTextUsed = value
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

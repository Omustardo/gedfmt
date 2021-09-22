Public Class FMPCensusLine
    Private m_intAge As Integer = 0
    Private m_intBirthYear As Integer = 0
    Private m_strBirthPlace As String = ""
    Private m_strFirstNames As String = ""
    Private m_strFullName As String = ""
    Private m_strGender As String = ""
    Private m_strLastName As String = ""
    Private m_strOccupation As String = ""
    Private m_strRole As String = ""
    Private m_strStatus As String = ""

    ' Private data for the write-once ID property.
    Private m_intID As Integer = 0

    Public Property FirstNames() As String
        Get
            Return m_strFirstNames
        End Get
        Set(value As String)
            m_strFirstNames = value
        End Set
    End Property
    Public Property LastName() As String
        Get
            Return m_strLastName
        End Get
        Set(value As String)
            m_strLastName = value
        End Set
    End Property
    Public Property FullName() As String
        Get
            If m_strFullName.Trim = "" Then
                If m_strFirstNames.Trim > "" Then
                    m_strFullName = m_strFirstNames
                    If m_strLastName.Trim > "" Then
                        m_strFullName &= " " & m_strLastName
                    End If
                ElseIf m_strLastName.Trim > "" Then
                    m_strFullName = m_strLastName
                End If
            End If
            Return m_strFullName
        End Get
        Set(value As String)
            m_strFullName = value
        End Set
    End Property
    Public Property Role() As String
        Get
            Return m_strRole
        End Get
        Set(value As String)
            m_strRole = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return m_strStatus
        End Get
        Set(value As String)
            m_strStatus = value
        End Set
    End Property
    Public Property Gender() As String
        Get
            Return m_strGender
        End Get
        Set(value As String)
            m_strGender = value
        End Set
    End Property
    Public Property Occupation() As String
        Get
            Return m_strOccupation
        End Get
        Set(value As String)
            m_strOccupation = value
        End Set
    End Property
    Public Property Age() As Integer
        Get
            Return m_intAge
        End Get
        Set(value As Integer)
            m_intAge = value
        End Set
    End Property
    Public Property BirthYear() As Integer
        Get
            Return m_intBirthYear
        End Get
        Set(value As Integer)
            m_intBirthYear = value
        End Set
    End Property
    Public Property BirthPlace() As String
        Get
            Return m_strBirthPlace
        End Get
        Set(value As String)
            m_strBirthPlace = value
        End Set
    End Property
    Public Property ID() As Integer
        Get
            Return m_intID
        End Get
        Set(value As Integer)
            ' The first time the ID property is set, the static
            ' Boolean is also set.  Subsequent calls do nothing.
            ' (It would be better to raise an error, instead.)
            Static blnAlreadySet As Boolean
            If Not blnAlreadySet Then
                blnAlreadySet = True
                m_intID = value
            End If
        End Set
    End Property
End Class

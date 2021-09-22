Imports System.Security.Cryptography
Imports System.Text
Module WTBRoutines
    Private Declare Auto Function URLDownloadToFile Lib "urlmon" (ByVal pCaller As IntPtr, ByVal szURL As String, ByVal szFileName As String, ByVal dwReserved As Integer, ByVal lpfnCB As IntPtr) As Integer
    Private Declare Function DeleteUrlCacheEntry Lib "Wininet.dll" Alias "DeleteUrlCacheEntryA" (ByVal lpszUrlName As String) As Long
    Private m_intExcessBirthBaptCitations As Integer = 0
    Private m_intExcessMarriageCitations01 As Integer = 0
    Private m_intExcessMarriageCitations02 As Integer = 0
    Private m_intExcessMarriageCitations03 As Integer = 0
    Private m_intExcessMarriageCitations04 As Integer = 0
    Private m_intExcessMarriageCitations05 As Integer = 0
    Private m_bAdoptedName As Boolean = False
    Private m_bGivenNames As Boolean = False
    Private m_bLastName As Boolean = False
    Private m_bLoggedIn As Boolean = False
    Private m_bMarried1Name As Boolean = False
    Private m_bMarried2Name As Boolean = False
    Private m_bMarried3Name As Boolean = False
    Private m_bMarried4Name As Boolean = False
    Private m_bMarried5Name As Boolean = False
    Private m_bNoBaptismDate As Boolean = False
    Private m_bNoBirthDate As Boolean = False
    Private m_bNoBurialDate As Boolean = False
    Private m_bNoDeathDate As Boolean = False
    Private m_bNoDivorce1Date As Boolean = False
    Private m_bNoDivorce2Date As Boolean = False
    Private m_bNoDivorce3Date As Boolean = False
    Private m_bNoDivorce4Date As Boolean = False
    Private m_bNoDivorce5Date As Boolean = False
    Private m_bNoMarriage1Date As Boolean = False
    Private m_bNoMarriage2Date As Boolean = False
    Private m_bNoMarriage3Date As Boolean = False
    Private m_bNoMarriage4Date As Boolean = False
    Private m_bNoMarriage5Date As Boolean = False
    Private m_bSpouseGiven1Name As Boolean = False
    Private m_bSpouseGiven2Name As Boolean = False
    Private m_bSpouseGiven3Name As Boolean = False
    Private m_bSpouseGiven4Name As Boolean = False
    Private m_bSpouseGiven5Name As Boolean = False
    Private m_bThereAreNames As Boolean = False
    Private m_strDownloadErrorMessage As String = ""
    Private m_strMessagingFolder As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WkiTree Reader Messages"

    Friend Sub IncrementBirthBaptismCitations()
        m_intExcessBirthBaptCitations += 1
    End Sub

    Friend Sub IncrementMarriageCitations(marriageNumber As Integer)
        Select Case marriageNumber
            Case 1
                m_intExcessMarriageCitations01 += 1
            Case 2
                m_intExcessMarriageCitations02 += 1
            Case 3
                m_intExcessMarriageCitations03 += 1
            Case 4
                m_intExcessMarriageCitations04 += 1
            Case 5
                m_intExcessMarriageCitations05 += 1
        End Select
    End Sub

    Friend Property ExcessBirthBaptismCitations As Integer
        Get
            Return m_intExcessBirthBaptCitations
        End Get
        Set(value As Integer)
            If value <> 0 Then
                value = 0
            End If
            m_intExcessBirthBaptCitations = value
        End Set
    End Property

    Friend Property ExcessMarriageCitations(marriageNumber As Integer) As Integer
        Get
            Select Case marriageNumber
                Case 1
                    Return m_intExcessMarriageCitations01
                Case 2
                    Return m_intExcessMarriageCitations02
                Case 3
                    Return m_intExcessMarriageCitations03
                Case 4
                    Return m_intExcessMarriageCitations04
                Case 5
                    Return m_intExcessMarriageCitations05
                Case Else
                    Return 0
            End Select
        End Get
        Set(value As Integer)
            If value <> 0 Then
                value = 0
            End If
            Select Case marriageNumber
                Case 1
                    m_intExcessMarriageCitations01 = value
                Case 2
                    m_intExcessMarriageCitations02 = value
                Case 3
                    m_intExcessMarriageCitations03 = value
                Case 4
                    m_intExcessMarriageCitations04 = value
                Case 5
                    m_intExcessMarriageCitations05 = value
            End Select
        End Set
    End Property

    Friend Function MessagesFolder() As String
        Return m_strMessagingFolder
    End Function

    Friend Property ThereAreNames() As Boolean
        Get
            Return m_bThereAreNames
        End Get
        Set(value As Boolean)
            m_bThereAreNames = value
        End Set
    End Property

    Friend Property NoBirthDate() As Boolean
        Get
            Return m_bNoBirthDate
        End Get
        Set(value As Boolean)
            m_bNoBirthDate = value
        End Set
    End Property

    Friend Property NoBaptismDate() As Boolean
        Get
            Return m_bNoBaptismDate
        End Get
        Set(value As Boolean)
            m_bNoBaptismDate = value
        End Set
    End Property

    Friend Property NoMarriageDate(ByVal MarriageNumber As Integer) As Boolean
        Get
            Dim bRTN As Boolean = False
            Select Case MarriageNumber
                Case 1
                    bRTN = m_bNoMarriage1Date
                Case 2
                    bRTN = m_bNoMarriage2Date
                Case 3
                    bRTN = m_bNoMarriage3Date
                Case 4
                    bRTN = m_bNoMarriage4Date
                Case 5
                    bRTN = m_bNoMarriage5Date
            End Select
            Return bRTN
        End Get
        Set(value As Boolean)
            Select Case MarriageNumber
                Case 1
                    m_bNoMarriage1Date = value
                Case 2
                    m_bNoMarriage2Date = value
                Case 3
                    m_bNoMarriage3Date = value
                Case 4
                    m_bNoMarriage4Date = value
                Case 5
                    m_bNoMarriage5Date = value
            End Select
        End Set
    End Property

    Friend Property NoDivorceDate(ByVal DivorceNumber As Integer) As Boolean
        Get
            Dim bRTN As Boolean = False
            Select Case DivorceNumber
                Case 1
                    bRTN = m_bNoDivorce1Date
                Case 2
                    bRTN = m_bNoDivorce2Date
                Case 3
                    bRTN = m_bNoDivorce3Date
                Case 4
                    bRTN = m_bNoDivorce4Date
                Case 5
                    bRTN = m_bNoDivorce5Date
            End Select
            Return bRTN
        End Get
        Set(value As Boolean)
            Select Case DivorceNumber
                Case 1
                    m_bNoDivorce1Date = value
                Case 2
                    m_bNoDivorce2Date = value
                Case 3
                    m_bNoDivorce3Date = value
                Case 4
                    m_bNoDivorce4Date = value
                Case 5
                    m_bNoDivorce5Date = value
            End Select
        End Set
    End Property

    Friend Property GivenNames() As Boolean
        Get
            Return m_bGivenNames
        End Get
        Set(value As Boolean)
            m_bGivenNames = value
        End Set
    End Property

    Friend Property LastName() As Boolean
        Get
            Return m_bLastName
        End Get
        Set(value As Boolean)
            m_bLastName = value
        End Set
    End Property

    Friend Property AdoptedName() As Boolean
        Get
            Return m_bAdoptedName
        End Get
        Set(value As Boolean)
            m_bAdoptedName = value
        End Set
    End Property

    Friend Property MarriedName(ByVal MarriageNumber As Integer) As Boolean
        Get
            Dim bRTN As Boolean = False
            Select Case MarriageNumber
                Case 1
                    bRTN = m_bMarried1Name
                Case 2
                    bRTN = m_bMarried2Name
                Case 3
                    bRTN = m_bMarried3Name
                Case 4
                    bRTN = m_bMarried4Name
                Case 5
                    bRTN = m_bMarried5Name
            End Select
            Return bRTN
        End Get
        Set(value As Boolean)
            Select Case MarriageNumber
                Case 1
                    m_bMarried1Name = value
                Case 2
                    m_bMarried2Name = value
                Case 3
                    m_bMarried3Name = value
                Case 4
                    m_bMarried4Name = value
                Case 5
                    m_bMarried5Name = value
            End Select
        End Set
    End Property

    Friend Property SpouseGivenName(ByVal MarriageNumber As Integer) As Boolean
        Get
            Dim bRTN As Boolean = False
            Select Case MarriageNumber
                Case 1
                    bRTN = m_bSpouseGiven1Name
                Case 2
                    bRTN = m_bSpouseGiven2Name
                Case 3
                    bRTN = m_bSpouseGiven3Name
                Case 4
                    bRTN = m_bSpouseGiven4Name
                Case 5
                    bRTN = m_bSpouseGiven5Name
            End Select
            Return bRTN
        End Get
        Set(value As Boolean)
            Select Case MarriageNumber
                Case 1
                    m_bSpouseGiven1Name = value
                Case 2
                    m_bSpouseGiven2Name = value
                Case 3
                    m_bSpouseGiven3Name = value
                Case 4
                    m_bSpouseGiven4Name = value
                Case 5
                    m_bSpouseGiven5Name = value
            End Select
        End Set
    End Property

    Friend Property NoDeathDate() As Boolean
        Get
            Return m_bNoDeathDate
        End Get
        Set(value As Boolean)
            m_bNoDeathDate = value
        End Set
    End Property

    Friend Property NoBurialDate() As Boolean
        Get
            Return m_bNoBurialDate
        End Get
        Set(value As Boolean)
            m_bNoBurialDate = value
        End Set
    End Property

    ''' <summary>
    ''' Routine to format a date
    ''' </summary>
    ''' <param name="IncomingDate">An Asian formatted date</param>
    ''' <param name="FormatIndex">The index number of the combo box</param>
    ''' <param name="StyleIndex">The index number of the combo box</param>
    ''' <returns>Returns a formatted date string as a string value</returns>
    ''' <remarks>The incoming date is always in Asian format</remarks>
    Friend Function FormatDateString(ByVal IncomingDate As String, ByVal FormatIndex As Integer, ByVal StyleIndex As Integer) As String
        Dim nDay As Integer = 1
        Dim nMonth As Integer = 1
        Dim nYear As Integer = 1
        Dim strRTN As String = ""

        Try
            If IsDate(IncomingDate) = True Then
                Select Case FormatIndex
                    Case 0
                        'American (Month, Day, Year)
                        Select Case StyleIndex
                            Case 0
                                'Numeric day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("MMM DD, YYYY")
                            Case 1
                                'Numeric day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("MMMM DD, YYYY")
                            Case 2
                                'Day of Week, Numeric Day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("MMM DD (DDDD), YYYY")
                            Case 3
                                'Day of Week, Numeric Day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("MMMM DD (DDDD), YYYY")
                            Case 4
                                'All numeric (Not recomended)
                                strRTN = DateValue(IncomingDate).ToString("MM DD, YYYY")
                        End Select
                    Case 1
                        'European (Day, Month, Year)
                        Select Case StyleIndex
                            Case 0
                                'Numeric day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("DD MMM, YYYY")
                            Case 1
                                'Numeric day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("DD MMMM, YYYY")
                            Case 2
                                'Day of Week, Numeric Day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("DDDD DD MMM, YYYY")
                            Case 3
                                'Day of Week, Numeric Day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("DDDD DD MMMM, YYYY")
                            Case 4
                                'All numeric (Not recomended)
                                strRTN = DateValue(IncomingDate).ToString("DD MM, YYYY")
                        End Select
                    Case 2
                        'Asian (year, month, Day)
                        Select Case StyleIndex
                            Case 0
                                'Numeric day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("YYYY MMM DD")
                            Case 1
                                'Numeric day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("YYYY MMMM DD")
                            Case 2
                                'Day of Week, Numeric Day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("YYYY MMMM DD (DDDD)")
                            Case 3
                                'Day of Week, Numeric Day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("YYYY MMMM DD (DDDD)")
                            Case 4
                                'All numeric (Not recomended)
                                strRTN = DateValue(IncomingDate).ToString("YYYY MM DD")
                        End Select
                    Case Else
                        'American (Month, Day, Year)
                        Select Case StyleIndex
                            Case 0
                                'Numeric day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("MMM DD, YYYY")
                            Case 1
                                'Numeric day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("MMMM DD, YYYY")
                            Case 2
                                'Day of Week, Numeric Day, Short Month
                                strRTN = DateValue(IncomingDate).ToString("MMM DD (DDDD), YYYY")
                            Case 3
                                'Day of Week, Numeric Day, Long Month
                                strRTN = DateValue(IncomingDate).ToString("MMMM DD (DDDD), YYYY")
                            Case 4
                                'All numeric (Not recomended)
                                strRTN = DateValue(IncomingDate).ToString("MM DD, YYYY")
                        End Select
                End Select
            End If
        Catch ex As Exception
            strRTN = ""
        End Try
        Return strRTN
    End Function

'    Friend Sub ShowHelp(helpPage As String)
'        Dim baseAddress As String = "https://www.wikitree.com/wiki/Space:WikiTree_Text_Formatter"
'        Dim strTarget As String = ""
'        If helpPage.Trim = "" Then
'            strTarget = baseAddress
'        Else
'            strTarget = baseAddress & "#" & helpPage
'        End If
'        Try
'            Process.Start(strTarget)
'        Catch ex As Exception
'            MessageBox.Show("An error occurred trying to show the help page requested: " & helpPage & vbCrLf & vbCrLf & "If this persists please contact Meredith-1182", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

    Friend Function GetHash(theInput As String) As String

        Using hasher As MD5 = MD5.Create()    ' create hash object

            ' Convert to byte array and get hash
            Dim dbytes As Byte() =
                 hasher.ComputeHash(Encoding.UTF8.GetBytes(theInput))

            ' sb to create string from bytes
            Dim sBuilder As New StringBuilder()

            ' convert byte data to hex string
            For n As Integer = 0 To dbytes.Length - 1
                sBuilder.Append(dbytes(n).ToString("X2"))
            Next n

            Return sBuilder.ToString()
        End Using

    End Function

'    Friend Sub ShowUnderConstruction()
'        Dim frmUC As New FUnderConstruction
'        frmUC.ShowDialog()
'    End Sub

    Friend Function AppTitle() As String
        Return "WikiTree Text Formatter"
    End Function
End Module

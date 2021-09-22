Imports System.Text
Public Class FamilySearchTextBlockConverter
    Private m_listErrorMessages As New List(Of String)
    Private m_strSuppliedSpouse As String = ""
    Private m_strSuppliedFather As String = ""
    Private m_strSuppliedMother As String = ""
    Private m_strFormattedText As String = ""

    Public ReadOnly Property FormattedText() As String
        Get
            Return m_strFormattedText
        End Get
    End Property

    Public Function ErrorMessages() As List(Of String)
        Return m_listErrorMessages
    End Function

    Public Property SuppledFather As String
        Get
            Return m_strSuppliedFather
        End Get
        Set(value As String)
            m_strSuppliedFather = value
        End Set
    End Property

    Public Property SuppliedMother As String
        Get
            Return m_strSuppliedMother
        End Get
        Set(value As String)
            m_strSuppliedMother = value
        End Set
    End Property

    Public Property SuppliedSpouse As String
        Get
            Return m_strSuppliedSpouse
        End Get
        Set(value As String)
            m_strSuppliedSpouse = value
        End Set
    End Property

    Public Sub ReadFSText(TextLines() As String)
        ClearStorage()
        ParseFSBlock(TextLines)
    End Sub
    Public Sub ReadFSText(TextLines() As String, SuppliedPersons As List(Of String))
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim astr() As String = Nothing
        For Each strPerson In SuppliedPersons
            astr = strPerson.Split(Chr(198))
            Select Case astr(0)
                Case "father"
                    m_strSuppliedFather = astr(1)
                Case "mother"
                    m_strSuppliedMother = astr(1)
                Case "spouse"
                    m_strSuppliedSpouse = astr(1)
            End Select
        Next
        ClearStorage()
        ParseFSBlock(TextLines)
    End Sub
    Public Sub ParseFSBlock(IncomingText() As String)
        Dim strLine As String = ""
        For Each strLine In IncomingText
            If strLine.Contains("https://familysearch.org") = True Then
                IsFamilySearch = True
            End If
            If NextIsCitation = False Then
                'This collects data from each line
                ExtractLineData(strLine)
            Else
                Citation = strLine
                NextIsCitation = False
            End If

        Next
    End Sub
    Public Function TabulateFSBlock(IncomingText As String) As String
        Dim astr() As String = Nothing
        Dim astrSub() As String = Nothing
        Dim astrSubSub() As String = Nothing
        Dim bNextIsCitation As Boolean = False
        Dim bCreateHeader As Boolean = True
        Dim bSectioned As Boolean = True
        'This is set to the current default but may be altered 
        Dim bCompactStyle As Boolean = CompactFSBlocks()
        '
        Dim bIsFS As Boolean = False
        Dim listLines As New List(Of String)
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim sbldSub As StringBuilder = New StringBuilder("")
        Dim strCitation As String = ""
        Dim strEventMonth As String = ""
        Dim strText As String = IncomingText
        Dim strFormattedText As String = ""
        'Dim strTemp As String = ""


        m_listErrorMessages.Clear()
        If strText > "" Then
            sbld.Append(strText)
            sbld.Replace(vbCrLf, Chr(198))
            sbld.Replace(vbCr, Chr(198))
            sbld.Replace(vbLf, Chr(198))
            astr = sbld.ToString.Split(Chr(198))
            ClearStorage()
            For Each strLine In astr
                If strLine.Contains("https://familysearch.org") = True Then
                    bIsFS = True
                End If
                If NextIsCitation = False Then
                    ExtractLineData(strLine)
                Else
                    Citation = strLine
                    NextIsCitation = False
                End If
            Next
            If bIsFS = True Then
                If SpouseName.Trim = "" Then
                    SpouseName = m_strSuppliedSpouse
                End If
                If MothersName.Trim = "" Then
                    MothersName = m_strSuppliedMother
                End If
                If FathersName.Trim = "" Then
                    FathersName = m_strSuppliedFather
                End If
                If EventType = "" OrElse EventType = "Baptism" Then
                    'Cycle the other data to see if we can find the type
                    'Or in the case of Baptism see if it was actually a birth or vice versa
                    If EventType = "" Then

                    End If
                    If IsBirth = True Then
                        If BirthPlace > "" AndAlso (BirthYear > "" Or BirthDate > "") Then
                            EventType = "Birth"
                        Else
                            'It may be a baptism and both have got flagged because the citation is Births & Baptisms
                            If IsBaptism = True Then
                                If BaptismPlace > "" AndAlso (BirthYear > "" Or BirthDate > "") Then

                                End If
                            End If
                        End If

                    End If
                End If
                'The style of compact block will depend on the other layout settings
                If LayoutStyle() = 0 Then
                    Select Case TimelineStyle()
                        Case 0, 1
                            'Narrative
                            bSectioned = False
                            bCreateHeader = False
                            bCompactStyle = True
                    End Select
                End If
                If bCreateHeader = True Then
                    'Applies to all sectioned blocks
                    sbld.Replace(vbTab, "||")
                    astr = sbld.ToString.Split(Chr(198))
                    sbldSub.Clear()
                    sbldSub.Append("===")
                    If EventYear = "" Then
                        Select Case EventType
                            Case "Baptism"
                                EventYear = Year(BaptismDate)
                            Case "Birth"
                                EventYear = BirthYear
                            Case "Christening"

                        End Select
                    End If
                    sbldSub.Append(EventYear)
                    sbldSub.Append(" ")
                    Select Case EventType
                        Case "Baptism"
                            Select Case BaptismText()
                                Case 0
                                    EventType = "Baptism"
                                Case 1
                                    EventType = "Christening"
                            End Select
                    End Select
                    sbldSub.Append(EventType)
                    sbldSub.Append("===")
                    listLines.Add(sbldSub.ToString)
                End If
                sbldSub.Clear()
                sbldSub.Append(":")
                If bSectioned = False AndAlso bCreateHeader = False Then
                    'This applies to narrative style and dates bold styles
                    If TimelineStyle() = 0 Then
                        'This is narrative
                        sbldSub.Append("In ")
                        sbldSub.Append(EventYear)
                        sbldSub.Append(" ")

                    Else
                        'This is dates bold
                        sbldSub.Append("'''")
                        sbldSub.Append(EventYear)
                        sbldSub.Append("''' ")
                    End If
                End If
                If EventType.ToLower.Contains("registration") = True Then
                    'These generally are UK registrations
                    If EventType.ToLower = "birth registration" Then
                        sbldSub.Append("The birth of ")
                        sbldSub.Append(PersonName)
                        If m_strSuppliedFather.Trim > "" Then
                            Select Case Gender.Trim
                                Case "F"
                                    sbldSub.Append(", daughter of ")
                                Case "M"
                                    sbldSub.Append(", son of ")
                                Case Else
                                    sbldSub.Append(", child of ")
                            End Select
                            sbldSub.Append(m_strSuppliedFather)
                            If m_strSuppliedMother.Trim > "" Then
                                sbldSub.Append(" and ")
                                sbldSub.Append(m_strSuppliedMother)
                            End If
                            sbldSub.Append(",")
                        Else
                            If m_strSuppliedMother.Trim > "" Then
                                Select Case Gender.Trim
                                    Case "F"
                                        sbldSub.Append(", daughter of ")
                                    Case "M"
                                        sbldSub.Append(", son of ")
                                    Case Else
                                        sbldSub.Append(", child of ")
                                End Select
                                sbldSub.Append(m_strSuppliedMother)
                                sbldSub.Append(",")
                            End If
                        End If
                        sbldSub.Append(" was registered")
                    ElseIf EventType.ToLower = "marriage registration" Then
                        sbldSub.Append("The marriage of ")
                        sbldSub.Append(PersonName)
                        If SpouseName.Trim > "" Then
                            sbldSub.Append(" to ")
                            sbldSub.Append(SpouseName)
                        End If
                        sbldSub.Append(" was registered")
                    ElseIf EventType.ToLower = "death registration" Then
                        sbldSub.Append("The death of ")
                        sbldSub.Append(PersonName)
                        sbldSub.Append(" was registered")
                    End If
                Else
                    sbldSub.Append(PersonName)
                    If EventType.Trim = "" Then
                        If Citation.Contains("Marriage") = True Then
                            sbldSub.Append(" was married")
                            If SpouseName.Trim > "" Then
                                sbldSub.Append(" to ")
                                sbldSub.Append(SpouseName)
                            End If
                        End If
                    Else
                        Select Case EventType.ToLower
                            Case "birth"
                                If FathersName = "" Then
                                    FathersName = m_strSuppliedFather.Trim
                                End If
                                If MothersName = "" Then
                                    MothersName = m_strSuppliedMother.Trim
                                End If
                                If FathersName > "" Then
                                    Select Case Gender.Trim
                                        Case "F"
                                            sbldSub.Append(", daughter of ")
                                        Case "M"
                                            sbldSub.Append(", son of ")
                                        Case Else
                                            sbldSub.Append(", child of ")
                                    End Select
                                    sbldSub.Append(FathersName)
                                    If MothersName.Trim > "" Then
                                        sbldSub.Append(" and ")
                                        sbldSub.Append(MothersName)
                                    End If
                                    sbldSub.Append(",")
                                Else
                                    If MothersName > "" Then
                                        Select Case Gender.Trim
                                            Case "F"
                                                sbldSub.Append(", daughter of ")
                                            Case "M"
                                                sbldSub.Append(", son of ")
                                            Case Else
                                                sbldSub.Append(", child of ")
                                        End Select
                                        sbldSub.Append(MothersName)
                                        sbldSub.Append(",")
                                    End If
                                End If
                                sbldSub.Append(" was born")
                            Case "burial"
                                sbldSub.Append(" was laid to rest")
                            Case "baptism", "christening"
                                If FathersName = "" Then
                                    FathersName = m_strSuppliedFather.Trim
                                End If
                                If MothersName = "" Then
                                    MothersName = m_strSuppliedMother.Trim
                                End If
                                If FathersName > "" Then
                                    Select Case Gender.Trim
                                        Case "F"
                                            sbldSub.Append(", daughter of ")
                                        Case "M"
                                            sbldSub.Append(", son of ")
                                        Case Else
                                            sbldSub.Append(", child of ")
                                    End Select
                                    sbldSub.Append(FathersName)
                                    If MothersName.Trim > "" Then
                                        sbldSub.Append(" and ")
                                        sbldSub.Append(MothersName)
                                    End If
                                    sbldSub.Append(",")
                                Else
                                    If MothersName > "" Then
                                        Select Case Gender.Trim
                                            Case "F"
                                                sbldSub.Append(", daughter of ")
                                            Case "M"
                                                sbldSub.Append(", son of ")
                                            Case Else
                                                sbldSub.Append(", child of ")
                                        End Select
                                        sbldSub.Append(MothersName)
                                        sbldSub.Append(",")
                                    End If
                                End If
                                Select Case BaptismText()
                                    Case 0
                                        sbldSub.Append(" was baptised")
                                    Case 1
                                        sbldSub.Append(" was christened")
                                End Select
                            Case "death"
                                sbldSub.Append(" passed away")
                            Case "workhouse admission"
                                sbldSub.Append(" was admitted")
                            Case "marriage"
                                sbldSub.Append(" was married")
                                If SpouseName.Trim > "" Then
                                    sbldSub.Append(" to ")
                                    sbldSub.Append(SpouseName)
                                End If
                            Case "military service"
                                sbldSub.Append(" was in the ")
                                sbldSub.Append(MilitaryUnit)
                                If MilitaryBattalion > "" Then
                                    sbldSub.Append(", ")
                                    sbldSub.Append(MilitaryBattalion)
                                End If
                        End Select
                    End If
                End If
                If EventPlace = "" Then
                    Select Case EventType.ToLower
                        Case "birth"
                            EventPlace = BirthPlace
                        Case "burial"
                            EventPlace = BurialPlace
                        Case "baptism", "christening"
                            EventPlace = BaptismPlace
                        Case "death"
                            EventPlace = DeathPlace
                        Case "workhouse admission"
                            EventPlace = ""
                        Case "marriage"
                            EventPlace = MarriagePlace
                    End Select
                End If
                If EventPlace.Trim > "" Then
                    sbldSub.Append(" at ")
                    sbldSub.Append(EventPlace)
                End If
                If EventMonth.Trim = "" Then
                    'These have not been filled from the text no now we need to sort of which these were
                    Select Case EventType.ToLower
                        Case "birth"
                            If BirthDate.Trim > "" Then
                                If IsDate(BirthDate) = True Then
                                    CreateEventDatesFromDate(BirthDate)
                                End If
                            ElseIf BirthYear.Trim > "" Then
                                EventYear = BirthYear
                            End If
                        Case "burial"
                            If BurialDate.Trim > "" Then
                            ElseIf BurialDate.Trim > "" Then
                            End If
                        Case "baptism", "christening"
                            EventPlace = BaptismPlace
                        Case "death"
                            EventPlace = DeathPlace
                        Case "workhouse admission"
                            EventPlace = ""
                        Case "marriage"
                            EventPlace = MarriagePlace
                    End Select

                End If
                If EventMonth.Trim > "" Then 'Month is not converted for asian dates
                    If FullDatesFormat() <> 2 Then
                        If ShortMonths() = False Then
                            Select Case Val(EventMonth)
                                Case 1
                                    strEventMonth = "January"
                                Case 2
                                    strEventMonth = "February"
                                Case 3
                                    strEventMonth = "March"
                                Case 4
                                    strEventMonth = "April"
                                Case 5
                                    strEventMonth = "May"
                                Case 6
                                    strEventMonth = "June"
                                Case 7
                                    strEventMonth = "July"
                                Case 8
                                    strEventMonth = "August"
                                Case 9
                                    strEventMonth = "September"
                                Case 10
                                    strEventMonth = "October"
                                Case 11
                                    strEventMonth = "November"
                                Case 12
                                    strEventMonth = "December"
                                Case Else
                                    strEventMonth = ""
                            End Select
                        Else
                            Select Case Val(EventMonth)
                                Case 1
                                    strEventMonth = "Jan"
                                Case 2
                                    strEventMonth = "Feb"
                                Case 3
                                    strEventMonth = "Mar"
                                Case 4
                                    strEventMonth = "Apr"
                                Case 5
                                    strEventMonth = "May"
                                Case 6
                                    strEventMonth = "Jun"
                                Case 7
                                    strEventMonth = "Jul"
                                Case 8
                                    strEventMonth = "Aug"
                                Case 9
                                    strEventMonth = "Sep"
                                Case 10
                                    strEventMonth = "Oct"
                                Case 11
                                    strEventMonth = "Nov"
                                Case 12
                                    strEventMonth = "Dec"
                                Case Else
                                    strEventMonth = ""
                            End Select
                        End If
                    End If
                    If EventDay.Trim > "" AndAlso strEventMonth > "" Then
                        Select Case FullDatesFormat()
                            Case 0
                                'European
                                If OrdinalIndicators() = True Then
                                    'Days are not padded when using ordinal indicators
                                    sbldSub.Append(" on the ")
                                    sbldSub.Append(EventDay)
                                    Select Case Val(EventDay)
                                        Case 1, 21, 31
                                            sbldSub.Append("st")
                                        Case 2, 22
                                            sbldSub.Append("nd")
                                        Case 3, 23
                                            sbldSub.Append("rd")
                                        Case 4 To 20, 24 To 30
                                            sbldSub.Append("th")
                                    End Select
                                    sbldSub.Append(" of ")
                                    sbldSub.Append(strEventMonth)
                                    sbldSub.Append(", ")
                                    sbldSub.Append(EventYear)
                                Else
                                    'Days may or may not be padded when not using ordinal indicators
                                    sbldSub.Append(" on ")
                                    If PadSingleDigits() = True Then
                                        'Padded days
                                        sbldSub.Append(EventDay.PadLeft(2, "0"))
                                    Else
                                        sbldSub.Append(EventDay)
                                    End If
                                    sbldSub.Append(" ")
                                    sbldSub.Append(strEventMonth)
                                    If OrdinalIndicators() = False AndAlso ShortMonths() = True Then
                                        sbldSub.Append(" ")
                                    Else
                                        sbldSub.Append(", ")
                                    End If
                                    sbldSub.Append(EventYear)
                                End If
                            Case 1
                                'US
                                sbldSub.Append(" on ")
                                sbldSub.Append(strEventMonth)
                                sbldSub.Append(" ")
                                If OrdinalIndicators() = True Then
                                    'Days are not padded when using ordinal indicators
                                    sbldSub.Append(EventDay)
                                    Select Case Val(EventDay)
                                        Case 1, 21, 31
                                            sbldSub.Append("st")
                                        Case 2, 22
                                            sbldSub.Append("nd")
                                        Case 3, 23
                                            sbldSub.Append("rd")
                                        Case 4 To 20, 24 To 30
                                            sbldSub.Append("th")
                                    End Select
                                Else
                                    'Days may or may not be padded when not using ordinal indicators
                                    If PadSingleDigits() = True Then
                                        'Padded days
                                        sbldSub.Append(EventDay.PadLeft(2, "0"))
                                    Else
                                        sbldSub.Append(EventDay)
                                    End If
                                End If
                                sbldSub.Append(", ")
                                sbldSub.Append(EventYear)
                            Case 2
                                'Asian
                                sbldSub.Append(" ")
                                sbldSub.Append(EventYear)
                                sbldSub.Append("-")
                                sbldSub.Append(EventMonth.PadLeft(2, "0"))
                                sbldSub.Append("-")
                                sbldSub.Append(EventDay.PadLeft(2, "0"))
                        End Select
                    Else
                        If EventMonth.Trim > "" Then
                            Select Case FullDatesFormat()
                                Case 0, 1
                                    'European and US
                                    sbldSub.Append(" in ")
                                    sbldSub.Append(strEventMonth)
                                    sbldSub.Append(" ")
                                    sbldSub.Append(EventYear)
                                Case 2
                                    'ASian
                                    sbldSub.Append(" in ")
                                    sbldSub.Append(EventYear)
                                    sbldSub.Append("-")
                                    sbldSub.Append(EventMonth)
                            End Select
                        Else
                            EventDay = ""
                            EventMonth = ""
                            sbldSub.Append(" in ")
                            sbldSub.Append(EventYear)
                        End If
                    End If
                Else
                    EventDay = ""
                    EventMonth = ""
                    sbldSub.Append(" in ")
                    sbldSub.Append(EventYear)
                End If
                sbldSub.Append(".<ref>")
                sbldSub.Append(Citation)
                sbldSub.Append("</ref>")
                listLines.Add(sbldSub.ToString)
                If bCompactStyle = False Then
                    'Basically all this means is that the table part is omitted
                    listLines.Add(":{|")
                    bNextIsCitation = False
                    For Each strLine In astr
                        If bNextIsCitation = False Then
                            If strLine.Contains("Citing this Record:") = True Then
                                bNextIsCitation = True
                            Else
                                If strLine.Trim > "" Then
                                    sbldSub.Clear()
                                    sbldSub.Append(strLine)
                                    sbldSub.Replace("||", Chr(198))
                                    astrSub = sbldSub.ToString.Split(Chr(198))
                                    If astrSub.Length = 2 Then
                                        If astrSub(1).Trim > "" Then
                                            listLines.Add("|" & strLine)
                                            listLines.Add("|-")
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            Exit For
                        End If
                    Next
                    listLines(listLines.Count - 1) = "|}"
                End If
                '
                Clipboard.Clear()
                If listLines.Count > 0 Then
                    sbld.Clear()
                    For Each strLine In listLines
                        sbld.AppendLine(strLine)
                    Next
                    strFormattedText = sbld.ToString
                    '                    Clipboard.SetText(sbld.ToString)
                Else
                    m_listErrorMessages.Add("There was problem. Nothing was stored.")
                    strFormattedText = ""
                End If
            Else
                m_listErrorMessages.Add("Sorry, this does not appear to be a block of Family Search text.")
                strFormattedText = ""
            End If
        End If
        Return strFormattedText
    End Function
End Class

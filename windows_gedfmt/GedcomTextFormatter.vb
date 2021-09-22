Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Public Class GedcomTextFormatter
    Private cSecs As New sections
    Private cSrc As New sources
    Private cOut As New outSections
    Private IncomingText() As String
    Private m_strBioText As String = ""
    'Private listSourceSections As New List(Of ListsClasses.TI)

    Public Function BioText() As String
        Return m_strBioText
    End Function
    Public WriteOnly Property GedcomGeneratedText() As String()
        Set(value As String())
            IncomingText = value
        End Set
    End Property
    Public WriteOnly Property TargetWTreeRef As String
        Set(value As String)
            cSecs.MainWikiTreeID = value
        End Set
    End Property
    Public WriteOnly Property TargetName As String
        Set(value As String)
            cSecs.MainName = value
        End Set
    End Property
    Public WriteOnly Property FatherWTreeRef As String
        Set(value As String)
            cSecs.FatherWikiTreeID = value
        End Set
    End Property
    Public WriteOnly Property FatherName As String
        Set(value As String)
            cSecs.FatherName = value
        End Set
    End Property
    Public WriteOnly Property MotherWTreeRef As String
        Set(value As String)
            cSecs.MotherWikiTreeID = value
        End Set
    End Property
    Public WriteOnly Property MotherName As String
        Set(value As String)
            cSecs.MotherName = value
        End Set
    End Property

    Public Sub ParseSources()
        Dim astr() As String = Nothing
        Dim astrSub() As String = Nothing
        Dim bBirthSectionPassed As Boolean = False
        Dim bBirthDatePassed As Boolean = False
        Dim bBirthPlaceWithRefPassed As Boolean = False
        Dim bInEventMode As Boolean = False
        Dim bInSection As Boolean = False
        Dim bNoInfoFound As Boolean = False
        Dim bHusbandPassed As Boolean = False
        Dim bPassedSource As Boolean = False
        Dim bPreBioHeading As Boolean = True
        Dim intBiographySectionsCount As Integer = 0
        Dim intCounter As Integer = 0
        Dim intCounter2 As Integer = 0
        Dim intCurrentIndex As Integer = 0
        Dim intSourceSectionsCount As Integer = 0
        Dim listOutput As New List(Of String)
        Dim listRepositories As New List(Of ListsClasses.T2)
        Dim listTempLines As New List(Of String)
        Dim listYearOrder As New List(Of String)
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim sbldTemp As StringBuilder = New StringBuilder("")
        Dim sectionEnum As IEnumerator = cSecs.GetEnumerator()
        Dim sourceEnum As IEnumerator = cSrc.GetEnumerator()
        Dim strCurrentEventIndex As String = ""
        Dim strCurrentMarriageIndex As String = ""
        Dim strCurrentResidenceIndex As String = ""
        Dim strLine As String = ""
        Dim strMainSection As String = ""
        Dim strNewReference As String = ""
        Dim strOldReference As String = ""
        Dim strTemp As String = ""
        Dim thisSection As section
        Dim thisSRC As source

        For intloop = 0 To IncomingText.Length - 1
            If IncomingText(intloop).Trim.StartsWith("== Biography ==") = True Then
                intBiographySectionsCount += 1
            ElseIf IncomingText(intloop).Trim.StartsWith("== Sources ==") = True Then
                intSourceSectionsCount += 1
            ElseIf IncomingText(intloop).Contains(" CONT ") = True Then
                sbld.Clear()
                sbld.Append(IncomingText(intloop))
                sbld.Replace(" CONT ", "<br />")
                IncomingText(intloop) = sbld.ToString
            ElseIf IncomingText(intloop).Contains("''This biography is a rough draft. It was auto-generated") = True Then
                'This is a standalone line and can be removed
                IncomingText(intloop) = ""
            End If
        Next
        For intloop = IncomingText.Length - 1 To 0 Step -1
            If IncomingText(intloop).Trim.StartsWith("* Source: <span id='") = True Then
                '* Source: <span id='S13'>S13</span> 
                sbld.Clear()
                sbld.Append(IncomingText(intloop))
                sbld.Replace("* Source: <span id='", "")
                sbld.Replace("'>", Chr(198))
                sbld.Replace("</span>", Chr(198))
                astr = sbld.ToString.Split(Chr(198))
                'This should now have three parts
                'Create the source using part 0
                intCounter = cSrc.Add(astr(0))
                'Part 1 is identical to part 0 but part 2 holds the text
                sbld.Clear()
                sbld.Append(astr(2))
                sbld.Replace("Type:  PHOTO Slideshow:  Y", "")
                sbld.Replace("Type:  PHOTO Slideshow:  N", "")
                ' Primary or Preferred:  N
                sbld.Replace("Primary or Preferred:  N", "")
                sbld.Replace("Primary or Preferred:  Y", "")
                sbld.Replace("Scrapbook:  Y", "")
                sbld.Replace("Scrapbook:  N", "")
                sbld.Replace("File:   Title:   Note:", "")
                sbld.Replace("External File:   Format:   ", "")
                sbld.Replace("Publication:  Name: Ancestry.com Operations Inc; Location: Provo, UT, USA; ", "")
                'What is left should usable text
                If sbld.ToString.Contains("Full Text:") = True Then
                    sbld.Replace("Full Text:", Chr(198))
                    astr = sbld.ToString.Split(Chr(198))
                    sbld.Clear()
                    sbld.Append(astr(0))
                End If
                cSrc(intCounter).SourceText = sbld.ToString.Trim
                cSrc(intCounter).SourceInLineCode = "<ref name=" & Chr(34) & cSrc(intCounter).ID & Chr(34) & " />"
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Trim.StartsWith("* Repository: <span id='") = True Then
                '* Repository: <span id='REPO6'>REPO6</span> Name:  Ancestry.com
                sbld.Clear()
                sbld.Append(IncomingText(intloop))
                sbld.Replace("* Repository: <span id='", "")
                sbld.Replace("</span>", "")
                sbld.Replace("Name:", Chr(198))
                sbld.Replace("'", "")
                astr = sbld.ToString.Split(Chr(198))
                astrSub = astr(0).Split(">")
                listRepositories.Add(New ListsClasses.T2(astrSub(0), astr(1).Trim))
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop) = "== Sources ==" Then
                If intSourceSectionsCount > 1 AndAlso bPassedSource = False Then
                    bPassedSource = True
                ElseIf intSourceSectionsCount > 1 AndAlso bPassedSource = True Then
                    bPassedSource = False
                    Exit For
                Else
                    Exit For
                End If
            End If
        Next
        'Now parse all the sources and replace the repository with the details
        '[[#REPO6]]
        For Each element In listRepositories
            sourceEnum.Reset()
            While sourceEnum.MoveNext()
                thisSRC = sourceEnum.Current()
                If thisSRC.SourceText.Trim.Contains(element.FirstValue.Trim) = True Then
                    sbld.Clear()
                    sbld.Append(thisSRC.SourceText)
                    strTemp = "[[#" & element.FirstValue & "]]"
                    sbld.Replace(strTemp, element.SecondValue.Trim)
                    thisSRC.SourceText = sbld.ToString
                End If
            End While
            thisSRC = Nothing
        Next
        For intloop = 0 To IncomingText.Length - 1
            If IncomingText(intloop).Contains("auto-generated by a GEDCOM") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains("...  He passed away") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains("''Can you add any information") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains("''No sources. The events of") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains("<!-- Please edit, add, or delete anything in this text") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains("''No more info is currently available. Can you add") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains(":: User ID:") = True Then
                IncomingText(intloop) = ""
            ElseIf IncomingText(intloop).Contains(":: Record ID Number:") = True Then
                IncomingText(intloop) = ""
            End If
        Next
        'Now the text is split into sections
        'A prebio section is created just in case
        intCurrentIndex = cSecs.Add("PreBiography")
        For intloop = 0 To IncomingText.Length - 1
            If IncomingText(intloop).StartsWith("== ") = True Then
                'This is a major level 1 section like Biography or Sources
                sbld.Clear()
                sbld.Append(IncomingText(intloop))
                sbld.Replace("=", "")
                intCurrentIndex = cSecs.Add(sbld.ToString.Trim)
                If sbld.ToString.Trim.StartsWith("Bio") = True Then
                    bPreBioHeading = False
                End If
                cSecs(intCurrentIndex).SectionLevel = 1
                strMainSection = cSecs(intCurrentIndex).ID
                bInSection = True
            ElseIf IncomingText(intloop).StartsWith("=== ") = True Then
                If IncomingText(intloop).Contains("Data Changed") = True OrElse IncomingText(intloop).Contains("External File") = True OrElse IncomingText(intloop).Contains("Reference") = True OrElse IncomingText(intloop).Contains("User ID") = True Then
                    'We skip this one
                    bInSection = False
                Else
                    'This is a level 2 section
                    strTemp = cSecs(intCurrentIndex).ID
                    sbld.Clear()
                    sbld.Append(IncomingText(intloop))
                    sbld.Replace("=", "")
                    If sbld.ToString = "Census" Then
                        intCurrentIndex = cSecs.Add("Residence")
                    Else
                        intCurrentIndex = cSecs.Add(sbld.ToString.Trim)
                    End If
                    cSecs(intCurrentIndex).SectionLevel = 2
                    cSecs(intCurrentIndex).ParentSection = strMainSection
                    bInSection = True
                End If
            Else
                If bInSection = True Then
                    If IncomingText(intloop).Trim > "" Then
                        cSecs(intCurrentIndex).Lines.Add(IncomingText(intloop))
                    End If
                Else
                    If bPreBioHeading = True Then
                        cSecs(intCurrentIndex).Lines.Add(IncomingText(intloop))
                    End If
                End If
            End If
        Next
        'Now we can write these back oot again in the right order
        'Sections can be 
        'Biography. Don't ignore this it may have data
        ' Birth
        ' Death
        ' Occupation
        ' Residence = may have many subsections
        sectionEnum.Reset()
        While sectionEnum.MoveNext
            thisSection = sectionEnum.Current
            Select Case thisSection.ID.ToLower
                Case "acknowledgements"
                    For Each strLine In thisSection.Lines
                        cOut.Acknowledgements.OtherLines.Add(RemoveLeadColons(strLine))
                    Next
                Case "baptism", "christening"
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        sbld.Clear()
                        sbld.Append(strLine)
                        If strLine.Trim.StartsWith(":: Date:") Then
                            sbld.Replace(":: Date:", "")
                            strTemp = sbld.ToString.Trim
                            If IsDate(strTemp) = True Then
                                'This is a valid date
                                'Setting this automatically sets the year of the section
                                cOut.BaptismData.SectionDate = CDate(strTemp)
                                cOut.BaptismData.DateStatus = DATE_CERT
                            Else
                                cOut.BaptismData.DateStatus = ParsePartialDate(strTemp)
                                'strTemp has been 'Fixed'
                                cOut.BaptismData.SectionDateString = strTemp

                            End If
                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                            sbld.Replace(":: Place:", "")
                            cOut.BaptismData.SectionPlace = sbld.ToString.Trim
                        Else
                            cOut.BaptismData.OtherLines.Add(RemoveLeadColons(strLine))
                        End If
                    Next
                Case "biography"
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        cOut.BiographyLines.OtherLines.Add(RemoveLeadColons(strLine))
                    Next
                Case "birth"
                    listTempLines.Clear()
                    For Each strLine In thisSection.Lines
                        listTempLines.Add(strLine)
                    Next
                    For Each strLine In thisSection.Lines
                        If strLine.Trim.StartsWith(": Birth:") = False Then
                            If CheckForMainID(strLine, strTemp) = True Then
                                cSecs.MainName = strTemp
                            End If
                            If CheckForFatherID(strLine, strTemp) = True Then
                                cSecs.FatherName = strTemp
                            End If
                            If CheckForMotherID(strLine, strTemp) = True Then
                                cSecs.MotherName = strTemp
                            End If
                            sbld.Clear()
                            sbld.Append(strLine)
                            If strLine.Trim.StartsWith(":: Date:") Then
                                If bBirthDatePassed = False Then
                                    sbld.Replace(":: Date:", "")
                                    strTemp = sbld.ToString.Trim
                                    If IsDate(strTemp) = True Then
                                        'This is a valid date
                                        'Setting this automatically sets the year of the section
                                        cOut.BirthData.SectionDate = CDate(strTemp)
                                        cOut.BirthData.DateStatus = DATE_CERT
                                    Else
                                        cOut.BirthData.DateStatus = ParsePartialDate(strTemp)
                                        'strTemp has been 'Fixed'
                                        cOut.BirthData.SectionDateString = strTemp
                                    End If
                                    bBirthDatePassed = True
                                ElseIf bBirthPlaceWithRefPassed = True Then
                                    'Mm, we have a date and place with a reference so this is possibly spurious
                                    sbld.Replace(":: Date:", "")
                                    strTemp = sbld.ToString.Trim
                                    If IsDate(strTemp) = True Then
                                        If cOut.BirthData.SectionDate <> CDate(strTemp) Then
                                            cOut.BirthData.OtherLines.Add("Additional birth date recorded: " & strTemp)
                                        End If
                                    End If
                                End If
                            ElseIf strLine.Trim.StartsWith(":: Place:") Then
                                sbld.Replace(":: Place:", "")
                                If bBirthPlaceWithRefPassed = False Then
                                    cOut.BirthData.SectionPlace = sbld.ToString.Trim
                                    If sbld.ToString.Contains("<ref>Source: [[#") Then
                                        bBirthPlaceWithRefPassed = True
                                    End If
                                Else
                                    If cOut.BirthData.SectionPlace.StartsWith(sbld.ToString) = False Then
                                        cOut.BirthData.OtherLines.Add("Additional birth place recorded: " & sbld.ToString)
                                    End If
                                End If
                            Else
                                cOut.BirthData.OtherLines.Add(RemoveLeadColons(strLine))
                            End If
                        End If
                    Next
                Case "burial"
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        sbld.Clear()
                        sbld.Append(strLine)
                        If strLine.Trim.StartsWith(":: Date:") Then
                            sbld.Replace(":: Date:", "")
                            strTemp = sbld.ToString.Trim
                            If IsDate(strTemp) = True Then
                                'This is a valid date
                                'Setting this automatically sets the year of the section
                                cOut.BurialData.SectionDate = CDate(strTemp)
                                cOut.BurialData.DateStatus = DATE_CERT
                            Else
                                cOut.BurialData.DateStatus = ParsePartialDate(strTemp)
                                'strTemp has been 'Fixed'
                                cOut.BurialData.SectionDateString = strTemp

                            End If
                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                            sbld.Replace(":: Place:", "")
                            cOut.BurialData.SectionPlace = sbld.ToString.Trim
                        Else
                            cOut.BurialData.OtherLines.Add(RemoveLeadColons(strLine))
                        End If
                    Next
                Case "census", "residence"
                    ': Residence:  Marital Status: Married; Relation to Head of House: Head
                    ':: Date:  1881
                    ':: Place:  19 Gt Francis St, Aston, Birmingham, Warwickshire<ref>Source: [[#S33]] </ref><ref>Source: [[#S13]]  Page:  Class: RG11; Piece: 3032; 
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        sbld.Clear()
                        sbld.Append(strLine.Trim)
                        If strLine.Trim.StartsWith(":: Date:") Then
                            sbld.Replace(":: Date:", "")
                            strTemp = sbld.ToString.Trim
                            If IsDate(strTemp) = True Then
                                strCurrentResidenceIndex = Year(strTemp).ToString
                                cOut.ResidenceData.Add(strCurrentResidenceIndex)
                                cOut.ResidenceData(strCurrentResidenceIndex).SectionDate = CDate(strTemp)
                                cOut.ResidenceData(strCurrentResidenceIndex).DateStatus = DATE_CERT
                            Else
                                strCurrentResidenceIndex = strTemp.Trim
                                cOut.ResidenceData.Add(strCurrentResidenceIndex)
                                cOut.ResidenceData(strCurrentResidenceIndex).DateStatus = DATE_CERT
                            End If
                            If sbldTemp.Length > 0 Then
                                cOut.ResidenceData(strCurrentResidenceIndex).OtherLines.Add(sbldTemp.ToString)
                            End If
                            sbldTemp.Clear()
                            If listTempLines.Count > 0 Then
                                For Each strTemp In listTempLines
                                    cOut.ResidenceData(strCurrentResidenceIndex).OtherLines.Add(strTemp)
                                Next
                                listTempLines.Clear()
                            End If
                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                            sbld.Replace(":: Place:", "")
                            cOut.ResidenceData(strCurrentResidenceIndex).SectionPlace = sbld.ToString.Trim
                        ElseIf strLine.Trim.StartsWith(": Residence:") Then
                            'This may be just 'residence' or may have other data
                            'If only residence ignore it otherwies hive off the data to the first line
                            'eith way this is a new residence once the year is known
                            strCurrentResidenceIndex = ""
                            sbldTemp.Clear()
                            If strLine.Trim.Length > 13 Then
                                sbldTemp.Append(strLine.Substring(13))
                            End If
                        Else
                            If strCurrentResidenceIndex.Trim > "" Then
                                cOut.ResidenceData(strCurrentResidenceIndex).OtherLines.Add(RemoveLeadColons(strLine))
                            Else
                                listTempLines.Add(RemoveLeadColons(strLine))
                            End If
                        End If
                    Next
                Case "children"
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        sbld.Clear()
                        sbld.Append(strLine)
                        If strLine.Trim.StartsWith(":: Date:") Then
                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                        End If
                    Next
                Case "death"
                    For Each strLine In thisSection.Lines
                        If strLine.Trim.StartsWith(": Death:") = False Then
                            If CheckForMainID(strLine, strTemp) = True Then
                                cSecs.MainName = strTemp
                            End If
                            If CheckForFatherID(strLine, strTemp) = True Then
                                cSecs.FatherName = strTemp
                            End If
                            If CheckForMotherID(strLine, strTemp) = True Then
                                cSecs.MotherName = strTemp
                            End If
                            sbld.Clear()
                            sbld.Append(strLine)
                            If strLine.Trim.StartsWith(":: Date:") Then
                                sbld.Replace(":: Date:", "")
                                strTemp = sbld.ToString.Trim
                                If IsDate(strTemp) = True Then
                                    'This is a valid date
                                    'Setting this automatically sets the year of the section
                                    cOut.DeathData.SectionDate = CDate(strTemp)
                                    cOut.DeathData.DateStatus = DATE_CERT
                                Else
                                    cOut.DeathData.DateStatus = ParsePartialDate(strTemp)
                                    'strTemp has been 'Fixed'
                                    cOut.DeathData.SectionDateString = strTemp

                                End If
                            ElseIf strLine.Trim.StartsWith(":: Place:") Then
                                sbld.Replace(":: Place:", "")
                                cOut.DeathData.SectionPlace = sbld.ToString.Trim
                            Else
                                cOut.DeathData.OtherLines.Add(RemoveLeadColons(strLine))
                            End If
                        End If
                    Next
                Case "divorce"
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        sbld.Clear()
                        sbld.Append(strLine)
                        If strLine.Trim.StartsWith(":: Date:") Then
                            sbld.Replace(":: Date:", "")
                            strTemp = sbld.ToString.Trim
                            If IsDate(strTemp) = True Then
                                'This is a valid date
                                'Setting this automatically sets the year of the section
                                cOut.DivorceData.SectionDate = CDate(strTemp)
                                cOut.DivorceData.DateStatus = DATE_CERT
                            Else
                                cOut.DivorceData.DateStatus = ParsePartialDate(strTemp)
                                'strTemp has been 'Fixed'
                                cOut.DivorceData.SectionDateString = strTemp

                            End If
                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                            sbld.Replace(":: Place:", "")
                            cOut.DivorceData.SectionPlace = sbld.ToString.Trim
                        Else
                            cOut.DivorceData.OtherLines.Add(RemoveLeadColons(strLine))
                        End If
                    Next
                Case "event"
                    'This is only for events with the level 3 heading === Event ====
                    listTempLines.Clear()
                    For Each strLine In thisSection.Lines
                        If strLine.Trim.StartsWith(": Event:") = False Then
                            If CheckForMainID(strLine, strTemp) = True Then
                                cSecs.MainName = strTemp
                            End If
                            If CheckForFatherID(strLine, strTemp) = True Then
                                cSecs.FatherName = strTemp
                            End If
                            If CheckForMotherID(strLine, strTemp) = True Then
                                cSecs.MotherName = strTemp
                            End If
                            sbld.Clear()
                            sbld.Append(strLine)
                            If strLine.Trim.StartsWith(":: Date:") Then
                                sbld.Replace(":: Date:", "")
                                strTemp = sbld.ToString.Trim
                                If IsDate(strTemp) = True Then
                                    'This is a valid date
                                    'Setting this automatically sets the year of the section
                                    strCurrentEventIndex = cOut.EventsData.Add(Year(strTemp).ToString)
                                    cOut.EventsData(strCurrentEventIndex).EventDate = CDate(strTemp)
                                    cOut.EventsData(strCurrentEventIndex).DateStatus = DATE_CERT
                                Else
                                    strCurrentEventIndex = cOut.EventsData.Add(strTemp)
                                    cOut.EventsData(strCurrentEventIndex).DateStatus = ParsePartialDate(strTemp)
                                    'strTemp has been 'Fixed'
                                    cOut.EventsData(strCurrentEventIndex).EventDateString = strTemp
                                End If
                                If listTempLines.Count > 0 Then
                                    For Each strTemp In listTempLines
                                        If strTemp.Contains("Type") = True Then
                                            sbld.Clear()
                                            sbld.Append(strTemp)
                                            sbld.Replace("Type:", "")
                                            cOut.EventsData(strCurrentEventIndex).EventType = strTemp.Trim
                                        ElseIf strTemp.Contains("Place") = True Then
                                            sbld.Clear()
                                            sbld.Append(strTemp)
                                            sbld.Replace("Place:", "")
                                            cOut.EventsData(strCurrentEventIndex).EventPlace = strTemp.Trim
                                        Else
                                            cOut.EventsData(strCurrentEventIndex).OtherLines.Add(strTemp)
                                        End If
                                    Next
                                    listTempLines.Clear()
                                End If
                            ElseIf strLine.Trim.StartsWith(":: Place:") Then
                                If strCurrentEventIndex > "" Then
                                    sbld.Replace(":: Place:", "")
                                    cOut.EventsData(strCurrentEventIndex).EventPlace = sbld.ToString.Trim
                                Else
                                    listTempLines.Add(RemoveLeadColons(strLine))
                                End If
                            ElseIf strLine.Trim.StartsWith(":: Type:") Then
                                If strCurrentEventIndex > "" Then
                                    sbld.Replace(":: Type:", "")
                                    cOut.EventsData(strCurrentEventIndex).EventType = sbld.ToString
                                Else
                                    listTempLines.Add(RemoveLeadColons(strLine))
                                End If
                            Else
                                If strCurrentEventIndex.Trim > "" Then
                                    cOut.EventsData(strCurrentEventIndex).OtherLines.Add(RemoveLeadColons(strLine))
                                Else
                                    listTempLines.Add(RemoveLeadColons(strLine))
                                End If
                            End If
                        Else
                            strCurrentEventIndex = ""
                            If listTempLines.Count > 0 Then
                                'An undated event, or one with an unrecognided date is left in the buffer
                                strCurrentEventIndex = cOut.EventsData.Add(Year(Today).ToString)
                                For Each strTemp In listTempLines
                                    If strTemp.Contains("Type") = True Then
                                        sbld.Clear()
                                        sbld.Append(strTemp)
                                        sbld.Replace("Type:", "")
                                        cOut.EventsData(strCurrentEventIndex).EventType = sbld.ToString.Trim
                                    ElseIf strTemp.Contains("Place") = True Then
                                        sbld.Clear()
                                        sbld.Append(strTemp)
                                        sbld.Replace("Place:", "")
                                        cOut.EventsData(strCurrentEventIndex).EventPlace = sbld.ToString.Trim
                                    Else
                                        cOut.EventsData(strCurrentEventIndex).OtherLines.Add(strTemp)
                                    End If
                                Next
                                listTempLines.Clear()
                                strCurrentEventIndex = ""
                            End If
                        End If
                    Next
                Case "general"
                    'This gets added to notes and placed at the end
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        cOut.NotesData.OtherLines.Add(RemoveLeadColons(strLine))
                    Next
                Case "marriage"
                    For Each strLine In thisSection.Lines
                        'The User ID line turns off the in event mode
                        If strLine.Trim.StartsWith(": User ID:") = False Then
                            'check for person
                            If CheckForMainID(strLine, strTemp) = True Then
                                cSecs.MainName = strTemp
                            End If
                            'check for father
                            If CheckForFatherID(strLine, strTemp) = True Then
                                cSecs.FatherName = strTemp
                            End If
                            'check for mother
                            If CheckForMotherID(strLine, strTemp) = True Then
                                cSecs.MotherName = strTemp
                            End If
                            'We don't need the marriage line it just acts as a switch
                            If strLine.StartsWith(": Marriage:") = True Then
                                bInEventMode = False
                                strCurrentMarriageIndex = ""
                            ElseIf strCurrentMarriageIndex > "" AndAlso strLine.Contains("Husband") = True Then
                                'This is the start of a new marriage
                                strCurrentMarriageIndex = ""
                                listTempLines.Add(RemoveLeadColons(strLine))
                            Else
                                'Get ready
                                sbld.Clear()
                                sbld.Append(strLine)
                                If strLine.StartsWith(": Event:") Then
                                    bInEventMode = True
                                    If strCurrentMarriageIndex.Trim > "" Then
                                        cOut.MarriageData(strCurrentMarriageIndex).OtherLines.Add(RemoveLeadColons(strLine))
                                    Else
                                        listTempLines.Add(RemoveLeadColons(strLine))
                                    End If
                                Else
                                    If bInEventMode = True Then
                                        If strCurrentMarriageIndex.Trim > "" Then
                                            cOut.MarriageData(strCurrentMarriageIndex).OtherLines.Add(RemoveLeadColons(strLine))
                                        Else
                                            listTempLines.Add(RemoveLeadColons(strLine))
                                        End If
                                    Else
                                        If strLine.Trim.StartsWith(":: Date:") Then
                                            sbld.Replace(":: Date:", "")
                                            strTemp = sbld.ToString.Trim
                                            If IsDate(strTemp) = True Then
                                                strCurrentMarriageIndex = Year(strTemp).ToString
                                                cOut.MarriageData.Add(strCurrentMarriageIndex)
                                                cOut.MarriageData(strCurrentMarriageIndex).SectionDate = CDate(strTemp)
                                                cOut.MarriageData(strCurrentMarriageIndex).DateStatus = DATE_CERT
                                            Else
                                                strCurrentMarriageIndex = strTemp.Trim
                                                cOut.MarriageData.Add(strCurrentMarriageIndex)
                                                cOut.MarriageData(strCurrentMarriageIndex).DateStatus = ParsePartialDate(strTemp)
                                            End If
                                            If sbldTemp.Length > 0 Then
                                                cOut.MarriageData(strCurrentMarriageIndex).OtherLines.Add(sbldTemp.ToString)
                                            End If
                                            If listTempLines.Count > 0 Then
                                                For Each strTemp In listTempLines
                                                    If strTemp.Contains("Husband:") = True Then
                                                        cOut.MarriageData(strCurrentMarriageIndex).Husband = strTemp.Substring(10).Trim
                                                        bHusbandPassed = True
                                                    ElseIf strTemp.Contains("Wife:") = True Then
                                                        cOut.MarriageData(strCurrentMarriageIndex).Wife = strTemp.Substring(7).Trim
                                                    ElseIf strTemp.Contains("Child:") = True Then
                                                        cOut.MarriageData(strCurrentMarriageIndex).Children.Add(strTemp.Substring(8).Trim)
                                                    Else
                                                        cOut.MarriageData(strCurrentMarriageIndex).OtherLines.Add(strTemp)
                                                    End If
                                                Next
                                                listTempLines.Clear()
                                            End If
                                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                                            sbld.Replace(":: Place:", "")
                                            cOut.MarriageData(strCurrentMarriageIndex).SectionPlace = sbld.ToString.Trim
                                        Else
                                            If strCurrentMarriageIndex.Trim > "" Then
                                                cOut.MarriageData(strCurrentMarriageIndex).OtherLines.Add(RemoveLeadColons(strLine))
                                            Else
                                                listTempLines.Add(RemoveLeadColons(strLine))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            bInEventMode = False
                            strCurrentMarriageIndex = ""
                        End If
                    Next
                Case "note", "notes", "research note", "research notes"
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        cOut.NotesData.OtherLines.Add(RemoveLeadColons(strLine))
                    Next
                Case "occupation"
                    'Puts this into bio
                    For Each strLine In thisSection.Lines
                        If CheckForMainID(strLine, strTemp) = True Then
                            cSecs.MainName = strTemp
                        End If
                        If CheckForFatherID(strLine, strTemp) = True Then
                            cSecs.FatherName = strTemp
                        End If
                        If CheckForMotherID(strLine, strTemp) = True Then
                            cSecs.MotherName = strTemp
                        End If
                        sbld.Clear()
                        sbld.Append(strLine)
                        If strLine.Trim.StartsWith(":: Date:") Then
                            sbld.Replace(":: Date:", "")
                            strTemp = sbld.ToString.Trim
                            If IsDate(strTemp) = True Then
                                'This is a valid date
                                'Setting this automatically sets the year of the section
                                cOut.BiographyLines.SectionDate = CDate(strTemp)
                                cOut.BiographyLines.DateStatus = DATE_CERT
                            Else
                                cOut.BiographyLines.DateStatus = ParsePartialDate(strTemp)
                                'strTemp has been 'Fixed'
                                cOut.BiographyLines.SectionDateString = strTemp
                            End If
                        ElseIf strLine.Trim.StartsWith(":: Place:") Then
                            sbld.Replace(":: Place:", "")
                            cOut.BiographyLines.SectionPlace = sbld.ToString.Trim
                        Else
                            cOut.BiographyLines.OtherLines.Add(RemoveLeadColons(strLine))
                        End If
                    Next
                Case "prebiography"
                    'This really only covers any stray line at the start that have probably been added later
                    For Each strLine In thisSection.Lines
                        cOut.PreBio.OtherLines.Add(RemoveLeadColons(strLine))
                    Next
                Case "sources"
                    'This really only covers any stray sources that have probably been added later
                    For Each strLine In thisSection.Lines
                        If strLine.Contains("created through the import of ") = True Then
                            'This is a gedcom import line that has got stuffed into the sources
                            sbld.Clear()
                            sbld.Append(strLine)
                            sbld.Replace("* ", "")
                            cOut.Acknowledgements.OtherLines.Add(sbld.ToString)
                        Else
                            cOut.SourcesData.OtherLines.Add(RemoveLeadColons(strLine))
                        End If
                    Next
            End Select
        End While
        'Collate the lines
        If cOut.PreBio.OtherLines.Count > 0 Then
            For Each strline In cOut.PreBio.OtherLines
                listOutput.Add(strline)
            Next
        End If
        If cSrc.Count = 0 Then
            listOutput.Add("{{unsourced}}")
        End If
        listOutput.Add("==Biography==")
        If cOut.BiographyLines.OtherLines.Count > 0 Then
            For Each strline In cOut.BiographyLines.OtherLines
                listOutput.Add(":" & strLine)
                If strLine.Contains("No more info is currently available") = True Then
                    bNoInfoFound = True
                End If
            Next
        End If
        'Before writing the timeline we need tp determine the correct order.
        'This can be done from the section years
        If cOut.BaptismData.SectionYear > 0 Then
            listYearOrder.Add(cOut.BaptismData.SectionYear.ToString & "|" & "baptism")
        End If
        If cOut.BirthData.SectionYear > 0 Then
            listYearOrder.Add(cOut.BirthData.SectionYear.ToString & "|" & "birth")
        End If
        If cOut.BurialData.SectionYear > 0 Then
            listYearOrder.Add(cOut.BurialData.SectionYear.ToString & "|" & "burial")
        End If
        If cOut.DeathData.SectionYear > 0 Then
            listYearOrder.Add(cOut.DeathData.SectionYear.ToString & "|" & "death")
        End If
        If cOut.DivorceData.SectionYear > 0 Then
            listYearOrder.Add(cOut.DivorceData.SectionYear.ToString & "|" & "divorce")
        End If
        If cOut.EventsData.Count > 0 Then
            For intloop = 1 To cOut.EventsData.Count
                If cOut.EventsData(intloop).EventYear > 0 Then
                    listYearOrder.Add(cOut.EventsData(intloop).EventYear.ToString & "|event")
                End If
            Next
        End If
        If cOut.MarriageData.Count > 0 Then
            For intloop = 1 To cOut.MarriageData.Count
                If cOut.MarriageData(intloop).SectionYear > 0 Then
                    listYearOrder.Add(cOut.MarriageData(intloop).SectionYear.ToString & "|marriage")
                End If
            Next
        End If
        'Notes data is alsways used
        If cOut.ResidenceData.Count > 0 Then
            For intloop = 1 To cOut.ResidenceData.Count
                If cOut.ResidenceData(intloop).SectionYear > 0 Then
                    listYearOrder.Add(cOut.ResidenceData(intloop).SectionYear.ToString & "|residence")
                End If
            Next
        End If
        If listYearOrder.Count > 0 Then
            listYearOrder.Sort()
        End If
        listOutput.Add("==Timeline==")
        For Each strYearIndex In listYearOrder
            astr = strYearIndex.Split("|")
            Select Case astr(1)
                Case "baptism"
                    If cOut.BaptismData.HasData = True Then
                        If cOut.BaptismData.SectionYear > 0 Then
                            listOutput.Add("===" & cOut.BaptismData.SectionYear.ToString & " Baptism===")
                        Else
                            listOutput.Add("===Baptism===")
                        End If
                        sbld.Clear()
                        sbld.Append(":")
                        If cSecs.MainName.Trim > "" Then
                            sbld.Append(cSecs.MainName)
                            If cSecs.FatherName.Trim > "" Then
                                sbld.Append(", child of ")
                                sbld.Append(cSecs.FatherName)
                            End If
                            If cSecs.MotherName.Trim > "" Then
                                If cSecs.FatherName.Trim > "" Then
                                    sbld.Append(" and ")
                                Else
                                    sbld.Append(", child of ")
                                End If
                                sbld.Append(cSecs.MotherName)
                            End If
                        Else
                            sbld.Append("''[enter name here]''")
                        End If
                        sbld.Append(" was baptised")
                        Select Case cOut.BaptismData.DateStatus
                            Case DATE_ABT
                                sbld.Append(" about ")
                                sbld.Append(cOut.BaptismData.SectionDateString)
                            Case DATE_AFT
                                sbld.Append(" after ")
                                sbld.Append(cOut.BaptismData.SectionDateString)
                            Case DATE_BFR
                                sbld.Append(" before ")
                                sbld.Append(cOut.BaptismData.SectionDateString)
                            Case DATE_CERT
                                sbld.Append(" on ")
                                sbld.Append(cOut.BaptismData.SectionDate.ToString("dd MMM yyyy"))
                            Case DATE_UNK
                                If cOut.BaptismData.SectionYear > 0 Then
                                    sbld.Append(" in ")
                                    sbld.Append(CStr(cOut.BaptismData.SectionYear))
                                    sbld.Append(" on an unknown day or month ")
                                Else
                                    sbld.Append(" on an unknown date ")
                                End If
                        End Select
                        If cOut.BaptismData.SectionPlace.Trim > "" Then
                            sbld.Append(" at ")
                            sbld.Append(cOut.BaptismData.SectionPlace)
                        End If
                        listOutput.Add(sbld.ToString)
                        If cOut.BaptismData.OtherLines.Count > 0 Then
                            listOutput.Add("")
                            For Each strLine In cOut.BaptismData.OtherLines
                                listOutput.Add(":" & strLine)
                            Next
                        End If
                    End If
                Case "birth"
                    If cOut.BirthData.HasData = True Then
                        If cOut.BirthData.SectionYear > 0 Then
                            listOutput.Add("===" & cOut.BirthData.SectionYear.ToString & " Birth===")
                        Else
                            listOutput.Add("===Birth===")
                        End If
                        sbld.Clear()
                        sbld.Append(":")
                        If cSecs.MainName.Trim > "" Then
                            sbld.Append(cSecs.MainName)
                            If cSecs.FatherName.Trim > "" Then
                                sbld.Append(", child of ")
                                sbld.Append(cSecs.FatherName)
                            End If
                            If cSecs.MotherName.Trim > "" Then
                                If cSecs.FatherName.Trim > "" Then
                                    sbld.Append(" and ")
                                Else
                                    sbld.Append(", child of ")
                                End If
                                sbld.Append(cSecs.MotherName)
                            End If
                        Else
                            sbld.Append("''[enter name here]''")
                        End If
                        sbld.Append(" was born")
                        Select Case cOut.BirthData.DateStatus
                            Case DATE_ABT
                                sbld.Append(" about ")
                                sbld.Append(cOut.BirthData.SectionDateString)
                            Case DATE_AFT
                                sbld.Append(" after ")
                                sbld.Append(cOut.BirthData.SectionDateString)
                            Case DATE_BFR
                                sbld.Append(" before ")
                                sbld.Append(cOut.BirthData.SectionDateString)
                            Case DATE_CERT
                                sbld.Append(" on ")
                                sbld.Append(cOut.BirthData.SectionDate.ToString("dd MMM yyyy"))
                            Case DATE_UNK
                                If cOut.BirthData.SectionYear > 0 Then
                                    sbld.Append(" in ")
                                    sbld.Append(CStr(cOut.BirthData.SectionYear))
                                    sbld.Append(" on an unknown day or month ")
                                Else
                                    sbld.Append(" on an unknown date ")
                                End If
                        End Select
                        If cOut.BirthData.SectionPlace.Trim > "" Then
                            sbld.Append(" at ")
                            sbld.Append(cOut.BirthData.SectionPlace)
                        End If
                        listOutput.Add(sbld.ToString)
                        If cOut.BirthData.OtherLines.Count > 0 Then
                            listOutput.Add("")
                            For Each strLine In cOut.BirthData.OtherLines
                                listOutput.Add(":" & strLine)
                            Next
                        End If
                        bBirthSectionPassed = True
                    End If
                Case "burial"
                    If cOut.BurialData.HasData = True Then
                        If cOut.BurialData.SectionYear > 0 Then
                            listOutput.Add("===" & cOut.BurialData.SectionYear.ToString & " Burial===")
                        Else
                            listOutput.Add("===Burial===")
                        End If
                        sbld.Clear()
                        sbld.Append(":")
                        If cSecs.MainName.Trim > "" Then
                            sbld.Append(cSecs.MainName)
                            If cSecs.FatherName.Trim > "" Then
                                sbld.Append(", child of ")
                                sbld.Append(cSecs.FatherName)
                            End If
                            If cSecs.MotherName.Trim > "" Then
                                If cSecs.FatherName.Trim > "" Then
                                    sbld.Append(" and ")
                                Else
                                    sbld.Append(", child of ")
                                End If
                                sbld.Append(cSecs.MotherName)
                            End If
                        Else
                            sbld.Append("''[enter name here]''")
                        End If
                        sbld.Append(" was laid to rest")
                        Select Case cOut.BurialData.DateStatus
                            Case DATE_ABT
                                sbld.Append(" about ")
                                sbld.Append(cOut.BurialData.SectionDateString)
                            Case DATE_AFT
                                sbld.Append(" after ")
                                sbld.Append(cOut.BurialData.SectionDateString)
                            Case DATE_BFR
                                sbld.Append(" before ")
                                sbld.Append(cOut.BurialData.SectionDateString)
                            Case DATE_CERT
                                sbld.Append(" on ")
                                sbld.Append(cOut.BurialData.SectionDate.ToString("dd MMM yyyy"))
                            Case DATE_UNK
                                If cOut.BurialData.SectionYear > 0 Then
                                    sbld.Append(" in ")
                                    sbld.Append(CStr(cOut.BurialData.SectionYear))
                                    sbld.Append(" on an unknown day or month ")
                                Else
                                    sbld.Append(" on an unknown date ")
                                End If
                        End Select
                        If cOut.BurialData.SectionPlace.Trim > "" Then
                            sbld.Append(" at ")
                            sbld.Append(cOut.BurialData.SectionPlace)
                        End If
                        listOutput.Add(sbld.ToString)
                        If cOut.BurialData.OtherLines.Count > 0 Then
                            listOutput.Add("")
                            For Each strLine In cOut.BurialData.OtherLines
                                listOutput.Add(":" & strLine)
                            Next
                        End If
                    End If
                Case "residence"
                    Try
                        If cOut.ResidenceData(astr(0)).SectionYear > 0 Then
                            listOutput.Add("===" & cOut.ResidenceData(astr(0)).SectionYear.ToString & " Residence===")
                            sbld.Clear()
                            sbld.Append(":In ")
                            sbld.Append(cOut.ResidenceData(astr(0)).SectionYear.ToString)
                            sbld.Append(" ")
                        Else
                            listOutput.Add("===Residence===")
                            sbld.Clear()
                        End If

                        If cSecs.MainName.Trim > "" Then
                            sbld.Append(cSecs.MainName)
                        Else
                            sbld.Append("''[enter name here]''")
                        End If
                        sbld.Append(" resided at ")
                        sbld.Append(cOut.ResidenceData(astr(0)).SectionPlace)
                        listOutput.Add(sbld.ToString)
                        If cOut.ResidenceData(astr(0)).OtherLines.Count > 0 Then
                            For Each strLine In cOut.ResidenceData(astr(0)).OtherLines
                                listOutput.Add(":" & strLine)
                            Next
                        End If
                    Catch ex As Exception

                    End Try
                Case "children"
                Case "death"
                    If cOut.DeathData.SectionYear > 0 Then
                        listOutput.Add("===" & cOut.DeathData.SectionYear.ToString & " Death===")
                    Else
                        listOutput.Add("===Death===")
                    End If
                    sbld.Clear()
                    sbld.Append(":")
                    If cSecs.MainName.Trim > "" Then
                        sbld.Append(cSecs.MainName)
                    Else
                        sbld.Append("''[enter name here]''")
                    End If
                    sbld.Append(" passed away")
                    Select Case cOut.DeathData.DateStatus
                        Case DATE_ABT
                            sbld.Append(" about ")
                            sbld.Append(cOut.DeathData.SectionDateString)
                        Case DATE_AFT
                            sbld.Append(" after ")
                            sbld.Append(cOut.DeathData.SectionDateString)
                        Case DATE_BFR
                            sbld.Append(" before ")
                            sbld.Append(cOut.DeathData.SectionDateString)
                        Case DATE_CERT
                            sbld.Append(" on ")
                            sbld.Append(cOut.DeathData.SectionDate.ToString("dd MMM yyyy"))
                        Case DATE_UNK
                            If cOut.DeathData.SectionYear > 0 Then
                                sbld.Append(" in ")
                                sbld.Append(CStr(cOut.DeathData.SectionYear))
                                sbld.Append(" on an unknown day or month ")
                            Else
                                sbld.Append(" on an unknown date ")
                            End If
                    End Select
                    If cOut.DeathData.SectionPlace.Trim > "" Then
                        sbld.Append(" at ")
                        sbld.Append(cOut.DeathData.SectionPlace)
                    End If
                    listOutput.Add(sbld.ToString)
                    If cOut.DeathData.OtherLines.Count > 0 Then
                        listOutput.Add("")
                        For Each strLine In cOut.DeathData.OtherLines
                            listOutput.Add(":" & strLine)
                        Next
                    End If
                Case "divorce"
                    If cOut.DivorceData.HasData = True Then

                    End If
                Case "event"
                    If cOut.EventsData(astr(0)).EventYear > 0 Then
                        If CInt(cOut.EventsData(astr(0)).EventYear) < Year(Today) Then
                            listOutput.Add("===" & CInt(cOut.EventsData(astr(0)).EventYear).ToString & " Event===")
                        Else
                            listOutput.Add("===Event===")
                        End If
                    Else
                        listOutput.Add("===Event===")
                    End If
                    sbld.Clear()
                    sbld.Append(":")
                    sbld.Append(cOut.EventsData(astr(0)).EventType)
                    Select Case cOut.EventsData(astr(0)).DateStatus
                        Case DATE_ABT
                            sbld.Append(" about ")
                            sbld.Append(cOut.EventsData(astr(0)).EventDateString)
                        Case DATE_AFT
                            sbld.Append(" after ")
                            sbld.Append(cOut.EventsData(astr(0)).EventDateString)
                        Case DATE_BFR
                            sbld.Append(" before ")
                            sbld.Append(cOut.EventsData(astr(0)).EventDateString)
                        Case DATE_CERT
                            sbld.Append(" on ")
                            sbld.Append(cOut.EventsData(astr(0)).EventDate.ToString("dd MMM yyyy"))
                        Case DATE_UNK
                            'Not sure about this bit yet
                            If cOut.EventsData(astr(0)).EventYear > 0 Then
                                sbld.Append(" in ")
                                sbld.Append(CStr(cOut.EventsData(astr(0)).EventYear))
                                sbld.Append(" on an unknown day or month ")
                            Else
                                sbld.Append(" on an unknown date ")
                            End If
                    End Select
                    If cOut.EventsData(astr(0)).EventPlace.Trim > "" Then
                        sbld.Append(" at ")
                        sbld.Append(cOut.EventsData(astr(0)).EventPlace)
                    End If
                    listOutput.Add(sbld.ToString)
                    If cOut.EventsData(astr(0)).OtherLines.Count > 0 Then
                        For Each strLine In cOut.EventsData(astr(0)).OtherLines
                            listOutput.Add(":" & strLine)
                        Next
                    End If
                Case "marriage"
                    Try
                        If cOut.BirthData.HasData = True Then
                            If bBirthSectionPassed = True Then
                                'This probably IS a marriage of the target person
                                If cOut.MarriageData(astr(0)).SectionYear > 0 Then
                                    listOutput.Add("===" & cOut.MarriageData(astr(0)).SectionYear.ToString & " Marriage===")
                                Else
                                    listOutput.Add("===Marriage===")
                                End If
                                sbld.Clear()
                                sbld.Append(":")
                                If cSecs.MainName.Trim > "" Then
                                    sbld.Append(cSecs.MainName)
                                Else
                                    sbld.Append("''[enter name here]''")
                                End If
                                sbld.Append(" was married")
                                If cOut.MarriageData(astr(0)).Wife.Trim > "" Then
                                    sbld.Append(" to ")
                                    sbld.Append(cOut.MarriageData(astr(0)).Wife.Trim)
                                End If
                                Select Case cOut.MarriageData(astr(0)).DateStatus
                                    Case DATE_ABT
                                        sbld.Append(" about ")
                                        sbld.Append(cOut.MarriageData(astr(0)).SectionDateString)
                                    Case DATE_AFT
                                        sbld.Append(" after ")
                                        sbld.Append(cOut.MarriageData(astr(0)).SectionDateString)
                                    Case DATE_BFR
                                        sbld.Append(" before ")
                                        sbld.Append(cOut.MarriageData(astr(0)).SectionDateString)
                                    Case DATE_CERT
                                        sbld.Append(" on ")
                                        sbld.Append(cOut.MarriageData(astr(0)).SectionDate.ToString("dd MMM yyyy"))
                                    Case DATE_UNK
                                        If cOut.MarriageData(astr(0)).SectionYear > 0 Then
                                            sbld.Append(" in ")
                                            sbld.Append(CStr(cOut.MarriageData(astr(0)).SectionYear))
                                            sbld.Append(" on an unknown day or month ")
                                        Else
                                            sbld.Append(" on an unknown date ")
                                        End If
                                End Select
                                If cOut.MarriageData(astr(0)).SectionPlace.Trim > "" Then
                                    sbld.Append(" at ")
                                    sbld.Append(cOut.MarriageData(astr(0)).SectionPlace)
                                End If
                                listOutput.Add(sbld.ToString)
                                If cOut.MarriageData(astr(0)).Children.Count > 0 Then
                                    listOutput.Add("====Children of the Marriage====")
                                    For Each strLine In cOut.MarriageData(astr(0)).Children
                                        listOutput.Add(":" & strLine)
                                    Next
                                    If cOut.MarriageData(astr(0)).OtherLines.Count > 0 Then
                                        listOutput.Add("====Other Marriage data====")
                                    Else
                                        listOutput.Add("")
                                    End If
                                Else
                                    listOutput.Add("")
                                End If
                                If cOut.MarriageData(astr(0)).OtherLines.Count > 0 Then
                                    For Each strLine In cOut.MarriageData(astr(0)).OtherLines
                                        listOutput.Add(":" & strLine)
                                    Next
                                End If
                            Else
                                'This is probably the marriage of the parents of the target person
                                'Collect the parents names if available and ignore the rest
                                If cSecs.FatherName.Trim = "" Then
                                    cSecs.FatherName = cOut.MarriageData(astr(0)).Husband.Trim
                                End If
                                If cSecs.MotherName.Trim = "" Then
                                    cSecs.MotherName = cOut.MarriageData(astr(0)).Wife.Trim
                                End If
                            End If
                        End If
                    Catch ex As Exception

                    End Try
            End Select
        Next
        '        Case "note", "notes", "research note", "research notes"
        If cOut.NotesData.OtherLines.Count > 0 Then
            listOutput.Add("==Notes==")
            For Each strline In cOut.NotesData.OtherLines
                listOutput.Add(":" & strline)
            Next
        End If
        '        Case "acknowledgements"
        If cOut.Acknowledgements.OtherLines.Count > 0 Then
            listOutput.Add("==Acknowledgements==")
            For Each strline In cOut.Acknowledgements.OtherLines
                listOutput.Add(":" & strLine)
                listOutput.Add("")
            Next
            listOutput.Add(":Initial bio tidy after GEDCOM import created using [https://www.wikitree.com/wiki/Space:WikiTree_Text_Formatter WikiTree Text Formatter] by [[Meredith-1182|David Loring]]")
        Else
            listOutput.Add("==Acknowledgements==")
            listOutput.Add(":Initial bio tidy after GEDCOM import created using [https://www.wikitree.com/wiki/Space:WikiTree_Text_Formatter WikiTree Text Formatter] by [[Meredith-1182|David Loring]]")
        End If
        '
        listOutput.Add("== Sources ==")
        '        Case "sources"
        If cOut.SourcesData.OtherLines.Count > 0 Then
            For Each strline In cOut.SourcesData.OtherLines
                If strline.Contains("<references />") = False Then
                    listOutput.Add(":" & strline)
                End If
            Next
        End If
        listOutput.Add("")
        listOutput.Add("<references />")
        listOutput.Add("")
        listOutput.Add("<!-- Where a section was undated it may be located at the wrong position - probably the top. Remember to move it either to the correct location in the timeline of to the end under notes. -->")
        'Finally the sources are made in line
        'Now the whole text is parsed replacing the sources with inline versions
        For intloop = 0 To listOutput.Count - 1
            strline = listOutput(intloop)
            If strline.Contains("<ref>Source: [[#") = True Then
                sourceEnum.Reset()
                While sourceEnum.MoveNext()
                    thisSRC = sourceEnum.Current()
                    If strline.Contains("[[#" & thisSRC.ID & "]]") = True Then
                        sbld.Clear()
                        sbld.Append(strline)
                        If thisSRC.FullTextUsed = True Then
                            '============================================================
                            '  THERE IS AN INLINE SOURCE CODE
                            '============================================================
                            strOldReference = "<ref>Source: [[#" & thisSRC.ID & "]]"
                            strNewReference = thisSRC.SourceInLineCode
                            sbld.Replace(strOldReference, strNewReference)
                            strline = sbld.ToString
                            'Get a start marker
                            intCounter = InStr(strline, thisSRC.ID)
                            'Get the end marker
                            intCounter2 = InStr(intCounter + 1, strline, "</ref>")
                            sbld.Clear()
                            sbld.Append(strline.Substring(0, intCounter2 - 1))
                            sbld.Append(Chr(198))
                            If intCounter2 + 5 < strline.Length Then
                                sbld.Append(strline.Substring(intCounter2 + 5))
                            End If
                            ''Replace the </ref> first as the length will change
                            'sbld.Replace("</ref>", Chr(198), intCounter2 - 1, 6)
                            'Now the reference can be replaced
                            sbld.Replace(strNewReference, Chr(198))
                            astr = sbld.ToString.Split(Chr(198))
                            sbld.Clear()
                            sbld.Append(astr(0))
                            sbld.Append(strNewReference)
                            If astr.Length > 2 Then
                                sbld.Append(astr(2))
                            End If
                            listOutput(intloop) = sbld.ToString
                        Else
                            '============================================================
                            '  THE FULL TEXT HAS NOT BEEN USED YET YET
                            '============================================================
                            strOldReference = "<ref>Source: [[#" & thisSRC.ID & "]]"
                            strNewReference = "<ref name=" & Chr(34) & thisSRC.ID & Chr(34) & ">"
                            sbld.Replace(strOldReference, strNewReference)
                            strline = sbld.ToString
                            'Get a start marker
                            intCounter = InStr(strline, thisSRC.ID & Chr(34))
                            'Get the end marker
                            intCounter2 = InStr(intCounter + 1, strline, "</ref>")
                            sbld.Clear()
                            sbld.Append(strline.Substring(0, intCounter2 - 1))
                            sbld.Append(Chr(198))
                            If intCounter2 + 5 < strline.Length Then
                                sbld.Append(strline.Substring(intCounter2 + 5))
                            End If
                            'Now the reference can be replaced
                            sbld.Replace(strNewReference, Chr(198))
                            astr = sbld.ToString.Split(Chr(198))
                            sbld.Clear()
                            sbld.Append(astr(0))
                            sbld.Append(strNewReference)
                            'Now the source text
                            sbld.Append(thisSRC.SourceText)
                            sbld.Append("</ref>")
                            If astr.Length > 2 Then
                                sbld.Append(astr(2))
                            End If
                            thisSRC.FullTextUsed = True
                            listOutput(intloop) = sbld.ToString
                        End If
                    End If
                End While
                thisSRC = Nothing
            End If
        Next
        'Now it is parsed yet again to remove spurious references with no source
        For intloop = 0 To listOutput.Count - 1
            strline = listOutput(intloop)
            If strline.Contains("<ref>Source: [[#") = True Then
                sbld.Clear()
                sbld.Append(strline)
                sbld.Replace("<ref>", Chr(198))
                sbld.Replace("</ref>", Chr(198))
                astr = sbld.ToString.Split(Chr(198))
                sbld.Clear()
                For Each strTemp In astr
                    If strTemp.Contains("Source: [[#") = False Then
                        sbld.Append(strTemp)
                    End If
                Next
                listOutput(intloop) = sbld.ToString.Trim
            End If
        Next
        'Create the text
        sbld.Clear()
        For Each strline In listOutput
            sbld.AppendLine(strline)
        Next
        m_strBioText = sbld.ToString
        '========================================
        'Now create web links from the sources
        sourceEnum.Reset()
        While sourceEnum.MoveNext
            thisSRC = sourceEnum.Current
            sbld.Clear()
            'If thisSRC.IsNote = False Then

            'End If
        End While
    End Sub
    Private Function RemoveLeadColons(ByVal IncomingText As String) As String
        Dim strRTN As String = ""
        Dim strTemp As String = IncomingText.Trim
        Do
            If IncomingText.StartsWith(":") = True Then
                strTemp = IncomingText.Substring(1)
                IncomingText = strTemp.Trim
            Else
                Exit Do
            End If
        Loop
        strRTN = IncomingText
        Return strRTN
    End Function
    Private Function CheckForMainID(ByVal TextLine As String, ByRef ReturnedName As String) As Boolean
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim astr() As String = Nothing
        Dim strRN As String = ""
        Dim bSuccess As Boolean = False
        If cSecs.MainWikiTreeID.Trim > "" Then
            If TextLine.Contains(cSecs.MainWikiTreeID) = True Then
                sbld.Clear()
                sbld.Append(TextLine)
                sbld.Replace("[[", Chr(198))
                sbld.Replace("]]", Chr(198))
                astr = sbld.ToString.Split(Chr(198))
                For Each strTemp In astr
                    If strTemp.Contains("|") = True Then
                        astr = strTemp.Split("|")
                        strRN = astr(1)
                        bSuccess = True
                    End If
                Next
            End If
        End If
        ReturnedName = strRN
        Return bSuccess
    End Function
    Private Function CheckForFatherID(ByVal TextLine As String, ByRef ReturnedName As String) As Boolean
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim astr() As String = Nothing
        Dim strRN As String = ""
        Dim bSuccess As Boolean = False
        If cSecs.FatherWikiTreeID.Trim > "" Then
            If TextLine.Contains(cSecs.FatherWikiTreeID) = True Then
                sbld.Clear()
                sbld.Append(TextLine)
                sbld.Replace("[[", Chr(198))
                sbld.Replace("]]", Chr(198))
                astr = sbld.ToString.Split(Chr(198))
                For Each strTemp In astr
                    If strTemp.Contains("|") = True Then
                        astr = strTemp.Split("|")
                        strRN = astr(1)
                        bSuccess = True
                    End If
                Next
            End If
        End If
        ReturnedName = strRN
        Return bSuccess
    End Function
    Private Function CheckForMotherID(ByVal TextLine As String, ByRef ReturnedName As String) As Boolean
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim astr() As String = Nothing
        Dim strRN As String = ""
        Dim bSuccess As Boolean = False
        If cSecs.MotherWikiTreeID.Trim > "" Then
            If TextLine.Contains(cSecs.MotherWikiTreeID) = True Then
                sbld.Clear()
                sbld.Append(TextLine)
                sbld.Replace("[[", Chr(198))
                sbld.Replace("]]", Chr(198))
                astr = sbld.ToString.Split(Chr(198))
                For Each strTemp In astr
                    If strTemp.Contains("|") = True Then
                        astr = strTemp.Split("|")
                        strRN = astr(1)
                        bSuccess = True
                    End If
                Next
            End If
        End If
        ReturnedName = strRN
        Return bSuccess
    End Function
    Private Function ParsePartialDate(ByRef IncomingDate As String) As Integer
        Dim intRTN As Integer = DATE_UNK
        Dim sbld As StringBuilder = New StringBuilder(IncomingDate.ToUpper)
        If IncomingDate.ToUpper.Contains("ABT") OrElse IncomingDate.ToUpper.Contains("ABOUT") Then
            intRTN = DATE_ABT
            sbld.Replace("ABOUT.", "")
            sbld.Replace("ABOUT", "")
            sbld.Replace("ABT.", "")
            sbld.Replace("ABT", "")
            IncomingDate = sbld.ToString.Trim
        ElseIf IncomingDate.ToUpper.Contains("AFT") OrElse IncomingDate.ToUpper.Contains("AFTER") Then
            intRTN = DATE_AFT
            sbld.Replace("AFTER.", "")
            sbld.Replace("AFTER", "")
            sbld.Replace("AFT.", "")
            sbld.Replace("AFT", "")
            IncomingDate = sbld.ToString.Trim
        ElseIf IncomingDate.ToUpper.Contains("BFR") OrElse IncomingDate.ToUpper.Contains("BEF") OrElse IncomingDate.ToUpper.Contains("BEFORE") Then
            intRTN = DATE_BFR
            sbld.Replace("BEFORE.", "")
            sbld.Replace("BEFORE", "")
            sbld.Replace("BFR.", "")
            sbld.Replace("BFR", "")
            sbld.Replace("BEF.", "")
            sbld.Replace("BEF", "")
            IncomingDate = sbld.ToString.Trim
        End If
        Return intRTN
    End Function
End Class

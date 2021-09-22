Imports System.IO, System.Text
Imports System.Drawing.Text
Imports ListsClasses
Imports System.Collections
Imports System.Collections.Generic

Module WTBData
    Private censusRecords As New FMPCensusLines
    Private sw As StreamWriter = Nothing
    'Control variables
    Private m_bBracketNameTypes As Boolean = False 'Only applied to dated sections
    Private m_bClearOnClipboardCopy As Boolean = False
    Private m_bCompactFSBlock As Boolean = False
    Private m_bCreateEmptyPlaceholders As Boolean = False 'Does not apply to timeline
    Private m_bCreateTemplates As Boolean = False 'Creates a project template for birth related sections
    Private m_bDoubleIndent As Boolean = False
    Private m_bGetBiography As Boolean = False 'Used on the get profile page. Not to be confused with Include Biography Section
    Private m_bIncludeBioSection As Boolean = True
    Private m_bOrdinalIndicators As Boolean = True 'Adds st, rd, th to day values in US and European formats
    Private m_bInCensusTable As Boolean = False
    Private m_bPadSingleDigits As Boolean = True 'Pads a single day or month with zero for Asian formats or days for European and US formats if st, rd, th, etc not used
    Private m_bSaveProfileID As Boolean = False
    Private m_bShortMonths As Boolean = False
    Private m_intBaptismText As Integer = 0
    Private m_intFullDatesFormat As Integer = 0
    Private m_intLayoutStyle As Integer = 0 'Timeline
    Private m_intTimelineStyle As Integer = 2 'Dated sections
    Private m_listPermittedFields As New List(Of String)
    Private m_strWTLogon As String = ""
    Private m_strWTPassword As String = ""
    'Variables for record data
    Private m_strWebRef As String = ""
    Private m_bIsBaptism As Boolean = False
    Private m_bIsBirth As Boolean = False
    Private m_bIsBurial As Boolean = False
    Private m_bIsCensus As Boolean = False
    Private m_bIsDeath As Boolean = False
    Private m_bIsFS As Boolean = False
    Private m_bIsMarriage As Boolean = False
    Private m_bIsWAdmission As Boolean = False
    Private m_bNextIsCitation As Boolean = False
    Private m_bRetainImportInfo As Boolean = False
    Private m_srtProvince As String = ""
    Private m_strAffiliateFilmNumber As String = ""
    Private m_strAffiliateName As String = ""
    Private m_strAffiliatePublicationNumber As String = ""
    Private m_strAffiliateRecordType As String = ""
    Private m_strAge As String = ""
    Private m_strAgeExpanded As String = ""
    Private m_strAssemblyDistrictNumber As String = ""
    Private m_strBaptismDate As String = ""
    Private m_strBaptismPlace As String = ""
    Private m_strBirthCountry As String = ""
    Private m_strBirthCounty As String = ""
    Private m_strBirthDate As String = ""
    Private m_strBirthplace As String = ""
    Private m_strBirthYear As String = ""
    Private m_strBookNumber As String = ""
    Private m_strBurialDate As String = ""
    Private m_strBurialPlace As String = ""
    Private m_strBurialYear As String = ""
    Private m_strCensusCountry As String = ""
    Private m_strCitation As String = ""
    Private m_strCitationType As String = ""
    Private m_strCityBorough As String = ""
    Private m_strCounty As String = ""
    Private m_strDeathDate As String = ""
    Private m_strDeathPlace As String = ""
    Private m_strDigitalFolderNumber As String = ""
    Private m_strDistrict As String = ""
    Private m_strDistrictName As String = ""
    Private m_strElectionDistrictNumber As String = ""
    Private m_strEventDate As String = ""
    Private m_strEventDay As String = ""
    Private m_strEventMonth As String = ""
    Private m_strEventPlace As String = ""
    Private m_strEventPlaceOriginal As String = ""
    Private m_strEventType As String = ""
    Private m_strEventYear As String = ""
    Private m_strFamilyNumber As String = ""
    Private m_strFathersAge As String = ""
    Private m_strFathersBirthplace As String = ""
    Private m_strFathersName As String = ""
    Private m_strFathersTitles As String = ""
    Private m_strGender As String = ""
    Private m_strGSFilmNumber As String = ""
    Private m_strHouseholdID As String = ""
    Private m_strHouseName As String = ""
    Private m_strHouseNumber As String = ""
    Private m_strImageNumber As String = ""
    Private m_strImmigrationYear As String = ""
    Private m_strIndexingProjectBatchNumber As String = ""
    Private m_strInstitution As String = ""
    Private m_strLastPlaceOfResidence As String = ""
    Private m_strLineNumber As String = ""
    Private m_strMaritalStatus As String = ""
    Private m_strMarriagePlace As String = ""
    Private m_strMilitaryBattalion As String = ""
    Private m_strMilitaryUnit As String = ""
    Private m_strMothersAge As String = ""
    Private m_strMothersBirthplace As String = ""
    Private m_strMothersName As String = ""
    Private m_strName As String = ""
    Private m_strNameNote As String = ""
    Private m_strOccupation As String = ""
    Private m_strPage As String = ""
    Private m_strPageNumber As String = ""
    Private m_strParentName As String = ""
    Private m_strParish As String = ""
    Private m_strPieceFolio As String = ""
    Private m_strRace As String = ""
    Private m_strRaceOriginal As String = ""
    Private m_strReferenceID As String = ""
    Private m_strRegistrationDistrict As String = ""
    Private m_strRegistrationNumber As String = ""
    Private m_strRegistrationQuarter As String = ""
    Private m_strRegistrationYear As String = ""
    Private m_strRelationship As String = ""
    Private m_strRelationshipOriginal As String = ""
    Private m_strRelationshipToHead As String = ""
    Private m_strReligion As String = ""
    Private m_strResidence As String = ""
    Private m_strResidenceNote As String = ""
    Private m_strResidencePlace As String = ""
    Private m_strScheduleType As String = ""
    Private m_strSheetLetter As String = ""
    Private m_strSheetNumber As String = ""
    Private m_strSheetNumberAndLetter As String = ""
    Private m_strShipName As String = ""
    Private m_strSpouseName As String = ""
    Private m_strStreet As String = ""
    Private m_strSubDistrict As String = ""
    Private m_strSystemOrigin As String = ""
    Private m_strTitlesAndTerms As String = ""
    Private m_strTown As String = ""
    Private m_strVolume As String = ""

    Friend Function SelectCensus(ByVal PastedText As String, ByVal CensusDataFormat As String) As String
        Dim sbld As StringBuilder = New StringBuilder(PastedText)
        Dim astr() As String = Nothing
        Dim bNextIsCitation As Boolean = False
        Dim listLines As New List(Of String)

        ClearStorage()
        sbld.Replace(vbCrLf, Chr(198))
        sbld.Replace(vbCr, Chr(198))
        sbld.Replace(vbLf, Chr(198))
        censusRecords.DataFormat = CensusDataFormat
        Select Case CensusDataFormat
            Case "FMP"
                sbld.Replace("Back to results", "CENSUS DATE TRIGGER")
                sbld.Replace("Household Members", "TABLE START")
                sbld.Replace("Census details", "TABLE END")
                'Deals with: 
                'UK census forms
                'Irish 1821 form
                'Irish 1841/1851 search forms
                'Canada 1861
                sbld.Replace("�Report an error in this transcription", "TERMINATOR")
                'Deals with Irish 1901, 1911 census forms
                sbld.Replace("National Archives of Ireland", "TERMINATOR")
                'Deals with the US census forms
                sbld.Replace("Index (c) IRI. Used by permission of FamilySearch Intl", "TERMINATOR")
                If PastedText.ToLower.Contains("brightsolid online publishing ltd") = True OrElse PastedText.ToLower.Contains("findmypast") = True _
                    OrElse PastedText.ToLower.Contains("used by permission of familysearch") OrElse PastedText.ToLower.Contains("myheritage") Then
                    m_bIsFS = False
                End If
            Case "FS"
                If PastedText.ToLower.Contains("//familysearch.org/") = True Then
                    m_bIsFS = True
                End If
        End Select
        If PastedText.ToLower.Contains("census") = True Then
            m_bIsCensus = True
        End If
        'Now the data is split into lines
        astr = sbld.ToString.Split(Chr(198))
        Return CreateWikiTreeCensusListing(astr)
    End Function

    'Friend Function CensusRecordData() As FMPCensusLines
    '    Return censusRecords
    'End Function
    Friend Function PermittedFields() As List(Of String)
        Return m_listPermittedFields
    End Function
    Friend Sub LoadSettings()
        m_listPermittedFields.Add("First Names")
        m_listPermittedFields.Add("Last Name")
        m_listPermittedFields.Add("Role")
        'm_listPermittedFields.Add("Status")
        m_listPermittedFields.Add("Gender")
        'm_listPermittedFields.Add("Occupation")
        m_listPermittedFields.Add("Age")
        'm_listPermittedFields.Add("Birth Year")
        m_listPermittedFields.Add("Birth Place")
    End Sub

#Region "RETURN FUNCTIONS"
'    Friend Function BaptismText() As Integer
'        Return My.Settings.BaptismText
'    End Function
'    Friend Function BracketTypeNames() As Boolean
'        Return My.Settings.BracketNameTypes
'    End Function
'    Friend Function CensusDoubleIndent() As Boolean
'        Return My.Settings.CensusDoubleIndent
'    End Function
'    Friend Function CreateEmptyPlaceholders() As Boolean
'        Return My.Settings.CreateEmptyPlaceholders
'    End Function
'    Friend Function CreateTemplates() As Boolean
'        Return My.Settings.CreateTemplates
'    End Function
'    Friend Function ClearOnClipboardCopy() As Boolean
'        Return My.Settings.ClearOnClipboard
'    End Function
'    Friend Function CompactFSBlocks() As Boolean
'        Return My.Settings.FSBlockCompact
'    End Function
'    Friend Function FullDatesFormat() As Integer
'        Return My.Settings.FullDatesFormat
'    End Function
'    ''Friend Function GetBiographyFlag() As Boolean
'    ''    Return m_bGetBiography
'    ''End Function
'    'Friend Function IncludeBioSection() As Boolean
'    '    Return m_bIncludeBioSection
'    'End Function
'    Friend Function OrdinalIndicators() As Boolean
'        Return My.Settings.OrdinalIndicators
'    End Function
'    Friend Function LayoutStyle() As Integer
'        Return My.Settings.LayoutStyle
'    End Function
'    Friend Function PadSingleDigits() As Boolean
'        Return My.Settings.PadSingleWithZeros
'    End Function
'    Friend Function RetainImportInfo() As Boolean
'        Return My.Settings.RetainImportInfo
'    End Function
'    Friend Function AddNoMoreInfo() As Boolean
'        Return My.Settings.AddNoMoreInfo
'    End Function
'    Friend Function RetainNoMoreInfo() As Boolean
'        Return My.Settings.RetainNoMoreInfo
'    End Function
'    Friend Function AddThresholdCount() As Decimal
'        Return My.Settings.AddThreshold
'    End Function
'    'Friend Function SaveProfileIDFlag() As Boolean
'    '    Return m_bSaveProfileID
'    'End Function
'    Friend Function ShortMonths() As Boolean
'        Return My.Settings.ShortMonths
'    End Function
    Friend Function TimelineStyle() As Integer
        Return m_intTimelineStyle
    End Function
'    'Friend Function WikiTreeLogonName() As String
'    '    Return m_strWTLogon
'    'End Function
'    'Friend Function WikiTreePassword() As String
'    '    Return m_strWTPassword
'    'End Function
'
#End Region
#Region "OTHER FUNCTIONS"
    Friend Function CensusDateBlock() As String
        Dim sbld As StringBuilder = New StringBuilder(" which took place on ")
        Dim bFound As Boolean = False
        If EventYear = "1841" Then
            '1841 can only be a UK census
            sbld.Append(CENSUS_DATE_1841UK)
            bFound = True
        ElseIf EventYear = "1851" Then
            Select Case censusRecords.CensusCountry
                Case "CA"
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1851UK)
                    bFound = True
            End Select
        ElseIf EventYear = "1861" Then
            Select Case censusRecords.CensusCountry
                Case "CA"
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1861UK)
                    bFound = True
            End Select
        ElseIf EventYear = "1871" Then
            Select Case censusRecords.CensusCountry
                Case "CA"
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1871UK)
            End Select
        ElseIf EventYear = "1881" Then
            Select Case censusRecords.CensusCountry
                Case "CA"
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1881UK)
                    bFound = True
            End Select
        ElseIf EventYear = "1891" Then
            Select Case censusRecords.CensusCountry
                Case "CA"
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1891UK)
                    bFound = True
            End Select
        ElseIf EventYear = "1901" Then
            Select Case censusRecords.CensusCountry
                Case "CA"
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1901UK)
                    bFound = True
            End Select
        ElseIf EventYear = "1911" Then
            'This can be either UK or EI
            Select Case censusRecords.CensusCountry
                Case "EI"
                Case "UK"
                    sbld.Append(CENSUS_DATE_1911UK)
                    bFound = True
            End Select
        End If
        If bFound = False Then
            sbld.Clear()
        End If
        Return sbld.ToString
    End Function
    Friend Function CreateResidence() As String
        Dim sbld As StringBuilder = New StringBuilder("")
        If ResidencePlace.Trim > "" Then
            sbld.Append(ResidencePlace)
        ElseIf m_strEventPlace.Trim > "" Then
            sbld.Append(m_strEventPlace)
        Else
            Select Case censusRecords.CensusCountry
                Case "CA"
                    If SubDistrict.Trim > "" Then
                        sbld.Append(SubDistrict)
                        sbld.Append(", ")
                    End If
                    If DistrictName.Trim > "" Then
                        sbld.Append(DistrictName)
                        sbld.Append(", ")
                    End If
                    If Province.Trim > "" Then
                        sbld.Append(Province)
                        sbld.Append(", ")
                    End If
                    If CensusCountry.Trim > "" Then
                        sbld.Append(CensusCountry)
                    End If
                Case Else
                    If HouseName.Trim > "" Then
                        sbld.Append(HouseName)
                        sbld.Append(" ")
                    End If
                    If HouseNumber.Trim > "" Then
                        sbld.Append(HouseNumber)
                        sbld.Append(", ")
                    End If
                    If Street.Trim > "" Then
                        sbld.Append(Street)
                        sbld.Append(", ")
                    End If
                    If Town.Trim > "" Then
                        sbld.Append(Town)
                        sbld.Append(", ")
                    End If
                    If Town.Trim > "" Then
                        sbld.Append(Town)
                        sbld.Append(", ")
                    End If
                    If County.Trim > "" Then
                        sbld.Append(County)
                        sbld.Append(", ")
                    End If
                    If CensusCountry.Trim > "" Then
                        sbld.Append(CensusCountry)
                    End If
            End Select
        End If
        If sbld.ToString.Trim = "" Then
            sbld.Append("an unknown location")
        End If
        Return sbld.ToString
    End Function
    Private Function CreateWikiTreeCensusListing(ByVal astr() As String) As String
        Dim censusData As IEnumerator = Nothing
        Dim thisPerson As FMPCensusLine
        Dim astrSub() As String = Nothing
        Dim bAddressNext As Boolean = False
        Dim bInCensusTrigger As Boolean = False
        Dim bInDetails As Boolean = False
        Dim bInTable As Boolean = False
        Dim bIsCensus As Boolean = False
        Dim bIsFMP As Boolean = False
        Dim bSingleRecordUSFormat As Boolean = False
        Dim bUSA As Boolean = False
        Dim bInvalidCensus As Boolean = False
        Dim bProcessingError As Boolean = False
        Dim intLineIndex As Integer = 0
        Dim intLoop As Integer = 0
        Dim listHeaderArea As New List(Of String)
        Dim listBodyArea As New List(Of String)
        Dim listPersonDetails As New List(Of String)
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim sbldSub As StringBuilder = New StringBuilder("")
        Dim sbldWT As StringBuilder = New StringBuilder("")
        'Assembly variables
        Dim strFolio As String = ""
        Dim strName As String = ""
        Select Case censusRecords.DataFormat
            Case "FMP"
                Try
                    'this is pass no 1
                    For intLoop = 0 To astr.Count - 1
                        If astr(intLoop).Trim = "CENSUS DATE TRIGGER" Then
                            bInCensusTrigger = True
                        ElseIf astr(intLoop) = "TABLE START" Then
                            bInCensusTrigger = False
                            bInTable = True
                        ElseIf astr(intLoop) = "TABLE END" Then
                            bInTable = False
                            bInDetails = True
                        ElseIf astr(intLoop) = "TERMINATOR" Then
                            Exit For
                        Else
                            If bInCensusTrigger = True Then
                                listHeaderArea.Add(astr(intLoop).Trim)
                            ElseIf bInTable = True Then
                                listBodyArea.Add(astr(intLoop).Trim)
                            ElseIf bInDetails = True Then
                                listPersonDetails.Add(astr(intLoop).Trim)
                            End If
                        End If
                    Next
                Catch ex1 As Exception
                                Throw ex1
'                    MessageBox.Show("Sorry, something went wrong parsing the initial data. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    bInvalidCensus = True
                    bProcessingError = True
                    sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                    sw.WriteLine(Now)
                    sw.WriteLine(m_strWebRef)
                    sw.WriteLine("Initial data parse error")
                    sw.WriteLine(ex1.Message)
                    sw.WriteLine(ex1.Source)
                    sw.WriteLine(ex1.InnerException)
                    sw.WriteLine(ex1.StackTrace)
                    sw.Close()
                    sw.Dispose()
                End Try
                If m_bIsFS = False AndAlso m_bIsCensus = True AndAlso bInvalidCensus = False Then
                    'EXTRACT THE CENSUS YEAR DETAILS ETC
                    Try
                        For Each strLine In listHeaderArea
                            If bAddressNext = True Then
                                'This is the address line
                                ResidencePlace = strLine.Trim
                                bAddressNext = False
                                Exit For
                            Else
                                If strLine.Contains("Ireland Census 1901") = True OrElse strLine.Contains("Ireland Census 1911") = True Then
                                    'The format of the Irish census is different
                                    sbldSub.Clear()
                                    sbldSub.Append(strLine)
                                    sbldSub.Replace("Ireland Census ", "")
                                    censusRecords.CensusYear = CInt(Val(sbldSub.ToString))
                                    censusRecords.CensusCountry = "EI"
                                    'This is the census information line
                                    EventYear = censusRecords.CensusYear.ToString
                                    sbld.Clear()
                                    sbld.Append(strLine)
                                    sbld.Replace(" Transcription", "")
                                    EventType = sbld.ToString
                                    bAddressNext = True
                                ElseIf strLine.Contains("Ireland Census search forms 1841 & 1851 Transcription") = True Then
                                    'The year of the census is carried in the peron details area

                                ElseIf strLine.Contains("Canada Census") = True Then
                                    'The format of the Canadian census is different
                                    sbldSub.Clear()
                                    sbldSub.Append(strLine)
                                    sbldSub.Replace("Canada Census ", "")
                                    censusRecords.CensusYear = CInt(Val(sbldSub.ToString))
                                    censusRecords.CensusCountry = "CA"
                                    'This is the census information line
                                    EventYear = censusRecords.CensusYear.ToString
                                    sbld.Clear()
                                    sbld.Append(strLine)
                                    sbld.Replace(" Transcription", "")
                                    EventType = sbld.ToString
                                    bAddressNext = True
                                ElseIf strLine.Contains("US Census") = True Then
                                    censusRecords.CensusYear = CInt(Val(strLine))
                                    censusRecords.CensusCountry = "US"
                                    'This is the census information line
                                    EventYear = Val(strLine).ToString
                                    sbld.Clear()
                                    sbld.Append(strLine)
                                    sbld.Replace(" Transcription", "")
                                    EventType = sbld.ToString
                                    bAddressNext = True
                                ElseIf strLine.Contains("England, Wales & Scotland") = True Then
                                    censusRecords.CensusYear = CInt(Val(strLine))
                                    censusRecords.CensusCountry = "UK"
                                    'This is the census information line
                                    EventYear = Val(strLine).ToString
                                    sbld.Clear()
                                    sbld.Append(strLine)
                                    sbld.Replace(" Transcription", "")
                                    EventType = sbld.ToString
                                    bAddressNext = True
                                End If
                            End If
                        Next
                    Catch ex2 As Exception
                                Throw ex2
'                        MessageBox.Show("Sorry, something went wrong parsing the header data. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        bInvalidCensus = True
                        bProcessingError = True
                        sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                        sw.WriteLine(Now)
                        sw.WriteLine(m_strWebRef)
                        sw.WriteLine("Header data parse error")
                        sw.WriteLine(ex2.Message)
                        sw.WriteLine(ex2.Source)
                        sw.WriteLine(ex2.InnerException)
                        sw.WriteLine(ex2.StackTrace)
                        sw.Close()
                        sw.Dispose()
                    End Try
                    If bInvalidCensus = False Then
                        Try
                            For Each strLine In listBodyArea
                                If intLineIndex = 0 Then
                                    If strLine.Trim = "" Then
                                        intLineIndex = censusRecords.Add()
                                        intLoop = 0
                                    Else
                                        Select Case censusRecords.CensusCountry
                                            Case "CA"
                                                Select Case censusRecords.CensusYear
                                                    Case 1861 To 1901
                                                        If strLine = "First name(s)" Then
                                                            censusRecords.FieldHeaderData(0).ItemData2 = 1
                                                        ElseIf strLine = "Last name" Then
                                                            censusRecords.FieldHeaderData(1).ItemData2 = 1
                                                        ElseIf strLine = "Sex" Or strLine = "Gender" Then
                                                            censusRecords.FieldHeaderData(2).ItemData2 = 1
                                                        ElseIf strLine = "Birth year" Then
                                                            censusRecords.FieldHeaderData(3).ItemData2 = 1
                                                        ElseIf strLine = "Birth place" Then
                                                            censusRecords.FieldHeaderData(4).ItemData2 = 1
                                                        ElseIf strLine.StartsWith("Relationship") = True Then
                                                            censusRecords.FieldHeaderData(5).ItemData2 = 1
                                                        End If
                                                    Case Else
                                                        bInvalidCensus = True
                                                End Select
                                            Case "EI"
                                                Select Case censusRecords.CensusYear
                                                    Case 1901, 1911
                                                        'This may seem a bit obvious but it weeds out any missing columns
                                                        If strLine = "First name(s)" Then
                                                            censusRecords.FieldHeaderData(0).ItemData2 = 1
                                                        ElseIf strLine = "Last name" Then
                                                            censusRecords.FieldHeaderData(1).ItemData2 = 1
                                                        ElseIf strLine.StartsWith("Relationship") = True Then
                                                            censusRecords.FieldHeaderData(2).ItemData2 = 1
                                                        ElseIf strLine = "Marital status" Then
                                                            censusRecords.FieldHeaderData(3).ItemData2 = 1
                                                        ElseIf strLine = "Age" Then
                                                            censusRecords.FieldHeaderData(4).ItemData2 = 1
                                                        ElseIf strLine = "Birth year" Then
                                                            censusRecords.FieldHeaderData(5).ItemData2 = 1
                                                        ElseIf strLine = "Birth place" Then
                                                            censusRecords.FieldHeaderData(6).ItemData2 = 1
                                                        End If
                                                    Case Else
                                                        bInvalidCensus = True
                                                End Select
                                            Case "UK"
                                                Select Case censusRecords.CensusYear
                                                    Case 1841
                                                        'This may seem a bit obvious but it weeds out any missing columns
                                                        If strLine = "First name(s)" Then
                                                            censusRecords.FieldHeaderData(0).ItemData2 = 1
                                                        ElseIf strLine = "Last name" Then
                                                            censusRecords.FieldHeaderData(1).ItemData2 = 1
                                                        ElseIf strLine = "Sex" Or strLine = "Gender" Then
                                                            censusRecords.FieldHeaderData(2).ItemData2 = 1
                                                        ElseIf strLine = "Age" Then
                                                            censusRecords.FieldHeaderData(3).ItemData2 = 1
                                                        ElseIf strLine = "Birth year" Then
                                                            censusRecords.FieldHeaderData(4).ItemData2 = 1
                                                        ElseIf strLine = "Birth place" Then
                                                            censusRecords.FieldHeaderData(5).ItemData2 = 1
                                                        End If
                                                    Case 1851 To 1901
                                                        'This may seem a bit obvious but it weeds out any missing columns
                                                        If strLine = "First name(s)" Then
                                                            censusRecords.FieldHeaderData(0).ItemData2 = 1
                                                        ElseIf strLine = "Last name" Then
                                                            censusRecords.FieldHeaderData(1).ItemData2 = 1
                                                        ElseIf strLine.StartsWith("Relationship") = True Then
                                                            censusRecords.FieldHeaderData(2).ItemData2 = 1
                                                        ElseIf strLine = "Marital status" Then
                                                            censusRecords.FieldHeaderData(3).ItemData2 = 1
                                                        ElseIf strLine = "Sex" Or strLine = "Gender" Then
                                                            censusRecords.FieldHeaderData(4).ItemData2 = 1
                                                        ElseIf strLine = "Age" Then
                                                            censusRecords.FieldHeaderData(5).ItemData2 = 1
                                                        ElseIf strLine = "Birth year" Then
                                                            censusRecords.FieldHeaderData(6).ItemData2 = 1
                                                        ElseIf strLine = "Occupation" Then
                                                            censusRecords.FieldHeaderData(7).ItemData2 = 1
                                                        ElseIf strLine = "Birth place" Then
                                                            censusRecords.FieldHeaderData(8).ItemData2 = 1
                                                        End If
                                                    Case 1911
                                                        'This may seem a bit obvious but it weeds out any missing columns
                                                        If strLine = "First name(s)" Then
                                                            censusRecords.FieldHeaderData(0).ItemData2 = 1
                                                        ElseIf strLine = "Last name" Then
                                                            censusRecords.FieldHeaderData(1).ItemData2 = 1
                                                        ElseIf strLine.StartsWith("Relationship") = True Then
                                                            censusRecords.FieldHeaderData(2).ItemData2 = 1
                                                        ElseIf strLine = "Marital status" Then
                                                            censusRecords.FieldHeaderData(3).ItemData2 = 1
                                                        ElseIf strLine = "Sex" Or strLine = "Gender" Then
                                                            censusRecords.FieldHeaderData(4).ItemData2 = 1
                                                        ElseIf strLine = "Occupation" Then
                                                            censusRecords.FieldHeaderData(5).ItemData2 = 1
                                                        ElseIf strLine = "Age" Then
                                                            censusRecords.FieldHeaderData(6).ItemData2 = 1
                                                        ElseIf strLine = "Birth year" Then
                                                            censusRecords.FieldHeaderData(7).ItemData2 = 1
                                                        ElseIf strLine = "Birth place" Then
                                                            censusRecords.FieldHeaderData(8).ItemData2 = 1
                                                        End If
                                                    Case 1921
                                                    Case Else
                                                        bInvalidCensus = True
                                                End Select
                                            Case "US"
                                                Select Case censusRecords.CensusYear
                                                    Case 1850 To 1870
                                                    Case 1880, 1900, 1910
                                                    Case 1920
                                                    Case 1930
                                                    Case 1940
                                                    Case Else
                                                        bInvalidCensus = True
                                                End Select
                                        End Select
                                    End If
                                Else
                                    If bInvalidCensus = False Then
                                        'This is where censusRecords gets filled
                                        If strLine.Trim = "Transcription" Then
                                            intLineIndex = censusRecords.Add()
                                            intLoop = 0
                                        Else
                                            Select Case censusRecords.CensusCountry
                                                Case "CA"
                                                    Select Case censusRecords.CensusYear
                                                        Case 1861, 1901, 1911
                                                            Select Case censusRecords.FieldHeaderData(intLoop).ItemData1
                                                                Case 0
                                                                    censusRecords(intLineIndex).FirstNames = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 1
                                                                    censusRecords(intLineIndex).LastName = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 2
                                                                    censusRecords(intLineIndex).Gender = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 3
                                                                    censusRecords(intLineIndex).BirthYear = CInt(Val(strLine))
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 4
                                                                    censusRecords(intLineIndex).BirthPlace = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 5
                                                                    censusRecords(intLineIndex).Role = strLine
                                                            End Select
                                                        Case 1871, 1881
                                                            Select Case censusRecords.FieldHeaderData(intLoop).ItemData1
                                                                Case 0
                                                                    censusRecords(intLineIndex).FirstNames = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 1
                                                                    censusRecords(intLineIndex).LastName = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 2
                                                                    censusRecords(intLineIndex).Gender = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 3
                                                                    censusRecords(intLineIndex).BirthYear = CInt(Val(strLine))
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 4
                                                                    censusRecords(intLineIndex).BirthPlace = strLine
                                                            End Select
                                                    End Select
                                                Case "EI"
                                                    Select Case censusRecords.CensusYear
                                                        Case 1901, 1911
                                                            Select Case censusRecords.FieldHeaderData(intLoop).ItemData1
                                                                Case 0
                                                                    censusRecords(intLineIndex).FirstNames = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 1
                                                                    censusRecords(intLineIndex).LastName = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 2
                                                                    censusRecords(intLineIndex).Role = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 3
                                                                    censusRecords(intLineIndex).Status = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 4
                                                                    censusRecords(intLineIndex).Age = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 5
                                                                    censusRecords(intLineIndex).BirthYear = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 6
                                                                    censusRecords(intLineIndex).BirthPlace = strLine
                                                            End Select
                                                    End Select
                                                Case "UK"
                                                    Select Case censusRecords.CensusYear
                                                        Case 1841
                                                            Select Case censusRecords.FieldHeaderData(intLoop).ItemData1
                                                                Case 0
                                                                    censusRecords(intLineIndex).FirstNames = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 1
                                                                    censusRecords(intLineIndex).LastName = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 2
                                                                    censusRecords(intLineIndex).Gender = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 3
                                                                    censusRecords(intLineIndex).Age = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 4
                                                                    censusRecords(intLineIndex).BirthYear = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 5
                                                                    censusRecords(intLineIndex).BirthPlace = strLine
                                                            End Select
                                                        Case 1851 To 1901
                                                            Select Case censusRecords.FieldHeaderData(intLoop).ItemData1
                                                                Case 0
                                                                    censusRecords(intLineIndex).FirstNames = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 1
                                                                    censusRecords(intLineIndex).LastName = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 2
                                                                    censusRecords(intLineIndex).Role = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 3
                                                                    censusRecords(intLineIndex).Status = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 4
                                                                    censusRecords(intLineIndex).Gender = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 5
                                                                    censusRecords(intLineIndex).Age = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 6
                                                                    censusRecords(intLineIndex).BirthYear = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 7
                                                                    censusRecords(intLineIndex).Occupation = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 8
                                                                    censusRecords(intLineIndex).BirthPlace = strLine
                                                            End Select
                                                        Case 1911
                                                            Select Case censusRecords.FieldHeaderData(intLoop).ItemData1
                                                                Case 0
                                                                    censusRecords(intLineIndex).FirstNames = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 1
                                                                    censusRecords(intLineIndex).LastName = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 2
                                                                    censusRecords(intLineIndex).Role = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 3
                                                                    censusRecords(intLineIndex).Status = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 4
                                                                    censusRecords(intLineIndex).Gender = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 5
                                                                    censusRecords(intLineIndex).Occupation = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 6
                                                                    censusRecords(intLineIndex).Age = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 7
                                                                    censusRecords(intLineIndex).BirthYear = strLine
                                                                    Do
                                                                        intLoop += 1
                                                                    Loop Until censusRecords.FieldHeaderData(intLoop).ItemData2 = 1
                                                                Case 8
                                                                    censusRecords(intLineIndex).BirthPlace = strLine
                                                            End Select
                                                        Case 1921
                                                    End Select
                                                Case "US"
                                            End Select
                                        End If
                                    Else
                                        Exit For
                                    End If
                                End If
                            Next
                        Catch ex3 As Exception
                                Throw ex3
'                            MessageBox.Show("Sorry, something went wrong parsing the table body data. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            bInvalidCensus = True
                            bProcessingError = True
                            sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                            sw.WriteLine(Now)
                            sw.WriteLine(m_strWebRef)
                            sw.WriteLine("Table body data parse error")
                            sw.WriteLine(ex3.Message)
                            sw.WriteLine(ex3.Source)
                            sw.WriteLine(ex3.InnerException)
                            sw.WriteLine(ex3.StackTrace)
                            sw.Close()
                            sw.Dispose()
                        End Try
                        If bInvalidCensus = False Then
                            Try
                                'The last record is a blank and has to be removed
                                censusRecords.Delete(intLineIndex)
                                intLoop = 0
                                Do
                                    '===== List of possible fields ====
                                    ' Age
                                    ' Archive reference
                                    ' Birth county
                                    ' Birth county as transcribed
                                    ' Birth place
                                    ' Birth place (other)
                                    ' Birth town
                                    ' Birth town as transcribed
                                    ' Birth year
                                    ' Book number
                                    ' Category
                                    ' City or borough
                                    ' Collections from
                                    ' Country
                                    ' County
                                    ' Family member first name
                                    ' Family member last name
                                    ' First name(s)
                                    ' Folio
                                    ' Folio number
                                    ' Gender
                                    ' House name
                                    ' House number
                                    ' Last name
                                    ' Marital status
                                    ' Occupation
                                    ' Page
                                    ' Page number
                                    ' Parish
                                    ' Parish or township
                                    ' Piece number
                                    ' Record set
                                    ' Registration district
                                    ' Relationship
                                    ' Street
                                    ' Subcategory
                                    ' Town
                                    '===================================
                                    'Additional fields on an Irish Cenus
                                    '===================================
                                    'Marital status as transcribed
                                    'Sex
                                    'Townland/Street
                                    'District electoral division
                                    'Religion
                                    'Literacy
                                    'Read and write
                                    '===================================
                                    'Additional fields introduced by US census and Irish 1841/1851 or Canadian
                                    '===================================
                                    'Barony
                                    'Birth date
                                    'Birth place as transcribed
                                    'Can read
                                    'Can write
                                    'Census year
                                    'City/township
                                    ' District name
                                    'District number
                                    'Dwelling
                                    'Ethnicity
                                    'Family number
                                    'Father's birth place
                                    'Father's first name(s)
                                    'Father's last name
                                    'Film
                                    'Gender:
                                    'Head of household's first name(s)
                                    'Head of household's last name
                                    'Image source
                                    'Immigration year
                                    'Location
                                    'Marriage year (estimated)
                                    'Mother's birth place
                                    'Mother's first name(s)
                                    'Mother's last name
                                    'NARA film
                                    'NARA series
                                    'Occupation
                                    'Province
                                    'Race
                                    'Race or Tribe
                                    'Relationship to head of household
                                    'Residence
                                    'Spouse's birth place
                                    'Spouse's first name(s)
                                    'Spouse's last name
                                    'State
                                    ' Subdistrict name
                                    'Subdistrict number
                                    'Townland
                                    'Year

                                    Select Case listPersonDetails(intLoop)
                                        Case "Age"
                                            Age = listPersonDetails(intLoop + 1)
                                        Case "Archive reference"
                                            ReferenceID = listPersonDetails(intLoop + 1)
                                        Case "Birth county"
                                            BirthCounty = listPersonDetails(intLoop + 1)
                                        Case "Birth county as transcribed"
                                            If BirthCounty.ToLower <> listPersonDetails(intLoop + 1) AndAlso listPersonDetails(intLoop + 1).Trim > "" Then
                                                BirthCounty = listPersonDetails(intLoop + 1)
                                            End If
                                        Case "Birth Place", "Birth place (other)"
                                            'This is the birth country
                                            BirthCountry = listPersonDetails(intLoop + 1)
                                        Case "Birth Town"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Birth town as transcribed"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Birth year"
                                            BirthYear = listPersonDetails(intLoop + 1)
                                        Case "Book number"
                                            BookNumber = listPersonDetails(intLoop + 1)
                                        Case "Category"
                                        Case "City or borough"
                                            CityOrBorough = listPersonDetails(intLoop + 1)
                                        Case "Collections from"
                                        Case "County"
                                            County = listPersonDetails(intLoop + 1)
                                        Case "Country"
                                            CensusCountry = listPersonDetails(intLoop + 1)
                                        Case "District name"
                                            DistrictName = listPersonDetails(intLoop + 1)
                                        Case "Family member first name", "Family member first name(s)"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Family member last name"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "First name(s)"
                                            strName = listPersonDetails(intLoop + 1)
                                        Case "Folio", "Folio number"
                                            strFolio &= " / " & listPersonDetails(intLoop + 1)
                                            PieceFolio = strFolio
                                        Case "Gender", "Gender:"
                                            Gender = listPersonDetails(intLoop + 1)
                                        Case "House Name"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "House Number"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Last name"
                                            strName &= " " & listPersonDetails(intLoop + 1)
                                            PersonName = strName
                                        Case "Marital Status"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Occupation"
                                            Occupation = listPersonDetails(intLoop + 1)
                                        Case "Page", "Page number"
                                            PageNumber = listPersonDetails(intLoop + 1)
                                        Case "Parish", "Parish or township"
                                            Parish = listPersonDetails(intLoop + 1)
                                        Case "Piece number"
                                            strFolio = listPersonDetails(intLoop + 1)
                                        Case "Province"
                                            Province = listPersonDetails(intLoop + 1)
                                        Case "Record set"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Registration district"
                                            RegistrationDistrict = listPersonDetails(intLoop + 1)
                                        Case "Relationship", "Relationship to head"
                                            RelationshipToHead = listPersonDetails(intLoop + 1)
                                        Case "Sex"
                                            Gender = listPersonDetails(intLoop + 1)
                                        Case "Street"
                                            Street = listPersonDetails(intLoop + 1)
                                        Case "Subcategory"
                                            '========
                                            ' TO DO
                                            '========
                                        Case "Subdistrict name"
                                            SubDistrict = listPersonDetails(intLoop + 1)
                                        Case "Town"
                                            Town = listPersonDetails(intLoop + 1)
                                    End Select
                                    intLoop += 1
                                Loop Until intLoop >= listPersonDetails.Count
                            Catch ex4 As Exception
                                Throw ex4
'                                 MessageBox.Show("Sorry, something went wrong parsing the personal details data. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                bProcessingError = True
                                sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                                sw.WriteLine(Now)
                                sw.WriteLine(m_strWebRef)
                                sw.WriteLine("Personal details parse error")
                                sw.WriteLine(ex4.Message)
                                sw.WriteLine(ex4.Source)
                                sw.WriteLine(ex4.InnerException)
                                sw.WriteLine(ex4.StackTrace)
                                sw.Close()
                                sw.Dispose()
                            End Try
                        Else
                            If bProcessingError = False Then
'                                MessageBox.Show("Sorry, this does not appear to be a valid census year currently formatted by the program. Please leave a message for Meredith-1182 if you think this is incorrect.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                            sbld.Clear()
                        End If
                    Else
                        sbld.Clear()
                    End If
                Else
                    If bProcessingError = False Then
'                        MessageBox.Show("Sorry, this does not appear to be a block of Find My Past census text.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    sbld.Clear()
                End If
            Case "FS"
                For Each strLine In astr
                    If strLine.Contains("Canada Census,") = True Then
                        astrSub = strLine.Split(",")
                        If astrSub.Length >= 2 Then
                            censusRecords.CensusYear = CInt(Val(astrSub(1)))
                            sbldSub.Clear()
                            sbldSub.Append(astrSub(0).Trim)
                            sbldSub.Append(" of ")
                            sbldSub.Append(astrSub(1).Trim)
                            sbldSub.Replace(Chr(34), "")
                            m_strEventType = sbldSub.ToString
                        End If
                        censusRecords.CensusCountry = "CA"
                    ElseIf strLine.Contains("England and Wales Census,") = True Then
                        astrSub = strLine.Split(",")
                        If astrSub.Length >= 2 Then
                            censusRecords.CensusYear = CInt(Val(astrSub(1)))
                            sbldSub.Clear()
                            sbldSub.Append(astrSub(0).Trim)
                            sbldSub.Append(" of ")
                            sbldSub.Append(astrSub(1).Trim)
                            sbldSub.Replace(Chr(34), "")
                            m_strEventType = sbldSub.ToString
                        End If
                        censusRecords.CensusCountry = "UK"
                    ElseIf strLine.Contains("New York State Census,") = True Then

                    ElseIf strLine.Contains("Qu�bec, recensement,") = True Then

                    ElseIf strLine.Contains("School Records,") = True Then

                    ElseIf strLine.Contains("United States Census,") Then
                        astrSub = strLine.Split(",")
                        If astrSub.Length >= 2 Then
                            censusRecords.CensusYear = CInt(Val(astrSub(1)))
                            sbldSub.Clear()
                            sbldSub.Append(astrSub(0).Trim)
                            sbldSub.Append(" of ")
                            sbldSub.Append(astrSub(1).Trim)
                            sbldSub.Replace(Chr(34), "")
                            m_strEventType = sbldSub.ToString
                        End If
                        censusRecords.CensusCountry = "US"
                    End If
                Next
                ParseDetailsBlock(astr)
        End Select
        '========================================================================
        '     COMMON (OTHER THAN REFERENCE) PROCESSING AREA TO PRODUCE RESULTS
        '========================================================================
        If bProcessingError = False Then
            Try
                'Now create the output
                sbldWT.Clear()
                If m_strEventYear.Trim = "" Then
                    If censusRecords.CensusYear > 0 Then
                        m_strEventYear = censusRecords.CensusYear.ToString
                    End If
                End If
                Select Case TimelineStyle()
                    Case 0
                        'Narrative
                        'Do nothing
                        sbldWT.Append(":")
                    Case 1
                        'Bold year
                        sbldWT.Append(":'''" & EventYear & "'''  ")
                    Case 2
                        'Dated sections
                        sbldWT.AppendLine("===" & EventYear & " Residence===")
                        sbldWT.Append(":")
                    Case 3
                        'Undated sections
                        sbldWT.AppendLine("===Residence===")
                        sbldWT.Append(":")
                End Select
                'CENSUS_DATE_1841UK
                sbldWT.Append("On the night of the ")
                sbldWT.Append(EventType)
                sbldWT.Append(CensusDateBlock)
                sbldWT.Append("<ref>")
                Select Case censusRecords.DataFormat
                    Case "FMP"
                        If m_strWebRef.Trim > "" Then
                            sbldWT.Append("[")
                            sbldWT.Append(m_strWebRef)
                            sbldWT.Append(" Find My Past] ")
                            sbldWT.Append(EventType)
                            sbldWT.Append(" Transcription")
                            sbldWT.Append(" ($/�/� to view)")
                        End If
                    Case "FS"
                        sbldWT.Append(Citation)
                End Select
                sbldWT.Append("</ref> ")
                sbldWT.Append(PersonName)
                sbldWT.Append(" was at ")
                sbldWT.Append(CreateResidence)
                If censusRecords.Count > 1 Then
                    Select Case RelationshipToHead.ToLower
                        Case "daughter", "mother", "neice", "wife"
                            sbldWT.AppendLine(" with her family members.")
                        Case "head"
                            sbldWT.AppendLine(" as the head of the family.")
                        Case "brother", "nephew", "son"
                            sbldWT.AppendLine(" with his family members.")
                        Case "lodger"
                            sbldWT.AppendLine(" lodging with the household.")
                        Case "visitor"
                            sbldWT.AppendLine(" visiting with the household.")
                        Case Else
                            If Gender.ToLower.StartsWith("m") = True Then
                                sbldWT.AppendLine(" with his family members.")
                            Else
                                sbldWT.AppendLine(" with her family members.")
                            End If
                    End Select
                Else
                    sbldWT.Append(vbCrLf)
                End If
            Catch ex5 As Exception
                                Throw ex5
'                MessageBox.Show("Sorry, something went wrong creating the main narrative and reference details. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                bProcessingError = True
                sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                sw.WriteLine(Now)
                sw.WriteLine(m_strWebRef)
                sw.WriteLine("Main narrative/reference create error")
                sw.WriteLine(ex5.Message)
                sw.WriteLine(ex5.Source)
                sw.WriteLine(ex5.InnerException)
                sw.WriteLine(ex5.StackTrace)
                sw.Close()
                sw.Dispose()
            End Try
            If bProcessingError = False Then
                Select Case TimelineStyle()
                    Case 1
                        Try
                            'Bold year
                            'If CompactFSBlocks() = False Then
                            'Not a compact format so the name will be displayed as a household members list
                            sbldWT.AppendLine("::Household Members")
                            'End If
                            censusData = censusRecords.GetEnumerator()
                            censusData.Reset()
                            While censusData.MoveNext()
                                thisPerson = censusData.Current()
                                For Each element As TI2 In censusRecords.FieldHeaderData

                                Next


                            End While
                            censusData = Nothing
                        Catch ex6 As Exception
                            Throw ex6
'                            MessageBox.Show("Sorry, something went wrong creating the household members details narrative. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            bProcessingError = True
                            sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                            sw.WriteLine(Now)
                            sw.WriteLine(m_strWebRef)
                            sw.WriteLine("Household members narrative type 1 error")
                            sw.WriteLine(ex6.Message)
                            sw.WriteLine(ex6.Source)
                            sw.WriteLine(ex6.InnerException)
                            sw.WriteLine(ex6.StackTrace)
                            sw.Close()
                            sw.Dispose()
                        End Try
                    Case 2, 3
                        Try
                            'Dated and undated sections
                            sbldWT.AppendLine(":{| style=" & Chr(34) & "width: 100%" & Chr(34))
                            'Create the census header line
                            sbldSub.Clear()
                            sbldSub.Append("|")
                            For Each element As TI2 In censusRecords.FieldHeaderData
                                Select Case censusRecords.DataFormat
                                    Case "FMP"
                                        If PermittedFields.Contains(element.TextEntry) Then
                                            If element.TextEntry <> "Last Name" Then
                                                If element.ItemData2 = 1 Then
                                                    If sbldSub.Length > 1 Then
                                                        sbldSub.Append("||")
                                                    End If
                                                    If element.TextEntry = "First Names" Then
                                                        sbldSub.Append("'''")
                                                        sbldSub.Append("Household")
                                                        sbldSub.Append("'''")
                                                    Else
                                                        sbldSub.Append("'''")
                                                        sbldSub.Append(element.TextEntry)
                                                        sbldSub.Append("'''")
                                                    End If
                                                End If
                                            End If
                                        ElseIf censusRecords.CensusCountry = "CA" AndAlso element.TextEntry = "Birth Year" Then
                                            'This is a special case for Canada as they only show the birth year
                                            'so the age must be extrapolated
                                            sbldSub.Append("||'''Age'''")
                                        End If
                                    Case "FS"
                                        If sbldSub.Length > 1 Then
                                            sbldSub.Append("||")
                                        End If
                                        sbldSub.Append("'''")
                                        sbldSub.Append(element.TextEntry)
                                        sbldSub.Append("'''")
                                End Select
                            Next
                            sbldWT.AppendLine(sbldSub.ToString)
                            sbldWT.AppendLine("|-")
                            censusData = censusRecords.GetEnumerator()
                            censusData.Reset()
                            intLoop = 0
                            While censusData.MoveNext()
                                thisPerson = censusData.Current()
                                sbldSub.Clear()
                                sbldSub.Append("|")
                                For Each element As TI2 In censusRecords.FieldHeaderData
                                    Select Case censusRecords.DataFormat
                                        Case "FMP"
                                            If PermittedFields.Contains(element.TextEntry) Then
                                                If element.TextEntry <> "Last Name" Then
                                                    If element.ItemData2 = 1 Then
                                                        'This is a field name in use
                                                        If sbldSub.Length > 1 Then
                                                            sbldSub.Append("||")
                                                        End If
                                                        Select Case censusRecords.CensusCountry
                                                            Case "CA"
                                                                Select Case censusRecords.CensusYear
                                                                    Case 1861, 1901, 1911
                                                                        Select Case element.TextEntry
                                                                            Case "First Names"
                                                                                If thisPerson.FirstNames & " " & thisPerson.LastName = PersonName Then
                                                                                    If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                                                        sbldSub.Append("'''")
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                        sbldSub.Append("'''")
                                                                                    Else
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                    End If
                                                                                Else
                                                                                    sbldSub.Append(thisPerson.FirstNames)
                                                                                    sbldSub.Append(" ")
                                                                                    sbldSub.Append(thisPerson.LastName)
                                                                                End If
                                                                                'Last name is ignored as an element
                                                                            Case "Gender"
                                                                                sbldSub.Append(thisPerson.Gender)
                                                                            Case "Birth Year"
                                                                                sbldSub.Append(thisPerson.BirthYear)
                                                                            Case "Birth Place"
                                                                                sbldSub.Append(thisPerson.BirthPlace)
                                                                            Case "Role"
                                                                                sbldSub.Append(thisPerson.Role)
                                                                        End Select
                                                                    Case 1871, 1881
                                                                        Select Case element.TextEntry.ToLower
                                                                            Case "first names"
                                                                                If thisPerson.FirstNames & " " & thisPerson.LastName = PersonName Then
                                                                                    If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                                                        sbldSub.Append("'''")
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                        sbldSub.Append("'''")
                                                                                    Else
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                    End If
                                                                                Else
                                                                                    sbldSub.Append(thisPerson.FirstNames)
                                                                                    sbldSub.Append(" ")
                                                                                    sbldSub.Append(thisPerson.LastName)
                                                                                End If
                                                                                'Last name is ignored as an element
                                                                            Case "gender"
                                                                                sbldSub.Append(thisPerson.Gender)
                                                                            Case "birth year"
                                                                                sbldSub.Append(thisPerson.BirthYear)
                                                                            Case "birth place"
                                                                                sbldSub.Append(thisPerson.BirthPlace)
                                                                        End Select
                                                                End Select
                                                            Case "EI"
                                                                Select Case censusRecords.CensusYear
                                                                    Case 1901, 1911
                                                                        Select Case element.TextEntry
                                                                            Case "First Names"
                                                                                If thisPerson.FirstNames & " " & thisPerson.LastName = PersonName Then
                                                                                    If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                                                        sbldSub.Append("'''")
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                        sbldSub.Append("'''")
                                                                                    Else
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                    End If
                                                                                Else
                                                                                    sbldSub.Append(thisPerson.FirstNames)
                                                                                    sbldSub.Append(" ")
                                                                                    sbldSub.Append(thisPerson.LastName)
                                                                                End If
                                                                                'Last name is ignored as an element
                                                                            Case "Role"
                                                                                sbldSub.Append(thisPerson.Role)
                                                                            Case "Status"
                                                                                sbldSub.Append(thisPerson.Status)
                                                                            Case "Age"
                                                                                sbldSub.Append(thisPerson.Age)
                                                                            Case "Birth Year"
                                                                                sbldSub.Append(thisPerson.BirthYear)
                                                                            Case "Birth Place"
                                                                                sbldSub.Append(thisPerson.BirthPlace)
                                                                        End Select
                                                                End Select
                                                            Case "UK"
                                                                Select Case censusRecords.CensusYear
                                                                    Case 1841
                                                                        Select Case element.TextEntry
                                                                            Case "First Names"
                                                                                If thisPerson.FirstNames & " " & thisPerson.LastName = PersonName Then
                                                                                    If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                                                        sbldSub.Append("'''")
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                        sbldSub.Append("'''")
                                                                                    Else
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                    End If
                                                                                Else
                                                                                    sbldSub.Append(thisPerson.FirstNames)
                                                                                    sbldSub.Append(" ")
                                                                                    sbldSub.Append(thisPerson.LastName)
                                                                                End If
                                                                                'Last name is ignored as an element
                                                                            Case "Gender"
                                                                                sbldSub.Append(thisPerson.Gender)
                                                                            Case "Age"
                                                                                sbldSub.Append(thisPerson.Age)
                                                                            Case "Birth Year"
                                                                                sbldSub.Append(thisPerson.BirthYear)
                                                                            Case "Birth Place"
                                                                                sbldSub.Append(thisPerson.BirthPlace)
                                                                        End Select
                                                                    Case 1851 To 1901
                                                                        Select Case element.TextEntry
                                                                            Case "First Names"
                                                                                If thisPerson.FirstNames & " " & thisPerson.LastName = PersonName Then
                                                                                    If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                                                        sbldSub.Append("'''")
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                        sbldSub.Append("'''")
                                                                                    Else
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                    End If
                                                                                Else
                                                                                    sbldSub.Append(thisPerson.FirstNames)
                                                                                    sbldSub.Append(" ")
                                                                                    sbldSub.Append(thisPerson.LastName)
                                                                                End If
                                                                                'Last name is ignored as an element
                                                                            Case "Role"
                                                                                sbldSub.Append(thisPerson.Role)
                                                                            Case "Status"
                                                                                sbldSub.Append(thisPerson.Status)
                                                                            Case "Gender"
                                                                                sbldSub.Append(thisPerson.Gender)
                                                                            Case "Age"
                                                                                sbldSub.Append(thisPerson.Age)
                                                                            Case "Birth Year"
                                                                                sbldSub.Append(thisPerson.BirthYear)
                                                                            Case "Occupation"
                                                                                sbldSub.Append(thisPerson.Occupation)
                                                                            Case "Birth Place"
                                                                                sbldSub.Append(thisPerson.BirthPlace)
                                                                        End Select
                                                                    Case 1911
                                                                        Select Case element.TextEntry
                                                                            Case "First Names"
                                                                                If thisPerson.FirstNames & " " & thisPerson.LastName = PersonName Then
                                                                                    If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                                                        sbldSub.Append("'''")
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                        sbldSub.Append("'''")
                                                                                    Else
                                                                                        sbldSub.Append(thisPerson.FirstNames)
                                                                                        sbldSub.Append(" ")
                                                                                        sbldSub.Append(thisPerson.LastName)
                                                                                    End If
                                                                                Else
                                                                                    sbldSub.Append(thisPerson.FirstNames)
                                                                                    sbldSub.Append(" ")
                                                                                    sbldSub.Append(thisPerson.LastName)
                                                                                End If
                                                                                'Last name is ignored as an element
                                                                            Case "Role"
                                                                                sbldSub.Append(thisPerson.Role)
                                                                            Case "Status"
                                                                                sbldSub.Append(thisPerson.Status)
                                                                            Case "Gender"
                                                                                sbldSub.Append(thisPerson.Gender)
                                                                            Case "Occupation"
                                                                                sbldSub.Append(thisPerson.Occupation)
                                                                            Case "Age"
                                                                                sbldSub.Append(thisPerson.Age)
                                                                            Case "Birth Year"
                                                                                sbldSub.Append(thisPerson.BirthYear)
                                                                            Case "Birth Place"
                                                                                sbldSub.Append(thisPerson.BirthPlace)
                                                                        End Select
                                                                    Case 1921
                                                                End Select
                                                            Case "US"
                                                        End Select
                                                    End If
                                                End If
                                            Else
                                                If censusRecords.CensusCountry = "CA" Then
                                                    If element.TextEntry = "Birth Year" Then
                                                        'This is a special case for Canada as they only show the birth year
                                                        'so the age must be extrapolated
                                                        sbldSub.Append("||")
                                                        sbldSub.Append(CStr(censusRecords.CensusYear - thisPerson.BirthYear))
                                                    End If
                                                End If
                                            End If
                                        Case "FS"
                                            If sbldSub.Length > 1 Then
                                                sbldSub.Append("||")
                                            End If
                                            Select Case element.TextEntry
                                                Case "Household"
                                                    If thisPerson.FullName = PersonName Then
                                                        If thisPerson.BirthYear = CInt(Val(BirthYear)) Then
                                                            sbldSub.Append("'''")
                                                            sbldSub.Append(thisPerson.FullName)
                                                            sbldSub.Append("'''")
                                                        Else
                                                            sbldSub.Append(thisPerson.FullName)
                                                        End If
                                                    Else
                                                        sbldSub.Append(thisPerson.FullName)
                                                    End If
                                                Case "Role"
                                                    sbldSub.Append(thisPerson.Role)
                                                Case "Status"
                                                    sbldSub.Append(thisPerson.Status)
                                                Case "Sex"
                                                    sbldSub.Append(thisPerson.Gender)
                                                Case "Age"
                                                    sbldSub.Append(thisPerson.Age)
                                                Case "Birth Place"
                                                    sbldSub.Append(thisPerson.BirthPlace)
                                            End Select
                                    End Select
                                Next
                                sbldWT.AppendLine(sbldSub.ToString)
                                intLoop += 1
                                If intLoop < censusRecords.Count Then
                                    sbldWT.AppendLine("|-")
                                End If
                            End While
                            'sbldWT.Remove(sbldWT.Length - 2, 2)
                            censusData = Nothing
                            sbldWT.AppendLine("|}")
                            If Occupation.Trim > "" Then
                                sbldWT.AppendLine(":Occupation: " & Occupation)
                            End If
                        Catch ex7 As Exception
                                Throw ex7
'                            MessageBox.Show("Sorry, something went wrong creating the household members details table. Please report this to Meredith-1182 giving the Find My Past URL of the census selected.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            bProcessingError = True
                            sw = New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter\FMPErrorData.txt", False)
                            sw.WriteLine(Now)
                            sw.WriteLine(m_strWebRef)
                            sw.WriteLine("Household members details types 2 & 3 error")
                            sw.WriteLine(ex7.Message)
                            sw.WriteLine(ex7.Source)
                            sw.WriteLine(ex7.InnerException)
                            sw.WriteLine(ex7.StackTrace)
                            sw.Close()
                            sw.Dispose()
                        End Try
                End Select
            Else
                sbldWT.Clear()
            End If
        Else
            sbldWT.Clear()
        End If
        Return sbldWT.ToString
    End Function

    Private Sub ParseDetailsBlock(ByVal astr() As String)
        Dim astrSub() As String = Nothing
        Dim strLine As String = ""
        Dim bNextIsCitation As Boolean = False
        Dim bEventPlaceDone As Boolean = False
        For Each strLine In astr
            If NextIsCitation = False Then
                ExtractLineData(strLine)
            Else
                Citation = strLine
                NextIsCitation = False
            End If
        Next
    End Sub
#End Region
#Region "SET FUNCTIONS"
'    Friend Sub SetBooleanFlag(ByVal FlagType As Integer, ByVal state As Boolean)
'        Select Case FlagType
'            Case BRACKET_NAME_TYPES
'                My.Settings.BracketNameTypes = state
'            Case CREATE_EMPTY_PLACEHOLDERS
'                My.Settings.CreateEmptyPlaceholders = state
'            Case FS_BLOCK_COMPACT
'                My.Settings.FSBlockCompact = state
'            Case CLEAR_ON_CLIPBOARD
'                My.Settings.ClearOnClipboard = state
'            Case PAD_SINGLE_DIGITS
'                My.Settings.PadSingleWithZeros = state
'            Case ORDINAL_INDICATORS
'                My.Settings.OrdinalIndicators = state
'            Case CREATE_TEMPLATES
'                My.Settings.CreateTemplates = state
'            Case SHORT_MONTHS
'                My.Settings.ShortMonths = state
'            Case RETAIN_IMPORT_INFO
'                My.Settings.RetainImportInfo = state
'            Case RETAIN_NO_MORE_INFO
'                My.Settings.RetainNoMoreInfo = state
'            Case ADD_NO_MORE_INFO
'                My.Settings.AddNoMoreInfo = state
'        End Select
'        My.Settings.Save()
'    End Sub
'    Friend Sub SetIntegerValue(ByVal FlagType As Integer, ByVal value As Integer)
'        Select Case FlagType
'            Case LAYOUT_STYLE
'                My.Settings.LayoutStyle = value
'            Case TIMELINE_STYLE
'                My.Settings.TimelineStyle = value
'            Case DATES_FORMAT
'                My.Settings.FullDatesFormat = value
'        End Select
'        My.Settings.Save()
'    End Sub
'    Friend Sub SetDecimalValue(ByVal FlagType As Integer, ByVal value As Decimal)
'        Select Case FlagType
'            Case ADD_THRESHOLD
'                My.Settings.AddThreshold = value
'        End Select
'        My.Settings.Save()
'    End Sub
#End Region
#Region "STORAGE FUNCTIONS"
    ''' <summary>
    ''' This is good for Family Search data only
    ''' </summary>
    ''' <param name="TextLine"></param>
    ''' <remarks></remarks>
    Friend Sub ExtractLineData(TextLine As String)
        Dim astr() As String = Nothing
        Dim astrSub() As String = Nothing
        Dim astrSubSub() As String = Nothing
        Dim sbld As StringBuilder = New StringBuilder("")
        Dim strTemp As String = ""
        Dim strDummy01 As String = ""
        Dim strDummy02 As String = ""
        Dim intLineIndex As Integer = 0
        Dim intLoop As Integer = 0

        If m_bInCensusTable = False Then
            If TextLine.Trim > "" Then
                If TextLine.ToLower.StartsWith("affiliate") = True Then
                    If TextLine.ToLower.Contains("film") = True Then
                        'affiliate film number:
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAffiliateFilmNumber = astrSub(1).Trim
                        End If
                    ElseIf TextLine.ToLower.Contains("name") = True Then
                        'affiliate name:
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAffiliateName = astrSub(1).Trim
                        End If
                    ElseIf TextLine.ToLower.Contains("publication") = True Then
                        'affiliate publication number:
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAffiliatePublicationNumber = astrSub(1).Trim
                        End If
                    ElseIf TextLine.ToLower.Contains("record") = True Then
                        'affiliate record type:
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAffiliateRecordType = astrSub(1).Trim
                        End If
                    End If
                    'age (expanded):
                ElseIf TextLine.ToLower.StartsWith("age") = True Then
                    'For the time being they all go to age
                    If TextLine.ToLower.Contains("available after") = True Then
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAge = astrSub(1).Trim
                        End If
                    ElseIf TextLine.ToLower.Contains("original") = True Then
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAge = astrSub(1).Trim
                        End If
                    ElseIf TextLine.ToLower.Contains("expanded") = True Then
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            m_strAge = astrSub(1).Trim
                        End If
                    Else
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            If astrSub(1).Trim > "" Then
                                m_strAge = astrSub(1).Trim
                            End If
                        End If
                    End If
                ElseIf TextLine.ToLower.StartsWith("birth") = True AndAlso TextLine.ToLower.Contains("place:") = False Then
                    If TextLine.Contains("Birth Date:") = True OrElse TextLine.Contains("Birth Date (available after June quarter 1969):") = True Then
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            If astrSub(1).Trim > "" Then
                                If IsDate(astrSub(1)) = True Then
                                    'Only collecting birth year at present
                                    ProcessDate(astrSub(1), m_strBirthYear, strDummy01, strDummy02)
                                    m_strBirthDate = astrSub(1).Trim
                                Else
                                    m_strBirthDate = astrSub(1).Trim
                                End If
                            End If
                        End If
                    ElseIf TextLine.Contains("Birth Year (Estimated):") = True Then
                        'Can be blank, range such as 1812-1816 or year
                        If m_strBirthYear = "" Then
                            astrSub = TextLine.Split(vbTab)
                            If astrSub.Length = 2 Then
                                If astrSub(1).Trim > "" Then
                                    m_strBirthYear = astrSub(1).Trim
                                End If
                            End If
                        End If
                    End If
                ElseIf TextLine.Contains("Book Number:") = True Then
                ElseIf TextLine.Contains("Burial Date:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        If astrSub(1).Trim > "" Then
                            astrSub = TextLine.Split(vbTab)
                            If IsDate(astrSub(1)) = True Then
                                'Only collecting burial year at present
                                ProcessDate(astrSub(1), m_strBurialYear, strDummy01, strDummy02)
                            Else
                                m_strBurialDate = astrSub(1).Trim
                            End If
                            m_bIsBurial = True
                            m_strEventType = "Burial"
                        End If
                    End If
                ElseIf TextLine.Contains("Christening Date:") = True OrElse TextLine.Contains("Christening Date (Original):") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        If astrSub(1).Trim > "" Then
                            astrSub = TextLine.Split(vbTab)
                            If IsDate(astrSub(1)) = True Then
                                ProcessDate(astrSub(1), m_strEventYear, m_strEventMonth, m_strEventDay)
                            Else
                                m_strEventYear = astrSub(1).Trim
                            End If
                            m_bIsBaptism = True
                            m_strEventType = "Christening"
                        End If
                    End If
                ElseIf TextLine.Contains("County:") = True Then
                ElseIf TextLine.Contains("Death Date:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        If astrSub(1).Trim > "" Then
                            astrSub = TextLine.Split(vbTab)
                            If IsDate(astrSub(1)) = True Then
                                ProcessDate(astrSub(1), m_strEventYear, m_strEventMonth, m_strEventDay)
                            Else
                                m_strEventYear = astrSub(1).Trim
                            End If
                            m_bIsDeath = True
                            m_strEventType = "Death"
                        End If
                    End If
                ElseIf TextLine.Contains("Digital Folder Number:") = True Then
                ElseIf TextLine.Contains("Event Year:") = True OrElse TextLine.Contains("Event Date:") = True Then
                    If EventYear.Trim = "" Then
                        astrSub = TextLine.Split(vbTab)
                        If astrSub.Length = 2 Then
                            If IsDate(astrSub(1)) = True Then
                                ProcessDate(astrSub(1), m_strEventYear, m_strEventMonth, m_strEventDay)
                            Else
                                m_strEventYear = astrSub(1).Trim
                            End If
                        End If
                    End If
                ElseIf TextLine.ToLower.Contains("place:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        If astrSub(1).Trim > "" Then
                            If astrSub(1).Trim.Contains(", ") = False Then
                                'these may be concatenated words but have no spaces
                                If astrSub(1).Trim.Contains(",") = True Then
                                    'there are commas
                                    astrSubSub = astrSub(1).Split(",")
                                    sbld.Clear()
                                    For Each strWord In astrSubSub
                                        If strWord.Trim > "" Then
                                            If sbld.Length > 0 Then
                                                sbld.Append(", ")
                                            End If
                                            sbld.Append(StrConv(strWord, VbStrConv.ProperCase))
                                        End If
                                    Next
                                    strTemp = sbld.ToString
                                Else
                                    strTemp = StrConv(astrSub(1), VbStrConv.ProperCase)
                                End If
                            Else
                                strTemp = StrConv(astrSub(1), VbStrConv.ProperCase)
                            End If
                        End If
                        Select Case astrSub(0).ToLower
                            Case "baptism place"
                                m_strBaptismPlace = strTemp
                            Case "birthplace:"
                                m_strBirthplace = strTemp
                            Case "burial place:"
                                m_strBurialPlace = strTemp
                            Case "christening place:"
                                m_strBaptismPlace = strTemp
                            Case "death place:"
                                m_strDeathPlace = strTemp
                            Case "event place:"
                                m_strEventPlace = strTemp
                            Case "father's birthplace:"
                                m_strFathersBirthplace = strTemp
                            Case "marriage place"
                                m_strMarriagePlace = strTemp
                            Case "mother's birthplace:"
                                m_strMothersBirthplace = strTemp
                            Case "residence place:"
                                m_strResidencePlace = strTemp
                        End Select
                    End If
                ElseIf TextLine.Contains("Event Type:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        If astrSub(1).Trim > "" Then
                            m_strEventType = astrSub(1).Trim
                            Select Case m_strEventType.ToLower
                                Case "birth registration"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    ResetBooleans()
                                    m_bIsBirth = True
                                Case "burial"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    ResetBooleans()
                                    m_bIsBurial = True
                                Case "census"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    'This is usually a single entry type - not tabular
                                    ResetBooleans()
                                    m_bIsCensus = True
                                Case "christening"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    ResetBooleans()
                                    m_bIsBaptism = True
                                Case "death"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    ResetBooleans()
                                    m_bIsDeath = True
                                Case "marriage registration", "marriage"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    ResetBooleans()
                                    m_bIsMarriage = True
                                Case "military service"
                                    ResetBooleans()
                                Case "workhouse admission"
                                    'Because these may have been set elsewhere but there is now a defined
                                    'event type, reset any other event type flags
                                    ResetBooleans()
                                    m_bIsWAdmission = True
                            End Select
                        End If
                    End If
                ElseIf TextLine.Contains("Father's Age:") = True Then
                ElseIf TextLine.Contains("Father's Name:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        m_strFathersName = astrSub(1).Trim
                    End If
                ElseIf TextLine.StartsWith("Gender:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub(1).Trim > "" Then
                        m_strGender = astrSub(1).Substring(0, 1).ToUpper
                    Else
                        m_strGender = ""
                    End If
                ElseIf TextLine.Contains("GS Film Number:") Then
                ElseIf TextLine.ToLower.StartsWith("household") = True AndAlso TextLine.ToLower.Contains("role") = True Then
                    m_bInCensusTable = True
                ElseIf TextLine.Contains("Image Number:	00713") = True Then
                ElseIf TextLine.Contains("Indexing Project (Batch) Number:") = True Then
                ElseIf TextLine.Contains("Institution:") = True Then
                ElseIf TextLine.Contains("Line Number:") = True Then
                ElseIf TextLine.Contains("Military Battalion:") = True Then
                    'Not always present on the records
                    astrSub = TextLine.Split(vbTab)
                    If astrSub(1).Trim > "" Then
                        m_strMilitaryBattalion = astrSub(1).Trim
                    Else
                        m_strMilitaryBattalion = ""
                    End If
                ElseIf TextLine.Contains("Military Regiment:") = True Then
                ElseIf TextLine.Contains("Military Unit Note") = True OrElse TextLine.Contains("Military Company/Regiment:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub(1).Trim > "" Then
                        m_strMilitaryUnit = astrSub(1).Trim
                    Else
                        m_strMilitaryUnit = ""
                    End If
                ElseIf TextLine.Contains("Mother's Age") = True Then
                ElseIf TextLine.Contains("Mother's Name:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        m_strMothersName = astrSub(1).Trim
                    End If
                ElseIf TextLine.Contains("Name Note:") = True Then
                ElseIf TextLine.StartsWith("Name:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    m_strName = astrSub(1)
                ElseIf TextLine.ToLower.StartsWith("occupation:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        m_strOccupation = astrSub(1).Trim
                    End If
                ElseIf TextLine.Contains("Page Number:") = True Then
                ElseIf TextLine.Contains("Page:") = True Then
                ElseIf TextLine.Contains("Parish:") = True Then
                ElseIf TextLine.Contains("Piece/Folio:") = True Then
                ElseIf TextLine.Contains("Race:") = True Then
                ElseIf TextLine.Contains("Reference ID:") = True Then
                ElseIf TextLine.Contains("Registration District:") = True Then
                ElseIf TextLine.Contains("Registration Number:") = True Then
                ElseIf TextLine.Contains("Registration Quarter:") = True Then
                ElseIf TextLine.Contains("Registration Year:") = True Then
                    If EventYear.Trim = "" Then
                        astrSub = TextLine.Split(vbTab)
                        If IsDate(astrSub(1)) = True Then
                            m_strEventYear = Year(CDate(astrSub(1)))
                        Else
                            m_strEventYear = astrSub(1).Trim
                        End If
                    End If
                ElseIf TextLine.ToLower.StartsWith("relationship to head of household") = True Then
                    astrSub = TextLine.Split(vbTab)
                    If astrSub.Length = 2 Then
                        m_strRelationshipToHead = astrSub(1).Trim
                    End If
                ElseIf TextLine.Contains("Residence Note:") = True Then
                ElseIf TextLine.Contains("Schedule Type:") = True Then
                ElseIf TextLine.Contains("Ship Name:") = True Then
                ElseIf TextLine.StartsWith("Spouse's Name:") = True Then
                    astrSub = TextLine.Split(vbTab)
                    m_strSpouseName = astrSub(1)
                ElseIf TextLine.Contains("Sub-District:") = True Then
                ElseIf TextLine.Contains("System Origin:") = True Then
                ElseIf TextLine.Contains("Titles and Terms:") = True Then
                ElseIf TextLine.Contains("Volume:") = True Then
                ElseIf TextLine.Contains("Citing this Record:") = True Then
                    m_bNextIsCitation = True
                End If
            End If
        Else
            'This line is a census table line
            If TextLine.Trim > "" Then
                If TextLine.ToLower.StartsWith("citing this record") Then
                    m_bNextIsCitation = True
                    m_bInCensusTable = False
                Else
                    astrSub = TextLine.Split(vbTab)
                    intLineIndex = censusRecords.Add()
                    intLoop = 0
                    For Each strTemp In astrSub
                        Select Case intLoop
                            Case 0
                                censusRecords(intLineIndex).FullName = strTemp
                                'Because this is always FS we can seed the birth date (which is not used)
                                'to get the bolding for the person
                                If strTemp = PersonName Then
                                    censusRecords(intLineIndex).BirthYear = CInt(Val(m_strBirthYear))
                                End If
                            Case 1
                                censusRecords(intLineIndex).Role = strTemp
                            Case 2
                                censusRecords(intLineIndex).Gender = strTemp
                            Case 3
                                censusRecords(intLineIndex).Age = CInt(Val(strTemp))
                            Case 4
                                censusRecords(intLineIndex).BirthPlace = strTemp
                        End Select
                        intLoop += 1
                    Next
                End If
            End If
        End If
    End Sub
    Friend Sub CreateEventDatesFromDate(DateText As String)
        ProcessDate(DateText, m_strEventYear, m_strEventMonth, m_strEventDay)
    End Sub
    Friend Function SetEventTypeForCitationType() As Boolean
        Dim bSuccess As Boolean = False
        If m_strCitationType.Trim > "" Then
            Select Case m_strCitationType
                Case FSCOLLNAME__MASUSABRTHCHR_1639_1915
                    m_strEventType = "Birth"
                    bSuccess = True
            End Select
        End If

        Return bSuccess
    End Function
    Friend Function CreateProjectTemplateCode() As String
        Dim sbld As StringBuilder = New StringBuilder("{{")

        Return sbld.ToString
    End Function
    Private Sub ProcessDate(ByVal DateIn As String, ByRef YearOut As String, ByRef MonthOut As String, ByRef DayOut As String)
        Try
            YearOut = Year(CDate(DateIn)).ToString
            MonthOut = Month(CDate(DateIn)).ToString
            DayOut = Microsoft.VisualBasic.Day(CDate(DateIn)).ToString
        Catch ex As Exception
            YearOut = Year(CDate(DateIn))
            MonthOut = ""
            DayOut = ""
        End Try
    End Sub
    Friend Property NextIsCitation As Boolean
        Get
            Return m_bNextIsCitation
        End Get
        Set(value As Boolean)
            m_bNextIsCitation = value
        End Set
    End Property
    Friend ReadOnly Property IsBaptism As Boolean
        Get
            Return m_bIsBaptism
        End Get
    End Property
    Friend ReadOnly Property IsBirth As Boolean
        Get
            Return m_bIsBirth
        End Get
    End Property
    Friend ReadOnly Property IsBurial As Boolean
        Get
            Return m_bIsBurial
        End Get
    End Property
    Friend ReadOnly Property IsCensus As Boolean
        Get
            Return m_bIsCensus
        End Get
    End Property
    Friend ReadOnly Property IsDeath As Boolean
        Get
            Return m_bIsDeath
        End Get
    End Property
    Friend ReadOnly Property IsMarriage As Boolean
        Get
            Return m_bIsMarriage
        End Get
    End Property
    Friend ReadOnly Property IsWorkshopAdmission As Boolean
        Get
            Return m_bIsWAdmission
        End Get
    End Property
    Friend Property IsFamilySearch As Boolean
        Get
            Return m_bIsFS
        End Get
        Set(value As Boolean)
            m_bIsFS = value
        End Set
    End Property
    Friend Property AffiateFilmNumber As String
        Get
            Return m_strAffiliateFilmNumber
        End Get
        Set(value As String)
            m_strAffiliateFilmNumber = value
        End Set
    End Property
    Friend Property AffiliateRecordType As String
        Get
            Return m_strAffiliateRecordType
        End Get
        Set(value As String)
            m_strAffiliateRecordType = value
        End Set
    End Property
    Friend Property Age As String
        Get
            Return m_strAge
        End Get
        Set(value As String)
            m_strAge = value
        End Set
    End Property
    Friend Property BirthCounty As String
        Get
            Return m_strBirthCounty
        End Get
        Set(value As String)
            m_strBirthCounty = value
        End Set
    End Property
    Friend Property BirthCountry As String
        Get
            Return m_strBirthCountry
        End Get
        Set(value As String)
            m_strBirthCountry = value
        End Set
    End Property
    Friend Property BirthDate As String
        Get
            Return m_strBirthDate
        End Get
        Set(value As String)
            m_strBirthDate = value
        End Set
    End Property
    Friend Property BirthYear As String
        Get
            Return m_strBirthYear
        End Get
        Set(value As String)
            m_strBirthYear = value
        End Set
    End Property
    Friend Property BirthPlace As String
        Get
            Return m_strBirthplace
        End Get
        Set(value As String)
            m_strBirthplace = value
        End Set
    End Property
    Friend Property BookNumber As String
        Get
            Return m_strBookNumber
        End Get
        Set(value As String)
            m_strBookNumber = value
        End Set
    End Property
    Friend Property BurialDate As String
        Get
            Return m_strBurialDate
        End Get
        Set(value As String)
            m_strBurialDate = value
        End Set
    End Property
    Friend Property BurialPlace As String
        Get
            Return m_strBurialPlace
        End Get
        Set(value As String)
            m_strBurialPlace = value
        End Set
    End Property
    Friend Property BurialYear As String
        Get
            Return m_strBurialYear
        End Get
        Set(value As String)
            m_strBurialYear = value
        End Set
    End Property
    Friend Property BaptismDate As String
        Get
            Return m_strBaptismDate
        End Get
        Set(value As String)
            m_strBaptismDate = value
        End Set
    End Property
    Friend Property BaptismPlace As String
        Get
            Return m_strBaptismPlace
        End Get
        Set(value As String)
            m_strBaptismPlace = value
        End Set
    End Property
    Friend Property Citation As String
        Get
            Return m_strCitation
        End Get
        Set(value As String)
            m_strCitation = value
            If value.ToLower.Contains("birth") = True Then
                If value.ToLower.Contains("registration") = False Then
                    If m_bIsBirth = False Then
                        m_bIsBirth = True
                        m_strCitationType = "Birth"
                    End If
                Else
                    If m_bIsBirth = False Then
                        m_bIsBirth = True
                        m_strCitationType = "Birth Registration"
                    Else
                        'Make sure it says registration
                        m_strCitationType = "Birth Registration"
                    End If
                    If value.Contains("Massachusetts Births and Christenings, 1639-1915,") Then
                        m_bIsBirth = True
                        m_strCitationType = FSCOLLNAME__MASUSABRTHCHR_1639_1915

                    End If

                End If
            End If
            'Have to set baptism as well and sort out later which it is
            If value.ToLower.Contains("baptism") = True Or value.ToLower.Contains("christen") = True Then
                If m_bIsBaptism = False Then
                    m_bIsBaptism = True
                    m_strCitationType = "Baptism"
                Else
                    If m_bIsBirth = True Then
                        'The baptism flag is already set so zap the type
                        m_strCitationType = "Baptism"
                    End If
                End If
            End If
            If value.ToLower.Contains("burial") = True Then
                If m_bIsBurial = False Then
                    m_bIsBurial = True
                End If
            End If
            If value.ToLower.Contains("census") = True Then
                If m_bIsCensus = False Then
                    m_bIsCensus = True
                    m_strCitationType = "Census"
                End If
            End If
            If value.ToLower.Contains("marriage") = True Then
                If value.ToLower.Contains("registration") = False Then
                    If m_bIsMarriage = False Then
                        m_bIsMarriage = True
                        m_strCitationType = "Marriage"
                    End If
                Else
                    If m_bIsMarriage = False Then
                        m_bIsMarriage = True
                        m_strCitationType = "Marriage Registration"
                    Else
                        'Make sure it says registration
                        m_strCitationType = "Marriage Registration"
                    End If
                End If
            End If
            If value.ToLower.Contains("death") = True Then
                If value.ToLower.Contains("registration") = False Then
                    If m_bIsDeath = False Then
                        m_bIsDeath = True
                        m_strCitationType = "Death"
                    End If
                Else
                    If m_bIsDeath = False Then
                        m_bIsDeath = True
                        m_strCitationType = "Death Registration"
                    Else
                        'Make sure it says registration
                        m_strCitationType = "Death Registration"
                    End If
                End If
            End If
            If value.ToLower.Contains("united kingdom, world war i service records") Then
                m_strCitationType = "United Kingdom, World War I Service Records, 1914-1920,"
            End If
        End Set
    End Property
    Friend ReadOnly Property CitationType As String
        Get
            Return m_strCitationType
        End Get
    End Property
    ''' <summary>
    ''' Normally used for FMP census only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property CityOrBorough As String
        Get
            Return m_strCityBorough
        End Get
        Set(value As String)
            m_strCityBorough = value
        End Set
    End Property
    Friend Property County As String
        Get
            Return m_strCounty
        End Get
        Set(value As String)
            m_strCounty = value
        End Set
    End Property
    Friend Property CensusCountry As String
        Get
            Return m_strCensusCountry
        End Get
        Set(value As String)
            m_strCensusCountry = value
        End Set
    End Property
    Friend Property DeathDate As String
        Get
            Return m_strDeathDate
        End Get
        Set(value As String)
            m_strDeathDate = value
        End Set
    End Property
    Friend Property DeathPlace As String
        Get
            Return m_strDeathPlace
        End Get
        Set(value As String)
            m_strDeathPlace = value
        End Set
    End Property
    Friend Property DigitalFolderNumber As String
        Get
            Return m_strDigitalFolderNumber
        End Get
        Set(value As String)
            m_strDigitalFolderNumber = value
        End Set
    End Property
    Friend Property DistrictName As String
        Get
            Return m_strDistrictName
        End Get
        Set(value As String)
            m_strDistrictName = value
        End Set
    End Property
    Friend Property EventDay As String
        Get
            Return m_strEventDay
        End Get
        Set(value As String)
            m_strEventDay = value
        End Set
    End Property
    Friend Property EventMonth As String
        Get
            Return m_strEventMonth
        End Get
        Set(value As String)
            m_strEventMonth = value
        End Set
    End Property
    Friend Property EventYear As String
        Get
            Return m_strEventYear
        End Get
        Set(value As String)
            m_strEventYear = value
        End Set
    End Property
    Friend Property EventPlace As String
        Get
            Return m_strEventPlace
        End Get
        Set(value As String)
            m_strEventPlace = value
        End Set
    End Property
    Friend Property EventType As String
        Get
            Return m_strEventType
        End Get
        Set(value As String)
            m_strEventType = value
        End Set
    End Property
    Friend Property FathersAge As String
        Get
            Return m_strFathersAge
        End Get
        Set(value As String)
            m_strFathersAge = value
        End Set
    End Property
    Friend Property FathersBirthplace As String
        Get
            Return m_strFathersBirthplace
        End Get
        Set(value As String)
            m_strFathersBirthplace = value
        End Set
    End Property
    Friend Property FathersName As String
        Get
            Return m_strFathersName
        End Get
        Set(value As String)
            m_strFathersAge = value
        End Set
    End Property
    Friend WriteOnly Property FMPCensusURL As String
        Set(value As String)
            m_strWebRef = value
        End Set
    End Property
    Friend Property Gender As String
        Get
            Return m_strGender
        End Get
        Set(value As String)
            m_strGender = value
        End Set
    End Property
    Friend Property GSFilmNumber As String
        Get
            Return m_strGSFilmNumber
        End Get
        Set(value As String)
            m_strGSFilmNumber = value
        End Set
    End Property
    Friend Property HouseName As String
        Get
            Return m_strHouseName
        End Get
        Set(value As String)
            m_strHouseName = value
        End Set
    End Property
    Friend Property HouseNumber As String
        Get
            Return m_strHouseNumber
        End Get
        Set(value As String)
            m_strHouseNumber = value
        End Set
    End Property
    Friend Property ImageNumber As String
        Get
            Return m_strImageNumber
        End Get
        Set(value As String)
            m_strImageNumber = value
        End Set
    End Property
    Friend Property IndexingProjectBatchNumber As String
        Get
            Return m_strIndexingProjectBatchNumber
        End Get
        Set(value As String)
            m_strIndexingProjectBatchNumber = value
        End Set
    End Property
    Friend Property Institution As String
        Get
            Return m_strInstitution
        End Get
        Set(value As String)
            m_strInstitution = value
        End Set
    End Property
    Friend Property LineNumber As String
        Get
            Return m_strLineNumber
        End Get
        Set(value As String)
            m_strLineNumber = value
        End Set
    End Property
    Friend Property MarriagePlace As String
        Get
            Return m_strMarriagePlace
        End Get
        Set(value As String)
            m_strMarriagePlace = value
        End Set
    End Property
    Friend Property MilitaryBattalion As String
        Get
            Return m_strMilitaryBattalion
        End Get
        Set(value As String)
            m_strMilitaryBattalion = value
        End Set
    End Property
    Friend Property MilitaryUnit As String
        Get
            Return m_strMilitaryUnit
        End Get
        Set(value As String)
            m_strMilitaryUnit = value
        End Set
    End Property
    Friend Property MothersAge As String
        Get
            Return m_strMothersAge
        End Get
        Set(value As String)
            m_strMothersAge = value
        End Set
    End Property
    Friend Property MothersBirthplace As String
        Get
            Return m_strMothersBirthplace
        End Get
        Set(value As String)
            m_strMothersBirthplace = value
        End Set
    End Property
    Friend Property MothersName As String
        Get
            Return m_strMothersName
        End Get
        Set(value As String)
            m_strMothersName = value
        End Set
    End Property
    Friend Property NameNote As String
        Get
            Return m_strNameNote
        End Get
        Set(value As String)
            m_strNameNote = value
        End Set
    End Property
    Friend Property PersonName As String
        Get
            Return m_strName
        End Get
        Set(value As String)
            m_strName = value
        End Set
    End Property
    Friend Property Province As String
        Get
            Return m_srtProvince
        End Get
        Set(value As String)
            m_srtProvince = value
        End Set
    End Property
    Friend Property Occupation As String
        Get
            Return m_strOccupation
        End Get
        Set(value As String)
            m_strOccupation = value
        End Set
    End Property
    Friend Property PageNumber As String
        Get
            Return m_strPageNumber
        End Get
        Set(value As String)
            m_strPageNumber = value
        End Set
    End Property
    Friend Property Page As String
        Get
            Return m_strPage
        End Get
        Set(value As String)
            m_strPage = value
        End Set
    End Property
    Friend Property Parish As String
        Get
            Return m_strParish
        End Get
        Set(value As String)
            m_strParish = value
        End Set
    End Property
    Friend Property PieceFolio As String
        Get
            Return m_strPieceFolio
        End Get
        Set(value As String)
            m_strPieceFolio = value
        End Set
    End Property
    Friend Property Race As String
        Get
            Return m_strRace
        End Get
        Set(value As String)
            m_strRace = value
        End Set
    End Property
    Friend Property ReferenceID As String
        Get
            Return m_strReferenceID
        End Get
        Set(value As String)
            m_strReferenceID = value
        End Set
    End Property
    Friend Property RegistrationDistrict As String
        Get
            Return m_strRegistrationDistrict
        End Get
        Set(value As String)
            m_strRegistrationDistrict = value
        End Set
    End Property
    Friend Property RegistrationNumber As String
        Get
            Return m_strRegistrationNumber
        End Get
        Set(value As String)
            m_strRegistrationNumber = value
        End Set
    End Property
    Friend Property RegistrationQuarter As String
        Get
            Return m_strRegistrationQuarter
        End Get
        Set(value As String)
            m_strRegistrationQuarter = value
        End Set
    End Property
    Friend Property RegistrationYear As String
        Get
            Return m_strRegistrationYear
        End Get
        Set(value As String)
            m_strRegistrationYear = value
        End Set
    End Property
    Friend Property RelationshipToHead As String
        Get
            Return m_strRelationshipToHead
        End Get
        Set(value As String)
            m_strRelationshipToHead = value
        End Set
    End Property
    Friend Property ResidenceNote As String
        Get
            Return m_strResidenceNote
        End Get
        Set(value As String)
            m_strResidenceNote = value
        End Set
    End Property
    Friend Property ResidencePlace As String
        Get
            Return m_strResidencePlace
        End Get
        Set(value As String)
            m_strResidencePlace = value
        End Set
    End Property
    Friend Property ScheduleType As String
        Get
            Return m_strScheduleType
        End Get
        Set(value As String)
            m_strScheduleType = value
        End Set
    End Property
    Friend Property ShipName As String
        Get
            Return m_strShipName
        End Get
        Set(value As String)
            m_strShipName = value
        End Set
    End Property
    Friend Property SpouseName As String
        Get
            Return m_strSpouseName
        End Get
        Set(value As String)
            m_strSpouseName = value
        End Set
    End Property
    ''' <summary>
    ''' The street from a census form (mainly FMP)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property Street As String
        Get
            Return m_strStreet
        End Get
        Set(value As String)
            m_strStreet = value
        End Set
    End Property
    Friend Property SubDistrict As String
        Get
            Return m_strSubDistrict
        End Get
        Set(value As String)
            m_strSubDistrict = value
        End Set
    End Property
    Friend Property SystemOrigin As String
        Get
            Return m_strSystemOrigin
        End Get
        Set(value As String)
            m_strSystemOrigin = value
        End Set
    End Property
    Friend Property TitlesAndTerms As String
        Get
            Return m_strTitlesAndTerms
        End Get
        Set(value As String)
            m_strTitlesAndTerms = value
        End Set
    End Property
    ''' <summary>
    ''' FMP census town
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property Town As String
        Get
            Return m_strTown
        End Get
        Set(value As String)
            m_strTown = value
        End Set
    End Property
    Friend Property Volume As String
        Get
            Return m_strVolume
        End Get
        Set(value As String)
            m_strVolume = value
        End Set
    End Property
    Friend Sub ClearStorage()
        censusRecords.Clear()
        censusRecords.CensusCountry = ""
        censusRecords.CensusYear = 0
        censusRecords.DataFormat = ""
        censusRecords.FieldHeaderData.Clear()
        m_bInCensusTable = False
        m_bIsBaptism = False
        m_bIsBirth = False
        m_bIsBurial = False
        m_bIsCensus = False
        m_bIsDeath = False
        m_bIsMarriage = False
        m_bIsWAdmission = False
        m_bNextIsCitation = False
        m_bIsFS = False
        m_strAffiliateFilmNumber = ""
        m_strAffiliateRecordType = ""
        m_strAge = ""
        m_strBaptismDate = ""
        m_strBaptismPlace = ""
        m_strBirthCounty = ""
        m_strBirthCountry = ""
        m_strBirthDate = ""
        m_strBirthplace = ""
        m_strBirthYear = ""
        m_strBookNumber = ""
        m_strBurialDate = ""
        m_strBurialPlace = ""
        m_strBurialYear = ""
        m_strCitation = ""
        m_strCityBorough = ""
        m_strCounty = ""
        m_strCensusCountry = ""
        m_strDeathDate = ""
        m_strDeathPlace = ""
        m_strDigitalFolderNumber = ""
        m_strDistrictName = ""
        m_strEventDay = ""
        m_strEventMonth = ""
        m_strEventPlace = ""
        m_strEventType = ""
        m_strEventYear = ""
        m_strFathersAge = ""
        m_strFathersBirthplace = ""
        m_strFathersName = ""
        m_strGender = ""
        m_strGSFilmNumber = ""
        m_strHouseName = ""
        m_strHouseNumber = ""
        m_strImageNumber = ""
        m_strIndexingProjectBatchNumber = ""
        m_strInstitution = ""
        m_strLineNumber = ""
        m_strMarriagePlace = ""
        m_strMothersAge = ""
        m_strMothersBirthplace = ""
        m_strMothersName = ""
        m_strName = ""
        m_strNameNote = ""
        m_strOccupation = ""
        m_strPage = ""
        m_strPageNumber = ""
        m_strParish = ""
        m_strPieceFolio = ""
        m_srtProvince = ""
        m_strRace = ""
        m_strReferenceID = ""
        m_strRegistrationDistrict = ""
        m_strRegistrationNumber = ""
        m_strRegistrationQuarter = ""
        m_strRegistrationYear = ""
        m_strRelationshipToHead = ""
        m_strResidenceNote = ""
        m_strResidencePlace = ""
        m_strScheduleType = ""
        m_strShipName = ""
        m_strSpouseName = ""
        m_strStreet = ""
        m_strSubDistrict = ""
        m_strSystemOrigin = ""
        m_strTitlesAndTerms = ""
        m_strTown = ""
        m_strVolume = ""
    End Sub
    Private Sub ResetBooleans()
        m_bIsBaptism = False
        m_bIsBirth = False
        m_bIsBurial = False
        m_bIsCensus = False
        m_bIsDeath = False
        m_bIsMarriage = False
        m_bIsWAdmission = False
    End Sub
#End Region
    ''' <summary>
    ''' Replaces quote marks [ ' ] with [ � ] 
    ''' </summary>
    ''' <param name="incomingText"></param>
    ''' <returns>Parsed string</returns>
    Friend Function ParseCharacters(incomingText As String) As String
        Dim sbld As StringBuilder = New StringBuilder(incomingText)
        sbld.Replace("'", Chr(146))
        Return sbld.ToString
    End Function

    ''' <summary>
    ''' Restores quote marks [ � ] with [ ' ]
    ''' </summary>
    ''' <param name="incomingText"></param>
    ''' <returns>Parsed string</returns>
    Friend Function UnParseCharacters(incomingText As String) As String
        Dim sbld As StringBuilder = New StringBuilder(incomingText)
        sbld.Replace(Chr(146), "'")
        Return sbld.ToString
    End Function

    Friend Function FileOrExtensionFromPath(ByVal PathAndFileName As String, ByVal ExtractExtension As Boolean) As String
        Dim strReturn As String = ""
        Dim intInPos As Integer = 0
        Dim strFileName As String = NameFromPathAndFilename(PathAndFileName, False)

        intInPos = InStrRev(strFileName, ".")
        If intInPos > 0 Then
            If ExtractExtension = False Then
                strReturn = strFileName.Substring(0, intInPos - 1)
            Else
                strReturn = strFileName.Substring(intInPos)
            End If
        Else
            strReturn = strFileName
        End If

        Return strReturn
    End Function

    Friend Function NameFromPathAndFilename(ByVal PathAndFileName As String, ByVal ExtractPath As Boolean) As String
        Dim intInPos As Integer = 0
        Dim strTemp As String = ""
        Dim strSep As String = "\"

        If PathAndFileName Is Nothing = False Then
            If PathAndFileName.Trim > "" Then
                If PathAndFileName.Contains(strSep) = False Then
                    'It could be a web address
                    strSep = "/"
                End If
                If PathAndFileName.Contains(strSep) = True Then
                    intInPos = InStrRev(PathAndFileName, strSep)
                    If intInPos > 0 Then
                        If ExtractPath = True Then
                            strTemp = PathAndFileName.Substring(0, intInPos - 1)
                        Else
                            strTemp = PathAndFileName.Substring(intInPos)
                        End If
                    Else
                        strTemp = PathAndFileName
                    End If
                Else
                    strTemp = PathAndFileName
                End If
            Else
                strTemp = PathAndFileName
            End If
        End If

        Return strTemp
    End Function
End Module

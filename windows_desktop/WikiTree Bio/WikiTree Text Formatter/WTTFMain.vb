Imports System.IO, System.Text
Public Class WTTFMain
    Private m_bLoading As Boolean = True
    Private classFSB As New FamilySearchTextBlockConverter
    'Private classFSC As New FamilySearchCensusConverter
    'Private classFMPC As New FindMyPastCensusConverter

#Region "OTHER ROUTINES"
    Private Sub FSTPaste_Enter(sender As Object, e As EventArgs) Handles txtFSTPaste.Enter
        If txtFSTPaste.Text.Trim > "" Then
            txtFSTPaste.SelectAll()
        End If
    End Sub
    Private Sub FSCPaste_Enter(sender As Object, e As EventArgs) Handles txtFSCPaste.Enter
        If txtFSCPaste.Text.Trim > "" Then
            txtFSCPaste.SelectAll()
        End If
    End Sub
    Private Sub FMPPaste_Enter(sender As Object, e As EventArgs) Handles txtFMPPaste.Enter
        If txtFMPPaste.Text.Trim > "" Then
            txtFMPPaste.SelectAll()
        End If
    End Sub
#End Region 'other routines

#Region "MENU AND COMMAND RELATED ROUTINES"
    Private Sub FSTOKClick(sender As Object, e As EventArgs) Handles cmdFSTOK.Click
        If txtFSTPaste.Text.Trim > "" Then
            classFSB.SuppledFather = txtFSTFather.Text.Trim
            classFSB.SuppliedMother = txtFSTMother.Text.Trim
            classFSB.SuppliedSpouse = txtFSTSpouse.Text.Trim
            txtFSTResult.Text = classFSB.TabulateFSBlock(txtFSTPaste.Text)
            If classFSB.ErrorMessages.Count > 0 Then
                MessageBox.Show(classFSB.ErrorMessages(0), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub FSTCopyClicked(sender As Object, e As EventArgs) Handles cmdCopyFST.Click
        If txtFSTResult.Text > "" Then
            Clipboard.SetText(txtFSTResult.Text)
            If ClearOnClipboardCopy() = True Then
                txtFSTPaste.Text = ""
                txtFSTResult.Text = ""
                txtFSTFather.Text = ""
                txtFSTMother.Text = ""
                txtFSTSpouse.Text = ""
            End If
            MessageBox.Show("The formatted text has been added to the clipboard.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Nothing to copy!", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub FSTCopy2Assembly(sender As Object, e As EventArgs) Handles cmdCoptFSTToAssembly.Click
        If txtFSTResult.Text > "" Then
            Dim frmCS As New CopySelector
            Dim sbld As StringBuilder = New StringBuilder("")
            frmCS.ShowDialog()
            If frmCS.CopySelection > -1 Then
                Select Case frmCS.CopySelection
                    Case 0
                        txtAssembly.Text = txtFSTResult.Text
                    Case 1
                        'Add above
                        sbld.AppendLine(txtFSTResult.Text.Trim)
                        sbld.Append(txtAssembly.Text)
                        txtAssembly.Text = sbld.ToString
                    Case 2
                        'Add below
                        sbld.AppendLine(txtAssembly.Text)
                        sbld.AppendLine(txtFSTResult.Text.Trim)
                        txtAssembly.Text = sbld.ToString
                End Select
                If ClearOnClipboardCopy() = True Then
                    txtFSTPaste.Text = ""
                    txtFSTResult.Text = ""
                    txtFSTFather.Text = ""
                    txtFSTMother.Text = ""
                    txtFSTSpouse.Text = ""
                End If
            End If
            frmCS.Close()
        Else
            MessageBox.Show("Nothing to copy!", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub FSCensusOKClick(sender As Object, e As EventArgs) Handles cmdFSCensusOK.Click
        Dim strText As String = ""
        Dim sbld As StringBuilder = New StringBuilder("")
        If txtFSCPaste.Text.Trim > "" Then
            Clipboard.Clear()
            strText = SelectCensus(txtFSCPaste.Text.Trim, "FS")
            If strText.Trim > "" Then
                txtFSCResult.Text = strText
            Else
                MessageBox.Show("There was problem", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub FSCCopyClick(sender As Object, e As EventArgs) Handles cmdFSCensusCopy.Click
        Clipboard.SetText(txtFSCResult.Text)
        If ClearOnClipboardCopy() = True Then
            txtFSCPaste.Text = ""
            txtFSCResult.Text = ""
        End If
        MessageBox.Show("The formatted census text has been added to the clipboard.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub FSCcopyToAssembly(sender As Object, e As EventArgs) Handles cmdFSCCopy2Assy.Click
        If txtFSTResult.Text > "" Then
            Dim frmCS As New CopySelector
            Dim sbld As StringBuilder = New StringBuilder("")
            frmCS.ShowDialog()
            If frmCS.CopySelection > -1 Then
                Select Case frmCS.CopySelection
                    Case 0
                        txtAssembly.Text = txtFSCResult.Text
                    Case 1
                        'Add above
                        sbld.AppendLine(txtFSCResult.Text.Trim)
                        sbld.Append(txtAssembly.Text)
                        txtAssembly.Text = sbld.ToString
                    Case 2
                        'Add below
                        sbld.AppendLine(txtAssembly.Text)
                        sbld.AppendLine(txtFSCResult.Text.Trim)
                        txtAssembly.Text = sbld.ToString
                End Select
                If ClearOnClipboardCopy() = True Then
                    txtFSCPaste.Text = ""
                    txtFSCResult.Text = ""
                End If
            End If
            frmCS.Close()
        Else
            MessageBox.Show("Nothing to copy!", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub FMPCensusOKClicked(sender As Object, e As EventArgs) Handles cmdFMPCensusOK.Click
        Dim strText As String = ""
        Dim sbld As StringBuilder = New StringBuilder("")
        If txtFMPPaste.Text.Trim > "" Then
            If txtFMPURL.Text.Trim = "" Then
                If MessageBox.Show("The address line of the record from the web page has not been pasted into the URL box. If you enter nothing a <ref></ref> place holder will be left for you to fill. Do you want to continue (Yes) or try again after filling the box (No)", AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            Else
                FMPCensusURL = txtFMPURL.Text.Trim
            End If
            Clipboard.Clear()

            strText = SelectCensus(txtFMPPaste.Text.Trim, "FMP")
            If strText.Trim > "" Then
                txtFMPResult.Text = strText
            Else
                MessageBox.Show("There was problem", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub FMPCensusCopyClick(sender As Object, e As EventArgs) Handles cmdFMPCensusCopy.Click
        Clipboard.SetText(txtFMPResult.Text)
        If ClearOnClipboardCopy() = True Then
            txtFMPURL.Text = ""
            txtFMPPaste.Text = ""
            txtFMPResult.Text = ""
        End If
        MessageBox.Show("The formatted census text has been added to the clipboard.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub FMPCensusToAssembly(sender As Object, e As EventArgs) Handles cmdFMPCtoAssembly.Click
        If txtFSTResult.Text > "" Then
            Dim frmCS As New CopySelector
            Dim sbld As StringBuilder = New StringBuilder("")
            frmCS.ShowDialog()
            If frmCS.CopySelection > -1 Then
                Select Case frmCS.CopySelection
                    Case 0
                        txtAssembly.Text = txtFMPResult.Text
                    Case 1
                        'Add above
                        sbld.AppendLine(txtFMPResult.Text.Trim)
                        sbld.Append(txtAssembly.Text)
                        txtAssembly.Text = sbld.ToString
                    Case 2
                        'Add below
                        sbld.AppendLine(txtAssembly.Text)
                        sbld.AppendLine(txtFMPResult.Text.Trim)
                        txtAssembly.Text = sbld.ToString
                End Select
                If ClearOnClipboardCopy() = True Then
                    txtFMPURL.Text = ""
                    txtFMPPaste.Text = ""
                    txtFMPResult.Text = ""
                End If
            End If
            frmCS.Close()
        Else
            MessageBox.Show("Nothing to copy!", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub GedcomOKClicked(sender As Object, e As EventArgs) Handles cmdGedcomOK.Click
        Dim strText() As String = Nothing
        Dim classGTF As New GedcomTextFormatter
        Dim result As Windows.Forms.DialogResult = Windows.Forms.DialogResult.OK
        If txtGedcomPaste.Text.Trim > "" Then
            If txtTargetWTRef.Text.Trim = "" Then
                result = MessageBox.Show("The main WikiTree ID is blank. Whilst the GEDCOM bio can still be unravelled it may not be possible to identify the name and thus no suggested research links can be created. Proceed?", AppTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
            End If
            If result = Windows.Forms.DialogResult.OK Then
                strText = txtGedcomPaste.Lines
                If txtTargetWTRef.Text.Trim > "" Then
                    If txtTargetWTRef.Text.Contains("-") = True Then
                        classGTF.TargetWTreeRef = txtTargetWTRef.Text
                    Else
                        classGTF.TargetName = txtTargetWTRef.Text
                    End If
                End If
                If txtFatherWTRef.Text.Trim > "" Then
                    If txtFatherWTRef.Text.Contains("-") = True Then
                        classGTF.FatherWTreeRef = txtFatherWTRef.Text
                    Else
                        classGTF.FatherName = txtFatherWTRef.Text
                    End If
                End If
                If txtMotherWTRef.Text.Trim > "" Then
                    If txtMotherWTRef.Text.Contains("-") = True Then
                        classGTF.MotherWTreeRef = txtMotherWTRef.Text
                    Else
                        classGTF.MotherName = txtMotherWTRef.Text
                    End If
                End If
                classGTF.GedcomGeneratedText = strText
                classGTF.ParseSources()
                txtGedcomResult.Text = classGTF.BioText
            End If
        Else
            MessageBox.Show("No text - nothing to do!", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub CopyGedcomParsed2Clip(sender As Object, e As EventArgs) Handles cmdCopyGedcom.Click
        Clipboard.SetText(txtGedcomResult.Text)
        If ClearOnClipboardCopy() = True Then
            txtGedcomPaste.Text = ""
            txtGedcomResult.Text = ""
            txtFatherWTRef.Text = ""
            txtMotherWTRef.Text = ""
            txtTargetWTRef.Text = ""
        End If
        MessageBox.Show("The formatted gedcom text has been added to the clipboard.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub CopyGedcomParsed2Assembly(sender As Object, e As EventArgs) Handles cmdCopyGedToAssembly.Click
        If txtFSTResult.Text > "" Then
            txtAssembly.Text = txtFMPResult.Text
        Else
            MessageBox.Show("Nothing to copy!", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Private Sub CopyAssemblyToClipboard(sender As Object, e As EventArgs) Handles cmdCopyAssyToClip.Click
        Clipboard.SetText(txtAssembly.Text)
        If ClearOnClipboardCopy() = True Then
            txtAssembly.Text = ""
        End If
        MessageBox.Show("The assembled text has been added to the clipboard.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub tsbtnDonate_Click(sender As Object, e As EventArgs) Handles tsbtnDonate.Click
        Dim frmAD As New AboutDonations
        frmAD.ShowDialog()
    End Sub
    Private Sub BracketDatesValueChanged(sender As Object, e As EventArgs) Handles chkBracketDates.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(BRACKET_NAME_TYPES, chkBracketDates.Checked)
        End If
    End Sub
    Private Sub CompactFormatValueChanged(sender As Object, e As EventArgs) Handles chkUseCompactFormat.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(FS_BLOCK_COMPACT, chkUseCompactFormat.Checked)
        End If
    End Sub
    Private Sub FullDatesFormatChanged(sender As Object, e As EventArgs) Handles optAsianDates.CheckedChanged, optEuroDates.CheckedChanged, optUSDates.CheckedChanged
        Dim opt As New RadioButton
        Dim intValue As Integer = 0
        If m_bLoading = False Then
            opt = TryCast(sender, RadioButton)
            Select Case opt.Name
                Case optAsianDates.Name
                    SetIntegerValue(DATES_FORMAT, 2)
                Case optEuroDates.Name
                    SetIntegerValue(DATES_FORMAT, 0)
                Case optUSDates.Name
                    SetIntegerValue(DATES_FORMAT, 1)
            End Select
        End If
    End Sub
    Private Sub TimelineStyleChanged(sender As Object, e As EventArgs) Handles optTLDatedSections.CheckedChanged, _
        optTLDatesBold.CheckedChanged, optTLNarrativeStyle.CheckedChanged, optTLSections.CheckedChanged

        Dim opt As New RadioButton
        Dim intValue As Integer = 0
        If m_bLoading = False Then
            opt = TryCast(sender, RadioButton)
            If opt.Name = optTLNarrativeStyle.Name Then
                SetIntegerValue(TIMELINE_STYLE, 0)
            ElseIf opt.Name = optTLDatesBold.Name Then
                SetIntegerValue(TIMELINE_STYLE, 1)
            ElseIf opt.Name = optTLDatedSections.Name Then
                SetIntegerValue(TIMELINE_STYLE, 2)
            Else
                SetIntegerValue(TIMELINE_STYLE, 3)
            End If
        End If
    End Sub
    Private Sub ThresholdCountChanged(sender As Object, e As EventArgs) Handles nudThreshold.ValueChanged
        SetDecimalValue(ADD_THRESHOLD, nudThreshold.Value)
    End Sub
    Private Sub chkClearOnClipboard_CheckedChanged(sender As Object, e As EventArgs) Handles chkClearOnClipboard.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(CLEAR_ON_CLIPBOARD, chkClearOnClipboard.Checked)
        End If
    End Sub
    Private Sub PadWithZeroChanged(sender As Object, e As EventArgs) Handles chkPad0.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(PAD_SINGLE_DIGITS, chkPad0.Checked)
        End If
    End Sub
    Private Sub UseOrdinalIndicatorsChanged(sender As Object, e As EventArgs) Handles chkUseOrdinalIndicators.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(ORDINAL_INDICATORS, chkUseOrdinalIndicators.Checked)
        End If
    End Sub
    Private Sub CreateProjectTemplatesChanged(sender As Object, e As EventArgs) Handles chkCreateTemplates.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(CREATE_TEMPLATES, chkCreateTemplates.Checked)
        End If
    End Sub
    Private Sub UseShortMonthsChanged(sender As Object, e As EventArgs) Handles chkShortMonths.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(SHORT_MONTHS, chkShortMonths.Checked)
        End If
    End Sub
    Private Sub CensusDoubleIndentChanged(sender As Object, e As EventArgs) Handles chkDoubleIndent.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(DOUBLE_INDENT, chkDoubleIndent.Checked)
        End If
    End Sub
    Private Sub RetainInfoChanged(sender As Object, e As EventArgs) Handles chkRetainImportInfo.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(RETAIN_IMPORT_INFO, chkRetainImportInfo.Checked)
        End If
    End Sub
    Private Sub RetainNoMoreInfoChanged(sender As Object, e As EventArgs) Handles chkRetainNoMoreInfo.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(RETAIN_NO_MORE_INFO, chkRetainNoMoreInfo.Checked)
        End If
        chkAddNoMoreInfo.Enabled = chkRetainNoMoreInfo.Checked
    End Sub
    Private Sub AddNoMoreInfoChanged(sender As Object, e As EventArgs) Handles chkAddNoMoreInfo.CheckedChanged
        If m_bLoading = False Then
            SetBooleanFlag(ADD_NO_MORE_INFO, chkAddNoMoreInfo.Checked)
        End If
    End Sub
    Private Sub ClearFields(sender As Object, e As EventArgs) Handles tsbtnClearFields.Click
        Select Case tabCtrlMain.SelectedIndex
            Case 0
                txtFSTPaste.Text = ""
                txtFSTResult.Text = ""
                txtFSTFather.Text = ""
                txtFSTMother.Text = ""
                txtFSTSpouse.Text = ""
            Case 1
                txtFSCPaste.Text = ""
                txtFSCResult.Text = ""
            Case 2
                txtFMPPaste.Text = ""
                txtFMPResult.Text = ""
            Case 3
                txtGedcomPaste.Text = ""
                txtGedcomResult.Text = ""
                txtFatherWTRef.Text = ""
                txtMotherWTRef.Text = ""
                txtTargetWTRef.Text = ""
            Case 5
                'The Assembly area
                txtAssembly.Text = ""
        End Select
    End Sub
#End Region 'menu and command related routines
#Region "HELP COMMANDS"
    Private Sub CompactFormatHelp(sender As Object, e As EventArgs) Handles cmdCompactHelp.Click
        ShowHelp("Compact_and_Open_Formats")
    End Sub
    Private Sub LayoutHelp(sender As Object, e As EventArgs) Handles cmdLayoutHelp.Click
        ShowHelp("Layout_Tab")
    End Sub
    Private Sub MainHelpClick(sender As Object, e As EventArgs) Handles tsbtnHelp.Click
        ShowHelp("")
    End Sub
    Private Sub FSTextBlockHelp(sender As Object, e As EventArgs) Handles cmdFSTHelp.Click
        ShowHelp("Family_Search_Text_Blocks")
    End Sub
    Private Sub FSCensusHelp(sender As Object, e As EventArgs) Handles cmdFSCHelp.Click
        ShowHelp("Family_Search_Census_Blocks")
    End Sub
    Private Sub FMPCensusHelp(sender As Object, e As EventArgs) Handles cmdFMPCHelp.Click
        ShowHelp("Find_My_Past_Census_Blocks")
    End Sub
    Private Sub GedcomHelp(sender As Object, e As EventArgs) Handles cmdGedcomHelp.Click
        ShowHelp("Gedcom_Formatter")
    End Sub
    Private Sub cmdBaptOr_Click(sender As Object, e As EventArgs) Handles cmdBaptOr.Click
        Dim msgtxt As StringBuilder = New StringBuilder("Baptism is a Greek word. Prior to Christianity, baptism was the ritual use of water for purification. Christian baptism is defined as a sacrament marked by the ritual use of water and admitting the recipient into the Christian community. This is the traditional term used and is an official sacrament of the Catholic Church. Baptism practices vary between churches, however it almost always involves the Trinitarian invocation ('I baptize you in the name of the father, the son, and the holy spirit'). In some cases recipients are fully submerged in water, and in other cases it may be poured or sprinkled over the head. The earliest non-biblical forms of baptism were referred to in the Didache around 100 AD. This reference speaks to the baptism of adults rather than children. Around the same time we have references from others about infant baptism being customary. From the 3rd century, onward, groups of Christians baptized infants as standard practice (although some families preferred to wait until the child was older).")
        msgtxt.AppendLine(vbCrLf)
        msgtxt.AppendLine("Christening: Introduced in the 14th century, Christening is the ceremony of baptizing and naming a child. It comes from English culture and isn’t properly defined in the modern day. Many dictionaries will refer to 'baptism'.")
        msgtxt.Append(vbCrLf & "[Source: http://christeningcards.net/Files/BrokerBranding/christeningCards/Include/difference.html]")
        MessageBox.Show(msgtxt.tostring, "Baptism Or Christening", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region
#Region "PRIVATE ROUTINES"
    Private Function CreateSourceString(ByVal SourceText As String, ByVal LinkText As String, ByVal TagValue As String, Optional ByVal Baptism As Boolean = False) As String
        Dim sbld As StringBuilder = New StringBuilder("")
        If TagValue = "LDS" Then
            'txtBirthSource1.Text.Contains("//familysearch.org")
        ElseIf TagValue = "FMP" Then
            'txtBirthSource1.Text.Contains("/search.findmypast.")
        ElseIf TagValue = "BMD" Then
            'txtBirthSource1.Text.Contains("freeBMD")
        ElseIf TagValue = "PEER" Then
            'txtBirthSource1.Text.Contains(".thepeerage.com")
        Else
            'txtBirthSource1.Tag = "OTHER"
            'Another type of citation
            sbld.Append("<ref>" & SourceText)
            If LinkText.Trim > "" Then
                sbld.Append(" [" & LinkText & " Source]")
            End If
            sbld.Append("</ref>")
        End If
        Return sbld.ToString
    End Function
#End Region 'private routines
#Region "STARTUP AND SHUTDOWN ROUTINES"
    Private Sub WBFMain_Load(sender As Object, e As EventArgs) Handles Me.Load
#If Debug = True Then
        DoDeveloperMenus()
#End If
        tmrLoad.Enabled = True
    End Sub

    Private Sub tsbtnQuit_Click(sender As Object, e As EventArgs) Handles tsbtnQuit.Click
        Me.Close()
    End Sub

    Private Sub WBFMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            If MessageBox.Show("Do you wish to close the Text Formatter?", "Close?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub LoadTimerFired(sender As Object, e As EventArgs) Handles tmrLoad.Tick
        Dim di As New DirectoryInfo(MessagesFolder)
        tmrLoad.Enabled = False
        LoadSettings()
        'Now we have the settings the layout tab must be sorted
        If BaptismText() = 0 Then
            optBaptTextBapt.Checked = True
        Else
            optBaptTextChristen.Checked = True
        End If
        chkBracketDates.Checked = BracketTypeNames()
        chkUseCompactFormat.Checked = CompactFSBlocks()
        chkClearOnClipboard.Checked = ClearOnClipboardCopy()
        chkUseOrdinalIndicators.Checked = OrdinalIndicators()
        chkPad0.Checked = PadSingleDigits()
        chkCreateTemplates.Checked = CreateTemplates()
        chkShortMonths.Checked = ShortMonths()
        chkDoubleIndent.Checked = CensusDoubleIndent()
        chkRetainImportInfo.Checked = RetainImportInfo()
        chkRetainNoMoreInfo.Checked = RetainNoMoreInfo()
        chkAddNoMoreInfo.Checked = AddNoMoreInfo()
        nudThreshold.Value = AddThresholdCount()
        Select Case TimelineStyle()
            Case 0
                optTLNarrativeStyle.Checked = True
            Case 1
                optTLDatesBold.Checked = True
            Case 2
                optTLDatedSections.Checked = True
            Case 3
                optTLSections.Checked = True
        End Select
        Select Case FullDatesFormat()
            Case 0
                optEuroDates.Checked = True
            Case 1
                optUSDates.Checked = True
            Case 2
                optAsianDates.Checked = True
        End Select
        m_bLoading = False
    End Sub
#End Region 'startup and shutdown routines
#If Debug = True Then
#Region "DEVELOPER AREA"
    Private Sub DoDeveloperMenus()
        Dim tsddbDev As New ToolStripDropDownButton("DEV", My.Resources.admin_24)
        Dim mnuItem As ToolStripMenuItem
        Dim newSep As New ToolStripSeparator

        'A separator is added to make it look cleaner
        tstrpMain.Items.Add(newSep)
        tstrpMain.Items.Add(tsddbDev)
        '===========================================
        mnuItem = New ToolStripMenuItem
        mnuItem.Text = "Check Hash Code Generator"
        mnuItem.Name = "tsmnuCheckHash"
        AddHandler mnuItem.Click, AddressOf CheckHash
        tsddbDev.DropDownItems.Add(mnuItem)

    End Sub

    Private Sub CheckHash(sender As Object, e As EventArgs)
        'Gwatkin-366 for which the MD5 hash is 573ab4b13537590f5413618148bf180b
        Dim strMD5Hash As String = GetHash("Meredith-1182")
        MessageBox.Show("The MD5 hash code for Meredith-1182 is:" & vbCrLf & vbCrLf & strMD5Hash.ToLower, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region
#End If
End Class

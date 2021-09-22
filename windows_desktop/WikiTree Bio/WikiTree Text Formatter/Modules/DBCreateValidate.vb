Imports System.Text, System.Data.SqlServerCe
Module DBCreateValidate
    Dim m_strServerLocation As String = ""
    Dim m_strServerName As String = "WTBioF.sdf"
    Dim m_strConnection As String = ""
    Dim strBLD As StringBuilder = New StringBuilder("")
    Dim m_bSuccess As Boolean = True
    Dim m_bValidated As Boolean = False

    Friend Function ValidateStore() As Boolean
        Dim bOK As Boolean = True
        Try
            m_strServerLocation = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WTBioFormatter"
            If My.Computer.FileSystem.DirectoryExists(m_strServerLocation) = False Then
                My.Computer.FileSystem.CreateDirectory(m_strServerLocation)
            End If
            If My.Computer.FileSystem.FileExists(m_strServerLocation & "\" & m_strServerName) = False Then
                bOK = CreateDatabase()
            Else
                m_strConnection = "Data Source=" & m_strServerLocation & "\" & m_strServerName & ";Encrypt Database=False;Max Database Size=4091;Persist Security Info=False;Max Buffer Size=1024;Password='';"
            End If
            m_bValidated = True
        Catch ex As Exception
            bOK = False
            m_bValidated = False
        End Try
        Return bOK
    End Function

    Private Function CreateDatabase() As Boolean
        Dim bSuccess As Boolean = True
        Dim intReturnValue As Integer = 0
        Dim bCreated As Boolean = True
        Dim listTableCreates As New List(Of String)
        Dim command As String = ""
        Dim cmd As New SqlCeCommand()

        Try
            'The connection will not have been initialised
            m_strConnection = "Data Source=" & m_strServerLocation & "\" & m_strServerName & ";Encrypt Database=False;Max Database Size=4091;Persist Security Info=False;Max Buffer Size=1024;Password='';"
            Try
                Dim sqlCeEngine As New SqlCeEngine(m_strConnection)
                sqlCeEngine.CreateDatabase()
            Catch ex As Exception
                bCreated = False
            End Try
            If bCreated = True Then
                '------------------------------Set up the information for the profile index table------------------------------------
                strBLD.Clear()
                strBLD.Append("CREATE TABLE [profileIndex] (")
                strBLD.Append("[Id] int IDENTITY (1,1) NOT NULL,")
                strBLD.Append("[profileID] nvarchar(50) NOT NULL CONSTRAINT DF_profileIndex_profileID  DEFAULT (''),")
                strBLD.Append("[person] nvarchar(200) NOT NULL CONSTRAINT DF_profileIndex_person  DEFAULT(''),")
                strBLD.Append("[storedCopy] bit NOT NULL CONSTRAINT DF_profileIndex_storedCopy  DEFAULT((0)),")
                strBLD.Append("CONSTRAINT PK_profileIndex PRIMARY KEY ([Id]))")
                listTableCreates.Add(strBLD.ToString)
                '------------------------------Set up the information for the storage table------------------------------------
                'strBLD.Clear()
                'strBLD.Append("CREATE TABLE [profiles] (")
                'strBLD.Append("[Id] int IDENTITY (1,1) NOT NULL,")
                'strBLD.Append("[songID] int NOT NULL,")
                'strBLD.Append("[linesText] ntext NOT NULL CONSTRAINT DF_profiles_linesText  DEFAULT(''),")
                'strBLD.Append("CONSTRAINT PK_songLines PRIMARY KEY ([Id]))")
                'listTableCreates.Add(strBLD.ToString)
                '-----------------------------Set up the settings table ----------------------------------------------------------
                strBLD.Clear()
                strBLD.Append("CREATE TABLE [settings] (")
                strBLD.Append("[settingName] nvarchar(60) NOT NULL,")
                strBLD.Append("[settingValue] nvarchar(100) NOT NULL,")
                strBLD.Append("[settingType] int NOT NULL CONSTRAINT DF_settings_settingType  DEFAULT ((5)),")
                strBLD.Append("CONSTRAINT PK_settings PRIMARY KEY ([settingName]))")
                listTableCreates.Add(strBLD.ToString)

                Using connection As New SqlCeConnection(m_strConnection)
                    connection.Open()
                    Try
                        cmd.Connection = connection
                        For Each command In listTableCreates
                            cmd.CommandText = command
                            cmd.ExecuteNonQuery()
                        Next
                        MessageBox.Show("Completed the database creation.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex1 As Exception
                        MessageBox.Show("An error occurred creating the tables . [" & ex1.Message & "]" & vbCrLf & vbCrLf & "The command causing the error was: " & command, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        bCreated = False
                    End Try
                    strBLD.Clear()
                    Try
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'GetBiography',N'1',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the SaveProfileID value." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'SaveProfileID',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the SaveProfileID value." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'WTLogonName',N'',5)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the WTLogonName value = ''." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'WTPassword',N'',5)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the WTPassword value = ''." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'FSBlockCompact',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Compact Block Formatting - defaults to Open." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'LayoutStyle',N'0',1)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Layout Style - Defaults to Timeline." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'IncludeBioSection',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Include Bio setting - Defaults to No Bio section." & vbCrLf)
                        End If
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'CreateEmptyPlaceholders',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Create Empty Placeholders setting - Defaults to No empty placeholders." & vbCrLf)
                        End If
                        'TimelineStyle
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'TimelineStyle',N'2',1)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Timeline Style setting - Defaults to Dated sections." & vbCrLf)
                        End If
                        'BracketNameTypes
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'BracketNameTypes',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Bracket Name Types setting - Defaults to No brackets." & vbCrLf)
                        End If
                        'BracketNameTypes
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'FullDatesFormat',N'0',1)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Full Dates Format setting - Defaults to European." & vbCrLf)
                        End If
                        'PadSingleWithZeros
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'PadSingleWithZeros',N'1',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Pad Zeros Format setting - Defaults to True (Single numeric days or months are padded)." & vbCrLf)
                        End If
                        'OrdinalIndicators
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'OrdinalIndicators',N'1',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Ordinal Indicators setting - Defaults to True (st, nd, rd, th are used)." & vbCrLf)
                        End If
                        'CreateTemplates
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'CreateTemplates',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Create Templates setting - Defaults to False." & vbCrLf)
                        End If
                        'ShortMonths
                        cmd.CommandText = "INSERT INTO [settings] ([settingName],[settingValue],[settingType]) VALUES (N'ShortMonths',N'0',3)"
                        If cmd.ExecuteNonQuery() <> 1 Then
                            strBLD.Append("An error occurred creating the Short Months setting - Defaults to False." & vbCrLf)
                        End If
                    Catch ex As Exception
                        MessageBox.Show("A non-fatal error occurred creating the default settings. [" & ex.Message & "]" & vbCrLf & vbCrLf & "The built in defaults will be used.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred writing the tables for the first time . [" & ex.Message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            bCreated = False
        End Try
        Return bSuccess
    End Function

    Friend Function DatabaseConnection() As String
        Dim strRTN As String = ""
        If m_bValidated = False Then
            If ValidateStore() = False Then
                m_strConnection = ""
            End If
        End If
        Return m_strConnection
    End Function

    Friend Function DatabaseFile() As String
        Dim strRTN As String = ""
        If m_bValidated = False Then
            If ValidateStore() = True Then
                strRTN = m_strServerLocation & "\" & m_strServerName
            End If
        Else
            strRTN = m_strServerLocation & "\" & m_strServerName
        End If
        Return strRTN
    End Function

#Region "Non Create Routines"
    Friend Function BackupDatabase() As Boolean
        Dim bSuccess As Boolean = True

        Try
            strBLD.Clear()
            strBLD.Append(DatabaseFile)
            strBLD.Replace(".sdf", "_Backup.sdf")
            If My.Computer.FileSystem.FileExists(strBLD.ToString) = True Then
                My.Computer.FileSystem.DeleteFile(strBLD.ToString)
            End If
            My.Computer.FileSystem.CopyFile(DatabaseFile, strBLD.ToString)
        Catch ex As Exception
            bSuccess = False
        End Try
        Return bSuccess
    End Function
#End Region
End Module

Public Class outSections
    Private secPreBio = New outSection
    Private secAcknowledge = New outSection
    Private secBiography = New outSection
    Private secBaptism = New outSection
    Private secBirth = New outSection
    Private secBurial = New outSection
    Private secDeath = New outSection
    Private secDivorce = New outSection
    Private classEvents As New eventsItems
    Private classRes As New residences
    Private classMarriage As New marriages
    Private secNotes As New outSection
    Private secSources As New outSection

    Public Function PreBio() As outSection
        Return secPreBio
    End Function
    Public Function Acknowledgements() As outSection
        Return secAcknowledge
    End Function
    Public Function BiographyLines() As outSection
        Return secBiography
    End Function
    Public Function BaptismData() As outSection
        Return secBaptism
    End Function
    Public Function BirthData() As outSection
        Return secBirth
    End Function
    Public Function BurialData() As outSection
        Return secBurial
    End Function
    Public Function DeathData() As outSection
        Return secDeath
    End Function
    Public Function DivorceData() As outSection
        Return secDivorce
    End Function
    Public Function EventsData() As eventsItems
        Return classEvents
    End Function
    Public Function MarriageData() As marriages
        Return classMarriage
    End Function
    Public Function NotesData() As outSection
        Return secNotes
    End Function
    Public Function ResidenceData() As residences
        Return classRes
    End Function
    Public Function SourcesData() As outSection
        Return secSources
    End Function
End Class

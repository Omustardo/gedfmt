Option Strict On

Imports System.Collections
Imports System.Collections.Generic
Imports System.IO        ' for Path

Module Example
Public Sub Main(args As String())
    If UBound(args) - LBound(args) + 1 = 0 Then
        Console.WriteLine("Missing infile parameter")
        Return
    END If
    Dim infileName = args(0)

    Dim lines = File.ReadAllLines(infileName)
    'Console.WriteLine("Full Input:")
    'Console.WriteLine(String.Join(Environment.NewLine, lines))
    'Console.WriteLine("")

    Dim classGTF As New GedcomTextFormatter
    classGTF.GedcomGeneratedText = lines
    classGTF.ParseSources()
    'Console.WriteLine("Full Output:")
    'Console.WriteLine(classGTF.BioText)

    Dim ext = Path.GetExtension(infileName).ToString()
    Dim name = Path.GetFileNameWithoutExtension(infileName).ToString()
    Dim outStream As New IO.FileStream(name + "_fmt" + ext, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
    Dim writer  As New IO.StreamWriter(outStream)
    writer.WriteLine(classGTF.BioText)
    writer.Close()
    End Sub
End Module

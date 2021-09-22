Public Class ListsClasses
    Public Class TI
        Private intItemData As Integer = 0
        Private strTextString As String = ""

        Public Sub New(ByVal TextString As String, ByVal ItemData As Integer)
            intItemData = ItemData
            strTextString = TextString
        End Sub

        Public Property ItemData() As Integer
            Get
                Return intItemData
            End Get
            Set(ByVal value As Integer)
                intItemData = value
            End Set
        End Property

        Public Property TextEntry() As String
            Get
                Return strTextString
            End Get

            Set(ByVal sValue As String)
                strTextString = sValue
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Required. Returns the string for the display member</remarks>
        Public Overrides Function ToString() As String
            Return strTextString
        End Function
    End Class
    Public Class TD
        Private ndecItemData As Decimal = 0
        Private strTextString As String = ""

        Public Sub New(ByVal TextString As String, ByVal ItemData As Decimal)
            ndecItemData = ItemData
            strTextString = TextString
        End Sub

        Public Property ItemData() As Decimal
            Get
                Return ndecItemData
            End Get
            Set(ByVal value As Decimal)
                ndecItemData = value
            End Set
        End Property

        Public Property TextEntry() As String
            Get
                Return strTextString
            End Get

            Set(ByVal sValue As String)
                strTextString = sValue
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Required. Returns the string for the display member</remarks>
        Public Overrides Function ToString() As String
            Return strTextString
        End Function
    End Class
    Public Class TI2
        Private intItemData As Integer = 0
        Private intOtherItemData As Integer = 0
        Private strTextString As String = ""

        Public Sub New(ByVal TextString As String, ByVal ItemData1 As Integer, ByVal ItemData2 As Integer)
            intItemData = ItemData1
            intOtherItemData = ItemData2
            strTextString = TextString
        End Sub

        Public Property ItemData1() As Integer
            Get
                Return intItemData
            End Get
            Set(ByVal value As Integer)
                intItemData = value
            End Set
        End Property

        Public Property ItemData2() As Integer
            Get
                Return intOtherItemData
            End Get
            Set(ByVal value As Integer)
                intOtherItemData = value
            End Set
        End Property

        Public Property TextEntry() As String
            Get
                Return strTextString
            End Get

            Set(ByVal sValue As String)
                strTextString = sValue
            End Set
        End Property

        ''' <summary>
        ''' Returns the string for the display member
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Required.</remarks>
        Public Overrides Function ToString() As String
            Return strTextString
        End Function
    End Class
    Public Class T2
        Private str1 As String = ""
        Private str2 As String = ""
        Public Sub New()
            str1 = ""
            str2 = ""
        End Sub
        Public Sub New(ByVal FirstString As String, ByVal SecondString As String)
            str1 = FirstString
            str2 = SecondString
        End Sub
        Public Property FirstValue() As String
            Get
                Return str1
            End Get

            Set(ByVal sValue As String)
                str1 = sValue
            End Set
        End Property
        Public Property SecondValue() As String
            Get
                Return str2
            End Get

            Set(ByVal sValue As String)
                str2 = sValue
            End Set
        End Property
        ''' <summary>
        ''' Returns the string for the display member
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Required.</remarks>
        Public Overrides Function ToString() As String
            Return str1
        End Function
    End Class
End Class

Public Class Errors
    Private m_intErrorCode As Integer = 0
    Private m_strMessage As String = ""
    Private m_strRoutine As String = ""
    Private m_strSource As String = ""
    Private m_strStackTrace As String = ""
    Private m_intLineNumber As Integer = 0

    Public Property ErrorCode() As Integer
        Get
            Return m_intErrorCode
        End Get
        Set(ByVal value As Integer)
            m_intErrorCode = value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return m_strMessage
        End Get
        Set(ByVal value As String)
            m_strMessage = value
        End Set
    End Property

    Public Property Routine() As String
        Get
            Return m_strRoutine
        End Get
        Set(ByVal value As String)
            m_strRoutine = value
        End Set
    End Property

    Public Property Source() As String
        Get
            Return m_strSource
        End Get
        Set(ByVal value As String)
            m_strSource = value
        End Set
    End Property

#If DEBUG Then
    Public ReadOnly Property StackTrace() As String
        Get
            Return m_strStackTrace
        End Get
    End Property

    Public ReadOnly Property LineNumber() As Integer
        Get
            Return m_intLineNumber
        End Get
    End Property

    Public Sub New(ByVal errCode As Integer, ByVal errMessage As String, ByVal errRoutine As String)
        m_intErrorCode = errCode
        m_strMessage = errMessage
        m_strRoutine = errRoutine
    End Sub

    Public Sub New(ByVal errCode As Integer, exceptionObject As Exception, ByVal errRoutine As String)
        Dim hr As Integer = Runtime.InteropServices.Marshal.GetHRForException(exceptionObject)
        m_intErrorCode = errCode
        m_strMessage = exceptionObject.GetType.ToString & "(0x" & hr.ToString("X8") & "): " & exceptionObject.Message
        m_strRoutine = errRoutine
        m_strStackTrace = exceptionObject.StackTrace
        m_strSource = exceptionObject.Source
        Dim st As StackTrace = New StackTrace(exceptionObject, True)
        For Each sf As StackFrame In st.GetFrames
            If sf.GetFileLineNumber() > 0 Then
                m_intLineNumber = sf.GetFileLineNumber()
            End If
        Next
    End Sub
#Else
    Public Sub New(ByVal errCode As Integer, ByVal errMessage As String, ByVal errRoutine As String)
        m_intErrorCode = errCode
        m_strMessage = errMessage
        m_strRoutine = errRoutine
    End Sub
#End If

End Class

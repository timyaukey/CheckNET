Option Strict On
Option Explicit On

Imports System.IO

Public Class FileReader
    Private mobjLoadFile As TextReader

    Public Sub OpenInputFile(ByVal strFileName As String)
        mobjLoadFile = New StreamReader(strFileName)
    End Sub

    Public Sub CloseInputFile()
        mobjLoadFile.Close()
        mobjLoadFile = Nothing
    End Sub

    Public Function strReadLine() As String
        Return mobjLoadFile.ReadLine()
    End Function
End Class

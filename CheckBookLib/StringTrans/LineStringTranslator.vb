Option Strict On
Option Explicit On

''' <summary>
''' A StringTranslator that loads its content from a text file, 
''' one element per line of that file after skipping the first line.
''' </summary>
''' <typeparam name="TElement"></typeparam>

Public MustInherit Class LineStringTranslator(Of TElement As StringTransElement)
    Inherits StringTranslator(Of TElement)

    Public Overrides Sub LoadFile(ByVal strPath As String)
        Try
            Dim strLine As String
            Init()
            Using objFile = New IO.StreamReader(strPath)
                'Skip first line.
                'First line can contain anything, like a comment indicating the
                'next available key value.
                objFile.ReadLine()
                Do
                    strLine = objFile.ReadLine()
                    If strLine Is Nothing Then
                        Exit Do
                    End If
                    Dim elm As TElement = ParseLine(strLine)
                    Add(elm)
                Loop
            End Using

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Protected MustOverride Function ParseLine(ByVal strLine As String) As TElement

End Class

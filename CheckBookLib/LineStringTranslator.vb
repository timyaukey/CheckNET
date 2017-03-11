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

        Dim intFile As Integer
        Dim strLine As String

        Try

            Init()
            intFile = FreeFile()
            FileOpen(intFile, strPath, OpenMode.Input)

            'Skip first line.
            'First line can contain anything, like a comment indicating the
            'next available key value.
            strLine = LineInput(intFile)
            While Not EOF(intFile)
                strLine = LineInput(intFile)
                Dim elm As TElement = ParseLine(strLine)
                Add(elm)
            End While

            FileClose(intFile)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Protected MustOverride Function ParseLine(ByVal strLine As String) As TElement

End Class

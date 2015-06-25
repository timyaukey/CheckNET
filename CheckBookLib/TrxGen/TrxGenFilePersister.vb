Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

Public Class TrxGenFilePersister
    Implements IFilePersister

    Public Function Load(strFile As String) As IFilePersistable Implements IFilePersister.Load
        Dim objSerializer As XmlSerializer
        Using file As New StreamReader(strFile)
            Dim strFirstLine As String
            Dim classIndex As Integer
            Dim strClassName As String
            Dim secondQuoteIndex As Integer
            strFirstLine = file.ReadLine()
            If strFirstLine = Nothing Then
                Throw New InvalidDataException(strFile + " is not a valid XML file")
            End If
            classIndex = strFirstLine.IndexOf("class=""")
            If classIndex <= 0 Then
                Throw New InvalidDataException("No ""class"" attribute in " + strFile)
            End If
            secondQuoteIndex = strFirstLine.IndexOf("""", classIndex + 7)
            If secondQuoteIndex <= 0 Then
                Throw New InvalidDataException("Missing second quote in ""class"" attribute value in " + strFile)
            End If
            strClassName = strFirstLine.Substring(classIndex + 7, secondQuoteIndex - classIndex - 7)
            If strClassName = "wccheckbook.repeat" Then
                objSerializer = New XmlSerializer(GetType(TGEGeneratorRepeat))
            ElseIf strClassName = "wccheckbook.interpolate" Then
                objSerializer = New XmlSerializer(GetType(TGEGeneratorInterpolate))
            Else
                Throw New InvalidDataException("Unrecognized class name """ + strClassName + """ in " + strFile)
            End If
        End Using
        Using inputStream As New FileStream(strFile, FileMode.Open)
            Dim objData As Object
            objData = objSerializer.Deserialize(inputStream)
            Load = DirectCast(objData, IFilePersistable)
        End Using
    End Function

    Public Sub Save(content As IFilePersistable, strFile As String) Implements IFilePersister.Save

    End Sub
End Class

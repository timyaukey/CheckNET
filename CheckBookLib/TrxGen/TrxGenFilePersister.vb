Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class TrxGenFilePersister
    Implements IFilePersister

    Private mobjSerializer As XmlSerializer

    Public Const TypeRepeat As String = "wccheckbook.repeat"
    Public Const TypeInterpolate As String = "wccheckbook.interpolate"

    Public Function Load(strFile As String) As IFilePersistable Implements IFilePersister.Load
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
            mobjSerializer = objGetSerializer(strClassName, strFile)
        End Using
        Using inputStream As New FileStream(strFile, FileMode.Open)
            Dim objData As Object
            TGEGeneratorBase.SetGroupReadOnly(True)
            objData = mobjSerializer.Deserialize(inputStream)
            Return DirectCast(objData, IFilePersistable)
        End Using
    End Function

    Public Function GetTypes() As List(Of FilePersistableType) Implements IFilePersister.GetTypes
        Dim result As List(Of FilePersistableType)
        result = New List(Of FilePersistableType)
        result.Add(New FilePersistableType("Repeat", TypeRepeat))
        result.Add(New FilePersistableType("Interpolate", TypeInterpolate))
        Return result
    End Function

    Public Function Create(ByVal strType As String, ByVal strFile As String) As IFilePersistable Implements IFilePersister.Create
        Dim objResult As TGEGeneratorBase
        If strType = TypeRepeat Then
            Dim objPersistable As TGEGeneratorRepeat = New TGEGeneratorRepeat()
            objPersistable.ClassName = TypeRepeat
            objPersistable.Repeat = New TGERepeat
            objResult = objPersistable
        ElseIf strType = TypeInterpolate Then
            Dim objPersistable As TGEGeneratorInterpolate = New TGEGeneratorInterpolate()
            objPersistable.ClassName = TypeInterpolate
            objPersistable.Schedule = New TGESchedule
            objPersistable.Samples = New List(Of TGESample)
            objResult = objPersistable
        Else
            Throw New FilePersisterTypeException("Unrecognized file type """ + strType + """")
        End If
        TGEGeneratorBase.SetGroupReadOnly(True)
        objResult.Enabled = "true"
        objResult.RepeatKey = DateTime.Now.ToString("yyyyMMdd-HHmmss")
        mobjSerializer = objGetSerializer(objResult.ClassName, strFile)
        Return objResult
    End Function

    Public Sub Save(content As IFilePersistable, strFile As String) Implements IFilePersister.Save
        Dim ns As XmlSerializerNamespaces = New XmlSerializerNamespaces()
        Dim strNewFile As String
        Dim objSettings As XmlWriterSettings
        ns.Add("", "")
        strNewFile = strFile
        objSettings = New XmlWriterSettings()
        objSettings.Indent = True
        objSettings.OmitXmlDeclaration = True
        objSettings.ConformanceLevel = ConformanceLevel.Auto
        Using writer As XmlWriter = XmlWriter.Create(strNewFile, objSettings)
            mobjSerializer.Serialize(writer, content, ns)
        End Using
    End Sub

    Public Sub Delete(ByVal strFile As String) Implements IFilePersister.Delete

    End Sub

    Public Sub Rename(ByVal strOldName As String, ByVal strNewName As String) Implements IFilePersister.Rename

    End Sub

    Private Function objGetSerializer(ByVal strClassName As String, ByVal strFile As String) As XmlSerializer
        If strClassName = TypeRepeat Then
            Return New XmlSerializer(GetType(TGEGeneratorRepeat))
        ElseIf strClassName = TypeInterpolate Then
            Return New XmlSerializer(GetType(TGEGeneratorInterpolate))
        Else
            Throw New FilePersisterTypeException("Unrecognized file type """ + strClassName +
                " in file """ + Path.GetFileName(strFile) + """")
        End If
    End Function
End Class

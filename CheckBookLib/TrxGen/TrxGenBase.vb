Option Strict On
Option Explicit On
Imports CheckBookLib

Public MustInherit Class TrxGenBase
    Implements ITrxGenerator

    Public MustOverride ReadOnly Property blnEnabled As Boolean Implements ITrxGenerator.blnEnabled
    Private mintMaxDaysOld As Integer?

    Public ReadOnly Property intMaxDaysOld() As Integer? Implements ITrxGenerator.intMaxDaysOld
        Get
            Return mintMaxDaysOld
        End Get
    End Property

    Public MustOverride ReadOnly Property strDescription As String Implements ITrxGenerator.strDescription
    Public MustOverride ReadOnly Property strRepeatKey As String Implements ITrxGenerator.strRepeatKey
    Public MustOverride Function colCreateTrx(objReg As Register, datRptEndMax As Date) As ICollection(Of TrxToCreate) Implements ITrxGenerator.colCreateTrx
    Public MustOverride Function strLoad(domDoc As VB6XmlDocument, objAccount As Account) As String Implements ITrxGenerator.strLoad

    Protected Function strLoadCore(ByVal domDoc As VB6XmlDocument) As String
        Dim vntAttrib As Object = domDoc.DocumentElement.GetAttribute("maxdaysold")
        Dim intMax As Integer
        If gblnXmlAttributeMissing(vntAttrib) Then
            mintMaxDaysOld = Nothing
            Return ""
        End If
        If Not Integer.TryParse(CStr(vntAttrib), intMax) Then
            Return "Invalid maxdaysold attribute"
        End If
        mintMaxDaysOld = intMax
        Return ""
    End Function
End Class

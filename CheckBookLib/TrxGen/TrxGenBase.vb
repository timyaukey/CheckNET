Option Strict On
Option Explicit On

Public MustInherit Class TrxGenBase
    Implements ITrxGenerator

    Public MustOverride ReadOnly Property IsEnabled As Boolean Implements ITrxGenerator.IsEnabled
    Private mintMaxDaysOld As Integer?

    Public ReadOnly Property MaxDaysOld() As Integer? Implements ITrxGenerator.MaxDaysOld
        Get
            Return mintMaxDaysOld
        End Get
    End Property

    Public MustOverride ReadOnly Property Description As String Implements ITrxGenerator.Description
    Public MustOverride ReadOnly Property RepeatKey As String Implements ITrxGenerator.RepeatKey
    Public MustOverride Function CreateTrx(objReg As Register, datRptEndMax As Date) As ICollection(Of TrxToCreate) Implements ITrxGenerator.CreateTrx
    Public MustOverride Function Load(domDoc As VB6XmlDocument, objAccount As Account) As String Implements ITrxGenerator.Load

    Protected Function LoadCore(ByVal domDoc As VB6XmlDocument) As String
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

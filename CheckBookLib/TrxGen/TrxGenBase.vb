Option Strict On
Option Explicit On
Imports CheckBookLib

Public MustInherit Class TrxGenBase
    Implements ITrxGenerator

    Public MustOverride ReadOnly Property blnEnabled As Boolean Implements ITrxGenerator.blnEnabled

    Public ReadOnly Property intMaxDaysOld() As Integer? Implements ITrxGenerator.intMaxDaysOld
        Get
            Return Nothing
        End Get
    End Property

    Public MustOverride ReadOnly Property strDescription As String Implements ITrxGenerator.strDescription
    Public MustOverride ReadOnly Property strRepeatKey As String Implements ITrxGenerator.strRepeatKey
    Public MustOverride Function colCreateTrx(objReg As Register, datRptEndMax As Date) As ICollection(Of TrxToCreate) Implements ITrxGenerator.colCreateTrx
    Public MustOverride Function strLoad(domDoc As VB6XmlDocument, objAccount As Account) As String Implements ITrxGenerator.strLoad
End Class

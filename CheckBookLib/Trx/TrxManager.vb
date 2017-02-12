Option Strict On
Option Explicit On

''' <summary>
''' A helper class to update or delete a Trx that is already in a Register.
''' Get an instance with Register.objGetTrxManager(), then call TrxManager.UpdateStart(),
''' then modify any properties you want of that Trx, then call TrxManager.UpdateEnd().
''' This handles all the details of managing the Register, budget tracking for normal Trx,
''' logging, firing events, etc.
''' </summary>

Public MustInherit Class TrxManager
    Protected mobjReg As Register
    Protected mlngTrxIndex As Integer
    Public ReadOnly objTrx As Trx
    Protected ReadOnly mobjOriginalLogTrx As Trx
    Protected mblnUpdateStarted As Boolean

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As Trx)
        mobjReg = objReg
        mlngTrxIndex = lngTrxIndex
        If Not objTrx Is mobjReg.objTrx(mlngTrxIndex) Then
            Throw New Exception("Trx passed to TrxManager must be at the specified index of the Register passed")
        End If
        Me.objTrx = objTrx
        mobjOriginalLogTrx = objTrx.objClone(Nothing)
        mblnUpdateStarted = False
    End Sub

    Public MustOverride Sub UpdateStart()

    Public Sub UpdateEnd(ByVal objLogger As ILogChange, ByVal strTitle As String)
        If Not mblnUpdateStarted Then
            Throw New Exception("TrxManager.UpdateStart() not called before UpdateEnd()")
        End If
        mobjReg.UpdateEnd(mlngTrxIndex, objLogger, strTitle, mobjOriginalLogTrx)
    End Sub
End Class

Public Class NormalTrxManager
    Inherits TrxManager

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As Trx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        If objTrx.lngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
            gRaiseError("NormalTrxManager.UpdateStart used for wrong transaction type")
        End If
        mobjReg.ClearFirstAffected()
        objTrx.UnApplyFromBudgets(mobjReg)
        objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

Public Class BudgetTrxManager
    Inherits TrxManager

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As Trx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        If objTrx.lngType <> Trx.TrxType.glngTRXTYP_BUDGET Then
            gRaiseError("BudgetTrxManager.UpdateStart used for wrong transaction type")
        End If
        mobjReg.ClearFirstAffected()
        'objTrx.UnApplyFromBudgets(mobjReg)
        objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

Public Class TransferTrxManager
    Inherits TrxManager

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As Trx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        If objTrx.lngType <> Trx.TrxType.glngTRXTYP_TRANSFER Then
            gRaiseError("TransferTrxManager.UpdateStart used for wrong transaction type")
        End If
        mobjReg.ClearFirstAffected()
        'objTrx.UnApplyFromBudgets(mobjReg)
        objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

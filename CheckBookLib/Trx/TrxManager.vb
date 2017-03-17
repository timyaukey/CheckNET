﻿Option Strict On
Option Explicit On

''' <summary>
''' A helper class to update or delete a Trx that is already in a Register.
''' Get an instance with Register.objGetTrxManager(), then call TrxManager.UpdateStart(),
''' then modify any properties you want of that Trx, then call TrxManager.UpdateEnd().
''' This handles all the details of managing the Register, budget tracking for normal Trx,
''' logging, firing events, etc.
''' NOTE: Once you call UpdateStart() you MUST call UpdateEnd() or the Register object
''' will only be partly updated.
''' </summary>

Public MustInherit Class TrxManager(Of TTrx As Trx)

    Protected mobjReg As Register
    Protected mlngTrxIndex As Integer
    Public ReadOnly objTrx As TTrx
    Protected ReadOnly mobjOriginalLogTrx As TTrx
    Protected mblnUpdateStarted As Boolean

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As TTrx)
        mobjReg = objReg
        mlngTrxIndex = lngTrxIndex
        If Not objTrx Is mobjReg.objTrx(mlngTrxIndex) Then
            Throw New Exception("Trx passed to TrxManager must be at the specified index of the Register passed")
        End If
        Me.objTrx = objTrx
        mobjOriginalLogTrx = DirectCast(objTrx.objClone(Nothing), TTrx)
        mblnUpdateStarted = False
    End Sub

    Public MustOverride Sub UpdateStart()

    Public Sub UpdateEnd(ByVal objLogger As ILogChange, ByVal strTitle As String)
        If Not mblnUpdateStarted Then
            Throw New Exception("TrxManager.UpdateStart() not called before UpdateEnd()")
        End If
        'TO DO: Make sure objTrx is still at index mlngTrxIndex, and if not
        'find the index it is currently stored at and use that instead.
        mobjReg.UpdateEnd(mlngTrxIndex, objLogger, strTitle, mobjOriginalLogTrx)
    End Sub
End Class

Public Class NormalTrxManager
    Inherits TrxManager(Of NormalTrx)

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As NormalTrx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        mobjReg.ClearFirstAffected()
        'These next two lines are why if you call UpdateStart(),
        'you must finish by calling UpdateEnd() to keep the Register in good condition.
        objTrx.UnApplyFromBudgets(mobjReg)
        objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

Public Class BudgetTrxManager
    Inherits TrxManager(Of BudgetTrx)

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As BudgetTrx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        mobjReg.ClearFirstAffected()
        'objTrx.UnApplyFromBudgets(mobjReg)
        objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

Public Class TransferTrxManager
    Inherits TrxManager(Of TransferTrx)

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As TransferTrx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        mobjReg.ClearFirstAffected()
        'objTrx.UnApplyFromBudgets(mobjReg)
        objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

Public Class ReplicaTrxManager
    Inherits TrxManager(Of ReplicaTrx)

    Public Sub New(ByVal objReg As Register, ByVal lngTrxIndex As Integer, ByVal objTrx As ReplicaTrx)
        MyBase.New(objReg, lngTrxIndex, objTrx)
    End Sub

    Public Overrides Sub UpdateStart()
        mobjReg.ClearFirstAffected()
        'objTrx.UnApplyFromBudgets(mobjReg)
        'objTrx.ClearRepeatTrx(mobjReg)
        mblnUpdateStarted = True
    End Sub
End Class

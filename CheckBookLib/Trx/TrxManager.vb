﻿Option Strict On
Option Explicit On

''' <summary>
''' A helper class to update a Trx that is already in a Register.
''' Create an instance of the TrxManager subclass appropriate to the Trx subclass,
''' passing to the constructor a Trx that is already in the Register.
''' Then call TrxManager.UpdateStart(), then modify any properties you want of that Trx,
''' then call TrxManager.UpdateEnd().
''' This handles all the details of managing the Register, budget tracking for normal Trx,
''' logging, firing events, etc.
''' NOTE: Once you call UpdateStart() you MUST call UpdateEnd() or the Register object
''' will only be partly updated.
''' NOTE: You CANNOT construct a new Trx to use with this. It must be the existing
''' Trx in the Register you wish to update.
''' </summary>

Public MustInherit Class TrxManager(Of TTrx As Trx)

    Public ReadOnly objTrx As TTrx
    Protected ReadOnly mobjOriginalLogTrx As TTrx
    Protected mblnUpdateStarted As Boolean

    Public Sub New(ByVal objTrx_ As TTrx)
        If Not objTrx_ Is objTrx_.objReg.objTrx(objTrx_.lngIndex) Then
            Throw New Exception("Trx passed to TrxManager must be at the specified index of the Register passed")
        End If
        objTrx = objTrx_
        mobjOriginalLogTrx = DirectCast(objTrx_.objClone(blnWillAddToRegister:=False), TTrx)
        mblnUpdateStarted = False
    End Sub

    Public Sub Update(ByVal objBuilder As Action(Of TTrx), ByVal objLogger As ILogChange, ByVal strTitle As String)
        UpdateStart()
        objBuilder(objTrx)
        UpdateEnd(objLogger, strTitle)
    End Sub

    Public Sub UpdateStart()
        objTrx.objReg.BeginCriticalOperation()
        objTrx.objReg.ClearFirstAffected()
        'These next two lines are why if you call UpdateStart(),
        'you must finish by calling UpdateEnd() to keep the Register in good condition.
        'I wish we could delay these steps until UpdateEnd(), but ClearRepeatTrx() requires
        'the original Trx object whose contents may have been changed by then.
        objTrx.UnApply()
        objTrx.ClearRepeatTrx()
        mblnUpdateStarted = True
    End Sub

    Public Sub UpdateEnd(ByVal objLogger As ILogChange, ByVal strTitle As String)
        If Not mblnUpdateStarted Then
            Throw New Exception("TrxManager.UpdateStart() not called before UpdateEnd()")
        End If
        objTrx.objReg.UpdateEnd(objTrx, objLogger, strTitle, mobjOriginalLogTrx)
        objTrx.objReg.EndCriticalOperation()
    End Sub
End Class

Public Class NormalTrxManager
    Inherits TrxManager(Of NormalTrx)

    Public Sub New(ByVal objTrx As NormalTrx)
        MyBase.New(objTrx)
    End Sub
End Class

Public Class BudgetTrxManager
    Inherits TrxManager(Of BudgetTrx)

    Public Sub New(ByVal objTrx As BudgetTrx)
        MyBase.New(objTrx)
    End Sub
End Class

Public Class TransferTrxManager
    Inherits TrxManager(Of TransferTrx)

    Public Sub New(ByVal objTrx As TransferTrx)
        MyBase.New(objTrx)
    End Sub
End Class

Public Class ReplicaTrxManager
    Inherits TrxManager(Of ReplicaTrx)

    Public Sub New(ByVal objTrx As ReplicaTrx)
        MyBase.New(objTrx)
    End Sub
End Class

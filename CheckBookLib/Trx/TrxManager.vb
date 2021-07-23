Option Strict On
Option Explicit On

''' <summary>
''' A helper class to update a BaseTrx that is already in a Register.
''' Create an instance of the TrxManager subclass appropriate to the BaseTrx subclass,
''' passing to the constructor a BaseTrx that is already in the Register.
''' Then call TrxManager.UpdateStart(), then modify any properties you want of that BaseTrx,
''' then call TrxManager.UpdateEnd().
''' This handles all the details of managing the Register, budget tracking for normal BaseTrx,
''' logging, firing events, etc.
''' NOTE: Once you call UpdateStart() you MUST call UpdateEnd() or the Register object
''' will only be partly updated.
''' NOTE: You CANNOT construct a new BaseTrx to use with this. It must be the existing
''' BaseTrx in the Register you wish to update.
''' </summary>

Public MustInherit Class TrxManager(Of TTrx As BaseTrx)

    Public ReadOnly Trx As TTrx
    Protected ReadOnly mOriginalLogTrx As TTrx
    Protected mHasUpdateStarted As Boolean

    Public Sub New(ByVal objTrx_ As TTrx)
        If Not objTrx_ Is objTrx_.Register.GetTrx(objTrx_.RegIndex) Then
            Throw New Exception("Trx passed to TrxManager must be at the specified index of the Register passed")
        End If
        Trx = objTrx_
        mOriginalLogTrx = DirectCast(objTrx_.CloneTrx(blnWillAddToRegister:=False), TTrx)
        mHasUpdateStarted = False
    End Sub

    Public Sub Update(ByVal objBuilder As Action(Of TTrx), ByVal objLogger As ILogChange, ByVal strTitle As String)
        UpdateStart()
        objBuilder(Trx)
        UpdateEnd(objLogger, strTitle)
    End Sub

    Public Sub UpdateStart()
        Trx.Register.BeginCriticalOperation()
        Trx.Register.ClearFirstAffected()
        'These next two lines are why if you call UpdateStart(),
        'you must finish by calling UpdateEnd() to keep the Register in good condition.
        'I wish we could delay these steps until UpdateEnd(), but ClearRepeatTrx() requires
        'the original BaseTrx object whose contents may have been changed by then.
        Trx.UnApply()
        Trx.ClearRepeatTrx()
        mHasUpdateStarted = True
    End Sub

    Public Sub UpdateEnd(ByVal objLogger As ILogChange, ByVal strTitle As String)
        If Not mHasUpdateStarted Then
            Throw New Exception("TrxManager.UpdateStart() not called before UpdateEnd()")
        End If
        Trx.Register.UpdateEnd(Trx, objLogger, strTitle, mOriginalLogTrx)
        Trx.Register.EndCriticalOperation()
    End Sub
End Class

Public Class NormalTrxManager
    Inherits TrxManager(Of BankTrx)

    Public Sub New(ByVal objTrx As BankTrx)
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

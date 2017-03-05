Option Strict On
Option Explicit On

Public Class NormalTrx
    Inherits Trx

    Public Overrides ReadOnly Property lngType As TrxType
        Get
            Return TrxType.glngTRXTYP_NORMAL
        End Get
    End Property

    Public Overrides Function objClone(objReg As Register) As Trx
        Dim objNormalTrx As NormalTrx = New NormalTrx()
        objNormalTrx.NewStartNormal(objReg, Me)
        CopySplits(objNormalTrx)
        Return objNormalTrx
    End Function

    Public Overrides Function objGetTrxManager(objReg As Register, lngIndex As Integer) As TrxManager
        Return New NormalTrxManager(objReg, lngIndex, Me)
    End Function

    Public Overrides Sub Validate(objReg As Register, lngIndex As Integer)
        Dim objSplit As TrxSplit
        Dim curTotal As Decimal
        MyBase.Validate(objReg, lngIndex)
        If mcolSplits Is Nothing Then
            objReg.RaiseValidationError(lngIndex, "Missing split collection")
        Else
            curTotal = 0
            For Each objSplit In mcolSplits
                curTotal = curTotal + objSplit.curAmount
                If Not objSplit.objBudget Is Nothing Then
                    If objSplit.objBudget.lngType <> TrxType.glngTRXTYP_BUDGET Then
                        objReg.RaiseValidationError(lngIndex, "Split applied to non-budget trx")
                    End If
                    If objSplit.strBudgetKey = "" Then
                        objReg.RaiseValidationError(lngIndex, "Split applied to budget trx has no budget key")
                    End If
                    If objSplit.strBudgetKey <> objSplit.objBudget.strBudgetKey Then
                        objReg.RaiseValidationError(lngIndex, "Split applied to budget trx has wrong budget key")
                    End If
                End If
            Next objSplit
            If curTotal <> mcurAmount Then
                objReg.RaiseValidationError(lngIndex, "Normal trx splits add up wrong")
            End If
        End If
        If mblnFake Then
            If mstrImportKey <> "" Then
                objReg.RaiseValidationError(lngIndex, "Normal trx cannot have import key if it is fake")
            End If
            If mlngStatus <> TrxStatus.glngTRXSTS_UNREC Then
                objReg.RaiseValidationError(lngIndex, "Normal trx must be unreconciled if it is fake")
            End If
        End If
        If mstrBudgetKey <> "" Then
            objReg.RaiseValidationError(lngIndex, "Normal trx cannot have budget key")
        End If
        If mcurBudgetLimit <> 0 Then
            objReg.RaiseValidationError(lngIndex, "Normal trx cannot have budget limit")
        End If
        If mcurBudgetApplied <> 0 Then
            objReg.RaiseValidationError(lngIndex, "Normal trx cannot have budget applied")
        End If
        If Not mcolAppliedSplits Is Nothing Then
            objReg.RaiseValidationError(lngIndex, "Normal trx cannot have applied splits collection")
        End If
        If mstrTransferKey <> "" Then
            objReg.RaiseValidationError(lngIndex, "Normal trx cannot have transfer key")
        End If
        If mcurTransferAmount <> 0 Then
            objReg.RaiseValidationError(lngIndex, "Normal trx cannot have transfer amount")
        End If
    End Sub
End Class

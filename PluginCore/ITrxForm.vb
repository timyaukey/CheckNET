Option Strict On
Option Explicit On

Public Interface ITrxForm
    Inherits IDisposable

    Function blnAddNormal(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As NormalTrx,
        ByRef datDefaultDate_ As Date, ByVal blnCheckInvoiceNum_ As Boolean,
        ByVal strLogTitle As String) As Boolean

    Function blnAddNormalSilent(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As NormalTrx,
        ByRef datDefaultDate_ As Date, ByVal blnCheckInvoiceNum_ As Boolean,
        ByVal strLogTitle As String) As Boolean

    Function blnAddBudget(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register,
        ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

    Function blnAddTransfer(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register,
        ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

    Function blnUpdate(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As Trx,
        ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean
End Interface

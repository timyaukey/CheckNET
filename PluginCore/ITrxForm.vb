Option Strict On
Option Explicit On

Public Interface ITrxForm
    Inherits IDisposable

    Function AddNormal(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As BankTrx,
        ByRef datDefaultDate_ As Date, ByVal blnCheckInvoiceNum_ As Boolean,
        ByVal strLogTitle As String) As Boolean

    Function AddNormalSilent(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As BankTrx,
        ByRef datDefaultDate_ As Date, ByVal blnCheckInvoiceNum_ As Boolean,
        ByVal strLogTitle As String) As Boolean

    Function AddBudget(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register,
        ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

    Function AddTransfer(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register,
        ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

    Function UpdateTrx(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As BaseTrx,
        ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean
End Interface

Option Strict On
Option Explicit On

Public Interface IHostTrxToolUI
    ReadOnly Property objReg() As Register
    Sub SetNumber(ByVal strNumber As String)
    Sub SetDate(ByVal datDate As DateTime)
    Sub SetFake(ByVal blnFake As Boolean)
    Function objGetTrxCopy() As BankTrx
    Sub SaveAndClose()
End Interface

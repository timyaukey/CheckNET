Option Strict On
Option Explicit On

Public Interface IHostTrxToolUI
    ReadOnly Property Reg() As Register
    Sub SetNumber(ByVal strNumber As String)
    Sub SetDate(ByVal datDate As DateTime)
    Sub SetFake(ByVal blnFake As Boolean)
    Function GetTrxCopy() As BankTrx
    Sub SaveAndClose()
End Interface

Option Strict On
Option Explicit On

Public Interface IRegisterForm
    Inherits IDisposable

    ReadOnly Property objReg() As Register

    Sub ShowMe(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register)

    Sub ShowMeAgain()

End Interface

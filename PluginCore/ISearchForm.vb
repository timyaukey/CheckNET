Option Strict On
Option Explicit On

Public Interface ISearchForm
    Inherits IDisposable

    Sub ShowMe(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register)

    Sub ShowMeAgain()

    Event SearchFormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs)
End Interface

Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Class RegForwardFrom(Of TTrx As BaseTrx)
    Inherits RegIterator(Of TTrx)

    Private mobjStartTrx As BaseTrx

    Public Sub New(ByVal objReg As Register, ByVal objStartTrx As BaseTrx)
        MyBase.New(objReg)
        mobjStartTrx = objStartTrx
    End Sub

    Protected Overrides Function GetFirst() As BaseTrx
        Return mobjStartTrx
    End Function

End Class

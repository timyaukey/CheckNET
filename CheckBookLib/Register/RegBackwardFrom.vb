Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Class RegBackwardFrom(Of TTrx As BaseTrx)
    Inherits RegReverse(Of TTrx)

    Private mobjStartTrx As BaseTrx

    Public Sub New(ByVal objReg As Register, ByVal objStartTrx As BaseTrx)
        MyBase.New(objReg)
        mobjStartTrx = objStartTrx
    End Sub

    Protected Overrides Function objGetLast() As BaseTrx
        Return mobjStartTrx
    End Function

End Class

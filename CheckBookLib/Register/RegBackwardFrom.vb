Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Class RegBackwardFrom(Of TTrx As Trx)
    Inherits RegReverse(Of TTrx)

    Private mobjStartTrx As Trx

    Public Sub New(ByVal objReg As Register, ByVal objStartTrx As Trx)
        MyBase.New(objReg)
        mobjStartTrx = objStartTrx
    End Sub

    Protected Overrides Function objGetLast() As Trx
        Return mobjStartTrx
    End Function

End Class

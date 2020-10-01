Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Class RegForwardFrom(Of TTrx As Trx)
    Inherits RegIterator(Of TTrx)

    Private mobjStartTrx As Trx

    Public Sub New(ByVal objReg As Register, ByVal objStartTrx As Trx)
        MyBase.New(objReg)
        mobjStartTrx = objStartTrx
    End Sub

    Protected Overrides Function objGetFirst() As Trx
        Return mobjStartTrx
    End Function

End Class

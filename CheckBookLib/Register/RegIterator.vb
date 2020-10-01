Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Class RegIterator(Of TTrx As Trx)
    Implements IEnumerable(Of TTrx)

    Protected mobjReg As Register

    Public Sub New(ByVal objReg_ As Register)
        mobjReg = objReg_
    End Sub

    Public Iterator Function GetEnumerator() As IEnumerator(Of TTrx) Implements IEnumerable(Of TTrx).GetEnumerator
        'Using a Trx as the cursor instead of a Register index means
        'we always return the Trx after the last one returned,
        'even if Trx are inserted or deleted earlier in the Register order.
        Dim objCurrentTrx As Trx = objGetFirst()
        Do
            If objCurrentTrx Is Nothing Then
                Return
            End If
            If blnAfterLast(objCurrentTrx) Then
                Return
            End If
            Dim objTypedTrx As TTrx = TryCast(objCurrentTrx, TTrx)
            If Not objTypedTrx Is Nothing Then
                Yield objTypedTrx
            End If
            objCurrentTrx = objCurrentTrx.objNext
        Loop

    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

    Protected Overridable Function objGetFirst() As Trx
        Return mobjReg.objFirstTrx
    End Function

    Protected Overridable Function blnAfterLast(ByVal objTrx As Trx) As Boolean
        Return False
    End Function
End Class

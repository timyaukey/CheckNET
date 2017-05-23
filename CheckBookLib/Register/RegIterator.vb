Option Strict On
Option Explicit On

Public Class RegIterator
    Protected mobjReg As Register

    Public Sub New(ByVal objReg_ As Register)
        mobjReg = objReg_
    End Sub

    Public Iterator Function colTrx() As IEnumerable(Of Trx)
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
            Yield objCurrentTrx
            objCurrentTrx = objCurrentTrx.objNext
        Loop

    End Function

    Protected Overridable Function objGetFirst() As Trx
        If mobjReg.lngTrxCount = 0 Then
            Return Nothing
        Else
            Return mobjReg.objTrx(1)
        End If
    End Function

    Protected Overridable Function blnAfterLast(ByVal objTrx As Trx) As Boolean
        Return False
    End Function
End Class

Option Strict On
Option Explicit On

Public Class RegIterator
    Protected mobjReg As Register

    Public Sub New(ByVal objReg_ As Register)
        mobjReg = objReg_
    End Sub

    Public Iterator Function colTrx() As IEnumerable(Of Trx)
        Dim lngIndex As Integer

        lngIndex = lngGetBeforeFirst()
        Do
            lngIndex = lngIndex + 1
            If lngIndex > mobjReg.lngTrxCount Then
                Return
            End If
            If blnAfterLast(lngIndex) Then
                Return
            End If
            Yield mobjReg.objTrx(lngIndex)
        Loop

    End Function

    Protected Overridable Function lngGetBeforeFirst() As Integer
        Return 0
    End Function

    Protected Overridable Function blnAfterLast(ByVal lngIndex As Integer) As Boolean
        Return False
    End Function
End Class

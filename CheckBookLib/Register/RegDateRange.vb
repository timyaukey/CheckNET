Option Strict On
Option Explicit On

Public Class RegDateRange(Of TTrx As BaseTrx)
    Inherits RegIterator(Of TTrx)

    Private mdatStart As DateTime
    Private mdatEnd As DateTime

    Public Sub New(objReg_ As Register, ByVal datStart As DateTime, ByVal datEnd As DateTime)
        MyBase.New(objReg_)
        mdatStart = datStart
        mdatEnd = datEnd
    End Sub

    Protected Overrides Function GetFirst() As BaseTrx
        Return mobjReg.FirstOnOrAfter(mdatStart)
    End Function

    Protected Overrides Function IsAfterLast(objTrx As BaseTrx) As Boolean
        Return objTrx.datDate > mdatEnd
    End Function
End Class

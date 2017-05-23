Option Strict On
Option Explicit On

Public Class RegDateRange
    Inherits RegIterator

    Private mdatStart As DateTime
    Private mdatEnd As DateTime

    Public Sub New(objReg_ As Register, ByVal datStart As DateTime, ByVal datEnd As DateTime)
        MyBase.New(objReg_)
        mdatStart = datStart
        mdatEnd = datEnd
    End Sub

    Protected Overrides Function objGetFirst() As Trx
        Return mobjReg.objFirstOnOrAfter(mdatStart)
    End Function

    Protected Overrides Function blnAfterLast(objTrx As Trx) As Boolean
        Return objTrx.datDate > mdatEnd
    End Function
End Class

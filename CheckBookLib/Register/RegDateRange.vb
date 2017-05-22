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

    Protected Overrides Function lngGetBeforeFirst() As Integer
        Return mobjReg.lngFindBeforeDate(mdatStart)
    End Function

    Protected Overrides Function blnAfterLast(lngIndex As Integer) As Boolean
        Return mobjReg.objTrx(lngIndex).datDate > mdatEnd
    End Function
End Class

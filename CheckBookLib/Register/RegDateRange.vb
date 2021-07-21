﻿Option Strict On
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

    Protected Overrides Function objGetFirst() As BaseTrx
        Return mobjReg.objFirstOnOrAfter(mdatStart)
    End Function

    Protected Overrides Function blnAfterLast(objTrx As BaseTrx) As Boolean
        Return objTrx.datDate > mdatEnd
    End Function
End Class

Option Strict On
Option Explicit On

Public Class ReportLineItem
    Private mobjParent As LineItemGroup
    Private mstrItemKey As String
    Private mstrItemTitle As String
    Private mcurTotal As Decimal
    Public blnPrinted As Boolean

    Public Sub New(ByVal objParent_ As LineItemGroup, ByVal strItemKey_ As String, ByVal strItemTitle_ As String)
        mobjParent = objParent_
        mstrItemKey = strItemKey_
        mstrItemTitle = strItemTitle_
        blnPrinted = False
    End Sub

    Public Sub Add(ByVal curAmount As Decimal)
        mcurTotal += curAmount
        mobjParent.Add(curAmount)
    End Sub

    Public ReadOnly Property objParent() As LineItemGroup
        Get
            Return mobjParent
        End Get
    End Property

    Public ReadOnly Property strItemKey() As String
        Get
            Return mstrItemKey
        End Get
    End Property

    Public ReadOnly Property strItemTitle() As String
        Get
            Return mstrItemTitle
        End Get
    End Property

    Public ReadOnly Property curTotal() As Decimal
        Get
            Return mcurTotal
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return mstrItemTitle + " " + gstrFormatCurrency(mcurTotal)
    End Function
End Class

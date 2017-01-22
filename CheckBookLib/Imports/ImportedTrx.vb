Option Strict On
Option Explicit On

''' <summary>
''' The ITrxReader interface returns instances of this
''' read from some kind of external source, like a file
''' or the clipboard.
''' </summary>

Public Class ImportedTrx
    Inherits Trx

    'Import match narrowing method
    Private mlngNarrowMethod As ImportMatchNarrowMethod
    'Range of trx amount to match
    Private mcurMatchMin As Decimal
    Private mcurMatchMax As Decimal

    Public Sub New()
        mlngNarrowMethod = ImportMatchNarrowMethod.None
    End Sub

    Public Property lngNarrowMethod() As ImportMatchNarrowMethod
        Get
            lngNarrowMethod = mlngNarrowMethod
        End Get
        Set(value As ImportMatchNarrowMethod)
            mlngNarrowMethod = value
        End Set
    End Property

    Public Property curMatchMin() As Decimal
        Get
            curMatchMin = mcurMatchMin
        End Get
        Set(value As Decimal)
            mcurMatchMin = value
        End Set
    End Property

    Public Property curMatchMax() As Decimal
        Get
            curMatchMax = mcurMatchMax
        End Get
        Set(value As Decimal)
            mcurMatchMax = value
        End Set
    End Property

End Class

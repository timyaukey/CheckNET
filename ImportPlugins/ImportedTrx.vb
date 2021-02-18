Option Strict On
Option Explicit On


''' <summary>
''' The ITrxReader interface returns instances of this
''' read from some kind of external source, like a file
''' or the clipboard.
''' </summary>

Public Class ImportedTrx
    Inherits NormalTrx

    'Import match narrowing method
    Public Property lngNarrowMethod As ImportMatchNarrowMethod
    'Range of trx amount to match
    Public Property curMatchMin As Decimal
    Public Property curMatchMax As Decimal
    Public Property blnAllowAutoBatchNew As Boolean
    Public Property blnAllowAutoBatchUpdate As Boolean

    Public Sub New(ByVal objReg_ As Register)
        MyBase.New(objReg_)
        lngNarrowMethod = ImportMatchNarrowMethod.None
    End Sub

End Class

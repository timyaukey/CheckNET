﻿Option Strict On
Option Explicit On

Public Class FilterNonReal
    Implements ISearchFilter

    Public Function blnInclude(objTrx As Trx) As Boolean Implements ISearchFilter.blnInclude
        Return objTrx.blnFake
    End Function

    Public Overrides Function ToString() As String
        Return "Fake or Generated"
    End Function
End Class

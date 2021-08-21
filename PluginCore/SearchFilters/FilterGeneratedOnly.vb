﻿Option Strict On
Option Explicit On

Public Class FilterGeneratedOnly
    Implements ISearchFilter

    Public Function IsIncluded(objTrx As BaseTrx) As Boolean Implements ISearchFilter.IsIncluded
        Return objTrx.IsAutoGenerated
    End Function

    Public Overrides Function ToString() As String
        Return "Generated Only"
    End Function
End Class

Option Strict On
Option Explicit On

Public Interface ISearchFilter
    Function IsIncluded(ByVal objTrx As BaseTrx) As Boolean
End Interface

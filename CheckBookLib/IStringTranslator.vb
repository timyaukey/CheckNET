Public Interface IStringTranslator
    ReadOnly Property intElements As Integer
    ReadOnly Property strKey(intIndex As Integer) As String
    ReadOnly Property strValue1(intIndex As Integer) As String
    ReadOnly Property strValue2(intIndex As Integer) As String
    Function intLookupKey(strKey_ As String) As Integer
    Function intLookupValue1(strValue1_ As String) As Integer
    Function strKeyToValue1(strKey_ As String) As String
End Interface

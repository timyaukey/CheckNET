Public Interface IStringTranslator
    ReadOnly Property ElementCount As Integer
    ReadOnly Property GetKey(intIndex As Integer) As String
    ReadOnly Property GetValue1(intIndex As Integer) As String
    ReadOnly Property GetValue2(intIndex As Integer) As String
    Function FindIndexOfKey(strKey_ As String) As Integer
    Function FindIndexOfValue1(strValue1_ As String) As Integer
    Function KeyToValue1(strKey_ As String) As String
    Function FormatElement(ByVal objElement As StringTransElement) As String
End Interface

Option Strict On
Option Explicit On

Public MustInherit Class StringTranslator(Of TElement As StringTransElement)
    Implements IStringTranslator

    'Encapsulate a searchable list of TElement objects.

    Private maudtElement() As TElement '1 to mintElements. Zeroeth element is unused.
    Private mintElements As Integer

    'maudtElement() indices, keyed by "#" & Element.strKey
    Private mdictKeyIndices As Dictionary(Of String, Integer)

    'maudtElement() indices, keyed by "#" & Element.strValue1
    Private mdictValue1Indices As Dictionary(Of String, Integer)

    Public Sub New()
        Init()
    End Sub

    Public Sub Init()
        mintElements = 0
        Erase maudtElement
        mdictKeyIndices = New Dictionary(Of String, Integer)()
        mdictValue1Indices = New Dictionary(Of String, Integer)()
    End Sub

    Public MustOverride Sub LoadFile(ByVal strPath As String)

    '$Description Add a key string and its pair of values to the list.
    '   AccountKey and value strings do not have to be in any particular order
    '   in the list.
    '$Param strKey_ The key string. Must be unique in list.
    '$Param strValue1_ The first value string for this key string. Must be unique in list.
    '$Param strValue2_ The second value string for this key string.

    Public Sub Add(ByVal elm As TElement)

        mintElements = mintElements + 1
        ReDim Preserve maudtElement(mintElements)
        maudtElement(mintElements) = elm
        mdictKeyIndices.Add("#" & elm.Key, mintElements)
        mdictValue1Indices.Add("#" & elm.Value1, mintElements)
    End Sub

    '$Description Search the list for the specified key string. Search is case
    '   sensitive.
    '$Param strKey_ The key string to look for.
    '$Returns The index of the specified key string, or zero if it was not found.
    '   This may be passed to the strValue1() or strValue2() properties
    '   to retrieve values for that key string.

    Public Function FindIndexOfKey(ByVal strKey_ As String) As Integer Implements IStringTranslator.FindIndexOfKey
        Dim result As Integer
        If mdictKeyIndices.TryGetValue("#" & strKey_, result) Then
            Return result
        Else
            Return 0
        End If
    End Function

    '$Description Search the list for the specified key string, and return
    '   the strValue1 for that index.
    '$Param strKey_ The key string to look for.
    '$Returns The strValue1 of the matched entry, or an empty string if
    '   the key was not found.

    Public Function KeyToValue1(ByVal strKey_ As String) As String Implements IStringTranslator.KeyToValue1
        Dim idx As Integer
        If mdictKeyIndices.TryGetValue("#" & strKey_, idx) Then
            Return maudtElement(idx).Value1
        Else
            Return ""
        End If
    End Function

    '$Description The number of key strings in the list.

    Public ReadOnly Property ElementCount() As Integer Implements IStringTranslator.ElementCount
        Get
            Return mintElements
        End Get
    End Property

    Public ReadOnly Property GetKey(ByVal intIndex As Integer) As String Implements IStringTranslator.GetKey
        Get
            Return maudtElement(intIndex).Key
        End Get
    End Property

    Public ReadOnly Property GetValue1(ByVal intIndex As Integer) As String Implements IStringTranslator.GetValue1
        Get
            Return maudtElement(intIndex).Value1
        End Get
    End Property

    Public ReadOnly Property GetValue2(ByVal intIndex As Integer) As String Implements IStringTranslator.GetValue2
        Get
            Return maudtElement(intIndex).Value2
        End Get
    End Property

    Public ReadOnly Property GetElement(ByVal intIndex As Integer) As TElement
        Get
            Return maudtElement(intIndex)
        End Get
    End Property

    '$Description Like intLookupKey(), but searches for a strValue1 match.

    Public Function FindIndexOfValue1(ByVal strValue1_ As String) As Integer Implements IStringTranslator.FindIndexOfValue1
        Dim result As Integer
        If mdictValue1Indices.TryGetValue("#" & strValue1_, result) Then
            Return result
        Else
            Return 0
        End If
    End Function

    Public Overridable Function FormatElement(ByVal objElement As StringTransElement) As String Implements IStringTranslator.FormatElement
        Return objElement.Value1
    End Function
End Class
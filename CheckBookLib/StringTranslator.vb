Option Strict Off
Option Explicit On
Public Class StringTranslator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Encapsulate a searchable list of key strings, each of which is associated with
    'two additional string values.

    Private Structure Element
        'Unique identifier in the list.
        'This is what is stored in the database.
        Dim strKey As String
        'Unique name of list element.
        Dim strValue1 As String
        'Alternate name of list element, not suitable for searching.
        Dim strValue2 As String

        Public Overrides Function ToString() As String
            Return strKey + "|" + strValue1
        End Function
    End Structure

    Private maudtElement() As Element '1 to mintElements
    Private mintElements As Short
    'maudtElement() indices, keyed by "#" & Element.strKey
    Private mcolKeyIndices As Collection
    'maudtElement() indices, keyed by "#" & Element.strValue1
    Private mcolValue1Indices As Collection

    '$Description Initialize an instance.

    Public Sub Init()
        mintElements = 0
        Erase maudtElement
        mcolKeyIndices = New Collection
        mcolValue1Indices = New Collection
    End Sub

    '$Description Load a text file into this object. Does NOT clear the object first.
    '   Each line of the file contains one key string and its associated value strings.
    '   The first character of each line is the separator character for that line,
    '   and must appear two more times in the line. The substring between the first
    '   and second occurences is the key string, between the second and third
    '   occurences is the first value string, and after the third occurence is
    '   the second value string.
    '$Param strPath The file name, with path.

    Public Sub LoadFile(ByVal strPath As String)

        Dim intFile As Short
        Dim strLine As String
        Dim intPos1 As Short
        Dim intPos2 As Short
        Dim strSeparator As String

        Try

            Init()
            intFile = FreeFile()
            FileOpen(intFile, strPath, OpenMode.Input)

            'Skip first line.
            'First line can contain anything, like a comment indicating the
            'next available key value.
            strLine = LineInput(intFile)
            While Not EOF(intFile)
                strLine = LineInput(intFile)
                strSeparator = Left(strLine, 1)
                intPos1 = InStr(2, strLine, strSeparator)
                If intPos1 = 0 Then
                    gRaiseError("Cannot find second separator " & strLine)
                End If
                intPos2 = InStr(intPos1 + 1, strLine, strSeparator)
                If intPos2 = 0 Then
                    gRaiseError("Cannot find third separator " & strLine)
                End If
                Add(Mid(strLine, 2, intPos1 - 2), Mid(strLine, intPos1 + 1, intPos2 - intPos1 - 1), Mid(strLine, intPos2 + 1))
            End While

            FileClose(intFile)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Add a key string and its pair of values to the list.
    '   Key and value strings do not have to be in any particular order
    '   in the list.
    '$Param strKey_ The key string. Must be unique in list.
    '$Param strValue1_ The first value string for this key string. Must be unique in list.
    '$Param strValue2_ The second value string for this key string.

    Public Sub Add(ByVal strKey_ As String, ByVal strValue1_ As String, ByVal strValue2_ As String)

        mintElements = mintElements + 1
        'UPGRADE_WARNING: Lower bound of array maudtElement was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        ReDim Preserve maudtElement(mintElements)
        With maudtElement(mintElements)
            .strKey = strKey_
            .strValue1 = strValue1_
            .strValue2 = strValue2_
        End With
        mcolKeyIndices.Add(mintElements, "#" & strKey_)
        mcolValue1Indices.Add(mintElements, "#" & strValue1_)
    End Sub

    '$Description Search the list for the specified key string. Search is case
    '   sensitive.
    '$Param strKey_ The key string to look for.
    '$Returns The index of the specified key string, or zero if it was not found.
    '   This may be passed to the strValue1() or strValue2() properties
    '   to retrieve values for that key string.

    Public Function intLookupKey(ByVal strKey_ As String) As Short
        If mcolKeyIndices.Contains("#" & strKey_) Then
            'Empirical research shows this to be extremely fast, even on large collections.
            Return mcolKeyIndices.Item("#" & strKey_)
        Else
            Return 0
        End If
    End Function

    '$Description Search the list for the specified key string, and return
    '   the strValue1 for that index.
    '$Param strKey_ The key string to look for.
    '$Returns The strValue1 of the matched entry, or an empty string if
    '   the key was not found.

    Public Function strKeyToValue1(ByVal strKey_ As String) As String
        Try
            Return maudtElement(mcolKeyIndices.Item("#" & strKey_)).strValue1
        Catch
            Return ""
        End Try
    End Function

    '$Description The number of key strings in the list.

    Public ReadOnly Property intElements() As Short
        Get
            intElements = mintElements
        End Get
    End Property

    Public ReadOnly Property strKey(ByVal intIndex As Short) As String
        Get
            strKey = maudtElement(intIndex).strKey
        End Get
    End Property

    Public ReadOnly Property strValue1(ByVal intIndex As Short) As String
        Get
            strValue1 = maudtElement(intIndex).strValue1
        End Get
    End Property

    Public ReadOnly Property strValue2(ByVal intIndex As Short) As String
        Get
            strValue2 = maudtElement(intIndex).strValue2
        End Get
    End Property

    '$Description Like intLookupKey(), but searches for a strValue1 match.

    Public Function intLookupValue1(ByVal strValue1_ As String) As Short
        Try
            'Empirical research shows this to be extremely fast, even on large collections.
            Return mcolValue1Indices.Item("#" & strValue1_)
        Catch
            Return 0
        End Try
    End Function

    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()
        Init()
    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub
End Class
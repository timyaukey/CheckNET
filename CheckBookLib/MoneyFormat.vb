Option Strict On
Option Explicit On

Public Class MoneyFormat

    Public Shared Function AmountToWords(ByVal curAmount As Decimal) As String
        '1 = one
        '19 = nineteen
        '20 = twenty
        '21 = twenty one
        '29 = twenty nine
        '100 = one hundred
        '101 = one hundred one
        '119 = one humdred nineteen
        '900 = nine hundred
        '901 = nine hundred one
        '920 = nine hundred twenty
        '1000 = one thousand
        '1200 = one thousand two hundred

        'Do millions (if > 0, 1 to 999 "million")
        'Do thousands (if >0, 1 to 999 "thousand")
        'Do remainder (0 to 999)

        Dim intMillions As Integer
        intMillions = CInt(Fix(curAmount / 1000000.0#))
        Dim intThousands As Integer
        intThousands = CInt(Fix((curAmount - intMillions * 1000000.0#) / 1000.0#))
        Dim intRemainder As Integer
        intRemainder = CInt(Fix(curAmount - intMillions * 1000000.0# - intThousands * 1000.0#))

        Dim strResult As String = ""
        If intRemainder > 0 Or (intMillions = 0 And intThousands = 0) Then
            strResult = WordsLessThan1000(intRemainder)
        End If
        If intThousands > 0 Then
            strResult = WordsLessThan1000(intThousands) & " thousand " & strResult
        End If
        If intMillions > 0 Then
            strResult = WordsLessThan1000(intMillions) & " million " & strResult
        End If

        AmountToWords = Trim(strResult)
    End Function

    Private Shared Function WordsLessThan1000(ByVal intNumber As Integer) As String
        Dim intHundredMult As Integer
        Dim intRemainder As Integer
        intHundredMult = CInt(Fix(intNumber / 100))
        intRemainder = intNumber Mod 100
        If intHundredMult > 0 Then
            If intRemainder = 0 Then
                WordsLessThan1000 = WordLessThan20(intHundredMult) & " hundred"
            Else
                WordsLessThan1000 = WordLessThan20(intHundredMult) & " hundred " & WordLessThan100(intRemainder)
            End If
        Else
            WordsLessThan1000 = WordLessThan100(intNumber)
        End If
    End Function

    Private Shared Function WordLessThan100(ByVal intNumber As Integer) As String
        Dim intTenMult As Integer
        Dim intRemainder As Integer
        If intNumber < 20 Then
            WordLessThan100 = WordLessThan20(intNumber)
        Else
            intTenMult = CInt(Fix(intNumber / 10))
            intRemainder = intNumber Mod 10
            If intRemainder > 0 Then
                WordLessThan100 = WordMultipliedByTen(intTenMult) & " " & WordLessThan20(intRemainder)
            Else
                WordLessThan100 = WordMultipliedByTen(intTenMult)
            End If
        End If
    End Function

    Private Shared Function WordLessThan20(ByVal intNumber As Integer) As String
        Select Case intNumber
            Case 0 : WordLessThan20 = "zero"
            Case 1 : WordLessThan20 = "one"
            Case 2 : WordLessThan20 = "two"
            Case 3 : WordLessThan20 = "three"
            Case 4 : WordLessThan20 = "four"
            Case 5 : WordLessThan20 = "five"
            Case 6 : WordLessThan20 = "six"
            Case 7 : WordLessThan20 = "seven"
            Case 8 : WordLessThan20 = "eight"
            Case 9 : WordLessThan20 = "nine"
            Case 10 : WordLessThan20 = "ten"
            Case 11 : WordLessThan20 = "eleven"
            Case 12 : WordLessThan20 = "twelve"
            Case 13 : WordLessThan20 = "thirteen"
            Case 14 : WordLessThan20 = "fourteen"
            Case 15 : WordLessThan20 = "fifteen"
            Case 16 : WordLessThan20 = "sixteen"
            Case 17 : WordLessThan20 = "seventeen"
            Case 18 : WordLessThan20 = "eighteen"
            Case 19 : WordLessThan20 = "nineteen"
            Case Else : WordLessThan20 = ""
        End Select
    End Function

    Private Shared Function WordMultipliedByTen(ByVal intNumber As Integer) As String
        Select Case intNumber
            Case 2 : WordMultipliedByTen = "twenty"
            Case 3 : WordMultipliedByTen = "thirty"
            Case 4 : WordMultipliedByTen = "forty"
            Case 5 : WordMultipliedByTen = "fifty"
            Case 6 : WordMultipliedByTen = "sixty"
            Case 7 : WordMultipliedByTen = "seventy"
            Case 8 : WordMultipliedByTen = "eighty"
            Case 9 : WordMultipliedByTen = "ninety"
            Case Else : WordMultipliedByTen = ""
        End Select
    End Function

End Class

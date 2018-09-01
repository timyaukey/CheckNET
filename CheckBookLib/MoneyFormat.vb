Option Strict On
Option Explicit On

Public Class MoneyFormat

    Public Shared Function strAmountToWords(ByVal curAmount As Decimal) As String
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
            strResult = strWordsLessThan1000(intRemainder)
        End If
        If intThousands > 0 Then
            strResult = strWordsLessThan1000(intThousands) & " thousand " & strResult
        End If
        If intMillions > 0 Then
            strResult = strWordsLessThan1000(intMillions) & " million " & strResult
        End If

        strAmountToWords = Trim(strResult)
    End Function

    Private Shared Function strWordsLessThan1000(ByVal intNumber As Integer) As String
        Dim intHundredMult As Integer
        Dim intRemainder As Integer
        intHundredMult = CInt(Fix(intNumber / 100))
        intRemainder = intNumber Mod 100
        If intHundredMult > 0 Then
            If intRemainder = 0 Then
                strWordsLessThan1000 = strWordLessThan20(intHundredMult) & " hundred"
            Else
                strWordsLessThan1000 = strWordLessThan20(intHundredMult) & " hundred " & strWordLessThan100(intRemainder)
            End If
        Else
            strWordsLessThan1000 = strWordLessThan100(intNumber)
        End If
    End Function

    Private Shared Function strWordLessThan100(ByVal intNumber As Integer) As String
        Dim intTenMult As Integer
        Dim intRemainder As Integer
        If intNumber < 20 Then
            strWordLessThan100 = strWordLessThan20(intNumber)
        Else
            intTenMult = CInt(Fix(intNumber / 10))
            intRemainder = intNumber Mod 10
            If intRemainder > 0 Then
                strWordLessThan100 = strWordMultipliedByTen(intTenMult) & " " & strWordLessThan20(intRemainder)
            Else
                strWordLessThan100 = strWordMultipliedByTen(intTenMult)
            End If
        End If
    End Function

    Private Shared Function strWordLessThan20(ByVal intNumber As Integer) As String
        Select Case intNumber
            Case 0 : strWordLessThan20 = "zero"
            Case 1 : strWordLessThan20 = "one"
            Case 2 : strWordLessThan20 = "two"
            Case 3 : strWordLessThan20 = "three"
            Case 4 : strWordLessThan20 = "four"
            Case 5 : strWordLessThan20 = "five"
            Case 6 : strWordLessThan20 = "six"
            Case 7 : strWordLessThan20 = "seven"
            Case 8 : strWordLessThan20 = "eight"
            Case 9 : strWordLessThan20 = "nine"
            Case 10 : strWordLessThan20 = "ten"
            Case 11 : strWordLessThan20 = "eleven"
            Case 12 : strWordLessThan20 = "twelve"
            Case 13 : strWordLessThan20 = "thirteen"
            Case 14 : strWordLessThan20 = "fourteen"
            Case 15 : strWordLessThan20 = "fifteen"
            Case 16 : strWordLessThan20 = "sixteen"
            Case 17 : strWordLessThan20 = "seventeen"
            Case 18 : strWordLessThan20 = "eighteen"
            Case 19 : strWordLessThan20 = "nineteen"
            Case Else : strWordLessThan20 = ""
        End Select
    End Function

    Private Shared Function strWordMultipliedByTen(ByVal intNumber As Integer) As String
        Select Case intNumber
            Case 2 : strWordMultipliedByTen = "twenty"
            Case 3 : strWordMultipliedByTen = "thirty"
            Case 4 : strWordMultipliedByTen = "forty"
            Case 5 : strWordMultipliedByTen = "fifty"
            Case 6 : strWordMultipliedByTen = "sixty"
            Case 7 : strWordMultipliedByTen = "seventy"
            Case 8 : strWordMultipliedByTen = "eighty"
            Case 9 : strWordMultipliedByTen = "ninety"
            Case Else : strWordMultipliedByTen = ""
        End Select
    End Function

End Class

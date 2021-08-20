Option Strict On
Option Explicit On

<TestFixture>
Public Class MiscFunctionsFixture
    Private mobjCompany As Company

    <Test>
    Public Sub Split()
        gUTSetSubTest("Utilities.Split")

        Dim astrParts() As String

        astrParts = Utilities.Split("", " ")
        gUTAssert(LBound(astrParts) = 0, "Empty string lbound")
        gUTAssert(UBound(astrParts) = 0, "Empty string ubound")
        gUTAssert(astrParts(0) = "", "Empty string content")

        astrParts = Utilities.Split("red", " ")
        gUTAssert(LBound(astrParts) = 0, "One string lbound")
        gUTAssert(UBound(astrParts) = 0, "One string ubound")
        gUTAssert(astrParts(0) = "red", "One string content")

        astrParts = Utilities.Split("red green", " ")
        gUTAssert(LBound(astrParts) = 0, "Two string lbound")
        gUTAssert(UBound(astrParts) = 1, "Two string ubound")
        gUTAssert(astrParts(0) = "red", "Two string content red")
        gUTAssert(astrParts(1) = "green", "Two string content green")

        astrParts = Utilities.Split("red green blue", " ")
        gUTAssert(LBound(astrParts) = 0, "Three string lbound")
        gUTAssert(UBound(astrParts) = 2, "Three string ubound")
        gUTAssert(astrParts(0) = "red", "Three string content red")
        gUTAssert(astrParts(1) = "green", "Three string content green")
        gUTAssert(astrParts(2) = "blue", "Three string content blue")

        astrParts = Utilities.Split(" ", " ")
        gUTAssert(LBound(astrParts) = 0, "Blank string lbound")
        gUTAssert(UBound(astrParts) = 1, "Blank string ubound")
        gUTAssert(astrParts(0) = "", "Blank string content 1")
        gUTAssert(astrParts(1) = "", "Blank string content 2")

        astrParts = Utilities.Split("red  green", " ")
        gUTAssert(LBound(astrParts) = 0, "Double blank string lbound")
        gUTAssert(UBound(astrParts) = 2, "Double blank string ubound")
        gUTAssert(astrParts(0) = "red", "Double blank string content 1")
        gUTAssert(astrParts(1) = "", "Double blank string content 2")
        gUTAssert(astrParts(2) = "green", "Double blank string content 3")

        astrParts = Utilities.Split(" red  green", " ")
        gUTAssert(LBound(astrParts) = 0, "Triple blank string lbound")
        gUTAssert(UBound(astrParts) = 3, "Triple blank string ubound")
        gUTAssert(astrParts(0) = "", "Triple blank string content 1")
        gUTAssert(astrParts(1) = "red", "Triple blank string content 2")
        gUTAssert(astrParts(2) = "", "Triple blank string content 3")
        gUTAssert(astrParts(3) = "green", "Triple blank string content 4")

        astrParts = Utilities.Split("red ", " ")
        gUTAssert(LBound(astrParts) = 0, "Trail lbound")
        gUTAssert(UBound(astrParts) = 1, "Trail ubound")
        gUTAssert(astrParts(0) = "red", "Trail content red")
        gUTAssert(astrParts(1) = "", "Trail content blank")

        astrParts = Utilities.Split("", "12")
        gUTAssert(LBound(astrParts) = 0, "12 Empty string lbound")
        gUTAssert(UBound(astrParts) = 0, "12 Empty string ubound")
        gUTAssert(astrParts(0) = "", "12 Empty string content")

        astrParts = Utilities.Split("red", "12")
        gUTAssert(LBound(astrParts) = 0, "12 string lbound")
        gUTAssert(UBound(astrParts) = 0, "12 string ubound")
        gUTAssert(astrParts(0) = "red", "12 string content")

        astrParts = Utilities.Split("12", "12")
        gUTAssert(LBound(astrParts) = 0, "12 sep lbound")
        gUTAssert(UBound(astrParts) = 1, "12 sep ubound")
        gUTAssert(astrParts(0) = "", "12 sep content 1")
        gUTAssert(astrParts(1) = "", "12 sep content 2")

        astrParts = Utilities.Split("red12green", "12")
        gUTAssert(LBound(astrParts) = 0, "12 Two string lbound")
        gUTAssert(UBound(astrParts) = 1, "12 Two string ubound")
        gUTAssert(astrParts(0) = "red", "12 Two string content red")
        gUTAssert(astrParts(1) = "green", "12 Two string content green")

        astrParts = Utilities.Split("red1212green", "12")
        gUTAssert(LBound(astrParts) = 0, "12 double string lbound")
        gUTAssert(UBound(astrParts) = 2, "12 double string ubound")
        gUTAssert(astrParts(0) = "red", "12 double string content red")
        gUTAssert(astrParts(1) = "", "12 double string content empty")
        gUTAssert(astrParts(2) = "green", "12 double string content green")
    End Sub

    <Test>
    Public Sub AgingBrackets()
        gUTSetSubTest("Current")

        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #6/10/2009#, #5/10/2009#, #6/20/2009#) = AgingUtils.CurrentLabel(), "not paid, invoiced, due in 20 days")
        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, True, #6/10/2009#, #5/10/2009#, #6/20/2009#) = AgingUtils.CurrentLabel(), "not paid, fake, invoiced, due in 20 days")

        gUTSetSubTest("Not invoiced")

        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #7/5/2009#, #6/10/2009#, #7/10/2009#) = AgingUtils.NotInvoicedLabel(), "future invoice date")

        gUTSetSubTest("Paid")

        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #5/30/2009#, #5/10/2009#, #5/20/2009#) = AgingUtils.PaidLabel(), "paid before aging date")
        gUTAssert(AgingUtils.MakeAgeBracket(#9/1/2009#, 30, False, #5/30/2009#, #5/10/2009#, #5/20/2009#) = AgingUtils.PaidLabel(), "paid WAY before aging date")

        gUTSetSubTest("Past Due")

        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #8/30/2009#, #5/10/2009#, #5/20/2009#) = AgingUtils.PastDueLabel(1, 30), "unpaid 11 days after due date")
        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #8/30/2009#, #4/10/2009#, #4/20/2009#) = AgingUtils.PastDueLabel(31, 60), "unpaid ~40 days after due date")

        gUTSetSubTest("Future")

        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #10/30/2009#, #5/10/2009#, #7/10/2009#) = AgingUtils.FutureLabel(-59, -30), "due ~40 days after aging date")
        gUTAssert(AgingUtils.MakeAgeBracket(#6/1/2009#, 30, False, #10/30/2009#, #5/10/2009#, #8/10/2009#) = AgingUtils.FutureLabel(-89, -60), "due ~70 days after aging date")
    End Sub

    <Test>
    Public Sub DateBrackets()
        gUTSetSubTest("Day count")

        gUTAssert(AgingUtils.MakeDateBracket(#6/1/2009#, 10, #6/1/2009#) = "2009/06/01", "equal to base date")
        gUTAssert(AgingUtils.MakeDateBracket(#6/2/2009#, 10, #6/1/2009#) = "2009/06/01", "day after base date")
        gUTAssert(AgingUtils.MakeDateBracket(#6/10/2009#, 10, #6/1/2009#) = "2009/06/01", "end of base bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#6/11/2009#, 10, #6/1/2009#) = "2009/06/11", "start of next bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#6/14/2009#, 10, #6/1/2009#) = "2009/06/11", "middle of next bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#6/20/2009#, 10, #6/1/2009#) = "2009/06/11", "end of next bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#6/21/2009#, 10, #6/1/2009#) = "2009/06/21", "start of next bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#5/31/2009#, 10, #6/1/2009#) = "2009/05/22", "end of previous bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#5/24/2009#, 10, #6/1/2009#) = "2009/05/22", "middle of previous bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#5/22/2009#, 10, #6/1/2009#) = "2009/05/22", "start of previous bracket")
        gUTAssert(AgingUtils.MakeDateBracket(#5/21/2009#, 10, #6/1/2009#) = "2009/05/12", "start of second previous bracket")

        gUTSetSubTest("Whole month")

        gUTAssert(AgingUtils.MakeDateBracket(#6/1/2009#, -1, #1/1/2001#) = "2009/06/01", "first day of month")
        gUTAssert(AgingUtils.MakeDateBracket(#6/2/2009#, -1, #1/1/2001#) = "2009/06/01", "second day of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/31/2009#, -1, #1/1/2001#) = "2009/07/01", "last day of month")

        gUTSetSubTest("Half month")

        gUTAssert(AgingUtils.MakeDateBracket(#7/1/2009#, -2, #1/1/2001#) = "2009/07/01", "first day of first half of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/5/2009#, -2, #1/1/2001#) = "2009/07/01", "middle of first half of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/15/2009#, -2, #1/1/2001#) = "2009/07/01", "end of first half of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/16/2009#, -2, #1/1/2001#) = "2009/07/16", "first day of second half of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/21/2009#, -2, #1/1/2001#) = "2009/07/16", "middle of second half of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/31/2009#, -2, #1/1/2001#) = "2009/07/16", "last day of second half of month")

        gUTSetSubTest("Quarter month")

        gUTAssert(AgingUtils.MakeDateBracket(#7/1/2009#, -4, #1/1/2001#) = "2009/07/01", "first day of first quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/5/2009#, -4, #1/1/2001#) = "2009/07/01", "middle of first quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/8/2009#, -4, #1/1/2001#) = "2009/07/01", "end of first quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/9/2009#, -4, #1/1/2001#) = "2009/07/09", "first day of second quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/12/2009#, -4, #1/1/2001#) = "2009/07/09", "middle of second quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/16/2009#, -4, #1/1/2001#) = "2009/07/09", "last day of second quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/17/2009#, -4, #1/1/2001#) = "2009/07/17", "first day of third quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/24/2009#, -4, #1/1/2001#) = "2009/07/17", "last day of third quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/25/2009#, -4, #1/1/2001#) = "2009/07/25", "first day of last quarter of month")
        gUTAssert(AgingUtils.MakeDateBracket(#7/31/2009#, -4, #1/1/2001#) = "2009/07/25", "last day of last quarter of month")

    End Sub

    <Test>
    Public Sub AmountsToWords()
        gUTSetSubTest("Main")

        TestOneAmountToWords(0, "zero")
        TestOneAmountToWords(1, "one")
        TestOneAmountToWords(1.1D, "one")
        TestOneAmountToWords(1.9D, "one")
        TestOneAmountToWords(2, "two")
        TestOneAmountToWords(3, "three")
        TestOneAmountToWords(3.01D, "three")
        TestOneAmountToWords(3.99D, "three")
        TestOneAmountToWords(4, "four")
        TestOneAmountToWords(5, "five")
        TestOneAmountToWords(6, "six")
        TestOneAmountToWords(7, "seven")
        TestOneAmountToWords(8, "eight")
        TestOneAmountToWords(9, "nine")
        TestOneAmountToWords(10, "ten")
        TestOneAmountToWords(11, "eleven")
        TestOneAmountToWords(12, "twelve")
        TestOneAmountToWords(13, "thirteen")
        TestOneAmountToWords(14, "fourteen")
        TestOneAmountToWords(15, "fifteen")
        TestOneAmountToWords(16, "sixteen")
        TestOneAmountToWords(17, "seventeen")
        TestOneAmountToWords(18, "eighteen")
        TestOneAmountToWords(19, "nineteen")
        TestOneAmountToWords(20, "twenty")
        TestOneAmountToWords(21, "twenty one")
        TestOneAmountToWords(22, "twenty two")
        TestOneAmountToWords(29, "twenty nine")
        TestOneAmountToWords(30, "thirty")
        TestOneAmountToWords(31, "thirty one")
        TestOneAmountToWords(39, "thirty nine")
        TestOneAmountToWords(40, "forty")
        TestOneAmountToWords(50, "fifty")
        TestOneAmountToWords(60, "sixty")
        TestOneAmountToWords(70, "seventy")
        TestOneAmountToWords(80, "eighty")
        TestOneAmountToWords(90, "ninety")
        TestOneAmountToWords(91, "ninety one")
        TestOneAmountToWords(99, "ninety nine")
        TestOneAmountToWords(100, "one hundred")
        TestOneAmountToWords(101, "one hundred one")
        TestOneAmountToWords(102, "one hundred two")
        TestOneAmountToWords(110, "one hundred ten")
        TestOneAmountToWords(111, "one hundred eleven")
        TestOneAmountToWords(112, "one hundred twelve")
        TestOneAmountToWords(119, "one hundred nineteen")
        TestOneAmountToWords(120, "one hundred twenty")
        TestOneAmountToWords(121, "one hundred twenty one")
        TestOneAmountToWords(129, "one hundred twenty nine")
        TestOneAmountToWords(130, "one hundred thirty")
        TestOneAmountToWords(190, "one hundred ninety")
        TestOneAmountToWords(199, "one hundred ninety nine")
        TestOneAmountToWords(200, "two hundred")
        TestOneAmountToWords(201, "two hundred one")
        TestOneAmountToWords(210, "two hundred ten")
        TestOneAmountToWords(211, "two hundred eleven")
        TestOneAmountToWords(900, "nine hundred")
        TestOneAmountToWords(990, "nine hundred ninety")
        TestOneAmountToWords(999, "nine hundred ninety nine")
        TestOneAmountToWords(1000, "one thousand")
        TestOneAmountToWords(1001, "one thousand one")
        TestOneAmountToWords(1002, "one thousand two")
        TestOneAmountToWords(1010, "one thousand ten")
        TestOneAmountToWords(1011, "one thousand eleven")
        TestOneAmountToWords(1019, "one thousand nineteen")
        TestOneAmountToWords(1020, "one thousand twenty")
        TestOneAmountToWords(1021, "one thousand twenty one")
        TestOneAmountToWords(1099, "one thousand ninety nine")
        TestOneAmountToWords(1100, "one thousand one hundred")
        TestOneAmountToWords(1101, "one thousand one hundred one")
        TestOneAmountToWords(1102, "one thousand one hundred two")
        TestOneAmountToWords(1119, "one thousand one hundred nineteen")
        TestOneAmountToWords(1130, "one thousand one hundred thirty")
        TestOneAmountToWords(1131, "one thousand one hundred thirty one")
        TestOneAmountToWords(1200, "one thousand two hundred")
        TestOneAmountToWords(1280, "one thousand two hundred eighty")
        TestOneAmountToWords(1289, "one thousand two hundred eighty nine")
        TestOneAmountToWords(1999, "one thousand nine hundred ninety nine")
        TestOneAmountToWords(2000, "two thousand")
        TestOneAmountToWords(2001, "two thousand one")
        TestOneAmountToWords(2015, "two thousand fifteen")
        TestOneAmountToWords(2050, "two thousand fifty")
        TestOneAmountToWords(2059, "two thousand fifty nine")
        TestOneAmountToWords(2150, "two thousand one hundred fifty")
        TestOneAmountToWords(2159, "two thousand one hundred fifty nine")
        TestOneAmountToWords(9999, "nine thousand nine hundred ninety nine")
        TestOneAmountToWords(10000, "ten thousand")
        TestOneAmountToWords(10001, "ten thousand one")
        TestOneAmountToWords(10019, "ten thousand nineteen")
        TestOneAmountToWords(10021, "ten thousand twenty one")
        TestOneAmountToWords(10200, "ten thousand two hundred")
        TestOneAmountToWords(10209, "ten thousand two hundred nine")
        TestOneAmountToWords(11000, "eleven thousand")
        TestOneAmountToWords(12500, "twelve thousand five hundred")
        TestOneAmountToWords(12502, "twelve thousand five hundred two")
        TestOneAmountToWords(12150, "twelve thousand one hundred fifty")
        TestOneAmountToWords(12152, "twelve thousand one hundred fifty two")
        TestOneAmountToWords(19000, "nineteen thousand")
        TestOneAmountToWords(19999, "nineteen thousand nine hundred ninety nine")
        TestOneAmountToWords(20000, "twenty thousand")
        TestOneAmountToWords(20001, "twenty thousand one")
        TestOneAmountToWords(20200, "twenty thousand two hundred")
        TestOneAmountToWords(20901, "twenty thousand nine hundred one")
        TestOneAmountToWords(32949, "thirty two thousand nine hundred forty nine")
        TestOneAmountToWords(100000, "one hundred thousand")
        TestOneAmountToWords(100500, "one hundred thousand five hundred")
        TestOneAmountToWords(100522, "one hundred thousand five hundred twenty two")
        TestOneAmountToWords(150000, "one hundred fifty thousand")
        TestOneAmountToWords(150001, "one hundred fifty thousand one")
        TestOneAmountToWords(152401, "one hundred fifty two thousand four hundred one")
        TestOneAmountToWords(152471, "one hundred fifty two thousand four hundred seventy one")
        TestOneAmountToWords(1000000, "one million")
        TestOneAmountToWords(1200000, "one million two hundred thousand")
        TestOneAmountToWords(1200471, "one million two hundred thousand four hundred seventy one")

    End Sub

    <Test>
    Public Sub TryParseUniversalDate()

        gUTSetSubTest("Bad format")
        AssertParseUniversalDate("", 0, 0, 0)
        AssertParseUniversalDate("1", 0, 0, 0)
        AssertParseUniversalDate("A", 0, 0, 0)
        AssertParseUniversalDate("1/3", 0, 0, 0)
        AssertParseUniversalDate("1/A/2000", 0, 0, 0)
        AssertParseUniversalDate("A/1/2000", 0, 0, 0)
        AssertParseUniversalDate("1/4/200A", 0, 0, 0)

        gUTSetSubTest("Valid dates 4 digit years")
        AssertParseUniversalDate("1/3/2000", 1, 3, 2000)
        AssertParseUniversalDate("6/13/2000", 6, 13, 2000)
        AssertParseUniversalDate("1/30/2000", 1, 30, 2000)
        AssertParseUniversalDate("12/31/2000", 12, 31, 2000)
        AssertParseUniversalDate("12/31/2001", 12, 31, 2001)
        AssertParseUniversalDate("2/28/2000", 2, 28, 2000)
        AssertParseUniversalDate("2/25/1999", 2, 25, 1999)
        AssertParseUniversalDate("2/28/2000", 2, 28, 2000)
        AssertParseUniversalDate("02/28/2000", 2, 28, 2000)
        AssertParseUniversalDate("4/29/1970", 4, 29, 1970)
        AssertParseUniversalDate("4/29/1920", 4, 29, 1920)
        AssertParseUniversalDate("4/29/1720", 4, 29, 1720)

        gUTSetSubTest("Valid dates 2 digit years")
        AssertParseUniversalDate("1/3/00", 1, 3, 2000)
        AssertParseUniversalDate("1/03/00", 1, 3, 2000)
        AssertParseUniversalDate("01/03/00", 1, 3, 2000)
        AssertParseUniversalDate("12/31/01", 12, 31, 2001)
        AssertParseUniversalDate("7/31/20", 7, 31, 2020)
        AssertParseUniversalDate("7/10/20", 7, 10, 2020)
        AssertParseUniversalDate("5/31/69", 5, 31, 2069)
        AssertParseUniversalDate("05/31/69", 5, 31, 2069)
        AssertParseUniversalDate("12/31/70", 12, 31, 1970)
        AssertParseUniversalDate("12/31/99", 12, 31, 1999)

        gUTSetSubTest("Bad month")
        AssertParseUniversalDate("0/3/2000", 0, 0, 0)
        AssertParseUniversalDate("00/3/2000", 0, 0, 0)
        AssertParseUniversalDate("13/3/2000", 0, 0, 0)

        gUTSetSubTest("Bad day")
        AssertParseUniversalDate("2/29/1999", 0, 0, 0)
        AssertParseUniversalDate("1/32/2000", 0, 0, 0)
        AssertParseUniversalDate("9/31/2000", 0, 0, 0)
        AssertParseUniversalDate("9/0/2000", 0, 0, 0)

        gUTSetSubTest("Bad year")
        AssertParseUniversalDate("2/1/3000", 0, 0, 0)

    End Sub

    Private Sub AssertParseUniversalDate(ByVal strInput As String, ByVal intMonth As Integer, ByVal intDay As Integer, ByVal intYear As Integer)
        Dim datOutput As DateTime
        If intMonth = 0 Then
            gUTAssert(Not Utilities.blnTryParseUniversalDate(strInput, datOutput), "Unexpectedly was able to parse date [" + strInput + "]")
        Else
            gUTAssert(Utilities.blnTryParseUniversalDate(strInput, datOutput), "Unable to parse date [" + strInput + "]")
            gUTAssert(datOutput = New DateTime(intYear, intMonth, intDay), "Wrong parsed values for [" + strInput + "]")
        End If
    End Sub

    Private Sub TestOneAmountToWords(ByVal curInput As Decimal, ByVal strExpectedOutput As String)

        Dim strActualOutput As String
        strActualOutput = MoneyFormat.AmountToWords(curInput)
        gUTAssert(strExpectedOutput = strActualOutput, curInput & " yields <" & strActualOutput & "> instead of <" & strExpectedOutput & ">")
    End Sub

    <OneTimeSetUp>
    Public Sub OneTimeSetup()
        mobjCompany = gobjUTStandardSetup()
    End Sub

    <OneTimeTearDown>
    Public Sub OneTimeTearDown()
        gUTStandardTearDown(mobjCompany)
    End Sub

End Class

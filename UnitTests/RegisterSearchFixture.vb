Option Strict On
Option Explicit On

<TestFixture>
Public Class RegisterSearchFixture

    Private mobjCompany As Company

    <Test>
    Public Sub TestMatchNormal()
        Dim objUTReg As UTRegister
        Dim colMatches As ICollection(Of BankTrx) = Nothing
        Dim blnExactMatch As Boolean

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("DEP", #4/1/2000#, 2000D, "Add1", 1, 1, 1, strDescription:="Descr1")
            .AddNormal("100", #4/3/2000#, -25D, "Add2", 2, 2, 2, strDescription:="Descr2")
            .AddNormal("101", #4/5/2000#, -37D, "Add3", 3, 3, 3, strDescription:="Descr3", curNormalMatchRange:=1D)
            .AddNormal("Pmt", #4/5/2000#, -37D, "Add4", 4, 4, 4, strDescription:="Descr4")
            .AddNormal("102", #4/12/2000#, -45.3D, "Add4", 5, 5, 5, strDescription:="Descr5")
            .AddNormal("Pmt", #4/18/2000#, -100D, "Add5", 6, 6, 6, strDescription:="Descr6")
        End With

        gUTSetSubTest("Search register")

        With objUTReg.objReg
            .MatchNormal(101, #4/1/2000#, 20, "", 555D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Should not find 101 (number only match)")
            'Verify date range filtering. Number, amount and descr always match,
            'so date filter is the only way to fail.
            'Date out of range before.
            .MatchNormal(100, #4/2/2000#, 0, "Descr2", -25D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Found 100 when out of date range before")
            'Date in range before.
            .MatchNormal(100, #4/2/2000#, 1, "Descr2", -25D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";2", "Expected to find 100-A")
            'Date out of range after.
            .MatchNormal(100, #4/4/2000#, 0, "Descr2", -25D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Found 100 when out of date range-B")
            'Date in range after.
            .MatchNormal(100, #4/4/2000#, 1, "Descr2", -25D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";2", "Expected to find 100-B")
            'End of date range filter checking.
            'Look for 101 without using trx number.
            'Two trx match that date and amount, because one of them
            'has an amount match range=1, but one is an exact match so only that is returned.
            .MatchNormal(0, #4/5/2000#, 10, "Descr3", -37D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-A")
            'Match all 3 properties.
            .MatchNormal(0, #4/5/2000#, 10, "Descr3", -38D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-B")
            'All the ways of matching 2 of 3 properties.
            'Descr+amount
            .MatchNormal(0, #4/4/2000#, 10, "Descr3", -38D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-D")
            'Date+amount, date+descr
            .MatchNormal(0, #4/5/2000#, 10, "Descr2", -38D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";2;3", "Expected to find 100,101-E")
            'Date+descr
            .MatchNormal(0, #4/5/2000#, 10, "Descr3", -40D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-F")
            'Date only, so fail.
            .MatchNormal(0, #4/5/2000#, 0, "Descr2", -40D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find 101-G")
            'Date only, so succeed in loose search.
            .MatchNormal(0, #4/5/2000#, 10, "Descr2", -40D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";1;2;3;4;5", "Did not expect to find 101-H")
            'Close date only, so succeed in loose search.
            .MatchNormal(0, #4/16/2000#, 10, "zzzz", -40000D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";5;6", "Expected to find Pmt-I")
            'Close date only, so succeed in loose search.
            .MatchNormal(0, #4/20/2000#, 10, "zzzz", -40000D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";6", "Expected to find Pmt-J1")
            'Close date only, so fail in strict search.
            .MatchNormal(0, #4/20/2000#, 10, "zzzz", -40000D, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-J2")
            'Just outside date, so fail in loose search.
            .MatchNormal(0, #4/28/2000#, 10, "zzzz", -40000D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-K")
            'Just outside date, so fail in loose search.
            .MatchNormal(0, #3/20/2000#, 10, "zzzz", -40000D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-L")
            'Amount within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -125D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";6", "Expected to find Pmt-M1")
            'Amount within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -84D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";6", "Expected to find Pmt-M2")
            'Amount not within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -126D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-O1")
            'Amount not within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -82D, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-O2")
        End With

    End Sub

    Private Function strConcatMatchResults(ByVal colMatches As ICollection(Of BankTrx)) As String
        Dim objElement As BankTrx
        Dim strResult As String = ""
        For Each objElement In colMatches
            strResult = strResult & ";" & objElement.RegIndex
        Next
        strConcatMatchResults = strResult
    End Function

    <Test>
    Public Sub TestMatchImportKey()
        Dim objUTReg As UTRegister
        Dim objMatch As BankTrx

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000D, "Add1", 1, 1, 1, strImportKey:="imp1")
            .AddNormal("101", #4/3/2000#, -25D, "Add2", 2, 2, 2, strImportKey:="imp2", blnFake:=True)
            .AddNormal("102", #4/3/2000#, -26D, "Add2", 3, 3, 3)
            .AddNormal("103", #4/3/2000#, -27D, "Add2", 4, 4, 4, strImportKey:="imp4")
            objMatch = .objReg.MatchImportKey("imp1")
            gUTAssert(objMatch.RegIndex = 1, "Did not find 100")
            objMatch = .objReg.MatchImportKey("imp2")
            gUTAssert(objMatch Is Nothing, "Did not expect to find 101")
            objMatch = .objReg.MatchImportKey("imp4")
            gUTAssert(objMatch.RegIndex = 4, "Did not find 103")
        End With

    End Sub

    <Test>
    Public Sub TestMatchPayee()
        Dim objUTReg As UTRegister
        Dim colMatches As ICollection(Of BankTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim objTrx As BankTrx

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000D, "Add1", 1, 1, 1, strDescription:="payee1")
            .AddNormal("101", #4/3/2000#, -25D, "Add2", 2, 2, 2, strDescription:="company2")
            .AddNormal("102", #4/5/2000#, -26D, "Add2", 3, 3, 3, strDescription:="payee1")
            .AddNormal("103", #4/7/2000#, -27D, "Add2", 4, 4, 4, strDescription:="payee1")

            .objReg.MatchPayee(#4/3/2000#, 1, "company2", True, colMatches, blnExactMatch)
            gUTAssert(colMatches.Count() = 1, "company2 fail")
            objTrx = Utilities.GetFirstElement(colMatches)
            gUTAssert(Utilities.GetFirstElement(colMatches).RegIndex = 2, "company2 index fail")
            gUTAssert(objTrx.Description = "company2", "company2 name fail")
            gUTAssert(objTrx.TrxDate = #4/3/2000#, "company2 date fail")
            gUTAssert(blnExactMatch = True, "company2 exact fail")

            .objReg.MatchPayee(#4/6/2000#, 1, "payee1", True, colMatches, blnExactMatch)
            gUTAssert(colMatches.Count() = 2, "payee1 fail")
            objTrx = Utilities.GetFirstElement(colMatches)
            gUTAssert(Utilities.GetFirstElement(colMatches).RegIndex = 3, "payee1#1 index fail")
            gUTAssert(Utilities.GetSecondElement(colMatches).RegIndex = 4, "payee1#2 index fail")
            gUTAssert(blnExactMatch = False, "payee#1 exact fail")
        End With

    End Sub

    <Test>
    Public Sub TestMatchInvoice()
        Dim objUTReg As UTRegister
        Dim colMatches As ICollection(Of BankTrx) = Nothing

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000D, "Add1", 1, 1, 1, strDescription:="payee1", strInvoiceNum:="I1000")
            .AddNormal("101", #4/3/2000#, -25D, "Add2", 2, 2, 2, strDescription:="company2", strInvoiceNum:="I1000", vcurAmount2:=-10D, strInvoiceNum2:="I1001")
            .AddNormal("102", #4/5/2000#, -26D, "Add2", 3, 3, 3, strDescription:="payee1", strInvoiceNum:="I1001")
            .AddNormal("103", #4/7/2000#, -27D, "Add2", 4, 4, 4, strDescription:="payee1", strInvoiceNum:="I1002")

            .objReg.MatchInvoice(#4/3/2000#, 10, "company2", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 I1000 fail")
            gUTAssert(Utilities.GetFirstElement(colMatches).RegIndex = 2, "company2 I1000 index fail")

            .objReg.MatchInvoice(#4/3/2000#, 10, "company2", "I1001", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 I1001 fail")
            gUTAssert(Utilities.GetFirstElement(colMatches).RegIndex = 2, "company2 I1001 index fail")

            .objReg.MatchInvoice(#4/5/2000#, 1, "company2", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 0, "company2 I1000 -2 fail")

            .objReg.MatchInvoice(#4/5/2000#, 3, "company3", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 0, "company3 I1000 fail")

            .objReg.MatchInvoice(#4/5/2000#, 3, "company2", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 I1000 -3 fail")
        End With

    End Sub

    <Test>
    Public Sub TestMatchPONumber()
        Dim objUTReg As UTRegister
        Dim colMatches As ICollection(Of BankTrx) = Nothing

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000D, "Add1", 1, 1, 1, strDescription:="payee1", strPONumber:="P1")
            .AddNormal("101", #4/3/2000#, -25D, "Add2", 2, 2, 2, strDescription:="company2", strPONumber:="P1", vcurAmount2:=-10D, strPONumber2:="P2")
            .AddNormal("102", #4/5/2000#, -26D, "Add2", 3, 3, 3, strDescription:="payee1", strPONumber:="P2")
            .AddNormal("103", #4/7/2000#, -27D, "Add2", 4, 4, 4, strDescription:="payee1", strPONumber:="P3")

            .objReg.MatchPONumber(#4/3/2000#, 10, "company2", "P1", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 P1 fail")
            gUTAssert(Utilities.GetFirstElement(colMatches).RegIndex = 2, "company2 P1 index fail")

            .objReg.MatchPONumber(#4/3/2000#, 10, "company2", "P2", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 P2 fail")
            gUTAssert(Utilities.GetFirstElement(colMatches).RegIndex = 2, "company2 I1001 index fail")

            .objReg.MatchPONumber(#4/5/2000#, 1, "company2", "P1", colMatches)
            gUTAssert(colMatches.Count() = 0, "company2 P1 -2 fail")

            .objReg.MatchPONumber(#4/5/2000#, 3, "company3", "P1", colMatches)
            gUTAssert(colMatches.Count() = 0, "company3 P1 fail")

            .objReg.MatchPONumber(#4/5/2000#, 3, "company2", "P1", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 P1 -3 fail")
        End With

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

Option Strict On
Option Explicit On

<TestFixture>
Public Class TrxNameSummaryFixture

    Private Const strAPAccountKey As String = "1"
    Private Const strAPRegKey As String = "1"
    Private Const strAPCatKey As String = "1.1"

    Private Const strARAccountKey As String = "2"
    Private Const strARRegKey As String = "1"
    Private Const strARCatKey As String = "2.1"

    Private Const strBankAccountKey As String = "3"
    Private Const strBankRegKey As String = "1"

    <Test>
    Public Sub APEmptyPayables()
        Dim objUTReg As UTRegister = MakeEmptyPayables()

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2050#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 0, "Should not have found anything")
    End Sub

    <Test>
    Public Sub AREmptyReceivables()
        Dim objUTReg As UTRegister = MakeEmptyReceivables()

        Dim colSummary As List(Of CustomerSummary) = CustomerSummary.ScanTrx(Of CustomerSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2050#, Account.SubType.Asset_AccountsReceivable)
        gUTAssert(colSummary.Count = 0, "Should not have found anything")
    End Sub

    <Test>
    Public Sub APNoPayablesInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #2/1/1999#, -100@, "fail", 1, 1, 1)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/1995#, #1/1/1995#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 0, "Should not have found anything")
    End Sub

    <Test>
    Public Sub APOneFuturePayableInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #12/18/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#1/18/2000#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #12/1/1999#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.TrxName = "Test", "Wrong name")
        gUTAssert(objSummary.Balance = -100@, "Wrong balance")
        gUTAssert(objSummary.SummaryFutureCharges.Subtotal = -100@, "Wrong future charges")
    End Sub

    <Test>
    Public Sub APOneCurrentPayableInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #12/18/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#1/18/2000#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #2/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.TrxName = "Test", "Wrong name")
        gUTAssert(objSummary.Balance = -100@, "Wrong balance")
        gUTAssert(objSummary.SummaryCurrentCharges.Subtotal = -100@, "Wrong current charges")
    End Sub

    <Test>
    Public Sub APOne30DayPayableInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/20/1999#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -100@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -100@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APOneOver90PayableInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #2/1/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test")

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.TrxName = "Test", "Wrong name")
        gUTAssert(objSummary.Balance = -100@, "Wrong balance")
        gUTAssert(objSummary.SummaryCurrentCharges.Subtotal = 0@, "Wrong current charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = -100@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub AROneOver90PayableInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyReceivables()
        objUTReg.AddNormal("Inv", #2/1/1999#, 100@, "fail", 1, 1, 1, strDescription:="Test")

        Dim colSummary As List(Of CustomerSummary) = CustomerSummary.ScanTrx(Of CustomerSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Asset_AccountsReceivable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As CustomerSummary = colSummary(0)
        gUTAssert(objSummary.TrxName = "Test", "Wrong name")
        gUTAssert(objSummary.Balance = 100@, "Wrong balance")
        gUTAssert(objSummary.SummaryCurrentCharges.Subtotal = 0@, "Wrong current charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 100@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APTwo30DayPayablesInDateRange()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("1002", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/24/1999#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -150@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -150@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APTwoVendors()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("1001", #9/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#11/20/1999#)
        objUTReg.AddNormal("1002", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test2", datDueDate:=#12/24/1999#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 2, "Should have found two")
        Dim objSummary1 As VendorSummary = colSummary(0)
        gUTAssert(objSummary1.TrxName = "Test1", "Wrong name for summary 1")
        gUTAssert(objSummary1.Balance = -100@, "Wrong balance for summary 1")
        gUTAssert(objSummary1.Summary31To60Charges.Subtotal = -100@, "Wrong 31-60 day charges for summary 1")
        gUTAssert(objSummary1.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges for summary 1")
    End Sub

    <Test>
    Public Sub APApplySinglePayment()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("Inv", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/24/1999#)
        objUTReg.AddNormal("Crm", #12/31/1999#, 90@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/31/199#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -60@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -60@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub ARApplySinglePayment()
        Dim objUTReg As UTRegister = MakeEmptyReceivables()
        objUTReg.AddNormal("Inv", #12/20/1999#, 100@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("Inv", #12/24/1999#, 50@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/24/1999#)
        objUTReg.AddNormal("Crm", #12/31/1999#, -90@, "fail", 1, 1, 1, strDescription:="Test", datDueDate:=#12/31/199#)

        Dim colSummary As List(Of CustomerSummary) = CustomerSummary.ScanTrx(Of CustomerSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Asset_AccountsReceivable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As CustomerSummary = colSummary(0)
        gUTAssert(objSummary.Balance = 60@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = 60@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APApplySinglePaymentMultipleVendors()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        'The trx numbers don't actually matter - it tells charges from payments by the sign of the amount.
        objUTReg.AddNormal("1001", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("1002", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test2", datDueDate:=#12/24/1999#)
        objUTReg.AddNormal("Pmt", #12/31/1999#, 90@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/31/199#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 2, "Should have found two")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -10@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -10@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APOverpayment()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        'The trx numbers don't actually matter - it tells charges from payments by the sign of the amount.
        objUTReg.AddNormal("1001", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("1002", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/24/1999#)
        objUTReg.AddNormal("Pmt", #12/31/1999#, 151@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/31/199#)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = 1@, "Wrong balance")
        gUTAssert(objSummary.SummaryPayments.Subtotal = 1@, "Wrong payments")
    End Sub

    <Test>
    Public Sub APApplyOneReplicaTrx()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("Inv", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test2", datDueDate:=#12/24/1999#)

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        AddPayment(objBankReg, "1001", #1/1/2000#, "Test1", strAPCatKey, "", -90@)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/1/2000#, #1/1/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 2, "Should have found two")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -10@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -10@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APApplyTwoReplicaTrx()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #12/20/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/20/1999#)
        objUTReg.AddNormal("Inv", #12/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/24/1999#)

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        AddPayment(objBankReg, "1001", #1/1/2000#, "Test1", strAPCatKey, "", -90@)
        AddPayment(objBankReg, "1002", #1/2/2000#, "Test1", strAPCatKey, "", -20@)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/2/2000#, #1/2/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -40@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -40@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APMatchInvoiceNum()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #10/24/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#10/24/1999#, strInvoiceNum:="I1")
        objUTReg.AddNormal("Inv", #10/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#11/20/1999#, strInvoiceNum:="2991")
        objUTReg.AddNormal("Inv", #10/24/1999#, -99@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/10/1999#, strInvoiceNum:="2992")

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        AddPayment(objBankReg, "1001", #1/1/2000#, "Test1", strAPCatKey, "2991", -50@)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/2/2000#, #1/2/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -199@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -99@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.Summary61To90Charges.Subtotal = -100@, "Wrong 61-90 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APMultiPaymentsOnInvoiceNum()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #10/24/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#10/24/1999#, strInvoiceNum:="I1")
        objUTReg.AddNormal("Inv", #10/24/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#11/20/1999#, strInvoiceNum:="2991")
        objUTReg.AddNormal("Inv", #10/24/1999#, -99@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/10/1999#, strInvoiceNum:="2992")

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        AddPayment(objBankReg, "1001", #1/1/2000#, "Test1", strAPCatKey, "2991", -40@)
        AddPayment(objBankReg, "1002", #12/10/1999#, "Test1", strAPCatKey, "2991", -7@)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/2/2000#, #1/2/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 1, "Should have found one")
        Dim objSummary As VendorSummary = colSummary(0)
        gUTAssert(objSummary.Balance = -202@, "Wrong balance")
        gUTAssert(objSummary.Summary1To30Charges.Subtotal = -99@, "Wrong 1-30 day charges")
        gUTAssert(objSummary.Summary31To60Charges.Subtotal = -3@, "Wrong 31-60 day charges")
        gUTAssert(objSummary.Summary61To90Charges.Subtotal = -100@, "Wrong 61-90 day charges")
        gUTAssert(objSummary.SummaryOver90Charges.Subtotal = 0@, "Wrong over 90 day charges")
    End Sub

    <Test>
    Public Sub APOnePaymentOnMultiInvoiceNum()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #10/21/1999#, -98@, "fail", 1, 1, 1, strDescription:="Test2", datDueDate:=#1/10/1999#, strInvoiceNum:="5000")
        objUTReg.AddNormal("Inv", #10/21/1999#, -99@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/10/1999#, strInvoiceNum:="2992")
        objUTReg.AddNormal("Inv", #10/24/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#10/24/1999#, strInvoiceNum:="I1")
        objUTReg.AddNormal("Inv", #10/25/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#11/20/1999#, strInvoiceNum:="2991")

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        AddPayment(objBankReg, "1001", #1/1/2000#, "Test2", strAPCatKey, "", -98@)
        AddPayment(objBankReg, "1002", #12/10/1999#, "Test1", strAPCatKey, "", -249@)

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/2/2000#, #1/2/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 2, "Should have found two")
        Dim objSummary1 As VendorSummary = colSummary(0)
        gUTAssert(objSummary1.Balance = 0@, "Wrong balance1")
        Dim objSummary2 As VendorSummary = colSummary(0)
        gUTAssert(objSummary2.Balance = 0@, "Wrong balance2")
    End Sub

    <Test>
    Public Sub APMultiSplitsToMultiInvoiceNum()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #10/21/1999#, -98@, "fail", 1, 1, 1, strDescription:="Test2", datDueDate:=#1/10/1999#, strInvoiceNum:="5000")
        objUTReg.AddNormal("Inv", #10/21/1999#, -99@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/10/1999#, strInvoiceNum:="2992")
        objUTReg.AddNormal("Inv", #10/24/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#10/24/1999#, strInvoiceNum:="I1")
        objUTReg.AddNormal("Inv", #10/25/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#11/20/1999#, strInvoiceNum:="2991")

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        Dim objBankTrx As BankTrx

        objBankTrx = CreateTrx(objBankReg, "1001", #11/1/1999#, "Test2")
        AddSplit(objBankTrx, strAPCatKey, "5000", -98@)
        objBankReg.NewAddEnd(objBankTrx, New LogAdd(), "")

        objBankTrx = CreateTrx(objBankReg, "1002", #11/1/1999#, "Test1")
        AddSplit(objBankTrx, strAPCatKey, "2991", -50@)
        AddSplit(objBankTrx, strAPCatKey, "2992", -99@)
        AddSplit(objBankTrx, strAPCatKey, "I1", -100@)
        AddSplit(objBankTrx, strAPCatKey, "NOSUCH", -1000@)
        objBankReg.NewAddEnd(objBankTrx, New LogAdd(), "")

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/2/2000#, #1/2/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 2, "Should have found two")
        Dim objSummary1 As VendorSummary = colSummary(0)
        gUTAssert(objSummary1.Balance = 1000@, "Wrong balance1")
        gUTAssert(objSummary1.SummaryPayments.Subtotal = 1000@, "Wrong payments1")
        Dim objSummary2 As VendorSummary = colSummary(1)
        gUTAssert(objSummary2.Balance = 0@, "Wrong balance2")
    End Sub

    <Test>
    Public Sub APMultiSplitsToInvoiceNumSubset()
        Dim objUTReg As UTRegister = MakeEmptyPayables()
        objUTReg.AddNormal("Inv", #10/21/1999#, -98@, "fail", 1, 1, 1, strDescription:="Test2", datDueDate:=#1/10/1999#, strInvoiceNum:="5000")
        objUTReg.AddNormal("Inv", #10/21/1999#, -99@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#12/10/1999#, strInvoiceNum:="2992")
        objUTReg.AddNormal("Inv", #10/24/1999#, -100@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#10/24/1999#, strInvoiceNum:="I1")
        objUTReg.AddNormal("Inv", #10/25/1999#, -50@, "fail", 1, 1, 1, strDescription:="Test1", datDueDate:=#11/20/1999#, strInvoiceNum:="2991")

        Dim objBankReg As Register = MakeEmptyBankAccount(objUTReg)
        Dim objBankTrx As BankTrx

        objBankTrx = CreateTrx(objBankReg, "1001", #11/1/1999#, "Test2")
        AddSplit(objBankTrx, strAPCatKey, "5000", -98@)
        objBankReg.NewAddEnd(objBankTrx, New LogAdd(), "")

        objBankTrx = CreateTrx(objBankReg, "1002", #11/1/1999#, "Test1")
        AddSplit(objBankTrx, strAPCatKey, "2991", -50@)
        AddSplit(objBankTrx, strAPCatKey, "2992", -99@)
        objBankReg.NewAddEnd(objBankTrx, New LogAdd(), "")

        Dim colSummary As List(Of VendorSummary) = VendorSummary.ScanTrx(Of VendorSummary)(objUTReg.objCompany,
            #1/2/2000#, #1/2/2000#, Account.SubType.Liability_AccountsPayable)
        gUTAssert(colSummary.Count = 2, "Should have found two")
        Dim objSummary1 As VendorSummary = colSummary(0)
        gUTAssert(objSummary1.Balance = -100@, "Wrong balance-1")
        gUTAssert(objSummary1.Summary61To90Charges.Subtotal = -100@, "Wrong 61to90charges-1")
        Dim objSummary2 As VendorSummary = colSummary(1)
        gUTAssert(objSummary2.Balance = 0@, "Wrong balance-2")
    End Sub

    Private Function CreateTrx(ByVal objReg As Register, ByVal strNum As String, ByVal datTrxDate As DateTime,
                               ByVal strDescription As String) As BankTrx
        Dim objTrx As BankTrx = New BankTrx(objReg)
        objTrx.NewStartNormal(True, strNum, datTrxDate, strDescription, "", BaseTrx.TrxStatus.Unreconciled,
                              False, 0@, False, False, 0, Nothing, Nothing)
        Return objTrx
    End Function

    Private Sub AddPayment(ByVal objReg As Register, ByVal strTrxNum As String,
                           ByVal datTrxDate As DateTime, ByVal strDescription As String, ByVal strCatKey As String,
                           ByVal strInvoiceNum As String, ByVal curAmount As Decimal)
        Dim objBankTrx As BankTrx = CreateTrx(objReg, strTrxNum, datTrxDate, strDescription)
        AddSplit(objBankTrx, strCatKey, strInvoiceNum, curAmount)
        objReg.NewAddEnd(objBankTrx, New LogAdd(), "")
    End Sub

    Private Sub AddSplit(ByVal objBankTrx As BankTrx, ByVal strCatKey As String, ByVal strInvoiceNum As String, ByVal curAmount As Decimal)
        objBankTrx.AddSplit("", strCatKey, "", strInvoiceNum, #1/1/0001#, #1/1/0001#, "", "", curAmount)
    End Sub

    Private Sub FinishLoadingSecondaryReg(ByVal objReg As Register)
        objReg.Sort()
        objReg.LoadApply()
        objReg.Sort()
        objReg.LoadFinish()
    End Sub

    Private Function MakeEmptyPayables() As UTRegister
        Dim objUTReg As UTRegister
        objUTReg = gobjUTNewReg()
        objUTReg.objAcct.AcctType = Account.AccountType.Liability
        objUTReg.objAcct.AcctSubType = Account.SubType.Liability_AccountsPayable
        objUTReg.objAcct.AccountKey = CInt(strAPAccountKey)
        Return objUTReg
    End Function

    Private Function MakeEmptyReceivables() As UTRegister
        Dim objUTReg As UTRegister
        objUTReg = gobjUTNewReg()
        objUTReg.objAcct.AcctType = Account.AccountType.Asset
        objUTReg.objAcct.AcctSubType = Account.SubType.Asset_AccountsReceivable
        objUTReg.objAcct.AccountKey = CInt(strARAccountKey)
        Return objUTReg
    End Function

    Private Function MakeEmptyBankAccount(ByVal objUTReg As UTRegister) As Register
        Dim objBankAcct As Account = objUTReg.AddAccount(strBankRegKey)
        objBankAcct.AcctType = Account.AccountType.Asset
        objBankAcct.AcctSubType = Account.SubType.Asset_CheckingAccount
        objBankAcct.AccountKey = CInt(strBankAccountKey)
        Dim objBankReg As Register = objBankAcct.Registers(0)
        FinishLoadingSecondaryReg(objBankReg)
        Return objBankReg
    End Function
End Class

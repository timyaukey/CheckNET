Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class RptScanSplitsForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Form to iterate transaction splits for selected accounts in a transaction date
    'range, and process them in some way typically a report or other summary.

    Public Enum SplitReportType
        glngSPLTRPT_TOTALS = 1
        glngSPLTRPT_PAYABLES = 2
    End Enum

    Private mlngRptType As SplitReportType
    Private mlngSplitCount As Integer

    'Array with elements corresponding to mobjCats elements.
    'For category summary report.
    Private maudtCatTotals() As PublicTypes.CategoryInfo

    'Total accounts payable by age bracket (days after due date) (0=0-30, 1=31-60, etc).
    Private macurPayables(20) As Decimal
    Private mcurPayablesTotal As Decimal
    Private mcurPayablesCurrent As Decimal
    Private mcurPayablesFuture As Decimal
    Private mcolPayables As Collection

    'Accounts selected to scan.
    Private mcolSelectAccounts As Collection

    'Other scan parameters.
    Private mdatStart As Date
    Private mdatEnd As Date
    Private mblnIncludeFake As Boolean
    Private mblnIncludeGenerated As Boolean
    Private mstrCatKey As String
    Private mdatReportDate As Date

    Public Sub ShowMe(ByVal lngType As SplitReportType)
        Try

            mlngRptType = lngType
            CustomizeForm()
            gLoadAccountListBox(lstAccounts)
            Me.Show()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub RptScanSplitsForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 350
        Me.Height = 450
    End Sub

    Private Sub CustomizeForm()
        Try

            Select Case mlngRptType
                Case SplitReportType.glngSPLTRPT_TOTALS
                    Me.Text = "Report of Totals By Category"
                Case SplitReportType.glngSPLTRPT_PAYABLES
                    Me.Text = "Accounts Payable Report"
                    lblCategory.Visible = True
                    cboCategory.Visible = True
                    gLoadComboFromStringTranslator(cboCategory, gobjCategories, True)
                    lblReportDate.Visible = True
                    txtReportDate.Visible = True
                Case Else
                    gRaiseError("Unrecognized category report type")
            End Select

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            If blnBadSpecs() Then
                Exit Sub
            End If
            Dim intIndex As Short
            If mlngRptType = SplitReportType.glngSPLTRPT_TOTALS Then
                InitCatTotals()
            ElseIf mlngRptType = SplitReportType.glngSPLTRPT_PAYABLES Then
                For intIndex = LBound(macurPayables) To UBound(macurPayables)
                    macurPayables(intIndex) = 0
                Next
                mcurPayablesTotal = 0
                mcurPayablesCurrent = 0
                mcurPayablesFuture = 0
                mcolPayables = New Collection
            End If
            IterateSplits()
            ShowResults()
            'Leave form open so can rerun with different params.
            'Unload Me

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    '$Description Display a diagnostic message and return True iff there are spec errors.

    Private Function blnBadSpecs() As Boolean
        Try

            blnBadSpecs = True
            If lstAccounts.SelectedItems.Count < 1 Then
                MsgBox("You must select at least one account.", MsgBoxStyle.Critical)
                Exit Function
            End If

            If Not gblnValidDate(txtStartDate.Text) Then
                MsgBox("Invalid transaction starting date.", MsgBoxStyle.Critical)
                Exit Function
            End If
            mdatStart = CDate(txtStartDate.Text)

            If Not gblnValidDate(txtEndDate.Text) Then
                MsgBox("Invalid transaction ending date.", MsgBoxStyle.Critical)
                Exit Function
            End If
            mdatEnd = CDate(txtEndDate.Text)

            If mdatEnd < mdatStart Then
                MsgBox("Ending date may not be earlier than starting date.", MsgBoxStyle.Critical)
                Exit Function
            End If

            If cboCategory.Visible Then
                If cboCategory.SelectedIndex = -1 Then
                    mstrCatKey = ""
                    'MsgBox "You must select a category.", vbCritical
                    'Exit Function
                Else
                    mstrCatKey = gobjCategories.strKey(gintVB6GetItemData(cboCategory, cboCategory.SelectedIndex))
                End If
            End If

            If txtReportDate.Visible Then
                If Not gblnValidDate(txtReportDate.Text) Then
                    MsgBox("Invalid report date.", MsgBoxStyle.Critical)
                    Exit Function
                End If
                mdatReportDate = CDate(txtReportDate.Text)
            End If

            mblnIncludeFake = (chkIncludeFake.CheckState = System.Windows.Forms.CheckState.Checked)
            mblnIncludeGenerated = (chkIncludeGenerated.CheckState = System.Windows.Forms.CheckState.Checked)

            blnBadSpecs = False

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub InitCatTotals()
        Dim intCharPos As Short
        Dim intCatIndex As Short
        Dim strCatName As String

        Try

            'UPGRADE_WARNING: Lower bound of array maudtCatTotals was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim maudtCatTotals(gobjCategories.intElements)
            For intCatIndex = 1 To gobjCategories.intElements
                strCatName = gobjCategories.strValue1(intCatIndex)
                For intCharPos = 1 To Len(strCatName)
                    If Mid(strCatName, intCharPos, 1) = ":" Then
                        maudtCatTotals(intCatIndex).intNestingLevel = maudtCatTotals(intCatIndex).intNestingLevel + 1
                    End If
                Next
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub IterateSplits()
        Dim objAcct As Account
        Dim objReg As Register
        Dim intAcctIdx As Short

        Try

            mcolSelectAccounts = New Collection
            mlngSplitCount = 0
            For intAcctIdx = 0 To lstAccounts.Items.Count - 1
                If lstAccounts.GetSelected(intAcctIdx) Then
                    objAcct = gcolAccounts.Item(intAcctIdx)
                    mcolSelectAccounts.Add(objAcct)
                    For Each objReg In objAcct.colRegisters
                        ScanRegister(objReg)
                    Next objReg
                End If
            Next
            lblProgress.Text = ""
            MsgBox("Checked " & mlngSplitCount & " splits.", MsgBoxStyle.Information)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ScanRegister(ByVal objReg As Register)
        Dim datLastProgress As Date
        Dim objTrx As Trx
        Dim lngTrxCount As Integer
        Dim objCursor As RegCursor
        Dim datDate As Date
        Dim blnInclude As Boolean
        Dim objSplit As Split_Renamed
        Dim strRegTitle As String

        Try

            lngTrxCount = objReg.lngTrxCount
            strRegTitle = objReg.strTitle
            objCursor = New RegCursor
            objCursor.Init(objReg)
            Do
                objTrx = objCursor.objGetNext
                If objTrx Is Nothing Then
                    Exit Do
                End If
                With objTrx
                    datDate = .datDate
                    If datDate <> datLastProgress Then
                        lblProgress.Text = strRegTitle & "  " & gstrFormatDate(datDate)
                        System.Windows.Forms.Application.DoEvents()
                        datLastProgress = datDate
                    End If
                    If .lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                        If datDate >= mdatStart And datDate <= mdatEnd Then
                            blnInclude = True
                            If .blnFake Then
                                If Not mblnIncludeFake Then
                                    blnInclude = False
                                End If
                            End If
                            If .blnAutoGenerated Then
                                If Not mblnIncludeGenerated Then
                                    blnInclude = False
                                End If
                            End If
                            If blnInclude Then
                                For Each objSplit In objTrx.colSplits
                                    mlngSplitCount = mlngSplitCount + 1
                                    ProcessSplit(objReg, objTrx, objSplit)
                                Next objSplit
                            End If
                        End If
                    End If
                End With
            Loop

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ProcessSplit(ByVal objReg As Register, ByRef objTrx As Trx, ByVal objSplit As Split_Renamed)
        Dim intCatIndex As Short

        Try

            Dim datInvoiceDate As Date
            Dim datDueDate As Date
            Dim intAgeInDays As Short
            Dim intAgeBracket As Short
            Select Case mlngRptType
                Case SplitReportType.glngSPLTRPT_TOTALS
                    intCatIndex = gobjCategories.intLookupKey(objSplit.strCategoryKey)
                    If intCatIndex = 0 Then
                        MsgBox("Could not find category key " & objSplit.strCategoryKey & " for " & "trx dated " & gstrFormatDate(objTrx.datDate) & " " & "in register " & objReg.strTitle)
                    Else
                        With maudtCatTotals(intCatIndex)
                            .lngCount = .lngCount + 1
                            .curAmount = .curAmount + objSplit.curAmount
                        End With
                    End If
                Case SplitReportType.glngSPLTRPT_PAYABLES
                    If objSplit.strCategoryKey = mstrCatKey Or mstrCatKey = "" Then
                        'Find the due date (used to calculate age),
                        'and if necessary estimate invoice date.
                        gGetSplitDates(objTrx, objSplit, datInvoiceDate, datDueDate)
                        'If item is invoiced as of the report date.
                        If datInvoiceDate <= mdatReportDate Then
                            'If item was not paid as of the report date.
                            If objTrx.blnFake Or (mdatReportDate < objTrx.datDate) Then
                                'Count it!
                                'We use the difference between datDueDate and mdatReportDate as
                                'the age because we only include in the report trx that were unpaid
                                'as of mdatReportDate, i.e. on mdatReportDate they were late by the
                                'number of days between datDueDate and mdatReportDate. If you think
                                'casually about the problem you might use the difference between
                                'datDueDate and transaction/payment date as the age, but this is actually
                                'a related but different number: The number of days late the payment
                                'ACTUALLY was, as opposed to the number of days late the payment was
                                'AS OF THE REPORT DATE. Unlike for the "actual days late" number, a
                                'transaction isn't reported AT ALL if it was paid on or before the
                                'report date. Another way of looking at it is one number describes
                                'PAID debts where you KNOW how late they were (or weren't), and the
                                'other describes UNPAID debts when you DON'T know how late they WILL
                                'BE when paid but you know how late they are NOW (or when the report
                                'was run).
                                If mdatReportDate <= datDueDate Then
                                    'Current or future payable.
                                    'Anything due more than 30 days in the future is a "future".
                                    'This definition is rather arbitrary.
                                    'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
                                    If DateDiff(Microsoft.VisualBasic.DateInterval.Day, mdatReportDate, datDueDate) <= 30 Then
                                        mcurPayablesCurrent = mcurPayablesCurrent + objSplit.curAmount
                                    Else
                                        mcurPayablesFuture = mcurPayablesFuture + objSplit.curAmount
                                    End If
                                Else
                                    'Payable is at or past due date, and so belongs in one of the age brackets.
                                    'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
                                    intAgeInDays = DateDiff(Microsoft.VisualBasic.DateInterval.Day, datDueDate, mdatReportDate)
                                    intAgeBracket = (intAgeInDays - 1) / 30
                                    If intAgeBracket <= UBound(macurPayables) Then
                                        macurPayables(intAgeBracket) = macurPayables(intAgeBracket) + objSplit.curAmount
                                    End If
                                End If
                                mcurPayablesTotal = mcurPayablesTotal + objSplit.curAmount
                                mcolPayables.Add(objSplit)
                            End If
                        End If
                    End If
                Case Else
                    gRaiseError("Unrecognized report type")
            End Select

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ShowResults()
        Dim frmSumRpt As CatSumRptForm

        Try

            Dim strPayables As String
            Dim intIndex As Short
            Dim curReconTotal As Decimal
            Select Case mlngRptType
                Case SplitReportType.glngSPLTRPT_TOTALS
                    frmSumRpt = New CatSumRptForm
                    frmSumRpt.ShowMe(maudtCatTotals, mcolSelectAccounts, gobjCategories, mdatStart, mdatEnd, mblnIncludeFake, mblnIncludeGenerated)
                Case SplitReportType.glngSPLTRPT_PAYABLES
                    strPayables = "Total payables as of " & gstrFormatDate(mdatReportDate) & " are $" & gstrFormatCurrency(mcurPayablesTotal) & vbCrLf & "Payables due in 30 days or less are $" & gstrFormatCurrency(mcurPayablesCurrent) & vbCrLf & "Payables more than 30 days out are $" & gstrFormatCurrency(mcurPayablesFuture)
                    curReconTotal = mcurPayablesCurrent + mcurPayablesFuture
                    For intIndex = LBound(macurPayables) To UBound(macurPayables)
                        If macurPayables(intIndex) <> 0 Then
                            strPayables = strPayables & vbCrLf & "Aged " & CStr(((intIndex + 0) * 30) + 1) & "-" & CStr((intIndex + 1) * 30) & " days: $" & gstrFormatCurrency(macurPayables(intIndex))
                        End If
                        curReconTotal = curReconTotal + macurPayables(intIndex)
                    Next
                    MsgBox(strPayables, MsgBoxStyle.Information)
                    If curReconTotal <> mcurPayablesTotal Then
                        MsgBox("Accounts payable current, future and age brackets do not add up to total!", MsgBoxStyle.Critical)
                    End If
                Case Else
                    gRaiseError("Unrecognized category report type")
            End Select

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub
End Class
Option Strict Off
Option Explicit On


Friend Class RptScanSplitsForm
    Inherits System.Windows.Forms.Form

    'Form to iterate transaction splits for selected accounts in a transaction date
    'range, and process them in some way typically a report or other summary.

    Public Enum SplitReportType
        Totals = 1
    End Enum

    Private mobjCompany As Company
    Private mlngRptType As SplitReportType
    Private mlngSplitCount As Integer
    Private mobjHostUI As IHostUI

    'Array with elements corresponding to mobjCats elements.
    'For category summary report.
    Private maudtCatTotals() As PublicTypes.CategoryInfo

    'Total accounts payable by age bracket (days after due date) (0=0-30, 1=31-60, etc).
    Private macurPayables(20) As Decimal
    Private mcurPayablesTotal As Decimal
    Private mcurPayablesCurrent As Decimal
    Private mcurPayablesFuture As Decimal

    'Accounts selected to scan.
    Private mcolSelectAccounts As ICollection(Of Account)

    'Other scan parameters.
    Private mdatStart As Date
    Private mdatEnd As Date
    Private mblnIncludeFake As Boolean
    Private mblnIncludeGenerated As Boolean
    Private mstrCatKey As String
    Private mdatReportDate As Date

    Public Sub ShowMe(ByVal lngType As SplitReportType, ByVal objHostUI As IHostUI)
        Try
            mobjCompany = objHostUI.objCompany
            mlngRptType = lngType
            'This form is an MDI child.
            'This code simulates the VB6 
            ' functionality of automatically
            ' loading and showing an MDI child's parent.
            mobjHostUI = objHostUI
            Me.MdiParent = mobjHostUI.objGetMainForm()
            mobjHostUI.objGetMainForm().Show()
            CustomizeForm()
            UITools.LoadAccountListBox(lstAccounts, mobjCompany)
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
                Case SplitReportType.Totals
                    Me.Text = "Report of Totals By Category"
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
            If mlngRptType = SplitReportType.Totals Then
                InitCatTotals()
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
                mobjHostUI.ErrorMessageBox("You must select at least one account.")
                Exit Function
            End If

            If Not Utilities.blnIsValidDate(txtStartDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid transaction starting date.")
                Exit Function
            End If
            mdatStart = CDate(txtStartDate.Text)

            If Not Utilities.blnIsValidDate(txtEndDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid transaction ending date.")
                Exit Function
            End If
            mdatEnd = CDate(txtEndDate.Text)

            If mdatEnd < mdatStart Then
                mobjHostUI.ErrorMessageBox("Ending date may not be earlier than starting date.")
                Exit Function
            End If

            If cboCategory.Visible Then
                If cboCategory.SelectedIndex = -1 Then
                    mstrCatKey = ""
                    'MsgBox "You must select a category.", vbCritical
                    'Exit Function
                Else
                    mstrCatKey = mobjCompany.Categories.strKey(UITools.GetItemData(cboCategory, cboCategory.SelectedIndex))
                End If
            End If

            If txtReportDate.Visible Then
                If Not Utilities.blnIsValidDate(txtReportDate.Text) Then
                    mobjHostUI.ErrorMessageBox("Invalid report date.")
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

            ReDim maudtCatTotals(mobjCompany.Categories.intElements)
            For intCatIndex = 1 To mobjCompany.Categories.intElements
                strCatName = mobjCompany.Categories.strValue1(intCatIndex)
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

            mcolSelectAccounts = New List(Of Account)
            mlngSplitCount = 0
            For intAcctIdx = 0 To lstAccounts.Items.Count - 1
                If lstAccounts.GetSelected(intAcctIdx) Then
                    objAcct = mobjCompany.Accounts.Item(intAcctIdx)
                    mcolSelectAccounts.Add(objAcct)
                    For Each objReg In objAcct.colRegisters
                        ScanRegister(objReg)
                    Next objReg
                End If
            Next
            lblProgress.Text = ""
            mobjHostUI.InfoMessageBox("Checked " & mlngSplitCount & " splits.")

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ScanRegister(ByVal objReg As Register)
        Dim datLastProgress As Date
        Dim objTrx As BaseTrx
        Dim datDate As Date
        Dim blnInclude As Boolean
        Dim objSplit As TrxSplit
        Dim strRegTitle As String

        Try

            strRegTitle = objReg.strTitle
            For Each objTrx In objReg.colDateRange(Of BaseTrx)(mdatStart, mdatEnd)
                With objTrx
                    datDate = .datDate
                    If datDate <> datLastProgress Then
                        lblProgress.Text = strRegTitle & "  " & Utilities.strFormatDate(datDate)
                        System.Windows.Forms.Application.DoEvents()
                        datLastProgress = datDate
                    End If
                    If .GetType() Is GetType(BankTrx) Then
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
                            For Each objSplit In DirectCast(objTrx, BankTrx).colSplits
                                mlngSplitCount = mlngSplitCount + 1
                                ProcessSplit(objReg, objTrx, objSplit)
                            Next objSplit
                        End If
                    End If
                End With
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ProcessSplit(ByVal objReg As Register, ByRef objTrx As BaseTrx, ByVal objSplit As TrxSplit)
        Dim intCatIndex As Integer

        Try

            Select Case mlngRptType
                Case SplitReportType.Totals
                    intCatIndex = mobjCompany.Categories.intLookupKey(objSplit.strCategoryKey)
                    If intCatIndex = 0 Then
                        mobjHostUI.InfoMessageBox("Could not find category key " & objSplit.strCategoryKey & " for " & "trx dated " & Utilities.strFormatDate(objTrx.datDate) & " " & "in register " & objReg.strTitle)
                    Else
                        With maudtCatTotals(intCatIndex)
                            .lngCount = .lngCount + 1
                            .curAmount = .curAmount + objSplit.curAmount
                        End With
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

            Select Case mlngRptType
                Case SplitReportType.Totals
                    frmSumRpt = New CatSumRptForm
                    frmSumRpt.ShowMe(mobjHostUI, maudtCatTotals, mcolSelectAccounts, mobjCompany.Categories, mdatStart, mdatEnd,
                                     mblnIncludeFake, mblnIncludeGenerated)
                Case Else
                    gRaiseError("Unrecognized category report type")
            End Select

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub
End Class
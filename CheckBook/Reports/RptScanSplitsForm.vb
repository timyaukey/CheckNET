Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class RptScanSplitsForm
    Inherits System.Windows.Forms.Form

    'Form to iterate transaction splits for selected accounts in a transaction date
    'range, and process them in some way typically a report or other summary.

    Public Enum SplitReportType
        glngSPLTRPT_TOTALS = 1
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
            gLoadAccountListBox(lstAccounts, mobjCompany)
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
            If mlngRptType = SplitReportType.glngSPLTRPT_TOTALS Then
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
                MsgBox("You must select at least one account.", MsgBoxStyle.Critical)
                Exit Function
            End If

            If Not Utilities.blnIsValidDate(txtStartDate.Text) Then
                MsgBox("Invalid transaction starting date.", MsgBoxStyle.Critical)
                Exit Function
            End If
            mdatStart = CDate(txtStartDate.Text)

            If Not Utilities.blnIsValidDate(txtEndDate.Text) Then
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
                    mstrCatKey = mobjCompany.objCategories.strKey(gintVB6GetItemData(cboCategory, cboCategory.SelectedIndex))
                End If
            End If

            If txtReportDate.Visible Then
                If Not Utilities.blnIsValidDate(txtReportDate.Text) Then
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

            ReDim maudtCatTotals(mobjCompany.objCategories.intElements)
            For intCatIndex = 1 To mobjCompany.objCategories.intElements
                strCatName = mobjCompany.objCategories.strValue1(intCatIndex)
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
                    objAcct = mobjCompany.colAccounts.Item(intAcctIdx)
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
        Dim datDate As Date
        Dim blnInclude As Boolean
        Dim objSplit As TrxSplit
        Dim strRegTitle As String

        Try

            strRegTitle = objReg.strTitle
            For Each objTrx In objReg.colDateRange(mdatStart, mdatEnd)
                With objTrx
                    datDate = .datDate
                    If datDate <> datLastProgress Then
                        lblProgress.Text = strRegTitle & "  " & Utilities.strFormatDate(datDate)
                        System.Windows.Forms.Application.DoEvents()
                        datLastProgress = datDate
                    End If
                    If .lngType = Trx.TrxType.Normal Then
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
                            For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
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

    Private Sub ProcessSplit(ByVal objReg As Register, ByRef objTrx As Trx, ByVal objSplit As TrxSplit)
        Dim intCatIndex As Integer

        Try

            Select Case mlngRptType
                Case SplitReportType.glngSPLTRPT_TOTALS
                    intCatIndex = mobjCompany.objCategories.intLookupKey(objSplit.strCategoryKey)
                    If intCatIndex = 0 Then
                        MsgBox("Could not find category key " & objSplit.strCategoryKey & " for " & "trx dated " & Utilities.strFormatDate(objTrx.datDate) & " " & "in register " & objReg.strTitle)
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
                Case SplitReportType.glngSPLTRPT_TOTALS
                    frmSumRpt = New CatSumRptForm
                    frmSumRpt.ShowMe(mobjCompany, maudtCatTotals, mcolSelectAccounts, mobjCompany.objCategories, mdatStart, mdatEnd,
                                     mblnIncludeFake, mblnIncludeGenerated, mobjHostUI)
                Case Else
                    gRaiseError("Unrecognized category report type")
            End Select

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub
End Class
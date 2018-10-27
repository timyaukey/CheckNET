Option Strict On
Option Explicit On

Imports CheckBookLib
Imports VB = Microsoft.VisualBasic

Friend Class TrxForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mobjAccount As Account
    Private mobjReg As Register
    Private mlngIndex As Integer
    Private mblnEditMode As Boolean
    Private mdatDefaultDate As Date
    Private mblnCancel As Boolean
    Private mlngType As Trx.TrxType
    Private mblnInSetNormalControls As Boolean
    Private mstrImportKey As String
    Private mintSplitCatKeyCode As Integer
    Private mintSplitBudgetKeyCode As Integer
    Private mblnBudgetAppliedVisible As Boolean
    Private mblnCheckInvoiceNum As Boolean
    Private mblnInDisplaySplits As Boolean
    Private mblnHasPlaceholderBudget As Boolean
    Private mblnSuppressPlaceholderAdjustment As Boolean
    Private mcurTotalAmount As Decimal
    Private mstrLogTitle As String
    'Values of existing Trx, if editing a Trx.
    Private mcurOldAmount As Decimal
    Private mlngOldStatus As Trx.TrxStatus
    Private mstrOldRepeatKey As String
    Private mintOldRepeatSeq As Integer

    Private cboSplitCategory() As ComboBox
    Private cboSplitBudget() As ComboBox
    Private lblSplitNumber() As Label
    Private chkChoose() As CheckBox
    Private txtSplitAmount() As TextBox
    Private txtSplitDueDate() As TextBox
    Private txtSplitInvoiceDate() As TextBox
    Private txtSplitInvoiceNum() As TextBox
    Private txtSplitMemo() As TextBox
    Private txtSplitPONum() As TextBox
    Private txtSplitTerms() As TextBox

    'Dim (Utilities.intLBOUND1 to mintSplits).
    Private maudtSplits() As SplitData
    Private mintSplits As Integer
    'Number of elements in maudtSplits() hidden above the top of the control array.
    Private mintSplitOffset As Integer
    Private Const mintSPLIT_CTRL_ARRAY_SIZE As Short = 10

    Private mblnSilentMode As Boolean

    'Definitions: "Shared" controls are those used exactly the same for all Trx types.
    '             "Normal" controls are those used for normal Trx type, unless they
    '             are "Shared". "Budget" and "Transfer" controls are the same for
    '             those Trx types. A control can have multiple types, so long as
    '             none of them is "Shared". A name in () means that control is hidden.
    '"Shared": date, description, memo, repeat key, awaiting review,
    '          auto generated
    '"Normal": number, status, fake, match range, import key
    '"Budget": (number), status, (fake), budget limit, budget ends, budget key
    '"Transfer": (number), status, fake, transfer to, transfer amount

    '$Description Enter a new normal Trx and add it to a Register.
    '$Returns True iff the operator cancelled.

    Public Function blnAddNormal(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As NormalTrx, ByRef datDefaultDate_ As Date, ByVal blnCheckInvoiceNum_ As Boolean, ByVal strLogTitle As String) As Boolean

        Try

            Init(objHostUI_, objTrx_.objReg, False, 0, Trx.TrxType.Normal, blnCheckInvoiceNum_, strLogTitle)
            mdatDefaultDate = datDefaultDate_
            ConfigSharedControls()
            SetSharedControls(objTrx_)
            ConfigNormalControls()
            SetNormalControls(objTrx_)
            Me.ShowDialog()
            If Not mblnCancel Then
                datDefaultDate_ = mdatDefaultDate
            End If
            blnAddNormal = mblnCancel

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Like blnAddNormal(), but only displays a UI if the data passed in
    '   has validation errors.
    '$Returns True iff the operator cancelled.

    Public Function blnAddNormalSilent(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As NormalTrx, ByRef datDefaultDate_ As Date, ByVal blnCheckInvoiceNum_ As Boolean, ByVal strLogTitle As String) As Boolean

        Try

            Init(objHostUI_, objTrx_.objReg, False, 0, Trx.TrxType.Normal, blnCheckInvoiceNum_, strLogTitle)
            mblnSilentMode = True
            mdatDefaultDate = datDefaultDate_
            ConfigSharedControls()
            SetSharedControls(objTrx_)
            ConfigNormalControls()
            SetNormalControls(objTrx_)
            If blnValidateAndSave() Then
                Me.Close()
            Else
                Me.ShowDialog()
            End If
            If Not mblnCancel Then
                datDefaultDate_ = mdatDefaultDate
            End If
            blnAddNormalSilent = mblnCancel

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Enter a new budget Trx and add it to a Register.
    '$Returns True iff the operator cancelled.

    Public Function blnAddBudget(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register, ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

        Try

            Init(objHostUI_, objReg_, False, 0, Trx.TrxType.Budget, False, strLogTitle)
            mdatDefaultDate = datDefaultDate_
            ConfigSharedControls()
            ClearSharedControls()
            ConfigBudgetControls()
            HideBudgetApplied()
            ClearBudgetControls()
            Me.ShowDialog()
            If Not mblnCancel Then
                datDefaultDate_ = mdatDefaultDate
            End If
            blnAddBudget = mblnCancel

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Enter a new transfer Trx and add it to a Register.
    '$Returns True iff the operator cancelled.

    Public Function blnAddTransfer(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register, ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

        Try

            Init(objHostUI_, objReg_, False, 0, Trx.TrxType.Transfer, False, strLogTitle)
            mdatDefaultDate = datDefaultDate_
            ConfigSharedControls()
            ClearSharedControls()
            ConfigTransferControls()
            ClearTransferControls()
            Me.ShowDialog()
            If Not mblnCancel Then
                datDefaultDate_ = mdatDefaultDate
            End If
            blnAddTransfer = mblnCancel

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Edit and existing Trx in the Register.
    '$Returns True iff the operator cancelled.

    Public Function blnUpdate(ByVal objHostUI_ As IHostUI, ByVal objTrx_ As Trx, ByRef datDefaultDate_ As Date, ByVal strLogTitle As String) As Boolean

        Try

            Init(objHostUI_, objTrx_.objReg, True, objTrx_.lngIndex, objTrx_.lngType, True, strLogTitle)
            ConfigSharedControls()
            SetSharedControls(objTrx_)
            mstrOldRepeatKey = objTrx_.strRepeatKey
            mintOldRepeatSeq = objTrx_.intRepeatSeq
            Select Case objTrx_.lngType
                Case Trx.TrxType.Normal
                    ConfigNormalControls()
                    SetNormalControls(DirectCast(objTrx_, NormalTrx))
                    CheckForPlaceholderBudget()
                    mcurOldAmount = objTrx_.curAmount
                    mlngOldStatus = objTrx_.lngStatus
                Case Trx.TrxType.Budget
                    ConfigBudgetControls()
                    SetBudgetControls(DirectCast(objTrx_, BudgetTrx))
                    ShowBudgetApplied(DirectCast(objTrx_, BudgetTrx))
                Case Trx.TrxType.Transfer
                    ConfigTransferControls()
                    SetTransferControls(DirectCast(objTrx_, TransferTrx))
            End Select
            Me.ShowDialog()
            If Not mblnCancel Then
                datDefaultDate_ = mdatDefaultDate
            End If
            blnUpdate = mblnCancel

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub ConfigSharedControls()
        UITools.LoadComboFromStringTranslator(cboRepeatKey, mobjAccount.objRepeats, True)
    End Sub

    Private Sub ClearSharedControls()
        txtDate.Text = Utilities.strFormatDate(mdatDefaultDate)
        txtDescription.Text = ""
        txtMemo.Text = ""
        cboRepeatKey.SelectedIndex = -1
        chkAwaitingReview.CheckState = System.Windows.Forms.CheckState.Unchecked
        chkAutoGenerated.CheckState = System.Windows.Forms.CheckState.Unchecked
        txtRepeatSeq.Text = ""
    End Sub

    Private Sub SetSharedControls(ByVal objTrx As Trx)
        With objTrx
            txtDate.Text = Utilities.strFormatDate(.datDate)
            txtDescription.Text = .strDescription
            txtMemo.Text = .strMemo
            SetComboFromStringTranslator(cboRepeatKey, mobjAccount.objRepeats, .strRepeatKey)
            chkAwaitingReview.CheckState = CType(IIf(.blnAwaitingReview, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked), CheckState)
            chkAutoGenerated.CheckState = CType(IIf(.blnAutoGenerated, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked), CheckState)
            If .intRepeatSeq <> 0 Then
                txtRepeatSeq.Text = CStr(.intRepeatSeq)
            End If
        End With
    End Sub

    Private Sub ConfigNormalControls()
        Dim intIndex As Short
        cmdPrintCheck.Visible = True
        cmdMailingAddress.Visible = True
        txtNumber.Visible = True
        cmdCopyInvoiceNumbers.Visible = True
        EnableStatus()
        chkFake.Visible = True
        For intIndex = 0 To mintSPLIT_CTRL_ARRAY_SIZE - 1
            UITools.LoadComboFromStringTranslator(cboSplitCategory(intIndex), mobjCompany.objCategories, True)
            UITools.LoadComboFromStringTranslator(cboSplitBudget(intIndex), mobjCompany.objBudgets, True)
        Next
        ShowFrame(frmNormal)
    End Sub

    'Private Sub ClearNormalControls()
    '    txtNumber = ""
    '    cboStatus.ListIndex = 0
    '    chkFake.Value = vbUnchecked
    '    txtMatchRange = "0.00"
    '    chkImported.Value = Unchecked
    '    LoadSplits New Collection
    '    DisplaySplits 0
    'End Sub

    Private Sub SetNormalControls(ByVal objTrx As NormalTrx)
        Try

            mblnInSetNormalControls = True
            With objTrx
                txtNumber.Text = .strNumber
                cboStatus.SelectedIndex = CInt(IIf(.lngStatus = Trx.TrxStatus.Unreconciled, 0, IIf(.lngStatus = Trx.TrxStatus.Selected, 1, 2)))
                chkFake.CheckState = CType(IIf(.blnFake, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked), CheckState)
                txtMatchRange.Text = Utilities.strFormatCurrency(.curNormalMatchRange)
                mstrImportKey = .strImportKey
                SetImportKeyControls((mstrImportKey <> ""))
                chkImported.Enabled = (mstrImportKey <> "")
                LoadSplits(.colSplits)
                DisplaySplits(0)
            End With
            mblnInSetNormalControls = False

            Exit Sub
        Catch ex As Exception
            mblnInSetNormalControls = False
            gNestedException(ex)
        End Try
    End Sub

    Private Sub CheckForPlaceholderBudget()
        Dim intIndex As Integer
        mblnHasPlaceholderBudget = False
        For intIndex = 1 To mintSplits
            If maudtSplits(intIndex).strBudgetKey = mobjCompany.strPlaceholderBudgetKey Then
                mblnHasPlaceholderBudget = True
                Exit Sub

            End If
        Next
    End Sub

    Private Sub SetImportKeyControls(ByVal blnChecked As Boolean)
        If blnChecked Then
            chkImported.CheckState = System.Windows.Forms.CheckState.Checked
            lblImportKey.Text = Replace(mstrImportKey, "&", "&&")
        Else
            chkImported.CheckState = System.Windows.Forms.CheckState.Unchecked
            lblImportKey.Text = ""
        End If
    End Sub

    Private Sub ConfigBudgetControls()
        cmdPrintCheck.Visible = False
        cmdMailingAddress.Visible = False
        txtNumber.Visible = False
        lblNumber.Visible = False
        cboStatus.Visible = False
        lblStatus.Visible = False
        lblBudgetStarts.Visible = True
        txtBudgetStarts.Visible = True
        chkFake.Visible = False
        cmdCopyInvoiceNumbers.Visible = False
        UITools.LoadComboFromStringTranslator(cboBudgetName, mobjCompany.objBudgets, True)
        ShowFrame(frmBudget)
    End Sub

    Private Sub ClearBudgetControls()
        txtBudgetLimit.Text = "0.00"
        cboBudgetName.SelectedIndex = -1
        txtBudgetApplied.Text = ""
        txtBudgetStarts.Text = ""
    End Sub

    Private Sub SetBudgetControls(ByVal objTrx As BudgetTrx)
        With objTrx
            txtBudgetLimit.Text = Utilities.strFormatCurrency(.curBudgetLimit)
            SetComboFromStringTranslator(cboBudgetName, mobjCompany.objBudgets, .strBudgetKey)
            txtBudgetApplied.Text = Utilities.strFormatCurrency(.curBudgetApplied)
            txtBudgetStarts.Text = Utilities.strFormatDate(.datBudgetStarts)
        End With
    End Sub

    Private Sub ShowBudgetApplied(ByVal objBudget As BudgetTrx)
        Dim lngCurrent As Integer
        Dim objCurrent As Trx
        Dim objSplit As TrxSplit
        Dim objItem As System.Windows.Forms.ListViewItem
        Dim curTotalApplied As Decimal

        lvwAppliedTo.Visible = True
        mblnBudgetAppliedVisible = True

        'Set lngCurrent to the index of the earliest Trx to check.
        lngCurrent = objBudget.lngEarliestPossibleAppliedIndex()

        'Check all Trx until we find one dated after the budget end date.
        Do
            If lngCurrent > mobjReg.lngTrxCount Then
                Exit Do
            End If
            objCurrent = mobjReg.objTrx(lngCurrent)
            If objCurrent.datDate > objBudget.datBudgetEnds Then
                Exit Do
            End If
            If objCurrent.lngType = Trx.TrxType.Normal Then
                For Each objSplit In DirectCast(objCurrent, NormalTrx).colSplits
                    If objSplit.objBudget Is objBudget Then
                        'Show it.
                        With objCurrent
                            objItem = UITools.ListViewAdd(lvwAppliedTo)
                            objItem.Text = Utilities.strFormatDate(.datDate)
                            objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, .strNumber))
                            objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, .strDescription))
                            objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objSplit.strInvoiceNum))
                            objItem.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objSplit.strPONumber))
                            objItem.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Utilities.strFormatCurrency(objSplit.curAmount)))
                            curTotalApplied = curTotalApplied + objSplit.curAmount
                            objItem.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing,
                                mobjCompany.objCategories.strTranslateKey(objSplit.strCategoryKey)))
                        End With
                    End If
                Next objSplit
            End If
            lngCurrent = lngCurrent + 1
        Loop

        If curTotalApplied <> objBudget.curBudgetApplied Then
            mobjHostUI.ErrorMessageBox("ERROR: Amount applied in budget trx does not equal sum " & "of matched splits (" & curTotalApplied & " vs " & objBudget.curBudgetApplied & ").")
        End If

    End Sub

    Private Sub HideBudgetApplied()
        If mblnBudgetAppliedVisible Then
            lvwAppliedTo.Visible = False
            mblnBudgetAppliedVisible = False
        End If
    End Sub

    Private Sub ConfigTransferControls()
        cmdPrintCheck.Visible = False
        cmdMailingAddress.Visible = False
        txtNumber.Visible = False
        lblNumber.Visible = False
        cboStatus.Visible = False
        lblStatus.Visible = False
        chkFake.Visible = True
        cmdCopyInvoiceNumbers.Visible = False
        UITools.LoadComboFromStringTranslator(cboTransferTo, mobjAccount.objRegisterList(), True)
        cboTransferTo.Enabled = Not mblnEditMode
        ShowFrame(frmTransfer)
    End Sub

    Private Sub ClearTransferControls()
        chkFake.CheckState = System.Windows.Forms.CheckState.Unchecked
        cboTransferTo.SelectedIndex = -1
        txtTransferAmount.Text = ""
    End Sub

    Private Sub SetTransferControls(ByVal objTrx As TransferTrx)
        With objTrx
            chkFake.CheckState = CType(IIf(.blnFake, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked), CheckState)
            SetComboFromStringTranslator(cboTransferTo, mobjAccount.objRegisterList(), .strTransferKey)
            txtTransferAmount.Text = Utilities.strFormatCurrency(.curTransferAmount)
        End With
    End Sub

    Private Sub Init(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register, ByVal blnEditMode_ As Boolean,
                     ByVal lngIndex_ As Integer, ByVal lngType_ As Trx.TrxType, ByVal blnCheckInvoiceNum_ As Boolean, ByVal strLogTitle As String)

        cboSplitCategory = {_cboSplitCategory_0, _cboSplitCategory_1, _cboSplitCategory_2, _cboSplitCategory_3, _cboSplitCategory_4, _cboSplitCategory_5, _cboSplitCategory_6, _cboSplitCategory_7, _cboSplitCategory_8, _cboSplitCategory_9}
        cboSplitBudget = {_cboSplitBudget_0, _cboSplitBudget_1, _cboSplitBudget_2, _cboSplitBudget_3, _cboSplitBudget_4, _cboSplitBudget_5, _cboSplitBudget_6, _cboSplitBudget_7, _cboSplitBudget_8, _cboSplitBudget_9}
        lblSplitNumber = {_lblSplitNumber_0, _lblSplitNumber_1, _lblSplitNumber_2, _lblSplitNumber_3, _lblSplitNumber_4, _lblSplitNumber_5, _lblSplitNumber_6, _lblSplitNumber_7, _lblSplitNumber_8, _lblSplitNumber_9}
        chkChoose = {_chkChoose_0, _chkChoose_1, _chkChoose_2, _chkChoose_3, _chkChoose_4, _chkChoose_5, _chkChoose_6, _chkChoose_7, _chkChoose_8, _chkChoose_9}
        txtSplitAmount = {_txtSplitAmount_0, _txtSplitAmount_1, _txtSplitAmount_2, _txtSplitAmount_3, _txtSplitAmount_4, _txtSplitAmount_5, _txtSplitAmount_6, _txtSplitAmount_7, _txtSplitAmount_8, _txtSplitAmount_9}
        txtSplitDueDate = {_txtSplitDueDate_0, _txtSplitDueDate_1, _txtSplitDueDate_2, _txtSplitDueDate_3, _txtSplitDueDate_4, _txtSplitDueDate_5, _txtSplitDueDate_6, _txtSplitDueDate_7, _txtSplitDueDate_8, _txtSplitDueDate_9}
        txtSplitInvoiceDate = {_txtSplitInvoiceDate_0, _txtSplitInvoiceDate_1, _txtSplitInvoiceDate_2, _txtSplitInvoiceDate_3, _txtSplitInvoiceDate_4, _txtSplitInvoiceDate_5, _txtSplitInvoiceDate_6, _txtSplitInvoiceDate_7, _txtSplitInvoiceDate_8, _txtSplitInvoiceDate_9}
        txtSplitInvoiceNum = {_txtSplitInvoiceNum_0, _txtSplitInvoiceNum_1, _txtSplitInvoiceNum_2, _txtSplitInvoiceNum_3, _txtSplitInvoiceNum_4, _txtSplitInvoiceNum_5, _txtSplitInvoiceNum_6, _txtSplitInvoiceNum_7, _txtSplitInvoiceNum_8, _txtSplitInvoiceNum_9}
        txtSplitMemo = {_txtSplitMemo_0, _txtSplitMemo_1, _txtSplitMemo_2, _txtSplitMemo_3, _txtSplitMemo_4, _txtSplitMemo_5, _txtSplitMemo_6, _txtSplitMemo_7, _txtSplitMemo_8, _txtSplitMemo_9}
        txtSplitPONum = {_txtSplitPONum_0, _txtSplitPONum_1, _txtSplitPONum_2, _txtSplitPONum_3, _txtSplitPONum_4, _txtSplitPONum_5, _txtSplitPONum_6, _txtSplitPONum_7, _txtSplitPONum_8, _txtSplitPONum_9}
        txtSplitTerms = {_txtSplitTerms_0, _txtSplitTerms_1, _txtSplitTerms_2, _txtSplitTerms_3, _txtSplitTerms_4, _txtSplitTerms_5, _txtSplitTerms_6, _txtSplitTerms_7, _txtSplitTerms_8, _txtSplitTerms_9}

        mobjHostUI = objHostUI_
        mobjReg = objReg_
        mobjAccount = mobjReg.objAccount
        mobjCompany = mobjHostUI.objCompany
        mblnEditMode = blnEditMode_
        mlngIndex = lngIndex_
        mlngType = lngType_
        mblnCancel = True
        mblnBudgetAppliedVisible = True
        mblnCheckInvoiceNum = blnCheckInvoiceNum_
        mblnHasPlaceholderBudget = False
        mcurTotalAmount = 0
        mstrLogTitle = strLogTitle

    End Sub

    Private Sub ShowFrame(ByVal ctlFrame As System.Windows.Forms.GroupBox)
        ctlFrame.Left = frmNormal.Left
        ctlFrame.Top = frmNormal.Top
        ctlFrame.Visible = True
    End Sub

    Private Sub EnableStatus()
        With cboStatus
            .Enabled = True
            .Items.Clear()
            .Items.Add("Unreconciled")
            .Items.Add("Selected")
            .Items.Add("Reconciled")
        End With
    End Sub

    Private Sub SetComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As IStringTranslator, ByVal strKey As String)
        Dim strValue1 As String
        Dim intIndex As Integer
        'Apparently you cannot assign a zero length string to a combo box
        'even if there is an empty element in the list.
        strValue1 = objList.strKeyToValue1(strKey)
        If strValue1 = "" Then
            cbo.SelectedIndex = -1
            'In case there is actually a blank element in the list.
            For intIndex = 0 To cbo.Items.Count - 1
                If UITools.GetItemString(cbo, intIndex) = strValue1 Then
                    cbo.SelectedIndex = intIndex
                    Exit For
                End If
            Next
        Else
            'cbo.Text = strValue1
            For intIndex = 0 To cbo.Items.Count - 1
                If UITools.GetItemString(cbo, intIndex) = strValue1 Then
                    cbo.SelectedIndex = intIndex
                    Exit For
                End If
            Next
        End If
    End Sub

    '$Description Load a collection of Split objects into maudtSplits().

    Private Sub LoadSplits(ByVal colSplits As IEnumerable(Of TrxSplit))
        Dim objSplit As TrxSplit
        Dim intIndex As Integer

        mintSplits = colSplits.Count()
        If mintSplits = 0 Then
            mintSplits = 1
        End If
        ReDim maudtSplits(mintSplits)
        FillSplitDataArray()
        intIndex = 0
        For Each objSplit In colSplits
            intIndex = intIndex + 1
            With maudtSplits(intIndex)
                .strMemo = objSplit.strMemo
                .strCategoryKey = objSplit.strCategoryKey
                .strPONumber = objSplit.strPONumber
                .strInvoiceNum = objSplit.strInvoiceNum
                If objSplit.datInvoiceDate = System.DateTime.FromOADate(0) Then
                    .strInvoiceDate = ""
                Else
                    .strInvoiceDate = Utilities.strFormatDate(objSplit.datInvoiceDate)
                End If
                If objSplit.datDueDate = System.DateTime.FromOADate(0) Then
                    .strDueDate = ""
                Else
                    .strDueDate = Utilities.strFormatDate(objSplit.datDueDate)
                End If
                .strTerms = objSplit.strTerms
                .strBudgetKey = objSplit.strBudgetKey
                .strAmount = Utilities.strFormatCurrency(objSplit.curAmount)
            End With
        Next objSplit
    End Sub

    Private Sub FillSplitDataArray()
        Dim intIndex As Integer
        For intIndex = 1 To mintSplits
            If maudtSplits(intIndex) Is Nothing Then
                maudtSplits(intIndex) = New SplitData
            End If
        Next
    End Sub

    '$Description Display maudtSplits() in the control arrays using the specified
    '   offset for the control array indices. Extends maudtSplits() if it does
    '   not have enough elements to cover all the control array elements using
    '   that offset. Sets mintSplitOffset to the specified offset.

    Private Sub DisplaySplits(ByVal intNewOffset As Integer)
        Dim intIndex As Integer

        Try

            mblnInDisplaySplits = True
            mintSplitOffset = intNewOffset
            If mintSplitOffset + mintSPLIT_CTRL_ARRAY_SIZE > mintSplits Then
                mintSplits = mintSplitOffset + mintSPLIT_CTRL_ARRAY_SIZE
                ReDim Preserve maudtSplits(mintSplits)
                FillSplitDataArray()
            End If
            For intIndex = 0 To mintSPLIT_CTRL_ARRAY_SIZE - 1
                With maudtSplits(intIndex + mintSplitOffset + 1)
                    lblSplitNumber(intIndex).Text = Utilities.strFormatInteger(intIndex + 1 + mintSplitOffset, "##0") & "."
                    SetComboFromStringTranslator(cboSplitCategory(intIndex), mobjCompany.objCategories, .strCategoryKey)
                    txtSplitPONum(intIndex).Text = .strPONumber
                    txtSplitInvoiceNum(intIndex).Text = .strInvoiceNum
                    txtSplitInvoiceDate(intIndex).Text = .strInvoiceDate
                    txtSplitDueDate(intIndex).Text = .strDueDate
                    txtSplitTerms(intIndex).Text = .strTerms
                    SetComboFromStringTranslator(cboSplitBudget(intIndex), mobjCompany.objBudgets, .strBudgetKey)
                    txtSplitMemo(intIndex).Text = .strMemo
                    mblnSuppressPlaceholderAdjustment = True
                    txtSplitAmount(intIndex).Text = .strAmount
                    mblnSuppressPlaceholderAdjustment = False
                    If .blnChoose Then
                        chkChoose(intIndex).CheckState = System.Windows.Forms.CheckState.Checked
                    Else
                        chkChoose(intIndex).CheckState = System.Windows.Forms.CheckState.Unchecked
                    End If
                End With
            Next
            mblnInDisplaySplits = False
            DisplaySplitTotal()

            Exit Sub
        Catch ex As Exception
            mblnInDisplaySplits = False
            gNestedException(ex)
        End Try
    End Sub

    '$Description Display the total of all splits in txtSplitTotal, or
    '   gstrUNABLE_TO_TRANSLATE if any of the split amounts are non-blank
    '   and non-numeric.

    Private Sub DisplaySplitTotal()
        Dim curTotal As Decimal
        Dim blnBadAmount As Boolean
        ComputeSplitTotal(curTotal, blnBadAmount)
        If blnBadAmount Then
            'Do NOT set mcurTotalAmount if cannot compute trx total.
            txtSplitTotal.Text = "???"
        Else
            txtSplitTotal.Text = Utilities.strFormatCurrency(curTotal)
            mcurTotalAmount = curTotal
        End If
    End Sub

    '$Description Compute the current total of all splits, without displaying
    '   it or setting mcurTotalAmount.

    Private Sub ComputeSplitTotal(ByRef curTotal As Decimal, ByRef blnBadAmount As Boolean)
        Dim intIndex As Integer
        Dim strAmount As String

        curTotal = 0
        blnBadAmount = False

        For intIndex = 1 To mintSplits
            strAmount = maudtSplits(intIndex).strAmount
            If Len(strAmount) > 0 Then
                If IsNumeric(strAmount) Then
                    curTotal = curTotal + CDec(strAmount)
                Else
                    blnBadAmount = True
                    Exit For
                End If
            End If
        Next
    End Sub

    '$Description Determine if placeholder budget split(s) must be adjusted
    '   if the amount of the specified split is changed.

    Private Function blnPlaceholderAdjustmentRequired(ByRef intSplitIndex As Integer) As Boolean
        If mblnHasPlaceholderBudget And maudtSplits(intSplitIndex).strBudgetKey <> mobjCompany.strPlaceholderBudgetKey Then
            blnPlaceholderAdjustmentRequired = True
        Else
            blnPlaceholderAdjustmentRequired = False
        End If
    End Function

    '$Description Adjust the amount of one or more splits with placeholder budget
    '   keys to make the trx total equal to mcurTotalAmount. This is used to keep
    '   the trx total from changing when non-placeholder splits are modified in
    '   a trx that has placeholder splits.
    '   For example, when a non-placeholder split is increased that takes money
    '   from a placeholder split (like a budget being used). There are many ways
    '   to decide what the splits should be, but we chose a very simple one: Set
    '   the first placeholder split to the total placeholder amount and set any
    '   additional placeholder splits to zero.

    Private Sub FindAndAdjustPlaceholderBudgets()
        Dim intSplitIndex As Integer
        Dim curNonPlaceholderTotal As Decimal
        Dim intPlaceholders As Integer
        Dim intPlaceholderIndexes() As Integer
        Dim intPlaceholderSelector As Integer
        Dim strSplitAmount As String
        Dim intControlIndex As Integer

        ReDim intPlaceholderIndexes(1)
        'Scan splits to find placeholders, and add up non-placeholders.
        For intSplitIndex = 1 To mintSplits
            If maudtSplits(intSplitIndex).strBudgetKey = mobjCompany.strPlaceholderBudgetKey Then
                intPlaceholders = intPlaceholders + 1
                ReDim Preserve intPlaceholderIndexes(intPlaceholders)
                intPlaceholderIndexes(intPlaceholders) = intSplitIndex
            Else
                strSplitAmount = maudtSplits(intSplitIndex).strAmount
                If Len(strSplitAmount) > 0 Then
                    If IsNumeric(strSplitAmount) Then
                        curNonPlaceholderTotal = curNonPlaceholderTotal + CDec(strSplitAmount)
                    Else
                        Exit Sub
                    End If
                End If
            End If
        Next
        'If we were able to compute the total (no splits with non-numeric amounts).
        If intPlaceholders > 0 Then
            'Set all placeholders
            mblnSuppressPlaceholderAdjustment = True
            For intPlaceholderSelector = Utilities.intLBOUND1 To UBound(intPlaceholderIndexes)
                intSplitIndex = intPlaceholderIndexes(intPlaceholderSelector)
                If intPlaceholderSelector = Utilities.intLBOUND1 Then
                    strSplitAmount = Utilities.strFormatCurrency(mcurTotalAmount - curNonPlaceholderTotal)
                Else
                    strSplitAmount = "0.00"
                End If
                maudtSplits(intSplitIndex).strAmount = strSplitAmount
                intControlIndex = intSplitIndex - mintSplitOffset - 1
                If intControlIndex >= 0 And intControlIndex < mintSPLIT_CTRL_ARRAY_SIZE Then
                    txtSplitAmount(intControlIndex).Text = maudtSplits(intSplitIndex).strAmount
                End If
            Next
            mblnSuppressPlaceholderAdjustment = False
        End If
    End Sub

    'I do not know why the SearchInComboBoxChange() does not work for this
    'combo box. The style was set correctly, and in all other ways I could find
    'it was identical to the category and budget combo boxes, except that it is
    'not a control array. I tried tab stop even.

    'Private Sub cboRepeatKey_Change()
    '    Try
    '    SearchInComboBoxChange cboRepeatKey, mintRepeatKeyCode
    '    Exit Sub
    'Catch ex As Exception
    '    TopError "cboSplitRepeatKey_Change"
    'End Sub

    'Private Sub cboRepeatKey_Click()
    '    Dim strValue As String
    '    strValue = strGetStringTranslatorKeyFromCombo(cboRepeatKey, _
    ''        mobjAccount.objRepeats)
    'End Sub

    'Private Sub cboRepeatKey_KeyDown(KeyCode As Integer, Shift As Integer)
    '    mintRepeatKeyCode = KeyCode
    'End Sub

    Private Sub SearchInComboBoxChange(ByVal cbo As System.Windows.Forms.ComboBox, ByVal intKeyCode As Integer)

        Dim intListIndex As Integer
        Dim intListMax As Integer
        Dim strText As String
        Dim intTextLen As Integer

        'For this to work cbo.Style must be zero (dropdown combo).

        If intKeyCode = System.Windows.Forms.Keys.Back Or intKeyCode = System.Windows.Forms.Keys.Delete Then
            Exit Sub
        End If

        strText = UCase(cbo.Text)
        intTextLen = Len(strText)
        intListMax = cbo.Items.Count - 1

        For intListIndex = 0 To intListMax
            If UCase(VB.Left(UITools.GetItemString(cbo, intListIndex), intTextLen)) = strText Then
                'This fires the Click event, which fills in the full match.
                'Strangely, the value assigned to cbo.ListIndex seems to change to
                '-1 after the event handler exits, for reasons I don't understand.
                cbo.SelectedIndex = intListIndex
                'We then select the part after what we typed, so if we continue
                'to type it will add to the end of what we typed before.
                cbo.SelectionStart = intTextLen
                cbo.SelectionLength = Len(cbo.Text) - intTextLen
                Exit For
            End If
        Next
    End Sub

    Private Sub chkImported_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkImported.CheckStateChanged
        Try

            SetImportKeyControls((chkImported.CheckState = System.Windows.Forms.CheckState.Checked))

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdCopyInvoiceNumbers_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopyInvoiceNumbers.Click
        Dim intSplit As Integer
        Dim strNumbers As String = ""

        For intSplit = 1 To mintSplits
            If blnSplitUsed(intSplit) Then
                If maudtSplits(intSplit).strInvoiceNum <> "" Then
                    strNumbers = strNumbers & " " & maudtSplits(intSplit).strInvoiceNum
                End If
            End If
        Next
        strNumbers = Trim(strNumbers)
        My.Computer.Clipboard.Clear()
        My.Computer.Clipboard.SetText(strNumbers)
    End Sub

    Private Sub cmdCopyAmount_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopyAmount.Click
        Dim strAmount As String = ""
        If mlngType = Trx.TrxType.Normal Then
            strAmount = txtSplitTotal.Text
        ElseIf mlngType = Trx.TrxType.Budget Then
            strAmount = txtBudgetLimit.Text
        ElseIf mlngType = Trx.TrxType.Transfer Then
            strAmount = txtTransferAmount.Text
        End If
        My.Computer.Clipboard.Clear()
        My.Computer.Clipboard.SetText(Utilities.strFormatCurrency(System.Math.Abs(CDec(strAmount))))
    End Sub

    Private Sub cmdCopyDate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopyDate.Click
        My.Computer.Clipboard.Clear()
        My.Computer.Clipboard.SetText(CDate(txtDate.Text).ToString("MM/dd/yyyy"))
    End Sub

    Private Sub cmdMailingAddress_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMailingAddress.Click
        Try

            Dim colPayees As VB6XmlNodeList
            Dim objPayee As VB6XmlElement
            Dim strMsg As String
            Dim strPayee As String
            Dim strAddress2 As String

            colPayees = mobjCompany.colFindPayeeMatches(txtDescription.Text)
            If colPayees.Length = 0 Then
                strMsg = "No matching memorized transactions."
            Else
                strMsg = "Matching Memorized Transaction(s):"
                For Each objPayee In colPayees
                    strPayee = CStr(objPayee.GetAttribute("Output")) & vbCrLf & gstrGetXMLChildText(objPayee, "Address1")
                    strAddress2 = gstrGetXMLChildText(objPayee, "Address2")
                    If Len(strAddress2) > 0 Then
                        strPayee = strPayee & vbCrLf & strAddress2
                    End If
                    strPayee = strPayee & vbCrLf & gstrGetXMLChildText(objPayee, "City") & " " & gstrGetXMLChildText(objPayee, "State") & " " & gstrGetXMLChildText(objPayee, "Zip") & vbCrLf & "Account #: " & gstrGetXMLChildText(objPayee, "Account")
                    strMsg = strMsg & vbCrLf & vbCrLf & strPayee
                Next objPayee
            End If
            MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Matching Payees")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            If blnValidateAndSave() Then
                Me.Close()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    '$Description Validate the data on the form, and add or update the Trx
    '   as appropriate if no errors were found.
    '$Returns True iff there were no errors and the Trx was saved.

    Private Function blnValidateAndSave() As Boolean
        Dim strOtherRegisterKey As String = ""
        If blnValidateShared() Then
            Exit Function
        End If
        Select Case mlngType
            Case Trx.TrxType.Normal
                If blnValidateNormal() Then
                    Exit Function
                End If
                SaveNormal()
            Case Trx.TrxType.Budget
                If blnValidateBudget() Then
                    Exit Function
                End If
                SaveBudget()
            Case Trx.TrxType.Transfer
                If blnValidateTransfer(strOtherRegisterKey) Then
                    Exit Function
                End If
                SaveTransfer(strOtherRegisterKey)
        End Select
        blnValidateAndSave = True
        mblnCancel = False
    End Function

    '$Description Validate shared controls. Displays a message if any errors
    '   detected.
    '$Returns True iff any errors detected.

    Private Function blnValidateShared() As Boolean
        Dim objMatchingTrx As Trx
        Dim intRepeatSeq As Integer

        blnValidateShared = True
        If Not Utilities.blnIsValidDate(txtDate.Text) Then
            ValidationError("Invalid transaction date.")
            Exit Function
        End If
        mdatDefaultDate = CDate(txtDate.Text)
        If txtDescription.Text = "" Then
            ValidationError("Description is required.")
            Exit Function
        End If
        If chkAutoGenerated.CheckState = System.Windows.Forms.CheckState.Checked Then
            ValidationError("You may not save a transaction whose ""Generated"" " & "box is checked, because this is reserved for automatically generated " & "transactions that are recreated each time the software starts." & vbCrLf & vbCrLf & "To save it you must uncheck ""Generated"", which makes it " & "a permanent part of the register instead of regenerating it " & "every time the software starts.")
            Exit Function
        End If
        Dim blnKeySeqInUse As Boolean
        If Trim(txtRepeatSeq.Text) <> "" Then
            If strRepeatKey() = "" Then
                ValidationError("Repeat sequence number not allowed without repeat key.")
                Exit Function
            End If
            If Not IsNumeric(txtRepeatSeq.Text) Then
                ValidationError("Invalid repeat sequence number.")
                Exit Function
            End If
            intRepeatSeq = CInt(Val(txtRepeatSeq.Text))
            If intRepeatSeq <= 0 Then
                ValidationError("Repeat sequence number must be greater than zero.")
                Exit Function
            End If
            objMatchingTrx = mobjReg.objRepeatTrx(strRepeatKey(), intRepeatSeq)
            If Not objMatchingTrx Is Nothing Then
                If mblnEditMode Then
                    blnKeySeqInUse = (Not objMatchingTrx Is mobjReg.objTrx(mlngIndex))
                Else
                    blnKeySeqInUse = True
                End If
                If blnKeySeqInUse Then
                    ValidationError("This repeat key and sequence number is already used by " & "another transaction.")
                    Exit Function
                End If
            End If
            If intRepeatSeq <> mintOldRepeatSeq And mintOldRepeatSeq > 0 Then
                If MsgBox("You have changed the repeat sequence number of this transaction. " &
                          "This will usually cause another transaction to be generated " &
                          "with the old sequence number, because the software will think the " &
                          "old transaction is missing." & vbCrLf & vbCrLf &
                          "It is generally bad to change the repeat sequence number unless you also " &
                          "create or change another transaction to use the old sequence " &
                          "number so there is no ""gap"" in the sequence numbers. " &
                          "It is usually better to set the amount to zero if you simply don't " &
                          "want to use that transaction." & vbCrLf & vbCrLf &
                          "Do you really want to change the sequence number?",
                          MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation,
                          "Repeat Sequence Warning") <> MsgBoxResult.Ok Then
                    Exit Function
                End If
            End If
        Else
            If strRepeatKey() <> "" Then
                ValidationError("Repeat key not allowed without repeat sequence number.")
                Exit Function
            End If
        End If
        If strRepeatKey() <> mstrOldRepeatKey And mblnEditMode Then
            If MsgBox("You have changed the repeat key of this transaction. This will usually " &
                      "cause another transaction to be generated with the old repeat " &
                      "key, because the software will think the old transaction is missing." & vbCrLf & vbCrLf &
                      "It is usually better to set the amount " &
                      "to zero than to change the repeat key or delete the transaction." & vbCrLf & vbCrLf &
                      "Do you really want to change the repeat key?",
                      MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation,
                      "Repeat Key Warning") <> MsgBoxResult.Ok Then
                Exit Function
            End If
        End If
        blnValidateShared = False
    End Function

    Private Function blnInvalidTrxDateVersusToday() As Boolean
        'blnInvalidTrxDateVersusToday = True
        'If Not mblnEditMode Then
        '    If blnInvalidDate(CDate(txtDate), Date, 3, 5, "Transaction date", "today's date") Then
        '        Exit Function
        '    End If
        'End If
        blnInvalidTrxDateVersusToday = False
    End Function

    '$Description See blnValidateShared().

    Private Function blnValidateNormal() As Boolean
        Dim intSplit As Integer
        Dim curAmount As Decimal
        Dim curNewTotal As Decimal
        Dim intSplitsUsed As Integer
        Dim strTrxNumLc As String
        Dim datInvoiceDate As Date
        Dim datDueDate As Date
        Dim intNetPos As Integer
        Dim intDaysUntilDue As Integer

        blnValidateNormal = True
        If Not IsNumeric(txtMatchRange.Text) Then
            ValidationError("Match range must be positive or zero.")
            Exit Function
        End If
        If CDec(txtMatchRange.Text) < 0.0# Then
            ValidationError("Match range must be positive or zero.")
            Exit Function
        End If
        If Trim(txtNumber.Text) = "" Then
            ValidationError("Number is required. Suggestion: " & "use something like ""Card"" for debit/credit card purchases, " & """Pmt"" for payments made through online banking at " & "your bank, etc.")
            Exit Function
        End If
        If blnIsDuplicateTrx() Then
            Exit Function
        End If
        If IsNumeric(txtNumber.Text) Then
            If chkFake.CheckState = System.Windows.Forms.CheckState.Checked Then
                ValidationError("Transactions with a check number may not be fake.")
                Exit Function
            End If
        Else
            strTrxNumLc = LCase(txtNumber.Text)
            If strTrxNumLc <> "inv" And strTrxNumLc <> "pmt" And strTrxNumLc <> "dep" And strTrxNumLc <> "eft" And strTrxNumLc <> "xfr" And strTrxNumLc <> "card" And strTrxNumLc <> "crm" And strTrxNumLc <> "ord" Then
                ValidationError("Number must be a check number or one of the " & "following: ""dep"" (deposit), ""card"" (credit or debit card), " & """pmt"" (payment), ""ord"" (order), ""inv"" (invoice), " & """crm"" (credit memo), " & """eft"" (electronic funds transfer), ""xfr"" (transfer). " & "Any combination of upper and lower case is allowed, " & "like ""card"", ""Card"" or ""CARD"". ")
                Exit Function
            End If
            If strTrxNumLc = "inv" Or strTrxNumLc = "crm" Or strTrxNumLc = "ord" Then
                If chkFake.CheckState <> System.Windows.Forms.CheckState.Checked Then
                    ValidationError("Orders, invoices and credit memos must be also be fake.")
                    Exit Function
                End If
            End If
        End If
        If chkFake.CheckState = System.Windows.Forms.CheckState.Checked Then
            If lngTrxStatus() <> Trx.TrxStatus.Unreconciled Then
                ValidationError("Fake transactions must be unreconciled.")
                Exit Function
            End If
            If chkImported.CheckState = System.Windows.Forms.CheckState.Checked Then
                ValidationError("Fake transactions may not be marked as imported.")
                Exit Function
            End If
        Else
            If mblnHasPlaceholderBudget Then
                ValidationError("Transactions with placeholder splits must be fake.")
                Exit Function
            End If
        End If
        Dim blnAccountIsPersonal As Boolean = (mobjReg.objAccount.lngType = Account.AccountType.Personal)
        For intSplit = 1 To mintSplits
            If blnSplitUsed(intSplit) Then
                intSplitsUsed = intSplitsUsed + 1
                With maudtSplits(intSplit)
                    If .strCategoryKey = "" Then
                        ValidationError("Split #" & intSplit & " has no category.")
                        Exit Function
                    Else
                        Dim intDotOffset As Integer = .strCategoryKey.IndexOf("."c)
                        If intDotOffset > 0 Then
                            Dim intAccountKey As Integer = Integer.Parse(.strCategoryKey.Substring(0, intDotOffset))
                            If intAccountKey = mobjReg.objAccount.intKey Then
                                ValidationError("Split category uses the same account")
                                Exit Function
                            End If
                        Else
                            If blnAccountIsPersonal <> CategoryTranslator.blnIsPersonal(mobjCompany.objCategories.strKeyToValue1(.strCategoryKey)) Then
                                ValidationError("Personal category may not be used in a non-personal account, or visa versa")
                                Exit Function
                            End If
                        End If
                    End If
                    If .strInvoiceDate <> "" Then
                        If Not Utilities.blnIsValidDate(.strInvoiceDate) Then
                            ValidationError("Split #" & intSplit & " has an invalid invoice date.")
                            Exit Function
                        End If
                        'If Not mblnEditMode Then
                        '    If blnInvalidDate(CDate(.strInvoiceDate), Date, 3, 0, "Invoice date", "today's date") Then
                        '        Exit Function
                        '    End If
                        'End If
                    End If
                    If .strDueDate <> "" Then
                        If Not Utilities.blnIsValidDate(.strDueDate) Then
                            ValidationError("Split #" & intSplit & " has an invalid due date.")
                            Exit Function
                        End If
                        datDueDate = CDate(.strDueDate)
                        'If blnInvalidDate(CDate(txtDate), datDueDate, 2, 2, "Transaction date", "the due date") Then
                        '    Exit Function
                        'End If
                        If .strInvoiceDate <> "" Then
                            datInvoiceDate = CDate(.strInvoiceDate)
                            If datInvoiceDate > datDueDate Then
                                ValidationError("Due date is before invoice date.")
                                Exit Function
                            End If
                            intNetPos = InStr(LCase(.strTerms), "net")
                            If intNetPos > 0 Then
                                intDaysUntilDue = CInt(Val(Trim(Mid(.strTerms, intNetPos + 3))))
                                'If intDaysUntilDue > 0 Then
                                '    If blnInvalidDate(datDueDate, _
                                ''        DateAdd("d", intDaysUntilDue, datInvoiceDate), 1, 1, "Due date", "the invoice date plus terms") Then
                                '        Exit Function
                                '    End If
                                'End If
                            End If
                        End If
                    Else
                        If blnInvalidTrxDateVersusToday() Then
                            Exit Function
                        End If
                    End If
                    If Not Utilities.blnIsValidAmount(.strAmount) Then
                        ValidationError("Split #" & intSplit & " has an invalid amount.")
                        Exit Function
                    End If
                    curAmount = CDec(.strAmount)
                    curNewTotal = curNewTotal + curAmount
                End With
            End If
        Next
        If intSplitsUsed = 0 Then
            ValidationError("At least one split must be entered.")
            Exit Function
        End If
        If mblnEditMode And (mlngOldStatus = Trx.TrxStatus.Reconciled) Then
            If curNewTotal <> mcurOldAmount Then
                If MsgBox("Saving this will change the amount of a transaction " &
                          "which has already been reconciled to a bank statement. " &
                          "Do you really want to do this?",
                          MsgBoxStyle.Question Or MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                    Exit Function
                End If
            End If
        End If
        blnValidateNormal = False
    End Function

    Private Function blnIsDuplicateTrx() As Boolean

        Dim strDescriptionKey As String
        Dim strCheckNumber As String
        Dim blnNumericCheckNumber As Boolean
        Dim objReg As Register

        Try

            strCheckNumber = txtNumber.Text
            blnNumericCheckNumber = IsNumeric(strCheckNumber)
            strDescriptionKey = strDescriptionCompareKey((txtDescription.Text))

            For Each objReg In mobjAccount.colRegisters
                If blnExistsInRegister(objReg, mobjReg, strCheckNumber, blnNumericCheckNumber, strDescriptionKey) Then
                    blnIsDuplicateTrx = True
                    Exit Function
                End If
            Next objReg

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Function blnExistsInRegister(ByVal objReg As Register, ByVal objOldReg As Register, ByVal strCheckNumber As String, ByVal blnNumericCheckNumber As Boolean, ByVal strDescriptionKey As String) As Boolean

        Dim lngIndex As Integer
        Dim objTrx As Trx
        Dim intSplit As Integer
        Dim objSplit As TrxSplit

        blnExistsInRegister = False

        'Start from latest trx and work backward, in case we decide
        'to start checking datEarliestToCheck.
        'datEarliestToCheck = DateAdd("d", -180, Date)
        lngIndex = objReg.lngTrxCount
        Do
            If lngIndex < 1 Then
                Exit Do
            End If
            objTrx = objReg.objTrx(lngIndex)
            If objTrx.lngType = Trx.TrxType.Normal Then
                'If objTrx.datDate < datEarliestToCheck Then
                '    Exit Do
                'End If
                'If not the same trx in edit mode
                If Not ((lngIndex = mlngIndex) And (objReg Is objOldReg) And mblnEditMode) Then
                    If objTrx.strNumber = strCheckNumber And blnNumericCheckNumber Then
                        ValidationError("Check number is already in use.")
                        blnExistsInRegister = True
                        Exit Function
                    End If
                    'Look for duplicate invoice if is same payee.
                    'Use a "standardized" version of the payee name to avoid problems like
                    '"Peoria" versus "Peoria Gardens".
                    If strDescriptionCompareKey((objTrx.strDescription)) = strDescriptionKey Then
                        For intSplit = 1 To mintSplits
                            If maudtSplits(intSplit).strInvoiceNum <> "" And mblnCheckInvoiceNum Then
                                'We now know this is a split we should check, because it is for
                                'the same (or a very similar) payee and there is an invoice number.
                                For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
                                    If maudtSplits(intSplit).strInvoiceNum = objSplit.strInvoiceNum Then
                                        ValidationError("Invoice number is already in use for this payee.")
                                        blnExistsInRegister = True
                                        Exit Function
                                    End If
                                    'They may have mistyped the invoice number, so check
                                    'for same split amount if split has invoice number.
                                    'Require an invoice number to filter out regular monthly bills
                                    'that may have the same amount every month, but which never
                                    'have an invoice number.
                                    'This function can be called before split information
                                    'has been validated, so we have to validate ourselves.
                                    'If Utilities.blnIsValidAmount(maudtSplits(intSplit).strAmount) Then
                                    '    If CCur(maudtSplits(intSplit).strAmount) = objSplit.curAmount Then
                                    '        If MsgBox("Invoice number " & objSplit.strInvoiceNum & _
                                    ''            " for payee " & objTrx.strDescription & _
                                    ''            " has the same amount. Continue?", _
                                    ''            vbCritical + vbOKCancel + vbDefaultButton2, _
                                    ''            "Possible Duplicate Invoice") = vbCancel Then
                                    '            blnExistsInRegister = True
                                    '            Exit Function
                                    '        End If
                                    '    End If
                                    'End If
                                Next objSplit
                            End If
                        Next
                    End If
                End If
            End If
            lngIndex = lngIndex - 1
        Loop

    End Function

    Private Function strDescriptionCompareKey(ByRef strInput As String) As String
        strDescriptionCompareKey = LCase(VB.Left(strInput, 4))
    End Function

    Private Function blnSplitUsed(ByVal intSplit As Integer) As Boolean
        blnSplitUsed = maudtSplits(intSplit).blnUsed
    End Function

    Private Function blnInvalidDate(ByVal datCheck As Date, ByVal datAnchor As Date, ByVal intWeeksBefore As Short, ByVal intWeeksAfter As Short, ByVal strCheckLabel As String, ByVal strAnchorLabel As String) As Boolean

        blnInvalidDate = True

        Dim datBefore As Date
        Dim datAfter As Date

        datBefore = DateAdd(Microsoft.VisualBasic.DateInterval.WeekOfYear, -intWeeksBefore, datAnchor)
        datAfter = DateAdd(Microsoft.VisualBasic.DateInterval.WeekOfYear, intWeeksAfter, datAnchor)

        If datCheck < datBefore Then
            If blnDeclineInvalidDate(strCheckLabel, datCheck, strAnchorLabel, intWeeksBefore, "before") Then
                Exit Function
            End If
        End If

        If datCheck > datAfter Then
            If blnDeclineInvalidDate(strCheckLabel, datCheck, strAnchorLabel, intWeeksAfter, "after") Then
                Exit Function
            End If
        End If

        blnInvalidDate = False
    End Function

    Private Function blnDeclineInvalidDate(ByVal strCheckLabel As String, ByVal datCheck As Date, ByVal strAnchorLabel As String, ByVal intWeeks As Integer, ByVal strRelationship As String) As Boolean

        Dim strMsg As String
        strMsg = strCheckLabel & " " & Utilities.strFormatDate(datCheck) & " is more than " & intWeeks & " weeks " & strRelationship & " " & strAnchorLabel & "." & vbCrLf & vbCrLf & "Is this date correct?"
        blnDeclineInvalidDate = MsgBox(strMsg, MsgBoxStyle.YesNo, "Date Validation") <> MsgBoxResult.Yes

    End Function

    Private Sub ValidationError(ByVal strMsg As String)
        MsgBox(strMsg, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
    End Sub

    Public Sub SaveNormal()
        Dim objTrxManager As NormalTrxManager
        Dim objTrx As NormalTrx
        If mblnEditMode Then
            objTrxManager = objUpdateStartNormal()
            objTrx = objTrxManager.objTrx
            AddSplits(objTrx)
            objTrxManager.UpdateEnd(New LogChange, mstrLogTitle)
        Else
            objTrx = objNewStartNormal()
            AddSplits(objTrx)
            mobjReg.NewAddEnd(objTrx, New LogAdd, mstrLogTitle)
        End If
        If objTrx.blnAnyUnmatchedBudget And Not mblnSilentMode Then
            mobjHostUI.InfoMessageBox("One or more budgeted splits in this transaction could not be " & "matched to budgets.")
        End If
    End Sub

    Private Function objUpdateStartNormal() As NormalTrxManager
        Dim objTrxManager As NormalTrxManager = mobjReg.objGetNormalTrxManager(mlngIndex)

        objTrxManager.UpdateStart()
        objTrxManager.objTrx.UpdateStartNormal(txtNumber.Text, CDate(txtDate.Text), txtDescription.Text, txtMemo.Text, lngTrxStatus(),
                                 blnTrxFake(), CDec(txtMatchRange.Text), blnAwaitingReview(), blnAutoGenerated(),
                                 intRepeatSeq(), strImportKey(), strRepeatKey())
        Return objTrxManager
    End Function

    Private Function objNewStartNormal() As NormalTrx
        objNewStartNormal = objCreateTrx(mobjReg)
    End Function

    Private Function objCreateTrx(ByVal objCreateReg As Register) As NormalTrx
        Dim objTrx As NormalTrx = New NormalTrx(objCreateReg)
        objTrx.NewStartNormal(True, txtNumber.Text, CDate(txtDate.Text), txtDescription.Text, txtMemo.Text,
                              lngTrxStatus(), blnTrxFake(), CDec(txtMatchRange.Text), blnAwaitingReview(), blnAutoGenerated(),
                              intRepeatSeq(), strImportKey(), strRepeatKey())
        objCreateTrx = objTrx
    End Function

    Private Function objCreateThrowawayTrx() As NormalTrx
        Dim objTrx As NormalTrx = New NormalTrx(Nothing)
        objTrx.NewStartNormal(False, txtNumber.Text, CDate(txtDate.Text), txtDescription.Text, txtMemo.Text,
                              lngTrxStatus(), blnTrxFake(), CDec(txtMatchRange.Text), blnAwaitingReview(), blnAutoGenerated(),
                              intRepeatSeq(), strImportKey(), strRepeatKey())
        objCreateThrowawayTrx = objTrx
    End Function

    Private Sub AddSplits(ByVal objTrx As NormalTrx)
        Dim intSplit As Integer
        For intSplit = 1 To mintSplits
            AddSplitIfUsed(objTrx, intSplit)
        Next
    End Sub

    Private Sub AddSplitIfUsed(ByVal objTrx As NormalTrx, ByVal intSplit As Integer)
        Dim datInvoiceDate As Date
        Dim datDueDate As Date

        If blnSplitUsed(intSplit) Then
            With maudtSplits(intSplit)
                If .strInvoiceDate = "" Then
                    datInvoiceDate = System.DateTime.FromOADate(0)
                Else
                    datInvoiceDate = CDate(.strInvoiceDate)
                End If
                If .strDueDate = "" Then
                    datDueDate = System.DateTime.FromOADate(0)
                Else
                    datDueDate = CDate(.strDueDate)
                End If
                objTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, datInvoiceDate, datDueDate, .strTerms, .strBudgetKey, CDec(.strAmount))
            End With
        End If
    End Sub

    Private Function lngTrxStatus() As Trx.TrxStatus
        Select Case VB.Left(cboStatus.Text, 1)
            Case "R"
                lngTrxStatus = Trx.TrxStatus.Reconciled
            Case "S"
                lngTrxStatus = Trx.TrxStatus.Selected
            Case Else
                lngTrxStatus = Trx.TrxStatus.Unreconciled
        End Select
    End Function

    Private Function blnTrxFake() As Boolean
        blnTrxFake = (chkFake.CheckState = System.Windows.Forms.CheckState.Checked)
    End Function

    Private Function blnAwaitingReview() As Boolean
        blnAwaitingReview = (chkAwaitingReview.CheckState = System.Windows.Forms.CheckState.Checked)
    End Function

    Private Function blnAutoGenerated() As Boolean
        blnAutoGenerated = (chkAutoGenerated.CheckState = System.Windows.Forms.CheckState.Checked)
    End Function

    Private Function intRepeatSeq() As Integer
        If txtRepeatSeq.Text = "" Then
            intRepeatSeq = 0
        Else
            intRepeatSeq = CInt(Val(txtRepeatSeq.Text))
        End If
    End Function

    Private Function strImportKey() As String
        strImportKey = CStr(IIf(chkImported.CheckState = System.Windows.Forms.CheckState.Checked, mstrImportKey, ""))
    End Function

    Private Function strRepeatKey() As String
        strRepeatKey = strGetStringTranslatorKeyFromCombo(cboRepeatKey, mobjAccount.objRepeats)
    End Function

    Private Sub cmdPrintCheck_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrintCheck.Click
        Dim strNumber As String
        Dim domCheckFormat As VB6XmlDocument
        Dim objTrx As NormalTrx

        Try
            If Not System.IO.File.Exists(mobjCompany.strCheckFormatPath()) Then
                mobjHostUI.InfoMessageBox("You must set up your check format first, using the option on the ""Setup"" menu.")
                Exit Sub
            End If

            If chkFake.CheckState <> System.Windows.Forms.CheckState.Checked Or chkAutoGenerated.CheckState = System.Windows.Forms.CheckState.Checked Then
                mobjHostUI.InfoMessageBox("You may only print a check for a fake, non-generated transaction.")
                Exit Sub
            End If
            If mcurTotalAmount >= 0 Then
                mobjHostUI.InfoMessageBox("You may only print a check for a debit transaction.")
                Exit Sub
            End If

            strNumber = InputBox("Please enter the check number to use:", "Check Number", gstrNextCheckNumToPrint)
            If strNumber = "" Then
                Exit Sub
            End If

            txtNumber.Text = strNumber
            txtDate.Text = Utilities.strFormatDate(Today)
            chkFake.CheckState = System.Windows.Forms.CheckState.Unchecked

            If blnValidateShared() Then
                Exit Sub
            End If
            If blnValidateNormal() Then
                Exit Sub
            End If

            gstrNextCheckNumToPrint = CStr(Val(strNumber) + 1)

            domCheckFormat = gdomGetCheckFormat(mobjHostUI)
            If domCheckFormat Is Nothing Then
                Exit Sub
            End If

            mobjReg.LogAction("PrintCheck:" & strNumber)
            objTrx = objCreateThrowawayTrx()
            AddSplits(objTrx)
            If gblnPrintCheck(mobjHostUI, domCheckFormat, objTrx) Then
                SaveNormal()
                Me.Close()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdDelSplits_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelSplits.Click
        Dim intMoveIndex As Integer
        Dim intDeleteIndex As Integer
        Dim intDeleteCount As Integer

        Try

            FixSplitComboBoxes()
            Dim counter As Integer
            counter = mintSplits
            For intDeleteIndex = counter To 1 Step -1
                If maudtSplits(intDeleteIndex).blnChoose Then
                    If blnPlaceholderAdjustmentRequired(intDeleteIndex) Then
                        maudtSplits(intDeleteIndex).strAmount = "0"
                        FindAndAdjustPlaceholderBudgets()
                    End If
                    For intMoveIndex = intDeleteIndex + 1 To mintSplits
                        maudtSplits(intMoveIndex - 1) = maudtSplits(intMoveIndex)
                    Next
                    mintSplits = mintSplits - 1
                    ReDim Preserve maudtSplits(mintSplits)
                    intDeleteCount = intDeleteCount + 1
                End If
            Next
            DisplaySplits(mintSplitOffset)
            If intDeleteCount = 0 Then
                mobjHostUI.InfoMessageBox("Please check one or more splits to delete.")
            Else
                mobjHostUI.InfoMessageBox("Deleted " & intDeleteCount & " splits.")
            End If
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdSplitScrollDown_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSplitScrollDown.Click
        'Yes, this routine is named the opposite of the button caption.
        Try
            FixSplitComboBoxes()
            DisplaySplits(mintSplitOffset + 1)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdSplitScrollUp_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSplitScrollUp.Click
        'Yes, this routine is named the opposite of the button caption.
        Try
            If mintSplitOffset > 0 Then
                FixSplitComboBoxes()
                DisplaySplits(mintSplitOffset - 1)
            End If
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    '$Description Sometimes the combo box .ListIndex properties are not set.
    '   This seems to happen when you type into a combo box, and SearchInComboBoxChange
    '   is called in the Change event handler. The underlying maudtSplits() information
    '   is set correctly, but the combo box controls may have a wrong .ListIndex property
    '   even though they appear visually correct. Since the maudtSplits() information is
    '   correct we set the combo boxes from that. The trx is safe to save even without
    '   doing this, but any operation which moves splits up or down must do this first
    '   or setting the combo box .ListIndex properties may not have the intended effect
    '   because .ListIndex may coincidentally already be the value assigned to it, so
    '   Windows thinks the combo box is already displaying the desired value when it
    '   really isn't.

    Private Sub FixSplitComboBoxes()
        Dim intControlIndex As Integer
        Dim intSplitIndex As Integer
        For intControlIndex = 0 To mintSPLIT_CTRL_ARRAY_SIZE - 1
            intSplitIndex = intControlIndex + mintSplitOffset + 1
            If intSplitIndex <= mintSplits Then
                With maudtSplits(intSplitIndex)
                    SetComboFromStringTranslator(cboSplitCategory(intControlIndex), mobjCompany.objCategories, .strCategoryKey)
                    SetComboFromStringTranslator(cboSplitBudget(intControlIndex), mobjCompany.objBudgets, .strBudgetKey)
                End With
            End If
        Next
    End Sub

    '$Description Divide the current trx into two trx. The checked splits move to a new trx,
    '   the uncheck trx stay with the original trx. All other properties of both trx are
    '   the same.

    Private Sub cmdDivideTrx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDivideTrx.Click
        Dim objTrxManager As NormalTrxManager
        Dim objTrx As NormalTrx
        Dim intSplit As Integer
        Dim intOldTrxSplitCount As Integer
        Dim intNewTrxSplitCount As Integer
        Dim objStartLogger As ILogGroupStart

        Try

            'Make sure all controls are valid.
            If chkFake.CheckState <> System.Windows.Forms.CheckState.Checked Then
                mobjHostUI.InfoMessageBox("Only fake transactions may be divided.")
                Exit Sub
            End If
            If blnValidateShared() Then
                Exit Sub
            End If
            If blnValidateNormal() Then
                Exit Sub
            End If

            'Make sure there will be at least one split in both new
            'and old splits.
            For intSplit = 1 To mintSplits
                If blnSplitUsed(intSplit) Then
                    If maudtSplits(intSplit).blnChoose Then
                        intNewTrxSplitCount = intNewTrxSplitCount + 1
                    Else
                        intOldTrxSplitCount = intOldTrxSplitCount + 1
                    End If
                End If
            Next
            If intNewTrxSplitCount = 0 Then
                mobjHostUI.InfoMessageBox("Please check the splits you want to move to a new transaction.")
                Exit Sub
            End If
            If intOldTrxSplitCount = 0 Then
                mobjHostUI.InfoMessageBox("You cannot move all the splits to a new transaction.")
                Exit Sub
            End If

            'Confirmation.
            If MsgBox("Are you sure you want to move the " & intNewTrxSplitCount & " checked " & "splits to a new transaction?",
                      MsgBoxStyle.Question Or MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
                Exit Sub
            End If

            'Update the existing trx, without the checked splits.
            objTrxManager = objUpdateStartNormal()
            objTrx = objTrxManager.objTrx
            For intSplit = 1 To mintSplits
                If Not maudtSplits(intSplit).blnChoose Then
                    AddSplitIfUsed(objTrx, intSplit)
                End If
            Next
            objStartLogger = mobjReg.objLogGroupStart("TrxForm.Divide")
            objTrxManager.UpdateEnd(New LogChange, mstrLogTitle & ".DivideOld")

            'Create the new trx, with the checked splits.
            objTrx = objNewStartNormal()
            objTrx.ClearRepeat()
            For intSplit = 1 To mintSplits
                If maudtSplits(intSplit).blnChoose Then
                    AddSplitIfUsed(objTrx, intSplit)
                End If
            Next
            mobjReg.NewAddEnd(objTrx, New LogAdd, mstrLogTitle & ".DivideNew")
            mobjReg.LogGroupEnd(objStartLogger)

            mblnCancel = False
            Me.Close()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub txtDate_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDate.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Dim intIncrement As Integer

        Try
            If KeyAscii = 43 Or KeyAscii = 61 Then
                '"+" or "="
                intIncrement = 1
            ElseIf KeyAscii = 45 Then
                '"-"
                intIncrement = -1
            Else
                Exit Sub
            End If
            If Utilities.blnIsValidDate(txtDate.Text) Then
                txtDate.Text = Utilities.strFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, intIncrement, CDate(txtDate.Text)))
            End If
            eventArgs.Handled = True
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub txtDescription_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDescription.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Dim strPayee As String = ""
        Dim strCategory As String = ""
        Dim strNumber As String = ""
        Dim strAmount As String = ""
        Dim strBudget As String = ""
        Dim strMemo As String = ""

        Try

            '^S (for memorized transaction search)
            If KeyAscii = 19 Then
                KeyAscii = 0
                If blnFindPayee(strPayee, strCategory, strNumber, strBudget, strAmount, strMemo) Then
                    txtDescription.Text = strPayee
                    If strMemo <> "" Then
                        txtMemo.Text = strMemo
                    End If
                    If strNumber <> "" Then
                        txtNumber.Text = strNumber
                    End If
                    If mlngType = Trx.TrxType.Normal Then
                        cboSplitCategory(0).Text = strCategory
                        If strBudget <> "" Then
                            cboSplitBudget(0).Text = strBudget
                        End If
                        If strAmount <> "" Then
                            txtSplitAmount(0).Text = strAmount
                        End If
                        'txtSplitAmount(0).SetFocus
                        txtSplitInvoiceNum(0).Focus()
                    ElseIf mlngType = Trx.TrxType.Transfer Then
                        If strAmount <> "" Then
                            txtTransferAmount.Text = strAmount
                        End If
                    ElseIf mlngType = Trx.TrxType.Budget Then
                        If strAmount <> "" Then
                            txtBudgetLimit.Text = strAmount
                        End If
                    End If
                End If
                eventArgs.Handled = True
            End If

        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnFindPayee(ByRef strPayee As String, ByRef strCategory As String, ByRef strNumber As String,
                                  ByRef strBudget As String, ByRef strAmount As String, ByRef strMemo As String) As Boolean

        Dim colPayees As VB6XmlNodeList
        'Dim strInput As String
        'Dim strXPath As String
        Dim elmPayee As VB6XmlElement
        Dim elmChild As VB6XmlElement
        Dim frm As PayeeMatchForm

        Try

            strPayee = ""
            strCategory = ""

            colPayees = mobjCompany.colFindPayeeMatches(txtDescription.Text)
            If colPayees.Length = 0 Then
                Exit Function
            End If
            If colPayees.Length = 1 Then
                elmPayee = DirectCast(colPayees.Item(0), VB6XmlElement)
            Else
                frm = New PayeeMatchForm
                elmPayee = frm.elmSelect(mobjHostUI, colPayees)
                If elmPayee Is Nothing Then
                    Exit Function
                End If
            End If
            strPayee = CStr(elmPayee.GetAttribute("Output"))
            elmChild = DirectCast(elmPayee.SelectSingleNode("Cat"), VB6XmlElement)
            If Not elmChild Is Nothing Then
                strCategory = elmChild.Text
            End If
            elmChild = DirectCast(elmPayee.SelectSingleNode("Num"), VB6XmlElement)
            If Not elmChild Is Nothing Then
                strNumber = elmChild.Text
            End If
            elmChild = DirectCast(elmPayee.SelectSingleNode("Amount"), VB6XmlElement)
            If Not elmChild Is Nothing Then
                strAmount = elmChild.Text
            End If
            elmChild = DirectCast(elmPayee.SelectSingleNode("Budget"), VB6XmlElement)
            If Not elmChild Is Nothing Then
                strBudget = elmChild.Text
            End If
            elmChild = DirectCast(elmPayee.SelectSingleNode("Memo"), VB6XmlElement)
            If Not elmChild Is Nothing Then
                strMemo = elmChild.Text
            End If
            blnFindPayee = True

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub txtNumber_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtNumber.TextChanged
        Try

            If mblnInSetNormalControls Then
                Exit Sub
            End If

            If IsNumeric(txtNumber.Text) Then
                SaveSetting(gstrREG_APP, mobjReg.strRegistryKey(), "TrxNum", txtNumber.Text)
            ElseIf LCase(txtNumber.Text) = "inv" Or LCase(txtNumber.Text) = "crm" Then
                chkFake.CheckState = System.Windows.Forms.CheckState.Checked
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub txtNumber_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtNumber.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Dim lngOldNum As Integer
        Dim lngNewNum As Integer
        Dim strKey As String

        Try

            strKey = Chr(KeyAscii)
            If InStr("-=+", strKey) = 0 Then
                Exit Sub
            End If
            eventArgs.Handled = True

            If IsNumeric(txtNumber.Text) Then
                lngOldNum = CInt(txtNumber.Text)
            Else
                lngOldNum = CInt(GetSetting(gstrREG_APP, mobjReg.strRegistryKey(), "TrxNum", "1000"))
            End If

            If strKey = "-" Then
                lngNewNum = lngOldNum - 1
            Else
                lngNewNum = lngOldNum + 1
            End If

            txtNumber.Text = CStr(lngNewNum)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdRptInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRptInfo.Click
        Dim frm As RepeatSeqInfoForm
        frm = New RepeatSeqInfoForm
        frm.ShowMe(mobjHostUI, mobjReg, strRepeatKey())
    End Sub

    Private Sub txtSplitPONum_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitPONum_0.TextChanged,
        _txtSplitPONum_1.TextChanged,
        _txtSplitPONum_2.TextChanged,
        _txtSplitPONum_3.TextChanged,
        _txtSplitPONum_4.TextChanged,
        _txtSplitPONum_5.TextChanged,
        _txtSplitPONum_6.TextChanged,
        _txtSplitPONum_7.TextChanged,
        _txtSplitPONum_8.TextChanged,
        _txtSplitPONum_9.TextChanged
        Dim index As Short = CShort(CType(eventSender, TextBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).strPONumber = txtSplitPONum(index).Text
    End Sub

    Private Sub txtSplitInvoiceNum_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitInvoiceNum_0.TextChanged,
        _txtSplitInvoiceNum_1.TextChanged,
        _txtSplitInvoiceNum_2.TextChanged,
        _txtSplitInvoiceNum_3.TextChanged,
        _txtSplitInvoiceNum_4.TextChanged,
        _txtSplitInvoiceNum_5.TextChanged,
        _txtSplitInvoiceNum_6.TextChanged,
        _txtSplitInvoiceNum_7.TextChanged,
        _txtSplitInvoiceNum_8.TextChanged,
        _txtSplitInvoiceNum_9.TextChanged
        Dim index As Short = CShort(CType(eventSender, TextBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).strInvoiceNum = txtSplitInvoiceNum(index).Text
    End Sub

    Private Sub txtSplitInvoiceDate_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitInvoiceDate_0.TextChanged,
        _txtSplitInvoiceDate_1.TextChanged,
        _txtSplitInvoiceDate_2.TextChanged,
        _txtSplitInvoiceDate_3.TextChanged,
        _txtSplitInvoiceDate_4.TextChanged,
        _txtSplitInvoiceDate_5.TextChanged,
        _txtSplitInvoiceDate_6.TextChanged,
        _txtSplitInvoiceDate_7.TextChanged,
        _txtSplitInvoiceDate_8.TextChanged,
        _txtSplitInvoiceDate_9.TextChanged
        Dim index As Short = CShort(CType(eventSender, TextBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).strInvoiceDate = txtSplitInvoiceDate(index).Text
    End Sub

    Private Sub txtSplitDueDate_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitDueDate_0.TextChanged,
        _txtSplitDueDate_1.TextChanged,
        _txtSplitDueDate_2.TextChanged,
        _txtSplitDueDate_3.TextChanged,
        _txtSplitDueDate_4.TextChanged,
        _txtSplitDueDate_5.TextChanged,
        _txtSplitDueDate_6.TextChanged,
        _txtSplitDueDate_7.TextChanged,
        _txtSplitDueDate_8.TextChanged,
        _txtSplitDueDate_9.TextChanged
        Dim index As Short = CShort(CType(eventSender, TextBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).strDueDate = txtSplitDueDate(index).Text
    End Sub

    Private Sub txtSplitTerms_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitTerms_0.TextChanged,
        _txtSplitTerms_1.TextChanged,
        _txtSplitTerms_2.TextChanged,
        _txtSplitTerms_3.TextChanged,
        _txtSplitTerms_4.TextChanged,
        _txtSplitTerms_5.TextChanged,
        _txtSplitTerms_6.TextChanged,
        _txtSplitTerms_7.TextChanged,
        _txtSplitTerms_8.TextChanged,
        _txtSplitTerms_9.TextChanged
        Dim index As Short = CShort(CType(eventSender, TextBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).strTerms = txtSplitTerms(index).Text
    End Sub

    Private Sub txtSplitAmount_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles _
        _txtSplitAmount_0.KeyPress,
        _txtSplitAmount_1.KeyPress,
        _txtSplitAmount_2.KeyPress,
        _txtSplitAmount_3.KeyPress,
        _txtSplitAmount_4.KeyPress,
        _txtSplitAmount_5.KeyPress,
        _txtSplitAmount_6.KeyPress,
        _txtSplitAmount_7.KeyPress,
        _txtSplitAmount_8.KeyPress,
        _txtSplitAmount_9.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Dim index As Integer = CShort(CType(eventSender, TextBox).Tag)
        Try

            '^D (for divide split)
            If KeyAscii <> 4 Then
                Exit Sub
            End If
            eventArgs.Handled = True

            'Current split amount must be numeric and non-zero.
            If Not IsNumeric(txtSplitAmount(index).Text) Then
                mobjHostUI.InfoMessageBox("Invalid split amount.")
                Exit Sub
            End If
            Dim intSplit As Integer
            Dim curSplitAmount As Decimal
            intSplit = intGetSplitIndex(index)
            curSplitAmount = CDec(maudtSplits(intSplit).strAmount)
            If curSplitAmount = 0 Then
                mobjHostUI.InfoMessageBox("Split to divide must not be zero amount.")
                Exit Sub
            End If

            'Ask and validate how much to move to new split.
            Dim strNewAmount As String
            Dim curNewAmount As Decimal
            strNewAmount = InputBox("Enter amount of new split (this will be subtracted " & "from the current split)")
            If Trim(strNewAmount) = "" Then
                Exit Sub
            End If
            If Not IsNumeric(strNewAmount) Then
                mobjHostUI.InfoMessageBox("Invalid amount.")
                Exit Sub
            End If
            curNewAmount = CDec(strNewAmount)
            If System.Math.Sign(curSplitAmount) <> System.Math.Sign(curNewAmount) Then
                mobjHostUI.InfoMessageBox("New split amount must have the same sign as the old one.")
                Exit Sub
            End If
            If System.Math.Abs(curNewAmount) >= System.Math.Abs(curSplitAmount) Then
                mobjHostUI.InfoMessageBox("New split amount must be smaller than the old amount.")
                Exit Sub
            End If

            'Find an unused split to use, or create a new one.
            Dim intDstSplit As Integer
            intDstSplit = 1
            Do
                If intDstSplit > mintSplits Then
                    mintSplits = mintSplits + 1
                    ReDim Preserve maudtSplits(mintSplits)
                    intDstSplit = mintSplits
                    Exit Do
                End If
                If Not blnSplitUsed(intDstSplit) Then
                    Exit Do
                End If
                intDstSplit = intDstSplit + 1
            Loop

            'Set up the new split and decrease the amount of the old one.
            Dim objOldSplit As SplitData
            Dim objNewSplit As SplitData
            Dim strNewInvoice As String
            Dim intParenPos As Integer
            Dim strInvNumPostfix As String
            objNewSplit = New SplitData
            maudtSplits(intDstSplit) = objNewSplit
            objOldSplit = maudtSplits(intSplit)
            objNewSplit.strCategoryKey = objOldSplit.strCategoryKey
            objNewSplit.strPONumber = objOldSplit.strPONumber
            objNewSplit.strBudgetKey = objOldSplit.strBudgetKey
            objNewSplit.strDueDate = objOldSplit.strDueDate
            objNewSplit.strInvoiceDate = objOldSplit.strInvoiceDate
            'Figure out the new invoice number.
            'It must end in "(n)", where "n" the current date and time.
            strNewInvoice = objOldSplit.strInvoiceNum
            If strNewInvoice <> "" Then
                strInvNumPostfix = "(" & Utilities.strFormatDate(Now, "MM/dd/yy") & ")"
                intParenPos = InStr(strNewInvoice, "(")
                If intParenPos > 0 Then
                    strNewInvoice = VB.Left(strNewInvoice, intParenPos - 1) & strInvNumPostfix
                Else
                    strNewInvoice = strNewInvoice & strInvNumPostfix
                End If
                objNewSplit.strInvoiceNum = strNewInvoice
            End If
            objNewSplit.strMemo = objOldSplit.strMemo
            objNewSplit.strTerms = objOldSplit.strTerms
            objOldSplit.strAmount = Utilities.strFormatCurrency(curSplitAmount - curNewAmount)
            objNewSplit.strAmount = Utilities.strFormatCurrency(curNewAmount)

            'Redisplay all the splits.
            DisplaySplits(mintSplitOffset)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub txtSplitAmount_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitAmount_0.TextChanged,
        _txtSplitAmount_1.TextChanged,
        _txtSplitAmount_2.TextChanged,
        _txtSplitAmount_3.TextChanged,
        _txtSplitAmount_4.TextChanged,
        _txtSplitAmount_5.TextChanged,
        _txtSplitAmount_6.TextChanged,
        _txtSplitAmount_7.TextChanged,
        _txtSplitAmount_8.TextChanged,
        _txtSplitAmount_9.TextChanged
        Dim index As Integer = CInt(CType(eventSender, TextBox).Tag)
        Dim intSplitIndex As Integer
        If Not (mblnSuppressPlaceholderAdjustment Or mblnInDisplaySplits) Then
            intSplitIndex = intGetSplitIndex(index)
            maudtSplits(intSplitIndex).strAmount = txtSplitAmount(index).Text
            If blnPlaceholderAdjustmentRequired(intSplitIndex) Then
                FindAndAdjustPlaceholderBudgets()
            Else
                DisplaySplitTotal()
            End If
        End If
    End Sub

    Private Function intGetSplitIndex(ByRef intControlIndex As Integer) As Integer
        intGetSplitIndex = intControlIndex + mintSplitOffset + 1
    End Function

    Private Sub txtSplitMemo_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _txtSplitMemo_0.TextChanged,
        _txtSplitMemo_1.TextChanged,
        _txtSplitMemo_2.TextChanged,
        _txtSplitMemo_3.TextChanged,
        _txtSplitMemo_4.TextChanged,
        _txtSplitMemo_5.TextChanged,
        _txtSplitMemo_6.TextChanged,
        _txtSplitMemo_7.TextChanged,
        _txtSplitMemo_8.TextChanged,
        _txtSplitMemo_9.TextChanged
        Dim index As Short = CShort(CType(eventSender, TextBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).strMemo = txtSplitMemo(index).Text
    End Sub

    '$Description See blnValidateShared().

    Private Function blnValidateBudget() As Boolean
        blnValidateBudget = True
        If Not Utilities.blnIsValidDate(txtBudgetStarts.Text) Then
            ValidationError("Invalid budget starting date.")
            Exit Function
        End If
        If CDate(txtBudgetStarts.Text) > CDate(txtDate.Text) Then
            ValidationError("Budget starting date may not be after transaction date.")
            Exit Function
        End If
        If Not Utilities.blnIsValidAmount(txtBudgetLimit.Text) Then
            ValidationError("Invalid budget limit.")
            Exit Function
        End If
        If cboBudgetName.SelectedIndex < 1 Then
            ValidationError("Budget name is required.")
            Exit Function
        End If
        blnValidateBudget = False
    End Function

    Private Sub SaveBudget()
        Dim objTrxManager As BudgetTrxManager
        Dim strBudgetKey As String
        Dim datBudgetStarts As Date
        strBudgetKey = strGetStringTranslatorKeyFromCombo(cboBudgetName, mobjCompany.objBudgets)
        datBudgetStarts = CDate(txtBudgetStarts.Text)
        If mblnEditMode Then
            objTrxManager = mobjReg.objGetBudgetTrxManager(mlngIndex)
            objTrxManager.UpdateStart()
            objTrxManager.objTrx.UpdateStartBudget(CDate(txtDate.Text), txtDescription.Text, txtMemo.Text, blnAwaitingReview(),
                                     blnAutoGenerated(), intRepeatSeq(), strRepeatKey(),
                                     CDec(txtBudgetLimit.Text), datBudgetStarts, strBudgetKey, mobjReg.datOldestBudgetEndAllowed)
            objTrxManager.UpdateEnd(New LogChange, mstrLogTitle)
        Else
            Dim objTrx As BudgetTrx = New BudgetTrx(mobjReg)
            objTrx.NewStartBudget(True, CDate(txtDate.Text), txtDescription.Text, txtMemo.Text, blnAwaitingReview(), blnAutoGenerated(), intRepeatSeq(), strRepeatKey(), CDec(txtBudgetLimit.Text), datBudgetStarts, strBudgetKey)
            mobjReg.NewAddEnd(objTrx, New LogAdd, mstrLogTitle)
        End If
    End Sub

    '$Description See blnValidateShared().

    Private Function blnValidateTransfer(ByRef strOtherRegisterKey As String) As Boolean
        blnValidateTransfer = True
        If blnInvalidTrxDateVersusToday() Then
            Exit Function
        End If
        If Not Utilities.blnIsValidAmount(txtTransferAmount.Text) Then
            ValidationError("Invalid transfer amount.")
            Exit Function
        End If
        If cboTransferTo.SelectedIndex < 1 Then
            ValidationError("""Transfer to"" is required.")
            Exit Function
        End If
        strOtherRegisterKey = strGetStringTranslatorKeyFromCombo(cboTransferTo, mobjAccount.objRegisterList())
        If strOtherRegisterKey = mobjReg.strRegisterKey Then
            ValidationError("You may not transfer to the same register.")
            Exit Function
        End If
        blnValidateTransfer = False
    End Function

    Private Sub SaveTransfer(ByVal strOtherRegisterKey As String)
        Dim objOtherReg As Register
        Dim objXfer As TransferManager

        objOtherReg = mobjAccount.objFindReg(strOtherRegisterKey)
        objXfer = New TransferManager
        If mblnEditMode Then
            objXfer.UpdateTransfer(mobjReg, mlngIndex, objOtherReg, CDate(txtDate.Text), txtDescription.Text, txtMemo.Text, blnTrxFake(), CDec(txtTransferAmount.Text), strRepeatKey(), blnAwaitingReview(), blnAutoGenerated(), intRepeatSeq())
        Else
            objXfer.AddTransfer(mobjReg, objOtherReg, CDate(txtDate.Text), txtDescription.Text, txtMemo.Text, blnTrxFake(), CDec(txtTransferAmount.Text), strRepeatKey(), blnAwaitingReview(), blnAutoGenerated(), intRepeatSeq())
        End If
    End Sub

    Private Function strGetStringTranslatorKeyFromCombo(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As IStringTranslator) As String

        Dim lngItemData As Integer

        If cbo.SelectedIndex = -1 Then
            strGetStringTranslatorKeyFromCombo = ""
        Else
            lngItemData = UITools.GetItemData(cbo, cbo.SelectedIndex)
            If lngItemData = 0 Then
                strGetStringTranslatorKeyFromCombo = ""
            Else
                strGetStringTranslatorKeyFromCombo = objList.strKey(lngItemData)
            End If
        End If
    End Function

    Private Sub chkChoose_CheckStateChanged(sender As Object, e As EventArgs) Handles _
        _chkChoose_0.CheckStateChanged,
        _chkChoose_1.CheckStateChanged,
        _chkChoose_2.CheckStateChanged,
        _chkChoose_3.CheckStateChanged,
        _chkChoose_4.CheckStateChanged,
        _chkChoose_5.CheckStateChanged,
        _chkChoose_6.CheckStateChanged,
        _chkChoose_7.CheckStateChanged,
        _chkChoose_8.CheckStateChanged,
        _chkChoose_9.CheckStateChanged

        Dim index As Short = CShort(CType(sender, CheckBox).Tag)
        maudtSplits(index + 1 + mintSplitOffset).blnChoose = (chkChoose(index).CheckState = System.Windows.Forms.CheckState.Checked)
    End Sub

    Private Sub cboSplitCategory_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        _cboSplitCategory_0.TextChanged,
        _cboSplitCategory_1.TextChanged,
        _cboSplitCategory_2.TextChanged,
        _cboSplitCategory_3.TextChanged,
        _cboSplitCategory_4.TextChanged,
        _cboSplitCategory_5.TextChanged,
        _cboSplitCategory_6.TextChanged,
        _cboSplitCategory_7.TextChanged,
        _cboSplitCategory_8.TextChanged,
        _cboSplitCategory_9.TextChanged

        Try
            Dim index As Integer = CShort(CType(sender, ComboBox).Tag)
            If Not cboSplitCategory Is Nothing Then
                If Not mblnInDisplaySplits Then
                    SearchInComboBoxChange(cboSplitCategory(index), mintSplitCatKeyCode)
                End If
            End If
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cboSplitCategory_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _
        _cboSplitCategory_0.KeyDown,
        _cboSplitCategory_1.KeyDown,
        _cboSplitCategory_2.KeyDown,
        _cboSplitCategory_3.KeyDown,
        _cboSplitCategory_4.KeyDown,
        _cboSplitCategory_5.KeyDown,
        _cboSplitCategory_6.KeyDown,
        _cboSplitCategory_7.KeyDown,
        _cboSplitCategory_8.KeyDown,
        _cboSplitCategory_9.KeyDown

        Dim KeyCode As Integer = e.KeyCode
        If Not cboSplitCategory Is Nothing Then
            mintSplitCatKeyCode = KeyCode
        End If
    End Sub

    Private Sub cboSplitCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles _
        _cboSplitCategory_0.SelectedIndexChanged,
        _cboSplitCategory_1.SelectedIndexChanged,
        _cboSplitCategory_2.SelectedIndexChanged,
        _cboSplitCategory_3.SelectedIndexChanged,
        _cboSplitCategory_4.SelectedIndexChanged,
        _cboSplitCategory_5.SelectedIndexChanged,
        _cboSplitCategory_6.SelectedIndexChanged,
        _cboSplitCategory_7.SelectedIndexChanged,
        _cboSplitCategory_8.SelectedIndexChanged,
        _cboSplitCategory_9.SelectedIndexChanged

        Try
            Dim index As Integer = CShort(CType(sender, ComboBox).Tag)
            If Not cboSplitCategory Is Nothing Then
                If Not mblnInDisplaySplits Then
                    maudtSplits(index + 1 + mintSplitOffset).strCategoryKey = strGetStringTranslatorKeyFromCombo(cboSplitCategory(index),
                        mobjCompany.objCategories)
                End If
            End If
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cboSplitBudget_TextChanged(sender As Object, e As EventArgs) Handles _
        _cboSplitBudget_0.TextChanged,
        _cboSplitBudget_1.TextChanged,
        _cboSplitBudget_2.TextChanged,
        _cboSplitBudget_3.TextChanged,
        _cboSplitBudget_4.TextChanged,
        _cboSplitBudget_5.TextChanged,
        _cboSplitBudget_6.TextChanged,
        _cboSplitBudget_7.TextChanged,
        _cboSplitBudget_8.TextChanged,
        _cboSplitBudget_9.TextChanged

        Try
            Dim index As Short = CShort(CType(sender, ComboBox).Tag)
            If Not cboSplitBudget Is Nothing Then
                If Not mblnInDisplaySplits Then
                    SearchInComboBoxChange(cboSplitBudget(index), mintSplitBudgetKeyCode)
                End If
            End If
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cboSplitBudget_KeyDown(sender As Object, e As KeyEventArgs) Handles _
        _cboSplitBudget_0.KeyDown,
        _cboSplitBudget_1.KeyDown,
        _cboSplitBudget_2.KeyDown,
        _cboSplitBudget_3.KeyDown,
        _cboSplitBudget_4.KeyDown,
        _cboSplitBudget_5.KeyDown,
        _cboSplitBudget_6.KeyDown,
        _cboSplitBudget_7.KeyDown,
        _cboSplitBudget_8.KeyDown,
        _cboSplitBudget_9.KeyDown

        Dim KeyCode As Integer = e.KeyCode
        If Not cboSplitBudget Is Nothing Then
            mintSplitBudgetKeyCode = KeyCode
        End If
    End Sub

    Private Sub cboSplitBudget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _
        _cboSplitBudget_0.SelectedIndexChanged,
        _cboSplitBudget_1.SelectedIndexChanged,
        _cboSplitBudget_2.SelectedIndexChanged,
        _cboSplitBudget_3.SelectedIndexChanged,
        _cboSplitBudget_4.SelectedIndexChanged,
        _cboSplitBudget_5.SelectedIndexChanged,
        _cboSplitBudget_6.SelectedIndexChanged,
        _cboSplitBudget_7.SelectedIndexChanged,
        _cboSplitBudget_8.SelectedIndexChanged,
        _cboSplitBudget_9.SelectedIndexChanged

        Try
            Dim index As Short = CShort(CType(sender, ComboBox).Tag)
            If Not cboSplitBudget Is Nothing Then
                If Not mblnInDisplaySplits Then
                    maudtSplits(index + 1 + mintSplitOffset).strBudgetKey = strGetStringTranslatorKeyFromCombo(cboSplitBudget(index), mobjCompany.objBudgets)
                    CheckForPlaceholderBudget()
                End If
            End If
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function gPrintCheck(domCheckFormat As VB6XmlDocument, objTrx As Trx) As Boolean
        Throw New NotImplementedException
    End Function

End Class
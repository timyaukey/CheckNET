Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class SearchForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mfrmReg As RegisterForm
    Private WithEvents mobjReg As Register
    Private mobjAccount As Account
    Private mdatDefaultDate As Date

    Private Structure SearchMatch
        Dim lngRegIndex As Integer
    End Structure

    Private maudtMatches() As SearchMatch
    Private mlngMatchesUsed As Integer
    Private mlngMatchesAlloc As Integer
    Private mcurAmountMatched As Decimal
    Private mcurAmountTotal As Decimal
    Private mblnIgnoreTrxUpdates As Boolean
    Private colCheckedTrx As Collection
    Private objSelectedTrx As Trx

    'Parameters of most recent successful search
    Private mlngLastSearchType As Trx.TrxSearchType
    Private mlngLastSearchField As Trx.TrxSearchField
    Private mstrLastSearchFor As String
    Private mdatLastStart As Date
    Private mdatLastEnd As Date
    Private mblnLastIncludeGenerated As Boolean

    Private Const mintHIDDEN_COL As Short = 12

    Public Sub ShowMe(ByVal objReg_ As Register, ByVal objAccount_ As Account, ByVal frmReg_ As RegisterForm)

        mobjReg = objReg_
        mobjAccount = objAccount_
        mfrmReg = frmReg_
        colCheckedTrx = New Collection
        mcurAmountMatched = 0
        mcurAmountTotal = 0
        mdatDefaultDate = Today
        Me.Text = "Search " & mobjReg.strTitle
        txtStartDate.Text = gstrFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, -2, Today))
        txtEndDate.Text = gstrFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, 6, Today))
        LoadSearchIn()
        LoadSearchType()
        LoadComboFromStringTranslator(cboSearchCats, gobjCategories)
        cboSearchIn.SelectedIndex = 0
        cboSearchType.SelectedIndex = 2
        txtSearchFor.Focus()
        Me.Show()

    End Sub

    Private Sub SearchForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 1011
        Me.Height = 547
    End Sub

    Private Sub SearchForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'UPGRADE_NOTE: Object mobjReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjReg = Nothing
    End Sub

    Private Sub LoadSearchIn()
        cboSearchIn.Items.Clear()
        LoadSearchInOne("Description", Trx.TrxSearchField.glngTRXSFL_DESCR)
        LoadSearchInOne("Memo", Trx.TrxSearchField.glngTRXSFL_MEMO)
        LoadSearchInOne("Category", Trx.TrxSearchField.glngTRXSFL_CATKEY)
        LoadSearchInOne("Number", Trx.TrxSearchField.glngTRXSFL_NUMBER)
        LoadSearchInOne("Amount", Trx.TrxSearchField.glngTRXSFL_AMOUNT)
        LoadSearchInOne("Invoice #", Trx.TrxSearchField.glngTRXSFL_INVNUM)
        LoadSearchInOne("PO #", Trx.TrxSearchField.glngTRXSFL_PONUMBER)
    End Sub

    Private Sub LoadSearchInOne(ByVal strTitle As String, ByVal lngFieldID As Trx.TrxSearchField)
        With cboSearchIn
            .Items.Add(gobjCreateListBoxItem(strTitle, lngFieldID))
        End With
    End Sub

    Private Sub LoadSearchType()
        cboSearchType.Items.Clear()
        LoadSearchTypeOne("Equal To", Trx.TrxSearchType.glngTRXSTP_EQUAL)
        LoadSearchTypeOne("Starts With", Trx.TrxSearchType.glngTRXSTP_STARTS)
        LoadSearchTypeOne("Contains", Trx.TrxSearchType.glngTRXSTP_CONTAINS)
    End Sub

    Private Sub LoadSearchTypeOne(ByVal strTitle As String, ByVal lngTypeID As Trx.TrxSearchType)
        With cboSearchType
            .Items.Add(gobjCreateListBoxItem(strTitle, lngTypeID))
        End With
    End Sub

    Private Sub LoadComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As StringTranslator)

        gLoadComboFromStringTranslator(cbo, objList, False)
        With cbo
            .Left = txtSearchFor.Left
            .Top = txtSearchFor.Top
        End With
    End Sub

    'UPGRADE_WARNING: Event cboSearchIn.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub cboSearchIn_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSearchIn.SelectedIndexChanged
        Select Case gintVB6GetItemData(cboSearchIn, cboSearchIn.SelectedIndex)
            Case Trx.TrxSearchField.glngTRXSFL_CATKEY
                txtSearchFor.Visible = False
                cboSearchCats.Visible = True
            Case Else
                txtSearchFor.Visible = True
                cboSearchCats.Visible = False
        End Select
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Dim lngSearchType As Trx.TrxSearchType
        Dim lngSearchField As Trx.TrxSearchField
        Dim strSearchFor As String
        Dim lngItemData As Integer

        Try

            lngSearchType = gintVB6GetItemData(cboSearchType, cboSearchType.SelectedIndex)
            lngSearchField = gintVB6GetItemData(cboSearchIn, cboSearchIn.SelectedIndex)
            Select Case lngSearchField
                Case Trx.TrxSearchField.glngTRXSFL_CATKEY
                    If lngSearchType <> Trx.TrxSearchType.glngTRXSTP_EQUAL And lngSearchType <> Trx.TrxSearchType.glngTRXSTP_STARTS Then
                        MsgBox("Category searches must be ""equal to"" or ""starts with"".", MsgBoxStyle.Critical)
                        Exit Sub
                    End If
                    If cboSearchCats.SelectedIndex = -1 Then
                        MsgBox("Please select a category to search for.", MsgBoxStyle.Critical)
                        Exit Sub
                    Else
                        lngItemData = gintVB6GetItemData(cboSearchCats, cboSearchCats.SelectedIndex)
                        strSearchFor = gobjCategories.strKey(lngItemData)
                        If lngSearchType = Trx.TrxSearchType.glngTRXSTP_STARTS Then
                            strSearchFor = gobjCategories.strKeyToValue1(strSearchFor)
                        End If
                    End If
                Case Else
                    'Allow "search for nothing" so you can find all trx in a date range.
                    'If txtSearchFor = "" Then
                    '    MsgBox "Please enter something to search for.", vbCritical
                    '    Exit Sub
                    'End If
                    strSearchFor = txtSearchFor.Text
            End Select

            If Not gblnValidDate(txtStartDate.Text) Then
                MsgBox("Invalid starting date.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If Not gblnValidDate(txtEndDate.Text) Then
                MsgBox("Invalid ending date.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            ClearResults()

            mlngLastSearchField = lngSearchField
            mstrLastSearchFor = strSearchFor
            mlngLastSearchType = lngSearchType
            mblnLastIncludeGenerated = (chkIncludeGenerated.CheckState = System.Windows.Forms.CheckState.Checked)
            mdatLastStart = CDate(txtStartDate.Text)
            mdatLastEnd = CDate(txtEndDate.Text)

            SearchInternal()
            RememberCheckedAndSelected()

            mcurAmountTotal = mcurAmountTotal + mcurAmountMatched
            ShowTotals()
            MsgBox(mlngMatchesUsed & " matches found.", MsgBoxStyle.Information)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub SearchInternal()
        Dim lngStartIndex As Integer
        Dim lngIndex As Integer
        Dim objTrx As Trx
        Dim curMatchAmount As Decimal

        lngStartIndex = mobjReg.lngFindBeforeDate(mdatLastStart) + 1
        For lngIndex = lngStartIndex To mobjReg.lngTrxCount
            objTrx = mobjReg.objTrx(lngIndex)
            If objTrx.datDate > mdatLastEnd Then
                Exit For
            End If
            If mblnLastIncludeGenerated Or Not objTrx.blnAutoGenerated Then
                If objTrx.blnIsSearchMatch(mlngLastSearchField, mstrLastSearchFor, mlngLastSearchType, curMatchAmount) Then
                    AddSearchMatch(objTrx, lngIndex, curMatchAmount)
                End If
            End If
        Next
    End Sub

    Private Sub cmdAddToTotal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddToTotal.Click
        Dim dblAmount As Double
        'UPGRADE_WARNING: Couldn't resolve default property of object blnValidAmount(dblAmount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If blnValidAmount(dblAmount) Then
            mcurAmountTotal = mcurAmountTotal + dblAmount
            ShowTotals()
        End If
    End Sub

    Private Sub cmdMultTotalBy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMultTotalBy.Click
        Dim dblAmount As Double
        'UPGRADE_WARNING: Couldn't resolve default property of object blnValidAmount(dblAmount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If blnValidAmount(dblAmount) Then
            mcurAmountTotal = mcurAmountTotal * dblAmount
            ShowTotals()
        End If
    End Sub

    Private Function blnValidAmount(ByRef dblAmount As Double) As Boolean
        If Len(Trim(txtAddMultAmount.Text)) = 0 Then
            MsgBox("Add/Mult amount is required.", MsgBoxStyle.Critical)
            Exit Function
        End If
        If Not IsNumeric(txtAddMultAmount.Text) Then
            MsgBox("Invalid Add/Mult amount.", MsgBoxStyle.Critical)
            Exit Function
        End If
        dblAmount = CDbl(txtAddMultAmount.Text)
        'UPGRADE_WARNING: Couldn't resolve default property of object blnValidAmount. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        blnValidAmount = True
    End Function

    Private Sub cmdClearTotal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClearTotal.Click
        mcurAmountMatched = 0
        mcurAmountTotal = 0
        ShowTotals()
    End Sub

    Private Sub cmdTotalToClipboard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdTotalToClipboard.Click
        My.Computer.Clipboard.SetText(gstrFormatCurrency(mcurAmountTotal))
    End Sub

    Private Sub ShowTotals()
        lblTotalDollars.Text = "Matched $" & gstrFormatCurrency(mcurAmountMatched) & "    Total $" & gstrFormatCurrency(mcurAmountTotal)
    End Sub

    Private Sub AddSearchMatch(ByVal objTrx As Trx, ByVal lngIndex As Integer, ByVal curMatchAmount As Decimal)

        Dim objItem As System.Windows.Forms.ListViewItem
        Dim strCategory As String = ""
        Dim strPONumber As String = ""
        Dim strInvoiceNum As String = ""
        Dim strInvoiceDate As String = ""
        Dim strDueDate As String = ""
        Dim strTerms As String = ""
        Dim strBudget As String = ""
        Dim curAvailable As Decimal

        If mlngMatchesUsed >= mlngMatchesAlloc Then
            mlngMatchesAlloc = mlngMatchesAlloc + 5
            'UPGRADE_WARNING: Lower bound of array maudtMatches was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim Preserve maudtMatches(mlngMatchesAlloc)
        End If
        mlngMatchesUsed = mlngMatchesUsed + 1
        With maudtMatches(mlngMatchesUsed)
            .lngRegIndex = lngIndex
        End With

        objItem = gobjListViewAdd(lvwMatches)
        With objItem
            .Text = gstrFormatDate(objTrx.datDate)
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > 1 Then
                objItem.SubItems(1).Text = objTrx.strNumber
            Else
                objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > 2 Then
                objItem.SubItems(2).Text = objTrx.strDescription
            Else
                objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > 3 Then
                objItem.SubItems(3).Text = gstrFormatCurrency(curMatchAmount)
            Else
                objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(curMatchAmount)))
            End If
            If objTrx.lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                gSummarizeSplits(objTrx, strCategory, strPONumber, strInvoiceNum, strInvoiceDate, strDueDate, strTerms, strBudget, curAvailable)
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 4 Then
                    objItem.SubItems(4).Text = gstrFormatCurrency(curAvailable)
                Else
                    objItem.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(curAvailable)))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 5 Then
                    objItem.SubItems(5).Text = strCategory
                Else
                    objItem.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strCategory))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 6 Then
                    objItem.SubItems(6).Text = strPONumber
                Else
                    objItem.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strPONumber))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 7 Then
                    objItem.SubItems(7).Text = strInvoiceNum
                Else
                    objItem.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strInvoiceNum))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 8 Then
                    objItem.SubItems(8).Text = strInvoiceDate
                Else
                    objItem.SubItems.Insert(8, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strInvoiceDate))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 9 Then
                    objItem.SubItems(9).Text = strDueDate
                Else
                    objItem.SubItems.Insert(9, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strDueDate))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 10 Then
                    objItem.SubItems(10).Text = strTerms
                Else
                    objItem.SubItems.Insert(10, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTerms))
                End If
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objItem.SubItems.Count > 11 Then
                    objItem.SubItems(11).Text = objTrx.strFakeStatus
                Else
                    objItem.SubItems.Insert(11, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strFakeStatus))
                End If
            Else
                objItem.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(8, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(9, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(10, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                objItem.SubItems.Insert(11, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintHIDDEN_COL Then
                objItem.SubItems(mintHIDDEN_COL).Text = CStr(mlngMatchesUsed)
            Else
                objItem.SubItems.Insert(mintHIDDEN_COL, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(mlngMatchesUsed)))
            End If
        End With
        mcurAmountMatched = mcurAmountMatched + curMatchAmount

    End Sub

    Private Sub ClearResults()
        lvwMatches.Items.Clear()
        mlngMatchesUsed = 0
        mlngMatchesAlloc = 0
        mcurAmountMatched = 0
    End Sub

    Private Sub lvwMatches_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwMatches.Click
        Dim lngResultIndex As Integer

        Try

            If lvwMatches.FocusedItem Is Nothing Then
                Exit Sub
            End If
            'UPGRADE_WARNING: Lower bound of collection lvwMatches.SelectedItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            lngResultIndex = CInt(lvwMatches.FocusedItem.SubItems(mintHIDDEN_COL).Text)
            mobjReg.SetCurrent(maudtMatches(lngResultIndex).lngRegIndex)
            mobjReg.ShowCurrent_Renamed()
            RememberCheckedAndSelected()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lvwMatches_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwMatches.DoubleClick
        cmdEditTrx_Click(cmdEditTrx, New System.EventArgs())
    End Sub

    Private Sub lvwMatches_ItemCheck(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ItemCheckEventArgs) Handles lvwMatches.ItemCheck
        Dim Item As System.Windows.Forms.ListViewItem = lvwMatches.Items(eventArgs.Index)
        Try

            RememberCheckedAndSelected()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem

            For Each objItem In lvwMatches.Items
                objItem.Checked = Not objItem.Checked
            Next objItem
            RememberCheckedAndSelected()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdEditTrx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditTrx.Click
        Try

            Dim lngResultIndex As Integer
            Dim lngRegSelect As Integer
            Dim frmEdit As TrxForm

            If lvwMatches.FocusedItem Is Nothing Then
                Exit Sub
            End If
            'UPGRADE_WARNING: Lower bound of collection lvwMatches.SelectedItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            lngResultIndex = CInt(lvwMatches.FocusedItem.SubItems(mintHIDDEN_COL).Text)
            lngRegSelect = maudtMatches(lngResultIndex).lngRegIndex
            frmEdit = frmCreateTrxForm()
            If frmEdit.blnUpdate(mobjAccount, lngRegSelect, mobjReg, mdatDefaultDate, "SearchForm.Edit") Then
                Exit Sub
            End If
            mobjReg.ValidateRegister()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdNewNormal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewNormal.Click
        Dim frm As TrxForm
        Dim objTrx As Trx

        Try

            objTrx = New Trx
            objTrx.NewEmptyNormal(mobjReg, mdatDefaultDate)
            frm = frmCreateTrxForm()
            If frm.blnAddNormal(mobjAccount, mobjReg, objTrx, mdatDefaultDate, True, "SearchForm.NewNormal") Then
                MsgBox("Canceled.")
            End If
            mobjReg.ValidateRegister()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function frmCreateTrxForm() As TrxForm
        Dim frm As TrxForm
        frm = New TrxForm
        'If chkBypassConfirmation.value = vbChecked Then
        '    frm.blnBypassConfirmation = True
        'End If
        frmCreateTrxForm = frm
    End Function

    Private Sub cmdExport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExport.Click
        Dim frmExport As ExportForm
        frmExport = New ExportForm
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim lngTrxIndex As Integer
            Dim objTrx As Trx
            Dim colSplits As Collection
            Dim objSplit As Split_Renamed
            Dim lngExportCount As Short

            If Not frmExport.blnGetSettings() Then
                MsgBox("Export canceled.")
                Exit Sub
            End If

            frmExport.OpenOutput()

            lngExportCount = 0
            For Each objItem In lvwMatches.Items
                If objItem.Checked Then
                    'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    lngTrxIndex = maudtMatches(CInt(objItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
                    objTrx = mobjReg.objTrx(lngTrxIndex)
                    'Ignore budgets and transfers instead of showing an error, because
                    'it is common to export all trx in a date range except these.
                    If objTrx.lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                        colSplits = objTrx.colSplits
                        For Each objSplit In colSplits
                            frmExport.WriteSplit(objTrx, objSplit)
                            lngExportCount = lngExportCount + 1
                        Next objSplit
                    End If
                End If
            Next objItem

            frmExport.CloseOutput()
            frmExport.Close()

            MsgBox(lngExportCount & " transactions exported.", MsgBoxStyle.Critical)

            Exit Sub
        Catch ex As Exception
            If Not frmExport Is Nothing Then
                frmExport.Close()
            End If
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdRecategorize_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRecategorize.Click
        Try

            Dim frmArgs As ChangeCategoryForm
            Dim strOldCatKey As String = ""
            Dim strNewCatKey As String = ""
            Dim colTrx As Collection
            Dim objItem As System.Windows.Forms.ListViewItem
            Dim lngTrxIndex As Integer
            Dim objTrx As Trx
            Dim objTrxOld As Trx
            Dim colSplits As Collection
            Dim objSplit As Split_Renamed
            Dim strCatKey As String
            Dim lngChgCount As Integer
            Dim objStartLogger As ILogGroupStart

            frmArgs = New ChangeCategoryForm
            If Not frmArgs.blnGetCategories(strOldCatKey, strNewCatKey) Then
                Exit Sub
            End If

            colTrx = New Collection
            For Each objItem In lvwMatches.Items
                If objItem.Checked Then
                    'Get the trx and validate it.
                    'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    lngTrxIndex = maudtMatches(CInt(objItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
                    objTrx = mobjReg.objTrx(lngTrxIndex)
                    If objTrx.lngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                        MsgBox("Budgets and transfers may not be recategorized.", MsgBoxStyle.Critical)
                        Exit Sub
                    End If
                    If objTrx.blnAutoGenerated Then
                        MsgBox("Generated transactions may not be recategorized.", MsgBoxStyle.Critical)
                        Exit Sub
                    End If
                    colSplits = objTrx.colSplits
                    For Each objSplit In colSplits
                        With objSplit
                            If objSplit.strCategoryKey = strOldCatKey Then
                                colTrx.Add(objTrx)
                                Exit For
                            End If
                        End With
                    Next objSplit
                End If
            Next objItem
            If colTrx.Count() = 0 Then
                MsgBox("No transactions selected, or none that use the old category.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            objStartLogger = mobjReg.objLogGroupStart("SearchForm.Recategorize")
            For Each objTrx In colTrx
                objTrxOld = objTrx.objClone(Nothing)
                lngTrxIndex = mobjReg.lngTrxIndex(objTrx)
                colSplits = objTrx.colSplits
                objTrx.UpdateStartNormal(mobjReg, objTrx)
                For Each objSplit In colSplits
                    With objSplit
                        strCatKey = objSplit.strCategoryKey
                        If strCatKey = strOldCatKey Then
                            strCatKey = strNewCatKey
                        End If
                        objTrx.AddSplit(.strMemo, strCatKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount, .strImageFiles)
                    End With
                Next objSplit
                'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                mobjReg.UpdateEnd(lngTrxIndex, New LogChange, "SearchForm.Recategorize", objTrxOld)
                lngChgCount = lngChgCount + 1
            Next objTrx
            mobjReg.LogGroupEnd(objStartLogger)

            MsgBox("Changed category of " & lngChgCount & " transactions.")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdCombine_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCombine.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim objNewTrx As Trx = Nothing
            Dim objOldTrx As Trx
            Dim colOldTrx As Collection
            Dim objOldSplit As Split_Renamed
            Dim lngTrxIndex As Integer
            Dim frmTrx As TrxForm
            Dim objStartLogger As ILogGroupStart
            Dim datToday As Date
            Dim datResult As Date

            'Build the new Trx from selected (checked) Trx.
            'Use the first Trx for Trx level data, and clone the splits of all Trx.
            'Keep a collection of the chosen Trx, to delete them at the end.
            colOldTrx = New Collection
            For Each objItem In lvwMatches.Items
                If objItem.Checked Then
                    'Get the old trx and validate it.
                    'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    lngTrxIndex = maudtMatches(CInt(objItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
                    objOldTrx = mobjReg.objTrx(lngTrxIndex)
                    If Not blnValidTrxForBulkOperation(objOldTrx, "combined") Then
                        Exit Sub
                    End If
                    'If we do not yet have a new trx, create it.
                    If objNewTrx Is Nothing Then
                        objNewTrx = New Trx
                        datToday = Today
                        objNewTrx.NewStartNormal(mobjReg, "", datToday, objOldTrx.strDescription, objOldTrx.strMemo, Trx.TrxStatus.glngTRXSTS_UNREC, New TrxGenImportData())
                    End If
                    'Remember the old Trx to delete later if the new Trx is saved.
                    'Remember the Trx object instead of its index because the index may change
                    'as the result of saving the new Trx or deleting other old ones.
                    colOldTrx.Add(objOldTrx)
                    'Clone all the splits in old trx and add them to new trx.
                    For Each objOldSplit In objOldTrx.colSplits
                        With objOldSplit
                            objNewTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount, .strImageFiles)
                        End With
                    Next objOldSplit
                End If
            Next objItem
            If colOldTrx.Count() = 0 Then
                MsgBox("No transactions selected.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            'Now let them edit it and possibly save it.
            frmTrx = frmCreateTrxForm()

            If frmTrx.blnAddNormal(mobjAccount, mobjReg, objNewTrx, datResult, False, "SearchForm.CombineNew") Then
                'They did not save it.
                Exit Sub
            End If
            objNewTrx = mobjReg.objTrx(mobjReg.lngCurrentTrxIndex())

            'Now delete old trx.
            'Because we start from the Trx object instead of its index, we don't need
            'to worry if saving the new trx or a prior delete changed the index of a Trx.
            objStartLogger = mobjReg.objLogGroupStart("SearchForm.CombineDelete")
            For Each objOldTrx In colOldTrx
                lngTrxIndex = mobjReg.lngTrxIndex(objOldTrx)
                'UPGRADE_WARNING: Couldn't resolve default property of object New (LogDelete). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                mobjReg.Delete(lngTrxIndex, New LogDelete, "SearchForm.CombineDeleteTrx")
            Next objOldTrx
            mobjReg.LogGroupEnd(objStartLogger)

            mobjReg.SetCurrent(mobjReg.lngTrxIndex(objNewTrx))
            mobjReg.ShowCurrent_Renamed()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdMove_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMove.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim lngTrxIndex As Integer
            Dim objTrxSrc As Trx
            Dim objTrxOld As Trx
            Dim objTrxNew As Trx
            Dim objTrxFirst As Trx = Nothing
            Dim colTrx As Collection
            Dim colSplits As Collection
            Dim objSplit As Split_Renamed
            Dim strNewDate As String = ""
            Dim objNewReg As Register = Nothing
            Dim datExplicitDate As Date
            Dim blnUseDayOffset As Boolean
            Dim intDayOffset As Short
            Dim datNewDate As Date
            Dim frmMoveTo As MoveDstForm

            colTrx = New Collection
            For Each objItem In lvwMatches.Items
                If objItem.Checked Then
                    'Get the trx and validate it.
                    'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    lngTrxIndex = maudtMatches(CInt(objItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
                    objTrxSrc = mobjReg.objTrx(lngTrxIndex)
                    If Not blnValidTrxForBulkOperation(objTrxSrc, "moved") Then
                        Exit Sub
                    End If
                    colTrx.Add(objTrxSrc)
                End If
            Next objItem
            If colTrx.Count() = 0 Then
                MsgBox("No transactions selected.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            frmMoveTo = New MoveDstForm
            If Not frmMoveTo.blnShowModal(mobjAccount.colLoadedRegisters, mobjReg, strNewDate, objNewReg) Then
                Exit Sub
            End If
            If gblnValidDate(strNewDate) Then
                datExplicitDate = CDate(strNewDate)
                blnUseDayOffset = False
            ElseIf IsNumeric(strNewDate) Or strNewDate = "" Then
                intDayOffset = Val(strNewDate)
                blnUseDayOffset = True
            Else
                'Should never get here.
                MsgBox("Invalid date or number of days.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            mblnIgnoreTrxUpdates = True

            Dim objStartLogger As ILogGroupStart
            objStartLogger = mobjReg.objLogGroupStart("SearchForm.Move")
            For Each objTrxSrc In colTrx
                lngTrxIndex = mobjReg.lngTrxIndex(objTrxSrc)
                If blnUseDayOffset Then
                    datNewDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDayOffset, objTrxSrc.datDate)
                Else
                    datNewDate = datExplicitDate
                End If
                With objTrxSrc
                    If objNewReg Is Nothing Then
                        'Changing date, not register.
                        objTrxOld = objTrxSrc.objClone(Nothing)
                        colSplits = objTrxSrc.colSplits
                        'This will clear the splits collection.
                        Dim objTempTrx As Trx = objTrxSrc.objClone(mobjReg)
                        objTempTrx.SetDate(datNewDate)
                        .UpdateStartNormal(mobjReg, objTempTrx)
                        'Add the splits back.
                        For Each objSplit In colSplits
                            .objAddSplit(objSplit)
                        Next objSplit
                        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogMove). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        mobjReg.UpdateEnd(lngTrxIndex, New LogMove, "SearchForm.MoveUpdate", objTrxOld)
                        If objTrxFirst Is Nothing Then
                            objTrxFirst = objTrxSrc
                        End If
                    Else
                        'Changing register, and possibly date.
                        objTrxNew = New Trx
                        Dim objTempTrx As Trx = objTrxSrc.objClone(mobjReg)
                        objTempTrx.SetDate(datNewDate)
                        objTrxNew.NewStartNormal(objNewReg, objTempTrx)
                        .CopySplits(objTrxNew)
                        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogAdd). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        objNewReg.NewAddEnd(objTrxNew, New LogAdd, "SearchForm.MoveAdd")
                        If objTrxFirst Is Nothing Then
                            objTrxFirst = objTrxNew
                        End If
                        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogDelete). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        mobjReg.Delete(lngTrxIndex, New LogDelete, "SearchForm.MoveDelete")
                    End If
                End With
            Next objTrxSrc
            mobjReg.LogGroupEnd(objStartLogger)

            mblnIgnoreTrxUpdates = False

            If Not objTrxFirst Is Nothing Then
                If objNewReg Is Nothing Then
                    mobjReg.SetCurrent(mobjReg.lngTrxIndex(objTrxFirst))
                    mobjReg.ShowCurrent_Renamed()
                Else
                    objNewReg.SetCurrent(objNewReg.lngTrxIndex(objTrxFirst))
                    objNewReg.ShowCurrent_Renamed()
                End If
            End If

            RedoSearch()

            Exit Sub
        Catch ex As Exception
            mblnIgnoreTrxUpdates = False
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidTrxForBulkOperation(ByVal objTrx As Trx, ByVal strOperation As String) As Boolean

        blnValidTrxForBulkOperation = False
        If objTrx.lngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
            MsgBox("Budgets and transfers may not be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        If Not objTrx.blnFake Then
            MsgBox("Only fake transactions may be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        If objTrx.blnAutoGenerated Then
            MsgBox("Generated transactions may not be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        If objTrx.strRepeatKey <> "" Then
            MsgBox("Transactions in a repeat sequence may not be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        blnValidTrxForBulkOperation = True

    End Function

    Private Sub mobjReg_RedisplayTrx() Handles mobjReg.RedisplayTrx
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxAdded(ByVal lngIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxAdded
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxDeleted(ByVal lngIndex As Integer) Handles mobjReg.TrxDeleted
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxUpdated(ByVal lngOldIndex As Integer, ByVal lngNewIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxUpdated
        If Not mblnIgnoreTrxUpdates Then
            RedoSearch()
        End If
    End Sub

    Private Sub RedoSearch()
        ClearResults()
        SearchInternal()
        RestoreCheckedAndSelected()
    End Sub

    Private Sub RememberCheckedAndSelected()

        Dim objItem As System.Windows.Forms.ListViewItem
        Dim lngTrxIndex As Integer
        Dim objCheckedTrx As Trx

        'Remember the results that are checked.
        colCheckedTrx = New Collection
        For Each objItem In lvwMatches.Items
            If objItem.Checked Then
                'Get the trx and validate it.
                'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                lngTrxIndex = maudtMatches(CInt(objItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
                objCheckedTrx = mobjReg.objTrx(lngTrxIndex)
                colCheckedTrx.Add(objCheckedTrx)
            End If
        Next objItem
        'Remember the result that is selected.
        'UPGRADE_NOTE: Object objSelectedTrx may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSelectedTrx = Nothing
        If Not lvwMatches.FocusedItem Is Nothing Then
            'UPGRADE_WARNING: Lower bound of collection lvwMatches.SelectedItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            lngTrxIndex = maudtMatches(CInt(lvwMatches.FocusedItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
            objSelectedTrx = mobjReg.objTrx(lngTrxIndex)
        End If
    End Sub

    Private Sub RestoreCheckedAndSelected()

        Dim objItem As System.Windows.Forms.ListViewItem
        Dim lngTrxIndex As Integer
        Dim objTrx As Trx
        Dim objCheckedTrx As Trx

        For Each objItem In lvwMatches.Items
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            lngTrxIndex = maudtMatches(CInt(objItem.SubItems(mintHIDDEN_COL).Text)).lngRegIndex
            objTrx = mobjReg.objTrx(lngTrxIndex)
            For Each objCheckedTrx In colCheckedTrx
                If objCheckedTrx Is objTrx Then
                    objItem.Checked = True
                    Exit For
                End If
            Next objCheckedTrx
            If objTrx Is objSelectedTrx Then
                lvwMatches.FocusedItem = objItem
                'UPGRADE_WARNING: MSComctlLib.ListItem method objItem.EnsureVisible has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
                objItem.EnsureVisible()
            End If
        Next objItem
    End Sub
End Class
Option Strict On
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Friend Class ReconcileForm
    Inherits System.Windows.Forms.Form

    Private mobjAccount As Account

    'Describe one normal, non-fake Trx in mobjAccount which is not already reconciled.
    Private Structure ReconTrx
        'Register containing the Trx.
        Dim objReg As Register
        'Index of Trx in objReg.
        'NOTE: There is no mechanism to keep this index accurate if any Register
        'in mobjAccount is modified during reconciliation, which means such
        'operations must not be allowed. We do this by making this form modal.
        Dim lngIndex As Integer
        'True iff Trx is selected in this reconciliation.
        Dim blnSelected As Boolean
        Dim objLvwItem As ListViewItem
        Dim datBankDate As DateTime?
    End Structure

    Private maudtTrx() As ReconTrx
    Private mlngTrxUsed As Integer
    Private mlngTrxAllocated As Integer
    Private mcurClearedBalance As Decimal

    Private Const mintCOL_DATE As Short = 1
    Private Const mintCOL_NUMBER As Short = 2
    Private Const mintCOL_DESCRIPTION As Short = 3
    Private Const mintCOL_AMOUNT As Short = 4
    Private Const mintCOL_IMPORTED As Short = 5
    Private Const mintCOL_BANK_DATE As Short = 6
    Private Const mintCOL_SORTABLE_DATE As Short = 7
    Private Const mintCOL_SORTABLE_NUMBER As Short = 8
    Private Const mintCOL_SORTABLE_BANK_DATE As Short = 9
    Private Const mintCOL_ARRAY_INDEX As Short = 10

    Private Const mstrREG_ENDING_BAL As String = "Ending Balances"

    Public Sub ShowMe(ByVal objAccount_ As Account)
        mobjAccount = objAccount_
        'This form must be modal because ReconTrx has a Register index,
        'which cannot change.
        Me.ShowDialog()
    End Sub

    Private Sub ReconcileForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try

            LoadTrx()
            txtEndingBalance.Text = GetSetting(gstrREG_APP, mstrREG_ENDING_BAL, mobjAccount.strTitle)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub LoadTrx()
        Dim lngIndex As Integer
        Dim objReg As Register
        Dim lngMaxRegIndex As Integer
        Dim objTrx As Trx
        Dim curStartingBalance As Decimal
        Dim curSelectedTotal As Decimal

        lvwTrx.Items.Clear()
        mlngTrxAllocated = 10
        mlngTrxUsed = 0
        ReDim maudtTrx(mlngTrxAllocated)

        For Each objReg In mobjAccount.colRegisters
            lngMaxRegIndex = objReg.lngTrxCount
            For lngIndex = 1 To lngMaxRegIndex
                objTrx = objReg.objTrx(lngIndex)
                With objTrx
                    If .lngType = Trx.TrxType.glngTRXTYP_NORMAL And Not .blnFake Then
                        If .lngStatus = Trx.TrxStatus.glngTRXSTS_RECON Then
                            curStartingBalance = curStartingBalance + .curAmount
                        Else
                            mlngTrxUsed = mlngTrxUsed + 1
                            If mlngTrxUsed > mlngTrxAllocated Then
                                mlngTrxAllocated = mlngTrxAllocated + 10
                                ReDim Preserve maudtTrx(mlngTrxAllocated)
                            End If
                            With maudtTrx(mlngTrxUsed)
                                .objReg = objReg
                                .lngIndex = lngIndex
                                .blnSelected = (objTrx.lngStatus = Trx.TrxStatus.glngTRXSTS_SELECTED)
                                If .blnSelected Then
                                    curSelectedTotal = curSelectedTotal + objTrx.curAmount
                                End If
                            End With
                            DisplayTrx(DirectCast(objTrx, NormalTrx), mlngTrxUsed)
                        End If
                    End If
                End With
            Next
        Next objReg

        txtStartingBalance.Text = Utilities.strFormatCurrency(curStartingBalance)
        mcurClearedBalance = curStartingBalance + curSelectedTotal
        txtClearedBalance.Text = Utilities.strFormatCurrency(mcurClearedBalance)
        gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_NUMBER)

    End Sub

    Private Sub DisplayTrx(ByVal objTrx As NormalTrx, ByVal lngTrxIndex As Integer)
        Dim objItem As System.Windows.Forms.ListViewItem
        Dim intPipe2 As Integer
        Dim datBankDate As DateTime
        Dim strBankDate As String
        Dim strSortableBankDate As String
        Dim strSortableNumber As String

        objItem = gobjListViewAdd(lvwTrx)
        maudtTrx(lngTrxIndex).objLvwItem = objItem
        With objItem
            AddSubItem(objItem, mintCOL_DATE, Utilities.strFormatDate(objTrx.datDate))
            AddSubItem(objItem, mintCOL_NUMBER, objTrx.strNumber)
            AddSubItem(objItem, mintCOL_DESCRIPTION, objTrx.strDescription)
            AddSubItem(objItem, mintCOL_AMOUNT, Utilities.strFormatCurrency(objTrx.curAmount))
            AddSubItem(objItem, mintCOL_IMPORTED, CStr(IIf(objTrx.strImportKey = "", "", "Y")))
            intPipe2 = InStr(2, objTrx.strImportKey, "|")
            strSortableBankDate = ""
            maudtTrx(lngTrxIndex).datBankDate = Nothing
            If intPipe2 > 0 Then
                strBankDate = Mid(objTrx.strImportKey, 2, intPipe2 - 2)
                If DateTime.TryParse(strBankDate, datBankDate) Then
                    strSortableBankDate = Utilities.strFormatDate(datBankDate)
                    maudtTrx(lngTrxIndex).datBankDate = datBankDate
                End If
            Else
                strBankDate = ""
            End If
            AddSubItem(objItem, mintCOL_BANK_DATE, strBankDate)
            AddSubItem(objItem, mintCOL_SORTABLE_DATE, objTrx.datDate.ToString("yyyyMMdd"))
            If IsNumeric(objTrx.strNumber) Then
                strSortableNumber = VB.Right("          " & objTrx.strNumber, 10)
            Else
                strSortableNumber = objTrx.strNumber.ToUpper()
            End If
            AddSubItem(objItem, mintCOL_SORTABLE_NUMBER, strSortableNumber)
            AddSubItem(objItem, mintCOL_SORTABLE_BANK_DATE, strSortableBankDate)
            AddSubItem(objItem, mintCOL_ARRAY_INDEX, CStr(mlngTrxUsed))
            .Checked = (objTrx.lngStatus = Trx.TrxStatus.glngTRXSTS_SELECTED)
        End With
    End Sub

    Private Sub AddSubItem(ByVal objItem As ListViewItem, ByVal intColIndex As Integer, ByVal strValue As String)
        If objItem.SubItems.Count > intColIndex Then
            objItem.SubItems(intColIndex).Text = strValue
        Else
            objItem.SubItems.Insert(intColIndex, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strValue))
        End If
    End Sub

    Private Sub lvwTrx_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles lvwTrx.ColumnClick
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lvwTrx.Columns(eventArgs.Column)
        Try

            With lvwTrx
                Select Case ColumnHeader.Index
                    Case mintCOL_DATE
                        gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_DATE)
                        .Refresh()
                    Case mintCOL_NUMBER
                        gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_NUMBER)
                        .Refresh()
                    Case mintCOL_DESCRIPTION
                        gSetListViewSortColumn(lvwTrx, mintCOL_DESCRIPTION)
                        .Refresh()
                    Case mintCOL_BANK_DATE
                        gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_BANK_DATE)
                        .Refresh()
                End Select
            End With

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lvwTrx_ItemCheck(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ItemCheckEventArgs) Handles lvwTrx.ItemCheck
        Dim Item As System.Windows.Forms.ListViewItem = lvwTrx.Items(eventArgs.Index)
        Dim lngArrayIndex As Integer
        Dim objTrx As NormalTrx

        Try
            lngArrayIndex = CInt(Item.SubItems(mintCOL_ARRAY_INDEX).Text)
            With maudtTrx(lngArrayIndex)
                objTrx = .objReg.objNormalTrx(.lngIndex)
                'Item.Checked still has the OLD value, unlike in VB6.
                If Not Item.Checked Then
                    mcurClearedBalance = mcurClearedBalance + objTrx.curAmount
                    .blnSelected = True
                Else
                    mcurClearedBalance = mcurClearedBalance - objTrx.curAmount
                    .blnSelected = False
                End If
            End With
            txtClearedBalance.Text = Utilities.strFormatCurrency(mcurClearedBalance)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    'UPGRADE_ISSUE: MSComctlLib.ListView event lvwTrx.ItemClick was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"'
    'Private Sub lvwTrx_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
    'Fired twice if an item is already selected - first for old item, then for new
    Private Sub lvwTrx_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTrx.SelectedIndexChanged
        Try
            Dim lvw As ListView
            Dim Item As ListViewItem
            lvw = DirectCast(sender, ListView)
            If lvw.SelectedIndices.Count > 0 Then
                Item = lvw.FocusedItem
                'Fires the "ItemCheck" event
                Item.Checked = Not Item.Checked
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdFinish_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFinish.Click
        Try

            If txtClearedBalance.Text <> txtEndingBalance.Text Then
                MsgBox("The cleared balance is not equal to the ending statement balance.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            SaveChanges(Trx.TrxStatus.glngTRXSTS_RECON)
            SaveSetting(gstrREG_APP, mstrREG_ENDING_BAL, mobjAccount.strTitle, "")

            MsgBox("Congratulations!" & vbCrLf & vbCrLf & "You have reconciled your account to the ending balance " & "on your bank statement. This means that the total of transactions marked as " & "reconciled in the software equals the bank statement ending balance.", MsgBoxStyle.Information)
            Me.Close()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdLater_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLater.Click
        Try

            SaveChanges(Trx.TrxStatus.glngTRXSTS_SELECTED)
            SaveSetting(gstrREG_APP, mstrREG_ENDING_BAL, mobjAccount.strTitle, txtEndingBalance.Text)

            MsgBox("Your work has been saved. To resume this reconciliation, just " & "reconcile normally. The software will remember what you have already done.", MsgBoxStyle.Information)

            Me.Close()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub SaveChanges(ByVal lngSelectedStatus As Trx.TrxStatus)
        Dim lngIndex As Integer
        Dim lngNewStatus As Trx.TrxStatus

        For lngIndex = 1 To mlngTrxUsed
            With maudtTrx(lngIndex)
                lngNewStatus = CType(IIf(.blnSelected, lngSelectedStatus, Trx.TrxStatus.glngTRXSTS_UNREC), Trx.TrxStatus)
                If .objReg.objTrx(.lngIndex).lngStatus <> lngNewStatus Then
                    .objReg.SetTrxStatus(.lngIndex, lngNewStatus, New LogStatus, "ReconcileForm.SaveChanges")
                End If
            End With
        Next

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSelectThroughDate_Click(sender As Object, e As EventArgs) Handles btnSelectThroughDate.Click
        Dim lngIndex As Integer
        Dim datCutoff As DateTime
        Dim blnSelect As Boolean
        Dim intSelectCount As Integer

        If Not IsDate(txtSelectThroughDate.Text) Then
            MsgBox("Invalid select-through date", MsgBoxStyle.Exclamation)
            Return
        End If
        datCutoff = CDate(txtSelectThroughDate.Text)

        intSelectCount = 0
        For lngIndex = 1 To mlngTrxUsed
            With maudtTrx(lngIndex)
                With maudtTrx(lngIndex)
                    If .datBankDate.HasValue Then
                        blnSelect = (.datBankDate.Value <= datCutoff)
                    Else
                        blnSelect = False
                    End If
                    If blnSelect Then
                        intSelectCount = intSelectCount + 1
                    End If
                    If .objLvwItem.Checked <> blnSelect Then
                        .objLvwItem.Checked = blnSelect
                    End If
                End With
            End With
        Next
        MsgBox("Selected " & intSelectCount & " transactions to clear.", MsgBoxStyle.Information)
    End Sub
End Class
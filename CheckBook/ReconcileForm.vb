Option Strict Off
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
    Private Const mintCOL_ARRAY_INDEX As Short = 9

    Private Const mstrREG_ENDING_BAL As String = "Ending Balances"

    Public Sub ShowMe(ByVal objAccount_ As Account)
        mobjAccount = objAccount_
        'This form must be modal because ReconTrx has a Register index,
        'which cannot change.
        Me.ShowDialog()
    End Sub

    Private Sub ReconcileForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        On Error GoTo ErrorHandler

        LoadTrx()
        txtEndingBalance.Text = GetSetting(gstrREG_APP, mstrREG_ENDING_BAL, mobjAccount.strTitle)

        Exit Sub
ErrorHandler:
        TopError("Form_Load")
    End Sub

    Private Sub LoadTrx()
        Dim lngIndex As Integer
        Dim objLoadedReg As LoadedRegister
        Dim objReg As Register
        Dim lngMaxRegIndex As Integer
        Dim objTrx As Trx
        Dim curStartingBalance As Decimal
        Dim curSelectedTotal As Decimal

        lvwTrx.Items.Clear()
        mlngTrxAllocated = 10
        mlngTrxUsed = 0
        'UPGRADE_WARNING: Lower bound of array maudtTrx was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        ReDim maudtTrx(mlngTrxAllocated)

        For Each objLoadedReg In mobjAccount.colLoadedRegisters
            objReg = objLoadedReg.objReg
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
                                'UPGRADE_WARNING: Lower bound of array maudtTrx was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
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
                            DisplayTrx(objTrx)
                        End If
                    End If
                End With
            Next
        Next objLoadedReg

        txtStartingBalance.Text = VB6.Format(curStartingBalance, gstrFORMAT_CURRENCY)
        mcurClearedBalance = curStartingBalance + curSelectedTotal
        txtClearedBalance.Text = VB6.Format(mcurClearedBalance, gstrFORMAT_CURRENCY)
        gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_NUMBER)

    End Sub

    Private Sub DisplayTrx(ByVal objTrx As Trx)
        Dim objItem As System.Windows.Forms.ListViewItem
        Dim intPipe2 As Short
        Dim strBankDate As String

        objItem = gobjListViewAdd(lvwTrx)
        With objItem
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_DATE Then
                objItem.SubItems(mintCOL_DATE).Text = VB6.Format(objTrx.datDate, gstrFORMAT_DATE)
            Else
                objItem.SubItems.Insert(mintCOL_DATE, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(objTrx.datDate, gstrFORMAT_DATE)))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_NUMBER Then
                objItem.SubItems(mintCOL_NUMBER).Text = objTrx.strNumber
            Else
                objItem.SubItems.Insert(mintCOL_NUMBER, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_DESCRIPTION Then
                objItem.SubItems(mintCOL_DESCRIPTION).Text = objTrx.strDescription
            Else
                objItem.SubItems.Insert(mintCOL_DESCRIPTION, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_AMOUNT Then
                objItem.SubItems(mintCOL_AMOUNT).Text = VB6.Format(objTrx.curAmount, gstrFORMAT_CURRENCY)
            Else
                objItem.SubItems.Insert(mintCOL_AMOUNT, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(objTrx.curAmount, gstrFORMAT_CURRENCY)))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_IMPORTED Then
                objItem.SubItems(mintCOL_IMPORTED).Text = IIf(objTrx.strImportKey = "", "", "Y")
            Else
                objItem.SubItems.Insert(mintCOL_IMPORTED, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, IIf(objTrx.strImportKey = "", "", "Y")))
            End If
            intPipe2 = InStr(2, objTrx.strImportKey, "|")
            If intPipe2 > 0 Then
                strBankDate = Mid(objTrx.strImportKey, 2, intPipe2 - 2)
            Else
                strBankDate = ""
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_BANK_DATE Then
                objItem.SubItems(mintCOL_BANK_DATE).Text = strBankDate
            Else
                objItem.SubItems.Insert(mintCOL_BANK_DATE, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strBankDate))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_SORTABLE_DATE Then
                objItem.SubItems(mintCOL_SORTABLE_DATE).Text = VB6.Format(objTrx.datDate, "yyyymmdd")
            Else
                objItem.SubItems.Insert(mintCOL_SORTABLE_DATE, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(objTrx.datDate, "yyyymmdd")))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_SORTABLE_NUMBER Then
                objItem.SubItems(mintCOL_SORTABLE_NUMBER).Text = VB.Right("          " & objTrx.strNumber, 10)
            Else
                objItem.SubItems.Insert(mintCOL_SORTABLE_NUMBER, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB.Right("          " & objTrx.strNumber, 10)))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > mintCOL_ARRAY_INDEX Then
                objItem.SubItems(mintCOL_ARRAY_INDEX).Text = CStr(mlngTrxUsed)
            Else
                objItem.SubItems.Insert(mintCOL_ARRAY_INDEX, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(mlngTrxUsed)))
            End If
            .Checked = (objTrx.lngStatus = Trx.TrxStatus.glngTRXSTS_SELECTED)
        End With
    End Sub

    Private Sub lvwTrx_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles lvwTrx.ColumnClick
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lvwTrx.Columns(eventArgs.Column)
        On Error GoTo ErrorHandler

        With lvwTrx
            Select Case ColumnHeader.Index - 1
                Case mintCOL_DATE
                    gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_DATE)
                    .Refresh()
                Case mintCOL_NUMBER
                    gSetListViewSortColumn(lvwTrx, mintCOL_SORTABLE_NUMBER)
                    .Refresh()
                Case mintCOL_DESCRIPTION
                    gSetListViewSortColumn(lvwTrx, mintCOL_DESCRIPTION)
                    .Refresh()
            End Select
        End With

        Exit Sub
ErrorHandler:
        TopError("lvwTrx_ColumnClick")
    End Sub

    Private Sub lvwTrx_ItemCheck(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ItemCheckEventArgs) Handles lvwTrx.ItemCheck
        Dim Item As System.Windows.Forms.ListViewItem = lvwTrx.Items(eventArgs.Index)
        Dim lngArrayIndex As Integer
        Dim objTrx As Trx

        On Error GoTo ErrorHandler
        'UPGRADE_WARNING: Lower bound of collection Item has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        lngArrayIndex = CInt(Item.SubItems(mintCOL_ARRAY_INDEX).Text)
        With maudtTrx(lngArrayIndex)
            objTrx = .objReg.objTrx(.lngIndex)
            'Item.Checked still has the OLD value, unlike in VB6.
            If Not Item.Checked Then
                mcurClearedBalance = mcurClearedBalance + objTrx.curAmount
                .blnSelected = True
            Else
                mcurClearedBalance = mcurClearedBalance - objTrx.curAmount
                .blnSelected = False
            End If
        End With
        txtClearedBalance.Text = VB6.Format(mcurClearedBalance, gstrFORMAT_CURRENCY)

        Exit Sub
ErrorHandler:
        TopError("lvwTrx_ItemCheck")
    End Sub

    'UPGRADE_ISSUE: MSComctlLib.ListView event lvwTrx.ItemClick was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"'
    'Private Sub lvwTrx_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
    'Fired twice if an item is already selected - first for old item, then for new
    Private Sub lvwTrx_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTrx.SelectedIndexChanged
        On Error GoTo ErrorHandler
        Dim lvw As ListView
        Dim Item As ListViewItem
        lvw = sender
        If lvw.SelectedIndices.Count > 0 Then
            Item = lvw.FocusedItem
            'Fires the "ItemCheck" event
            Item.Checked = Not Item.Checked
        End If

        Exit Sub
ErrorHandler:
        TopError("lvwTrx_SelectedIndexChanged")
    End Sub

    Private Sub cmdFinish_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFinish.Click
        On Error GoTo ErrorHandler

        If txtClearedBalance.Text <> txtEndingBalance.Text Then
            MsgBox("The cleared balance is not equal to the ending statement balance.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        SaveChanges(Trx.TrxStatus.glngTRXSTS_RECON)
        SaveSetting(gstrREG_APP, mstrREG_ENDING_BAL, mobjAccount.strTitle, "")

        MsgBox("Congratulations!" & vbCrLf & vbCrLf & "You have reconciled your account to the ending balance " & "on your bank statement. This means that the total of transactions marked as " & "reconciled in the software equals the bank statement ending balance.", MsgBoxStyle.Information)
        Me.Close()

        Exit Sub
ErrorHandler:
        TopError("cmdFinish_Click")
    End Sub

    Private Sub cmdLater_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLater.Click
        On Error GoTo ErrorHandler

        SaveChanges(Trx.TrxStatus.glngTRXSTS_SELECTED)
        SaveSetting(gstrREG_APP, mstrREG_ENDING_BAL, mobjAccount.strTitle, txtEndingBalance.Text)

        MsgBox("Your work has been saved. To resume this reconciliation, just " & "reconcile normally. The software will remember what you have already done.", MsgBoxStyle.Information)

        Me.Close()

        Exit Sub
ErrorHandler:
        TopError("cmdLater_Click")
    End Sub

    Private Sub SaveChanges(ByVal lngSelectedStatus As Trx.TrxStatus)
        Dim lngIndex As Integer
        Dim lngNewStatus As Trx.TrxStatus

        For lngIndex = 1 To mlngTrxUsed
            With maudtTrx(lngIndex)
                lngNewStatus = IIf(.blnSelected, lngSelectedStatus, Trx.TrxStatus.glngTRXSTS_UNREC)
                If .objReg.objTrx(.lngIndex).lngStatus <> lngNewStatus Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object New (LogStatus). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    .objReg.SetTrxStatus(.lngIndex, lngNewStatus, New LogStatus, "ReconcileForm.SaveChanges")
                End If
            End With
        Next

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("ReconcileFom." & strRoutine)
    End Sub
End Class
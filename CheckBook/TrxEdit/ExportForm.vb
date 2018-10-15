Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class ExportForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company

    Private mblnCancel As Short
    Private mstrOutputFile As String
    Private mintOutputFile As Short

    Private mblnIncludeAging As Boolean
    Private mdatAgingDate As Date
    Private mintAgingDays As Short

    Private mblnIncludeDue As Boolean
    Private mdatDueDate As Date
    Private mintDueDays As Short

    Private mblnIncludeTrans As Boolean
    Private mdatTransDate As Date
    Private mintTransDays As Short

    Private mblnIncludeInv As Boolean
    Private mdatInvDate As Date
    Private mintInvDays As Short

    Public Function blnGetSettings(ByVal objHostUI As IHostUI) As Boolean
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany
        mblnCancel = True
        mstrOutputFile = mobjCompany.strReportPath() & "\ExportSplits.csv"
        lblOutputFile.Text = "Will output to " & mstrOutputFile
        Me.ShowDialog()
        blnGetSettings = Not mblnCancel
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            If Not blnValidControls(chkIncludeAging, txtAgingDate, txtAgingDays, "aging", mblnIncludeAging, mdatAgingDate, mintAgingDays, False) Then
                Exit Sub
            End If

            If Not blnValidControls(chkIncludeTransDate, txtTransDate, txtTransDays, "transaction", mblnIncludeTrans, mdatTransDate, mintTransDays, True) Then
                Exit Sub
            End If

            If Not blnValidControls(chkIncludeDueDate, txtDueDate, txtDueDays, "due", mblnIncludeDue, mdatDueDate, mintDueDays, True) Then
                Exit Sub
            End If

            If Not blnValidControls(chkIncludeInvDate, txtInvDate, txtInvDays, "invoice", mblnIncludeInv, mdatInvDate, mintInvDays, True) Then
                Exit Sub
            End If

            mblnCancel = False
            Me.Hide()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidControls(ByVal chkInclude As System.Windows.Forms.CheckBox, ByVal txtDate As System.Windows.Forms.TextBox, ByVal txtDays As System.Windows.Forms.TextBox, ByVal strLabel As String, ByRef blnInclude As Boolean, ByRef datDate As Date, ByRef intDays As Short, ByVal blnAllowMonthPart As Boolean) As Boolean

        blnValidControls = False
        If chkInclude.CheckState = System.Windows.Forms.CheckState.Checked Then
            If Not Utilities.blnIsValidDate(txtDate.Text) Then
                If blnAllowMonthPart Then
                    If LCase(txtDate.Text) = "whole month" Then
                        intDays = -1
                        blnValidControls = True
                        blnInclude = True
                        Exit Function
                    End If
                    If LCase(txtDate.Text) = "half month" Then
                        intDays = -2
                        blnValidControls = True
                        blnInclude = True
                        Exit Function
                    End If
                    If LCase(txtDate.Text) = "quarter month" Then
                        intDays = -4
                        blnValidControls = True
                        blnInclude = True
                        Exit Function
                    End If
                End If
                mobjHostUI.InfoMessageBox("Invalid " & strLabel & " bracket start date.")
                Exit Function
            End If
            If Not ((txtDays.Text Like "#") Or (txtDays.Text Like "##") Or (txtDays.Text Like "###")) Then
                mobjHostUI.InfoMessageBox("Invalid number of days in " & strLabel & " bracket.")
                Exit Function
            End If
            blnInclude = True
            datDate = CDate(txtDate.Text)
            intDays = CShort(txtDays.Text)
            If intDays < 1 Then
                mobjHostUI.InfoMessageBox("Invalid number of days in " & strLabel & " bracket.")
                Exit Function
            End If
        Else
            blnInclude = False
        End If
        blnValidControls = True

    End Function

    Public Sub OpenOutput()
        Dim strExtraFields As String = ""
        'The order of these extra fields must match the order they
        'are added in WriteSplit().
        If mblnIncludeAging Then
            strExtraFields = strExtraFields & ",AgingBracket"
        End If
        If mblnIncludeTrans Then
            strExtraFields = strExtraFields & ",TransactionDateBracket"
        End If
        If mblnIncludeDue Then
            strExtraFields = strExtraFields & ",DueDateBracket"
        End If
        If mblnIncludeInv Then
            strExtraFields = strExtraFields & ",InvoiceDateBracket"
        End If
        mintOutputFile = FreeFile()
        FileOpen(mintOutputFile, mstrOutputFile, OpenMode.Output)
        PrintLine(mintOutputFile, "TransDate,Number,Payee,Amount,Category," & "DueDate,DueDateToUse,InvoiceDate,InvoiceDateToUse,PONumber,InvoiceNumber,Terms" & strExtraFields)
    End Sub

    Public Sub WriteSplit(ByVal objTrx As Trx, ByVal objSplit As TrxSplit)
        Dim strLine As String
        Dim datInvToUse As Date
        Dim datDueToUse As Date
        Dim strInvDate As String = ""
        Dim strDueDate As String = ""
        Dim strBracket As String

        Try

            gGetSplitDates(objTrx, objSplit, datInvToUse, datDueToUse)
            If objSplit.datInvoiceDate > System.DateTime.FromOADate(0) Then
                strInvDate = Utilities.strFormatDate(objSplit.datInvoiceDate)
            End If
            If objSplit.datDueDate > System.DateTime.FromOADate(0) Then
                strDueDate = Utilities.strFormatDate(objSplit.datDueDate)
            End If

            strLine = Utilities.strFormatDate(objTrx.datDate) & "," & objTrx.strNumber & ",""" & objTrx.strDescription & """," _
                & Utilities.strFormatCurrency(objSplit.curAmount) & ",""" & mobjCompany.objCategories.strKeyToValue1(objSplit.strCategoryKey) _
                & """," & strDueDate & "," & Utilities.strFormatDate(datDueToUse) & "," & strInvDate & "," _
                & Utilities.strFormatDate(datInvToUse) & ",""" & objSplit.strPONumber & """,""" & objSplit.strInvoiceNum & """,""" & objSplit.strTerms & """"

            'The order of these extra fields must match the order they
            'are added in OpenOutput().

            If mblnIncludeAging Then
                strBracket = AgingUtils.strMakeAgeBracket(mdatAgingDate, mintAgingDays, objTrx.blnFake, objTrx.datDate, datInvToUse, datDueToUse)
                strLine = strLine & ",""" & strBracket & """"
            End If

            If mblnIncludeTrans Then
                strBracket = AgingUtils.strMakeDateBracket(objTrx.datDate, mintTransDays, mdatTransDate)
                strLine = strLine & ",""" & strBracket & """"
            End If

            If mblnIncludeDue Then
                strBracket = AgingUtils.strMakeDateBracket(datDueToUse, mintDueDays, mdatDueDate)
                strLine = strLine & ",""" & strBracket & """"
            End If

            If mblnIncludeInv Then
                strBracket = AgingUtils.strMakeDateBracket(datInvToUse, mintInvDays, mdatInvDate)
                strLine = strLine & ",""" & strBracket & """"
            End If

            PrintLine(mintOutputFile, strLine)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub CloseOutput()
        FileClose(mintOutputFile)
    End Sub
End Class
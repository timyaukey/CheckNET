Option Strict Off
Option Explicit On


Friend Class ExportForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company

    Private mblnCancel As Short
    Private mstrOutputFile As String
    Private mobjOutputFile As IO.StreamWriter

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

    Public Function GetSettings(ByVal objHostUI As IHostUI) As Boolean
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.Company
        mblnCancel = True
        mstrOutputFile = mobjCompany.ReportsFolderPath() & "\ExportSplits.csv"
        lblOutputFile.Text = "Will output to " & mstrOutputFile
        Me.ShowDialog()
        GetSettings = Not mblnCancel
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            If Not IsValidControls(chkIncludeAging, txtAgingDate, txtAgingDays, "aging", mblnIncludeAging, mdatAgingDate, mintAgingDays, False) Then
                Exit Sub
            End If

            If Not IsValidControls(chkIncludeTransDate, txtTransDate, txtTransDays, "transaction", mblnIncludeTrans, mdatTransDate, mintTransDays, True) Then
                Exit Sub
            End If

            If Not IsValidControls(chkIncludeDueDate, txtDueDate, txtDueDays, "due", mblnIncludeDue, mdatDueDate, mintDueDays, True) Then
                Exit Sub
            End If

            If Not IsValidControls(chkIncludeInvDate, txtInvDate, txtInvDays, "invoice", mblnIncludeInv, mdatInvDate, mintInvDays, True) Then
                Exit Sub
            End If

            mblnCancel = False
            Me.Hide()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Function IsValidControls(ByVal chkInclude As System.Windows.Forms.CheckBox, ByVal txtDate As System.Windows.Forms.TextBox, ByVal txtDays As System.Windows.Forms.TextBox, ByVal strLabel As String, ByRef blnInclude As Boolean, ByRef datDate As Date, ByRef intDays As Short, ByVal blnAllowMonthPart As Boolean) As Boolean

        IsValidControls = False
        If chkInclude.CheckState = System.Windows.Forms.CheckState.Checked Then
            If Not Utilities.IsValidDate(txtDate.Text) Then
                If blnAllowMonthPart Then
                    If LCase(txtDate.Text) = "whole month" Then
                        intDays = -1
                        IsValidControls = True
                        blnInclude = True
                        Exit Function
                    End If
                    If LCase(txtDate.Text) = "half month" Then
                        intDays = -2
                        IsValidControls = True
                        blnInclude = True
                        Exit Function
                    End If
                    If LCase(txtDate.Text) = "quarter month" Then
                        intDays = -4
                        IsValidControls = True
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
        IsValidControls = True

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
        mobjOutputFile = New IO.StreamWriter(mstrOutputFile)
        mobjOutputFile.WriteLine("TransDate,Number,Payee,Amount,Category," & "DueDate,DueDateToUse,InvoiceDate,InvoiceDateToUse,PONumber,InvoiceNumber,Terms" & strExtraFields)
    End Sub

    Public Sub WriteSplit(ByVal objTrx As BaseTrx, ByVal objSplit As TrxSplit)
        Dim strLine As String
        Dim datInvToUse As Date
        Dim datDueToUse As Date
        Dim strInvDate As String = ""
        Dim strDueDate As String = ""
        Dim strBracket As String

        Try

            datInvToUse = objSplit.InvoiceDateEffective
            datDueToUse = objSplit.DueDateEffective
            If objSplit.InvoiceDate > Utilities.EmptyDate Then
                strInvDate = Utilities.FormatDate(objSplit.InvoiceDate)
            End If
            If objSplit.DueDate > Utilities.EmptyDate Then
                strDueDate = Utilities.FormatDate(objSplit.DueDate)
            End If

            strLine = Utilities.FormatDate(objTrx.TrxDate) & "," & objTrx.Number & ",""" & objTrx.Description & """," _
                & Utilities.FormatCurrency(objSplit.Amount) & ",""" & mobjCompany.Categories.KeyToValue1(objSplit.CategoryKey) _
                & """," & strDueDate & "," & Utilities.FormatDate(datDueToUse) & "," & strInvDate & "," _
                & Utilities.FormatDate(datInvToUse) & ",""" & objSplit.PONumber & """,""" & objSplit.InvoiceNum & """,""" & objSplit.Terms & """"

            'The order of these extra fields must match the order they
            'are added in OpenOutput().

            If mblnIncludeAging Then
                strBracket = AgingUtils.MakeAgeBracket(mdatAgingDate, mintAgingDays, objTrx.IsFake, objTrx.TrxDate, datInvToUse, datDueToUse)
                strLine = strLine & ",""" & strBracket & """"
            End If

            If mblnIncludeTrans Then
                strBracket = AgingUtils.MakeDateBracket(objTrx.TrxDate, mintTransDays, mdatTransDate)
                strLine = strLine & ",""" & strBracket & """"
            End If

            If mblnIncludeDue Then
                strBracket = AgingUtils.MakeDateBracket(datDueToUse, mintDueDays, mdatDueDate)
                strLine = strLine & ",""" & strBracket & """"
            End If

            If mblnIncludeInv Then
                strBracket = AgingUtils.MakeDateBracket(datInvToUse, mintInvDays, mdatInvDate)
                strLine = strLine & ",""" & strBracket & """"
            End If

            mobjOutputFile.WriteLine(strLine)

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Public Sub CloseOutput()
        mobjOutputFile.Close()
    End Sub
End Class
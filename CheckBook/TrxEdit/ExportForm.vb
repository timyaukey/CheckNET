Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class ExportForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

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

    Public Function blnGetSettings() As Boolean
        mblnCancel = True
        mstrOutputFile = gstrReportPath() & "\ExportSplits.csv"
        lblOutputFile.Text = "Will output to " & mstrOutputFile
        Me.ShowDialog()
        blnGetSettings = Not mblnCancel
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        On Error GoTo ErrorHandler

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
ErrorHandler:
        TopError("cmdOkay_Click")
    End Sub

    Private Function blnValidControls(ByVal chkInclude As System.Windows.Forms.CheckBox, ByVal txtDate As System.Windows.Forms.TextBox, ByVal txtDays As System.Windows.Forms.TextBox, ByVal strLabel As String, ByRef blnInclude As Boolean, ByRef datDate As Date, ByRef intDays As Short, ByVal blnAllowMonthPart As Boolean) As Boolean

        blnValidControls = False
        If chkInclude.CheckState = System.Windows.Forms.CheckState.Checked Then
            If Not gblnValidDate(txtDate.Text) Then
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
                MsgBox("Invalid " & strLabel & " bracket start date.")
                Exit Function
            End If
            If Not ((txtDays.Text Like "#") Or (txtDays.Text Like "##") Or (txtDays.Text Like "###")) Then
                MsgBox("Invalid number of days in " & strLabel & " bracket.")
                Exit Function
            End If
            blnInclude = True
            datDate = CDate(txtDate.Text)
            intDays = CShort(txtDays.Text)
            If intDays < 1 Then
                MsgBox("Invalid number of days in " & strLabel & " bracket.")
                Exit Function
            End If
        Else
            blnInclude = False
        End If
        blnValidControls = True

    End Function

    Public Sub OpenOutput()
        Dim strExtraFields As String
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

    Public Sub WriteSplit(ByVal objTrx As Trx, ByVal objSplit As Split_Renamed)
        Dim strLine As String
        Dim datInvToUse As Date
        Dim datDueToUse As Date
        Dim strInvDate As String
        Dim strDueDate As String
        Dim strBracket As String

        On Error GoTo ErrorHandler

        gGetSplitDates(objTrx, objSplit, datInvToUse, datDueToUse)
        If objSplit.datInvoiceDate > System.DateTime.FromOADate(0) Then
            strInvDate = gstrVB6Format(objSplit.datInvoiceDate, gstrFORMAT_DATE)
        End If
        If objSplit.datDueDate > System.DateTime.FromOADate(0) Then
            strDueDate = gstrVB6Format(objSplit.datDueDate, gstrFORMAT_DATE)
        End If

        strLine = gstrVB6Format(objTrx.datDate, gstrFORMAT_DATE) & "," & objTrx.strNumber & ",""" & objTrx.strDescription & """," & gstrVB6Format(objSplit.curAmount, gstrFORMAT_CURRENCY) & ",""" & gobjCategories.strKeyToValue1(objSplit.strCategoryKey) & """," & strDueDate & "," & gstrVB6Format(datDueToUse, gstrFORMAT_DATE) & "," & strInvDate & "," & gstrVB6Format(datInvToUse, gstrFORMAT_DATE) & ",""" & objSplit.strPONumber & """,""" & objSplit.strInvoiceNum & """,""" & objSplit.strTerms & """"

        'The order of these extra fields must match the order they
        'are added in OpenOutput().

        If mblnIncludeAging Then
            strBracket = gstrMakeAgingBracket(mdatAgingDate, mintAgingDays, objTrx.blnFake, objTrx.datDate, datInvToUse, datDueToUse)
            strLine = strLine & ",""" & strBracket & """"
        End If

        If mblnIncludeTrans Then
            strBracket = gstrMakeDateBracket(objTrx.datDate, mintTransDays, mdatTransDate)
            strLine = strLine & ",""" & strBracket & """"
        End If

        If mblnIncludeDue Then
            strBracket = gstrMakeDateBracket(datDueToUse, mintDueDays, mdatDueDate)
            strLine = strLine & ",""" & strBracket & """"
        End If

        If mblnIncludeInv Then
            strBracket = gstrMakeDateBracket(datInvToUse, mintInvDays, mdatInvDate)
            strLine = strLine & ",""" & strBracket & """"
        End If

        PrintLine(mintOutputFile, strLine)

        Exit Sub
ErrorHandler:
        NestedError("WriteSplit")
    End Sub

    Public Sub CloseOutput()
        FileClose(mintOutputFile)
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("ExportForm." & strRoutine)
    End Sub

    Private Sub NestedError(ByVal strRoutine As String)
        gNestedErrorTrap("ExportForm." & strRoutine)
    End Sub
End Class
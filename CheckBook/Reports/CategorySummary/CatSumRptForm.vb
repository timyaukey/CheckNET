Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports System.Collections.Generic

Friend Class CatSumRptForm
	Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private maudtCatTotals() As PublicTypes.CategoryInfo 'Indexed by index in gobjCategories.
    Private mcolSelectedAccounts As IEnumerable(Of Account)
    Private mobjCats As IStringTranslator
    Private mdatStart As Date
	Private mdatEnd As Date
	Private mblnIncludeFake As Boolean
	Private mblnIncludeGenerated As Boolean
    Private mobjOutFile As IO.StreamWriter
    Private mblnLoadComplete As Boolean
    Private mintMaxCatNameWidth As Short
    Private mcolResultRows As List(Of ResultRow)

    Private Const mintAMOUNT_WIDTH As Short = 24

    Public Sub ShowMe(ByVal objHostUI As IHostUI, ByRef audtCatTotals() As PublicTypes.CategoryInfo, ByVal colSelectedAccounts As IEnumerable(Of Account),
                      ByVal objCats As IStringTranslator, ByVal datStart As Date, ByVal datEnd As Date,
                      ByVal blnIncludeFake As Boolean, ByVal blnIncludeGenerated As Boolean)

        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.Company
        maudtCatTotals = audtCatTotals.Clone()
        mcolSelectedAccounts = colSelectedAccounts
        mobjCats = objCats
        mdatStart = datStart
        mdatEnd = datEnd
        mblnIncludeFake = blnIncludeFake
        mblnIncludeGenerated = blnIncludeGenerated
        'This form is an MDI child.
        'This code simulates the VB6 
        ' functionality of automatically
        ' loading and showing an MDI
        ' child's parent.
        Me.MdiParent = objHostUI.GetMainForm()
        mobjHostUI.GetMainForm().Show()

        Me.Show()

    End Sub

    Private Sub CatSumRptForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Try

            'Force the window to the size specified in the IDE, because MDI child windows
            'are not initially sized the same as in the IDE.
            Me.Width = 435
            Me.Height = 550

            mblnLoadComplete = True

            LoadUI()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub LoadUI()
        Dim strOutFile As String

        Try

            strOutFile = mobjCompany.ReportsFolderPath() & "\CatSum.rpt"
            Using objOutFile As IO.StreamWriter = New IO.StreamWriter(strOutFile)
                mobjOutFile = objOutFile
                ShowSpecs(mcolSelectedAccounts, mdatStart, mdatEnd, mblnIncludeFake, mblnIncludeGenerated)
                LoadGrid()
            End Using
            mobjHostUI.InfoMessageBox("Report also written to """ & strOutFile & """.")

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub ShowSpecs(ByVal colSelectedAccounts As IEnumerable(Of Account), ByVal datStart As Date, ByVal datEnd As Date, ByVal blnIncludeFake As Boolean, ByVal blnIncludeGenerated As Boolean)

        Dim objAcct As Account

        Try

            WriteRptLine("Totals By Category Report")
            WriteRptLine("-------------------------")
            WriteRptLine("")
            WriteRptLine("Printed On: " & Utilities.FormatDate(Now, "MM/dd/yyyy hh:mmtt"))
            WriteRptLine("")
            WriteRptLine("Accounts Included:")

            With lstAccounts
                .Items.Clear()
                For Each objAcct In colSelectedAccounts
                    .Items.Add(objAcct.Title)
                    WriteRptLine("  " & objAcct.Title)
                Next objAcct
            End With
            WriteRptLine("")

            txtStartDate.Text = Utilities.FormatDate(datStart)
            txtEndDate.Text = Utilities.FormatDate(datEnd)
            chkIncludeFake.CheckState = IIf(blnIncludeFake, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)
            chkIncludeGenerated.CheckState = IIf(blnIncludeGenerated, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)

            WriteRptLine("Start Date:    " & Utilities.FormatDate(datStart))
            WriteRptLine("End Date:      " & Utilities.FormatDate(datEnd))
            WriteRptLine("Fake Trx:      " & IIf(blnIncludeFake, "Yes", "No"))
            WriteRptLine("Generated Trx: " & IIf(blnIncludeGenerated, "Yes", "No"))

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub LoadGrid()
        Dim intCatIndex As Short
        Dim intCatNameWidth As Short

        Try

            mintMaxCatNameWidth = 0
            For intCatIndex = 1 To UBound(maudtCatTotals)
                intCatNameWidth = Len(mobjCats.GetValue1(intCatIndex))
                If intCatNameWidth > mintMaxCatNameWidth Then
                    mintMaxCatNameWidth = intCatNameWidth
                End If
            Next
            'Add space for the "Total" prefix, assuming it could be applied to the longest
            'even though that is very unlikely but we have room to spare.
            mintMaxCatNameWidth = mintMaxCatNameWidth + 12
            WriteRptLine("")
            WriteRptLine(VB.Left("Category" & Space(mintMaxCatNameWidth), mintMaxCatNameWidth) & Space(mintAMOUNT_WIDTH - Len("Amount")) & "Amount")
            WriteRptLine(New String("-", mintMaxCatNameWidth) & " " & New String("-", mintAMOUNT_WIDTH - 1))

            mcolResultRows = New List(Of ResultRow)
            intCatIndex = 1
            curProcessChildren(intCatIndex, 0, "Grand Total", 0)
            grdResults.AutoGenerateColumns = False
            grdResults.DataSource = mcolResultRows

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    'Description:
    '   Process categories with the specified nesting level starting at the specified
    '   category index, until no more categories or a category with a lower nesting
    '   level is encountered.
    'Return Value:
    '   Return the sum of all the direct and indirect child amounts. Does not
    '   include the parent amount.

    Private Function curProcessChildren(ByRef intCatIndex As Short, ByVal intNestingLevel As Short, ByVal strLabel As String, ByVal curParentAmount As Decimal) As Decimal

        Dim strCatName As String = ""
        Dim curAmount As Decimal
        Dim curChildTotal As Decimal

        Try

            Do
                If intCatIndex > UBound(maudtCatTotals) Then
                    Exit Do
                End If
                If maudtCatTotals(intCatIndex).intNestingLevel < intNestingLevel Then
                    Exit Do
                ElseIf maudtCatTotals(intCatIndex).intNestingLevel = intNestingLevel Then
                    strCatName = mobjCats.GetValue1(intCatIndex)
                    curAmount = maudtCatTotals(intCatIndex).curAmount
                    curChildTotal = curChildTotal + curAmount
                    AddOutputRow(strCatName, curAmount, intNestingLevel)
                    intCatIndex = intCatIndex + 1
                ElseIf maudtCatTotals(intCatIndex).intNestingLevel = intNestingLevel + 1 Then
                    curChildTotal = curChildTotal + curProcessChildren(intCatIndex, intNestingLevel + 1, "-- Total -- " & strCatName, curAmount)
                Else
                    RaiseErrorMsg("Improper nesting of categories")
                End If
            Loop
            'Output cumulative total
            AddOutputRow(strLabel, curChildTotal + curParentAmount, intNestingLevel - 1)
            curProcessChildren = curChildTotal

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Sub AddOutputRow(ByVal strLabel As String, ByVal curAmount As Decimal, ByVal intNestingLevel As Short)

        Dim strRightPad As String
        Dim strAmount As String
        Dim objResultRow As ResultRow

        objResultRow = New ResultRow()

        strRightPad = New String(" ", intNestingLevel + 1)
        objResultRow.Label = strRightPad & strRightPad & strRightPad & strLabel
        strAmount = Utilities.FormatCurrency(curAmount)
        objResultRow.Amount = strAmount & strRightPad & strRightPad & strRightPad
        mcolResultRows.Add(objResultRow)

        WriteRptLine(VB.Left(strLabel & Space(mintMaxCatNameWidth), mintMaxCatNameWidth) & VB.Right(Space(mintAMOUNT_WIDTH) & strAmount, mintAMOUNT_WIDTH))
    End Sub

    Private Class ResultRow
        Private mLabel As String
        Public Property Label() As String
            Get
                Label = mLabel
            End Get
            Set(ByVal value As String)
                mLabel = value
            End Set
        End Property

        Private mAmount As String
        Public Property Amount() As String
            Get
                Amount = mAmount
            End Get
            Set(ByVal value As String)
                mAmount = value
            End Set
        End Property
    End Class

    Private Sub WriteRptLine(ByVal strLine As String)
        mobjOutFile.WriteLine(strLine)
    End Sub
	
	Private Sub cmdResultToClipboard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdResultToClipboard.Click
        Try

            Dim intIndex As Short
            Dim strResult As String = ""
            Dim strCatCode As String
            Dim strCatName As String
            Dim strLine As String

            For intIndex = Utilities.LowerBound1 To UBound(maudtCatTotals)
                strCatCode = mobjCats.GetKey(intIndex)
                strCatName = mobjCats.GetValue1(intIndex)
                strLine = strCatName & vbTab & maudtCatTotals(intIndex).curAmount & vbTab & strCatCode
                strResult = strResult & strLine & vbCrLf
            Next

            My.Computer.Clipboard.Clear()
            My.Computer.Clipboard.SetText(strResult)

            mobjHostUI.InfoMessageBox("Results copied for clipboard.")

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub
End Class
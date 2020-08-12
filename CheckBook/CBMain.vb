Option Strict On
Option Explicit On


''' <summary>
''' Static methods related to WinForm user interface management.
''' </summary>

Public Module CBMain

    Public Const gstrREG_APP As String = "Willow Creek Checkbook"
    Public Const gstrREG_KEY_GENERAL As String = "General"
    'See also: gstrRegkeyRegister().

    'Lowest index in a ListView ListItem collection.
    'Will be 0 in .NET, 1 in VB6.
    Public Const gintLISTITEM_LOWINDEX As Integer = 0

    Public Sub Main()
        CBMainForm.Show()
    End Sub

    Public Function gcolForms() As IEnumerable(Of Form)
        Dim frm As System.Windows.Forms.Form
        Dim colResult As List(Of Form)
        colResult = New List(Of Form)
        For Each frm In CBMainForm.MdiChildren
            colResult.Add(frm)
        Next frm
        gcolForms = colResult
    End Function

    Public Function gblnAskAndCreateAccount(ByVal objHostUI As IHostUI) As Boolean
        Dim objAccount As Account
        Dim strFile As String

        objAccount = New Account()
        objAccount.Init(objHostUI.objCompany)
        objAccount.intKey = objHostUI.objCompany.intGetUnusedAccountKey()
        objAccount.lngSubType = Account.SubType.Liability_LoanPayable

        Using frm As AccountForm = New AccountForm()
            If frm.ShowDialog(objHostUI, objAccount, False, False) = DialogResult.OK Then
                strFile = objHostUI.objCompany.strAccountPath() & "\" & objAccount.strFileNameRoot & ".act"
                If Dir(strFile) <> "" Then
                    objHostUI.ErrorMessageBox("Account file already exists with that name.")
                    Exit Function
                End If
                objAccount.Create()
                Return True
            End If
        End Using
        Return False
    End Function

    Public Sub gInitPayeeList(ByVal lvwPayees As System.Windows.Forms.ListView)
        With lvwPayees
            .Columns.Clear()
            .Columns.Add("", "Number", 55)
            .Columns.Add("", "Name/Description", 200)
            .Columns.Add("", "Category", 160)
            .Columns.Add("", "Amount", 65)
            .Columns.Add("", "Budget", 130)
            .Columns.Add("", "Memo", 200)
            .View = System.Windows.Forms.View.Details
            .FullRowSelect = True
            .HideSelection = False
            .Sort()
            .LabelEdit = False
        End With
        gDisablePayeeListSorting(lvwPayees)
    End Sub

    Public Sub gDisablePayeeListSorting(ByVal lvwPayees As System.Windows.Forms.ListView)
        UITools.SetListViewSortColumn(lvwPayees, 0)
    End Sub

    Public Sub gSortPayeeListByName(ByVal lvwPayees As System.Windows.Forms.ListView)
        UITools.SetListViewSortColumn(lvwPayees, 1)
    End Sub

    Public Function gobjCreatePayeeListItem(ByVal elmPayee As VB6XmlElement, ByVal lvwPayees As System.Windows.Forms.ListView, ByVal intIndex As Short) As System.Windows.Forms.ListViewItem

        Dim objItem As System.Windows.Forms.ListViewItem
        Dim elmNum As VB6XmlElement
        Dim strNum As String

        elmNum = DirectCast(elmPayee.SelectSingleNode("Num"), VB6XmlElement)
        If elmNum Is Nothing Then
            strNum = ""
        Else
            strNum = elmNum.Text
        End If
        gDisablePayeeListSorting(lvwPayees)
        objItem = lvwPayees.Items.Add(strNum)
        objItem.Tag = CStr(intIndex)
        If objItem.SubItems.Count > 1 Then
            objItem.SubItems(1).Text = CStr(elmPayee.GetAttribute("Output"))
        Else
            objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(elmPayee.GetAttribute("Output"))))
        End If
        SetPayeeSubItem(objItem, 2, elmPayee, "Cat")
        SetPayeeSubItem(objItem, 3, elmPayee, "Amount")
        SetPayeeSubItem(objItem, 4, elmPayee, "Budget")
        SetPayeeSubItem(objItem, 5, elmPayee, "Memo")
        gobjCreatePayeeListItem = objItem
    End Function

    Private Sub SetPayeeSubItem(ByVal objItem As System.Windows.Forms.ListViewItem, ByVal intSubItem As Short, ByVal elmPayee As VB6XmlElement, ByVal strChildName As String)

        Dim elmChild As VB6XmlElement
        Dim strText As String

        elmChild = DirectCast(elmPayee.SelectSingleNode(strChildName), VB6XmlElement)
        If elmChild Is Nothing Then
            strText = ""
        Else
            strText = elmChild.Text
        End If
        If objItem.SubItems.Count > intSubItem Then
            objItem.SubItems(intSubItem).Text = strText
        Else
            objItem.SubItems.Insert(intSubItem, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strText))
        End If
    End Sub

    Public Sub gLoadMatchNarrowingMethods(ByVal cbo As ComboBox)
        cbo.Items.Clear()
        cbo.Items.Add(UITools.CreateListBoxItem("None", ImportMatchNarrowMethod.None))
        cbo.Items.Add(UITools.CreateListBoxItem("Closest Date", ImportMatchNarrowMethod.ClosestDate))
        cbo.Items.Add(UITools.CreateListBoxItem("Earliest Date", ImportMatchNarrowMethod.EarliestDate))
    End Sub

End Module
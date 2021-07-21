Option Strict Off
Option Explicit On


Friend Class LiveBudgetListForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjHostUI As IHostUI
    Private mobjReg As Register
    Private mobjBudgets As IStringTranslator

    Public Sub ShowModal(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register, ByVal objBudgets_ As IStringTranslator)

        mobjHostUI = objHostUI_
        mobjReg = objReg_
        mobjBudgets = objBudgets_
        Me.Text = "Live Budgets In " & mobjReg.Title
        Me.ShowDialog()

    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Dim datTarget As Date
        Dim objItem As System.Windows.Forms.ListViewItem

        Try

            If Not Utilities.blnIsValidDate(txtTargetDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid target date.")
                Exit Sub
            End If
            datTarget = CDate(txtTargetDate.Text)

            lvwMatches.Items.Clear()
            For Each objTrx As BudgetTrx In mobjReg.GetAllTrx(Of BudgetTrx)()
                With objTrx
                    If .InBudgetPeriod(datTarget) Then
                        objItem = UITools.ListViewAdd(lvwMatches)
                        objItem.Text = Utilities.strFormatDate(.datDate)

                        If objItem.SubItems.Count > 1 Then
                            objItem.SubItems(1).Text = .strDescription
                        Else
                            objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, .strDescription))
                        End If

                        If objItem.SubItems.Count > 2 Then
                            objItem.SubItems(2).Text = mobjBudgets.strKeyToValue1(.strBudgetKey)
                        Else
                            objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, mobjBudgets.strKeyToValue1(.strBudgetKey)))
                        End If

                        If objItem.SubItems.Count > 3 Then
                            objItem.SubItems(3).Text = Utilities.strFormatCurrency(.curBudgetLimit)
                        Else
                            objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Utilities.strFormatCurrency(.curBudgetLimit)))
                        End If

                        If objItem.SubItems.Count > 4 Then
                            objItem.SubItems(4).Text = Utilities.strFormatCurrency(.curBudgetApplied)
                        Else
                            objItem.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Utilities.strFormatCurrency(.curBudgetApplied)))
                        End If

                        If objItem.SubItems.Count > 5 Then
                            objItem.SubItems(5).Text = Utilities.strFormatCurrency(.curAmount)
                        Else
                            objItem.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Utilities.strFormatCurrency(.curAmount)))
                        End If
                    End If
                End With
            Next

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class
Option Strict Off
Option Explicit On


Friend Class PayeeMatchForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mcolPayees As VB6XmlNodeList
    Private melmSelect As VB6XmlElement
    Private mblnActivated As Boolean

    Public Function elmSelect(ByVal objHostUI As IHostUI, ByVal colPayees As VB6XmlNodeList) As VB6XmlElement
        Dim elmPayee As VB6XmlElement
        Dim intIndex As Short

        mobjHostUI = objHostUI
        elmSelect = Nothing
        Try

            mcolPayees = colPayees
            gInitPayeeList(lvwPayees)
            lvwPayees.Items.Clear()
            For intIndex = 1 To mcolPayees.Length
                elmPayee = mcolPayees.Item(intIndex - 1)
                gobjCreatePayeeListItem(elmPayee, lvwPayees, intIndex - 1)
            Next
            gSortPayeeListByName(lvwPayees)
            'UPGRADE_NOTE: Object melmSelect may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            melmSelect = Nothing
            Me.ShowDialog()
            elmSelect = melmSelect

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub PayeeMatchForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        Dim objFirst As System.Windows.Forms.ListViewItem

        Try

            If Not mblnActivated Then
                mblnActivated = True
                objFirst = lvwPayees.TopItem
                If Not objFirst Is Nothing Then
                    lvwPayees.FocusedItem = objFirst
                End If
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            If lvwPayees.FocusedItem Is Nothing Then
                mobjHostUI.ErrorMessageBox("Select a payee first.")
                Exit Sub
            End If
            melmSelect = mcolPayees.Item(lvwPayees.FocusedItem.Tag)
            Me.Close()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lvwPayees_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwPayees.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub
End Class
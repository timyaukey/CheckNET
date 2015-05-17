Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class PayeeMatchForm
    Inherits System.Windows.Forms.Form

    Private mcolPayees As VB6XmlNodeList
    Private melmSelect As VB6XmlElement
    Private mblnActivated As Boolean

    Public Function elmSelect(ByVal colPayees As VB6XmlNodeList) As VB6XmlElement
        Dim elmPayee As VB6XmlElement
        Dim intIndex As Short

        On Error GoTo ErrorHandler

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
ErrorHandler:
        NestedError("elmSelect")
    End Function

    'UPGRADE_WARNING: Form event PayeeMatchForm.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub PayeeMatchForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        Dim objFirst As System.Windows.Forms.ListViewItem

        On Error GoTo ErrorHandler

        If Not mblnActivated Then
            mblnActivated = True
            objFirst = lvwPayees.TopItem
            If Not objFirst Is Nothing Then
                lvwPayees.FocusedItem = objFirst
            End If
        End If

        Exit Sub
ErrorHandler:
        TopError("Form_Activate")
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        On Error GoTo ErrorHandler

        If lvwPayees.FocusedItem Is Nothing Then
            MsgBox("Select a payee first.", MsgBoxStyle.Critical)
            Exit Sub
        End If
        melmSelect = mcolPayees.Item(lvwPayees.FocusedItem.Tag)
        Me.Close()

        Exit Sub
ErrorHandler:
        TopError("cmdOkay_Click")
    End Sub

    Private Sub lvwPayees_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwPayees.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("PayeeMatchForm." & strRoutine)
    End Sub

    Private Sub NestedError(ByVal strRoutine As String)
        gNestedErrorTrap("PayeeMatchForm." & strRoutine)
    End Sub
End Class
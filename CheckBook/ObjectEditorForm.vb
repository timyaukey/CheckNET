Option Explicit On
Option Strict On

Imports CheckBookLib

Public Class ObjectEditorForm

    Private mobjData As IFilePersistable

    Public Function ShowEditor(ByVal objData As IFilePersistable) As Boolean
        mobjData = objData
        grdData.SelectedObject = mobjData
        Me.ShowDialog()
    End Function
End Class
Option Explicit On
Option Strict On

Imports System.IO
Imports CheckBookLib

Public Class FileListEditorForm

    Private mstrFolder As String
    Private mstrFileType As String
    Private mobjPersister As IFilePersister

    Public Sub ShowDialogForPath(ByVal strTitle As String, ByVal strFolder As String, _
        ByVal strFileType As String, objPersister As IFilePersister)
        Me.Text = strTitle
        mstrFolder = strFolder
        mstrFileType = strFileType
        mobjPersister = objPersister
        Me.ShowDialog()
    End Sub

    Private Sub FileListEditorForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFileList()
    End Sub

    Private Sub LoadFileList()
        Dim strFile As String
        Dim item As ListViewItem

        lvwFiles.Items.Clear()
        For Each strFile In Directory.EnumerateFiles(mstrFolder)
            Dim file As FileInfo
            Dim subItems(3) As String
            Dim strName As String

            file = New FileInfo(strFile)
            strName = file.Name
            If strName.ToLower().EndsWith("." + mstrFileType) Then
                subItems(0) = file.Name
                subItems(1) = file.LastWriteTime.ToShortDateString() + " " + file.LastWriteTime.ToShortTimeString()
                subItems(2) = file.Length.ToString()
                item = New ListViewItem(subItems)
                item.Tag = file
                lvwFiles.Items.Add(item)
            End If
        Next
    End Sub

    Public Class FileItem
        Public File As FileInfo
    End Class

    Private Sub lvwFiles_DoubleClick(sender As Object, e As EventArgs) Handles lvwFiles.DoubleClick
        EditSelectedFile()
    End Sub

    Private Sub EditSelectedFile()
        Dim strFile As String
        strFile = GetCurrentFile()
        If strFile <> Nothing Then
            Dim objData As IFilePersistable
            objData = mobjPersister.Load(strFile)
            Using frm As New ObjectEditorForm
                If frm.ShowEditor(objData) Then
                    mobjPersister.Save(objData, strFile)
                End If
            End Using
        End If
    End Sub

    Private Function GetCurrentFile() As String
        If lvwFiles.SelectedItems.Count = 0 Then
            GetCurrentFile = Nothing
        Else
            GetCurrentFile = DirectCast(lvwFiles.SelectedItems(0).Tag, FileInfo).FullName
        End If
    End Function
End Class
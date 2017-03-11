Option Strict On
Option Explicit On

Imports System.IO
Imports CheckBookLib

Public Class FileListEditorForm

    Private mstrFolder As String
    Private mstrFileType As String
    Private mobjPersister As IFilePersister

    Public Sub ShowDialogForPath(ByVal strTitle As String, ByVal strFolder As String, _
        ByVal strFileType As String, ByVal objPersister As IFilePersister)
        Me.Text = strTitle
        mstrFolder = strFolder
        mstrFileType = strFileType
        mobjPersister = objPersister
        Me.ShowDialog()
    End Sub

    Private Sub FileListEditorForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFileList()
        LoadTypeList()
    End Sub

    Private Sub LoadFileList()
        Dim strFile As String
        Dim item As ListViewItem

        lvwFiles.Items.Clear()
        If Not Directory.Exists(mstrFolder) Then
            Directory.CreateDirectory(mstrFolder)
        End If
        For Each strFile In Directory.EnumerateFiles(mstrFolder)
            Dim file As FileInfo
            Dim subItems(3) As String
            Dim strName As String

            file = New FileInfo(strFile)
            strName = file.Name
            If strName.ToLower().EndsWith("." + mstrFileType) Then
                subItems(0) = Path.GetFileNameWithoutExtension(file.Name)
                subItems(1) = file.LastWriteTime.ToShortDateString() + " " + file.LastWriteTime.ToShortTimeString()
                item = New ListViewItem(subItems)
                item.Tag = file
                lvwFiles.Items.Add(item)
            End If
        Next
    End Sub

    Public Sub LoadTypeList()
        cboNewType.Items.Clear()
        For Each objType As FilePersistableType In mobjPersister.GetTypes()
            cboNewType.Items.Add(objType)
        Next
    End Sub

    Public Class FileItem
        Public File As FileInfo
    End Class

    Private Sub lvwFiles_DoubleClick(sender As Object, e As EventArgs) Handles lvwFiles.DoubleClick
        EditSelectedFile()
    End Sub

    Private Sub btnEditFile_Click(sender As Object, e As EventArgs) Handles btnEditFile.Click
        EditSelectedFile()
    End Sub

    Private Sub EditSelectedFile()
        Dim strFile As String
        strFile = GetCurrentFile()
        If strFile <> Nothing Then
            Dim objData As IFilePersistable
            Try
                objData = mobjPersister.Load(strFile)
                Using frm As New ObjectEditorForm
                    If frm.ShowEditor(objData, Path.GetFileNameWithoutExtension(strFile)) Then
                        objData.CleanForSave()
                        mobjPersister.Save(objData, strFile)
                        MsgBox("File saved.", MsgBoxStyle.Information)
                    End If
                End Using
            Catch ex As FilePersisterException
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnNewFile_Click(sender As Object, e As EventArgs) Handles btnNewFile.Click
        Dim objData As IFilePersistable
        Dim strFile As String = Nothing

        If cboNewType.SelectedItem Is Nothing Then
            MsgBox("Please select new file type.")
            Exit Sub
        End If
        If Not CheckNewFileName(strFile) Then
            Exit Sub
        End If
        objData = mobjPersister.Create(DirectCast(cboNewType.SelectedItem, FilePersistableType).strType, strFile)
        Using frm As New ObjectEditorForm
            If frm.ShowEditor(objData, Path.GetFileNameWithoutExtension(strFile)) Then
                objData.CleanForSave()
                mobjPersister.Save(objData, strFile)
                MsgBox("File saved.", MsgBoxStyle.Information)
                LoadFileList()
            End If
        End Using
    End Sub

    Private Sub btnRenameFile_Click(sender As Object, e As EventArgs) Handles btnRenameFile.Click
        Dim strNewFile As String = Nothing
        Dim strOldFile As String = GetCurrentFile()
        If strOldFile = Nothing Then
            MsgBox("Please select file to rename.")
            Exit Sub
        End If
        If Not CheckNewFileName(strNewFile) Then
            Exit Sub
        End If
        If MsgBox("Are you sure you want to rename this file?", MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
            MsgBox("File not renamed.")
            Exit Sub
        End If
        File.Move(strOldFile, strNewFile)
        LoadFileList()
        MsgBox("Renamed """ + Path.GetFileName(strOldFile) + """ to """ + Path.GetFileName(strNewFile) + """.")
    End Sub

    Private Sub btnDeleteFile_Click(sender As Object, e As EventArgs) Handles btnDeleteFile.Click
        Dim strOldFile As String
        strOldFile = GetCurrentFile()
        If strOldFile = Nothing Then
            MsgBox("Please select file to delete.")
            Exit Sub
        End If
        If MsgBox("Are you sure you want to delete this file?", MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
            MsgBox("File not deleted.")
            Exit Sub
        End If
        File.Delete(strOldFile)
        LoadFileList()
        MsgBox("Deleted """ + Path.GetFileName(strOldFile) + """.")
    End Sub

    Private Function GetCurrentFile() As String
        If lvwFiles.SelectedItems.Count = 0 Then
            GetCurrentFile = Nothing
        Else
            GetCurrentFile = DirectCast(lvwFiles.SelectedItems(0).Tag, FileInfo).FullName
        End If
    End Function

    Private Function CheckNewFileName(ByRef strFile As String) As Boolean
        Dim strName As String = txtNewName.Text
        If String.IsNullOrEmpty(strName) Then
            MsgBox("Please enter new file name.")
            Return False
        End If
        If strName.Contains(".") Or strName.Contains("\") Or strName.Contains("/") Then
            MsgBox("Do not enter path or file type for new file.")
            Return False
        End If
        strFile = Path.Combine(mstrFolder, strName) + "." + mstrFileType
        If File.Exists(strFile) Then
            MsgBox("New file name already exists.")
            Return False
        End If
        Return True
    End Function

End Class
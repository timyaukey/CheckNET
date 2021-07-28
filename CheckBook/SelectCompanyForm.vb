Option Strict On
Option Explicit On

Public Class SelectCompanyForm
    Public strDataPath As String
    Private mstrDefaultRootFolder As String
    Private mblnNewOnEmptyAsked As Boolean

    Private mobjHostUI As IHostUI
    Private mobjShowMessage As Action(Of String)

    Public Function ShowCompanyDialog(ByVal objHostUI As IHostUI, ByVal objShowMessage As Action(Of String)) As DialogResult
        mobjHostUI = objHostUI
        mobjShowMessage = objShowMessage
        mstrDefaultRootFolder = Company.DefaultRootFolder(mobjHostUI.strSoftwareName)
        mblnNewOnEmptyAsked = False
        If My.Settings.CompanyList Is Nothing Then
            My.Settings.CompanyList = New Specialized.StringCollection()
        End If
        ShowHistoryList()
        Return ShowDialog()
    End Function

    Private Sub ShowHistoryList()
        lstHistory.Items.Clear()
        For Each strHistory As String In My.Settings.CompanyList
            lstHistory.Items.Add(New CompanyHistoryItem(strHistory))
        Next
    End Sub

    Private Sub lstHistory_DoubleClick(sender As Object, e As EventArgs) Handles lstHistory.DoubleClick
        btnOpen_Click(sender, e)
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Try
            Dim objHistItem As CompanyHistoryItem = DirectCast(lstHistory.SelectedItem, CompanyHistoryItem)
            If objHistItem Is Nothing Then
                mobjHostUI.ErrorMessageBox("Please select a company folder from the list above.")
                Return
            End If
            If Not Company.IsDataPathValid(objHistItem.strDataPath) Then
                If mobjHostUI.OkCancelMessageBox("Could not find the selected company folder. " +
                "Do you want to remove it from the list?") = DialogResult.OK Then
                    HistoryRemove(objHistItem.strDataPath)
                    ShowHistoryList()
                    Return
                End If
                Return
            End If

            Me.strDataPath = objHistItem.strDataPath
            HistoryRemove(Me.strDataPath)
            HistoryAdd(Me.strDataPath)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            CBMainForm.blnCancelStart = True
            TopException(ex)
            Me.Close()
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Try
            CreateDefaultRootFolder()
            Using frmNewCompany As NewCompanyForm = New NewCompanyForm()
                If frmNewCompany.ShowNewCompanyDialog(mobjHostUI) <> DialogResult.OK Then
                    Return
                End If
                Dim strDataPath As String = System.IO.Path.Combine(mstrDefaultRootFolder, frmNewCompany.strCompanyName)
                If System.IO.Directory.Exists(strDataPath) Then
                    mobjHostUI.ErrorMessageBox("A company folder with that name already exists.")
                    Return
                End If
                Dim objCreateCompany As Company = New Company(strDataPath)
                objCreateCompany.CreateInitialData(mobjShowMessage)
                Me.strDataPath = strDataPath
                HistoryAdd(Me.strDataPath)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            CBMainForm.blnCancelStart = True
            TopException(ex)
            Me.Close()
        End Try
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Try
            CreateDefaultRootFolder()
            dlgBrowseCompany.SelectedPath = mstrDefaultRootFolder
            Dim result As DialogResult = dlgBrowseCompany.ShowDialog()
            If result <> DialogResult.OK Then
                Exit Sub
            End If
            If Company.IsDataPathValid(dlgBrowseCompany.SelectedPath) Then
                Me.strDataPath = dlgBrowseCompany.SelectedPath
                HistoryRemove(Me.strDataPath)
                HistoryAdd(Me.strDataPath)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                mobjHostUI.ErrorMessageBox("Selected folder is not a company data folder.")
            End If
        Catch ex As Exception
            CBMainForm.blnCancelStart = True
            TopException(ex)
            Me.Close()
        End Try
    End Sub

    Private Sub CreateDefaultRootFolder()
        If Not System.IO.Directory.Exists(mstrDefaultRootFolder) Then
            System.IO.Directory.CreateDirectory(mstrDefaultRootFolder)
        End If
    End Sub

    Private Sub HistoryAdd(ByVal strDataPath As String)
        My.Settings.CompanyList.Insert(0, strDataPath)
    End Sub

    Private Sub HistoryRemove(ByVal strDataPath As String)
        If My.Settings.CompanyList.Contains(strDataPath) Then
            My.Settings.CompanyList.Remove(strDataPath)
        End If
    End Sub

    Private Sub SelectCompanyForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If Not mblnNewOnEmptyAsked Then
            If My.Settings.CompanyList.Count = 0 Then
                btnNew_Click(sender, e)
            End If
            mblnNewOnEmptyAsked = True
        End If
    End Sub

    Private Class CompanyHistoryItem
        Public strDataPath As String
        Private strCompanyName As String
        Private strParentFolder As String

        Public Sub New(ByVal strDataPath_ As String)
            strDataPath = strDataPath_
            strCompanyName = System.IO.Path.GetFileName(strDataPath)
            strParentFolder = System.IO.Path.GetDirectoryName(strDataPath)
        End Sub

        Public Overrides Function ToString() As String
            Return strCompanyName + "  (in " + strParentFolder + ")"
        End Function
    End Class

End Class

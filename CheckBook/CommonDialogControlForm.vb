Option Strict Off
Option Explicit On


Friend Class CommonDialogControlForm
    Inherits System.Windows.Forms.Form

    Public Shared Function strChooseFile(ByVal strTitle As String, _
            ByVal strFileType As String, ByVal strSettingsKey As String) As String
        Dim strFile As String
        Dim strPath As String

        strChooseFile = ""
        strFile = ""
        strPath = ""
        Using frm As New CommonDialogControlForm
            With frm.ctlDialogOpen
                .Title = strTitle
                .Filter = Microsoft.VisualBasic.UCase(strFileType) + " files|*." + Microsoft.VisualBasic.LCase(strFileType) + "|All files (*.*)|*.*"
                .FilterIndex = 1
                .ShowReadOnly = False
                .CheckFileExists = True
                .CheckPathExists = True
                strPath = GetSetting(gstrREG_APP, gstrREG_KEY_GENERAL, strSettingsKey, "")
                .InitialDirectory = strPath
                .ShowDialog()
                strFile = .FileName
            End With
        End Using

        If strFile = "" Then
            Exit Function
        End If
        strPath = Microsoft.VisualBasic.Left(strFile, InStrRev(strFile, "\"))
        If Microsoft.VisualBasic.Right(strPath, 1) = "\" Then
            strPath = Microsoft.VisualBasic.Left(strPath, Len(strPath) - 1)
        End If
        SaveSetting(gstrREG_APP, gstrREG_KEY_GENERAL, strSettingsKey, strPath)
        strChooseFile = strFile

    End Function
End Class
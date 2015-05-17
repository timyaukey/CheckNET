Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportBankDownloadQIF
    Implements _ITrxImport
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Implement ITrxImport for a QIF file downloaded from an online banking service
    'such as FSBLink. The downloaded data will not have category or split
    'information, and the payee names will be passed through a translation algorithm
    'to scrub them and provide a category based on matching them to a predefined
    'list of possible payees. Payees not in the list are allowed, but no category
    'information will be added for them.


    Private mintFile As Short
    Private mstrFile As String
    Private mobjUtil As ImportUtilities

    Private Function ITrxImport_blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements _ITrxImport.blnOpenSource
        Dim frm As CommonDialogControlForm
        Dim strPath As String
        Dim strFirstLine As String

        On Error GoTo ErrorHandler

        mobjUtil = New ImportUtilities
        mobjUtil.Init(objAccount_)
        mobjUtil.LoadTrxTypeTable()

        frm = New CommonDialogControlForm
        'UPGRADE_WARNING: CommonDialog variable was not upgraded Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="671167DC-EA81-475D-B690-7A40C7BF4A23"'
        With CType(frm.Controls("ctlDialog"), Object)
            .Title = "Select Bank Download QIF File To Import"
            'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            .Filter = "QIF files|*.qif|All files (*.*)|*.*"
            .FilterIndex = 1
            'UPGRADE_WARNING: FileOpenConstants constant FileOpenConstants.cdlOFNHideReadOnly was upgraded to OpenFileDialog.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            'UPGRADE_WARNING: MSComDlg.CommonDialog property frm!ctlDialog.Flags was upgraded to frm!ctlDialogOpen.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            'UPGRADE_WARNING: FileOpenConstants constant FileOpenConstants.cdlOFNHideReadOnly was upgraded to OpenFileDialog.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            .ShowReadOnly = False
            'UPGRADE_WARNING: MSComDlg.CommonDialog property frm!ctlDialog.Flags was upgraded to frm!ctlDialogOpen.CheckFileExists which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            .CheckFileExists = True
            .CheckPathExists = True
            strPath = GetSetting(gstrREG_APP, gstrREG_KEY_GENERAL, "BankQIFPath", "")
            .InitialDirectory = strPath
            .ShowDialog()
            mstrFile = .FileName
        End With
        frm.Close()
        If mstrFile = "" Then
            Exit Function
        End If
        strPath = Left(mstrFile, InStrRev(mstrFile, "\"))
        If Right(strPath, 1) = "\" Then
            strPath = Left(strPath, Len(strPath) - 1)
        End If
        SaveSetting(gstrREG_APP, gstrREG_KEY_GENERAL, "BankQIFPath", strPath)
        mintFile = FreeFile()
        FileOpen(mintFile, mstrFile, OpenMode.Input)
        strFirstLine = LineInput(mintFile)
        If strFirstLine = "!Type:Bank" Then
            mobjUtil.blnMakeFakeTrx = False
        ElseIf strFirstLine = "!Type:Docs" Then
            mobjUtil.blnMakeFakeTrx = True
        Else
            MsgBox("File is not a QIF file.", MsgBoxStyle.Critical)
            FileClose(mintFile)
            Exit Function
        End If

        ITrxImport_blnOpenSource = True

        Exit Function
ErrorHandler:
        If Not frm Is Nothing Then
            frm.Close()
        End If
        NestedError("ITrxImport_blnOpenSource")
    End Function

    Private Sub ITrxImport_CloseSource() Implements _ITrxImport.CloseSource
        If mintFile Then
            FileClose(mintFile)
            mintFile = 0
        End If
    End Sub

    Private Function ITrxImport_objNextTrx() As Trx Implements _ITrxImport.objNextTrx
        Dim strLine As String
        Dim strPrefix As String
        Dim strDate As String

        On Error GoTo ErrorHandler

        mobjUtil.ClearSavedTrxData()
        Do
            If EOF(mintFile) Then
                Exit Do
            End If
            strLine = LineInput(mintFile)
            strPrefix = Left(strLine, 1)
            Select Case strPrefix
                Case "^"
                    ITrxImport_objNextTrx = mobjUtil.objMakeTrx()
                    Exit Do
                Case "D"
                    strDate = Replace(Mid(strLine, 2), "' ", "/200")
                    strDate = Replace(strDate, "'", "/")
                    mobjUtil.strTrxDate = strDate
                Case "P"
                    mobjUtil.strTrxPayee = Mid(strLine, 2)
                Case "T"
                    mobjUtil.strTrxAmount = Mid(strLine, 2)
                Case "M"
                    mobjUtil.strTrxMemo = Mid(strLine, 2)
                Case "N"
                    mobjUtil.strTrxNumber = Mid(strLine, 2)
                Case Else
                    gRaiseError("Unrecognized input line: " & strLine)
            End Select
        Loop

        Exit Function
ErrorHandler:
        NestedError("ITrxImport_objNextTrx")
    End Function

    Private ReadOnly Property ITrxImport_strSource() As String Implements _ITrxImport.strSource
        Get
            ITrxImport_strSource = mstrFile
        End Get
    End Property

    Private Sub NestedError(ByVal strRoutine As String)
        gNestedErrorTrap("ImportBankDownloadQIF." & strRoutine)
    End Sub
End Class
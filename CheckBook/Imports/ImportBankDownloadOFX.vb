Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportBankDownloadOFX
    Implements _ITrxImport
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Implement ITrxImport for an OFX file downloaded from an online banking service.
    'The downloaded data will not have category or split
    'information, and the payee names will be passed through a translation algorithm
    'to scrub them and provide a category based on matching them to a predefined
    'list of possible payees. Payees not in the list are allowed, but no category
    'information will be added for them.


    Private mintFile As Short
    Private mstrFile As String
    Private mstrInputLine As String
    Private mblnInputEOF As Boolean
    Private mobjUtil As ImportUtilities

    Private Function ITrxImport_blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements _ITrxImport.blnOpenSource
        Dim frm As CommonDialogControlForm
        Dim strPath As String
        Dim strHeaderLine As String

        On Error GoTo ErrorHandler

        mobjUtil = New ImportUtilities
        mobjUtil.Init(objAccount_)
        mobjUtil.LoadTrxTypeTable()

        frm = New CommonDialogControlForm

        'UPGRADE_WARNING: CommonDialog variable was not upgraded Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="671167DC-EA81-475D-B690-7A40C7BF4A23"'
        With frm.ctlDialogOpen 'CType(frm.Controls("ctlDialog"), Object)
            .Title = "Select Bank Download OFX File To Import"
            'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            .Filter = "OFX files|*.ofx|All files (*.*)|*.*"
            .FilterIndex = 1
            'UPGRADE_WARNING: FileOpenConstants constant FileOpenConstants.cdlOFNHideReadOnly was upgraded to OpenFileDialog.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            'UPGRADE_WARNING: MSComDlg.CommonDialog property frm!ctlDialog.Flags was upgraded to frm!ctlDialogOpen.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            'UPGRADE_WARNING: FileOpenConstants constant FileOpenConstants.cdlOFNHideReadOnly was upgraded to OpenFileDialog.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            .ShowReadOnly = False
            'UPGRADE_WARNING: MSComDlg.CommonDialog property frm!ctlDialog.Flags was upgraded to frm!ctlDialogOpen.CheckFileExists which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
            .CheckFileExists = True
            .CheckPathExists = True
            strPath = GetSetting(gstrREG_APP, gstrREG_KEY_GENERAL, "BankOFXPath", "")
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
        SaveSetting(gstrREG_APP, gstrREG_KEY_GENERAL, "BankOFXPath", strPath)
        mintFile = FreeFile()
        FileOpen(mintFile, mstrFile, OpenMode.Input)
        strHeaderLine = LineInput(mintFile)
        If strHeaderLine = "OFXHEADER:100" Then
            mobjUtil.blnMakeFakeTrx = False
        Else
            MsgBox("File is not an OFX file.", MsgBoxStyle.Critical)
            FileClose(mintFile)
            Exit Function
        End If
        strHeaderLine = LineInput(mintFile)
        strHeaderLine = LineInput(mintFile)
        If strHeaderLine <> "VERSION:102" Then
            MsgBox("File is not correct OFX version.", MsgBoxStyle.Critical)
            FileClose(mintFile)
            Exit Function
        End If
        Do
            If EOF(mintFile) Then
                MsgBox("Unexpected EOF in header section of OFX file.", MsgBoxStyle.Critical)
                FileClose(mintFile)
                Exit Function
            End If
            strHeaderLine = LineInput(mintFile)
            If strHeaderLine = "" Then
                Exit Do
            End If
        Loop

        ITrxImport_blnOpenSource = True
        mblnInputEOF = False
        mstrInputLine = ""

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
        Dim strToken As String
        Dim strCheckNum As String

        On Error GoTo ErrorHandler

        mobjUtil.ClearSavedTrxData()
        strCheckNum = ""
        Do
            strToken = strGetToken()
            If mblnInputEOF Then
                gRaiseError("Unexpected EOF in OFX file")
            End If
            Select Case strToken
                Case "</OFX>"
                    Exit Do
                Case "</STMTTRN>"
                    If strCheckNum <> "" Then
                        mobjUtil.strTrxNumber = strCheckNum
                    End If
                    ITrxImport_objNextTrx = mobjUtil.objMakeTrx()
                    strCheckNum = ""
                    Exit Do
                Case "<DTPOSTED>"
                    strToken = strGetToken()
                    mobjUtil.strTrxDate = Mid(strToken, 5, 2) & "/" & Mid(strToken, 7, 2) & "/" & Mid(strToken, 1, 4)
                Case "<NAME>"
                    mobjUtil.strTrxPayee = strGetToken()
                Case "<FITID>"
                    mobjUtil.strTrxUniqueKey = strGetToken()
                Case "<TRNAMT>"
                    mobjUtil.strTrxAmount = strGetToken()
                Case "<TRNTYPE>"
                    strToken = strGetToken()
                    If strToken = "POS" Then
                        mobjUtil.strTrxNumber = "Card"
                    ElseIf (strToken = "DEP") Or (strToken = "DIRECTDEP") Then
                        mobjUtil.strTrxNumber = "DEP"
                    ElseIf strToken = "CHECK" Then
                        'Will be overridden later
                        mobjUtil.strTrxNumber = "CHECK"
                    Else
                        mobjUtil.strTrxNumber = "Pmt"
                    End If
                Case "<CHECKNUM>"
                    strCheckNum = strGetToken()
                    Do
                        If Left(strCheckNum, 1) = "0" Then
                            strCheckNum = Mid(strCheckNum, 2)
                        Else
                            Exit Do
                        End If
                    Loop
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
        gNestedErrorTrap("ImportBankDownloadOFX." & strRoutine)
    End Sub

    Private Function strGetToken() As String
        Dim intEndPos As Short
        mstrInputLine = Trim(mstrInputLine)
        If mstrInputLine = "" Then
            If EOF(mintFile) Then
                strGetToken = ""
                mblnInputEOF = True
                Exit Function
            End If
            mstrInputLine = LineInput(mintFile)
        End If
        If Left(mstrInputLine, 1) = "<" Then
            intEndPos = InStr(mstrInputLine, ">")
            strGetToken = strUnescape(Left(mstrInputLine, intEndPos))
            mstrInputLine = Mid(mstrInputLine, intEndPos + 1)
        Else
            intEndPos = InStr(mstrInputLine, "<")
            If intEndPos > 0 Then
                strGetToken = strUnescape(Left(mstrInputLine, intEndPos - 1))
                mstrInputLine = Mid(mstrInputLine, intEndPos)
            Else
                strGetToken = strUnescape(mstrInputLine)
                mstrInputLine = ""
            End If
        End If
    End Function

    Private Function strUnescape(ByVal strInput As String) As String
        strUnescape = Replace(strInput, "&amp;", "&")
    End Function
End Class
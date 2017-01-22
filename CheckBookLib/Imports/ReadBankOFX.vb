Option Strict On
Option Explicit On

Imports System.IO

Public Class ReadBankOFX
    Implements ITrxReader
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Implement ITrxReader for an OFX file downloaded from an online banking service.
    'The downloaded data will not have category or split
    'information, and the payee names will be passed through a translation algorithm
    'to scrub them and provide a category based on matching them to a predefined
    'list of possible payees. Payees not in the list are allowed, but no category
    'information will be added for them.


    Private mobjFile As TextReader
    Private mstrFile As String
    Private mstrInputLine As String
    Private mblnInputEOF As Boolean
    Private mobjUtil As ImportUtilities

    Public Sub New(ByVal objFile As TextReader, ByVal strFile As String)
        mobjFile = objFile
        mstrFile = strFile
    End Sub

    Private Function blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxReader.blnOpenSource
        Dim strHeaderLine As String

        Try

            mobjUtil = New ImportUtilities
            mobjUtil.Init(objAccount_)
            mobjUtil.LoadTrxTypeTable()

            strHeaderLine = mobjFile.ReadLine()
            If strHeaderLine = "OFXHEADER:100" Then
                mobjUtil.blnMakeFakeTrx = False
            Else
                MsgBox("File is not an OFX file.", MsgBoxStyle.Critical)
                mobjFile.Close()
                Exit Function
            End If
            mobjFile.ReadLine()
            strHeaderLine = mobjFile.ReadLine()
            If strHeaderLine <> "VERSION:102" Then
                MsgBox("File is not correct OFX version.", MsgBoxStyle.Critical)
                mobjFile.Close()
                Exit Function
            End If
            Do
                strHeaderLine = mobjFile.ReadLine()
                If strHeaderLine Is Nothing Then
                    MsgBox("Unexpected EOF in header section of OFX file.", MsgBoxStyle.Critical)
                    mobjFile.Close()
                    Exit Function
                End If
                If strHeaderLine = "" Then
                    Exit Do
                End If
            Loop

            blnOpenSource = True
            mblnInputEOF = False
            mstrInputLine = ""

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub CloseSource() Implements ITrxReader.CloseSource
        If Not mobjFile Is Nothing Then
            mobjFile.Close()
            mobjFile = Nothing
        End If
    End Sub

    Private Function objNextTrx() As ImportedTrx Implements ITrxReader.objNextTrx
        Dim strToken As String
        Dim strCheckNum As String

        objNextTrx = Nothing
        Try

            objNextTrx = Nothing
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
                        objNextTrx = mobjUtil.objMakeTrx()
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
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private ReadOnly Property strSource() As String Implements ITrxReader.strSource
        Get
            strSource = mstrFile
        End Get
    End Property

    Private Function strGetToken() As String
        Dim intEndPos As Integer
        mstrInputLine = Trim(mstrInputLine)
        If mstrInputLine = "" Then
            mstrInputLine = mobjFile.ReadLine()
            If mstrInputLine Is Nothing Then
                strGetToken = ""
                mblnInputEOF = True
                Exit Function
            End If
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
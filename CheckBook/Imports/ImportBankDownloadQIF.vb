Option Strict Off
Option Explicit On

Imports System.IO
Imports CheckBookLib

Public Class ImportBankDownloadQIF
    Implements ITrxReader
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Implement ITrxImport for a QIF file downloaded from an online banking service
    'such as FSBLink. The downloaded data will not have category or split
    'information, and the payee names will be passed through a translation algorithm
    'to scrub them and provide a category based on matching them to a predefined
    'list of possible payees. Payees not in the list are allowed, but no category
    'information will be added for them.

    Private mobjFile As TextReader
    Private mstrFile As String
    Private mobjUtil As ImportUtilities

    Public Sub New(ByVal objFile As TextReader, ByVal strFile As String)
        mobjFile = objFile
        mstrFile = strFile
    End Sub

    Private Function ITrxImport_blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxReader.blnOpenSource
        Dim strFirstLine As String

        Try

            mobjUtil = New ImportUtilities
            mobjUtil.Init(objAccount_)
            mobjUtil.LoadTrxTypeTable()

            strFirstLine = mobjFile.ReadLine()
            If strFirstLine = "!Type:Bank" Then
                mobjUtil.blnMakeFakeTrx = False
            ElseIf strFirstLine = "!Type:Docs" Then
                mobjUtil.blnMakeFakeTrx = True
            Else
                MsgBox("File is not a QIF file.", MsgBoxStyle.Critical)
                mobjFile.Close()
                Exit Function
            End If

            ITrxImport_blnOpenSource = True

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub ITrxImport_CloseSource() Implements ITrxReader.CloseSource
        If Not mobjFile Is Nothing Then
            mobjFile.Close()
            mobjFile = Nothing
        End If
    End Sub

    Private Function ITrxImport_objNextTrx() As ImportedTrx Implements ITrxReader.objNextTrx
        Dim strLine As String
        Dim strPrefix As String
        Dim strDate As String

        ITrxImport_objNextTrx = Nothing
        Try

            ITrxImport_objNextTrx = Nothing
            mobjUtil.ClearSavedTrxData()
            Do
                strLine = mobjFile.ReadLine()
                If strLine Is Nothing Then
                    Exit Do
                End If
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
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private ReadOnly Property ITrxImport_strSource() As String Implements ITrxReader.strSource
        Get
            ITrxImport_strSource = mstrFile
        End Get
    End Property
End Class
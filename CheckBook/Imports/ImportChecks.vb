Option Strict Off
Option Explicit On

Imports System.IO
Imports CheckBookLib

Public Class ImportChecks
    Implements ITrxImport

    Private mobjFile As TextReader
    Private mstrFile As String
    Private mobjSpecs As ImportChecksSpec
    Private mobjUtil As ImportUtilities

    Public Sub New(ByVal objFile As TextReader, ByVal strFile As String, ByVal objSpecs As ImportChecksSpec)
        mobjFile = objFile
        mstrFile = strFile
        mobjSpecs = objSpecs
    End Sub

    Private Function ITrxImport_blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxImport.blnOpenSource

        Try

            mobjUtil = New ImportUtilities
            mobjUtil.Init(objAccount_)
            mobjUtil.LoadTrxTypeTable()
            mobjUtil.blnMakeFakeTrx = False
            mobjUtil.blnNoImportKey = True

            ITrxImport_blnOpenSource = True

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub ITrxImport_CloseSource() Implements ITrxImport.CloseSource
        If Not mobjFile Is Nothing Then
            mobjFile.Close()
            mobjFile = Nothing
        End If
    End Sub

    Private Function ITrxImport_objNextTrx() As ImportedTrx Implements ITrxImport.objNextTrx
        Dim strLine As String
        Dim astrParts() As String

        ITrxImport_objNextTrx = Nothing
        Try

            ITrxImport_objNextTrx = Nothing
            Do
                mobjUtil.ClearSavedTrxData()

                strLine = mobjFile.ReadLine()
                If strLine Is Nothing Then
                    Exit Function
                End If
                astrParts = gaSplit(Trim(strLine), vbTab)
                mobjUtil.strTrxNumber = mobjSpecs.strConvertTrxNum(astrParts(mobjSpecs.NumberColumn))
                mobjUtil.strTrxDate = astrParts(mobjSpecs.DateColumn)
                mobjUtil.strTrxPayee = astrParts(mobjSpecs.DescrColumn)
                If (mobjSpecs.MemoColumn >= 0) Then
                    mobjUtil.strTrxMemo = astrParts(mobjSpecs.MemoColumn)
                End If
                If astrParts(mobjSpecs.AmountColumn).StartsWith("-") Then
                    mobjUtil.strTrxAmount = astrParts(mobjSpecs.AmountColumn)
                Else
                    mobjUtil.strTrxAmount = "-" + astrParts(mobjSpecs.AmountColumn)
                End If
                If Not mobjSpecs.blnSkipRecord(mobjUtil) Then
                    Exit Do
                End If
            Loop

            ITrxImport_objNextTrx = mobjUtil.objMakeTrx()

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private ReadOnly Property ITrxImport_strSource() As String Implements ITrxImport.strSource
        Get
            ITrxImport_strSource = mstrFile
        End Get
    End Property
End Class

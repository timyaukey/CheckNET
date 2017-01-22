Option Strict Off
Option Explicit On

Imports System.IO

Public Class ReadChecks
    Implements ITrxReader

    Private mobjFile As TextReader
    Private mstrFile As String
    Private mobjSpecs As ReadChecksSpec
    Private mobjUtil As ImportUtilities

    Public Sub New(ByVal objFile As TextReader, ByVal strFile As String, ByVal objSpecs As ReadChecksSpec)
        mobjFile = objFile
        mstrFile = strFile
        mobjSpecs = objSpecs
    End Sub

    Private Function blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxReader.blnOpenSource

        Try

            mobjUtil = New ImportUtilities
            mobjUtil.Init(objAccount_)
            mobjUtil.LoadTrxTypeTable()
            mobjUtil.blnMakeFakeTrx = False
            mobjUtil.blnNoImportKey = True

            blnOpenSource = True

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
        Dim strLine As String
        Dim astrParts() As String

        objNextTrx = Nothing
        Try

            objNextTrx = Nothing
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

            objNextTrx = mobjUtil.objMakeTrx()

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
End Class

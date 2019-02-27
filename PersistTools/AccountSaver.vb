Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

Public Class AccountSaver
    Private mobjAccount As Account
    Private mobjCompany As Company
    'File for Save().
    Private mobjSaveFile As StreamWriter

    Public Sub New(ByVal objAccount As Account)
        mobjAccount = objAccount
        mobjCompany = mobjAccount.objCompany
    End Sub

    Public ReadOnly Property objAccount As Account
        Get
            Return mobjAccount
        End Get
    End Property

    Public Sub Save(ByVal strPath_ As String)
        Dim objReg As Register
        Dim objSubTypeMatched As Account.SubTypeDef = Nothing

        Try

            mobjSaveFile = New StreamWriter(mobjCompany.strAccountPath() & "\" & strPath_)

            SaveLine("FHCKBK2")
            If mobjAccount.strTitle <> "" Then
                SaveLine("AT" & mobjAccount.strTitle)
            End If
            SaveLine("AK" & CStr(mobjAccount.intKey))
            For Each objSubType As Account.SubTypeDef In Account.arrSubTypeDefs
                If objSubType.lngSubType = mobjAccount.lngSubType Then
                    objSubTypeMatched = objSubType
                End If
            Next
            If objSubTypeMatched Is Nothing Then
                Throw New Exception("Could not match account subtype in save for " + mobjAccount.strTitle)
            End If
            SaveLine("AY" & objSubTypeMatched.strSaveCode)
            'Define each register at the top of the file.
            For Each objReg In mobjAccount.colRegisters
                If Not objReg.blnDeleted Then
                    SaveDefineRegister(objReg)
                End If
            Next objReg
            SaveRelatedAcct(mobjAccount.objRelatedAcct1, "1")
            SaveRelatedAcct(mobjAccount.objRelatedAcct2, "2")
            SaveRelatedAcct(mobjAccount.objRelatedAcct3, "3")
            SaveRelatedAcct(mobjAccount.objRelatedAcct4, "4")
            'Save the transactions for each register.
            For Each objReg In mobjAccount.colRegisters
                If Not objReg.blnDeleted Then
                    SaveLoadedRegister(objReg)
                End If
            Next objReg
            SaveLine(".A")

            mobjAccount.blnUnsavedChanges = False

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        Finally
            If Not mobjSaveFile Is Nothing Then
                mobjSaveFile.Close()
                mobjSaveFile = Nothing
            End If
        End Try
    End Sub

    Private Sub SaveRelatedAcct(ByVal objRelatedAccount As Account, ByVal strSelector As String)
        If Not objRelatedAccount Is Nothing Then
            SaveLine("AR" + strSelector + " " + objRelatedAccount.intKey.ToString())
        End If
    End Sub

    Private Sub SaveDefineRegister(ByVal objReg As Register)
        With objReg
            SaveLine("RK" & .strRegisterKey)
            SaveLine("RT" & .strTitle)
            If .blnShowInitially Then
                SaveLine("RS")
            End If
            SaveLine("RI")
        End With
    End Sub

    '$Description Save one Register for Save(). Writes real, fake non-generated
    '   and fake generated Trx for LoadedRegister.

    Private Sub SaveLoadedRegister(ByVal objReg As Register)
        Dim objSaver As RegisterSaver
        Dim colFakeLines As ICollection(Of String)
        Dim strLine As String

        objSaver = New RegisterSaver

        'Output the non-fake Trx, and remember the non-generated fake.
        colFakeLines = New List(Of String)
        SaveLine("RL" & objReg.strRegisterKey)
        objSaver.Save(mobjSaveFile, objReg, colFakeLines)
        SaveLine(".R")

        'Save non-generated fake Trx we saved above.
        SaveLine("RF" & objReg.strRegisterKey)
        For Each strLine In colFakeLines
            SaveLine(strLine)
        Next strLine
        SaveLine(".R")

        'RR line is for repeating register, no longer used.

        objReg.LogSave()
        objReg.WriteEventLog(System.IO.Path.GetFileNameWithoutExtension(mobjAccount.strFileNameRoot), mobjAccount.objRepeats)
    End Sub

    '$Description Write one line to the Save() output file.
    '$Param strLine The line to write.

    Friend Sub SaveLine(ByVal strLine As String)
        mobjSaveFile.WriteLine(strLine)
    End Sub


End Class

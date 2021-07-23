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
        mobjCompany = mobjAccount.Company
    End Sub

    Public ReadOnly Property Account As Account
        Get
            Return mobjAccount
        End Get
    End Property

    Public Sub Save(ByVal strPath_ As String)
        Dim objReg As Register
        Dim objSubTypeMatched As Account.SubTypeDef = Nothing

        Try

            mobjSaveFile = New StreamWriter(mobjCompany.AccountsFolderPath() & "\" & strPath_)

            SaveLine("FHCKBK2")
            If mobjAccount.Title <> "" Then
                SaveLine("AT" & mobjAccount.Title)
            End If
            SaveLine("AK" & CStr(mobjAccount.AccountKey))
            For Each objSubType As Account.SubTypeDef In Account.SubTypeDefs
                If objSubType.lngSubType = mobjAccount.AcctSubType Then
                    objSubTypeMatched = objSubType
                End If
            Next
            If objSubTypeMatched Is Nothing Then
                Throw New Exception("Could not match account subtype in save for " + mobjAccount.Title)
            End If
            SaveLine("AY" & objSubTypeMatched.strSaveCode)
            'Define each register at the top of the file.
            For Each objReg In mobjAccount.Registers
                If Not objReg.IsDeleted Then
                    SaveDefineRegister(objReg)
                End If
            Next objReg
            SaveRelatedAcct(mobjAccount.RelatedAcct1, "1")
            SaveRelatedAcct(mobjAccount.RelatedAcct2, "2")
            SaveRelatedAcct(mobjAccount.RelatedAcct3, "3")
            SaveRelatedAcct(mobjAccount.RelatedAcct4, "4")
            'Save the transactions for each register.
            For Each objReg In mobjAccount.Registers
                If Not objReg.IsDeleted Then
                    SaveLoadedRegister(objReg)
                End If
            Next objReg
            SaveLine(".A")

            mobjAccount.HasUnsavedChanges = False

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
            SaveLine("AR" + strSelector + " " + objRelatedAccount.AccountKey.ToString())
        End If
    End Sub

    Private Sub SaveDefineRegister(ByVal objReg As Register)
        With objReg
            SaveLine("RK" & .RegisterKey)
            SaveLine("RT" & .Title)
            If .ShowInitially Then
                SaveLine("RS")
            End If
            SaveLine("RI")
        End With
    End Sub

    '$Description Save one Register for Save(). Writes real, fake non-generated
    '   and fake generated BaseTrx for LoadedRegister.

    Private Sub SaveLoadedRegister(ByVal objReg As Register)
        Dim objSaver As RegisterSaver
        Dim colFakeLines As ICollection(Of String)
        Dim strLine As String

        objSaver = New RegisterSaver

        'Output the non-fake BaseTrx, and remember the non-generated fake.
        colFakeLines = New List(Of String)
        SaveLine("RL" & objReg.RegisterKey)
        objSaver.Save(mobjSaveFile, objReg, colFakeLines)
        SaveLine(".R")

        'Save non-generated fake BaseTrx we saved above.
        SaveLine("RF" & objReg.RegisterKey)
        For Each strLine In colFakeLines
            SaveLine(strLine)
        Next strLine
        SaveLine(".R")

        'RR line is for repeating register, no longer used.

        objReg.LogSave()
        objReg.WriteEventLog(System.IO.Path.GetFileNameWithoutExtension(mobjAccount.FileNameRoot), mobjAccount.Repeats)
    End Sub

    '$Description Write one line to the Save() output file.
    '$Param strLine The line to write.

    Friend Sub SaveLine(ByVal strLine As String)
        mobjSaveFile.WriteLine(strLine)
    End Sub


End Class

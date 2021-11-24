Option Strict On
Option Explicit On

Public Class CompanySaver

    Public Shared Sub Unload(ByVal objCompany As Company)
        objCompany.FireBeforeUnload()
        objCompany.Teardown()
        objCompany.UnlockCompany()
        objCompany.FireAfterUnload()
    End Sub

    Public Shared Sub SaveChangedAccounts(ByVal objCompany As Company)
        Dim objAccount As Account
        Dim strBackupFile As String
        If objCompany.AnyCriticalOperationFailed Then
            Throw New Register.CriticalOperationException("Unable to save company because a critical operation failed earlier")
        End If
        objCompany.FireBeforeSaveCompany()
        For Each objAccount In objCompany.Accounts
            If objAccount.HasUnsavedChanges Then
                strBackupFile = objCompany.BackupsFolderPath() & "\" & objAccount.FileNameRoot & "." & Now.ToString("MM$dd$yy$hh$mm")
                If Len(Dir(strBackupFile)) > 0 Then
                    Kill(strBackupFile)
                End If
                Rename(objCompany.AccountsFolderPath() & "\" & objAccount.FileNameRoot, strBackupFile)
                PurgeAccountBackups(objAccount)
                Dim objSaver As AccountSaver = New AccountSaver(objAccount)
                objSaver.Save(objAccount.FileNameRoot)
                objCompany.FireSavedAccount(objAccount.Title)
            End If
        Next objAccount
        objCompany.FireAfterSaveCompany()
    End Sub

    Private Shared Sub PurgeAccountBackups(ByVal objAccount As Account)
        'Backups older than the upper bound of this array (in days) will be deleted.
        Dim adatDays(30) As BackupPurgeDay
        Dim strBackup As String
        Dim strParsableDate As String
        Dim datCreateDate As Date
        Dim strEncodedDate As String
        Dim intIndex As Integer
        Dim intBackupsToKeep As Integer
        Dim intAgeInDays As Integer
        Dim colInstances As List(Of BackupInstance)
        Dim colOlderFiles As List(Of String) = New List(Of String)

        Try

            For intIndex = LBound(adatDays) To UBound(adatDays)
                Dim backupPurgeDay As BackupPurgeDay = New BackupPurgeDay()
                adatDays(intIndex) = backupPurgeDay
                adatDays(intIndex).Instances = New List(Of BackupInstance)
            Next

            strBackup = Dir(objAccount.Company.BackupsFolderPath() & "\" & objAccount.FileNameRoot & ".*", FileAttribute.Normal)
            Do While strBackup <> ""
                strEncodedDate = Mid(strBackup, InStr(UCase(strBackup), ".ACT.") + 5)
                strParsableDate = "20" & Mid(strEncodedDate, 7, 2) & "/" & Mid(strEncodedDate, 1, 2) & "/" & Mid(strEncodedDate, 4, 2) & " " & Mid(strEncodedDate, 10, 2) & ":" & Mid(strEncodedDate, 13, 2)
                datCreateDate = CDate(strParsableDate)
                intAgeInDays = CInt(DateDiff(Microsoft.VisualBasic.DateInterval.Day, datCreateDate, Today))
                If intAgeInDays <= UBound(adatDays) Then
                    Dim inst As BackupInstance = New BackupInstance With {
                        .CreateDate = datCreateDate,
                        .Name = strBackup
                    }
                    adatDays(intAgeInDays).Instances.Add(inst)
                Else
                    colOlderFiles.Add(strBackup)
                End If
                strBackup = Dir()
            Loop

            'Delete the very old backups
            For Each strBackup In colOlderFiles
                Kill(objAccount.Company.BackupsFolderPath() & "\" & strBackup)
            Next

            'Delete everything but the "intBackupsToKeep" most recent backups created on each date.
            For intAgeInDays = 0 To UBound(adatDays)
                If intAgeInDays = 0 Then
                    'Keep all backups from the current date.
                    intBackupsToKeep = 100
                ElseIf intAgeInDays < 5 Then
                    intBackupsToKeep = 10
                Else
                    intBackupsToKeep = 1
                End If

                colInstances = adatDays(intAgeInDays).Instances
                colInstances.Sort(AddressOf BackupInstanceComparer)
                For intIndex = 1 To colInstances.Count() - intBackupsToKeep
                    strBackup = colInstances(intIndex - 1).Name
                    Kill(objAccount.Company.BackupsFolderPath() & "\" & strBackup)
                Next
            Next

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Shared Function BackupInstanceComparer(ByVal i1 As BackupInstance, ByVal i2 As BackupInstance) As Integer
        Return i1.CreateDate.CompareTo(i2.CreateDate)
    End Function

    Private Class BackupPurgeDay
        Public Instances As List(Of BackupInstance)
    End Class

    Private Class BackupInstance
        Public CreateDate As Date
        Public Name As String
    End Class

End Class

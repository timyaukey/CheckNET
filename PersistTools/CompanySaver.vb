Option Strict On
Option Explicit On

Public Class CompanySaver

    Public Shared Sub SaveChangedAccounts(ByVal objCompany As Company)
        Dim objAccount As Account
        Dim strBackupFile As String
        For Each objAccount In objCompany.colAccounts
            If objAccount.blnUnsavedChanges Then
                strBackupFile = objCompany.strBackupPath() & "\" & objAccount.strFileNameRoot & "." & Now.ToString("MM$dd$yy$hh$mm")
                If Len(Dir(strBackupFile)) > 0 Then
                    Kill(strBackupFile)
                End If
                Rename(objCompany.strAccountPath() & "\" & objAccount.strFileNameRoot, strBackupFile)
                PurgeAccountBackups(objAccount)
                Dim objSaver As AccountSaver = New AccountSaver(objAccount)
                objSaver.Save(objAccount.strFileNameRoot)
                objCompany.FireSavedAccount(objAccount.strTitle)
            End If
        Next objAccount
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
                adatDays(intIndex).colInstances = New List(Of BackupInstance)
            Next

            strBackup = Dir(objAccount.objCompany.strBackupPath() & "\" & objAccount.strFileNameRoot & ".*", FileAttribute.Normal)
            Do While strBackup <> ""
                strEncodedDate = Mid(strBackup, InStr(UCase(strBackup), ".ACT.") + 5)
                strParsableDate = "20" & Mid(strEncodedDate, 7, 2) & "/" & Mid(strEncodedDate, 1, 2) & "/" & Mid(strEncodedDate, 4, 2) & " " & Mid(strEncodedDate, 10, 2) & ":" & Mid(strEncodedDate, 13, 2)
                datCreateDate = CDate(strParsableDate)
                intAgeInDays = CInt(DateDiff(Microsoft.VisualBasic.DateInterval.Day, datCreateDate, Today))
                If intAgeInDays <= UBound(adatDays) Then
                    Dim inst As BackupInstance = New BackupInstance With {
                        .datCreate = datCreateDate,
                        .strName = strBackup
                    }
                    adatDays(intAgeInDays).colInstances.Add(inst)
                Else
                    colOlderFiles.Add(strBackup)
                End If
                strBackup = Dir()
            Loop

            'Delete the very old backups
            For Each strBackup In colOlderFiles
                Kill(objAccount.objCompany.strBackupPath() & "\" & strBackup)
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

                colInstances = adatDays(intAgeInDays).colInstances
                colInstances.Sort(AddressOf BackupInstanceComparer)
                For intIndex = 1 To colInstances.Count() - intBackupsToKeep
                    strBackup = colInstances(intIndex - 1).strName
                    Kill(objAccount.objCompany.strBackupPath() & "\" & strBackup)
                Next
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Shared Function BackupInstanceComparer(ByVal i1 As BackupInstance, ByVal i2 As BackupInstance) As Integer
        Return i1.datCreate.CompareTo(i2.datCreate)
    End Function

    Private Class BackupPurgeDay
        Public colInstances As List(Of BackupInstance)
    End Class

    Private Class BackupInstance
        Public datCreate As Date
        Public strName As String
    End Class

End Class

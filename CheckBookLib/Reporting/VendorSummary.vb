Option Strict On
Option Explicit On

Public Class VendorSummary
    Public strVendorName As String
    Public curBalance As Decimal

    Public Shared Function colScanVendors(ByVal objCompany As Company, ByVal datEnd As DateTime,
                                          ByVal lngSubType As Account.SubType) As List(Of VendorSummary)
        Dim colVendors As List(Of VendorSummary) = New List(Of VendorSummary)()
        Dim objDict As Dictionary(Of String, VendorSummary) = New Dictionary(Of String, VendorSummary)()
        Dim datAmount As Date

        For Each objAccount As Account In objCompany.colAccounts
            If objAccount.lngSubType = lngSubType Then
                For Each objReg As Register In objAccount.colRegisters
                    For Each objTrx As Trx In objReg.colDateRange(New DateTime(1900, 1, 1), datEnd)
                        If Not objTrx.blnFake Then
                            Dim objNormalTrx As NormalTrx = TryCast(objTrx, NormalTrx)
                            If Not objNormalTrx Is Nothing Then
                                Dim objVendor As VendorSummary = objGetVendorSummary(colVendors, objDict, objTrx)
                                For Each objSplit As TrxSplit In objNormalTrx.colSplits
                                    objVendor.curBalance += objSplit.curAmount
                                    datAmount = objSplit.datDueDateEffective
                                Next
                            Else
                                Dim objReplicaTrx As ReplicaTrx = TryCast(objTrx, ReplicaTrx)
                                If Not objReplicaTrx Is Nothing Then
                                    Dim objVendor As VendorSummary = objGetVendorSummary(colVendors, objDict, objTrx)
                                    objVendor.curBalance += objReplicaTrx.curAmount
                                    datAmount = objReplicaTrx.datDate
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        Next
        colVendors.Sort(AddressOf intVendorComparer)

        Return colVendors
    End Function

    Private Shared Function objGetVendorSummary(ByVal colVendors As List(Of VendorSummary),
                                         ByVal objDict As Dictionary(Of String, VendorSummary),
                                         ByVal objTrx As Trx) As VendorSummary
        Dim objVendor As VendorSummary = Nothing
        Dim strVendorKey As String = objTrx.strDescription.ToUpper()
        If strVendorKey.Length > 16 Then
            strVendorKey = strVendorKey.Substring(0, 16)
        End If
        If Not objDict.TryGetValue(strVendorKey, objVendor) Then
            objVendor = New VendorSummary()
            objVendor.strVendorName = objTrx.strDescription
            objVendor.curBalance = 0D
            colVendors.Add(objVendor)
            objDict.Add(strVendorKey, objVendor)
        End If
        Return objVendor
    End Function

    Private Shared Function intVendorComparer(ByVal objVendor1 As VendorSummary, ByVal objVendor2 As VendorSummary) As Integer
        Return objVendor1.strVendorName.CompareTo(objVendor2.strVendorName)
    End Function
End Class

Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public Class SearchComparers
    Public Shared Function objGetComparer(ByVal cboSearchType As Windows.Forms.ComboBox) As SearchComparerDelegate
        Dim lngSearchType As Trx.TrxSearchType = CType(UITools.GetItemData(cboSearchType, cboSearchType.SelectedIndex), Trx.TrxSearchType)
        Select Case lngSearchType
            Case Trx.TrxSearchType.EqualTo
                Return AddressOf SearchComparers.blnEqualTo
            Case Trx.TrxSearchType.StartsWith
                Return AddressOf SearchComparers.blnStartsWith
            Case Trx.TrxSearchType.Contains
                Return AddressOf SearchComparers.blnContains
            Case Else
                Throw New Exception("Unrecognized search type")
        End Select
    End Function

    Public Shared Function blnEqualTo(ByVal str1 As String, ByVal str2 As String) As Boolean
        Return StrComp(str1, str2, CompareMethod.Text) = 0
    End Function

    Public Shared Function blnStartsWith(ByVal str1 As String, ByVal str2 As String) As Boolean
        Return StrComp(Left(str1, Len(str2)), str2, CompareMethod.Text) = 0
    End Function

    Public Shared Function blnContains(ByVal str1 As String, ByVal str2 As String) As Boolean
        Return InStr(1, str1, str2, CompareMethod.Text) > 0
    End Function
End Class

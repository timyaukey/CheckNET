Option Strict Off
Option Explicit On

Imports System.Collections.Generic

Friend Class RepeatSeqInfoForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjHostUI As IHostUI
    Private mobjReg As Register
    Private mstrRepeatKey As String

    Public Sub ShowMe(ByVal objHostUI As IHostUI, ByVal objReg As Register, ByVal strRepeatKey As String)
        mobjHostUI = objHostUI
        mobjReg = objReg
        If strRepeatKey = "" Then
            mobjHostUI.InfoMessageBox("Transaction has no repeat key.")
            Exit Sub
        End If
        mstrRepeatKey = strRepeatKey
        Me.ShowDialog()
    End Sub

    Private Sub RepeatSeqInfoForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objTrx As BaseTrx
        Dim colRows As List(Of RptGridRow) = New List(Of RptGridRow)()
        Dim objRow As RptGridRow

        For Each objTrx In mobjReg.GetAllTrx(Of BaseTrx)()
            If objTrx.RepeatKey = mstrRepeatKey Then
                objRow = New RptGridRow
                objRow.TrxDate = objTrx.TrxDate.ToString(Utilities.DateFormatWithTwoDigitYear)
                objRow.Descr = objTrx.Description
                objRow.Amount = Utilities.FormatCurrency(objTrx.Amount)
                objRow.SeqNum = objTrx.RepeatSeq.ToString()
                If TypeOf objTrx Is BankTrx Then
                    objRow.DueDate = DirectCast(objTrx, BankTrx).SummarizeDueDates()
                Else
                    objRow.DueDate = ""
                End If
                objRow.Type = objTrx.FakeStatusLabel
                colRows.Add(objRow)
            End If
        Next

        grdTrx.AutoGenerateColumns = False
        grdTrx.DataSource = colRows
    End Sub

    Private Class RptGridRow
        Private mTrxDate As String
        Public Property TrxDate() As String
            Get
                TrxDate = mTrxDate
            End Get
            Set(ByVal value As String)
                mTrxDate = value
            End Set
        End Property

        Private mDescr As String
        Public Property Descr() As String
            Get
                Descr = mDescr
            End Get
            Set(ByVal value As String)
                mDescr = value
            End Set
        End Property

        Private mAmount As String
        Public Property Amount() As String
            Get
                Amount = mAmount
            End Get
            Set(ByVal value As String)
                mAmount = value
            End Set
        End Property

        Private mSeqNum As String
        Public Property SeqNum() As String
            Get
                SeqNum = mSeqNum
            End Get
            Set(ByVal value As String)
                mSeqNum = value
            End Set
        End Property

        Private mDueDate As String
        Public Property DueDate() As String
            Get
                DueDate = mDueDate
            End Get
            Set(ByVal value As String)
                mDueDate = value
            End Set
        End Property

        Private mType As String
        Public Property Type() As String
            Get
                Type = mType
            End Get
            Set(ByVal value As String)
                mType = value
            End Set
        End Property
    End Class
End Class
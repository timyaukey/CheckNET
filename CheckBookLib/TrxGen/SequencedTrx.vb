Option Strict On
Option Explicit On

Public Class SequencedTrx

    Public TrxDate As Date
    Public Amount As Decimal
    Public RepeatSeq As Integer
    Public SkipSeqNum As Boolean

    Public Sub Init(ByVal datDate_ As Date, ByVal curAmount_ As Decimal, ByVal intRepeatSeq_ As Integer)
        TrxDate = datDate_
        Amount = curAmount_
        RepeatSeq = intRepeatSeq_
        SkipSeqNum = False
    End Sub
End Class
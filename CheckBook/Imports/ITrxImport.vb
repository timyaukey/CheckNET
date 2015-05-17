Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Public Interface _ITrxImport
    Function blnOpenSource(ByVal objAccount_ As Account) As Boolean
    ReadOnly Property strSource() As String
    Function objNextTrx() As Trx
    Sub CloseSource()
End Interface

Public Class ITrxImport
    Implements _ITrxImport

    'This interface allows the application to retrieve a series
    'of new Trx objects from some external source.

    '$Description Locate an external source, and prepare to
    '   obtain data from it. Must be called before other members.
    '   If appropriate, may display a modal window asking the
    '   operator to identify the source to use, e.g. a file.
    '$Param objAccount_ The Account object these Trx may be added to.
    '$Returns True iff successful.

    Public Function blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements _ITrxImport.blnOpenSource

    End Function

    '$Description A string identifying the source opened by blnOpenSource().
    '   For example, implementations reading from a file could return the
    '   name of the file or some kind of title information read from it.

    Public ReadOnly Property strSource() As String Implements _ITrxImport.strSource
        Get

        End Get
    End Property

    '$Description Create and return a Trx whose data are read from the
    '   source opened by blnOpenSource(). Successive calls to ojbNextTrx()
    '   will return successive transactions from the source.
    '$Returns The new Trx, or Nothing if there were no more transactions.
    '   The Trx will have at least one Split, but the category for any
    '   Split may be a zero length string.

    Public Function objNextTrx() As Trx Implements _ITrxImport.objNextTrx

    End Function

    '$Description Close the source opened by blnOpenSource().
    '   Does things like close a file. Must be the last member used.

    Public Sub CloseSource() Implements _ITrxImport.CloseSource

    End Sub
End Class
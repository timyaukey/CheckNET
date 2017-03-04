Option Strict On
Option Explicit On

'Base class for all exceptions thrown by TrxGeneratorBase
'and all subclasses that might realistically happen, as opposed
'to things like corrupted XML files.
Public Class FilePersisterException
    Inherits Exception

    Sub New(ByVal msg_ As String)
        MyBase.New(msg_)
    End Sub

End Class

'Exception indicating an unrecognized object class.
Public Class FilePersisterTypeException
    Inherits FilePersisterException

    Public Sub New(ByVal msg_ As String)
        MyBase.New(msg_)
    End Sub
End Class

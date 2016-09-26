Public Interface IFilePersistable
    'Returns Nothing if object valid, otherwise a diagnostic message.
    Function Validate() As String
    'Set object references to null as needed for serialization.
    Sub CleanForSave()
End Interface
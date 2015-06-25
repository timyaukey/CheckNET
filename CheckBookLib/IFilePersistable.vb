Public Interface IFilePersistable
    'Returns Nothing if object valid, otherwise a diagnostic message.
    Function Validate() As String
End Interface
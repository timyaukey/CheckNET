Public Interface IFilePersister
    Function Load(ByVal strFile As String) As IFilePersistable
    Sub Save(ByVal content As IFilePersistable, ByVal strFile As String)
End Interface

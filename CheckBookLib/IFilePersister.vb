Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Interface IFilePersister
    'Load the specified file and return the contents in an IFilePersistable.
    'Throws an exception if could not load the file.
    Function Load(ByVal strFile As String) As IFilePersistable

    'Return a list of object types that can be created by Create().
    'The strType property of one of these must be passed to Create().
    'The strName properties are intended to be displayed in a UI of some kind
    'for the operator to choose from.
    Function GetTypes() As List(Of FilePersistableType)

    'Create an IFilePersistable of the specified type, which must be the the strType
    'property of one of the FilePersistableType objects returned by GetTypes().
    'The file will not actually exist until the result is passed to Save().
    Function Create(ByVal strType As String, ByVal strFile As String) As IFilePersistable

    'Save a file returned by Load() or Create(). Throws an exception if
    'file not saved, including if validation failed for some reason.
    Sub Save(ByVal objFile As IFilePersistable, ByVal strFile As String)

    'Rename the specified file. Throws an exception if file not renamed,
    'including if validation failed for some reason.
    Sub Rename(ByVal strOldName As String, ByVal strNewName As String)

    'Delete the specified file. Throws an exception if file not deleted,
    'including if validation failed for some reason.
    Sub Delete(ByVal strFile As String)
End Interface

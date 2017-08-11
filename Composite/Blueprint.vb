Public Class Blueprint
    Inherits Component

    Private Components As New List(Of Component)
    Private SlotsEmpty As New List(Of String)
    Private SlotsFilled As New List(Of String)
    Private SlotsOptional As New List(Of String)

    Public Shared Function Load(ByVal blueprintName As String) As Blueprint
        Using sr As New System.IO.StreamReader("data/blueprints.txt")

        End Using
    End Function
    Public Function Construct() As BodyPart
        Dim bp As New BodyPart
        For Each c In Components
            bp.merge(c)
        Next
        Return bp
    End Function
End Class

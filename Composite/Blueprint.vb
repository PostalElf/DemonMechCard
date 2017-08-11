Public Class Blueprint
    Inherits Component

    Private Components As New List(Of Component)
    Private SlotsEmpty As New List(Of String)
    Private SlotsFilled As New List(Of String)
    Private SlotsOptional As New List(Of String)

    Public Overloads Shared Function Load(ByVal blueprintName As String) As Blueprint
        Dim raw As Queue(Of String) = SquareBracketLoader("data/blueprints.txt", blueprintName)
        If raw Is Nothing Then Throw New Exception("Invalid BlueprintName") : Return Nothing

        Dim blueprint As New Blueprint
        With blueprint
            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key As String = ln(0).Trim
                Dim value As String = ln(1).Trim
                .Build(key, value)
            End While
        End With
        Return blueprint
    End Function
    Private Sub Build(ByVal key As String, ByVal value As String)
        Select Case key

        End Select
    End Sub
    Public Function Construct() As BodyPart
        Dim bp As New BodyPart
        For Each c In Components
            bp.merge(c)
        Next
        Return bp
    End Function
End Class

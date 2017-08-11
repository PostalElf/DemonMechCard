Public Class Blueprint
    Public DesignName As String
    Private BlueprintName As String
    Private Components As New List(Of Component)
    Private BlueprintModifier As New Component
    Private SlotsEmpty As New List(Of String)
    Private SlotsFilled As New List(Of String)

    Public Overloads Shared Function Load(ByVal blueprintName As String) As Blueprint
        Dim raw As Queue(Of String) = SquareBracketLoader("data/blueprints.txt", blueprintName)
        If raw Is Nothing Then Throw New Exception("Invalid BlueprintName") : Return Nothing

        Dim blueprint As New Blueprint
        With blueprint
            .BlueprintName = raw.Dequeue
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
            Case "Slot" : SlotsEmpty.Add(value)
            Case Else : BlueprintModifier.Build(key, value)
        End Select
    End Sub
    Public Function Construct() As BodyPart
        If SlotsEmpty.Count > 0 Then Return Nothing

        Dim bp As New BodyPart
        bp.name = DesignName
        For Each c In Components
            bp.Merge(c)
        Next
        bp.Merge(BlueprintModifier)         'remember to add BlueprintModifier!
        bp.FinalMerge()                     'call FinalMerge() to finish up loose ends from merging
        Return bp
    End Function

    Public Sub AddComponent(ByVal c As Component)
        If SlotsEmpty.Contains(c.Slot) = False Then Exit Sub

        Components.Add(c)
        SlotsEmpty.Remove(c.Slot)
        SlotsFilled.Add(c.Slot)
    End Sub
    Public Sub RemoveComponent(ByVal c As Component)
        If SlotsFilled.Contains(c.Slot) = False OrElse Components.Contains(c) = False Then Exit Sub

        Components.Remove(c)
        SlotsFilled.Remove(c.Slot)
        SlotsEmpty.Add(c.Slot)
    End Sub
End Class

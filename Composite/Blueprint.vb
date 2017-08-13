Public Class Blueprint
    Protected BlueprintName As String                       'what the blueprint pattern is called
    Protected Slot As String                                'what the bodypart slot will be called
    Protected Components As New List(Of Component)
    Protected BlueprintModifier As New Component
    Protected ComponentTypesEmpty As New List(Of String)
    Protected ComponentTypesFilled As New List(Of String)

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
    Protected Sub Build(ByVal key As String, ByVal value As String)
        Select Case key
            Case "ComponentType" : ComponentTypesEmpty.Add(value)
            Case "Slot" : Slot = value
            Case Else : BlueprintModifier.Build(key, value)
        End Select
    End Sub
    Public Function Construct(ByVal DesignName As String, ByVal finalDamageType As DamageType) As BodyPart
        If ComponentTypesEmpty.Count > 0 Then Return Nothing

        Dim bp As New BodyPart
        bp.Name = DesignName
        For Each c In Components
            bp.Merge(c)
        Next
        bp.Merge(BlueprintModifier)         'remember to add BlueprintModifier!
        bp.FinalMerge(finalDamageType)      'call FinalMerge() to finish up loose ends from merging
        bp.Slot = Slot
        Return bp
    End Function
    Public Overrides Function ToString() As String
        Return BlueprintName
    End Function

    Public Sub AddComponent(ByVal c As Component)
        If ComponentTypesEmpty.Contains(c.Slot) = False Then Exit Sub

        Components.Add(c)
        ComponentTypesEmpty.Remove(c.Slot)
        ComponentTypesFilled.Add(c.Slot)
    End Sub
    Public Sub RemoveComponent(ByVal c As Component)
        If ComponentTypesFilled.Contains(c.Slot) = False OrElse Components.Contains(c) = False Then Exit Sub

        Components.Remove(c)
        ComponentTypesFilled.Remove(c.Slot)
        ComponentTypesEmpty.Add(c.Slot)
    End Sub
End Class

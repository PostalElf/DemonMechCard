Public Class Blueprint
    Protected BlueprintName As String                       'what the blueprint pattern is called
    Protected Slot As String                                'what the bodypart slot will be called
    Protected Components As New List(Of Component)
    Protected BlueprintModifier As New Component
    Protected ComponentTypesEmpty As New List(Of String)
    Protected ComponentTypesFilled As New List(Of String)
    Protected ComponentPrices As New Dictionary(Of String, Integer)

    Public Shared Function Load(ByVal blueprintName As String) As Blueprint
        Dim raw As Queue(Of String) = SquareBracketLoader("data/blueprints.txt", blueprintName)
        If raw Is Nothing Then Throw New Exception("Invalid BlueprintName") : Return Nothing

        Dim blueprint As New Blueprint
        With blueprint
            .BlueprintName = raw.Dequeue
            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key, value As String
                If ln.Count = 2 Then
                    key = ln(0).Trim
                    value = ln(1).Trim
                Else
                    key = ln(0).Trim
                    value = ""
                End If
                .Build(key, value)
            End While
        End With
        Return blueprint
    End Function
    Protected Sub Build(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Component"
                Dim v As String() = value.Split("|")
                Dim cName As String = v(0).Trim
                Dim cCost As Integer = CInt(v(1).Trim)
                ComponentTypesEmpty.Add(cName)
                If ComponentPrices.ContainsKey(cName) = False Then ComponentPrices.Add(cName, 0)
                ComponentPrices(cName) = cCost
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
        bp.Slot = Slot
        bp.Merge(BlueprintModifier)         'remember to add BlueprintModifier!
        bp.FinalMerge(finalDamageType)      'call FinalMerge() to finish up loose ends from merging
        Return bp
    End Function
    Public Overrides Function ToString() As String
        Return BlueprintName
    End Function

    Public Shared Function LoadUserDesign(ByVal TargetUserDesignName As String) As Blueprint
        Dim raw As Queue(Of String) = SquareBracketLoader("data/user/blueprints.txt", TargetUserDesignName)
        Dim userDesignName As String = raw.Dequeue
        Dim blueprintName As String = raw.Dequeue

        'get raw blueprint first
        Dim blueprint As Blueprint = Load(blueprintName)

        'fill in with components
        While raw.Count > 0
            Dim componentName As String = raw.Dequeue
            Dim component As Component = component.Load(componentName)
            blueprint.AddComponent(component)
        End While

        Return blueprint
    End Function
    Public Sub SaveUserDesign(ByVal targetUserDesignName As String)
        Dim raw As New Queue(Of String)
        With raw
            .Enqueue(targetUserDesignName)
            .Enqueue(BlueprintName)

            For Each c In Components
                .Enqueue(c.Name)
            Next
        End With

        SquareBracketPacker("data/user/blueprints.txt", raw)
    End Sub

    Public Sub AddComponent(ByVal c As Component)
        If ComponentTypesEmpty.Contains(c.Slot) = False Then Exit Sub

        Components.Add(c)
        ComponentTypesEmpty.Remove(c.Slot)
        ComponentTypesFilled.Add(c.Slot)
    End Sub
    Public Sub AddComponent(ByVal componentName As String)
        AddComponent(Component.Load(componentName))
    End Sub
    Public Sub RemoveComponent(ByVal c As Component)
        If ComponentTypesFilled.Contains(c.Slot) = False OrElse Components.Contains(c) = False Then Exit Sub

        Components.Remove(c)
        ComponentTypesFilled.Remove(c.Slot)
        ComponentTypesEmpty.Add(c.Slot)
    End Sub
End Class

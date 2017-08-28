Public Class MechDesign
    Inherits Blueprint
    Public Overloads Shared Function Load(ByVal blueprintName As String) As MechDesign
        Dim raw As Queue(Of String) = SquareBracketLoader("data/mechdesigns.txt", blueprintName)
        If raw Is Nothing Then Throw New Exception("Invalid MechDesignName") : Return Nothing

        Dim mechdesign As New MechDesign
        With mechdesign
            .BlueprintName = raw.Dequeue
            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key As String = ln(0).Trim
                Dim value As String = ln(1).Trim
                .Build(key, value)
            End While
        End With
        Return mechdesign
    End Function
    Private Shadows Sub Build(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Slot" : ComponentTypesEmpty.Add(value)
            Case Else : BlueprintModifier.Build(key, value)
        End Select
    End Sub
    Public Shadows Function Construct(ByVal mechName As String) As Mech
        If ComponentTypesEmpty.Count > 0 Then Return Nothing

        Dim bodyparts As New List(Of BodyPart)
        For Each c As Component In Components
            If TypeOf c Is BodyPart = False Then Throw New Exception("Non-Bodypart found in mechdesign component list.") : Return Nothing
            bodyparts.Add(CType(c, BodyPart))
        Next
        Return Mech.Build(mechName, bodyparts, Inventory, BlueprintModifier)
    End Function
    Public Overrides Function ToString() As String
        Return BlueprintName
    End Function

    Public Shared Shadows Function LoadUserDesign(ByVal targetUserDesignName As String) As MechDesign
        Dim raw As Queue(Of String) = SquareBracketLoader("data/user/mechdesigns.txt", targetUserDesignName)
        Dim userDesignName As String = raw.Dequeue
        Dim blueprintName As String = raw.Dequeue

        'get raw blueprint first
        Dim MechBlueprint As MechDesign = MechDesign.Load(blueprintName)

        'fill in with bodyparts
        While raw.Count > 0
            Dim partBlueprintName As String = raw.Dequeue
            Dim partBlueprint As Blueprint = Blueprint.LoadUserDesign(partBlueprintName)
            Dim part As BodyPart = partBlueprint.Construct(partBlueprintName, Nothing)
            MechBlueprint.AddComponent(part)
        End While

        Return MechBlueprint
    End Function
    Public Shadows Sub SaveUserDesign(ByVal targetUserDesignName As String)
        Dim raw As New Queue(Of String)
        With raw
            .Enqueue(targetUserDesignName)
            .Enqueue(BlueprintName)

            For Each hw In Inventory
                .Enqueue(hw.Name)
            Next
            For Each c In Components
                .Enqueue(c.Name)
            Next
        End With

        SquareBracketPacker("data/user/mechdesigns.txt", raw)
    End Sub

    Private ReadOnly Property InventorySpace As Integer
        Get
            Dim total As Integer = 0
            For Each c In Components
                total += c.InventorySpace
            Next
            total += BlueprintModifier.InventorySpace
            Return total
        End Get
    End Property
    Private ReadOnly Property InventorySpaceFree As Integer
        Get
            Dim usedInventory As Integer = 0
            For Each w In Inventory
                usedInventory += w.HandCost
            Next

            Return InventorySpace - usedInventory
        End Get
    End Property
    Private Inventory As New List(Of BodyPart)
    Public Overloads Sub AddComponent(ByVal c As Component)
        If c.Slot = "Handweapon" Then
            'is handweapon, check inventory space and add to inventory
            If InventorySpaceFree < c.HandCost Then Exit Sub
            Inventory.Add(c)
        Else
            'is normal bodypart
            MyBase.AddComponent(c)
        End If
    End Sub
    Public Overloads Sub RemoveComponent(ByVal c As Component)
        If c.Slot = "Handweapon" Then
            If Inventory.Contains(c) = False Then Exit Sub
            Inventory.Remove(c)
        Else
            MyBase.RemoveComponent(c)
        End If
    End Sub
End Class

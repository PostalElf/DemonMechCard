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
    Private Overloads Sub Build(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Slot" : ComponentTypesEmpty.Add(value)
            Case Else : BlueprintModifier.Build(key, value)
        End Select
    End Sub
    Public Overloads Function Construct(ByVal mechName As String) As Mech
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
End Class

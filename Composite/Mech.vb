Public Class Mech
    Inherits Combatant
    Private ReadOnly Property TotalHands As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.ExtraHands
            Next
            total += BaseModifier.ExtraHands
            Return total
        End Get
    End Property
    Private ReadOnly Property TotalHandsUsed As Integer
        Get
            Dim total As Integer = 0
            For Each bp In WeaponsEquipped
                total += bp.HandCost
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property TotalHandsFree As Integer
        Get
            Return TotalHands - TotalHandsUsed
        End Get
    End Property
    Private WeaponsEquipped As New List(Of BodyPart)
    Private WeaponsInventory As New List(Of BodyPart)
    Public Function EquipWeapon(ByVal index As Integer) As String
        If WeaponsInventory.Count - 1 < index Then Return "Invalid index"
        Dim tbp As BodyPart = WeaponsInventory(index)
        If TotalHandsFree < tbp.HandCost Then Return "Insufficient hands"

        WeaponsInventory.Remove(tbp)
        WeaponsEquipped.Add(tbp)
        Return Nothing
    End Function
    Public Function UnequipWeapon(ByVal index As Integer) As String
        If WeaponsEquipped.Count - 1 < index Then Return "Invalid index"
        Dim tbp As BodyPart = WeaponsEquipped(index)

        WeaponsEquipped.Remove(tbp)
        WeaponsInventory.Add(tbp)
        Return Nothing
    End Function
    Public Overrides ReadOnly Property Attacks As List(Of BodyPart)
        Get
            Dim total As New List(Of BodyPart)
            For Each bp In BodyParts.Concat(WeaponsEquipped)
                If bp.IsReady = True Then total.Add(bp)
            Next
            Return total
        End Get
    End Property

    Public Shared Function Build(ByVal _name As String, ByVal _bodyparts As List(Of BodyPart), ByVal _inventory As List(Of BodyPart), ByVal _blueprintModifier As Component)
        Dim mech As New Mech
        With mech
            .Name = _name
            .BodyParts.AddRange(_bodyparts)
            .WeaponsInventory.AddRange(_inventory)
            For Each bp In .BodyParts : bp.Owner = mech : Next
            For Each bp In .WeaponsInventory : bp.Owner = mech : Next

            'add blueprint modifier
            .BaseModifier = _blueprintModifier
        End With
        Return mech
    End Function
    Public Overloads Sub FullReady()
        MyBase.FullReady()
        For Each weapon In WeaponsInventory
            weapon.FullReady()
        Next
        For Each weapon In WeaponsEquipped
            weapon.FullReady()
        Next
    End Sub
End Class

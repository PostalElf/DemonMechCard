Public Class Mech
    Inherits Combatant
    Private Sefirot As New Sefirot
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
    Public Function GetEquippableWeapons() As List(Of BodyPart)
        Dim total As New List(Of BodyPart)
        For Each bp In WeaponsInventory
            If EquipWeaponCheck(bp) = "" Then total.Add(bp)
        Next
        Return total
    End Function
    Private Function EquipWeaponCheck(ByVal tbp As BodyPart) As String
        If TotalHandsFree < tbp.HandCost Then Return "Insufficient hands"
        If WeaponsInventory.Contains(tbp) = False Then Return "Invalid handweapon"

        Return Nothing
    End Function
    Public Function EquipWeapon(ByVal tbp As BodyPart) As String
        If EquipWeaponCheck(tbp) <> "" Then Return EquipWeaponCheck(tbp)

        WeaponsInventory.Remove(tbp)
        WeaponsEquipped.Add(tbp)
        Return Nothing
    End Function
    Private Function UnequipWeaponCheck(ByVal tbp As BodyPart) As String
        If WeaponsEquipped.Contains(tbp) = False Then Return "Invalid handweapon"

        Return Nothing
    End Function
    Public Function UnequipWeapon(ByVal tbp As BodyPart) As String
        If UnequipWeaponCheck(tbp) <> "" Then Return UnequipWeaponCheck(tbp)

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

        Sefirot.ResetSeals()
        Sefirot.BreakSeal()
    End Sub
End Class

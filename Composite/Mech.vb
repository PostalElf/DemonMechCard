Public Class Mech
    Private ReadOnly Property TotalHands As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.ExtraHands
            Next
            total += BlueprintModifier.ExtraHands
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

    Private BodyParts As New List(Of BodyPart)
    Private BlueprintModifier As Component
    Private ReadOnly Property TotalHealth As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.Health
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property TotalHealthMax As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.healthmax
            Next
            total += BlueprintModifier.healthmax
            Return total
        End Get
    End Property
    Private ReadOnly Property HealthPercentage As Integer
        Get
            Dim total As Double = TotalHealth / TotalHealthMax * 100
            Return Math.Ceiling(total)
        End Get
    End Property

    Public Shared Function Build(ByVal _bodyparts As List(Of BodyPart), ByVal _blueprintModifier As Component)
        Dim mech As New Mech
        With mech
            'add hand weapons to inventory and everything else to bodyparts
            For Each bp In _bodyparts
                If bp.HandCost > 0 Then .WeaponsInventory.Add(bp) Else .BodyParts.Add(bp)
            Next

            .BlueprintModifier = _blueprintModifier
        End With
        Return mech
    End Function
End Class

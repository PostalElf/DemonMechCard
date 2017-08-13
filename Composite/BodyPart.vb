Public Class BodyPart
    Inherits Component
    Public Owner As Combatant
    Public Health As Integer
    Public Ammo As Integer
    Public Damage As Damage
    Public ReadOnly Property IsWeapon As Boolean
        Get
            If WeaponType = Nothing Then Return False Else Return True
        End Get
    End Property
    Public ReadOnly Property IsReady As Boolean
        Get
            If IsWeapon = False Then Return False
            If Ammo > 0 AndAlso Health > 0 Then Return True Else Return False
        End Get
    End Property

    Public Overloads Sub FinalMerge(ByVal finalDamageType As DamageType)
        If IsWeapon = True Then
            For Each ar In AttackRangesRemove
                If AttackRanges.Contains(ar) Then AttackRanges.Remove(ar)
            Next
            If DamageTypes.Contains(finalDamageType) = False Then finalDamageType = DamageType.Kinetic
            Damage = New Damage(DamageMin, DamageSpread, Accuracy, finalDamageType)
        End If
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Sub FullReady()
        Health = _HealthMax
        Ammo = AmmoMax
    End Sub
    Public Function IsAttacked(ByVal damage As Damage) As String
        Dim total As String = " hit " & Owner.Name & "'s " & Name & " for "
        Dim dmg As Integer = damage.Roll

        Dim modifier As Double = 1
        Dim modString As String = ""
        If Defences.Contains(damage.DamageType) Then modifier -= 0.5 : modString &= "DEF "

        Dim roll As Integer = Rng.Next(0, 101)
        If damage.Accuracy > Dodge Then
            'critical chance
            If roll < damage.Accuracy - Dodge Then modifier *= 2 : modString &= "CRIT "
        ElseIf Dodge > damage.Accuracy Then
            'dodge chance
            If roll < Dodge - damage.Accuracy Then modifier -= 0.5 : modString &= "DDG "
        End If

        dmg *= modifier
        total &= dmg & " " & damage.DamageType.ToString
        If modString <> "" Then total &= " [" & modString.Trim & "]"
        Return total
    End Function
End Class

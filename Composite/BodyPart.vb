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
    Public Function CheckAttackRange(ByVal attackRange As AttackRange) As Boolean
        Return AttackRanges.Contains(attackRange)
    End Function
    Public Function CheckDefences(ByVal damageType As DamageType) As Boolean
        Return Defences.Contains(damageType)
    End Function
    Public Function CheckCritDodge(ByVal accuracy As Integer) As String
        Dim roll As Integer = Rng.Next(1, 101)
        If accuracy > Dodge Then
            'crit
            If roll < accuracy - Dodge Then Return "CRIT"
        ElseIf Dodge > accuracy Then
            'dodge
            If roll < Dodge - accuracy Then Return "DDG"
        End If
        Return Nothing
    End Function

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
End Class

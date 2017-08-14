Public MustInherit Class Combatant
    Public Name As String

    Protected BaseModifier As Component
    Protected BodyParts As New List(Of BodyPart)
    Private ReadOnly Property TotalHealth As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                If bp.IsCritical = True OrElse bp.IsVital = True Then total += bp.Health
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property TotalHealthMax As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                If bp.IsCritical = True OrElse bp.IsVital = True Then total += bp.HealthMax
            Next
            Return total
        End Get
    End Property
    Public ReadOnly Property HealthPercentage As Integer
        Get
            Dim total As Double = TotalHealth / TotalHealthMax * 100
            Return Math.Ceiling(total)
        End Get
    End Property
    Public ReadOnly Property SpeedTokens As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.Speed
            Next
            total += BaseModifier.Speed

            'return /= 10 for the number of tokens it adds to the bag, minimum 1
            total /= 10
            If total <= 0 Then total = 1
            Return total
        End Get
    End Property

    Private DistanceFromMiddle As AttackRange
    Private Function GetDistance(ByVal target As Combatant) As AttackRange
        Dim total As Integer = DistanceFromMiddle
        total += target.DistanceFromMiddle

        Select Case total
            Case 0 : Return AttackRange.Close
            Case 1 : Return AttackRange.Average
            Case 2 : Return AttackRange.Far
            Case Else : Return AttackRange.Out
        End Select
    End Function
    Public MustOverride ReadOnly Property Attacks As List(Of BodyPart)
    Public Function PerformsAttack(ByVal attackLimbIndex As Integer, ByVal target As Combatant, ByVal targetLimbIndex As Integer) As String
        'set attackLimb and damage
        If attackLimbIndex < 0 OrElse attackLimbIndex > Attacks.Count Then Return "Invalid weapon!"
        Dim attackLimb As BodyPart = Attacks(attackLimbIndex)
        If attackLimb.CheckAttackRange(GetDistance(target)) = False Then Return "Target out of range!"
        If attackLimb.IsReady = False Then Return "Weapon not ready!"
        Dim damage As Damage = attackLimb.Damage

        'set targetLimb
        If targetLimbIndex < 0 OrElse targetLimbIndex > BodyParts.Count Then Return "Invalid limb target!"
        Dim targetLimb As BodyPart = target.BodyParts(targetLimbIndex)
        If CheckProtection(targetLimb) <> Nothing Then Return "Target protected by " & CheckProtection(targetLimb)

        'initialise report
        Dim total As String = Name & " hits " & target.Name & "'s " & targetLimb.Name & " for "

        'roll for damage and check for modifiers
        Dim dmg As Integer = damage.Roll
        Dim modifier As Double = 1
        Dim modString As String = ""
        If targetLimb.CheckDefences(damage.DamageType) = True Then modifier -= 0.5 : modString &= "DEF+"
        Select Case targetLimb.CheckCritDodge(damage.Accuracy)
            Case "CRIT" : modifier *= 2 : modString &= "CRIT+"
            Case "DDG" : modifier -= 0.5 : modString &= "DDG+"
        End Select
        If modString <> "" Then modString = modString.Remove(modString.Length - 1, 1)

        'apply the modifier and update report
        dmg = Math.Ceiling(dmg * modifier)
        total &= dmg & " " & damage.DamageType.ToString
        If modString <> "" Then total &= " [" & modString & "]"

        'actually do the attack
        attackLimb.Ammo -= 1
        targetLimb.Health -= dmg
        If targetLimb.Health <= 0 Then total &= vbCrLf & DestroyLimb(targetLimb)

        'return report
        Return total
    End Function
    Private Function CheckProtection(ByVal targetLimb As BodyPart) As String
        For Each bp In BodyParts
            If bp.CheckProtecting(targetLimb.Name) = True Then Return targetLimb.Name
        Next
        Return Nothing
    End Function

    Public Battlefield As Battlefield
    Private Function DestroyLimb(ByVal targetLimb As BodyPart) As String
        Dim total As String = Name & "'s " & targetLimb.Name & " is destroyed!"

        'remove limb from bodyparts
        targetLimb.Owner = Nothing
        BodyParts.Remove(targetLimb)

        'check if vital or critical
        If Not (targetLimb.IsVital = False) Then
        ElseIf Not (targetLimb.IsCritical = True AndAlso TotalHealth <= 0) Then
        Else
            total &= vbCrLf & Name & " has been annihilated!"
            DestroySelf()
        End If

        Return total
    End Function
    Private Sub DestroySelf()
        Select Case Me.GetType
            Case GetType(Enemy), GetType(Companion)
                If Battlefield Is Nothing = False Then Battlefield.RemoveCombatant(Me)
            Case GetType(Mech)
                MsgBox("CH-BABOOM!")
                End
        End Select
    End Sub
    Public Sub FullReady()
        For Each bp In BodyParts
            bp.FullReady()
        Next
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

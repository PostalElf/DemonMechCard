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
    Private HasTakenStandardAction As Boolean
    Private HasTakenQuickAction As Boolean
    Public ReadOnly Property IsReadyAct(Optional ByVal isQuick As Boolean = False) As Boolean
        Get
            If isQuick = True Then
                'quick action
                Return Not (HasTakenQuickAction)
            Else
                'standard action
                If HasTakenQuickAction = False AndAlso HasTakenStandardAction = False Then Return True
                Return False
            End If
        End Get
    End Property
    Public Sub FlagAction(Optional ByVal isQuick As Boolean = False)
        If isQuick = True Then
            'quick action
            HasTakenQuickAction = True
        Else
            'standard action
            HasTakenQuickAction = True
            HasTakenStandardAction = True
        End If
    End Sub
    Public Sub EndInit()
        'remove action flags
        HasTakenQuickAction = False
        HasTakenStandardAction = False
    End Sub

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
    Public Function GetPotentialAttacks(ByVal target As Combatant) As List(Of BodyPart)
        Dim potentialTargets = Battlefield.GetTargets(Me)
        Dim total As New List(Of BodyPart)
        For Each attack In Attacks
            If attack.CheckAttackRange(GetDistance(target)) = True Then total.Add(attack)
        Next
        Return total
    End Function
    Public Function GetPotentialTargets(ByVal attack As BodyPart) As List(Of Combatant)
        Dim potentialTargets As List(Of Combatant) = Battlefield.GetTargets(Me)
        Dim total As New List(Of Combatant)
        For Each pt In potentialTargets
            If attack.CheckAttackRange(GetDistance(pt)) = True Then total.Add(pt)
        Next
        Return total
    End Function
    Public Function GetTargetableLimbs() As List(Of BodyPart)
        Dim total As New List(Of BodyPart)
        For Each bp In BodyParts
            If CheckProtection(bp) = "" Then total.Add(bp)
        Next
        Return total
    End Function
    Public Function PerformsAttack(ByVal attackLimb As BodyPart, ByVal target As Combatant, ByVal targetLimb As BodyPart) As String
        'set attackLimb and damage
        If attackLimb.CheckAttackRange(GetDistance(target)) = False Then Return "Target out of range!"
        If attackLimb.IsReady = False Then Return "Weapon not ready!"
        Dim damage As Damage = attackLimb.Damage

        'set targetLimb
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
        If attackLimb.Ammo <> -1 Then attackLimb.Ammo -= 1
        If targetLimb.IsInvulnerable = False Then targetLimb.Health -= dmg
        If targetLimb.Health <= 0 Then total &= vbCrLf & target.DestroyLimb(targetLimb)

        'return report
        Return total
    End Function
    Public Function PerformsAttack(ByVal attackLimbIndex As Integer, ByVal target As Combatant, ByVal targetLimbIndex As Integer) As String
        If attackLimbIndex < 0 OrElse attackLimbIndex > Attacks.Count Then Return "Invalid weapon!"
        Dim attackLimb As BodyPart = Attacks(attackLimbIndex)

        If targetLimbIndex < 0 OrElse targetLimbIndex > BodyParts.Count Then Return "Invalid limb target!"
        Dim targetLimb As BodyPart = target.BodyParts(targetLimbIndex)

        Return PerformsAttack(attackLimb, target, targetLimb)
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
        If Not (targetLimb.IsVital = False) OrElse Not (targetLimb.IsCritical = True AndAlso TotalHealth <= 0) Then
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
        Return "[" & HealthPercentage.ToString("000") & "%] " & Name
    End Function
    Public Function ConsoleReport() As String
        Dim total As String = ""
        total &= Name & vbCrLf
        total &= "└ Health:   " & HealthPercentage & "%" & vbCrLf
        total &= "└ Init:     " & SpeedTokens & vbCrLf
        total &= "└ Position: " & DistanceFromMiddle.ToString & vbCrLf

        total &= "└ Limbs:" & vbCrLf
        For Each bp In BodyParts
            total &= "  └ " & bp.consolereport & vbCrLf
        Next
        Return total
    End Function
End Class

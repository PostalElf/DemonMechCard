﻿Public MustInherit Class Combatant
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
    Private ReadOnly Property TotalSpeed As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.Speed
            Next
            total += BaseModifier.Speed
            Return total
        End Get
    End Property
    Public ReadOnly Property HealthPercentage As Integer
        Get
            Dim total As Double = TotalHealth / TotalHealthMax * 100
            Return Math.Ceiling(total)
        End Get
    End Property
    Private Initiative As Integer
    Private Actions As Integer
    Private HasAttacked As Boolean
    Private HasMoved As Boolean
    Public Sub FlagAction(ByVal actionType As String)
        Select Case actionType
            Case "Attack" : HasAttacked = True : Actions -= 2
            Case "QuickAttack" : HasAttacked = True : Actions -= 1
            Case "Move" : HasMoved = True : Actions -= 1
            Case "Equip" : Actions -= 1
        End Select
    End Sub
    Public Function CheckAction(ByVal actionType As String) As Boolean
        Select Case actionType
            Case "Attack" : If HasAttacked = True OrElse Actions < 2 Then Return False Else Return True
            Case "QuickAttack" : If HasAttacked = True OrElse Actions < 1 Then Return False Else Return True
            Case "Move" : If HasMoved = True OrElse Actions < 1 Then Return False Else Return True
            Case "Equip" : If Actions < 1 Then Return False Else Return True
            Case Else : Return True
        End Select
    End Function
    Public Function InitTick() As Boolean
        Initiative -= 1
        If Initiative <= 0 Then Return True
        Return False
    End Function
    Public Sub EndInit()
        'remove action flags
        HasAttacked = False
        HasMoved = False
        Actions = 2

        'reset initiative
        Initiative = 50 - TotalSpeed
    End Sub

    Private Trap As Damage
    Private _DistanceFromMiddle As AttackRange
    Public ReadOnly Property DistanceFromMiddle As AttackRange
        Get
            Return _DistanceFromMiddle
        End Get
    End Property
    Private Function GetDistance(ByVal target As Combatant) As AttackRange
        Dim total As Integer = _DistanceFromMiddle
        total += target._DistanceFromMiddle

        Select Case total
            Case 0 : Return AttackRange.Close
            Case 1 : Return AttackRange.Average
            Case 2 : Return AttackRange.Far
            Case Else : Return AttackRange.Out
        End Select
    End Function
    Public Function PerformsMove(ByVal direction As Char) As String
        Dim total As String = ""

        'default movement is one range
        If direction = "b"c Then
            _DistanceFromMiddle += 1
            If _DistanceFromMiddle > AttackRange.Far Then _DistanceFromMiddle = AttackRange.Far
        Else
            _DistanceFromMiddle -= 1
            If _DistanceFromMiddle < AttackRange.Close Then _DistanceFromMiddle = AttackRange.Close
        End If

        'check for trap
        If Trap.Min > 0 AndAlso Trap.Max > 0 Then
            Dim targetLimb As BodyPart = GetRandom(Of BodyPart)(GetTargetableLimbs)
            total &= "Trap! It deals " & ApplyDamage(Trap, targetLimb) & " to " & targetLimb.Name & "." & vbCrLf
        End If

        total &= Name & " is now at " & DistanceFromMiddle.ToString & " range."
        Return total
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
        Dim protectors As New List(Of String)

        'add all unprotected limbs
        For Each bp In BodyParts
            Dim protector As String = CheckProtection(bp)
            If protector = "" Then total.Add(bp) Else protectors.Add(protector)
        Next

        'add all protectors
        For Each protector In protectors
            For Each bp In BodyParts
                If bp.Name = protector AndAlso total.Contains(bp) = False Then total.Add(bp)
            Next
        Next

        'return
        Return total
    End Function
    Public Function PerformsAttack(ByVal attackLimb As BodyPart, ByVal target As Combatant, ByVal targetLimb As BodyPart) As String
        'set attackLimb and damage
        If attackLimb.CheckAttackRange(GetDistance(target)) = False Then Return "Target out of range!"
        If attackLimb.IsReady = False Then Return "Weapon not ready!"
        Dim damage As Damage = attackLimb.Damage

        'set targetLimb
        If target.CheckProtection(targetLimb) <> Nothing Then Return "Target protected by " & target.CheckProtection(targetLimb)

        'remove ammo
        If attackLimb.Ammo <> -1 Then attackLimb.Ammo -= 1

        'prime report, then actually apply damage
        Dim total As String = Name & " hits " & target.Name & "'s " & targetLimb.Name & " for "
        total &= target.ApplyDamage(attackLimb.Damage, targetLimb)

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
    Public Function ApplyDamage(ByVal damage As Damage, ByVal targetLimb As BodyPart) As String
        Dim total As String = ""

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
        If targetLimb.IsInvulnerable = False Then targetLimb.Health -= dmg
        If targetLimb.Health <= 0 Then total &= vbCrLf & DestroyLimb(targetLimb)

        Return total
    End Function
    Private Function CheckProtection(ByVal targetLimb As BodyPart) As String
        For Each bp In BodyParts
            If bp.CheckProtecting("ALL") AndAlso targetLimb.Name <> bp.Name Then Return bp.Name
            If bp.CheckProtecting(targetLimb.Name) = True Then Return bp.Name
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
        Dim critVit As Boolean = False
        If targetLimb.IsVital = True Then : critVit = True
        ElseIf targetLimb.IsCritical = True AndAlso TotalHealth <= 0 Then : critVit = True
        End If
        If critVit = True Then
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
        EndInit()
    End Sub

    Public Overrides Function ToString() As String
        Return "[" & HealthPercentage.ToString("000") & "%] " & Name
    End Function
    Public Function ConsoleReport() As String
        Dim total As String = ""
        total &= Name & vbCrLf
        total &= "└ Health:   " & HealthPercentage & "%" & vbCrLf
        total &= "└ Speed:    " & TotalSpeed & vbCrLf
        total &= "└ Position: " & _DistanceFromMiddle.ToString & vbCrLf

        total &= "└ Limbs:" & vbCrLf
        For Each bp In BodyParts
            total &= "  └ " & bp.ConsoleReport & vbCrLf
        Next
        Return total
    End Function
End Class

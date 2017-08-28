Public Class Spell
    Private Owner As Combatant
    Private AttackRange As AttackRange
    Private Damage As Damage
    Private Target As Combatant              'if target is nothing, it's AOE
    Private TargetLimb As BodyPart

    Public Sub Resolve(ByVal battlefield As Battlefield)
        If Target Is Nothing = False Then
            ResolveDamage(Target)
        Else
            Dim targets As List(Of Combatant) = battlefield.GetTargets(Owner, AttackRange)
            For Each t In targets
                ResolveDamage(t)
            Next
        End If
    End Sub
    Private Sub ResolveDamage(ByVal t As Combatant)
        If TargetLimb Is Nothing Then TargetLimb = GetRandom(Of BodyPart)(t.GetTargetableLimbs)
        t.ApplyDamage(Damage, TargetLimb)
    End Sub
End Class

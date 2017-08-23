Public Class Enemy
    Inherits Combatant

    Public Shared Function Load(ByVal enemyName As String) As Enemy
        Dim raw As Queue(Of String) = SquareBracketLoader("data/enemies.txt", enemyName)
        If raw Is Nothing Then Throw New Exception("Invalid Enemy Name") : Return Nothing

        Dim enemy As New Enemy
        With enemy
            .Name = raw.Dequeue
            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim limbName As String = ln(1).Trim

                Dim bp As BodyPart = BodyPart.Load(limbName, ln(0))
                bp.FinalMerge()
                .BodyParts.Add(bp)
            End While

            .BaseModifier = New Component
            .FullReady()
        End With
        Return enemy
    End Function

    Public Overrides ReadOnly Property Attacks As System.Collections.Generic.List(Of BodyPart)
        Get
            Dim total As New List(Of BodyPart)
            For Each bp In BodyParts
                If bp.IsReady = True Then total.Add(bp)
            Next
            Return total
        End Get
    End Property
    Public Function PerformAction() As String
        'determine target
        Dim target As Combatant
        Dim potentialTargets As List(Of Combatant) = Battlefield.GetTargets(Me)
        If potentialTargets.Count = 1 Then
            target = potentialTargets(0)
        Else
            '75% chance of targeting mech
            If Rng.Next(1, 101) <= 75 Then
                'target mech
                target = potentialTargets(0)
            Else
                'target companion
                Dim i As Integer = Rng.Next(1, potentialTargets.Count - 1)
                target = potentialTargets(i)
            End If
        End If

        'determine attack
        Dim attacks As List(Of BodyPart) = GetPotentialAttacks(target)
        Dim attack As BodyPart = GetRandom(Of BodyPart)(attacks)

        'determine targetLimb
        Dim targetLimbs As List(Of BodyPart) = target.GetTargetableLimbs()
        Dim targetLimb As BodyPart = GetRandom(Of BodyPart)(targetLimbs)

        'actually attack
        PerformAction = MyBase.PerformsAttack(attack, target, targetLimb)

        'end turn
        EndInit()
    End Function
End Class

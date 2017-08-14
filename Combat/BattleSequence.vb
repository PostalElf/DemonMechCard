Public Class BattleSequence
    Private Terrain As BattlefieldTerrain
    Private Difficulty As Integer
    Private Sequence As New Queue(Of Encounter)
    Private ActiveBattlefield As Battlefield

    Public Mech As Mech
    Public Allies As New List(Of Combatant)

    Public Shared Function Construct(ByVal _terrain As BattlefieldTerrain, ByVal difficulty As Integer, ByVal length As Integer) As BattleSequence
        Dim bs As New BattleSequence
        With bs
            .Terrain = _terrain
            .Difficulty = difficulty

            For n = 1 To length
                .Sequence.Enqueue(Encounter.Construct(bs, .Terrain, .Difficulty))
            Next
        End With
        Return bs
    End Function
End Class

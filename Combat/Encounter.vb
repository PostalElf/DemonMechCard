Public MustInherit Class Encounter
    Protected Terrain As BattlefieldTerrain
    Public MustOverride ReadOnly Property IsOver As Boolean

    Public Shared Function Construct(ByVal battleSequence As BattleSequence, ByVal terrain As BattlefieldTerrain, ByVal difficulty As Integer) As Encounter
        '50% battle, 10% challenge, 25% skill, 15% branch
        Select Case GetProbability({50, 10, 25, 15})
            Case 0 : Return Battlefield.Construct(battleSequence, terrain, difficulty)
            Case 1 : Return Battlefield.Construct(battleSequence, terrain, difficulty + 1)
                'Case 2 
                'Case 3
            Case Else : Throw New Exception("Invalid probability") : Return Nothing
        End Select
    End Function
End Class

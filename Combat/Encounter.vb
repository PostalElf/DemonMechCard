Public MustInherit Class Encounter
    Protected Terrain As BattlefieldTerrain
    Public MustOverride ReadOnly Property IsOver As Boolean

    Public Shared Function Construct(ByVal battleSequence As BattleSequence, ByVal terrain As BattlefieldTerrain, ByVal difficulty As Integer, Optional ByVal encounterType As String = "") As Encounter
        If encounterType = "" Then
            '70% battle, 10% challenge, 20% skill
            Select Case GetProbability({70, 10, 20})
                Case 0 : encounterType = "Battle"
                Case 1 : encounterType = "Challenge"
                Case 2 : encounterType = "Skill"
                Case Else : Throw New Exception("Invalid probability") : Return Nothing
            End Select
        End If

        Select Case encounterType
            Case "Battle" : Return Battlefield.Construct(battleSequence, terrain, difficulty, False)
            Case "Challenge" : Return Battlefield.Construct(battleSequence, terrain, difficulty + 1, False)
            Case "Boss" : Return Battlefield.Construct(battleSequence, terrain, difficulty, True)
                'Case "Skill"
                'Case "Branch"
            Case Else : Throw New Exception("Invalid encounterType") : Return Nothing
        End Select
    End Function
End Class

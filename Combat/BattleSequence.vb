Public Class BattleSequence
    Private Terrain As BattlefieldTerrain
    Private Difficulty As Integer
    Private LengthMax As Integer
    Private Length As Integer

    Public Mech As Mech
    Public Companions As New List(Of Combatant)

    Public Shared Function Construct(ByVal _terrain As BattlefieldTerrain, ByVal difficulty As Integer, ByVal length As Integer) As BattleSequence
        Dim bs As New BattleSequence
        With bs
            .Terrain = _terrain
            .Difficulty = difficulty
            .LengthMax = length
            .Length = .LengthMax
        End With
        Return bs
    End Function

    Public Sub AddCombatant(ByVal combatant As Combatant)
        If TypeOf combatant Is Companion Then
            Companions.Add(combatant)
        ElseIf TypeOf combatant Is Mech Then
            Mech = combatant
        End If
    End Sub
    Public Sub RemoveCombatant(ByVal combatant As Combatant)
        If TypeOf combatant Is Companion Then
            If Companions.Contains(combatant) = False Then Throw New Exception("Invalid combatant to remove.") : Exit Sub
            Companions.Remove(combatant)
        ElseIf TypeOf combatant Is Mech Then
            Mech = Nothing
        End If
    End Sub
    Public Function GetEncounter(Optional ByVal encounterType As String = "") As Encounter
        Length -= 1
        If encounterType = "" Then
            Select Case Length
                Case Is < 0 : Return Nothing
                Case 0 : encounterType = "Boss"
                Case LengthMax / 2 : encounterType = "Branch"
            End Select
        End If

        Return Encounter.Construct(Me, Terrain, Difficulty, encounterType)
    End Function
End Class

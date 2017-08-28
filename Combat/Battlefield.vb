Public Class Battlefield
    Inherits Encounter
    Private BattleSequence As BattleSequence
    Public ReadOnly Property Mech As Mech
        Get
            Return BattleSequence.Mech
        End Get
    End Property
    Public ReadOnly Property Companions As List(Of Combatant)
        Get
            Return BattleSequence.Companions
        End Get
    End Property
    Public Enemies As New List(Of Combatant)
    Private Function GetCombatants(ByVal location As AttackRange) As List(Of Combatant)
        Dim total As New List(Of Combatant)
        If location = Nothing OrElse Mech.DistanceFromMiddle = location Then total.Add(Mech)
        For Each c In Companions.Concat(Enemies)
            If location = Nothing OrElse c.DistanceFromMiddle = location Then total.Add(c)
        Next
        Return total
    End Function
    Public Sub AddCombatant(ByVal combatant As Combatant)
        combatant.Battlefield = Me
        Select Case combatant.GetType
            Case GetType(Mech), GetType(Companion) : BattleSequence.AddCombatant(combatant)
            Case GetType(Enemy) : Enemies.Add(combatant)
        End Select
    End Sub
    Public Sub RemoveCombatant(ByVal combatant As Combatant)
        combatant.Battlefield = Nothing
        Select Case combatant.GetType
            Case GetType(Mech), GetType(Companion) : BattleSequence.RemoveCombatant(combatant)
            Case GetType(Enemy) : Enemies.Remove(combatant)
        End Select
    End Sub
    Public Function GetTargets(ByVal attacker As Combatant) As List(Of Combatant)
        Dim total As New List(Of Combatant)
        If TypeOf attacker Is Mech OrElse TypeOf attacker Is Companion Then
            total.AddRange(Enemies)
        ElseIf TypeOf attacker Is Enemy Then
            total.Add(Mech)
            total.AddRange(Companions)
        Else
            Throw New Exception("No targets available.")
            Return Nothing
        End If

        Return total
    End Function
    Public Function GetAllies(ByVal target As Combatant) As List(Of Combatant)
        Dim total As New List(Of Combatant)
        If TypeOf target Is Mech OrElse TypeOf target Is Companion Then
            total.Add(Mech)
            total.AddRange(Companions)
            total.Remove(target)
        ElseIf TypeOf target Is Enemy Then
            total.AddRange(Enemies)
            total.Remove(target)
        Else
            Throw New Exception("No allies available.")
            Return Nothing
        End If
        Return total
    End Function
    Public Overrides ReadOnly Property IsOver As Boolean
        Get
            If Mech Is Nothing Then Return True
            If Enemies.Count = 0 Then Return True

            Return False
        End Get
    End Property

    Private InitBag As New List(Of Combatant)
    Public Function InitBagGrab() As Combatant
        'grabs next combatant
        'if multiple combatants end on the same tick, grab a random one from the bag

        While InitBag.Count = 0
            For Each c In GetCombatants(Nothing)
                If c.InitTick() = True Then InitBag.Add(c)
            Next
        End While

        Return GrabRandom(Of Combatant)(InitBag)
    End Function

    Private IsMagicResistant As Boolean
    Private HourActive As LitHour
    Private HourSpells As New Dictionary(Of LitHour, List(Of Spell))
    Public Sub LitHourAdvance()
        If IsMagicResistant = True Then HourActive += 1 Else HourActive += 2
        If HourActive > 8 Then HourActive = LitHour.Matins

        'check if it's an active hour
        If HourActive Mod 2 = 1 Then
            'active hour, resolve spells
            For Each spell In HourSpells(HourActive)
                spell.Resolve()
            Next
            HourSpells(HourActive).Clear()
        End If
    End Sub

    Public Shared Shadows Function Construct(ByVal _battleSequence As BattleSequence, ByVal _terrain As BattlefieldTerrain, ByVal difficulty As Integer, Optional ByVal isBoss As Boolean = False) As Battlefield
        Dim bf As New Battlefield
        With bf
            .BattleSequence = _battleSequence
            .Terrain = _terrain

            'get enemyList, which is a list of enemySets
            'eg. [1] Tentacled Horror, Tentacled Horror, 
            'bosses are tagged, eg. [1 Boss] Eater of Souls
            Dim enemyFileName As String = "data/enemies/" & .Terrain.ToString.ToLower & ".txt"
            Dim categoryName As String = difficulty
            If isBoss = True Then categoryName &= " Boss"
            Dim enemyList As List(Of Queue(Of String)) = SquareBracketCategorialLoader(enemyFileName, difficulty)

            'pick a random enemySet, then construct them as enemies
            Dim chosenEnemySet As Queue(Of String) = GetRandom(Of Queue(Of String))(enemyList)
            While chosenEnemySet.Count > 0
                Dim enemyName As String = chosenEnemySet.Dequeue
                .AddCombatant(Enemy.Load(enemyName))
            End While

            'add mech and companions
            .BattleSequence.Mech.Battlefield = bf
            For Each c In .BattleSequence.Companions
                c.Battlefield = bf
            Next
        End With
        Return bf
    End Function
    Public Sub New()
        For Each h In [Enum].GetValues(GetType(LitHour))
            If h Mod 2 = 1 Then
                HourSpells.Add(h, New List(Of Spell))
            End If
        Next
        HourActive = (Rng.Next(1, 5) * 2) - 1
    End Sub
    Public Function ConsoleReport()
        Dim total As String = ""
        For Each ar In [Enum].GetValues(GetType(AttackRange))
            total &= ar.ToString & ":" & vbCrLf
            Dim cs As List(Of Combatant) = GetCombatants(ar)
            For Each c In cs
                total &= " └ " & c.ToString & vbCrLf
            Next
        Next
        Return total
    End Function
    Public Overrides Function ToString() As String
        Return Mech.Name & " vs " & Enemies.Count
    End Function
End Class

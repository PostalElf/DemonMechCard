Public Class Battlefield
    Inherits Encounter
    Private BattleSequence As BattleSequence
    Private ReadOnly Property Mech As Mech
        Get
            Return BattleSequence.Mech
        End Get
    End Property
    Private ReadOnly Property Allies As List(Of Combatant)
        Get
            Return BattleSequence.Allies
        End Get
    End Property
    Private Enemies As New List(Of Combatant)
    Protected Overrides ReadOnly Property IsOver As Boolean
        Get
            If Mech Is Nothing Then Return True
            If Enemies.Count = 0 Then Return True

            Return False
        End Get
    End Property

    Private InitBag As New List(Of Combatant)
    Private Sub InitBagNew()
        For Each enemy In Enemies
            For n = 1 To enemy.SpeedTokens
                InitBag.Add(enemy)
            Next
        Next
        For n = 1 To Mech.SpeedTokens
            InitBag.Add(Mech)
        Next
        For Each ally In Allies
            For n = 1 To ally.SpeedTokens
                InitBag.Add(ally)
            Next
        Next
    End Sub
    Public Function InitBagGrab() As Combatant
        If InitBag.Count = 0 Then InitBagNew()

        Dim roll As Integer = Rng.Next(InitBag.Count)
        InitBagGrab = InitBag(roll)
        InitBag.RemoveAt(roll)
    End Function
    Public Function InitBagReport() As List(Of String)
        'build a dictionary that counts the number of combatants in the bag
        Dim count As New Dictionary(Of Combatant, Integer)
        For Each init In InitBag
            If count.ContainsKey(init) = False Then count.Add(init, 0)
            count(init) += 1
        Next

        'report
        Dim ac As New AutoIncrementer
        Dim total As New List(Of String)
        For Each init In count.Keys
            total.Add(init.Name & " x" & count(init))
        Next
        Return total
    End Function
    Public Sub AICombatantAct()
        'this is called from GUI whenever an AI combatant is drawn
        'TODO: AI takes actions here
    End Sub

    Public Shared Shadows Function Construct(ByVal _battleSequence As BattleSequence, ByVal _terrain As BattlefieldTerrain, ByVal difficulty As Integer, Optional ByVal isBoss As Boolean = False) As Battlefield
        Dim bf As New Battlefield
        With bf
            .BattleSequence = _battleSequence
            .Terrain = _terrain

            'get enemyList, which is a list of enemySets
            'eg. [1] Tentacled Horror, Tentacled Horror, 
            Dim enemyFileName As String = "data/enemies/" & .Terrain.ToString.ToLower & ".txt"
            Dim categoryName As String = difficulty
            If isBoss = True Then categoryName &= " Boss"
            Dim enemyList As List(Of Queue(Of String)) = SquareBracketCategorialLoader(enemyFileName, difficulty)

            'pick a random enemySet, then construct them as enemies
            Dim chosenEnemySet As Queue(Of String) = GetRandom(Of Queue(Of String))(enemyList)
            While chosenEnemySet.Count > 0
                Dim enemyName As String = chosenEnemySet.Dequeue
                .Enemies.Add(Enemy.Load(enemyName))
            End While
        End With
        Return bf
    End Function
End Class

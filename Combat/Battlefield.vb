Public Class Battlefield
    Private Terrain As BattlefieldTerrain
    Private Enemies As List(Of Enemy)
    Private Mech As Combatant
    Private Allies As List(Of Combatant)
    Private ReadOnly Property IsOver As Boolean
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


End Class

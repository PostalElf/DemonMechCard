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
    Private Function GetAttack() As BodyPart
        Return GetRandom(Of BodyPart)(Attacks)
    End Function
    Public Function PerformAction() As String
        Dim attack As BodyPart = GetAttack()
        Dim targets As List(Of Combatant) = GetTargets(attack)
    End Function
End Class

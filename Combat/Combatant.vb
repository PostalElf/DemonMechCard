Public MustInherit Class Combatant
    Public Name As String

    Protected BaseModifier As Component
    Protected BodyParts As New List(Of BodyPart)
    Private ReadOnly Property TotalHealth As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                If bp.IsCritical = True Then total += bp.Health
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property TotalHealthMax As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                If bp.IsCritical = True Then total += bp.HealthMax
            Next
            Return total
        End Get
    End Property
    Public ReadOnly Property HealthPercentage As Integer
        Get
            Dim total As Double = TotalHealth / TotalHealthMax * 100
            Return Math.Ceiling(total)
        End Get
    End Property
    Public ReadOnly Property TotalSpeed As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.Speed
            Next
            total += BaseModifier.Speed
            Return total
        End Get
    End Property

    Private DistanceFromMiddle As AttackRange
    Private Function GetDistance(ByVal target As Combatant) As AttackRange
        Dim total As Integer = DistanceFromMiddle
        total += target.DistanceFromMiddle

        Select Case total
            Case 0 : Return AttackRange.Close
            Case 1 : Return AttackRange.Average
            Case 2 : Return AttackRange.Far
            Case Else : Return AttackRange.Out
        End Select
    End Function
    Public MustOverride ReadOnly Property Attacks As List(Of BodyPart)
    Public Function IsAttacked(ByVal damage As Damage, ByVal targetLimbIndex As Integer) As String
        If targetLimbIndex < 0 OrElse targetLimbIndex > BodyParts.Count Then Return Nothing
        Dim targetLimb As BodyPart = BodyParts(targetLimbIndex)

        Return targetLimb.IsAttacked(damage)
    End Function

    Public Sub FullReady()
        For Each bp In BodyParts
            bp.FullReady()
        Next
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

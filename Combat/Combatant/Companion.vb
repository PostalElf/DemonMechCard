Public Class Companion
    Inherits Combatant
    Public Overrides ReadOnly Property Attacks As List(Of BodyPart)
        Get
            Dim total As New List(Of BodyPart)
            For Each bp In BodyParts
                If bp.IsReady = True Then total.Add(bp)
            Next
            Return total
        End Get
    End Property
End Class

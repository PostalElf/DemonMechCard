Public Class Component
    Protected Speed As Integer
    Protected Defences As New List(Of DamageType)

    Protected AttackRange As AttackRange
    Protected AmmoMax As Integer
    Protected Accuracy As Integer
    Protected Dodge As Integer
    Protected Damages As New Damages

    Public Sub Merge(ByVal c As Component)
        Speed += c.Speed
        For Each d In Defences
            If Defences.Contains(d) = False Then Defences.Add(d)
        Next

        If AttackRange < c.AttackRange Then AttackRange = c.AttackRange
        AmmoMax += c.AmmoMax
        Accuracy += c.Accuracy
        Dodge += c.Dodge
        Damages += c.Damages
    End Sub
End Class

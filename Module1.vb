Module Module1

    Sub Main()
        Dim d1 As New Damages
        d1.Add(New Damage(1, 4, DamageType.Frost))
        Dim d2 As New Damages
        d2.Add(New Damage(1, 3, DamageType.Fire))
        d1 += d2
    End Sub

End Module

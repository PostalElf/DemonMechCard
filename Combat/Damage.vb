Public Structure Damage
    Public Min As Integer
    Public Max As Integer
    Public DamageType As DamageType

    Public Sub New(ByVal _min As Integer, ByVal _spread As Integer, ByVal dt As DamageType)
        Min = _min
        Max = _min + _spread
        DamageType = dt
    End Sub
    Public Function Roll() As Integer
        Dim total As Integer = 0
        For n = 1 To 3
            total += Rng.Next(Min, Max + 1)
        Next
        Return Math.Round(total / 3)
    End Function
    Public Overrides Function ToString() As String
        Return Min & "-" & Max & " " & DamageType.ToString
    End Function
End Structure

Public Structure Damage
    Public Min As Integer
    Public Max As Integer
    Public Accuracy As Integer
    Public DamageType As DamageType

    Public Sub New(ByVal _min As Integer, ByVal _spread As Integer, ByVal _accuracy As Integer, ByVal dt As DamageType)
        Min = _min
        Max = _min + _spread
        Accuracy = _accuracy
        DamageType = dt
    End Sub
    Public Function Roll() As Integer
        Dim total As Integer = 0
        For n = 1 To 3
            total += Rng.Next(Min, Max + 1)
        Next
        Return Math.Round(total / 3)
    End Function
    Public Shared Function Shortener(ByVal dt As DamageType) As String
        Select Case dt
            Case DamageType.Kinetic : Return "K"
            Case DamageType.Fire : Return "F"
            Case DamageType.Frost : Return "R"
            Case DamageType.Lightning : Return "L"
            Case DamageType.Alchemical : Return "A"
            Case DamageType.Sorcerous : Return "S"
            Case Else : Throw New Exception("Unrecognised damagetype")
        End Select
    End Function
    Public Overrides Function ToString() As String
        Return Min & "-" & Max & " " & DamageType.ToString
    End Function
End Structure

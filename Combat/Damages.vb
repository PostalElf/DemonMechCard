Public Class Damages
    Inherits Dictionary(Of DamageType, Damage)
    Public Overloads Sub Add(ByVal Damage As Damage)
        If Me.ContainsKey(Damage.Type) = False Then Me.Add(Damage.Type, Nothing)
        Me(Damage.Type) += Damage
    End Sub
    Public Shared Operator +(ByVal d1 As Damages, ByVal d2 As Damages) As Damages
        For Each d In d2.Keys
            If d1.ContainsKey(d) = False Then d1.Add(d, Nothing)
            d1(d) += d2(d)
        Next
        Return d1
    End Operator
    Public Shadows Function Count() As Integer
        Return Me.Keys.Count
    End Function
End Class

Public Structure Damage
    Public Min As Integer
    Public Max As Integer
    Public Type As DamageType

    Public Sub New(ByVal _min As Integer, ByVal _max As Integer, ByVal _type As DamageType)
        Min = _min
        Max = _max
        Type = _type
    End Sub
    Public Overrides Function ToString() As String
        Return Min & " - " & Max & " " & Type.ToString
    End Function
    Public Shared Operator +(ByVal d1 As Damage, ByVal d2 As Damage) As Damage
        Dim dmg As New Damage
        With dmg
            .Min = d1.Min + d2.Min
            .Max = d1.Max + d2.Max
            If d1.Type = d2.Type Then
                .Type = d1.Type
            ElseIf d1.Type > d2.Type Then
                .Type = d1.Type
            ElseIf d2.Type > d1.Type Then
                .Type = d2.Type
            End If
        End With
        Return dmg
    End Operator
End Structure

Public Enum DamageType
    Kinetic = 1
    Fire
    Frost
    Lightning
    Alchemical
    Sorcerous
End Enum

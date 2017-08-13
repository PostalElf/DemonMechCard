Public Class BodyPart
    Inherits Component
    Public Health As Integer
    Public Ammo As Integer
    Public ReadOnly Property IsReady As Boolean
        Get
            If Ammo > 0 AndAlso Health > 0 Then Return True Else Return False
        End Get
    End Property

    Public Overloads Sub FinalMerge(ByVal finalDamageType As DamageType)
        MyBase.FinalMerge(finalDamageType)
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Sub FullReady()
        Health = _HealthMax
        Ammo = AmmoMax
    End Sub
End Class

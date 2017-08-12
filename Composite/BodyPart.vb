Public Class BodyPart
    Inherits Component
    Public Name As String
    Public Health As Integer
    Public Ammo As Integer
    Public Charge As Integer
    Public ReadOnly Property IsReady As Boolean
        Get
            If Ammo > 0 AndAlso Health > 0 Then Return True Else Return False
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Overloads Sub FinalMerge()
        MyBase.FinalMerge()
    End Sub
    Public Sub CombatStart()
        Health = _HealthMax
        Ammo = AmmoMax
    End Sub
End Class

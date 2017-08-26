Public Class BunkerRoom
    Private _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property
    Private _SpaceUsed As Integer
    Public ReadOnly Property SpaceUsed As Integer
        Get
            Return _SpaceUsed
        End Get
    End Property
    Public BunkerEffects As New List(Of BunkerEffect)
End Class

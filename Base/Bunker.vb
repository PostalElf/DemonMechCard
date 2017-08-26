Public Class Bunker
    Private Name As String
    Private _Money As Integer
    Public ReadOnly Property Money As Integer
        Get
            Return _Money
        End Get
    End Property

    Private LandExcavated As Integer
    Private LandUnexcavated As Integer
    Private ReadOnly Property SpaceFree As Integer
        Get
            Return LandExcavated - SpaceUsed
        End Get
    End Property
    Private ReadOnly Property SpaceUsed As Integer
        Get
            Dim total As Integer = 0
            For Each r In Rooms
                total += r.SpaceUsed
            Next
            Return total
        End Get
    End Property
    Private Rooms As New List(Of BunkerRoom)
End Class

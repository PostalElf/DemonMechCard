Public Class Component
    Protected Speed As Integer
    Protected Defences As New List(Of DamageType)

    Protected AttackRange As AttackRange
    Protected AmmoMax As Integer
    Protected Accuracy As Integer
    Protected Dodge As Integer
    Protected Damages As New Damages

    Public Shared Function Load(ByVal componentName As String) As Component
        Dim raw As Queue(Of String) = SquareBracketLoader("data/components.txt", componentName)
        If raw Is Nothing Then Throw New Exception("Invalid ComponentName") : Return Nothing

        Dim component As New Component
        With component
            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key As String = ln(0).Trim
                Dim value As String = ln(1).Trim
                .Build(key, value)
            End While
        End With
        Return component
    End Function
    Private Sub Build(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Speed" : Speed = CInt(value)
            Case "Defences"
            Case "AttackRange"
            Case "AmmoMax"
            Case "Accuracy"
            Case "Dodge"

        End Select
    End Sub
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

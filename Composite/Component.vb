Public Class Component
    Public Slot As String
    Protected Speed As Integer
    Protected Defences As New List(Of DamageType)
    Protected AttackRanges As New List(Of AttackRange)
    Protected AttackRangesRemove As New List(Of AttackRange)
    Protected AmmoMax As Integer
    Protected Accuracy As Integer
    Protected Dodge As Integer
    Protected Damages As New Damages

    Public Shared Function Load(ByVal componentName As String) As Component
        Dim raw As Queue(Of String) = SquareBracketLoader("data/components.txt", componentName)
        If raw Is Nothing Then Throw New Exception("Invalid ComponentName") : Return Nothing

        Dim component As New Component
        With component
            .Slot = raw.Dequeue

            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key As String = ln(0).Trim
                Dim value As String = ln(1).Trim
                .Build(key, value)
            End While
        End With
        Return component
    End Function
    Public Sub Build(ByVal key As String, ByVal value As String)
        Select Case key.ToLower
            Case "Speed".ToLower : Speed = CInt(value)
            Case "Defences".ToLower
                Dim total As List(Of String) = UnformatCommaList(value)
                For Each v In total
                    Dim entry As DamageType = String2Enum(Of DamageType)(v)
                    If entry <> Nothing Then Defences.Add(entry)
                Next
            Case "AttackRange".ToLower
                Dim range As AttackRange = String2Enum(Of AttackRange)(value)
                If AttackRanges.Contains(range) = False Then AttackRanges.Add(range)
            Case "AttackRangeRemove".ToLower
                Dim range As AttackRange = String2Enum(Of AttackRange)(value)
                If AttackRangesRemove.Contains(range) = False Then AttackRangesRemove.Add(range)
            Case "AmmoMax".ToLower : AmmoMax = CInt(value)
            Case "Accuracy".ToLower : Accuracy = CInt(value)
            Case "Dodge".ToLower : Dodge = CInt(value)
            Case "Damage".ToLower
                Dim total As List(Of String) = UnformatCommaList(value)
                For Each v In total
                    Dim entry As Damage? = Damage.Construct(v)
                    If entry Is Nothing = False Then Damages.Add(entry)
                Next
        End Select
    End Sub
    Public Sub Merge(ByVal c As Component)
        Speed += c.Speed
        For Each d In c.Defences
            If Defences.Contains(d) = False Then Defences.Add(d)
        Next
        For Each ar In c.AttackRanges
            If AttackRanges.Contains(ar) = False Then AttackRanges.Add(ar)
        Next
        For Each ar In c.AttackRangesRemove
            If AttackRangesRemove.Contains(ar) = False Then AttackRangesRemove.Add(ar)
        Next
        AmmoMax += c.AmmoMax
        Accuracy += c.Accuracy
        Dodge += c.Dodge
        Damages += c.Damages
    End Sub
    Public Sub FinalMerge()
        For Each ar In AttackRangesRemove
            If AttackRanges.Contains(ar) Then AttackRanges.Remove(ar)
        Next
    End Sub
End Class

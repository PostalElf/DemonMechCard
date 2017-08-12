Public Class Component
    Public Slot As String
    Protected Speed As Integer
    Protected _HealthMax As Integer
    Public ReadOnly Property HealthMax As Integer
        Get
            Return _HealthMax
        End Get
    End Property
    Protected Dodge As Integer
    Protected Defences As New List(Of DamageType)

    Protected WeaponType As WeaponType
    Protected _HandCost As Integer
    Public ReadOnly Property HandCost As Integer
        Get
            Return _HandCost
        End Get
    End Property
    Protected IsQuick As Boolean = False
    Protected AttackRanges As New List(Of AttackRange)
    Protected AttackRangesRemove As New List(Of AttackRange)
    Protected AmmoMax As Integer
    Protected Accuracy As Integer
    Protected Damages As New Damages

    Protected _ExtraHands As Integer
    Public ReadOnly Property ExtraHands As Integer
        Get
            Return _ExtraHands
        End Get
    End Property


    Public Shared Function Load(ByVal componentName As String) As Component
        Dim raw As Queue(Of String) = SquareBracketLoader("data/components.txt", componentName)
        If raw Is Nothing Then Throw New Exception("Invalid ComponentName") : Return Nothing

        Dim component As New Component
        With component
            .Slot = raw.Dequeue

            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key, value As String
                If ln.Count = 2 Then
                    key = ln(0).Trim
                    value = ln(1).Trim
                Else
                    key = ln(0).Trim
                    value = ""
                End If
                .Build(key, value)
            End While
        End With
        Return component
    End Function
    Public Sub Build(ByVal key As String, ByVal value As String)
        Select Case key.ToLower
            Case "Speed".ToLower : Speed = CInt(value)
            Case "HealthMax".ToLower : _HealthMax = CInt(value)
            Case "Dodge".ToLower : Dodge = CInt(value)
            Case "Defences".ToLower
                Dim total As List(Of String) = UnformatCommaList(value)
                For Each v In total
                    Dim entry As DamageType = String2Enum(Of DamageType)(v)
                    If entry <> Nothing Then Defences.Add(entry)
                Next

            Case "WeaponType".ToLower : WeaponType = String2Enum(Of WeaponType)(value)
            Case "HandCost".ToLower : _HandCost = CInt(value)
            Case "IsQuick".ToLower : IsQuick = True
            Case "AttackRange".ToLower
                Dim ranges As List(Of String) = UnformatCommaList(value)
                For Each range In ranges
                    Dim e As AttackRange = String2Enum(Of AttackRange)(range)
                    If AttackRanges.Contains(e) = False Then AttackRanges.Add(e)
                Next
            Case "AttackRangeRemove".ToLower
                Dim ranges As List(Of String) = UnformatCommaList(value)
                For Each range In ranges
                    Dim e As AttackRange = String2Enum(Of AttackRange)(range)
                    If AttackRangesRemove.Contains(e) = False Then AttackRangesRemove.Add(e)
                Next
            Case "AmmoMax".ToLower : AmmoMax = CInt(value)
            Case "Accuracy".ToLower : Accuracy = CInt(value)
            Case "Damage".ToLower
                Dim total As List(Of String) = UnformatCommaList(value)
                For Each v In total
                    Dim entry As Damage? = Damage.Construct(v)
                    If entry Is Nothing = False Then Damages.Add(entry)
                Next

            Case "ExtraHands".ToLower : _ExtraHands += CInt(value)
        End Select
    End Sub
    Public Sub Merge(ByVal c As Component)
        Speed += c.Speed
        _HealthMax += c._HealthMax
        Dodge += c.Dodge
        For Each d In c.Defences
            If Defences.Contains(d) = False Then Defences.Add(d)
        Next

        If WeaponType = Nothing AndAlso c.WeaponType <> Nothing Then WeaponType = c.WeaponType
        _HandCost += c._HandCost
        For Each ar In c.AttackRanges
            If AttackRanges.Contains(ar) = False Then AttackRanges.Add(ar)
        Next
        For Each ar In c.AttackRangesRemove
            If AttackRangesRemove.Contains(ar) = False Then AttackRangesRemove.Add(ar)
        Next
        AmmoMax += c.AmmoMax
        Accuracy += c.Accuracy
        Damages += c.Damages

        _ExtraHands += c._ExtraHands
    End Sub
    Public Sub FinalMerge()
        If WeaponType = Nothing Then WeaponType = WeaponType.Conventional
        For Each ar In AttackRangesRemove
            If AttackRanges.Contains(ar) Then AttackRanges.Remove(ar)
        Next
    End Sub
End Class

Public Class Component
    Public Slot As String
    Public Name As String
    Protected _HealthMax As Integer
    Public ReadOnly Property HealthMax As Integer
        Get
            Return _HealthMax
        End Get
    End Property
    Protected Dodge As Integer
    Protected Defences As New List(Of DamageType)
    Protected Protects As New List(Of String)

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
    Protected DamageMin As Integer
    Protected DamageSpread As Integer
    Protected DamageTypes As New List(Of DamageType)

    Protected _Speed As Integer
    Public ReadOnly Property Speed As Integer
        Get
            Return _Speed
        End Get
    End Property
    Protected _ExtraHands As Integer
    Public ReadOnly Property ExtraHands As Integer
        Get
            Return _ExtraHands
        End Get
    End Property
    Protected _InventorySpace As Integer
    Public ReadOnly Property InventorySpace As Integer
        Get
            Return _InventorySpace
        End Get
    End Property
    Protected _Cost As Integer
    Public ReadOnly Property Cost As Integer
        Get
            Return _Cost
        End Get
    End Property

    Public Shared Function Load(ByVal componentName As String) As Component
        Dim raw As Queue(Of String) = SquareBracketLoader("data/components.txt", componentName)
        If raw Is Nothing Then Throw New Exception("Invalid ComponentName") : Return Nothing

        Dim component As New Component
        With component
            .Name = raw.Dequeue

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
        Select Case key
            Case "Slot" : Slot = value
            Case "HealthMax", "Health" : _HealthMax = CInt(value)
            Case "Dodge" : Dodge = CInt(value)
            Case "Defences"
                Dim total As List(Of String) = UnformatCommaList(value)
                For Each v In total
                    Dim entry As DamageType = String2Enum(Of DamageType)(v)
                    If entry <> Nothing Then Defences.Add(entry)
                Next
            Case "Protects"
                Dim total As List(Of String) = UnformatCommaList(value)
                For Each v In total
                    If Protects.Contains(v) = False Then Protects.Add(v)
                Next

            Case "WeaponType" : WeaponType = String2Enum(Of WeaponType)(value)
            Case "HandCost" : _HandCost = CInt(value)
            Case "IsQuick" : IsQuick = True
            Case "AttackRange", "AttackRanges"
                Dim ranges As List(Of String) = UnformatCommaList(value)
                For Each range In ranges
                    Dim e As AttackRange = String2Enum(Of AttackRange)(range)
                    If AttackRanges.Contains(e) = False Then AttackRanges.Add(e)
                Next
            Case "AttackRangeRemove", "AttackRangesRemove"
                Dim ranges As List(Of String) = UnformatCommaList(value)
                For Each range In ranges
                    Dim e As AttackRange = String2Enum(Of AttackRange)(range)
                    If AttackRangesRemove.Contains(e) = False Then AttackRangesRemove.Add(e)
                Next
            Case "AmmoMax" : AmmoMax = CInt(value)
            Case "Accuracy" : Accuracy = CInt(value)
            Case "DamageMin" : DamageMin = CInt(value)
            Case "DamageSpread" : DamageSpread = CInt(value)
            Case "DamageType" : Dim dt As DamageType = String2Enum(Of DamageType)(value) : If DamageTypes.Contains(dt) = False Then DamageTypes.Add(dt)
            Case "Damage"
                Dim ln As String() = value.Split(" ")
                DamageMin = CInt(ln(0))
                DamageSpread = CInt(ln(2) - DamageMin)
                Build("DamageType", ln(3))

            Case "Speed" : _Speed = CInt(value)
            Case "ExtraHands" : _ExtraHands += CInt(value)
            Case "InventorySpace" : _InventorySpace += CInt(value)
            Case "Cost" : _Cost += CInt(value)
        End Select
    End Sub
    Public Sub Merge(ByVal c As Component)
        _HealthMax += c._HealthMax
        Dodge += c.Dodge
        For Each d In c.Defences
            If Defences.Contains(d) = False Then Defences.Add(d)
        Next
        For Each p In c.Protects
            If Protects.Contains(p) = False Then Protects.Add(p)
        Next

        If WeaponType = Nothing AndAlso c.WeaponType <> Nothing Then WeaponType = c.WeaponType
        _HandCost += c._HandCost
        If c.IsQuick = True Then IsQuick = True
        For Each ar In c.AttackRanges
            If AttackRanges.Contains(ar) = False Then AttackRanges.Add(ar)
        Next
        For Each ar In c.AttackRangesRemove
            If AttackRangesRemove.Contains(ar) = False Then AttackRangesRemove.Add(ar)
        Next
        AmmoMax += c.AmmoMax
        Accuracy += c.Accuracy
        DamageMin += c.DamageMin
        DamageSpread += c.DamageSpread
        For Each dt In c.DamageTypes
            If DamageTypes.Contains(dt) = False Then DamageTypes.Add(dt)
        Next

        _Speed += c._Speed
        _ExtraHands += c._ExtraHands
        _InventorySpace += c._InventorySpace
        _Cost += c._Cost
    End Sub
    Public Overrides Function ToString() As String
        Return Slot
    End Function
End Class

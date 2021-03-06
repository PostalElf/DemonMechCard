﻿Public Class BodyPart
    Inherits Component
    Public Owner As Combatant
    Public Health As Integer
    Private ReadOnly Property HealthPercentage As Integer
        Get
            Return Math.Max(Math.Ceiling(Health / HealthMax * 100), 0)
        End Get
    End Property
    Public Ammo As Integer
    Public Damage As Damage

    Private _IsCritical As Boolean
    Public ReadOnly Property IsCritical As Boolean
        Get
            Return _IsCritical
        End Get
    End Property
    Private _IsVital As Boolean
    Public ReadOnly Property IsVital As Boolean
        Get
            Return _IsVital
        End Get
    End Property
    Public Shadows ReadOnly Property IsQuick As Boolean
        Get
            Return MyBase.IsQuick
        End Get
    End Property
    Public IsInvulnerable As Boolean
    Public ReadOnly Property IsWeapon As Boolean
        Get
            If WeaponType = Nothing Then Return False Else Return True
        End Get
    End Property
    Public ReadOnly Property IsReady As Boolean
        Get
            If IsWeapon = False Then Return False
            If Health <= 0 Then Return False
            If Ammo <= 0 AndAlso AmmoMax <> -1 Then Return False
            Return True
        End Get
    End Property
    Public Function CheckAttackRange(ByVal attackRange As AttackRange) As Boolean
        Return AttackRanges.Contains(attackRange)
    End Function
    Public Function CheckDefences(ByVal damageType As DamageType) As Boolean
        Return Defences.Contains(damageType)
    End Function
    Public Function CheckCritDodge(ByVal accuracy As Integer) As String
        Dim roll As Integer = Rng.Next(1, 101)
        If accuracy > Dodge Then
            'crit
            If roll < accuracy - Dodge Then Return "CRIT"
        ElseIf Dodge > accuracy Then
            'dodge
            If roll < Dodge - accuracy Then Return "DDG"
        End If
        Return Nothing
    End Function
    Public Function CheckProtecting(ByVal name As String) As Boolean
        Return Protects.Contains(name)
    End Function

    Public Overloads Shared Function Load(ByVal enemyLimbName As String, ByVal critVital As String) As BodyPart
        'builds bodypart from a name in enemylimbs.txt
        Dim raw As Queue(Of String) = SquareBracketLoader("data/enemies/enemylimbs.txt", enemyLimbName)

        Dim bp As New BodyPart
        With bp
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

            Select Case critVital
                Case "Critical" : ._IsCritical = True : ._IsVital = True
                Case "Vital" : ._IsVital = True
            End Select
        End With
        Return bp
    End Function
    Public Overloads Sub FinalMerge(Optional ByVal finalDamageType As DamageType = Nothing)
        If IsWeapon = True Then
            AttackRangesRemove.Add(AttackRange.Out)             'always remove Out of Reach range for obvious reasons
            For Each ar In AttackRangesRemove
                If AttackRanges.Contains(ar) Then AttackRanges.Remove(ar)
            Next

            If finalDamageType = Nothing Then
                'no FinalDamageType specified; attempt to use i=0 if count=1, or roll random if count>1
                Select Case DamageTypes.Count
                    Case 0 : finalDamageType = DamageType.Kinetic
                    Case 1 : finalDamageType = DamageTypes(0)
                    Case Is > 1 : finalDamageType = GetRandom(Of DamageType)(DamageTypes)
                End Select
            Else
                'finalDamageType specified, check if it is contained within damageTypes
                'if not, just set to kinetic as default
                If DamageTypes.Contains(finalDamageType) = False Then finalDamageType = DamageType.Kinetic
            End If

            Damage = New Damage(DamageMin, DamageSpread, Accuracy, finalDamageType)
        End If

        If HealthMax <= 0 Then _HealthMax = 1
        Select Case Slot
            Case "Chassis", "Phylactery" : _IsVital = True
        End Select
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function
    Public Function ConsoleReport() As String
        Dim total As String = ""
        total &= "[" & HealthPercentage.ToString("000") & "%"
        If IsCritical = True OrElse IsVital = True Then
            total &= "-"
            If IsCritical = True Then total &= "c"
            If IsVital = True Then total &= "v"
        End If
        total &= "] "
        total &= Name & ": "

        total &= "»" & Dodge & "%"
        If Defences.Count > 0 Then
            total &= ","
            For Each d In Defences
                total &= Damage.Shortener(d)
            Next
        End If
        total &= "» "

        If IsWeapon = True Then
            total &= Damage.ToString
        End If

        Return total
    End Function

    Public Sub FullReady()
        Health = _HealthMax
        Ammo = AmmoMax
    End Sub
End Class

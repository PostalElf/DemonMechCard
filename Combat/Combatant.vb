﻿Public MustInherit Class Combatant
    Public Name As String

    Protected BaseModifier As Component
    Protected BodyParts As New List(Of BodyPart)
    Private ReadOnly Property TotalHealth As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.Health
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property TotalHealthMax As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.healthmax
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property HealthPercentage As Integer
        Get
            Dim total As Double = TotalHealth / TotalHealthMax * 100
            Return Math.Ceiling(total)
        End Get
    End Property

    Public ReadOnly Property TotalSpeed As Integer
        Get
            Dim total As Integer = 0
            For Each bp In BodyParts
                total += bp.speed
            Next
            total += BaseModifier.speed
            Return total
        End Get
    End Property
End Class

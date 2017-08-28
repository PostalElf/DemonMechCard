Public Class Bunker
    Public Sub New()
        For Each rs In [Enum].GetValues(GetType(ResearchSphere))
            ResearchCrew.Add(rs, New List(Of Crew))
            ResearchSubranks.Add(rs, New Dictionary(Of String, Integer))
            For Each ss In GetResearchSubSpheres(rs)
                ResearchSubranks(rs).Add(ss, 0)
            Next
            ResearchFocus.Add(rs, "")
            ResearchFunding.Add(rs, 0)
            ResearchProgress.Add(rs, 0)
            ResearchThreshold.Add(rs, 0)
        Next
    End Sub
    Private Sub Destroy()
        'TODO
    End Sub
    Public Function Tick() As List(Of String)
        Dim total As New List(Of String)
        total.AddRange(IncomeTick)
        total.AddRange(ResearchTick)
        Return total
    End Function

    Private Name As String
    Private _Money As Integer
    Public ReadOnly Property Money As Integer
        Get
            Return _Money
        End Get
    End Property
    Public ReadOnly Property Income As Integer
        Get
            Dim total As Integer = 0
            For Each a In Assets
                total += a.Income
            Next
            For Each rf In ResearchFunding.Values
                total += rf
            Next
            Return total
        End Get
    End Property
    Private Assets As New List(Of Asset)
    Private Function IncomeTick() As List(Of String)
        Dim report As New List(Of String)

        'increase and report
        _Money += Income
        report.Add("Income: " & Income.ToString("$0.00"))
        If _Money <= 5000 Then report.Add("Warning! Your reserves are low.")

        If _Money <= 0 Then Destroy()
        Return report
    End Function

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
    Private Function GetBunkerEffects() As List(Of BunkerEffect)
        Dim total As New List(Of BunkerEffect)
        For Each r In Rooms
            total.AddRange(r.BunkerEffects)
        Next
        Return total
    End Function

    Private Function GetResearchSphereRank(ByVal targetSphere As ResearchSphere) As Integer
        For Each rs In ResearchSubranks.Keys
            If rs = targetSphere Then
                Dim target As Dictionary(Of String, Integer) = ResearchSubranks(rs)
                Dim lowest As Integer = Int32.MaxValue
                For Each v In target.Values
                    If v < lowest Then lowest = v
                Next
                Return lowest
            End If
        Next
        Return -1
    End Function
    Private Function GetResearchSphere(ByVal subsphere As String) As ResearchSphere
        For Each rs In [Enum].GetValues(GetType(ResearchSphere))
            If GetResearchSubSpheres(rs).Contains(subsphere) Then Return rs
        Next
        Throw New Exception("Invalid subsphere.")
        Return Nothing
    End Function
    Private Function GetResearchSubSpheres(ByVal rs As ResearchSphere) As List(Of String)
        Select Case rs
            Case ResearchSphere.Iron : Return New List(Of String) From {"Alchemy", "Warfare", "Crusading"}
            Case ResearchSphere.Brass : Return New List(Of String) From {"Sigilcraft", "Samum", "Divination"}
            Case ResearchSphere.Silver : Return New List(Of String) From {"Thaumaturgy", "Baal Shem", "Hermeticism"}
            Case Else : Throw New Exception("ResearchSphere out of range")
        End Select
    End Function
    Private ResearchCrew As New Dictionary(Of ResearchSphere, List(Of Crew))
    Private ReadOnly Property ResearchEfficiency(ByVal rs As ResearchSphere) As Double
        Get
            Dim total As Double = 0
            For Each c In ResearchCrew(rs)
                total += c.Efficiency
            Next
            Return total
        End Get
    End Property
    Private ResearchSubranks As New Dictionary(Of ResearchSphere, Dictionary(Of String, Integer))
    Private ResearchFocus As New Dictionary(Of ResearchSphere, String)
    Public ResearchFunding As New Dictionary(Of ResearchSphere, Integer)
    Private ResearchProgress As New Dictionary(Of ResearchSphere, Integer)
    Private ResearchThreshold As New Dictionary(Of ResearchSphere, Integer)
    Private ResearchOverflow As Integer
    Private ResearchOverflowThreshold As Integer
    Private Function ResearchTick() As List(Of String)
        Dim report As New List(Of String)
        For Each rs In [Enum].GetValues(GetType(ResearchSphere))
            Dim progress As Integer = ResearchFunding(rs) * ResearchEfficiency(rs)
            ResearchProgress(rs) += progress
            If ResearchProgress(rs) >= ResearchThreshold(rs) Then
                'increase rank and report
                Dim focus As String = ResearchFocus(rs)
                ResearchSubranks(rs)(focus) += 1
                report.Add("Research completed: " & focus & " (" & rs & ") is now rank " & ResearchSubranks(rs)(focus))

                'get overflow
                ResearchOverflow += Math.Abs(ResearchProgress(rs) - ResearchThreshold(rs))
                If ResearchOverflow >= ResearchOverflowThreshold Then
                    ResearchOverflow = Math.Abs(ResearchOverflow - ResearchOverflowThreshold)
                    report.Add(ResearchOverflowTick())
                End If

                'reset progress, threshold, and focus
                ResearchProgress(rs) = 0
                ResearchThreshold(rs) = GetResearchSphereRank(rs) * 1000
                ResearchFocus(rs) = GetResearchFocus(rs)
            End If
        Next
        Return report
    End Function
    Private Function GetResearchFocus(ByVal rs As ResearchSphere) As String
        'get random lowest ranked subsphere
        Dim subspheres As List(Of String) = GetResearchSubSpheres(rs)
        Dim lowestSS As New List(Of String)
        Dim lowestSSValue As Integer = Int32.MaxValue
        Dim resetted As Boolean = True
        While resetted = True
            resetted = False
            lowestSS.Clear()
            For Each ss In subspheres
                Dim rank As Integer = ResearchSubranks(rs)(ss)
                If rank = lowestSSValue Then
                    lowestSS.Add(ss)
                ElseIf rank < lowestSSValue Then
                    lowestSS.Clear()
                    lowestSS.Add(ss)
                    lowestSSValue = rank
                    resetted = True
                End If
            Next
        End While

        Return GetRandom(Of String)(lowestSS)
    End Function
    Private Function ResearchOverflowTick() As String
        'TODO
    End Function
End Class

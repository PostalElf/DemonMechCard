Public Class Sefirot
    Private Names As String() = {"Malchut", "Yesod", "Hod", "Netzach", "Tiferet", "Gevurah", "Chesed", "Daat", "Binah", "Chochmah", "Keter"}
    Private BrokenSeals(10) As Boolean
    Private LastBroken As Integer = -1

    Public Sub New()
        ResetSeals()
    End Sub
    Public Sub ResetSeals()
        For n = 0 To 10
            BrokenSeals(n) = False
        Next
    End Sub
    Public Function CheckSealBreakage(ByVal i As Integer) As Boolean
        Return BrokenSeals(i)
    End Function
    Public Function BreakSeal(Optional ByVal i As Integer = -1) As String
        If i = -1 Then
            'break random adjacent seal
            Dim adjacents As Integer() = GetAdjacentIndexes(LastBroken)
            If adjacents Is Nothing Then Return "All broken"
            Dim roll As Integer = Rng.Next(adjacents.Length)
            i = adjacents(roll)
        End If

        'check for adjacency
        If LastBroken <> -1 Then
            Dim adjacents As Integer() = GetAdjacentIndexes(LastBroken)
            If adjacents Is Nothing Then Return "All broken"
            If adjacents.Contains(i) = False Then Return "Not adjacent"
        End If

        'break and note break
        BrokenSeals(i) = True
        LastBroken = i
        Return ""
    End Function
    Public Function CheckLastBreak() As String
        If LastBroken = -1 Then Return "" Else Return Names(LastBroken)
    End Function

    Private Function GetAdjacentIndexes(ByVal i As Integer) As Integer()
        Select Case i
            Case -1 : Return {0}
            Case 0 : Return {1}
            Case 1 : Return {2, 3, 4}
            Case 2 : Return {4, 5}
            Case 3 : Return {4, 6}
            Case 4 : Return {5, 6, 7}
            Case 5 : Return {7, 8}
            Case 6 : Return {7, 9}
            Case 7 : Return {8, 9, 10}
            Case 8 : Return {10}
            Case 9 : Return {10}
            Case 10 : Return Nothing
            Case Else : Return Nothing
        End Select
    End Function
End Class

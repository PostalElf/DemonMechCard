Public Class MechDesign
    Inherits Blueprint
    Public Overloads Shared Function Load(ByVal blueprintName As String) As MechDesign
        Dim raw As Queue(Of String) = SquareBracketLoader("data/mechdesigns.txt", blueprintName)
        If raw Is Nothing Then Throw New Exception("Invalid MechDesignName") : Return Nothing

        Dim mechdesign As New MechDesign
        With mechdesign
            .BlueprintName = raw.Dequeue
            While raw.Count > 0
                Dim ln As String() = raw.Dequeue.Split(":")
                Dim key As String = ln(0).Trim
                Dim value As String = ln(1).Trim
                .Build(key, value)
            End While
        End With
        Return mechdesign
    End Function
    Private Overloads Sub Build(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Slot" : ComponentTypesEmpty.Add(value)
            Case Else : BlueprintModifier.Build(key, value)
        End Select
    End Sub
    Public Overloads Function Construct() As mech
        If ComponentTypesEmpty.Count > 0 Then Return Nothing

        Dim bodyparts As New List(Of BodyPart)
        For Each c As Component In Components
            If TypeOf c Is BodyPart = False Then Throw New Exception("Non-Bodypart found in mechdesign component list.") : Return Nothing
            bodyparts.Add(CType(c, BodyPart))
        Next
        Return Mech.Build(bodyparts, BlueprintModifier)
    End Function
End Class

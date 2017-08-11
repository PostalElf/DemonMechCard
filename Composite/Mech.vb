Public Class Mech
    Private BodyParts As New List(Of BodyPart)
    Private BlueprintModifier As Component

    Public Shared Function Build(ByVal _bodyparts As List(Of BodyPart), ByVal _blueprintModifier As Component)
        Dim mech As New Mech
        With mech
            .BodyParts.AddRange(_bodyparts)
            .BlueprintModifier = _blueprintModifier
        End With
        Return mech
    End Function
End Class

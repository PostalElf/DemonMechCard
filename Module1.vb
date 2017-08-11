Module Module1

    Sub Main()
        Dim test1 As Component = Component.Load("Test 01")
        Dim test2 As Component = Component.Load("Test 02")
        Dim blueprint As Blueprint = blueprint.Load("Test Blueprint 01")
        blueprint.AddComponent(test1)
        blueprint.AddComponent(test2)
        Dim bodypart As BodyPart = blueprint.Construct
    End Sub

End Module

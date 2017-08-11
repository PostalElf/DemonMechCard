Module Module1

    Sub Main()
        Dim test1 As Component = Component.Load("Testgem")
        Dim test2 As Component = Component.Load("Testhandle")
        Dim blueprint As Blueprint = blueprint.Load("Testgun")
        blueprint.AddComponent(test1)
        blueprint.AddComponent(test2)
        Dim bodypart As BodyPart = blueprint.Construct("Testgun 01")

        Dim mechdesign As MechDesign = mechdesign.Load("Testmech")
        mechdesign.AddComponent(bodypart)
        mechdesign.Construct()
    End Sub

End Module

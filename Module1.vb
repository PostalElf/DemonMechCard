Module Module1

    Sub Main()
        Dim test1 As Component = Component.Load("Testgem")
        Dim test2 As Component = Component.Load("Testhandle")
        Dim blueprint As Blueprint = blueprint.Load("Testlimb")
        blueprint.AddComponent(test1)
        blueprint.AddComponent(test2)
        Dim bodypart As BodyPart = blueprint.Construct("Wavy Arm", DamageType.Kinetic)

        blueprint = blueprint.Load("Testgun")
        blueprint.AddComponent(test1)
        blueprint.AddComponent(test2)
        Dim weapon As BodyPart = blueprint.Construct("Pewpew Testgun", DamageType.Kinetic)

        Dim mechdesign As MechDesign = mechdesign.Load("Testmech")
        mechdesign.AddComponent(bodypart)
        mechdesign.AddComponent(weapon)
        Dim mech As Mech = mechdesign.Construct("Testsloth")
    End Sub

End Module

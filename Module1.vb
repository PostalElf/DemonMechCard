Module Module1

    Sub Main()
        Dim barrel As Component = Component.Load("Rifled Barrel")
        Dim magazine As Component = Component.Load("Automatic Clip")
        Dim ammo As Component = Component.Load("Albedo Rounds")
        Dim handcannonBP As Blueprint = Blueprint.Load("Handcannon")
        handcannonBP.AddComponent(barrel)
        handcannonBP.AddComponent(magazine)
        handcannonBP.AddComponent(ammo)
        Dim handcannon As BodyPart = handcannonBP.Construct("Alchemical Pistol", DamageType.Alchemical)

        Dim material As Component = Component.Load("Silvered Steel")
        Dim hand As Component = Component.Load("Articulated Hand")
        Dim motor As Component = Component.Load("Micromotor")
        Dim armBP As Blueprint = Blueprint.Load("Standard Arm")
        armBP.AddComponent(material)
        armBP.AddComponent(hand)
        armBP.AddComponent(motor)
        Dim arm As BodyPart = armBP.Construct("Arm", DamageType.Kinetic)

        Dim mechdesign As MechDesign = mechdesign.Load("Testmech")
        mechdesign.AddComponent(handcannon)
        mechdesign.AddComponent(arm)
        Dim mech As Mech = mechdesign.Construct("Testsloth")
        mech.FullReady()

    End Sub

End Module

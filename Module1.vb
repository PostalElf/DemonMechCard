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

        Dim armLBP As Blueprint = Blueprint.Load("Standard Arm")
        armLBP.AddComponent(Component.Load("Silvered Steel"))
        armLBP.AddComponent(Component.Load("Articulated Hand"))
        armLBP.AddComponent(Component.Load("Micromotor"))
        Dim armL As BodyPart = armLBP.Construct("Left Arm", DamageType.Kinetic)

        Dim armRBP As Blueprint = Blueprint.Load("Standard Arm")
        armRBP.AddComponent(Component.Load("Nanocarbon Steel"))
        armRBP.AddComponent(Component.Load("Articulated Hand"))
        armRBP.AddComponent(Component.Load("Micromotor"))
        Dim armR As BodyPart = armRBP.Construct("Right Arm", DamageType.Kinetic)

        Dim chassisBP As Blueprint = Blueprint.Load("Standard Chassis")
        chassisBP.AddComponent(Component.Load("Silvered Steel"))
        chassisBP.AddComponent(Component.Load("Synthetic Network"))
        chassisBP.AddComponent(Component.Load("Nuclear Reactor"))
        Dim chassis As BodyPart = chassisBP.Construct("Chassis", Nothing)

        Dim mechdesign As MechDesign = mechdesign.Load("Testmech")
        mechdesign.AddComponent(handcannon)
        mechdesign.AddComponent(armL)
        mechdesign.AddComponent(armR)
        mechdesign.AddComponent(chassis)
        Dim mech As Mech = mechdesign.Construct("Testsloth")
        mech.FullReady()

    End Sub

End Module

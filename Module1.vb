Module Module1

    Sub Main()
        Dim handcannon As BodyPart
        With Blueprint.Load("Handcannon")
            .AddComponent(Component.Load("Rifled Barrel"))
            .AddComponent(Component.Load("Automatic Clip"))
            .AddComponent(Component.Load("Albedo Rounds"))
            handcannon = .Construct("Alchemical Pistol", DamageType.Alchemical)
        End With

        Dim armL As BodyPart
        With Blueprint.Load("Standard Arm")
            .AddComponent(Component.Load("Silvered Steel"))
            .AddComponent(Component.Load("Articulated Hand"))
            .AddComponent(Component.Load("Micromotor"))
            armL = .Construct("Left Arm", DamageType.Kinetic)
        End With

        Dim armR As BodyPart
        With Blueprint.Load("Standard Arm")
            .AddComponent(Component.Load("Nanocarbon Steel"))
            .AddComponent(Component.Load("Powerfist"))
            .AddComponent(Component.Load("Micromotor"))
            armR = .Construct("Right Fist Arm", DamageType.Kinetic)
        End With

        Dim chassis As BodyPart
        With Blueprint.Load("Standard Chassis")
            .AddComponent(Component.Load("Silvered Steel"))
            .AddComponent(Component.Load("Synthetic Network"))
            .AddComponent(Component.Load("Nuclear Reactor"))
            chassis = .Construct("Chassis", Nothing)
        End With

        Dim mechdesign As MechDesign = mechdesign.Load("Testmech")
        mechdesign.AddComponent(handcannon)
        mechdesign.AddComponent(armL)
        mechdesign.AddComponent(armR)
        mechdesign.AddComponent(chassis)
        Dim mech As Mech = mechdesign.Construct("Testsloth")
        mech.FullReady()

        Console.WriteLine(mech.IsAttacked(mech.Attacks(0).Damage, 0))
        Console.ReadKey()
    End Sub

End Module

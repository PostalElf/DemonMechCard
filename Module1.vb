﻿Module Module1

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
            .AddComponent("Silvered Steel")
            .AddComponent("Articulated Hand")
            .AddComponent("Micromotor")
            armL = .Construct("Left Arm", DamageType.Kinetic)
        End With

        Dim armR As BodyPart
        With Blueprint.Load("Standard Arm")
            .AddComponent("Nanocarbon Steel")
            .AddComponent("Powerfist")
            .AddComponent("Micromotor")
            armR = .Construct("Right Fist Arm", DamageType.Kinetic)
        End With

        Dim chassis As BodyPart
        With Blueprint.Load("Standard Chassis")
            .AddComponent("Silvered Steel")
            .AddComponent("Synthetic Network")
            .AddComponent("Nuclear Reactor")
            chassis = .Construct("Chassis", Nothing)
        End With

        Dim mech As Mech
        With MechDesign.Load("Testmech")
            .AddComponent(handcannon)
            .AddComponent(armL)
            .AddComponent(armR)
            .AddComponent(chassis)
            mech = .Construct("Testsloth")
        End With
        mech.FullReady()

        Dim BattleSequence As New BattleSequence
        BattleSequence.AddCombatant(mech)
        Dim Battlefield As Battlefield = Battlefield.Construct(BattleSequence, BattlefieldTerrain.Wasteland, 1)
        Combat(Battlefield)
    End Sub

    Private Sub Combat(ByVal battlefield As Battlefield)
        While battlefield.IsOver = False
            Dim active As Combatant = battlefield.InitBagGrab
            Console.WriteLine("It is " & active.Name & "'s initiative.")
            If TypeOf active Is Enemy Then
                Console.WriteLine(CType(active, Enemy).PerformAction)
                Console.ReadKey()
            ElseIf TypeOf active Is Companion OrElse TypeOf active Is Mech Then
                Dim choices As New Dictionary(Of Char, String)
                With choices
                    .Add("a"c, "Attack")
                    .Add("s"c, "Examine Self")
                    .Add("x"c, "Examine Enemies")
                End With
                Dim choice As Char = Menu.getListChoice(choices, 0)

                Select Case choice
                    Case "a"c
                    Case "s"c
                        Console.WriteLine(battlefield.Mech.ConsoleReport)
                        Console.ReadKey()
                    Case "x"c
                End Select
            End If
        End While
    End Sub
End Module

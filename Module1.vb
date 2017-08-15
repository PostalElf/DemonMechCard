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
        mech.Battlefield = Battlefield
        mech.FullReady()
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

                While True
                    Dim choices As New Dictionary(Of Char, String)
                    With choices
                        .Add("a"c, "Attack")
                        .Add("e"c, "Equip Weapon")
                        .Add("s"c, "Examine Self")
                        .Add("x"c, "Examine Enemies")
                    End With
                    Dim choice As Char = Menu.getListChoice(choices, 0)
                    Console.WriteLine()

                    Select Case choice
                        Case "a"c
                            Dim attack As BodyPart = Menu.getListChoice(Of BodyPart)(battlefield.Mech.Attacks, 0, "Select an attack:")
                            If attack Is Nothing Then Console.WriteLine("You have no attacks!") : Continue While
                            Dim target As Combatant = Menu.getListChoice(Of Combatant)(battlefield.Mech.GetPotentialTargets(attack), 0, "Select a target:")
                            If target Is Nothing Then Console.WriteLine("No valid targets!") : Continue While
                            Dim targetLimb As BodyPart = Menu.getListChoice(Of BodyPart)(target.GetTargetableLimbs, 0, "Select a target limb:")
                            If targetLimb Is Nothing Then Console.WriteLine("No valid target limbs!") : Continue While
                            Console.WriteLine(battlefield.Mech.PerformsAttack(attack, target, targetLimb))
                            Exit While
                        Case "e"c

                        Case "s"c
                            Console.WriteLine(battlefield.Mech.ConsoleReport)
                            Exit While
                        Case "x"c
                    End Select
                End While

                Console.ReadKey()
                Console.Clear()
            End If
        End While
    End Sub
End Module

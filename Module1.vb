Module Module1

    Sub Main()
        Dim handcannon As BodyPart
        With Blueprint.Load("Handcannon")
            .AddComponent("Rifled Barrel")
            .AddComponent("Automatic Clip")
            .AddComponent("Albedo Rounds")
            handcannon = .Construct("Alchemical Pistol", DamageType.Alchemical)
        End With

        Dim armL As BodyPart
        With Blueprint.Load("Standard Arm")
            .AddComponent("Silvered Steel")
            .AddComponent("Articulated Hand")
            .AddComponent("Micromotor")
            armL = .Construct("Left Arm", DamageType.Kinetic)
            armL.IsInvulnerable = True
        End With

        Dim armR As BodyPart
        With Blueprint.Load("Standard Arm")
            .AddComponent("Nanocarbon Steel")
            .AddComponent("Powerfist")
            .AddComponent("Micromotor")
            armR = .Construct("Right Fist Arm", DamageType.Kinetic)
            armR.IsInvulnerable = True
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
            If TypeOf active Is Enemy Then
                Console.WriteLine(CType(active, Enemy).PerformAction)
                Console.ReadKey()
            ElseIf TypeOf active Is Companion OrElse TypeOf active Is Mech Then
                Dim mech As Mech = battlefield.Mech
                While True
                    Dim choices As New Dictionary(Of Char, String)
                    With choices
                        .Add("a"c, "Attack")
                        .Add("v"c, "Move")
                        .Add("e"c, "Equip Weapon")
                        .Add("s"c, "Examine Self")
                        .Add("c"c, "Examine Companions")
                        .Add("x"c, "Examine Enemies")
                        .Add("."c, "End Turn")
                    End With
                    Console.Clear()
                    Console.WriteLine("It is " & active.Name & "'s initiative.")
                    Console.WriteLine(battlefield.ConsoleReport())
                    Dim choice As Char = Menu.getListChoice(choices, 0)
                    Console.WriteLine()

                    Select Case choice
                        Case "a"c
                            Dim attack As BodyPart = Menu.getListChoice(Of BodyPart)(mech.Attacks, 0, "Select an attack:")
                            If attack Is Nothing Then Console.WriteLine("You have no attacks!") : Console.ReadKey() : Continue While
                            Dim attackType As String : If attack.IsQuick = True Then attackType = "QuickAttack" Else attackType = "Attack"
                            If mech.CheckAction(attackType) = False Then Console.WriteLine("You may only attack once per turn.") : Console.ReadKey() : Continue While
                            Dim target As Combatant = Menu.getListChoice(Of Combatant)(mech.GetPotentialTargets(attack), 0, "Select a target:")
                            If target Is Nothing Then Console.WriteLine("No valid targets!") : Console.ReadKey() : Continue While
                            Dim targetLimb As BodyPart = Menu.getListChoice(Of BodyPart)(target.GetTargetableLimbs, 0, "Select a target limb:")
                            If targetLimb Is Nothing Then Console.WriteLine("No valid target limbs!") : Console.ReadKey() : Continue While
                            Console.WriteLine(mech.PerformsAttack(attack, target, targetLimb))
                            If attack.IsQuick = True Then mech.FlagAction("QuickAttack") Else mech.FlagAction("Attack")
                        Case "v"c
                            If mech.CheckAction("Move") = False Then Console.WriteLine("You may only move once per turn.") : Console.ReadKey() : Continue While
                            Dim forwardBack As New Dictionary(Of Char, String)
                            forwardBack.Add("f"c, "Forwards")
                            forwardBack.Add("b"c, "Backwards")
                            Select Case Menu.getListChoice(forwardBack, 0, "Move Forwards or Backwards?")
                                Case "f"c : mech.PerformsMove("f"c)
                                Case "b"c : mech.PerformsMove("b"c)
                                Case Else : Console.ReadKey("Invalid selection.") : Console.ReadKey() : Continue While
                            End Select
                            Console.WriteLine()
                            Console.WriteLine(mech.Name & " is now at " & mech.DistanceFromMiddle.ToString & " range.")
                            mech.FlagAction("Move")
                        Case "e"c
                            If mech.CheckAction("Equip") = False Then Console.WriteLine("Insufficient actions!") : Console.ReadKey() : Continue While
                            Dim target As BodyPart = Menu.getListChoice(Of BodyPart)(mech.GetEquippableWeapons, 0, "Select a weapon to equip:")
                            If target Is Nothing Then Console.WriteLine("No valid handweapons!") : Console.ReadKey() : Continue While
                            mech.EquipWeapon(target)
                            Console.WriteLine(mech.Name & " equips " & target.Name & ".")
                            mech.FlagAction("Equip")
                        Case "s"c
                            Console.WriteLine(mech.ConsoleReport)
                        Case "c"c
                            Dim target As Combatant = Menu.getListChoice(Of Combatant)(battlefield.GetAllies(mech), 0, "Select companion:")
                            If target Is Nothing Then Console.WriteLine("No valid companions!") : Console.ReadKey() : Continue While
                            Console.WriteLine(target.ConsoleReport)
                        Case "x"c
                            Dim target As Combatant = Menu.getListChoice(Of Combatant)(battlefield.GetTargets(mech), 0, "Select an enemy:")
                            If target Is Nothing Then Console.WriteLine("No valid targets!") : Console.ReadKey() : Continue While
                            Console.WriteLine(target.ConsoleReport)
                        Case "."c
                            mech.EndInit()
                            Exit While
                    End Select
                    Console.ReadKey()
                End While
            End If
        End While
    End Sub
End Module

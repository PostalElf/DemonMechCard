Module Module1

    Sub Main()
        Dim test1 As Component = Component.Load("Test 01")
        Dim test2 As Component = Component.Load("Test 02")
        test1.Merge(test2)
    End Sub

End Module

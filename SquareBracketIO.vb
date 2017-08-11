<DebuggerStepThrough()>
Public Module SquareBracketIO
    Public Function SquareBracketLoader(ByVal path As String, ByVal targetName As String) As Queue(Of String)
        Dim total As New Queue(Of String)
        Dim searchFound As Boolean = False

        Using sr As New System.IO.StreamReader(path)
            While sr.Peek <> -1 AndAlso searchFound = False
                Dim line As String = sr.ReadLine
                If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                    'category header
                    'strip square brackets and check if it matches targetname
                    Dim name As String = line
                    name = name.Replace("[", "")
                    name = name.Replace("]", "")
                    If name = targetName Then
                        'category header matched
                        total.Enqueue(name)

                        'add to queue until end of file or empty line
                        While sr.Peek <> -1
                            line = sr.ReadLine
                            If line <> "" Then total.Enqueue(line) Else searchFound = True : Exit While
                        End While
                    End If
                End If
            End While
        End Using

        If total.Count = 0 Then Return Nothing Else Return total
    End Function

    Public Sub SquareBracketPacker(ByVal path As String, ByVal q As Queue(Of String))
        'TODO

        Using sw As New System.IO.StreamWriter(path)

        End Using
    End Sub
End Module

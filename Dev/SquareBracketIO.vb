'<DebuggerStepThrough()>
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
        Dim name As String = q.Peek
        Dim total = SquareBracketAllLoader(path)
        If total.ContainsKey(name) Then total.Remove(name)
        total.Add(name, q)
        SquareBracketAllPacker(path, total)
    End Sub
    Private Function SquareBracketAllLoader(ByVal path As String) As Dictionary(Of String, Queue(Of String))
        Dim total As New Dictionary(Of String, Queue(Of String))
        Using sr As New System.IO.StreamReader(path)
            While sr.Peek <> -1
                Dim line As String = sr.ReadLine
                If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                    'category header
                    'strip square brackets
                    Dim name As String = line
                    name = name.Replace("[", "")
                    name = name.Replace("]", "")

                    'create a new queue
                    Dim q As New Queue(Of String)
                    q.Enqueue(name)

                    'add to queue until end of file or empty line
                    While sr.Peek <> -1
                        line = sr.ReadLine
                        If line <> "" Then q.Enqueue(line) Else total.Add(name, q) : Exit While
                    End While
                End If
            End While
        End Using
        Return total
    End Function
    Private Sub SquareBracketAllPacker(ByVal path As String, ByVal total As Dictionary(Of String, Queue(Of String)))
        Using sw As New System.IO.StreamWriter(path)
            For Each key In total.Keys
                Dim q As Queue(Of String) = total(key)
                q.Dequeue()         'strip queue header
                sw.WriteLine("[" & key & "]")

                While q.Count > 0
                    sw.WriteLine(q.Dequeue)
                End While

                'add empty line
                sw.WriteLine()
            Next
        End Using
    End Sub

    Public Function SquareBracketCategorialLoader(ByVal path As String, ByVal categoryName As String) As List(Of Queue(Of String))
        'gets all items whose squarebracket names match
        Dim total As New List(Of Queue(Of String))
        Using sr As New System.IO.StreamReader(path)
            While sr.Peek <> -1
                Dim line As String = sr.ReadLine
                If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                    'category header
                    'strip square brackets and check if it matches
                    Dim name As String = line
                    name = name.Replace("[", "")
                    name = name.Replace("]", "")
                    If name = categoryName Then
                        Dim q As New Queue(Of String)

                        'add to queue until end of file or empty line
                        While sr.Peek <> -1
                            line = sr.ReadLine
                            If line = "" Then
                                'entry ends upon encountering empty line
                                total.Add(q)
                                Exit While
                            Else
                                q.Enqueue(line)
                            End If
                        End While
                    End If
                End If
            End While
        End Using

        Return total
    End Function
End Module

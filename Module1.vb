Module Module1



    Const TestRexExpr = "^([0-9]+\.){1,4}[0-9]+$"
    ''' <summary>
    ''' Rückgabe über Environment.ExitCode (errorlevel):
    ''' 0 = Versionen sind gleich
    ''' 1 = erste Version ist größer
    ''' -1 = erste Version ist kleiner
    ''' </summary>
    ''' <remarks></remarks>
    Sub Main()
        Dim errorWriter As IO.TextWriter = Console.Error
        Environment.ExitCode = 0

        errorWriter.WriteLine(My.Application.Info.AssemblyName)

        If My.Application.CommandLineArgs.Count <> 2 Then
            errorWriter.WriteLine("Zwei Argumente erwartet.")
        Else
            Dim v1 As String = My.Application.CommandLineArgs(0)
            Dim v2 As String = My.Application.CommandLineArgs(1)
            If Not Text.RegularExpressions.Regex.IsMatch(v1, TestRexExpr) Then
                errorWriter.WriteLine("Der erste Parameter entspricht nicht dem vorgegebenen Muster (Version).")
            Else
                If Not Text.RegularExpressions.Regex.IsMatch(v2, TestRexExpr) Then
                    errorWriter.WriteLine("Der zweite Parameter entspricht nicht dem vorgegebenen Muster (Version).")
                Else
                    'Einlesen der Versionen in interne Arrays
                    Dim v1Array As New List(Of Integer)
                    For Each m As Text.RegularExpressions.Match In Text.RegularExpressions.Regex.Matches(v1, "[0-9]+")
                        v1Array.Add(Integer.Parse(m.Value))
                    Next
                    Dim v2Array As New List(Of Integer)
                    For Each m As Text.RegularExpressions.Match In Text.RegularExpressions.Regex.Matches(v2, "[0-9]+")
                        v2Array.Add(Integer.Parse(m.Value))
                    Next

                    'Korrigieren unterschiedlicher Länge
                    If v1Array.Count > v2Array.Count Then
                        For i As Integer = v2Array.Count To v1Array.Count - 1
                            v2Array.Add(0)
                        Next
                    ElseIf v1Array.Count < v2Array.Count Then
                        For i As Integer = v1Array.Count To v2Array.Count - 1
                            v1Array.Add(0)
                        Next
                    End If

                    'errorWriter.WriteLine("v1: " + String.Join("#", v1Array))
                    'errorWriter.WriteLine("v2: " + String.Join("#", v2Array))

                    'Vergleich
                    For i As Integer = 0 To v1Array.Count - 1
                        If v1Array.Item(i) > v2Array.Item(i) Then
                            Environment.ExitCode = 1
                            Exit For
                        ElseIf v1Array.Item(i) < v2Array.Item(i) Then
                            Environment.ExitCode = -1
                            Exit For
                        End If
                    Next

                    errorWriter.WriteLine("ExitCode: {0}", Environment.ExitCode)
                    '####################
                End If
            End If
        End If
        'Console.ReadLine()
    End Sub

End Module

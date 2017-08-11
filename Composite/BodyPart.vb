Public Class BodyPart
    Inherits Component
    Public Name As String

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

﻿Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub



    Private Sub Form1_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
        Form1.ActiveForm.Close()
    End Sub
End Class
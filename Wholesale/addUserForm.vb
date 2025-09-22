Public Class addUserForm
    Public Sub New()
        InitializeComponent()

        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        Label1.BackColor = Color.FromArgb(224, 166, 109)


        saveButton.BackColor = Color.FromArgb(224, 166, 109)
        cancelButton.BackColor = Color.FromArgb(224, 166, 109)
        saveButton.ForeColor = Color.FromArgb(79, 51, 40)
        cancelButton.ForeColor = Color.FromArgb(79, 51, 40)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_SYSCOMMAND As Integer = &H112
        Const SC_RESTORE As Integer = &HF120
        Const SC_MOVE As Integer = &HF010

        If m.Msg = WM_SYSCOMMAND Then
            Dim command As Integer = (m.WParam.ToInt32() And &HFFF0)

            ' Block restore
            If command = SC_RESTORE Then
                Return
            End If

            ' Block moving
            If command = SC_MOVE Then
                Return
            End If
        End If

        MyBase.WndProc(m)
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub
End Class
Public Class sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(230, 216, 177)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)

    Public Sub New()
        InitializeComponent()
        Me.BackColor = colorClicked

    End Sub

    Private Sub sidePanelControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each btn As Button In Me.Controls.OfType(Of Button)()
            btn.Padding = New Padding(10, 0, 10, 0)
            btn.BackColor = colorClicked
            btn.ForeColor = colorUnclicked
            btn.Tag = False
            AddHandler btn.Click, AddressOf Button_Click
        Next
    End Sub

    Public Event ButtonClicked As EventHandler(Of String)

    Private Sub Button_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        RaiseEvent ButtonClicked(Me, btn.Name)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ' Reset global page
        chooseDashboard2.globalPage = ""
        Debug.WriteLine("current value is: " & chooseDashboard2.globalPage)

        ' Ensure dashboard is shown
        If Not chooseDashboard2.Visible Then
            chooseDashboard2.Show()
        End If
        chooseDashboard2.BringToFront()

        ' Keep LoginForm (startup form) and chooseDashboard alive; close others
        Dim login As Form = Nothing
        Try
            login = LoginForm
        Catch
            ' ignore if default instance not available
        End Try

        For Each frm As Form In Application.OpenForms.Cast(Of Form)().ToList()
            If frm Is chooseDashboard2 OrElse (login IsNot Nothing AndAlso frm Is login) Then
                Continue For
            End If

            Try
                ' Close other forms to reset their state
                frm.Close()
            Catch ex As Exception
                ' Fallback to hide if closing fails
                frm.Hide()
                Debug.WriteLine("Error closing form, hid instead: " & ex.Message)
            End Try
        Next

        ' Bring dashboard to front again
        chooseDashboard2.Show()
        chooseDashboard2.BringToFront()
    End Sub




    'Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
    '    ' Reset global page
    '    chooseDashboard.globalPage = ""
    '    Debug.WriteLine("current value is: " & chooseDashboard.globalPage)

    '    ' Ensure chooseDashboard is visible before closing other forms
    '    If Not chooseDashboard.Visible Then
    '        chooseDashboard.Show()
    '        chooseDashboard.BringToFront()
    '    End If

    '    ' Collect all currently open forms
    '    Dim openForms = Application.OpenForms.OfType(Of Form)().ToList()

    '    ' Close and fully dispose all except chooseDashboard
    '    For Each frm As Form In openForms
    '        If frm IsNot chooseDashboard Then
    '            Try
    '                ' Dispose of panels
    '                For Each ctrl As Control In frm.Controls.OfType(Of Control).ToList()
    '                    If TypeOf ctrl Is sidePanelControl OrElse
    '                   TypeOf ctrl Is sidePanelControl2 OrElse
    '                   TypeOf ctrl Is topPanelControl OrElse
    '                   TypeOf ctrl Is topPanelControl2 Then

    '                        frm.Controls.Remove(ctrl)
    '                        ctrl.Dispose()
    '                    End If
    '                Next

    '                frm.Close()
    '                frm.Dispose()

    '            Catch ex As Exception
    '                Debug.WriteLine("Error closing form: " & ex.Message)
    '            End Try
    '        End If
    '    Next

    '    ' Force garbage collection to clear memory
    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()

    '    ' Finally, make sure dashboard is focused
    '    chooseDashboard.Show()
    '    chooseDashboard.BringToFront()
    'End Sub


    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ' Confirm logout
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Restart()
        End If
    End Sub

End Class
﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class loginRecordsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        mainPanel = New Panel()
        Panel1 = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel5 = New Panel()
        toTextBox = New TextBox()
        Label2 = New Label()
        Panel4 = New Panel()
        fromTextBox = New TextBox()
        Label1 = New Label()
        tableDataGridView = New DataGridView()
        mainPanel.SuspendLayout()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel5.SuspendLayout()
        Panel4.SuspendLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' mainPanel
        ' 
        mainPanel.Controls.Add(Panel1)
        mainPanel.Controls.Add(tableDataGridView)
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Location = New Point(0, 0)
        mainPanel.Name = "mainPanel"
        mainPanel.Padding = New Padding(50, 120, 50, 20)
        mainPanel.Size = New Size(1810, 792)
        mainPanel.TabIndex = 0
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top
        Panel1.Controls.Add(TableLayoutPanel1)
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1810, 114)
        Panel1.TabIndex = 1
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.None
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Controls.Add(Panel5, 1, 0)
        TableLayoutPanel1.Controls.Add(Panel4, 0, 0)
        TableLayoutPanel1.Location = New Point(660, 21)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Size = New Size(441, 75)
        TableLayoutPanel1.TabIndex = 0
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(toTextBox)
        Panel5.Controls.Add(Label2)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(223, 3)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(215, 69)
        Panel5.TabIndex = 7
        ' 
        ' toTextBox
        ' 
        toTextBox.Anchor = AnchorStyles.None
        toTextBox.BorderStyle = BorderStyle.FixedSingle
        toTextBox.Font = New Font("Segoe UI", 12F)
        toTextBox.Location = New Point(47, 18)
        toTextBox.Name = "toTextBox"
        toTextBox.Size = New Size(123, 29)
        toTextBox.TabIndex = 7
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.None
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 10F)
        Label2.Location = New Point(18, 23)
        Label2.Name = "Label2"
        Label2.Size = New Size(26, 19)
        Label2.TabIndex = 6
        Label2.Text = "To:"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(fromTextBox)
        Panel4.Controls.Add(Label1)
        Panel4.Dock = DockStyle.Left
        Panel4.Location = New Point(3, 3)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(214, 69)
        Panel4.TabIndex = 3
        ' 
        ' fromTextBox
        ' 
        fromTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        fromTextBox.BorderStyle = BorderStyle.FixedSingle
        fromTextBox.Font = New Font("Segoe UI", 12F)
        fromTextBox.Location = New Point(60, 18)
        fromTextBox.Name = "fromTextBox"
        fromTextBox.Size = New Size(123, 29)
        fromTextBox.TabIndex = 3
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 10F)
        Label1.Location = New Point(12, 23)
        Label1.Name = "Label1"
        Label1.Size = New Size(44, 19)
        Label1.TabIndex = 2
        Label1.Text = "From:"
        Label1.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' tableDataGridView
        ' 
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.AllowUserToResizeColumns = False
        tableDataGridView.AllowUserToResizeRows = False
        tableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tableDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        tableDataGridView.Dock = DockStyle.Fill
        tableDataGridView.Location = New Point(50, 120)
        tableDataGridView.Name = "tableDataGridView"
        tableDataGridView.ReadOnly = True
        tableDataGridView.Size = New Size(1710, 652)
        tableDataGridView.TabIndex = 0
        ' 
        ' loginRecordsForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1810, 792)
        Controls.Add(mainPanel)
        Name = "loginRecordsForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Logs"
        WindowState = FormWindowState.Maximized
        mainPanel.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainPanel As Panel
    Friend WithEvents tableDataGridView As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents fromTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents toTextBox As TextBox
    Friend WithEvents Label2 As Label
End Class

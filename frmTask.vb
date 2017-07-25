Imports System.Data
Imports System.Data.SqlClient

Public Class frmTask
    Dim strSql As String = ""
    Dim con As New SqlConnection
    Dim cmd As New SqlCommand

#Region "Create task"
    Private Sub btncreate_Click(sender As Object, e As EventArgs) Handles btncreate.Click
        If btncreate.Text = "Create" Then
            txtname.Text = ""
            txtDesc.Text = ""
            btncreate.Text = "&Add"
        ElseIf txtname.Text = "" Or txtDesc.Text = "" Then
            MessageBox.Show("Cannot Add Empty!")

        ElseIf btncreate.Text = "&Add" Then

            For Each rowx As DataGridViewRow In dgvtask.Rows
                If rowx.Cells("colname").Value = txtname.Text Then
                    MessageBox.Show("Name exists already, Please enter another Name")
                    Exit Sub
                End If
            Next

            Dim con As New SqlConnection
            Dim cmd As New SqlCommand

            Try
                con.ConnectionString = "Server=PCPRIYA;Database=ASSIGNMENT;Trusted_Connection=Yes;Connect Timeout=100"
                con.Open()
                cmd.Connection = con
                cmd.CommandText = "insert into Task (Name,Description,datecreated,dateupdated,active) values ('" & txtname.Text.Trim.Replace("'", "''").ToString & "', '" & txtDesc.Text.Trim.Replace("'", "''").ToString & "','" & Format(DateTime.Now, "yyyy-MM-dd HH:mm:ss") & "','" & Format(DateTime.Now, "yyyy-MM-dd HH:mm:ss") & "',1)"
                cmd.ExecuteNonQuery()

                MessageBox.Show("Name and Description Created Successfully!")

            Catch ex As Exception
                MessageBox.Show("Error while inserting record on table..." & ex.Message, "Insert Records")
            Finally
                con.Close()
            End Try
        End If

        LoadDgv_Task()

    End Sub
#End Region

#Region "Load_Dgv (List task)"
    Private Sub frmTask_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDgv_Task()
    End Sub

    Private Sub LoadDgv_Task()

        dgvtask.Rows.Clear()
        Dim dttask As DataTable
        strSql = "Select ID,Name,Description,datecreated,dateupdated,active from task"

        Try
            Dim conn As New SqlConnection("Server=PCPRIYA;Database=ASSIGNMENT;Trusted_Connection=Yes;Connect Timeout=100")
            Dim comm As New SqlCommand()
            Dim objDataReader As SqlDataReader
            comm.Connection = conn
            Dim myadapter As New SqlDataAdapter(comm)

            conn.Open()
            comm.CommandText = strSql
            objDataReader = comm.ExecuteReader()
            objDataReader.Close()
            Dim myset As New DataSet
            Dim dttDatatable As New DataTable
            myadapter.Fill(myset, "table")
            conn.Close()
            dttask = myset.Tables("table")


        Catch ex As Exception
            Dim strErrorMessage = ex.Message


        End Try

        If dttask.Rows.Count <> 0 Then
            For i = 0 To dttask.Rows.Count - 1
                dgvtask.Rows.Add(dttask.Rows(i).Item("id").ToString, dttask.Rows(i).Item("Name").ToString, dttask.Rows(i).Item("Description").ToString, dttask.Rows(i).Item("active").ToString)
            Next
        End If
    End Sub
#End Region
   
#Region "Edit task"
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Try
            con.ConnectionString = "Server=PCPRIYA;Database=ASSIGNMENT;Trusted_Connection=Yes;Connect Timeout=100"
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "update Task set Name = '" & txtname.Text.Trim.Replace("'", "''").ToString & "',Description = '" & txtDesc.Text.Trim.Replace("'", "''").ToString & "',dateupdated = '" & Format(DateTime.Now, "yyyy-MM-dd HH:mm:ss") & "' where ID = '" & dgvtask.CurrentRow.Cells("colID").Value.ToString & "'"
            cmd.ExecuteNonQuery()

            MessageBox.Show("Updated Successfully!")
        Catch ex As Exception
            MessageBox.Show("Error while Editing record on table..." & ex.Message, "Insert Records")
        Finally
            con.Close()
        End Try

        dgvtask.Rows(dgvtask.CurrentRow.Index).Cells("colname").Value = txtname.Text
        dgvtask.Rows(dgvtask.CurrentRow.Index).Cells("colDesc").Value = txtDesc.Text
        'LoadDgv_Task()

    End Sub
#End Region
  
#Region "Delete task"
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnclear.Click

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Try
            con.ConnectionString = "Server=PCPRIYA;Database=ASSIGNMENT;Trusted_Connection=Yes;Connect Timeout=100"
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "Delete from Task where ID = '" & dgvtask.CurrentRow.Cells("colID").Value.ToString & "'"
            cmd.ExecuteNonQuery()

            MessageBox.Show("Deleted Successfully!")
        Catch ex As Exception
            MessageBox.Show("Error while deleting record on table..." & ex.Message, "Insert Records")
        Finally
            con.Close()
        End Try

        LoadDgv_Task()

    End Sub
#End Region

#Region "Clear and Cell click"
    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click
        txtname.Text = ""
        txtDesc.Text = ""
    End Sub

    Private Sub dgvtask_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvtask.CellClick
        If dgvtask.Rows.Count = 0 Then Exit Sub

        txtname.Text = dgvtask.CurrentRow.Cells(1).Value.ToString
        txtDesc.Text = dgvtask.CurrentRow.Cells(2).Value.ToString
        txtname.Enabled = True
        txtDesc.Enabled = True

        btncreate.Text = "Create"

    End Sub
#End Region

End Class
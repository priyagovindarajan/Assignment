Public Class frmTask
    Dim strSql As String = ""
    Dim alcol As New ArrayList
    Dim alwhere As New ArrayList
    Dim clsDa = New clsDataAccess
  
    Private Sub btncreate_Click(sender As Object, e As EventArgs) Handles btncreate.Click
        If btncreate.Text = "Create" Then
            txtname.Text = ""
            txtDesc.Text = ""
            btncreate.Text = "&Add"

        ElseIf btncreate.Text = "&Add" Then
            ' Me.Cursor = Cursors.WaitCursor
            strSql = "SELECT 1 FROM [Task] where [Name]= '" & txtname.Text.Trim & "' OR Description= '" & txtDesc.Text.Trim & "'"
            DBEvent(strSql)

            If dt.Rows.Count = 0 Then
                clsDa.TransactionBegin()

                alcol.Clear()
                alcol.Add({"Name", txtname.Text})
                alcol.Add({"Description", txtDesc.Text})
                alcol.Add({"datecreated", DateTime.Now})
                alcol.Add({"dateupdated", DateTime.Now})
                alcol.Add({"active", 1})

                Dim strErr As String = ""

                If clsDa.InsertTable("Task", alcol, , strErr) = False Then

                    clsDa.TransactionRollback()
                    ErrorMessage(strErr)
                    Exit Sub
                End If

                clsDa.TransactionCommit()
                InformationMessage("Name and Description Successfully Created.")
            Else
                WarningMessage("Name exists already, Please enter another Name.")
            End If
            
            LoadDgv_Task()
        End If

    End Sub

    Private Sub frmTask_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDgv_Task()
    End Sub

    Private Sub LoadDgv_Task()

        dgvtask.Rows.Clear()

        strSql = "Select ID,Name,Description,datecreated,dateupdated,active from task"
        Dim dttask As DataTable = clsDa.GetDatatable(strSql, , , )

        If dttask.Rows.Count <> 0 Then
            For i = 0 To dttask.Rows.Count - 1
                dgvtask.Rows.Add(dttask.Rows(i).Item("id").ToString, dttask.Rows(i).Item("Name").ToString, dttask.Rows(i).Item("Description").ToString, dttask.Rows(i).Item("active").ToString)
            Next
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        clsDa.TransactionBegin()
        alcol.Clear()
        alcol.Add({"Name", txtname.Text})
        alcol.Add({"Description", txtDesc.Text})
        alcol.Add({"dateupdated", DateTime.Now})
        alwhere.Clear()
        alwhere.Add({"ID", "=", dgvtask.CurrentRow.Cells("colID").Value.ToString})
        Dim strErr As String = ""
        strErr = ""
        If clsDa.UpdateTable("Task", alcol, alwhere, , strErr) = False Then
            clsDa.TransactionRollback()
            ErrorMessage(strErr)
            Exit Sub
        End If
        clsDa.TransactionCommit()
        InformationMessage("Name and Description Updated Successfully.")
        LoadDgv_Task()
    End Sub

    Private Sub dgvtask_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvtask.CellClick
        If dgvtask.Rows.Count = 0 Then Exit Sub

        txtname.Text = dgvtask.CurrentRow.Cells(1).Value.ToString
        txtDesc.Text = dgvtask.CurrentRow.Cells(2).Value.ToString
        '  btnsave.Enabled = True
        txtname.Enabled = True
        txtDesc.Enabled = True

        btncreate.Text = "Create"

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If ConfirmationMessage("Delete " & dgvtask.Rows(dgvtask.CurrentRow.Index).Cells("colname").Value.ToString & " ?") = True Then
            strSql = "Delete [Task] WHERE ID = '" & dgvtask.Rows(dgvtask.CurrentRow.Index).Cells("colID").Value.ToString & "'"
            DBEvent(strSql)
            InformationMessage("Selected Name and Description permanently deleted .")
            LoadDgv_Task()

        End If
    End Sub
End Class
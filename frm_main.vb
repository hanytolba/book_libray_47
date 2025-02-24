Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder
Imports System.Data.SQLite
Imports System.Reflection.Emit
'Imports System.Management
'Imports checkid
Public Class frm_main
#Region "======== MAIN_CODE==========================================="
    Dim myversion As String = "47"
    Private DBCommand As String = ""
    Private bindingsrc As BindingSource
    Private connstring As String = "Data Source=booklib" & myversion & ".db;Version=3;"
    Private connection As New SQLiteConnection(connstring)
    Private command As New SQLiteCommand("", connection)
    Dim catselect As Integer = 1
    Dim i As Integer

    '--------------------------------------
    Dim Sqlite_DB As String = "Data Source=booklib" & myversion & ".db;Version=3;"
    Dim Sqlite_connect As SQLite.SQLiteConnection
    Private sqlite_command As SQLite.SQLiteCommand
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub CheckIfDatbaseExist()
        If System.IO.File.Exists(Application.StartupPath & "booklib" & myversion & ".db") Then
            'msgboxX("تماااااااااااااام ")
        Else
            msgboxX("ملف الداتا غير موجود ")
            Application.Exit()
        End If
        ' checkid.checkId("26001180201615")
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Public Function IdToBdate(ByVal id As String) As String
        Dim bYear = Mid(id, 2, 2)
        Dim bMonth = Mid(id, 4, 2)
        Dim bDay = Mid(id, 6, 2)
        Dim cent = Mid(id, 1, 1) * 100 + 1700
        Dim bdate = (bDay.ToString) & " / " & (bMonth.ToString) & " / " & ((cent + bYear).ToString)
        Return bdate
    End Function
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub msgboxX(ByVal msg As String)
        Form1.Label1.Text = msg
        Form1.Show()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub frm_main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages.Remove(TabPage6)
        GroupBox3.Location = New Point(1, 1)
        'TabControl1.ItemSize = New Size(0, 1)
        'TabControl1.SizeMode = TabSizeMode.Fixed
        CheckIfDatbaseExist()
        Me.Text = "نظام سما نور مصر لإدارة المكتبات اصدار   " & "  001." & myversion
        Label42.Text = Me.Text
        lbl_version.Text = "001." & myversion
        Lbl_VersionText.Text = Label42.Text
        'showbooks("1")
        showbooks2()
        'Application.DoEvents()
        fillcat1()
        'Application.DoEvents()
        Fillpublisher()
        'Application.DoEvents()
        showpersons()
        'Application.DoEvents()
        show_borrowed_books()
        '====================================================
        Dim x As Process = Process.GetCurrentProcess()
        Form1.Hide()
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "dd-MM-yyyy"
        DateTimePicker2.Format = DateTimePickerFormat.Custom
        DateTimePicker2.CustomFormat = "dd-MM-yyyy"
        'Dim inf As String
        'inf = "Mem Usage: " & x.WorkingSet / 1024 & " K" & vbCrLf _
        '    & "Paged Memory: " & x.PagedMemorySize / 1024 & " K"
        'MessageBox.Show(inf, "Memory Usage")
        Lbl_VersionText.Text = "نظام  نور سما مصر لادارة المكتبات  - اصدار " &
                                "1." & myversion

    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button20_Click_1(sender As Object, e As EventArgs) Handles Btn_exportToCSV.Click
        exprt2csv2()
    End Sub

    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles btn_UsrLog.Click
        ' زائر
        GroupBox3.Visible = False

        TabControl1.TabPages.Remove(TabPage6)
        TabControl1.TabPages.Remove(TabPage2)
        TabControl1.TabPages.Remove(TabPage3)
        Btn_exportToCSV.Enabled = False

        'Button20.Visible = False
        'Btn_addBook.Visible = False
        'Btn_deleteBook.Visible = False
        'Btn_editBook.Visible = False

        Btn_addBook.Enabled = False
        Btn_deleteBook.Enabled = False
        Btn_editBook.Enabled = False
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Btn_AdmLog.Click
        ' ادارى لادخال كلمة سر
        GroupBox4.Visible = True
        btn_UsrLog.Visible = False
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Btn_return.Click
        ' عودة
        GroupBox4.Visible = False
        btn_UsrLog.Visible = True
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Btn_Enter.Click
        ' ادارى

        TabControl1.TabPages.Remove(TabPage6)
        'If Txt_password.Text = "1234" Then

        '    GroupBox3.Visible = False
        '    TabControl1.TabPages.Remove(TabPage6)
        'Else
        '    msgboxX("كلمة السر خطأ")
        'End If
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#End Region
#Region "=================BOOKS ================================"
    Private Sub searchdata()
        Dim Strsql As String = " SELECT * from books 
                                                        WHERE title Like '%" & Txt_BookTitle.Text & "%' 
                                                                And writer Like '%" & Txt_bookWriter.Text & "%'
                                                                And bookno like '%" & txt_BookNo.Text & "%'
                                                                And cat Like '%" & txt_BookCat.Text & "%'

                                                                And publisher Like '%" & CmbBx_publisher.Text & "%'
                                                                And bookid like '%" & Txt_BookID.Text & "%'
                                                        order by FORMAT('%06d', bookno)"

        Dim Sqlite_dataAdapter As New SQLite.SQLiteDataAdapter
        Dim dt As New DataTable

        Sqlite_connect = New SQLite.SQLiteConnection(Sqlite_DB)
        Sqlite_connect.Open()
        sqlite_command = New SQLite.SQLiteCommand(Strsql, Sqlite_connect)
        Sqlite_dataAdapter.SelectCommand = sqlite_command
        Sqlite_dataAdapter.Fill(dt)
        DataGV_Books.DataSource = dt
        Sqlite_connect.Close()
        lbl_BooksCount.ForeColor = Color.Black
        lbl_BooksCount.Text = "   عدد الكتب  " & DataGV_Books.Rows.Count

    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub showbooks2()

        DataGridView2.AutoGenerateColumns = False
        DataGridView2.Rows.Clear()

        Dim Strsql As String = "SELECT * from books  order by  bookid"
        Dim Sqlite_dataAdapter As New SQLite.SQLiteDataAdapter
        Dim dt As New DataTable

        Sqlite_connect = New SQLite.SQLiteConnection(Sqlite_DB)
        Sqlite_connect.Open()
        sqlite_command = New SQLite.SQLiteCommand(Strsql, Sqlite_connect)
        Sqlite_dataAdapter.SelectCommand = sqlite_command
        Sqlite_dataAdapter.Fill(dt)
        DataGV_Books.DataSource = dt
        Sqlite_connect.Close()
        lbl_BooksCount.ForeColor = Color.Black
        lbl_BooksCount.Text = "   عدد الكتب  " & DataGV_Books.Rows.Count
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub clearfields()
        Txt_BookTitle.Text = ""
        Txt_bookWriter.Text = ""
        txt_bookCab.Text = ""
        txt_bookShelf.Text = ""
        txt_publishInfo.Text = ""
        Txt_BookID.Text = ""
        txt_BookNotes.Text = ""
        txt_BookNo.Text = ""
        txt_BookCat.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
        ComboBox3.Text = ""
        CmbBx_publisher.Text = ""
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub addbook()
        If Txt_BookID.Text = "" Or Txt_BookTitle.Text = "" Then
            msgboxX("لا يمكن ادخال بيانات كتاب بدون رقم او عنوان")
        Else
            Try
                connection.Open()
                If connection.State = ConnectionState.Open Then
                    command.Connection = connection
                    command.CommandText = "insert into books (bookid, title ,writer,publisher,bookno ,
                                                                                             cat,cab,shelf,publishinfo,notes) 
                                                              values ( '" & Txt_BookID.Text & " ',
                                                                          '" & Txt_BookTitle.Text & " ',
                                                                          '" & Txt_bookWriter.Text & " ',
                                                                          '" & CmbBx_publisher.Text & " ' ,
                                                                          '" & txt_BookNo.Text & " ',
                                                                          '" & txt_BookCat.Text & " ',
                                                                          '" & txt_bookCab.Text & " ',
                                                                          '" & txt_bookShelf.Text & " ',
                                                                          '" & txt_publishInfo.Text & " ',
                                                                          '" & txt_BookNotes.Text & " ')"
                    command.ExecuteNonQuery()
                End If
                connection.Close()
            Catch ex As Exception
                If connection.State = ConnectionState.Open Then connection.Close()
                MsgBox(ex.Message)
                'MsgBox("! خطأ رقم الكتاب موجود لكتاب اخر ")
            End Try
            showbooks2()
        End If
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub delete_book()
        Dim ask As MsgBoxResult = MsgBox(" هل تريد حذف كتاب رقم  " & Txt_BookID.Text & " - " & Txt_BookTitle.Text, MsgBoxStyle.YesNo)
        If ask = MsgBoxResult.Yes Then
            connection.Open()
            If connection.State = ConnectionState.Open Then
                command.Connection = connection
                command.CommandText = "DELETE from books where bookid ='" & Txt_BookID.Text & "'"
                command.ExecuteNonQuery()

            End If
            connection.Close()
        End If

        showbooks2()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub update_book()
        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = " update books 
                                                         set  title='" & Txt_BookTitle.Text & "',
                                                         writer='" & Txt_bookWriter.Text & "',
                                                         cab='" & txt_bookCab.Text & "',
                                                         shelf='" & txt_bookShelf.Text & "',
                                                         bookno='" & txt_BookNo.Text & "',
                                                         bookid='" & Txt_BookID.Text & "',
                                                         cat='" & txt_BookCat.Text & "',
                                                         publishinfo='" & txt_publishInfo.Text & "',
                                                         publisher='" & CmbBx_publisher.Text & "',
                                                         notes=' " & txt_BookNotes.Text & " '

                                                        where bookid ='" & Txt_BookID.Text & "'"
            command.ExecuteNonQuery()
        End If
        connection.Close()
        showbooks2()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Fillpublisher()
        CmbBx_publisher.Items.Clear()
        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = "Select distinct publisher From books order by publisher"
            Dim reader As SQLiteDataReader = command.ExecuteReader
            Using reader
                While reader.Read
                    CmbBx_publisher.Items.Add(reader.GetString(0))
                End While
            End Using
        End If
        connection.Close()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub fillcat1()

        ComboBox1.Items.Clear()
        connection.Open()
        If connection.State = ConnectionState.Open Then

            command.Connection = connection
            command.CommandText = "Select * From cat Where catid Like '%00'"

            Dim reader As SQLiteDataReader = command.ExecuteReader

            Using reader
                While reader.Read
                    ComboBox1.Items.Add(reader.GetString(0) & " - " & reader.GetString(1))
                End While
            End Using
        End If
        connection.Close()

        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub fillcat2()

        ComboBox2.Items.Clear()
        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = "Select * 
                                                       From cat 
                                                       Where catid Like '" & ComboBox1.SelectedIndex & "%0' 
                                                       AND catid NOT Like '" & ComboBox1.SelectedIndex & "%00' "
            '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@============
            Dim reader As SQLiteDataReader = command.ExecuteReader
            Using reader
                While reader.Read
                    ComboBox2.Items.Add(reader.GetString(0) & " - " & reader.GetString(1))
                End While
            End Using
        End If
        connection.Close()
        ComboBox3.Items.Clear()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub fillcat3()
        ComboBox3.Items.Clear()

        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = "Select * 
                                                       From cat  
                                                       Where catid Like '" & ComboBox1.SelectedIndex & ComboBox2.SelectedIndex + 1 & "%'
                                                       AND catid NOT Like '%0' "
            '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@============
            Dim reader As SQLiteDataReader = command.ExecuteReader
            Using reader
                While reader.Read
                    ComboBox3.Items.Add(reader.GetString(0) & " - " & reader.GetString(1))
                End While
            End Using
        End If
        connection.Close()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub exprt2csv2()
        Dim tt As String = DateTime.Now.ToString("HH_mm_ss")
        Dim file As New System.IO.StreamWriter("EB" & tt & ".csv", True)
        file.WriteLine("رقم الكتاب;العنوان;المؤلف;الناشر;مسلسل;التصنيف;الدولاب;الرف;معلومات  الناشر;ملاحظات")
        Dim irowindex As Integer
        For i As Integer = 1000 To DataGV_Books.Rows.Count - 1
            Application.DoEvents()
            For y As Integer = 0 To 9
                Application.DoEvents()
                'irowindex = DataGV_Books.SelectedCells.Item(i).RowIndex
                file.Write(Trim(DataGV_Books.Rows(i).Cells(y).Value) & ";")
                '    Txt_BookTitle.Text = DataGV_Books.Rows(irowindex).Cells(1).Value
                '    Txt_bookWriter.Text = DataGV_Books.Rows(irowindex).Cells(2).Value
                '    CmbBx_publisher.Text = DataGV_Books.Rows(irowindex).Cells(3).Value
                '    txt_BookNo.Text = DataGV_Books.Rows(irowindex).Cells(4).Value
                '    txt_BookCat.Text = DataGV_Books.Rows(irowindex).Cells(5).Value
                '    txt_bookCab.Text = DataGV_Books.Rows(irowindex).Cells(6).Value
                '    txt_bookShelf.Text = DataGV_Books.Rows(irowindex).Cells(7).Value
                '    txt_publishInfo.Text = DataGV_Books.Rows(irowindex).Cells(8).Value
                '    txt_BookNotes.Text = DataGV_Books.Rows(irowindex).Cells(9).Value
                '    If Len(Trim(txt_BookCat.Text)) = 3 Then

                '        ComboBox1.SelectedIndex = CInt(Mid(txt_BookCat.Text, 1, 1))
                '        ComboBox2.SelectedIndex = CInt(Mid(txt_BookCat.Text, 2, 1)) - 1
                '        ComboBox3.SelectedIndex = CInt(Mid(txt_BookCat.Text, 3, 1)) - 1
                '    Else

                '        ComboBox1.SelectedIndex = -1
                '        ComboBox2.SelectedIndex = -1
                '        ComboBox3.SelectedIndex = -1
                '    End If
            Next
            file.WriteLine()
        Next
        file.Close()
    End Sub

    Private Sub export2csv()
        Dim sortcase As String = " SELECT * from books  order by  bookid"

        Dim file As New System.IO.StreamWriter("EXPORTED_BOOKS.csv", True)
        file.WriteLine("رقم الكتاب;العنوان;المؤلف;الناشر;مسلسل;التصنيف;الدولاب;الرف;معلومات  الناشر;ملاحظات")
        Cursor = Cursors.WaitCursor
        msgboxX("      جارى التصدير     ")
        'Application.DoEvents()

        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = sortcase
            Dim reader As SQLiteDataReader = command.ExecuteReader
            Using reader
                While reader.Read
                    file.WriteLine(
                        reader.GetInt16(0) & ";" & reader.GetString(1) & ";" &
                        reader.GetString(2) & ";" & reader.GetString(3) & ";" &
                        reader.GetString(4) & ";" & reader.GetString(5) & ";" &
                        reader.GetString(6) & ";" & reader.GetString(7) & ";" &
                        reader.GetString(8) & ";" & reader.GetString(9)
                        )
                End While
            End Using
        End If
        connection.Close()
        Form1.Hide()
        Cursor = Cursors.Default
        file.Close()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Btn_BookClear.Click
        clearfields()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Btn_BookSearch.Click
        searchdata()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub DataGridView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles DataGV_Books.MouseDoubleClick
        Dim irowindex As Integer

        irowindex = DataGV_Books.SelectedCells.Item(i).RowIndex
        Txt_BookID.Text = DataGV_Books.Rows(irowindex).Cells(0).Value
        Txt_BookTitle.Text = DataGV_Books.Rows(irowindex).Cells(1).Value
        Txt_bookWriter.Text = DataGV_Books.Rows(irowindex).Cells(2).Value
        CmbBx_publisher.Text = DataGV_Books.Rows(irowindex).Cells(3).Value
        txt_BookNo.Text = DataGV_Books.Rows(irowindex).Cells(4).Value
        txt_BookCat.Text = DataGV_Books.Rows(irowindex).Cells(5).Value
        txt_bookCab.Text = DataGV_Books.Rows(irowindex).Cells(6).Value
        txt_bookShelf.Text = DataGV_Books.Rows(irowindex).Cells(7).Value
        txt_publishInfo.Text = DataGV_Books.Rows(irowindex).Cells(8).Value
        txt_BookNotes.Text = DataGV_Books.Rows(irowindex).Cells(9).Value
        If Len(Trim(txt_BookCat.Text)) = 3 Then

            ComboBox1.SelectedIndex = CInt(Mid(txt_BookCat.Text, 1, 1))
            ComboBox2.SelectedIndex = CInt(Mid(txt_BookCat.Text, 2, 1)) - 1
            ComboBox3.SelectedIndex = CInt(Mid(txt_BookCat.Text, 3, 1)) - 1
        Else

            ComboBox1.SelectedIndex = -1
            ComboBox2.SelectedIndex = -1
            ComboBox3.SelectedIndex = -1
        End If

    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Btn_addBook.Click
        addbook()
    End Sub

    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Btn_deleteBook.Click
        delete_book()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Btn_editBook.Click
        update_book()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ComboBox3.Text = ""
        fillcat3()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        txt_BookCat.Text = ComboBox1.SelectedIndex & ComboBox2.SelectedIndex + 1 & ComboBox3.SelectedIndex + 1


    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub catTextBox1_Enter(sender As Object, e As EventArgs) Handles txt_BookCat.Enter
        txt_BookCat.BackColor = Color.White
        catselect = 1
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub catTextBox1_Leave(sender As Object, e As EventArgs) Handles txt_BookCat.Leave
        txt_BookCat.BackColor = Color.FromArgb(255, 255, 128)
    End Sub


    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ComboBox2.Text = ""
        ComboBox3.Text = ""
        fillcat2()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#End Region
#Region "===================== PERSONS ==================================="
    Private Sub showpersons()
        Label29.ForeColor = Color.Red
        Label29.Text = " .... loading ....."
        Cursor = Cursors.WaitCursor
        'Application.DoEvents()

        DataGridView2.Rows.Clear()
        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = " SELECT * from persons  order by  id "
            Dim reader As SQLiteDataReader = command.ExecuteReader
            Using reader
                While reader.Read
                    Me.DataGridView2.Rows.Add(reader.GetInt16(0), reader.GetString(1), reader.GetString(2),
                                                                      reader.GetString(3), reader.GetString(4), reader.GetString(5),
                                                                      reader.GetString(6), reader.GetString(7))
                    Application.DoEvents()

                End While
            End Using
        End If
        connection.Close()
        Label29.ForeColor = Color.Black
        Label29.Text = "   عدد المشتركين  " & DataGridView2.Rows.Count


        'For i As Integer = 0 To Me.DataGridView2.Rows.Count - 1
        '    If chkId(Me.DataGridView2.Rows(i).Cells(2)) Then

        '        'DataGridView2.Rows(i).Cells(2).Style.BackColor = Color.Red
        '        DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.Red
        '    End If
        'Next

        For i As Integer = 0 To Me.DataGridView2.Rows.Count - 1
            If (Mid(Me.DataGridView2.Rows(i).Cells(2).Value, 1, 1) = "0") Then
                'DataGridView2.Rows(i).Cells(2).Style.BackColor = Color.Red
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.Red
            End If
        Next

        Cursor = Cursors.Default
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub addperson()
        If txt_userNo.Text = "" Or TextBox9.Text = "" Or TextBox14.Text = "" Then
            msgboxX("لا يمكن ادخال بيانات مشترك بدون رقم او أسم او رقم قومى")
        Else
            Try
                connection.Open()
                If connection.State = ConnectionState.Open Then
                    command.Connection = connection
                    command.CommandText = "insert into persons (id,name,natid,tel,grade,study,village,notes)
                                                              values ( '" & txt_userNo.Text & " ',
                                                                          '" & TextBox9.Text & " ',
                                                                          '" & TextBox14.Text & " ',
                                                                          '" & TextBox15.Text & " ',
                                                                          '" & TextBox16.Text & " ',
                                                                         '" & TextBox18.Text & " ',
                                                                          '" & TextBox20.Text & " ',
                                                                          '" & TextBox21.Text & "')"
                    command.ExecuteNonQuery()
                End If
                connection.Close()
            Catch ex As Exception
                If connection.State = ConnectionState.Open Then connection.Close()
                MsgBox(ex.Message)

            End Try
            showpersons()
        End If
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub delete_person()
        Dim ask As MsgBoxResult = MsgBox(" هل تريد حذف مشترك  رقم  " & txt_userNo.Text & " - " & TextBox9.Text, MsgBoxStyle.YesNo)
        If ask = MsgBoxResult.Yes Then
            connection.Open()
            If connection.State = ConnectionState.Open Then
                command.Connection = connection
                command.CommandText = "DELETE from persons where id='" & txt_userNo.Text & "'"
                command.ExecuteNonQuery()
            End If
            connection.Close()

        End If
        showpersons()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub clearPersonsField()

        Dim aaa As Control
        For Each aaa In GroupBox2.Controls
            If aaa.Tag = "ppp" Then
                aaa.Text = ""
            End If
        Next
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub searchpersons()
        Label29.ForeColor = Color.Red
        Label29.Text = " .... loading ....."
        Cursor = Cursors.WaitCursor
        'Application.DoEvents()

        DataGridView2.Rows.Clear()
        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = " SELECT * from persons 
                                                        WHERE id like '%" & txt_userNo.Text & "%' 
                                                        And name Like '%" & TextBox9.Text & "%'
                                                        And natid like '%" & TextBox14.Text & "%'
                                                        And tel Like '%" & TextBox15.Text & "%'
                                                        order by  id"

            Dim reader As SQLiteDataReader = command.ExecuteReader
            Using reader
                While reader.Read
                    Me.DataGridView2.Rows.Add(reader.GetInt16(0), reader.GetString(1), reader.GetString(2),
                    reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7))
                End While
            End Using
        End If
        connection.Close()
        Label29.ForeColor = Color.Black
        Label29.Text = "   عدد المشتركين :" & DataGridView2.Rows.Count
        Cursor = Cursors.Default
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub UpdatePerson()
        connection.Open()
        If connection.State = ConnectionState.Open Then
            command.Connection = connection
            command.CommandText = " update persons
                                                         set 
                                                               id='" & txt_userNo.Text & "',
                                                         name='" & TextBox9.Text & "',
                                                          natid='" & TextBox14.Text & "',
                                                              tel='" & TextBox15.Text & "',
                                                         grade='" & TextBox16.Text & "',
                                                        village='" & TextBox18.Text & "',
                                                        study='" & TextBox20.Text & "',
                                                          notes='" & TextBox21.Text & "'

                                                        where id ='" & txt_userNo.Text & "'"
            command.ExecuteNonQuery()
        End If
        connection.Close()
        showpersons()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub DataGridView2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles DataGridView2.MouseDoubleClick
        Dim irowindex As Integer
        For i As Integer = 0 To DataGridView2.SelectedCells.Count - 1
            irowindex = DataGridView2.SelectedCells.Item(i).RowIndex
            txt_userNo.Text = Trim(DataGridView2.Rows(irowindex).Cells(0).Value)
            TextBox9.Text = DataGridView2.Rows(irowindex).Cells(1).Value
            TextBox14.Text = DataGridView2.Rows(irowindex).Cells(2).Value
            TextBox15.Text = DataGridView2.Rows(irowindex).Cells(3).Value
            TextBox16.Text = DataGridView2.Rows(irowindex).Cells(4).Value
            TextBox18.Text = DataGridView2.Rows(irowindex).Cells(5).Value
            TextBox20.Text = DataGridView2.Rows(irowindex).Cells(6).Value
            TextBox21.Text = DataGridView2.Rows(irowindex).Cells(7).Value
            Label28.Text = (Date.Now.Year) -
                ((Mid(DataGridView2.Rows(irowindex).Cells(2).Value, 1, 1) * 100) + 1700 +
                (Mid(DataGridView2.Rows(irowindex).Cells(2).Value, 2, 2)))
            Label31.Text = IdToBdate(TextBox14.Text)
            If (Label28.Text) > 100 Or (Label28.Text) < 4 Then
                Label28.BackColor = Color.Red
            Else
                Label28.BackColor = Color.White '
            End If
        Next
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        addperson()
    End Sub
    Private Sub member_Button1_Click(sender As Object, e As EventArgs) Handles member_Button1.Click
        delete_person()
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        clearPersonsField()
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs)
        searchpersons()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        UpdatePerson()
    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        showpersons()
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#End Region
#Region "============ BORROW ======================================="
    Private Sub show_borrowed_books()
        DataGV_Borrow.AutoGenerateColumns = False
        DataGV_Borrow.Rows.Clear()
        Dim Strsql As String =
        "SELECT borrowid,w.bookid, b.title , w.personid,p.name,w.borrowdate,
                w.period,w.actualretdate,
                JULIANDAY(w.actualretdate) - JULIANDAY(w.borrowdate) as sd
         FROM persons p 
                                JOIN books b 
                                JOIN borrows w 
          WHERE w.personId = p.id and w.bookid = b.bookid
          order by sd desc"
        Dim Sqlite_dataAdapter As New SQLite.SQLiteDataAdapter
        Dim dt As New DataTable
        Sqlite_connect = New SQLite.SQLiteConnection(Sqlite_DB)
        Sqlite_connect.Open()
        sqlite_command = New SQLite.SQLiteCommand(Strsql, Sqlite_connect)
        Sqlite_dataAdapter.SelectCommand = sqlite_command
        Sqlite_dataAdapter.Fill(dt)
        DataGV_Borrow.DataSource = dt
        Sqlite_connect.Close()

    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub clearBorrow()
        Dim aaa As Control
        For Each aaa In GroupBox1.Controls
            If aaa.Tag = "bbb" Then
                aaa.Text = ""
            End If
        Next

    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub addBorrow()
        'clearBorrow()
        'Txt_borrow_no.Text = DataGridView3.Rows.Count + 1
        'txt_borrow_book_no.Text = Txt_BookNo.Text
        'TextBox11.Text = TextBox2.Text
        'txt_borrow_user_no.Text = TextBox1.Text
        'TextBox8.Text = TextBox9.Text

    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub saveborrow()

        'If Txt_borrow_no.Text = "" Or txt_borrow_book_no.Text = "" Or txt_borrow_user_no.Text = "" Then
        '    msgboxX("لا يمكن ادخال استعارة بدون رقم او كتاب او مشترك")
        'Else
        '    Try
        '        connection.Open()
        '        If connection.State = ConnectionState.Open Then
        '            command.Connection = connection
        '            command.CommandText = "insert into borrows (borrowid, bookid,
        '                                                          personid,borrowdate,period,
        '                                                          actualretdate,notes) 
        '                                                      values ( '" & Txt_borrow_no.Text & " ',
        '                                                                  '" & txt_borrow_book_no.Text & " ',
        '                                                                  '" & txt_borrow_user_no.Text & " ',
        '                                                                  '" & DateTimePicker1.Value.Date.ToString("yyyy-MM-dd") & " ',
        '                                                                  '" & TextBox24.Text & " ',
        '                                                                  '" & DateTimePicker2.Value.Date.ToString("yyyy-MM-dd") & " ',
        '                                                                  '" & TextBox26.Text & " ')"
        '            command.ExecuteNonQuery()
        '        End If
        '        connection.Close()
        '    Catch ex As Exception
        '        If connection.State = ConnectionState.Open Then connection.Close()
        '        MsgBox(ex.Message)
        '        'MsgBox("! خطأ رقم الكتاب موجود لكتاب اخر ")
        '    End Try
        '    'showborrow()
        'End If
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub DataGridView3_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles DataGV_Borrow.MouseDoubleClick
        'Dim irowindex As Integer
        'For i As Integer = 0 To DataGV_Borrow.SelectedCells.Count - 1
        '    irowindex = DataGV_Borrow.SelectedCells.Item(i).RowIndex
        '    Txt_borrow_no.Text = DataGV_Borrow.Rows(irowindex).Cells(0).Value
        '    txt_borrow_book_no.Text = DataGV_Borrow.Rows(irowindex).Cells(1).Value
        '    TextBox11.Text = DataGV_Borrow.Rows(irowindex).Cells(2).Value
        '    txt_borrow_user_no.Text = DataGV_Borrow.Rows(irowindex).Cells(3).Value
        '    TextBox8.Text = DataGV_Borrow.Rows(irowindex).Cells(4).Value

        '    TextBox24.Text = DataGV_Borrow.Rows(irowindex).Cells(6).Value
        '    TextBox23.Text = DataGV_Borrow.Rows(irowindex).Cells(7).Value

        '    TextBox26.Text = DataGV_Borrow.Rows(irowindex).Cells(9).Value

        '    DateTimePicker1.Value = Convert.ToDateTime(DataGV_Borrow.Rows(irowindex).Cells(5).Value)
        '    DateTimePicker2.Value = Convert.ToDateTime(DataGV_Borrow.Rows(irowindex).Cells(8).Value)
        'Next
    End Sub
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        addBorrow()
    End Sub
    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        clearBorrow()
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        saveborrow()
        show_borrowed_books()

    End Sub
    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Dim asd As Date = DateAdd("d", Convert.ToInt32(TextBox24.Text), DateTimePicker1.Value)
        TextBox23.Text = Format(asd, "dd, MMM, yyyy")
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        show_borrowed_books()
    End Sub

    Private Sub Btn_ShowBook_Click(sender As Object, e As EventArgs) Handles Btn_ShowBook.Click
        showbooks2()

    End Sub








#End Region
End Class
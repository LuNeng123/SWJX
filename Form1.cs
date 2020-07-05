using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace SWJX
{
    public partial class Form1 : Form
    {
        //private Sqlite_ sq = new Sqlite_();
        private Mysql_ sq = new Mysql_();
        private Mis_ ms = new Mis_();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label37.Text = "用户名：" + Environment.UserName;
            sq.f0();
            MySqlDataReader reader = sq.f1("select rowid, 任务号, 任务内容 from 任务表 where 状态 <> 4 order by 任务号 asc ");
            while (reader.Read())
            {
                string str0 = Convert.ToString((int)reader["rowid"]);
                string str1 = (string)reader["任务号"];
                string combo_str =  str0+"/"+ str1 + "/ " + (string)reader["任务内容"];
                listBox1.Items.Add(combo_str);
                listBox2.Items.Add(combo_str);
                listBox3.Items.Add(combo_str);
            }
            reader.Close();

            MySqlDataReader reader2 = sq.f1("select id,名称 from 输出模板  order by id");
            while (reader2.Read())
            {
                string str1 = Convert.ToString((int)reader2["id"]);
                string str2 = (string)reader2["名称"];
                string combo_str = "(" + str1 + ")    " + str2;
                listBox4.Items.Add(combo_str);
            }
            reader2.Close();
            listBox4.SelectedIndex = 0;

            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;
            listBox3.SelectedIndex = 0;
            listBox5.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 0;
            dateTimePicker8.Value = DateTime.Today.AddDays(-7);
            dateTimePicker7.Value = DateTime.Today.AddDays(7);
            dateTimePicker3.Value = DateTime.Today.AddDays(-7);
            dateTimePicker4.Value = DateTime.Today.AddDays(7);
        }


        class Mis_
        {
            public int get_layers(string str)
            {
                int ct = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '-')
                    {
                        ct += 1;
                    }
                }
                return ct;
            }

            public ArrayList Parse_layers(string str)
            {
                int ct = get_layers(str);
                ArrayList arr = new ArrayList();
                string[] spl = str.Split('-');
                for (int i = 1; i < spl.Length; i++)
                {
                    try
                    {
                        arr.Add(Convert.ToInt32(spl[i]));
                    }
                    catch
                    {
                        arr.Add(999);
                    }
                }
                return arr;  
            }

            public string StringRepeat(string str, int n)
            {
                if (String.IsNullOrEmpty(str) || n <= 0)
                    return str;
                StringBuilder sb = new StringBuilder();
                while (n > 0)
                {
                    sb.Append(str);
                    n--;
                }
                return sb.ToString();
            }
        }

        /*class Sqlite_
        {
            private static string connStr = "Data Source=" + @"C:\Users\陆能\Desktop\项目管理\SWJX.db";//;Version=3;";
                                    // "Data Source=" + @"C:\Users\tdht\Desktop\test.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
            private static SQLiteConnection conn = conn = new SQLiteConnection(connStr);

            //连接测试
            public void f0()
            {
                try 
                { 
                    conn.Open();
                }
                 catch{
                    MessageBox.Show("数据库连接失败！");
                }
            }


            //查询sqlite数据库
            public SQLiteDataReader f1(string sql)
            {
                if (conn.State == ConnectionState.Broken)
                {
                    f0();
                }
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                return reader;

            }

            public void f2(string sql)
            {
                if (conn.State == ConnectionState.Broken)
                {
                    f0();
                }
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                int row = cmd.ExecuteNonQuery();
            }

            
            public static void TestSqlite()
            {
                try
                {
                    //测试连接是否成功。
                    f0();
                    conn.Open();
                    //查询。
                    f1();
                    //增加。
                    f2();
                    //修改
                    f3();
                    //删除
                    //f4();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                Console.ReadKey();
            }
        }
        */
        class Mysql_
        {
            private static MySqlConnection conn;
            public void f0()
            {
                String mysqlStr = "Database=luneng;Data Source=sql.w127.vhostgo.com; User Id=luneng;Password=Abcpb666;pooling=false;CharSet=utf8;port=3306";
                // String mySqlCon = ConfigurationManager.ConnectionStrings["MySqlCon"].ConnectionString;
                conn = new MySqlConnection(mysqlStr);
                try
                {
                    conn.Open();
                }
                catch
                {
                    MessageBox.Show("数据库连接失败！\n请检查网络");
                }
    
            }

            public MySqlDataReader f1(string sql)
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    f0();
                }
                MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                return reader;
                /*
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("name:" + reader.GetString(0));
                        }
                    }*/

            }

            public void f2(string sql)
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    f0();
                }
                MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
                int row = mySqlCommand.ExecuteNonQuery();
            }

            public void f3(string sql, string v0, string v1, int v2)
            {
                /*
                MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
                StringBuilder sql = new StringBuilder();
                sql.Append("refresh_");
                Convert.ToString(sql);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                MySqlParameter[] parameters = {
                   new MySqlParameter("@rwh", MySqlDbType.Text,0),
                   new MySqlParameter("@usr", MySqlDbType.Text,0),
                   new MySqlParameter("@zt", MySqlDbType.Int32,11),
                };
                parameters[0].Value = v0;
                parameters[0].Direction = ParameterDirection.Input;
                parameters[1].Value =v1;
                parameters[1].Direction = ParameterDirection.Input;
                parameters[2].Value = v2;
                parameters[2].Direction = ParameterDirection.Input;
                mySqlCommand.ExecuteNonQuery();
                */
            }

        }

        private void listbox_refresh(int to_select)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            string sqlstr;
            if (checkBox1.Checked && checkBox2.Checked)
            {
                sqlstr = "select rowid, 任务号, 任务内容 from 任务表 order by 任务号 asc ";
            }
            else if (checkBox1.Checked && !checkBox2.Checked)
            {
                sqlstr = "select rowid, 任务号, 任务内容 from 任务表 where 状态 <> 4 order by 任务号 asc ";
            }
            else if (!checkBox1.Checked && checkBox2.Checked)
            {
                sqlstr = "select rowid, 任务号, 任务内容 from 任务表 where 状态 <> 2 order by 任务号 asc ";
            }
            else
            {
                sqlstr = "select rowid, 任务号, 任务内容 from 任务表  where 状态 <> 4 and 状态 <> 2  order by 任务号 asc ";
            }
            MySqlDataReader reader = sq.f1(sqlstr);
            while (reader.Read())
            {
                string str0 = Convert.ToString((int)reader["rowid"]);
                string str1 = (string)reader["任务号"];
                string combo_str = str0 + "/" + str1 + "/ " + (string)reader["任务内容"];
                listBox1.Items.Add(combo_str);
                listBox2.Items.Add(combo_str);
                listBox3.Items.Add(combo_str);
            }
            reader.Close();
            try
            {
                listBox1.SelectedIndex = to_select;
                listBox2.SelectedIndex = to_select;
                listBox3.SelectedIndex = to_select;
            }
            catch
            {
                listBox1.SelectedIndex = 0;
                listBox2.SelectedIndex = 0;
                listBox3.SelectedIndex = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int to_select = listBox1.SelectedIndex;
            listbox_refresh(to_select);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox4.Text = "";
            button2.Text = "确认添加";
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedItem = 0;
            dateTimePicker2.Refresh();
            textBox5.Text = "";
            panel1.Enabled = true;
            string mystr = (string)listBox1.SelectedItem;

            textBox00.Text = mystr.Split('/')[0].Replace(" ", "");
            string nowstr = mystr.Split('/')[1].Replace(" ", "");
            string taskname = mystr.Split('/')[2].Replace(" ", "");
            int nowstr_layers = ms.get_layers(nowstr);
            MySqlDataReader reader_0 = sq.f1("select 任务号 from 任务表 where 状态<>4 AND (任务号 ='" 
                + nowstr + "' OR 任务号 LIKE '" + nowstr + "-%')  order by 任务号 desc ");
            reader_0.Read(); //只用读取第一个即可，是倒序排序；
            ArrayList arr = ms.Parse_layers((string)reader_0 ["任务号"]+ "-00");
            reader_0.Close();

            arr[nowstr_layers] = (int)arr[nowstr_layers] + 1;
            ArrayList arr_new = new ArrayList();
            for (int i = 0; i < nowstr_layers+1; i++)
            {
                arr_new.Add(arr[i]);
            }
            string str0 = "RW";  //任务号字符串头
            for (int i = 0; i < arr_new.Count; i++)
            {
                string hh = "0" + (arr[i].ToString());
                str0 = str0 + "-" + hh.Substring(hh.Length - 2, 2);
            }

            textBox1.Text = str0;
            textBox2.Text = "(" + taskname + " 的子任务)";
            textBox2.Select(0, textBox2.Text.Length);
            textBox2.Focus();
            textBox14.Text = "-1";
            textBox15.Text = "-1";

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            byte state_ = 1;   // 1 - 新增  2 - 修改
            if (button2.Text == "确认修改")
            {
                state_ = 2;
            }

            string str_id = textBox00.Text;
            if (state_ ==2)
            {
                string sqlstr = "DELETE FROM 任务表 WHERE rowid=" + str_id;
                sq.f2(sqlstr);
                button2.Text = "确认添加";
            }

            string str0 = textBox1.Text;
            string str1 = textBox2.Text;
            if (str0.Trim().Length == 0 || str1.Trim().Length == 0)
            {
                MessageBox.Show("任务内容/任务号不能为空！");
            }
            else
            {
                string str2 = textBox3.Text;
                string str3 = textBox4.Text;
                string str4 = comboBox2.Text.Substring(0, 1);
                string str5 = comboBox3.Text.Substring(0, 1);
                string str6 = dateTimePicker2.Value.ToString("yyyyMMdd");
                string str7 = textBox5.Text;
                string str8 = textBox14.Text;
                string str9 = textBox15.Text;
                string sqlstr;
                //后插入
                sqlstr = "INSERT INTO 任务表(任务号, 任务内容, 责任人, 支持方, 状态, 困难程度, 预计完成时间, 备注, 最新状态的流水号, 对应流水号, user) " +
                    "VALUES('" + str0 + "','" + str1 + "','" + str2 + "','" + str3 + "'," + str4 + "," + str5 + ", " + str6 + " ,'" + str7 + "'," 
                    + str8 + ",'"+ str9 + "','"+ Environment.UserName + "')";
                sq.f2(sqlstr);
                button3_Click(new object(), new EventArgs());
                panel1.Enabled = false;

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                dateTimePicker2.Refresh();
                textBox5.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int to_select = listBox1.SelectedIndex;
            listbox_refresh(to_select);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nowstr = (string)listBox1.SelectedItem;
            string str_1 = nowstr.Split('/')[1].Replace(" ", "");
            string str_2 = nowstr.Split('/')[2].Replace(" ", "");
            textBox01.Text = nowstr.Split('/')[0].Replace(" ", "");
            textBox9.Text = str_1;
            textBox8.Text = str_2;
            //textBox6.Text = "";
            //textBox7.Text = "";
            button6.Enabled = true;
            textBox6.Enabled = true;
            checkBox17.Checked = false;
        }

        private void button6_Click(object sender, EventArgs e)   //添加作业
        {

            string str0 = dateTimePicker1.Value.ToString("yyyyMMdd");  // 作业日期
            string str_id = textBox01.Text; // rowid
            string str1 = textBox9.Text;  // 任务号
            string str2 = textBox6.Text.Replace("\n","").Replace("\r","").Trim();  // 任务内容
            string str3 = "99" + listBox5.Text.Substring(0, 1); //状态
            string str4 = textBox7.Text;  // 作业人员

            if (str2.Trim().Length == 0 || str4.Trim().Length == 0)
            {
                MessageBox.Show("请填写完整内容！");
            }
            else
            {
                string sql_before = "SELECT 最新状态的流水号 FROM 任务表 WHERE rowid=" + str_id;
                MySqlDataReader reader_b = sq.f1(sql_before);
                reader_b.Read();
                int before_lsh = Convert.ToInt32(reader_b["最新状态的流水号"]);  // 提取之前的流水号
                reader_b.Close();
                string sql_0 = "INSERT INTO 作业流水记录 (时间, 任务表id,内容,作业结果,作业人员,user) VALUES(" + str0 + ", '" + str_id + "','"
                    + str2 + "'," + str3 + ",'" + str4 + "','"+ Environment.UserName+"')";
                sq.f2(sql_0);
                string sql_1 = "CALL refresh_new('" + str_id + "');";
                sq.f2(sql_1);
                if (checkBox17.Checked)  // 覆盖上一条数据
                {
                    string sql_after = "DELETE FROM 作业流水记录 WHERE rowid=" + Convert.ToSingle(before_lsh);
                    sq.f2(sql_after);
                }
                panel4.Visible = true;
                timer1.Enabled = true;

                dateTimePicker1.Refresh();
                //textBox9.Text = "";
                textBox6.Text = "";
                listBox5.Text = "";
                //textBox7.Text = "";
                checkBox17.Checked = false;
                //button6.Enabled = false;
                //textBox6.Enabled = false;
                
            }
            mock_btn_10_click();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != tabControl1.TabPages[0])
            {
                panel1.Enabled = false;
            }
            if (tabControl1.SelectedTab != tabControl1.TabPages[1])
            {
                checkBox17.Checked = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string rwh = (string)listBox1.SelectedItem;
            richTextBox1.Clear();
            string nowstr = rwh.Split('/')[1].Replace(" ", "");
            int nowstr_layers = ms.get_layers(nowstr);
            MySqlDataReader reader = sq.f1(
                "SELECT rowid, 任务号 FROM 任务表 WHERE 任务号 ='" + nowstr + "' OR 任务号 LIKE'" + nowstr + "-%'  ORDER BY 任务号 ASC ");
            ArrayList rwh_arr = new ArrayList();
            string last_ctt = "nothing";
            while (reader.Read())
            {
                string ctt = (string)reader["任务号"];
                if (radioButton1.Checked)
                {
                    int textbox10_text;
                    try
                    {
                        textbox10_text = Math.Abs(Convert.ToInt32(textBox10.Text));
                    }
                    catch
                    {
                        textbox10_text = 3;
                    }
                    if (ms.get_layers(ctt) <= nowstr_layers + textbox10_text)
                    {
                        rwh_arr.Add(ctt);
                    }
                }
                else if (radioButton2.Checked)
                {
                    rwh_arr.Add(ctt);
                }
                else
                {
                    if (ctt.IndexOf(last_ctt) < 0)
                    {
                        rwh_arr.Add(last_ctt);
                    }
                    last_ctt = ctt;
                }
            }
            reader.Close();
            if (radioButton3.Checked)
            {
                rwh_arr.Add(last_ctt);
                rwh_arr.RemoveAt(0);
            }

            string clause1 = textBox12.Text;
            clause1 = string.Join("%' OR 责任人 LIKE'%", Regex.Split(clause1, "、"));
            clause1 = "(责任人 LIKE'%" + clause1 + "%')";
            string clause2 = textBox13.Text;
            clause2 = string.Join("%' OR 支持方 LIKE'", Regex.Split(clause2, "、"));
            clause2 = "(支持方 LIKE'%" + clause2 + "%')";
            string clause3;
            clause3 = "(预计完成时间>=" + dateTimePicker5.Value.ToString("yyyyMMdd")
                + " AND 预计完成时间<=" + dateTimePicker6.Value.ToString("yyyyMMdd") + ") ";
            string clause4_1 = checkBox7.Checked ? "状态=0" : "状态=999";
            string clause4_2 = checkBox8.Checked ? "状态=1" : "状态=999";
            string clause4_3 = checkBox9.Checked ? "状态=2" : "状态=999";
            string clause4_4 = checkBox10.Checked ? "状态=3" : "状态=999";
            string clause4_5 = checkBox11.Checked ? "状态=4" : "状态=999";
            string clause4 = "(" + clause4_1 + " OR " + clause4_2 + " OR " + clause4_3 + " OR "
                + clause4_4 + " OR " + clause4_5 + ")";
            string clause5_1 = checkBox12.Checked ? "困难程度=0" : "困难程度=999";
            string clause5_2 = checkBox13.Checked ? "困难程度=1" : "困难程度=999";
            string clause5_3 = checkBox14.Checked ? "困难程度=2" : "困难程度=999";
            string clause5_4 = checkBox15.Checked ? "困难程度=3" : "困难程度=999";
            string clause5_5 = checkBox16.Checked ? "困难程度=4" : "困难程度=999";
            string clause5 = "(" + clause5_1 + " OR " + clause5_2 + " OR " + clause5_3 + " OR "
                + clause5_4 + " OR " + clause5_5 + ")";
            string sql0 = "SELECT * FROM 任务表 WHERE " + clause1 + " AND " + clause2 +
                " AND " + clause3 + " AND " + clause4 + " AND " + clause5 + "order by 任务号 asc ";
            MySqlDataReader reader2 = sq.f1(sql0);
            Font oldFont = richTextBox1.SelectionFont;
            Font tiny_font = new Font(oldFont.FontFamily, 8);
            Font title_font = new Font(oldFont.FontFamily, 16, FontStyle.Bold);
            Font id_font = new Font(oldFont.FontFamily, 18);
            Font state_font = new Font(oldFont.FontFamily, 13);
            Font ctt_font = new Font(oldFont.FontFamily, 13);

            ArrayList reader2_arr = new ArrayList();
            while (reader2.Read())
            {
                Dictionary<string, object> dct0 = new Dictionary<string, object>();
                dct0.Add("rowid", reader2["rowid"]);
                dct0.Add("任务号", reader2["任务号"]);
                dct0.Add("任务内容", reader2["任务内容"]);
                dct0.Add("状态", reader2["状态"]);
                dct0.Add("困难程度", reader2["困难程度"]);
                dct0.Add("责任人", reader2["责任人"]);
                dct0.Add("支持方", reader2["支持方"]);
                dct0.Add("预计完成时间", reader2["预计完成时间"]);
                dct0.Add("备注", reader2["备注"]);
                dct0.Add("最新状态的流水号", reader2["最新状态的流水号"]);
                dct0.Add("对应流水号", reader2["对应流水号"]);
                reader2_arr.Add(dct0);
            }
            reader2.Close();

            string[] signs = {"", " ▇ ", "※ ", "# ", "ㅇ ", "· ", "   " };
            Color[] colors = { Color.FromArgb(255, 0, 0), Color.FromArgb(255,90, 0),
                                      Color.FromArgb(255,160, 0),Color.FromArgb(255,160, 80),Color.FromArgb(255,160, 160),
                                      Color.FromArgb(200,200, 200),Color.FromArgb(230,200, 200)};

            foreach (Dictionary<string, object> dct in reader2_arr)
            {
                string id_rw = (string)dct["任务号"];
                richTextBox1.SelectionIndent = 1;
                if (rwh_arr.IndexOf(id_rw) >= 0)
                {
                    richTextBox1.AppendText("\n");
                    //richTextBox1.SelectionColor = Color.White;
                    //richTextBox1.SelectionFont = tiny_font;
                    //string inner_rowid = "0000" + Convert.ToString((int)dct["rowid"]);
                    //richTextBox1.AppendText(" " + inner_rowid.Substring(inner_rowid.Length-4,4));
                    richTextBox1.SelectionColor = Color.DarkGray;
                    richTextBox1.SelectionFont = id_font;
                    richTextBox1.AppendText(" " + id_rw.Substring(2));
                    int count_layers = ms.get_layers(id_rw) ;
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    richTextBox1.SelectionColor = colors[Math.Min(count_layers, 6)];
                    richTextBox1.SelectionFont = title_font;
                    richTextBox1.AppendText("  " +signs[Math.Min(count_layers, 6)] 
                        + (string)dct["任务内容"] + "     ");
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    richTextBox1.SelectionFont = state_font;
                    Dictionary<int, string> state = new Dictionary<int, string>();
                    state.Add(0, "未开始");
                    state.Add(1, "进行中");
                    state.Add(2, "已完成");
                    state.Add(3, "持续性工作");
                    state.Add(4, "已作废");
                    state.Add(991, "已完成");
                    state.Add(992, "待开展");
                    int state_int = (int)dct["状态"];
                    int ref_id = Convert.ToInt32(dct["最新状态的流水号"]);
                    string ref_id_s = (string)dct["对应流水号"];

                    if (state_int == 2)
                    {
                        richTextBox1.SelectionColor = Color.Gray;
                    }
                    else
                    {
                        richTextBox1.SelectionColor = Color.LightSeaGreen;
                    }
                    richTextBox1.AppendText("  " + state[state_int] + "  ");
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    string[] kn = { "容易", "较容易", "一般", "较困难", "困难" };
                    int kn_int = (int)dct["困难程度"];
                    if (kn_int == 0 || kn_int == 1)
                    {
                        richTextBox1.SelectionColor = Color.LightSeaGreen;
                    }
                    else if (kn_int == 2)
                    {
                        richTextBox1.SelectionColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        richTextBox1.SelectionColor = Color.Tomato;
                    }
                    richTextBox1.AppendText(" 【 " + kn[kn_int] + " 】 ");
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    richTextBox1.SelectionFont = ctt_font;
                    string memo;
                    if (checkBox18.Checked)
                    {
                        richTextBox1.SelectionColor = Color.LightGray;
                        memo = "     " + (string)dct["责任人"] + "    "
                        + (string)dct["支持方"] + "    预计时间:" + dct["预计完成时间"]
                        + "    " + ((string)dct["备注"]).Replace("\r", "").Replace("\n", "  ") + " \n";
                    }
                    else
                    {
                        memo = "\n";
                    }

                    richTextBox1.AppendText(memo);
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    richTextBox1.SelectionFont = ctt_font;
                    richTextBox1.SelectionColor = Color.DeepSkyBlue;
                    MySqlDataReader reader3;
                    string clause_1 = "(时间>= " + dateTimePicker3.Value.ToString("yyyyMMdd") + " AND 时间<"
                        + dateTimePicker4.Value.ToString("yyyyMMdd") + ")";
                    string clause_2 = string.Join("%' OR 作业人员 LIKE '%", Regex.Split(textBox11.Text, "、"));
                    clause_2 = "(作业人员 LIKE '%" + clause_2 + "%')";
                    if (comboBox1.SelectedIndex == 0)  //提取全部流水记录
                    {
                        string sql2 = "SELECT * FROM 作业流水记录 WHERE ( rowid in (" + ref_id_s + ") ) AND"
                            + clause_1 + " AND " + clause_2 + "  ORDER BY 时间 ASC";
                        //string sql2 = "SELECT * FROM 作业流水记录 WHERE (任务号 ='" + id_rw + "')  AND"
                        //   + clause_1 + " AND " + clause_2 + "  ORDER BY 时间 ASC";
                        reader3 = sq.f1(sql2);
                        while (reader3.Read())
                        {
                            richTextBox1.SelectionColor = Color.White;
                            richTextBox1.SelectionFont = tiny_font;
                            string inner_rowid = "00000" + Convert.ToString((int)reader3["rowid"]);
                            richTextBox1.AppendText(" " + inner_rowid.Substring(inner_rowid.Length-5,5));
                            string output = "     \t‹ " + state[(int)reader3["作业结果"]]
                                + "   " + reader3["时间"] + "   " + reader3["作业人员"] + " ›     "
                                  + ((string)reader3["内容"]).Replace("\r", "").Replace("\n", "\t") + "\n";
                            richTextBox1.SelectionFont = ctt_font;
                            richTextBox1.SelectionColor = Color.DeepSkyBlue;
                            richTextBox1.AppendText(output);
                            richTextBox1.Select(richTextBox1.Text.Length, 0);
                        }
                        reader3.Close();

                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        string sql2 = "";
                        if (ref_id < 0)
                        {
                            continue;
                        }
                        else
                        {
                            sql2 = "SELECT * FROM 作业流水记录 WHERE  (rowid=" 
                                + Convert.ToString(ref_id) + ") AND " + clause_1 + " AND " + clause_2;
                        }

                        reader3 = sq.f1(sql2);
                        while (reader3.Read())
                        {
                            richTextBox1.SelectionColor = Color.White;
                            richTextBox1.SelectionFont = tiny_font;
                            string inner_rowid = "00000" + Convert.ToString((int)reader3["rowid"]);
                            richTextBox1.AppendText(" " + inner_rowid.Substring(inner_rowid.Length - 5, 5));
                            string output = "     \t‹ " + state[(int)reader3["作业结果"]]
                                + "   " + reader3["时间"] + "   " + reader3["作业人员"] + " ›     "
                                  + ((string)reader3["内容"]).Replace("\r","").Replace("\n", "\t") + "\n";
                            richTextBox1.SelectionIndent = 10;
                            richTextBox1.SelectionFont = ctt_font;
                            richTextBox1.SelectionColor = Color.DeepSkyBlue;
                            richTextBox1.AppendText(output);
                            richTextBox1.Select(richTextBox1.Text.Length, 0);
                        }
                        reader3.Close();

                    }
                    
                    else
                    {
                        // 不用操作
                    }
                }
            }
            
            Form2 form2 = new Form2();
            form2.myrft = richTextBox1.Rtf;
            form2.Show();

        }



        private void button8_Click(object sender, EventArgs e)
        {
            int to_select = listBox1.SelectedIndex;
            listbox_refresh(to_select);
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            button2.Text = "确认修改";
            string nowstr = (string)listBox1.SelectedItem;
            string str_id = nowstr.Split('/')[0].Replace(" ", "");
            MySqlDataReader reader = sq.f1("select * from 任务表 where rowid =" + str_id);
            reader.Read();   //只用读取第一个即可，是倒序排序；
            textBox00.Text = Convert.ToString((int)reader["rowid"]);
            textBox1.Text = (string)reader["任务号"];
            textBox2.Text = (string)reader["任务内容"];
            textBox3.Text = (string)reader["责任人"];
            textBox4.Text = (string)reader["支持方"];
            comboBox2.SelectedIndex = (int)reader["状态"];
            comboBox3.SelectedIndex = (int)reader["困难程度"];
            textBox5.Text = (string)reader["备注"];
            int dtime = Convert.ToInt32(reader["预计完成时间"]);
            int year = Convert.ToInt32(dtime / 10000);
            int month = Convert.ToInt32((dtime - year * 10000) / 100);
            int day = dtime - year * 10000 - month * 100;
            dateTimePicker2.Value = new DateTime(year, month, day);
            textBox14.Text = Convert.ToString(reader["最新状态的流水号"]);
            textBox15.Text = Convert.ToString(reader["对应流水号"]);
            textBox2.Focus();
            reader.Close();
        }


        private void mock_btn_10_click()
        {

            string rwh = (string)listBox1.SelectedItem;
            richTextBox1.Clear();
            listView1.Items.Clear();
            string str_id = rwh.Split('/')[0].Replace(" ", "");
            string nowstr = rwh.Split('/')[1].Replace(" ", "");
            int nowstr_layers = ms.get_layers(nowstr);
            string sql0 = "SELECT *, 预计完成时间 as YJ  FROM 任务表 WHERE rowid =" + str_id;
            MySqlDataReader reader2 = sq.f1(sql0);
            Font oldFont = richTextBox1.SelectionFont;
            Font title_font = new Font(oldFont.FontFamily, 16, FontStyle.Bold);
            Font id_font = new Font(oldFont.FontFamily, 18);
            Font state_font = new Font(oldFont.FontFamily, 13);
            Font ctt_font = new Font(oldFont.FontFamily, 13);
            ArrayList reader2_arr = new ArrayList();
            while (reader2.Read())
            {
                Dictionary<string, object> dct0 = new Dictionary<string, object>();
                dct0.Add("任务号", reader2["任务号"]);
                dct0.Add("任务内容", reader2["任务内容"]);
                dct0.Add("状态", reader2["状态"]);
                dct0.Add("困难程度", reader2["困难程度"]);
                dct0.Add("责任人", reader2["责任人"]);
                dct0.Add("支持方", reader2["支持方"]);
                dct0.Add("YJ", reader2["YJ"]);
                dct0.Add("备注", reader2["备注"]);
                dct0.Add("对应流水号", reader2["对应流水号"]);
                reader2_arr.Add(dct0);
            }
            reader2.Close();
            foreach (Dictionary<string, object> dct in reader2_arr)
            {
                string id_rw = (string)dct["任务号"];
                richTextBox1.SelectionIndent = 1;

                richTextBox1.AppendText("\n");
                richTextBox1.SelectionColor = Color.DarkGray;
                richTextBox1.SelectionFont = id_font;
                richTextBox1.AppendText(" " + id_rw);
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = title_font;
                richTextBox1.AppendText("  " + (string)dct["任务内容"] + "     ");
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                richTextBox1.SelectionFont = state_font;
                Dictionary<int, string> state = new Dictionary<int, string>();
                state.Add(0, "未开始");
                state.Add(1, "进行中");
                state.Add(2, "已完成");
                state.Add(3, "持续性工作");
                state.Add(4, "已作废");
                state.Add(991, "已完成");
                state.Add(992, "待开展");
                int state_int = (int)dct["状态"];
                if (state_int == 2)
                {
                    richTextBox1.SelectionColor = Color.Gray;
                }
                else
                {
                    richTextBox1.SelectionColor = Color.LightSeaGreen;
                }
                richTextBox1.AppendText("  " + state[state_int] + "  ");
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                string[] kn = { "容易", "较容易", "一般", "较困难", "困难" };
                int kn_int = (int)dct["困难程度"];
                if (kn_int == 0 || kn_int == 1)
                {
                    richTextBox1.SelectionColor = Color.LightSeaGreen;
                }
                else if (kn_int == 2)
                {
                    richTextBox1.SelectionColor = Color.SkyBlue;
                }
                else
                {
                    richTextBox1.SelectionColor = Color.Tomato;
                }
                richTextBox1.AppendText(" 【 " + kn[kn_int] + " 】 ");
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                richTextBox1.SelectionFont = ctt_font;
                richTextBox1.SelectionColor = Color.Gray;
                string memo = "     " + (string)dct["责任人"] + "   "
                    + (string)dct["支持方"] + "   " + dct["YJ"]
                    + "   " + ((string)dct["备注"]).Replace("\r", "").Replace("\n", " ") + " \n";
                richTextBox1.AppendText(memo);
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                richTextBox1.SelectionFont = ctt_font;
                richTextBox1.SelectionColor = Color.DimGray;
                string ref_id_s = (string)dct["对应流水号"];

                MySqlDataReader reader3;
                string sql2 = "SELECT * FROM 作业流水记录 WHERE ( rowid in (" + ref_id_s + ") ) ORDER BY 时间 ASC";
                reader3 = sq.f1(sql2);
                while (reader3.Read())
                {
                    string output1 = "     ‹ " + state[(int)reader3["作业结果"]]
                        + "   " + reader3["时间"] + "   " + reader3["作业人员"] + " ›    ";
                    string output2 = reader3["时间"] + " " + reader3["作业人员"];
                    string output3 = ((string)reader3["内容"]).Replace("\n", "\t").Replace("\r", "") + "\n";
                    string output4 = "[" + state[(int)reader3["作业结果"]] + "] " + output3;
                    richTextBox1.SelectionFont = ctt_font;
                    richTextBox1.SelectionColor = Color.DimGray;
                    richTextBox1.AppendText(output1 + output3);
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    string that_datetime = reader3["时间"].ToString();
                    DateTime nowtime = DateTime.Now.Date;
                    TimeSpan sp = nowtime.Subtract(Convert.ToDateTime(that_datetime.Substring(0, 4) + "-"
                         + that_datetime.Substring(4, 2) + "-" + that_datetime.Substring(6, 2)));
                    if (((string)reader3["user"] == Environment.UserName) && sp.Days <= 5)
                    {

                        listView1.Items.Add(new ListViewItem(new string[] { "可删除 [" + reader3["rowid"].ToString() + "]", output2, output4 }));
                    }
                    else
                    {
                        listView1.Items.Add(new ListViewItem(new string[] { " ", output2, output4 }));
                    }
                }
                reader3.Close();
                if (listView1.Items.Count > 1)
                {
                    //listView1.Items[listView1.Items.Count - 1].Selected = true;
                    listView1.EnsureVisible(listView1.Items.Count - 1);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mock_btn_10_click();
            Form2 form2 = new Form2();
            form2.Size = new Size(800, 500);
            form2.myrft = richTextBox1.Rtf;
            form2.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            mock_btn_10_click();
            Form2 form2 = new Form2();
            form2.Size = new Size(800, 500);
            form2.myrft = richTextBox1.Rtf;
            form2.Show();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = checkBox4.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox3.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = checkBox6.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox5.Checked;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox3.SelectedIndex;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox2.SelectedIndex;
            mock_btn_10_click();
            button5_Click(null, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = checkBox1.Checked;
            checkBox6.Checked = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = checkBox2.Checked;
            checkBox5.Checked = checkBox2.Checked;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = listBox1.SelectedIndex;
            listBox3.SelectedIndex = listBox1.SelectedIndex;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            button1_Click(new object(), new EventArgs());

        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            button5_Click(new object(), new EventArgs());

        }

        private void bindingSource1_CurrentChanged_1(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            /*
            Mysql_ sq = new Mysql_();
            MySqlConnection mc = sq.getMySqlCon();
            string sql = "SELECT * FROM `have`";
            MySqlCommand msc = sq.getSqlCommand(sql, mc);
            sq.getResultset(msc);
            */
        }

        private void listBox3_DrawItem(object sender, DrawItemEventArgs e)
        {
                e.DrawBackground();          //先调用基类实现

                if (e.Index < 0)            //form load 的时候return
                    return;

            string full_s = listBox3.Items[e.Index].ToString();
            string[] spl_s = full_s.Split('/');
            Font here_font = e.Font;
            string blank_s = new String(' ', (spl_s[1]).Length);
            e.Graphics.DrawString(blank_s + spl_s[2], here_font, Brushes.Black, e.Bounds);
            e.Graphics.DrawString(spl_s[1], here_font, Brushes.DarkGray, e.Bounds);
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();          //先调用基类实现

            if (e.Index < 0)            //form load 的时候return
                return;

            string full_s = listBox2.Items[e.Index].ToString();
            string[] spl_s = full_s.Split('/');
            Font here_font = e.Font;
            string blank_s = new String(' ', (spl_s[1]).Length);
            e.Graphics.DrawString(blank_s + spl_s[2], here_font, Brushes.Black, e.Bounds);
            e.Graphics.DrawString(spl_s[1], here_font, Brushes.DarkGray, e.Bounds);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();          //先调用基类实现

            if (e.Index < 0)            //form load 的时候return
                return;

            string full_s = listBox1.Items[e.Index].ToString();
            string[] spl_s = full_s.Split('/');
            Font here_font = e.Font;
            string blank_s = new String(' ', (spl_s[1]).Length);
            e.Graphics.DrawString(blank_s + spl_s[2], here_font, Brushes.Black, e.Bounds);
            e.Graphics.DrawString(spl_s[1], here_font, Brushes.DarkGray, e.Bounds);
            // 最前面的rowid不显示
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e) //生成HTML
        {
            string rwh = (string)listBox1.SelectedItem;
            string nowstr = rwh.Split('/')[1].Replace(" ", "");
            int nowstr_layers = ms.get_layers(nowstr);
            MySqlDataReader reader = sq.f1(
                "SELECT rowid, 任务号 FROM 任务表 WHERE 任务号 ='" + nowstr + "' OR 任务号 LIKE'" + nowstr + "-%'  ORDER BY 任务号 ASC ");
            ArrayList rwh_arr = new ArrayList();
            string last_ctt = "nothing";
            while (reader.Read())
            {
                string ctt = (string)reader["任务号"];
                if (radioButton1.Checked)
                {
                    int textbox10_text;
                    try
                    {
                        textbox10_text = Math.Abs(Convert.ToInt32(textBox10.Text));
                    }
                    catch
                    {
                        textbox10_text = 3;
                    }
                    if (ms.get_layers(ctt) <= nowstr_layers + textbox10_text)
                    {
                        rwh_arr.Add(ctt);
                    }
                }
                else if (radioButton2.Checked)
                {
                    rwh_arr.Add(ctt);
                }
                else
                {
                    if (ctt.IndexOf(last_ctt) < 0)
                    {
                        rwh_arr.Add(last_ctt);
                    }
                    last_ctt = ctt;
                }
            }
            reader.Close();
            if (radioButton3.Checked)
            {
                rwh_arr.Add(last_ctt);
                rwh_arr.RemoveAt(0);
            }

            string clause1 = textBox12.Text;
            clause1 = string.Join("%' OR 责任人 LIKE'%", Regex.Split(clause1, "、"));
            clause1 = "(责任人 LIKE'%" + clause1 + "%')";
            string clause2 = textBox13.Text;
            clause2 = string.Join("%' OR 支持方 LIKE'", Regex.Split(clause2, "、"));
            clause2 = "(支持方 LIKE'%" + clause2 + "%')";
            string clause3;
            clause3 = "(预计完成时间>=" + dateTimePicker5.Value.ToString("yyyyMMdd")
                + " AND 预计完成时间<=" + dateTimePicker6.Value.ToString("yyyyMMdd") + ") ";
            string clause4_1 = checkBox7.Checked ? "状态=0" : "状态=999";
            string clause4_2 = checkBox8.Checked ? "状态=1" : "状态=999";
            string clause4_3 = checkBox9.Checked ? "状态=2" : "状态=999";
            string clause4_4 = checkBox10.Checked ? "状态=3" : "状态=999";
            string clause4_5 = checkBox11.Checked ? "状态=4" : "状态=999";
            string clause4 = "(" + clause4_1 + " OR " + clause4_2 + " OR " + clause4_3 + " OR "
                + clause4_4 + " OR " + clause4_5 + ")";
            string clause5_1 = checkBox12.Checked ? "困难程度=0" : "困难程度=999";
            string clause5_2 = checkBox13.Checked ? "困难程度=1" : "困难程度=999";
            string clause5_3 = checkBox14.Checked ? "困难程度=2" : "困难程度=999";
            string clause5_4 = checkBox15.Checked ? "困难程度=3" : "困难程度=999";
            string clause5_5 = checkBox16.Checked ? "困难程度=4" : "困难程度=999";
            string clause5 = "(" + clause5_1 + " OR " + clause5_2 + " OR " + clause5_3 + " OR "
                + clause5_4 + " OR " + clause5_5 + ")";
            string sql0 = "SELECT * FROM 任务表 WHERE " + clause1 + " AND " + clause2 +
                " AND " + clause3 + " AND " + clause4 + " AND " + clause5 + "order by 任务号 asc ";
            MySqlDataReader reader2 = sq.f1(sql0);
            ArrayList reader2_arr = new ArrayList();
            while (reader2.Read())
            {
                Dictionary<string, object> dct0 = new Dictionary<string, object>();
                dct0.Add("rowid", reader2["rowid"]);
                dct0.Add("任务号", reader2["任务号"]);
                dct0.Add("任务内容", reader2["任务内容"]);
                dct0.Add("状态", reader2["状态"]);
                dct0.Add("困难程度", reader2["困难程度"]);
                dct0.Add("责任人", reader2["责任人"]);
                dct0.Add("支持方", reader2["支持方"]);
                dct0.Add("预计完成时间", reader2["预计完成时间"]);
                dct0.Add("备注", reader2["备注"]);
                dct0.Add("最新状态的流水号", reader2["最新状态的流水号"]);
                dct0.Add("对应流水号", reader2["对应流水号"]);
                reader2_arr.Add(dct0);
            }
            reader2.Close();

            //string[] signs = { "", " ▇ ", "※ ", "# ", "ㅇ ", "· ", "   " };
            string[] signs = { "▇▇ ", " ▇ ", "", "", "", "", "" };
            Color[] colors = { Color.FromArgb(255, 0, 0), Color.FromArgb(255,90, 0),
                                      Color.FromArgb(255,160, 0),Color.FromArgb(255,160, 80),Color.FromArgb(255,160, 160),
                                      Color.FromArgb(200,200, 200),Color.FromArgb(230,200, 200)};

            StreamWriter sw = new StreamWriter("generated_htm.html");
            sw.Write(textBox_html.Text);
            foreach (Dictionary<string, object> dct in reader2_arr)
            {
                string id_rw = (string)dct["任务号"];
                if (rwh_arr.IndexOf(id_rw) >= 0)
                {
                    int count_layers = ms.get_layers(id_rw);
                    sw.Write("\n\n<br>");
                    sw.Write("<table id=\"TB1\" style=\"table - layout:fixed\" " +
                        "width=100%><colgroup><col width=40% /><col width=60%/></colgroup>" +
                        "<tr><td><table style=\"table - layout:fixed\" width=100%><colgroup>" +
                        "<col width=" +Convert.ToString(20+5*count_layers) +"% />" +
                        "<col width=" + Convert.ToString(Math.Max(80 - 5,5) * count_layers) + "%/></colgroup><tr><td>" +
                        "<p id=A>"+ id_rw + "</p></td>\n");

                    sw.Write("<td><p id=B"+ Convert.ToString(Math.Min(6, count_layers)) +">"
                        + signs[Math.Min(count_layers, 6)]
                        + (string)dct["任务内容"] + "</p></td></tr></table></td>\n");
                    Dictionary<int, string> state = new Dictionary<int, string>();
                    state.Add(0, "未开始");
                    state.Add(1, "进行中");
                    state.Add(2, "已完成");
                    state.Add(3, "持续性工作");
                    state.Add(4, "已作废");
                    state.Add(991, "已完成");
                    state.Add(992, "待开展");
                    int state_int = (int)dct["状态"];
                    int ref_id = Convert.ToInt32(dct["最新状态的流水号"]);
                    string ref_id_s = (string)dct["对应流水号"];
                    sw.Write("<td><table  style = \"table-layout:fixed\" width " +
                        "= 100%><colgroup ><col width = 10%/><col width = 15%/><col width" +
                        " = 20%/><col width = 30%/><col width = 25%/></colgroup ><tr><td>" +
                        "<p id = C > " + state[state_int] + " </p ></td > \n");
                    string[] kn = { "容易", "较容易", "一般", "较困难", "困难" };
                    int kn_int = (int)dct["困难程度"];
                    if (kn_int == 0 || kn_int == 1)
                    {
                        sw.Write("<td><p id=D1>["+ kn[kn_int]+"]</p></td>");
                    }
                    else if (kn_int == 2)
                    {
                        sw.Write("<td><p id=D2>[" + kn[kn_int] + "]</p></td>");
                    }
                    else
                    {
                        sw.Write("<td><p id=D3>[" + kn[kn_int] + "]</p></td>");
                    }
                    if (checkBox18.Checked)
                    {
                        sw.Write("<td ><p id = E >" + (string)dct["责任人"] + "</p ></td >"
                                + "<td ><p id = E > " + (string)dct["支持方"] + "</p ></td ><td ><p id = E >预期时间:"
                                + dct["预计完成时间"] + "</p></td></tr><tr><table><tr><td><p id=F>"
                                + ((string)dct["备注"]).Replace("\r", "").Replace("\n", "&nbsp") + "</p></tr></td></table>"
                                + "</tr></table ></td ></tr ></table>\n");
                    }
                    else
                    {
                        sw.Write("</tr></table ></td ></tr ></table >\n");
                    }

                    MySqlDataReader reader3;
                    string clause_1 = "(时间>= " + dateTimePicker3.Value.ToString("yyyyMMdd") + " AND 时间<"
                        + dateTimePicker4.Value.ToString("yyyyMMdd") + ")";
                    string clause_2 = string.Join("%' OR 作业人员 LIKE '%", Regex.Split(textBox11.Text, "、"));
                    clause_2 = "(作业人员 LIKE '%" + clause_2 + "%')";
                    if (comboBox1.SelectedIndex == 0)  //提取全部流水记录
                    {
                        string sql2 = "SELECT * FROM 作业流水记录 WHERE ( rowid in (" + ref_id_s + ") ) AND"
                            + clause_1 + " AND " + clause_2 + "  ORDER BY 时间 ASC";
                        //string sql2 = "SELECT * FROM 作业流水记录 WHERE (任务号 ='" + id_rw + "')  AND"
                        //   + clause_1 + " AND " + clause_2 + "  ORDER BY 时间 ASC";
                        reader3 = sq.f1(sql2);
                        bool put_head = false;
                        while (reader3.Read())
                        {
                            if (!put_head)
                            {
                                sw.Write("<table id=TB2 style=\"table - layout:fixed\" width=100%><tr><td>\n");
                                put_head = true;
                            }
                            string inner_rowid = "00000" + Convert.ToString((int)reader3["rowid"]);
                            sw.Write("<table style=\"table - layout:fixed\" width=100%>"
                                     + "<colgroup ><col width = 10% ><col width = 25%><col width = 65%></colgroup >"
                                     + "<tr ><td ><p id = H >" + inner_rowid.Substring(inner_rowid.Length - 5, 5)
                                     + "</td >\n");
                            sw.Write("<td><p id=G>‹&nbsp" + state[(int)reader3["作业结果"]]
                                + "&nbsp&nbsp" + reader3["时间"] + "&nbsp&nbsp" + reader3["作业人员"] + "&nbsp›</p></td>\n");
                            sw.Write("<td><p id=G>" + ((string)reader3["内容"]).Replace("\r", "").Replace("\n", "&nbsp")
                                + " </p></td></tr></table >");
                        }
                        reader3.Close();
                        if (put_head)
                        {
                            sw.Write("</td></tr></table>");
                        }

                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        string sql2 = "";
                        if (ref_id < 0)
                        {
                            continue;
                        }
                        else
                        {
                            sql2 = "SELECT * FROM 作业流水记录 WHERE  (rowid="
                                + Convert.ToString(ref_id) + ") AND " + clause_1 + " AND " + clause_2;
                        }

                        reader3 = sq.f1(sql2);
                        bool put_head = false;
                        while (reader3.Read())
                        {
                            if (!put_head)
                            {
                                sw.Write("<table id=TB2 style=\"table - layout:fixed\" width=100%><tr><td>");
                                put_head = true;
                            }
                            string inner_rowid = "00000" + Convert.ToString((int)reader3["rowid"]);
                            sw.Write("<table style=\"table - layout:fixed\" width=100%>"
                                     + "<colgroup ><col width = 10% ><col width = 25%><col width = 65%></colgroup >"
                                     + "<tr ><td ><p id = H >" + inner_rowid.Substring(inner_rowid.Length - 5, 5)
                                     + "</td >\n");
                            sw.Write("<td><p id=G>‹&nbsp" + state[(int)reader3["作业结果"]]
                                + "&nbsp&nbsp" + reader3["时间"] + "&nbsp&nbsp" + reader3["作业人员"] + "&nbsp›</p></td>\n");
                            sw.Write("<td><p id=G>" + ((string)reader3["内容"]).Replace("\r", "").Replace("\n", "&nbsp")
                                + " </p></td></tr></table >");
                        }
                        reader3.Close();
                        if (put_head)
                        {
                            sw.Write("</td></tr></table>");
                        }
                    }

                    else
                    {
                        // 不用操作
                    }
                }
            }

            sw.Close();
            Form form3 = new Form3();
            form3.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex >= 0)
            {
                string now_str = (string)(listBox4.Items[listBox4.SelectedIndex]);
                int mb_id = Convert.ToInt32(now_str.Substring(1, 3));
                listBox4.Items[listBox4.SelectedIndex] = now_str.Substring(0, 9) + textBox17.Text;
                string sql = "UPDATE 输出模板 SET 名称 = '" + textBox17.Text + "' WHERE id=" + Convert.ToString(mb_id); ;
                sq.f2(sql);

                listBox4.Items.Clear();
                MySqlDataReader reader2 = sq.f1("select id,名称 from 输出模板  order by id");
                while (reader2.Read())
                {
                    string str1 = Convert.ToString((int)reader2["id"]);
                    string str2 = (string)reader2["名称"];
                    string combo_str = "(" + str1 + ")    " + str2;
                    listBox4.Items.Add(combo_str);
                }
                reader2.Close();

            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
       
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex >= 0)
            {
                // 刷新
                string now_str = (string)(listBox4.Items[listBox4.SelectedIndex]);
                int mb_id = Convert.ToInt32(now_str.Substring(1, 3));
                MySqlDataReader reader2 = sq.f1("select 内容 from 输出模板 where id=" + Convert.ToString(mb_id));
                reader2.Read();
                textBox16.Text = (string)reader2["内容"];
                reader2.Close();

                textBox17.Text = now_str.Substring(9);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex >= 0)
            {
                string now_str = (string)(listBox4.Items[listBox4.SelectedIndex]);
                int mb_id = Convert.ToInt32(now_str.Substring(1, 3));
                string sql = "UPDATE 输出模板 SET 内容 = '" + textBox16.Text + "' WHERE id=" + Convert.ToString(mb_id);
                sq.f2(sql);
                panel4.Visible = true;
                timer1.Enabled = true;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
        }

        private void textBox17_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox17.Text[0] == '(')
                {
                    textBox17.Text = "";
                }
            }
            catch
            {

            }
        }

        private void button21_Click(object sender, EventArgs e)
        {

            if (listBox4.SelectedIndex >= 0)
            {
                string now_str = (string)(listBox4.Items[listBox4.SelectedIndex]);
                int mb_id = Convert.ToInt32(now_str.Substring(1, 3));
                string sql = "UPDATE 输出模板 SET 内容 = '" + textBox16.Text + "' WHERE id=" + Convert.ToString(mb_id);
                sq.f2(sql);
                panel4.Visible = true;
                timer1.Enabled = true;
            }

            string s = textBox16.Text;
            List<int> symbol_left_list = new List<int>();
            List<int> symbol_right_list = new List<int>();
            List<string> to_replace_list = new List<string>();
            int i1 = s.IndexOf("[");
            int cumulative_cut_length = 0;
            while (i1 > -1)
            {
                symbol_left_list.Add(cumulative_cut_length+ i1);
                cumulative_cut_length += i1 + 1;
                s = s.Substring(i1 + 1);
                i1 = s.IndexOf("[");
            }
            s = textBox16.Text;
            int i2 = s.IndexOf("]");
            cumulative_cut_length = 0;
            while (i2 > -1)
            {
                symbol_right_list.Add(cumulative_cut_length + i2);
                cumulative_cut_length += i2 + 1;
                s = s.Substring(i2 + 1);
                i2 = s.IndexOf("]");
            }
            if (symbol_left_list.Count != symbol_right_list.Count)
            {
                MessageBox.Show("左右方括号不匹配！");
            }
            else
            {
                for (int i_symbol = 0; i_symbol < symbol_left_list.Count; i_symbol++)
                {
                    string symbol_content = textBox16.Text.Substring(symbol_left_list[i_symbol] + 1,
                        symbol_right_list[i_symbol] - 1 - symbol_left_list[i_symbol]);

                    // 先搜寻任务表
                    string sql0 = "";
                    if (symbol_content.Split('/')[2] == "0")
                        sql0 = "SELECT * FROM 任务表 WHERE 状态<> 4 AND 任务号 LIKE '" + symbol_content.Split('/')[0] + "%' ";
                    else
                        sql0 = "SELECT * FROM 任务表 WHERE 状态<> 4 AND 任务号 = '" + symbol_content.Split('/')[0] + "' ";

                    MySqlDataReader reader_0 = sq.f1(sql0);
                    List<string> lsh = new List<string>();
                    while (reader_0.Read())
                    {
                        string ctt = (string)reader_0["对应流水号"];
                        lsh.Add(ctt);
                    }
                    reader_0.Close();

                    // 再搜索作业表
                    string clause_1 = "(时间>= " + dateTimePicker8.Value.ToString("yyyyMMdd") + " AND 时间<"
                           + dateTimePicker7.Value.ToString("yyyyMMdd") + ")";
                    string clause_2;
                    if (symbol_content.Split('/')[1] == "A")
                    {
                        clause_2 = "(作业结果 = 991)"; 
                    }
                    else
                    {
                        clause_2 = "(作业结果 = 992)";
                    }

                    string concat_str = "";
                    int xh_ = 0;
                    for (int i_lsh = 0; i_lsh < lsh.Count; i_lsh++)
                    {
                        string ref_id_s = lsh[i_lsh];
                        string sql = "SELECT * FROM 作业流水记录 WHERE ( rowid in (" + ref_id_s + ") ) AND"
                               + clause_1 + " AND " + clause_2 + "  ORDER BY 时间 ASC";
                        MySqlDataReader reader = sq.f1(sql);
                        while (reader.Read())
                        {
                            string ctt = ((string)reader["内容"]).Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                            string zyr = ((string)reader["作业人员"]).Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", ""); ;
                            if (ctt.Length > 0)
                            {
                                xh_++;
                                if (checkBox19.Checked)
                                {
                                    if (checkBox20.Checked)
                                    {
                                        concat_str = concat_str + Convert.ToString(xh_) + ". " + ctt + "（" + zyr + "）" + "\r\n";
                                    }
                                    else
                                    {
                                        concat_str = concat_str + ctt + "（" + zyr + "）" + "\r\n";
                                    }
                                }
                                else
                                {
                                    if (checkBox20.Checked)
                                    {
                                        concat_str = concat_str + Convert.ToString(xh_) + ". " + ctt + "；\r\n";
                                    }
                                    else
                                    {
                                        concat_str = concat_str + ctt + "；\r\n";
                                    }
                                }
                            }
                        }
                        reader.Close();
                    }
                    if (concat_str.Length >= 1)
                    {
                        to_replace_list.Add(concat_str);
                    }
                    else
                    {
                        to_replace_list.Add("");
                    }
                }

                // 放入textBox18
                string temp_string = "";
                int last_p = 0;
                for (int i_sym=0; i_sym < symbol_left_list.Count ; i_sym++)
                {
                    int p = symbol_left_list[i_sym];
                    temp_string += textBox16.Text.Substring(last_p, p - last_p);
                    temp_string += to_replace_list[i_sym];
                    last_p = symbol_right_list[i_sym] + 1;
                }
                temp_string += textBox16.Text.Substring(last_p);
                textBox18.Text = temp_string;

                Form4 form4 = new Form4();
                form4.textBox1.Text = textBox18.Text;
                form4.Show();

            }



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string help;
            help = "请按如下规则使用通配符\r\n（1） 通配符形如“[任务号/状态标识/匹配方式]”\r\n"
                + "（2）状态标识为A时，输出已完成作业，状态标识为B时，输出准备进行的作业\r\n"
                + "（3）匹配方式为0时，输出该任务及其子任务，匹配方式为1时，只输出该任务本身\r\n";
            MessageBox.Show(help, "帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MySqlDataReader reader_0 = sq.f1("SELECT pwd FROM info");
            reader_0.Read();
            string true_pwd = (string)reader_0["pwd"];
            reader_0.Close();
            if (true_pwd == textBox19.Text)
            {
                panel3.Enabled = true;
            }
            else
            {
                MessageBox.Show("密码错误！");
            }
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox20_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox20.Text[0] == '(')
                {
                    textBox20.Text = "";
                }
            }
            catch { }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Convert.ToInt32(textBox20.Text)) != textBox20.Text)
            {
                MessageBox.Show("作业序号输入错误");
            }
            else
            {
                MySqlDataReader reader_0 =
                    sq.f1("SELECT 内容 FROM 作业流水记录 WHERE rowid=" + textBox20.Text);
                reader_0.Read();
                string contt = (string)reader_0["内容"];
                reader_0.Close();
                if (MessageBox.Show("是否删除该记录:\r\n" + contt, "", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    sq.f2("DELETE FROM 作业流水记录 WHERE rowid=" + textBox20.Text);
                    MessageBox.Show("已删除");
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel4.Visible = false;
            timer1.Enabled = false;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if ((listView1.SelectedItems[0].SubItems[0].Text).Split(' ')[0] == "可删除")
            {
                if (MessageBox.Show("是否删除如下内容：\r\n\r\n" +
                    (listView1.SelectedItems[0].SubItems[1].Text + " " + listView1.SelectedItems[0].SubItems[2].Text).Trim(), ""
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    sq.f2("DELETE FROM 作业流水记录 WHERE rowid=" + (listView1.SelectedItems[0]
                        .SubItems[0].Text).Split(' ')[1].Replace('[', ' ').Replace(']', ' '));
                    MessageBox.Show("已删除");
                    mock_btn_10_click();
                }
                
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                textBox6.Text = listView1.SelectedItems[0].SubItems[2].Text.Split(']')[1].Trim();
            }
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.Text = "添加至：" + textBox8.Text;
            button6.ForeColor = Color.Red;
        }

        private void tabPage2_MouseHover(object sender, EventArgs e)
        {

        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.ForeColor = Color.Black;
            button6.Text = "确认添加";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                textBox6.SelectAll();
            }
        }

        private void listBox2_Click(object sender, EventArgs e)
        {
            //button5_Click(new object(), new EventArgs());
        }
    }
}


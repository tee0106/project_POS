using System;
using System.Linq;
using System.Windows.Forms;
using static WindowsFormsApp1.Database;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Database database = new Database();
        string number = "1";
        int total = 0;
        string item_id = "";

        public Form1()
        {
            InitializeComponent();
            database.Connect();
        }

        //datas
        private int needtopay = 0;                          //需付
        private int paidnum = 0;                            //已付
        private int changenum = 0;                          //找續

        //funt.
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
            button5.Text = "刪除";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            panel3.BringToFront();
            button5.Text = "退貨";
            string sql = "Select * FROM record";
            if (database.ConnectionOpen())
            {
                MySqlDataReader data = database.GetData(sql);
                while (data.Read())
                {
                    string barcode = data.GetString("barcode");
                    string total = data.GetString("total");
                    string payment = data.GetString("payment");
                    string datetime = data.GetString("datetime");
                    listBox1.Items.Add(barcode + "      " + datetime.Split(' ')[0]);
                }
                database.ConnectionClose();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            panel4.BringToFront();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains('*'))
            {
                String[] muiltNum = textBox1.Text.Split('*');
                if (muiltNum[0] != "")
                {
                    number = muiltNum[0];
                    label3.Text = "x" + muiltNum[0] ;
                }
                else
                {
                    label3.Text = "";
                }
                textBox1.Text = "";
            }

        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paidnum = 0;
                changenum = 0;
                if (database.ConnectionOpen())
                {
                    string sql = "SELECT * FROM goods WHERE barcode = " + textBox1.Text;
                    MySqlDataReader data = database.GetData(sql);
                    if (data.Read())
                    {
                        string barcode = data.GetString("barcode");
                        string name = data.GetString("name");
                        string price = data.GetString("price");
                        int itemTotal = Int32.Parse(price) * Int32.Parse(number);
                        DataGridViewRowCollection rows = dataGridView1.Rows;
                        rows.Add(new Object[] { barcode, name, number, price, itemTotal });
                        total += itemTotal;
                        label7.Text = total.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Input Error!");
                    }
                    database.ConnectionClose();
                }
                else
                {
                    MessageBox.Show("Connect Error!");
                }
                textBox1.Text = "";
                number = "1";
                label3.Text = "";
            }
        }


        private void Button4_Click(object sender, EventArgs e)
        {
            if (total == 0)
                return;
            needtopay = total;
            Form2 payment = new Form2();
            payment.setdatas(needtopay);
            payment.Owner = this;
            payment.Show();
            this.Enabled = false;
        }


        public void getchanges(int change, int paid)
        {
            changenum = change;
            paidnum = paid;
            label5.Text = "實收 : " + paidnum;
            label6.Text = "找續 : " + changenum;
            this.Enabled = true;
            addItemToDatabase();
            //reset
            textBox1.Text = "";
            number = "1";
            label3.Text = "";
            total = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(button5.Text == "刪除")
            {
                if (total == 0)
                    return;

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int itemtotal = Int32.Parse(row.Cells["總價"].Value.ToString());
                    total -= itemtotal;
                    label7.Text = total.ToString();
                    dataGridView1.Rows.Remove(row);
                }
            }
            else if(button5.Text == "退貨")
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    string name = row.Cells["商品1"].Value.ToString();
                    if (database.ConnectionOpen())
                    {
                        string sql = $"Update record_goods SET isReturn = 1 WHERE name = '{name}' AND record_id = '{item_id}'";
                        int n = database.UpdateData(sql);
                        if (n > 0)
                            MessageBox.Show("Success!");
                        database.ConnectionClose();
                        string curItem = listBox1.SelectedItem.ToString();
                        int index = listBox1.FindString(curItem);
                        listBox1.SetSelected(index, true);
                        
                    }
                }
            }
        }

        private void addItemToDatabase()
        {
            string recordCode = "";
            long insertNum;
            string sql;
            Random random = new Random();
            for (int i = 0; i < 13; i++)
            {
                recordCode += random.Next(10);
            }
            Console.WriteLine(recordCode);
            if (database.ConnectionOpen())
            {
                sql = $"INSERT INTO record(barcode, total, payment) VALUES ('{recordCode}', '{total}', '{paidnum}')";
                insertNum = database.InsertData(sql);
                Console.WriteLine(insertNum);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        string barcode = row.Cells["條碼"].Value.ToString();
                        string name = row.Cells["商品"].Value.ToString();
                        string number = row.Cells["數量"].Value.ToString();
                        string price = row.Cells["單價"].Value.ToString();
                        string priceTotal = row.Cells["總價"].Value.ToString();
                        Console.WriteLine(barcode + name + number + price + priceTotal);

                        sql = "INSERT INTO record_goods(record_id, name, price, number, total, barcode) VALUES " +
                              $"('{insertNum}', '{name}', '{price}', '{number}', '{priceTotal}', '{barcode}')";
                        database.InsertData(sql);
                    }
                    catch
                    {

                    }
                }
                database.ConnectionClose();
            }
            }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            string total = "";
            string payment = "";
            MySqlDataReader data;
            if (database.ConnectionOpen())
            {
                string select_barcode = listBox1.GetItemText(listBox1.SelectedItem).Split(' ')[0];
                Console.WriteLine(select_barcode);
                string sql = "Select * FROM record WHERE barcode = " + select_barcode;
                data = database.GetData(sql);
                if (data.Read())
                {
                    item_id = data.GetString("id");
                    total = data.GetString("total");
                    payment = data.GetString("payment");
                }
                database.ConnectionClose();
            }
            if (database.ConnectionOpen())
            {
                string sql = "Select * FROM record_goods WHERE record_id = " + item_id;
                data = database.GetData(sql);
                while (data.Read())
                {
                    string name = data.GetString("name");
                    string price = data.GetString("price");
                    string number = data.GetString("number");
                    string item_total = data.GetString("total");
                    string isReturn = data.GetString("isReturn");
                    DataGridViewRowCollection rows = dataGridView2.Rows;
                    rows.Add(new Object[] { name, number, price, item_total });
                    Console.WriteLine(isReturn);
                    if (isReturn == "True")
                    {
                        Console.WriteLine("FINDDD");
                        rows[dataGridView2.Rows.Count - 2].DefaultCellStyle.BackColor = Color.Crimson;
                    }
                }
                label1.Text = total;
                label10.Text = payment;
                database.ConnectionClose();
            }
        }
    }
}

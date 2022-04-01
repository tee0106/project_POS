using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        //datas
        private int needtopay = 0;                          //需付
        private int paidnum = 0;                            //已付
        private int changenum = 0;

        //funt.

        private void Form2_Load(object sender, EventArgs e)
        {
        }



        private void Button1_Click(object sender, EventArgs e)      //check
        {
            //debug
            finish();
        }

        public void setdatas(int pays)
        {
            needtopay = pays;
            label1.Text += needtopay;
        }                           //setdatas


        

        private void Button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += 1;
        }   //button 1

        private void Button9_Click(object sender, EventArgs e)      //button 2
        {
            textBox1.Text += 2;
        }

        private void Button10_Click(object sender, EventArgs e)     //button 3
        {
            textBox1.Text += 3;
        }

        private void Button5_Click(object sender, EventArgs e)     //button 4
        {
            textBox1.Text += 4;
        }

        private void Button6_Click(object sender, EventArgs e)     //button 5
        {
            textBox1.Text += 5;
        }

        private void Button7_Click(object sender, EventArgs e)     //button 6
        {
            textBox1.Text += 6;
        }

        private void Button2_Click(object sender, EventArgs e)     //button 7
        {
            textBox1.Text += 7;
        }

        private void Button3_Click(object sender, EventArgs e)     //button 8
        {
            textBox1.Text += 8;
        }

        private void Button4_Click(object sender, EventArgs e)     //button 9
        {
            textBox1.Text += 9;
        }

        private void Button11_Click(object sender, EventArgs e)     //button 0
        {
            textBox1.Text += 0;
        }


        private void Button15_Click(object sender, EventArgs e)     //button .
        {

        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                finish();
            }
        }  //enterkey

        private void finish()
        {
            if (Convert.ToInt32(textBox1.Text) < needtopay)
            {
                MessageBox.Show("Input Error!");
                textBox1.Text = "";
                return;
            }
            paidnum = Convert.ToInt32(textBox1.Text);
            changenum = paidnum - needtopay;
            MessageBox.Show("共付 : " + paidnum + "\n找回: " + changenum);
            Form1 lForm1 = (Form1)this.Owner;
            lForm1.getchanges(changenum, paidnum);
            this.Close();
        }                                          //return

    }
}


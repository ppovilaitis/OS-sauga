using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace sifravimas2
{
    public partial class Form1 : Form
    { 
        public Form1(Rrg _rrg)
        {
            InitializeComponent();
            window = _rrg;
        }

        public Rrg window;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2(window);
            F2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!textBox2.Text.Contains(' ')) {
                if (!checkIfWritten())
                {
                    SlaptazodiuSifravimas pwSifravimas = new SlaptazodiuSifravimas();
                    File.AppendAllText(@"TheList.txt", textBox1.Text.Replace(' ', '_') + " " + pwSifravimas.EncryptPlainTextToCipherText(textBox2.Text, window.masterpassword) + "\n");
                }
                else
                {
                    MessageBox.Show("Jau buvo ivestas sios programos pavadinimas!");
                }
            }
            else
            {
                MessageBox.Show("Slaptazodis negali tureti tarpu");
            }
        }

        //tikrina ar ivestas pavadinimas jau nebuvo ivestas pries tai
        private bool checkIfWritten()
        {
            
            for(int i = 0; i < File.ReadAllLines(@"TheList.txt").Length; i++)
            {
                
                if(File.ReadAllLines(@"TheList.txt")[i].Split(' ')[0] == textBox1.Text.Replace(' ','_'))
                {
                    return true;
                }
                
            }
            return false;
        }
    }
}

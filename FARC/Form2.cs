using FARC2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FARC
{
    public partial class Form2 : Form
    {
        string tx1, tx2;
        FS filesys = new FS();
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            { 
                textBox1.Text = tx1;
            }
            tx1 = textBox1.Text;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox2.Text))
            {
                textBox2.Text = tx2;
            }
            tx2 = textBox2.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox2.Text))
            {
                if (File.Exists(textBox1.Text))
                {
                    this.Hide();
                    MessageBox.Show("Extracting media, please wait...");
                    filesys.extractmedia(textBox1.Text, textBox2.Text);
                    this.Hide();
                    MessageBox.Show("Media extracted");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("File not found");
                }
            }
            else
            {
                MessageBox.Show("Directory not found");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = Environment.GetEnvironmentVariable("USERPROFILE") + "\\media.farc";
            textBox2.Text = Environment.GetEnvironmentVariable("USERPROFILE");
        }
    }
}

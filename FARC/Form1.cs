using FARC2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace FARC
{
    public partial class Form1 : Form
    {
        string otz;
        static char[] reservedch = new char[] //reserved characters
        {
            '<','>',':','"','/','\\','|','?','*'
        };
        static string[] reservedfn = new string[]   //reserved filenames
  {
        "con",
        "prn",
        "aux",
        "nul",
        "com1",
        "com2",
        "com3",
        "com4",
        "com5",
        "com6",
        "com7",
        "com8",
        "com9",
        "lpt1",
        "lpt2",
        "lpt3",
        "lpt4",
        "lpt5",
        "lpt6",
        "lpt7",
        "lpt8",
        "lpt9",
        "clock$"
  };
        public static bool IsReservedFileName(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileName = fileName.ToLower();
                if (reservedfn.Contains(fileName))
                {
                    return true;
                }
            return false;
        }
        bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("["
                  + Regex.Escape(Path.GetInvalidPathChars().ToString()) + "]");
            if (containsABadCharacter.IsMatch(testName)) { return false; };
            return true;
        }
        string otxs,otys;
        FS filesys = new FS();
        List<string> adr = new List<string>();
        List<string> bdr = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Environment.GetEnvironmentVariable("USERPROFILE");  //sets the initial folder as the user folder
            textBox2.Text = Environment.GetEnvironmentVariable("USERPROFILE");  //sets the initial folder as the user folder

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                textBox1.Text = textBox1.Text + "\\" + listBox1.SelectedItem.ToString();
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            adr.RemoveAt(adr.Count - 1);
            textBox1.Text = String.Join("\\", adr.ToArray());

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains("\\"))
            {
                textBox1.Text = otxs;
            }
            
            if (Directory.Exists(textBox1.Text))
            {
                listBox1.Items.Clear();
                try
                {
                    foreach (string item in Directory.GetDirectories(textBox1.Text))
                    {
                        listBox1.Items.Add(item.Split('\\').Last());
                    }
                    adr = textBox1.Text.Split('\\').ToList();
                }
                catch (Exception)
                {

                    textBox1.Text = otxs;
                }

            }
            else
            {
                listBox1.Items.Clear();
            }
            otxs = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!IsValidFilename(textBox2.Text))
            {
                textBox2.Text = otys;
            }

            if (Directory.Exists(textBox2.Text))
            {
                listBox2.Items.Clear();
                try
                {
                    foreach (string item in Directory.GetDirectories(textBox2.Text))
                    {
                        listBox2.Items.Add(item.Split('\\').Last());
                    }
                    foreach (string item in Directory.GetFiles(textBox2.Text))
                    {
                        listBox2.Items.Add(item.Split('\\').Last());
                    }
                    bdr = textBox2.Text.Split('\\').ToList();
                }
                catch (Exception)
                {

                    textBox2.Text = otys;
                }

            }
            else
            {
                if (File.Exists(textBox2.Text))
                {
                    textBox3.Text = textBox2.Text.Split('\\').Last();
                }
                textBox2.Text = otys;
            }
            otys = textBox2.Text;
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            bdr.RemoveAt(bdr.Count - 1);
            textBox2.Text = String.Join("\\", bdr.ToArray());
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (IsReservedFileName(textBox3.Text))
            {
                textBox3.Text = otz;
            }
            foreach (char itm in reservedch)
            {
                if (textBox3.Text.Contains(itm))
                {
                    textBox3.Text = otz;
                }
            }
            otz = textBox3.Text;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1)
            {
                textBox2.Text = textBox2.Text + "\\" + listBox2.SelectedItem.ToString();
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            filesys.createmedia(textBox2.Text + "\\" + textBox3.Text,textBox1.Text);
            this.Close();
        }
    }
}

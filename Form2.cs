using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWJX
{
    public partial class Form2 : Form
    {
        public string myrft;
        public Form2()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Rtf = myrft;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            richTextBox1.Size = new Size(Size.Width - 15, Size.Height - 65);
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "作战图 " + DateTime.Now.ToString("yyyyMMdd") + ".doc";
            saveFileDialog1.ShowDialog();
            richTextBox1.SaveFile(saveFileDialog1.FileName);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}

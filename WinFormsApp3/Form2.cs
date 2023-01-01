using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp3
{
    public partial class Form2 : Form
    {



        public ListBox lb { get; set; }
        public CheckBox cb { get; set; }
        public int form2sel = -1;
        
        
        
        public Form2()
        {
            InitializeComponent();
            foreach(preset p in preset.presets)
            {
                listBox1.Items.Add(p.name);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            form2sel = listBox1.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form2sel = -1;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) // 구성 추가
        {
            if(lb.Items.Count == 0)
            {
                MessageBox.Show("팀 구성이 비어있습니다.","오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            Form3 form3 = new Form3();
            form3.ShowDialog();
            if(form3.name == "")
            {
                return;
            }
            preset np = new preset(form3.name, lb.Items.Cast<String>().ToList(), cb.Checked);
            preset.presets.Add(np);
            listBox1.Items.Add(form3.name);
            StreamWriter sw = new StreamWriter("presets.txt",true);
            preset.Save(sw, np);
            sw.Close();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(form2sel != -1)
            {
                Close();
            }
        }

        private void button4_Click(object sender, EventArgs e) // 구성 제거
        {
            int t = listBox1.SelectedIndex;
            if(t != -1 && t != 0 && t != 1)
            {
                preset.presets.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                preset.SaveAll();
            }
        }
    }
}

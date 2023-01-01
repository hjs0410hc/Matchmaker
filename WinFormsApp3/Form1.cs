namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                FileStream fs = File.Open("presets.txt", FileMode.CreateNew);
                //default presets.
                preset.presets.Add(new preset("롤 5대5 내전(라인 구별)", new List<string> { "탑1", "탑2", "정글1", "정글2", "미드1", "미드2", "원딜1", "원딜2", "서폿1", "서폿2", }, false));
                preset.presets.Add(new preset("롤 5대5 내전(팀 구별만)", new List<string> { "팀1", "팀2" }, true));

                StreamWriter streamWriter = new StreamWriter(fs);
                
                preset.Save(streamWriter, preset.presets[0]);
                preset.Save(streamWriter, preset.presets[1]);
                streamWriter.Close();
            }
            catch(Exception)
            {
                StreamReader sr = new StreamReader("presets.txt");
                while(sr.Peek() != -1)
                {
                    //1:  name 2: t/f 3: list 4: \t
                    string name = sr.ReadLine();
                    bool check = sr.ReadLine() == "True" ? true : false;
                    string elem;
                    List<string> list = new List<string>();
                    while ((elem=sr.ReadLine())!="\t")
                    {
                        list.Add(elem);
                    }
                    preset np = new preset(name,list,check);
                    preset.presets.Add(np);
                }
                sr.Close();
            }

        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter && !textBox2.Text.Equals("") && !textBox2.Text.Equals(" "))
            {
                listBox2.Items.Add(textBox2.Text);
                textBox2.Text = "";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !textBox1.Text.Equals("") && !textBox1.Text.Equals(" "))
            {
                listBox1.Items.Add(textBox1.Text);
                textBox1.Text = "";
            }
        }


        void runner()
        {
            if(checkBox1.Checked && listBox1.Items.Count % listBox2.Items.Count != 0)
            {
                MessageBox.Show("팀 균형을 맞출 수 없음","오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            int rem = listBox1.Items.Count / listBox2.Items.Count;
            if (rem != 1)
            {
                int cn = listBox2.Items.Count;
                for (int i = 0; i < cn; i++)
                {
                    for (int j = 0; j < rem-1; j++)
                    {
                        listBox2.Items.Add(listBox2.Items[i].ToString());
                    }
                }
            }


            Random rand = new Random();
            int cnt = listBox2.Items.Count;
            List<String> list = new List<String>();
            for (int i = 0; i < cnt; i++)
            {
                var target = listBox2.Items[rand.Next(listBox2.Items.Count)];
                listBox2.Items.Remove(target);
                list.Add(target.ToString());
            }
            listBox2.Items.Clear();
            foreach (String s in list)
            {
                listBox2.Items.Add(s);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            runner();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listBox2.SelectedIndex != -1)
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                listBox2.SelectedIndex = -1;
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                listBox1.SelectedIndex = -1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            checkBox1.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e) // RESULT SAVE
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.FileName = "result.txt";
            ofd.DefaultExt = "txt";
            ofd.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();
            if(dr== DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(ofd.FileName);
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd dddd H:mm"));
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        sw.Write(listBox1.Items[i].ToString());
                        sw.Write('\t');
                        sw.Write(listBox2.Items[i].ToString());
                        sw.Write('\n');
                    }
                    sw.Close();
                }
            }

        }



        void ULB(int idx)
        {
            preset target = preset.presets[idx];
            if (target == null) return;
            listBox2.Items.Clear();
            foreach(String s in target.list)
            {
                listBox2.Items.Add(s);
            }
            checkBox1.Checked = target.check;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.lb = listBox2;
            form2.cb = checkBox1;
            form2.ShowDialog();
            if (form2.form2sel != -1)
            {
                ULB(form2.form2sel);
            }
        }

        private void button5_Click(object sender, EventArgs e) // SAVE MEMBERS
        {
            if(listBox1.Items.Count == 0)
            {
                MessageBox.Show("사람 목록이 비어있습니다.","오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.FileName = "members.txt";
            ofd.DefaultExt = "txt";
            ofd.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();
            if(dr == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(ofd.FileName);
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        sw.Write(listBox1.Items[i].ToString());
                        sw.Write('\n');
                    }
                    sw.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e) // LOAD MEMBERS
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "members.txt";
            ofd.DefaultExt = "txt";
            ofd.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();
            if(dr==DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    List<string> list = new List<string>();
                    try
                    {
                        StreamReader sr = new StreamReader(ofd.FileName);
                        while (sr.Peek() != -1)
                        {
                            string elem = sr.ReadLine();
                            if (elem == "")
                            {
                                throw new Exception("Wrong File");
                            }
                            list.Add(elem);
                        }
                        sr.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    listBox1.Items.Clear();
                    foreach (string s in list)
                    {
                        listBox1.Items.Add(s);
                    }
                }
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("제작자: infboy#6584\n프리셋은 프로그램 경로에 presets.txt로 저장됩니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
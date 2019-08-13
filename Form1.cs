using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawAndGroup
{
    public partial class Form1 : Form
    {
        private int continueStatus;
        private string[] tempArr;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            continueStatus = 0;
            tempArr = null;
        }

        private LinkedList<string> initItemArray()
        {
            string[] rep = { "\r\n" };
            string[] tempArr = textBox2.Text.Split(rep, StringSplitOptions.RemoveEmptyEntries);
            LinkedListNode<string> prelln = null;
            LinkedList<string> ll = new LinkedList<string>();
            for (int i = 0; i < tempArr.Length; i++)
            {
                LinkedListNode<string> lln = new LinkedListNode<string>(tempArr[i]);
                if (i == 0) ll.AddFirst(lln);
                else if (i == tempArr.Length - 1) ll.AddLast(lln);
                else ll.AddAfter(prelln, lln);
                prelln = lln;
            }
            return ll;
        }

        private string[] getItemArray()
        {
            LinkedList<string> filterArray = initItemArray();
            int len = filterArray.Count, cur = 0;
            string[] arr = new string[len];
            Random rand = new Random();
            while (len > 0)
            {
                arr[cur] = filterArray.ElementAt(rand.Next(0, len)).ToString();
                filterArray.Remove(arr[cur]);
                len = filterArray.Count;
                cur++;
            }
            return arr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text[0] == '0') textBox1.Text = textBox1.Text.TrimStart('0');
            if (textBox1.Text == "" || textBox2.Text.Trim() == "") return;
            continueStatus = 0;
            string[] newArr = getItemArray();
            int cou = int.Parse(textBox1.Text);
            decimal num = Math.Ceiling(decimal.Divide(newArr.Length, cou));
            string ft = "";
            for (int i = 0; i < newArr.Length; i++)
            {
                if (i % num == 0)
                {
                    if (i != 0) ft += "\r\n";
                    ft = ft + "第" + (i / num + 1).ToString() + "组：\r\n";
                }
                ft = ft + newArr[i] + "\r\n";
            }
            textBox3.Text = ft;
        }

        private void importBt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ext files (*.txt)|*.txt";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;
                string safefilename = ofd.SafeFileName;
                string text = File.ReadAllText(filename);
                textBox2.Text = text;
                label1.Text = ofd.SafeFileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Trim() == "") {
                MessageBox.Show("导出数据不能为空！", "提示");
                return;
            }
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择文件保存目录";  //定义在对话框上显示的文本
            DialogResult result = folder.ShowDialog();
            if (result == DialogResult.OK)
            {
                string addr = folder.SelectedPath;
                string fineName = "/draw_and_group_" + GetTimeStamp() + ".txt";
                File.WriteAllText(addr + fineName, textBox3.Text, Encoding.UTF8);
            }
        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 阻止从键盘输入键
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') e.Handled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text[0] == '0') textBox1.Text = textBox1.Text.TrimStart('0');
            if (textBox1.Text == "" || textBox2.Text.Trim() == "") return;
            continueStatus = 0;
            string[] newArr = getItemArray();
            int cou = int.Parse(textBox1.Text);
            string ft = "";
            for (int i = 0; i < newArr.Length; i++)
            {
                if (i >= cou) break;
                ft = ft + newArr[i] + "\r\n";
            }
            textBox3.Text = ft;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text[0] == '0') textBox1.Text = textBox1.Text.TrimStart('0');
            if (textBox2.Text.Trim() == "") return;
            if (continueStatus == 0) {
                textBox3.Text = "";
                tempArr = getItemArray();
            }
            if (continueStatus >= tempArr.Length) {
                MessageBox.Show("所有数据已抽取完毕！");
                return;
            }
            textBox3.Text = textBox3.Text + tempArr[continueStatus] + "\r\n";
            continueStatus++;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            string msg = "1.请导入 txt 文件（UTF-8格式），一条数据占一行\n2.数量只作用于分组和一次抽取，连续抽取不受影响";
            toolTip1.Show(msg, pictureBox1, 0, -68);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(pictureBox1);
        }
    }
}

using LabelManager2;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PrintLabel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void importcsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = new DataTable();
                string Filepath = System.IO.Path.GetDirectoryName(file.FileName) + "\\" + file.SafeFileName; //獲取目錄信息 + 選擇的文件名和擴展名
                this.textBox1.Text = Filepath;
                dt = open_file(Filepath); //把數據寫入dataTable
                this.dataGridView1.DataSource = dt;  //把寫入的DataTable數據源 賦值給datagridview
                this.label1.Text = dt.Rows.Count.ToString();
                this.label2.Text = "0";

            }
        }

        private static DataTable open_file(string Filepath)   
        {
            FileStream fs = new FileStream(Filepath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            DataTable dt = new DataTable();
            string strLine = "";
            //記錄每行記錄中的各字段內容
            string[] aryLine = null;
            string[] tableHead = null;
            //標示列數
            int columnCount = 2;
            //標示是否是讀取的第一行
            bool IsFirst = true;
            //逐行讀取CSV中的數據
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');//
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //創建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        tableHead[i] = tableHead[i].Replace("\"", "");
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].Replace("\"", "");
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();
            fs.Close();
            return dt;
        }

        private void labelTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                string Filepath = System.IO.Path.GetDirectoryName(file.FileName) + "\\" + file.SafeFileName; //獲取目錄信息 + 選擇的文件名和擴展名
                this.textBox2.Text = Filepath;
            }
        }

        private void printbutton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!= "" && textBox2.Text!="")
            {
                Printlabel(textBox1.Text, textBox2.Text);
            }
            else
            {
                MessageBox.Show("请导入csv文件并且选择正确的label 模板");           
            }
        }

        /// <summary>
        /// 遍历csv文件，调用label档实现列印
        /// </summary>
        /// <param name="csvpath"> csv文件路径</param> 
        /// <param name="labelpath"> label图档路径 </param>
        public  void Printlabel(string csvpath,string labelpath)
        {

            FileStream fs = new FileStream(csvpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            //StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //記錄每次讀取的一行記錄
            DataTable dt1 = new DataTable();
            string strLine = "";
            //記錄每行記錄中的各字段內容
            string[] aryLine = null;
            string[] tableHead = null;
            //標示列數
            int columnCount = 2;
            //標示是否是讀取的第一行
            bool IsFirst = true;
            //逐行讀取CSV中的數據
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //創建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        tableHead[i] = tableHead[i].Replace("\"", "");
                        DataColumn dc1 = new DataColumn(tableHead[i]);
                        dt1.Columns.Add(dc1);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr1 = dt1.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr1[j] = aryLine[j].Replace("\"", "");
                    }
                    dt1.Rows.Add(dr1);

                    ApplicationClass lbl = new ApplicationClass(); //codesoft7軟件 引用的dll            
                    lbl.Documents.Open(labelpath, false);// 调用设计好的label文件
                    Document doc = lbl.ActiveDocument;
                    //  doc.Variables.FormVariables.Item("Var0").Value = para1; //給差數傳值

                    string temp;
                    for (int j = 0; j < columnCount; j++)
                    {
                        // -----说明---
                        // label 图档 差数值以ASCII +A  以此累加 参考other文件
                        // 需要codesoft软件
                        temp = "ASCII" + Convert.ToChar(65 + j).ToString();
                        doc.Variables.FormVariables.Item(temp).Value = dr1[j].ToString(); //给参数传值
                    }
                    doc.PrintDocument(1);

                    label2.Text = (Convert.ToDouble(label2.Text) +1).ToString();
                }
            }

            sr.Close();
            fs.Close();
            
        }

    }
}

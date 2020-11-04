using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Modbus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // printDocument1 为 打印控件
            //设置打印用的纸张 当设置为Custom的时候，可以自定义纸张的大小，还可以选择A4,A5等常用纸型
            this.printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 500, 300);
            this.printDocument1.PrintPage += new PrintPageEventHandler(this.MyPrintDocument_PrintPage);
            //将写好的格式给打印预览控件以便预览
            printPreviewDialog1.Document = printDocument1;
            //显示打印预览
            DialogResult result = printPreviewDialog1.ShowDialog();



            Modbus_name modbus_RTU = new Modbus_RTU();
            richTextBox1.AppendText("\r\n"+ BitConverter.ToString(message( modbus_RTU.GenerateH01(function.H01, 01, 10, 1).ToList())));
            richTextBox1.AppendText("\r\n" + BitConverter.ToString(message(modbus_RTU.GenerateH02(function.H02, 01, 1, 10).ToList())));
            richTextBox1.AppendText("\r\n" + BitConverter.ToString(message(modbus_RTU.GenerateH03(function.H03, 01, 1, 10).ToList())));
            //给网友测试的代码
            //查询窗口的TextBox 控件 如果有GroupBox  Panel 这类控件请先遍历 该类  然后用该类的Controls 属性去遍历TextBox 控件
            var t = (from Control P in this.Controls where P is TextBox orderby P.Name select P).ToList();
            //遍历赋值
            for (int i = 0; i < t.Count; i++)
                t[i].Text = i.ToString();
            //假设有GroupBox  
            var t2 = (from Control P in this.Controls where P is GroupBox orderby P.Name select P).ToList();
            List<TextBox> textBoxes = new List<TextBox>(); 
            foreach(var i in t2)
            {
                var t3 = from Control P in i.Controls where P is TextBox orderby P.Name select P;
                foreach(var i1 in t3)
                {
                    if (i1 is TextBox) textBoxes.Add((TextBox)i1);
                }
            }
            var textBoxes1= textBoxes.OrderBy(pi => pi.Name).ToList();//排序
            for (int i = 0; i < textBoxes1.Count; i++)
                textBoxes1[i].Text = i.ToString();
        }
        /// <summary>
        /// 打印的格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            /*如果需要改变自己 可以在new Font(new FontFamily("黑体"),11）中的“黑体”改成自己要的字体就行了，黑体 后面的数字代表字体的大小
             System.Drawing.Brushes.Blue , 170, 10 中的 System.Drawing.Brushes.Blue 为颜色，后面的为输出的位置 */
            e.Graphics.DrawString("新乡市三月软件公司入库单", new Font(new FontFamily("黑体"), 11), System.Drawing.Brushes.Black, 170, 10);
            e.Graphics.DrawString("供货商:河南科技学院", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Blue, 10, 12);
            //信息的名称
            e.Graphics.DrawLine(Pens.Black, 8, 30, 480, 30);
            e.Graphics.DrawString("入库单编号", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 9, 35);
            e.Graphics.DrawString("商品名称", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 160, 35);
            e.Graphics.DrawString("数量", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 260, 35);
            e.Graphics.DrawString("单价", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 330, 35);
            e.Graphics.DrawString("总金额", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 400, 35);
            e.Graphics.DrawLine(Pens.Black, 8, 50, 480, 50);
            //产品信息
            e.Graphics.DrawString("R2011-01-2016:06:35", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 9, 55);
            e.Graphics.DrawString("联想A460", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 160, 55);
            e.Graphics.DrawString("100", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 260, 55);
            e.Graphics.DrawString("200.00", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 330, 55);
            e.Graphics.DrawString("20000.00", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 400, 55);


            e.Graphics.DrawLine(Pens.Black, 8, 200, 480, 200);
            e.Graphics.DrawString("地址：新乡市河南科技学院信息工程学院", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 9, 210);
            e.Graphics.DrawString("经办人:任忌", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 220, 210);
            e.Graphics.DrawString("服务热线:15083128577", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 320, 210);
            e.Graphics.DrawString("入库时间:" + DateTime.Now.ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 9, 230);
        }
        public  byte[] message(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i < 5) vs[i] = Convert.ToByte(random.Next(1, 255));//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
    }
}

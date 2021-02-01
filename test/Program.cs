using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Modbus;
using System.Threading;
using System.Reflection;
namespace test
{
    class Program
    {
        /// <summary>
        /// 测试通讯
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //测试
            Modbus_TCPrealize modbus_TC = new Modbus_TCPrealize(new IPEndPoint(IPAddress.Parse("192.168.0.182"), int.Parse("502")));
            OperateResult<int> operateResult= modbus_TC.Open();
            Console.WriteLine(operateResult.IsSuccess);
            //发送第一个报文测试
            for (int i = 0; i < 9099; i++)
            {
                //Console.WriteLine(modbus_TC.write_multi_Bool(0, 16, 0XFF).Content);

                Console.WriteLine((modbus_TC.Read_Byet(1,high_low.high).Content.ToString()));//读取设备第一个寄存器
                //Thread.Sleep(10);
                Console.WriteLine((modbus_TC.Read_short(0).Content.ToString()));//读取设备第一个寄存器
                //Thread.Sleep(10);
                Console.WriteLine((modbus_TC.Read_int(0).Content.ToString()));//读取设备第一个寄存器
                //Thread.Sleep(10);
                Console.WriteLine((modbus_TC.Write_Byte(0,1,high_low.high).Content.ToString()));//写入设备第一个寄存器
                ////Thread.Sleep(100);
                //Console.WriteLine((modbus_TC.Write_short(0, 0).Content.ToString()));//写入设备第一个寄存器
                ////Thread.Sleep(10);
                Console.WriteLine((modbus_TC.write_Bool(1,i%2==1?coil.OFF:coil.ON).Content.ToString()));//读取设备的线圈
                //Thread.Sleep(10);
                //var dx=modbus_TC.Read_multi_Bool(0,17);//读取设备的多个线圈
                Console.WriteLine((modbus_TC.Read_Bool(2).Content.ToString()));//写入设备第一个寄存器
                //Console.WriteLine(modbus_TC.write_multi_Bool(20, 3, 15).Content);
                //Thread.Sleep(10);
                
            }



            //反射加载
            //Assembly assembly = Assembly.LoadFrom(@"D:\C#项目\Modbus客户端报文产生\Modbus_Class\bin\Debug\Modbus_Class.dll");
            //Type type = assembly.GetType("Modbus_Class.Class1");//获取type实例
            //var activator = Activator.CreateInstance(type);//实例化类
            //var inx = type.GetMethods(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            // var dxd= inx[0].Invoke(activator, new object[] { 11 });

            Console.WriteLine("测试准备开始");
            //实例化套接字
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Inex:
            Console.WriteLine("请输入IP：  默认端口502");
            if (!System.Net.IPAddress.TryParse(Console.ReadLine(), out System.Net.IPAddress address))
            {
                Console.WriteLine("输入IP错误请重新输入");
                goto Inex;
            }
                IPEndPoint iPEndPoint = new IPEndPoint(address, int.Parse("502"));
                Console.WriteLine("下位机是否准备好？？？？--准备开始链接");
                Console.ReadLine();
            try
            {
                socket.Connect(iPEndPoint);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                goto Inex;
            }
            Modbus_name modbus_RTU = new Modbus_RTU();
            Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH01(function.H01, 1, 0, 10).ToList()), socket)));//H01发送
            Thread.Sleep(500);
            Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH02(function.H02, 1, 0, 10).ToList()), socket)));//H01发送
            Thread.Sleep(500);
            Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH03(function.H03, 1, 0, 4).ToList()), socket)));//H03发送
            Thread.Sleep(500);
            Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH05(function.H05, 1, 0, coil.ON).ToList()), socket)));//H05发送
            Thread.Sleep(500);
            Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH06(function.H06, 1, 1, 12345).ToList()), socket)));//H06发送
            Thread.Sleep(500);
            //多线圈写入测试
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH15(function.H15, 1, 0, 3, Convert.ToByte((3 * i))).ToList()), socket)));//H06发送
                Thread.Sleep(500);
            }
            //多寄存器写入
            Console.WriteLine(BitConverter.ToString(communication(message(modbus_RTU.GenerateH16(function.H16, 1, 0, 3,new byte[] { 0x1,0x2,0x3,0x4,0x5,0x66}).ToList()), socket)));//H06发送
            Thread.Sleep(500);
            Console.ReadLine();
        }
        public static byte[] communication(byte[] Data,Socket socket)
        {
            //报文测试
            socket.Send(Data);//发送数据
            byte[] message = new byte[20];
            socket.Receive(message);
            return message;
        }
        //这个方法是吧rtu转TCP--简单粗暴
        public  static byte[] message(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i < 5) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
    }
}

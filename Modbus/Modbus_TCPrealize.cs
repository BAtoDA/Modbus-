using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Modbus
{
    /// <summary>
    /// 用于选择访问设备返回的类型
    /// </summary>
    public enum numerical_format
    {
         Byet, Hex_16_Bit, Hex_32_Bit, Binary_16_Bit, Binary_32_Bit, Unsigned_16_Bit, Signed_16_Bit
           , Unsigned_32_Bit, Signed_32_Bit, Signed_64_Bit, Float_32_Bit
    }
    public enum high_low
    {
        high=1,
        low=0
    }
    /// <summary>
    /// 本类用于实现modbus_TCP 并且返回操作结果
    /// </summary>
    public class Modbus_TCPrealize
    {
        /// <summary>
        /// 实例化一个套接字
        /// </summary>
        Socket socket { get; set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// 实例化一个IP地址与端口
        /// </summary>
        IPEndPoint IPEnd { get; set; } = new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse("502"));
        /// <summary>
        /// 指示着是否访问成功
        /// </summary>
        public bool Socket_ready { get; set; }
        /// <summary>
        /// 实例化一个TCP报文生成对象
        /// </summary>
        Modbus_name Modbus_TCP { get; set; } = new Modbus_TCP();
        /// <summary>
        /// 互斥锁 预防多线程进入导致 数据错乱
        /// </summary>
        static Mutex mutex { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPEnd"></param>
        public Modbus_TCPrealize(IPEndPoint iPEnd)
        {
            
            this.IPEnd = iPEnd;//获取地址
            this.IPEnd.Port = 502;//强行更改端口
            this.socket.ReceiveTimeout = 8000;//超时
            this.socket.SendTimeout = 8000;//超时
            mutex = new Mutex();//实例化
        }
        public Modbus_TCPrealize()
        {

            this.IPEnd.Port = 502;//强行更改端口
            this.socket.ReceiveTimeout = 8000;//超时
            this.socket.SendTimeout = 8000;//超时
            mutex = new Mutex();//实例化
        }
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public OperateResult<int> Open()
        {
            try
            {
                this.socket.Connect(this.IPEnd);//链接远程设备
                this.Socket_ready = true;//标志着访问成功
                return new OperateResult<int>() { Content = 0, ErrorCode ="0", IsSuccess = true };//返回本次访问的结果
            }
            catch(Exception e)
            {
               return Err<int>(e);//推出异常
            }
        }
        /// <summary>
        /// 切断套接字
        /// </summary>
        public void Close()
        {
            if (this.socket.EnableBroadcast)
                this.socket.Close();//切断一个套接字
            else
                return;
        }
        /// <summary>
        /// 使用H03功能码发送报文并且等待设备回复--字节--堵塞模式
        /// </summary>
        /// <param name="address">起始地址字</param>
        /// <param name="high_Low">高低位</param>
        /// <returns></returns>
        public OperateResult<byte> Read_Byet(byte address, high_low high_Low)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Modbus_TCP.GenerateH03(function.H03, 1, address, 1));//发送报文
                    return (OperateResult<byte>) Read_return(numerical_format.Byet,high_Low);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch(Exception e)
            {
                mutex.ReleaseMutex();
                return Err<byte>(e);
            }
        }
        /// <summary>
        /// 使用H03功能码发送报文并且等待设备回复--无符号字--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<short> Read_short(byte address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Modbus_TCP.GenerateH03(function.H03, 1, address, 1));//发送报文
                    return (OperateResult<short>)Read_return(numerical_format.Signed_16_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<short>(e);
            }
        }
        /// <summary>
        /// 使用H03功能码发送报文并且等待设备回复--有符号字--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<ushort> Read_ushort(byte address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Modbus_TCP.GenerateH03(function.H03, 1, address, 1));//发送报文
                    return (OperateResult<ushort>)Read_return(numerical_format.Unsigned_16_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<ushort>(e);
            }
        }
        /// <summary>
        /// 使用H03功能码发送报文并且等待设备回复--无符号双字--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<int> Read_int(byte address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Modbus_TCP.GenerateH03(function.H03, 1, address, 2));//发送报文
                    return (OperateResult<int>)Read_return(numerical_format.Signed_32_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<int>(e);
            }
        }
        /// <summary>
        /// 使用H03功能码发送报文并且等待设备回复--有符号双字--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<uint> Read_uint(byte address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Modbus_TCP.GenerateH03(function.H03, 1, address, 2));//发送报文
                    return (OperateResult<uint>)Read_return(numerical_format.Unsigned_32_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<uint>(e);
            }
        }
        /// <summary>
        /// 用于返回指定类型
        /// </summary>
        /// <param name="numerical">起始地址</param>
        /// <returns></returns>
        private object Read_return(numerical_format numerical)
        {
            object message= new OperateResult<short>() { Content = 0, ErrorCode = "0", IsSuccess = true };
            Thread.Sleep(10);
            try
            {
                byte[] data = new byte[100];//定义数据接收数组  
                this.socket.Receive(data);//接收数据到data数组  
                byte[] datashow = new byte[data[8]];//定义所要显示的接收的数据的长度  
                for (int i = 0; i < datashow.Length; i++)//遍历获取数据
                    datashow[i] = data[data[5] + 3];

                switch (numerical)
                {
                    case numerical_format.Byet:
                        message= new OperateResult<byte>() { Content = data[10], ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_16_Bit:
                        message= new OperateResult<short>() { Content =BitConverter.ToInt16(new byte[] { data[10],data[9]},0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_16_Bit:
                        message = new OperateResult<ushort>() { Content = BitConverter.ToUInt16(new byte[] { data[10], data[9] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_32_Bit:
                        message = new OperateResult<int>() { Content = BitConverter.ToInt32(new byte[] { data[10],data[9], data[12], data[11]}, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_32_Bit:
                        message = new OperateResult<uint>() { Content = BitConverter.ToUInt32(new byte[] { data[10], data[9], data[12], data[11] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                }
            }
            catch(Exception e)
            {
                mutex.ReleaseMutex();
                return Err<int>(e);
            }
            mutex.ReleaseMutex();
            return message;//返回内容
        }
        /// <summary>
        /// 用于返回指定类型
        /// </summary>
        /// <param name="numerical">起始地址</param>
        /// <returns></returns>
        private object Read_return(numerical_format numerical, high_low high_Low)
        {
            object message = new OperateResult<short>() { Content = 0, ErrorCode = "0", IsSuccess = true };
            Thread.Sleep(10);
            try
            {
                byte[] data = new byte[100];//定义数据接收数组  
                this.socket.Receive(data);//接收数据到data数组  
                byte[] datashow = new byte[data[8]];//定义所要显示的接收的数据的长度  
                for (int i = 0; i < datashow.Length; i++)//遍历获取数据
                    datashow[i] = data[data[5] + 3];

                switch (numerical)
                {
                    case numerical_format.Byet:
                        message = new OperateResult<byte>() { Content = (data[10- (byte)high_Low]), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_16_Bit:
                        message = new OperateResult<short>() { Content = BitConverter.ToInt16(new byte[] { data[10], data[9] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_16_Bit:
                        message = new OperateResult<ushort>() { Content = BitConverter.ToUInt16(new byte[] { data[10], data[9] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_32_Bit:
                        message = new OperateResult<int>() { Content = BitConverter.ToInt32(new byte[] { data[10], data[9], data[12], data[11] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_32_Bit:
                        message = new OperateResult<uint>() { Content = BitConverter.ToUInt32(new byte[] { data[10], data[9], data[12], data[11] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                }
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<int>(e);
            }
            mutex.ReleaseMutex();
            return message;//返回内容
        }
        /// <summary>
        /// 使用H16功能码进行数据写入--字节--堵塞模式
        /// </summary>
        /// <param name="address">起始字地址</param>
        /// <param name="Data">数据</param>
        /// <param name="high_Low">写入高低字节</param>
        /// <returns></returns>
        public OperateResult<byte> Write_Byte(byte address,byte Data,high_low high_Low)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (OperateResult<byte>)Write_return(numerical_format.Byet,Data,address, high_Low);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<byte>(e);
            }
        }
        /// <summary>
        /// 使用H16功能码进行数据写入--无符号字--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<short> Write_short(byte address, short Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (OperateResult<short>)Write_return(numerical_format.Signed_16_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<short>(e);
            }
        }
        /// <summary>
        /// 使用H16功能码进行数据写入--无符号双字--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<int> Write_int(byte address, int Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (OperateResult<int>)Write_return(numerical_format.Signed_32_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<int>(e);
            }
        }
        /// <summary>
        /// 使用H16功能码进行数据写入--无符号长整型--堵塞模式
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<long> Write_Long(byte address, long Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (OperateResult<long>)Write_return(numerical_format.Signed_64_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<long>(e);
            }
        }
        /// <summary>
        /// 用于写入的操作结果
        /// </summary>
        /// <typeparam name="T">要写入的类型--约束泛型T</typeparam>
        /// <param name="numerical">要写入的格式</param>
        /// <param name="Data">要写入的数据--类型是约束T的类型</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        private object Write_return<T>(numerical_format numerical,T Data, byte address)
        {
            mutex.WaitOne(3000);
            Thread.Sleep(10);
            object message = new OperateResult<short>() { Content = 0, ErrorCode = "0", IsSuccess = true };
            try
            {
                byte[] Write_Data = new byte[10];//创建默认要写入的数据
                byte[] transition = BitConverter.GetBytes(Convert.ToInt64(Data));
                for (int i = 0; i < transition.Length; i++)//获得要写入的数据
                    Write_Data[i] = transition[i];
                switch (numerical)
                {
                    case numerical_format.Byet:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, (ushort)Write_Data[0]));
                        message = new OperateResult<byte>() { Content = Write_Data[0], ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_16_Bit:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] },0) ));
                        message = new OperateResult<short>() { Content = BitConverter.ToInt16(new byte[] { Write_Data[0], Write_Data[1] },0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_32_Bit:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address+1), BitConverter.ToUInt16(new byte[] { Write_Data[2], Write_Data[3] }, 0)));
                        message = new OperateResult<int>() { Content = BitConverter.ToInt32(new byte[] { Write_Data[0], Write_Data[1],Write_Data[2],Write_Data[3] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_64_Bit:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 1), BitConverter.ToUInt16(new byte[] { Write_Data[2], Write_Data[3] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 2), BitConverter.ToUInt16(new byte[] { Write_Data[4], Write_Data[5] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 3), BitConverter.ToUInt16(new byte[] { Write_Data[6], Write_Data[7] }, 0)));
                        message = new OperateResult<long>() { Content = BitConverter.ToInt64(new byte[] { Write_Data[0], Write_Data[1], Write_Data[2], Write_Data[3],Write_Data[4],Write_Data[5] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                }
                //统一等待报文回复
                byte[] data = new byte[100];//定义数据接收数组  
                this.socket.Receive(data);//接收数据到data数组
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<T>(e);
            }
            mutex.ReleaseMutex();
            return message;//返回内容
        }
        /// <summary>
        /// 用于写入的操作结果
        /// </summary>
        /// <typeparam name="T">要写入的类型--约束泛型T</typeparam>
        /// <param name="numerical">要写入的格式</param>
        /// <param name="Data">要写入的数据--类型是约束T的类型</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        private object Write_return<T>(numerical_format numerical, T Data, byte address,high_low high_Low)
        {
            mutex.WaitOne(3000);
            Thread.Sleep(10);
            object message = new OperateResult<short>() { Content = 0, ErrorCode = "0", IsSuccess = true };
            try
            {
                byte[] Write_Data = new byte[10];//创建默认要写入的数据
                byte[] transition = BitConverter.GetBytes(Convert.ToInt64(Data));
                for (int i = 0; i < transition.Length; i++)//获得要写入的数据
                    Write_Data[i] = transition[i];
                switch (numerical)
                {
                    case numerical_format.Byet:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, high_Low==high_low.low?BitConverter.ToUInt16(new byte[] { Write_Data[0],Write_Data[1]},0): BitConverter.ToUInt16(new byte[] { Write_Data[1], Write_Data[0] }, 0)));
                        message = new OperateResult<byte>() { Content = Write_Data[0], ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_16_Bit:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0)));
                        message = new OperateResult<short>() { Content = BitConverter.ToInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_32_Bit:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 1), BitConverter.ToUInt16(new byte[] { Write_Data[2], Write_Data[3] }, 0)));
                        message = new OperateResult<int>() { Content = BitConverter.ToInt32(new byte[] { Write_Data[0], Write_Data[1], Write_Data[2], Write_Data[3] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_64_Bit:
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, address, BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 1), BitConverter.ToUInt16(new byte[] { Write_Data[2], Write_Data[3] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 2), BitConverter.ToUInt16(new byte[] { Write_Data[4], Write_Data[5] }, 0)));
                        this.socket.Send(this.Modbus_TCP.GenerateH06(function.H06, 1, (ushort)(address + 3), BitConverter.ToUInt16(new byte[] { Write_Data[6], Write_Data[7] }, 0)));
                        message = new OperateResult<long>() { Content = BitConverter.ToInt64(new byte[] { Write_Data[0], Write_Data[1], Write_Data[2], Write_Data[3], Write_Data[4], Write_Data[5] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                }
                //统一等待报文回复
                byte[] data = new byte[100];//定义数据接收数组  
                this.socket.Receive(data);//接收数据到data数组
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<T>(e);
            }
            mutex.ReleaseMutex();
            return message;//返回内容
        }
        /// <summary>
        /// 使用H01功能码进行单个线圈读取
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public OperateResult<bool> Read_Bool(byte address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    Thread.Sleep(10);
                    this.socket.Send(this.Modbus_TCP.GenerateH01(function.H01, 1, address, 1));//发送报文
                    byte[] Data = new byte[50];
                    this.socket.Receive(Data);//接收回复
                    mutex.ReleaseMutex();
                    return new OperateResult<bool>() { Content = Data[9]>0?true:false, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 使用H01功能码读取指定的多个线圈状态
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="number">读取个数</param>
        /// <returns></returns>
        public OperateResult<bool[]> Read_multi_Bool(byte address, byte number)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    Thread.Sleep(10);
                    this.socket.Send(this.Modbus_TCP.GenerateH01(function.H01, 1, address, number));//发送报文
                    byte[] data = new byte[50];
                    this.socket.Receive(data);//接收回复
                    bool[] data_manage = new bool[data[8] * 8];
                    int zhizhen = 0;
                    for (int ix = 0; ix < data[8]; ix++)
                    {
                        bool[] impor = ConvertIntToBoolArray(data[9 + ix], 8);
                        for (int i = 0; i < impor.Length; i++)
                        {
                            data_manage[zhizhen] = impor[i];
                            zhizhen += 1;
                        }
                    }
                    mutex.ReleaseMutex();
                    return new OperateResult<bool[]>() { Content = data_manage, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool[]>(e);
            }
        }
        /// <summary>
        /// byte转B00L
        /// </summary>
        /// <param name="result">要解析的数据</param>
        /// <param name="len">解析后返回的个数</param>
        /// <returns></returns>
        public bool[] ConvertIntToBoolArray(byte result, byte len)
        {

            if (len > 8 || len < 0)
            {
                Console.WriteLine("bool数组长度应该在0到8之间。");
            }

            bool[] barray2 = new bool[len];

            for (int i = 0; i < len; i++)
            {
                barray2[len - i - 1] = ((result >> i) % 2) == 1;
            }
            Array.Reverse(barray2);
            return barray2;
        }
        /// <summary>
        /// 使用功能码H05进行单线圈写入
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="coil">要写入的状态</param>
        /// <returns></returns>
        public OperateResult<bool> write_Bool(byte address,coil coil)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    Thread.Sleep(10);
                    this.socket.Send(this.Modbus_TCP.GenerateH05(function.H05,1,address,coil));//发送报文
                    byte[] data = new byte[100];//定义数据接收数组  
                    this.socket.Receive(data);//接收数据到data数组
                    mutex.ReleaseMutex();
                    return new OperateResult<bool>() { Content = Convert.ToBoolean(coil), ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 使用H15功能码进行多线圈写入
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="multi">要写入的个数</param>
        /// <param name="coil">要写的数据--最大字节</param>
        /// <returns></returns>
        public OperateResult<bool> write_multi_Bool(byte address,byte multi, byte coil)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    Thread.Sleep(10);
                    this.socket.Send(this.Modbus_TCP.GenerateH15(function.H15,1,address, multi,coil));//发送报文
                    byte[] data = new byte[100];//定义数据接收数组  
                    this.socket.Receive(data);//接收数据到data数组
                    mutex.ReleaseMutex();
                    return new OperateResult<bool>() { Content = Convert.ToBoolean(coil), ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 处理Err异常  返回结果
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private OperateResult<T> Err<T>(Exception exception)
        {
            this.Socket_ready = false;//表示链接已经切断
            return new OperateResult<T>() {  ErrorCode = exception.Message, IsSuccess = false };//返回本次访问的结果
        }
    }
}

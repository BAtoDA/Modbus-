using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus
{
    class Modbus_TCP: Modbus_RTU, Modbus_name
    {
        Modbus_name modbus_Name;
        public Modbus_TCP()
        {
            modbus_Name = new Modbus_RTU();
        }
        byte[] Modbus_name.GenerateH01(function function, ushort Station, ushort start, ushort number)
        {
            //return message_H01(modbus_Name.GenerateH01(function, Station, start, number).ToList());
            return message_H02(modbus_Name.GenerateH02(function.H02, Station, start, number).ToList());
        }
        byte[] Modbus_name.GenerateH02(function function, ushort Station, ushort start, ushort number)
        {
            return message_H02(modbus_Name.GenerateH02(function, Station, start, number).ToList());
        }
        byte[] Modbus_name.GenerateH03(function function, ushort Station, ushort start, ushort number)
        {
            //return message_H03(modbus_Name.GenerateH03(function, Station, start, number).ToList());
            return message_H03(modbus_Name.GenerateH03(function.H04, Station, start, number).ToList());
        }
        byte[] Modbus_name.GenerateH04(function function, ushort Station, ushort start, ushort number)
        {
            return message_H03(modbus_Name.GenerateH03(function.H04, Station, start, number).ToList());
        }
        byte[] Modbus_name.GenerateH05(function function, ushort Station, ushort start, coil coil)
        {
            return message_H05(modbus_Name.GenerateH05(function, Station, start, coil).ToList());
        }
        byte[] Modbus_name.GenerateH06(function function, ushort Station, ushort start, ushort number)
        {
            return message_H06(modbus_Name.GenerateH06(function, Station, start, number).ToList());
        }
        byte[] Modbus_name.GenerateH15(function function, ushort Station, ushort start, ushort number, byte coil)
        {
            return message_H15(modbus_Name.GenerateH15(function, Station, start, number,coil).ToList());
        }
        byte[] Modbus_name.GenerateH16(function function, ushort Station, ushort start, ushort number, byte[] content)
        {
            return message_H16(modbus_Name.GenerateH16(function, Station, start, number, content).ToList());
        }
        //这个方法是吧rtu转TCP
        public static byte[] message_H03(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i ==0) vs[i] = Convert.ToByte(38);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(88);//填充标头5个字节
                if (i < 5&i>1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
        public static byte[] message_H06(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i == 0) vs[i] = Convert.ToByte(66);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(23);//填充标头5个字节
                if (i < 5 & i > 1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
        public static byte[] message_H01(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i == 0) vs[i] = Convert.ToByte(12);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(34);//填充标头5个字节
                if (i < 5 & i > 1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
        public static byte[] message_H02(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i == 0) vs[i] = Convert.ToByte(22);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(55);//填充标头5个字节
                if (i < 5 & i > 1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
        public static byte[] message_H05(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i == 0) vs[i] = Convert.ToByte(87);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(65);//填充标头5个字节
                if (i < 5 & i > 1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
        public static byte[] message_H15(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i == 0) vs[i] = Convert.ToByte(11);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(11);//填充标头5个字节
                if (i < 5 & i > 1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
        public static byte[] message_H16(List<byte> Data)
        {
            //实例化随机数--获取标头
            Random random = new Random();
            //由于TCP不需要CRC 所以需要移除CRC二个字节
            Data.RemoveAt(Data.Count - 1);
            Data.RemoveAt(Data.Count - 1);
            byte[] vs = new byte[Data.Count + 6];//定义要发送的长度
            for (int i = 0; i < vs.Length; i++)
            {
                if (i == 0) vs[i] = Convert.ToByte(238);//填充标头5个字节
                if (i == 1) vs[i] = Convert.ToByte(238);//填充标头5个字节
                if (i < 5 & i > 1) vs[i] = Convert.ToByte(00);//填充标头5个字节
                if (i == 5) vs[i] = Convert.ToByte(Data.Count);//填充rtu字节长度
                if (i > 5) vs[i] = Data[i - 6];
            }
            return vs;//返回报文
        }
    }
}

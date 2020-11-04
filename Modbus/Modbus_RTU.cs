using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus
{
    /// <summary>
    /// Modbus-rtu通讯方式--继承接口Modbus_name与Modbus_crc-CRC效验
    /// 其实方法的枚举可以不要--完全可以吧方法合并--谨防小白而已
    /// 其实很多方法和可以合并--H01--H06 除了H05,H15,H16基本都合并成一个方法
    /// @BY-沓
    /// @2020年-5-7
    /// </summary>
    public class Modbus_RTU :Modbus_crc, Modbus_name
    {
        /// <summary>
        /// 实现接口对下位机进行多线圈读取
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">读取起始地址</param>
        /// <param name="number">读取个数</param>
        /// <returns></returns>
         byte[] Modbus_name.GenerateH01(function function, ushort Station, ushort start, ushort number)
        {
            byte[] message = new byte[6];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
                message[4 + i] = (int_to_byte(number)[i]);
            }
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 实现接口对下位机读取设备输入状态
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">读取起始地址</param>
        /// <param name="number">读取个数</param>
        /// <returns></returns>
        byte[] Modbus_name.GenerateH02(function function, ushort Station, ushort start, ushort number)
        {
            byte[] message = new byte[6];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
                message[4 + i] = (int_to_byte(number)[i]);
            }
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 实现对下位机进行字(多字)读取--H03
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">读取起始地址</param>
        /// <param name="number">读取个数（字）</param>
        /// <returns></returns>
        byte[] Modbus_name.GenerateH03(function function, ushort Station, ushort start, ushort number)
        {
            byte[] message = new byte[6];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
                message[4 + i] = (int_to_byte(number)[i]);
            }
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 实现对下位机进行线圈写入--H05
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">写入起始地址</param>
        /// <param name="coil">要写入的状态</param>
        /// <returns></returns>
        byte[] Modbus_name.GenerateH05(function function, ushort Station, ushort start, coil coil)
        {
            byte[] message = new byte[6];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
            }
            message[4] = Convert.ToByte(coil);
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 实现对下位机进行寄存器写入--H06
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">写入起始地址</param>
        /// <param name="number">要写入的数据</param>
        /// <returns></returns>
        byte[] Modbus_name.GenerateH06(function function, ushort Station, ushort start, ushort number)
        {
            byte[] message = new byte[6];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
                message[4 + i] = (int_to_byte(number)[i]);
            }
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 对下位机进行多线圈写入--H15
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">写入起始地</param>
        /// <param name="number">要写入的个数(线圈个数)默认限制8个bit位</param>
        /// <param name="coil">要写入的数据（把要写入的二进制转换成无符号字）</param>
        byte[] Modbus_name.GenerateH15(function function, ushort Station, ushort start, ushort number, byte coil)
        {
            byte[] message = new byte[8];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
                message[4 + i] = (int_to_byte(number)[i]);           
            }
            message[6] = 1;//此处是要写入的寄存器个数--默认8bit 
            message[7] = coil;//要写入的状态
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 实现对下位机进行多寄存器写入--H16
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>          
        /// <param name="start">写入起始地</param>
        /// <param name="number">要写入的个数</param>
        /// <param name="content">要写入的数据数组</param>
        /// <returns></returns>
        byte[] Modbus_name.GenerateH16(function function, ushort Station, ushort start, ushort number, byte[] content)
        {
            byte[] message = new byte[6+content.Length+1];
            message[0] = Convert.ToByte(Station);
            message[1] = Convert.ToByte(function);
            for (int i = 0; i < 2; i++)
            {
                message[2 + i] = (int_to_byte(start)[i]);
                message[4 + i] = (int_to_byte(number)[i]);
            }
            message[6] = Convert.ToByte(content.Length);
            for (int i = 0; i < content.Length; i++)
                message[7 + i] = content[i];
            return this.GetCRCDatas(message);
        }
        /// <summary>
        /// 进行int32转字节-放弃高字节
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] int_to_byte(int Data)
        {
            byte[] transition_Data = new byte[2];
            byte[] transition = BitConverter.GetBytes(Data);
            if (transition.Length > 1)
                for (int i = 0; i < transition_Data.Length; i++)
                    transition_Data[i] = transition[i];
            else
                transition_Data[0] = transition[0];
            Array.Reverse(transition_Data);
            return transition_Data;
        }
    }
}

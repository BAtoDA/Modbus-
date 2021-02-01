using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus
{
    public enum function
    {
        H01=0X1,
        H02=0X2,
        H03=0X3,
        H04=0X4,
        H05=0X5,
        H06=0X6,
        H15=0XF,
        H16=0X10
    }
    public enum coil
    {
        ON=0XFF,
        OFF=0X00
    }
    public interface Modbus_name
    {
        /// <summary>
        /// 对下位机进行多线圈读取--H01
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">读取起始地址</param>
        /// <param name="number">读取个数</param>
        /// <returns></returns>
        byte[] GenerateH01(function function, ushort Station, ushort start, ushort number);
        /// <summary>
        /// 对下位机读取设备输入状态--H02
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">读取起始地址</param>
        /// <param name="number">读取个数</param>
        /// <returns></returns>
        byte[] GenerateH02(function function, ushort Station, ushort start, ushort number);
        /// <summary>
        /// 对下位机进行字(多字)读取--H03
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">读取起始地址</param>
        /// <param name="number">读取个数（字）</param>
        /// <returns></returns>
        byte[] GenerateH03(function function, ushort Station, ushort start, ushort number);
        /// <summary>
        /// 对下位机进行模拟量读取--H04
        /// </summary>
        /// <param name="function"></param>
        /// <param name="Station"></param>
        /// <param name="start"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        byte[] GenerateH04(function function, ushort Station, ushort start, ushort number);
        //byte[] GenerateH04(function function, ushort Station, ushort start, ushort number);
        /// <summary>
        /// 对下位机进行线圈写入--H05
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">写入起始地址</param>
        /// <param name="coil">要写入的状态</param>
        /// <returns></returns>
        byte[] GenerateH05(function function, ushort Station, ushort start, coil coil);
        /// <summary>
        /// 对下位机进行寄存器写入--H06
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">写入起始地址</param>
        /// <param name="number">要写入的数据</param>
        /// <returns></returns>
        byte[] GenerateH06(function function, ushort Station, ushort start, ushort number);
        /// <summary>
        /// 对下位机进行多线圈写入--H15
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>
        /// <param name="start">写入起始地</param>
        /// <param name="number">要写入的个数--线圈个数不能大于一个字</param>
        /// <param name="coil">要写入的数据（把要写入的二进制转换成无符号字）</param>
        /// <returns></returns>
        byte[] GenerateH15(function function, ushort Station, ushort start, ushort number, byte coil);
        /// <summary>
        /// 对下位机进行多寄存器写入--H16
        /// </summary>
        /// <param name="function">功能码枚举</param>
        /// <param name="Station">访问的站号</param>          
        /// <param name="start">写入起始地</param>
        /// <param name="number">要写入的个数</param>
        /// <param name="content">要写入的数据数组</param>
        /// <returns></returns>
        byte[] GenerateH16(function function, ushort Station, ushort start, ushort number, byte[] content);
        //byte[] Resolution(ushort data);
    }
}

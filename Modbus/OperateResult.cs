using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus
{
    /// <summary>
    /// 指示着访问结果-与访问返回的数据
    /// </summary>
    public class OperateResult<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperateResult()
        {

        }
        /// <summary>
        ///  用户自定义的泛型数据
        /// </summary>
        public T Content { get; set; }
        /// <summary>
        ///   指示本次访问是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 具体的错误代码
        /// </summary>
        public string ErrorCode { get; set; }
    }
}

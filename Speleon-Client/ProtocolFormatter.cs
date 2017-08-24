using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speleon_Client
{
    static class ProtocolFormatter
    {
        public enum CMDType
        {
            Login
        }

        static public string FormatProtocol(CMDType cmdType,params object[] ProtocalValues)
        {
            UnityModule.DebugPrint("开始格式化通信协议：{0}-{1}",cmdType.ToString(),string.Join("/",ProtocalValues));
            //每条协议最后加一个换行符，否则服务端无法使用正则匹配最后一个参数
            switch (cmdType)
            {
                case CMDType.Login:
                    {
                        if (ProtocalValues.Length == 3)
                            return string.Format("HEY_CVER={0}_CMDTYPE=LOGIN_USERID={1}_PASSWORD={2}\n", ProtocalValues[0], ProtocalValues[1], ProtocalValues[2]);
                        else
                        {
                            UnityModule.DebugPrint("！ 参数个数不等于3");
                            return "";
                        }
                    }
            }
            return "";
        }

    }
}

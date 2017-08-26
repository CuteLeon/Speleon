using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speleon_Server
{
    static class ProtocolFormatter
    {
        /// <summary>
        /// 服务器收到的协议数据类型
        /// </summary>
        public enum CMDType
        {
            /// <summary>
            /// 异地登录
            /// </summary>
            AnothorSignIn,
            /// <summary>
            /// 登录
            /// </summary>
            SignIn,
            /// <summary>
            /// 登录成功
            /// </summary>
            SignInSuccessfully,
            /// <summary>
            /// 登录失败
            /// </summary>
            SignInUnsuccessfully,
            /// <summary>
            /// 用户报告
            /// </summary>
            WhoAmI,
            /// <summary>
            /// 聊天消息
            /// </summary>
            ChatMessage
        }

        /// <summary>
        /// 返回获取CMDTYPE的正则表达式
        /// </summary>
        /// <returns></returns>
        static public string GetCMDTypePattern()
        {
            return "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=(?<CMDTYPE>.+?)_";
        }

        /// <summary>
        /// 获取指定协议类型的正则表达式
        /// </summary>
        /// <param name="cmdType">协议类型</param>
        /// <returns>指定协议的正则表达式</returns>
        static public string GetProtocolPattern(CMDType cmdType)
        {
            UnityModule.DebugPrint("开始获取协议正则表达式：{0}", cmdType.ToString());
            string ProtocolString = "";
            //每条协议最后使用换行符结束
            try
            {
                switch (cmdType)
                {
                    case CMDType.SignIn:
                        {
                            ProtocolString = "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=(?<CMDTYPE>.+?)_USERID=(?<USERID>.+?)_PASSWORD=(?<PASSWORD>.+?)\n";
                            break;
                        }
                    case CMDType.WhoAmI:
                        {
                            ProtocolString = "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=(?<CMDTYPE>.+?)_USERID=(?<USERID>.+?)\n";
                            break;
                        }
                    default:
                        {
                            return "";
                        }
                }
                UnityModule.DebugPrint("协议正则表达式：{0}", ProtocolString);
                return ProtocolString;
            }
            catch (Exception ex)
            {
                UnityModule.DebugPrint("获取 {0} 协议正则表达式时遇到错误：{1}",cmdType.ToString(),ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 获取按指定协议类型格式化的协议字符串
        /// </summary>
        /// <param name="cmdType">协议类型</param>
        /// <returns>协议字符串</returns>
        static public string FormatProtocol(CMDType cmdType, params object[] ProtocolValues)
        {
            UnityModule.DebugPrint("开始格式化通信协议：{0}-{1}", cmdType.ToString(), string.Join("/", ProtocolValues));
            //每条协议最后加一个换行符，否则服务端无法使用正则匹配最后一个参数
            try
            {
                switch (cmdType)
                {
                    case CMDType.ChatMessage:
                        {
                            return string.Format("HI_CMDTYPE=CHATMESSAGE_FROMID={0}",ProtocolValues[0] as string);
                        }
                    case CMDType.SignInSuccessfully:
                        {
                            return String.Format("HI_CMDTYPE=SIGNINSUCCESSFULLY_USERID={0}\n",ProtocolValues[0] as string);
                        }
                    case CMDType.SignInUnsuccessfully:
                        {
                            return String.Format("HI_CMDTYPE=SIGNINUNSUCCESSFULLY_USERID={0}\n", ProtocolValues[0] as string);
                        }
                    case CMDType.AnothorSignIn:
                        {
                            return string.Format("HI_CMDTYPE=ANOTHORSIGNIN_USERID={0}\n",ProtocolValues[0] as string);
                        }
                    default:
                        {
                            return "";
                        }
                }
            }
            catch (Exception ex)
            {
                UnityModule.DebugPrint("获取 {0} 通信协议时遇到错误：{1}", cmdType.ToString(), ex.Message);
                return "";
            }
        }
    }
}

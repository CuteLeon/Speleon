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
                    case CMDType.ChatMessage:
                        {
                            ProtocolString = "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=CHATMESSAGE_TOID=(?<TOID>.+?)_MESSAGE=(?<MESSAGE>.+?)\n";
                            break;
                        }
                    case CMDType.SignIn:
                        {
                            ProtocolString = "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=SIGNIN_USERID=(?<USERID>.+?)_PASSWORD=(?<PASSWORD>.+?)\n";
                            break;
                        }
                    case CMDType.WhoAmI:
                        {
                            ProtocolString = "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=WHOAMI_USERID=(?<USERID>.+?)\n";
                            break;
                        }
                    default:
                        {
                            ProtocolString = "";
                            break;
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
            string ProtocolString=null;
            //每条协议最后加一个换行符，否则服务端无法使用正则匹配最后一个参数
            try
            {
                switch (cmdType)
                {
                    case CMDType.ChatMessage:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=CHATMESSAGE_FROMID={0}_MESSAGE={1}\n",ProtocolValues[0] as string,Convert.ToBase64String(Encoding.UTF8.GetBytes(ProtocolValues[1] as string)));
                            break;
                        }
                    case CMDType.SignInSuccessfully:
                        {
                            ProtocolString = String.Format("HI_CMDTYPE=SIGNINSUCCESSFULLY_USERID={0}\n",ProtocolValues[0] as string);
                            break;
                        }
                    case CMDType.SignInUnsuccessfully:
                        {
                            ProtocolString = String.Format("HI_CMDTYPE=SIGNINUNSUCCESSFULLY_USERID={0}\n", ProtocolValues[0] as string);
                            break;
                        }
                    case CMDType.AnothorSignIn:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=ANOTHORSIGNIN_USERID={0}\n",ProtocolValues[0] as string);
                            break;
                        }
                    default:
                        {
                            ProtocolString = "";
                            break;
                        }
                }
                UnityModule.DebugPrint("协议内容：{0}", ProtocolString);
                return ProtocolString;
            }
            catch (Exception ex)
            {
                UnityModule.DebugPrint("获取 {0} 通信协议时遇到错误：{1}", cmdType.ToString(), ex.Message);
                return "";
            }
        }

    }
}

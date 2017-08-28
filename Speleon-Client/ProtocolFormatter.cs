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
            /// <summary>
            /// 异地登录
            /// </summary>
            AnothorSignIn,
            /// <summary>
            /// 登录
            /// </summary>
            SignIn,
            /// <summary>
            /// 获取好友列表
            /// </summary>
            GetFriendsList,
            /// <summary>
            /// 好友列表发送完成
            /// </summary>
            FriendsListComplete,
            /// <summary>
            /// 获取暂存消息
            /// </summary>
            GetMessageNotSendYet,
            /// <summary>
            /// 暂存消息发送完成
            /// </summary>
            MessageNSYComplete,
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
            /// 服务端关闭
            /// </summary>
            ServerShutdown,
            /// <summary>
            /// 聊天消息
            /// </summary>
            ChatMessage,
            /// <summary>
            /// 注销登录
            /// </summary>
            SignOut
        }

        /// <summary>
        /// 获取指定协议类型的正则表达式
        /// </summary>
        /// <param name="cmdType">协议类型</param>
        /// <returns>指定协议的正则表达式</returns>
        static public string GetProtocolPattern(CMDType cmdType)
        {
            UnityModule.DebugPrint("开始获取协议正则表达式：{0}", cmdType.ToString());
            string ProtocolString = null;
            //每条协议最后使用换行符结束
            switch (cmdType)
            {
                case CMDType.ChatMessage:
                    {
                        ProtocolString = "HI_CMDTYPE=CHATMESSAGE_FROMID=(?<FROMID>.+?)_CHATTIME=(?<CHATTIME>.+?)_MESSAGE=(?<MESSAGE>.+?)\n";
                        break;
                    }
                case CMDType.GetFriendsList:
                    {
                        ProtocolString = "HI_CMDTYPE=GETFRIENDSLIST_FRIENDID=(?<FRIENDID>.+?)_NICKNAME=(?<NICKNAME>.+?)_SIGNATURE=(?<SIGNATURE>.+?)\n";
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

        /// <summary>
        /// 获取按指定协议类型格式化的协议字符串
        /// </summary>
        /// <param name="cmdType">协议类型</param>
        /// <returns>协议字符串</returns>
        static public string FormatProtocol(CMDType cmdType,params string[] ProtocolValues)
        {
            UnityModule.DebugPrint("开始格式化通信协议：{0} : {1}", cmdType.ToString(), string.Join(" + ", ProtocolValues));
            //每条协议最后加一个换行符，否则服务端无法使用正则匹配最后一个参数
            string ProtocolString = "";
            try
            {
                switch (cmdType)
                {
                    case CMDType.ChatMessage:
                        {
                            ProtocolString = string.Format("HEY_CVER={0}_CMDTYPE=CHATMESSAGE_TOID={1}_MESSAGE={2}\n",ProtocolValues[0], ProtocolValues[1], Convert.ToBase64String(Encoding.UTF8.GetBytes(ProtocolValues[2])));
                            break;
                        }
                    case CMDType.SignIn:
                        {
                            ProtocolString = string.Format("HEY_CVER={0}_CMDTYPE=SIGNIN_USERID={1}_PASSWORD={2}\n", ProtocolValues[0], ProtocolValues[1], ProtocolValues[2]);
                            break;
                        }
                    case CMDType.WhoAmI:
                        {
                            ProtocolString = string.Format("HEY_CVER={0}_CMDTYPE=WHOAMI_USERID={1}\n", ProtocolValues[0], ProtocolValues[1]);
                            break;
                        }
                    case CMDType.GetFriendsList:
                        {
                            ProtocolString = string.Format("HEY_CVER={0}_CMDTYPE=GETFRIENDSLIST_USERID={1}\n",ProtocolValues[0],ProtocolValues[1]);
                            break;
                        }
                    case CMDType.SignOut:
                        {
                            ProtocolString = string.Format("HEY_CVER={0}_CMDTYPE=SIGNOUT_USERID={1}\n",ProtocolValues[0],ProtocolValues[1]);
                            break;
                        }
                    case CMDType.GetMessageNotSendYet:
                        {
                            ProtocolString = string.Format("HEY_CVER={0}_CMDTYPE=GETMESSAGENOTSENDYET_USERID={1}\n",ProtocolValues[0],ProtocolValues[1]);
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
                UnityModule.DebugPrint("获取 {0} 通信协议时遇到错误：{1}",cmdType.ToString(),ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 返回获取CMDTYPE的正则表达式
        /// </summary>
        /// <returns></returns>
        static public string GetCMDTypePattern()
        {
            return "HI_CMDTYPE=(?<CMDTYPE>.+?)_";
        }

    }
}

﻿using System;
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
            /// 用户报告
            /// </summary>
            WhoAmI,
            /// <summary>
            /// 好友登录
            /// </summary>
            FriendSignIn,
            /// <summary>
            /// 好友注销登录
            /// </summary>
            FriendSignOut,
            /// <summary>
            /// 获取好友列表
            /// </summary>
            GetFriendsList,
            /// <summary>
            /// 获取聊天历史记录
            /// </summary>
            GetChatHistory,
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
                    case CMDType.GetChatHistory:
                        {
                            ProtocolString = "HEY_CVER=(?<CLIENTVERSION>.+?)_CMDTYPE=GETCHATHISTORY_FRIENDID=(?<FRIENDID>.+?)_FIRSTMESSAGEID=(?<FIRSTMESSAGEID>.+?)\n";
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
        static public string FormatProtocol(CMDType cmdType, params string[] ProtocolValues)
        {
            UnityModule.DebugPrint("开始格式化通信协议：{0} : {1}", cmdType.ToString(), string.Join(" + ", ProtocolValues));
            string ProtocolString=null;
            //每条协议最后加一个换行符，否则服务端无法使用正则匹配最后一个参数
            try
            {
                switch (cmdType)
                {
                    case CMDType.ChatMessage:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=CHATMESSAGE_FROMID={0}_CHATTIME={1}_MESSAGEID={2}_MESSAGE={3}\n",ProtocolValues[0],ProtocolValues[1],ProtocolValues[2],ProtocolValues[3]);
                            break;
                        }
                    case CMDType.GetFriendsList:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=GETFRIENDSLIST_FRIENDID={0}_NICKNAME={1}_SIGNATURE={2}_ONLINE={3}\n",ProtocolValues[0], Convert.ToBase64String(Encoding.UTF8.GetBytes(ProtocolValues[1])), Convert.ToBase64String(Encoding.UTF8.GetBytes(ProtocolValues[2])),ProtocolValues[3]);
                            break;
                        }
                    case CMDType.SignInSuccessfully:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=SIGNINSUCCESSFULLY_USERID={0}\n",ProtocolValues[0]);
                            break;
                        }
                    case CMDType.SignInUnsuccessfully:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=SIGNINUNSUCCESSFULLY_USERID={0}\n", ProtocolValues[0]);
                            break;
                        }
                    case CMDType.FriendsListComplete:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=FRIENDSLISTCOMPLETE_USERID={0}\n",ProtocolValues[0]);
                            break;
                        }
                    case CMDType.MessageNSYComplete:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=MESSAGENSYCOMPLETE_USERID={0}\n",ProtocolValues[0]);
                            break;
                        }
                    case CMDType.FriendSignIn:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=FRIENDSIGNIN_FRIENDID={0}\n", ProtocolValues[0]);
                            break;
                        }
                    case CMDType.FriendSignOut:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=FRIENDSIGNOUT_FRIENDID={0}\n", ProtocolValues[0]);
                            break;
                        }
                    case CMDType.AnothorSignIn:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=ANOTHORSIGNIN_USERID={0}\n",ProtocolValues[0]);
                            break;
                        }
                    case CMDType.ServerShutdown:
                        {
                            ProtocolString = string.Format("HI_CMDTYPE=SERVERSHUTDOWN_\n");
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

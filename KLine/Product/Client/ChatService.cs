/*****************************************************************************\
*                                                                             *
* ChatService.cs -  Base service functions, types, and definitions            *
*                                                                             *
*               Version 1.00 ★                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by QiChunyou.                                         *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KLine
{
    /// <summary>
    /// 聊天服务
    /// </summary>
    public class ChatService:BaseService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChatService()
        {
            ServiceID = SERVICEID_CHAT;
        }

        /// <summary>
        /// 聊天服务ID
        /// </summary>
        public const int SERVICEID_CHAT = 7;

        /// <summary>
        /// 进入聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_ENTER = 0;

        /// <summary>
        /// 退出聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_EXIT = 1;

        /// <summary>
        /// 发送聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_SEND = 2;

        /// <summary>
        /// 接收聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_RECV = 3;

        private int m_socketID;

        /// <summary>
        /// 获取或设置套接字ID
        /// </summary>
        public int SocketID
        {
            get { return m_socketID; }
            set { m_socketID = value; }
        }

        /// <summary>
        /// 进入弹幕
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="args">弹幕数据</param>
        public int Enter(int requestID, ChatData chat)
        {
            List<ChatData> datas = new List<ChatData>();
            datas.Add(chat);
            int ret = SendToAllClients(FUNCTIONID_CHAT_ENTER, requestID, datas);
            datas.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 退出弹幕
        /// </summary>
        /// <param name="requestID">请求ID</param>
       /// <param name="args">弹幕数据</param>
        public int Exit(int requestID, ChatData chat)
        {
            List<ChatData> datas = new List<ChatData>();
            datas.Add(chat);
            int ret = SendToAllClients(FUNCTIONID_CHAT_EXIT, requestID, datas);
            datas.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取弹幕信息
        /// </summary>
        /// <param name="datas">用于返回弹幕信息</param>
        /// <param name="body">包体</param>
        /// <param name="bodyLength">包体长度</param>
        /// <returns></returns>
        public static int GetChatDatas(List<ChatData> datas, byte[] body, int bodyLength)
        {
            Binary br = new Binary();
            br.Write(body, bodyLength);
            int size = br.ReadInt();
            for (int i = 0; i < size; i++)
            {
                ChatData chat = new ChatData();
                chat.m_userID = br.ReadInt();
                chat.m_type = br.ReadChar();
                chat.m_text = br.ReadString();
                datas.Add(chat);
            }
            br.Close();
            return 1;     
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="message">消息</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            if (message.m_functionID == FUNCTIONID_CHAT_RECV)
            {
                SendToListener(message);
            }         
        }

        /// <summary>
        /// 进入弹幕
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="requestID">请求ID</param>
        /// <param name="args"></param>
        public int Send(int requestID, ChatData chat)
        {
            List<ChatData> datas = new List<ChatData>();
            datas.Add(chat);
            int ret = SendToAllClients(FUNCTIONID_CHAT_SEND, requestID, datas);
            datas.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="userID">方法ID</param>
        /// <param name="userID">请求ID</param>
        /// <param name="text">发送字符</param>
        public int SendToAllClients(int functionID, int requestID, List<ChatData> datas)
        {
            Binary bw = new Binary();
            int chatSize = datas.Count;
            bw.WriteInt(chatSize);
            for (int i = 0; i < chatSize; i++)
            {
                ChatData chat = datas[i];
                bw.WriteInt(chat.m_userID);
                bw.WriteChar((char)chat.m_type);
                bw.WriteString(chat.m_text);
            }
            byte[] bytes = bw.GetBytes();
            int ret = Send(new CMessage(GroupID, ServiceID, functionID, SessionID, requestID, m_socketID, 0, CompressType, bytes.Length, bytes));
            bw.Close();
            return ret;
        }
    }
}

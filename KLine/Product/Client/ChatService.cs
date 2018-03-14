/*****************************************************************************\
*                                                                             *
* ChatService.cs -  Base service functions, types, and definitions            *
*                                                                             *
*               Version 1.00 ��                                               *
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
    /// �������
    /// </summary>
    public class ChatService:BaseService
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public ChatService()
        {
            ServiceID = SERVICEID_CHAT;
        }

        /// <summary>
        /// �������ID
        /// </summary>
        public const int SERVICEID_CHAT = 7;

        /// <summary>
        /// �������칦��ID
        /// </summary>
        public const int FUNCTIONID_CHAT_ENTER = 0;

        /// <summary>
        /// �˳����칦��ID
        /// </summary>
        public const int FUNCTIONID_CHAT_EXIT = 1;

        /// <summary>
        /// �������칦��ID
        /// </summary>
        public const int FUNCTIONID_CHAT_SEND = 2;

        /// <summary>
        /// �������칦��ID
        /// </summary>
        public const int FUNCTIONID_CHAT_RECV = 3;

        private int m_socketID;

        /// <summary>
        /// ��ȡ�������׽���ID
        /// </summary>
        public int SocketID
        {
            get { return m_socketID; }
            set { m_socketID = value; }
        }

        /// <summary>
        /// ���뵯Ļ
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="args">��Ļ����</param>
        public int Enter(int requestID, ChatData chat)
        {
            List<ChatData> datas = new List<ChatData>();
            datas.Add(chat);
            int ret = SendToAllClients(FUNCTIONID_CHAT_ENTER, requestID, datas);
            datas.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// �˳���Ļ
        /// </summary>
        /// <param name="requestID">����ID</param>
       /// <param name="args">��Ļ����</param>
        public int Exit(int requestID, ChatData chat)
        {
            List<ChatData> datas = new List<ChatData>();
            datas.Add(chat);
            int ret = SendToAllClients(FUNCTIONID_CHAT_EXIT, requestID, datas);
            datas.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ��ȡ��Ļ��Ϣ
        /// </summary>
        /// <param name="datas">���ڷ��ص�Ļ��Ϣ</param>
        /// <param name="body">����</param>
        /// <param name="bodyLength">���峤��</param>
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
        /// ������Ϣ
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            if (message.m_functionID == FUNCTIONID_CHAT_RECV)
            {
                SendToListener(message);
            }         
        }

        /// <summary>
        /// ���뵯Ļ
        /// </summary>
        /// <param name="userID">�û�ID</param>
        /// <param name="requestID">����ID</param>
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
        /// ������Ϣ
        /// </summary>
        /// <param name="userID">����ID</param>
        /// <param name="userID">����ID</param>
        /// <param name="text">�����ַ�</param>
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

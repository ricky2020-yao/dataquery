/**************************************************************************************\
*                                                                                      *
* UserSessionService.cs -  User session service functions, types, and definitions.     *
*                                                                                      *
*               Version 1.00 ★                                                        *
*                                                                                      *
*               Copyright (c) 2016-2016, Client. All rights reserved.                  *
*               Created by TangWen.                                                    *
*                                                                                      *
***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KLine
{
    /// <summary>
    /// 用户会话服务
    /// </summary>
    public class UserSessionService : BaseService
    {
        /// <summary>
        /// 创建用户会话服务
        /// </summary>
        public UserSessionService()
        {
            ServiceID = SERVICEID_USERSESSION;
        }

        /// <summary>
        /// 用户会话服务ID
        /// </summary>
        public const int SERVICEID_USERSESSION = 8;

        /// <summary>
        /// 添加用户会话方法ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_ADDSESSION = 0;

        /// <summary>
        /// 删除用户会话方法ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_DELETESESSION = 1;

        /// <summary>
        /// 获取所有用户会话方法ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_GETSESSIONS = 2;

        /// <summary>
        /// 更新用户会话方法ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_UPDATESESSION = 3;

        private int m_socketID = 0;

        /// <summary>
        /// 获取或设置套接字ID
        /// </summary>
        public int SocketID
        {
            get { return m_socketID; }
            set { m_socketID = value; }
        }

        /// <summary>
        /// 添加用户会话
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="session">用户会话</param>
        /// <returns>状态</returns>
        public int AddSession(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_ADDSESSION, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 删除用户会话
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="session">用户会话</param>
        /// <returns>状态</returns>
        public int DeleteSession(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_DELETESESSION, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取所有用户会话
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="session">用户会话</param>
        /// <returns>状态</returns>
        public int GetSessions(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_GETSESSIONS, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 通过流获取用户会话
        /// </summary>
        /// <param name="sessions">用户会话</param>
        /// <param name="body">包体</param>
        /// <param name="bodyLength">包体长度</param>
        public static int GetSessions(List<UserSession> sessions, byte[] body, int bodyLength)
        {
            Binary br = new Binary();
            br.Write(body, bodyLength);
            int size = br.ReadInt();
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    UserSession session = new UserSession();
                    session.m_userID = br.ReadInt();
                    session.m_key = br.ReadString();
                    session.m_value = br.ReadString();
                    sessions.Add(session);
                }
            }
            br.Close();
            return 1;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            SendToListener(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="functionID">方法ID</param>
        /// <param name="requestID">请求ID</param>
        /// <param name="sessions">用户会话集合</param>
        /// <returns>状态</returns>
        public int Send(int functionID, int requestID, List<UserSession> sessions)
        {
            Binary bw = new Binary();
            int size = sessions.Count;
            bw.WriteInt(size);
            for (int i = 0; i < size; i++)
            {
                UserSession session = sessions[i];
                bw.WriteInt(session.m_userID);
                bw.WriteString(session.m_key);
                bw.WriteString(session.m_value);
            }
            byte[] bytes = bw.GetBytes();
            int ret = Send(new CMessage(GroupID, ServiceID, functionID, SessionID, requestID, m_socketID, 0, CompressType, bytes.Length, bytes));
            bw.Close();
            return ret;
        }

        /// <summary>
        /// 更新会话
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="session">会话</param>
        /// <returns>状态</returns>
        public int UpdateSession(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_UPDATESESSION, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }
    }
}

/**************************************************************************************\
*                                                                                      *
* UserSessionService.cs -  User session service functions, types, and definitions.     *
*                                                                                      *
*               Version 1.00 ��                                                        *
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
    /// �û��Ự����
    /// </summary>
    public class UserSessionService : BaseService
    {
        /// <summary>
        /// �����û��Ự����
        /// </summary>
        public UserSessionService()
        {
            ServiceID = SERVICEID_USERSESSION;
        }

        /// <summary>
        /// �û��Ự����ID
        /// </summary>
        public const int SERVICEID_USERSESSION = 8;

        /// <summary>
        /// ����û��Ự����ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_ADDSESSION = 0;

        /// <summary>
        /// ɾ���û��Ự����ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_DELETESESSION = 1;

        /// <summary>
        /// ��ȡ�����û��Ự����ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_GETSESSIONS = 2;

        /// <summary>
        /// �����û��Ự����ID
        /// </summary>
        public const int FUNCTIONID_USERSESSION_UPDATESESSION = 3;

        private int m_socketID = 0;

        /// <summary>
        /// ��ȡ�������׽���ID
        /// </summary>
        public int SocketID
        {
            get { return m_socketID; }
            set { m_socketID = value; }
        }

        /// <summary>
        /// ����û��Ự
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="session">�û��Ự</param>
        /// <returns>״̬</returns>
        public int AddSession(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_ADDSESSION, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ɾ���û��Ự
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="session">�û��Ự</param>
        /// <returns>״̬</returns>
        public int DeleteSession(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_DELETESESSION, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ��ȡ�����û��Ự
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="session">�û��Ự</param>
        /// <returns>״̬</returns>
        public int GetSessions(int requestID, UserSession session)
        {
            List<UserSession> sessions = new List<UserSession>();
            sessions.Add(session);
            int ret = Send(FUNCTIONID_USERSESSION_GETSESSIONS, requestID, sessions);
            sessions.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ͨ������ȡ�û��Ự
        /// </summary>
        /// <param name="sessions">�û��Ự</param>
        /// <param name="body">����</param>
        /// <param name="bodyLength">���峤��</param>
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
        /// ��������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            SendToListener(message);
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="functionID">����ID</param>
        /// <param name="requestID">����ID</param>
        /// <param name="sessions">�û��Ự����</param>
        /// <returns>״̬</returns>
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
        /// ���»Ự
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="session">�Ự</param>
        /// <returns>״̬</returns>
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

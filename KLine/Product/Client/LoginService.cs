/*****************************************************************************\
*                                                                             *
* LoginService.cs -  Login service functions, types, and definitions          *
*                                                                             *
*               Version 1.00 ��                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by Lord.                                              *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KLine
{
    /// <summary>
    /// ��¼����
    /// </summary>
    public class LoginService:BaseService
    {
        #region Lord 2016/3/24
        /// <summary>
        /// ������¼����
        /// </summary>
        public LoginService()
        {
            ServiceID = SERVICEID_LOGIN;
        }

        /// <summary>
        /// ָ�����ID
        /// </summary>
        public const int SERVICEID_LOGIN = 5;

        /// <summary>
        /// �û���¼�Ĺ���ID
        /// </summary>
        public const int FUNCTIONID_LOGIN_LOGIN = 0;

        /// <summary>
        /// �û��ǳ��Ĺ���ID
        /// </summary>
        public const int FUNCTIONID_LOGIN_LOGOUT = 1;

        /// <summary>
        /// ��ȡ�û���Ϣ
        /// </summary>
        public const int FUNCTIONID_LOGIN_GETLOGININFOS = 2;

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
        /// ��ȡ��¼��Ϣ
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <returns>״̬</returns>
        public int GetLoginInfos(int requestID)
        {
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.m_sessionID = SessionID;
            List<LoginInfo> loginInfos = new List<LoginInfo>();
            loginInfos.Add(loginInfo);
            int ret = Send(FUNCTIONID_LOGIN_GETLOGININFOS, requestID, loginInfos);
            loginInfos.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ͨ������ȡ��¼��Ϣ
        /// </summary>
        /// <param name="loginInfos">��¼��Ϣ</param>
        /// <param name="body">����</param>
        /// <param name="bodyLength">���峤��</param>
        public static int GetLoginInfos(List<LoginInfo> loginInfos, byte[] body, int bodyLength)
        {
            Binary br = new Binary();
            br.Write(body, bodyLength);
            int size = br.ReadInt();
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    LoginInfo loginInfo = new LoginInfo();
                    loginInfo.m_userID = br.ReadInt();
                    loginInfo.m_userName = br.ReadString();
                    loginInfo.m_passWord = br.ReadString();
                    loginInfo.m_nickName = br.ReadString();
                    loginInfo.m_type = br.ReadInt();
                    loginInfo.m_state = br.ReadInt();
                    loginInfo.m_sessionID = br.ReadInt();
                    loginInfos.Add(loginInfo);
                }
            }
            br.Close();
            return 1;
        }

        /// <summary>
        /// ��¼
        /// </summary>
        /// <param name="userName">�û���</param>
        /// <param name="passWord">����</param>
        /// <param name="requestID">����ID</param>
        /// <returns>״̬</returns>
        public int Login(String userName, String passWord, int requestID)
        {
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.m_userName = userName;
            loginInfo.m_passWord = passWord;
            List<LoginInfo> loginInfos = new List<LoginInfo>();
            loginInfos.Add(loginInfo);
            int ret = Send(FUNCTIONID_LOGIN_LOGIN, requestID, loginInfos);
            loginInfos.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// �ǳ�
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <returns>״̬</returns>
        public int Logout(int requestID)
        {
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.m_sessionID = SessionID;
            List<LoginInfo> loginInfos = new List<LoginInfo>();
            loginInfos.Add(loginInfo);
            int ret = Send(FUNCTIONID_LOGIN_LOGOUT, requestID, loginInfos);
            loginInfos.Clear();
            return ret > 0 ? 1 : 0;
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
        /// <param name="loginInfos">��¼��Ϣ����</param>
        /// <returns>״̬</returns>
        public int Send(int functionID, int requestID, List<LoginInfo> loginInfos)
        {
            Binary bw = new Binary();
            int loginInfosSize = loginInfos.Count;
            bw.WriteInt(loginInfosSize);
            for (int i = 0; i < loginInfosSize; i++)
            {
                LoginInfo loginInfo = loginInfos[i];
                bw.WriteInt(loginInfo.m_userID);
                bw.WriteString(loginInfo.m_userName);
                bw.WriteString(loginInfo.m_passWord);
                bw.WriteString(loginInfo.m_nickName);
                bw.WriteInt(loginInfo.m_type);
                bw.WriteInt(loginInfo.m_state);
                bw.WriteInt(loginInfo.m_sessionID);
            }
            byte[] bytes = bw.GetBytes();
            int ret = Send(new CMessage(GroupID, ServiceID, functionID, SessionID, requestID, m_socketID, 0, CompressType, bytes.Length, bytes));
            bw.Close();
            return ret;
        }
        #endregion
    }
}

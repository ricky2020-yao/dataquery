/*****************************************************************************\
*                                                                             *
* LoginService.cs -  Login service functions, types, and definitions          *
*                                                                             *
*               Version 1.00 ★                                               *
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
    /// 登录服务
    /// </summary>
    public class LoginService:BaseService
    {
        #region Lord 2016/3/24
        /// <summary>
        /// 创建登录服务
        /// </summary>
        public LoginService()
        {
            ServiceID = SERVICEID_LOGIN;
        }

        /// <summary>
        /// 指标服务ID
        /// </summary>
        public const int SERVICEID_LOGIN = 5;

        /// <summary>
        /// 用户登录的功能ID
        /// </summary>
        public const int FUNCTIONID_LOGIN_LOGIN = 0;

        /// <summary>
        /// 用户登出的功能ID
        /// </summary>
        public const int FUNCTIONID_LOGIN_LOGOUT = 1;

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public const int FUNCTIONID_LOGIN_GETLOGININFOS = 2;

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
        /// 获取登录信息
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <returns>状态</returns>
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
        /// 通过流获取登录信息
        /// </summary>
        /// <param name="loginInfos">登录信息</param>
        /// <param name="body">包体</param>
        /// <param name="bodyLength">包体长度</param>
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
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="requestID">请求ID</param>
        /// <returns>状态</returns>
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
        /// 登出
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <returns>状态</returns>
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
        /// <param name="loginInfos">登录信息集合</param>
        /// <returns>状态</returns>
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

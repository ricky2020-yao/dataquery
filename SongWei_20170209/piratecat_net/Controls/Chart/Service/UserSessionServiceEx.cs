/***********************************************************************************\
*                                                                                   *
* UserSessionServiceEx.cs - User session service functions, types, and definitions. *
*                                                                                   *
*               Version 1.00  ★★★                                                *
*                                                                                   *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.            *
*               Created by Lord 2016/1/20.                                          *
*                                                                                   *
************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// 用户会话服务扩展
    /// </summary>
    public class UserSessionServiceEx : UserSessionService
    {
        #region Lord 2016/10/4
        /// <summary>
        /// 创建用户会话服务扩展
        /// </summary>
        public UserSessionServiceEx()
        {
        }

        /// <summary>
        /// 会话列表
        /// </summary>
        private Dictionary<String, UserSession> m_sessions = new Dictionary<String, UserSession>();

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取列表的请求ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// 获取或设置是否已加载
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取操作请求ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_sessions.Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="session">会话</param>
        /// <returns>状态</returns>
        public int GetSession(String key, ref UserSession session)
        {
	        int state = 0;
            lock(m_sessions)
            {
                if(m_sessions.ContainsKey(key))
                {
                    session = m_sessions[key];
                    state=1;
                }
            }
            return state;
        }

        public override void OnReceive(CMessage message)
        {
            List<UserSession> sessions = new List<UserSession>();
            GetSessions(sessions, message.m_body, message.m_bodyLength);
            int sessionsSize = sessions.Count;
            switch (message.m_functionID)
            {
                case FUNCTIONID_USERSESSION_GETSESSIONS:
                    {
                        lock (m_sessions)
                        {
                            for (int i = 0; i < sessionsSize; i++)
                            {
                                UserSession session = sessions[i];
                                m_sessions[session.m_key] = session;
                                OnSessionChanged(session);
                            }
                        }
                        m_loaded = true;
                        break;
                    }
                case FUNCTIONID_USERSESSION_ADDSESSION:
                    {
                        lock (m_sessions)
                        {
                            for (int i = 0; i < sessionsSize; i++)
                            {
                                UserSession session = sessions[i];
                                m_sessions[session.m_key] = session;
                                OnSessionChanged(session);
                            }
                        }
                        break;
                    }
                case FUNCTIONID_USERSESSION_DELETESESSION:
                    {
                        lock (m_sessions)
                        {
                            for (int i = 0; i < sessionsSize; i++)
                            {
                                UserSession session = sessions[i];
                                if (m_sessions.ContainsKey(session.m_key))
                                {
                                    m_sessions.Remove(session.m_key);

                                    session.m_value = "";
                                    OnSessionChanged(session);
                                }
                            }
                            break;
                        }
                    }
            }
            base.OnReceive(message);
        }

        /// <summary>
        /// 会话改变方法
        /// </summary>
        /// <param name="session">会话</param>
        public virtual void OnSessionChanged(UserSession session)
        {
        }
        #endregion
    }
}

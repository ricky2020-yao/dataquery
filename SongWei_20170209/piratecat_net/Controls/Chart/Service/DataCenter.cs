/*****************************************************************************\
*                                                                             *
* DataCenter.cs -  Data center functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/10.                                    *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// 处理行情数据
    /// </summary>
    public class DataCenter
    {
        #region Lord 2016/3/10
        /// <summary>
        /// 登录信息
        /// </summary>
        private static LoginInfo m_loginInfo = new LoginInfo();

        /// <summary>
        /// 画线工具
        /// </summary>
        private static Dictionary<String, String> m_plots = new Dictionary<String, String>();

        /// <summary>
        /// 获取画线工具
        /// </summary>
        public static Dictionary<String, String> Plots
        {
            get { return m_plots; }
        }

        private static ChatServiceEx m_chatService = new ChatServiceEx();

        /// <summary>
        /// 获取或设置聊天服务
        /// </summary>
        public static ChatServiceEx ChatService
        {
            get { return m_chatService; }
        }

        private static ConnectService m_connectService = new ConnectService();

        /// <summary>
        /// 获取连接服务
        /// </summary>
        public static ConnectService ConnectService
        {
            get { return DataCenter.m_connectService; }
        }

        private static int m_connectState = 0;

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public static int ConnectState
        {
            get { return DataCenter.m_connectState; }
        }

        private static IndicatorServiceEx m_indicatorService = new IndicatorServiceEx();

        /// <summary>
        /// 获取或设置指标服务
        /// </summary>
        public static IndicatorServiceEx IndicatorService
        {
            get { return DataCenter.m_indicatorService; }
        }

        private static IndicatorLayoutServiceEx m_indicatorLayoutService = new IndicatorLayoutServiceEx();

        /// <summary>
        /// 获取或设置指标布局服务
        /// </summary>
        public static IndicatorLayoutServiceEx IndicatorLayoutService
        {
            get { return DataCenter.m_indicatorLayoutService; }
        }

        private static LoginService m_loginService = new LoginService();

        /// <summary>
        /// 获取登录服务
        /// </summary>
        public static LoginService LoginService
        {
            get { return DataCenter.m_loginService; }
        }

        private static MacroServiceEx m_macroService = new MacroServiceEx();

        /// <summary>
        /// 获取宏服务
        /// </summary>
        public static MacroServiceEx MacroService
        {
            get { return DataCenter.m_macroService; }
        }

        private static UIXmlEx m_mainUI;

        /// <summary>
        /// 设置主控件
        /// </summary>
        public static UIXmlEx MainUI
        {
            set { DataCenter.m_mainUI = value; }
        }

        private static ProxyService m_proxyService = new ProxyService();

        /// <summary>
        /// 获取代理连接信息
        /// </summary>
        public static ProxyService ProxyService
        {
            get { return DataCenter.m_proxyService; }
        }

        private static QuoteService m_quoteService = new QuoteService();

        /// <summary>
        /// 获取行情服务
        /// </summary>
        public static QuoteService QuoteService
        {
            get { return DataCenter.m_quoteService; }
        }

        private static SecurityFilterServiceEx m_securityFilterService = new SecurityFilterServiceEx();

        /// <summary>
        /// 获取条件选股服务
        /// </summary>
        public static SecurityFilterServiceEx SecurityFilterService
        {
            get { return DataCenter.m_securityFilterService; }
        }

        private static SecurityServiceEx m_securityService = new SecurityServiceEx();

        /// <summary>
        /// 获取证券服务
        /// </summary>
        public static SecurityServiceEx SecurityService
        {
            get { return DataCenter.m_securityService; }
        }

        private static ServerService m_serverService = new ServerService();

        /// <summary>
        /// 获取服务器服务
        /// </summary>
        public static ServerService ServerService
        {
            get { return DataCenter.m_serverService; }
        }

        private static int m_userID = -1;

        /// <summary>
        /// 获取用户ID
        /// </summary>
        public static int UserID
        {
            get { return m_userID; }
        }

        private static UserCookieService m_userCookieService = new UserCookieService();

        /// <summary>
        /// 用户Cookie服务
        /// </summary>
        public static UserCookieService UserCookieService
        {
            get { return DataCenter.m_userCookieService; }
        }

        private static UserSecurityServiceEx m_userSecurityService = new UserSecurityServiceEx();

        /// <summary>
        /// 获取或设置自选股服务
        /// </summary>
        public static UserSecurityServiceEx UserSecurityService
        {
            get { return DataCenter.m_userSecurityService; }
        }

        private static UserSessionServiceEx m_userSessionService = new UserSessionServiceEx();

        /// <summary>
        /// 获取用户会话服务
        /// </summary>
        public static UserSessionServiceEx UserSessionService
        {
            get { return DataCenter.m_userSessionService; }
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns>状态</returns>
        public static int Connect()
        {
            int socketID = -1;
            int qtSocketID = -1;
            List<ConnectInfo> connectInfos = new List<ConnectInfo>();
            m_connectService.GetConnects(connectInfos);
            int connectInfosSize = connectInfos.Count;
            for (int i = 0; i < connectInfosSize; i++)
            {
                ConnectInfo connectInfo = connectInfos[i];
                if (connectInfo.m_type == "主服务器")
                {
                    List<ProxyInfo> proxyInfos = new List<ProxyInfo>();
                    m_proxyService.GetProxyInfos(proxyInfos);
                    int proxyInfosSize = proxyInfos.Count;
                    for (int j = 0; j < proxyInfosSize; j++)
                    {
                        ProxyInfo pInfo = proxyInfos[i];
                        if (pInfo.m_use)
                        {
                            String ip = connectInfo.m_ip, proxyIP = "", proxyUserName = pInfo.m_name, proxyPwd = pInfo.m_pwd, proxyDomain = pInfo.m_domain;
					         int proxyPort = pInfo.m_port;
					        if(pInfo.m_type == 3)
					        {
						        //CurlHttp *http = CurlHttp::GetInstance();
						        //http->GetIeProxySet(&proxyIP, &proxyPort);
					        }
					        else
					        {
						        proxyIP = pInfo.m_ip;
					        }
					        int cType = 0;
					        if(pInfo.m_type == 1 || pInfo.m_type == 3)
					        {
						        cType = 1;
					        }
					        else if(pInfo.m_type == 2)
					        {
						        cType = 3;
					        }
                            socketID = BaseService.ConnectToServer(cType, ip, connectInfo.m_port, proxyIP, proxyPort, proxyUserName, proxyPwd, proxyDomain, 6);
                            qtSocketID = BaseService.ConnectToServer(cType, ip, connectInfo.m_port, proxyIP, proxyPort, proxyUserName, proxyPwd, proxyDomain, 6);
                            break;
                        }
                    }                  
                }
            }
            if (socketID > 0)
            {
                m_connectState = 1;
                m_loginService.SocketID = socketID;
                m_indicatorService.SocketID = socketID;
                m_quoteService.SocketID = qtSocketID;
                m_securityService.SocketID = socketID;
                m_chatService.SocketID = socketID;
                m_indicatorLayoutService.SocketID = socketID;
                m_macroService.SocketID = socketID;
                m_securityFilterService.SocketID = socketID;
                m_userSecurityService.SocketID = socketID;
                m_userSessionService.SocketID = socketID;
            }
            return socketID;
        }

        /// <summary>
        /// 取消连接
        /// </summary>
        /// <returns>状态</returns>
        public static int DisConnect()
        {
            m_connectState = 0;
            BaseService.CloseClient(m_loginService.SocketID);
            BaseService.CloseClient(m_quoteService.SocketID);
            return 1;
        }

        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// 获取用户目录
        /// </summary>
        /// <returns>用户目录</returns>
        public static String GetUserPath()
        {
            String userPath = Environment.GetEnvironmentVariable("LOCALAPPDATA");
	        if(!CFileA.IsDirectoryExist(userPath))
	        {
		        userPath = GetAppPath();
	        }
	        else
	        {
		        userPath += "\\piratecat2";
		        if(!CFileA.IsDirectoryExist(userPath))
		        {
			        CFileA.CreateDirectory(userPath);
		        }
	        }
	        return userPath;
            
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>加载状态</returns>
        public static int LoadData(int state)
        {
            ChatData chatData = new ChatData();
            m_chatService.Enter(m_chatService.RequestID, chatData);
            Indicator indicator = new Indicator();
            indicator.m_userID = m_userID;
            m_indicatorService.GetIndicators(m_indicatorService.GetListRequestID, indicator);
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            template.m_userID = m_userID;
            m_securityFilterService.GetTemplates(m_securityFilterService.GetListRequestID, template);
            UserSecurityVisitsCount visitsCount = new UserSecurityVisitsCount();
            visitsCount.m_userID = DataCenter.UserID;
            m_userSecurityService.GetVisitsCount(m_userSecurityService.GetListRequestID, visitsCount);
            UserSecurityCategory category = new UserSecurityCategory();
            category.m_userID = m_userID;
            m_userSecurityService.GetCategories(m_userSecurityService.GetListRequestID, category);
            IndicatorLayout indicatorLayout = new IndicatorLayout();
            indicatorLayout.m_userID = m_userID;
            m_indicatorLayoutService.GetLayouts(m_indicatorLayoutService.GetListRequestID, indicatorLayout);
            Macro macro = new Macro();
            macro.m_userID = m_userID;
            m_macroService.GetMacros(m_macroService.GetListRequestID, macro);
            UserSession userSession = new UserSession();
            userSession.m_userID = m_userID;
            m_userSessionService.GetSessions(m_userSessionService.GetListRequestID, userSession);
            if (state == 1)
            {
                m_securityService.LoadSecurities();
            }
            if (m_mainUI != null)
            {
                m_mainUI.LoadData();
            }
            return 0;
        }

        /// <summary>
        /// 重连方法
        /// </summary>
        private static void OnReconnect()
        {
            long downFLow = 0;
            while(true)
            {
                long newDownFLow = BaseService.DownFlow;
                if(downFLow > 0)
                {
                    if(downFLow == newDownFLow)
                    {
                        DisConnect();
                        while (true)
                        {
                            if(Connect() > 0)
                            {
                                int loginOutReqID = BaseService.GetRequestID();
                                int loginReqID = BaseService.GetRequestID();
                                m_loginService.Logout(loginOutReqID);
                                CMessage message = new CMessage();
                                m_loginService.RegisterWait(loginReqID, message);
                                m_loginService.Login(m_loginInfo.m_userName, m_loginInfo.m_passWord, loginReqID);
                                if(m_loginService.WaitMessage(loginReqID, 10000) > 0)
                                {
                                    List<LoginInfo> loginInfos = new List<LoginInfo>();
                                    LoginService.GetLoginInfos(loginInfos, message.m_body, message.m_bodyLength);
                                    int loginInfosSize = loginInfos.Count;
                                    if(loginInfosSize > 0)
                                    {
                                        LoginInfo loginInfo = loginInfos[0];
                                        if(loginInfo.m_state == 1)
                                        {
                                            SaveLoginInfo(loginInfo);
                                            LoadData(2);
                                        }
                                    }
                                    loginInfos.Clear();
                                }
                                else
                                {
                                    m_loginService.Logout(loginOutReqID);
                                }
                                break;
                            }
                            Thread.Sleep(1000);
                        }
                    }
                }
                downFLow = newDownFLow;
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// 读取所有的画线工具
        /// </summary>
        private static void ReadPlots()
        {
            String xmlPath = Path.Combine(GetAppPath(), "config\\Plots.xml");
            m_plots.Clear();
            if (File.Exists(xmlPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode rootNode = xmlDoc.DocumentElement;
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name.ToUpper() == "PLOT")
                    {
                        String name = String.Empty;
                        String text = String.Empty;
                        foreach (XmlNode childeNode in node.ChildNodes)
                        {
                            if (childeNode.Name.ToUpper() == "NAME")
                            {
                                name = childeNode.InnerText;
                            }
                            else if (childeNode.Name.ToUpper() == "TEXT")
                            {
                                text = childeNode.InnerText;
                            }
                        }
                        m_plots[name] = text;
                    }
                }
            }
        }

        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="sessionID">会话ID</param>
        public static void SaveLoginInfo(LoginInfo loginInfo)
        {
            if (m_userID == -1)
            {
                Thread hThread = new Thread(new ThreadStart(OnReconnect));
                hThread.IsBackground = true;
                hThread.Start();
            }
            m_loginInfo = loginInfo;
            m_userID = loginInfo.m_userID;
            m_chatService.SessionID = loginInfo.m_sessionID;
            m_indicatorLayoutService.SessionID = loginInfo.m_sessionID;
            m_indicatorService.SessionID = loginInfo.m_sessionID;
            m_loginService.SessionID = loginInfo.m_sessionID;
            m_macroService.SessionID = loginInfo.m_sessionID;
            m_quoteService.SessionID = loginInfo.m_sessionID;
            m_securityFilterService.SessionID = loginInfo.m_sessionID;
            m_securityService.SessionID = loginInfo.m_sessionID;
            m_userCookieService.UserID = m_userID;
            m_userSecurityService.SessionID = loginInfo.m_sessionID;
            m_userSessionService.SessionID = loginInfo.m_sessionID;
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="appPath">程序路径</param>
        public static void StartService()
        {
            BaseService.AddService(m_chatService);
            BaseService.AddService(m_indicatorLayoutService);
            BaseService.AddService(m_indicatorService);
            BaseService.AddService(m_loginService);
            BaseService.AddService(m_macroService);
            BaseService.AddService(m_quoteService);
            BaseService.AddService(m_securityFilterService);
            BaseService.AddService(m_securityService);
            BaseService.AddService(m_userSecurityService);
            BaseService.AddService(m_userSessionService);
            //List<BaseService> services = new List<BaseService>();
            //BaseService.GetServices(services);
            //int servicesSize = services.Count;
            //for (int i = 0; i < servicesSize; i++)
            //{
            //    BaseService service = services[i];
            //    service.CompressType = BaseService.COMPRESSTYPE_NONE;
            //}
            ReadPlots();
        }
        #endregion
    }
}

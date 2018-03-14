/*****************************************************************************\
*                                                                             *
* LoginWindow.cs - Login window functions, types, and definitions.            *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/9.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using OwLibCT;
using owplane;

namespace piratecat
{
    /// <summary>
    /// 登录窗体
    /// </summary>
    public class LoginWindow : WindowXmlEx
    {
        #region Lord 2016/4/9
        /// <summary>
        /// 创建登录窗体
        /// </summary>
        /// <param name="native">方法库</param>
        public LoginWindow(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\LoginWindow.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowLogin") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //注册点击事件
            RegisterEvents(m_window);
            m_loginService = DataCenter.LoginService;
            m_loginDataCallBackEvent = new ListenerMessageCallBack(LoginDataCallBack);
            m_loginService.RegisterListener(m_loginRequestID, m_loginDataCallBackEvent);
        }

        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// 登录数据回调事件
        /// </summary>
        private ListenerMessageCallBack m_loginDataCallBackEvent;

        /// <summary>
        /// 登录请求ID
        /// </summary>
        private int m_loginRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 登录服务
        /// </summary>
        private LoginService m_loginService;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 打飞机
        /// </summary>
        private Sky m_sky;

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置行情控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnClose" || name == "btnCancel")
                {
                    Environment.Exit(0);
                }
                else if (name == "btnLogin")
                {
                    Login();
                }
                else if (name == "lblNetSetting")
                {
                    ShowNetSetting();
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_loginService != null)
                {
                    if (m_loginDataCallBackEvent != null)
                    {
                        m_loginService.UnRegisterListener(m_loginRequestID, m_loginDataCallBackEvent);
                        m_loginDataCallBackEvent = null;
                    }
                    m_loginService = null;
                }
                if (m_window != null)
                {
                    m_window.UnRegisterEvent(m_invokeEvent, EVENTID.INVOKE);
                    m_invokeEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            int state = DataCenter.Connect();
            if (state == -1)
            {
                m_chart.ShowMessageBox("无法连接服务器!", "提示", 0);
                return;
            }
            TabControlA tabLogin = GetTabControl("tabLogin");
            TabPageA selectedTabpage = tabLogin.SelectedTabPage;
            TextBoxA txtUserName = GetTextBox("txtUserName");
            TextBoxA txtPassword = GetTextBox("txtPassword");
            String userName = txtUserName.Text;
            String passWord = txtPassword.Text;
            if(userName.Length == 0)
	        {
		        m_chart.ShowMessageBox("请输入邮箱！", "提示", 0);
		        return;
	        }
	        if(userName.IndexOf("@") == -1)
	        {
		        m_chart.ShowMessageBox("邮箱格式不符合要求！", "提示", 0);
		        return;
	        }
	        if(passWord.Length == 0)
	        {
		        m_chart.ShowMessageBox("请输入密码！", "提示", 0);
		        return;
	        }
            int ret = m_loginService.Login(userName, passWord, m_loginRequestID);
            if (ret != -1)
            {
                UserCookieService cookieService = DataCenter.UserCookieService;
                UserCookie cookie = new UserCookie();
                cookie.m_key = "LOGININFO";
                cookie.m_value = userName + "," + passWord;
                cookieService.AddCookie(cookie);
                m_window.Enabled = false;
            }
        }

        /// <summary>
        /// 登录数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void LoginDataCallBack(CMessage message)
        {
            OnLoginDataCallBack(message);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
            int state = (int)args;
            if (state >= 0)
            {
                DataCenter.LoadData(state);
                Close();
            }
            else
            {
                m_chart.ShowMessageBox("您输入的用户名或密码错误！", "提示", 0);
                m_window.Enabled = true;
            }
            m_native.Invalidate();
        }

        /// <summary>
        /// 登录数据回调处理方法
        /// </summary>
        /// <param name="message">消息</param>
        private void OnLoginDataCallBack(CMessage message)
        {
            List<LoginInfo> loginInfos = new List<LoginInfo>();
            LoginService.GetLoginInfos(loginInfos, message.m_body, message.m_bodyLength);
            if (loginInfos.Count > 0)
            {
                LoginInfo loginInfo = loginInfos[0];
                if (loginInfo.m_state == 1)
                {
                    DataCenter.SaveLoginInfo(loginInfo);
                    m_window.Invoke(loginInfo.m_type);
                }
                else
                {
                    m_window.Invoke(-1);
                }
            }
        }

        /// <summary>
        /// 窗体位置改变方法
        /// </summary>
        public void OnWindowLocationChanged()
        {
            if (m_sky != null)
            {
                m_sky.Location = new POINT(m_window.Left, m_window.Top - 210);
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ButtonA button = controls[i] as ButtonA;
                LinkLabelA linkLabel = controls[i] as LinkLabelA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (linkLabel != null)
                {
                    linkLabel.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            UserCookieService cookieService = DataCenter.UserCookieService;
            UserCookie cookie = new UserCookie();
            if (cookieService.GetCookie("LOGININFO", ref cookie) > 0)
            {
                String[] strs = cookie.m_value.Split(',');
                if (strs.Length >= 2)
                {
                    TextBoxA txtUserName = GetTextBox("txtUserName");
                    TextBoxA txtPassword = GetTextBox("txtPassword");
                    txtUserName.Text = strs[0];
                    txtPassword.Text = strs[1];
                }
            }
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            m_window.Invalidate();
            m_sky = new Sky();
            m_sky.Size = new SIZE(m_window.Width, 200);
            m_window.Frame.AddControl(m_sky);
            m_window.RegisterEvent(new ControlEvent(WindowLocationChanged), EVENTID.LOCATIONCHANGED);
        }

        /// <summary>
        /// 显示网络设置
        /// </summary>
        public void ShowNetSetting()
        {
            ConnectWindow connectWindow = new ConnectWindow(m_native);
            connectWindow.Chart = m_chart;
            connectWindow.Show();
        }

        /// <summary>
        /// 窗体位置改变方法
        /// </summary>
        /// <param name="sender"></param>
        private void WindowLocationChanged(object sender)
        {
            OnWindowLocationChanged();
        }
        #endregion
    }
}

/*****************************************************************************\
*                                                                             *
* LoginWindow.cs - Login window functions, types, and definitions.            *
*                                                                             *
*               Version 1.00  ����                                          *
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
    /// ��¼����
    /// </summary>
    public class LoginWindow : WindowXmlEx
    {
        #region Lord 2016/4/9
        /// <summary>
        /// ������¼����
        /// </summary>
        /// <param name="native">������</param>
        public LoginWindow(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\LoginWindow.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowLogin") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����¼�
            RegisterEvents(m_window);
            m_loginService = DataCenter.LoginService;
            m_loginDataCallBackEvent = new ListenerMessageCallBack(LoginDataCallBack);
            m_loginService.RegisterListener(m_loginRequestID, m_loginDataCallBackEvent);
        }

        /// <summary>
        /// ���ÿؼ������¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// ��¼���ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_loginDataCallBackEvent;

        /// <summary>
        /// ��¼����ID
        /// </summary>
        private int m_loginRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��¼����
        /// </summary>
        private LoginService m_loginService;

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// ��ɻ�
        /// </summary>
        private Sky m_sky;

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ����������ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// ��ť����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
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
        /// �رմ���
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// ���ٷ���
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
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="args">����</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// ��¼
        /// </summary>
        public void Login()
        {
            int state = DataCenter.Connect();
            if (state == -1)
            {
                m_chart.ShowMessageBox("�޷����ӷ�����!", "��ʾ", 0);
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
		        m_chart.ShowMessageBox("���������䣡", "��ʾ", 0);
		        return;
	        }
	        if(userName.IndexOf("@") == -1)
	        {
		        m_chart.ShowMessageBox("�����ʽ������Ҫ��", "��ʾ", 0);
		        return;
	        }
	        if(passWord.Length == 0)
	        {
		        m_chart.ShowMessageBox("���������룡", "��ʾ", 0);
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
        /// ��¼���ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void LoginDataCallBack(CMessage message)
        {
            OnLoginDataCallBack(message);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
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
                m_chart.ShowMessageBox("��������û������������", "��ʾ", 0);
                m_window.Enabled = true;
            }
            m_native.Invalidate();
        }

        /// <summary>
        /// ��¼���ݻص�������
        /// </summary>
        /// <param name="message">��Ϣ</param>
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
        /// ����λ�øı䷽��
        /// </summary>
        public void OnWindowLocationChanged()
        {
            if (m_sky != null)
            {
                m_sky.Location = new POINT(m_window.Left, m_window.Top - 210);
            }
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
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
        /// ��ʾ
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
        /// ��ʾ��������
        /// </summary>
        public void ShowNetSetting()
        {
            ConnectWindow connectWindow = new ConnectWindow(m_native);
            connectWindow.Chart = m_chart;
            connectWindow.Show();
        }

        /// <summary>
        /// ����λ�øı䷽��
        /// </summary>
        /// <param name="sender"></param>
        private void WindowLocationChanged(object sender)
        {
            OnWindowLocationChanged();
        }
        #endregion
    }
}

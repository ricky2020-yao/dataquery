/*****************************************************************************\
*                                                                             *
* ChatWindow.cs - Chart window functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/6/10.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// 聊天窗体
    /// </summary>
    public class ChatWindow : WindowXmlEx
    {
        #region 齐春友 2016/6/10
        /// <summary>
        /// 创建聊天窗体
        /// </summary>
        /// <param name="native">方法库</param>
        public ChatWindow(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\ChatWindow.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowChat") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //注册点击事件
            RegisterEvents(m_window);
            m_chatService = DataCenter.ChatService;
            m_chatDataCallBackEvent = new ListenerMessageCallBack(ChatDataCallBack);
            m_chatService.RegisterListener(m_chatService.RequestID, m_chatDataCallBackEvent);
        }

        /// <summary>
        /// 登录数据回调事件
        /// </summary>
        private ListenerMessageCallBack m_chatDataCallBackEvent;

        /// <summary>
        /// 登录服务
        /// </summary>
        private ChatServiceEx m_chatService;

        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

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
                    Close();
                }
                else if (name == "btnSend")
                {
                    Send();
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
                if (m_chatService != null)
                {
                    if (m_chatDataCallBackEvent != null)
                    {
                        m_chatService.UnRegisterListener(m_chatService.RequestID, m_chatDataCallBackEvent);
                        m_chatDataCallBackEvent = null;
                    }
                    m_chatService = null;
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
        /// 发送聊天内容
        /// </summary>
        public void Send()
        {
            TextBoxA txtData = GetTextBox("txtData");
            ChatData data = new ChatData();
            data.m_text = txtData.Text;
            data.m_type = 0;
            data.m_userID = DataCenter.UserID;
            m_chatService.Send(m_chatService.RequestID, data);
        }

        /// <summary>
        /// 聊天数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void ChatDataCallBack(CMessage message)
        {
            OnChatDataCallBack(message);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            List<ChatData> datas = new List<ChatData>();
            ChatService.GetChatDatas(datas, message.m_body, message.m_bodyLength);
            int size = datas.Count;
            String chatInfo = "";
            for(int i = 0; i < size; i++)
            {
                chatInfo = datas[i].m_text;
                Barrage brg = new Barrage();
                brg.Text = chatInfo;
                m_chart.BarrageDiv.AddBarrage(brg);
            }
            datas.Clear();
        }

        /// <summary>
        /// 聊天数据回调处理方法
        /// </summary>
        /// <param name="message">消息</param>
        private void OnChatDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
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
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
        }
        #endregion
    }
}

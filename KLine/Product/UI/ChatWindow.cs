/*****************************************************************************\
*                                                                             *
* ChatWindow.cs - Chart window functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ����                                          *
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

namespace KLine
{
    /// <summary>
    /// ���촰��
    /// </summary>
    public class ChatWindow : WindowXmlEx
    {
        /// <summary>
        /// �������촰��
        /// </summary>
        /// <param name="native">������</param>
        public ChatWindow(INativeBase native)
        {
            Load(native, "ChatWindow", "windowChat");
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����¼�
            RegisterEvents(m_window);
            m_chatService = DataCenter.ChatService;
            m_chatDataCallBackEvent = new ListenerMessageCallBack(ChatDataCallBack);
            m_chatService.RegisterListener(m_chatService.RequestID, m_chatDataCallBackEvent);
        }

        /// <summary>
        /// ��¼���ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_chatDataCallBackEvent;

        /// <summary>
        /// ��¼����
        /// </summary>
        private ChatServiceEx m_chatService;

        /// <summary>
        /// ���ÿؼ������¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ��������LordManager
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
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
                if (name == "btnSend")
                {
                    Send();
                }
            }
        }

        /// <summary>
        /// ���ٷ���
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
        /// ������������
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
        /// �������ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void ChatDataCallBack(CMessage message)
        {
            OnChatDataCallBack(message);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
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
                //m_lordManager.BarrageDiv.AddBarrage(brg);
            }
            datas.Clear();
        }

        /// <summary>
        /// �������ݻص�������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void OnChatDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
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
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }
    }
}

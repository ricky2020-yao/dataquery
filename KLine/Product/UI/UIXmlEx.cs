/*****************************************************************************\
*                                                                             *
* ChartUIXml.cs - Chart xml functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OwLib;
using System.Drawing;
using System.Windows.Forms;

namespace KLine
{
    /// <summary>
    /// ��Ʊͼ�οؼ�Xml����
    /// </summary>
    public class UIXmlEx:UIXml
    {
        #region Lord 2016/12/24
        private double m_scaleFactor = 1;

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// �����ؼ�
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="type">����</param>
        /// <returns>�ؼ�</returns>
        public override ControlA CreateControl(XmlNode node, String type)
        {
            if (type == "barragediv")
            {
                return new BarrageDiv();
            }
            else if (type == "floatdiv")
            {
                return new FloatDiv();
            }
            else if (type == "indexdiv")
            {
                return new IndexDiv();
            }
            else if (type == "klinediv")
            {
                return new ChartA();
            }
            else if (type == "latestdiv")
            {
                return new LatestDiv();
            }
            else if (type == "ribbonbutton")
            {
                return new RibbonButton();
            }
            else if (type == "windowex")
            {
                return new WindowEx();
            }
            else
            {
                return base.CreateControl(node, type);
            }
        }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        /// <param name="name">����</param>
        public virtual void ShowForm(String name)
        {
            MainForm mainForm = new MainForm();
            mainForm.Load(name);
            mainForm.Show();
        }

        /// <summary>
        /// �˳�����
        /// </summary>
        public virtual void Exit()
        {
        }

        /// <summary>
        /// ����XML
        /// </summary>
        /// <param name="xmlPath">XML��ַ</param>
        public virtual void Load(String xmlPath)
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual void LoadData()
        {
        }

        /// <summary>
        /// �������ųߴ�
        /// </summary>
        /// <param name="clientSize">�ͻ��˴�С</param>
        public void ResetScaleSize(SIZE clientSize)
        {
            INativeBase native = Native;
            if (native != null)
            {
                ControlHost host = native.Host;
                SIZE nativeSize = native.DisplaySize;
                List<ControlA> controls = native.GetControls();
                int controlsSize = controls.Count;
                for (int i = 0; i < controlsSize; i++)
                {
                    WindowFrameA frame = controls[i] as WindowFrameA;
                    if (frame != null)
                    {
                        WindowEx window = frame.GetControls()[0] as WindowEx;
                        if (window != null && !window.AnimateMoving)
                        {
                            POINT location = window.Location;
                            if (location.x < 10 || location.x > nativeSize.cx - 10)
                            {
                                location.x = 0;
                            }
                            if (location.y < 30 || location.y > nativeSize.cy - 30)
                            {
                                location.y = 0;
                            }
                            window.Location = location;
                        }
                    }
                }
                native.ScaleSize = new SIZE((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.Update();
            }
        }
        #endregion
    }

    /// <summary>
    /// ����XML��չ
    /// </summary>
    public class WindowXmlEx : UIXmlEx
    {
        /// <summary>
        /// ���ÿؼ������¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// Windows����
        /// </summary>
        protected BugHoleForm m_winForm;

        protected bool m_isWinForm;

        /// <summary>
        /// ��ȡ�������Ƿ���Windows����
        /// </summary>
        public bool IsWinForm
        {
            get { return m_isWinForm; }
            set { m_isWinForm = value; }
        }

        protected WindowEx m_window;

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
                if (m_window != null && control == m_window.CloseButton)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// �ر�
        /// </summary>
        public virtual void Close()
        {
            m_window.Invoke("close");
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_winForm != null && m_winForm.Window != null)
                {
                    m_winForm.Window = null;
                    m_winForm.Close();
                    m_winForm = null;
                }
                if (m_window != null)
                {
                    m_invokeEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// ���ؽ���
        /// </summary>
        public virtual void Load(INativeBase native, string xmlName, string windowName)
        {
            Native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\" + xmlName + ".html";
            Script = new GaiaScript(this);
            LoadFile(xmlPath, null);
            m_window = FindControl(windowName) as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����¼�
            RegisterEvents(m_window);
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
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public void OnInvoke(object args)
        {
            if (args != null && args.ToString() == "close")
            {
                Dispose();
                Native.Invalidate();
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
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public virtual void Show()
        {
            if (m_isWinForm)
            {
                SIZE windowSize = new SIZE();
                List<ControlA> controls = m_window.m_controls;
                int controlsSize = controls.Count;
                m_winForm = new BugHoleForm();
                for (int i = 0; i < controlsSize; i++)
                {
                    ControlA subControl = controls[i];
                    if (!(subControl is WindowButton))
                    {
                        windowSize = subControl.Size;
                        subControl.Margin = new PADDING(0, 0, 0, 0);
                        m_winForm.AddBugHole(Native, subControl);
                        break;
                    }
                }
                Native = m_winForm.Native;
                m_winForm.Text = m_window.Text;
                if (m_window.WindowState == WindowStateA.Max)
                {
                    m_winForm.WindowState = FormWindowState.Maximized;
                }
                else if (m_window.WindowState == WindowStateA.Min)
                {
                    m_winForm.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    m_winForm.ClientSize = new Size(windowSize.cx, windowSize.cy);
                }
                m_winForm.Window = this;
                m_winForm.Show();
            }
            else
            {
                m_window.Location = new POINT(-m_window.Width, -m_window.Height);
                m_window.AnimateShow(false);
                m_window.Invalidate();
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public virtual void ShowDialog()
        {
            if (m_isWinForm)
            {
                SIZE windowSize = new SIZE();
                List<ControlA> controls = m_window.m_controls;
                int controlsSize = controls.Count;
                m_winForm = new BugHoleForm();
                for (int i = 0; i < controlsSize; i++)
                {
                    ControlA subControl = controls[i];
                    if (!(subControl is WindowButton))
                    {
                        windowSize = subControl.Size;
                        subControl.Margin = new PADDING(0, 0, 0, 0);
                        m_winForm.AddBugHole(Native, subControl);
                        break;
                    }
                }
                Native = m_winForm.Native;
                m_winForm.Text = m_window.Text;
                if (m_window.WindowState == WindowStateA.Max)
                {
                    m_winForm.WindowState = FormWindowState.Maximized;
                }
                else if (m_window.WindowState == WindowStateA.Min)
                {
                    m_winForm.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    m_winForm.ClientSize = new Size(windowSize.cx, windowSize.cy);
                }
                m_winForm.Window = this;
                m_winForm.ShowDialog();
            }
            else
            {
                m_window.Location = new POINT(-m_window.Width, -m_window.Height);
                m_window.AnimateShow(true);
                m_window.Invalidate();
            }
        }
    }
}

/*****************************************************************************\
*                                                                             *
* ChartUIXml.cs - Chart xml functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 股票图形控件Xml解析
    /// </summary>
    public class UIXmlEx:UIXml
    {
        #region Lord 2016/12/24
        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
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
        /// 显示窗体
        /// </summary>
        /// <param name="name">名称</param>
        public virtual void ShowForm(String name)
        {
            MainForm mainForm = new MainForm();
            mainForm.Load(name);
            mainForm.Show();
        }

        /// <summary>
        /// 退出方法
        /// </summary>
        public virtual void Exit()
        {
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML地址</param>
        public virtual void Load(String xmlPath)
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
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
    /// 窗体XML扩展
    /// </summary>
    public class WindowXmlEx : UIXmlEx
    {
        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// Windows窗体
        /// </summary>
        protected BugHoleForm m_winForm;

        protected bool m_isWinForm;

        /// <summary>
        /// 获取或设置是否是Windows窗体
        /// </summary>
        public bool IsWinForm
        {
            get { return m_isWinForm; }
            set { m_isWinForm = value; }
        }

        protected WindowEx m_window;

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
                if (m_window != null && control == m_window.CloseButton)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            m_window.Invoke("close");
        }

        /// <summary>
        /// 销毁方法
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
        /// 加载界面
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
            //注册点击事件
            RegisterEvents(m_window);
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
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
            if (args != null && args.ToString() == "close")
            {
                Dispose();
                Native.Invalidate();
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
        /// 显示
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

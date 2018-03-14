/*****************************************************************************\
*                                                                             *
* WPFHost.cs - Wpf control host, types, and definitions                       *
*                                                                             *
*               Version 4.00 ★★★★★                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using owchart_wpf;
using piratecat;

namespace OwLib
{
    /// <summary>
    /// WPF控件管理器
    /// </summary>
    public class WPFHost:ControlHost
    {
        #region Lord 2016/11/25
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int IntersectRect(ref RECT lpDestRect, ref RECT lpSrc1Rect, ref RECT lpSrc2Rect);
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int UnionRect(ref RECT lpDestRect, ref RECT lpSrc1Rect, ref RECT lpSrc2Rect);
        public delegate void TimerProc(IntPtr hWnd, uint nMsg, int nIDEvent, int dwTime);
        [DllImport("user32")]
        public static extern int SetTimer(IntPtr hWnd, int nIDEvent, int uElapse, TimerProc CB);
        [DllImport("user32")]
        public static extern int KillTimer(IntPtr hWnd, int nIDEvent);
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern int GetKeyState(int nVirtKey);

        /// <summary>
        /// 调用控件线程方法的参数
        /// </summary>
        private Dictionary<int, object> m_invokeArgs = new Dictionary<int, object>();

        /// <summary>
        /// 调用控件线程方法的控件
        /// </summary>
        private Dictionary<int, ControlA> m_invokeControls = new Dictionary<int, ControlA>();

        /// <summary>
        /// 线程方法序列号
        /// </summary>
        private int m_invokeSerialID = 0;

        private bool m_allowOperate = true;

        /// <summary>
        /// 获取或设置是否可以操作
        /// </summary>
        public override bool AllowOperate
        {
            get { return m_allowOperate; }
            set { m_allowOperate = value; }
        }

        /// <summary>
        /// 获取活设置是否允许局部绘图
        /// </summary>
        public override bool AllowPartialPaint
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        private Control m_container;

        /// <summary>
        /// 获取或设置容器
        /// </summary>
        public Control Container
        {
            get { return m_container; }
            set { m_container = value; }
        }

        private INativeBase m_native;

        /// <summary>
        /// 获取或设置方法库
        /// </summary>
        public override INativeBase Native
        {
            get { return m_native; }
            set { m_native = value; }
        }

        private int m_pInvokeMsgID = 0x0401;

        /// <summary>
        /// 获取或设置调用线程方法的消息ID
        /// </summary>
        public int PInvokeMsgID
        {
            get { return m_pInvokeMsgID; }
            set { m_pInvokeMsgID = value; }
        }

        private Render m_render;

        /// <summary>
        /// 获取或设置绘图控件
        /// </summary>
        public Render Render
        {
            get { return m_render; }
            set { m_render = value; }
        }

        private IntPtr m_windowHwnd = IntPtr.Zero;

        /// <summary>
        /// 获取或设置窗体的句柄
        /// </summary>
        public IntPtr WindowHwnd
        {
            get { return m_windowHwnd; }
            set { m_windowHwnd = value; }
        }

        /// <summary>
        /// 在控件的线程中调用方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="args">参数</param>
        public override void BeginInvoke(ControlA control, object args)
        {
            lock (m_invokeArgs)
            {
                m_invokeArgs[m_invokeSerialID] = args;
            }
            lock (m_invokeControls)
            {
                m_invokeControls[m_invokeSerialID] = control;
            }
            SendMessage(m_windowHwnd, m_pInvokeMsgID, m_invokeSerialID, 0);
            m_invokeSerialID++;
        }

        /// <summary>
        /// 复制文本
        /// </summary>
        /// <param name="text">文本</param>
        public override void Copy(String text)
        {
            Clipboard.SetText(text);
        }

        /// <summary>
        /// 创建内部控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="clsid">控件标识</param>
        /// <returns>内部控件</returns>
        public override ControlA CreateInternalControl(ControlA parent, String clsid)
        {
            //日历控件
            CalendarA calendar = parent as CalendarA;
            if (calendar != null)
            {
                if (clsid == "datetitle")
                {
                    return new DateTitle(calendar);
                }
                else if (clsid == "headdiv")
                {
                    HeadDiv headDiv = new HeadDiv(calendar);
                    headDiv.Width = parent.Width;
                    headDiv.Dock = DockStyleA.Top;
                    return headDiv;
                }
                else if (clsid == "lastbutton")
                {
                    return new ArrowButton(calendar);
                }
                else if (clsid == "nextbutton")
                {
                    ArrowButton nextBtn = new ArrowButton(calendar);
                    nextBtn.ToLast = false;
                    return nextBtn;
                }
            }
            //分割层
            SplitLayoutDivA splitLayoutDiv = parent as SplitLayoutDivA;
            if (splitLayoutDiv != null)
            {
                if (clsid == "splitter")
                {
                    ButtonA splitter = new ButtonA();
                    splitter.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    splitter.Size = new SIZE(5, 5);
                    return splitter;
                }
            }
            //滚动条
            ScrollBarA scrollBar = parent as ScrollBarA;
            if (scrollBar != null)
            {
                scrollBar.BackColor = COLOR.EMPTY;
                if (clsid == "addbutton")
                {
                    RibbonButton addButton = new RibbonButton();
                    addButton.Size = new SIZE(15, 15);
                    if (scrollBar is HScrollBarA)
                    {
                        addButton.ArrowType = 2;
                    }
                    else if (scrollBar is VScrollBarA)
                    {
                        addButton.ArrowType = 4;
                    }
                    return addButton;
                }
                else if (clsid == "backbutton")
                {
                    ButtonA backButton = new ButtonA();
                    return backButton;
                }
                else if (clsid == "scrollbutton")
                {
                    ButtonA scrollButton = new ButtonA();
                    scrollButton.AllowDrag = true;
                    scrollButton.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    scrollButton.BorderColor = CDraw.PCOLORS_LINECOLOR3;
                    return scrollButton;
                }
                else if (clsid == "reducebutton")
                {
                    RibbonButton reduceButton = new RibbonButton();
                    reduceButton.Size = new SIZE(15, 15);
                    if (scrollBar is HScrollBarA)
                    {
                        reduceButton.ArrowType = 1;
                    }
                    else if (scrollBar is VScrollBarA)
                    {
                        reduceButton.ArrowType = 3;
                    }
                    return reduceButton;
                }
            }
            //页夹
            TabPageA tabPage = parent as TabPageA;
            if (tabPage != null)
            {
                if (clsid == "headerbutton")
                {
                    RibbonButton button = new RibbonButton();
                    button.AllowDrag = true;
                    SIZE size = new SIZE(100, 20);
                    button.Size = size;
                    return button;
                }
            }
            //下拉列表
            ComboBoxA comboBox = parent as ComboBoxA;
            if (comboBox != null)
            {
                if (clsid == "dropdownbutton")
                {
                    RibbonButton dropDownButton = new RibbonButton();
                    dropDownButton.ArrowType = 4;
                    dropDownButton.DisplayOffset = false;
                    int width = comboBox.Width;
                    int height = comboBox.Height;
                    POINT location = new POINT(width - 20, 0);
                    dropDownButton.Location = location;
                    SIZE size = new SIZE(20, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu")
                {
                    ComboBoxMenu comboBoxMenu = new ComboBoxMenu();
                    comboBoxMenu.ComboBox = comboBox;
                    comboBoxMenu.Popup = true;
                    SIZE size = new SIZE(100, 200);
                    comboBoxMenu.Size = size;
                    return comboBoxMenu;
                }
            }
            //日期选择
            DatePickerA datePicker = parent as DatePickerA;
            if (datePicker != null)
            {
                if (clsid == "dropdownbutton")
                {
                    RibbonButton dropDownButton = new RibbonButton();
                    dropDownButton.ArrowType = 4;
                    dropDownButton.DisplayOffset = false;
                    int width = datePicker.Width;
                    int height = datePicker.Height;
                    POINT location = new POINT(width - 16, 0);
                    dropDownButton.Location = location;
                    SIZE size = new SIZE(16, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu")
                {
                    MenuA dropDownMenu = new MenuA();
                    dropDownMenu.Padding = new PADDING(1);
                    dropDownMenu.Popup = true;
                    SIZE size = new SIZE(200, 200);
                    dropDownMenu.Size = size;
                    return dropDownMenu;
                }
            }
            //数字选择
            SpinA spin = parent as SpinA;
            if (spin != null)
            {
                if (clsid == "downbutton")
                {
                    RibbonButton downButton = new RibbonButton();
                    downButton.ArrowType = 4;
                    downButton.DisplayOffset = false;
                    SIZE size = new SIZE(16, 16);
                    downButton.Size = size;
                    return downButton;
                }
                else if (clsid == "upbutton")
                {
                    RibbonButton upButton = new RibbonButton();
                    upButton.ArrowType = 3;
                    upButton.DisplayOffset = false;
                    SIZE size = new SIZE(16, 16);
                    upButton.Size = size;
                    return upButton;
                }
            }
            //容器层
            DivA div = parent as DivA;
            if (div != null)
            {
                if (clsid == "hscrollbar")
                {
                    HScrollBarA hScrollBar = new HScrollBarA();
                    hScrollBar.Visible = false;
                    hScrollBar.Size = new SIZE(15, 15);
                    return hScrollBar;
                }
                else if (clsid == "vscrollbar")
                {
                    VScrollBarA vScrollBar = new VScrollBarA();
                    vScrollBar.Visible = false;
                    vScrollBar.Size = new SIZE(15, 15);
                    return vScrollBar;
                }
            }
            //表格
            GridA grid = parent as GridA;
            if (grid != null)
            {
                if (clsid == "edittextbox")
                {
                    TextBoxA textBox = new TextBoxA();
                    textBox.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    return textBox;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }

        /// <summary>
        /// 获取光标
        /// </summary>
        /// <returns>光标</returns>
        public override CursorsA GetCursor()
        {
            if (m_container != null)
            {
                Cursor cursor = m_container.Cursor;
                if (cursor == Cursors.Arrow)
                {
                    return CursorsA.Arrow;
                }
                else if (cursor == Cursors.AppStarting)
                {
                    return CursorsA.AppStarting;
                }
                else if (cursor == Cursors.Cross)
                {
                    return CursorsA.Cross;
                }
                else if (cursor == Cursors.Hand)
                {
                    return CursorsA.Hand;
                }
                else if (cursor == Cursors.Help)
                {
                    return CursorsA.Help;
                }
                else if (cursor == Cursors.IBeam)
                {
                    return CursorsA.IBeam;
                }
                else if (cursor == Cursors.No)
                {
                    return CursorsA.No;
                }
                else if (cursor == Cursors.SizeAll)
                {
                    return CursorsA.SizeAll;
                }
                else if (cursor == Cursors.SizeNESW)
                {
                    return CursorsA.SizeNESW;
                }
                else if (cursor == Cursors.SizeNS)
                {
                    return CursorsA.SizeNS;
                }
                else if (cursor == Cursors.SizeNWSE)
                {
                    return CursorsA.SizeNWSE;
                }
                else if (cursor == Cursors.SizeWE)
                {
                    return CursorsA.SizeWE;
                }
                else if (cursor == Cursors.UpArrow)
                {
                    return CursorsA.UpArrow;
                }
            }
            return CursorsA.Arrow;
        }

        /// <summary>
        /// 获取系统按键代码
        /// </summary>
        /// <param name="key">按键</param>
        /// <returns>系统按键代码</returns>
        public int GetKeyCode(Key key)
        {
            return KeyInterop.VirtualKeyFromKey(key);  
        }

        /// <summary>
        /// 获取矩形相交区
        /// </summary>
        /// <param name="lpDestRect">相交矩形</param>
        /// <param name="lpSrc1Rect">矩形1</param>
        /// <param name="lpSrc2Rect">矩形2</param>
        /// <returns>是否相交</returns>
        public override int GetIntersectRect(ref RECT lpDestRect, ref RECT lpSrc1Rect, ref RECT lpSrc2Rect)
        {
            return IntersectRect(ref lpDestRect, ref lpSrc1Rect, ref lpSrc2Rect);
        }

        /// <summary>
        /// 获取鼠标位置
        /// </summary>
        /// <returns>坐标</returns>
        public override POINT GetMousePoint()
        {
            POINT mp = new POINT();
            GetCursorPos(out mp);
            Point clientPoint = m_container.PointFromScreen(new Point(mp.x, mp.y));
            mp.x = (int)clientPoint.X;
            mp.y = (int)clientPoint.Y;
            return mp;
        }

        /// <summary>
        /// 获取尺寸
        /// </summary>
        /// <returns>大小</returns>
        public override SIZE GetSize()
        {
            SIZE size = new SIZE();
            if (m_native != null)
            {
                size.cx = (int)(m_container.RenderSize.Width);
                size.cy = (int)(m_container.RenderSize.Height);
            }
            return size;
        }

        /// <summary>
        /// 获取联合矩形
        /// </summary>
        /// <param name="lpDestRect">相交矩形</param>
        /// <param name="lpSrc1Rect">矩形1</param>
        /// <param name="lpSrc2Rect">矩形2</param>
        /// <returns>是否相交</returns>
        public override int GetUnionRect(ref RECT lpDestRect, ref RECT lpSrc1Rect, ref RECT lpSrc2Rect)
        {
            return UnionRect(ref lpDestRect, ref lpSrc1Rect, ref lpSrc2Rect);
        }

        /// <summary>
        /// 刷新绘图
        /// </summary>
        public override void Invalidate()
        {
            if (m_render != null)
            {
                try
                {
                    m_render.InvalidateVisual();
                }
                catch (Exception ex)
                {
                    SendMessage(m_windowHwnd, 0x0402, 0, 0);
                }
            }
        }

        /// <summary>
        /// 刷新绘图
        /// </summary>
        /// <param name="rect">区域</param>
        public override void Invalidate(RECT rect)
        {
            Invalidate();
        }
        /// <summary>
        /// 在控件的线程中调用方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="args">参数</param>
        public override void Invoke(ControlA control, object args)
        {
            lock (m_invokeArgs)
            {
                m_invokeArgs[m_invokeSerialID] = args;
            }
            lock (m_invokeControls)
            {
                m_invokeControls[m_invokeSerialID] = control;
            }
            PostMessage(m_windowHwnd, m_pInvokeMsgID, m_invokeSerialID, 0);
            m_invokeSerialID++;
        }


        /// <summary>
        /// 获取按键的状态
        /// </summary>
        /// <param name="key">按键</param>
        /// <returns>状态</returns>
        public override bool IsKeyPress(int key)
        {
            int state = GetKeyState(key) & 0x8000;
            if (state > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 调用控件线程的方法
        /// </summary>
        /// <param name="invokeSerialID">消息ID</param>
        public virtual void OnInvoke(int invokeSerialID)
        {
            object args = null;
            ControlA control = null;
            lock (m_invokeArgs)
            {
                if (m_invokeArgs.ContainsKey(invokeSerialID))
                {
                    args = m_invokeArgs[invokeSerialID];
                    m_invokeArgs.Remove(invokeSerialID);
                }
            }
            lock (m_invokeControls)
            {
                if (m_invokeControls.ContainsKey(invokeSerialID))
                {
                    control = m_invokeControls[invokeSerialID];
                    m_invokeControls.Remove(invokeSerialID);
                    m_invokeArgs[m_invokeSerialID] = control;
                }
            }
            if (control != null)
            {
                control.OnInvoke(args);
            }
        }

        /// <summary>
        /// 获取粘贴文本
        /// </summary>
        /// <returns>文本</returns>
        public override String Paste()
        {
            return Clipboard.GetText();
        }

        /// <summary>
        /// 设置光标
        /// </summary>
        /// <param name="cursor">光标</param>
        public override void SetCursor(CursorsA cursor)
        {
            if (m_container != null)
            {
                CursorsA csr = cursor;
                if (csr == CursorsA.Arrow)
                {
                    m_container.Cursor = Cursors.Arrow;
                }
                else if (csr == CursorsA.AppStarting)
                {
                    m_container.Cursor = Cursors.AppStarting;
                }
                else if (csr == CursorsA.Cross)
                {
                    m_container.Cursor = Cursors.Cross;
                }
                else if (csr == CursorsA.Hand)
                {
                    m_container.Cursor = Cursors.Hand;
                }
                else if (csr == CursorsA.Help)
                {
                    m_container.Cursor = Cursors.Help;
                }
                else if (csr == CursorsA.IBeam)
                {
                    m_container.Cursor = Cursors.IBeam;
                }
                else if (csr == CursorsA.No)
                {
                    m_container.Cursor = Cursors.No;
                }
                else if (csr == CursorsA.SizeAll)
                {
                    m_container.Cursor = Cursors.SizeAll;
                }
                else if (csr == CursorsA.SizeNESW)
                {
                    m_container.Cursor = Cursors.SizeNESW;
                }
                else if (csr == CursorsA.SizeNS)
                {
                    m_container.Cursor = Cursors.SizeNS;
                }
                else if (csr == CursorsA.SizeNWSE)
                {
                    m_container.Cursor = Cursors.SizeNWSE;
                }
                else if (csr == CursorsA.SizeWE)
                {
                    m_container.Cursor = Cursors.SizeWE;
                }
                else if (csr == CursorsA.UpArrow)
                {
                    m_container.Cursor = Cursors.UpArrow;
                }
            }
        }

        /// <summary>
        /// 开启秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        /// <param name="interval">间隔</param>
        public override void StartTimer(int timerID, int interval)
        {
            SetTimer(m_windowHwnd, timerID, interval, null);
        }

        /// <summary>
        /// 停止秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void StopTimer(int timerID)
        {
            KillTimer(m_windowHwnd, timerID);
        }
        #endregion
    }
}

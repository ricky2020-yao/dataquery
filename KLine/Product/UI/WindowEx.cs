/*****************************************************************************\
*                                                                             *
* WindowEx.cs - Window functions, types, and definitions.                     *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/5/8.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Drawing;
using KLine;

namespace OwLib
{
    /// <summary>
    /// ����״̬
    /// </summary>
    public enum WindowStateA
    {
        /// <summary>
        /// ��ͨ
        /// </summary>
        Normal,
        /// <summary>
        /// ���
        /// </summary>
        Max,
        /// <summary>
        /// ��С��
        /// </summary>
        Min
    }

    /// <summary>
    /// �ػ洰��
    /// </summary>
    public class WindowEx : WindowA
    {
        #region Lord 2016/5/8
        /// <summary>
        /// ��������
        /// </summary>
        public WindowEx()
        {
            BackColor = COLOR.EMPTY;
            BorderColor = CDraw.PCOLORS_LINECOLOR3;
            CaptionHeight = 25;
            Font = new FONT("΢���ź�", 14, false, false, false);
            ForeColor = COLOR.EMPTY;
            Opacity = 0;
            ShadowColor = CDraw.PCOLORS_BACKCOLOR5;
            ShadowSize = 0;
        }

        /// <summary>
        /// �ƶ�����
        /// </summary>
        private int m_animateDirection = -1;

        /// <summary>
        /// ��������
        /// </summary>
        private int m_animateType = 0;

        /// <summary>
        /// ��ͨ�ߴ�
        /// </summary>
        private POINT m_normalLocation;

        /// <summary>
        /// ��ͨ�ߴ�
        /// </summary>
        private SIZE m_normalSize;

        /// <summary>
        /// �������
        /// </summary>
        private Random m_rd = new Random();

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private bool m_animateMoving;

        /// <summary>
        /// ��ȡ�Ƿ����ڶ����ƶ�
        /// </summary>
        public bool AnimateMoving
        {
            get { return m_animateMoving; }
        }

        private WindowButton m_closeButton;

        /// <summary>
        /// ��ȡ�����ùرհ�ť
        /// </summary>
        public WindowButton CloseButton
        {
            get { return m_closeButton; }
            set { m_closeButton = value; }
        }

        private bool m_isChildWindow;

        /// <summary>
        /// ��ȡ�������Ƿ��Ӵ���
        /// </summary>
        public bool IsChildWindow
        {
            get { return m_isChildWindow; }
            set { m_isChildWindow = value; }
        }

        private WindowButton m_maxOrRestoreButton;

        /// <summary>
        /// ��ȡ��������󻯰�ť
        /// </summary>
        public WindowButton MaxOrRestoreButton
        {
            get { return m_maxOrRestoreButton; }
            set { m_maxOrRestoreButton = value; }
        }

        private WindowButton m_minButton;

        /// <summary>
        /// ��ȡ��������С����ť
        /// </summary>
        public WindowButton MinButton
        {
            get { return m_minButton; }
            set { m_minButton = value; }
        }

        private bool m_showMaxBox = true;

        /// <summary>
        /// ��ȡ�������Ƿ���ʾ��󻯰�ť
        /// </summary>
        public bool ShowMaxBox
        {
            get { return m_showMaxBox; }
            set
            {
                m_showMaxBox = value;
                if (m_maxOrRestoreButton != null)
                {
                    m_maxOrRestoreButton.Visible = value;
                }
            }
        }

        private bool m_showMinBox = true;

        /// <summary>
        /// ��ȡ�������Ƿ���ʾ��С����ť
        /// </summary>
        public bool ShowMinBox
        {
            get 
            {
                return m_showMinBox;
            }
            set 
            {
                m_showMinBox = value;
                if (m_minButton != null)
                {
                    m_minButton.Visible = value;
                }
            }
        }

        private WindowStateA m_windowState = WindowStateA.Normal;

        /// <summary>
        /// ��ȡ�����õ�ǰ��״̬
        /// </summary>
        public WindowStateA WindowState
        {
            get { return m_windowState; }
            set { m_windowState = value; }
        }

        /// <summary>
        /// �Զ�����ʽ����
        /// </summary>
        public void AnimateHide()
        {
            m_animateType = 1;
            INativeBase native = Native;
            ControlHost host = native.Host;
            m_animateDirection = m_rd.Next(0, 4);
            StartTimer(m_timerID, 10);
            m_animateMoving = true;
            host.AllowOperate = false;
        }

        /// <summary>
        /// �Զ�����ʽ��ʾ
        /// </summary>
        /// <param name="showDialog">�Ƿ�Ի����</param>
        public void AnimateShow(bool showDialog)
        {
            m_animateType = 0;
            INativeBase native = Native;
            ControlHost host = native.Host;
            SIZE nativeSize = native.DisplaySize;
            int width = Width, height = Height, mx = (nativeSize.cx - width) / 2, my = (nativeSize.cy - height) / 2, x = mx, y = my;
            m_animateDirection = m_rd.Next(0, 4);
            switch (m_animateDirection)
            {
                case 0:
                    x = -width;
                    break;
                case 1:
                    x = nativeSize.cx;
                    break;
                case 2:
                    y = -height;
                    break;
                case 3:
                    y = nativeSize.cy;
                    break;
            }
            POINT location = new POINT(x, y);
            Location = location;
            if (showDialog)
            {
                ShowDialog();
            }
            else
            {
                Show();
            }
            Update();
            StartTimer(m_timerID, 10);
            m_animateMoving = true;
            host.AllowOperate = false;
        }

        /// <summary>
        /// �����ť����
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
                if (name == "btnMaxOrRestore")
                {
                    MaxOrRestore();
                }
                else if (name == "btnMin")
                {
                    Min();
                }
            }
        }

        /// <summary>
        /// ���ٿؼ�����
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_animateMoving = false;
                StopTimer(m_timerID);
            }
            base.Dispose();
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="type">����</param>
        public override void GetProperty(string name, ref string value, ref string type)
        {
            if (name == "showmaxbox")
            {
                type = "bool";
                value = CStr.ConvertBoolToStr(ShowMaxBox);
            }
            else if (name == "showminbox")
            {
                type = "bool";
                value = CStr.ConvertBoolToStr(ShowMinBox);
            }
            else if (name == "windowstate")
            {
                type = "enum:WindowStateA";
                WindowStateA windowState = WindowState;
                if (windowState == WindowStateA.Max)
                {
                    value = "Max";
                }
                else if (windowState == WindowStateA.Min)
                {
                    value = "Min";
                }
                else if (windowState == WindowStateA.Normal)
                {
                    value = "Normal";
                }
            }
            else
            {
                base.GetProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        /// <returns>�����б�</returns>
        public override List<string> GetPropertyNames()
        {
            List<String> propertyNames = base.GetPropertyNames();
            propertyNames.AddRange(new String[] { "ShowMaxBox", "ShowMinBox", "WindowState" });
            return propertyNames;
        }

        /// <summary>
        /// �ؼ���ӷ���
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();
            if (m_closeButton == null)
            {
                m_closeButton = new WindowButton();
                m_closeButton.Name = "btnClose";
                SIZE buttonSize = new SIZE(20, 20);
                m_closeButton.Size = buttonSize;
                AddControl(m_closeButton);
            }
            if (m_maxOrRestoreButton == null)
            {
                m_maxOrRestoreButton = new WindowButton();
                m_maxOrRestoreButton.Name = "btnMaxOrRestore";
                m_maxOrRestoreButton.Style = WindowButtonStyle.Max;
                SIZE buttonSize = new SIZE(20, 20);
                m_maxOrRestoreButton.Size = buttonSize;
                AddControl(m_maxOrRestoreButton);
                m_maxOrRestoreButton.Visible = m_showMaxBox;
                m_maxOrRestoreButton.RegisterEvent(new ControlMouseEvent(ClickButton), EVENTID.CLICK);
            }
            if (m_minButton == null)
            {
                m_minButton = new WindowButton();
                m_minButton.Name = "btnMin";
                m_minButton.Style = WindowButtonStyle.Min;
                SIZE buttonSize = new SIZE(20, 20);
                m_minButton.Size = buttonSize;
                AddControl(m_minButton);
                m_minButton.Visible = m_showMinBox;
                m_minButton.RegisterEvent(new ControlMouseEvent(ClickButton), EVENTID.CLICK);
            }
        }

        /// <summary>
        /// �϶���ʼ����
        /// </summary>
        /// <param name="startOffset">ƫ��������</param>
        public override void OnDragReady(ref POINT startOffset)
        {
            startOffset.x = 0;
            startOffset.y = 0;
        }

        /// <summary>
        /// �ػ�ǰ������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            RECT rect = new RECT(0, 0, width, height);
            long backColor = CDraw.PCOLORS_WINDOWBACKCOLOR;
            long foreColor = CDraw.PCOLORS_WINDOWFORECOLOR;
            if (paint.SupportTransparent())
            {
                backColor = CDraw.PCOLORS_WINDOWBACKCOLOR2;
            }
            int captionHeight = CaptionHeight;
            RECT contentRect = rect;
            contentRect.top += captionHeight;
            contentRect.bottom -= 6;
            contentRect.left += 6;
            contentRect.right -= 6;
            paint.BeginPath();
            paint.AddRect(contentRect);
            paint.ExcludeClipPath();
            paint.ClosePath();
            paint.FillRoundRect(backColor, rect, 6);
            paint.SetClip(clipRect);
            if (contentRect.right - contentRect.left > 0 && contentRect.bottom - contentRect.top > 0)
            {
                contentRect.top -= 1;
                contentRect.left -= 1;
                contentRect.right += 1;
                contentRect.bottom += 1;
                paint.FillRect(CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR, contentRect);
            }
            CDraw.DrawText(paint, Text, foreColor, Font, 5, 3);
        }

        /// <summary>
        /// �����
        /// </summary>
        /// <param name="timerID">���ID</param>
        public override void OnTimer(int timerID)
        {
            base.OnTimer(timerID);
            if (m_timerID == timerID)
            {
                INativeBase native = Native;
                ControlHost host = native.Host;
                SIZE nativeSize = native.DisplaySize;
                int x = Left, y = Top, width = Width, height = Height;
                if (m_animateType == 0)
                {
                    int xSub = nativeSize.cx / 4;
                    int ySub = nativeSize.cy / 4;
                    int mx = (nativeSize.cx - width) / 2;
                    int my = (nativeSize.cy - height) / 2;
                    float opacity = Opacity;
                    opacity += 0.1F;
                    if (opacity > 1)
                    {
                        opacity = 1;
                    }
                    Opacity = opacity;
                    bool stop = false;
                    switch (m_animateDirection)
                    {
                        //��������
                        case 0:
                            if (x + xSub >= mx)
                            {
                                x = mx;
                                stop = true;
                            }
                            else
                            {
                                x += xSub;
                            }
                            break;
                        //��������
                        case 1:
                            if (x - xSub <= mx)
                            {
                                x = mx;
                                stop = true;
                            }
                            else
                            {
                                x -= xSub;
                            }
                            break;
                        //��������
                        case 2:
                            if (y + ySub >= my)
                            {
                                y = my;
                                stop = true;
                            }
                            else
                            {
                                y += ySub;
                            }
                            break;
                        //��������
                        case 3:
                            if (y - ySub <= my)
                            {
                                y = my;
                                stop = true;
                            }
                            else
                            {
                                y -= ySub;
                            }
                            break;
                    }
                    if (stop)
                    {
                        Opacity = 1;
                        m_animateMoving = false;
                        StopTimer(m_timerID);
                        host.AllowOperate = true;
                    }
                }
                else
                {
                    int xSub = nativeSize.cx / 4;
                    int ySub = nativeSize.cy / 4;
                    float opacity = Opacity;
                    opacity -= 0.1F;
                    if (opacity < 0)
                    {
                        opacity = 0;
                    }
                    Opacity = opacity;
                    bool stop = false;
                    switch (m_animateDirection)
                    {
                        //��������
                        case 0:
                            if (x - xSub <= -width)
                            {
                                x = 0;
                                stop = true;
                            }
                            else
                            {
                                x -= xSub;
                            }
                            break;
                        //��������
                        case 1:
                            if (x + xSub >= nativeSize.cx)
                            {
                                x = 0;
                                stop = true;
                            }
                            else
                            {
                                x += xSub;
                            }
                            break;
                        //��������
                        case 2:
                            if (y - ySub <= -height)
                            {
                                y = 0;
                                stop = true;
                            }
                            else
                            {
                                y -= ySub;
                            }
                            break;
                        //��������
                        case 3:
                            if (y + ySub >= nativeSize.cy)
                            {
                                y = 0;
                                stop = true;
                            }
                            else
                            {
                                y += ySub;
                            }
                            break;
                    }
                    if (stop)
                    {
                        Opacity = 0;
                        m_animateMoving = false;
                        StopTimer(m_timerID);
                        host.AllowOperate = true;
                        Hide();
                    }
                }
                POINT location = new POINT(x, y);
                Location = location;
                native.Invalidate();
            }
        }

        /// <summary>
        /// ��󻯻�ָ�
        /// </summary>
        public void Max()
        {
            m_normalLocation = Location;
            m_normalSize = Size;
            Dock = DockStyleA.Fill;
            m_windowState = WindowStateA.Max;
            POINT maxLocation = new POINT(0, 0);
            Location = maxLocation;
            SIZE maxSize = Native.DisplaySize;
            Size = maxSize;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Restore;
            Native.Update();
            Native.Invalidate();
        }

        /// <summary>
        /// ��󻯻�ָ�
        /// </summary>
        public void MaxOrRestore()
        {
            if (m_windowState == WindowStateA.Normal)
            {
                Max();
            }
            else
            {
                Restore();
            }
        }

        /// <summary>
        /// ��С��
        /// </summary>
        public void Min()
        {
            m_normalLocation = Location;
            m_normalSize = Size;
            Dock = DockStyleA.None;
            m_windowState = WindowStateA.Min;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Restore;
            SIZE minSize = new SIZE(150, CaptionHeight);
            Size = minSize;
            Update();
            Native.Invalidate();
        }

        /// <summary>
        /// �ָ�
        /// </summary>
        public void Restore()
        {
            Dock = DockStyleA.None;
            m_windowState = WindowStateA.Normal;
            Location = m_normalLocation;
            Size = m_normalSize;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Max;
            Native.Update();
            Native.Invalidate();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        public override void SetProperty(string name, string value)
        {
            if (name == "showmaxbox")
            {
                ShowMaxBox = CStr.ConvertStrToBool(value);
            }
            else if (name == "showminbox")
            {
                ShowMinBox = CStr.ConvertStrToBool(value);
            }
            else if (name == "windowstate")
            {
                String str = value.ToLower();
                if (str == "max")
                {
                    WindowState = WindowStateA.Max;
                }
                else if (str == "min")
                {
                    WindowState = WindowStateA.Min;
                }
                else if (str == "normal")
                {
                    WindowState = WindowStateA.Normal;
                }
            }
            else
            {
                base.SetProperty(name, value);
            }
        }

        /// <summary>
        /// ���ָı䷽��
        /// </summary>
        public override void Update()
        {
            base.Update();
            int width = Width;
            if (m_closeButton != null)
            {
                POINT location = new POINT(width - 26, 2);
                m_closeButton.Location = location;
            }
            if (m_maxOrRestoreButton != null && m_maxOrRestoreButton.Visible)
            {
                POINT location = new POINT(width - 48, 2);
                m_maxOrRestoreButton.Location = location;
            }
            if (m_minButton != null && m_minButton.Visible)
            {
                POINT location = new POINT(width - 70, 2);
                if (m_maxOrRestoreButton != null && !m_maxOrRestoreButton.Visible)
                {
                    location.x = width - 48;
                }
                m_minButton.Location = location;
            }
        }
        #endregion
    }
}

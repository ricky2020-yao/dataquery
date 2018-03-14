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

namespace piratecat
{
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
            CaptionHeight = 23;
            Font = new FONT("SimSun", 14, true, false, false);
            ForeColor = COLOR.EMPTY;
            Opacity = 0;
            ResourcePath = DataCenter.GetAppPath() + "\\config";
            ShadowColor = CDraw.PCOLORS_BACKCOLOR5;
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

        private RibbonButton m_closeButton;

        /// <summary>
        /// ��ȡ�����ùرհ�ť
        /// </summary>
        public RibbonButton CloseButton
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
        /// �ؼ���ӷ���
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();
            if (m_closeButton == null)
            {
                m_closeButton = new RibbonButton();
                m_closeButton.IsClose = true;
                m_closeButton.Name = "btnClose";
                SIZE buttonSize = new SIZE(22, 22);
                m_closeButton.Size = buttonSize;
                AddControl(m_closeButton);
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
            RECT hRect = new RECT(0, 0, width, captionHeight);
            paint.FillRect(backColor, hRect);
            RECT lRect = new RECT(0, captionHeight, 5, height);
            paint.FillRect(backColor, lRect);
            RECT rRect = new RECT(width - 5, captionHeight, width, height);
            paint.FillRect(backColor, rRect);
            RECT bRect = new RECT(0, height - 5, width, height);
            paint.FillRect(backColor, bRect);
            RECT contentRect = rect;
            contentRect.top += captionHeight;
            contentRect.bottom -= 5;
            contentRect.left += 5;
            contentRect.right -= 5;
            if (contentRect.right - contentRect.left > 0 && contentRect.bottom - contentRect.top > 0)
            {
                paint.FillRect(CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR, contentRect);
            }
            CDraw.DrawText(paint, Text, foreColor, Font, 5, 5);
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
        /// ���ָı䷽��
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (m_closeButton != null)
            {
                POINT location = new POINT(Width - 26, 2);
                m_closeButton.Location = location;
            }
        }
        #endregion
    }
}

/*****************************************************************************\
*                                                                             *
* WindowEx.cs - Window functions, types, and definitions.                     *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 重绘窗体
    /// </summary>
    public class WindowEx : WindowA
    {
        #region Lord 2016/5/8
        /// <summary>
        /// 创建窗体
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
        /// 移动方向
        /// </summary>
        private int m_animateDirection = -1;

        /// <summary>
        /// 动画类型
        /// </summary>
        private int m_animateType = 0;

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random m_rd = new Random();

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private bool m_animateMoving;

        /// <summary>
        /// 获取是否正在动画移动
        /// </summary>
        public bool AnimateMoving
        {
            get { return m_animateMoving; }
        }

        private RibbonButton m_closeButton;

        /// <summary>
        /// 获取或设置关闭按钮
        /// </summary>
        public RibbonButton CloseButton
        {
            get { return m_closeButton; }
            set { m_closeButton = value; }
        }

        private bool m_isChildWindow;

        /// <summary>
        /// 获取或设置是否子窗体
        /// </summary>
        public bool IsChildWindow
        {
            get { return m_isChildWindow; }
            set { m_isChildWindow = value; }
        }

        /// <summary>
        /// 以动画形式隐藏
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
        /// 以动画形式显示
        /// </summary>
        /// <param name="showDialog">是否对话框打开</param>
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
        /// 销毁控件方法
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
        /// 控件添加方法
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
        /// 拖动开始方法
        /// </summary>
        /// <param name="startOffset">偏移启动量</param>
        public override void OnDragReady(ref POINT startOffset)
        {
            startOffset.x = 0;
            startOffset.y = 0;
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
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
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
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
                        //从左向右
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
                        //从右向左
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
                        //从上往下
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
                        //从下往上
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
                        //从右向左
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
                        //从左向右
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
                        //从下往上
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
                        //从上往下
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
        /// 布局改变方法
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

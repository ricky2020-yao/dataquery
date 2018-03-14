/*******************************************************************************\
*                                                                               *
* RibbonButton.cs - Ribbon button functions, types, and definitions.            *
*                                                                               *
*               Version 6.00 ★                                                 *
*                                                                               *
*               Copyright (c) 2016-2016, Order. All rights reserved.            *
*               Created by Lord.                                                *
*                                                                               *
********************************************************************************/

using KLine;
using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// 透明按钮
    /// </summary>
    public class RibbonButton : ButtonA
    {
        #region Lord 2016/12/2
        /// <summary>
        /// 创建透明按钮
        /// </summary>
        public RibbonButton()
        {
            BackColor = COLOR.EMPTY;
            BorderColor = COLOR.EMPTY;
        }

        private int m_arrowType;

        /// <summary>
        /// 获取或设置箭头类型
        /// </summary>
        public int ArrowType
        {
            get { return m_arrowType; }
            set { m_arrowType = value; }
        }

        private bool m_isClose;

        /// <summary>
        /// 获取或设置是否是关闭按钮
        /// </summary>
        public bool IsClose
        {
            get { return m_isClose; }
            set { m_isClose = value; }
        }

        /// <summary>
        /// 获取或设置是否选中
        /// </summary>
        public bool Selected
        {
            get
            {
                ControlA parent = Parent;
                if (parent != null)
                {
                    TabControlA tabControl = parent as TabControlA;
                    if (tabControl != null)
                    {
                        TabPageA selectedTabPage = tabControl.SelectedTabPage;
                        if (selectedTabPage != null)
                        {
                            if (this == selectedTabPage.HeaderButton)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取要绘制的前景色
        /// </summary>
        /// <returns>前景色</returns>
        protected override long GetPaintingForeColor()
        {
            if (Enabled)
            {
                return CDraw.PCOLORS_FORECOLOR;
            }
            else
            {
                return CDraw.PCOLORS_FORECOLOR2;
            }
        }

        /// <summary>
        /// 重绘背景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            int mw = width / 2;
            int mh = height / 2;
            RECT drawRect = new RECT(0, 0, width, height);
            if (m_isClose)
            {
                long lineColor = CDraw.PCOLORS_LINECOLOR;
                RECT ellipseRect = new RECT(1, 1, width - 2, height - 2);
                paint.FillEllipse(CDraw.PCOLORS_UPCOLOR, ellipseRect);
                paint.DrawLine(lineColor, 2, 0, 4, 4, width - 7, height - 7);
                paint.DrawLine(lineColor, 2, 0, 4, height - 7, width - 7, 3);
            }
            else
            {
                paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, drawRect, 0, 90);
                paint.DrawRect(CDraw.PCOLORS_LINECOLOR, 1, 0, drawRect);
            }
            if (m_arrowType > 0)
            {
                POINT[] points = new POINT[3];
                switch (m_arrowType)
                {
                    //向左
                    case 1:
                        points[0] = new POINT(mw - 4, mh);
                        points[1] = new POINT(mw + 4, mh - 4);
                        points[2] = new POINT(mw + 4, mh + 4);
                        break;
                    //向右
                    case 2:
                        points[0] = new POINT(mw + 4, mh);
                        points[1] = new POINT(mw - 4, mh - 4);
                        points[2] = new POINT(mw - 4, mh + 4);
                        break;
                    //向上
                    case 3:
                        points[0] = new POINT(mw, mh - 4);
                        points[1] = new POINT(mw - 4, mh + 4);
                        points[2] = new POINT(mw + 4, mh + 4);
                        break;
                    //向下
                    case 4:
                        points[0] = new POINT(mw, mh + 4);
                        points[1] = new POINT(mw - 4, mh - 4);
                        points[2] = new POINT(mw + 4, mh - 4);
                        break;
                }
                paint.FillPolygon(CDraw.PCOLORS_FORECOLOR, points);
            }
            //绘制选中效果
            if (paint.SupportTransparent())
            {
                if (Selected)
                {
                    paint.FillRect(CDraw.PCOLORS_BACKCOLOR2, drawRect);
                }
                else
                {
                    INativeBase native = Native;
                    if (this == native.PushedControl)
                    {
                        paint.FillRect(CDraw.PCOLORS_BACKCOLOR4, drawRect);
                    }
                    else if (this == native.HoveredControl)
                    {
                        paint.FillRect(CDraw.PCOLORS_BACKCOLOR3, drawRect);
                    }
                }
            }
        }
        #endregion
    }
}

/*******************************************************************************\
*                                                                               *
* RibbonButton.cs - Ribbon button functions, types, and definitions.            *
*                                                                               *
*               Version 6.00 ��                                                 *
*                                                                               *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.        *
*               Created by Lord 2016/12/2.                                      *
*                                                                               *
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace piratecat
{
    /// <summary>
    /// ͸����ť
    /// </summary>
    public class RibbonButton : ButtonA
    {
        #region Lord 2016/12/2
        /// <summary>
        /// ����͸����ť
        /// </summary>
        public RibbonButton()
        {
            BackColor = COLOR.EMPTY;
            BorderColor = COLOR.EMPTY;
        }

        private int m_arrowType;

        /// <summary>
        /// ��ȡ�����ü�ͷ����
        /// </summary>
        public int ArrowType
        {
            get { return m_arrowType; }
            set { m_arrowType = value; }
        }

        private bool m_isClose;

        /// <summary>
        /// ��ȡ�������Ƿ��ǹرհ�ť
        /// </summary>
        public bool IsClose
        {
            get { return m_isClose; }
            set { m_isClose = value; }
        }

        /// <summary>
        /// ��ȡ�������Ƿ�ѡ��
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
        /// ��ȡҪ���Ƶ�ǰ��ɫ
        /// </summary>
        /// <returns>ǰ��ɫ</returns>
        protected override long GetPaintingForeColor()
        {
            if (Enabled)
            {
                return CDraw.PCOLORS_FORECOLOR;
            }
            else
            {
                return CDraw.PCOLORS_FORECOLOR5;
            }
        }

        /// <summary>
        /// �ػ汳��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width, height = Height;
            int mw = width / 2, mh = height / 2;
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
                int cornerRadius = 4;
                if (m_arrowType > 0)
                {
                    cornerRadius = 0;
                }
                ControlA parent = Parent;
                if (parent != null)
                {
                    TabControlA tabControl = parent as TabControlA;
                    if (tabControl != null)
                    {
                        cornerRadius = 0;
                    }
                }
                paint.DrawGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, drawRect, cornerRadius, 90);
                paint.DrawRoundRect(CDraw.PCOLORS_LINECOLOR3, 1, 0, drawRect, cornerRadius);
            }
            if (m_arrowType > 0)
            {
                POINT[] points = new POINT[3];
                int ts = Math.Min(mw, mh) / 2;
                switch (m_arrowType)
                {
                    //����
                    case 1:
                        points[0] = new POINT(mw - ts, mh);
                        points[1] = new POINT(mw + ts, mh - ts);
                        points[2] = new POINT(mw + ts, mh + ts);
                        break;
                    //����
                    case 2:
                        points[0] = new POINT(mw + ts, mh);
                        points[1] = new POINT(mw - ts, mh - ts);
                        points[2] = new POINT(mw - ts, mh + ts);
                        break;
                    //����
                    case 3:
                        points[0] = new POINT(mw, mh - ts);
                        points[1] = new POINT(mw - ts, mh + ts);
                        points[2] = new POINT(mw + ts, mh + ts);
                        break;
                    //����
                    case 4:
                        points[0] = new POINT(mw, mh + ts);
                        points[1] = new POINT(mw - ts, mh - ts);
                        points[2] = new POINT(mw + ts, mh - ts);
                        break;
                }
                paint.FillPolygon(CDraw.PCOLORS_FORECOLOR, points);
            }
            //����ѡ��Ч��
            if (paint.SupportTransparent())
            {
                INativeBase native = Native;
                if (Selected)
                {
                    paint.FillRect(CDraw.PCOLORS_BACKCOLOR2, drawRect);
                }
                else if (this == native.PushedControl)
                {
                    paint.FillRect(CDraw.PCOLORS_BACKCOLOR6, drawRect);
                }
                else if (this == native.HoveredControl)
                {
                    paint.FillRect(CDraw.PCOLORS_BACKCOLOR5, drawRect);
                }
            }
        }
        #endregion
    }
}

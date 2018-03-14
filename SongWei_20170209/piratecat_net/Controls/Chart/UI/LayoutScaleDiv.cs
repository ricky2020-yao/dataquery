/*****************************************************************************\
*                                                                             *
* LayoutScaleDiv.cs - Layout scale div functions, types, and definitions.     *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/5/25.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace piratecat
{
    /// <summary>
    /// 指标布局缩略图
    /// </summary>
    public class LayoutScaleDiv : ControlA
    {
        #region Lord 2016/5/25
        /// <summary>
        /// 创建指标布局缩略图
        /// </summary>
        public LayoutScaleDiv()
        {
        }

        /// <summary>
        /// 系统颜色
        /// </summary>
        private long[] m_sysColors = new long[] { COLOR.ARGB(255, 255, 255), COLOR.ARGB(255,255,0), COLOR.ARGB(255, 0, 255),
            COLOR.ARGB(0, 255, 0), COLOR.ARGB(82, 255, 255), COLOR.ARGB(255, 82, 82) };

        private ChartA m_chart;

        /// <summary>
        /// 获取或设置股票控件
        /// </summary>
        public ChartA Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private String m_layoutStr;

        /// <summary>
        /// 获取或设置布局字符串
        /// </summary>
        public String LayoutStr
        {
            get { return m_layoutStr; }
            set { m_layoutStr = value; }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaint(CPaint paint, RECT clipRect)
        {
            if (m_layoutStr != null && m_layoutStr.Length > 0)
            {
                int width = Width, height = Height;
                FONT font = Font;
                String[] strs = m_layoutStr.Split(new String[] { "\r\n" }, StringSplitOptions.None);
                int strsSize = strs.Length;
                int divPos = 0, divTop = 0, divHeight = 0;
                List<String> inds = new List<String>();
                List<String> viceInds = new List<String>();
                SIZE sizeK = new SIZE(15, 16);
                for (int i = 0; i < strsSize; i++)
                {
                    String subStr = strs[i];
                    if (subStr.Length > 0)
                    {
                        String[] subStrs = subStr.Split(new String[] { "=" }, StringSplitOptions.None);
                        if (subStrs.Length >= 2)
                        {
                            String name = subStrs[0];
                            String value = subStrs[1];
                            //高度
                            if (name == "HEIGHT")
                            {
                                divHeight = height * CStr.ConvertStrToInt(value) / 100;
                            }
                            //主要指标
                            else if (name == "IND")
                            {
                                inds.Add(value);
                            }
                            //辅图指标
                            else if (name == "VICEIND")
                            {
                                viceInds.Add(value + "(副)");
                            }
                        }
                    }
                    if (subStr == "DIV" || i == strsSize - 1)
                    {
                        if (i != 0)
                        {
                            //绘制图层
                            RECT divRect = new RECT(0, divTop, width, divTop + divHeight);
                            paint.FillRect(CDraw.PCOLORS_BACKCOLOR3, divRect);
                            paint.DrawLine(CDraw.PCOLORS_LINECOLOR5, 1, 0, 0, divRect.bottom - 1, width, divRect.bottom - 1);
                            List<String> paintStrs = new List<String>();
                            if (divPos == 0)
                            {
                                paintStrs.Add("主K线");
                            }
                            else if (divPos == 1)
                            {
                                paintStrs.Add("成交量");
                            }
                            //添加指标
                            int indsSize = inds.Count;
                            for (int j = 0; j < indsSize; j++)
                            {
                                paintStrs.Add(inds[j]);
                            }
                            int viceIndsSize = viceInds.Count;
                            for (int j = 0; j < viceIndsSize; j++)
                            {
                                paintStrs.Add(viceInds[j]);
                            }
                            //绘制文字
                            int paintStrsSize = paintStrs.Count;
                            int left = 5, top = divTop + 5;
                            for (int j = 0; j < paintStrsSize; j++)
                            {
                                long color = m_sysColors[j % 6];
                                SIZE textSize = paint.TextSize(paintStrs[j], font);
                                int rectCsX = left;
                                int rectCsY = top - 2;
                                int rectCsW = sizeK.cx;
                                int rectCsH = sizeK.cy;
                                //K线
                                if (j == 0 && divPos == 0)
                                {
                                    paint.DrawLine(CDraw.PCOLORS_DOWNCOLOR, 1, 0, rectCsX + 4, rectCsY + 6, rectCsX + 4, rectCsY + rectCsH - 2);
                                    paint.DrawLine(CDraw.PCOLORS_UPCOLOR, 1, 0, rectCsX + 9, rectCsY + 2, rectCsX + 9, rectCsY + rectCsH - 4);
                                    paint.FillRect(CDraw.PCOLORS_DOWNCOLOR, new RECT(rectCsX + 3, rectCsY + 8, rectCsX + 6, rectCsY + 13));
                                    paint.FillRect(CDraw.PCOLORS_UPCOLOR, new RECT(rectCsX + 8, rectCsY + 4, rectCsX + 11, rectCsY + 9));
                                }
                                //成交量
                                else if (j == 0 && divPos == 1)
                                {
                                    paint.FillRect(CDraw.PCOLORS_UPCOLOR, new RECT(rectCsX + 1, rectCsY + 10, rectCsX + 4, rectCsY + rectCsH - 1));
                                    paint.FillRect(CDraw.PCOLORS_UPCOLOR, new RECT(rectCsX + 6, rectCsY + 3, rectCsX + 9, rectCsY + rectCsH - 1));
                                    paint.FillRect(CDraw.PCOLORS_UPCOLOR, new RECT(rectCsX + 11, rectCsY + 8, rectCsX + 14, rectCsY + rectCsH - 1));
                                }
                                //指标
                                else
                                {
                                    paint.DrawLine(color, 1, 0, rectCsX + 2, rectCsY + 5, rectCsX + 12, rectCsY + 1);
                                    paint.DrawLine(color, 1, 0, rectCsX + 2, rectCsY + 10, rectCsX + 12, rectCsY + 6);
                                    paint.DrawLine(color, 1, 0, rectCsX + 2, rectCsY + 15, rectCsX + 12, rectCsY + 11);
                                }
                                left += rectCsW + 2;
                                RECT textRect = new RECT(left, top, left + textSize.cx, top + textSize.cy);
                                paint.DrawText(paintStrs[j], color, font, textRect);
                                left += textSize.cx + 10;
                                if (left > width - 5)
                                {
                                    left = 5;
                                    top = 20;
                                }
                            }
                            paintStrs.Clear();
                            inds.Clear();
                            viceInds.Clear();
                            divPos++;
                            divTop += divHeight;
                        }
                    }
                }
                inds.Clear();
                viceInds.Clear();
            }
        }
        #endregion
    }
}

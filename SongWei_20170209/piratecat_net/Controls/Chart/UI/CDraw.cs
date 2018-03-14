/*****************************************************************************\
*                                                                             *
* CDraw.cs -    Draw functions, types, and definitions.                       *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/21.                                    *
*                                                                             *
******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace piratecat
{
    /// <summary>
    /// 绘图类
    /// </summary>
    public class CDraw
    {
        #region Lord 2016/3/21
        /// <summary>
        /// 用户自定义色
        /// </summary>
        public const long PCOLORS_USERCOLOR = -200000000100;

        /// <summary>
        /// 背景色
        /// </summary>
        public const long PCOLORS_BACKCOLOR = PCOLORS_USERCOLOR - 1;

        /// <summary>
        /// 背景色2
        /// </summary>
        public const long PCOLORS_BACKCOLOR2 = PCOLORS_USERCOLOR - 2;

        /// <summary>
        /// 背景色3
        /// </summary>
        public const long PCOLORS_BACKCOLOR3 = PCOLORS_USERCOLOR - 3;

        /// <summary>
        /// 背景色4
        /// </summary>
        public const long PCOLORS_BACKCOLOR4 = PCOLORS_USERCOLOR - 4;

        /// <summary>
        /// 背景色5
        /// </summary>
        public const long PCOLORS_BACKCOLOR5 = PCOLORS_USERCOLOR - 5;

        /// <summary>
        /// 背景色6
        /// </summary>
        public const long PCOLORS_BACKCOLOR6 = PCOLORS_USERCOLOR - 6;

        /// <summary>
        /// 背景色7
        /// </summary>
        public const long PCOLORS_BACKCOLOR7 = PCOLORS_USERCOLOR - 7;

        /// <summary>
        /// 前景色
        /// </summary>
        public const long PCOLORS_FORECOLOR = PCOLORS_USERCOLOR - 100;

        /// <summary>
        /// 前景色2
        /// </summary>
        public const long PCOLORS_FORECOLOR2 = PCOLORS_USERCOLOR - 101;

        /// <summary>
        /// 前景色3
        /// </summary>
        public const long PCOLORS_FORECOLOR3 = PCOLORS_USERCOLOR - 102;

        /// <summary>
        /// 前景色4
        /// </summary>
        public const long PCOLORS_FORECOLOR4 = PCOLORS_USERCOLOR - 103;

        /// <summary>
        /// 前景色5
        /// </summary>
        public const long PCOLORS_FORECOLOR5 = PCOLORS_USERCOLOR - 104;

        /// <summary>
        /// 前景色6
        /// </summary>
        public const long PCOLORS_FORECOLOR6 = PCOLORS_USERCOLOR - 105;

        /// <summary>
        /// 前景色7
        /// </summary>
        public const long PCOLORS_FORECOLOR7 = PCOLORS_USERCOLOR - 106;

        /// <summary>
        /// 前景色8
        /// </summary>
        public const long PCOLORS_FORECOLOR8 = PCOLORS_USERCOLOR - 107;

        /// <summary>
        /// 前景色9
        /// </summary>
        public const long PCOLORS_FORECOLOR9 = PCOLORS_USERCOLOR - 108;

        /// <summary>
        /// 前景色10
        /// </summary>
        public const long PCOLORS_FORECOLOR10 = PCOLORS_USERCOLOR - 109;

        /// <summary>
        /// 前景色11
        /// </summary>
        public const long PCOLORS_FORECOLOR11 = PCOLORS_USERCOLOR - 110;

        /// <summary>
        /// 线的颜色
        /// </summary>
        public const long PCOLORS_LINECOLOR = PCOLORS_USERCOLOR - 200;

        /// <summary>
        /// 线的颜色2
        /// </summary>
        public const long PCOLORS_LINECOLOR2 = PCOLORS_USERCOLOR - 201;

        /// <summary>
        /// 线的颜色3
        /// </summary>
        public const long PCOLORS_LINECOLOR3 = PCOLORS_USERCOLOR - 202;

        /// <summary>
        /// 线的颜色4
        /// </summary>
        public const long PCOLORS_LINECOLOR4 = PCOLORS_USERCOLOR - 203;

        /// <summary>
        /// 线的颜色5
        /// </summary>
        public const long PCOLORS_LINECOLOR5 = PCOLORS_USERCOLOR - 204;

        /// <summary>
        /// 平盘色
        /// </summary>
        public const long PCOLORS_MIDCOLOR = PCOLORS_USERCOLOR - 300;

        /// <summary>
        /// 上涨色
        /// </summary>
        public const long PCOLORS_UPCOLOR = PCOLORS_USERCOLOR - 301;

        /// <summary>
        /// 下跌色
        /// </summary>
        public const long PCOLORS_DOWNCOLOR = PCOLORS_USERCOLOR - 302;

        /// <summary>
        /// 下跌色2
        /// </summary>
        public const long PCOLORS_DOWNCOLOR2 = PCOLORS_USERCOLOR - 303;

        /// <summary>
        /// 下跌色3
        /// </summary>
        public const long PCOLORS_DOWNCOLOR3 = PCOLORS_USERCOLOR - 304;

        /// <summary>
        /// 选中行颜色
        /// </summary>
        public const long PCOLORS_SELECTEDROWCOLOR = PCOLORS_USERCOLOR - 400;

        /// <summary>
        /// 悬停行颜色
        /// </summary>
        public const long PCOLORS_HOVEREDROWCOLOR = PCOLORS_USERCOLOR - 401;

        /// <summary>
        /// 窗体前景色
        /// </summary>
        public const long PCOLORS_WINDOWFORECOLOR = PCOLORS_USERCOLOR - 500;

        /// <summary>
        /// 窗体背景色
        /// </summary>
        public const long PCOLORS_WINDOWBACKCOLOR = PCOLORS_USERCOLOR - 501;

        /// <summary>
        /// 窗体背景色2
        /// </summary>
        public const long PCOLORS_WINDOWBACKCOLOR2 = PCOLORS_USERCOLOR - 502;

        /// <summary>
        /// 窗体内容背景色
        /// </summary>
        public const long PCOLORS_WINDOWCONTENTBACKCOLOR = PCOLORS_USERCOLOR - 503;

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public static SIZE DrawText(CPaint paint, String text, long dwPenColor, FONT font, int x, int y)
        {
            SIZE tSize = paint.TextSize(text, font);
            RECT tRect = new RECT(x, y, x + tSize.cx, y + tSize.cy);
            paint.DrawText(text, dwPenColor, font, tRect);
            return tSize;
        }

        /// <summary>
        /// 绘制有下划线的数字
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="value">值</param>
        /// <param name="digit">保留小数位数</param>
        /// <param name="font">字体</param>
        /// <param name="fontColor">文字颜色</param>
        /// <param name="zeroAsEmpty">0是否为空</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>绘制的横坐标</returns>
        public static int DrawUnderLineNum(CPaint paint, double value, int digit, FONT font, long fontColor, bool zeroAsEmpty, int x, int y)
        {
            if (zeroAsEmpty && value == 0)
            {
                String text = "-";
                SIZE size = paint.TextSize(text, font);
                CDraw.DrawText(paint, text, fontColor, font, x, y);
                return size.cx;
            }
            else
            {
                String[] nbs = CStr.GetValueByDigit(value, digit).Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (nbs.Length == 1)
                {
                    SIZE size = paint.TextSize(nbs[0], font);
                    CDraw.DrawText(paint, nbs[0], fontColor, font, x, y);
                    return size.cx;
                }
                else
                {
                    SIZE decimalSize = paint.TextSize(nbs[0], font);
                    SIZE size = paint.TextSize(nbs[1], font);
                    CDraw.DrawText(paint, nbs[0], fontColor, font, x, y);
                    CDraw.DrawText(paint, nbs[1], fontColor, font, x
                        + decimalSize.cx + 1, y);
                    paint.DrawLine(fontColor, 1, 0, x
                        + decimalSize.cx + 1, y + decimalSize.cy,
                        x + decimalSize.cx + size.cx, y + decimalSize.cy);
                    return decimalSize.cx + size.cx;
                }
            }
        }

        /// <summary>
        /// 获取黑色风格的颜色
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>新的颜色</returns>
        public static long GetBlackColor(long color)
        {
            if (color > CDraw.PCOLORS_USERCOLOR)
            {
                if (color == COLOR.CONTROL)
                {
                    color = COLOR.ARGB(100, 0, 0, 0);
                }
                else if (color == COLOR.CONTROLBORDER)
                {
                    color = 3289650;
                }
                else if (color == COLOR.CONTROLTEXT)
                {
                    color = COLOR.ARGB(255, 255, 255);
                }
                else if (color == COLOR.DISABLEDCONTROL)
                {
                    color = COLOR.ARGB(50, 255, 255, 255);
                }
                else if (color == COLOR.DISABLEDCONTROLTEXT)
                {
                    color = 3289650;
                }
                else if (color == COLOR.HOVEREDCONTROL)
                {
                    color = COLOR.ARGB(50, 255, 255, 255);
                }
                else if (color == COLOR.PUSHEDCONTROL)
                {
                    color = COLOR.ARGB(50, 255, 255, 255);
                }
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR)
            {
                color = COLOR.ARGB(255, 50, 50, 50);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR2)
            {
                color = COLOR.ARGB(150, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR3)
            {
                color = COLOR.ARGB(100, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR4)
            {
                color = COLOR.ARGB(0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR5)
            {
                color = COLOR.ARGB(10, 255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR6)
            {
                color = COLOR.ARGB(25, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR7)
            {
                color = COLOR.ARGB(200, 255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR2)
            {
                color = COLOR.ARGB(217, 217, 68);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR3)
            {
                color = COLOR.ARGB(80, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR4)
            {
                color = COLOR.ARGB(112, 112, 112);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR5)
            {
                color = COLOR.ARGB(192, 192, 192);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR6)
            {
                color = COLOR.ARGB(0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR7)
            {
                color = COLOR.ARGB(0, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR8)
            {
                color = COLOR.ARGB(50, 50, 50);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR9)
            {
                color = COLOR.ARGB(255, 255, 0);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR10)
            {
                color = COLOR.ARGB(255, 255, 80);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR11)
            {
                color = COLOR.ARGB(135, 206, 235);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR2)
            {
                color = COLOR.ARGB(217, 217, 68);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR3)
            {
                color = COLOR.ARGB(50, 255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR4)
            {
                color = COLOR.ARGB(150, 0, 0);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR5)
            {
                color = COLOR.ARGB(100, 100, 100);
            }
            else if (color == CDraw.PCOLORS_MIDCOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_UPCOLOR)
            {
                color = COLOR.ARGB(255, 82, 82);
            }
            else if (color == CDraw.PCOLORS_DOWNCOLOR)
            {
                color = COLOR.ARGB(80, 255, 80);
            }
            else if (color == CDraw.PCOLORS_DOWNCOLOR2)
            {
                color = COLOR.ARGB(80, 255, 255);
            }
            else if (color == CDraw.PCOLORS_DOWNCOLOR3)
            {
                color = COLOR.ARGB(100, 0, 255);
            }
            else if (color == CDraw.PCOLORS_SELECTEDROWCOLOR)
            {
                color = COLOR.ARGB(150, 100, 100, 100);
            }
            else if (color == CDraw.PCOLORS_HOVEREDROWCOLOR)
            {
                color = COLOR.ARGB(150, 150, 150, 150);
            }
            else if (color == CDraw.PCOLORS_WINDOWFORECOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR)
            {
                color = COLOR.ARGB(255, 50, 50, 50);
            }
            else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR2)
            {
                color = COLOR.ARGB(200, 20, 20, 20);
            }
            else if (color == CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR)
            {
                color = COLOR.ARGB(200, 0, 0, 0);
            }
            return color;
        }

        /// <summary>
        /// 获取白色风格的颜色
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>新的颜色</returns>
        public static long GetWhiteColor(long color)
        {
            if (color > CDraw.PCOLORS_USERCOLOR)
            {
                if (color == COLOR.CONTROL)
                {
                    color = 16777215;
                }
                else if (color == COLOR.CONTROLBORDER)
                {
                    color = 3289650;
                }
                else if (color == COLOR.CONTROLTEXT)
                {
                    color = 0;
                }
                else if (color == COLOR.DISABLEDCONTROL)
                {
                    color = 13158600;
                }
                else if (color == COLOR.DISABLEDCONTROLTEXT)
                {
                    color = 3289650;
                }
                else if (color == COLOR.HOVEREDCONTROL)
                {
                    color = 13158600;
                }
                else if (color == COLOR.PUSHEDCONTROL)
                {
                    color = 9868950;
                }
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR)
            {
                color = COLOR.ARGB(255, 50, 50, 50);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR2)
            {
                color = COLOR.ARGB(150, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR3)
            {
                color = COLOR.ARGB(100, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR4)
            {
                color = COLOR.ARGB(0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR5)
            {
                color = COLOR.ARGB(25, 255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR6)
            {
                color = COLOR.ARGB(25, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_BACKCOLOR7)
            {
                color = COLOR.ARGB(200, 255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR2)
            {
                color = COLOR.ARGB(217, 217, 68);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR3)
            {
                color = COLOR.ARGB(80, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR4)
            {
                color = COLOR.ARGB(112, 112, 112);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR5)
            {
                color = COLOR.ARGB(192, 192, 192);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR6)
            {
                color = COLOR.ARGB(0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR7)
            {
                color = COLOR.ARGB(0, 255, 255);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR8)
            {
                color = COLOR.ARGB(50, 50, 50);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR9)
            {
                color = COLOR.ARGB(255, 255, 0);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR10)
            {
                color = COLOR.ARGB(255, 255, 80);
            }
            else if (color == CDraw.PCOLORS_FORECOLOR11)
            {
                color = COLOR.ARGB(135, 206, 235);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR2)
            {
                color = COLOR.ARGB(217, 217, 68);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR3)
            {
                color = COLOR.ARGB(0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR4)
            {
                color = COLOR.ARGB(150, 0, 0);
            }
            else if (color == CDraw.PCOLORS_LINECOLOR5)
            {
                color = COLOR.ARGB(100, 100, 100);
            }
            else if (color == CDraw.PCOLORS_MIDCOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_UPCOLOR)
            {
                color = COLOR.ARGB(255, 82, 82);
            }
            else if (color == CDraw.PCOLORS_DOWNCOLOR)
            {
                color = COLOR.ARGB(80, 255, 80);
            }
            else if (color == CDraw.PCOLORS_DOWNCOLOR2)
            {
                color = COLOR.ARGB(80, 255, 255);
            }
            else if (color == CDraw.PCOLORS_DOWNCOLOR3)
            {
                color = COLOR.ARGB(100, 0, 255);
            }
            else if (color == CDraw.PCOLORS_SELECTEDROWCOLOR)
            {
                color = COLOR.ARGB(150, 100, 100, 100);
            }
            else if (color == CDraw.PCOLORS_HOVEREDROWCOLOR)
            {
                color = COLOR.ARGB(150, 150, 150, 150);
            }
            else if (color == CDraw.PCOLORS_WINDOWFORECOLOR)
            {
                color = COLOR.ARGB(255, 255, 255);
            }
            else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR)
            {
                color = COLOR.ARGB(255, 50, 50, 50);
            }
            else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR2)
            {
                color = COLOR.ARGB(150, 0, 0, 0);
            }
            else if (color == CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR)
            {
                color = COLOR.ARGB(200, 255, 255, 255);
            }
            return color;
        }

        /// <summary>
        /// 根据价格获取颜色
        /// </summary>
        /// <param name="price">价格</param>
        /// <param name="comparePrice">比较价格</param>
        /// <returns>颜色</returns>
        public static long GetPriceColor(double price, double comparePrice)
        {
            if (price != 0)
            {
                if (price > comparePrice)
                {
                    return CDraw.PCOLORS_UPCOLOR;
                }
                else if (price < comparePrice)
                {
                    return CDraw.PCOLORS_DOWNCOLOR;
                }
            }
            return CDraw.PCOLORS_MIDCOLOR;
        }
        #endregion
    }
}

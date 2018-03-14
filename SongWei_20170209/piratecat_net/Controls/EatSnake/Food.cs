/*****************************************************************************\
*                                                                             *
* Food.cs -     Food functions, types, and definitions                        *
*                                                                             *
*               Version 4.00 ★★★                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 食物
    /// </summary>
    public class Food : ControlA
    {
        /// <summary>
        /// 创建食物
        /// </summary>
        public Food()
        {
            BackColor = COLOR.ARGB(255, 0, 0);
            BorderColor = COLOR.EMPTY;
            CanFocus = false;
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            if (width > 0 && height > 0)
            {
                //绘制食物
                RECT drawRect = new RECT(0, 0, width - 1, height - 1);
                long paintingBackColor = GetPaintingBackColor();
                paint.DrawGradientEllipse(paintingBackColor, COLOR.RatioColor(paint, paintingBackColor, 0.9), drawRect, 90);
            }
        }
    }
}
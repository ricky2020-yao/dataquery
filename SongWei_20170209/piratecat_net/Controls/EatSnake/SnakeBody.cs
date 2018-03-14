/*****************************************************************************\
*                                                                             *
* SnakeBody.cs - SnakeBody functions, types, and definitions                  *
*                                                                             *
*                Version 4.00 ★★★                                          *
*                                                                             *
*                Copyright (c) 2016-2016, Lord's Snake. All rights reserved.  *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 蛇关节
    /// </summary>
    public class SnakeBody : ControlA
    {
        /// <summary>
        /// 创建蛇身
        /// </summary>
        /// <param name="rect">区域</param>
        public SnakeBody(RECT rect)
        {
            BackColor = COLOR.ARGB(20, 20, 20);
            BorderColor = COLOR.EMPTY;
            Bounds = rect;
            CanFocus = false;
        }

        private SnakeDirection m_direction = SnakeDirection.Left;

        /// <summary>
        /// 获取或设置移动方向
        /// </summary>
        public SnakeDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }

        private bool m_isHeader;

        /// <summary>
        /// 设置是否头部
        /// </summary>
        public bool IsHeader
        {
            set { m_isHeader = value; }
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
                //绘制关节
                RECT drawRect = new RECT(0, 0, width, width);
                long paintingBackColor = GetPaintingBackColor();
                paint.DrawGradientEllipse(paintingBackColor, COLOR.RatioColor(paint, paintingBackColor, 1.1), drawRect, 90);
                if (m_isHeader)
                {
                    POINT eyeLeft = new POINT();
                    POINT eyeRight = new POINT();
                    //绘制眼睛
                    switch (m_direction)
                    {
                        //向下
                        case SnakeDirection.Down:
                            eyeLeft = new POINT(width / 3, height * 2 / 3);
                            eyeRight = new POINT(width * 2 / 3, height * 2 / 3);
                            break;
                        //向左
                        case SnakeDirection.Left:
                            eyeLeft = new POINT(width / 3, height / 3);
                            eyeRight = new POINT(width / 3, height * 2 / 3);
                            break;
                        //向右
                        case SnakeDirection.Right:
                            eyeLeft = new POINT(width * 2 / 3, height * 2 / 3);
                            eyeRight = new POINT(width * 2 / 3, height / 3);
                            break;
                        //向上
                        case SnakeDirection.Up:
                            eyeLeft = new POINT(width / 3, height / 3);
                            eyeRight = new POINT(width * 2 / 3, height / 3);
                            break;
                    }
                    //画左眼
                    RECT lRect = new RECT(eyeLeft.x - 2, eyeLeft.y - 2, eyeLeft.x + 2, eyeLeft.y + 2);
                    paint.FillEllipse(COLOR.ARGB(255, 255, 255), lRect);
                    //画右眼
                    RECT rRect = new RECT(eyeRight.x - 2, eyeRight.y - 2, eyeRight.x + 2, eyeRight.y + 2);
                    paint.FillEllipse(COLOR.ARGB(255, 255, 255), rRect);
                }
            }
        }
    }
}
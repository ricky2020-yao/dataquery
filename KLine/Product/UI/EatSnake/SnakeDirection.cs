/*****************************************************************************\
*                                                                             *
* SnakeDirection.cs - SnakeDirection types                                    *
*                                                                             *
*                Version 4.00 ★★★                                          *
*                                                                             *
*                Copyright (c) 2016-2016, Lord's Snake. All rights reserved.  *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;

namespace KLine
{
    /// <summary>
    /// 蛇身体的移动方向
    /// </summary>
    public enum SnakeDirection
    {
        /// <summary>
        /// 向下
        /// </summary>
        Down,
        /// <summary>
        /// 向左
        /// </summary>
        Left,
        /// <summary>
        /// 向右
        /// </summary>
        Right,
        /// <summary>
        /// 向上
        /// </summary>
        Up
    }
}
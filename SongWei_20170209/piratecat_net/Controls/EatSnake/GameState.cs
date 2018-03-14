/*****************************************************************************\
*                                                                             *
* GameState.cs - GameState types                                              *
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
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// 开始
        /// </summary>
        Begin,
        /// <summary>
        /// 失败
        /// </summary>
        Lose,
        /// <summary>
        /// 正在游戏
        /// </summary>
        Playing,
        /// <summary>
        /// 暂停
        /// </summary>
        Suspend
    }
}
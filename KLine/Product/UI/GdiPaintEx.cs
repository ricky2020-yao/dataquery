/*****************************************************************************\
*                                                                             *
* GdiPaintEx.cs - Gdi paint functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/29.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// GDI绘图扩展类
    /// </summary>
    public class GdiPaintEx : GdiPaint
    {
        #region Lord 2016/4/29
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public override long GetColor(long dwPenColor)
        {
            if (dwPenColor < COLOR.EMPTY)
            {
                return CDraw.GetWhiteColor(dwPenColor);
            }
            else
            {
                return base.GetColor(dwPenColor);
            }
        }
        #endregion
    }
}

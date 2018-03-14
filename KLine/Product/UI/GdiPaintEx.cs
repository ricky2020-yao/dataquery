/*****************************************************************************\
*                                                                             *
* GdiPaintEx.cs - Gdi paint functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
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
    /// GDI��ͼ��չ��
    /// </summary>
    public class GdiPaintEx : GdiPaint
    {
        #region Lord 2016/4/29
        /// <summary>
        /// ��ȡ��ɫ
        /// </summary>
        /// <param name="dwPenColor">������ɫ</param>
        /// <returns>�����ɫ</returns>
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

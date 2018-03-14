/*****************************************************************************\
*                                                                             *
* GdiPlusPaintEx.cs - GdiPlus paint functions, types, and definitions.        *
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

namespace piratecat
{
    /// <summary>
    /// GDI+��ͼ��չ��
    /// </summary>
    public class GdiPlusPaintEx : GdiPlusPaint
    {
        #region Lord 2016/4/29
        /// <summary>
        /// �Ƿ�֧��͸����
        /// </summary>
        private bool m_supportTransparent = true;

        /// <summary>
        /// ��ȡ��ɫ
        /// </summary>
        /// <param name="dwPenColor">������ɫ</param>
        /// <returns>�����ɫ</returns>
        public override long GetColor(long dwPenColor)
        {
            if (dwPenColor < COLOR.EMPTY)
            {
                return CDraw.GetBlackColor(dwPenColor);
            }
            else
            {
                return base.GetColor(dwPenColor);
            }
        }

        /// <summary>
        /// �����Ƿ�֧��͸����
        /// </summary>
        /// <param name="supportTransparent">�Ƿ�֧��͸����</param>
        public void SetSupportTransparent(bool supportTransparent)
        {
            m_supportTransparent = supportTransparent;
        }

        /// <summary>
        /// �Ƿ�֧��͸����
        /// </summary>
        /// <returns>֧��͸����</returns>
        public override bool SupportTransparent()
        {
            return m_supportTransparent;
        }
        #endregion
    }
}

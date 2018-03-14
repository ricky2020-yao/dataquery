/*****************************************************************************\
*                                                                             *
* ChartUIXml.cs - Chart xml functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OwLib;

namespace piratecat
{
    /// <summary>
    /// ��Ʊͼ�οؼ�Xml����
    /// </summary>
    public class UIXmlEx:UIXml
    {
        #region Lord 2016/12/24
        /// <summary>
        /// �����ؼ�
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="type">����</param>
        /// <returns>�ؼ�</returns>
        public override ControlA CreateControl(XmlNode node, String type)
        {
            INativeBase native = Native;
            if (type == "barragediv")
            {
                return new BarrageDiv();
            }
            else if (type == "floatdiv")
            {
                return new FloatDiv();
            }
            else if (type == "indexdiv")
            {
                return new IndexDiv();
            }
            else if (type == "klinediv")
            {
                return new ChartA();
            }
            else if (type == "latestdiv")
            {
                return new LatestDiv();
            }
            else if (type == "layoutscalediv")
            {
                return new LayoutScaleDiv();
            }
            else if (type == "ribbonbutton")
            {
                return new RibbonButton();
            }
            else if (type == "searchdiv")
            {
                return new SearchDiv();
            }
            else if (type == "windowex")
            {
                return new WindowEx();
            }
            else
            {
                return base.CreateControl(node, type);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual void LoadData()
        {
        }
        #endregion
    }

    /// <summary>
    /// ����XML��չ
    /// </summary>
    public class WindowXmlEx : UIXmlEx
    {
        /// <summary>
        /// �ر�
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public virtual void Show()
        {
        }
    }
}

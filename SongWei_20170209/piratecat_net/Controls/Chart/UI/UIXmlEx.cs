/*****************************************************************************\
*                                                                             *
* ChartUIXml.cs - Chart xml functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 股票图形控件Xml解析
    /// </summary>
    public class UIXmlEx:UIXml
    {
        #region Lord 2016/12/24
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
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
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
        }
        #endregion
    }

    /// <summary>
    /// 窗体XML扩展
    /// </summary>
    public class WindowXmlEx : UIXmlEx
    {
        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
        }
    }
}

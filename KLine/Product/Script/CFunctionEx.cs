/*******************************************************************************************\
*                                                                                           *
* CFunctionEx.cs -  Indicator functions, types, and definitions.                            *
*                                                                                           *
*               Version 1.00  ����                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/10/17.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace KLine
{
    /// <summary>
    /// ��ʾ����
    /// </summary>
    public class CFunctionEx : CFunction
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="indicator">ָ��</param>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        /// <param name="withParameters">�Ƿ��в���</param>
        public CFunctionEx(CIndicator indicator, int id, String name, UIXml xml)
        {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
            m_xml = xml;
        }

        /// <summary>
        /// ָ��
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// XML����
        /// </summary>
        public UIXml m_xml;

        /// <summary>
        /// �����ֶ�
        /// </summary>
        private const String FUNCTIONS = "";

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public override double OnCalculate(CVariable var)
        {
            switch (var.m_functionID)
            {
                default:
                    return 0;
            }
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="native">������</param>
        /// <param name="script">�ű�</param>
        /// <param name="xml">XML</param>
        /// <returns>ָ��</returns>
        public static CIndicator CreateIndicator(string script, UIXml xml)
        {
            CIndicator indicator = xml.Native.CreateIndicator();
            CTable table = xml.Native.CreateTable();
            indicator.DataSource = table;
            CFunctionBase.AddFunctions(indicator);
            CFunctionUI.AddFunctions(indicator, xml);
            CFunctionWin.AddFunctions(indicator);
            CFunctionAjax.AddFunctions(indicator);
            int index = 1000000;
            string[] functions = FUNCTIONS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.AddFunction(new CFunctionEx(indicator, index + i, functions[i], xml));
            }
            indicator.Script = script;
            table.AddColumn(0);
            table.Set(0, 0, 0);
            indicator.OnCalculate(0);
            return indicator;
        }
    }
}

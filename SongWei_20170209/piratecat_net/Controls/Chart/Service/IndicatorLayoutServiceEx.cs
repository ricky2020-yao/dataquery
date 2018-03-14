/*******************************************************************************************\
*                                                                                           *
* IndicatorLayoutServiceEx.cs -  IndicatorLayout service functions, types, and definitions. *
*                                                                                           *
*               Version 1.00  ����                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/3/10.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ָ�겼�ַ�����չ
    /// </summary>
    public class IndicatorLayoutServiceEx : IndicatorLayoutService
    {
        #region Lord 2016/5/25
        /// <summary>
        /// ����ָ�겼�ַ�����չ
        /// </summary>
        public IndicatorLayoutServiceEx()
        {
        }

        /// <summary>
        /// ���ּ���
        /// </summary>
        private List<IndicatorLayout> m_layouts = new List<IndicatorLayout>();

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ�б������ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// ��ȡ�������Ƿ��Ѽ���
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ��������ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_layouts.Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// ����ID��ȡ����
        /// </summary>
        /// <param name="layoutID">����ID</param>
        /// <param name="layout">����</param>
        /// <returns>״̬</returns>
        public bool GetLayoutByID(String layoutID, ref IndicatorLayout layout)
        {
            int layoutsSize = m_layouts.Count;
            for (int i = 0; i < layoutsSize; i++)
            {
                if (m_layouts[i].m_layoutID == layoutID)
                {
                    layout = m_layouts[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �������ƻ�ȡ����
        /// </summary>
        /// <param name="layoutName">��������</param>
        /// <param name="layout">����</param>
        /// <returns>״̬</returns>
        public bool GetLayoutByName(String layoutName, ref IndicatorLayout layout)
        {
            int layoutsSize = m_layouts.Count;
            for (int i = 0; i < layoutsSize; i++)
            {
                if (m_layouts[i].m_name == layoutName)
                {
                    layout = m_layouts[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡ���еĲ���
        /// </summary>
        /// <param name="layouts">���ּ���</param>
        /// <returns>״̬</returns>
        public bool GetLayouts(List<IndicatorLayout> layouts)
        {
            int layoutsSize = m_layouts.Count;
            for (int i = 0; i < layoutsSize; i++)
            {
                layouts.Add(m_layouts[i]);
            }
            return true;
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            if (message.m_functionID != FUNCTIONID_INDICATORLAYOUT_GETLAYOUTS)
            {
                message.m_requestID = m_operatorRequestID;
            }
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            GetLayouts(layouts, message.m_body, message.m_bodyLength);
            int layoutsSize = layouts.Count;
            switch (message.m_functionID)
            {
                case FUNCTIONID_INDICATORLAYOUT_GETLAYOUTS:
                    {
                        m_layouts = layouts;
                        m_loaded = true;
                        break;
                    }
                case FUNCTIONID_INDICATORLAYOUT_ADDLAYOUTS:
                    {
                        bool add = false;
                        for (int i = 0; i < layoutsSize; i++)
                        {
                            IndicatorLayout layout = null;
                            if (!GetLayoutByID(layouts[i].m_layoutID, ref layout))
                            {
                                m_layouts.Add(layouts[i]);
                                add = true;
                            }
                        }
                        if (!add)
                        {
                            return;
                        }
                        break;
                    }
                case FUNCTIONID_INDICATORLAYOUT_DELETELAYOUTS:
                    {
                        for (int i = 0; i < layoutsSize; i++)
                        {
                            IndicatorLayout layout = null;
                            if (GetLayoutByID(layouts[i].m_layoutID, ref layout))
                            {
                                m_layouts.Remove(layout);
                            }
                        }
                        break;
                    }
                case FUNCTIONID_INDICATORLAYOUT_UPDATELAYOUTS:
                    {
                        for (int i = 0; i < layoutsSize; i++)
                        {
                            IndicatorLayout updateLayout = layouts[i];
                            int curLayoutsSize = m_layouts.Count;
                            for (int j = 0; j < curLayoutsSize; j++)
                            {
                                IndicatorLayout layout = m_layouts[j];
                                if (layout.m_layoutID == updateLayout.m_layoutID)
                                {
                                    m_layouts[j] = updateLayout;
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            base.OnReceive(message);
        }
        #endregion
    }
}

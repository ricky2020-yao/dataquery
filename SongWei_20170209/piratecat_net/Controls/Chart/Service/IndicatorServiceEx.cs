/*****************************************************************************\
*                                                                             *
* IndicatorServiceEx.cs - Indicator service functions, types, and definitions.*
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/20.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ָ��������չ
    /// </summary>
    public class IndicatorServiceEx:IndicatorService
    {
        #region Lord 2016/1/20
        /// <summary>
        /// ����ָ�������չ
        /// </summary>
        public IndicatorServiceEx()
        {
        }

        /// <summary>
        /// ָ�꼯��
        /// </summary>
        private List<Indicator> m_indicators = new List<Indicator>();

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
                m_indicators.Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// ����ID��ȡָ��
        /// </summary>
        /// <param name="indicatorID">ָ��ID</param>
        /// <param name="indicator">����ָ��</param>
        /// <returns>״̬</returns>
        public bool GetIndicatorByID(String indicatorID, ref Indicator indicator)
        {
            int indicatorSize = m_indicators.Count;
            for (int i = 0; i < indicatorSize; i++)
            {
                if (m_indicators[i].m_indicatorID == indicatorID)
                {
                    indicator = m_indicators[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �������ƻ�ȡָ��
        /// </summary>
        /// <param name="name">ָ������</param>
        /// <param name="indicator">����ָ��</param>
        /// <returns>״̬</returns>
        public bool GetIndicatorByName(String name, ref Indicator indicator)
        {
            int indicatorSize = m_indicators.Count;
            for (int i = 0; i < indicatorSize; i++)
            {
                if (m_indicators[i].m_name == name)
                {
                    indicator = m_indicators[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡ���е�ָ��
        /// </summary>
        /// <param name="indicators">ָ�꼯��</param>
        /// <returns>״̬</returns>
        public bool GetIndicators(List<Indicator> indicators)
        {
            int indicatorSize = m_indicators.Count;
            for (int i = 0; i < indicatorSize; i++)
            {
                indicators.Add(m_indicators[i]);
            }
            return true;
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            if (message.m_functionID != FUNCTIONID_INDICATOR_GETINDICATORS)
            {
                message.m_requestID = m_operatorRequestID;
            }
            List<Indicator> indicators = new List<Indicator>();
            GetIndicators(indicators, message.m_body, message.m_bodyLength);
            int indicatorsSize = indicators.Count;
            switch (message.m_functionID)
            {
                case FUNCTIONID_INDICATOR_GETINDICATORS:
                    {
                        m_indicators = indicators;
                        m_loaded = true;
                        break;
                    }
                case FUNCTIONID_INDICATOR_ADDINDICATORS:
                    {
                        bool add = false;
                        for (int i = 0; i < indicatorsSize; i++)
                        {
                             Indicator indicator = null;
                             if (!GetIndicatorByID(indicators[i].m_indicatorID, ref indicator))
                             {
                                 m_indicators.Add(indicators[i]);
                                 add = true;
                             }
                        }
                        if (!add)
                        {
                            return;
                        }
                        break;
                    }
                case FUNCTIONID_INDICATOR_DELETEINDICATORS:
                    {
                        for (int i = 0; i < indicatorsSize; i++)
                        {
                            Indicator indicator = null;
                            if (GetIndicatorByID(indicators[i].m_indicatorID, ref indicator))
                            {
                                m_indicators.Remove(indicator);
                            }
                        }
                        break;
                    }
                case FUNCTIONID_INDICATOR_UPDATEINDICATORS:
                    {
                        for (int i = 0; i < indicatorsSize; i++)
                        {
                            Indicator updateIndicator = indicators[i];
                            int curIndicatorsSize = m_indicators.Count;
                            for (int j = 0; j < curIndicatorsSize; j++)
                            {
                                Indicator indicator = m_indicators[j];
                                if (indicator.m_indicatorID == updateIndicator.m_indicatorID)
                                {
                                    m_indicators[j] = updateIndicator;
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

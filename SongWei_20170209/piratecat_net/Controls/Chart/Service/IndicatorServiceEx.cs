/*****************************************************************************\
*                                                                             *
* IndicatorServiceEx.cs - Indicator service functions, types, and definitions.*
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 指标服务的扩展
    /// </summary>
    public class IndicatorServiceEx:IndicatorService
    {
        #region Lord 2016/1/20
        /// <summary>
        /// 创建指标服务扩展
        /// </summary>
        public IndicatorServiceEx()
        {
        }

        /// <summary>
        /// 指标集合
        /// </summary>
        private List<Indicator> m_indicators = new List<Indicator>();

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取列表的请求ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// 获取或设置是否已加载
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取操作请求ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        /// <summary>
        /// 销毁对象
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
        /// 根据ID获取指标
        /// </summary>
        /// <param name="indicatorID">指标ID</param>
        /// <param name="indicator">返回指标</param>
        /// <returns>状态</returns>
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
        /// 根据名称获取指标
        /// </summary>
        /// <param name="name">指标名称</param>
        /// <param name="indicator">返回指标</param>
        /// <returns>状态</returns>
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
        /// 获取所有的指标
        /// </summary>
        /// <param name="indicators">指标集合</param>
        /// <returns>状态</returns>
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
        /// 接收消息方法
        /// </summary>
        /// <param name="message">消息</param>
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

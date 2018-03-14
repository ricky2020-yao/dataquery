/**********************************************************************************\
*                                                                                  *
* SecurityFilterTask.cs - Security filter task functions, types, and definitions.  *
*                                                                                  *
*               Version 1.00  ����                                               *
*                                                                                  *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.           *
*               Created by Lord 2016/2/28.                                         *
*                                                                                  *
***********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ����ѡ������
    /// </summary>
    public class SecurityFilterTask:IDisposable
    {
        #region Lord 2016/2/28
        /// <summary>
        /// ��������
        /// </summary>
        public SecurityFilterTask()
        {
            m_quoteService = DataCenter.QuoteService;
            m_latestDataCallBack = new ListenerMessageCallBack(LatestDataCallBack);
            m_quoteService.RegisterListener(m_qtRequestID, m_latestDataCallBack);
            m_securityFilterService = DataCenter.SecurityFilterService;
            m_securityFilterDataCallBack = new ListenerMessageCallBack(SecurityFilterDataCallBack);
            m_securityFilterService.RegisterListener(m_sfRequestID, m_securityFilterDataCallBack);
        }

        /// <summary>
        /// ��������
        /// </summary>
        ~SecurityFilterTask()
        {
            Dispose();
        }

        /// <summary>
        /// ��Ʊ�ؼ�
        /// </summary>
        private ChartA m_chart;

        /// <summary>
        /// ֤ȯ��Ϣ
        /// </summary>
        private Dictionary<String, SecurityFilterInfo> m_infos = new Dictionary<String, SecurityFilterInfo>();

        /// <summary>
        /// �������ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_latestDataCallBack;

        /// <summary>
        /// ��������ID
        /// </summary>
        private int m_qtRequestID = BaseService.GetRequestID();

        private QuoteService m_quoteService;

        /// <summary>
        /// ����ѡ������ID
        /// </summary>
        private int m_sfRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ʷ���ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_securityFilterDataCallBack;

        private SecurityFilterServiceEx m_securityFilterService;

        private bool m_isDisposed;

        /// <summary>
        /// ��ȡ�Ƿ��ѱ�����
        /// </summary>
        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }

        private CTable m_dataSource;

        /// <summary>
        /// ��ȡ����Դ
        /// </summary>
        public CTable DataSource
        {
            get { return m_dataSource; }
            set { m_dataSource = value; }
        }

        private CIndicator m_indicator;

        /// <summary>
        /// ��ȡָ��
        /// </summary>
        public CIndicator Indicator
        {
            get { return m_indicator; }
        }

        private int m_matchCount;

        /// <summary>
        /// ��ȡ�����÷�������������
        /// </summary>
        public int MatchCount
        {
            get { return m_matchCount; }
            set { m_matchCount = value; }
        }

        private INativeBase m_native;

        /// <summary>
        /// ��ȡ�����÷�����
        /// </summary>
        public INativeBase Native
        {
            get { return m_native; }
            set { m_native = value; }
        }

        private int m_runningState = -1;

        /// <summary>
        /// ��ȡ����������״̬ -1:δ��ʼ��0:���� >0��ǰ����
        /// </summary>
        public int RunningState
        {
            get { return m_runningState; }
            set { m_runningState = value; }
        }

        private SecurityFilterTemplate m_template;

        /// <summary>
        /// ��ȡ������ģ��
        /// </summary>
        public SecurityFilterTemplate Template
        {
            get { return m_template; }
            set { m_template = value; }
        }

        /// <summary>
        /// ���ٶ��󷽷�
        /// </summary>
        public virtual void Dispose()
        {
            if (!m_isDisposed)
            {
                if (m_chart != null)
                {
                    m_chart.Dispose();
                    m_chart = null;
                }
                if (m_dataSource != null)
                {
                    m_dataSource.Dispose();
                    m_dataSource = null;
                }
                if (m_latestDataCallBack != null)
                {
                    m_quoteService.UnRegisterListener(m_qtRequestID, m_latestDataCallBack);
                    m_latestDataCallBack = null;
                }
                if (m_securityFilterDataCallBack != null)
                {
                    Stop();
                    m_securityFilterService.UnRegisterListener(m_sfRequestID, m_securityFilterDataCallBack);
                    m_securityFilterDataCallBack = null;
                }
                if (m_indicator != null)
                {
                    m_indicator.Dispose();
                    m_indicator = null;
                }
                m_isDisposed = true;
            }
        }

        /// <summary>
        /// ��ȡ֤ȯ��Ϣ�б�
        /// </summary>
        /// <param name="infos">֤ȯ��Ϣ�б�</param>
        public void GetSecurityInfos(List<SecurityFilterInfo> infos)
        {
            foreach (SecurityFilterInfo info in m_infos.Values)
            {
                infos.Add(info);
            }
        }

        /// <summary>
        /// ��ȡ֤ȯ��Ϣ�б�����
        /// </summary>
        /// <returns>����</returns>
        public int GetSecurityInfosSize()
        {
            return m_infos.Count;
        }


        /// <summary>
        /// �������ݻص��¼�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void LatestDataCallBack(CMessage message)
        {
            OnLatestDataCallBack(message);
        }

        /// <summary>
        /// �������ݻص�����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public void OnLatestDataCallBack(CMessage message)
        {
            LatestDataInfo dataInfo = new LatestDataInfo();
            List<SecurityLatestData> latestDatas = new List<SecurityLatestData>();
            QuoteService.GetLatestDatas(ref dataInfo, latestDatas, message.m_body, message.m_bodyLength);
            int latestDatasSize = latestDatas.Count;
            for (int i = 0; i < latestDatasSize; i++)
            {
                String code = latestDatas[i].m_securityCode;
                if (m_infos.ContainsKey(code))
                {
                    m_infos[code].LatestData = latestDatas[i];
                }
            }
        }

        /// <summary>
        /// ����ѡ�ɻص�����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public void OnSecurityFilterDataCallBack(CMessage message)
        {
            if (message.m_bodyLength > 0)
            {
                int currentBatch = 0, totalBatch = 1;
                if (m_runningState != -1)
                {
                    Dictionary<String, double[]> datas = new Dictionary<String, double[]>();
                    SecurityFilterService.GetFilterResults(datas, ref currentBatch, ref totalBatch, message.m_body, message.m_bodyLength);
                    if (datas.Count > 0)
                    {
                        foreach (String code in datas.Keys)
                        {
                            double[] result = datas[code];
                            if (m_infos.ContainsKey(code))
                            {
                                SecurityFilterInfo info = m_infos[code];
                                if (result != null)
                                {
                                    int pos = 0;
                                    foreach (String name in m_indicator.MainVariables.Keys)
                                    {
                                        double value = result[pos];
                                        info.SetValue(name, value);
                                        if (name == "FILTER" && value == 1)
                                        {
                                            m_matchCount++;
                                        }
                                        pos++;
                                    }
                                }
                                m_runningState++;
                                info.Completed = true;
                            }
                        }
                    }
                    datas.Clear();
                }
                if (currentBatch == totalBatch)
                {
                    m_runningState = -1;
                }
            }
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        public void Run()
        {
            if (m_chart == null)
            {
                m_chart = new ChartA();
                m_chart.Native = m_native;
                m_dataSource = SecurityDataHelper.CreateDataSource(m_chart);
            }
            if (m_indicator != null)
            {
                m_indicator.Dispose();
            }
            String indicator = m_template.m_indicator;
            if (m_template.m_filter != null && m_template.m_filter.Length > 0)
            {
                indicator += "FILTER:=" + m_template.m_filter;
            }
            else
            {
                indicator += "FILTER:=1";
            }
            m_indicator = SecurityDataHelper.CreateIndicator(m_chart, m_dataSource, indicator, m_template.m_parameters);
            m_indicator.OnCalculate(0);
            m_infos.Clear();
            List<Security> securities = new List<Security>();
            DataCenter.SecurityService.GetSecuritiesByCodes(m_template.m_codes, securities);
            int securitiesSize = securities.Count;
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = securities[i];
                SecurityFilterInfo info = new SecurityFilterInfo();
                info.DataSource = m_dataSource;
                info.Security = security;
                m_infos[security.m_code] = info;
            }
            LatestDataInfo dataInfo = new LatestDataInfo();
            dataInfo.m_codes = m_template.m_codes;
            dataInfo.m_formatType = 1;
            m_quoteService.PushLatestDatas(m_qtRequestID, dataInfo);
            m_securityFilterService.StartFilter(m_sfRequestID, m_template);
            m_runningState = 0;
            m_matchCount = 0;
        }

        /// <summary>
        /// �������ݻص��¼�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void SecurityFilterDataCallBack(CMessage message)
        {
            OnSecurityFilterDataCallBack(message);
        }

        /// <summary>
        /// ֹͣ����
        /// </summary>
        public void Stop()
        {
            m_securityFilterService.StopFilter(m_sfRequestID, m_template);
            m_runningState = -1;
        }
        #endregion
    }

    /// <summary>
    /// ����ѡ����Ϣ
    /// </summary>
    public class SecurityFilterInfo
    {
        /// <summary>
        /// ��ֵ
        /// </summary>
        private Dictionary<String, double> m_values = new Dictionary<String, double>();

        private bool m_completed;

        /// <summary>
        /// ��ȡ�������Ƿ����
        /// </summary>
        public bool Completed
        {
            get { return m_completed; }
            set { m_completed = value; }
        }

        private CTable m_dataSource;

        /// <summary>
        /// ����Դ
        /// </summary>
        public CTable DataSource
        {
            get { return m_dataSource; }
            set { m_dataSource = value; }
        }

        private SecurityLatestData m_latestData = new SecurityLatestData();

        /// <summary>
        /// ��ȡ������ʵʱ����
        /// </summary>
        public SecurityLatestData LatestData
        {
            get { return m_latestData; }
            set { m_latestData = value; }
        }

        private Security m_security;

        /// <summary>
        /// ��ȡ�����ù�Ʊ��Ϣ
        /// </summary>
        public Security Security
        {
            get { return m_security; }
            set { m_security = value; }
        }

        /// <summary>
        /// ��ȡ��ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>ֵ</returns>
        public double GetValue(String key)
        {
            if (m_completed)
            {
                if (m_values.ContainsKey(key))
                {
                    return m_values[key];
                }
            }
            return double.NaN;
        }

        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        public void SetValue(String key, double value)
        {
            if (!m_completed)
            {
                m_values[key] = value;
            }
        }
    }
}

/*****************************************************************************\
*                                                                             *
* IndexDiv.cs - Index div functions, types, and definitions.                  *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/21.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ָ����
    /// </summary>
    public class IndexDiv : ControlA
    {
        #region Lord 2016/3/21
        /// <summary>
        /// �����ؼ�
        /// </summary>
        public IndexDiv()
        {
            BackColor = CDraw.PCOLORS_BACKCOLOR4;
            BorderColor = CDraw.PCOLORS_LINECOLOR4;
            ListenerMessageCallBack callBack = new ListenerMessageCallBack(LatestDataCallBack);
            DataCenter.QuoteService.RegisterListener(m_requestID, callBack);
        }

        /// <summary>
        /// ��ҵ��ָ������
        /// </summary>
        private SecurityLatestData m_cyLatestData = new SecurityLatestData();

        /// <summary>
        /// ������
        /// </summary>
        private int m_requestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ָ֤������
        /// </summary>
        private SecurityLatestData m_ssLatestData = new SecurityLatestData();

        /// <summary>
        /// ��ָ֤������
        /// </summary>
        private SecurityLatestData m_szLatestData = new SecurityLatestData();

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ����������ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                QuoteService quoteService = DataCenter.QuoteService;
                quoteService.StopPushLatestDatas(m_requestID);
                quoteService.UnRegisterListener(m_requestID);
            }
            base.Dispose();
        }

        /// <summary>
        /// ���µ����ݵĻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public void LatestDataCallBack(CMessage message)
        {
            OnLatestDataCallBack(message);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public override void OnInvoke(object args)
        {
            base.OnInvoke(args);
            if (args != null)
            {
                CMessage message = (CMessage)args;
                LatestDataInfo dataInfo = new LatestDataInfo();
                List<SecurityLatestData> datas = new List<SecurityLatestData>();
                QuoteService.GetLatestDatas(ref dataInfo, datas, message.m_body, message.m_bodyLength);
                int datasSize = datas.Count;
                for (int i = 0; i < datasSize; i++)
                {
                    SecurityLatestData latestData = datas[i];
                    if (i == 0)
                    {
                        if (latestData.m_securityCode == "000001.SH")
                        {
                            if (!latestData.Equal(m_ssLatestData))
                            {
                                m_ssLatestData = latestData;
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        if (latestData.m_securityCode == "399001.SZ")
                        {
                            if (!latestData.Equal(m_szLatestData))
                            {
                                m_szLatestData = latestData;
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        if (latestData.m_securityCode == "399006.SZ")
                        {
                            if (!latestData.Equal(m_cyLatestData))
                            {
                                m_cyLatestData = latestData;
                            }
                        }
                    }
                }
                Invalidate();
            }
        }

        /// <summary>
        /// �����������ݵķ���
        /// </summary>
        /// <param name="message">����</param>
        public void OnLatestDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            BeginInvoke(copyMessage);
        }

        /// <summary>
        /// ��갴�·���
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        public override void OnMouseDown(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseDown(mp, button, clicks, delta);
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                int width = Width;
                Security security = new Security();
                security.m_type = 18;
                if (mp.x < width / 3)
                {
                    security.m_code = "000001.SH";
                    security.m_name = "��ָ֤��";
                }
                else if (mp.x < width * 2 / 3)
                {
                    security.m_code = "399001.SZ";
                    security.m_name = "��֤��ָ";
                }
                else
                {
                    security.m_code = "399006.SZ";
                    security.m_name = "��ҵ��ָ��";
                }
                m_chart.SearchSecurity(security);
            }
        }

        /// <summary>
        /// ����ǰ������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintForeground(CPaint paint, RECT clipRect)
        {
            RECT bounds = Bounds;
            int width = bounds.right - bounds.left;
            int height = bounds.bottom - bounds.top;
            if (width > 0 && height > 0)
            {
                if (m_ssLatestData != null && m_szLatestData != null && m_cyLatestData != null)
                {
                    long titleColor = CDraw.PCOLORS_FORECOLOR2;
                    FONT font = new FONT("SimSun", 16, false, false, false);
                    FONT indexFont = new FONT("Arial", 14, true, false, false);
                    long grayColor = CDraw.PCOLORS_FORECOLOR4;
                    //��ָ֤��
                    long indexColor = CDraw.GetPriceColor(m_ssLatestData.m_close, m_ssLatestData.m_lastClose);
                    int left = 1;
                    CDraw.DrawText(paint, "��֤", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    String amount = (m_ssLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    SIZE amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    int length = CDraw.DrawUnderLineNum(paint, m_ssLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_ssLatestData.m_close - m_ssLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //��ָ֤��
                    left = width / 3;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = CDraw.GetPriceColor(m_szLatestData.m_close, m_szLatestData.m_lastClose);
                    CDraw.DrawText(paint, "��֤", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_szLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width * 2 / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_szLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_szLatestData.m_close - m_szLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //��ҵָ��
                    left = width * 2 / 3;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = CDraw.GetPriceColor(m_cyLatestData.m_close, m_cyLatestData.m_lastClose);
                    CDraw.DrawText(paint, "��ҵ", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_cyLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_cyLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4 + length;
                    length = CDraw.DrawUnderLineNum(paint, m_cyLatestData.m_close - m_cyLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    paint.DrawRect(grayColor, 1, 0, new RECT(0, 0, width - 1, height - 1));
                }
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void Start()
        {
            LatestDataInfo dataInfo = new LatestDataInfo();
            dataInfo.m_codes = "000001.SH,399001.SZ,399006.SZ";
            dataInfo.m_formatType = 1;
            DataCenter.QuoteService.PushLatestDatas(m_requestID, dataInfo);
        }
        #endregion
    }
}

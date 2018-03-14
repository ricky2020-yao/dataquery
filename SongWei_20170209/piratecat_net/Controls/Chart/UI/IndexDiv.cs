/*****************************************************************************\
*                                                                             *
* IndexDiv.cs - Index div functions, types, and definitions.                  *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 指数栏
    /// </summary>
    public class IndexDiv : ControlA
    {
        #region Lord 2016/3/21
        /// <summary>
        /// 创建控件
        /// </summary>
        public IndexDiv()
        {
            BackColor = CDraw.PCOLORS_BACKCOLOR4;
            BorderColor = CDraw.PCOLORS_LINECOLOR4;
            ListenerMessageCallBack callBack = new ListenerMessageCallBack(LatestDataCallBack);
            DataCenter.QuoteService.RegisterListener(m_requestID, callBack);
        }

        /// <summary>
        /// 创业板指数数据
        /// </summary>
        private SecurityLatestData m_cyLatestData = new SecurityLatestData();

        /// <summary>
        /// 请求编号
        /// </summary>
        private int m_requestID = BaseService.GetRequestID();

        /// <summary>
        /// 上证指数数据
        /// </summary>
        private SecurityLatestData m_ssLatestData = new SecurityLatestData();

        /// <summary>
        /// 深证指数数据
        /// </summary>
        private SecurityLatestData m_szLatestData = new SecurityLatestData();

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置行情控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        /// <summary>
        /// 销毁方法
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
        /// 最新的数据的回调
        /// </summary>
        /// <param name="message">消息</param>
        public void LatestDataCallBack(CMessage message)
        {
            OnLatestDataCallBack(message);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
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
        /// 处理最新数据的方法
        /// </summary>
        /// <param name="message">数据</param>
        public void OnLatestDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            BeginInvoke(copyMessage);
        }

        /// <summary>
        /// 鼠标按下方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
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
                    security.m_name = "上证指数";
                }
                else if (mp.x < width * 2 / 3)
                {
                    security.m_code = "399001.SZ";
                    security.m_name = "深证成指";
                }
                else
                {
                    security.m_code = "399006.SZ";
                    security.m_name = "创业板指数";
                }
                m_chart.SearchSecurity(security);
            }
        }

        /// <summary>
        /// 绘制前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
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
                    //上证指数
                    long indexColor = CDraw.GetPriceColor(m_ssLatestData.m_close, m_ssLatestData.m_lastClose);
                    int left = 1;
                    CDraw.DrawText(paint, "上证", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    String amount = (m_ssLatestData.m_amount / 100000000).ToString("0.0") + "亿";
                    SIZE amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    int length = CDraw.DrawUnderLineNum(paint, m_ssLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_ssLatestData.m_close - m_ssLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //深证指数
                    left = width / 3;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = CDraw.GetPriceColor(m_szLatestData.m_close, m_szLatestData.m_lastClose);
                    CDraw.DrawText(paint, "深证", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_szLatestData.m_amount / 100000000).ToString("0.0") + "亿";
                    amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width * 2 / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_szLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_szLatestData.m_close - m_szLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //创业指数
                    left = width * 2 / 3;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = CDraw.GetPriceColor(m_cyLatestData.m_close, m_cyLatestData.m_lastClose);
                    CDraw.DrawText(paint, "创业", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_cyLatestData.m_amount / 100000000).ToString("0.0") + "亿";
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
        /// 开始请求
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

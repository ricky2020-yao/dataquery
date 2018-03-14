/*****************************************************************************\
*                                                                             *
* OwChart.cs -  Chart functions, types, and definitions.                      *
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
using OwLib;
using System.Windows.Forms;
using System.Threading;
using KLine;

namespace KLine
{
    /// <summary>
    /// 行情系统
    /// </summary>
    public class OwChart
    {
        private MainFrame m_mainFrame = null;
        /// <summary>
        /// 创建行情系统
        /// </summary>
        public OwChart(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            InitInterface();
        }

        /// <summary>
        /// 正要添加的画线工具
        /// </summary>
        private String m_addingPlotType;

        /// <summary>
        /// 成交量线
        /// </summary>
        private BarShape m_bar;

        /// <summary>
        /// 成交量预测线
        /// </summary>
        private BarShape m_barForecast;

        /// <summary>
        /// K线
        /// </summary>
        private CandleShape m_candle;

        /// <summary>
        /// 主div的编号
        /// </summary>
        private CDiv m_candleDiv;

        /// <summary>
        /// K线的横轴间隔
        /// </summary>
        private double m_candleHScalePixel;

        /// <summary>
        /// 当前操作的图层
        /// </summary>
        private CDiv m_currentDiv;

        /// <summary>
        /// 所有的层
        /// </summary>
        private List<CDiv> m_divs = new List<CDiv>();

        /// <summary>
        /// 浮动层
        /// </summary>
        //private FloatDiv m_floatDiv;

        /// <summary>
        /// 成交量预测请求等待检测
        /// </summary>
        private int m_forecastTick = 0;

        /// <summary>
        /// 横轴的步长
        /// </summary>
        private List<double> m_hScaleSteps = new List<double>();

        /// <summary>
        /// 指标服务
        /// </summary>
        private IndicatorServiceEx m_indicatorService;

        /// <summary>
        /// 指标面板
        /// </summary>
        //private IndexDiv m_indexDiv;

        /// <summary>
        /// 指标布局服务
        /// </summary>
        private IndicatorLayoutServiceEx m_indicatorLayoutService;

        /// <summary>
        /// 指标的右键菜单
        /// </summary>
        private MenuA m_indicatorRightMenu;

        /// <summary>
        /// 实时数据面板
        /// </summary>
        private LatestDiv m_latestDiv;

        /// <summary>
        /// 当前布局ID
        /// </summary>
        private String m_layoutID = "";

        /// <summary>
        /// 分时线的平均线
        /// </summary>
        private PolylineShape m_minuteAvgLine;

        /// <summary>
        /// 分时线
        /// </summary>
        private PolylineShape m_minuteLine;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 画线工具的右键菜单
        /// </summary>
        private MenuA m_plotRightMenu;

        /// <summary>
        /// 行情服务
        /// </summary>
        private QuoteService m_quoteService;

        /// <summary>
        /// 请求编号
        /// </summary>
        private int m_requestID = BaseService.GetRequestID();

        /// <summary>
        /// 是否反转纵坐标
        /// </summary>
        private bool m_reverseVScale = false;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private MenuA m_rightMenu;

        /// <summary>
        /// 正在查询的证券代码
        /// </summary>
        private Security m_searchSecurity;

        /// <summary>
        /// 股票服务
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// 会话数据回调
        /// </summary>
        private ListenerMessageCallBack m_userSessionDataCallBack;

        /// <summary>
        /// 自选股服务
        /// </summary>
        private UserSecurityServiceEx m_userSecurityService;

        /// <summary>
        /// 用户会话服务
        /// </summary>
        private UserSessionServiceEx m_userSessionService;

        /// <summary>
        /// 成交量预测请求ID
        /// </summary>
        private int m_vfRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 成交量层
        /// </summary>
        private CDiv m_volumeDiv;

        private BarrageDiv m_barrageDiv;

        /// <summary>
        /// 获取或设置弹幕层
        /// </summary>
        public BarrageDiv BarrageDiv
        {
            get { return m_barrageDiv; }
        }

        private ChartA m_chart;

        /// <summary>
        /// 获取或设置图形控件
        /// </summary>
        public ChartA Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private int m_cycle = 0;

        /// <summary>
        /// 获取或设置周期
        /// </summary>
        public int Cycle
        {
            get 
            {
                if (m_showMinuteLine)
                {
                    return 0;
                }
                else
                {
                    return m_cycle;
                }
            }
            set { m_cycle = value; }
        }

        private int m_digit = 2;

        /// <summary>
        /// 获取或设置价格保留小数位数
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private int m_index = -1;

        /// <summary>
        /// 获取当前实际的数据索引
        /// </summary>
        public int Index
        {
            get { return m_index; }
        }

        private List<CIndicator> m_indicators = new List<CIndicator>();

        /// <summary>
        /// 获取或设置所有指标
        /// </summary>
        public List<CIndicator> Indicators
        {
            get { return m_indicators; }
            set { m_indicators = value; }
        }

        /// <summary>
        /// 获取最新数据
        /// </summary>
        public SecurityLatestData LatestData
        {
            get
            {
                return m_latestDiv.LatestData;
            }
        }

        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        private SearchDiv m_searchDiv;

        /// <summary>
        /// 获取或设置键盘精灵
        /// </summary>
        public SearchDiv SearchDiv
        {
            get { return m_searchDiv; }
            set { m_searchDiv = value; }
        }

        private bool m_showMinuteLine = true;

        /// <summary>
        /// 获取或设置是否分时图
        /// </summary>
        public bool ShowMinuteLine
        {
            get { return m_showMinuteLine; }
            set { m_showMinuteLine = value; }
        }

        private int m_subscription = 1;

        /// <summary>
        /// 获取或设置复权方式
        /// </summary>
        public int Subscription
        {
            get { return m_subscription; }
            set { m_subscription = value; }
        }

        /// <summary>
        /// 添加空白层
        /// </summary>
        public void AddBlankDiv()
        {
            //隐藏的x轴
            int divSize = m_divs.Count;
            for (int i = 0; i < divSize; i++)
            {
                m_divs[i].HScale.Visible = false;
                m_divs[i].HScale.Height = 0;
            }
            CDiv div = m_chart.AddDiv();
            div.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_divs.Add(div);
            div.HScale.Height = 22;
            div.VGrid.Distance = 40;
            div.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR;
            div.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            div.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR;
            div.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            RefreshData();
        }

        /// <summary>
        /// 设置主图指标
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="title">标题</param>
        /// <param name="script">脚本</param>
        /// <param name="parameters">参数</param>
        /// <param name="div">图层</param>
        /// <param name="update">是否更新</param>
        /// <returns>指标对象</returns>
        public CIndicator AddMainIndicator(String name, String title, String script, String parameters, CDiv div, bool update)
        {
            //计算数据
            CIndicator indicator = SecurityDataHelper.CreateIndicator(m_chart, m_chart.DataSource, script, parameters);
            indicator.Name = name;
            //indicator.FullName = title;
            indicator.AttachVScale = AttachVScale.Left;
            m_indicators.Add(indicator);
            indicator.Div = div;
            indicator.OnCalculate(0);
            if (div != m_candleDiv && div != m_volumeDiv)
            {
                div.TitleBar.Text = title;
            }
            //刷新图像
            if (update)
            {
                m_chart.Update();
                m_native.Invalidate();
            }
            return indicator;
        }

        /// <summary>
        /// 添加自选股
        /// </summary>
        /// <param name="categoryID">类别ID</param>
        /// <param name="code">证券代码</param>
        public void AddUserSecurity(String categoryID, String code)
        {
            //List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            //m_userSecurityService.GetCategories(categories);
            //int categoriesSize = categories.Count;
            //if (categoriesSize > 0)
            //{
            //    UserSecurityCategory fCategory = categories[0];
            //    UserSecurityCategory category = new UserSecurityCategory();
            //    category.m_codes = code;
            //    if (categoryID == null || categoryID.Length == 0)
            //    {
            //        category.m_categoryID = fCategory.m_categoryID;
            //    }
            //    else
            //    {
            //        category.m_categoryID = categoryID;
            //    }
            //    category.m_userID = DataCenter.UserID;
            //    m_userSecurityService.AddSecurities(m_userSecurityService.OperatorRequestID, category);
            //}
        }

        /// <summary>
        /// 添加辅图指标
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="script">脚本</param>
        /// <param name="param">参数</param>
        /// <param name="div">图层</param>
        /// <param name="update">是否更新布局</param>
        /// <returns>指标对象</returns>
        public CIndicator AddViceIndicator(String name, String script, String parameters, CDiv div, bool update)
        {
            CIndicator indicator = SecurityDataHelper.CreateIndicator(m_chart, m_chart.DataSource, script, parameters);  
            indicator.AttachVScale = AttachVScale.Right;
            m_indicators.Add(indicator);
            indicator.Div = div;
            indicator.Name = name;
            //计算数据
            indicator.OnCalculate(0);
            if (update)
            {
                m_chart.Update();
                m_native.Invalidate();
            }
            return indicator;
        }

        /// <summary>
        /// 绑定历史数据
        /// </summary>
        /// <param name="dataInfo">数据信息</param>
        /// <param name="historyDatas">历史数据</param>
        public void BindHistoryData(HistoryDataInfo dataInfo, List<SecurityData> historyDatas)
        {
            if (dataInfo.m_securityCode == m_latestDiv.SecurityCode && dataInfo.m_cycle == m_cycle
                && dataInfo.m_subscription == m_subscription)
            {
                CTable dataSource = m_chart.DataSource;
                int[] fields = new int[] { KeyFields.CLOSE_INDEX, KeyFields.HIGH_INDEX, KeyFields.LOW_INDEX, KeyFields.OPEN_INDEX, KeyFields.VOL_INDEX, KeyFields.AMOUNT_INDEX, KeyFields.AVGPRICE_INDEX };
                int index = -1;
                if (dataInfo.m_pushData)
                {
                    SecurityData latestData = historyDatas[historyDatas.Count - 1];
                    index = SecurityDataHelper.InsertLatestData(m_chart, dataSource, m_indicators, fields, latestData);
                    if (dataInfo.m_cycle == 1440)
                    {
                        if (m_forecastTick % 10 == 0)
                        {
                            m_quoteService.GetVolumeForecast(m_vfRequestID, dataInfo.m_securityCode);
                        }
                        m_forecastTick++;
                    }
                }
                else
                {
                    SecurityDataHelper.BindHistoryDatas(m_chart, dataSource, m_indicators, fields, historyDatas);
                    if (dataInfo.m_cycle == 1440)
                    {
                        m_quoteService.GetVolumeForecast(m_vfRequestID, dataInfo.m_securityCode);
                    }
                    index = 0;
                }
                if (index >= 0)
                {
                    int rowsSize = dataSource.RowsCount;
                    m_hScaleSteps.Clear();
                    //设置坐标轴
                    if (m_showMinuteLine)
                    {
                        DateTime date = CStrA.ConvertNumToDate(dataSource.GetXValue(0));
                        int year = date.Year, month = date.Month, day = date.Day;
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 9, 0, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 9, 30, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 10, 0, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 10, 30, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 11, 0, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 13, 0, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 13, 30, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 14, 0, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 14, 30, 0)));
                        m_hScaleSteps.Add(CStrA.ConvertDateToNum(new DateTime(year, month, day, 15, 0, 0)));
                    }
                    for (int i = index; i < rowsSize; i++)
                    {
                        double volume = dataSource.Get2(i, KeyFields.VOL_INDEX);
                        if (!double.IsNaN(volume))
                        {
                            m_index = i;
                        }
                        if (!m_showMinuteLine)
                        {
                            double close = dataSource.Get2(i, KeyFields.CLOSE_INDEX);
                            double open = dataSource.Get2(i, KeyFields.OPEN_INDEX);
                            if (close >= open)
                            {
                                dataSource.Set2(i, m_bar.StyleField, 1);
                                dataSource.Set2(i, m_bar.ColorField, CDraw.PCOLORS_UPCOLOR);
                            }
                            else
                            {
                                dataSource.Set2(i, m_bar.StyleField, 0);
                                dataSource.Set2(i, m_bar.ColorField, CDraw.PCOLORS_DOWNCOLOR2);
                            }
                        }
                    }
                    RefreshData();
                }
                m_chart.Update();
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 修改周期
        /// </summary>
        /// <param name="cycle">周期</param>
        public void ChangeCycle(int cycle)
        {
            int oldCycle = Cycle;
            if (cycle > 0)
            {
                if (oldCycle > 0 && oldCycle != cycle)
                {
                    m_candleHScalePixel = m_chart.HScalePixel;
                }
                Cycle = cycle;
                m_showMinuteLine = false;
            }
            else
            {
                Cycle = cycle;
                m_showMinuteLine = true;
            }
            String securityCode = m_latestDiv.SecurityCode;
            if (securityCode != null && securityCode.Length > 0)
            {
                Security security = new Security();
                m_securityService.GetSecurityByCode(securityCode, ref security);
                SearchSecurity(security);
            }
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void ChartInvoke(object sender, object args)
        {
            if (args != null)
            {
                CMessage message = args as CMessage;
                if (message != null)
                {
                    if (message.m_serviceID == m_quoteService.ServiceID)
                    {
                        if (message.m_functionID == QuoteService.FUNCTIONID_QUOTE_PUSHHISTORYDATA && message.m_requestID == m_requestID)
                        {
                            List<SecurityData> datas = new List<SecurityData>();
                            HistoryDataInfo dataInfo = new HistoryDataInfo();
                            QuoteService.GetHistoryDatas(ref dataInfo, datas, message.m_body, message.m_bodyLength);
                            BindHistoryData(dataInfo, datas);
                            datas.Clear();
                        }
                        else if (message.m_functionID == QuoteService.FUNCTIONID_QUOTE_GETVOLUMEFORECAST && message.m_requestID == m_vfRequestID)
                        {
                            if (Cycle == 1440)
                            {
                                CTable dataSource = m_chart.DataSource;
                                int rowsCount = dataSource.RowsCount;
                                if (rowsCount > 0)
                                {
                                    double key = dataSource.GetXValue(rowsCount - 1);
                                    double volume = dataSource.Get2(rowsCount - 1, KeyFields.VOL_INDEX);
                                    double forecast = 0;
                                    List<VolumeForecastData> datas = new List<VolumeForecastData>();
                                    QuoteService.GetVolumeForecastData(datas, message.m_body, message.m_bodyLength);
                                    int datasSize = datas.Count;
                                    for (int i = 0; i < datasSize; i++)
                                    {
                                        VolumeForecastData data = datas[i];
                                        if (data.m_securityCode == m_latestDiv.SecurityCode && data.m_rate > 0)
                                        {
                                            forecast = volume / data.m_rate;
                                        }
                                    }
                                    if (forecast > 0)
                                    {
                                        dataSource.Set2(rowsCount - 1, m_barForecast.FieldName, forecast);
                                    }
                                    datas.Clear();
                                }
                            }
                        }
                    }
                    //会话服务
                    else if (message.m_serviceID == m_userSessionService.ServiceID)
                    {
                    }
                    else if (message.m_serviceID == m_indicatorService.ServiceID)
                    {
                        //指标
                        List<Indicator> indicators = new List<Indicator>();
                        m_indicatorService.GetIndicators(indicators);
                        foreach (Indicator indicator in indicators)
                        {
                            if (indicator.m_name == "MA")
                            {
                                AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, m_candleDiv, true);
                            }
                            else if (indicator.m_name == "KDJ")
                            {
                                AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, m_chart.GetDivs()[2], true);
                            }
                        }
                        Security security = new Security();
                        security.m_code = "000001.SH";
                        security.m_name = "上证指数";
                        SearchSecurity(security);
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ChartMouseDown(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            ControlA control = sender as ControlA;
            mp = control.PointToNative(mp);
            if (m_addingPlotType != null && m_addingPlotType.Length > 0)
            {
                if (button == MouseButtonsA.Left && clicks == 1)
                {
                    //添加画线工具
                    long plotColor = CDraw.PCOLORS_LINECOLOR;
                    CPlot plot = null;
                    //自定义画线
                    plot = m_native.CreatePlot(m_addingPlotType);
                    m_chart.ShowCrossLine = false;
                    plot.Color = plotColor;
                    plot.SelectedColor = plotColor;
                    CDiv mouseOverDiv = m_chart.GetMouseOverDiv();
                    m_chart.AddPlot(plot, new POINT(mp.x - m_chart.Left - mouseOverDiv.Bounds.left,
                        mp.y - m_chart.Top - mouseOverDiv.Bounds.top - mouseOverDiv.TitleBar.Height), mouseOverDiv);
                    m_chart.Cursor = CursorsA.Arrow;
                    m_chart.Invalidate();
                }
            }
            else
            {
                if (button == MouseButtonsA.Right && clicks == 1)
                {
                    if (m_chart.SelectedShape != null && GetSelectedIndicator() != null)
                    {
                        if (GetSelectedIndicator().GetShapes().Contains(m_chart.SelectedShape))
                        {
                            if (m_indicatorRightMenu != null)
                            {
                                //弹出指标的右键菜单
                                m_indicatorRightMenu.Focused = true;
                                m_indicatorRightMenu.Visible = true;
                                m_indicatorRightMenu.Location = mp;
                                m_indicatorRightMenu.BringToFront();
                                m_native.Invalidate();
                            }
                        }
                    }
                    else
                    {
                        if (m_chart.SelectedPlot != null)
                        {
                            if (m_plotRightMenu != null)
                            {
                                //弹出画线工具的右键菜单
                                m_plotRightMenu.Focused = true;
                                m_plotRightMenu.Visible = true;
                                m_plotRightMenu.Location = mp;
                                m_plotRightMenu.BringToFront();
                                m_native.Invalidate();
                            }
                        }
                        else
                        {
                            if (m_rightMenu != null)
                            {
                                m_currentDiv = m_chart.GetMouseOverDiv();
                                //弹出右键菜单
                                SIZE nativeSize = m_native.DisplaySize;
                                int rightMenuHeight = m_rightMenu.Height;
                                if (mp.y + rightMenuHeight > nativeSize.cy)
                                {
                                    mp.y = nativeSize.cy - rightMenuHeight;
                                }
                                m_rightMenu.Location = mp;
                                m_rightMenu.Focused = true;
                                m_rightMenu.Update();
                                m_rightMenu.Visible = true;
                                m_rightMenu.BringToFront();
                                m_native.Invalidate();
                            }
                        }
                    }
                    return;
                }
            }
            m_addingPlotType = String.Empty;
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ChartMouseMove(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_addingPlotType != null && m_addingPlotType.Length > 0)
            {
                m_chart.Cursor = CursorsA.Hand;
                m_chart.Invalidate();
            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ChartMouseUp(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            //m_floatDiv.Visible = m_chart.ShowCrossLine;
            m_native.Invalidate();
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            ButtonA closeButton = sender as ButtonA;
            WindowA window = closeButton.Parent as WindowA;
            window.Close();
            window.Dispose();
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="indicator">指标</param>
        public void DeleteIndicator(CIndicator indicator)
        {
            indicator.Clear();
            m_indicators.Remove(indicator);
            indicator.Dispose();
            m_chart.Update();
            m_native.Invalidate();
        }

        /// <summary>
        /// 删除所有指标
        /// </summary>
        /// <param name="update">是否更新</param>
        public void DeleteIndicators(bool update)
        {
            int m_indicatorsSize = m_indicators.Count;
            for (int i = 0; i < m_indicatorsSize; i++)
            {
                CIndicator indicator = m_indicators[i];
                indicator.Clear();
                indicator.Dispose();
            }
            m_indicators.Clear();
            if (update)
            {
                m_chart.Update();
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 删除选中的指标
        /// </summary>
        public void DeleteSelectedIndicator()
        {
            CIndicator indicator = GetSelectedIndicator();
            if (indicator != null)
            {
                indicator.Clear();
                m_indicators.Remove(indicator);
                indicator.Dispose();
                m_chart.Update();
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 删除选中的画线
        /// </summary>
        public void DeleteSelectedPlot()
        {
            CPlot selectedPlot = m_chart.SelectedPlot;
            if (selectedPlot != null)
            {
                selectedPlot.Div.RemovePlot(selectedPlot);
                selectedPlot.Dispose();
                m_chart.Update();
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public void Dispose()
        {
            m_quoteService.StopPushHistoryDatas(m_requestID);
            m_quoteService.UnRegisterListener(m_requestID);
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public void Exit()
        {
            Thread.Sleep(2000);
            DataCenter.DisConnect();
        }

        /// <summary>
        /// 获取指标组合的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public String GetIndicatorLayout()
        {
            String str = "";
            List<CDiv> divs = m_chart.GetDivs();
            int divsSize = divs.Count;
            int indicatorsSize = m_indicators.Count;
            int totalPercent = 0;
            for (int i = 0; i < divsSize; i++)
            {
                CDiv div = divs[i];
                str += "DIV\r\n";
                int percent = div.Height * 100 / m_chart.Height;
                if (i == divsSize - 1)
                {
                    percent = 100 - totalPercent;
                }
                totalPercent += percent;
                str += "HEIGHT=" + percent + "\r\n";
                for (int j = 0; j < indicatorsSize; j++)
                {
                    CIndicator indicator = m_indicators[j];
                    if (indicator.Div == div)
                    {
                        if (indicator.AttachVScale == AttachVScale.Left)
                        {
                            str += "IND=" + indicator.Name + "\r\n";
                        }
                        else
                        {
                            str += "VICEIND=" + indicator.Name + "\r\n";
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 获取选中的指标
        /// </summary>
        /// <returns>指标</returns>
        private CIndicator GetSelectedIndicator()
        {
            BaseShape shape = m_chart.SelectedShape;
            if (shape != null)
            {
                foreach (CIndicator indicator in m_indicators)
                {
                    List<BaseShape> shapes = indicator.GetShapes();
                    if (shapes.Contains(shape))
                    {
                        return indicator;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 指标数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void IndicatorDataCallBack(CMessage message)
        {
            if (message.m_bodyLength > 0)
            {
                OnIndicatorDataCallBack(message);
            }
        }

        /// <summary>
        /// 初始化图形界面
        /// </summary>
        private void InitInterface()
        {
            m_chart = m_mainFrame.GetChart("divKLine");
            CTable dataSource = m_chart.DataSource;
            m_chart.RegisterEvent(new ControlInvokeEvent(ChartInvoke), EVENTID.INVOKE);
            m_chart.RegisterEvent(new ControlMouseEvent(ChartMouseDown), EVENTID.MOUSEDOWN);
            m_chart.RegisterEvent(new ControlMouseEvent(ChartMouseMove), EVENTID.MOUSEMOVE);
            m_chart.RegisterEvent(new ControlMouseEvent(ChartMouseUp), EVENTID.MOUSEUP);
            m_chart.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_chart.BorderColor = CDraw.PCOLORS_LINECOLOR4;
            //设置可以拖动K线，成交量，线及标记
            m_chart.CanMoveShape = true;
            //设置滚动加速
            m_chart.ScrollAddSpeed = true;
            //设置左右Y轴的宽度
            m_chart.LeftVScaleWidth = 85;
            m_chart.RightVScaleWidth = 85;
            //设置X轴刻度间距
            m_chart.HScalePixel = 3;
            //设置X轴
            m_chart.HScaleFieldText = "日期";
            //添加k线层
            m_candleDiv = m_chart.AddDiv(60);
            m_candleDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_candleDiv.TitleBar.Text = "分时线"; 
            //设置主Div左右Y轴数值带下划线
            m_candleDiv.VGrid.Visible = true;
            m_candleDiv.LeftVScale.NumberStyle = NumberStyle.UnderLine;
            m_candleDiv.LeftVScale.PaddingTop = 2;
            m_candleDiv.LeftVScale.PaddingBottom = 2;
            m_candleDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            m_candleDiv.RightVScale.NumberStyle = NumberStyle.UnderLine;
            m_candleDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            m_candleDiv.RightVScale.PaddingTop = 2;
            m_candleDiv.RightVScale.PaddingBottom = 2;
            CTitle priceTitle = new CTitle(KeyFields.CLOSE_INDEX, "", CDraw.PCOLORS_FORECOLOR9, 2, true);
            priceTitle.FieldTextMode = TextMode.Value;
            m_candleDiv.TitleBar.Titles.Add(priceTitle);
            //添加K线图
            m_candle = new CandleShape();
            m_candleDiv.AddShape(m_candle);
            m_candle.CloseField = KeyFields.CLOSE_INDEX;
            m_candle.HighField = KeyFields.HIGH_INDEX;
            m_candle.LowField = KeyFields.LOW_INDEX;
            m_candle.OpenField = KeyFields.OPEN_INDEX;
            m_candle.CloseFieldText = "收盘";
            m_candle.HighFieldText = "最高";
            m_candle.LowFieldText = "最低";
            m_candle.OpenFieldText = "开盘";
            m_candle.Visible = false;
            //分时线
            m_minuteLine = new PolylineShape();
            m_candleDiv.AddShape(m_minuteLine);
            m_minuteLine.Color = CDraw.PCOLORS_LINECOLOR;
            m_minuteLine.FieldName = KeyFields.CLOSE_INDEX;
            //分时线的平均线
            m_minuteAvgLine = new PolylineShape();
            m_candleDiv.AddShape(m_minuteAvgLine);
            m_minuteAvgLine.Color = CDraw.PCOLORS_LINECOLOR2;
            m_minuteAvgLine.FieldName = KeyFields.AVGPRICE_INDEX;
            //添加成交量层
            m_volumeDiv = m_chart.AddDiv(15);
            m_volumeDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            //设置成交量的单位
            m_volumeDiv.LeftVScale.Digit = 0;
            m_volumeDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            m_volumeDiv.VGrid.Distance = 30;
            m_volumeDiv.RightVScale.Digit = 0;
            m_volumeDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            //添加成交量
            m_bar = new BarShape();
            m_bar.ColorField = CTable.AutoField;
            m_bar.StyleField = CTable.AutoField;
            m_bar.UpColor = CDraw.PCOLORS_LINECOLOR2;
            m_volumeDiv.AddShape(m_bar);
            m_bar.FieldName = KeyFields.VOL_INDEX;
            //添加成交量预测
            m_barForecast = new BarShape();
            m_barForecast.FieldText = "成交量预测";
            m_barForecast.ZOrder = -1;
            m_volumeDiv.AddShape(m_barForecast);
            m_barForecast.FieldName = CTable.AutoField;
            //设置标题
            m_volumeDiv.TitleBar.Text = "成交量";
            //设置成交量显示名称
            m_bar.FieldText = "成交量";
            //设置成交量标题只显示值
            CTitle barTitle = new CTitle(KeyFields.VOL_INDEX, "成交量", m_bar.DownColor, 0, true);
            barTitle.FieldTextMode = TextMode.Value;
            m_volumeDiv.TitleBar.Titles.Add(barTitle);
            //添加指标层
            CDiv indDiv = m_chart.AddDiv(25);
            indDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            indDiv.VGrid.Distance = 40;
            indDiv.LeftVScale.PaddingTop = 2;
            indDiv.LeftVScale.PaddingBottom = 2;
            indDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            indDiv.RightVScale.PaddingTop = 2;
            indDiv.RightVScale.PaddingBottom = 2;
            indDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            //设置X轴不可见
            m_candleDiv.HScale.Visible = false;
            m_candleDiv.HScale.Height = 0;
            m_volumeDiv.HScale.Visible = false;
            m_volumeDiv.HScale.Height = 0;
            indDiv.HScale.Visible = true;
            indDiv.HScale.Height = 22;
            //设置坐标轴的颜色
            m_volumeDiv.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR11;
            m_volumeDiv.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR11;
            indDiv.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR;
            indDiv.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR;
            //设置坐标轴的网格线间隔
            //添加到集合
            m_divs.AddRange(new CDiv[] { m_candleDiv, m_volumeDiv, indDiv });
            //添加用户自定义层
            //m_floatDiv = m_mainFrame.FindControl("divFloat") as FloatDiv;
            //m_floatDiv.Chart = this;
            //当前数据层
            m_latestDiv = m_mainFrame.FindControl("divLatest") as LatestDiv;
            m_latestDiv.Chart = this;
            //m_indexDiv = m_mainFrame.FindControl("divIndex") as IndexDiv;
            //m_indexDiv.Chart = this;
            dataSource.AddColumn(KeyFields.CLOSE_INDEX);
            dataSource.AddColumn(KeyFields.HIGH_INDEX);
            dataSource.AddColumn(KeyFields.LOW_INDEX);
            dataSource.AddColumn(KeyFields.OPEN_INDEX);
            dataSource.AddColumn(KeyFields.VOL_INDEX);
            dataSource.AddColumn(KeyFields.AMOUNT_INDEX);
            dataSource.AddColumn(KeyFields.AVGPRICE_INDEX);
            dataSource.AddColumn(m_bar.ColorField);
            dataSource.AddColumn(m_bar.StyleField);
            dataSource.AddColumn(m_barForecast.FieldName);
            dataSource.SetColsCapacity(16);
            dataSource.SetColsGrowStep(4);
            m_indicatorService = DataCenter.IndicatorService;
            m_indicatorLayoutService = DataCenter.IndicatorLayoutService;
            m_quoteService = DataCenter.QuoteService;
            m_securityService = DataCenter.SecurityService;
            m_userSecurityService = DataCenter.UserSecurityService;
            m_userSessionService = DataCenter.UserSessionService;
            m_indicatorService.RegisterListener(m_indicatorService.GetListRequestID, new ListenerMessageCallBack(IndicatorDataCallBack));
            m_userSessionDataCallBack = new ListenerMessageCallBack(UserSessionDataCallBack);
            m_userSessionService.RegisterListener(m_userSessionService.GetListRequestID, m_userSessionDataCallBack);
            m_userSessionService.RegisterListener(m_userSessionService.OperatorRequestID, m_userSessionDataCallBack);
        }

        /// <summary>
        /// 是否有窗体显示
        /// </summary>
        /// <returns>是否显示</returns>
        public bool IsWindowShowing()
        {
            List<ControlA> controls = m_native.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                WindowFrameA frame = controls[i] as WindowFrameA;
                if (frame != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            if (m_rightMenu == null)
            {
                m_barrageDiv = m_mainFrame.FindControl("divBarrage") as BarrageDiv;
                //创建右键菜单
                m_rightMenu = m_mainFrame.GetMenu("rightMenu");
                MenuItemMouseEvent menuItemClickEvent = new MenuItemMouseEvent(MenuItemClick);
                m_rightMenu.RegisterEvent(menuItemClickEvent, EVENTID.MENUITEMCLICK);
                m_rightMenu.Visible = false;
                ControlEvent rightMenuVisibleChangedEvent = new ControlEvent(RightMenuVisibleChanged);
                m_rightMenu.RegisterEvent(rightMenuVisibleChangedEvent, EVENTID.VISIBLECHANGED);
                //画线
                MenuItemA plotItemRoot = m_mainFrame.GetMenuItem("ADDPLOT");
                m_rightMenu.Update();
                Dictionary<String, String> plots = DataCenter.Plots;
                foreach (String plotType in plots.Keys)
                {
                    MenuItemA plotItem = new MenuItemA(plots[plotType]);
                    plotItem.Name = "PLOT_ADDPLOT_" + plotType;
                    plotItemRoot.AddItem(plotItem);
                }
                //指标的右键菜单
                m_indicatorRightMenu = m_mainFrame.GetMenu("indRightMenu");
                m_indicatorRightMenu.RegisterEvent(menuItemClickEvent, EVENTID.MENUITEMCLICK);
                //画线工具的右键菜单
                m_plotRightMenu = m_mainFrame.GetMenu("plotRightMenu");
                m_plotRightMenu.RegisterEvent(menuItemClickEvent, EVENTID.MENUITEMCLICK);
                //m_quoteService.RegisterListener(m_vfRequestID, new ListenerMessageCallBack(QuoteDataCallBack));
                //测试
                //MacroDiv macroDiv = new MacroDiv();
                //macroDiv.BackColor = COLOR.EMPTY;
                //macroDiv.Width = 64 * 10;
                //macroDiv.Height = 64 * 2;
                //GetDiv("divMain").AddControl(macroDiv);
                //macroDiv.BringToFront();
                //DataCenter.MacroService.Chart = this;
            }
            //m_indexDiv.Start();
            Security security = new Security();
            security.m_code = m_latestDiv.SecurityCode;
            security.m_name = m_latestDiv.SecurityName;
            if (security.m_code != null)
            {
                SearchSecurity(security);
            }
            //UserSecurityListT2 userSecurityT2 = new UserSecurityListT2();
            //userSecurityT2.Chart = this;
            //userSecurityT2.Size = new SIZE(600, 600);
            //userSecurityT2.Location = new POINT(300, 30);
            //m_native.AddControl(userSecurityT2);
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="item">菜单项</param>
        public void MenuItemClick(object sender, MenuItemA item, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            String name = item.Name;
            if (name != null && name.Length > 0)
            {
                bool setChecked = false;
                //云自选股
                if(name.StartsWith("USERSECURITY_"))
                {
                    String type = name.Substring(13);
                    if (type == "ADD")
                    {
                        String securityCode = m_latestDiv.SecurityCode;
                        if (securityCode != null && securityCode.Length > 0)
                        {
                            AddUserSecurity("", securityCode);
                        }
                    }
                    else if (type == "MANAGER")
                    {
                        ShowUserSecurityList();
                    }
                }
                //指标
                else if (name.StartsWith("IND_"))
                {
                    String type = name.Substring(4);
                    //公式编辑
                    if (type == "INDICATOREDIT")
                    {
                        ShowIndicatorList();
                    }
                    //指标选股
                    else if (type == "SECURITYFILTER")
                    {
                        ShowSecurityFilterList();
                    }
                    else if (type.StartsWith("ADDMAININD_"))
                    {
                        String ind = type.Substring(11);
                        List<Indicator> indicators = new List<Indicator>();
                        //m_indicatorService.GetIndicators(indicators);
                        foreach (Indicator indicator in indicators)
                        {
                            if (indicator.m_name == ind)
                            {
                                CDiv div = m_currentDiv;
                                if (div == null)
                                {
                                    div = m_chart.GetDivs()[m_chart.GetDivs().Count - 1];
                                }
                                AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, div, true);
                                break;
                            }
                        }
                    }
                    else if (type.StartsWith("ADDVICEIND_"))
                    {
                        String ind = type.Substring(11);
                        List<Indicator> indicators = new List<Indicator>();
                        //m_indicatorService.GetIndicators(indicators);
                        foreach (Indicator indicator in indicators)
                        {
                            if (indicator.m_name == ind)
                            {
                                CDiv div = m_currentDiv;
                                if (div == null)
                                {
                                    div = m_chart.GetDivs()[m_chart.GetDivs().Count - 1];
                                }
                                AddViceIndicator(indicator.m_name, indicator.m_text, indicator.m_parameters, m_currentDiv, true);
                                break;
                            }
                        }
                    }
                    else if (type == "DELETEIND")
                    {
                        DeleteSelectedIndicator();
                    }
                    else if (type == "SETPARAMETERS")
                    {
                        SetIndicatorParameters();
                    }
                }
                //主图类型
                else if (name.StartsWith("CANDLE_"))
                {
                    if (!m_showMinuteLine)
                    {
                        String candleStye = name.Substring(7);
                        switch (candleStye)
                        {
                            case "STANDARD":
                                m_candle.Style = CandleStyle.Rect;
                                break;
                            case "TOWER":
                                m_candle.Style = CandleStyle.Tower;
                                break;
                            case "AMERICAN":
                                m_candle.Style = CandleStyle.American;
                                break;
                            case "CLOSE":
                                m_candle.Style = CandleStyle.CloseLine;
                                break;
                        }
                        setChecked = true;
                    }
                }
                //坐标轴类型
                else if (name.StartsWith("SCALE_"))
                {
                    String scaleStyle = name.Substring(6);
                    switch (scaleStyle)
                    {
                        case "STANDARD":
                            m_candleDiv.LeftVScale.System = VScaleSystem.Standard;
                            break;
                        case "LOG":
                            m_candleDiv.LeftVScale.System = VScaleSystem.Logarithmic;
                            break;
                        case "DIFF":
                            m_candleDiv.LeftVScale.Type = VScaleType.EqualDiff;
                            break;
                        case "EQUALRATIO":
                            m_candleDiv.LeftVScale.Type = VScaleType.EqualRatio;
                            break;
                        case "DIVIDE":
                            m_candleDiv.LeftVScale.Type = VScaleType.Divide;
                            break;
                        case "PERCENT":
                            m_candleDiv.LeftVScale.Type = VScaleType.Percent;
                            break;
                        case "GOLDENRATIO":
                            m_candleDiv.LeftVScale.Type = VScaleType.GoldenRatio;
                            break;
                        case "REVERSEH":
                            m_chart.ReverseHScale = !m_chart.ReverseHScale;
                            break;
                        case "REVERSEV":
                            m_reverseVScale = !m_reverseVScale;
                            List<CDiv> divs = m_chart.GetDivs();
                            int divsSize = divs.Count;
                            for (int i = 0; i < divsSize; i++)
                            {
                                CDiv div = divs[i];
                                if (div != m_volumeDiv)
                                {
                                    div.LeftVScale.Reverse = m_reverseVScale;
                                    div.RightVScale.Reverse = m_reverseVScale;
                                }
                            }
                            break;
                    }
                }
                //布局
                else if (name.StartsWith("LAYOUT_"))
                {
                    String type = name.Substring(7);
                    switch (type)
                    {
                        case "ADDBLANKDIV":
                            AddBlankDiv();
                            break;
                        case "REMOVEBLANKDIVS":
                            RemoveBlankDivs(true);
                            break;
                        case "LAYOUTMANAGER":
                            ShowIndicatorLayoutList();
                            break;
                    }
                }
                //切换周期
                else if (name.StartsWith("CYCLE_"))
                {
                    String type = name.Substring(6);
                    int cycle = 0;
                    switch (type)
                    {
                        case "MINUTELINE":
                            m_showMinuteLine = true;
                            break;
                        case "1MINUTE":
                            cycle = 1;
                            break;
                        case "5MINUTE":
                            cycle = 5;
                            break;
                        case "15MINUTE":
                            cycle = 15;
                            break;
                        case "30MINUTE":
                            cycle = 30;
                            break;
                        case "60MINUTE":
                            cycle = 60;
                            break;
                        case "DAY":
                            cycle = 1440;
                            break;
                        case "WEEK":
                            cycle = 10080;
                            break;
                        case "MONTH":
                            cycle = 43200;
                            break;
                    }
                    ChangeCycle(cycle);
                    setChecked = true;
                }
                //复权方式
                else if (name.StartsWith("SUBSCRIPTION_"))
                {
                    if (!m_showMinuteLine)
                    {
                        String type = name.Substring(13);
                        switch (type)
                        {
                            case "NONE":
                                m_subscription = 0;
                                break;
                            case "FRONT":
                                m_subscription = 1;
                                break;
                            case "BACK":
                                m_subscription = 2;
                                break;
                        }
                        String securityCode = m_latestDiv.SecurityCode;
                        if (securityCode != null && securityCode.Length > 0)
                        {
                            Security security = new Security();
                            //m_securityService.GetSecurityByCode(securityCode, ref security);
                            SearchSecurity(security);
                        }
                        setChecked = true;
                    }
                }
                //画线工具
                else if (name.StartsWith("PLOT_"))
                {
                    String type = name.Substring(5);
                    if (type.StartsWith("ADDPLOT_"))
                    {
                        m_addingPlotType = type.Substring(8);
                    }
                    else if (type == "DELETEPLOT")
                    {
                        DeleteSelectedPlot();
                    }
                }
                //工具
                else if (name.StartsWith("TOOLS_"))
                {
                    String type = name.Substring(6);
                    //弹幕聊天
                    if (type == "CHAT")
                    {
                        ShowChatWindow();
                    }
                    //沙盘推演
                    else if (type == "SANDBOX")
                    {
                        if (!m_showMinuteLine)
                        {
                            ShowSandBoxWindow();
                        }
                    }
                    //江恩正方
                    else if (type == "GANNSQUARE")
                    {
                        WindowEx window = new WindowEx();
                        window.Text = "江恩正方";
                        window.Native = m_native;
                        GannSquare gannSquare = new GannSquare();
                        gannSquare.BackColor = COLOR.EMPTY;
                        SIZE cellSize = new SIZE(70, 30);
                        gannSquare.CellSize = cellSize;
                        int number = gannSquare.Number;
                        SIZE size = new SIZE(cellSize.cx * number, cellSize.cy * number);
                        gannSquare.Size = size;
                        //gannSquare.Style = SquareStyle.Spiral;
                        POINT location = new POINT(5, 26);
                        gannSquare.Location = location;
                        size.cx += 10;
                        size.cy += 32;
                        window.Size = size;
                        //window.AddControl(gannSquare);
                        CTable dataSource = m_chart.DataSource;
                        int rowsCount = dataSource.RowsCount;
                        if (rowsCount > 0)
                        {
                            gannSquare.StartValue = dataSource.Get2(rowsCount - 1, KeyFields.CLOSE_INDEX);
                        }
                        gannSquare.Update();
                        window.AnimateShow(false);
                        gannSquare.Focus();
                        ButtonA closeButton = window.CloseButton;
                        closeButton.RegisterEvent(new ControlMouseEvent(ClickButton), EVENTID.CLICK);
                    }
                    //贪吃蛇
                    else if (type == "EATSNAKE")
                    {
                        WindowEx window = new WindowEx();
                        window.Text = "贪吃蛇";
                        window.Native = m_native;
                        Battle battle = new Battle();
                        SIZE size = new SIZE(400, 400);
                        battle.Size = size;
                        SIZE nativeSize = m_native.DisplaySize;
                        POINT location = new POINT(5, 26);
                        battle.Location = location;
                        size.cx += 10;
                        size.cy += 32;
                        window.Size = size;
                        //window.AddControl(battle);
                        window.AnimateShow(false);
                        battle.Focus();
                        ButtonA closeButton = window.CloseButton;
                        closeButton.RegisterEvent(new ControlMouseEvent(ClickButton), EVENTID.CLICK);
                    }
                }
                //宏
                else if(name.StartsWith("MACRO_"))
                {
                    String macroType = name.Substring(6);
                    if (macroType == "MANAGER")
                    {
                        MacroList macroList = new MacroList(m_native);
                        macroList.Chart = this;
                        macroList.Show();
                    }
                }
                //绘图模式
                else if (name.StartsWith("DRAWMODE_"))
                {
                    String drawMode = name.Substring(9);
                    if (drawMode == "HIGH")
                    {
                        m_native.Paint = new GdiPlusPaintEx();
                        m_native.Invalidate();
                    }
                    else if (drawMode == "LOW")
                    {
                        m_native.Paint = new GdiPaintEx();
                        m_native.Invalidate();
                    }
                }
                if (setChecked)
                {
                    List<MenuItemA> items = item.ParentItem.GetItems();
                    int itemsSize = items.Count;
                    for (int i = 0; i < itemsSize; i++)
                    {
                        items[i].Checked = items[i] == item;
                    }
                }
                m_native.Update();
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 处理指标数据
        /// </summary>
        /// <param name="message">消息</param>
        private void OnIndicatorDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            m_chart.BeginInvoke(copyMessage);
        }

        /// <summary>
        /// 处理历史数据
        /// </summary>
        /// <param name="message">消息</param>
        private void OnQuoteDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            m_chart.BeginInvoke(copyMessage);
        }

        /// <summary>
        /// 右键菜单可见状态改变方法
        /// </summary>
        private void OnRightMenuVisibleChanged()
        {
            if (m_rightMenu.Visible)
            {
                MenuItemA mainIndRootItem = m_mainFrame.GetMenuItem("IND_ADDIND");
                MenuItemA viceIndRootItem = m_mainFrame.GetMenuItem("IND_ADDVICEIND");
                List <MenuItemA> mainIndSubItems = mainIndRootItem.GetItems();
                List<MenuItemA> viceIndSubItems = viceIndRootItem.GetItems();
                int mainIndSubItemsSize = mainIndSubItems.Count;
                for (int i = 0; i < mainIndSubItemsSize; i++)
                {
                    mainIndSubItems[i].ClearItems();
                }
                int viceIndSubItemsSize = viceIndSubItems.Count;
                for (int i = 0; i < viceIndSubItemsSize; i++)
                {
                    viceIndSubItems[i].ClearItems();
                }
                List<Indicator> indicators = new List<Indicator>();
                m_indicatorService.GetIndicators(indicators);
                foreach (Indicator indicator in indicators)
                {
                    //附图指标
                    String title = indicator.m_description;
                    if (indicator.m_name != null && indicator.m_name.Length > 0)
                    {
                        title += "(" + indicator.m_name + ")";
                    }
                    for (int j = 0; j < mainIndSubItemsSize; j++)
                    {
                        MenuItemA item = mainIndSubItems[j];
                        if (item.Text == indicator.m_category)
                        {
                            MenuItemA mainIndItem = new MenuItemA(title);
                            mainIndItem.Name = "IND_ADDMAININD_" + indicator.m_name;
                            item.AddItem(mainIndItem);
                        }
                    }
                    for (int j = 0; j < viceIndSubItemsSize; j++)
                    {
                        MenuItemA item = viceIndSubItems[j];
                        if (item.Text == indicator.m_category)
                        {
                            MenuItemA viceIndItem = new MenuItemA(title);
                            viceIndItem.Name = "IND_ADDVICEIND_" + indicator.m_name;
                            item.AddItem(viceIndItem);
                        }
                    }
                }
                indicators.Clear();
            }
        }

        /// <summary>
        /// 用户会话数据回调方法
        /// </summary>
        /// <param name="message">消息</param>
        public void OnUserSessionDataCallBack(CMessage message)
        {
            if (DataCenter.ConnectState == 1)
            {
                m_chart.BeginInvoke(message);
            }
        }

        /// <summary>
        /// 行情数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void QuoteDataCallBack(CMessage message)
        {
            if (message.m_bodyLength > 0)
            {
                OnQuoteDataCallBack(message);
            }
        }

        /// <summary>
        /// 移除新添加的空白层
        /// </summary>
        /// <param name="update">是否更新布局</param>
        public void RemoveBlankDivs(bool update)
        {
            List<CDiv> removeDivs = new List<CDiv>();
            //获取要移除的层
            foreach (CDiv div in m_chart.GetDivs())
            {
                if (div != m_candleDiv && div != m_volumeDiv)
                {
                    if (div.GetShapes(SortType.NONE).Count == 0)
                    {
                        removeDivs.Add(div);
                        m_divs.Remove(div);
                    }
                }
            }
            //移除层
            int removeDivSize = removeDivs.Count;
            for (int i = 0; i < removeDivSize; i++)
            {
                m_chart.RemoveDiv(removeDivs[i]);
            }
            //重新设置X轴
            List<CDiv> divsCopy = m_chart.GetDivs();
            int divSize = divsCopy.Count;
            for (int i = 0; i < divSize; i++)
            {
                if (i == divSize - 1)
                {
                    divsCopy[i].HScale.Visible = true;
                    divsCopy[i].HScale.Height = 22;
                }
                else
                {
                    divsCopy[i].HScale.Visible = false;
                    divsCopy[i].HScale.Height = 0;
                }
            }
            if (update)
            {
                m_chart.Update();
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
        public void ResetScaleSize(SIZE clientSize)
        {
            if (m_native != null)
            {
                ControlHost host = m_native.Host;
                SIZE nativeSize = m_native.DisplaySize;
                List<ControlA> controls = m_native.GetControls();
                int controlsSize = controls.Count;
                for (int i = 0; i < controlsSize; i++)
                {
                    WindowFrameA frame = controls[i] as WindowFrameA;
                    if (frame != null)
                    {
                        WindowEx window = frame.GetControls()[0] as WindowEx;
                        if (window != null && !window.AnimateMoving)
                        {
                            POINT location = window.Location;
                            if (location.x < 10 || location.x > nativeSize.cx - 10)
                            {
                                location.x = 0;
                            }
                            if (location.y < 30 || location.y > nativeSize.cy - 30)
                            {
                                location.y = 0;
                            }
                            window.Location = location;
                        }
                    }
                }
                m_native.ScaleSize = new SIZE((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                m_native.Update();
            }
        }

        /// <summary>
        /// 右键菜单可见状态改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void RightMenuVisibleChanged(object sender)
        {
            OnRightMenuVisibleChanged();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            if (m_showMinuteLine)
            {
                m_candleDiv.LeftVScale.ForeColor2 = CDraw.PCOLORS_DOWNCOLOR;
                m_candleDiv.RightVScale.ForeColor2 = CDraw.PCOLORS_DOWNCOLOR;
                m_candleDiv.RightVScale.Type = VScaleType.Percent;
                m_candle.DownColor = CDraw.PCOLORS_LINECOLOR;
                m_candle.Style = CandleStyle.CloseLine;
                m_candle.TagColor = COLOR.EMPTY;
                m_candle.UpColor = CDraw.PCOLORS_LINECOLOR;
                m_bar.Style = BarStyle.Line;
                m_minuteLine.Visible = true;
                m_minuteAvgLine.Visible = true;
                m_candle.Visible = false;
                m_volumeDiv.LeftVScale.Magnitude = 1;
                m_volumeDiv.RightVScale.Magnitude = 1;
            }
            else
            {
                m_candleDiv.LeftVScale.ForeColor2 = COLOR.EMPTY;
                m_candleDiv.RightVScale.ForeColor2 = CDraw.PCOLORS_DOWNCOLOR;
                m_candleDiv.RightVScale.Type = VScaleType.Percent;
                m_candle.DownColor = CDraw.PCOLORS_DOWNCOLOR2;
                m_candle.Style = CandleStyle.Rect;
                m_candle.TagColor = CDraw.PCOLORS_FORECOLOR;
                m_candle.UpColor = CDraw.PCOLORS_UPCOLOR;
                m_bar.Style = BarStyle.Rect;
                m_minuteLine.Visible = false;
                m_candle.Visible = true;
                m_minuteAvgLine.Visible = false;
                m_volumeDiv.LeftVScale.Magnitude = 1000;
                m_volumeDiv.RightVScale.Magnitude = 1000;
            }
            int indicatorSize = m_indicators.Count;
            for (int i = 0; i < indicatorSize; i++)
            {
                CIndicator indicator = m_indicators[i];
                CDiv div = indicator.Div;
                if (div == m_candleDiv)
                {
                    //隐藏显示线条
                    List<BaseShape> shapes = indicator.GetShapes();
                    int shapesSize = shapes.Count;
                    for (int j = 0; j < shapesSize; j++)
                    {
                        BaseShape shape = shapes[j];
                        shape.Visible = !m_showMinuteLine;
                    }
                    //隐藏显示标题
                    List<CTitle> titles = div.TitleBar.Titles;
                    int titlesSize = titles.Count;
                    for (int j = 0; j < titlesSize; j++)
                    {
                        CTitle title = titles[j];
                        if (title.FieldName == KeyFields.CLOSE_INDEX)
                        {
                            title.Visible = m_showMinuteLine;
                        }
                        else
                        {
                            title.Visible = !m_showMinuteLine;
                        }
                    }
                }
            }
            m_latestDiv.Digit = m_digit;
            SecurityLatestData latestData = m_latestDiv.LatestData;
            CTable dataSource = m_chart.DataSource;
            foreach (CDiv div in m_chart.GetDivs())
            {

                if (div == m_candleDiv)
                {
                    double lastClose = 0;
                    if (latestData != null && latestData.m_securityCode != null && latestData.m_securityCode.Length > 0)
                    {
                        lastClose = latestData.m_lastClose;
                    }
                    else
                    {
                        int rowsSize = dataSource.RowsCount;
                        if (rowsSize > 0)
                        {
                            if (m_showMinuteLine)
                            {
                                lastClose = dataSource.Get2(0, KeyFields.CLOSE_INDEX);
                            }
                            else
                            {
                                if (rowsSize == 1)
                                {
                                    lastClose = dataSource.Get2(0, KeyFields.OPEN_INDEX);
                                }
                                else
                                {
                                    lastClose = dataSource.Get2(rowsSize - 2, KeyFields.CLOSE_INDEX);
                                }
                            }
                        }
                    }
                    if (m_showMinuteLine)
                    {
                        div.LeftVScale.MidValue = lastClose;            
                    }
                    else
                    {
                        div.LeftVScale.MidValue = 0;
                    }
                    div.RightVScale.MidValue = lastClose;
                }
                if (div != m_volumeDiv)
                {
                    div.LeftVScale.Digit = m_digit;
                    div.RightVScale.Digit = m_digit;
                }
                div.HScale.SetScaleSteps(m_hScaleSteps);
                div.VGrid.Visible = m_showMinuteLine;
            }
            List<ControlA> controls = m_native.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                WindowFrameA frame = controls[i] as WindowFrameA;
                if (frame != null)
                {
                    WindowA window = frame.GetControls()[0] as WindowA;
                    if (window != null)
                    {
                        List<ControlA> subControls = window.GetControls();
                        int subControlsSize = subControls.Count;
                        for (int j = 0; j < subControlsSize; j++)
                        {
                            ControlA subControl = subControls[j];
                            Battle battle = subControl as Battle;
                            GannSquare gannSquare = subControl as GannSquare;
                            //贪吃蛇
                            if (battle != null)
                            {
                                int speed = 100;
                                if (latestData.m_close > 0 && latestData.m_lastClose > 0)
                                {
                                    speed -= (int)(1000 * (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose);
                                    speed /= 2;
                                }
                                battle.Speed = speed;
                            }
                            //江恩正方
                            else if (gannSquare != null)
                            {
                                gannSquare.StartValue = latestData.m_close;
                                gannSquare.Update();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 查询股票
        /// </summary>
        /// <param name="security">股票</param>
        public void SearchSecurity(Security security)
        {
            if (m_showMinuteLine)
            {
                m_chart.AutoFillHScale = true;
                if (m_candleHScalePixel > 0)
                {
                    m_candleHScalePixel = m_chart.HScalePixel;
                }
            }
            else
            {
                m_chart.AutoFillHScale = false;
                if (m_candleHScalePixel == 0)
                {
                    m_candleHScalePixel = 9;
                }
                m_chart.HScalePixel = m_candleHScalePixel;
            }
            bool showCrossLine = m_chart.ShowCrossLine;
            m_index = -1;
            m_chart.Clear();
            m_chart.ShowCrossLine = showCrossLine;
            System.GC.Collect();
            m_searchSecurity = security;
            m_latestDiv.Type = security.m_type;
            m_latestDiv.SecurityCode = security.m_code;
            m_latestDiv.SecurityName = security.m_name;
            HistoryDataInfo dataInfo = new HistoryDataInfo();
            dataInfo.m_securityCode = security.m_code;
            int cycle = Cycle;
            if (cycle <= 60)
            {
                dataInfo.m_cycle = cycle;
                if (m_showMinuteLine)
                {
                    m_candleDiv.TitleBar.Text = "分时线";
                }
                else
                {
                    m_candleDiv.TitleBar.Text = dataInfo.m_cycle.ToString() + "分钟线";
                }
            }
            else
            {
                if (cycle == 1440)
                {
                    m_candleDiv.TitleBar.Text = "日线";
                }
                else if (cycle == 10080)
                {
                    m_candleDiv.TitleBar.Text = "周线";
                }
                else if (cycle == 43200)
                {
                    m_candleDiv.TitleBar.Text = "月线";
                }
                dataInfo.m_cycle = cycle;
            }
            dataInfo.m_pushData = true;
            dataInfo.m_subscription = m_subscription;
            dataInfo.m_type = security.m_type;
            m_quoteService.StopPushHistoryDatas(m_requestID);
            m_quoteService.UnRegisterListener(m_requestID);
            m_requestID = BaseService.GetRequestID();
            m_quoteService.RegisterListener(m_requestID, new ListenerMessageCallBack(QuoteDataCallBack));
            m_quoteService.PushHistoryDatas(m_requestID, dataInfo);
            m_forecastTick = 0;
            m_chart.Update();
            m_native.Invalidate();
        }

        /// <summary>
        /// 设置指标布局字符串
        /// </summary>
        /// <param name="layoutID">布局ID</param>
        /// <param name="str">布局字符串</param>
        public void SetIndicatorLayout(String layoutID, String str)
        {
            m_layoutID = layoutID;
            //清除原有指标
            DeleteIndicators(false);
            RemoveBlankDivs(false);
            List<CDiv> divs = m_chart.GetDivs();
            String[] strs = str.Split(new String[] { "\r\n" }, StringSplitOptions.None);
            int strsSize = strs.Length;
            int divPos = 0;
            List<int> percents = new List<int>();
            for (int i = 0; i < strsSize; i++)
            {
                String subStr = strs[i];
                if (subStr.Length > 0)
                {
                    if (subStr == "DIV")
                    {
                        divPos++;
                        //添加空白层
                        if (divPos > divs.Count)
                        {
                            AddBlankDiv();
                        }
                    }
                    else
                    {
                        String[] subStrs = subStr.Split(new String[] { "=" }, StringSplitOptions.None);
                        String name = subStrs[0];
                        String value = subStrs[1];
                        CDiv div = m_chart.GetDivs()[divPos - 1];
                        //高度
                        if (name == "HEIGHT")
                        {
                            int verticalPercent = (int)div.VerticalPercent;
                            percents.Add(verticalPercent);
                        }
                        //指标
                        else if (name == "IND")
                        {
                            Indicator indicator = new Indicator();
                            if (m_indicatorService.GetIndicatorByName(value, ref indicator))
                            {
                                AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, div, false);
                            }
                        }
                        //辅图指标
                        else if (name == "VICEIND")
                        {
                            Indicator indicator = new Indicator();
                            if (m_indicatorService.GetIndicatorByName(value, ref indicator))
                            {
                                AddViceIndicator(indicator.m_name, indicator.m_text, indicator.m_parameters, div, false);
                            }
                        }
                    }
                }
            }
            divs = m_chart.GetDivs();
            int percentsSize = percents.Count;
            for (int i = 0; i < percentsSize; i++)
            {
                divs[i].VerticalPercent = percents[i];
            }
            percents.Clear();
            m_chart.Update();
            m_native.Invalidate();
        }

        /// <summary>
        /// 设置指标的参数
        /// </summary>
        private void SetIndicatorParameters()
        {
            CIndicator indicator = GetSelectedIndicator();
            if (indicator != null)
            {
                //IndicatorData indicatorData = indicator.Tag as IndicatorData;
                //String parameters = indicatorData.m_parameters;
                //if (parameters != null && parameters.Length > 0)
                //{
                //    ParametersWindow parametersWindow = new ParametersWindow(m_native, indicator);
                //    parametersWindow.Chart = this;
                //    parametersWindow.Show();
                //}
            }
        }

        /// <summary>
        /// 显示聊天窗体
        /// </summary>
        public void ShowChatWindow()
        {
            ChatWindow chatWindow = new ChatWindow(m_native);
            //chatWindow.Chart = this;
            chatWindow.Show();
        }

        /// <summary>
        /// 显示布局窗体
        /// </summary>
        public void ShowIndicatorLayoutList()
        {
            //IndicatorLayoutList indicatorLayoutList = new IndicatorLayoutList(m_native);
            //indicatorLayoutList.Chart = this;
            //indicatorLayoutList.Show();
        }

        /// <summary>
        /// 显示公式编辑
        /// </summary>
        public void ShowIndicatorList()
        {
            //IndicatorList indicatorList = new IndicatorList(m_native);
            //indicatorList.Chart = this;
            //indicatorList.Div = m_chart.GetMouseOverDiv();
            //indicatorList.Show();
        }

        /// <summary>
        /// 显示登录窗体
        /// </summary>
        public void ShowLoginWindow()
        {
            //LoginWindow loginWindow = new LoginWindow(m_native);
            //loginWindow.Chart = this;
            //loginWindow.Show();
        }

        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="caption">标题</param>
        /// <param name="uType">格式</param>
        /// <returns>结果</returns>
        public int ShowMessageBox(String text, String caption, int uType)
        {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// 显示沙盘推演窗体
        /// </summary>
        public void ShowSandBoxWindow()
        {
            //SandBoxWindow sandBoxWindow = new SandBoxWindow(m_native);
            //sandBoxWindow.Chart = this;
            //sandBoxWindow.Show();
        }

        /// <summary>
        /// 显示键盘精灵层
        /// </summary>
        /// <param name="key">按键</param>
        public void ShowSearchDiv(char key)
        {
            //ControlA focusedControl = m_native.FocusedControl;
            //if (focusedControl != null)
            //{
            //    String name = focusedControl.Name;
            //    if (IsWindowShowing() && name != "txtSearch")
            //    {
            //        return;
            //    }
            //    if (!(focusedControl is TextBoxA) || (m_searchDiv != null && focusedControl == m_searchDiv.SearchTextBox)
            //        || name == "txtSearch")
            //    {
            //        Keys keyData = (Keys)key;
            //        //创建键盘精灵
            //        if (m_searchDiv == null)
            //        {
            //            m_searchDiv = new SearchDiv();
            //            m_searchDiv.Popup = true;
            //            m_searchDiv.Size = new SIZE(240, 200);
            //            m_searchDiv.Visible = false;
            //            m_native.AddControl(m_searchDiv);
            //            m_searchDiv.BringToFront();
            //            m_searchDiv.Chart = this;
            //        }
            //        //退出
            //        if (keyData == Keys.Escape)
            //        {
            //            m_searchDiv.Visible = false;
            //            m_searchDiv.Invalidate();
            //        }
            //        //切换分时图和K线
            //        else if (keyData == Keys.F5)
            //        {
            //            m_showMinuteLine = !m_showMinuteLine;
            //            if (m_showMinuteLine)
            //            {
            //                m_cycle = 0;
            //            }
            //            else
            //            {
            //                m_cycle = 1440;
            //            }
            //            String securityCode = m_latestDiv.SecurityCode;
            //            if (securityCode != null && securityCode.Length > 0)
            //            {
            //                Security security = new Security();
            //                m_securityService.GetSecurityByCode(securityCode, ref security);
            //                SearchSecurity(security);
            //            }
            //        }
            //        //输入
            //        else
            //        {
            //            if (!m_searchDiv.Visible)
            //            {
            //                char ch = '\0';
            //                if ((keyData >= Keys.D0) && (keyData <= Keys.D9))
            //                {
            //                    ch = (char)((0x30 + keyData) - 0x30);
            //                }
            //                else if ((keyData >= Keys.A) && (keyData <= Keys.Z))
            //                {
            //                    ch = (char)((0x41 + keyData) - 0x41);
            //                }
            //                else if ((keyData >= Keys.NumPad0) && (keyData <= Keys.NumPad9))
            //                {
            //                    ch = (char)((0x30 + keyData) - 0x60);
            //                }
            //                if (ch != '\0')
            //                {
            //                    SIZE size = m_native.Host.GetSize();
            //                    POINT location = new POINT(size.cx - m_searchDiv.Width, size.cy - m_searchDiv.Height);
            //                    if (name == "txtSearch")
            //                    {
            //                        POINT fPoint = new POINT(0, 0);
            //                        fPoint = focusedControl.PointToNative(fPoint);
            //                        location = new POINT(fPoint.x, fPoint.y - m_searchDiv.Height + focusedControl.Height);
            //                        m_searchDiv.CategoryID = focusedControl.Tag.ToString();
            //                    }
            //                    else
            //                    {
            //                        m_searchDiv.CategoryID = "";
            //                    }
            //                    m_searchDiv.Location = location;
            //                    m_searchDiv.SearchTextBox.Text = "";
            //                    m_searchDiv.FilterSearch();
            //                    m_searchDiv.Visible = true;
            //                    m_searchDiv.SearchTextBox.Focused = true;
            //                    m_searchDiv.Update();
            //                    m_searchDiv.Invalidate();
            //                }
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 显示指标选股窗体
        /// </summary>
        public void ShowSecurityFilterList()
        {
            //SecurityFilterList securityFilterList = new SecurityFilterList(m_native);
            //securityFilterList.Chart = this;
            //securityFilterList.Show();
        }

        /// <summary>
        /// 显示自选股窗体
        /// </summary>
        public void ShowUserSecurityList()
        {
            //UserSecurityList userSecurityList = new UserSecurityList(m_native);
            //userSecurityList.Chart = this;
            //userSecurityList.Show();
        }

        /// <summary>
        /// 会话数据回调方法
        /// </summary>
        /// <param name="message">消息</param>
        private void UserSessionDataCallBack(CMessage message)
        {
            OnUserSessionDataCallBack(message);
        }
    }
}

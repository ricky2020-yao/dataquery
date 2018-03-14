/*****************************************************************************\
*                                                                             *
* UserSecurityList.cs - User security list functions, types, and definitions. *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/8.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// 条件选股浮点型单元格
    /// </summary>
    public class UserSecurityDoubleCell : GridDoubleCell
    {
        /// <summary>
        /// 创建单元格
        /// </summary>
        public UserSecurityDoubleCell()
        {
            GridCellStyle style = new GridCellStyle();
            style.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            style.ForeColor = CDraw.PCOLORS_FORECOLOR6;
            style.Font = new FONT("Arial", 14, true, false, false);
            Style = style;
        }

        private UserSecurityList m_userSecurityList;

        /// <summary>
        /// 获取或设置自选股
        /// </summary>
        public UserSecurityList UserSecurityList
        {
            get { return m_userSecurityList; }
            set { m_userSecurityList = value; }
        }

        /// <summary>
        /// 获取浮点型数值
        /// </summary>
        /// <returns>浮点型数值</returns>
        public override double GetDouble()
        {
            GridRow row = Row;
            String securityCode = row.GetCell(0).Text;
            SecurityLatestData data = new SecurityLatestData();
            if (m_userSecurityList.m_latestDatas.ContainsKey(securityCode))
            {
                data = m_userSecurityList.m_latestDatas[securityCode];
            }
            int dataSize = data != null ? data.m_securityCode.Length : 0;
            GridColumn column = Column;
            String columnName = column.Name;
            if (columnName == "colClose")
            {
                if (dataSize > 0)
                {
                    return data.m_close;
                }
            }
            else if (columnName == "colDiff")
            {
                if (dataSize > 0)
                {
                    return data.m_close - data.m_lastClose;
                }
            }
            else if (columnName == "colDiffRange")
            {
                if (dataSize > 0)
                {
                    if (data.m_lastClose != 0)
                    {
                        return 100 * (data.m_close - data.m_lastClose) / data.m_lastClose;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据价格获取颜色
        /// </summary>
        /// <param name="price">价格</param>
        /// <param name="comparePrice">比较价格</param>
        /// <returns>颜色</returns>
        public static long GetPriceColor(double price, double comparePrice)
        {
            if (price != 0)
            {
                if (price > comparePrice)
                {
                    return CDraw.PCOLORS_UPCOLOR;
                }
                else if (price < comparePrice)
                {
                    return CDraw.PCOLORS_DOWNCOLOR3;
                }
            }
            return CDraw.PCOLORS_FORECOLOR6;
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">矩形</param>
        /// <param name="clipRect">裁剪矩形</param>
        /// <param name="isAlternate">是否交替行</param>
        public override void OnPaint(CPaint paint, RECT rect, RECT clipRect, bool isAlternate)
        {
            int clipW = clipRect.right - clipRect.left;
            int clipH = clipRect.bottom - clipRect.top;
            if (clipW > 0 && clipH > 0)
            {
                GridA grid = Grid;
                GridRow row = Row;
                GridColumn column = Column;
                if (grid != null && row != null && column != null)
                {
                    //判断选中
                    String text = "-";
                    //绘制背景
                    bool selected = false;
                    List<GridRow> selectedRows = grid.SelectedRows;
                    int selectedRowsSize = selectedRows.Count;
                    for (int i = 0; i < selectedRowsSize; i++)
                    {
                        if (selectedRows[i] == row)
                        {
                            selected = true;
                            break;
                        }
                    }
                    long backColor = COLOR.EMPTY;
                    GridRowStyle rowStyle = grid.RowStyle;
                    if (selected)
                    {
                        backColor = rowStyle.SelectedBackColor;
                    }
                    else if (row == grid.HoveredRow)
                    {
                        backColor = rowStyle.HoveredBackColor;
                    }
                    else
                    {
                        backColor = rowStyle.BackColor;
                    }
                    paint.FillRect(backColor, clipRect);
                    //获取颜色
                    FONT font = null;
                    long foreColor = COLOR.EMPTY;
                    GridCellStyle style = Style;
                    if (style != null)
                    {
                        foreColor = style.ForeColor;
                        if (style.Font != null)
                        {
                            font = style.Font;
                        }
                    }
                    double value = GetDouble();
                    if (!double.IsNaN(value))
                    {
                        SecurityLatestData data = new SecurityLatestData();
                        String securityCode = row.GetCell(0).Text;
                        if (m_userSecurityList.m_latestDatas.ContainsKey(securityCode))
                        {
                            data = m_userSecurityList.m_latestDatas[securityCode];
                        }
                        String columnName = column.Name;
                        int dataSize = data != null ? data.m_securityCode.Length : 0;
                        if (columnName == "colNo")
                        {
                            foreColor = CDraw.PCOLORS_FORECOLOR7;
                            text = ((int)value + 1).ToString();
                        }
                        else if (columnName == "colDiff")
                        {
                            if (dataSize > 0)
                            {
                                foreColor = GetPriceColor(value, 0);
                                text = value.ToString("0.00");
                            }
                        }
                        else if (columnName == "colDiffRange")
                        {
                            if (dataSize > 0)
                            {
                                foreColor = GetPriceColor(data.m_close, data.m_lastClose);
                                text = value.ToString("0.00") + "%";
                            }
                        }
                        else
                        {
                            if (dataSize > 0)
                            {
                                foreColor = GetPriceColor(value, data.m_lastClose);
                                text = value.ToString("0.00");
                            }
                        }
                    }
                    SIZE tSize = paint.TextSize(text, font);
                    POINT tPoint = new POINT(rect.right - tSize.cx - 3, rect.top + clipH / 2 - tSize.cy / 2);
                    RECT tRect = new RECT(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    paint.DrawText(text, foreColor, font, tRect);
                }
            }
        }
    }

    /// <summary>
    /// 自选股界面
    /// </summary>
    public class UserSecurityList : WindowXmlEx
    {
        #region Lord 2016/1/8
        /// <summary>
        /// 创建自选股界面
        /// </summary>
        /// <param name="native">方法库</param>
        public UserSecurityList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\UserSecurityList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowUS") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //注册秒表
            m_timerEvent = new ControlTimerEvent(CallTimer);
            m_window.RegisterEvent(m_timerEvent, EVENTID.TIMER);
            //注册点击事件
            RegisterEvents(m_window);
            m_gridCategory = GetGrid("gridCategory");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_gridCategory.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_gridCellEditEndEvent = new GridCellEvent(CategoryGridCellEditEnd);
            m_gridCategory.RegisterEvent(m_gridCellEditEndEvent, EVENTID.GRIDCELLEDITEND);
            m_gridSecurities = GetGrid("gridSecurities");
            m_gridCellClick = new GridCellMouseEvent(GridCellClick);
            m_gridSecurities.RegisterEvent(m_gridCellClick, EVENTID.GRIDCELLCLICK);
            //注册服务
            m_securityService = DataCenter.SecurityService;
            m_userSecurityService = DataCenter.UserSecurityService;
            m_userSecurityDataCallBack = new ListenerMessageCallBack(UserSecurityDataCallBack);
            m_userSecurityService.RegisterListener(m_userSecurityService.OperatorRequestID, m_userSecurityDataCallBack);
            //注册行情
            m_quoteService = DataCenter.QuoteService;
            m_latestDataCallBack = new ListenerMessageCallBack(LatestDataCallBack);
            m_quoteService.RegisterListener(m_latestDataRequestID, m_latestDataCallBack);
            //搜索文本框
            m_txtSearch = GetTextBox("txtSearch");
            m_searchTextBoxGotFocus = new ControlEvent(SearchTextBoxGotFocus);
            m_txtSearch.RegisterEvent(m_searchTextBoxGotFocus, EVENTID.GOTFOCUS);
        }

        /// <summary>
        /// 类别表
        /// </summary>
        private GridA m_gridCategory;

        /// <summary>
        /// 表格单元格点击事件
        /// </summary>
        private GridCellMouseEvent m_gridCellClick;

        /// <summary>
        /// 表格单元格编辑结束事件
        /// </summary>
        private GridCellEvent m_gridCellEditEndEvent;

        /// <summary>
        /// 证券表格
        /// </summary>
        private GridA m_gridSecurities;

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// 最新数据回调
        /// </summary>
        private ListenerMessageCallBack m_latestDataCallBack;

        /// <summary>
        /// 最新数据的请求ID
        /// </summary>
        private int m_latestDataRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 调用控件线程方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 行情服务
        /// </summary>
        private QuoteService m_quoteService;

        /// <summary>
        /// 最新数据
        /// </summary>
        public Dictionary<String, SecurityLatestData> m_latestDatas = new Dictionary<String, SecurityLatestData>();

        /// <summary>
        /// 搜索文本改变事件
        /// </summary>
        private ControlEvent m_searchTextBoxGotFocus;

        /// <summary>
        /// 证券服务
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// 秒表事件
        /// </summary>
        private ControlTimerEvent m_timerEvent;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        /// <summary>
        /// 搜索文本框
        /// </summary>
        private TextBoxA m_txtSearch;

        /// <summary>
        /// 自选股数据回调事件
        /// </summary>
        private ListenerMessageCallBack m_userSecurityDataCallBack;

        /// <summary>
        /// 自选股服务
        /// </summary>
        private UserSecurityServiceEx m_userSecurityService;

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置股票控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// 添加类别
        /// </summary>
        public void AddCategory()
        {
            UserSecurityCategory category = new UserSecurityCategory();
            category.m_name = "未命名的自选股";
            category.m_userID = DataCenter.UserID;
            m_userSecurityService.AddCategory(m_userSecurityService.OperatorRequestID, category);
        }

        /// <summary>
        /// 添加类别到表格中
        /// </summary>
        /// <param name="categories">类别</param>
        public void AddCategoriesToCategoryGrid(List<UserSecurityCategory> categories)
        {
            if (m_gridCategory.EditTextBox != null)
            {
                m_gridCategory.OnCellEditEnd(null);
            }
            m_gridCategory.BeginUpdate();
            int categoriesSize = categories.Count;
            for (int i = 0; i < categoriesSize; i++)
            {
                UserSecurityCategory category = categories[i];
                GridRow row = new GridRow();
                row.Grid = m_gridCategory;
                GridStringCell idCell = new GridStringCell();
                idCell.Text = category.m_categoryID;
                row.AddCell(0, idCell);
                GridStringCell textCell = new GridStringCell();
                textCell.AllowEdit = true;
                textCell.Text = category.m_name;
                row.AddCell(1, textCell);
                m_gridCategory.AddRow(row);
                if (i == 0)
                {
                    m_gridCategory.OnCellEditBegin(textCell);
                }
            }
            m_gridCategory.EndUpdate();
            m_gridCategory.Invalidate();
        }

        /// <summary>
        /// 添加证券成份
        /// </summary>
        public void AddSecurities()
        {
            List<GridRow> selectedCategoryRows = m_gridCategory.SelectedRows;
            int rowsSize = selectedCategoryRows.Count;
            if (rowsSize > 0)
            {
                UserSecurityCategory category = new UserSecurityCategory();
                m_userSecurityService.GetCategory(selectedCategoryRows[0].GetCell(0).GetString(), ref category);
                SecurityList securityList = new SecurityList(m_native);
                securityList.Show();
                WindowEx window = securityList.Window;
                window.IsChildWindow = true;
                window.Tag = securityList;
                if (category.m_codes != null && category.m_codes.Length > 0)
                {
                    List<Security> securities = new List<Security>();
                    m_securityService.GetSecuritiesByCodes(category.m_codes, securities);
                    securityList.AddSecuritiesToSecuritiesGrid(securities);
                }
                WindowClosingEvent windowClosingEvent = new WindowClosingEvent(SecurityListWindowClosed);
                window.RegisterEvent(windowClosingEvent, EVENTID.WINDOWCLOSING);
                m_native.Invalidate();
            }
        }

        /// <summary>
        /// 增加证券到表格中
        /// </summary>
        /// <param name="securities">证券列表</param>
        public void AddSecuritiesToSecuritiesGrid(List<Security> securities)
        {
            Dictionary<String, String> existsCodes = new Dictionary<String, String>();
            List<GridRow> rows = m_gridSecurities.GetRows();
            int rowSize = rows.Count;
            for (int i = 0; i < rowSize; i++)
            {
                GridRow row = rows[i];
                String code = row.GetCell(0).GetString();
                existsCodes[code] = "";
            }
            int securitiesSize = securities.Count;
            m_gridSecurities.BeginUpdate();
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = securities[i];
                if (!existsCodes.ContainsKey(security.m_code))
                {
                    GridRow row = new GridRow();
                    m_gridSecurities.AddRow(row);
                    row.AddCell(0, new GridStringCell(security.m_code));
                    row.AddCell(1, new GridStringCell(security.m_name));
                    UserSecurityDoubleCell closeCell = new UserSecurityDoubleCell();
                    closeCell.UserSecurityList = this;
                    row.AddCell(2, closeCell);
                    UserSecurityDoubleCell diffCell = new UserSecurityDoubleCell();
                    diffCell.UserSecurityList = this;
                    row.AddCell(3, diffCell);
                    UserSecurityDoubleCell diffRangeCell = new UserSecurityDoubleCell();
                    diffRangeCell.UserSecurityList = this;
                    row.AddCell(4, diffRangeCell);
                    existsCodes[security.m_code] = "";
                }
            }
            existsCodes.Clear();
            m_gridSecurities.EndUpdate();
            m_gridSecurities.Invalidate();
        }

        /// <summary>
        /// 绑定自选股类别中的证券到表格
        /// </summary>
        /// <param name="category">类别</param>
        public void BindSecuritiesToSecuritiesGrid(UserSecurityCategory category)
        {
            m_gridSecurities.ClearRows();
            List<Security> securities = new List<Security>();
            if (category.m_codes != null && category.m_codes.Length > 0)
            {
                String[] codes = category.m_codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int codesSize = codes.Length;
                for (int i = 0; i < codesSize; i++)
                {
                    Security security = new Security();
                    bool state = m_securityService.GetSecurityByCode(codes[i], ref security);
                    if (state)
                    {
                        securities.Add(security);
                    }
                }
            }
            AddSecuritiesToSecuritiesGrid(securities);
        }

        /// <summary>
        /// 类别表格单元格编辑完成事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        private void CategoryGridCellEditEnd(object sender, GridCell cell)
        {
            OnCategoryGridCellEditEnd(cell);
        }

        /// <summary>
        /// 调用秒表
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="timerID">秒表ID</param>
        private void CallTimer(object sender, int timerID)
        {
            OnTimer(timerID);
        }

        /// <summary>
        /// 点击按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                //添加类别
                if (name == "btnAddCategory")
                {
                    AddCategory();
                }
                //添加证券
                else if (name == "btnAddSecurities")
                {
                    AddSecurities();
                }
                //关闭
                else if (name == "btnClose")
                {
                    Close();
                }
                //删除类别
                else if (name == "btnDeleteCategory")
                {
                    DeleteCategory();
                }
                //删除证券
                else if (name == "btnDeleteSecurities")
                {
                    DeleteSecurities();
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// 删除类别
        /// </summary>
        public void DeleteCategory()
        {
            List<GridRow> selectedRows = m_gridCategory.SelectedRows;
            int rowsSize = selectedRows.Count;
            if (rowsSize > 0)
            {
                UserSecurityCategory category = new UserSecurityCategory();
                m_userSecurityService.GetCategory(selectedRows[0].GetCell(0).GetString(), ref category);
                if (category != null)
                {
                    m_userSecurityService.DeleteCategory(m_userSecurityService.OperatorRequestID, category);
                }
            }
        }

        /// <summary>
        /// 删除证券成份
        /// </summary>
        public void DeleteSecurities()
        {
            List<GridRow> selectedCategoryRows = m_gridCategory.SelectedRows;
            int rowsSize = selectedCategoryRows.Count;
            if (rowsSize > 0)
            {
                UserSecurityCategory category = new UserSecurityCategory();
                m_userSecurityService.GetCategory(selectedCategoryRows[0].GetCell(0).GetString(), ref category);
                List<GridRow> selectedRows = m_gridSecurities.SelectedRows;
                int selectedRowsSize = selectedRows.Count;
                List<Security> securities = new List<Security>();
                if (selectedRowsSize > 0)
                {
                    String codes = "";
                    for (int i = 0; i < selectedRowsSize; i++)
                    {
                        GridRow row = selectedRows[i];
                        codes += row.GetCell(0).GetString();
                        if (i != selectedRowsSize - 1)
                        {
                            codes += ",";
                        }
                    }
                    UserSecurityCategory updateCategory = new UserSecurityCategory();
                    updateCategory.m_categoryID = category.m_categoryID;
                    updateCategory.m_codes = codes;
                    updateCategory.m_userID = DataCenter.UserID;
                    m_userSecurityService.DeleteSecurities(m_userSecurityService.OperatorRequestID, updateCategory);
                }
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_gridCategory != null)
                {
                    m_gridCategory.UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_gridCategory.UnRegisterEvent(m_gridCellEditEndEvent, EVENTID.GRIDCELLEDITEND);
                    m_gridCellEditEndEvent = null;
                    m_gridCategory = null;
                }
                if (m_gridSecurities != null)
                {
                    m_gridSecurities.UnRegisterEvent(m_gridCellClick, EVENTID.GRIDCELLCLICK);
                    m_gridCellClick = null;
                    m_gridSecurities = null;
                }
                if (m_quoteService != null)
                {
                    m_quoteService.UnRegisterListener(m_latestDataRequestID, m_latestDataCallBack);
                    m_latestDataCallBack = null;
                    m_quoteService = null;
                }
                m_latestDatas.Clear();
                if (m_txtSearch != null)
                {
                    m_txtSearch.UnRegisterEvent(m_searchTextBoxGotFocus, EVENTID.GOTFOCUS);
                    m_searchTextBoxGotFocus = null;
                    m_txtSearch = null;
                }
                if (m_userSecurityService != null)
                {
                    m_userSecurityService.UnRegisterListener(m_userSecurityService.OperatorRequestID, m_userSecurityDataCallBack);
                    m_userSecurityDataCallBack = null;
                    m_userSecurityService = null;
                }
                if (m_window != null)
                {
                    m_window.UnRegisterEvent(m_invokeEvent, EVENTID.INVOKE);
                    m_invokeEvent = null;
                    m_window.UnRegisterEvent(m_timerEvent, EVENTID.TIMER);
                    m_timerEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// 表格单元格点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 2)
            {
                OnGridCellDoubleClick(cell);
            }
        }

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void GridSelectedRowsChanged(object sender)
        {
            OnGridSelectedRowsChanged();
        }

        /// <summary>
        /// 最新数据的回调
        /// </summary>
        /// <param name="message">消息</param>
        public void LatestDataCallBack(CMessage message)
        {
            OnLatestDataCallBack(message);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// 添加类别到表格中
        /// </summary>
        /// <param name="categories">类别</param>
        public void LoadCategoriesToCategoryGrid(List<UserSecurityCategory> categories)
        {
            m_gridCategory.BeginUpdate();
            int categoriesSize = categories.Count;
            for (int i = 0; i < categoriesSize; i++)
            {
                UserSecurityCategory category = categories[i];
                GridRow row = new GridRow();
                row.Grid = m_gridCategory;
                GridStringCell idCell = new GridStringCell();
                idCell.Grid = m_gridCategory;
                idCell.Column = m_gridCategory.GetColumn(0);
                idCell.Text = category.m_categoryID;
                row.AddCell(0, idCell);
                GridStringCell textCell = new GridStringCell();
                textCell.Grid = m_gridCategory;
                textCell.Column = m_gridCategory.GetColumn(0);
                textCell.AllowEdit = true;
                textCell.Text = category.m_name;
                row.AddCell(1, textCell);
                m_gridCategory.AddRow(row);
            }
            m_gridCategory.EndUpdate();
            m_gridCategory.Invalidate();
        }

        /// <summary>
        /// 类别表格单元格编辑完成方法
        /// </summary>
        /// <param name="cell">单元格</param>
        public void OnCategoryGridCellEditEnd(GridCell cell)
        {
            if (cell != null)
            {
                UserSecurityCategory category = new UserSecurityCategory();
                m_userSecurityService.GetCategory(cell.Row.GetCell(0).GetString(), ref category);
                if (category != null)
                {
                    UserSecurityCategory updateCategory = new UserSecurityCategory();
                    updateCategory.m_categoryID = category.m_categoryID;
                    updateCategory.m_codes = category.m_codes;
                    updateCategory.m_name = m_gridCategory.EditTextBox.Text;
                    updateCategory.m_orderNum = category.m_orderNum;
                    updateCategory.m_type = category.m_type;
                    updateCategory.m_userID = category.m_userID;
                    m_userSecurityService.UpdateCategory(m_userSecurityService.OperatorRequestID, updateCategory);
                }
            }
        }

        /// <summary>
        /// 表格单元格点击方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public void OnGridCellDoubleClick(GridCell cell)
        {
            GridRow row = cell.Row;
            Security security = new Security();
            m_securityService.GetSecurityByCode(row.GetCell(0).Text, ref security);
            m_chart.SearchSecurity(security);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            if (message.m_serviceID == m_quoteService.ServiceID)
            {
                LatestDataInfo dataInfo = new LatestDataInfo();
                List<SecurityLatestData> datas = new List<SecurityLatestData>();
                QuoteService.GetLatestDatas(ref dataInfo, datas, message.m_body, message.m_bodyLength);
                int datasSize = datas.Count;
                for (int i = 0; i < datasSize; i++)
                {
                    m_latestDatas[datas[i].m_securityCode] = datas[i];
                }
                datas.Clear();
            }
            else
            {
                List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
                UserSecurityService.GetCategories(categories, message.m_body, message.m_bodyLength);
                UserSecurityCategory category = categories[0];
                switch (message.m_functionID)
                {
                    case UserSecurityService.FUNCTIONID_USERSECURITY_ADDCATEGORIES:
                        AddCategoriesToCategoryGrid(categories);
                        break;
                    case UserSecurityService.FUNCTIONID_USERSECURITY_DELETECATEGORIES:
                        RemoveCategoriesFromCategoryGrid(categories);
                        break;
                    case UserSecurityService.FUNCTIONID_USERSECURITY_UPDATECATEGORIES:
                        UpdateCategoriesToCategoryGrid(categories);
                        break;
                    case UserSecurityService.FUNCTIONID_USERSECURITY_ADDSECURITIES:
                    case UserSecurityService.FUNCTIONID_USERSECURITY_DELETESECURITIES:
                        {
                            String categoryID = "";
                            List<GridRow> selectedRows = m_gridCategory.SelectedRows;
                            int selectedRowsSize = selectedRows.Count;
                            if (selectedRowsSize > 0)
                            {
                                categoryID = selectedRows[0].GetCell(0).Text;
                            }
                            if (categoryID != null && categoryID == category.m_categoryID)
                            {
                                List<Security> securities = new List<Security>();
                                m_securityService.GetSecuritiesByCodes(category.m_codes, securities);
                                if (message.m_functionID == UserSecurityService.FUNCTIONID_USERSECURITY_ADDSECURITIES)
                                {
                                    AddSecuritiesToSecuritiesGrid(securities);
                                }
                                else if (message.m_functionID == UserSecurityService.FUNCTIONID_USERSECURITY_DELETESECURITIES)
                                {
                                    RemoveSecuritiesFromSecuritiesGrid(securities);
                                }
                            }
                            break;
                        }
                }
            }
            m_window.Invalidate();
        }

        /// <summary>
        /// 证券列表窗体关闭方法
        /// </summary>
        /// <param name="securityList">是否取消</param>
        /// <param name="cancel">是否取消</param>
        private void OnSecurityListWindowClosed(SecurityList securityList, ref bool cancel)
        {
            List<GridRow> selectedCategoryRows = m_gridCategory.SelectedRows;
            int rowsSize = selectedCategoryRows.Count;
            if (rowsSize > 0)
            {
                UserSecurityCategory category = new UserSecurityCategory();
                m_userSecurityService.GetCategory(selectedCategoryRows[0].GetCell(0).GetString(), ref category);
                if (securityList.IsSubmit)
                {
                    List<Security> selectedSecurities = new List<Security>();
                    securityList.GetSelectedSecurities(selectedSecurities);
                    int selectedSecuritiesSize = selectedSecurities.Count;
                    String codes = "";
                    for (int i = 0; i < selectedSecuritiesSize; i++)
                    {
                        codes += selectedSecurities[i].m_code;
                        if (i != selectedSecuritiesSize - 1)
                        {
                            codes += ",";
                        }
                    }
                    category.m_codes = codes;
                    m_userSecurityService.UpdateCategory(m_userSecurityService.OperatorRequestID, category);
                }
            }
            m_window.Invalidate();
        }

        /// <summary>
        /// 表格选中行改变方法
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            String categoryID = "";
            List<GridRow> selectedRows = m_gridCategory.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                categoryID = selectedRows[0].GetCell(0).Text;
            }
            UserSecurityCategory category = new UserSecurityCategory();
            m_userSecurityService.GetCategory(categoryID, ref category);
            BindSecuritiesToSecuritiesGrid(category);
        }

        /// <summary>
        /// 处理最新数据的方法
        /// </summary>
        /// <param name="message">数据</param>
        public void OnLatestDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            m_window.BeginInvoke(copyMessage);
        }

        /// <summary>
        /// 搜索文本框获得焦点方法
        /// </summary>
        public void OnSearchTextBoxGotFocus()
        {
            String categoryID = "";
            List<GridRow> selectedRows = m_gridCategory.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                categoryID = selectedRows[0].GetCell(0).Text;
            }
            m_txtSearch.Tag = categoryID;
            m_chart.ShowSearchDiv('6');
        }

        /// <summary>
        /// 执行秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public void OnTimer(int timerID)
        {
            if (m_timerID == timerID)
            {
                int scrollY = m_gridSecurities.VScrollBar.Pos;
                POINT fPoint = new POINT(0, m_gridSecurities.HeaderHeight + 1 + scrollY);
                POINT ePoint = new POINT(0, m_gridSecurities.Height - 10 + scrollY);
                GridRow fRow = m_gridSecurities.GetRow(fPoint);
                GridRow eRow = m_gridSecurities.GetRow(ePoint);
                while (eRow == null && ePoint.y > 0)
                {
                    ePoint.y -= 10;
                    eRow = m_gridSecurities.GetRow(ePoint);
                }
                if (fRow != null && eRow != null)
                {
                    List<String> requestCodes = new List<String>();
                    for (int i = fRow.Index; i <= eRow.Index; i++)
                    {
                        requestCodes.Add(m_gridSecurities.GetRow(i).GetCell(0).Text);
                    }
                    int requestCodesSize = requestCodes.Count;
                    String strCodes = "";
                    for (int i = 0; i < requestCodesSize; i++)
                    {
                        strCodes += requestCodes[i];
                        if (i != requestCodesSize)
                        {
                            strCodes += ",";
                        }
                    }
                    LatestDataInfo dataInfo = new LatestDataInfo();
                    dataInfo.m_codes = strCodes;
                    dataInfo.m_formatType = 1;
                    m_quoteService.GetLatestDatas(m_latestDataRequestID, dataInfo);
                    requestCodes.Clear();
                }
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                GridColumn column = subControl as GridColumn;
                ButtonA button = subControl as ButtonA;
                GridA grid = subControl as GridA;
                if (column != null)
                {
                    column.AllowResize = true;
                    column.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    column.BorderColor = CDraw.PCOLORS_LINECOLOR3;
                    column.ForeColor = CDraw.PCOLORS_FORECOLOR;
                }
                else if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (grid != null)
                {
                    GridRowStyle rowStyle = new GridRowStyle();
                    grid.RowStyle = rowStyle;
                    rowStyle.BackColor = COLOR.EMPTY;
                    rowStyle.SelectedBackColor = CDraw.PCOLORS_SELECTEDROWCOLOR;
                    rowStyle.HoveredBackColor = CDraw.PCOLORS_HOVEREDROWCOLOR;
                }
                RegisterEvents(subControl);
            }
        }

        /// <summary>
        /// 从表格中删除证券
        /// </summary>
        /// <param name="securities">证券列表</param>
        public void RemoveSecuritiesFromSecuritiesGrid(List<Security> securities)
        {
            Dictionary<String, Security> existsCodes = new Dictionary<String, Security>();
            int securitiesSize = securities.Count;
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = securities[i];
                existsCodes[security.m_code] = security;
            }
            int rowSize = m_gridSecurities.GetRows().Count;
            m_gridSecurities.BeginUpdate();
            for (int i = 0; i < rowSize; i++)
            {
                GridRow row = m_gridSecurities.GetRow(i);
                Security security = new Security();
                if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                {
                    if (existsCodes.ContainsKey(security.m_code))
                    {
                        m_gridSecurities.RemoveRow(row);
                        rowSize--;
                        i--;
                    }
                }
            }
            m_gridSecurities.EndUpdate();
            m_gridSecurities.Invalidate();
        }

        /// <summary>
        /// 从表格中移除类别
        /// </summary>
        /// <param name="categories">类别</param>
        public void RemoveCategoriesFromCategoryGrid(List<UserSecurityCategory> categories)
        {
            if (m_gridCategory.EditTextBox != null)
            {
                m_gridCategory.OnCellEditEnd(null);
            }
            m_gridCategory.BeginUpdate();
            Dictionary<String, String> removeKeys = new Dictionary<String, String>();
            int categoriesSize = categories.Count;
            for (int i = 0; i < categoriesSize; i++)
            {
                UserSecurityCategory category = categories[i];
                removeKeys[category.m_categoryID] = "";
            }
            int rowSize = m_gridCategory.GetRows().Count;
            for (int i = 0; i < rowSize; i++)
            {
                GridRow row = m_gridCategory.GetRow(i);
                String categoryID = row.GetCell(0).GetString();
                if (removeKeys.ContainsKey(categoryID))
                {
                    m_gridCategory.RemoveRow(row);
                    row.ClearCells();
                    row.Dispose();
                    rowSize--;
                    i--;
                }
            }
            removeKeys.Clear();
            m_gridCategory.EndUpdate();
            m_gridCategory.Invalidate();
        }

        /// <summary>
        /// 搜索文本框获得焦点事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void SearchTextBoxGotFocus(object sender)
        {
            OnSearchTextBoxGotFocus();
        }

        /// <summary>
        /// 证券列表窗体关闭事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cancel">是否取消</param>
        private void SecurityListWindowClosed(object sender, ref bool cancel)
        {
            ControlA control = sender as ControlA;
            OnSecurityListWindowClosed(control.Tag as SecurityList, ref cancel);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            m_userSecurityService.GetCategories(categories);
            LoadCategoriesToCategoryGrid(categories);
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            m_window.Invalidate();
            m_window.StartTimer(m_timerID, 1000);
        }

        /// <summary>
        /// 更新表格中的类别
        /// </summary>
        /// <param name="categories">类别</param>
        public void UpdateCategoriesToCategoryGrid(List<UserSecurityCategory> categories)
        {
            Dictionary<String, UserSecurityCategory> updateKeys = new Dictionary<String, UserSecurityCategory>();
            int categoriesSize = categories.Count;
            for (int i = 0; i < categoriesSize; i++)
            {
                UserSecurityCategory category = categories[i];
                updateKeys[category.m_categoryID] = category;
            }
            List<GridRow> rows = m_gridCategory.GetRows();
            int rowSize = rows.Count;
            for (int i = 0; i < rowSize; i++)
            {
                GridRow row = rows[i] as GridRow;
                UserSecurityCategory category = new UserSecurityCategory();
                m_userSecurityService.GetCategory(row.GetCell(0).GetString(), ref category);
                if (updateKeys.ContainsKey(category.m_categoryID))
                {
                    UserSecurityCategory updateCategory = updateKeys[category.m_categoryID];
                    m_userSecurityService.GetCategory(updateCategory.m_categoryID, ref updateCategory);
                    row.GetCell(1).SetString(updateCategory.m_name);
                    BindSecuritiesToSecuritiesGrid(category);
                }
            }
            updateKeys.Clear();
        }

        /// <summary>
        /// 自选股数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void UserSecurityDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            m_window.BeginInvoke(message);
        }
        #endregion
    }
}

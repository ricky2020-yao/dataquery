/*****************************************************************************\
*                                                                             *
* UserSecurityList.cs - User security list functions, types, and definitions. *
*                                                                             *
*               Version 1.00  ����                                          *
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
    /// ����ѡ�ɸ����͵�Ԫ��
    /// </summary>
    public class UserSecurityDoubleCell : GridDoubleCell
    {
        /// <summary>
        /// ������Ԫ��
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
        /// ��ȡ��������ѡ��
        /// </summary>
        public UserSecurityList UserSecurityList
        {
            get { return m_userSecurityList; }
            set { m_userSecurityList = value; }
        }

        /// <summary>
        /// ��ȡ��������ֵ
        /// </summary>
        /// <returns>��������ֵ</returns>
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
        /// ���ݼ۸��ȡ��ɫ
        /// </summary>
        /// <param name="price">�۸�</param>
        /// <param name="comparePrice">�Ƚϼ۸�</param>
        /// <returns>��ɫ</returns>
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
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="rect">����</param>
        /// <param name="clipRect">�ü�����</param>
        /// <param name="isAlternate">�Ƿ�����</param>
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
                    //�ж�ѡ��
                    String text = "-";
                    //���Ʊ���
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
                    //��ȡ��ɫ
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
    /// ��ѡ�ɽ���
    /// </summary>
    public class UserSecurityList : WindowXmlEx
    {
        #region Lord 2016/1/8
        /// <summary>
        /// ������ѡ�ɽ���
        /// </summary>
        /// <param name="native">������</param>
        public UserSecurityList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\UserSecurityList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowUS") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����
            m_timerEvent = new ControlTimerEvent(CallTimer);
            m_window.RegisterEvent(m_timerEvent, EVENTID.TIMER);
            //ע�����¼�
            RegisterEvents(m_window);
            m_gridCategory = GetGrid("gridCategory");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_gridCategory.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_gridCellEditEndEvent = new GridCellEvent(CategoryGridCellEditEnd);
            m_gridCategory.RegisterEvent(m_gridCellEditEndEvent, EVENTID.GRIDCELLEDITEND);
            m_gridSecurities = GetGrid("gridSecurities");
            m_gridCellClick = new GridCellMouseEvent(GridCellClick);
            m_gridSecurities.RegisterEvent(m_gridCellClick, EVENTID.GRIDCELLCLICK);
            //ע�����
            m_securityService = DataCenter.SecurityService;
            m_userSecurityService = DataCenter.UserSecurityService;
            m_userSecurityDataCallBack = new ListenerMessageCallBack(UserSecurityDataCallBack);
            m_userSecurityService.RegisterListener(m_userSecurityService.OperatorRequestID, m_userSecurityDataCallBack);
            //ע������
            m_quoteService = DataCenter.QuoteService;
            m_latestDataCallBack = new ListenerMessageCallBack(LatestDataCallBack);
            m_quoteService.RegisterListener(m_latestDataRequestID, m_latestDataCallBack);
            //�����ı���
            m_txtSearch = GetTextBox("txtSearch");
            m_searchTextBoxGotFocus = new ControlEvent(SearchTextBoxGotFocus);
            m_txtSearch.RegisterEvent(m_searchTextBoxGotFocus, EVENTID.GOTFOCUS);
        }

        /// <summary>
        /// ����
        /// </summary>
        private GridA m_gridCategory;

        /// <summary>
        /// ���Ԫ�����¼�
        /// </summary>
        private GridCellMouseEvent m_gridCellClick;

        /// <summary>
        /// ���Ԫ��༭�����¼�
        /// </summary>
        private GridCellEvent m_gridCellEditEndEvent;

        /// <summary>
        /// ֤ȯ���
        /// </summary>
        private GridA m_gridSecurities;

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// �������ݻص�
        /// </summary>
        private ListenerMessageCallBack m_latestDataCallBack;

        /// <summary>
        /// �������ݵ�����ID
        /// </summary>
        private int m_latestDataRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ���ÿؼ��̷߳����¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// �������
        /// </summary>
        private QuoteService m_quoteService;

        /// <summary>
        /// ��������
        /// </summary>
        public Dictionary<String, SecurityLatestData> m_latestDatas = new Dictionary<String, SecurityLatestData>();

        /// <summary>
        /// �����ı��ı��¼�
        /// </summary>
        private ControlEvent m_searchTextBoxGotFocus;

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// ����¼�
        /// </summary>
        private ControlTimerEvent m_timerEvent;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        /// <summary>
        /// �����ı���
        /// </summary>
        private TextBoxA m_txtSearch;

        /// <summary>
        /// ��ѡ�����ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_userSecurityDataCallBack;

        /// <summary>
        /// ��ѡ�ɷ���
        /// </summary>
        private UserSecurityServiceEx m_userSecurityService;

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ�����ù�Ʊ�ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public void AddCategory()
        {
            UserSecurityCategory category = new UserSecurityCategory();
            category.m_name = "δ��������ѡ��";
            category.m_userID = DataCenter.UserID;
            m_userSecurityService.AddCategory(m_userSecurityService.OperatorRequestID, category);
        }

        /// <summary>
        /// �����𵽱����
        /// </summary>
        /// <param name="categories">���</param>
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
        /// ���֤ȯ�ɷ�
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
        /// ����֤ȯ�������
        /// </summary>
        /// <param name="securities">֤ȯ�б�</param>
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
        /// ����ѡ������е�֤ȯ�����
        /// </summary>
        /// <param name="category">���</param>
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
        /// �����Ԫ��༭����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cell">��Ԫ��</param>
        private void CategoryGridCellEditEnd(object sender, GridCell cell)
        {
            OnCategoryGridCellEditEnd(cell);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="timerID">���ID</param>
        private void CallTimer(object sender, int timerID)
        {
            OnTimer(timerID);
        }

        /// <summary>
        /// �����ť����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                //������
                if (name == "btnAddCategory")
                {
                    AddCategory();
                }
                //���֤ȯ
                else if (name == "btnAddSecurities")
                {
                    AddSecurities();
                }
                //�ر�
                else if (name == "btnClose")
                {
                    Close();
                }
                //ɾ�����
                else if (name == "btnDeleteCategory")
                {
                    DeleteCategory();
                }
                //ɾ��֤ȯ
                else if (name == "btnDeleteSecurities")
                {
                    DeleteSecurities();
                }
            }
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// ɾ�����
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
        /// ɾ��֤ȯ�ɷ�
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
        /// ���ٷ���
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
        /// ���Ԫ�����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 2)
            {
                OnGridCellDoubleClick(cell);
            }
        }

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        /// <param name="sender">������</param>
        private void GridSelectedRowsChanged(object sender)
        {
            OnGridSelectedRowsChanged();
        }

        /// <summary>
        /// �������ݵĻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public void LatestDataCallBack(CMessage message)
        {
            OnLatestDataCallBack(message);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="args">����</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// �����𵽱����
        /// </summary>
        /// <param name="categories">���</param>
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
        /// �����Ԫ��༭��ɷ���
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
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
        /// ���Ԫ��������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        public void OnGridCellDoubleClick(GridCell cell)
        {
            GridRow row = cell.Row;
            Security security = new Security();
            m_securityService.GetSecurityByCode(row.GetCell(0).Text, ref security);
            m_chart.SearchSecurity(security);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
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
        /// ֤ȯ�б���رշ���
        /// </summary>
        /// <param name="securityList">�Ƿ�ȡ��</param>
        /// <param name="cancel">�Ƿ�ȡ��</param>
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
        /// ���ѡ���иı䷽��
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
        /// �����������ݵķ���
        /// </summary>
        /// <param name="message">����</param>
        public void OnLatestDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            m_window.BeginInvoke(copyMessage);
        }

        /// <summary>
        /// �����ı����ý��㷽��
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
        /// ִ�������
        /// </summary>
        /// <param name="timerID">���ID</param>
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
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
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
        /// �ӱ����ɾ��֤ȯ
        /// </summary>
        /// <param name="securities">֤ȯ�б�</param>
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
        /// �ӱ�����Ƴ����
        /// </summary>
        /// <param name="categories">���</param>
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
        /// �����ı����ý����¼�
        /// </summary>
        /// <param name="sender">������</param>
        private void SearchTextBoxGotFocus(object sender)
        {
            OnSearchTextBoxGotFocus();
        }

        /// <summary>
        /// ֤ȯ�б���ر��¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cancel">�Ƿ�ȡ��</param>
        private void SecurityListWindowClosed(object sender, ref bool cancel)
        {
            ControlA control = sender as ControlA;
            OnSecurityListWindowClosed(control.Tag as SecurityList, ref cancel);
        }

        /// <summary>
        /// ��ʾ
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
        /// ���±���е����
        /// </summary>
        /// <param name="categories">���</param>
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
        /// ��ѡ�����ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void UserSecurityDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            m_window.BeginInvoke(message);
        }
        #endregion
    }
}

/************************************************************************************\
*                                                                                    *
* SecurityFilterResult.cs - Security filter result functions, types, and definitions.*
*                                                                                    *
*               Version 1.00  ����                                                 *
*                                                                                    *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.             *
*               Created by Lord 2016/3/17.                                           *
*                                                                                    *
*************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;

namespace KLine
{
    /// <summary>
    /// ���˽����Ԫ��
    /// </summary>
    public class SecurityFilterResultRow : GridRow
    {
        private SecurityFilterInfo m_info;

        /// <summary>
        /// ��ȡ������ѡ����Ϣ
        /// </summary>
        public SecurityFilterInfo Info
        {
            get { return m_info; }
            set { m_info = value; }
        }
    }

    /// <summary>
    /// ����ѡ���ַ�����Ԫ��
    /// </summary>
    public class SecurityFilterResultStringCell : GridStringCell
    {
        /// <summary>
        /// ������Ԫ��
        /// </summary>
        public SecurityFilterResultStringCell()
        {
            GridCellStyle style = new GridCellStyle();
            style.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            style.ForeColor = CDraw.PCOLORS_FORECOLOR10;
            style.Font = new FONT("SimSun", 14, false, false, false);
            Style = style;
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
                    String text = Text;
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
                    //��ȡ��ɫ
                    FONT font = null;
                    long foreColor = COLOR.EMPTY;
                    GridCellStyle style = Style;
                    if (style != null)
                    {
                        if (style.Font != null)
                        {
                            font = style.Font;
                        }
                        foreColor = style.ForeColor;
                    }
                    SecurityFilterInfo info = (row as SecurityFilterResultRow).Info;
                    if (info.GetValue("FILTER") != 1)
                    {
                        foreColor = CDraw.PCOLORS_FORECOLOR8;
                    }
                    SIZE tSize = paint.TextSize(text, font);
                    POINT tPoint = new POINT(rect.left + 1, rect.top + clipH / 2 - tSize.cy / 2);
                    if (column.Name == "colCode")
                    {
                        tPoint.x = rect.right - tSize.cx;
                    }
                    RECT tRect = new RECT(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    paint.DrawText(text, foreColor, font, tRect);
                    if (selected)
                    {
                        paint.DrawLine(CDraw.PCOLORS_LINECOLOR, 2, 0, rect.left, rect.bottom - 1, rect.right, rect.bottom - 1);
                    }
                }
            }
        }
    }


    /// <summary>
    /// ����ѡ�ɸ����͵�Ԫ��
    /// </summary>
    public class SecurityFilterResultDoubleCell : GridDoubleCell
    {
        /// <summary>
        /// ������Ԫ��
        /// </summary>
        public SecurityFilterResultDoubleCell()
        {
            GridCellStyle style = new GridCellStyle();
            style.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            style.ForeColor = CDraw.PCOLORS_FORECOLOR;
            style.Font = new FONT("Arial", 14, false, false, false);
            Style = style;
        }

        private String m_fieldName;

        /// <summary>
        /// ��ȡ�������ֶ�����
        /// </summary>
        public String FieldName
        {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        /// <summary>
        /// ��ȡ��������ֵ
        /// </summary>
        /// <returns>��������ֵ</returns>
        public override double GetDouble()
        {
            GridRow row = Row;
            SecurityFilterInfo info = (row as SecurityFilterResultRow).Info;
            if (m_fieldName != null && m_fieldName.Length > 0)
            {
                return info.GetValue(m_fieldName);
            }
            else
            {
                SecurityLatestData data = info.LatestData;
                int dataSize = data != null ? data.m_securityCode.Length : 0;
                GridColumn column = Column;
                String columnName = column.Name;
                if (columnName == "colNo")
                {
                    return row.Index;
                }
                else if (columnName == "colAmount")
                {
                    if (dataSize > 0)
                    {
                        return data.m_amount;
                    }
                }
                else if (columnName == "colClose")
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
                else if (columnName == "colHigh")
                {
                    if (dataSize > 0)
                    {
                        return data.m_high;
                    }
                }
                else if (columnName == "colLow")
                {
                    if (dataSize > 0)
                    {
                        return data.m_low;
                    }
                }
                else if (columnName == "colBuy")
                {
                    if (dataSize > 0)
                    {
                        return data.m_buyPrice1;
                    }
                }
                else if (columnName == "colSell")
                {
                    if (dataSize > 0)
                    {
                        return data.m_sellPrice1;
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
                else if (columnName == "colOpen")
                {
                    if (dataSize > 0)
                    {
                        return data.m_open;
                    }
                }
                else if (columnName == "colLastClose")
                {
                    if (dataSize > 0)
                    {
                        return data.m_lastClose;
                    }
                }
                else if (columnName == "colVolume")
                {
                    if (dataSize > 0)
                    {
                        return data.m_volume;
                    }
                }
            }
            return 0;
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
                    SecurityFilterInfo info = (row as SecurityFilterResultRow).Info;
                    double value = GetDouble();
                    if (!double.IsNaN(value))
                    {
                        if (m_fieldName != null && m_fieldName.Length > 0)
                        {
                            if (m_fieldName == "FILTER")
                            {
                                if (value == 1)
                                {
                                    foreColor = CDraw.PCOLORS_FORECOLOR9;
                                    text = "��";
                                }
                                else
                                {
                                    foreColor = CDraw.PCOLORS_FORECOLOR7;
                                    text = "��";
                                }
                            }
                            else
                            {
                                foreColor = CDraw.GetPriceColor(value, 0);
                                text = value.ToString("0.0000");
                            }
                        }
                        else
                        {
                            SecurityLatestData data = info.LatestData;
                            String columnName = column.Name;
                            int dataSize = data != null ? data.m_securityCode.Length : 0;
                            if (columnName == "colNo")
                            {
                                foreColor = CDraw.PCOLORS_FORECOLOR7;
                                text = ((int)value + 1).ToString();
                            }
                            else if (columnName == "colAmount" || columnName == "colVolume")
                            {
                                if (dataSize > 0)
                                {
                                    foreColor = CDraw.PCOLORS_FORECOLOR9;
                                    text = ((long)value).ToString();
                                }
                            }
                            else if (columnName == "colDiff")
                            {
                                if (dataSize > 0)
                                {
                                    foreColor = CDraw.GetPriceColor(value, 0);
                                    text = value.ToString("0.00");
                                }
                            }
                            else if (columnName == "colDiffRange")
                            {
                                if (dataSize > 0)
                                {
                                    foreColor = CDraw.GetPriceColor(data.m_close, data.m_lastClose);
                                    text = value.ToString("0.00") + "%";
                                }
                            }
                            else if (columnName == "colLastClose")
                            {
                                if (dataSize > 0)
                                {
                                    foreColor = CDraw.PCOLORS_FORECOLOR9;
                                    text = value.ToString("0.00");
                                }
                            }
                            else
                            {
                                if (dataSize > 0)
                                {
                                    foreColor = CDraw.GetPriceColor(value, data.m_lastClose);
                                    text = value.ToString("0.00");
                                }
                            }
                        }
                    }
                    if (info.GetValue("FILTER") != 1)
                    {
                        foreColor = CDraw.PCOLORS_FORECOLOR8;
                    }
                    SIZE tSize = paint.TextSize(text, font);
                    POINT tPoint = new POINT(rect.right - tSize.cx, rect.top + clipH / 2 - tSize.cy / 2);
                    RECT tRect = new RECT(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    paint.DrawText(text, foreColor, font, tRect);
                    if (selected)
                    {
                        paint.DrawLine(CDraw.PCOLORS_LINECOLOR, 2, 0, rect.left, rect.bottom - 1, rect.right, rect.bottom - 1);
                    }
                }
            }
        }
    }

    /// <summary>
    /// ѡ�ɽ������
    /// </summary>
    public class SecurityFilterResult
    {
        #region Lord 2016/3/17
        /// <summary>
        /// ����ѡ�ɽ������
        /// </summary>
        /// <param name="native">������</param>
        public SecurityFilterResult(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\SecurityFilterResult.xml";
            m_xml = new UIXmlEx();
            m_xml.Native = m_native;
            m_xml.LoadFile(xmlPath, null);
            m_window = m_xml.FindControl("windowSFR") as WindowEx;
            m_gridFilterResult = m_xml.FindControl("gridFilterResult") as GridA;
            m_gridFilterResult.RowStyle.Font = new FONT("SimSun", 14, false, false, false);
            m_gridCellClick = new GridCellMouseEvent(GridCellClick);
            m_gridFilterResult.RegisterEvent(m_gridCellClick, EVENTID.GRIDCELLCLICK);
            //ע�����
            m_timerEvent = new ControlTimerEvent(CallTimer);
            m_window.RegisterEvent(m_timerEvent, EVENTID.TIMER);
            //ע�����¼�
            RegisterEvents(m_window);
        }

        /// <summary>
        /// ���Ԫ�����¼�
        /// </summary>
        private GridCellMouseEvent m_gridCellClick;

        /// <summary>
        /// ����ѡ�ɽ�����
        /// </summary>
        private GridA m_gridFilterResult;

        /// <summary>
        /// ����״̬
        /// </summary>
        private int m_runningState = -1;

        /// <summary>
        /// ����¼�
        /// </summary>
        private ControlTimerEvent m_timerEvent;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private OwChart m_chart;

        /// <summary>
        /// ��Ʊ�ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private bool m_isDisposed;

        /// <summary>
        /// ��ȡ�Ƿ��ѱ�����
        /// </summary>
        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }

        private INativeBase m_native;

        /// <summary>
        /// ��ȡ�����÷�����
        /// </summary>
        public INativeBase Native
        {
            get { return m_native; }
        }

        private SecurityFilterTask m_task;

        /// <summary>
        /// ��ȡ����������ѡ������
        /// </summary>
        public SecurityFilterTask Task
        {
            get { return m_task; }
            set { m_task = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        private UIXmlEx m_xml;

        /// <summary>
        /// ��ȡ������XML�ĵ�
        /// </summary>
        public UIXmlEx Xml
        {
            get { return m_xml; }
        }

        /// <summary>
        /// ���ָ����
        /// </summary>
        private void AddIndicatorColumns()
        {
            if (m_task != null)
            {
                m_gridFilterResult.BeginUpdate();
                CIndicator indicator = m_task.Indicator;
                foreach (String name in indicator.MainVariables.Keys)
                {
                    GridColumn column = new GridColumn(name);
                    column.Name = name;
                    column.TextAlign = ContentAlignmentA.MiddleRight;
                    column.Width = 80;
                    m_gridFilterResult.AddColumn(column);
                    if (name == "FILTER")
                    {
                        column.Text = "��������";
                    }
                }
                List<GridColumn> columns = m_gridFilterResult.GetColumns();
                int columnsSize = columns.Count;
                for (int i = 0; i < columnsSize; i++)
                {
                    GridColumn column = columns[i];
                    column.AllowResize = true;
                    column.BackColor = CDraw.PCOLORS_BACKCOLOR4;
                    column.BorderColor = COLOR.EMPTY;
                    column.Font = new FONT("SimSun", 14, false, false, false);
                    column.ForeColor = CDraw.PCOLORS_FORECOLOR7;
                }
                m_gridFilterResult.EndUpdate();
            }
        }

        /// <summary>
        /// ���ָ����
        /// </summary>
        private void AddIndicatorRows()
        {
            List<SecurityFilterInfo> infos = new List<SecurityFilterInfo>();
            m_task.GetSecurityInfos(infos);
            m_gridFilterResult.BeginUpdate();
            int infosSize = infos.Count;
            for (int i = 0; i < infosSize; i++)
            {
                SecurityFilterInfo info = infos[i];
                SecurityFilterResultRow row = new SecurityFilterResultRow();
                row.Height = 30;
                row.Info = info;
                m_gridFilterResult.AddRow(row);
                SecurityFilterResultDoubleCell noCell = new SecurityFilterResultDoubleCell();
                row.AddCell(0, noCell);
                SecurityFilterResultStringCell nameCell = new SecurityFilterResultStringCell();
                nameCell.Text = info.Security.m_name;
                row.AddCell(1, nameCell);
                SecurityFilterResultDoubleCell closeCell = new SecurityFilterResultDoubleCell();
                row.AddCell(2, closeCell);
                SecurityFilterResultDoubleCell diffCell = new SecurityFilterResultDoubleCell();
                row.AddCell(3, diffCell);
                SecurityFilterResultDoubleCell volumeCell = new SecurityFilterResultDoubleCell();
                row.AddCell(4, volumeCell);
                SecurityFilterResultDoubleCell highCell = new SecurityFilterResultDoubleCell();
                row.AddCell(5, highCell);
                SecurityFilterResultDoubleCell lowCell = new SecurityFilterResultDoubleCell();
                row.AddCell(6, lowCell);
                SecurityFilterResultDoubleCell buyCell = new SecurityFilterResultDoubleCell();
                row.AddCell(7, buyCell);
                SecurityFilterResultDoubleCell sellCell = new SecurityFilterResultDoubleCell();
                row.AddCell(8, sellCell);
                SecurityFilterResultDoubleCell diffRange = new SecurityFilterResultDoubleCell();
                row.AddCell(9, diffRange);
                SecurityFilterResultDoubleCell openCell = new SecurityFilterResultDoubleCell();
                row.AddCell(10, openCell);
                SecurityFilterResultStringCell codeCell = new SecurityFilterResultStringCell();
                codeCell.Style.Font = new FONT("Arial", 14, false, false, false);
                codeCell.Text = info.Security.m_code;
                row.AddCell(11, codeCell);
                SecurityFilterResultDoubleCell lastCloseCell = new SecurityFilterResultDoubleCell();
                row.AddCell(12, lastCloseCell);
                SecurityFilterResultDoubleCell amountCell = new SecurityFilterResultDoubleCell();
                row.AddCell(13, amountCell);
                CIndicator indicator = m_task.Indicator;
                int index = 14;
                foreach (String name in indicator.MainVariables.Keys)
                {
                    SecurityFilterResultDoubleCell indicatorCell = new SecurityFilterResultDoubleCell();
                    indicatorCell.FieldName = name;
                    row.AddCell(index, indicatorCell);
                    index++;
                }
            }
            m_gridFilterResult.EndUpdate();
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
                //�ر�
                if (name == "btnClose")
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                m_chart = null;
                if (m_gridFilterResult != null)
                {
                    m_gridFilterResult.UnRegisterEvent(m_gridCellClick, EVENTID.GRIDCELLCLICK);
                    m_gridCellClick = null;
                    m_gridFilterResult = null;
                }
                if (m_window != null)
                {
                    m_window.StopTimer(m_timerID);
                    m_window.UnRegisterEvent(m_timerEvent, EVENTID.TIMER);
                    m_timerEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                if (m_xml != null)
                {
                    m_xml.Dispose();
                    m_xml = null;
                }
                m_isDisposed = true;
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
            SecurityFilterInfo info = (cell.Row as SecurityFilterResultRow).Info;
            if (info != null && m_chart != null)
            {
                m_chart.Chart.DataSource.Clear();
                SecurityFilterTemplate template = m_task.Template;
                m_chart.DeleteIndicators(true);
                List<CDiv> divs = m_chart.Chart.GetDivs();
                int divsSize = divs.Count;
                m_chart.AddMainIndicator(template.m_name, template.m_name, template.m_indicator, template.m_parameters, divs[divsSize - 1], true);
                m_chart.Cycle = m_task.Template.m_cycle;
                m_chart.Subscription = m_task.Template.m_subscription;
                m_chart.SearchSecurity(info.Security);
            }
        }

        /// <summary>
        /// ִ�������
        /// </summary>
        /// <param name="timerID">���ID</param>
        public void OnTimer(int timerID)
        {
            if (m_timerID == timerID)
            {
                if (m_task != null)
                {
                    int runningState = m_task.RunningState;
                    if (runningState != m_runningState)
                    {
                        int size = m_task.GetSecurityInfosSize();
                        int matchCount = m_task.MatchCount;
                        String title = "����ѡ��";
                        if (runningState == -1)
                        {
                            if (size > 0)
                            {
                                title += "��������������(" + matchCount.ToString() + "/" + size.ToString() + ")";
                            }
                        }
                        else if (runningState == 0)
                        {
                            title += "(������������...)";
                        }
                        else
                        {
                            if (size > 0)
                            {
                                title += "(�����У����ȣ�" + runningState.ToString() + "/" + size.ToString() + "����������" + matchCount.ToString() + ")";
                            }
                        }
                        if (runningState != m_runningState && runningState == -1)
                        {
                            List<GridColumn> columns = m_gridFilterResult.GetColumns();
                            GridColumn filterColumn = null;
                            int columnsSize = columns.Count;
                            for (int i = 0; i < columnsSize; i++)
                            {
                                if (columns[i].Name == "FILTER")
                                {
                                    filterColumn = columns[i];
                                    break;
                                }
                            }
                            if (filterColumn != null)
                            {
                                m_gridFilterResult.SortColumn(m_gridFilterResult, filterColumn, GridColumnSortMode.Desc);
                                m_gridFilterResult.Update();
                            }
                        }
                        m_window.Text = title;
                        m_runningState = runningState;
                        m_window.Invalidate();
                    }
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
                ButtonA button = controls[i] as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public void Show()
        {
            AddIndicatorColumns();
            AddIndicatorRows();
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
            m_window.StartTimer(m_timerID, 1000);
        }
        #endregion
    }
}

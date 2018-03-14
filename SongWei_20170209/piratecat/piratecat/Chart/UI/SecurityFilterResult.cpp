#include "..\\..\\stdafx.h"
#include "SecurityFilterResult.h"

SecurityFilterResultRow::SecurityFilterResultRow()
{
	m_info = 0;
}

SecurityFilterResultRow::~SecurityFilterResultRow()
{
	m_info = 0;
}

SecurityFilterInfo* SecurityFilterResultRow::GetInfo()
{
	return m_info;
}

void SecurityFilterResultRow::SetInfo(SecurityFilterInfo *info)
{
	m_info = info;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityFilterResultStringCell::SecurityFilterResultStringCell()
{
    GridCellStyle style;
    style.SetBackColor(PCOLORS_BACKCOLOR4);
    style.SetForeColor(PCOLORS_FORECOLOR10);
	FONT sFont(L"SimSun", 14, false, false, false);
    style.SetFont(&sFont);
    SetStyle(&style);
}

SecurityFilterResultStringCell::~SecurityFilterResultStringCell()
{
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityFilterResultStringCell::OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate)
{
	int clipW = clipRect.right - clipRect.left;
    int clipH = clipRect.bottom - clipRect.top;
    if (clipW > 0 && clipH > 0)
    {
        GridA *grid = GetGrid();
        GridRow *row = GetRow();
        GridColumn *column = GetColumn();
        if (grid && row && column)
        {
            String text = GetText();
            bool selected = false;
            vector<GridRow*> selectedRows = grid->GetSelectedRows();
            int selectedRowsSize = (int)selectedRows.size();
            for (int i = 0; i < selectedRowsSize; i++)
            {
                if (selectedRows[i] == row)
                {
                    selected = true;
                    break;
                }
            }
            FONT *font = 0;
            _int64 foreColor = COLOR_EMPTY;
            GridCellStyle *style = GetStyle();
            if (style)
            {
                if (style->GetFont())
                {
                    font = style->GetFont();
                }
                foreColor = style->GetForeColor();
            }
			SecurityFilterInfo *info = dynamic_cast<SecurityFilterResultRow*>(row)->GetInfo(); 
            if (info->GetValue(L"FILTER") != 1)
            {
                foreColor = PCOLORS_FORECOLOR8;
            }
            SIZE tSize = paint->TextSize(text.c_str(), font);
			POINT tPoint = {rect.left + 1, rect.top + clipH / 2 - tSize.cy / 2};
            if (column->GetName() == L"colCode")
            {
                tPoint.x = rect.right - tSize.cx;
            }
			RECT tRect = {tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy};
			paint->DrawText(text.c_str(), foreColor, font, tRect);
            if (selected)
            {
                paint->DrawLine(PCOLORS_LINECOLOR, 2, 0, rect.left, rect.bottom - 1, rect.right, rect.bottom - 1);
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityFilterResultDoubleCell::SecurityFilterResultDoubleCell()
{
	m_fieldName = L"";
    GridCellStyle style;
    style.SetBackColor(PCOLORS_BACKCOLOR4);
    style.SetForeColor(PCOLORS_FORECOLOR);
	FONT sFont(L"Arial", 14, false, false, false);
    style.SetFont(&sFont);
    SetStyle(&style);
}

SecurityFilterResultDoubleCell::~SecurityFilterResultDoubleCell()
{
}

String SecurityFilterResultDoubleCell::GetFieldName()
{
	return m_fieldName;
}

void SecurityFilterResultDoubleCell::SetFieldName(const String& fieldName)
{
	m_fieldName = fieldName;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

double SecurityFilterResultDoubleCell::GetDouble()
{
	GridRow *row = GetRow();
	SecurityFilterInfo *info = dynamic_cast<SecurityFilterResultRow*>(row)->GetInfo();
	if (m_fieldName.length() > 0)
    {
        return info->GetValue(m_fieldName);
    }
    else
    {
        SecurityLatestData data = info->GetLatestData();
		int dataSize = (int)data.m_securityCode.length();
        GridColumn *column = GetColumn();
        String columnName = column->GetName();
        if (columnName == L"colNo")
        {
            return row->GetIndex();
        }
        else if (columnName == L"colAmount")
        {
            if (dataSize > 0)
            {
                return data.m_amount;
            }
        }
        else if (columnName == L"colClose")
        {
            if (dataSize > 0)
            {
                return data.m_close;
            }
        }
        else if (columnName == L"colDiff")
        {
            if (dataSize > 0)
            {
                return data.m_close - data.m_lastClose;
            }
        }
        else if (columnName == L"colHigh")
        {
            if (dataSize > 0)
            {
                return data.m_high;
            }
        }
        else if (columnName == L"colLow")
        {
            if (dataSize > 0)
            {
                return data.m_low;
            }
        }
        else if (columnName == L"colBuy")
        {
            if (dataSize > 0)
            {
                return data.m_buyPrice1;
            }
        }
        else if (columnName == L"colSell")
        {
            if (dataSize > 0)
            {
                return data.m_sellPrice1;
            }
        }
        else if (columnName == L"colDiffRange")
        {
            if (dataSize > 0)
            {
                if (data.m_lastClose != 0)
                {
                    return 100 * (data.m_close - data.m_lastClose) / data.m_lastClose;
                }
            }
        }
        else if (columnName == L"colOpen")
        {
            if (dataSize > 0)
            {
                return data.m_open;
            }
        }
        else if (columnName == L"colLastClose")
        {
            if (dataSize > 0)
            {
                return data.m_lastClose;
            }
        }
        else if (columnName == L"colVolume")
        {
            if (dataSize > 0)
            {
                return data.m_volume;
            }
        }
    }
    return 0;
}

void SecurityFilterResultDoubleCell::OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate)
{
	int clipW = clipRect.right - clipRect.left;
    int clipH = clipRect.bottom - clipRect.top;
    if (clipW > 0 && clipH > 0)
    {
        GridA *grid = GetGrid();
        GridRow *row = GetRow();
        GridColumn *column = GetColumn();
        if (grid && row && column)
        {
            String text = L"-";
            bool selected = false;
            vector<GridRow*> selectedRows = grid->GetSelectedRows();
            int selectedRowsSize = (int)selectedRows.size();
            for (int i = 0; i < selectedRowsSize; i++)
            {
                if (selectedRows[i] == row)
                {
                    selected = true;
                    break;
                }
            }
            FONT *font = 0;
            _int64 foreColor = COLOR_EMPTY;
            GridCellStyle *style = GetStyle();
            if (style)
            {
                if (style->GetFont())
                {
                    font = style->GetFont();
                }
                foreColor = style->GetForeColor();
            }
			SecurityFilterInfo *info = dynamic_cast<SecurityFilterResultRow*>(row)->GetInfo(); 
            double value = GetDouble();
            if (!info->GetDataSource()->IsNaN(value))
            {
				wchar_t str[20] = {0};
                if (m_fieldName.length() > 0)
                {
                    if (m_fieldName == L"FILTER")
                    {
                        if (value == 1)
                        {
                            foreColor = PCOLORS_FORECOLOR9;
                            text = L"是";
                        }
                        else
                        {
                            foreColor = PCOLORS_FORECOLOR7;
                            text = L"否";
                        }
                    }
                    else
                    {
						foreColor = CDraw::GetPriceColor(value, 0);
						_stprintf_s(str, 19, L"%.4f", value);
                        text = str;
                    }
                }
                else
                {
                    SecurityLatestData data = info->GetLatestData();
                    String columnName = column->GetName();
                    int dataSize = (int)data.m_securityCode.length();
                    if (columnName == L"colNo")
                    {
                        foreColor = PCOLORS_FORECOLOR7;
						_stprintf_s(str, 19, L"%d", (int)value + 1);
                        text = str;
                    }
                    else if (columnName == L"colAmount" || columnName == L"colVolume")
                    {
                        if (dataSize > 0)
                        {
                            foreColor = PCOLORS_FORECOLOR9;
							_stprintf_s(str, 19, L"%ld", (_int64)value);
                            text = str;
                        }
                    }
                    else if (columnName == L"colDiff")
                    {
                        if (dataSize > 0)
                        {
							foreColor = CDraw::GetPriceColor(value, 0);
							_stprintf_s(str, 19, L"%.2f", value);
                            text = str;
                        }
                    }
                    else if (columnName == L"colDiffRange")
                    {
                        if (dataSize > 0)
                        {
							foreColor = CDraw::GetPriceColor(data.m_close, data.m_lastClose);
							_stprintf_s(str, 19, L"%.2f", value);
                            text = str;
							text += L"%";
                        }
                    }
                    else if (columnName == L"colLastClose")
                    {
                        if (dataSize > 0)
                        {
                            foreColor = PCOLORS_FORECOLOR9;
							_stprintf_s(str, 19, L"%.2f", value);
                            text = str;
                        }
                    }
                    else
                    {
                        if (dataSize > 0)
                        {
							foreColor = CDraw::GetPriceColor(value, data.m_lastClose);
							_stprintf_s(str, 19, L"%.2f", value);
                            text = str;
                        }
                    }
                }
            }
            if (info->GetValue(L"FILTER") != 1)
            {
                foreColor = PCOLORS_FORECOLOR8;;
            }
			SIZE tSize = paint->TextSize(text.c_str(), font);
			POINT tPoint = {rect.right - tSize.cx, rect.top + clipH / 2 - tSize.cy / 2};
			RECT tRect = {tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy};
			paint->DrawText(text.c_str(), foreColor, font, tRect);
            if (selected)
            {
                paint->DrawLine(PCOLORS_LINECOLOR, 2, 0, rect.left, rect.bottom - 1, rect.right, rect.bottom - 1);
            }
        }
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityFilterResult::AddIndicatorColumns()
{
	if (m_task)
    {
        m_gridFilterResult->BeginUpdate();
        CIndicator *indicator = m_task->GetIndicator();
		map<String, int>::iterator sIter = indicator->m_mainVariables.begin();
		for(; sIter != indicator->m_mainVariables.end(); ++sIter)
		{
			String name = sIter->first;
			GridColumn *column = new GridColumn(name);
            column->SetName(name);
            column->SetTextAlign(ContentAlignmentA_MiddleRight);
            column->SetWidth(80);
            m_gridFilterResult->AddColumn(column);
            if (name == L"FILTER")
            {
                column->SetText(L"符合条件");
            }
		}
        vector<GridColumn*> columns = m_gridFilterResult->m_columns;
		vector<GridColumn*>::iterator sIter2 = columns.begin();
		FONT colFont(L"SimSun", 14, false, false, false);
		for(; sIter2 != columns.end(); ++sIter2)
		{
			GridColumn *column = *sIter2;
			column->SetAllowResize(true);
            column->SetBackColor(PCOLORS_BACKCOLOR4);
            column->SetBorderColor(COLOR_EMPTY);
            column->SetFont(&colFont);
            column->SetForeColor(PCOLORS_FORECOLOR7);
		}
        m_gridFilterResult->EndUpdate();
    }
}

void SecurityFilterResult::AddIndicatorRows()
{
    vector<SecurityFilterInfo*> infos;
    m_task->GetSecurityInfos(&infos);
    m_gridFilterResult->BeginUpdate();
	vector<SecurityFilterInfo*>::iterator sIter = infos.begin();
	for(; sIter != infos.end(); ++sIter)
	{
		SecurityFilterInfo *info = *sIter;
		SecurityFilterResultRow *row = new SecurityFilterResultRow;
        row->SetHeight(30);
        row->SetInfo(info);
        m_gridFilterResult->AddRow(row);
        SecurityFilterResultDoubleCell *noCell = new SecurityFilterResultDoubleCell;
        row->AddCell(0, noCell);
        SecurityFilterResultStringCell *nameCell = new SecurityFilterResultStringCell;
        nameCell->SetText(info->GetSecurity().m_name);
        row->AddCell(1, nameCell);
        SecurityFilterResultDoubleCell *closeCell = new SecurityFilterResultDoubleCell;
        row->AddCell(2, closeCell);
        SecurityFilterResultDoubleCell *diffCell = new SecurityFilterResultDoubleCell;
        row->AddCell(3, diffCell);
        SecurityFilterResultDoubleCell *volumeCell = new SecurityFilterResultDoubleCell;
        row->AddCell(4, volumeCell);
        SecurityFilterResultDoubleCell *highCell = new SecurityFilterResultDoubleCell;
        row->AddCell(5, highCell);
        SecurityFilterResultDoubleCell *lowCell = new SecurityFilterResultDoubleCell;
        row->AddCell(6, lowCell);
        SecurityFilterResultDoubleCell *buyCell = new SecurityFilterResultDoubleCell;
        row->AddCell(7, buyCell);
        SecurityFilterResultDoubleCell *sellCell = new SecurityFilterResultDoubleCell;
        row->AddCell(8, sellCell);
        SecurityFilterResultDoubleCell *diffRange = new SecurityFilterResultDoubleCell;
        row->AddCell(9, diffRange);
        SecurityFilterResultDoubleCell *openCell = new SecurityFilterResultDoubleCell;
        row->AddCell(10, openCell);
        SecurityFilterResultStringCell *codeCell = new SecurityFilterResultStringCell;
		FONT cFont(L"Arial", 14, false, false, false);
        codeCell->GetStyle()->SetFont(&cFont);
        codeCell->SetText(info->GetSecurity().m_code);
        row->AddCell(11, codeCell);
        SecurityFilterResultDoubleCell *lastCloseCell = new SecurityFilterResultDoubleCell;
        row->AddCell(12, lastCloseCell);
        SecurityFilterResultDoubleCell *amountCell = new SecurityFilterResultDoubleCell;
        row->AddCell(13, amountCell);
        CIndicator *indicator = m_task->GetIndicator();
        int index = 14;
		map<String, int>::iterator sIter2 = indicator->m_mainVariables.begin();
		for(; sIter2 != indicator->m_mainVariables.end(); ++sIter2)
		{
			String name = sIter2->first;
			SecurityFilterResultDoubleCell *indicatorCell = new SecurityFilterResultDoubleCell;
            indicatorCell->SetFieldName(name);
            row->AddCell(index, indicatorCell);
            index++;
		}
	}
    m_gridFilterResult->EndUpdate();
}

void SecurityFilterResult::CallTimer(void *sender, int timerID, void *pInvoke)
{
	SecurityFilterResult *securityFilterResult = (SecurityFilterResult*)pInvoke;
	securityFilterResult->OnTimer(timerID);
}

void SecurityFilterResult::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		SecurityFilterResult *securityFilterResult = (SecurityFilterResult*)pInvoke;
        if (name == L"btnClose")
        {
            securityFilterResult->Close();
        }
    }
}

void SecurityFilterResult::GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	SecurityFilterResult *securityFilterResult = (SecurityFilterResult*)pInvoke;
    if (button == MouseButtonsA_Left && clicks == 2)
    {
        securityFilterResult->OnGridCellDoubleClick(cell);
    }
}

void SecurityFilterResult::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ButtonA *button = dynamic_cast<ButtonA*>(*sIter);
		if(button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		RegisterEvents(*sIter);
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityFilterResult::SecurityFilterResult(NativeBase *native)
{
	m_chart = 0;
    m_native = native;
	m_runningState = -1;
	m_task = 0;
	m_timerID = ControlA::GetNewTimerID();
	string xmlPath = DataCenter::GetAppPath() + "\\config\\SecurityFilterResult.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    m_xml = new UIXmlEx;
    m_xml->SetNative(m_native);
    m_xml->LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(m_xml->FindControl(L"windowSFR"));
    m_gridFilterResult = dynamic_cast<GridA*>(m_xml->FindControl(L"gridFilterResult"));
	FONT gFont(L"SimSun", 14, false, false, false);
    m_gridFilterResult->GetRowStyle()->SetFont(&gFont);
    m_gridCellClick = &GridCellClick;
	m_gridFilterResult->RegisterEvent(m_gridCellClick, EVENTID::GRIDCELLCLICK, this);
    m_timerEvent = &CallTimer;
	m_window->RegisterEvent(m_timerEvent, EVENTID::TIMER, this);
    RegisterEvents(m_window);
}

SecurityFilterResult::~SecurityFilterResult()
{
    m_chart = 0;
    if (m_gridFilterResult)
    {
		m_gridFilterResult->UnRegisterEvent(m_gridCellClick, EVENTID::GRIDCELLCLICK);
        m_gridCellClick = 0;
        m_gridFilterResult = 0;
    }
	m_task = 0;
    if (m_window)
    {
        m_window->StopTimer(m_timerID);
		m_window->UnRegisterEvent(m_timerEvent, EVENTID::TIMER);
        m_timerEvent = 0;
		if(m_window->GetParent())
		{
			m_window->Close();
		}
		delete m_window;
        m_window = 0;
    }
    if (m_xml)
    {
        delete m_xml;
        m_xml = 0;
    }
}

OwChart* SecurityFilterResult::GetChart()
{
	return m_chart;
}

void SecurityFilterResult::SetChart(OwChart *chart)
{
	m_chart = chart;
}

NativeBase* SecurityFilterResult::GetNative()
{
	return m_native;
}

SecurityFilterTask* SecurityFilterResult::GetTask()
{
	return m_task;
}

void SecurityFilterResult::SetTask(SecurityFilterTask *task)
{
	m_task = task;
}

WindowEx* SecurityFilterResult::GetWindow()
{
	return m_window;
}

UIXmlEx* SecurityFilterResult::GetXml()
{
	return m_xml;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityFilterResult::Close()
{
	ControlA *focusedControl = m_native->GetFocusedControl();
	if(focusedControl)
	{
		m_native->RemoveControl(focusedControl);
	}
	m_window->Close();
	m_native->Invalidate();
	delete this;
}

void SecurityFilterResult::OnGridCellDoubleClick(GridCell *cell)
{
    SecurityFilterInfo *info = dynamic_cast<SecurityFilterResultRow*>(cell->GetRow())->GetInfo();
    if (info && m_chart)
    {
        m_chart->GetChart()->GetDataSource()->Clear();
        SecurityFilterTemplate sfTemplate = m_task->GetTemplate();
        m_chart->DeleteIndicators(true);
        vector<CDiv*> divs = m_chart->GetChart()->GetDivs();
        int divsSize = (int)divs.size();
        m_chart->AddMainIndicator(sfTemplate.m_name, sfTemplate.m_name, sfTemplate.m_indicator, sfTemplate.m_parameters, divs[divsSize - 1], true);
		m_chart->SetCycle(sfTemplate.m_cycle);
		m_chart->SetSubscription(sfTemplate.m_subscription);
        m_chart->SearchSecurity(&info->GetSecurity());
    }
}

void SecurityFilterResult::OnTimer(int timerID)
{
    if (m_timerID == timerID)
    {
        if (m_task)
        {
            int runningState = m_task->GetRunningState();
			if(m_runningState != runningState)
			{
				int size = m_task->GetSecurityInfosSize();
				int matchCount = m_task->GetMatchCount();
				String title = L"条件选股";
				if (runningState == -1)
				{
					if (size > 0)
					{
						wchar_t str[100] = {0};
						_stprintf_s(str, 99, L" 符合条件数：(%d/%d)", matchCount, size);
						title += str;
					}
				}
				else if (runningState == 0)
				{
					title += L"(正在下载数据...)";
				}
				else
				{
					if (size > 0)
					{
						wchar_t str[100] = {0};
						_stprintf_s(str, 99, L"计算中，进度：%d/%d，符合数：%d", runningState, size, matchCount);
						title += str;
					}
				}
				if (runningState != m_runningState && runningState == -1)
				{
					vector<GridColumn*> columns = m_gridFilterResult->m_columns;
					GridColumn *filterColumn = 0;
					vector<GridColumn*>::iterator sIter = columns.begin();
					for(; sIter != columns.end(); ++sIter)
					{
						GridColumn *column = *sIter;
						if(column->GetName() == L"FILTER")
						{
							filterColumn = column;
							break;
						}
					}
					if (filterColumn)
					{
						m_gridFilterResult->Sort(filterColumn, GridColumnSortMode_Desc);
						m_gridFilterResult->Update();
					}
				}
				m_runningState = runningState;
				m_window->SetText(title);
				m_window->Invalidate();
			}
        }
    }
}

void SecurityFilterResult::Show()
{
	AddIndicatorColumns();
    AddIndicatorRows();
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(false);
    m_window->Invalidate();
    m_window->StartTimer(m_timerID, 1000);
}
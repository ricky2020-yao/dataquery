#include "..\\..\\stdafx.h"
#include "SandBoxWindow.h"

int SandBoxCandleDiv::GetMovingLocation()
{
    int height = GetHeight();
    m_perFixMoveStep = 10;
    GetDestPos(&m_destPoint, &m_destHighY);
    int tempLocationY = m_originLocation.y - m_totalDistance;
    if (m_direction == 1)
    {
        tempLocationY = m_originLocation.y + m_totalDistance + height;
    }
    m_tempStops.x = m_originLocation.x;
    m_tempStops.y = tempLocationY;
    float slope = 0, bYIntercept = 0;
    if (m_tempStops.y > m_destPoint.y)
    {
        slope = (float)(m_destPoint.y - m_tempStops.y + 5) / (float)(m_destPoint.x - m_tempStops.x);
        bYIntercept = m_destPoint.y - slope * m_destPoint.x;
    }
    else
    {
        slope = (float)(m_tempStops.y - m_destPoint.y + 5) / (float)(m_tempStops.x - m_destPoint.x);
        bYIntercept = m_destPoint.y - slope * m_destPoint.x;
    }
    m_slope = slope;
    m_yIntercept = bYIntercept;
    m_perDyncMoveStep = (float)((m_destPoint.x - m_tempStops.x + 5) / m_perFixMoveTimes);
    return 1;
}

void SandBoxCandleDiv::GetDestPos(POINT *location, float *destHighY)
{
    ChartA *chart = m_chart->GetChart();
    CTable *dataSource = chart->GetDataSource();
    int rowIndex = dataSource->GetRowIndex((double)m_data.m_date);
    int insRowIndex = rowIndex;
    int size = dataSource->RowsCount();
    if (rowIndex == -1)
    {
        for (int i = 0; i < size; i++)
        {
            double pk = dataSource->GetXValue(i);
            if (pk > m_data.m_date)
            {
                insRowIndex = i;
                break;
            }
        }
    }
    if (insRowIndex == -1 || insRowIndex > chart->GetLastVisibleIndex())
    {
        insRowIndex = chart->GetLastVisibleIndex();
    }
    double insLow = dataSource->Get2(insRowIndex, LOW_INDEX);
    float xDest = chart->GetX(insRowIndex);
    vector<CDiv*> divs = chart->GetDivs();
    float yDest = chart->GetY(divs[0], insLow, AttachVScale_Left);
    location->x = (int)(xDest);
    location->y = (int)(yDest);
    *destHighY = chart->GetY(divs[0], m_data.m_high, AttachVScale_Left);
}

void SandBoxCandleDiv::GetDestSize(SIZE *size)
{
    int cw = 0, ch = 0;
    ChartA *chart = m_chart->GetChart();
    CTable *dataSource = chart->GetDataSource();
    int rowsCount = dataSource->RowsCount();
    if (rowsCount <= 0)
    {
        cw = 0;
    }
    else if (rowsCount == 1)
    {
        cw = (int)(chart->GetX(0) * 2 / 3);
    }
    else
    {
        int index1 = rowsCount - 1, idnex2 = rowsCount - 2;
        cw = (int)((chart->GetX(rowsCount - 1) - chart->GetX(rowsCount - 2)) * 2 / 3);
    }
    vector<CDiv*> divs = chart->GetDivs();
    ch = (int)(chart->GetY(divs[0], m_data.m_low, AttachVScale_Left) - chart->GetY(divs[0], m_data.m_high, AttachVScale_Left));
    size->cx = cw;
    size->cy = ch;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SandBoxCandleDiv::SandBoxCandleDiv()
{
	m_chart = 0;
	m_destHighY = 0;
	m_destPoint.x = 0;
	m_destPoint.x = 0;
	m_direction = 0;
	m_moveStart = false;
	m_moving = false;
	m_originLocation.x = 0;
	m_originLocation.y = 0;
	m_perDyncMoveStep = 0;
	m_perFixMoveStep = 10;
	m_perFixMoveTimes = 10;
	m_slope = 0;
	m_tempStops.x = 0;
	m_tempStops.y = 0;
	m_ticks = 0;
	m_totalDistance = 50;
	m_yIntercept = 0;
	SetBackColor(COLOR_EMPTY);
	SetBorderColor(COLOR_EMPTY);
	SetTopMost(true);
}

SandBoxCandleDiv::~SandBoxCandleDiv()
{
	m_chart = 0;
}

OwChart* SandBoxCandleDiv::GetChart()
{
	return m_chart;
}

void SandBoxCandleDiv::SetChart(OwChart* chart)
{
	m_chart = chart;
}

SecurityData SandBoxCandleDiv::GetData()
{
	return m_data;
}

void SandBoxCandleDiv::SetData(SecurityData data)
{
	m_data = data;
}

int SandBoxCandleDiv::GetDirection()
{
	return m_direction;
}

void SandBoxCandleDiv::SetDirection(int direction)
{
	m_direction = direction;
}

bool SandBoxCandleDiv::IsMoving()
{
	return m_moving;
}

void SandBoxCandleDiv::SetMoving(bool moving)
{
	if(moving)
	{
        m_moving = moving;
        m_originLocation = GetLocation();
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SandBoxCandleDiv::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth(), height = GetHeight();
	int left = 0, right = width, top = 0, bottom = height;
	if (!m_moving)
	{
		paint->DrawRect(PCOLORS_LINECOLOR, 1, 0, clipRect);
		width -= 10;
		height -= 5;
		left = 5;
		right = left + width;
		top = 5;
		bottom = height + top;
	}
	if (width % 2 == 0)
	{
		width -= 1;
	}
	_int64 color = PCOLORS_UPCOLOR;
	if (m_data.m_low >= m_data.m_high
		|| m_data.m_open < m_data.m_low
		|| m_data.m_close < m_data.m_low
		|| m_data.m_open > m_data.m_high
		|| m_data.m_close > m_data.m_high)
	{
		RECT rect = {left, top, right, top + 1};
		paint->FillRect(color, rect);
		return;
	}
	double perLength = (double)height / (m_data.m_high - m_data.m_low);
	POINT pointStartUp = {0};
	POINT pointEndUp = {0};
	POINT pointStartDown = {0};
	POINT pointEndDown = {0};
	bool drawUpLine = false, drawDownLine = false;
	int midX = left + width / 2 + 1;
	if (m_data.m_close >= m_data.m_open)
	{
        top += (int)((m_data.m_high - m_data.m_close) * perLength);
        bottom = 5 + (int)((m_data.m_high - m_data.m_open) * perLength);
        if (m_data.m_high != m_data.m_close)
        {
            pointStartUp.x = midX;
            pointStartUp.y = 5;
            pointEndUp.x = midX;
            pointEndUp.y = top;
            drawUpLine = true;
        }
        if (m_data.m_low != m_data.m_open)
        {
            pointStartDown.x = midX;
            pointStartDown.y = bottom;
            pointEndDown.x = midX;
            pointEndDown.y = height;
            drawDownLine = true;
        }
	}
	else if (m_data.m_close < m_data.m_open)
	{
		color = PCOLORS_DOWNCOLOR;
        top += (int)((m_data.m_high - m_data.m_open) * perLength);
        bottom = 5 + (int)((m_data.m_high - m_data.m_close) * perLength);
        if (m_data.m_high != m_data.m_open)
        {
            pointStartUp.x = midX;
            pointStartUp.y = top;
            pointEndUp.x = midX;
            pointEndUp.y = 5;
            drawUpLine = true;
        }
        if (m_data.m_low != m_data.m_close)
        {
            pointStartDown.x = midX;
            pointStartDown.y = bottom;
            pointEndDown.x = midX;
            pointEndDown.y = height;
            drawDownLine = true;
        }
	}
	_int64 lineColor = PCOLORS_LINECOLOR;
	RECT cRect = {left, top, right, bottom};
	paint->FillRect(color, cRect);
	if (!m_moving)
	{
		paint->DrawRect(lineColor, 1, 0, cRect);
	}
	if (drawUpLine)
	{
		paint->DrawLine(color, 1, 0, pointStartUp, pointEndUp);
		if (!m_moving)
		{
			paint->DrawLine(lineColor, 1, 0, pointStartUp.x + 1, pointStartUp.y, pointEndUp.x + 1, pointEndUp.y);
		}
	}
	if (drawDownLine)
	{
		paint->DrawLine(color, 1, 0, pointStartDown, pointEndDown);
		if (!m_moving)
		{
			paint->DrawLine(lineColor, 1, 0, pointStartDown.x + 1, pointStartDown.y, pointEndDown.x + 1, pointEndDown.y);
		}
	}
}

bool SandBoxCandleDiv::OnTimer()
{
    if (!m_moving)
    {
        return false;
    }
    GetMovingLocation();
    POINT location = GetLocation();
    float xMove = (float)location.x;
    float yMove = (float)location.y;
    if (!m_moveStart && ((m_direction == 0 && yMove > m_tempStops.y) 
        || (m_direction == 1 && yMove < m_tempStops.y)))
    {
        if (m_direction == 0)
        {
            yMove -= m_perFixMoveStep;
        }
        else
        {
            yMove += m_perDyncMoveStep;
        }
		POINT newLocation = {(int)xMove, (int)yMove};
        SetLocation(newLocation);
    }
    else
    {
        m_moveStart = true;
		SIZE changeSize = {0};
        GetDestSize(&changeSize);
        SetSize(changeSize);
        if (m_ticks < m_perFixMoveTimes)
        {
            m_ticks++;
            if (m_ticks == m_perFixMoveTimes)
            {
                SetLocation(m_destPoint);
            }
            else
            {
                xMove += m_perDyncMoveStep;
                yMove = m_slope * xMove + m_yIntercept;
				POINT newLocation = {(int)xMove, (int)yMove};
                SetLocation(newLocation);
            }
        }
        else
        {
            return true;
        }
    }
    return false;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SandBoxWindow::CallTimer(void *sender, int timerID, void *pInvoke)
{
	SandBoxWindow *sandBoxWindow = (SandBoxWindow*)pInvoke;
	sandBoxWindow->OnTimer(timerID);
}

void SandBoxWindow::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		SandBoxWindow *sandBoxWindow = (SandBoxWindow*)pInvoke;
		if (name == L"btnAdd")
        {
            sandBoxWindow->Add();
        }
		if (name == L"btnClose")
		{
			sandBoxWindow->Close();
		}
        else if (name == L"btnDelete")
        {
            sandBoxWindow->Delete();
        }
        else if (name == L"btnNext")
        {
            sandBoxWindow->CreateNextData(true);
        }
	}
}

void SandBoxWindow::ConvertStrToDate(const String& str, int *year, int *month, int *day, int *hour, int *minute, int *second, int *ms)
{
	if(str.length() > 0)
	{
		*year = CStr::ConvertStrToInt(str.substr(0, 4).c_str());
		*month = CStr::ConvertStrToInt(str.substr(5, 2).c_str());
		*day = CStr::ConvertStrToInt(str.substr(8, 2).c_str());
		if(str.find(L":") != -1)
		{
			*hour = CStr::ConvertStrToInt(str.substr(11, 2).c_str());
			*minute = CStr::ConvertStrToInt(str.substr(13, 2).c_str());
		}
	}
}

void SandBoxWindow::CreateCandlePos(SIZE size, float candleSeparater, POINT *newPoint)
{
    SIZE wSize = m_window->GetSize();
    int waitDivsSize = 0;
	int divsSize = (int)m_divs.size();
	vector<SandBoxCandleDiv*>::iterator sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
		if (!div->IsMoving())
        {
            waitDivsSize++;
        }
	}
    if (waitDivsSize == 0)
    {
		newPoint->x = m_defaultCandleLocation.x;
		newPoint->y = m_defaultCandleLocation.y;
        return;
    }
    float xPosition = 0;
    float rightTotalWidth = (float)((m_defaultWindowSize.cx - 20) / 0.618);
    float divideWidth = rightTotalWidth / (waitDivsSize + 1);
    float start = 0, end = 0;
    sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
        if (!div->IsMoving())
        {
            end = start + divideWidth;
            xPosition = (start + end) / 2 + m_defaultWindowSize.cx;
			newPoint->x = (int)xPosition;
			newPoint->y = (int)m_defaultCandleLocation.y;
            div->SetLocation(*newPoint);
            start = end;
        }
    }
    end = start + divideWidth;
    xPosition = (start + end) / 2 + m_defaultWindowSize.cx;
    newPoint->x = (int)xPosition;
	newPoint->y = (int)m_defaultCandleLocation.y;
}

void SandBoxWindow::CreatePreView()
{
	POINT newPoint = {0};
    SIZE newSize = m_defaultCandleSize;
    float separator = 0;
    CreateCandleSize(&newSize, &separator);
    CreateCandlePos(newSize, separator, &newPoint);
    SandBoxCandleDiv *div = new SandBoxCandleDiv;
    div->SetChart(m_chart);
    div->SetSize(newSize);
    div->SetLocation(newPoint);
    SetCandleStaticValue(div);
    m_window->AddControl(div);
    m_divs.push_back(div);
    m_native->Invalidate();
}

void SandBoxWindow::CreateCandleSize(SIZE *newSize, float *separator)
{
    int waitDivsSize = 0;
	int divsSize = (int)m_divs.size();
	vector<SandBoxCandleDiv*>::iterator sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
		if (!div->IsMoving())
        {
            waitDivsSize++;
        }
	}
    if (waitDivsSize == 0)
    {
        newSize->cx = m_defaultCandleSize.cx;
        newSize->cy = m_defaultCandleSize.cy;
        *separator = 20;
        return;
    }
    int cw = m_defaultCandleSize.cx - waitDivsSize;
    int ch = m_defaultCandleSize.cy - waitDivsSize * (m_defaultCandleSize.cy / m_defaultCandleSize.cx);
    *separator = (float)(20 - divsSize * 0.67);
    if (cw < 3)
    {
        cw = 3;
        ch = 6;
    }
    if (*separator < 0.5)
    {
        *separator = 0.5;
    }
    newSize->cx = cw;
    newSize->cy = ch;
	sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
		if(!div->IsMoving())
		{
			div->SetSize(*newSize);
		}
	}
}

void SandBoxWindow::CreateNextData(bool addOrDelete)
{
    ChartA *chart = m_chart->GetChart();
	CTable *dataSource = chart->GetDataSource();
    LabelA *lblDate = GetLabel(L"lblDate");
	String str = lblDate->GetText();
	if(str.length() > 0)
	{
		double date = 0;
		if(addOrDelete)
		{
			int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
			ConvertStrToDate(str, &year, &month, &day, &hour, &minute, &second, &ms);
			date = ::M129(year, month, day, hour, minute, second, ms);
		}
		else
		{
			int rowsCount = dataSource->RowsCount();
			date = dataSource->GetXValue(rowsCount - 1);
		}
		date += m_chart->GetCycle() * 60;
		SetSpinValue(date);
	}
}

void SandBoxWindow::Delete()
{
	ChartA *chart = m_chart->GetChart();
    CTable *dataSource = chart->GetDataSource();
    int rowsCount = dataSource->RowsCount();
    if (rowsCount > 0)
    {
        dataSource->RemoveAt(rowsCount - 1);
    }
    vector<CIndicator*> indicators = m_chart->GetIndicators();
	vector<CIndicator*>::iterator sIter = indicators.end();
	for(; sIter != indicators.end(); ++sIter)
	{
		(*sIter)->OnCalculate(0);
	}
    CreateNextData(false);
    m_chart->GetChart()->Update();
    m_native->Invalidate();
}

void SandBoxWindow::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	ControlEvent spinInputChangedEvent = &SpinTextChanged;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ButtonA *button = dynamic_cast<ButtonA*>(*sIter);
		if(button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		SpinA *spin = dynamic_cast<SpinA*>(*sIter);
		if(spin)
		{
			spin->RegisterEvent(spinInputChangedEvent, EVENTID::TEXTCHANGED, this);
		}
		RegisterEvents(*sIter);
	}
}

void SandBoxWindow::SetCandleStaticValue(SandBoxCandleDiv *div)
{
	SecurityData data = div->GetData();
    SpinA *spinOpen = GetSpin(L"txtOpen");
    SpinA *spinClose = GetSpin(L"txtClose");
    SpinA *spinHigh = GetSpin(L"txtHigh");
    SpinA *spinLow = GetSpin(L"txtLow");
    SpinA *spinVolume = GetSpin(L"txtVolume");
    SpinA *spinAmount = GetSpin(L"txtAmount");
    LabelA *lblDate = GetLabel(L"lblDate");
	data.m_open = (float)spinOpen->GetValue();
    data.m_close = (float)spinClose->GetValue();
    data.m_high = (float)spinHigh->GetValue();
    data.m_low = (float)spinLow->GetValue();
    data.m_volume = spinVolume->GetValue();
    data.m_amount = spinAmount->GetValue();
	int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
	String str = lblDate->GetText();
	if(str.length() > 0)
	{
		ConvertStrToDate(lblDate->GetText(), &year, &month, &day, &hour, &minute, &second, &ms);
		data.m_date = ::M129(year, month, day, hour, minute, second, ms);
	}
	div->SetData(data);
}

void SandBoxWindow::SetSpinValue(double date)
{
    ChartA *chart = m_chart->GetChart();
    CTable *dataSource = chart->GetDataSource();
    int rowsCount = dataSource->RowsCount();
    if (rowsCount > 0)
    {
        int index = dataSource->GetRowIndex((double)date);
        if (index < 1)
        {
            index = rowsCount - 1;
        }
        SpinA *spinOpen = GetSpin(L"txtOpen");
        SpinA *spinClose = GetSpin(L"txtClose");
        SpinA *spinHigh = GetSpin(L"txtHigh");
        SpinA *spinLow = GetSpin(L"txtLow");
        SpinA *spinVolume = GetSpin(L"txtVolume");
        SpinA *spinAmount = GetSpin(L"txtAmount");
        LabelA *lblDate = GetLabel(L"lblDate");
        spinOpen->SetValue(dataSource->Get2(index, OPEN_INDEX));
        spinClose->SetValue(dataSource->Get2(index, CLOSE_INDEX));
        spinHigh->SetValue(dataSource->Get2(index, HIGH_INDEX));
        spinLow->SetValue(dataSource->Get2(index, LOW_INDEX));
        spinVolume->SetValue(dataSource->Get2(index, VOL_INDEX));
        spinAmount->SetValue(dataSource->Get2(index, AMOUNT_INDEX));
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;	    
		::M130((double)date, &year, &month, &day, &hour, &minute, &second, &msecond);
        int m_cycle = m_chart->GetCycle();
		wchar_t szDate[100] = {0};
        if (m_cycle <= 60)
        {
			_stprintf_s(szDate, 99, L"%d-%02d-%02d %02d:%02d:%02d", year, month, day, hour, minute, second);
        }
        else
        {
			_stprintf_s(szDate, 99, L"%d-%02d-%02d", year, month, day);
        }
		lblDate->SetText(szDate);
        m_window->Invalidate();
    }
}

void SandBoxWindow::SpinTextChanged(void *sender, void *pInvoke)
{
	SandBoxWindow *sandBoxCandleDiv = (SandBoxWindow*)pInvoke;
	SpinA *spin = (SpinA*)sender;
	sandBoxCandleDiv->OnSpinTextChanged(spin);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SandBoxWindow::SandBoxWindow(NativeBase *native)
{
	m_chart = 0;
	m_defaultCandleSize.cx = 80;
	m_defaultCandleSize.cy = 160;
	m_defaultCandleLocation.x = 320;
	m_defaultCandleLocation.y = 60;
	m_defaultWindowSize.cx = 260;
	m_defaultWindowSize.cy = 300;
	m_native = native;
	m_useAnimation = true;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\SandBoxWindow.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowSB"));
    m_timerEvent = &CallTimer;
	m_window->RegisterEvent(m_timerEvent, EVENTID::TIMER, this);
	RegisterEvents(m_window);
}

SandBoxWindow::~SandBoxWindow()
{
	m_chart = 0;
	NativeBase *native = GetNative();
	vector<SandBoxCandleDiv*>::iterator sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
		if (div->IsMoving())
        {
            m_native->RemoveControl(div);
        }
        else
        {
            m_window->RemoveControl(div);
        }
	}
	m_divs.clear();
	if (m_window)
    {
		m_window->UnRegisterEvent(m_timerEvent, EVENTID::TIMER);
        m_timerEvent = 0;
        m_window->StopTimer(m_timerID);
		if(m_window->GetParent())
		{
			m_window->Close();
		}
		delete m_window;
		m_window = 0;
    }
}

OwChart* SandBoxWindow::GetChart()
{
	return m_chart;
}

void SandBoxWindow::SetChart(OwChart *chart)
{
	m_chart = chart;
    ChartA *chartA = m_chart->GetChart();
    CTable *dataSource = chartA->GetDataSource();
    int dataSize = dataSource->RowsCount();
    if (dataSize == 0)
    {
        return;
    }
    double date = dataSource->GetXValue(dataSize - 1);
    SetSpinValue(date);
}

bool SandBoxWindow::UseAnimation()
{
	return m_useAnimation;
}

void SandBoxWindow::SetUseAnimation(bool useAnimation)
{
	m_useAnimation = useAnimation;
}

WindowA* SandBoxWindow::GetWindow()
{
	return m_window;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SandBoxWindow::Add()
{
	NativeBase *native = GetNative();
	vector<SandBoxCandleDiv*>::iterator sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
		if (!div->IsMoving())
        {
            int clientX = native->ClientX(div);
            int clientY = native->ClientY(div);
            int direction = 0;
            if (clientY < 100)
            {
                direction = 1;
                clientY += 150;
            }
            else
            {
                clientY -= 150;
            }
            m_window->RemoveControl(div);
            div->SetParent(0);
            m_native->AddControl(div);
			POINT newLocation = {clientX, clientY};
            div->SetDirection(direction);
            div->SetLocation(newLocation);
            div->SetMoving(true);
        }
	}
    CreateNextData(true);
    CreatePreView();
    m_native->Invalidate();
}

void SandBoxWindow::Close()
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

void SandBoxWindow::OnSpinTextChanged(SpinA *spin)
{
	vector<SandBoxCandleDiv*>::iterator sIter = m_divs.begin();
	for(; sIter != m_divs.end(); ++sIter)
	{
        SandBoxCandleDiv *div = *sIter;
		if (!div->IsMoving())
        {
            SecurityData data = div->GetData();
            String name = spin->GetName();
            double value = spin->GetValue();
            if (name == L"txtOpen")
            {
                data.m_open = (float)value;
            }
            else if (name == L"txtClose")
            {
                data.m_close = (float)value;
            }
            else if (name == L"txtHigh")
            {
                data.m_high = (float)value;
            }
            else if (name == L"txtLow")
            {
                data.m_low = (float)value;
            }
            else if (name == L"txtVolume")
            {
                data.m_volume = value;
            }
            else if (name == L"txtAmount")
            {
                data.m_amount = value;
            }
            div->SetData(data);
            m_native->Invalidate();
            break;
        }
	}
}

void SandBoxWindow::OnTimer(int timerID)
{
	if (m_timerID == timerID)
    {
		NativeBase *native = GetNative();
		ChartA *chart = m_chart->GetChart();
        CTable *dataSource = chart->GetDataSource();
        int divsSize = (int)m_divs.size();
        bool painting = false;
        int cx = native->ClientX(m_window);
        int cy = native->ClientY(m_window);
        vector<SandBoxCandleDiv*> removeDivs;
		vector<SandBoxCandleDiv*>::iterator sIter = m_divs.begin();
		for(; sIter != m_divs.end(); ++sIter)
		{
            SandBoxCandleDiv *div = *sIter;
            if (div->IsMoving())
            {
                painting = true;
            }
            if (div->OnTimer())
            {
                SecurityData data = div->GetData();
                int fields[7];
                fields[0] = CLOSE_INDEX;
                fields[1] = HIGH_INDEX;
                fields[2] = LOW_INDEX;
                fields[3] = OPEN_INDEX;
                fields[4] = VOL_INDEX;
                fields[5] = AMOUNT_INDEX;
				fields[6] = AVGPRICE_INDEX;
				SecurityDataHelper::InsertLatestData(m_chart->GetChart(), dataSource, &m_chart->GetIndicators(), fields, &data);
				removeDivs.push_back(div);
            }
        }
		int removeDivsSize = (int)removeDivs.size();
		if(removeDivsSize > 0)
		{
			vector<SandBoxCandleDiv*>::iterator sIter2 = removeDivs.begin();
			for(; sIter2 != removeDivs.end(); ++sIter2)
			{
				SandBoxCandleDiv *div = *sIter2;
				m_native->RemoveControl(div);
				sIter = m_divs.begin();
				for(; sIter != m_divs.end(); ++sIter)
				{
					if(*sIter == div)
					{
						m_divs.erase(sIter);
						break;
					}
				}
                delete div;
			}
			removeDivs.clear();
            m_chart->GetChart()->Update();
		}
        if (painting)
        {
            m_native->Invalidate();
        }
    }
}

void SandBoxWindow::Show()
{
	POINT location = {-m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
    m_window->AnimateShow(false);
    m_window->Invalidate();
    m_window->StartTimer(m_timerID, 20);
    CreateNextData(true);
    CreatePreView();
}
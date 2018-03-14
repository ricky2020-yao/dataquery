#include "..\\..\\stdafx.h"
#include "OwChart.h"

void OwChart::ChartInvoke(void *sender, void *args, void *pInvoke)
{
	OwChart *chart = (OwChart*)pInvoke;
	chart->OnChartInvoke(args);
}

void OwChart::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	OwChart *chart = (OwChart*)pInvoke;
	ButtonA *btn = (ButtonA*)sender;
	if(btn->GetParent()->GetName() == L"tabMainFrame")
	{
		LabelA *lblTitle = chart->GetLabel(L"lblTitle");
		lblTitle->SetText(btn->GetText());
		lblTitle->GetNative()->Invalidate();
	}
}

void OwChart::CloseToolWindow(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    ButtonA *btn = (ButtonA*)sender;
	btn->GetNative()->RemoveControl(btn);
	WindowA *window = dynamic_cast<WindowA*>(btn->GetParent());
    window->Close();
	delete window;
}

void OwChart::IndicatorDataCallBack(CMessage *message, void *pInvoke)
{
	OwChart *chart = (OwChart*)pInvoke;
	if(chart)
	{
		chart->OnQuoteDataCallBack(message);
	}
}

void OwChart::QuoteDataCallBack(CMessage *message, void *pInvoke)
{
	OwChart *chart = (OwChart*)pInvoke;
	if(chart)
	{
		chart->OnQuoteDataCallBack(message);
	}
}

void OwChart::MenuItemClick(void *sender, MenuItemA *item, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if(button == MouseButtonsA_Left && clicks == 1)
	{
		const String& name = item->GetName();
		if (name.length() > 0)
		{
			OwChart *chart = (OwChart*)pInvoke;
			chart->SelectMenu(item, name);
		}
	}
}

void OwChart::RegisterEvents(ControlA *control)
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

void OwChart::RightMenuVisibleChanged(void *sender, void *pInvoke)
{
	OwChart *chart = (OwChart*)pInvoke;
	chart->OnRightMenuVisibleChanged();
}

void OwChart::UserSessionDataCallBack(CMessage *message, void *pInvoke)
{
	OwChart *chart = (OwChart*)pInvoke;
	chart->OnUserSessionDataCallBack(message);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

OwChart::OwChart()
{
	m_addingPlotType = L"";
	m_bar = 0;
	m_barForecast = 0;
	m_candle = 0;
	m_candleDiv = 0;
	m_candleHScalePixel = 0;
	m_chart = 0;
	m_currentDiv = 0;
	m_cycle = 0;
	m_digit = 2;
	m_float = 0;
	m_forecastTick = 0;
	m_index = -1;
	m_indexDiv = 0;
	m_indDiv = 0;
	m_indicatorLayoutService = 0;
	m_indicatorService = 0;
	m_indRightMenu = 0;
	m_latestDiv = 0;
	m_layoutID = L"";
	m_minuteAvgLine = 0;
	m_minuteLine = 0;
	m_plotRightMenu = 0;
	m_quoteService = 0;
	m_requestID = BaseService::GetRequestID();
	m_reverseVScale = false;
	m_rightMenu = 0;
	m_searchDiv = 0;
	m_securityService = 0;
	m_scaleFactor = 1;
	m_showMinuteLine = true;
	m_subscription = 1;
	m_userSecurityService = 0;
	m_userSessionService = 0;
	m_vfRequestID = BaseService::GetRequestID();
	m_volumeDiv = 0;
}

OwChart::~OwChart()
{
	m_quoteService->StopPushHistoryDatas(m_requestID);
	m_quoteService->UnRegisterListener(m_requestID);
	vector<CIndicator*>::iterator sIter = m_indicators.begin();
	for(;sIter != m_indicators.end();++sIter)
	{
		delete *sIter;
	}
	m_host = 0;
	m_bar = 0;
	m_barForecast = 0;
	m_candle = 0;
	m_candleDiv = 0;
	m_chart = 0;
	m_currentDiv = 0;
	m_divs.clear();
	m_float = 0;
	m_hScaleSteps.clear();
	m_indexDiv = 0;
	m_indicatorLayoutService = 0;
	m_indicators.clear();
	m_indicatorService = 0;
	m_indDiv = 0;
	m_indRightMenu = 0;
	m_latestDiv = 0;
	m_minuteAvgLine = 0;
	m_minuteLine = 0;
	m_plotRightMenu = 0;
	m_rightMenu = 0;
	m_searchDiv = 0;
	m_securityService = 0;
	m_userSecurityService = 0;
	m_userSessionDataCallBack = 0;
	m_userSessionService = 0;
	m_volumeDiv = 0;
	if(m_native)
	{
		delete m_native;
		m_native = 0;
	}
}

BarrageDiv* OwChart::GetBarrageDiv()
{
	return m_barrageDiv;
}

ChartA* OwChart::GetChart()
{
	return m_chart;
}

int OwChart::GetCycle()
{
	if(m_showMinuteLine)
	{
		return 0;
	}
	else
	{
		return m_cycle;
	}
}

void OwChart::SetCycle(int cycle)
{
	m_cycle = cycle;
}

int OwChart::GetDigit()
{
	return m_digit;
}

void OwChart::SetDigit(int digit)
{
	m_digit = digit;
}

int OwChart::GetIndex()
{
	return m_index;
}

vector<CIndicator*> OwChart::GetIndicators()
{
	return m_indicators;
}

SecurityLatestData OwChart::GetLatestData()
{
	return m_latestDiv->GetLatestData();
}

bool OwChart::ShowMinuteLine()
{
	return m_showMinuteLine;
}

void OwChart::SetShowMinuteLine(bool showMinuteLine)
{
	m_showMinuteLine = showMinuteLine;
}

double OwChart::GetScaleFactor()
{
	return m_scaleFactor;
}

void OwChart::SetScaleFactor(double scaleFactor)
{
	m_scaleFactor = scaleFactor;
}

int OwChart::GetSubscription()
{
	return m_subscription;
}

void OwChart::SetSubscription(int subscription)
{
	m_subscription = subscription;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void OwChart::AddBlankDiv()
{
	vector<CDiv*>::iterator sIter = m_divs.begin();
	for(;sIter != m_divs.end();++sIter)
	{
		CDiv *div = *sIter;
		div->GetHScale()->SetVisible(false);
		div->GetHScale()->SetHeight(0);
	}
	CDiv *div = m_chart->AddDiv();
	div->SetBackColor(PCOLORS_BACKCOLOR4);
	m_divs.push_back(div);
	div->GetHScale()->SetVisible(true);
	div->GetHScale()->SetHeight(22);
	div->GetVGrid()->SetDistance(40);
	FONT vFont(L"Arial", 14, false, false, false);
	div->GetLeftVScale()->SetForeColor(PCOLORS_FORECOLOR);
	div->GetLeftVScale()->SetFont(&vFont);
	div->GetRightVScale()->SetForeColor(PCOLORS_FORECOLOR);
	div->GetRightVScale()->SetFont(&vFont);
	RefreshData();
}

CIndicator* OwChart::AddMainIndicator(const String& name, const String& title, const String& script, const String& parameters, CDiv *div, bool update)
{
	CIndicator *indicator = SecurityDataHelper::CreateIndicator(m_chart, m_chart->GetDataSource(), script, parameters);
    indicator->SetName(name);
    indicator->SetFullName(title);
    indicator->SetAttachVScale(AttachVScale_Left);
    indicator->SetDiv(div);
    indicator->OnCalculate(0);
	m_indicators.push_back(indicator);
	if(div != m_candleDiv && div != m_volumeDiv)
	{
		div->GetTitleBar()->SetText(indicator->GetTitle());
	}
	if(update)
	{
		m_chart->Update();
		m_native->Invalidate();
	}
	return indicator;
}

void OwChart::AddUserSecurity(const String& categoryID, const String& code)
{
	vector<UserSecurityCategory> categories;
	m_userSecurityService->GetCategories(&categories);
	int categoriesSize = (int)categories.size();
	if(categoriesSize > 0)
	{
		UserSecurityCategory fCategory = categories[0];
		UserSecurityCategory category;
		if(categoryID.length() == 0)
		{
			category.m_categoryID = fCategory.m_categoryID;
		}
		else
		{
			category.m_categoryID = categoryID;
		}
		category.m_codes = code;
		category.m_userID = DataCenter::GetUserID();
		m_userSecurityService->AddSecurities(m_userSecurityService->GetOperatorRequestID(), &category);
	}
}

CIndicator* OwChart::AddViceIndicator(const String& name, const String& script, const String& parameters, CDiv *div, bool update)
{
	CIndicator *indicator = SecurityDataHelper::CreateIndicator(m_chart, m_chart->GetDataSource(), script, parameters);
    indicator->SetAttachVScale(AttachVScale_Right);
    m_indicators.push_back(indicator);
    indicator->SetDiv(div);
	indicator->SetName(name);
    indicator->OnCalculate(0);
	if(update)
	{
		m_chart->Update();
		m_native->Invalidate();
	}
	return indicator;
}

void OwChart::BindHistoryData(HistoryDataInfo *dataInfo, vector<SecurityData> *historyDatas)
{
	if(dataInfo->m_securityCode ==  m_latestDiv->GetSecurityCode()
		&& dataInfo->m_cycle == m_cycle && dataInfo->m_subscription == m_subscription)
	{
		CTable *dataSource = m_chart->GetDataSource();
		int fields[7];
		fields[0] = CLOSE_INDEX;
		fields[1] = HIGH_INDEX;
		fields[2] = LOW_INDEX;
		fields[3] = OPEN_INDEX;
		fields[4] = VOL_INDEX;
		fields[5] = AMOUNT_INDEX;
		fields[6] = AVGPRICE_INDEX;
		int index = -1;
		if (dataInfo->m_pushData)
		{
			SecurityData latestData = (*historyDatas)[historyDatas->size() - 1];
			index = SecurityDataHelper::InsertLatestData(m_chart, dataSource, &m_indicators, fields, &latestData);
			if (dataInfo->m_cycle == 1440)
            {
                if (m_forecastTick % 10 == 0)
                {
					m_quoteService->GetVolumeForecast(m_vfRequestID, dataInfo->m_securityCode);
                }
                m_forecastTick++;
            }
		}
		else
		{
			SecurityDataHelper::BindHistoryDatas(m_chart, dataSource, &m_indicators, fields, historyDatas);
			if (dataInfo->m_cycle == 1440)
            {
				m_quoteService->GetVolumeForecast(m_vfRequestID, dataInfo->m_securityCode);
			}
			index = 0;
		}
		if (index >= 0)
		{
			m_hScaleSteps.clear();
	        if (m_showMinuteLine)
            {
				int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
				::M130(dataSource->GetXValue(0), &year, &month, &day, &hour, &minute, &second, &ms);
				m_hScaleSteps.push_back(::M129(year, month, day, 9, 0, 0, 0));
				m_hScaleSteps.push_back(::M129(year, month, day, 9, 30, 0, 0));
				m_hScaleSteps.push_back(::M129(year, month, day, 10, 0, 0, 0));
                m_hScaleSteps.push_back(::M129(year, month, day, 10, 30, 0, 0));
                m_hScaleSteps.push_back(::M129(year, month, day, 11, 0, 0, 0));
                m_hScaleSteps.push_back(::M129(year, month, day, 13, 0, 0, 0));
                m_hScaleSteps.push_back(::M129(year, month, day, 13, 30, 0, 0));
                m_hScaleSteps.push_back(::M129(year, month, day, 14, 0, 0, 0));
                m_hScaleSteps.push_back(::M129(year, month, day, 14, 30, 0, 0));
				m_hScaleSteps.push_back(::M129(year, month, day, 15, 0, 0, 0));
            }
			else
			{
				int rowsSize = dataSource->RowsCount();
				for (int i = index; i < rowsSize; i++)
				{
			        double volume = dataSource->Get2(i, VOL_INDEX);
                    if (!dataSource->IsNaN(volume))
                    {
                        m_index = i;
                    }
					if(!m_showMinuteLine)
					{
						double close = dataSource->Get2(i, CLOSE_INDEX);
						double open = dataSource->Get2(i, OPEN_INDEX);
						if (close >= open)
						{
							dataSource->Set2(i, m_bar->GetStyleField(), 1);
							dataSource->Set2(i, m_bar->GetColorField(), PCOLORS_UPCOLOR);
						}
						else
						{
							dataSource->Set2(i, m_bar->GetStyleField(), 0);
							dataSource->Set2(i, m_bar->GetColorField(), PCOLORS_DOWNCOLOR2);
						}
					}
				}
			}
			RefreshData();
		}
		m_chart->Update();
		m_native->Invalidate();
	}
}

void OwChart::ChangeCycle(int cycle)
{
	if(cycle > 0)
	{
		int oldCycle = GetCycle();
		if(oldCycle > 0 && oldCycle != cycle)
		{
			m_candleHScalePixel = m_chart->GetHScalePixel();
		}
		SetCycle(cycle);
		m_showMinuteLine = false;
	}
	else
	{
		SetCycle(cycle);
		m_showMinuteLine = true;
	}
    String securityCode = m_latestDiv->GetSecurityCode();
    if (securityCode.length() > 0)
    {
        Security security;
		m_securityService->GetSecurityByCode(securityCode, &security);
        SearchSecurity(&security);
    }
}

void OwChart::DeleteIndicators(bool update)
{
	vector<CIndicator*>::iterator sIter = m_indicators.begin();
	for(;sIter != m_indicators.end();++sIter)
	{
		CIndicator *indicator = *sIter;
		indicator->Clear();
		delete indicator;
	}
    m_indicators.clear();
	if(update)
	{
		m_chart->Update();
		m_native->Invalidate();
	}
}

void OwChart::DeleteSelectedIndicator()
{
    CIndicator *indicator = GetSelectedIndicator();
    if (indicator)
    {
        indicator->Clear();
		vector<CIndicator*>::iterator sIter = m_indicators.begin();
		for(;sIter != m_indicators.end();++sIter)
		{
			if(indicator == *sIter)
			{
				m_indicators.erase(sIter);
				break;
			}
		}
		delete indicator;
        m_chart->Update();
        m_native->Invalidate();
    }
}

void OwChart::DeleteSelectedPlot()
{
	CPlot *selectedPlot = m_chart->GetSelectedPlot();
	if(selectedPlot)
	{
		selectedPlot->GetDiv()->RemovePlot(selectedPlot);
		delete selectedPlot;
		m_chart->Update();
        m_native->Invalidate();
	}
}

void OwChart::Exit()
{
	::Sleep(2000);
	DataCenter::DisConnect();
}

String OwChart::GetIndicatorLayout()
{
	String str = L"";
    vector<CDiv*> divs = m_chart->GetDivs();
	int divsSize = (int)divs.size();
	int indicatorsSize = (int)m_indicators.size();
	wchar_t szPercent[20] = {0};
	int totalPercent = 0;
    for (int i = 0; i < divsSize; i++)
    {
        CDiv *div = divs[i];
        str += L"DIV\r\n";
		int percent = (int)(div->GetHeight() * 100 / m_chart->GetHeight());
        if (i == divsSize - 1)
        {
            percent = 100 - totalPercent;
        }
        totalPercent += percent;
		_stprintf_s(szPercent, 19, L"%d", percent); 
		String strPernet = szPercent;
		str += L"HEIGHT=" + strPernet + L"\r\n";
        for (int j = 0; j < indicatorsSize; j++)
        {
            CIndicator *indicator = m_indicators[j];
            if (indicator->GetDiv() == div)
            {
                if (indicator->GetAttachVScale() == AttachVScale_Left)
                {
                    str += L"IND=" + indicator->GetName() + L"\r\n";
                }
                else
                {
                    str += L"VICEIND=" + indicator->GetName() + L"\r\n";
                }
            }
        }
    }
    return str;
}

CIndicator* OwChart::GetSelectedIndicator()
{
	BaseShape *shape = m_chart->GetSelectedShape();
    if (shape)
    {
		vector<CIndicator*>::iterator sIter = m_indicators.begin();
		for(;sIter != m_indicators.end();++sIter)
		{
			CIndicator *indicator = *sIter;
			vector<BaseShape*> shapes = indicator->GetShapes();
			vector<BaseShape*>::iterator sIter2 = shapes.begin();
			for(;sIter2 != shapes.end();++sIter2)
			{
				if(shape == *sIter2)
				{
					return indicator;
				}
			}
		}
    }
    return 0;
}

void OwChart::InitInterface()
{
	m_chart = dynamic_cast<ChartA*>(FindControl(L"divKLine"));
	CTable *dataSource = m_chart->GetDataSource();
	ControlInvokeEvent invokeEvent = &ChartInvoke;
	m_chart->RegisterEvent(invokeEvent, EVENTID::INVOKE, this);
	m_chart->SetBackColor(PCOLORS_BACKCOLOR4);
	m_chart->SetBorderColor(PCOLORS_LINECOLOR4);
	m_chart->SetCanMoveShape(true);
	m_chart->SetScrollAddSpeed(true);
	m_chart->SetLeftVScaleWidth(85);
	m_chart->SetRightVScaleWidth(85);
	m_chart->SetHScalePixel(3);
	m_chart->SetHScaleFieldText(L"日期");
	m_candleDiv = m_chart->AddDiv(60);
	m_candleDiv->GetTitleBar()->SetText(L"分时线");
	m_candleDiv->GetVGrid()->SetVisible(true);
	m_candleDiv->GetLeftVScale()->SetNumberStyle(NumberStyle_Underline);
	m_candleDiv->GetLeftVScale()->SetMarginTop(2);
	m_candleDiv->GetLeftVScale()->SetMarginBottom(2);
	FONT vFont(L"Arial", 14, false, false, false);
	m_candleDiv->GetLeftVScale()->SetFont(&vFont);
	m_candleDiv->GetRightVScale()->SetNumberStyle(NumberStyle_Underline);
	m_candleDiv->GetRightVScale()->SetFont(&vFont);
	m_candleDiv->GetRightVScale()->SetMarginTop(2);
    m_candleDiv->GetRightVScale()->SetMarginBottom(2);
	CTitle *priceTitle = new CTitle(CLOSE_INDEX, L"", PCOLORS_FORECOLOR9, 2, true);
    priceTitle->SetFieldTextMode(TextMode_Value);
    m_candleDiv->GetTitleBar()->Titles.push_back(priceTitle);
	m_candle = new CandleShape;
	m_candleDiv->AddShape(m_candle);
	m_candleDiv->SetBackColor(PCOLORS_BACKCOLOR4);
	m_candle->SetCloseField(CLOSE_INDEX);
	m_candle->SetOpenField(OPEN_INDEX);
	m_candle->SetHighField(HIGH_INDEX);
	m_candle->SetLowField(LOW_INDEX);
	m_candle->SetCloseFieldText(L"收盘");
	m_candle->SetHighFieldText(L"最高");
	m_candle->SetLowFieldText(L"最低");
	m_candle->SetOpenFieldText(L"开盘");
	m_candle->SetVisible(false);
	m_minuteLine = new PolylineShape;
	m_candleDiv->AddShape(m_minuteLine);
	m_minuteLine->SetColor(PCOLORS_LINECOLOR);
	m_minuteLine->SetFieldName(CLOSE_INDEX);
	m_minuteAvgLine = new PolylineShape;
    m_candleDiv->AddShape(m_minuteAvgLine);
	m_minuteAvgLine->SetColor(PCOLORS_LINECOLOR2);
	m_minuteAvgLine->SetFieldName(AVGPRICE_INDEX);
	m_volumeDiv = m_chart->AddDiv(15);
	m_volumeDiv->SetBackColor(PCOLORS_BACKCOLOR4);
	m_volumeDiv->GetVGrid()->SetDistance(30);
	m_volumeDiv->GetLeftVScale()->SetDigit(0);
	m_volumeDiv->GetLeftVScale()->SetFont(&vFont);
	m_volumeDiv->GetRightVScale()->SetDigit(0);
	m_volumeDiv->GetRightVScale()->SetFont(&vFont);
	m_bar = new BarShape;
	int styleField = dataSource->AUTOFIELD();
	int colorField = dataSource->AUTOFIELD();
	m_bar->SetColorField(colorField);
	m_bar->SetStyleField(styleField);
	m_bar->SetUpColor(PCOLORS_LINECOLOR2);
	m_bar->SetFieldName(VOL_INDEX);
	m_bar->SetFieldText(VOL);
	m_volumeDiv->AddShape(m_bar);
	m_barForecast = new BarShape;
    m_barForecast->SetFieldText(L"成交量预测");
	m_barForecast->SetZOrder(-1);
    m_volumeDiv->AddShape(m_barForecast);
    m_barForecast->SetFieldName(dataSource->AUTOFIELD());
	m_volumeDiv->GetTitleBar()->SetText(L"成交量");
	CTitle *barTitle = new CTitle(VOL_INDEX, L"成交量", m_bar->GetDownColor(), 0, true);
	barTitle->SetFieldTextMode(TextMode_Value);
	m_volumeDiv->GetTitleBar()->Titles.push_back(barTitle);
	m_indDiv = m_chart->AddDiv(25);
	m_indDiv->SetBackColor(PCOLORS_BACKCOLOR4);
	m_indDiv->GetVGrid()->SetDistance(40);
	m_indDiv->GetLeftVScale()->SetMarginTop(2);
	m_indDiv->GetLeftVScale()->SetMarginBottom(2);
	m_indDiv->GetLeftVScale()->SetFont(&vFont);
	m_indDiv->GetRightVScale()->SetMarginTop(2);
	m_indDiv->GetRightVScale()->SetMarginBottom(2);
	m_indDiv->GetRightVScale()->SetFont(&vFont);
	m_candleDiv->GetHScale()->SetVisible(false);
	m_candleDiv->GetHScale()->SetHeight(0);
	m_volumeDiv->GetHScale()->SetVisible(false);
	m_volumeDiv->GetHScale()->SetHeight(0);
	m_indDiv->GetHScale()->SetVisible(true);
	m_indDiv->GetHScale()->SetHeight(22);
	m_volumeDiv->GetLeftVScale()->SetForeColor(PCOLORS_FORECOLOR11);
	m_volumeDiv->GetRightVScale()->SetForeColor(PCOLORS_FORECOLOR11);
	m_indDiv->GetLeftVScale()->SetForeColor(PCOLORS_FORECOLOR);
	m_indDiv->GetRightVScale()->SetForeColor(PCOLORS_FORECOLOR);
	m_divs.push_back(m_candleDiv);
	m_divs.push_back(m_volumeDiv);
	m_divs.push_back(m_indDiv);
	m_float = dynamic_cast<FloatDiv*>(FindControl(L"divFloat"));
	m_float->SetChart(this);
	m_latestDiv = dynamic_cast<LatestDiv*>(FindControl(L"divLatest"));
	m_latestDiv->SetChart(this);
	m_indexDiv = dynamic_cast<IndexDiv*>(FindControl(L"divIndex"));
	m_indexDiv->SetChart(this);
	dataSource->AddColumn(CLOSE_INDEX);
	dataSource->AddColumn(OPEN_INDEX);
	dataSource->AddColumn(HIGH_INDEX);
	dataSource->AddColumn(LOW_INDEX);
	dataSource->AddColumn(VOL_INDEX);
	dataSource->AddColumn(AMOUNT_INDEX);
	dataSource->AddColumn(AVGPRICE_INDEX);
	dataSource->AddColumn(colorField);
	dataSource->AddColumn(styleField);
	dataSource->AddColumn(m_barForecast->GetFieldName());	
	dataSource->SetColsCapacity(16);
	dataSource->SetColsGrowStep(4);
	m_indicatorLayoutService = DataCenter::GetIndicatorLayoutService();
	m_indicatorService = DataCenter::GetIndicatorService();
    m_quoteService = DataCenter::GetQuoteService();
    m_securityService = DataCenter::GetSecurityService();
    m_userSecurityService = DataCenter::GetUserSecurityService();
    m_userSessionService = DataCenter::GetUserSessionService();
	ListenerMessageCallBack indicatorDataCallBack = &IndicatorDataCallBack;
	m_indicatorService->RegisterListener(m_indicatorService->GetListRequestID(), indicatorDataCallBack, this);
	m_userSessionDataCallBack = &UserSessionDataCallBack;
    m_userSessionService->RegisterListener(m_userSessionService->GetListRequestID(), m_userSessionDataCallBack, this);
    m_userSessionService->RegisterListener(m_userSessionService->GetOperatorRequestID(), m_userSessionDataCallBack, this);
}

bool OwChart::IsWindowShowing()
{
	vector<ControlA*> controls = m_native->GetControls();
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
        WindowFrameA *frame = dynamic_cast<WindowFrameA*>(*sIter);
        if (frame)
        {
			return true;
		}
	}
	return false;
}

void OwChart::Load(String xmlPath)
{
	LoadFile(xmlPath, 0);
	m_native = GetNative();
	m_host = dynamic_cast<WinformControlHost*>(m_native->GetHost());
	InitInterface();
	DataCenter::SetMainUI(this);
}

void OwChart::LoadData()
{
	if(!m_rightMenu)
	{
		m_barrageDiv = dynamic_cast<BarrageDiv*>(FindControl(L"divBarrage"));
		m_rightMenu = GetMenu(L"rightMenu");
		m_rightMenu->RegisterEvent(MenuItemClick, EVENTID::MENUITEMCLICK, this);
		m_rightMenu->RegisterEvent(RightMenuVisibleChanged, EVENTID::VISIBLECHANGED, this);
		m_rightMenu->Update();
		MenuItemA *plotItemRoot = GetMenuItem(L"ADDPLOT");
		map<String, String> plots = DataCenter::GetPlots();
		for (map<String, String>::iterator sIter = plots.begin(); sIter != plots.end(); sIter++)
		{
			MenuItemA *plotItem = new MenuItemA(sIter->second);
			plotItem->SetName(L"PLOT_ADDPLOT_" + sIter->first);
			plotItemRoot->AddItem(plotItem);
		}
		plots.clear();
		m_indRightMenu = GetMenu(L"indRightMenu");
		m_indRightMenu->RegisterEvent(MenuItemClick, EVENTID::MENUITEMCLICK, this);
		m_plotRightMenu = GetMenu(L"plotRightMenu");
		m_plotRightMenu->RegisterEvent(MenuItemClick, EVENTID::MENUITEMCLICK, this);
		m_chart->Update();
		m_quoteService->RegisterListener(m_vfRequestID, &QuoteDataCallBack, this);
		DataCenter::GetMacroService()->SetChart(this);
		RegisterEvents(GetDiv(L"divMain"));
	}
	m_indexDiv->Start();
	Security security;
	security.m_code = m_latestDiv->GetSecurityCode();
	security.m_name = m_latestDiv->GetSecurityName();
	if((int)security.m_code.length() > 0)
	{
		SearchSecurity(&security);
	}
	UserSecurityListT2 *userSecurityT2 = dynamic_cast<UserSecurityListT2*>(FindControl(L"userSecurityT2"));
	userSecurityT2->SetChart(this);
}

void OwChart::OnChartInvoke(void *args)
{
	if(args)
	{
		CMessage *message = (CMessage*)args;
		if(message)
		{
			if(message->m_serviceID == m_quoteService->GetServiceID())
			{
				if (message->m_functionID == FUNCTIONID_QUOTE_PUSHHISTORYDATA && message->m_requestID == m_requestID)
				{
					vector<SecurityData> datas;
					HistoryDataInfo dataInfo;
					QuoteService::GetHistoryDatas(&dataInfo, &datas, message->m_body, message->m_bodyLength);
					BindHistoryData(&dataInfo, &datas);
					datas.clear();
				}
				else if (message->m_functionID == FUNCTIONID_QUOTE_GETVOLUMEFORECAST && message->m_requestID == m_vfRequestID)
				{
					if (GetCycle() == 1440)
					{
						CTable *dataSource = m_chart->GetDataSource();
						int rowsCount = dataSource->RowsCount();
						if (rowsCount > 0)
						{
							double key = dataSource->GetXValue(rowsCount - 1);
							double volume = dataSource->Get2(rowsCount - 1, VOL_INDEX);
							double forecast = 0;
							vector<VolumeForecastData> datas;
							QuoteService::GetVolumeForecastData(&datas, message->m_body, message->m_bodyLength);
							int datasSize = (int)datas.size();
							for (int i = 0; i < datasSize; i++)
							{
								VolumeForecastData data = datas[i];
								if (data.m_securityCode == m_latestDiv->GetSecurityCode() && data.m_rate > 0)
								{
									forecast = volume / data.m_rate;
								}
							}
							if (forecast > 0)
							{
								dataSource->Set2(rowsCount - 1, m_barForecast->GetFieldName(), forecast);
							}
						}
					}
				}
			}
			if(message->m_serviceID == m_indicatorService->GetServiceID())
			{
				vector<Indicator> userIndicators;
				m_indicatorService->GetIndicators(&userIndicators);
				vector<Indicator>::iterator sIter = userIndicators.begin();
				for(;sIter != userIndicators.end();++sIter)
				{
					Indicator indicator = *sIter;
					if (indicator.m_name == L"MA")
					{
						AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, m_candleDiv, true);
					}
					else if (indicator.m_name == L"KDJ")
					{
						AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, m_indDiv, true);
					}
				}
				Security security;
				security.m_code = L"601857.SH";
				security.m_name = L"中国石油";
				SearchSecurity(&security);
			}
			delete message->m_body;
			delete message;
			message = 0;
		}
	}
}

void OwChart::OnQuoteDataCallBack(CMessage *message)
{
	CMessage *copyMessage = new CMessage;
	copyMessage->Copy(message);
	copyMessage->m_body = new char[copyMessage->m_bodyLength];
	char *str = new char[copyMessage->m_bodyLength];
	for(int i = 0; i < copyMessage->m_bodyLength; i++)
	{
		str[i] = message->m_body[i];
	}
	copyMessage->m_body = str;
	m_chart->BeginInvoke(copyMessage);
}

void OwChart::OnIndicatorDataCallBack(CMessage *message)
{
	CMessage *copyMessage = new CMessage;
	copyMessage->Copy(message);
	copyMessage->m_body = new char[copyMessage->m_bodyLength];
	char *str = new char[copyMessage->m_bodyLength];
	for(int i = 0; i < copyMessage->m_bodyLength; i++)
	{
		str[i] = message->m_body[i];
	}
	copyMessage->m_body = str;
	m_chart->BeginInvoke(copyMessage);
}

void OwChart::OnRightMenuVisibleChanged()
{
    if (m_rightMenu->IsVisible())
    {
        MenuItemA *mainIndRootItem = GetMenuItem(L"IND_ADDIND");
        MenuItemA *viceIndRootItem = GetMenuItem(L"IND_ADDVICEIND");
        vector<MenuItemA*> mainIndSubItems = mainIndRootItem->GetItems();
        vector<MenuItemA*> viceIndSubItems = viceIndRootItem->GetItems();
		int mainIndSubItemsSize = (int)mainIndSubItems.size();
        for (int i = 0; i < mainIndSubItemsSize; i++)
        {
            mainIndSubItems[i]->ClearItems();
        }
        int viceIndSubItemsSize = (int)viceIndSubItems.size();
        for (int i = 0; i < viceIndSubItemsSize; i++)
        {
            viceIndSubItems[i]->ClearItems();
        }
        vector<Indicator> indicators;
		m_indicatorService->GetIndicators(&indicators);
		vector<Indicator>::iterator sIter = indicators.begin();
		for(; sIter != indicators.end(); ++sIter)
		{
			Indicator indicator = *sIter;
            String title = indicator.m_description;
			if (indicator.m_name.length() > 0)
            {
                title += L"(" + indicator.m_name + L")";
            }
            for (int j = 0; j < mainIndSubItemsSize; j++)
            {
                MenuItemA *item = mainIndSubItems[j];
                if (item->GetText() == indicator.m_category)
                {
                    MenuItemA *mainIndItem = new MenuItemA(title);
                    mainIndItem->SetName(L"IND_ADDMAININD_" + indicator.m_name);
                    item->AddItem(mainIndItem);
					item->Update();
                }
            }
            for (int j = 0; j < viceIndSubItemsSize; j++)
            {
                MenuItemA *item = viceIndSubItems[j];
                if (item->GetText() == indicator.m_category)
                {
                    MenuItemA *viceIndItem = new MenuItemA(title);
                    viceIndItem->SetName(L"IND_ADDVICEIND_" + indicator.m_name);
                    item->AddItem(viceIndItem);
                }
            }
        }
		indicators.clear();
    }
}

void OwChart::OnUserSessionDataCallBack(CMessage *message)
{
	if(DataCenter::GetConnectState() == 1)
	{
		CMessage *copyMessage = new CMessage;
		copyMessage->Copy(message);
		copyMessage->m_body = new char[copyMessage->m_bodyLength];
		char *str = new char[copyMessage->m_bodyLength];
		for(int i = 0; i < copyMessage->m_bodyLength; i++)
		{
			str[i] = message->m_body[i];
		}
		copyMessage->m_body = str;
		m_chart->BeginInvoke(copyMessage);
	}
}

void OwChart::RefreshData()
{
	if (m_showMinuteLine)
    {
		m_candleDiv->GetLeftVScale()->SetForeColor2(PCOLORS_DOWNCOLOR);
		m_candleDiv->GetRightVScale()->SetForeColor2(PCOLORS_DOWNCOLOR);
        m_candleDiv->GetRightVScale()->SetType(VScaleType_Percent);
        m_candle->SetDownColor(PCOLORS_LINECOLOR);
        m_candle->SetStyle(CandleStyle_CloseLine);
        m_candle->SetTagColor(COLOR_EMPTY);
        m_candle->SetUpColor(PCOLORS_LINECOLOR);
		m_bar->SetStyle(BarStyle_Line);
		m_candle->SetVisible(false);
		m_minuteAvgLine->SetVisible(true);
		m_minuteLine->SetVisible(true);
        m_volumeDiv->GetLeftVScale()->SetMagnitude(1);
        m_volumeDiv->GetRightVScale()->SetMagnitude(1);
    }
    else
    {
		m_candleDiv->GetLeftVScale()->SetForeColor2(COLOR_EMPTY);
		m_candleDiv->GetRightVScale()->SetForeColor2(PCOLORS_DOWNCOLOR);
        m_candleDiv->GetRightVScale()->SetType(VScaleType_Percent);
		m_candle->SetDownColor(PCOLORS_DOWNCOLOR2);
        m_candle->SetStyle(CandleStyle_Rect);
        m_candle->SetTagColor(PCOLORS_FORECOLOR);
        m_candle->SetUpColor(PCOLORS_UPCOLOR);
		m_bar->SetStyle(BarStyle_Rect);
		m_candle->SetVisible(true);
		m_minuteAvgLine->SetVisible(false);
		m_minuteLine->SetVisible(false);
		m_volumeDiv->GetLeftVScale()->SetMagnitude(1000);
        m_volumeDiv->GetRightVScale()->SetMagnitude(1000);
    }
	vector<CIndicator*>::iterator sIter = m_indicators.begin();
	for(; sIter != m_indicators.end(); ++sIter)
	{
		CIndicator *indicator = *sIter;
        CDiv *div = indicator->GetDiv();
        if (div == m_candleDiv)
        {		
            vector<BaseShape*> shapes = indicator->GetShapes();
			vector<BaseShape*>::iterator sIter2 = shapes.begin();
			for(; sIter2 != shapes.end(); ++sIter2)
			{
				BaseShape *shape = *sIter2;
                shape->SetVisible(!m_showMinuteLine);
			}
			vector<CTitle*> titles = div->GetTitleBar()->Titles;
			vector<CTitle*>::iterator sIter3 = titles.begin();
			for(; sIter3 != titles.end(); ++sIter3)
			{
				CTitle *title = *sIter3;
				if(title->GetFieldName() == CLOSE_INDEX)
				{
					title->SetVisible(m_showMinuteLine);
				}
				else
				{
					title->SetVisible(!m_showMinuteLine);
				}
			}
        }
	}
	m_latestDiv->SetDigit(m_digit);
	SecurityLatestData latestData = m_latestDiv->GetLatestData();
	vector<CDiv*> divs = m_chart->GetDivs();
	vector<CDiv*>::iterator sIter2 = divs.begin();
	CTable *dataSource = m_chart->GetDataSource();
	for(; sIter2 != divs.end(); ++sIter2)
	{
		CDiv *div = *sIter2;
		if (div == m_candleDiv)
        {
			double lastClose = 0;
			if(latestData.m_securityCode.length() > 0)
			{
				lastClose = latestData.m_lastClose;
			}
			else
			{
				int rowsSize = dataSource->RowsCount();
				if(rowsSize > 0)
				{
					if (m_showMinuteLine)
					{
						lastClose = dataSource->Get2(0, CLOSE_INDEX);
					}
					else
					{
						if(rowsSize == 1)
						{
							lastClose = dataSource->Get2(0, OPEN_INDEX);
						}
						else
						{
							lastClose = dataSource->Get2(rowsSize - 2, CLOSE_INDEX);
						}
					}
				}
			}
            if(m_showMinuteLine)
            {
                div->GetLeftVScale()->SetMidValue(lastClose);
            }
			else
			{
				div->GetLeftVScale()->SetMidValue(0);
			}
			div->GetRightVScale()->SetMidValue(lastClose);
        }
		if(div != m_volumeDiv)
		{
			div->GetLeftVScale()->SetDigit(m_digit);
			div->GetRightVScale()->SetDigit(m_digit);
		}
		div->GetHScale()->SetScaleSteps(m_hScaleSteps);
		div->GetVGrid()->SetVisible(m_showMinuteLine);
	}
    vector<ControlA*> controls = m_native->m_controls;
	vector<ControlA*>::iterator sIter3 = controls.begin();
	for(; sIter3 != controls.end(); ++sIter3)
	{
		WindowFrameA *frame = dynamic_cast<WindowFrameA*>(*sIter3);
        if (frame)
        {
            WindowA *window = dynamic_cast<WindowA*>(frame->m_controls[0]);
            if (window)
            {
                vector<ControlA*> subControls = window->m_controls;
				vector<ControlA*>::iterator sIter4 = subControls.begin();
				for(; sIter4 != subControls.end(); ++sIter4)
				{
                    Battle *battle = dynamic_cast<Battle*>(*sIter4);
					GannSquare *gannSquare = dynamic_cast<GannSquare*>(*sIter4);
                    if (battle)
                    {
                        int speed = 100;
                        if (latestData.m_close > 0 && latestData.m_lastClose > 0)
                        {
                            speed -= (int)(1000 * (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose);
                            speed /= 2;
                        }
                        battle->SetSpeed(speed);
                    }
					else if(gannSquare)
					{
						gannSquare->SetStartValue(latestData.m_close);
						gannSquare->Update();
					}
                }
            }
        }
	}
}

void OwChart::RemoveBlankDivs(bool update)
{
	vector<CDiv*> divs = m_chart->GetDivs();
	vector<CDiv*>::iterator sIter = divs.begin();
	for(;sIter != divs.end();++sIter)
	{
		CDiv *div = *sIter;
		if (div != m_candleDiv && div != m_volumeDiv)
		{
			if(div->GetShapes(SORTTYPE_NONE).size() == 0)
			{
				vector<CDiv*>::iterator sIter2 = m_divs.begin();
				for(; sIter2 != m_divs.end(); ++sIter2)
				{
					if(div == *sIter2)
					{
						m_divs.erase(sIter2);
						break;
					}
				}
				m_chart->RemoveDiv(div);
				delete div;
			}
		}
	}
	divs = m_chart->GetDivs();
	sIter = divs.begin();
	int i = 0;
	for(; sIter != divs.end(); ++sIter)
	{
		CDiv *div = *sIter;
		if(i == (int)divs.size() - 1)
		{
			div->GetHScale()->SetVisible(true);
			div->GetHScale()->SetHeight(22);
		}
		else
		{
	        div->GetHScale()->SetVisible(false);
            div->GetHScale()->SetHeight(0);
		}
		i++;
	}
	if(update)
	{
		m_chart->Update();
		m_native->Invalidate();
	}
}

void OwChart::ResetScaleSize(SIZE clientSize)
{
	if(GetNative())
	{
		m_native = GetNative();
	}
	if(m_native)
	{
		SIZE nativeSize = m_native->GetDisplaySize();
        vector<ControlA*> controls = m_native->GetControls();
		vector<ControlA*>::iterator sIter = controls.begin();
		for(; sIter != controls.end(); ++sIter)
		{
            WindowFrameA *frame = dynamic_cast<WindowFrameA*>(*sIter);
            if (frame)
            {
                WindowEx *window = dynamic_cast<WindowEx*>(frame->m_controls[0]);
                if (window && !window->IsAnimateMoving())
                {
                    POINT location = window->GetLocation();
                    if (location.x < 10 || location.x > nativeSize.cx - 10)
                    {
                        location.x = 0;
                    }
                    if (location.y < 30 || location.y > nativeSize.cy - 30)
                    {
                        location.y = 0;
                    }
                    window->SetLocation(location);
                }
            }
        }
	}
	SIZE scaleSize = {(int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor)};
	m_native->SetScaleSize(scaleSize);
	m_native->Update();
	m_native->Invalidate();
}

void OwChart::SearchSecurity(Security *security)
{
	GetTabControl(L"tabMainFrame")->SetSelectedIndex(1);
	NewsList *newsList = dynamic_cast<NewsList*>(FindControl(L"divNewsList"));
	newsList->SetSecurityCode(security->m_code);
	if (m_showMinuteLine)
    {
		m_chart->SetAutoFillHScale(true);
		if (m_candleHScalePixel > 0)
		{
			m_candleHScalePixel = m_chart->GetHScalePixel();
		}
    }
    else
    {
		m_chart->SetAutoFillHScale(false);
		if (m_candleHScalePixel == 0)
        {
            m_candleHScalePixel = 9;
        }
        m_chart->SetHScalePixel(m_candleHScalePixel);
    }
	bool showCrossLine = m_chart->IsShowCrossLine();
	m_chart->Clear();
	m_chart->SetShowCrossLine(showCrossLine);
	String securityCode = security->m_code;
	String securityName = security->m_name;
	m_latestDiv->SetType(security->m_type);
	m_latestDiv->SetSecurityCode(securityCode);
	m_latestDiv->SetSecurityName(securityName);
	HistoryDataInfo dataInfo;
	dataInfo.m_securityCode = securityCode;
	int cycle = GetCycle();
	dataInfo.m_cycle = cycle;
	if (cycle <= 60)
    {
		wchar_t strTitle[10] = {0};
		if (m_showMinuteLine)
		{
			_stprintf_s(strTitle, 9, L"%s", L"分时线");
		}
		else
		{
			_stprintf_s(strTitle, 9, L"%d分钟线", cycle);
		}
        m_candleDiv->GetTitleBar()->SetText(strTitle);
    }
    else
    {
        if (cycle == 1440)
        {
            m_candleDiv->GetTitleBar()->SetText(L"日线");
        }
        else if(cycle == 10080)
        {
            m_candleDiv->GetTitleBar()->SetText(L"周线");
        }
        else if(cycle == 43200)
        {
            m_candleDiv->GetTitleBar()->SetText(L"月线");
        }
    }
	dataInfo.m_pushData = true;
    dataInfo.m_subscription = m_subscription;
	dataInfo.m_type = security->m_type;
	QuoteService *quoteService = DataCenter::GetQuoteService();
	quoteService->StopPushHistoryDatas(m_requestID);
	quoteService->UnRegisterListener(m_requestID);
	m_requestID = BaseService::GetRequestID();
	ListenerMessageCallBack historyDataCallBack = &QuoteDataCallBack;
    quoteService->RegisterListener(m_requestID, historyDataCallBack, this);
    quoteService->PushHistoryDatas(m_requestID, &dataInfo);
	m_chart->Update();
    m_native->Invalidate();
}

void OwChart::SetIndicatorLayout(const String& layoutID, const String& str)
{
	m_layoutID = layoutID;
	DeleteIndicators(false);
	RemoveBlankDivs(false);
    vector<CDiv*> divs = m_chart->GetDivs();
	vector<String> strs = CStr::Split(str, L"\r\n");
	int strsSize = (int)strs.size();
    int divPos = 0;
	vector<int> percents;
    for (int i = 0; i < strsSize; i++)
    {
        String subStr = strs[i];
		if(subStr.length() > 0)
		{
			if (subStr == L"DIV")
			{
				divPos++;
				if (divPos > (int)divs.size())
				{
					AddBlankDiv();
				}
			}
			else
			{
				vector<String> subStrs = CStr::Split(subStr, L"=");
				String name = subStrs[0];
				String value = subStrs[1];
				CDiv *div = m_chart->GetDivs()[divPos - 1];
				if (name == L"HEIGHT")
				{
					percents.push_back((int)CStr::ConvertStrToDouble(value.c_str()));
				}
				else if (name == L"IND")
				{
					Indicator indicator;
					if (m_indicatorService->GetIndicatorByName(value, &indicator))
					{
						AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, div, false);
					}
				}
				else if (name == L"VICEIND")
				{
					Indicator indicator;
					if (m_indicatorService->GetIndicatorByName(value, &indicator))
					{
						AddViceIndicator(indicator.m_name, indicator.m_text, indicator.m_parameters, div, false);
					}
				}
			}
		}
    }
	divs = m_chart->GetDivs();
    int percentsSize = (int)percents.size();
    for (int i = 0; i < percentsSize; i++)
    {
        divs[i]->SetVerticalPercent(percents[i]);
    }
    percents.clear();
	m_chart->Update();
    m_native->Invalidate();
}

void OwChart::SelectMenu(MenuItemA *item, const String& tag)
{
	bool setChecked = false;
	if(tag.length() >= 13 && tag.find(L"USERSECURITY_") == 0)
	{
		String type = tag.substr(13);
		if(type == L"ADD")
		{
			String securityCode = m_latestDiv->GetSecurityCode();
			if (securityCode.length() > 0)
			{
				AddUserSecurity(L"", securityCode);
			}
		}
		else if(type == L"MANAGER")
		{
			ShowUserSecurityList();
		}
	}
	else if (tag.length() >= 4 && tag.find(L"IND_") == 0)
	{
		String type = tag.substr(4);
		if (type.find(L"INDICATOREDIT") == 0)
		{
			ShowIndicatorList();
		}
		else if (type.find(L"SECURITYFILTER") == 0)
		{
			ShowSecurityFilterList();
		}
		if (type.length() >= 10 && type.find(L"ADDMAININD_") == 0)
		{
			String ind = type.substr(11);
			vector<Indicator> userIndicators;
			m_indicatorService->GetIndicators(&userIndicators);
			vector<Indicator>::iterator sIter = userIndicators.begin();
			for(;sIter != userIndicators.end();++sIter)
			{
				Indicator indicator = *sIter;
				if (indicator.m_name == ind)
				{
					CDiv *div = m_currentDiv;
                    if (!div)
                    {
                        div = m_chart->GetDivs()[m_chart->GetDivs().size() - 1];
                    }
					AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, div, true);
					break;
				}
			}
		}
		else if (type.length() >= 10 && type.find(L"ADDVICEIND_") == 0)
		{
			String ind = type.substr(11);
			vector<Indicator> userIndicators;
			m_indicatorService->GetIndicators(&userIndicators);
			vector<Indicator>::iterator sIter = userIndicators.begin();
			for(;sIter != userIndicators.end();++sIter)
			{
				Indicator indicator = *sIter;
				if (indicator.m_name == ind)
				{
					CDiv *div = m_currentDiv;
                    if (!div)
                    {
                        div = m_chart->GetDivs()[m_chart->GetDivs().size() - 1];
                    }
					AddViceIndicator(indicator.m_name, indicator.m_text, indicator.m_parameters, div, true);
					break;
				}
			}
		}
		else if (type == L"DELETEIND")
		{
			DeleteSelectedIndicator();
		}
        else if (type == L"SETPARAMETERS")
        {
            SetIndicatorParameters();
        }
	}
	else if (tag.length() >= 7 && tag.find(L"CANDLE_") == 0)
	{
		if(!m_showMinuteLine)
		{
			String candleStyle = tag.substr(7);
			if(candleStyle == L"STANDARD")
			{
				m_candle->SetStyle(CandleStyle_Rect);
			}
			else if(candleStyle == L"TOWER")
			{
				m_candle->SetStyle(CandleStyle_Tower);
			}
			else if(candleStyle == L"AMERICAN")
			{
				m_candle->SetStyle(CandleStyle_American);
			}
			else if(candleStyle == L"CLOSE")
			{
				m_candle->SetStyle(CandleStyle_CloseLine);
			}
			setChecked = true;
		}
	}
	else if (tag.length() >= 6 && tag.find(L"SCALE_") == 0)
	{
		String scaleStyle = tag.substr(6);
		if(scaleStyle == L"STANDARD")
		{
			m_candleDiv->GetLeftVScale()->SetSystem(VScaleSystem_Standard);
		}
		else if(scaleStyle == L"LOG")
		{
			m_candleDiv->GetLeftVScale()->SetSystem(VScaleSystem_Logarithmic);
		}
		else if(scaleStyle == L"DIFF")
		{
			m_candleDiv->GetLeftVScale()->SetType(VScaleType_EqualDiff);
		}
		else if(scaleStyle == L"EQUALRATIO")
		{
			m_candleDiv->GetLeftVScale()->SetType(VScaleType_EqualRatio);
		}
		else if(scaleStyle == L"DIVIDE")
		{
			m_candleDiv->GetLeftVScale()->SetType(VScaleType_Divide);
		}
		else if(scaleStyle == L"PERCENT")
		{
			m_candleDiv->GetLeftVScale()->SetType(VScaleType_Percent);
		}
		else if(scaleStyle == L"GOLDENRATIO")
		{
			m_candleDiv->GetLeftVScale()->SetType(VScaleType_GoldenRatio);
		}
		else if(scaleStyle == L"REVERSEH")
		{
			m_chart->SetReverseHScale(!m_chart->IsReverseHScale());
		}
		else if(scaleStyle == L"REVERSEV")
		{
			m_reverseVScale = !m_reverseVScale;
            vector<CDiv*> divs = m_chart->GetDivs();
			vector<CDiv*>::iterator sIter = divs.begin();
			for(; sIter != divs.end(); ++sIter)
			{
                CDiv *div = *sIter;
                if (div != m_volumeDiv)
                {
                    div->GetLeftVScale()->SetReverse(m_reverseVScale);
                    div->GetRightVScale()->SetReverse(m_reverseVScale);
                }
			}
		}
	}
	else if(tag.length() >= 7 && tag.find(L"LAYOUT_") == 0)
	{
		String type = tag.substr(7);
		if(type == L"ADDBLANKDIV")
		{
			AddBlankDiv();
		}
		else if(type == L"REMOVEBLANKDIVS")
		{
			RemoveBlankDivs(true);
		}
		else if(type == L"LAYOUTMANAGER")
		{
			ShowIndicatorLayoutList();	
		}
	}
	else if(tag.length() >= 6 && tag.find(L"CYCLE_") == 0)
	{
		String type = tag.substr(6);
		int cycle = 0;
		if(type == L"MINUTELINE")
		{
			m_showMinuteLine = true;
		}
		else if(type == L"1MINUTE")
		{
			cycle = 1;
		}
		else if(type == L"5MINUTE")
		{
			cycle = 5;
		}
		else if(type == L"15MINUTE")
		{
			cycle = 15;
		}
		else if(type == L"30MINUTE")
		{
			cycle = 30;
		}
		else if(type == L"60MINUTE")
		{
			cycle = 60;
		}
		else if(type == L"DAY")
		{
			cycle = 1440;
		}
		else if(type == L"WEEK")
		{
			cycle = 10080;
		}
		else if(type == L"MONTH")
		{
			cycle = 43200;
		}
		ChangeCycle(cycle);
		setChecked = true;
	}
	else if(tag.length() >= 13 && tag.find(L"SUBSCRIPTION_") == 0)
	{
		if(!m_showMinuteLine)
		{
			String type = tag.substr(13);
			if(type == L"NONE")
			{
				m_subscription = 0;
			}
			else if(type == L"FRONT")
			{
				m_subscription = 1;
			}
			else if(type == L"BACK")
			{
				m_subscription = 2;
			}
			String securityCode = m_latestDiv->GetSecurityCode();
			if (securityCode.length() > 0)
			{
				Security security;
				m_securityService->GetSecurityByCode(securityCode, &security);
				SearchSecurity(&security);
			}
			setChecked = true;
		}
	}
	else if (tag.length() >= 5 && tag.find(L"PLOT_") == 0)
	{
		String type = tag.substr(5);
		if (type.length() >= 8 && type.find(L"ADDPLOT_") == 0)
		{
			m_addingPlotType = type.substr(8);
		}
		else if (type.length() >= 10 && type.find(L"DELETEPLOT") == 0)
		{
			DeleteSelectedPlot();
		}
	}
    else if(tag.length() >= 6 && tag.find(L"TOOLS_") == 0)
    {
        String type = tag.substr(6);
		if(type == L"CHAT")
		{
			ShowChatWindow();
		}
		else if(type == L"SANDBOX")
		{
			if(!m_showMinuteLine)
			{
				ShowSandBoxWindow();
			}
		}
		else if (type == L"GANNSQUARE")
        {
            WindowEx *window = new WindowEx;
            window->SetText(L"江恩正方");
            window->SetNative(m_native);
            GannSquare *gannSquare = new GannSquare;
            gannSquare->SetBackColor(COLOR_EMPTY);
			SIZE cellSize = {70, 30};
            gannSquare->SetCellSize(cellSize);
            int number = gannSquare->GetNumber();
			SIZE size = {cellSize.cx * number, cellSize.cy * number};
            gannSquare->SetSize(size);
            gannSquare->SetStyle(SquareStyle_Spiral);
			POINT location = {5, 26};
            gannSquare->SetLocation(location);
            size.cx += 10;
            size.cy += 32;
            window->SetSize(size);
            window->AddControl(gannSquare);
            CTable *dataSource = m_chart->GetDataSource();
            int rowsCount = dataSource->RowsCount();
            if (rowsCount > 0)
            {
                gannSquare->SetStartValue(dataSource->Get2(rowsCount - 1, CLOSE_INDEX));
            }
			gannSquare->Update();
            window->AnimateShow(false);
            gannSquare->Focus();
			ButtonA *closeButton = window->GetCloseButton();
			closeButton->RegisterEvent(&CloseToolWindow, EVENTID::CLICK, this);
        }
        else if (type == L"EATSNAKE")
        {
            WindowEx *window = new WindowEx;
            window->SetText(L"贪吃蛇");
            window->SetNative(m_native);
            Battle *battle = new Battle;
			SIZE size = {400, 400};
            battle->SetSize(size);
            SIZE nativeSize = m_native->GetDisplaySize();
			POINT location = {5, 26};
            battle->SetLocation(location);
            size.cx += 10;
            size.cy += 32;
            window->SetSize(size);
            window->AddControl(battle);
            window->AnimateShow(false);
            battle->Focus();
			ButtonA *closeButton = window->GetCloseButton();
			closeButton->RegisterEvent(&CloseToolWindow, EVENTID::CLICK, this);
        }
    }
	else if(tag.length() >= 6 && tag.find(L"MACRO_") == 0)
	{
		String macroType = tag.substr(6);
		if(macroType == L"MANAGER")
		{
            MacroList *macroList = new MacroList(m_native);
            macroList->SetChart(this);
            macroList->Show();
		}
	}
	else if(tag.length() >= 9 && tag.find(L"DRAWMODE_") == 0)
	{
		String type = tag.substr(9);
		if(type == L"HIGH")
		{
			GdiPlusPaintEx *gdiPlusPaintEx = new GdiPlusPaintEx;
			m_native->SetPaint(gdiPlusPaintEx);
			m_native->Invalidate();
		}
		else if(type == L"LOW")
		{
			GdiPaintEx *gdiPaintEx = new GdiPaintEx;
			m_native->SetPaint(gdiPaintEx);
			m_native->Invalidate();
		}
	}
    if (setChecked)
    {
        vector<MenuItemA*> items = item->GetParentItem()->GetItems();
		vector<MenuItemA*>::iterator sIter = items.begin();
		for(; sIter != items.end(); ++sIter)
		{
			(*sIter)->SetChecked(*sIter == item);
		}
    }
	m_chart->Update();
	m_native->Invalidate();
}


void OwChart::SetIndicatorParameters()
{
    CIndicator *indicator = GetSelectedIndicator();
    if (indicator)
    {
		String parameters = indicator->GetParameters();
        if (parameters.length() > 0)
        {
            ParametersWindow *parametersWindow = new ParametersWindow(m_native, indicator);
            parametersWindow->SetChart(this);
            parametersWindow->Show();
        }
    }
}

void OwChart::ShowChatWindow()
{
    ChatWindow *chatWindow = new ChatWindow(m_native);
    chatWindow->SetChart(this);
    chatWindow->Show();
}

void OwChart::ShowIndicatorLayoutList()
{
    IndicatorLayoutList *indicatorLayoutList = new IndicatorLayoutList(m_native);
    indicatorLayoutList->SetChart(this);
    indicatorLayoutList->Show();
}

void OwChart::ShowIndicatorList()
{
    IndicatorList *indicatorList = new IndicatorList(m_native);
    indicatorList->SetChart(this);
    indicatorList->SetDiv(m_chart->GetMouseOverDiv());
    indicatorList->Show();
}

void OwChart::ShowLoginWindow()
{
	LoginWindow *loginWindow = new LoginWindow(m_native);
    loginWindow->SetChart(this);
    loginWindow->Show();
}

int OwChart::ShowMessageBox(const String& text, const String& caption, UINT uType)
{
	WinformControlHost *host = dynamic_cast<WinformControlHost*>(m_host);
	return MessageBox(host->GetHWnd(), text.c_str(), caption.c_str(), uType);
}

void OwChart::ShowSandBoxWindow()
{
    SandBoxWindow *sandBoxWindow = new SandBoxWindow(m_native);
    sandBoxWindow->SetChart(this);
    sandBoxWindow->Show();
}

void OwChart::ShowSearchDiv(char key)
{
	bool focuedTextBox = false;
	ControlA *focusedControl = m_native->GetFocusedControl();
	if(focusedControl)
	{
		String name = focusedControl->GetName();
        if (IsWindowShowing() && name != L"txtSearch")
        {
            return;
        }
		TextBoxA *textBox = dynamic_cast<TextBoxA*>(focusedControl);
		if(textBox)
		{
			focuedTextBox = true;
		}
		if (!focuedTextBox || (m_searchDiv && focusedControl == m_searchDiv->m_searchTextBox) || name == L"txtSearch")
		{
			if(!m_searchDiv)
			{
				m_searchDiv = new SearchDiv();
				m_native->AddControl(m_searchDiv);
				m_searchDiv->SetChart(this);
				m_searchDiv->SetPopup(true);
				SIZE sSize = {240, 200};
				m_searchDiv->SetSize(sSize);
				m_searchDiv->SetVisible(false);
				m_searchDiv->BringToFront();
			}
			if(key == 27)
			{
				m_searchDiv->SetVisible(false);
				m_searchDiv->Invalidate();
			}
			else if(key == 116)
			{
				m_showMinuteLine = !m_showMinuteLine;
				if(m_showMinuteLine)
				{
					m_cycle = 0;
				}
				else
				{
					m_cycle = 1440;
				}
				String securityCode = m_latestDiv->GetSecurityCode();
				if (securityCode.length() > 0)
				{
					Security security;
					m_securityService->GetSecurityByCode(securityCode, &security);
					SearchSecurity(&security);
				}
			}
			else
			{
				if (!m_searchDiv->IsVisible())
				{
					char ch = '\0';
					if ((key >= 48) && (key <= 57))
					{
						ch = (char)((0x30 + key) - 0x30);
					}
					else if ((key >= 65) && (key <= 90))
					{
						ch = (char)((0x41 + key) - 0x41);
					}
					else if ((key >= 96) && (key <= 122))
					{
						ch = (char)((0x30 + key) - 0x60);
					}
					if (ch != '\0')
					{
						SIZE size = m_native->GetHost()->GetSize();
						POINT location = {size.cx - m_searchDiv->GetWidth(), size.cy - m_searchDiv->GetHeight()};
						if(name == L"txtSearch")
						{
							POINT fPoint = {0};
                            fPoint = focusedControl->PointToNative(fPoint);
                            location.x = fPoint.x;
							location.y = fPoint.y - m_searchDiv->GetHeight() + focusedControl->GetHeight(); 
							String *pCategoryID = (String*)focusedControl->GetTag();
							m_searchDiv->SetCategoryID(*pCategoryID);
							delete pCategoryID;
							pCategoryID = 0;
						}
						else
						{
							m_searchDiv->SetCategoryID(L"");
						}
						m_searchDiv->SetLocation(location);
						m_searchDiv->m_searchTextBox->SetText(L"");
						m_searchDiv->FilterSearch();
						m_searchDiv->Show();
						m_searchDiv->m_searchTextBox->SetFocused(true);
						m_searchDiv->Update();
						m_searchDiv->Invalidate();
					}
				}
			}
		}
	}
}

void OwChart::ShowSecurityFilterList()
{
    SecurityFilterList *securityFilterList = new SecurityFilterList(m_native);
    securityFilterList->SetChart(this);
    securityFilterList->Show();
}

void OwChart::ShowUserSecurityList()
{
    UserSecurityList *userSecurityList = new UserSecurityList(m_native);
    userSecurityList->SetChart(this);
    userSecurityList->Show();
}

int OwChart::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	if(message == WM_LBUTTONDOWN)
	{
		if(m_addingPlotType.length() > 0)
		{
			POINT mp = m_native->GetMousePoint();
			_int64 plotColor = PCOLORS_LINECOLOR;
            CPlot *plot = 0;
			plot = PFactory::CreatePlot(m_addingPlotType);
            m_chart->SetShowCrossLine(false);
            plot->SetColor(plotColor);
            plot->SetSelectedColor(plotColor);
            CDiv *mouseOverDiv = m_chart->GetMouseOverDiv();
			POINT pp = {mp.x - m_chart->GetLeft() - mouseOverDiv->GetLeft(), 
				mp.y - m_chart->GetTop() - mouseOverDiv->GetTop() - mouseOverDiv->GetTitleBar()->GetHeight()};
            m_chart->AddPlot(plot, pp, mouseOverDiv);
			m_native->SetCursor(CursorsA_Arrow);
            m_chart->Invalidate();
			m_addingPlotType = L"";
		}
	}
	else if(message == WM_RBUTTONDOWN)
	{
		if(m_chart && !IsWindowShowing())
		{
			POINT mp = m_native->GetMousePoint();
			BaseShape *selectedShape = m_chart->GetSelectedShape();
			CIndicator *selectedIndicator = GetSelectedIndicator();
			CPlot *selectedPlot = m_chart->GetSelectedPlot();
			if(selectedShape && selectedIndicator)
			{
				vector<BaseShape*> shapes = selectedIndicator->GetShapes();
				vector<BaseShape*>::iterator sIter = shapes.begin();
				for(;sIter != shapes.end();++sIter)
				{
					if(selectedShape == *sIter)
					{
						if(m_indRightMenu)
						{
							m_indRightMenu->SetFocused(true);
							m_indRightMenu->SetVisible(true);
							m_indRightMenu->SetLocation(mp);
							m_indRightMenu->Update();
							m_indRightMenu->BringToFront();
							m_native->Invalidate();
							return 0;
						}
					}
				}
			}
			else if(selectedPlot)
			{
				if(m_plotRightMenu)
				{
					m_plotRightMenu->SetFocused(true);
					m_plotRightMenu->SetVisible(true);
					m_plotRightMenu->SetLocation(mp);
					m_plotRightMenu->Update();
					m_plotRightMenu->BringToFront();
					m_native->Invalidate();
					return 0;
				}
			}
			else
			{
				if(m_rightMenu)
				{
					m_currentDiv = m_chart->GetMouseOverDiv();
					m_rightMenu->SetFocused(true);
					m_rightMenu->SetVisible(true);
					SIZE nativeSize = m_native->GetDisplaySize();
					int rightMenuHeight = m_rightMenu->GetHeight();
					if (mp.y + rightMenuHeight > nativeSize.cy)
					{
						mp.y = nativeSize.cy - rightMenuHeight;
					}
					m_rightMenu->SetLocation(mp);
					m_rightMenu->Update();
					m_rightMenu->BringToFront();
					m_native->Invalidate();
					return 0;
				}
			}
		}
	}
	else if(message == WM_KEYDOWN)
	{
		char key = (char)wParam;
		ShowSearchDiv(key);
	}
	if(m_host && m_host->OnMessage(hWnd, message, wParam, lParam))
	{
		return 1;
	}
	if(message == WM_LBUTTONDBLCLK && m_float && m_chart)
	{
		m_float->SetVisible(m_chart->IsShowCrossLine());
	}	
	return 0;
}

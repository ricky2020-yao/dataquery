#include "..\\..\\stdafx.h"
#include "SecurityFilterList.h"

void SecurityFilterList::AddTemplatesToGrid(vector<SecurityFilterTemplate> *templates)
{
    m_gridTemplate->BeginUpdate();
	vector<SecurityFilterTemplate>::iterator sIter = templates->begin();
	for(; sIter != templates->end(); ++sIter)
	{
        SecurityFilterTemplate sfTemplate = *sIter;
        GridRow *row = new GridRow;
        row->SetGrid(m_gridTemplate);
        GridStringCell *idCell = new GridStringCell;
        idCell->SetGrid(m_gridTemplate);
        idCell->SetColumn(m_gridTemplate->GetColumn(0));
        idCell->SetText(sfTemplate.m_templateID);
        row->AddCell(0, idCell);
        GridStringCell *textCell = new GridStringCell;
        textCell->SetGrid(m_gridTemplate);
        textCell->SetColumn(m_gridTemplate->GetColumn(1));
        textCell->SetText(sfTemplate.m_name);
        row->AddCell(1, textCell);
        m_gridTemplate->AddRow(row);
	}
    m_gridTemplate->EndUpdate();
}

void SecurityFilterList::CallTimer(void *sender, int timerID, void *pInvoke)
{
	SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
	securityFilterList->OnTimer(timerID);
}

void SecurityFilterList::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
        if (name == L"btnClose")
        {
            securityFilterList->Close();
        }
		else if (name == L"btnCreate")
        {
            securityFilterList->Create();
        }
        else if (name == L"btnDelete")
        {
            securityFilterList->Delete();
        }
        else if (name == L"btnRun")
        {
			securityFilterList->Run();
        }
        else if (name == L"btnSelectSecurities")
        {
            securityFilterList->SelectSecurities();
        }
        else if (name == L"btnSave")
        {
            securityFilterList->Save();
        }
        else if (name == L"btnSelectIndicator")
        {
            securityFilterList->SelectIndicator();
        }
        else if (name == L"btnWatch")
        {
			securityFilterList->Watch();
        }
    }
}

map<String, GridRow*> SecurityFilterList::GetTemplateRows()
{
    map<String, GridRow*> templateRowsMap;
    vector<GridRow*> rows = m_gridTemplate->m_rows;
	vector<GridRow*>::iterator sIter = rows.begin();
	for(; sIter != rows.end(); ++sIter)
	{
		GridRow *row = *sIter;
        templateRowsMap[row->GetCell(0)->GetText()] = row;
	}
    return templateRowsMap;
}

void SecurityFilterList::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
	securityFilterList->OnGridSelectedRowsChanged();
}

void SecurityFilterList::IndicatorListWindowClosed(void *sender, bool *cancel, void *pInvoke)
{
	SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
    ControlA *control = (ControlA*)sender;
	IndicatorList *indicatorList = (IndicatorList*)control->GetTag();
    securityFilterList->OnIndicatorListWindowClosed(indicatorList, cancel);
}

void SecurityFilterList::Invoke(void *sender, void *args, void *pInvoke)
{
	SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
	securityFilterList->OnInvoke(args);
}

void SecurityFilterList::LoadTemplate(SecurityFilterTemplate *sfTemplate)
{
    TextBoxA *txtName = GetTextBox(L"txtName");
    TextBoxA *txtIndicator = GetTextBox(L"txtIndicator");
    TextBoxA *txtParameters = GetTextBox(L"txtParameters");
    ComboBoxA *cbCycle = GetComboBox(L"cbCycle");
    ComboBoxA *cbSubscription = GetComboBox(L"cbSubscription");
    TextBoxA *txtFilter = GetTextBox(L"txtFilter");
    ButtonA *btnDelete = GetButton(L"btnDelete");
    ButtonA *btnRun = GetButton(L"btnRun");
    ButtonA *btnWatch = GetButton(L"btnWatch");
    ButtonA *btnSave = GetButton(L"btnSave");
    txtName->SetText(sfTemplate->m_name);
    txtIndicator->SetText(sfTemplate->m_indicator);
    txtParameters->SetText(sfTemplate->m_parameters);
	wchar_t szCycle[10] = {0};
	_stprintf_s(szCycle, 9, L"%d", sfTemplate->m_cycle);
    cbCycle->SetSelectedValue(szCycle);
	cbSubscription->SetSelectedIndex(sfTemplate->m_subscription);
    txtFilter->SetText(sfTemplate->m_filter);
    m_codes = sfTemplate->m_codes;
    bool btnEnabled = sfTemplate->m_templateID.length() > 0;
    btnDelete->SetEnabled(btnEnabled);
    btnRun->SetEnabled(btnEnabled);
    btnWatch->SetEnabled(btnEnabled);
    btnSave->SetEnabled(btnEnabled);
}

void SecurityFilterList::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ControlA *subControl = *sIter;
		ButtonA *button = dynamic_cast<ButtonA*>(subControl);
		GridA *grid = dynamic_cast<GridA*>(subControl);
		if(button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		else if(grid)
		{
			GridRowStyle rowStyle;
			rowStyle.SetBackColor(COLOR_EMPTY);
			rowStyle.SetSelectedBackColor(PCOLORS_SELECTEDROWCOLOR);
			rowStyle.SetHoveredBackColor(PCOLORS_HOVEREDROWCOLOR);
			grid->SetRowStyle(&rowStyle);
		}

		RegisterEvents(*sIter);
	}
}

void SecurityFilterList::SecurityFilterDataCallBack(CMessage *message, void *pInvoke)
{
	SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
	securityFilterList->OnSecurityFilterDataCallBack(message);
}

void SecurityFilterList::SecurityListWindowClosed(void *sender, bool *cancel, void *pInvoke)
{
    ControlA *control = (ControlA*)sender;
	SecurityFilterList *securityFilterList = (SecurityFilterList*)pInvoke;
	SecurityList *securityList = (SecurityList*)control->GetTag();
    securityFilterList->OnSecurityListWindowClosed(securityList, cancel);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityFilterList::SecurityFilterList(NativeBase *native)
{
	m_chart = 0;
	m_codes = L"";
	m_native = native;
	m_timerID = ControlA::GetNewTimerID();
	string xmlPath = DataCenter::GetAppPath() + "\\config\\SecurityFilterList.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowSF"));
    m_gridTemplate = GetGrid(L"gridTemplates");
    m_gridSelectedRowsChangedEvent = &GridSelectedRowsChanged;
	m_gridTemplate->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
    m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    m_timerEvent = &CallTimer;
	m_window->RegisterEvent(m_timerEvent, EVENTID::TIMER, this);
    RegisterEvents(m_window);
	m_securityService = DataCenter::GetSecurityService();
	m_securityFilterService = DataCenter::GetSecurityFilterService();
	m_securityFilterDataCallBackEvent = &SecurityFilterDataCallBack;
    m_securityFilterService->RegisterListener(m_securityFilterService->GetOperatorRequestID(), m_securityFilterDataCallBackEvent, this);
}

SecurityFilterList::~SecurityFilterList()
{
	m_chart = 0;
    if (m_gridTemplate)
    {
		m_gridTemplate->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_gridTemplate = 0;
    }
	m_native = 0;
	m_securityService = 0;
    if (m_securityFilterService)
    {
        m_securityFilterService->UnRegisterListener(m_securityFilterService->GetOperatorRequestID(), m_securityFilterDataCallBackEvent);
		m_securityFilterDataCallBackEvent = 0;
		m_securityFilterService = 0;
    }
	if (m_window)
    {
		m_window->UnRegisterEvent(m_invokeEvent, EVENTID::INVOKE);
        m_invokeEvent = 0;
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
}

OwChart* SecurityFilterList::GetChart()
{
	return m_chart;
}

void SecurityFilterList::SetChart(OwChart *chart)
{
	m_chart = chart;
}

WindowEx* SecurityFilterList::GetWindow()
{
	return m_window;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityFilterList::Close()
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

void SecurityFilterList::Create()
{
    SecurityFilterTemplate sfTemplate;
    sfTemplate.m_name = L"新的模板";
    sfTemplate.m_cycle = 1440;
    sfTemplate.m_subscription = 1;
	sfTemplate.m_userID = DataCenter::GetUserID();
    m_securityFilterService->AddTemplate(m_securityFilterService->GetOperatorRequestID(), &sfTemplate);
}

void SecurityFilterList::Delete()
{
    vector<GridRow*> selectedRows = m_gridTemplate->GetSelectedRows();
    int selectedRowsSize = (int)selectedRows.size();
    if (selectedRowsSize > 0)
    {
        String templateID = selectedRows[0]->GetCell(0)->GetText();
        SecurityFilterTemplate sfTemplate;
        m_securityFilterService->GetTemplate(templateID, &sfTemplate);
        m_securityFilterService->DeleteTemplate(m_securityFilterService->GetOperatorRequestID(), &sfTemplate);
    }
}

void SecurityFilterList::GetTemplate(SecurityFilterTemplate *sfTemplate)
{
    TextBoxA *txtName = GetTextBox(L"txtName");
    TextBoxA *txtIndicator = GetTextBox(L"txtIndicator");
    TextBoxA *txtParameters = GetTextBox(L"txtParameters");
    ComboBoxA *cbCycle = GetComboBox(L"cbCycle");
    ComboBoxA *cbSubscription = GetComboBox(L"cbSubscription");
    TextBoxA *txtFilter = GetTextBox(L"txtFilter");
	if (m_template.m_templateID.length() > 0)
    {
        sfTemplate->m_templateID = m_template.m_templateID;
    }
    sfTemplate->m_name = txtName->GetText();
    sfTemplate->m_indicator = txtIndicator->GetText();
    sfTemplate->m_parameters = txtParameters->GetText();
	if(cbCycle->GetSelectedValue().length() > 0)
	{
		sfTemplate->m_cycle = CStr::ConvertStrToInt(cbCycle->GetSelectedValue().c_str());
	}
	else
	{
		sfTemplate->m_cycle = 1440;
	}
	sfTemplate->m_subscription = cbSubscription->GetSelectedIndex();
    sfTemplate->m_filter = txtFilter->GetText();
    sfTemplate->m_codes = m_codes;
	sfTemplate->m_userID = DataCenter::GetUserID();
}

void SecurityFilterList::OnGridSelectedRowsChanged()
{
	if(m_gridTemplate)
	{
		String templateID = L"";
		vector<GridRow*> selectedRows = m_gridTemplate->GetSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		if (selectedRowsSize > 0)
		{
			templateID = selectedRows[0]->GetCell(0)->GetText();
		}
		SecurityFilterTemplate sfTemplate;
		m_securityFilterService->GetTemplate(templateID, &sfTemplate);
		m_template = sfTemplate;
		LoadTemplate(&m_template);
		m_window->Invalidate();
	}
}

void SecurityFilterList::OnIndicatorListWindowClosed(IndicatorList *indicatorList, bool *cancel)
{
    if (indicatorList->IsSubmit())
    {
        Indicator indicator;
        indicatorList->GetIndicator(&indicator);
        TextBoxA *txtIndicator = GetTextBox(L"txtIndicator");
        TextBoxA *txtParameters = GetTextBox(L"txtParameters");
        txtIndicator->SetText(indicator.m_text);
        if (indicator.m_parameters.length() > 0)
        {
			vector<String> strs = CStr::Split(indicator.m_parameters, L";");
            int strsSize = (int)strs.size();
            String parameters = L"";
            for (int i = 0; i < strsSize; i++)
            {
                if (strs[i].find(L",") != -1)
                {
					vector<String> subStrs = CStr::Split(strs[i], L",");
                    parameters += subStrs[0] + L":" + subStrs[3] + L";";
                }
            }
            txtParameters->SetText(parameters);
        }
        else
        {
            txtParameters->SetText(L"");
        }
    }
	m_window->StartTimer(m_timerID, 100);
	m_window->Invalidate();
}

void SecurityFilterList::OnInvoke(void *args)
{
    CMessage *message = (CMessage*)args;
    vector<SecurityFilterTemplate> templates;
	SecurityFilterService::GetTemplates(&templates, message->m_body, message->m_bodyLength);
    switch (message->m_functionID)
    {
        case FUNCTIONID_SECURITYFILTER_ADDTEMPLATES:
            AddTemplatesToGrid(&templates);
            break;
        case FUNCTIONID_SECURITYFILTER_DELETETEMPLATES:
            {
                map<String, GridRow*> templateRowsMap = GetTemplateRows();
				vector<SecurityFilterTemplate>::iterator sIter = templates.begin();
				for(; sIter != templates.end(); ++sIter)
				{
					SecurityFilterTemplate sfTemplate = *sIter;
					map<String, GridRow*>::iterator sIter2 = templateRowsMap.find(sfTemplate.m_templateID);
					if(sIter2 != templateRowsMap.end())
					{
						m_gridTemplate->RemoveRow(sIter2->second);
						delete sIter2->second;
					}
				}
				m_gridTemplate->Update();
                break;
            }
        case FUNCTIONID_SECURITYFILTER_UPDATETEMPLATES:
            {
				map<String, GridRow*> templateRowsMap = GetTemplateRows();
				vector<SecurityFilterTemplate>::iterator sIter = templates.begin();
				for(; sIter != templates.end(); ++sIter)
				{
					SecurityFilterTemplate sfTemplate = *sIter;
					map<String, GridRow*>::iterator sIter2 = templateRowsMap.find(sfTemplate.m_templateID);
					if(sIter2 != templateRowsMap.end())
					{
						sIter2->second->GetCell(1)->SetText(sfTemplate.m_name);
					}
				}
                break;
            }
    }
    m_window->Invalidate();
}

void SecurityFilterList::OnSecurityFilterDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void SecurityFilterList::OnSecurityListWindowClosed(SecurityList *securityList, bool *cancel)
{
    if (securityList->IsSubmit())
    {
        vector<Security> selectedSecurities;
        securityList->GetSelectedSecurities(&selectedSecurities);
        int selectedSecuritiesSize = (int)selectedSecurities.size();
        String codes = L"";
        for (int i = 0; i < selectedSecuritiesSize; i++)
        {
            codes += selectedSecurities[i].m_code;
            if (i != selectedSecuritiesSize - 1)
            {
                codes += L",";
            }
        }
        m_codes = codes;
    }
	m_window->StartTimer(m_timerID, 100);
	m_window->Invalidate();
}

void SecurityFilterList::OnTimer(int timerID)
{
    if (m_timerID == timerID)
    {
        SecurityFilterTemplate sfTemplate;
        GetTemplate(&sfTemplate);
        SecurityFilterTask *task = m_securityFilterService->GetTask(sfTemplate.m_templateID);
        ButtonA *btnRun = GetButton(L"btnRun");
        if (task && btnRun)
        {
            if (task->GetRunningState() >= 0)
            {
                if (btnRun->GetText() != L"停止")
                {
                    btnRun->SetText(L"停止");
                    m_window->Invalidate();
                }
            }
            else
            {
                if (btnRun->GetText() != L"运行")
                {
                    btnRun->SetText(L"运行");
                    m_window->Invalidate();
                }
            }
        }
        else
        {
            if (btnRun && btnRun->GetText() != L"运行")
            {
                btnRun->SetText(L"运行");
                m_window->Invalidate();
            }
        }
    }
}

void SecurityFilterList::Run()
{
    SecurityFilterTemplate sfTemplate;
    GetTemplate(&sfTemplate);
    SecurityFilterTask *task = m_securityFilterService->GetTask(sfTemplate.m_templateID);
    if (!task)
    {
        m_securityFilterService->AddTask(sfTemplate, m_native);
    }
    task = m_securityFilterService->GetTask(sfTemplate.m_templateID);
    if (task)
    {
        if (task->GetRunningState() >= 0)
        {
            task->Stop();
        }
        else
        {
            task->SetTemplate(sfTemplate);
            task->Run();
        }
        m_window->Invalidate();
    }
}

void SecurityFilterList::Save()
{
    SecurityFilterTemplate sfTemplate;
    GetTemplate(&sfTemplate);
    if (sfTemplate.m_templateID.length() > 0)
    {
        m_securityFilterService->UpdateTemplate(m_securityFilterService->GetOperatorRequestID(), &sfTemplate);
    }
}

void SecurityFilterList::SelectIndicator()
{
    IndicatorList *indicatorList = new IndicatorList(m_native);
    indicatorList->Show();
    WindowEx *window = indicatorList->GetWindow();
	window->SetChildWindow(true);
    window->SetTag(indicatorList);
    WindowClosingEvent windowClosingEvent = &IndicatorListWindowClosed;
	window->RegisterEvent(windowClosingEvent, EVENTID::WINDOWCLOSING, this);
	m_native->Invalidate();
}

void SecurityFilterList::SelectSecurities()
{
    SecurityList *securityList = new SecurityList(m_native);
    securityList->Show();
    WindowEx *window = securityList->GetWindow();
	window->SetChildWindow(true);
    window->SetTag(securityList);
    if (m_codes.length() > 0)
    {
        vector<Security> securities;
		m_securityService->GetSecuritiesByCodes(m_codes, &securities);
        securityList->AddSecuritiesToSecuritiesGrid(&securities);
    }
    WindowClosingEvent windowClosingEvent = &SecurityListWindowClosed;
	window->RegisterEvent(windowClosingEvent, EVENTID::WINDOWCLOSING, this);
	m_native->Invalidate();
}

void SecurityFilterList::Show()
{
	vector<SecurityFilterTemplate> templates;
    m_securityFilterService->GetTemplates(&templates);
    AddTemplatesToGrid(&templates);
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(true);
    m_window->Invalidate();
	m_window->StartTimer(m_timerID, 1000);
}

void SecurityFilterList::Watch()
{
    SecurityFilterTemplate sfTemplate;
    GetTemplate(&sfTemplate);
    SecurityFilterTask *task = m_securityFilterService->GetTask(sfTemplate.m_templateID);
    if (task)
    {
        SecurityFilterResult *securityFilterResult = new SecurityFilterResult(m_native);
        securityFilterResult->SetChart(m_chart);
        securityFilterResult->SetTask(task);
        securityFilterResult->Show();
        WindowEx *window = securityFilterResult->GetWindow();
		window->SetChildWindow(true);
		m_native->Invalidate();
    }
}
#include "..\\..\\stdafx.h"
#include "IndicatorList.h"

void IndicatorList::CallTimer(void *sender, int timerID, void *pInvoke)
{
	IndicatorList *indicatorList = (IndicatorList*)pInvoke;
	indicatorList->OnTimer(timerID);
}

void IndicatorList::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		IndicatorList *indicatorList = (IndicatorList*)pInvoke;
        if (name == L"cbUsePassword")
        {
            indicatorList->ChangeUsePassword();
        }
        else if (name == L"btnClose")
        {
            indicatorList->Close();
        }
        else if (name == L"btnDelete")
        {
            indicatorList->Delete();
        }
        else if (name == L"btnSave")
        {
            indicatorList->Save(false);
        }
        else if (name == L"btnSaveAs")
        {
            indicatorList->Save(true);
        }
        else if (name == L"btnSubmit")
        {
            indicatorList->Submit();
        }
    }
}

void IndicatorList::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	IndicatorList *indicatorList = (IndicatorList*)pInvoke;
	indicatorList->OnGridSelectedRowsChanged();
}

void IndicatorList::IndicatorDataCallBack(CMessage *message, void *pInvoke)
{
	IndicatorList *indicatorList = (IndicatorList*)pInvoke;
	indicatorList->OnIndicatorDataCallBack(message);
}

void IndicatorList::Invoke(void *sender, void *args, void *pInvoke)
{
	IndicatorList *indicatorList = (IndicatorList*)pInvoke;
	indicatorList->OnInvoke(args);
}

void IndicatorList::RegisterEvents(ControlA *control)
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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

IndicatorList::IndicatorList(NativeBase *native)
{
	m_chart = 0;
	m_div = 0;
	m_native = native;
	m_isSubmit = false;
	m_timerID = ControlA::GetNewTimerID();
	string xmlPath = DataCenter::GetAppPath() + "\\config\\IndicatorList.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowIE"));
    m_tvList = GetTree(L"tvList");
    m_gridSelectedRowsChangedEvent = &GridSelectedRowsChanged;
	m_tvList->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
    m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    RegisterEvents(m_window);
    m_timerEvent = &CallTimer;
	m_window->RegisterEvent(m_timerEvent, EVENTID::TIMER, this);
	m_indicatorService = DataCenter::GetIndicatorService();
	m_indicatorDataCallBackEvent = &IndicatorDataCallBack;
    m_indicatorService->RegisterListener(m_indicatorService->GetOperatorRequestID(), m_indicatorDataCallBackEvent, this);
}

IndicatorList::~IndicatorList()
{
    if (m_indicatorService)
    {
        m_indicatorService->UnRegisterListener(m_indicatorService->GetOperatorRequestID(), m_indicatorDataCallBackEvent);
		m_indicatorDataCallBackEvent = 0;
		m_indicatorService = 0;
    }
	m_chart = 0;
	m_div = 0;
	m_native = 0;
	if (m_tvList)
    {
		m_tvList->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_tvList = 0;
    }
    if (m_window)
    {
		m_window->UnRegisterEvent(m_invokeEvent, EVENTID::INVOKE);
		m_invokeEvent = 0;
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

OwChart* IndicatorList::GetChart()
{
	return m_chart;
}

void IndicatorList::SetChart(OwChart *chart)
{
	m_chart = chart;
}

CDiv* IndicatorList::GetDiv()
{
	return m_div;
}

void IndicatorList::SetDiv(CDiv *div)
{
	m_div = div;
}

bool IndicatorList::IsSubmit()
{
	return m_isSubmit;
}

WindowEx* IndicatorList::GetWindow()
{
	return m_window;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void IndicatorList::AddIndicatorsToTree(vector<Indicator> *indicators)
{
    map<String, TreeNodeA*> nodesMap = GetIndicatorsCategoryNodes();
	vector<Indicator>::iterator sIter = indicators->begin();
	int pos = 0;
	for(; sIter != indicators->end(); ++sIter)
	{
		Indicator indicator = *sIter;
		map<String, TreeNodeA*>::iterator sIter2 = nodesMap.find(indicator.m_category);
		if(sIter2 != nodesMap.end())
		{
			TreeNodeA *indicatorNode = new TreeNodeA;
            indicatorNode->SetGrid(m_tvList);
            indicatorNode->SetName(indicator.m_indicatorID);
            indicatorNode->SetText(indicator.m_description);
            TreeNodeA *parentNode = sIter2->second;
            parentNode->AppendNode(indicatorNode);
			if(pos == 0)
			{
				m_indicator = indicator;
                LoadIndicator(&indicator);
			}
			pos++;
		}
	}
    m_tvList->Update();
}

void IndicatorList::ChangeUsePassword()
{
    CheckBoxA *cbUsePassword = GetCheckBox(L"cbUsePassword");
    TextBoxA *txtPassword = GetTextBox(L"txtPassword");
    txtPassword->SetEnabled(!cbUsePassword->IsChecked());
    txtPassword->Invalidate();
}

void IndicatorList::Close()
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

void IndicatorList::Delete()
{
    vector<GridRow*> selectedRows = m_tvList->GetSelectedRows();
    int selectedRowsSize = (int)selectedRows.size();
    if (selectedRowsSize > 0)
    {
        TreeNodeA *node = dynamic_cast<TreeNodeA*>(selectedRows[0]->GetCell(0));
        if (node->GetChildNodes().size() == 0)
        {
            String indicatorID = node->GetName();
            Indicator indicator;
            m_indicatorService->GetIndicatorByID(indicatorID, &indicator);
            m_indicatorService->DeleteIndicator(m_indicatorService->GetOperatorRequestID(), &indicator);
        }
    }
}

map<String, TreeNodeA*> IndicatorList::GetIndicatorsCategoryNodes()
{
    map<String, TreeNodeA*> nodesMap;
    vector<TreeNodeA*> nodes = m_tvList->GetChildNodes();
	vector<TreeNodeA*>::iterator sIter = nodes.begin();
	for(; sIter != nodes.end(); ++sIter)
	{
        TreeNodeA *node = *sIter;
        nodesMap[node->GetText()] = node;
	}
    return nodesMap;
}

map<String, TreeNodeA*> IndicatorList::GetIndicatorsNodes()
{
    map<String, TreeNodeA*> indicatorNodesMap;
    map<String, TreeNodeA*> nodesMap = GetIndicatorsCategoryNodes();
	map<String, TreeNodeA*>::iterator sIter = nodesMap.begin();
	for(; sIter != nodesMap.end(); ++sIter)
	{
		vector<TreeNodeA*> childNodes = sIter->second->GetChildNodes();
		vector<TreeNodeA*>::iterator sIter2 = childNodes.begin();
		for(; sIter2 != childNodes.end(); ++sIter2)
		{
			TreeNodeA *childNode = *sIter2;
            indicatorNodesMap[childNode->GetName()] = childNode;
		}
	}
    return indicatorNodesMap;
}

void IndicatorList::GetIndicator(Indicator *indicator)
{
    TextBoxA *txtName = GetTextBox(L"txtName");
    CheckBoxA *cbUsePassword = GetCheckBox(L"cbUsePassword");
    TextBoxA *txtPassword = GetTextBox(L"txtPassword");
    TextBoxA *txtDescription = GetTextBox(L"txtDescription");
    ComboBoxA *cbCategory = GetComboBox(L"cbCategory");
    ComboBoxA *cbPaintType = GetComboBox(L"cbPaintType");
    ComboBoxA *cbDigit = GetComboBox(L"cbDigit");
    TextBoxA *txtVersion = GetTextBox(L"txtVersion");
    TextBoxA *txtCoordinate = GetTextBox(L"txtCoordinate");
    TextBoxA *txtSpecialCoordinate = GetTextBox(L"txtSpecialCoordinate");
    TextBoxA *txtText = GetTextBox(L"txtText");
	wchar_t strName[20] = {0};
	String parameters = L"";
    for (int i = 1; i <= 16; i++)
    {
		_stprintf_s(strName, 19, L"txtPN%d", i);
        TextBoxA *txtPN = GetTextBox(strName);
        if (txtPN->GetText().length() > 0)
        {
			_stprintf_s(strName, 19, L"txtP%d", i);
			TextBoxA *txtP = GetTextBox(strName);
			_stprintf_s(strName, 19, L"txtPMin%d", i);
			TextBoxA *txtPMin = GetTextBox(strName);
			_stprintf_s(strName, 19, L"txtPMax%d", i);
			TextBoxA *txtPMax = GetTextBox(strName);
            parameters += txtPN->GetText() + L"," + txtPMin->GetText() + L"," + txtPMax->GetText() + L"," + txtP->GetText() + L";";
        }
    }
    if (indicator->m_indicatorID.length() == 0)
    {
        indicator->m_indicatorID = m_indicator.m_indicatorID;
    }
    indicator->m_name = txtName->GetText();
    indicator->m_usePassword = cbUsePassword->IsChecked() ? 1 : 0;
    if (indicator->m_usePassword == 1)
    {
        indicator->m_password = txtPassword->GetText();
    }
    else
    {
        indicator->m_password = L"";
    }
    indicator->m_description = txtDescription->GetText();
    indicator->m_category = cbCategory->GetSelectedText();
    indicator->m_paintType = cbPaintType->GetSelectedIndex();
    indicator->m_digit = cbDigit->GetSelectedIndex();
	indicator->m_version = CStr::ConvertStrToInt(txtVersion->GetText().c_str());
    indicator->m_coordinate = txtCoordinate->GetText();
    indicator->m_specialCoordinate = txtSpecialCoordinate->GetText();
    indicator->m_text = txtText->GetText();
    indicator->m_parameters = parameters;
    indicator->m_type = 1;
	indicator->m_userID = DataCenter::GetUserID();
}

void IndicatorList::LoadIndicator(Indicator *indicator)
{
    TextBoxA *txtName = GetTextBox(L"txtName");
    CheckBoxA *cbUsePassword = GetCheckBox(L"cbUsePassword");
    TextBoxA *txtPassword = GetTextBox(L"txtPassword");
    TextBoxA *txtDescription = GetTextBox(L"txtDescription");
    ComboBoxA *cbCategory = GetComboBox(L"cbCategory");
    ComboBoxA *cbPaintType = GetComboBox(L"cbPaintType");
    ComboBoxA *cbDigit = GetComboBox(L"cbDigit");
    TextBoxA *txtVersion = GetTextBox(L"txtVersion");
    TextBoxA *txtCoordinate = GetTextBox(L"txtCoordinate");
    TextBoxA *txtSpecialCoordinate = GetTextBox(L"txtSpecialCoordinate");
    TextBoxA *txtText = GetTextBox(L"txtText");
    ButtonA *btnDelete = GetButton(L"btnDelete");
    ButtonA *btnSave = GetButton(L"btnSave");
    bool allowEdit = indicator->m_type == 1;
    int strSize = 0;
    vector<String> strs;
    if (indicator->m_parameters.length() > 0)
    {
		strs = CStr::Split(indicator->m_parameters, L";");
        strSize = (int)strs.size();
    }
	wchar_t strName[20] = {0};
    for (int i = 1; i <= 16; i++)
    {
		_stprintf_s(strName, 19, L"txtPN%d", i);
        TextBoxA *txtPN = GetTextBox(strName);
		_stprintf_s(strName, 19, L"txtP%d", i);
        TextBoxA *txtP = GetTextBox(strName);
		_stprintf_s(strName, 19, L"txtPMin%d", i);
        TextBoxA *txtPMin = GetTextBox(strName);
		_stprintf_s(strName, 19, L"txtPMax%d", i);
        TextBoxA *txtPMax = GetTextBox(strName);
        txtPN->SetEnabled(allowEdit);
        txtP->SetEnabled(allowEdit);
        txtPMin->SetEnabled(allowEdit);
        txtPMax->SetEnabled(allowEdit);
        if (i <= strSize)
        {
            if (strs[i - 1].length() > 0)
            {
				vector<String> subStrs = CStr::Split(strs[i - 1], L",");
                txtPN->SetText(subStrs[0]);
                txtPMin->SetText(subStrs[1]);
                txtPMax->SetText(subStrs[2]);
                txtP->SetText(subStrs[3]);
            }
        }
        else
        {
            txtPN->SetText(L"");
            txtP->SetText(L"");
            txtPMin->SetText(L"");
            txtPMax->SetText(L"");
        }
    }
    txtName->SetText(indicator->m_name);
	txtName->SetReadOnly(!allowEdit);
    cbUsePassword->SetChecked(indicator->m_usePassword == 1);
    cbUsePassword->SetEnabled(allowEdit);
    txtPassword->SetEnabled(cbUsePassword->IsChecked());
    txtPassword->SetText(indicator->m_password);
    txtPassword->SetReadOnly(!allowEdit);
    txtDescription->SetText(indicator->m_description);
    txtDescription->SetReadOnly(!allowEdit);
    cbCategory->SetSelectedText(indicator->m_category);
    cbCategory->SetEnabled(allowEdit);
    cbPaintType->SetSelectedIndex(indicator->m_paintType);
    cbPaintType->SetEnabled(allowEdit);
    cbDigit->SetSelectedIndex(indicator->m_digit);
    cbDigit->SetEnabled(allowEdit);
	_stprintf_s(strName, 19, L"%d", indicator->m_version);
    txtVersion->SetText(strName);
    txtVersion->SetReadOnly(!allowEdit);
    txtCoordinate->SetText(indicator->m_coordinate);
    txtCoordinate->SetReadOnly(!allowEdit);
    txtSpecialCoordinate->SetText(indicator->m_specialCoordinate);
    txtSpecialCoordinate->SetReadOnly(!allowEdit);
    txtText->SetText(indicator->m_text);
    txtText->SetReadOnly(!allowEdit);
    btnDelete->SetEnabled(allowEdit);
    btnSave->SetEnabled(allowEdit);
}

void IndicatorList::OnGridSelectedRowsChanged()
{
	if(m_tvList)
	{
		String indicatorID = L"";
		vector<GridRow*> selectedRows = m_tvList->GetSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		if (selectedRowsSize > 0)
		{
			TreeNodeA *node = dynamic_cast<TreeNodeA*>(selectedRows[0]->GetCell(0));
			indicatorID = node->GetName();
		}
		Indicator indicator;
		m_indicatorService->GetIndicatorByID(indicatorID, &indicator);
		m_indicator = indicator;
		LoadIndicator(&m_indicator);
		m_window->Invalidate();
	}
}

void IndicatorList::OnIndicatorDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void IndicatorList::OnInvoke(void *args)
{
    CMessage *message = (CMessage*)args;
    vector<Indicator> indicators;
	IndicatorService::GetIndicators(&indicators, message->m_body, message->m_bodyLength);
    switch (message->m_functionID)
    {
        case FUNCTIONID_INDICATOR_ADDINDICATORS:
            AddIndicatorsToTree(&indicators);
            break;
        case FUNCTIONID_INDICATOR_DELETEINDICATORS:
            {
                map<String, TreeNodeA*> indicatorNodes = GetIndicatorsNodes();
				Indicator tempIndicator;
                m_indicator = tempIndicator;
				vector<Indicator>::iterator sIter = indicators.begin();
				for(; sIter != indicators.end(); ++sIter)
				{
                    Indicator indicator = *sIter;
					map<String, TreeNodeA*>::iterator sIter2 = indicatorNodes.find(indicator.m_indicatorID);
					if(sIter2 != indicatorNodes.end())
					{
                        m_tvList->RemoveNode(sIter2->second);
					}
				}
                break;
            }
        case FUNCTIONID_INDICATOR_UPDATEINDICATORS:
            {
				map<String, TreeNodeA*> indicatorNodes = GetIndicatorsNodes();
				vector<Indicator>::iterator sIter = indicators.begin();
				for(; sIter != indicators.end(); ++sIter)
				{
                    Indicator indicator = *sIter;
					map<String, TreeNodeA*>::iterator sIter2 = indicatorNodes.find(indicator.m_indicatorID);
					if(sIter2 != indicatorNodes.end())
					{
						sIter2->second->SetText(indicator.m_description);
					}
				}
                break;
            }
    }
    m_window->Invalidate();
}

void IndicatorList::OnTimer(int timerID)
{
	if (m_timerID == timerID)
    {
        POINT mp = m_window->GetMousePoint();
        int height = m_window->GetHeight();
        if (mp.y >= 0 && mp.y <= height)
        {
            bool paint = false;
			int left = m_tvList->GetLeft(), right = m_tvList->GetRight();
            if (mp.x < right + 5)
            {
                if (left < 0)
                {
                    m_tvList->SetLeft(left + abs(left) * 2 / 3);
                    paint = true;
                }
            }
            else if (mp.x > right + 100)
            {
                if (left > -195)
                {
                    m_tvList->SetLeft(left - (195 - abs(left)) * 2 / 3);
                    paint = true;
                }
            }
            if (paint)
            {
                m_window->Invalidate();
            }
        }
    }
}

void IndicatorList::Save(bool saveAsNew)
{
    Indicator indicator;
    GetIndicator(&indicator);
    if ((indicator.m_indicatorID.length() == 0) || saveAsNew)
    {
        m_indicatorService->AddIndicator(m_indicatorService->GetOperatorRequestID(), &indicator);
    }
    else
    {
        m_indicatorService->UpdateIndicator(m_indicatorService->GetOperatorRequestID(), &indicator);
    }
}

void IndicatorList::Show()
{
	vector<Indicator> indicators;
    m_indicatorService->GetIndicators(&indicators);
    AddIndicatorsToTree(&indicators);
    m_tvList->Update();
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(true);
    m_window->Invalidate();
	m_window->StartTimer(m_timerID, 10);
}

void IndicatorList::Submit()
{
    if (m_div)
    {
        Indicator indicator;
        GetIndicator(&indicator);
		if (indicator.m_indicatorID.length() > 0)
        {
            if (indicator.m_paintType == 0)
            {
                m_chart->AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, m_div, true);
            }
            else if (indicator.m_paintType == 1)
            {
                m_chart->AddViceIndicator(indicator.m_name, indicator.m_text, indicator.m_parameters, m_div, true);
            }
        }
    }
    else
    {
        m_isSubmit = true;
        Close();
    }
}
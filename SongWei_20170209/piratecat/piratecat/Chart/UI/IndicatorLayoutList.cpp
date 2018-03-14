#include "..\\..\\stdafx.h"
#include "IndicatorLayoutList.h"

void IndicatorLayoutList::AddLayoutsToGrid(vector<IndicatorLayout> *layouts)
{
    m_gridLayouts->BeginUpdate();
	vector<IndicatorLayout>::iterator sIter = layouts->begin();
	for(; sIter != layouts->end(); ++sIter)
	{
        IndicatorLayout layout = *sIter;
        GridRow *row = new GridRow;
        row->SetGrid(m_gridLayouts);
        GridStringCell *idCell = new GridStringCell;
        idCell->SetGrid(m_gridLayouts);
        idCell->SetColumn(m_gridLayouts->GetColumn(0));
        idCell->SetText(layout.m_layoutID);
        row->AddCell(0, idCell);
        GridStringCell *textCell = new GridStringCell;
        textCell->SetGrid(m_gridLayouts);
        textCell->SetColumn(m_gridLayouts->GetColumn(1));
        textCell->SetText(layout.m_name);
        row->AddCell(1, textCell);
        m_gridLayouts->AddRow(row);
	}
    m_gridLayouts->EndUpdate();
}

void IndicatorLayoutList::Apply()
{
	String layoutID = L"";
	vector<GridRow*> selectedRows = m_gridLayouts->GetSelectedRows();
	int selectedRowsSize = (int)selectedRows.size();
	if (selectedRowsSize > 0)
	{
		layoutID = selectedRows[0]->GetCell(0)->GetText();
	}
    IndicatorLayout indicatorLayout;
    if (m_indicatorLayoutService->GetLayoutByID(layoutID, &indicatorLayout))
    {
		m_chart->SetIndicatorLayout(indicatorLayout.m_layoutID, indicatorLayout.m_text);
    }
}

void IndicatorLayoutList::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		IndicatorLayoutList *indicatorLayoutList = (IndicatorLayoutList*)pInvoke;
		if(name == L"btnApply")
		{
			indicatorLayoutList->Apply();
		}
        else if (name == L"btnClose")
        {
            indicatorLayoutList->Close();
        }
		else if (name == L"btnCreate")
        {
            indicatorLayoutList->Create();
        }
        else if (name == L"btnDelete")
        {
            indicatorLayoutList->Delete();
        }
        else if (name == L"btnSave")
        {
            indicatorLayoutList->Save();
        }
    }
}

map<String, GridRow*> IndicatorLayoutList::GetIndicatorLayoutsRows()
{
    map<String, GridRow*> ilRowsMap;
    vector<GridRow*> rows = m_gridLayouts->m_rows;
	vector<GridRow*>::iterator sIter = rows.begin();
	for(; sIter != rows.end(); ++sIter)
	{
		GridRow *row = *sIter;
        ilRowsMap[row->GetCell(0)->GetText()] = row;
	}
    return ilRowsMap;
}

void IndicatorLayoutList::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	IndicatorLayoutList *indicatorLayoutList = (IndicatorLayoutList*)pInvoke;
	indicatorLayoutList->OnGridSelectedRowsChanged();
}

void IndicatorLayoutList::IndicatorLayoutDataCallBack(CMessage *message, void *pInvoke)
{
	IndicatorLayoutList *indicatorLayoutList = (IndicatorLayoutList*)pInvoke;
	indicatorLayoutList->OnIndicatorLayoutDataCallBack(message);
}

void IndicatorLayoutList::Invoke(void *sender, void *args, void *pInvoke)
{
	IndicatorLayoutList *indicatorLayoutList = (IndicatorLayoutList*)pInvoke;
	indicatorLayoutList->OnInvoke(args);
}

void IndicatorLayoutList::LoadLayout(IndicatorLayout *layout)
{
    TextBoxA *txtLayoutName = GetTextBox(L"txtLayoutName");
    LayoutScaleDiv *layoutScaleDiv = dynamic_cast<LayoutScaleDiv*>(FindControl(L"divLayoutScale"));
    ButtonA *btnDelete = GetButton(L"btnDelete");
    ButtonA *btnApply = GetButton(L"btnApply");
    ButtonA *btnCreate = GetButton(L"btnCreate");
    ButtonA *btnSave = GetButton(L"btnSave");
    txtLayoutName->SetText(layout->m_name);
    layoutScaleDiv->SetLayoutStr(layout->m_text);
	bool btnEnabled = layout->m_layoutID.length() > 0;
    btnDelete->SetEnabled(btnEnabled);
    btnApply->SetEnabled(btnEnabled);
    btnSave->SetEnabled(btnEnabled);
}

void IndicatorLayoutList::RegisterEvents(ControlA *control)
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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

IndicatorLayoutList::IndicatorLayoutList(NativeBase *native)
{
	m_chart = 0;
	m_native = native;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\IndicatorLayoutList.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowILL"));
    m_gridLayouts = GetGrid(L"gridLayouts");
    m_gridSelectedRowsChangedEvent = &GridSelectedRowsChanged;
	m_gridLayouts->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
    m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    RegisterEvents(m_window);
	m_indicatorLayoutService = DataCenter::GetIndicatorLayoutService();
	m_indicatorLayoutDataCallBack = &IndicatorLayoutDataCallBack;
    m_indicatorLayoutService->RegisterListener(m_indicatorLayoutService->GetOperatorRequestID(), m_indicatorLayoutDataCallBack, this);
}

IndicatorLayoutList::~IndicatorLayoutList()
{
	m_chart = 0;
    if (m_gridLayouts)
    {
		m_gridLayouts->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_gridLayouts = 0;
    }
	m_native = 0;
    if (m_indicatorLayoutService)
    {
        m_indicatorLayoutService->UnRegisterListener(m_indicatorLayoutService->GetOperatorRequestID(), m_indicatorLayoutDataCallBack);
		m_indicatorLayoutDataCallBack = 0;
		m_indicatorLayoutService = 0;
    }
	if (m_window)
    {
		m_window->UnRegisterEvent(m_invokeEvent, EVENTID::INVOKE);
        m_invokeEvent = 0;
		if(m_window->GetParent())
		{
			m_window->Close();
		}
        delete m_window;
        m_window = 0;
    }
}

OwChart* IndicatorLayoutList::GetChart()
{
	return m_chart;
}

void IndicatorLayoutList::SetChart(OwChart *chart)
{
	m_chart = chart;
}

WindowEx* IndicatorLayoutList::GetWindow()
{
	return m_window;
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void IndicatorLayoutList::Close()
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

void IndicatorLayoutList::Create()
{
    IndicatorLayout layout;
    layout.m_name = L"新的布局";
    layout.m_text = m_chart->GetIndicatorLayout();
	layout.m_userID = DataCenter::GetUserID();
    m_indicatorLayoutService->AddLayout(m_indicatorLayoutService->GetOperatorRequestID(), &layout);
}

void IndicatorLayoutList::Delete()
{
    vector<GridRow*> selectedRows = m_gridLayouts->GetSelectedRows();
    int selectedRowsSize = (int)selectedRows.size();
    if (selectedRowsSize > 0)
    {
        String layoutID = selectedRows[0]->GetCell(0)->GetText();
        IndicatorLayout layout;
        m_indicatorLayoutService->GetLayoutByID(layoutID, &layout);
        m_indicatorLayoutService->DeleteLayout(m_indicatorLayoutService->GetOperatorRequestID(), &layout);
    }
}

void IndicatorLayoutList::GetLayout(IndicatorLayout *layout)
{
    TextBoxA *txtLayoutName = GetTextBox(L"txtLayoutName");
    LayoutScaleDiv *layoutScaleDiv = dynamic_cast<LayoutScaleDiv*>(FindControl(L"divLayoutScale"));
    if (m_layout.m_layoutID.length() > 0)
    {
        layout->m_layoutID = m_layout.m_layoutID;
    }
    layout->m_name = txtLayoutName->GetText();
    layout->m_text = layoutScaleDiv->GetLayoutStr();
	layout->m_userID = DataCenter::GetUserID();
}

void IndicatorLayoutList::OnGridSelectedRowsChanged()
{
	if(m_gridLayouts)
	{
		String layoutID = L"";
		vector<GridRow*> selectedRows = m_gridLayouts->GetSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		if (selectedRowsSize > 0)
		{
			layoutID = selectedRows[0]->GetCell(0)->GetText();
		}
		IndicatorLayout layout;
		m_indicatorLayoutService->GetLayoutByID(layoutID, &layout);
		m_layout = layout;
		LoadLayout(&m_layout);
		m_window->Invalidate();
	}
}

void IndicatorLayoutList::OnInvoke(void *args)
{
    CMessage *message = (CMessage*)args;
    vector<IndicatorLayout> layouts;
	IndicatorLayoutService::GetLayouts(&layouts, message->m_body, message->m_bodyLength);
    switch (message->m_functionID)
    {
        case FUNCTIONID_INDICATORLAYOUT_ADDLAYOUTS:
            AddLayoutsToGrid(&layouts);
            break;
        case FUNCTIONID_INDICATORLAYOUT_DELETELAYOUTS:
            {
                map<String, GridRow*> ilRowsMap = GetIndicatorLayoutsRows();
				vector<IndicatorLayout>::iterator sIter = layouts.begin();
				for(; sIter != layouts.end(); ++sIter)
				{
					IndicatorLayout layout = *sIter;
					map<String, GridRow*>::iterator sIter2 = ilRowsMap.find(layout.m_layoutID);
					if(sIter2 != ilRowsMap.end())
					{
						m_gridLayouts->RemoveRow(sIter2->second);
						delete sIter2->second;
					}
				}
				m_gridLayouts->Update();
                break;
            }
        case FUNCTIONID_INDICATORLAYOUT_UPDATELAYOUTS:
            {
                map<String, GridRow*> ilRowsMap = GetIndicatorLayoutsRows();
				vector<IndicatorLayout>::iterator sIter = layouts.begin();
				for(; sIter != layouts.end(); ++sIter)
				{
					IndicatorLayout layout = *sIter;
					map<String, GridRow*>::iterator sIter2 = ilRowsMap.find(layout.m_layoutID);
					if(sIter2 != ilRowsMap.end())
					{
						sIter2->second->GetCell(1)->SetText(layout.m_name);
					}
				}
                break;
            }
    }
    m_window->Invalidate();
}

void IndicatorLayoutList::OnIndicatorLayoutDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void IndicatorLayoutList::Save()
{
    IndicatorLayout layout;
    GetLayout(&layout);
    if (layout.m_layoutID.length() > 0)
    {
        m_indicatorLayoutService->UpdateLayout(m_indicatorLayoutService->GetOperatorRequestID(), &layout);
    }
}

void IndicatorLayoutList::Show()
{
	vector<IndicatorLayout> layouts;
    m_indicatorLayoutService->GetLayouts(&layouts);
    AddLayoutsToGrid(&layouts);
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(false);
    m_window->Invalidate();
}
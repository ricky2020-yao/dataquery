#include "..\\..\\stdafx.h"
#include "SecurityList.h"

void SecurityList::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		SecurityList *securityList = (SecurityList*)pInvoke;
		if (name == L"btnCancel" || name == L"btnClose")
		{
			securityList->Close();
		}
		else if (name == L"btnSelectSecurities")
		{
			securityList->SelectSecurities();
		}
		else if (name == L"btnSelectAllSecurities")
		{
			securityList->SelectAllSecurities();
		}
		else if (name == L"btnSubmit")
		{
			securityList->Submit();
		}
		else if (name == L"btnUnSelectSecurities")
		{
			securityList->UnSelectSecurities();
		}
		else if (name == L"btnUnSelectAllSecurities")
		{
			securityList->UnSelectAllSecurities();
		}
	}
}

void SecurityList::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	SecurityList *securityList = (SecurityList*)pInvoke;
	securityList->OnGridSelectedRowsChanged();
}

void SecurityList::Invoke(void *sender, void *args, void *pInvoke)
{
	SecurityList *securityList = (SecurityList*)pInvoke;
	securityList->OnInvoke(args);
}

void SecurityList::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ControlA *subControl = *sIter;
        GridColumn *column = dynamic_cast<GridColumn*>(subControl);
        ButtonA *button = dynamic_cast<ButtonA*>(subControl);
		GridA *grid = dynamic_cast<GridA*>(subControl);
        if (column)
        {
            column->SetAllowResize(true);
			column->SetBackColor(PCOLORS_BACKCOLOR);
			column->SetBorderColor(PCOLORS_LINECOLOR3);
            column->SetForeColor(PCOLORS_FORECOLOR);
        }
        else if (button)
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
        RegisterEvents(subControl);
	}
}

void SecurityList::SecurityDataCallBack(CMessage *message, void *pInvoke)
{
	SecurityList *securityList = (SecurityList*)pInvoke;
	securityList->OnSecurityDataCallBack(message);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityList::SecurityList(NativeBase *native)
{
	m_isSubmit = false;
	m_native = native;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\SecurityList.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowSecurity"));
	m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    RegisterEvents(m_window);
    m_gridSecurities = GetGrid(L"gridSecurities");
    m_gridSelectSecurities = GetGrid(L"gridSelectSecurities");
	m_tvBlock = GetTree(L"tvBlock");
	m_gridSelectedRowsChangedEvent = &GridSelectedRowsChanged;
	m_tvBlock->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
	m_securityService = DataCenter::GetSecurityService();
	m_securityDataCallBack = &SecurityDataCallBack;
	m_securitiesRequestID = BaseService::GetRequestID();
    m_securityService->RegisterListener(m_securitiesRequestID, m_securityDataCallBack, this);
	m_userSecurityService = DataCenter::GetUserSecurityService();
}

SecurityList::~SecurityList()
{
	m_gridSecurities = 0;
	m_gridSelectSecurities = 0;
	m_native = 0;
	if(m_securityService)
	{
		m_securityService->UnRegisterListener(m_securitiesRequestID, m_securityDataCallBack);
		m_securityDataCallBack = 0;
		m_securityService = 0;
	}
	if(m_tvBlock)
	{
		m_tvBlock->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_tvBlock = 0;
	}
	m_userSecurityService = 0;
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

bool SecurityList::IsSubmit()
{
	return m_isSubmit;
}

WindowEx* SecurityList::GetWindow()
{
	return m_window;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityList::AddSecuritiesToSecuritiesGrid(vector<Security> *securities)
{
    map<String, String> existsCodes;
    vector<GridRow*> rows = m_gridSecurities->m_rows;
	vector<GridRow*>::iterator sIter = rows.begin();
	for(; sIter != rows.end(); ++sIter)
	{
        GridRow *row = *sIter;
        String code = row->GetCell(0)->GetString();
        existsCodes[code] = L"";
	}
	m_gridSecurities->BeginUpdate();
	vector<Security>::iterator sIter2 = securities->begin();
	for(; sIter2 != securities->end(); ++sIter2)
	{
		Security security = *sIter2;
		map<String, String>::iterator sIter3 = existsCodes.find(security.m_code);
        if (sIter3 == existsCodes.end())
        {
            GridRow *row = new GridRow();
            m_gridSecurities->AddRow(row);
            row->AddCell(0, new GridStringCell(security.m_code));
            row->AddCell(1, new GridStringCell(security.m_name));
            existsCodes[security.m_code] = L"";
        }
	}
    m_gridSecurities->EndUpdate();
    m_gridSecurities->Invalidate();
}

void SecurityList::BindSecuritiesToSelectSecuritiesGrid(vector<Security> *securities)
{
    m_gridSelectSecurities->BeginUpdate();
	m_gridSelectSecurities->ClearRows();
	vector<Security>::iterator sIter = securities->begin();
	for(; sIter != securities->end(); ++sIter)
	{
        Security security = *sIter;
        GridRow *row = new GridRow();
        m_gridSelectSecurities->AddRow(row);
        row->AddCell(0, new GridStringCell(security.m_code));
        row->AddCell(1, new GridStringCell(security.m_name));
	}
    m_gridSelectSecurities->EndUpdate();
    m_gridSelectSecurities->Invalidate();
}

void SecurityList::Close()
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

void SecurityList::GetSelectedSecurities(vector<Security> *securities)
{
	if(m_gridSecurities)
	{
		vector<GridRow*> rows = m_gridSecurities->m_rows;
		vector<GridRow*>::iterator sIter = rows.begin();
		for(; sIter != rows.end(); ++sIter)
		{
			GridRow *row = *sIter;
			Security security;
			if(m_securityService->GetSecurityByCode(row->GetCell(0)->GetString(), &security))
			{
				securities->push_back(security);
			}
		}
	}
}

void SecurityList::LoadUserSecurityCategories(vector<UserSecurityCategory> *categories)
{
	m_tvBlock->BeginUpdate();
	vector<TreeNodeA*> childNodes = m_tvBlock->GetChildNodes();
	int childNodesSize = (int)childNodes.size();
	if(childNodesSize > 0)
	{
		TreeNodeA *userSecurityNode = childNodes[childNodesSize - 1];
		vector<UserSecurityCategory>::iterator sIter = categories->begin();
		for(; sIter != categories->end(); ++sIter)
		{
			UserSecurityCategory category = *sIter;
			TreeNodeA *categoryNode = new TreeNodeA;
			userSecurityNode->AppendNode(categoryNode);
			categoryNode->SetText(category.m_name);
			categoryNode->SetValue(category.m_categoryID);
		}
	}
	m_tvBlock->EndUpdate();
}

void SecurityList::OnGridSelectedRowsChanged()
{
	if(m_tvBlock)
	{
		vector<GridRow*> selectedRows = m_tvBlock->GetSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		if (selectedRowsSize > 0)
		{
			TreeNodeA *node = dynamic_cast<TreeNodeA*>(selectedRows[0]->GetCell(0));
			TreeNodeA *parentNode = node->GetParent();
            if (parentNode && parentNode->GetText() == L"×ÔÑ¡¹É")
            {
                String categoryID = node->GetValue();
                UserSecurityCategory category;
                m_userSecurityService->GetCategory(categoryID, &category);
                vector<Security> securities;
                m_securityService->GetSecuritiesByCodes(category.m_codes, &securities);
                BindSecuritiesToSelectSecuritiesGrid(&securities);
            }
            else
            {
				if (node->GetChildNodes().size() == 0)
                {
                    m_securityService->GetSecuritiesByBlockID(m_securitiesRequestID, node->GetValue());
                }
            }
		}
	}
}

void SecurityList::OnInvoke(void *args)
{
	CMessage *message = (CMessage*)args;
	vector<Security> securities;
	SecurityService::GetSecurities(&securities, message->m_functionID, message->m_body, message->m_bodyLength);
	int securitiesSize = (int)securities.size();
	for(int i = 0; i < securitiesSize; i++)
	{
		Security security;
		m_securityService->GetSecurityByCode(securities[i].m_code, &security);
		securities[i] = security;
	}
    BindSecuritiesToSelectSecuritiesGrid(&securities);
    securities.clear();
}

void SecurityList::OnSecurityDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void SecurityList::RemoveSecuritiesFromSecuritiesGrid(vector<Security> *securities)
{
    map<String, Security> existsCodes;
	vector<Security>::iterator sIter = securities->begin();
	for(; sIter != securities->end(); ++sIter)
	{
		Security security = *sIter;
        existsCodes[security.m_code] = security;
	}
	int rowSize = (int)m_gridSecurities->m_rows.size();
    m_gridSecurities->BeginUpdate();
    for (int i = 0; i < rowSize; i++)
    {
		GridRow *row = m_gridSecurities->GetRow(i);
        Security security;
        if(m_securityService->GetSecurityByCode(row->GetCell(0)->GetString(), &security))
		{
			map<String, Security>::iterator sIter2 = existsCodes.find(security.m_code);
			if(sIter2 != existsCodes.end())
			{
				m_gridSecurities->RemoveRow(row);
				delete row;
				i--;
				rowSize--;
			}
		}
    }
    m_gridSecurities->EndUpdate();
    m_gridSecurities->Invalidate();
}

void SecurityList::SelectSecurities()
{
    vector<GridRow*> selectedRows = m_gridSelectSecurities->GetSelectedRows();
    int selectedRowsSize = (int)selectedRows.size();
    if (selectedRowsSize > 0)
    {
		vector<Security> securities;
		vector<GridRow*>::iterator sIter = selectedRows.begin();
		for(; sIter != selectedRows.end(); ++sIter)
		{
            GridRow *row = *sIter;
            Security security;
            if(m_securityService->GetSecurityByCode(row->GetCell(0)->GetString(), &security))
			{
				securities.push_back(security);
			}
		}
        AddSecuritiesToSecuritiesGrid(&securities);
    }
}

void SecurityList::SelectAllSecurities()
{
    vector<GridRow*> rows = m_gridSelectSecurities->m_rows;
    int rowSize = (int)rows.size();
    if (rowSize > 0)
    {
		vector<Security> securities;
		vector<GridRow*>::iterator sIter = rows.begin();
		for(; sIter != rows.end(); ++sIter)
		{
            GridRow *row = *sIter;
            Security security;
            if(m_securityService->GetSecurityByCode(row->GetCell(0)->GetString(), &security))
			{
				securities.push_back(security);
			}
		}
        AddSecuritiesToSecuritiesGrid(&securities);
    }
}

void SecurityList::Show()
{
	vector<UserSecurityCategory> categories;
    m_userSecurityService->GetCategories(&categories);
    LoadUserSecurityCategories(&categories);
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(true);
    m_window->Invalidate();
}

void SecurityList::Submit()
{
    m_isSubmit = true;
	Close();
}

void SecurityList::UnSelectAllSecurities()
{
    vector<GridRow*> rows = m_gridSecurities->m_rows;
    int rowSize = (int)rows.size();
    if (rowSize > 0)
    {
		vector<Security> securities;
		vector<GridRow*>::iterator sIter = rows.begin();
		for(; sIter != rows.end(); ++sIter)
		{
			GridRow *row = *sIter;
            Security security;
            if(m_securityService->GetSecurityByCode(row->GetCell(0)->GetString(), &security))
			{
				securities.push_back(security);
			}
		}
        RemoveSecuritiesFromSecuritiesGrid(&securities);
    }
}

void SecurityList::UnSelectSecurities()
{
    vector<GridRow*> selectedRows = m_gridSecurities->GetSelectedRows();
	int selectedRowsSize = (int)selectedRows.size();
	if(selectedRowsSize > 0)
	{
		vector<Security> securities;
		vector<GridRow*>::iterator sIter = selectedRows.begin();
		for(; sIter != selectedRows.end(); ++sIter)
		{
			GridRow *row = *sIter;
            Security security;
            if(m_securityService->GetSecurityByCode(row->GetCell(0)->GetString(), &security))
			{
				securities.push_back(security);
			}
		}
		RemoveSecuritiesFromSecuritiesGrid(&securities);
	}
}
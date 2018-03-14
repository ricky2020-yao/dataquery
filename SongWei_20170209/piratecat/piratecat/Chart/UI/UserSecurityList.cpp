#include "..\\..\\stdafx.h"
#include "UserSecurityList.h"

void UserSecurityList::CallTimer(void *sender, int timerID, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnTimer(timerID);
}

void UserSecurityList::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    if (button == MouseButtonsA_Left && clicks == 1)
    {
        ControlA *control = (ControlA*)sender;
        String name = control->GetName();
		UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
        if (name == L"btnAddCategory")
        {
            userSecurityList->AddCategory();
        }
        else if (name == L"btnAddSecurities")
        {
            userSecurityList->AddSecurities();
        }
        else if (name == L"btnClose")
        {
            userSecurityList->Close();
        }
        else if (name == L"btnDeleteCategory")
        {
            userSecurityList->DeleteCategory();
        }
        else if (name == L"btnDeleteSecurities")
        {
            userSecurityList->DeleteSecurities();
        }
    }
}

void UserSecurityList::CategoryGridCellEditEnd(void *sender, GridCell *cell, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnCategoryGridCellEditEnd(cell);
}

void UserSecurityList::GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
    if (button == MouseButtonsA_Left && clicks == 2)
    {
        userSecurityList->OnGridCellDoubleClick(cell);
    }
}

void UserSecurityList::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnGridSelectedRowsChanged();
}

void UserSecurityList::Invoke(void *sender, void *args, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnInvoke(args);
}

void UserSecurityList::LatestDataCallBack(CMessage *message, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnLatestDataCallBack(message);
}

void UserSecurityList::RegisterEvents(ControlA *control)
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
        else if(button)
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

void UserSecurityList::SearchTextBoxGotFocus(void *sender, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnSearchTextBoxGotFocus();
}

void UserSecurityList::SecurityListWindowClosed(void *sender, bool *cancel, void *pInvoke)
{
    ControlA *control = (ControlA*)sender;
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	SecurityList *securityList = (SecurityList*)control->GetTag();
    userSecurityList->OnSecurityListWindowClosed(securityList, cancel);
}

void UserSecurityList::UserSecurityDataCallBack(CMessage *message, void *pInvoke)
{
	UserSecurityList *userSecurityList = (UserSecurityList*)pInvoke;
	userSecurityList->OnUserSecurityDataCallBack(message);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

UserSecurityList::UserSecurityList(NativeBase *native)
{
	m_chart = 0;
	m_native = native;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\UserSecurityList.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowUS"));
    m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    m_timerEvent = &CallTimer;
	m_window->RegisterEvent(m_timerEvent, EVENTID::TIMER, this);
    RegisterEvents(m_window);
    m_gridCategory = GetGrid(L"gridCategory");
    m_gridSelectedRowsChangedEvent =&GridSelectedRowsChanged;
	m_gridCategory->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
    m_gridCellEditEndEvent = &CategoryGridCellEditEnd;
	m_gridCategory->RegisterEvent(m_gridCellEditEndEvent, EVENTID::GRIDCELLEDITEND, this);
    m_gridSecurities = GetGrid(L"gridSecurities");
	m_gridCellClick = &GridCellClick;
	m_gridSecurities->RegisterEvent(m_gridCellClick, EVENTID::GRIDCELLCLICK, this);
	m_securityService = DataCenter::GetSecurityService();
	m_userSecurityService = DataCenter::GetUserSecurityService();
	m_userSecurityDataCallBack = &UserSecurityDataCallBack;
    m_userSecurityService->RegisterListener(m_userSecurityService->GetOperatorRequestID(), m_userSecurityDataCallBack, this);
	m_quoteService = DataCenter::GetQuoteService();
	m_latestDataCallBack = &LatestDataCallBack;
	m_latestDataRequestID = BaseService::GetRequestID();
	m_quoteService->RegisterListener(m_latestDataRequestID, m_latestDataCallBack, this);
	m_txtSearch = GetTextBox(L"txtSearch");
	m_searchTextBoxGotFocus = &SearchTextBoxGotFocus;
	m_txtSearch->RegisterEvent(m_searchTextBoxGotFocus, EVENTID::GOTFOCUS, this);
}

UserSecurityList::~UserSecurityList()
{
	m_chart = 0;
	if(m_gridCategory)
	{
		m_gridCategory->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_gridCategory->UnRegisterEvent(m_gridCellEditEndEvent, EVENTID::GRIDCELLEDITEND);
		m_gridCellEditEndEvent = 0;
		m_gridCategory = 0;
	}
    if (m_gridSecurities)
    {
		m_gridSecurities->UnRegisterEvent(m_gridCellClick, EVENTID::GRIDCELLCLICK);
        m_gridCellClick = 0;
        m_gridSecurities = 0;
    }
	m_gridSecurities = 0;
	m_native = 0;
	m_securityService = 0;
	if(m_txtSearch)
	{
		m_txtSearch->UnRegisterEvent(m_searchTextBoxGotFocus, EVENTID::GOTFOCUS);
		m_searchTextBoxGotFocus = 0;
		m_txtSearch = 0;
	}
	if(m_quoteService)
	{
		m_quoteService->UnRegisterListener(m_latestDataRequestID, m_latestDataCallBack);
		m_latestDataCallBack = 0;
		m_quoteService = 0;
	}
	m_latestDatas.clear();
	if(m_userSecurityService)
	{
		m_userSecurityService->UnRegisterListener(m_userSecurityService->GetOperatorRequestID(), m_userSecurityDataCallBack);
		m_userSecurityDataCallBack = 0;
		m_userSecurityService = 0;
	}
	if(m_window)
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

OwChart* UserSecurityList::GetChart()
{
	return m_chart;
}

void UserSecurityList::SetChart(OwChart *chart)
{
	m_chart = chart;
}

WindowEx* UserSecurityList::GetWindow()
{
	return m_window;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void UserSecurityList::AddCategory()
{
    UserSecurityCategory category;
    category.m_name = L"未命名的自选股";
	category.m_userID = DataCenter::GetUserID();
    m_userSecurityService->AddCategory(m_userSecurityService->GetOperatorRequestID(), &category);
}

void UserSecurityList::AddCategoriesToCategoryGrid(vector<UserSecurityCategory> *categories)
{
	if (m_gridCategory->GetEditTextBox())
    {
        m_gridCategory->OnCellEditEnd(0);
    }
	m_gridCategory->BeginUpdate();
    int categoriesSize = (int)categories->size();
    for (int i = 0; i < categoriesSize; i++)
    {
        UserSecurityCategory category = (*categories)[i];
        GridRow *row = new GridRow;
        row->SetGrid(m_gridCategory);
        GridStringCell *idCell = new GridStringCell;
        idCell->SetText(category.m_categoryID);
        row->AddCell(0, idCell);
        GridStringCell *textCell = new GridStringCell;
        textCell->SetAllowEdit(true);
        textCell->SetText(category.m_name);
        row->AddCell(1, textCell);
        m_gridCategory->AddRow(row);
        if (i == 0)
        {
            m_gridCategory->OnCellEditBegin(textCell);
        }
    }
    m_gridCategory->EndUpdate();
    m_gridCategory->Invalidate();
}

void UserSecurityList::AddSecurities()
{
    vector<GridRow*> selectedCategoryRows = m_gridCategory->GetSelectedRows();
    int rowsSize = (int)selectedCategoryRows.size();
    if (rowsSize > 0)
    {
        UserSecurityCategory category;
        m_userSecurityService->GetCategory(selectedCategoryRows[0]->GetCell(0)->GetString(), &category);
        SecurityList *securityList = new SecurityList(m_native);
        securityList->Show();
        WindowEx *window = securityList->GetWindow();
		window->SetChildWindow(true);
        window->SetTag(securityList);
        if (category.m_codes.length() > 0)
        {
            vector<Security> securities;
			m_securityService->GetSecuritiesByCodes(category.m_codes, &securities);
            securityList->AddSecuritiesToSecuritiesGrid(&securities);
        }
        WindowClosingEvent windowClosingEvent = &SecurityListWindowClosed;
		window->RegisterEvent(windowClosingEvent, EVENTID::WINDOWCLOSING, this);
		m_native->Invalidate();
    }
}

void UserSecurityList::AddSecuritiesToSecuritiesGrid(vector<Security> *securities)
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
		if(sIter3 == existsCodes.end())
		{
			GridRow *row = new GridRow;
            m_gridSecurities->AddRow(row);
            row->AddCell(0, new GridStringCell(security.m_code));
            row->AddCell(1, new GridStringCell(security.m_name));
			UserSecurityDoubleCell *closeCell = new UserSecurityDoubleCell;
            closeCell->SetUserSecurityList(this);
            row->AddCell(2, closeCell);
            UserSecurityDoubleCell *diffCell = new UserSecurityDoubleCell;
            diffCell->SetUserSecurityList(this);
            row->AddCell(3, diffCell);
            UserSecurityDoubleCell *diffRangeCell = new UserSecurityDoubleCell;
            diffRangeCell->SetUserSecurityList(this);
            row->AddCell(4, diffRangeCell);
            existsCodes[security.m_code] = L"";
		}
	}
	existsCodes.clear();
    m_gridSecurities->EndUpdate();
    m_gridSecurities->Invalidate();
}

void UserSecurityList::BindSecuritiesToSecuritiesGrid(UserSecurityCategory *category)
{
    m_gridSecurities->ClearRows();
    vector<Security> securities;
    if (category->m_codes.length() > 0)
    {
		vector<String> codes = CStr::Split(category->m_codes, L",");
        int codesSize = (int)codes.size();
        for (int i = 0; i < codesSize; i++)
        {
            Security security;
            bool state = m_securityService->GetSecurityByCode(codes[i], &security);
            if (state)
            {
                securities.push_back(security);
            }
        }
    }
    AddSecuritiesToSecuritiesGrid(&securities);
}

void UserSecurityList::Close()
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

void UserSecurityList::DeleteCategory()
{
    vector<GridRow*> selectedRows = m_gridCategory->GetSelectedRows();
    int rowsSize = (int)selectedRows.size();
    if (rowsSize > 0)
    {
        UserSecurityCategory category;
        m_userSecurityService->GetCategory(selectedRows[0]->GetCell(0)->GetString(), &category);
        if (category.m_categoryID.length() > 0)
        {
            m_userSecurityService->DeleteCategory(m_userSecurityService->GetOperatorRequestID(), &category);
        }
    }
}

void UserSecurityList::DeleteSecurities()
{
	vector<GridRow*> selectedCategoryRows = m_gridCategory->GetSelectedRows();
    int rowsSize = (int)selectedCategoryRows.size();
    if (rowsSize > 0)
    {
        UserSecurityCategory category;
        m_userSecurityService->GetCategory(selectedCategoryRows[0]->GetCell(0)->GetString(), &category);
        vector<GridRow*> selectedRows = m_gridSecurities->GetSelectedRows();
        int selectedRowsSize = (int)selectedRows.size();
        if (selectedRowsSize > 0)
        {
			vector<Security> securities;
            String codes = L"";
            for (int i = 0; i < selectedRowsSize; i++)
            {
                GridRow *row = selectedRows[i];
                codes += row->GetCell(0)->GetString();
                if (i != selectedRowsSize - 1)
                {
                    codes += L",";
                }
            }
            UserSecurityCategory updateCategory;
            updateCategory.m_categoryID = category.m_categoryID;
            updateCategory.m_codes = codes;
	    updateCategory.m_userID = DataCenter::GetUserID();
            m_userSecurityService->DeleteSecurities(m_userSecurityService->GetOperatorRequestID(), &updateCategory);
        }
    }
}

void UserSecurityList::LoadCategoriesToCategoryGrid(vector<UserSecurityCategory> *categories)
{
    m_gridCategory->BeginUpdate();
	vector<UserSecurityCategory>::iterator sIter = categories->begin();
	for(; sIter != categories->end(); ++sIter)
	{
		UserSecurityCategory category = *sIter;
        GridRow *row = new GridRow;
        row->SetGrid(m_gridCategory);
        GridStringCell *idCell = new GridStringCell;
        idCell->SetGrid(m_gridCategory);
        idCell->SetColumn(m_gridCategory->GetColumn(0));
        idCell->SetText(category.m_categoryID);
        row->AddCell(0, idCell);
        GridStringCell *textCell = new GridStringCell;
        textCell->SetGrid(m_gridCategory);
        textCell->SetColumn(m_gridCategory->GetColumn(0));
        textCell->SetAllowEdit(true);
        textCell->SetText(category.m_name);
        row->AddCell(1, textCell);
        m_gridCategory->AddRow(row);
	}
    m_gridCategory->EndUpdate();
    m_gridCategory->Invalidate();
}

void UserSecurityList::OnCategoryGridCellEditEnd(GridCell *cell)
{
    if (cell)
    {
        UserSecurityCategory category;
        m_userSecurityService->GetCategory(cell->GetRow()->GetCell(0)->GetString(), &category);
        if (category.m_categoryID.length() > 0)
        {
            UserSecurityCategory updateCategory;
            updateCategory.m_categoryID = category.m_categoryID;
            updateCategory.m_codes = category.m_codes;
            updateCategory.m_name = m_gridCategory->GetEditTextBox()->GetText();
            updateCategory.m_orderNum = category.m_orderNum;
            updateCategory.m_type = category.m_type;
            updateCategory.m_userID = category.m_userID;
            m_userSecurityService->UpdateCategory(m_userSecurityService->GetOperatorRequestID(), &updateCategory);
        }
    }
}

void UserSecurityList::OnInvoke(void *args)
{
    CMessage *message = (CMessage*)args;
	if(message->m_serviceID == m_quoteService->GetServiceID())
	{
		LatestDataInfo dataInfo;
        vector<SecurityLatestData> datas;
		QuoteService::GetLatestDatas(&dataInfo, &datas, message->m_body, message->m_bodyLength);
        int datasSize = (int)datas.size();
        for (int i = 0; i < datasSize; i++)
        {
            m_latestDatas[datas[i].m_securityCode] = datas[i];
        }
        datas.clear();
	}
	else
	{
		vector<UserSecurityCategory> categories;
		UserSecurityService::GetCategories(&categories, message->m_body, message->m_bodyLength);
		UserSecurityCategory category = categories[0];
		switch (message->m_functionID)
		{
			case FUNCTIONID_USERSECURITY_ADDCATEGORIES:
				AddCategoriesToCategoryGrid(&categories);
				break;
			case FUNCTIONID_USERSECURITY_DELETECATEGORIES:
				RemoveCategoriesFromCategoryGrid(&categories);
				break;
			case FUNCTIONID_USERSECURITY_UPDATECATEGORIES:
				UpdateCategoriesToCategoryGrid(&categories);
				break;
			case FUNCTIONID_USERSECURITY_ADDSECURITIES:
			case FUNCTIONID_USERSECURITY_DELETESECURITIES:
				{
                    String categoryID = L"";
                    vector<GridRow*> selectedRows = m_gridCategory->GetSelectedRows();
                    int selectedRowsSize = (int)selectedRows.size();
                    if (selectedRowsSize > 0)
                    {
                        categoryID = selectedRows[0]->GetCell(0)->GetText();
                    }
					if (categoryID.length() > 0 && categoryID==category.m_categoryID)
                    {
                        vector<Security> securities;
                        m_securityService->GetSecuritiesByCodes(category.m_codes, &securities);
                        if (message->m_functionID == FUNCTIONID_USERSECURITY_ADDSECURITIES)
                        {
                            AddSecuritiesToSecuritiesGrid(&securities);
                        }
                        else if (message->m_functionID == FUNCTIONID_USERSECURITY_DELETESECURITIES)
                        {
                            RemoveSecuritiesFromSecuritiesGrid(&securities);
                        }
                    }
                    break;
				}
				
		}
	}
    m_window->Invalidate();
}

void UserSecurityList::OnGridCellDoubleClick(GridCell *cell)
{
    GridRow *row = cell->GetRow();
    Security security;
	DataCenter::GetSecurityService()->GetSecurityByCode(row->GetCell(0)->GetText(), &security);
    m_chart->SearchSecurity(&security);
}

void UserSecurityList::OnGridSelectedRowsChanged()
{
	if(m_gridCategory)
	{
		String categoryID = L"";
		vector<GridRow*> selectedRows = m_gridCategory->GetSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		if (selectedRowsSize > 0)
		{
			categoryID = selectedRows[0]->GetCell(0)->GetText();
		}
		UserSecurityCategory category;
		m_userSecurityService->GetCategory(categoryID, &category);
		BindSecuritiesToSecuritiesGrid(&category);
	}
}

void UserSecurityList::OnLatestDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void UserSecurityList::OnSecurityListWindowClosed(SecurityList *securityList, bool *cancel)
{
    vector<GridRow*> selectedCategoryRows = m_gridCategory->GetSelectedRows();
    int rowsSize = (int)selectedCategoryRows.size();
    if (rowsSize > 0)
    {
        UserSecurityCategory category;
        m_userSecurityService->GetCategory(selectedCategoryRows[0]->GetCell(0)->GetString(), &category);
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
            category.m_codes = codes;
            m_userSecurityService->UpdateCategory(m_userSecurityService->GetOperatorRequestID(), &category);
        }
    }
	m_window->Invalidate();
}

void UserSecurityList::OnSearchTextBoxGotFocus()
{
	String categoryID = L"";
	vector<GridRow*> selectedRows = m_gridCategory->GetSelectedRows();
	int selectedRowsSize = (int)selectedRows.size();
	if (selectedRowsSize > 0)
	{
		categoryID = selectedRows[0]->GetCell(0)->GetText();
	}
	String *pCategoryID = new String(categoryID.c_str());
	m_txtSearch->SetTag(pCategoryID);
	m_chart->ShowSearchDiv('6');
}

void UserSecurityList::OnTimer(int timerID)
{
    if (m_timerID == timerID)
    {
        int scrollY = m_gridSecurities->GetVScrollBar()->GetPos();
		POINT fPoint = {0, m_gridSecurities->GetHeaderHeight() + 1 + scrollY};
		POINT ePoint = {0, m_gridSecurities->GetHeight() - 10 + scrollY};
        GridRow *fRow = m_gridSecurities->GetRow(fPoint);
        GridRow *eRow = m_gridSecurities->GetRow(ePoint);
        while (!eRow && ePoint.y > 0)
        {
            ePoint.y -= 10;
            eRow = m_gridSecurities->GetRow(ePoint);
        }
        if (fRow && eRow)
        {
            vector<String> requestCodes;
            for (int i = fRow->GetIndex(); i <= eRow->GetIndex(); i++)
            {
				String code = m_gridSecurities->GetRow(i)->GetCell(0)->GetText();
				requestCodes.push_back(code);
            }
			int requestCodesSize = (int)requestCodes.size();
            String strCodes = L"";
            for (int i = 0; i < requestCodesSize; i++)
            {
                strCodes += requestCodes[i];
                if (i != requestCodesSize)
                {
                    strCodes += L",";
                }
            }
            LatestDataInfo dataInfo ;
            dataInfo.m_codes = strCodes;
            dataInfo.m_formatType = 1;
            m_quoteService->GetLatestDatas(m_latestDataRequestID, &dataInfo);
            requestCodes.clear();
        }
    }
}

void UserSecurityList::OnUserSecurityDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void UserSecurityList::RemoveSecuritiesFromSecuritiesGrid(vector<Security> *securities)
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
				rowSize--;
				i--;
			}
		}
    }
    m_gridSecurities->EndUpdate();
    m_gridSecurities->Invalidate();
}

void UserSecurityList::RemoveCategoriesFromCategoryGrid(vector<UserSecurityCategory> *categories)
{
	if (m_gridCategory->GetEditTextBox())
    {
        m_gridCategory->OnCellEditEnd(0);
    }
    m_gridCategory->BeginUpdate();
    map<String, String> removeKeys;
	vector<UserSecurityCategory>::iterator sIter = categories->begin();
	for(; sIter != categories->end(); ++sIter)
	{
		UserSecurityCategory category = *sIter;
		removeKeys[category.m_categoryID] = L"";
	}
    int rowSize = (int)m_gridCategory->m_rows.size();
    for (int i = 0; i < rowSize; i++)
    {
		GridRow *row = m_gridCategory->GetRow(i);
        String categoryID = row->GetCell(0)->GetString();
		map<String, String>::iterator sIter2 = removeKeys.find(categoryID);
		if(sIter2 != removeKeys.end())
		{
			m_gridCategory->RemoveRow(row);
            row->ClearCells();
            delete row;
            rowSize--;
			i--;
		}
    }
    removeKeys.clear();
    m_gridCategory->EndUpdate();
    m_gridCategory->Invalidate();
}

void UserSecurityList::Show()
{
	vector<UserSecurityCategory> categories;
    m_userSecurityService->GetCategories(&categories);
    LoadCategoriesToCategoryGrid(&categories);
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(true);
    m_window->Invalidate();
	m_window->StartTimer(m_timerID, 1000);
}

void UserSecurityList::UpdateCategoriesToCategoryGrid(vector<UserSecurityCategory> *categories)
{
    map<String, UserSecurityCategory> updateKeys;
	vector<UserSecurityCategory>::iterator sIter = categories->begin();
	for(; sIter != categories->end(); ++sIter)
	{
		UserSecurityCategory category = *sIter;
        updateKeys[category.m_categoryID] = category;
	}
    vector<GridRow*> rows = m_gridCategory->m_rows;
	vector<GridRow*>::iterator sIter2 = rows.begin();
	for(; sIter2 != rows.end(); ++sIter2)
	{
		GridRow *row = *sIter2;
        UserSecurityCategory category;
        m_userSecurityService->GetCategory(row->GetCell(0)->GetString(), &category);
		map<String, UserSecurityCategory>::iterator sIter3 = updateKeys.find(category.m_categoryID);
        if (sIter3 != updateKeys.end())
        {
            UserSecurityCategory updateCategory = sIter3->second;
            m_userSecurityService->GetCategory(updateCategory.m_categoryID, &updateCategory);
            row->GetCell(1)->SetString(updateCategory.m_name);
            BindSecuritiesToSecuritiesGrid(&category);
        }
	}
    updateKeys.clear();
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

UserSecurityDoubleCell::UserSecurityDoubleCell()
{
	m_userSecurityList = 0;
    GridCellStyle style;
    style.SetBackColor(PCOLORS_BACKCOLOR4);
    style.SetForeColor(PCOLORS_FORECOLOR6);
	FONT uFont(L"Arial", 14, true, false, false);
    style.SetFont(&uFont);
    SetStyle(&style);
}

UserSecurityDoubleCell::~UserSecurityDoubleCell()
{
	m_userSecurityList = 0;
}

UserSecurityList* UserSecurityDoubleCell::GetUserSecurityList()
{
	return m_userSecurityList;
}

void UserSecurityDoubleCell::SetUserSecurityList(UserSecurityList *userSecurityList)
{
	m_userSecurityList = userSecurityList;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

double UserSecurityDoubleCell::GetDouble()
{
	GridRow *row = GetRow();
	SecurityLatestData data;
	String securityCode = row->GetCell(0)->GetText();
	if(m_userSecurityList->m_latestDatas.find(securityCode) != m_userSecurityList->m_latestDatas.end())
	{
		data = m_userSecurityList->m_latestDatas[securityCode];
	}
    GridColumn *column = GetColumn();
    String columnName = column->GetName();
	int dataSize = (int)data.m_securityCode.length();
    if (columnName == L"colClose")
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
    return 0;
}

int UserSecurityDoubleCell::GetPriceColor(double price, double comparePrice)
{
    if (price != 0)
    {
        if (price > comparePrice)
        {
            return PCOLORS_UPCOLOR;
        }
        else if (price < comparePrice)
        {
            return PCOLORS_DOWNCOLOR3;
        }
    }
    return PCOLORS_FORECOLOR6;
}

void UserSecurityDoubleCell::OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate)
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
			vector<GridRow*>::iterator sIter = selectedRows.begin();
			for(; sIter != selectedRows.end(); ++sIter)
			{
				GridRow *selectedRow = *sIter;
                if (selectedRow == row)
                {
                    selected = true;
                    break;
                }
            }
            _int64 backColor = COLOR_EMPTY;
            GridRowStyle *rowStyle = grid->GetRowStyle();
            if (selected)
            {
                backColor = rowStyle->GetSelectedBackColor();
            }
            else if (row == grid->GetHoveredRow())
            {
                backColor = rowStyle->GetHoveredBackColor();
            }
            else
            {
                backColor = rowStyle->GetBackColor();
            }
            paint->FillRect(backColor, clipRect);
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
            double value = GetDouble();
			wchar_t str[20] = {0};
            SecurityLatestData data;
			String securityCode = row->GetCell(0)->GetText();
			if(m_userSecurityList->m_latestDatas.find(securityCode) != m_userSecurityList->m_latestDatas.end())
			{
				data = m_userSecurityList->m_latestDatas[securityCode];
			}
			int dataSize = (int)data.m_securityCode.length();
            String columnName = column->GetName();
            if (columnName == L"colDiff")
            {
                if (dataSize > 0)
                {
					foreColor = GetPriceColor(value, 0);
					_stprintf_s(str, 19, L"%.2f", value);
                    text = str;
                }
            }
            else if (columnName == L"colDiffRange")
            {
                if (dataSize > 0)
                {
					foreColor = GetPriceColor(data.m_close, data.m_lastClose);
					_stprintf_s(str, 19, L"%.2f", value);
                    text = str;
					text += L"%";
                }
            }
            else
            {
                if (dataSize > 0)
                {
					foreColor = GetPriceColor(value, data.m_lastClose);
					_stprintf_s(str, 19, L"%.2f", value);
                    text = str;
                }
            }
			SIZE tSize = paint->TextSize(text.c_str(), font);
			POINT tPoint = {rect.right - tSize.cx - 3, rect.top + clipH / 2 - tSize.cy / 2};
			RECT tRect = {tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy};
			paint->DrawText(text.c_str(), foreColor, font, tRect);
        }
    }
}
#include "..\\..\\stdafx.h"
#include "SearchDiv.h"

void SearchDiv::GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if(button == MouseButtonsA_Left && clicks == 2)
	{
		SearchDiv *searchDiv = (SearchDiv*)pInvoke;
		if(searchDiv)
		{
			searchDiv->OnSelectRow();
		}
	}
}

void SearchDiv::GridKeyDown(void *sender, char key, void *pInvoke)
{
	SearchDiv *searchDiv = (SearchDiv*)pInvoke;
    if (key == 13)
    {
        searchDiv->OnSelectRow();
    }
}

void SearchDiv::TextBoxInput(void *sender, void *pInvoke)
{
	TextBoxA *control = (TextBoxA*)sender;
	SearchDiv *searchDiv = (SearchDiv*)pInvoke;
	searchDiv->m_searchTextBox = control;
    searchDiv->FilterSearch();
	String text = control->GetText();
	if(text.length() == 0)
	{
		searchDiv->SetVisible(false);
	}
    searchDiv->Invalidate();
}

void SearchDiv::TextBoxKeyDown(void *sender, char key, void *pInvoke)
{
	SearchDiv *searchDiv = (SearchDiv*)pInvoke;
    if (key == 13)
    {
        searchDiv->OnSelectRow();
    }
    else if (key == 38 || key == 40)
    {
        searchDiv->OnKeyDown(key);
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SearchDiv::SearchDiv()
{
	m_categoryID = L"";
	m_chart = 0;
	m_grid = 0;
	m_gridCellClickEvent = GridCellClick;
	m_gridKeyDownEvent = GridKeyDown;
	m_searchTextBox = 0;
	m_textBoxInputChangedEvent = TextBoxInput;
	m_textBoxKeyDownEvent = TextBoxKeyDown;
	SetBackColor(COLOR_EMPTY);
    SetWindow(true);
}

SearchDiv::~SearchDiv()
{
	m_chart = 0;
	if(m_grid)
	{
		if(m_gridCellClickEvent)
		{
			m_grid->UnRegisterEvent(m_gridCellClickEvent, EVENTID::GRIDCELLCLICK);
			m_grid->UnRegisterEvent(m_gridKeyDownEvent, EVENTID::KEYDOWN);
			m_gridCellClickEvent = 0;
		}
	}
	m_grid = 0;
	if(m_searchTextBox)
	{
		if(m_textBoxInputChangedEvent)
		{
			m_searchTextBox->UnRegisterEvent(m_textBoxInputChangedEvent, EVENTID::TEXTCHANGED);
			m_textBoxInputChangedEvent = 0;
		}
		if(m_textBoxKeyDownEvent)
		{
			m_searchTextBox->UnRegisterEvent(m_textBoxKeyDownEvent, EVENTID::KEYDOWN);
			m_textBoxKeyDownEvent = 0;
		}
	}
	m_searchTextBox = 0;
}

String SearchDiv::GetCategoryID()
{
	return m_categoryID;
}

void SearchDiv::SetCategoryID(const String& categoryID)
{
	m_categoryID = categoryID;
}

OwChart* SearchDiv::GetChart()
{
	return m_chart;
}

void SearchDiv::SetChart(OwChart *chart)
{
	m_chart = chart;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SearchDiv::FilterSearch()
{
	String sText = m_searchTextBox->GetText();
	m_grid->BeginUpdate();
	m_grid->ClearRows();
	sText = CStr::ToUpper(sText);
	CList<Security*> securities;
	DataCenter::GetSecurityService()->FilterSecurities(sText, &securities);
	int rowSize = (int)securities.size();
	for(int i = 0; i < rowSize; i++)
	{
		Security *security = securities.get(i);
		GridRow *gridRow = new GridRow();
		m_grid->AddRow(gridRow);
		gridRow->AddCell(0, new GridStringCell(security->m_code));
		gridRow->AddCell(1, new GridStringCell(security->m_name));
	}
	securities.clear();
	m_grid->GetVScrollBar()->SetPos(0);
	m_grid->EndUpdate();
}

void SearchDiv::OnLoad()
{
	MenuA::OnLoad();
	if(!m_grid)
	{
		m_grid = new GridA;
		m_grid->SetAutoEllipsis(true);
		m_grid->SetGridLineColor(COLOR_EMPTY);
		SIZE gSize = {240, 200};
		m_grid->SetSize(gSize);
		m_grid->RegisterEvent(m_gridCellClickEvent, EVENTID::GRIDCELLCLICK, this);
		m_grid->RegisterEvent(m_gridKeyDownEvent, EVENTID::KEYDOWN, this);
		AddControl(m_grid);
		m_grid->BeginUpdate();
		FONT colFont(L"Simsun", 14, true, false, false);
		GridColumn *securityCodeColumn = new GridColumn(L"股票代码");
		securityCodeColumn->SetBackColor(PCOLORS_BACKCOLOR);
		securityCodeColumn->SetBorderColor(PCOLORS_LINECOLOR3);
        securityCodeColumn->SetFont(&colFont);
		securityCodeColumn->SetForeColor(PCOLORS_FORECOLOR);
        securityCodeColumn->SetTextAlign(ContentAlignmentA_MiddleLeft);
		securityCodeColumn->SetWidth(120);
		m_grid->AddColumn(securityCodeColumn);
		GridColumn *securityNameColumn = new GridColumn(L"股票名称");
		securityNameColumn->SetBackColor(PCOLORS_BACKCOLOR);
		securityNameColumn->SetBorderColor(PCOLORS_LINECOLOR3);
        securityNameColumn->SetFont(&colFont);
		securityNameColumn->SetForeColor(PCOLORS_FORECOLOR);
        securityNameColumn->SetTextAlign(ContentAlignmentA_MiddleLeft);
		securityNameColumn->SetWidth(120);
		m_grid->AddColumn(securityNameColumn);
		GridColumn *spellColumn = new GridColumn(L"PINGYIN");
		spellColumn->SetVisible(false);
		m_grid->AddColumn(spellColumn);
		m_grid->EndUpdate();
	}
	if(!m_searchTextBox)
	{
		m_searchTextBox = new TextBoxA();
		POINT location = {0, 200};
		m_searchTextBox->SetLocation(location);
		SIZE sSize = {240, 20};
		m_searchTextBox->SetSize(sSize);
		m_searchTextBox->RegisterEvent(m_textBoxInputChangedEvent, EVENTID::TEXTCHANGED, this);
		m_searchTextBox->RegisterEvent(m_textBoxKeyDownEvent, EVENTID::KEYDOWN, this);
		AddControl(m_searchTextBox);
	}
}

void SearchDiv::OnKeyDown(char key)
{
	MenuA::OnKeyDown(key);
	if (key == 38 || key == 40)
    {
        m_grid->OnKeyDown(key);
    }
}

void SearchDiv::OnSelectRow()
{
    vector<GridRow*> rows = m_grid->GetSelectedRows();
	if(rows.size() > 0)
	{
		GridRow *selectedRow = rows[0];
		vector<GridCell*> cells = selectedRow->GetCells();
        Security security;
		DataCenter::GetSecurityService()->GetSecurityByCode(cells[0]->GetString(), &security);
		SetVisible(false);
        Invalidate();
		if(m_chart)
		{
			if(m_categoryID.length() > 0)
			{
				m_chart->AddUserSecurity(m_categoryID, security.m_code);
			}
			else
			{
				m_chart->SearchSecurity(&security);
			}
		}
	}
}

void SearchDiv::OnVisibleChanged()
{
	if(!IsVisible())
	{
		if (m_chart)
        {
            m_chart->GetChart()->SetFocused(true);
        }
	}
	MenuA::OnVisibleChanged();
}

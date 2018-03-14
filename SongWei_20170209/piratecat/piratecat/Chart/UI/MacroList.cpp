#include "..\\..\\stdafx.h"
#include "MacroList.h"

GridIconCell::GridIconCell()
{
	m_id = L"";
	m_keyPress = false;
}

GridIconCell::~GridIconCell()
{
}

String GridIconCell::GetID()
{
	return m_id;
}

void GridIconCell::SetID(String id)
{
	m_id = id;
}

bool GridIconCell::IsKeyPress()
{
	return m_keyPress;
}

void GridIconCell::SetKeyPress(bool keyPress)
{
	m_keyPress = keyPress;
}

void GridIconCell::OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate)
{
	GridA *grid = GetGrid();
    GridRow *row = GetRow();
	float opacity = grid->GetOpacity();
	float oldOpacity = grid->GetOpacity();
    if (grid->GetHoveredRow() == row)
    {
        POINT mp = grid->GetMousePoint();
        if (mp.x >= clipRect.left && mp.x <= clipRect.right
            && mp.y >= clipRect.top && mp.y <= clipRect.bottom)
        {
			if (m_id.length() > 0)
            {
                opacity = 1;
            }
		}
	}
    else if (m_keyPress)
    {
        opacity = 1;
    }
	if (m_id.length() > 0)
    {
        grid->SetOpacity(opacity);
        paint->SetOpacity(opacity);
    }
	String image = GetString();
	if(image.length() > 0)
	{
		paint->DrawImage(image.c_str(), rect);
	}
	else
	{
		paint->DrawRect(PCOLORS_LINECOLOR, 1, 0, rect);
	}
	if (m_id.length() > 0)
	{
		grid->SetOpacity(oldOpacity);
        paint->SetOpacity(oldOpacity);
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void MacroList::AddMacrosToGrid(vector<Macro> *macros)
{
    m_gridMacros->BeginUpdate();
	vector<Macro>::iterator sIter = macros->begin();
	for(; sIter != macros->end(); ++sIter)
	{
        Macro macro = *sIter;
        GridRow *row = new GridRow;
        row->SetGrid(m_gridMacros);
        GridStringCell *idCell = new GridStringCell;
        idCell->SetGrid(m_gridMacros);
        idCell->SetColumn(m_gridMacros->GetColumn(0));
        idCell->SetText(macro.m_macroID);
        row->AddCell(0, idCell);
        GridStringCell *textCell = new GridStringCell;
        textCell->SetGrid(m_gridMacros);
        textCell->SetColumn(m_gridMacros->GetColumn(1));
        textCell->SetText(macro.m_name);
        row->AddCell(1, textCell);
        m_gridMacros->AddRow(row);
	}
    m_gridMacros->EndUpdate();
}

void MacroList::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
    if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		MacroList *macroList = (MacroList*)pInvoke;
        if (name == L"btnClose")
        {
            macroList->Close();
        }
		else if (name == L"btnCreate")
        {
            macroList->Create();
        }
        else if (name == L"btnDelete")
        {
            macroList->Delete();
        }
        else if (name == L"btnSave")
        {
            macroList->Save();
        }
		else if (name == L"btnTest")
        {
            macroList->Test();
        }
    }
}

map<String, GridRow*> MacroList::GetMacroRows()
{
    map<String, GridRow*> mRowsMap;
	vector<GridRow*> rows = m_gridMacros->m_rows;
	vector<GridRow*>::iterator sIter = rows.begin();
	for(; sIter != rows.end(); ++sIter)
	{
		GridRow *row = *sIter;
        mRowsMap[row->GetCell(0)->GetText()] = row;
	}
    return mRowsMap;
}

void MacroList::GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	MacroList *macroList = (MacroList*)pInvoke;
	macroList->OnGridCellClick(cell, mp, button, clicks, delta);
}

void MacroList::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	MacroList *macroList = (MacroList*)pInvoke;
	macroList->OnGridSelectedRowsChanged();
}

void MacroList::Invoke(void *sender, void *args, void *pInvoke)
{
	MacroList *macroList = (MacroList*)pInvoke;
	macroList->OnInvoke(args);
}

void MacroList::LoadIcons()
{
	string dir = DataCenter::GetAppPath() + "\\config\\icons\\";
    String wDir = L"";
	CStr::stringTowstring(wDir, dir);
	m_gridIcons->SetResourcePath(wDir);
    vector<string> files;
	CFileA::GetFiles(dir.c_str(), &files);
	int filesSize = (int)files.size();
	int columnsSize = (int)m_gridIcons->m_columns.size();
    m_gridIcons->BeginUpdate();
    GridRow *row = 0;
    for (int i = 0; i < filesSize; i++)
    {
        int col = i;
        if (i >= columnsSize)
        {
            col = i % columnsSize;
        }
        if (col == 0)
        {
            row = new GridRow();
            row->SetHeight(64);
            m_gridIcons->AddRow(row);
        }
        string file = files[i];
		file = file.substr(file.find_last_of('\\') + 1);
		String wFile = L"";
		CStr::stringTowstring(wFile, file);
        GridIconCell *iconCell = new GridIconCell;
        iconCell->SetString(wFile);
        row->AddCell(col, iconCell);
    }
    m_gridIcons->EndUpdate();
    m_gridIcons->Invalidate();
	files.clear();
}

void MacroList::LoadMacro(Macro *macro)
{
    TextBoxA *txtMacroName = GetTextBox(L"txtMacroName");
    TextBoxA *txtMacroDescription = GetTextBox(L"txtMacroDescription");
    TextBoxA *txtMacroScript = GetTextBox(L"txtMacroScript");
    SpinA *spinInterval = GetSpin(L"spinInterval");
    SpinA *spinCD = GetSpin(L"spinCD");
    ButtonA *btnDelete = GetButton(L"btnDelete");
    ButtonA *btnApply = GetButton(L"btnApply");
    ButtonA *btnCreate = GetButton(L"btnCreate");
    ButtonA *btnSave = GetButton(L"btnSave");
	ComboBoxA *cbType = GetComboBox(L"cbType");
    txtMacroName->SetText(macro->m_name);
    txtMacroDescription->SetText(macro->m_description);
    txtMacroScript->SetText(macro->m_script);
    spinInterval->SetValue(macro->m_interval);
    spinCD->SetValue(macro->m_cd);
	cbType->SetSelectedIndex(macro->m_type);
	bool btnEnabled = macro->m_macroID.length() > 0;
    btnDelete->SetEnabled(btnEnabled);
    btnSave->SetEnabled(btnEnabled);
	m_divIcon->SetBackImage(macro->m_icon);
}

void MacroList::MacroDataCallBack(CMessage *message, void *pInvoke)
{
	MacroList *macroList = (MacroList*)pInvoke;
	macroList->OnMacroDataCallBack(message);
}

void MacroList::RegisterEvents(ControlA *control)
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

MacroList::MacroList(NativeBase *native)
{
	m_chart = 0;
	m_native = native;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\MacroList.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowMacro"));
	m_divIcon = GetDiv(L"divIcon");
	string dir = DataCenter::GetAppPath() + "\\config\\icons\\";
    String wDir = L"";
	CStr::stringTowstring(wDir, dir);
	m_divIcon->SetResourcePath(wDir);
	m_gridIcons = GetGrid(L"gridIcons");
	m_gridCellClickEvent = &GridCellClick;
	m_gridIcons->RegisterEvent(m_gridCellClickEvent, EVENTID::GRIDCELLCLICK, this);
    m_gridMacros = GetGrid(L"gridMacros");
    m_gridSelectedRowsChangedEvent = &GridSelectedRowsChanged;
	m_gridMacros->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
    m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    RegisterEvents(m_window);
	m_macroService = DataCenter::GetMacroService();
	m_macroDataCallBack = &MacroDataCallBack;
    m_macroService->RegisterListener(m_macroService->GetOperatorRequestID(), m_macroDataCallBack, this);
}

MacroList::~MacroList()
{
	m_chart = 0;
	if(m_gridIcons)
	{
		m_gridIcons->UnRegisterEvent(m_gridCellClickEvent, EVENTID::GRIDCELLCLICK);
		m_gridCellClickEvent = 0;
		m_gridIcons = 0;
	}
    if (m_gridMacros)
    {
		m_gridMacros->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_gridMacros = 0;
    }
	m_native = 0;
    if (m_macroService)
    {
        m_macroService->UnRegisterListener(m_macroService->GetOperatorRequestID(), m_macroDataCallBack);
		m_macroDataCallBack = 0;
		m_macroService = 0;
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

OwChart* MacroList::GetChart()
{
	return m_chart;
}

void MacroList::SetChart(OwChart *chart)
{
	m_chart = chart;
}

WindowEx* MacroList::GetWindow()
{
	return m_window;
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void MacroList::Close()
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

void MacroList::Create()
{
    Macro macro;
    macro.m_name = L"ÐÂµÄºê";
	macro.m_userID = DataCenter::GetUserID();
    m_macroService->AddMacro(m_macroService->GetOperatorRequestID(), &macro);
}

void MacroList::Delete()
{
    vector<GridRow*> selectedRows = m_gridMacros->GetSelectedRows();
    int selectedRowsSize = (int)selectedRows.size();
    if (selectedRowsSize > 0)
    {
        String macroID = selectedRows[0]->GetCell(0)->GetText();
        Macro macro;
        m_macroService->GetMacroByID(macroID, &macro);
        m_macroService->DeleteMacro(m_macroService->GetOperatorRequestID(), &macro);
    }
}

void MacroList::GetMacro(Macro *macro)
{
    TextBoxA *txtMacroName = GetTextBox(L"txtMacroName");
    TextBoxA *txtMacroDescription = GetTextBox(L"txtMacroDescription");
    TextBoxA *txtMacroScript = GetTextBox(L"txtMacroScript");
    SpinA *spinInterval = GetSpin(L"spinInterval");
    SpinA *spinCD = GetSpin(L"spinCD");
	ComboBoxA *cbType = GetComboBox(L"cbType");
	macro->m_type = cbType->GetSelectedIndex();
	if (m_macro.m_macroID.length() > 0)
    {
        macro->m_macroID = m_macro.m_macroID;
    }
    macro->m_name = txtMacroName->GetText();
    macro->m_description = txtMacroDescription->GetText();
    macro->m_script = txtMacroScript->GetText();
    macro->m_interval = (int)spinInterval->GetValue();
    macro->m_cd = (int)spinCD->GetValue();
	macro->m_icon = m_divIcon->GetBackImage();
}

void MacroList::OnGridCellClick(GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	m_divIcon->SetBackImage(cell->GetString());
    m_divIcon->Invalidate();
}

void MacroList::OnGridSelectedRowsChanged()
{
	if(m_gridMacros)
	{
		String macroID = L"";
		vector<GridRow*> selectedRows = m_gridMacros->GetSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		if (selectedRowsSize > 0)
		{
			macroID = selectedRows[0]->GetCell(0)->GetText();
		}
		Macro macro;
		m_macroService->GetMacroByID(macroID, &macro);
		m_macro = macro;
		LoadMacro(&m_macro);
		m_window->Invalidate();
	}
}

void MacroList::OnInvoke(void *args)
{
    CMessage *message = (CMessage*)args;
    vector<Macro> macros;
	MacroService::GetMacros(&macros, message->m_body, message->m_bodyLength);
    switch (message->m_functionID)
    {
        case FUNCTIONID_MACRO_ADDMACROS:
            AddMacrosToGrid(&macros);
            break;
        case FUNCTIONID_MACRO_DELETEMACROS:
            {
                map<String, GridRow*> mRowsMap = GetMacroRows();
				vector<Macro>::iterator sIter = macros.begin();
				for(; sIter != macros.end(); ++sIter)
				{
					Macro macro = *sIter;
					map<String, GridRow*>::iterator sIter2 = mRowsMap.find(macro.m_macroID);
					if(sIter2 != mRowsMap.end())
					{
						m_gridMacros->RemoveRow(sIter2->second);
						delete sIter2->second;
					}
				}
				m_gridMacros->Update();
                break;
            }
        case FUNCTIONID_MACRO_UPDATEMACROS:
            {
                map<String, GridRow*> mRowsMap = GetMacroRows();
				vector<Macro>::iterator sIter = macros.begin();
				for(; sIter != macros.end(); ++sIter)
				{
					Macro macro = *sIter;
					map<String, GridRow*>::iterator sIter2 = mRowsMap.find(macro.m_macroID);
					if(sIter2 != mRowsMap.end())
					{
						sIter2->second->GetCell(1)->SetText(macro.m_name);
					}
				}
                break;
            }
    }
    m_window->Invalidate();
}

void MacroList::OnMacroDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void MacroList::Save()
{
    Macro macro;
    GetMacro(&macro);
    if (macro.m_macroID.length() > 0)
    {
        m_macroService->UpdateMacro(m_macroService->GetOperatorRequestID(), &macro);
    }
}

void MacroList::Show()
{
	vector<Macro> macros;
    m_macroService->GetMacros(&macros);
    AddMacrosToGrid(&macros);
	LoadIcons();
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(false);
    m_window->Invalidate();
}

void MacroList::Test()
{
    Macro macro;
    GetMacro(&macro);
    m_macroService->Run(&macro);
}
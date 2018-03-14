#include "..\\..\\stdafx.h"
#include "ConnectWindow.h"

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void ConnectWindow::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
	{
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		ConnectWindow *connectWindow = (ConnectWindow*)pInvoke;
		if (name == L"btnAdd")
		{
			connectWindow->AddServer();
		}
		else if (name == L"btnDelete")
		{
			connectWindow->DeleteServer();
		}
		else if (name == L"btnClose" || name == L"btnCancel")
		{
			connectWindow->Close();
		}
		else if (name == L"btnSave")
		{
			connectWindow->SaveData();
		}
		else if (name == L"btnUpdate")
		{
			connectWindow->UpdateServer();
		}
	}
}

void ConnectWindow::ComboBoxSelectedIndexChanged(void *sender, void *pInvoke)
{
	ConnectWindow *connectWindow = (ConnectWindow*)pInvoke;
	ComboBoxA *comboBox = (ComboBoxA*)sender;
	connectWindow->OnComboBoxSelectedIndexChanged(comboBox);
}

void ConnectWindow::GridSelectedRowsChanged(void *sender, void *pInvoke)
{
	ConnectWindow *connectWindow = (ConnectWindow*)pInvoke;
	connectWindow->OnGridSelectedRowsChanged();
}

void ConnectWindow::Invoke(void *sender, void *args, void *pInvoke)
{
	ConnectWindow *connectWindow = (ConnectWindow*)pInvoke;
	connectWindow->OnInvoke(args);
}

void ConnectWindow::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	ControlEvent selectedIndexChangedEvent = &ComboBoxSelectedIndexChanged;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		GridColumn *column = dynamic_cast<GridColumn*>(*sIter);
		ButtonA *button = dynamic_cast<ButtonA*>(*sIter);
		ComboBoxA *comboBox = dynamic_cast<ComboBoxA*>(*sIter);
		GridA *grid = dynamic_cast<GridA*>(*sIter);
		if (column)
		{
            column->SetAllowResize(true);
			column->SetAllowSort(false);
			column->SetBackColor(PCOLORS_BACKCOLOR);
			column->SetBorderColor(COLOR_CONTROLBORDER);
            column->SetForeColor(PCOLORS_FORECOLOR);
		}
		else if (button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		else if (comboBox)
		{
			comboBox->RegisterEvent(selectedIndexChangedEvent, EVENTID::SELECTEDINDEXCHANGED, this);
		}
		else if(grid)
		{
			grid->SetGridLineColor(COLOR_EMPTY);
			GridRowStyle rowStyle;
			rowStyle.SetBackColor(COLOR_EMPTY);
			rowStyle.SetSelectedBackColor(PCOLORS_SELECTEDROWCOLOR);
			rowStyle.SetHoveredBackColor(PCOLORS_HOVEREDROWCOLOR);
			grid->SetRowStyle(&rowStyle);
		}
		RegisterEvents(*sIter);
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ConnectWindow::ConnectWindow(NativeBase *native)
{
	m_native = native;
	string xmlPath = DataCenter::GetAppPath()+ "\\config\\ConnectWindow.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
	SetNative(m_native);
	LoadFile(wXmlPath, 0);
	m_window = dynamic_cast<WindowEx*>(FindControl(L"windowConnect"));
	m_gridServers = GetGrid(L"gridServers");
	m_gridSelectedRowsChangedEvent = &GridSelectedRowsChanged;
	m_gridServers->RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED, this);
	m_connectService = DataCenter::GetConnectService();
	m_proxyService = DataCenter::GetProxyService();
	m_serverService = DataCenter::GetServerService();
	m_cbMainServerIP = GetComboBox(L"cbMainServerIP");
	m_cbType = GetComboBox(L"cbType");
	m_spinPort = GetSpin(L"spinPort");
	m_txtIP = GetTextBox(L"txtIP");
	m_spinMainServerPort = GetSpin(L"spinMainServerPort");
	RegisterEvents(m_window);
}

ConnectWindow::~ConnectWindow()
{
	m_connectService = 0;
	if(m_gridServers)
	{
		m_gridServers->UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID::GRIDSELECTEDROWSCHANGED);
		m_gridSelectedRowsChangedEvent = 0;
		m_gridServers = 0;
	}
	m_proxyService = 0;
	m_serverService = 0;
	if (m_window)
	{
		if(m_window->GetParent())
		{
			m_window->Close();
		}
		delete m_window;
		m_window = 0;
	}
}

OwChart* ConnectWindow::GetChart()
{
	return m_chart;
}

void ConnectWindow::SetChart(OwChart *chart)
{
	m_chart = chart;
}

WindowEx* ConnectWindow::GetWindow()
{
	return m_window;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void ConnectWindow::AddServer()
{
	String guid;
	CStr::stringTowstring(guid, CStrA::GetGuid());
	ServerInfo serverInfo;
	serverInfo.m_id = guid;
	serverInfo.m_ip = m_txtIP->GetText();
	serverInfo.m_port = (int)m_spinPort->GetValue();
	serverInfo.m_type = m_cbType->GetText();
	m_serverService->AddServer(&serverInfo);
	vector<ServerInfo> serverInfos;
	serverInfos.push_back(serverInfo);
	AddServersToGrid(&serverInfos);
	serverInfos.clear();
	BindServersToComboBox();
	m_window->Invalidate();
}

void ConnectWindow::AddServersToGrid(vector<ServerInfo> *serverInfos)
{
	m_gridServers->BeginUpdate();
	vector<ServerInfo>::iterator sIter = serverInfos->begin();
	for(; sIter != serverInfos->end(); ++sIter)
	{
		GridRow *row = new GridRow();
		row->SetGrid(m_gridServers);
		GridCell *idCell = new GridStringCell();
		idCell->SetGrid(m_gridServers);
		idCell->SetColumn(m_gridServers->GetColumn(0));
		idCell->SetText(sIter->m_id);
		row->AddCell(0, idCell);
		GridCell *ipCell = new GridStringCell();
		ipCell->SetGrid(m_gridServers);
		ipCell->SetColumn(m_gridServers->GetColumn(1));
		ipCell->SetText(sIter->m_ip);
		row->AddCell(1, ipCell);
		GridCell *portCell = new GridIntCell();
		portCell->SetGrid(m_gridServers);
		portCell->SetColumn(m_gridServers->GetColumn(2));
		portCell->SetInt(sIter->m_port);
		row->AddCell(2, portCell);
		GridCell *typeCell = new GridStringCell();
		typeCell->SetGrid(m_gridServers);
		typeCell->SetColumn(m_gridServers->GetColumn(3));
		typeCell->SetText(sIter->m_type);
		row->AddCell(3, typeCell);
		m_gridServers->AddRow(row);
	}
	m_gridServers->EndUpdate();
}

void ConnectWindow::BindServersToComboBox()
{
	m_cbMainServerIP->ClearItems();
	vector<ServerInfo> serverInfos;
	m_serverService->GetServers(&serverInfos);
	vector<ServerInfo>::iterator sIter = serverInfos.begin();	
	char pPort[10];
	for(;sIter != serverInfos.end(); ++sIter)
	{
		String type = sIter->m_type;
		MenuItemA *item = new MenuItemA(sIter->m_ip);
		memset(pPort, '\0', 10);
		sprintf_s(pPort, 9, "%d", sIter->m_port);
		string sPort(pPort);
		String wPort;
		CStrA::stringTowstring(wPort, sPort.c_str());
		item->SetValue(wPort);
		if (type == L"主服务器")
		{
			m_cbMainServerIP->AddItem(item);
		}
	}
}

void ConnectWindow::BindServerTypeComboBox()
{
	m_cbType->ClearItems();
	MenuItemA *item1 = new MenuItemA(L"主服务器");
	m_cbType->AddItem(item1);
}

void ConnectWindow::Close()
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

void ConnectWindow::DeleteServer()
{
	vector<GridRow*> selectedRows = m_gridServers->GetSelectedRows();
	vector<GridRow*>::iterator sIter = selectedRows.begin();
	for(;sIter != selectedRows.end(); ++sIter)
	{
		GridRow* row = dynamic_cast<GridRow*>(*sIter);
		m_serverService->DeleteServer(row->GetCell(0)->GetText());
		m_gridServers->RemoveRow(row);
		delete row;
		row = 0;
		break;
	}
	m_gridServers->Update();
	BindServersToComboBox();
	m_window->Invalidate();
}

void ConnectWindow::LoadData()
{
	BindServerTypeComboBox();
	vector<ConnectInfo> connectInfos;
	m_connectService->GetConnects(&connectInfos);
	vector<ConnectInfo>::iterator sIter = connectInfos.begin();
	for(;sIter != connectInfos.end(); sIter++)
	{
		String type = sIter->m_type;
		if (type == L"主服务器")
		{
			m_cbMainServerIP->SetText(sIter->m_ip);
			m_spinMainServerPort->SetValue(sIter->m_port);
		}
	}
	ComboBoxA *cbProxyType = GetComboBox(L"cbProxyType");
	vector<ProxyInfo> proxyInfos;
	m_proxyService->GetProxyInfos(&proxyInfos);
	vector<ProxyInfo>::iterator sIter2 = proxyInfos.begin();
	for(; sIter2 != proxyInfos.end(); ++sIter2)
	{
		ProxyInfo proxyInfo = *sIter2;
		if(proxyInfo.m_use)
		{
			cbProxyType->SetSelectedIndex(proxyInfo.m_type);
			break;
		}
	}
}

void ConnectWindow::LoadProxyInfo()
{
	ComboBoxA *cbProxyType = GetComboBox(L"cbProxyType");
	TextBoxA *txtProxyIP = GetTextBox(L"txtProxyIP");
	SpinA *spinProxyPort = GetSpin(L"spinProxyPort");
	TextBoxA *txtProxyUserName = GetTextBox(L"txtProxyUserName");
	TextBoxA *txtProxyUserPwd = GetTextBox(L"txtProxyUserPwd");
	TextBoxA *txtProxyDomain = GetTextBox(L"txtProxyDomain");
	vector<ProxyInfo> proxyInfos;
	m_proxyService->GetProxyInfos(&proxyInfos);
	vector<ProxyInfo>::iterator sIter = proxyInfos.begin();
	for(; sIter != proxyInfos.end(); ++sIter)
	{
		ProxyInfo proxyInfo = *sIter;
		if(cbProxyType->GetSelectedIndex() == proxyInfo.m_type)
		{
			txtProxyIP->SetEnabled(proxyInfo.m_type == 1 || proxyInfo.m_type == 2);
			spinProxyPort->SetEnabled(proxyInfo.m_type == 1 || proxyInfo.m_type == 2);
			txtProxyUserName->SetEnabled(proxyInfo.m_type == 1 || proxyInfo.m_type == 2|| proxyInfo.m_type == 3);
			txtProxyUserPwd->SetEnabled(proxyInfo.m_type == 1 || proxyInfo.m_type == 2|| proxyInfo.m_type == 3);
			txtProxyDomain->SetEnabled(proxyInfo.m_type == 2);
			txtProxyIP->SetText(proxyInfo.m_ip);
			spinProxyPort->SetValue(proxyInfo.m_port);
			txtProxyUserName->SetText(proxyInfo.m_name);
			txtProxyUserPwd->SetText(proxyInfo.m_pwd);
			txtProxyDomain->SetText(proxyInfo.m_domain);
			break;
		}
	}
	m_window->Invalidate();
	proxyInfos.clear();
}

void ConnectWindow::OnComboBoxSelectedIndexChanged(ComboBoxA *comboBox)
{
	String name = comboBox->GetName();
	if(name == L"cbMainServerIP")
	{
		String wPort = comboBox->GetSelectedValue();
		int port = CStr::ConvertStrToInt(wPort.c_str());
		m_spinMainServerPort->SetValue(port);
	}
	else if(name == L"cbProxyType")
	{
		LoadProxyInfo();
	}
}

void ConnectWindow::OnGridSelectedRowsChanged()
{
	vector<GridRow*> selectedRows = m_gridServers->GetSelectedRows();
	vector<GridRow*>::iterator sIter = selectedRows.begin();
	for(;sIter != selectedRows.end(); ++sIter)
	{
		GridRow *row = dynamic_cast<GridRow*>(*sIter);
		m_txtIP->SetText(row->GetCell(1)->GetText());
		m_spinPort->SetValue(row->GetCell(2)->GetInt());
		m_cbType->SetText(row->GetCell(3)->GetText());
		m_window->Invalidate();
		break;
	}
}

void ConnectWindow::OnInvoke(void *args)
{
}

void ConnectWindow::SaveData()
{
	vector<ConnectInfo> connectInfos;
	m_connectService->GetConnects(&connectInfos);
	vector<ConnectInfo>::iterator sIter = connectInfos.begin();
	for(;sIter != connectInfos.end(); ++sIter)
	{
		ConnectInfo connectInfo;
		connectInfo.m_id = sIter->m_id;
		connectInfo.m_ip = sIter->m_ip;
		connectInfo.m_port = sIter->m_port;
		connectInfo.m_type = sIter->m_type;
		String type = sIter->m_type;
		if (type == L"主服务器")
		{
			connectInfo.m_ip = m_cbMainServerIP->GetText();
			connectInfo.m_port = (int)m_spinMainServerPort->GetValue();
		}
		m_connectService->UpdateConnect(&connectInfo);
	}
	ComboBoxA *cbProxyType = GetComboBox(L"cbProxyType");
    TextBoxA *txtProxyIP = GetTextBox(L"txtProxyIP");
    SpinA *spinProxyPort = GetSpin(L"spinProxyPort");
    TextBoxA *txtProxyUserName = GetTextBox(L"txtProxyUserName");
    TextBoxA *txtProxyUserPwd = GetTextBox(L"txtProxyUserPwd");
    TextBoxA *txtProxyDomain = GetTextBox(L"txtProxyDomain");
    ProxyInfo proxyInfo;
    proxyInfo.m_domain = txtProxyDomain->GetText();
    proxyInfo.m_ip = txtProxyIP->GetText();
    proxyInfo.m_port = (int)spinProxyPort->GetValue();
    proxyInfo.m_name = txtProxyUserName->GetText();
    proxyInfo.m_pwd = txtProxyUserPwd->GetText();
    proxyInfo.m_type = cbProxyType->GetSelectedIndex();
	proxyInfo.m_use = true;
	vector<ProxyInfo> proxyInfos;
	m_proxyService->GetProxyInfos(&proxyInfos);
	vector<ProxyInfo>::iterator sIter2 = proxyInfos.begin();
	for(; sIter2 != proxyInfos.end(); ++sIter2)
	{
		ProxyInfo pInfo = *sIter2;
		if(cbProxyType->GetSelectedIndex() == pInfo.m_type)
		{
			m_proxyService->UpdateProxyInfo(&proxyInfo);
		}
		else
		{
			pInfo.m_use = false;
			m_proxyService->UpdateProxyInfo(&pInfo);
		}
	}
}

void ConnectWindow::Show()
{
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
	m_window->SetLocation(location);
	m_window->AnimateShow(true);
	vector<ServerInfo> serverInfos;
	m_serverService->GetServers(&serverInfos);
	AddServersToGrid(&serverInfos);
	BindServersToComboBox();
	LoadData();
	m_window->Invalidate();
}

void ConnectWindow::UpdateServer()
{
	vector<GridRow*> selectedRows = m_gridServers->GetSelectedRows();
	vector<GridRow*>::iterator sIter = selectedRows.begin();
	for(;sIter != selectedRows.end(); ++sIter)
	{
		GridRow *row = dynamic_cast<GridRow*>(*sIter);
		ServerInfo serverInfo;
		serverInfo.m_id = row->GetCell(0)->GetText();
		serverInfo.m_ip = m_txtIP->GetText();
		row->GetCell(1)->SetText(serverInfo.m_ip);
		serverInfo.m_port = (int)m_spinPort->GetValue();
		row->GetCell(2)->SetInt(serverInfo.m_port);
		serverInfo.m_type = m_cbType->GetText();
		row->GetCell(3)->SetText(serverInfo.m_type);
		m_serverService->UpdateServer(&serverInfo);
		break;
	}
	m_gridServers->Update();
	BindServersToComboBox();
	m_window->Invalidate();
}
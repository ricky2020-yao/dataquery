#include "..\\..\\stdafx.h"
#include "MacroServiceEx.h"

CFunctionEx::CFunctionEx(OwChart *chart, CIndicator *indicator, int id, String name, bool withParameters)
{
	m_chart = chart;
	m_indicator = indicator;
	m_id = id;
	m_name = name;
	m_withParameters = withParameters;
}

CFunctionEx::~CFunctionEx()
{
	m_chart = 0;
	m_indicator = 0;
}

String CFunctionEx::GetText(String text)
{
	return CStr::Replace(text, L"\'", L"");
}

bool CFunctionEx::IsNewData()
{
    CTable *dataSource = m_indicator->GetDataSource();
    return dataSource->RowsCount() == m_indicator->GetIndex() + 1;
}

double CFunctionEx::OnCalculate(CVariable *var)
{
	switch(var->m_functionID)
	{
	case 1000:
		return Alert(var);
	case 1001:
		return SetCycle(var);
	case 1002:
		return SetLayout(var);
	case 1003:
		return SetCode(var);
	case 1004:
		return ShowWindow(var);
	case 1005:
		return Sleep(var);
    case 1006:
        return ScrollLeft(var);
    case 1007:
        return ScrollRight(var);
    case 1008:
        return ZoomIn(var);
    case 1009:
        return ZoomOut(var);
	case 1010:
        return WinMouseEvent(var);
    case 1011:
        return WinSetText(var);
    case 1012:
        return WinExecute(var);
	case 1013:
		return WinSendKey(var);
	case 1014:
		return WinGetValue(var);
	default:
		return 0;
	}
}

double CFunctionEx::Alert(CVariable *var)
{
	BarrageDiv *barrageDiv = m_chart->GetBarrageDiv();
	Barrage *barrage = new Barrage;
	barrage->SetText(GetText(var->m_parameters[0]->m_expression));
	barrageDiv->AddBarrage(barrage);
    return 1;
}

double CFunctionEx::ScrollLeft(CVariable *var)
{
	m_chart->GetChart()->ScrollLeft(1);
	m_chart->GetChart()->Update();
	m_chart->GetNative()->Invalidate();
	return 1;
}

double CFunctionEx::ScrollRight(CVariable *var)
{
	m_chart->GetChart()->ScrollRight(1);
	m_chart->GetChart()->Update();
	m_chart->GetNative()->Invalidate();
	return 1;
}

double CFunctionEx::SetCode(CVariable *var)
{
	Security security;
	if(DataCenter::GetSecurityService()->GetSecurityByCode(GetText(var->m_parameters[0]->m_expression), &security))
	{
		m_chart->SearchSecurity(&security);
	}
	return 1;
}

double CFunctionEx::SetCycle(CVariable *var)
{
	m_chart->ChangeCycle((int)m_indicator->GetValue(var->m_parameters[0]));
	return 1;
}

double CFunctionEx::SetLayout(CVariable *var)
{
    IndicatorLayout indicatorLayout;
	if (DataCenter::GetIndicatorLayoutService()->GetLayoutByName(GetText(var->m_parameters[0]->m_expression), &indicatorLayout))
    {
		m_chart->SetIndicatorLayout(indicatorLayout.m_layoutID, indicatorLayout.m_text);
    }
	return 1;
}

double CFunctionEx::ShowWindow(CVariable *var)
{
	String window = GetText(var->m_parameters[0]->m_expression);
	if (window == L"CHATWINDOW")
    {
        m_chart->ShowChatWindow();
    }
    else if (window == L"INDICATORLAYOUTLIST")
    {
        m_chart->ShowIndicatorLayoutList();
    }
    else if (window == L"INDICATORLIST")
    {
        m_chart->ShowIndicatorList();
    }
    else if (window == L"SANDBOXWINDOW")
    {
        m_chart->ShowSandBoxWindow();
    }
    else if (window == L"SECURITYFILTERLIST")
    {
        m_chart->ShowSecurityFilterList();
    }
    else if (window == L"USERSECURITYLIST")
    {
        m_chart->ShowUserSecurityList();
    }
	return 1;
}

double CFunctionEx::Sleep(CVariable *var)
{
	::Sleep((int)m_indicator->GetValue(var->m_parameters[0]));
    return 1;
}

double CFunctionEx::WinExecute(CVariable *var)
{
	NativeBase *native = m_chart->GetNative();
    WinformControlHostEx *host = dynamic_cast<WinformControlHostEx*>(native->GetHost());
    if (host)
    {
		host->Execute(GetText(var->m_parameters[0]->m_expression));
	}
	return 1;
}

double CFunctionEx::WinGetValue(CVariable *var)
{
    double value = 0;
    NativeBase *native = m_chart->GetNative();
	WinformControlHostEx *host = dynamic_cast<WinformControlHostEx*>(native->GetHost());
    if (host)
    {
        String text = host->GetText();
		value = CStr::ConvertStrToDouble(text.c_str());
    }
    return value;
}

double CFunctionEx::WinSendKey(CVariable *var)
{
	NativeBase *native = m_chart->GetNative();
    WinformControlHostEx *host = dynamic_cast<WinformControlHostEx*>(native->GetHost());
    if (host)
    {
        SIZE displaySize = native->GetDisplaySize();
        int vlen = var->m_parametersLength;
        String key = L"";
        if (vlen >= 1)
        {
			key = GetText(var->m_parameters[0]->m_expression);
        }
		host->SendKey(key);
    }
	return 1;
}

double CFunctionEx::WinMouseEvent(CVariable *var)
{
    NativeBase *native = m_chart->GetNative();
    WinformControlHostEx *host = dynamic_cast<WinformControlHostEx*>(native->GetHost());
    if (host)
    {
        SIZE displaySize = native->GetDisplaySize();
        int dx = 0, dy = 0, data = 0;
        int vlen = var->m_parametersLength;
        String eventID = L"";
        if (vlen >= 1)
        {
			eventID = GetText(var->m_parameters[0]->m_expression);
        }
        if (vlen >= 2)
        {
            dx = (int)m_indicator->GetValue(var->m_parameters[1]);
        }
        if (vlen >= 3)
        {
            dy = (int)m_indicator->GetValue(var->m_parameters[2]);
        }
        if (vlen >= 4)
        {
            data = (int)m_indicator->GetValue(var->m_parameters[3]);
        }
        host->MouseEvent(eventID, dx, dy, data);
    }
	return 1;
}

double CFunctionEx::WinSetText(CVariable *var)
{
	NativeBase *native = m_chart->GetNative();
    WinformControlHostEx *host = dynamic_cast<WinformControlHostEx*>(native->GetHost());
    if (host)
    {
		host->SetText(GetText(var->m_parameters[0]->m_expression));
	}
	return 1;
}

double CFunctionEx::ZoomIn(CVariable *var)
{
	m_chart->GetChart()->ZoomIn();
	m_chart->GetChart()->Update();
	m_chart->GetNative()->Invalidate();
	return 1;
}

double CFunctionEx::ZoomOut(CVariable *var)
{
	m_chart->GetChart()->ZoomOut();
	m_chart->GetChart()->Update();
	m_chart->GetNative()->Invalidate();
	return 1;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void MacroServiceEx::ChartInvoke(void *sender, void *args, void *pInvoke)
{
	MacroServiceEx *macroService = (MacroServiceEx*)pInvoke;
	if(macroService)
	{
		macroService->OnChartInvoke(args);
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

MacroServiceEx::MacroServiceEx()
{
	m_chart = 0;
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
	m_operatorRequestID = BaseService::GetRequestID();
}

MacroServiceEx::~MacroServiceEx()
{
	m_chart = 0;
	m_macros.clear();
	m_lock.Lock();
	m_runningMacros.clear();
	m_lock.UnLock();
}

OwChart* MacroServiceEx::GetChart()
{
	return m_chart;
}

void MacroServiceEx::SetChart(OwChart* chart)
{
	m_chart = chart;
	ControlInvokeEvent invokeEvent = &ChartInvoke;
	m_chart->GetDiv(L"divMain")->RegisterEvent(invokeEvent, EVENTID::INVOKE, this);
}

int MacroServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

bool MacroServiceEx::IsLoaded()
{
	return m_loaded;
}

int MacroServiceEx::GetOperatorRequestID()
{
	return m_operatorRequestID;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

bool MacroServiceEx::GetMacroByID(const String& macroID, Macro *macro)
{
	vector<Macro>::iterator sIter = m_macros.begin();
	for(; sIter != m_macros.end(); ++sIter)
	{
		if((*sIter).m_macroID == macroID)
		{
			*macro = *sIter;
			return true;
		}
	}
	return false;
}

bool MacroServiceEx::GetMacros(vector<Macro> *macros)
{
	vector<Macro>::iterator sIter = m_macros.begin();
	for(; sIter != m_macros.end(); ++sIter)
	{
		macros->push_back(*sIter);
	}
	return true;
}

void MacroServiceEx::OnChartInvoke(void *args)
{
	Macro *macro = (Macro*)args;
    if (macro)
    {
        Run(macro);
		delete macro;
		macro = 0;
    }
}

int MacroServiceEx::OnMacroEnd(Macro *macro)
{
	m_lock.Lock();
	if((int)macro->m_macroID.length() > 0)
	{
		map<String, String>::iterator sIter = m_runningMacros.find(macro->m_macroID);
		if(sIter != m_runningMacros.end())
		{
			m_runningMacros.erase(sIter);
		}
	}
	m_lock.UnLock();
	return 1;
}

int MacroServiceEx::OnMacroRunning(Macro *macro)
{
	if(OnMacroStart(macro) == 0)
	{
		return 0;
	}
	if(macro->m_script.length() > 0)
	{	
		int endIndex = m_chart->GetIndex();
		ChartA *ct = m_chart->GetChart();
		CTable *dataSource = ct->GetDataSource();
		CTable *newDataSource = SecurityDataHelper::CreateDataSource(ct);
		int interval = macro->m_interval;
		CIndicator *indicator = SecurityDataHelper::CreateIndicator(ct, newDataSource, L"", L"");
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1000, L"ALERT", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1001, L"SETCYCLE", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1002, L"SETLAYOUT", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1003, L"SETCODE", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1004, L"SHOWWINDOW", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1005, L"SLEEP", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1006, L"SCROLLLEFT", false));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1007, L"SCROLLRIGHT", false));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1008, L"ZOOMIN", false));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1009, L"ZOOMOUT", false));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1010, L"WIN_MOUSEEVENT", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1011, L"WIN_SETTEXT", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1012, L"WIN_EXECUTE", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1013, L"WIN_SENDKEY", true));
		indicator->AddFunction(new CFunctionEx(m_chart, indicator, 1014, L"WIN_GETVALUE", false));
		indicator->SetScript(macro->m_script);
		int fields[6];
		fields[0] = CLOSE_INDEX;
		fields[1] = HIGH_INDEX;
		fields[2] = LOW_INDEX;
		fields[3] = OPEN_INDEX;
		fields[4] = VOL_INDEX;
		fields[5] = AMOUNT_INDEX;
		int fieldsSize = 6;
		int startIndex = endIndex - (interval - 1);
        if (startIndex < 0)
        {
            startIndex = 0;
        }
        if (startIndex > endIndex)
        {
            startIndex = endIndex;
        }
		if(macro->m_type == 0 || macro->m_type == 2 || endIndex == -1)
		{
			double date = 0;
			newDataSource->Set(date, VOL_INDEX, 0);
			int index = newDataSource->GetRowIndex(date);
			for (int j = 0; j < fieldsSize; j++)
			{
				if (fields[j] != VOL_INDEX)
				{
					newDataSource->Set2(index, fields[j], 0);
				}
			}
		}
		else
		{
			for (int i = startIndex; i <= endIndex - 1; i++)
			{
				double date = dataSource->GetXValue(i);
				newDataSource->Set(date, VOL_INDEX, dataSource->Get2(i, VOL_INDEX));
				int index = newDataSource->GetRowIndex(date);
				for (int j = 0; j < fieldsSize; j++)
				{
					if (fields[j] != VOL_INDEX)
					{
						newDataSource->Set2(index, fields[j], dataSource->Get2(i, fields[j]));
					}
				}
			}
		}
		if(macro->m_type == 1 || macro->m_type == 3)
		{
			indicator->OnCalculate(0);
		}
		else
		{
			for(int i = 0; i < interval; i++)
			{
				indicator->OnCalculate(0);
			}
		}
		indicator->Clear();
		delete indicator;
		indicator = 0;
		delete newDataSource;
		newDataSource = 0;
	}
	OnMacroEnd(macro);
	return 1;
}

int MacroServiceEx::OnMacroStart(Macro *macro)
{
	int state = 1;
	m_lock.Lock();
	if((int)macro->m_macroID.length() > 0)
	{
		map<String, String>::iterator sIter = m_runningMacros.find(macro->m_macroID);
		if(sIter != m_runningMacros.end())
		{
			m_runningMacros.erase(sIter);
			state = 0;
		}
		else
		{
			m_runningMacros[macro->m_macroID] = L"";
		}
	}
	m_lock.UnLock();
	return state;
}

void MacroServiceEx::OnReceive(CMessage *message)
{
	if(message->m_functionID != FUNCTIONID_MACRO_GETMACROS)
	{
		message->m_requestID = m_operatorRequestID;
	}
    vector<Macro> macros;
	MacroService::GetMacros(&macros, message->m_body, message->m_bodyLength);
    int macrosSize = (int)macros.size();
	switch (message->m_functionID)
    {
		case FUNCTIONID_MACRO_GETMACROS:
			{
				m_macros = macros;
				m_loaded = true;
				break;
			}
        case FUNCTIONID_MACRO_ADDMACROS:
            {
				bool add = false;
                for (int i = 0; i < macrosSize; i++)
                {
					bool has = false;
					vector<Macro>::iterator sIter = m_macros.begin();
					for(; sIter != m_macros.end(); ++sIter)
					{
						if((*sIter).m_macroID == macros[i].m_macroID)
						{
							has = true;
							break;
						}
					}
					if(!has)
					{
						m_macros.push_back(macros[i]);
						add = true;
					}
                }
				if(!add)
				{
					return;
				}
                break;
            }
        case FUNCTIONID_MACRO_DELETEMACROS:
            {
                for (int i = 0; i < macrosSize; i++)
                {
					vector<Macro>::iterator sIter = m_macros.begin();
					for(; sIter != m_macros.end(); ++sIter)
					{
						if((*sIter).m_macroID == macros[i].m_macroID)
						{
							m_macros.erase(sIter);
							break;
						}
					}
                }
                break;
            }
		case FUNCTIONID_MACRO_EXECUTEMACROS:
            {
                for (int i = 0; i < macrosSize; i++)
                {
                    Macro macro;
                    if (!GetMacroByID(macros[i].m_macroID, &macro))
                    {
						macro = macros[i];
                    }
					Macro *newMacro = new Macro;
					newMacro->m_cd = macro.m_cd;
					newMacro->m_description = macro.m_description;
					newMacro->m_icon = macro.m_icon;
					newMacro->m_interval = macro.m_interval;
					newMacro->m_macroID = macro.m_macroID;
					newMacro->m_name = macro.m_name;
					newMacro->m_orderNum = macro.m_orderNum;
					newMacro->m_script = macro.m_script;
					newMacro->m_type = macro.m_type;
					newMacro->m_userID = macro.m_userID;
					m_chart->GetDiv(L"divMain")->BeginInvoke((void*)newMacro);
                }
                break;
            }
        case FUNCTIONID_MACRO_UPDATEMACROS:
            {
                for (int i = 0; i < macrosSize; i++)
                {
                    Macro updateMacro = macros[i];
					int curMacrosSize = (int)m_macros.size();
                    for (int j = 0; j < curMacrosSize; j++)
                    {
                        Macro macro = m_macros[j];
                        if (macro.m_macroID == updateMacro.m_macroID)
                        {
                            m_macros[j] = updateMacro;
                            break;
                        }
                    }
                }
                break;
            }
    }
	MacroService::OnReceive(message);
}

DWORD WINAPI RunMacro(LPVOID lpParam)
{
	Macro *macro = (Macro*)lpParam;
	DataCenter::GetMacroService()->OnMacroRunning(macro);
	delete macro;
	macro = 0;
	return 1;
}

void MacroServiceEx::Run(Macro *macro)
{
	Macro *newMacro = new Macro;
	newMacro->m_cd = macro->m_cd;
	newMacro->m_description = macro->m_description;
	newMacro->m_icon = macro->m_icon;
	newMacro->m_interval = macro->m_interval;
	newMacro->m_macroID = macro->m_macroID;
	newMacro->m_name = macro->m_name;
	newMacro->m_orderNum = macro->m_orderNum;
	newMacro->m_script = macro->m_script;
	newMacro->m_type = macro->m_type;
	newMacro->m_userID = macro->m_userID;
	if(newMacro->m_type > 2)
	{
		HANDLE hThread = ::CreateThread(0, 0, RunMacro, (LPVOID)newMacro, 0, NULL);
		::CloseHandle(hThread);
	}
	else
	{
		RunMacro((LPVOID)newMacro);
	}
}
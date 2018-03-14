#include "..\\..\\stdafx.h"
#include "DataCenter.h"

static AIService *m_aiService = 0;
static ChatServiceEx *m_chatService = 0;
static ConnectService *m_connectService = 0;
static int m_connectState = 0;
static IndicatorLayoutServiceEx *m_indicatorLayoutService = 0;
static IndicatorServiceEx *m_indicatorService = 0;
static LoginInfo m_loginInfo;
static LoginService *m_loginService = 0;
static MacroServiceEx *m_macroService = 0;
static NewsService *m_newsService = 0;
static UIXmlEx *m_mainUI = 0;
static ProxyService *m_proxyService = 0;
static QuoteService *m_quoteService = 0;
static SecurityServiceEx *m_securityService = 0;
static SecurityFilterServiceEx *m_securityFilterService = 0;
static ServerService *m_serverService = 0;
static int m_userID = -1;
static UserCookieService* m_userCookieService = 0;
static UserSecurityServiceEx *m_userSecurityService = 0;
static UserSessionServiceEx *m_userSessionService = 0;
static map<String, Security> m_securities;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

DataCenter::DataCenter()
{
}

DataCenter::~DataCenter()
{
}

void DataCenter::dprintf( char *format, ...)
{
	static char buf[1024];
	va_list args;
	va_start( args, format );
	vsprintf_s( buf, format, args );
	va_end( args);
	OutputDebugStringA( buf);
	printf("%s", buf);
}

IXMLDOMDocument* DataCenter::DomFromCOM()
{
   HRESULT hr;
   IXMLDOMDocument *pxmldoc = 0;
 
   HRCALL( CoCreateInstance(__uuidof(MSXML2::DOMDocument30),
				  NULL,
				  CLSCTX_INPROC_SERVER,
				  __uuidof(IXMLDOMDocument),
				  (void**)&pxmldoc),
				  "Create a new DOMDocument");
 
	HRCALL( pxmldoc->put_async(VARIANT_FALSE),
			"should never fail");
	HRCALL( pxmldoc->put_validateOnParse(VARIANT_FALSE),
			"should never fail");
	HRCALL( pxmldoc->put_resolveExternals(VARIANT_FALSE),
			"should never fail");
 
	return pxmldoc;
clean:
	if (pxmldoc)
	{
		pxmldoc->Release();
	}
	return 0;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

AIService*  DataCenter::GetAIService()
{
	return m_aiService;
}

string DataCenter::GetAppPath()
{   
    char exeFullPath[MAX_PATH]; 
    string strPath = "";
    GetModuleFileNameA(0, exeFullPath, MAX_PATH);
    strPath = (string)exeFullPath; 
    int pos = (int)strPath.find_last_of('\\', strPath.length());
    return strPath.substr(0, pos);
}

ConnectService* DataCenter::GetConnectService()
{
	return m_connectService;
}

int DataCenter::GetConnectState()
{
	return m_connectState;
}

string DataCenter::GetUserPath()
{	
	char buffer[1024] = {0};
	::GetEnvironmentVariableA("LOCALAPPDATA", buffer, 1024);
	string userPath = buffer;
	if(!CFileA::IsDirectoryExist(userPath.c_str()))
	{
		userPath = GetAppPath();
	}
	else
	{
		userPath += "\\piratecat";
		if(!CFileA::IsDirectoryExist(userPath.c_str()))
		{
			CFileA::CreateDirectory(userPath.c_str());
		}
	}
	return userPath;
}

map<String, String> DataCenter::GetPlots()
{
	map<String, String> plots;
	IXMLDOMDocument *pXMLDom = 0;
    IXMLDOMParseError *pXMLErr = 0;
	IXMLDOMElement *spElement = 0;
	IXMLDOMNodeList *spNodeList = 0;
    BSTR bstr = 0;
    VARIANT_BOOL status;
    VARIANT var;
    HRESULT hr;
	String wPath;
	string path = GetAppPath() + "\\config\\Plots.xml";
	CStr::stringTowstring(wPath, path);
	long lCount = 0;
    pXMLDom = DomFromCOM();
    if (!pXMLDom) goto clean;
	VariantInit(&var);
	V_BSTR(&var) = SysAllocString(wPath.c_str());
	V_VT(&var) = VT_BSTR;
	HRCALL(pXMLDom->load(var, &status), "");
	if (status != VARIANT_TRUE) {
		HRCALL(pXMLDom->get_parseError(&pXMLErr),"");
		HRCALL(pXMLErr->get_reason(&bstr),"");
		goto clean;
	}
	HRCALL(pXMLDom->get_xml(&bstr), "");
	hr = pXMLDom->get_documentElement(&spElement);
	hr = spElement->get_childNodes(&spNodeList); 
	hr = spNodeList->get_length(&lCount);  
	for (long i = 0; i < lCount; ++i)
	{
		IXMLDOMNode *spChildNode = 0;
		hr = spNodeList->get_item(i, &spChildNode); 
		BSTR name;
		hr = spChildNode->get_nodeName(&name);
		String strName = name;
		if(name)
		{
			SysFreeString(name);
		}
		if(strName == L"Plot")
		{
			IXMLDOMNodeList *spChildNodeList = 0;
			hr = spChildNode->get_childNodes(&spChildNodeList);
			long childCount = 0;
			hr = spChildNodeList->get_length(&childCount);
			String plotName;
			String plotText;
			for(long j = 0; j < childCount; j++)
			{
				IXMLDOMNode *spChildNode2 = 0;
				hr = spChildNodeList->get_item(j, &spChildNode2);
				BSTR name2;
				hr = spChildNode2->get_nodeName(&name2);
				String strName2 = name2;
				if(name2)
				{
					SysFreeString(name2);
				}
				BSTR value;
				spChildNode2->get_text(&value);
				String strValue = value;
				if(value)
				{
					SysFreeString(value);
				}
				if(strName2 == L"Name")
				{
					plotName = strValue;
				}
				else if(strName2 == L"Text")
				{
					plotText = strValue;
				}
				if(spChildNode2)
				{
					spChildNode2->Release();
				}
			}
			plots[plotName] = plotText;
			if(spChildNodeList)
			{
				spChildNodeList->Release();
			}
		}
		if(spChildNode)
		{
			spChildNode->Release();
		}
	}
	clean:
    if (bstr) SysFreeString(bstr);
    if (&var) VariantClear(&var);
    if (pXMLErr) pXMLErr->Release();
    if (pXMLDom) pXMLDom->Release();
	if (spElement) spElement->Release();
	if (spNodeList) spNodeList->Release();
	return plots;
}

ChatServiceEx* DataCenter::GetChatService()
{
	return m_chatService;
}

IndicatorLayoutServiceEx* DataCenter::GetIndicatorLayoutService()
{
	return m_indicatorLayoutService;
}

IndicatorServiceEx* DataCenter::GetIndicatorService()
{
	return m_indicatorService;
}

LoginService* DataCenter::GetLoginService()
{
	return m_loginService;
}

MacroServiceEx* DataCenter::GetMacroService()
{
	return m_macroService;
}

NewsService* DataCenter::GetNewsService()
{
	return m_newsService;
}

ProxyService* DataCenter::GetProxyService()
{
	return m_proxyService;
}

QuoteService* DataCenter::GetQuoteService()
{
	return m_quoteService;
}

SecurityFilterServiceEx* DataCenter::GetSecurityFilterService()
{
	return m_securityFilterService;
}

SecurityServiceEx* DataCenter::GetSecurityService()
{
	return m_securityService;
}

ServerService* DataCenter::GetServerService()
{
	return m_serverService;
}

int DataCenter::GetUserID()
{
	return m_userID;
}

UserCookieService* DataCenter::GetUserCookieService()
{
	return m_userCookieService;
}

UserSecurityServiceEx* DataCenter::GetUserSecurityService()
{
	return m_userSecurityService;
}

UserSessionServiceEx* DataCenter::GetUserSessionService()
{
	return m_userSessionService;
}

void DataCenter::SetMainUI(UIXmlEx *mainUI)
{
	m_mainUI = mainUI;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int DataCenter::Connect()
{
	int socketID = -1;
	int qtSocketID = -1;
	vector<ConnectInfo> connectInfos;
	m_connectService->GetConnects(&connectInfos);
	vector<ConnectInfo>::iterator sIter = connectInfos.begin();
	for(; sIter != connectInfos.end(); ++sIter)
	{
		ConnectInfo connectInfo = *sIter;
		if(connectInfo.m_type == L"Ö÷·þÎñÆ÷")
		{
			vector<ProxyInfo> proxyInfos;
			m_proxyService->GetProxyInfos(&proxyInfos);
			vector<ProxyInfo>::iterator sIter2 = proxyInfos.begin();
			for(; sIter2 != proxyInfos.end(); ++sIter2)
			{
				ProxyInfo pInfo = *sIter2;
				if(pInfo.m_use)
				{
					string ip = "", proxyIP = "", proxyUserName = "", proxyPwd = "", proxyDomain = "";
					CStrA::wstringTostring(ip, connectInfo.m_ip);
					CStrA::wstringTostring(proxyUserName, pInfo.m_name);
					CStrA::wstringTostring(proxyPwd, pInfo.m_pwd);
					CStrA::wstringTostring(proxyDomain, pInfo.m_domain);
					 int proxyPort = pInfo.m_port;
					if(pInfo.m_type == 3)
					{
						//CurlHttp *http = CurlHttp::GetInstance();
						//http->GetIeProxySet(&proxyIP, &proxyPort);
					}
					else
					{
						CStrA::wstringTostring(proxyIP, pInfo.m_ip);
					}
					int cType = 0;
					if(pInfo.m_type == 1 || pInfo.m_type == 3)
					{
						cType = 1;
					}
					else if(pInfo.m_type == 2)
					{
						cType = 3;
					}
					socketID = BaseService::ConnectToServer(cType, ip.c_str(), connectInfo.m_port, proxyIP.c_str(), proxyPort, proxyUserName.c_str(), proxyPwd.c_str(), proxyDomain.c_str(), 6);
					qtSocketID = BaseService::ConnectToServer(cType, ip.c_str(), connectInfo.m_port, proxyIP.c_str(), proxyPort, proxyUserName.c_str(), proxyPwd.c_str(), proxyDomain.c_str(), 6);
					break;
				}
			}
		}
	}
	if(socketID > 0)
	{
		m_connectState = 1;
		m_aiService->SetSocketID(socketID);
		m_loginService->SetSocketID(socketID);
		m_newsService->SetSocketID(socketID);
		m_quoteService->SetSocketID(qtSocketID);
		m_indicatorService->SetSocketID(qtSocketID);
		m_securityService->SetSocketID(socketID);
		m_chatService->SetSocketID(socketID);
		m_indicatorLayoutService->SetSocketID(socketID);
		m_macroService->SetSocketID(socketID);
		m_securityFilterService->SetSocketID(socketID);
		m_userSecurityService->SetSocketID(socketID);
		m_userSessionService->SetSocketID(socketID);
	}
	return socketID;
}

int DataCenter::DisConnect()
{
	m_connectState = 0;
	BaseService::CloseClient(m_loginService->GetSocketID());
	BaseService::CloseClient(m_quoteService->GetSocketID());
	return 1;
}

int DataCenter::LoadData(int state)
{
	ChatData chatData;
	m_chatService->Enter(m_chatService->GetRequestID(), &chatData);
	Indicator indicator;
	indicator.m_userID = m_userID;
	(dynamic_cast<IndicatorService*>(m_indicatorService))->GetIndicators(m_indicatorService->GetListRequestID(), &indicator);
	SecurityFilterTemplate sfTemplate;
	sfTemplate.m_userID = m_userID;
	(dynamic_cast<SecurityFilterService*>(m_securityFilterService))->GetTemplates(m_securityFilterService->GetListRequestID(), &sfTemplate);
	UserSecurityVisitsCount visitsCount;
    visitsCount.m_userID = m_userID;
    m_userSecurityService->GetVisitsCount(m_userSecurityService->GetListRequestID(), &visitsCount);
	Macro macro;
	macro.m_userID = m_userID;
	(dynamic_cast<MacroService*>(m_macroService))->GetMacros(m_macroService->GetListRequestID(), &macro);
	UserSecurityCategory category;
	category.m_userID = m_userID;
	(dynamic_cast<UserSecurityService*>(m_userSecurityService))->GetCategories(m_userSecurityService->GetListRequestID(), &category);
	IndicatorLayout indicatorLayout;
	indicatorLayout.m_userID = m_userID;
	(dynamic_cast<IndicatorLayoutService*>(m_indicatorLayoutService))->GetLayouts(m_indicatorLayoutService->GetListRequestID(), &indicatorLayout);
	UserSession userSession;
    userSession.m_userID = m_userID;
    (dynamic_cast<UserSessionService*>(m_userSessionService))->GetSessions(m_userSessionService->GetListRequestID(), &userSession);
	if(state == 1)
	{
		m_securityService->LoadSecurities();
	}
	if(m_mainUI)
	{
		m_mainUI->LoadData();
	}
	return 0;
}

DWORD WINAPI OnReconnect(LPVOID lpParam)
{
	_int64 downFLow = 0;
    while(1)
    {
        _int64 newDownFLow = BaseService::GetDownFlow();
        if(downFLow > 0)
        {
            if(downFLow == newDownFLow)
            {
                DataCenter::DisConnect();
                while (1)
                {
                    if(DataCenter::Connect() > 0)
                    {
                        int loginOutReqID = BaseService::GetRequestID();
                        int loginReqID = BaseService::GetRequestID();
                        m_loginService->Logout(loginOutReqID);
                        CMessage message;
                        m_loginService->RegisterWait(loginReqID, &message);
                        m_loginService->Login(m_loginInfo.m_userName, m_loginInfo.m_passWord, loginReqID);
                        if(m_loginService->WaitMessage(loginReqID, 10000) > 0)
                        {
                            vector<LoginInfo> loginInfos;
                            LoginService::GetLoginInfos(&loginInfos, message.m_body, message.m_bodyLength);
                            int loginInfosSize = (int)loginInfos.size();
                            if(loginInfosSize > 0)
                            {
                                LoginInfo loginInfo = loginInfos[0];
                                if(loginInfo.m_state == 1)
                                {
                                    DataCenter::SaveLoginInfo(&loginInfo);
                                    DataCenter::LoadData(2);
                                }
                            }
                            loginInfos.clear();
                        }
                        else
                        {
                            m_loginService->Logout(loginOutReqID);
                        }
                        break;
                    }
                    Sleep(1000);
                }
            }
        }
        downFLow = newDownFLow;
        Sleep(10000);
    }
    return 1;
}

void DataCenter::SaveLoginInfo(LoginInfo* loginInfo)
{
	if(m_userID == -1)
	{
		HANDLE hThread = ::CreateThread(0, 0, OnReconnect, 0, 0, 0);
		::CloseHandle(hThread);
	}
	m_loginInfo = *loginInfo;
    m_userID = loginInfo->m_userID;
	m_aiService->SetSessionID(loginInfo->m_sessionID);
	m_chatService->SetSessionID(loginInfo->m_sessionID);
	m_indicatorLayoutService->SetSessionID(loginInfo->m_sessionID);
    m_indicatorService->SetSessionID(loginInfo->m_sessionID);
	m_loginService->SetSessionID(loginInfo->m_sessionID);
	m_macroService->SetSessionID(loginInfo->m_sessionID);
	m_newsService->SetSessionID(loginInfo->m_sessionID);
    m_quoteService->SetSessionID(loginInfo->m_sessionID);
	m_securityFilterService->SetSessionID(loginInfo->m_sessionID);
    m_securityService->SetSessionID(loginInfo->m_sessionID);
	m_userCookieService->SetUserID(m_userID);
    m_userSecurityService->SetSessionID(loginInfo->m_sessionID);
	m_userSessionService->SetSessionID(loginInfo->m_sessionID);
}

void DataCenter::StartService()
{
	m_aiService = new AIService;
	m_chatService = new ChatServiceEx;
	m_connectService = new ConnectService;
	m_indicatorLayoutService = new IndicatorLayoutServiceEx;
	m_indicatorService = new IndicatorServiceEx;
	m_loginService = new LoginService;
	m_macroService = new MacroServiceEx;
	m_newsService = new NewsService;
	m_proxyService = new ProxyService;
	m_quoteService = new QuoteService;
	m_securityService = new SecurityServiceEx;
	m_securityFilterService = new SecurityFilterServiceEx;
	m_serverService = new ServerService;
	m_userCookieService = new UserCookieService;
	m_userSecurityService = new UserSecurityServiceEx;
	m_userSessionService = new UserSessionServiceEx;
	BaseService::AddService(m_aiService);
	BaseService::AddService(m_chatService);
	BaseService::AddService(m_indicatorLayoutService);
	BaseService::AddService(m_indicatorService);
	BaseService::AddService(m_loginService);
	BaseService::AddService(m_macroService);
	BaseService::AddService(m_newsService);
	BaseService::AddService(m_quoteService);
	BaseService::AddService(m_securityFilterService);
	BaseService::AddService(m_securityService);
	BaseService::AddService(m_userSecurityService);
	BaseService::AddService(m_userSessionService);
	vector<BaseService*> services;
	BaseService::GetServices(&services);
	vector<BaseService*>::iterator sIter = services.begin();
	for(; sIter != services.end(); ++sIter)
	{
		BaseService *service = *sIter;
		service->SetCompressType(COMPRESSTYPE_NONE);
	}
}


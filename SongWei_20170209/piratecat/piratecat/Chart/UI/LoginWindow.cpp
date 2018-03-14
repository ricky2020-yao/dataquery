#include "..\\..\\stdafx.h"
#include "LoginWindow.h"

void LoginWindow::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		LoginWindow *loginWindow = (LoginWindow*)pInvoke;
		if (name == L"btnClose" || name == L"btnCancel")
		{
			exit(0);
		}
		else if (name == L"btnLogin")
		{
			loginWindow->Login();
		}
		else if (name == L"lblNetSetting")
		{
			loginWindow->ShowNetSetting();
		}
	}
}

void LoginWindow::Invoke(void *sender, void *args, void *pInvoke)
{
	LoginWindow *loginWindow = (LoginWindow*)pInvoke;
	loginWindow->OnInvoke(args);
}

void LoginWindow::LoginDataCallBack(CMessage *message, void *pInvoke)
{
	LoginWindow *loginWindow = (LoginWindow*)pInvoke;
	loginWindow->OnLoginDataCallBack(message);
}

void LoginWindow::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ButtonA *button = dynamic_cast<ButtonA*>(*sIter);
		LinkLabelA *linkLabel = dynamic_cast<LinkLabelA*>(*sIter);
		if(button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		else if(linkLabel)
		{
			linkLabel->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		RegisterEvents(*sIter);
	}
}

void LoginWindow::WindowLocationChanged(void *sender, void *pInvoke)
{
	LoginWindow *loginWindow = (LoginWindow*)pInvoke;
	loginWindow->OnWindowLocationChanged();
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

LoginWindow::LoginWindow(NativeBase *native)
{
	m_native = native;
	m_sky = 0;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\LoginWindow.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowLogin"));
	m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    RegisterEvents(m_window);
	m_loginService = DataCenter::GetLoginService();
	m_loginDataCallBackEvent = LoginDataCallBack;
	m_loginRequestID = BaseService::GetRequestID();
	m_loginService->RegisterListener(m_loginRequestID, m_loginDataCallBackEvent, this);
}

LoginWindow::~LoginWindow()
{
	if(m_loginService)
	{
		if(m_loginDataCallBackEvent)
		{
			m_loginService->UnRegisterListener(m_loginRequestID, m_loginDataCallBackEvent);
			m_loginDataCallBackEvent = 0;
		}
		m_loginService = 0;
	}
	m_sky = 0;
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

OwChart* LoginWindow::GetChart()
{
	return m_chart;
}

void LoginWindow::SetChart(OwChart *chart)
{
	m_chart = chart;
}


WindowEx* LoginWindow::GetWindow()
{
	return m_window;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


void LoginWindow::Close()
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

void LoginWindow::Login()
{
	int state = DataCenter::Connect();
	if(state == -1)
	{
		m_chart->ShowMessageBox(L"无法连接服务器!", L"提示", 0);
		return;
	}
    TabControlA *tabLogin = GetTabControl(L"tabLogin");
    TabPageA *selectedTabpage = tabLogin->GetSelectedTabPage();
    TextBoxA *txtUserName = GetTextBox(L"txtUserName");
    TextBoxA *txtPassword = GetTextBox(L"txtPassword");
    String userName = txtUserName->GetText();
    String passWord = txtPassword->GetText();
	if(userName.length() == 0)
	{
		m_chart->ShowMessageBox(L"请输入邮箱！", L"提示", 0);
		return;
	}
	if(userName.find(L"@") == -1)
	{
		m_chart->ShowMessageBox(L"邮箱格式不符合要求！", L"提示", 0);
		return;
	}
	if(passWord.length() == 0)
	{
		m_chart->ShowMessageBox(L"请输入密码！", L"提示", 0);
		return;
	}
    int ret = m_loginService->Login(userName, passWord, m_loginRequestID);
    if (ret != -1)
    {
		UserCookieService *cookieService = DataCenter::GetUserCookieService();
		UserCookie cookie;
		cookie.m_key = L"LOGININFO";
        cookie.m_value = userName + L"," + passWord;
        cookieService->AddCookie(&cookie);
        m_window->SetEnabled(false);
    }
}

void LoginWindow::OnInvoke(void *args)
{
    int state = (int)args;
	NativeBase *native = m_native;
    if (state >= 0)
    {
		DataCenter::LoadData(state);
        Close();
    }
    else
    {
		MessageBox(0, L"您输入的用户名或密码错误！", L"提示", 0);
        m_window->SetEnabled(true);
    }
    native->Invalidate();
}

void LoginWindow::OnLoginDataCallBack(CMessage *message)
{
    vector<LoginInfo> loginInfos;
	LoginService::GetLoginInfos(&loginInfos, message->m_body, message->m_bodyLength);
    if (loginInfos.size() > 0)
    {
        LoginInfo loginInfo = loginInfos[0];
        if (loginInfo.m_state == 1)
        {
			DataCenter::SaveLoginInfo(&loginInfo);
            m_window->Invoke((void*)loginInfo.m_type);
        }
        else
        {
            m_window->Invoke((void*)-1);
        }
    }
}

void LoginWindow::OnWindowLocationChanged()
{
    if (m_sky)
    {
		POINT location = {m_window->GetLeft(), m_window->GetTop() - 210};
        m_sky->SetLocation(location);
    }
}

void LoginWindow::Show()
{
	UserCookieService *cookieService = DataCenter::GetUserCookieService();
    UserCookie cookie;
    if (cookieService->GetCookie(L"LOGININFO", &cookie) > 0)
    {
		vector<String> strs = CStr::Split(cookie.m_value, L",");
        if ((int)strs.size() >= 2)
        {
			TextBoxA *txtUserName = GetTextBox(L"txtUserName");
            TextBoxA *txtPassword = GetTextBox(L"txtPassword");
            txtUserName->SetText(strs[0]);
            txtPassword->SetText(strs[1]);
        }
		strs.clear();
    }
	POINT location ={- m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(true);
    m_window->Invalidate();
	m_sky = new Sky;
	SIZE size = {m_window->GetWidth(), 200};
    m_sky->SetSize(size);
    m_window->GetFrame()->AddControl(m_sky);
	ControlEvent locationChanged = &WindowLocationChanged;
	m_window->RegisterEvent(locationChanged, EVENTID::LOCATIONCHANGED, this);
}

void LoginWindow::ShowNetSetting()
{
	ConnectWindow *connectWindow = new ConnectWindow(m_native);
	connectWindow->SetChart(m_chart);
	connectWindow->Show();
}


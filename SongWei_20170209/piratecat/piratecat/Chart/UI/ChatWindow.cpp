#include "..\\..\\stdafx.h"
#include "ChatWindow.h"

void ChatWindow::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		ChatWindow *chatWindow = (ChatWindow*)pInvoke;
		if (name == L"btnClose" || name == L"btnCancel")
		{
			chatWindow->Close();
		}
		else if (name == L"btnSend")
		{
			chatWindow->Send();
		}
	}
}

void ChatWindow::Invoke(void *sender, void *args, void *pInvoke)
{
	ChatWindow *chatWindow = (ChatWindow*)pInvoke;
	chatWindow->OnInvoke(args);
}

void ChatWindow::ChatDataCallBack(CMessage *message, void *pInvoke)
{
	ChatWindow *chatWindow = (ChatWindow*)pInvoke;
	chatWindow->OnChatDataCallBack(message);
}

void ChatWindow::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ButtonA *button = dynamic_cast<ButtonA*>(*sIter);
		if(button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		RegisterEvents(*sIter);
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ChatWindow::ChatWindow(NativeBase *native)
{
	m_native = native;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\ChatWindow.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowChat"));
	m_invokeEvent = &Invoke;
	m_window->RegisterEvent(m_invokeEvent, EVENTID::INVOKE, this);
    RegisterEvents(m_window);
	m_chatService = DataCenter::GetChatService();
	m_chatDataCallBackEvent = ChatDataCallBack;
	m_chatService->RegisterListener(m_chatService->GetRequestID(), m_chatDataCallBackEvent, this);
}

ChatWindow::~ChatWindow()
{
	if(m_chatService)
	{
		if(m_chatDataCallBackEvent)
		{
			m_chatService->UnRegisterListener(m_chatService->GetRequestID(), m_chatDataCallBackEvent);
			m_chatDataCallBackEvent = 0;
		}
		m_chatService = 0;
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

OwChart* ChatWindow::GetChart()
{
	return m_chart;
}

void ChatWindow::SetChart(OwChart *chart)
{
	m_chart = chart;
}

WindowEx* ChatWindow::GetWindow()
{
	return m_window;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


void ChatWindow::Close()
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

void ChatWindow::OnInvoke(void *args)
{
	CMessage* message = (CMessage*)args;
	vector<ChatData> datas;
	ChatService::GetChatDatas(&datas, message->m_body, message->m_bodyLength);
	vector<ChatData>::iterator sIter = datas.begin();
	for(; sIter != datas.end(); ++sIter)
	{
		ChatData data = *sIter;
		Barrage* brg = new Barrage();
		brg->SetText(data.m_text);
		m_chart->GetBarrageDiv()->AddBarrage(brg);
	}
	datas.clear();
}

void ChatWindow::OnChatDataCallBack(CMessage *message)
{
	m_window->BeginInvoke(message);
}

void ChatWindow::Send()
{
	TextBoxA *txtData = GetTextBox(L"txtData");
	ChatData data;
	data.m_text = txtData->GetText();
	data.m_userID = DataCenter::GetUserID();
	m_chatService->Send(m_chatService->GetRequestID(), &data);
}

void ChatWindow::Show()
{
	POINT location ={-m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(false);
    m_window->Invalidate();
}
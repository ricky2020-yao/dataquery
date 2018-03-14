#include "..\\..\\stdafx.h"
#include "AIChat.h"

AIChatCell::AIChatCell()
{
	m_chatList = 0;
	m_index = 0;
	m_isSelf = false;
	SetShowVScrollBar(false);
	SetBackColor(COLOR_EMPTY);
	SetBorderColor(COLOR_EMPTY);
	FONT font(L"SimSun", 20, false, false, false);
	SetFont(&font);
	SetMultiline(true);
	SIZE size = {0, 0};
	SetSize(size);
	SetReadOnly(true);
	SetWordWrap(true);
}

AIChatCell::~AIChatCell()
{
	m_chatList = 0;
}

AIChatList* AIChatCell::GetChatList()
{
	return m_chatList;
}

void AIChatCell::SetChatList(AIChatList *chatList)
{
	m_chatList = chatList;
}

int AIChatCell::GetIndex()
{
	return m_index;
}

void AIChatCell::SetIndex(int index)
{
	m_index = index;
}

bool AIChatCell::IsSelf()
{
	return m_isSelf;
}

void AIChatCell::SetIsSelf(bool isSelf)
{
	m_isSelf = isSelf;
	if(m_isSelf)
	{
		SetForeColor(COLOR::ARGB(0, 0, 0));
	}
	else
	{
		SetForeColor(COLOR::ARGB(255, 255, 255));
	}
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void AIChatCell::OnAdd()
{
}

void AIChatCell::OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if(m_chatList)
	{
		m_chatList->OnCellMouseWheel(this, mp, button, clicks, delta);
	}
}

void AIChatCell::OnPaint(CPaint *paint, const RECT& clipRect)
{
	if(m_isSelf)
	{
		paint->FillRoundRect(COLOR::ARGB(200, 255, 255, 255), clipRect, 10);
	}
	else
	{
		paint->FillRoundRect(COLOR::ARGB(200, 0, 255, 0), clipRect, 10);
	}
	TextBoxA::OnPaint(paint, clipRect);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void AIChatList::AiDataCallBack(CMessage *message, void *pInvoke)
{
	AIChatList *aiChatList = (AIChatList*)pInvoke;
	aiChatList->OnAiDataCallBack(message);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

AIChatList::AIChatList()
{
	m_aiService = 0;
	m_requestID = BaseService::GetRequestID();
	SetShowVScrollBar(true);
}

AIChatList::~AIChatList()
{
	m_cells.clear();
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void AIChatList::OnAdd()
{
	m_aiService = DataCenter::GetAIService();
	m_aiService->RegisterListener(m_requestID, &AiDataCallBack, this);
	Update();
}

void AIChatList::OnAiDataCallBack(CMessage *message)
{
    BeginInvoke(message);
}

void AIChatList::OnCellMouseWheel(AIChatCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	OnMouseWheel(GetMousePoint(), button, clicks, delta);
}

void AIChatList::OnInvoke(void *args)
{
	CMessage* message = (CMessage*)args;
	vector<AIChatData> resDatas;
	AIService::GetDatas(&resDatas, message->m_body, message->m_bodyLength);
	AIChatCell* cell = new AIChatCell();
	m_cells.push_back(cell);
	AddControl(cell);
	cell->SetForeColor(COLOR::ARGB(255, 255, 255));
	cell->SetIsSelf(false);
	cell->SetText(resDatas[0].m_text);
	Invalidate();
	Update();
	Invalidate();
}

void AIChatList::Send(const String& str)
{
	AIChatCell* cell = new AIChatCell();
	m_cells.push_back(cell);
	AddControl(cell);
	cell->SetIsSelf(true);
	cell->SetForeColor(COLOR::ARGB(0, 0, 0));
	cell->SetText(str);
	vector<AIChatData> aiChatDatas;
	AIChatData aiChatData;
	aiChatData.m_text = str;
	aiChatDatas.push_back(aiChatData);
	m_aiService->Ask(m_requestID, &aiChatDatas);
	Invalidate();
	Update();
    PageDown();
	Invalidate();
}

void AIChatList::OnPaint(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
    int cellsSize = (int)m_cells.size();
	SIZE size = GetSize();
	int top = 10;
    for (int i = 0; i < cellsSize; i++)
    {
        AIChatCell* cell = m_cells[i];
		SIZE cSize = cell->GetSize();
		SIZE textSize = paint->TextSize(cell->GetText().c_str(), cell->GetFont());
		textSize.cx += 5;
		cell->SetSize(textSize);
		POINT cLocation = {0};
		if(cell->IsSelf())
		{
			cLocation.x = 10;
		}
		else
		{
			cLocation.x = width - textSize.cx - 30;
		}
		cLocation.y = top;
		cell->SetLocation(cLocation);
		top += cSize.cy + 5;
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void AIChat::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		AIChat *aiChat = (AIChat*)pInvoke;
		if (name == L"btnSend")
		{
			aiChat->Send();
		}
	}
}

void AIChat::RegisterEvents(ControlA *control)
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

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

AIChat::AIChat()
{
	m_btnSend = 0;
	m_chatList = 0;
	m_txtChat = 0;
}

AIChat::~AIChat()
{
	m_btnSend = 0;
	m_chatList = 0;
	m_txtChat = 0;
}

void AIChat::OnAdd()
{
	m_btnSend = new ButtonA;
	m_btnSend->SetName(L"btnSend");
	m_btnSend->SetText(L"·¢ËÍ");
	AddControl(m_btnSend);
	m_chatList = new AIChatList;
	AddControl(m_chatList);
	m_txtChat = new TextBoxA;
	AddControl(m_txtChat);
	FONT txtFont(L"SimSun", 20, false, false, false);
	m_txtChat->SetFont(&txtFont);
	m_txtChat->SetLineHeight(30);
	RegisterEvents(this);
}

void AIChat::Send()
{
	String text = m_txtChat->GetText();
	if((int)text.length() > 0)
	{
		m_chatList->Send(m_txtChat->GetText());
		m_txtChat->SetText(L"");
		m_txtChat->SetFocused(true);
	}
}

void AIChat::Update()
{
	if(GetNative())
	{
		int txtHeight = 50, btnWidth = 100;
		int width = GetWidth(), height = GetHeight();
		SIZE chatListSize = {width, height - txtHeight};
		m_chatList->SetSize(chatListSize);
		SIZE txtChatSize = {width - btnWidth, txtHeight};
		m_txtChat->SetSize(txtChatSize);
		m_txtChat->SetLineHeight(txtHeight);
		POINT txtChatLocation = {0, height - txtHeight};
		m_txtChat->SetLocation(txtChatLocation);
		SIZE btnSendSize = {btnWidth, txtHeight};
		m_btnSend->SetSize(btnSendSize);
		POINT btnSendLocation = {width - btnWidth, height - txtHeight};
		m_btnSend->SetLocation(btnSendLocation);
	}
	ControlA::Update();
}
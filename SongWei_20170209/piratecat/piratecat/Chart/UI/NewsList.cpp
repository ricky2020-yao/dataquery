#include "..\\..\\stdafx.h"
#include "NewsList.h"

NewsCell::NewsCell()
{
	m_companyType = 0;
	m_index = 0;
	m_newsList = 0;
	SetBackColor(COLOR_EMPTY);
	SetBorderColor(COLOR_EMPTY);
	SetCanFocus(true);
	FONT font(L"SimSun", 20, false, false, false);
	SetFont(&font);
}

NewsCell::~NewsCell()
{
	m_newsList = 0;
}

int NewsCell::GetCompanyType()
{
	return m_companyType;
}

void NewsCell::SetCompanyType(int companyType)
{
	m_companyType =companyType;
}

int NewsCell::GetIndex()
{
	return m_index;
}

void NewsCell::SetIndex(int index)
{
	m_index = index;
}

NewsList* NewsCell::GetNewsList()
{
	return m_newsList;
}

void NewsCell::SetNewsList(NewsList *newsList)
{
	m_newsList = newsList;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void NewsCell::OnAdd()
{
}

void NewsCell::OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if(m_newsList)
	{
		m_newsList->OnClickCell(this, mp, button, clicks, delta);
	}
}

void NewsCell::OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if(m_newsList)
	{
		m_newsList->OnCellMouseWheel(this, mp, button, clicks, delta);
	}
}

void NewsCell::OnPaint(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth(), height = GetHeight();
	if(m_index == -1)
	{
		int padding = 5;
		RECT iRect = {padding, padding, width - padding * 2, height - padding * 2};
		paint->FillRect(GetPaintingBackColor(), iRect);
		paint->DrawImage(GetBackImage().c_str(), iRect);
	}
	else
	{
		LinkLabelA::OnPaint(paint, clipRect);
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void NewsList::NewsDataCallBack(CMessage *message, void *pInvoke)
{
	NewsList *newsList = (NewsList*)pInvoke;
	newsList->OnNewsDataCallBack(message);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

NewsList::NewsList()
{
	m_filterType = 0;
	m_newsService = 0;
	m_requestID = BaseService::GetRequestID();
	m_securityCode = L"";
	string dir = DataCenter::GetAppPath() + "\\config\\images\\";
    String wDir = L"";
	CStr::stringTowstring(wDir, dir);
	SetResourcePath(wDir);
	SetShowVScrollBar(true);
}

NewsList::~NewsList()
{
	m_cells.clear();
}

int NewsList::GetFilterType()
{
	return m_filterType;
}

void NewsList::SetFilterType(int filterType)
{
	m_filterType = filterType;
}

String NewsList::GetSecurityCode()
{
	return m_securityCode;
}

void NewsList::SetSecurityCode(const String& securityCode)
{
	m_securityCode = securityCode;
	NewsRequestData newsRequestData;
	newsRequestData.m_code = securityCode;
	newsRequestData.m_companyType = 0;
	vector<NewsRequestData> reqDatas;
	reqDatas.push_back(newsRequestData);
	m_newsService->GetNews(m_requestID, &reqDatas);
	reqDatas.clear();
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void NewsList::ClearNews()
{
	int cellsSize = (int)m_cells.size();
	for (int i = 3; i < cellsSize; i++)
	{
		NewsCell *cell = m_cells[i];
		RemoveControl(cell);
	}
	NewsCell *sinaCell = m_cells[0];
	NewsCell *baiduCell = m_cells[1];
	NewsCell *eastmoneyCell = m_cells[2];
	m_cells.clear();
	m_cells.push_back(sinaCell);
	m_cells.push_back(baiduCell);
	m_cells.push_back(eastmoneyCell);
}

void NewsList::FilterNews()
{
	int cellsSize = (int)m_cells.size();
	for (int i = 3; i < cellsSize; i++)
	{
		NewsCell *cell = m_cells[i];
		if(m_filterType == 0)
		{
			cell->SetVisible(true);
		}
		else
		{
			cell->SetVisible(cell->GetCompanyType() == m_filterType);
		}
	}
}

void NewsList::OnAdd()
{
	if(!m_newsService)
	{
		m_newsService = DataCenter::GetNewsService();
		m_newsService->RegisterListener(m_requestID, &NewsDataCallBack, this);
		NewsCell *sinaCell = new NewsCell;
		sinaCell->SetBackImage(L"sina.png");
		sinaCell->SetBackColor(COLOR::ARGB(255, 255, 255));
		sinaCell->SetCompanyType(1);
		sinaCell->SetIndex(-1);
		sinaCell->SetNewsList(this);
		AddControl(sinaCell);
		m_cells.push_back(sinaCell);
		NewsCell *baiduCell = new NewsCell;
		baiduCell->SetBackImage(L"baidu.png");
		baiduCell->SetBackColor(COLOR::ARGB(255, 255, 255));
		baiduCell->SetCompanyType(2);
		baiduCell->SetIndex(-1);
		baiduCell->SetNewsList(this);
		AddControl(baiduCell);
		m_cells.push_back(baiduCell);
		NewsCell *eastmoneyCell = new NewsCell;
		eastmoneyCell->SetBackImage(L"eastmoney.png");
		eastmoneyCell->SetBackColor(COLOR::ARGB(255, 255, 255));
		eastmoneyCell->SetCompanyType(3);
		eastmoneyCell->SetIndex(-1);
		eastmoneyCell->SetNewsList(this);
		AddControl(eastmoneyCell);
		m_cells.push_back(eastmoneyCell);
		Update();
		Invalidate();
	}
}

void NewsList::OnCellMouseWheel(NewsCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	OnMouseWheel(GetMousePoint(), button, clicks, delta);
}

void NewsList::OnClickCell(NewsCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if(cell->GetIndex() == -1)
	{
		SetFilterType(cell->GetCompanyType());
		FilterNews();
		Update();
		Invalidate();
	}
}

void NewsList::OnInvoke(void *args)
{
	CMessage* message = (CMessage*)args;
	vector<NewsData> newsDatas;
	NewsService::GetNewsDatas(&newsDatas, message->m_body, message->m_bodyLength);
	ClearNews();
	int newsDatasSize = (int)newsDatas.size();
	for(int i = 0; i < newsDatasSize; i++)
	{
		NewsData newsData = newsDatas[i];
		NewsCell *newsCell = new NewsCell;
		newsCell->SetCompanyType(newsData.m_companyType);
		newsCell->SetIndex(i);
		newsCell->SetNewsList(this);
		newsCell->SetText(newsData.m_title);
		m_cells.push_back(newsCell);
		AddControl(newsCell);
	}
	newsDatas.clear();
	Update();
	Invalidate();
}

void NewsList::OnNewsDataCallBack(CMessage *message)
{
	BeginInvoke(message);
}

void NewsList::Update()
{
	if(GetNative())
	{
		int width = GetWidth() - 16;
		int cellsSize = (int)m_cells.size();
		SIZE size = GetSize();
		int left = 10, top = 10;
		int headerHeight = 100, contentHeight = 40;
		for (int i = 0; i < cellsSize; i++)
		{
			NewsCell *cell = m_cells[i];
			if(cell->IsVisible())
			{
				if(i <= 2)
				{
					int cwidth = width / 3;
					RECT cellBounds = {left, top, left + cwidth, top + headerHeight};
					cell->SetBounds(cellBounds);
					left += cwidth;
					if(i == 2)
					{
						left = 0;
						top = 100;
					}
				}
				else
				{
					RECT cellBounds = {left, top, left + width, top + contentHeight};
					cell->SetBounds(cellBounds);
					top += 50;
				}
			}
		}
	}
	DivA::Update();
}
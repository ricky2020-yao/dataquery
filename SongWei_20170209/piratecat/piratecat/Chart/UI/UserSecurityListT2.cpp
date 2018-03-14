#include "..\\..\\stdafx.h"
#include "UserSecurityListT2.h"

UserSecurityCellT2::UserSecurityCellT2()
{
	m_clicks = 0;
	m_index = 0;
	m_list = 0;
	m_mode = 1;
	m_securityCode = L"";
	SetAllowDrag(true);
	SetBorderColor(COLOR_EMPTY);
	string resourcePath = DataCenter::GetAppPath() +  "\\config\\images";
	String path;
	CStr::stringTowstring(path, resourcePath);
	SetResourcePath(path);

}

UserSecurityCellT2::~UserSecurityCellT2()
{
	m_list = 0;
}

int UserSecurityCellT2::GetClicks()
{
	return m_clicks;
}

void UserSecurityCellT2::SetClicks(int clicks)
{
	m_clicks = clicks;
}

int UserSecurityCellT2::GetIndex()
{
	return m_index;
}

void UserSecurityCellT2::SetIndex(int index)
{
	m_index = index;
}

UserSecurityListT2* UserSecurityCellT2::GetList()
{
	return m_list;
}

void UserSecurityCellT2::SetList(UserSecurityListT2 *list)
{
	m_list = list;
}

int UserSecurityCellT2::GetMode()
{
	return m_mode;
}

void UserSecurityCellT2::SetMode(int mode)
{
	m_mode = mode;
}

RECT UserSecurityCellT2::GetPaintRect()
{
	return m_paintRect;
}

void UserSecurityCellT2::SetPaintRect(const RECT& paintRect)
{
	m_paintRect = paintRect;
}

double UserSecurityCellT2::GetRate()
{
	if(m_mode == 1)
	{
		return m_clicks;
	}
	else if(m_mode == 2)
	{
		double rate = 0;
		if(m_list->m_latestDatas.find(m_securityCode)  != m_list->m_latestDatas.end())
		{
			SecurityLatestData latestData = m_list->m_latestDatas[m_securityCode];
			double close = latestData.m_close, lastClose = latestData.m_lastClose;
			if(lastClose == 0)
			{
				lastClose = latestData.m_close;
			}
			double sub = 0;
			if (close == 0)
			{
				sub = latestData.m_buyPrice1 - lastClose;
				rate = 100 * (latestData.m_buyPrice1 - lastClose) / lastClose;
			}
			else
			{
				sub = close - latestData.m_lastClose;
				rate = 100 * (close - lastClose) / lastClose;
			}
			return rate;
		}
		else
		{
			return 0;
		}
	}
	return 0;
}

String UserSecurityCellT2::GetSecurityCode()
{
	return m_securityCode;
}

void UserSecurityCellT2::SetSecurityCode(String securityCode)
{
	m_securityCode = securityCode;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void UserSecurityCellT2::OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if(m_list)
	{
		m_list->OnClickCell(this, mp, button, clicks, delta);
	}
}

bool UserSecurityCellT2::OnDragBegin()
{
	return m_list->IsEditing();
}

void UserSecurityCellT2::OnDragging()
{
	ButtonA::OnDragging();
	if (m_list)
	{
		m_list->OnCellDragging(this);
	}
}

void UserSecurityCellT2::OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if (m_list)
	{
		m_list->OnCellMouseDown(this, mp, button, clicks, delta);
	}
}

void UserSecurityCellT2::OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if (m_list)
	{
		m_list->OnCellMouseMove(this, mp, button, clicks, delta);
	}
}

void UserSecurityCellT2::OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if (m_list)
	{
		m_list->OnCellMouseUp(this, mp, button, clicks, delta);
	}
}

void UserSecurityCellT2::OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if (m_list)
	{
		m_list->OnCellMouseWheel(this, mp, button, clicks, delta);
	}
}

void UserSecurityCellT2::OnPaint(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth(), height = GetHeight();
	RECT rect = {1, 1, width, height};
	int cornerRadius = 10;
	int cLeft = 1, cTop = 1;
	bool isDragging = IsDragging(), isEditing = m_list->IsEditing();
	if(!isDragging && isEditing)
	{
		rect.left += rand() % 15;
		rect.top += rand() % 15;
		rect.right -= rand() % 15;
		rect.bottom -= rand() % 15;
		cornerRadius = rand() % 10 + 10;
		cLeft += rand() % 5;
		cTop += rand() % 5;
	}
	paint->DrawGradientRect(COLOR::ARGB(200, 0, 0, 0), COLOR::ARGB(200, 0, 0, 0), rect, cornerRadius, 0);
	if (this == m_list->GetSelectedCell())
	{
		paint->DrawRoundRect(PCOLORS_LINECOLOR, 1, 0, rect, cornerRadius);
	}
	else
	{
		paint->DrawRoundRect(PCOLORS_LINECOLOR3, 1, 0, rect, cornerRadius);
	}
	SecurityServiceEx* securityService = DataCenter::GetSecurityService();
    Security security;
	if (securityService->GetSecurityByCode(m_securityCode, &security))
    {
        String securityName = security.m_name;
        FONT font(L"Î¢ÈíÑÅºÚ", 20, false, false, false);
        FONT font2(L"Arial", 18, false, false, false);
        if (m_index >= 2 && m_index <= 5)
        {
            font.m_fontSize = 18;
            font2.m_fontSize = 16;
        }
        else if (m_index >= 6)
        {
            font.m_fontSize = 16;
            font2.m_fontSize = 14;
        }
		String drawCode = m_securityCode.substr(0, m_securityCode.find(L"."));
        int left = rect.left + 10;
		SIZE tSize = paint->TextSize(securityName.c_str(), &font);
        SIZE tSize2 = paint->TextSize(drawCode.c_str(), &font2);
        int top = (height - tSize.cy - tSize2.cy) / 2;
        int mid = left + max(tSize.cx, tSize2.cx) / 2;
		CDraw::DrawText(paint, drawCode.c_str(), PCOLORS_FORECOLOR, &font2, mid - tSize2.cx / 2, top);
		CDraw::DrawText(paint, securityName.c_str(), PCOLORS_FORECOLOR, &font, mid - tSize.cx / 2, top + tSize2.cy);
        left += max(tSize.cx, tSize2.cx) + 20;
		if (m_list->m_latestDatas.find(m_securityCode) != m_list->m_latestDatas.end())
        {
            SecurityLatestData latestData = m_list->m_latestDatas[m_securityCode];
            double close = latestData.m_close, lastClose = latestData.m_lastClose;
            if (lastClose == 0)
            {
                lastClose = latestData.m_close;
            }
            double diff = 0;
            double rate = 0;
            if (close == 0)
            {
                diff = latestData.m_buyPrice1 - lastClose;
                rate = 100 * (latestData.m_buyPrice1 - lastClose) / lastClose;
            }
            else
            {
                diff = close - latestData.m_lastClose;
                rate = 100 * (close - lastClose) / lastClose;
            }
			long color = (long)CDraw::GetPriceColor(close, lastClose);
            String strRate = L"", strClose = L"";
			wchar_t str[20] = {0};
			_stprintf_s(str, 19, L"%.2f", rate);
			strRate = str;
			strRate += L"%";
			_stprintf_s(str, 19, L"%.2f", close);
			strClose = str;
            if (rate > 0)
            {
                strRate = L"+" + strRate;
            }
            tSize = paint->TextSize(strRate.c_str(), &font2);
            tSize2 = paint->TextSize(strClose.c_str(), &font2);
            if (m_index > 1)
            {
                mid = left + max(tSize.cx, tSize2.cx) / 2;
				CDraw::DrawText(paint, strClose.c_str(), color, &font2, mid - tSize2.cx / 2, top);
				CDraw::DrawText(paint, strRate.c_str(), color, &font2, mid - tSize.cx / 2, top + tSize2.cy + 5);
            }
            else
            {
				CDraw::DrawText(paint, strClose.c_str(), color, &font2, left, top);
				CDraw::DrawText(paint, strRate.c_str(), color, &font2, left + tSize.cx, top);
            }
        }
    }
    if (m_index <= 1)
    {
        int iLeft = 100, iTop = 45, iSize = 30;
        if (!isDragging && isEditing)
        {
            iLeft += rand()%10;
            iTop += rand()%10;
        }
        vector<String> images;
		images.push_back(L"attention.png");
        images.push_back(L"search.png");
        images.push_back(L"note.png");
        images.push_back(L"news.png");
		rect.left = iLeft;
		rect.top = iTop;
		rect.right = iLeft + iSize;
		rect.bottom = iTop + iSize;
		for (int i = 0; i < 4; i++)
        {
			RECT iRect = {iLeft, iTop, iLeft + iSize, iTop + iSize};
			paint->DrawImage(images[i].c_str(), iRect);
            iLeft += iSize + 20;
        }
		images.clear();
    }
    if (!isDragging && isEditing)
    {
        FONT font(L"Î¢ÈíÑÅºÚ", 16, false, false, false);
       	SIZE cSize = {30, 30};
		long lineColor = PCOLORS_LINECOLOR;
     	RECT ellipseRect = {cLeft, cTop, cLeft + cSize.cx, cTop + cSize.cy};
		paint->FillEllipse(PCOLORS_UPCOLOR, ellipseRect);
        SIZE tSize = paint->TextSize(L"É¾", &font);
		CDraw::DrawText(paint, L"É¾", COLOR_CONTROLTEXT, &font, ellipseRect.left + (cSize.cx - tSize.cx) / 2, ellipseRect.top + (cSize.cy - tSize.cy) / 2);
        paint->DrawEllipse(lineColor, 1, 0, ellipseRect);
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int UserSecurityListT2::GetAnimationPos(int now, int to)
{
	int sub = abs(now - to);
    if (sub < 10)
    {
        now = to;
    }
    else
    {
        if (now > to)
        {
            if (sub > 50)
            {
                now -= sub / 3;
            }
            else
            {
                now -= 10;
            }
        }
        else if (now < to)
        {
            if (sub > 50)
            {
                now += sub / 3;
            }
            else
            {
                now += 10;
            }
        }
    }
    return now;
}

void UserSecurityListT2::LatestDataCallBack(CMessage *message, void *pInvoke)
{
	UserSecurityListT2 *userSecurityList = (UserSecurityListT2*)pInvoke;
	userSecurityList->OnLatestDataCallBack(message);
}

void UserSecurityListT2::LoadCells()
{
	int cellsSize = (int)m_cells.size();
  	map<String, UserSecurityCellT2*> removeCells;
    for (int i = 0; i < cellsSize; i++)
    {
        UserSecurityCellT2* cell = m_cells[i];
        removeCells[cell->GetSecurityCode()] = cell;
    }
	m_cells.clear();
	vector<UserSecurityCategory> categories;
    m_userSecurityService->GetCategories(&categories);
    UserSecurityCategory category = categories[0];
	vector<String> strs = CStr::Split(category.m_codes, L",");
    int strsSize = (int)strs.size();
    for (int i = 0; i < strsSize; i++)
    {
        String securityCode = strs[i];
		if (removeCells.find(securityCode) != removeCells.end())
        {
			m_cells.push_back(removeCells[securityCode]);
			removeCells.erase(removeCells.find(securityCode));
        }
        else
        {
            UserSecurityCellT2* cell = new UserSecurityCellT2();
            cell->SetSecurityCode(securityCode) ;
			cell->SetBackColor(COLOR::ARGB(200, 0, 0, 0));
			cell->SetPaintRect(cell->GetBounds());
			cell->SetList(this);
            if (m_userSecurityService->m_visitsCount.find(securityCode) != m_userSecurityService->m_visitsCount.end())
            {
				cell->SetClicks(m_userSecurityService->m_visitsCount[securityCode]);
            }
            m_cells.push_back(cell);
            AddControl(cell);
        }
    }
	map<String, UserSecurityCellT2*>::iterator iter;
	for(iter = removeCells.begin(); iter != removeCells.end(); iter++)
	{
		UserSecurityCellT2* cell = iter->second;
		if(cell == m_selectedCell)
		{
			m_selectedCell = 0;
		}
		RemoveControl(cell);
		delete cell;
	}
	removeCells.clear();
	sort(m_cells.begin(),m_cells.end(), UserSecurityCellT2Desc); 
    Update();
}

void UserSecurityListT2::UserSecurityDataCallBack(CMessage *message, void *pInvoke)
{
	UserSecurityListT2* userSecurityList = (UserSecurityListT2*)pInvoke;
	userSecurityList->OnUserSecurityDataCallBack(message);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

bool UserSecurityListT2::UserSecurityCellT2Asc(UserSecurityCellT2 *x, UserSecurityCellT2 *y)
{
	return x->GetRate() < y->GetRate();  
}

bool UserSecurityListT2::UserSecurityCellT2Desc(UserSecurityCellT2 *x, UserSecurityCellT2 *y)
{
	return x->GetRate() > y->GetRate();  
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

UserSecurityListT2::UserSecurityListT2()
{
	m_cellState = 0;
	m_chart = 0;
	m_isEditing = false;
	m_latestDataRequestID = BaseService::GetRequestID();	
	m_quoteService = 0;
	m_securityService = 0;
	m_selectedCell = 0;
	m_tick = 0;
	m_tick2 = 0;
	m_timerID = GetNewTimerID();
	m_useAnimation = true;
	m_userSecurityService = 0;
	SetBackColor(COLOR_EMPTY);
	SetShowVScrollBar(true);
}

UserSecurityListT2::~UserSecurityListT2()
{
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
}

OwChart* UserSecurityListT2::GetChart()
{
	return m_chart;
}

void UserSecurityListT2::SetChart(OwChart *chart)
{
	m_chart = chart;
}

bool UserSecurityListT2::IsEditing()
{
	return m_isEditing;
}

void UserSecurityListT2::SetIsEditing(bool isEditing)
{
	m_isEditing = isEditing;
}

UserSecurityCellT2* UserSecurityListT2::GetSelectedCell()
{
	return m_selectedCell;
}

void UserSecurityListT2::SetSelectedCell(UserSecurityCellT2 *selectedCell)
{
	m_selectedCell = selectedCell;
}

bool UserSecurityListT2::UseAnimation()
{
	return m_useAnimation;
}

void UserSecurityListT2::SetUseAnimation(bool useAnimation)
{
	m_useAnimation = useAnimation;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void UserSecurityListT2::OnAdd()
{
	DivA::OnAdd();
	if(!m_securityService)
	{
		m_securityService = DataCenter::GetSecurityService();
		m_userSecurityService = DataCenter::GetUserSecurityService();
		m_userSecurityDataCallBack = &UserSecurityDataCallBack;
		m_userSecurityService->RegisterListener(m_userSecurityService->GetListRequestID(), m_userSecurityDataCallBack, this);
		m_userSecurityService->RegisterListener(m_userSecurityService->GetOperatorRequestID(), m_userSecurityDataCallBack, this);
		m_quoteService = DataCenter::GetQuoteService();
		m_latestDataCallBack = &LatestDataCallBack;
		m_quoteService->RegisterListener(m_latestDataRequestID, m_latestDataCallBack, this);
		StartTimer(m_timerID, 10);
	}
}

void UserSecurityListT2::OnLatestDataCallBack(CMessage *message)
{
    BeginInvoke(message);
}

void UserSecurityListT2::OnCellDragging(UserSecurityCellT2 *cell)
{
	ControlHost* host = GetNative()->GetHost();	
	RECT* tempRect = new RECT();
    RECT bounds = cell->GetBounds();
	int cellsSize = (int)m_cells.size();
    int thisIndex = -1;
    for (int i = 0; i < cellsSize; i++)
    {
        UserSecurityCellT2* iCell = m_cells[i];
        if (iCell == cell)
        {
            thisIndex = i;
            break;
        }
    }
    int mx = bounds.left + (bounds.right - bounds.left) / 2;
    int my = bounds.top + (bounds.bottom - bounds.top) / 2;
    for (int i = 0; i < cellsSize; i++)
    {
        UserSecurityCellT2* iCell = m_cells[i];
        if (iCell != cell)
        {
			RECT iBounds = iCell->GetPaintRect();
            if (host->GetIntersectRect(tempRect, &bounds, &iBounds) > 0)
            {
                if (mx >= iBounds.left && mx <= iBounds.right && my >= iBounds.top && my <= iBounds.bottom)
                {
                    m_cells[thisIndex] = iCell;
                    m_cells[i] = cell;
                    Update();
                    break;
                }
            }
        }
    }
}

void UserSecurityListT2::OnClickCell(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	if (m_cellState > 0)
	{
		if (m_isEditing)
		{
			if (mp.x <= 30 && mp.y <= 30)
			{
				UserSecurityCategory* category = new UserSecurityCategory();
				category->m_categoryID = L"primary";
				category->m_userID = DataCenter::GetUserID();
				category->m_codes = cell->GetSecurityCode();
				m_userSecurityService->DeleteSecurities(m_userSecurityService->GetOperatorRequestID(), category);
			}
			Invalidate();
		}
		else
		{
			Security* security = new Security();
			m_securityService->GetSecurityByCode(cell->GetSecurityCode(), security);
			m_userSecurityService->AddVisitsCount(cell->GetSecurityCode());
			m_chart->SearchSecurity(security);
			cell->SetClicks(cell->GetClicks() + 1);
			sort(m_cells.begin(),m_cells.end(), UserSecurityCellT2Desc); 
			Update();
		}
	}
    m_cellState = 0;
}

void UserSecurityListT2::OnCellMouseDown(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	m_selectedCell = cell;
	m_cellState = 1;
	m_tick2 = 0;
}

void UserSecurityListT2::OnCellMouseMove(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
    if (m_cellState == 1)
    {
        m_cellState = 0;
    }
}

void UserSecurityListT2::OnCellMouseUp(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
    if (m_cellState == 1)
    {
        m_cellState = 2;
    }
}

void UserSecurityListT2::OnCellMouseWheel(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	OnMouseWheel(GetMousePoint(), button, clicks, delta);
}

void UserSecurityListT2::OnInvoke(void *args)
{
    CMessage* message = (CMessage*)args;
    if (message->m_serviceID == m_quoteService->GetServiceID())
    {
        LatestDataInfo* dataInfo = new LatestDataInfo();
        vector<SecurityLatestData> datas;
		QuoteService::GetLatestDatas(dataInfo, &datas, message->m_body, message->m_bodyLength);
		int datasSize = (int)datas.size();
        for (int i = 0; i < datasSize; i++)
        {
            m_latestDatas[datas[i].m_securityCode] = datas[i];
        }
        datas.clear();
    }
    else if (message->m_serviceID == m_userSecurityService->GetServiceID())
    {
        if (message->m_functionID < FUNCTIONID_USERSECURITY_GETVISITSCOUNT)
        {
            LoadCells();
        }
    }
    Invalidate();
}

void UserSecurityListT2::OnTimer(int timerID)
{
	DivA::OnTimer(timerID);
    if (m_timerID == timerID)
    {
        bool paint = false;
        if (m_cellState == 1)
        {
			if (m_selectedCell && !m_selectedCell->IsDragging())
            {
                m_tick2++;
                if (m_isEditing)
                {
                    if (m_tick2 > 20)
                    {
                        m_isEditing = false;
                        paint = true;
                        m_cellState = 0;
                        Update();
                    }
                }
                else
                {
                    if (m_tick2 > 20)
                    {
                        m_isEditing = true;
                        m_cellState = 0;
                        paint = true;
                        Update();
                    }
                }
            }
        }
        if (m_tick % 50 == 0)
        {
            vector<UserSecurityCategory> categories;
			m_userSecurityService->GetCategories(&categories);
            int categoriesSize = (int)categories.size();
            if (categoriesSize > 0)
            {
                LatestDataInfo* dataInfo = new LatestDataInfo();
                dataInfo->m_codes = categories[0].m_codes;
                dataInfo->m_formatType = 1;
                m_quoteService->GetLatestDatas(m_latestDataRequestID, dataInfo);
                categories.clear();
            }
        }
        int width = GetWidth() - 15, height = GetHeight();
        int cellsSize = (int)m_cells.size();
        for (int i = 0; i < cellsSize; i++)
        {
            UserSecurityCellT2* cell = m_cells[i];
            if (!cell->IsDragging())
            {
                RECT bounds = cell->GetBounds();
                RECT paintRect = cell->GetPaintRect();
                int left = bounds.left, top = bounds.top, right = bounds.right, bottom = bounds.bottom;
                int pLeft = paintRect.left, pTop = paintRect.top, pRight = paintRect.right, pBottom = paintRect.bottom;
                int subLeft = abs(left - pLeft), subTop = abs(top - pTop), subRight = abs(right - pRight), subBottom = abs(bottom - pBottom);
                if (subTop > height || subBottom > height)
                {
                    paint = true;
					cell->SetBounds(paintRect);
                }
                else
                {
                    if (subLeft > 0)
                    {
                        paint = true;
                        left = GetAnimationPos(left, pLeft);
                    }
                    if (subTop > 0)
                    {
                        paint = true;
                        top = GetAnimationPos(top, pTop);
                    }
                    if (subRight > 0)
                    {
                        paint = true;
                        right = GetAnimationPos(right, pRight);
                    }
                    if (subBottom > 0)
                    {
                        paint = true;
                        bottom = GetAnimationPos(bottom, pBottom);
                    }
					RECT rect = {left, top, right, bottom};
					cell->SetBounds(rect);
                }
            }
        }
        m_tick++;
        if (m_tick > 1000)
        {
            m_tick = 0;
        }
        if (m_useAnimation && m_isEditing)
        {
            if (m_tick % 2 == 0)
            {
                paint = true;
            }
        }
        if (paint)
        {
            Invalidate();
        }
    }
}

void UserSecurityListT2::OnUserSecurityDataCallBack(CMessage *message)
{
	BeginInvoke(message);
}

void UserSecurityListT2::Update()
{
    int width = GetWidth() - 15, height = GetHeight();
    int uHeight = 80;
    int dTop = uHeight * 3;
    int cellsSize = (int)m_cells.size();
    for (int i = 0; i < cellsSize; i++)
    {
        UserSecurityCellT2* cell = m_cells[i];
        cell->SetIndex(i);
        if (i == 0 || i == 1)
        {
			RECT rect = {0, uHeight * i, width, uHeight * (i + 1)};
            cell->SetPaintRect(rect);
        }
        else if (i == 2)
        {
			RECT rect = {0, uHeight * 2, width / 2, uHeight * 3};
            cell->SetPaintRect(rect);
        }
        else if (i == 3)
        {
			RECT rect = {width / 2, uHeight * 2, width, uHeight * 3};
            cell->SetPaintRect(rect);
        }
        else if (i == 4)
        {
			RECT rect = {0, uHeight * 3, width / 2, uHeight * 4};
            cell->SetPaintRect(rect);
        }
        else if (i == 5)
        {
			RECT rect = {width / 2, uHeight * 3, width, uHeight * 4};
            cell->SetPaintRect(rect);
        }
        else
        {
            int index = (i - 6) % 3;
            if (index == 0)
            {
                dTop += uHeight;
            }
            int uwidth = width / 3;
			RECT rect = {uwidth * index, dTop, uwidth * (index + 1), dTop + uHeight};
            cell->SetPaintRect(rect);
        }
		if (!m_useAnimation || (cell->GetLeft() == 0 && cell->GetTop() == 0))
        {
			cell->SetBounds(cell->GetPaintRect());
        }
    }
	DivA::Update();
    Invalidate();
}
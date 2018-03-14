#include "..\\..\\stdafx.h"
#include "WindowEx.h"

WindowEx::WindowEx()
{
	m_animateDirection = -1;
	m_animateMoving = false;
	m_animateType = 0;
	m_closeButton = 0;
	m_isChildWindow = false;
	m_timerID =  ControlA::GetNewTimerID();
	SetBackColor(COLOR_EMPTY);
	SetBorderColor(PCOLORS_LINECOLOR3);
	SetCaptionHeight(23);
	FONT wFont(L"SimSun", 14, true, false, false);
	SetFont(&wFont);
	SetForeColor(COLOR_EMPTY);
	SetOpacity(0);
	SetShadowColor(PCOLORS_BACKCOLOR5);
}

WindowEx::~WindowEx()
{
	m_animateMoving = false;
	m_closeButton = 0;
	StopTimer(m_timerID);
}

bool WindowEx::IsAnimateMoving()
{
	return m_animateMoving;
}

RibbonButton* WindowEx::GetCloseButton()
{
	return m_closeButton;
}

void WindowEx::SetCloseButton(RibbonButton* closeButton)
{
	m_closeButton = closeButton;
}

bool WindowEx::IsChildWindow()
{
	return m_isChildWindow;
}

void WindowEx::SetChildWindow(bool isChildWindow)
{
	m_isChildWindow = isChildWindow;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void WindowEx::AnimateHide()
{
    m_animateType = 1;
	NativeBase *native = GetNative();
    ControlHost *host = native->GetHost();
	m_animateDirection = rand() % 4;
    StartTimer(m_timerID, 10);
	m_animateMoving = true;
    host->SetAllowOperate(false);
}

void WindowEx::AnimateShow(bool showDialog)
{
	m_animateType = 0;
    NativeBase *native = GetNative();
    ControlHost *host = native->GetHost();
    SIZE nativeSize = native->GetDisplaySize();
    int width = GetWidth(), height = GetHeight(), mx = (nativeSize.cx - width) / 2, my = (nativeSize.cy - height) / 2, x = mx, y = my;
	m_animateDirection = rand() % 4;
    switch (m_animateDirection)
    {
        case 0:
            x = -width;
            break;
        case 1:
            x = nativeSize.cx;
            break;
        case 2:
            y = -height;
            break;
        case 3:
            y = nativeSize.cy;
            break;
    }
	POINT location = {x, y};
    SetLocation(location);
	if(showDialog)
	{
		ShowDialog();
	}
	else
	{
		Show();
	}
	Update();
    StartTimer(m_timerID, 10);
	m_animateMoving = true;
    host->SetAllowOperate(false);
}

void WindowEx::OnAdd()
{
	ControlA::OnAdd();
    if (!m_closeButton)
    {
        m_closeButton = new RibbonButton;
		m_closeButton->SetClose(true);
        m_closeButton->SetName(L"btnClose");
		SIZE buttonSize = {22, 22};
        m_closeButton->SetSize(buttonSize);
        AddControl(m_closeButton);
    }
}

void WindowEx::OnDragReady(POINT *startOffset)
{
	startOffset->x = 0;
	startOffset->y = 0;
}

void WindowEx::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
    int width = GetWidth();
    int height = GetHeight();
	RECT rect = {0, 0, width, height};
    _int64 backColor = PCOLORS_WINDOWBACKCOLOR;
    _int64 foreColor = PCOLORS_WINDOWFORECOLOR;
    if (paint->SupportTransparent())
    {
		backColor = PCOLORS_WINDOWBACKCOLOR2;
    }
    int captionHeight = GetCaptionHeight();
	RECT hRect = {0, 0, width, captionHeight};
    paint->FillRect(backColor, hRect);
	RECT lRect = {0, captionHeight, 5, height};
	paint->FillRect(backColor, lRect);
	RECT rRect = {width - 5, captionHeight, width, height};
	paint->FillRect(backColor, rRect);
	RECT bRect = {0, height - 5, width, height};
	paint->FillRect(backColor, bRect);
	RECT contentRect = rect;
    contentRect.top = captionHeight;
    contentRect.bottom -= 5;
    contentRect.left += 5;
    contentRect.right -= 5;
    if (contentRect.right - contentRect.left > 0 && contentRect.bottom - contentRect.top > 0)
    {
		paint->FillRect(PCOLORS_WINDOWCONTENTBACKCOLOR, contentRect);
    }
	POINT location = {5, 5};
	CDraw::DrawText(paint, GetText().c_str(), foreColor, GetFont(), location.x, location.y);
}

void WindowEx::OnTimer(int timerID)
{
	ControlA::OnTimer(timerID);
    if (m_timerID == timerID)
    {
        NativeBase *native = GetNative();
        ControlHost *host = native->GetHost();
        SIZE nativeSize = native->GetDisplaySize();
        int x = GetLeft(), y = GetTop(), width = GetWidth(), height = GetHeight();
		if(m_animateType == 0)
		{
			int xSub = nativeSize.cx / 4;
			int ySub = nativeSize.cy / 4;
			int mx = (nativeSize.cx - width) / 2;
			int my = (nativeSize.cy - height) / 2;
			float opacity = GetOpacity();
			opacity += (float)0.1;
			if(opacity > 1)
			{
				opacity = 1;
			}
			SetOpacity(opacity);
			bool stop = false;
			switch (m_animateDirection)
            {
                case 0:
                    if (x + xSub >= mx)
                    {
                        x = mx;
                        stop = true;
                    }
                    else
                    {
                        x += xSub;
                    }
                    break;
                case 1:
                    if (x - xSub <= mx)
                    {
                        x = mx;
                        stop = true;
                    }
                    else
                    {
                        x -= xSub;
                    }
                    break;
                case 2:
                    if (y + ySub >= my)
                    {
                        y = my;
                        stop = true;
                    }
                    else
                    {
                        y += ySub;
                    }
                    break;
                case 3:
                    if (y - ySub <= my)
                    {
                        y = my;
                        stop = true;
                    }
                    else
                    {
                        y -= ySub;
                    }
                    break;
            }
			if (stop)
			{
				SetOpacity(1);
				m_animateMoving = false; 
				StopTimer(m_timerID);
				host->SetAllowOperate(true);
			}
		}
		else
		{
			int xSub = nativeSize.cx / 4;
			int ySub = nativeSize.cy / 4;
			bool stop = false;
			float opacity = GetOpacity();
			opacity -= 0.1F;
			if(opacity < 0)
			{
				opacity = 0;
			}
			SetOpacity(opacity);
			switch (m_animateDirection)
            {
                case 0:
                    if (x - xSub <= -width)
                    {
                        x = 0;
                        stop = true;
                    }
                    else
                    {
                        x -= xSub;
                    }
                    break;
                case 1:
                    if (x +xSub >= nativeSize.cx)
                    {
                        x = 0;
                        stop = true;
                    }
                    else
                    {
                        x += xSub;
                    }
                    break;
                case 2:
                    if (y - ySub <= -height)
                    {
                        y = 0;
                        stop = true;
                    }
                    else
                    {
                        y -= ySub;
                    }
                    break;
                case 3:
                    if (y + ySub >= nativeSize.cy)
                    {
                        y = 0;
                        stop = true;
                    }
                    else
                    {
                        y += ySub;
                    }
                    break;
            }
            if (stop)
            {
				SetOpacity(0);
				m_animateMoving = false; 
                StopTimer(m_timerID);
                host->SetAllowOperate(true);
                Hide();
            }
		}
		POINT location = {x, y};
        SetLocation(location);
        native->Invalidate();
    }
}

void WindowEx::Update()
{
	ControlA::Update();
    if (m_closeButton)
    {
		POINT location = {GetWidth() - 26, 2};
        m_closeButton->SetLocation(location);
    }
}

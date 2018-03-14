#include "..\\..\\stdafx.h"
#include "BarrageDiv.h"

Barrage::Barrage()
{
	m_color = 0;
	m_font = new FONT(L"SimSun", 40, true, false, false);
	m_mode = 0;
	m_rect.left = 0;
	m_rect.top = 0;
	m_rect.right = 0;
	m_rect.bottom = 0;
	m_speed = 10;
	m_text = L"";
	m_tick = 200;
}

Barrage::~Barrage()
{
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

__int64 Barrage::GetColor()
{
	return m_color;
}

void Barrage::SetColor(__int64 color)
{
	m_color = color;
}

FONT* Barrage::GetFont()
{
	return m_font;
}

void Barrage::SetFONT(FONT *font)
{
	m_font->Copy(font);
}

int Barrage::GetMode()
{
	return m_mode;
}

void Barrage::SetMode(int mode)
{
	m_mode = mode;
}

int Barrage::GetSpeed()
{
	return m_speed;
}

void Barrage::SetSpeed(int speed)
{
	m_speed = speed;
}

RECT Barrage::GetRect()
{
	return m_rect;
}

void Barrage::SetRect(const RECT& rect)
{
	m_rect = rect;
}

String Barrage::GetText()
{
	return m_text;
}

void Barrage::SetText(const String& text)
{
	m_text = text;
}

int Barrage::GetTick()
{
	return m_tick;
}

void Barrage::SetTick(int tick)
{
	m_tick = tick;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Barrage::Calculate()
{
    m_rect.left -= m_speed;
    m_rect.right -= m_speed;   
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

BarrageDiv::BarrageDiv()
{
	m_sysColors[0] = COLOR::ARGB(255, 255, 255);
	m_sysColors[1] = COLOR::ARGB(255,255,0);
	m_sysColors[2] = COLOR::ARGB(255, 0, 255);
    m_sysColors[3] = COLOR::ARGB(0, 255, 0);
	m_sysColors[4] = COLOR::ARGB(82, 255, 255);
	m_sysColors[5] = COLOR::ARGB(255, 82, 82);
	m_tick = 0;
	m_timerID = GetNewTimerID();
	SetBackColor(COLOR_EMPTY);
}

BarrageDiv::~BarrageDiv()
{
	StopTimer(m_timerID);
	m_lock.Lock();
	vector<Barrage*>::iterator sIter = m_barrages.begin();
	for(; sIter != m_barrages.end(); ++sIter)
	{
		delete *sIter;
	}
	m_barrages.clear();
	m_lock.UnLock();
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void BarrageDiv::AddBarrage(Barrage *barrage)
{
	barrage->SetColor(m_sysColors[m_tick % 6]);
	int width = GetWidth(), height = GetHeight();
	if(height < 100)
	{
		height = 100;
	}
	int mode = barrage->GetMode();
	if(barrage->GetMode() == 0)
	{
		RECT rect = {width, rand() % height, width, 0};
		barrage->SetRect(rect);
	}
	else
	{
		int left = 0, top = 0;
		if(width > 200)
		{
			left = rand() % (width - 200);
		}
		if(height > 200)
		{
			top = rand() & (height - 200);
		}
		RECT rect = {left, top, left, 0};
		barrage->SetRect(rect);
	}
	m_lock.Lock();
	m_barrages.push_back(barrage);
	m_lock.UnLock();
	m_tick++;
}

bool BarrageDiv::ContainsPoint(const POINT& point)
{
	return false;
}

void BarrageDiv::OnLoad()
{
	ControlA::OnLoad();
	StartTimer(m_timerID, 10);
}

void BarrageDiv::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	ControlA::OnPaintBackground(paint, clipRect);
	m_lock.Lock();
	vector<Barrage*>::iterator sIter = m_barrages.begin();
	for(; sIter != m_barrages.end(); ++sIter)
	{
		Barrage *brg = *sIter;
		FONT* font = brg->GetFont();
		RECT rect = brg->GetRect();
		String str = brg->GetText();
		SIZE size = paint->TextSize(str.c_str(), font);
		rect.right = rect.left + size.cx;
		rect.bottom = rect.top + size.cy;
		brg->SetRect(rect);
		_int64 color = brg->GetColor();
        int mode = brg->GetMode();
        if (mode == 1)
        {
            int a = 0, r = 0, g = 0, b = 0;
			COLOR::ToARGB(0, color, &a, &r, &g, &b);
            a = a * brg->GetTick() / 400;
			color = COLOR::ARGB(a, r, g, b);
        }
		paint->DrawText(str.c_str(), color, font, rect);
	}
	m_lock.UnLock();
}

void BarrageDiv::OnTimer(int timerID)
{
	ControlA::OnTimer(timerID);
    if (m_timerID == timerID)
	{
		bool paint = false;
		m_lock.Lock();
		int barragesSize = (int)m_barrages.size();
		if(barragesSize > 0)
		{
			int width = GetWidth(), height = GetHeight();
			for(int i = 0; i < barragesSize; i++)
			{
				Barrage *brg = m_barrages[i];
				int mode = brg->GetMode();
				if(mode == 0)
				{
					if(brg->GetRect().right < 0)
					{
						m_barrages.erase(m_barrages.begin() + i);
						i--;
						barragesSize--;
					}
					else
					{
						brg->Calculate();
					}
					paint = true;
				}
				else if(mode == 1)
				{
					int tick = brg->GetTick();
					tick--;
					if(tick <= 0)
					{
						m_barrages.erase(m_barrages.begin() + i);
						i--;
						barragesSize--;
						paint = true;
					}
					else
					{
						brg->SetTick(tick);
					}
					if(tick % 20 == 0)
					{
						paint = true;
					}
				}
			}
		}
		m_lock.UnLock();
		if(paint)
		{
			Invalidate();
		}
	}
}
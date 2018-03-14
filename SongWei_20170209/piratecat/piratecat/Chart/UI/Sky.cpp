#include "..\\..\\stdafx.h"
#include "Sky.h"
#include "..\\Service\\DataCenter.h"

void Sky::CheckAttach()
{
    POINT pLocation = m_plane->GetLocation();
    int pmx = pLocation.x;
    int pmy = pLocation.y;
    bool attach = false;
	int bulletsSize = (int)m_bullets.size();
    for (int i = 0; i < bulletsSize; i++)
	{
        Bullet *bullet = m_bullets[i];
        int bmx = bullet->GetLocation().x;
        int bmy = bullet->GetLocation().y;
        if (bmx >= pmx - 1 && bmx <= pmx + 1 && bmy >= pmy - 1 && bmy <= pmy + 1)
        {
            attach = true;
            break;
        }
    }
	m_tick2 = GetTickCount();
    double num = (double)(m_tick2 - m_tick);
    if (attach)
    {
		for (int i = 0; i < bulletsSize; i++)
		{
			Bullet *bullet = m_bullets[i];
			delete bullet;
		}
        m_bullets.clear();
        m_difficult = 1;
		m_tick = GetTickCount();
    }
    else if ((num / 1000) > m_high)
    {
        m_high = num / 1000;
    }
}

void Sky::CreateBullets()
{
     int size = GetWidth() / 250;
    if (size < 2)
    {
        size = 2;
    }
    for (int i = 0; i < size; i++)
    {
		int pmx = m_plane->GetLocation().x;
        int pmy = m_plane->GetLocation().y;
        Bullet *bullet = new Bullet;
        float k = 0;
        float b = 0;
        while (k == 0 && b == 0)
        {
            int type = rand() % 4;
			POINT location = {0};
            if (type == 0)
            {
				location.x = -5;
				location.y = rand() % 100;
            }
            else if (type == 1)
            {
				location.x = GetWidth() + 5;
				location.y = rand() % 100;
            }
            else
            {
				location.x = rand() % GetWidth() + 5;
				location.y = -5;
            }
			bullet->SetLocation(location);
            M107((float)location.x, (float)location.y, (float)(rand() % GetWidth()), (float)pmy, 0, 0, &k, &b);
        }
        bullet->SetK(k);
        bullet->SetB(b);
        bullet->SetSpeed(rand() % 4 + 2);
        bullet->SetBackColor(m_sysColors[rand() % 6]);
        m_bullets.push_back(bullet);
    }
    for (int i = 0; i < 1; i++)
    {
        if (rand() % 2 == 0)
        {
			int pmx = m_plane->GetLocation().x;
			int pmy = m_plane->GetLocation().y;
            Bullet *bullet = new Bullet;
            float k = 0;
            float b = 0;
            while (k == 0 && b == 0)
            {
                int x = rand() % GetWidth();
				POINT location = {x, -5};
                bullet->SetLocation(location);
                M107((float)x, (float)location.y, (float)pmx, (float)pmy, 0, 0, &k, &b);
            }
			bullet->SetK(k);
			bullet->SetB(b);
            bullet->SetSpeed(rand() % 6 + 3 + m_difficult);
            bullet->SetBackColor(m_sysColors[rand() % 6]);
            m_bullets.push_back(bullet);
        }
    }
}

void Sky::MoveBullets()
{
	if (m_ticks % 2 == 0)
    {
		int bulletsSize = (int)m_bullets.size();
        for (int i = 0; i < bulletsSize; i++)
        {
			Bullet *bullet = m_bullets[i];
			float k = bullet->GetK();
			float b = bullet->GetB();
			int speed = bullet->GetSpeed();
			int y = bullet->GetLocation().y + bullet->GetSpeed();
			int x = (int)((y - b) / k);
			int subX = abs(bullet->GetLocation().x - x);
			int subY = abs(bullet->GetLocation().y - y);
			POINT bmp = bullet->GetLocation();
			if (subX > speed)
			{
				if (bmp.x > x)
				{
					x = bmp.x + speed;
				}
				else if (bmp.x > x)
				{
					x = bmp.x - speed;
				}
			}
			if (subY > speed)
			{
				if (bmp.y > y)
				{
					y = bmp.y + speed;
				}
				else if (bmp.y > y)
				{
					y = bmp.y - speed;
				}
			}
			POINT newLocation = {x, y};
			bullet->SetLocation(newLocation);
        }
    }
}

void Sky::RemoveOutBullets()
{
    int m_bulletsSize = (int)m_bullets.size();
    for (int i = 0; i < m_bulletsSize; i++)
    {
        Bullet *bullet = m_bullets[i];
        POINT bmp = bullet->GetLocation();
        int x = bmp.x;
        int y = bmp.y;
        if ((y < -10) || (y > GetHeight() + 50))
        {
			m_bullets.erase(m_bullets.begin() + i);
			delete bullet;
            i--;
            m_bulletsSize--;
        }
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Sky::Sky()
{
	m_difficult = 1;
	m_high = 0;
	m_plane = 0;
	m_point.x = 0;
	m_point.y = 0;
	m_showRecord = false;
	m_tick = 0;
	m_tick2 = 0;
	m_ticks = 0;
	m_timerID = ControlA::GetNewTimerID();
	SetBackColor(COLOR::ARGB(0, 0, 100, 255));
    SetBorderColor(COLOR_EMPTY);
	FONT font(L"Arial", 16, false, false, false);
    SetFont(&font);
	SetForeColor(COLOR::ARGB(255, 255, 255));
	string dir = DataCenter::GetAppPath() + "\\config";
    String wDir = L"";
	CStr::stringTowstring(wDir, dir);
	SetResourcePath(wDir);
}

Sky::~Sky()
{
	StopTimer(m_timerID);
	int m_bulletsSize = (int)m_bullets.size();
	for (int i = 0; i < m_bulletsSize; i++)
	{
		Bullet *bullet = m_bullets[i];
		delete bullet;
	}
    m_bullets.clear();
}

bool Sky::ShowRecord()
{
	return m_showRecord;
}

void Sky::SetShowRecord(bool showRecord)
{
	m_showRecord = showRecord;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Sky::OnAdd()
{
	ControlA::OnAdd();
    m_plane = new Plane;
	POINT location = {GetWidth() / 2, GetHeight() - 25};
    m_plane->SetLocation(location);
    m_point = location;
	m_sysColors[0] = COLOR::ARGB(255, 255, 255);
	m_sysColors[1] = COLOR::ARGB(255,255,0);
	m_sysColors[2] = COLOR::ARGB(255, 0, 255);
    m_sysColors[3] = COLOR::ARGB(0, 255, 0);
	m_sysColors[4] = COLOR::ARGB(82, 255, 255);
	m_sysColors[5] = COLOR::ARGB(255, 82, 82);
	m_tick = GetTickCount();
	m_tick2 = GetTickCount();
	StartTimer(m_timerID, 10);
}

void Sky::OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	ControlA::OnMouseDown(mp, button, clicks, delta);
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		m_point = mp;
	}
}

void Sky::OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	ControlA::OnMouseMove(mp, button, clicks, delta);
    if (button == MouseButtonsA_Left && clicks == 1)
    {
        POINT location = m_plane->GetLocation();
        location.x += (mp.x - m_point.x);
        location.y += (mp.y - m_point.y);
        m_plane->SetLocation(location);
        m_point = mp;
    }
}

void Sky::OnPaint(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
    int height = GetHeight();
    if (width > 0 && height > 0)
    {
		RECT drawRect = {0, 0, width, height};
        paint->FillRect(GetPaintingBackColor(), clipRect);
		RECT logoRect = {30, 50, 300, 150};
        paint->DrawImage(L"file='Logo.png'", logoRect);
        if (m_showRecord)
        {
			m_tick2 = GetTickCount();
            double num = (double)(m_tick2 - m_tick);
			wchar_t szStr[100] = {0};
			_stprintf_s(szStr, L"HIGH %.3f", m_high);
            FONT *font = GetFont();
            SIZE tSize = paint->TextSize(szStr, font);
			RECT tRect = {GetWidth() / 2 - tSize.cx / 2, 5, GetWidth(), 20};
            paint->DrawText(szStr, GetForeColor(), font, tRect);
			_stprintf_s(szStr, L"TIME %.3f", num / 1000);
			RECT tRect2 = {0, 5, 100, 20};
            paint->DrawText(szStr, GetForeColor(), font, tRect2);
        }
        m_plane->OnPaintBackground(paint);
        int m_bulletsSize = (int)m_bullets.size();
		for (int i = 0; i < m_bulletsSize; i++)
		{
            Bullet *bullet = m_bullets[i];
            bullet->OnPaintBackground(paint);
        }
    }
}

void Sky::OnTimer(int timerID)
{
    if (m_ticks % 5 == 0)
    {
        CreateBullets();
    }
    if (m_ticks % 500 == 0 && m_difficult < 20)
    {
        m_difficult++;
    }
    CheckAttach();
    MoveBullets();
    RemoveOutBullets();
    Invalidate();
    m_ticks++;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Plane::Plane()
{
	m_backColor = COLOR::ARGB(255, 80, 80);
	m_location.x = 0;
	m_location.y = 0;
	m_tick = 1;
	m_tick2 = 2;
}

Plane::~Plane()
{
}

_int64 Plane::GetBackColor()
{
	return m_backColor;
}

void Plane::SetBackColor(_int64 backColor)
{
	m_backColor = backColor;
}

POINT Plane::GetLocation()
{
	return m_location;
}

void Plane::SetLocation(POINT location)
{
	m_location = location;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Plane::OnPaintBackground(CPaint *paint)
{
	RECT drawRect = {m_location.x - 6, m_location.y - 6, m_location.x + 6, m_location.y + 6};
	paint->DrawGradientEllipse(m_backColor, COLOR::RatioColor(paint, m_backColor, 1.1), drawRect, 90);
    drawRect.left -= m_tick;
    drawRect.top -= m_tick;
    drawRect.right += m_tick;
    drawRect.bottom += m_tick;
	paint->FillEllipse(COLOR::ARGB(50, 255, 255, 0), drawRect);
    if (m_tick > 10)
    {
        m_tick = 0;
    }
    if (m_tick2 % 1 == 0)
    {
        m_tick++;
    }
    m_tick2++;
    if (m_tick2 > 1000)
    {
        m_tick2 = 0;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Bullet::Bullet()
{
	m_b = 0;
	m_backColor = COLOR::ARGB(255, 0, 0);
	m_k = 0;
	m_location.x = 0;
	m_location.y = 0;
	m_mode = 0;
	m_speed = 1;
	m_tick = 1;
	m_tick2 = 2;
}

Bullet::~Bullet()
{
}

float Bullet::GetB()
{
	return m_b;
}

void Bullet::SetB(float b)
{
	m_b = b;
}

_int64 Bullet::GetBackColor()
{
	return m_backColor;
}

void Bullet::SetBackColor(_int64 backColor)
{
	m_backColor = backColor;
}

float Bullet::GetK()
{
	return m_k;
}

void Bullet::SetK(float k)
{
	m_k = k;
}

POINT Bullet::GetLocation()
{
	return m_location;
}

void Bullet::SetLocation(POINT location)
{
	m_location = location;
}

int Bullet::GetSpeed()
{
	return m_speed;
}

void Bullet::SetSpeed(int speed)
{
	m_speed = speed;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Bullet::OnPaintBackground(CPaint *paint)
{
    if (m_k != 0 && m_b != 0)
    {
		RECT drawRect = {m_location.x - m_tick, m_location.y - m_tick, m_location.x + m_tick, m_location.y + m_tick};
		paint->DrawGradientEllipse(m_backColor, COLOR::RatioColor(paint, m_backColor, 1.1), drawRect, 90);
        if (m_tick2 % 5 == 0)
        {
            if (m_mode == 0)
            {
                m_tick++;
                if (m_tick > 10)
                {
                    m_mode = 1;
                }
            }
            else if (m_mode == 1)
            {
                m_tick--;
                if (m_tick < 4)
                {
                    m_mode = 0;
                }
            }
        }
        m_tick2++;
        if (m_tick2 > 1000)
        {
            m_tick2 = 0;
        }
    }
}
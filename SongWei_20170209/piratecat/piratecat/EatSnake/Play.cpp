#include "..\\stdafx.h"
#include "Play.h"

Play::Play(Battle* battle)
{
	m_gameState = GameState_Begin;
	m_battle = battle;
	SetBorderColor(COLOR_EMPTY);
	SetForeColor(RGB(255, 255, 255));
	FONT pFont(L"Simsun", 16, true, false, true);
	SetFont(&pFont);
}

Play::~Play()
{
}

GameState Play::GetGameState()
{
	return m_gameState;
}

void Play::SetGameState(GameState gameState)
{
	m_gameState = gameState;
	if (m_gameState == GameState_Playing)
	{
        SetVisible(false);
	}
    else
	{
        SetVisible(true);
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////

void Play::OnKeyDown(char key)
{
	ControlA::OnKeyDown(key);
	if(m_battle)
	{
		m_battle->OnKeyDown(key);
	}
}

void Play::OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	ControlA::OnMouseDown(mp, button, clicks, delta);
	if(m_battle)
	{
		m_battle->OnMouseDown(mp, button, clicks, delta);
	}
}

void Play::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
    int height = GetHeight();
    if (width > 0 && height > 0)
    {
		LPCTSTR text = 0;
		switch(m_gameState)
		{
		case GameState_Lose:
			text = L"游戏失败，按回车重新开始";
			break;
		case GameState_Suspend:
			text = L"游戏暂停，按回车回到游戏";
			break;
		case GameState_Begin:
			text = L"按回车开始游戏";
			break;
		}
		if(text)
		{
			FONT *font = GetFont();
			SIZE textSize = paint->TextSize(text, font);
			RECT tRect = {0};
			tRect.left = width / 2 - textSize.cx / 2;
			tRect.top = height / 2 - textSize.cy / 2;
			tRect.right = tRect.left + textSize.cx;
			tRect.bottom = tRect.top + textSize.cy;
			paint->DrawText(text, GetPaintingForeColor(), font, tRect);
		}
	}
}
#include "..\\stdafx.h"
#include "Battle.h"
void Battle::AddFood()
{
	Food *food = new Food;
	SIZE fSize = {SNAKE_SIZE, SNAKE_SIZE};
	food->SetSize(fSize);
    ControlHost *host = GetNative()->GetHost();
	while (true)
	{
		int num = rand() % (GetWidth() / SNAKE_SIZE);
		int num2 = rand() % (GetHeight() / SNAKE_SIZE);
		POINT location = {num * SNAKE_SIZE, num2 * SNAKE_SIZE};
		food->SetLocation(location);
		bool reset = false;
		RECT fRect = food->GetBounds();
		int bodySize = (int)m_bodys.size();
		for (int i = 0; i < bodySize; i++)
		{
			RECT tempRect = {0};
			RECT bRect = m_bodys[i]->GetBounds();
            if (host->GetIntersectRect(&tempRect, &bRect, &fRect) > 0)
            {
                reset = true;
            }
		}
		int foodSize = (int)m_foods.size();
		for(int i = 0;i < foodSize; i++)
		{
			RECT tempRect = {0};
			RECT fiRect = m_foods[i]->GetBounds();
			if (host->GetIntersectRect(&tempRect, &fiRect, &fRect) > 0)
            {
                reset = true;
            }
		}
		if (!reset)
		{
			break;
		}
	}
	AddControl(food);
	m_foods.push_back(food);
	food->BringToFront();
}

void Battle::ChangeState()
{
	m_play->BringToFront();
    switch (m_play->GetGameState())
    {
        case GameState_Playing:
            m_play->SetGameState(GameState_Suspend);
            break;
        case GameState_Lose:
            ResetSnake();
            m_play->SetGameState(GameState_Playing);
            break;
        case GameState_Suspend:
        case GameState_Begin:
            m_play->SetGameState(GameState_Playing);
            break;
    }
    SetFocused(true);
    Invalidate();
}

bool Battle::IsLose()
{
	ControlHost *host = GetNative()->GetHost();
    SnakeBody *body = m_bodys[0];
    if (body->GetLeft() < 0 || body->GetRight() > GetWidth() 
		|| body->GetTop() < 0 || body->GetBottom() > GetHeight())
    {
        return true;
    }
	int bodySize = (int)m_bodys.size();
	for (int i = 0; i < bodySize; i++)
    {
		for (int j = i + 1; j < bodySize - i - 1; j++)
        {
			RECT tempRect = {0};
			RECT iRect = m_bodys[i]->GetBounds();
			RECT jRect = m_bodys[j]->GetBounds();
			if (host->GetIntersectRect(&tempRect, &iRect, &jRect))
            {
                return true;
            }
        }
    }
    return false;
}

void Battle::MoveSnake()
{
	ControlHost *host = GetNative()->GetHost();
	int bodySize = (int)m_bodys.size();
	SnakeBody *lastBody = m_bodys[bodySize - 1];
    int left = lastBody->GetLeft();
    int top = lastBody->GetTop();
    for (int i = 0; i < bodySize; i++)
    {
        SnakeBody *body = m_bodys[i];
        switch (body->GetDirection())
        {
            case SnakeDirection_Down:
				body->SetTop(body->GetTop() + body->GetHeight());
                break;
            case SnakeDirection_Left:
				body->SetLeft(body->GetLeft() - body->GetWidth());
                break;
            case SnakeDirection_Right:
				body->SetLeft(body->GetLeft() + body->GetWidth());
                break;
            case SnakeDirection_Up:
				body->SetTop(body->GetTop() - body->GetHeight());
                break;
        }
    }
    if (IsLose())
    {
        m_play->SetGameState(GameState_Lose);
        m_play->BringToFront();
    }
    else
    {
        bool newFood = false;
		vector<Food*>::iterator sIter = m_foods.begin();
		for(;sIter != m_foods.end();++sIter)
		{
			Food *food = *sIter;
			RECT tempRect = {0};
            RECT bRect = m_bodys[0]->GetBounds();
            RECT fRect = food->GetBounds();
            if (host->GetIntersectRect(&tempRect, &bRect, &fRect) > 0)
            {
				m_eatCount++;
				RECT rc = {left, top, left + SNAKE_SIZE, top + SNAKE_SIZE};
				SnakeBody *body = new SnakeBody(rc);
				m_bodys.push_back(body);
				AddControl(body);
				body->BringToFront();
				RemoveControl(food);
				m_foods.erase(sIter);
				newFood = true;
				break;
			}
		}
		int bodysSize = (int)m_bodys.size();
        for (int i = bodysSize - 1; i > 0; i--)
        {
            m_bodys[i]->SetDirection(m_bodys[i - 1]->GetDirection());
        }
        if (newFood)
        {
            AddFood();
        }
    }
    Invalidate();
}

void Battle::ResetSnake()
{
	OnRemove();
	StartTimer(m_timerID, m_speed);
	m_eatCount = 0;
	m_play = new Play(this);
	RECT displayRect = {0, 0, GetWidth(), GetHeight()};
	m_play->SetBackColor(COLOR_EMPTY);
	m_play->SetBounds(displayRect);
    AddControl(m_play);
    int left = SNAKE_SIZE * 5;
    int top = SNAKE_SIZE * 5;
    for (int i = 0; i < SNAKE_LENGTH; i++)
    {
		RECT rc = {left + i * SNAKE_SIZE, top, left + (i + 1) * SNAKE_SIZE, top + SNAKE_SIZE};
        SnakeBody *body = new SnakeBody(rc);
        if (i == 0)
        {
            body->SetHeader();
        }
        AddControl(body);
        body->SetDirection(SnakeDirection_Left);
        m_bodys.push_back(body);
        body->BringToFront();
    }
    AddFood();
    Invalidate();
}

Battle::Battle()
{
	m_alpha = 0;
	m_alphaDirection = 0;
	m_eatCount = 0;
	m_play = 0;
	m_speed = 50;
	m_timerID = GetNewTimerID();
	SetBorderColor(RGB(112, 112, 112));
    SetBackColor(RGB(255, 255, 255));
    SetForeColor(RGB(255, 255, 255));
	FONT bFont(L"Arial", 100, false, false, false);
	SetFont(&bFont);
}

Battle::~Battle()
{
	OnRemove();
}

int Battle::GetSpeed()
{
	return m_speed;
}

void Battle::SetSpeed(int speed)
{
	m_speed = speed;
	StartTimer(m_timerID, m_speed);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Battle::OnLoad()
{
	ControlA::OnLoad();
    ResetSnake();
}

void Battle::OnKeyDown(char key)
{
	ControlA::OnKeyDown(key);
    ControlHost *host = GetNative()->GetHost();
    if (!host->IsKeyPress(0x10) && !host->IsKeyPress(0x11) && !host->IsKeyPress(0x12))
    {
		if (m_play->GetGameState() == GameState_Playing)
		{
			SnakeBody *body = m_bodys[0];
			switch(key)
			{
			case 37:
				if (body->GetDirection() != SnakeDirection_Right)
				   body->SetDirection(SnakeDirection_Left);
				break;
			case 39:
				if (body->GetDirection() != SnakeDirection_Left)
					body->SetDirection(SnakeDirection_Right);
				break;
			case 38:
				if (body->GetDirection() != SnakeDirection_Down)
					body->SetDirection(SnakeDirection_Up);
				break;
			case 40:
				if (body->GetDirection() != SnakeDirection_Up)
				   body->SetDirection(SnakeDirection_Down);
				break;
			}
		}
        if (key == 13)
		{
			ChangeState();
		}
	}
}

void Battle::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
    int height = GetHeight();
    if (width > 0 && height > 0)
    {
		RECT drawRect = {0, 0, width, height};
		paint->DrawGradientRect(COLOR::ARGB(200, 90, 120, 24), COLOR::ARGB(200, 122, 156, 40), drawRect, 0, 90);
		if (m_play->GetGameState() != GameState_Begin)
		{
			FONT *font = GetFont();
			wchar_t strScore[20] = {0};
			_stprintf_s(strScore, 19, L"%d", m_eatCount);
			SIZE tSize = paint->TextSize(strScore, font);
			if (m_play->GetGameState() == GameState_Playing)
			{
				if (m_alphaDirection == 0)
				{
					m_alpha -= 5;
				}
				else if (m_alphaDirection == 1)
				{
					m_alpha += 5;
				}
				if (m_alpha > 50)
				{
					m_alphaDirection = 0;
					m_alpha = 50;
				}
				else if (m_alpha < 20)
				{
					m_alphaDirection = 1;
					m_alpha = 20;
				}
			}
			else
			{
				m_alpha = 50;
			}
			RECT tRect = {0};
			tRect.left = drawRect.right / 2 - tSize.cx / 2;
			tRect.top = drawRect.bottom / 2 - tSize.cy / 2;
			tRect.right = tRect.left + tSize.cx;
			tRect.bottom = tRect.top + tSize.cy;
			paint->DrawText(strScore, COLOR::ARGB(m_alpha * 3, 255, 255, 255), font, tRect);
		}
	}
}

void Battle::OnRemove()
{
	ControlA::OnRemove();
	StopTimer(m_timerID);
	int foodSize = (int)m_foods.size();
	for (int i = 0; i < foodSize; i++)
    {
        RemoveControl(m_foods[i]);
		delete m_foods[i];
    }
	m_foods.clear();
	if(m_play)
	{
		RemoveControl(m_play);
		delete m_play;
		m_play = 0;
	}
	int bodySize = (int)m_bodys.size();
	for (int i = 0; i < bodySize; i++)
    {
        RemoveControl(m_bodys[i]);
		delete m_bodys[i];
    }
    m_bodys.clear();
}

void Battle::OnTimer(int timerID)
{
	ControlA::OnTimer(timerID);
    if (m_timerID == timerID)
    {
		if(m_play)
		{
			if (m_play->GetGameState() == GameState_Playing)
			{
				MoveSnake();
			}
		}
    }
}
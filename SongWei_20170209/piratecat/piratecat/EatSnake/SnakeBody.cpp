#include "..\\stdafx.h"
#include "SnakeBody.h"

SnakeBody::SnakeBody(const RECT& rect)
{
	m_isHeader = false;
	m_direction = SnakeDirection_Left;
	SetBackColor(RGB(20, 20, 20));
	SetBorderColor(COLOR_EMPTY);
	SetBounds(rect);
	SetCanFocus(false);
}

SnakeBody::~SnakeBody()
{
}

SnakeDirection SnakeBody::GetDirection()
{
	return m_direction;
}

void SnakeBody::SetDirection(SnakeDirection direction)
{
	m_direction = direction;
}

void SnakeBody::SetHeader()
{
	m_isHeader = true;
}

void SnakeBody::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
	int height = GetHeight();
	if (width > 0 && height > 0)
	{
		RECT drawRect = {0, 0, width, width};
        _int64 paintingBackColor = GetPaintingBackColor();
		paint->DrawGradientEllipse(paintingBackColor, COLOR::RatioColor(paint, paintingBackColor, 1.1), drawRect, 90);
		if (m_isHeader)
		{
			POINT eyeLeft = {drawRect.left, drawRect.top};
			POINT eyeRight = {drawRect.left, drawRect.top};
			switch (m_direction)
			{
				case SnakeDirection_Down:
					eyeLeft.x += width / 3;
					eyeLeft.y += height * 2 / 3;
					eyeRight.x += width *2 / 3;
					eyeRight.y += height * 2 / 3;
					break;
				case SnakeDirection_Left:
					eyeLeft.x += width / 3;
					eyeLeft.y += height / 3;
					eyeRight.x += width / 3;
					eyeRight.y += height * 2 / 3;
					break;
				case SnakeDirection_Right:
					eyeLeft.x += width * 2 / 3;
					eyeLeft.y += height * 2 / 3;
					eyeRight.x += width *2 / 3;
					eyeRight.y += height / 3;
					break;
				case SnakeDirection_Up:
					eyeLeft.x += width / 3;
					eyeLeft.y += height / 3;
					eyeRight.x += width *2 / 3;
					eyeRight.y += height / 3;
					break;
			}
			RECT lRect = {eyeLeft.x - 2, eyeLeft.y - 2, eyeLeft.x + 2, eyeLeft.y + 2};
			paint->FillEllipse(RGB(255, 255, 255), lRect);
			RECT rRect = {eyeRight.x - 2, eyeRight.y - 2, eyeRight.x + 2, eyeRight.y + 2};
			paint->FillEllipse(RGB(255, 255, 255), rRect);
		}
	}
}
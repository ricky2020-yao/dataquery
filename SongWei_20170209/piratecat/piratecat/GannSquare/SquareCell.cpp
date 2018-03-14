#include "..\\stdafx.h"
#include "SquareCell.h"

namespace OwLib
{
	SquareCell::SquareCell()
	{
		m_bounds.left = 0;
		m_bounds.top = 0;
		m_bounds.right = 0;
		m_bounds.bottom = 0;
		m_number = 0;
		m_selected = false;
		m_style = SquareCellStyle_Normal;
	}

	SquareCell::~SquareCell()
	{
	}

	_int64 SquareCell::GetBackColor()
	{
		if(m_selected)
		{
			return m_gannSquare->GetSelectedColor();
		}
		else
		{
			if(m_style == SquareCellStyle_Major)
			{
				return m_gannSquare->GetMajorColor();
			}
			else if(m_style == SquareCellStyle_Minor)
			{
				return m_gannSquare->GetMinorColor();
			}
			else
			{
				return m_gannSquare->GetNormalColor();
			}
		}
	}

	RECT SquareCell::GetBounds()
	{
		return m_bounds;
	}

	void SquareCell::SetBounds(RECT bounds)
	{
		m_bounds = bounds;
	}

	_int64 SquareCell::GetForeColor()
	{
		return COLOR::ARGB(0, 0, 0);
	}

	int SquareCell::GetNumber()
	{
		return m_number;
	}

	void SquareCell::SetNumber(int number)
	{
		m_number = number;
	}

	bool SquareCell::IsSelected()
	{
		return m_selected;
	}

	void SquareCell::SetSelected(bool selected)
	{
		m_selected = selected;
	}

	SquareCellStyle SquareCell::GetStyle()
	{
		return m_style;
	}

	void SquareCell::SetStyle(SquareCellStyle style)
	{
		m_style = style;
	}

	double SquareCell::GetValue()
	{
		if(m_gannSquare->GetBolsterOrStress() == BolsterOrStress_Bolster)
		{
			return m_gannSquare->GetStartValue() - m_gannSquare->GetUnit() * (m_number - 1);
		}
		else
		{
			return m_gannSquare->GetStartValue() + m_gannSquare->GetUnit() * (m_number - 1);
		}
	}

	void SquareCell::OnPaint()
	{
		int width = m_bounds.right - m_bounds.left;
		int height = m_bounds.bottom - m_bounds.top;
		NativeBase *native = m_gannSquare->GetNative();
		CPaint *paint = native->GetPaint();
		paint->FillRect(GetBackColor(), m_bounds);
		wchar_t text[20] = {0};
		_stprintf_s(text, 19, L"%.2f", GetValue());
		SIZE textSize = paint->TextSize(text, m_gannSquare->GetCellFont());
		int tRectL = m_bounds.left + width / 2 - textSize.cx / 2;
		int tRectT = m_bounds.top + height / 2 - textSize.cy / 2;
		int tRectR = tRectL + textSize.cx;
		int tRectB = tRectT + textSize.cy;
		RECT tRect = {tRectL, tRectT, tRectR, tRectB};
		paint->DrawText(text, GetForeColor(), m_gannSquare->GetCellFont(), tRect);
		paint->DrawLine(COLOR::ARGB(167, 170, 178), 1, 0, m_bounds.left, m_bounds.top, m_bounds.right, m_bounds.top);
		paint->DrawLine(COLOR::ARGB(167, 170, 178), 1, 0, m_bounds.right, m_bounds.top, m_bounds.right, m_bounds.bottom);
	}
}
#include "..\\stdafx.h"
#include "GannSquare.h"

namespace OwLib
{
	GannSquare::GannSquare()
	{
		m_bolsterOrStress = BolsterOrStress_Bolster;
		m_cellFont = new FONT(L"Arial", 14, true, false, false);
		m_cellSize.cx = 40;
		m_cellSize.cy = 20;
		m_digit = 2;
		m_majorColor = COLOR::ARGB(255, 82, 82);
		m_minorColor = COLOR::ARGB(255, 255, 0);
		m_normalColor = COLOR::ARGB(255, 255, 255);
		m_number = 9;
		m_selectedColor = COLOR::ARGB(0, 255, 255);
		m_startValue = 0;
		m_style = SquareStyle_Normal;
		m_suffix = L"";
		m_unit = 0.01;
		SetBackColor(COLOR::ARGB(255, 255, 255));
		SIZE size = {200, 200};
		SetSize(size);
		ReCreate();
	}

	GannSquare::~GannSquare()
	{
		if(m_cellFont)
		{
			delete m_cellFont;
			m_cellFont = 0;
		}
		vector<SquareCell*>::iterator sIter = m_cells.begin();
		for(;sIter != m_cells.end();++sIter)
		{
			SquareCell *cell = *sIter;
			delete cell;
		}
		m_cells.clear();
	}

	BolsterOrStress GannSquare::GetBolsterOrStress()
	{
		return m_bolsterOrStress;
	}

	void GannSquare::SetBolsterOrStress(BolsterOrStress bolsterOrStress)
	{
		m_bolsterOrStress = bolsterOrStress;
	}

	FONT* GannSquare::GetCellFont()
	{
		return m_cellFont;
	}

	void GannSquare::SetCellFont(FONT* cellFont)
	{
		if(m_cellFont)
		{
			delete m_cellFont;
		}
		m_cellFont = cellFont;
	}

	SIZE GannSquare::GetCellSize()
	{
		return m_cellSize;
	}

	void GannSquare::SetCellSize(SIZE cellSize)
	{
		m_cellSize = cellSize;
	}

	int GannSquare::GetDigit()
	{
		return m_digit;
	}

	void GannSquare::SetDigit(int digit)
	{
		m_digit = digit;
	}

	_int64 GannSquare::GetMajorColor()
	{
		return m_majorColor;
	}

	void GannSquare::SetMajorColor(_int64 majorColor)
	{
		m_majorColor = majorColor;
	}

	_int64 GannSquare::GetMinorColor()
	{
		return m_minorColor;
	}

	void GannSquare::SetMinorColor(_int64 minorColor)
	{
		m_minorColor = minorColor;
	}

	_int64 GannSquare::GetNormalColor()
	{
		return m_normalColor;
	}

	void GannSquare::SetNormalColor(_int64 normalColor)
	{
		m_normalColor = normalColor;
	}

	int GannSquare::GetNumber()
	{
		return m_number;
	}

	void GannSquare::SetNumber(int number)
	{
		if (number > 0)
		{
			if (number % 2 == 0)
			{
				number += 1;
			}
			m_number = number;
		}
	}

	_int64 GannSquare::GetSelectedColor()
	{
		return m_selectedColor;
	}

	void GannSquare::SetSelectedColor(_int64 selectedColor)
	{
		m_selectedColor = selectedColor;
	}

	double GannSquare::GetSelectedValue()
	{
		vector<SquareCell*>::iterator sIter = m_cells.begin();
		for(;sIter != m_cells.end();++sIter)
		{
			SquareCell *cell = *sIter;
			if(cell->IsSelected())
			{
				return cell->GetValue();
			}
		}
		return m_startValue;
	}

	double GannSquare::GetStartValue()
	{
		return m_startValue;
	}

	void GannSquare::SetStartValue(double startValue)
	{
		m_startValue = startValue;
	}

	SquareStyle GannSquare::GetStyle()
	{
		return m_style;
	}

	void GannSquare::SetStyle(SquareStyle style)
	{
		m_style = style;
	}

	String GannSquare::GetSuffix()
	{
		return m_suffix;
	}

	void GannSquare::SetSuffix(const String& suffix)
	{
		m_suffix = suffix;
	}

	double GannSquare::GetUnit()
	{
		return m_unit;
	}

	void GannSquare::SetUnit(double unit)
	{
		m_unit = unit;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void GannSquare::GetProperty(const String& name, String *value, String *type)
	{
	    if (name == L"bolsterorstress")
        {
            *type = L"enum:BolsterOrStress";
            BolsterOrStress bolsterOrStess = GetBolsterOrStress();
            if (bolsterOrStess == BolsterOrStress_Bolster)
            {
                *value = L"bolster";
            }
            else if (bolsterOrStess == BolsterOrStress_Stress)
            {
                *value = L"stress";
            }
        }
        else if (name == L"cellfont")
        {
            *type = L"font";
			*value = CStr::ConvertFontToStr(GetCellFont());
        }
        else if (name == L"cellsize")
        {
            *type = L"size";
			*value = CStr::ConvertSizeToStr(GetCellSize());
        }
        else if (name == L"digit")
        {
            *type = L"int";
			*value = CStr::ConvertIntToStr(GetDigit());
        }
        else if (name == L"majorcolor")
        {
            *type = L"color";
			*value = CStr::ConvertColorToStr(GetMajorColor());
        }
        else if (name == L"minorcolor")
        {
            *type = L"color";
			*value = CStr::ConvertColorToStr(GetMinorColor());
        }
        else if (name == L"normalcolor")
        {
            *type = L"color";
			*value = CStr::ConvertColorToStr(GetNormalColor());
        }
        else if (name == L"number")
        {
            *type = L"int";
			*value = CStr::ConvertIntToStr(GetNumber());
        }
        else if (name == L"selectedcolor")
        {
            *type = L"color";
			*value = CStr::ConvertColorToStr(GetSelectedColor());
        }
        else if (name == L"startvalue")
        {
            *type = L"double";
			*value = CStr::ConvertDoubleToStr(GetStartValue());
        }
        else if (name == L"style")
        {
            *type = L"enum:SquareStyle";
            SquareStyle style = GetStyle();
            if (style == SquareStyle_Normal)
            {
                *value = L"normal";
            }
            else if (style == SquareStyle_Spiral)
            {
                *value = L"spiral";
            }
        }
		else if (name == L"suffix")
		{
			*type = L"text";
			*value = GetSuffix();
		}
        else if (name == L"unit")
        {
            *type = L"double";
			*value = CStr::ConvertDoubleToStr(GetUnit());
        }
        else
        {
			ControlA::GetProperty(name, value, type);
        }
	}

	vector<String> GannSquare::GetPropertyNames()
	{
		vector<String> propertyNames = ControlA::GetPropertyNames();
		propertyNames.push_back(L"BolsterOrStress");
		propertyNames.push_back(L"CellFont");
		propertyNames.push_back(L"CellSize");
		propertyNames.push_back(L"Digit");
		propertyNames.push_back(L"MajorColor");
		propertyNames.push_back(L"MinorColor");
		propertyNames.push_back(L"NormalColor");
		propertyNames.push_back(L"Number");
		propertyNames.push_back(L"SelectedColor");
		propertyNames.push_back(L"StartValue");
		propertyNames.push_back(L"Style");
		propertyNames.push_back(L"Suffix");
		propertyNames.push_back(L"Unit");
		return propertyNames;
	}

	void GannSquare::OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta)
	{
		ControlA::OnClick(mp, button, clicks, delta);
		vector<SquareCell*>::iterator sIter = m_cells.begin();
		for(;sIter != m_cells.end();++sIter)
		{
			SquareCell *cell = *sIter;
			RECT bounds = cell->GetBounds();
			if(mp.x >= bounds.left&& mp.x <= bounds.right
				&& mp.y >= bounds.top && mp.y <= bounds.bottom)
			{
				cell->SetSelected(true);
			}
			else
			{
				cell->SetSelected(false);
			}
		}
		Invalidate();
	}

	void GannSquare::OnPaintBackground(CPaint *paint, const RECT& clipRect)
	{
		int width = GetWidth(), height = GetHeight();
		RECT rect = {0, 0, width, height};
		paint->FillRect(GetPaintingBackColor(), rect);
		vector<SquareCell*>::iterator sIter = m_cells.begin();
		for(;sIter != m_cells.end();++sIter)
		{
			SquareCell *cell = *sIter;
			cell->OnPaint();
		}
		int left = (width - m_number * m_cellSize.cx) / 2;
		int top = (height - m_number * m_cellSize.cy) / 2 + 1;
		int w = m_number * m_cellSize.cx;
		int h = m_number * m_cellSize.cy;
		paint->DrawLine(COLOR::ARGB(167, 170, 178), 1, 0, left, top, left, top + h);
		paint->DrawLine(COLOR::ARGB(167, 170, 178), 1, 0, left, top + h, left + w, top + h);
	}

	void GannSquare::ReCreate()
	{
		vector<SquareCell*>::iterator sIter = m_cells.begin();
		for(;sIter != m_cells.end();++sIter)
		{
			SquareCell *cell = *sIter;
			delete cell;
		}
		m_cells.clear();
		for (int i = 1; i <= m_number * m_number; i++)
		{
			SquareCell *cell = new SquareCell();
			cell->m_gannSquare = this;
			cell->SetNumber(i);
			m_cells.push_back(cell);
		}
		Update();
	}

	void GannSquare::SetProperty(const String& name, const String& value)
	{
		if (name == L"bolsterorstress")
        {
			String str = CStr::ToLower(value);
            if (value == L"bolster")
            {
                SetBolsterOrStress(BolsterOrStress_Bolster);
            }
            else if (value == L"stress")
            {
                SetBolsterOrStress(BolsterOrStress_Stress);
            }
        }
        else if (name == L"cellfont")
        {
			SetCellFont(CStr::ConvertStrToFont(value));
        }
        else if (name == L"cellsize")
        {
			SetCellSize(CStr::ConvertStrToSize(value));
        }
        else if (name == L"digit")
        {
			SetDigit(CStr::ConvertStrToInt(value));
        }
        else if (name == L"majorcolor")
        {
			SetMajorColor(CStr::ConvertStrToColor(value));
        }
        else if (name == L"minorcolor")
        {
			SetMinorColor(CStr::ConvertStrToColor(value));
        }
        else if (name == L"normalcolor")
        {
			SetNormalColor(CStr::ConvertStrToColor(value));
        }
        else if (name == L"number")
        {
			SetNumber(CStr::ConvertStrToInt(value));
        }
        else if (name == L"selectedcolor")
        {
			SetSelectedColor(CStr::ConvertStrToColor(value));
        }
        else if (name == L"startvalue")
        {
			SetStartValue(CStr::ConvertStrToDouble(value));
        }
        else if (name == L"style")
        {
            String str = CStr::ToLower(value);
            if (str == L"normal")
            {
                SetStyle(SquareStyle_Normal);
            }
            else if (str == L"spiral")
            {
                SetStyle(SquareStyle_Spiral);
            }
        }
		else if(name == L"suffix")
		{
			SetSuffix(value);
		}
        else if (name == L"unit")
        {
			SetUnit(CStr::ConvertStrToDouble(value));
        }
        else
        {
			ControlA::SetProperty(name, value);
        }
	}

	void GannSquare::Update()
	{
		if(!GetNative())
		{
			return;
		}
		int mid = m_number / 2 + 1;
		int midnum = m_number * m_number / 2 + 1;
		int w = GetWidth();
		int h = GetHeight();
		int left = (w - m_number * m_cellSize.cx) / 2;
		int top = (h - m_number * m_cellSize.cy) / 2;
		int num = 0;
		int state = 0;
		vector<int> list;
		if (m_style == SquareStyle_Spiral)
		{
			if (m_cells.size() > 2)
			{
				int n = 2;
				while (n <= (int)m_cells.size())
				{
					list.push_back(n);
					int last = (int)list.size() > 1 ? list[(int)list.size() - 2] : 2;
					n += list[(int)list.size() - 1] - last + 8;
				}
			}
		}
		vector<SquareCell*>::iterator sIter = m_cells.begin();
		int cellSize = (int)m_cells.size();
		for (int i = 0;i < cellSize; i++, sIter++)
		{
			SquareCell *cell = *sIter;
			if (m_style == SquareStyle_Spiral)
			{
				if (i == 1)
				{
					state = 0;
				}
				else if (find(list.begin(),list.end(),i)!=list.end())
				{
					state = 1;
				}
				if (i == 0)
				{
					num = midnum;
				}
				else
				{
					switch (state)
					{
						case 0:
							num = num - m_number;
							break;
						case 1:
							num = num + 1;
							break;
						case 2:
							num = num + m_number;
							break;
						case 3:
							num = num - 1;
							break;
					}
				}

			}
			else
			{
				num = cell->GetNumber();
			}
			int cindex = (num - 1) / m_number;
			int rindex = m_number - (num % m_number == 0 ? m_number : num % m_number);
			RECT cellRect;
			cellRect.left = left + m_cellSize.cx * cindex;
			cellRect.top = top + m_cellSize.cy * rindex;
			cellRect.right = cellRect.left + m_cellSize.cx;
			cellRect.bottom = cellRect.top + m_cellSize.cy;
			cell->SetBounds(cellRect);
			if (num == cindex * (m_number + 1) + 1 || num == cindex * (m_number - 1) + m_number)
			{
				cell->SetStyle(SquareCellStyle_Major);
				if (m_style == SquareStyle_Spiral)
				{
					if (num == cindex * (m_number + 1) + 1)
					{
						if (num >= midnum)
						{
							state = 3;
						}
					}
					else if (num == cindex * (m_number - 1) + m_number)
					{
						if (num < midnum)
						{
							state = 2;
						}
						else
						{
							state = 0;
						}
					}
				}
			}
			else if ((num - mid) % m_number == 0 || (num >= (mid - 1) * m_number + 1 && num <= mid * m_number))
			{
				cell->SetStyle(SquareCellStyle_Minor);
			}
		}
	}
}
#include "..\\..\\stdafx.h"
#include "CDraw.h"

_int64 CDraw::GetBlackColor(_int64 color)
{
    if (color > PCOLORS_USERCOLOR)
    {
        if (color == COLOR_CONTROL)
        {
			color = COLOR::ARGB(100, 0, 0, 0);
        }
        else if (color == COLOR_CONTROLBORDER)
        {
            color = 3289650;
        }
        else if (color == COLOR_CONTROLTEXT)
        {
			color = COLOR::ARGB(255, 255, 255);
        }
        else if (color == COLOR_DISABLEDCONTROL)
        {
            color = COLOR::ARGB(50, 255, 255, 255);
        }
        else if (color == COLOR_DISABLEDCONTROLTEXT)
        {
            color = 3289650;
        }
		else if(color == COLOR_HOVEREDCONTROL)
		{
			color = COLOR::ARGB(50, 255, 255, 255);
		}
		else if(color == COLOR_PUSHEDCONTROL)
		{
			color = COLOR::ARGB(50, 255, 255, 255);
		}
    }
	if (color == PCOLORS_BACKCOLOR)
    {
		color = COLOR::ARGB(255, 50, 50, 50);
    }
    else if (color == PCOLORS_BACKCOLOR2)
    {
		color = COLOR::ARGB(150, 0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR3)
    {
		color = COLOR::ARGB(100, 0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR4)
    {
        color = COLOR::ARGB(0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR5)
    {
		color = COLOR::ARGB(25, 255, 255, 255);
    }
    else if (color == PCOLORS_BACKCOLOR6)
    {
		color = COLOR::ARGB(25, 0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR7)
    {
		color = COLOR::ARGB(200, 255, 255, 255);
    }
    else if (color == PCOLORS_FORECOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_FORECOLOR2)
    {
        color = COLOR::ARGB(217, 217, 68);
    }
    else if (color == PCOLORS_FORECOLOR3)
    {
        color = COLOR::ARGB(80, 255, 255);
    }
    else if (color == PCOLORS_FORECOLOR4)
    {
        color = COLOR::ARGB(112, 112, 112);
    }
    else if (color == PCOLORS_FORECOLOR5)
    {
        color = COLOR::ARGB(192, 192, 192);
    }
    else if (color == PCOLORS_FORECOLOR6)
    {
        color = COLOR::ARGB(0, 0, 0);
    }
    else if (color == PCOLORS_FORECOLOR7)
    {
        color = COLOR::ARGB(0, 255, 255);
    }
	else if (color == PCOLORS_FORECOLOR8)
    {
		color = COLOR::ARGB(50, 50, 50);
    }
	else if (color == PCOLORS_FORECOLOR9)
	{
		color = COLOR::ARGB(255, 255, 0);
	}
    else if (color == PCOLORS_FORECOLOR10)
    {
        color = COLOR::ARGB(255, 255, 80);
    }
    else if (color == PCOLORS_FORECOLOR11)
    {
        color = COLOR::ARGB(135, 206, 235);
    }
    else if (color == PCOLORS_LINECOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_LINECOLOR2)
    {
        color = COLOR::ARGB(217, 217, 68);
    }
    else if (color == PCOLORS_LINECOLOR3)
    {
		color = COLOR::ARGB(50, 255, 255, 255);
    }
    else if (color == PCOLORS_LINECOLOR4)
    {
        color = COLOR::ARGB(150, 0, 0);
    }
	else if(color == PCOLORS_LINECOLOR5)
	{
		color = COLOR::ARGB(100, 100, 100);
	}
    else if (color == PCOLORS_MIDCOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_UPCOLOR)
    {
        color = COLOR::ARGB(255, 82, 82);
    }
    else if (color == PCOLORS_DOWNCOLOR)
    {
        color = COLOR::ARGB(80, 255, 80);
    }
    else if (color == PCOLORS_DOWNCOLOR2)
    {
        color = COLOR::ARGB(80, 255, 255);
    }
	else if (color == PCOLORS_DOWNCOLOR3)
    {
        color = COLOR::ARGB(100, 0, 255);
    }
    else if (color == PCOLORS_SELECTEDROWCOLOR)
    {
        color = COLOR::ARGB(150, 100, 100, 100);
    }
    else if (color == PCOLORS_HOVEREDROWCOLOR)
    {
		color = COLOR::ARGB(150, 150, 150, 150);
    }
    else if (color == PCOLORS_WINDOWFORECOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_WINDOWBACKCOLOR)
    {
		color = COLOR::ARGB(255, 50, 50, 50);
    }
    else if (color == PCOLORS_WINDOWBACKCOLOR2)
    {
		color = COLOR::ARGB(200, 20, 20, 20);
    }
    else if (color == PCOLORS_WINDOWCONTENTBACKCOLOR)
    {
		color = COLOR::ARGB(200, 0, 0, 0);
    }
    return color;
}


_int64 CDraw::GetWhiteColor(_int64 color)
{
    if (color > PCOLORS_USERCOLOR)
    {
        if (color == COLOR_CONTROL)
        {
			color = COLOR::ARGB(150, 255, 255, 255);
        }
        else if (color == COLOR_CONTROLBORDER)
        {
            color = 3289650;
        }
        else if (color == COLOR_CONTROLTEXT)
        {
            color = 0;
        }
        else if (color == COLOR_DISABLEDCONTROL)
        {
            color = 13158600;
        }
        else if (color == COLOR_DISABLEDCONTROLTEXT)
        {
            color = 3289650;
        }
		else if(color == COLOR_HOVEREDCONTROL)
		{
			color = 13158600;
		}
		else if(color == COLOR_PUSHEDCONTROL)
		{
			color = 9868950;
		}
    }
	if (color == PCOLORS_BACKCOLOR)
    {
		color = COLOR::ARGB(255, 50, 50, 50);
    }
    else if (color == PCOLORS_BACKCOLOR2)
    {
		color = COLOR::ARGB(150, 0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR3)
    {
		color = COLOR::ARGB(100, 0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR4)
    {
        color = COLOR::ARGB(0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR5)
    {
		color = COLOR::ARGB(10, 255, 255, 255);
    }
    else if (color == PCOLORS_BACKCOLOR6)
    {
		color = COLOR::ARGB(25, 0, 0, 0);
    }
    else if (color == PCOLORS_BACKCOLOR7)
    {
		color = COLOR::ARGB(200, 255, 255, 255);
    }
    else if (color == PCOLORS_FORECOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_FORECOLOR2)
    {
        color = COLOR::ARGB(217, 217, 68);
    }
    else if (color == PCOLORS_FORECOLOR3)
    {
        color = COLOR::ARGB(80, 255, 255);
    }
    else if (color == PCOLORS_FORECOLOR4)
    {
        color = COLOR::ARGB(112, 112, 112);
    }
    else if (color == PCOLORS_FORECOLOR5)
    {
        color = COLOR::ARGB(192, 192, 192);
    }
    else if (color == PCOLORS_FORECOLOR6)
    {
        color = COLOR::ARGB(0, 0, 0);
    }
    else if (color == PCOLORS_FORECOLOR7)
    {
        color = COLOR::ARGB(0, 255, 255);
    }
	else if (color == PCOLORS_FORECOLOR8)
    {
		color = COLOR::ARGB(50, 50, 50);
    }
	else if (color == PCOLORS_FORECOLOR9)
	{
		color = COLOR::ARGB(255, 255, 0);
	}
    else if (color == PCOLORS_FORECOLOR10)
    {
        color = COLOR::ARGB(255, 255, 80);
    }
    else if (color == PCOLORS_FORECOLOR11)
    {
        color = COLOR::ARGB(135, 206, 235);
    }
    else if (color == PCOLORS_LINECOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_LINECOLOR2)
    {
        color = COLOR::ARGB(217, 217, 68);
    }
    else if (color == PCOLORS_LINECOLOR3)
    {
        color = COLOR::ARGB(0, 0, 0);
    }
    else if (color == PCOLORS_LINECOLOR4)
    {
        color = COLOR::ARGB(150, 0, 0);
    }
	else if(color == PCOLORS_LINECOLOR5)
	{
		color = COLOR::ARGB(100, 100, 100);
	}
    else if (color == PCOLORS_MIDCOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_UPCOLOR)
    {
        color = COLOR::ARGB(255, 82, 82);
    }
    else if (color == PCOLORS_DOWNCOLOR)
    {
        color = COLOR::ARGB(80, 255, 80);
    }
    else if (color == PCOLORS_DOWNCOLOR2)
    {
        color = COLOR::ARGB(80, 255, 255);
    }
	else if (color == PCOLORS_DOWNCOLOR3)
    {
        color = COLOR::ARGB(100, 0, 255);
    }
    else if (color == PCOLORS_SELECTEDROWCOLOR)
    {
        color = COLOR::ARGB(150, 100, 100, 100);
    }
    else if (color == PCOLORS_HOVEREDROWCOLOR)
    {
		color = COLOR::ARGB(150, 150, 150, 150);
    }
    else if (color == PCOLORS_WINDOWFORECOLOR)
    {
        color = COLOR::ARGB(255, 255, 255);
    }
    else if (color == PCOLORS_WINDOWBACKCOLOR)
    {
		color = COLOR::ARGB(255, 50, 50, 50);
    }
    else if (color == PCOLORS_WINDOWBACKCOLOR2)
    {
		color = COLOR::ARGB(150, 0, 0, 0);
    }
    else if (color == PCOLORS_WINDOWCONTENTBACKCOLOR)
    {
		color = COLOR::ARGB(200, 255, 255, 255);
    }
    return color;
}

SIZE CDraw::DrawText(CPaint *paint, LPCWSTR strText, _int64 dwPenColor, FONT *font, int x, int y)
{
	SIZE textSize = paint->TextSize(strText, font);
	RECT rect = {x, y, x + textSize.cx, y + textSize.cy};
	paint->DrawText(strText, dwPenColor, font, rect);
	return textSize;
}

int CDraw::DrawUnderLineNum(CPaint *paint, double value, int digit, FONT *font, _int64 fontColor, bool zeroAsEmpty, int x, int y)
{
	if(zeroAsEmpty && value == 0)
	{
		String text = L"-";
		SIZE size = paint->TextSize(text.c_str(), font);
		RECT tRect = {x, y, x + size.cx, y + size.cy};
		paint->DrawText(text.c_str(), fontColor, font, tRect);
		return size.cx;	
	}
	else
	{
		wchar_t strValue[100] = {0};
		CStr::GetValueByDigit(value, digit, strValue);
		vector<String> nbs = CStr::Split(strValue, L".");
		if(nbs.size() == 1)
		{
			SIZE size = paint->TextSize(nbs[0].c_str(), font);
			RECT tRect = {x, y, x + size.cx, y + size.cy};
			paint->DrawText(nbs[0].c_str(), fontColor, font, tRect);
			return size.cx;
		}
		else
		{
			SIZE decimalSize = paint->TextSize(nbs[0].c_str(), font);
			SIZE size = paint->TextSize(nbs[1].c_str(), font);
			CDraw::DrawText(paint, nbs[0].c_str(), fontColor, font, x, y);
			CDraw::DrawText(paint, nbs[1].c_str(), fontColor, font, x
                + decimalSize.cx + 1, y);
            paint->DrawLine(fontColor, 1, 0, x
                + decimalSize.cx + 1, y + decimalSize.cy,
                x + decimalSize.cx + size.cx, y + decimalSize.cy);
            return decimalSize.cx + size.cx;
		}
	}
}

_int64 CDraw::GetPriceColor(double price, double comparePrice)
{
	if(price != 0)
	{
		if(price > comparePrice)
		{
			return PCOLORS_UPCOLOR;
		}
		else if(price < comparePrice)
		{
			return PCOLORS_DOWNCOLOR;
		}
	}
	return PCOLORS_MIDCOLOR;
}

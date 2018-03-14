#include "..\\stdafx.h"
#include "Food.h"

Food::Food()
{
	SetBackColor(RGB(255, 0, 0));
	SetBorderColor(COLOR_EMPTY);
	SetCanFocus(false);
}

Food::~Food()
{
}

void Food::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
    int width = GetWidth();
    int height = GetHeight();
    if (width > 0 && height > 0)
    {
		RECT drawRect = {0, 0, width - 1, height - 1};
        _int64 paintingBackColor = GetPaintingBackColor();
		paint->DrawGradientEllipse(paintingBackColor, COLOR::RatioColor(paint, paintingBackColor, 0.9), drawRect, 90);
    }
}

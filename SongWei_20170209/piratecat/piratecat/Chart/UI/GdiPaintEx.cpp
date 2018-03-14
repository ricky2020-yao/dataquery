#include "..\\..\\stdafx.h"
#include "GdiPaintEx.h"

GdiPaintEx::GdiPaintEx()
{
}

GdiPaintEx::~GdiPaintEx()
{
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

_int64 GdiPaintEx::GetColor(_int64 dwPenColor)
{
	if (dwPenColor < COLOR_EMPTY)
    {
		return CDraw::GetWhiteColor(dwPenColor);
    }
    else
    {
		return GdiPaint::GetColor(dwPenColor);
    }
}
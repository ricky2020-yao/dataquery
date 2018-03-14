#include "..\\..\\stdafx.h"
#include "GdiPlusPaintEx.h"

GdiPlusPaintEx::GdiPlusPaintEx()
{
	m_supportTransparent = true;
}

GdiPlusPaintEx::~GdiPlusPaintEx()
{
	
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

_int64 GdiPlusPaintEx::GetColor(_int64 dwPenColor)
{
	if (dwPenColor <=  COLOR_EMPTY)
    {
		return CDraw::GetBlackColor(dwPenColor);
    }
    else
    {
		return GdiPlusPaint::GetColor(dwPenColor);
    }
}

void GdiPlusPaintEx::SetSupportTransparent(bool supportTransparent)
{
	m_supportTransparent = supportTransparent;
}

bool GdiPlusPaintEx::SupportTransparent()
{
	return m_supportTransparent;
}

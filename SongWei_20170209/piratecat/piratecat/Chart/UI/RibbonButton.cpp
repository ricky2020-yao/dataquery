#include "..\\..\\stdafx.h"
#include "RibbonButton.h"

RibbonButton::RibbonButton()
{
	m_arrowType = 0;
	m_isClose = false;
	SetBackColor(COLOR_EMPTY);
    SetBorderColor(COLOR_EMPTY);
}

RibbonButton::~RibbonButton()
{
}

int RibbonButton::GetArrowType()
{
	return m_arrowType;
}

void RibbonButton::SetArrowType(int arrowType)
{
	m_arrowType = arrowType;
}

bool RibbonButton::IsClose()
{
	return m_isClose;
}

void RibbonButton::SetClose(bool isClose)
{
	m_isClose = isClose;
}

bool RibbonButton::IsSelected()
{
	ControlA *parent = GetParent();
    if (parent)
    {
        TabControlA *tabControl = dynamic_cast<TabControlA*>(parent);
        if (tabControl)
        {
            TabPageA *selectedTabPage = tabControl->GetSelectedTabPage();
            if (selectedTabPage)
            {
                if (this == selectedTabPage->GetHeaderButton())
                {
                    return true;
                }
            }
        }
    }
    return false;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

_int64 RibbonButton::GetPaintingForeColor()
{
    if (IsEnabled())
    {
        return PCOLORS_FORECOLOR;
    }
    else
    {
        return PCOLORS_FORECOLOR5;;
    }
}

void RibbonButton::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth(), height = GetHeight();
    int mw = width / 2, mh = height / 2;
	RECT drawRect = {0, 0, width, height};
    if (m_isClose)
    {
        long lineColor = PCOLORS_LINECOLOR;
		RECT ellipseRect = {1, 1, width - 2, height - 2};
        paint->FillEllipse(PCOLORS_UPCOLOR, ellipseRect);
        paint->DrawLine(lineColor, 2, 0, 4, 4, width - 7, height - 7);
        paint->DrawLine(lineColor, 2, 0, 4, height - 7, width - 7, 3);
    }
    else
    {
        int cornerRadius = 4;
        if (m_arrowType > 0)
        {
            cornerRadius = 0;
        }
        ControlA *parent = GetParent();
        if (parent)
        {
            TabControlA *tabControl = dynamic_cast<TabControlA*>(parent);
            if (tabControl)
            {
                cornerRadius = 0;
            }
        }
		paint->DrawGradientRect(PCOLORS_BACKCOLOR, PCOLORS_BACKCOLOR2, drawRect, cornerRadius, 90);
		paint->DrawRoundRect(PCOLORS_LINECOLOR3, 1, 0, drawRect, cornerRadius);
    }
    if (m_arrowType > 0)
    {
		POINT point1 = {0};
		POINT point2 = {0};
		POINT point3 = {0};
		int ts = min(mw, mh) / 2;
        switch (m_arrowType)
        {
            case 1:
				point1.x = mw - ts;
				point1.y = mh;
				point2.x = mw + ts;
				point2.y = mh - ts;
				point3.x = mw + ts;
				point3.y = mh + ts;
                break;
            case 2:
				point1.x = mw + ts;
				point1.y = mh;
				point2.x = mw - ts;
				point2.y = mh - ts;
				point3.x = mw - ts;
				point3.y = mh + ts;
                break;
            case 3:
				point1.x = mw;
				point1.y = mh - ts;
				point2.x = mw - ts;
				point2.y = mh + ts;
				point3.x = mw + ts;
				point3.y = mh + ts;
                break;
            case 4:
				point1.x = mw;
				point1.y = mh + ts;
				point2.x = mw - ts;
				point2.y = mh - ts;
				point3.x = mw + ts;
				point3.y = mh - ts;
                break;
        }
		POINT points[3];
		points[0] = point1;
		points[1] = point2;
		points[2] = point3;
        paint->FillPolygon(PCOLORS_FORECOLOR, points, 3);
    }
    if (paint->SupportTransparent())
    {
	    NativeBase *native = GetNative();
		if (IsSelected())
        {
			paint->FillRect(PCOLORS_BACKCOLOR2, drawRect);
        }
		else if (this == native->GetPushedControl())
        {
			paint->FillRect(PCOLORS_BACKCOLOR6, drawRect);
        }
        else if (this == native->GetHoveredControl())
        {
			paint->FillRect(PCOLORS_BACKCOLOR5, drawRect);
        }
    }
}

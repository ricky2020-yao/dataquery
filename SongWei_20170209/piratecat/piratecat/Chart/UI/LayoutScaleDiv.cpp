#include "..\\..\\stdafx.h"
#include "LayoutScaleDiv.h"

LayoutScaleDiv::LayoutScaleDiv()
{
	m_chart = 0;
	m_layoutStr = L"";
	m_sysColors[0] = COLOR::ARGB(255, 255, 255);
	m_sysColors[1] = COLOR::ARGB(255,255,0);
	m_sysColors[2] = COLOR::ARGB(255, 0, 255);
	m_sysColors[3] = COLOR::ARGB(0, 255, 0);
	m_sysColors[4] = COLOR::ARGB(82, 255, 255);
	m_sysColors[5] = COLOR::ARGB(255, 82, 82);
	SetBackColor(COLOR_EMPTY);
}

LayoutScaleDiv::~LayoutScaleDiv()
{
}

OwChart* LayoutScaleDiv::GetChart()
{
	return m_chart;
}

void LayoutScaleDiv::SetChart(OwChart *chart)
{
	m_chart = chart;
}

String LayoutScaleDiv::GetLayoutStr()
{
	return m_layoutStr;
}

void LayoutScaleDiv::SetLayoutStr(const String&  layoutStr)
{
	m_layoutStr = layoutStr;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void LayoutScaleDiv::OnPaintForeground(CPaint *paint, const RECT& clipRect)
{
	if (m_layoutStr.length() > 0)
    {
        int width = GetWidth(), height = GetHeight();
        FONT *font = GetFont();
		vector<String> strs = CStr::Split(m_layoutStr, L"\r\n");
		int strsSize = (int)strs.size();
        int divPos = 0, divTop = 0, divHeight = 0;
        vector<String> inds;
        vector<String> viceInds;
		SIZE sizeK = {15, 16};
        for (int i = 0; i < strsSize; i++)
        {
            String subStr = strs[i];
			if (subStr.length() > 0)
            {
				vector<String> subStrs = CStr::Split(subStr, L"=");
				if (subStrs.size() >= 2)
                {
                    String name = subStrs[0];
                    String value = subStrs[1];
                    if (name == L"HEIGHT")
                    {
						divHeight = height * CStr::ConvertStrToInt(value.c_str()) / 100;
                    }
                    else if (name == L"IND")
                    {
						inds.push_back(value);
                    }
                    else if (name == L"VICEIND")
                    {
                        viceInds.push_back(value + L"(副)");
                    }
                }
            }
            if (subStr == L"DIV" || i == strsSize - 1)
            {
                if (i != 0)
                {
					RECT divRect = {0, divTop, width, divTop + divHeight};
					paint->FillRect(PCOLORS_BACKCOLOR3, divRect);
                    paint->DrawLine(PCOLORS_LINECOLOR5, 1, 0, 0, divRect.bottom - 1, width, divRect.bottom - 1);
                    vector<String> paintStrs;
                    if (divPos == 0)
                    {
						paintStrs.push_back(L"主K线");
                    }
                    else if (divPos == 1)
                    {
                        paintStrs.push_back(L"成交量");
                    }
                    int indsSize = (int)inds.size();
                    for (int j = 0; j < indsSize; j++)
                    {
                        paintStrs.push_back(inds[j]);
                    }
                    int viceIndsSize = (int)viceInds.size();
                    for (int j = 0; j < viceIndsSize; j++)
                    {
                        paintStrs.push_back(viceInds[j]);
                    }
                    int paintStrsSize = (int)paintStrs.size();
                    int left = 5, top = divTop + 5;
                    for (int j = 0; j < paintStrsSize; j++)
                    {
                        _int64 color = m_sysColors[j % 6];
						SIZE textSize = paint->TextSize(paintStrs[j].c_str(), font);
						int rectCsX = left;
                        int rectCsY = top - 2;
                        int rectCsW = sizeK.cx;
                        int rectCsH = sizeK.cy;
                        if (j == 0 && divPos == 0)
                        {
                            paint->DrawLine(PCOLORS_DOWNCOLOR, 1, 0, rectCsX + 4, rectCsY + 6, rectCsX + 4, rectCsY + rectCsH - 2);
                            paint->DrawLine(PCOLORS_UPCOLOR, 1, 0, rectCsX + 9, rectCsY + 2, rectCsX + 9, rectCsY + rectCsH - 4);
							RECT kRectA = {rectCsX + 3, rectCsY + 8, rectCsX + 6, rectCsY + 13};
                            paint->FillRect(PCOLORS_DOWNCOLOR, kRectA);
							RECT kRectB = {rectCsX + 8, rectCsY + 4, rectCsX + 11, rectCsY + 9};
                            paint->FillRect(PCOLORS_UPCOLOR, kRectB);
                        }
                        else if (j == 0 && divPos == 1)
                        {
							RECT vRectA = {rectCsX + 1, rectCsY + 10, rectCsX + 4, rectCsY + rectCsH - 1};
                            paint->FillRect(PCOLORS_UPCOLOR, vRectA);
							RECT vRectB = {rectCsX + 6, rectCsY + 3, rectCsX + 9, rectCsY + rectCsH - 1};
                            paint->FillRect(PCOLORS_UPCOLOR, vRectB);
							RECT vRectC = {rectCsX + 11, rectCsY + 8, rectCsX + 14, rectCsY + rectCsH - 1};
                            paint->FillRect(PCOLORS_UPCOLOR, vRectC);
                        }
                        else
                        {
                            paint->DrawLine(color, 1, 0, rectCsX + 2, rectCsY + 5, rectCsX + 12, rectCsY + 1);
                            paint->DrawLine(color, 1, 0, rectCsX + 2, rectCsY + 10, rectCsX + 12, rectCsY + 6);
                            paint->DrawLine(color, 1, 0, rectCsX + 2, rectCsY + 15, rectCsX + 12, rectCsY + 11);
                        }
                        left += rectCsW + 2;
						RECT textRect = {left, top, left + textSize.cx,top + textSize.cy};
						paint->DrawText(paintStrs[j].c_str(), color, font, textRect);
                        left += textSize.cx + 10;
                        if (left > width - 5)
                        {
                            left = 5;
                            top = 20;
                        }
                    }
                    paintStrs.clear();
                    inds.clear();
                    viceInds.clear();
                    divPos++;
                    divTop += divHeight;
                }
            }
        }
		strs.clear();
	    inds.clear();
        viceInds.clear();
    }
}
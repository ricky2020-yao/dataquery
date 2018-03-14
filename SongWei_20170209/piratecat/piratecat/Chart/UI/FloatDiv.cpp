#include "..\\..\\stdafx.h"
#include "FloatDiv.h"

FloatDiv::FloatDiv()
{
	m_chart = 0;
	m_digit = 2;
	SetBackColor(PCOLORS_BACKCOLOR3);
	SetBorderColor(PCOLORS_LINECOLOR4);
	SetCursor(CursorsA_SizeAll);
}

FloatDiv::~FloatDiv()
{
	m_chart = 0;
}

OwChart* FloatDiv::GetChart()
{
	return m_chart;
}

void FloatDiv::SetChart(OwChart *chart)
{
	m_chart = chart;
}

int FloatDiv::GetDigit()
{
	return m_digit;
}

void FloatDiv::SetDigit(int digit)
{
	m_digit = digit;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void FloatDiv::OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	ControlA::OnClick(mp, button, clicks, delta);
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		int width = GetWidth();
		if (mp.x >= width - 14 && mp.y <= 14)
		{
			SetVisible(false);
			GetNative()->Invalidate();
		}
	}
}

void FloatDiv::OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	ControlA::OnClick(mp, button, clicks, delta);
    int width = GetWidth();
    if (mp.x >= width - 14 && mp.y <= 14)
    {
        SetCursor(CursorsA_Arrow);
    }
    else
    {
        SetCursor(CursorsA_SizeAll);
    }
    Invalidate();
}

void FloatDiv::OnPaintBackground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
	int height = GetHeight();
	if(width > 0 && height > 0)
	{
		ChartA *chart = m_chart->GetChart();
		RECT rect = {0, 0, width, height};
		int left = 0;
		int top = 0;
		FONT *nFont = GetFont();
		if(chart->IsShowCrossLine())
		{
			CTable *dataSource = chart->GetDataSource();
			int crossStopIndex = chart->GetCrossStopIndex();
			if(dataSource->RowsCount() > 0)
			{
				if (crossStopIndex < chart->GetFirstVisibleIndex())
				{
					crossStopIndex = chart->GetFirstVisibleIndex();
				}
				if (crossStopIndex > chart->GetLastVisibleIndex())
				{
					crossStopIndex = chart->GetLastVisibleIndex();
				}
			}
			else
			{
				crossStopIndex = -1;
			}
			RECT rect = {0, 0, width, height};
			paint->FillRect(GetPaintingBackColor(), rect);
			paint->DrawRect(GetPaintingBorderColor(), 1, 0, rect);
			_int64 lineColor = PCOLORS_LINECOLOR;
            paint->DrawLine(lineColor, 2, 0, width - 6, 4, width - 14, 12);
            paint->DrawLine(lineColor, 2, 0, width - 6, 12, width - 14, 4);
			FONT font(L"Arial", 14, false, false, false);
			FONT lfont(L"Arial", 12, false, false, false);
			FONT nfont(L"Arial", 14, true, false, false);
			if(crossStopIndex >= 0)
			{
				wchar_t strTemp[100] = {0};
				double date = dataSource->GetXValue(crossStopIndex);
				int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;	    
				::M130(date, &year, &month, &day, &hour, &minute, &second, &msecond);
				int cycle = m_chart->GetCycle();
				if(cycle <= 1)
				{
					_stprintf_s(strTemp, 99, L"%02d:%02d", hour, minute);
				}
				else if(cycle >= 5 && cycle <= 60)
				{
					_stprintf_s(strTemp, 99, L"%02d-%02d %02d:%02d", month, day, hour, minute);
				}
				else
				{
					_stprintf_s(strTemp, 99, L"%d-%02d-%02d", year, month, day);
				}
				SIZE dtSize = paint->TextSize(strTemp, &font);
				CDraw::DrawText(paint, strTemp, PCOLORS_FORECOLOR3, &font, left + width / 2 - dtSize.cx / 2, top + 20);
				double close = 0, high = 0, low = 0, open = 0, amount = 0;
				close = dataSource->Get2(crossStopIndex, CLOSE_INDEX);
				high = dataSource->Get2(crossStopIndex, HIGH_INDEX);
				low = dataSource->Get2(crossStopIndex, LOW_INDEX);
				open = dataSource->Get2(crossStopIndex, OPEN_INDEX);
				amount = dataSource->Get2(crossStopIndex, AMOUNT_INDEX);
				if (dataSource->IsNaN(close))
				{
					close = 0; 
				}
				if (dataSource->IsNaN(high))
				{
					high = 0;
				}
				if (dataSource->IsNaN(low))
				{
					low = 0;
				}
				if (dataSource->IsNaN(open))
				{
					open = 0;
				}
				if (dataSource->IsNaN(amount))
				{
					amount = 0;
				}
				double rate = 1;
				double lastClose = 0;
				if (crossStopIndex > 1)
				{
					lastClose = dataSource->Get2(crossStopIndex - 1, CLOSE_INDEX);
					if (cycle == 0)
                    {
                        lastClose = m_chart->GetLatestData().m_lastClose;
                    }
					if (!dataSource->IsNaN(lastClose))
					{
						if (lastClose != 0)
						{
							rate = (close - lastClose) / lastClose;
						}
					}
				}
				CStr::GetValueByDigit(open, m_digit, strTemp); 
				SIZE tSize = paint->TextSize(strTemp, &nfont);
				CDraw::DrawText(paint, strTemp, CDraw::GetPriceColor(open, lastClose), &nfont, left + width / 2 - tSize.cx / 2, top + 60);
				CStr::GetValueByDigit(high, m_digit, strTemp); 
				tSize = paint->TextSize(strTemp, &nfont);
				CDraw::DrawText(paint, strTemp, CDraw::GetPriceColor(high, lastClose), &nfont, left + width / 2 - tSize.cx / 2, top + 100);
				CStr::GetValueByDigit(low, m_digit, strTemp); 
				tSize = paint->TextSize(strTemp, &nfont);
				CDraw::DrawText(paint, strTemp, CDraw::GetPriceColor(low, lastClose), &nfont, left + width / 2 - tSize.cx / 2, top + 140);
				CStr::GetValueByDigit(close, m_digit, strTemp); 
				tSize = paint->TextSize(strTemp, &nfont);
				CDraw::DrawText(paint, strTemp, CDraw::GetPriceColor(close, lastClose), &nfont, left + width / 2 - tSize.cx / 2, top + 180);
				String unit = L"";
                if (amount > 100000000)
                {
                    amount /= 100000000;
                    unit = L"亿";
                }
                else if (amount > 10000)
                {
                    amount /= 10000;
                    unit = L"万";
                }
				CStr::GetValueByDigit(amount, m_digit, strTemp); 
				String amountStr = strTemp + unit;
				tSize = paint->TextSize(amountStr.c_str(), &lfont);
				CDraw::DrawText(paint, amountStr.c_str(), PCOLORS_FORECOLOR3, &lfont, left + width / 2 - tSize.cx / 2, top + 220);
				wchar_t strTemp2[100] = {0};
				CStr::GetValueByDigit(rate*100, m_digit, strTemp2);
				CStr::Contact(strTemp, strTemp2, L"%", L"");
				tSize = paint->TextSize(strTemp, &lfont);
				CDraw::DrawText(paint, strTemp, CDraw::GetPriceColor(close, lastClose), &lfont, left + width / 2 - tSize.cx / 2, top + 260);
			}
			_int64 whiteColor = PCOLORS_FORECOLOR;
			CDraw::DrawText(paint, L"日 期", whiteColor, &font, left + 25, top + 2);
			CDraw::DrawText(paint, L"开 盘", whiteColor, &font, left + 25, top + 40);
			CDraw::DrawText(paint, L"最 高", whiteColor, &font, left + 25, top + 80);
			CDraw::DrawText(paint, L"最 低", whiteColor, &font, left + 25, top + 120);
			CDraw::DrawText(paint, L"收 盘", whiteColor, &font, left + 25, top + 160);
			CDraw::DrawText(paint, L"金 额", whiteColor, &font, left + 25, top + 200);
			CDraw::DrawText(paint, L"涨 幅", whiteColor, &font, left + 25, top + 240);
		}
	}
}

void FloatDiv::OnPaintBorder(CPaint *paint, const RECT& clipRect)
{
}
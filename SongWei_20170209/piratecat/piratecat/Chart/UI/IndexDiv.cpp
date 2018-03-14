#include "..\\..\\stdafx.h"
#include "IndexDiv.h"

IndexDiv::IndexDiv()
{
	m_chart = 0;
	m_requestID = BaseService::GetRequestID();
	SetBackColor(PCOLORS_BACKCOLOR4);
	SetBorderColor(PCOLORS_LINECOLOR4);
	ListenerMessageCallBack callBack = IndexDiv::LatestDataCallBack;
	DataCenter::GetQuoteService()->RegisterListener(m_requestID, callBack, this);
}

IndexDiv::~IndexDiv()
{
	m_chart = 0;
	QuoteService *quoteService = DataCenter::GetQuoteService();
	quoteService->StopPushLatestDatas(m_requestID);
	quoteService->UnRegisterListener(m_requestID);
}

OwChart* IndexDiv::GetChart()
{
	return m_chart;
}

void IndexDiv::SetChart(OwChart* chart)
{
	m_chart = chart;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void IndexDiv::LatestDataCallBack(CMessage *message, void *pInvoke)
{
	IndexDiv *div = (IndexDiv*)pInvoke;
	if(div)
	{
		div->OnLatestDataCallBack(message);
	}
}

void IndexDiv::OnInvoke(void *args)
{
	ControlA::OnInvoke(args);
	if(args)
	{
		CMessage *message = (CMessage*)args;
		LatestDataInfo dataInfo;
		vector<SecurityLatestData> datas;
		QuoteService::GetLatestDatas(&dataInfo, &datas, message->m_body, message->m_bodyLength);
		bool changed = false;
		int datasSize = (int)datas.size();
		for(int i = 0; i < datasSize; i++)
		{
			SecurityLatestData latestData = datas[i];
			if(i == 0)
			{
				if (latestData.m_securityCode == L"000001.SH")
				{
					if (!latestData.Equal(&m_ssLatestData))
					{
						m_ssLatestData = latestData;
					}
				}
			}
			else if(i == 1)
			{
				if (latestData.m_securityCode == L"399001.SZ")
				{
					if (!latestData.Equal(&m_szLatestData))
					{
						m_szLatestData = latestData;
					}
				}
			}
			else if(i == 2)
			{
				if (latestData.m_securityCode == L"399006.SZ")
				{
					if (!latestData.Equal(&m_cyLatestData))
					{
						m_cyLatestData = latestData;
					}
				}
			}
		}
		Invalidate();
		delete message->m_body;
		delete message;
		message = 0;
	}
}

void IndexDiv::OnLatestDataCallBack(CMessage* message)
{
	CMessage *copyMessage = new CMessage;
	copyMessage->Copy(message);
	copyMessage->m_body = new char[copyMessage->m_bodyLength];
	char *str = new char[copyMessage->m_bodyLength];
	for(int i = 0; i < copyMessage->m_bodyLength; i++)
	{
		str[i] = message->m_body[i];
	}
	copyMessage->m_body = str;
	BeginInvoke(copyMessage);
}

void IndexDiv::OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta)
{
	ControlA::OnMouseDown(mp, button, clicks, delta);
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		int width = GetWidth();
		Security security;
        security.m_type = 18;
        if (mp.x < width / 3)
        {
            security.m_code = L"000001.SH";
            security.m_name = L"上证指数";
        }
        else if (mp.x < width * 2 / 3)
        {
            security.m_code = L"399001.SZ";
            security.m_name = L"深证成指";
        }
        else
        {
            security.m_code = L"399006.SZ";
            security.m_name = L"创业板指数";
        }
        m_chart->SearchSecurity(&security);
	}
}

void IndexDiv::OnPaintForeground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
	int height = GetHeight();
	if(width > 0 && height > 0)
	{
		_int64 titleColor = PCOLORS_FORECOLOR2;
		FONT indexFont(L"Arial", 14, true, false, false);
		FONT indexBoldFont(L"SimSun", 14, true, false, false);
		_int64 grayColor = PCOLORS_FORECOLOR4;
		_int64 indexColor = CDraw::GetPriceColor(m_ssLatestData.m_close, m_ssLatestData.m_lastClose);
		int left = 1;
		int top = 2;
		SIZE tSize = paint->TextSize(L"上证", &indexBoldFont);
		RECT tRect = {left, top, left + tSize.cx,top + tSize.cy};
		paint->DrawText(L"上证", titleColor, &indexBoldFont, tRect);
		left += 40;
		paint->DrawLine(grayColor, 1, 0, left, top, left, height);
		wchar_t amountStr[100] = {0};
		_stprintf_s(amountStr, 99, L"%.1f%s", m_ssLatestData.m_amount / 100000000, L"亿");
		tSize = paint->TextSize(amountStr, &indexFont);
		int sLeft = width / 3 - tSize.cx;
		RECT tRect2 = {sLeft, top, sLeft + tSize.cx, top + tSize.cy};
		paint->DrawText(amountStr, titleColor, &indexFont, tRect2);
		left += (width / 3 - 40 - tSize.cx) / 4;
		int length = CDraw::DrawUnderLineNum(paint, m_ssLatestData.m_close, 2, &indexFont, indexColor, false, left, top);
		left += length + (width / 2 - 40 - tSize.cx) / 4;
		length = CDraw::DrawUnderLineNum(paint, m_ssLatestData.m_close - m_ssLatestData.m_lastClose, 2, &indexFont, indexColor, false, left, top);
		left = width / 3;
		paint->DrawLine(grayColor, 1, 0, left, top, left, height);
		indexColor = CDraw::GetPriceColor(m_szLatestData.m_close, m_szLatestData.m_lastClose);
		tSize = paint->TextSize(L"深证", &indexBoldFont);
		RECT tRect3 = {left, top, left + tSize.cx, top + tSize.cy};
		paint->DrawText(L"深证", titleColor, &indexBoldFont, tRect3);
		left += 40;
		paint->DrawLine(grayColor, 1, 0, left, top, left, height);
		_stprintf_s(amountStr, 99, L"%.1f%s", m_szLatestData.m_amount / 100000000, L"亿");
		tSize = paint->TextSize(amountStr, &indexFont);
		sLeft = width * 2 / 3 - tSize.cx;
		RECT tRect4 = {sLeft, top, sLeft + tSize.cx, top + tSize.cy};
		paint->DrawText(amountStr, titleColor, &indexFont, tRect4);
		left += (width / 3 - 40 - tSize.cx) / 4;
		length = CDraw::DrawUnderLineNum(paint, m_szLatestData.m_close, 2, &indexFont, indexColor, false, left, top);
		left += length + (width / 2 - 40 - tSize.cx) / 4;
		length = CDraw::DrawUnderLineNum(paint, m_szLatestData.m_close - m_szLatestData.m_lastClose, 2, &indexFont, indexColor, false, left, top);
		left = width * 2 / 3;
		paint->DrawLine(grayColor, 1, 0, left, top, left, height);
		indexColor = CDraw::GetPriceColor(m_cyLatestData.m_close, m_cyLatestData.m_lastClose);
		tSize = paint->TextSize(L"创业", &indexBoldFont);
		RECT tRect5 = {left, top, left + tSize.cx, top + tSize.cy};
		paint->DrawText(L"创业", titleColor, &indexBoldFont, tRect5);
		left += 40;
		paint->DrawLine(grayColor, 1, 0, left, top, left, height);
		_stprintf_s(amountStr, 99, L"%.1f%s", m_cyLatestData.m_amount / 100000000, L"亿");
		tSize = paint->TextSize(amountStr, &indexFont);
		sLeft = width - tSize.cx;
		RECT tRect6 = {sLeft, top, sLeft + tSize.cx, top + tSize.cy};
		paint->DrawText(amountStr, titleColor, &indexFont, tRect6);
		left += (width/3 - 40 - tSize.cx) / 4;
		length = CDraw::DrawUnderLineNum(paint, m_cyLatestData.m_close,2, &indexFont, indexColor, false, left, top);
		left += length + (width / 2 - 40 - tSize.cx) / 4;
		length = CDraw::DrawUnderLineNum(paint, m_cyLatestData.m_close - m_cyLatestData.m_lastClose, 2, &indexFont,indexColor, false, left, top);
		RECT rect = {0, 0, width, height};
		paint->DrawRect(grayColor, 1, 0, rect);
	}
}

void IndexDiv::Start()
{
	LatestDataInfo dataInfo;
	dataInfo.m_codes = L"000001.SH,399001.SZ,399006.SZ";
	dataInfo.m_formatType = 1;
	DataCenter::GetQuoteService()->PushLatestDatas(m_requestID, &dataInfo);
}
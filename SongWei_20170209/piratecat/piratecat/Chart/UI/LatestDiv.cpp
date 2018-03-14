#include "..\\..\\stdafx.h"
#include "LatestDiv.h"

LatestDiv::LatestDiv()
{
	m_chart = 0;
	m_digit = 2;
	m_gridTransaction = 0;
	m_lV2 = false;
	m_requestID = BaseService::GetRequestID();
	m_securityCode = L"";
	m_securityName = L"";
	SetBackColor(PCOLORS_BACKCOLOR4);
	SetBorderColor(PCOLORS_LINECOLOR4);
}

LatestDiv::~LatestDiv()
{
	QuoteService *quoteService = DataCenter::GetQuoteService();
	quoteService->StopPushLatestDatas(m_requestID);
	quoteService->UnRegisterListener(m_requestID);
	m_chart = 0;
	m_gridTransaction = 0;
}

OwChart* LatestDiv::GetChart()
{
	return m_chart;
}

void LatestDiv::SetChart(OwChart *chart)
{
	m_chart = chart;
}

int LatestDiv::GetDigit()
{
	return m_digit;
}

void LatestDiv::SetDigit(int digit)
{
	m_digit = digit;
}

SecurityLatestData LatestDiv::GetLatestData()
{
	return m_latestData;
}

bool LatestDiv::IsLV2()
{
	return m_lV2;
}

void LatestDiv::SetLV2(bool lV2)
{
	m_lV2 = lV2;
}

int LatestDiv::GetRequestID()
{
	return m_requestID;
}

String LatestDiv::GetSecurityCode()
{
	return m_securityCode;
}

void LatestDiv::SetSecurityCode(const String&  securityCode)
{
	if(m_securityCode != securityCode)
	{
		SecurityLatestData lastestData;
        m_latestData = lastestData;
		SecurityLatestDataLV2 latestDataLV2;
        m_latestDataLV2 = latestDataLV2;
		m_lV2 = m_type != 17;
		m_securityCode = securityCode;
		QuoteService *quoteService = DataCenter::GetQuoteService();
		quoteService->StopPushLatestDatas(m_requestID);
		quoteService->UnRegisterListener(m_requestID);
		m_requestID = BaseService::GetRequestID();
		ListenerMessageCallBack latestDataCallBack = LatestDiv::LatestDataCallBack;
		quoteService->RegisterListener(m_requestID, latestDataCallBack, this);
		LatestDataInfo dataInfo;
		dataInfo.m_codes = m_securityCode;
		dataInfo.m_lv2 = 1;
		quoteService->PushLatestDatas(m_requestID, &dataInfo);
		m_gridTransaction->ClearRows();
        m_gridTransaction->Update();
		Update();
	}
}

String LatestDiv::GetSecurityName()
{
	return m_securityName;
}

void LatestDiv::SetSecurityName(const String& securityName)
{
	m_securityName = securityName;
}

int LatestDiv::GetType()
{
	return m_type;
}

void LatestDiv::SetType(int type)
{
	m_type = type;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void LatestDiv::LatestDataCallBack(CMessage *message, void *pInvoke)
{
	LatestDiv *div = (LatestDiv*)pInvoke;
	if(div)
	{
		div->OnLatestDataCallBack(message);
	}
}

double LatestDiv::Max(vector<double> *list)
{
	double max = 0;
	int i = 0;
	int length = (int)list->size();
	for(i = 0;i < length;i++)
	{
		if(i == 0)
		{
			max = (*list)[i];
		}
		else
		{
			if(max < (*list)[i])
			{
				max = (*list)[i];
			}
		}
	}
	return max;
}

void LatestDiv::OnAdd()
{
	ControlA::OnAdd();
	if (!m_gridTransaction)
    {
        m_gridTransaction = new GridA;
        m_gridTransaction->SetBackColor(COLOR_EMPTY);
		m_gridTransaction->SetBorderColor(COLOR_EMPTY);
        m_gridTransaction->SetGridLineColor(COLOR_EMPTY);
        m_gridTransaction->SetHeaderVisible(false);
        m_gridTransaction->SetSelectionMode(GridSelectionMode_SelectNone);
        AddControl(m_gridTransaction);
        GridColumn *dateColumn = new GridColumn;
        dateColumn->SetWidth(80);
        m_gridTransaction->AddColumn(dateColumn);
        GridColumn *priceColumn = new GridColumn;
        priceColumn->SetWidth(70);
        m_gridTransaction->AddColumn(priceColumn);
        GridColumn *volumeColumn = new GridColumn;
        volumeColumn->SetWidth(100);
        m_gridTransaction->AddColumn(volumeColumn);
        m_gridTransaction->Update();
    }
}

void LatestDiv::OnInvoke(void *args)
{
	ControlA::OnInvoke(args);
	if(args)
	{
		CMessage *message = (CMessage*)args;
		if(message->m_requestID ==  m_requestID)
		{
			if(message->m_functionID == FUNCTIONID_QUOTE_PUSHLATESTDATA)
			{
				LatestDataInfo dataInfo;
				vector<SecurityLatestData> datas;
				QuoteService::GetLatestDatas(&dataInfo, &datas, message->m_body, message->m_bodyLength);
				SecurityLatestData data = datas[0];
				if(data.m_securityCode == m_securityCode)
				{
					if(m_latestData.m_securityCode == L"" || !m_latestData.Equal(&data))
					{
						m_latestData.Copy(&data);
						int digit = 2;
						if(m_latestData.m_securityCode.find(L"1") == 0 || m_latestData.m_securityCode.find(L"5") == 0)
						{
							digit = 3;
						}
                        m_chart->SetDigit(digit);
                        m_chart->RefreshData();
					}
				}
			}
	        else if (message->m_functionID == FUNCTIONID_QUOTE_PUSHLATESTDATALV2)
            {
                LatestDataInfoLV2 dataInfo;
                vector<SecurityLatestDataLV2> datas;
				QuoteService::GetLatestDatasLV2(&dataInfo, &datas, message->m_body, message->m_bodyLength);
                SecurityLatestDataLV2 latestDataLV2 = datas[0];
                if (latestDataLV2.m_securityCode == m_securityCode &&
                !latestDataLV2.Equal(&m_latestDataLV2))
                {
                    m_latestDataLV2 = latestDataLV2;
                }
            }
			else if(message->m_functionID == FUNCTIONID_QUOTE_PUSHTRANSACTIONDATA)
			{
				String securityCode = L"";
				vector<TransactionData> transactionDatas;
				QuoteService::GetTransactionDatas(&securityCode, &transactionDatas, message->m_body, message->m_bodyLength);
				int transactionDatasSize = (int)transactionDatas.size();
				for (int i = 0; i < transactionDatasSize; i++)
				{
					TransactionData transactionData = transactionDatas[i];
					GridRow *row = new GridRow;
					m_gridTransaction->InsertRow(0, row);
					int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;	    
					::M130((double)transactionData.m_date, &year, &month, &day, &hour, &minute, &second, &msecond);
					wchar_t szDate[20] = {0};
					_stprintf_s(szDate, 19, L"%02d:%02d:%02d", hour, minute, second);
					TransactionDateCell *dateCell = new TransactionDateCell;
					dateCell->SetText(szDate);
					row->AddCell(0, dateCell);
					GridCellStyle dateCellStyle;
					dateCellStyle.SetBackColor(COLOR_EMPTY);
					FONT dFont(L"SimSun", 14, true, false, false);
					dateCellStyle.SetFont(&dFont);
					dateCellStyle.SetForeColor(PCOLORS_FORECOLOR2);
					dateCell->SetStyle(&dateCellStyle);
					TransactionDoubleCell *priceCell = new TransactionDoubleCell;
					priceCell->SetDigit(2);
					priceCell->SetDouble(transactionData.m_price);
					row->AddCell(1, priceCell);
					GridCellStyle priceCellStyle;
					priceCellStyle.SetBackColor(COLOR_EMPTY);
					priceCellStyle.SetFont(&dFont);
					priceCellStyle.SetForeColor(CDraw::GetPriceColor(transactionData.m_price, m_latestData.m_lastClose));
					priceCell->SetStyle(&priceCellStyle);
					TransactionDoubleCell *volumeCell = new TransactionDoubleCell;
					volumeCell->SetDouble(transactionData.m_volume);
					row->AddCell(2, volumeCell);
					GridCellStyle volumeCellStyle;
					volumeCellStyle.SetBackColor(COLOR_EMPTY);
					volumeCellStyle.SetFont(&dFont);
					if (transactionData.m_type == 0)
					{
						volumeCellStyle.SetForeColor(PCOLORS_FORECOLOR);
					}
					else if (transactionData.m_type == 1)
					{
						volumeCellStyle.SetForeColor(PCOLORS_UPCOLOR);
					}
					else
					{
						volumeCellStyle.SetForeColor(PCOLORS_DOWNCOLOR);
					}
					volumeCell->SetStyle(&volumeCellStyle);
				}
				m_gridTransaction->Update();
			}
		}
		delete message->m_body;
		delete message;
		message = 0;
		Invalidate();
	}
}

void LatestDiv::OnLatestDataCallBack(CMessage *message)
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

void LatestDiv::OnPaintForeground(CPaint *paint, const RECT& clipRect)
{
	int width = GetWidth();
	int height = GetHeight();
	if(width > 0 && height > 0)
	{
		FONT font(L"SimSun", 16, false, false, false);
		_int64 wordColor = PCOLORS_FORECOLOR5;
		int top = 32, step = 20;	
		CDraw::DrawText(paint, L"卖", wordColor, &font, 1, (m_lV2 ? 87 : 47));
		CDraw::DrawText(paint, L"盘", wordColor, &font, 1, (m_lV2 ? 140 : 100));
		CDraw::DrawText(paint, L"买", wordColor, &font, 1, (m_lV2 ? 267 : 147));
		CDraw::DrawText(paint, L"盘", wordColor, &font, 1, (m_lV2 ? 310 : 200));
        String buySellStr = L"5,4,3,2,1,1,2,3,4,5";
        if (m_lV2)
        {
            step = 16;
            buySellStr = L"总卖量,10,9,8,7,6," + buySellStr + L",6,7,8,9,10,总买量";
            font.m_fontSize = 14;
        }
		vector<String> buySellStrs = CStr::Split(buySellStr, L",");
        int strsSize = (int)buySellStrs.size();
        for (int i = 0; i < strsSize; i++)
        {
			CDraw::DrawText(paint, buySellStrs[i].c_str(), wordColor, &font, 25, top);
            top += step;
        }
		buySellStrs.clear();
        font.m_fontSize = 16;
        top = m_lV2 ? 390 : 232;
		CDraw::DrawText(paint, L"最新", wordColor, &font, 1, top);
		CDraw::DrawText(paint, L"升跌", wordColor, &font, 1, top + 20);
		CDraw::DrawText(paint, L"幅度", wordColor, &font, 1, top + 40);
        CDraw::DrawText(paint, L"总手", wordColor, &font, 1, top + 60);
        CDraw::DrawText(paint, L"涨停", wordColor, &font, 1, top + 80);
        CDraw::DrawText(paint, L"外盘", wordColor, &font, 1, top + 100);
        CDraw::DrawText(paint, L"开盘", wordColor, &font, 110, top);
        CDraw::DrawText(paint, L"最高", wordColor, &font, 110, top + 20);
        CDraw::DrawText(paint, L"最低", wordColor, &font, 110, top + 40);
        CDraw::DrawText(paint, L"换手", wordColor, &font, 110, top + 60);
        CDraw::DrawText(paint, L"跌停", wordColor, &font, 110, top + 80);
        CDraw::DrawText(paint, L"内盘", wordColor, &font, 110, top + 100);
		font.m_bold = true;
		_int64 yellowColor = PCOLORS_FORECOLOR2;
		if(m_latestData.m_securityCode != L"")
		{
	        double close = m_latestData.m_close, open = m_latestData.m_open, high = m_latestData.m_high, low = m_latestData.m_low, lastClose = m_latestData.m_lastClose;
            if (close == 0)
            {
                if (m_latestData.m_buyPrice1 > 0)
                {
                    close = m_latestData.m_buyPrice1;
                    open = m_latestData.m_buyPrice1;
                    high = m_latestData.m_buyPrice1;
                    low = m_latestData.m_buyPrice1;
                }
                else if (m_latestData.m_sellPrice1 > 0)
                {
                    close = m_latestData.m_sellPrice1;
                    open = m_latestData.m_sellPrice1;
                    high = m_latestData.m_sellPrice1;
                    low = m_latestData.m_sellPrice1;
                }
            }
	        if (lastClose == 0)
            {
                lastClose = close;
            }
			vector<double> plist;
            vector<double> vlist;
            if (m_lV2)
            {
                plist.push_back(m_latestDataLV2.m_sellPrice10);
                plist.push_back(m_latestDataLV2.m_sellPrice9);
                plist.push_back(m_latestDataLV2.m_sellPrice8);
                plist.push_back(m_latestDataLV2.m_sellPrice7);
                plist.push_back(m_latestDataLV2.m_sellPrice6);
                vlist.push_back(m_latestDataLV2.m_sellVolume10);
                vlist.push_back(m_latestDataLV2.m_sellVolume9);
                vlist.push_back(m_latestDataLV2.m_sellVolume8);
                vlist.push_back(m_latestDataLV2.m_sellVolume7);
                vlist.push_back(m_latestDataLV2.m_sellVolume6);
            }
            plist.push_back(m_latestData.m_sellPrice5);
            plist.push_back(m_latestData.m_sellPrice4);
            plist.push_back(m_latestData.m_sellPrice3);
            plist.push_back(m_latestData.m_sellPrice2);
            plist.push_back(m_latestData.m_sellPrice1);
            vlist.push_back(m_latestData.m_sellVolume5);
            vlist.push_back(m_latestData.m_sellVolume4);
            vlist.push_back(m_latestData.m_sellVolume3);
            vlist.push_back(m_latestData.m_sellVolume2);
            vlist.push_back(m_latestData.m_sellVolume1);
            plist.push_back(m_latestData.m_buyPrice1);
            plist.push_back(m_latestData.m_buyPrice2);
            plist.push_back(m_latestData.m_buyPrice3);
            plist.push_back(m_latestData.m_buyPrice4);
            plist.push_back(m_latestData.m_buyPrice5);
            vlist.push_back(m_latestData.m_buyVolume1);
            vlist.push_back(m_latestData.m_buyVolume2);
            vlist.push_back(m_latestData.m_buyVolume3);
            vlist.push_back(m_latestData.m_buyVolume4);
            vlist.push_back(m_latestData.m_buyVolume5);
            if (m_lV2)
            {
                plist.push_back(m_latestDataLV2.m_buyPrice6);
                plist.push_back(m_latestDataLV2.m_buyPrice7);
                plist.push_back(m_latestDataLV2.m_buyPrice8);
                plist.push_back(m_latestDataLV2.m_buyPrice9);
                plist.push_back(m_latestDataLV2.m_buyPrice10);
                vlist.push_back(m_latestDataLV2.m_buyVolume6);
                vlist.push_back(m_latestDataLV2.m_buyVolume7);
                vlist.push_back(m_latestDataLV2.m_buyVolume8);
                vlist.push_back(m_latestDataLV2.m_buyVolume9);
                vlist.push_back(m_latestDataLV2.m_buyVolume10);
            }
			_int64 color = 0;
			double max = Max(&vlist);
			font.m_fontSize = (float)(m_lV2 ? 14 : 16);
			if(max != 0)
			{
				int pLength = (int)plist.size();
                top = 32;
		        if (m_lV2)
                {
					color = CDraw::GetPriceColor(m_latestDataLV2.m_avgSellPrice, lastClose);
					CDraw::DrawUnderLineNum(paint, m_latestDataLV2.m_avgSellPrice, m_digit, &font, color, false, 80, top);
					color = CDraw::GetPriceColor(0, m_latestDataLV2.m_allSellVol);
					CDraw::DrawUnderLineNum(paint, m_latestDataLV2.m_allSellVol, 0, &font, yellowColor, false, 130, top);
                    top += step;
                }
                for (int i = 0; i < pLength; i++)
                {
					color = CDraw::GetPriceColor(plist[i], lastClose);
					CDraw::DrawUnderLineNum(paint, plist[i], m_digit, &font, color, true, m_lV2 ? 80 : 60, top);
					CDraw::DrawUnderLineNum(paint, vlist[i], 0, &font, yellowColor, true, m_lV2 ? 130 : 110, top);
                    paint->FillRect(color, width - (int)(vlist[i] / max * 50), top + step / 2 - 2, width, top + step / 2 + 2);
                    top += step;
                }
		        if (m_lV2)
                {
					color = CDraw::GetPriceColor(m_latestDataLV2.m_avgBuyPrice, lastClose);
					CDraw::DrawUnderLineNum(paint, m_latestDataLV2.m_avgBuyPrice, m_digit, &font, color, false, 80, top);
					color = CDraw::GetPriceColor(m_latestDataLV2.m_allBuyVol, 0);
					CDraw::DrawUnderLineNum(paint, m_latestDataLV2.m_allBuyVol, 0, &font, yellowColor, false, 130, top);
                    top += step;
                }
			}
			vlist.clear();
            plist.clear();
			top = m_lV2 ? 390 : 232;
			color = CDraw::GetPriceColor(close, lastClose);
			CDraw::DrawUnderLineNum(paint, close, m_digit, &font, color,  true, 45,  top);
			double sub = 0;
			if(close == 0)
			{
				sub = m_latestData.m_buyPrice1 - lastClose;
				double range = 100 * (m_latestData.m_buyPrice1 - lastClose) / lastClose;
				int cleft = CDraw::DrawUnderLineNum(paint, range, m_digit, &font, color, false, 45, top + 40);
				CDraw::DrawText(paint, L"%", color, &font, cleft + 47, top + 40);
			}
			else
			{
				sub = close - lastClose;
				double range = 100 * (close - lastClose) / lastClose;
				int cleft = CDraw::DrawUnderLineNum(paint, range, m_digit, &font, color, false, 45, top + 40);
				CDraw::DrawText(paint, L"%", color, &font, cleft + 47, top + 40);
			}
			CDraw::DrawUnderLineNum(paint, sub, m_digit, &font, color, true, 45, top + 20);
			double volume = m_latestData.m_volume / 100;
            String unit = L"";
            if (volume > 100000000)
            {
                volume /= 100000000;
                unit = L"亿";
            }
            else if (volume > 10000)
            {
                volume /= 10000;
                unit = L"万";
            }
            //总手
			int cleft = CDraw::DrawUnderLineNum(paint, volume, unit.length() > 0 ? m_digit : 0, &font, yellowColor, true, 45, top + 60);
			if (unit.length() > 0)
            {
				CDraw::DrawText(paint, unit.c_str(), yellowColor, &font, cleft + 47, top + 60);
            }
			cleft = CDraw::DrawUnderLineNum(paint, m_latestData.m_turnoverRate, m_digit, &font, yellowColor, false, 155, top + 60);
			if(m_latestData.m_turnoverRate > 0)
			{
				CDraw::DrawText(paint, L"%", yellowColor, &font, cleft + 157, top + 60);
			}
			color = CDraw::GetPriceColor(open, lastClose);
			CDraw::DrawUnderLineNum(paint, open, m_digit, &font, color, true, 155, top);
			color = CDraw::GetPriceColor(high, lastClose);
			CDraw::DrawUnderLineNum(paint, high, m_digit, &font, color, true, 155, top + 20);
			color = CDraw::GetPriceColor(low, lastClose);
			CDraw::DrawUnderLineNum(paint, low, m_digit, &font, color, true, 155, top + 40);
			double upPrice = lastClose * 1.1;
			if(m_securityName.find(L"ST") == 0 || m_securityName.find(L"*ST") == 0)
			{
				upPrice = lastClose * 1.05;
			}
			CDraw::DrawUnderLineNum(paint, upPrice, m_digit, &font, PCOLORS_UPCOLOR, true, 45, top + 80);
			double downPrice = lastClose * 0.9;
			if(m_securityName.find(L"ST") == 0 || m_securityName.find(L"*ST") == 0)
			{
				downPrice = lastClose * 0.95;
			}
			CDraw::DrawUnderLineNum(paint, downPrice, m_digit, &font, PCOLORS_DOWNCOLOR, true, 155, top + 80);
	        double outerVol = m_latestData.m_outerVol;
            unit = L"";
            if (outerVol > 100000000)
            {
                outerVol /= 100000000;
                unit = L"亿";
            }
            else if (outerVol > 10000)
            {
                outerVol /= 10000;
                unit = L"万";
            }
			cleft = CDraw::DrawUnderLineNum(paint, outerVol, unit.length() > 0 ? m_digit : 0, &font, PCOLORS_UPCOLOR, false, 45, top + 100);
			if (unit.length() > 0)
            {
				CDraw::DrawText(paint, unit.c_str(), PCOLORS_UPCOLOR, &font, cleft + 47, top + 100);
            }
            unit = L"";
            double innerVol = m_latestData.m_innerVol;
            if (innerVol > 100000000)
            {
                innerVol /= 100000000;
                unit = L"亿";
            }
            else if (innerVol > 10000)
            {
                innerVol /= 10000;
                unit = L"万";
            }
			cleft = CDraw::DrawUnderLineNum(paint, innerVol, unit.length() > 0 ? m_digit : 0, &font, PCOLORS_DOWNCOLOR, true, 155, top + 100);
			if (unit.length() > 0)
            {
				CDraw::DrawText(paint, unit.c_str(), PCOLORS_DOWNCOLOR, &font, cleft + 157, top + 100);
            }
		}
		font.m_bold = false;
		font.m_fontSize = 20;
		if(m_securityCode.length() > 0)
		{
			CDraw::DrawText(paint, m_securityCode.c_str(), PCOLORS_FORECOLOR3, &font, 2, 4);
		}
		if(m_securityName.length() > 0)
		{
			CDraw::DrawText(paint, m_securityName.c_str(), yellowColor, &font,  100, 4);
		}
		_int64 frameColor = PCOLORS_LINECOLOR4;
		paint->DrawLine(frameColor, 1, 0, 0, 0, 0, height);
		paint->DrawLine(frameColor, 1, 0, 0, 30, width, 30);
		paint->DrawLine(frameColor, 1, 0, 24, 30, 24,  top - 2);
		paint->DrawLine(frameColor, 1, 0, 0, m_lV2 ? 208 : 130, width, m_lV2 ? 208: 130);
		paint->DrawLine(frameColor, 1, 0, 0, top - 2, width,  top - 2);
		paint->DrawLine(frameColor, 1, 0, width, 0, width, height);
		paint->DrawLine(frameColor, 1, 0, 0, top + 120, width,  top + 120);
	}
}

void LatestDiv::Update()
{
	ControlA::Update();
    if (m_gridTransaction)
    {
		int top = m_lV2 ? 510 : 352;
		m_gridTransaction->SetTop(top);
        int width = GetWidth(), height = GetHeight();
		SIZE gridSize = {width, height - top};
        if (gridSize.cy > 0)
        {
            m_gridTransaction->SetSize(gridSize);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

TransactionDateCell::TransactionDateCell()
{
}

TransactionDateCell::~TransactionDateCell()
{
}

void TransactionDateCell::OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate)
{
    int clipW = clipRect.right - clipRect.left;
    int clipH = clipRect.bottom - clipRect.top;
    if (clipW > 0 && clipH > 0)
    {
        GridA *grid = GetGrid();
        GridRow *row = GetRow();
        GridColumn *column = GetColumn();
        if (grid && row && column)
        {
            double value = GetDouble();
            String text = GetText();
			GridCellStyle *style = GetStyle();
            FONT *font = style->GetFont();
            _int64 foreColor = style->GetForeColor();
            SIZE tSize = paint->TextSize(text.c_str(), font);
			POINT tPoint = {rect.left, rect.top + clipH / 2 - tSize.cy / 2};
			RECT tRect = {tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy};
            paint->DrawText(text.c_str(), foreColor, font, tRect);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

TransactionDoubleCell::TransactionDoubleCell()
{
	m_digit = 0;
}

TransactionDoubleCell::~TransactionDoubleCell()
{
}

int TransactionDoubleCell::GetDigit()
{
	return m_digit;
}

void TransactionDoubleCell::SetDigit(int digit)
{
	m_digit = digit;
}

void TransactionDoubleCell::OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate)
{
	int clipW = clipRect.right - clipRect.left;
    int clipH = clipRect.bottom - clipRect.top;
    if (clipW > 0 && clipH > 0)
    {
        GridA *grid = GetGrid();
        GridRow *row = GetRow();
        GridColumn *column = GetColumn();
        if (grid && row && column)
        {
            double value = GetDouble();
            String text = L" ";
			GridCellStyle *style = GetStyle();
            FONT *font = style->GetFont();
            _int64 foreColor = style->GetForeColor();
            SIZE tSize = paint->TextSize(text.c_str(), font);
			POINT tPoint = {rect.left, rect.top + clipH / 2 - tSize.cy / 2};
			CDraw::DrawUnderLineNum(paint, value, m_digit, font, foreColor, false, tPoint.x, tPoint.y);
        }
    }
}
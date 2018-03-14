#include "..\\..\\stdafx.h"
#include "SecurityDataHelper.h"

SecurityDataHelper::SecurityDataHelper()
{
}

SecurityDataHelper::~SecurityDataHelper()
{
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CTable* SecurityDataHelper::CreateDataSource(ChartA *chart)
{
	CTable *dataSource = new CTable;
    dataSource->AddColumn(CLOSE_INDEX);
    dataSource->AddColumn(HIGH_INDEX);
    dataSource->AddColumn(LOW_INDEX);
    dataSource->AddColumn(OPEN_INDEX);
    dataSource->AddColumn(VOL_INDEX);
	dataSource->AddColumn(AMOUNT_INDEX);
	return dataSource;
}

CIndicator* SecurityDataHelper::CreateIndicator(ChartA *chart, CTable *dataSource, const String& text, const String& parameters)
{
    CIndicator *indicator = new CIndicator;
    indicator->SetDataSource(dataSource);
    indicator->SetName(L"");
    indicator->SetFullName(L"");
	if(dataSource)
	{
		indicator->SetSourceField(CLOSE, CLOSE_INDEX);
		indicator->SetSourceField(HIGH, HIGH_INDEX);
		indicator->SetSourceField(LOW, LOW_INDEX);
		indicator->SetSourceField(OPEN, OPEN_INDEX);
		indicator->SetSourceField(VOL, VOL_INDEX);
		indicator->SetSourceField(AMOUNT, AMOUNT_INDEX);
		indicator->SetSourceField(CLOSE.substr(0, 1), CLOSE_INDEX);
		indicator->SetSourceField(HIGH.substr(0, 1), HIGH_INDEX);
		indicator->SetSourceField(LOW.substr(0, 1), LOW_INDEX);
		indicator->SetSourceField(OPEN.substr(0, 1), OPEN_INDEX);
		indicator->SetSourceField(VOL.substr(0, 1), VOL_INDEX);
		indicator->SetSourceField(AMOUNT.substr(0, 1), AMOUNT_INDEX);
	}
    indicator->SetParameters(parameters);
	if(text.length() > 0)
	{
		indicator->SetScript(text);
	}
    return indicator;
}

void SecurityDataHelper::BindHistoryDatas(ChartA *chart, CTable *dataSource, vector<CIndicator*> *indicators, int *fields, vector<SecurityData> *historyDatas)
{
	dataSource->Clear();
    int size = (int)historyDatas->size();
    dataSource->SetRowsCapacity(size + 10);
    dataSource->SetRowsGrowStep(100);
	int columnsCount = dataSource->ColumnsCount();
    for (int i = 0; i < size; i++)
    {
        SecurityData securityData = (*historyDatas)[i];
		if(dataSource == chart->GetDataSource())
		{
			InsertData(chart, dataSource, fields, &securityData);
		}
		else
		{
			double *ary = new double[columnsCount];
            ary[0] = securityData.m_close;
            ary[1] = securityData.m_high;
            ary[2] = securityData.m_low;
            ary[3] = securityData.m_open;
            ary[4] = securityData.m_volume;
            for (int j = 5; j < columnsCount; j++)
            {
                ary[j] = dataSource->NaN;
            }
            dataSource->AddRow((double)securityData.m_date, ary, columnsCount);
		}
    }
    int indicatorsSize = (int)indicators->size();
    for (int i = 0; i < indicatorsSize; i++)
    {
        (*indicators)[i]->OnCalculate(0);
    }
}

int SecurityDataHelper::InsertData(ChartA *chart, CTable *dataSource, int *fields, SecurityData *securityData)
{
	double close = securityData->m_close, high = securityData->m_high, low = securityData->m_low, open = securityData->m_open, avgPrice = securityData->m_avgPrice, volume = securityData->m_volume, amount = securityData->m_amount;
    if (volume > 0 || close > 0)
    {
        if (high == 0)
        {
            high = close;
        }
        if (low == 0)
        {
            low = close;
        }
        if (open == 0)
        {
            open = close;
        }
        if (avgPrice == 0)
        {
            avgPrice = dataSource->NaN;
        }
    }
    else
    {
        close = dataSource->NaN;
        high = dataSource->NaN;
        low = dataSource->NaN;
        open = dataSource->NaN;
        volume = dataSource->NaN;
        amount = dataSource->NaN;
		avgPrice = dataSource->NaN;
    }
	double date = (double)securityData->m_date;
    dataSource->Set(date, fields[4], volume);
    int index = dataSource->GetRowIndex(date);
    dataSource->Set2(index, fields[0], close);
    dataSource->Set2(index, fields[1], high);
    dataSource->Set2(index, fields[2], low);
    dataSource->Set2(index, fields[3], open);
	dataSource->Set2(index, fields[5], amount);
	dataSource->Set2(index, fields[6], avgPrice);
    return index;
}

int SecurityDataHelper::InsertLatestData(ChartA *chart, CTable *dataSource, vector<CIndicator*> *indicators, int *fields, SecurityData *latestData)
{
	if(latestData->m_close > 0 && latestData->m_volume > 0)
	{
		int indicatorsSize = (int)indicators->size();
		int index = InsertData(chart, dataSource, fields, latestData);
		for (int i = 0; i < indicatorsSize; i++)
		{
			(*indicators)[i]->OnCalculate(index);
		}   
		return index;
	}
	else
	{
		return -1;
	}
}

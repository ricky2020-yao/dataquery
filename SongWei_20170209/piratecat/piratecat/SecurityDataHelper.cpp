#include "..\\..\\stdafx.h"
#include "SecurityDataHelper.h"

SecurityDataHelper::SecurityDataHelper()
{
}

SecurityDataHelper::~SecurityDataHelper()
{
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CIndicator* SecurityDataHelper::CreateIndicator(Chart* chart, CTable* dataSource, wstring text, wstring parameters)
{
    if (parameters.find(_T(",")) != -1)
    {
		vector<wstring> strs = CStr::Split(parameters, _T(";")); 
        parameters = _T("");
        int strsSize = strs.size();
        for (int i = 0; i < strsSize; i++)
        {
            if (strs[i].find(_T(",")) != -1)
            {
				vector<wstring> subStrs = CStr::Split(strs[i], _T(","));
                parameters += subStrs[0] + _T(":") + subStrs[3] + _T(";");
            }
        }
    }
    CIndicator *indicator = new CIndicator;
    indicator->SetDataSource(dataSource);
    indicator->SetName(_T(""));
    indicator->SetFullName(_T(""));
	indicator->SetSourceField(CLOSE, CLOSE_INDEX);
	indicator->SetSourceField(HIGH, HIGH_INDEX);
    indicator->SetSourceField(LOW, LOW_INDEX);
    indicator->SetSourceField(OPEN, OPEN_INDEX);
    indicator->SetSourceField(VOL, VOL_INDEX);
    indicator->SetParameters(parameters);
    indicator->SetScript(text);
    return indicator;
}

void SecurityDataHelper::BindHistoryDatas(Chart* chart, CTable* dataSource, vector<CIndicator*> indicators, vector<SecurityData>* historyDatas)
{
    int size = historyDatas->size();
    dataSource->SetRowsCapacity(size + 10);
    dataSource->SetRowsGrowStep(100);
	int columnsCount = dataSource->ColumnsCount();
    for (int i = 0; i < size; i++)
    {
        SecurityData securityData = (*historyDatas)[i];
		if(securityData.m_close > 0)
		{
			if (dataSource == chart.DataSource)
            {
                InsertData(chart, dataSource, &securityData);
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
                dataSource->AddRow(securityData.m_date, ary, columnsCount);
            }
		}
    }
    int indicatorsSize = indicators.size();
    for (int i = 0; i < indicatorsSize; i++)
    {
        indicators[i]->OnCalculate(0);
    }
}

void SecurityDataHelper::GetSecurityData(SecurityDataRuntime* runtimeData, SecurityData* securityData)
{
    securityData->m_date = runtimeData->m_date;
    securityData->m_close = runtimeData->m_close;
    securityData->m_high = runtimeData->m_high;
    securityData->m_low = runtimeData->m_low;
    securityData->m_open = runtimeData->m_open;
    securityData->m_volume = runtimeData->m_volume;
    securityData->m_adjclose = runtimeData->m_close;
}

int SecurityDataHelper::InsertData(Chart* chart, CTable* dataSource, SecurityData* securityData)
{
    int col = dataSource->GetColumnIndex(OPEN_INDEX);
    double datenum = securityData->m_date;
    dataSource->Set(datenum, VOL_INDEX, securityData->m_volume);
    int index = dataSource->RowsCount() - 1;
	double close = securityData->m_close, high = securityData->m_high, low = securityData->m_low, open = securityData->m_open, adjClose = securityData->m_adjclose;
    double amount = close * securityData->m_volume;
    if (close != adjClose)
    {
        double rate = adjClose / close;
        high *= rate;
        low *= rate;
        open *= rate;
        close = adjClose;
    }
    dataSource->Set2(index, OPEN_INDEX, open);
    dataSource->Set2(index, HIGH_INDEX, high);
    dataSource->Set2(index, LOW_INDEX, low);
    dataSource->Set2(index, CLOSE_INDEX, close);
    return index;
}

int SecurityDataHelper::InsertLastestData(Chart* chart, CTable* dataSource, vector<CIndicator*> indicators, SecurityDataRuntime* runtimeData)
{
    int indicatorsSize = indicators.size();
    SecurityData newData;
    GetSecurityData(runtimeData, &newData);
    int index = InsertData(chart, dataSource, &newData);
    for (int i = 0; i < indicatorsSize; i++)
    {
        indicators[i]->OnCalculate(index);
    }   
    return index;
}

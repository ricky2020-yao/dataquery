#include "..\\..\\stdafx.h"
#include "SecurityFilterTask.h"

SecurityFilterInfo::SecurityFilterInfo()
{
	m_completed = false;
	m_dataSource = 0;
}

SecurityFilterInfo::~SecurityFilterInfo()
{
	m_dataSource = 0;
	m_values.clear();
}

bool SecurityFilterInfo::IsCompleted()
{
	return m_completed;
}

void SecurityFilterInfo::SetCompleted(bool completed)
{
	m_completed = completed;
}

CTable* SecurityFilterInfo::GetDataSource()
{
	return m_dataSource;
}

void SecurityFilterInfo::SetDataSource(CTable *dataSource)
{
	m_dataSource = dataSource;
}

SecurityLatestData SecurityFilterInfo::GetLatestData()
{
	return m_latestData;
}

void SecurityFilterInfo::SetLatestData(SecurityLatestData latestData)
{
	m_latestData = latestData;
}

Security SecurityFilterInfo::GetSecurity()
{
	return m_security;
}

void SecurityFilterInfo::SetSecurity(Security security)
{
	m_security = security;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

double SecurityFilterInfo::GetValue(const String&  key)
{
    if (m_completed)
    {
		map<String, double>::iterator sIter = m_values.find(key);
		if(sIter != m_values.end())
		{
			return sIter->second;
		}
    }
    return m_dataSource->NaN;
}

void SecurityFilterInfo::SetValue(const String&  key, double value)
{
    if (!m_completed)
    {
        m_values[key] = value;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityFilterTask::LatestDataCallBack(CMessage *message, void *pInvoke)
{
	SecurityFilterTask *task = (SecurityFilterTask*)pInvoke;
	task->OnLatestDataCallBack(message);
}

void SecurityFilterTask::SecurityFilterDataCallBack(CMessage *message, void *pInvoke)
{
	SecurityFilterTask *task = (SecurityFilterTask*)pInvoke;
	task->OnSecurityFilterDataCallBack(message);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityFilterTask::SecurityFilterTask()
{
	m_chart = 0;
	m_dataSource = 0;
	m_indicator = 0;
	m_lastestDataCallBack = &LatestDataCallBack;
	m_matchCount = 0;
	m_native = 0;
	m_qtRequestID = BaseService::GetRequestID();
	m_quoteService = DataCenter::GetQuoteService();
	m_quoteService->RegisterListener(m_qtRequestID, m_lastestDataCallBack, this);
	m_requestID = BaseService::GetRequestID();
	m_runningState = -1;
	m_securityFilterService = DataCenter::GetSecurityFilterService();
	m_securityFilterDataCallBack = &SecurityFilterDataCallBack;
	m_sfRequestID = BaseService::GetRequestID();;
	m_securityFilterService->RegisterListener(m_sfRequestID, m_securityFilterDataCallBack, this);
}

SecurityFilterTask::~SecurityFilterTask()
{
    if (m_chart)
    {
        delete m_chart;
        m_chart = 0;
    }
    if (m_dataSource)
    {
        delete m_dataSource;
        m_dataSource = 0;
    }
	map<String, SecurityFilterInfo*>::iterator sIter = m_infos.begin();
	for(; sIter != m_infos.end(); ++sIter)
	{
		delete sIter->second;
	}
    m_infos.clear();
	if (m_lastestDataCallBack)
	{
		m_quoteService->UnRegisterListener(m_requestID, m_lastestDataCallBack);
		m_lastestDataCallBack = 0;
	}
	m_quoteService = 0;
	if(m_securityFilterDataCallBack)
	{
		Stop();
		m_securityFilterService->UnRegisterListener(m_sfRequestID, m_securityFilterDataCallBack);
		m_securityFilterDataCallBack = 0;
		m_securityFilterService = 0;
	}
    if (m_indicator)
    {
        delete m_indicator;
        m_indicator = 0;
    }
	m_quoteService = 0;
}

CTable* SecurityFilterTask::GetDataSource()
{
	return m_dataSource;
}

void SecurityFilterTask::SetDataSource(CTable *dataSource)
{
	m_dataSource = dataSource;
}

CIndicator* SecurityFilterTask::GetIndicator()
{
	return m_indicator;
}

void SecurityFilterTask::SetIndicator(CIndicator *indicator)
{
	m_indicator = indicator;
}

int SecurityFilterTask::GetMatchCount()
{
	return m_matchCount;
}

void SecurityFilterTask::SetMatchCount(int matchCount)
{
	m_matchCount = matchCount;
}

NativeBase* SecurityFilterTask::GetNative()
{
	return m_native;
}

void SecurityFilterTask::SetNative(NativeBase *native)
{
	m_native = native;
}

int SecurityFilterTask::GetRunningState()
{
	return m_runningState;
}

void SecurityFilterTask::SetRunningState(int runningState)
{
	m_runningState = runningState;
}

SecurityFilterTemplate SecurityFilterTask::GetTemplate()
{
	return m_template;
}

void SecurityFilterTask::SetTemplate(SecurityFilterTemplate sfTemplate)
{
	m_template = sfTemplate;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityFilterTask::GetSecurityInfos(vector<SecurityFilterInfo*> *infos)
{
	map<String, SecurityFilterInfo*>::iterator sIter = m_infos.begin();
	for(; sIter != m_infos.end(); ++sIter)
	{
		infos->push_back(sIter->second);
	}
}

int SecurityFilterTask::GetSecurityInfosSize()
{
	return (int)m_infos.size();
}

void SecurityFilterTask::OnLatestDataCallBack(CMessage *message)
{
    if (message->m_bodyLength > 0)
    {
		LatestDataInfo dataInfo;
        vector<SecurityLatestData> latestDatas;
		QuoteService::GetLatestDatas(&dataInfo, &latestDatas, message->m_body, message->m_bodyLength);
		vector<SecurityLatestData>::iterator sIter = latestDatas.begin();
		for(; sIter != latestDatas.end(); ++sIter)
		{
			SecurityLatestData latestData = *sIter;
			String code = latestData.m_securityCode;
			map<String, SecurityFilterInfo*>::iterator sIter2 = m_infos.find(code);
			if(sIter2 != m_infos.end())
			{
				sIter2->second->SetLatestData(latestData);
			}
		}
    }
}

void SecurityFilterTask::OnSecurityFilterDataCallBack(CMessage *message)
{
	if(message->m_bodyLength > 0)
	{
		int currentBatch = 0, totalBatch = 1;
		if(m_runningState != -1)
		{
			map<String, double*> datas;
			SecurityFilterService::GetFilterResults(&datas, &currentBatch, &totalBatch, message->m_body, message->m_bodyLength);
			if (datas.size() > 0)
			{
				map<String, double*>::iterator sIter = datas.begin();
				for(; sIter != datas.end(); ++sIter)
				{
					String code = sIter->first;
					double *result = sIter->second;
					map<String, SecurityFilterInfo*>::iterator sIter2 = m_infos.find(code);
					if(sIter2 != m_infos.end())
					{
						SecurityFilterInfo *info = sIter2->second;
						if (result)
						{
							int pos = 0;
							map<String, int>::iterator sIter3 = m_indicator->m_mainVariables.begin();
							for(; sIter3 != m_indicator->m_mainVariables.end(); ++sIter3)
							{
								String name = sIter3->first;
								double value = result[pos];
								info->SetValue(name, value);
								if (name == L"FILTER" && value == 1)
								{
									m_matchCount++;
								}
								pos++;
							}
						}
						m_runningState++;
						info->SetCompleted(true);
					}
					if(result)
					{
						delete[] result;
						result = 0;
					}
				}
			}
			datas.clear();
		}
        if (currentBatch == totalBatch)
        {
            m_runningState = -1;
        }
	}
}

void SecurityFilterTask::Run()
{
    if (!m_chart)
    {
		m_chart = new ChartA;
		m_chart->SetNative(m_native);
		m_dataSource = SecurityDataHelper::CreateDataSource(m_chart);
    }
	m_dataSource->Clear();
	String indicator = m_template.m_indicator;
    if (m_template.m_filter.length() > 0)
    {
        indicator += L"FILTER:=" + m_template.m_filter;
    }
    else
    {
        indicator += L"FILTER:=1";
    }
	if(m_indicator)
	{
		delete m_indicator;
	}
	m_indicator = SecurityDataHelper::CreateIndicator(m_chart, m_dataSource, indicator, m_template.m_parameters);
    m_indicator->OnCalculate(0);
	map<String, SecurityFilterInfo*>::iterator sIter = m_infos.begin();
	for(; sIter != m_infos.end(); ++sIter)
	{
		delete sIter->second;
	}
    m_infos.clear();
    vector<Security> securities;
	DataCenter::GetSecurityService()->GetSecuritiesByCodes(m_template.m_codes, &securities);
	int securitiesSize = (int)securities.size();
    for (int i = 0; i < securitiesSize; i++)
    {
        Security security = securities[i];
        SecurityFilterInfo *info = new SecurityFilterInfo();
        info->SetDataSource(m_dataSource);
        info->SetSecurity(security);
        m_infos[security.m_code] = info;
    }
	LatestDataInfo dataInfo;
	dataInfo.m_codes = m_template.m_codes;
	dataInfo.m_formatType = 1;
	m_quoteService->PushLatestDatas(m_qtRequestID, &dataInfo);
	//m_securityFilterService->StartFilter(m_sfRequestID, &m_template);
	//m_runningState = 0;
	m_runningState = securitiesSize;
    m_matchCount = 0;
}

void SecurityFilterTask::Stop()
{
	m_securityFilterService->StopFilter(m_sfRequestID, &m_template);
    m_runningState = -1;
}
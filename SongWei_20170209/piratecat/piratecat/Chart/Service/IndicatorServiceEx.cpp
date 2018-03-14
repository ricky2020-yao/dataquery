#include "..\\..\\stdafx.h"
#include "IndicatorServiceEx.h"

IndicatorServiceEx::IndicatorServiceEx()
{
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
	m_operatorRequestID = BaseService::GetRequestID();
}

IndicatorServiceEx::~IndicatorServiceEx()
{
	m_indicators.clear();
}

int IndicatorServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

bool IndicatorServiceEx::IsLoaded()
{
	return m_loaded;
}

int IndicatorServiceEx::GetOperatorRequestID()
{
	return m_operatorRequestID;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

bool IndicatorServiceEx::GetIndicatorByID(const String&  indicatorID, Indicator *indicator)
{
	vector<Indicator>::iterator sIter = m_indicators.begin();
	for(; sIter != m_indicators.end(); ++sIter)
	{
		if((*sIter).m_indicatorID == indicatorID)
		{
			*indicator = *sIter;
			return true;
		}
	}
	return false;
}

bool IndicatorServiceEx::GetIndicatorByName(const String&  name, Indicator *indicator)
{
	vector<Indicator>::iterator sIter = m_indicators.begin();
	for(; sIter != m_indicators.end(); ++sIter)
	{
		if((*sIter).m_name == name)
		{
			*indicator = *sIter;
			return true;
		}
	}
	return false;
}

bool IndicatorServiceEx::GetIndicators(vector<Indicator> *indicators)
{
	vector<Indicator>::iterator sIter = m_indicators.begin();
	for(; sIter != m_indicators.end(); ++sIter)
	{
		indicators->push_back(*sIter);
	}
	return true;
}

void IndicatorServiceEx::OnReceive(CMessage *message)
{
	if(message->m_functionID != FUNCTIONID_INDICATOR_GETINDICATORS)
	{
		message->m_requestID = m_operatorRequestID;
	}
    vector<Indicator> indicators;
	IndicatorService::GetIndicators(&indicators, message->m_body, message->m_bodyLength);
    int indicatorsSize = (int)indicators.size();
	switch (message->m_functionID)
    {
		case FUNCTIONID_INDICATOR_GETINDICATORS:
			{
				m_indicators = indicators;
				m_loaded = true;
				break;
			}
        case FUNCTIONID_INDICATOR_ADDINDICATORS:
            {
				bool add = false;
                for (int i = 0; i < indicatorsSize; i++)
                {
					bool has = false;
					vector<Indicator>::iterator sIter = m_indicators.begin();
					for(; sIter != m_indicators.end(); ++sIter)
					{
						if((*sIter).m_indicatorID == indicators[i].m_indicatorID)
						{
							has = true;
							break;
						}
					}
					if(!has)
					{
						m_indicators.push_back(indicators[i]);
						add = true;
					}
                }
				if(!add)
				{
					return;
				}
                break;
            }
        case FUNCTIONID_INDICATOR_DELETEINDICATORS:
            {
                for (int i = 0; i < indicatorsSize; i++)
                {
					vector<Indicator>::iterator sIter = m_indicators.begin();
					for(; sIter != m_indicators.end(); ++sIter)
					{
						if((*sIter).m_indicatorID == indicators[i].m_indicatorID)
						{
							m_indicators.erase(sIter);
							break;
						}
					}
                }
                break;
            }
        case FUNCTIONID_INDICATOR_UPDATEINDICATORS:
            {
                for (int i = 0; i < indicatorsSize; i++)
                {
					Indicator updateIndicator = indicators[i];
					int curIndicatorsSize = (int)m_indicators.size();
					for(int j = 0; j < curIndicatorsSize; j++)
					{
						Indicator indicator = m_indicators[j];
						if(indicator.m_indicatorID == updateIndicator.m_indicatorID)
						{
							m_indicators[j] =  updateIndicator;
							break;
						}
					}
                }
                break;
            }
    }
	IndicatorService::OnReceive(message);
}

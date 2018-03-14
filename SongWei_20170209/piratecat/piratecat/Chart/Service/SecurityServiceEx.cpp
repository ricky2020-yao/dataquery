#include "..\\..\\stdafx.h"
#include "SecurityServiceEx.h"

SecurityOrder::SecurityOrder()
{
	m_order = 0;
	m_security = 0;
}

SecurityOrder::~SecurityOrder()
{
	m_security = 0;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

DWORD WINAPI LoadSecuritiesToCache(LPVOID lpParam)
{
	SecurityServiceEx *securityService = (SecurityServiceEx*)lpParam;
	securityService->Load();
	return 1;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityServiceEx::SecurityServiceEx()
{
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
}

SecurityServiceEx::~SecurityServiceEx()
{
}

int SecurityServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

bool SecurityServiceEx::IsLoaded()
{
	return m_loaded;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

bool SecurityServiceEx::CompareMakeupSecutitycode(const String& code, const String& makeupCode)
{
	if (code == makeupCode)
	{
		return true;
	}
	int index = StrStr(code, makeupCode);
	if (index != 0)
	{
		return false;
	}
	if (code.length() > makeupCode.length())
	{
		if(code.substr(makeupCode.length(), 1) == L".")
		{
			return true;
		}
	}
	return false;
}

int SecurityServiceEx::Find(const String& key, int limitSize, CList<Security*>* securities)
{
	int order = 0, order1 = 0, order2 = 0, order3 = 0, minOrder = 0;
	int securityListSize = m_securities->size();
    CList<SecurityOrder*> securityReturnList;
    securityReturnList.set_capacity(limitSize);
    for (int i = 0; i < securityListSize; i++)
    {
        CList<Security*> *securityList = m_securities->get(i);
        int securitySize = securityList->size();
        for (int j = 0; j < securitySize; j++)
        {
            Security* security = securityList->get(j);
            order1 = IsMatching(security->m_code, security->m_type, key, 0, i);
            order2 = IsMatching(security->m_name, security->m_type, key, 1, i);
            order3 = IsMatching(security->m_pingyin, security->m_type, key, 2, i);
            order = max(order1, max(order2, order3));
            if (order > 0)
            {
                SecurityOrder* securityReturn = new SecurityOrder();
                securityReturn->m_order = order;
                securityReturn->m_security = security;                        
                if (securityReturnList.size() < limitSize)
                {
                    securityReturnList.push_back(securityReturn);
                }
                else
                {
                    int index = 0;    
                    minOrder = securityReturnList.get(0)->m_order;
                    for (int m = 0; m < securityReturnList.size(); m++)
                    {
                        if (securityReturnList.get(m)->m_order < minOrder)
                        {
                            minOrder = securityReturnList.get(m)->m_order;
                            index = m;
                        }
                    }
                    if (minOrder < order)
                    {
						SecurityOrder* ret = securityReturnList.get(index);
						ret->m_order = order;
                        ret->m_security = security;
                    }
                }
            }
        }
    }
    int retListSize = securityReturnList.size();
    SecurityOrder *security1 = 0;
	SecurityOrder *security2 = 0; 
	string str1 = "", str2 = "";
    for (int i = 0; i < retListSize - 1; i++)
    {
        for (int j = i + 1; j < retListSize; j++)
        {
            security1 = securityReturnList.get(i);
            security2 = securityReturnList.get(j);
			CStr::wstringTostring(str1, security1->m_security->m_code);
			CStr::wstringTostring(str2, security2->m_security->m_code);
            if (security1->m_order < security2->m_order
				|| (security1->m_order == security2->m_order && str1.compare(str2) > 0))
            {
                securityReturnList.set(i, security2);
                securityReturnList.set(j, security1);
            }
        }
    }
    for (int i = 0; i < retListSize; i++)
    { 
        securities->push_back(securityReturnList.get(i)->m_security);
		delete securityReturnList.get(i);
    }
	securityReturnList.clear();
    return securities->size();
}

bool SecurityServiceEx::FilterSecurities(const String& key, CList<Security*>* securities)
{
	Find(key, 10, securities);
	return true;
}

bool SecurityServiceEx::GetSecurityByCode(const String&  code, Security *security)
{
	String key = CStr::ToUpper(code);
	key = CStr::Replace(key, L"\r", L"");
	key = CStr::Replace(key, L"\n", L"");
	map<String, Security*>::iterator sIter = m_securitiesMap.find(key);
	if(sIter != m_securitiesMap.end())
	{
        *security = *sIter->second;
        return true;
    }
    else
    {
        return false;
    }
	return true;
}

bool SecurityServiceEx::GetSecuritiesByCodes(const String& codes, vector<Security> *securities)
{
	vector<String> strCodes = CStr::Split(codes, L",");
	vector<String>::iterator sIter = strCodes.begin();
	for(; sIter != strCodes.end(); ++sIter)
	{
		String code = CStr::ToUpper(*sIter);
		map<String, Security*>::iterator sIter = m_securitiesMap.find(code);
		if(sIter != m_securitiesMap.end())
		{
			securities->push_back(*sIter->second);
		}
	}
	return true;
}

int SecurityServiceEx::IsMatching(const String& securityVal, int securityType, const String& input, int type, int orderIndex)
{
	 int len1 = (int)securityVal.length(), len2 = (int)input.length();
    int ret = 0;
	int index = StrStr(securityVal, input);		
    if (index == -1)
    {
        return ret;
    }
    else if (index == 0)
    {
        ret += 2000;
        if (securityVal == input)
        {
            ret += 6500;
        }
        if (type == 0)
        {
            if (len1 > len2)
            {
                if (securityVal[len2] == '.')
                {
                    ret += 6500;
                }
            }
            if (MakeupSecurity(securityVal, input, securityType))
            {
                ret += 3500;
                if ((securityType == 1) && (len2 <= 3))
                {
                    ret += 1;
                }
            }
        }
    }
    else 
    {
        ret += 500;
        if (type == 0)
        {
            if (MakeupSecurity(securityVal, input, securityType))
            {
                ret += 5000;
                if (securityType == 1 && len2 <= 3)
                {
                    ret += 1;
                }
            }
        }
		String rest = securityVal.substr(index);
        if (rest.compare(input) == 0)
        {
            ret += 1000;
        }
        if (type == 0)
        {
            if ((int)rest.length() > len2)
            {
                if (rest[len2] == '.')
                {
                    ret += 500;
                }
            }
        }
    }
    ret = ret - orderIndex * 5;
	return ret;
}

void SecurityServiceEx::Load()
{
	m_securities = new CList<CList<Security*>*>;
	m_securities->set_capacity(200);
	for(int i = 0; i < 200; i++)
	{
		CList<Security*>* list = new CList<Security*>();
		m_securities->push_back(list);
	}
	int securitiesSize = (int)m_securitiesCache.size();
	int capacities[1000] = {0};
	for (int i = 0; i < securitiesSize; i++)
    {
        Security security = m_securitiesCache[i];
        capacities[security.m_type] = capacities[security.m_type] + 1;
    }
	for(int i = 0; i < 200; i++)
	{
		CList<Security*>* list = m_securities->get(i);
		list->set_capacity(capacities[i]);
	}
	for (int i = 0; i < securitiesSize; i++)
	{
		Security security = m_securitiesCache[i];
		CList<Security*>* p = m_securities->get(security.m_type);
		if(p)
		{
			Security *newSecurity = new Security;
			newSecurity->m_code = security.m_code;
			newSecurity->m_name = security.m_name;
			newSecurity->m_pingyin = security.m_pingyin;
			newSecurity->m_status = security.m_status;
			newSecurity->m_type = security.m_type;
			m_securitiesMap[CStr::ToUpper(security.m_code)] = newSecurity;
			p->push_back(newSecurity);
		}		
	}
	m_securitiesCache.clear();
	m_loaded = true;
}

void SecurityServiceEx::LoadSecurities()
{
    GetSecurities(m_getListRequestID);
}

bool SecurityServiceEx::MakeupSecurity(const String& code, const String& input, int type)
{
	String zore = L"", compare = L"";
	bool ret = false;
	int len = (int)input.length();
	if (code == L"" || input == L"" || len > 5)
	{
		return ret;
	}
	int zorenum = 5 - len;
	for (int i = 0; i < zorenum; i++)
	{
		zore += L"0";
	}
	if (type == 1)
	{
		compare += L"6";
		compare += zore;
		compare += input;
		ret = CompareMakeupSecutitycode(code, compare);
	}
	if(ret)
	{
		return ret;
	}
	compare = L"";
	if(type == 2)
	{
		compare += L"3";
		compare += zore;
		compare += input;
		ret = CompareMakeupSecutitycode(code, compare);
	}
	if(ret)
	{
		return ret;
	}
	compare = L"";
	if(len <=4 && (type ==71 || type == 72))
	{
		compare += zore;
		compare += input;
		ret = CompareMakeupSecutitycode(code, compare);
	}
	if(ret)
	{
		return ret;
	}
	compare = L"";
	if(type == 2 && len <=3)
	{
		compare += L"002";
		zorenum = 6 - len - 3;
		for(int i = 0; i < zorenum; i++)
		{
			compare += L"0";
		}
		compare += input;
		ret = CompareMakeupSecutitycode(code, compare);
	}
	return ret;
}

void SecurityServiceEx::OnReceive(CMessage *message)
{
	if (message->m_functionID == FUNCTIONID_SECURITY_GETSECURITIES)
	{
		GetSecurities(&m_securitiesCache, message->m_functionID, message->m_body, message->m_bodyLength);
		HANDLE hThread = ::CreateThread(0, 0, LoadSecuritiesToCache, (LPVOID)this, 0, NULL);
		::CloseHandle(hThread);
	}
	SecurityService::OnReceive(message);
}

int SecurityServiceEx::StrStr(const String& str1, const String& str2)
{
	String newStr1 = CStr::ToLower(str1);
	String newStr2 = CStr::ToLower(str2);
	int result = -1, i = 0, j = 0, len1 = (int)newStr1.length(), len2 = (int)newStr2.length();
	if (len1 >= len2)
	{
		while (i <= len2 - 1)
		{
			if (newStr1[i] == newStr2[j])
			{
				if (j == len2 - 1)
				{
					result = i - j;
					break;
				}
				else
				{
					j++;
				}
			}
			else
			{
				i -= j;
				j = 0;
			}
			i++;
		}
	}
	return result;
}

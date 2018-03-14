#include "..\\..\\stdafx.h"
#include "UserSecurityServiceEx.h"

UserSecurityServiceEx::UserSecurityServiceEx()
{
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
	m_operatorRequestID = BaseService::GetRequestID();
}

UserSecurityServiceEx::~UserSecurityServiceEx()
{
	m_categories.clear();
	m_visitsCount.clear();
}

int UserSecurityServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

int UserSecurityServiceEx::GetOperatorRequestID()
{
	return m_operatorRequestID;
}

bool UserSecurityServiceEx::IsLoaded()
{
	return m_loaded;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int UserSecurityServiceEx::AddVisitsCount(String code)
{
	UserSecurityVisitsCount visitsCount;
	visitsCount.m_userID = DataCenter::GetUserID();
    visitsCount.m_codes[code] = 1;
	dynamic_cast<UserSecurityService*>(this)->AddVisitsCount(0, &visitsCount);
	if (m_visitsCount.find(code) != m_visitsCount.end())
    {
        m_visitsCount[code] = m_visitsCount[code] + 1;
    }
    else
    {
        m_visitsCount[code] = 1;
    }
    return 1;
}

bool UserSecurityServiceEx::GetCategory(const String&  categoryID, UserSecurityCategory *category)
{
	vector<UserSecurityCategory>::iterator sIter = m_categories.begin();
	for(; sIter != m_categories.end(); ++sIter)
	{
		if((*sIter).m_categoryID == categoryID)
		{
			*category = *sIter;
			return true;
		}
	}
	return false;
}

bool UserSecurityServiceEx::GetCategories(vector<UserSecurityCategory> *categories)
{
	vector<UserSecurityCategory>::iterator sIter = m_categories.begin();
	for(; sIter != m_categories.end(); ++sIter)
	{
		categories->push_back(*sIter);
	}
	return false;
}

void UserSecurityServiceEx::OnReceive(CMessage *message)
{
	if (message->m_functionID == FUNCTIONID_USERSECURITY_GETVISITSCOUNT)
	{
		UserSecurityVisitsCount visitsCount;
		Binary binary;
		binary.Write(message->m_body, message->m_bodyLength);
		visitsCount.m_userID = binary.ReadInt();
		String str = L"";
		binary.ReadString(str);
		visitsCount.CodesFromString(str);
		m_visitsCount = visitsCount.m_codes;
	}
	else
	{
		if(message->m_functionID != FUNCTIONID_USERSECURITY_GETCATEGORIES)
		{
			message->m_requestID = m_operatorRequestID;
		}
		vector<UserSecurityCategory> categories;
		UserSecurityService::GetCategories(&categories, message->m_body, message->m_bodyLength);
		int categoriesSize = (int)categories.size();
		switch (message->m_functionID)
		{
			case FUNCTIONID_USERSECURITY_GETCATEGORIES:
				{
					m_categories = categories;
					m_loaded = true;
					break;
				}
			case FUNCTIONID_USERSECURITY_ADDCATEGORIES:
				{
					bool add = false;
					for (int i = 0; i < categoriesSize; i++)
					{
						bool has = false;
						vector<UserSecurityCategory>::iterator sIter = m_categories.begin();
						for(; sIter != m_categories.end(); ++sIter)
						{
							if((*sIter).m_categoryID == categories[i].m_categoryID)
							{
								has = true;
								break;
							}
						}
						if(!has)
						{
							m_categories.push_back(categories[i]);
							add = true;
						}
					}
					if(!add)
					{
						return;
					}
					break;
				}
			case FUNCTIONID_USERSECURITY_DELETECATEGORIES:
				{
					for (int i = 0; i < categoriesSize; i++)
					{
						vector<UserSecurityCategory>::iterator sIter = m_categories.begin();
						for(; sIter != m_categories.end(); ++sIter)
						{
							if((*sIter).m_categoryID == categories[i].m_categoryID)
							{
								m_categories.erase(sIter);
								break;
							}
						}
					}
					break;
				}
			case FUNCTIONID_USERSECURITY_UPDATECATEGORIES:
				{
					for (int i = 0; i < categoriesSize; i++)
					{
						UserSecurityCategory updateCategory = categories[i];
						int curCategoriesSize = (int)m_categories.size();
						for(int j = 0; j < curCategoriesSize; j++)
						{
							UserSecurityCategory category = m_categories[j];
							if(category.m_categoryID == updateCategory.m_categoryID)
							{
								m_categories[j] =  updateCategory;
								break;
							}
						}
					}
					break;
			case FUNCTIONID_USERSECURITY_ADDSECURITIES:
				{
					for (int i = 0; i < categoriesSize; i++)
					{
						UserSecurityCategory updateCategory = categories[i];
						int curCategoriesSize = (int)m_categories.size();
						for (int j = 0; j < curCategoriesSize; j++)
						{
							UserSecurityCategory category = m_categories[j];
							if (category.m_categoryID == updateCategory.m_categoryID)
							{
								if (category.m_codes.length() > 0)
								{
									updateCategory.m_codes = m_categories[j].m_codes + L"," + updateCategory.m_codes;
								}
								m_categories[j] = updateCategory;
								break;
							}
						}
					}
					break;
				}
			case FUNCTIONID_USERSECURITY_DELETESECURITIES:
				{
					for (int i = 0; i < categoriesSize; i++)
					{
						UserSecurityCategory updateCategory = categories[i];
						int curCategoriesSize = (int)m_categories.size();
						for (int j = 0; j < curCategoriesSize; j++)
						{
							UserSecurityCategory category = m_categories[j];
							if (category.m_categoryID == updateCategory.m_categoryID)
							{
								if (updateCategory.m_codes.length()> 0 && category.m_codes.length() > 0)
								{
									vector<String> deleteCodes = CStr::Split(updateCategory.m_codes, L",");
									int deleteCodesSize = (int)deleteCodes.size();
									map<String, String> deleteCodesMap;
									for (int m = 0; m < deleteCodesSize; m++)
									{
										deleteCodesMap[deleteCodes[m]] = L"";
									}
									vector<String> codes = CStr::Split(category.m_codes, L",");
									int codesSize = (int)codes.size();
									String newCodes = L"";
									for (int m = 0; m < codesSize; m++)
									{
										if (deleteCodesMap.find(codes[m]) == deleteCodesMap.end())
										{
											newCodes += codes[m] + L",";
										}
									}
									if (newCodes.length() > 0)
									{
										if (newCodes.substr(newCodes.length() - 1) == L",")
										{
											newCodes = newCodes.substr(0, newCodes.length() - 1);
										}
									}
									updateCategory.m_codes = newCodes;
								}
								m_categories[j] = updateCategory;
								break;
							}
						}
					}
					break;
				}
			}
		}
	}
	UserSecurityService::OnReceive(message);
}

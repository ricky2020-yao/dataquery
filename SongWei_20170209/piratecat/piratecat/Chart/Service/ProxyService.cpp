#include "..\\..\\stdafx.h"
#include "..\\..\\..\\includes\\Sock\\CStr.h"
#include "ProxyService.h"
#include "..\\..\\..\\third_party\\SQLite\sqlite3x.hpp"
using namespace sqlite3x;

ProxyInfo::ProxyInfo()
{
	m_domain = L"";
	m_ip = L"";
	m_name = L"";
	m_port = 0;
	m_pwd = L"";
	m_type = 0;
	m_use = false;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ProxyService::ProxyService()
{
	m_createTableSQL = "CREATE TABLE PROXYINFOS(TYPE INTEGER PRIMARY KEY, IP, PORT INTEGER, NAME, PASSWORD, DOMAIN, USE INTEGER)";
	string dataPath = DataCenter::GetUserPath() + "\\data";
	if(!CFileA::IsDirectoryExist(dataPath.c_str()))
	{
		CFileA::CreateDirectory(dataPath.c_str());
	}
	string path = dataPath + "\\proxyinfos.db";
	CStrA::stringTowstring(m_dataBasePath, path);
	bool createDataBase = !CFileA::IsFileExist(path.c_str());
	if(createDataBase)
	{
		CreateTable();
	}
}

ProxyService::~ProxyService()
{
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int ProxyService::AddProxyInfo(ProxyInfo *proxyInfo)
{
	int strLen = sizeof(int) * 3+ CStrA::GetWStringCapacity(proxyInfo->m_ip) + CStrA::GetWStringCapacity(proxyInfo->m_name) + CStrA::GetWStringCapacity(proxyInfo->m_pwd) + CStrA::GetWStringCapacity(proxyInfo->m_domain) + 200;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"INSERT INTO PROXYINFOS(TYPE, IP, PORT, NAME, PASSWORD, DOMAIN, USE) values (%d,'%s',%d,'%s','%s','%s',%d)",
		proxyInfo->m_type, CStrA::GetDBString(proxyInfo->m_ip).c_str(), proxyInfo->m_port, CStrA::GetDBString(proxyInfo->m_name).c_str(),
		CStrA::GetDBString(proxyInfo->m_pwd).c_str(), CStrA::GetDBString(proxyInfo->m_domain).c_str(), proxyInfo->m_use ? 1: 0);

	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	return 1;
}

void ProxyService::CreateTable()
{
	const wchar_t *strPath = m_dataBasePath.c_str();
	sqlite3_connection conn(strPath);
	conn.executenonquery(m_createTableSQL.c_str());
	conn.close();
	CreateDefaultProxyInfos();
}

void ProxyService::CreateDefaultProxyInfos()
{
	ProxyInfo proxyInfo1;
	proxyInfo1.m_use = true;
	AddProxyInfo(&proxyInfo1);
	ProxyInfo proxyInfo2;
	proxyInfo2.m_type = 1;
	AddProxyInfo(&proxyInfo2);
	ProxyInfo proxyInfo3;
	proxyInfo3.m_type = 2;
	AddProxyInfo(&proxyInfo3);
	ProxyInfo proxyInfo4;
	proxyInfo4.m_type = 3;
	AddProxyInfo(&proxyInfo4);
}

int ProxyService::GetProxyInfos(vector<ProxyInfo> *proxyInfos)
{
	int strLen = 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, 0, strLen * sizeof(wchar_t));
	_stprintf_s(sql, strLen - 1, L"SELECT * FROM PROXYINFOS");
	sqlite3_connection conn(m_dataBasePath.c_str());
	sqlite3_command cmd(conn, sql);
	sqlite3_reader reader = cmd.executereader();
	while(reader.read())
	{
		ProxyInfo proxyInfo;
		proxyInfo.m_type = reader.getint(0);
		proxyInfo.m_ip = reader.getstring16(1);
		proxyInfo.m_port = reader.getint(2);
		proxyInfo.m_name = reader.getstring16(3);
		proxyInfo.m_pwd = reader.getstring16(4);
		proxyInfo.m_domain = reader.getstring16(5);
		proxyInfo.m_use = reader.getint(6) == 1;
		proxyInfos->push_back(proxyInfo);
	}
	reader.close();
	delete[] sql;
	return 1;
}

int ProxyService::UpdateProxyInfo(ProxyInfo *proxyInfo)
{
	int strLen = sizeof(int) * 3+ CStrA::GetWStringCapacity(proxyInfo->m_ip) + CStrA::GetWStringCapacity(proxyInfo->m_name) + CStrA::GetWStringCapacity(proxyInfo->m_pwd) + CStrA::GetWStringCapacity(proxyInfo->m_domain) + 200;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"UPDATE PROXYINFOS SET IP = '%s', PORT = %d, NAME = '%s', PASSWORD = '%s', DOMAIN='%s', USE = %d WHERE TYPE = %d"
		, CStrA::GetDBString(proxyInfo->m_ip).c_str(), proxyInfo->m_port, CStrA::GetDBString(proxyInfo->m_name).c_str(),
		CStrA::GetDBString(proxyInfo->m_pwd).c_str(), CStrA::GetDBString(proxyInfo->m_domain).c_str(), proxyInfo->m_use ? 1: 0, proxyInfo->m_type);
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	return 1;
}
#ifndef __SECURITY_H__
#define __SECURITY_H__
#pragma once

static String CLOSE = L"CLOSE";
static String OPEN = L"OPEN";
static String HIGH = L"HIGH";
static String LOW = L"LOW";
static String VOL = L"VOL";
static String AMOUNT = L"AMOUNT";
static String AVGPRICE = L"AVGPRICE";
static int CLOSE_INDEX = 0;
static int OPEN_INDEX = 1;
static int HIGH_INDEX = 2;
static int LOW_INDEX = 3;
static int VOL_INDEX = 4;
static int AMOUNT_INDEX = 5;
static int AVGPRICE_INDEX = 6;

class Indicator
{
public:
	Indicator();
	String m_category;
    String m_coordinate;
    String m_description;
    int m_digit;
    String m_indicatorID;
    String m_name;
    int m_orderNum;
    int m_paintType;
    String m_parameters;
    String m_password;
    String m_specialCoordinate;
    String m_text;
    int m_type;
    int m_usePassword;
    int m_userID;
    int m_version;
};

class Security
{
public:
	Security();
	String m_code;
	String m_name;
	String m_pingyin;
	int m_type;
	int m_status;
};

class SecurityData
{
public:
	SecurityData();
	float m_avgPrice;
	double m_date;
	float m_open;
	float m_high;
	float m_low;
	float m_close;
	double m_volume;
	double m_amount;
	void Copy(SecurityData *data)
	{
		m_avgPrice = data->m_avgPrice;
		m_date = data->m_date;
		m_open = data->m_open;
		m_high = data->m_high;
		m_low = data->m_low;
		m_close = data->m_close;
		m_volume = data->m_volume;
		m_amount = data->m_amount;
	}
};

class SecurityLatestData
{
public:
	SecurityLatestData();
	double m_amount;
    int m_buyVolume1;
    int m_buyVolume2;
    int m_buyVolume3;
    int m_buyVolume4;
    int m_buyVolume5;
    float m_buyPrice1;
    float m_buyPrice2;
    float m_buyPrice3;
    float m_buyPrice4;
    float m_buyPrice5;
    float m_close;
    double m_date;
    float m_high;
	int m_innerVol;
    float m_lastClose;
    float m_low;
    float m_open;
	double m_openInterest;
	int m_outerVol;
    String m_securityCode;
    int m_sellVolume1;
    int m_sellVolume2;
    int m_sellVolume3;
    int m_sellVolume4;
    int m_sellVolume5;
    float m_sellPrice1;
    float m_sellPrice2;
    float m_sellPrice3;
    float m_sellPrice4;
    float m_sellPrice5;
	float m_settlePrice;
	float m_turnoverRate;
    double m_volume;

	void Copy(SecurityLatestData *data)
	{
		if (!data) return;
        m_amount = data->m_amount;
        m_buyVolume1 = data->m_buyVolume1;
        m_buyVolume2 = data->m_buyVolume2;
        m_buyVolume3 = data->m_buyVolume3;
        m_buyVolume4 = data->m_buyVolume4;
        m_buyVolume5 = data->m_buyVolume5;
        m_buyPrice1 = data->m_buyPrice1;
        m_buyPrice2 = data->m_buyPrice2;
        m_buyPrice3 = data->m_buyPrice3;
        m_buyPrice4 = data->m_buyPrice4;
        m_buyPrice5 = data->m_buyPrice5;
        m_close = data->m_close;
        m_date = data->m_date;
        m_high = data->m_high;
        m_innerVol = data->m_innerVol;
        m_lastClose = data->m_lastClose;
        m_low = data->m_low;
        m_open = data->m_open;
		m_openInterest = data->m_openInterest;
		m_outerVol = data->m_outerVol;
        m_securityCode = data->m_securityCode;
        m_sellVolume1 = data->m_sellVolume1;
        m_sellVolume2 = data->m_sellVolume2;
        m_sellVolume3 = data->m_sellVolume3;
        m_sellVolume4 = data->m_sellVolume4;
        m_sellVolume5 = data->m_sellVolume5;
        m_sellPrice1 = data->m_sellPrice1;
        m_sellPrice2 = data->m_sellPrice2;
        m_sellPrice3 = data->m_sellPrice3;
        m_sellPrice4 = data->m_sellPrice4;
        m_sellPrice5 = data->m_sellPrice5;
		m_settlePrice = data->m_settlePrice;
		m_turnoverRate = data->m_turnoverRate;
        m_volume = data->m_volume;
	}

	bool Equal(SecurityLatestData *data)
	{
		if (!data) return false;
        if (m_amount == data->m_amount
        && m_buyVolume1 == data->m_buyVolume1
        && m_buyVolume2 == data->m_buyVolume2
        && m_buyVolume3 == data->m_buyVolume3
        && m_buyVolume4 == data->m_buyVolume4
        && m_buyVolume5 == data->m_buyVolume5
        && m_buyPrice1 == data->m_buyPrice1
        && m_buyPrice2 == data->m_buyPrice2
        && m_buyPrice3 == data->m_buyPrice3
        && m_buyPrice4 == data->m_buyPrice4
        && m_buyPrice5 == data->m_buyPrice5
        && m_close == data->m_close
        && m_date == data->m_date
        && m_high == data->m_high
        && m_innerVol == data->m_innerVol
        && m_lastClose == data->m_lastClose
        && m_low == data->m_low
        && m_open == data->m_open
		&& m_openInterest == data->m_openInterest
		&& m_outerVol == data->m_outerVol
        && m_securityCode == data->m_securityCode
        && m_sellVolume1 == data->m_sellVolume1
        && m_sellVolume2 == data->m_sellVolume2
        && m_sellVolume3 == data->m_sellVolume3
        && m_sellVolume4 == data->m_sellVolume4
        && m_sellVolume5 == data->m_sellVolume5
        && m_sellPrice1 == data->m_sellPrice1
        && m_sellPrice2 == data->m_sellPrice2
        && m_sellPrice3 == data->m_sellPrice3
        && m_sellPrice4 == data->m_sellPrice4
        && m_sellPrice5 == data->m_sellPrice5
		&& m_settlePrice == data->m_settlePrice
		&& m_turnoverRate == data->m_turnoverRate
        && m_volume == data->m_volume)
        {
            return true;
        }
		return false;
	}
};

class SecurityLatestDataLV2
{
public:
	SecurityLatestDataLV2();
    double m_allBuyVol;
    double m_allSellVol;
    float m_avgBuyPrice;
    float m_avgSellPrice;
    int m_buyVolume6;
    int m_buyVolume7;
    int m_buyVolume8;
    int m_buyVolume9;
    int m_buyVolume10;
    float m_buyPrice6;
    float m_buyPrice7;
    float m_buyPrice8;
    float m_buyPrice9;
    float m_buyPrice10;
    String m_securityCode;
    int m_sellVolume6;
    int m_sellVolume7;
    int m_sellVolume8;
    int m_sellVolume9;
    int m_sellVolume10;
    float m_sellPrice6;
    float m_sellPrice7;
    float m_sellPrice8;
    float m_sellPrice9;
    float m_sellPrice10;

	void Copy(SecurityLatestDataLV2 *data)
	{
		if (!data) return;
	    m_allBuyVol = data->m_allBuyVol;
        m_allSellVol = data->m_allSellVol;
        m_avgBuyPrice = data->m_avgBuyPrice;
        m_avgSellPrice = data->m_avgSellPrice;
        m_buyVolume6 = data->m_buyVolume6;
        m_buyVolume7 = data->m_buyVolume7;
        m_buyVolume8 = data->m_buyVolume8;
        m_buyVolume9 = data->m_buyVolume9;
        m_buyVolume10 = data->m_buyVolume10;
        m_buyPrice6 = data->m_buyPrice6;
        m_buyPrice7 = data->m_buyPrice7;
        m_buyPrice8 = data->m_buyPrice8;
        m_buyPrice9 = data->m_buyPrice9;
        m_buyPrice10 = data->m_buyPrice10;
        m_securityCode = data->m_securityCode;
        m_sellVolume6 = data->m_sellVolume6;
        m_sellVolume7 = data->m_sellVolume7;
        m_sellVolume8 = data->m_sellVolume8;
        m_sellVolume9 = data->m_sellVolume9;
        m_sellVolume10 = data->m_sellVolume10;
        m_sellPrice6 = data->m_sellPrice6;
        m_sellPrice7 = data->m_sellPrice7;
        m_sellPrice8 = data->m_sellPrice8;
        m_sellPrice9 = data->m_sellPrice9;
        m_sellPrice10 = data->m_sellPrice10;
	}

	bool Equal(SecurityLatestDataLV2 *data)
	{
		if (!data) return false;
	    if (m_allBuyVol == data->m_allBuyVol
        && m_allSellVol == data->m_allSellVol
        && m_avgBuyPrice == data->m_avgBuyPrice
        && m_avgSellPrice == data->m_avgSellPrice
        && m_buyVolume6 == data->m_buyVolume6
        && m_buyVolume7 == data->m_buyVolume7
        && m_buyVolume8 == data->m_buyVolume8
        && m_buyVolume9 == data->m_buyVolume9
        && m_buyVolume10 == data->m_buyVolume10
        && m_buyPrice6 == data->m_buyPrice6
        && m_buyPrice7 == data->m_buyPrice7
        && m_buyPrice8 == data->m_buyPrice8
        && m_buyPrice9 == data->m_buyPrice9
        && m_buyPrice10 == data->m_buyPrice10
        && m_securityCode == data->m_securityCode
        && m_sellVolume6 == data->m_sellVolume6
        && m_sellVolume7 == data->m_sellVolume7
        && m_sellVolume8 == data->m_sellVolume8
        && m_sellVolume9 == data->m_sellVolume9
        && m_sellVolume10 == data->m_sellVolume10
        && m_sellPrice6 == data->m_sellPrice6
        && m_sellPrice7 == data->m_sellPrice7
        && m_sellPrice8 == data->m_sellPrice8
        && m_sellPrice9 == data->m_sellPrice9
        && m_sellPrice10 == data->m_sellPrice10)
        {
            return true;
        }
        else
        {
            return false;
        }
	}
};

class UserSecurityCategory
{
public:
	UserSecurityCategory();
    String m_categoryID;
    int m_userID;
    String m_name;
    String m_codes;
    int m_type;
    int m_orderNum;
};

class SecurityFilterTemplate
{
public:
	SecurityFilterTemplate();
    String m_templateID;
    int m_userID;
    String m_name;
    String m_codes;
    String m_indicator;
    String m_parameters;
	String m_filter;
    int m_cycle;
	int m_subscription;
    int m_orderNum;
};

class LoginInfo
{
public:
	LoginInfo();
	int m_maxUsers;
	String m_nickName;
	String m_passWord;
	int m_sessionID;
	int m_state;
	int m_type;
	int m_userID;
	String m_userName;
};

class HistoryDataInfo
{
public:
	HistoryDataInfo();
	int m_cycle;
	double m_endDate;
	bool m_pushData;
	String m_securityCode;
	int m_size;
	double m_startDate;
	int m_subscription;
	int m_type;
};

class LatestDataInfo
{
public:
	LatestDataInfo();
	String m_codes;
	int m_formatType;
	int m_lv2;
	int m_size;
};

class LatestDataInfoLV2
{
public:
	LatestDataInfoLV2();
	int m_size;
};

class TransactionData
{
public:
	TransactionData();
    double m_date;
    float m_price;
    int m_type;
    double m_volume;
};

class VolumeForecastData
{
public:
	VolumeForecastData();
	String m_securityCode;
	double m_rate;
};

class IndicatorLayout
{
public:
	IndicatorLayout();
	String m_layoutID;
    String m_name;
    int m_orderNum;
	String m_text;
    int m_type;
    int m_userID;
};

class ChatData
{
public:
	ChatData();
	String m_text;
	int m_type;
	int m_userID;
};

class UserSession
{
public:
	UserSession();
	String m_key;
	int m_userID;
	String m_value;
};

class Macro
{
public:
	Macro();
	int m_cd;
    String m_description;
    String m_icon;
	int m_interval;
    String m_macroID;
    String m_name;
    int m_orderNum;
    String m_script;
    int m_type;
    int m_userID;
};

class UserSecurityVisitsCount
{
public:
	UserSecurityVisitsCount();
	virtual ~UserSecurityVisitsCount();
	map<String, int> m_codes;
	int m_userID;
public:
	void CodesFromString(String str);
	String CodesToString();
};
#endif
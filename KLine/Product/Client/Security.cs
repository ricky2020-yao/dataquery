using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    /// <summary>
    /// ָ�����
    /// </summary>
    public class Indicator
    {
        /// <summary>
        /// ���
        /// </summary>
        public String m_category = "";

        /// <summary>
        /// Ԥ����ʾ����
        /// </summary>
        public String m_coordinate = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_description = "";

        /// <summary>
        /// ��ʾС����λ��
        /// </summary>
        public int m_digit;

        /// <summary>
        /// ָ��ID
        /// </summary>
        public String m_indicatorID = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �б�˳��
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// ���߷���
        /// </summary>
        public int m_paintType;

        /// <summary>
        /// ����
        /// </summary>
        public String m_parameters = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_password = "";

        /// <summary>
        /// ����Y������
        /// </summary>
        public String m_specialCoordinate = "";

        /// <summary>
        /// �ı�
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �Ƿ�ʹ������
        /// </summary>
        public int m_usePassword;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// �汾
        /// </summary>
        public int m_version;
    }

    /// <summary>
    /// ��Ʊ��Ϣ
    /// </summary>
    public class Security
    {
        /// <summary>
        /// �������̾���
        /// </summary>
        public Security()
        {
        }

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// ƴ��
        /// </summary>
        public String m_pingyin = "";

        /// <summary>
        /// ״̬
        /// </summary>
        public int m_status;

        /// <summary>
        /// �г�����
        /// </summary>
        public int m_type;
    }

    /// <summary>
    /// ֤ȯ��ʷ����
    /// </summary>
    public class SecurityData
    {
        /// <summary>
        /// ƽ���۸�
        /// </summary>
        public float m_avgPrice;

        /// <summary>
        /// ���̼�
        /// </summary>
        public float m_close;

        /// <summary>
        /// ����
        /// </summary>
        public double m_date;

        /// <summary>
        /// ��߼�
        /// </summary>
        public float m_high;

        /// <summary>
        /// ��ͼ�
        /// </summary>
        public float m_low;

        /// <summary>
        /// ���̼�
        /// </summary>
        public float m_open;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_volume;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_amount;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="data">����</param>
        public void Copy(SecurityData data)
        {
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_low = data.m_low;
            m_open = data.m_open;
            m_volume = data.m_volume;
            m_amount = data.m_amount;
        }
    }

    /// <summary>
    /// ��Ʊʵʱ����
    /// </summary>
    public class SecurityLatestData
    {
        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_amount;

        /// <summary>
        /// ��һ��
        /// </summary>
        public int m_buyVolume1;

        /// <summary>
        /// �����
        /// </summary>
        public int m_buyVolume2;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume3;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume4;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume5;

        /// <summary>
        /// ��һ��
        /// </summary>
        public float m_buyPrice1;

        /// <summary>
        /// �����
        /// </summary>
        public float m_buyPrice2;

        /// <summary>
        /// ������
        /// </summary>
        public float m_buyPrice3;

        /// <summary>
        /// ���ļ�
        /// </summary>
        public float m_buyPrice4;

        /// <summary>
        /// �����
        /// </summary>
        public float m_buyPrice5;

        /// <summary>
        /// ��ǰ�۸�
        /// </summary>
        public float m_close;

        /// <summary>
        /// ���ڼ�ʱ��
        /// </summary>
        public double m_date;

        /// <summary>
        /// ��߼�
        /// </summary>
        public float m_high;

        /// <summary>
        /// ���̳ɽ���
        /// </summary>
        public int m_innerVol;

        /// <summary>
        /// �������̼�
        /// </summary>
        public float m_lastClose;

        /// <summary>
        /// ��ͼ�
        /// </summary>
        public float m_low;

        /// <summary>
        /// ���̼�
        /// </summary>
        public float m_open;

        /// <summary>
        /// �ڻ��ֲ���
        /// </summary>
        public double m_openInterest;

        /// <summary>
        /// ���̳ɽ���
        /// </summary>
        public int m_outerVol;

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_securityCode = "";

        /// <summary>
        /// ��һ��
        /// </summary>
        public int m_sellVolume1;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume2;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume3;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume4;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume5;

        /// <summary>
        /// ��һ��
        /// </summary>
        public float m_sellPrice1;

        /// <summary>
        /// ������
        /// </summary>
        public float m_sellPrice2;

        /// <summary>
        /// ������
        /// </summary>
        public float m_sellPrice3;

        /// <summary>
        /// ���ļ�
        /// </summary>
        public float m_sellPrice4;

        /// <summary>
        /// �����
        /// </summary>
        public float m_sellPrice5;

        /// <summary>
        /// �ڻ������
        /// </summary>
        public float m_settlePrice;

        /// <summary>
        /// ������
        /// </summary>
        public float m_turnoverRate;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_volume;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="data">����</param>
        public void Copy(SecurityLatestData data)
        {
            if (data == null) return;
            m_amount = data.m_amount;
            m_buyVolume1 = data.m_buyVolume1;
            m_buyVolume2 = data.m_buyVolume2;
            m_buyVolume3 = data.m_buyVolume3;
            m_buyVolume4 = data.m_buyVolume4;
            m_buyVolume5 = data.m_buyVolume5;
            m_buyPrice1 = data.m_buyPrice1;
            m_buyPrice2 = data.m_buyPrice2;
            m_buyPrice3 = data.m_buyPrice3;
            m_buyPrice4 = data.m_buyPrice4;
            m_buyPrice5 = data.m_buyPrice5;
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_innerVol = data.m_innerVol;
            m_lastClose = data.m_lastClose;
            m_low = data.m_low;
            m_open = data.m_open;
            m_openInterest = data.m_openInterest;
            m_outerVol = data.m_outerVol;
            m_securityCode = data.m_securityCode;
            m_sellVolume1 = data.m_sellVolume1;
            m_sellVolume2 = data.m_sellVolume2;
            m_sellVolume3 = data.m_sellVolume3;
            m_sellVolume4 = data.m_sellVolume4;
            m_sellVolume5 = data.m_sellVolume5;
            m_sellPrice1 = data.m_sellPrice1;
            m_sellPrice2 = data.m_sellPrice2;
            m_sellPrice3 = data.m_sellPrice3;
            m_sellPrice4 = data.m_sellPrice4;
            m_sellPrice5 = data.m_sellPrice5;
            m_settlePrice = data.m_settlePrice;
            m_turnoverRate = data.m_turnoverRate;
            m_volume = data.m_volume;
        }

        /// <summary>
        /// �Ƚ��Ƿ���ͬ
        /// </summary>
        /// <param name="data">����</param>
        /// <returns>�Ƿ���ͬ</returns>
        public bool Equal(SecurityLatestData data)
        {
            if (data == null) return false;
            if (m_amount == data.m_amount
            && m_buyVolume1 == data.m_buyVolume1
            && m_buyVolume2 == data.m_buyVolume2
            && m_buyVolume3 == data.m_buyVolume3
            && m_buyVolume4 == data.m_buyVolume4
            && m_buyVolume5 == data.m_buyVolume5
            && m_buyPrice1 == data.m_buyPrice1
            && m_buyPrice2 == data.m_buyPrice2
            && m_buyPrice3 == data.m_buyPrice3
            && m_buyPrice4 == data.m_buyPrice4
            && m_buyPrice5 == data.m_buyPrice5
            && m_close == data.m_close
            && m_date == data.m_date
            && m_high == data.m_high
            && m_innerVol == data.m_innerVol
            && m_lastClose == data.m_lastClose
            && m_low == data.m_low
            && m_open == data.m_open
            && m_openInterest == data.m_openInterest
            && m_outerVol == data.m_outerVol
            && m_securityCode == data.m_securityCode
            && m_sellVolume1 == data.m_sellVolume1
            && m_sellVolume2 == data.m_sellVolume2
            && m_sellVolume3 == data.m_sellVolume3
            && m_sellVolume4 == data.m_sellVolume4
            && m_sellVolume5 == data.m_sellVolume5
            && m_sellPrice1 == data.m_sellPrice1
            && m_sellPrice2 == data.m_sellPrice2
            && m_sellPrice3 == data.m_sellPrice3
            && m_sellPrice4 == data.m_sellPrice4
            && m_sellPrice5 == data.m_sellPrice5
            && m_settlePrice == data.m_settlePrice
            && m_turnoverRate == data.m_turnoverRate
            && m_volume == data.m_volume)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// ��Ʊʵʱ����LV2
    /// </summary>
    public class SecurityLatestDataLV2
    {
        #region Lord 2016/6/3
        /// <summary>
        /// ί������
        /// </summary>
        public double m_allBuyVol;

        /// <summary>
        /// ί������
        /// </summary>
        public double m_allSellVol;

        /// <summary>
        /// ��Ȩƽ��ί���۸�
        /// </summary>
        public float m_avgBuyPrice;

        /// <summary>
        /// ��Ȩƽ��ί���۸�
        /// </summary>
        public float m_avgSellPrice;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume6;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume7;

        /// <summary>
        /// �����
        /// </summary>
        public int m_buyVolume8;

        /// <summary>
        /// �����
        /// </summary>
        public int m_buyVolume9;

        /// <summary>
        /// ��ʮ��
        /// </summary>
        public int m_buyVolume10;

        /// <summary>
        /// ������
        /// </summary>
        public float m_buyPrice6;

        /// <summary>
        /// ���߼�
        /// </summary>
        public float m_buyPrice7;

        /// <summary>
        /// ��˼�
        /// </summary>
        public float m_buyPrice8;

        /// <summary>
        /// ��ż�
        /// </summary>
        public float m_buyPrice9;

        /// <summary>
        /// ��ʮ��
        /// </summary>
        public float m_buyPrice10;

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_securityCode;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume6;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume7;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume8;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume9;

        /// <summary>
        /// ��ʮ��
        /// </summary>
        public int m_sellVolume10;

        /// <summary>
        /// ������
        /// </summary>
        public float m_sellPrice6;

        /// <summary>
        /// ���߼�
        /// </summary>
        public float m_sellPrice7;

        /// <summary>
        /// ���˼�
        /// </summary>
        public float m_sellPrice8;

        /// <summary>
        /// ���ż�
        /// </summary>
        public float m_sellPrice9;

        /// <summary>
        /// ��ʮ��
        /// </summary>
        public float m_sellPrice10;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="data">����</param>
        public void Copy(SecurityLatestDataLV2 data)
        {
            if (data == null) return;
            m_allBuyVol = data.m_allBuyVol;
            m_allSellVol = data.m_allSellVol;
            m_avgBuyPrice = data.m_avgBuyPrice;
            m_avgSellPrice = data.m_avgSellPrice;
            m_buyVolume6 = data.m_buyVolume6;
            m_buyVolume7 = data.m_buyVolume7;
            m_buyVolume8 = data.m_buyVolume8;
            m_buyVolume9 = data.m_buyVolume9;
            m_buyVolume10 = data.m_buyVolume10;
            m_buyPrice6 = data.m_buyPrice6;
            m_buyPrice7 = data.m_buyPrice7;
            m_buyPrice8 = data.m_buyPrice8;
            m_buyPrice9 = data.m_buyPrice9;
            m_buyPrice10 = data.m_buyPrice10;
            m_securityCode = data.m_securityCode;
            m_sellVolume6 = data.m_sellVolume6;
            m_sellVolume7 = data.m_sellVolume7;
            m_sellVolume8 = data.m_sellVolume8;
            m_sellVolume9 = data.m_sellVolume9;
            m_sellVolume10 = data.m_sellVolume10;
            m_sellPrice6 = data.m_sellPrice6;
            m_sellPrice7 = data.m_sellPrice7;
            m_sellPrice8 = data.m_sellPrice8;
            m_sellPrice9 = data.m_sellPrice9;
            m_sellPrice10 = data.m_sellPrice10;
        }

        /// <summary>
        /// �Ƚ��Ƿ���ͬ
        /// </summary>
        /// <param name="data">����</param>
        /// <returns>�Ƿ���ͬ</returns>
        public bool Equal(SecurityLatestDataLV2 data)
        {
            if (data == null) return false;
            if (m_allBuyVol == data.m_allBuyVol
            && m_allSellVol == data.m_allSellVol
            && m_avgBuyPrice == data.m_avgBuyPrice
            && m_avgSellPrice == data.m_avgSellPrice
            && m_buyVolume6 == data.m_buyVolume6
            && m_buyVolume7 == data.m_buyVolume7
            && m_buyVolume8 == data.m_buyVolume8
            && m_buyVolume9 == data.m_buyVolume9
            && m_buyVolume10 == data.m_buyVolume10
            && m_buyPrice6 == data.m_buyPrice6
            && m_buyPrice7 == data.m_buyPrice7
            && m_buyPrice8 == data.m_buyPrice8
            && m_buyPrice9 == data.m_buyPrice9
            && m_buyPrice10 == data.m_buyPrice10
            && m_securityCode == data.m_securityCode
            && m_sellVolume6 == data.m_sellVolume6
            && m_sellVolume7 == data.m_sellVolume7
            && m_sellVolume8 == data.m_sellVolume8
            && m_sellVolume9 == data.m_sellVolume9
            && m_sellVolume10 == data.m_sellVolume10
            && m_sellPrice6 == data.m_sellPrice6
            && m_sellPrice7 == data.m_sellPrice7
            && m_sellPrice8 == data.m_sellPrice8
            && m_sellPrice9 == data.m_sellPrice9
            && m_sellPrice10 == data.m_sellPrice10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// �����ֶ�
    /// </summary>
    public class KeyFields
    {
        #region Lord 2016/10/03
        /// <summary>
        /// ���̼�
        /// </summary>
        public const String CLOSE = "CLOSE";
        /// <summary>
        /// ��߼�
        /// </summary>
        public const String HIGH = "HIGH";
        /// <summary>
        /// ��ͼ�
        /// </summary>
        public const String LOW = "LOW";
        /// <summary>
        /// ���̼�
        /// </summary>
        public const String OPEN = "OPEN";
        /// <summary>
        /// �ɽ���
        /// </summary>
        public const String VOL = "VOL";
        /// <summary>
        /// �ɽ���
        /// </summary>
        public const String AMOUNT = "AMOUNT";

        /// <summary>
        /// ƽ���۸�
        /// </summary>
        public const String AVGPRICE = "AVGPRICE";

        /// <summary>
        /// ���̼��ֶ�
        /// </summary>
        public const int CLOSE_INDEX = 0;
        /// <summary>
        /// ��߼��ֶ�
        /// </summary>
        public const int HIGH_INDEX = 1;
        /// <summary>
        /// ��ͼ��ֶ�
        /// </summary>
        public const int LOW_INDEX = 2;
        /// <summary>
        /// ���̼��ֶ�
        /// </summary>
        public const int OPEN_INDEX = 3;
        /// <summary>
        /// �ɽ����ֶ�
        /// </summary>
        public const int VOL_INDEX = 4;
        /// <summary>
        /// �ɽ����ֶ�
        /// </summary>
        public const int AMOUNT_INDEX = 5;

        /// <summary>
        /// ƽ���۸��ֶ�
        /// </summary>
        public const int AVGPRICE_INDEX = 6;
        #endregion
    }

    /// <summary>
    /// ��ѡ�����
    /// </summary>
    public class UserSecurityCategory
    {
        #region Lord 2016/12/22
        /// <summary>
        /// ���ID
        /// </summary>
        public String m_categoryID = "";

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_codes = "";

        /// <summary>
        /// �������
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �б�˳��
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// ���
        /// </summary>
        public int m_type;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;
        #endregion
    }

    /// <summary>
    /// ����ѡ��ģ��
    /// </summary>
    public class SecurityFilterTemplate
    {
        #region Lord 2016/12/22
        /// <summary>
        /// ��ѡ�ɴ���
        /// </summary>
        public String m_codes = "";

        /// <summary>
        /// ��������
        /// </summary>
        public String m_filter = "";

        /// <summary>
        /// ָ��
        /// </summary>
        public String m_indicator = "";

        /// <summary>
        /// ģ��ID
        /// </summary>
        public String m_templateID = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_cycle;

        /// <summary>
        /// ��Ȩģʽ
        /// </summary>
        public int m_subscription;

        /// <summary>
        /// ģ������
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �б�˳��
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// ����
        /// </summary>
        public String m_parameters = "";

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;
        #endregion
    }

    /// <summary>
    /// ��¼��Ϣ
    /// </summary>
    public class LoginInfo
    {
        #region Lord 2016/3/23
        /// <summary>
        /// ����û���
        /// </summary>
        public int m_maxUsers;

        /// <summary>
        /// �ǳ�
        /// </summary>
        public String m_nickName = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_passWord = "";

        /// <summary>
        /// �ỰID
        /// </summary>
        public int m_sessionID;

        /// <summary>
        /// ״̬
        /// </summary>
        public int m_state;

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// �û�����
        /// </summary>
        public String m_userName = "";
        #endregion
    }

    /// <summary>
    /// ��ʷ������Ϣ
    /// </summary>
    public class HistoryDataInfo
    {
        #region Lord 2016/3/27
        /// <summary>
        /// ����
        /// </summary>
        public int m_cycle;

        /// <summary>
        /// ��������
        /// </summary>
        public double m_endDate;

        /// <summary>
        /// �Ƿ���Ҫ��������
        /// </summary>
        public bool m_pushData;

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_securityCode;

        /// <summary>
        /// ��������
        /// </summary>
        public int m_size;

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public double m_startDate;

        /// <summary>
        /// ��Ȩģʽ
        /// </summary>
        public int m_subscription;

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;
        #endregion
    }

    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public class LatestDataInfo
    {
        #region Lord 2016/5/18
        /// <summary>
        /// ����
        /// </summary>
        public String m_codes;

        /// <summary>
        /// ��ʽ
        /// </summary>
        public int m_formatType;

        /// <summary>
        /// �Ƿ����LV2
        /// </summary>
        public int m_lv2;

        /// <summary>
        /// ��������
        /// </summary>
        public int m_size;
        #endregion
    }

    /// <summary>
    /// ����LV2������Ϣ
    /// </summary>
    public class LatestDataInfoLV2
    {
        #region Lord 2016/6/3
        /// <summary>
        /// ��������
        /// </summary>
        public int m_size;
        #endregion
    }

    /// <summary>
    /// �ɽ�����
    /// </summary>
    public class TransactionData
    {
        #region Lord 2016/5/11
        /// <summary>
        /// ����
        /// </summary>
        public double m_date;

        /// <summary>
        /// �۸�
        /// </summary>
        public float m_price;

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_volume;
        #endregion
    }

    /// <summary>
    /// �ɽ���Ԥ������
    /// </summary>
    public class VolumeForecastData
    {
        #region Lord 2016/5/12
        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_securityCode;

        /// <summary>
        /// �ɽ���ռ��
        /// </summary>
        public double m_rate;
        #endregion
    }

    /// <summary>
    /// ָ�겼��
    /// </summary>
    public class IndicatorLayout
    {
        #region Lord 2016/5/22
        /// <summary>
        /// ����ID
        /// </summary>
        public String m_layoutID = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �б�˳��
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// �ĵ�
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;
        #endregion
    }

    /// <summary>
    /// ��������
    /// </summary>
    public class ChatData
    {
        #region �봺�� 2016/6/9
        /// <summary>
        /// ����
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;
        #endregion
    }

    /// <summary>
    /// �û��Ự��Ϣ
    /// </summary>
    public class UserSession
    {
        #region ���� 2016/6/9
        /// <summary>
        /// �Ự��
        /// </summary>
        public String m_key = "";

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// �Ựֵ
        /// </summary>
        public String m_value = "";
        #endregion
    }

    /// <summary>
    /// ��
    /// </summary>
    public class Macro
    {
        #region Lord 2015/10/1
        /// <summary>
        /// ��ȴʱ��
        /// </summary>
        public int m_cd;

        /// <summary>
        /// ����
        /// </summary>
        public String m_description = "";

        /// <summary>
        /// ͼ��
        /// </summary>
        public String m_icon = "";

        /// <summary>
        /// ���ݳ���
        /// </summary>
        public int m_interval = 1;

        /// <summary>
        /// ��ID
        /// </summary>
        public String m_macroID = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �����ֶ�
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// �ű�
        /// </summary>
        public String m_script = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;
        #endregion
    }

    /// <summary>
    /// ���ʴ���
    /// </summary>
    public class UserSecurityVisitsCount
    {
        #region Lord 2016/04/06
        /// <summary>
        /// ���������
        /// </summary>
        public Dictionary<String, int> m_codes = new Dictionary<String, int>();

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// ���ַ���ת��Ϊ����
        /// </summary>
        /// <param name="str">�ַ���</param>
        public void CodesFromString(String str)
        {
            m_codes.Clear();
            String[] strs = str.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            int strsSize = strs.Length;
            for (int i = 0; i < strsSize; i++)
            {
                String[] subStrs = strs[i].Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                m_codes[subStrs[0]] = CStrA.ConvertStrToInt(subStrs[1]);
            }
        }

        /// <summary>
        /// ת��Ϊ�ַ���
        /// </summary>
        /// <returns>�ַ���</returns>
        public String CodesToString()
        {
            String str = "";
            foreach (String code in m_codes.Keys)
            {
                str += code + ":" + m_codes[code].ToString() + ";";
            }
            return str;
        }
        #endregion
    }
}

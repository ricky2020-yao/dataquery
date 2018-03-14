using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;

namespace KLine
{
    /// <summary>
    /// ��ƱȨֵ����Ϣ
    /// </summary>
    public class SecurityOrder
    {
        /// <summary>
        /// ����Ȩֵ��Ϣ
        /// </summary>
        public SecurityOrder()
        {
        }

        /// <summary>
        /// Ȩֵ
        /// </summary>
        public int m_order;

        /// <summary>
        /// ���̾���
        /// </summary>
        public Security m_security;
    }

    /// <summary>
    /// ֤ȯ������չ
    /// </summary>
    public class SecurityServiceEx : SecurityService
    {
        /// <summary>
        /// ����֤ȯ������չ
        /// </summary>
        public SecurityServiceEx()
        {

        }

        /// <summary>
        /// ֤ȯ�����б�
        /// </summary>
        private CList<CList<Security>> m_securities = new CList<CList<Security>>();

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        private List<Security> m_securitiesCache = new List<Security>();

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        private Dictionary<String, Security> m_securitiesMap = new Dictionary<String, Security>();

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ�б������ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// ��ȡ�������Ƿ��Ѽ���
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        /// <summary>
        /// ���������ƥ���Ʊ����
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <param name="makeupCode">�����û�����</param>
        /// <returns>�Ƿ�ƥ��</returns>
        private bool CompareMakeupSecutitycode(String code, String makeupCode)
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
            if (code.Length > makeupCode.Length)
            {
                if (code.Substring(makeupCode.Length, 1) == ".")
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_securitiesMap.Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// ��ѯ���̾���
        /// </summary>
        /// <param name="key">����ֵ</param>
        /// <param name="limitSize">���ֵ</param>
        /// <param name="securities">���ؼ��̾��鼯��</param>
        /// <returns>Ԥ��</returns>
        public int Find(String key, int limitSize, CList<Security> securities)
        {
            int order = 0, order1 = 0, order2 = 0, order3 = 0, minOrder = 0;
            int securityListSize = m_securities.size();
            CList<SecurityOrder> securityReturnList = new CList<SecurityOrder>();
            securityReturnList.set_capacity(limitSize);
            //����
            for (int i = 0; i < securityListSize; i++)
            {
                CList<Security> securityList = m_securities.get(i);
                int securitySize = securityList.size();
                for (int j = 0; j < securitySize; j++)
                {
                    Security security = securityList.get(j);
                    order1 = IsMatching(security.m_code, security.m_type, key, 0, i);
                    order2 = IsMatching(security.m_name, security.m_type, key, 1, i);
                    order3 = IsMatching(security.m_pingyin, security.m_type, key, 2, i);
                    order = Math.Max(order1, Math.Max(order2, order3));
                    if (order > 0)
                    {
                        SecurityOrder securityReturn = new SecurityOrder();
                        securityReturn.m_order = order;
                        securityReturn.m_security = security;
                        if (securityReturnList.size() < limitSize)
                        {
                            securityReturnList.push_back(securityReturn);
                        }
                        else
                        {
                            int index = 0;
                            minOrder = securityReturnList.get(0).m_order;
                            for (int m = 0; m < securityReturnList.size(); m++)
                            {
                                if (securityReturnList.get(m).m_order < minOrder)
                                {
                                    minOrder = securityReturnList.get(m).m_order;
                                    index = m;
                                }
                            }
                            if (minOrder < order)
                            {
                                securityReturnList.get(index).m_order = order;
                                securityReturnList.get(index).m_security = security;
                            }
                        }
                    }
                }
            }
            //����
            int retListSize = securityReturnList.size();
            SecurityOrder security1 = null;
            SecurityOrder security2 = null;
            for (int i = 0; i < retListSize - 1; i++)
            {
                for (int j = i + 1; j < retListSize; j++)
                {
                    security1 = securityReturnList.get(i);
                    security2 = securityReturnList.get(j);
                    if (security1.m_order < security2.m_order
                        || (security1.m_order == security2.m_order && String.Compare(security1.m_security.m_code, security2.m_security.m_code) > 0))
                    {
                        securityReturnList.set(i, security2);
                        securityReturnList.set(j, security1);
                    }
                }
            }
            //����
            for (int i = 0; i < retListSize; i++)
            {
                securities.push_back(securityReturnList.get(i).m_security);
            }
            securityReturnList.clear();
            return securities.size();
        }

        /// <summary>
        /// ����֤ȯ����
        /// </summary>
        /// <param name="key">�ؼ���</param>
        /// <param name="securities">֤ȯ�����б�</param>
        /// <returns>״̬</returns>
        public bool FilterSecurities(String key, CList<Security> securities)
        {
            if (!m_loaded)
            {
                return false;
            }
            Find(key, 10, securities);
            return true;
        }

        /// <summary>
        /// ���ݴ����ȡ֤ȯ
        /// </summary>
        /// <param name="code">֤ȯ����</param>
        /// <param name="security">֤ȯ��Ϣ</param>
        /// <returns>�Ƿ��ȡ�ɹ�</returns>
        public bool GetSecurityByCode(String code, ref Security security)
        {
            if (!m_loaded)
            {
                return false;
            }
            String key = code.ToUpper();
            key = key.Replace("\r", "");
            key = key.Replace("\n", "");
            if (m_securitiesMap.ContainsKey(code))
            {
                security = m_securitiesMap[code];
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ���ݴ����ȡ֤ȯ�б�
        /// </summary>
        /// <param name="codes">����</param>
        /// <param name="securities">֤ȯ�б�</param>
        /// <returns>�Ƿ��ȡ�ɹ�</returns>
        public bool GetSecuritiesByCodes(String codes, List<Security> securities)
        {
            if (!m_loaded)
            {
                return false;
            }
            String[] strCodes = codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int strCodesSize = strCodes.Length;
            for (int i = 0; i < strCodesSize; i++)
            {
                String code = strCodes[i].ToUpper();
                if (m_securitiesMap.ContainsKey(code))
                {
                    securities.Add(m_securitiesMap[code]);
                }
            }
            return true;
        }

        /// <summary>
        /// ����֤ȯ
        /// </summary>
        private void Load()
        {
            //Ԥ�����ڴ�
            m_securities.set_capacity(200);
            for (int i = 0; i < 200; i++)
            {
                CList<Security> list = new CList<Security>();
                m_securities.push_back(list);
            }
            int securitiesSize = m_securitiesCache.Count;
            int[] capacities = new int[1000];
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = m_securitiesCache[i];
                capacities[security.m_type] = capacities[security.m_type] + 1;
            }
            for (int i = 0; i < 200; i++)
            {
                CList<Security> list = m_securities.get(i);
                list.set_capacity(capacities[i]);
            }
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = m_securitiesCache[i];
                m_securities.get(security.m_type).push_back(security);
                m_securitiesMap[security.m_code.ToUpper()] = security;
            }
            m_securitiesCache.Clear();
            m_loaded = true;
            capacities = null;
            System.GC.Collect();
        }

        /// <summary>
        /// ����֤ȯ���ݵ�����
        /// </summary>
        private void LoadSecuritiesToCache()
        {
            Load();
        }

        /// <summary>
        /// ����֤ȯ����
        /// </summary>
        public void LoadSecurities()
        {
            GetSecurities(m_getListRequestID);
        }

        /// <summary>
        /// ���̾��鲹��ƥ��
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <param name="input">����</param>
        /// <param name="type">��Ʊ����</param>
        /// <returns>�Ƿ�ƥ��</returns>
        private bool MakeupSecurity(String code, String input, int type)
        {
            String zore = "", compare = "";
            bool ret = false;
            int len = input.Length;
            if (code == "" || input == "" || len > 5)
            {
                return ret;
            }
            int zorenum = 5 - len;
            for (int i = 0; i < zorenum; i++)
            {
                zore += "0";
            }
            //��1λ��6��ƥ����֤A��
            if (type == 1)
            {
                compare += "6";
                compare += zore;
                compare += input;
                ret = CompareMakeupSecutitycode(code, compare);
            }
            if (ret)
            {
                return ret;
            }
            compare = "";
            //��1λ��3��ƥ����֤A��,��ҵ��
            if (type == 2)
            {
                compare += "3";
                compare += zore;
                compare += input;
                ret = CompareMakeupSecutitycode(code, compare);
            }
            if (ret)
            {
                return ret;
            }
            compare = "";
            //ƥ��۹�
            if (len <= 4 && (type == 71 || type == 72))
            {
                compare += zore;
                compare += input;
                ret = CompareMakeupSecutitycode(code, compare);
            }
            if (ret)
            {
                return ret;
            }
            compare = "";
            //ƥ����֤A�ɣ���С��
            if (type == 2 && len <= 3)
            {
                compare += "002";
                zorenum = 6 - len - 3;
                for (int i = 0; i < zorenum; i++)
                {
                    compare += "0";
                }
                compare += input;
                ret = CompareMakeupSecutitycode(code, compare);
            }
            return ret;
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            if (message.m_functionID == FUNCTIONID_SECURITY_GETSECURITIES)
            {
                SecurityService.GetSecurities(m_securitiesCache, message.m_functionID, message.m_body, message.m_bodyLength);
                Thread hThread = new Thread(new ThreadStart(LoadSecuritiesToCache));
                hThread.IsBackground = true;
                hThread.Start();
            }
            base.OnReceive(message);
        }

        /// <summary>
        /// ���̾�������
        /// </summary>
        /// <param name="securityVal">���̾����Ա����</param>
        /// <param name="input">�û�����</param>
        /// <param name="type">���롢���ơ�ƴ��</param>
        /// <returns>����</returns>
        private int IsMatching(String securityVal, int securityType, String input, int type, int orderIndex)
        {
            int len1 = securityVal.Length, len2 = input.Length;
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
                //�Ƚϴ���ƥ��������п��ܳ��ֲ���ƥ������
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
                String rest = securityVal.Substring(index);
                if (String.Compare(rest, input) == 0)
                {
                    //�����ȫƥ��
                    ret += 1000;
                }
                if (type == 0)
                {
                    if (rest.Length > len2)
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

        /// <summary>
        /// �Ƚ��ַ���
        /// </summary>
        /// <param name="str1">�ַ���1</param>
        /// <param name="str2">�ַ���2</param>
        /// <returns>����</returns>
        public int StrStr(String str1, String str2)
        {
            int result = -1, i = 0, j = 0, len1 = str1.Length, len2 = str2.Length;
            if (len1 >= len2)
            {
                while (i <= len2 - 1)
                {
                    if (str1[i] == str2[j])
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
    }
}

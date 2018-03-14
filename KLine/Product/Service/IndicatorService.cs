using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    public class IndicatorService : BaseService
    {
        public const int FUNCTIONID_INDICATOR_ADDINDICATORS = 0;
        public const int FUNCTIONID_INDICATOR_DELETEINDICATORS = 1;
        public const int FUNCTIONID_INDICATOR_GETINDICATORS = 2;
        public const int FUNCTIONID_INDICATOR_UPDATEINDICATORS = 3;
        public const int FUNCTIONID_INDICATOR_UPDATEORDERNUM = 4;
        private int m_socketID = 0;
        public const int SERVICEID_INDICATOR = 4;

        public IndicatorService()
        {
            base.ServiceID = SERVICEID_INDICATOR;
        }

        public int SocketID
        {
            get
            {
                return this.m_socketID;
            }
            set
            {
                this.m_socketID = value;
            }
        }

        public int AddIndicator(int requestID, Indicator indicator)
        {
            List<Indicator> indicators = new List<Indicator>();
            indicators.Add(indicator);
            int num = this.Send(0, requestID, indicators);
            indicators.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int DeleteIndicator(int requestID, Indicator indicator)
        {
            List<Indicator> indicators = new List<Indicator>();
            indicators.Add(indicator);
            int num = this.Send(1, requestID, indicators);
            indicators.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int GetIndicators(int requestID, Indicator indicator)
        {
            List<Indicator> indicators = new List<Indicator>();
            indicators.Add(indicator);
            int num = this.Send(2, requestID, indicators);
            indicators.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetIndicators(List<Indicator> indicators, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    Indicator item = new Indicator();
                    item.m_indicatorID = binary.ReadString();
                    item.m_userID = binary.ReadInt();
                    item.m_category = binary.ReadString();
                    item.m_coordinate = binary.ReadString();
                    item.m_description = binary.ReadString();
                    item.m_digit = binary.ReadInt();
                    item.m_name = binary.ReadString();
                    item.m_orderNum = binary.ReadInt();
                    item.m_paintType = binary.ReadInt();
                    item.m_parameters = binary.ReadString();
                    item.m_password = binary.ReadString();
                    item.m_specialCoordinate = binary.ReadString();
                    item.m_text = binary.ReadString();
                    item.m_type = binary.ReadInt();
                    item.m_usePassword = binary.ReadInt();
                    item.m_version = binary.ReadInt();
                    indicators.Add(item);
                }
            }
            binary.Close();
            return 1;
        }

        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            this.SendToListener(message);
        }

        public int Send(int functionID, int requestID, List<Indicator> indicators)
        {
            Binary binary = new Binary();
            int count = indicators.Count;
            binary.WriteInt(count);
            for (int i = 0; i < count; i++)
            {
                Indicator indicator = indicators[i];
                binary.WriteString(indicator.m_indicatorID);
                binary.WriteInt(indicator.m_userID);
                binary.WriteString(indicator.m_category);
                binary.WriteString(indicator.m_coordinate);
                binary.WriteString(indicator.m_description);
                binary.WriteInt(indicator.m_digit);
                binary.WriteString(indicator.m_name);
                binary.WriteInt(indicator.m_orderNum);
                binary.WriteInt(indicator.m_paintType);
                binary.WriteString(indicator.m_parameters);
                binary.WriteString(indicator.m_password);
                binary.WriteString(indicator.m_specialCoordinate);
                binary.WriteString(indicator.m_text);
                binary.WriteInt(indicator.m_type);
                binary.WriteInt(indicator.m_usePassword);
                binary.WriteInt(indicator.m_version);
            }
            byte[] bytes = binary.GetBytes();
            int num3 = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num3;
        }

        public int UpdateIndicator(int requestID, Indicator indicator)
        {
            List<Indicator> indicators = new List<Indicator>();
            indicators.Add(indicator);
            int num = this.Send(3, requestID, indicators);
            indicators.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int UpdateOrderNum(int requestID, int userID, List<string> ids)
        {
            string str = "";
            int count = ids.Count;
            for (int i = 0; i < count; i++)
            {
                str = str + ids[i];
                if (i != (count - 1))
                {
                    str = str + ",";
                }
            }
            Indicator indicator = new Indicator();
            indicator.m_indicatorID = str;
            indicator.m_userID = userID;
            List<Indicator> indicators = new List<Indicator>();
            indicators.Add(indicator);
            int num3 = this.Send(4, requestID, indicators);
            indicators.Clear();
            return ((num3 > 0) ? 1 : 0);
        }
    }
}

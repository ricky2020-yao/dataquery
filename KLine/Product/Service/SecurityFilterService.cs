using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    public class SecurityFilterService : BaseService
    {
        public const int FUNCTIONID_SECURITYFILTER_ADDTEMPLATES = 0;
        public const int FUNCTIONID_SECURITYFILTER_DELETETEMPLATES = 1;
        public const int FUNCTIONID_SECURITYFILTER_GETTEMPLATES = 2;
        public const int FUNCTIONID_SECURITYFILTER_STARTFILTER = 4;
        public const int FUNCTIONID_SECURITYFILTER_STOPFILTER = 5;
        public const int FUNCTIONID_SECURITYFILTER_UPDATETEMPLATES = 3;
        public const int FUNCTIONID_SECURITYFILTER_UPDTAEORDERNUM = 6;
        private int m_socketID = 0;
        public const int SERVICEID_SECURITYFILTER = 2;

        public SecurityFilterService()
        {
            base.ServiceID = SERVICEID_SECURITYFILTER;
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

        public int AddTemplate(int requestID, SecurityFilterTemplate template)
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>(); 
            templates.Add(template);
            int num = this.Send(0, requestID, templates);
            templates.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int DeleteTemplate(int requestID, SecurityFilterTemplate template)
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            templates.Add(template);
            int num = this.Send(1, requestID, templates);
            templates.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetFilterResults(Dictionary<string, double[]> datas, ref int currentBatch, ref int totalBatch, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            currentBatch = binary.ReadInt();
            totalBatch = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    string str = binary.ReadString();
                    int num3 = binary.ReadInt();
                    double[] numArray = new double[num3];
                    for (int j = 0; j < num3; j++)
                    {
                        numArray[j] = binary.ReadDouble();
                    }
                    datas[str] = numArray;
                }
            }
            binary.Close();
            return 1;
        }

        public int GetTemplates(int requestID, SecurityFilterTemplate template)
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            templates.Add(template);
            int num = this.Send(2, requestID, templates);
            templates.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetTemplates(List<SecurityFilterTemplate> templates, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    SecurityFilterTemplate item = new SecurityFilterTemplate();
                    item.m_templateID = binary.ReadString();
                    item.m_userID = binary.ReadInt();
                    item.m_name = binary.ReadString();
                    item.m_codes = binary.ReadString();
                    item.m_indicator = binary.ReadString();
                    item.m_parameters = binary.ReadString();
                    item.m_filter = binary.ReadString();
                    item.m_cycle = binary.ReadInt();
                    item.m_subscription = binary.ReadInt();
                    item.m_orderNum = binary.ReadInt();
                    templates.Add(item);
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

        public int Send(int functionID, int requestID, List<SecurityFilterTemplate> templates)
        {
            Binary binary = new Binary();
            int count = templates.Count;
            binary.WriteInt(count);
            for (int i = 0; i < count; i++)
            {
                SecurityFilterTemplate template = templates[i];
                binary.WriteString(template.m_templateID);
                binary.WriteInt(template.m_userID);
                binary.WriteString(template.m_name);
                binary.WriteString(template.m_codes);
                binary.WriteString(template.m_indicator);
                binary.WriteString(template.m_parameters);
                binary.WriteString(template.m_filter);
                binary.WriteInt(template.m_cycle);
                binary.WriteInt(template.m_subscription);
                binary.WriteInt(template.m_orderNum);
            }
            byte[] bytes = binary.GetBytes();
            int num3 = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num3;
        }

        public int StartFilter(int requestID, SecurityFilterTemplate template)
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            templates.Add(template);
            int num = this.Send(4, requestID, templates);
            templates.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int StopFilter(int requestID, SecurityFilterTemplate template)
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            templates.Add(template);
            int num = this.Send(5, requestID, templates);
            templates.Clear();
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
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            template.m_templateID = str;
            template.m_userID = userID;
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>(); 
            templates.Add(template);
            int num3 = this.Send(6, requestID, templates);
            templates.Clear();
            return ((num3 > 0) ? 1 : 0);
        }

        public int UpdateTemplate(int requestID, SecurityFilterTemplate template)
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            templates.Add(template);
            int num = this.Send(3, requestID, templates);
            templates.Clear();
            return ((num > 0) ? 1 : 0);
        }
    }
}

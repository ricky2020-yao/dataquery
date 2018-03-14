using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    public class SecurityService : BaseService
    {
        public const int FUNCTIONID_SECURITY_GETSECURITIES = 0;
        public const int FUNCTIONID_SECURITY_GETSECURITIESBYID = 1;
        private int m_socketID = 0;
        public const int SERVICEID_SECURITY = 3;

        public SecurityService()
        {
            base.ServiceID = SERVICEID_SECURITY;
        }

        public int GetSecurities(int requestID)
        {
            Binary binary = new Binary();
            binary.WriteString("lord");
            byte[] bytes = binary.GetBytes();
            int num = this.Send(new CMessage(base.GroupID, base.ServiceID, 0, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetSecurities(List<Security> securities, int functionID, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    Security item = new Security(); 
                    item.m_code = binary.ReadString();
                    
                    if (functionID == 0)
                    {
                        item.m_name = binary.ReadString();
                        item.m_pingyin = binary.ReadString();
                        item.m_type = binary.ReadShort();
                        item.m_status = binary.ReadChar();
                    }
                    securities.Add(item);
                }
            }
            binary.Close();
            return 1;
        }

        public int GetSecuritiesByBlockID(int requestID, string blockID)
        {
            Binary binary = new Binary();
            binary.WriteString(blockID);
            byte[] bytes = binary.GetBytes();
            int num = this.Send(new CMessage(base.GroupID, base.ServiceID, 1, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return ((num > 0) ? 1 : 0);
        }

        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            this.SendToListener(message);
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
    }
}

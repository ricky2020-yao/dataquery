using System;
using System.Collections.Generic;

namespace KLine
{
    public class UserSecurityService : BaseService
    {
        public const int FUNCTIONID_USERSECURITY_ADDCATEGORIES = 0;
        public const int FUNCTIONID_USERSECURITY_ADDSECURITIES = 4;
        public const int FUNCTIONID_USERSECURITY_ADDVISITSCOUNT = 8;
        public const int FUNCTIONID_USERSECURITY_DELETECATEGORIES = 1;
        public const int FUNCTIONID_USERSECURITY_DELETESECURITIES = 5;
        public const int FUNCTIONID_USERSECURITY_GETCATEGORIES = 2;
        public const int FUNCTIONID_USERSECURITY_GETVISITSCOUNT = 7;
        public const int FUNCTIONID_USERSECURITY_REMOVEVISITSCOUNT = 9;
        public const int FUNCTIONID_USERSECURITY_UPDATECATEGORIES = 3;
        public const int FUNCTIONID_USERSECURITY_UPDTAEORDERNUM = 6;
        private int m_socketID = 0;
        public const int SERVICEID_USERSECURITY = 1;

        public UserSecurityService()
        {
            base.ServiceID = 1;
        }

        public int AddCategory(int requestID, UserSecurityCategory category)
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num = this.Send(0, requestID, categories);
            categories.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int AddSecurities(int requestID, UserSecurityCategory category)
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num = this.Send(4, requestID, categories);
            categories.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int AddVisitsCount(int requestID, UserSecurityVisitsCount visitsCount)
        {
            return ((this.SendVisitsCount(8, requestID, visitsCount) > 0) ? 1 : 0);
        }

        public int DeleteCategory(int requestID, UserSecurityCategory category)
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num = this.Send(1, requestID, categories);
            categories.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int DeleteSecurities(int requestID, UserSecurityCategory category)
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num = this.Send(5, requestID, categories);
            categories.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int GetCategories(int requestID, UserSecurityCategory category)
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num = this.Send(2, requestID, categories);
            categories.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetCategories(List<UserSecurityCategory> categories, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    UserSecurityCategory item = new UserSecurityCategory();
                    item.m_categoryID = binary.ReadString();
                    item.m_userID = binary.ReadInt();
                    item.m_name = binary.ReadString();
                    item.m_codes = binary.ReadString();
                    item.m_type = binary.ReadInt();
                    item.m_orderNum = binary.ReadInt();
                    categories.Add(item);
                }
            }
            binary.Close();
            return 1;
        }

        public int GetVisitsCount(int requestID, UserSecurityVisitsCount visitsCount)
        {
            return ((this.SendVisitsCount(7, requestID, visitsCount) > 0) ? 1 : 0);
        }

        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            this.SendToListener(message);
        }

        public int RemoveVisitsCount(int requestID, UserSecurityVisitsCount visitsCount)
        {
            return ((this.SendVisitsCount(9, requestID, visitsCount) > 0) ? 1 : 0);
        }

        public int Send(int functionID, int requestID, List<UserSecurityCategory> categories)
        {
            Binary binary = new Binary();
            int count = categories.Count;
            binary.WriteInt(count);
            for (int i = 0; i < count; i++)
            {
                UserSecurityCategory category = categories[i];
                binary.WriteString(category.m_categoryID);
                binary.WriteInt(category.m_userID);
                binary.WriteString(category.m_name);
                binary.WriteString(category.m_codes);
                binary.WriteInt(category.m_type);
                binary.WriteInt(category.m_orderNum);
            }
            byte[] bytes = binary.GetBytes();
            int num3 = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num3;
        }

        public int SendVisitsCount(int functionID, int requestID, UserSecurityVisitsCount visitsCount)
        {
            Binary binary = new Binary();
            binary.WriteInt(visitsCount.m_userID);
            binary.WriteString(visitsCount.CodesToString());
            byte[] bytes = binary.GetBytes();
            int num = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num;
        }

        public int UpdateCategory(int requestID, UserSecurityCategory category)
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num = this.Send(3, requestID, categories);
            categories.Clear();
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
            UserSecurityCategory category = new UserSecurityCategory();
            category.m_categoryID = str;
            category.m_userID = userID;
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            categories.Add(category);
            int num3 = this.Send(6, requestID, categories);
            categories.Clear();
            return ((num3 > 0) ? 1 : 0);
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


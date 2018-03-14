using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    public class IndicatorLayoutService : BaseService
    {
        public const int FUNCTIONID_INDICATORLAYOUT_ADDLAYOUTS = 0;
        public const int FUNCTIONID_INDICATORLAYOUT_DELETELAYOUTS = 1;
        public const int FUNCTIONID_INDICATORLAYOUT_GETLAYOUTS = 2;
        public const int FUNCTIONID_INDICATORLAYOUT_UPDATELAYOUTS = 3;
        public const int FUNCTIONID_INDICATORLAYOUT_UPDATEORDERNUM = 4;
        private int m_socketID = 0;
        public const int SERVICEID_INDICATORLAYOUT = 6;

        public IndicatorLayoutService()
        {
            base.ServiceID = 6;
        }

        public int AddLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int num = this.Send(0, requestID, layouts);
            layouts.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int DeleteLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int num = this.Send(1, requestID, layouts);
            layouts.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public int GetLayouts(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int num = this.Send(2, requestID, layouts);
            layouts.Clear();
            return ((num > 0) ? 1 : 0);
        }

        public static int GetLayouts(List<IndicatorLayout> layouts, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    IndicatorLayout item = new IndicatorLayout();
                    item.m_layoutID = binary.ReadString();
                    item.m_userID = binary.ReadInt();
                    item.m_name = binary.ReadString();
                    item.m_text = binary.ReadString();
                    item.m_type = binary.ReadInt();
                    item.m_orderNum = binary.ReadInt();
                    layouts.Add(item);
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

        public int Send(int functionID, int requestID, List<IndicatorLayout> layouts)
        {
            Binary binary = new Binary();
            int count = layouts.Count;
            binary.WriteInt(count);
            for (int i = 0; i < count; i++)
            {
                IndicatorLayout layout = layouts[i];
                binary.WriteString(layout.m_layoutID);
                binary.WriteInt(layout.m_userID);
                binary.WriteString(layout.m_name);
                binary.WriteString(layout.m_text);
                binary.WriteInt(layout.m_type);
                binary.WriteInt(layout.m_orderNum);
            }
            byte[] bytes = binary.GetBytes();
            int num3 = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num3;
        }

        public int UpdateLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int num = this.Send(3, requestID, layouts);
            layouts.Clear();
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
            IndicatorLayout layout = new IndicatorLayout();
            layout.m_layoutID = str;
            layout.m_userID = userID;
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int num3 = this.Send(4, requestID, layouts);
            layouts.Clear();
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

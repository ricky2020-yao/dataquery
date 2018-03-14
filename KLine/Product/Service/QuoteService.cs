using System;
using System.Collections.Generic;
using System.Text;

namespace KLine
{
    /// <summary>
    /// 行情服务
    /// </summary>
    public class QuoteService : BaseService
    {
        public const int FUNCTIONID_QUOTE_GETHISTORYDATA = 6;
        public const int FUNCTIONID_QUOTE_GETLATESTDATA = 5;
        public const int FUNCTIONID_QUOTE_GETVOLUMEFORECAST = 8;
        public const int FUNCTIONID_QUOTE_PUSHHISTORYDATA = 1;
        public const int FUNCTIONID_QUOTE_PUSHLATESTDATA = 0;
        public const int FUNCTIONID_QUOTE_PUSHLATESTDATALV2 = 9;
        public const int FUNCTIONID_QUOTE_PUSHTRANSACTIONDATA = 7;
        public const int FUNCTIONID_QUOTE_STOPPUSHHISTORYDATA = 3;
        public const int FUNCTIONID_QUOTE_STOPPUSHLATESTDATA = 4;
        private int m_socketID = -1;
        public const int SERVICEID_QUOTE = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        public QuoteService()
        {
            base.ServiceID = 0;
        }
        /// <summary>
        /// 获取或者设置SocketID
        /// </summary>
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

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public int GetHistoryDatas(int requestID, HistoryDataInfo dataInfo)
        {
            return ((this.Send(FUNCTIONID_QUOTE_GETHISTORYDATA, requestID, this.m_socketID, dataInfo) > 0) ? 1 : 0);
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="datas"></param>
        /// <param name="body"></param>
        /// <param name="bodyLength"></param>
        /// <returns></returns>
        public static int GetHistoryDatas(ref HistoryDataInfo dataInfo, List<SecurityData> datas, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            dataInfo.m_securityCode = binary.ReadString();
            dataInfo.m_type = binary.ReadChar();
            dataInfo.m_size = binary.ReadInt();
            dataInfo.m_cycle = binary.ReadInt();
            dataInfo.m_subscription = binary.ReadInt();
            dataInfo.m_startDate = binary.ReadDouble();
            dataInfo.m_endDate = binary.ReadDouble();
            dataInfo.m_pushData = binary.ReadBool();
            int size = dataInfo.m_size;
            for (int i = 0; i < size; i++)
            {
                SecurityData item = new SecurityData();
                item.m_date = binary.ReadDouble();
                item.m_close = binary.ReadFloat();
                item.m_high = binary.ReadFloat();
                item.m_low = binary.ReadFloat();
                item.m_open = binary.ReadFloat();
                item.m_volume = binary.ReadDouble();
                item.m_amount = binary.ReadDouble();
                if (dataInfo.m_cycle == 0)
                {
                    item.m_avgPrice = binary.ReadFloat();
                }
                datas.Add(item);
            }
            binary.Close();
            return 1;
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public int GetLatestDatas(int requestID, LatestDataInfo dataInfo)
        {
            return ((this.Send(FUNCTIONID_QUOTE_GETLATESTDATA, requestID, this.m_socketID, dataInfo) > 0) ? 1 : 0);
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="datas"></param>
        /// <param name="body"></param>
        /// <param name="bodyLength"></param>
        /// <returns></returns>
        public static int GetLatestDatas(ref LatestDataInfo dataInfo, List<SecurityLatestData> datas, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            dataInfo.m_formatType = binary.ReadChar();
            dataInfo.m_lv2 = binary.ReadChar();
            dataInfo.m_size = binary.ReadInt();
            for (int i = 0; i < dataInfo.m_size; i++)
            {
                SecurityLatestData item = new SecurityLatestData();
                item.m_securityCode = binary.ReadString();
                item.m_open = binary.ReadFloat();
                item.m_lastClose = binary.ReadFloat();
                item.m_close = binary.ReadFloat();
                item.m_high = binary.ReadFloat();
                item.m_low = binary.ReadFloat();
                item.m_volume = binary.ReadDouble();
                item.m_amount = binary.ReadDouble();
                if (dataInfo.m_formatType == 0)
                {
                    item.m_buyVolume1 = binary.ReadInt();
                    item.m_buyPrice1 = binary.ReadFloat();
                    item.m_buyVolume2 = binary.ReadInt();
                    item.m_buyPrice2 = binary.ReadFloat();
                    item.m_buyVolume3 = binary.ReadInt();
                    item.m_buyPrice3 = binary.ReadFloat();
                    item.m_buyVolume4 = binary.ReadInt();
                    item.m_buyPrice4 = binary.ReadFloat();
                    item.m_buyVolume5 = binary.ReadInt();
                    item.m_buyPrice5 = binary.ReadFloat();
                    item.m_sellVolume1 = binary.ReadInt();
                    item.m_sellPrice1 = binary.ReadFloat();
                    item.m_sellVolume2 = binary.ReadInt();
                    item.m_sellPrice2 = binary.ReadFloat();
                    item.m_sellVolume3 = binary.ReadInt();
                    item.m_sellPrice3 = binary.ReadFloat();
                    item.m_sellVolume4 = binary.ReadInt();
                    item.m_sellPrice4 = binary.ReadFloat();
                    item.m_sellVolume5 = binary.ReadInt();
                    item.m_sellPrice5 = binary.ReadFloat();
                    item.m_innerVol = binary.ReadInt();
                    item.m_outerVol = binary.ReadInt();
                    item.m_turnoverRate = binary.ReadFloat();
                    item.m_openInterest = binary.ReadDouble();
                    item.m_settlePrice = binary.ReadFloat();
                }
                item.m_date = binary.ReadDouble();
                datas.Add(item);
            }
            binary.Close();
            return 1;
        }

        /// <summary>
        /// 获取实时数据L2
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="datas"></param>
        /// <param name="body"></param>
        /// <param name="bodyLength"></param>
        /// <returns></returns>
        public static int GetLatestDatasLV2(ref LatestDataInfoLV2 dataInfo, List<SecurityLatestDataLV2> datas, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            dataInfo.m_size = binary.ReadInt();
            for (int i = 0; i < dataInfo.m_size; i++)
            {
                SecurityLatestDataLV2 item = new SecurityLatestDataLV2();
                item.m_securityCode = binary.ReadString();
                item.m_allBuyVol = binary.ReadDouble();
                item.m_avgBuyPrice = binary.ReadFloat();
                item.m_allSellVol = binary.ReadDouble();
                item.m_avgSellPrice = binary.ReadFloat();
                item.m_buyVolume6 = binary.ReadInt();
                item.m_buyPrice6 = binary.ReadFloat();
                item.m_buyVolume7 = binary.ReadInt();
                item.m_buyPrice7 = binary.ReadFloat();
                item.m_buyVolume8 = binary.ReadInt();
                item.m_buyPrice8 = binary.ReadFloat();
                item.m_buyVolume9 = binary.ReadInt();
                item.m_buyPrice9 = binary.ReadFloat();
                item.m_buyVolume10 = binary.ReadInt();
                item.m_buyPrice10 = binary.ReadFloat();
                item.m_sellVolume6 = binary.ReadInt();
                item.m_sellPrice6 = binary.ReadFloat();
                item.m_sellVolume7 = binary.ReadInt();
                item.m_sellPrice7 = binary.ReadFloat();
                item.m_sellVolume8 = binary.ReadInt();
                item.m_sellPrice8 = binary.ReadFloat();
                item.m_sellVolume9 = binary.ReadInt();
                item.m_sellPrice9 = binary.ReadFloat();
                item.m_sellVolume10 = binary.ReadInt();
                item.m_sellPrice10 = binary.ReadFloat();
                datas.Add(item);
            }
            binary.Close();
            return 1;
        }

        /// <summary>
        /// 获取交易数据
        /// </summary>
        /// <param name="securityCode"></param>
        /// <param name="transactionDatas"></param>
        /// <param name="body"></param>
        /// <param name="bodyLength"></param>
        /// <returns></returns>
        public static int GetTransactionDatas(ref string securityCode, List<TransactionData> transactionDatas, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            securityCode = binary.ReadString();
            int num = binary.ReadInt();
            for (int i = 0; i < num; i++)
            {
                TransactionData item = new TransactionData();
                item.m_date = binary.ReadDouble();
                item.m_price = binary.ReadFloat();
                item.m_volume = binary.ReadDouble();
                item.m_type = binary.ReadChar();
                transactionDatas.Add(item);
            }
            binary.Close();
            return 1;
        }

        /// <summary>
        /// 获取预测的成交量
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public int GetVolumeForecast(int requestID, string securityCode)
        {
            Binary binary = new Binary();
            binary.WriteInt(1);
            binary.WriteString(securityCode);
            binary.WriteDouble(0.0);
            byte[] bytes = binary.GetBytes();
            binary.Close();
            return ((this.Send(new CMessage(base.GroupID, base.ServiceID, FUNCTIONID_QUOTE_GETVOLUMEFORECAST, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes)) > 0) ? 1 : 0);
        }

        /// <summary>
        /// 获取预测的成交量
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="body"></param>
        /// <param name="bodyLength"></param>
        /// <returns></returns>
        public static int GetVolumeForecastData(List<VolumeForecastData> datas, byte[] body, int bodyLength)
        {
            Binary binary = new Binary();
            binary.Write(body, bodyLength);
            int num = binary.ReadInt();
            for (int i = 0; i < num; i++)
            {
                VolumeForecastData item = new VolumeForecastData();
                item.m_securityCode = binary.ReadString();
                item.m_rate = binary.ReadDouble();
                datas.Add(item);
            }
            binary.Close();
            return 1;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message"></param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            this.SendToListener(message);
        }

        /// <summary>
        /// 请求获取历史数据
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public int PushHistoryDatas(int requestID, HistoryDataInfo dataInfo)
        {
            return ((this.Send(FUNCTIONID_QUOTE_PUSHHISTORYDATA, requestID, this.m_socketID, dataInfo) > 0) ? 1 : 0);
        }

        /// <summary>
        /// 请求获取最新的数据
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public int PushLatestDatas(int requestID, LatestDataInfo dataInfo)
        {
            return ((this.Send(FUNCTIONID_QUOTE_PUSHLATESTDATA, requestID, this.m_socketID, dataInfo) > 0) ? 1 : 0);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="requestID"></param>
        /// <param name="socketID"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public int Send(int functionID, int requestID, int socketID, HistoryDataInfo dataInfo)
        {
            Binary binary = new Binary();
            binary.WriteString(dataInfo.m_securityCode);
            binary.WriteChar((char)dataInfo.m_type);
            binary.WriteInt(dataInfo.m_size);
            binary.WriteInt(dataInfo.m_cycle);
            binary.WriteInt(dataInfo.m_subscription);
            binary.WriteDouble(dataInfo.m_startDate);
            binary.WriteDouble(dataInfo.m_endDate);
            binary.WriteBool(dataInfo.m_pushData);
            byte[] bytes = binary.GetBytes();
            int num = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="requestID"></param>
        /// <param name="socketID"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        public int Send(int functionID, int requestID, int socketID, LatestDataInfo dataInfo)
        {
            Binary binary = new Binary();
            binary.WriteString(dataInfo.m_codes);
            binary.WriteChar((char)dataInfo.m_formatType);
            binary.WriteChar((char)dataInfo.m_lv2);
            binary.WriteInt(dataInfo.m_size);
            byte[] bytes = binary.GetBytes();
            int num = this.Send(new CMessage(base.GroupID, base.ServiceID, functionID, base.SessionID, requestID, socketID, 0, base.CompressType, bytes.Length, bytes));
            binary.Close();
            return num;
        }

        /// <summary>
        /// 停止推送历史数据
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public int StopPushHistoryDatas(int requestID)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("1");
            return ((this.Send(new CMessage(base.GroupID, base.ServiceID, FUNCTIONID_QUOTE_STOPPUSHHISTORYDATA, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes)) > 0) ? 1 : 0);
        }

        /// <summary>
        /// 停止推送最新数据
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public int StopPushLatestDatas(int requestID)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("1");
            return ((this.Send(new CMessage(base.GroupID, base.ServiceID, FUNCTIONID_QUOTE_STOPPUSHLATESTDATA, base.SessionID, requestID, this.m_socketID, 0, base.CompressType, bytes.Length, bytes)) > 0) ? 1 : 0);
        }
    }
}

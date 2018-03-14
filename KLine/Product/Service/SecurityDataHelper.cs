using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// 股票数据处理
    /// </summary>
    public class SecurityDataHelper
    {
        /// <summary>
        /// 创建数据源
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <returns>数据源</returns>
        public static CTable CreateDataSource(ChartA chart)
        {
            CTable dataSource = chart.Native.CreateTable();
            dataSource.AddColumn(KeyFields.CLOSE_INDEX);
            dataSource.AddColumn(KeyFields.HIGH_INDEX);
            dataSource.AddColumn(KeyFields.LOW_INDEX);
            dataSource.AddColumn(KeyFields.OPEN_INDEX);
            dataSource.AddColumn(KeyFields.VOL_INDEX);
            dataSource.AddColumn(KeyFields.AMOUNT_INDEX);
            return dataSource;
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="text">文本</param>
        /// <param name="parameters">参数</param>
        public static CIndicator CreateIndicator(ChartA chart, CTable dataSource, String text, String parameters)
        {
            CIndicator indicator = chart.Native.CreateIndicator();
            indicator.DataSource = dataSource;
            indicator.Name = "";
            //indicator.FullName = "";
            if (dataSource != null)
            {
                indicator.SetSourceField(KeyFields.CLOSE, KeyFields.CLOSE_INDEX);
                indicator.SetSourceField(KeyFields.HIGH, KeyFields.HIGH_INDEX);
                indicator.SetSourceField(KeyFields.LOW, KeyFields.LOW_INDEX);
                indicator.SetSourceField(KeyFields.OPEN, KeyFields.OPEN_INDEX);
                indicator.SetSourceField(KeyFields.VOL, KeyFields.VOL_INDEX);
                indicator.SetSourceField(KeyFields.AMOUNT, KeyFields.AMOUNT_INDEX);
                indicator.SetSourceField(KeyFields.CLOSE.Substring(0, 1), KeyFields.CLOSE_INDEX);
                indicator.SetSourceField(KeyFields.HIGH.Substring(0, 1), KeyFields.HIGH_INDEX);
                indicator.SetSourceField(KeyFields.LOW.Substring(0, 1), KeyFields.LOW_INDEX);
                indicator.SetSourceField(KeyFields.OPEN.Substring(0, 1), KeyFields.OPEN_INDEX);
                indicator.SetSourceField(KeyFields.VOL.Substring(0, 1), KeyFields.VOL_INDEX);
                indicator.SetSourceField(KeyFields.AMOUNT.Substring(0, 1), KeyFields.AMOUNT_INDEX);
            }
            IndicatorData indicatorData = new IndicatorData();
            indicatorData.m_parameters = parameters;
            indicatorData.m_script = text;
            indicator.Tag = indicatorData;
            String constValue = "";
            if (parameters != null && parameters.Length > 0)
            {
                String[] strs = parameters.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                int strsSize = strs.Length;
                for (int i = 0; i < strsSize; i++)
                {
                    String str = strs[i];
                    String[] strs2 = str.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    constValue += "const " + strs2[0] + ":" + strs2[3] + ";";
                }
            }
            if (text != null && text.Length > 0)
            {
                indicator.Script = constValue + text;
            }
            return indicator;
        }

        /// <summary>
        /// 绑定历史数据
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="indicators">指标</param>
        /// <param name="fields">字段</param>
        /// <param name="historyDatas">历史数据</param>
        public static void BindHistoryDatas(ChartA chart, CTable dataSource, List<CIndicator> indicators, int[] fields, List<SecurityData> historyDatas)
        {
            dataSource.Clear();
            int size = historyDatas.Count;
            dataSource.SetRowsCapacity(size + 10);
            dataSource.SetRowsGrowStep(100);
            int columnsCount = dataSource.ColumnsCount;
            for (int i = 0; i < size; i++)
            {
                SecurityData securityData = historyDatas[i];
                if (dataSource == chart.DataSource)
                {
                    InsertData(chart, dataSource, fields, securityData);
                }
                else
                {
                    double[] ary = new double[columnsCount];
                    ary[0] = securityData.m_close;
                    ary[1] = securityData.m_high;
                    ary[2] = securityData.m_low;
                    ary[3] = securityData.m_open;
                    ary[4] = securityData.m_volume;
                    for (int j = 5; j < columnsCount; j++)
                    {
                        ary[j] = double.NaN;
                    }
                    dataSource.AddRow(securityData.m_date, ary, columnsCount);
                }
            }
            int indicatorsSize = indicators.Count;
            for (int i = 0; i < indicatorsSize; i++)
            {
                indicators[i].OnCalculate(0);
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="chart">证券控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="fields">字段</param>
        /// <param name="securityData">证券数据</param>
        /// <returns>索引</returns>
        public static int InsertData(ChartA chart, CTable dataSource, int[] fields, SecurityData securityData)
        {
            double close = securityData.m_close, high = securityData.m_high, low = securityData.m_low, open = securityData.m_open, avgPrice = securityData.m_avgPrice, volume = securityData.m_volume, amount = securityData.m_amount;
            if (volume > 0 || close > 0)
            {
                if (high == 0)
                {
                    high = close;
                }
                if (low == 0)
                {
                    low = close;
                }
                if (open == 0)
                {
                    open = close;
                }
                if (avgPrice == 0)
                {
                    avgPrice = double.NaN;
                }
            }
            else
            {
                close = double.NaN;
                high = double.NaN;
                low = double.NaN;
                open = double.NaN;
                volume = double.NaN;
                amount = double.NaN;
                avgPrice = double.NaN;
            }
            double date = securityData.m_date;
            dataSource.Set(date, fields[4], volume);
            int index = dataSource.GetRowIndex(date);
            dataSource.Set2(index, fields[0], close);
            dataSource.Set2(index, fields[1], high);
            dataSource.Set2(index, fields[2], low);
            dataSource.Set2(index, fields[3], open);
            dataSource.Set2(index, fields[5], amount);
            dataSource.Set2(index, fields[6], avgPrice);
            return index;
        }

        /// <summary>
        /// 插入最新数据
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="indicators">指标</param>
        /// <param name="fields">字段</param>
        /// <param name="historyDatas">最近的历史数据</param>
        /// <param name="latestData">实时数据</param>
        /// <returns>索引</returns>
        public static int InsertLatestData(ChartA chart, CTable dataSource, List<CIndicator> indicators, int[] fields, SecurityData latestData)
        {
            if (latestData.m_close > 0 && latestData.m_volume > 0)
            {
                int indicatorsSize = indicators.Count;
                int index = InsertData(chart, dataSource, fields, latestData);
                for (int i = 0; i < indicatorsSize; i++)
                {
                    indicators[i].OnCalculate(index);
                }
                return index;
            }
            else
            {
                return -1;
            }
        }
    }
}

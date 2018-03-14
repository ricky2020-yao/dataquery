/*****************************************************************************\
*                                                                             *
* SandBoxWindow.cs - Sandbox window functions, types, and definitions.        *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/6/4.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ɳ������Ԥ��ͼ��
    /// </summary>
    public class SandBoxCandleDiv : ControlA
    {
        #region Ҧ���� 2016/6/4
        /// <summary>
        /// ���캯��
        /// </summary>
        public SandBoxCandleDiv()
        {
            BackColor = COLOR.EMPTY;
            BorderColor = COLOR.EMPTY;
            TopMost = true;
        }

        /// <summary>
        /// �ƶ���Ŀ��K����ߵ��Y����
        /// </summary>
        private float m_destHighY;

        /// <summary>
        /// �ƶ�����Ŀ�ĵ�����
        /// </summary>
        private POINT m_destPoint;

        /// <summary>
        /// �Ƿ��Ѿ�����
        /// </summary>
        private bool m_moveStart;

        /// <summary>
        /// K�����ɵ�����
        /// </summary>
        private POINT m_originLocation;

        /// <summary>
        /// ÿ�ζ�̬�ƶ��Ĳ���
        /// </summary>
        private float m_perDyncMoveStep;

        /// <summary>
        /// ÿ�ι̶��ƶ��Ĳ���
        /// </summary>
        private int m_perFixMoveStep = 10;

        /// <summary>
        /// ÿ�ι̶��ƶ��Ĵ���
        /// </summary>
        private int m_perFixMoveTimes = 10;

        /// <summary>
        /// ���й켣��б��
        /// </summary>
        private float m_slope;

        /// <summary>
        /// �ƶ���������ʱͣ�����λ��
        /// </summary>
        private POINT m_tempStops;

        /// <summary>
        /// �Ѿ��ƶ��Ĵ���
        /// </summary>
        private int m_ticks;

        /// <summary>
        /// ���»������ƶ�����������
        /// </summary>
        private int m_totalDistance = 50;

        /// <summary>
        /// Y��Ľؾ�
        /// </summary>
        private float m_yIntercept;

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ�����ù�Ʊ�ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private SecurityData m_data = new SecurityData();
        /// <summary>
        /// ��ȡ�������û���K�ߵ��������
        /// </summary>
        public SecurityData Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        private int m_direction;

        /// <summary>
        /// ��ȡ�����÷���
        /// </summary>
        public int Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }

        private bool m_moving;

        /// <summary>
        /// ��ȡ�������Ƿ������ƶ���
        /// </summary>
        public bool Moving
        {
            get { return m_moving; }
            set
            {
                if (value)
                {
                    m_moving = value;
                    m_originLocation = Location;
                }
            }
        }

        /// <summary>
        /// ��ȡK���ƶ��е�����
        /// </summary>
        /// <returns>״̬</returns>
        private int GetMovingLocation()
        {
            int height = Height;
            m_perFixMoveStep = 10;
            GetDestPos(ref m_destPoint, ref m_destHighY);
            int tempLocationY = m_originLocation.y - m_totalDistance;
            if (m_direction == 1)
            {
                tempLocationY = m_originLocation.y + m_totalDistance + height;
            }
            m_tempStops.x = m_originLocation.x;
            m_tempStops.y = tempLocationY;
            float slope = 0, bYIntercept = 0;
            if (m_tempStops.y > m_destPoint.y)
            {
                slope = (float)(m_destPoint.y - m_tempStops.y + 5) / (float)(m_destPoint.x - m_tempStops.x);
                bYIntercept = m_destPoint.y - slope * m_destPoint.x;
            }
            else
            {
                slope = (float)(m_tempStops.y - m_destPoint.y + 5) / (float)(m_tempStops.x - m_destPoint.x);
                bYIntercept = m_destPoint.y - slope * m_destPoint.x;
            }
            m_slope = slope;
            m_yIntercept = bYIntercept;
            m_perDyncMoveStep = (m_destPoint.x - m_tempStops.x + 5) / m_perFixMoveTimes;
            return 1;
        }

        /// <summary>
        /// ��ȡĿ��K�ߵ�����
        /// </summary>
        /// <param name="location">Ŀ��K�ߵ�����</param>
        /// <param name="destHighY">�ƶ���Ŀ��K����ߵ��Y����</param>
        private void GetDestPos(ref POINT location, ref float destHighY)
        {
            ChartA chart = m_chart.Chart;
            CTable dataSource = chart.DataSource;
            int rowIndex = dataSource.GetRowIndex(m_data.m_date);
            int insRowIndex = rowIndex;
            int size = dataSource.RowsCount;
            if (rowIndex == -1)
            {
                for (int i = 0; i < size; i++)
                {
                    double pk = dataSource.GetXValue(i);
                    if (pk > m_data.m_date)
                    {
                        insRowIndex = i;
                        break;
                    }
                }
            }
            if (insRowIndex == -1 || insRowIndex > chart.LastVisibleIndex)
            {
                insRowIndex = chart.LastVisibleIndex;
            }
            double insLow = dataSource.Get2(insRowIndex, KeyFields.LOW_INDEX);
            float xDest = chart.GetX(insRowIndex);
            List<CDiv> divs = chart.GetDivs();
            float yDest = chart.GetY(divs[0], insLow, AttachVScale.Left);
            location.x = (int)(xDest);
            location.y = (int)(yDest);
            destHighY = chart.GetY(divs[0], m_data.m_high, AttachVScale.Left);
        }

        /// <summary>
        /// ��ȡĿ��K�ߵĴ�С
        /// </summary>
        /// <param name="candleDiv">K�߲�</param>
        /// <param name="size">Ŀ��K�ߵĴ�С</param>
        /// <returns>K�ߵĴ�С</returns>
        private void GetDestSize(ref SIZE size)
        {
            int cw = 0, ch = 0;
            ChartA chart = m_chart.Chart;
            CTable dataSource = chart.DataSource;
            int rowsCount = dataSource.RowsCount;
            if (rowsCount <= 0)
            {
                cw = 0;
            }
            else if (rowsCount == 1)
            {
                cw = (int)(chart.GetX(0) * 2 / 3);
            }
            else
            {
                int index1 = rowsCount - 1, idnex2 = rowsCount - 2;
                cw = (int)((chart.GetX(rowsCount - 1) - chart.GetX(rowsCount - 2)) * 2 / 3);
            }
            List<CDiv> divs = chart.GetDivs();
            ch = (int)(chart.GetY(divs[0], m_data.m_low, AttachVScale.Left) - chart.GetY(divs[0], m_data.m_high, AttachVScale.Left));
            size.cx = cw;
            size.cy = ch;
        }

        /// <summary>
        /// �ػ汳������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width, height = Height;
            int left = 0, right = width, top = 0, bottom = height;
            if (!m_moving)
            {
                paint.DrawRect(CDraw.PCOLORS_LINECOLOR, 1, 0, clipRect);
                width -= 10;
                height -= 5;
                left = 5;
                right = left + width;
                top = 5;
                bottom = height + top;
            }
            if (width % 2 == 0)
            {
                width -= 1;
            }
            long color = CDraw.PCOLORS_UPCOLOR;
            if (m_data.m_low >= m_data.m_high
                || m_data.m_open < m_data.m_low
                || m_data.m_close < m_data.m_low
                || m_data.m_open > m_data.m_high
                || m_data.m_close > m_data.m_high)
            {
                paint.FillRect(color, new RECT(left, top, right, top + 1));
                return;
            }
            double perLength = (double)height / (m_data.m_high - m_data.m_low);
            POINT pointStartUp = new POINT();
            POINT pointEndUp = new POINT();
            POINT pointStartDown = new POINT();
            POINT pointEndDown = new POINT();
            bool drawUpLine = false, drawDownLine = false;
            int midX = left + width / 2 + 1;
            //��������
            if (m_data.m_close >= m_data.m_open)
            {
                top += (int)((m_data.m_high - m_data.m_close) * perLength);
                bottom = 5 + (int)((m_data.m_high - m_data.m_open) * perLength);
                if (m_data.m_high != m_data.m_close)
                {
                    pointStartUp.x = midX;
                    pointStartUp.y = 5;
                    pointEndUp.x = midX;
                    pointEndUp.y = top;
                    drawUpLine = true;
                }
                if (m_data.m_low != m_data.m_open)
                {
                    pointStartDown.x = midX;
                    pointStartDown.y = bottom;
                    pointEndDown.x = midX;
                    pointEndDown.y = height;
                    drawDownLine = true;
                }
            }
            //��������
            else if (m_data.m_close < m_data.m_open)
            {
                color = CDraw.PCOLORS_DOWNCOLOR;
                top += (int)((m_data.m_high - m_data.m_open) * perLength);
                bottom = 5 + (int)((m_data.m_high - m_data.m_close) * perLength);
                if (m_data.m_high != m_data.m_open)
                {
                    pointStartUp.x = midX;
                    pointStartUp.y = top;
                    pointEndUp.x = midX;
                    pointEndUp.y = 5;
                    drawUpLine = true;
                }
                if (m_data.m_low != m_data.m_close)
                {
                    pointStartDown.x = midX;
                    pointStartDown.y = bottom;
                    pointEndDown.x = midX;
                    pointEndDown.y = height;
                    drawDownLine = true;
                }
            }
            long lineColor = CDraw.PCOLORS_LINECOLOR;
            RECT cRect = new RECT(left, top, right, bottom);
            paint.FillRect(color, cRect);
            if (!m_moving)
            {
                paint.DrawRect(lineColor, 1, 0, cRect);
            }
            if (drawUpLine)
            {
                paint.DrawLine(color, 1, 0, pointStartUp, pointEndUp);
                if (!m_moving)
                {
                    paint.DrawLine(lineColor, 1, 0, pointStartUp.x + 1, pointStartUp.y, pointEndUp.x + 1, pointEndUp.y);
                }
            }
            if (drawDownLine)
            {
                paint.DrawLine(color, 1, 0, pointStartDown, pointEndDown);
                if (!m_moving)
                {
                    paint.DrawLine(lineColor, 1, 0, pointStartDown.x + 1, pointStartDown.y, pointEndDown.x + 1, pointEndDown.y);
                }
            }
        }

        /// <summary>
        /// ִ�������
        /// </summary>
        /// <returns>�Ƿ����</returns>
        public bool OnTimer()
        {
            if (!m_moving)
            {
                return false;
            }
            GetMovingLocation();
            POINT location = Location;
            float xMove = location.x;
            float yMove = location.y;
            if (!m_moveStart && ((m_direction == 0 && yMove > m_tempStops.y) 
                || (m_direction == 1 && yMove < m_tempStops.y)))
            {
                if (m_direction == 0)
                {
                    yMove -= m_perFixMoveStep;
                }
                else
                {
                    yMove += m_perDyncMoveStep;
                }
                POINT newLocation = new POINT(xMove, yMove);
                Location = newLocation;
            }
            else
            {
                m_moveStart = true;
                SIZE changeSize = new SIZE();
                GetDestSize(ref changeSize);
                Size = changeSize;
                if (m_ticks < m_perFixMoveTimes)
                {
                    m_ticks++;
                    if (m_ticks == m_perFixMoveTimes)
                    {
                        Location = m_destPoint;
                    }
                    else
                    {
                        xMove += m_perDyncMoveStep;
                        yMove = m_slope * xMove + m_yIntercept;
                        POINT newLocation = new POINT(xMove, yMove);
                        Location = newLocation;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }

    /// <summary>
    /// ɳ�����ݴ���
    /// </summary>
    public class SandBoxWindow : WindowXmlEx
    {
        #region Ҧ���� 2016/5/25
        /// <summary>
        /// ����ɳ�����ݴ���
        /// </summary>
        /// <param name="native">������</param>
        public SandBoxWindow(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\SandBoxWindow.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowSB") as WindowEx;
            //ע�����
            m_timerEvent = new ControlTimerEvent(CallTimer);
            m_window.RegisterEvent(m_timerEvent, EVENTID.TIMER);
            //ע�����¼�
            RegisterEvents(m_window);
        }

        /// <summary>
        /// Ĭ��K�ߴ�С
        /// </summary>
        private SIZE m_defaultCandleSize = new SIZE(80, 160);

        /// <summary>
        /// Ĭ��K��λ��
        /// </summary>
        private POINT m_defaultCandleLocation = new POINT(320, 60);

        /// <summary>
        /// Ĭ�ϴ����С
        /// </summary>
        private SIZE m_defaultWindowSize = new SIZE(260, 300);

        /// <summary>
        /// K���б�
        /// </summary>
        private List<SandBoxCandleDiv> m_divs = new List<SandBoxCandleDiv>();

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// ����¼�
        /// </summary>
        private ControlTimerEvent m_timerEvent;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ�����ù�Ʊ�ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set
            {
                m_chart = value;
                ChartA chart = m_chart.Chart;
                CTable dataSource = chart.DataSource;
                int dataSize = dataSource.RowsCount;
                if (dataSize == 0)
                {
                    return;
                }
                double date = dataSource.GetXValue(dataSize - 1);
                SetSpinValue(date);
            }
        }

        private bool m_useAnimation;

        /// <summary>
        /// ��ȡ�������Ƿ�ʹ�ö���Ч��
        /// </summary>
        public bool UseAnimation
        {
            get { return m_useAnimation; }
            set { m_useAnimation = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// ���
        /// </summary>
        private void Add()
        {
            INativeBase native = Native;
            int divsSize = m_divs.Count;
            for (int i = 0; i < divsSize; i++)
            {
                SandBoxCandleDiv div = m_divs[i];
                if (!div.Moving)
                {
                    int clientX = native.ClientX(div);
                    int clientY = native.ClientY(div);
                    int direction = 0;
                    if (clientY < 100)
                    {
                        direction = 1;
                        clientY += 150;
                    }
                    else
                    {
                        clientY -= 150;
                    }
                    m_window.RemoveControl(div);
                    div.Parent = null;
                    m_native.AddControl(div);
                    POINT newLocation = new POINT(clientX, clientY);
                    div.Direction = direction;
                    div.Location = newLocation;
                    div.Moving = true;
                }
            }
            CreateNextData(true);
            CreatePreView();
            m_native.Invalidate();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="timerID">���ID</param>
        private void CallTimer(object sender, int timerID)
        {
            OnTimer(timerID);
        }

        /// <summary>
        /// �����ť����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                //���
                if (name == "btnAdd")
                {
                    Add();
                }
                //�ر�
                else if (name == "btnClose")
                {
                    Close();
                }
                // ɾ��
                else if (name == "btnDelete")
                {
                    Delete();
                }
                // ��һ������
                else if (name == "btnNext")
                {
                    CreateNextData(true);
                }
            }
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// ����K�ߵ�λ��
        /// </summary>
        /// <param name="size">����K�ߵĴ�С</param>
        /// <param name="candleSeparater">������K�߼��</param>
        /// <param name="newPoint">������K��λ��</param>
        private void CreateCandlePos(SIZE size, float candleSeparater, ref POINT newPoint)
        {
            SIZE wSize = m_window.Size;
            int divsSize = m_divs.Count;
            int waitDivsSize = 0;
            for (int i = 0; i < divsSize; i++)
            {
                if (!m_divs[i].Moving)
                {
                    waitDivsSize++;
                }
            }
            if (waitDivsSize == 0)
            {
                newPoint.x = m_defaultCandleLocation.x;
                newPoint.y = m_defaultCandleLocation.y;
                return;
            }
            float xPosition = 0;
            float rightTotalWidth = (m_defaultWindowSize.cx - 20) / 0.618F;
            float divideWidth = rightTotalWidth / (waitDivsSize + 1);
            float start = 0, end = 0;
            for (int i = 0; i < divsSize; i++)
            {
                SandBoxCandleDiv div = m_divs[i];
                if (!div.Moving)
                {
                    end = start + divideWidth;
                    xPosition = (start + end) / 2 + m_defaultWindowSize.cx;
                    newPoint.x = (int)xPosition;
                    newPoint.y = m_defaultCandleLocation.y;
                    div.Location = newPoint;
                    start = end;
                }
            }
            end = start + divideWidth;
            xPosition = (start + end) / 2 + m_defaultWindowSize.cx;
            newPoint.x = (int)xPosition;
            newPoint.y = m_defaultCandleLocation.y;
        }

        /// <summary>
        /// ��������ĸ߿��������ݣ�����K��Ԥ��ͼ��
        /// </summary>
        private void CreatePreView()
        {
            POINT newPoint = new POINT();
            SIZE newSize = m_defaultCandleSize;
            float separator = 0;
            CreateCandleSize(ref newSize, ref separator);
            CreateCandlePos(newSize, separator, ref newPoint);
            SandBoxCandleDiv div = new SandBoxCandleDiv();
            div.Chart = m_chart;
            div.Size = newSize;
            div.Location = newPoint;
            SetCandleStaticValue(div);
            m_window.AddControl(div);
            m_divs.Add(div);
            m_native.Invalidate();
        }

        /// <summary>
        /// ����K�ߵĳߴ�
        /// </summary>
        /// <param name="newSize">K�ߵĴ�С</param>
        /// <param name="separator">K�߼��</param>
        private void CreateCandleSize(ref SIZE newSize, ref float separator)
        {
            int divsSize = m_divs.Count;
            int waitDivsSize = 0;
            for (int i = 0; i < divsSize; i++)
            {
                if (!m_divs[i].Moving)
                {
                    waitDivsSize++;
                }
            }
            if (waitDivsSize == 0)
            {
                newSize.cx = m_defaultCandleSize.cx;
                newSize.cy = m_defaultCandleSize.cy;
                separator = 20;
                return;
            }
            int cw = m_defaultCandleSize.cx - waitDivsSize;
            int ch = m_defaultCandleSize.cy - waitDivsSize * (m_defaultCandleSize.cy / m_defaultCandleSize.cx);
            separator = 20 - divsSize * 0.67F;
            if (cw < 3)
            {
                cw = 3;
                ch = 6;
            }
            // ����K��֮��ļ��
            if (separator < 0.5F)
            {
                separator = 0.5F;
            }
            newSize.cx = cw;
            newSize.cy = ch;
            for (int i = 0; i < divsSize; i++)
            {
                SandBoxCandleDiv div = m_divs[i];
                if (!div.Moving)
                {
                    div.Size = newSize;
                }
            }
        }

        /// <summary>
        /// ����K�ߵ���һ������
        /// </summary>
        /// <param name="addOrDelete">��ӻ�ɾ��</param>
        private void CreateNextData(bool addOrDelete)
        {
            ChartA chart = m_chart.Chart;
            CTable dataSource = chart.DataSource;
            LabelA lblDate = GetLabel("lblDate");
            String strDate = lblDate.Text;
            if (strDate != null && strDate.Length > 0)
            {

                double date = 0;
                if (addOrDelete)
                {
                    DateTime dateTime = Convert.ToDateTime(strDate);
                    date = chart.ConvertDateToNum(dateTime);
                }
                else
                {
                    int rowsCount = dataSource.RowsCount;
                    if (rowsCount > 0)
                    {
                        date = dataSource.GetXValue(rowsCount - 1);
                    }
                }
                date += m_chart.Cycle * 60;
                SetSpinValue(date);
            }
        }

        /// <summary>
        /// ɾ��K��
        /// </summary>
        private void Delete()
        {
            ChartA chart = m_chart.Chart;
            CTable dataSource = chart.DataSource;
            int rowsCount = dataSource.RowsCount;
            if (rowsCount > 0)
            {
                dataSource.RemoveAt(rowsCount - 1);
            }
            List<CIndicator> indicators = m_chart.Indicators;
            int indicatorsSize = indicators.Count;
            for (int i = 0; i < indicatorsSize; i++)
            {
                indicators[i].OnCalculate(0);
            }
            CreateNextData(false);
            m_chart.Chart.Update();
            m_native.Invalidate();
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                int divsSize = m_divs.Count;
                for (int i = 0; i < divsSize; i++)
                {
                    SandBoxCandleDiv div = m_divs[i];
                    if (div.Moving)
                    {
                        m_native.RemoveControl(div);
                    }
                    else
                    {
                        m_window.RemoveControl(div);
                    }
                    div.Dispose();
                }
                m_divs.Clear();
                if (m_window != null)
                {
                    m_window.UnRegisterEvent(m_timerEvent, EVENTID.TIMER);
                    m_timerEvent = null;
                    m_window.StopTimer(m_timerID);
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// ��ֵ����仯����
        /// </summary>
        /// <param name="spinA">��ֵ�ؼ�</param>
        public void OnSpinTextChanged(SpinA spin)
        {
            int divsSize = m_divs.Count;
            for (int i = 0; i < divsSize; i++)
            {
                SandBoxCandleDiv div = m_divs[i];
                if (!div.Moving)
                {
                    SecurityData data = div.Data;
                    String name = spin.Name;
                    double value = spin.Value;
                    if (name == "txtOpen")
                    {
                        data.m_open = (float)value;
                    }
                    else if (name == "txtClose")
                    {
                        data.m_close = (float)value;
                    }
                    else if (name == "txtHigh")
                    {
                        data.m_high = (float)value;
                    }
                    else if (name == "txtLow")
                    {
                        data.m_low = (float)value;
                    }
                    else if (name == "txtVolume")
                    {
                        data.m_volume = value;
                    }
                    else if (name == "txtAmount")
                    {
                        data.m_amount = value;
                    }
                    div.Data = data;
                    m_native.Invalidate();
                    break;
                }
            }
        }

        /// <summary>
        /// ִ�������
        /// </summary>
        /// <param name="timerID">���ID</param>
        public void OnTimer(int timerID)
        {
            if (m_timerID == timerID)
            {
                INativeBase native = Native;
                ChartA chart = m_chart.Chart;
                CTable dataSource = chart.DataSource;
                int divsSize = m_divs.Count;
                bool painting = false;
                int cx = native.ClientX(m_window);
                int cy = native.ClientY(m_window);
                List<SandBoxCandleDiv> removeDivs = new List<SandBoxCandleDiv>();
                for (int i = 0; i < divsSize; i++)
                {
                    SandBoxCandleDiv div = m_divs[i];
                    //�ж��Ƿ���Ҫ�ػ�
                    if (div.Moving)
                    {
                        painting = true;
                    }
                    //ִ�����
                    if (m_divs[i].OnTimer())
                    {
                        SecurityData data = div.Data;
                        int rowIndex = dataSource.GetRowIndex(data.m_date);
                        int[] fields = new int[7];
                        fields[0] = KeyFields.CLOSE_INDEX;
                        fields[1] = KeyFields.HIGH_INDEX;
                        fields[2] = KeyFields.LOW_INDEX;
                        fields[3] = KeyFields.OPEN_INDEX;
                        fields[4] = KeyFields.VOL_INDEX;
                        fields[5] = KeyFields.AMOUNT_INDEX;
                        fields[6] = KeyFields.AVGPRICE_INDEX;
                        //��������
                        SecurityDataHelper.InsertLatestData(m_chart.Chart, dataSource, m_chart.Indicators, fields, data);
                        removeDivs.Add(div);
                    }
                }
                //�Ƴ��ؼ�
                int removeDivsSize = removeDivs.Count;
                if (removeDivsSize > 0)
                {
                    for (int i = 0; i < removeDivsSize; i++)
                    {
                        SandBoxCandleDiv div = removeDivs[i];
                        m_native.RemoveControl(div);
                        m_divs.Remove(div);
                        div.Dispose();
                    }
                    removeDivs.Clear();
                    m_chart.Chart.Update();
                }
                //�ػ�
                if (painting)
                {
                    m_native.Invalidate();
                }
            }
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            ControlEvent spinInputChangedEvent = new ControlEvent(SpinTextChanged);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ButtonA button = controls[i] as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                SpinA spin = controls[i] as SpinA;
                if (spin != null)
                {
                    spin.RegisterEvent(spinInputChangedEvent, EVENTID.TEXTCHANGED);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ȡK�߾�̬��ֵ
        /// </summary>
        /// <param name="div">K�߲�</param>
        private void SetCandleStaticValue(SandBoxCandleDiv div)
        {
            SecurityData data = div.Data;
            SpinA spinOpen = GetSpin("txtOpen");
            SpinA spinClose = GetSpin("txtClose");
            SpinA spinHigh = GetSpin("txtHigh");
            SpinA spinLow = GetSpin("txtLow");
            SpinA spinVolume = GetSpin("txtVolume");
            SpinA spinAmount = GetSpin("txtAmount");
            LabelA lblDate = GetLabel("lblDate");
            data.m_open = (float)spinOpen.Value;
            data.m_close = (float)spinClose.Value;
            data.m_high = (float)spinHigh.Value;
            data.m_low = (float)spinLow.Value;
            data.m_volume = spinVolume.Value;
            data.m_amount = spinAmount.Value;
            String strDate = lblDate.Text;
            if (strDate != null && strDate.Length > 0)
            {
                data.m_date = m_chart.Chart.ConvertDateToNum(Convert.ToDateTime(strDate));
            }
            div.Data = data;
        }

        /// <summary>
        /// ��ָ������K�ߵĸ߿������������õ��������ֵ�ؼ���
        /// </summary>
        /// <param name="date">����</param>
        private void SetSpinValue(double date)
        {
            ChartA chart = m_chart.Chart;
            CTable dataSource = chart.DataSource;
            int rowsCount = dataSource.RowsCount;
            if (rowsCount > 0)
            {
                int index = dataSource.GetRowIndex(date);
                if (index < 1)
                {
                    index = rowsCount - 1;
                }
                SpinA spinOpen = GetSpin("txtOpen");
                SpinA spinClose = GetSpin("txtClose");
                SpinA spinHigh = GetSpin("txtHigh");
                SpinA spinLow = GetSpin("txtLow");
                SpinA spinVolume = GetSpin("txtVolume");
                SpinA spinAmount = GetSpin("txtAmount");
                LabelA lblDate = GetLabel("lblDate");
                spinOpen.Value = dataSource.Get2(index, KeyFields.OPEN_INDEX);
                spinClose.Value = dataSource.Get2(index, KeyFields.CLOSE_INDEX);
                spinHigh.Value = dataSource.Get2(index, KeyFields.HIGH_INDEX);
                spinLow.Value = dataSource.Get2(index, KeyFields.LOW_INDEX);
                spinVolume.Value = dataSource.Get2(index, KeyFields.VOL_INDEX);
                spinAmount.Value = dataSource.Get2(index, KeyFields.AMOUNT_INDEX);
                DateTime dateTime = m_chart.Chart.ConvertNumToDate(date);
                int m_cycle = m_chart.Cycle;
                if (m_cycle <= 60)
                {
                    lblDate.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    lblDate.Text = dateTime.ToString("yyyy-MM-dd");
                }
                m_window.Invalidate();
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public override void Show()
        {
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
            m_window.StartTimer(m_timerID, 20);
            CreateNextData(true);
            CreatePreView();
        }

        /// <summary>
        /// �ı�������ֵ�仯
        /// </summary>
        /// <param name="sender">�ؼ�</param>
        private void SpinTextChanged(object sender)
        {
            SpinA spin = sender as SpinA;
            OnSpinTextChanged(spin);
        }
        #endregion
    }
}

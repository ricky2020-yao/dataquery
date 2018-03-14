/*******************************************************************************\
*                                                                               *
* UserSecurityListT2.cs - User security list functions, types, and definitions. *
*                                                                               *
*               Version 1.00  ����                                            *
*                                                                               *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.        *
*               Created by Lord 2016/3/31.                                       *
*                                                                               *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// ��ѡ�ɵ�Ԫ��
    /// </summary>
    public class UserSecurityCellT2 : ButtonA
    {
        #region Lord 2016/3/31
        /// <summary>
        /// ������Ԫ��
        /// </summary>
        public UserSecurityCellT2()
        {
            AllowDrag = true;
            BorderColor = COLOR.EMPTY;
            ResourcePath = DataCenter.GetAppPath() + "\\config\\images";
        }

        private int m_clicks;

        /// <summary>
        /// ��ȡ�����õ������
        /// </summary>
        public int Clicks
        {
            get { return m_clicks; }
            set { m_clicks = value; }
        }

        private int m_index;

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
        }

        private UserSecurityListT2 m_list;

        /// <summary>
        /// ��ȡ�������б�
        /// </summary>
        public UserSecurityListT2 List
        {
            get { return m_list; }
            set { m_list = value; }
        }

        private int m_mode = 1;

        /// <summary>
        /// ��ȡ������ģʽ
        /// </summary>
        public int Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        private RECT m_paintRect;

        /// <summary>
        /// ��ȡ�����û��ƾ���
        /// </summary>
        public RECT PaintRect
        {
            get { return m_paintRect; }
            set { m_paintRect = value; }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        public double Rate
        {
            get
            {
                if (m_mode == 1)
                {
                    return m_clicks;
                }
                else if (m_mode == 2)
                {
                    double rate = 0;
                    if (m_list.m_latestDatas.ContainsKey(m_securityCode))
                    {
                        SecurityLatestData latestData = m_list.m_latestDatas[m_securityCode];
                        double close = latestData.m_close, lastClose = latestData.m_lastClose;
                        if (lastClose == 0)
                        {
                            lastClose = latestData.m_close;
                        }
                        double sub = 0;
                        if (close == 0)
                        {
                            sub = latestData.m_buyPrice1 - lastClose;
                            rate = 100 * (latestData.m_buyPrice1 - lastClose) / lastClose;
                        }
                        else
                        {
                            sub = close - latestData.m_lastClose;
                            rate = 100 * (close - lastClose) / lastClose;
                        }
                    }
                    return rate;
                }
                else
                {
                    return 0;
                }
            }
        }

        private String m_securityCode;

        /// <summary>
        /// ��ȡ�����ù�Ʊ����
        /// </summary>
        public String SecurityCode
        {
            get { return m_securityCode; }
            set { m_securityCode = value; }
        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public override void OnClick(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_list != null)
            {
                m_list.OnClickCell(this, mp, button, clicks, delta);
            }
        }

        /// <summary>
        /// �϶���ʼ����
        /// </summary>
        /// <returns>�Ƿ��϶�</returns>
        public override bool OnDragBegin()
        {
            return m_list.IsEditing;
        }

        /// <summary>
        /// �϶�����
        /// </summary>
        public override void OnDragging()
        {
            base.OnDragging();
            if (m_list != null)
            {
                m_list.OnCellDragging(this);
            }
        }

        /// <summary>
        /// ��갴�·���
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public override void OnMouseDown(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_list != null)
            {
                m_list.OnCellMouseDown(this, mp, button, clicks, delta);
            }
        }

        /// <summary>
        /// ����ƶ�����
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public override void OnMouseMove(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_list != null)
            {
                m_list.OnCellMouseMove(this, mp, button, clicks, delta);
            }
        }

        /// <summary>
        /// ���̧�𷽷�
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public override void OnMouseUp(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_list != null)
            {
                m_list.OnCellMouseUp(this, mp, button, clicks, delta);
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public override void OnMouseWheel(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_list != null)
            {
                m_list.OnCellMouseWheel(this, mp, button, clicks, delta);
            }
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaint(CPaint paint, RECT clipRect)
        {
            int width = Width, height = Height;
            RECT rect = new RECT(1, 1, width, height);
            int cornerRadius = 10;
            int cLeft = 1, cTop = 1;
            bool isDragging = IsDragging, isEditing = m_list.IsEditing;
            if (!isDragging && isEditing)
            {
                Random rd = m_list.m_rd;
                rect.left += rd.Next(0, 15);
                rect.top += rd.Next(0, 15);
                rect.right -= rd.Next(0, 15);
                rect.bottom -= rd.Next(0, 15);
                cornerRadius = rd.Next(10, 20);
                cLeft += rd.Next(0, 5);
                cTop += rd.Next(0, 5);
            }
            //paint.DrawGradientRect(COLOR.ARGB(200, 0, 0, 0), COLOR.ARGB(200, 0, 0, 0), rect, cornerRadius, 0);
            paint.DrawRoundRect(COLOR.ARGB(200, 0, 0, 0), 1, 0, rect, cornerRadius);
            if (this == m_list.SelectedCell)
            {
                paint.DrawRoundRect(CDraw.PCOLORS_LINECOLOR, 1, 0, rect, cornerRadius);
            }
            else
            {
                paint.DrawRoundRect(CDraw.PCOLORS_LINECOLOR3, 1, 0, rect, cornerRadius);
            }
            SecurityServiceEx securityService = DataCenter.SecurityService;
            Security security = new Security();
            if (securityService.GetSecurityByCode(m_securityCode, ref security))
            {
                String securityName = security.m_name;
                FONT font = new FONT("΢���ź�", 20, false, false, false);
                FONT font2 = new FONT("Arial", 18, false, false, false);
                if (m_index >= 2 && m_index <= 5)
                {
                    font.m_fontSize = 18;
                    font2.m_fontSize = 16;
                }
                else if (m_index >= 6)
                {
                    font.m_fontSize = 16;
                    font2.m_fontSize = 14;
                }
                String drawCode = m_securityCode.Substring(0, m_securityCode.IndexOf("."));
                int left = rect.left + 10;
                SIZE tSize = paint.TextSize(securityName, font);
                SIZE tSize2 = paint.TextSize(drawCode, font2);
                int top = (height - tSize.cy - tSize2.cy) / 2;
                int mid = left + Math.Max(tSize.cx, tSize2.cx) / 2;
                CDraw.DrawText(paint, drawCode, CDraw.PCOLORS_FORECOLOR, font2, mid - tSize2.cx / 2, top);
                CDraw.DrawText(paint, securityName, CDraw.PCOLORS_FORECOLOR, font, mid - tSize.cx / 2, top + tSize2.cy);
                left += Math.Max(tSize.cx, tSize2.cx) + 20;
                if (m_list.m_latestDatas.ContainsKey(m_securityCode))
                {
                    SecurityLatestData latestData = m_list.m_latestDatas[m_securityCode];
                    double close = latestData.m_close, lastClose = latestData.m_lastClose;
                    if (lastClose == 0)
                    {
                        lastClose = latestData.m_close;
                    }
                    //����
                    double diff = 0;
                    double rate = 0;
                    if (close == 0)
                    {
                        diff = latestData.m_buyPrice1 - lastClose;
                        rate = 100 * (latestData.m_buyPrice1 - lastClose) / lastClose;
                    }
                    else
                    {
                        diff = close - latestData.m_lastClose;
                        rate = 100 * (close - lastClose) / lastClose;
                    }
                    long color = CDraw.GetPriceColor(close, lastClose);
                    String strRate = rate.ToString("0.00") + "%", strClose = close.ToString("0.00");
                    if (rate > 0)
                    {
                        strRate = "+" + strRate;
                    }
                    tSize = paint.TextSize(strRate, font2);
                    tSize2 = paint.TextSize(strClose, font2);
                    if (m_index > 1)
                    {
                        mid = left + Math.Max(tSize.cx, tSize2.cx) / 2;
                        CDraw.DrawText(paint, strClose, color, font2, mid - tSize2.cx / 2, top);
                        CDraw.DrawText(paint, strRate, color, font2, mid - tSize.cx / 2, top + tSize2.cy + 5);
                    }
                    else
                    {
                        CDraw.DrawText(paint, strClose, color, font2, left, top);
                        CDraw.DrawText(paint, strRate, color, font2, left + tSize.cx, top);
                    }
                }
            }
            //����ͼ��
            if (m_index <= 1)
            {
                int iLeft = 100, iTop = 45, iSize = 30;
                if (!isDragging && isEditing)
                {
                    Random rd = m_list.m_rd;
                    iLeft += rd.Next(0, 10);
                    iTop += rd.Next(0, 10);
                }
                List<String> images = new List<String>();
                images.Add("attention.png");
                images.Add("search.png");
                images.Add("note.png");
                images.Add("news.png");
                for (int i = 0; i < 4; i++)
                {
                    paint.DrawImage(images[i], new RECT(iLeft, iTop, iLeft + iSize, iTop + iSize));
                    iLeft += iSize + 20;
                }
                images.Clear();
            }
            //�Ƿ����ڹر�
            if (!isDragging && isEditing)
            {
                FONT font = new FONT("΢���ź�", 16, false, false, false);
                SIZE cSize = new SIZE(30, 30);
                long lineColor = CDraw.PCOLORS_LINECOLOR;
                RECT ellipseRect = new RECT(cLeft, cTop, cLeft + cSize.cx, cTop + cSize.cy);
                paint.FillEllipse(CDraw.PCOLORS_UPCOLOR, ellipseRect);
                SIZE tSize = paint.TextSize("ɾ", font);
                CDraw.DrawText(paint, "ɾ", COLOR.CONTROLTEXT, font, ellipseRect.left + (cSize.cx - tSize.cx) / 2, ellipseRect.top + (cSize.cy - tSize.cy) / 2);
                paint.DrawEllipse(lineColor, 1, 0, ellipseRect);
            }
        }
        #endregion
    }

    /// <summary>
    /// ��ѡ�ɵ�Ԫ������
    /// </summary>
    public class UserSecurityCellT2Compare : IComparer<UserSecurityCellT2>
    {
        /// <summary>
        /// �Ƚ�˳��
        /// </summary>
        /// <param name="x">����1</param>
        /// <param name="y">����2</param>
        /// <returns>���</returns>
        public int Compare(UserSecurityCellT2 x, UserSecurityCellT2 y)
        {
            return y.Rate.CompareTo(x.Rate);
        }
    }

    /// <summary>
    /// ��ѡ�ɽ���
    /// </summary>
    public class UserSecurityListT2 : DivA
    {
        #region Lord 2016/1/8
        /// <summary>
        /// ������ѡ�ɽ���
        /// </summary>
        /// <param name="native">������</param>
        public UserSecurityListT2()
        {
            AllowDrag = true;
            BackColor = COLOR.EMPTY;
            ShowVScrollBar = true;
        }

        /// <summary>
        /// ��Ԫ��
        /// </summary>
        private List<UserSecurityCellT2> m_cells = new List<UserSecurityCellT2>();

        /// <summary>
        /// ��Ԫ��״̬
        /// </summary>
        private int m_cellState;

        /// <summary>
        /// �������ݻص�
        /// </summary>
        private ListenerMessageCallBack m_latestDataCallBack;

        /// <summary>
        /// �������ݵ�����ID
        /// </summary>
        private int m_latestDataRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��������
        /// </summary>
        public Dictionary<String, SecurityLatestData> m_latestDatas = new Dictionary<String, SecurityLatestData>();

        /// <summary>
        /// �������
        /// </summary>
        private QuoteService m_quoteService;

        /// <summary>
        /// �漴����
        /// </summary>
        public Random m_rd = new Random();

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// ������
        /// </summary>
        private int m_tick;

        /// <summary>
        /// ������2
        /// </summary>
        private int m_tick2;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        /// <summary>
        /// ��ѡ�����ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_userSecurityDataCallBack;

        /// <summary>
        /// ��ѡ�ɷ���
        /// </summary>
        private UserSecurityServiceEx m_userSecurityService;

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ�����ù�Ʊ�ؼ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private bool m_isEditing;

        /// <summary>
        /// ��ȡ�������Ƿ����ڱ༭
        /// </summary>
        public bool IsEditing
        {
            get { return m_isEditing; }
            set { m_isEditing = value; }
        }

        private UserSecurityCellT2 m_selectedCell;

        /// <summary>
        /// ��ȡ������ѡ�еĵ�Ԫ��
        /// </summary>
        public UserSecurityCellT2 SelectedCell
        {
            get { return m_selectedCell; }
            set { m_selectedCell = value; }
        }

        private bool m_useAnimation = true;

        /// <summary>
        /// ��ȡ�������Ƿ�ʹ�ö���
        /// </summary>
        public bool UseAnimation
        {
            get { return m_useAnimation; }
            set { m_useAnimation = value; }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_quoteService != null)
                {
                    m_quoteService.UnRegisterListener(m_latestDataRequestID, m_latestDataCallBack);
                    m_latestDataCallBack = null;
                    m_quoteService = null;
                }
                m_latestDatas.Clear();
                if (m_userSecurityService != null)
                {
                    m_userSecurityService.UnRegisterListener(m_userSecurityService.OperatorRequestID, m_userSecurityDataCallBack);
                    m_userSecurityDataCallBack = null;
                    m_userSecurityService = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// ��ȡ�˶�λ��
        /// </summary>
        /// <param name="now">��ǰλ��</param>
        /// <param name="to">Ŀ��λ��</param>
        /// <returns>�˶�λ��</returns>
        private int GetAnimationPos(int now, int to)
        {
            int sub = Math.Abs(now - to);
            if (sub < 10)
            {
                now = to;
            }
            else
            {
                if (now > to)
                {
                    if (sub > 50)
                    {
                        now -= sub / 3;
                    }
                    else
                    {
                        now -= 10;
                    }
                }
                else if (now < to)
                {
                    if (sub > 50)
                    {
                        now += sub / 3;
                    }
                    else
                    {
                        now += 10;
                    }
                }
            }
            return now;
        }

        /// <summary>
        /// �������ݵĻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public void LatestDataCallBack(CMessage message)
        {
            OnLatestDataCallBack(message);
        }

        /// <summary>
        /// ���ص�Ԫ��
        /// </summary>
        private void LoadCells()
        {
            int cellsSize = m_cells.Count;
            Dictionary<String, UserSecurityCellT2> removeCells = new Dictionary<String, UserSecurityCellT2>();
            for (int i = 0; i < cellsSize; i++)
            {
                UserSecurityCellT2 cell = m_cells[i];
                removeCells[cell.SecurityCode] = cell;
            }
            m_cells.Clear();
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            m_userSecurityService.GetCategories(categories);
            UserSecurityCategory category = categories[0];
            String[] strs = category.m_codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int strsSize = strs.Length;
            for (int i = 0; i < strsSize; i++)
            {
                String securityCode = strs[i];
                if (removeCells.ContainsKey(securityCode))
                {
                    m_cells.Add(removeCells[securityCode]);
                    removeCells.Remove(securityCode);
                }
                else
                {
                    UserSecurityCellT2 cell = new UserSecurityCellT2();
                    cell.SecurityCode = securityCode;
                    cell.BackColor = COLOR.ARGB(200, 0, 0, 0);
                    cell.PaintRect = cell.Bounds;
                    cell.List = this;
                    if (m_userSecurityService.VisitsCount.ContainsKey(securityCode))
                    {
                        cell.Clicks = m_userSecurityService.VisitsCount[securityCode];
                    }
                    m_cells.Add(cell);
                    AddControl(cell);
                }
            }
            foreach (UserSecurityCellT2 cell in removeCells.Values)
            {
                if (cell == m_selectedCell)
                {
                    m_selectedCell = null;
                }
                RemoveControl(cell);
                cell.Dispose();
            }
            removeCells.Clear();
            m_cells.Sort(new UserSecurityCellT2Compare());
            Update();
        }

        /// <summary>
        /// ��ӿؼ�����
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();
            if (m_securityService == null)
            {
                //ע�����
                m_securityService = DataCenter.SecurityService;
                m_userSecurityService = DataCenter.UserSecurityService;
                m_userSecurityDataCallBack = new ListenerMessageCallBack(UserSecurityDataCallBack);
                m_userSecurityService.RegisterListener(m_userSecurityService.GetListRequestID, m_userSecurityDataCallBack);
                m_userSecurityService.RegisterListener(m_userSecurityService.OperatorRequestID, m_userSecurityDataCallBack);
                //ע������
                m_quoteService = DataCenter.QuoteService;
                m_latestDataCallBack = new ListenerMessageCallBack(LatestDataCallBack);
                m_quoteService.RegisterListener(m_latestDataRequestID, m_latestDataCallBack);
                StartTimer(m_timerID, 10);
            }
        }

        /// <summary>
        /// ��Ԫ���϶�����
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        public void OnCellDragging(UserSecurityCellT2 cell)
        {
            ControlHost host = Native.Host;
            RECT tempRect = new RECT();
            RECT bounds = cell.Bounds;
            int cellsSize = m_cells.Count;
            int thisIndex = -1;
            for (int i = 0; i < cellsSize; i++)
            {
                UserSecurityCellT2 iCell = m_cells[i];
                if (iCell == cell)
                {
                    thisIndex = i;
                    break;
                }
            }
            int mx = bounds.left + (bounds.right - bounds.left) / 2;
            int my = bounds.top + (bounds.bottom - bounds.top) / 2;
            for (int i = 0; i < cellsSize; i++)
            {
                UserSecurityCellT2 iCell = m_cells[i];
                if (iCell != cell)
                {
                    RECT iBounds = iCell.PaintRect;
                    if (host.GetIntersectRect(ref tempRect, ref bounds, ref iBounds) > 0)
                    {
                        if (mx >= iBounds.left && mx <= iBounds.right && my >= iBounds.top && my <= iBounds.bottom)
                        {
                            m_cells[thisIndex] = iCell;
                            m_cells[i] = cell;
                            Update();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// �����Ԫ��
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public void OnClickCell(UserSecurityCellT2 cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_cellState > 0)
            {
                if (m_isEditing)
                {
                    if (mp.x <= 30 && mp.y <= 30)
                    {
                        UserSecurityCategory category = new UserSecurityCategory();
                        category.m_categoryID = "primary";
                        category.m_userID = DataCenter.UserID;
                        category.m_codes = cell.SecurityCode;
                        m_userSecurityService.DeleteSecurities(m_userSecurityService.OperatorRequestID, category);
                    }
                    Invalidate();
                }
                else
                {
                    Security security = new Security();
                    m_securityService.GetSecurityByCode(cell.SecurityCode, ref security);
                    m_userSecurityService.AddVisitsCount(security.m_code);
                    m_chart.SearchSecurity(security);
                    cell.Clicks++;
                    m_cells.Sort(new UserSecurityCellT2Compare());
                    Update();
                }
            }
            m_cellState = 0;
        }

        /// <summary>
        /// ��Ԫ����갴�·���
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public void OnCellMouseDown(UserSecurityCellT2 cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_selectedCell = cell;
            m_cellState = 1;
            m_tick2 = 0;
        }

        /// <summary>
        /// ��Ԫ������ƶ�����
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public void OnCellMouseMove(UserSecurityCellT2 cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_cellState == 1)
            {
                m_cellState = 0;
            }
        }

        /// <summary>
        /// ��Ԫ�����̧��
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public void OnCellMouseUp(UserSecurityCellT2 cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_cellState == 1)
            {
                m_cellState = 2;
            }
        }

        /// <summary>
        /// ��Ԫ��������
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">������ֵ</param>
        public void OnCellMouseWheel(UserSecurityCellT2 cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            OnMouseWheel(MousePoint, button, clicks, delta);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public override void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            if (message.m_serviceID == m_quoteService.ServiceID)
            {
                LatestDataInfo dataInfo = new LatestDataInfo();
                List<SecurityLatestData> datas = new List<SecurityLatestData>();
                QuoteService.GetLatestDatas(ref dataInfo, datas, message.m_body, message.m_bodyLength);
                int datasSize = datas.Count;
                for (int i = 0; i < datasSize; i++)
                {
                    m_latestDatas[datas[i].m_securityCode] = datas[i];
                }
                datas.Clear();
            }
            else if (message.m_serviceID == m_userSecurityService.ServiceID)
            {
                if (message.m_functionID < UserSecurityService.FUNCTIONID_USERSECURITY_GETVISITSCOUNT)
                {
                    LoadCells();
                }
            }
            Invalidate();
        }

        /// <summary>
        /// �����������ݵķ���
        /// </summary>
        /// <param name="message">����</param>
        public void OnLatestDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            BeginInvoke(copyMessage);
        }

        /// <summary>
        /// ִ�������
        /// </summary>
        /// <param name="timerID">���ID</param>
        public override void OnTimer(int timerID)
        {
            base.OnTimer(timerID);
            if (m_timerID == timerID)
            {
                //�Ƿ��ͼ
                bool paint = false;
                if (m_cellState == 1)
                {
                    if (m_selectedCell != null && !m_selectedCell.IsDragging)
                    {
                        m_tick2++;
                        if (m_isEditing)
                        {
                            if (m_tick2 > 20)
                            {
                                m_isEditing = false;
                                paint = true;
                                m_cellState = 0;
                                Update();
                            }
                        }
                        else
                        {
                            if (m_tick2 > 20)
                            {
                                m_isEditing = true;
                                m_cellState = 0;
                                paint = true;
                                Update();
                            }
                        }
                    }
                }
                //��ѯ��������
                if (m_tick % 50 == 0)
                {
                    List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
                    m_userSecurityService.GetCategories(categories);
                    int categoriesSize = categories.Count;
                    if (categoriesSize > 0)
                    {
                        LatestDataInfo dataInfo = new LatestDataInfo();
                        dataInfo.m_codes = categories[0].m_codes;
                        dataInfo.m_formatType = 1;
                        m_quoteService.GetLatestDatas(m_latestDataRequestID, dataInfo);
                        categories.Clear();
                    }
                }
                int width = Width - 15, height = Height;
                int cellsSize = m_cells.Count;
                for (int i = 0; i < cellsSize; i++)
                {
                    UserSecurityCellT2 cell = m_cells[i];
                    if (!cell.IsDragging)
                    {
                        RECT bounds = cell.Bounds;
                        RECT paintRect = cell.PaintRect;
                        //��ǰ����
                        int left = bounds.left, top = bounds.top, right = bounds.right, bottom = bounds.bottom;
                        //��ͼ����
                        int pLeft = paintRect.left, pTop = paintRect.top, pRight = paintRect.right, pBottom = paintRect.bottom;
                        int subLeft = Math.Abs(left - pLeft), subTop = Math.Abs(top - pTop), subRight = Math.Abs(right - pRight), subBottom = Math.Abs(bottom - pBottom);
                        if (subTop > height || subBottom > height)
                        {
                            paint = true;
                            cell.Bounds = paintRect;
                        }
                        else
                        {
                            ///��
                            if (subLeft > 0)
                            {
                                paint = true;
                                left = GetAnimationPos(left, pLeft);
                            }
                            //��
                            if (subTop > 0)
                            {
                                paint = true;
                                top = GetAnimationPos(top, pTop);
                            }
                            //��
                            if (subRight > 0)
                            {
                                paint = true;
                                right = GetAnimationPos(right, pRight);
                            }
                            //��
                            if (subBottom > 0)
                            {
                                paint = true;
                                bottom = GetAnimationPos(bottom, pBottom);
                            }
                            cell.Bounds = new RECT(left, top, right, bottom);
                        }
                    }
                }
                m_tick++;
                if (m_tick > 1000)
                {
                    m_tick = 0;
                }
                //�رն���
                if (m_useAnimation && m_isEditing)
                {
                    if (m_tick % 2 == 0)
                    {
                        paint = true;
                    }
                }
                //��ͼ
                if (paint)
                {
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// ���²��ַ���
        /// </summary>
        public override void Update()
        {
            int width = Width - 15, height = Height;
            int uHeight = 80;
            int dTop = uHeight * 3;
            int cellsSize = m_cells.Count;
            for (int i = 0; i < cellsSize; i++)
            {
                UserSecurityCellT2 cell = m_cells[i];
                cell.Index = i;
                if (i == 0 || i == 1)
                {
                    cell.PaintRect = new RECT(0, uHeight * i, width, uHeight * (i + 1));
                }
                else if (i == 2)
                {
                    cell.PaintRect = new RECT(0, uHeight * 2, width / 2, uHeight * 3);
                }
                else if (i == 3)
                {
                    cell.PaintRect = new RECT(width / 2, uHeight * 2, width, uHeight * 3);
                }
                else if (i == 4)
                {
                    cell.PaintRect = new RECT(0, uHeight * 3, width / 2, uHeight * 4);
                }
                else if (i == 5)
                {
                    cell.PaintRect = new RECT(width / 2, uHeight * 3, width, uHeight * 4);
                }
                else
                {
                    int index = (i - 6) % 3;
                    if (index == 0)
                    {
                        dTop += uHeight;
                    }
                    int uwidth = width / 3;
                    cell.PaintRect = new RECT(uwidth * index, dTop, uwidth * (index + 1), dTop + uHeight);
                }
                if (!m_useAnimation || (cell.Left == 0 && cell.Top == 0))
                {
                    cell.Bounds = cell.PaintRect;
                }
            }
            base.Update();
            Invalidate();
        }

        /// <summary>
        /// ��ѡ�����ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void UserSecurityDataCallBack(CMessage message)
        {
            CMessage copyMessage = new CMessage();
            copyMessage.Copy(message);
            BeginInvoke(message);
        }
        #endregion
    }
}

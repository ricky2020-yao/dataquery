/*****************************************************************************\
*                                                                             *
* Sky.cs - Sky functions, types, and definitions.                             *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Sky. All rights reserved.            *
*               Created by Lord 2016/6/1.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Diagnostics;
using System.Security;
using System.Runtime.InteropServices;
using piratecat;

namespace owplane
{
    /// <summary>
    /// ���
    /// </summary>
    public class Sky : ControlA
    {
        /// <summary>
        /// �������
        /// </summary>
        public Sky()
        {
            BackColor = COLOR.ARGB(0, 0, 100, 255);
            BorderColor = COLOR.EMPTY;
            Font = new FONT("Arial", 16, false, false, false);
            ForeColor = COLOR.ARGB(255, 255, 255);
            ResourcePath = DataCenter.GetAppPath() + "\\config";
        }

        /// <summary>
        /// �ӵ�
        /// </summary>
        private List<Bullet> m_bullets = new List<Bullet>();

        /// <summary>
        /// �Ѷ�
        /// </summary>
        private int m_difficult = 1;

        /// <summary>
        /// ��߷�
        /// </summary>
        private double m_high;

        /// <summary>
        /// �ɻ�
        /// </summary>
        private Plane m_plane;

        /// <summary>
        /// ����
        /// </summary>
        private POINT m_point;

        /// <summary>
        /// �������
        /// </summary>
        private Random m_random = new Random();

        /// <summary>
        /// ��ʱ��
        /// </summary>
        private Stopwatch m_stopWatch = new Stopwatch();

        /// <summary>
        /// ϵͳ��ɫ
        /// </summary>
        private long[] m_sysColors = new long[] { COLOR.ARGB(255, 255, 255), COLOR.ARGB(255,255,0), COLOR.ARGB(255, 0, 255),
            COLOR.ARGB(0, 255, 0), COLOR.ARGB(82, 255, 255), COLOR.ARGB(255, 82, 82) };

        /// <summary>
        /// ����
        /// </summary>
        private int m_ticks;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private bool m_showRecord;

        /// <summary>
        /// ��ȡ�������Ƿ���ʾ��¼
        /// </summary>
        public bool ShowRecord
        {
            get { return m_showRecord; }
            set { m_showRecord = value; }
        }

        [SuppressUnmanagedCodeSecurity, DllImport("owmath.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void M107(float x1, float y1, float x2, float y2, float oX, float oY, ref float k, ref float b);

        /// <summary>
        /// �����ײ
        /// </summary>
        private void CheckAttach()
        {
            POINT pLocation = m_plane.Location;
            int pmx = pLocation.x;
            int pmy = pLocation.y;
            bool attach = false;
            List<Bullet> list = new List<Bullet>();
            for (int i = 0; i < m_bullets.Count; i++)
            {
                Bullet bullet = m_bullets[i];
                int bmx = bullet.Location.x;
                int bmy = bullet.Location.y;
                if (bmx >= pmx - 1 && bmx <= pmx + 1 && bmy >= pmy - 1 && bmy <= pmy + 1)
                {
                    attach = true;
                    break;
                }
            }
            double num = Convert.ToDouble(m_stopWatch.ElapsedMilliseconds);
            if (attach)
            {
                m_bullets.Clear();
                m_difficult = 1;
                m_stopWatch.Stop();
                m_stopWatch.Reset();
                m_stopWatch.Start();
            }
            else if ((num / 1000) > m_high)
            {
                m_high = num / 1000;
            }
        }

        /// <summary>
        /// �����ӵ�
        /// </summary>
        private void CreateBullets()
        {
            int size = Width / 250;
            if (size < 2)
            {
                size = 2;
            }
            for (int i = 0; i < size; i++)
            {
                int pmx = m_plane.Location.x;
                int pmy = m_plane.Location.y;
                Bullet bullet = new Bullet();
                float k = 0f;
                float b = 0f;
                while (k == 0 && b == 0)
                {
                    int type = m_random.Next(0, 4);
                    POINT location = new POINT();
                    if (type == 0)
                    {
                        location = new POINT(-5, m_random.Next(0, 100));
                    }
                    else if (type == 1)
                    {
                        location = new POINT(Width + 5, m_random.Next(0, 100));
                    }
                    else
                    {
                        location = new POINT(m_random.Next(0, Width), -5);
                    }
                    bullet.Location = location;
                    M107(location.x, location.y, (float)m_random.Next(0, Width), pmy, 0f, 0f, ref k, ref b);
                }
                bullet.K = k;
                bullet.B = b;
                bullet.Speed = m_random.Next(2, 5);
                bullet.BackColor = m_sysColors[m_random.Next(0, m_sysColors.Length)];
                m_bullets.Add(bullet);
            }
            for (int i = 0; i < 1; i++)
            {
                if (m_random.Next(0, 2) == 0)
                {
                    int pmx = m_plane.Location.x;
                    int pmy = m_plane.Location.y;
                    Bullet bullet = new Bullet();
                    float k = 0f;
                    float b = 0f;
                    while (k == 0 && b == 0)
                    {
                        int x = m_random.Next(0, Width);
                        bullet.Location = new POINT(x, -5);
                        M107((float)x, bullet.Location.y, pmx, pmy, 0f, 0f, ref k, ref b);
                    }
                    bullet.K = k;
                    bullet.B = b;
                    bullet.Speed = m_random.Next(3, 10) + m_difficult;
                    bullet.BackColor = m_sysColors[m_random.Next(0, m_sysColors.Length)];
                    m_bullets.Add(bullet);
                }
            }
        }

        /// <summary>
        /// ������Դ����
        /// </summary>
        public override void Dispose()
        {
            if(!IsDisposed)
            {
                StopTimer(m_timerID);
                m_bullets.Clear();
            }
            base.Dispose();
        }

        /// <summary>
        /// �Ƴ��ӵ�
        /// </summary>
        private void MoveBullets()
        {
            if (m_ticks % 2 == 0)
            {
                int bulletsSize = m_bullets.Count;
                for (int i = 0; i < bulletsSize; i++)
                {
                    Bullet bullet = m_bullets[i];
                    float k = bullet.K;
                    float b = bullet.B;
                    int speed = bullet.Speed;
                    float y = bullet.Location.y + bullet.Speed;
                    float x = (y - b) / k;
                    int subX = Math.Abs(bullet.Location.x - (int)x);
                    int subY = Math.Abs(bullet.Location.y - (int)y);
                    POINT bmp = bullet.Location;
                    if (subX > speed)
                    {
                        if (bmp.x > x)
                        {
                            x = bmp.x + speed;
                        }
                        else if (bmp.x > x)
                        {
                            x = bmp.x - speed;
                        }
                    }
                    if (subY > speed)
                    {
                        if (bmp.y > y)
                        {
                            y = bmp.y + speed;
                        }
                        else if (bmp.y > y)
                        {
                            y = bmp.y - speed;
                        }
                    }
                    bullet.Location = new POINT(x, y);
                }
            }
        }

        /// <summary>
        /// �ؼ���ӷ���
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();
            m_plane = new Plane();
            m_plane.Location = new POINT(Width / 2, Height - 25);
            m_point = m_plane.Location;
            m_stopWatch.Start();
            StartTimer(m_timerID, 10);
        }

        /// <summary>
        /// ��갴�·���
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        public override void OnMouseDown(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseDown(mp, button, clicks, delta);
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                m_point = mp;
            }
        }

        /// <summary>
        /// ����ƶ�����
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        public override void OnMouseMove(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseMove(mp, button, clicks, delta);
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                POINT location = m_plane.Location;
                location.x += (mp.x - m_point.x);
                location.y += (mp.y - m_point.y);
                m_plane.Location = location;
                m_point = mp;
            }
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaint(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            if (width > 0 && height > 0)
            {
                //���Ʊ���
                RECT drawRect = new RECT(0, 0, width, height);
                paint.FillRect(GetPaintingBackColor(), clipRect);
                paint.DrawImage("file='Logo.png'", new RECT(30, 50, 300, 150));
                if (m_showRecord)
                {
                    double num = Convert.ToDouble(m_stopWatch.ElapsedMilliseconds);
                    String highRecord = "HIGH " + m_high.ToString("0.000");
                    FONT font = Font;
                    SIZE tSize = paint.TextSize(highRecord, font);
                    paint.DrawText(highRecord, ForeColor, font, new RECT(Width / 2 - tSize.cx / 2, 5, Width, 20));
                    String nowRecord = "TIME" + ((num / 1000.0)).ToString("0.000");
                    paint.DrawText(nowRecord, ForeColor, font, new RECT(0, 5, 100, 20));
                }
                m_plane.OnPaintBackground(paint);
                for (int i = 0; i < m_bullets.Count; i++)
                {
                    Bullet bullet = m_bullets[i];
                    bullet.OnPaintBackground(paint);
                }
            }
        }

        /// <summary>
        /// �����
        /// </summary>
        /// <param name="timerID">���ID</param>
        public override void OnTimer(int timerID)
        {
            if (m_ticks % 5 == 0)
            {
                CreateBullets();
            }
            if (m_ticks % 500 == 0 && m_difficult < 20)
            {
                m_difficult++;
            }
            CheckAttach();
            MoveBullets();
            RemoveOutBullets();
            Invalidate();
            m_ticks++;
        }

        /// <summary>
        /// �Ƴ��ӵ�
        /// </summary>
        private void RemoveOutBullets()
        {
            int m_bulletsSize = m_bullets.Count;
            for (int i = 0; i < m_bulletsSize; i++)
            {
                Bullet bullet = m_bullets[i];
                POINT bmp = bullet.Location;
                float x = bmp.x;
                float y = bmp.y;
                if ((y < -10) || (y > Height + 50))
                {
                    m_bullets.RemoveAt(i);
                    i--;
                    m_bulletsSize--;
                }
            }
        }
    }

    /// <summary>
    /// �ɻ�
    /// </summary>
    public class Plane
    {
        /// <summary>
        /// �����ɻ�
        /// </summary>
        public Plane()
        {
        }

        /// <summary>
        /// ����
        /// </summary>
        private int m_tick = 1;

        /// <summary>
        /// ����2
        /// </summary>
        private int m_tick2 = 2;

        private long m_backColor  = COLOR.ARGB(255, 80, 80);

        /// <summary>
        /// ��ȡ�����ñ���ɫ
        /// </summary>
        public long BackColor
        {
            get { return m_backColor; }
            set { m_backColor = value; }
        }

        private POINT m_location = new POINT();

        /// <summary>
        /// ��ȡ������λ��
        /// </summary>
        public POINT Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        /// <summary>
        /// �ػ汳������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        public void OnPaintBackground(CPaint paint)
        {
            RECT drawRect = new RECT(m_location.x - 6, m_location.y - 6, m_location.x + 6, m_location.y + 6);
            paint.DrawGradientEllipse(m_backColor, COLOR.RatioColor(paint, m_backColor, 1.1), drawRect, 90);
            drawRect.left -= m_tick;
            drawRect.top -= m_tick;
            drawRect.right += m_tick;
            drawRect.bottom += m_tick;
            paint.FillEllipse(COLOR.ARGB(50, 255, 255, 0), drawRect);
            if (m_tick > 10)
            {
                m_tick = 0;
            }
            if (m_tick2 % 1 == 0)
            {
                m_tick++;
            }
            m_tick2++;
            if (m_tick2 > 1000)
            {
                m_tick2 = 0;
            }
        }
    }

    /// <summary>
    /// �ӵ�
    /// </summary>
    public class Bullet
    {
        /// <summary>
        /// �����ӵ�
        /// </summary>
        public Bullet()
        {
            
        }

        /// <summary>
        /// ģʽ
        /// </summary>
        private int m_mode;

        /// <summary>
        /// ����
        /// </summary>
        private int m_tick = 1;

        /// <summary>
        /// ����2
        /// </summary>
        private int m_tick2 = 2;

        private float m_b;

        /// <summary>
        /// ��ȡ������ֱ�߲���B
        /// </summary>
        public float B
        {
            get { return m_b; }
            set { m_b = value; }
        }

        private long m_backColor = COLOR.ARGB(255, 0, 0);

        /// <summary>
        /// ��ȡ�����ñ���ɫ
        /// </summary>
        public long BackColor
        {
            get { return m_backColor; }
            set { m_backColor = value; }
        }

        private float m_k;

        /// <summary>
        /// ��ȡ������ֱ�߲���K
        /// </summary>
        public float K
        {
            get { return m_k; }
            set { m_k = value; }
        }

        private POINT m_location = new POINT();

        /// <summary>
        /// ��ȡ������λ��
        /// </summary>
        public POINT Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        private int m_speed = 1;

        /// <summary>
        /// ��ȡ�������ٶ�
        /// </summary>
        public int Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        /// <summary>
        /// �ػ汳������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        public void OnPaintBackground(CPaint paint)
        {
            if (m_k != 0 && m_b != 0)
            {
                RECT drawRect = new RECT(m_location.x - m_tick, m_location.y - m_tick, m_location.x + m_tick, m_location.y + m_tick);
                paint.DrawGradientEllipse(m_backColor, COLOR.RatioColor(paint, m_backColor, 1.1), drawRect, 90);
                if (m_tick2 % 5 == 0)
                {
                    if (m_mode == 0)
                    {
                        m_tick++;
                        if (m_tick > 10)
                        {
                            m_mode = 1;
                        }
                    }
                    else if (m_mode == 1)
                    {
                        m_tick--;
                        if (m_tick < 4)
                        {
                            m_mode = 0;
                        }
                    }
                }
                m_tick2++;
                if (m_tick2 > 1000)
                {
                    m_tick2 = 0;
                }
            }
        }
    }
}

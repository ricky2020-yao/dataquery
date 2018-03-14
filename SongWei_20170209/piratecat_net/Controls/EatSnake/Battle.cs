/*****************************************************************************\
*                                                                             *
* Battle.cs -   Battle functions, types, and definitions                      *
*                                                                             *
*               Version 4.00 ★★★                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 战场
    /// </summary>
    public class Battle : ControlA
    {
        /// <summary>
        /// 创建战场
        /// </summary>
        public Battle()
        {
            m_timerID = GetNewTimerID();
            //初始化颜色
            BorderColor = COLOR.ARGB(112, 112, 112);
            BackColor = COLOR.ARGB(255, 255, 255);
            ForeColor = COLOR.ARGB(255, 255, 255);
            Font = new FONT("Arial", 100, false, false, false);
        }

        /// <summary>
        /// 透明变动方向
        /// </summary>
        private int m_alphaDirection = 0;

        /// <summary>
        /// 透明值
        /// </summary>
        private int m_alpha = 50;

        /// <summary>
        /// 蛇身集合
        /// </summary>
        private List<SnakeBody> m_bodys = new List<SnakeBody>();

        /// <summary>
        /// 吃到食物的数量
        /// </summary>
        private int m_eatCount = 0;

        /// <summary>
        /// 食物
        /// </summary>
        private List<Food> m_foods = new List<Food>();

        /// <summary>
        /// 游戏控制
        /// </summary>
        private Play m_play;

        /// <summary>
        /// 随机数
        /// </summary>
        private Random m_random = new Random();

        /// <summary>
        /// 蛇的长度
        /// </summary>
        private const int SNAKE_LENGTH = 10;

        /// <summary>
        /// 蛇的尺寸
        /// </summary>
        private const int SNAKE_SIZE = 14;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID;

        private int m_speed = 50;

        /// <summary>
        /// 获取或设置速度
        /// </summary>
        public int Speed
        {
            get { return m_speed; }
            set
            {
                m_speed = value;
                StartTimer(m_timerID, value);
            }
        }

        /// <summary>
        /// 添加食物
        /// </summary>
        private void AddFood()
        {
            Food food = new Food();
            food.Size = new SIZE(SNAKE_SIZE, SNAKE_SIZE);
            ControlHost host = Native.Host;
            while (true)
            {
                //获取随机数
                int num = m_random.Next(0, Width / SNAKE_SIZE - 1);
                int num2 = m_random.Next(0, Height / SNAKE_SIZE - 1);
                //设置位置
                food.Location = new POINT(num * SNAKE_SIZE, num2 * SNAKE_SIZE);
                bool reset = false;
                RECT fRect = food.Bounds;
                int bodySize = m_bodys.Count;
                for (int j = 0; j < bodySize; j++)
                {
                    //如果与关节重合，需要重新设置位置
                    RECT tempRect = new RECT();
                    RECT bRect = m_bodys[j].Bounds;
                    if (host.GetIntersectRect(ref tempRect, ref bRect, ref fRect) > 0)
                    {
                        reset = true;
                    }
                }
                int foodSize = m_foods.Count;
                for (int j = 0; j < foodSize; j++)
                {
                    RECT fiRect = m_foods[j].Bounds;
                    RECT tempRect = new RECT();
                    //如果与食物重合，需要重新设置位置
                    if (host.GetIntersectRect(ref tempRect, ref fiRect, ref fRect) > 0)
                    {
                        reset = true;
                    }
                }
                if (!reset)
                {
                    break;
                }
            }
            AddControl(food);
            m_foods.Add(food);
            food.BringToFront();
        }

        /// <summary>
        /// 修改游戏状态
        /// </summary>
        private void ChangeState()
        {
            m_play.BringToFront();
            switch (m_play.GameState)
            {
                //游戏中
                case GameState.Playing:
                    m_play.GameState = GameState.Suspend;
                    break;
                //失败
                case GameState.Lose:
                    ResetSnake();
                    m_play.GameState = GameState.Playing;
                    break;
                //暂停或开始
                case GameState.Suspend:
                case GameState.Begin:
                    m_play.GameState = GameState.Playing;
                    break;
            }
            Focused = true;
            Invalidate();
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                OnRemove();
            }
            base.Dispose();
        }

        /// <summary>
        /// 判断是否失败
        /// </summary>
        /// <returns>是否失败</returns>
        private bool IsLose()
        {
            ControlHost host = Native.Host;
            SnakeBody body = m_bodys[0];
            //蛇头是否撞墙
            if (body.Left < 0 || body.Right > Width || body.Top < 0 || body.Bottom > Width)
            {
                return true;
            }
            //蛇关节间是否碰撞
            int bodySize = m_bodys.Count;
            for (int i = 0; i < bodySize; i++)
            {
                for (int j = i + 1; j < bodySize - i - 1; j++)
                {
                    RECT tempRect = new RECT();
                    RECT iRect = m_bodys[i].Bounds;
                    RECT jRect = m_bodys[j].Bounds;
                    if (host.GetIntersectRect(ref tempRect, ref iRect, ref jRect) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 移动蛇
        /// </summary>
        private void MoveSnake()
        {
            ControlHost host = Native.Host;
            int bodySize = m_bodys.Count;
            SnakeBody lastBody = m_bodys[bodySize - 1];
            int left = lastBody.Left;
            int top = lastBody.Top;
            //改变蛇身每个关节的位置
            for (int i = 0; i < bodySize; i++)
            {
                SnakeBody body = m_bodys[i];
                switch (body.Direction)
                {
                    //向下
                    case SnakeDirection.Down:
                        body.Top += body.Height;
                        break;
                    //向左
                    case SnakeDirection.Left:
                        body.Left -= body.Width;
                        break;
                    //向右
                    case SnakeDirection.Right:
                        body.Left += body.Width;
                        break;
                    //向上
                    case SnakeDirection.Up:
                        body.Top -= body.Height;
                        break;
                }
            }
            //判断游戏失败
            if (IsLose())
            {
                m_play.GameState = GameState.Lose;
                m_play.BringToFront();
            }
            else
            {
                bool newFood = false;
                //判断吃食物
                int foodSize = m_foods.Count;
                for (int i = 0; i < foodSize; i++)
                {
                    Food food = m_foods[i];
                    RECT tempRect = new RECT();
                    RECT bRect = m_bodys[0].Bounds;
                    RECT fRect = food.Bounds;
                    if (host.GetIntersectRect(ref tempRect, ref bRect, ref fRect) > 0)
                    {
                        m_eatCount++;
                        //添加关节
                        RECT rc = new RECT(left, top, left + SNAKE_SIZE, top + SNAKE_SIZE);
                        SnakeBody body = new SnakeBody(rc);
                        m_bodys.Add(body);
                        AddControl(body);
                        body.BringToFront();
                        //移除食物
                        RemoveControl(food);
                        m_foods.Remove(food);
                        newFood = true;
                        break;
                    }
                }
                //改变移动方向
                bodySize = m_bodys.Count;
                for (int i = bodySize - 1; i > 0; i--)
                {
                    m_bodys[i].Direction = m_bodys[i - 1].Direction;
                }
                //设置新的食物
                if (newFood)
                {
                    AddFood();
                }
            }
            Invalidate();
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            //初始化蛇身
            ResetSnake();
        }

        /// <summary>
        /// 按键方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void OnKeyDown(char key)
        {
            base.OnKeyDown(key);
            ControlHost host = Native.Host;
            if (!host.IsKeyPress(0x10) && !host.IsKeyPress(0x11) && !host.IsKeyPress(0x12))
            {
                if (m_play.GameState == GameState.Playing)
                {
                    int bodySize = m_bodys.Count;
                    if (bodySize > 0)
                    {
                        switch ((int)key)
                        {
                            //向左
                            case 37:
                                if (m_bodys[0].Direction != SnakeDirection.Right)
                                    m_bodys[0].Direction = SnakeDirection.Left;
                                break;
                            //向上
                            case 38:
                                if (m_bodys[0].Direction != SnakeDirection.Down)
                                    m_bodys[0].Direction = SnakeDirection.Up;
                                break;
                            //向右
                            case 39:
                                if (m_bodys[0].Direction != SnakeDirection.Left)
                                    m_bodys[0].Direction = SnakeDirection.Right;
                                break;
                            //向下
                            case 40:
                                if (m_bodys[0].Direction != SnakeDirection.Up)
                                    m_bodys[0].Direction = SnakeDirection.Down;
                                break;
                        }
                    }
                }
                //回车
                if (key == 13)
                {
                    ChangeState();
                }
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            if (width > 0 && height > 0)
            {
                //绘制背景
                RECT drawRect = new RECT(0, 0, width, height);
                paint.DrawGradientRect(COLOR.ARGB(200, 90, 120, 24), COLOR.ARGB(200, 122, 156, 40), drawRect, 0, 90);
                if (m_play.GameState != GameState.Begin)
                {
                    //绘制分数
                    FONT font = Font;
                    String strScore = m_eatCount.ToString();
                    SIZE tSize = paint.TextSize(strScore, font);
                    //修正透明度
                    if (m_play.GameState == GameState.Playing)
                    {
                        //减少
                        if (m_alphaDirection == 0)
                        {
                            m_alpha -= 5;
                        }
                        //增加
                        else if (m_alphaDirection == 1)
                        {
                            m_alpha += 5;
                        }
                        //改变变动方向
                        if (m_alpha > 50)
                        {
                            m_alphaDirection = 0;
                            m_alpha = 50;
                        }
                        else if (m_alpha < 20)
                        {
                            m_alphaDirection = 1;
                            m_alpha = 20;
                        }
                    }
                    else
                    {
                        m_alpha = 50;
                    }
                    RECT tRect = new RECT();
                    tRect.left = drawRect.right / 2 - tSize.cx / 2;
                    tRect.top = drawRect.bottom / 2 - tSize.cy / 2;
                    tRect.right = tRect.left + tSize.cx;
                    tRect.bottom = tRect.top + tSize.cy;
                    paint.DrawText(strScore, COLOR.ARGB(m_alpha * 3, 255, 255, 255), font, tRect);
                }
            }
        }

        /// <summary>
        /// 移除控件方法
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();
            StopTimer(m_timerID);
            //移除食物
            int foodSize = m_foods.Count;
            for (int i = 0; i < foodSize; i++)
            {
                RemoveControl(m_foods[i]);
                m_foods[i].Dispose();
            }
            m_foods.Clear();
            //移除开始标签
            if (m_play != null)
            {
                RemoveControl(m_play);
                m_play.Dispose();
                m_play = null;
            }
            //移除蛇身
            int bodySize = m_bodys.Count;
            for (int i = 0; i < bodySize; i++)
            {
                RemoveControl(m_bodys[i]);
                m_bodys[i].Dispose();
            }
            m_bodys.Clear();
        }

        /// <summary>
        /// 重置蛇
        /// </summary>
        private void ResetSnake()
        {
            //重新设定秒表
            OnRemove();
            StartTimer(m_timerID, m_speed);
            m_eatCount = 0;
            //创建开始标签
            m_play = new Play(this);    
            m_play.BackColor = COLOR.EMPTY;
            m_play.Bounds = new RECT(0, 0, Width, Height);
            AddControl(m_play);
            int left = SNAKE_SIZE * 5;
            int top = SNAKE_SIZE * 5;
            //添加蛇关节
            for (int i = 0; i < SNAKE_LENGTH; i++)
            {
                RECT rc = new RECT(left + i * SNAKE_SIZE, top, left + i * SNAKE_SIZE + SNAKE_SIZE, top + SNAKE_SIZE);
                SnakeBody body = new SnakeBody(rc);
                if (i == 0)
                {
                    body.IsHeader = true;
                }
                AddControl(body);
                body.Direction = SnakeDirection.Left;
                m_bodys.Add(body);
                body.BringToFront();
            }
            //添加食物
            AddFood();
            Invalidate();
        }

        /// <summary>
        /// 秒表回调方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void OnTimer(int timerID)
        {
            CallTimerEvents(EVENTID.TIMER, timerID);
            if (m_timerID == timerID)
            {
                if (m_play != null)
                {
                    if (m_play.GameState == GameState.Playing)
                    {
                        MoveSnake();
                    }
                }
            }
        }
    }
}
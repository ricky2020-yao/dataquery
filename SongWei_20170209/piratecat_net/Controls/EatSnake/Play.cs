/*****************************************************************************\
*                                                                             *
* Play.cs -     Play functions, types, and definitions                        *
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
    /// 开始标签
    /// </summary>
    public class Play : ControlA
    {
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="battle">战场</param>
        public Play(Battle battle)
        {
            m_battle = battle;
            BorderColor = COLOR.EMPTY;
            Font = new FONT("SimSun", 16, true, false, true);
            ForeColor = COLOR.ARGB(255, 255, 255);
        }

        /// <summary>
        /// 战场
        /// </summary>
        private Battle m_battle;

        private GameState m_gameState = GameState.Begin;

        /// <summary>
        /// 获取或设置游戏状态
        /// </summary>
        public GameState GameState
        {
            get { return m_gameState; }
            set
            {
                m_gameState = value;
                if (m_gameState == GameState.Playing)
                {
                    Visible = false;
                }
                else
                {
                    Visible = true;
                }
            }
        }

        /// <summary>
        /// 键盘方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void OnKeyDown(char key)
        {
            base.OnKeyDown(key);
            if (m_battle != null)
            {
                m_battle.OnKeyDown(key);
            }
        }

        /// <summary>
        /// 鼠标按下方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        public override void OnMouseDown(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseDown(mp, button, clicks, delta);
            if (m_battle != null)
            {
                m_battle.OnMouseDown(mp, button, clicks, delta);
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
                String text = null;
                switch (m_gameState)
                {
                    //失败
                    case GameState.Lose:
                        text = "游戏失败，按回车重新开始";
                        break;
                    //暂停
                    case GameState.Suspend:
                        text = "游戏暂停，按回车回到游戏";
                        break;
                    //开始
                    case GameState.Begin:
                        text = "按回车开始游戏";
                        break;
                }
                //绘制文字
                if (text != null)
                {
                    FONT font = Font;
                    SIZE textSize = paint.TextSize(text, font);
                    RECT tRect = new RECT();
                    tRect.left = width / 2 - textSize.cx / 2;
                    tRect.top = height / 2 - textSize.cy / 2;
                    tRect.right = tRect.left + textSize.cx;
                    tRect.bottom = tRect.top + textSize.cy;
                    paint.DrawText(text, GetPaintingForeColor(), font, tRect);
                }
            }
        }
    }
}
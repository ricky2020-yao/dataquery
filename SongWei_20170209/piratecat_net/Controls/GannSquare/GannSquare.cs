/*****************************************************************************\
*                                                                             *
* OwGannSquare.cs -   OwGannSquare functions, types, and definitions          *
*                                                                             *
*               Version 4.00 ★★★                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Square. All rights reserved.  *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 江恩正方 
    /// </summary>
    public partial class GannSquare : ControlA
    {
        #region Lord 2016/8/23
        /// <summary>
        /// 创建江恩正方　
        /// </summary>
        public GannSquare()
        {
            BackColor = COLOR.ARGB(255, 255, 255);
            SIZE size = new SIZE(200, 200);
            Size = size;
        }

        private BolsterOrStress m_bolsterOrStress = BolsterOrStress.Bolster;

        /// <summary>
        /// 获取或设置支撑位或压力位
        /// </summary>
        public BolsterOrStress BolsterOrStress
        {
            get { return m_bolsterOrStress; }
            set 
            {
                m_bolsterOrStress = value;
            }
        }

        private FONT m_cellFont = new FONT("Arial", 14, false, false, false);

        /// <summary>
        /// 单元格的字体
        /// </summary>
        public FONT CellFont
        {
            get { return m_cellFont; }
            set { m_cellFont = value; }
        }

        private List<SquareCell> m_cells = new List<SquareCell>();

        /// <summary>
        /// 获取或设置单元格
        /// </summary>
        internal List<SquareCell> Cells
        {
            get { return m_cells; }
            set { m_cells = value; }
        }

        private SIZE m_cellSize = new SIZE(40, 20);

        /// <summary>
        /// 获取或设置单元格的大小
        /// </summary>
        public SIZE CellSize
        {
            get { return m_cellSize; }
            set
            { 
                if (value.cx > 0 && value.cy > 0)
                {
                    m_cellSize = value;
                }
            }
        }

        private int m_digit = 2;

        /// <summary>
        /// 获取或设置保留小数的位数
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private long m_majorColor = COLOR.ARGB(255, 0, 0);

        /// <summary>
        /// 获取或设置主要颜色
        /// </summary>
        public long MajorColor
        {
            get { return m_majorColor; }
            set { m_majorColor = value; }
        }

        private long m_minorColor = COLOR.ARGB(255, 255, 0);

        /// <summary>
        /// 获取或设置次要颜色
        /// </summary>
        public long MinorColor
        {
            get { return m_minorColor; }
            set { m_minorColor = value; }
        }

        private long m_normalColor = COLOR.ARGB(240, 240, 240);

        /// <summary>
        /// 获取或设置普通颜色
        /// </summary>
        public long NormalColor
        {
            get { return m_normalColor; }
            set { m_normalColor = value; }
        }

        private int m_number = 9;

        /// <summary>
        /// 获取或设置数量
        /// </summary>
        public int Number
        {
            get { return m_number; }
            set
            {
                if (value > 0)
                {
                    if (value % 2 == 0)
                    {
                        value += 1;
                    }
                    m_number = value;
                }
            }
        }

        private long m_selectedColor = COLOR.ARGB(0, 255, 255);

        /// <summary>
        /// 获取或设置选中的颜色
        /// </summary>
        public long SelectedColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
        }

        /// <summary>
        /// 获取选中值
        /// </summary>
        public double SelectedValue
        {
            get
            {
                int cellSize = m_cells.Count;
                for (int i = 0; i < cellSize; i++)
                {
                    if (m_cells[i].Selected)
                    {
                        return m_cells[i].Value;
                    }
                }
                return m_startValue;
            }
        }

        private double m_startValue = 0;

        /// <summary>
        /// 获取或设置起始值
        /// </summary>
        public double StartValue
        {
            get { return m_startValue; }
            set { m_startValue = value; }
        }

        private SquareStyle m_style = SquareStyle.Normal;

        /// <summary>
        /// 获取或设置正方样式
        /// </summary>
        public SquareStyle Style
        {
            get { return m_style; }
            set 
            {
                m_style = value;
            }
        }

        private String m_suffix = "";

        /// <summary>
        /// 获取或设置后缀
        /// </summary>
        public String Suffix
        {
            get { return m_suffix; }
            set { m_suffix = value; }
        }

        private double m_unit = 0.1;

        /// <summary>
        /// 获取或设置单位值
        /// </summary>
        public double Unit
        {
            get { return m_unit; }
            set { m_unit = value; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_cells.Clear();
            }
            base.Dispose();
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void GetProperty(String name, ref String value, ref String type)
        {
            if (name == "bolsterorstress")
            {
                type = "enum:BolsterOrStress";
                BolsterOrStress bolsterOrStess = BolsterOrStress;
                if (bolsterOrStess == BolsterOrStress.Bolster)
                {
                    value = "bolster";
                }
                else if (bolsterOrStess == BolsterOrStress.Stress)
                {
                    value = "stress";
                }
            }
            else if (name == "cellfont")
            {
                type = "font";
                value = CStr.ConvertFontToStr(CellFont);
            }
            else if (name == "cellsize")
            {
                type = "size";
                value = CStr.ConvertSizeToStr(CellSize);
            }
            else if (name == "digit")
            {
                type = "int";
                value = CStr.ConvertIntToStr(Digit);
            }
            else if (name == "majorcolor")
            {
                type = "color";
                value = CStr.ConvertColorToStr(MajorColor);
            }
            else if (name == "minorcolor")
            {
                type = "color";
                value = CStr.ConvertColorToStr(MinorColor);
            }
            else if (name == "normalcolor")
            {
                type = "color";
                value = CStr.ConvertColorToStr(NormalColor);
            }
            else if (name == "number")
            {
                type = "int";
                value = CStr.ConvertIntToStr(Number);
            }
            else if (name == "selectedcolor")
            {
                type = "color";
                value = CStr.ConvertColorToStr(SelectedColor);
            }
            else if (name == "startvalue")
            {
                type = "double";
                value = CStr.ConvertDoubleToStr(StartValue);
            }
            else if (name == "style")
            {
                type = "enum:SquareStyle";
                SquareStyle style = Style;
                if (style == SquareStyle.Normal)
                {
                    value = "normal";
                }
                else if (style == SquareStyle.Spiral)
                {
                    value = "spiral";
                }
            }
            else if (name == "suffix")
            {
                type = "text";
                value = Suffix;
            }
            else if (name == "unit")
            {
                type = "double";
                value = CStr.ConvertDoubleToStr(Unit);
            }
            else
            {
                base.GetProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public override List<String> GetPropertyNames()
        {
            List<String> propertyNames = base.GetPropertyNames();
            propertyNames.AddRange(new String[] { "BolsterOrStress", "CellFont", "CellSize", "Digit",
            "MajorColor", "MinorColor", "NormalColor", "Number", "SelectedColor", "StartValue",
            "Style", "Suffix", "Unit"});
            return propertyNames;
        }

        /// <summary>
        /// 点击方法
        /// </summary>
        /// <param name="mp">鼠标坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public override void OnClick(POINT mp, MouseButtonsA button, int clicks,int delta)
        {
            base.OnClick(mp, button, clicks, delta);
            int cellSize = m_cells.Count;
            for (int i = 0; i < cellSize; i++)
            {
                SquareCell cell = m_cells[i];
                if (mp.x >= cell.Bounds.left && mp.x <= cell.Bounds.right
                    && mp.y >= cell.Bounds.top && mp.y <= cell.Bounds.bottom)
                {
                    cell.Selected = true;
                }
                else
                {
                    cell.Selected = false;
                }
            }
            Invalidate();
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            ReCreate();
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            RECT rect = new RECT(0, 0, Width, Height);
            paint.FillRect(GetPaintingBackColor(), rect);
            int cellSize = m_cells.Count;
            for (int i = 0; i < cellSize; i++)
            {
                m_cells[i].OnPaint(paint);
            }
            int left = (Width - m_number * CellSize.cx) / 2;
            int top = (Height - m_number * CellSize.cy) / 2;
            int w = m_number * CellSize.cx;
            int h = m_number * CellSize.cy;
            paint.DrawLine(COLOR.ARGB(167, 170, 178), 1, 0, left, top, left, top + h);
            paint.DrawLine(COLOR.ARGB(167, 170, 178), 1, 0, left, top + h, left + w, top + h);
        }

        /// <summary>
        /// 重新创建
        /// </summary>
        public void ReCreate()
        {
            if (Native != null)
            {
                //清除单元格
                m_cells.Clear();
                //创建单元格
                for (int i = 1; i <= m_number * m_number; i++)
                {
                    SquareCell cell = new SquareCell(this);
                    cell.Number = i;
                    m_cells.Add(cell);
                }
                Update();
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void SetProperty(String name, String value)
        {
            if (name == "bolsterorstress")
            {
                String str = value.ToLower();
                if (value == "bolster")
                {
                    BolsterOrStress = BolsterOrStress.Bolster;
                }
                else if (value == "stress")
                {
                    BolsterOrStress = BolsterOrStress.Stress;
                }
            }
            else if (name == "cellfont")
            {
                CellFont = CStr.ConvertStrToFont(value);
            }
            else if (name == "cellsize")
            {
                CellSize = CStr.ConvertStrToSize(value);
            }
            else if (name == "digit")
            {
                Digit = CStr.ConvertStrToInt(value);
            }
            else if (name == "majorcolor")
            {
                MajorColor = CStr.ConvertStrToColor(value);
            }
            else if (name == "minorcolor")
            {
                MinorColor = CStr.ConvertStrToColor(value);
            }
            else if (name == "normalcolor")
            {
                NormalColor = CStr.ConvertStrToColor(value);
            }
            else if (name == "number")
            {
                Number = CStr.ConvertStrToInt(value);
            }
            else if (name == "selectedcolor")
            {
                SelectedColor = CStr.ConvertStrToColor(value);
            }
            else if (name == "startvalue")
            {
                StartValue = CStr.ConvertStrToDouble(value);
            }
            else if (name == "style")
            {
                String str = value.ToLower();
                if (str == "normal")
                {
                    Style = SquareStyle.Normal;
                }
                else if (str == "spiral")
                {
                    Style = SquareStyle.Spiral;
                }
            }
            else if (name == "suffix")
            {
                Suffix = value;
            }
            else if (name == "unit")
            {
                Unit = CStr.ConvertStrToDouble(value);
            }
            else
            {
                base.SetProperty(name, value);
            }
        }

        /// <summary>
        /// 重置布局
        /// </summary>
        public override void Update()
        {
            if (Native != null)
            {
                //中值
                int mid = m_number / 2 + 1;
                int midnum = m_number * m_number / 2 + 1;
                RECT bounds = Bounds;
                int width = bounds.right - bounds.left;
                int height = bounds.bottom - bounds.top;
                //左侧及上侧的坐标
                int left = (width - m_number * m_cellSize.cx) / 2;
                int top = (height - m_number * m_cellSize.cy) / 2;
                int num = 0;
                //方向状态
                int state = 0;
                //获取特殊旋转方向
                List<int> list = new List<int>();
                int cellSize = m_cells.Count;
                if (m_style == SquareStyle.Spiral)
                {
                    if (cellSize > 2)
                    {
                        int n = 2;
                        while (n <= cellSize)
                        {
                            list.Add(n);
                            int listSize = list.Count;
                            int last = listSize > 1 ? list[listSize - 2] : 2;
                            n += list[listSize - 1] - last + 8;
                        }
                    }
                }
                //循环操作
                for (int i = 0; i < cellSize; i++)
                {
                    SquareCell cell = m_cells[i];
                    SIZE cSize = m_cellSize;
                    //螺旋
                    if (m_style == SquareStyle.Spiral)
                    {
                        //修正
                        if (i == 1)
                        {
                            state = 0;
                        }
                        else if (list.Contains(i))
                        {
                            state = 1;
                        }
                        //初始化
                        if (i == 0)
                        {
                            num = midnum;
                        }
                        else
                        {
                            switch (state)
                            {
                                //向左
                                case 0:
                                    num = num - m_number;
                                    break;
                                //向上
                                case 1:
                                    num = num + 1;
                                    break;
                                //向右
                                case 2:
                                    num = num + m_number;
                                    break;
                                //向下
                                case 3:
                                    num = num - 1;
                                    break;
                            }
                        }

                    }
                    //普通
                    else
                    {
                        num = cell.Number;
                    }
                    //获取列号和行号
                    int cindex = (num - 1) / m_number;
                    int rindex = m_number - (num % m_number == 0 ? m_number : num % m_number);
                    //获取横坐标和纵坐标
                    POINT cLocation = new POINT(left + m_cellSize.cx * cindex, top + m_cellSize.cy * rindex);
                    cell.Bounds = new RECT(cLocation.x, cLocation.y, cLocation.x + cSize.cx, cLocation.y + cSize.cy);
                    //主要
                    if (num == cindex * (m_number + 1) + 1 || num == cindex * (m_number - 1) + m_number)
                    {
                        cell.Style = SquareCellStyle.Major;
                        //螺旋
                        if (m_style == SquareStyle.Spiral)
                        {
                            if (num == cindex * (m_number + 1) + 1)
                            {
                                if (num >= midnum)
                                {
                                    state = 3;
                                }
                            }
                            else if (num == cindex * (m_number - 1) + m_number)
                            {
                                if (num < midnum)
                                {
                                    state = 2;
                                }
                                else
                                {
                                    state = 0;
                                }
                            }
                        }
                    }
                    //次要
                    else if ((num - mid) % m_number == 0 || (num >= (mid - 1) * m_number + 1 && num <= mid * m_number))
                    {
                        cell.Style = SquareCellStyle.Minor;
                    }
                }
            }
        }
        #endregion
    }
}

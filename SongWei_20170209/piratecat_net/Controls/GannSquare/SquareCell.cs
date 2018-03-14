/*****************************************************************************\
*                                                                             *
* SquareCell.cs -   SquareCell functions, types, and definitions              *
*                                                                             *
*               Version 4.00 ����                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Square. All rights reserved.  *
*                                                                             *
*******************************************************************************/

using System;

namespace OwLib
{
    /// <summary>
    /// ������Ԫ��
    /// </summary>
    public class SquareCell
    {
        #region Lord 2016/8/24
        /// <summary>
        /// ������Ԫ��
        /// </summary>
        /// <param name="gannSquare">����</param>
        public SquareCell(GannSquare gannSquare):base()
        {
            m_gannSquare = gannSquare;
        }

        /// <summary>
        /// ��ȡ����ɫ
        /// </summary>
        public long BackColor
        {
            get
            {
                if (m_selected)
                {
                    return m_gannSquare.SelectedColor;
                }
                else
                {
                    if (m_style == SquareCellStyle.Major)
                    {
                        return m_gannSquare.MajorColor;
                    }
                    else if (m_style == SquareCellStyle.Minor)
                    {
                        return m_gannSquare.MinorColor;
                    }
                    else
                    {
                        return m_gannSquare.NormalColor;
                    }
                }
            }
        }

        private RECT m_bounds;

        /// <summary>
        /// ��ȡ��������ʾ�ľ���
        /// </summary>
        public RECT Bounds
        {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        /// <summary>
        /// ��ȡ�����ñ���ɫ
        /// </summary>
        public long ForeColor
        {
            get { return m_gannSquare.ForeColor; }
        }

        private GannSquare m_gannSquare;

        /// <summary>
        /// ��ȡ�����ý�������
        /// </summary>
        public GannSquare GannSquare
        {
            get { return m_gannSquare; }
        }

        private int m_number;

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public int Number
        {
            get { return m_number; }
            set { m_number = value; }
        }

        private bool m_selected;

        /// <summary>
        /// ��ȡ�������Ƿ�ѡ��
        /// </summary>
        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        private SquareCellStyle m_style = SquareCellStyle.Normal;

        /// <summary>
        /// ��ȡ��������ʽ
        /// </summary>
        public SquareCellStyle Style
        {
            get { return m_style; }
            set { m_style = value; }
        }

        /// <summary>
        /// ��ȡ������ֵ
        /// </summary>
        public double Value
        {
            get
            {
                if (m_gannSquare.BolsterOrStress == BolsterOrStress.Bolster)
                {
                    return m_gannSquare.StartValue - m_gannSquare.Unit * (m_number - 1);
                }
                else
                {
                    return m_gannSquare.StartValue + m_gannSquare.Unit * (m_number + 1);
                }
            }
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        public void OnPaint(CPaint paint)
        {
            RECT rect = Bounds;
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            if ((rect.left >= 0 || rect.top >= 0 || rect.right >= 0 || rect.bottom >= 0) && width > 0 && height > 0)
            {
                //���Ʊ���
                paint.FillRect(BackColor, rect);
                String text = CStr.GetValueByDigit(Value, m_gannSquare.Digit);
                String suffix = m_gannSquare.Suffix;
                if (suffix != null && suffix.Length > 0)
                {
                    text += suffix;
                }
                SIZE textSize = paint.TextSize(text, m_gannSquare.CellFont);
                //������
                POINT tPoint = new POINT(rect.left + (width - textSize.cx) / 2,
                rect.top + (height - textSize.cy) / 2);
                RECT tRect = new RECT(tPoint.x, tPoint.y, tPoint.x + textSize.cx, tPoint.y + textSize.cy);
                paint.DrawText(text, ForeColor, m_gannSquare.CellFont, tRect);
                //���Ʊ���
                paint.DrawLine(COLOR.ARGB(167, 170, 178), 0, 0, rect.left,rect.top, rect.right, rect.top);
                paint.DrawLine(COLOR.ARGB(167, 170, 178), 0, 0,
                    rect.right, rect.top, rect.right, rect.bottom);
            }
        }
        #endregion
    }
}

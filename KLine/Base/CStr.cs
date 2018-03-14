/*****************************************************************************\
*                                                                             *
* Str.cs -    Str functions, types, and definitions                           *
*                                                                             *
*               Version 1.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by Lord.                                              *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KLine
{
    public class CStrA
    {
        /// <summary>
        /// ��ȡ֤ȯ���ļ�����
        /// </summary>
        /// <param name="code">����</param>
        /// <returns>�ļ�����</returns>
        public static String ConvertDBCodeToFileName(String code)
        {
            String fileName = code;
            if (fileName.IndexOf(".") != -1)
            {
                fileName = fileName.Substring(fileName.IndexOf('.') + 1) + fileName.Substring(0, fileName.IndexOf('.'));
            }
            fileName += ".txt";
            return fileName;
        }

        /// <summary>
        /// Doubleת����ʱ��
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static DateTime ConvertNumToDate(double num)
        {
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisecond = 0;
            CMathLib.M130(num, ref year, ref month, ref day, ref hour, ref minute, ref second, ref millisecond);
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// ʱ��ת����Double
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertDateToNum(DateTime date)
        {
            return CMathLib.M129(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        /// <summary>
        /// �ַ���ת��Ϊ������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>��ֵ</returns>
        public static double ConvertStrToDouble(String str)
        {
            double value = 0;
            double.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// �ַ���ת��Ϊ����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>��ֵ</returns>
        public static int ConvertStrToInt(String str)
        {
            int value = 0;
            int.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// ��ȡ���ݿ�ת���ַ���
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>ת���ַ���</returns>
        public static String GetDBString(String str)
        {
            return str.Replace("'", "''");
        }

        /// <summary>
        /// ��ȡGuid
        /// </summary>
        /// <returns></returns>
        public static String GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

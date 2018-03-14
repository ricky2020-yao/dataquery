/*************************************************************************************\
*                                                                                     *
* UserSecurityServiceEx.cs - User security service functions, types, and definitions. *
*                                                                                     *
*               Version 1.00  ★★★                                                  *
*                                                                                     *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.              *
*               Created by Lord 2016/1/12.                                            *
*                                                                                     *
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;
using System.IO;

namespace piratecat
{
    /// <summary>
    /// 自选股服务扩展
    /// </summary>
    public class UserSecurityServiceEx:UserSecurityService
    {
        #region Lord 2016/1/12
        /// <summary>
        /// 创建自选股服务扩展
        /// </summary>
        public UserSecurityServiceEx()
        {
        }

        /// <summary>
        /// 自选股类别
        /// </summary>
        private List<UserSecurityCategory> m_categories = new List<UserSecurityCategory>();

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取列表的请求ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// 获取或设置是否已加载
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取操作请求ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        private Dictionary<String, int> m_visitsCount = new Dictionary<String, int>();

        /// <summary>
        /// 获取或设置访问次数
        /// </summary>
        public Dictionary<String, int> VisitsCount
        {
            get { return m_visitsCount; }
        }

        /// <summary>
        /// 添加访问次数
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>状态</returns>
        public int AddVisitsCount(String code)
        {
            UserSecurityVisitsCount visitsCount = new UserSecurityVisitsCount();
            visitsCount.m_userID = DataCenter.UserID;
            visitsCount.m_codes[code] = 1;
            AddVisitsCount(0, visitsCount);
            if (m_visitsCount.ContainsKey(code))
            {
                m_visitsCount[code] = m_visitsCount[code] + 1;
            }
            else
            {
                m_visitsCount[code] = 1;
            }
            return 1;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_categories.Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// 获取类别信息
        /// </summary>
        /// <param name="categoryID">类别ID</param>
        /// <param name="category">返回类别</param>
        /// <returns>状态</returns>
        public bool GetCategory(String categoryID, ref UserSecurityCategory category)
        {
            int categoriesSize = m_categories.Count;
            for (int i = 0; i < categoriesSize; i++)
            {
                if (m_categories[i].m_categoryID == categoryID)
                {
                    category = m_categories[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <param name="categories">类别列表</param>
        /// <returns>状态</returns>
        public bool GetCategories(List<UserSecurityCategory> categories)
        {
            int categoriesSize = m_categories.Count;
            for (int i = 0; i < categoriesSize; i++)
            {
                categories.Add(m_categories[i]);
            }
            return true;
        }

        /// <summary>
        /// 接收消息方法
        /// </summary>
        /// <param name="message">消息</param>
        public override void OnReceive(CMessage message)
        {
            if (message.m_functionID == FUNCTIONID_USERSECURITY_GETVISITSCOUNT)
            {
                UserSecurityVisitsCount visitsCount = new UserSecurityVisitsCount();
                Binary br = new Binary();
                br.Write(message.m_body, message.m_bodyLength);
                visitsCount.m_userID = br.ReadInt();
                visitsCount.CodesFromString(br.ReadString());
                m_visitsCount = visitsCount.m_codes;
                br.Close();
            }
            else
            {
                List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
                UserSecurityService.GetCategories(categories, message.m_body, message.m_bodyLength);
                int categoriesSize = categories.Count;
                if (message.m_functionID != FUNCTIONID_USERSECURITY_GETCATEGORIES)
                {
                    message.m_requestID = m_operatorRequestID;
                }
                switch (message.m_functionID)
                {
                    case FUNCTIONID_USERSECURITY_GETCATEGORIES:
                        {
                            m_categories = categories;
                            m_loaded = true;
                            break;
                        }
                    case FUNCTIONID_USERSECURITY_ADDCATEGORIES:
                        {
                            bool add = false;
                            for (int i = 0; i < categoriesSize; i++)
                            {
                                UserSecurityCategory category = null;
                                if (!GetCategory(categories[i].m_categoryID, ref category))
                                {
                                    m_categories.Add(categories[i]);
                                    add = true;
                                }
                            }
                            if (!add)
                            {
                                return;
                            }
                            break;
                        }
                    case FUNCTIONID_USERSECURITY_DELETECATEGORIES:
                        {
                            for (int i = 0; i < categoriesSize; i++)
                            {
                                UserSecurityCategory category = null;
                                if (GetCategory(categories[i].m_categoryID, ref category))
                                {
                                    m_categories.Remove(category);
                                }
                            }
                            break;
                        }
                    case FUNCTIONID_USERSECURITY_UPDATECATEGORIES:
                        {
                            for (int i = 0; i < categoriesSize; i++)
                            {
                                UserSecurityCategory updateCategory = categories[i];
                                int curCategoriesSize = m_categories.Count;
                                for (int j = 0; j < curCategoriesSize; j++)
                                {
                                    UserSecurityCategory category = m_categories[j];
                                    if (category.m_categoryID == updateCategory.m_categoryID)
                                    {
                                        m_categories[j] = updateCategory;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case FUNCTIONID_USERSECURITY_ADDSECURITIES:
                        {
                            for (int i = 0; i < categoriesSize; i++)
                            {
                                UserSecurityCategory updateCategory = categories[i];
                                int curCategoriesSize = m_categories.Count;
                                for (int j = 0; j < curCategoriesSize; j++)
                                {
                                    UserSecurityCategory category = m_categories[j];
                                    if (category.m_categoryID == updateCategory.m_categoryID)
                                    {
                                        if (category.m_codes != null && category.m_codes.Length > 0)
                                        {
                                            updateCategory.m_codes = m_categories[j].m_codes + "," + updateCategory.m_codes;
                                        }
                                        m_categories[j] = updateCategory;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case FUNCTIONID_USERSECURITY_DELETESECURITIES:
                        {
                            for (int i = 0; i < categoriesSize; i++)
                            {
                                UserSecurityCategory updateCategory = categories[i];
                                int curCategoriesSize = m_categories.Count;
                                for (int j = 0; j < curCategoriesSize; j++)
                                {
                                    UserSecurityCategory category = m_categories[j];
                                    if (category.m_categoryID == updateCategory.m_categoryID)
                                    {
                                        if (updateCategory.m_codes != null && updateCategory.m_codes.Length > 0 && category.m_codes != null && category.m_codes.Length > 0)
                                        {
                                            String[] deleteCodes = updateCategory.m_codes.Split(',');
                                            int deleteCodesSize = deleteCodes.Length;
                                            Dictionary<String, String> deleteCodesMap = new Dictionary<String, String>();
                                            for (int m = 0; m < deleteCodesSize; m++)
                                            {
                                                deleteCodesMap[deleteCodes[m]] = "";
                                            }
                                            String[] codes = category.m_codes.Split(',');
                                            int codesSize = codes.Length;
                                            String newCodes = "";
                                            for (int m = 0; m < codesSize; m++)
                                            {
                                                if (!deleteCodesMap.ContainsKey(codes[m]))
                                                {
                                                    newCodes += codes[m] + ",";
                                                }
                                            }
                                            if (newCodes != null && newCodes.Length > 0)
                                            {
                                                if (newCodes.Substring(newCodes.Length - 1) == ",")
                                                {
                                                    newCodes = newCodes.Substring(0, newCodes.Length - 1);
                                                }
                                            }
                                            updateCategory.m_codes = newCodes;
                                        }
                                        m_categories[j] = updateCategory;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            base.OnReceive(message);
        }
        #endregion
    }
}

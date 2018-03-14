/*****************************************************************************\
*                                                                             *
* ChatServiceEx.cs -  Chart service functions, types, and definitions.        *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/20.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// 聊天服务的扩展
    /// </summary>
    public class ChatServiceEx : ChatService
    {
        #region Lord 2016/1/20
        /// <summary>
        /// 创建指标服务扩展
        /// </summary>
        public ChatServiceEx()
        {
        }

        private int m_requestID = BaseService.GetRequestID();

        /// <summary>
        /// 获取请求ID
        /// </summary>
        public int RequestID
        {
            get { return m_requestID; }
        }
        #endregion
    }
}

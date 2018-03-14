/*******************************************************************************************\
*                                                                                           *
* SecurityFilterServiceEx.cs - Security filter service functions, types, and definitions.   *
*                                                                                           *
*               Version 1.00  ����                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/1/22.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ����ѡ�ɷ���
    /// </summary>
    public class SecurityFilterServiceEx:SecurityFilterService
    {
        #region Lord 2016/1/22
        /// <summary>
        /// ��������ѡ�ɷ���
        /// </summary>
        public SecurityFilterServiceEx()
        {
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private List<SecurityFilterTask> m_tasks = new List<SecurityFilterTask>();

        /// <summary>
        /// ģ���б�
        /// </summary>
        private List<SecurityFilterTemplate> m_templates = new List<SecurityFilterTemplate>();

        private int m_getListRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ�б������ID
        /// </summary>
        public int GetListRequestID
        {
            get { return m_getListRequestID; }
        }

        private bool m_loaded;

        /// <summary>
        /// ��ȡ�������Ƿ��Ѽ���
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        private int m_operatorRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ȡ��������ID
        /// </summary>
        public int OperatorRequestID
        {
            get { return m_operatorRequestID; }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="template">ģ��</param>
        /// <param name="native">������</param>
        /// <returns>����</returns>
        public SecurityFilterTask AddTask(SecurityFilterTemplate template, INativeBase native)
        {
            int tasksSize = m_tasks.Count;
            for (int i = 0; i < tasksSize; i++)
            {
                if (m_tasks[i].Template.m_templateID == template.m_templateID)
                {
                    return null;
                }
            }
            SecurityFilterTask task = new SecurityFilterTask();
            task.Native = native;
            task.Template = template;
            m_tasks.Add(task);
            return task;
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_templates.Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="templateID">ģ��ID</param>
        /// <param name="task">����</param>
        /// <returns>����</returns>
        public SecurityFilterTask GetTask(String templateID)
        {
            int tasksSize = m_tasks.Count;
            for (int i = 0; i < tasksSize; i++)
            {
                if (m_tasks[i].Template.m_templateID == templateID)
                {
                    return m_tasks[i];
                }
            }
            return null;
        }

        /// <summary>
        /// ��ȡģ����Ϣ
        /// </summary>
        /// <param name="categoryID">ģ��ID</param>
        /// <param name="template">����ģ��</param>
        /// <returns>״̬</returns>
        public bool GetTemplate(String templateID, ref SecurityFilterTemplate template)
        {
            int templatesSize = m_templates.Count;
            for (int i = 0; i < templatesSize; i++)
            {
                if (m_templates[i].m_templateID == templateID)
                {
                    template = m_templates[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡ����ģ��
        /// </summary>
        /// <param name="templates">ģ���б�</param>
        /// <returns>״̬</returns>
        public bool GetTemplates(List<SecurityFilterTemplate> templates)
        {
            int templatesSize = m_templates.Count;
            for (int i = 0; i < templatesSize; i++)
            {
                templates.Add(m_templates[i]);
            }
            return true;
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            if (message.m_functionID != SecurityFilterService.FUNCTIONID_SECURITYFILTER_STARTFILTER
                && message.m_functionID != SecurityFilterService.FUNCTIONID_SECURITYFILTER_STOPFILTER)
            {
                if (message.m_functionID != FUNCTIONID_SECURITYFILTER_GETTEMPLATES)
                {
                    message.m_requestID = m_operatorRequestID;
                }
                List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
                SecurityFilterService.GetTemplates(templates, message.m_body, message.m_bodyLength);
                int templatesSize = templates.Count;
                switch (message.m_functionID)
                {
                    case FUNCTIONID_SECURITYFILTER_GETTEMPLATES:
                        {
                            m_templates = templates;
                            m_loaded = true;
                            break;
                        }
                    case FUNCTIONID_SECURITYFILTER_ADDTEMPLATES:
                        {
                            bool add = false;
                            for (int i = 0; i < templatesSize; i++)
                            {
                                SecurityFilterTemplate template = null;
                                if (!GetTemplate(templates[i].m_templateID, ref template))
                                {
                                    m_templates.Add(templates[i]);
                                    add = true;
                                }
                            }
                            if (!add)
                            {
                                return;
                            }
                            break;
                        }
                    case FUNCTIONID_SECURITYFILTER_DELETETEMPLATES:
                        {
                            for (int i = 0; i < templatesSize; i++)
                            {
                                SecurityFilterTemplate template = null;
                                if (GetTemplate(templates[i].m_templateID, ref template))
                                {
                                    m_templates.Remove(template);
                                }
                            }
                            break;
                        }
                    case FUNCTIONID_SECURITYFILTER_UPDATETEMPLATES:
                        {
                            for (int i = 0; i < templatesSize; i++)
                            {
                                SecurityFilterTemplate updateTemplate = templates[i];
                                int curTemplatesSize = m_templates.Count;
                                for (int j = 0; j < curTemplatesSize; j++)
                                {
                                    SecurityFilterTemplate sfTemplate = m_templates[j];
                                    if (sfTemplate.m_templateID == updateTemplate.m_templateID)
                                    {
                                        m_templates[j] = updateTemplate;
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

        /// <summary>
        /// �Ƴ�����
        /// </summary>
        /// <param name="templateID">ģ��ID</param>
        public bool RemoveTask(String templateID)
        {
            int tasksSize = m_tasks.Count;
            for (int i = 0; i < tasksSize; i++)
            {
                if (m_tasks[i].Template.m_templateID == templateID)
                {
                    m_tasks.Remove(m_tasks[i]);
                    m_tasks[i].Dispose();
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}

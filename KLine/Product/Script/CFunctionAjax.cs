using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;

namespace KLine
{
    /// <summary>
    /// Ajax������Ϣ
    /// </summary>
    public class AjaxRequest
    {
        /// <summary>
        /// ��������
        /// </summary>
        public AjaxRequest()
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="indicator">ָ��</param>
        /// <param name="type">����</param>
        /// <param name="url">��ַ</param>
        /// <param name="dataType">��������</param>
        /// <param name="callBack">�ص�</param>
        public AjaxRequest(CIndicator indicator, String type, String url, String dataType, String callBack)
        {
            m_indicator = indicator;
            m_url = url;
            m_type = type;
            m_dataType = dataType;
            m_callBack = callBack;
        }

        /// <summary>
        /// �ص�����
        /// </summary>
        public String m_callBack = "";

        /// <summary>
        /// ��������
        /// </summary>
        public String m_dataType = "";

        /// <summary>
        /// ָ��
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// ����
        /// </summary>
        public String m_type = "";

        /// <summary>
        /// ��ַ
        /// </summary>
        public String m_url = "";
    }
    
    /// <summary>
    /// Ajax��Ӧ��Ϣ
    /// </summary>
    public class AjaxResponse
    {
        /// <summary>
        /// ������Ӧ
        /// </summary>
        public AjaxResponse()
        {
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="indicator">ָ��</param>
        /// <param name="type">����</param>
        /// <param name="url">��ַ</param>
        /// <param name="dataType">��������</param>
        /// <param name="callBack">�ص�</param>
        /// <param name="resStr">�ص��ַ���</param>
        public AjaxResponse(CIndicator indicator, String type, String url, String dataType, String callBack, String resStr)
        {
            m_indicator = indicator;
            m_url = url;
            m_type = type;
            m_dataType = dataType;
            m_callBack = callBack;
            m_resStr = resStr;
        }

        /// <summary>
        /// �ص�����
        /// </summary>
        public String m_callBack = "";

        /// <summary>
        /// ��������
        /// </summary>
        public String m_dataType = "";

        /// <summary>
        /// ָ��
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// ��Ӧ�ַ���
        /// </summary>
        public String m_resStr = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_type = "";

        /// <summary>
        /// ��ַ
        /// </summary>
        public String m_url = "";
    }

    /// <summary>
    /// Ajax��
    /// </summary>
    public class CFunctionAjax : CFunction
    {
         /// <summary>
        /// ��������
        /// </summary>
        /// <param name="indicator">ָ��</param>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        public CFunctionAjax(CIndicator indicator, int id, String name)
        {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// ָ��
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// ���ÿؼ�
        /// </summary>
        private static ControlA m_listener;

        /// <summary>
        /// AJAX����
        /// </summary>
        public static List<AjaxRequest> m_requests;

        /// <summary>
        /// ����
        /// </summary>
        private static string FUNCTIONS = "AJAX";

        /// <summary>
        /// ǰ׺
        /// </summary>
        private static string PREFIX = "$.";

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private const int STARTINDEX = 30000;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
        public override double OnCalculate(CVariable var)
        {
            switch (var.m_functionID)
            {
                case STARTINDEX + 0:
                    return AJAX(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// ��ӷ���
        /// </summary>
        /// <param name="indicator">������</param>
        /// <returns>ָ��</returns>
        public static void AddFunctions(CIndicator indicator)
        {
            string[] functions = FUNCTIONS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.AddFunction(new CFunctionAjax(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
            if (m_requests == null)
            {
                m_requests = new List<AjaxRequest>();
                Thread thread = new Thread(new ThreadStart(AjaxHandle));
                thread.Start();
            }
        }

        /// <summary>
        /// Ajax������
        /// </summary>
        public static void AjaxHandle()
        {
            while (true)
            {
                AjaxRequest request = null;
                if (m_requests != null)
                {
                    lock (m_requests)
                    {
                        int requestsSize = m_requests.Count;
                        if (requestsSize > 0)
                        {
                            request = m_requests[requestsSize - 1];
                            m_requests.RemoveAt(requestsSize - 1);
                        }
                    }
                }
                if (request != null)
                {
                    String resStr = "";
                    String lowerType = request.m_type.ToLower();
                    if (lowerType == "get")
                    {
                        resStr = HttpGetService.Get(request.m_url);
                    }
                    else if (lowerType == "post")
                    {
                        resStr = HttpPostService.Post(request.m_url);
                    }
                    if (m_listener != null)
                    {
                        m_listener.Invoke(new AjaxResponse(request.m_indicator, request.m_type, request.m_url, request.m_dataType, request.m_callBack, resStr));
                    }
                    else
                    {
                        request.m_indicator.CallFunction(request.m_callBack + "('" + resStr + "');");
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="args">����</param>
        private static void Invoke(object sender, object args)
        {
            AjaxResponse response = args as AjaxResponse;
            if (response != null)
            {
                response.m_indicator.CallFunction(response.m_callBack + "('" + response.m_resStr + "');");
            }
        }

        /// <summary>
        /// ���õ��ÿؼ�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        public static void SetListener(ControlA control)
        {
            m_listener = control;
            m_listener.RegisterEvent(new ControlInvokeEvent(Invoke), EVENTID.INVOKE);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Ajax��������
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        public double AJAX(CVariable var)
        {
            lock (m_requests)
            {
                Dictionary<String, String> parameters = new Dictionary<string, string>();
                int parametersLen = var.m_parameters.Length;
                AjaxRequest request = new AjaxRequest();
                request.m_indicator = m_indicator;
                for (int i = 0; i < parametersLen; i++)
                {
                    String parameter = m_indicator.GetText(var.m_parameters[i]);
                    int idx = parameter.IndexOf(':');
                    String name = parameter.Substring(0, idx).ToLower();
                    String value = parameter.Substring(idx + 1);
                    if (name == "type")
                    {
                        request.m_type = value;
                    }
                    else if (name == "url")
                    {
                        request.m_url = value;
                    }
                    else if (name == "datatype")
                    {
                        request.m_dataType = value;
                    }
                    else if (name == "success")
                    {
                        request.m_callBack = value;
                    }
                }
                m_requests.Add(request);

            }
            return 1;
        }
    }
}

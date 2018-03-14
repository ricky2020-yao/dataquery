#include "..\\..\\stdafx.h"
#include "ChatServiceEx.h"

ChatServiceEx::ChatServiceEx()
{
	m_requestID = BaseService::GetRequestID();
}

ChatServiceEx::~ChatServiceEx()
{
}

int ChatServiceEx::GetRequestID()
{
	return m_requestID;
}
#pragma once
#include <string>
#include <winsock2.h>
#include "result.h"

//�ͻ��� �ӿ�
class i_client
{
public:
	//��������
	virtual result* connected(SOCKET socket, SOCKADDR_IN addr) = 0;
};
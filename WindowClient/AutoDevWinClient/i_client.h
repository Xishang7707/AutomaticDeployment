#pragma once
#include <string>
#include <winsock2.h>
#include "result.h"

//客户端 接口
class i_client
{
public:
	//接收连接
	virtual result* connected(SOCKET socket, SOCKADDR_IN addr) = 0;
};
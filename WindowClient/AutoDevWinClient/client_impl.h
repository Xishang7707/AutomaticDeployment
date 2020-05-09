#pragma once
#include "i_client.h"
#include "winsock2.h"
#include <thread>

/*
客户端 实现
*/
class client_impl : public i_client
{
protected:
	//客户端关闭状态
	bool _client_is_closed;

	//客户端套接字
	SOCKET _client_socket;

	//客户端地址
	SOCKADDR_IN _client_addr;

	//接收数据线程
	std::thread* th_recv_data;
public:
	client_impl();

public:
	//接收连接
	result* connected(SOCKET socket, SOCKADDR_IN addr) override;

private:
	//接收数据
	void recv_data();
};


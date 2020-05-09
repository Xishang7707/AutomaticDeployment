#pragma once
#include "i_clientmanager.h"
#include <winsock2.h>
#include <iostream>
#include "i_client.h"
#include <list>

class clientmanager_impl :public i_clientmanager
{
public:
	clientmanager_impl(int port);

public:
	//启动
	result* start() override;

	//接收客户端连接
	result* connected();
protected:
	//程序监听端口
	int _server_port;

	//服务端套接字
	SOCKET _server_socket;

	//服务端地址
	SOCKADDR_IN _server_addr;

	//客户端列表
	std::list<i_client*>* client_list;
private:
	//初始化网络
	result* init_socket();
};


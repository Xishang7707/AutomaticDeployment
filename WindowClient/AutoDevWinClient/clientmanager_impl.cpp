#include "clientmanager_impl.h"
#include <Windows.h>
#include <thread>
#include "appfactory.h"

#pragma comment(lib, "ws2_32.lib")

//运行 持续
result* run(clientmanager_impl* instance);

clientmanager_impl::clientmanager_impl(int port)
{
	this->_server_port = port;
	ZeroMemory(&this->_server_socket, sizeof(SOCKET));
	ZeroMemory(&this->_server_addr, sizeof(SOCKADDR_IN));
	client_list = new std::list<i_client*>();
}

result* clientmanager_impl::start()
{
	result* res = this->init_socket();

	//启动运行
	std::thread run_thread(run, this);
	run_thread.join();
	return res;
}

result* clientmanager_impl::connected()
{
	result* res = new result();
	sockaddr_in client_addr;
	int client_addr_len = sizeof(client_addr);
	SOCKET client_socket = accept(this->_server_socket, (sockaddr FAR*) & client_addr, &client_addr_len);
	if (client_socket == INVALID_SOCKET)
	{
		res->msg = "client socket failed!";
		return res;
	}
	delete res;
	i_client* client = app_factory<i_client>();
	res = client->connected(client_socket, client_addr);
	if (!res->result)
	{
		return res;
	}
	//加入列表
	this->client_list->push_back(client);
	return res;
}

result* clientmanager_impl::init_socket()
{
	result* res = new result();
	WSADATA         wsd;            //WSADATA变量  
	//初始化套结字动态库 
	if (WSAStartup(MAKEWORD(2, 2), &wsd) != 0)
	{
		res->msg = "WSAStartup failed!";
		return res;
	}
	this->_server_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (INVALID_SOCKET == this->_server_socket)
	{
		res->msg = "socket failed!";
		WSACleanup();//释放套接字资源;  
		return  res;
	}
	//服务器套接字地址   
	this->_server_addr.sin_family = AF_INET;//IPv4 
	this->_server_addr.sin_port = htons(this->_server_port);//设置端口 建议大于1024
	this->_server_addr.sin_addr.s_addr = INADDR_ANY; //表示接受任何客户端的请求

	//绑定套接字  绑定服务端socket 和 端口
	int bind_res = bind(this->_server_socket, (LPSOCKADDR)&this->_server_addr, sizeof(SOCKADDR_IN));
	if (SOCKET_ERROR == bind_res)
	{
		res->msg = "bind failed!";
		closesocket(this->_server_socket);   //关闭套接字  
		WSACleanup();           //释放套接字资源;  
		return res;
	}

	//监听
	int listen_res = listen(this->_server_socket, 10);
	res->result = true;
	return res;
}

result* run(clientmanager_impl* instance)
{
	result* res = nullptr;

	while (true)
	{
		//释放内存
		if (res != nullptr)
		{
			delete res;
			res = nullptr;
		}
		res = instance->connected();
	}

	res->result = true;
	return res;
}

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
	//����
	result* start() override;

	//���տͻ�������
	result* connected();
protected:
	//��������˿�
	int _server_port;

	//������׽���
	SOCKET _server_socket;

	//����˵�ַ
	SOCKADDR_IN _server_addr;

	//�ͻ����б�
	std::list<i_client*>* client_list;
private:
	//��ʼ������
	result* init_socket();
};


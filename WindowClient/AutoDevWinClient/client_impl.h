#pragma once
#include "i_client.h"
#include "winsock2.h"
#include <thread>

/*
�ͻ��� ʵ��
*/
class client_impl : public i_client
{
protected:
	//�ͻ��˹ر�״̬
	bool _client_is_closed;

	//�ͻ����׽���
	SOCKET _client_socket;

	//�ͻ��˵�ַ
	SOCKADDR_IN _client_addr;

	//���������߳�
	std::thread* th_recv_data;
public:
	client_impl();

public:
	//��������
	result* connected(SOCKET socket, SOCKADDR_IN addr) override;

private:
	//��������
	void recv_data();
};


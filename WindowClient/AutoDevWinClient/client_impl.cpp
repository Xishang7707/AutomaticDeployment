#include "client_impl.h"

//接收数据
void func_recv_data(client_impl* instance);

client_impl::client_impl()
{
	this->_client_is_closed = false;
}

result* client_impl::connected(SOCKET socket, SOCKADDR_IN addr)
{
	result* res = new result();
	this->_client_socket = socket;
	this->_client_addr = addr;

	th_recv_data = new std::thread(func_recv_data, this);
	th_recv_data->detach();
	res->result = true;
	return res;
}

void client_impl::recv_data()
{

}

void func_recv_data(client_impl* instance)
{

}
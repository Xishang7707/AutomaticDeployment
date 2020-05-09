#pragma once
#include <iostream>
#include "i_client.h"
#include "i_clientmanager.h"

#include "client_impl.h"
#include "clientmanager_impl.h"

template<class I>
I* app_factory(int val = 0)
{
	if (std::is_same<I, i_client>::value)
	{
		return (I*)new client_impl();
	}
	else if (std::is_same<I, i_clientmanager>::value)
	{
		return (I*)new clientmanager_impl(val);
	}

	return nullptr;
}
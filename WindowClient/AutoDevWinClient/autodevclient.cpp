#include <iostream>
#include "appfactory.h"

int main(int argc, char* args[])
{
	i_clientmanager* il = app_factory<i_clientmanager>(7000);
	il->start();
	return 0;
}
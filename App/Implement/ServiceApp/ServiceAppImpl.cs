using App.Interface;
using Model.In;
using Model.In.Service;
using Model.Out;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Implement.ServiceApp
{
    class ServiceAppImpl : IServiceApp
    {
        private IServiceServer serviceServer = ServerFactory.Get<IServiceServer>();

        public async Task<Result> AddService(In<AddServiceIn> inData)
        {
            return await serviceServer.AddService(inData);
        }
    }
}

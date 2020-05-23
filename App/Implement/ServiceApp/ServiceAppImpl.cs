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

        public async Task<Result> DeleteService(In<DeleteServiceIn> inData)
        {
            return await serviceServer.DeleteService(inData);
        }

        public async Task<Result> EditService(In<EditServiceIn> inData)
        {
            return await serviceServer.EditService(inData);
        }

        public async Task<Result> GetDropService()
        {
            return await serviceServer.GetDropService();
        }

        public async Task<Result> GetService(In<string> inData)
        {
            return await serviceServer.GetService(inData);
        }

        public async Task<Result> GetServiceList()
        {
            return await serviceServer.GetServiceList();
        }
    }
}

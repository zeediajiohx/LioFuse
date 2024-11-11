using Grpc.Core;
using GrpcGreeterClient.Wells;

namespace GrpcGreeterClient.Services
{
    public class Grpc_well_lite: WellService.WellServiceClient
    {
        private readonly ILogger<Grpc_well_lite> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public Grpc_well_lite(ILogger <Grpc_well_lite> logger,IHttpClientFactory httpclientFactory) {
            _logger = logger;
            _httpClientFactory = httpclientFactory;
        }
        //public override async Task<WellResponse> GetWells(WellRequest request,ServerCallContext context)
        //{
        //    return Task.FromResult(new WellResponse
        //    {
                
        //    });
        //}
    }
}

















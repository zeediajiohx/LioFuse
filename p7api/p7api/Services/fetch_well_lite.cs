using Grpc.Net.Client;

namespace pseven.Services
{
    public class fetch_well_lite
    {
        public fetch_well_lite() {
            var channel = GrpcChannel.ForAddress("https://localhost:7004");

        }
        

    }
}

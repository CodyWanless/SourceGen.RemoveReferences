using ServiceStack;

namespace Original.Messages
{
    [Route("/Widget/{Id}", "GET")]
    public class GetWidgetsRequest : IReturn<GetWidgetsResponse>
    {
        public Guid Id { get; set; }
    }
}

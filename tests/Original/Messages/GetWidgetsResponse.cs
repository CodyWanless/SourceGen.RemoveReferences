using Original.Types;
using ServiceStack;

namespace Original.Messages
{
    public class GetWidgetsResponse : IHasResponseStatus
    {
        public ResponseStatus? ResponseStatus { get; set; }

        public Widget? Widget { get; set; }
    }
}

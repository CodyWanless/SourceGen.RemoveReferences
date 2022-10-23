using Expected.Types;

namespace Expected.Messages
{
    public class GetWidgetsResponse
    {
        public object? ResponseStatus { get; set; }

        public Widget? Widget { get; set; }
    }
}

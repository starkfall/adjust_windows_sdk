using System.Collections.Generic;
using static AdjustSdk.NetStandard.Constants;

namespace AdjustSdk.NetStandard
{
    public class AdjustEventSuccess
    {
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string Adid { get; set; }
        public string EventToken { get; set; }
        public string CallbackId { get; set; }
        public Dictionary<string, string> JsonResponse { get; set; }

        public override string ToString()
        {
            return Util.F("Event Success msg:{0} time:{1} adid:{2} event:{3} cid:{4} json:{5}",
                Message,
                Timestamp,
                Adid,
                EventToken,
                CallbackId,
                JsonResponse);
        }
    }
}

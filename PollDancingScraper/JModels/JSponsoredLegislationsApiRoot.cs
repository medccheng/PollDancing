using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JSponsoredLegislationsApiRoot
    {
        [JsonPropertyName("sponsoredlegislation")]
        public List<JSponsoredLegislation> SponsoredLegislations { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JLegislationApiRoot
    {
        [JsonPropertyName("bills")]
        public List<JLegislation> Bills { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JLegislationDetailsApiRoot
    {
        [JsonPropertyName("bill")]
        public JLegislation Bill { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JActionApiRoot
    {
        [JsonPropertyName("actions")]
        public List<JAction> Actions { get; set; }
    }
}

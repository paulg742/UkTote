using Newtonsoft.Json;

namespace UkTote.UI.Model
{
    public class SlackMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

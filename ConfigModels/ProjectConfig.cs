using System.Collections.Generic;
using Newtonsoft.Json;
using proAV.Core.Config;

namespace Ch5Test.ConfigModels {
    public class ProjectConfig : ConfigBase {
        
        [JsonProperty("rooms")]
        public List<ConfigBase> Rooms { get; set; }
        
        [JsonProperty("devices")]
        public DeviceConfigs DeviceConfigs { get; set; }
        
        [JsonProperty("touchpanels")]
        public List<TouchpanelConfig> Touchpanels { get; set; }
    }
}
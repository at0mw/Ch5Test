using System.Collections.Generic;
using Newtonsoft.Json;
using proAV.Core.Config;
using proAV.Core.Dmc;

namespace Ch5Test.ConfigModels {
    public class DeviceConfigs {
        [JsonProperty("airmedias")]
        public List<DeviceConfig> Airmedias { get; set; }
        
        [JsonProperty("nvxframes")] 
        public List<DmcServerConfig> NvxFrames { get; set; }
        
        [JsonProperty("displays")]
        public List<DeviceConfig> Displays { get; set; }
    }
}
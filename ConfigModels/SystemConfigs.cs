using proAV.Core.Attributes;
using proAV.Core.Framework;

namespace Ch5Test.ConfigModels {
    public class SystemConfigs : JsonConfigContainer {
        [ConfigName("projectconfig.json")]
        public ProjectConfig ProjectConfig { get; set; }
        
    }
}
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using proAV.Core.Config;
using proAV.Core.Devices.Displays;
using proAV.Core.Enumeration;
using proAV.Core.Extensions;
using proAV.Core.Interfaces.Device.Agnostic;

namespace Ch5Test.Rooms {
    public class RoomBase : IGroupIdentifier, IObjectIdentifiers {
        public int GroupId { get; set; }
        public string ObjectName { get; set; }
        public int ObjectId { get; set; }
        public string ObjectAddress { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string CustomLabel { get; set; }
        public IEnumerable<Display> RoomDisplays => ControlSystem.Devices.OfType<Display>().Where(x_ => x_.GroupId == GroupId);
        public void ParseConfigProperties(ConfigBase config_) {
            GroupId = config_.GroupId;
            ObjectName = config_.Name;
            ObjectId = config_.Id;
            ObjectAddress = config_.Address;
            Type = config_.Type;
            Description = config_.Description;
            CustomLabel = config_.CustomLabel;
        }

        public void RoomOn() {
            foreach (var roomDisplay in RoomDisplays) {
                roomDisplay.Power = true;
            }
        }

        public void RoomOff() {
            foreach (var roomDisplay in RoomDisplays) {
                roomDisplay.Power = false;
            }
        }
        
        public void RoomReport() {
            "*************************************".PrintLine(ConsolePrintColours.Blue);
            "\n".PrintLine();
            var room = $"Report for Room {ObjectName} Group ID: {GroupId.ToString(CultureInfo.InvariantCulture)}";
            room.PrintLine(ConsolePrintColours.Cyan);
            var roomType = $"Room Type: {Type}";
            roomType.PrintLine(ConsolePrintColours.Cyan);
            "\n".PrintLine();
            "End of Report".PrintLine();
            "*************************************".PrintLine(ConsolePrintColours.Blue);
        }
    }
}
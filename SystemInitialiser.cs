using System.Collections.Generic;
using System.Linq;
using Ch5Test.Rooms;
using Ch5Test.UserInterface;
using Ch5Test.UserInterface.Listeners;
using Crestron.LondonOffice.Constants;
using Crestron.SimplSharp.Reflection;
using Crestron.SimplSharpPro.UI;
using proAV.Core;
using proAV.Core.Config;
using proAV.Core.Devices;
using proAV.Core.Devices.Displays;
using proAV.Core.Devices.Projectors;
using proAV.Core.Enumeration;
using proAV.Core.Extensions;
using proAV.Core.Framework;
using proAV.Core.Interfaces.API;
using proAV.Core.Interfaces.UserInterface;
using proAV.Core.UserInterface;
using proAV.Core.Utilities;

namespace Ch5Test {
    public class SystemInitialiser : SystemInitialiserBase {
        public override void Initialise() {
            ProAvControlSystem.CrestronEndpoints.RegisterAllDevices();
            ProAvControlSystem.CrestronNativeDevices.RegisterAllDevices();
            ProAvControlSystem.UserInterfaces.InitialiseAll();
            ControlSystem.Devices.OfType<IInitialise>().ForEach(x_ => x_.Initialise());
        }

        #region Touchpanels

        [SystemCreationMethod("UserInterfaces", Order = 1)]
        public static void CreateTouchpanels() {
            if (ControlSystem.Configs.ProjectConfig.Touchpanels.Count == 0) {
                return;
            }
            var touchpanelConfigs = new List<TouchpanelConfig>();
            touchpanelConfigs.AddRange(ControlSystem.Configs.ProjectConfig.Touchpanels);
            
            var newUserInterfaces = UserInterfaceFactory.Create(touchpanelConfigs);
            var joinMaps = newUserInterfaces as IUserInterface[] ?? newUserInterfaces.ToArray();
            if (ProAvControlSystem.UserInterfaces != null) {
                ProAvControlSystem.UserInterfaces.AddRange(joinMaps);
                "All TouchPanels Created".PrintSystemMessage();
                "Creating UserInterface Listeners".PrintSystemMessage();
                foreach (var userInterface in ProAvControlSystem.UserInterfaces) {
                    var userInterfaceId = userInterface.ObjectId;
                    if (touchpanelConfigs.Any(x_ => x_.Id.Equals(userInterfaceId))) {
                        UserInterfaceListeners.Create(userInterface);
                    }
                }
            }

            // foreach (var touchpanelConfig in ControlSystem.Configs.ProjectConfig.Touchpanels) {
            //     var sigHandler = TouchpanelFactory.Create(touchpanelConfig);
            //     if (sigHandler != null) {
            //         switch (touchpanelConfig.Type) {
            //             case "Ch5": {
            //                 var userInterface = new ProjectUserInterface(sigHandler) {
            //                     ObjectId = touchpanelConfig.Id,
            //                     GroupId = touchpanelConfig.GroupId,
            //                     ObjectType = touchpanelConfig.Userinterface.Type,
            //                 };
            //                 break;
            //             }
            //         }
            //     }
            // }
            //
            // ProAvControlSystem.UserInterfaces.AddRange(userInterfaces);
            // foreach (var userInterface in userInterfaces) {
            //     if (userInterface is ProjectUserInterface newUi) {
            //         var touchpanelListener = new UserInterfaceListener(newUi);
            //         ControlSystem.TouchpanelListeners.Add(touchpanelListener);
            //     }
            // }
        }

        [SystemCreationMethod("UserInterfaceListeners", Order = 2)]
        private void CreateUserInterfaceListeners() {
            foreach (var userInterface in ProAvControlSystem.UserInterfaces.OfType<UserInterfaceBase>()) {
                userInterface.AllUserInterfaces = () => ProAvControlSystem.UserInterfaces;
                if (userInterface.Touchpanel is Tsw1060) {
                    UserInterfaceListeners.Create(userInterface);
                }
            }
        }

        #endregion

        #region Device Cluster Tags

        [SystemCreationMethod("DeviceClusterTags", Order = 3)]
        private void CreateDeviceClusterTags() {
            if (ControlSystem.Devices != null) {
                DeviceClusterBase.TypeTags[DeviceTags.VideoOnlySource] = new List<CType>();
                DeviceClusterBase.TypeTags[DeviceTags.AudioSource] = new List<CType>();
                DeviceClusterBase.TypeTags[DeviceTags.AudioOnlySource] = new List<CType>();
                DeviceClusterBase.TypeTags[DeviceTags.VideoOutput] = new List<CType> {
                    typeof(Display),
                    typeof(Projector),
                    typeof(VideoOutput)
                };
                DeviceClusterBase.TypeTags[DeviceTags.AudioOutput] = new List<CType> {
                    typeof(AudioOutput)
                };
            }
            else {
                this.Error("DeviceCluster", "ControlSystem.Devices is null. Not good news!");
            }
        }

        #endregion

        #region Rooms

        [SystemCreationMethod("RoomLogic", Order = 4)]
        private void CreateRooms() {
            if (ControlSystem.Configs.ProjectConfig.Rooms == null) {
                return;
            }
            
            var types = ReflectionHelper.GetInheritingTypes<RoomBase>(Assembly.GetExecutingAssembly());
            var roomTypes = types as List<CType> ?? types.ToList();
            foreach (var roomConfig in ControlSystem.Configs.ProjectConfig.Rooms) {
                var roomTypeName = roomConfig.Type;
                var roomType = roomTypes.FirstOrDefault(x_ => x_.Name == roomTypeName);
                if (roomType != null) {
                    if (ReflectionHelper.ConstructType(roomType, null) is RoomBase newRoom) {
                        "Creating room {0} of type : {1}, with Group ID: {2}".PrintLine(ConsolePrintColours.Purple, roomConfig.Name, roomTypeName, roomConfig.GroupId.ToString());
                        newRoom.ParseConfigProperties(roomConfig);
                        ControlSystem.Rooms.Add(newRoom);
                        ConsoleCommands.Create(x_ => ControlSystem.Rooms[newRoom.GroupId].RoomReport(), $"roomrep{newRoom.GroupId.ToString()}", $"Prints a report of room with group id {newRoom.GroupId.ToString()}");
                    }
                    else {
                        this.Error("CreateRooms", "Couldn't create room type {0} with id {1}", roomConfig.Type, roomConfig.Id);
                    }
                }
                else {
                    if (string.IsNullOrEmpty(roomConfig.Type)) {
                        this.Error("CreateRooms", "Room type is null or empty");
                    }
                    else {
                        this.Error("CreateRooms", "Couldn't find a RoomBase object type with name {0}", roomConfig.Type);
                    }
                }
            }
        }

        #endregion
    }
}
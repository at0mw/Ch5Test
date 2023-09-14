using System;
using System.Collections.Generic;
using Ch5Test.ConfigModels;
using Ch5Test.Rooms;
using Ch5Test.UserInterface;
using Crestron.SimplSharp.Reflection;
using proAV.Core;
using proAV.Core.Enumeration;
using proAV.Core.Extensions;
using proAV.Core.Framework;
using proAV.Core.Interfaces.Device.TypeSpecific.Dmc;
using proAV.Core.UserInterface.Listeners;
using proAV.Core.Utilities;

namespace Ch5Test
{
    public class ControlSystem : ProAvControlSystem
    {
        public static SystemConfigs Configs { get; private set; }
        public static  DeviceCluster<DeviceBase> Devices { get; set; }
        public static List<RoomBase> Rooms { get; private set; }
        public static List<UserInterface.Listeners.ListenerBase<ProjectUserInterface>> TouchpanelListeners { get; private set; }
        
        public ControlSystem() : base(Assembly.GetExecutingAssembly(), 50) {
        }

        protected override object StartProgram(object _) {
            Configs = new SystemConfigs();
            Devices = new DeviceCluster<DeviceBase>();
            Rooms = new List<RoomBase>();
#if DEBUG
            "Program is in DEBUG mode. Type 'start' to run".PrintSystemMessage();
            ProgramAutoStart();
            
            // Create 'start' command with action to cancel auto start 
            ConsoleCommands.Create(x_ => {
	            CancelAutoStart();
#endif
	            ProgramStart();
#if DEBUG
	            ConsoleCommands.Remove("start");
            }, "start", "Start program in debug mode");
#endif
            return null;
        }

        private static void ProgramStart() {
#if DEBUG
	        "Starting program in Debug mode".PrintLine(ConsolePrintColours.Cyan);
	        Configs.GetEmbeddedConfigs(Assembly.GetExecutingAssembly(), false);
	        // Enable Program Auto Update 
	        ProgramUpdateChecker.AutoUpdateProgram = true;
#endif
#if !DEBUG
            Configs.GetEmbeddedConfigs(Assembly.GetExecutingAssembly(), false);
#endif
	        try {
	            Configs.UpdateConfigs();
	            var initialiser = new SystemInitialiser();
	            SystemInitialiser.CreateTouchpanels();
	            initialiser.Create(new ConsolePercentageFeedback());
	            initialiser.Initialise();
	        }
	        catch (Exception exception) {
		        Console.WriteLine(exception);
		        throw;
	        }
        }
    }
}
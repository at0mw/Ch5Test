using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Ch5Test.UserInterface.Listeners {
    public class UserInterfaceListener : ListenerBase<ProjectUserInterface> {
        public UserInterfaceListener(ProjectUserInterface userInterface_) : base(userInterface_) {
            if (userInterface_.Touchpanel is BasicTriList userInterface) {
                userInterface.OnlineStatusChange += UserInterfaceOnlineStatusChange;
            }
        }
        
        private void UserInterfaceOnlineStatusChange(GenericBase currentDevice_, OnlineOfflineEventArgs args_) {
        }
    }
}
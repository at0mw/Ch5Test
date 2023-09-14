using proAV.Core.Interfaces.UserInterface;

namespace Ch5Test.UserInterface.Listeners {
    public static class UserInterfaceListeners {
        public static void Create(IUserInterface userInterface_) {
            if (userInterface_ is ProjectUserInterface projectUserInterface) {
                CreateDefaultListeners(projectUserInterface);
            }
        }
        
        private static void CreateDefaultListeners(ProjectUserInterface userInterface_) {
            userInterface_.Listeners.AddRange(new IListenerBase[] {
                new MainListener(userInterface_),
            });
        }
    }
}
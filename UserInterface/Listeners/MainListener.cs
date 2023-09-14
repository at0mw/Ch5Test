using proAV.Core.CustomEventArgs;
using proAV.Core.Extensions;
using proAV.Core.Interfaces.UserInterface;

namespace Ch5Test.UserInterface.Listeners {
    public class MainListener : IListenerBase {
        private readonly ProjectUserInterface _userInterface;
        
        public MainListener(ProjectUserInterface userInterface_) {
            userInterface_.MainPage.Button1.OnPressAndRelease = Button1PressAndRelease;
            userInterface_.MainPage.Button2.OnPressAndRelease = Button2PressAndRelease; 
            userInterface_.MainPage.Slider.ProgramReceivedJoinValueChange += SliderOnProgramReceivedJoinValueChange;
            _userInterface = userInterface_;
        }

        private void Button1PressAndRelease() {
            "Button1 PressAndRelease".PrintLine();
            _userInterface.MainPage.TextBox.Set("Button 1 Pressed");
        }
        
        private void Button2PressAndRelease() {
            "Button1 PressAndRelease".PrintLine();
            _userInterface.MainPage.TextBox.Set("Button 2 Pressed");
        }
        
        private void SliderOnProgramReceivedJoinValueChange(object sender_, UShortEventArgs e_) {
            _userInterface.MainPage.TextBox.Set($"Slider 1 Value = {e_.Value}");
        }
    }
}
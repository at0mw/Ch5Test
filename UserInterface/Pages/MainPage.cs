using System.Net.Mime;
using proAV.Core.Interfaces.UserInterface;
using proAV.Core.UserInterface;
using proAV.Core.UserInterface.JoinConstants;
using proAV.Core.UserInterface.Joins.AnalogJoins;
using proAV.Core.UserInterface.Joins.SerialJoins;
using proAV.Core.UserInterface.VisionToolsObjects;

namespace Ch5Test.UserInterface.Pages {
	public class MainPage {
		public BasicButton Button1;
		public BasicButton Button2;
		public AnalogJoin Slider;
		public SerialInputJoin TextBox;

		public MainPage(IUiSigHandler uiSigHandler_) {
			Button1 = new BasicButton(uiSigHandler_.JoinCollection) { PressJoinNumber = TouchPanelBoolJoins.Button1 };
			Button2 = new BasicButton(uiSigHandler_.JoinCollection) { PressJoinNumber = TouchPanelBoolJoins.Button2 };
			Slider = new AnalogJoin(uiSigHandler_.JoinCollection, TouchPanelAnalogJoins.Slider);
			TextBox = JoinMapper.Map<SerialInputJoin>(uiSigHandler_.JoinCollection, TouchPanelSerialJoins.TextBox);
		}
	}
}
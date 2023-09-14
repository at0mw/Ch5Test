using System;
using System.Collections.Generic;
using System.Linq;
using Ch5Test.UserInterface.Pages;
using proAV.Core;
using proAV.Core.Config.VtPro;
using proAV.Core.Interfaces.UserInterface;
using proAV.Core.UserInterface;
using proAV.Core.UserInterface.Joins.BoolJoins;

namespace Ch5Test.UserInterface {
	public class ProjectUserInterface : UserInterfaceBase {
		public MainPage MainPage;

		public ProjectUserInterface(IUiSigHandler uiSigHandler_) : base(uiSigHandler_) {
			MainPage = new MainPage(uiSigHandler_);
		}
	}
}
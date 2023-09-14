using proAV.Core.Interfaces.UserInterface;

namespace Ch5Test.UserInterface.Listeners {
	public abstract class ListenerBase<T> : IListenerBase where T : IUserInterface {
		public T UserInterface { get; private set; }
		protected ListenerBase(T userInterface_) {
			UserInterface = userInterface_;
		}
	}
}
namespace Framework
{

	/// <summary>
	/// Game Event base class
	/// </summary>
	public class GameEvent
	{
		// Event name
		protected Event eventName;

		public GameEvent()
		{
		}

		public GameEvent(Event eventName)
		{
			this.eventName = eventName;
		}

		public Event EventName
		{
			get { return eventName; }
		}

		public enum Event {
			NONE
		}
	}
}
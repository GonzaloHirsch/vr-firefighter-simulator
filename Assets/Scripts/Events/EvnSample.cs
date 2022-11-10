
public class EvnSample : Framework.GameEvent
{
    public new const Framework.GameEvent.Event EventName = Framework.GameEvent.Event.NONE;

    public static EvnSample notifier = new EvnSample();
    
    /// <summary>
    /// Constructor
    /// </summary>
    public EvnSample()
    {
        eventName = EventName;
    }
}

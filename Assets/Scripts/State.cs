public class State : Framework.Singleton<State> {
    public float startTime;
    public float endTime;
    public bool hasPlayed = false;
    public float approvalLimit;

    public void Reset() {
        this.startTime = this.endTime = this.approvalLimit = 0;
        this.hasPlayed = false;
    }
}
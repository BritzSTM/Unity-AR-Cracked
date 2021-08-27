public class TimeTextUpdater : GMTextUpdaterBase
{
    protected override void OnUpdateText(GameManager manager)
    {
        int h = (int)manager.PlayTime / 3600;
        int m = (int)manager.PlayTime / 60 % 60;
        int s = (int)manager.PlayTime % 60;

        SetText(string.Format("{0:D2} : {1:D2} : {2:D2}", h, m, s));
    }
}

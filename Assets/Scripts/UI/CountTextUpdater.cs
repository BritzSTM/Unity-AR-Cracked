public class CountTextUpdater : GMTextUpdaterBase
{
    protected override void OnUpdateText(GameManager manager)
    {
        SetText(manager.CurrentDimCount.ToString("D4"));
    }
}

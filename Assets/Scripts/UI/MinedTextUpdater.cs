public class MinedTextUpdater : GMTextUpdaterBase
{
    protected override void OnUpdateText(GameManager manager)
    {
        SetText(manager.MinedDimCount.ToString("D8"));
    }
}

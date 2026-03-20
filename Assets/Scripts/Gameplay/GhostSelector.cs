public static class GhostSelector
{
    private static GhostBehavior selectedGhost;

    public static void SelectGhost(GhostBehavior ghost)
    {
        if (selectedGhost != null)
            selectedGhost.SetSelected(false);

        selectedGhost = ghost;

        if (selectedGhost != null)
            selectedGhost.SetSelected(true);
    }

    public static GhostBehavior GetSelectedGhost() => selectedGhost;
}
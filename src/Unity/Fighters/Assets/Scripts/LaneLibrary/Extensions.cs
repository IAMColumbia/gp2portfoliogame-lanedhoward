namespace LaneLibrary
{
    public static class Extensions
    {
        public static int ToInt(this bool b)
        {
            if (b) return 1;
            return 0;
        }

    }
}
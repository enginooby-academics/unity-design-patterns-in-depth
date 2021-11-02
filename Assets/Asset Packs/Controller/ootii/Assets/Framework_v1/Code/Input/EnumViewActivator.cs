namespace com.ootii.Input
{
    /// <summary>
    /// Defines the valid options for the ViewActivator button on an InputSource
    /// </summary>
    public static class EnumViewActivator
    {
        public static int NONE = 0;
        public static int LEFT_BUTTON = 1;
        public static int RIGHT_BUTTON = 2;
        public static int LEFT_OR_RIGHT_BUTTON = 3;
        public static int MIDDLE_MOUSE_BUTTON = 4;

        public static string[] Names =
        {
            "None",
            "Left Mouse Button",
            "Right Mouse Button",
            "Left or Right Mouse Button",
            "Middle Mouse Button"
        };
    }
}
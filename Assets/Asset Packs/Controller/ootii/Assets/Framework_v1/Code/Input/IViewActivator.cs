namespace com.ootii.Input
{    
    /// <summary>
    /// Indicates than an InputSource has a ViewActivator button option
    /// </summary>
    public interface IViewActivator 
    {
        /// <summary>
        /// Key or button used to allow view to be activated
        /// 0 = none
        /// 1 = left mouse button
        /// 2 = right mouse button
        /// 3 = left and right mouse button
        /// </summary>
        int ViewActivator { get; set; }

        /// <summary>
        /// Determines if the player can freely look around
        /// </summary>        
        bool IsViewingActivated { get; }
    }
}

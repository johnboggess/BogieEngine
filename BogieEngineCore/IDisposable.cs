namespace BogieEngineCore
{
    ///todo: Use .nets IDisposable interface
    /// <summary>
    /// Used to handle disposing unmanaged resources. Should probably use .Net's IDisposable interface. 
    /// </summary>
    interface IDisposable
    {
        /// <summary>
        /// Has the object been disposed?
        /// </summary>
        bool Disposed { get; }

        /// <summary>
        /// Dispose of the object.
        /// </summary>
        void Dispose();
    }
}

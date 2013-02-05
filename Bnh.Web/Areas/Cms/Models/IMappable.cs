namespace Cms.Models
{
    /// <summary>
    /// Adds map brick support
    /// </summary>
    public interface IMappable
    {
        /// <summary>
        /// Location of an entity
        /// </summary>
        string GpsLocation { get; }

        /// <summary>
        /// Entity bounds
        /// </summary>
        string GpsBounds { get; }
    }
}
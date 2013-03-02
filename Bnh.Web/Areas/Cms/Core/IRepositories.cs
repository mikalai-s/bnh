using Cms.Infrastructure;
using Cms.Models;

namespace Cms.Core
{
    public interface IRepositories
    {
        MongoRepository<SpecialScene> SpecialScenes { get; }
        ReviewRepository Reviews { get; }
        MongoRepository<Profile> Profiles { get; }
        MongoRepository<Comment> Feedback { get; }
    }
}
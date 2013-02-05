using Cms.Infrastructure;
using Cms.Models;

namespace Cms.Core
{
    public interface IRepositories
    {
        MongoRepository<Scene> Scenes { get; }
        MongoRepository<BrickContent> BrickContents { get; }
        ReviewRepository Reviews { get; }
        MongoRepository<Profile> Profiles { get; }
        MongoRepository<Comment> Feedback { get; }
    }
}
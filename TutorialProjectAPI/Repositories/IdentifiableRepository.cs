using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Models;

namespace TutorialProjectAPI.Repositories
{
    public class IdentifiableRepository<T> : GenericRepository<T> where T : class, IIdentifiableDB
    {
        public IdentifiableRepository(MainContext context) : base(context)
        {
        }

        public MainContext? Context { get; internal set; }

        public T GetById(Guid id)
        {
            return entities.SingleOrDefault<T>(s => s.Id == id);
        }
    }
}

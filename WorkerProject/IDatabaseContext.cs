using System.Threading.Tasks;

namespace WorkerProject
{
    public interface IDatabaseContext
    {
        Task SaveChangesAsync();
    }
}

using CalculatorMAUI.Entities;

namespace CalculatorMAUI.Services
{
    public interface IDbService
    {
        public Task<IEnumerable<Project>> GetAllProjectsAsync();
        public Task<IEnumerable<Entities.Task>> GetProjectTasksAsync(int projectId);
        public System.Threading.Tasks.Task InitAsync();
    }
}

using System.Diagnostics;
using CalculatorMAUI.Entities;
using SQLite;

namespace CalculatorMAUI.Services
{
    public class SQLiteService : IDbService
    {
        private readonly SQLiteAsyncConnection db;

        public SQLiteService()
        {
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Projects.db3");
            db = new SQLiteAsyncConnection(databasePath);
        }

        public async System.Threading.Tasks.Task InitAsync()
        {
            try
            {
                await db.CreateTableAsync<Entities.Task>();
                await db.CreateTableAsync<Project>();

                var existingProjects = await db.Table<Project>().CountAsync();
                if (existingProjects > 0) return;

                var Projects = new List<Project>
                {
                    new Project { Name = "Lab3 C#", CreatedDate = new DateTime(2025, 04, 12) },
                    new Project { Name = "Lab4 C#", CreatedDate = new DateTime(2025, 04, 15) },
                    new Project { Name = "Lab5 C#", CreatedDate = new DateTime(2025, 04, 16) }
                };
                await db.InsertAllAsync(Projects);

                var Tasks = new List<Entities.Task>
                {
                    new Entities.Task { Description = "Сделать калькулятор", Duration = 3, ProjectId = Projects[0].Id },
                    new Entities.Task { Description = "Протестировать", Duration = 3, ProjectId = Projects[0].Id },
                    new Entities.Task { Description = "Сдать лабу", Duration = 3, ProjectId = Projects[0].Id },

                    new Entities.Task { Description = "Сделать ProgresDemo", Duration = 3, ProjectId = Projects[1].Id },
                    new Entities.Task { Description = "Протестировать", Duration = 3, ProjectId = Projects[1].Id },
                    new Entities.Task { Description = "Сдать лабу", Duration = 3, ProjectId = Projects[1].Id },

                    new Entities.Task { Description = "Сделать SQLiteDemo", Duration = 3, ProjectId = Projects[2].Id },
                    new Entities.Task { Description = "Протестировать", Duration = 3, ProjectId = Projects[2].Id },
                    new Entities.Task { Description = "Сдать лабу", Duration = 3, ProjectId = Projects[2].Id }
                };
                await db.InsertAllAsync(Tasks);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Init error: {ex}");
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            try
            {
                return await db.Table<Project>().ToListAsync();
            }
            catch
            {
                Debug.WriteLine("Error in GetAllProjectsAsync()!");
                return new List<Project>();
            }
        }

        public async Task<IEnumerable<Entities.Task>> GetProjectTasksAsync(int projectId)
        {
            try
            {
                return await db.Table<Entities.Task>()
                         .Where(t => t.ProjectId == projectId)
                         .ToListAsync();
            }
            catch
            {
                Debug.WriteLine("Error in GetProjectTasksAsync(int projectId)!");
                return new List<Entities.Task>();
            }
        }
    }
}

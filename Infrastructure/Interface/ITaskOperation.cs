using AutoTask.Domain.Model;
using System.Collections.Generic;

namespace AutoTask.Shared.Interface
{
    public interface ITaskOperation
    {
        System.Threading.Tasks.Task CreateTask(string name, string status, int progress, int priority, int parentId, int? userId);
        System.Threading.Tasks.Task DeleteTask(int id);
        System.Threading.Tasks.Task<IEnumerable<Task>> GetAll();
        System.Threading.Tasks.Task<Task> GetById(int id);
        System.Threading.Tasks.Task UpdateTask(int id, string name, string status, int progress, int priority, int parentId);
    }
}
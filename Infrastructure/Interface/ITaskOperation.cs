using AutoTask.Domain.Model;
using System.Collections.Generic;

namespace AutoTask.Shared.Interface
{
    public interface ITaskOperation
    {
        void CreateTask(string name, string status, int progress, int priority, int parentId, int? userId);
        void DeleteTask(int id);
        IEnumerable<Task> GetAll();
        Task GetById(int id);
        void UpdateTask(int id, string name, string status, int progress, int priority, int parentId);
    }
}
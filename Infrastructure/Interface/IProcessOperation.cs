using AutoTask.Domain.Model;
using System;
using System.Collections.Generic;

namespace AutoTask.Shared.Interface
{
    public interface IProcessOperation
    {
        System.Threading.Tasks.Task CreateProcess(string name, DateTime beginDate, DateTime? endDate, string description);
        System.Threading.Tasks.Task DeleteProcess(int id);
        System.Threading.Tasks.Task<IEnumerable<Process>> GetAll();
        System.Threading.Tasks.Task<Process> GetById(int id);
        System.Threading.Tasks.Task UpdateProcess(int id, string name, DateTime beginDate, DateTime? endDate, string description);
    }
}
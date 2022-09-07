using AutoTask.Domain.Model;
using System;
using System.Collections.Generic;

namespace AutoTask.Shared.Interface
{
    public interface IProcessOperation
    {
        void CreateProcess(string name, DateTime beginDate, DateTime? endDate, string description);
        void DeleteProcess(int id);
        IEnumerable<Process> GetAll();
        Process GetById(int id);
        void UpdateProcess(int id, string name, DateTime beginDate, DateTime? endDate, string description);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(var context = new DomainContext())
            {
                //Process process = new Process()
                //{
                //    Name = "Add DataBase",
                //    Status = "New",
                //    Begin = DateTime.Now,
                //    Description = "Adding MS SQl Server support to application"
                //};
                //context.Processes.Add(process);
                //context.SaveChanges();

                //List<Task> tasks = new List<Task>()
                //{
                //    new Task()
                //    {
                //        Name = "Download SQL Server",
                //        Status = "New",
                //        Progress = 0,
                //        Priority = 1,
                //        ProcessId = process.Id,
                //    },
                //    new Task()
                //    {
                //        Name = "Make new database",
                //        Status = "New",
                //        Progress = 0,
                //        Priority = 2,
                //        ProcessId = process.Id,
                //    },
                //};
                //context.Tasks.AddRange(tasks);
                //context.SaveChanges();

                //foreach (var process1 in context.Processes.ToList())
                //{
                //    if (process.Tasks != null)
                //    {
                //        List<Task> tasks1 = process1.Tasks.ToList();
                //        foreach (Task task1 in tasks1)
                //        {
                //            Console.WriteLine(task1.Id + " " + task1.Name + " " + task1.Status + " " + task1.ProcessId + " " + task1.Priority);
                //        }
                //    }
                //}
                //Console.ReadLine();
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SV_13_14_2Teste
{
    #region Question task
    public interface Task
    {
        void Run();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(Type t)
        {
            this.Type = t;
        }

        public Type Type { get; set; }
    }

    [DependsOn(typeof(Task3))]
    class Task1 : Task
    {

        public Task1() { }

        public void Run()
        {
            Console.WriteLine("Task1");
        }
    }

    class Task2 : Task { public void Run() { Console.WriteLine("Task2"); } }

    [DependsOn(typeof(Task2))]
    class Task3 : Task { public void Run() { Console.WriteLine("Task3"); } }


    public class TaskRunner
    {
        public static void Submit(Task task)
        {
            LinkedList<Task> tasks = new LinkedList<Task>();
            Task tas = (Task)Activator.CreateInstance(task.GetType());
            tasks.AddFirst(tas);

            while (true)
            {
                Type typeTask = tas.GetType();
                bool haveTask = typeTask.IsDefined(typeof(DependsOnAttribute), false);
                if (haveTask)
                {
                    DependsOnAttribute t = (DependsOnAttribute)typeTask.GetCustomAttributes(typeof(DependsOnAttribute), false)[0];
                    tas = (Task)Activator.CreateInstance(t.Type);
                    typeTask = tas.GetType();
                    if (tas != null)
                        tasks.AddFirst(tas);
                }
                else
                    break;
            }
                foreach (Task t in tasks)
                {
                    MethodInfo mf = t.GetType().GetMethod("Run");
                    if (mf != null)
                    mf.Invoke(t, null);
                }

            }
        }
    #endregion
       
    class Program
        {
            //6
            public static Func<K, K> ComposeIf<K>(Func<K, K> f1, Func<K, K> f2, Predicate<K> p)
            {
                return x =>
                {
                    if (p(x))
                        return f2(f1(x));
                    else
                        return x;
                };
            }
           
        static void Main(string[] args)
            {

                TaskRunner.Submit(new Task1());
                Func<int, int>[] array = { a => a * 2, a => a + 3 };
                Predicate<int> p = a => a % 2 != 0;
                Func<int, int> k = ComposeIf(array[0], array[1], p);
                Console.WriteLine(k(1));
                Console.WriteLine(k(2));


            }

        }
    
}
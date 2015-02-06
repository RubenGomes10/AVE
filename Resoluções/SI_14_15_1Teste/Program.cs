using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SI_14_15_1Teste
{

    class Student
    {
        [Formatter(typeof(NameFormatter))]
        public string Name { get; set; }

        [Formatter(typeof(BirthDateFormatter))]
        public DateTime BirthDate { get; set; }

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatterAttribute : Attribute
    {
        public FormatterAttribute(Type t)
        {
            this.TypeFormat = t;
        }
        public Type TypeFormat { get; set; }

    }

    public interface IFormatter<T> 
    {
         T Format(T t);
    }

    public class NameFormatter : IFormatter<string>
    {
        

        public string Format(string scr)
        {
            return scr.ToLower();
        }
    }

    public class BirthDateFormatter : IFormatter<DateTime>
    {

        public DateTime Format(DateTime t)
        {
            return t;
        }
    }

    class Format
    {
        public static string FormatProps(object src)
        {
            Type t = src.GetType();
            string res = "{";
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.IsDefined(typeof(FormatterAttribute), false)) 
                {
                    FormatterAttribute at = (FormatterAttribute)pi.GetCustomAttributes(typeof(FormatterAttribute), false)[0];
                    object formater = (object)Activator.CreateInstance(at.TypeFormat);
                    Type formatType = formater.GetType();
                    MethodInfo mi = formatType.GetMethod("Format");
                    res += mi.Invoke(formater, new object[] { pi.GetValue(src) })+ ",";
                }
            }
            res += "}";
            return res;
        }
    }
        public delegate void Observer(DateTime current);

    public class Alarm
    {
        private event Observer obs; //d sem alterações e-- melhor event pois quando é accionado o trigger ele invoca todos os membros
                                          // é desnecessario o NotifyAll a percorrer a lista de invocationList melhoria de desempenho
        public void AddObserver(Observer o) { obs +=o; } //c
        public void RemoveObserver(Observer o) { obs -= o; } // c
        
        public void NotifyAll(DateTime current)
        {
            foreach (Observer o in obs.GetInvocationList()) //c
                o(current);//a)
        }

        public void Start(long timeoutInMilis)
        {
            Stopwatch watch = Stopwatch.StartNew();
            do { } while (watch.ElapsedMilliseconds < timeoutInMilis);
            //NotifyAll(DateTime.Now);
            obs(DateTime.Now);
        }
    }

    class ConsoleWriter 
    {
        public void Notify(DateTime current)
        {
            Console.WriteLine("current : " + current);
        }

        public void Notify2(DateTime current)
        {
            Console.WriteLine("Current 2 : " + current);
        }
    }




    static class Program
    {
        public static IEnumerable<T> MyFilter<T>(this IEnumerable<T> src, Func<T, bool> p)
        {
            int second = 2;
            foreach (T item in src)
            {
                if (second - 1 > 0)
                {
                    Console.WriteLine(item + " ");
                    --second;
                }
                if (p(item))
                {
                    if (second > 0)
                    {
                        Console.WriteLine(item + " ");
                        --second;
                    }
                    yield return item;
                }
            }
        }

        public static IEnumerable<R> MyMap<T,R>(this IEnumerable<T> src, Func<T, R> f)
        {
            bool first = true;

            foreach (T item in src)
            {
                if (first)
                {
                    first = false;
                    Console.WriteLine(item + " ");
                }
                yield return f(item);
            }

        }

        static void Main(string[] args)
        {
            //Student s = new Student();
            //s.Name = "jose PAULITO";
            //s.BirthDate = new DateTime(1974, 6, 7);
            //Console.WriteLine(Format.FormatProps(s));

            //Alarm a = new Alarm();
            //b
            //Observer b = new ConsoleWriter().Notify;
            //a.AddObserver(b);
            //a.AddObserver(new ConsoleWriter().Notify2);
            //a.Start(2000);

            IEnumerable<int> l = new List<int> { 1, 2, 3, 4, 5, 6 };

            int r = l
                    .MyFilter(n => n % 2 == 0)
                    .MyMap(n => n + 1)
                    .First();
            Console.WriteLine(r);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SV_12_13_1Teste
{
    

    #region Question 1
    //1 R : Dá true se a referencia tiver hereditariedade ou seja se T1 extender uma classe filha de T2 ou se T2 estender uma classe ficha de T1.
    class Base { }

    class T2 : Base { }

    class T1 : T3 { }

    class T3 : T2 { }

    class T4 : Base { }

    #endregion

    #region Question 3

    public delegate void CapacityLevel(object sender, int level);

    public interface ICapacity
    {
        event CapacityLevel MaxLevel;
    }

    public class A : ICapacity
    {
        private CapacityLevel MaxLevelDelegate;

        protected virtual void OnMaxLevel(int level)
        {
            Console.WriteLine("trigger Max Level in A");
            if (MaxLevelDelegate != null)
                MaxLevelDelegate(this, level);
        }

        public bool isMax(int level)
        {
            return level > 30;
        }

        public void Simulatelevel(int level)
        {
            if (isMax(level)) // firts level = 60 -- true
                OnMaxLevel(level); // class B overrides this method so jump to OnMaxLevel in class B
        }

        public event CapacityLevel MaxLevel
        {
            add
            {
                Console.WriteLine("Regist {0} on A", value.Method.Name);
                MaxLevelDelegate += value;
            }

            remove
            {
                MaxLevelDelegate -= value;
            }
        }
    }
    //change to class B : A, ICapacity for b)
    class B : A, ICapacity
    {
        public event CapacityLevel MaxLevel;

        public bool isMax(int level)
        {
            return level < 50;
        }

        protected override void OnMaxLevel(int level)
        {
            Console.WriteLine("trigger Max Level in B");
            if (MaxLevel != null) // invocation list have 2 handlers1
                MaxLevel(this, level);// the same of trigger all delegates in this invokationList
        }
    }

    #endregion

    #region Question 4

    public struct S
    {
        public int Val { get; set; }

        public override string ToString()
        {
            return Val.ToString();
        }

    }

    #endregion


       public static class MainClass
        {
            public static void Handler1(object sender, int level) // call 2x
            {
                Console.WriteLine("Handler with level {0}", level);

            }

            public static void Handler2(object sender, int level)
            {
                Console.WriteLine("Handler2 with level {0}", level);
            }

            //a) not possible T? so just T(object) returns a value Nullable so we need do invoke toString for the method CompareTo
            public static void ShowMax<T>(ref T t1, ref T t2)
            {
                if (t1 == null || t2 == null) return;
                Console.WriteLine(t1.ToString().CompareTo(t2.ToString())
                                        > 0 ? t1 : t2); // unbox
            }

            #region Question 5


            public static Double Sum(this IEnumerable<Double> source)
            {
                Double prices = 0;
                foreach (Double price in source)
                    prices += price;
                return prices;
            }

           //answer b)
            public static Double Sum<T>(this IEnumerable<T> source, Func<T, Double> selector)
            {
                Double prices = 0;
                foreach (T t in source)
                    prices += selector(t);
                return prices;
            }

            public static void TestPrices()
            {
                var products = new[]{
                                     new {IdProd = 1, Price = 10.2},
                                     new {IdProd = 2, Price = 0.0},
                                     new {IdProd = 3, Price = 5.7}
                                    };
                var prices = products.Select(p => p.Price);
                var total = Sum(prices);
                Console.WriteLine(total);

                //answer a)
                total = products.Sum(p => p.Price); 
                Console.WriteLine(total);
                //correct
                total = Sum(products, p => p.Price); 
                Console.WriteLine(total);
            }

            #endregion

            #region Question 6

           //a)
           [AttributeUsageAttribute(AttributeTargets.Constructor | AttributeTargets.Method)]
           class StereoTypeAttribute : Attribute
           {
               public StereoTypeAttribute()
               {
                   
               }

               public string Category { get; set; }
               public string Description { get; set; }

           }

           struct Pair<T,U>
           {
               public T first;
               public U second;
           }

           class exe6
           {
               //b
               public static IEnumerable<MemberInfo> ListStereotipedElements<T>()
               {
                   T obj = (T)Activator.CreateInstance(typeof(T));
                   Type obj1 = obj.GetType();

                   foreach (MemberInfo mf in obj1.GetMembers())
                       foreach (object attribute in mf.GetCustomAttributes(true))
                           if (attribute is StereoTypeAttribute)
                               yield return mf;
               }

               //c

               public static IEnumerable<Pair<string,T>> CreatFromCateg<T>(string [] cats){
                   T obj = (T)Activator.CreateInstance(typeof(T));
                   Type obj1 = obj.GetType();

                   foreach(string s in cats)
                       foreach(MemberInfo mf in obj1.GetConstructors())
                           foreach(object attribute  in mf.GetCustomAttributes(true)){
                               Pair<string,T> p = new Pair<string,T>();
                               if (attribute.GetType().GetProperty("Category").GetValue(attribute).Equals(s))
                               {
                                   p.first = s;
                                   p.second = (T)Activator.CreateInstance(typeof(T));
                                   yield return p;
                               }
                           }

               }

           }


           //Test ListStereotipedElements
           class TestListStereotipedElements
           {
               [StereoType(Category = "Modifier",Description="Change Customer name")]
               public TestListStereotipedElements()
               {

               }

               [StereoType(Category = "Teste method")]
               public  void TestMethodStereoTiped() { }


           }

           //Test for C)

           class Categorized
           {
               [StereoType(Category = "cat1")]
               public Categorized() { }

               [StereoType(Category = "cat2")]
               public Categorized(DateTime time){ }

               [StereoType(Category = "Cat4")]
               public Categorized(String s) { }
               
 
           }



            #endregion



            static void Main(string[] args)
            {

                #region Teste Question 1

                Console.WriteLine("--------------------------Teste Question 1--------------------");
                T3 r = new T3();

                Console.WriteLine((r is T1) && (r is T2)); //false
                r = new T1();
                Console.WriteLine((r is T1) && (r is T2)); // true
                #endregion

                #region Teste Question 3

                Console.WriteLine("--------------------------Teste Question 3--------------------");
                B b = new B();
                A a = b;
                ICapacity c = a;

                b.MaxLevel += Handler1; // b have Handler1 
                a.MaxLevel += Handler2; // a have handler2 but references b so have handler1 to
                //output -> Regist Handler2 in A
                b.MaxLevel += Handler1;// b have a base event handler2 and 2 handler1 in invocation list

                b.Simulatelevel(60);
                //output-- For a)
                //trigger Max Level in B
                //Handler with level 60
                //Handler with level 60

                // for b) the same

                a.Simulatelevel(40);
                //For a)
                //trigger Max Level in B
                //Handler with level 40
                //Handler with level 40

                //for b) the same 
                // class B ja tem a interface implementada aquando cria a assinatura do evento MaxLevel, deste modo estar 
                //explicito ou nao na assinatura da class que ela implementa a interface ICapacity é dispensavel.

                #endregion

                #region Teste Question 4
                Console.WriteLine("--------------------------Teste Question 4--------------------");

                S? s1 = new S { Val = 2 }, s2 = new S { Val = 3 };
                ShowMax<S?>(ref s1, ref s2); //box

                #endregion

                #region Teste Question 5
                Console.WriteLine("--------------------------Teste Question 5--------------------");
                
                TestPrices();
         
                #endregion

                #region Teste Question 6

                Console.WriteLine("--------------------------Teste Question 6--------------------");
                //
                //b)
                //
                Console.WriteLine("--------------------------Teste Question 6 b)--------------------");
                var members =  exe6.ListStereotipedElements<TestListStereotipedElements>();

                foreach (MemberInfo mf in members)
                    Console.WriteLine("Name method - " + mf.Name + " | MemberType - " + mf.MemberType);

                //
                //c)
                //

                Console.WriteLine("--------------------------Teste Question 6 c)--------------------");
                string [] cats = new []{"cat1","cat2","cat3"};
                var pairs = exe6.CreatFromCateg<Categorized>(cats);

                foreach (var p in pairs)
                    Console.WriteLine("Categoria ->" + p.first + " | Instancia ->" + p.second);

                #endregion
            }
        }

}


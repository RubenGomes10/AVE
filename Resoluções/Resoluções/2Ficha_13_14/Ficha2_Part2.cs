using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Resoluções._2Ficha_13_14
{

    public class TestClass
    {
        [DumperValue(Format = "int"),]
        public readonly string fieldText = "Test BindTo Sucess";

        public int fieldText2;

        public TestClass()
        {
            this.Text = "Teste GetValues";
        }

        public string Text { get; set; }

        [DumperValue()]
        public int SomeInt { get; set; }

        [DumperValue(Format = "int")]
        public double SomeDouble { get; set; }

      
        public override string ToString()
        {
            return Text;
        }
    }
    //Class Pair For GetValues and BindTo
    class Pair<T1, T2>
    {

        public Pair(T1 t1, T2 t2)
        {
            this.Obj1 = t1;
            this.Obj2 = t2;
        }

        public T1 Obj1 { get; set; }
        public T2 Obj2 { get; set; }

        public override string ToString()
        {
            return Obj1 +" "+ Obj2;
        }
    }

    

    [AttributeUsageAttribute(AttributeTargets.Property|AttributeTargets.Field,Inherited = true, AllowMultiple = true)]
    class DumperValueAttribute : Attribute
    {

        public DumperValueAttribute()
        {
            this.Format = "String";
        }

        public String Format { get; set; }
        
    }


    class Formarter : IFormatter{

        public Formarter(string formatTo)
        {
            this.FormatTo = formatTo;
        }

        public string FormatTo { get; set; }

        public SerializationBinder Binder
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public StreamingContext Context
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object Deserialize(System.IO.Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(System.IO.Stream serializationStream, object graph)
        {
            throw new NotImplementedException();
        }

        public ISurrogateSelector SurrogateSelector
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    public class TestClassBindTo
    {
        public string fieldText;

        public int SomeInt { get; set; }

        public double SomeDouble { get; set; }
    }

     static class MAinClass{

        public static IEnumerable<Pair<string, object>> GetValues(object obj)
        {
            Type obj1 = obj.GetType();
            
            //for fields
            foreach (FieldInfo fi in obj1.GetFields())
                foreach(object attribute in fi.GetCustomAttributes(true))
                    if(attribute is DumperValueAttribute)
                        yield return new Pair<string, object>(fi.Name, fi.GetValue(obj));

            //for Prop
            foreach (PropertyInfo pi in obj1.GetProperties())
                    foreach(object attribute in pi.GetCustomAttributes(true))
                        if(attribute is DumperValueAttribute)
                            yield return new Pair<string, object>(pi.Name,pi.GetValue(obj));
        }

        public static T BindTo<T>(this IEnumerable<Pair<string,object>> values)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            Type obj1 = obj.GetType();

            foreach (Pair<string, object> p in values)
            {
                FieldInfo fi = obj1.GetField(p.Obj1);
                if (fi != null)
                    fi.SetValue(obj, p.Obj2);
            }

            foreach (Pair<string,object> p in values)
            {
                PropertyInfo pi = obj1.GetProperty(p.Obj1);
                if (pi != null)
                    pi.SetValue(obj, p.Obj2);
            }

            return obj;

        }


        static void Main(){

            //Teste GetValues
            TestClass text = new TestClass();

            IEnumerable<Pair<string, object>> e = GetValues(text);

            IEnumerator<Pair<string, object>> it = e.GetEnumerator();

            while (it.MoveNext())
                Console.WriteLine(it.Current);


            //Teste BindTo
            TestClassBindTo text2 = BindTo<TestClassBindTo>(e);

            foreach (FieldInfo fi in text2.GetType().GetFields())
                Console.WriteLine(fi.Name + " " + fi.GetValue(text2) + "--Sucess Bind");

            foreach (PropertyInfo pi in text2.GetType().GetProperties())
                Console.WriteLine(pi.Name + " " +pi.GetValue(text2)+ "--Sucess Bind" );
           
                                              
        }
    }

}






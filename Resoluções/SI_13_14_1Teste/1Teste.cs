using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SI_13_14_1Teste
{

        #region Question 1
    /*  Tipo valor-> é chamado o initobj que posteriormente chamada o construtor por omissao sem parametros iniciando 
     * os parametros a 0 no stack
    
     * Tipo referencia -> é chamado o newObj que aloca no heap espaço necessario preciso para tipo e  de seguida é chamado o construtor, 
     * */
    #endregion

        #region Question 2
    /*  Tipo valor é chamado um call devido a nao ser necessario alojar no heap. Por outro o tipo valor não tem this o que por si
     * só não gera um callvirt e sendo o método GetType() um metodo estatico estamos presentes de um despacho estatico, acede directamente 
     * ao endereço onde se encontra alocado.
     * 
     * Tipo referencia é chamado um callVirt devido a que os tipos referencia terem a sua propria referencia , nomeadamente o this, porem
     * GetType() é um metodo estatico, mas como estamos a aceder a ele com uma referencia é necessario haver despacho dinamico gerando 
     * assim um callVirt.
     * 
    */

    #endregion

        #region Question 3

    public class X
    {
        public delegate void MyHandler();
        public event MyHandler MyEvent;

        /* É gerado o construtor por omissão da class A
         * Um delegate do tipo void
         * Um evento do tipo MyHandler
         * Um metodo add pertencente ao evento
         * Um metodo remove pertencent ao evento
         * 
         */
    }

    #endregion

        #region Question 4

        delegate void MyHandler();

        static class Handlers
        {
            public static void FooA() { }
            public static void FooB() { }

        }
        



    #endregion

        #region Question 5
            

 

    #endregion

        #region Question 6
        class Y
        {
            public delegate void MyHandler();
            public event MyHandler MyEvent
            {   /* Erro loop infinito
                add { MyEvent +=value;}
                remove { MyEvent -= value;}
                */ 
                //Soluçao
                add { observer += value; }
                
                remove { observer -= value; }
            }
            //soluçao

            private MyHandler observer;


            //Exemplo de disparar evento
            public void SimulateEvent()
            {
                if (observer != null)
                    observer();
            }

        }
            





        #endregion

        #region Question 7
        interface I
        {
            void M();
        }

        class A : I
        {
             public new void M()
            {
                Console.WriteLine("A");
            }
        }
        class B : A
        {
            public virtual void M() 
            {
                Console.WriteLine("B");
            }
        }
        class C : B
        {
            public override void M()
            {
               Console.WriteLine("C");
            }
        }
        /* a) É chamado o método da interface explicitamente , isto é  como a ultima implementação do metodo M da interface é na class A 
         * ele executa o metodo m escrevendo A , metodo invocado explicitamente. Poderiamos ter I.M(){ Console.WriteLine("A explicit")
         * era este que era chamado!
         * 
         * 
         * b) Ambas vao dar, no caso de B: A,I este implementa um novo metodo M da interface dizendo que é virtual que por sua vez é redefinido 
         * pela class C. No caso de C : B, I a instancia é de C e o metodo M da interface é chamado explicitamente nest caso faz override 
         * imprimindo C.
         */

        #endregion

        #region Question 8
        struct PropPair
        {
            public readonly string name;
            public readonly object value;
            public readonly Type type;

            public PropPair(string name, object value, Type type) { 
                this.name = name; 
                this.value = value; 
                this.type = type; 
            }

        }
        static class ex8
        {
            //a)
            public static IEnumerable<PropPair> GetPropValues(this object obj)
            {
                Type t = obj.GetType();

                foreach(PropertyInfo pi in t.GetProperties())
                    yield return new PropPair(pi.Name, pi.GetValue(obj), pi.PropertyType);
            }

            //b)

            public static IEnumerable<PropPair> CompatibleWith(this IEnumerable<PropPair> values, Type t)
            {
                foreach (PropPair p in values)
                    if (p.type.Equals(t))
                        yield return p;
            }
        }

        class Student
        {
            public Student(string name, int id, string course)
            {
                this.Name = name;
                this.Id = id;
                this.Course = course;
            }

            public string Name { get; set; }
            public string Course { get; set; }
            public int Id { get; set; }

        }
        #endregion


        public static class MainClass
    {
            static void Foo() { Console.WriteLine("Foo"); }




        static void Main(string[] args)
        {

            #region Teste Question 4
            MyHandler fA = Handlers.FooA; 
            MyHandler fB = Handlers.FooB;
            MyHandler h = fA; // h fica com a referencia para o delegate FooA
            h += fB;    // fica com h fica com uma invocationList que contem 2 delegates FooA e FooB sendo o primeiro FooA
            h -= fA;    // remove da invocationList o delegate FooA ficando h referenciado o delegate FooB
            Console.WriteLine(object.ReferenceEquals(h, fB)); // true

            #endregion

            #region Teste Question 6
            Y b = new Y();
            b.MyEvent += Foo;

            // da StackOverFlowException -- entra num loop infinito pois acrescenta ao proprio evento e nao a um observador disparando novamente.
            b.SimulateEvent();


            #endregion

            #region Teste Question 7

            I i = new C();
            i.M();

            #endregion

            #region Teste Question 8
           
            Student s = new Student("Jose", 127, "LEIC");
            IEnumerable<PropPair> values = ex8.GetPropValues(s);
            
            //Teste get All properties
            Console.WriteLine("-------------------All Properties in Student-------------------");
            Console.WriteLine();
            foreach (PropPair p in values)
                 Console.WriteLine("PropertyName -> {0} | PropertyValue -> {1} | PropertyType -> {2}", p.name, p.value, p.type);

            Console.WriteLine();
            //Teste properties types compatible with string
            Console.WriteLine("-------------------All Properties with Type String in Student-------------------");
           
            IEnumerable<PropPair> valuesTypeString = ex8.CompatibleWith(values, typeof(string));
            
            foreach (PropPair p in valuesTypeString)
                 Console.WriteLine("PropertyName -> {0} | PropertyValue -> {1} | PropertyType -> {2}", p.name, p.value, p.type);

            Console.WriteLine();
            //Teste properties types compatible with int
            Console.WriteLine("-------------------All Properties with Type int in Student-------------------");
            Console.WriteLine();
            IEnumerable<PropPair> valuesTypeInt = ex8.CompatibleWith(values, typeof(int));
            
            foreach (PropPair p in valuesTypeInt)
                Console.WriteLine("PropertyName -> {0} | PropertyValue -> {1} | PropertyType -> {2}", p.name, p.value, p.type);

            Console.WriteLine();
            //Teste properties types compatible with Double
            Console.WriteLine("-------------------All Properties with Type Double in Student-------------------");
            IEnumerable<PropPair> valuesTypeDouble = ex8.CompatibleWith(values, typeof(double));
           
            foreach (PropPair p in valuesTypeDouble)
                Console.WriteLine("PropertyName -> {0} | PropertyValue -> {1} | PropertyType -> {2}", p.name, p.value, p.type);
           

            #endregion

        }
    }
}

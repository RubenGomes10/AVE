using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resoluções._2Ficha_13_14
{
    //1)
    delegate void MyHandler();

     class Handlers
    {
        public void FooA3()
        {
            Console.WriteLine("A");
        }

        public static void FooA()
        {
            Console.WriteLine("A");
        }
        public static void FooA2() { }

        public static void FooB()
        {
            Console.WriteLine("B");
        }
        public static void FooB2() { }

        public static void Foo() 
        {
            Console.WriteLine("Test Foo");
        }
    }

   


    class A
    {   //5
     //   public delegate void MyHandler();
     //  public event MyHandler MyEvent;

        //R: event, delegate e um metodo Add e remove por default que adiciona e remove observadores.

        //6

        public event MyHandler MyEvent
        {
            add { observer += value; }
            remove { observer -= value; }
        }
        //solucçao
        private MyHandler observer;

    }

        

    

            



    static class App
    {
        //8
        //- Implemente o método de extensão Split(this String s, char sep) que retorna um IEnumerable<string> 
        // contendo as substrings do parâmetro ‘s’ separadas pelo caracter ‘sep’. 
        //- NÃO pode reutilizar o método Split de String. SÓ pode reutilizar os métodos IndexOf(char value, int startIndex) 
        // e Substring (int startIndex, int length) da classe String.


        public static IEnumerable<string> Split(this string s, char sep)
        {

            int index = 0;
            while( s.Length != -1)
            {
                index = s.IndexOf(sep,0);
                if (index == -1)
                {
                    yield return s.Substring(0);
                    break;
                }
                else
                    yield return s.Substring(0, index);

               s = s.Substring(index + 1);
             
                
            }
           
            
        }
            
        

        static void Main()
        {
            //1
            MyHandler h = new MyHandler(Handlers.FooA);
            MyHandler fb1 = new MyHandler(Handlers.FooB);
            MyHandler fb2 = new MyHandler(Handlers.FooB);
            h = (MyHandler)Delegate.Combine(h, fb1); // A,B
            h = (MyHandler)Delegate.Combine(h, fb2);// A,B,B
            h = (MyHandler)Delegate.Remove(h, fb1);// A,B
            h = (MyHandler)Delegate.Remove(h, fb1);// A
            h();//A
            //R- tanto fb1 como fb2 referenciam o mesmo delegate o que basta que um deles seja a referencia
            // quando se remove pela segunda vez o fb1, na verdade estasse a remover o delegate ao qual ele referencia.

            //2
            MyHandler fA = Handlers.FooA2;
            MyHandler fB = Handlers.FooB2;
            h = fA; // fA
            h += fB; // fA e fB
            h -= fA;    //fB
            Console.WriteLine(Object.ReferenceEquals(h, fB));

            //R: da true pois h tem a mesma referencia que fB para o especifico delegate  

            //3
            h = null;
            MyHandler f = new MyHandler(Handlers.Foo);
            h += f;
            //h();
            //R: h vai ficar com a referencia criada pela instancia de f
        
            //6
            A a = new A();
            a.MyEvent += Handlers.Foo; // StackOverFlowException-- ao adicionar ele chama novamente recursividade infinita
             
            //7
            h = new MyHandler(new Handlers().FooA3); //A
            h+= new MyHandler(Handlers.FooB); //A e B
            h-= new MyHandler(new Handlers().FooA3); // A e B devido a nova instancia referenciar um novo objecto ao contrario de referencias estaticas
            h -= new MyHandler(Handlers.FooB); // como B é estatico referencia o mesmo delegate logo remove ficando A
            h(); //A

            //teste Split(this string s , char sep);

            string test = "Sucess,Test,Split",
                   test2 = "Teste Bem sucedido";
            IEnumerable<string> split =  Split(test, ','),
                                split2 = Split(test2, ' ' ); 
            
            foreach (string s in split) 
                Console.WriteLine(s);

            foreach (string s in split2)
                Console.WriteLine(s);



 
        
        }

    }


}

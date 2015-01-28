using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resoluções._1Ficha_13_14
{
    

        //1. No contexto do desenvolvimento de aplicações Unmanaged, a reutilização de componentes (e.g. DLL) 
        //    obriga à utilização de um ficheiro adicional/complementar ao componente. Qual é?
            //R : header (ficheiro .h)
       
        //2. Qual a responsabilidade deste ficheiro no processo de compilação?
        //R: Referenciar / especificar  assinatura dos métodos e a sua metadata. com a ajuda do .lib.  e .obj

        //3 . Porque é que na plataforma .Net é dispensada a utilização deste ficheiro? Quem substitui a sua responsabilidade?
            //R: é utilizdo bibliotecas dinãmicas nomeadas de .dll, que contem toda a metadata e assinatura dos métodos/tipo/ informação da classe

        //4 . A característica indicada na resposta 3 foi pioneira na plataforma .Net e não existia noutros ambientes, ex: Java?
          //  R: Sim foi pioneira do dot.net , java utiliza o ficheiro .class para guardar / fornecer esses dados.

        //5 . Quais as duas responsabilidades da instrução IL newobj do CLI?
            // R: chamar o construtor de uma instancia neste caso tem que ser um tipo referencia depois de alocar no heap o this + #sync + "htype" +
            // parametros necessarios. 

        //6 .Qual a principal diferença da instrução IL newobj para o seu equivalente em Java?
                // r : neste caso necessitamos de alocar o this no stack e invocar o newobj, no caso do java não é necessario.

        //7 .Qual o resultado da tradução da definição de um namespace pelo compilador de C#?
            // equivalente a um package em java

        //8. Justifique como verdadeira, ou falsa, a seguinte afirmação: “As variáveis de tipo referência ocupam 
        //  o mesmo espaço em memória independentemente do seu tipo”.
        //  R: falso, ocupam sim uma dimensao fixa , neste caso 8 bytes( 4 de "sync" e 4 de "htype" ) + o this e os parametros que forem necessarios
        //  /metodos pertencentes a esse tipo.


        //9.Explique qual o resultado em IL da compilação de um casting implícito. Ex: String str = …; Object o = str;
            // ldstr "x"
            // stloc.0  // String str = "x";
            // ldloc.0  //carrega no stack str
            // stloc.1  // object o = str; --  cast implicito pois sao ambos tipos referencia.

        //Justifique por qual das alternativas seguintes optaria em termos de desempenho:
        //A: String s; if(r is String){s = (String) r; ...} B: String s = r as String; if(s != null){...}
        // R: B, pois executa um cast para string e basta verificar se é null ou nao, enquanto que no A 
            // iria verificar nas suas classes base se realmente r era string.
        
        //11.Qual a diferença do código IL resultante da compilação de new A(7), consoante A seja um tipo valor ou referência?
        // R: Diferencas:
        //tipo valor -> executa um initobj que chama o seu construtor por omissao, colocando na stack e iniciando os valores a 0.
        //tipo referencia -> executa um newobj onde é alocado espaço no heap para A e de seguida é chamado o seu constructor.


        //12.   Sendo p uma variável do tipo Point, então consoante Point seja um tipo valor, ou referência, 
        //a compilação de: p.GetType(), gera um call ou um calvirt respectivamente. Justifique:
        // R:   Dado que o método GetType() é não virtual estamos presentes de um despacho estático. 
            // se p é tipo valor-> basta um call pois ele acede ao seu endereço especifico.
            // se p é do tipo refencia -> é chamado um call virt pois p é alocado no heap e necessita do seu this para invocar o getType().

        //13. Existe mais alguma diferença no código IL gerado na questão 12, para além do já indicado? Justifique.
            //R:

        //14. Sendo a uma variável do tipo referência A, justifique qual a diferença do resultado das expressões:
        //Expressão 1: typeof(A).Equals(a.GetType()) Expressão 2: typeof(A) == a.GetType()
        //R:    1- Compara igualdade(referencias) 2-Compara identidade ( se é o proprio)


/*17.
   .class public auto App{
 * .method private static int32 m(int32 n) {
        ldarg.0
        ldc.i4.1
        bne.un.s TARGET_01
        ldc.i4.1
        ret
        TARGET_01:
        ldarg.0
        ldarg.0
        ldc.i4.1
        sub
        call int32 App::m(int32)
        mul
        ret
    } // end of method App::m
*/
    public class ex17
    {
        private static int M(int n)
        {
            if (n == 1) return 1;
            n -= 1;
            return n * M(n);
        }
    }

    //18)18. Qual o resultado da execução do programa App:

/*
    class A 
    {
        public virtual void m() 
        { 
            Console.WriteLine("A"); 
        }
    }
    class B : A 
    { 
        public override void m() 
        {
            Console.WriteLine("B"); 
        }
    }
    
    class C : B 
    { 
        public virtual new void m() 
        {
            Console.WriteLine("C"); 
        }
    }
    
    class D : C 
    { 
        public override void m() 
        {
            Console.WriteLine("D"); 
        } 
    }
    
    class App { 
     
        static void Main() 
            {
                A a = new D();
                a.m();     //   (B) -- B redefine o metodo m de A ,embora seja uma instancia de D a referencia é A. C implementa um novo metodo m
                ((B)a).m();//   (B) -- metodo C implementa um novo m
                ((C)a).m();//   (D) -- método C implementa um novo m e D redefine  
                ((D)a).m();//   (D) -- instancia de D invoca o proprio metodo m da sua class
            }
        }
  */  
    //19)
    
    /*
    interface I 
    { 
        void m(); 
    }
    
    class A : I 
    {
        public virtual void m() 
        { 
            Console.WriteLine("A"); 
        }
     
    }
    class B : A
    {
        public override void m()
        {
            Console.WriteLine("B");
        }
    }
    class C : B, I
    {
        void I.m()
        {
            Console.WriteLine("C");
        }
    }
    class D : C
    {
        public override void m()
        {
            Console.WriteLine("D");
        }
    }
    class App
    {
        static void Main()
        {
            ((I)new A()).m(); // (A) instancia de A mas referencia a I como A implementa I resulta na chamada do método m
            ((I)new B()).m(); // (B) instancia de B faz override do metodo m cuja referencia é I da class A
            ((I)new C()).m(); // (C) chama o método privado de I da class C
            ((I)new D()).m(); // (C) chama o método privado de I da class C 
            A a = new D();
            I i = a;
            a.m();  //(D)
            i.m();  //(C)
            a = new C();
            a.m(); // (B)
            i = a;
            i.m(); //(C)

        }
    }
    */
}


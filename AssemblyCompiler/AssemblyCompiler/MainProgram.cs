using AssemblyCompiler.Utilitys;
using AssemblyCompiler.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCompiler
{
    class MainProgram
    {
        List <CompilerAssembler> listTest = new List <CompilerAssembler> (); //list para cada linha de codigo digitada - alterar nome
       
        //ViewModelMainWindow view = new ViewModelMainWindow();

        public void main(ViewModelMainWindow view, IList<string> codeAreaByLine)
        {
            listTest.Clear();
            view.ResultsArea = null;
            foreach (var line in codeAreaByLine)
            {
                //var test = codePerLine[i];
                var splitedTest = line.Split(' '); //cria um array separado por " ", para cada linha
                int j = 0;
                if (j + 3 < splitedTest.Length)
                {
                    CompilerAssembler newCommand = new CompilerAssembler();
                    newCommand.assembly = newCommand.DetermineType(splitedTest[j]);
                    newCommand.languageBinary = newCommand.registerToBin(splitedTest[j + 1]);
                    newCommand.languageHexa = splitedTest[j + 2];
                    newCommand.controlSingal = splitedTest[j + 3];

                    listTest.Add(newCommand);
                    //instancia uma classe do compilador apra cada linha - alterar construtor e criar metodos de verificação do que foi digitado
                    //criar metodo na classe CompilerAssembler para gerar os resultados na list
                }
            }
            view.ResultsArea = listTest; //joga a list para o GridView da tela, que mostrara cada atributo da classe CompilerAssembler linha por linha digitada
        }
    }
}

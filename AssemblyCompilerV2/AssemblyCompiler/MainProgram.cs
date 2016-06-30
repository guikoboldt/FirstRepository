using AssemblyCompiler.Utilitys;
using AssemblyCompiler.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AssemblyCompiler
{
    class MainProgram
    {
        List <CompilerAssembler> listTest = new List <CompilerAssembler> (); //list para cada linha de codigo digitada - alterar nome

        public void main(ViewModelMainWindow view, IList<string> codeAreaByLine)
        {
            listTest.Clear();
            bool checkCommand = true;
            int count = 1;
            view.ResultsArea = null;
            foreach (var line in codeAreaByLine)
            {
                //var test = codePerLine[i];
                var splitedTest = line.Split(' '); //cria um array separado por " ", para cada linha
                if (splitedTest.Length > 1) //cada linha precisa ter mais de 1 "variavel"                   
                    {                        
                        CompilerAssembler newCommand = new CompilerAssembler();
                        newCommand.assembly = line;
                        var bin = newCommand.lineToBin(line, count);
                        if (bin.Length!=32)
                        {
                            checkCommand = false;
                            break;
                        }                        
                        newCommand.languageBinary = bin;
                        newCommand.languageHexa = newCommand.convertToHex(bin);
                        newCommand.controlSingal = newCommand.getControlSignal(line);

                        listTest.Add(newCommand);
                        //instancia uma classe do compilador apra cada linha - alterar construtor e criar metodos de verificação do que foi digitado
                        //criar metodo na classe CompilerAssembler para gerar os resultados na list
                        count++;
                    }
                else
                {
                    checkCommand = false;
                    break;
                }                
            }
            if (checkCommand)
            {
                view.ResultsArea = listTest; //joga a list para o GridView da tela, que mostrara cada atributo da classe CompilerAssembler linha por linha digitada
            }
            else
            {
                MessageBox.Show("Erro na linha " + count);
            }            
        }
    }
}

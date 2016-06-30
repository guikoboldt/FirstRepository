using AssemblyCompiler.Utilitys;
using AssemblyCompiler.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AssemblyCompiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainProgram test = new MainProgram(); //mainprogram class usada para chamar o main
        ViewModelMainWindow view = new ViewModelMainWindow(); //cria uma view model para interagir os componentes
        List<string> codeAreaByLine = new List<string>(); //list para registrar linha por linha do textBox

        public MainWindow()
        {
            this.DataContext = view; //indica que a view model controle os componentes
            InitializeComponent();
        }

        private void codeArea_PreviewKeyDown(object sender, KeyEventArgs e)
        { //evento que avalia cada letra digitada, serve para limitar o numero de caracteres de cada linha
            int tamanhoMaximoPorLinha = 29; //numero maximo de caracteres - 30 caracteres maximo (zero based)
            int[] keysLiberadas = { (int)Key.Enter, (int)Key.Back }; //teclas liberadas para quando alcançar o maximo de char

            int posicaoAtual = codeArea.SelectionStart;
            int linhaAtual = codeArea.GetLineIndexFromCharacterIndex(posicaoAtual);

            if (codeArea.GetLineLength(linhaAtual) > tamanhoMaximoPorLinha && !keysLiberadas.Contains((int)e.Key))
            {
                e.Handled = true; //indica que a tecla digitada e invalida
            }
        }

        private void readLine ()
        {
            codeAreaByLine.Clear();
            var lines = codeArea.GetFirstVisibleLineIndex();
            while (lines <= codeArea.GetLastVisibleLineIndex())
            {
                codeAreaByLine.Add(codeArea.GetLineText(lines));
                lines++;
            }
        }

        private void compile_Click(object sender, RoutedEventArgs e)
        { //pressionar botao, que chama o main da MainProgram
            readLine(); //faz a leitura linha por linha
            test.main(view, codeAreaByLine); //chama o metodo main, que testa as linhas de codigo
        }
    }
}

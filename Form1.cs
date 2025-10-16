using System;
using System.Data.SQLite;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static OpenTK.Graphics.OpenGL.GL;
using static SkiaSharp.HarfBuzz.SKShaper;
namespace Estudo_Instramed43
{
    public partial class Form1 : Form
    {
        //coloquei variavel global
        string path = @"C:\Users\Douglas.beccari\OneDrive - Strattner\Área de Trabalho\Instramed_Estudo_C#\Estudo_Instramed43 GráficoWalterFuncionando\DataCorreto.csv";
        int numeroTimer = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {
        }

        public async Task calcularGraficos()
        {
            int janela = Convert.ToInt32(TxtJanela.Text);
            int passos = Convert.ToInt32(TxtPassos.Text);

            if (File.Exists(path))
            {
                if (janela <= passos)
                {
                    MessageBox.Show("Não pode ser maior ou igual ao número de janelas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                List<double> dataV = new List<double>();
                List<double> dataW = new List<double>();
                List<double> dataX = new List<double>();
                List<double> dataY = new List<double>();
                List<double> dataZ = new List<double>();
                List<int> numeros = new List<int>();

                await Task.Run(() =>
                {
                    string[] linhas = File.ReadAllLines(path);

                    //vou colocar a migração para o sqlite aqui
                    string dbPath = @"Data Source=C:\Users\Douglas.beccari\OneDrive - Strattner\Área de Trabalho\Instramed_Estudo_C#\Estudo_Instramed43 GráficoWalterFuncionando\bancoValores.db";

                    try
                    {
                        using (var connection = new SQLiteConnection(dbPath))
                        {
                            connection.Open();

                            using (var transaction = connection.BeginTransaction())
                            {
                                foreach (string linha in linhas)
                                {
                                    string[] data = linha.Split(';');

                                    if (data.Length > 0 && int.TryParse(data[2], out int numero)) //int.TryParse tenta converter o que ta na posiçao 2 do array data pra int, se conseguir ele joga na variavel numero usar ele no lugar do convert
                                    {
                                        dataV.Add(numero);
                                        numeros.Add(numero);

                                        string sql = "INSERT INTO Valores (Numero) VALUES (@numero)";
                                        using (var command = new SQLiteCommand(sql, connection))
                                        {
                                            command.Parameters.AddWithValue("@numero", numero);
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                                transaction.Commit();
                            }
                            MessageBox.Show("Banco de dados alimentado com sucesso!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao alimentar o banco de dados: " + ex.Message);
                        return;
                    }


                    int cortar = dataV.Count - (dataV.Count / passos); //corta de acordo com o passos e imprimi igual os outros datas
                    dataV = dataV.Take(cortar).ToList();
                    //aqui é um exemplo do take:
                    //var lista = new List<int> { 1, 2, 3, 4, 5 };
                    //var metade = lista.Take(3).ToList(); //resultado: [1, 2, 3]


                    for (int i = 0; (i + janela) <= numeros.Count; i += passos)
                    {
                        double soma = 0;
                        double elevados = 0;

                        int[] a = new int[janela];

                        for (int j = 0; j < janela; j++)
                        {
                            a[j] = numeros[i + j];
                            soma = soma + a[j]; //aqui junta os valores de: a0, a1, a2, a3 para fazer a média
                        }


                        //média comum
                        double media = soma / janela;


                        //desvio padrão
                        elevados += Math.Pow(media, 2);
                        double desvioPadrao = Math.Sqrt(elevados / janela);


                        //variante 
                        double variante = elevados / janela;


                        //adicionando os pontos no grafico
                        dataY.Add(media / 1.7);
                        dataW.Add(desvioPadrao);
                        dataZ.Add(variante / 1000000);
                    }

                    for (int k = 0; k < numeros.Count; k++) //aqui precisa imprimir no grafico os numeros de 0 até o ultimo pro grafico ficar intendivel
                    {
                        dataX.Add(k);
                    }
                });

                Lb1.Text = string.Join(" | ", numeros);

                formsPlot1.Plot.Clear();
                formsPlot1.Plot.Title("Média por Janela");
                formsPlot1.Plot.XLabel("Numeros");
                formsPlot1.Plot.YLabel("Média");
                formsPlot1.Plot.Add.Scatter(dataX, dataV);
                formsPlot1.Plot.Add.Scatter(dataX, dataW);
                formsPlot1.Plot.Add.Scatter(dataX, dataY);
                formsPlot1.Plot.Add.Scatter(dataX, dataZ);
                formsPlot1.Enabled = false; //desabilita o zoom
                formsPlot1.Plot.Axes.AutoScale(); //faz o grafico se ajustar ao tamanho dos dados


                //vou colocar a migração do Questpdf aqui

                try
                {
                    QuestPDF.Settings.License = LicenseType.Community;

                    var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(2, Unit.Centimetre);

                            page.Content().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(80);
                                });

                                //Lb1.Text = string.Join(" ", numeros);
                                table.Cell().Padding(5).Text(Lb1.Text);
                            });
                        });
                    });

                    string pathPdf = @"C:\Users\Douglas.beccari\OneDrive - Strattner\Área de Trabalho\Instramed_Estudo_C#\Estudo_Instramed43 GráficoWalterFuncionando\relatorio.pdf";

                    document.GeneratePdf(pathPdf);

                    MessageBox.Show("PDF gerado com sucesso!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro ao gerar PDF:\n");
                }
            }

            else
            {
                Lb1.Text = "Arquivo não encontrado.";
                formsPlot1.Enabled = false;
                formsPlot1.Plot.Axes.AutoScale();
            }
        }

        private async void BtEnviar_Click(object sender, EventArgs e)
        {
            await calcularGraficos();
            formsPlot1.Refresh();
        }

        private void Lb3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtIniciar1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void BtParar1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LbTimer.Text = numeroTimer.ToString();
            numeroTimer++;
        }

        private void BtReiniciar_Click(object sender, EventArgs e)
        {
            LbTimer.Text = null;
        }
    }
}

//jogar pra um pdf FEITO
//jogar pro sqlite
//colocar o timer pra ir de um segundo aparecer uma quantidade de pontos para ele ir em "tempo real"
//tirar o timer que eu coloquei que não ta sendo usado 
//ver como colocar uma tela de carregamento
//usar a classe Exam

//bibliotecas usadas:
//nuget: ScottPlot
//nuget: QuestPDF
//nuget: SQLitePCLRaw.bundle_e_sqlite3
//nuget: System.Data.SQLite
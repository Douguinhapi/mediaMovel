using System;

using System.Collections.Generic;

using System.Data.SQLite;

using System.IO;

using System.Linq;

using System.Threading.Tasks;

using System.Windows.Forms;

using QuestPDF.Fluent;

using QuestPDF.Helpers;

using QuestPDF.Infrastructure;

namespace Estudo_Instramed43

{

    public partial class Form1 : Form

    {

        // Variáveis globais

        string path = @"C:\Users\Douglas.beccari\OneDrive - Strattner\Área de Trabalho\Instramed_Estudo_C#\Estudo_Instramed43 GráficoWalterFuncionando\DataCorreto.csv";

        int numeroTimer = 0;

        // Dados globais para gráfico em tempo real

        List<double> dadosV = new List<double>();

        List<double> dadosW = new List<double>();

        List<double> dadosY = new List<double>();

        List<double> dadosZ = new List<double>();

        List<double> dadosX = new List<double>();

        int indiceAtual = 0;

        System.Windows.Forms.Timer timerDados = new System.Windows.Forms.Timer();

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

            if (!File.Exists(path))

            {

                Lb1.Text = "Arquivo não encontrado.";

                formsPlot1.Enabled = false;

                return;

            }

            if (janela <= passos)

            {

                MessageBox.Show("Não pode ser maior ou igual ao número de janelas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            List<int> numeros = new List<int>();

            dadosV.Clear(); dadosW.Clear(); dadosY.Clear(); dadosZ.Clear(); dadosX.Clear();

            indiceAtual = 0;

            await Task.Run(() =>

            {

                string[] linhas = File.ReadAllLines(path);

                //banco de dados sqlite

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

                                if (data.Length > 2 && int.TryParse(data[2], out int numero))

                                {

                                    dadosV.Add(numero);

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

                    }

                    MessageBox.Show("Banco de dados alimentado com sucesso!");

                }

                catch (Exception ex)

                {

                    MessageBox.Show("Erro ao alimentar o banco de dados: " + ex.Message);

                    return;

                }

                int cortar = dadosV.Count - (dadosV.Count / passos);

                dadosV = dadosV.Take(cortar).ToList();

                for (int i = 0; (i + janela) <= numeros.Count; i += passos)

                {

                    double soma = 0;

                    double elevados = 0;

                    int[] a = new int[janela];

                    for (int j = 0; j < janela; j++)

                    {

                        a[j] = numeros[i + j];

                        soma += a[j];

                    }

                    double media = soma / janela;

                    elevados += Math.Pow(media, 2);

                    double desvioPadrao = Math.Sqrt(elevados / janela);

                    double variante = elevados / janela;

                    dadosY.Add(media / 1.7);

                    dadosW.Add(desvioPadrao);

                    dadosZ.Add(variante / 1000000);

                }

                for (int k = 0; k < numeros.Count; k++)

                    dadosX.Add(k);

            });

            Lb1.Text = string.Join(" | ", numeros);

            // Inicia timer de atualização do gráfico em "tempo real"

            formsPlot1.Plot.Clear();

            timerDados.Interval = 25; // 1 segundo

            timerDados.Tick -= TimerDados_Tick;

            timerDados.Tick += TimerDados_Tick;

            timerDados.Start();

        }

        private void TimerDados_Tick(object sender, EventArgs e)

        {

            int pontosPorSegundo = 1;

            int limite = Math.Min(indiceAtual + pontosPorSegundo, dadosV.Count);

            formsPlot1.Plot.Clear();

            formsPlot1.Plot.Add.Scatter(dadosX.Take(limite).ToArray(), dadosV.Take(limite).ToArray());

            formsPlot1.Plot.Add.Scatter(dadosX.Take(limite).ToArray(), dadosW.Take(limite).ToArray());

            formsPlot1.Plot.Add.Scatter(dadosX.Take(limite).ToArray(), dadosY.Take(limite).ToArray());

            formsPlot1.Plot.Add.Scatter(dadosX.Take(limite).ToArray(), dadosZ.Take(limite).ToArray());

            formsPlot1.Plot.Axes.AutoScale();

            formsPlot1.Refresh();

            indiceAtual = limite;

            if (indiceAtual >= dadosV.Count)

            {

                timerDados.Stop();

                SendPdf();

            }

        }

        private void SendPdf()

        {
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

        private async void BtEnviar_Click(object sender, EventArgs e)

        {
            await calcularGraficos();

        }

        private void Lb3_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

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

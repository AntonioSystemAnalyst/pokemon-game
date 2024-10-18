using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pokedex
{
    public partial class Pokedex : Form
    {
        private MySqlConnection conn; // Conexão com o banco de dados
        private int currentPokemonIndex = 0; // Índice do Pokémon atual

        public Pokedex()
        {
            InitializeComponent();
            txtUsername.Text = "root";
            txtDatabase.Text = "orphew_pokemon";
            txtPassword.Text = "usbw";
        }

        private void Pokedex_Load(object sender, EventArgs e)
        {

        }

        // Função para conectar ao banco de dados
        private MySqlConnection ConectarBancoDeDados(string database, string username, string password)
        {
            // Adicione a porta 3307 à string de conexão
            string connectionString = $"Server=localhost;Port=3307;Database={database};User ID={username};Password={password};";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
                MessageBox.Show("Conexão aberta com sucesso!");
                return conn; // Retorna a conexão aberta
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erro ao conectar: {ex.Message}");
                return null; // Retorna null em caso de erro
            }
        }
        private void buttonConectar_Click(object sender, EventArgs e)
        {
            conn = ConectarBancoDeDados(txtDatabase.Text, txtUsername.Text, txtPassword.Text);
        }

        private void ShowNextPokemon()
        {
            if (conn == null || conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Conexão não estabelecida.");
                return;
            }

            // Consulta para pegar o próximo Pokémon baseado no índice
            MySqlCommand cmd = new MySqlCommand("SELECT ocidental_name, type1, type2 FROM pokemon LIMIT 1 OFFSET @index", conn);
            cmd.Parameters.AddWithValue("@index", currentPokemonIndex);

            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Pega os dados do Pokémon e exibe nos Labels
                        string nomePokemon = reader["ocidental_name"].ToString().ToLower(); // Converte para minúsculas
                        lblName.Text = $"Nome: {reader["ocidental_name"]}";
                        lblType1.Text = $"Tipo 1: {reader["type1"]}";
                        lblType2.Text = $"Tipo 2: {reader["type2"]}";

                        // Atualiza a imagem do PictureBox
                        string imagePath = $"resources/image/{nomePokemon}.png"; // Altere a extensão se necessário
                        if (System.IO.File.Exists(imagePath))
                        {
                            PBPokemon.Image = Image.FromFile(imagePath);
                        }
                        else
                        {
                            MessageBox.Show("Imagem do Pokémon não encontrada.");
                            PBPokemon.Image = null; // Limpa a imagem se não for encontrada
                        }

                        // Incrementa o índice para o próximo Pokémon
                        currentPokemonIndex++;
                    }
                    else
                    {
                        MessageBox.Show("Nenhum Pokémon encontrado.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erro ao buscar dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void buttonNextPokemon_Click(object sender, EventArgs e)
        {
            ShowNextPokemon();
        }

        // -----------------------------------------------------------
    }
}

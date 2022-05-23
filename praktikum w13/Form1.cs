using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace praktikum_w13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MySqlConnection sqlConnect = new MySqlConnection("server=localhost;uid=root;pwd=;database=premier_league");
        MySqlCommand sqlCommand;
        MySqlDataAdapter sqlAdapter;
        String sqlQuery;
        DataTable dtPemain = new DataTable();
        DataTable dtcboxTeam = new DataTable();
        DataTable dtNumber = new DataTable();
        DataTable dtcboxNationality = new DataTable();
        int PosisiSekarang = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            DataPemain(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlQuery = "SELECT p.player_id ,p.player_name,p.birthdate,n.nation,t.team_name,p.team_number from player p , team t, nationality n where p.team_id = t.team_id and p.nationality_id = n.nationality_id;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtPemain);
            

            sqlQuery = "SELECT  t.team_name ,t.team_id from team t;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtcboxTeam);
            comboBoxTeam.DataSource = dtcboxTeam;
            comboBoxTeam.DisplayMember = "team_name";
            comboBoxTeam.ValueMember = "team_id";

            sqlQuery = "SELECT n.nation as nation from nationality n;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtcboxNationality);
            comboBoxNationality.DataSource = dtcboxNationality;
            comboBoxNationality.ValueMember = "nation";
            DataPemain(0);
        }
        public void DataPemain(int Posisi)
        {
            textBoxID.Text = dtPemain.Rows[Posisi][0].ToString();
            textBoxNama.Text = dtPemain.Rows[Posisi][1].ToString();
            dateTimePickerBirthDate.Text = dtPemain.Rows[Posisi][2].ToString();
            comboBoxNationality.Text = dtPemain.Rows[Posisi][3].ToString();
            comboBoxTeam.Text = dtPemain.Rows[Posisi][4].ToString();
            numericUpDownNumber.Text = dtPemain.Rows[Posisi][5].ToString();
            PosisiSekarang = Posisi;
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            DataPemain(dtPemain.Rows.Count - 1);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (labelAvailable.Text == "Not Available")
            {
                MessageBox.Show("Error message: Tidak bisa save karena team number tidak tersedia");
            }
            else if(labelAvailable.Text == "Available")
            {
                sqlQuery = $"UPDATE player SET player_name = '" + textBoxNama.Text + "', team_number = '" + numericUpDownNumber.Value.ToString() + "', nationality_id = '" + comboBoxNationality.SelectedValue.ToString() + "', team_id = '" + comboBoxTeam.SelectedValue.ToString() + "' where player_id = '" + textBoxID.Text + "' ";
                sqlConnect.Open();
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
                sqlCommand.ExecuteNonQuery();
                sqlConnect.Close();
            }

        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (PosisiSekarang > 0)
            {
                PosisiSekarang--;
                DataPemain(PosisiSekarang);
            }
            else
            {
                MessageBox.Show("Data Sudah Data Pertama");
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (PosisiSekarang < dtPemain.Rows.Count - 1)
            {
                PosisiSekarang++;
                DataPemain(PosisiSekarang);
            }
            else
            {
                MessageBox.Show("Data Sudah Data Terakhir");
            }
        }

        private void comboBoxNationality_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        private void numericUpDownNumber_ValueChanged(object sender, EventArgs e)
        {
            dtNumber = new DataTable();
            sqlQuery = $"SELECT * FROM player WHERE team_id='{comboBoxTeam.SelectedValue}' and team_number={numericUpDownNumber.Value}";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtNumber);

            if (dtNumber.Rows.Count > 0)
            {
                labelAvailable.Text = "Not Available";
            }
            else
            {
                labelAvailable.Text = "Available";
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

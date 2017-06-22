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
using MySql.Data.Types;
using MySql.Data;

namespace beresteyskiy_pekar
{
    public partial class Form2 : Form
    {
        MySqlConnection con = new MySqlConnection(@"Data Source=localhost;port=3306;Initial Catalog=productpekar; User Id=root;password=root");
        public Form2()
        {
            InitializeComponent();
            select_provider();
            select_products();
            select_delivery();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void select_provider()
        {
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM provider";
            
            MySqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                this.dataGridView3.Columns.Add("idProvider", "ID");
                this.dataGridView3.Columns["idProvider"].Width = 40;
                this.dataGridView3.Columns.Add("FIO", "ФИО");
                this.dataGridView3.Columns["FIO"].Width = 100;
                this.dataGridView3.Columns.Add("phoneNumber", "Номер телефона");
                this.dataGridView3.Columns["phoneNumber"].Width = 100;
                this.dataGridView3.Columns.Add("address", "Адрес");
                this.dataGridView3.Columns["address"].Width = 240;
                this.dataGridView3.Columns.Add("organization", "Организация");
                this.dataGridView3.Columns["organization"].Width = 120;
                while (reader.Read())
                {
                    dataGridView3.Rows.Add(reader["idProvider"].ToString(), reader["FIO"].ToString(), reader["phoneNumber"].ToString(), reader["address"].ToString(), reader["organization"].ToString());
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public void select_products()
        {
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM products";

            MySqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                this.dataGridView1.Columns.Add("idProduct", "ID");
                this.dataGridView1.Columns["idProduct"].Width = 40;
                this.dataGridView1.Columns.Add("nameOfProduct", "Наименование");
                this.dataGridView1.Columns["nameOfProduct"].Width = 140;
                this.dataGridView1.Columns.Add("productCount", "Количество");
                this.dataGridView1.Columns["productCount"].Width = 70;
                this.dataGridView1.Columns.Add("description", "Описание");
                this.dataGridView1.Columns["description"].Width = 400;
                
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader["idProduct"].ToString(), reader["nameOfProduct"].ToString(), reader["productCount"].ToString(), reader["description"].ToString());
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public void select_delivery()
        {
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM delivery";

            MySqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                this.dataGridView2.Columns.Add("idDelivery", "ID Поставки");
                this.dataGridView2.Columns["idDelivery"].Width = 40;
                this.dataGridView2.Columns.Add("idProvider", "ID Поставщика");
                this.dataGridView2.Columns["idProvider"].Width = 40;
                this.dataGridView2.Columns.Add("idProduct", "ID Товара");
                this.dataGridView2.Columns["idProduct"].Width = 40;
                this.dataGridView2.Columns.Add("dateOfDelivery", "Дата поставки");
                this.dataGridView2.Columns["dateOfDelivery"].Width = 140;
                this.dataGridView2.Columns.Add("count", "Количество");
                this.dataGridView2.Columns["count"].Width = 70;
                this.dataGridView2.Columns.Add("employee", "Ответственный");
                this.dataGridView2.Columns["employee"].Width = 100;

                while (reader.Read())
                {
                    dataGridView2.Rows.Add(reader["idDelivery"].ToString(), reader["idProvider"].ToString(), reader["idProduct"].ToString(), reader["dateOfDelivery"].ToString(), reader["count"].ToString(), reader["employee"].ToString());
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

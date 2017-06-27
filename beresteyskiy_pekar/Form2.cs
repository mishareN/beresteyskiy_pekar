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
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
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
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
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
            MySqlCommand cmd2 = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT idDelivery, provider.organization, products.nameOfProduct, dateOfDelivery, count, employees.FIO " 
                            +"FROM delivery "
                            +"INNER JOIN employees ON delivery.employee = employees.user_idUser "
                            +"INNER JOIN products ON delivery.idProduct = products.idProduct "
                            +"INNER JOIN provider ON delivery.idProvider = provider.idProvider";

            MySqlDataReader reader;
            try
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                reader = cmd.ExecuteReader();
                this.dataGridView2.Columns.Add("idDelivery", "ID");
                this.dataGridView2.Columns["idDelivery"].Width = 40;
                this.dataGridView2.Columns.Add("organization", "Поставщик");
                this.dataGridView2.Columns["organization"].Width = 110;
                this.dataGridView2.Columns.Add("nameOfProduct", "Наименование товара");
                this.dataGridView2.Columns["nameOfProduct"].Width = 110;
                this.dataGridView2.Columns.Add("dateOfDelivery", "Дата поставки");
                this.dataGridView2.Columns["dateOfDelivery"].Width = 120;
                this.dataGridView2.Columns.Add("count", "Количество");
                this.dataGridView2.Columns["count"].Width = 70;
                this.dataGridView2.Columns.Add("FIO", "Ответственный");
                this.dataGridView2.Columns["FIO"].Width = 165;

                while (reader.Read())
                {
                    dataGridView2.Rows.Add(reader["idDelivery"].ToString(), reader["organization"].ToString(), reader["nameOfProduct"].ToString(), reader["dateOfDelivery"].ToString(), reader["count"].ToString(), reader["FIO"].ToString());
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "" || dateTimePicker2.Text.ToString() == "" || textBox4.Text == "" || Convert.ToInt32(textBox5.Text) <= 0 || Convert.ToInt32(textBox4.Text) <= 0)
            {
                label20.Text = "Ошибка! Некоторые поля не заполнены!";
            }
            else
            {
                try
                {
                    con.Open();
                    MySqlCommand check = con.CreateCommand();
                    check.CommandType = CommandType.Text;
                    check.CommandText = "SELECT * FROM delivery where idDelivery = @id";
                    check.Parameters.AddWithValue("@id", textBox5.Text);
                    MySqlDataReader reader = check.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        label20.Text = "Поставка с данным ID уже существует!";
                        con.Close();
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO delivery(idDelivery, idProvider, idProduct, dateOfDelivery, count, employee) VALUES(@idDelivery, @idProvider, @idProduct, @dateOfDelivery, @count, @employee)";
                        cmd.Parameters.AddWithValue("@idDelivery", textBox5.Text);
                        cmd.Parameters.AddWithValue("@idProvider", comboBox5.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idProduct", comboBox4.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@dateOfDelivery", dateTimePicker2.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@count", textBox4.Text);
                        cmd.Parameters.AddWithValue("@employee", comboBox6.SelectedValue.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();
                        select_delivery();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: \r\n{0}", ex.ToString());
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "" || Convert.ToInt32(textBox6.Text)<=0)
            {
                label20.Text = "Ошибка! Некоторые поля не заполнены!";
            }
            else 
            {
                try
                {
                    con.Open();
                    MySqlCommand check = con.CreateCommand();
                    check.CommandType = CommandType.Text;
                    check.CommandText = "SELECT * FROM provider where idProvider = @id";
                    check.Parameters.AddWithValue("@id", textBox6.Text);
                    MySqlDataReader reader = check.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        label20.Text = "Ошибка! Поставщик с данным ID уже существует!";
                        con.Close();
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO provider(idProvider, FIO, phoneNumber, address, organization) VALUES(@idProvider, @FIO, @phoneNumber, @address, @organization)";
                        cmd.Parameters.AddWithValue("@idProvider", textBox6.Text);
                        cmd.Parameters.AddWithValue("@FIO", textBox7.Text);
                        cmd.Parameters.AddWithValue("@phoneNumber", textBox8.Text);
                        cmd.Parameters.AddWithValue("@address", textBox9.Text);
                        cmd.Parameters.AddWithValue("@organization", textBox10.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        select_provider();
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox9.Clear();
                        textBox10.Clear();
                        this.providerTableAdapter.Fill(this.productpekarDataSet.provider);
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: \r\n{0}", ex.ToString());
                }
            } 
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "productpekarDataSet.delivery". При необходимости она может быть перемещена или удалена.
            this.deliveryTableAdapter.Fill(this.productpekarDataSet.delivery);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "productpekarDataSet.employees". При необходимости она может быть перемещена или удалена.
            this.employeesTableAdapter.Fill(this.productpekarDataSet.employees);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "productpekarDataSet.products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.productpekarDataSet.products);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "productpekarDataSet.provider". При необходимости она может быть перемещена или удалена.
            this.providerTableAdapter.Fill(this.productpekarDataSet.provider);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно хотите удалить данного поставщика?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int idProvider = Convert.ToInt32(comboBox7.SelectedValue.ToString());
                    con.Open();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM provider WHERE idProvider = @idProvider";
                    cmd.Parameters.AddWithValue("@idProvider", comboBox7.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();
                    select_provider();
                    this.providerTableAdapter.Fill(this.productpekarDataSet.provider);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: \r\n{0}", ex.ToString());
                }
            } 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно хотите подтвердить поставку товара на склад?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int idDelivery = Convert.ToInt32(comboBox3.SelectedValue.ToString());
                    con.Open();
                    MySqlCommand check = con.CreateCommand();
                    check.CommandType = CommandType.Text;
                    check.CommandText = "SELECT idProduct, count FROM delivery WHERE idDelivery = @idDelivery";
                    check.Parameters.AddWithValue("@idDelivery", idDelivery);

                    MySqlDataReader reader = check.ExecuteReader();
                    reader.Read();
                    if (!reader.HasRows)
                    {
                        label20.Text = "Ошибка! Пожалуйста, добавьте сначала данный товар в базу";
                        con.Close();
                        reader.Close();
                    }
                    else
                    {
                        int idProduct = Convert.ToInt32(reader["idProduct"]);
                        int count = Convert.ToInt32(reader["count"]);
                        reader.Close();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE products SET productCount = productCount + @count WHERE idProduct = @idProduct";
                        cmd.Parameters.AddWithValue("@idProduct", idProduct);
                        cmd.Parameters.AddWithValue("@count", count);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "DELETE FROM delivery WHERE idDelivery = @idDelivery";
                        cmd.Parameters.AddWithValue("@idDelivery", idDelivery);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        select_provider();
                        select_products();
                        select_delivery();
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox9.Clear();
                        textBox10.Clear();
                        this.deliveryTableAdapter.Fill(this.productpekarDataSet.delivery);
                    }
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
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                label20.Text = "Ошибка! Некоторые поля не заполнены!";
            }
            else
            {
                try
                {
                    con.Open();
                    MySqlCommand check = con.CreateCommand();
                    check.CommandType = CommandType.Text;
                    check.CommandText = "SELECT * FROM products where idProduct = @idProduct";
                    check.Parameters.AddWithValue("@idProduct", textBox2.Text);
                    MySqlDataReader reader = check.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        label20.Text = "Ошибка! Товар с данным ID уже существует!";
                        con.Close();
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO products(idProduct, nameOfProduct, productCount, description) VALUES(@idProduct, @nameOfProduct, 0, @description)";
                        cmd.Parameters.AddWithValue("@idProduct", textBox2.Text);
                        cmd.Parameters.AddWithValue("@nameOfProduct", textBox3.Text);
                        cmd.Parameters.AddWithValue("@description", textBox11.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        select_provider();
                        select_products();
                        textBox2.Clear();
                        textBox3.Clear();
                        this.productsTableAdapter.Fill(this.productpekarDataSet.products);
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: \r\n{0}", ex.ToString());
                }
            } 
        }
    }
}

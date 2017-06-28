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
                this.dataGridView1.Columns["description"].Width = 350;
                
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
                this.dataGridView2.Columns["idDelivery"].Width = 35;
                this.dataGridView2.Columns.Add("organization", "Поставщик");
                this.dataGridView2.Columns["organization"].Width = 110;
                this.dataGridView2.Columns.Add("nameOfProduct", "Наименование товара");
                this.dataGridView2.Columns["nameOfProduct"].Width = 100;
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

        public void implm_product() 
        {
            if (textBox1.Text == "" || comboBox1.SelectedValue.ToString() == "" || comboBox2.SelectedValue.ToString() == "")
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
                    check.Parameters.AddWithValue("@idProduct", comboBox1.SelectedValue.ToString());
                    MySqlDataReader reader = check.ExecuteReader();
                    reader.Read();
                    int count = Convert.ToInt32(reader["productCount"]);
                    int implCount = Convert.ToInt32(textBox1.Text);
                    int idProduct = Convert.ToInt32(comboBox1.SelectedValue);
                    String dateOfImplm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    String employer = comboBox2.SelectedValue.ToString();
                    if (count - implCount < 0)
                    {
                        label20.Text = "Не хватает товара на складе!";
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE products SET productCount = productCount - @implCount WHERE idProduct = @idProduct";
                        cmd.Parameters.AddWithValue("@idProduct", idProduct);
                        cmd.Parameters.AddWithValue("@implCount", implCount);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO implementation(dateOfImplm, idProduct, count, employer) VALUES(@dateOfImplm, @idProduct, @count, @employer)";
                        cmd.Parameters.AddWithValue("@dateOfImplm", dateOfImplm);
                        cmd.Parameters.AddWithValue("@count", implCount);
                        cmd.Parameters.AddWithValue("@employer", employer);
                        cmd.ExecuteNonQuery();
                        select_delivery();
                        select_products();
                        textBox1.Clear();
                        adapter_update();
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

        public void added_product() 
        {
            if (textBox3.Text == "")
            {
                label20.Text = "Ошибка! Некоторые поля не заполнены!";
            }
            else
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO products(nameOfProduct, productCount, description) VALUES(@nameOfProduct, 0, @description)";
                    cmd.Parameters.AddWithValue("@nameOfProduct", textBox3.Text);
                    cmd.Parameters.AddWithValue("@description", textBox11.Text);
                    cmd.ExecuteNonQuery();
                    select_provider();
                    select_products();
                    textBox3.Clear();
                    textBox11.Clear();
                    adapter_update();
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

        public void delete_product() 
        {
            if (MessageBox.Show("Действительно хотите удалить данный товар?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int idProduct = Convert.ToInt32(comboBox8.SelectedValue.ToString());
                    con.Open();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM products WHERE idProduct = @idProduct";
                    cmd.Parameters.AddWithValue("@idProduct", idProduct);
                    cmd.ExecuteNonQuery();
                    select_products();
                    adapter_update();
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

        public void added_delivery() 
        {
            if (dateTimePicker2.Text.ToString() == "" || textBox4.Text == "" || Convert.ToInt32(textBox4.Text) <= 0)
            {
                label20.Text = "Ошибка! Некоторые поля не заполнены!";
            }
            else
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO delivery(idProvider, idProduct, dateOfDelivery, count, employee) VALUES(@idProvider, @idProduct, @dateOfDelivery, @count, @employee)";
                    cmd.Parameters.AddWithValue("@idProvider", comboBox5.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@idProduct", comboBox4.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@dateOfDelivery", dateTimePicker2.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@count", textBox4.Text);
                    cmd.Parameters.AddWithValue("@employee", comboBox6.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    select_delivery();
                    textBox4.Clear();
                    adapter_update();
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

        public void goods_arrival()
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
                        select_provider();
                        select_products();
                        select_delivery();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox9.Clear();
                        textBox10.Clear();
                        adapter_update();
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

        public void added_provider() 
        {
            if (textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "")
            {
                label20.Text = "Ошибка! Некоторые поля не заполнены!";
            }
            else
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO provider(FIO, phoneNumber, address, organization) VALUES(@FIO, @phoneNumber, @address, @organization)";
                    cmd.Parameters.AddWithValue("@FIO", textBox7.Text);
                    cmd.Parameters.AddWithValue("@phoneNumber", textBox8.Text);
                    cmd.Parameters.AddWithValue("@address", textBox9.Text);
                    cmd.Parameters.AddWithValue("@organization", textBox10.Text);
                    cmd.ExecuteNonQuery();
                    select_provider();
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox9.Clear();
                    textBox10.Clear();
                    adapter_update();
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

        public void delete_provider() 
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
                    select_provider();
                    adapter_update();
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

        public void adapter_update()
        {
            this.deliveryTableAdapter.Fill(this.productpekarDataSet.delivery);
            this.employeesTableAdapter.Fill(this.productpekarDataSet.employees);
            this.productsTableAdapter.Fill(this.productpekarDataSet.products);
            this.providerTableAdapter.Fill(this.productpekarDataSet.provider);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            added_delivery();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            added_provider();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            adapter_update();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            delete_provider();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            goods_arrival();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            added_product();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            implm_product();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            delete_product();
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 fm = new Form1();
            fm.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 fm3 = new Form3();
            fm3.Show();
        }
    }
}

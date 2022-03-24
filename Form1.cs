using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;


namespace SQL_Task_Manager
{
    public partial class Form1 : Form
    {
        public SqlConnection sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            sqlConnection.Open();
            SqlCommand creation = new SqlCommand("IF OBJECT_ID(N'[dbo].[tasks]', N'U') IS NULL BEGIN CREATE TABLE tasks (id int IDENTITY(1,1) PRIMARY KEY, subject nvarchar(50) NOT NULL, task nvarchar(50) NOT NULL, deadline nvarchar(50), iscomplete nvarchar(50)) END;", sqlConnection);
            creation.ExecuteNonQuery();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Connection is  Open");
            }


            
        }

        private void INSERT_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox4.Text != "")
            {


                string check = "";
                if (checkBox2.Checked) { check = "Complete !"; }
                else { check = "Not complete !"; }

                SqlCommand command = new SqlCommand("INSERT INTO [tasks] (subject, task, deadline, iscomplete) VALUES(@subject, @task, @deadline, @iscomplete)", sqlConnection);
                command.Parameters.AddWithValue("subject", textBox1.Text);
                command.Parameters.AddWithValue("task", textBox4.Text);
                command.Parameters.AddWithValue("deadline", textBox3.Text);
                command.Parameters.AddWithValue("iscomplete", check);
                string good = "1";
                if (command.ExecuteNonQuery().ToString() == good) { MessageBox.Show("New ited added :)"); }
                else { MessageBox.Show("Something went wrong< check your data !!!"); }
            }
            else { MessageBox.Show("Error !\nFields 'Subject' and 'Task' can`t be empty !!!"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = textBox5.Text;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "SELECT id,subject,task,deadline,iscomplete FROM tasks";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView2.DataSource = dataSet.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
       

            string query_init(int id, string name, string value)
            {
                string quer = $"UPDATE tasks SET {name}= '{value}' WHERE id = {id}";
                return quer;
            }

            



                void proces(string query_string) 
                {
                    SqlCommand command = new SqlCommand(query_string, sqlConnection);
                    string result = command.ExecuteNonQuery().ToString();
                
                   
                }
            
                string [] changed_list()
                {
                    string[] list = new string[3];
                    for(int i =0; i<3; i++)
                    {
                        if(textBox7.Text != "") { list[i] = "subject"; }
                        else if(textBox8.Text != "") { list[i] = "task";}
                        else if (textBox9.Text != "") { list[i] = "deadline"; }
                    }

                    return list;
                }

                string [] list_c = changed_list();
                
                
                foreach(string i in list_c)
                {
                    
                    if (i == "subject") { proces(query_init(Convert.ToInt32(textBox6.Text),i, textBox7.Text)); }
                    else if (i == "task") { proces(query_init(Convert.ToInt32(textBox6.Text), i, textBox8.Text)); }
                    else if (i == "deadline") { proces(query_init(Convert.ToInt32(textBox6.Text), i, textBox9.Text)) ; }
                
                }

            if (textBox6.Text != "")
            {

                if (checkBox1.Checked) { proces($"UPDATE tasks SET iscomplete = 'Complete !' WHERE id = {Convert.ToInt32(textBox6.Text)}"); }
                else if(!checkBox1.Checked) { proces($"UPDATE tasks SET iscomplete = 'Not complete !' WHERE id = {Convert.ToInt32(textBox6.Text)}"); }


                MessageBox.Show("base updated");

            }

            else { MessageBox.Show("Field 'Id' can`t be empty !!!"); }

            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM tasks";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            void delete(string key, string value, string key2 ="", string value2 ="")  
            {
                string query_string = "";
                if (key == "id") { query_string = $"DELETE FROM tasks WHERE {key} = {Convert.ToInt32(value)}"; }
                else { query_string = $"DELETE FROM tasks WHERE {key} = '{value}' AND {key2} = '{value2}'"; }
                SqlCommand command = new SqlCommand(query_string, sqlConnection);
                command.ExecuteNonQuery();

            }
            
            
            if (textBox10.Text != "") { delete("id", textBox10.Text); MessageBox.Show("Deleting complete :)"); }
            else if (textBox10.Text == "" && textBox11.Text !="" &&  textBox12.Text != "")
            { delete("subject", textBox11.Text, "task", textBox12.Text); MessageBox.Show("Deleting complete :)"); }
            else if (textBox10.Text == "" && textBox11.Text == "" && textBox12.Text == "") { MessageBox.Show("Error!\nIf ID is empty, fields 'subject' and 'task' can`t be empty !!!"); }
            else if (true) { MessageBox.Show("Error!\'Id' field is empty! :\t Fields 'subject' and 'task' both must be staisfied!\nOne of there is empty !"); }
            

        }
    }
}

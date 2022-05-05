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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            
            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.ZooprojectConnectionString"].ConnectionString;

            sqlConnection =new SqlConnection(connectionString);
            ShowZoos();
            ShowAllAnimals();

            
            
        }

        private void ShowAllAnimals()
        {

            try
            {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    ListAllAnimals.DisplayMemberPath = "Name";
                    ListAllAnimals.SelectedValuePath = "Id";
                    ListAllAnimals.ItemsSource = animalTable.DefaultView;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }





        }


        private void ShowZoos()

        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);

                    List_Zoos.DisplayMemberPath = "Location";
                    List_Zoos.SelectedValuePath = "Id";
                    List_Zoos.ItemsSource = zooTable.DefaultView;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        private void ShowAssociatedAnimals()

        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id=za.AnimalId where za.ZooId=@ZooId";
                SqlCommand sqlCommand= new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", List_Zoos.SelectedValue);

                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    List_associated_animals.DisplayMemberPath = "Name";
                    List_associated_animals.SelectedValuePath = "Id";
                    List_associated_animals.ItemsSource = animalTable.DefaultView;
                }
            }

            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }

        }



        private void List_Zoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooIntextBox();
            //ShowSelectedAnimalInTextBox();
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where id=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", List_Zoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();


            }


        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();


            }

        }

        private void addAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", List_Zoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();


            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo Set Location=@Location  where Id=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", List_Zoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();


            }
        }


        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal Set Name=@Name  where Id=@AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();


            }
        }



        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id=@AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();


            }

        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();


            }

        }

        private void ShowSelectedZooIntextBox()
        {
            try
            {
                string query = "select location from Zoo where Id=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", List_Zoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();
                    sqlDataAdapter.Fill(zooDataTable);

                    myTextBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                }
            }

            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }


        }

        private void ShowSelectedAnimalInTextBox()
        {
            try
            {
                string query = "select name from Animal where Id=@AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", ListAllAnimals.SelectedValue);

                    DataTable AnimalsDataTable = new DataTable();
                    sqlDataAdapter.Fill(AnimalsDataTable);

                    myTextBox.Text = AnimalsDataTable.Rows[0]["Name"].ToString();
                }
            }

            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }

        }

        private void ListAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }
    }
}

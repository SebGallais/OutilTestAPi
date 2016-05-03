using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;
namespace ClientAPI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void BoutonConnexion_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                string statutOk = "Ok";

                String Username = usernameBox.Text;
                String Password = passwordBox.Password;
                String str = webClient.UploadString("http://172.16.2.71:8080/apiAnnuaire/v1/login?Username=" + Username + "&Password=" + Password + "", "POST");

                
                    ClientAPI.Model.Token auth = JsonConvert.DeserializeObject<ClientAPI.Model.Token>(str);
                    String statut = auth.Statut;

                    if (statut == statutOk)
                    {
                        ApplicationGestion applicationGestion = new ApplicationGestion(auth);
                        applicationGestion.Show();
                        this.Close();
                       
                    }
                    else
                    {
                        MessageBox.Show("Couple Login/Password incorrecte");
                        usernameBox.Text = "";
                        passwordBox.Password = "";

                    }


                }
                catch
                {
                    MessageBox.Show("Connexion impossible");
                }
               

            }

        }
    }
}

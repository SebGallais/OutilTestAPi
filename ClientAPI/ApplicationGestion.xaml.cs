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
using System.Windows.Shapes;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ClientAPI
{
    /// <summary>
    /// Logique d'interaction pour ApplicationGestion.xaml
    /// </summary>
    public partial class ApplicationGestion : Window
    {
        private string jeton {get;set;}
        public ApplicationGestion(ClientAPI.Model.Token token)
        {
            InitializeComponent();

             jeton = token.token;

            TypeAppliModif.Items.Add("A3");
            TypeAppliModif.Items.Add("HORNET");
            TypeAppliModif.Items.Add("EOLE");
            TypeAppliModif.Items.Add("FAIR");
            TypeAppliModif.Items.Add("LIFERAY");

            TypeAppliAjout.Items.Add("A3");
            TypeAppliAjout.Items.Add("HORNET");
            TypeAppliAjout.Items.Add("EOLE");
            TypeAppliAjout.Items.Add("FAIR");
            TypeAppliAjout.Items.Add("LIFERAY");

        }

        private void ButtonRechercherAppli(object sender, RoutedEventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {

                try
                {
                String identifiant = AppliId.Text;
                webClient.Headers.Add(HttpRequestHeader.Authorization, jeton);
                Stream stream = webClient.OpenRead("http://172.16.2.71:8080/apiAnnuaire/v1/applications/" + identifiant + "");
                StreamReader reader = new StreamReader(stream);

                String str = reader.ReadToEnd();


                    ClientAPI.Model.Application application = JsonConvert.DeserializeObject<ClientAPI.Model.Application>(str);
                    nomAppliModif.Text = application.nomApplication;
                    codeAppliModif.Text = application.codeApplication;
                    descAppliModif.Text = application.descriptionApplication;

                }
                catch
                {
                    MessageBox.Show("Application introuvable");
                    AppliId.Text = "";

                }

                
                
            }

        }

        private void ButtonModifierAppli_Click(object sender, RoutedEventArgs e)
        {



            using (WebClient webClient = new WebClient())
            {
                try
                {
                    if (TypeAppliModif.Text == "") 
                    { 
                        throw new Exception(); 
                    
                    };

                    String identifiant = AppliId.Text;
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json;UTF-8");
                    webClient.UploadString("http://172.16.2.71:8080/apiAnnuaire/v1/applications/" + identifiant + "", "PUT", JsonConvert.SerializeObject(new ApplicationDTO() { codeApplication = codeAppliModif.Text, nomApplication = nomAppliModif.Text, descriptionApplication = descAppliModif.Text, codeTypeApplication = TypeAppliModif.Text }));

                    MessageBox.Show("Application modifiée");
                    codeAppliModif.Text = "";
                    nomAppliModif.Text = "";
                    descAppliModif.Text = "";
                    TypeAppliModif.Text = "";
                    AppliId.Text = "";
                }
                catch
                {
                    MessageBox.Show("Impossible de modifier l'application");
                }
            }


        }

        private void BtnSupprimerAppli_Click(object sender, RoutedEventArgs e)
        {

            using (WebClient webClient = new WebClient())
            {
                String identifiant = AppliId.Text;
                webClient.UploadString("http://172.16.2.71:8080/apiAnnuaire/v1/applications/" + identifiant + "", "DELETE");

                MessageBox.Show("ok");
            }
        }

        private void ButtonAjoutAppli_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {

                    if (TypeAppliAjout.Text == "") 
                    { 
                        throw new Exception(); 
                    };

                    webClient.Headers.Add(HttpRequestHeader.Authorization, jeton);
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json;UTF-8");
                    webClient.UploadString("http://172.16.2.71:8080/apiAnnuaire/v1/applications", "POST", JsonConvert.SerializeObject(new ApplicationDTO() { codeApplication = codeAppliAjout.Text, nomApplication = nomAppliAjout.Text, descriptionApplication = descAppliAjout.Text, codeTypeApplication = TypeAppliAjout.Text }));

                    MessageBox.Show("Application ajoutée");

                    codeAppliAjout.Text = "";
                    nomAppliAjout.Text = "";
                    descAppliAjout.Text= "";
                    TypeAppliAjout.Text = "";
                    


                }
                catch
                {
                    MessageBox.Show("Impossible d'ajouter l'application");
                }
            }




        }


      
    }
}

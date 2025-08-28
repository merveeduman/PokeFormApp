using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokeFormApp.Autofac;
using PokeFormApp.Models;
using PokeFormApp.Services;
using PokemonReviewApp.Dto;

namespace PokeFormApp
{
    public partial class Login : Form
    {
        private readonly IHttpRequest _httpRequest;
        public string Token { get; private set; }

        public Login()
        {
            InitializeComponent();
            _httpRequest = InstanceFactory.GetInstance<IHttpRequest>();
            this.AcceptButton = button1;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*'; // Şifre kutusu yıldızlı görünsün
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            var loginData = new LoginRequest { Email = email, Password = password };
            string apiUrl = "https://localhost:7091/api/Auth/login";

            




            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync(apiUrl, content);
                    var responseText = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Login başarısız! Kod: " + response.StatusCode +
                                        "\nCevap: " + responseText);
                        return;
                    }

                    string token = TryExtractToken(responseText);

                    if (!string.IsNullOrEmpty(token))
                    {
                        LoginSuccess(token);
                    }
                    else
                    {
                        MessageBox.Show("Token parse edilemedi!\nGelen cevap:\n" + responseText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("İstek hatası: " + ex.Message);
                }
            }
        }

        private string TryExtractToken(string responseText)
        {
            MessageBox.Show("Gelen yanıt: " + (string.IsNullOrEmpty(responseText) ? "Boş" : responseText));
            try
            {
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseText);
                var token = tokenResponse?.Token
                           ?? tokenResponse?.AccessTokenSnake
                           ?? tokenResponse?.AccessTokenCamel
                           ?? tokenResponse?.Jwt
                           ?? tokenResponse?.BearerToken
                           ?? tokenResponse?.data?.token
                           ?? (responseText.StartsWith("eyJ") ? responseText.Trim('\"') : null);
                MessageBox.Show("Çıkarılan token: " + (string.IsNullOrEmpty(token) ? "Boş" : token));
                return token;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message + "\nYanıt: " + responseText);
                if (!string.IsNullOrWhiteSpace(responseText) && responseText.StartsWith("eyJ"))
                    return responseText.Trim('\"');
                return null;
            }
        }

        private void LoginSuccess(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Token boş!");
                return;
            }

            this.Token = token;           
            SessionManager.Token = token; // BURADA TOKEN'ı SESSION MANAGER'a ATA
            this.Hide();
            Form1 form1 = new Form1();
            form1.FormClosed += (s, args) => this.Close(); // Form1 kapanınca Login form da tam kapanır
            form1.Show();    
        }

    }
}
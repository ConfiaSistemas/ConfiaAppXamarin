using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using ConfiApp.Modelos;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
using Acr.UserDialogs;
using Newtonsoft.Json.Converters;

namespace ConfiApp
{
    public partial class MainPage : ContentPage
    {
       
        public static string NombreUsuario = "";
        ILoginManager iml = null;
        public MainPage(ILoginManager ilm)
        {
            InitializeComponent();
            iml = ilm;
        }
        public class Usr
        {
            public string id { get; set; }
            public string nm { get; set; }
            public string pass { get; set; }
            public string grp { get; set; }
            public string nm_complete { get; set; }
            public string addr { get; set; }
          
            public string img { get; set; }
            public string imgstr { get; set; }
            public string tlf { get; set; }
            public string fecha_alta { get; set; }
            public string Activo { get; set; }
            public string DDNS { get; set; }
        }
        public class RSA
        {
            public RSA() => Provider = new RSACryptoServiceProvider(2048);
            public RSA(string key)
            {
                Provider = new RSACryptoServiceProvider(2048);
                Provider.FromXmlString(key);
            }
            public RSACryptoServiceProvider Provider;
            public string PublicKey() => Convert.ToBase64String(Encoding.UTF8.GetBytes(Provider.ToXmlString(false)));
            public string PrivateKey() => Convert.ToBase64String(Encoding.UTF8.GetBytes(Provider.ToXmlString(true)));
            public string Encrypt(string meta) => Convert.ToBase64String(Provider.Encrypt(Encoding.UTF8.GetBytes(meta), RSAEncryptionPadding.Pkcs1));
            public string Decrypt(string meta) => Encoding.UTF8.GetString(Provider.Decrypt(Convert.FromBase64String(meta), RSAEncryptionPadding.Pkcs1)).ToString();
        }

        public static string Decryption(string strText)
        {
            var privateKey = "<RSAKeyValue><Modulus>1D3WnN4qZq3DGHf7i3WjEixX0Nzka28RIR9lOrT7Eg2QmJTACm/E6388wMdGgH3yTwKPp/T5zm61yvZn44oHMka8EKlACGKkADsxN/ci0RLKJ0ZGYDVPtB+KSzI+GbmX2eO00R+FlYvXTpMzGXy3e4QpeCJbIbrDGFpt3rmXy28=</Modulus><Exponent>AQAB</Exponent><P>98YU9Nkhu3qQ4Y168ZbMX+kCFUEPK9mRDEc2yZiyfABU9UlrKU4Ja1qy47WJrQNSALA9nnARZgbiY/3JkslISQ==</P><Q>20nBaoUN5QK9cuH6yX0QqAzhhB88rsI/HFzb8xrbEkceNfsTbYOVt+7biqzQQAvsyEILU+0l529eSe6S52yl9w==</Q><DP>wk3CLWkBfQZXC6ppmX9KcoRFr+k/PoH1r41BN8LZZUjVVy3mLZQW6utLkirQ9q695fBPwincWwhXDVb+dnAGkQ==</DP><DQ>Ks/0hhJiCxME37gE2W+kX9rb8IqUs13TKntqqcTVfnUKDent+hSVl2p3zFQ++DIb0WEriwAixVN16iM85RfOMw==</DQ><InverseQ>coc9FIy42//YO02qCCKfORevLIU8GIeTSYFqD9kMwhHSy1a6QMDpKaYWrYvv8FcMHglqWzdTqbSgBSrsfcnibw==</InverseQ><D>JvPie4/awFWLxOXgaMwCTceNpmukEIOl5SpZ7dhhbALJUveZ91BkF8SWZdss+VAkNJQHwY+YeWagPsvSbVRb1WylY11D8gBQEb6EOR3EsM17/5+v6nRrJ5+cySfpcahRbUjUdJFaVhMCUQ1wsnevLNY/Xo20UF4XCE5Exp7TW0E=</D></RSAKeyValue>";

            var testData = Encoding.UTF8.GetBytes(strText);

            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    var base64Encrypted = strText;

                    // server decrypting data with private key                    
                    rsa.FromXmlString(privateKey);

                    var resultBytes = Convert.FromBase64String(base64Encrypted);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        public static byte[] StringToByteArray(string hex) { return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray(); }
        public static ImageSource DisplayImage(string hex) { byte[] imgBytes = StringToByteArray(hex); return ImageSource.FromStream(() => new MemoryStream(imgBytes)); }
        [Obsolete]
        private async void Button_Clicked(Object sender,EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Trabajando");
            Login log = new Login
            {
                Email = txtEmail.Text,
            Pass = txtpass.Text

            };
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://www.prestamosconfia.com/UserList.php?usr=" + txtEmail.Text);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
           HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                try
                {
                    var resultado = JsonConvert.DeserializeObject<List<Usr>>(content);
                    var rsa = new RSA();
                    string contra = resultado[0].pass.ToString();
                    var pvKey = "<RSAKeyValue><Modulus>1D3WnN4qZq3DGHf7i3WjEixX0Nzka28RIR9lOrT7Eg2QmJTACm/E6388wMdGgH3yTwKPp/T5zm61yvZn44oHMka8EKlACGKkADsxN/ci0RLKJ0ZGYDVPtB+KSzI+GbmX2eO00R+FlYvXTpMzGXy3e4QpeCJbIbrDGFpt3rmXy28=</Modulus><Exponent>AQAB</Exponent><P>98YU9Nkhu3qQ4Y168ZbMX+kCFUEPK9mRDEc2yZiyfABU9UlrKU4Ja1qy47WJrQNSALA9nnARZgbiY/3JkslISQ==</P><Q>20nBaoUN5QK9cuH6yX0QqAzhhB88rsI/HFzb8xrbEkceNfsTbYOVt+7biqzQQAvsyEILU+0l529eSe6S52yl9w==</Q><DP>wk3CLWkBfQZXC6ppmX9KcoRFr+k/PoH1r41BN8LZZUjVVy3mLZQW6utLkirQ9q695fBPwincWwhXDVb+dnAGkQ==</DP><DQ>Ks/0hhJiCxME37gE2W+kX9rb8IqUs13TKntqqcTVfnUKDent+hSVl2p3zFQ++DIb0WEriwAixVN16iM85RfOMw==</DQ><InverseQ>coc9FIy42//YO02qCCKfORevLIU8GIeTSYFqD9kMwhHSy1a6QMDpKaYWrYvv8FcMHglqWzdTqbSgBSrsfcnibw==</InverseQ><D>JvPie4/awFWLxOXgaMwCTceNpmukEIOl5SpZ7dhhbALJUveZ91BkF8SWZdss+VAkNJQHwY+YeWagPsvSbVRb1WylY11D8gBQEb6EOR3EsM17/5+v6nRrJ5+cySfpcahRbUjUdJFaVhMCUQ1wsnevLNY/Xo20UF4XCE5Exp7TW0E=</D></RSAKeyValue>";
                    var decrypted = new RSA(pvKey).Decrypt(contra);

                    // txtpass.Text = npas;
                    char[] charsRead = new char[decrypted.Length];
                    using (StringReader reader = new StringReader(decrypted))
                    {
                        await reader.ReadAsync(charsRead, 0, decrypted.Length);
                    }

                    StringBuilder reformattedText = new StringBuilder();
                    string a = null;
                    using (StringWriter writer = new StringWriter(reformattedText))
                    {
                        foreach (char c in charsRead)
                        {
                            if (char.IsLetter(c) || char.IsDigit(c))
                            {
                                await writer.WriteLineAsync(c);
                                a = a + c;
                            }
                        }
                    }
                    //txtEmail.Text = reformattedText.ToString();
                    //txtEmail.Text = a;
                    if (a == txtpass.Text)
                    {
                        
                        NombreUsuario = resultado[0].nm.ToString();
                        App.Current.Properties["name"] = NombreUsuario;
                        App.Current.Properties["IsLoggedIn"] = true;
                        App.Current.Properties["DDNS"] = resultado[0].DDNS.ToString();
                        App.Current.Properties["nm_completo"] = resultado[0].nm_complete.ToString();
                        App.Current.Properties["imgUsr"] = resultado[0].imgstr.ToString();
                        App.Current.Properties["imgI"] = resultado[0].img;
                        
                        //byte[] by = System.Text.Encoding.Unicode.GetBytes(resultado[0].img.Replace(@"\u00", string.Empty));
                        //  var bArray = (byte[])resultado[0].img;
                        iml.ShowMainPage();


                        // await Navigation.PushAsync(new Home());
                    }
                    else
                    {
                        await DisplayAlert("Mensaje", "Contraseña incorrecta", "Ok");
                    }

                }catch(JsonException ex)
                {
                    await DisplayAlert("Mensaje", "Datos Incorrectos", "Ok");
                }








            }

            UserDialogs.Instance.HideLoading();
        }

    }
    
}

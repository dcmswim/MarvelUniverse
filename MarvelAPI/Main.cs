using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarvelUniverse.MarvelAPI
{
    public class Main
    {
        //string base_URL = "https://gateway.marvel.com/v1/public/characters";
        private readonly string publicKey = "8727b229c501ce42f0d084665cba3146";
        //private key should be in a config file so that it can't be accessed! Located here temporarily for ease of access
        private readonly string privateKey = "4821819b4c2ad1067279a917c49920bc8dc953aa";
        private static HttpClient client = new HttpClient();

        public Main()
        {

        }
        public async Task<CharacterDataWrapper> GetCharacters()
        {

            //creation of unique time stamp 
            string timestamp = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
            string s = String.Format("{0}{1}{2}", timestamp, privateKey, publicKey);
            string hash = CreateHash(s);

            var requestURL = String.Format("https://gateway.marvel.com/v1/public/characters?ts=" + timestamp + "&apikey=" + publicKey + "&hash=" + hash);
            var url = new Uri(requestURL);
            var response = await client.GetAsync(url);
            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            CharacterDataWrapper cdw = JsonConvert.DeserializeObject<CharacterDataWrapper>(json);

            return cdw;

        }


        private static string CreateHash(string input)
        {
            var hash = String.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                hash = sBuilder.ToString();
            }
            return hash;

        }
    }
}

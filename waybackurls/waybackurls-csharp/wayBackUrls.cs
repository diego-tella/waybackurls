using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace waybackurls_csharp
{
    class wayBackUrls
    {
        public void start(bool verbose, bool output, string url)
        {
            if (checkUrl(url))
                getUrls(verbose, output, url);
            else
                Console.WriteLine("This site does not contain any snapshots available. Did you enter correctly? Link inserted: " + url);
        }
        private bool checkUrl(string url)
        {
            WebClient req = new WebClient();
            string jsonReq = req.DownloadString("http://archive.org/wayback/available?url=" + url);

            dynamic stuff = JsonConvert.DeserializeObject(jsonReq);
            try
            {
                if (stuff.archived_snapshots.closest.available == "True") //if there is available snapshots
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        private void getUrls(bool verbose, bool output, string url)
        {
            WebClient req = new WebClient();
            string resp = req.DownloadString("http://web.archive.org/cdx/search/cdx?url=*."+url+"/*&output=json&collapse=urlkey");

            dynamic stuff = JsonConvert.DeserializeObject(resp);
            int j = 0;
            StreamWriter file = new StreamWriter("output.txt");

            try
            {
                while (true)
                {
                    j++;
                    if (verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(stuff[j][4] + " - ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(stuff[j][2]+" - ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        string data = stuff[j][1];
                        Console.Write(parseData(data)+" - ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(stuff[j][3]+"\n");
                        
                        if(output)
                            file.WriteLine(stuff[j][4] + " - " + stuff[j][2] + " - " + stuff[j][1] + " - " + stuff[j][3]);
                    }
                    else
                    {
                        Console.WriteLine(stuff[j][2]);
                        
                        if(output)
                            file.WriteLine(stuff[j][2]);
                    }
                }
                
            }
            catch  //come here after the loop
            {
                file.Close();
                if (verbose)
                    Console.ResetColor();

                if (output)
                    Console.WriteLine("A file with the output was created in: " + Environment.CurrentDirectory + "\\output.txt");
                else
                    File.Delete("output.txt");
            }
        }
        private string parseData(string data)
        {
            string year = data.Substring(0, 4);
            string month = data.Substring(4, 2);
            string day = data.Substring(6, 2);
            string hour = data.Substring(8, 2);
            string minute = data.Substring(10, 2);
            
            return year+"-"+month+"-"+day+"T "+hour+":"+minute;
        }
    }
}

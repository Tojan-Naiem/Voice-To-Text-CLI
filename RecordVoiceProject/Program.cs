// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("The program is running");
        //api key
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsetting.json", optional: false).Build();
        string apiKey = config["apiKey:lemonfox"];
        string fileName = "/home/tojan/record.wav";
        string pidFile = "/tmp/recording.pid";
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "arecord",
            Arguments = $"-f cd -t wav {fileName}",
            RedirectStandardInput = false,
            UseShellExecute = true,
            CreateNoWindow = false
        };
        Process recording=null;
        if (args.Length != 0)
        {
            if (args[0] == "record")
            {
                 recording = Process.Start(psi);
                 File.WriteAllText(pidFile,recording.Id.ToString());
            }
            else if (args[0] == "stop")
            {
                if (!File.Exists(pidFile))
                {
                    Console.WriteLine("No PID file found. Recording might not be running.");
                    return;
                }

                int pid = int.Parse(File.ReadAllText(pidFile));
                try
                {
                    Process.GetProcessById(pid).Kill();

                }
                catch
                {
                    Console.WriteLine("No pid found");
                }
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    using (var form = new MultipartFormDataContent())
                    {
                        var audioContent = new ByteArrayContent(File.ReadAllBytes(fileName));
                        audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/wav");
                        form.Add(audioContent,"file","record.wav");
                        form.Add(new StringContent("whisper-1"),"model");
                        var response = await client.PostAsync("https://api.lemonfox.ai/v1/audio/transcriptions", form);
                        
                        Console.WriteLine("The text : ");
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                }
                File.Delete(pidFile);
            }
        }


    }



    
 

}
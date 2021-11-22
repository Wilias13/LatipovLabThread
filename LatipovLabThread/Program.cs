using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Cryptography;


namespace LatipovLabThread
{
    class Data {
        public List<string> passw;
        public int left;
        public int right;
    }

    class Program
    {
        static string filename = "File.txt";
        static string[] hashed = { "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad", "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b", "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f" };
        static private void CreateFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),filename);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            char[] arr = new char[5];
            for (char c='a'; c<='z'; c++)
            {
                
                arr[0] = c;
                for (char x = 'a'; x <= 'z'; x++)
                {
                    arr[1] = x;
                    for (char z = 'a'; z <= 'z'; z++)
                    {
                        arr[2] = z;
                        for (char y = 'a'; y <= 'z'; y++)
                        {
                            arr[3] = y;
                            for (char s = 'a'; s <= 'z'; s++)
                            {
                                arr[4] = s;
                                streamWriter.WriteLine(arr);
                                
                            }
                        }
                    }
                }

            }
            streamWriter.Close();
            fileStream.Close();
        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        static void Main(string[] args)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filename))) 
            {
                CreateFile();     
            }
            List<string> hashed = new List<string>();
            FileStream fileStream = new FileStream((Path.Combine(Directory.GetCurrentDirectory(), "passwords.txt")), FileMode.Open);
            StreamReader sr = new StreamReader(fileStream);
            while (!sr.EndOfStream)
            {
                hashed.Add(sr.ReadLine());
            }
            fileStream.Close();
            sr.Close();
            Console.WriteLine("Enter thread value:");
            int numofthr = Convert.ToInt32(Console.ReadLine());
            FileStream  fs = new FileStream((Path.Combine(Directory.GetCurrentDirectory(), filename)), FileMode.Open);
            StreamReader strr = new StreamReader(fs);
            List<string> pass = new List<string>();
            while (!strr.EndOfStream)
            {
                pass.Add(strr.ReadLine());
            }
            int step = pass.Count / numofthr;
            
            for (int i= 0; i < numofthr; i++)
            {
                Data data = new Data();
                data.left = 0 + i * step;
                data.right = data.left + step;
                data.passw = pass;
                Thread thread = new Thread(new ParameterizedThreadStart(Checker));
                thread.Start(data);
            }

            
        }

        public static void Checker(object data)
        {
            Data d = (Data)data;
            foreach(var hs in hashed)
            {
                for(int i = d.left; i <= d.right; i++)
                {
                    if(hs== sha256_hash(d.passw[i])){
                        Console.WriteLine($"{ d.passw[i]} is  password");
                    }
                }
            }
        }
    }
    

}

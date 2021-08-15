using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findString
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("0 for bytes, 1 for string, 2, for int to bytes");
            switch(Console.ReadKey().KeyChar)
            {
                case '0':
                    Console.Clear();
                    Console.WriteLine("Enter bytes (separated by ,)");
                    var str = Console.ReadLine().Split(',');
                    byte[] bs = new byte[str.Length];
                    for (int i = 0; i < str.Length; i++)
                    {
                        bs[i] = (byte)int.Parse(str[i]);
                    }
                    Console.WriteLine("Enter dir path");
                    var d = Directory.GetFiles(Console.ReadLine(), "*.*", SearchOption.AllDirectories);
                    for (int i = 0; i < d.Length; i++)
                    {
                        var bytes = File.ReadAllBytes(d[i]);
                        for (int ii = 0; ii < bytes.Length; ii++)
                        {
                            if(ii + bs.Length >= bytes.Length)
                            {
                                break;
                            }
                            if(bytes[ii] == bs[0])
                            {
                                bool error = false;
                                for (int iii = 1; iii < bs.Length; iii++)
                                {
                                    if (bytes[ii + iii] != bs[iii])
                                    {
                                        error = true;
                                    }
                                }
                                if(!error)
                                {
                                    Console.WriteLine("Found the bytes in " + d[i]);
                                }
                            }
                           
                        }
                    }
                   
                    break;
                case '1':
                    Console.Clear();
                    Console.WriteLine("Enter string");
                    var strr = Console.ReadLine();
                    Console.WriteLine("Enter dir path");
                    var dd = Directory.GetFiles(Console.ReadLine(), "*.*", SearchOption.AllDirectories);
                    for (int i = 0; i < dd.Length; i++)
                    {
                        if (File.ReadAllText(dd[i]).Contains(strr))
                        {
                            Console.WriteLine("File " + dd[i] + " contains " + strr);
                        }
                    }
                    break;
                case '2':
                    Console.Clear();
                    Console.WriteLine("Enter the number");
                    var num = int.Parse(Console.ReadLine());
                    var bytess = BitConverter.GetBytes(num);
                    Console.WriteLine(String.Join(",", bytess));
                    break;
            }
            Main(null);
        }
    }
}

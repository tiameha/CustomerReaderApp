using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CustomerReaderLib;


namespace CustomerReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool success = false;
            List<string> fileNames = new List<string>();

            // Validate args
            if (args.Length > 0)
            {
                // Verify files
                foreach (string fileName in args)
                {
                    // File exists?
                    if ((success = File.Exists(fileName)) == false)
                    {
                        Console.WriteLine($"Can not locate file: {fileName}");
                        break;
                    }

                    // Valid extension?
                    try
                    {
                        if (!CustomerReader.AllowedFileTypes.Contains(Path.GetExtension(fileName)))
                            break;
                    }
                    catch (Exception)
                    {
                        // Just break and fall to error output
                        break;
                    }

                    // Add to list
                    fileNames.Add(fileName.ToLower());
                }
            }

            // Halt if error
            if (!success)
            {
                Console.WriteLine("File(s) must exist and be of valid type.");
                Console.WriteLine("Valid file extensions are .csv, .xml, .json");
                Console.WriteLine("Example usage: CustomReaderApp.exe file1.csv file2.xml file3.json");
                Console.ReadKey();
                return;
            }

            // Import customer data
            CustomerReader customerReader = new CustomerReader();
            try
            {
                customerReader.Execute(fileNames);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Display Customer data
            Console.WriteLine("Added this many customers: " + customerReader.getCustomers().Count + "\n");
            customerReader.DisplayCustomers();
            Console.ReadLine();
        }

    }
}

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using CustomerReaderLib.Models;

namespace CustomerReaderLib
{
    public class CustomerReader
    {
        // Customer list
        private ListEx<Customer> customers;

        // Types of files allowed for import
        public static string[] AllowedFileTypes = new string[] { ".csv", ".xml", ".json" };

        // Delegate for threading the readers
        delegate void CustomerReaderProc(string fileName);

        // Ctor
        public CustomerReader()
        {
            customers = new ListEx<Customer>();
        }

        // Executes appropriate readers for given file(s)
        public void Execute(List<string> fileNames)
        {
            string fileExt = "";

            List<Task> readerTasks = new List<Task>();

            foreach (string fileName in fileNames)
            {
                CustomerReaderProc customerReaderProc = null;
                fileExt = Path.GetExtension(fileName).ToLower();

                switch (fileExt)
                {
                    case ".csv":
                        customerReaderProc = new CustomerReaderProc(ReadCustomersCsv);
                        break;
                    case ".xml":
                        customerReaderProc = new CustomerReaderProc(ReadCustomersXml);
                        break;
                    case ".json":
                        customerReaderProc = new CustomerReaderProc(ReadCustomersJson);
                        break;
                    default:
                        Console.WriteLine($"File with extension {fileExt} not supported!");
                        break;
                }

                Task readerTask = Task<int>.Run(() => customerReaderProc(fileName));
                readerTasks.Add(readerTask);
            }

            // Wait for all readers to finish
            Task.WaitAll(readerTasks.ToArray());
        }

        // Return the list of Customers
        public List<Customer> getCustomers()
        {
            return customers;
        }


        // This method reads customers from a CSV file and puts them into the customers list.
        private void ReadCustomersCsv(String customer_file_path)
        {

            try
            {
                StreamReader br = new StreamReader(File.Open(customer_file_path, FileMode.Open));
                String line = br.ReadLine();

                while (line != null)
                {
                    String[] attributes = line.Split(',');

                    Customer customer = new Customer();
                    customer.Email = attributes[0];
                    customer.FirstName = attributes[1];
                    customer.LastName = attributes[2];
                    customer.Phone = attributes[3];
                    customer.StreetAddress = attributes[4];
                    customer.City = attributes[5];
                    customer.State = attributes[6];
                    customer.ZipCode = attributes[7];

                    customers.AddEx(customer);
                    line = br.ReadLine();
                }
            }
            catch (IOException ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        // This method reads customers from a XML file and puts them into the customers list.
        private void ReadCustomersXml(String customerFilePath)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(customerFilePath);

                XmlNodeList nList = doc.GetElementsByTagName("Customers");

                for (int temp = 0; temp < nList.Count; temp++)
                {
                    XmlNode nNode = nList[temp];
                    
                    if (nNode.NodeType == XmlNodeType.Element)
                    {
                        Customer customer = new Customer();
                        XmlElement eElement = (XmlElement)nNode;

                        customer.Email = eElement.GetElementsByTagName("Email").Item(0).InnerText;
                        customer.FirstName = eElement.GetElementsByTagName("FirstName").Item(0).InnerText;
                        customer.LastName = eElement.GetElementsByTagName("LastName").Item(0).InnerText;
                        customer.Phone = eElement.GetElementsByTagName("PhoneNumber").Item(0).InnerText;

                        XmlElement aElement = (XmlElement)eElement.GetElementsByTagName("Address").Item(0);

                        customer.StreetAddress = aElement.GetElementsByTagName("StreetAddress").Item(0).InnerText;
                        customer.City = aElement.GetElementsByTagName("City").Item(0).InnerText;
                        customer.State = aElement.GetElementsByTagName("State").Item(0).InnerText;
                        customer.ZipCode = aElement.GetElementsByTagName("ZipCode").Item(0).InnerText;

                        customers.AddEx(customer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        // This method reads customers from a Json file and puts them into the customers list.
        private void ReadCustomersJson(String customersFilePath)
        {
            JsonTextReader reader = new JsonTextReader(System.IO.File.OpenText(customersFilePath));

            try
            {
                JArray obj = (JArray)JToken.ReadFrom(reader);

                foreach (JObject o in obj)
                {
                    Customer customer = new Customer();
                    JObject record = (JObject)o;

                    String Email = (String)record["Email"];
                    customer.Email = Email;

                    String firstName = (String)record["FirstName"];
                    customer.FirstName = firstName;

                    String lastName = (String)record["LastName"];
                    customer.LastName = lastName;

                    String Phone = (String)record["PhoneNumber"];
                    customer.Phone = Phone;

                    JObject address = (JObject)record["Address"];

                    String StreetAddress = (String)address["StreetAddress"];
                    customer.StreetAddress = StreetAddress;

                    String City = (String)address["City"];
                    customer.City = City;

                    String State = (String)address["State"];
                    customer.State = State;

                    String zipCode = (String)address["ZipCode"];
                    customer.ZipCode = zipCode;

                    customers.AddEx(customer);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        // Sanitizes and displays the Customer data to console
        public void DisplayCustomers()
        {
            foreach (Customer customer in customers)
            {
                StringBuilder sb = new StringBuilder();

                // TODO: Refactor the data sanitizing 
                sb.Append($"Email: {customer.Email.Trim().ToLower()}\n");
                string firstName = customer.FirstName.Trim().FirstCharToUpper();
                string lastName = customer.LastName.Trim().FirstCharToUpper();
                sb.Append($"First Name: {firstName}\n");
                sb.Append($"Last Name: {lastName}\n");
                sb.Append($"Full Name: {firstName} {lastName}\n");
                sb.Append($"Phone Number: {customer.Phone.Trim()}\n");
                sb.Append($"Street Address: {customer.StreetAddress.Trim().WordsInSentenceFirstCharToUpper()}\n");
                sb.Append($"City: {customer.City.Trim().FirstCharToUpper()}\n");
                sb.Append($"State: {customer.State.Trim().ToUpper()}\n");
                sb.Append($"Zip Code: {customer.ZipCode.Trim()}\n");

                Console.WriteLine(sb.ToString());
            }
        }
    }

}

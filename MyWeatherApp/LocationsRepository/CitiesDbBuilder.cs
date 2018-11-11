using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;

namespace MyWeatherApp.LocationsRepository
{
    public class CitiesDbBuilder
    {
        private const string ResourceFilePath = @"/home/alter/RiderProjects/MyWeatherApp/MyWeatherApp/city.list.test.json";
        
        public readonly LocationsContext _context;

        public CitiesDbBuilder(LocationsContext context)
        {
            _context = context;
        }

        public void MakeCitiesDbFromJson()
        {
            List<City> citiesList = new List<City>();
            
            try
            {   
                using (StreamReader file = File.OpenText(ResourceFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    citiesList = (List<City>)serializer.Deserialize(file, typeof(List<City>));
                }
                
                AddCitiesToDb(citiesList);
            }
            
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        
        
        
        private void AddCitiesToDb(List<City> list)
        {
            try
            {
                
                    foreach (var city in list)
                    {
                        _context.Cities.Add(city);
                        _context.SaveChanges();
                    }
                
                Console.WriteLine("The database was successfully made!");
            }
            catch (Exception e)
            {
                Console.WriteLine("The database creation failed:");
                Console.WriteLine(e.Message);
            }
        }

        
        
        public FileInfo DownloadSourceFile()
        {
            string URI = "http://bulk.openweathermap.org/sample/city.list.json.gz";
            FileInfo info = null;
            using (WebClient wc = new WebClient())
            {
                string save_path = @"/home/alter/Загрузки/";
                string name = "city.list.json.gz";
                wc.DownloadFile(URI, save_path + name);
                info = new FileInfo(save_path + name);
            }

            return info;
        }


        public void DecompressSourceFile(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }
    }
}
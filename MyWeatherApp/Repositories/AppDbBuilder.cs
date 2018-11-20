using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace MyWeatherApp.Repositories
{
    public class AppDbBuilder
    {
        private const string ResourseFileLink = "http://bulk.openweathermap.org/sample/city.list.json.gz"; 
        
        public void MakeCitiesDbFromJson()
        {
            string filePath = DecompressSourceFile(DownloadSourceFile());
            
            List<City> citiesList = new List<City>();
            
            try
            {   
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    citiesList = (List<City>)serializer.Deserialize(file, typeof(List<City>));
                }
                
                AddCitiesToDb(citiesList);
                File.Delete(filePath);
            }
            
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void AddCitiesToDb(List<City> list)
        {
            Console.WriteLine($"Adding {list.Count} cities to the database...");
            
            try
            {
                using(AppContext context = new AppContext())
                {
                    foreach (var city in list)
                    {
                        
                        context.Cities.Add(city);
                        //context.Coords.Add(city.Coord); // выяснить про внешний ключ
                    }
                    
                    context.SaveChanges();
                }
                
                Console.WriteLine("The database was successfully made!");
            }
            catch (Exception e)
            {
                Console.WriteLine("The database creation failed:");
                Console.WriteLine(e.Message);
            }
        }

        
        
        private FileInfo DownloadSourceFile()
        {
            FileInfo info = null;
            Console.WriteLine("Downloading source files...");
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string savePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"/Загрузки/";
                    string name = "city.list.json.gz";
                    webClient.DownloadFile(ResourseFileLink, savePath + name);
                    info = new FileInfo(savePath + name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Source files downloading failed:");
                Console.WriteLine(e.Message);
            }
            return info;
        }


        private string DecompressSourceFile(FileInfo fileToDecompress)
        {
            string newFileName = null;
            Console.WriteLine("Decompressing source files...");
            try
            {
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                        }
                    }
                    File.Delete(currentFileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Source files decompression failed:");
                Console.WriteLine(e.Message);
            }
            return newFileName;
        }
    }
}
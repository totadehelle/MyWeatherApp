using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using MyWeatherApp.Utility;
using Newtonsoft.Json;

namespace MyWeatherApp.Repositories
{
    public class AppDbBuilder
    {
        private const string ResourceFileLink = "http://bulk.openweathermap.org/sample/city.list.json.gz"; 
        
        public void MakeCitiesDbFromJson()
        {
            var filePath = DecompressSourceFile(DownloadSourceFile());

            try
            {
                List<City> citiesList;
                using (StreamReader file = File.OpenText(filePath))
                {
                    var serializer = new JsonSerializer();
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

        private void AddCitiesToDb(IReadOnlyList<City> list)
        {
            var totalCities = list.Count;
            Console.WriteLine($"Adding {totalCities} cities to the database...");
            
            try
            {
                using(var context = new AppContext())
                {
                    using (var progress = new ProgressBar())
                    {
                        for (var i = 0; i < totalCities; i++)
                        {
                            context.Cities.Add(list[i]);
                            progress.Report((double)i/totalCities);
                        }
                    }
                    Console.WriteLine("Saving the database...");
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
                    var savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    const string name = "city.list.json.gz";
                    webClient.DownloadFile(ResourceFileLink, savePath + name);
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
                    var currentFileName = fileToDecompress.FullName;
                    newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
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
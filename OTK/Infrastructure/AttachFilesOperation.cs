using OTK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OTK.Infrastructure
{
    internal class AttachFiles
    {
        public string FileName { get; set; }
        public string FullFileName;
    }


    internal class AttachListFiles
    {
        int Year;
        string SubFolder;
        public ObservableCollection<AttachFiles> ListFiles { get; set; }
        string NewPath;
        public int id;
#if DEBUG
        static string FileStorage = @"\\ngk-dc-07\FileStore$\OTKD\";
#else
        static string FileStorage = @"\\ngk-dc-07\FileStore$\OTK\";
#endif


        //-----------------------------------------------------------------------------------------------------------
        // конструктор
        //-----------------------------------------------------------------------------------------------------------
        public AttachListFiles(int year, int ID, string folder = null)
        {
            Year = year;
            SubFolder = folder;
            ListFiles = new ObservableCollection<AttachFiles>();
            id = ID;

            string NewFolder = FileStorage + Year.ToString() + "\\" + (string.IsNullOrEmpty(folder) ? null : folder + "\\");
            NewPath = NewFolder + id.ToString() + ".";

            try
            {
                if (!Directory.Exists(NewFolder))
                    Directory.CreateDirectory(NewFolder);
            }
            catch
            {
            }

        }


        //-----------------------------------------------------------------------------------------------------------
        // Получение списка файлов
        //-----------------------------------------------------------------------------------------------------------
        public void AssignFiles(IEnumerable<string> files)
        {
            if (files is null)
                return;

            foreach (var file in files)
            {
                FileInfo info = new FileInfo(file);
                AttachFiles af = new AttachFiles();
                af.FileName = info.Name;
                if(info.Exists)
                    af.FullFileName = file;
                ListFiles.Add(af);
            }
        }


        //--------------------------------------------------------------------------------------------
        // получение нужного пути и созздание диреткории года
        //--------------------------------------------------------------------------------------------
        //private string CurrentPath()
        //{
        //    string NewPath = FileStorage + Year.ToString() + "\\" + 
        //        (string.IsNullOrEmpty(SubFolder) ? null : SubFolder + "\\") + 
        //        id.ToString() + ".";

        //    try
        //    {
        //        if (!Directory.Exists(NewPath))
        //            Directory.CreateDirectory(NewPath);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return NewPath;
        //}


        //-----------------------------------------------------------------------------------------------------------
        // Копивароние файлов на хранение
        //-----------------------------------------------------------------------------------------------------------
        //public void SaveFiles()
        //{
        //    string NewName;

        //    foreach (var item in ListFiles)
        //    {
        //        if (item.FullFileName != null)
        //        {
        //            NewName = NewPath +  item.FileName;
        //            System.IO.File.Copy(item.FullFileName, NewName, true);

        //            // записано, обнуляем
        //            //item.FullFileName = null;
        //        }
        //    }

        //}

        //--------------------------------------------------------------------------------------------
        // Копивароние файлов на хранение
        //--------------------------------------------------------------------------------------------
        public async void AddFilesAsync()
        {
            await Task.Run(() => CopyFiles());
        }


        //--------------------------------------------------------------------------------------------
        // Копио=рование файлов
        //--------------------------------------------------------------------------------------------
        public Task<bool> CopyFiles()
        {
            string NewName;


            foreach (var item in ListFiles)
            {
                if (item.FullFileName != null)
                {
                    NewName = NewPath +  item.FileName;
                     try
                    {
                            File.Copy(item.FullFileName, NewName, true);
                        }
                        catch { };

                        // записано, обнуляем
                        item.FullFileName = null;
                    }
                }
            return Task.FromResult(true);
        }

        //--------------------------------------------------------------------------------------------
        // Удаление файлов
        //--------------------------------------------------------------------------------------------
        public void DeleteFiles()
        {
            string NewName;

            //if (string.IsNullOrEmpty(NewPath))
            //    return;

            foreach (var item in ListFiles)
            {
                NewName = NewPath + item.FileName;
                try
                {
                    File.Delete(NewName);
                }
                catch { }
            }
        }

        //--------------------------------------------------------------------------------------------
        // Получение файла в TEMP папку
        //--------------------------------------------------------------------------------------------
        public string GetFile(string FileName)
        {

            if (string.IsNullOrEmpty(NewPath))
                return null;

            string NewName = NewPath + FileName;

            string TempFileName = Path.GetTempPath() + FileName;

            try
            {
                File.Copy(NewName, TempFileName, true);
            }
            catch
            {
                return null;
            }

            return TempFileName;
        }


        //--------------------------------------------------------------------------------
        // Команда Открыть файл
        //--------------------------------------------------------------------------------
        public void StartFile(string FileName)
        {

            string TempFileName = GetFile(FileName);

            if (TempFileName != null)
                Process.Start(TempFileName);

        }


    }
}

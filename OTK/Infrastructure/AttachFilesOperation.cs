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

    internal class AttachListFiles<T> where T : class, IAddFiles, new()
    {
        private int Year;
        private string SubFolder;
        public ObservableCollection<IAddFiles> ListFiles { get; set; }
        //private string NewPath;
        private int? NumberID;
        public ICollection<IAddFiles> DeleteItem;

#if DEBUG
        private static string FileStorage = @"\\ngk-dc-07\FileStore$\OTKD\";
#else
        private static string FileStorage = @"\\ngk-dc-07\FileStore$\OTK\";
#endif


        //-----------------------------------------------------------------------------------------------------------
        // конструктор
        //-----------------------------------------------------------------------------------------------------------
        public AttachListFiles(int year, /*int ID,*/ string folder = null)
        {
            Year = year;
            SubFolder = folder;
            ListFiles = new ObservableCollection<IAddFiles>();
            DeleteItem = new List<IAddFiles>();

            //id = ID;

            //string NewFolder = FileStorage + Year.ToString() + "\\" + (string.IsNullOrEmpty(folder) ? null : folder + "\\");
            //NewPath = NewFolder + id.ToString() + ".";

            //try
            //{
            //    if (!Directory.Exists(NewFolder))
            //        Directory.CreateDirectory(NewFolder);
            //}
            //catch
            //{
            //}

        }


        //--------------------------------------------------------------------------------------------
        // Получение пути файла и создание папки
        //--------------------------------------------------------------------------------------------
        private string GetPathFiles()
        {
            string NewFolder = FileStorage + Year.ToString() + "\\" + (string.IsNullOrEmpty(SubFolder) ? null : SubFolder + "\\");
            //NewPath = NewFolder + id.ToString() + ".";

            try
            {
                if (!Directory.Exists(NewFolder))
                    Directory.CreateDirectory(NewFolder);
            }
            catch
            {
            }

            return NewFolder;

        }


        //--------------------------------------------------------------------------------------------
        // Добавление файла в список
        //--------------------------------------------------------------------------------------------
        public void AssignFiles(IEnumerable<IAddFiles> ListItems)
        {
            if (ListItems is null)
                return;

            foreach(var item in ListItems)
            {
                ListFiles.Add(item);
                NumberID = item.idParent;
            }
        }



        //-----------------------------------------------------------------------------------------------------------
        // Загрузка списка файлов
        //-----------------------------------------------------------------------------------------------------------
        public void AddFiles(IEnumerable<string> files)
        {
            if (files is null)
                return;

            foreach (var file in files)
            {
                FileInfo info = new FileInfo(file);
                T NewItem = new T();

                NewItem.FileName = info.Name;
                if(info.Exists)
                    NewItem.FullName = file;
                ListFiles.Add(NewItem);
            }
        }


        //--------------------------------------------------------------------------------------------
        // Копировоние файлов на хранение
        //--------------------------------------------------------------------------------------------
        public async void CommitFilesAsync()
        {
            await Task.Run(() => CommitFiles());
        }


        //--------------------------------------------------------------------------------------------
        // Копирование файлов
        //--------------------------------------------------------------------------------------------
        public Task<bool> CommitFiles()
        {
            string NewName;
            string NewPath = GetPathFiles();

            // копирование добавленных файлов
            foreach (var item in ListFiles)
            {
                if (item.FullName != null)
                {
                    NewName = NewPath + item.idParent.ToString() + "." + item.FileName;
                    try
                    {
                        File.Copy(item.FullName, NewName, true);
                    }
                    catch { };

                    // записано, обнуляем
                    item.FullName = null;
                }
            }

            // удаление файлов
            foreach(var item in DeleteItem)
            {
                NewName = NewPath + NumberID.ToString() + "." + item.FileName;
                try
                {
                    File.Delete(NewName);
                }
                catch { }
            }

            return Task.FromResult(true);
        }


        //--------------------------------------------------------------------------------------------
        // Удаление файла из списка
        //--------------------------------------------------------------------------------------------
        public void DeleteFile(IAddFiles item)
        {
            DeleteItem.Add(item);
            ListFiles.Remove(item);
        }

        //--------------------------------------------------------------------------------------------
        // Получение файла в TEMP папку
        //--------------------------------------------------------------------------------------------
        public string GetFile(IAddFiles file)
        {

            //if (string.IsNullOrEmpty(NewPath))
            //    return null;

            string NewPath = GetPathFiles();

            string NewName = NewPath + file.idParent.ToString() + "." + file.FileName;

            string TempFileName = Path.GetTempPath() + file.FileName;

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
        public void StartFile(IAddFiles file)
        {

            string TempFileName = GetFile(file);

            if (TempFileName != null)
                Process.Start(TempFileName);
            else if(file.FullName != null)
                Process.Start(file.FullName);


        }


    }
}

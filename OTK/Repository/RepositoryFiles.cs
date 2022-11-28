using OTK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OTK.Repository
{
    internal class RepositoryFiles
    {
#if DEBUG
        static string FileStorage = @"\\ngk-dc-07\FileStore$\OTKD\";
#else
        static string FileStorage = @"\\ngk-dc-07\FileStore$\OTK\";
#endif

        //--------------------------------------------------------------------------------------------
        // получение нужного пути и созздание диреткории года
        //--------------------------------------------------------------------------------------------
        private string CurrentPath(int Year)
        {
            string NewPath = FileStorage + Year.ToString() + "\\";
            try
            {
                if (!Directory.Exists(NewPath))
                    Directory.CreateDirectory(NewPath);
            }
            catch
            {
                return null;
            }
            return NewPath;
        }

        //--------------------------------------------------------------------------------------------
        // Добавление файлов
        //--------------------------------------------------------------------------------------------
        public void AddFiles(ActionUser CurrentAction)
        {
            string NewName;
            string NewPath = CurrentPath(CurrentAction.Jobs.JobDate.Year);

            if (string.IsNullOrEmpty(NewPath))
                return;

            foreach (ActionFiles item in CurrentAction.ActionFiles)
            {
                if (item.FullName != null)
                {
                    NewName = NewPath + CurrentAction.id.ToString() + "." + item.af_FileName;
                    File.Copy(item.FullName, NewName, true);

                    // записано, обнуляем
                    item.FullName = null;
                }
            }
        }

        //--------------------------------------------------------------------------------------------
        // Добавление файлов
        //--------------------------------------------------------------------------------------------
        public async void AddFilesAsync(ActionUser CurrentAction)
        {
            await Task.Run(() => CopyFiles(CurrentAction));
        }


        //--------------------------------------------------------------------------------------------
        // Копио=рование файлов
        //--------------------------------------------------------------------------------------------
        public Task<bool> CopyFiles(ActionUser CurrentAction)
        {
            string NewName;
            string NewPath = CurrentPath(CurrentAction.Jobs.JobDate.Year);

            if (!string.IsNullOrEmpty(NewPath))
            {

                foreach (ActionFiles item in CurrentAction.ActionFiles)
                {
                    if (item.FullName != null)
                    {
                        NewName = NewPath + CurrentAction.id.ToString() + "." + item.af_FileName;
                        try
                        {
                            File.Copy(item.FullName, NewName, true);
                        }
                        catch { };

                        // записано, обнуляем
                        item.FullName = null;
                    }
                }
            }
            return Task.FromResult(true);
        }

        //--------------------------------------------------------------------------------------------
        // Удаление файлов
        //--------------------------------------------------------------------------------------------
        public void DeleteFiles(Jobs job)
        {
            string NewPath = CurrentPath(job.JobDate.Year);
            string NewName;

            if (string.IsNullOrEmpty(NewPath))
                return;

            foreach (var CurrentAction in job.Action)
            {
                foreach (ActionFiles item in CurrentAction.ActionFiles)
                {
                    NewName = NewPath + CurrentAction.id.ToString() + "." + item.af_FileName;
                    try
                    {
                        File.Delete(NewName);
                    }
                    catch { }
                }
            }
        }

        //--------------------------------------------------------------------------------------------
        // Получение файла в TEMP папку
        //--------------------------------------------------------------------------------------------
        public string GetFile(ActionFiles raFile)
        {
            string NewPath = CurrentPath(raFile.ActionUser.Jobs.JobDate.Year);

            if (string.IsNullOrEmpty(NewPath))
                return null;

            //string NewPath = FileStorage + raFile.RouteOrder.Order.o_date_created.Year.ToString() + "\\";
            string NewName = NewPath + raFile.ActionUser.id.ToString() + "." + raFile.af_FileName;

            string TempFileName = Path.GetTempPath() + raFile.af_FileName;

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
        public void OpenActionFile(ActionFiles actionFile)
        {
            //RepositoryFiles repoFiles = new RepositoryFiles();

            string TempFileName = GetFile(actionFile);

            if (TempFileName != null)
                Process.Start(TempFileName);

        }

    }
}

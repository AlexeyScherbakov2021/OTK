using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.ViewModels.Setting
{
    internal class VendorsUCViewModel : Observable, ISettings
    {
        private readonly RepositoryMSSQL<Vendor> repo;
        public ObservableCollection<Vendor> ListVendor { get; set; }


        public VendorsUCViewModel()
        {
            repo = new RepositoryMSSQL<Vendor>();
            ListVendor = new ObservableCollection<Vendor>( repo.Items.OrderBy(o => o.VendName));
            ListVendor.CollectionChanged += ListVendor_CollectionChanged;
        }

        private void ListVendor_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                Vendor item = (Vendor)e.NewItems[0];
                repo.Add(item, true);
            }
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                Vendor item = (Vendor)e.OldItems[0];
                repo.Delete(item, true);

            }

        }

        public void SaveUsers()
        {
            //repo.AddRange(ListVendor, true);
            repo.Save();
        }
    }
}

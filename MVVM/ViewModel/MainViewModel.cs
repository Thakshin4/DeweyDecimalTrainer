using LibraryDeweyDecimalApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDeweyDecimalApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        // -----
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ReplacingBooksViewCommand { get; set; }
        public RelayCommand IdentifyingAreasViewCommand { get; set; }


        // -----
        public HomeViewModel HomeVM { get; set; }
        public ReplacingBooksViewModel ReplacingBooksVM { get; set; }
        public IdentifyingAreasViewModel IdentifyingAreasVM { get; set; }


        //
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        //
        public MainViewModel()
        {
            //
            HomeVM = new HomeViewModel();
            ReplacingBooksVM = new ReplacingBooksViewModel();
            IdentifyingAreasVM = new IdentifyingAreasViewModel();


            //
            CurrentView = HomeVM;

            // -----
            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            ReplacingBooksViewCommand = new RelayCommand(o =>
            {
                CurrentView = ReplacingBooksVM;
            });

            IdentifyingAreasViewCommand = new RelayCommand(o =>
            {
                CurrentView = IdentifyingAreasVM;
            });
        }
    }
}

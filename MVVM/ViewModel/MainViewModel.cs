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
        public RelayCommand FindingCallNumbersViewCommand { get; set; }

        public RelayCommand TimesViewCommand { get; set; }


        // -----
        public HomeViewModel HomeVM { get; set; }
        public ReplacingBooksViewModel ReplacingBooksVM { get; set; }
        public IdentifyingAreasViewModel IdentifyingAreasVM { get; set; }
        public FindingCallNumbersViewModel FindingCallNumbersVM { get; set; }

        public TimesViewModel TimesVM { get; set; }



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
            FindingCallNumbersVM = new FindingCallNumbersViewModel();
            TimesVM = new TimesViewModel();



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
            FindingCallNumbersViewCommand = new RelayCommand(o =>
            {
                CurrentView = FindingCallNumbersVM;
            });
            TimesViewCommand = new RelayCommand(o =>
            {
                CurrentView = TimesVM;
            });
        }
    }
}

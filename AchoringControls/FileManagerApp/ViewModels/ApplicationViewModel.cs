using FileManagerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManagerApp.ViewModels
{
    public class ApplicationViewModel : ObservableObject
    {
        private ICommand _changePageCommand;
        private IBaseViewModel _currentPageViewModel;
        private List<IBaseViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            // Add available pages
            PageViewModels.Add(new MainViewModel());
            //PageViewModels.Add(new ProductsViewModel());

            // Set starting page
            CurrentPageViewModel = PageViewModels.First();
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IBaseViewModel)p),
                        p => p is IBaseViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IBaseViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IBaseViewModel>();

                return _pageViewModels;
            }
        }

        public IBaseViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }


        private void ChangeViewModel(IBaseViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using TotallySwankWP.DataServices;
using TotallySwankWP.Models;
using TotallySwankWP.Services;

namespace TotallySwankWP.ViewModels
{
  /// <summary>
  /// This class contains properties that a View can data bind to.
  /// <para>
  /// See http://www.galasoft.ch/mvvm
  /// </para>
  /// </summary>
  public class HomeViewModel : ViewModelBase
  {
    private readonly IWOTDDataService _dataService;
    private readonly SimpleNavigationService _navService;

    private ObservableCollection<Entry> _entries;
    public ObservableCollection<Entry> Entries
    {
      get
      {
        return _entries;
      }
      set
      {
        if (_entries != value) {
          _entries = value;
          RaisePropertyChanged(() => Entries);
        }
      }
    }

    private bool _loading = true;
    public bool Loading
    {
      get
      {
        return _loading;
      }
      set
      {
        if (_loading != value) {
          _loading = value;
          RaisePropertyChanged(() => Loading);

          // we also want a new loading phrase for next time!
          RaisePropertyChanged(() => LoadingPhrase);

          // also update dependent commands
          NavToSearchPage.RaiseCanExecuteChanged();
        }
      }
    }

    private RelayCommand _navToSearchPage;
    public RelayCommand NavToSearchPage
    {
      get
      {
        return _navToSearchPage
            ?? (_navToSearchPage = new RelayCommand(
                                  () => {
                                    _navService.Navigate(ViewModelLocator.SEARCH_PATH);
                                  },
                                  () => !Loading));
      }
    }

    #region loading phrases

    private string[] _loadingPhrases = {"Loadin' definitions",
                                        "Gettin' words",
                                        "Retrieving English",
                                        "Slinging slang",
                                        "Obtaining entries",
                                        "Internetting",
                                        "Getting down to business",
                                        "Dictionary-ing",
                                        "Trawling for language",
                                        "Stealing messages from The Sims",
                                        "Using your data plan",
                                        "Pretending to be busy",
                                        "Helpful Message",
                                        "Doing something",
                                        "OPERATING"};
    public string LoadingPhrase
    {
      get
      {
        return _loadingPhrases[new Random().Next(_loadingPhrases.Length)];
      }
    }

    #endregion

    public HomeViewModel(IWOTDDataService dataService, SimpleNavigationService navService)
    {
      _dataService = dataService;
      _navService = navService;

      Loading = true;
      _dataService.GetEntries().ContinueWith(processEntries);
    }

    private void processEntries (Task<IEnumerable<Entry>> entryTask)
    {
      var e = entryTask.Exception;
      if (e != null) {
        if (e.InnerException is WebException)
        {
            var entries = entryTask.Result ?? new List<Entry>();
          ((List<Entry>)entries).Add(
            new Entry("Woah dude!",
                      "We couldn't connect to the Urban Dictionary. You sure that you're connected to the internet?",
                      "FAILURE TO LOAD ABORT ABORT")
          );
        }
      }

      Entries = new ObservableCollection<Entry>(entryTask.Result);

      Loading = false;
    }
  }
}
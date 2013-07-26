using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using TotallySwankWP.DataServices;
using TotallySwankWP.Models;

namespace TotallySwankWP.ViewModels
{
  /// <summary>
  /// This class contains properties that a View can data bind to.
  /// <para>
  /// See http://www.galasoft.ch/mvvm
  /// </para>
  /// </summary>
  public class SearchViewModel : ViewModelBase
  {
    private readonly ISearchDataService _dataService;

    private ObservableCollection<Entry> _entries = new ObservableCollection<Entry>();
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

    private bool _loading = false;
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
        }
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

    #region commands

    private RelayCommand _loaded;
    public RelayCommand Loaded
    {
      get
      {
        return _loaded ?? (_loaded = new RelayCommand(onLoad));
      }
    }
    
    private RelayCommand<string> _search;
    public RelayCommand<string> Search
    {
      get
      {
        return _search ?? (_search = new RelayCommand<string>(
            (s) => _dataService.GetEntries(s, processEntries),
            (s) => !string.IsNullOrWhiteSpace(s)));
      }
    }

    private RelayCommand _searchAgain;
    public RelayCommand SearchAgain
    {
      get
      {
        return _searchAgain
            ?? (_searchAgain = new RelayCommand(
                                  () => {
                                    MessengerInstance.Send<String>("", "RefocusToken");
                                  }));
      }
    }

    private RelayCommand _openBrowser;
    public RelayCommand OpenBrowser
    {
      get
      {
        return _openBrowser ??
          (_openBrowser = new RelayCommand(
              () =>
              {
                WebBrowserTask wbt = new WebBrowserTask();
                Uri pageUri = new Uri("http://urbandictionary.com/define.php?term=" + Uri.EscapeDataString(Entries[0].Name), UriKind.Absolute);

                wbt.Uri = pageUri;
                wbt.Show();
              },
              () =>
              {
                return Entries.Count > 0 && Entries[0].Name.Equals("Woah dude!") != true;
              }
            )
          );
      }
    }
            
    #endregion

    public SearchViewModel(ISearchDataService dataService)
    {
      _dataService = dataService;
      
      MessengerInstance.Register<string>(this, "SearchToken", startSearch);
    }

    private void startSearch(string query)
    {
      Loading = true;
      onLoad();

      _dataService.GetEntries(query, processEntries);
    }

    private void onLoad()
    {
      if (Entries != null && Entries.Count > 0) Entries.Clear();
    }

    private void processEntries(IEnumerable<Entry> entries, Exception e)
    {
      if (e != null) {
        if (e is ArgumentException) {
          entries = entries ?? new List<Entry>();
          ((List<Entry>)entries).Add(
            new Entry("Woah dude!",
                      "We couldn't find any words that matched your query. You sure you spelled it right?",
                      "I don't know what happened here.")
          );
        }
        else if (e is WebException)
        {
          entries = entries ?? new List<Entry>();
          ((List<Entry>)entries).Add(
            new Entry("Woah dude!",
                      "We couldn't connect to the Urban Dictionary. You sure that you're connected to the internet?",
                      "FAILURE TO LOAD ABORT ABORT")
          );
        }
      }

      Entries.Clear();

      foreach (var entry in entries) Entries.Add(entry);

      OpenBrowser.RaiseCanExecuteChanged();

      MessengerInstance.Send<String>("unfocus", "UnfocusToken");      

      Loading = false;
    }

  }
}
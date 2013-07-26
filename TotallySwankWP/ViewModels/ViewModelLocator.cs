/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TotallySwankWP.ViewModels"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TotallySwankWP.DataServices;
using TotallySwankWP.Services;

namespace TotallySwankWP.ViewModels
{
  /// <summary>
  /// This class contains static references to all the view models in the
  /// application and provides an entry point for the bindings.
  /// <para>
  /// See http://www.galasoft.ch/mvvm
  /// </para>
  /// </summary>
  public class ViewModelLocator
  {
    public const string SEARCH_PATH = "/Views/SearchView.xaml";

    static ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      if (ViewModelBase.IsInDesignModeStatic) {
        SimpleIoc.Default.Register<IWOTDDataService, Design.DesignWOTDDataService>();
      }
      else {
        SimpleIoc.Default.Register<IWOTDDataService, WOTDDataService>();
        SimpleIoc.Default.Register<ISearchDataService, SearchDataService>();
      }

      SimpleIoc.Default.Register<HomeViewModel>();
      SimpleIoc.Default.Register<SearchViewModel>();

      SimpleIoc.Default.Register<SimpleNavigationService>();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
    public HomeViewModel Home
    {
      get
      {
        return ServiceLocator.Current.GetInstance<HomeViewModel>();
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
    public SearchViewModel Search
    {
      get
      {
        return ServiceLocator.Current.GetInstance<SearchViewModel>();
      }
    }
  }
}
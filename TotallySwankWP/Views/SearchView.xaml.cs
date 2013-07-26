using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xna.Framework.Media;

namespace TotallySwankWP.Views
{
  public partial class SearchView : PhoneApplicationPage
  {
    private bool _animatingOff = false;

    // Constructor
    public SearchView()
    {
      InitializeComponent();

      Messenger.Default.Register<String>(this, "UnfocusToken", unfocus);
      Messenger.Default.Register<String>(this, "RefocusToken", refocus);
    }    

    private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
    {
      SearchBar.Focus();
    }

    private void SearchBar_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.Enter) {
        Messenger.Default.Send<String>(SearchBar.Text, "SearchToken");
      }
    }

    private void unfocus(string s)
    {
      if (_animatingOff == false)
      {
        _animatingOff = true;
        
        SearchFlip.Focus();

        TextBoxSlideoff.Begin();

        TextBoxSlideoff.Completed += (e, se) =>
        {
          SearchBar.Visibility = Visibility.Collapsed;
          _animatingOff = false;
        };
      }
    }

    private void refocus(string s)
    {
      SearchBar.Visibility = Visibility.Visible;

      TextBoxSlideoff.Stop();
      TextBoxSlideoff.SeekAlignedToLastTick(new TimeSpan(0));

      SearchBar.Focus();
      SearchBar.SelectAll();
    }

    private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
    {
      unfocus("");
    }
  }
}
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace TotallySwankWP.Services
{
  public class SimpleNavigationService
  {
    private PhoneApplicationFrame _mainFrame;
    public event NavigatingCancelEventHandler Navigating;

    public void Navigate(string uri)
    {
      this.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
    }

    public void Navigate(Uri uri)
    {
      if (ensureMainFrame()) _mainFrame.Navigate(uri);
    }

    public void Navigate(Uri uri, IDictionary<string, string> parameters)
    {
      this.Navigate(uri.OriginalString, parameters);
    }

    public void Navigate(string uri, IDictionary<string, string> parameters)
    {
      if (ensureMainFrame()) {
        StringBuilder uriBuilder = new StringBuilder();
        uriBuilder.Append(uri);

        if (parameters != null && parameters.Count > 0) {
          uriBuilder.Append("?");
          bool preprendAmp = false;

          foreach (KeyValuePair<string, string> paramPair in parameters) {
            if (preprendAmp) uriBuilder.Append("&");

            uriBuilder.AppendFormat("{0}={1}", paramPair.Key, paramPair.Value);
            preprendAmp = true;
          }
        }

        uri = uriBuilder.ToString();
        _mainFrame.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
      }
    }

    public void GoBack()
    {
      if (ensureMainFrame() && _mainFrame.CanGoBack) _mainFrame.GoBack();
    }

    public void ClearBackStack()
    {
      if (ensureMainFrame()) {
        while (_mainFrame.CanGoBack) {
          Deployment.Current.Dispatcher.BeginInvoke(() => _mainFrame.RemoveBackEntry());
        }
      }
    }

    private bool ensureMainFrame()
    {
      if (_mainFrame != null) return true;

      _mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;

      if (_mainFrame != null) {

        // allow others to subscribe to navigation events
        _mainFrame.Navigating += (s, e) => {
          if (Navigating != null) Navigating(s, e);
        };

        return true;
      }

      return false;
    }

  }
}

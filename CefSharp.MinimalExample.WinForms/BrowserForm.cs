// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp;
using CefSharp.DevTools.IO;
using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class BrowserForm : Form
    {
        
        private bool _allowClose = false; // solo true cuando uses tu clave secreta

#if DEBUG
        private const string Build = "Debug";
#else
        private const string Build = "Release";
#endif
        private readonly string title = "CefSharp.MinimalExample.WinForms (" + Build + ")";
        private readonly ChromiumWebBrowser browser;

        public BrowserForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            KeyPreview = true;

            Text = title;

            // evita cierres "normales" (X, Alt+F4 transformado, etc.)
            this.FormClosing += (s, e) =>
            {
                if (!_allowClose)
                {
                    e.Cancel = true;
                }
            };

            browser = new ChromiumWebBrowser("portalbac.baccredomatic.com");

            // Handlers clave
            browser.KeyboardHandler = new KioskKeyboardHandler(
                exitAction: () => SafeExit(),
                f6Action: () => BeginInvoke(new Action(OnF6))
            );
            browser.MenuHandler = new NoMenuHandler();
            browser.LifeSpanHandler = new SingleHostLifeSpanHandler();
            browser.RequestHandler = new KioskRequestHandler(new[]
            {
                "portalbac.baccredomatic.com",           // <-- pon tus hosts permitidos aquí
            });

            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadingStateChanged += OnLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.LoadError += OnBrowserLoadError;

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
               Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

#if NETCOREAPP
            // .NET Core
            var environment = string.Format("Environment: {0}, Runtime: {1}",
                System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant(),
                System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
#else
            // .NET Framework
            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var environment = String.Format("Environment: {0}", bitness);
#endif

            DisplayOutput(string.Format("{0}, {1}", version, environment));
        }

        // Mata el SC_CLOSE que manda Alt+F4 (y otros cierres del sistema)
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && ((int)m.WParam & 0xFFF0) == SC_CLOSE)
            {
                // Ignorar el cierre si no has autorizado salir
                if (!_allowClose) return;
            }

            base.WndProc(ref m);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F6)
            {
                OnF6();              // <-- Tu acción de F6
                return true;         // Consumimos la tecla aquí
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnF6()
        {
            // Ejemplo A: recargar forzando cache limpio
            browser.Reload(ignoreCache: true);
            MessageBox.Show("F6: recarga forzando cache limpio");

            // Ejemplo B (alternativo): ir a una URL de mantenimiento
            // browser.Load("https://mi-panel-interno.local");

            // Ejemplo C (alternativo): abrir un diálogo admin (PIN) y mostrar opciones
            // using (var dlg = new AdminDialog()) dlg.ShowDialog(this);
        }

        private bool SafeExit()
        {
            _allowClose = true;     // autoriza cerrar
            if (InvokeRequired)
                BeginInvoke(new Action(Close));
            else
                Close();
            return true;
        }

        private void OnBrowserLoadError(object sender, LoadErrorEventArgs e)
        {
            //Actions that trigger a download will raise an aborted error.
            //Aborted is generally safe to ignore
            if (e.ErrorCode == CefErrorCode.Aborted)
            {
                return;
            }

            var errorHtml = string.Format("<html><body><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              e.FailedUrl, e.ErrorText, e.ErrorCode);

            _ = e.Browser.SetMainFrameDocumentContentAsync(errorHtml);

            //AddressChanged isn't called for failed Urls so we need to manually update the Url TextBox
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = e.FailedUrl);
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var b = ((ChromiumWebBrowser)sender);

            this.InvokeOnUiThreadIfRequired(() => b.Focus());
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Text = title + " - " + args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(action: () => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading)
        {
            goButton.Text = isLoading ?
                "Stop" :
                "Go";
            goButton.Image = isLoading ?
                Properties.Resources.nav_plain_red :
                Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        public void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string urlString)
        {
            // No action unless the user types in some sort of url
            if (string.IsNullOrEmpty(urlString))
            {
                return;
            }

            Uri url;

            var success = Uri.TryCreate(urlString, UriKind.RelativeOrAbsolute, out url);

            // Basic parsing was a success, now we need to perform additional checks
            if (success)
            {
                // Load absolute urls directly.
                // You may wish to validate the scheme is http/https
                // e.g. url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps
                if (url.IsAbsoluteUri)
                {
                    browser.LoadUrl(urlString);

                    return;
                }

                // Relative Url
                // We'll do some additional checks to see if we can load the Url
                // or if we pass the url off to the search engine
                var hostNameType = Uri.CheckHostName(urlString);

                if (hostNameType == UriHostNameType.IPv4 || hostNameType == UriHostNameType.IPv6)
                {
                    browser.LoadUrl(urlString);

                    if (browser.CanSelect)
                    {
                        browser.Select();
                    }

                    return;
                }

                if (hostNameType == UriHostNameType.Dns)
                {
                    try
                    {
                        var hostEntry = Dns.GetHostEntry(urlString);
                        if (hostEntry.AddressList.Length > 0)
                        {
                            browser.LoadUrl(urlString);

                            if (browser.CanSelect)
                            {
                                browser.Select();
                            }

                            return;
                        }
                    }
                    catch (Exception)
                    {
                        // Failed to resolve the host
                    }
                }
            }

            // Failed parsing load urlString is a search engine
            var searchUrl = "https://www.google.com/search?q=" + Uri.EscapeDataString(urlString);

            browser.LoadUrl(searchUrl);

            if (browser.CanSelect)
            {
                browser.Select();
            }
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void BrowserForm_Load(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Bloquea atajos típicos (Alt+F4, Ctrl+N/T/W, F11, Ctrl+Shift+I)
    /// y ofrece una "clave" para salir: Ctrl+Shift+S
    /// </summary>
    public class KioskKeyboardHandler : IKeyboardHandler
    {
        private readonly Func<bool> exitAction;
        private readonly Action f6Action;   // ← añade esta acción

        public KioskKeyboardHandler(Func<bool> exitAction, Action f6Action = null) { 
            this.exitAction = exitAction; 
            this.f6Action = f6Action;
        }

        public bool OnPreKeyEvent(IWebBrowser browser, IBrowser ibrowser, KeyType type,
                                  int windowsKeyCode, int nativeKeyCode,
                                  CefEventFlags modifiers, bool isSystemKey,
                                  ref bool isKeyboardShortcut)
        {
            if (type != KeyType.KeyDown && type != KeyType.RawKeyDown) return false;

            bool ctrl = modifiers.HasFlag(CefEventFlags.ControlDown);
            bool alt = modifiers.HasFlag(CefEventFlags.AltDown);
            bool shift = modifiers.HasFlag(CefEventFlags.ShiftDown);

            // Bloquear combinaciones típicas de escape
            if ((alt && windowsKeyCode == (int)Keys.F4) ||                          // Alt+F4
                (ctrl && (windowsKeyCode == (int)Keys.N ||                          // Ctrl+N
                          windowsKeyCode == (int)Keys.T ||                          // Ctrl+T
                          windowsKeyCode == (int)Keys.W)) ||                        // Ctrl+W
                (windowsKeyCode == (int)Keys.F11) ||                                // F11 (full-screen)
                (ctrl && shift && windowsKeyCode == (int)Keys.I))                   // Ctrl+Shift+I (DevTools)
            {
                return true; // Consumir
            }

            // "Clave" para salir: Ctrl+Shift+S (cámbiala por un PIN si quieres)
            if (ctrl && shift && windowsKeyCode == (int)Keys.S)
            {
                return exitAction?.Invoke() ?? true;
            }

            // ✅ F6 aquí
            if (windowsKeyCode == (int)Keys.F6)
            {
                // Ejecuta en hilo de UI si hace falta
                if (f6Action != null)
                {
                    try { f6Action(); } catch { /* log opcional */ }
                }
                return true; // consumimos F6
            }

            return false; // No consumido
        }

        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type,
                               int windowsKeyCode, int nativeKeyCode,
                               CefEventFlags modifiers, bool isSystemKey) => false;
    }

    /// <summary>Quita el menú contextual (clic derecho).</summary>
    public class NoMenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser c, IBrowser b, IFrame f, IContextMenuParams p, IMenuModel m) => m.Clear();
        public bool OnContextMenuCommand(IWebBrowser c, IBrowser b, IFrame f, IContextMenuParams p, CefMenuCommand cmd, CefEventFlags e) => false;
        public void OnContextMenuDismissed(IWebBrowser c, IBrowser b, IFrame f) { }
        public bool RunContextMenu(IWebBrowser c, IBrowser b, IFrame f, IContextMenuParams p, IMenuModel m, IRunContextMenuCallback cb) => false;
    }

    /// <summary>Evita que se abran nuevas ventanas/pestañas: todo va en la misma.</summary>
    public class SingleHostLifeSpanHandler : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) => false;
        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

        public bool OnBeforePopup(IWebBrowser c, IBrowser b, IFrame f,
            string targetUrl, string targetFrameName,
            WindowOpenDisposition disposition, bool userGesture,
            IPopupFeatures features, IWindowInfo windowInfo,
            IBrowserSettings settings, ref bool noJavascriptAccess,
            out IWebBrowser newBrowser)
        {
            newBrowser = null;
            c.Load(targetUrl); // redirige el popup a la misma vista
            return true;       // cancela la creación de nueva ventana
        }
    }

    /// <summary>Whitelist de hosts permitidos; bloquea el resto.</summary>
    public class KioskRequestHandler : CefSharp.Handler.RequestHandler
    {
        private readonly HashSet<string> allowedHosts;

        public KioskRequestHandler(IEnumerable<string> hosts)
        {
            allowedHosts = new HashSet<string>(hosts ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        }

        protected override bool OnBeforeBrowse(IWebBrowser c, IBrowser b, IFrame f, IRequest r, bool userGesture, bool isRedirect)
        {
            try
            {
                var uri = new Uri(r.Url);
                if (allowedHosts.Count > 0 && !allowedHosts.Contains(uri.Host))
                    return true; // cancelar navegación
            }
            catch
            {
                return true; // URL inválida => bloquear
            }
            return false; // permitir
        }
    }

    /// <summary>Helpers opcionales para reiniciar/apagar Windows.</summary>
    public static class SystemActions
    {
        public static void RebootNow()
        {
            Process.Start(new ProcessStartInfo("shutdown", "/r /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }

        public static void PowerOffNow()
        {
            Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }
    }

}

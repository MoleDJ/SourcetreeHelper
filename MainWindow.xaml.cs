using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibGit2Sharp;

namespace SourceTreeHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DoExecute();
        }

        void DoExecute()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                MessageBox.Show("Not arguments received");
                Application.Current.Shutdown();
            }
            switch (args[1])
            {
                case "OpenGitRepo":
                    {
                        var path = args[2];
                        var repo = new Repository(path);
                        var url = repo.Config.Get<string>("remote.origin.url").Value;
                        if (url.EndsWith(".git"))
                        {
                            url = url.Substring(0, url.Length - 4);
                        }
                        System.Diagnostics.Process.Start(url);
                        Application.Current.Shutdown();
                        break;
                        break;
                    }
                case "OpenGitCommit":
                {
                    var path = args[2];
                    var shaCommit = args[3];
                    var repo = new Repository(path);
                    var url = repo.Config.Get<string>("remote.origin.url").Value;
                    if (url.EndsWith(".git"))
                    {
                        url = url.Substring(0, url.Length - 4);
                    }
                    url += "/commit/" + shaCommit;
                    System.Diagnostics.Process.Start(url);
                    Application.Current.Shutdown();
                    break;
                }
                case "UpdateSmartbanner":
                {
                    var path = args[2];
                    var repoDir = System.IO.Directory.GetDirectories(path, "smartbanner", SearchOption.AllDirectories);
                    if (repoDir.Length == 1)
                    {
                        var smartbannerRepoPath = repoDir[0];
                        var smartbannerPath = System.Configuration.ConfigurationManager.AppSettings["smartbannerPath"];
                        var bcCompare = System.Configuration.ConfigurationManager.AppSettings["bcPath"];
                        var arguments = "\"" + smartbannerPath + "\" \"" + smartbannerRepoPath + "\"";
                        System.Diagnostics.Process.Start(bcCompare, arguments);
                    }
                    else
                    {
                        MessageBox.Show("This repository does not have smartbanner folder.");
                    }
                    Application.Current.Shutdown();
                    
                    break;
                }
                case "UpdateTTClasses":
                {
                    var path = args[2];
                    var classTTConnectionFiles = System.IO.Directory.GetFiles(path, "classTTConnection.php", SearchOption.AllDirectories);
                    if (classTTConnectionFiles.Length == 1)
                    {
                        var ttlibRepoPath = classTTConnectionFiles[0].Replace("classTTConnection.php","");
                        var ttlibPath = System.Configuration.ConfigurationManager.AppSettings["ttlibPath"];
                        var bcCompare = System.Configuration.ConfigurationManager.AppSettings["bcPath"];
                        var arguments = "\"" + ttlibPath + "\" \"" + ttlibRepoPath + "\"";
                        System.Diagnostics.Process.Start(bcCompare, arguments);
                    }
                    else
                    {
                        MessageBox.Show("This repository does not have classTTConnection.php file.");
                    }
                    Application.Current.Shutdown();

                    break;
                }
                case "UpdateBasePlugin":
                {
                    var path = args[2];
                    var basePluginFiles = System.IO.Directory.GetFiles(path, "MbqAppEnv.php", SearchOption.AllDirectories);
                    if (basePluginFiles.Length > 0)
                    {
                        var file = basePluginFiles.FirstOrDefault(p => p.Contains(@"\mobiquo\"));
                        if (file == null)
                        {
                            file = basePluginFiles.First();
                        }
                        var repoPath = file.Replace("MbqAppEnv.php", "");
                        var basePluginPath = System.Configuration.ConfigurationManager.AppSettings["basePluginPath"];
                        var bcCompare = System.Configuration.ConfigurationManager.AppSettings["bcPath"];
                        var arguments = "\"" + basePluginPath + "\" \"" + repoPath + "\"";
                        System.Diagnostics.Process.Start(bcCompare, arguments);
                    }
                    else
                    {
                        MessageBox.Show("This repository does not have MbqAppEnv.php file so BasePlugin not implemented.");
                    }
                    Application.Current.Shutdown();
                    break;
                }
                default:
                {
                    MessageBox.Show(args[1] + " operation not supported.");
                    break;
                }
            }

        }
    }
}

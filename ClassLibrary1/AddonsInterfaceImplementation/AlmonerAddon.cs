using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;

namespace LibraryForPaint
{
    internal class AlmonerAddon : IAlmonerAddon
    {
        private Dictionary<string, IAddons> _addonsCache = new Dictionary<string, IAddons>();
        private string _trustedDllsRegistryKey = "Software\\MDIPaintFox\\TrustedDlls";
        private string _autoLoadsDllsRegistryKey = "Software\\MDIPaintFox\\AutoLoads";
        private List<string> _trustedDlls = new List<string>();
        private List<string> _autoLoadDlls = new List<string>();

        public AlmonerAddon()
        {
            LoadTrustedDlls();
            LoadAutoLoadDlls();
            ScanAndLoadAddons();
        }

        private void LoadTrustedDlls()
        {
            using (var key = Registry.CurrentUser.CreateSubKey(_trustedDllsRegistryKey))
            {
                var trusted = key.GetValue("TrustedList", "")?.ToString() ?? "";
                _trustedDlls = trusted.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToList();
            }
        }
        private void LoadAutoLoadDlls()
        {
            using (var key = Registry.CurrentUser.CreateSubKey(_autoLoadsDllsRegistryKey))
            {
                var autoLoad = key.GetValue("AutoLoadList", "")?.ToString() ?? "";
                _autoLoadDlls = autoLoad.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToList();
            }
        }

        private void SaveTrustedDll(InfoAddon dllPath)
        {
            if (!_trustedDlls.Contains(dllPath.DllName))
            {
                _trustedDlls.Add(dllPath.DllName);
                using (var key = Registry.CurrentUser.CreateSubKey(_trustedDllsRegistryKey))
                {
                    key.SetValue("TrustedList", string.Join(";", _trustedDlls));
                }
            }
        }

        public void SetAutoLoad(InfoAddon dllPath, bool autoLoad)
        {
            if (autoLoad && !_autoLoadDlls.Contains(dllPath.DllName))
            {
                _autoLoadDlls.Add(dllPath.DllName);
            }
            else if (!autoLoad)
            {
                _autoLoadDlls.Remove(dllPath.DllName);
            }

            using (var key = Registry.CurrentUser.CreateSubKey(_autoLoadsDllsRegistryKey))
            {
                key.SetValue("AutoLoadList", string.Join(";", _autoLoadDlls));
            }
        }

        public IAddons[] GetAutoLoadAddons()
        {
            var result = new List<IAddons>();

            foreach (var dllPath in _autoLoadDlls)
            {
                try
                {
                    var addons = _addonsCache.Values
                        .Where(addon => addon.GetType().Assembly.Location == dllPath)
                        .ToArray();

                    result.AddRange(addons);
                }
                catch
                {
                }
            }

            return result.ToArray();
        }

        public bool IsAutoLoad(InfoAddon dllPath)
        {
            return _autoLoadDlls.Contains(dllPath.DllName);
        }

        private void ScanAndLoadAddons()
        {
            _addonsCache.Clear();
            var dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            foreach (var dll in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    var addonTypes = assembly.GetTypes()
                        .Where(t => typeof(IAddons).IsAssignableFrom(t) && !t.IsInterface);

                    foreach (var type in addonTypes)
                    {
                        var addon = (IAddons)Activator.CreateInstance(type);
                        var info = addon.InfoAddons;
                        info.DllName = assembly.FullName;
                        string fullName = $"{info.Name}, {info.Description}, {info.DllName}";
                        _addonsCache[fullName] = addon;
                    }
                }
                catch 
                {

                }
            }
        }

        public InfoAddon[] GetAddonsInfo()
        {
            if (Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Length != _addonsCache.Count)
            {
                ScanAndLoadAddons();
            }
            return _addonsCache.Select(x => new InfoAddon
            {
                Name = x.Value.InfoAddons.Name,
                Description = x.Value.InfoAddons.Description,
                DllName = x.Value.GetType().Assembly.FullName
            }
            ).ToArray();
        }

        public IAddons GetAddonsToGroup(InfoAddon info)
        {
            var dllPath = info.DllName;

            if (!_trustedDlls.Contains(dllPath))
            {
                var result = MessageBox.Show(
                    $"Доверяете ли вы DLL:\n{dllPath}?\n\n" +
                    $"Предупреждение: Загрузка непроверенных DLL может быть опасна!",
                    "Подтверждение безопасности",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    SaveTrustedDll(info);
                }
                else
                {
                    return null;
                }
            }
            string fullName = $"{info.Name}, {info.Description}, {info.DllName}";
            return _addonsCache.TryGetValue(fullName, out var addon) ? addon : null;
        }
    }
}

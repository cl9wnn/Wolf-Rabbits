using System.Reflection;
using Contract;
namespace Simulation
{
    public partial class MainPage : ContentPage
    {   
        private Assembly _currentAssembly;
        private string _path;

        public MainPage()
        {
            InitializeComponent();
            AddGetDllButton();
        }

        private void AddGetDllButton()
        {
            dllButton.Clicked += async (sender, e) =>
            {
                PickDllFile();
            };
        }

        private async Task PickDllFile()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {
                        DevicePlatform.WinUI, new[] {".dll"}
                    }
                });
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions()
                {
                    PickerTitle = "Выберите сборку",
                    FileTypes = customFileType
                });

                _path = result.FullPath;
                UploadNewDllFile();
            }
            catch
            {
                await Console.Out.WriteLineAsync("Ошибка!");
            }
        }

        private void UploadNewDllFile()
        {
            Assembly asm = Assembly.LoadFrom(_path);
            _currentAssembly = asm;
            CheckContract(_currentAssembly);
        }

        private async void CheckContract(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            bool hasImplementation = types.Any(t => typeof(IGameOfLife).IsAssignableFrom(t) && t.IsClass);
            
            if (hasImplementation)
            {
                await Navigation.PushAsync(new SecondPage(_currentAssembly));
            }
            else 
                await DisplayAlert("Warning", "В вашем dll. файле нет нужного контракта", "Ок");
        }

    }
}
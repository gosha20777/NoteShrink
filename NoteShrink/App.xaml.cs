using Avalonia;
using Avalonia.Markup.Xaml;

namespace NoteShrink
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}
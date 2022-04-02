using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GUI_20212202_CM7A68.MenuWindows.ViewModels
{
    public class MainMenuWindowViewModel
    {
        public List<string> Maps { get; set; }
        public List<string> PlayerOneSkins { get; set; }
        public List<string> PlayerTwoSkins { get; set; }
        public MainMenuWindowViewModel()
        {
            var asd = Directory.GetCurrentDirectory();
            Maps = Directory.GetFiles(Path.Combine(asd,"Renderer", "Images", "Backgrounds"), "*.jpg").ToList();
            PlayerOneSkins = Directory.GetFiles(Path.Combine(asd,"Renderer", "Images", "Robots"), "*.png").ToList();
            PlayerTwoSkins = Directory.GetFiles(Path.Combine(asd,"Renderer", "Images", "Robots"), "*.png").ToList();
            ;
        }
    }
}

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace SkillProfiWPF
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath)
            {
                string projectRootPath = @"C:\Users\RusinA01\Desktop\C#\FinalSkillProfi\SkillProfi-main22\SkillProfi-main\SkillProfi";
                return Path.Combine(projectRootPath, "wwwroot", "images", imagePath);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

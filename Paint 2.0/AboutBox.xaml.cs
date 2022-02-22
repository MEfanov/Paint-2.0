using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Reflection;

namespace Paint_2._0
{
    /// <summary>
    /// Логика взаимодействия для AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void InfoPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            
            AssemblyTitleAttribute? titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            AssemblyCompanyAttribute? companyAttribute = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            AssemblyProductAttribute? productAttribute = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            AssemblyCopyrightAttribute? copyrightAttribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            AssemblyTrademarkAttribute? trademarkAttribute = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>();
            AssemblyVersionAttribute? versionAttribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            AssemblyFileVersionAttribute? fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            AssemblyDescriptionAttribute? descriptionAttribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();

            if (titleAttribute != null)
                TitleBox.Text = titleAttribute.Title;
            if (companyAttribute != null)
                CompanyBox.Text = companyAttribute.Company;
            if (productAttribute != null)
                ProductBox.Text = productAttribute.Product;
            if (copyrightAttribute != null)
                CopyrightBox.Text = copyrightAttribute.Copyright;
            if (trademarkAttribute != null)
                TrademarkBox.Text = trademarkAttribute.Trademark;
            if (versionAttribute != null)
                AssemblyVersionBox.Text = versionAttribute.Version;
            if (fileVersionAttribute != null)
                FileVersionBox.Text = fileVersionAttribute.Version;
            if (descriptionAttribute != null)
                DescriptionBox.Text = descriptionAttribute.Description;
        }
    }
}

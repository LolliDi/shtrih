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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shtrih
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GenerateCode();
        }



        public void GenerateCode()
        {
            Random random = new Random();
            DateTime date = new DateTime(2000,1,1);
            date = date.AddDays(random.Next(36500));

            string leftCodeForm = date.ToString("ddMMyy");
            

            string rightCodeForm = "";
            for(int i=0;i<6;i++)
            {
                rightCodeForm += "" + random.Next(0, 10);
            }
            string code = leftCodeForm + rightCodeForm;
            string kontrolPart;
            int kontrolEven = 0;
            int kontrolNonEven = 0;
            for (int i = 0; i < 12; i++)
            {
                int ii = Convert.ToInt32(code[i]);
                if (i+1 % 2 == 1)
                {
                    kontrolEven += ii;
                }
                else
                {
                    kontrolNonEven += ii;
                }
            }
            kontrolEven *= 3;
            kontrolPart = (kontrolEven + kontrolNonEven).ToString().Last().ToString();
            string structura = EAN13.GetStructur(Convert.ToInt32(kontrolPart));
            
            string leftCode = "";
            for(int i = 0; i < 6;i++)
            {
                if(structura[i]=='L')
                {
                    leftCode += EAN13.GetLeftL(leftCodeForm[i]);
                }
                else
                {
                    leftCode += EAN13.GetLeftG(leftCodeForm[i]);
                }
            }
            string rightCode = "";
            foreach(char c in rightCodeForm)
            {
                rightCode += EAN13.GetRightR(c);
            }
            TextBlock tbControl = new TextBlock()
            {
                FontSize=17,
                Text = kontrolPart+" ",
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            StackPanelShtrih.Children.Add(tbControl);
            StackPanelShtrih.Children.Add(GenerateShtrihPanel("101", 80));
            StackPanelShtrih.Children.Add(GenerateMiddleStackPanel(leftCode,leftCodeForm));
            StackPanelShtrih.Children.Add(GenerateShtrihPanel("01010", 80));
            StackPanelShtrih.Children.Add(GenerateMiddleStackPanel(rightCode,rightCodeForm));
            StackPanelShtrih.Children.Add(GenerateShtrihPanel("101", 80));
        }

        public StackPanel GenerateMiddleStackPanel(string code, string text)
        {
            StackPanel spLeft = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Height = 102
            };
            spLeft.Children.Add(GenerateShtrihPanel(code, 70));
            spLeft.Children.Add(new TextBlock()
            {
                FontSize = 17,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = text,
                VerticalAlignment = VerticalAlignment.Bottom
            });
            return spLeft;
        }

        public StackPanel GenerateShtrihPanel(string code, int height)
        {
            StackPanel spShtrih = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Height = height,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 10, 0, 0)
            };
            foreach(char c in code)
            {
                if(c=='1')
                {
                    spShtrih.Children.Add(CreateRectangle(Brushes.Black, height));
                }
                else
                {
                    spShtrih.Children.Add(CreateRectangle(Brushes.White, height));
                }
            }
            return spShtrih;
        }

        public Rectangle CreateRectangle(Brush color, int height)
        {
            return new Rectangle()
            {
                Stroke = color,
                StrokeThickness = 2,
                SnapsToDevicePixels = true,
                Height = height
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StackPanelShtrih.Children.Clear();
            GenerateCode();
        }
    }
}

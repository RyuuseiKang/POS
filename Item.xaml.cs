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

namespace POS
{
    /// <summary>
    /// Item.xaml の相互作用ロジック
    /// </summary>
    public partial class Item : UserControl
    {
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(Item),
                                    new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int Count {
            get { return (int)GetValue(CountProperty); }
            set { SetCount(ref value);
                  SetValue(CountProperty, value); }
        }

        public static readonly DependencyProperty ItemPriceProperty =
            DependencyProperty.Register("ItemPrice", typeof(int), typeof(Item),
                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int ItemPrice {
            get { return (int)GetValue(ItemPriceProperty); }
            set { SetValue(ItemPriceProperty, value); }
        }

        public static DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(Item));

        public string ItemName {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public static DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(Item));

        public string ImageUrl {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public static DependencyProperty TotalPriceProperty =
            DependencyProperty.Register("TotalPrice", typeof(int), typeof(Item));

        public int TotalPrice {
            get { return (int)GetValue(TotalPriceProperty); }
            set { SetValue(TotalPriceProperty, value); }
        }

        public Item()
        {
            InitializeComponent();
        }

        private void upRect_MouseUp(object sender, MouseButtonEventArgs e) {
            ++Count;

            count_TextBack.Text = (Count - 1).ToString();
            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("CountUp");
            BeginStoryboard(storyBoard);
            
        }

        private void downRect_MouseUp(object sender, MouseButtonEventArgs e) {
            bool isFlag = false;
            if (Count <= 0)
                isFlag = true;

            --Count;

            if (!isFlag) { 
            count_TextBack.Text = (Count + 1).ToString();
            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("CountDown");
            BeginStoryboard(storyBoard);
            }

        }

        private void SetCount(ref int _v) {
            if (_v < 0) {
                _v = 0;
            }

            TotalPrice = ItemPrice * _v;

            if (this.CountChanged != null)
                this.CountChanged(null, null);
        }

        public event EventHandler CountChanged;
        
        public int ClearAll()
        {
            int tmp = Count;
            if (tmp == 0) return 0;
            Count = 0;

            count_TextBack.Text = (tmp).ToString();
            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("CountDown");
            BeginStoryboard(storyBoard);

            return 0;
        }
    }
}

using System.IO;
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
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadReceipt();
        }

        struct receiptData {
            public int Id;
            public string Timestamp;
            public int Item0;
            public int Item1;
            public int Item2;
            public int PayType;
            public int CouponCount;
            public int Amount;
        }

        private void Init() {
            item0.Count = 0;
            item1.Count = 0;
            item2.Count = 0;
            SetCoupon(0, false);
            ChangeKakaku();
            SetKessai(0);
        }

        private string getNow()
        {
            int day, hour, minute, second;

            day = DateTime.Now.Day;
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;
            second = DateTime.Now.Second;


            string tmp = DateTime.Now.Month.ToString() + "/";

            if (day < 10)
                tmp += "0";
            tmp += day.ToString() + " ";

            if (hour < 10)
                tmp += "0";
            tmp += hour.ToString() + ":";

            if (minute < 10)
                tmp += "0";
            tmp += minute.ToString() + ":";

            if (second < 10)
                tmp += "0";
            tmp += second.ToString();

            return tmp;
        }

        List<receiptData> receipt = new List<receiptData>();

        private int IdCount = 0;
        private void ReadReceipt() {
            string csvPath = AppDomain.CurrentDomain.BaseDirectory;

            StreamReader sr = new StreamReader(csvPath + "receipt.csv");
            while (!sr.EndOfStream) {
                string s = sr.ReadLine();
                string[] temp = s.Split(',');

                receipt.Add(new receiptData {
                    Id = IdCount = Int32.Parse(temp[0]),
                    Timestamp = temp[1],
                    Item0 = Int32.Parse(temp[2]),
                    Item1 = Int32.Parse(temp[3]),
                    Item2 = Int32.Parse(temp[4]),
                    PayType = Int32.Parse(temp[5]),
                    CouponCount = Int32.Parse(temp[6]),
                    Amount = Int32.Parse(temp[7])
                });

            }


        }

        private void SaveReceipt() {
            string csvPath = AppDomain.CurrentDomain.BaseDirectory;


            string now = getNow();
            receipt.Add(new receiptData {
                Timestamp = now,
                Id = ++IdCount,
                Item0 = item0.ClearAll(),
                Item1 = item1.ClearAll(),
                Item2 = item2.ClearAll(),
                PayType = isKessai,
                CouponCount = coupon,
                Amount = sum
            });

            receipt.Sort((left, right) => left.Id - right.Id);

            var path = System.IO.Path.Combine(csvPath, "receipt.csv");
            using (var writer = new StreamWriter(path, false)) {
                foreach (var e in receipt) {
                    writer.WriteLine(string.Join(",",
                        e.Id, e.Timestamp, e.Item0, e.Item1, e.Item2, e.PayType, e.CouponCount, e.Amount));
                }
            }
        }

        private void kaikeiRect_MouseUp(object sender, MouseButtonEventArgs e) {
            if(sum != 0) {
                System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource("kaikei_MouseUp");
                BeginStoryboard(storyBoard);

                SaveReceipt();
                Init();
            }
                
        }

        int isKessai = 0;
        private void Window_Initialized(object sender, EventArgs e) {
            
        }

        private void kenkinRect_MouseUp(object sender, MouseButtonEventArgs e) {
            if(!(isKessai == 1))
                SetKessai(1);
        }

        private void soukinRect_MouseUp(object sender, MouseButtonEventArgs e) {
            if (!(isKessai == 2))
                SetKessai(2);
        }

        private void SetKessai(int _kessai) {
            string resourceName = "";
            switch (_kessai) {
                case 0:
                    resourceName = "allOff";
                    break;
                case 1:
                    resourceName = "kenkinOn";
                    break;
                case 2:
                    resourceName = "soukinOn";
                    break;
            }
            
            System.Windows.Media.Animation.Storyboard storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource(resourceName);
            BeginStoryboard(storyBoard);

            isKessai = _kessai;
        }

        private void priceRect_BackImage_MouseDown(object sender, MouseButtonEventArgs e) {
            try {
                DragMove();
            } catch(Exception eD) {

            }
        }

        private int sum = 0;
        private void item0_CountChanged(object sender, EventArgs e) {
            ChangeKakaku();
        }

        private void item1_CountChanged(object sender, EventArgs e) {
            ChangeKakaku();
        }

        private void item2_CountChanged(object sender, EventArgs e) {
            ChangeKakaku();
        }

        private void ChangeKakaku() {
            sum = item0.TotalPrice + item1.TotalPrice + item2.TotalPrice;
            kakakuText.Text = String.Format("{0:##,##0}", sum);
        }

        private void couponUpRect_MouseUp(object sender, MouseButtonEventArgs e) {
            SetCoupon(++coupon, true);
        }

        private void couponDownRect_MouseUp(object sender, MouseButtonEventArgs e) {
            SetCoupon(--coupon, false);
        }

        int coupon = 0;
        private void SetCoupon(int _c, bool isUp) {
            bool isZero = false;
            if (_c == 0)
                isZero = true;

            if (_c < 0) {
                _c = 0;
                coupon = 0;
                couponCountText.Text = coupon.ToString() + "개 적용됨";
                return;
            }
                
            coupon = _c;

            System.Windows.Media.Animation.Storyboard storyBoard;
            string resourceName = "";
            if (isUp) {
                resourceName = "CouponUp";
            } else {
                resourceName = "CouponDown";
            }
            
            if (isZero) {
                resourceName = "hideCouponCount";
            }

            storyBoard = (System.Windows.Media.Animation.Storyboard)FindResource(resourceName);
            BeginStoryboard(storyBoard);

            couponCountText.Text = coupon.ToString() + "개 적용됨"; 
        }
    }
}

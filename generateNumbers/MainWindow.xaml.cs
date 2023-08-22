using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace generateNumbers
{
    
    public partial class MainWindow : Window
    {
        private Thread primeGeneratorThread;
        private bool stopFlag;
        private Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stopFlag = false;
            int lowerBound = string.IsNullOrEmpty(Tbox2.Text) ? 2 : int.Parse(Tbox2.Text);
            int upperBound = string.IsNullOrEmpty(Tbox1.Text) ? int.MaxValue : int.Parse(Tbox1.Text);

            primeGeneratorThread = new Thread(() => GenerateRandom(lowerBound, upperBound));
            primeGeneratorThread.Start();
        }

        private  void  GenerateRandom(int lowerBound, int upperBound)
        {
            while (!stopFlag)
            {
                int randomNum = random.Next(lowerBound, upperBound + 1);

                if (IsPrime(randomNum))
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        ListNumbers.Items.Add(randomNum);
                    });

                }

                Thread.Sleep(1000); 
            }
        }
        private bool IsPrime(int num)
        {
            if (num <= 1)
                return false;

            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            stopFlag = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (primeGeneratorThread != null && primeGeneratorThread.IsAlive)
            {
                stopFlag = true;
                primeGeneratorThread.Join();
            }
            base.OnClosed(e);
        }
    }
}

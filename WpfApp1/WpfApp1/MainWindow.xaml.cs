using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<CountryInfo> lstCountry; //数据源
        private BindingList<CountryInfo> bindingList; //用于更新comboBox

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) //初始化国家数据，加载ComboBox
        {
            lstCountry = new List<CountryInfo>();
            // 用于绑定数据源
            //try
            //{
            //    StreamReader sr = new StreamReader("E:/C#/WpfApp1/WpfApp1/country.txt"); // 创建一个 StreamReader 的实例来读取文件 
            //    {
            //        string line;
            //        while ((line = sr.ReadLine()) != null) // 从文件读取一行，直到文件的末尾 
            //        {
            //            //中国 CN zh-CN
            //            lstCountry.Add(new CountryInfo { Country = line.Split()[0], Region = line.Split()[1], Culture = line.Split()[2] });
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    // 向用户显示出错消息
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(exc.Message);
            //}
            lstCountry.Add(new CountryInfo { Country = "中国", Region = "CN", Culture = "zh-CN" });
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            for (int i = 1; i < cultures.Length; i++)
            {
                if (cultures[i].Name.Split('-').Length == 1) //Length=1为语言名称
                {
                    continue;
                }
                try
                {
                    if (cultures[i].Name.Split('-').Length == 2)
                    {
                        //中国 CN zh-CN
                        RegionInfo region = new RegionInfo(cultures[i].Name);
                        lstCountry.Add(new CountryInfo { Country = region.DisplayName, Region = cultures[i].Name.Split('-')[1], Culture = cultures[i].Name });
                    }
                    else if (cultures[i].Name.Split('-').Length == 3)
                    {
                        RegionInfo region = new RegionInfo(cultures[i].Name);
                        lstCountry.Add(new CountryInfo { Country = region.DisplayName, Region = cultures[i].Name, Culture = cultures[i].Name });
                    }
                }
                catch (Exception exc)
                {
                    //向用户显示出错消息
                    Console.WriteLine(exc.Message);
                    continue;
                }
            }

            this.countryComboBox.ItemsSource = lstCountry;
            this.countryComboBox.SelectedIndex = 0; // 默认选中中国
        }

        private void Country_Changed(object sender, SelectionChangedEventArgs e)
        {
            CountryInfo selectedCountry = (CountryInfo)countryComboBox.Items[countryComboBox.SelectedIndex];
            //当前选中加1
            //CountryInfo selectedCountry;
            //if (countryComboBox.SelectedIndex + 1 == countryComboBox.Items.Count)
            //{
            //    selectedCountry = (CountryInfo)countryComboBox.Items[0];
            //}
            //else
            //{
            //    selectedCountry = (CountryInfo)countryComboBox.Items[countryComboBox.SelectedIndex + 1];
            //}
            string country = selectedCountry.Country; //国家
            string region = selectedCountry.Region; //区域
            string culture = selectedCountry.Culture; //文化

            //CultureInfo类基于 RFC 4646 为每个区域性指定唯一名称 例如：中国 zh-CN
            CultureInfo myCultureInfo = new CultureInfo(culture);
            //与 CultureInfo 类不同， RegionInfo 类不表示用户首选项，且不依赖于用户的语言或区域性。
            //RegionInfo 是在 ISO 3166 中为国家 / 地区定义的由两个字母组成的代码之一 例如：中国CN
            RegionInfo myRegionInfo = new RegionInfo(region);

            this.countryGroupBox.Header = country + "国家和文化信息";
            this.Info1.Text = "国家全名：" + myRegionInfo.DisplayName;
            this.Info2.Text = "国家英文名：" + myRegionInfo.EnglishName;
            this.Info3.Text = "货币符号：" + myRegionInfo.CurrencySymbol;
            this.Info4.Text =  "是否使用公制：" + (myRegionInfo.IsMetric ? "是" : "否").ToString();
            this.Info5.Text = "三字地区码：" + myRegionInfo.ThreeLetterISORegionName;
            this.Info6.Text = "语言名称：" + myCultureInfo.DisplayName;
        }

        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            CountryInfo selectedCountry = (CountryInfo)countryComboBox.Items[countryComboBox.SelectedIndex];
            string country = selectedCountry.Country; //国家
            using (StreamWriter sw = new StreamWriter("E:/C#/WpfApp1/WpfApp1/Info.txt", true)) //以追加的方式写入文件
            {
                sw.WriteLine(country + "国家和文化信息");
                sw.WriteLine(this.Info1.Text);
                sw.WriteLine(this.Info2.Text);
                sw.WriteLine(this.Info3.Text);
                sw.WriteLine(this.Info4.Text);
                sw.WriteLine(this.Info5.Text);
                sw.WriteLine(this.Info6.Text);
                sw.WriteLine();
            }
            MessageBox.Show(country + "国家和文化信息保存成功");
        }
        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            
            string str = Interaction.InputBox("请输入需要添加的国家信息，输入格式：中国 CN zh-CN", "添加国家", "", -1, -1);
            if (str.Split().Length != 3)
            {
                MessageBox.Show("输入格式有误！");
                return;
            }
            else if (str.Split().Length == 3)
            {
                try
                {
                    Console.WriteLine(new RegionInfo(str.Split()[1]));
                    Console.WriteLine(new CultureInfo(str.Split()[2]));
                }
                catch (Exception)
                {
                    MessageBox.Show("输入内容有误！");
                    return;
                }
            }
            lstCountry.Insert(0, new CountryInfo { Country = str.Split()[0], Region = str.Split()[1], Culture = str.Split()[2] });
            bindingList = new BindingList<CountryInfo>(lstCountry);
            this.countryComboBox.ItemsSource = bindingList;
            MessageBox.Show("😉国家添加成功！");
        }
    }
    public class CountryInfo

    {
        private string country; //国家名称
        private string region; //地区
        private string culture; //文化

        public string Country
        { 
            get { return country; }
            set { country = value; }
        }
        public string Region
        {
            get { return region; }
            set { region = value; }
        }
        public string Culture
        {
            get { return culture; }
            set { culture = value; }
        }
    }
}

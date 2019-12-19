using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MarketKasaOtomasyon
{
    class Program
    {
        static void Main(string[] args)
        {
            Market market = new Market();
            string productName; //ÜRÜN İSMİ
            //double productPrice; //ÜRÜN FİYATI
            int productCode; //ÜRÜN KODU
            int selectNumber; //İŞLEM YAPILACAK SAYI
            ConsoleSetting(); //KONSOL AYARLARINI YAPAN FONKSİYONU ÇAĞIRIYOR.
        mainstart:
            Console.WriteLine("\n\t\t------------------ İşlem yapmak istediğiniz numarayı giriniz! ------------------");
            Console.Write("\n\n\t\t\t\t\t1 - OKUT VE HESAPLA" +
                              "\n\n\t\t\t\t\t2 - ÜRÜN EKLE" +
                              "\n\n\t\t\t\t\t3 - ÜRÜN SİL" +
                              "\n\n\t\t\t\t\t4 - ÜRÜN BİLGİSİ" +
                              "\n\n\t\t\t\t\t5 - Çıkış" +
                              "\n\nSeçim: ");

            selectNumber = Int32.Parse(Console.ReadLine()); //İŞLEM YAPILACAK SAYIYI ALIYOR.

            isDirectory(); //KLASÖR KONTROLÜ

            switch (selectNumber)
            {
                case 1:
                    double totalPrice = 0;
                    TotalPriceCalculator(ref totalPrice);
                    goto mainstart;
                case 2:
                producterror:
                    Console.Write("Ürün ismi giriniz: ");
                    productName = Console.ReadLine(); //ÜRÜN İSMİNİ ALIYOR.
                    Console.Write("(Ondalıklı sayı için virgül ',' kullanınız!) Fiyat giriniz: ");
                    market.ProductPrice = Math.Abs(Double.Parse(Console.ReadLine())); //ÜRÜN FİYATINI ALIYOR. (MUTLAK DEĞER İÇİNDE)
                    Console.Write("(En fazla 5 haneli) Ürün kodu giriniz: ");
                    productCode = Int32.Parse(Console.ReadLine()); //ÜRÜN KODUNU ALIYOR.
                    if (productCode > 99999 || productCode < 1) //ÜRÜN KODU 1-99999 (1 ile 99999 dahil) ARASINDA OLMAZSA
                    {
                        Console.Clear();
                        Console.WriteLine("\nLütfen 1-99999 (1 ile 99999 dahil) arasında giriniz!");
                        goto producterror;
                    }
                    ProductAdd(productCode, productName, market.ProductPrice); //ÜRÜN EKLEME FONKSİYONUNU ÇAĞIRIYOR.
                    goto mainstart;
                case 3:
                producterror2:
                    Console.Write("(En fazla 5 haneli) Ürün kodu giriniz: ");
                    productCode = Int32.Parse(Console.ReadLine()); //ÜRÜN KODUNU ALIYOR.
                    if (productCode > 99999 || productCode < 1) //ÜRÜN KODU 1-99999 (1 ile 99999 dahil) ARASINDA OLMAZSA
                    {
                        Console.Clear();
                        Console.WriteLine("\nLütfen 1-99999 (1 ile 99999 dahil) arasında giriniz!");
                        goto producterror2;
                    }
                    ProductDelete(productCode); //ÜRÜN SİLME FONKSİYONUNU ÇAĞIRIYOR.
                    goto mainstart;
                case 4:
                producterror3:
                    Console.Write("(En fazla 5 haneli) Bilgi alınacak ürün kodunu giriniz: ");
                    productCode = Int32.Parse(Console.ReadLine()); //ÜRÜN KODUNU ALIYOR.
                    if (productCode > 99999 || productCode < 1) //ÜRÜN KODU 1-99999 (1 ile 99999 dahil) ARASINDA OLMAZSA
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nLütfen 1-99999 (1 ile 99999 dahil) arasında giriniz!");
                        Console.ForegroundColor = ConsoleColor.Black;
                        goto producterror3;
                    }
                    ProductInfo(productCode);
                    goto mainstart;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\t\t\t\t!!!!!!!!!!!!!!!!!! Hatalı giriş yaptınız !!!!!!!!!!!!!!!!!!");
                    Console.ForegroundColor = ConsoleColor.Black;
                    goto mainstart;
            }
        }

        static void TotalPriceCalculator(ref double totalPrice) //FİYAT HESAPLAYICI
        {
            Market market = new Market();
        newAdd:
            Console.WriteLine("Ürün Kodu Giriniz: ");
            int productCode = Int32.Parse(Console.ReadLine());//ÜRÜN KODU ALIYOR
            long barcode = market.BarcodeCreate(productCode); //BARKOD NUMARASI ALINIYOR
            bool isFile = false; //DOSYA VAR MI?
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler\" + barcode + ".txt"; //DOSYA YOLU
            string[] dir = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler"); //KLASÖRDE BULUNAN DOSYALARIN YOLUNU ALIYOR.
            foreach (var item in dir) //DOSYANIN OLUP OLMADIĞINI ARAŞTIRIYOR.
            {
                if (item == path)
                {
                choiceerror:
                    Console.Write("\t\t\t\t------ NE TÜR HESAP YAPACAKSINIZ ------\n\n" +
                                  "\t\t\t\t\t\t1 - KG\n\n" +
                                  "\t\t\t\t\t\t2 - ADET\n\n" +
                                  "Seçim: ");
                    int choice = Int32.Parse(Console.ReadLine()); //SEÇİLEN İŞLEMİN NUMARASINI ALIYOR
                    switch (choice)
                    {
                        case 1: //KG HESABI YAPAR
                            isFile = true;
                            double kg;
                            Console.WriteLine("Kaç 'Gram': ");
                            kg = Double.Parse(Console.ReadLine());
                            kg /= 1000;
                            totalPrice += (kg * (double.Parse(File.ReadAllLines(path)[1])));
                            Console.WriteLine("Ekleme yapıcak mısınız? (E/H)");
                            string add = Console.ReadLine().ToUpper();
                            if (add == "E") { goto newAdd; } //YENİ ÜRÜN OKUT.
                            else if (add == "H") //TOPLAM FİYATI YAZDIR.
                            {
                                Console.Clear();
                                Console.WriteLine("Fiyat: " + Math.Round(totalPrice, 2) + " TL");
                                totalPrice = 0;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                                Console.ReadLine();
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            continue;
                        case 2: //ADET HESABI YAPAR
                            isFile = true;
                            int pieces;
                            Console.WriteLine("Kaç adet: ");
                            pieces = Int32.Parse(Console.ReadLine());
                            totalPrice += (pieces * (double.Parse(File.ReadAllLines(path)[1])));
                            Console.WriteLine("Ekleme yapıcak mısınız? (E/H)");
                            string add2 = Console.ReadLine().ToUpper();
                            if (add2 == "E") { goto newAdd; } //YENİ ÜRÜN OKUT.
                            else if (add2 == "H") //TOPLAM FİYATI YAZDIR.
                            {
                                Console.Clear();
                                Console.WriteLine("Fiyat: " + Math.Round(totalPrice, 2) + " TL");
                                totalPrice = 0;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                                Console.ReadLine();
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            continue;
                        default:
                            Console.WriteLine("Hatalı seçim yaptınız!");
                            goto choiceerror;
                    }
                }
                if (!isFile) //DOSYA YOKSA
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n\t\t\t!!!!!!!!!!!!!!!!!! BÖYLE BİR ÜRÜN YOK !!!!!!!!!!!!!!!!!!");
                    Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                    Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                    isFile = true;
                    goto newAdd;
                }
            }
        }

        static void ProductInfo(int productCode) //ÜRÜN BİLGİLERİNİ VERİR.
        {
            Market market = new Market();
            bool isFile = false; //DOSYA VAR MI?
            string fileString; //DOSYANIN İÇİNDEKİ YAZI BU STRINGE AKTARILACAK.
            long barcode = market.BarcodeCreate(productCode); //BARKOD NUMARASI ALINIYOR.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler\" + barcode + ".txt"; //DOSYA YOLU
            string[] dir = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler"); //KLASÖRDE BULUNAN DOSYALARIN YOLUNU ALIYOR.
            foreach (var item in dir) //DOSYANIN OLUP OLMADIĞINI ARAŞTIRIYOR.
            {
                if (item == path)
                {
                    fileString = File.ReadAllText(path).ToUpper(); //DOSYANIN İÇİNDEKİ YAZILAR STRINGE AKTARILIYOR
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n" + fileString + " TL");
                    Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                    Console.ReadLine();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Black;
                    isFile = true;
                }
            }
            if (!isFile) //DOSYA YOKSA
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\t\t\t!!!!!!!!!!!!!!!!!! BÖYLE BİR ÜRÜN YOK !!!!!!!!!!!!!!!!!!");
                Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                Console.ReadLine();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }

        static void ProductAdd(int productCode, string productName, double productPrice) //ÜRÜN EKLEYİCİ
        {
            Market market = new Market();
            bool isFile = false; //DOSYA VAR MI?
            long barcode = market.BarcodeCreate(productCode); //BARKOD NUMARASI ALINIYOR.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler\" + barcode + ".txt"; //DOSYA YOLU
            string[] dir = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler"); //KLASÖRDE BULUNAN DOSYALARIN YOLUNU ALIYOR.
            foreach (var item in dir) //DOSYANIN OLUP OLMADIĞINI ARAŞTIRIYOR.
            {
                if (item == path) //DOSYA YOLU İLE ARANAN DOSYA YOLUNUN İSMİ AYNI İSE
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n\t\t\t!!!!!!!!!!!!!!!!!! BU ÜRÜN KODUNDA BİR ÜRÜN BULUNMAKTADIR !!!!!!!!!!!!!!!!!!");
                    Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                    Console.ReadLine();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Black;
                    isFile = true;
                }
            }
            if (!isFile) //DOSYA YOKSA
            {
                File.Create(path).Close(); //BARKOD NUMARALI DOSYA OLUŞTURULUYOR.
                File.WriteAllText(path, productName.ToUpper() + "\n" + productPrice); //DOSYANIN İÇİNE ÜRÜNÜN İSMİNİ YAZIYOR.
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\t\t\t\t!!!!!!!!!!!!!!!!!! ÜRÜN EKLENDİ !!!!!!!!!!!!!!!!!!");
                Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                Console.ReadLine();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }

        static void ProductDelete(int productCode)
        {
            Market market = new Market();
            bool isFile = false; //DOSYA VAR MI?
            long barcode = market.BarcodeCreate(productCode); //BARKOD NUMARASI ALINIYOR.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler\" + barcode + ".txt"; //DOSYA YOLU
            string[] dir = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler"); //KLASÖRDE BULUNAN DOSYALARIN YOLUNU ALIYOR.
            foreach (var item in dir) //DOSYANIN OLUP OLMADIĞINI ARAŞTIRIYOR.
            {
                if (item == path) //DOSYA YOLU İLE ARANAN DOSYA YOLUNUN İSMİ AYNI İSE
                {
                    File.Delete(path); //BARKOD NUMARALI DOSYA SİLİNİYOR.
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n\t\t\t\t!!!!!!!!!!!!!!!!!! ÜRÜN SİLİNDİ !!!!!!!!!!!!!!!!!!");
                    Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                    Console.ReadLine();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Black;
                    isFile = true;
                }
            }
            if (!isFile) //DOSYA YOKSA
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\t\t\t!!!!!!!!!!!!!!!!!! BU ÜRÜN KODUNDA BİR ÜRÜN BULUNMAKTADIR !!!!!!!!!!!!!!!!!!");
                Console.WriteLine("\n\n\t\t\t!!!!!!!!!!!!!!!!!! DEVAM ETMEK İÇİN 'ENTER' TUŞUNA BASINIZ! !!!!!!!!!!!!!!!!!!");
                Console.ReadLine();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }

        static private void ConsoleSetting() //KONSOL AYARLARI YAPILIYOR.
        {
            Console.SetWindowSize(63 * 2, 63 / 2); //KONSOLUN BOYUTU
            Console.SetWindowPosition(0, 0); //KONSOLUN POZİSYONU
            Console.BackgroundColor = ConsoleColor.Cyan; //KONSOLUN ARKA RENGİ
            Console.Clear(); //ARKA RENGİ YAYMAK İÇİN KONSOL SİLİNİYOR.
            Console.ForegroundColor = ConsoleColor.Black; //KONSOL ÖN RENGİ
        }

        static private void isDirectory() //EĞER DOSYA YOKSA DOSYA OLUŞTURUR.
        {
            Market market = new Market();
            bool isDir = false; //DOSYA VAR MI?
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ürünler\"; //KLASÖR YOLU
            string[] dir = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)); //KLASÖRDE BULUNAN KLASÖRLERİN YOLUNU ALIYOR.
            foreach (var item in dir) //DOSYANIN OLUP OLMADIĞINI ARAŞTIRIYOR.
            {
                if (item == path) //KLASÖR YOLU İLE ARANAN KLASÖR YOLUNUN İSMİ AYNI İSE
                {
                    isDir = true;
                    break;
                }
            }
            if (!isDir) //DOSYA YOKSA
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path); //KLASÖR OLUŞTURULUYOR.
                }
            }
        }
    }

    class Market
    {
        //BARKOD OLUŞTURMA SİSTEMİ  -> ("ÜLKE VEYA SİMGE KODU")("FİRMA KODU")("ÜRÜN KODU")("KONTROL KODU")
        //BARKOD NUMARASI -> (869)(8686)(ÜÜÜÜÜ)(K) -> 869-TÜRKİYE KODU  8686-FİRMA KODU  ÜÜÜÜ-ÜRÜN KODU  K-KONTROL KODU
        public long BarcodeCreate(int productCode) //BARKOD OLUŞTURUCU
        {
            long barcodenumber = 0; //BARKOD NUMARASI (GERİ DÖNECEK)
            int oddNumbersSum = 22; //KONTROL KODU İÇİN TEK SAYILARIN TOPLAMI
            int evenNumbersSum = 29; //KONTROL KODU İÇİN ÇİFT SAYILARIN TOPLAMI
            int flagproductcode = productCode; //ÜRÜN KODUNU KAYBETMEMEK İÇİN YEDEKLEME
            int controlCode; //KONTROL KODU

            for (int i = 0; flagproductcode > 0; i++) //KONTROL KODU OLUŞTURULUYOR.
            {
                if (i % 2 == 0) //TEK SAYILAR TOPLANIYOR.
                {
                    oddNumbersSum += flagproductcode % 10;
                }
                else if (i % 2 != 0) //ÇİFT SAYILAR TOPLANIYOR.
                {
                    evenNumbersSum += flagproductcode % 10;
                }
                flagproductcode /= 10;
            }

            //KONTROL KODU HESAPLAMA -> (((((TekSayılarToplamı * 3) + ÇiftSayılarToplamı) / 10) + 1) * 10) - ((TekSayılarToplamı * 3) + ÇiftSayılarToplamı)
            controlCode = (((((oddNumbersSum * 3) + evenNumbersSum) / 10) + 1) * 10) - ((oddNumbersSum * 3) + evenNumbersSum);

            //10000
            flagproductcode = productCode;
            for (int i = 0; flagproductcode > 0; i++) //ÜRÜN KODU YERLEŞTİRİLİYOR.
            {
                if (i == 0) { barcodenumber = 8698686000000 + ((flagproductcode % 10) * 10); } //ÜRÜN KODUNUN BİRLER BASAMAĞI
                else if (i == 1) { barcodenumber += ((flagproductcode % 10) * 100); } //ÜRÜN KODUNUN ONLAR BASAMAĞI
                else if (i == 2) { barcodenumber += ((flagproductcode % 10) * 1000); } //ÜRÜN KODUNUN YÜZLER BASAMAĞI
                else if (i == 3) { barcodenumber += ((flagproductcode % 10) * 10000); } //ÜRÜN KODUNUN BİNLER BASAMAĞI
                else if (i == 4) { barcodenumber += ((flagproductcode % 10) * 100000); } //ÜRÜN KODUNUN ON BİNLER BASAMAĞI
                flagproductcode /= 10; //BİR SONRAKİ BAMAK İÇİN 10'A BÖLÜNÜYOR.
            }

            barcodenumber += controlCode; //KONTROL KODU YERLEŞTİRİLİYOR.

            return barcodenumber; //OLUŞTURULAN BARKOD KODU GERİ DÖNDÜRÜLÜYOR.
        }

        private double productPrice = 0;

        public double ProductPrice
        {
            get
            {
                return productPrice;
            }
            set
            {
                productPrice = Math.Abs(value);
            }
        }
    }
}

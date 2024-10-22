using IronBarCode;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //GeneratedBarcode barcode = BarcodeWriter.CreateBarcode(Encoding.UTF8.GetBytes("Đây là tiếng việt. A Á ắ ấ b đ ê ế"), BarcodeWriterEncoding.QRCode);
            //barcode.SaveAsPng("barcode.png");

            Console.OutputEncoding = Encoding.UTF8; 
            var x = (new Test()
            {
                Status = Status.Undone
            });

            Console.WriteLine(x.Status);
            Console.WriteLine(x.Status.GetAttribute<DisplayAttribute>().Name);

            x.Status = 0;
            Console.WriteLine(x.Status);

            Console.WriteLine(x.Status.GetAttribute<DisplayAttribute>().Name); 
            Console.ReadLine();
        }
    }

    public class Test
    {
        public Status Status { get; set; }
    }
    public enum Status
    {
        [Display(Name = "Đã xong")]
        Done = 0,
        [Display(Name = "Chưa xong")]
        Undone = 1
    }

    public static class Extension
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}

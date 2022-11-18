using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using WebRazor.Models;

namespace WebRazor.Helpers
{
    public class PDFHelper
    {
        public static PdfDocument GenPDFInvoice(string name, string address, Dictionary<Product, OrderDetail> listProducts)
        {
            var html = $@"<h1 style='text-align: center;color: tomato;'>Customer Infors</h1><br>
                       <table style='margin: 0 auto;color: rgb(49, 51, 141);font-size: 20px;'>
                        <tr>
                            <td>Customer name:</td>
                            <td>{name}</td>
                        </tr>
                        <tr><td>Address:</td><td>{address}</td></tr>";
            decimal totalPrice = 0;
            foreach (var product in listProducts)
            {
                html += $@"<tr><td>----------------</td><td>
                            <tr><td>Product name:</td><td>{product.Key.ProductName}</td></tr>
                            <tr><td>Quantity:</td><td>{product.Value.Quantity}</td></tr>
                            <tr><td>Unit price:</td><td>{product.Value.UnitPrice}</td></tr>
                            <tr><td>Price:</td><td>{product.Value.UnitPrice * product.Value.Quantity}</td></tr>";
                            totalPrice += product.Value.UnitPrice * product.Value.Quantity;
            }
                html += $@"<tr><td><h2>Total price:</h2></td><td><h2>{totalPrice}</h2></td></tr>
                        </table>";

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(html);
            return doc;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using WebRazor.Models;

namespace WebRazor.Helpers
{
    public class PDFHelper
    {
        public static PdfDocument GenPDFInvoice(string name, string address, Dictionary<Product, OrderDetail> listProducts)
        {
            var html = $@"<h1>Customer Infors:</h1><br>
                        Customer name: {name} <br>
                        Address: {address}<br>
                        <h1>List Products: </h1>";
            decimal totalPrice = 0;
            foreach (var product in listProducts)
            {
                html += $@"Product name: {product.Key.ProductName}<br>
                           Quantity: {product.Value.Quantity}<br>
                           Unit price: {product.Value.UnitPrice}<br>
                           Price: {product.Value.UnitPrice * product.Value.Quantity}<br><br>";
                totalPrice += product.Value.UnitPrice * product.Value.Quantity;
            }
            html += $@"<h2>Total price: {totalPrice}</h2>";

            HtmlToPdf converter = new HtmlToPdf();

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(html);
            return doc;
        }
    }
}

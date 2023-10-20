using DotNetTechWebFormProject.Model;
using DotNetTechWebFormProject.Utils;
using iText.Layout.Borders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DotNetTechWebFormProject.Pages
{
    public class OrderViewModel : PageModel
    {
        readonly IConfiguration _configuration;
        private readonly IVnPayController _vnPayController;
        private string _orderId;
        private decimal _totalAmount = 2000000; // The safe limit to perform test on VNPay
        private Order _curOrder = new Order();
        public List<OrderDetail> orderDetailParameterList = new List<OrderDetail>();
        public string connectionString;

        public OrderViewModel(IConfiguration configuration, IVnPayController vnPayController)
        {
            _configuration = configuration;
            _vnPayController = vnPayController;
        }

        public string GetOrderId()
        {
            return _orderId;
        }

        public Order GetOrder()
        {
            return _curOrder;
        }

        public void OnGetOrderId(string orderid)
        {
            _orderId = orderid;
            OnGet();
        }

        public ActionResult OnGetPrintReport(string orderid) 
        {
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            _curOrder = order.GetOrder(connectionString, orderid);
            orderDetailParameterList = orderDetail.GetOrderParametersById(connectionString, orderid);

            PDFGenerator generator = new PDFGenerator();
            return generator.CreatePDF(_curOrder, orderDetailParameterList);
        }

        public void OnGet()
        {
            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            _curOrder = order.GetOrder(connectionString, _orderId);
            orderDetailParameterList = orderDetail.GetOrderParametersById(connectionString, _orderId);
        }

        public IActionResult OnGetCreatePayment(string orderid)
        {
            Payment payment = new Payment(orderid, _totalAmount);
            string url = _vnPayController.CreatePaymentUrl(payment, HttpContext);

            return Redirect(url);
        }

        public IActionResult OnGetPaymentCallback()
        {
            PaymentResponse response = _vnPayController.PaymentExecute(Request.Query);

            return RedirectToPage("/Auth/PaymentResponse", new { message = response.TransactionId });
        }
    }
}

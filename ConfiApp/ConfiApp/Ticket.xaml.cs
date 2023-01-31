
using Android;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using ConfiApp.Modelos;
using Newtonsoft.Json;
using PdfSharp.Xamarin.Forms;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Syncfusion.Drawing.Color;
using PointF = Syncfusion.Drawing.PointF;

namespace ConfiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ticket : ContentPage
    {
        public class Detalle
        {
            public int idTicket { get; set; }
            public int idPago { get; set; }
            public string Concepto { get; set; }
            public string Monto { get; set; }
        }

        public class TicketI
        {
            public int id { get; set; }
            public string Total { get; set; }
            public string Subtotal { get; set; }
            public string Descuento { get; set; }
            public string Tasa16 { get; set; }
            public string iva { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
            public int idCredito { get; set; }
            public string Nombre { get; set; }
            public string NombreUsuario { get; set; }
            public int Caja { get; set; }
            public string Concepto { get; set; }
            public string Estado { get; set; }
            public List<Detalle> Detalle { get; set; }
        }

        public List<TicketI> Resultado;
        public ObservableCollection<TicketI> OTicket { get; set; }
        public int _idTicket;
        public Ticket(int idTicket)
        {
            InitializeComponent();
            _idTicket = idTicket;
           // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            GetTicketsApi();
        }

       
        public async void GetTicketsApi()
        {

           
            //UserDialogs.Instance.ShowLoading("Consultando");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiTicketImpresion?id=" + _idTicket.ToString());
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var MGestors = JsonConvert.DeserializeObject<ObservableCollection<TicketI>>(content);
                Resultado = JsonConvert.DeserializeObject<List<TicketI>>(content);
               OTicket = MGestors;
                BindableLayout.SetItemsSource(stackTicket, OTicket);
                // await DisplayAlert("Mensaje", MGestor[0].Multas, "Ok");
               

            }
            //    UserDialogs.Instance.HideLoading();

          //  BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
           // await DisplayAlert("Mensaje", OTicket[0].idCredito.ToString(), "Ok");

        }

        [Obsolete]
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {

            //var pdf = PDFManager.GeneratePDFFromView(Content); // aqui le paso la vista que quiero que vuelva pdf

            //var basepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var pdfpath = System.IO.Path.Combine(basepath,ToString(), "/mypdf.pdf");
            //var at = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/mypdf.pdf";
            //try
            //{
            //    pdf.Save(at);
            //    await Share.RequestAsync(new ShareFileRequest(new ShareFile(at)));
            //    await DisplayAlert("Mensaje", at, "Ok");

            //}
            //catch (Exception ex)
            //{
            //  await DisplayAlert("Mensaje", ex.Message, "Ok");
            //}


            //MemoryStream stream = new MemoryStream();

            //Create a new PDF document
            //using (PdfDocument document = new PdfDocument())
            //{
            //    //Add page to the PDF document.
            //    PdfPage page = document.Pages.Add();

            //    //Create graphics instance.
            //    PdfGraphics graphics = page.Graphics;

            //    Stream imageStream = null;


            //        imageStream = new MemoryStream(DependencyService.Get<ISave>().CaptureAsync().Result);


            //    //Load the image
            //    PdfBitmap image = new PdfBitmap(imageStream);

            //    //Draw the image
            //    graphics.DrawImage(image, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);

            //    //Save the document into memory stream
            //    document.Save(stream);

            //}

            //stream.Position = 0;
            double heightGrid;
            heightGrid = 0;
            PdfDocument document = new PdfDocument();

            //Add a page to the document.

            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page.

            PdfGraphics graphics = page.Graphics;

            //Set the standard font.

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            Stream imageStream = GetEmbeddedResourceStream("LOGO_2_X_1.png");
            PdfBitmap image = new PdfBitmap(imageStream);
            
            graphics.DrawImage(image, new PointF(((page.GetClientSize().Width / 2)- (image.Width /2)) + 55, 0));
            //Draw the text.
            PdfStringFormat format = new PdfStringFormat();
            PdfStringFormat formatNombre = new PdfStringFormat();
            PdfStringFormat FormatCell = new PdfStringFormat();
            
            //Set the text alignment.
            format.Alignment = PdfTextAlignment.Center;
            formatNombre.WordWrap = PdfWordWrapType.Word;
            string Empresa = "Préstamos Confía S.A. de C.V.";
            SizeF sizeEmpresa = font.MeasureString(Empresa);

            string dirEmpresa = "PCO150525PJ8";
            SizeF sizeDirEmpresa = font.MeasureString(dirEmpresa);

            string lTicket = "Ticket:";
            SizeF sizeLTicket = font.MeasureString(lTicket);

            string lIdTicket = OTicket[0].id.ToString();
            SizeF sizeLIdTicket = font.MeasureString(lIdTicket);

            string lCaja = "Caja:";
            SizeF sizeLCaja = font.MeasureString(lCaja);

            string lIdCaja = OTicket[0].Caja.ToString(); ;
            SizeF sizeLIdCaja = font.MeasureString(lIdCaja);

            string lUsuario ="Atendido por:" ;
            SizeF sizeLUsuario = font.MeasureString(lUsuario);

            

            string lNombreUsuario = OTicket[0].NombreUsuario.ToString();
            SizeF sizeLNombreUsuario = font.MeasureString(lNombreUsuario);


            PdfTextElement elementNombreUsuario = new PdfTextElement(lNombreUsuario, font);
            PdfStringFormat formatw = new PdfStringFormat();


            formatw.WordWrap = PdfWordWrapType.Word;
            elementNombreUsuario.StringFormat = formatw;


            string lCredito = "Crédito no.:";
            SizeF sizeLCredito = font.MeasureString(lCredito);

            string lIdCredito = OTicket[0].idCredito.ToString();
            SizeF sizeLIdCredito = font.MeasureString(lIdCredito);


            string lCliente = "Cliente:";
            SizeF sizeLCliente = font.MeasureString(lCliente);

            string lNombreCliente = OTicket[0].Nombre.ToString();
            SizeF sizeLNombreCliente = font.MeasureString(lNombreCliente);

            PdfTextElement elementNombreCliente = new PdfTextElement(lNombreCliente, font);
            elementNombreCliente.StringFormat = formatw;

            string lFecha = "Fecha:";
            SizeF sizeLFecha = font.MeasureString(lFecha);

            string lFechaHora = OTicket[0].Fecha.ToString() + " - " + OTicket[0].Hora.ToString();
            SizeF sizeLFechaHora = font.MeasureString(lFechaHora);


            string lSubTotal16 = "SubTotal Tasa 16%:";
            SizeF sizeLSubtotal16 = font.MeasureString(lSubTotal16);

            string lValorSubTotal16 = OTicket[0].Tasa16.ToString();
            SizeF sizeLValorSubTotal16 = font.MeasureString(lValorSubTotal16);


            string lIVA = "I.V.A.:";
            SizeF sizeLIVA = font.MeasureString(lIVA);

            string lValorIVA = OTicket[0].iva.ToString();
            SizeF sizeLValorIVA = font.MeasureString(lValorIVA);

            string lSubtotal = "Subtotal:";
            SizeF sizeLSubtotal = font.MeasureString(lSubtotal);

            string lValorSubtotal = OTicket[0].Subtotal.ToString();
            SizeF sizeLValorSubtotal = font.MeasureString(lValorSubtotal);


         
 
            string lDescuento = "Descuento:";
            SizeF sizeLDescuento = font.MeasureString(lDescuento);

            string lValorDescuento = OTicket[0].Descuento.ToString();
            SizeF sizeLValorDescuento = font.MeasureString(lValorDescuento);


            string lTotal = "Total:";
            SizeF sizeLTotal = font.MeasureString(lTotal);

            string lValorTotal = OTicket[0].Total.ToString();
            SizeF sizeLValorTotal = font.MeasureString(lValorTotal);

            PdfGrid pdfGrid = new PdfGrid();
           


            DataTable dataDetalle;
            dataDetalle = new DataTable();

            dataDetalle.Columns.Add("Descripción");
            dataDetalle.Columns.Add("Monto");

            foreach(var ass in OTicket[0].Detalle)
            {
                dataDetalle.Rows.Add(new object[] { ass.Concepto, ass.Monto });
            }

            pdfGrid.DataSource = dataDetalle;


            pdfGrid.Columns[0].Width = 400;
            pdfGrid.Columns[1].Width = 100;

            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.Bottom.Color = new PdfColor(Color.Transparent);
            cellStyle.Borders.Top.Color = new PdfColor(Color.Transparent);
            cellStyle.Borders.Left.Color = new PdfColor(Color.Transparent);

            cellStyle.Borders.Right.Color = new PdfColor(Color.Transparent);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            // cellStyle.StringFormat = format;
            for (int i = 0; i < pdfGrid.Rows.Count; i++)
            {
                for (int j = 0; j < pdfGrid.Rows[i].Cells.Count; j++)
                {
                    //Apply style
                    pdfGrid.Rows[i].Cells[j].Style = cellStyle;
                }
            }

            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.Left.Color = new  PdfColor(Color.Transparent);
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            headerStyle.Borders.Right.Color = new PdfColor(Color.Transparent);
            PdfGridRow header1 = pdfGrid.Headers[0];
          //  PdfGridRow header2 = pdfGrid.Headers[1];

            header1.ApplyStyle(headerStyle);
            double widthGrid;
            widthGrid = 0;
           
            foreach(var row in pdfGrid.Rows)
            {
                heightGrid = heightGrid + row.Height;
            }

            heightGrid = heightGrid + pdfGrid.Headers[0].Height + 20;
            RectangleF recUsuario;
            RectangleF recCliente;
            PdfPen pen = new PdfPen(PdfBrushes.Black, 1f);
          

            // header2.ApplyStyle(headerStyle);
            graphics.DrawString(Empresa, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2)-(sizeEmpresa.Width/2), image.Height + 10, sizeEmpresa.Width,sizeEmpresa.Height), format);
            graphics.DrawString(dirEmpresa, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2)-(sizeDirEmpresa.Width/2), image.Height + sizeEmpresa.Height  + 10, sizeDirEmpresa.Width, sizeDirEmpresa.Height), format);
            graphics.DrawString(lTicket, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + 10) + 10, sizeLTicket.Width, sizeLTicket.Height), format);
            graphics.DrawString(lIdTicket, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2)-50, ( image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + 10) + 10, sizeLIdTicket.Width, sizeLIdTicket.Height), format);
            graphics.DrawString(lCaja, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + 10) + 10, sizeLCaja.Width, sizeLCaja.Height), format);
            graphics.DrawString(lIdCaja, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + 10) + 10, sizeLIdCaja.Width, sizeLIdCaja.Height), format);

            graphics.DrawString(lUsuario, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + 10) + 10, sizeLUsuario.Width, sizeLUsuario.Height), format);
            //graphics.DrawString(lNombreUsuario, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2) - 50, (sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + 10) + 10, sizeLNombreUsuario.Width, sizeLNombreUsuario.Height), formatNombre);
            //elementNombreUsuario.Draw(page, new PointF((page.GetClientSize().Width / 2) - 50, (sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + 10) + 10));
          
            if(sizeLNombreUsuario.Width > (page.GetClientSize().Width / 2))
            {
                recUsuario = new RectangleF((page.GetClientSize().Width / 2) - 50, ( image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + 10) + 10, 300, 100);

            }
            else
            {
                recUsuario = new RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + 10) + 10, sizeLNombreUsuario.Width, sizeLNombreUsuario.Height);
            }
            
            var resultUsuario =  elementNombreUsuario.Draw(page, recUsuario);
            graphics.DrawString(lCredito, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + 10) + 10, sizeLCredito.Width, sizeLCredito.Height), format);
            graphics.DrawString(lIdCredito, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + 10) + 10, sizeLIdCredito.Width, sizeLIdCredito.Height), format);


            graphics.DrawString(lCliente, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + 10) + 10, sizeLCliente.Width, sizeLCliente.Height), format);
            //graphics.DrawString(lNombreCliente, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCliente.Height +10) + 10, sizeLNombreCliente.Width, sizeLNombreCliente.Height), formatNombre);

            if (sizeLNombreCliente.Width > (page.GetClientSize().Width / 2))
            {
                recCliente = new  RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCliente.Height + 10) + 10, 300, 100);

            }
            else
            {
                recCliente = new RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCliente.Height + 10) + 10, sizeLNombreCliente.Width, sizeLNombreCliente.Height);
            }

            var result =  elementNombreCliente.Draw(page, recCliente);


            graphics.DrawString(lFecha, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + 10) + 10, sizeLFecha.Width, sizeLFecha.Height), format);
            graphics.DrawString(lFechaHora, font, PdfBrushes.Black, new RectangleF((page.GetClientSize().Width / 2) - 50, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + 10) + 10, sizeLFechaHora.Width, sizeLFechaHora.Height), format);
            //Save the document into memory stream.



            pdfGrid.Draw(page, new PointF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + 10) + 30));

            PointF point1 = new PointF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + 10) + 10);
            PointF point2 = new PointF(500, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + 10) + 10);

            //Draw the line on PDF document
            page.Graphics.DrawLine(pen, point1, point2);
            graphics.DrawString(lSubTotal16, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + 10) + 10, sizeLSubtotal16.Width, sizeLSubtotal16.Height), format);
            graphics.DrawString(lValorSubTotal16, font, PdfBrushes.Black, new RectangleF(400, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + 10) + 10, sizeLValorSubTotal16.Width, sizeLValorSubTotal16.Height), format);


            graphics.DrawString(lIVA, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + 10) + 10, sizeLIVA.Width, sizeLIVA.Height), format);
            graphics.DrawString(lValorIVA, font, PdfBrushes.Black, new RectangleF(400, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + 10) + 10, sizeLValorIVA.Width, sizeLValorIVA.Height), format);

            

            graphics.DrawString(lSubtotal, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + 10) + 10, sizeLSubtotal.Width, sizeLSubtotal.Height), format);
            graphics.DrawString(lValorSubtotal, font, PdfBrushes.Black, new RectangleF(400, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + 10) + 10, sizeLValorSubtotal.Width, sizeLValorSubtotal.Height), format);
            PointF pointIva1 = new PointF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + 10) + 10);
            PointF pointIva2 = new PointF(500, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + 10) + 10);
            page.Graphics.DrawLine(pen, pointIva1, pointIva2);

            graphics.DrawString(lDescuento, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + sizeLSubtotal.Height + 10) + 10, sizeLDescuento.Width, sizeLDescuento.Height), format);
            graphics.DrawString(lValorDescuento, font, PdfBrushes.Black, new RectangleF(400, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + sizeLSubtotal.Height + 10) + 10, sizeLValorDescuento.Width, sizeLValorDescuento.Height), format);


            graphics.DrawString(lTotal, font, PdfBrushes.Black, new RectangleF(0, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + sizeLSubtotal.Height + sizeLDescuento.Height + 10) + 10, sizeLTotal.Width, sizeLTotal.Height), format);
            graphics.DrawString(lValorTotal, font, PdfBrushes.Black, new RectangleF(400, (image.Height + sizeDirEmpresa.Height + sizeEmpresa.Height + sizeLTicket.Height + sizeLCaja.Height + resultUsuario.Bounds.Height + sizeLCredito.Height + result.Bounds.Height + sizeLFecha.Height + Convert.ToInt32(heightGrid) + sizeLSubtotal16.Height + sizeLIVA.Height + sizeLSubtotal.Height + sizeLValorTotal.Height + 10) + 10, sizeLValorTotal.Width, sizeLValorTotal.Height), format);
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Close the document.

            document.Close(true);

            Xamarin.Forms.DependencyService.Get<ISave>().Save(Id.ToString() + lNombreCliente + ".pdf", "application/pdf", stream);
           
            await Share.RequestAsync(new ShareFileRequest(new ShareFile(App.Current.Properties["RutaArchivo"].ToString())));

            //    pdf.Save(at);
            //  DependencyService.Get<IFileIO>().Save(pdf, "pdfName.pdf")
            // try
            //  {

            //     await Share.RequestAsync(new ShareFileRequest(new ShareFile(at)));
            //  }
            //  catch
            //  {

            //  }
        }
        public static Stream GetEmbeddedResourceStream(string resourceFileName)
        {
            var imagePath = $"ConfiApp.Assets.Images.{resourceFileName}";

            return typeof(Ticket).GetTypeInfo().Assembly.GetManifestResourceStream(imagePath);
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }


            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }
            return tb;
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }



    }
}
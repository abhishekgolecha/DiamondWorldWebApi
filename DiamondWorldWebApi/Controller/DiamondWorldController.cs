using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace DiamondWorldWebApi.Controller
{
    public class DiamondWorldController : ApiController
    {

        //public string Get()
        //{
        //    return "Hello World";
        //}
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
        //        public HttpResponseMessage GetFile(string id)
        //{
        //    if (String.IsNullOrEmpty(id))
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);

        //    string fileName;
        //    string localFilePath;
        //    int fileSize;

        //    localFilePath = getFileFromID(id, out fileName, out fileSize);

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    response.Content.Headers.ContentDisposition.FileName = fileName;
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        //    return response;
        //}

        [HttpGet]
        [Route("api/myfunc/")]
        public string Get()
        {
            return "Hello World";
        }
        [HttpGet]
        [Route("DownloadPdfFile/{id}/{Wt:decimal}")]
        public HttpResponseMessage DownloadPdfFile(string id,decimal wt)
        {
            HttpResponseMessage result = null;
            try
            {

                string strCertno = Convert.ToString(id);// "7348814041";
                string strWt =Convert.ToString( wt);// "1.03";
               

                    byte[] bytes = GetGIACertificatePicureFromGIASite(strCertno, strWt);

                if (bytes !=null && bytes.Length > 0)
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, "application/pdf");
                    result.Content = new ByteArrayContent(bytes);
                    //result.ContentType = "application/pdf";
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = strCertno + ".pdf";

                    //string message = string.Format("Successfully download!");
                    //return Request.CreateResponse(HttpStatusCode.OK, message);

                    //return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    //var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("Invalid input = ReportNo {0} or  Weight {1}", strCertno, strWt)),ReasonPhrase = "ReportNo Not Found"
                    //};
                    //throw new HttpResponseException(resp);

                    string message = string.Format("Invalid input = ReportNo {0} or  Weight {1}", strCertno, strWt);
                   return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    //shubham
                }


                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.Gone);
            }
        }
        GIANewService.ReportCheckWSClient objGIANewService = new GIANewService.ReportCheckWSClient();
        public byte[] GetGIACertificatePicureFromGIASite(string strCurCertNo, string strCurWeight)
        {
            byte[] pdfbyte = null;
            //Image objImage = null;

            try
            {
                string strJIAIPAddress = string.Empty;
                // DataSet  dsReturn.Clear();
                //dsReturn.Tables.Clear();

                DataSet dsReturn = new DataSet();
                if (strJIAIPAddress == "101.95.24.158")
                {
                    strJIAIPAddress = "180.169.112.238"; //HOngkong
                }
                else
                {
                    strJIAIPAddress = "101.95.24.158";//Shanghai

                }
                //strCurWeight = "1.03";
                //strCurWeight = "00.00" ;

                //string strRequest = "<?xml version=\"1.0\" encoding=\"UTF8\"?><REPORT_CHECK_REQUEST><HEADER><IP_ADDRESS>" + strJIAIPAddress + "</IP_ADDRESS></HEADER><BODY><REPORT_DTLS><REPORT_DTL><REPORT_NO>" + strCurCertNo + "</REPORT_NO><REPORT_WEIGHT>" + strCurWeight + "</REPORT_WEIGHT></REPORT_DTL></REPORT_DTLS></BODY></REPORT_CHECK_REQUEST>";

                //string strRequest = "<?xml version=\"1.0\" encoding=\"UTF8\"?>" +
                //    "<REPORT_CHECK_REQUEST>" +
                //    "<HEADER>" +
                //    "<IP_ADDRESS>" + strJIAIPAddress + "</IP_ADDRESS>" +
                //    "</HEADER>" +
                //    "<BODY>" +
                //    "<REPORT_DTLS>" +
                //         "<REPORT_DTL>" +
                //             "<REPORT_NO>" + strCurCertNo + "</REPORT_NO>" +
                //              "<REPORT_WEIGHT>" + strCurWeight + "</REPORT_WEIGHT>" +
                //           "</REPORT_DTL>" +
                //    "</REPORT_DTLS>" +
                //    "</BODY>" +
                //    "</REPORT_CHECK_REQUEST>";
                string strRequest = "<?xml version=\"1.0\" encoding=\"UTF8\"?>" +
                    "<REPORT_CHECK_REQUEST>" +
                    "<HEADER>" +
                    "<IP_ADDRESS>" + strJIAIPAddress + "</IP_ADDRESS>" +
                    "</HEADER>" +
                    "<BODY>" +
                    "<REPORT_DTLS>" +
                         "<REPORT_DTL>" +
                             "<REPORT_NO>" + strCurCertNo + "</REPORT_NO>" +
                              "<REPORT_WEIGHT>"+ strCurWeight + "</REPORT_WEIGHT>" +
                           "</REPORT_DTL>" +
                    "</REPORT_DTLS>" +
                    "</BODY>" +
                    "</REPORT_CHECK_REQUEST>";

                pdfbyte = objGIANewService.downloadPDFReport(strRequest);


                //string strResult = objGIANewService.processRequest(strRequest);

                //#region Load Xml in dataset

                //System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);

                //using (DataSet dsFinal = new DataSet())
                //{

                //    dsFinal.ReadXml(xmlSR, XmlReadMode.Auto);

                //    if (Convert.ToString(dsFinal.Tables["Report_CHECK_RESPONSE"].Rows[0]["STATUS"]) == "SUCCESS" && dsFinal.Tables["REPORT_DTL"].Rows.Count > 0 && dsFinal.Tables["REPORT_DTL"].Rows[0]["MESSAGE"].ToString().Trim() == "")
                //    {
                //        //dsFinal.Tables["REPORT_DTL"].TableName = "GIACertificate";
                //        dsReturn.Merge(dsFinal.Tables["REPORT_DTL"]);
                //        dsReturn.Tables["REPORT_DTL"].TableName = "GIACertificate";

                //        dsReturn.Tables[0].Columns.Remove("REPORT_DTLS_Id");
                //        dsReturn.Tables[0].Columns.Add("REPORT_DTLS_Id");

                //        //                    REPORT_DTLS_Id

                //        //dsReturn.Tables.Add(GetStatus(0, "Successfull"));

                //        string strIsPDF = Convert.ToString(dsReturn.Tables["GIACertificate"].Rows[0]["IS_PDF_AVAILABLE"]);
                //        if (strIsPDF == "TRUE")
                //        {
                //           // string strRequest1 = "<?xml version=\"1.0\" encoding=\"UTF8\"?><REPORT_CHECK_REQUEST><HEADER><IP_ADDRESS>" + strJIAIPAddress + "</IP_ADDRESS></HEADER><BODY><REPORT_DTLS><REPORT_DTL><REPORT_NO>" + strCurCertNo + "</REPORT_NO></REPORT_DTL></REPORT_DTLS></BODY></REPORT_CHECK_REQUEST>";

                //            pdfbyte = objGIANewService.downloadPDFReport(strRequest);
                            
                //        }
                //    }
                //    else
                //    {
                //       // dsReturn.Tables.Add(GetStatus(1, "Certificate not found."));
                //    }
                //}

               // #endregion

            }
            catch (Exception ex)
            {

                //dsReturn.Tables.Add(GetStatus(99, ex.Message));
            }
            finally
            {

                //objGIANewService = null;
            }
            return pdfbyte;

        }
    }


    //public class GlobalExceptionHandler: System.Web.Http.ExceptionHandling.ExceptionHandler
    //{  
    //    public async override TaskHandleAsync(ExceptionHandlerContext context, System.Threading.CancellationToken cancellationToken)
    //{
    //    // Access Exception using context.Exception;  
    //    const string errorMessage = "An unexpected error occured";
    //    var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
    //        new
    //        {
    //            Message = errorMessage
    //        });
    //    response.Headers.Add("X-Error", errorMessage);
    //    context.Result = new ResponseMessageResult(response);
    //}
     //}  
}
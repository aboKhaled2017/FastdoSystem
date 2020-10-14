using ExcelDataReader;
using Fastdo.backendsys.Controllers.Stocks;
using Fastdo.backendsys.Controllers.Stocks.Models;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public class StkDrgsReportFromExcelResponse
    {
        public bool Status { get; set; } = false;
        public string ErrorMess { get; set; }
        public IEnumerable<StkDrug> StkDrugsReport { get; set; }

    }
    public class StkDrugsReportFromExcelService
    {
        private  IHostingEnvironment _env { get; }
        private IExcelDataReader reader { get; set; }
        private StkDrgsReportFromExcelResponse _ServiceResponse { get; }
        public StkDrugsReportFromExcelService(IHostingEnvironment env)
        {
            _env = env;
            _ServiceResponse = new StkDrgsReportFromExcelResponse { };
        }
        public StkDrgsReportFromExcelResponse ProcessFileAndGetReport(
            object Id, 
            List<DiscountPerStkDrug> currentDrgs,
            StockDrugsReporstModel model)
        {
            if (model.Sheet == null)
                return _ServiceResponse;
            string ext = Path.GetExtension(model.Sheet.FileName);          
            try
            { //delete old image if exists
                var file = model.Sheet.OpenReadStream();
                if (file.Length > 0)
                {
                    var filePath = Variables.ExcelPaths.StockDrugsReportFilePath + $@"/{Id}{ext}";
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        model.Sheet.CopyTo(fs);
                        fs.Flush();

                    }                    
                        string path=filePath.Replace(RequestStaticServices.GetHostingEnv().WebRootPath, "")
                        .Replace(@"\", "/");
                    ReadFileReport(model,currentDrgs, filePath);
                    return _ServiceResponse;
                }               
            }
            catch(FormatException)
            {
                if (!reader.IsClosed) reader.Close();
                _ServiceResponse.ErrorMess = "تأكد بانك ادخلت ترتيب الاعمدة بشكل صحيح الاسم رقم1 والسعر رقم2 والخصم رقم3";
                return _ServiceResponse;
            }
            catch (IndexOutOfRangeException)
            {
                if (!reader.IsClosed) reader.Close();
                _ServiceResponse.ErrorMess = "يبدو انك ادخلت رقم عمود غير موجود,تأكد من ترتيب ارقام الاعمدة";
                return _ServiceResponse;
            }
            catch(NullReferenceException)
            {
                if (!reader.IsClosed) reader.Close();
                _ServiceResponse.ErrorMess ="من فضلك تأكد انه لايوجد اى حقل فارغ داخل الملف";
                return _ServiceResponse;
            }
            catch (Exception ex)
            {
                if (!reader.IsClosed) reader.Close();
                _ServiceResponse.ErrorMess = ex.Message;
                return _ServiceResponse;
            }
            _ServiceResponse.Status = true;
            return _ServiceResponse;
        }
        private void ReadFileReport(StockDrugsReporstModel model, List<DiscountPerStkDrug> currentDrgs, string filePath)
        {
            var items = new List<StkDrug>() { };
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (reader = ExcelReaderFactory.CreateReader(stream))
                {
                    if (reader.FieldCount<3)
                    {
                        reader.Close();
                        _ServiceResponse.ErrorMess = "عدد الحقول الرأسية لابد ان يكون عددها 3";
                        return;
                    }
                        while (reader.Read())
                    {
                        var price = Convert.ToDouble(reader.GetValue(model.ColPriceOrder));
                        var name = reader.GetValue(model.ColNameOrder).ToString().Trim().ToLower();
                        if (price < 1)
                        {
                            reader.Close();
                            _ServiceResponse.ErrorMess = "تأكد ان جميع خانات  حقل السعر مملؤة او ان قيمتها لا تساوى صفر";
                            return;
                        }
                        var oldModel = currentDrgs.FirstOrDefault(d => d.Name.Equals(name));
                        items.Add(new StkDrug
                        {
                            Id=oldModel?.Id??Guid.Empty,
                            Name = name,
                            Price =price,
                            Discount =DiscountClassifier<Guid>.GetNewDiscount(
                                oldModel==null?null:oldModel.DiscountStr,
                                model.ForClassId,
                                Convert.ToDouble(reader.GetValue(model.ColDiscountOrder)))
                        }); 
                    }
                    reader.Close();
                    _ServiceResponse.StkDrugsReport = items;
                    _ServiceResponse.Status = true;
                }
            }
        }
    }
}

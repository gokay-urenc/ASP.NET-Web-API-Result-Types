using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API_2_Result_Types.Models;
using Web_API_2_Result_Types.Models.DTO;

namespace Web_API_2_Result_Types.Controllers
{
    public class DefaultController : ApiController
    {
        NORTHWND db = new NORTHWND();
        public HttpResponseMessage GetCategories()
        {
            // Veritabanında ilişkili tablolar olduğundan serilize işlemi yapmak için KategoriDTO tipinde bir class oluşturduk.
            // Kategori tablosunun ilişkisini bu sayede ayırmış olduk.
            List<KategoriDTO> kategorilerim = db.Categories.Select(x => new KategoriDTO
            {
                kategoriID = x.CategoryID,
                adi = x.CategoryName,
                aciklama = x.Description
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, db.Categories.ToList());
        }

        // Web servis istekleri MVC'de olduğu gibi parametrelerini url(header) ve body(form tagı) olan kısımlardan yakalar.
        // FromUrl: Gelecek parametrenin sadece URL'den okunacağını belirtir.
        // FromBody: Gelecek parametrenin sadece body(sayfa içi veya form)'dan okunacağını belirtir.
        // Bunları belirtmezsek her yerden gelen parametreler kabul edilir.
        public HttpResponseMessage PostKategoriEkle(Category parametre)
        {
            try
            {
                db.Categories.Add(parametre);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, parametre);
            }
            catch (Exception error)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Girilen bilgiler yanlış. Kontrol ediniz.", error);
            }
        }
    }
}
// Result Type: Status Code ile beraber veri gönderebileceğimiz tiplerdir.
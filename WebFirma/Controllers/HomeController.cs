using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using WebFirma.Models;

namespace WebFirma.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //using (DataContext db = new DataContext())
            //{
            //    // создаем два объекта User
            //    //User user1 = new User {Name = "Tom", Age = 33, OtdelId = 1, IspytId = 1, KvalId = 1, Date_Start = DateTime.Now};
            //    //User user2 = new User { Name = "Djon", Age = 22, OtdelId = 2, IspytId = 3, KvalId = 1, Date_Start = DateTime.Now };

            //    //// добавляем их в бд
            //    //db.Users.Add(user1);
            //    //db.Users.Add(user2);
            //    //db.SaveChanges();

            //    Ispyt i1 = new Ispyt { Name = 1 };
            //    Ispyt i2 = new Ispyt { Name = 2 };
            //    Ispyt i3 = new Ispyt { Name = 3 };

            //    // добавляем их в бд
            //    db.Ispyts.Add(i1);
            //    db.Ispyts.Add(i2);
            //    db.Ispyts.Add(i3);
            //    db.SaveChanges();

            //    Kval k1 = new Kval { Name = "практикант" };
            //    Kval k2 = new Kval { Name = "инженер" };
            //    Kval k3 = new Kval { Name = "ведущий инженер" };

            //    // добавляем их в бд
            //    db.Kvals.Add(k1);
            //    db.Kvals.Add(k2);
            //    db.Kvals.Add(k3);
            //    db.SaveChanges();

            //    Otdel o1 = new Otdel { Name = "отдел продаж" };
            //    Otdel o2 = new Otdel { Name = "производственный отдел" };
            //    Otdel o3 = new Otdel { Name = "отдел доставки" };
            //    Otdel o4 = new Otdel { Name = "отдел кадров" };

            //    // добавляем их в бд
            //    db.Otdels.Add(o1);
            //    db.Otdels.Add(o2);
            //    db.Otdels.Add(o3);
            //    db.Otdels.Add(o4);
            //    db.SaveChanges();
            //}
           
            return View();
        }
    }
}

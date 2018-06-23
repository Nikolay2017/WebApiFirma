using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebFirma.Models;

namespace WebFirma.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Users вывод всех сотрудников
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            //обновление флага user.fl если сотрудник прошел испытательный срок
            db.Database.SqlQuery<User>("Procedure_update_fl").ToList();
            var d = db.Users.Include(p => p.Ispyt).Include(p => p.Kval).Include(p => p.Otdel);
            return d;
        }
        // GET: api/users/otdels
        [HttpGet()]
        [Route("otdels")]
        public IEnumerable<Otdel> GetOtdels()
        {
            // Формируем список подразделегий
            IEnumerable<Otdel> list = db.Otdels;
            return list;
        }
        // GET: api/users/ispyts
        [HttpGet()]
        [Route("ispyts")]
        public IEnumerable<Ispyt> GetIspyts()
        {
            // Формируем список испытательных сроков
            IEnumerable<Ispyt> list = db.Ispyts;
            return list;
        }
        // GET: api/users/kvals
        [HttpGet()]
        [Route("kvals")]
        public IEnumerable<Kval> GetKvals()
        {
            // Формируем список квалификаций
            IEnumerable<Kval> list = db.Kvals;
            return list;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5  изменение сотрудника
        [ResponseType(typeof(User))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (user.Name == "" || user.Age == 0)
            {
                ModelState.AddModelError("NonIspyt", "Не указаны данные для пользователя");
                return BadRequest(ModelState);
            }
            //количество мест в отделе
            var count_sotrud = db.Otdels.FirstOrDefault(u => u.Id == user.OtdelId).Count;
            //количество сотрудников в отделе без учета текущего сотрудника
            var count_user = db.Users.Count(u => u.OtdelId == user.OtdelId && u.Id != user.Id);
            if (count_sotrud <= count_user)
            {
                ModelState.AddModelError("NonOtdel", "Для перевода сотрудника в этот отдел необходимо подтверждение директора!");
            }

            if (!user.fl && user.KvalId > 2)
            {
                ModelState.AddModelError("NonKval", "Новому сотруднику не может быть назначен уровень выше 2");
            }

            if (!ModelState.IsValid)return BadRequest(ModelState);

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //история сотрудника
            action_users(user,"update");

            User d = db.Users.Include(p => p.Ispyt).Include(p => p.Kval).Include(p => p.Otdel).FirstOrDefault(u => u.Id == user.Id); 
            return Ok(d);
        }

        

        // POST: api/Users добавление сотрудника
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (user.Name=="" || user.Age == 0)
            {
                ModelState.AddModelError("NonIspyt", "Не указаны данные для пользователя");
                return BadRequest(ModelState);
            }
            else
            {
                user.Date_Start = DateTime.Now;
            }
            if (!user.fl && user.KvalId > 2)
            {
                ModelState.AddModelError("NonKval", "Новому сотруднику не может быть назначен уровень выше 2");
            }

            var count_sotrud = db.Otdels.FirstOrDefault(u => u.Id == user.OtdelId).Count;
            var count_user = db.Users.Count(u => u.OtdelId == user.OtdelId);
            if (count_sotrud <= count_user)
            {
                ModelState.AddModelError("NonOtdel", "Для приема сотрудника свыше лимита необходимо подтверждение директора!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();
            //история сотрудника
            action_users(user, "insert");

            User d = db.Users.Include(p => p.Ispyt).Include(p => p.Kval).Include(p => p.Otdel).FirstOrDefault(u => u.Id == user.Id);
            return Ok(d);
        }

        // DELETE: api/Users/5  удаление сотрудника
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();
            //история сотрудника
            action_users(user, "delete");
            return Ok(user);
        }

        //поиск по фильтру
        // POST: api/Users/filtr
        [HttpPost]
        [Route("filtr")]
        public IEnumerable<User> FiltrUsers([FromBody] Filter s)
        {
            IEnumerable<User> list = null;
            switch (s.filtr)
            {
                case "ФИО":
                    list = db.Users.Include(p => p.Ispyt).Include(p => p.Kval).Include(p => p.Otdel).Where(p => p.Name.Contains(s.search));
                    break;
                case "подразделение":
                    list = db.Users.Include(p => p.Ispyt).Include(p => p.Kval).Include(p => p.Otdel).Where(p => p.Otdel.Name.Contains(s.search));
                    break;
                case "квалификация":
                    list = db.Users.Include(p => p.Ispyt).Include(p => p.Kval).Include(p => p.Otdel).Where(p => p.Kval.Name.Contains(s.search));
                    break;
            }
            return list;
        }

        //история сотрудника
        private void action_users(User user, string empty)
        {
            UsersHistory userHistory = new UsersHistory
            {
                ActionUsers = empty,
                Age = user.Age,
                DateActionUsers = DateTime.Now,
                Date_Start = user.Date_Start,
                IspytId = user.IspytId,
                KvalId = user.KvalId,
                Name = user.Name,
                OtdelId = user.OtdelId,
                UserId = user.Id,
                fl = user.fl
            };
            db.UsersHistory.Add(userHistory);
            db.SaveChanges();
        }


        int item_id = 0;
        private List<Kval> kvals = null;
        private List<Otdel> otdels = null;
        private UsersHistory insert_main = null;
        // POST: api/users/history
        [HttpPost()]
        [Route("history")]
        public UsersHistory History([FromBody] prop_next s)
        {
            var all = db.UsersHistory.Where(o => o.UserId == s.UserId);
            kvals = db.Kvals.ToList();
            otdels = db.Otdels.ToList();
            insert_main = all.FirstOrDefault(o => o.ActionUsers == "insert");
            var updates= all.Where(o => o.ActionUsers == "update").OrderBy(o=>o.DateActionUsers).ToList();
            List<UsersHistory> uh = new List<UsersHistory>();
            int count_edit= all.Count(o => o.ActionUsers == "insert" || o.ActionUsers == "update");

            for (int i = 1; i <= count_edit; i++)
            {
                update_all_user(uh, updates, insert_main);
            }
           
            return uh.Skip(s.CountProp).FirstOrDefault();
        }

        private void update_all_user(List<UsersHistory> uh, List<UsersHistory> updates, UsersHistory insert)
        {
            foreach (var item in updates)
            {
                if (insert != item)
                {
                    item_id = item.Id;
                    List<properties_fields> list_props = new List<properties_fields>();
                    if (insert.Name != item.Name) list_props.Add(new properties_fields { Name = "ФИО", OldValue = insert.Name, NewValue = item.Name });
                    if (insert.Age != item.Age) list_props.Add(new properties_fields { Name = "Возраст", OldValue = insert.Age.ToString(), NewValue = item.Age.ToString() });
                    if (insert.KvalId != item.KvalId) list_props.Add(new properties_fields { Name = "Квалификация", OldValue = kvals.FirstOrDefault(o => o.Id == insert.KvalId).Name, NewValue = kvals.FirstOrDefault(o => o.Id == item.KvalId).Name });
                    if (insert.OtdelId != item.OtdelId) list_props.Add(new properties_fields { Name = "Подразделение", OldValue = otdels.FirstOrDefault(o => o.Id == insert.OtdelId).Name, NewValue = otdels.FirstOrDefault(o => o.Id == item.OtdelId).Name });
                    item.usersHistory = list_props;
                    uh.Add(item);
                    updates.Remove(insert);
                    break;
                }
            }
            insert_main = updates.FirstOrDefault(o => o.Id == item_id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}
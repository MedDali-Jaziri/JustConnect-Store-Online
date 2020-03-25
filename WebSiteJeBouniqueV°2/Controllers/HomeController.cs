using WebSiteJeBouniqueV_2.DAL;
using WebSiteJeBouniqueV_2.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSiteJeBouniqueV_2.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntities1 ctx = new dbMyOnlineShoppingEntities1();

        public ActionResult Index(string search, int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, 4, page));
        }

        public ActionResult AddToCart(int productId)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Tbl_Product.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var count = cart.Count();
                var product = ctx.Tbl_Product.Find(productId);
                for (int i = 0; i < count; i++)
                {
                    if (cart[i].Product.ProductId == productId)
                    {
                        int prevQty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Product.ProductId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                        }
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("index");
        }

        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult CheckoutDetails()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Tbl_User account)
        {
            if (ModelState.IsValid)
            {
                using (ctx)
                {
                    ctx.Tbl_User.Add(account);
                    ctx.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.UserName + " " + account.Email + " sucessfully registred";
                return RedirectToAction("login");
            }
            return View();
        }


        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(Tbl_User user)
        {
            using (ctx)
            {
                var usr = ctx.Tbl_User.Single(u => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password));

                //String nomAdmin = usr.UserName.ToString();
                //String passAdmin = usr.Password.ToString();
                //if (nomAdmin.Equals("Admin") && passAdmin.Equals("123"))
                //{
                //    return RedirectToAction("/Admin/Dashboard");
                //}
                if (usr != null)
                {
                    String nomAdmin = usr.UserName.ToString();
                    String passAdmin = usr.Password.ToString();

                    Session["UserID"] = nomAdmin;
                    Session["Username"] = passAdmin;
                    if (nomAdmin.Equals("Mohamed Ali") && passAdmin.Equals("1234"))
                    {
                        return RedirectToAction("Dashboard","Admin");
                    }
                        return RedirectToAction("Checkout");
                }
                else
                {
                    return RedirectToAction("Erreur");
                    ModelState.AddModelError("", "Username or password is wrong");
                }
            }
            return View();
        }

        public ActionResult Erreur()
        {
            return View();
        }

        public ActionResult loggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Checkout");
        }

        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Index");
        }

        public ActionResult Paiment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Paiment(Tbl_OrderP order)
        {
            if (ModelState.IsValid)
            {
                using (ctx)
                {
                    ctx.Tbl_OrderP.Add(order);
                    ctx.SaveChanges();
                    //return RedirectToAction("/Home/index");
                }
                ModelState.Clear();
                //ViewBag.Message = account.UserName + " " + account.Email + " sucessfully registred";
                return RedirectToAction("Done");
            }
            return View();
        }

        public ActionResult Done()
        {
            return View();
        }
    }
}
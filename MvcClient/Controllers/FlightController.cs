using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using Newtonsoft.Json;

namespace MvcClient.Controllers
{
    public class FlightController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var lst = new List<Flights>();
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:62307/api/Flight/");
                HttpResponseMessage res = await httpclient.GetAsync("GetAllFlights");
                if(res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    lst = JsonConvert.DeserializeObject<List<Flights>>(result);
                }
            }
           return View(lst);
        }
        public async Task<IActionResult> Details(int id)
        {
            Flights flight=new Flights();
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:62307/");
              
                HttpResponseMessage res = await httpclient.GetAsync("api/Flight/GetFlightById?id="+id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    flight = JsonConvert.DeserializeObject<Flights>(result);
                }
            }
            return View(flight);
        }

       public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Flights flight)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:62307/");
                var postData = httpclient.PostAsJsonAsync<Flights>("/api/Flight/AddFlight",flight);
                var res = postData.Result;
                if(res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(flight);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Flights flight = new Flights();
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:62307/");
                HttpResponseMessage res = await httpclient.GetAsync("api/Flight/GetFlightById?id=" + id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    flight = JsonConvert.DeserializeObject<Flights>(result);
                }
            }
            return View(flight);
        }

        [HttpPost]
        public IActionResult Edit(Flights flight)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:62307/");
                var postdata = httpclient.PutAsJsonAsync("/api/Flight/UpdateFlight?id=" + flight.FlightId,flight);
                var res = postdata.Result;
                if(res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(flight);
        }

        
        public IActionResult Delete(int id)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:62307/");
                var delete = httpclient.DeleteAsync("/api/Flight/DeleteFlight?id=" + id);
                var res = delete.Result;
                return RedirectToAction("Index");
            }
        }
    }
}
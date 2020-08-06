using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArcadisWeb.Models;
using ArcadisWebSite;
using ClosedXML.Excel;
using System.IO;
using System.Net.Http;

namespace ArcadisWebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly EquipmentDBContext _context;

        public EquipmentsController(EquipmentDBContext context)
        {
            _context = context;
        }

        // GET: api/Equipments
        [HttpGet]
        public IEnumerable<Equipment> GetEquipments()
        {
            return _context.Equipments;
        }

        // GET: api/Equipments/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetEquipment([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var equipment = await _context.Equipments.FindAsync(id);

        //    if (equipment == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(equipment);
        //}
        // GET: api/Equipments/5
        [HttpGet("{title}")]
        public async Task<IActionResult> GetEquipment([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = await _context.Equipments.Where(n => n.Title.Contains(title)).ToListAsync();

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }


        [HttpGet("GetExcel/{title}")]
        public ActionResult GetEquipmentreport([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IEnumerable<Equipment> equipment; 
            if (String.IsNullOrEmpty(title) || title == "undefined")
             equipment =    _context.Equipments.ToList();
            else
             equipment =  _context.Equipments.Where(n => n.Title.Contains(title)).ToList();

            if (equipment == null)
            {
                return NotFound();
            }
            using (var workbook = new XLWorkbook())
            {
                Stream fs = new MemoryStream();
                workbook.AddWorksheet("Report");
                var ws = workbook.Worksheet("Report");
                ws.Cell("A4").Value = "Title";
                ws.Cell("B4").Value = "Cost";
                ws.Cell("C4").Value = "Quantity";
                ws.Cell("D4").Value = "Total Cost";

                int row = 5;
                int grandtotal = 0;
                foreach (var c in equipment)
                {

                    ws.Cell("A" + row.ToString()).Value = c.Title;
                    ws.Cell("B" + row.ToString()).Value = c.Cost;
                    ws.Cell("C" + row.ToString()).Value = c.Quantity;
                    ws.Cell("D" + row.ToString()).Value = c.Cost * c.Quantity;
                    grandtotal = grandtotal + (c.Cost * c.Quantity);


                    row++;

                }
                ws.Cell("A" + row.ToString()).Value = "GrandTotal";
               
                ws.Cell("D" + row.ToString()).Value = grandtotal;

                //ws.Range(ws.Cell("A1"), ws.Cell("E1")).Merge();
                var headersTable = ws.Range(ws.Cell(4, 1), ws.Cell(row, 4)).CreateTable("Headers");
                headersTable.Theme = XLTableTheme.TableStyleMedium2;
                headersTable.ShowAutoFilter = false;
                ws.Columns("A", "D").AdjustToContents();
                workbook.SaveAs(fs);
                fs.Position = 0;
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

                response.Content = new StreamContent(fs);

                return new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "Report" + DateTime.Now.ToString() + ".xlsx" };
                //return Ok(equipment);
            }
        }

        // PUT: api/Equipments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment([FromRoute] int id, [FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equipment.EquipmentId)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Equipments
        [HttpPost]
        public async Task<IActionResult> PostEquipment([FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipment", new { id = equipment.EquipmentId }, equipment);
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();

            return Ok(equipment);
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.EquipmentId == id);
        }
    }
}
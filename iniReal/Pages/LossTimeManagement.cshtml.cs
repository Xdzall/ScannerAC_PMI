using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LossTimeComponent.Services; 
using LossTimeComponent.Models;  

namespace iniReal.Pages
{
    [IgnoreAntiforgeryToken]
    public class LossTimeManagementModel : PageModel
    {
        private readonly LossTimeService _lossTimeService;

        public List<UnassignedLossEvent> LossEvents { get; set; }

        public LossTimeManagementModel(LossTimeService lossTimeService)
        {
            _lossTimeService = lossTimeService;
        }

        public async Task OnGetAsync()
        {
            // Ambil semua data losstime yang belum punya reason
            LossEvents = await _lossTimeService.GetUnassignedLossEventsAsync();
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostAssignReasonAsync()
        {
            if (!int.TryParse(Request.Form["lossTimeId"], out var lossTimeId))
            {
                return BadRequest("lossTimeId tidak ditemukan atau tidak valid.");
            }

            string reason = Request.Form["reason"];
            string detailedReason = Request.Form["detailedReason"];

            if (string.IsNullOrEmpty(reason))
            {
                return BadRequest("Reason tidak boleh kosong.");
            }

            await _lossTimeService.AssignLossTimeReasonAsync(lossTimeId, reason, detailedReason);

            return RedirectToPage();
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!int.TryParse(Request.Form["lossTimeId"], out var lossTimeId))
            {
                return BadRequest("lossTimeId tidak ditemukan atau tidak valid.");
            }

            await _lossTimeService.DeleteLossTimeAsync(lossTimeId);

            return RedirectToPage();
        }
    }
}
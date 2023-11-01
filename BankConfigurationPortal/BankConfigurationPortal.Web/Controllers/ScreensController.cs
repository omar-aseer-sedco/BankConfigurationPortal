using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BankConfigurationPortal.Web.Controllers {
    public class ScreensController : ApiController {
        private readonly IScreenData db;

        public ScreensController(IScreenData db) {
            this.db = db;
        }

        public IHttpActionResult Get(int branchId = 0, int counterId = 0) {
            try {
                var screen = db.GetActiveScreen(GetBankName());
                if (screen.ScreenId == 0) {
                    return NotFound();
                }

                IEnumerable<TicketingButton> buttons;

                if (branchId == 0 && counterId == 0) {
                    buttons = db.GetButtons(GetBankName(), screen.ScreenId);
                }
                else if (branchId != 0 && counterId != 0) {
                    buttons = db.GetButtons(GetBankName(), screen.ScreenId, branchId, counterId);
                }
                else {
                    return BadRequest("You must supply both the counter ID and the branch ID to filter buttons, or neither to get all buttons.");
                }

                ScreenModel ret = new ScreenModel() {
                    BankName = screen.BankName,
                    ScreenTitle = screen.ScreenTitle,
                    ButtonCount = buttons.Count(),
                    Buttons = new ButtonModel[buttons.Count()],
                };

                int i = 0;
                foreach (var button in buttons) {
                    int serviceId = 0;
                    string messageEn = string.Empty, messageAr = string.Empty;
                    if (button is IssueTicketButton issueTicketButton) {
                        serviceId = issueTicketButton.ServiceId;
                    }
                    if (button is ShowMessageButton showMessageButton) {
                        messageEn = showMessageButton.MessageEn;
                        messageAr = showMessageButton.MessageAr;
                    }

                    ret.Buttons[i] = new ButtonModel {
                        Type = button.Type,
                        NameEn = button.NameEn,
                        NameAr = button.NameAr,
                        ServiceId = serviceId,
                        MessageEn = messageEn,
                        MessageAr = messageAr
                    };
                    ++i;
                }

                return Ok(ret);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return InternalServerError();
            }
        }

        private string GetBankName() {
            return "bank1"; // TODO: fix this
        }

        private class ScreenModel {
            public string BankName { get; set; }
            public string ScreenTitle { get; set; }
            public int ButtonCount { get; set; }
            public ButtonModel[] Buttons { get; set; }
        }

        private class ButtonModel {
            public ButtonsConstants.Types Type { get; set; }
            public string NameEn { get; set; }
            public string NameAr { get; set; }
            public int ServiceId { get; set; }
            public string MessageEn { get; set; }
            public string MessageAr { get; set; }
        }
    }
}

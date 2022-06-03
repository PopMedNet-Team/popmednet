using Lpp.Dns.DTO;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class EditRequestMetadataDialog
    {
        private readonly IPage _page;
        private readonly IFrame _frame;
        private readonly string frameUrl = ConfigurationManager.AppSettings["baseUrl"] 
            + "workflow/workflowrequests/editwfrequestmetadatadialog";

        public EditRequestMetadataDialog(IPage page)
        {
            _page = page;
            _frame = _page.FrameByUrl(frameUrl);
        }

        public async Task ClickCancel()
        {
            await _frame.Locator("button text='Cancel'").ClickAsync();
        }

        public async Task<RequestDetailsPage> Cancel()
        {
            
            return new RequestDetailsPage(_page);
        }

        public async Task<RequestDetailsPage> Save()
        {
            await _frame.Locator("button:has-text('Save')").ClickAsync();
            return new RequestDetailsPage(_page);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="budgetSource">Should 'Budget=Source' checkbox be checked?</param>
        /// <returns></returns>
        public async Task FillRequestMetadata(RequestMetadataDTO data, bool budgetSource=false)
        {
            await _frame.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _frame.FillAsync("#Edit_Name", data.Name);
            
            await _frame.FillAsync("#Request-Due-Date", data.DueDate.ToString()); // May need to change to make it easier...
            
            // TODO: 
            // Description
            // Priority
            // PurposeOfUse
            // PhiDisclosureLevel
            // RequesterCenterID
            // ActivityID
            // ActivityProjectID
            // TaskOrderID
            // SourceActivityID
            // SourceActivityProjectID
            // TSourceTaskOrderID
            // WorkplanTypeID
            // MSRequestID
            // ReportAggregationLevelID
            // ApplyChangesToRoutings
        }


    }
}

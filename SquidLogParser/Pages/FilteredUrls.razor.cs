using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using SquidLogParser.Domain;
using SquidLogParser.Services;

namespace SquidLogParser.Pages
{
    public class FilteredUrlsBase: PageBase
    {
        [Inject]
        protected FilterUrlService FilterUrlService {get; set; }

        protected IEnumerable<FilteredUrlView> FilteredUrls;

        protected string Url;

        protected bool AddModal;
        protected bool DeleteModal;

        protected DeleteView DeleteView;

        protected bool HasModalError;

        protected string ModalErrorMessage;

        protected FilteredUrlView DataModel;

        protected override void OnInitialized()
        {
            Url = "";
            HideModalErrorMessage();
            HideAddModal();

            GetUrls();
        }

        protected void GetUrls()
        {
            HideErrorMessage();

            var result = FilterUrlService.GetUrls(Url);

            result.Match(right => {

                FilteredUrls = right;

            }, ShowErrorMessage);
        }

        protected void AddUrl()
        {
            var result = FilterUrlService.AddUrl(DataModel.Url);

            result.Match(Right => {

                HideModalErrorMessage();
                HideAddModal();
                GetUrls();
            }, ShowModalErrorMessage);
        }

        protected void DeleteUrl()
        {
            var result = FilterUrlService.RemoveUrl(DeleteView.Name);

            result.Match(Right => {

                HideDeleteModal();
                GetUrls();

            }, ShowModalErrorMessage);
        }

        protected void ShowAddModal()
        {
            DataModel = new FilteredUrlView();
            AddModal = true;
        }

        protected void HideAddModal()
        {
            AddModal = false;
        }

        protected void ShowDeleteModal()
        {
            DeleteModal = true;
        }

        protected void HideDeleteModal()
        {
            DeleteModal = false;
        }

        protected void HideModalErrorMessage()
        {
            HasModalError = false;
            ModalErrorMessage = "";
        }

        protected void ShowModalErrorMessage(string error)
        {
            HasModalError = true;
            ModalErrorMessage = error;
        }

        protected void GetDeleteInformation(long id, string name)
        {
            DeleteView = new DeleteView()
            {
                Id = id,
                Name = name
            };

            ShowDeleteModal();
        }

        // ---------------------------------------------------------------------------------


    }
}
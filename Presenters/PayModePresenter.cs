using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarker_mvp.Models;
using Supermarker_mvp.Views;

namespace Supermarker_mvp.Presenters
{
    internal class PayModePresenter
    {
        private IPayModeView view;
        private IPayModeRepository repository;
        private BindingSource payModeBindingSource;
        private IEnumerable<PayModeModel> payModeList;

        public PayModePresenter(IPayModeView view, IPayModeRepository repository)
        {
            this.payModeBindingSource = new BindingSource();
            this.view = view;
            this.repository = repository;

            this.view.SearchEvent += SearchPayMode;
            this.view.AddNewEvent += AddNewPayMode;
            this.view.EditedEvent += LoadSelectPayModeToEdit;
            this.view.DeleteEvent += DeleteSelectedPayMode;
            this.view.CancelEvent += CancelAction;
            this.view.SaveEvent += SavePayMode;

            this.view.setPayModeListBildingSource(payModeBindingSource);
            
            loadAllPayModeList();
            this.view.Show();

        }

        private void loadAllPayModeList()
        {
            payModeList=repository.GetAll();
            payModeBindingSource.DataSource= payModeList;
        }

        private void CancelAction(object? sender, EventArgs e)
        {
            CleanViewFields();
        }

        private void SavePayMode(object? sender, EventArgs e)
        {
            //Crea el objeto de la clase PayModeModel y asigna datos
            //de las cajas de texto vista
            var payMode = new PayModeModel();
            payMode.Id = Convert.ToInt32(view.PayModeId);
            payMode.Name = view.PayModeName;
            payMode.Observation = view.PayModeObservation;

            try
            {
                new Common.ModelDataValidation().Validacion(payMode);
                if (view.IsEdit)
                {
                    repository.Edit(payMode);
                    view.Message = "PayMode edited successfuly";
                }
                else
                {
                    repository.Add(payMode);
                    view.Message = "PayMode added successfuly";
                }
                view.IsSuccessful = true;
                loadAllPayModeList();
                CleanViewFields();
            }
            catch (Exception ex) 
            {
                view.IsSuccessful = false;
                view.Message = ex.Message;
            }
        }

        private void CleanViewFields()
        {
            view.PayModeId = "0";
            view.PayModeName = "";
            view.PayModeObservation = "";
        }

        private void DeleteSelectedPayMode(object? sender, EventArgs e)
        {
            try
            {
                var payMode = (PayModeModel)payModeBindingSource.Current; //Recupera el objeto de la fila seleccionada del dataviewwgrid

                repository.Delete(payMode.Id);
                view.IsSuccessful = true;
                view.Message = "Pay Mode Id delete successfully";
                loadAllPayModeList();
            }
            catch (Exception ex)
            {
                view.IsSuccessful = false;
                view.Message = "An error ocurred, could not delete pay mode";
            }
        }

        private void LoadSelectPayModeToEdit(object? sender, EventArgs e)
        {
            var payMode = (PayModeModel)payModeBindingSource.Current;

            view.PayModeId = payMode.Id.ToString(); // cambia el contenidode las cajas de texto
            view.PayModeName = payMode.Name;
            view.PayModeObservation = payMode.Observation;

            view.IsEdit = true; // establece el modo de edicion
        }

        private void AddNewPayMode(object? sender, EventArgs e)
        {
            MessageBox.Show("Hizo clic en el boton nuevo");
            view.IsEdit = false;
        }

        private void SearchPayMode(object? sender, EventArgs e)
        {
            bool emptyValue=string.IsNullOrWhiteSpace(this.view.SearchValue);
            if (emptyValue==false)
            {
                payModeList=repository.GetByValue(this.view.SearchValue);
            }
            else
            {
                payModeList= repository.GetAll();
            }
            payModeBindingSource.DataSource = payModeList;
        }
    }
}

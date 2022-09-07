using System;
using ReactiveUI;
using System.Windows.Input;
using FinBalance2.Views.Components;
using FinBalance2.Models;
using FinBalance2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Controls;
using System.ComponentModel;

namespace FinBalance2.ViewModels
{
    public enum BalancePeriodType
    {
        Day = 0,
        Week = 1,
        Month = 2,
        Year = 3,
        Period = 4
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private BalancePeriodType selectedBalancePeriodType;
        private IPeriodProperties currentPeriodPropertiesComponent;
        private IPeriodProperties[] loadedPeriodPropertiesComponents;
        private ObservableCollection<FinMovement> loadedMovements;
        private FinMovementsService databaseService;
        private decimal balanceValue;
        private bool isSavingData;
        private string statusMessage;
        private bool isLoadingData;

        public MainWindowViewModel()
        {
            loadedMovements = new();
            loadedMovements.CollectionChanged += LoadedMovements_CollectionChanged;
            databaseService = new();
            balanceValue = 0;
            isLoadingData = false;
            loadedPeriodPropertiesComponents = new IPeriodProperties[]
                {
                    new DayBalanceProperties(),
                    new WeekBalanceProperties(),
                    new MonthBalanceProperties(),
                    new YearBalanceProperties(),
                    new PeriodBalanceProperties()
                };

            currentPeriodPropertiesComponent = new DayBalanceProperties();
            ViewFinMovementCommand = ReactiveCommand.Create(ViewFinMovementButton_Pressed);
            SaveFinMovementCommand = ReactiveCommand.Create(SaveFinMovementButton_Pressed);
            CreateFinMovementCommand = ReactiveCommand.Create(CreateFinMovementButton_Pressed);
            DeleteFinMovementCommand = ReactiveCommand.Create<FinMovement>(DeleteFinMovementButton_Pressed);
            DeletedMovements = new();
            isSavingData = false;
            statusMessage = "";
            SelectedBalancePeriodType = BalancePeriodType.Day;
        }

        public IPeriodProperties CurrentPeriodPropertiesComponent 
        {
            get => currentPeriodPropertiesComponent;
            private set { this.RaiseAndSetIfChanged(ref currentPeriodPropertiesComponent, value); }
        }
        public decimal BalanceValue
        {
            get => balanceValue;
            set { this.RaiseAndSetIfChanged(ref balanceValue, value); }
        }
        public ICommand ViewFinMovementCommand { get; }
        public ICommand SaveFinMovementCommand { get; }
        public ICommand DeleteFinMovementCommand { get; }
        public ICommand CreateFinMovementCommand { get; }
        public List<FinMovement> DeletedMovements { get; private set; }
        public bool IsLoadingData
        {
            get => isLoadingData;
            set { this.RaiseAndSetIfChanged(ref isLoadingData, value); }
        }
        public bool IsSavingData 
        {
            get => isSavingData;
            private set { this.RaiseAndSetIfChanged(ref isSavingData, value); } 
        }
        public string StatusMessage 
        {
            get => statusMessage;
            private set { this.RaiseAndSetIfChanged(ref statusMessage, value); }
        }
        public BalancePeriodType SelectedBalancePeriodType
        {
            get => selectedBalancePeriodType;
            set 
            {
                this.RaiseAndSetIfChanged(ref selectedBalancePeriodType, value);
                setNewPeriodPropertiesComponent(value);
            }
        }

        internal ObservableCollection<FinMovement> LoadedMovements
        {
            get => loadedMovements;
            set { this.RaiseAndSetIfChanged(ref loadedMovements, value); }
        }

        private void ViewFinMovementButton_Pressed()
        {
            IsLoadingData = true;
            Task.Run( async () =>
            {
                var fromDatabase = await databaseService.GetFromPeriod(CurrentPeriodPropertiesComponent.BeginDate, CurrentPeriodPropertiesComponent.EndDate);

                foreach (FinMovement move in fromDatabase)
                    move.PropertyChanged += LoadedMovements_Item_ProperyChanged;

                this.LoadedMovements = new(fromDatabase);

                this.LoadedMovements.CollectionChanged += LoadedMovements_CollectionChanged;
                this.DeletedMovements = new();

                RefreshBalanceValueAndStatusMessage();

                IsLoadingData = false;
            });
        }

        private void LoadedMovements_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshBalanceValueAndStatusMessage();

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    ((ReactiveObject)item).PropertyChanged += LoadedMovements_Item_ProperyChanged;
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                    ((ReactiveObject)item).PropertyChanged -= LoadedMovements_Item_ProperyChanged;
            }
        }

        private void LoadedMovements_Item_ProperyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RefreshBalanceValueAndStatusMessage();
        }

        private void RefreshBalanceValueAndStatusMessage()
        {
            decimal resultBalance = 0;
            int moveCount = 0;
            foreach (FinMovement move in this.LoadedMovements)
            {
                resultBalance += move.Value;
                moveCount++;
            }

            this.BalanceValue = resultBalance;
            this.StatusMessage = moveCount >= 2 ? $"{moveCount} registros carregados" : $"{moveCount} registro carregado";
        }

        private void SaveFinMovementButton_Pressed()
        {
            IsSavingData = true;
            Task.Run(async () =>
           {
               int affectedRows = await databaseService.SaveChanges(LoadedMovements, DeletedMovements);
               StatusMessage = affectedRows > 0 ? "Registros salvos com sucesso!" : "Nenhum dado alterado.";
               IsSavingData = false;
           });
        }

        private void CreateFinMovementButton_Pressed()
        {
            LoadedMovements.Add(new FinMovement()
            {
                Id = null,
                Name = "Novo registro",
                Date = DateTime.Today,
                Value = 0,
                Notes = ""
            });
        }
        private void setNewPeriodPropertiesComponent(BalancePeriodType balancePeriodType)
        {
            CurrentPeriodPropertiesComponent = loadedPeriodPropertiesComponents[(int)balancePeriodType];
        }

        private void DeleteFinMovementButton_Pressed(FinMovement moveToDelete)
        {
            loadedMovements.Remove(moveToDelete);
            DeletedMovements.Add(moveToDelete);
        }

    }
}

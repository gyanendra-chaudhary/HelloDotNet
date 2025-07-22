using SignalRWithSqlTableDependency.Hubs;
using SignalRWithSqlTableDependency.Models;
using TableDependency.SqlClient;

namespace SignalRWithSqlTableDependency.SubscribeTableDependencies
{
    public class SubscribeSaleTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Sale> _tableDependency;
        DashboardHub _dashboardHub;
        public SubscribeSaleTableDependency(DashboardHub dashboardHub)
        {
            _dashboardHub = dashboardHub;
        }
        public void SubscribeTableDependency(string connectionString)
        {
            _tableDependency = new SqlTableDependency<Sale>(connectionString);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }
        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Sale> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                _dashboardHub.SendSales();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Sale)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}

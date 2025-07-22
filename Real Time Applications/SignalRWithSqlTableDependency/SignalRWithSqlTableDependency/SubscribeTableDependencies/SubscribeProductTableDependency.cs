using SignalRWithSqlTableDependency.Hubs;
using SignalRWithSqlTableDependency.Models;
using TableDependency.SqlClient;

namespace SignalRWithSqlTableDependency.SubscribeTableDependencies
{
    public class SubscribeProductTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Product> _tableDependency;
        DashboardHub _dashboardHub;
        public SubscribeProductTableDependency(DashboardHub dashboardHub)
        {
            _dashboardHub = dashboardHub;
        }
        public void SubscribeTableDependency(string connectionString)
        {
            _tableDependency = new SqlTableDependency<Product>(connectionString);
            _tableDependency.OnChanged += TableDependency_OnChanged;    
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }
        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Product> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                _dashboardHub.SendProducts();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Product)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}

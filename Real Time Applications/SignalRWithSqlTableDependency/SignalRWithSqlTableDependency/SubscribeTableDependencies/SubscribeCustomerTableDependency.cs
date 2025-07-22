using SignalRWithSqlTableDependency.Hubs;
using SignalRWithSqlTableDependency.Models;
using SignalRWithSqlTableDependency.Repositories;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
namespace SignalRWithSqlTableDependency.SubscribeTableDependencies
{
    public class SubscribeCustomerTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Customer> _tableDependency;
        DashboardHub _dashboardhub;

        public SubscribeCustomerTableDependency(DashboardHub dashboardHub)
        {
            _dashboardhub = dashboardHub;
        }
        public void SubscribeTableDependency(string connectionString)
        {
            _tableDependency = new SqlTableDependency<Customer>(connectionString);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }
        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Customer> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                _dashboardhub.SendCustomers();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Product)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}

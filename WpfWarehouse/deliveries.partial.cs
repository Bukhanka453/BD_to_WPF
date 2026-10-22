namespace WpfWarehouse
{
    public partial class deliveries
    {
        public decimal total_cost
        {
            get
            {
                return quantity * cost;
            }
        }
    }
}